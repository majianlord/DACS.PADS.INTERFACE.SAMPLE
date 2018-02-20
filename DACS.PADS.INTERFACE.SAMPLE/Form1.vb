

Imports System.Configuration
Imports DACS
Imports NLog


Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Setup DACS Crypto needed to Decrypt the Connection String
        Dim dacsCrypto As New Crypto
        'Setup Pads Interface for Adding a Record
        Dim padsInter As New PadsInterface(LogLevel.Debug, dacsCrypto.DecryptData(ConfigurationManager.AppSettings("PADS_PUBLIC_Connection")))
        'Get a Variable to Hold the JOBID just in case we want to track its Progress
        Dim padsJobID As Long
        'Then we call the PadsDBLoad to Load the Record for Pads to Process it.
        padsJobID = padsInter.PadsDbLoad(SampleRecord)

        'Job Tracking is not 100% in the Interface yet we expect it to be in a version or 2.

    End Sub

    Public Function SampleRecord() As PadsRecord
        'We need to Setup the Record Container to hold the Information
        Dim samplePadsRecord As New PadsRecord

        'Then we Setup the Job Information
        Dim sampleJob As New PadsJob
        'Do we want Parent Category's to Come down to this folder?  This is Hard Set to False right now until we have a system that needs it
        sampleJob.CatInheritFlag = False
        'Set the job date time
        sampleJob.JobDtTm = Now
        'Set the SourceSystem Name
        sampleJob.SrcSystem = "DACS.PADS>INTERFACE.SAMPLE"
        'Set the SourceSystemID (Used to Track the Job back to a Source system JOB)
        sampleJob.SrcReference = "Some Number or Name"
        'Then we Set the Path in CS to upload this file to using CS Standard Naming
        ''Allowed RootNames : PersonalWS,EnterpriseWS,CategoriesWS,ReportsWS
        sampleJob.CSPath = "EnterpriseWS:SampleFolder1:SampleFolder2"
        'Then we Set a CS File Name if you want it different than the name of the Uploaded File
        sampleJob.DestDocName = "My Test File"
        'Then we set the Library ID for these files to be uploaded to, This number you will need to get from the DACS SA for now as they are different for Public/Private 
        sampleJob.LibDetId = "1"
        'Last Step in the Basic Job creation is giving it a path to a file on a Share somewhere.
        sampleJob.SrcDocPath = "\\Server\Share\folder\Testfile.pdf"
        'Now we assign the Job Details to the Pads Record
        samplePadsRecord.Job = sampleJob

        ''If your JOB has no CATS and ATTS this is as far as you need to go you can submit this as is and it will upload the file to the folder

        'To Add Catts and ATTS to a Job First you need to Setup the Base Objects to Hold them in our case we ill be adding 2 Category's to the Document but you can add as many as needed
        Dim SampleCat1 As New Category
        Dim SampleCat2 As New Category
        'You will need the ID from CS to assign to the CategoryID this lets us Assign it to the correct Category
        SampleCat1.CategoryID = 1
        SampleCat2.CategoryID = 2

        'Now you need to create some Attributes to add to the category i only add 2 each here but you can do as many is needed per category
        Dim Attrib1 As New Attribute
        'Then we set some values
        'AttributeName is the name as it shows in Content Server
        Attrib1.AttributeName = "Date Time"
        'AttributeType is the type of Attribute as CS calls them
        Attrib1.AttributeType = AttributeType.DATEValue
        'Then you need to assign the correct value based on the type, aka DateValue for DateValue type
        'It needs to be a List(of Object type) as you can have more than 1 Value assigned to the same Attribute if the category is setup for it.
        Attrib1.DateValue = New List(Of DateTime) From {Now}
        'You can also do this all inline if you wanted..
        Dim Attrib2 As New Attribute With {.AttributeName = "String Value", .AttributeType = AttributeType.STRINGValue, .StringValue = New List(Of String) From {"SomeString", "SomeString2"}}
        Dim Cat2Attrib1 As New Attribute With {.AttributeName = "Integer Value", .AttributeType = AttributeType.INTEGERValue, .IntegerValue = New List(Of Long) From {1, 2, 3, 4, 5, 6}}
        'Booleans are the only Type in CS that cant have Multi-value at this time,  This might expand later with additional Cats and Attrs Modules
        Dim cat2attrib2 As New Attribute With {.AttributeName = "Bol Value", .AttributeType = AttributeType.BOOLEANValue, .BooleanValue = False}

        'Now that we have the Attributes Built out we can assign them to there Category Objects
        SampleCat1.Attributes = New List(Of Attribute) From {Attrib1, Attrib2}
        SampleCat2.Attributes = New List(Of Attribute) From {Cat2Attrib1, cat2attrib2}

        'Now we assign the Category's to the Record
        samplePadsRecord.JobCategory = New List(Of Category) From {SampleCat1, SampleCat2}

        'We are now done and we can Submit the Record
        Return samplePadsRecord
    End Function
End Class
