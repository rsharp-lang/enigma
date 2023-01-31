Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.Activations

Public Class CustomFunction : Inherits IActivationFunction

    Public Overrides ReadOnly Property Store As ActiveFunction
        Get
            Return New ActiveFunction With {
                .name = "",
                .Arguments = {}
            }
        End Get
    End Property

    ReadOnly lambda As Func(Of Double, Double)

    Public Overrides Function [Function](x As Double) As Double
        Return lambda(x)
    End Function

    Public Overrides Function ToString() As String
        Return Store.ToString
    End Function

    Protected Overrides Function Derivative(x As Double) As Double
        Throw New NotImplementedException()
    End Function
End Class
