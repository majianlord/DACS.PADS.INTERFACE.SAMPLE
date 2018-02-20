Imports System.IO
Imports DACS



Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim WebService As New PADSUpload.PADSUploadSoapClient
        FileCrypto.CryptFile("C:\Temp\FiletoEncrypt.txt", "C:\Temp\FiletoEncrypt.txt.enc", "FiletoEncrypt.txt.enc", True)
        Dim UploadFileStream As New FileStream("C:\Temp\FiletoEncrypt.txt.enc", FileMode.Open, FileAccess.Read)
        Dim UploadBinaryReader As New BinaryReader(UploadFileStream)
        Dim UploadData As Byte() = UploadBinaryReader.ReadAllBytes
        UploadFileStream.Close()
        UploadFileStream.Close()
        Dim uploadResult As PADSUpload.FileUploadStatus = WebService.UploadFile(UploadData, "FiletoEncrypt.txt.enc", True)
        TextBox1.Text += uploadResult.UploadStatus.ToString & vbCrLf
        TextBox1.Text += uploadResult.UploadPath & vbCrLf
        Dim PadsCat As New PADSUpload.Category
        Dim Padsjob As New PADSUpload.PadsJob
        Dim PadsItem As New PADSUpload.PadsRecord
        Padsjob.CatInheritFlag = False
        Padsjob.CSPath = "EnterpriseWS:DEMO"
        Padsjob.DestDocName = "FiletoEncrypt.txt"
        Padsjob.JobDtTm = Now
        Padsjob.LibDetId = 1
        Padsjob.SrcDocPath = uploadResult.UploadPath
        Padsjob.SrcSystem = "UPLOADTOOL"
        Padsjob.SrcReference = Guid.NewGuid.ToString
        PadsItem.Job = Padsjob
        PadsCat.CategoryID = 1
        Dim Attribs As New List(Of PadsUpload.Attribute) From {
          New PadsUpload.Attribute With {.AttributeType = PadsUpload.AttributeType.DATEValue, .AttributeName = "Date Time", .DateValue = New PadsUpload.ArrayOfDateTime From {DateTime.Now, DateTime.Now.AddDays(1)}},
          New PadsUpload.Attribute With {.AttributeType = PadsUpload.AttributeType.BOOLEANValue, .AttributeName = "CheckBox", .BooleanValue = True},
          New PadsUpload.Attribute With {.AttributeType = PadsUpload.AttributeType.INTEGERValue, .AttributeName = "Integer Field", .IntegerValue = New PadsUpload.ArrayOfLong From {50, 100}},
          New PadsUpload.Attribute With {.AttributeType = PadsUpload.AttributeType.INTEGERValue, .AttributeName = "Integer Popup", .IntegerValue = New PadsUpload.ArrayOfLong From {1, 2, 3, 4, 5}},
          New PadsUpload.Attribute With {.AttributeType = PadsUpload.AttributeType.STRINGValue, .AttributeName = "textbox32", .StringValue = New PadsUpload.ArrayOfString From {"String", "String2"}}
        }
        PadsCat.Attributes = Attribs
        PadsItem.JobCategory = New List(Of PADSUpload.Category) From {PadsCat}
        Dim Result As Integer = WebService.AddPadsJob(PadsItem)
        TextBox1.Text += Result & vbCrLf 

    End Sub
End Class
