Imports Microsoft.VisualBasic.DataMining.ComponentModel.Encoder
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MachineLearning
Imports Microsoft.VisualBasic.MachineLearning.SVM
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Public Class SVMModel : Inherits MLModel

    Public Property svr As Boolean = False

    Public Overrides Function DoCallTraining(args As list, env As Environment) As MLModel
        Dim problem As [Variant](Of Message, SVM.Problem)
        Dim params As New SVM.Parameter With {
            .svmType = If(svr, SvmType.EPSILON_SVR, SvmType.C_SVC)
        }
        Dim weights As Dictionary(Of String, Double) = args.getValue("weights", env, New Dictionary(Of String, Double))

        If svr Then
            problem = svmDataSet.svrProblem(Features, Labels(Scan0), data, env)
        Else
            Problem = svmDataSet.svmProblem(Features, REnv.asVector(Of String)(data(Me.Labels(Scan0))), data, env)
        End If

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

                Call params.weights.Add(label.factor, 1)
            Next
        End If

        Me.Model = LibSVM.getSvmModel(problem, params)

        Return Me
    End Function

    Public Overrides Function Solve(data As Object, env As Environment) As Object
        Dim svm As SVM.SVMModel = Model
        Dim result = svm.svmClassify1(data, env)

        If svm.SVR AndAlso TypeOf result Is list Then
            Dim fits As list = DirectCast(result, list)
            Dim ypredicts = New Double(fits.length - 1) {}
            Dim i As Integer = 0

            For Each name As String In fits.getNames
                ypredicts(i) = fits.slots(name)
                i += 1
            Next

            result = New dataframe With {
                .rownames = fits.getNames,
                .columns = New Dictionary(Of String, Array) From {
                    {$"{Labels(Scan0)}(predicts)", ypredicts}
                }
            }
        End If

        Return result
    End Function
End Class
