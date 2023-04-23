Imports System

Namespace LLVM
	' Token: 0x02000004 RID: 4
	Public Class FloatType
		Inherits Global.LLVM.DerivedType

		' Token: 0x06000008 RID: 8 RVA: 0x000020FD File Offset: 0x000002FD
		Friend Sub New(typeref As Global.System.IntPtr)
			MyBase.New(typeref)
		End Sub

		' Token: 0x06000009 RID: 9 RVA: 0x00002108 File Offset: 0x00000308
		Public Shared Function [Get](context As Global.LLVM.Context, bits As Integer) As Global.LLVM.FloatType
			Dim typeref As Global.System.IntPtr
			If bits <> 32 Then
				If bits <> 64 Then
					Throw New Global.System.NotSupportedException("Floats of size " + bits + " bits")
				End If
				typeref = Global.LLVM.llvm.GetDouble(context)
			Else
				typeref = Global.LLVM.llvm.GetFloat(context)
			End If
			Return New Global.LLVM.FloatType(typeref)
		End Function
	End Class
End Namespace
