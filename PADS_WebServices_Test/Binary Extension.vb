Imports System.IO
Imports System.Runtime.CompilerServices

Module Binary_Extension

    <Extension()>
    Public Function ReadAllBytes(ByVal reader As BinaryReader) As Byte()
        Const bufferSize As Integer = 4096
        Using ms = New MemoryStream()
            Dim buffer As Byte() = New Byte(bufferSize - 1) {}
            Dim count As Integer
            While (__InlineAssignHelper(count, reader.Read(buffer, 0, buffer.Length))) <> 0
                ms.Write(buffer, 0, count)
            End While
            Return ms.ToArray()
        End Using
    End Function



    Private Function __InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function
End Module
