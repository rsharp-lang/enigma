Imports System

Namespace LLVM.GarbageCollection.NativeGlue
	' Token: 0x02000031 RID: 49
	Friend Structure ExternalGCInfo
		' Token: 0x04000048 RID: 72
		Public NeededSafePoints As GarbageCollection.SafePointKind

		' Token: 0x04000049 RID: 73
		Public CustomReadBarriers As Boolean

		' Token: 0x0400004A RID: 74
		Public CustomWriteBarriers As Boolean

		' Token: 0x0400004B RID: 75
		Public CustomRoots As Boolean

		' Token: 0x0400004C RID: 76
		Public CustomSafePoints As Boolean

		' Token: 0x0400004D RID: 77
		Public InitRoots As Boolean

		' Token: 0x0400004E RID: 78
		Public UsesMetadata As Boolean

		' Token: 0x0400004F RID: 79
		Public InitializeCustomLowering As Global.System.IntPtr

		' Token: 0x04000050 RID: 80
		Public PerformCustomLowering As Global.System.IntPtr

		' Token: 0x04000051 RID: 81
		Public FindCustomSafePoints As Global.System.IntPtr
	End Structure
End Namespace
