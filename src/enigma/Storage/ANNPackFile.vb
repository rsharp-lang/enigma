Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork.StoreProcedure

Public Class ANNPackFile : Implements IDisposable

    ReadOnly ann As Network
    ReadOnly file As StreamPack

    Private disposedValue As Boolean

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ann"></param>
    ''' <param name="file"></param>
    ''' <param name="openRead">
    ''' the target <paramref name="file"/> is just open for read model data
    ''' </param>
    Sub New(ann As Network, file As Stream, Optional openRead As Boolean = False)
        Me.ann = ann
        Me.file = New StreamPack(file, [readonly]:=openRead)

        If Not openRead Then

        End If
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub Write()
        Call CreateSnapshot _
            .TakeSnapshot(ann, {}) _
            .ScatteredStore(file)
    End Sub

    Public Function Load() As Network
        Return Scattered.ScatteredLoader(file).LoadModel
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function OpenRead(file As Stream) As Network
        Return New ANNPackFile(Nothing, file, openRead:=True).Load
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                Call file.Dispose()
            End If

            ' TODO: 释放未托管的资源(未托管的对象)并重写终结器
            ' TODO: 将大型字段设置为 null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: 仅当“Dispose(disposing As Boolean)”拥有用于释放未托管资源的代码时才替代终结器
    ' Protected Overrides Sub Finalize()
    '     ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
