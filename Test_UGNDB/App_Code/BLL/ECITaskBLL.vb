''******************************************************************************************************
''* ECITaskBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 07/01/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECITaskBLL
    Private ECITaskAdapter As ECITaskTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECITaskTableAdapter
        Get
            If ECITaskAdapter Is Nothing Then
                ECITaskAdapter = New ECITaskTableAdapter()
            End If
            Return ECITaskAdapter
        End Get
    End Property
    ''*****
    ''* Select ECITask returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECITask(ByVal ECINo As Integer) As ECI.ECITask_MaintDataTable

        Try

            Return Adapter.GetECITask(ECINo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECITask : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECITaskBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECITask : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECITaskBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New ECITask
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertECITask(ByVal ECINo As Integer, ByVal TaskID As Integer, ByVal TaskTeamMemberID As Integer, ByVal TargetDate As String) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If TargetDate Is Nothing Then
                TargetDate = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertECITask(ECINo, TaskID, TaskTeamMemberID, TargetDate, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo:" & ECINo & ",TaskID:" & TaskID _
            & ", TaskTeamMemberID:" & TaskTeamMemberID & ",TargetDate:" & TargetDate _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECITask : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECITaskBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECITask : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECITaskBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    '* Update ECITask
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateECITask(ByVal RowID As Integer, ByVal original_RowID As Integer, ByVal TaskID As Integer, _
        ByVal TaskTeamMemberID As Integer, ByVal TargetDate As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If TargetDate Is Nothing Then
                TargetDate = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateECITask(original_RowID, TaskID, TaskTeamMemberID, TargetDate, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", TaskID: " & TaskID _
            & ", TaskTeamMemberID: " & TaskTeamMemberID _
            & ", TargetDate: " & TargetDate _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateECITask : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECITaskBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateECITask : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECITaskBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    '* Delete ECITask
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteECITask(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim rowsAffected As Integer = Adapter.DeleteECITask(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteECITask: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECITaskBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteECITask: " & commonFunctions.convertSpecialChar(ex.Message, False), "ECITaskBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
