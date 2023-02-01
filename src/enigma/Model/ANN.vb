Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MachineLearning
Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.Activations
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork.Activations
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports REnv = SMRUCC.Rsharp.Runtime

Public Class ANN : Inherits MLModel

    Public Property data As Object
    Public Property input As String()
    Public Property hidden As HiddenLayerBuilderArgument
    Public Property output As OutputLayerBuilderArgument

    Public Overrides Function DoCallTraining(args As list, env As Environment) As MLModel
        Dim activate As New LayerActives With {
            .output = output.activate,
            .hiddens = hidden.activate
        }
        Dim model As New Network(
            inputSize:=input.Length,
            hiddenSize:=hidden.size,
            outputSize:=output.labels.Length,
            active:=activate
        )
        Dim trainer As New TrainingUtils(model)
        Dim truncate As Double = args.getValue(Of Double)({"truncate", "Truncate"}, env, [default]:=-1.0)

        trainer.Truncate = truncate

        If TypeOf data Is dataframe Then
            Dim inputs = DirectCast(data, dataframe).forEachRow(input).ToArray
            Dim outputs = DirectCast(data, dataframe).forEachRow(output.labels).ToArray

            For i As Integer = 0 To inputs.Length - 1
                Dim input As Double() = REnv.asVector(Of Double)(inputs(i).value)
                Dim output As Double() = REnv.asVector(Of Double)(outputs(i).value)

                Call trainer.Add(input, output)
            Next

            Call trainer.Train()
        Else
            Throw New NotImplementedException
        End If

        Me.Model = model

        Return Me
    End Function

    Public Overrides Function Solve(data As Object, env As Environment) As Object
        If TypeOf data Is dataframe Then
            Dim df As dataframe = DirectCast(data, dataframe)
            Dim inputs = df.forEachRow(input).ToArray
            Dim rowNames As String() = df.getRowNames
            Dim outputs As New Dictionary(Of String, Double())
            Dim ANN As Network = Model

            For Each label As String In output.labels
                outputs.Add(label, New Double(rowNames.Length - 1) {})
            Next

            For i As Integer = 0 To inputs.Length - 1
                Dim v As Double() = REnv.asVector(Of Double)(inputs(i).value)
                Dim o As Double() = ANN.Compute(v)
                Dim j As i32 = Scan0

                For Each label As String In output.labels
                    outputs(label)(i) = o(++j)
                Next
            Next

            Dim result As New dataframe With {
                .columns = New Dictionary(Of String, Array),
                .rownames = rowNames
            }

            For Each name As String In outputs.Keys
                Call result.columns.Add(name, outputs(name))
            Next

            Return result
        Else
            Throw New NotImplementedException
        End If
    End Function
End Class

Public Class HiddenLayerBuilderArgument

    Public Property size As Integer()
    Public Property activate As IActivationFunction

End Class

Public Class OutputLayerBuilderArgument

    Public Property labels As String()
    Public Property activate As IActivationFunction

End Class

''' <summary>
''' the machine learning model
''' </summary>
Public MustInherit Class MLModel

    ''' <summary>
    ''' the trained machine learning model object
    ''' </summary>
    ''' <returns></returns>
    Public Property Model As Model

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns>
    ''' this function just returns it self with 
    ''' the <see cref="Model"/> property value 
    ''' updated.
    ''' </returns>
    Public MustOverride Function DoCallTraining(args As list, env As Environment) As MLModel

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="env"></param>
    ''' <returns>
    ''' this function should returns a <see cref="dataframe"/> object.
    ''' </returns>
    Public MustOverride Function Solve(data As Object, env As Environment) As Object

End Class