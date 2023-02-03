Imports System.IO
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
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

    <ExportAPI("xgboost")>
    Public Function xgboost() As XGBoost
        Return New XGBoost
    End Function

    <ExportAPI("ANN.regression")>
    Public Function ANNRegression() As ANNRegression
        Return New ANNRegression
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
            Using writer As New ANNPackFile(model, file.Open(FileMode.OpenOrCreate, doClear:=True))
                Call writer.Write()
            End Using
        ElseIf TypeOf model Is XGBoost Then
            Using writer As New StreamPack(file.Open(FileMode.OpenOrCreate, doClear:=True))
                Call writer.WriteText(DirectCast(model, XGBoost).GetModelFile, "/xgboost.txt")
                Call writer.WriteText(model.Features, "/features.txt")
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
    Public Function readModelFile(<RRawVectorArgument> file As Object, Optional env As Environment = Nothing) As Object
        Dim data = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Read, env)

        If data Like GetType(Message) Then
            Return data.TryCast(Of Message)
        End If

        Using buffer As Stream = data.TryCast(Of Stream)
            Dim pack As New StreamPack(buffer, [readonly]:=True)
            Dim cls As String = pack.ReadText("/etc/model.class").Trim

            Select Case cls
                Case "ANN"
                    Return ANNPackFile.OpenRead(buffer)
                Case "xgboost"
                    Return Internal.debug.stop($"unsure how to parse the model file with class label: '{cls}'", env)
                Case Else
                    Return Internal.debug.stop($"unsure how to parse the model file with class label: '{cls}'", env)
            End Select
        End Using
    End Function
End Module
