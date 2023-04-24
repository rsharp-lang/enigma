Imports System.Runtime.CompilerServices
Imports System.Text

Namespace LLVM
	' Token: 0x02000025 RID: 37
	Public Class StructType : Inherits CompositeType

		' Token: 0x060000FC RID: 252 RVA: 0x00002073 File Offset: 0x00000273
		Friend Sub New(typeref As IntPtr)
			MyBase.New(typeref)
		End Sub

		' Token: 0x060000FD RID: 253 RVA: 0x000035AC File Offset: 0x000017AC
		Private Shared Function structType(context As Context, elementTypes As IEnumerable(Of Type), Optional packed As Boolean = False) As IntPtr
			If elementTypes Is Nothing Then
				elementTypes = New Type(-1) {}
			End If
			Dim source As IEnumerable(Of Type) = elementTypes
			Dim Hx0___1_ As Func(Of Type, IntPtr) = c.Hx0___1_0
			Dim selector As Func(Of Type, IntPtr) = Hx0___1_
			If Hx0___1_ Is Nothing Then
				Dim func As Func(Of Type, IntPtr) = Function(type As Type) type
				selector = func
				c.Hx0___1_0 = func
			End If
			Dim array As IntPtr() = source.[Select](selector).ToArray()
			Return llvm.StructType(context, array, array.Length, packed)
		End Function

		' Token: 0x060000FE RID: 254 RVA: 0x000035FF File Offset: 0x000017FF
		<MethodImpl(MethodImplOptions.AggressiveInlining)>
		Private Shared Function structType(context As Context) As IntPtr
			Return llvm.StructCreateEmptyType(context)
		End Function

		' Token: 0x060000FF RID: 255 RVA: 0x0000360C File Offset: 0x0000180C
		Private Shared Function structType(context As Context, name As String) As IntPtr
			Return llvm.StructType(context, name)
		End Function

		' Token: 0x06000100 RID: 256 RVA: 0x0000361A File Offset: 0x0000181A
		Public Sub New(context As Context, elementTypes As Collections.Generic.IEnumerable(Of Type), Optional packed As Boolean = False)
			MyBase.New(structType(context, elementTypes, packed))
		End Sub

		' Token: 0x06000101 RID: 257 RVA: 0x0000362A File Offset: 0x0000182A
		Public Sub New(context As Context, name As String, elementTypes As Collections.Generic.IEnumerable(Of Type), Optional packed As Boolean = False)
			MyBase.New(structType(context, name))
			Me.SetBody(elementTypes, packed)
		End Sub

		' Token: 0x06000102 RID: 258 RVA: 0x00003642 File Offset: 0x00001842
		Public Sub New(context As Context)
			MyBase.New(structType(context))
		End Sub

		' Token: 0x06000103 RID: 259 RVA: 0x00003650 File Offset: 0x00001850
		Public Sub New(context As Context, name As String)
			MyBase.New(structType(context, name))
		End Sub

		' Token: 0x17000016 RID: 22
		' (get) Token: 0x06000104 RID: 260 RVA: 0x0000365F File Offset: 0x0000185F
		Public ReadOnly Property IsPacked As Boolean
			Get
				Return llvm.IsPackedStruct(Me)
			End Get
		End Property

		' Token: 0x17000017 RID: 23
		' (get) Token: 0x06000105 RID: 261 RVA: 0x0000366C File Offset: 0x0000186C
		Public ReadOnly Property IsOpaque As Boolean
			Get
				Return llvm.IsOpaqueStruct(Me)
			End Get
		End Property

		' Token: 0x17000018 RID: 24
		' (get) Token: 0x06000106 RID: 262 RVA: 0x00003679 File Offset: 0x00001879
		Public ReadOnly Property FieldCount As UInteger
			Get
				Return llvm.StructFieldCount(Me)
			End Get
		End Property

		' Token: 0x17000019 RID: 25
		' (get) Token: 0x06000107 RID: 263 RVA: 0x00003688 File Offset: 0x00001888
		' (set) Token: 0x06000108 RID: 264 RVA: 0x000036C4 File Offset: 0x000018C4
		Public Property FieldTypes As Type()
			Get
				Dim array As IntPtr() = New IntPtr(Me.FieldCount - 1) {}
				llvm.StructElements(Me, array)
				Return array.[Select](AddressOf Type.DetectType).ToArray()
			End Get
			Set(value As Type())
			End Set
		End Property

		' Token: 0x06000109 RID: 265 RVA: 0x000036C8 File Offset: 0x000018C8
		Public Sub SetBody(fieldTypes As IEnumerable(Of Type), Optional packed As Boolean = False)
			If fieldTypes Is Nothing Then
				fieldTypes = New Type(-1) {}
			End If
			Dim source As IEnumerable(Of Type) = fieldTypes
			Dim Hx0___17_ As Func(Of Type, IntPtr) = c.Hx0___17_0
			Dim selector As Func(Of Type, IntPtr) = Hx0___17_
			If Hx0___17_ Is Nothing Then
				Dim func As Func(Of Type, IntPtr) = Function(type As Type) type
				selector = func
				c.Hx0___17_0 = func
			End If
			Dim array As IntPtr() = source.[Select](selector).ToArray()
			llvm.StructSetBody(Me, array, array.Length, packed)
		End Sub

		' Token: 0x0600010A RID: 266 RVA: 0x0000371C File Offset: 0x0000191C
		Public Overrides Function ToString() As String
			Dim fieldTypes As Type() = Me.FieldTypes
			If fieldTypes.Length = 0 Then
				Return "{}"
			End If
			Dim stringBuilder As New StringBuilder("{ " + fieldTypes(0))
			For i As Integer = 1 To fieldTypes.Length - 1
				stringBuilder.Append(" * " + fieldTypes(i))
			Next
			stringBuilder.Append(" }")
			Return stringBuilder.ToString()
		End Function

		' Token: 0x0600010B RID: 267 RVA: 0x00003784 File Offset: 0x00001984
		Public Overrides Function StructuralEquals(obj As Type) As Boolean
			If Me Is Nothing AndAlso obj Is Nothing Then
				Return True
			End If
			Dim structType As StructType = TryCast(obj, StructType)
			Return structType IsNot Nothing AndAlso structType.IsPacked = Me.IsPacked AndAlso Me.FieldTypes.SequenceEqual(structType.FieldTypes)
		End Function

		' Token: 0x02000038 RID: 56
		<CompilerGenerated()>
		<Serializable()>
		Private NotInheritable Class c
			' Token: 0x06000152 RID: 338 RVA: 0x00003D9B File Offset: 0x00001F9B
			' Note: this type is marked as 'beforefieldinit'.
			Shared Sub New()
			End Sub

			' Token: 0x06000153 RID: 339 RVA: 0x000037DA File Offset: 0x000019DA
			Public Sub New()
			End Sub

			' Token: 0x06000154 RID: 340 RVA: 0x00003D7F File Offset: 0x00001F7F
			Friend Function b__1_0(type As Type) As IntPtr
				Return type
			End Function

			' Token: 0x06000155 RID: 341 RVA: 0x00003D7F File Offset: 0x00001F7F
			Friend Function b__17_0(type As Type) As IntPtr
				Return type
			End Function

			' Token: 0x04000059 RID: 89
			Public Shared Hx0_ As c = New c()

			' Token: 0x0400005A RID: 90
			Public Shared Hx0___1_0 As Func(Of Type, IntPtr)

			' Token: 0x0400005B RID: 91
			Public Shared Hx0___17_0 As Func(Of Type, IntPtr)
		End Class
	End Class
End Namespace
