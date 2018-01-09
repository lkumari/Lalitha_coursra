
' ************************************************************************************************
'
' Name:		AR_Price_Change_No_Accrual_Wizard_Future.aspx
' Purpose:	This Code Behind is for the AR Part Accrual Wizard for Future Part Numbers, which are pulled from the Planning & Forecasting Module
'
'	The Price Change – “No Accrual” event 
'   a.	Multiple Parts / Price Code Only - Current or Future, NOT BOTH TYPES
'   b.	No selection of SOLDTO, CABBV, DABBV, or Facility. All will be affected per part selection
'   c.	Only allowed for Mass Production And Service'  
'   f.	The System will check shipping history daily. If the price at the ship date matches the AR Event price, then a notification will be sent to Accounting to close the event.
'   g.	Approval only needed by Accounting Mgr
' NO PRICE CODE WILL BE SELECTED FOR FUTURE PARTS
' Date		Author	    
' 05/17/2011   Roderick Carlson
Partial Class AR_Price_Change_No_Accrual_Wizard_Future
    Inherits System.Web.UI.Page

    Protected Sub BindCriteria()

        Try
            Dim ds As DataSet

            Dim strUser As String = ""
            Dim iRowCounter As Integer = 0

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

            If HttpContext.Current.Request.Cookies("UGNDB_User") IsNot Nothing Then
                strUser = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                'strUser = "mechevarria"
                strUser = strUser.ToUpper
            End If

            ''bind existing data to drop down Customer control for selection criteria for search
            ds = PFModule.GetFuturePartNoByCreatedBy(strUser)

            'use the current user first for a match, if nothing found then use all users
            If commonFunctions.CheckDataSet(ds) = False Then
                ds = PFModule.GetFuturePartNoByCreatedBy("")
            End If

            If commonFunctions.CheckDataSet(ds) = True Then               
                ddTeamMember.Items.Clear()
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    Dim liListItem As New ListItem
                    liListItem.Text = ds.Tables(0).Rows(iRowCounter).Item("EmpName").ToString
                    liListItem.Value = ds.Tables(0).Rows(iRowCounter).Item("CreatedBy").ToString.ToUpper
                    ddTeamMember.Items.Add(liListItem)
                Next

                ddTeamMember.Items.Insert(0, "")

                If ddTeamMember.Items.FindByValue(strUser) IsNot Nothing And strUser <> "" Then
                    ddTeamMember.SelectedValue = strUser
                End If
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

    Protected Sub BindData()

        Try

            Dim ds As DataSet
            Dim dt As DataTable

            Dim objAREventDetailBLL As AREventDetailBLL = New AREventDetailBLL

            ds = ARGroupModule.GetAREvent(ViewState("AREID"))

            If commonFunctions.CheckDataSet(ds) = True Then
                ViewState("CustApprvEffDate") = ds.Tables(0).Rows(0).Item("CustApprvEffDate").ToString
            End If

            dt = objAREventDetailBLL.GetAREventDetail(ViewState("AREID"))

            If commonFunctions.CheckDataTable(dt) = True Then
                ddUGNFacility.SelectedValue = dt.Rows(0).Item("COMPNY").ToString

                If dt.Rows.Count = 1 Then
                    txtPartNo.Text = Replace(dt.Rows(0).Item("PARTNO").ToString.Trim, "( f )", "").Trim
                End If
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

    Private Sub RememberOldFuturePARTNOValues()

        Try
            Dim SelectedPARTNOList As New Collections.ArrayList()

            Dim index As String = ""

            For Each row As GridViewRow In gvPartNo.Rows

                index = gvPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

                Dim result As Boolean = DirectCast(row.FindControl("cbSelect"), CheckBox).Checked

                ' Check in the Session
                If Session("CHECKED_FUTURE_PARTNO_ITEMS") IsNot Nothing Then
                    SelectedPARTNOList = DirectCast(Session("CHECKED_FUTURE_PARTNO_ITEMS"), Collections.ArrayList)
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
                Session("CHECKED_FUTURE_PARTNO_ITEMS") = SelectedPARTNOList

                btnUpdate.Visible = ViewState("isAdmin")
                rbUpdateType.Visible = ViewState("isAdmin")
            Else
                Session("CHECKED_FUTURE_PARTNO_ITEMS") = Nothing

                btnUpdate.Visible = False
                rbUpdateType.Visible = False
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

    Private Sub RememberOldPendingPARTNOValues()

        Try
            Dim SelectedPARTNOList As New Collections.ArrayList()

            Dim index As String = ""

            For Each row As GridViewRow In gvPendingPartNo.Rows

                index = gvPendingPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

                Dim result As Boolean = DirectCast(row.FindControl("cbSelect"), CheckBox).Checked

                ' Check in the Session
                If Session("CHECKED_PENDING_PARTNO_ITEMS") IsNot Nothing Then
                    SelectedPARTNOList = DirectCast(Session("CHECKED_PENDING_PARTNO_ITEMS"), Collections.ArrayList)
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
                Session("CHECKED_PENDING_PARTNO_ITEMS") = SelectedPARTNOList

                btnUpdate.Visible = ViewState("isAdmin")
                rbUpdateType.Visible = ViewState("isAdmin")
            Else
                Session("CHECKED_PENDING_PARTNO_ITEMS") = Nothing

                btnUpdate.Visible = False
                rbUpdateType.Visible = False
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

    Private Sub RePopulateFuturePARTNOValues()

        Try
            Dim SelectedPARTNOList As Collections.ArrayList = DirectCast(Session("CHECKED_FUTURE_PARTNO_ITEMS"), Collections.ArrayList)

            If SelectedPARTNOList IsNot Nothing AndAlso SelectedPARTNOList.Count > 0 Then
                For Each row As GridViewRow In gvPartNo.Rows

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

    Private Sub RePopulatePendingPARTNOValues()

        Try
            Dim SelectedPARTNOList As Collections.ArrayList = DirectCast(Session("CHECKED_PENDING_PARTNO_ITEMS"), Collections.ArrayList)

            If SelectedPARTNOList IsNot Nothing AndAlso SelectedPARTNOList.Count > 0 Then
                For Each row As GridViewRow In gvPendingPartNo.Rows

                    Dim index As String = gvPendingPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

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

    Protected Sub cbSelectFuturePartNo_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            ClearMessages()

            Dim cbSelectedCheckbox As CheckBox

            cbSelectedCheckbox = CType(sender, CheckBox)

            lblMessage.Text = cbSelectedCheckbox.ToolTip & " was checked."

            RememberOldFuturePARTNOValues()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cbSelectPendingPartNo_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            ClearMessages()

            Dim cbSelectedCheckbox As CheckBox

            cbSelectedCheckbox = CType(sender, CheckBox)

            lblMessage.Text = cbSelectedCheckbox.ToolTip & " was checked."

            RememberOldPendingPARTNOValues()

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
            m.ContentLabel = "Price Change - No Accrual Wizard for FUTURE Parts (pulled from the Planning & Forecasting Module"

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
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

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
            'ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                If iTeamMemberID = 530 Then
                    iTeamMemberID = 246 'Mike Echevarria
                End If

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

    Private Sub EnableControls()

        Try

            DisableControls()

            'if sales/vp sales or admin, then allow use
            btnFilterPartNo.Visible = ViewState("isAdmin")
            btnClearFilterPartNo.Visible = ViewState("isAdmin")

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page            
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub DisableControls()

        Try
            btnUpdate.Visible = False
            rbUpdateType.Visible = False

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

                BindCriteria()

                CheckRights()

                Session("CHECKED_FUTURE_PARTNO_ITEMS") = Nothing
                Session("CHECKED_PENDING_PARTNO_ITEMS") = Nothing
              
                ViewState("AREID") = HttpContext.Current.Request.QueryString("AREID")
                lblAREID.Text = ViewState("AREID")

                BindData()

                ''search Customer PartNo
                'Dim strCustomerPartNoClientScript As String = HandleCustomerPartNoPopUps(txtCustomerPartNo.ClientID, "")
                'iBtnCustomerPartNo.Attributes.Add("onClick", strCustomerPartNoClientScript)

                EnableControls()

            End If

            'normally, this would all be handled in the Init function but since there is a need to check the AR EventID against the deduction table, it needs to be handled here.
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Accounts Receivable Tracking </b> > <a href='AR_Event_List.aspx'>AR Event Search </a> > <a href='AR_Event_Detail.aspx?AREID=" & ViewState("AREID") & "'> Event Detail </a> > Price Change - No Accrual Wizard for FUTURE parts "

                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
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

            DisableControls()

            Session("CHECKED_FUTURE_PARTNO_ITEMS") = Nothing
            Session("CHECKED_PENDING_PARTNO_ITEMS") = Nothing

            txtPartNo.Text = ""
            txtPartDesc.Text = ""
            ddTeamMember.SelectedIndex = -1

            gvPartNo.DataBind()
            gvPendingPartNo.DataBind()

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

    Protected Sub btnFilterPartNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilterPartNo.Click

        Try
            'filter part number list based on text fields (FG part no or cust part no)

            ClearMessages()

            DisableControls()

            Session("CHECKED_FUTURE_PARTNO_ITEMS") = Nothing
            Session("CHECKED_PENDING_PARTNO_ITEMS") = Nothing

            txtPartDesc.Text = txtPartDesc.Text.Trim
            txtPartNo.Text = txtPartNo.Text.Trim

            gvPartNo.DataBind()
            gvPendingPartNo.DataBind()

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

    Protected Sub gvPartNo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPartNo.PageIndexChanging

        Try

            RememberOldFuturePARTNOValues()
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

            RePopulateFuturePARTNOValues()

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

            Dim SelectedPARTNOList As New Collections.ArrayList()
        
            Dim iApprovalStatus As Integer = 0
            Dim iAccountingManagerID As Integer = 0
            Dim iApprovalRowID As Integer = 0

            Dim iPartRowCounter As Integer = 0
            Dim iPartTotalCount As Integer = 0

            Dim strFacility As String = ""
            Dim strPartNo As String = ""
            Dim strPriceCode As String = "A"

            If ddPriceCode.SelectedIndex >= 0 Then
                strPriceCode = ddPriceCode.SelectedValue
            End If

            If ddUGNFacility.SelectedIndex > 0 Then
                strFacility = ddUGNFacility.SelectedValue

                If Session("CHECKED_FUTURE_PARTNO_ITEMS") IsNot Nothing Or Session("CHECKED_PENDING_PARTNO_ITEMS") IsNot Nothing Then
                    'delete any current parts
                    ARGroupModule.DeleteAREventDetail(ViewState("AREID"), "C")

                    'check to see if future parts should be deleted or appended
                    If rbUpdateType.SelectedValue = "R" Then
                        'delete all AR Event Details
                        ARGroupModule.DeleteAREventDetail(ViewState("AREID"), "")
                    End If
                End If

                'insert Future P & F parts new AR Event Details
                If Session("CHECKED_FUTURE_PARTNO_ITEMS") IsNot Nothing Then

                    ''delete any current parts
                    'ARGroupModule.DeleteAREventDetail(ViewState("AREID"), "C")

                    ''check to see if future parts should be deleted or appended
                    'If rbUpdateType.SelectedValue = "R" Then
                    '    'delete all AR Event Details
                    '    ARGroupModule.DeleteAREventDetail(ViewState("AREID"), "")
                    'End If

                    iPartTotalCount = 0
                    SelectedPARTNOList = DirectCast(Session("CHECKED_FUTURE_PARTNO_ITEMS"), Collections.ArrayList)

                    If Session("CHECKED_FUTURE_PARTNO_ITEMS") IsNot Nothing And SelectedPARTNOList IsNot Nothing Then
                        iPartTotalCount = SelectedPARTNOList.Count
                    End If

                    For iRowPartCounter = 0 To iPartTotalCount - 1
                        strPartNo = SelectedPARTNOList.Item(iRowPartCounter).ToString.Trim

                        If strPartNo <> "" Then

                            'only facility and partno were selected
                            '(LREY) 01/08/2014
                            'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strFacility, "", "", strPartNo, "", "", strPriceCode, "", 0, 0, 0, True, 0)
                            ARGroupModule.InsertAREventDetail(ViewState("AREID"), strFacility, "", strPartNo, "", "", strPriceCode, 0, 0, 0, True, 0)

                        End If ' If strPartNo <> "" Then
                    Next 'iRowPartCounter = 0 
                End If 'If Session("CHECKED_FUTURE_PARTNO_ITEMS") IsNot Nothing Then

                'insertAS400 Pending to Ship parts new AR Event Details
                If Session("CHECKED_PENDING_PARTNO_ITEMS") IsNot Nothing Then

                    iPartTotalCount = 0
                    SelectedPARTNOList = DirectCast(Session("CHECKED_PENDING_PARTNO_ITEMS"), Collections.ArrayList)

                    If Session("CHECKED_PENDING_PARTNO_ITEMS") IsNot Nothing And SelectedPARTNOList IsNot Nothing Then
                        iPartTotalCount = SelectedPARTNOList.Count
                    End If

                    For iRowPartCounter = 0 To iPartTotalCount - 1
                        strPartNo = SelectedPARTNOList.Item(iRowPartCounter).ToString.Trim

                        If strPartNo <> "" Then

                            'only facility and partno were selected
                            '(LREY) 01/08/2014
                            'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strFacility, "", "", strPartNo, "", "", strPriceCode, "", 0, 0, 0, True, 0)
                            ARGroupModule.InsertAREventDetail(ViewState("AREID"), strFacility, "", strPartNo, "", "", strPriceCode, 0, 0, 0, True, 0)

                        End If ' If strPartNo <> "" Then
                    Next 'iRowPartCounter = 0 
                End If 'If Session("CHECKED_PENDING_PARTNO_ITEMS") IsNot Nothing Then

                If Session("CHECKED_FUTURE_PARTNO_ITEMS") IsNot Nothing Or Session("CHECKED_PENDING_PARTNO_ITEMS") IsNot Nothing Then

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

                    lblMessage.Text &= "Information Saved."

                    Response.Redirect("AR_Event_Detail.aspx?AREID=" & ViewState("AREID"), False)

                End If 'If Session("CHECKED_FUTURE_PARTNO_ITEMS") IsNot Nothing or Session("CHECKED_PENDING_PARTNO_ITEMS") IsNot Nothing Then
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

    Protected Sub btnCurrentPriceChangeNoAccrualWizard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentPriceChangeNoAccrualWizard.Click

        Try
            ClearMessages()

            Response.Redirect("AR_Price_Change_No_Accrual_Wizard_Current.aspx?AREID=" & ViewState("AREID"), False)                   

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
