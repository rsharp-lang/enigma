﻿Imports System.Runtime
Imports System.Runtime.InteropServices

Namespace LLVM.GarbageCollection.NativeGlue
    ' Token: 0x02000033 RID: 51
    ' (Invoke) Token: 0x0600013E RID: 318
    <UnmanagedFunctionPointer(InteropServices.CallingConvention.Cdecl)>
    Friend Delegate Function PerformCustomLowering([function] As IntPtr) As Boolean
End Namespace
