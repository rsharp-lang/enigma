using Enigma;
using ManagedCuda;

CudaContext _context = new CudaContext();

T[] RunKernel<T>(Action<T[]> method, T[] parameters) where T : struct
{
    var methodInfo = method.Method;
    string[] kernels;
    string llvmIr, ptxIr;
    var ptx = Enigma.IL_Cuda.Translate(out kernels, out llvmIr, out ptxIr, "sm_20", methodInfo);
    Console.WriteLine(llvmIr);
    Console.WriteLine(ptxIr);
    var kernel = _context.LoadKernelPTX(ptx, kernels[0]);
    var maxThreads = kernel.MaxThreadsPerBlock;
    if (parameters.Length <= maxThreads)
    {
        kernel.BlockDimensions = parameters.Length;
        kernel.GridDimensions = 1;
    }
    else
    {
        kernel.BlockDimensions = maxThreads;
        kernel.GridDimensions = parameters.Length / maxThreads;
        if ((kernel.BlockDimensions * kernel.GridDimensions) != parameters.Length)
            throw new Exception(string.Format("Invalid parameters size (must be <= {0} or a multiple of {0}", maxThreads));
    }
    var gpuMem = new CudaDeviceVariable<T>(parameters.Length);
    gpuMem.CopyToDevice(parameters);
    kernel.Run(gpuMem.DevicePointer);
    gpuMem.CopyToHost(parameters);
    gpuMem.Dispose();
    return parameters;
}

int callFunc(int i)
{
    return i + 2;
}

Console.WriteLine(RunKernel(p => p[0] += 2, new int[] { 2 }));
Console.WriteLine(RunKernel(p => p[0] = callFunc(p[0]), new int[] { 2 }));
Console.WriteLine(RunKernel(p =>
{
    for (var i = 0; i < 2; i++)
    {
        p[0]++;
    }
}, new int[] { 2 }));

Console.WriteLine(RunKernel(p =>
{
    var tid = Gpu.BlockX() * Gpu.ThreadDimX() + Gpu.ThreadX();
    p[tid] = tid;
}, new int[256]));