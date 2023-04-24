Imports System.Runtime.InteropServices

Namespace LLVM

	' Token: 0x0200001B RID: 27
	Friend Module llvm

		' Token: 0x0400001D RID: 29
		Public Const llvmdll As String = "LLVM-3.3"

		' Token: 0x06000078 RID: 120
		Public Declare Function GetInt32 Lib "LLVM-3.3" Alias "LLVMInt32TypeInContext" (context As IntPtr) As IntPtr

		' Token: 0x06000079 RID: 121
		Public Declare Function GetInt Lib "LLVM-3.3" Alias "LLVMIntTypeInContext" (context As IntPtr, bits As Integer) As IntPtr

		' Token: 0x0600007A RID: 122
		Public Declare Function GetFloat Lib "LLVM-3.3" Alias "LLVMFloatTypeInContext" (context As IntPtr) As IntPtr

		' Token: 0x0600007B RID: 123
		Public Declare Function GetDouble Lib "LLVM-3.3" Alias "LLVMDoubleTypeInContext" (context As IntPtr) As IntPtr

		' Token: 0x0600007C RID: 124
		Public Declare Function GetVoid Lib "LLVM-3.3" Alias "LLVMVoidTypeInContext" (context As IntPtr) As IntPtr

		' Token: 0x0600007D RID: 125
		Public Declare Function GetTypeKind Lib "LLVM-3.3" Alias "LLVMGetTypeKind" (typeref As IntPtr) As TypeKind

		' Token: 0x0600007E RID: 126
		Public Declare Function GetPointerType Lib "LLVM-3.3" Alias "LLVMPointerType" (valueType As IntPtr, addressSpace As Integer) As IntPtr

		' Token: 0x0600007F RID: 127
		Public Declare Function FunctionType Lib "LLVM-3.3" Alias "LLVMFunctionType" (ret As IntPtr, args As IntPtr(), argcount As Integer, vararg As Boolean) As IntPtr

		' Token: 0x06000080 RID: 128
		Public Declare Function GetArgumentCount Lib "LLVM-3.3" Alias "LLVMCountParamTypes" (functionType As IntPtr) As Integer

		' Token: 0x06000081 RID: 129
		Public Declare Sub GetArgumentTypes Lib "LLVM-3.3" Alias "LLVMGetParamTypes" (functionType As IntPtr, types As IntPtr())

		' Token: 0x06000082 RID: 130
		Public Declare Function GetReturnType Lib "LLVM-3.3" Alias "LLVMGetReturnType" (functionType As IntPtr) As IntPtr

		' Token: 0x06000083 RID: 131
		Public Declare Function ArrayType Lib "LLVM-3.3" Alias "LLVMArrayType" (elemType As IntPtr, elemCount As Integer) As IntPtr

		' Token: 0x06000084 RID: 132
		Public Declare Function StructType Lib "LLVM-3.3" Alias "LLVMStructTypeInContext" (context As IntPtr, elements As IntPtr(), elemcount As Integer, packed As Boolean) As IntPtr

		' Token: 0x06000085 RID: 133
		Public Declare Function StructType Lib "LLVM-3.3" Alias "LLVMStructCreateNamed" (context As IntPtr, name As String) As IntPtr

		' Token: 0x06000086 RID: 134
		Public Declare Function StructCreateEmptyType Lib "LLVM-3.3" Alias "LLVMStructCreateEmptyTypeInContext" (context As IntPtr) As IntPtr

		' Token: 0x06000087 RID: 135
		Public Declare Sub StructSetBody Lib "LLVM-3.3" Alias "LLVMStructSetBody" (structType As IntPtr, elementTypes As IntPtr(), elementCount As Integer, packed As Boolean)

		' Token: 0x06000088 RID: 136
		Public Declare Function IsPackedStruct Lib "LLVM-3.3" Alias "LLVMIsPackedStruct" (structType As IntPtr) As Boolean

		' Token: 0x06000089 RID: 137
		Public Declare Function IsOpaqueStruct Lib "LLVM-3.3" Alias "LLVMIsOpaqueStruct" (structType As IntPtr) As Boolean

		' Token: 0x0600008A RID: 138
		Public Declare Function StructFieldCount Lib "LLVM-3.3" Alias "LLVMCountStructElementTypes" (structType As IntPtr) As UInteger

		' Token: 0x0600008B RID: 139
		Public Declare Sub StructElements Lib "LLVM-3.3" Alias "LLVMGetStructElementTypes" (structType As IntPtr, types As IntPtr())

		' Token: 0x0600008C RID: 140
		Public Declare Function [TypeOf] Lib "LLVM-3.3" Alias "LLVMTypeOf" (value As IntPtr) As IntPtr

		' Token: 0x0600008D RID: 141
		Public Declare Function GetElementType Lib "LLVM-3.3" Alias "LLVMGetElementType" (type As IntPtr) As IntPtr

		' Token: 0x0600008E RID: 142
		Public Declare Function GetWidth Lib "LLVM-3.3" Alias "LLVMGetIntTypeWidth" (inttype As IntPtr) As Integer

		' Token: 0x0600008F RID: 143
		Public Declare Function AlignOf Lib "LLVM-3.3" Alias "LLVMAlignOf" (typeref As IntPtr) As IntPtr

		' Token: 0x06000090 RID: 144
		Public Declare Function SizeOf Lib "LLVM-3.3" Alias "LLVMSizeOf" (typeref As IntPtr) As IntPtr

		' Token: 0x06000091 RID: 145
		Public Declare Function GetTypeContext Lib "LLVM-3.3" Alias "LLVMGetTypeContext" (typeref As IntPtr) As IntPtr

		' Token: 0x06000092 RID: 146
		Public Declare Function CreateModule Lib "LLVM-3.3" Alias "LLVMModuleCreateWithNameInContext" (name As String, context As IntPtr) As IntPtr

		' Token: 0x06000093 RID: 147
		Public Declare Function CreateFunction Lib "LLVM-3.3" Alias "LLVMAddFunction" ([module] As IntPtr, name As String, type As IntPtr) As IntPtr

		' Token: 0x06000094 RID: 148
		Public Declare Function AddGlobal Lib "LLVM-3.3" Alias "LLVMAddGlobal" ([module] As IntPtr, type As IntPtr, name As String) As IntPtr

		' Token: 0x06000095 RID: 149
		Public Declare Function AddGlobal Lib "LLVM-3.3" Alias "LLVMAddGlobalValue" ([module] As IntPtr, type As IntPtr, name As String, constant As IntPtr) As IntPtr

		' Token: 0x06000096 RID: 150
		Public Declare Function GetNamedGlobal Lib "LLVM-3.3" Alias "LLVMGetNamedGlobal" ([module] As IntPtr, name As String) As IntPtr

		' Token: 0x06000097 RID: 151
		Public Declare Function GetContext Lib "LLVM-3.3" Alias "LLVMGetModuleContext" ([module] As IntPtr) As IntPtr

		' Token: 0x06000098 RID: 152
		Public Declare Function InitializeNative Lib "LLVM-3.3" Alias "LLVMInitializeNativeTargetDynamicLibrary" () As Boolean

		' Token: 0x06000099 RID: 153
		Public Declare Sub InitializeAllTargets Lib "LLVM-3.3" Alias "LLVMInitializeAllTargetsDynamicLibrary" ()

		' Token: 0x0600009A RID: 154
		Public Declare Function GetGlobalContext Lib "LLVM-3.3" Alias "LLVMGetGlobalContext" () As IntPtr

		' Token: 0x0600009B RID: 155
		Public Declare Function CreateExecutionEngine Lib "LLVM-3.3" Alias "LLVMCreateExecutionEngineForModule" (<Out()> ByRef engine As IntPtr, [module] As IntPtr, <Out()> ByRef [error] As String) As Boolean

		' Token: 0x0600009C RID: 156
		Public Declare Function CreateBlock Lib "LLVM-3.3" Alias "LLVMAppendBasicBlockInContext" (context As IntPtr, func As IntPtr, name As String) As IntPtr

		' Token: 0x0600009D RID: 157
		Public Declare Function CreateBuilder Lib "LLVM-3.3" Alias "LLVMCreateBuilderInContext" (context As IntPtr) As IntPtr

		' Token: 0x0600009E RID: 158
		Public Declare Function GetBuilderContext Lib "LLVM-3.3" Alias "LLVMGetBuilderContext" (builder As IntPtr) As IntPtr

		' Token: 0x0600009F RID: 159
		Public Declare Sub PointToEnd Lib "LLVM-3.3" Alias "LLVMPositionBuilderAtEnd" (instructionBuilder As IntPtr, block As IntPtr)

		' Token: 0x060000A0 RID: 160
		Public Declare Function GetPointer Lib "LLVM-3.3" Alias "LLVMGetPointerToGlobal" (executionEngine As IntPtr, globalval As IntPtr) As IntPtr

		' Token: 0x060000A1 RID: 161
		Public Declare Function GetPointerToFunction Lib "LLVM-3.3" Alias "LLVMGetPointerToFunction" (executionEngine As IntPtr, globalval As IntPtr) As IntPtr

		' Token: 0x060000A2 RID: 162
		Public Declare Sub SetLazyFunctionCallback Lib "LLVM-3.3" Alias "LLVMSetLazyFunctionCallback" (executionEngine As IntPtr, loader As LazyFunctionLoader)

		' Token: 0x060000A3 RID: 163
		Public Declare Function GetParameter Lib "LLVM-3.3" Alias "LLVMGetParam" ([function] As IntPtr, index As Integer) As IntPtr

		' Token: 0x060000A4 RID: 164
		Public Declare Sub Dump Lib "LLVM-3.3" Alias "LLVMDumpValue" (valueref As IntPtr)

		' Token: 0x060000A5 RID: 165
		Public Declare Function Print Lib "LLVM-3.3" Alias "LLVMPrint" (valueref As IntPtr) As <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.BStr)> String

		' Token: 0x060000A6 RID: 166
		Public Declare Sub SetName Lib "LLVM-3.3" Alias "LLVMSetValueName" (valueref As IntPtr, name As String)

		' Token: 0x060000A7 RID: 167
		Public Declare Function GetName Lib "LLVM-3.3" Alias "LLVMGetValueNameAsBSTR" (valueref As IntPtr) As <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.BStr)> String

		' Token: 0x060000A8 RID: 168
		Public Declare Function GetUndefinedValue Lib "LLVM-3.3" Alias "LLVMGetUndef" (type As IntPtr) As IntPtr

		' Token: 0x060000A9 RID: 169
		Public Declare Function GetZero Lib "LLVM-3.3" Alias "LLVMConstNull" (type As IntPtr) As IntPtr

		' Token: 0x060000AA RID: 170
		Public Declare Function Constant Lib "LLVM-3.3" Alias "LLVMConstInt" (typeref As IntPtr, value As ULong, sign As Boolean) As IntPtr

		' Token: 0x060000AB RID: 171
		Public Declare Function ToPointer Lib "LLVM-3.3" Alias "LLVMConstIntToPtr" (value As IntPtr, targetType As IntPtr) As IntPtr

		' Token: 0x060000AC RID: 172
		Public Declare Sub SetCallingConvention Lib "LLVM-3.3" Alias "LLVMSetFunctionCallConv" (func As IntPtr, conv As CallingConvention)

		' Token: 0x060000AD RID: 173
		Public Declare Function GetCallingConvention Lib "LLVM-3.3" Alias "LLVMGetFunctionCallConv" (func As IntPtr) As CallingConvention

		' Token: 0x060000AE RID: 174
		Public Declare Sub SetInstructionCallingConvention Lib "LLVM-3.3" Alias "LLVMSetInstructionCallConv" (func As IntPtr, conv As CallingConvention)

		' Token: 0x060000AF RID: 175
		Public Declare Function GetInstructionCallingConvention Lib "LLVM-3.3" Alias "LLVMGetInstructionCallConv" (func As IntPtr) As CallingConvention

		' Token: 0x060000B0 RID: 176
		Public Declare Function IsTailCall Lib "LLVM-3.3" Alias "LLVMIsTailCall" ([call] As IntPtr) As Boolean

		' Token: 0x060000B1 RID: 177
		Public Declare Sub SetTailCall Lib "LLVM-3.3" Alias "LLVMSetTailCall" ([call] As IntPtr, value As Boolean)

		' Token: 0x060000B2 RID: 178
		Public Declare Function GetFunction Lib "LLVM-3.3" Alias "LLVMGetNamedFunction" ([module] As IntPtr, name As String) As IntPtr

		' Token: 0x060000B3 RID: 179
		Public Declare Sub SetGC Lib "LLVM-3.3" Alias "LLVMSetGC" ([function] As IntPtr, name As String)

		' Token: 0x060000B4 RID: 180
		Public Declare Function CreateGC Lib "LLVM-3.3" Alias "LLVMCreateExternalGC" (gcinfo As GarbageCollection.NativeGlue.ExternalGCInfo) As IntPtr

		' Token: 0x060000B5 RID: 181
		Public Declare Function RegisterGC Lib "LLVM-3.3" Alias "LLVMRegisterExternalGC" (name As IntPtr, descr As IntPtr, ctor As GarbageCollection.NativeGlue.GCStrategyConstructor) As IntPtr

		' Token: 0x060000B6 RID: 182
		Public Declare Function EmitCompare Lib "LLVM-3.3" Alias "LLVMBuildICmp" (ibuilder As IntPtr, comparison As IntegerComparison, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000B7 RID: 183
		Public Declare Function EmitIsNull Lib "LLVM-3.3" Alias "LLVMBuildIsNull" (ibuilder As IntPtr, value As IntPtr, name As String) As IntPtr

		' Token: 0x060000B8 RID: 184
		Public Declare Function EmitIf Lib "LLVM-3.3" Alias "LLVMBuildCondBr" (ibuilder As IntPtr, cond As IntPtr, ontrue As IntPtr, onfalse As IntPtr) As IntPtr

		' Token: 0x060000B9 RID: 185
		Public Declare Function EmitSwitch Lib "LLVM-3.3" Alias "LLVMBuildSwitch" (ibuilder As IntPtr, value As IntPtr, elseCase As IntPtr, caseCount As Integer) As IntPtr

		' Token: 0x060000BA RID: 186
		Public Declare Function SwitchAdd Lib "LLVM-3.3" Alias "LLVMAddCase" (switch As IntPtr, value As IntPtr, target As IntPtr) As IntPtr

		' Token: 0x060000BB RID: 187
		Public Declare Function EmitGoTo Lib "LLVM-3.3" Alias "LLVMBuildBr" (ibuilder As IntPtr, targetBlock As IntPtr) As IntPtr

		' Token: 0x060000BC RID: 188
		Public Declare Function EmitCall Lib "LLVM-3.3" Alias "LLVMBuildCall" (ibuilder As IntPtr, func As IntPtr, args As IntPtr(), argc As Integer, name As String) As IntPtr

		' Token: 0x060000BD RID: 189
		Public Declare Function EmitReturn Lib "LLVM-3.3" Alias "LLVMBuildRet" (instructionBuilder As IntPtr, value As IntPtr) As IntPtr

		' Token: 0x060000BE RID: 190
		Public Declare Function EmitReturn Lib "LLVM-3.3" Alias "LLVMBuildRetVoid" (instructionBuilder As IntPtr) As IntPtr

		' Token: 0x060000BF RID: 191
		Public Declare Function EmitSubtract Lib "LLVM-3.3" Alias "LLVMBuildSub" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000C0 RID: 192
		Public Declare Function EmitAdd Lib "LLVM-3.3" Alias "LLVMBuildAdd" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000C1 RID: 193
		Public Declare Function EmitAddFloat Lib "LLVM-3.3" Alias "LLVMBuildFAdd" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000C2 RID: 194
		Public Declare Function EmitMultiply Lib "LLVM-3.3" Alias "LLVMBuildMul" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000C3 RID: 195
		Public Declare Function EmitDivideUnsigned Lib "LLVM-3.3" Alias "LLVMBuildUDiv" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000C4 RID: 196
		Public Declare Function EmitDivideSigned Lib "LLVM-3.3" Alias "LLVMBuildSDiv" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000C5 RID: 197
		Public Declare Function EmitReminderUnsigned Lib "LLVM-3.3" Alias "LLVMBuildURem" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000C6 RID: 198
		Public Declare Function EmitReminderSigned Lib "LLVM-3.3" Alias "LLVMBuildSRem" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000C7 RID: 199
		Public Declare Function EmitNegate Lib "LLVM-3.3" Alias "LLVMBuildNeg" (ibuilder As IntPtr, value As IntPtr, name As String) As IntPtr

		' Token: 0x060000C8 RID: 200
		Public Declare Function EmitAnd Lib "LLVM-3.3" Alias "LLVMBuildAnd" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000C9 RID: 201
		Public Declare Function EmitOr Lib "LLVM-3.3" Alias "LLVMBuildOr" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000CA RID: 202
		Public Declare Function EmitXor Lib "LLVM-3.3" Alias "LLVMBuildXor" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000CB RID: 203
		Public Declare Function EmitNot Lib "LLVM-3.3" Alias "LLVMBuildNot" (ibuilder As IntPtr, value As IntPtr, name As String) As IntPtr

		' Token: 0x060000CC RID: 204
		Public Declare Function EmitShiftLeft Lib "LLVM-3.3" Alias "LLVMBuildShl" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000CD RID: 205
		Public Declare Function EmitShiftRightUnsigned Lib "LLVM-3.3" Alias "LLVMBuildLShr" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000CE RID: 206
		Public Declare Function EmitShiftRightSigned Lib "LLVM-3.3" Alias "LLVMBuildAShr" (ibuilder As IntPtr, left As IntPtr, right As IntPtr, name As String) As IntPtr

		' Token: 0x060000CF RID: 207
		Public Declare Function EmitStackAlloc Lib "LLVM-3.3" Alias "LLVMBuildAlloca" (ibuilder As IntPtr, typeref As IntPtr, name As String) As IntPtr

		' Token: 0x060000D0 RID: 208
		Public Declare Function EmitStackAlloc Lib "LLVM-3.3" Alias "LLVMBuildArrayAlloca" (ibuilder As IntPtr, typeref As IntPtr, size As IntPtr, name As String) As IntPtr

		' Token: 0x060000D1 RID: 209
		Public Declare Function EmitStore Lib "LLVM-3.3" Alias "LLVMBuildStore" (ibuilder As IntPtr, value As IntPtr, pointer As IntPtr) As IntPtr

		' Token: 0x060000D2 RID: 210
		Public Declare Function EmitLoad Lib "LLVM-3.3" Alias "LLVMBuildLoad" (ibuilder As IntPtr, pointer As IntPtr, name As String) As IntPtr

		' Token: 0x060000D3 RID: 211
		Public Declare Function EmitStructElementPointer Lib "LLVM-3.3" Alias "LLVMBuildStructGEP" (ibuilder As IntPtr, valueref As IntPtr, index As Integer, name As String) As IntPtr

		' Token: 0x060000D4 RID: 212
		Public Declare Function EmitGetElementPointer Lib "LLVM-3.3" Alias "LLVMBuildGEP" (ibuilder As IntPtr, value As IntPtr, offsets As IntPtr(), count As Integer, name As String) As IntPtr

		' Token: 0x060000D5 RID: 213
		Public Declare Function EmitTrunc Lib "LLVM-3.3" Alias "LLVMBuildTrunc" (ibuilder As IntPtr, value As IntPtr, destType As IntPtr, name As String) As IntPtr

		' Token: 0x060000D6 RID: 214
		Public Declare Function EmitZeroExtend Lib "LLVM-3.3" Alias "LLVMBuildZExt" (ibuilder As IntPtr, value As IntPtr, destType As IntPtr, name As String) As IntPtr

		' Token: 0x060000D7 RID: 215
		Public Declare Function EmitSignExtend Lib "LLVM-3.3" Alias "LLVMBuildSExt" (ibuilder As IntPtr, value As IntPtr, destType As IntPtr, name As String) As IntPtr

		' Token: 0x060000D8 RID: 216
		Public Declare Function EmitPointerCast Lib "LLVM-3.3" Alias "LLVMBuildPointerCast" (ibuilder As IntPtr, value As IntPtr, destType As IntPtr, name As String) As IntPtr

		' Token: 0x060000D9 RID: 217
		Public Declare Function EmitBitCast Lib "LLVM-3.3" Alias "LLVMBuildBitCast" (ibuilder As IntPtr, value As IntPtr, destType As IntPtr, name As String) As IntPtr

		' Token: 0x060000DA RID: 218
		Public Declare Function EmitIntToPtr Lib "LLVM-3.3" Alias "LLVMBuildIntToPtr" (ibuilder As IntPtr, value As IntPtr, destType As IntPtr, name As String) As IntPtr

		' Token: 0x060000DB RID: 219
		Public Declare Function EmitPtrToInt Lib "LLVM-3.3" Alias "LLVMBuildPtrToInt" (ibuilder As IntPtr, value As IntPtr, destType As IntPtr, name As String) As IntPtr

		' Token: 0x060000DC RID: 220
		Public Declare Function EmitExtract Lib "LLVM-3.3" Alias "LLVMBuildExtractValue" (ibuilder As IntPtr, value As IntPtr, index As Integer, name As String) As IntPtr

		' Token: 0x060000DD RID: 221
		Public Declare Function EmitInsert Lib "LLVM-3.3" Alias "LLVMBuildInsertValue" (ibuilder As IntPtr, into As IntPtr, what As IntPtr, index As Integer, name As String) As IntPtr

	End Module
End Namespace
