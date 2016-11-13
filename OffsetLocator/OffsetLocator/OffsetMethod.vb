' ***********************************************************************
' Assembly         : OffsetLocator
' Author           : fudmario
' Created          : 15-06-2015
'
' Last Modified By : fudmario
' Last Modified On : 11-12-2016
' ***********************************************************************
' <copyright file="Offset.vb" company="DeveloperTeam">
'     Copyright ©  2016
' </copyright>
' <summary></summary>
' ***********************************************************************
Imports System.IO
Imports OffsetLocator.Extensions

Public NotInheritable Class OffsetMethod
    Private Const FormatName As String = "{0}_{1}{2}"


#Region "OffsetLocator Functions"

    Public Shared Sub AvFucker(ByVal filePath As String, ByVal dirpath As String,
                                   ByVal offsetStart As Integer, ByVal offsetEnd As Integer,
                                   ByVal blockbyte As Integer, ByVal valueData As String)
        If Not File.Exists(filePath) Then Throw New FileNotFoundException(message:="File does not exist", fileName:=filePath)
        If Not Directory.Exists(dirpath) Then Throw New DirectoryNotFoundException(message:="The folder does not exist")
        Dim c As New FileInfo(filePath)

        If Not offsetStart.IsRange(0, c.Length) Then Throw New ArgumentOutOfRangeException(paramName:="offsetStart", message:="The initial value is not in range")
        If Not offsetEnd.IsRange(0, c.Length) Then Throw New ArgumentOutOfRangeException(paramName:="offsetEnd", message:="The initial value is not in range")

        Dim fileOutput As String
        Dim ext As String = Path.GetExtension(filePath)
        Dim value As Integer = Convert.ToInt32(value:=valueData, fromBase:=16)
        Dim byteArray As Byte() = Internal_GenBlockBytes(CByte(value), blockbyte)
        Dim res As List(Of Tuple(Of Integer, Integer)) = Internal_StepGenList(offsetStart, offsetEnd, blockbyte)
        Parallel.ForEach(res, Sub(m)
                                  fileOutput = Path.Combine(dirpath, String.Format(FormatName, m.Item1, blockbyte, ext))
                                  If m.Item2 <> 0 Then byteArray = Internal_GenBlockBytes(CByte(value), m.Item2)
                                  Internal_OffsetReplaceB(filePath, fileOutput, m.Item1, byteArray)
                              End Sub)
    End Sub
    Public Shared Sub DSplit(ByVal filepath As String, ByVal dirpath As String, ByVal offsetStart As Integer, ByVal offsetEnd As Integer,
                          ByVal blockBytes As Integer)
        If Not File.Exists(filepath) Then Throw New FileNotFoundException(message:="File does not exist", fileName:=filepath)
        If Not Directory.Exists(dirpath) Then Throw New DirectoryNotFoundException(message:="The folder does not exist")
        Dim c As New FileInfo(filepath)
        If Not offsetStart.IsRange(0, c.Length) Then Throw New ArgumentOutOfRangeException(paramName:="offsetStart", message:="The initial value is not in range")
        If Not offsetEnd.IsRange(0, c.Length) Then Throw New ArgumentOutOfRangeException(paramName:="offsetEnd", message:="The initial value is not in range")

        Dim res As List(Of Tuple(Of Integer, Integer)) = Internal_StepGenList(offsetStart, offsetEnd, blockBytes)

        Parallel.ForEach(res, Sub(m)
                                  Dim output As String = Path.Combine(dirpath, String.Format(FormatName, m.Item1, blockBytes, c.Extension))
                                  CreateFileWithBytes(filepath:=filepath, fileoutput:=output, length:=m.Item1)
                                  If m.Item2 > 0 Then
                                      CreateFileWithBytes(filepath, Path.Combine(dirpath, String.Format(FormatName, offsetEnd, m.Item2, c.Extension)), offsetEnd)
                                  End If
                              End Sub)

    End Sub
    Public Shared Sub C256Combination(ByVal filepath As String, ByVal dirpath As String, ByVal offset As Integer)
        If Not File.Exists(filepath) Then Throw New FileNotFoundException(message:="File does not exist", fileName:=filepath)
        If Not Directory.Exists(dirpath) Then Throw New DirectoryNotFoundException(message:="The folder does not exist")
        If Not offset.IsRange(0, New FileInfo(filepath).Length) Then Throw New ArgumentOutOfRangeException(paramName:="offset", message:="The initial value is not in range")

        Dim ext As String = Path.GetExtension(filepath)
        Dim value As String
        Dim output As String
        Parallel.For(0, 255, Sub(i)
                                 value = Convert.ToString(value:=i, toBase:=16).ToUpper
                                 If value.Length = 1 Then value = String.Format("0{0}", value)
                                 output = Path.Combine(dirpath, String.Format(FormatName, offset, value, ext))
                                 Internal_OffsetReplaceA(filepath, output, offset, i)
                             End Sub)


    End Sub
    Public Shared Sub OffsetReplace(ByVal filepath As String, ByVal fileOutput As String, ByVal offset As Integer, ByVal valueData As String)

        If Not File.Exists(filepath) Then Throw New FileNotFoundException(message:="File does not exist", fileName:=filepath)
        If Not offset.IsRange(0, New FileInfo(filepath).Length) Then Throw New ArgumentOutOfRangeException(paramName:="offset", message:="This value is not in range")
        Dim value As Integer = Convert.ToInt32(value:=valueData, fromBase:=16)
        Internal_OffsetReplaceA(filepath, fileOutput, offset, value)
    End Sub
#End Region
#Region " Internal Method"

    Private Shared Sub Internal_OffsetReplaceA(ByVal filepath As String, ByVal fileOutput As String,
                                                   ByVal offset As Integer, ByVal value As Integer)
        Using fs As New FileStream(path:=filepath, mode:=FileMode.Open, access:=FileAccess.Read)
            Using fs2 As New FileStream(path:=fileOutput, mode:=FileMode.Create, access:=FileAccess.Write)
                fs.CopyTo(fs2)
                fs2.Position = offset
                fs2.WriteByte(value:=CByte(value))
            End Using
        End Using
    End Sub

    Private Shared Sub Internal_OffsetReplaceB(ByVal filepath As String, ByVal fileOutput As String, ByVal offset As Integer, ByVal blockByte As Byte())
        Using fs As New FileStream(path:=filepath, mode:=FileMode.Open, access:=FileAccess.Read)
            Using fs2 As New FileStream(path:=fileOutput, mode:=FileMode.Create, access:=FileAccess.Write)
                fs.CopyTo(fs2)
                fs2.Position = offset
                fs2.Write(blockByte, 0, blockByte.Length)
            End Using
        End Using
    End Sub
    Private Shared Sub CreateFileWithBytes(filepath As String, fileoutput As String, length As Integer)
        Dim buffer As Byte() = New Byte(length - 1) {}
        Using fs As New FileStream(path:=filepath, mode:=FileMode.Open, access:=FileAccess.Read)
            fs.Seek(0, SeekOrigin.Begin)
            fs.Read(buffer, 0, buffer.Length)
        End Using
        File.WriteAllBytes(fileoutput, buffer)
    End Sub


    Private Shared Function Internal_GenBlockBytes(ByVal value As Byte, ByVal length As Integer) As Byte()
        Return ParallelEnumerable.Repeat(element:=value, count:=length).ToArray()
    End Function
    Private Shared Function Internal_StepGenList(ByVal startIndex As Integer, ByVal endIndex As Integer, ByVal stepSize As Integer) As List(Of Tuple(Of Integer, Integer))
        Dim c As New List(Of Tuple(Of Integer, Integer))
        For i = startIndex To endIndex Step stepSize
            Dim k As Integer = endIndex - i
            If k < stepSize Then
                c.Add(New Tuple(Of Integer, Integer)(item1:=i, item2:=k))
            Else
                c.Add(New Tuple(Of Integer, Integer)(item1:=i, item2:=0))
            End If
        Next
        Return c
    End Function

#End Region

End Class
