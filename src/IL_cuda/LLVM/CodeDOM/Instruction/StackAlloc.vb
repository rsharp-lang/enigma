Imports System.Runtime.CompilerServices

Namespace LLVM

	' Token: 0x02000023 RID: 35
	Public Class StackAlloc : Inherits Unary

		' Token: 0x17000015 RID: 21
		' (get) Token: 0x060000FA RID: 250 RVA: 0x0000359D File Offset: 0x0000179D
		Public Overrides ReadOnly Property Type As Type
			<MethodImpl(MethodImplOptions.AggressiveInlining)>
			Get
				Return CType(MyBase.Type, PointerType)
			End Get
		End Property

		Public ReadOnly Property PointerType As PointerType
			<MethodImpl(MethodImplOptions.AggressiveInlining)>
			Get
				Return Type
			End Get
		End Property

		' Token: 0x060000F9 RID: 249 RVA: 0x00003594 File Offset: 0x00001794
		<MethodImpl(MethodImplOptions.AggressiveInlining)>
		Friend Sub New(valueref As IntPtr)
			MyBase.New(valueref)
		End Sub

	End Class
End Namespace
