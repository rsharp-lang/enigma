Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.CompilerServices

Friend Module Extensions
    <Extension()>
    Public Function IndexOf(Of T)(ByVal enumerable As IEnumerable(Of T), ByVal selector As Func(Of T, Boolean)) As Integer
        Return enumerable.Select(Function(e, i) New With {
            e,
            i
        }).First(Function(pair) selector(pair.e)).i
    End Function

    <Extension()>
    Public Function StripNameToValidPtx(ByVal name As String) As String
        Dim builder = New StringBuilder(name.Length)
        For Each c In name.Where(Function(ci) Char.IsLetterOrDigit(ci) OrElse ci = "_"c)
            builder.Append(c)
        Next
        Return builder.ToString()
    End Function
End Module
