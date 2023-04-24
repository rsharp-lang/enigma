Namespace LLVM
	' Token: 0x02000004 RID: 4
	Public Class FloatType : Inherits DerivedType

		' Token: 0x06000008 RID: 8 RVA: 0x000020FD File Offset: 0x000002FD
		Friend Sub New(typeref As IntPtr)
			MyBase.New(typeref)
		End Sub

		' Token: 0x06000009 RID: 9 RVA: 0x00002108 File Offset: 0x00000308
		Public Shared Function [Get](context As Context, bits As Integer) As FloatType
			Dim typeref As IntPtr
			If bits <> 32 Then
				If bits <> 64 Then
					Throw New NotSupportedException("Floats of size " + bits + " bits")
				End If
				typeref = llvm.GetDouble(context)
			Else
				typeref = llvm.GetFloat(context)
			End If
			Return New FloatType(typeref)
		End Function
	End Class
End Namespace
