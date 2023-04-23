Imports System

Namespace LLVM
	' Token: 0x02000026 RID: 38
	Public Class Target
		' Token: 0x0600010C RID: 268 RVA: 0x000037C6 File Offset: 0x000019C6
		Public Shared Sub InitializeNative()
			If Not llvm.InitializeNative() Then
				Throw New Global.System.NotSupportedException("There's no native platform here")
			End If
		End Sub

		' Token: 0x0600010D RID: 269 RVA: 0x000037DA File Offset: 0x000019DA
		Public Sub New()
		End Sub
	End Class
End Namespace
