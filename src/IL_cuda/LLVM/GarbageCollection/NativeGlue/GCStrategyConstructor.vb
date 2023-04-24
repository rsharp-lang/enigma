Imports System.Runtime
Imports System.Runtime.InteropServices

Namespace LLVM.GarbageCollection.NativeGlue
    ' Token: 0x02000035 RID: 53
    ' (Invoke) Token: 0x06000146 RID: 326
    <UnmanagedFunctionPointer(InteropServices.CallingConvention.Cdecl)>
    Friend Delegate Function GCStrategyConstructor() As IntPtr
End Namespace
