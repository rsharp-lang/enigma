Imports System

Namespace LLVM
	' Token: 0x02000003 RID: 3
	Public Class ArrayType
		Inherits CompositeType

		' Token: 0x06000003 RID: 3 RVA: 0x00002073 File Offset: 0x00000273
		Friend Sub New(typeref As Global.System.IntPtr)
			MyBase.New(typeref)
		End Sub

		' Token: 0x06000004 RID: 4 RVA: 0x0000207C File Offset: 0x0000027C
		Public Sub New(elementType As Type, Optional elementCount As Integer = 0)
			Me.New(ArrayType.arrayType(elementType, elementCount))
		End Sub

		' Token: 0x06000005 RID: 5 RVA: 0x0000208B File Offset: 0x0000028B
		Private Shared Function arrayType(elemType As Type, elementCount As Integer) As Global.System.IntPtr
			Return llvm.ArrayType(elemType, elementCount)
		End Function

		' Token: 0x17000001 RID: 1
		' (get) Token: 0x06000006 RID: 6 RVA: 0x00002099 File Offset: 0x00000299
		Public ReadOnly Property ElementType As Type
			Get
				Return Type.DetectType(llvm.GetElementType(Me))
			End Get
		End Property

		' Token: 0x06000007 RID: 7 RVA: 0x000020AC File Offset: 0x000002AC
		Public Overrides Function ToString() As String
			Dim elementType As Type = Me.ElementType
			If TypeOf elementType Is DerivedType Then
				Return "[" + elementType.[GetType]().Name + "]"
			End If
			Return "[" + Me.ElementType.ToString() + "]"
		End Function
	End Class
End Namespace
