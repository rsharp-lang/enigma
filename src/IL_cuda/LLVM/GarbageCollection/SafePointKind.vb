Imports System

Namespace LLVM.GarbageCollection
	' Token: 0x0200002F RID: 47
	<Flags()>
	Public Enum SafePointKind As UInteger
		' Token: 0x0400003E RID: 62
		None = 0UI
		' Token: 0x0400003F RID: 63
		[Loop] = 1UI
		' Token: 0x04000040 RID: 64
		[Return] = 2UI
		' Token: 0x04000041 RID: 65
		PreCall = 4UI
		' Token: 0x04000042 RID: 66
		PostCall = 8UI
	End Enum
End Namespace
