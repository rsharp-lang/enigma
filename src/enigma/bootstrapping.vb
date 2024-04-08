Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MachineLearning.Bootstrapping.GraphEmbedding
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("bootstrapping")>
Module bootstrapping

    <ExportAPI("arguments")>
    Public Function arguments() As Arguments

    End Function

    <ExportAPI("complex")>
    Public Function ComplEx(args As Arguments,
                            <RRawVectorArgument(TypeCodes.string)>
                            Optional algorithm As Object = "ComplEx|complex_NNE|complex_NNE_AER|complex_R",
                            Optional rules As String = Nothing,
                            Optional env As Environment = Nothing) As Object

    End Function
End Module
