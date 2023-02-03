Imports Microsoft.VisualBasic.MachineLearning.XGBoost.train
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object

Public Class XGBoost : Inherits MLModel

    Public Function GetModelFile() As String()
        Return ModelSerializer.save_model(Model).ToArray
    End Function

    Public Overrides Function DoCallTraining(args As list, env As Environment) As MLModel
        Throw New NotImplementedException()
    End Function

    Public Overrides Function Solve(data As Object, env As Environment) As Object
        Throw New NotImplementedException()
    End Function
End Class
