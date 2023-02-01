
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.Activations
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
    Public Function tensorModel(model As Object, Optional env As Environment = Nothing) As Object
        If model Is Nothing Then
            Return Internal.debug.stop("a required of the machine learning model object could not be nothing!", env)
        ElseIf TypeOf model Is DeclareNewFunction Then
            Return checkModel(model:=DirectCast(model, Expression).Evaluate(env), env)
        ElseIf TypeOf model Is RMethodInfo Then
            Return checkModel(model:=DirectCast(model, RMethodInfo).Invoke(env, {}), env)
        ElseIf TypeOf model Is String Then
            ' is model file path
            Return enigma.models.readModelFile(model, env)
        ElseIf TypeOf model Is MLModel Then
            Return model
        Else
            Return Message.InCompatibleType(GetType(String), model.GetType, env)
        End If
    End Function

    Private Function checkModel(model As Object, env As Environment) As Object
        If Program.isException(model) Then
            Return model
        ElseIf TypeOf model Is MLModel Then
            Return model
        Else
            Return Internal.debug.stop("invalid model function, the function should be procude a new machine learning model object!", env)
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
    Public Function feed(model As MLModel, x As Object,
                         <RRawVectorArgument> features As Object,
                         <RListObjectArgument>
                         Optional args As list = Nothing,
                         Optional env As Environment = Nothing) As Object

        If TypeOf model Is ANN Then
            Dim ANN As ANN = DirectCast(model, ANN)

            ANN.data = x
            ANN.input = REnv.asVector(Of String)(features)

            Return model
        Else
            Return Internal.debug.stop(New NotImplementedException(model.GetType.FullName), env)
        End If
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
    Public Function hidden_layer(model As ANN, <RRawVectorArgument> size As Object,
                                 Optional activate As Object = Nothing,
                                 Optional env As Environment = Nothing) As Object

        Dim f = activateFunction.getFunction(activate, env)
        Dim sizeVec As Integer() = REnv.asVector(Of Integer)(size)

        If f Like GetType(Message) Then
            Return f.TryCast(Of Message)
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
    <ExportAPI("output_layer")>
    <RApiReturn(GetType(MLModel))>
    Public Function output_layer(model As ANN,
                                 <RRawVectorArgument>
                                 Optional labels As Object = Nothing,
                                 Optional activate As Object = Nothing,
                                 Optional env As Environment = Nothing) As Object

        Dim labelStr As String() = REnv.asVector(Of String)(labels)
        Dim f = activateFunction.getFunction(activate, env)

        If f Like GetType(Message) Then
            Return f.TryCast(Of Message)
        End If

        model.output = New OutputLayerBuilderArgument With {
            .labels = labelStr,
            .activate = f.TryCast(Of IActivationFunction)
        }

        Return model
    End Function

    ''' <summary>
    ''' Do machine learning model training
    ''' </summary>
    ''' <param name="model"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("learn")>
    <RApiReturn(GetType(MLModel))>
    Public Function learn(model As MLModel, Optional env As Environment = Nothing) As Object
        Return model.DoCallTraining
    End Function

    ''' <summary>
    ''' Do data prediction
    ''' </summary>
    ''' <param name="model"></param>
    ''' <param name="data"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("solve")>
    Public Function fitData(model As MLModel, data As Object, Optional env As Environment = Nothing) As Object
        Return model.Solve(data, env)
    End Function
End Module
