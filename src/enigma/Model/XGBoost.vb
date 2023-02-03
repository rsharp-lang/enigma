Imports Microsoft.VisualBasic.MachineLearning.XGBoost.train
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Public Class XGBoost : Inherits MLModel

    Public Function GetModelFile() As String()
        Return ModelSerializer.save_model(Model).ToArray
    End Function

    Public Overrides Function DoCallTraining(args As list, env As Environment) As MLModel
        Dim output As Double() = REnv.asVector(Of Double)(data(Me.Labels(Scan0)))
        Dim trainData As TrainData = data.trainingDataSet(output, featureNames:=Features)
        Dim gb As New GBM()

        Call gb.fit(trainData, Nothing)
        Me.Model = gb

        Return Me
    End Function

    Public Overrides Function Solve(data As Object, env As Environment) As Object
        Throw New NotImplementedException()
    End Function
End Class
