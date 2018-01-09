''******************************************************************************************************
''* RDPrioritiesBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: Priorities.aspx - gvPriorities
''* Author  : LRey 05/18/2009
''******************************************************************************************************
Imports RDTestIssuanceTableAdapters
<System.ComponentModel.DataObject()> _
Public Class RDPrioritiesBLL
    Private tcAdapter As TestIssuance_Priorities_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As RDTestIssuanceTableAdapters.TestIssuance_Priorities_TableAdapter
        Get
            If tcAdapter Is Nothing Then
                tcAdapter = New TestIssuance_Priorities_TableAdapter()
            End If
            Return tcAdapter
        End Get
    End Property

    ''*****
    ''* Select TestIssuance_Priorities_Maint returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetPriorities(ByVal PriorityDescription As String) As RDTestIssuance.TestIssuance_PrioritiesDataTable

        Try
            If PriorityDescription = Nothing Then PriorityDescription = ""

            Return Adapter.Get_TestIssuance_Priorities(PriorityDescription)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PriorityDescription: " & PriorityDescription & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPriorities : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RDPrioritiesBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RnD/Prorities_Maint.aspx"

            UGNErrorTrapping.InsertErrorLog("GetPriorities : " & commonFunctions.convertSpecialChar(ex.Message, False), "RDPrioritiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function
    ''*****
    ''* Insert New TestIssuance_Priorities_Maint
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertPriorities(ByVal ColorCode As String, ByVal PriorityDescription As String) As Boolean
        Try
            ' Create a new TestIssuance_Priorities_MaintRow instance
            Dim tcTable As New RDTestIssuance.TestIssuance_PrioritiesDataTable
            Dim tcRow As RDTestIssuance.TestIssuance_PrioritiesRow = tcTable.NewTestIssuance_PrioritiesRow
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ' Logical Rule - Cannot insert a record without a null  column
            If ColorCode = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Color Code - is a required field.")
            End If
            If PriorityDescription = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Prioritiy Description - is a required field.")
            End If

            ' Insert the new TestIssuance_Assignments row
            Dim rowsAffected As Integer = Adapter.sp_Insert_TestIssuance_Priorities(ColorCode, PriorityDescription, User)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PriorityDescription: " & PriorityDescription & ", ColorCode:" & ColorCode & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertPriorities : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RDPrioritiesBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RnD/Prorities_Maint.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertPriorities : " & commonFunctions.convertSpecialChar(ex.Message, False), "RDPrioritiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function
    ''*****
    ''* Update TestIssuance_Priorities_Maint
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function UpdatePriorities(ByVal PriorityDescription As String, ByVal Obsolete As Boolean, ByVal original_PID As Integer, ByVal ColorCode As String) As Boolean

        Try
            ' Create a new TestIssuance_Priorities_MaintRow instance
            Dim tcTable As New RDTestIssuance.TestIssuance_PrioritiesDataTable
            Dim tcRow As RDTestIssuance.TestIssuance_PrioritiesRow = tcTable.NewTestIssuance_PrioritiesRow
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ' Logical Rule - Cannot insert a record without a null column
            If ColorCode = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Color Code - is a required field.")
            End If
            If PriorityDescription = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Prioritiy Description - is a required field.")
            End If

            ' Insert the new TestIssuance_Assignments row
            Dim rowsAffected As Integer = Adapter.sp_Update_TestIssuance_Priorities(original_PID, ColorCode, PriorityDescription, Obsolete, User)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PriorityDescription: " & PriorityDescription & ", ColorCode:" & ColorCode & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdatePriorities : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RDPrioritiesBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RnD/Prorities_Maint.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdatePriorities : " & commonFunctions.convertSpecialChar(ex.Message, False), "RDPrioritiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function

End Class
