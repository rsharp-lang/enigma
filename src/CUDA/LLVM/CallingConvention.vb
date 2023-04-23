Imports System

Namespace LLVM
	' Token: 0x02000009 RID: 9
	Public Enum CallingConvention
		' Token: 0x04000002 RID: 2
		C
		' Token: 0x04000003 RID: 3
		Cdecl = 0
		' Token: 0x04000004 RID: 4
		Fast = 8
		' Token: 0x04000005 RID: 5
		Cold
		' Token: 0x04000006 RID: 6
		GHC
		' Token: 0x04000007 RID: 7
		StdCallX86 = 64
		' Token: 0x04000008 RID: 8
		FirstTargetSpecific = 64
		' Token: 0x04000009 RID: 9
		FastCallX86
		' Token: 0x0400000A RID: 10
		ApcsARM
		' Token: 0x0400000B RID: 11
		EABI
		' Token: 0x0400000C RID: 12
		EABI_VFP
		' Token: 0x0400000D RID: 13
		MSP430_INTR
	End Enum
End Namespace
