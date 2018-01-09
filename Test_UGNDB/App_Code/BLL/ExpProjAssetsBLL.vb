''******************************************************************************************************
''* ExpProjAssetsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: AssetsExpProj.aspx - gvExpenses
''* Author  : LRey 03/02/2010
''* Modified: 10/24/2012    LRey    Consolidated all Assets BLL's into this one file.
''******************************************************************************************************
Imports ExpProjAssetsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class ExpProjAssetsBLL
#Region "Adapters"
    Private pAdapter1 As ExpProj_Assets_Expenditure_TableAdapter = Nothing
    Private pAdapter2 As ExpProj_Assets_Documents_TableAdapter = Nothing
    Private pAdapter3 As ExpProj_Assets_Approval_TableAdapter = Nothing
    Private pAdapter4 As ExpProj_Assets_RSS_TableAdapter = Nothing
    Private pAdapter5 As ExpProj_Assets_RSS_Reply_TableAdapter = Nothing

    Protected ReadOnly Property Adapter1() As ExpProjAssetsTableAdapters.ExpProj_Assets_Expenditure_TableAdapter
        Get
            If pAdapter1 Is Nothing Then
                pAdapter1 = New ExpProj_Assets_Expenditure_TableAdapter()
            End If
            Return pAdapter1
        End Get
    End Property 'EOF ExpProj_Assets_Expenditure_TableAdapter

    Protected ReadOnly Property Adapter2() As ExpProjAssetsTableAdapters.ExpProj_Assets_Documents_TableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New ExpProj_Assets_Documents_TableAdapter()
            End If
            Return pAdapter2
        End Get
    End Property 'EOF ExpProj_Assets_Documents_TableAdapter

    Protected ReadOnly Property Adapter3() As ExpProjAssetsTableAdapters.ExpProj_Assets_Approval_TableAdapter
        Get
            If pAdapter3 Is Nothing Then
                pAdapter3 = New ExpProj_Assets_Approval_TableAdapter()
            End If
            Return pAdapter3
        End Get
    End Property 'EOF ExpProj_Assets_Approval_TableAdapter

    Protected ReadOnly Property Adapter4() As ExpProjAssetsTableAdapters.ExpProj_Assets_RSS_TableAdapter
        Get
            If pAdapter4 Is Nothing Then
                pAdapter4 = New ExpProj_Assets_RSS_TableAdapter()
            End If
            Return pAdapter4
        End Get
    End Property 'EOF ExpProj_Assets_RSS_TableAdapter

    Protected ReadOnly Property Adapter5() As ExpProjAssetsTableAdapters.ExpProj_Assets_RSS_Reply_TableAdapter
        Get
            If pAdapter5 Is Nothing Then
                pAdapter5 = New ExpProj_Assets_RSS_Reply_TableAdapter()
            End If
            Return pAdapter5
        End Get
    End Property 'EOF ExpProj_Assets_RSS_Reply_TableAdapter

#End Region 'EOF "Adapters"

#Region "ExpProj_Assets_Expenditure"
    ''*****
    ''* Select ExpProj_Assets_Expenditure returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjAssetsExpenditure(ByVal ProjectNo As String, ByVal EID As Integer) As ExpProjAssets.ExpProj_Assets_ExpenditureDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If EID = Nothing Then EID = 0

            Return Adapter1.Get_ExpProj_Assets_Expenditure(ProjectNo, EID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjAssetsExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjAssetsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/AssetsExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjAssetsExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjAssetsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjAssetsExpenditure

    ''*****
    ''* Delete ExpProj_Assets_Expenditure
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjAssetsExpenditure(ByVal ProjectNo As String, ByVal Original_EID As Integer, ByVal Original_ProjectNo As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter1.sp_Delete_ExpProj_Assets_Expenditure(Original_EID, Original_ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & Original_EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjAssetsExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjAssetsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/AssetsExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjAssetsExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjAssetsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF DeleteExpProjAssetsExpenditure
#End Region 'EOF "ExpProj_Assets_Expenditure"

#Region "ExpProj_Assets_Documents"
    ''*****
    ''* Select ExpProj_Assets_Documents returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjAssetsDocuments(ByVal ProjectNo As String, ByVal DocID As Integer) As ExpProjAssets.ExpProj_Assets_DocumentsDataTable

        Try
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If DocID = Nothing Then DocID = 0

            Return Adapter2.Get_ExpProj_Assets_Documents(ProjectNo, DocID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjAssetsDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjAssetsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/AssetsExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjAssetsDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjAssetsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjAssetsDocuments

    ''*****
    ''* Delete ExpProj_Assets_Documents
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjAssetsDocuments(ByVal ProjectNo As String, ByVal Original_DocID As Integer, ByVal Original_ProjectNo As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter2.sp_Delete_ExpProj_Assets_Documents(Original_DocID, Original_ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & Original_DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjAssetsDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjAssetsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/AssetsExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjAssetsDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjAssetsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteExpProjAssetsDocuments
#End Region 'EOF "ExpProj_Assets_Documents"

#Region "ExpProj_Assets_Approval"
    ''*****
    ''* Select ExpProj_Assets_Approval returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjAssetsApproval(ByVal ProjectNo As String, ByVal Sequence As Integer) As ExpProjAssets.ExpProj_Assets_ApprovalDataTable
        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If Sequence = Nothing Then Sequence = 0

            Return Adapter3.Get_ExpProj_Assets_Approval(ProjectNo, Sequence, 0, 0, 0)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", SeqNo: " & Sequence & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjAssetsApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjAssetsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/AssetsExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjAssetsApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjAssetsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjAssetsApproval
    '*****
    '* Insert ExpProj_Assets_Approval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function InsertExpProjAssetsAddLvl1Aprvl(ByVal ProjectNo As String, ByVal SeqNo As Integer, ByVal ResponsibleTMID As Integer, ByVal OriginalTMID As Integer) As Boolean

        Try
            Dim pTable As ExpProjAssets.ExpProj_Assets_ApprovalDataTable = Adapter3.Get_ExpProj_Assets_Approval(ProjectNo, 0, 0, 0, 0)
            Dim pscpRow As ExpProjAssets.ExpProj_Assets_ApprovalRow = pTable(0)
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

            ' Insert the ExpProj_Assets_Approval record
            Dim rowsAffected As Integer = Adapter3.sp_Insert_ExpProj_Assets_AddLvl1Aprvl(ProjectNo, 1, ResponsibleTMID, OriginalTMID, User, Date.Today)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RespTMID: " & ResponsibleTMID & ", OrigTMID: " & OriginalTMID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertExpProjAssetsAddLvl1Aprvl : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjAssetsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/AssetsExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertExpProjAssetsAddLvl1Aprvl : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjAssetsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF InsertExpProjAssetsAddLvl1Aprvl

    '*****
    '* Update ExpProj_Assets_Approval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function UpdateExpProjAssetsApproval(ByVal Status As String, ByVal Comments As String, ByVal SameTMID As Boolean, ByVal original_ProjectNo As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer, ByVal TeamMemberName As String, ByVal SeqNo As Integer, ByVal DateNotified As String) As Boolean

        Try
            Dim pTable As ExpProjAssets.ExpProj_Assets_ApprovalDataTable = Adapter3.Get_ExpProj_Assets_Approval(original_ProjectNo, 0, 0, 0, 0)
            Dim pscpRow As ExpProjAssets.ExpProj_Assets_ApprovalRow = pTable(0)
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
                ' Update the ExpProj_Assets_Approval record
                Dim rowsAffected As Integer = Adapter3.sp_Update_ExpProj_Assets_Approval(original_ProjectNo, original_TeamMemberID, TMSigned, Status, Comments, original_SeqNo, SameTMID, User, Date.Today)

                ' Return true if precisely one row was updated, otherwise false
                Return rowsAffected = 1
            Else
                Return False
            End If
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & original_ProjectNo & ", OrigTMID: " & original_TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjAssetsApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjAssetsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/AssetsExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjAssetsApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjAssetsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateExpProjAssetsApproval

    ''*****
    ''* Delete ExpProj_Assets_Approval
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjAssetsApproval(ByVal ProjectNo As String, ByVal SeqNo As Integer, ByVal ResponsibleTMID As Integer, ByVal original_ProjectNo As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter3.sp_Delete_ExpProj_Assets_Approval(original_ProjectNo, original_SeqNo, original_TeamMemberID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function 'EOF DeleteExpProjAssetsApproval

#End Region 'EOF "ExpProj_Assets_Approval"

#Region "ExpProj_Assets_RSS"
    ''*****
    ''* Select ExpProj_Assets_RSS returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjAssetsRSS(ByVal ProjectNo As String, ByVal RSSID As Integer) As ExpProjAssets.ExpProj_Assets_RSSDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If RSSID = Nothing Then RSSID = 0

            Return Adapter4.Get_ExpProj_Assets_RSS(ProjectNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjAssetsRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjAssetsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/AssetsExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjAssetsRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjAssetsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjAssetsRSS
#End Region 'EOF "ExpProj_Assets_RSS"

#Region "ExpProj_Assets_RSS_Reply"
    ''*****
    ''* Select ExpProj_Assets_RSS_Reply returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjAssetsRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer) As ExpProjAssets.ExpProj_Assets_RSS_ReplyDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If RSSID = Nothing Then RSSID = 0

            Return Adapter5.Get_ExpProj_Assets_RSS_Reply(ProjectNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetExpProjAssetsRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjAssetsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/AssetsExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjAssetsRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjAssetsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjAssetsRSSReply
#End Region 'EOF "ExpProj_Assets_RSS_Reply"

End Class

