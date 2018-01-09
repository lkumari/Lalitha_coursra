''******************************************************************************************************
''* RDTestingClassificationBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: TestingClassification.aspx - gvTestClass
''* Author  : LRey 03/30/2009
''******************************************************************************************************
Imports RDTestIssuanceTableAdapters
<System.ComponentModel.DataObject()> _
Public Class RDTestingClassificationBLL
    Private tcAdapter As Testing_Classification_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As RDTestIssuanceTableAdapters.Testing_Classification_TableAdapter
        Get
            If tcAdapter Is Nothing Then
                tcAdapter = New Testing_Classification_TableAdapter()
            End If
            Return tcAdapter
        End Get
    End Property


    ''*****
    ''* Select TestingClassification_Maint returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetTestingClassification(ByVal TestClassName As String) As RDTestIssuance.Testing_ClassificationDataTable

        Try
            If TestClassName = Nothing Then TestClassName = ""

            Return Adapter.Get_Testing_Classification(TestClassName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TestClassName: " & TestClassName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTestingClassification : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RDTestingClassificationBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RnD/TestingClass_Maint.aspx"

            UGNErrorTrapping.InsertErrorLog("GetTestingClassification : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RDTestingClassificationBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New TestingClassification_Maint
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertTestingClassification(ByVal TestClassName As String) As Boolean
        Try


            ' Create a new TestingClassification_MaintRow instance
            Dim tcTable As New RDTestIssuance.Testing_ClassificationDataTable
            Dim tcRow As RDTestIssuance.Testing_ClassificationRow = tcTable.NewTesting_ClassificationRow
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ' Logical Rule - Cannot insert a record without a null Subscriptions column
            If TestClassName = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Testing Classification Name - is a required field.")
            End If

            ' Insert the new TestIssuance_Assignments row
            Dim rowsAffected As Integer = Adapter.sp_Insert_Testing_Classification(TestClassName, User)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TestClassName: " & TestClassName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTestingClassification : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RDTestingClassificationBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RnD/TestingClass_Maint.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertTestingClassification : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RDTestingClassificationBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function
    ''*****
    ''* Update TestingClassification_Maint
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function UpdateTestingClassification(ByVal TestClassName As String, ByVal Obsolete As Boolean, ByVal Original_TestClassID As Integer) As Boolean
        Try
            ' Create a new TestingClassification_MaintRow instance
            Dim tcTable As New RDTestIssuance.Testing_ClassificationDataTable
            Dim tcRow As RDTestIssuance.Testing_ClassificationRow = tcTable.NewTesting_ClassificationRow
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ' Logical Rule - Cannot insert a record without a null Subscriptions column
            If TestClassName = Nothing Then
                Throw New ApplicationException("Update Cancelled: Testing Classification Name - is a required field.")
            End If

            ' Insert the new TestIssuance_Assignments row
            Dim rowsAffected As Integer = Adapter.sp_Update_Testing_Classification(Original_TestClassID, TestClassName, Obsolete, User)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TestClassName: " & TestClassName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateTestingClassification : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RDTestingClassificationBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RnD/TestingClass_Maint.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateTestingClassification : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RDTestingClassificationBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

End Class
