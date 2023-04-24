Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Enigma.LLVM
Imports CC = System.Runtime.InteropServices.CallingConvention
Imports [Module] = Enigma.LLVM.Module

Friend Module PInvoke

    <Extension()>
    Public Sub SetTarget([module] As [Module], triple As String)
        PInvoke.LLVMSetTarget([module], triple)
    End Sub

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Sub LLVMSetTarget([module] As IntPtr, triple As String)
    End Sub

    <Extension()>
    Public Sub SetDataLayout([module] As [Module], triple As String)
        PInvoke.LLVMSetDataLayout([module], triple)
    End Sub

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Sub LLVMSetDataLayout([module] As IntPtr, triple As String)
    End Sub

    <Extension()>
    Public Sub AddNamedMetadataOperand([module] As [Module], name As String, value As IntPtr)
        PInvoke.LLVMAddNamedMetadataOperand([module], name, value)
    End Sub

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Sub LLVMAddNamedMetadataOperand([module] As IntPtr, name As String, value As IntPtr)
    End Sub

    ' ReSharper disable once InconsistentNaming
    <Extension()>
    Public Function MetadataNodeInContext(context As Context, values As IntPtr()) As IntPtr
        Return PInvoke.LLVMMDNodeInContext(context, values, values.Length)
    End Function

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMMDNodeInContext(context As IntPtr, values As IntPtr(), count As UInteger) As IntPtr
    End Function

    ' ReSharper disable once InconsistentNaming
    Public Function LLVMMDStringInContext(context As IntPtr, str As String) As IntPtr
        Return PInvoke.LLVMMDStringInContext(context, str, str.Length)
    End Function

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMMDStringInContext(context As IntPtr, str As String, strLen As UInteger) As IntPtr
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

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMCreateTargetMachine(target As IntPtr,
                                            triple As String,
                                            cpu As String,
                                            features As String,
                                            level As PInvoke.LlvmCodeGenOptLevel,
                                            reloc As PInvoke.LlvmRelocMode,
                                            codeModel As PInvoke.LlvmCodeModel) As IntPtr
    End Function

    Public Enum LlvmCodeGenFileType
        LlvmAssemblyFile
        LlvmObjectFile
    End Enum

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Sub LLVMInitializeNVPTXAsmPrinter()
    End Sub

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Sub LLVMInitializeNVPTXTarget()
    End Sub

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Sub LLVMInitializeNVPTXTargetMC()
    End Sub

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Sub LLVMInitializeNVPTXTargetInfo()
    End Sub

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMGetTarget([module] As IntPtr) As IntPtr
    End Function

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMGetTargetFromTriple(triple As String, <Out> ByRef target As IntPtr, <Out> ByRef errorMessage As IntPtr) As Boolean
    End Function

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMTargetMachineEmitToMemoryBuffer(targetMachine As IntPtr, [module] As IntPtr, codegen As PInvoke.LlvmCodeGenFileType, <Out> ByRef errorMessage As IntPtr, <Out> ByRef memoryBuffer As IntPtr) As Boolean
    End Function

    '[DllImport(LlvmDll, CallingConvention = CC.Cdecl)]
    'public static extern bool LLVMTargetMachineEmitToFile(IntPtr targetMachine, IntPtr module, string filename, LlvmCodeGenFileType codegen, out IntPtr errorMessage);

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMGetBufferStart(memoryBuffer As IntPtr) As IntPtr
    End Function

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMGetBufferSize(memoryBuffer As IntPtr) As IntPtr
    End Function

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Sub LLVMDisposeMemoryBuffer(memoryBuffer As IntPtr)
    End Sub

    ' internal StructType(IntPtr typeref) : ... { }
    Private ReadOnly StructTypeConstructor As ConstructorInfo = GetType(StructType) _
        .GetConstructor(BindingFlags.NonPublic Or BindingFlags.Instance, Nothing, {GetType(IntPtr)}, Nothing)

    <Extension()>
    Public Function GetTypeByName([module] As [Module], name As String) As StructType
        Dim intPtr = PInvoke.LLVMGetTypeByName([module], name)
        If intPtr = System.IntPtr.Zero Then Return Nothing
        Return CType(PInvoke.StructTypeConstructor.Invoke(New Object() {intPtr}), StructType)
    End Function

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMGetTypeByName([module] As IntPtr, name As String) As IntPtr
    End Function

    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Public Function LLVMPrintModuleToString([module] As IntPtr) As IntPtr
    End Function

    ' internal Value(IntPtr typeref) : ... { }
    Private ReadOnly ValueConstructor As ConstructorInfo = GetType(Value).GetConstructor(BindingFlags.NonPublic Or BindingFlags.Instance, Nothing, {GetType(IntPtr)}, Nothing)
    <Extension()>
    Public Function FloatToUnsignedInt(builder As InstructionBuilder, value As Value, type As LLVM.Type, Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildFPToUI(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildFPToUI(builder As IntPtr, val As IntPtr, destTy As IntPtr, name As String) As IntPtr
    End Function

    <Extension()>
    Public Function FloatToSignedInt(builder As InstructionBuilder, value As Value, type As LLVM.Type, Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildFPToSI(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildFPToSI(builder As IntPtr, val As IntPtr, destTy As IntPtr, name As String) As IntPtr
    End Function

    <Extension()>
    Public Function UnsignedIntToFloat(builder As InstructionBuilder, value As Value, type As LLVM.Type, Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildUIToFP(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildUIToFP(builder As IntPtr, val As IntPtr, destTy As IntPtr, name As String) As IntPtr
    End Function

    <Extension()>
    Public Function SignedIntToFloat(builder As InstructionBuilder, value As Value, type As LLVM.Type, Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildSIToFP(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildSIToFP(builder As IntPtr, val As IntPtr, destTy As IntPtr, name As String) As IntPtr
    End Function

    <Extension()>
    Public Function FloatTrunc(builder As InstructionBuilder, value As Value, type As LLVM.Type, Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildFPTrunc(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildFPTrunc(builder As IntPtr, val As IntPtr, destTy As IntPtr, name As String) As IntPtr
    End Function

    <Extension()>
    Public Function FloatExtend(builder As InstructionBuilder, value As Value, type As LLVM.Type, Optional name As String = "") As Value
        Return CType(PInvoke.ValueConstructor.Invoke(New Object() {PInvoke.LLVMBuildFPExt(builder, value, type, name)}), Value)
    End Function
    <DllImportAttribute(LLVM.llvmdll, CallingConvention:=CC.Cdecl)>
    Private Function LLVMBuildFPExt(builder As IntPtr, val As IntPtr, destTy As IntPtr, name As String) As IntPtr
    End Function
End Module
