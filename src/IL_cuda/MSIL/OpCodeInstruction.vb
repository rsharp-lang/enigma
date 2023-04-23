
Imports System.Reflection.Emit

Friend Structure OpCodeInstruction

    Public ReadOnly Property InstructionStart As Long
    Public ReadOnly Property Opcode As OpCode
    Public ReadOnly Property Parameter As Object

    Public Sub New(ByVal instructionStart As Long, ByVal opcode As OpCode, ByVal parameter As Object)
        _InstructionStart = instructionStart
        _Opcode = opcode
        _Parameter = parameter
    End Sub

    Public Overrides Function ToString() As String
        Return String.Format("{0}: {1} - {2}", _InstructionStart, _Opcode, _Parameter)
    End Function
End Structure
