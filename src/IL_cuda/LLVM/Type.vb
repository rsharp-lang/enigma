Imports System

Namespace LLVM
	' Token: 0x02000028 RID: 40
	Public Class Type
		Inherits ReferenceBase

		' Token: 0x0600010F RID: 271 RVA: 0x00002205 File Offset: 0x00000405
		Friend Sub New(typeref As IntPtr)
			MyBase.New(typeref)
		End Sub

		' Token: 0x06000110 RID: 272 RVA: 0x000037E4 File Offset: 0x000019E4
		Friend Shared Function DetectType(typeref As IntPtr) As Type
			Dim kind As TypeKind = New Type(typeref).Kind
			If kind = TypeKind.Void Then
				Return New Type(typeref)
			End If
			Select Case kind
				Case TypeKind.[Integer]
					Return New IntegerType(typeref)
				Case TypeKind.[Function]
					Return New FunctionType(typeref)
				Case TypeKind.Struct
					Return New StructType(typeref)
				Case TypeKind.Array
					Return New ArrayType(typeref)
				Case TypeKind.Pointer
					Return New PointerType(typeref)
				Case Else
					Return New Type(typeref)
			End Select
		End Function

		' Token: 0x1700001A RID: 26
		' (get) Token: 0x06000111 RID: 273 RVA: 0x0000384E File Offset: 0x00001A4E
		Public ReadOnly Property Context As Context
			Get
				Return New Context(llvm.GetTypeContext(Me))
			End Get
		End Property

		' Token: 0x06000112 RID: 274 RVA: 0x00003860 File Offset: 0x00001A60
		Public Shared Function GetVoid(context As Context) As Type
			Return New Type(llvm.GetVoid(context))
		End Function

		' Token: 0x1700001B RID: 27
		' (get) Token: 0x06000113 RID: 275 RVA: 0x00003872 File Offset: 0x00001A72
		Public ReadOnly Property Kind As TypeKind
			Get
				Return llvm.GetTypeKind(Me)
			End Get
		End Property

		' Token: 0x1700001C RID: 28
		' (get) Token: 0x06000114 RID: 276 RVA: 0x0000387F File Offset: 0x00001A7F
		Public ReadOnly Property UndefinedValue As Value
			Get
				If Me.Kind = TypeKind.Void Then
					Return Me.Zero
				End If
				Return New Value(llvm.GetUndefinedValue(Me))
			End Get
		End Property

		' Token: 0x1700001D RID: 29
		' (get) Token: 0x06000115 RID: 277 RVA: 0x000038A0 File Offset: 0x00001AA0
		Public ReadOnly Property Zero As Constant
			Get
				Return New Constant(llvm.GetZero(Me))
			End Get
		End Property

		' Token: 0x1700001E RID: 30
		' (get) Token: 0x06000116 RID: 278 RVA: 0x000038B2 File Offset: 0x00001AB2
		Public ReadOnly Property Size As Value
			Get
				Return New Value(llvm.SizeOf(Me))
			End Get
		End Property

		' Token: 0x1700001F RID: 31
		' (get) Token: 0x06000117 RID: 279 RVA: 0x000038C4 File Offset: 0x00001AC4
		Public ReadOnly Property Align As Value
			Get
				Return New Value(llvm.AlignOf(Me))
			End Get
		End Property

		' Token: 0x06000118 RID: 280 RVA: 0x000038D8 File Offset: 0x00001AD8
		Public Overrides Function ToString() As String
			Return Me.Kind.ToString()
		End Function

		' Token: 0x06000119 RID: 281 RVA: 0x000038FC File Offset: 0x00001AFC
		Public Overridable Function StructuralEquals(obj As Type) As Boolean
			Return(Me Is Nothing AndAlso obj Is Nothing) OrElse (obj IsNot Nothing AndAlso Me.Kind = TypeKind.Void AndAlso obj.Kind = TypeKind.Void)
		End Function
	End Class
End Namespace
