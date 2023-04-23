Imports System

Namespace LLVM
	' Token: 0x02000018 RID: 24
	Public Class IntegerType
		Inherits DerivedType

		' Token: 0x06000069 RID: 105 RVA: 0x000020FD File Offset: 0x000002FD
		Friend Sub New(typeref As IntPtr)
			MyBase.New(typeref)
		End Sub

		' Token: 0x0600006A RID: 106 RVA: 0x0000324D File Offset: 0x0000144D
		Public Shared Function GetInt32(context As Context) As IntegerType
			Return New IntegerType(llvm.GetInt32(context))
		End Function

		' Token: 0x0600006B RID: 107 RVA: 0x0000325F File Offset: 0x0000145F
		Public Shared Function [Get](context As Context, bits As Integer) As IntegerType
			Return New IntegerType(llvm.GetInt(context, bits))
		End Function

		' Token: 0x0600006C RID: 108 RVA: 0x00003272 File Offset: 0x00001472
		Public Function Constant(value As ULong, sign As Boolean) As IntegerConstant
			Return New IntegerConstant(llvm.Constant(Me, value, sign))
		End Function

		' Token: 0x1700000C RID: 12
		' (get) Token: 0x0600006D RID: 109 RVA: 0x00003286 File Offset: 0x00001486
		Public ReadOnly Property Width As Integer
			Get
				Return llvm.GetWidth(Me)
			End Get
		End Property

		' Token: 0x0600006E RID: 110 RVA: 0x00003293 File Offset: 0x00001493
		Public Overrides Function ToString() As String
			Return "i" + Me.Width
		End Function

		' Token: 0x0600006F RID: 111 RVA: 0x000032AC File Offset: 0x000014AC
		Public Overrides Function StructuralEquals(obj As Type) As Boolean
			If Me Is Nothing AndAlso obj Is Nothing Then
				Return True
			End If
			Dim integerType As IntegerType = TryCast(obj, IntegerType)
			Return integerType IsNot Nothing AndAlso Me.Width = integerType.Width
		End Function
	End Class
End Namespace
