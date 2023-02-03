Imports Microsoft.VisualBasic.MachineLearning
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Public Class SVMModel : Inherits MLModel

    Public Overrides Function DoCallTraining(args As list, env As Environment) As MLModel
        Dim labels As String() = REnv.asVector(Of String)(data(Me.Labels(Scan0)))
        Dim problem = svmDataSet.svmProblem(Features, labels, data, env)
        Dim params As New SVM.Parameter

        If problem Like GetType(Message) Then
            Call problem.TryCast(Of Message).ThrowCLRError()
        End If

        Me.Model = SVM.Train(problem, params)

        Return Me
    End Function

    Public Overrides Function Solve(data As Object, env As Environment) As Object

    End Function
End Class
