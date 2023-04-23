Namespace LLVM

	' Token: 0x0200001D RID: 29
	Public Class [Module] : Inherits ReferenceBase

		' Token: 0x060000DF RID: 223 RVA: 0x00003331 File Offset: 0x00001531
		Public Sub New(name As String, context As Context)
			MyBase.New(llvm.CreateModule(name, context))
		End Sub

		' Token: 0x060000E0 RID: 224 RVA: 0x00002205 File Offset: 0x00000405
		Friend Sub New(moduleRef As IntPtr)
			MyBase.New(moduleRef)
		End Sub

		' Token: 0x060000E1 RID: 225 RVA: 0x00003345 File Offset: 0x00001545
		Public Function CreateFunction(name As String, type As FunctionType) As [Function]
			Return New [Function](llvm.CreateFunction(Me, name, type))
		End Function

		' Token: 0x060000E2 RID: 226 RVA: 0x00003360 File Offset: 0x00001560
		Public Function GetFunction(name As String) As [Function]
			Dim [function] As IntPtr = llvm.GetFunction(Me, name)
			If [function].IsNull() Then
				Return Nothing
			End If
			Return New [Function]([function])
		End Function

		' Token: 0x060000E3 RID: 227 RVA: 0x0000338C File Offset: 0x0000158C
		Public Function AddGlobal(type As Type, name As String, Optional value As Constant = Nothing) As GlobalValue
			Dim valueref As IntPtr
			If value Is Nothing Then
				valueref = llvm.AddGlobal(Me, type, name)
			Else
				Dim constant As IntPtr = value
				valueref = llvm.AddGlobal(Me, type, name, constant)
			End If
			Return New GlobalValue(valueref)
		End Function

		' Token: 0x060000E4 RID: 228 RVA: 0x000033D4 File Offset: 0x000015D4
		Public Function GetGlobal(name As String) As Value
			Dim namedGlobal As IntPtr = llvm.GetNamedGlobal(Me, name)
			If Not namedGlobal.IsNull() Then
				Return New Value(namedGlobal)
			End If
			Return Nothing
		End Function

		' Token: 0x1700000F RID: 15
		' (get) Token: 0x060000E5 RID: 229 RVA: 0x000033FE File Offset: 0x000015FE
		Public ReadOnly Property Context As Context
			Get
				Return New Context(llvm.GetContext(Me))
			End Get
		End Property

		' Token: 0x060000E6 RID: 230 RVA: 0x00003410 File Offset: 0x00001610
		Public Function GetIntrinsic(name As String, type As FunctionType) As [Function]
			Dim [function] As [Function] = Me.GetFunction(name)
			If [function] IsNot Nothing Then
				Return [function]
			End If
			Return Me.CreateFunction(name, type)
		End Function

		' Token: 0x17000010 RID: 16
		' (get) Token: 0x060000E7 RID: 231 RVA: 0x00003434 File Offset: 0x00001634
		Public ReadOnly Property MemMove32 As [Function]
			Get
				Return Me.GetFunction("llvm.memmove.p0i8.p0i8.i32")
			End Get
		End Property

		' Token: 0x17000011 RID: 17
		' (get) Token: 0x060000E8 RID: 232 RVA: 0x00003441 File Offset: 0x00001641
		Public ReadOnly Property MemMove64 As [Function]
			Get
				Return Me.GetFunction("llvm.memmove.p0i8.p0i8.i64")
			End Get
		End Property

		' Token: 0x17000012 RID: 18
		' (get) Token: 0x060000E9 RID: 233 RVA: 0x0000344E File Offset: 0x0000164E
		Public ReadOnly Property Void As Type
			Get
				Return Type.GetVoid(Me.Context)
			End Get
		End Property

		' Token: 0x17000013 RID: 19
		' (get) Token: 0x060000EA RID: 234 RVA: 0x0000345B File Offset: 0x0000165B
		Public ReadOnly Property PVoid As PointerType
			Get
				Return PointerType.[Get](Type.GetVoid(Me.Context), 0)
			End Get
		End Property
	End Class
End Namespace
