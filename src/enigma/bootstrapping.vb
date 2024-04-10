Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MachineLearning.Bootstrapping
Imports Microsoft.VisualBasic.MachineLearning.Bootstrapping.GraphEmbedding
Imports Microsoft.VisualBasic.MachineLearning.Bootstrapping.node2vec
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
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

    <ExportAPI("graph2vec")>
    Public Function graph2vec(<RRawVectorArgument> graph As Object) As Object

    End Function

    <ExportAPI("node2vec")>
    Public Function node2vec(graph As Object, Optional dims As Integer = 10) As Object
        Dim g As node2vec.Graph

        If TypeOf graph Is String Then
            g = loadGraphFrom(file:=graph)
        ElseIf TypeOf graph Is node2vec.Graph Then
            g = graph
        ElseIf TypeOf graph Is dataframe Then
            Dim df = DirectCast(graph, dataframe)
            Dim u As String() = CLRVector.asCharacter(df.getBySynonym("source", "u"))
            Dim v As String() = CLRVector.asCharacter(df.getBySynonym("target", "v"))
            Dim w As Double() = CLRVector.asNumeric(df.getBySynonym("weight", "w"))

            g = Solver.CreateGraph(u, v, w)
        Else
            Throw New NotImplementedException
        End If

        Return Solver.CreateEmbedding(g, dimensions:=dims)
    End Function

    ''' <summary>
    ''' load graph data from file
    ''' input format: node1_id_int node2_id_int &lt;weight_float> </summary>
    ''' <param name="file"> path of the input file </param>
    Private Function loadGraphFrom(file As String) As Graph
        Dim u As New List(Of String)
        Dim v As New List(Of String)
        Dim w As New List(Of Double)

        ' read graph info from file
        For Each lineTxt As String In file.IterateAllLines
            ' parse the line text to get the edge info
            Dim strList = lineTxt.Split(" "c)
            Dim node1ID = strList(0)
            Dim node2ID = strList(1)

            Call u.Add(node1ID)
            Call v.Add(node2ID)

            ' add the edge to the graph
            If strList.Length > 2 Then
                Call w.Add(Double.Parse(strList(2)))
            Else
                Call w.Add(1.0)
            End If
        Next

        Return Solver.CreateGraph(u.ToArray, v.ToArray, w.ToArray)
    End Function
End Module
