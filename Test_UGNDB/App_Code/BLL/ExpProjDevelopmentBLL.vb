''****************************************************************************************************
''* ExpProjDevelopmentBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 11/01/2011
''* Modified: 01/21/2013    LRey    Consolidated all Assets BLL's into this one file.
''****************************************************************************************************

Imports ExpProjDevelopmentTableAdapters
<System.ComponentModel.DataObject()> _
Public Class ExpProjDevelopmentBLL
#Region "Adapters"
    Private pAdapter1 As ExpProj_Development_Documents_TableAdapter = Nothing
    Private pAdapter2 As ExpProj_Development_RSS_TableAdapter = Nothing
    Private pAdapter3 As ExpProj_Development_RSS_Reply_TableAdapter = Nothing
    Private pAdapter4 As ExpProj_Development_Approval_TableAdapter = Nothing

    Protected ReadOnly Property Adapter1() As ExpProjDevelopmentTableAdapters.ExpProj_Development_Documents_TableAdapter
        Get
            If pAdapter1 Is Nothing Then
                pAdapter1 = New ExpProj_Development_Documents_TableAdapter()
            End If
            Return pAdapter1
        End Get
    End Property 'EOF ExpProj_Development_Documents_TableAdapter
    Protected ReadOnly Property Adapter2() As ExpProjDevelopmentTableAdapters.ExpProj_Development_RSS_TableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New ExpProj_Development_RSS_TableAdapter()
            End If
            Return pAdapter2
        End Get
    End Property 'EOF ExpProj_Development_RSS_TableAdapter
    Protected ReadOnly Property Adapter3() As ExpProjDevelopmentTableAdapters.ExpProj_Development_RSS_Reply_TableAdapter
        Get
            If pAdapter3 Is Nothing Then
                pAdapter3 = New ExpProj_Development_RSS_Reply_TableAdapter()
            End If
            Return pAdapter3
        End Get
    End Property 'EOF ExpProj_Development_RSS_Reply_TableAdapter
    Protected ReadOnly Property Adapter4() As ExpProjDevelopmentTableAdapters.ExpProj_Development_Approval_TableAdapter
        Get
            If pAdapter4 Is Nothing Then
                pAdapter4 = New ExpProj_Development_Approval_TableAdapter()
            End If
            Return pAdapter4
        End Get
    End Property 'EOF ExpProj_Development_Approval_TableAdapter
#End Region 'EOF "Adapters"

#Region "ExpProj_Development_Documents"
    ''*****
    ''* Select ExpProj_Development_Documents returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjDevelopmentDocuments(ByVal ProjectNo As String, ByVal DocID As Integer) As ExpProjDevelopment.ExpProj_Development_DocumentsDataTable

        Try
            If ProjectNo = Nothing Then ProjectNo = ""
            If DocID = Nothing Then DocID = 0

            Return Adapter1.Get_ExpProj_Development_Documents(ProjectNo, DocID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjDevelopmentDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjDevelopmentBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/DevelopmentExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjDevelopmentDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjDevelopmentBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjDevelopmentDocuments

    ''*****
    ''* Delete ExpProj_Development_Documents
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjDevelopmentDocuments(ByVal ProjectNo As String, ByVal Original_DocID As Integer, ByVal Original_ProjectNo As String) As Boolean

        Try
            If Original_ProjectNo = Nothing Then Original_ProjectNo = ""
            If Original_DocID = Nothing Then Original_DocID = 0

            Dim rowsAffected As Integer = Adapter1.sp_Delete_ExpProj_Development_Documents(Original_DocID, Original_ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & Original_DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjDevelopmentDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjDevelopmentBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/DevelopmentExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjDevelopmentDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjDevelopmentBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteExpProjDevelopmentDocuments
#End Region 'EOF "ExpProj_Development_Documents"

#Region "ExpProj_Development_RSS"
    ''*****
    ''* Select ExpProj_Development_RSS returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjDevelopmentRSS(ByVal ProjectNo As String, ByVal RSSID As Integer) As ExpProjDevelopment.ExpProj_Development_RSSDataTable

        Try
            If ProjectNo = Nothing Then ProjectNo = ""
            If RSSID = Nothing Then RSSID = 0

            Return Adapter2.Get_ExpProj_Development_RSS(ProjectNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjDevelopmentRSS : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjDevelopmentBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/DevelopmentExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjDevelopmentRSS : " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjDevelopmentBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjDevelopmentRSS
#End Region 'EOF "ExpProj_Development_RSS"

#Region "ExpProj_Development_RSS_Reply"
    ''*****
    ''* Select ExpProj_Development_RSS_Reply returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjDevelopmentRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer) As ExpProjDevelopment.ExpProj_Development_RSS_ReplyDataTable

        Try
            If ProjectNo = Nothing Then ProjectNo = ""
            If RSSID = Nothing Then RSSID = 0

            Return Adapter3.Get_ExpProj_Development_RSS_Reply(ProjectNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetExpProjDevelopmentRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjDevelopmentBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/DevelopmentExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjDevelopmentRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjDevelopmentBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjDevelopmentRSSReply
#End Region 'EOF ExpProj_Development_RSS_Reply

#Region "ExpProj_Development_Approval"
    ''*****
    ''* Select ExpProj_Development_Approval returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjDevelopmentApproval(ByVal ProjectNo As String, ByVal Sequence As Integer) As ExpProjDevelopment.ExpProj_Development_ApprovalDataTable
        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If Sequence = Nothing Then
                Sequence = 0
            End If

            Return Adapter4.Get_ExpProj_Development_Approval(ProjectNo, Sequence, 0, 0, 0)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", SeqNo: " & Sequence & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjDevelopmentApproval : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjDevelopmentBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/DevelopmentExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjDevelopmentApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjDevelopmentBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjDevelopmentApproval
    '*****
    '* Insert ExpProj_Development_Approval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function InsertExpProjDevelopmentAddLvl1Aprvl(ByVal ProjectNo As String, ByVal SeqNo As Integer, ByVal ResponsibleTMID As Integer, ByVal OriginalTMID As Integer) As Boolean

        Try
            Dim pTable As ExpProjDevelopment.ExpProj_Development_ApprovalDataTable = Adapter4.Get_ExpProj_Development_Approval(ProjectNo, 0, 0, 0, 0)
            Dim pscpRow As ExpProjDevelopment.ExpProj_Development_ApprovalRow = pTable(0)
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As String = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim TMSigned As Boolean = False

            If pTable.Count = 0 Then
                ' no matching record found, return false
                Return False
            End If

            ' Logical Rule - Cannot update a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            ' Insert the ExpProj_Development_Approval record
            Dim rowsAffected As Integer = Adapter4.sp_Insert_ExpProj_Development_AddLvl1Aprvl(ProjectNo, 1, ResponsibleTMID, OriginalTMID, User, Date.Today)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RespTMID: " & ResponsibleTMID & ", OrigTMID: " & OriginalTMID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertExpProjDevelopmentAddLvl1Aprvl : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjDevelopmentBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/DevelopmentExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertExpProjDevelopmentAddLvl1Aprvl : " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjDevelopmentBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF InsertExpProjDevelopmentAddLvl1Aprvl

    '*****
    '* Update ExpProj_Development_Approval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function UpdateExpProjDevelopmentApproval(ByVal Status As String, ByVal Comments As String, ByVal SameTMID As Boolean, ByVal original_ProjectNo As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer, ByVal TeamMemberName As String, ByVal SeqNo As Integer, ByVal DateNotified As String) As Boolean

        Try
            Dim pTable As ExpProjDevelopment.ExpProj_Development_ApprovalDataTable = Adapter4.Get_ExpProj_Development_Approval(original_ProjectNo, 0, 0, 0, 0)
            Dim pscpRow As ExpProjDevelopment.ExpProj_Development_ApprovalRow = pTable(0)
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As String = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim TMSigned As Boolean = False

            If pTable.Count = 0 Then
                ' no matching record found, return false
                Return False
            End If

            ' Logical Rule - Cannot update a record without null columns
            If original_ProjectNo = Nothing Then
                Throw New ApplicationException("Update Cancelled: ProjectNo is a required field.")
            End If
            If Status = Nothing Then
                Throw New ApplicationException("Update Cancelled: Status is a required field.")
            End If
            If Comments = Nothing And Status = "Rejected" Then
                Throw New ApplicationException("Update Cancelled: Comments is a required field.")
            End If

            Comments = commonFunctions.replaceSpecialChar(Comments, False)

            If Status <> "Pending" Then
                TMSigned = True
            End If
            If Comments <> "" Or Comments <> Nothing Then
                ' Update the ExpProj_Development_Approval record
                Dim rowsAffected As Integer = Adapter4.sp_Update_ExpProj_Development_Approval(original_ProjectNo, original_TeamMemberID, TMSigned, Status, Comments, original_SeqNo, SameTMID, User, Date.Today)

                ' Return true if precisely one row was updated, otherwise false
                Return rowsAffected = 1
            Else
                Return False
            End If
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & original_ProjectNo & ", OrigTMID: " & original_TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjDevelopmentApproval : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjDevelopmentBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/DevelopmentExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjDevelopmentApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjDevelopmentBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateExpProjDevelopmentApproval

    ''*****
    ''* Delete ExpProj_Development_Approval
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjDevelopmentApproval(ByVal ProjectNo As String, ByVal SeqNo As Integer, ByVal ResponsibleTMID As Integer, ByVal original_ProjectNo As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter4.sp_Delete_ExpProj_Development_Approval(original_ProjectNo, original_SeqNo, original_TeamMemberID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function
#End Region 'EOF "ExpProj_Development_Approval"

End Class

