Imports System
Imports System.Runtime.InteropServices

Namespace LLVM
	' Token: 0x0200000A RID: 10
	Public Module CallingConventionHelpers
		' Token: 0x06000012 RID: 18 RVA: 0x000021C0 File Offset: 0x000003C0
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function ToLLVM(convention As Runtime.InteropServices.CallingConvention) As CallingConvention
			Select Case convention
				Case Runtime.InteropServices.CallingConvention.Cdecl
					Return CallingConvention.C
				Case Runtime.InteropServices.CallingConvention.StdCall
					Return CallingConvention.StdCallX86
				Case Runtime.InteropServices.CallingConvention.ThisCall
					Throw New NotSupportedException()
				Case Runtime.InteropServices.CallingConvention.FastCall
					Return CallingConvention.FastCallX86
				Case Else
					Throw New NotImplementedException(convention.ToString())
			End Select
		End Function
	End Module
End Namespace
