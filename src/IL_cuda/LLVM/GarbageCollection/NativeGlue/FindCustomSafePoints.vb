Imports System
Imports System.Runtime.InteropServices

Namespace LLVM.GarbageCollection.NativeGlue
	' Token: 0x02000034 RID: 52
	' (Invoke) Token: 0x06000142 RID: 322
	<Global.System.Runtime.InteropServices.UnmanagedFunctionPointer(Global.System.Runtime.InteropServices.CallingConvention.Cdecl)>
	Friend Delegate Function FindCustomSafePoints(functionInfo As Global.System.IntPtr, machineFunction As Global.System.IntPtr) As Boolean
End Namespace
