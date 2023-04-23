Imports System.Runtime.InteropServices

Namespace LLVM
	' Token: 0x02000010 RID: 16
	' (Invoke) Token: 0x06000022 RID: 34
	<UnmanagedFunctionPointer(Runtime.InteropServices.CallingConvention.Cdecl)>
	Public Delegate Function LazyFunctionLoader(name As String) As IntPtr
End Namespace
