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
		Public Sub New(executionEngine As ExecutionEngine)
			If executionEngine Is Nothing Then
				Throw New ArgumentNullException("executionEngine")
			End If
			Me.executionEngine = executionEngine
			Me.nativeWrapper = New Interop.ClrInterop.Native()
			Me.managedWrapper = New Interop.ClrInterop.Managed()
		End Sub

		' Token: 0x06000124 RID: 292 RVA: 0x000039C2 File Offset: 0x00001BC2
		Public Function GetDelegate(Of T As Class)([function] As [Function], [module] As [Module], Optional debug As Boolean = False) As T
			Return TryCast(Me.GetDelegate([function], GetType(!!0), [module], debug), !!0)
		End Function

		' Token: 0x06000125 RID: 293 RVA: 0x000039E4 File Offset: 0x00001BE4
		Public Function GetDelegate([function] As [Function], delegateType As Type, [module] As [Module], Optional debug As Boolean = False) As [Delegate]
			Dim [global] As [Function] = Me.nativeWrapper.Wrap([function], [module], debug)
			Dim pointer As IntPtr = Me.executionEngine.GetPointer([global])
			Return Me.managedWrapper.Unwrap(pointer, delegateType, debug)
		End Function

		' Token: 0x0400002E RID: 46
		Private executionEngine As ExecutionEngine

		' Token: 0x0400002F RID: 47
		Private nativeWrapper As Interop.ClrInterop.Native

		' Token: 0x04000030 RID: 48
		Private managedWrapper As Interop.ClrInterop.Managed

		' Token: 0x02000039 RID: 57
		Friend Class Managed
			' Token: 0x06000156 RID: 342 RVA: 0x00003DA7 File Offset: 0x00001FA7
			Public Sub New()
				Me.[module] = AppDomain.CurrentDomain.DefineDynamicAssembly(New Reflection.AssemblyName() With { .Version = New Version(1, 0), .Name = Interop.ClrInterop.Managed.GenerateIdentifier() }, Reflection.Emit.AssemblyBuilderAccess.Run).DefineDynamicModule("DelegateTypes")
			End Sub

			' Token: 0x06000157 RID: 343 RVA: 0x00003DE7 File Offset: 0x00001FE7
			Public Sub New([module] As Reflection.Emit.ModuleBuilder)
				If [module] Is Nothing Then
					Throw New ArgumentNullException("module")
				End If
				Me.[module] = [module]
			End Sub

			' Token: 0x06000158 RID: 344 RVA: 0x00003E0C File Offset: 0x0000200C
			Public Function Unwrap(wrapperEntryPoint As IntPtr, delegateType As Type, debug As Boolean) As [Delegate]
				Dim t As Type = Me.WrapDelegateType(delegateType)
				Dim delegateForFunctionPointer As [Delegate] = Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(wrapperEntryPoint, t)
				Return Me.Unwrap(delegateForFunctionPointer, delegateType, debug)
			End Function

			' Token: 0x06000159 RID: 345 RVA: 0x00003E34 File Offset: 0x00002034
			Friend Function Unwrap(wrapped As [Delegate], delegateType As Type, debug As Boolean) As [Delegate]
				Dim method As Reflection.MethodInfo = delegateType.GetMethod("Invoke")
				Dim parameters As Collections.Generic.IEnumerable(Of Reflection.ParameterInfo) = method.GetParameters()
				Dim parameterExpression As Linq.Expressions.ParameterExpression = If(Interop.ClrInterop.Managed.NeedsReturnWrapping(method.ReturnType), Linq.Expressions.Expression.Variable(method.ReturnType, "retval"), Nothing)
				Dim <>9__4_ As Func(Of Reflection.ParameterInfo, Linq.Expressions.ParameterExpression) = Interop.ClrInterop.Managed.<>c.<>9__4_0
				Dim selector As Func(Of Reflection.ParameterInfo, Linq.Expressions.ParameterExpression) = <>9__4_
				If <>9__4_ Is Nothing Then
					Dim func As Func(Of Reflection.ParameterInfo, Linq.Expressions.ParameterExpression) = Function(p As Reflection.ParameterInfo) Linq.Expressions.Expression.Parameter(p.ParameterType, p.Name)
					selector = func
					Interop.ClrInterop.Managed.<>c.<>9__4_0 = func
				End If
				Dim array As Linq.Expressions.ParameterExpression() = parameters.[Select](selector).ToArray()
				Dim list As Collections.Generic.List(Of Linq.Expressions.ParameterExpression) = array.ToList()
				If parameterExpression IsNot Nothing Then
					list.Add(parameterExpression)
				End If
				Dim item As Linq.Expressions.InvocationExpression = Linq.Expressions.Expression.Invoke(Linq.Expressions.Expression.Constant(wrapped, wrapped.[GetType]()), list)
				Dim list2 As Collections.Generic.List(Of Linq.Expressions.Expression) = New Collections.Generic.List(Of Linq.Expressions.Expression)()
				If debug Then
					list2.Add(Linq.Expressions.Expression.[Call](GetType(Diagnostics.Debugger).GetMethod("Break"), New Linq.Expressions.Expression(-1) {}))
				End If
				Dim list3 As Collections.Generic.List(Of Linq.Expressions.ParameterExpression) = New Collections.Generic.List(Of Linq.Expressions.ParameterExpression)()
				If parameterExpression IsNot Nothing Then
					list3.Add(parameterExpression)
					list2.Add(item)
					list2.Add(parameterExpression)
				Else
					list2.Add(item)
				End If
				Dim body As Linq.Expressions.BlockExpression = Linq.Expressions.Expression.Block(list3, list2)
				Return Linq.Expressions.Expression.Lambda(delegateType, body, array).Compile()
			End Function

			' Token: 0x0600015A RID: 346 RVA: 0x00003F3C File Offset: 0x0000213C
			Public Function WrapDelegateType(delegateType As Type) As Type
				Dim method As Reflection.MethodInfo = delegateType.GetMethod("Invoke")
				Dim typeBuilder As Reflection.Emit.TypeBuilder = Me.[module].DefineType("DELEGATE_" + Interop.ClrInterop.Managed.GenerateIdentifier(), Reflection.TypeAttributes.[Public] Or Reflection.TypeAttributes.Sealed Or Reflection.TypeAttributes.AutoClass, GetType(MulticastDelegate))
				typeBuilder.SetCustomAttribute(Interop.ClrInterop.Managed.callingConventionAttribute)
				typeBuilder.DefineConstructor(Reflection.MethodAttributes.FamANDAssem Or Reflection.MethodAttributes.Family Or Reflection.MethodAttributes.HideBySig Or Reflection.MethodAttributes.RTSpecialName, Reflection.CallingConventions.Standard, New Type() { GetType(Object), GetType(IntPtr) }).SetImplementationFlags(Reflection.MethodImplAttributes.CodeTypeMask)
				Dim parameters As Collections.Generic.IEnumerable(Of Reflection.ParameterInfo) = method.GetParameters()
				Dim <>9__5_ As Func(Of Reflection.ParameterInfo, Type) = Interop.ClrInterop.Managed.<>c.<>9__5_0
				Dim selector As Func(Of Reflection.ParameterInfo, Type) = <>9__5_
				If <>9__5_ Is Nothing Then
					Dim func As Func(Of Reflection.ParameterInfo, Type) = Function(p As Reflection.ParameterInfo) p.ParameterType
					selector = func
					Interop.ClrInterop.Managed.<>c.<>9__5_0 = func
				End If
				Dim source As Collections.Generic.IEnumerable(Of Type) = parameters.[Select](selector)
				Dim <>9__5_2 As Func(Of Type, Type) = Interop.ClrInterop.Managed.<>c.<>9__5_1
				Dim selector2 As Func(Of Type, Type) = <>9__5_2
				If <>9__5_2 Is Nothing Then
					Dim func2 As Func(Of Type, Type) = Function(t As Type)
						If Not t.IsValueType Then
							Return t
						End If
						Return t.MakeByRefType()
					End Function
					selector2 = func2
					Interop.ClrInterop.Managed.<>c.<>9__5_1 = func2
				End If
				Dim list As Collections.Generic.List(Of Type) = source.[Select](selector2).ToList()
				Dim returnByRef As Boolean
				Dim returnType As Type = Interop.ClrInterop.Managed.WrapDelegateReturnType(method.ReturnType, list, returnByRef)
				Dim methodBuilder As Reflection.Emit.MethodBuilder = typeBuilder.DefineMethod("Invoke", Reflection.MethodAttributes.FamANDAssem Or Reflection.MethodAttributes.Family Or Reflection.MethodAttributes.Virtual Or Reflection.MethodAttributes.HideBySig Or Reflection.MethodAttributes.VtableLayoutMask)
				methodBuilder.SetSignature(returnType, Nothing, Nothing, list.ToArray(), Nothing, Nothing)
				methodBuilder.SetImplementationFlags(Reflection.MethodImplAttributes.CodeTypeMask)
				Interop.ClrInterop.Managed.WrapDelegateParameters(methodBuilder, method, returnByRef)
				Return typeBuilder.CreateType()
			End Function

			' Token: 0x0600015B RID: 347 RVA: 0x00004058 File Offset: 0x00002258
			Private Shared Sub WrapDelegateParameters(method As Reflection.Emit.MethodBuilder, originalMethod As Reflection.MethodInfo, returnByRef As Boolean)
				Dim num As Integer = 1
				For Each parameterInfo As Reflection.ParameterInfo In originalMethod.GetParameters()
					If Interop.ClrInterop.Managed.NeedsWrapping(parameterInfo) Then
						parameterInfo.ParameterType.MakeByRefType()
						method.DefineParameter(num, Reflection.ParameterAttributes.[In], parameterInfo.Name)
					Else
						method.DefineParameter(num, parameterInfo.Attributes, parameterInfo.Name)
					End If
					num += 1
				Next
				If returnByRef Then
					method.DefineParameter(0, Reflection.ParameterAttributes.Out, "retval")
				End If
			End Sub

			' Token: 0x0600015C RID: 348 RVA: 0x000040CE File Offset: 0x000022CE
			Private Shared Function NeedsWrapping(parameter As Reflection.ParameterInfo) As Boolean
				Return parameter.ParameterType.IsValueType
			End Function

			' Token: 0x0600015D RID: 349 RVA: 0x000040DC File Offset: 0x000022DC
			Private Shared Function WrapDelegateReturnType(originalType As Type, parameters As Collections.Generic.ICollection(Of Type), <System.Runtime.InteropServices.OutAttribute()> ByRef returnByRef As Boolean) As Type
				returnByRef = Interop.ClrInterop.Managed.NeedsReturnWrapping(originalType)
				If Not returnByRef Then
					Return originalType
				End If
				Dim item As Type = originalType.MakeByRefType()
				parameters.Add(item)
				Return GetType(Void)
			End Function

			' Token: 0x0600015E RID: 350 RVA: 0x0000410F File Offset: 0x0000230F
			Private Shared Function NeedsReturnWrapping(type As Type) As Boolean
				Return type.IsValueType AndAlso Not GetType(Void).Equals(type)
			End Function

			' Token: 0x0600015F RID: 351 RVA: 0x00004130 File Offset: 0x00002330
			Private Shared Function GenerateIdentifier() As String
				Return Guid.NewGuid().ToString().Replace("-"c, "_"c)
			End Function

			' Token: 0x06000160 RID: 352 RVA: 0x00004159 File Offset: 0x00002359
			' Note: this type is marked as 'beforefieldinit'.
			Shared Sub New()
			End Sub

			' Token: 0x0400005C RID: 92
			Private [module] As Reflection.Emit.ModuleBuilder

			' Token: 0x0400005D RID: 93
			Private Shared callingConventionAttribute As Reflection.Emit.CustomAttributeBuilder = New Reflection.Emit.CustomAttributeBuilder(GetType(Runtime.InteropServices.UnmanagedFunctionPointerAttribute).GetConstructor(New Type() { GetType(Runtime.InteropServices.CallingConvention) }), New Object() { Runtime.InteropServices.CallingConvention.Cdecl })

			' Token: 0x0400005E RID: 94
			Private Const InvokeAttributes As Reflection.MethodAttributes = Reflection.MethodAttributes.FamANDAssem Or Reflection.MethodAttributes.Family Or Reflection.MethodAttributes.Virtual Or Reflection.MethodAttributes.HideBySig Or Reflection.MethodAttributes.VtableLayoutMask

			' Token: 0x0400005F RID: 95
			Private Const RuntimeMethod As Reflection.MethodImplAttributes = Reflection.MethodImplAttributes.CodeTypeMask

			' Token: 0x0200003F RID: 63
			<Runtime.CompilerServices.CompilerGenerated()>
			<Serializable()>
			Private NotInheritable Class <>c
				' Token: 0x06000172 RID: 370 RVA: 0x000044F2 File Offset: 0x000026F2
				' Note: this type is marked as 'beforefieldinit'.
				Shared Sub New()
				End Sub

				' Token: 0x06000173 RID: 371 RVA: 0x000037DA File Offset: 0x000019DA
				Public Sub New()
				End Sub

				' Token: 0x06000174 RID: 372 RVA: 0x000044FE File Offset: 0x000026FE
				Friend Function <Unwrap>b__4_0(p As Reflection.ParameterInfo) As Linq.Expressions.ParameterExpression
					Return Linq.Expressions.Expression.Parameter(p.ParameterType, p.Name)
				End Function

				' Token: 0x06000175 RID: 373 RVA: 0x00004511 File Offset: 0x00002711
				Friend Function <WrapDelegateType>b__5_0(p As Reflection.ParameterInfo) As Type
					Return p.ParameterType
				End Function

				' Token: 0x06000176 RID: 374 RVA: 0x00004519 File Offset: 0x00002719
				Friend Function <WrapDelegateType>b__5_1(t As Type) As Type
					If Not t.IsValueType Then
						Return t
					End If
					Return t.MakeByRefType()
				End Function

				' Token: 0x04000068 RID: 104
				Public Shared <>9 As Interop.ClrInterop.Managed.<>c = New Interop.ClrInterop.Managed.<>c()

				' Token: 0x04000069 RID: 105
				Public Shared <>9__4_0 As Func(Of Reflection.ParameterInfo, Linq.Expressions.ParameterExpression)

				' Token: 0x0400006A RID: 106
				Public Shared <>9__5_0 As Func(Of Reflection.ParameterInfo, Type)

				' Token: 0x0400006B RID: 107
				Public Shared <>9__5_1 As Func(Of Type, Type)
			End Class
		End Class

		' Token: 0x0200003A RID: 58
		Friend Class Native
			' Token: 0x06000161 RID: 353 RVA: 0x00004198 File Offset: 0x00002398
			Public Function Wrap([function] As [Function], [module] As [Module], debug As Boolean) As [Function]
				Dim type As FunctionType = [function].Type
				Dim type2 As FunctionType = Me.WrapFunctionType(type)
				Dim function2 As [Function] = [module].CreateFunction(Guid.NewGuid().ToString().Replace("-", ""), type2)
				function2.CallingConvention = CallingConvention.C
				Dim block As Block = New Block("", [module].Context, function2)
				Dim gen As InstructionBuilder = New InstructionBuilder([module].Context, block)
				If debug Then
					Me.EmitDebugBreak(gen, [module])
				End If
				Dim callResult As [Call] = Interop.ClrInterop.Native.WrapCall([function], function2, gen)
				Interop.ClrInterop.Native.GenerateReturn(type, function2, gen, callResult)
				If debug Then
					function2.PrintToString()
				End If
				Return function2
			End Function

			' Token: 0x06000162 RID: 354 RVA: 0x00004234 File Offset: 0x00002434
			Private Sub EmitDebugBreak(gen As InstructionBuilder, [module] As [Module])
				Dim [function] As [Function] = [module].GetFunction("LLVMDebugBreak")
				If [function] Is Nothing Then
					Throw New NotSupportedException("Module must have LLVMDebugBreak function defined")
				End If
				gen.[Call]([function], New Value(-1) {})
			End Sub

			' Token: 0x06000163 RID: 355 RVA: 0x0000426C File Offset: 0x0000246C
			Friend Function WrapFunctionType(originalType As FunctionType) As FunctionType
				Dim list As Collections.Generic.List(Of Type) = originalType.ArgumentTypes.[Select](AddressOf Me.WrapArg).ToList()
				Dim flag As Boolean
				Return New FunctionType(Interop.ClrInterop.Native.WrapReturnType(originalType, list, flag), list.ToArray(), False)
			End Function

			' Token: 0x06000164 RID: 356 RVA: 0x000042AC File Offset: 0x000024AC
			Private Shared Sub GenerateReturn(originalType As FunctionType, wrapper As [Function], gen As InstructionBuilder, callResult As [Call])
				Dim kind As TypeKind = originalType.ReturnType.Kind
				If kind = TypeKind.Void Then
					gen.[Return]()
					Return
				End If
				If kind <> TypeKind.Pointer Then
					Dim argument As Argument = wrapper(originalType.ArgumentCount)
					argument.Name = "retval"
					gen.Store(callResult, argument)
					gen.[Return]()
					Return
				End If
				gen.[Return](callResult)
			End Sub

			' Token: 0x06000165 RID: 357 RVA: 0x00004308 File Offset: 0x00002508
			Private Shared Function WrapCall([function] As [Function], wrapper As [Function], gen As InstructionBuilder) As [Call]
				Dim argumentTypes As Type() = wrapper.Type.ArgumentTypes
				Dim argumentTypes2 As Type() = [function].Type.ArgumentTypes
				Dim list As Collections.Generic.List(Of Value) = New Collections.Generic.List(Of Value)()
				For i As Integer = 0 To argumentTypes2.Length - 1
					Dim argument As Argument = wrapper(i)
					If String.IsNullOrEmpty(argument.Name) Then
						argument.Name = "arg" + i
					End If
					If argumentTypes2(i).Kind = argumentTypes(i).Kind Then
						list.Add(argument)
					Else
						Dim item As Load = gen.Load(argument, "")
						list.Add(item)
					End If
				Next
				Return gen.[Call]([function], list.ToArray())
			End Function

			' Token: 0x06000166 RID: 358 RVA: 0x000043B0 File Offset: 0x000025B0
			Private Shared Function WrapReturnType(originalType As FunctionType, wrapperArgs As Collections.Generic.List(Of Type), <System.Runtime.InteropServices.OutAttribute()> ByRef returnByRef As Boolean) As Type
				Dim kind As TypeKind = originalType.ReturnType.Kind
				If kind = TypeKind.Void OrElse kind = TypeKind.Pointer Then
					returnByRef = False
					Return originalType.ReturnType
				End If
				returnByRef = True
				wrapperArgs.Add(PointerType.[Get](originalType.ReturnType, 0))
				Return Type.GetVoid(originalType.Context)
			End Function

			' Token: 0x06000167 RID: 359 RVA: 0x000043FC File Offset: 0x000025FC
			Private Function WrapArg(argType As Type) As Type
				Dim kind As TypeKind = argType.Kind
				If kind = TypeKind.Pointer Then
					Return argType
				End If
				Return PointerType.[Get](argType, 0)
			End Function

			' Token: 0x06000168 RID: 360 RVA: 0x000037DA File Offset: 0x000019DA
			Public Sub New()
			End Sub
		End Class
	End Class
End Namespace
