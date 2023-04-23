''' <summary>
''' Generic exception from CudaSharp
''' </summary>
Public Class CudaSharpException : Inherits Exception

    ''' <summary>
    ''' Creates a CudaSharp exception
    ''' </summary>
    ''' <param name="message">Message</param>
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub
End Class