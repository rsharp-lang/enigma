Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.Activations
Imports Microsoft.VisualBasic.Text.Xml.Models

Public Class CustomFunction : Inherits IActivationFunction

    Public Overrides ReadOnly Property Store As ActiveFunction
        Get
            Return New ActiveFunction With {
                .name = "f",
                .Arguments = {New NamedValue("x", "0.0")}
            }
        End Get
    End Property

    ReadOnly m_lambda As Func(Of Double, Double)
    ReadOnly m_derivative As Func(Of Double, Double)

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function [Function](x As Double) As Double
        Return m_lambda(x)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function ToString() As String
        Return Store.ToString
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Protected Overrides Function Derivative(x As Double) As Double
        Return m_derivative(x)
    End Function
End Class
