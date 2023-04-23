Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Runtime.CompilerServices

Namespace CudaSharp
    Friend Module Disasm
        Private ReadOnly OpcodeLookupTable As Dictionary(Of Short, OpCode) = GetType(OpCodes) _
            .GetFields() _
            .[Select](Function(f)
                          Return CType(f.GetValue(Nothing), OpCode)
                      End Function) _
            .ToDictionary(Function(op)
                              Return op.Value
                          End Function)

        <Extension()>
        Public Iterator Function Disassemble(ByVal method As MethodBase) As IEnumerable(Of OpCodeInstruction)
            Dim [module] = method.Module
            Dim methodBody = method.GetMethodBody()
            If methodBody Is Nothing Then Throw New CudaSharpException("Could not get method body of " & method.Name)
            Dim stream = New BinaryReader(New MemoryStream(methodBody.GetILAsByteArray()))
            While stream.BaseStream.Position < stream.BaseStream.Length
                Dim instructionStart = stream.BaseStream.Position
                Dim byteOpcode = stream.ReadByte()
                Dim shortOpcode = If(byteOpcode = &HFE, BitConverter.ToInt16(If(BitConverter.IsLittleEndian, {stream.ReadByte(), byteOpcode}, {byteOpcode, stream.ReadByte()}), 0), byteOpcode)
                Dim opcode = Disasm.OpcodeLookupTable(shortOpcode)
                Dim parameter As Object
                Select Case opcode.OperandType
                    Case OperandType.InlineBrTarget
                        parameter = stream.ReadInt32()
                    Case OperandType.InlineField
                        parameter = [module].ResolveField(stream.ReadInt32())
                    Case OperandType.InlineI
                        parameter = stream.ReadInt32()
                    Case OperandType.InlineI8
                        parameter = stream.ReadInt64()
                    Case OperandType.InlineMethod
                        parameter = [module].ResolveMethod(stream.ReadInt32())
                    Case OperandType.InlineNone
                        parameter = Nothing
                    Case OperandType.InlineR
                        parameter = stream.ReadDouble()
                    Case OperandType.InlineSig
                        parameter = [module].ResolveSignature(stream.ReadInt32())
                    Case OperandType.InlineString
                        parameter = [module].ResolveString(stream.ReadInt32())
                    Case OperandType.InlineSwitch
                        parameter = stream.ReadInt32()
                    Case OperandType.InlineTok
                        parameter = [module].ResolveMember(stream.ReadInt32())
                    Case OperandType.InlineType
                        parameter = [module].ResolveType(stream.ReadInt32())
                    Case OperandType.InlineVar
                        parameter = stream.ReadInt16()
                    Case OperandType.ShortInlineBrTarget
                        parameter = stream.ReadSByte()
                    Case OperandType.ShortInlineI
                        parameter = stream.ReadSByte()
                    Case OperandType.ShortInlineR
                        parameter = stream.ReadSingle()
                    Case OperandType.ShortInlineVar
                        parameter = stream.ReadByte()
                    Case Else
                        Throw New ArgumentOutOfRangeException()
                End Select
                Yield New OpCodeInstruction(instructionStart, opcode, parameter)
            End While
        End Function
    End Module

    Friend Structure OpCodeInstruction
        Private ReadOnly _instructionStart As Long
        Private ReadOnly _opcode As OpCode
        Private ReadOnly _parameter As Object

        Public Sub New(ByVal instructionStart As Long, ByVal opcode As OpCode, ByVal parameter As Object)
            _instructionStart = instructionStart
            _opcode = opcode
            _parameter = parameter
        End Sub

        Public ReadOnly Property InstructionStart As Long
            Get
                Return _instructionStart
            End Get
        End Property

        Public ReadOnly Property Opcode As OpCode
            Get
                Return _opcode
            End Get
        End Property

        Public ReadOnly Property Parameter As Object
            Get
                Return _parameter
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("{0}: {1} - {2}", _instructionStart, _opcode, _parameter)
        End Function
    End Structure
End Namespace
