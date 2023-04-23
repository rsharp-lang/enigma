Imports System

Namespace LLVM
	' Token: 0x02000006 RID: 6
	Public Class Block
		Inherits Global.LLVM.ReferenceBase

		' Token: 0x0600000B RID: 11 RVA: 0x00002167 File Offset: 0x00000367
		Public Sub New(name As String, context As Global.LLVM.Context, func As Global.LLVM.[Function])
			MyBase.New(Global.LLVM.llvm.CreateBlock(context, func, name))
		End Sub
	End Class
End Namespace
