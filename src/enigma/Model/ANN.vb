Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.Activations

Public Class ANN : Inherits MLModel

    Public Property data As Object
    Public Property input As Object
    Public Property hidden As HiddenLayerBuilderArgument
    Public Property output As OutputLayerBuilderArgument

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

End Class