Imports System

Namespace LLVM
	' Token: 0x02000023 RID: 35
	Public Class StackAlloc
		Inherits Global.LLVM.Unary

		' Token: 0x060000F9 RID: 249 RVA: 0x00003594 File Offset: 0x00001794
		Friend Sub New(valueref As Global.System.IntPtr)
			MyBase.New(valueref)
		End Sub

		' Token: 0x17000015 RID: 21
		' (get) Token: 0x060000FA RID: 250 RVA: 0x0000359D File Offset: 0x0000179D
		Public ReadOnly Property Type As Global.LLVM.PointerType
			Get
				Return CType(MyBase.Type, Global.LLVM.PointerType)
			End Get
		End Property
	End Class
End Namespace
