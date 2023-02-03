Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork.StoreProcedure

Public Class ANNPackFile : Inherits MLPackFile(Of ANN)

    Public Overrides ReadOnly Property [Class] As String
        Get
            Return "ANN"
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ann"></param>
    ''' <param name="file"></param>
    Sub New(ann As ANN, file As Stream)
        Call MyBase.New(ann, file)
    End Sub

    Sub New(file As FileStream)
        Call MyBase.New(file)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Protected Overrides Sub WriteModel()
        Call CreateSnapshot _
            .TakeSnapshot(model.Model, {}) _
            .ScatteredStore(file)
    End Sub

    Protected Overrides Function Load() As MachineLearning.Model
        Return Scattered.ScatteredLoader(file).LoadModel
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function OpenRead(file As Stream) As ANN
        Dim network = New ANNPackFile(file).Load

        Return New ANN With {
            .Model = network
        }
    End Function
End Class
