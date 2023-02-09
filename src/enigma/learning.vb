
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.MachineLearning
Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.Activations
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork
Imports Microsoft.VisualBasic.MachineLearning.XGBoost.train
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Closure
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports REnv = SMRUCC.Rsharp.Runtime

''' <summary>
''' machine learning toolkit
''' </summary>
<Package("learning")>
Public Module learning

    ''' <summary>
    ''' create a new machine learning model
    ''' </summary>
    ''' <param name="model">
    ''' the source of the machine learning model where it comes from:
    ''' 
    ''' 1. could be a file name to the trained model file
    ''' 2. could be a function name in ``models`` namespace to train a new model
    ''' 
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("tensor")>
    <RApiReturn(GetType(MLModel))>
    Public Function tensorModel(model As Object, Optional env As Environment = Nothing) As MLModel
        If model Is Nothing Then
            Return Internal.debug _
                .stop("a required of the machine learning model object could not be nothing!", env) _
                .CreateError
        ElseIf TypeOf model Is DeclareNewFunction Then
            Return checkModel(model:=DirectCast(model, Expression).Evaluate(env), env)
        ElseIf TypeOf model Is RMethodInfo Then
            Return checkModel(model:=DirectCast(model, RMethodInfo).Invoke(env, {}), env)
        ElseIf TypeOf model Is String Then
            ' is model file path
            Return enigma.models.readModelFile(model, env)
        ElseIf TypeOf model Is MLModel Then
            Return model
        ElseIf TypeOf model Is Model Then
            Return wrapModel(model, env)
        Else
            Return Message _
                .InCompatibleType(GetType(String), model.GetType, env) _
                .CreateError
        End If
    End Function

    ''' <summary>
    ''' wrap model object from other CLR function outputs
    ''' </summary>
    ''' <param name="model"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    Private Function wrapModel(model As Model, env As Environment) As MLModel
        Select Case model.GetType
            Case GetType(Network) : Return New ANN With {.Model = model}
            Case GetType(GBM) : Return New XGBoost With {.Model = model}
            Case Else
                Return Internal.debug _
                    .stop(New NotImplementedException(model.GetType.FullName), env) _
                    .CreateError
        End Select
    End Function

    Private Function checkModel(model As Object, env As Environment) As MLModel
        If Program.isException(model) Then
            Return model
        ElseIf TypeOf model Is MLModel Then
            Return model
        ElseIf TypeOf model Is Model Then
            Return wrapModel(model, env)
        Else
            Return Internal.debug _
                .stop("invalid model function, the function should be procude a new machine learning model object!", env) _
                .CreateError
        End If
    End Function

    ''' <summary>
    ''' feed training data to the machine learning model
    ''' </summary>
    ''' <param name="model">A machine learning model object</param>
    ''' <param name="x">
    ''' the training data to feed to the model object, 
    ''' it usually be a dataframe object.
    ''' </param>
    ''' <param name="features">
    ''' usually a character vector to gets the training data fields
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("feed")>
    <RApiReturn(GetType(MLModel))>
    <Extension>
    Public Function feed(model As MLModel, x As Object,
                         <RRawVectorArgument> features As Object,
                         <RListObjectArgument>
                         Optional args As list = Nothing,
                         Optional env As Environment = Nothing) As MLModel
        model.data = x
        model.Features = REnv.asVector(Of String)(features)

        Return model
    End Function

    ''' <summary>
    ''' configs of the hidden layer of the ``<see cref="ANN"/>`` model
    ''' </summary>
    ''' <param name="model">The ``<see cref="ANN"/>`` machine learning model object</param>
    ''' <param name="size">A integer vector for configs the layer size in the hidden layer</param>
    ''' <param name="activate">Activation function of the hidden layer</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("hidden_layer")>
    <RApiReturn(GetType(MLModel))>
    <Extension>
    Public Function hidden_layer(model As ANN, <RRawVectorArgument> size As Object,
                                 Optional activate As Object = Nothing,
                                 Optional env As Environment = Nothing) As MLModel

        Dim f = activateFunction.getFunction(activate, env)
        Dim sizeVec As Integer() = REnv.asVector(Of Integer)(size)

        If f Like GetType(Message) Then
            Return f.TryCast(Of Message).CreateError
        End If

        model.hidden = New HiddenLayerBuilderArgument With {
            .size = sizeVec,
            .activate = f.TryCast(Of IActivationFunction)
        }

        Return model
    End Function

    ''' <summary>
    ''' configs of the output layer of the ``<see cref="ANN"/>`` model
    ''' </summary>
    ''' <param name="model"></param>
    ''' <param name="labels"></param>
    ''' <param name="activate"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("output")>
    <RApiReturn(GetType(MLModel))>
    <Extension>
    Public Function output_layer(model As MLModel,
                                 <RRawVectorArgument>
                                 Optional labels As Object = Nothing,
                                 Optional activate As Object = Nothing,
                                 Optional env As Environment = Nothing) As MLModel

        Dim labelStr As String() = REnv.asVector(Of String)(labels)
        Dim f = activateFunction.getFunction(activate, env)

        If f Like GetType(Message) Then
            Return f.TryCast(Of Message).CreateError
        ElseIf TypeOf model Is ANN Then
            DirectCast(model, ANN).output = New OutputLayerBuilderArgument With {
                .activate = f.TryCast(Of IActivationFunction)
            }
        End If

        model.Labels = labelStr

        Return model
    End Function

    ''' <summary>
    ''' Do machine learning model training
    ''' </summary>
    ''' <param name="model"></param>
    ''' <param name="args">
    ''' the additional arguments to the machine learning model trainer, it could be:
    ''' 
    ''' 1. ANN model
    ''' 
    ''' + truncate, numeric
    ''' + threshold, numeric
    ''' + parallel, logical
    ''' + softmax, logical
    ''' + max.epochs, integer
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("learn")>
    <RApiReturn(GetType(MLModel))>
    <Extension>
    Public Function learn(model As MLModel,
                          <RListObjectArgument>
                          Optional args As list = Nothing,
                          Optional env As Environment = Nothing) As MLModel

        Return model.DoCallTraining(args, env)
    End Function

    ''' <summary>
    ''' Do data prediction
    ''' </summary>
    ''' <param name="model"></param>
    ''' <param name="data"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("solve")>
    <Extension>
    Public Function fitData(model As MLModel, data As Object, Optional env As Environment = Nothing) As Object
        Return model.Solve(data, env)
    End Function

    ''' <summary>
    ''' ### Split a Dataset into Train and Test Sets
    ''' 
    ''' The train-test split is used to estimate the performance
    ''' of machine learning algorithms that are applicable for 
    ''' prediction-based Algorithms/Applications. This method is 
    ''' a fast and easy procedure to perform such that we can 
    ''' compare our own machine learning model results to machine
    ''' results. By default, the Test set is split into 30 % of 
    ''' actual data and the training set is split into 70% of the 
    ''' actual data.
    ''' 
    ''' We need To split a dataset into train And test sets To 
    ''' evaluate how well our machine learning model performs. The
    ''' train Set Is used To fit the model, And the statistics Of
    ''' the train Set are known. The second Set Is called the test
    ''' data Set, this Set Is solely used For predictions.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="train_ratio"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("trainTestSplit")>
    <RApiReturn("train", "test")>
    Public Function trainTestSplit(x As dataframe,
                                   Optional train_ratio As Double = 0.8,
                                   Optional env As Environment = Nothing) As Object

        If train_ratio <= 0 OrElse train_ratio > 1 Then
            Return Internal.debug.stop($"invalid training set ratio, ratio value should be in range of (0,1], and you have set an invalid value '{train_ratio}' for parameter {NameOf(train_ratio)}!", env)
        ElseIf x Is Nothing Then
            Return New list() With {
                .slots = New Dictionary(Of String, Object) From {
                    {"train", Nothing},
                    {"test", Nothing}
                }
            }
        End If

        Dim fields As String() = x.colnames
        Dim rows As NamedCollection(Of Object)() = x.forEachRow(fields).Shuffles.ToArray
        Dim train = rows.Take(CInt(train_ratio * rows.Length) + 1).ToArray
        Dim test = rows.Skip(train.Length).ToArray
        Dim df_train = dataframe.CreateDataFrame(train, fields)
        Dim df_tests = dataframe.CreateDataFrame(test, fields)

        Return New list With {
            .slots = New Dictionary(Of String, Object) From {
                {"train", df_train},
                {"test", df_tests}
            }
        }
    End Function
End Module
