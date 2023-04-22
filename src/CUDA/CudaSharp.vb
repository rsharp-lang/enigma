Imports System
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Runtime.InteropServices
Imports System.Text
Imports LLVM

Namespace CudaSharp

    ''' <summary>
    ''' Main CudaSharp class
    ''' </summary>
    Public Module CudaSharp
        Sub New()
            Dim extractTo = Path.GetFullPath("LLVM-3.3")
            If IO.File.Exists(extractTo) Then Return
            Dim file = IO.File.Open(extractTo, FileMode.OpenOrCreate)
            Dim llvm34 = Assembly.GetExecutingAssembly().GetManifestResourceStream("LLVM-3.4.dll")
            If llvm34 Is Nothing Then Throw New CudaSharpException("Could not extract LLVM-3.4.dll")
            llvm34.CopyTo(file)
            file.Close()
        End Sub

        ''' <summary>
        ''' List of CIL instructions that are unsupported by the library
        ''' </summary>
        Public ReadOnly Property UnsupportedInstructions As OpCode()
            Get
                Return Translator.UnsupportedInstructions
            End Get
        End Property

        ''' <summary>
        ''' Translates all supplied methods into a single PTX object using the sm_20 target.
        ''' Note that method names may not be accurately translated (kernel name is not MethodInfo.Name), for example anonymous method names.
        ''' Use an overload of this function to get the real name.
        ''' </summary>
        ''' <param name="methods">Methods to translate</param>
        ''' <returns>PTX object to be loaded into the GPU or written to a .ptx file on disk</returns>
        Public Function Translate(ParamArray methods As MethodInfo()) As Byte()
            Dim dummy As String()
            Return Translate(dummy, methods)
        End Function

        ''' <summary>
        ''' Translates all supplied methods into a single PTX object using the sm_20 target, giving back the kernel names
        ''' </summary>
        ''' <param name="kernelNames">Resulting kernel names in the ptx object</param>
        ''' <param name="methods">Methods to translate</param>
        ''' <returns>PTX object to be loaded into the GPU or written to a .ptx file on disk</returns>
        Public Function Translate(<Out> ByRef kernelNames As String(), ParamArray methods As MethodInfo()) As Byte()
            Return Translate(kernelNames, "sm_20", methods)
        End Function

        ''' <summary>
        ''' Translates all supplied methods into a single PTX object using the supplied target, giving back the kernel names
        ''' </summary>
        ''' <param name="kernelNames">Resulting kernel names in the ptx object</param>
        ''' <param name="targetCpu">Target, usually in the form sm_##</param>
        ''' <param name="methods">Methods to translate</param>
        ''' <returns>PTX object to be loaded into the GPU or written to a .ptx file on disk</returns>
        Public Function Translate(<Out> ByRef kernelNames As String(), ByVal targetCpu As String, ParamArray methods As MethodInfo()) As Byte()
            Dim [module] = Translator.Translate(Context.Global, methods)
            kernelNames = methods.[Select](Function(m) m.Name.StripNameToValidPtx()).ToArray()
            Dim ptx = PInvokeHelper.EmitInMemory([module], targetCpu)
            Return ptx
        End Function

        ''' <summary>
        ''' Translates all supplied methods into a single PTX object using the supplied target, giving back the kernel names and intermediate representations (for debugging)
        ''' </summary>
        ''' <param name="kernelNames">Resulting kernel names in the ptx object</param>
        ''' <param name="llvmIr">LLVM IR of the module</param>
        ''' <param name="ptxIr">PTX assembly of the module</param>
        ''' <param name="targetCpu">Target, usually in the form sm_##</param>
        ''' <param name="methods">Methods to translate</param>
        ''' <returns>PTX object to be loaded into the GPU or written to a .ptx file on disk</returns>
        Public Function Translate(<Out> ByRef kernelNames As String(), <Out> ByRef llvmIr As String, <Out> ByRef ptxIr As String, ByVal targetCpu As String, ParamArray methods As MethodInfo()) As Byte()
            Dim [module] = Translator.Translate(Context.Global, methods)
            kernelNames = methods.[Select](Function(m) m.Name.StripNameToValidPtx()).ToArray()
            Dim ptx = PInvokeHelper.EmitInMemory([module], targetCpu)
            llvmIr = Marshal.PtrToStringAnsi(PInvoke.LLVMPrintModuleToString([module]))
            ptxIr = Encoding.UTF8.GetString(ptx)
            Return ptx
        End Function
    End Module
End Namespace
