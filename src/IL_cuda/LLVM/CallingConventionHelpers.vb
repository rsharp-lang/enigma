Imports System
Imports System.Runtime.InteropServices

Namespace LLVM
	' Token: 0x0200000A RID: 10
	Public Module CallingConventionHelpers
		' Token: 0x06000012 RID: 18 RVA: 0x000021C0 File Offset: 0x000003C0
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function ToLLVM(convention As Global.System.Runtime.InteropServices.CallingConvention) As Global.LLVM.CallingConvention
			Select Case convention
				Case Global.System.Runtime.InteropServices.CallingConvention.Cdecl
					Return Global.LLVM.CallingConvention.C
				Case Global.System.Runtime.InteropServices.CallingConvention.StdCall
					Return Global.LLVM.CallingConvention.StdCallX86
				Case Global.System.Runtime.InteropServices.CallingConvention.ThisCall
					Throw New Global.System.NotSupportedException()
				Case Global.System.Runtime.InteropServices.CallingConvention.FastCall
					Return Global.LLVM.CallingConvention.FastCallX86
				Case Else
					Throw New Global.System.NotImplementedException(convention.ToString())
			End Select
		End Function
	End Module
End Namespace
