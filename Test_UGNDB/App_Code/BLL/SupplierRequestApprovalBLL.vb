''******************************************************************************************************
''* SupplierRequestApprovalBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: AssetsExpProj.aspx - gvApproval
''* Author  : LRey 03/02/2010
''******************************************************************************************************
Imports SupplierTableAdapters

<System.ComponentModel.DataObject()> _
Public Class SupplierRequestApprovalBLL
    Private pAdapter As Supplier_Request_Approval_TableAdapter = Nothing
    Private pAdapter2 As Supplier_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As SupplierTableAdapters.Supplier_Request_Approval_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Supplier_Request_Approval_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property

    ''*****
    ''* Select Supplier_Request_Approval returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetSupplierRequestApproval(ByVal SUPNo As Integer, ByVal Sequence As Integer) As Supplier.Supplier_Request_ApprovalDataTable
        Try
            ' Logical Rule - Cannot insert a record without null columns
            If SUPNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: SUPNo is a required field.")
            End If

            If Sequence = Nothing Then
                Sequence = 0
            End If

            Return Adapter.Get_Supplier_Request_Approval(SUPNo, Sequence, 0, 0, 0)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", SeqNo: " & Sequence & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SupplierRequestApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/SUP/SupplierRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SupplierRequestApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetSupplierRequestApproval

    '*****
    '* Update Supplier_Request_Approval
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function UpdateSupplierRequestApproval(ByVal Status As String, ByVal Comments As String, ByVal SameTMID As Boolean, ByVal original_SUPNo As Integer, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer, ByVal TeamMemberName As String, ByVal DateNotified As String, ByVal SeqNo As Integer, ByVal OrigTeamMemberName As String) As Boolean

        Try
            Dim pTable As Supplier.Supplier_Request_ApprovalDataTable = Adapter.Get_Supplier_Request_Approval(original_SUPNo, 0, 0, 0, 0)
            Dim pscpRow As Supplier.Supplier_Request_ApprovalRow = pTable(0)
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As String = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim TMSigned As Boolean = False

            If pTable.Count = 0 Then
                ' no matching record found, return false
                Return False
            End If

            ' Logical Rule - Cannot update a record without null columns
            If original_SUPNo = Nothing Then
                Throw New ApplicationException("Update Cancelled: SUPNo is a required field.")
            End If
            If Status = Nothing Then
                Throw New ApplicationException("Update Cancelled: Status is a required field.")
            End If
            If Comments = Nothing And Status = "Rejected" Then
                Throw New ApplicationException("Update Cancelled: Comments is a required field.")
            Else
                Comments = commonFunctions.replaceSpecialChar(Comments, False)
            End If

            If Status <> "Pending" Then
                TMSigned = True
            End If

            Dim Ds As DataSet = New DataSet
            Dim dsCorpAcct As DataSet
            Dim dsCorpAcctMgr As DataSet
            Dim iCorpAcctTMID As Integer = 0 'Used to locate Corporate Accounting 
            Dim iCorpAcctMgrTMID As Integer = 0 'Used to locate Corporate Accounting Mgr 
            Dim VendorNo As Integer = 0
            Dim InBPCS As Boolean = False
            ''***********
            ''* Get vendor and InBPCS Values
            ''***********
            If commonFunctions.CheckDataSet(Ds) = True Then
                VendorNo = Ds.Tables(0).Rows(0).Item("VendorNo").ToString()
                InBPCS = Ds.Tables(0).Rows(0).Item("InBPCS").ToString()
            End If

            ''***********
            ''* Locate Corporate Accounting
            ''***********
            dsCorpAcct = commonFunctions.GetTeamMemberBySubscription(95)
            If dsCorpAcct IsNot Nothing Then
                If dsCorpAcct.Tables.Count And dsCorpAcct.Tables(0).Rows.Count > 0 Then
                    iCorpAcctTMID = dsCorpAcct.Tables(0).Rows(0).Item("TMID")
                End If
            End If

            ''***********
            ''* Locate Corporate Accounting Mgr
            ''***********
            dsCorpAcctMgr = commonFunctions.GetTeamMemberBySubscription(118)
            If dsCorpAcct IsNot Nothing Then
                If dsCorpAcctMgr.Tables.Count And dsCorpAcctMgr.Tables(0).Rows.Count > 0 Then
                    iCorpAcctMgrTMID = dsCorpAcctMgr.Tables(0).Rows(0).Item("TMID")
                End If
            End If

            If (iCorpAcctTMID = original_TeamMemberID Or iCorpAcctMgrTMID = original_TeamMemberID) Then
                If VendorNo = 0 And InBPCS <> True Then
                    Return False
                End If
            End If

            If Comments <> "" Or Comments <> Nothing Then
                ' Update the Supplier_Request_Approval record
                Dim rowsAffected As Integer = Adapter.sp_Update_Supplier_Request_Approval(original_SUPNo, original_TeamMemberID, TMSigned, Status, Comments, original_SeqNo, SameTMID, User, Date.Today)

                ' Return true if precisely one row was updated, otherwise false
                Return rowsAffected = 1
            Else
                Return False
            End If
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & original_SUPNo & ", OrigTMID: " & original_TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SupplierRequestApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/SUP/SupplierRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SupplierRequestApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateSupplierRequestApproval

    ' ''    ''*****
    ' ''    ''* Delete Supplier_Request_Approval
    ' ''    ''*****
    ' ''    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    ' ''    Public Function DeleteSupplierRequestApproval(ByVal SUPNo As String, ByVal SeqNo As Integer, ByVal ResponsibleTMID As Integer, ByVal original_SUPNo As String, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer) As Boolean

    ' ''        Dim rowsAffected As Integer = Adapter.sp_Delete_Supplier_Request_Approval(original_SUPNo, original_SeqNo, original_TeamMemberID)

    ' ''        ' Return true if precisely one row was deleted, otherwise false
    ' ''        Return rowsAffected = 1

    ' ''    End Function

End Class

