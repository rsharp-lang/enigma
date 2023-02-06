Imports Microsoft.VisualBasic.MachineLearning.RandomForests
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Vectorization

Public Class RandomForest : Inherits MLModel

    Public Property regression As Boolean

    Public Overrides Function DoCallTraining(args As list, env As Environment) As MLModel
        Dim model As New RanFog With {.LF_c = If(regression, LF_c.Mean_Squared_Error, LF_c.Information_Gain)}
        Dim input As New Data With {
            .ID = data.getRowNames,
            .phenotype = CLRVector.asNumeric(data(Labels(Scan0))),
            .attributeNames = Features,
            .Genotype = data _
                .forEachRow(Features) _
                .Select(Function(r) CLRVector.asNumeric(r.value)) _
                .ToArray
        }

        Call model.Run(input)
        Me.Model = model

        Return Me
    End Function

    Public Overrides Function Solve(data As Object, env As Environment) As Object
        Throw New NotImplementedException()
    End Function
End Class
