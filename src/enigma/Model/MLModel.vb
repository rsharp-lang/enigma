Imports Microsoft.VisualBasic.MachineLearning
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Components.Interface
Imports SMRUCC.Rsharp.Runtime.Internal.Object

''' <summary>
''' the machine learning model
''' </summary>
Public MustInherit Class MLModel : Implements RPipeline

    ''' <summary>
    ''' the trained machine learning model object
    ''' </summary>
    ''' <returns></returns>
    Public Property Model As Model

    ''' <summary>
    ''' the input feature names to create input data
    ''' </summary>
    ''' <returns></returns>
    Public Property Features As String()
    ''' <summary>
    ''' the output labels
    ''' </summary>
    ''' <returns></returns>
    Public Property Labels As String()
    Public Property data As dataframe

    Public ReadOnly Property isError As Boolean Implements RPipeline.isError
        Get
            Return Not err Is Nothing
        End Get
    End Property

    Protected Friend err As Message

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns>
    ''' this function just returns it self with 
    ''' the <see cref="Model"/> property value 
    ''' updated.
    ''' </returns>
    Public MustOverride Function DoCallTraining(args As list, env As Environment) As MLModel

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="env"></param>
    ''' <returns>
    ''' this function should returns a <see cref="dataframe"/> object.
    ''' </returns>
    Public MustOverride Function Solve(data As Object, env As Environment) As Object

    Public Function getError() As Message Implements RPipeline.getError
        Return err
    End Function
End Class

Public Class MLPipelineError : Inherits MLModel

    Sub New(err As Message)
        Me.err = err
    End Sub

    Public Overrides Function DoCallTraining(args As list, env As Environment) As MLModel
        Return Me
    End Function

    Public Overrides Function Solve(data As Object, env As Environment) As Object
        Return getError()
    End Function

    Public Overrides Function ToString() As String
        Return getError.ToString
    End Function
End Class