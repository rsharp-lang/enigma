Imports System.IO
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.MachineLearning.XGBoost.train

Public Class XGBoostPackFile : Inherits MLPackFile(Of XGBoost)

    Public Overrides ReadOnly Property [Class] As String
        Get
            Return "xgboost"
        End Get
    End Property

    Public Sub New(file As Stream)
        MyBase.New(file)
    End Sub

    Public Sub New(model As XGBoost, file As Stream)
        MyBase.New(model, file)
    End Sub

    Protected Overrides Sub WriteModel()
        Call file.WriteText(model.GetModelFile, "/xgboost.txt")
    End Sub

    Protected Overrides Function Load() As MachineLearning.Model
        Dim modelLines As String() = file.ReadText("/xgboost.txt").LineTokens
        Dim gb As GBM = ModelSerializer.load_model(modelLines)

        Return gb
    End Function

    Public Shared Function OpenRead(file As Stream) As XGBoost
        Dim reader As New XGBoostPackFile(file)

        Return New XGBoost With {
            .Model = reader.Load
        }
    End Function
End Class
