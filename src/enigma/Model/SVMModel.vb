Imports Microsoft.VisualBasic.DataMining.ComponentModel.Encoder
Imports Microsoft.VisualBasic.MachineLearning
Imports Microsoft.VisualBasic.MachineLearning.SVM
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Public Class SVMModel : Inherits MLModel

    Public Overrides Function DoCallTraining(args As list, env As Environment) As MLModel
        Dim labels As String() = REnv.asVector(Of String)(data(Me.Labels(Scan0)))
        Dim problem = svmDataSet.svmProblem(Features, labels, data, env)
        Dim params As New SVM.Parameter With {.svmType = SvmType.NU_SVC}
        Dim weights As Dictionary(Of String, Double) = args.getValue("weights", env, New Dictionary(Of String, Double))

        If problem Like GetType(Message) Then
            Return New MLPipelineError(problem.TryCast(Of Message))
        End If

        If Not weights Is Nothing Then
            For Each label In weights.AsEnumerable
                Call params.weights.Add(CInt(label.Key), label.Value)
            Next
        Else
            For Each label As ColorClass In problem.TryCast(Of Problem).Y _
                .GroupBy(Function(a) a.name) _
                .Select(Function(a) a.First)

                Call params.weights.Add(label.enumInt, 1)
            Next
        End If

        Me.Model = LibSVM.getSvmModel(problem, params)

        Return Me
    End Function

    Public Overrides Function Solve(data As Object, env As Environment) As Object
        Dim svm As SVM.SVMModel = Model
        Dim result = svm.svmClassify1(data, env)

        Return result
    End Function
End Class
