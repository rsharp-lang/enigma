Imports System.Runtime
Imports System.Runtime.CompilerServices

Namespace LLVM

    ' Token: 0x0200000A RID: 10
    Public Module CallingConventionHelpers

        ' Token: 0x06000012 RID: 18 RVA: 0x000021C0 File Offset: 0x000003C0
        <Extension>
        Public Function ToLLVM(convention As InteropServices.CallingConvention) As CallingConvention
            Select Case convention
                Case InteropServices.CallingConvention.Cdecl
                    Return CallingConvention.C
                Case InteropServices.CallingConvention.StdCall
                    Return CallingConvention.StdCallX86
                Case InteropServices.CallingConvention.ThisCall
                    Throw New NotSupportedException()
                Case InteropServices.CallingConvention.FastCall
                    Return CallingConvention.FastCallX86
                Case Else
                    Throw New NotImplementedException(convention.ToString())
            End Select
        End Function
    End Module
End Namespace
