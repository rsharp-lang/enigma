
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Closure
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components

<Package("learning")>
Public Module learning

    ''' <summary>
    ''' create a new machine learning model
    ''' </summary>
    ''' <param name="x">
    ''' the source of the machine learning model where it comes from:
    ''' 
    ''' 1. could be a file name to the trained model file
    ''' 2. could be a function name in ``models`` namespace to train a new model
    ''' 
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("model")>
    Public Function model(x As Object, Optional env As Environment = Nothing) As Object
        If x Is Nothing Then
            Return Internal.debug.stop("a required of the machine learning model object could not be nothing!", env)
        ElseIf TypeOf x Is DeclareNewFunction Then
        ElseIf TypeOf x Is String Then
            ' is model file path
            Return learning.readModelFile(x, env)
        Else
            Return Message.InCompatibleType(GetType(String), x.GetType, env)
        End If
    End Function

    <ExportAPI("readModelFile")>
    Public Function readModelFile(file As Object, Optional env As Environment = Nothing) As Object

    End Function
End Module
