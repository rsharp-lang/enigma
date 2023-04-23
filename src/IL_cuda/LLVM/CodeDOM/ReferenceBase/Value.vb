Imports System

Namespace LLVM
	' Token: 0x0200002C RID: 44
	Public Class Value : Inherits ReferenceBase

		' Token: 0x0600011C RID: 284 RVA: 0x00002205 File Offset: 0x00000405
		Friend Sub New(valueref As IntPtr)
			MyBase.New(valueref)
		End Sub

		' Token: 0x17000020 RID: 32
		' (get) Token: 0x0600011D RID: 285 RVA: 0x0000392B File Offset: 0x00001B2B
		Public ReadOnly Property Type As Type
			Get
				Return Type.DetectType(llvm.[TypeOf](Me))
			End Get
		End Property

		' Token: 0x0600011E RID: 286 RVA: 0x0000393D File Offset: 0x00001B3D
		Public Overrides Function ToString() As String
			Return String.Format("{0}: {1}", Me.Type, Me)
		End Function

		' Token: 0x0600011F RID: 287 RVA: 0x0000395A File Offset: 0x00001B5A
		Public Sub Dump()
			llvm.Dump(Me)
		End Sub

		' Token: 0x06000120 RID: 288 RVA: 0x00003967 File Offset: 0x00001B67
		Public Function PrintToString() As String
			Return llvm.Print(Me)
		End Function

		' Token: 0x17000021 RID: 33
		' (get) Token: 0x06000121 RID: 289 RVA: 0x00003974 File Offset: 0x00001B74
		' (set) Token: 0x06000122 RID: 290 RVA: 0x00003981 File Offset: 0x00001B81
		Public Property Name As String
			Get
				Return llvm.GetName(Me)
			End Get
			Set(value As String)
				llvm.SetName(Me, value)
			End Set
		End Property
	End Class
End Namespace
