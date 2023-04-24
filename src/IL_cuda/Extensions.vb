Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Linq

Module Extensions

    <Extension()>
    Public Function IndexOf(Of T)(enumerable As IEnumerable(Of T), selector As Func(Of T, Boolean)) As Integer
        Dim q = enumerable.SeqIterator.First(Function(pair) selector(pair.value))
        Dim index As Integer = q.i
        Return index
    End Function

    <Extension()>
    Public Function StripNameToValidPtx(name As String) As String
        Dim builder As New StringBuilder(name.Length)
        For Each c In name.Where(Function(ci) Char.IsLetterOrDigit(ci) OrElse ci = "_"c)
            builder.Append(c)
        Next
        Return builder.ToString()
    End Function
End Module
