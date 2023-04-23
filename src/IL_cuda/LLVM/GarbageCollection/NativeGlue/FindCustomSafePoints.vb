Imports System
Imports System.Runtime.InteropServices

Namespace LLVM.GarbageCollection.NativeGlue
	' Token: 0x02000034 RID: 52
	' (Invoke) Token: 0x06000142 RID: 322
	<Runtime.InteropServices.UnmanagedFunctionPointer(Runtime.InteropServices.CallingConvention.Cdecl)>
	Friend Delegate Function FindCustomSafePoints(functionInfo As IntPtr, machineFunction As IntPtr) As Boolean
End Namespace
