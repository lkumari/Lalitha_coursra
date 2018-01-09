''******************************************************************************************************
''* RFDApprovalBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 01/18/2010
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDApprovalBLL
    Private RFDApprovalAdapter As RFDApprovalTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDApprovalTableAdapter
        Get
            If RFDApprovalAdapter Is Nothing Then
                RFDApprovalAdapter = New RFDApprovalTableAdapter()
            End If
            Return RFDApprovalAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDApproval returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDApproval(ByVal RFDNo As Integer, ByVal SubscriptionID As Integer, _
        ByVal TeamMemberID As Integer, ByVal filterNotified As Boolean, ByVal isNotified As Boolean, _
        ByVal isHistorical As Boolean, ByVal filterWorking As Boolean, ByVal isWorking As Boolean) As RFD.RFDApproval_MaintDataTable

        Try

            Return Adapter.GetRFDApproval(RFDNo, SubscriptionID, TeamMemberID, filterNotified, isNotified, isHistorical, filterWorking, isWorking)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", SubscriptionID: " & SubscriptionID _
            & ", TeamMemberID: " & TeamMemberID & ", FilterNotified: " & filterNotified _
            & ", isNotified: " & isNotified & ", isHistorical: " & isHistorical _
            & ", filterWorking: " & isNotified & ", isWorking: " & isHistorical _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDApproval : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDApproval : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "RFDApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ' ''*****
    ' ''* Insert New RFDApproval
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    'Public Function InsertRFDApproval(ByVal RFDNo As Integer, ByVal UGNDBApprovalID As Integer, ByVal PPAPDueDate As String, ByVal PPAPCompletionDate As String, ByVal ApprovalSignedDate As String) As Boolean

    '    Try
    '        Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
    '
    '        Dim rowsAffected As Integer = Adapter.InsertRFDApproval(RFDNo, UGNDBApprovalID, PPAPDueDate, PPAPCompletionDate, ApprovalSignedDate, createdBy)
    '        ' Return true if precisely one row was inserted, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RFDNo:" & RFDNo _
    '        & ", UGNDBApprovalID:" & UGNDBApprovalID _
    '        & ", PPAPDueDate:" & PPAPDueDate _
    '        & ", PPAPCompletionDate:" & PPAPCompletionDate _
    '        & ", ApprovalSignedDate:" & ApprovalSignedDate _
    '        & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertRFDApproval : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDApprovalBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertRFDApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDApprovalBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
    ''*****
    '* Update RFDApproval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateRFDApproval(ByVal RFDNo As Integer, ByVal SubscriptionID As Integer, _
        ByVal TeamMemberID As Integer, ByVal original_RowID As Integer) As Boolean
        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'in reality, this will obsolete the current team member and insert a new one. This way, the comments and other details remain in history
            Dim rowsAffected As Integer = Adapter.ChangeRFDApprovalTeamMember(RFDNo, SubscriptionID, TeamMemberID, UpdatedBy)
            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", RFDNo:" & RFDNo _
            & ", SubscriptionID:" & SubscriptionID _
            & ", TeamMemberID:" & TeamMemberID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDApproval : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDApprovalBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Delete RFDApproval
    ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    '    Public Function DeleteRFDApproval(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

    '    Try

    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        Dim rowsAffected As Integer = Adapter.DeleteRFDApproval(original_RowID, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RowID:" & original_RowID _
    '        & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "DeleteRFDApproval : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDApprovalBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("DeleteRFDApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDApprovalBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function

End Class
