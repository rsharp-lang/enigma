Imports System
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Threading

Namespace LLVM
	' Token: 0x0200000F RID: 15
	Public Class ExecutionEngine
		Inherits Global.LLVM.ReferenceBase

		' Token: 0x06000018 RID: 24 RVA: 0x00002224 File Offset: 0x00000424
		Private Shared Function Create(target As Global.LLVM.[Module]) As Global.System.IntPtr
			Dim result As Global.System.IntPtr
			Dim text As String
			If Global.LLVM.llvm.CreateExecutionEngine(result, target, text) Then
				Throw New Global.System.InvalidOperationException(text)
			End If
			String.IsNullOrEmpty(text)
			Return result
		End Function

		' Token: 0x06000019 RID: 25 RVA: 0x00002251 File Offset: 0x00000451
		Public Sub New(target As Global.LLVM.[Module])
			MyBase.New(Global.LLVM.ExecutionEngine.Create(target))
			Me.loader = AddressOf Me.OnLazyLoad
			Global.LLVM.llvm.SetLazyFunctionCallback(Me, Me.loader)
		End Sub

		' Token: 0x0600001A RID: 26 RVA: 0x00002282 File Offset: 0x00000482
		Public Function GetPointer([global] As Global.LLVM.GlobalValue) As Global.System.IntPtr
			Return Global.LLVM.llvm.GetPointer(Me, [global])
		End Function

		' Token: 0x0600001B RID: 27 RVA: 0x00002295 File Offset: 0x00000495
		Public Function GetValue(Of T As Structure)([global] As Global.LLVM.GlobalValue) As T
			Return CType(CObj(Global.System.Runtime.InteropServices.Marshal.PtrToStructure(Me.GetPointer([global]), GetType(!!0))), !!0)
		End Function

		' Token: 0x0600001C RID: 28 RVA: 0x000022B2 File Offset: 0x000004B2
		Private Shared Function Align(value As Integer, alignment As Integer) As Integer
			Return(value + alignment - 1) / alignment * alignment
		End Function

		' Token: 0x0600001D RID: 29 RVA: 0x000022C0 File Offset: 0x000004C0
		Public Shared Function GetLayout(type As Global.LLVM.Type) As Global.LLVM.LayoutInfo
			Dim num As Integer = Global.System.IntPtr.Size * 8
			Dim kind As Global.LLVM.TypeKind = type.Kind
			If kind <> Global.LLVM.TypeKind.Void Then
				Select Case kind
					Case Global.LLVM.TypeKind.[Integer]
						Dim result As Global.LLVM.LayoutInfo = New Global.LLVM.LayoutInfo() With { .Size = CType(type, Global.LLVM.IntegerType).Width }
						result.Align = result.Size
						For i As Integer = 8 To num - 1
							If result.Align <= i Then
								result.Align = i
								Return result
							End If
						Next
						result.Align = Global.LLVM.ExecutionEngine.Align(result.Align, num)
						Return result
					Case Global.LLVM.TypeKind.Struct
						Dim num2 As Integer = 0
						Dim num3 As Integer = 8
						Dim fieldTypes As Global.LLVM.Type() = CType(type, Global.LLVM.StructType).FieldTypes
						For j As Integer = 0 To fieldTypes.Length - 1
							Dim layout As Global.LLVM.LayoutInfo = Global.LLVM.ExecutionEngine.GetLayout(fieldTypes(j))
							num2 = Global.LLVM.ExecutionEngine.Align(num2, layout.Align)
							num3 = Global.System.Math.Max(num3, layout.Align)
							num2 += layout.Size
						Next
						Return New Global.LLVM.LayoutInfo() With { .Size = num2, .Align = num3 }
					Case Global.LLVM.TypeKind.Pointer
						Return New Global.LLVM.LayoutInfo(Global.System.IntPtr.Size * 8)
				End Select
				Throw New Global.System.NotSupportedException()
			End If
			Return Nothing
		End Function

		' Token: 0x14000001 RID: 1
		' (add) Token: 0x0600001E RID: 30 RVA: 0x000023FC File Offset: 0x000005FC
		' (remove) Token: 0x0600001F RID: 31 RVA: 0x00002434 File Offset: 0x00000634
		Public Custom Event LazyLoad As Global.LLVM.LazyFunctionLoader
			<Global.System.Runtime.CompilerServices.CompilerGenerated()>
			AddHandler
				Dim lazyFunctionLoader As Global.LLVM.LazyFunctionLoader = Me.LazyLoad
				Dim lazyFunctionLoader2 As Global.LLVM.LazyFunctionLoader
				Do
					lazyFunctionLoader2 = lazyFunctionLoader
					Dim value2 As Global.LLVM.LazyFunctionLoader = CType(Global.System.[Delegate].Combine(lazyFunctionLoader2, value), Global.LLVM.LazyFunctionLoader)
					lazyFunctionLoader = Global.System.Threading.Interlocked.CompareExchange(Of Global.LLVM.LazyFunctionLoader)(Me.LazyLoad, value2, lazyFunctionLoader2)
				Loop While lazyFunctionLoader IsNot lazyFunctionLoader2
			End AddHandler
			<Global.System.Runtime.CompilerServices.CompilerGenerated()>
			RemoveHandler
				Dim lazyFunctionLoader As Global.LLVM.LazyFunctionLoader = Me.LazyLoad
				Dim lazyFunctionLoader2 As Global.LLVM.LazyFunctionLoader
				Do
					lazyFunctionLoader2 = lazyFunctionLoader
					Dim value2 As Global.LLVM.LazyFunctionLoader = CType(Global.System.[Delegate].Remove(lazyFunctionLoader2, value), Global.LLVM.LazyFunctionLoader)
					lazyFunctionLoader = Global.System.Threading.Interlocked.CompareExchange(Of Global.LLVM.LazyFunctionLoader)(Me.LazyLoad, value2, lazyFunctionLoader2)
				Loop While lazyFunctionLoader IsNot lazyFunctionLoader2
			End RemoveHandler
		End Event

		' Token: 0x06000020 RID: 32 RVA: 0x0000246C File Offset: 0x0000066C
		Private Function OnLazyLoad(name As String) As Global.System.IntPtr
			If Me.LazyLoad Is Nothing Then
				Return Global.System.IntPtr.Zero
			End If
			Dim invocationList As Global.System.[Delegate]() = Me.LazyLoad.GetInvocationList()
			For i As Integer = 0 To invocationList.Length - 1
				Dim intPtr As Global.System.IntPtr = CType(invocationList(i), Global.LLVM.LazyFunctionLoader)(name)
				If intPtr <> Global.System.IntPtr.Zero Then
					Return intPtr
				End If
			Next
			Return Global.System.IntPtr.Zero
		End Function

		' Token: 0x0400000E RID: 14
		Private loader As Global.LLVM.LazyFunctionLoader

		' Token: 0x0400000F RID: 15
		<Global.System.Runtime.CompilerServices.CompilerGenerated()>
		Private LazyLoad As Global.LLVM.LazyFunctionLoader
	End Class
End Namespace
