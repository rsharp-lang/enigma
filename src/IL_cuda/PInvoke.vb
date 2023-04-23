Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports LLVM
Imports CC = System.Runtime.InteropServices.CallingConvention
Imports [Module] = LLVM.Module
Imports System.Runtime.CompilerServices

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
        If PInvoke.LLVMGetTargetFromTriple(triple, target, errorMessage) Then Throw New CudaSharpException(Marshal.PtrToStringAnsi(errorMessage))
        Dim targetMachine = PInvoke.LLVMCreateTargetMachine(target, triple, targetCpu, "", PInvoke.LlvmCodeGenOptLevel.LlvmCodeGenLevelDefault, PInvoke.LlvmRelocMode.LlvmRelocDefault, PInvoke.LlvmCodeModel.LlvmCodeModelDefault)

        Dim memoryBuffer As IntPtr
        PInvoke.LLVMTargetMachineEmitToMemoryBuffer(targetMachine, [module], PInvoke.LlvmCodeGenFileType.LlvmAssemblyFile, errorMessage, memoryBuffer)

        If errorMessage <> IntPtr.Zero Then
            Dim errorMessageStr = Marshal.PtrToStringAnsi(errorMessage)
            If String.IsNullOrWhiteSpace(errorMessageStr) = False Then Throw New CudaSharpException(errorMessageStr)
        End If
        Dim bufferStart = PInvoke.LLVMGetBufferStart(memoryBuffer)
        Dim bufferLength = PInvoke.LLVMGetBufferSize(memoryBuffer)
        Dim buffer = New Byte(bufferLength.ToInt32() - 1) {}
        Marshal.Copy(bufferStart, buffer, 0, buffer.Length)
        PInvoke.LLVMDisposeMemoryBuffer(memoryBuffer)
        Return buffer
    End Function
End Module

Friend Module PInvoke
    Const LlvmDll As String = "LLVM-3.3"

    <Extension()>
    Public Sub SetTarget(ByVal [module] As [Module], ByVal triple As String)
        PInvoke.LLVMSetTarget([module], triple)
    End Sub

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Sub LLVMSetTarget(ByVal [module] As IntPtr, ByVal triple As String)
    End Sub

    <Extension()>
    Public Sub SetDataLayout(ByVal [module] As [Module], ByVal triple As String)
        PInvoke.LLVMSetDataLayout([module], triple)
    End Sub

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Sub LLVMSetDataLayout(ByVal [module] As IntPtr, ByVal triple As String)
    End Sub

    <Extension()>
    Public Sub AddNamedMetadataOperand(ByVal [module] As [Module], ByVal name As String, ByVal value As IntPtr)
        PInvoke.LLVMAddNamedMetadataOperand([module], name, value)
    End Sub

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Sub LLVMAddNamedMetadataOperand(ByVal [module] As IntPtr, ByVal name As String, ByVal value As IntPtr)
    End Sub

    ' ReSharper disable once InconsistentNaming
    <Extension()>
    Public Function MetadataNodeInContext(ByVal context As Context, ByVal values As IntPtr()) As IntPtr
        Return PInvoke.LLVMMDNodeInContext(context, values, values.Length)
    End Function

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMMDNodeInContext(ByVal context As IntPtr, ByVal values As IntPtr(), ByVal count As UInteger) As IntPtr
    End Function

    ' ReSharper disable once InconsistentNaming
    Public Function LLVMMDStringInContext(ByVal context As IntPtr, ByVal str As String) As IntPtr
        Return PInvoke.LLVMMDStringInContext(context, str, str.Length)
    End Function

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMMDStringInContext(ByVal context As IntPtr, ByVal str As String, ByVal strLen As UInteger) As IntPtr
    End Function

    Public Enum LlvmCodeGenOptLevel
        LlvmCodeGenLevelNone
        LlvmCodeGenLevelLess
        LlvmCodeGenLevelDefault
        LlvmCodeGenLevelAggressive
    End Enum

    Public Enum LlvmRelocMode
        LlvmRelocDefault
        LlvmRelocStatic
        LlvmRelocPic
        LlvmRelocDynamicNoPic
    End Enum

    Public Enum LlvmCodeModel
        LlvmCodeModelDefault
        LlvmCodeModelJitDefault
        LlvmCodeModelSmall
        LlvmCodeModelKernel
        LlvmCodeModelMedium
        LlvmCodeModelLarge
    End Enum

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMCreateTargetMachine(ByVal target As IntPtr, ByVal triple As String, ByVal cpu As String, ByVal features As String, ByVal level As PInvoke.LlvmCodeGenOptLevel, ByVal reloc As PInvoke.LlvmRelocMode, ByVal codeModel As PInvoke.LlvmCodeModel) As IntPtr
    End Function

    Public Enum LlvmCodeGenFileType
        LlvmAssemblyFile
        LlvmObjectFile
    End Enum

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Sub LLVMInitializeNVPTXAsmPrinter()
    End Sub

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Sub LLVMInitializeNVPTXTarget()
    End Sub

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Sub LLVMInitializeNVPTXTargetMC()
    End Sub

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Sub LLVMInitializeNVPTXTargetInfo()
    End Sub

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMGetTarget(ByVal [module] As IntPtr) As IntPtr
    End Function

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMGetTargetFromTriple(ByVal triple As String, <Out> ByRef target As IntPtr, <Out> ByRef errorMessage As IntPtr) As Boolean
    End Function

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMTargetMachineEmitToMemoryBuffer(ByVal targetMachine As IntPtr, ByVal [module] As IntPtr, ByVal codegen As PInvoke.LlvmCodeGenFileType, <Out> ByRef errorMessage As IntPtr, <Out> ByRef memoryBuffer As IntPtr) As Boolean
    End Function

    '[DllImport(LlvmDll, CallingConvention = CC.Cdecl)]
    'public static extern bool LLVMTargetMachineEmitToFile(IntPtr targetMachine, IntPtr module, string filename, LlvmCodeGenFileType codegen, out IntPtr errorMessage);

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMGetBufferStart(ByVal memoryBuffer As IntPtr) As IntPtr
    End Function

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMGetBufferSize(ByVal memoryBuffer As IntPtr) As IntPtr
    End Function

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Sub LLVMDisposeMemoryBuffer(ByVal memoryBuffer As IntPtr)
    End Sub

    ' internal StructType(IntPtr typeref) : ... { }
    Private ReadOnly StructTypeConstructor As ConstructorInfo = GetType(StructType).GetConstructor(BindingFlags.NonPublic Or BindingFlags.Instance, Nothing, {GetType(IntPtr)}, Nothing)
    <Extension()>
    Public Function GetTypeByName(ByVal [module] As [Module], ByVal name As String) As StructType
        Dim intPtr = PInvoke.LLVMGetTypeByName([module], name)
        If intPtr = System.IntPtr.Zero Then Return Nothing
        Return CType(PInvoke.StructTypeConstructor.Invoke(New Object() {intPtr}), StructType)
    End Function

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMGetTypeByName(ByVal [module] As IntPtr, ByVal name As String) As IntPtr
    End Function

    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMPrintModuleToString(ByVal [module] As IntPtr) As IntPtr
    End Function

    ' internal Value(IntPtr typeref) : ... { }
    Private ReadOnly ValueConstructor As ConstructorInfo = GetType(Value).GetConstructor(BindingFlags.NonPublic Or BindingFlags.Instance, Nothing, {GetType(IntPtr)}, Nothing)
    <Extension()>
    Public Function FloatToUnsignedInt(ByVal builder As InstructionBuilder, ByVal value As Value, ByVal type As LLVM.Type, ByVal Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildFPToUI(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildFPToUI(ByVal builder As IntPtr, ByVal val As IntPtr, ByVal destTy As IntPtr, ByVal name As String) As IntPtr
    End Function

    <Extension()>
    Public Function FloatToSignedInt(ByVal builder As InstructionBuilder, ByVal value As Value, ByVal type As LLVM.Type, ByVal Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildFPToSI(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildFPToSI(ByVal builder As IntPtr, ByVal val As IntPtr, ByVal destTy As IntPtr, ByVal name As String) As IntPtr
    End Function

    <Extension()>
    Public Function UnsignedIntToFloat(ByVal builder As InstructionBuilder, ByVal value As Value, ByVal type As LLVM.Type, ByVal Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildUIToFP(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildUIToFP(ByVal builder As IntPtr, ByVal val As IntPtr, ByVal destTy As IntPtr, ByVal name As String) As IntPtr
    End Function

    <Extension()>
    Public Function SignedIntToFloat(ByVal builder As InstructionBuilder, ByVal value As Value, ByVal type As LLVM.Type, ByVal Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildSIToFP(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildSIToFP(ByVal builder As IntPtr, ByVal val As IntPtr, ByVal destTy As IntPtr, ByVal name As String) As IntPtr
    End Function

    <Extension()>
    Public Function FloatTrunc(ByVal builder As InstructionBuilder, ByVal value As Value, ByVal type As LLVM.Type, ByVal Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildFPTrunc(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildFPTrunc(ByVal builder As IntPtr, ByVal val As IntPtr, ByVal destTy As IntPtr, ByVal name As String) As IntPtr
    End Function

    <Extension()>
    Public Function FloatExtend(ByVal builder As InstructionBuilder, ByVal value As Value, ByVal type As LLVM.Type, ByVal Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildFPExt(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(PInvoke.LlvmDll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildFPExt(ByVal builder As IntPtr, ByVal val As IntPtr, ByVal destTy As IntPtr, ByVal name As String) As IntPtr
    End Function
End Module
