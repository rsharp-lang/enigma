Imports System
Imports System.Diagnostics.Contracts
Imports System.Runtime.InteropServices

Namespace LLVM.GarbageCollection
	' Token: 0x02000030 RID: 48
	Public Class ShadowStack
		Inherits Global.LLVM.GarbageCollection.GarbageCollector

		' Token: 0x06000130 RID: 304 RVA: 0x00003BF4 File Offset: 0x00001DF4
		Public Sub New(engine As Global.LLVM.ExecutionEngine, [module] As Global.LLVM.[Module], rootVisitor As Global.LLVM.GarbageCollection.ShadowStack.RootVisitor)
			Global.System.Diagnostics.Contracts.Contract.Requires(Of Global.System.ArgumentNullException)(rootVisitor IsNot Nothing)
			Global.System.Diagnostics.Contracts.Contract.Requires(Of Global.System.ArgumentNullException)(engine IsNot Nothing)
			Global.System.Diagnostics.Contracts.Contract.Requires(Of Global.System.ArgumentNullException)([module] IsNot Nothing)
			Me.rootVisitor = rootVisitor
			Me.[module] = [module]
			Me.engine = engine
			Me.rootPointer = [module].AddGlobal(Global.LLVM.Type.GetVoid([module].Context), "llvm_gc_root_chain", Nothing)
		End Sub

		' Token: 0x06000131 RID: 305 RVA: 0x00003C54 File Offset: 0x00001E54
		Public Sub VisitRoots()
			Me.VisitRoots(Me.rootVisitor)
		End Sub

		' Token: 0x06000132 RID: 306 RVA: 0x00003C64 File Offset: 0x00001E64
		Private Function GetRoot() As Global.LLVM.GarbageCollection.ShadowStack.StackEntry
			Dim value As Global.System.IntPtr = Me.engine.GetValue(Of Global.System.IntPtr)(Me.rootPointer)
			If Not value.IsNull() Then
				Return CType(Global.System.Runtime.InteropServices.Marshal.PtrToStructure(value, GetType(Global.LLVM.GarbageCollection.ShadowStack.StackEntry)), Global.LLVM.GarbageCollection.ShadowStack.StackEntry)
			End If
			Return Nothing
		End Function

		' Token: 0x06000133 RID: 307 RVA: 0x00003CAC File Offset: 0x00001EAC
		Public Sub VisitRoots(visitor As Global.LLVM.GarbageCollection.ShadowStack.RootVisitor)
			Global.System.Diagnostics.Contracts.Contract.Requires(Of Global.System.ArgumentNullException)(visitor IsNot Nothing)
			Dim stackEntry As Global.LLVM.GarbageCollection.ShadowStack.StackEntry = Me.GetRoot()
			While stackEntry.FrameMap <> Global.System.IntPtr.Zero
				Dim i As Integer = 0
				Dim frameMap As Global.LLVM.GarbageCollection.ShadowStack.FrameMap = stackEntry.GetFrameMap()
				Dim metaCount As Integer = frameMap.MetaCount
				While i < metaCount
					visitor(stackEntry.Roots + i * Global.System.IntPtr.Size, frameMap.Meta + i * Global.System.IntPtr.Size)
					i += 1
				End While
				Dim rootCount As Integer = frameMap.RootCount
				While i < rootCount
					visitor(stackEntry.Roots + i * Global.System.IntPtr.Size, Global.System.IntPtr.Zero)
					i += 1
				End While
				stackEntry = stackEntry.GetNext()
			End While
		End Sub

		' Token: 0x17000023 RID: 35
		' (get) Token: 0x06000134 RID: 308 RVA: 0x00003D5D File Offset: 0x00001F5D
		Public ReadOnly Property [Module] As Global.LLVM.[Module]
			Get
				Return Me.[module]
			End Get
		End Property

		' Token: 0x06000135 RID: 309 RVA: 0x00003D65 File Offset: 0x00001F65
		Protected Overrides Function InitializeCustomLowering([module] As Global.LLVM.[Module]) As Boolean
			Throw New Global.System.NotSupportedException()
		End Function

		' Token: 0x06000136 RID: 310 RVA: 0x00003D65 File Offset: 0x00001F65
		Protected Overrides Function PerformCustomLowering([function] As Global.LLVM.[Function]) As Boolean
			Throw New Global.System.NotSupportedException()
		End Function

		' Token: 0x06000137 RID: 311 RVA: 0x00003D65 File Offset: 0x00001F65
		Protected Overrides Function FindCustomSafePoints(functionInfo As Global.System.IntPtr, machineFunction As Global.System.IntPtr) As Boolean
			Throw New Global.System.NotSupportedException()
		End Function

		' Token: 0x17000024 RID: 36
		' (get) Token: 0x06000138 RID: 312 RVA: 0x00003D6C File Offset: 0x00001F6C
		Public Overrides ReadOnly Property Name As String
			Get
				Return "shadow-stack"
			End Get
		End Property

		' Token: 0x04000043 RID: 67
		Private Const rootName As String = "llvm_gc_root_chain"

		' Token: 0x04000044 RID: 68
		Private rootVisitor As Global.LLVM.GarbageCollection.ShadowStack.RootVisitor

		' Token: 0x04000045 RID: 69
		Private rootPointer As Global.LLVM.GlobalValue

		' Token: 0x04000046 RID: 70
		Private engine As Global.LLVM.ExecutionEngine

		' Token: 0x04000047 RID: 71
		Private [module] As Global.LLVM.[Module]

		' Token: 0x0200003C RID: 60
		' (Invoke) Token: 0x0600016D RID: 365
		<Global.System.Runtime.InteropServices.UnmanagedFunctionPointer(Global.System.Runtime.InteropServices.CallingConvention.Cdecl)>
		Public Delegate Sub RootVisitor(root As Global.System.IntPtr, meta As Global.System.IntPtr)

		' Token: 0x0200003D RID: 61
		Private Structure FrameMap
			' Token: 0x04000062 RID: 98
			Public RootCount As Integer

			' Token: 0x04000063 RID: 99
			Public MetaCount As Integer

			' Token: 0x04000064 RID: 100
			Public Meta As Global.System.IntPtr
		End Structure

		' Token: 0x0200003E RID: 62
		Private Structure StackEntry
			' Token: 0x06000170 RID: 368 RVA: 0x00004490 File Offset: 0x00002690
			Public Function GetNext() As Global.LLVM.GarbageCollection.ShadowStack.StackEntry
				Global.System.Diagnostics.Contracts.Contract.Requires(Of Global.System.InvalidOperationException)(Me.[Next] <> Global.System.IntPtr.Zero)
				Return CType(Global.System.Runtime.InteropServices.Marshal.PtrToStructure(Me.[Next], GetType(Global.LLVM.GarbageCollection.ShadowStack.StackEntry)), Global.LLVM.GarbageCollection.ShadowStack.StackEntry)
			End Function

			' Token: 0x06000171 RID: 369 RVA: 0x000044C1 File Offset: 0x000026C1
			Public Function GetFrameMap() As Global.LLVM.GarbageCollection.ShadowStack.FrameMap
				Global.System.Diagnostics.Contracts.Contract.Requires(Of Global.System.InvalidOperationException)(Me.FrameMap <> Global.System.IntPtr.Zero)
				Return CType(Global.System.Runtime.InteropServices.Marshal.PtrToStructure(Me.FrameMap, GetType(Global.LLVM.GarbageCollection.ShadowStack.FrameMap)), Global.LLVM.GarbageCollection.ShadowStack.FrameMap)
			End Function

			' Token: 0x04000065 RID: 101
			Public [Next] As Global.System.IntPtr

			' Token: 0x04000066 RID: 102
			Public FrameMap As Global.System.IntPtr

			' Token: 0x04000067 RID: 103
			Public Roots As Global.System.IntPtr
		End Structure
	End Class
End Namespace
