﻿Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Runtime.CompilerServices
Imports Enigma
Imports [Module] = Enigma.LLVM.[Module]
Imports Type = Enigma.LLVM.Type

Namespace MSIL

    Friend Module Translator

        Public Function Translate(context As LLVM.Context, ParamArray methods As System.Reflection.MethodInfo()) As LLVM.[Module]
            Dim [module] = New LLVM.[Module]("Module", context)

            If System.Environment.Is64BitOperatingSystem Then
                [module].SetTarget("nvptx64-nvidia-cuda")
                [module].SetDataLayout("e-p:64:64:64-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-v16:16:16-v32:32:32-v64:64:64-v128:128:128-n16:32:64")
            Else
                [module].SetTarget("nvptx-nvidia-cuda")
                [module].SetDataLayout("e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-v16:16:16-v32:32:32-v64:64:64-v128:128:128-n16:32:64")
            End If

            For Each method In methods
                Call Translator.Translate([module], method)
            Next

            Return [module]
        End Function

        Private Sub Translate([module] As LLVM.[Module], method As System.Reflection.MethodBase)
            Dim [function] = Translator.EmitFunction([module], method)
            If method.IsStatic = False Then Throw New CudaException("Cannot translate instance methods to GPU code")

            Dim metadataArgs = {[function], PInvoke.LLVMMDStringInContext([module].Context, "kernel"), LLVM.IntegerType.GetInt32(CType(([module].Context), LLVM.Context)).Constant(1, True)}
            Dim metadata = [module].Context.MetadataNodeInContext(metadataArgs)

            [module].AddNamedMetadataOperand("nvvm.annotations", metadata)
        End Sub

        Private Function EmitFunction([module] As LLVM.[Module], method As System.Reflection.MethodBase) As LLVM.[Function]
            Dim methodInfo = TryCast(method, System.Reflection.MethodInfo)
            Dim methodConstructor = TryCast(method, System.Reflection.ConstructorInfo)
            Dim declaringType = method.DeclaringType
            If methodInfo Is Nothing AndAlso methodConstructor Is Nothing Then Throw New CudaException("Unknown MethodBase type " & method.[GetType]().FullName)
            If declaringType Is Nothing Then Throw New CudaException("Could not find the declaring type of " & method.Name.StripNameToValidPtx())

            Dim parameters = method.GetParameters().[Select](Function(p) p.ParameterType)
            If methodConstructor IsNot Nothing Then parameters = {declaringType.MakeByRefType()}.Concat(parameters)
            If methodInfo IsNot Nothing AndAlso methodInfo.IsStatic = False Then
                If declaringType.IsValueType = False Then Throw New CudaException("Cannot compile object instance methods (did you forget to mark the method as static?)")
                parameters = {declaringType.MakeByRefType()}.Concat(parameters)
            End If
            Dim llvmParameters = parameters.[Select](Function(t) Translator.ConvertType([module], t)).ToArray()
            Dim funcType = New LLVM.FunctionType(Translator.ConvertType([module], If(methodInfo Is Nothing, GetType(Void), methodInfo.ReturnType)), llvmParameters)
            Dim intrinsic = method.GetCustomAttribute(Of Gpu.BuiltinAttribute)()
            If intrinsic IsNot Nothing Then
                Dim name = intrinsic.Intrinsic
                Dim preExisting = [module].GetFunction(name)
                If preExisting IsNot Nothing Then Return preExisting
                Return [module].CreateFunction(name, funcType)
            End If

            Dim [function] = [module].CreateFunction(If(methodConstructor Is Nothing, method.Name.StripNameToValidPtx(), declaringType.Name.StripNameToValidPtx() & "_ctor"), funcType)

            Dim block = New LLVM.Block("entry", [module].Context, [function])
            Dim writer = New LLVM.InstructionBuilder([module].Context, block)

            Dim opcodes = method.Disassemble().ToList()
            Call Translator.FindBranchTargets(opcodes, [module].Context, [function])

            Dim body = method.GetMethodBody()
            Dim efo = New EmitFuncObj([module], [function], body, writer, Nothing, New Stack(Of LLVM.Value)(), If(body Is Nothing, Nothing, New LLVM.Value(body.LocalVariables.Count - 1) {}), New LLVM.Value(llvmParameters.Length - 1) {})

            Call Translator.PrintHeader(efo)
            For Each opcode In opcodes
                If Translator.EmitFunctions.ContainsKey(opcode.Opcode) = False Then
                    Throw New CudaException("Unsupported CIL instruction " & opcode.Opcode.ToString)
                End If
                Dim func = Translator.EmitFunctions(opcode.Opcode)
                efo.Argument = opcode.Parameter
                func(efo)
            Next

            Return [function]
        End Function

        Private Sub PrintHeader(__ As EmitFuncObj)
            For index = 0 To __.Parameters.Length - 1
                __.Parameters(index) = __.Builder.StackAlloc(__.[Function](CInt((index))).Type)
                __.Builder.Store(__.[Function](index), __.Parameters(index))
            Next
            For index = 0 To __.Locals.Length - 1
                __.Locals(index) = __.Builder.StackAlloc(Translator.ConvertType(__.[Module], __.CilMethod.LocalVariables(CInt((index))).LocalType))
            Next
        End Sub

        Private Sub FindBranchTargets(opCodes As IList(Of OpCodeInstruction), context As LLVM.Context, [function] As LLVM.[Function])
            For i = 0 To opCodes.Count - 1
                Dim op = opCodes(i)
                Dim opcode = op.Opcode
                Select Case opcode.FlowControl
                    Case FlowControl.Branch, FlowControl.Cond_Branch
                    Case Else
                        Continue For
                End Select

                Dim target = System.Convert.ToInt32(op.Parameter)
                target += CInt(opCodes(CInt((i + 1))).InstructionStart)

                Dim insert = 0
                While opCodes(CInt((insert))).InstructionStart <> target
                    insert += 1
                End While

                Dim contBlock = If(opcode.FlowControl = FlowControl.Cond_Branch, New LLVM.Block("", context, [function]), Nothing)
                Dim block As LLVM.Block
                If opCodes(CInt((insert))).Opcode = Emit.OpCodes.Nop AndAlso opCodes(CInt((insert))).Parameter IsNot Nothing Then
                    block = CType(opCodes(CInt((insert))).Parameter, LLVM.Block)
                Else
                    block = New LLVM.Block("", context, [function])
                    opCodes.Insert(insert, New OpCodeInstruction(target, Emit.OpCodes.Nop, block))
                    If insert < i Then i += 1
                End If
                opCodes(i) = New OpCodeInstruction(op.InstructionStart, op.Opcode, If(contBlock Is Nothing, CObj(block), System.Tuple.Create(contBlock, block)))
            Next
        End Sub

        Private Function ConvertType([module] As LLVM.[Module], type As System.Type) As LLVM.Type
            If (type Is Nothing OrElse type Is GetType(Void)) Then
                Return LLVM.Type.GetVoid([module].Context)
            End If

            If type Is GetType(Boolean) Then Return LLVM.IntegerType.[Get]([module].Context, 1)
            If type Is GetType(Byte) Then Return LLVM.IntegerType.[Get]([module].Context, 8)
            If type Is GetType(Short) Then Return LLVM.IntegerType.[Get]([module].Context, 16)
            If type Is GetType(Integer) Then Return LLVM.IntegerType.GetInt32([module].Context)
            If type Is GetType(Long) Then Return LLVM.IntegerType.[Get]([module].Context, 64)
            If type Is GetType(Single) Then Return LLVM.FloatType.[Get]([module].Context, 32)
            If type Is GetType(Double) Then Return LLVM.FloatType.[Get]([module].Context, 64)
            If type.IsArray Then Return LLVM.PointerType.[Get](Translator.ConvertType([module], type.GetElementType()), 1)
            If type.IsByRef Then Return LLVM.PointerType.[Get](Translator.ConvertType([module], type.GetElementType()))
            If type.IsValueType Then
                Dim name = type.Name.StripNameToValidPtx()
                Dim preExisting = [module].GetTypeByName(name)
                If preExisting IsNot Nothing Then Return preExisting
                Return New LLVM.StructType([module].Context, name, Translator.AllFields(type).[Select](Function(t) Translator.ConvertType([module], t.FieldType)))
            End If

            Throw New CudaException("Type cannot be translated to CUDA: " & type.FullName)
        End Function

        Private Delegate Sub EmitFunc(arg As EmitFuncObj)

        Private ReadOnly EmitFunctions As New Dictionary(Of OpCode, Translator.EmitFunc) From {
{OpCodes.Nop, AddressOf Translator.Nop},
{OpCodes.Pop, Sub(__) __.Stack.Pop()},
{OpCodes.Dup, Sub(__) __.Stack.Push(__.Stack.Peek())},
{OpCodes.Ret, Sub(__)
                  If __.Stack.Count = 0 Then
                      __.Builder.[Return]()
                  Else
                      __.Builder.[Return](__.Stack.Pop())
                  End If

                  __.Builder = Nothing
              End Sub},
{OpCodes.Ldc_I4, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(CULng(System.Convert.ToInt64(__.Argument)), True))},
{OpCodes.Ldc_I4_S, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(CULng(System.Convert.ToInt64(__.Argument)), True))},
{OpCodes.Ldc_I4_0, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(0, True))},
{OpCodes.Ldc_I4_1, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(1, True))},
{OpCodes.Ldc_I4_2, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(2, True))},
{OpCodes.Ldc_I4_3, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(3, True))},
{OpCodes.Ldc_I4_4, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(4, True))},
{OpCodes.Ldc_I4_5, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(5, True))},
{OpCodes.Ldc_I4_6, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(6, True))},
{OpCodes.Ldc_I4_7, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(7, True))},
{OpCodes.Ldc_I4_8, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(8, True))},
{OpCodes.Ldc_I4_M1, Sub(__) __.Stack.Push(LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(ULong.MaxValue, True))},
{OpCodes.Ldc_I8, Sub(__) __.Stack.Push(LLVM.IntegerType.[Get](CType((__.Context), LLVM.Context), CInt((64))).Constant(CULng(System.Convert.ToInt64(__.Argument)), True))},
{OpCodes.Stelem, AddressOf Translator.StElem},
{OpCodes.Stelem_I, AddressOf Translator.StElem},
{OpCodes.Stelem_I1, AddressOf Translator.StElem},
{OpCodes.Stelem_I2, AddressOf Translator.StElem},
{OpCodes.Stelem_I4, AddressOf Translator.StElem},
{OpCodes.Stelem_I8, AddressOf Translator.StElem},
{OpCodes.Stelem_R4, AddressOf Translator.StElem},
{OpCodes.Stelem_R8, AddressOf Translator.StElem},
{OpCodes.Stelem_Ref, AddressOf Translator.StElem},
{OpCodes.Ldelem, AddressOf Translator.LdElem},
{OpCodes.Ldelem_I, AddressOf Translator.LdElem},
{OpCodes.Ldelem_I1, AddressOf Translator.LdElem},
{OpCodes.Ldelem_I2, AddressOf Translator.LdElem},
{OpCodes.Ldelem_I4, AddressOf Translator.LdElem},
{OpCodes.Ldelem_I8, AddressOf Translator.LdElem},
{OpCodes.Ldelem_R4, AddressOf Translator.LdElem},
{OpCodes.Ldelem_R8, AddressOf Translator.LdElem},
{OpCodes.Ldelem_Ref, AddressOf Translator.LdElem},
{OpCodes.Ldelem_U1, AddressOf Translator.LdElem},
{OpCodes.Ldelem_U2, AddressOf Translator.LdElem},
{OpCodes.Ldelem_U4, AddressOf Translator.LdElem},
{OpCodes.Ldelema, AddressOf Translator.LdElemA},
{OpCodes.Ldobj, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Stobj, Sub(__) __.Builder.Store(__.Stack.Pop(), __.Stack.Pop())},
{OpCodes.Ldind_I, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Ldind_I1, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Ldind_I2, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Ldind_I4, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Ldind_I8, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Ldind_R4, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Ldind_R8, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Ldind_U1, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Ldind_U2, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Ldind_U4, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Ldind_Ref, Sub(__) __.Stack.Push(__.Builder.Load(__.Stack.Pop()))},
{OpCodes.Stind_I, Sub(__) __.Builder.Store(__.Stack.Pop(), __.Stack.Pop())},
{OpCodes.Stind_I1, Sub(__) __.Builder.Store(__.Stack.Pop(), __.Stack.Pop())},
{OpCodes.Stind_I2, Sub(__) __.Builder.Store(__.Stack.Pop(), __.Stack.Pop())},
{OpCodes.Stind_I4, Sub(__) __.Builder.Store(__.Stack.Pop(), __.Stack.Pop())},
{OpCodes.Stind_I8, Sub(__) __.Builder.Store(__.Stack.Pop(), __.Stack.Pop())},
{OpCodes.Stind_R4, Sub(__) __.Builder.Store(__.Stack.Pop(), __.Stack.Pop())},
{OpCodes.Stind_R8, Sub(__) __.Builder.Store(__.Stack.Pop(), __.Stack.Pop())},
{OpCodes.Stind_Ref, Sub(__) __.Builder.Store(__.Stack.Pop(), __.Stack.Pop())},
{OpCodes.Conv_I1, Sub(__) Translator.ConvertNum(__, LLVM.IntegerType.[Get](__.Context, 8), True)},
{OpCodes.Conv_I2, Sub(__) Translator.ConvertNum(__, LLVM.IntegerType.[Get](__.Context, 16), True)},
{OpCodes.Conv_I4, Sub(__) Translator.ConvertNum(__, LLVM.IntegerType.[Get](__.Context, 32), True)},
{OpCodes.Conv_I8, Sub(__) Translator.ConvertNum(__, LLVM.IntegerType.[Get](__.Context, 64), True)},
{OpCodes.Conv_U1, Sub(__) Translator.ConvertNum(__, LLVM.IntegerType.[Get](__.Context, 8), False)},
{OpCodes.Conv_U2, Sub(__) Translator.ConvertNum(__, LLVM.IntegerType.[Get](__.Context, 16), False)},
{OpCodes.Conv_U4, Sub(__) Translator.ConvertNum(__, LLVM.IntegerType.[Get](__.Context, 32), False)},
{OpCodes.Conv_U8, Sub(__) Translator.ConvertNum(__, LLVM.IntegerType.[Get](__.Context, 64), False)},
{OpCodes.Conv_R4, Sub(__) Translator.ConvertNum(__, LLVM.FloatType.[Get](__.Context, 32), True)},
{OpCodes.Conv_R8, Sub(__) Translator.ConvertNum(__, LLVM.FloatType.[Get](__.Context, 64), True)},
{OpCodes.Neg, Sub(__) __.Stack.Push(__.Builder.Negate(__.Stack.Pop()))},
{OpCodes.[Not], Sub(__) __.Stack.Push(__.Builder.[Not](__.Stack.Pop()))},
{OpCodes.Add, Sub(__) __.Stack.Push(__.Builder.Add(__.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.[Sub], Sub(__) __.Stack.Push(__.Builder.Subtract(__.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.Mul, Sub(__) __.Stack.Push(__.Builder.Multiply(__.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.Div, Sub(__) __.Stack.Push(__.Builder.Divide(True, __.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.Div_Un, Sub(__) __.Stack.Push(__.Builder.Divide(False, __.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.[Rem], Sub(__) __.Stack.Push(__.Builder.Reminder(True, __.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.Rem_Un, Sub(__) __.Stack.Push(__.Builder.Reminder(False, __.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.[And], Sub(__) __.Stack.Push(__.Builder.[And](__.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.[Or], Sub(__) __.Stack.Push(__.Builder.[Or](__.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.[Xor], Sub(__) __.Stack.Push(__.Builder.[Xor](__.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.Shl, Sub(__) __.Stack.Push(__.Builder.ShiftLeft(__.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.Shr, Sub(__) __.Stack.Push(__.Builder.ShiftRight(True, __.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.Shr_Un, Sub(__) __.Stack.Push(__.Builder.ShiftRight(False, __.Stack.Pop(), __.Stack.Pop()))},
{OpCodes.Ceq, AddressOf Translator.Ceq},
{OpCodes.Cgt, AddressOf Translator.Cgt},
{OpCodes.Cgt_Un, AddressOf Translator.CgtUn},
{OpCodes.Clt, AddressOf Translator.Clt},
{OpCodes.Clt_Un, AddressOf Translator.CltUn},
{OpCodes.Ldloca, Sub(__) Translator.LdVarA(__, __.Locals, System.Convert.ToInt32(__.Argument))},
{OpCodes.Ldloca_S, Sub(__) Translator.LdVarA(__, __.Locals, System.Convert.ToInt32(__.Argument))},
{OpCodes.Ldloc, Sub(__) Translator.LdVar(__, __.Locals, System.Convert.ToInt32(__.Argument))},
{OpCodes.Ldloc_S, Sub(__) Translator.LdVar(__, __.Locals, System.Convert.ToInt32(__.Argument))},
{OpCodes.Ldloc_0, Sub(__) Translator.LdVar(__, __.Locals, 0)},
{OpCodes.Ldloc_1, Sub(__) Translator.LdVar(__, __.Locals, 1)},
{OpCodes.Ldloc_2, Sub(__) Translator.LdVar(__, __.Locals, 2)},
{OpCodes.Ldloc_3, Sub(__) Translator.LdVar(__, __.Locals, 3)},
{OpCodes.Stloc, Sub(__) Translator.StVar(__, __.Locals, System.Convert.ToInt32(__.Argument))},
{OpCodes.Stloc_S, Sub(__) Translator.StVar(__, __.Locals, System.Convert.ToInt32(__.Argument))},
{OpCodes.Stloc_0, Sub(__) Translator.StVar(__, __.Locals, 0)},
{OpCodes.Stloc_1, Sub(__) Translator.StVar(__, __.Locals, 1)},
{OpCodes.Stloc_2, Sub(__) Translator.StVar(__, __.Locals, 2)},
{OpCodes.Stloc_3, Sub(__) Translator.StVar(__, __.Locals, 3)},
{OpCodes.Ldarga, Sub(__) Translator.LdVarA(__, __.Parameters, System.Convert.ToInt32(__.Argument))},
{OpCodes.Ldarga_S, Sub(__) Translator.LdVarA(__, __.Parameters, System.Convert.ToInt32(__.Argument))},
{OpCodes.Ldarg, Sub(__) Translator.LdVar(__, __.Parameters, System.Convert.ToInt32(__.Argument))},
{OpCodes.Ldarg_S, Sub(__) Translator.LdVar(__, __.Parameters, System.Convert.ToInt32(__.Argument))},
{OpCodes.Ldarg_0, Sub(__) Translator.LdVar(__, __.Parameters, 0)},
{OpCodes.Ldarg_1, Sub(__) Translator.LdVar(__, __.Parameters, 1)},
{OpCodes.Ldarg_2, Sub(__) Translator.LdVar(__, __.Parameters, 2)},
{OpCodes.Ldarg_3, Sub(__) Translator.LdVar(__, __.Parameters, 3)},
{OpCodes.Starg, Sub(__) Translator.StVar(__, __.Parameters, System.Convert.ToInt32(__.Argument))},
{OpCodes.Starg_S, Sub(__) Translator.StVar(__, __.Parameters, System.Convert.ToInt32(__.Argument))},
{OpCodes.Br, AddressOf Translator.Br},
{OpCodes.Br_S, AddressOf Translator.Br},
{OpCodes.Brtrue, Sub(__) Translator.BrCond(__, True)},
{OpCodes.Brtrue_S, Sub(__) Translator.BrCond(__, True)},
{OpCodes.Brfalse, Sub(__) Translator.BrCond(__, False)},
{OpCodes.Brfalse_S, Sub(__) Translator.BrCond(__, False)},
{OpCodes.Beq, Sub(__)
                  Call Translator.Ceq(__)
                  Call Translator.BrCond(__, True)
              End Sub},
{OpCodes.Beq_S, Sub(__)
                    Call Translator.Ceq(__)
                    Call Translator.BrCond(__, True)
                End Sub},
{OpCodes.Bne_Un, Sub(__)
                     Call Translator.Ceq(__)
                     Call Translator.BrCond(__, False)
                 End Sub},
{OpCodes.Bne_Un_S, Sub(__)
                       Call Translator.Ceq(__)
                       Call Translator.BrCond(__, False)
                   End Sub},
{OpCodes.Ble, Sub(__)
                  Call Translator.Cle(__)
                  Call Translator.BrCond(__, True)
              End Sub},
{OpCodes.Ble_S, Sub(__)
                    Call Translator.Cle(__)
                    Call Translator.BrCond(__, True)
                End Sub},
{OpCodes.Ble_Un, Sub(__)
                     Call Translator.CleUn(__)
                     Call Translator.BrCond(__, True)
                 End Sub},
{OpCodes.Ble_Un_S, Sub(__)
                       Call Translator.CleUn(__)
                       Call Translator.BrCond(__, True)
                   End Sub},
{OpCodes.Blt, Sub(__)
                  Call Translator.Clt(__)
                  Call Translator.BrCond(__, True)
              End Sub},
{OpCodes.Blt_S, Sub(__)
                    Call Translator.Clt(__)
                    Call Translator.BrCond(__, True)
                End Sub},
{OpCodes.Blt_Un, Sub(__)
                     Call Translator.CltUn(__)
                     Call Translator.BrCond(__, True)
                 End Sub},
{OpCodes.Blt_Un_S, Sub(__)
                       Call Translator.CltUn(__)
                       Call Translator.BrCond(__, True)
                   End Sub},
{OpCodes.Bge, Sub(__)
                  Call Translator.Cge(__)
                  Call Translator.BrCond(__, True)
              End Sub},
{OpCodes.Bge_S, Sub(__)
                    Call Translator.Cge(__)
                    Call Translator.BrCond(__, True)
                End Sub},
{OpCodes.Bge_Un, Sub(__)
                     Call Translator.CgeUn(__)
                     Call Translator.BrCond(__, True)
                 End Sub},
{OpCodes.Bge_Un_S, Sub(__)
                       Call Translator.CgeUn(__)
                       Call Translator.BrCond(__, True)
                   End Sub},
{OpCodes.Bgt, Sub(__)
                  Call Translator.Cgt(__)
                  Call Translator.BrCond(__, True)
              End Sub},
{OpCodes.Bgt_S, Sub(__)
                    Call Translator.Cgt(__)
                    Call Translator.BrCond(__, True)
                End Sub},
{OpCodes.Bgt_Un, Sub(__)
                     Call Translator.CgtUn(__)
                     Call Translator.BrCond(__, True)
                 End Sub},
{OpCodes.Bgt_Un_S, Sub(__)
                       Call Translator.CgtUn(__)
                       Call Translator.BrCond(__, True)
                   End Sub},
{OpCodes.Tailcall, Sub(__)
                   End Sub},
{OpCodes.[Call], AddressOf Translator.[Call]},
{OpCodes.Ldfld, AddressOf Translator.Ldfld},
{OpCodes.Stfld, Sub(__)
                    Dim value = __.Stack.Pop()
                    Dim ptr = __.Stack.Pop()
                    __.Builder.Store(value, Translator.ElementPointer(__, ptr, Translator.FieldIndex(CType(__.Argument, System.Reflection.FieldInfo))))
                End Sub},
{OpCodes.Newobj, Sub(__)
                     Call Translator.NewobjPreConstructor(__)
                     Call Translator.[Call](__)
                 End Sub},
{OpCodes.Initobj, Sub(__) __.Builder.Store(Translator.ConvertType(CType((__.[Module]), LLVM.[Module]), CType(CType(__.Argument, System.Type), System.Type)).Zero, __.Stack.Pop())}
}

        Private Sub Ldfld(__ As EmitFuncObj)
            Dim obj = __.Stack.Pop()

            If TypeOf obj.Type Is LLVM.PointerType Then
                Call __.Stack.Push(__.Builder.Load(Translator.ElementPointer(__, obj, Translator.FieldIndex(CType(__.Argument, System.Reflection.FieldInfo)))))
            Else
                Call __.Stack.Push(__.Builder.Extract(obj, Translator.FieldIndex(CType(__.Argument, System.Reflection.FieldInfo))))
            End If
        End Sub

        Private Function ElementPointer(__ As EmitFuncObj, pointer As LLVM.Value, index As Integer) As LLVM.Value
            Dim zeroConstant = LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(0, False)
            Dim indexConstant = LLVM.IntegerType.GetInt32(CType((__.Context), LLVM.Context)).Constant(CULng(index), False)
            ' guarenteed to be pointer type
            Return __.Builder.Element(pointer, New LLVM.Value() {zeroConstant, indexConstant})
        End Function

        Private Function AllFields(type As System.Type) As IEnumerable(Of System.Reflection.FieldInfo)
            Return type.GetRuntimeFields().Where(Function(f) f.IsStatic = False)
        End Function

        Private Function FieldIndex(field As System.Reflection.FieldInfo) As Integer
            Return Translator.AllFields(field.DeclaringType).IndexOf(Function(f) f Is field)
        End Function

        Private Sub NewobjPreConstructor(__ As EmitFuncObj)
            Dim stackalloca = __.Builder.StackAlloc(Translator.ConvertType(__.[Module], CType(__.Argument, ConstructorInfo).DeclaringType))

            Dim altstack = New Stack(Of LLVM.Value)()
            Dim paramLength = CType(__.Argument, MethodBase).GetParameters().Length

            For i = 0 To paramLength - 1
                altstack.Push(__.Stack.Pop())
            Next

            __.Stack.Push(stackalloca)

            For i = 0 To paramLength - 1
                __.Stack.Push(altstack.Pop())
            Next
        End Sub

        Private Sub [Call](__ As EmitFuncObj)
            Dim method = CType(__.Argument, MethodBase)
            Dim count = method.GetParameters().Length

            If TypeOf method Is ConstructorInfo OrElse method.IsStatic = False Then
                count += 1
            End If

            Dim args = Enumerable.Range(0, count).[Select](Function(x) __.Stack.Pop()).Reverse().ToArray()
            Dim result = __.Builder.[Call](Translator.EmitFunction(__.[Module], method), args)

            If result.Type.StructuralEquals(LLVM.Type.GetVoid(__.Context)) = False Then
                Call __.Stack.Push(result)
            ElseIf TypeOf method Is System.Reflection.ConstructorInfo Then
                Call __.Stack.Push(__.Builder.Load(args(0)))
            End If
        End Sub

        Private Sub FlipTopTwoStack(__ As EmitFuncObj)
            Dim top = __.Stack.Pop()
            Dim bottom = __.Stack.Pop()
            __.Stack.Push(top)
            __.Stack.Push(bottom)
        End Sub

        Private Sub Ceq(__ As EmitFuncObj)
            Call Translator.FlipTopTwoStack(__)
            __.Stack.Push(__.Builder.Compare(LLVM.IntegerComparison.Equal, Translator.PopNoBool(__), Translator.PopNoBool(__)))
        End Sub

        Private Sub Cgt(__ As EmitFuncObj)
            Call Translator.FlipTopTwoStack(__)
            __.Stack.Push(__.Builder.Compare(LLVM.IntegerComparison.SignedGreater, __.Stack.Pop(), __.Stack.Pop()))
        End Sub

        Private Sub CgtUn(__ As EmitFuncObj)
            Call Translator.FlipTopTwoStack(__)
            __.Stack.Push(__.Builder.Compare(LLVM.IntegerComparison.UnsignedGreater, __.Stack.Pop(), __.Stack.Pop()))
        End Sub

        Private Sub Cge(__ As EmitFuncObj)
            Call Translator.FlipTopTwoStack(__)
            __.Stack.Push(__.Builder.Compare(LLVM.IntegerComparison.SignedGreaterEqual, __.Stack.Pop(), __.Stack.Pop()))
        End Sub

        Private Sub CgeUn(__ As EmitFuncObj)
            Call Translator.FlipTopTwoStack(__)
            __.Stack.Push(__.Builder.Compare(LLVM.IntegerComparison.UnsignedGreaterEqual, __.Stack.Pop(), __.Stack.Pop()))
        End Sub

        Private Sub Clt(__ As EmitFuncObj)
            Call Translator.FlipTopTwoStack(__)
            __.Stack.Push(__.Builder.Compare(LLVM.IntegerComparison.SignedLess, __.Stack.Pop(), __.Stack.Pop()))
        End Sub

        Private Sub CltUn(__ As EmitFuncObj)
            Call Translator.FlipTopTwoStack(__)
            __.Stack.Push(__.Builder.Compare(LLVM.IntegerComparison.UnsignedLess, __.Stack.Pop(), __.Stack.Pop()))
        End Sub

        Private Sub Cle(__ As EmitFuncObj)
            Call Translator.FlipTopTwoStack(__)
            __.Stack.Push(__.Builder.Compare(LLVM.IntegerComparison.SignedLessEqual, __.Stack.Pop(), __.Stack.Pop()))
        End Sub

        Private Sub CleUn(__ As EmitFuncObj)
            Call Translator.FlipTopTwoStack(__)
            __.Stack.Push(__.Builder.Compare(LLVM.IntegerComparison.UnsignedLessEqual, __.Stack.Pop(), __.Stack.Pop()))
        End Sub

        Private ReadOnly UnsupportedInstructionsArr As OpCode() = GetType(OpCodes) _
            .GetFields() _
            .[Select](Function(f) CType(f.GetValue(Nothing), OpCode)) _
            .Where(Function(op) Not Translator.EmitFunctions.ContainsKey(op)) _
            .ToArray()

        Public ReadOnly Property UnsupportedInstructions As OpCode()
            Get
                Return Translator.UnsupportedInstructionsArr
            End Get
        End Property

        Private Function PopNoBool(__ As EmitFuncObj) As LLVM.Value
            Dim popped = __.Stack.Pop()
            If popped.Type.StructuralEquals(LLVM.IntegerType.[Get](__.Context, 1)) Then popped = __.Builder.ZeroExtend(popped, LLVM.IntegerType.GetInt32(__.Context))
            Return popped
        End Function

        Private Sub Nop(__ As EmitFuncObj)
            Dim block = CType(__.Argument, LLVM.Block)
            If block Is Nothing Then Return
            If __.Builder IsNot Nothing Then __.Builder.[GoTo](block)
            __.Builder = New LLVM.InstructionBuilder(__.Context, block)
        End Sub

        Private Sub Br(__ As EmitFuncObj)
            __.Builder.[GoTo](CType(__.Argument, LLVM.Block))
            __.Builder = Nothing
        End Sub

        Private Sub BrCond(__ As EmitFuncObj, isTrue As Boolean)
            Dim tuple = CType(__.Argument, System.Tuple(Of LLVM.Block, LLVM.Block))
            Dim cont = tuple.Item1
            Dim target = tuple.Item2

            __.Builder.[If](__.Stack.Pop(), If(isTrue, target, cont), If(isTrue, cont, target))
            __.Builder = New LLVM.InstructionBuilder(__.Context, cont)
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Sub LdVar(__ As EmitFuncObj, values As LLVM.Value(), index As Integer)
            __.Stack.Push(__.Builder.Load(values(index)))
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Sub LdVarA(__ As EmitFuncObj, values As LLVM.Value(), index As Integer)
            __.Stack.Push(values(index))
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Sub StVar(__ As EmitFuncObj, values As LLVM.Value(), index As Integer)
            Call __.Builder.Store(__.Stack.Pop(), values(index))
        End Sub

        Private Sub StElem(__ As EmitFuncObj)
            Dim value = __.Stack.Pop()
            Dim index = __.Stack.Pop()
            Dim array = __.Stack.Pop()
            Dim idx = __.Builder.Element(array, {index})

            Call __.Builder.Store(value, idx)
        End Sub

        Private Sub LdElem(__ As EmitFuncObj)
            Dim index = __.Stack.Pop()
            Dim array = __.Stack.Pop()
            Dim idx = __.Builder.Element(array, {index})
            Dim load = __.Builder.Load(idx)
            __.Stack.Push(load)
        End Sub

        Private Sub LdElemA(__ As EmitFuncObj)
            Dim index = __.Stack.Pop()
            Dim array = __.Stack.Pop()
            Dim idx = __.Builder.Element(array, {index})

            Call __.Stack.Push(idx)
        End Sub

        Private Sub ConvertNum(__ As EmitFuncObj, target As LLVM.Type, integerSignedness As Boolean)
            Dim value = __.Stack.Pop()
            Dim valueType = value.Type
            If TypeOf valueType Is LLVM.IntegerType AndAlso TypeOf target Is LLVM.FloatType Then
                If integerSignedness Then
                    value = __.Builder.SignedIntToFloat(value, target)
                Else
                    value = __.Builder.UnsignedIntToFloat(value, target)
                End If
            ElseIf TypeOf valueType Is LLVM.FloatType AndAlso TypeOf target Is LLVM.IntegerType Then
                If integerSignedness Then
                    value = __.Builder.FloatToSignedInt(value, target)
                Else
                    value = __.Builder.FloatToUnsignedInt(value, target)
                End If
            ElseIf TypeOf valueType Is LLVM.IntegerType AndAlso TypeOf target Is LLVM.IntegerType Then
                Dim valueInt = CType(valueType, LLVM.IntegerType)
                Dim targetInt = CType(target, LLVM.IntegerType)
                If valueInt.Width > targetInt.Width Then
                    value = __.Builder.Trunc(value, target)
                ElseIf integerSignedness Then
                    value = __.Builder.SignExtend(value, target)
                Else
                    value = __.Builder.ZeroExtend(value, target)
                End If
            Else
                Throw New CudaException(String.Format("Cannot convert {0} to {1}", valueType, target))
            End If

            __.Stack.Push(value)
        End Sub
    End Module
End Namespace