
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.Activations
Imports Microsoft.VisualBasic.Math.Lambda
Imports Microsoft.VisualBasic.MIME.application.xml.MathML
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Closure
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components

''' <summary>
''' the activate function handler
''' </summary>
<Package("activateFunction")>
Public Module activateFunction

    ''' <summary>
    ''' Create sigmoid activation function
    ''' </summary>
    ''' <param name="alpha"></param>
    ''' <returns></returns>
    <ExportAPI("sigmoid")>
    Public Function Sigmoid(Optional alpha As Double = 0.2) As IActivationFunction
        Return New Sigmoid(alpha)
    End Function

    <ExportAPI("identical")>
    Public Function Identical() As IActivationFunction
        Return New Identical()
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
    Public Function customFunc(forward As Object, derivative As Object, Optional env As Environment = Nothing) As Object
        Dim func = getLambda(forward, env)
        Dim deri = getLambda(derivative, env)

        If func Like GetType(Message) Then
            Return func.TryCast(Of Message)
        ElseIf deri Like GetType(Message) Then
            Return deri.TryCast(Of Message)
        End If

        Return New CustomFunction(func, deri)
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
        ElseIf TypeOf func Is String Then
            Return ActiveFunction.Parse(func).CreateFunction
        ElseIf TypeOf func Is IActivationFunction Then
            Return DirectCast(func, IActivationFunction)
        ElseIf TypeOf func Is ActiveFunction Then
            Return DirectCast(func, ActiveFunction).CreateFunction
        Else
            Return Message.InCompatibleType(GetType(IActivationFunction), func, env)
        End If
    End Function
End Module
