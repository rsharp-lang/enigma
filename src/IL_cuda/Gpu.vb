''' <summary>
''' Various GPU intrinsics
''' </summary>
Public Module Gpu

    ''' <summary>
    ''' Apply this attribute to a method if you want it to be compiled to a GPU intrinsic 
    ''' (use the LLVM intrinsic name, not the traditional C library one)
    ''' </summary>
    <AttributeUsage(AttributeTargets.Method)>
    Public Class BuiltinAttribute : Inherits Attribute

        ''' <summary>
        ''' Gets the intrinsic function name
        ''' </summary>
        Public ReadOnly Property Intrinsic As String

        ''' <summary>
        ''' Applies the intrinsic instead of calling the method
        ''' </summary>
        ''' <param name="intrinsic">The intrinsic to call</param>
        Public Sub New(ByVal intrinsic As String)
            _intrinsic = intrinsic
        End Sub

        Public Overrides Function ToString() As String
            Return Intrinsic
        End Function
    End Class

    Private ReadOnly Property Exception As Exception
        Get
            Return New Exception("Cannot use methods from the Gpu class on the CPU")
        End Get
    End Property

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The thread index X (threadIdx.x in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.tid.x")>
    Public Function ThreadX() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The thread index Y (threadIdx.y in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.tid.y")>
    Public Function ThreadY() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The thread index Z (threadIdx.z in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.tid.z")>
    Public Function ThreadZ() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The block index X (blockIdx.x in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.ctaid.x")>
    Public Function BlockX() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The block index Y (blockIdx.y in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.ctaid.y")>
    Public Function BlockY() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The block index Z (blockIdx.z in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.ctaid.z")>
    Public Function BlockZ() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The thread size X (threadDim.x in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.ntid.x")>
    Public Function ThreadDimX() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The thread size Y (threadDim.y in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.ntid.y")>
    Public Function ThreadDimY() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The thread size Z (threadDim.z in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.ntid.z")>
    Public Function ThreadDimZ() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The block size X (blockDim.x in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.nctaid.x")>
    Public Function BlockDimX() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The block size Y (blockDim.y in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.nctaid.y")>
    Public Function BlockDimY() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The block size Z (blockDim.z in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.nctaid.z")>
    Public Function BlockDimZ() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU
    ''' </summary>
    ''' <returns>The warp size (does not exist in cuda C)</returns>
    <Gpu.BuiltinAttribute("llvm.nvvm.read.ptx.sreg.warpsize")>
    Public Function WarpSize() As Integer
        Throw Gpu.Exception
    End Function

    ''' <summary>
    ''' Do not call this method from the CPU.
    ''' This method is the equivalent of cuda C's __syncthreads() function
    ''' </summary>
    <Gpu.BuiltinAttribute("llvm.nvvm.barrier0")>
    Public Sub Barrier()
        Throw Gpu.Exception
    End Sub
End Module
