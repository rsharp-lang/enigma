Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("models")>
Public Module models

    ''' <summary>
    ''' function for create artificial neurol network
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("ANN")>
    Public Function ANN() As ANN
        Return New ANN
    End Function

End Module
