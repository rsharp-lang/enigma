Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.Activations
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork.Activations
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Vectorization

Public Class ANN : Inherits MLModel

    Public Property hidden As HiddenLayerBuilderArgument
    Public Property output As OutputLayerBuilderArgument

    Public Overrides Function DoCallTraining(args As list, env As Environment) As MLModel
        Dim activate As New LayerActives With {
            .output = output.activate,
            .hiddens = hidden.activate
        }
        Dim learnRate As Double = args.getValue({"learn", "learn.rate"}, env, [default]:=0.01)
        Dim model As New Network(
            inputSize:=Features.Length,
            hiddenSize:=hidden.size,
            outputSize:=Me.Labels.Length,
            active:=activate,
            learnRate:=learnRate
        )
        Dim trainer As New TrainingUtils(model)
        Dim truncate As Double = args.getValue({"truncate", "Truncate"}, env, [default]:=-1.0)
        Dim threshold As Double = args.getValue({"threshold"}, env, [default]:=0.01)
        Dim parallel As Boolean = args.getValue({"parallel", "Parallel"}, env, [default]:=False)
        Dim softmax As Boolean = args.getValue("softmax", env, [default]:=False)

        trainer.Truncate = truncate
        trainer.ErrorThreshold = threshold
        trainer.SetLayerNormalize(softmax)

        Helpers.MaxEpochs = args.getValue({"MaxEpochs", "max.epochs", "epochs"}, env, [default]:=10000)

        Call trainOnDataframe(trainer, data, parallel)

        Me.Model = model

        Return Me
    End Function

    Private Sub trainOnDataframe(trainer As TrainingUtils, data As dataframe, parallel As Boolean)
        Dim inputs = DirectCast(data, dataframe).forEachRow(Features).ToArray
        Dim outputs = DirectCast(data, dataframe).forEachRow(Me.Labels).ToArray
        Dim output_range As New Dictionary(Of String, DoubleRange)
        Dim std As DoubleRange = New Double() {0, 1}

        For Each field As String In Me.Labels
            Dim v As Double() = CLRVector.asNumeric(data(field))
            Dim range As New DoubleRange(v)

            output_range.Add(field, range)
        Next

        output.range = output_range.ToDictionary(Function(a) a.Key, Function(a) {a.Value.Min, a.Value.Max})

        For i As Integer = 0 To inputs.Length - 1
            Dim input As Double() = CLRVector.asNumeric(inputs(i).value)
            Dim output As Double() = CLRVector.asNumeric(outputs(i).value)

            Call Me.Labels _
                .Select(Function(key, idx)
                            output(idx) = output_range(key).ScaleMapping(output(idx), std)
                            Return 1
                        End Function) _
                .ToArray
            Call trainer.Add(input, output)
        Next

        trainer.Train(parallel)
    End Sub

    Public Overrides Function Solve(data As Object, env As Environment) As Object
        If TypeOf data Is dataframe Then
            Dim df As dataframe = DirectCast(data, dataframe)
            Dim inputs = df.forEachRow(Features).ToArray
            Dim rowNames As String() = df.getRowNames
            Dim outputs As New Dictionary(Of String, Double())
            Dim ANN As Network = Model
            Dim std As DoubleRange = New Double() {0, 1}

            For Each label As String In Me.Labels
                outputs.Add(label, New Double(rowNames.Length - 1) {})
            Next

            For i As Integer = 0 To inputs.Length - 1
                Dim v As Double() = CLRVector.asNumeric(inputs(i).value)
                Dim o As Double() = ANN.Compute(v)
                Dim j As i32 = Scan0

                For Each label As String In Me.Labels
                    outputs(label)(i) = std.ScaleMapping(o(++j), output.range(label))
                Next
            Next

            Dim result As New dataframe(df)

            For Each name As String In outputs.Keys
                Call result.columns.Add("PREDICT_OUTPUT: " & name, outputs(name))
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

    Public Property activate As IActivationFunction
    Public Property range As Dictionary(Of String, Double())

End Class
