''******************************************************************************************************
''* ExpProjPackagingBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: PackagingExpProj.aspx - gvCustomer
''* Author  : LRey 05/03/2011
''* Modified: 10/31/2012    LRey    Consolidated all Tooling BLL's into this one file.
''******************************************************************************************************
Imports ExpProjPackagingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class ExpProjPackagingBLL
#Region "Adapters"
    Private pAdapter1 As ExpProj_Packaging_Customer_TableAdapter = Nothing
    Private pAdapter2 As ExpProj_Packaging_Expenditure_TableAdapter = Nothing
    Private pAdapter3 As ExpProj_Packaging_Documents_TableAdapter = Nothing
    Private pAdapter4 As ExpProj_Packaging_Documents_TableAdapter = Nothing
    Private pAdapter5 As ExpProj_Packaging_Approval_TableAdapter = Nothing
    Private pAdapter6 As ExpProj_Packaging_RSS_TableAdapter = Nothing
    Private pAdapter7 As ExpProj_Packaging_RSS_Reply_TableAdapter = Nothing

    Protected ReadOnly Property Adapter1() As ExpProjPackagingTableAdapters.ExpProj_Packaging_Customer_TableAdapter
        Get
            If pAdapter1 Is Nothing Then
                pAdapter1 = New ExpProj_Packaging_Customer_TableAdapter()
            End If
            Return pAdapter1
        End Get
    End Property 'EOF ExpProj_Packaging_Customer_TableAdapter

    Protected ReadOnly Property Adapter2() As ExpProj_Packaging_Expenditure_TableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New ExpProj_Packaging_Expenditure_TableAdapter()
            End If
            Return pAdapter2
        End Get
    End Property 'EOF ExpProj_Packaging_Expenditure_TableAdapter

    Protected ReadOnly Property Adapter3() As ExpProj_Packaging_Documents_TableAdapter
        Get
            If pAdapter3 Is Nothing Then
                pAdapter3 = New ExpProj_Packaging_Documents_TableAdapter()
            End If
            Return pAdapter3
        End Get
    End Property 'EOF ExpProj_Packaging_Documents_TableAdapter

    Protected ReadOnly Property Adapter4() As ExpProj_Packaging_Documents_TableAdapter
        Get
            If pAdapter4 Is Nothing Then
                pAdapter4 = New ExpProj_Packaging_Documents_TableAdapter()
            End If
            Return pAdapter4
        End Get
    End Property 'EOF ExpProj_Packaging_Documents_TableAdapter

    Protected ReadOnly Property Adapter5() As ExpProj_Packaging_Approval_TableAdapter
        Get
            If pAdapter5 Is Nothing Then
                pAdapter5 = New ExpProj_Packaging_Approval_TableAdapter()
            End If
            Return pAdapter5
        End Get
    End Property 'EOF ExpProj_Packaging_Approval_TableAdapter

    Protected ReadOnly Property Adapter6() As ExpProj_Packaging_RSS_TableAdapter
        Get
            If pAdapter6 Is Nothing Then
                pAdapter6 = New ExpProj_Packaging_RSS_TableAdapter()
            End If
            Return pAdapter6
        End Get
    End Property 'EOF ExpProj_Packaging_RSS_TableAdapter

    Protected ReadOnly Property Adapter7() As ExpProj_Packaging_RSS_Reply_TableAdapter
        Get
            If pAdapter7 Is Nothing Then
                pAdapter7 = New ExpProj_Packaging_RSS_Reply_TableAdapter()
            End If
            Return pAdapter7
        End Get
    End Property 'EOF ExpProj_Packaging_RSS_Reply_TableAdapter

#End Region 'EOF "Adapters"

#Region "ExpProj_Packaging_Customer"
    ''*****
    ''* Select ExpProj_Packaging_Customer returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjPackagingCustomer(ByVal ProjectNo As String, ByVal PCID As Integer) As ExpProjPackaging.ExpProj_Packaging_CustomerDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If PCID = Nothing Then PCID = 0

            Return Adapter1.Get_ExpProj_Packaging_Customer(ProjectNo, PCID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", PCID: " & PCID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjPackagingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/PackagingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjPackagingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjPackagingCustomer

    ''*****
    ''* Delete ExpProj_Packaging_Customer
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjPackagingCustomer(ByVal PCID As Integer, ByVal ProjectNo As String, ByVal ProgramID As Integer, ByVal PartNo As String, ByVal original_PCID As Integer, ByVal original_ProjectNo As String, ByVal original_ProgramID As Integer, ByVal original_PartNo As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter1.sp_Delete_ExpProj_Packaging_Customer(original_PCID, original_ProjectNo, original_ProgramID, original_PartNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", PCID: " & original_PCID & ", ProgramID: " & original_ProgramID & ", PartNo: " & original_PartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjPackagingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/PackagingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjPackagingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteExpProjPackagingCustomer
#End Region 'EOF "ExpProj_Packaging_Customer"

#Region "ExpProj_Packaging_Expenditure"
    ''*****
    ''* Select ExpProj_Packaging_Expenditure returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjPackagingExpenditure(ByVal ProjectNo As String, ByVal EID As Integer) As ExpProjPackaging.ExpProj_Packaging_ExpenditureDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If EID = Nothing Then EID = 0

            Return Adapter2.Get_ExpProj_Packaging_Expenditure(ProjectNo, EID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjPackagingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/PackagingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjPackagingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjPackagingExpenditure

    ''*****
    ''* Delete ExpProj_Packaging_Expenditure
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjPackagingExpenditure(ByVal ProjectNo As String, ByVal Original_EID As Integer, ByVal Original_ProjectNo As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter2.sp_Delete_ExpProj_Packaging_Expenditure(Original_EID, Original_ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & Original_EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjPackagingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/PackagingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjPackagingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF DeleteExpProjPackagingExpenditure
#End Region 'EOF "ExpProj_Packaging_Expenditure"

#Region "ExpProj_Packaging_Documents"
    ''*****
    ''* Select ExpProj_Packaging_Documents returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjPackagingDocuments(ByVal ProjectNo As String, ByVal DocID As Integer) As ExpProjPackaging.ExpProj_Packaging_DocumentsDataTable

        Try
            Return Adapter4.Get_ExpProj_Packaging_Documents(ProjectNo, DocID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjPackagingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/PackagingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjPackagingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjPackagingDocuments

    ''*****
    ''* Delete ExpProj_Packaging_Documents
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjPackagingDocuments(ByVal ProjectNo As String, ByVal Original_DocID As Integer, ByVal Original_ProjectNo As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter4.sp_Delete_ExpProj_Packaging_Documents(Original_DocID, Original_ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & Original_DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteExpProjPackagingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/PackagingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteExpProjPackagingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteExpProjPackagingDocuments
#End Region 'EOF "ExpProj_Packaging_Documents"

#Region "ExpProj_Packaging_Approval"
    ''*****
    ''* Select ExpProj_Packaging_Approval returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetExpProjPackagingApproval(ByVal ProjectNo As String, ByVal Sequence As Integer) As ExpProjPackaging.ExpProj_Packaging_ApprovalDataTable
        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If Sequence = Nothing Then
                Sequence = 0
            End If

            Return Adapter5.Get_ExpProj_Packaging_Approval(ProjectNo, Sequence, 0, 0, 0)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", SeqNo: " & Sequence & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjPackagingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/PackagingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjPackagingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjPackagingApproval

    '*****
    '* Update ExpProj_Packaging_Approval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function UpdateExpProjPackagingApproval(ByVal Status As String, ByVal Comments As String, ByVal original_ProjectNo As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer, ByVal DateNotified As String) As Boolean

        Try
            Dim pTable As ExpProjPackaging.ExpProj_Packaging_ApprovalDataTable = Adapter5.Get_ExpProj_Packaging_Approval(original_ProjectNo, 0, 0, 0, 0)
            Dim pscpRow As ExpProjPackaging.ExpProj_Packaging_ApprovalRow = pTable(0)
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


            If Status <> "Pending" And Comments <> "" Or Comments <> Nothing Then
                TMSigned = True
                Comments = commonFunctions.replaceSpecialChar(Comments, False)
                ' Update the ExpProj_Packaging_Approval record
                Dim rowsAffected As Integer = Adapter5.sp_Update_ExpProj_Packaging_Approval(original_ProjectNo, original_TeamMemberID, TMSigned, Status, Comments, original_SeqNo, User, Date.Now)

                ' Return true if precisely one row was updated, otherwise false
                Return rowsAffected = 1
            Else
                Return False
            End If
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & original_ProjectNo & ", OrigTMID: " & original_TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjPackagingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/PackagingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjPackagingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateExpProjPackagingApproval
    ''*****
    ''* Delete ExpProj_Packaging_Approval
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteExpProjPackagingApproval(ByVal ProjectNo As String, ByVal SeqNo As Integer, ByVal ResponsibleTMID As Integer, ByVal original_ProjectNo As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter5.sp_Delete_ExpProj_Packaging_Approval(original_ProjectNo, original_SeqNo, original_TeamMemberID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function

#End Region 'EOF "ExpProj_Packaging_Approval"

#Region "ExpProj_Packaging_RSS"
    ''*****
    ''* Select ExpProj_Packaging_RSS returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjPackagingRSS(ByVal ProjectNo As String, ByVal RSSID As Integer) As ExpProjPackaging.ExpProj_Packaging_RSSDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If RSSID = Nothing Then RSSID = 0

            Return Adapter6.Get_ExpProj_Packaging_RSS(ProjectNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjPackagingRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/PackagingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjPackagingRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjPackagingRSS
#End Region 'EOF "ExpProj_Packaging_RSS"

#Region "ExpProj_Packaging_RSS_Reply"
    ''*****
    ''* Select ExpProj_Packaging_RSS_Reply returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjPackagingRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer) As ExpProjPackaging.ExpProj_Packaging_RSS_ReplyDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If ProjectNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: ProjectNo is a required field.")
            End If

            If RSSID = Nothing Then RSSID = 0

            Return Adapter7.Get_ExpProj_Packaging_RSS_Reply(ProjectNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetExpProjPackagingRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjPackagingBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/PackagingExpProjList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjPackagingRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetExpProjPackagingRSSReply
#End Region 'EOF "ExpProj_Packaging_RSS_Reply"





End Class

