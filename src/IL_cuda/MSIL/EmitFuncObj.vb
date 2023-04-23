Imports System.Reflection

Namespace MSIL

    Friend Class EmitFuncObj

        Public Property Builder As LLVM.InstructionBuilder
        Public Property Argument As Object

        Public ReadOnly Property Context As LLVM.Context
            Get
                Return Me.[Module].Context
            End Get
        End Property

        Public Property [Module] As LLVM.[Module]
        Public Property [Function] As LLVM.[Function]
        Public Property CilMethod As MethodBody
        Public Property Stack As Stack(Of LLVM.Value)
        Public Property Locals As LLVM.Value()
        Public Property Parameters As LLVM.Value()

        Public Sub New(ByVal [module] As LLVM.[Module],
                       ByVal [function] As LLVM.[Function],
                       ByVal cilMethod As MethodBody,
                       ByVal instructionBuilder As LLVM.InstructionBuilder,
                       ByVal argument As Object,
                       ByVal stack As Stack(Of LLVM.Value),
                       ByVal locals As LLVM.Value(),
                       ByVal parameters As LLVM.Value())

            Me.[Module] = [module]
            Me.[Function] = [function]
            Me.CilMethod = cilMethod
            Me.Builder = instructionBuilder
            Me.Argument = argument
            Me.Stack = stack
            Me.Locals = locals
            Me.Parameters = parameters
        End Sub
    End Class
End Namespace