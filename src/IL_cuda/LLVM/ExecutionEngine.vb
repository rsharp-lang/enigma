Imports System
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Threading

Namespace LLVM
	' Token: 0x0200000F RID: 15
	Public Class ExecutionEngine
		Inherits ReferenceBase

		' Token: 0x06000018 RID: 24 RVA: 0x00002224 File Offset: 0x00000424
		Private Shared Function Create(target As [Module]) As IntPtr
			Dim result As IntPtr
			Dim text As String
			If llvm.CreateExecutionEngine(result, target, text) Then
				Throw New InvalidOperationException(text)
			End If
			String.IsNullOrEmpty(text)
			Return result
		End Function

		' Token: 0x06000019 RID: 25 RVA: 0x00002251 File Offset: 0x00000451
		Public Sub New(target As [Module])
			MyBase.New(ExecutionEngine.Create(target))
			Me.loader = AddressOf Me.OnLazyLoad
			llvm.SetLazyFunctionCallback(Me, Me.loader)
		End Sub

		' Token: 0x0600001A RID: 26 RVA: 0x00002282 File Offset: 0x00000482
		Public Function GetPointer([global] As GlobalValue) As IntPtr
			Return llvm.GetPointer(Me, [global])
		End Function

		' Token: 0x0600001B RID: 27 RVA: 0x00002295 File Offset: 0x00000495
		Public Function GetValue(Of T As Structure)([global] As GlobalValue) As T
			Return CType(CObj(Runtime.InteropServices.Marshal.PtrToStructure(Me.GetPointer([global]), GetType(!!0))), !!0)
		End Function

		' Token: 0x0600001C RID: 28 RVA: 0x000022B2 File Offset: 0x000004B2
		Private Shared Function Align(value As Integer, alignment As Integer) As Integer
			Return(value + alignment - 1) / alignment * alignment
		End Function

		' Token: 0x0600001D RID: 29 RVA: 0x000022C0 File Offset: 0x000004C0
		Public Shared Function GetLayout(type As Type) As LayoutInfo
			Dim num As Integer = IntPtr.Size * 8
			Dim kind As TypeKind = type.Kind
			If kind <> TypeKind.Void Then
				Select Case kind
					Case TypeKind.[Integer]
						Dim result As LayoutInfo = New LayoutInfo() With { .Size = CType(type, IntegerType).Width }
						result.Align = result.Size
						For i As Integer = 8 To num - 1
							If result.Align <= i Then
								result.Align = i
								Return result
							End If
						Next
						result.Align = ExecutionEngine.Align(result.Align, num)
						Return result
					Case TypeKind.Struct
						Dim num2 As Integer = 0
						Dim num3 As Integer = 8
						Dim fieldTypes As Type() = CType(type, StructType).FieldTypes
						For j As Integer = 0 To fieldTypes.Length - 1
							Dim layout As LayoutInfo = ExecutionEngine.GetLayout(fieldTypes(j))
							num2 = ExecutionEngine.Align(num2, layout.Align)
							num3 = Math.Max(num3, layout.Align)
							num2 += layout.Size
						Next
						Return New LayoutInfo() With { .Size = num2, .Align = num3 }
					Case TypeKind.Pointer
						Return New LayoutInfo(IntPtr.Size * 8)
				End Select
				Throw New NotSupportedException()
			End If
			Return Nothing
		End Function

		' Token: 0x14000001 RID: 1
		' (add) Token: 0x0600001E RID: 30 RVA: 0x000023FC File Offset: 0x000005FC
		' (remove) Token: 0x0600001F RID: 31 RVA: 0x00002434 File Offset: 0x00000634
		Public Custom Event LazyLoad As LazyFunctionLoader
			<Runtime.CompilerServices.CompilerGenerated()>
			AddHandler
				Dim lazyFunctionLoader As LazyFunctionLoader = Me.LazyLoad
				Dim lazyFunctionLoader2 As LazyFunctionLoader
				Do
					lazyFunctionLoader2 = lazyFunctionLoader
					Dim value2 As LazyFunctionLoader = CType([Delegate].Combine(lazyFunctionLoader2, value), LazyFunctionLoader)
					lazyFunctionLoader = Threading.Interlocked.CompareExchange(Of LazyFunctionLoader)(Me.LazyLoad, value2, lazyFunctionLoader2)
				Loop While lazyFunctionLoader IsNot lazyFunctionLoader2
			End AddHandler
			<Runtime.CompilerServices.CompilerGenerated()>
			RemoveHandler
				Dim lazyFunctionLoader As LazyFunctionLoader = Me.LazyLoad
				Dim lazyFunctionLoader2 As LazyFunctionLoader
				Do
					lazyFunctionLoader2 = lazyFunctionLoader
					Dim value2 As LazyFunctionLoader = CType([Delegate].Remove(lazyFunctionLoader2, value), LazyFunctionLoader)
					lazyFunctionLoader = Threading.Interlocked.CompareExchange(Of LazyFunctionLoader)(Me.LazyLoad, value2, lazyFunctionLoader2)
				Loop While lazyFunctionLoader IsNot lazyFunctionLoader2
			End RemoveHandler
		End Event

		' Token: 0x06000020 RID: 32 RVA: 0x0000246C File Offset: 0x0000066C
		Private Function OnLazyLoad(name As String) As IntPtr
			If Me.LazyLoad Is Nothing Then
				Return IntPtr.Zero
			End If
			Dim invocationList As [Delegate]() = Me.LazyLoad.GetInvocationList()
			For i As Integer = 0 To invocationList.Length - 1
				Dim intPtr As IntPtr = CType(invocationList(i), LazyFunctionLoader)(name)
				If intPtr <> IntPtr.Zero Then
					Return intPtr
				End If
			Next
			Return IntPtr.Zero
		End Function

		' Token: 0x0400000E RID: 14
		Private loader As LazyFunctionLoader

		' Token: 0x0400000F RID: 15
		<Runtime.CompilerServices.CompilerGenerated()>
		Private LazyLoad As LazyFunctionLoader
	End Class
End Namespace
