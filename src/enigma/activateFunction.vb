
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.Activations
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime

<Package("activateFunction")>
Public Module activateFunction

    <ExportAPI("sigmoid")>
    Public Function Sigmoid(Optional alpha As Double = 0.2) As IActivationFunction
        Return New Sigmoid(alpha)
    End Function

    ''' <summary>
    ''' create a new custom activate function
    ''' </summary>
    ''' <param name="forward"></param>
    ''' <param name="derivative"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("func")>
    Public Function customFunc(forward As Object, derivative As Object, Optional env As Environment = Nothing) As Object

    End Function
End Module
