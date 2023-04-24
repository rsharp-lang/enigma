Namespace LLVM
	' Token: 0x02000012 RID: 18
	Public Class FunctionType : Inherits DerivedType

		' Token: 0x0600002D RID: 45 RVA: 0x000020FD File Offset: 0x000002FD
		Friend Sub New(typeref As IntPtr)
			MyBase.New(typeref)
		End Sub

		' Token: 0x0600002E RID: 46 RVA: 0x00002594 File Offset: 0x00000794
		Private Shared Function functionType(ret As Type, Optional args As Type() = Nothing, Optional vararg As Boolean = False) As IntPtr
			If args Is Nothing Then
				args = New Type(-1) {}
			End If
			Dim ret2 As IntPtr = ret
			Dim source As IEnumerable(Of Type) = args
			Dim selector As Func(Of Type, IntPtr) = Function(arg As Type) arg
			Return llvm.FunctionType(ret2, source.[Select](selector).ToArray(), args.Length, vararg)
		End Function

		' Token: 0x0600002F RID: 47 RVA: 0x000025E5 File Offset: 0x000007E5
		Public Sub New(ret As Type, Optional args As Type() = Nothing, Optional vararg As Boolean = False)
			MyBase.New(functionType(ret, args, vararg))
		End Sub

		' Token: 0x17000008 RID: 8
		' (get) Token: 0x06000030 RID: 48 RVA: 0x000025F5 File Offset: 0x000007F5
		Public ReadOnly Property ArgumentCount As Integer
			Get
				Return llvm.GetArgumentCount(Me)
			End Get
		End Property

		' Token: 0x17000009 RID: 9
		' (get) Token: 0x06000031 RID: 49 RVA: 0x00002604 File Offset: 0x00000804
		Public ReadOnly Property ArgumentTypes As Type()
			Get
				Dim array As IntPtr() = New IntPtr(Me.ArgumentCount - 1) {}
				llvm.GetArgumentTypes(Me, array)
				Return array.[Select](AddressOf Type.DetectType).ToArray()
			End Get
		End Property

		' Token: 0x1700000A RID: 10
		' (get) Token: 0x06000032 RID: 50 RVA: 0x00002640 File Offset: 0x00000840
		Public ReadOnly Property ReturnType As Type
			Get
				Return Type.DetectType(llvm.GetReturnType(Me))
			End Get
		End Property

		' Token: 0x06000033 RID: 51 RVA: 0x00002654 File Offset: 0x00000854
		Public Overrides Function ToString() As String
			If Me.ArgumentCount <= 0 Then
				Return "void -> " & Me.ReturnType.ToString
			End If
			Dim separator As String = " * "
			Dim argumentTypes As IEnumerable(Of Type) = Me.ArgumentTypes
			Dim selector As Func(Of Type, String) = Function(t As Type) t.ToString()

			Return String.Join(separator, argumentTypes.[Select](selector)) & " -> " & Me.ReturnType.ToString
		End Function

		' Token: 0x06000034 RID: 52 RVA: 0x000026C0 File Offset: 0x000008C0
		Public Overrides Function Equals(obj As Object) As Boolean
			Dim functionType As FunctionType = TryCast(obj, FunctionType)
			Return functionType IsNot Nothing AndAlso functionType.ReturnType.Equals(Me.ReturnType) AndAlso Me.ArgumentTypes.SequenceEqual(functionType.ArgumentTypes)
		End Function
	End Class
End Namespace
