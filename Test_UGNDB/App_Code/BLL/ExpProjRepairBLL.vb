''******************************************************************************************************
''* ExpProjRepairBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 11/22/2010
''* Modified: 10/24/2012    LRey    Consolidated all Repair BLL's into this one file.
''******************************************************************************************************

Imports ExpProjRepairTableAdapters
<System.ComponentModel.DataObject()> _
Public Class ExpProjRepairBLL
#Region "Adapters"
    Private pAdapter1 As ExpProj_Repair_Expenditure_TableAdapter = Nothing
    Private pAdapter2 As ExpProj_Repair_Documents_TableAdapter = Nothing
    Private pAdapter3 As ExpProj_Repair_Approval_TableAdapter = Nothing
    Private pAdapter4 As ExpProj_Repair_RSS_TableAdapter = Nothing
    Private pAdapter5 As ExpProj_Repair_RSS_Reply_TableAdapter = Nothing

    Protected ReadOnly Property Adapter1() As ExpProjRepairTableAdapters.ExpProj_Repair_Expenditure_TableAdapter
        Get
            If pAdapter1 Is Nothing Then
                pAdapter1 = New ExpProj_Repair_Expenditure_TableAdapter()
            End If
            Return pAdapter1
        End Get
    End Property 'EOF ExpProj_Repair_Expenditure_TableAdapter

    Protected ReadOnly Property Adapter2() As ExpProjRepairTableAdapters.ExpProj_Repair_Documents_TableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New ExpProj_Repair_Documents_TableAdapter()
            End If
            Return pAdapter2
        End Get
    End Property 'EOF ExpProj_Repair_Documents_TableAdapter

    Protected ReadOnly Property Adapter3() As ExpProjRepairTableAdapters.ExpProj_Repair_Approval_TableAdapter
        Get
            If pAdapter3 Is Nothing Then
                pAdapter3 = New ExpProj_Repair_Approval_TableAdapter()
            End If
            Return pAdapter3
        End Get
    End Property 'EOF ExpProj_Repair_Approval_TableAdapter

    Protected ReadOnly Property Adapter4() As ExpProjRepairTableAdapters.ExpProj_Repair_RSS_TableAdapter
        Get
            If pAdapter4 Is Nothing Then
                pAdapter4 = New ExpProj_Repair_RSS_TableAdapter()
            End If
            Return pAdapter4
        End Get
    End Property 'EOF ExpProj_Repair_RSS_TableAdapter

    Protected ReadOnly Property Adapter5() As ExpProjRepairTableAdapters.ExpProj_Repair_RSS_Reply_TableAdapter
        Get
            If pAdapter5 Is Nothing Then
                pAdapter5 = New ExpProj_Repair_RSS_Reply_TableAdapter()
            End If
            Return pAdapter5
        End Get
    End Property 'EOF ExpProj_Repair_RSS_Reply_TableAdapter
#End Region 'EOF "Adapters"

#Region "ExpProj_Repair_Expenditure"
    ''*****
    ''* Select ExpProj_Repair_Expenditure returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjRepairExpenditure(ByVal ProjectNo As String, ByVal EID As Integer) As ExpProjRepair.ExpProj_Repair_ExpenditureDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If EID = Nothing Then EID = 0

            Return Adapter1.Get_ExpProj_Repair_Expenditure(ProjectNo, 0)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjRepairExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjRepairBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/RepairExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjRepairExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjRepairBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjRepairExpenditure

    ''*****
    ''* Delete ExpProj_Repair_Expenditure
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjRepairExpenditure(ByVal ProjectNo As String, ByVal Original_EID As Integer, ByVal Original_ProjectNo As String) As Boolean

        Try

            Dim rowsAffected As Integer = Adapter1.sp_Delete_ExpProj_Repair_Expenditure(Original_EID, Original_ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & Original_EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjRepairExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjRepairBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/RepairExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjRepairExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjRepairBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF DeleteExpProjRepairExpenditure
#End Region 'EOF "ExpProj_Repair_Expenditure"

#Region "ExpProj_Repair_Documents"
    ''*****
    ''* Select ExpProj_Repair_Documents returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjRepairDocuments(ByVal ProjectNo As String, ByVal DocID As Integer) As ExpProjRepair.ExpProj_Repair_DocumentsDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If DocID = Nothing Then DocID = 0

            Return Adapter2.Get_ExpProj_Repair_Documents(ProjectNo, DocID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjRepairDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjRepairBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/RepairExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjRepairDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjRepairBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjRepairDocuments

    ''*****
    ''* Delete ExpProj_Repair_Documents
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjRepairDocuments(ByVal ProjectNo As String, ByVal Original_DocID As Integer, ByVal Original_ProjectNo As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter2.sp_Delete_ExpProj_Repair_Documents(Original_DocID, Original_ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & Original_DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjRepairDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjRepairBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/RepairExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjRepairDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjRepairBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteExpProjRepairDocuments
#End Region 'EOF "ExpProj_Repair_Documents"

#Region "ExpProj_Repair_Approval"
    ''*****
    ''* Select ExpProj_Repair_Approval returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjRepairApproval(ByVal ProjectNo As String, ByVal Sequence As Integer) As ExpProjRepair.ExpProj_Repair_ApprovalDataTable
        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If Sequence = Nothing Then
                Sequence = 0
            End If

            Return Adapter3.Get_ExpProj_Repair_Approval(ProjectNo, Sequence, 0, 0, 0)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", SeqNo: " & Sequence & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjRepairApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjRepairBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/RepairExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjRepairApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjRepairBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjRepairApproval
    '*****
    '* Insert ExpProj_Repair_Approval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function InsertExpProjRepairAddLvl1Aprvl(ByVal ProjectNo As String, ByVal SeqNo As Integer, ByVal ResponsibleTMID As Integer, ByVal OriginalTMID As Integer) As Boolean

        Try
            Dim pTable As ExpProjRepair.ExpProj_Repair_ApprovalDataTable = Adapter3.Get_ExpProj_Repair_Approval(ProjectNo, 0, 0, 0, 0)
            Dim pscpRow As ExpProjRepair.ExpProj_Repair_ApprovalRow = pTable(0)
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

            ' Insert the ExpProj_Repair_Approval record
            Dim rowsAffected As Integer = Adapter3.sp_Insert_ExpProj_Repair_AddLvl1Aprvl(ProjectNo, 1, ResponsibleTMID, OriginalTMID, User, Date.Today)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RespTMID: " & ResponsibleTMID & ", OrigTMID: " & OriginalTMID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertExpProjRepairAddLvl1Aprvl : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjRepairBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/RepairExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertExpProjRepairAddLvl1Aprvl : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjRepairBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF InsertExpProjRepairAddLvl1Aprvl

    '*****
    '* Update ExpProj_Repair_Approval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function UpdateExpProjRepairApproval(ByVal Status As String, ByVal Comments As String, ByVal SameTMID As Boolean, ByVal original_ProjectNo As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer, ByVal SeqNo As Integer, ByVal TeamMemberName As String, ByVal DateNotified As String) As Boolean

        Try
            Dim pTable As ExpProjRepair.ExpProj_Repair_ApprovalDataTable = Adapter3.Get_ExpProj_Repair_Approval(original_ProjectNo, 0, 0, 0, 0)
            Dim pscpRow As ExpProjRepair.ExpProj_Repair_ApprovalRow = pTable(0)
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
                ' Update the ExpProj_Repair_Approval record
                Dim rowsAffected As Integer = Adapter3.sp_Update_ExpProj_Repair_Approval(original_ProjectNo, original_TeamMemberID, TMSigned, Status, Comments, original_SeqNo, SameTMID, User, Date.Today)

                ' Return true if precisely one row was updated, otherwise false
                Return rowsAffected = 1
            Else
                Return False
            End If
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & original_ProjectNo & ", OrigTMID: " & original_TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjRepairApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjRepairBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/RepairExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjRepairApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjRepairBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateExpProjRepairApproval
    ''*****
    ''* Delete ExpProj_Repair_Approval
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjRepairApproval(ByVal ProjectNo As String, ByVal SeqNo As Integer, ByVal ResponsibleTMID As Integer, ByVal original_ProjectNo As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter3.sp_Delete_ExpProj_Repair_Approval(original_ProjectNo, original_SeqNo, original_TeamMemberID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function 'EOF DeleteExpProjRepairApproval

#End Region 'EOF ExpProj_Repair_Approval

#Region "ExpProj_Repair_RSS"
    ''*****
    ''* Select ExpProj_Repair_RSS returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjRepairRSS(ByVal ProjectNo As String, ByVal RSSID As Integer) As ExpProjRepair.ExpProj_Repair_RSSDataTable

        Try
            Return Adapter4.Get_ExpProj_Repair_RSS(ProjectNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjRepairRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjRepairBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/RepairExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjRepairRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjRepairBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjRepairRSS
#End Region 'EOF ExpProj_Repair_RSS

#Region "ExpProj_Repair_RSS_Reply"
    ''*****
    ''* Select ExpProj_Repair_RSS_Reply returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjRepairRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer) As ExpProjRepair.ExpProj_Repair_RSS_ReplyDataTable

        Try
            Return Adapter5.Get_ExpProj_Repair_RSS_Reply(ProjectNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetExpProjRepairRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjRepairBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/RepairExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjRepairRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjRepairBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjRepairRSSReply
#End Region 'EOF ExpProj_Repair_RSS_Reply

End Class

