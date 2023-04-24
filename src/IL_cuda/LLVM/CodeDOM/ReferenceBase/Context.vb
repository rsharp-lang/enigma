Imports System.Runtime.CompilerServices

Namespace LLVM
	' Token: 0x0200000D RID: 13
	Public Class Context : Inherits ReferenceBase

		' Token: 0x17000004 RID: 4
		' (get) Token: 0x06000016 RID: 22 RVA: 0x0000220E File Offset: 0x0000040E
		Public Shared ReadOnly Property [Global] As Context
			<MethodImpl(MethodImplOptions.AggressiveInlining)>
			Get
				Return New Context(llvm.GetGlobalContext())
			End Get
		End Property

		' Token: 0x06000015 RID: 21 RVA: 0x00002205 File Offset: 0x00000405
		Friend Sub New(contextRef As IntPtr)
			MyBase.New(contextRef)
		End Sub
	End Class
End Namespace
