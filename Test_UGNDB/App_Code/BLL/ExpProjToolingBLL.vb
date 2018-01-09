''******************************************************************************************************
''* ExpProjToolingBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: ToolingExpProj.aspx - gvCustomer
''* Author  : LRey 09/02/2009
''* Modified: 10/29/2012    LRey    Consolidated all Tooling BLL's into this one file.
''******************************************************************************************************
Imports ExpProjTableAdapters

<System.ComponentModel.DataObject()> _
Public Class ExpProjToolingBLL
#Region "Adapters"
    Private pAdapter1 As ExpProj_Tooling_Customer_TableAdapter = Nothing
    Private pAdapter2 As ExpProj_Tooling_Expenditure_TableAdapter = Nothing
    Private pAdapter3 As ExpProj_Tooling_Documents_TableAdapter = Nothing
    Private pAdapter4 As ExpProj_Tooling_Approval_TableAdapter = Nothing
    Private pAdapter5 As ExpProj_Tooling_RSS_TableAdapter = Nothing
    Private pAdapter6 As ExpProj_Tooling_RSS_Reply_TableAdapter = Nothing
    Private pAdapter7 As ExpProj_Tooling_Yearly_Volume_TableAdapter = Nothing
    Private pAdapter8 As ExpProj_Tooling_Customer_EIOR_TableAdapter = Nothing

    Protected ReadOnly Property Adapter1() As ExpProjTableAdapters.ExpProj_Tooling_Customer_TableAdapter
        Get
            If pAdapter1 Is Nothing Then
                pAdapter1 = New ExpProj_Tooling_Customer_TableAdapter()
            End If
            Return pAdapter1
        End Get
    End Property 'EOF ExpProj_Tooling_Customer_TableAdapter

    Protected ReadOnly Property Adapter2() As ExpProjTableAdapters.ExpProj_Tooling_Expenditure_TableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New ExpProj_Tooling_Expenditure_TableAdapter()
            End If
            Return pAdapter2
        End Get
    End Property 'EOF ExpProj_Tooling_Expenditure_TableAdapter

    Protected ReadOnly Property Adapter3() As ExpProjTableAdapters.ExpProj_Tooling_Documents_TableAdapter
        Get
            If pAdapter3 Is Nothing Then
                pAdapter3 = New ExpProj_Tooling_Documents_TableAdapter()
            End If
            Return pAdapter3
        End Get
    End Property 'EOF ExpProj_Tooling_Documents_TableAdapter

    Protected ReadOnly Property Adapter4() As ExpProjTableAdapters.ExpProj_Tooling_Approval_TableAdapter
        Get
            If pAdapter4 Is Nothing Then
                pAdapter4 = New ExpProj_Tooling_Approval_TableAdapter()
            End If
            Return pAdapter4
        End Get
    End Property 'EOF ExpProj_Tooling_Approval_TableAdapter

    Protected ReadOnly Property Adapter5() As ExpProjTableAdapters.ExpProj_Tooling_RSS_TableAdapter
        Get
            If pAdapter5 Is Nothing Then
                pAdapter5 = New ExpProj_Tooling_RSS_TableAdapter()
            End If
            Return pAdapter5
        End Get
    End Property 'EOF ExpProj_Tooling_RSS_TableAdapter

    Protected ReadOnly Property Adapter6() As ExpProjTableAdapters.ExpProj_Tooling_RSS_Reply_TableAdapter
        Get
            If pAdapter6 Is Nothing Then
                pAdapter6 = New ExpProj_Tooling_RSS_Reply_TableAdapter()
            End If
            Return pAdapter6
        End Get
    End Property 'EOF ExpProj_Tooling_RSS_Reply_TableAdapter

    Protected ReadOnly Property Adapter7() As ExpProjTableAdapters.ExpProj_Tooling_Yearly_Volume_TableAdapter
        Get
            If pAdapter7 Is Nothing Then
                pAdapter7 = New ExpProj_Tooling_Yearly_Volume_TableAdapter()
            End If
            Return pAdapter7
        End Get
    End Property 'EOF ExpProj_Tooling_Yearly_Volume_TableAdapter

    Protected ReadOnly Property Adapter8() As ExpProjTableAdapters.ExpProj_Tooling_Customer_EIOR_TableAdapter
        Get
            If pAdapter8 Is Nothing Then
                pAdapter8 = New ExpProj_Tooling_Customer_EIOR_TableAdapter()
            End If
            Return pAdapter8
        End Get
    End Property 'EOF ExpProj_Tooling_Customer_EIOR_TableAdapter

#End Region 'EOF "Adapters"

#Region "ExpProj_Tooling_Customer"
    ''*****
    ''* Select ExpProj_Tooling_Customer returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjToolingCustomer(ByVal ProjectNo As String, ByVal TCID As Integer) As ExpProj.ExpProj_Tooling_CustomerDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If TCID = Nothing Then TCID = 0

            Return Adapter1.Get_ExpProj_Tooling_Customer(ProjectNo, TCID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", TCID: " & TCID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjToolingCustomer

    ''*****
    ''* Delete ExpProj_Tooling_Customer
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjToolingCustomer(ByVal TCID As Integer, ByVal ProjectNo As String, ByVal ProgramID As Integer, ByVal PartNo As String, ByVal original_TCID As Integer, ByVal original_ProjectNo As String, ByVal original_ProgramID As Integer, ByVal original_PartNo As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter1.sp_Delete_ExpProj_Tooling_Customer(original_TCID, original_ProjectNo, original_ProgramID, original_PartNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", TCID: " & original_TCID & ", ProgramID: " & original_ProgramID & ", PartNo: " & original_PartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjToolingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjToolingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteExpProjToolingCustomer

    ''*****
    ''* Select ExpProj_Tooling_Customer_EIOR returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjToolingCustomerEIOR(ByVal ProjectNo As String) As ExpProj.ExpProj_Tooling_Customer_EIORDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            'If ProjectNo = Nothing Then
            '    Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            'End If

            Dim pProjNo As String = Nothing
            If HttpContext.Current.Request.QueryString("pProjNo") <> "" Then
                pProjNo = HttpContext.Current.Request.QueryString("pProjNo").Substring(0, 1)
            End If
            If pProjNo <> "T" Then
                Return Nothing
            Else
                Return Adapter8.Get_ExpProj_Tooling_Customer_EIOR(ProjectNo)
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingCustomerEIOR : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingCustomerEIOR : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjToolingCustomer
    ''*****
    ''* Update ExpProj_Tooling_Customer_EIOR
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateExpProjToolingCustomerEIOR(ByVal RevisionLevel As String, ByVal original_ProjectNo As String, ByVal original_PartNo As String, ByVal original_RevisionLevel As String) As Boolean

        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim UserID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
        Try
            Dim b As String = original_ProjectNo
            Dim f As String = original_PartNo
            Dim r As String = original_RevisionLevel
            Dim h As String = RevisionLevel

            ''Used to define the primary record
            Dim IORNo As Integer = 0
            If HttpContext.Current.Request.QueryString("pIORNo") <> "" Then
                IORNo = HttpContext.Current.Request.QueryString("pIORNo")
            End If

            Dim rowsAffected As Integer = Adapter8.sp_Update_ExpProj_Tooling_Customer_EIOR(original_ProjectNo, original_PartNo, RevisionLevel, original_RevisionLevel, User)

            ''*****************
            ''History Tracking when RevisionLevel is different
            ''*****************
            If original_RevisionLevel <> RevisionLevel Then
                EXPModule.InsertExpProjToolingHistory(original_ProjectNo, "", UserID, "Revision Level was updated from '" & original_RevisionLevel & "' to '" & RevisionLevel & "' for Part Number '" & original_PartNo & "'.")
                PURModule.InsertInternalOrderRequestHistory(IORNo, "", UserID, "Revision Level was updated from '" & original_RevisionLevel & "' to '" & RevisionLevel & "' for Part Number '" & original_PartNo & "'.")
            End If

            ' Return true if precisely one row was Updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & original_ProjectNo & ", User: " & User

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjToolingCustomerEIOR : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjToolingCustomerEIOR : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateExpProjToolingCustomerEIOR

#End Region 'EOF "ExpProj_Tooling_Customer"

#Region "ExpProj_Tooling_Expenditure"
    ''*****
    ''* Select ExpProj_Tooling_Expenditure returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjToolingExpenditure(ByVal ProjectNo As String, ByVal EID As Integer) As ExpProj.ExpProj_Tooling_ExpenditureDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If EID = Nothing Then EID = 0

            Return Adapter2.Get_ExpProj_Tooling_Expenditure(ProjectNo, EID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjToolingExpenditure

    ''*****
    ''* Delete ExpProj_Tooling_Expenditure
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjToolingExpenditure(ByVal ProjectNo As String, ByVal Original_EID As Integer, ByVal Original_ProjectNo As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter2.sp_Delete_ExpProj_Tooling_Expenditure(Original_EID, Original_ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & Original_EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjToolingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjToolingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF DeleteExpProjToolingExpenditure
#End Region 'EOF "ExpProj_Tooling_Expenditure"

#Region "ExpProj_Tooling_Documents"
    ''*****
    ''* Select ExpProj_Tooling_Documents returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjToolingDocuments(ByVal ProjectNo As String, ByVal DocID As Integer) As ExpProj.ExpProj_Tooling_DocumentsDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If DocID = Nothing Then DocID = 0

            Return Adapter3.Get_ExpProj_Tooling_Documents(ProjectNo, DocID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjToolingDocuments
    ''*****
    ''* Delete ExpProj_Tooling_Documents
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjToolingDocuments(ByVal ProjectNo As String, ByVal Original_DocID As Integer, ByVal Original_ProjectNo As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter3.sp_Delete_ExpProj_Tooling_Documents(Original_DocID, Original_ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & Original_DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjToolingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjToolingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteExpProjToolingDocuments
#End Region 'EOF "ExpProj_Tooling_Documents"

#Region "ExpProj_Tooling_Approval"
    ''*****
    ''* Select ExpProj_Tooling_Approval returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjToolingApproval(ByVal ProjectNo As String, ByVal Sequence As Integer) As ExpProj.ExpProj_Tooling_ApprovalDataTable
        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If Sequence = Nothing Then Sequence = 0

            Return Adapter4.Get_ExpProj_Tooling_Approval(ProjectNo, Sequence, 0, 0, 0)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", SeqNo: " & Sequence & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjToolingApproval

    '*****
    '* Update ExpProj_Tooling_Approval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function UpdateExpProjToolingApproval(ByVal Status As String, ByVal Comments As String, ByVal original_ProjectNo As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer, ByVal SeqNo As Integer, ByVal DateNotified As String) As Boolean

        Try
            Dim pTable As ExpProj.ExpProj_Tooling_ApprovalDataTable = Adapter4.Get_ExpProj_Tooling_Approval(original_ProjectNo, 0, 0, 0, 0)
            Dim pscpRow As ExpProj.ExpProj_Tooling_ApprovalRow = pTable(0)
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
                ' Update the ExpProj_Tooling_Approval record
                Dim rowsAffected As Integer = Adapter4.sp_Update_ExpProj_Tooling_Approval(original_ProjectNo, original_TeamMemberID, TMSigned, Status, Comments, original_SeqNo, User, Date.Today)

                ' Return true if precisely one row was updated, otherwise false
                Return rowsAffected = 1
            Else
                Return False
            End If
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & original_ProjectNo & ", OrigTMID: " & original_TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjToolingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjToolingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateExpProjToolingApproval
#End Region 'EOF "ExpProj_Tooling_Approval"

#Region "ExpProj_Tooling_RSS"
    ''*****
    ''* Select ExpProj_Tooling_RSS returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjToolingRSS(ByVal ProjectNo As String, ByVal RSSID As Integer) As ExpProj.ExpProj_Tooling_RSSDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If RSSID = Nothing Then RSSID = 0

            Return Adapter5.Get_ExpProj_Tooling_RSS(ProjectNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjToolingRSS
#End Region 'EOF "ExpProj_Tooling_RSS"

#Region "ExpProj_Tooling_RSS_Reply"
    ''*****
    ''* Select ExpProj_Tooling_RSS_Reply returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjToolingRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer) As ExpProj.ExpProj_Tooling_RSS_ReplyDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If RSSID = Nothing Then RSSID = 0

            Return Adapter6.Get_ExpProj_Tooling_RSS_Reply(ProjectNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjToolingRSSReply
#End Region 'EOF "ExpProj_Tooling_RSS_Reply"

#Region "ExpProj_Tooling_Yearly_Volume"
    ''*****
    ''* Select ExpProj_Tooling_Yearly_Volume returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjToolingYearlyVolume(ByVal ProjectNo As String) As ExpProj.ExpProj_Tooling_Yearly_VolumeDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            Return Adapter7.Get_ExpProj_Tooling_Yearly_Volume(ProjectNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingYearlyVolume : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingYearlyVolume : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjToolingYearlyVolume
    ''*****
    ''* Insert a New row to ExpProj_Tooling_Yearly_Volume table
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertExpProjToolingYearlyVolume(ByVal ProjectNo As String, ByVal Year As Integer, ByVal Volume As Integer) As Boolean

        Try
            ' Create a new pscpRow instance
            Dim pTable As New ExpProj.ExpProj_Tooling_Yearly_VolumeDataTable
            Dim pscpRow As ExpProj.ExpProj_Tooling_Yearly_VolumeRow = pTable.NewExpProj_Tooling_Yearly_VolumeRow
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If
            If Year = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Year is a required field.")
            End If
            If Volume = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Volume is a required field.")
            End If


            ' Insert the new ExpProj_Tooling_Yearly_Volume row
            Dim rowsAffected As Integer = Adapter7.sp_Insert_ExpProj_Tooling_Yearly_Volume(ProjectNo, Year, Volume, User)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertExpProjToolingYearlyVolume : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertExpProjToolingYearlyVolume : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF InsertExpProjToolingYearlyVolume

    ''*****
    ''* Update ExpProj_Tooling_Yearly_Volume
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function UpdateExpProjToolingYearlyVolume(ByVal ProjectNo As String, ByVal Year As Integer, ByVal Volume As Integer, ByVal Original_Year As Integer, ByVal Original_Volume As Integer, ByVal original_YVID As Integer) As Boolean

        Try
            Dim pTable As ExpProj.ExpProj_Tooling_Yearly_VolumeDataTable = Adapter7.Get_ExpProj_Tooling_Yearly_Volume(ProjectNo)
            Dim pscpRow As ExpProj.ExpProj_Tooling_Yearly_VolumeRow = pTable(0)
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If pTable.Count = 0 Then
                ' no matching record found, return false
                Return False
            End If

            ' Logical Rule - Cannot update a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If
            If Year = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Year is a required field.")
            End If
            If Volume = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Volume is a required field.")
            End If

            ' Update the ExpProj_Tooling_Yearly_Volume record
            Dim rowsAffected As Integer = Adapter7.sp_Update_ExpProj_Tooling_Yearly_Volume(original_YVID, ProjectNo, Year, Volume, User, Original_Year, Original_Volume)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjToolingYearlyVolume : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjToolingYearlyVolume : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateExpProjToolingYearlyVolume
    ''*****
    ''* Delete ExpProj_Tooling_Yearly_Volume
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjToolingYearlyVolume(ByVal YVID As Integer, ByVal ProjectNo As String, ByVal original_Year As Integer, ByVal original_Volume As Integer, ByVal original_YVID As Integer, ByVal original_ProjectNo As String) As Boolean
        Try
            Dim rowsAffected As Integer = Adapter7.sp_Delete_ExpProj_Tooling_Yearly_Volume(original_YVID, ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", YVID: " & original_YVID & ", Year: " & original_Year & ", Volume: " & original_Volume & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjToolingYearlyVolume : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjToolingYearlyVolume : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjToolingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteExpProjToolingYearlyVolume
#End Region 'EOF ExpProj_Tooling_Yearly_Volume
End Class

