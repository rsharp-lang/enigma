Namespace LLVM
    ' Token: 0x02000015 RID: 21
    Public Class InstructionBuilder : Inherits ReferenceBase

        ' Token: 0x06000037 RID: 55 RVA: 0x00002205 File Offset: 0x00000405
        Friend Sub New(builderref As IntPtr)
            MyBase.New(builderref)
        End Sub

        ' Token: 0x06000038 RID: 56 RVA: 0x00002708 File Offset: 0x00000908
        Public Sub New(context As Context)
            MyBase.New(llvm.CreateBuilder(context))
        End Sub

        ' Token: 0x06000039 RID: 57 RVA: 0x0000271B File Offset: 0x0000091B
        Public Sub New(context As Context, block As Block)
            MyBase.New(llvm.CreateBuilder(context))
            Me.PointToEnd(block)
        End Sub

        ' Token: 0x0600003A RID: 58 RVA: 0x00002735 File Offset: 0x00000935
        Private Shared Sub CheckBinaryIntegerOpArguments(left As Value, right As Value)
            If left Is Nothing Then
                Throw New ArgumentNullException("left")
            End If
            If right Is Nothing Then
                Throw New ArgumentNullException("right")
            End If
            InstructionBuilder.CheckIntegerType(left.Type, "left")
            InstructionBuilder.CheckIntegerType(right.Type, "right")
        End Sub

        ' Token: 0x0600003B RID: 59 RVA: 0x00002773 File Offset: 0x00000973
        Private Shared Sub CheckIntegerType(type As Type, value As String)
            If Not (TypeOf type Is IntegerType) Then
                Throw New ArgumentException("Value must be of integer type", value)
            End If
        End Sub

        ' Token: 0x0600003C RID: 60 RVA: 0x00002789 File Offset: 0x00000989
        Private Shared Sub CheckPointerType(value As Value, valueName As String)
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            If Not (TypeOf value.Type Is PointerType) Then
                Throw New ArgumentException("Value must be of pointer type", valueName)
            End If
        End Sub

        ' Token: 0x0600003D RID: 61 RVA: 0x000027B4 File Offset: 0x000009B4
        Public Function Compare(comparison As IntegerComparison, left As Value, right As Value, Optional name As String = "") As Value
            If left Is Nothing Then
                Throw New ArgumentNullException("left")
            End If
            If right Is Nothing Then
                Throw New ArgumentNullException("right")
            End If
            If left.Type.Kind <> TypeKind.Pointer AndAlso right.Type.Kind <> TypeKind.Pointer Then
                InstructionBuilder.CheckBinaryIntegerOpArguments(left, right)
            End If
            If Not left.Type.Equals(right.Type) Then
                Throw New ArgumentException(String.Concat(New Object() {"Type mismatch: ", left.Type, " vs ", right.Type}))
            End If
            Return New Value(llvm.EmitCompare(Me, comparison, left, right, name))
        End Function

        ' Token: 0x0600003E RID: 62 RVA: 0x00002867 File Offset: 0x00000A67
        Public Function IsNull(value As Value, Optional name As String = "") As Value
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            Return New Value(llvm.EmitIsNull(Me, value, name))
        End Function

        ' Token: 0x0600003F RID: 63 RVA: 0x00002890 File Offset: 0x00000A90
        Public Function [If](condition As Value, ontrue As Block, onfalse As Block) As Branch
            If condition Is Nothing Then
                Throw New ArgumentNullException("condition")
            End If
            If ontrue Is Nothing Then
                Throw New ArgumentNullException("ontrue")
            End If
            If onfalse Is Nothing Then
                Throw New ArgumentNullException("onfalse")
            End If
            If condition.Type.Kind <> TypeKind.[Integer] OrElse TryCast(condition.Type, IntegerType).Width <> 1 Then
                Throw New ArgumentException("Invalid condition type. Must be i1")
            End If
            Return New Branch(llvm.EmitIf(Me, condition, ontrue, onfalse))
        End Function

        ' Token: 0x06000040 RID: 64 RVA: 0x00002918 File Offset: 0x00000B18
        Public Function Switch(value As Value, elseCase As Block, branches As Tuple(Of Value, Block)()) As Switch
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            If elseCase Is Nothing Then
                Throw New ArgumentNullException("elseCase")
            End If
            If branches Is Nothing Then
                Throw New ArgumentNullException("branches")
            End If
            Dim _switch As Switch = New Switch(llvm.EmitSwitch(Me, value, elseCase, branches.Length))
            For Each tuple As Tuple(Of Value, Block) In branches
                _switch.Add(tuple.Item1, tuple.Item2)
            Next
            Return _switch
        End Function

        ' Token: 0x06000041 RID: 65 RVA: 0x00002996 File Offset: 0x00000B96
        Public Function [GoTo](dest As Block) As Branch
            If dest Is Nothing Then
                Throw New ArgumentNullException("dest")
            End If
            Return New Branch(llvm.EmitGoTo(Me, dest))
        End Function

        ' Token: 0x06000042 RID: 66 RVA: 0x000029BC File Offset: 0x00000BBC
        Public Function [Call](target As [Function], args As Value(), Optional name As String = "") As [Call]
            If args Is Nothing Then
                args = New Value(-1) {}
            End If
            Dim argumentTypes As Type() = target.FunctionType.ArgumentTypes
            InstructionBuilder.CheckArgumentTypes(args, argumentTypes)
            Dim ibuilder As IntPtr = Me
            Dim func As IntPtr = target
            Dim source As IEnumerable(Of Value) = args
            Dim selector As Func(Of Value, IntPtr) = Function(val As Value) val
            Return New [Call](llvm.EmitCall(ibuilder, func, source.[Select](selector).ToArray(), args.Length, name))
        End Function

        ' Token: 0x06000043 RID: 67 RVA: 0x00002A2C File Offset: 0x00000C2C
        Private Shared Sub CheckArgumentTypes(args As Value(), argtypes As Type())
            If argtypes.Length <> args.Length Then
                Throw New InvalidProgramException(String.Format("Incorrect number of arguments. Expecting: {0}, given: {1}", argtypes.Length, args.Length))
            End If
            For i As Integer = 0 To argtypes.Length - 1
                If Not argtypes(i).Equals(args(i).Type) Then
                    Throw New InvalidProgramException(String.Format("Incorrect argument {0} type. Expecting: {1}, given: {2}", i, argtypes(i), args(i).Type))
                End If
            Next
        End Sub

        ' Token: 0x06000044 RID: 68 RVA: 0x00002AA4 File Offset: 0x00000CA4
        Public Function [Call](target As [Function], ParamArray args As Value()) As [Call]
            Dim selector As Func(Of Value, IntPtr) = Function(val As Value) val
            Dim args2 As IntPtr() = args.[Select](selector).ToArray()
            InstructionBuilder.CheckArgumentTypes(args, target.FunctionType.ArgumentTypes)
            Return New [Call](llvm.EmitCall(Me, target, args2, args.Length, "")) With {
                .CallingConvention = target.CallingConvention
            }
        End Function

        ' Token: 0x06000045 RID: 69 RVA: 0x00002B18 File Offset: 0x00000D18
        Public Function [Return](value As Value) As [Return]
            Return New [Return](llvm.EmitReturn(Me, value))
        End Function

        ' Token: 0x06000046 RID: 70 RVA: 0x00002B30 File Offset: 0x00000D30
        Public Function [Return]() As [Return]
            Return New [Return](llvm.EmitReturn(Me))
        End Function

        ' Token: 0x06000047 RID: 71 RVA: 0x00002B42 File Offset: 0x00000D42
        Public Function Add(left As Value, right As Value, Optional name As String = "") As Value
            InstructionBuilder.CheckBinaryIntegerOpArguments(left, right)
            Return New Value(llvm.EmitAdd(Me, left, right, name))
        End Function

        ' Token: 0x06000048 RID: 72 RVA: 0x00002B68 File Offset: 0x00000D68
        Public Function AddFloat(left As Value, right As Value, Optional name As String = "") As Value
            Return New Value(llvm.EmitAddFloat(Me, left, right, name))
        End Function

        ' Token: 0x06000049 RID: 73 RVA: 0x00002B87 File Offset: 0x00000D87
        Public Function Subtract(left As Value, right As Value, Optional name As String = "") As Value
            Return New Value(llvm.EmitSubtract(Me, left, right, name))
        End Function

        ' Token: 0x0600004A RID: 74 RVA: 0x00002BA6 File Offset: 0x00000DA6
        Public Function Multiply(left As Value, right As Value, Optional name As String = "") As Value
            Return New Value(llvm.EmitMultiply(Me, left, right, name))
        End Function

        ' Token: 0x0600004B RID: 75 RVA: 0x00002BC5 File Offset: 0x00000DC5
        Public Function Divide(signed As Boolean, left As Value, right As Value, Optional name As String = "") As Value
            Return New Value(If(signed, llvm.EmitDivideSigned(Me, left, right, name), llvm.EmitDivideUnsigned(Me, left, right, name)))
        End Function

        ' Token: 0x0600004C RID: 76 RVA: 0x00002C03 File Offset: 0x00000E03
        Public Function Reminder(signed As Boolean, left As Value, right As Value, Optional name As String = "") As Value
            Return New Value(If(signed, llvm.EmitReminderSigned(Me, left, right, name), llvm.EmitReminderUnsigned(Me, left, right, name)))
        End Function

        ' Token: 0x0600004D RID: 77 RVA: 0x00002C41 File Offset: 0x00000E41
        Public Function Negate(value As Value, Optional name As String = "") As Value
            Return New Value(llvm.EmitNegate(Me, value, name))
        End Function

        ' Token: 0x0600004E RID: 78 RVA: 0x00002C5A File Offset: 0x00000E5A
        Public Function [And](left As Value, right As Value, Optional name As String = "") As Value
            InstructionBuilder.CheckBinaryIntegerOpArguments(left, right)
            Return New Value(llvm.EmitAnd(Me, left, right, name))
        End Function

        ' Token: 0x0600004F RID: 79 RVA: 0x00002C80 File Offset: 0x00000E80
        Public Function [Or](left As Value, right As Value, Optional name As String = "") As Value
            InstructionBuilder.CheckBinaryIntegerOpArguments(left, right)
            Return New Value(llvm.EmitOr(Me, left, right, name))
        End Function

        ' Token: 0x06000050 RID: 80 RVA: 0x00002CA6 File Offset: 0x00000EA6
        Public Function [Xor](left As Value, right As Value, Optional name As String = "") As Value
            InstructionBuilder.CheckBinaryIntegerOpArguments(left, right)
            Return New Value(llvm.EmitXor(Me, left, right, name))
        End Function

        ' Token: 0x06000051 RID: 81 RVA: 0x00002CCC File Offset: 0x00000ECC
        Public Function [Not](value As Value, Optional name As String = "") As Value
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            InstructionBuilder.CheckIntegerType(value.Type, "value")
            Return New Value(llvm.EmitNot(Me, value, name))
        End Function

        ' Token: 0x06000052 RID: 82 RVA: 0x00002D03 File Offset: 0x00000F03
        Public Function ShiftLeft(value As Value, shiftBy As Value, Optional name As String = "") As Value
            InstructionBuilder.CheckShiftArguments(value, shiftBy)
            Return New Value(llvm.EmitShiftLeft(Me, value, shiftBy, name))
        End Function

        ' Token: 0x06000053 RID: 83 RVA: 0x00002D29 File Offset: 0x00000F29
        Private Shared Sub CheckShiftArguments(value As Value, shiftBy As Value)
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            InstructionBuilder.CheckIntegerType(value.Type, "value")
            If shiftBy Is Nothing Then
                Throw New ArgumentNullException("shiftBy")
            End If
            InstructionBuilder.CheckIntegerType(shiftBy.Type, "shiftBy")
        End Sub

        ' Token: 0x06000054 RID: 84 RVA: 0x00002D68 File Offset: 0x00000F68
        Public Function ShiftRight(signed As Boolean, value As Value, shiftBy As Value, Optional name As String = "") As Value
            InstructionBuilder.CheckShiftArguments(value, shiftBy)
            Return New Value(If(signed, llvm.EmitShiftRightSigned(Me, value, shiftBy, name), llvm.EmitShiftRightUnsigned(Me, value, shiftBy, name)))
        End Function

        ' Token: 0x06000055 RID: 85 RVA: 0x00002DB8 File Offset: 0x00000FB8
        Public Function StackAlloc(type As Type, Optional name As String = "") As StackAlloc
            Return New StackAlloc(llvm.EmitStackAlloc(Me, type, name))
        End Function

        ' Token: 0x06000056 RID: 86 RVA: 0x00002DD1 File Offset: 0x00000FD1
        Public Function StackAlloc(type As Type, arraySize As Value, Optional name As String = "") As StackAlloc
            Return New StackAlloc(llvm.EmitStackAlloc(Me, type, arraySize, name))
        End Function

        ' Token: 0x06000057 RID: 87 RVA: 0x00002DF0 File Offset: 0x00000FF0
        Public Function Store(value As Value, pointer As Value) As Store
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            If pointer Is Nothing Then
                Throw New ArgumentNullException("pointer")
            End If
            Dim pointerType As PointerType = TryCast(pointer.Type, PointerType)
            If pointerType Is Nothing Then
                Throw New ArgumentException("Value must be of pointer type", "pointer")
            End If
            If Not value.Type.Equals(pointerType.ElementType) Then
                Throw New InvalidProgramException(String.Format("Can't store {0} to {1} location", value.Type, pointerType.ElementType))
            End If
            Return New Store(llvm.EmitStore(Me, value, pointer))
        End Function

        ' Token: 0x06000058 RID: 88 RVA: 0x00002E83 File Offset: 0x00001083
        Public Function Load(pointer As Value, Optional name As String = "") As Load
            InstructionBuilder.CheckPointerType(pointer, "pointer")
            Return New Load(llvm.EmitLoad(Me, pointer, name))
        End Function

        ' Token: 0x06000059 RID: 89 RVA: 0x00002EA7 File Offset: 0x000010A7
        Public Function StructureElement(pointer As Value, index As Integer, Optional name As String = "") As Value
            InstructionBuilder.CheckPointerType(pointer, "pointer")
            Return New Value(llvm.EmitStructElementPointer(Me, pointer, index, name))
        End Function

        ' Token: 0x0600005A RID: 90 RVA: 0x00002ECC File Offset: 0x000010CC
        Public Function Element(pointer As Value, indexes As Value(), Optional name As String = "") As Value
            InstructionBuilder.CheckPointerType(pointer, "pointer")
            If indexes Is Nothing Then
                Throw New ArgumentNullException("indexes")
            End If
            Dim selector As Func(Of Value, IntPtr) = Function(i As Value) i
            Dim offsets As IntPtr() = indexes.[Select](selector).ToArray()
            Return New Value(llvm.EmitGetElementPointer(Me, pointer, offsets, indexes.Length, name))
        End Function

        ' Token: 0x0600005B RID: 91 RVA: 0x00002F38 File Offset: 0x00001138
        Public Function Trunc(value As Value, destType As Type, Optional name As String = "") As Value
            Return New Value(llvm.EmitTrunc(Me, value, destType, name))
        End Function

        ' Token: 0x0600005C RID: 92 RVA: 0x00002F57 File Offset: 0x00001157
        Public Function ZeroExtend(value As Value, destType As Type, Optional name As String = "") As Value
            Return New Value(llvm.EmitZeroExtend(Me, value, destType, name))
        End Function

        ' Token: 0x0600005D RID: 93 RVA: 0x00002F76 File Offset: 0x00001176
        Public Function SignExtend(value As Value, destType As Type, Optional name As String = "") As Value
            Return New Value(llvm.EmitSignExtend(Me, value, destType, name))
        End Function

        ' Token: 0x0600005E RID: 94 RVA: 0x00002F98 File Offset: 0x00001198
        Public Function PointerCast(value As Value, destType As PointerType, Optional name As String = "") As Value
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            If value.Type.Kind <> TypeKind.Pointer Then
                Throw New ArgumentException("value must be of pointer type", "value")
            End If
            If destType Is Nothing Then
                Throw New ArgumentNullException("destType")
            End If
            Return New Value(llvm.EmitPointerCast(Me, value, destType, name))
        End Function

        ' Token: 0x0600005F RID: 95 RVA: 0x00003000 File Offset: 0x00001200
        Public Function IntToPointer(value As Value, destType As PointerType, Optional name As String = "") As Value
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            If destType Is Nothing Then
                Throw New ArgumentNullException("destType")
            End If
            If value.Type.Kind <> TypeKind.[Integer] Then
                Throw New ArgumentException("Value must be of integer type", "value")
            End If
            Return New Value(llvm.EmitIntToPtr(Me, value, destType, name))
        End Function

        ' Token: 0x06000060 RID: 96 RVA: 0x00003064 File Offset: 0x00001264
        Public Function PointerToInt(value As Value, destType As IntegerType, Optional name As String = "") As Value
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            If destType Is Nothing Then
                Throw New ArgumentNullException("destType")
            End If
            If value.Type.Kind <> TypeKind.Pointer Then
                Throw New ArgumentException("Value must be of pointer type", "value")
            End If
            Return New Value(llvm.EmitPtrToInt(Me, value, destType, name))
        End Function

        ' Token: 0x06000061 RID: 97 RVA: 0x000030C9 File Offset: 0x000012C9
        Public Function BitCast(value As Value, destType As Type, Optional name As String = "") As Value
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            If destType Is Nothing Then
                Throw New ArgumentNullException("destType")
            End If
            Return New Value(llvm.EmitBitCast(Me, value, destType, name))
        End Function

        ' Token: 0x06000062 RID: 98 RVA: 0x00003104 File Offset: 0x00001304
        Public Function Insert(into As Value, what As Value, index As Integer, Optional name As String = "") As Value
            If into Is Nothing Then
                Throw New ArgumentNullException("into")
            End If
            If what Is Nothing Then
                Throw New ArgumentNullException("what")
            End If
            Return New Value(llvm.EmitInsert(Me, into, what, index, name))
        End Function

        ' Token: 0x06000063 RID: 99 RVA: 0x00003141 File Offset: 0x00001341
        Public Function Extract(from As Value, index As Integer, Optional name As String = "") As Value
            If from Is Nothing Then
                Throw New ArgumentNullException("from")
            End If
            Return New Value(llvm.EmitExtract(Me, from, index, name))
        End Function

        ' Token: 0x06000064 RID: 100 RVA: 0x0000316C File Offset: 0x0000136C
        Public Function GCRoot(value As StackAlloc, [module] As [Module]) As [Call]
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            If value.Type Is Nothing Then
                Throw New ArgumentException()
            End If
            Dim pointerType As PointerType = PointerType.[Get](IntegerType.[Get](Me.Context, 8), 0)
            Dim pointerType2 As PointerType = PointerType.[Get](pointerType, 0)
            Dim value2 As Value = Me.PointerCast(value, pointerType2, "")
            Dim name As String = "llvm.gcroot"
            Dim void As Type = Type.GetVoid(Me.Context)
            Dim args As Type() = New PointerType() {pointerType2, pointerType}
            Dim intrinsic As [Function] = [module].GetIntrinsic(name, New FunctionType(void, args, False))
            Return Me.[Call](intrinsic, New Value() {value2, pointerType.Zero})
        End Function

        ' Token: 0x1700000B RID: 11
        ' (get) Token: 0x06000065 RID: 101 RVA: 0x00003207 File Offset: 0x00001407
        Public ReadOnly Property Context As Context
            Get
                Return New Context(llvm.GetBuilderContext(Me))
            End Get
        End Property

        ' Token: 0x06000066 RID: 102 RVA: 0x00003219 File Offset: 0x00001419
        Public Sub PointToEnd(block As Block)
            llvm.PointToEnd(Me, block)
        End Sub
    End Class
End Namespace
