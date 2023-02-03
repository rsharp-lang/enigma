Imports Microsoft.VisualBasic.MachineLearning
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object

Public Class SVMModel : Inherits MLModel

    Public Overrides Function DoCallTraining(args As list, env As Environment) As MLModel
        Dim problem As SVM.Problem
        Dim params As SVM.Parameter

        Me.Model = SVM.Train(problem, params)

        Return Me
    End Function

    Public Overrides Function Solve(data As Object, env As Environment) As Object

    End Function
End Class
