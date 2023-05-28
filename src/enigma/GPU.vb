
Imports ILGPU
Imports ILGPU.Runtime.Cuda
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("gpu")>
Public Module GPU

    ''' <summary>
    ''' try to execute the expression on gpu
    ''' </summary>
    ''' <param name="exp"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("exec")>
    Public Function gpu_exec(<RLazyExpression> exp As Expression, Optional env As Environment = Nothing) As Object
        Dim context As Context = Context.CreateDefault
        Dim accelerator = context.CreateCudaAccelerator(0)

        Call accelerator.Synchronize()
        Call accelerator.Dispose()
        Call context.Dispose()
    End Function
End Module
