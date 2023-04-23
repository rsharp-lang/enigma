Imports System

Namespace LLVM
	' Token: 0x02000028 RID: 40
	Public Class Type
		Inherits Global.LLVM.ReferenceBase

		' Token: 0x0600010F RID: 271 RVA: 0x00002205 File Offset: 0x00000405
		Friend Sub New(typeref As Global.System.IntPtr)
			MyBase.New(typeref)
		End Sub

		' Token: 0x06000110 RID: 272 RVA: 0x000037E4 File Offset: 0x000019E4
		Friend Shared Function DetectType(typeref As Global.System.IntPtr) As Global.LLVM.Type
			Dim kind As Global.LLVM.TypeKind = New Global.LLVM.Type(typeref).Kind
			If kind = Global.LLVM.TypeKind.Void Then
				Return New Global.LLVM.Type(typeref)
			End If
			Select Case kind
				Case Global.LLVM.TypeKind.[Integer]
					Return New Global.LLVM.IntegerType(typeref)
				Case Global.LLVM.TypeKind.[Function]
					Return New Global.LLVM.FunctionType(typeref)
				Case Global.LLVM.TypeKind.Struct
					Return New Global.LLVM.StructType(typeref)
				Case Global.LLVM.TypeKind.Array
					Return New Global.LLVM.ArrayType(typeref)
				Case Global.LLVM.TypeKind.Pointer
					Return New Global.LLVM.PointerType(typeref)
				Case Else
					Return New Global.LLVM.Type(typeref)
			End Select
		End Function

		' Token: 0x1700001A RID: 26
		' (get) Token: 0x06000111 RID: 273 RVA: 0x0000384E File Offset: 0x00001A4E
		Public ReadOnly Property Context As Global.LLVM.Context
			Get
				Return New Global.LLVM.Context(Global.LLVM.llvm.GetTypeContext(Me))
			End Get
		End Property

		' Token: 0x06000112 RID: 274 RVA: 0x00003860 File Offset: 0x00001A60
		Public Shared Function GetVoid(context As Global.LLVM.Context) As Global.LLVM.Type
			Return New Global.LLVM.Type(Global.LLVM.llvm.GetVoid(context))
		End Function

		' Token: 0x1700001B RID: 27
		' (get) Token: 0x06000113 RID: 275 RVA: 0x00003872 File Offset: 0x00001A72
		Public ReadOnly Property Kind As Global.LLVM.TypeKind
			Get
				Return Global.LLVM.llvm.GetTypeKind(Me)
			End Get
		End Property

		' Token: 0x1700001C RID: 28
		' (get) Token: 0x06000114 RID: 276 RVA: 0x0000387F File Offset: 0x00001A7F
		Public ReadOnly Property UndefinedValue As Global.LLVM.Value
			Get
				If Me.Kind = Global.LLVM.TypeKind.Void Then
					Return Me.Zero
				End If
				Return New Global.LLVM.Value(Global.LLVM.llvm.GetUndefinedValue(Me))
			End Get
		End Property

		' Token: 0x1700001D RID: 29
		' (get) Token: 0x06000115 RID: 277 RVA: 0x000038A0 File Offset: 0x00001AA0
		Public ReadOnly Property Zero As Global.LLVM.Constant
			Get
				Return New Global.LLVM.Constant(Global.LLVM.llvm.GetZero(Me))
			End Get
		End Property

		' Token: 0x1700001E RID: 30
		' (get) Token: 0x06000116 RID: 278 RVA: 0x000038B2 File Offset: 0x00001AB2
		Public ReadOnly Property Size As Global.LLVM.Value
			Get
				Return New Global.LLVM.Value(Global.LLVM.llvm.SizeOf(Me))
			End Get
		End Property

		' Token: 0x1700001F RID: 31
		' (get) Token: 0x06000117 RID: 279 RVA: 0x000038C4 File Offset: 0x00001AC4
		Public ReadOnly Property Align As Global.LLVM.Value
			Get
				Return New Global.LLVM.Value(Global.LLVM.llvm.AlignOf(Me))
			End Get
		End Property

		' Token: 0x06000118 RID: 280 RVA: 0x000038D8 File Offset: 0x00001AD8
		Public Overrides Function ToString() As String
			Return Me.Kind.ToString()
		End Function

		' Token: 0x06000119 RID: 281 RVA: 0x000038FC File Offset: 0x00001AFC
		Public Overridable Function StructuralEquals(obj As Global.LLVM.Type) As Boolean
			Return(Me Is Nothing AndAlso obj Is Nothing) OrElse (obj IsNot Nothing AndAlso Me.Kind = Global.LLVM.TypeKind.Void AndAlso obj.Kind = Global.LLVM.TypeKind.Void)
		End Function
	End Class
End Namespace
