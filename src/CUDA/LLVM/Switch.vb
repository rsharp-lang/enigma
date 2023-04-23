Imports System

Namespace LLVM
	' Token: 0x02000002 RID: 2
	Public Class Switch
		Inherits Global.LLVM.Terminator

		' Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		Friend Sub New(valueref As Global.System.IntPtr)
			MyBase.New(valueref)
		End Sub

		' Token: 0x06000002 RID: 2 RVA: 0x00002059 File Offset: 0x00000259
		Public Sub Add(value As Global.LLVM.Value, target As Global.LLVM.Block)
			Global.LLVM.llvm.SwitchAdd(Me, value, target)
		End Sub
	End Class
End Namespace
