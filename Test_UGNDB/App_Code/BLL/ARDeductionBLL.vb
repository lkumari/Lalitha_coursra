''******************************************************************************************************
''* ARDeductionBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : 04/10/2012    LRey
''* Modified: 07/11/2012    LRey    Added DefaultNotify to the AR_Deduction_Reason Update and Insert
''******************************************************************************************************

Imports ARTableAdapters
<System.ComponentModel.DataObject()> _
Public Class ARDeductionBLL
    Private pAdapter1 As AR_Deduction_Approval_TableAdapter = Nothing
    Private pAdapter2 As AR_Deduction_Documents_TableAdapter = Nothing
    Private pAdapter3 As AR_Deduction_RSS_TableAdapter = Nothing
    Private pAdapter4 As AR_Deduction_RSS_Reply_TableAdapter = Nothing
    Private pAdapter5 As AR_Deduction_Reason_TableAdapter = Nothing
    Protected ReadOnly Property Adapter1() As ARTableAdapters.AR_Deduction_Approval_TableAdapter
        Get
            If pAdapter1 Is Nothing Then
                pAdapter1 = New AR_Deduction_Approval_TableAdapter()
            End If
            Return pAdapter1
        End Get
    End Property
    Protected ReadOnly Property Adapter2() As ARTableAdapters.AR_Deduction_Documents_TableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New AR_Deduction_Documents_TableAdapter()
            End If
            Return pAdapter2
        End Get
    End Property

    Protected ReadOnly Property Adapter3() As ARTableAdapters.AR_Deduction_RSS_TableAdapter
        Get
            If pAdapter3 Is Nothing Then
                pAdapter3 = New AR_Deduction_RSS_TableAdapter()
            End If
            Return pAdapter3
        End Get
    End Property

    Protected ReadOnly Property Adapter4() As ARTableAdapters.AR_Deduction_RSS_Reply_TableAdapter
        Get
            If pAdapter4 Is Nothing Then
                pAdapter4 = New AR_Deduction_RSS_Reply_TableAdapter()
            End If
            Return pAdapter4
        End Get
    End Property
    Protected ReadOnly Property Adapter5() As ARTableAdapters.AR_Deduction_Reason_TableAdapter
        Get
            If pAdapter5 Is Nothing Then
                pAdapter5 = New AR_Deduction_Reason_TableAdapter()
            End If
            Return pAdapter5
        End Get
    End Property
    ''*****
    ''* Select AR_Deduction_Approval returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetARDeductionApproval(ByVal ARDID As Integer, ByVal SeqNo As Integer) As AR.AR_Deduction_ApprovalDataTable

        Try
            Return Adapter1.Get_AR_Deduction_Approval(ARDID, SeqNo, 0, 0, 0)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", SeqNo: " & SeqNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionApproval : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARDeductionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Deduction_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetARDeductionApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARDeductionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetARDeductionApproval

    '*****
    '* Update AR_Deduction_Approval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function UpdateARDeductionApproval(ByVal Status As String, ByVal Comments As String, ByVal SameTMID As Boolean, ByVal original_ARDID As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer, ByVal TeamMemberName As String, ByVal DateNotified As String, ByVal SeqNo As Integer, ByVal ddStatus As String) As Boolean

        Try
            Dim pTable As AR.AR_Deduction_ApprovalDataTable = Adapter1.Get_AR_Deduction_Approval(original_ARDID, 0, 0, 0, 0)
            Dim pscpRow As AR.AR_Deduction_ApprovalRow = pTable(0)
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As String = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim TMSigned As Boolean = False

            If pTable.Count = 0 Then
                ' no matching record found, return false
                Return False
            End If

            ' Logical Rule - Cannot update a record without null columns
            If original_ARDID = Nothing Then
                Throw New ApplicationException("Update Cancelled: ARDID is a required field.")
            End If
            'If Status = Nothing Then
            '    Throw New ApplicationException("Update Cancelled: Status is a required field.")
            'End If
            If Comments = Nothing And Status = "Rejected" Then
                Throw New ApplicationException("Update Cancelled: Comments is a required field.")
            End If

            If ddStatus <> "Pending" Then
                TMSigned = True
            End If
            If Comments <> "" Or Comments <> Nothing Then
                ' Update the ExpProj_Assets_Approval record
                Dim rowsAffected As Integer = Adapter1.sp_Update_AR_Deduction_Approval(original_ARDID, original_TeamMemberID, TMSigned, ddStatus, Comments, original_SeqNo, SameTMID, User, Date.Today)

                ' Return true if precisely one row was updated, otherwise false
                Return rowsAffected = 1
            Else
                Return False
            End If
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & original_ARDID & ", SeqNo: " & SeqNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateARDeductionApproval : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARDeductionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Deduction_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateARDeductionApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARDeductionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateARDeductionApproval

    ''*****
    ''* Select AR_Deduction_Documents returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetARDeductionDocuments(ByVal ARDID As Integer, ByVal DocID As Integer, ByVal MaxDateOfUpload As Boolean) As AR.AR_Deduction_DocumentsDataTable

        Try
            Return Adapter2.Get_AR_Deduction_Documents(ARDID, DocID, False)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARDeductionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Deduction_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetARDeductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARDeductionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetARDeductionDocuments

    ''*****
    ''* Delete AR_Deduction_Documents returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function DeleteARDeductionDocuments(ByVal Original_ARDID As Integer, ByVal Original_DocID As Integer) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter2.sp_Delete_AR_Deduction_Documents(Original_ARDID, Original_DocID)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Original_ARDID: " & Original_ARDID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteARDeductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARDeductionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Deduction_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteARDeductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARDeductionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF DeleteARDeductionDocuments
    ''*****
    ''* Select AR_Deduction_RSS returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetARDeductionRSS(ByVal ARDID As Integer, ByVal RSSID As Integer) As AR.AR_Deduction_RSSDataTable

        Try
            Return Adapter3.Get_AR_Deduction_RSS(ARDID, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionRSS : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARDeductionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Deduction_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetARDeductionRSS : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARDeductionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetARDeductionRSS
    ''*****
    ''* Select AR_Deduction_RSS_Reply returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetARDeductionRSSReply(ByVal ARDID As Integer, ByVal RSSID As Integer) As AR.AR_Deduction_RSS_ReplyDataTable

        Try
            Return Adapter4.Get_AR_Deduction_RSS_Reply(ARDID, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARDeductionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Deduction_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetARDeductionRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARDeductionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetARDeductionRSSReply

    ''*****
    ''* Select AR_Deduction_Reason returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetARDeductionReason(ByVal ReasonDesc As String) As AR.AR_Deduction_ReasonDataTable

        Try
            Return Adapter5.Get_AR_Deduction_Reason(ReasonDesc)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReasonDesc: " & ReasonDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionReason : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARDeductionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Deduction_Reason_Maint.aspx"

            UGNErrorTrapping.InsertErrorLog("GetARDeductionReason : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARDeductionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetARDeductionReason

    '*****
    '* Insert AR_Deduction_Reason
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function InsertARDeductionReason(ByVal ReasonDesc As String, ByVal DefaultNotify As String) As Boolean

        Try
            ReasonDesc = commonFunctions.convertSpecialChar(ReasonDesc, False)

            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As String = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

            ' Insert the AR_Deduction_Reason record
            Dim rowsAffected As Integer = Adapter5.sp_Insert_AR_Deduction_Reason(ReasonDesc, DefaultNotify, User, Date.Now)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReasonDesc: " & ReasonDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertARDeductionReason : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARDeductionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Deduction_Reason_Maint.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertARDeductionReason : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARDeductionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF InsertARDeductionReason

    '*****
    '* Update AR_Deduction_Reason
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function UpdateARDeductionReason(ByVal ReasonDesc As String, ByVal DefaultNotify As String, ByVal Obsolete As Boolean, ByVal original_RID As Integer) As Boolean

        Try
            original_RID = IIf(original_RID = Nothing, 0, original_RID)
            ReasonDesc = commonFunctions.convertSpecialChar(ReasonDesc, False)

            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As String = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

            ' Update the AR_Deduction_Reason record
            Dim rowsAffected As Integer = Adapter5.sp_Update_AR_Deduction_Reason(original_RID, ReasonDesc, DefaultNotify, Obsolete, User, Date.Now)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReasonDesc: " & ReasonDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateARDeductionReason : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARDeductionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Deduction_Reason_Maint.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateARDeductionReason : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARDeductionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateARDeductionReason
End Class

