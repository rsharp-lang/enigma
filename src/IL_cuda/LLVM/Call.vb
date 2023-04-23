Imports System

Namespace LLVM
	' Token: 0x02000008 RID: 8
	Public Class [Call]
		Inherits Instruction

		' Token: 0x0600000D RID: 13 RVA: 0x00002181 File Offset: 0x00000381
		Friend Sub New(valueref As IntPtr)
			MyBase.New(valueref)
		End Sub

		' Token: 0x17000002 RID: 2
		' (get) Token: 0x0600000E RID: 14 RVA: 0x0000218A File Offset: 0x0000038A
		' (set) Token: 0x0600000F RID: 15 RVA: 0x00002197 File Offset: 0x00000397
		Public Property CallingConvention As CallingConvention
			Get
				Return llvm.GetInstructionCallingConvention(Me)
			End Get
			Set(value As CallingConvention)
				llvm.SetInstructionCallingConvention(Me, value)
			End Set
		End Property

		' Token: 0x17000003 RID: 3
		' (get) Token: 0x06000010 RID: 16 RVA: 0x000021A5 File Offset: 0x000003A5
		' (set) Token: 0x06000011 RID: 17 RVA: 0x000021B2 File Offset: 0x000003B2
		Public Property TailCall As Boolean
			Get
				Return llvm.IsTailCall(Me)
			End Get
			Set(value As Boolean)
				llvm.SetTailCall(Me, value)
			End Set
		End Property
	End Class
End Namespace
