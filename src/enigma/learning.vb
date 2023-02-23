
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
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
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports REnv = SMRUCC.Rsharp.Runtime

''' <summary>
''' ## machine learning toolkit
''' 
''' Machine learning (ML) is a field of inquiry devoted to
''' understanding and building methods that "learn" – that
''' is, methods that leverage data to improve performance 
''' on some set of tasks. It is seen as a part of artificial
''' intelligence.
'''
''' Machine learning algorithms build a model based On sample
''' data, known As training data, In order To make predictions
''' Or decisions without being explicitly programmed To Do so.
''' Machine learning algorithms are used In a wide variety Of 
''' applications, such As In medicine, email filtering, speech 
''' recognition, agriculture, And computer vision, where it Is
''' difficult Or unfeasible To develop conventional algorithms 
''' To perform the needed tasks.
'''
''' A subset Of machine learning Is closely related To computational 
''' statistics, which focuses On making predictions Using computers,
''' but Not all machine learning Is statistical learning. The 
''' study Of mathematical optimization delivers methods, theory 
''' And application domains To the field Of machine learning. Data
''' mining Is a related field Of study, focusing On exploratory 
''' data analysis through unsupervised learning.
'''
''' Some implementations Of machine learning use data And neural 
''' networks In a way that mimics the working Of a biological 
''' brain.
'''
''' In its application across business problems, machine learning 
''' Is also referred to as predictive analytics.
''' </summary>
<Package("learning")>
Public Module learning

    ''' <summary>
    ''' ### create a new machine learning model
    ''' 
    ''' In mathematics, a tensor is an algebraic object that describes
    ''' a multilinear relationship between sets of algebraic objects 
    ''' related to a vector space. Tensors may map between different 
    ''' objects such as vectors, scalars, and even other tensors. There
    ''' are many types of tensors, including scalars and vectors (which 
    ''' are the simplest tensors), dual vectors, multilinear maps between 
    ''' vector spaces, and even some operations such as the dot product.
    ''' Tensors are defined independent of any basis, although they are 
    ''' often referred to by their components in a basis related to a 
    ''' particular coordinate system.
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
    ''' <remarks>
    ''' Typically, machine learning models require a high quantity of 
    ''' reliable data in order for the models to perform accurate predictions. 
    ''' When training a machine learning model, machine learning engineers
    ''' need to target and collect a large and representative sample of 
    ''' data. Data from the training set can be as varied as a corpus of 
    ''' text, a collection of images, sensor data, and data collected from 
    ''' individual users of a service. Overfitting is something to watch out
    ''' for when training a machine learning model. Trained models derived 
    ''' from biased or non-evaluated data can result in skewed or undesired 
    ''' predictions. Bias models may result in detrimental outcomes thereby 
    ''' furthering the negative impacts on society or objectives. Algorithmic
    ''' bias is a potential result of data not being fully prepared for 
    ''' training. Machine learning ethics is becoming a field of study and 
    ''' notably be integrated within machine learning engineering teams.
    ''' </remarks>
    <ExportAPI("feed")>
    <RApiReturn(GetType(MLModel))>
    <Extension>
    Public Function feed(model As MLModel, x As Object,
                         <RRawVectorArgument> features As Object,
                         <RListObjectArgument>
                         Optional args As list = Nothing,
                         Optional env As Environment = Nothing) As MLModel
        model.data = x
        model.Features = CLRVector.asCharacter(features)

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
        Dim sizeVec As Integer() = CLRVector.asInteger(size)

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
    ''' ### configs of the output labels 
    ''' 
    ''' configs of the output labels of the feeded training data 
    ''' for machine learning model
    ''' </summary>
    ''' <param name="model"></param>
    ''' <param name="labels">
    ''' The data field name for get the actual label value from the input
    ''' training data
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("output")>
    <RApiReturn(GetType(MLModel))>
    <Extension>
    Public Function config_output_labels(model As MLModel,
                                         <RRawVectorArgument>
                                         Optional labels As Object = Nothing,
                                         <RListObjectArgument>
                                         Optional args As list = Nothing,
                                         Optional env As Environment = Nothing) As MLModel

        Dim labelStr As String() = CLRVector.asCharacter(labels)

        If TypeOf model Is ANN Then
            Dim f = activateFunction.getFunction(args.getByName("activate"), env)

            If f Like GetType(Message) Then
                Return f.TryCast(Of Message).CreateError
            End If

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
    ''' <param name="model">
    ''' A machine learning algorithm model which is generated from 
    ''' the ``tensor`` function.
    ''' </param>
    ''' <param name="args">
    ''' the additional arguments to the machine learning model trainer, it could be:
    ''' 
    ''' 1. Artificial neural networks model
    ''' 
    ''' + truncate, numeric
    ''' + threshold, numeric
    ''' + parallel, logical
    ''' + softmax, logical
    ''' + max.epochs, integer
    ''' 
    ''' 2. xgboost
    ''' 
    ''' + loss,Loss: logloss for classification and squareloss for regression
    ''' + cost,eval_metric: auc for classification and mse for regression
    ''' + gamma, numeric: default 0.0
    ''' + lambda, numeric: default 1.0
    ''' + learn_rate, eta, numeric: default 0.3
    ''' + max_depth, integer: default 7
    ''' + num_boost_round, integer, default 10
    ''' + max, maximize, logical: default true
    ''' 
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
    ''' <param name="model">
    ''' A machine learning model which is has been trained via the ``learn`` function.
    ''' Or this machine learning algorithm model also could be loaded from the snapshot 
    ''' file via the ``readModelFile``.
    ''' </param>
    ''' <param name="data">
    ''' A dataframe object which should contains the same features fields with the 
    ''' input training data
    ''' </param>
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
    <RApiReturn("train", "test", "validate")>
    Public Function trainTestSplit(x As dataframe,
                                   Optional train_ratio As Double = 0.8,
                                   Optional cross As Double = 0.2,
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
        Dim trainCross = train.Shuffles.Take(CInt(train.Length * cross) + 1).ToArray
        Dim test = rows.Skip(train.Length).ToArray
        Dim testCross = test.Shuffles.Take(CInt(test.Length * cross) + 1).ToArray
        Dim df_train = dataframe.CreateDataFrame(train.JoinIterates(testCross).ToArray, fields)
        Dim df_tests = dataframe.CreateDataFrame(test.JoinIterates(trainCross).ToArray, fields)
        Dim validates = dataframe.CreateDataFrame(trainCross.JoinIterates(testCross).ToArray, fields)

        Return New list With {
            .slots = New Dictionary(Of String, Object) From {
                {"train", df_train},
                {"test", df_tests},
                {"validate", validates}
            }
        }
    End Function
End Module
