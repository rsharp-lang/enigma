
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.Activations
Imports Microsoft.VisualBasic.Math.Lambda
Imports Microsoft.VisualBasic.MIME.application.xml.MathML
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Closure
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' the activate function handler
''' 
''' An activation function is a function used in artificial neural
''' networks which outputs a small value for small inputs, and a
''' larger value if its inputs exceed a threshold. If the inputs
''' are large enough, the activation function "fires", otherwise it
''' does nothing. In other words, an activation function is like a
''' gate that checks that an incoming value is greater than a 
''' critical number.
''' 
''' Activation functions are useful because they add non-linearities
''' into neural networks, allowing the neural networks To learn 
''' powerful operations. If the activation functions were To be removed
''' from a feedforward neural network, the entire network could be 
''' re-factored To a simple linear operation Or matrix transformation
''' On its input, And it would no longer be capable Of performing 
''' complex tasks such As image recognition.
''' 
''' Well-known activation functions used in data science include the 
''' rectified linear unit (ReLU) function, And the family of sigmoid 
''' functions such as the logistic sigmoid function, the hyperbolic
''' tangent, And the arctangent function.
''' </summary>
''' <remarks>
''' Activation functions in computer science are inspired by the 
''' action potential in neuroscience. If the electrical potential
''' between a neuron's interior and exterior exceeds a value called 
''' the action potential, the neuron undergoes a chain reaction 
''' which allows it to 'fire' and transmit a signal to neighboring 
''' neurons. The resultant sequence of activations, called a 'spike 
''' train', enables sensory neurons to transmit feeling from the 
''' fingers to the brain, and allows motor neurons to transmit 
''' instructions from the brain to the limbs.
''' </remarks>
<Package("activateFunction")>
Public Module activateFunction

    ''' <summary>
    ''' Logistic Sigmoid Function Formula
    ''' </summary>
    ''' <param name="alpha"></param>
    ''' <returns></returns>
    <ExportAPI("sigmoid")>
    Public Function Sigmoid(Optional alpha As Double = 0.2, Optional truncate As Double = -1) As IActivationFunction
        Return New Sigmoid(alpha) With {.Truncate = truncate}
    End Function

    <ExportAPI("identical")>
    Public Function Identical(Optional truncate As Double = -1) As IActivationFunction
        Return New Identical() With {.Truncate = truncate}
    End Function

    <ExportAPI("qlinear")>
    Public Function QLinear(Optional truncate As Double = -1) As IActivationFunction
        Return New QLinear With {.Truncate = truncate}
    End Function

    ''' <summary>
    ''' create a new custom activate function
    ''' </summary>
    ''' <param name="forward"></param>
    ''' <param name="derivative">
    ''' derivative formula of the ``<paramref name="forward"/>`` formula
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("func")>
    Public Function customFunc(forward As Object, derivative As Object,
                               Optional truncate As Double = -1,
                               Optional env As Environment = Nothing) As Object

        Dim func = getLambda(forward, env)
        Dim deri = getLambda(derivative, env)

        If func Like GetType(Message) Then
            Return func.TryCast(Of Message)
        ElseIf deri Like GetType(Message) Then
            Return deri.TryCast(Of Message)
        End If

        Return New CustomFunction(func, deri) With {
            .Truncate = truncate
        }
    End Function

    Private Function getLambda(raw As Object, env As Environment) As [Variant](Of Func(Of Double, Double), Message)
        Dim lambda As LambdaExpression

        If TypeOf raw Is String Then
            ' parse the math expression and the expression
            ' should contains only one variable: x
            lambda = Compiler.GetLambda(DirectCast(raw, String), "x")
        ElseIf TypeOf raw Is DeclareLambdaFunction Then
            lambda = Compiler.GetLambda(DirectCast(raw, DeclareLambdaFunction))
        Else
            Return Message.InCompatibleType(GetType(DeclareLambdaFunction), raw.GetType, env)
        End If

        Dim plambda = MathMLCompiler.CreateLambda(lambda)
        Dim pfunc As Func(Of Double, Double) = plambda.Compile

        Return pfunc
    End Function

    Friend Function getFunction(func As Object, env As Environment) As [Variant](Of Message, IActivationFunction)
        If func Is Nothing Then
            Return Internal.debug.stop("the required activate function can not be nothing!", env)
        ElseIf TypeOf func Is Message Then
            Return DirectCast(func, Message)
        ElseIf TypeOf func Is String Then
            Return ActiveFunction.Parse(func).CreateFunction
        ElseIf TypeOf func Is IActivationFunction Then
            Return DirectCast(func, IActivationFunction)
        ElseIf TypeOf func Is ActiveFunction Then
            Return DirectCast(func, ActiveFunction).CreateFunction
        ElseIf TypeOf func Is RMethodInfo Then
            Return getFunction(DirectCast(func, RMethodInfo).Invoke(env, {}), env)
        Else
            Return Message.InCompatibleType(GetType(IActivationFunction), func, env)
        End If
    End Function
End Module
