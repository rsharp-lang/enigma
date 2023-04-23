Imports System

Namespace LLVM
	' Token: 0x0200001D RID: 29
	Public Class [Module]
		Inherits Global.LLVM.ReferenceBase

		' Token: 0x060000DF RID: 223 RVA: 0x00003331 File Offset: 0x00001531
		Public Sub New(name As String, context As Global.LLVM.Context)
			MyBase.New(Global.LLVM.llvm.CreateModule(name, context))
		End Sub

		' Token: 0x060000E0 RID: 224 RVA: 0x00002205 File Offset: 0x00000405
		Friend Sub New(moduleRef As Global.System.IntPtr)
			MyBase.New(moduleRef)
		End Sub

		' Token: 0x060000E1 RID: 225 RVA: 0x00003345 File Offset: 0x00001545
		Public Function CreateFunction(name As String, type As Global.LLVM.FunctionType) As Global.LLVM.[Function]
			Return New Global.LLVM.[Function](Global.LLVM.llvm.CreateFunction(Me, name, type))
		End Function

		' Token: 0x060000E2 RID: 226 RVA: 0x00003360 File Offset: 0x00001560
		Public Function GetFunction(name As String) As Global.LLVM.[Function]
			Dim [function] As Global.System.IntPtr = Global.LLVM.llvm.GetFunction(Me, name)
			If [function].IsNull() Then
				Return Nothing
			End If
			Return New Global.LLVM.[Function]([function])
		End Function

		' Token: 0x060000E3 RID: 227 RVA: 0x0000338C File Offset: 0x0000158C
		Public Function AddGlobal(type As Global.LLVM.Type, name As String, Optional value As Global.LLVM.Constant = Nothing) As Global.LLVM.GlobalValue
			Dim valueref As Global.System.IntPtr
			If value Is Nothing Then
				valueref = Global.LLVM.llvm.AddGlobal(Me, type, name)
			Else
				Dim constant As Global.System.IntPtr = value
				valueref = Global.LLVM.llvm.AddGlobal(Me, type, name, constant)
			End If
			Return New Global.LLVM.GlobalValue(valueref)
		End Function

		' Token: 0x060000E4 RID: 228 RVA: 0x000033D4 File Offset: 0x000015D4
		Public Function GetGlobal(name As String) As Global.LLVM.Value
			Dim namedGlobal As Global.System.IntPtr = Global.LLVM.llvm.GetNamedGlobal(Me, name)
			If Not namedGlobal.IsNull() Then
				Return New Global.LLVM.Value(namedGlobal)
			End If
			Return Nothing
		End Function

		' Token: 0x1700000F RID: 15
		' (get) Token: 0x060000E5 RID: 229 RVA: 0x000033FE File Offset: 0x000015FE
		Public ReadOnly Property Context As Global.LLVM.Context
			Get
				Return New Global.LLVM.Context(Global.LLVM.llvm.GetContext(Me))
			End Get
		End Property

		' Token: 0x060000E6 RID: 230 RVA: 0x00003410 File Offset: 0x00001610
		Public Function GetIntrinsic(name As String, type As Global.LLVM.FunctionType) As Global.LLVM.[Function]
			Dim [function] As Global.LLVM.[Function] = Me.GetFunction(name)
			If [function] IsNot Nothing Then
				Return [function]
			End If
			Return Me.CreateFunction(name, type)
		End Function

		' Token: 0x17000010 RID: 16
		' (get) Token: 0x060000E7 RID: 231 RVA: 0x00003434 File Offset: 0x00001634
		Public ReadOnly Property MemMove32 As Global.LLVM.[Function]
			Get
				Return Me.GetFunction("llvm.memmove.p0i8.p0i8.i32")
			End Get
		End Property

		' Token: 0x17000011 RID: 17
		' (get) Token: 0x060000E8 RID: 232 RVA: 0x00003441 File Offset: 0x00001641
		Public ReadOnly Property MemMove64 As Global.LLVM.[Function]
			Get
				Return Me.GetFunction("llvm.memmove.p0i8.p0i8.i64")
			End Get
		End Property

		' Token: 0x17000012 RID: 18
		' (get) Token: 0x060000E9 RID: 233 RVA: 0x0000344E File Offset: 0x0000164E
		Public ReadOnly Property Void As Global.LLVM.Type
			Get
				Return Global.LLVM.Type.GetVoid(Me.Context)
			End Get
		End Property

		' Token: 0x17000013 RID: 19
		' (get) Token: 0x060000EA RID: 234 RVA: 0x0000345B File Offset: 0x0000165B
		Public ReadOnly Property PVoid As Global.LLVM.PointerType
			Get
				Return Global.LLVM.PointerType.[Get](Global.LLVM.Type.GetVoid(Me.Context), 0)
			End Get
		End Property
	End Class
End Namespace
