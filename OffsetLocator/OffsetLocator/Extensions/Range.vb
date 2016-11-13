Imports System.Runtime.CompilerServices

Namespace Extensions
    <ComponentModel.ImmutableObject(True)>
    <HideModuleName>
    Module Range
        <Extension>
        <DebuggerStepThrough>
        Public Function IsRange(sender As Integer, inicial As Integer, final As Integer) As Boolean
            Return ((sender >= inicial) AndAlso (sender <= final))
        End Function
    End Module
End Namespace