﻿Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Public Class ANNRegression : Inherits ANN

    Public Overrides Function DoCallTraining(args As list, env As Environment) As MLModel
        Dim model As New Netz(input.Length, 100, hidden.size.Length, output.labels.Length, activate:=AddressOf output.activate.Function)

        If TypeOf data Is dataframe Then
            Call trainOnDataframe(model, data)
        Else
            Throw New NotImplementedException
        End If

        Me.Model = model
        Return Me
    End Function

    Private Sub trainOnDataframe(model As Netz, data As dataframe)
        Dim inputs = DirectCast(data, dataframe).forEachRow(input).ToArray
        Dim outputs = DirectCast(data, dataframe).forEachRow(output.labels).ToArray

        For i As Integer = 0 To inputs.Length - 1
            Dim input As Double() = REnv.asVector(Of Double)(inputs(i).value)
            Dim output As Double() = REnv.asVector(Of Double)(outputs(i).value)

            Call model.train(input, output)
        Next
    End Sub

    Public Overrides Function Solve(data As Object, env As Environment) As Object
        If TypeOf data Is dataframe Then
            Dim df As dataframe = DirectCast(data, dataframe)
            Dim inputs = df.forEachRow(input).ToArray
            Dim rowNames As String() = df.getRowNames
            Dim outputs As New Dictionary(Of String, Double())
            Dim ANN As Netz = Model

            For Each label As String In output.labels
                outputs.Add(label, New Double(rowNames.Length - 1) {})
            Next

            For i As Integer = 0 To inputs.Length - 1
                Dim v As Double() = REnv.asVector(Of Double)(inputs(i).value)
                Dim o As Double() = ANN.predict(v)
                Dim j As i32 = Scan0

                For Each label As String In output.labels
                    outputs(label)(i) = o(++j)
                Next
            Next

            Dim result As New dataframe(df)

            For Each name As String In outputs.Keys
                Call result.columns.Add("PREDICT_OUTPUT: " & name, outputs(name))
            Next

            Return result
        Else
            Throw New NotImplementedException
        End If
    End Function
End Class