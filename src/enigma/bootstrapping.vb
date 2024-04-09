Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MachineLearning.Bootstrapping
Imports Microsoft.VisualBasic.MachineLearning.Bootstrapping.GraphEmbedding
Imports Microsoft.VisualBasic.MachineLearning.Bootstrapping.node2vec
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization

<Package("bootstrapping")>
Module bootstrapping

    <ExportAPI("arguments")>
    Public Function arguments(fnTrainTriples As String, fnValidTriples As String, fnTestTriples As String, fnAllTriples As String) As Arguments
        Return New Arguments With {
            .fnAllTriples = fnAllTriples,
            .fnTestTriples = fnTestTriples,
            .fnTrainTriples = fnTrainTriples,
            .fnValidTriples = fnValidTriples
        }
    End Function

    <ExportAPI("complex")>
    Public Function ComplEx(args As Arguments,
                            <RRawVectorArgument(TypeCodes.string)>
                            Optional algorithm As Object = "ComplEx|complex_NNE|complex_NNE_AER|complex_R",
                            Optional rules As String = Nothing,
                            Optional iterations As Integer = 1000,
                            Optional env As Environment = Nothing) As Object

        Dim alg As String = CLRVector.asCharacter(algorithm).DefaultFirst("ComplEx")

        args.other = New Dictionary(Of String, String) From {{"rules", rules}}
        args.iterations = iterations

        Select Case alg.ToLower
            Case "complex" : Return GraphEmbedding.Algorithm.learn(Of GraphEmbedding.complex.ComplEx)(args)
            Case "complex_nne" : Return GraphEmbedding.Algorithm.learn(Of GraphEmbedding.complex_NNE.ComplEx)(args)
            Case "complex_nne_aer" : Return GraphEmbedding.Algorithm.learn(Of GraphEmbedding.complex_NNE_AER.ComplEx)(args)
            Case "complex_r" : Return GraphEmbedding.Algorithm.learn(Of GraphEmbedding.complex_R.ComplEx)(args)
            Case Else
                Return Internal.debug.stop($"invalid algorithm name({alg})!", env)
        End Select
    End Function

    <ExportAPI("node2vec")>
    Public Function node2vec(graph As Object, Optional dims As Integer = 10) As Object
        Dim g As node2vec.Graph

        If TypeOf graph Is String Then
            g = New node2vec.Graph(graph, True)
        ElseIf TypeOf graph Is node2vec.Graph Then
            g = graph
        Else
            Throw New NotImplementedException
        End If

        Return Solver.CreateEmbedding(g, dimensions:=dims)
    End Function
End Module
