﻿Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.MachineLearning

Public MustInherit Class MLPackFile : Implements IDisposable

    Public Const Model_Class As String = "/etc/model.class"

    Protected ReadOnly file As StreamPack

    Dim disposedValue As Boolean

    Public MustOverride ReadOnly Property [Class] As String

    Protected Sub New(file As StreamPack)
        Me.file = file
    End Sub

    Public MustOverride Sub Write()

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function CheckClass() As Boolean
        Return [Class] = GetClass(file)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function GetClass(file As StreamPack) As String
        Return file.ReadText(Model_Class).Trim
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

Public MustInherit Class MLPackFile(Of T As MLModel) : Inherits MLPackFile

    Protected ReadOnly model As T

    ''' <summary>
    ''' model writer
    ''' </summary>
    ''' <param name="model"></param>
    ''' <param name="file"></param>
    Sub New(model As T, file As Stream)
        Call MyBase.New(New StreamPack(file, [readonly]:=False))

        Me.model = model
        Me.file.WriteText(Me.Class, Model_Class)
    End Sub

    ''' <summary>
    ''' model reader
    ''' </summary>
    ''' <param name="file"></param>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Sub New(file As Stream)
        Call MyBase.New(New StreamPack(file, [readonly]:=True))
    End Sub

    Public Overrides Sub Write()
        Call WriteModel()

        ' write the general model information
        Call file.WriteText(model.Features, "/features.txt")
        Call file.WriteText(model.Labels, "/labels.txt")
    End Sub

    Protected MustOverride Sub WriteModel()
    Protected MustOverride Function Load() As Model

End Class
