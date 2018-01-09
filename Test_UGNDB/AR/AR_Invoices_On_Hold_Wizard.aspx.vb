' ************************************************************************************************
'
' Name:		AR_Invoices_On_Hold_Wizard.aspx
' Purpose:	This Code Behind is for the AR Invoices On Hold Wizard
'
'	The "Invoice On Hold No Accrual" event 
'   a.  Created Automatically when an invoice is on hold if the part has not caused any past events
'   b.  Sales will be notified to add an estimated price
'   c.	Require exactly one part number
'   d.	No selection of SOLDTO, CABBV, DABBV, or Facility. All will be affected per part selection
'   e.	Only allowed for all price codes
'   f.	Must currently be putting an invoice on hold
'   g.	The System will check invoices on hold daily. If the part is no longer placing an invoice on hold, then a notification will be sent to Accounting and Sales that the event was automatically closed.
'   h.	Approval only needed by Accounting Mgr
'
' Date		Author	    
' 07/26/2010   Roderick Carlson
Partial Class AR_Invoices_On_Hold_Wizard
    Inherits System.Web.UI.Page

    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim dsSubscription As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0
            ViewState("SubscriptionID") = 0
            ViewState("CurrentTeamMemberID") = 0
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                ' ''test developer as another team member
                'If iTeamMemberID = 530 Then
                '    'mike echevarria
                '    iTeamMemberID = 246
                'End If

                ViewState("TeamMemberID") = iTeamMemberID

                'Sales
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 9)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 9
                End If

                'VP of Sales
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 23)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 23
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 49) '52

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)                                    
                            ViewState("isAdmin") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete

                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
                End If
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page            
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ClearMessages()

        Try

            lblMessage.Text = ""
            lblMessageBottom.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Private Sub BindCriteria()

    '    Try
    '        'bind existing data to drop down controls for selection criteria for search       

    '        Dim ds As DataSet

    '        ds = ARGroupModule.GetAREventTypeList()
    '        If commonFunctions.CheckDataSet(ds) = True Then
    '            ddEventType.DataSource = ds
    '            ddEventType.DataTextField = ds.Tables(0).Columns("ddEventTypeName").ColumnName.ToString()
    '            ddEventType.DataValueField = ds.Tables(0).Columns("EventTypeID").ColumnName
    '            ddEventType.DataBind()
    '            ddEventType.Items.Insert(0, "")
    '        End If

    '        'ds =ARGroupModule.GetAREventStatusList()
    '        'If commonFunctions.CheckDataset(ds) = True Then
    '        '    ddEventStatus.DataSource = ds
    '        '    ddEventStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
    '        '    ddEventStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
    '        '    ddEventStatus.DataBind()
    '        '    ddEventStatus.Items.Insert(0, "")
    '        'End If

    '    Catch ex As Exception
    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Private Sub RememberOldPARTNOValues()

        Try
            Dim SelectedPARTNOList As New Collections.ArrayList()

            'Dim index As Integer = -1
            Dim index As String = ""

            For Each row As GridViewRow In gvPartNo.Rows

                'index = CInt(gvPARTNO.DataKeys(row.RowIndex).Value)

                index = gvPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

                Dim result As Boolean = DirectCast(row.FindControl("cbSelect"), CheckBox).Checked

                ' Check in the Session
                If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing Then
                    SelectedPARTNOList = DirectCast(Session("CHECKED_PARTNO_ITEMS"), Collections.ArrayList)
                End If

                If result Then
                    If Not SelectedPARTNOList.Contains(index) Then
                        SelectedPARTNOList.Add(index)
                    End If
                Else
                    SelectedPARTNOList.Remove(index)
                End If
            Next

            If SelectedPARTNOList IsNot Nothing AndAlso SelectedPARTNOList.Count > 0 Then
                Session("CHECKED_PARTNO_ITEMS") = SelectedPARTNOList
            Else
                Session("CHECKED_PARTNO_ITEMS") = Nothing               
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub UNCheckAllParts()

        Try
            Session("CHECKED_PARTNO_ITEMS") = Nothing

            For Each row As GridViewRow In gvPartNo.Rows

                Dim index As String = gvPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

                If index <> txtInvoicePartNo.Text Then
                    Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                    myCheckBox.Checked = False
                End If

            Next

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub UNCheckAllPriceCodes()

        Try
            Session("CHECKED_PRCCDE_ITEMS") = Nothing

            For Each row As GridViewRow In gvPriceCode.Rows

                Dim index As String = gvPriceCode.DataKeys(row.RowIndex).Value.ToString.Trim

                If index <> txtInvoicePriceCode.Text Then
                    Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                    myCheckBox.Checked = False
                End If

            Next

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub RePopulatePARTNOValues()

        Try
            Dim SelectedPARTNOList As Collections.ArrayList = DirectCast(Session("CHECKED_PARTNO_ITEMS"), Collections.ArrayList)

            If SelectedPARTNOList IsNot Nothing AndAlso SelectedPARTNOList.Count > 0 Then
                For Each row As GridViewRow In gvPartNo.Rows

                    'Dim index As Integer = CInt(gvPARTNO.DataKeys(row.RowIndex).Value)
                    Dim index As String = gvPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

                    If SelectedPARTNOList.Contains(index) Then
                        Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                        myCheckBox.Checked = True
                    End If

                Next

            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cbSelectPartNo_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            ClearMessages()

            Dim cbSelectedCheckbox As CheckBox

            ''only one part can be selected at a time
            'UNCheckAllParts()

            cbSelectedCheckbox = CType(sender, CheckBox)

            If cbSelectedCheckbox.Checked = True Then
                lblMessage.Text = cbSelectedCheckbox.ToolTip.Trim & " was checked."
                txtInvoicePartNo.Text = cbSelectedCheckbox.ToolTip.Trim
            Else
                lblMessage.Text = cbSelectedCheckbox.ToolTip.Trim & " was unchecked."
                txtInvoicePartNo.Text = ""
            End If

            ''only one part can be selected at a time
            UNCheckAllParts()

            RememberOldPARTNOValues()

            gvPriceCode.DataBind()
            gvPriceCode.Visible = True
            gvInvoicesOnHold.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cbSelectPriceCode_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            ClearMessages()

            Dim cbSelectedCheckbox As CheckBox

            ''only one price code can be selected at a time
            'UNCheckAllPriceCodes()

            cbSelectedCheckbox = CType(sender, CheckBox)

            If cbSelectedCheckbox.Checked = True Then
                lblMessage.Text = cbSelectedCheckbox.ToolTip.Trim & " was checked."
                txtInvoicePriceCode.Text = cbSelectedCheckbox.ToolTip.Trim
            Else
                lblMessage.Text = cbSelectedCheckbox.ToolTip.Trim & " was unchecked."
                txtInvoicePriceCode.Text = ""
            End If

            ''only one price code can be selected at a time
            UNCheckAllPriceCodes()

            RememberOldPriceCodeValues()

            gvInvoicesOnHold.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub RememberOldPriceCodeValues()

        Try
            Dim SelectedPriceCodeList As New Collections.ArrayList()

            'Dim index As Integer = -1
            Dim index As String = ""

            For Each row As GridViewRow In gvPriceCode.Rows

                'index = CInt(gvPriceCode.DataKeys(row.RowIndex).Value)
                index = gvPriceCode.DataKeys(row.RowIndex).Value.ToString.Trim

                Dim result As Boolean = DirectCast(row.FindControl("cbSelect"), CheckBox).Checked

                ' Check in the Session
                If Session("CHECKED_PRCCDE_ITEMS") IsNot Nothing Then
                    SelectedPriceCodeList = DirectCast(Session("CHECKED_PRCCDE_ITEMS"), Collections.ArrayList)
                End If

                If result Then
                    If Not SelectedPriceCodeList.Contains(index) Then
                        SelectedPriceCodeList.Add(index)
                    End If
                Else
                    SelectedPriceCodeList.Remove(index)
                End If
            Next

            If SelectedPriceCodeList IsNot Nothing AndAlso SelectedPriceCodeList.Count > 0 Then
                Session("CHECKED_PRCCDE_ITEMS") = SelectedPriceCodeList

                btnUpdate.Visible = ViewState("isAdmin")
            Else
                Session("CHECKED_PRCCDE_ITEMS") = Nothing

                btnUpdate.Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub RePopulatePriceCodeValues()

        Try
            Dim SelectedPriceCodeList As Collections.ArrayList = DirectCast(Session("CHECKED_PRCCDE_ITEMS"), Collections.ArrayList)

            If SelectedPriceCodeList IsNot Nothing AndAlso SelectedPriceCodeList.Count > 0 Then
                For Each row As GridViewRow In gvPriceCode.Rows

                    'Dim index As Integer = CInt(gvPriceCode.DataKeys(row.RowIndex).Value)
                    Dim index As String = gvPriceCode.DataKeys(row.RowIndex).Value.ToString.Trim

                    If SelectedPriceCodeList.Contains(index) Then
                        Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                        myCheckBox.Checked = True
                    End If

                Next

            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Invoices On Hold  - Selection Wizard"

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("ARExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub DisableControls()

        Try
            Session("CHECKED_PARTNO_ITEMS") = Nothing
            Session("CHECKED_PRCCDE_ITEMS") = Nothing

            RePopulatePARTNOValues()
            RePopulatePriceCodeValues()

            btnUpdate.Visible = False

            gvPriceCode.Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub


    Private Sub BindData()

        Try

            Dim ds As DataSet

            ds = ARGroupModule.GetAREventDetail(ViewState("AREID"))

            If commonFunctions.CheckDataset(ds) = True Then

                txtFGPartNo.Text = ds.Tables(0).Rows(0).Item("PartNo").ToString.Trim
                txtInvoicePartNo.Text = ds.Tables(0).Rows(0).Item("PartNo").ToString.Trim
                txtInvoicePriceCode.Text = ds.Tables(0).Rows(0).Item("PRCCDE").ToString.Trim

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then
                ViewState("AREID") = 0

                CheckRights()

                'BindCriteria()

                'Session("CHECKED_PARTNO_ITEMS") = Nothing
                'Session("CHECKED_PRCCDE_ITEMS") = Nothing

                ViewState("AREID") = HttpContext.Current.Request.QueryString("AREID")
                lblAREID.Text = ViewState("AREID")

                If ViewState("AREID") > 0 Then
                    BindData()
                End If

                DisableControls()
            End If

            'normally, this would all be handled in the Init function but since there is a need to check the AR EventID against the deduction table, it needs to be handled here.
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Accounts Receivable Tracking </b> > <a href='AR_Event_List.aspx'>AR Event Search </a> > <a href='AR_Event_Detail.aspx?AREID=" & ViewState("AREID") & "'> Event Detail </a> > Event Invoices On Hold Wizard"

                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnBackToAREvent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackToAREvent.Click

        Try
            ClearMessages()

            Response.Redirect("AR_Event_Detail.aspx?AREID=" & ViewState("AREID"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnClearFilterPartNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearFilterPartNo.Click

        Try
            ClearMessages()

            ' btnUpdate.Visible = False
            DisableControls()

            txtFGPartNo.Text = ""
            txtInvoicePartNo.Text = ""

            'Session("CHECKED_PARTNO_ITEMS") = Nothing
            'Session("CHECKED_PRCCDE_ITEMS") = Nothing

            RePopulatePARTNOValues()
            RePopulatePriceCodeValues()

            gvPartNo.DataBind()
            'gvPriceCode.Visible = False

            gvInvoicesOnHold.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnFilterPartNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilterPartNo.Click

        Try
            ClearMessages()

            'Session("CHECKED_PARTNO_ITEMS") = Nothing
            'Session("CHECKED_PRCCDE_ITEMS") = Nothing
            DisableControls()

            RePopulatePARTNOValues()
            RePopulatePriceCodeValues()

            'Session("PARTNOWhereClause") = " AND SHPDTE >= '20000101' "
            'Session("PARTNOWhereClause") = Nothing

            Dim ds As DataSet

            Dim iRowCounter As Integer = 0

            Dim strTempWhereClause As String = "AND PARTNO IN  ("
            Dim strPartNoList As String = ""

            If txtFGPartNo.Text.Trim <> "" Then
                txtInvoicePartNo.Text = txtFGPartNo.Text.Trim
                '(LREY) 01/08/2014
                'ds = commonFunctions.GetCustomerPartBPCSPartRelate(txtFGPartNo.Text.Trim, "", "", "", "")

                'If commonFunctions.CheckDataSet(ds) = True Then
                '    For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                '        If ds.Tables(0).Rows(iRowCounter).Item("BPCSPartNo").ToString <> "" Then
                '            If strPartNoList <> "" Then
                '                strPartNoList &= ","
                '            End If

                '            strPartNoList &= "'" & ds.Tables(0).Rows(iRowCounter).Item("BPCSPartNo") & "'"
                '        End If
                '    Next
                'End If
            End If

            If strPartNoList <> "" Then
                strTempWhereClause &= strPartNoList
            End If

            'If strPartNoList <> "" Then
            '    Session("PARTNOWhereClause") = " AND SHPDTE >= '20000101' " & strTempWhereClause & ")"
            'End If

            gvPartNo.DataBind()
            gvPriceCode.DataBind()

            gvInvoicesOnHold.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvPartNo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPartNo.PageIndexChanging

        Try

            RememberOldPARTNOValues()
            gvPartNo.PageIndex = e.NewPageIndex
            gvPartNo.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvPartNo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPartNo.RowDataBound

        Try

            RePopulatePARTNOValues()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        Try
            ClearMessages()

            Dim dt As DataTable
            Dim objAREventApproval As New ARApprovalBLL

            Dim iApprovalStatus As Integer = 0
            Dim iAccountingManagerID As Integer = 0
            Dim iApprovalRowID As Integer = 0
            Dim dEstimatedPrice As Double = 0

            If txtEstimatedPrice.Text.Trim <> "" Then
                dEstimatedPrice = CType(txtEstimatedPrice.Text.Trim, Double)
            End If

            'delete all AR Event Details
            ARGroupModule.DeleteAREventDetail(ViewState("AREID"), "")

            If txtInvoicePartNo.Text.Trim <> "" And txtInvoicePriceCode.Text.Trim <> "" Then
                'save AR Event Details
                '(LREY) 01/08/2014
                'ARGroupModule.InsertAREventDetail(ViewState("AREID"), "", "", 0, txtInvoicePartNo.Text.Trim, "", "", txtInvoicePriceCode.Text.Trim, "", 0, 0, 0, False, dEstimatedPrice)

                ARGroupModule.InsertAREventDetail(ViewState("AREID"), "", "", txtInvoicePartNo.Text.Trim, "", "", txtInvoicePriceCode.Text.Trim, "", 0, 0, False, dEstimatedPrice)

                'save AR Event Invoice On Hold While Event is open
                ARGroupModule.InsertAREventInvoicesOnHold(ViewState("AREID"), txtInvoicePartNo.Text.Trim, txtInvoicePriceCode.Text.Trim)

                'if event was already approved, reset approval
                dt = objAREventApproval.GetAREventApprovalStatus(ViewState("AREID"), 21)
                If commonFunctions.CheckDataTable(dt) = True Then
                    If dt.Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                        iApprovalStatus = dt.Rows(0).Item("StatusID")

                        If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                            iAccountingManagerID = dt.Rows(0).Item("TeamMemberID")
                        End If

                        If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                            iApprovalRowID = dt.Rows(0).Item("RowID")
                        End If

                        Select Case iApprovalStatus
                            Case 0, 1, 3
                                'do nothing
                            Case 4 'already approved - set to open
                                'reset Accounting Manager approval
                                ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), iAccountingManagerID, 1)
                                ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 1) 'Open (Pending Sales Submission)
                                ViewState("EventStatusID") = 1
                                'update history
                                ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Updated Event")
                        End Select
                    End If
                End If 'end if dt has values

                Response.Redirect("AR_Event_Detail.aspx?AREID=" & ViewState("AREID"), False)
            Else
                lblMessage.Text = "Error: One part and one price code must be selected."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

End Class
