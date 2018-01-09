''******************************************************************************************************
''* InternalOrderRequestBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: InternalOrderRequest.aspx - gvExpenses
''* Author  : LRey 08/27/2010
''******************************************************************************************************
Imports IORTableAdapters

<System.ComponentModel.DataObject()> _
Public Class InternalOrderRequestBLL
#Region "Adapters"
    Private pAdapter1 As Internal_Order_Request_Expenditure_TableAdapter = Nothing
    Private pAdapter2 As IOR_by_ExpProj_TableAdapter = Nothing
    Private pAdapter3 As Internal_Order_Request_Approval_TableAdapter = Nothing
    Private pAdapter4 As Internal_Order_Request_Documents_TableAdapter = Nothing
    Private pAdapter5 As Internal_Order_Request_RSS_TableAdapter = Nothing
    Private pAdapter6 As Internal_Order_Request_RSS_Reply_TableAdapter = Nothing

    Protected ReadOnly Property Adapter1() As IORTableAdapters.Internal_Order_Request_Expenditure_TableAdapter
        Get
            If pAdapter1 Is Nothing Then
                pAdapter1 = New Internal_Order_Request_Expenditure_TableAdapter()
            End If
            Return pAdapter1
        End Get
    End Property 'EOF Internal_Order_Request_Expenditure_TableAdapter

    Protected ReadOnly Property Adapter2() As IORTableAdapters.IOR_by_ExpProj_TableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New IOR_by_ExpProj_TableAdapter()
            End If
            Return pAdapter2
        End Get
    End Property 'EOF IOR_by_ExpProj_TableAdapter

    Protected ReadOnly Property Adapter3() As IORTableAdapters.Internal_Order_Request_Approval_TableAdapter
        Get
            If pAdapter3 Is Nothing Then
                pAdapter3 = New Internal_Order_Request_Approval_TableAdapter()
            End If
            Return pAdapter3
        End Get
    End Property 'EOF Internal_Order_Request_Approval_TableAdapter

    Protected ReadOnly Property Adapter4() As IORTableAdapters.Internal_Order_Request_Documents_TableAdapter
        Get
            If pAdapter4 Is Nothing Then
                pAdapter4 = New Internal_Order_Request_Documents_TableAdapter()
            End If
            Return pAdapter4
        End Get
    End Property 'EOF Internal_Order_Request_Documents_TableAdapter

    Protected ReadOnly Property Adapter5() As IORTableAdapters.Internal_Order_Request_RSS_TableAdapter
        Get
            If pAdapter5 Is Nothing Then
                pAdapter5 = New Internal_Order_Request_RSS_TableAdapter()
            End If
            Return pAdapter5
        End Get
    End Property 'EOF Internal_Order_Request_RSS_TableAdapter

    Protected ReadOnly Property Adapter6() As IORTableAdapters.Internal_Order_Request_RSS_Reply_TableAdapter
        Get
            If pAdapter6 Is Nothing Then
                pAdapter6 = New Internal_Order_Request_RSS_Reply_TableAdapter()
            End If
            Return pAdapter6
        End Get
    End Property 'EOF Internal_Order_Request_RSS_Reply_TableAdapter
#End Region 'EOF "Adapters"

#Region "Internal_Order_Request_Expenditure"
    ''*****
    ''* Select Internal_Order_Request_Expenditure returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetInternalOrderRequestExpenditure(ByVal IORNO As Integer, ByVal EID As Integer) As IOR.Internal_Order_Request_ExpenditureDataTable

        Try

            Return Adapter1.Get_Internal_Order_Request_Expenditure(IORNO, 0)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", EID: " & EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetInternalOrderRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> InternalOrderRequestBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "InternalOrderRequestBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetInternalOrderRequestExpenditure

    ''*****
    ''* Delete Internal_Order_Request_Expenditure
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteInternalOrderRequestExpenditure(ByVal IORNO As Integer, ByVal Original_EID As Integer, ByVal Original_IORNO As Integer) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter1.sp_Delete_Internal_Order_Request_Expenditure(Original_EID, Original_IORNO)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", EID: " & Original_EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteInternalOrderRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> InternalOrderRequestBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteInternalOrderRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "InternalOrderRequestBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF DeleteInternalOrderRequestExpenditure
#End Region 'EOF "Internal_Order_Request_Expenditure"

#Region "GetIORbyExpProj"
    ''*****
    ''* Select Internal_Order_Request_Expenditure returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetIORbyExpProj(ByVal AppropriationCode As String) As IOR.IOR_by_ExpProjDataTable

        Try

            Return Adapter2.GetIORbyExpProj(AppropriationCode)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AppropriationCode: " & AppropriationCode & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetIORbyExpProj : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> InternalOrderRequestBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("GetIORbyExpProj : " & commonFunctions.replaceSpecialChar(ex.Message, False), "InternalOrderRequestBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetInternalOrderRequestExpenditure
#End Region 'EOF GetIORbyExpProj

#Region "Internal_Order_Request_Approval"

    ''*****
    ''* Select Internal_Order_Request_Approval returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetInternalOrderRequestApproval(ByVal IORNO As String, ByVal Sequence As Integer) As IOR.Internal_Order_Request_ApprovalDataTable
        Try
            ' Logical Rule - Cannot insert a record without null columns
            If IORNO = Nothing Then
                Throw New ApplicationException("Insert Cancelled: IORNO is a required field.")
            End If

            If Sequence = Nothing Then
                Sequence = 0
            End If

            Return Adapter3.Get_Internal_Order_Request_Approval(IORNO, Sequence, 0, 0, 0)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", SeqNo: " & Sequence & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetInternalOrderRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> InternalOrderRequestBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/IOR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "InternalOrderRequestBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetInternalOrderRequestApproval

    '*****
    '* Update Internal_Order_Request_Approval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function UpdateInternalOrderRequestApproval(ByVal Status As String, ByVal Comments As String, ByVal SameTMID As Boolean, ByVal original_IORNo As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer, ByVal TeamMemberName As String, ByVal DateNotified As String, ByVal SeqNo As Integer, ByVal OrigTeamMemberName As String) As Boolean
        Try
            Dim pTable As IOR.Internal_Order_Request_ApprovalDataTable = Adapter3.Get_Internal_Order_Request_Approval(original_IORNo, 0, 0, 0, 0)
            Dim pscpRow As IOR.Internal_Order_Request_ApprovalRow = pTable(0)
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As String = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim TMSigned As Boolean = False

            If pTable.Count = 0 Then
                ' no matching record found, return false
                Return False
            End If

            ' Logical Rule - Cannot update a record without null columns
            If original_IORNo = Nothing Then
                Throw New ApplicationException("Update Cancelled: IORNO is a required field.")
            End If
            If Status = Nothing Then
                Throw New ApplicationException("Update Cancelled: Status is a required field.")
            End If
            If Comments = Nothing And Status = "Rejected" Then
                Throw New ApplicationException("Update Cancelled: Comments is a required field.")
            End If

            If Status <> "Pending" Then
                TMSigned = True
            End If

            ''***********
            ''* Locate Buyer
            ''***********
            Dim Ds As DataSet = New DataSet
            Dim PONo As Integer = 0
            Dim iBuyerTMID As Integer = 0 'Used to locate the Buyer 
            Dim dsBuyer As DataSet
            If commonFunctions.CheckDataSet(Ds) = True Then
                PONo = Ds.Tables(0).Rows(0).Item("PONo").ToString()
            End If

            dsBuyer = commonFunctions.GetTeamMemberBySubscription(99)
            If dsBuyer IsNot Nothing Then
                If dsBuyer.Tables.Count And dsBuyer.Tables(0).Rows.Count > 0 Then
                    For i = 0 To dsBuyer.Tables(0).Rows.Count - 1
                        If dsBuyer.Tables(0).Rows(0).Item("TMID") = original_TeamMemberID And PONo = 0 Then
                            Return False
                        End If
                    Next
                End If
            End If

            If Comments <> "" Or Comments <> Nothing Then
                ' Update the Internal_Order_Request_Approval record
                Dim rowsAffected As Integer = Adapter3.sp_Update_Internal_Order_Request_Approval(original_IORNo, original_TeamMemberID, TMSigned, Status, Comments, original_SeqNo, SameTMID, 0, User, Date.Today)
                ' Return true if precisely one row was updated, otherwise false
                Return rowsAffected = 1
            Else
                Return False
            End If
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & original_IORNo & ", OrigTMID: " & original_TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateInternalOrderRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> InternalOrderRequestBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/IOR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateInternalOrderRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "InternalOrderRequestBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateInternalOrderRequestApproval

    ''*****
    ''* Delete Internal_Order_Request_Approval
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteInternalOrderRequestApproval(ByVal original_IORNO As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter3.sp_Delete_Internal_Order_Request_Approval(original_IORNO, original_SeqNo, original_TeamMemberID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function 'EOF DeleteInternalOrderRequestApproval

#End Region 'EOF Internal_Order_Request_Approval

#Region "Internal_Order_Request_Documents"
    ''*****
    ''* Select Internal_Order_Request_Documents returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetInternalOrderRequestDocuments(ByVal IORNO As Integer, ByVal DocID As Integer) As IOR.Internal_Order_Request_DocumentsDataTable

        Try
            Return Adapter4.Get_Internal_Order_Request_Documents(IORNO, DocID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetInternalOrderRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> InternalOrderRequestBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "InternalOrderRequestBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetInternalOrderRequestDocuments
    ''*****
    ''* Delete Internal_Order_Request_Documents
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteInternalOrderRequestDocuments(ByVal IORNO As Integer, ByVal Original_DocID As Integer, ByVal Original_IORNO As Integer) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter4.sp_Delete_Internal_Order_Request_Documents(Original_DocID, Original_IORNO)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", DocID: " & Original_DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteInternalOrderRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> InternalOrderRequestBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteInternalOrderRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "InternalOrderRequestBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteInternalOrderRequestDocuments

#End Region 'EOF Internal_Order_Request_Documents

#Region "Internal_Order_Request_RSS"
    ''*****
    ''* Select Internal_Order_Request_RSS returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetInternalOrderRequestRss(ByVal IORNO As Integer, ByVal RSSID As Integer) As IOR.Internal_Order_Request_RSSDataTable

        Try
            Return Adapter5.Get_Internal_Order_Request_RSS(IORNO, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetInternalOrderRequestRss : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> InternalOrderRequestBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestRss : " & commonFunctions.replaceSpecialChar(ex.Message, False), "InternalOrderRequestBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetInternalOrderRequestRss
#End Region 'EOF Internal_Order_Request_RSS

#Region "Internal_Order_Request_RSS_Reply"
    ''*****
    ''* Select Internal_Order_Request_RSS_Reply returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetInternalOrderRequestRSSReply(ByVal IORNO As Integer, ByVal RSSID As Integer) As IOR.Internal_Order_Request_RSS_ReplyDataTable

        Try
            Return Adapter6.Get_Internal_Order_Request_RSS_Reply(IORNO, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetInternalOrderRequestRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> InternalOrderRequestBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "InternalOrderRequestBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetInternalOrderRequestRSSReply#Region "Internal_Order_Request_RSS_Reply"

#End Region 'EOF Internal_Order_Request_RSS_Reply

End Class

