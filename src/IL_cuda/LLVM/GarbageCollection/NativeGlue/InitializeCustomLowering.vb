﻿Imports System.Runtime
Imports System.Runtime.InteropServices

Namespace LLVM.GarbageCollection.NativeGlue
    ' Token: 0x02000032 RID: 50
    ' (Invoke) Token: 0x0600013A RID: 314
    <UnmanagedFunctionPointer(InteropServices.CallingConvention.Cdecl)>
    Friend Delegate Function InitializeCustomLowering([module] As IntPtr) As Boolean
End Namespace
