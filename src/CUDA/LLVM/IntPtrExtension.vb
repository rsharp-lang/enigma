Imports System

Namespace LLVM
	' Token: 0x02000019 RID: 25
	Public Module IntPtrExtension
		' Token: 0x06000070 RID: 112 RVA: 0x000032DB File Offset: 0x000014DB
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function IsNull(value As Global.System.IntPtr) As Boolean
			Return value.ToInt64() = 0L
		End Function

		' Token: 0x06000071 RID: 113 RVA: 0x000032E8 File Offset: 0x000014E8
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function IsNull(value As Global.System.UIntPtr) As Boolean
			Return value.ToUInt64() = 0UL
		End Function
	End Module
End Namespace
