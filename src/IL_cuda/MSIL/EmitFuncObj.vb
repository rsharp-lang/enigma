Friend Class EmitFuncObj
    Private _Module As LLVM.[Module], _Function As LLVM.[Function], _CilMethod As System.Reflection.MethodBody, _Stack As System.Collections.Generic.Stack(Of LLVM.Value), _Locals As LLVM.Value(), _Parameters As LLVM.Value()
    Public Property Builder As LLVM.InstructionBuilder
    Public Property Argument As Object
    Public ReadOnly Property Context As LLVM.Context
        Get
            Return Me.[Module].Context
        End Get
    End Property

    Public Property [Module] As LLVM.[Module]
        Get
            Return _Module
        End Get
        Private Set(ByVal value As LLVM.[Module])
            _Module = value
        End Set
    End Property

    Public Property [Function] As LLVM.[Function]
        Get
            Return _Function
        End Get
        Private Set(ByVal value As LLVM.[Function])
            _Function = value
        End Set
    End Property

    Public Property CilMethod As System.Reflection.MethodBody
        Get
            Return _CilMethod
        End Get
        Private Set(ByVal value As System.Reflection.MethodBody)
            _CilMethod = value
        End Set
    End Property

    Public Property Stack As System.Collections.Generic.Stack(Of LLVM.Value)
        Get
            Return _Stack
        End Get
        Private Set(ByVal value As System.Collections.Generic.Stack(Of LLVM.Value))
            _Stack = value
        End Set
    End Property

    Public Property Locals As LLVM.Value()
        Get
            Return _Locals
        End Get
        Private Set(ByVal value As LLVM.Value())
            _Locals = value
        End Set
    End Property

    Public Property Parameters As LLVM.Value()
        Get
            Return _Parameters
        End Get
        Private Set(ByVal value As LLVM.Value())
            _Parameters = value
        End Set
    End Property

    Public Sub New(ByVal [module] As LLVM.[Module], ByVal [function] As LLVM.[Function], ByVal cilMethod As System.Reflection.MethodBody, ByVal instructionBuilder As LLVM.InstructionBuilder, ByVal argument As Object, ByVal stack As System.Collections.Generic.Stack(Of LLVM.Value), ByVal locals As LLVM.Value(), ByVal parameters As LLVM.Value())
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
