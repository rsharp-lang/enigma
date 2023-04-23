Imports System

Namespace LLVM
	' Token: 0x02000020 RID: 32
	Public MustInherit Class ReferenceBase
		Implements IEquatable(Of ReferenceBase)

		' Token: 0x060000F1 RID: 241 RVA: 0x0000350A File Offset: 0x0000170A
		Protected Friend Sub New(reference As IntPtr)
			If reference = IntPtr.Zero Then
				Throw New ArgumentNullException("reference")
			End If
			Me.reference = reference
		End Sub

		' Token: 0x060000F2 RID: 242 RVA: 0x00003531 File Offset: 0x00001731
		Public Shared Widening Operator CType(reference As ReferenceBase) As IntPtr
			Return reference.reference
		End Operator

		' Token: 0x060000F3 RID: 243 RVA: 0x00003539 File Offset: 0x00001739
		Public Overloads Function Equals(other As ReferenceBase) As Boolean Implements System.IEquatable(Of LLVM.ReferenceBase).Equals
			Return other IsNot Nothing AndAlso Me.reference = other.reference
		End Function

		' Token: 0x060000F4 RID: 244 RVA: 0x00003551 File Offset: 0x00001751
		Public Overrides Overloads Function Equals(obj As Object) As Boolean
			Return Me.Equals(TryCast(obj, ReferenceBase))
		End Function

		' Token: 0x060000F5 RID: 245 RVA: 0x0000355F File Offset: 0x0000175F
		Public Overrides Function GetHashCode() As Integer
			Return Me.reference.GetHashCode() Xor 19142074
		End Function

		' Token: 0x060000F6 RID: 246 RVA: 0x00003572 File Offset: 0x00001772
		Public Overrides Function ToString() As String
			Return String.Format("{0}: {1}", MyBase.[GetType]().Name, Me.reference)
		End Function

		' Token: 0x0400001E RID: 30
		Private reference As IntPtr
	End Class
End Namespace
