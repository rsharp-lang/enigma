Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.CompilerServices

Namespace LLVM
	' Token: 0x02000012 RID: 18
	Public Class FunctionType
		Inherits DerivedType

		' Token: 0x0600002D RID: 45 RVA: 0x000020FD File Offset: 0x000002FD
		Friend Sub New(typeref As Global.System.IntPtr)
			MyBase.New(typeref)
		End Sub

		' Token: 0x0600002E RID: 46 RVA: 0x00002594 File Offset: 0x00000794
		Private Shared Function functionType(ret As Type, Optional args As Type() = Nothing, Optional vararg As Boolean = False) As Global.System.IntPtr
			If args Is Nothing Then
				args = New Type(-1) {}
			End If
			Dim ret2 As Global.System.IntPtr = ret
			Dim source As Global.System.Collections.Generic.IEnumerable(Of Type) = args
			Dim <>9__1_ As Global.System.Func(Of Type, Global.System.IntPtr) = FunctionType.<>c.<>9__1_0
			Dim selector As Global.System.Func(Of Type, Global.System.IntPtr) = <>9__1_
			If <>9__1_ Is Nothing Then
				Dim func As Global.System.Func(Of Type, Global.System.IntPtr) = Function(arg As Type) arg
				selector = func
				FunctionType.<>c.<>9__1_0 = func
			End If
			Return llvm.FunctionType(ret2, source.[Select](selector).ToArray(), args.Length, vararg)
		End Function

		' Token: 0x0600002F RID: 47 RVA: 0x000025E5 File Offset: 0x000007E5
		Public Sub New(ret As Type, Optional args As Type() = Nothing, Optional vararg As Boolean = False)
			MyBase.New(FunctionType.functionType(ret, args, vararg))
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
				Dim array As Global.System.IntPtr() = New Global.System.IntPtr(Me.ArgumentCount - 1) {}
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
				Return "void -> " + Me.ReturnType
			End If
			Dim separator As String = " * "
			Dim argumentTypes As Global.System.Collections.Generic.IEnumerable(Of Type) = Me.ArgumentTypes
			Dim <>9__9_ As Global.System.Func(Of Type, String) = FunctionType.<>c.<>9__9_0
			Dim selector As Global.System.Func(Of Type, String) = <>9__9_
			If <>9__9_ Is Nothing Then
				Dim func As Global.System.Func(Of Type, String) = Function(t As Type) t.ToString()
				selector = func
				FunctionType.<>c.<>9__9_0 = func
			End If
			Return String.Join(separator, argumentTypes.[Select](selector)) + " -> " + Me.ReturnType
		End Function

		' Token: 0x06000034 RID: 52 RVA: 0x000026C0 File Offset: 0x000008C0
		Public Overrides Function Equals(obj As Object) As Boolean
			Dim functionType As FunctionType = TryCast(obj, FunctionType)
			Return functionType IsNot Nothing AndAlso functionType.ReturnType.Equals(Me.ReturnType) AndAlso Me.ArgumentTypes.SequenceEqual(functionType.ArgumentTypes)
		End Function

		' Token: 0x02000036 RID: 54
		<Global.System.Runtime.CompilerServices.CompilerGenerated()>
		<Global.System.Serializable()>
		Private NotInheritable Class <>c
			' Token: 0x06000149 RID: 329 RVA: 0x00003D73 File Offset: 0x00001F73
			' Note: this type is marked as 'beforefieldinit'.
			Shared Sub New()
			End Sub

			' Token: 0x0600014A RID: 330 RVA: 0x000037DA File Offset: 0x000019DA
			Public Sub New()
			End Sub

			' Token: 0x0600014B RID: 331 RVA: 0x00003D7F File Offset: 0x00001F7F
			Friend Function <functionType>b__1_0(arg As Type) As Global.System.IntPtr
				Return arg
			End Function

			' Token: 0x0600014C RID: 332 RVA: 0x00003D87 File Offset: 0x00001F87
			Friend Function <ToString>b__9_0(t As Type) As String
				Return t.ToString()
			End Function

			' Token: 0x04000052 RID: 82
			Public Shared <>9 As FunctionType.<>c = New FunctionType.<>c()

			' Token: 0x04000053 RID: 83
			Public Shared <>9__1_0 As Global.System.Func(Of Type, Global.System.IntPtr)

			' Token: 0x04000054 RID: 84
			Public Shared <>9__9_0 As Global.System.Func(Of Type, String)
		End Class
	End Class
End Namespace
