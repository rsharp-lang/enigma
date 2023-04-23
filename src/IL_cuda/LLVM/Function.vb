Imports System
Imports System.Runtime.CompilerServices
Imports LLVM.GarbageCollection

Namespace LLVM
	' Token: 0x02000011 RID: 17
	Public Class [Function]
		Inherits GlobalValue

		' Token: 0x06000025 RID: 37 RVA: 0x000024C4 File Offset: 0x000006C4
		Friend Sub New(valueref As IntPtr)
			MyBase.New(valueref)
		End Sub

		' Token: 0x17000005 RID: 5
		<Runtime.CompilerServices.IndexerName("Arguments")>
		Public ReadOnly Default Property Item(index As Integer) As Argument
			Get
				If index < 0 Then
					Throw New ArgumentOutOfRangeException("index")
				End If
				Return New Argument(llvm.GetParameter(Me, index))
			End Get
		End Property

		' Token: 0x17000006 RID: 6
		' (get) Token: 0x06000027 RID: 39 RVA: 0x000024EF File Offset: 0x000006EF
		Public ReadOnly Property Type As FunctionType
			Get
				Return New FunctionType(llvm.GetElementType(llvm.[TypeOf](Me)))
			End Get
		End Property

		' Token: 0x17000007 RID: 7
		' (get) Token: 0x06000028 RID: 40 RVA: 0x00002506 File Offset: 0x00000706
		' (set) Token: 0x06000029 RID: 41 RVA: 0x00002513 File Offset: 0x00000713
		Public Property CallingConvention As CallingConvention
			Get
				Return llvm.GetCallingConvention(Me)
			End Get
			Set(value As CallingConvention)
				llvm.SetCallingConvention(Me, value)
			End Set
		End Property

		' Token: 0x0600002A RID: 42 RVA: 0x00002521 File Offset: 0x00000721
		Private Sub SetShadowStackGC()
			llvm.SetGC(Me, "shadow-stack")
		End Sub

		' Token: 0x0600002B RID: 43 RVA: 0x00002533 File Offset: 0x00000733
		Public Sub SetGC(Of GC As GarbageCollection.GarbageCollector)()
			If GetType(!!0) = GetType(GarbageCollection.ShadowStack) Then
				Me.SetShadowStackGC()
				Return
			End If
			llvm.SetGC(Me, GetType(!!0).FullName)
		End Sub

		' Token: 0x0600002C RID: 44 RVA: 0x00002571 File Offset: 0x00000771
		Public Overrides Function ToString() As String
			Return Me.CallingConvention + " " + Me.Type.ToString()
		End Function
	End Class
End Namespace
