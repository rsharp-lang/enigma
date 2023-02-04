Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.Language
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
    ''' ### XGBoost (eXtreme Gradient Boosting)
    ''' 
    ''' XGBoost is an optimized distributed gradient boosting library
    ''' designed to be highly efficient, flexible and portable. 
    ''' It implements machine learning algorithms under the Gradient
    ''' Boosting framework. XGBoost provides a parallel tree boosting 
    ''' (also known as GBDT, GBM) that solve many data science 
    ''' problems in a fast and accurate way. 
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("xgboost")>
    Public Function xgboost() As XGBoost
        Return New XGBoost
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("svm")>
    Public Function svmModel() As SVMModel
        Return New SVMModel
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("svr")>
    Public Function svrModel() As SVMModel
        Return New SVMModel With {.svr = True}
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
        Dim writer = model.StorageProvider(file.Open(FileMode.OpenOrCreate, doClear:=True), env)

        If writer Like GetType(Message) Then
            Return writer.TryCast(Of Message)
        Else
            Call writer.TryCast(Of MLPackFile).Write()
            Call writer.Dispose()
        End If

        Return True
    End Function

    <Extension>
    Private Function StorageProvider(model As MLModel, file As Stream, env As Environment) As [Variant](Of Message, MLPackFile)
        If TypeOf model Is ANN Then
            Return New ANNPackFile(model, file)
        ElseIf TypeOf model Is XGBoost Then
            Return New XGBoostPackFile(model, file)
        Else
            Return Message.InCompatibleType(GetType(MLModel), model.GetType, env)
        End If
    End Function

    ''' <summary>
    ''' load saved machine learning model
    ''' </summary>
    ''' <param name="file">A file path to the model snapshot data file.</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("readModelFile")>
    Public Function readModelFile(<RRawVectorArgument> file As Object, Optional env As Environment = Nothing) As MLModel
        Dim data = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Read, env)

        If data Like GetType(Message) Then
            Return New MLPipelineError(data.TryCast(Of Message))
        End If

        Using buffer As Stream = data.TryCast(Of Stream)
            Dim pack As New StreamPack(buffer, [readonly]:=True)
            Dim cls As String = MLPackFile(Of MLModel).GetClass(pack)

            Select Case cls
                Case "ANN" : Return ANNPackFile.OpenRead(buffer)
                Case "xgboost" : Return XGBoostPackFile.OpenRead(buffer)
                Case Else
                    Return Internal.debug _
                        .stop($"unsure how to parse the model file with class label: '{cls}'", env) _
                        .CreateError
            End Select
        End Using
    End Function
End Module
