Imports Microsoft.VisualBasic.MachineLearning
Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.Activations
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork.Activations
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports REnv = SMRUCC.Rsharp.Runtime

Public Class ANN : Inherits MLModel

    Public Property data As Object
    Public Property input As String()
    Public Property hidden As HiddenLayerBuilderArgument
    Public Property output As OutputLayerBuilderArgument

    Public Overrides Function DoCallTraining() As Model
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

        If TypeOf data Is dataframe Then
            Dim inputs = DirectCast(data, dataframe).forEachRow(input).ToArray
            Dim outputs = DirectCast(data, dataframe).forEachRow(output.labels).ToArray

            For i As Integer = 0 To inputs.Length - 1
                Dim input As Double() = renv.asvector(Of Double)(inputs(i).value)
                Dim output As Double() = REnv.asVector(Of Double)(outputs(i).value)

                Call trainer.Add(input, output)
            Next

            Call trainer.Train()
        Else
            Throw New NotImplementedException
        End If

        Return model
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

    Public MustOverride Function DoCallTraining() As Model

End Class