Imports System
Imports System.Diagnostics.Contracts
Imports System.Runtime.InteropServices

Namespace LLVM.GarbageCollection
	' Token: 0x02000030 RID: 48
	Public Class ShadowStack
		Inherits GarbageCollection.GarbageCollector

		' Token: 0x06000130 RID: 304 RVA: 0x00003BF4 File Offset: 0x00001DF4
		Public Sub New(engine As ExecutionEngine, [module] As [Module], rootVisitor As GarbageCollection.ShadowStack.RootVisitor)
			Diagnostics.Contracts.Contract.Requires(Of ArgumentNullException)(rootVisitor IsNot Nothing)
			Diagnostics.Contracts.Contract.Requires(Of ArgumentNullException)(engine IsNot Nothing)
			Diagnostics.Contracts.Contract.Requires(Of ArgumentNullException)([module] IsNot Nothing)
			Me.rootVisitor = rootVisitor
			Me.[module] = [module]
			Me.engine = engine
			Me.rootPointer = [module].AddGlobal(Type.GetVoid([module].Context), "llvm_gc_root_chain", Nothing)
		End Sub

		' Token: 0x06000131 RID: 305 RVA: 0x00003C54 File Offset: 0x00001E54
		Public Sub VisitRoots()
			Me.VisitRoots(Me.rootVisitor)
		End Sub

		' Token: 0x06000132 RID: 306 RVA: 0x00003C64 File Offset: 0x00001E64
		Private Function GetRoot() As GarbageCollection.ShadowStack.StackEntry
			Dim value As IntPtr = Me.engine.GetValue(Of IntPtr)(Me.rootPointer)
			If Not value.IsNull() Then
				Return CType(Runtime.InteropServices.Marshal.PtrToStructure(value, GetType(GarbageCollection.ShadowStack.StackEntry)), GarbageCollection.ShadowStack.StackEntry)
			End If
			Return Nothing
		End Function

		' Token: 0x06000133 RID: 307 RVA: 0x00003CAC File Offset: 0x00001EAC
		Public Sub VisitRoots(visitor As GarbageCollection.ShadowStack.RootVisitor)
			Diagnostics.Contracts.Contract.Requires(Of ArgumentNullException)(visitor IsNot Nothing)
			Dim stackEntry As GarbageCollection.ShadowStack.StackEntry = Me.GetRoot()
			While stackEntry.FrameMap <> IntPtr.Zero
				Dim i As Integer = 0
				Dim frameMap As GarbageCollection.ShadowStack.FrameMap = stackEntry.GetFrameMap()
				Dim metaCount As Integer = frameMap.MetaCount
				While i < metaCount
					visitor(stackEntry.Roots + i * IntPtr.Size, frameMap.Meta + i * IntPtr.Size)
					i += 1
				End While
				Dim rootCount As Integer = frameMap.RootCount
				While i < rootCount
					visitor(stackEntry.Roots + i * IntPtr.Size, IntPtr.Zero)
					i += 1
				End While
				stackEntry = stackEntry.GetNext()
			End While
		End Sub

		' Token: 0x17000023 RID: 35
		' (get) Token: 0x06000134 RID: 308 RVA: 0x00003D5D File Offset: 0x00001F5D
		Public ReadOnly Property [Module] As [Module]
			Get
				Return Me.[module]
			End Get
		End Property

		' Token: 0x06000135 RID: 309 RVA: 0x00003D65 File Offset: 0x00001F65
		Protected Overrides Function InitializeCustomLowering([module] As [Module]) As Boolean
			Throw New NotSupportedException()
		End Function

		' Token: 0x06000136 RID: 310 RVA: 0x00003D65 File Offset: 0x00001F65
		Protected Overrides Function PerformCustomLowering([function] As [Function]) As Boolean
			Throw New NotSupportedException()
		End Function

		' Token: 0x06000137 RID: 311 RVA: 0x00003D65 File Offset: 0x00001F65
		Protected Overrides Function FindCustomSafePoints(functionInfo As IntPtr, machineFunction As IntPtr) As Boolean
			Throw New NotSupportedException()
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
		Private rootVisitor As GarbageCollection.ShadowStack.RootVisitor

		' Token: 0x04000045 RID: 69
		Private rootPointer As GlobalValue

		' Token: 0x04000046 RID: 70
		Private engine As ExecutionEngine

		' Token: 0x04000047 RID: 71
		Private [module] As [Module]

		' Token: 0x0200003C RID: 60
		' (Invoke) Token: 0x0600016D RID: 365
		<Runtime.InteropServices.UnmanagedFunctionPointer(Runtime.InteropServices.CallingConvention.Cdecl)>
		Public Delegate Sub RootVisitor(root As IntPtr, meta As IntPtr)

		' Token: 0x0200003D RID: 61
		Private Structure FrameMap
			' Token: 0x04000062 RID: 98
			Public RootCount As Integer

			' Token: 0x04000063 RID: 99
			Public MetaCount As Integer

			' Token: 0x04000064 RID: 100
			Public Meta As IntPtr
		End Structure

		' Token: 0x0200003E RID: 62
		Private Structure StackEntry
			' Token: 0x06000170 RID: 368 RVA: 0x00004490 File Offset: 0x00002690
			Public Function GetNext() As GarbageCollection.ShadowStack.StackEntry
				Diagnostics.Contracts.Contract.Requires(Of InvalidOperationException)(Me.[Next] <> IntPtr.Zero)
				Return CType(Runtime.InteropServices.Marshal.PtrToStructure(Me.[Next], GetType(GarbageCollection.ShadowStack.StackEntry)), GarbageCollection.ShadowStack.StackEntry)
			End Function

			' Token: 0x06000171 RID: 369 RVA: 0x000044C1 File Offset: 0x000026C1
			Public Function GetFrameMap() As GarbageCollection.ShadowStack.FrameMap
				Diagnostics.Contracts.Contract.Requires(Of InvalidOperationException)(Me.FrameMap <> IntPtr.Zero)
				Return CType(Runtime.InteropServices.Marshal.PtrToStructure(Me.FrameMap, GetType(GarbageCollection.ShadowStack.FrameMap)), GarbageCollection.ShadowStack.FrameMap)
			End Function

			' Token: 0x04000065 RID: 101
			Public [Next] As IntPtr

			' Token: 0x04000066 RID: 102
			Public FrameMap As IntPtr

			' Token: 0x04000067 RID: 103
			Public Roots As IntPtr
		End Structure
	End Class
End Namespace
