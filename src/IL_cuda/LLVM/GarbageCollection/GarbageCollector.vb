﻿Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports LLVM.GarbageCollection.NativeGlue

Namespace LLVM.GarbageCollection
	' Token: 0x0200002E RID: 46
	Public MustInherit Class GarbageCollector
		' Token: 0x06000126 RID: 294 RVA: 0x00003A20 File Offset: 0x00001C20
		Protected Sub New()
			Me.findCustomSafePoints = Global.System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(AddressOf Me.FindCustomSafePoints)
			Me.initializeCustomLowering = Global.System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(AddressOf Me.InitializeCustomLowering)
			Me.performCustomLowering = Global.System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(AddressOf Me.PerformCustomLowering)
		End Sub

		' Token: 0x06000127 RID: 295
		Protected MustOverride Function InitializeCustomLowering([module] As Global.LLVM.[Module]) As Boolean

		' Token: 0x06000128 RID: 296 RVA: 0x00003A80 File Offset: 0x00001C80
		Private Function InitializeCustomLowering([module] As Global.System.IntPtr) As Boolean
			Return Me.InitializeCustomLowering(New Global.LLVM.[Module]([module]))
		End Function

		' Token: 0x06000129 RID: 297
		Protected MustOverride Function PerformCustomLowering([function] As Global.LLVM.[Function]) As Boolean

		' Token: 0x0600012A RID: 298 RVA: 0x00003A8E File Offset: 0x00001C8E
		Private Function PerformCustomLowering([function] As Global.System.IntPtr) As Boolean
			Return Me.PerformCustomLowering(New Global.LLVM.[Function]([function]))
		End Function

		' Token: 0x0600012B RID: 299
		Protected MustOverride Function FindCustomSafePoints(functionInfo As Global.System.IntPtr, machineFunction As Global.System.IntPtr) As Boolean

		' Token: 0x17000022 RID: 34
		' (get) Token: 0x0600012C RID: 300
		Public MustOverride ReadOnly Property Name As String

		' Token: 0x0600012D RID: 301 RVA: 0x00003A9C File Offset: 0x00001C9C
		Private Function GetExternalGCInfo() As Global.LLVM.GarbageCollection.NativeGlue.ExternalGCInfo
			Return New Global.LLVM.GarbageCollection.NativeGlue.ExternalGCInfo() With { .CustomReadBarriers = Me.CustomReadBarriers, .CustomRoots = Me.CustomRoots, .CustomSafePoints = Me.CustomSafePoints, .CustomWriteBarriers = Me.CustomWriteBarriers, .FindCustomSafePoints = Me.findCustomSafePoints, .InitializeCustomLowering = Me.initializeCustomLowering, .InitRoots = Me.InitRoots, .NeededSafePoints = Me.NeededSafePoints, .PerformCustomLowering = Me.performCustomLowering, .UsesMetadata = Me.UsesMetadata }
		End Function

		' Token: 0x0600012E RID: 302 RVA: 0x00003B34 File Offset: 0x00001D34
		Public Shared Sub Register(Of GC As{Global.LLVM.GarbageCollection.GarbageCollector, New})()
			Dim <>9__21_ As Global.LLVM.GarbageCollection.NativeGlue.GCStrategyConstructor = Global.LLVM.GarbageCollection.GarbageCollector.<>c__21(Of !!0).<>9__21_0
			Dim gcstrategyConstructor As Global.LLVM.GarbageCollection.NativeGlue.GCStrategyConstructor = <>9__21_
			If <>9__21_ Is Nothing Then
				Dim gcstrategyConstructor2 As Global.LLVM.GarbageCollection.NativeGlue.GCStrategyConstructor = Function()
					Dim obj2 As Global.System.Collections.Generic.List(Of Global.LLVM.GarbageCollection.GarbageCollector) = Global.LLVM.GarbageCollection.GarbageCollector.collectors
					Dim result As Global.System.IntPtr
					SyncLock obj2
						Dim gc As GC = Global.System.Activator.CreateInstance(Of GC)()
						Dim intPtr As Global.System.IntPtr = Global.LLVM.llvm.CreateGC(gc.GetExternalGCInfo())
						Global.LLVM.GarbageCollection.GarbageCollector.collectors.Add(gc)
						result = intPtr
					End SyncLock
					Return result
				End Function
				gcstrategyConstructor = gcstrategyConstructor2
				Global.LLVM.GarbageCollection.GarbageCollector.<>c__21(Of !!0).<>9__21_0 = gcstrategyConstructor2
			End If
			Dim gcstrategyConstructor3 As Global.LLVM.GarbageCollection.NativeGlue.GCStrategyConstructor = gcstrategyConstructor
			Dim fullName As String = GetType(!!0).FullName
			Dim name As Global.System.IntPtr = Global.System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(fullName)
			Dim obj As Global.System.Collections.Generic.Dictionary(Of String, Global.LLVM.GarbageCollection.NativeGlue.GCStrategyConstructor) = Global.LLVM.GarbageCollection.GarbageCollector.constructors
			SyncLock obj
				Global.LLVM.GarbageCollection.GarbageCollector.constructors.Add(fullName, gcstrategyConstructor3)
				Try
					Global.LLVM.llvm.RegisterGC(name, Global.System.IntPtr.Zero, gcstrategyConstructor3)
				Catch ex As Global.System.Exception
					Global.LLVM.GarbageCollection.GarbageCollector.constructors.Remove(fullName)
					Throw
				End Try
			End SyncLock
		End Sub

		' Token: 0x0600012F RID: 303 RVA: 0x00003BDC File Offset: 0x00001DDC
		' Note: this type is marked as 'beforefieldinit'.
		Shared Sub New()
		End Sub

		' Token: 0x04000031 RID: 49
		Protected NeededSafePoints As Global.LLVM.GarbageCollection.SafePointKind

		' Token: 0x04000032 RID: 50
		Protected CustomReadBarriers As Boolean

		' Token: 0x04000033 RID: 51
		Protected CustomWriteBarriers As Boolean

		' Token: 0x04000034 RID: 52
		Protected CustomRoots As Boolean

		' Token: 0x04000035 RID: 53
		Protected CustomSafePoints As Boolean

		' Token: 0x04000036 RID: 54
		Protected InitRoots As Boolean = True

		' Token: 0x04000037 RID: 55
		Protected UsesMetadata As Boolean

		' Token: 0x04000038 RID: 56
		Private findCustomSafePoints As Global.System.IntPtr

		' Token: 0x04000039 RID: 57
		Private initializeCustomLowering As Global.System.IntPtr

		' Token: 0x0400003A RID: 58
		Private performCustomLowering As Global.System.IntPtr

		' Token: 0x0400003B RID: 59
		Private Shared collectors As Global.System.Collections.Generic.List(Of Global.LLVM.GarbageCollection.GarbageCollector) = New Global.System.Collections.Generic.List(Of Global.LLVM.GarbageCollection.GarbageCollector)()

		' Token: 0x0400003C RID: 60
		Private Shared constructors As Global.System.Collections.Generic.Dictionary(Of String, Global.LLVM.GarbageCollection.NativeGlue.GCStrategyConstructor) = New Global.System.Collections.Generic.Dictionary(Of String, Global.LLVM.GarbageCollection.NativeGlue.GCStrategyConstructor)()

		' Token: 0x0200003B RID: 59
		<Global.System.Runtime.CompilerServices.CompilerGenerated()>
		<Global.System.Serializable()>
		Private NotInheritable Class <>c__21(Of GC As{Global.LLVM.GarbageCollection.GarbageCollector, New})
			' Token: 0x06000169 RID: 361 RVA: 0x0000441E File Offset: 0x0000261E
			' Note: this type is marked as 'beforefieldinit'.
			Shared Sub New()
			End Sub

			' Token: 0x0600016A RID: 362 RVA: 0x000037DA File Offset: 0x000019DA
			Public Sub New()
			End Sub

			' Token: 0x0600016B RID: 363 RVA: 0x0000442C File Offset: 0x0000262C
			Friend Function <Register>b__21_0() As Global.System.IntPtr
				Dim collectors As Global.System.Collections.Generic.List(Of Global.LLVM.GarbageCollection.GarbageCollector) = Global.LLVM.GarbageCollection.GarbageCollector.collectors
				Dim result As Global.System.IntPtr
				SyncLock collectors
					Dim gc As GC = Global.System.Activator.CreateInstance(Of GC)()
					Dim intPtr As Global.System.IntPtr = Global.LLVM.llvm.CreateGC(gc.GetExternalGCInfo())
					Global.LLVM.GarbageCollection.GarbageCollector.collectors.Add(gc)
					result = intPtr
				End SyncLock
				Return result
			End Function

			' Token: 0x04000060 RID: 96
			Public Shared <>9 As Global.LLVM.GarbageCollection.GarbageCollector.<>c__21(Of GC) = New Global.LLVM.GarbageCollection.GarbageCollector.<>c__21(Of !0)()

			' Token: 0x04000061 RID: 97
			Public Shared <>9__21_0 As Global.LLVM.GarbageCollection.NativeGlue.GCStrategyConstructor
		End Class
	End Class
End Namespace