Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Namespace LLVM.Interop
	' Token: 0x0200002D RID: 45
	Public Class ClrInterop
		' Token: 0x06000123 RID: 291 RVA: 0x0000398F File Offset: 0x00001B8F
		Public Sub New(executionEngine As Global.LLVM.ExecutionEngine)
			If executionEngine Is Nothing Then
				Throw New Global.System.ArgumentNullException("executionEngine")
			End If
			Me.executionEngine = executionEngine
			Me.nativeWrapper = New Global.LLVM.Interop.ClrInterop.Native()
			Me.managedWrapper = New Global.LLVM.Interop.ClrInterop.Managed()
		End Sub

		' Token: 0x06000124 RID: 292 RVA: 0x000039C2 File Offset: 0x00001BC2
		Public Function GetDelegate(Of T As Class)([function] As Global.LLVM.[Function], [module] As Global.LLVM.[Module], Optional debug As Boolean = False) As T
			Return TryCast(Me.GetDelegate([function], GetType(!!0), [module], debug), !!0)
		End Function

		' Token: 0x06000125 RID: 293 RVA: 0x000039E4 File Offset: 0x00001BE4
		Public Function GetDelegate([function] As Global.LLVM.[Function], delegateType As Global.System.Type, [module] As Global.LLVM.[Module], Optional debug As Boolean = False) As Global.System.[Delegate]
			Dim [global] As Global.LLVM.[Function] = Me.nativeWrapper.Wrap([function], [module], debug)
			Dim pointer As Global.System.IntPtr = Me.executionEngine.GetPointer([global])
			Return Me.managedWrapper.Unwrap(pointer, delegateType, debug)
		End Function

		' Token: 0x0400002E RID: 46
		Private executionEngine As Global.LLVM.ExecutionEngine

		' Token: 0x0400002F RID: 47
		Private nativeWrapper As Global.LLVM.Interop.ClrInterop.Native

		' Token: 0x04000030 RID: 48
		Private managedWrapper As Global.LLVM.Interop.ClrInterop.Managed

		' Token: 0x02000039 RID: 57
		Friend Class Managed
			' Token: 0x06000156 RID: 342 RVA: 0x00003DA7 File Offset: 0x00001FA7
			Public Sub New()
				Me.[module] = Global.System.AppDomain.CurrentDomain.DefineDynamicAssembly(New Global.System.Reflection.AssemblyName() With { .Version = New Global.System.Version(1, 0), .Name = Global.LLVM.Interop.ClrInterop.Managed.GenerateIdentifier() }, Global.System.Reflection.Emit.AssemblyBuilderAccess.Run).DefineDynamicModule("DelegateTypes")
			End Sub

			' Token: 0x06000157 RID: 343 RVA: 0x00003DE7 File Offset: 0x00001FE7
			Public Sub New([module] As Global.System.Reflection.Emit.ModuleBuilder)
				If [module] Is Nothing Then
					Throw New Global.System.ArgumentNullException("module")
				End If
				Me.[module] = [module]
			End Sub

			' Token: 0x06000158 RID: 344 RVA: 0x00003E0C File Offset: 0x0000200C
			Public Function Unwrap(wrapperEntryPoint As Global.System.IntPtr, delegateType As Global.System.Type, debug As Boolean) As Global.System.[Delegate]
				Dim t As Global.System.Type = Me.WrapDelegateType(delegateType)
				Dim delegateForFunctionPointer As Global.System.[Delegate] = Global.System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(wrapperEntryPoint, t)
				Return Me.Unwrap(delegateForFunctionPointer, delegateType, debug)
			End Function

			' Token: 0x06000159 RID: 345 RVA: 0x00003E34 File Offset: 0x00002034
			Friend Function Unwrap(wrapped As Global.System.[Delegate], delegateType As Global.System.Type, debug As Boolean) As Global.System.[Delegate]
				Dim method As Global.System.Reflection.MethodInfo = delegateType.GetMethod("Invoke")
				Dim parameters As Global.System.Collections.Generic.IEnumerable(Of Global.System.Reflection.ParameterInfo) = method.GetParameters()
				Dim parameterExpression As Global.System.Linq.Expressions.ParameterExpression = If(Global.LLVM.Interop.ClrInterop.Managed.NeedsReturnWrapping(method.ReturnType), Global.System.Linq.Expressions.Expression.Variable(method.ReturnType, "retval"), Nothing)
				Dim <>9__4_ As Global.System.Func(Of Global.System.Reflection.ParameterInfo, Global.System.Linq.Expressions.ParameterExpression) = Global.LLVM.Interop.ClrInterop.Managed.<>c.<>9__4_0
				Dim selector As Global.System.Func(Of Global.System.Reflection.ParameterInfo, Global.System.Linq.Expressions.ParameterExpression) = <>9__4_
				If <>9__4_ Is Nothing Then
					Dim func As Global.System.Func(Of Global.System.Reflection.ParameterInfo, Global.System.Linq.Expressions.ParameterExpression) = Function(p As Global.System.Reflection.ParameterInfo) Global.System.Linq.Expressions.Expression.Parameter(p.ParameterType, p.Name)
					selector = func
					Global.LLVM.Interop.ClrInterop.Managed.<>c.<>9__4_0 = func
				End If
				Dim array As Global.System.Linq.Expressions.ParameterExpression() = parameters.[Select](selector).ToArray()
				Dim list As Global.System.Collections.Generic.List(Of Global.System.Linq.Expressions.ParameterExpression) = array.ToList()
				If parameterExpression IsNot Nothing Then
					list.Add(parameterExpression)
				End If
				Dim item As Global.System.Linq.Expressions.InvocationExpression = Global.System.Linq.Expressions.Expression.Invoke(Global.System.Linq.Expressions.Expression.Constant(wrapped, wrapped.[GetType]()), list)
				Dim list2 As Global.System.Collections.Generic.List(Of Global.System.Linq.Expressions.Expression) = New Global.System.Collections.Generic.List(Of Global.System.Linq.Expressions.Expression)()
				If debug Then
					list2.Add(Global.System.Linq.Expressions.Expression.[Call](GetType(Global.System.Diagnostics.Debugger).GetMethod("Break"), New Global.System.Linq.Expressions.Expression(-1) {}))
				End If
				Dim list3 As Global.System.Collections.Generic.List(Of Global.System.Linq.Expressions.ParameterExpression) = New Global.System.Collections.Generic.List(Of Global.System.Linq.Expressions.ParameterExpression)()
				If parameterExpression IsNot Nothing Then
					list3.Add(parameterExpression)
					list2.Add(item)
					list2.Add(parameterExpression)
				Else
					list2.Add(item)
				End If
				Dim body As Global.System.Linq.Expressions.BlockExpression = Global.System.Linq.Expressions.Expression.Block(list3, list2)
				Return Global.System.Linq.Expressions.Expression.Lambda(delegateType, body, array).Compile()
			End Function

			' Token: 0x0600015A RID: 346 RVA: 0x00003F3C File Offset: 0x0000213C
			Public Function WrapDelegateType(delegateType As Global.System.Type) As Global.System.Type
				Dim method As Global.System.Reflection.MethodInfo = delegateType.GetMethod("Invoke")
				Dim typeBuilder As Global.System.Reflection.Emit.TypeBuilder = Me.[module].DefineType("DELEGATE_" + Global.LLVM.Interop.ClrInterop.Managed.GenerateIdentifier(), Global.System.Reflection.TypeAttributes.[Public] Or Global.System.Reflection.TypeAttributes.Sealed Or Global.System.Reflection.TypeAttributes.AutoClass, GetType(Global.System.MulticastDelegate))
				typeBuilder.SetCustomAttribute(Global.LLVM.Interop.ClrInterop.Managed.callingConventionAttribute)
				typeBuilder.DefineConstructor(Global.System.Reflection.MethodAttributes.FamANDAssem Or Global.System.Reflection.MethodAttributes.Family Or Global.System.Reflection.MethodAttributes.HideBySig Or Global.System.Reflection.MethodAttributes.RTSpecialName, Global.System.Reflection.CallingConventions.Standard, New Global.System.Type() { GetType(Object), GetType(Global.System.IntPtr) }).SetImplementationFlags(Global.System.Reflection.MethodImplAttributes.CodeTypeMask)
				Dim parameters As Global.System.Collections.Generic.IEnumerable(Of Global.System.Reflection.ParameterInfo) = method.GetParameters()
				Dim <>9__5_ As Global.System.Func(Of Global.System.Reflection.ParameterInfo, Global.System.Type) = Global.LLVM.Interop.ClrInterop.Managed.<>c.<>9__5_0
				Dim selector As Global.System.Func(Of Global.System.Reflection.ParameterInfo, Global.System.Type) = <>9__5_
				If <>9__5_ Is Nothing Then
					Dim func As Global.System.Func(Of Global.System.Reflection.ParameterInfo, Global.System.Type) = Function(p As Global.System.Reflection.ParameterInfo) p.ParameterType
					selector = func
					Global.LLVM.Interop.ClrInterop.Managed.<>c.<>9__5_0 = func
				End If
				Dim source As Global.System.Collections.Generic.IEnumerable(Of Global.System.Type) = parameters.[Select](selector)
				Dim <>9__5_2 As Global.System.Func(Of Global.System.Type, Global.System.Type) = Global.LLVM.Interop.ClrInterop.Managed.<>c.<>9__5_1
				Dim selector2 As Global.System.Func(Of Global.System.Type, Global.System.Type) = <>9__5_2
				If <>9__5_2 Is Nothing Then
					Dim func2 As Global.System.Func(Of Global.System.Type, Global.System.Type) = Function(t As Global.System.Type)
						If Not t.IsValueType Then
							Return t
						End If
						Return t.MakeByRefType()
					End Function
					selector2 = func2
					Global.LLVM.Interop.ClrInterop.Managed.<>c.<>9__5_1 = func2
				End If
				Dim list As Global.System.Collections.Generic.List(Of Global.System.Type) = source.[Select](selector2).ToList()
				Dim returnByRef As Boolean
				Dim returnType As Global.System.Type = Global.LLVM.Interop.ClrInterop.Managed.WrapDelegateReturnType(method.ReturnType, list, returnByRef)
				Dim methodBuilder As Global.System.Reflection.Emit.MethodBuilder = typeBuilder.DefineMethod("Invoke", Global.System.Reflection.MethodAttributes.FamANDAssem Or Global.System.Reflection.MethodAttributes.Family Or Global.System.Reflection.MethodAttributes.Virtual Or Global.System.Reflection.MethodAttributes.HideBySig Or Global.System.Reflection.MethodAttributes.VtableLayoutMask)
				methodBuilder.SetSignature(returnType, Nothing, Nothing, list.ToArray(), Nothing, Nothing)
				methodBuilder.SetImplementationFlags(Global.System.Reflection.MethodImplAttributes.CodeTypeMask)
				Global.LLVM.Interop.ClrInterop.Managed.WrapDelegateParameters(methodBuilder, method, returnByRef)
				Return typeBuilder.CreateType()
			End Function

			' Token: 0x0600015B RID: 347 RVA: 0x00004058 File Offset: 0x00002258
			Private Shared Sub WrapDelegateParameters(method As Global.System.Reflection.Emit.MethodBuilder, originalMethod As Global.System.Reflection.MethodInfo, returnByRef As Boolean)
				Dim num As Integer = 1
				For Each parameterInfo As Global.System.Reflection.ParameterInfo In originalMethod.GetParameters()
					If Global.LLVM.Interop.ClrInterop.Managed.NeedsWrapping(parameterInfo) Then
						parameterInfo.ParameterType.MakeByRefType()
						method.DefineParameter(num, Global.System.Reflection.ParameterAttributes.[In], parameterInfo.Name)
					Else
						method.DefineParameter(num, parameterInfo.Attributes, parameterInfo.Name)
					End If
					num += 1
				Next
				If returnByRef Then
					method.DefineParameter(0, Global.System.Reflection.ParameterAttributes.Out, "retval")
				End If
			End Sub

			' Token: 0x0600015C RID: 348 RVA: 0x000040CE File Offset: 0x000022CE
			Private Shared Function NeedsWrapping(parameter As Global.System.Reflection.ParameterInfo) As Boolean
				Return parameter.ParameterType.IsValueType
			End Function

			' Token: 0x0600015D RID: 349 RVA: 0x000040DC File Offset: 0x000022DC
			Private Shared Function WrapDelegateReturnType(originalType As Global.System.Type, parameters As Global.System.Collections.Generic.ICollection(Of Global.System.Type), <System.Runtime.InteropServices.OutAttribute()> ByRef returnByRef As Boolean) As Global.System.Type
				returnByRef = Global.LLVM.Interop.ClrInterop.Managed.NeedsReturnWrapping(originalType)
				If Not returnByRef Then
					Return originalType
				End If
				Dim item As Global.System.Type = originalType.MakeByRefType()
				parameters.Add(item)
				Return GetType(Void)
			End Function

			' Token: 0x0600015E RID: 350 RVA: 0x0000410F File Offset: 0x0000230F
			Private Shared Function NeedsReturnWrapping(type As Global.System.Type) As Boolean
				Return type.IsValueType AndAlso Not GetType(Void).Equals(type)
			End Function

			' Token: 0x0600015F RID: 351 RVA: 0x00004130 File Offset: 0x00002330
			Private Shared Function GenerateIdentifier() As String
				Return Global.System.Guid.NewGuid().ToString().Replace("-"c, "_"c)
			End Function

			' Token: 0x06000160 RID: 352 RVA: 0x00004159 File Offset: 0x00002359
			' Note: this type is marked as 'beforefieldinit'.
			Shared Sub New()
			End Sub

			' Token: 0x0400005C RID: 92
			Private [module] As Global.System.Reflection.Emit.ModuleBuilder

			' Token: 0x0400005D RID: 93
			Private Shared callingConventionAttribute As Global.System.Reflection.Emit.CustomAttributeBuilder = New Global.System.Reflection.Emit.CustomAttributeBuilder(GetType(Global.System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute).GetConstructor(New Global.System.Type() { GetType(Global.System.Runtime.InteropServices.CallingConvention) }), New Object() { Global.System.Runtime.InteropServices.CallingConvention.Cdecl })

			' Token: 0x0400005E RID: 94
			Private Const InvokeAttributes As Global.System.Reflection.MethodAttributes = Global.System.Reflection.MethodAttributes.FamANDAssem Or Global.System.Reflection.MethodAttributes.Family Or Global.System.Reflection.MethodAttributes.Virtual Or Global.System.Reflection.MethodAttributes.HideBySig Or Global.System.Reflection.MethodAttributes.VtableLayoutMask

			' Token: 0x0400005F RID: 95
			Private Const RuntimeMethod As Global.System.Reflection.MethodImplAttributes = Global.System.Reflection.MethodImplAttributes.CodeTypeMask

			' Token: 0x0200003F RID: 63
			<Global.System.Runtime.CompilerServices.CompilerGenerated()>
			<Global.System.Serializable()>
			Private NotInheritable Class <>c
				' Token: 0x06000172 RID: 370 RVA: 0x000044F2 File Offset: 0x000026F2
				' Note: this type is marked as 'beforefieldinit'.
				Shared Sub New()
				End Sub

				' Token: 0x06000173 RID: 371 RVA: 0x000037DA File Offset: 0x000019DA
				Public Sub New()
				End Sub

				' Token: 0x06000174 RID: 372 RVA: 0x000044FE File Offset: 0x000026FE
				Friend Function <Unwrap>b__4_0(p As Global.System.Reflection.ParameterInfo) As Global.System.Linq.Expressions.ParameterExpression
					Return Global.System.Linq.Expressions.Expression.Parameter(p.ParameterType, p.Name)
				End Function

				' Token: 0x06000175 RID: 373 RVA: 0x00004511 File Offset: 0x00002711
				Friend Function <WrapDelegateType>b__5_0(p As Global.System.Reflection.ParameterInfo) As Global.System.Type
					Return p.ParameterType
				End Function

				' Token: 0x06000176 RID: 374 RVA: 0x00004519 File Offset: 0x00002719
				Friend Function <WrapDelegateType>b__5_1(t As Global.System.Type) As Global.System.Type
					If Not t.IsValueType Then
						Return t
					End If
					Return t.MakeByRefType()
				End Function

				' Token: 0x04000068 RID: 104
				Public Shared <>9 As Global.LLVM.Interop.ClrInterop.Managed.<>c = New Global.LLVM.Interop.ClrInterop.Managed.<>c()

				' Token: 0x04000069 RID: 105
				Public Shared <>9__4_0 As Global.System.Func(Of Global.System.Reflection.ParameterInfo, Global.System.Linq.Expressions.ParameterExpression)

				' Token: 0x0400006A RID: 106
				Public Shared <>9__5_0 As Global.System.Func(Of Global.System.Reflection.ParameterInfo, Global.System.Type)

				' Token: 0x0400006B RID: 107
				Public Shared <>9__5_1 As Global.System.Func(Of Global.System.Type, Global.System.Type)
			End Class
		End Class

		' Token: 0x0200003A RID: 58
		Friend Class Native
			' Token: 0x06000161 RID: 353 RVA: 0x00004198 File Offset: 0x00002398
			Public Function Wrap([function] As Global.LLVM.[Function], [module] As Global.LLVM.[Module], debug As Boolean) As Global.LLVM.[Function]
				Dim type As Global.LLVM.FunctionType = [function].Type
				Dim type2 As Global.LLVM.FunctionType = Me.WrapFunctionType(type)
				Dim function2 As Global.LLVM.[Function] = [module].CreateFunction(Global.System.Guid.NewGuid().ToString().Replace("-", ""), type2)
				function2.CallingConvention = Global.LLVM.CallingConvention.C
				Dim block As Global.LLVM.Block = New Global.LLVM.Block("", [module].Context, function2)
				Dim gen As Global.LLVM.InstructionBuilder = New Global.LLVM.InstructionBuilder([module].Context, block)
				If debug Then
					Me.EmitDebugBreak(gen, [module])
				End If
				Dim callResult As Global.LLVM.[Call] = Global.LLVM.Interop.ClrInterop.Native.WrapCall([function], function2, gen)
				Global.LLVM.Interop.ClrInterop.Native.GenerateReturn(type, function2, gen, callResult)
				If debug Then
					function2.PrintToString()
				End If
				Return function2
			End Function

			' Token: 0x06000162 RID: 354 RVA: 0x00004234 File Offset: 0x00002434
			Private Sub EmitDebugBreak(gen As Global.LLVM.InstructionBuilder, [module] As Global.LLVM.[Module])
				Dim [function] As Global.LLVM.[Function] = [module].GetFunction("LLVMDebugBreak")
				If [function] Is Nothing Then
					Throw New Global.System.NotSupportedException("Module must have LLVMDebugBreak function defined")
				End If
				gen.[Call]([function], New Global.LLVM.Value(-1) {})
			End Sub

			' Token: 0x06000163 RID: 355 RVA: 0x0000426C File Offset: 0x0000246C
			Friend Function WrapFunctionType(originalType As Global.LLVM.FunctionType) As Global.LLVM.FunctionType
				Dim list As Global.System.Collections.Generic.List(Of Global.LLVM.Type) = originalType.ArgumentTypes.[Select](AddressOf Me.WrapArg).ToList()
				Dim flag As Boolean
				Return New Global.LLVM.FunctionType(Global.LLVM.Interop.ClrInterop.Native.WrapReturnType(originalType, list, flag), list.ToArray(), False)
			End Function

			' Token: 0x06000164 RID: 356 RVA: 0x000042AC File Offset: 0x000024AC
			Private Shared Sub GenerateReturn(originalType As Global.LLVM.FunctionType, wrapper As Global.LLVM.[Function], gen As Global.LLVM.InstructionBuilder, callResult As Global.LLVM.[Call])
				Dim kind As Global.LLVM.TypeKind = originalType.ReturnType.Kind
				If kind = Global.LLVM.TypeKind.Void Then
					gen.[Return]()
					Return
				End If
				If kind <> Global.LLVM.TypeKind.Pointer Then
					Dim argument As Global.LLVM.Argument = wrapper(originalType.ArgumentCount)
					argument.Name = "retval"
					gen.Store(callResult, argument)
					gen.[Return]()
					Return
				End If
				gen.[Return](callResult)
			End Sub

			' Token: 0x06000165 RID: 357 RVA: 0x00004308 File Offset: 0x00002508
			Private Shared Function WrapCall([function] As Global.LLVM.[Function], wrapper As Global.LLVM.[Function], gen As Global.LLVM.InstructionBuilder) As Global.LLVM.[Call]
				Dim argumentTypes As Global.LLVM.Type() = wrapper.Type.ArgumentTypes
				Dim argumentTypes2 As Global.LLVM.Type() = [function].Type.ArgumentTypes
				Dim list As Global.System.Collections.Generic.List(Of Global.LLVM.Value) = New Global.System.Collections.Generic.List(Of Global.LLVM.Value)()
				For i As Integer = 0 To argumentTypes2.Length - 1
					Dim argument As Global.LLVM.Argument = wrapper(i)
					If String.IsNullOrEmpty(argument.Name) Then
						argument.Name = "arg" + i
					End If
					If argumentTypes2(i).Kind = argumentTypes(i).Kind Then
						list.Add(argument)
					Else
						Dim item As Global.LLVM.Load = gen.Load(argument, "")
						list.Add(item)
					End If
				Next
				Return gen.[Call]([function], list.ToArray())
			End Function

			' Token: 0x06000166 RID: 358 RVA: 0x000043B0 File Offset: 0x000025B0
			Private Shared Function WrapReturnType(originalType As Global.LLVM.FunctionType, wrapperArgs As Global.System.Collections.Generic.List(Of Global.LLVM.Type), <System.Runtime.InteropServices.OutAttribute()> ByRef returnByRef As Boolean) As Global.LLVM.Type
				Dim kind As Global.LLVM.TypeKind = originalType.ReturnType.Kind
				If kind = Global.LLVM.TypeKind.Void OrElse kind = Global.LLVM.TypeKind.Pointer Then
					returnByRef = False
					Return originalType.ReturnType
				End If
				returnByRef = True
				wrapperArgs.Add(Global.LLVM.PointerType.[Get](originalType.ReturnType, 0))
				Return Global.LLVM.Type.GetVoid(originalType.Context)
			End Function

			' Token: 0x06000167 RID: 359 RVA: 0x000043FC File Offset: 0x000025FC
			Private Function WrapArg(argType As Global.LLVM.Type) As Global.LLVM.Type
				Dim kind As Global.LLVM.TypeKind = argType.Kind
				If kind = Global.LLVM.TypeKind.Pointer Then
					Return argType
				End If
				Return Global.LLVM.PointerType.[Get](argType, 0)
			End Function

			' Token: 0x06000168 RID: 360 RVA: 0x000037DA File Offset: 0x000019DA
			Public Sub New()
			End Sub
		End Class
	End Class
End Namespace
