Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime

<Package("model")>
Public Module models

    ''' <summary>
    ''' function for create artificial neurol network
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("ANN")>
    Public Function ANN() As ANN
        Return New ANN
    End Function

    <ExportAPI("snapshot")>
    Public Function snapshot(model As Model, file As String, Optional env As Environment = Nothing) As Object

    End Function

    <ExportAPI("readModelFile")>
    Public Function readModelFile(file As Object, Optional env As Environment = Nothing) As Object

    End Function
End Module
