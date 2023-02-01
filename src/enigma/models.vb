Imports System.IO
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' Create the machine learning model
''' </summary>
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

    ''' <summary>
    ''' make the snapshot of the machine learning model
    ''' </summary>
    ''' <param name="model">A machine learning model</param>
    ''' <param name="file">A file path value, the model snapshot will be save to this file.</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("snapshot")>
    <RApiReturn(GetType(Boolean))>
    Public Function snapshot(model As MLModel, file As String, Optional env As Environment = Nothing) As Object
        If TypeOf model Is ANN Then
            Using writer As New ANNPackFile(model.Model, file.Open(FileMode.OpenOrCreate, doClear:=True))
                Call writer.Write()
            End Using
        Else
            Return Message.InCompatibleType(GetType(MLModel), model.GetType, env)
        End If

        Return True
    End Function

    ''' <summary>
    ''' load saved machine learning model
    ''' </summary>
    ''' <param name="file">A file path to the model snapshot data file.</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("readModelFile")>
    Public Function readModelFile(file As Object, Optional env As Environment = Nothing) As Object

    End Function
End Module
