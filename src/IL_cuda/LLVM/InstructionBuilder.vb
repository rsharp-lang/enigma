Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.CompilerServices

Namespace LLVM
	' Token: 0x02000015 RID: 21
	Public Class InstructionBuilder
		Inherits Global.LLVM.ReferenceBase

		' Token: 0x06000037 RID: 55 RVA: 0x00002205 File Offset: 0x00000405
		Friend Sub New(builderref As Global.System.IntPtr)
			MyBase.New(builderref)
		End Sub

		' Token: 0x06000038 RID: 56 RVA: 0x00002708 File Offset: 0x00000908
		Public Sub New(context As Global.LLVM.Context)
			MyBase.New(Global.LLVM.llvm.CreateBuilder(context))
		End Sub

		' Token: 0x06000039 RID: 57 RVA: 0x0000271B File Offset: 0x0000091B
		Public Sub New(context As Global.LLVM.Context, block As Global.LLVM.Block)
			MyBase.New(Global.LLVM.llvm.CreateBuilder(context))
			Me.PointToEnd(block)
		End Sub

		' Token: 0x0600003A RID: 58 RVA: 0x00002735 File Offset: 0x00000935
		Private Shared Sub CheckBinaryIntegerOpArguments(left As Global.LLVM.Value, right As Global.LLVM.Value)
			If left Is Nothing Then
				Throw New Global.System.ArgumentNullException("left")
			End If
			If right Is Nothing Then
				Throw New Global.System.ArgumentNullException("right")
			End If
			Global.LLVM.InstructionBuilder.CheckIntegerType(left.Type, "left")
			Global.LLVM.InstructionBuilder.CheckIntegerType(right.Type, "right")
		End Sub

		' Token: 0x0600003B RID: 59 RVA: 0x00002773 File Offset: 0x00000973
		Private Shared Sub CheckIntegerType(type As Global.LLVM.Type, value As String)
			If Not(TypeOf type Is Global.LLVM.IntegerType) Then
				Throw New Global.System.ArgumentException("Value must be of integer type", value)
			End If
		End Sub

		' Token: 0x0600003C RID: 60 RVA: 0x00002789 File Offset: 0x00000989
		Private Shared Sub CheckPointerType(value As Global.LLVM.Value, valueName As String)
			If value Is Nothing Then
				Throw New Global.System.ArgumentNullException("value")
			End If
			If Not(TypeOf value.Type Is Global.LLVM.PointerType) Then
				Throw New Global.System.ArgumentException("Value must be of pointer type", valueName)
			End If
		End Sub

		' Token: 0x0600003D RID: 61 RVA: 0x000027B4 File Offset: 0x000009B4
		Public Function Compare(comparison As Global.LLVM.IntegerComparison, left As Global.LLVM.Value, right As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			If left Is Nothing Then
				Throw New Global.System.ArgumentNullException("left")
			End If
			If right Is Nothing Then
				Throw New Global.System.ArgumentNullException("right")
			End If
			If left.Type.Kind <> Global.LLVM.TypeKind.Pointer AndAlso right.Type.Kind <> Global.LLVM.TypeKind.Pointer Then
				Global.LLVM.InstructionBuilder.CheckBinaryIntegerOpArguments(left, right)
			End If
			If Not left.Type.Equals(right.Type) Then
				Throw New Global.System.ArgumentException(String.Concat(New Object() { "Type mismatch: ", left.Type, " vs ", right.Type }))
			End If
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitCompare(Me, comparison, left, right, name))
		End Function

		' Token: 0x0600003E RID: 62 RVA: 0x00002867 File Offset: 0x00000A67
		Public Function IsNull(value As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			If value Is Nothing Then
				Throw New Global.System.ArgumentNullException("value")
			End If
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitIsNull(Me, value, name))
		End Function

		' Token: 0x0600003F RID: 63 RVA: 0x00002890 File Offset: 0x00000A90
		Public Function [If](condition As Global.LLVM.Value, ontrue As Global.LLVM.Block, onfalse As Global.LLVM.Block) As Global.LLVM.Branch
			If condition Is Nothing Then
				Throw New Global.System.ArgumentNullException("condition")
			End If
			If ontrue Is Nothing Then
				Throw New Global.System.ArgumentNullException("ontrue")
			End If
			If onfalse Is Nothing Then
				Throw New Global.System.ArgumentNullException("onfalse")
			End If
			If condition.Type.Kind <> Global.LLVM.TypeKind.[Integer] OrElse TryCast(condition.Type, Global.LLVM.IntegerType).Width <> 1 Then
				Throw New Global.System.ArgumentException("Invalid condition type. Must be i1")
			End If
			Return New Global.LLVM.Branch(Global.LLVM.llvm.EmitIf(Me, condition, ontrue, onfalse))
		End Function

		' Token: 0x06000040 RID: 64 RVA: 0x00002918 File Offset: 0x00000B18
		Public Function Switch(value As Global.LLVM.Value, elseCase As Global.LLVM.Block, branches As Global.System.Tuple(Of Global.LLVM.Value, Global.LLVM.Block)()) As Global.LLVM.Switch
			If value Is Nothing Then
				Throw New Global.System.ArgumentNullException("value")
			End If
			If elseCase Is Nothing Then
				Throw New Global.System.ArgumentNullException("elseCase")
			End If
			If branches Is Nothing Then
				Throw New Global.System.ArgumentNullException("branches")
			End If
			Dim switch As Global.LLVM.Switch = New Global.LLVM.Switch(Global.LLVM.llvm.EmitSwitch(Me, value, elseCase, branches.Length))
			For Each tuple As Global.System.Tuple(Of Global.LLVM.Value, Global.LLVM.Block) In branches
				switch.Add(tuple.Item1, tuple.Item2)
			Next
			Return switch
		End Function

		' Token: 0x06000041 RID: 65 RVA: 0x00002996 File Offset: 0x00000B96
		Public Function [GoTo](dest As Global.LLVM.Block) As Global.LLVM.Branch
			If dest Is Nothing Then
				Throw New Global.System.ArgumentNullException("dest")
			End If
			Return New Global.LLVM.Branch(Global.LLVM.llvm.EmitGoTo(Me, dest))
		End Function

		' Token: 0x06000042 RID: 66 RVA: 0x000029BC File Offset: 0x00000BBC
		Public Function [Call](target As Global.LLVM.[Function], args As Global.LLVM.Value(), Optional name As String = "") As Global.LLVM.[Call]
			If args Is Nothing Then
				args = New Global.LLVM.Value(-1) {}
			End If
			Dim argumentTypes As Global.LLVM.Type() = target.Type.ArgumentTypes
			Global.LLVM.InstructionBuilder.CheckArgumentTypes(args, argumentTypes)
			Dim ibuilder As Global.System.IntPtr = Me
			Dim func As Global.System.IntPtr = target
			Dim source As Global.System.Collections.Generic.IEnumerable(Of Global.LLVM.Value) = args
			Dim <>9__11_ As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr) = Global.LLVM.InstructionBuilder.<>c.<>9__11_0
			Dim selector As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr) = <>9__11_
			If <>9__11_ Is Nothing Then
				Dim func2 As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr) = Function(val As Global.LLVM.Value) val
				selector = func2
				Global.LLVM.InstructionBuilder.<>c.<>9__11_0 = func2
			End If
			Return New Global.LLVM.[Call](Global.LLVM.llvm.EmitCall(ibuilder, func, source.[Select](selector).ToArray(), args.Length, name))
		End Function

		' Token: 0x06000043 RID: 67 RVA: 0x00002A2C File Offset: 0x00000C2C
		Private Shared Sub CheckArgumentTypes(args As Global.LLVM.Value(), argtypes As Global.LLVM.Type())
			If argtypes.Length <> args.Length Then
				Throw New Global.System.InvalidProgramException(String.Format("Incorrect number of arguments. Expecting: {0}, given: {1}", argtypes.Length, args.Length))
			End If
			For i As Integer = 0 To argtypes.Length - 1
				If Not argtypes(i).Equals(args(i).Type) Then
					Throw New Global.System.InvalidProgramException(String.Format("Incorrect argument {0} type. Expecting: {1}, given: {2}", i, argtypes(i), args(i).Type))
				End If
			Next
		End Sub

		' Token: 0x06000044 RID: 68 RVA: 0x00002AA4 File Offset: 0x00000CA4
		Public Function [Call](target As Global.LLVM.[Function], ParamArray args As Global.LLVM.Value()) As Global.LLVM.[Call]
			Dim <>9__13_ As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr) = Global.LLVM.InstructionBuilder.<>c.<>9__13_0
			Dim selector As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr) = <>9__13_
			If <>9__13_ Is Nothing Then
				Dim func As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr) = Function(val As Global.LLVM.Value) val
				selector = func
				Global.LLVM.InstructionBuilder.<>c.<>9__13_0 = func
			End If
			Dim args2 As Global.System.IntPtr() = args.[Select](selector).ToArray()
			Global.LLVM.InstructionBuilder.CheckArgumentTypes(args, target.Type.ArgumentTypes)
			Return New Global.LLVM.[Call](Global.LLVM.llvm.EmitCall(Me, target, args2, args.Length, "")) With { .CallingConvention = target.CallingConvention }
		End Function

		' Token: 0x06000045 RID: 69 RVA: 0x00002B18 File Offset: 0x00000D18
		Public Function [Return](value As Global.LLVM.Value) As Global.LLVM.[Return]
			Return New Global.LLVM.[Return](Global.LLVM.llvm.EmitReturn(Me, value))
		End Function

		' Token: 0x06000046 RID: 70 RVA: 0x00002B30 File Offset: 0x00000D30
		Public Function [Return]() As Global.LLVM.[Return]
			Return New Global.LLVM.[Return](Global.LLVM.llvm.EmitReturn(Me))
		End Function

		' Token: 0x06000047 RID: 71 RVA: 0x00002B42 File Offset: 0x00000D42
		Public Function Add(left As Global.LLVM.Value, right As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Global.LLVM.InstructionBuilder.CheckBinaryIntegerOpArguments(left, right)
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitAdd(Me, left, right, name))
		End Function

		' Token: 0x06000048 RID: 72 RVA: 0x00002B68 File Offset: 0x00000D68
		Public Function AddFloat(left As Global.LLVM.Value, right As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitAddFloat(Me, left, right, name))
		End Function

		' Token: 0x06000049 RID: 73 RVA: 0x00002B87 File Offset: 0x00000D87
		Public Function Subtract(left As Global.LLVM.Value, right As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitSubtract(Me, left, right, name))
		End Function

		' Token: 0x0600004A RID: 74 RVA: 0x00002BA6 File Offset: 0x00000DA6
		Public Function Multiply(left As Global.LLVM.Value, right As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitMultiply(Me, left, right, name))
		End Function

		' Token: 0x0600004B RID: 75 RVA: 0x00002BC5 File Offset: 0x00000DC5
		Public Function Divide(signed As Boolean, left As Global.LLVM.Value, right As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Return New Global.LLVM.Value(If(signed, Global.LLVM.llvm.EmitDivideSigned(Me, left, right, name), Global.LLVM.llvm.EmitDivideUnsigned(Me, left, right, name)))
		End Function

		' Token: 0x0600004C RID: 76 RVA: 0x00002C03 File Offset: 0x00000E03
		Public Function Reminder(signed As Boolean, left As Global.LLVM.Value, right As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Return New Global.LLVM.Value(If(signed, Global.LLVM.llvm.EmitReminderSigned(Me, left, right, name), Global.LLVM.llvm.EmitReminderUnsigned(Me, left, right, name)))
		End Function

		' Token: 0x0600004D RID: 77 RVA: 0x00002C41 File Offset: 0x00000E41
		Public Function Negate(value As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitNegate(Me, value, name))
		End Function

		' Token: 0x0600004E RID: 78 RVA: 0x00002C5A File Offset: 0x00000E5A
		Public Function [And](left As Global.LLVM.Value, right As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Global.LLVM.InstructionBuilder.CheckBinaryIntegerOpArguments(left, right)
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitAnd(Me, left, right, name))
		End Function

		' Token: 0x0600004F RID: 79 RVA: 0x00002C80 File Offset: 0x00000E80
		Public Function [Or](left As Global.LLVM.Value, right As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Global.LLVM.InstructionBuilder.CheckBinaryIntegerOpArguments(left, right)
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitOr(Me, left, right, name))
		End Function

		' Token: 0x06000050 RID: 80 RVA: 0x00002CA6 File Offset: 0x00000EA6
		Public Function [Xor](left As Global.LLVM.Value, right As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Global.LLVM.InstructionBuilder.CheckBinaryIntegerOpArguments(left, right)
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitXor(Me, left, right, name))
		End Function

		' Token: 0x06000051 RID: 81 RVA: 0x00002CCC File Offset: 0x00000ECC
		Public Function [Not](value As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			If value Is Nothing Then
				Throw New Global.System.ArgumentNullException("value")
			End If
			Global.LLVM.InstructionBuilder.CheckIntegerType(value.Type, "value")
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitNot(Me, value, name))
		End Function

		' Token: 0x06000052 RID: 82 RVA: 0x00002D03 File Offset: 0x00000F03
		Public Function ShiftLeft(value As Global.LLVM.Value, shiftBy As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Global.LLVM.InstructionBuilder.CheckShiftArguments(value, shiftBy)
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitShiftLeft(Me, value, shiftBy, name))
		End Function

		' Token: 0x06000053 RID: 83 RVA: 0x00002D29 File Offset: 0x00000F29
		Private Shared Sub CheckShiftArguments(value As Global.LLVM.Value, shiftBy As Global.LLVM.Value)
			If value Is Nothing Then
				Throw New Global.System.ArgumentNullException("value")
			End If
			Global.LLVM.InstructionBuilder.CheckIntegerType(value.Type, "value")
			If shiftBy Is Nothing Then
				Throw New Global.System.ArgumentNullException("shiftBy")
			End If
			Global.LLVM.InstructionBuilder.CheckIntegerType(shiftBy.Type, "shiftBy")
		End Sub

		' Token: 0x06000054 RID: 84 RVA: 0x00002D68 File Offset: 0x00000F68
		Public Function ShiftRight(signed As Boolean, value As Global.LLVM.Value, shiftBy As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Value
			Global.LLVM.InstructionBuilder.CheckShiftArguments(value, shiftBy)
			Return New Global.LLVM.Value(If(signed, Global.LLVM.llvm.EmitShiftRightSigned(Me, value, shiftBy, name), Global.LLVM.llvm.EmitShiftRightUnsigned(Me, value, shiftBy, name)))
		End Function

		' Token: 0x06000055 RID: 85 RVA: 0x00002DB8 File Offset: 0x00000FB8
		Public Function StackAlloc(type As Global.LLVM.Type, Optional name As String = "") As Global.LLVM.StackAlloc
			Return New Global.LLVM.StackAlloc(Global.LLVM.llvm.EmitStackAlloc(Me, type, name))
		End Function

		' Token: 0x06000056 RID: 86 RVA: 0x00002DD1 File Offset: 0x00000FD1
		Public Function StackAlloc(type As Global.LLVM.Type, arraySize As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.StackAlloc
			Return New Global.LLVM.StackAlloc(Global.LLVM.llvm.EmitStackAlloc(Me, type, arraySize, name))
		End Function

		' Token: 0x06000057 RID: 87 RVA: 0x00002DF0 File Offset: 0x00000FF0
		Public Function Store(value As Global.LLVM.Value, pointer As Global.LLVM.Value) As Global.LLVM.Store
			If value Is Nothing Then
				Throw New Global.System.ArgumentNullException("value")
			End If
			If pointer Is Nothing Then
				Throw New Global.System.ArgumentNullException("pointer")
			End If
			Dim pointerType As Global.LLVM.PointerType = TryCast(pointer.Type, Global.LLVM.PointerType)
			If pointerType Is Nothing Then
				Throw New Global.System.ArgumentException("Value must be of pointer type", "pointer")
			End If
			If Not value.Type.Equals(pointerType.ElementType) Then
				Throw New Global.System.InvalidProgramException(String.Format("Can't store {0} to {1} location", value.Type, pointerType.ElementType))
			End If
			Return New Global.LLVM.Store(Global.LLVM.llvm.EmitStore(Me, value, pointer))
		End Function

		' Token: 0x06000058 RID: 88 RVA: 0x00002E83 File Offset: 0x00001083
		Public Function Load(pointer As Global.LLVM.Value, Optional name As String = "") As Global.LLVM.Load
			Global.LLVM.InstructionBuilder.CheckPointerType(pointer, "pointer")
			Return New Global.LLVM.Load(Global.LLVM.llvm.EmitLoad(Me, pointer, name))
		End Function

		' Token: 0x06000059 RID: 89 RVA: 0x00002EA7 File Offset: 0x000010A7
		Public Function StructureElement(pointer As Global.LLVM.Value, index As Integer, Optional name As String = "") As Global.LLVM.Value
			Global.LLVM.InstructionBuilder.CheckPointerType(pointer, "pointer")
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitStructElementPointer(Me, pointer, index, name))
		End Function

		' Token: 0x0600005A RID: 90 RVA: 0x00002ECC File Offset: 0x000010CC
		Public Function Element(pointer As Global.LLVM.Value, indexes As Global.LLVM.Value(), Optional name As String = "") As Global.LLVM.Value
			Global.LLVM.InstructionBuilder.CheckPointerType(pointer, "pointer")
			If indexes Is Nothing Then
				Throw New Global.System.ArgumentNullException("indexes")
			End If
			Dim <>9__35_ As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr) = Global.LLVM.InstructionBuilder.<>c.<>9__35_0
			Dim selector As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr) = <>9__35_
			If <>9__35_ Is Nothing Then
				Dim func As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr) = Function(i As Global.LLVM.Value) i
				selector = func
				Global.LLVM.InstructionBuilder.<>c.<>9__35_0 = func
			End If
			Dim offsets As Global.System.IntPtr() = indexes.[Select](selector).ToArray()
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitGetElementPointer(Me, pointer, offsets, indexes.Length, name))
		End Function

		' Token: 0x0600005B RID: 91 RVA: 0x00002F38 File Offset: 0x00001138
		Public Function Trunc(value As Global.LLVM.Value, destType As Global.LLVM.Type, Optional name As String = "") As Global.LLVM.Value
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitTrunc(Me, value, destType, name))
		End Function

		' Token: 0x0600005C RID: 92 RVA: 0x00002F57 File Offset: 0x00001157
		Public Function ZeroExtend(value As Global.LLVM.Value, destType As Global.LLVM.Type, Optional name As String = "") As Global.LLVM.Value
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitZeroExtend(Me, value, destType, name))
		End Function

		' Token: 0x0600005D RID: 93 RVA: 0x00002F76 File Offset: 0x00001176
		Public Function SignExtend(value As Global.LLVM.Value, destType As Global.LLVM.Type, Optional name As String = "") As Global.LLVM.Value
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitSignExtend(Me, value, destType, name))
		End Function

		' Token: 0x0600005E RID: 94 RVA: 0x00002F98 File Offset: 0x00001198
		Public Function PointerCast(value As Global.LLVM.Value, destType As Global.LLVM.PointerType, Optional name As String = "") As Global.LLVM.Value
			If value Is Nothing Then
				Throw New Global.System.ArgumentNullException("value")
			End If
			If value.Type.Kind <> Global.LLVM.TypeKind.Pointer Then
				Throw New Global.System.ArgumentException("value must be of pointer type", "value")
			End If
			If destType Is Nothing Then
				Throw New Global.System.ArgumentNullException("destType")
			End If
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitPointerCast(Me, value, destType, name))
		End Function

		' Token: 0x0600005F RID: 95 RVA: 0x00003000 File Offset: 0x00001200
		Public Function IntToPointer(value As Global.LLVM.Value, destType As Global.LLVM.PointerType, Optional name As String = "") As Global.LLVM.Value
			If value Is Nothing Then
				Throw New Global.System.ArgumentNullException("value")
			End If
			If destType Is Nothing Then
				Throw New Global.System.ArgumentNullException("destType")
			End If
			If value.Type.Kind <> Global.LLVM.TypeKind.[Integer] Then
				Throw New Global.System.ArgumentException("Value must be of integer type", "value")
			End If
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitIntToPtr(Me, value, destType, name))
		End Function

		' Token: 0x06000060 RID: 96 RVA: 0x00003064 File Offset: 0x00001264
		Public Function PointerToInt(value As Global.LLVM.Value, destType As Global.LLVM.IntegerType, Optional name As String = "") As Global.LLVM.Value
			If value Is Nothing Then
				Throw New Global.System.ArgumentNullException("value")
			End If
			If destType Is Nothing Then
				Throw New Global.System.ArgumentNullException("destType")
			End If
			If value.Type.Kind <> Global.LLVM.TypeKind.Pointer Then
				Throw New Global.System.ArgumentException("Value must be of pointer type", "value")
			End If
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitPtrToInt(Me, value, destType, name))
		End Function

		' Token: 0x06000061 RID: 97 RVA: 0x000030C9 File Offset: 0x000012C9
		Public Function BitCast(value As Global.LLVM.Value, destType As Global.LLVM.Type, Optional name As String = "") As Global.LLVM.Value
			If value Is Nothing Then
				Throw New Global.System.ArgumentNullException("value")
			End If
			If destType Is Nothing Then
				Throw New Global.System.ArgumentNullException("destType")
			End If
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitBitCast(Me, value, destType, name))
		End Function

		' Token: 0x06000062 RID: 98 RVA: 0x00003104 File Offset: 0x00001304
		Public Function Insert(into As Global.LLVM.Value, what As Global.LLVM.Value, index As Integer, Optional name As String = "") As Global.LLVM.Value
			If into Is Nothing Then
				Throw New Global.System.ArgumentNullException("into")
			End If
			If what Is Nothing Then
				Throw New Global.System.ArgumentNullException("what")
			End If
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitInsert(Me, into, what, index, name))
		End Function

		' Token: 0x06000063 RID: 99 RVA: 0x00003141 File Offset: 0x00001341
		Public Function Extract(from As Global.LLVM.Value, index As Integer, Optional name As String = "") As Global.LLVM.Value
			If from Is Nothing Then
				Throw New Global.System.ArgumentNullException("from")
			End If
			Return New Global.LLVM.Value(Global.LLVM.llvm.EmitExtract(Me, from, index, name))
		End Function

		' Token: 0x06000064 RID: 100 RVA: 0x0000316C File Offset: 0x0000136C
		Public Function GCRoot(value As Global.LLVM.StackAlloc, [module] As Global.LLVM.[Module]) As Global.LLVM.[Call]
			If value Is Nothing Then
				Throw New Global.System.ArgumentNullException("value")
			End If
			If value.Type Is Nothing Then
				Throw New Global.System.ArgumentException()
			End If
			Dim pointerType As Global.LLVM.PointerType = Global.LLVM.PointerType.[Get](Global.LLVM.IntegerType.[Get](Me.Context, 8), 0)
			Dim pointerType2 As Global.LLVM.PointerType = Global.LLVM.PointerType.[Get](pointerType, 0)
			Dim value2 As Global.LLVM.Value = Me.PointerCast(value, pointerType2, "")
			Dim name As String = "llvm.gcroot"
			Dim void As Global.LLVM.Type = Global.LLVM.Type.GetVoid(Me.Context)
			Dim args As Global.LLVM.Type() = New Global.LLVM.PointerType() { pointerType2, pointerType }
			Dim intrinsic As Global.LLVM.[Function] = [module].GetIntrinsic(name, New Global.LLVM.FunctionType(void, args, False))
			Return Me.[Call](intrinsic, New Global.LLVM.Value() { value2, pointerType.Zero })
		End Function

		' Token: 0x1700000B RID: 11
		' (get) Token: 0x06000065 RID: 101 RVA: 0x00003207 File Offset: 0x00001407
		Public ReadOnly Property Context As Global.LLVM.Context
			Get
				Return New Global.LLVM.Context(Global.LLVM.llvm.GetBuilderContext(Me))
			End Get
		End Property

		' Token: 0x06000066 RID: 102 RVA: 0x00003219 File Offset: 0x00001419
		Public Sub PointToEnd(block As Global.LLVM.Block)
			Global.LLVM.llvm.PointToEnd(Me, block)
		End Sub

		' Token: 0x02000037 RID: 55
		<Global.System.Runtime.CompilerServices.CompilerGenerated()>
		<Global.System.Serializable()>
		Private NotInheritable Class <>c
			' Token: 0x0600014D RID: 333 RVA: 0x00003D8F File Offset: 0x00001F8F
			' Note: this type is marked as 'beforefieldinit'.
			Shared Sub New()
			End Sub

			' Token: 0x0600014E RID: 334 RVA: 0x000037DA File Offset: 0x000019DA
			Public Sub New()
			End Sub

			' Token: 0x0600014F RID: 335 RVA: 0x00003D7F File Offset: 0x00001F7F
			Friend Function <Call>b__11_0(val As Global.LLVM.Value) As Global.System.IntPtr
				Return val
			End Function

			' Token: 0x06000150 RID: 336 RVA: 0x00003D7F File Offset: 0x00001F7F
			Friend Function <Call>b__13_0(val As Global.LLVM.Value) As Global.System.IntPtr
				Return val
			End Function

			' Token: 0x06000151 RID: 337 RVA: 0x00003D7F File Offset: 0x00001F7F
			Friend Function <Element>b__35_0(i As Global.LLVM.Value) As Global.System.IntPtr
				Return i
			End Function

			' Token: 0x04000055 RID: 85
			Public Shared <>9 As Global.LLVM.InstructionBuilder.<>c = New Global.LLVM.InstructionBuilder.<>c()

			' Token: 0x04000056 RID: 86
			Public Shared <>9__11_0 As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr)

			' Token: 0x04000057 RID: 87
			Public Shared <>9__13_0 As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr)

			' Token: 0x04000058 RID: 88
			Public Shared <>9__35_0 As Global.System.Func(Of Global.LLVM.Value, Global.System.IntPtr)
		End Class
	End Class
End Namespace
