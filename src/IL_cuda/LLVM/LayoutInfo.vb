Imports System

Namespace LLVM
	' Token: 0x0200001A RID: 26
	Public Structure LayoutInfo
		' Token: 0x06000072 RID: 114 RVA: 0x000032F5 File Offset: 0x000014F5
		Public Sub New(size As Integer, align As Integer)
			Me.size = size
			Me.align = align
		End Sub

		' Token: 0x06000073 RID: 115 RVA: 0x00003305 File Offset: 0x00001505
		Public Sub New(size As Integer)
			Me = New LayoutInfo(size, size)
		End Sub

		' Token: 0x1700000D RID: 13
		' (get) Token: 0x06000074 RID: 116 RVA: 0x0000330F File Offset: 0x0000150F
		' (set) Token: 0x06000075 RID: 117 RVA: 0x00003317 File Offset: 0x00001517
		Public Property Size As Integer
			Get
				Return Me.size
			End Get
			Set(value As Integer)
				Me.size = value
			End Set
		End Property

		' Token: 0x1700000E RID: 14
		' (get) Token: 0x06000076 RID: 118 RVA: 0x00003320 File Offset: 0x00001520
		' (set) Token: 0x06000077 RID: 119 RVA: 0x00003328 File Offset: 0x00001528
		Public Property Align As Integer
			Get
				Return Me.align
			End Get
			Set(value As Integer)
				Me.align = value
			End Set
		End Property

		' Token: 0x0400001B RID: 27
		Private size As Integer

		' Token: 0x0400001C RID: 28
		Private align As Integer
	End Structure
End Namespace
