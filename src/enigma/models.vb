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
''' ### Create the machine learning model
''' 
''' Performing machine learning involves creating a model, which is trained
''' on some training data and then can process additional data to make predictions.
''' Various types of models have been used and researched for machine 
''' learning systems.
''' </summary>
<Package("model")>
Public Module models

    ''' <summary>
    ''' ### create artificial neural network
    ''' 
    ''' Artificial neural networks (ANNs), or connectionist systems, are 
    ''' computing systems vaguely inspired by the biological neural networks
    ''' that constitute animal brains. Such systems "learn" to perform 
    ''' tasks by considering examples, generally without being programmed 
    ''' with any task-specific rules.
    '''
    ''' An ANN Is a model based On a collection Of connected units Or nodes
    ''' called "artificial neurons", which loosely model the neurons In a 
    ''' biological brain. Each connection, Like the synapses In a biological 
    ''' brain, can transmit information, a "signal", from one artificial 
    ''' neuron To another. An artificial neuron that receives a signal can
    ''' process it And Then signal additional artificial neurons connected 
    ''' To it. In common ANN implementations, the signal at a connection 
    ''' between artificial neurons Is a real number, And the output Of Each 
    ''' artificial neuron Is computed by some non-linear Function Of the sum 
    ''' Of its inputs. The connections between artificial neurons are called 
    ''' "edges". Artificial neurons And edges typically have a weight that 
    ''' adjusts As learning proceeds. The weight increases Or decreases the
    ''' strength Of the signal at a connection. Artificial neurons may have 
    ''' a threshold such that the signal Is only sent If the aggregate signal
    ''' crosses that threshold. Typically, artificial neurons are aggregated 
    ''' into layers. Different layers may perform different kinds Of transformations 
    ''' On their inputs. Signals travel from the first layer (the input layer)
    ''' To the last layer (the output layer), possibly after traversing the
    ''' layers multiple times.
    '''
    ''' The original goal Of the ANN approach was To solve problems In the
    ''' same way that a human brain would. However, over time, attention 
    ''' moved To performing specific tasks, leading To deviations from biology. 
    ''' Artificial neural networks have been used On a variety Of tasks,
    ''' including computer vision, speech recognition, machine translation,
    ''' social network filtering, playing board And video games And medical 
    ''' diagnosis.
    '''
    ''' Deep learning consists Of multiple hidden layers In an artificial 
    ''' neural network. This approach tries To model the way the human brain 
    ''' processes light And sound into vision And hearing. Some successful 
    ''' applications Of deep learning are computer vision And speech recognition.
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

    <ExportAPI("randomForest")>
    Public Function randomForest(Optional regression As Boolean = False) As RandomForest
        Return New RandomForest With {.regression = regression}
    End Function

    <ExportAPI("randomForest.regression")>
    Public Function randomForestRegression() As RandomForest
        Return New RandomForest With {.regression = True}
    End Function

    ''' <summary>
    ''' ### Support-vector machines
    '''
    ''' Support-vector machines (SVMs), also known as support-vector 
    ''' networks, are a set of related supervised learning methods 
    ''' used for classification And regression. Given a set of training 
    ''' examples, each marked as belonging to one of two categories, 
    ''' an SVM training algorithm builds a model that predicts whether 
    ''' a New example falls into one category. An SVM training 
    ''' algorithm Is a non-probabilistic, binary, linear classifier, 
    ''' although methods such as Platt scaling exist to use SVM in a 
    ''' probabilistic classification setting. In addition to performing
    ''' linear classification, SVMs can efficiently perform a non-linear 
    ''' classification using what Is called the kernel trick, 
    ''' implicitly mapping their inputs into high-dimensional feature 
    ''' spaces.
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
