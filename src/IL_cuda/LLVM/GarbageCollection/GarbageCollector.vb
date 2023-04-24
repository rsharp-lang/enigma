Imports System.Runtime.InteropServices
Imports Enigma.LLVM.GarbageCollection.NativeGlue

Namespace LLVM.GarbageCollection
    ' Token: 0x0200002E RID: 46
    Public MustInherit Class GarbageCollector
        ' Token: 0x06000126 RID: 294 RVA: 0x00003A20 File Offset: 0x00001C20
        Protected Sub New()
            Me._findCustomSafePoints = Marshal.GetFunctionPointerForDelegate(New Func(Of IntPtr, IntPtr, Boolean)(AddressOf Me.FindCustomSafePoints))
            Me._initializeCustomLowering = Marshal.GetFunctionPointerForDelegate(New Func(Of [Module], Boolean)(AddressOf Me.InitializeCustomLowering))
            Me._performCustomLowering = Marshal.GetFunctionPointerForDelegate(New Func(Of [Function], Boolean)(AddressOf Me.PerformCustomLowering))
        End Sub

        ' Token: 0x06000127 RID: 295
        Protected MustOverride Function InitializeCustomLowering([module] As [Module]) As Boolean

        ' Token: 0x06000128 RID: 296 RVA: 0x00003A80 File Offset: 0x00001C80
        Private Function InitializeCustomLowering([module] As IntPtr) As Boolean
            Return Me.InitializeCustomLowering(New [Module]([module]))
        End Function

        ' Token: 0x06000129 RID: 297
        Protected MustOverride Function PerformCustomLowering([function] As [Function]) As Boolean

        ' Token: 0x0600012A RID: 298 RVA: 0x00003A8E File Offset: 0x00001C8E
        Private Function PerformCustomLowering([function] As IntPtr) As Boolean
            Return Me.PerformCustomLowering(New [Function]([function]))
        End Function

        ' Token: 0x0600012B RID: 299
        Protected MustOverride Function FindCustomSafePoints(functionInfo As IntPtr, machineFunction As IntPtr) As Boolean

        ' Token: 0x17000022 RID: 34
        ' (get) Token: 0x0600012C RID: 300
        Public MustOverride ReadOnly Property Name As String

        ' Token: 0x0600012D RID: 301 RVA: 0x00003A9C File Offset: 0x00001C9C
        Private Function GetExternalGCInfo() As ExternalGCInfo
            Return New ExternalGCInfo() With {
                .CustomReadBarriers = Me.CustomReadBarriers,
                .CustomRoots = Me.CustomRoots,
                .CustomSafePoints = Me.CustomSafePoints,
                .CustomWriteBarriers = Me.CustomWriteBarriers,
                .FindCustomSafePoints = Me._findCustomSafePoints,
                .InitializeCustomLowering = Me._initializeCustomLowering,
                .InitRoots = Me.InitRoots,
                .NeededSafePoints = Me.NeededSafePoints,
                .PerformCustomLowering = Me._performCustomLowering,
                .UsesMetadata = Me.UsesMetadata
            }
        End Function

        ' Token: 0x0600012E RID: 302 RVA: 0x00003B34 File Offset: 0x00001D34
        Public Shared Sub Register(Of GC As {GarbageCollector, New})()
            Dim Hx0___21_ As GCStrategyConstructor = c__21(Of GC).Hx0___21_0
            Dim gcstrategyConstructor As GCStrategyConstructor = Hx0___21_
            If Hx0___21_ Is Nothing Then
                Dim gcstrategyConstructor2 As GCStrategyConstructor =
                    Function()
                        Dim obj2 As List(Of GarbageCollector) = GarbageCollector.collectors
                        Dim result As IntPtr
                        SyncLock obj2
                            Dim gc1 As GC = Activator.CreateInstance(Of GC)()
                            Dim intPtr As IntPtr = llvm.CreateGC(gc1.GetExternalGCInfo())
                            GarbageCollector.collectors.Add(gc1)
                            result = intPtr
                        End SyncLock
                        Return result
                    End Function
                gcstrategyConstructor = gcstrategyConstructor2
                c__21(Of GC).Hx0___21_0 = gcstrategyConstructor2
            End If
            Dim gcstrategyConstructor3 As GCStrategyConstructor = gcstrategyConstructor
            Dim fullName As String = GetType(GC).FullName
            Dim name As IntPtr = Marshal.StringToHGlobalAnsi(fullName)
            Dim obj As Dictionary(Of String, GCStrategyConstructor) = GarbageCollector.constructors

            SyncLock obj
                GarbageCollector.constructors.Add(fullName, gcstrategyConstructor3)
                Try
                    llvm.RegisterGC(name, IntPtr.Zero, gcstrategyConstructor3)
                Catch ex As Exception
                    GarbageCollector.constructors.Remove(fullName)
                    Throw
                End Try
            End SyncLock
        End Sub

        ' Token: 0x0600012F RID: 303 RVA: 0x00003BDC File Offset: 0x00001DDC
        ' Note: this type is marked as 'beforefieldinit'.
        Shared Sub New()
        End Sub

        ' Token: 0x04000031 RID: 49
        Protected NeededSafePoints As SafePointKind

        ' Token: 0x04000032 RID: 50
        Protected CustomReadBarriers As Boolean

        ' Token: 0x04000033 RID: 51
        Protected CustomWriteBarriers As Boolean

        ' Token: 0x04000034 RID: 52
        Protected CustomRoots As Boolean

        ' Token: 0x04000035 RID: 53
        Protected CustomSafePoints As Boolean

        ' Token: 0x04000036 RID: 54
        Protected InitRoots As Boolean = True

        ' Token: 0x04000037 RID: 55
        Protected UsesMetadata As Boolean

        ' Token: 0x04000038 RID: 56
        Private _findCustomSafePoints As IntPtr

        ' Token: 0x04000039 RID: 57
        Private _initializeCustomLowering As IntPtr

        ' Token: 0x0400003A RID: 58
        Private _performCustomLowering As IntPtr

        ' Token: 0x0400003B RID: 59
        Private Shared collectors As New List(Of GarbageCollector)()

        ' Token: 0x0400003C RID: 60
        Private Shared constructors As New Dictionary(Of String, GCStrategyConstructor)()

        <Serializable()>
        Private NotInheritable Class c__21(Of GC As {GarbageCollector, New})
            ' Token: 0x06000169 RID: 361 RVA: 0x0000441E File Offset: 0x0000261E
            ' Note: this type is marked as 'beforefieldinit'.
            Shared Sub New()
            End Sub

            ' Token: 0x0600016A RID: 362 RVA: 0x000037DA File Offset: 0x000019DA
            Public Sub New()
            End Sub

            ' Token: 0x0600016B RID: 363 RVA: 0x0000442C File Offset: 0x0000262C
            Friend Function b__21_0() As IntPtr
                Dim collectors As List(Of GarbageCollector) = GarbageCollector.collectors
                Dim result As IntPtr
                SyncLock collectors
                    Dim gc As GC = Activator.CreateInstance(Of GC)()
                    Dim intPtr As IntPtr = llvm.CreateGC(gc.GetExternalGCInfo())
                    GarbageCollector.collectors.Add(gc)
                    result = intPtr
                End SyncLock
                Return result
            End Function

            ' Token: 0x04000060 RID: 96
            Public Shared Hx0_ As c__21(Of GC) = New c__21(Of GC)()

            ' Token: 0x04000061 RID: 97
            Public Shared Hx0___21_0 As GCStrategyConstructor
        End Class
    End Class
End Namespace
