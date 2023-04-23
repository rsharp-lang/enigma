Imports System.Runtime.InteropServices
Imports [Module] = Enigma.LLVM.Module

Friend Module PInvokeHelper

    Private _initialized As Boolean

    Public Function EmitInMemory(ByVal [module] As [Module], ByVal targetCpu As String) As Byte()
        If Not PInvokeHelper._initialized Then
            PInvokeHelper._initialized = True
            PInvoke.LLVMInitializeNVPTXTarget()
            PInvoke.LLVMInitializeNVPTXTargetMC()
            PInvoke.LLVMInitializeNVPTXTargetInfo()
            PInvoke.LLVMInitializeNVPTXAsmPrinter()
        End If

        Dim triple = Marshal.PtrToStringAnsi(PInvoke.LLVMGetTarget([module]))
        Dim errorMessage As IntPtr
        Dim target As IntPtr

        If PInvoke.LLVMGetTargetFromTriple(triple, target, errorMessage) Then
            Throw New CudaException(Marshal.PtrToStringAnsi(errorMessage))
        End If

        Dim targetMachine = PInvoke.LLVMCreateTargetMachine(target, triple, targetCpu, "", PInvoke.LlvmCodeGenOptLevel.LlvmCodeGenLevelDefault, PInvoke.LlvmRelocMode.LlvmRelocDefault, PInvoke.LlvmCodeModel.LlvmCodeModelDefault)

        Dim memoryBuffer As IntPtr
        PInvoke.LLVMTargetMachineEmitToMemoryBuffer(targetMachine, [module], PInvoke.LlvmCodeGenFileType.LlvmAssemblyFile, errorMessage, memoryBuffer)

        If errorMessage <> IntPtr.Zero Then
            Dim errorMessageStr = Marshal.PtrToStringAnsi(errorMessage)

            If String.IsNullOrWhiteSpace(errorMessageStr) = False Then
                Throw New CudaException(errorMessageStr)
            End If
        End If

        Dim bufferStart = PInvoke.LLVMGetBufferStart(memoryBuffer)
        Dim bufferLength = PInvoke.LLVMGetBufferSize(memoryBuffer)
        Dim buffer = New Byte(bufferLength.ToInt32() - 1) {}

        Marshal.Copy(bufferStart, buffer, 0, buffer.Length)
        PInvoke.LLVMDisposeMemoryBuffer(memoryBuffer)

        Return buffer
    End Function
End Module