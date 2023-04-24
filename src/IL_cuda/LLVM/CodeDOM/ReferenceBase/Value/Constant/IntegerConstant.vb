Imports System.Diagnostics.Contracts

Namespace LLVM
	' Token: 0x02000017 RID: 23
	Public Class IntegerConstant : Inherits Constant

		' Token: 0x06000067 RID: 103 RVA: 0x000026FF File Offset: 0x000008FF
		Friend Sub New(valueref As IntPtr)
			MyBase.New(valueref)
		End Sub

		' Token: 0x06000068 RID: 104 RVA: 0x0000322C File Offset: 0x0000142C
		Public Function ToPointer(pointer As PointerType) As PointerConstant
			Contract.Requires(Of ArgumentNullException)(pointer IsNot Nothing)
			Return New PointerConstant(llvm.ToPointer(Me, pointer))
		End Function
	End Class
End Namespace
