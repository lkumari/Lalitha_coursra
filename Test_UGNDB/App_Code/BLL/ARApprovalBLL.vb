''******************************************************************************************************
''* ARApprovalBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 12/22/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ARTableAdapters

<System.ComponentModel.DataObject()> _
Public Class ARApprovalBLL
    Private ARApprovalTableAdapter As ARApprovalTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ARTableAdapters.ARApprovalTableAdapter
        Get
            If ARApprovalTableAdapter Is Nothing Then
                ARApprovalTableAdapter = New ARApprovalTableAdapter
            End If
            Return ARApprovalTableAdapter
        End Get
    End Property
    ''*****
    ''* Select ARApproval returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetAREventApprovalStatus(ByVal AREID As Integer, ByVal SubscriptionID As Integer) As AR.ARApprovalDataTable

        Try

            Return Adapter.GetAREventApprovalStatus(AREID, SubscriptionID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", SubscriptionID: " & SubscriptionID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARApproval : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARApprovalBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
   
    ' ''*****
    ''* Update AR Event Approval Status
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateAREventApprovalStatus(ByVal AREID As Integer, ByVal RoutingLevel As Integer, ByVal TeamMemberID As Integer, _
    ByVal SubscriptionID As Integer, ByVal Comment As String, _
    ByVal StatusID As Integer, ByVal original_RowID As Integer, ByVal RowID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.UpdateAREventApprovalStatus(original_RowID, AREID, RoutingLevel, TeamMemberID, _
            SubscriptionID, Comment, StatusID, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", AREID:" & AREID _
            & ", TeamMemberID:" & TeamMemberID _
            & ", Comment:" & Comment _
            & ", StatusID:" & StatusID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateARApproval : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateARApproval : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "ARApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
