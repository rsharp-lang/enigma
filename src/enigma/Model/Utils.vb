Imports System.Runtime.CompilerServices
Imports SMRUCC.Rsharp.Runtime.Components

Public Module Utils

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function CreateError(err As Message) As MLPipelineError
        Return New MLPipelineError(err)
    End Function
End Module
