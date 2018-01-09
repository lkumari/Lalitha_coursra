' ************************************************************************************************
' Name:	RepairExpProj.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 11/22/2010    LRey			Created .Net application
' 10/25/2012    LRey            Modified to use .NET Mail process; Consolidated Email Body; Modified forward to include TM's at same level after a rejection.
' 01/07/2013    LRey        Added a control to hide the Edit button in the approval process to prevent out of sequence approval.
' 04/18/2013    LRey        Allow any team member to select 1st level approval if UGN Location is TP
' 02/12/2014    LRey        Replaced DeptOrCostCenter with new ERP values.
' ************************************************************************************************
Partial Class EXP_RepairExpProj
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to define the primary record
            If HttpContext.Current.Request.QueryString("pProjNo") <> "" Then
                ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
            Else
                ViewState("pProjNo") = ""
            End If

            ''Used for Supplement entry
            If HttpContext.Current.Request.QueryString("pPrntProjNo") <> "" Then
                ViewState("pPrntProjNo") = HttpContext.Current.Request.QueryString("pPrntProjNo")

                If ViewState("pProjNo") = Nothing Then
                    lblProjectID.Text = ViewState("pPrntProjNo") & "?"
                End If
            Else
                ViewState("pPrntProjNo") = ""
            End If

            ''Used for Expense binddata and update
            If HttpContext.Current.Request.QueryString("pEID") <> "" Then
                ViewState("pEID") = HttpContext.Current.Request.QueryString("pEID")
            Else
                ViewState("pEID") = 0
            End If

            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pAprv") <> "" Then
                ViewState("pAprv") = HttpContext.Current.Request.QueryString("pAprv")
            Else
                ViewState("pAprv") = 0
            End If

            ''Used to take user back to Expense Tab after reset/save
            If HttpContext.Current.Request.QueryString("pEV") <> "" Then
                ViewState("pEV") = HttpContext.Current.Request.QueryString("pEV")
            Else
                ViewState("pEV") = 0
            End If

            ''Used to take user back to Supporting Documents Tab after save.
            If HttpContext.Current.Request.QueryString("pSD") <> "" Then
                ViewState("pSD") = HttpContext.Current.Request.QueryString("pSD")
            Else
                ViewState("pSD") = 0
            End If

            ''Used to take user back to Communication Board Tab after reset/save
            If HttpContext.Current.Request.QueryString("pRID") <> "" Then
                ViewState("pRID") = HttpContext.Current.Request.QueryString("pRID")
            Else
                ViewState("pRID") = 0
            End If

            ''Used to allow TM(s) to Communicated with Approvers for Q&A
            If HttpContext.Current.Request.QueryString("pRC") <> "" Then
                ViewState("pRC") = HttpContext.Current.Request.QueryString("pRC")
            Else
                ViewState("pRC") = 0
            End If

            ''Used to Show/Hide Future Part Info text boxes
            ViewState("pFPNo") = False

            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            If ViewState("pProjNo") = Nothing Then
                m.ContentLabel = "New R Project"
            Else
                m.ContentLabel = "R Project"
            End If

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pProjNo") = Nothing Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='RepairExpProjList.aspx'><b>R Project Search</b></a> > New R Project"
                Else
                    If ViewState("pAprv") = 0 Then
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='RepairExpProjList.aspx'><b>R Project Search</b></a> > R Project"
                    Else 'Go Back To approval Screen
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='RepairExpProjList.aspx'><b>R Project Search</b></a> > <a href='crExpProjRepairApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1'><b>Approval</b></a> > R Project"
                    End If
                End If
                lbl.Visible = True
            End If

            ctl = m.FindControl("SiteMapPath1")
            If ctl IsNot Nothing Then
                Dim smp As SiteMapPath = CType(ctl, SiteMapPath)
                smp.Visible = False
            End If

            ''******************************************
            '' Expand this Master Page menu item
            ''******************************************
            ctl = m.FindControl("SPRExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                BindCriteria()

                ''****************************************
                ''Redirect user to the right tab location
                ''****************************************
                If ViewState("pProjNo") <> "" Then
                    BindData(ViewState("pProjNo"))
                Else
                    BindData(ViewState("pPrntProjNo"))
                    txtProjectTitle.Focus()
                    txtDateSubmitted.Text = Date.Today
                End If

                If ViewState("pEID") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True
                ElseIf ViewState("pEV") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True
                ElseIf ViewState("pSD") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True
                ElseIf ViewState("pRID") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(3)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(3).Selected = True
                ElseIf ViewState("pRC") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(3)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(3).Selected = True
                Else
                    mvTabs.ActiveViewIndex = Int32.Parse(0)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(0).Selected = True
                End If
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            txtProjectTitle.Attributes.Add("onkeypress", "return tbLimit();")
            txtProjectTitle.Attributes.Add("onkeyup", "return tbCount(" + lblProjectTitle.ClientID + ");")
            txtProjectTitle.Attributes.Add("maxLength", "50")

            txtProjDateNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtProjDateNotes.Attributes.Add("onkeyup", "return tbCount(" + lblProjDateNotes.ClientID + ");")
            txtProjDateNotes.Attributes.Add("maxLength", "2000")

            txtJustification.Attributes.Add("onkeypress", "return tbLimit();")
            txtJustification.Attributes.Add("onkeyup", "return tbCount(" + lblJustification.ClientID + ");")
            txtJustification.Attributes.Add("maxLength", "2000")

            txtDescription.Attributes.Add("onkeypress", "return tbLimit();")
            txtDescription.Attributes.Add("onkeyup", "return tbCount(" + lblDescription.ClientID + ");")
            txtDescription.Attributes.Add("maxLength", "50")

            txtNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNotes.ClientID + ");")
            txtNotes.Attributes.Add("maxLength", "300")

            txtClosingNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtClosingNotes.Attributes.Add("onkeyup", "return tbCount(" + lblClosingNotes.ClientID + ");")
            txtClosingNotes.Attributes.Add("maxLength", "300")

            txtVoidReason.Attributes.Add("onkeypress", "return tbLimit();")
            txtVoidReason.Attributes.Add("onkeyup", "return tbCount(" + lblVoidReason.ClientID + ");")
            txtVoidReason.Attributes.Add("maxLength", "300")

            txtReply.Attributes.Add("onkeypress", "return tbLimit();")
            txtReply.Attributes.Add("onkeyup", "return tbCount(" + lblReply.ClientID + ");")
            txtReply.Attributes.Add("maxLength", "300")

            txtFileDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtFileDesc.Attributes.Add("onkeyup", "return tbCount(" + lblFileDesc.ClientID + ");")
            txtFileDesc.Attributes.Add("maxLength", "200")

            txtReSubmit.Attributes.Add("onkeypress", "return tbLimit();")
            txtReSubmit.Attributes.Add("onkeyup", "return tbCount(" + lblReSubmitCnt.ClientID + ");")
            txtReSubmit.Attributes.Add("maxLength", "300")

            Dim strPreviewClientScript As String = "javascript:void(window.open('crViewExpProjRepair.aspx?pProjNo=" & ViewState("pProjNo") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
            btnPreview.Attributes.Add("onclick", strPreviewClientScript)
3:
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

    Protected Sub mnuTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuTabs.MenuItemClick
        mvTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If mvTabs.ActiveViewIndex = 0 Then
            BindData(ViewState("pProjNo"))
        End If
    End Sub 'EOF mnuTabs_MenuItemClick

#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        Try
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            ViewState("strProdOrTestEnvironment") = strProdOrTestEnvironment
            ViewState("Admin") = False
            ViewState("ObjectRole") = False
            ViewState("DefaultUserFacility") = Nothing

            ''*******
            '' Disable controls by default
            ''*******
            btnAdd.Enabled = False
            btnSave1.Enabled = False
            btnSave2.Enabled = True
            btnSave3.Enabled = False
            btnReset1.Enabled = False
            btnReset3.Enabled = False
            btnReset4.Enabled = False
            btnReset5.Enabled = True
            btnReset6.Enabled = False
            btnUpload.Enabled = False
            btnDelete.Enabled = False
            btnPreview.Enabled = False
            btnAppend.Enabled = False
            btnFwdApproval.Enabled = False
            btnBuildApproval.Enabled = False
            btnBuildApproval.Visible = False
            ddProjectStatus.Enabled = False
            btnAddtoGrid2.Enabled = False
            btnCRProjNoReq.Enabled = True
            cbCRProjNoReq.Enabled = False
            uploadFile.Enabled = False
            mnuTabs.Items(0).Enabled = True
            mnuTabs.Items(1).Enabled = False
            mnuTabs.Items(2).Enabled = False
            mnuTabs.Items(3).Enabled = False
            gvExpense.Columns(6).Visible = False
            gvSupportingDocument.Columns(4).Visible = False
            gvApprovers.Columns(7).Visible = False
            gvApprovers.Columns(8).Visible = False
            gvApprovers.Columns(9).Visible = False
            gvApprovers.ShowFooter = False
            gvQuestion.Columns(0).Visible = True
            txtActualCost.Visible = False
            txtCustomerCost.Visible = False
            txtClosingNotes.Visible = False
            txtVoidReason.Visible = False
            txtActualCost.Enabled = False
            txtCustomerCost.Enabled = False
            txtClosingNotes.Enabled = False
            txtVoidReason.Enabled = False
            txtEstCmpltDt.Enabled = True
            txtHDEstCmpltDt.Enabled = True
            txtNextEstCmpltDt.Enabled = False
            lblActualCost.Visible = False
            lblCustomerCost.Visible = False
            lblClosingNts.Visible = False
            lblVoidRsn.Visible = False
            lblReqCustomerCost.Visible = False
            lblReqClosingNts.Visible = False
            lblReqActualCost.Visible = False
            lblReqVoidRsn.Visible = False
            AEExtender.Collapsed = False
            SDExtender.Collapsed = False
            rfvClostingNotes.Enabled = False
            rfvVoidReason.Enabled = False
            btnCRProjNoReq.Enabled = False
            If cddDepartment.SelectedValue = Nothing Then
                rfvDCC.Enabled = True
            Else
                rfvDCC.Enabled = False
            End If

            lblReqReSubmit.Visible = False
            lblReSubmit.Visible = False
            txtReSubmit.Visible = False
            rfvReSubmit.Enabled = False
            vsReSubmit.Enabled = False

            ''** Project Status
            Dim ProjectStatus As String = Nothing
            Select Case txtRoutingStatus.Text
                Case "N"
                    ProjectStatus = "Open" ''ddProjectStatus.SelectedValue"
                    ddProjectStatus.Visible = True
                    ddProjectStatus2.Visible = False
                Case "A"
                    ProjectStatus = "Approved" ''ddProjectStatus.SelectedValue
                    ddProjectStatus.Visible = True
                    ddProjectStatus2.Visible = False
                Case "C"
                    ProjectStatus = "Completed" ''ddProjectStatus.SelectedValue
                    ddProjectStatus.Visible = True
                    ddProjectStatus2.Visible = False
                Case "H"
                    ProjectStatus = "Hold" ''ddProjectStatus2.SelectedValue
                    ddProjectStatus.Visible = False
                    ddProjectStatus2.Visible = True
                Case "T"
                    ProjectStatus = "In Process" ''ddProjectStatus2.SelectedValue
                    ddProjectStatus.Visible = False
                    ddProjectStatus2.Visible = True
                Case "S"
                    ProjectStatus = "In Process" ''ddProjectStatus2.SelectedValue
                    ddProjectStatus.Visible = False
                    ddProjectStatus2.Visible = True
                Case "R"
                    ProjectStatus = "Rejected" ''ddProjectStatus2.SelectedValue
                    ddProjectStatus.Visible = False
                    ddProjectStatus2.Visible = True
                    lblReqReSubmit.Visible = True
                    lblReSubmit.Visible = True
                    txtReSubmit.Visible = True
                    rfvReSubmit.Enabled = True
                    vsReSubmit.Enabled = True
                Case "V"
                    ProjectStatus = "Void" ''ddProjectStatus.SelectedValue
                    ddProjectStatus.Visible = True
                    ddProjectStatus2.Visible = False
                Case Else
                    ddProjectStatus.Visible = True
                    ddProjectStatus2.Visible = False
            End Select
            ViewState("ProjectStatus") = ProjectStatus

            If ProjectStatus = "Capitalized" Then
                lblReqActualCost.Visible = True
                lblActualCost.Visible = True
                txtActualCost.Visible = True
                txtActualCost.Enabled = True

                lblReqCustomerCost.Visible = True
                txtCustomerCost.Visible = True
                txtCustomerCost.Enabled = True
                lblCustomerCost.Visible = True

                lblReqClosingNts.Visible = True
                lblClosingNts.Visible = True
                txtClosingNotes.Enabled = True
                txtClosingNotes.Visible = True
                rfvClostingNotes.Enabled = True
            End If

            If ProjectStatus = "Void" Then
                lblActualCost.Visible = True
                lblCustomerCost.Visible = True
                lblClosingNts.Visible = True
                txtActualCost.Visible = True
                txtCustomerCost.Visible = True
                txtClosingNotes.Visible = True

                lblReqVoidRsn.Visible = True
                lblVoidRsn.Visible = True
                txtVoidReason.Visible = True
                txtVoidReason.Enabled = True
                rfvVoidReason.Enabled = True
            End If

            ''* Estimated Completion Date
            If txtEstCmpltDt.Text = "" Then
                rfvEstCmpltDt.Enabled = False
            End If

            rfvEstCmpltDtChngRsn.Enabled = True
            If txtHDEstCmpltDt.Text.Trim <> "" And txtNextEstCmpltDt.Text.Trim <> "" Then
                If CType(txtHDEstCmpltDt.Text, Date) = CType(txtNextEstCmpltDt.Text, Date) Then
                    rfvEstCmpltDtChngRsn.Enabled = False
                Else
                    rfvEstCmpltDtChngRsn.Enabled = True
                End If
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim dsTMFacility As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iTMFacility As String = Nothing
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 116 'Repair Expense Form ID
            Dim iRoleID As Integer = 0
            Dim i As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Jason.Earl", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    ViewState("iTeamMemberID") = iTeamMemberID
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Facility Location based on SubscriptionID
                        dsTMFacility = SecurityModule.GetTMWorkHistory(iTeamMemberID, 92)
                        If dsTMFacility IsNot Nothing Then
                            If dsTMFacility.Tables.Count And dsTMFacility.Tables(0).Rows.Count > 0 Then
                                iTMFacility = dsTMFacility.Tables(0).Rows(0).Item("UGNFacility")
                                ViewState("DefaultUserFacility") = iTMFacility
                            Else
                                iTMFacility = HttpContext.Current.Session("UserFacility")
                                ViewState("DefaultUserFacility") = HttpContext.Current.Session("UserFacility")
                            End If
                        End If

                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)

                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                For i = 0 To dsRoleForm.Tables(0).Rows.Count - 1
                                    iRoleID = dsRoleForm.Tables(0).Rows(i).Item("RoleID")
                                    Select Case iRoleID
                                        Case 11 '*** UGNAdmin: Full Access
                                            ''Used by full admin such as Developers
                                            ViewState("ObjectRole") = True
                                            ViewState("Admin") = True
                                            btnAdd.Enabled = True
                                            btnSave1.Enabled = True
                                            btnReset1.Enabled = True
                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pProjNo") = Nothing Or ViewState("pProjNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                txtProjectTitle.Focus()
                                            Else
                                                btnFwdApproval.Enabled = True
                                                If ProjectStatus = "Open" Or (txtRoutingStatus.Text = "N") Then
                                                    ''Build approval during first initial save.
                                                    btnBuildApproval.Enabled = True
                                                    btnBuildApproval.Visible = True
                                                    gvApprovers.Columns(8).Visible = True
                                                    gvApprovers.ShowFooter = True
                                                    ddProjectStatus.Enabled = False
                                                    If ddCRProjNo.SelectedValue = Nothing And cbCRProjNoReq.Checked = False Then
                                                        btnCRProjNoReq.Enabled = True
                                                    End If
                                                ElseIf ProjectStatus = "In Process" And (txtRoutingStatus.Text = "R") Then
                                                    btnBuildApproval.Enabled = True
                                                    btnBuildApproval.Visible = True
                                                    btnFwdApproval.Enabled = True
                                                    gvApprovers.Columns(8).Visible = True
                                                    gvApprovers.ShowFooter = True
                                                    ddProjectStatus2.Enabled = True
                                                    btnCRProjNoReq.Enabled = False
                                                ElseIf ProjectStatus = "In Process" And ((txtRoutingStatus.Text = "H") Or (txtRoutingStatus.Text = "T")) Then
                                                    ddProjectStatus2.Enabled = True

                                                ElseIf (txtRoutingStatus.Text <> "N") And (txtRoutingStatus.Text <> "R") And (txtRoutingStatus.Text <> "H") And (txtRoutingStatus.Text <> "T") Then
                                                    btnAppend.Enabled = True
                                                    ddProjectStatus.Enabled = True
                                                    btnCRProjNoReq.Enabled = False
                                                    If (txtRoutingStatus.Text = "A") Then
                                                        ddProjectStatus.Enabled = True
                                                    End If
                                                End If
                                                btnSave2.Enabled = True
                                                btnSave3.Enabled = True
                                                btnReset3.Enabled = True
                                                btnReset4.Enabled = True
                                                btnReset5.Enabled = True
                                                btnPreview.Enabled = True
                                                If txtRoutingStatus.Text = "N" Then
                                                    btnDelete.Enabled = True
                                                End If
                                                btnReset6.Enabled = True
                                                btnUpload.Enabled = True
                                                btnAddtoGrid2.Enabled = True
                                                gvExpense.Columns(6).Visible = True
                                                gvSupportingDocument.Columns(4).Visible = True
                                                uploadFile.Enabled = True
                                                If (txtRoutingStatus.Text = "T" Or txtRoutingStatus.Text = "R") Then
                                                    gvApprovers.Columns(7).Visible = True
                                                End If
                                                gvApprovers.Columns(9).Visible = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                txtNextEstCmpltDt.Enabled = True
                                                If ddCRProjNo.SelectedValue = Nothing Then
                                                    btnCRProjNoReq.Enabled = True
                                                End If
                                            End If
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Project Leader
                                            ViewState("ObjectRole") = True
                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pProjNo") = Nothing Or ViewState("pProjNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                txtProjectTitle.Focus()
                                                btnCRProjNoReq.Enabled = False
                                                btnAdd.Enabled = True
                                                btnSave1.Enabled = True
                                                btnReset1.Enabled = True
                                            Else
                                                If ViewState("iTeamMemberID") = ddProjectLeader.SelectedValue Then
                                                    ViewState("Admin") = True
                                                    If txtRoutingStatus.Text <> "C" Then
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                    End If
                                                    btnAdd.Enabled = True
                                                    Select Case ProjectStatus
                                                        Case "Open"
                                                            If (txtRoutingStatus.Text = "N") Then
                                                                If ddUGNLocation.SelectedValue = "UT" Then

                                                                    btnBuildApproval.Enabled = True
                                                                    btnBuildApproval.Visible = True
                                                                    gvApprovers.Columns(8).Visible = True
                                                                    gvApprovers.ShowFooter = True
                                                                End If
                                                                btnFwdApproval.Enabled = True
                                                                btnDelete.Enabled = True
                                                                ddProjectStatus.Enabled = False
                                                            End If
                                                            If ddCRProjNo.SelectedValue = Nothing And cbCRProjNoReq.Checked = False Then
                                                                btnCRProjNoReq.Enabled = True
                                                            End If
                                                            txtNextEstCmpltDt.Enabled = True
                                                            btnAddtoGrid2.Enabled = True
                                                            gvExpense.Columns(6).Visible = True
                                                            btnSave3.Enabled = True
                                                            btnReset3.Enabled = True
                                                            btnReset4.Enabled = True
                                                            btnReset6.Enabled = True
                                                            btnUpload.Enabled = True
                                                            uploadFile.Enabled = True
                                                            gvSupportingDocument.Columns(4).Visible = True
                                                        Case "In Process"
                                                            txtNextEstCmpltDt.Enabled = True
                                                            If (txtRoutingStatus.Text = "R" Or txtRoutingStatus.Text = "S") Then
                                                                btnReset4.Enabled = True
                                                                btnFwdApproval.Enabled = True
                                                                btnAddtoGrid2.Enabled = True
                                                                gvExpense.Columns(6).Visible = True
                                                                btnSave3.Enabled = True
                                                                btnReset3.Enabled = True
                                                                btnReset6.Enabled = True
                                                                btnUpload.Enabled = True
                                                                uploadFile.Enabled = True
                                                                gvSupportingDocument.Columns(4).Visible = True
                                                                btnSave2.Enabled = True
                                                                btnReset5.Enabled = True
                                                                btnCRProjNoReq.Enabled = False
                                                            ElseIf txtRoutingStatus.Text = "T" Then
                                                                btnSave2.Enabled = True
                                                                btnReset5.Enabled = True
                                                                btnReset6.Enabled = True
                                                                btnUpload.Enabled = True
                                                                uploadFile.Enabled = True
                                                                gvSupportingDocument.Columns(4).Visible = True
                                                            End If
                                                            ddProjectStatus2.Enabled = True
                                                            AEExtender.Collapsed = True
                                                        Case "Approved"
                                                            txtNextEstCmpltDt.Enabled = True
                                                            AEExtender.Collapsed = True
                                                            SDExtender.Collapsed = True
                                                            btnReset6.Enabled = True
                                                            btnUpload.Enabled = True
                                                            uploadFile.Enabled = True
                                                            gvSupportingDocument.Columns(4).Visible = True
                                                            btnSave2.Enabled = True
                                                            btnReset5.Enabled = True
                                                            btnCRProjNoReq.Enabled = False
                                                            ddProjectStatus.Enabled = True
                                                        Case "Capitalized"
                                                            If txtRoutingStatus.Text <> "C" Then
                                                                txtActualCost.Enabled = True
                                                                txtCustomerCost.Enabled = True
                                                                txtClosingNotes.Enabled = True
                                                            End If
                                                            AEExtender.Collapsed = True
                                                            SDExtender.Collapsed = True
                                                            btnCRProjNoReq.Enabled = False
                                                        Case "Completed"
                                                            ddProjectStatus.Enabled = True
                                                            txtNextEstCmpltDt.Enabled = True
                                                            AEExtender.Collapsed = True
                                                            SDExtender.Collapsed = True
                                                            txtActualCost.Enabled = True
                                                            txtCustomerCost.Enabled = True
                                                            txtClosingNotes.Enabled = True
                                                            btnCRProjNoReq.Enabled = False
                                                        Case "Void"
                                                            If txtRoutingStatus.Text <> "V" Then
                                                                btnSave1.Enabled = True
                                                                btnReset1.Enabled = True
                                                            End If
                                                            AEExtender.Collapsed = True
                                                            SDExtender.Collapsed = True
                                                            txtVoidReason.Enabled = True
                                                            txtVoidReason.Visible = True
                                                            lblVoidRsn.Visible = True
                                                            lblReqVoidRsn.Visible = True
                                                            btnCRProjNoReq.Enabled = False
                                                            ddProjectStatus.Enabled = True
                                                        Case "Hold"
                                                            ddProjectStatus2.Enabled = True
                                                            btnCRProjNoReq.Enabled = False
                                                    End Select
                                                    If (txtRoutingStatus.Text <> "N") And (txtRoutingStatus.Text <> "R") And (txtRoutingStatus.Text <> "T") And (txtRoutingStatus.Text <> "H") Then
                                                        ddProjectStatus.Enabled = True
                                                        btnAppend.Enabled = True
                                                    End If
                                                    If txtRoutingStatus.Text = "N" And txtRoutingStatus.Text = "H" Then
                                                        ddProjectStatus.Enabled = False
                                                        btnDelete.Enabled = True
                                                    End If
                                                    btnPreview.Enabled = True
                                                    mnuTabs.Items(1).Enabled = True
                                                    mnuTabs.Items(2).Enabled = True
                                                    mnuTabs.Items(3).Enabled = True
                                                    txtEstCmpltDt.Enabled = True
                                                End If
                                            End If 'EOF  ViewState("iTeamMemberID") = ddProjectLeader.SelectedValue 
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            '** Used by Approvers & Backup persons
                                            If ViewState("pProjNo") = Nothing Or ViewState("pProjNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                txtProjectTitle.Focus()
                                                btnCRProjNoReq.Enabled = False
                                            Else
                                                ViewState("ObjectRole") = False
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                btnPreview.Enabled = True
                                                btnSave2.Enabled = True
                                                btnReset5.Enabled = True
                                                Select Case ProjectStatus
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "T") Then
                                                            AEExtender.Collapsed = True
                                                            gvApprovers.Columns(7).Visible = True
                                                            btnReset6.Enabled = False
                                                            btnUpload.Enabled = False
                                                            uploadFile.Enabled = False
                                                            gvSupportingDocument.Columns(4).Visible = False
                                                            btnCRProjNoReq.Enabled = False
                                                        End If
                                                End Select
                                            End If
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            ViewState("ObjectRole") = False
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                            mnuTabs.Items(3).Enabled = True
                                            btnPreview.Enabled = True
                                            SDExtender.Collapsed = True
                                            AEExtender.Collapsed = True
                                            btnCRProjNoReq.Enabled = False
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ''Used by the Accounting Group
                                            ViewState("ObjectRole") = False
                                            ViewState("Admin") = True
                                            btnCRProjNoReq.Enabled = False
                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pProjNo") = Nothing Or ViewState("pProjNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                txtProjectTitle.Focus()
                                            Else
                                                Select Case ProjectStatus
                                                    Case "In Process"
                                                        AEExtender.Collapsed = True
                                                        gvApprovers.Columns(9).Visible = True
                                                        ddProjectStatus2.Enabled = True
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        btnSave3.Enabled = True
                                                        btnReset3.Enabled = True
                                                        btnReset4.Enabled = True
                                                        btnSave2.Enabled = True
                                                        btnReset5.Enabled = True
                                                        btnReset6.Enabled = True
                                                        btnAddtoGrid2.Enabled = True
                                                        btnCRProjNoReq.Enabled = True

                                                    Case "Void"
                                                        If txtRoutingStatus.Text <> "V" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                        End If
                                                        SDExtender.Collapsed = True
                                                        AEExtender.Collapsed = True
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidRsn.Visible = True
                                                        lblReqVoidRsn.Visible = True
                                                        ddProjectStatus.Enabled = True
                                                    Case "Approved"
                                                        SDExtender.Collapsed = True
                                                        AEExtender.Collapsed = True
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        ddProjectStatus.Enabled = True
                                                        btnSave2.Enabled = True
                                                        btnReset5.Enabled = True
                                                    Case "Capitalized"
                                                        SDExtender.Collapsed = True
                                                        AEExtender.Collapsed = True
                                                        If txtRoutingStatus.Text <> "C" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            txtActualCost.Enabled = True
                                                            txtCustomerCost.Enabled = True
                                                            txtClosingNotes.Enabled = True
                                                        End If
                                                        lblActualCost.Visible = True
                                                        lblCustomerCost.Visible = True
                                                        lblClosingNts.Visible = True
                                                        lblReqCustomerCost.Visible = True
                                                        lblReqClosingNts.Visible = True
                                                        lblReqActualCost.Visible = True
                                                    Case "Completed"
                                                        SDExtender.Collapsed = True
                                                        AEExtender.Collapsed = True
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        txtActualCost.Enabled = True
                                                        txtCustomerCost.Enabled = True
                                                        txtClosingNotes.Enabled = True
                                                        txtActualCost.Visible = True
                                                        txtCustomerCost.Visible = True
                                                        txtClosingNotes.Visible = True
                                                        lblActualCost.Visible = True
                                                        lblCustomerCost.Visible = True
                                                        lblClosingNts.Visible = True
                                                        lblReqCustomerCost.Visible = True
                                                        lblReqClosingNts.Visible = True
                                                        lblReqActualCost.Visible = True
                                                        ddProjectStatus.Enabled = True
                                                End Select
                                                If (txtRoutingStatus.Text <> "N") And (txtRoutingStatus.Text <> "R") And (txtRoutingStatus.Text <> "T") Then
                                                    btnAppend.Enabled = True
                                                End If
                                                btnAdd.Enabled = True
                                                gvSupportingDocument.Columns(4).Visible = True
                                                gvApprovers.Columns(9).Visible = False
                                                uploadFile.Enabled = True
                                                btnReset6.Enabled = True
                                                btnUpload.Enabled = True
                                                btnPreview.Enabled = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                            End If
                                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                            ''** No Entry allowed **''
                                    End Select 'EOF of "Select Case iRoleID"
                                Next
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub
#End Region 'EOF Form Level Security

#Region "General - Project Detail"
    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Project Leader control for selection criteria for search
            ds = commonFunctions.GetTeamMember("") '**SubscriptionID # is used for Project Leader
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddProjectLeader.DataSource = ds
                ddProjectLeader.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddProjectLeader.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddProjectLeader.DataBind()
                ddProjectLeader.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Team Member control for selection criteria for search
            ds = commonFunctions.GetTeamMember("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddTeamMember.DataSource = ds
                ddTeamMember.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddTeamMember.DataBind()
                ddTeamMember.Items.Insert(0, "")
                ddTeamMember.Enabled = False
            End If

            commonFunctions.UserInfo()
            ddProjectLeader.SelectedValue = HttpContext.Current.Session("UserId")
            ddTeamMember.SelectedValue = HttpContext.Current.Session("UserId")

            ''bind existing data to drop down Line # control for selection criteria for search
            ds = EXPModule.GetExpProjRepairExpenditure(ViewState("pProjNo"), 0)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddLineNo.DataSource = ds
                ddLineNo.DataTextField = ds.Tables(0).Columns("EID").ColumnName.ToString()
                ddLineNo.DataValueField = ds.Tables(0).Columns("EID").ColumnName.ToString()
                ddLineNo.DataBind()
                ddLineNo.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Line # control for selection criteria for search
            ds = EXPModule.GetCostReductionList(IIf(ddUGNLocation.SelectedValue = "", "", ddUGNLocation.SelectedValue))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddCRProjNo.DataSource = ds
                ddCRProjNo.DataTextField = ds.Tables(0).Columns("ddProjNoDesc").ColumnName.ToString()
                ddCRProjNo.DataValueField = ds.Tables(0).Columns("ProjectNo").ColumnName.ToString()
                ddCRProjNo.DataBind()
                ddCRProjNo.Items.Insert(0, "")
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindCriteria

    Public Sub BindData(ByVal ProjNo As String)
        Dim ds As DataSet = New DataSet
        Dim ds2 As DataSet = New DataSet

        Try
            If ProjNo <> Nothing Then
                ds = EXPModule.GetExpProjRepair(ProjNo, "", "", "", 0, "", "")
                If commonFunctions.CheckDataSet(ds) = True Then
                    If ViewState("pPrntProjNo") = Nothing Then
                        'lblProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                        'Select Case ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                        '    Case "N"
                        '        ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        '    Case "A"
                        '        ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        '    Case "C"
                        '        ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        '    Case "H"
                        '        ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        '    Case "T"
                        '        ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        '    Case "S"
                        '        ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        '    Case "R"
                        '        ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        '    Case "V"
                        '        ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        'End Select
                        'txtRoutingStatus.Text = ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                        'If ddProjectStatus.SelectedValue <> "Void" Then
                        '    lblRoutingStatusDesc.Text = ds.Tables(0).Rows(0).Item("RoutingStatusDesc").ToString()
                        'End If
                        'lblRoutingStatusDesc.Visible = True
                    Else
                        If ViewState("pProjNo") = Nothing Then
                            lblProjectID.Text = ProjNo & "?"
                            ddProjectStatus.SelectedValue = "Open"
                            lblPrntProjNo.Text = ProjNo

                            lblPrntAppDate.Text = IIf(ds.Tables(0).Rows(0).Item("OriginalApprovedDt").ToString() = "01/01/1900", "", ds.Tables(0).Rows(0).Item("OriginalApprovedDt").ToString())
                        Else
                            'lblProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                            'ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                            'txtRoutingStatus.Text = ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                            'If ddProjectStatus.SelectedValue <> "Void" Then
                            '    lblRoutingStatusDesc.Text = ds.Tables(0).Rows(0).Item("RoutingStatusDesc").ToString()
                            'End If

                            'lblRoutingStatusDesc.Visible = True
                            lblPrntProjNo.Text = ds.Tables(0).Rows(0).Item("ParentProjectNo").ToString()
                            lblPrntAppDate.Text = ds.Tables(0).Rows(0).Item("OriginalApprovedDt").ToString()
                        End If
                    End If

                    lblProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                    Select Case ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                        Case "N"
                            ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        Case "A"
                            ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        Case "C"
                            ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        Case "H"
                            ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        Case "T"
                            ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        Case "S"
                            ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        Case "R"
                            ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        Case "V"
                            ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                    End Select
                    txtRoutingStatus.Text = ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                    If ddProjectStatus.SelectedValue <> "Void" Then
                        lblRoutingStatusDesc.Text = ds.Tables(0).Rows(0).Item("RoutingStatusDesc").ToString()
                    End If
                    lblRoutingStatusDesc.Visible = True

                    txtProjectTitle.Text = ds.Tables(0).Rows(0).Item("ProjectTitle").ToString()
                    cddUGNLocation.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                    cddDepartment.SelectedValue = ds.Tables(0).Rows(0).Item("DeptOrCostCenter").ToString()

                    If cddDepartment.SelectedValue = Nothing Then
                        rfvDCC.Enabled = True
                    Else
                        rfvDCC.Enabled = False
                    End If

                    ddProjectLeader.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectLeaderTMID").ToString()
                    txtProjDateNotes.Text = ds.Tables(0).Rows(0).Item("ProjDtNotes").ToString()
                    txtJustification.Text = ds.Tables(0).Rows(0).Item("Justification").ToString()
                    txtDateSubmitted.Text = ds.Tables(0).Rows(0).Item("DateSubmitted").ToString()
                    txtEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDt").ToString()
                    txtHDEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDt").ToString()
                    txtNextEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDt").ToString()
                    txtEstSpendDt.Text = ds.Tables(0).Rows(0).Item("EstSpendDt").ToString()
                    txtEstEndSpendDt.Text = ds.Tables(0).Rows(0).Item("EstEndSpendDt").ToString()
                    txtActualCost.Text = ds.Tables(0).Rows(0).Item("ActualCost").ToString()
                    txtCustomerCost.Text = ds.Tables(0).Rows(0).Item("CustomerCost").ToString()
                    txtClosingNotes.Text = ds.Tables(0).Rows(0).Item("ClosingNotes").ToString()
                    txtVoidReason.Text = ds.Tables(0).Rows(0).Item("VoidReason").ToString()

                    txtLessRtrdEqVal.Text = ds.Tables(0).Rows(0).Item("RtdEqpValue").ToString()
                    txtWorkingCapital.Text = ds.Tables(0).Rows(0).Item("WorkingCapital").ToString()
                    txtStartupExpense.Text = ds.Tables(0).Rows(0).Item("StartupExpense").ToString()
                    txtCustReimb.Text = ds.Tables(0).Rows(0).Item("CustReimb").ToString()
                    cbNotRequired.Checked = ds.Tables(0).Rows(0).Item("NotRequired").ToString()
                    cbCRProjNoReq.Checked = ds.Tables(0).Rows(0).Item("CRProjectNoRequested").ToString()

                    If cbCRProjNoReq.Checked = True Then
                        cbCRProjNoReq.Text = "Submitted"
                    End If

                    If ds.Tables(0).Rows(0).Item("CRProjectNo").ToString() <> 0 And ds.Tables(0).Rows(0).Item("CRProjectNo").ToString() <> Nothing Then
                        ddCRProjNo.SelectedValue = ds.Tables(0).Rows(0).Item("CRProjectNo").ToString()
                    Else
                        ds2 = EXPModule.GetCostReductionList(IIf(ddUGNLocation.SelectedValue = "", "", ddUGNLocation.SelectedValue))
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            ddCRProjNo.DataSource = ds2
                            ddCRProjNo.DataTextField = ds2.Tables(0).Columns("ddProjNoDesc").ColumnName.ToString()
                            ddCRProjNo.DataValueField = ds2.Tables(0).Columns("ProjectNo").ColumnName.ToString()
                            ddCRProjNo.DataBind()
                            ddCRProjNo.Items.Insert(0, "")
                        End If
                    End If

                    lblSubtotalRepair.Text = Format(ds.Tables(0).Rows(0).Item("TotalInv"), "#,###.00")
                    lblTotalInvestment.Text = Format(((lblSubtotalRepair.Text - txtLessRtrdEqVal.Text) + txtWorkingCapital.Text), "#,###.00")
                    lblTotalInvestment1.Text = Format(((lblSubtotalRepair.Text - txtLessRtrdEqVal.Text) + txtWorkingCapital.Text), "#,###.00")
                    txtHDTotalInvestment.Text = Format(ds.Tables(0).Rows(0).Item("OrigTotalInv"), "#,##0.00")
                    cbProjectInLatestForecast.Checked = ds.Tables(0).Rows(0).Item("ProjectInLatestForecast").ToString()
                    txtRepairSavings.Text = ds.Tables(0).Rows(0).Item("RepairSavings").ToString()
                    txtScrapSavings.Text = ds.Tables(0).Rows(0).Item("ScrapSavings").ToString()
                    txtConsumableSavings.Text = ds.Tables(0).Rows(0).Item("ConsumableSavings").ToString()
                    txtLaborSavings.Text = ds.Tables(0).Rows(0).Item("LaborSavings").ToString()
                    txtOtherSavings.Text = ds.Tables(0).Rows(0).Item("OtherSavings").ToString()

                End If

                ''Bind Expenses
                If ViewState("pEID") <> 0 Then
                    ds = EXPModule.GetExpProjRepairExpenditure(ViewState("pProjNo"), ViewState("pEID"))
                    If commonFunctions.CheckDataSet(ds) = True Then
                        txtDescription.Text = ds.Tables(0).Rows(0).Item("Description").ToString()
                        txtQuantity.Text = ds.Tables(0).Rows(0).Item("Quantity").ToString()
                        txtAmountPer.Text = ds.Tables(0).Rows(0).Item("Amount").ToString()
                        txtNotes.Text = ds.Tables(0).Rows(0).Item("Notes").ToString()
                    Else 'no record found reset query string pRptID
                        Response.Redirect("RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pEID=0", False)
                    End If
                End If

                ''Bind Communication Board
                If ViewState("pRID") <> 0 Then
                    ds = EXPModule.GetRepairExpProjRSS(ViewState("pProjNo"), ViewState("pRID"))
                    If commonFunctions.CheckDataSet(ds) = True Then
                        txtQC.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                    Else 'no record found reset query string pRptID
                        Response.Redirect("RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pRID=0&pRC=1", False)
                    End If
                End If
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindData()

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("RepairExpProj.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnAppend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAppend.Click
        Response.Redirect("RepairExpProj.aspx?pProjNo=&pPrntProjNo=" & ViewState("pProjNo"), False)
    End Sub 'EOF btnAppend_Click

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click, btnSave3.Click
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            lblErrors.Text = Nothing
            lblErrors.Visible = False

            Dim UGNLocation As String = commonFunctions.GetCCDValue(cddUGNLocation.SelectedValue)
            Dim Department As String = commonFunctions.GetCCDValue(cddDepartment.SelectedValue)

            If (ViewState("pProjNo") <> Nothing Or ViewState("pProjNo") <> "") Then
                '***************
                '* Update Data
                '***************
                UpdateRecord(ViewState("ProjectStatus"), IIf(ViewState("ProjectStatus") = "Capitalized", "C", IIf(ViewState("ProjectStatus") = "Void", "V", IIf(ViewState("ProjectStatus") = "Open", "N", IIf(ViewState("ProjectStatus") = "Approved", "A", txtRoutingStatus.Text)))), False)

                '**************
                '* Reload the data - may contain calculated information to TotalInv
                '**************
                BindData(ViewState("pProjNo"))

                ''*************
                ''Check for Capitalized, Completed & Void status, send email notfication 
                ''*************
                If ddProjectStatus.SelectedValue = "Capitalized" And txtRoutingStatus.Text = "C" Then
                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Capitalized", "", "", "", "")
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Capitalized")
                    End If
                ElseIf ddProjectStatus.SelectedValue = "Completed" Then
                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Completed", "", "", "", "")
                    If txtRoutingStatus.Text <> "N" And (txtHDEstCmpltDt.Text = txtNextEstCmpltDt.Text) Then
                        SendNotifWhenEventChanges("Completed")
                    End If
                ElseIf ddProjectStatus.SelectedValue = "Void" And txtRoutingStatus.Text = "V" Then
                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "VOID - Reason:" & txtVoidReason.Text, "", "", "", "")
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Void")
                    End If
                End If
            Else 'New Record
                Dim NewTestReq As Boolean = False
                Dim Consult As Boolean = False
                Dim Current As Boolean = False

                '***************
                '* Locate Next available ProjectNo based on Facility selection
                '***************
                Dim ds As DataSet = Nothing
                ds = EXPModule.GetNextExpProjectNo(ViewState("pPrntProjNo"), UGNLocation, "Repair")

                ViewState("pProjNo") = CType(ds.Tables(0).Rows(0).Item("NextAvailProjNo").ToString, String)

                '***************
                '* Save Data
                '***************
                EXPModule.InsertExpProjRepair(ViewState("pProjNo"), ViewState("pPrntProjNo"), txtProjectTitle.Text, "Open", UGNLocation, ddProjectLeader.SelectedValue, txtProjDateNotes.Text, txtJustification.Text, "N", txtDateSubmitted.Text, txtEstCmpltDt.Text, txtEstSpendDt.Text, txtEstEndSpendDt.Text, lblPrntAppDate.Text, Department, cbProjectInLatestForecast.Checked, DefaultUser, DefaultDate)

                ''*****************
                ''History Tracking
                ''*****************
                EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Record created.", "", "", "", "")

                ''*******************
                ''Build Approval List
                ''*******************
                BuildApprovalList()

                '***************
                '* Redirect user back to the page.
                '***************
                Dim Aprv As String = Nothing
                If ViewState("pAprv") = 1 Then
                    Aprv = "&pAprv=1"
                End If
                Response.Redirect("RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pPrntProjNo=" & ViewState("pPrntProjNo") & Aprv, False)
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnSave1_Click

    Public Function UpdateRecord(ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal RecSubmitted As Boolean) As String
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim UGNLocation As String = commonFunctions.GetCCDValue(cddUGNLocation.SelectedValue)
            Dim Department As String = commonFunctions.GetCCDValue(cddDepartment.SelectedValue)

            Dim EstCmpltDt As String = txtHDEstCmpltDt.Text
            Dim SendEmailToDefaultAdmin As Boolean = False

            '************************************
            '* Capture Imp. Date Change History
            '************************************ 
            If CType(txtHDEstCmpltDt.Text, Date) <> CType(txtNextEstCmpltDt.Text, Date) Then
                If txtDateSubmitted.Text <> Nothing Then
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Estimated Completion Date Changed")
                    End If
                End If
                ''Assign EstImpDate with new value.
                EstCmpltDt = txtNextEstCmpltDt.Text
                lblReqEstCmpltDtChange.Visible = False
                lblEstCmpltDtChange.Visible = False
                txtEstCmpltDtChngRsn.Visible = False
            End If

            '***************
            '* Update Data
            '***************
            EXPModule.UpdateExpProjRepair(ViewState("pProjNo"), txtProjectTitle.Text, ProjectStatus, UGNLocation, ddProjectLeader.SelectedValue, txtProjDateNotes.Text, txtJustification.Text, "", txtDateSubmitted.Text, EstCmpltDt, txtEstSpendDt.Text, txtEstEndSpendDt.Text, RoutingStatus, txtActualCost.Text, txtCustomerCost.Text, txtClosingNotes.Text, txtVoidReason.Text, Department, IIf(txtLessRtrdEqVal.Text = "", 0, txtLessRtrdEqVal.Text), IIf(txtWorkingCapital.Text = "", 0, txtWorkingCapital.Text), IIf(txtStartupExpense.Text = "", 0, txtStartupExpense.Text), IIf(txtCustReimb.Text = "", 0, txtCustReimb.Text), cbNotRequired.Checked, cbProjectInLatestForecast.Checked, IIf(txtRepairSavings.Text = "", 0, txtRepairSavings.Text), IIf(txtScrapSavings.Text = "", 0, txtScrapSavings.Text), IIf(txtConsumableSavings.Text = "", 0, txtConsumableSavings.Text), IIf(txtLaborSavings.Text = "", 0, txtLaborSavings.Text), IIf(txtOtherSavings.Text = "", 0, txtOtherSavings.Text), IIf(ddCRProjNo.SelectedValue = "", 0, ddCRProjNo.SelectedValue), cbCRProjNoReq.Checked, DefaultUser, DefaultDate)

            Return True

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            Return False
        End Try
    End Function 'EOF UpdateRecord

    Protected Sub btnReset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset1.Click, btnReset4.Click, btnReset5.Click, btnReset6.Click

        'Redirect user to the same tab view when Reset button was clicked
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If
        If ViewState("pProjNo") <> "" Then
            If ViewState("pEID") > 0 Then
                Response.Redirect("RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pEV=1" & Aprv, False)
            ElseIf ViewState("pRID") > 0 Then
                Response.Redirect("RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pRC=1" & Aprv, False)
            ElseIf ViewState("pSD") > 0 Then
                Response.Redirect("RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pSD=1" & Aprv, False)
            Else
                Response.Redirect("RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & Aprv, False)
            End If
        Else
            Response.Redirect("RepairExpProj.aspx", False)
        End If
    End Sub 'EOF btnReset1_Click

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            '***************
            '* Delete Record
            '***************
            If ViewState("pPrntProjNo") = Nothing Then
                EXPModule.DeleteExpProjRepair(ViewState("pProjNo"), ViewState("pPrntProjNo"), False)
            Else
                EXPModule.DeleteExpProjRepair(ViewState("pProjNo"), ViewState("pPrntProjNo"), True)
            End If

            '***************
            '* Redirect user back to the search page.
            '***************
            Response.Redirect("RepairExpProjList.aspx", False)

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnDelete_Click

    Protected Sub ddProjectStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProjectStatus.SelectedIndexChanged
        Select Case ddProjectStatus.SelectedValue
            Case "Capitalized"
                txtActualCost.Visible = True
                txtCustomerCost.Visible = True
                txtClosingNotes.Visible = True
                lblReqActualCost.Visible = True
                lblActualCost.Visible = True
                lblCustomerCost.Visible = True
                lblReqCustomerCost.Visible = True
                lblReqClosingNts.Visible = True
                lblClosingNts.Visible = True
                rfvClostingNotes.Enabled = True
                rfvActualCost.Enabled = True
                rfvCustomerCost.Enabled = True
                txtActualCost.Enabled = True
                txtCustomerCost.Enabled = True
                txtActualCost.Text = Nothing
                txtCustomerCost.Text = Nothing
                txtClosingNotes.Enabled = True
                txtVoidReason.Visible = False
                lblReqVoidRsn.Visible = False
                lblVoidRsn.Visible = False
                rfvVoidReason.Enabled = False
            Case "Void"
                lblReqActualCost.Visible = False
                lblReqCustomerCost.Visible = False
                lblReqClosingNts.Visible = False
                lblActualCost.Visible = True
                lblCustomerCost.Visible = True
                lblClosingNts.Visible = True
                txtActualCost.Visible = True
                txtCustomerCost.Visible = True
                txtClosingNotes.Visible = True
                txtActualCost.Enabled = False
                txtCustomerCost.Enabled = False
                txtClosingNotes.Enabled = False
                txtVoidReason.Visible = True
                lblReqVoidRsn.Visible = True
                lblVoidRsn.Visible = True
                rfvClostingNotes.Enabled = False
                rfvVoidReason.Enabled = True
            Case Else
                txtActualCost.Visible = False
                txtCustomerCost.Visible = False
                txtClosingNotes.Visible = False
                txtVoidReason.Visible = False
                lblReqActualCost.Visible = False
                lblActualCost.Visible = False
                lblCustomerCost.Visible = False
                lblReqCustomerCost.Visible = False
                lblReqClosingNts.Visible = False
                lblClosingNts.Visible = False
                lblReqVoidRsn.Visible = False
                lblVoidRsn.Visible = False
                rfvClostingNotes.Enabled = False
                rfvVoidReason.Enabled = False
        End Select
    End Sub 'EOF ddProjectStatus_SelectedIndexChanged

    Protected Sub txtNextEstCmpltDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNextEstCmpltDt.TextChanged
        Try

            If CType(txtHDEstCmpltDt.Text, Date) <> CType(txtNextEstCmpltDt.Text, Date) Then
                rfvEstCmpltDt.Enabled = False
                lblReqEstCmpltDtChange.Visible = True
                lblEstCmpltDtChange.Visible = True
                txtEstCmpltDtChngRsn.Visible = True
                txtEstCmpltDtChngRsn.Focus()
            Else
                rfvEstCmpltDt.Enabled = True
                lblReqEstCmpltDtChange.Visible = False
                lblEstCmpltDtChange.Visible = False
                txtEstCmpltDtChngRsn.Visible = False
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br/>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF txtNextEstCmpltDt_TextChanged

    Protected Sub ddCRProjNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCRProjNo.SelectedIndexChanged
        If ddCRProjNo.SelectedValue = Nothing Then
            btnCRProjNoReq.Enabled = False
        Else
            btnCRProjNoReq.Enabled = True
        End If

        ''*************************************************
        '' "Form Level Security using Roles &/or Subscriptions"
        ''*************************************************
        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

    End Sub 'EOF ddCRProjNo_SelectedIndexChanged
#End Region 'EOF "General - Project Detail"

#Region "Repair Expense"
    Protected Sub btnAddtoGrid2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddtoGrid2.Click
        Try
            If ViewState("pProjNo") <> Nothing Then
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

                If ViewState("pEID") = 0 Or ViewState("pEID") = Nothing Then
                    '***************
                    '* Insert Expense information to table
                    '***************
                    EXPModule.InsertExpProjRepairExpenditure(ViewState("pProjNo"), 0, "", txtDescription.Text, IIf(txtQuantity.Text = "", 0, txtQuantity.Text), IIf(txtAmountPer.Text = "", 0, txtAmountPer.Text), txtNotes.Text, DefaultUser)

                    txtDescription.Text = Nothing
                    txtQuantity.Text = Nothing
                    txtAmountPer.Text = Nothing
                    txtNotes.Text = Nothing
                Else
                    '***************
                    '* Update Expense information to table
                    '***************
                    EXPModule.UpdateExpProjRepairExpenditure(ViewState("pEID"), ViewState("pProjNo"), 0, "", txtDescription.Text, IIf(txtQuantity.Text = "", 0, txtQuantity.Text), IIf(txtAmountPer.Text = "", 0, txtAmountPer.Text), txtNotes.Text, DefaultUser)

                    Response.Redirect("RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pEV=1", False)
                End If

                gvExpense.DataBind()
                BindCriteria()
                BindData(ViewState("pProjNo"))

            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnAddtoGrid2_Click

    Protected Sub btnReset3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset3.Click

        'Redirect user to the same tab view when Reset button was clicked
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If

        Response.Redirect("RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pEV=1" & Aprv, False)

    End Sub 'EOF btnReset3_Click

    Protected Sub gvExpense_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExpense.RowCreated
        ''Do nothing
    End Sub 'EOF gvExpense_RowCreated

    Protected Sub gvExpense_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExpense.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(6).Controls(1), ImageButton)


                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As ExpProjRepair.ExpProj_Repair_ExpenditureRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProjRepair.ExpProj_Repair_ExpenditureRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record for " & DataBinder.Eval(e.Row.DataItem, "Description") & "?');")
                End If
            End If
        End If

        ''**************************************************************************************
        ''Reload data - When a delete occurs, it will recalc the TotalInv & Profit/Loss fields.
        ''**************************************************************************************
        BindData(ViewState("pProjNo"))

    End Sub 'EOF gvExpense_RowDataBound

#End Region 'EOF "Asset Expense"

#Region "Communication Board"
    Public Function GoToCommunicationBoard(ByVal ProjectNo As String, ByVal RSSID As String, ByVal ApprovalLevel As Integer, ByVal TeamMemberID As Integer) As String
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If
        Return "RepairExpProj.aspx?pProjNo=" & ProjectNo & "&pAL=" & ApprovalLevel & "&pTMID=" & TeamMemberID & "&pRID=" & RSSID & "&pRC=1" & Aprv
    End Function 'EOF GoToCommunicationBoard

    Protected Sub btnSave2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave2.Click
        Try
            ''************************************
            ''Send response back to requestor
            ''************************************
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value
            Dim SeqNo As Integer = IIf(HttpContext.Current.Request.QueryString("pAL") = "", 0, HttpContext.Current.Request.QueryString("pAL"))
            Dim TMID As Integer = IIf(HttpContext.Current.Request.QueryString("pTMID") = "", 0, HttpContext.Current.Request.QueryString("pTMID"))

            Dim ds As DataSet = New DataSet
            Dim dsCC As DataSet = New DataSet
            Dim ds2CC As DataSet = New DataSet

            Dim EmailTO As String = Nothing
            Dim EmpName As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing
            Dim i As Integer = 0

            Dim CurrentEmpEmail As String = Nothing
            If HttpContext.Current.Request.Cookies("UGNDB_User_Email") IsNot Nothing Then
                CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailFrom = CurrentEmpEmail
                EmailCC = CurrentEmpEmail & ";"
            Else
                CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                EmailFrom = "Database.Notifications@ugnauto.com"
            End If

            lblErrors.Text = Nothing
            lblErrors.Visible = False

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then
                ''*************************************************************************
                ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                ''*************************************************************************
                Dim dsExp As DataSet = New DataSet

                ''***************************************************************
                ''Send Reply back to requestor
                ''***************************************************************
                ds = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), 0, TMID, False, False) '
                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(ds) = True Then
                    For i = 0 To ds.Tables.Item(0).Rows.Count - 1
                        If (ds.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                        (ds.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                            EmailTO &= ds.Tables(0).Rows(i).Item("Email") & ";"
                            EmpName &= ds.Tables(0).Rows(i).Item("EmailTMName") & ", "

                        End If
                    Next
                End If 'EOF  If ds.Tables.Count > 0

                ''********************************************************
                ''Send Notification only if there is a valid Email Address
                ''********************************************************
                If EmailTO <> Nothing Then
                    Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                    ''********************************
                    ''Carbon Copy Project Leader
                    ''********************************
                    EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                    ''***************************************************************
                    ''Carbon Copy Previous Levels
                    ''***************************************************************
                    ds2CC = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), SeqNo, TMID, False, False)
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(ds2CC) = True Then
                        For i = 0 To ds2CC.Tables(0).Rows.Count - 1
                            If (ds2CC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                            (ds2CC.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                EmailCC &= ds2CC.Tables(0).Rows(i).Item("Email") & ";"

                            End If
                        Next
                    End If 'EOF  If ds.Tables.Count > 0

                    ''Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                    Else
                        MyMessage.Subject = ""
                        MyMessage.Body = ""
                        ' MyMessage.Body = "THIS IS A TEST. DATA IS NOT VALID FOR USE<br/>"
                    End If

                    MyMessage.Subject &= "R Project: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text & " - MESSAGE RECIEVED"
                    MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
                    MyMessage.Body &= " <tr>"
                    MyMessage.Body &= "     <td valign='top' width='20%'>"
                    MyMessage.Body &= "         <img src='" & ViewState("strProdOrTestEnvironment") & "/images/messanger60.jpg'/>"
                    MyMessage.Body &= "     </td>"
                    MyMessage.Body &= "     <td valign='top'>"
                    MyMessage.Body &= "         <b>Attention:</b> " & EmpName
                    MyMessage.Body &= "         <p><b>" & DefaultUserFullName & "</b> replied to your message regarding "
                    MyMessage.Body &= "         <font color='red'>" & ViewState("pProjNo") & " - " & txtProjectTitle.Text & "</font>."
                    MyMessage.Body &= "         <br/><br/><i>Question:&nbsp;&nbsp;</i><b>" & txtQC.Text & "</b>"
                    MyMessage.Body &= "         <br/><br/><i>Response:&nbsp;&nbsp;</i><b>" & txtReply.Text & "</b><br/><br/>"

                    MyMessage.Body &= "         </p>"
                    MyMessage.Body &= "         <p><a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crExpProjRepairApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to return to the approval page."
                    MyMessage.Body &= "     </td>"
                    MyMessage.Body &= " </tr>"
                    MyMessage.Body &= "</table>"
                    MyMessage.Body &= "<br/><br/>"


                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                        MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                        EmailFrom = "Database.Notifications@ugnauto.com"
                        EmailTO = "lynette.rey@ugnauto.com"
                        EmailCC = "lynette.rey@ugnauto.com"
                    End If

                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Message Sent", "", "", "", "")

                    ''**********************************
                    ''Save Reponse to child table
                    ''**********************************
                    EXPModule.InsertExpProjRepairRSSReply(ViewState("pProjNo"), ViewState("pRID"), txtProjectTitle.Text, DefaultTMID, txtReply.Text)

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (R)", ViewState("pProjNo"))
                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                    Catch ex As Exception
                        lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."

                        UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, CurrentEmpEmail & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
                        'get current event name
                        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                        'log and email error
                        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                    End Try
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False

                    gvQuestion.DataBind()
                    mvTabs.ActiveViewIndex = Int32.Parse(3)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(3).Selected = True

                Else 'EmailTO = ''
                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pProjNo"))

                    lblErrors.Text = "Unable to locate a valid email address to send message to. Please contact the Application Department for assistance."
                    lblErrors.Visible = True
                End If 'EOF EmailTO <> ''
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnSave2_Click

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim RSSID As Integer
            Dim drRSSID As ExpProjRepair.ExpProj_Repair_RSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, ExpProjRepair.ExpProj_Repair_RSSRow)

            If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                RSSID = drRSSID.RSSID
                ' Reference the rpCBRC ObjectDataSource
                Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                ' Set the CategoryID Parameter value
                rpCBRC.SelectParameters("ProjectNo").DefaultValue = drRSSID.ProjectNo.ToString()
                rpCBRC.SelectParameters("RSSID").DefaultValue = drRSSID.RSSID.ToString()
            End If
        End If
    End Sub 'EOF gvQuestion_RowDataBound

#End Region 'EOF "Communication Board"

#Region "Supporting Documents"
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim DefaultDate As Date = Date.Now
            lblMessageView4.Text = Nothing
            lblMessageView4.Visible = False

            If ViewState("pProjNo") <> "" Then
                If uploadFile.HasFile Then
                    If uploadFile.PostedFile.ContentLength <= 3500000 Then
                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFile.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFile.PostedFile.FileName)

                        '** Original code with use of MS Office 2003 or older **/
                        ' ''Dim BinaryFile(uploadFile.PostedFile.InputStream.Length) As Byte
                        ' ''Dim EncodeType As String = uploadFile.PostedFile.ContentType
                        ' ''uploadFile.PostedFile.InputStream.Read(BinaryFile, 0, BinaryFile.Length)
                        ' ''Dim FileSize As Integer = uploadFile.PostedFile.ContentLength

                        '** With use of MS Office 2007 **/
                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(uploadFile.PostedFile.InputStream.Length)
                        Dim SupportingDocEncodeType As String = uploadFile.PostedFile.ContentType
                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        uploadFile.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".doc") Or (FileExt = ".xlsx") Or (FileExt = ".docx") Then
                            ''*************
                            '' Display File Info
                            ''*************
                            If SupportingDocEncodeType = "application/octet-stream" Then
                                lblMessageView4.Text = "Error with File: " & uploadFile.FileName & " upload. <br/>" & _
                               "Unknown File Type. Please try again.<br/>"
                                lblMessageView4.Visible = True
                                lblMessageView4.Width = 500
                                lblMessageView4.Height = 30

                            Else
                                lblMessageView4.Text = "File name: " & uploadFile.FileName & "<br/>" & _
                              "File Size: " & CType((SupportingDocFileSize / 1024), Integer) & " KB<br/>"
                                lblMessageView4.Visible = True
                                lblMessageView4.Width = 500
                                lblMessageView4.Height = 30

                                ''***************
                                '' Insert Record
                                ''***************
                                EXPModule.InsertExpProjRepairDocuments(ViewState("pProjNo"), ddTeamMember.SelectedValue, txtFileDesc.Text, SupportingDocBinaryFile, uploadFile.FileName, SupportingDocEncodeType, SupportingDocFileSize, IIf(ddLineNo.SelectedValue = "", 0, ddLineNo.SelectedValue), 0, "", "")
                            End If

                            gvSupportingDocument.DataBind()
                            revUploadFile.Enabled = False
                            txtFileDesc.Text = Nothing
                            ddLineNo.SelectedValue = Nothing
                        End If
                    Else
                        lblMessageView4.Text = "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
                        lblMessageView4.Visible = True
                        btnUpload.Enabled = False
                    End If
                End If
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnUpload_Click

    Protected Sub gvSupportingDocument_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSupportingDocument.RowDataBound
        '***
        'This section provides the user with the popup for confirming the delete of a record.
        'Called by the onClientClick event.
        '***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(4).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As ExpProjRepair.ExpProj_Repair_DocumentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProjRepair.ExpProj_Repair_DocumentsRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record (" & DataBinder.Eval(e.Row.DataItem, "Description") & ")?');")
                End If
            End If
        End If
    End Sub 'EOF gvSupportingDocument_RowDataBound

    Protected Sub gvSupportingDocument_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSupportingDocument.RowCommand
        ''***
        ''This section allows the inserting of a new row when save button is clicked from the footer.
        ''***
        If e.CommandName = "Delete" Then
            ''Reprompt current page
            Dim Aprv As String = Nothing
            If ViewState("pAprv") = 1 Then
                Aprv = "&pAprv=1"
            End If
            Response.Redirect("RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pSD=1" & Aprv, False)
        End If
    End Sub 'EOF gvSupportingDocument_RowCommand

    Public Function DisplayImage(ByVal EncodeType As String) As String
        Dim strReturn As String = ""

        If EncodeType = Nothing Then
            strReturn = ""
        ElseIf EncodeType = "application/vnd.ms-excel" Then
            strReturn = "~/images/xls.jpg"
        ElseIf EncodeType = "application/pdf" Then
            strReturn = "~/images/pdf.jpg"
        ElseIf EncodeType = "application/msword" Then
            strReturn = "~/images/doc.jpg"
        ElseIf EncodeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" Then
            strReturn = "~/images/xls.jpg"
        ElseIf EncodeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" Then
            strReturn = "~/images/doc.jpg"
        End If

        Return strReturn
    End Function 'EOF DisplayImage

#End Region 'EOF "Supporting Documents"

#Region "Approval Status"
    Protected Sub gvApprovers_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvApprovers.RowCommand
        Try
            ''***
            ''This section allows the inserting of a new row when save button is clicked from the footer.
            ''***
            If e.CommandName = "Insert" Then
                ''Insert data
                Dim ResponsibleTMID As DropDownList
                Dim OriginalTMID As DropDownList

                If gvApprovers.Rows.Count = 0 Then
                    '' We are inserting through the DetailsView in the EmptyDataTemplate
                    Return
                End If

                '' Only perform the following logic when inserting through the footer
                ResponsibleTMID = CType(gvApprovers.FooterRow.FindControl("ddResponsibleTM"), DropDownList)
                odsApprovers.InsertParameters("ResponsibleTMID").DefaultValue = ResponsibleTMID.SelectedValue

                OriginalTMID = CType(gvApprovers.FooterRow.FindControl("ddResponsibleTM"), DropDownList)
                odsApprovers.InsertParameters("OriginalTMID").DefaultValue = OriginalTMID.SelectedValue

                odsApprovers.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvApprovers.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvApprovers.ShowFooter = True
                Else
                    gvApprovers.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                Dim ResponsibleTMID As DropDownList

                ResponsibleTMID = CType(gvApprovers.FooterRow.FindControl("ddResponsibleTM"), DropDownList)
                ResponsibleTMID.ClearSelection()
                ResponsibleTMID.Items.Add("")
                ResponsibleTMID.SelectedValue = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub 'EOF gvApprovers_RowCommand

    Protected Sub gvApprovers_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Try
            Dim row As GridViewRow = gvApprovers.Rows(e.RowIndex)
            If row IsNot Nothing Then
                lblErrors.Text = Nothing
                lblErrors.Visible = False
                lblReqAppComments.Text = Nothing
                lblReqAppComments.Visible = False

                Dim DefaultDate As Date = Date.Now
                Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                Dim UGNLocation As String = commonFunctions.GetCCDValue(cddUGNLocation.SelectedValue)

                Dim t As DropDownList = TryCast(row.FindControl("ddStatus"), DropDownList)
                Dim c As TextBox = TryCast(row.FindControl("txtAppComments"), TextBox)
                Dim tm As TextBox = TryCast(row.FindControl("txtTeamMemberID"), TextBox)
                Dim TeamMemberID As Integer = CType(tm.Text, Integer)
                Dim otm As TextBox = TryCast(row.FindControl("txtOrigTeamMemberID"), TextBox)
                Dim OrigTeamMemberID As Integer = CType(otm.Text, Integer)
                Dim s As TextBox = TryCast(row.FindControl("hfSeqNo"), TextBox)
                Dim hfSeqNo As Integer = CType(s.Text, Integer)
                Dim ds As DataSet = New DataSet


                If (t.Text <> "Pending") Then
                    If (c.Text <> Nothing Or c.Text <> "") Then
                        ds = SecurityModule.GetTeamMember(IIf(TeamMemberID <> OrigTeamMemberID, OrigTeamMemberID, TeamMemberID), Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                        Dim ShortName As String = ds.Tables(0).Rows(0).Item("ShortName").ToString()

                        ''*****************
                        ''History Tracking
                        ''*****************
                        EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, (DefaultUser.ToUpper) & " " & t.Text & " this record on behalf of " & ShortName.ToUpper & " - Comments: " & c.Text, "", "", "", "")

                        ''********
                        ''* Email sent to the next approvers
                        ''********188 371 510 569
                        Dim ds1st As DataSet = New DataSet
                        Dim ds2nd As DataSet = New DataSet
                        Dim dsCC As DataSet = New DataSet
                        Dim dsRej As DataSet = New DataSet
                        Dim dsCommodity As DataSet = New DataSet
                        Dim EmailTO As String = Nothing
                        Dim EmpName As String = Nothing
                        Dim EmailCC As String = Nothing
                        Dim EmailFrom As String = Nothing
                        Dim SponsSameAs1stLvlAprvr As Boolean = False
                        Dim i As Integer = 0
                        Dim LvlApvlCmplt As Boolean = False

                        Dim CurrentEmpEmail As String = Nothing
                        If HttpContext.Current.Request.Cookies("UGNDB_User_Email") IsNot Nothing Then
                            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                            EmailFrom = CurrentEmpEmail
                            EmailCC = CurrentEmpEmail & ";"
                        Else
                            CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                        End If


                        '********
                        '* Only users with valid email accounts can send an email.
                        '********
                        If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then
                            If t.SelectedValue = "Rejected" And c.Text = Nothing Then
                                lblErrors.Text = "Your comments is required for Rejection."
                                lblErrors.Visible = True
                            Else 'BUILD EMAIL
                                ''*******************************************************************
                                ''*Build Email Notification
                                ''*Verify that atleast one Expense Info entry is entered
                                ''*******************************************************************
                                Dim dsExp As DataSet = New DataSet
                                dsExp = EXPModule.GetExpProjRepairExpenditure(ViewState("pProjNo"), 0)
                                If (dsExp.Tables.Item(0).Rows.Count = 0) Then 'If missing kick user out from submission.
                                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                                    mvTabs.GetActiveView()
                                    mnuTabs.Items(1).Selected = True

                                    rfvDescription.IsValid = False
                                    rfvQuantity.IsValid = False
                                    rfvAmountPer.IsValid = False
                                    vsRepairExpense.ShowSummary = True

                                    lblErrors.Text = "Atleast one Expense entry is required for submission."
                                    lblErrors.Visible = True
                                    lblErrors.Font.Size = 12
                                    Exit Sub
                                Else
                                    ''*****************
                                    ''Declare Variables
                                    ''*****************
                                    Dim SeqNo As Integer = 0
                                    Dim NextSeqNo As Integer = 0
                                    Dim NextLvl As Integer = 0

                                    Select Case hfSeqNo
                                        Case 1
                                            SeqNo = 1
                                            NextSeqNo = 2
                                            NextLvl = 81
                                        Case 2
                                            SeqNo = 2
                                            NextSeqNo = 3
                                            NextLvl = 82
                                        Case 3
                                            SeqNo = 3
                                            NextSeqNo = 0
                                            NextLvl = 83
                                        Case 4
                                            SeqNo = 4
                                            NextSeqNo = 0
                                            NextLvl = 0
                                    End Select

                                    If SeqNo = 4 Then
                                        NextLvl = 83
                                    End If

                                    ''**********************
                                    ''* Save data prior to submission before approvals
                                    ''**********************
                                    UpdateRecord(IIf(SeqNo = 4, IIf(t.SelectedValue = "Rejected", "In Process", "Approved"), ViewState("ProjectStatus")), IIf(SeqNo = 4, IIf(t.SelectedValue = "Rejected", "R", "A"), IIf(t.SelectedValue = "Rejected", "R", "T")), False)

                                    ''***********************************
                                    ''Update Current Level Approver record.
                                    ''***********************************
                                    EXPModule.UpdateExpProjRepairApproval(ViewState("pProjNo"), TeamMemberID, True, t.SelectedValue, c.Text, SeqNo, 0, DefaultUser, DefaultDate)

                                    ''*******************************
                                    ''Locate Next Approver
                                    ''*******************************
                                    ''Check at same sequence level
                                    ds1st = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), SeqNo, 0, True, False)
                                    If commonFunctions.CheckDataSet(ds1st) = False Then
                                        If t.SelectedValue <> "Rejected" Then
                                            ds2nd = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), IIf(SeqNo < 4, (SeqNo + 1), SeqNo), 0, True, False)
                                            If commonFunctions.CheckDataSet(ds2nd) = True Then
                                                For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                                    If (ds2nd.Tables(0).Rows(i).Item("OrigTeamMemberID") = ds2nd.Tables(0).Rows(i).Item("TeamMemberID")) Then
                                                        If (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                                                        (ddProjectLeader.SelectedValue <> ds2nd.Tables(0).Rows(i).Item("TeamMemberID")) Then

                                                            EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                                            EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                                            ''*****************************************
                                                            ''Update Approvers DateNotified field.
                                                            ''*****************************************
                                                            EXPModule.UpdateExpProjRepairApproval(ViewState("pProjNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", IIf(SeqNo < 4, (SeqNo + 1), SeqNo), 0, DefaultUser, DefaultDate)
                                                        End If
                                                    End If
                                                Next

                                            End If 'EOF ds2nd.Tables.Count > 0 
                                        End If 'EOF t.SelectedValue <> "Rejected"
                                    End If 'EOF ds1st.Tables.Count > 0

                                    'Rejected or last approval
                                    If t.SelectedValue = "Rejected" Or (SeqNo = 4 And t.SelectedValue = "Approved") Then
                                        ''********************************************************
                                        ''Notify Project Lead
                                        ''********************************************************
                                        dsRej = EXPModule.GetExpProjRepairLead(ViewState("pProjNo"))
                                        ''Check that the recipient(s) is a valid Team Member
                                        If commonFunctions.CheckDataSet(dsRej) = True Then
                                            For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                                If (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                                    EmailTO &= dsRej.Tables(0).Rows(i).Item("Email") & ";"
                                                    EmpName &= dsRej.Tables(0).Rows(i).Item("TMName") & ", "

                                                End If
                                            Next
                                        End If
                                    End If 'EOF t.SelectedValue = "Rejected"

                                    ''********************************************************
                                    ''Send Notification only if there is a valid Email Address
                                    ''********************************************************
                                    If EmailTO <> Nothing Then
                                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                                        Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                                        ''**************************************************************
                                        ''Carbon Copy Previous Levels
                                        ''**************************************************************
                                        If SeqNo > 1 Then
                                            ''**************************************************************
                                            ''Carbon Copy Previous Level Approvers - 1 down
                                            ''**************************************************************
                                            EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), "", 0, 0, EmailCC, DefaultTMID)
                                            If SeqNo = 4 Then
                                                ''**************************************************************
                                                ''Carbon Copy 1st Level Approvers
                                                ''**************************************************************
                                                EmailCC = CarbonCopyList(MyMessage, 0, UGNLocation, 1, 0, EmailCC, DefaultTMID)
                                                ''**************************************************************
                                                ''Carbon Copy 2nd Level Approvers
                                                ''**************************************************************
                                                EmailCC = CarbonCopyList(MyMessage, 0, "", 2, 0, EmailCC, DefaultTMID)
                                                ''**************************************************************
                                                ''Carbon Copy 3rd Level Approvers
                                                ''**************************************************************
                                                EmailCC = CarbonCopyList(MyMessage, 0, "", 3, 0, EmailCC, DefaultTMID)
                                                ''**************************************************************
                                                ''Carbon Copy 3rd Level Approvers
                                                ''**************************************************************
                                                EmailCC = CarbonCopyList(MyMessage, 0, "", 4, 0, EmailCC, DefaultTMID)
                                            End If
                                        Else
                                            If SeqNo = 1 Then
                                                EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), UGNLocation, 1, 0, EmailCC, DefaultTMID)
                                            Else
                                                EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), "", 0, 0, EmailCC, DefaultTMID)
                                            End If
                                        End If

                                        If (SeqNo < 4 And t.SelectedValue <> "Rejected") Then
                                            ''**************************************************************
                                            ''Carbon Copy Project Lead
                                            ''**************************************************************
                                            EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                                        ElseIf (SeqNo = 4 And t.SelectedValue <> "Rejected") Then
                                            ''**************************************
                                            ''*Carbon Copy List
                                            ''**************************************
                                            EmailCC = CarbonCopyList(MyMessage, 113, "", 0, 0, EmailCC, DefaultTMID)

                                        End If 'EOF If (SeqNo < 4 And t.SelectedValue <> "Rejected") Then

                                        'Test or Production Message display
                                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                                            MyMessage.Subject = "TEST: "
                                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE.<br/><br/>"
                                        Else
                                            MyMessage.Subject = ""
                                            MyMessage.Body = ""
                                            ' MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE.<br/><br/>"
                                        End If

                                        MyMessage.Subject &= "R Project: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text
                                        MyMessage.Body = "<font size='2' face='Tahoma'>"
                                        If t.SelectedValue = "Rejected" Then

                                            MyMessage.Subject &= " - REJECTED"
                                            MyMessage.Body &= EmpName
                                            MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' was <font color='red'>REJECTED</font>.  "
                                            MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.<br/><br/>"
                                            MyMessage.Body &= "<i>Reason for rejection:</i> <b>" & c.Text & "</b></p>"
                                        Else
                                            If SeqNo = 4 Then
                                                MyMessage.Subject &= " - APPROVED"
                                                MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' is Approved. "
                                                MyMessage.Body &= " <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</p>"
                                            Else

                                                MyMessage.Body &= EmpName
                                                MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' is available for your Review/Approval. "
                                                MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crExpProjRepairApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"
                                            End If
                                        End If
                                        MyMessage.Body &= "</font>"


                                        ''*****************
                                        ''Build Email body
                                        ''*****************
                                        EmailBody(MyMessage)


                                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                                            MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                                            EmailFrom = "Database.Notifications@ugnauto.com"
                                            EmailTO = "lynette.rey@ugnauto.com"
                                            EmailCC = "lynette.rey@ugnauto.com"
                                        End If

                                        ''**********************************
                                        ''Connect & Send email notification
                                        ''**********************************
                                        Try
                                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (R)", ViewState("pProjNo"))
                                            lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                            lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."

                                        Catch ex As Exception
                                            lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                            lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                                            UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, CurrentEmpEmail & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                            'get current event name
                                            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                                            'log and email error
                                            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                                        End Try
                                        lblReqAppComments.Visible = True
                                        lblReqAppComments.ForeColor = Color.Red
                                        lblErrors.Visible = True
                                        lblErrors.Font.Size = 12
                                        MaintainScrollPositionOnPostBack = False

                                        ''*****************
                                        ''History Tracking
                                        ''*****************
                                        If t.SelectedValue <> "Rejected" Then
                                            If SeqNo = 4 Then
                                                EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to all involved.", "", "", "", "")
                                            Else
                                                EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to level " & IIf(SeqNo < 4, (SeqNo + 1), SeqNo) & " approver(s): " & EmpName, "", "", "", "")
                                            End If
                                        Else
                                            EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to " & EmpName, "", "", "", "")
                                        End If
                                    Else
                                        lblErrors.Text = "Your response was submitted successfully."
                                        lblErrors.Visible = True
                                        lblReqAppComments.Text = "Your response was submitted successfully."
                                        lblReqAppComments.Visible = True
                                    End If 'EOF EmailTo <> Nothing
                                End If
                            End If
                        End If
                    End If

                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pProjNo"))
                    gvApprovers.DataBind()

                    ''*************************************************
                    '' "Form Level Security using Roles &/or Subscriptions"
                    ''*************************************************
                    CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                    mvTabs.ActiveViewIndex = Int32.Parse(2)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(2).Selected = True

                Else
                    lblReqAppComments.Text = "Comments is a required field when approving for another team member."
                    lblReqAppComments.Visible = True
                    lblReqAppComments.ForeColor = Color.Red
                End If
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br/>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF gvApprovers_RowUpdating

    Protected Sub gvApprovers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApprovers.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(9).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As ExpProjRepair.ExpProj_Repair_ApprovalRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProjRepair.ExpProj_Repair_ApprovalRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record for " & DataBinder.Eval(e.Row.DataItem, "TeamMemberName") & "?');")
                End If
            End If

            Dim imgEdit As ImageButton = CType(e.Row.FindControl("ibtnEdit"), ImageButton)
            If imgEdit IsNot Nothing Then
                Dim db2 As ImageButton = CType(e.Row.Cells(7).Controls(1), ImageButton)
                Dim c As Label = TryCast(e.Row.FindControl("lblDateNotified"), Label)
                If c.Text = Nothing Then
                    db2.Visible = False
                Else
                    db2.Visible = True
                End If
            End If

        End If
    End Sub 'EOF gvExpense_RowDataBound

#End Region 'EOF "Approval Status"

#Region "Email Notifications"
    Protected Sub btnBuildApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuildApproval.Click

        BuildApprovalList()
        gvApprovers.DataBind()

        mvTabs.ActiveViewIndex = Int32.Parse(2)
        mvTabs.GetActiveView()
        mnuTabs.Items(2).Selected = True

    End Sub 'EOF btnBuildApproval

    Public Function BuildApprovalList() As String
        Try
            ''********
            ''* This function is used to build the Approval List
            ''********
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim UGNLocation As String = commonFunctions.GetCCDValue(cddUGNLocation.SelectedValue)

            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If ViewState("pProjNo") <> Nothing Then
                If (txtRoutingStatus.Text = "R") Or (txtRoutingStatus.Text = "N") Then
                    ''***************
                    ''* Delete 1st Level Approval for rebuild
                    ''***************
                    EXPModule.DeleteExpProjRepairApproval(ViewState("pProjNo"), 0)

                    '***************
                    '* Build 1st level Approval
                    '***************
                    EXPModule.InsertExpProjRepairApproval(ViewState("pProjNo"), UGNLocation, 103, DefaultUser, DefaultDate)

                    '***************
                    '* Build 2nd Level Approval
                    '***************
                    EXPModule.InsertExpProjRepairApproval(ViewState("pProjNo"), UGNLocation, 104, DefaultUser, DefaultDate)

                    '***************
                    '* Build 3rd Level Approval
                    '***************
                    EXPModule.InsertExpProjRepairApproval(ViewState("pProjNo"), UGNLocation, 105, DefaultUser, DefaultDate)

                    '***************
                    '* Build 4th Level Approval
                    '***************
                    EXPModule.InsertExpProjRepairApproval(ViewState("pProjNo"), UGNLocation, 112, DefaultUser, DefaultDate)

                    gvApprovers.DataBind()
                    mvTabs.ActiveViewIndex = Int32.Parse(2)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(2).Selected = True

                End If 'EOF If (txtRoutingStatus.Text <> "R") Then                  
            End If 'EOF  If ViewState("pProjNo") <> Nothing Then

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
        Return True
    End Function 'EOF BuildApprovalList

    Protected Sub btnFwdApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFwdApproval.Click
        Try
            ''********
            ''* This function is used to submit email to the Approvers after the Project Sponsor is done.
            ''********
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim UGNLocation As String = ddUGNLocation.SelectedValue ''commonFunctions.GetCCDValue(cddUGNLocation.SelectedValue)

            Dim ds1st As DataSet = New DataSet
            Dim ds2nd As DataSet = New DataSet
            Dim dsCC As DataSet = New DataSet
            Dim dsCommodity As DataSet = New DataSet
            Dim EmpName As String = Nothing
            Dim EmailTO As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing
            Dim i As Integer = 0
            Dim SponsSameAs1stLvlAprvr As Boolean = False
            Dim SeqNo As Integer = 0
            Dim OrigTMID As Integer = 0

            Dim CurrentEmpEmail As String = Nothing
            If HttpContext.Current.Request.Cookies("UGNDB_User_Email") IsNot Nothing Then
                CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailFrom = CurrentEmpEmail
                EmailCC = CurrentEmpEmail & ";"
            Else
                CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                EmailFrom = "Database.Notifications@ugnauto.com"
            End If


            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then

                ''***************
                ''Verify that atleast one R Project Info entry has been entered before
                ''***************
                Dim dsExp As DataSet = New DataSet
                Dim ReqExpFound As Boolean = False
                dsExp = EXPModule.GetExpProjRepairExpenditure(ViewState("pProjNo"), 0)
                If commonFunctions.CheckDataSet(dsExp) = False Then 'If missing kick user out from submission.
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True

                    rfvDescription.IsValid = False
                    rfvQuantity.IsValid = False
                    rfvAmountPer.IsValid = False
                    vsRepairExpense.ShowSummary = True
                    ReqExpFound = True

                    lblErrors.Text = "Atleast one R Project entry is required for submission."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                End If 'EOF If commonFunctions.CheckDataset(dsExp) = True

                If (txtRoutingStatus.Text = "N") And ViewState("DefaultUserFacility") <> "UT" Then
                    ''***************
                    ''* Delete 1st Level Approval for rebuild
                    ''***************
                    EXPModule.DeleteExpProjRepairApproval(ViewState("pProjNo"), 0)

                    '***************
                    '* Build Approval List
                    '***************
                    'EXPModule.InsertExpProjRepairApproval(ViewState("pProjNo"), UGNLocation, 103, DefaultUser, DefaultDate)
                    BuildApprovalList()

                Else
                    If (txtRoutingStatus.Text <> "N") Then
                        If txtHDTotalInvestment.Text <> 0 And _
                        txtHDTotalInvestment.Text <> lblTotalInvestment.Text Then
                            BuildApprovalList()
                            EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Total Investment amount changed from $" & txtHDTotalInvestment.Text & " to $" & lblTotalInvestment.Text, "Total Investment", txtHDTotalInvestment.Text, lblTotalInvestment.Text, "")
                        ElseIf txtHDTotalInvestment.Text = 0 Then
                            BuildApprovalList()
                        End If
                    End If
                End If

                '**************
                '* Make sure that there is a level 1 approver before submission otherwise alert user
                '**************
                Dim Level1Found As Boolean = False
                Dim ds As DataSet = New DataSet
                ds = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), 0, 0, False, False)
                If commonFunctions.CheckDataSet(ds) = True Then
                    For a = 0 To ds.Tables.Item(0).Rows.Count - 1
                        If ds.Tables(0).Rows(a).Item("SeqNo") = "1" Then
                            Level1Found = True
                        End If
                    Next
                End If

                If Level1Found = False Then
                    lblErrors.Text = "Level 1 approver is required prior to submission."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    lblReqAppComments.Text = "Level 1 approver is required prior to submission."
                    lblReqAppComments.Visible = True
                    lblReqAppComments.ForeColor = Color.Red
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                End If

                ''**********************
                ''* Save data prior to submission before approvals
                ''**********************
                UpdateRecord("In Process", "T", True)


                ''*********************************
                ''Send Notification to Approvers
                ''*********************************
                If ReqExpFound = False Then
                    ''*******************************
                    ''Locate 1st level approver
                    ''*******************************
                    Dim EM As String = Nothing
                    If (txtRoutingStatus.Text <> "R") Then
                        ds1st = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), 1, 0, False, False)
                    Else 'IF Rejected - only notify the TM who Rejected the record
                        If txtHDTotalInvestment.Text = lblTotalInvestment.Text Then
                            ds1st = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), 0, 0, False, True)
                        Else
                            ds1st = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), 1, 0, False, False)
                        End If

                    End If
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(ds1st) = True Then
                        For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                            If (ds1st.Tables(0).Rows(i).Item("Email").ToString.ToUpper <> CurrentEmpEmail.ToUpper) And _
                            (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                            (ddProjectLeader.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID")) Then
                                If EM <> ds1st.Tables(0).Rows(i).Item("Email") Then

                                    EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"
                                    EmpName &= ds1st.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                End If
                                ''************************************************************
                                ''Update 1st level DateNotified field.
                                ''************************************************************
                                If (txtRoutingStatus.Text <> "R") Then
                                    EXPModule.UpdateExpProjRepairApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 1, 0, DefaultUser, DefaultDate)
                                Else 'IF Rejected - only notify the TM who Rejected the record
                                    EXPModule.UpdateExpProjRepairApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), 0, DefaultUser, DefaultDate)
                                    SeqNo = ds1st.Tables(0).Rows(i).Item("SeqNo")
                                    OrigTMID = ds1st.Tables(0).Rows(i).Item("OrigTeamMemberID")
                                End If
                            Else
                                ''************************************************************
                                ''1st Level Approver same as Project Sponsor.  Update record.DefaultTMID
                                ''************************************************************
                                If (txtRoutingStatus.Text <> "R") Then
                                    EXPModule.UpdateExpProjRepairApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), True, "Approved", "", 1, 1, DefaultUser, DefaultDate)
                                Else 'IF Rejected - only notify the TM who Rejected the record
                                    EXPModule.UpdateExpProjRepairApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Approved", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), 1, DefaultUser, DefaultDate)
                                    SeqNo = ds1st.Tables(0).Rows(i).Item("SeqNo")
                                    OrigTMID = ds1st.Tables(0).Rows(i).Item("OrigTeamMemberID")
                                End If

                                If (ds1st.Tables(0).Rows(i).Item("SubmitFlag") = True) Then
                                    SponsSameAs1stLvlAprvr = True
                                End If
                            End If 'EOF If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail)
                            EM = ds1st.Tables(0).Rows(i).Item("Email")
                        Next 'EOF For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                    End If 'EOF If commonFunctions.CheckDataset(ds1st) = True 

                    ''***************************************************************
                    ''Locate 2nd Level Approver(s)
                    ''***************************************************************
                    EM = Nothing
                    If SponsSameAs1stLvlAprvr = True And EmailTO = Nothing Then
                        ds2nd = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), 2, 0, False, False)
                        ''Check that the recipient(s) is a valid Team Member
                        If commonFunctions.CheckDataSet(ds2nd) = True Then
                            For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                    If EM <> ds2nd.Tables(0).Rows(i).Item("Email") Then

                                        EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                        EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                    End If
                                    ''************************************************************
                                    ''Update 2nd level DateNotified field.
                                    ''************************************************************
                                    EXPModule.UpdateExpProjRepairApproval(ViewState("pProjNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 2, 0, DefaultUser, DefaultDate)

                                End If
                                EM = ds2nd.Tables(0).Rows(i).Item("Email")
                            Next
                        End If 'EOF IF commonFunctions.CheckDataset(ds2nd) = True 
                    End If 'EOF If SponsSameAs1stLvlAprvr = True Then

                    ''********************************************************
                    ''Send Notification only if there is a valid Email Address
                    ''********************************************************
                    If EmailTO <> Nothing Then
                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                        If (txtRoutingStatus.Text = "R") Then
                            ''********************************
                            ''Carbon Copy Same Level
                            ''********************************
                            EmailCC = CarbonCopyList(MyMessage, 0, "", IIf(SeqNo = 4, (SeqNo - 1), SeqNo), OrigTMID, EmailCC, DefaultTMID)
                        End If


                        ''Test or Production Message display
                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                            'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
                        End If

                        MyMessage.Subject &= "R Project: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text

                        MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName
                        MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' is available for your Review/Approval. <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crExpProjRepairApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"
                        MyMessage.Body &= "</font>"

                        If txtReSubmit.Text <> Nothing Then
                            MyMessage.Body &= "<font size='2' face='Tahoma'><p><i>Reason for resubmission:</i> <b><font color='red'>" & txtReSubmit.Text & "</font></b></p></font>"
                        End If


                        ''*******************
                        ''Build Email Body
                        ''*******************
                        EmailBody(MyMessage)

                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                            MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                            EmailTO = "lynette.rey@ugnauto.com"
                            EmailCC = "lynette.rey@ugnauto.com"
                        End If

                        ''*****************
                        ''History Tracking
                        ''*****************
                        If txtReSubmit.Text = Nothing Then
                            EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Record completed and forwarded to " & EmpName & " for approval.", "", "", "", "")
                        Else
                            EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Record completed and forwarded to " & EmpName & " for approval.- Reason: " & txtReSubmit.Text, "", "", "", "")
                        End If



                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (R)", ViewState("pProjNo"))

                            lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                            lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."
                        Catch ex As SmtpException
                            lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                            lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                            UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                            'get current event name
                            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                            'log and email error
                            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                        End Try
                        lblReqAppComments.Visible = True
                        lblReqAppComments.ForeColor = Color.Red
                        lblErrors.Visible = True
                        lblErrors.Font.Size = 12
                        MaintainScrollPositionOnPostBack = False

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pProjNo"))
                        gvApprovers.DataBind()

                        ''*************************************************
                        '' "Form Level Security using Roles &/or Subscriptions"
                        ''*************************************************
                        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                    Else
                        If ddUGNLocation.SelectedValue = "UT" Then
                            lblErrors.Text = "Please Build Approval List prior to Submission."
                            lblErrors.Visible = True
                            lblReqAppComments.Text = "Please Build Approval List prior to Submission."
                            lblReqAppComments.Visible = True
                            lblReqAppComments.ForeColor = Color.Red
                        End If
                    End If 'EOF EmailTo <> Nothing
                End If
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnFwdApproval_Click

    Public Function CarbonCopyList(ByVal MyMessage As MailMessage, ByVal SubscriptionID As Integer, ByVal UGNLoc As String, ByVal SeqNo As Integer, ByVal RejectedTMID As Integer, ByVal EmailCC As String, ByVal DefaultTMID As Integer) As String
        Try
            Dim dsCC As DataSet = New DataSet
            Dim IncludeOrigAprvlTM As Boolean = False
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            If SeqNo = 0 Then 'No Rejections have been made, Send notification to all who applies
                If SubscriptionID = 0 Then ''Account Mananager
                    dsCC = EXPModule.GetExpProjRepairLead(ViewState("pProjNo"))
                    IncludeOrigAprvlTM = True
                Else '' All others based on facility
                    If UGNLoc <> Nothing Then
                        dsCC = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(SubscriptionID, UGNLoc)
                    Else
                        If SubscriptionID = 87 Or SubscriptionID = 113 Or SubscriptionID = 124 Or SubscriptionID = 103 Or SubscriptionID = 104 Or SubscriptionID = 105 Then
                            ''Notify Accounting, CC List or 1st level IS or 1st level or 2nd level or 3rd level
                            dsCC = commonFunctions.GetTeamMemberBySubscription(SubscriptionID)
                        End If
                    End If
                End If

                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("TMID") <> DefaultTMID) And _
                        (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                            EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"

                        End If
                    Next
                End If
            Else 'Notify same level approvers after a rejection has been released 
                dsCC = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), SeqNo, 0, False, False)
                'Carbon Copy pending approvers at same level as who rejected the record.
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                        (RejectedTMID <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) Then

                            EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"

                        End If
                    Next
                End If
            End If

            If IncludeOrigAprvlTM = True Then
                dsCC = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), 0, 0, False, False)
                'CC Orgiginal Approver in an event that their a corporate calendar default responsibility
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                        (ddProjectLeader.SelectedValue <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) Then
                            If dsCC.Tables(0).Rows(i).Item("OrigEmail") <> dsCC.Tables(0).Rows(i).Item("Email") Then

                                EmailCC &= dsCC.Tables(0).Rows(i).Item("OrigEmail") & ";"

                            End If
                        End If
                    Next
                End If
            End If

            Return EmailCC

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            Return False

        End Try
    End Function 'EOF CarbonCopyList

    Public Function EmailBody(ByVal MyMessage As MailMessage) As String

        MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>PROJECT OVERVIEW</strong></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Project No:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("pProjNo") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Project Title:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtProjectTitle.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Project Leader:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddProjectLeader.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Description:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td style='width: 700px;'>" & txtProjDateNotes.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Justification/Analysis:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td style='width: 700px;'>" & txtJustification.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddUGNLocation.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Department or Cost Center:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddDepartment.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Estimated Completion Date:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtNextEstCmpltDt.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Estimated Start Spend Date:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtEstSpendDt.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Estimated End Spend Date:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtEstEndSpendDt.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>INVESTMENTS</strong></td>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Total Expenditures ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & lblSubtotalRepair.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>LESS - Retired Equipment Value ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtLessRtrdEqVal.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Working Capital ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtWorkingCapital.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Total Investment ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & lblTotalInvestment.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>RELATED EXPENSES</strong></td>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Start-up Expense ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtStartupExpense.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Customer Reimbursement ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtCustReimb.Text & "</td>"
        MyMessage.Body &= "</tr>"

        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>JUSTIFICATION</strong></td>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Not Required:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & IIf(cbNotRequired.Checked = True, "Yes", "No") & "</td>"
        MyMessage.Body &= "</tr>"
        If ddCRProjNo.SelectedValue <> "" Or ddCRProjNo.SelectedValue <> Nothing Then
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right'>Cost Reduction Ref #:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td><a href='" & ViewState("strProdOrTestEnvironment") & "/CR/CostReduction.aspx?pProjNo=" & ddCRProjNo.SelectedValue & "' target='_blank'>" & ddCRProjNo.SelectedItem.Text & "</a></td>"
            MyMessage.Body &= "</tr>"
        End If

        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Repair Savings ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtRepairSavings.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Scrap Savings ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtScrapSavings.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Consumable Savings ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtConsumableSavings.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Labor Savings ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtLaborSavings.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Other Savings ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtOtherSavings.Text & "</td>"
        MyMessage.Body &= "</tr>"

        ''***************************************************
        ''Get list of Supporting Documentation
        ''***************************************************
        Dim dsAED As DataSet
        dsAED = EXPModule.GetRepairExpDocument(ViewState("pProjNo"), 0)
        If dsAED.Tables.Count > 0 And (dsAED.Tables.Item(0).Rows.Count > 0) Then
            MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
            MyMessage.Body &= "<td colspan='2'><strong>SUPPORTING DOCUMENT(S):</strong></td>"
            MyMessage.Body &= "</tr>"
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td colspan='2'>"
            MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
            MyMessage.Body &= "  <tr>"
            MyMessage.Body &= "   <td width='250px'><b>File Description</b></td>"
            MyMessage.Body &= "   <td width='250px'>&nbsp;</td>"
            MyMessage.Body &= "</tr>"
            For i = 0 To dsAED.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td height='25'>" & dsAED.Tables(0).Rows(i).Item("Description") & "</td>"
                MyMessage.Body &= "<td height='25'><a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/RepairExpProjDocument.aspx?pProjNo=" & ViewState("pProjNo") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
                MyMessage.Body &= "</tr>"
            Next
            MyMessage.Body &= "</table>"
            MyMessage.Body &= "</tr>"
        End If
        MyMessage.Body &= "</table>"

        Return True

    End Function 'EOF EmailBody()

    Public Sub SendNotifWhenEventChanges(ByVal EventDesc As String)
        ''********
        ''* This section will be used in the following methods:
        ''*     1) Email sent to all involved when an Repair is CAPITALIZED
        ''*     2) Email sent to all involved when the Estimated Completion Date changes with the Project Status is not Open
        ''*     3) Email sent to all involved with an Repair is VOID
        ''*     4) Email sent to Account with an Repair is COMPLETED
        ''********188 371 510 569
        Dim DefaultDate As Date = Date.Now
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
        Dim DefaultUserName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

        Dim ds1st As DataSet = New DataSet
        Dim ds2nd As DataSet = New DataSet
        Dim dsCC As DataSet = New DataSet
        Dim dsCommodity As DataSet = New DataSet
        Dim EmpName As String = Nothing
        Dim EmailTO As String = Nothing
        Dim EmailCC As String = Nothing
        Dim EmailFrom As String = Nothing
        Dim GroupNotif As Boolean = False
        Dim i As Integer = 0
        Dim SponsSameAs1stLvlAprvr As Boolean = False

        Dim CurrentEmpEmail As String = Nothing
        If HttpContext.Current.Request.Cookies("UGNDB_User_Email") IsNot Nothing Then
            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
            EmailFrom = CurrentEmpEmail
            EmailCC = CurrentEmpEmail & ";"
        Else
            CurrentEmpEmail = "Database.Notifications@ugnauto.com"
            EmailFrom = "Database.Notifications@ugnauto.com"
        End If


        lblErrors.Text = Nothing
        lblErrors.Visible = False

        Try
            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then
                ''Is this for a Group Notification or Individual
                Select Case EventDesc
                    Case "Capitalized" 'Sent by Accounting, notify all
                        GroupNotif = True
                    Case "Void" 'Sent by Project Leader, notify all
                        GroupNotif = True
                    Case "Estimated Completion Date Changed" 'Sent by Project Leader, notify all
                        GroupNotif = True
                    Case "Completed" 'Sent by Project Leader, notify accounting
                        GroupNotif = False
                End Select

                ''*********************************
                ''Send Notification
                ''*********************************
                If GroupNotif = True Then
                    ''*******************************
                    ''Notify Approvers--include Plant Controllers and Ops Mgrs.
                    ''*******************************
                    ds1st = EXPModule.GetRepairExpProjApproval(ViewState("pProjNo"), 0, 0, False, False)
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(ds1st) = True Then
                        For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                            If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                            (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                            (ddProjectLeader.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID")) Then

                                EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"

                            End If
                        Next
                    End If 'EOF Notify Approvers

                    ''********************************************************
                    ''Notify Project Lead
                    ''********************************************************
                    ds1st = EXPModule.GetExpProjRepairLead(ViewState("pProjNo"))
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(ds1st) = True Then
                        For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                            If (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) Or _
                            (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then

                                EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"

                            End If
                        Next
                    End If 'EOF Notify Project Lead
                Else
                    ''*******************************************
                    ''Notify Accounting
                    ''*******************************************
                    ds1st = commonFunctions.GetTeamMemberBySubscription(87)
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(ds1st) = True Then
                        For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                            If (ds1st.Tables(0).Rows(i).Item("WorkStatus") = 1) Or _
                            (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then

                                EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"

                            End If
                        Next
                    End If 'EOF CC Accounting    
                End If 'EOF  If GroupNotif = True Then
            End If 'EOF  If ReqExpFound = False Then

            ''********************************************************
            ''Send Notification only if there is a valid Email Address
            ''********************************************************
            If EmailTO <> Nothing Then
                Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                If GroupNotif = True And EventDesc = "Capitalized" Then
                    ''*****************************
                    ''Carbon Copy List
                    ''*****************************
                    EmailCC = CarbonCopyList(MyMessage, 113, "", 0, 0, EmailCC, DefaultTMID)
                End If

                'Test or Production Message display
                If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                    MyMessage.Subject = "TEST: "
                    MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                Else
                    MyMessage.Subject = ""
                    MyMessage.Body = ""
                    'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
                End If

                MyMessage.Subject &= "R Project: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text & " - " & EventDesc

                MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"
                If EventDesc = "Estimated Completion Date Changed" Then
                    MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>" & EventDesc.ToUpper & " by " & DefaultUserName & ".</strong></td>"
                Else
                    MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>This CapEx R Project was '" & EventDesc.ToUpper & "' by " & DefaultUserName & ".</strong></td>"
                End If

                MyMessage.Body &= "</tr>"
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td class='p_text' align='right' style='width: 150px;'>Project No:&nbsp;&nbsp; </td>"
                MyMessage.Body &= "<td> <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>" & ViewState("pProjNo") & "</a></td>"
                MyMessage.Body &= "</tr>"
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td class='p_text' align='right'>Project Title:&nbsp;&nbsp; </td>"
                MyMessage.Body &= "<td>" & txtProjectTitle.Text & "</td>"
                MyMessage.Body &= "</tr>"
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td class='p_text' align='right'>Project Leader:&nbsp;&nbsp; </td>"
                MyMessage.Body &= "<td>" & ddProjectLeader.SelectedItem.Text & "</td>"
                MyMessage.Body &= "</tr>"

                Select Case EventDesc
                    Case "Capitalized" 'Sent by Accounting, notify all
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>Actual Cost:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>" & txtActualCost.Text & "</td>"
                        MyMessage.Body &= "</tr>"
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>Customer Cost:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>" & txtCustomerCost.Text & "</td>"
                        MyMessage.Body &= "</tr>"
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Comments:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>" & txtClosingNotes.Text & "</td>"
                        MyMessage.Body &= "</tr>"
                    Case "Void" 'Sent by Project Leader, notify all
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>Void Reason:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>" & txtVoidReason.Text & "</td>"
                        MyMessage.Body &= "</tr>"
                    Case "Estimated Completion Date Changed" 'Sent by Project Leader, notify all
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>Previous Value:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>" & txtHDEstCmpltDt.Text & "</td>"
                        MyMessage.Body &= "</tr>"
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>New Value:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>" & txtNextEstCmpltDt.Text & "</td>"
                        MyMessage.Body &= "</tr>"
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>Change Reason:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>" & txtEstCmpltDtChngRsn.Text & "</td>"
                        MyMessage.Body &= "</tr>"
                    Case "Completed" 'Sent by Project Leader, notify accounting
                        ''no additional info needed all in the subject line
                End Select
                MyMessage.Body &= "</table>"


                ''*****************
                ''History Tracking
                ''*****************
                Select Case EventDesc
                    Case "Capitalized" 'Sent by Accounting, notify all
                        EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Capitalized", "", "", "", "")
                    Case "Void" 'Sent by Project Leader, notify all
                        EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "VOID - Reason:" & txtVoidReason.Text, "", "", "", "")
                    Case "Estimated Completion Date Changed" 'Sent by Project Leader, notify all
                        EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Estimated Completion Date Changed From '" & txtHDEstCmpltDt.Text & "' to '" & txtNextEstCmpltDt.Text & "' - Reason: " & txtEstCmpltDtChngRsn.Text, "Estimated Completion Date", txtHDEstCmpltDt.Text, txtNextEstCmpltDt.Text, txtEstCmpltDtChngRsn.Text)
                    Case "Completed" 'Sent by Project Leader, notify accounting
                        EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Completed", "", "", "", "")
                End Select


                If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                    MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                    MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                    EmailFrom = "Database.Notifications@ugnauto.com"
                    EmailTO = "lynette.rey@ugnauto.com"
                    EmailCC = "lynette.rey@ugnauto.com"
                End If


                ''**********************************
                ''Connect & Send email notification
                ''**********************************
                Try
                    commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (R)", ViewState("pProjNo"))
                    lblErrors.Text = "Notification sent successfully."
                Catch ex As Exception
                    lblErrors.Text &= "Email Notification is queued for the next automated release."

                    UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, CurrentEmpEmail & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
                    'get current event name
                    Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                    'log and email error
                    UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                End Try
                lblErrors.Visible = True
                lblErrors.Font.Size = 12
                MaintainScrollPositionOnPostBack = False

                ''**********************************
                ''Rebind the data to the form
                ''********************************** 
                BindData(ViewState("pProjNo"))
                gvApprovers.DataBind()

                ''*************************************************
                '' "Form Level Security using Roles &/or Subscriptions"
                ''*************************************************
                CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                mvTabs.ActiveViewIndex = Int32.Parse(0)
                mvTabs.GetActiveView()
                mnuTabs.Items(0).Selected = True
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF SendNotifWhenEventChanges

    Protected Sub btnCRProjNoReq_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCRProjNoReq.Click
        Try
            ''********
            ''* This function is used to submit email to the Approvers after the Project Sponsor is done.
            ''********
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim ds1st As DataSet = New DataSet
            Dim ds2nd As DataSet = New DataSet
            Dim dsCC As DataSet = New DataSet
            Dim dsCommodity As DataSet = New DataSet
            Dim EmpName As String = Nothing
            Dim EmailTO As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing
            Dim i As Integer = 0
            Dim SponsSameAs1stLvlAprvr As Boolean = False
            Dim UGNLocation As String = commonFunctions.GetCCDValue(cddUGNLocation.SelectedValue)

            Dim CurrentEmpEmail As String = Nothing
            If HttpContext.Current.Request.Cookies("UGNDB_User_Email") IsNot Nothing Then
                CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailFrom = CurrentEmpEmail
                EmailCC = CurrentEmpEmail & ";"
            Else
                CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                EmailFrom = "Database.Notifications@ugnauto.com"
            End If


            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then

                ''***************
                ''Verify that atleast one R Project Info entry has been entered before
                ''***************
                Dim dsExp As DataSet = New DataSet
                Dim ReqExpFound As Boolean = False
                dsExp = EXPModule.GetExpProjRepairExpenditure(ViewState("pProjNo"), 0)
                If commonFunctions.CheckDataSet(dsExp) = False Then 'If missing kick user out from submission.
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True

                    rfvDescription.IsValid = False
                    rfvQuantity.IsValid = False
                    rfvAmountPer.IsValid = False
                    vsRepairExpense.ShowSummary = True
                    ReqExpFound = True

                    lblErrors.Text = "Atleast one R Project entry is required for submission."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    Exit Sub
                End If 'EOF If commonFunctions.CheckDataset(dsExp) = True

                ''***************
                ''Set flag to send notification for Cost Reduction entry
                ''***************
                Dim SendCRNotif As Boolean = False
                If ddCRProjNo.SelectedValue = Nothing Then
                    SendCRNotif = True
                End If

                ''*********************************
                ''Send Notification based on facility to Cost Reduction Team Member User based on Facility
                ''*********************************
                Dim ds As DataSet = New DataSet
                If SendCRNotif = True And ReqExpFound = False Then
                    ''*********************************
                    ''Verify that Cost Reduction Ref# is valid before proceeding.
                    ''If it does not exist, default notification to a member of CR module
                    ''*********************************
                    ds = CRModule.GetCostReduction(ddCRProjNo.SelectedValue, 0, UGNLocation, 0, 0, "", 0, False, False, "")
                    If commonFunctions.CheckDataSet(ds) = False Then
                        SendCRNotif = False 'skip to approval process
                    Else
                        ''Locate CR Team Leader according to UGN Facility
                        ds1st = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(75, UGNLocation)
                        If commonFunctions.CheckDataSet(ds1st) = True Then
                            For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                                If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                                (ddProjectLeader.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TMID")) Then

                                    EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"
                                    EmpName &= ds1st.Tables(0).Rows(i).Item("TMName") & ", "

                                End If 'EOF If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                            Next 'EOF For i = 0 To
                        End If 'EOF commonFunctions.CheckDataset(ds1st) = True 
                    End If 'EOF If commonFunctions.CheckDataset(ds) = True 
                End If 'If SendCRNotif = True Then

                ''********************************************************
                ''Send Notification only if there is a valid Email Address
                ''********************************************************
                If EmailTO <> Nothing Then
                    'Update Record
                    UpdateRecord(ViewState("ProjectStatus"), IIf(ViewState("ProjectStatus") = "Capitalized", "C", IIf(ViewState("ProjectStatus") = "Void", "V", IIf(ViewState("ProjectStatus") = "Open", "N", IIf(ViewState("ProjectStatus") = "Approved", "A", txtRoutingStatus.Text)))), False)


                    Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                    ''Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                    Else
                        MyMessage.Subject = ""
                        MyMessage.Body = ""
                        'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
                    End If

                    If SendCRNotif = True Then
                        MyMessage.Subject &= " Cost Reduction Project Request for "
                    End If

                    MyMessage.Subject &= "R Project: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text

                    MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' requires a Cost Reduction Project entry. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/CostReduction.aspx?pProjNo=0&pCPNo=" & ViewState("pProjNo") & "'>Click here</a> to create a new record. Below is a summary of the R Project Request for your review. For additional information about the CapEx R Project <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/RepairExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>click here</a>.</p>"

                    ''*****************
                    ''Build Email body
                    ''*****************
                    EmailBody(MyMessage)

                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                        MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                        EmailFrom = "Database.Notifications@ugnauto.com"
                        EmailTO = "lynette.rey@ugnauto.com"
                        EmailCC = "lynette.rey@ugnauto.com"
                    End If

                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjRepairHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Cost Reduction Project request sent to " & EmpName & "..", "", "", "", "")

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (R)", ViewState("pProjNo"))
                        lblErrors.Text = "Cost Reduction Reference Request Submitted Successfully. "

                        'lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                        mvTabs.ActiveViewIndex = Int32.Parse(2)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(2).Selected = True

                    Catch ex As SmtpException
                        lblErrors.Text &= "Cost Reduction Reference Request is queued for the next automated release."

                        'lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."

                        UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                        'get current event name
                        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                        'log and email error
                        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                    End Try
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False

                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pProjNo"))

                    ''*************************************************
                    '' "Form Level Security using Roles &/or Subscriptions"
                    ''*************************************************
                    CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True
                    ddCRProjNo.Focus()
                End If 'EOF EmailTo <> Nothing
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnCRProjNoReq_Click

#End Region 'EOF "Email Notifications"

End Class