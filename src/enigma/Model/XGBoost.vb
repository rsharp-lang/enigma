Imports Microsoft.VisualBasic.DataMining.Evaluation
Imports Microsoft.VisualBasic.MachineLearning.XGBoost.train
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
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

        ' squareloss + mse 
        ' for regression problem
        Dim loss As String = args.getValue({"loss", "Loss"}, env, [default]:="logloss")
        Dim cost As String = args.getValue({"cost", "eval_metric"}, env, [default]:="auc")
        Dim eval_metric As Metrics = Metric.Parse(cost)

        Call gb.fit(trainData, Nothing, loss:=loss, eval_metric:=eval_metric)
        Me.Model = gb

        Return Me
    End Function

    Public Overrides Function Solve(data As Object, env As Environment) As Object
        If TypeOf data Is dataframe Then
            Dim df As dataframe = DirectCast(data, dataframe)
            Dim test As TestData = df.testDataSet(Features)
            Dim output As Double() = DirectCast(Model, GBM).predict(test.origin_feature)

            df = New dataframe(df)
            df.add($"{Labels(Scan0)}(predicts)", output)

            Return df
        Else
            Return Message.InCompatibleType(GetType(dataframe), data.GetType, env)
        End If
    End Function
End Class
