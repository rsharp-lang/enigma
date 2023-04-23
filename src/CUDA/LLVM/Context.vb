Imports System

Namespace LLVM
	' Token: 0x0200000D RID: 13
	Public Class Context
		Inherits Global.LLVM.ReferenceBase

		' Token: 0x06000015 RID: 21 RVA: 0x00002205 File Offset: 0x00000405
		Friend Sub New(contextRef As Global.System.IntPtr)
			MyBase.New(contextRef)
		End Sub

		' Token: 0x17000004 RID: 4
		' (get) Token: 0x06000016 RID: 22 RVA: 0x0000220E File Offset: 0x0000040E
		Public Shared ReadOnly Property [Global] As Global.LLVM.Context
			Get
				Return New Global.LLVM.Context(Global.LLVM.llvm.GetGlobalContext())
			End Get
		End Property
	End Class
End Namespace
