Imports System

Namespace LLVM
	' Token: 0x0200001F RID: 31
	Public Class PointerType : Inherits SequentialType

		' Token: 0x17000014 RID: 20
		' (get) Token: 0x060000EE RID: 238 RVA: 0x00002099 File Offset: 0x00000299
		Public ReadOnly Property ElementType As Type
			Get
				Return Type.DetectType(llvm.GetElementType(Me))
			End Get
		End Property

		' Token: 0x060000EC RID: 236 RVA: 0x0000346E File Offset: 0x0000166E
		Friend Sub New(typeref As IntPtr)
			MyBase.New(typeref)
		End Sub

		' Token: 0x060000ED RID: 237 RVA: 0x00003477 File Offset: 0x00001677
		Public Shared Function [Get](valueType As Type, Optional addressSpace As Integer = 0) As PointerType
			Return New PointerType(llvm.GetPointerType(valueType, addressSpace))
		End Function

		' Token: 0x060000EF RID: 239 RVA: 0x0000348C File Offset: 0x0000168C
		Public Overrides Function ToString() As String
			Dim elementType As Type = Me.ElementType
			If TypeOf elementType Is DerivedType AndAlso Not(TypeOf elementType Is PointerType) Then
				Return elementType.[GetType]().Name + "*"
			End If
			Return Me.ElementType + "*"
		End Function

		' Token: 0x060000F0 RID: 240 RVA: 0x000034D8 File Offset: 0x000016D8
		Public Overrides Function StructuralEquals(obj As Type) As Boolean
			If obj Is Nothing AndAlso Me Is Nothing Then
				Return True
			End If
			Dim pointerType As PointerType = TryCast(obj, PointerType)
			Return pointerType IsNot Nothing AndAlso Me.ElementType.Equals(pointerType.ElementType)
		End Function
	End Class
End Namespace
