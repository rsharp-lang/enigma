Imports Microsoft.VisualBasic.MachineLearning
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object

''' <summary>
''' the machine learning model
''' </summary>
Public MustInherit Class MLModel

    ''' <summary>
    ''' the trained machine learning model object
    ''' </summary>
    ''' <returns></returns>
    Public Property Model As Model

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

End Class