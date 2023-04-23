Imports System.IO
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Development.VisualStudio

Namespace MSIL

    ''' <summary>
    ''' extract the msil code from a .net clr function object
    ''' </summary>
    Module Disasm

        ''' <summary>
        ''' extract the msil code from a given clr method/function
        ''' </summary>
        ''' <param name="method"></param>
        ''' <returns></returns>
        <Extension()>
        Public Iterator Function Disassemble(ByVal method As MethodBase) As IEnumerable(Of OpCodeInstruction)
            Dim [module] As [Module] = method.Module
            Dim methodBody = method.GetMethodBody()

            If methodBody Is Nothing Then
                Throw New CudaException("Could not get method body of " & method.Name)
            End If

            Dim stream = New BinaryReader(New MemoryStream(methodBody.GetILAsByteArray()))

            While stream.BaseStream.Position < stream.BaseStream.Length
                Dim instructionStart = stream.BaseStream.Position
                Dim byteOpcode = stream.ReadByte()
                Dim shortOpcode = If(
                    byteOpcode = &HFE,
                    BitConverter.ToInt16(If(BitConverter.IsLittleEndian, {stream.ReadByte(), byteOpcode}, {byteOpcode, stream.ReadByte()}), 0),
                    byteOpcode
                )
                Dim opcode As OpCode = IL.GetOpCode(shortOpcode)

                Yield New OpCodeInstruction(instructionStart, opcode, stream.ParseILOpCode([module], opcode))
            End While
        End Function

        <Extension>
        Private Function ParseILOpCode(stream As BinaryReader, [module] As [Module], opcode As OpCode) As Object
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

            Return parameter
        End Function
    End Module
End Namespace