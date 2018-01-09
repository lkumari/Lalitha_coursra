' ************************************************************************************************
' Name:	DevelopmentExpProj.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 11/01/2011    LRey			Created .Net application
' 05/23/2012    LRey            Consolidated redundant logic into reusable functions
' 07/16/2013    LRey            Corrected the same program DPrjNo sequence. Added a few carryover values in the BindData() for new record.
' 04/09/2014    LRey            Modified the Approval Chain Build to use the UGNFacility. 
'                               Reset the approval chain when the Total Investment expense changes.
' ************************************************************************************************
Partial Class EXP_DevelopmentExpProj
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim a As String = commonFunctions.UserInfo()
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
                    txtProjectID.Text = ""
                    txtOrigProjectID.Text = ""

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


            ''Used to create a new function classification under the same numbering scheme
            If HttpContext.Current.Request.QueryString("pFC") <> "" Then
                ViewState("pFC") = HttpContext.Current.Request.QueryString("pFC")
            Else
                ViewState("pFC") = ""
            End If


            ''Used to Show/Hide Future Part Info text boxes
            ViewState("pFPNo") = False
            ViewState("HideVehicleEntry") = False

            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            If ViewState("pProjNo") = Nothing Then
                m.ContentLabel = "New Development Project"
            Else
                m.ContentLabel = "Development Project"
            End If

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pProjNo") = Nothing Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='DevelopmentExpProjList.aspx'><b>Development Project Search</b></a> > New Development Project"
                Else
                    If ViewState("pAprv") = 0 Then
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='DevelopmentExpProjList.aspx'><b>Development Project Search</b></a> > Development Project"
                    Else 'Go Back To approval Screen
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='DevelopmentExpProjList.aspx'><b>Development Project Search</b></a> > <a href='crExpProjDevelopmentApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1'><b>Approval</b></a> > Development Project"
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
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            ' ''CheckRights() '"Form Level Security using Roles &/or Subscriptions"

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
                    txtYear.Focus()
                    txtDateSubmitted.Text = Date.Today
                End If

                If ViewState("pSD") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True
                ElseIf ViewState("pEID") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(2)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(2).Selected = True
                ElseIf ViewState("pEV") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(2)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(2).Selected = True
                ElseIf ViewState("pRID") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(4)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(4).Selected = True
                ElseIf ViewState("pRC") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(4)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(4).Selected = True
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

            txtProjDateNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtProjDateNotes.Attributes.Add("onkeyup", "return tbCount(" + lblProjDateNotes.ClientID + ");")
            txtProjDateNotes.Attributes.Add("maxLength", "2000")

            txtJustification.Attributes.Add("onkeypress", "return tbLimit();")
            txtJustification.Attributes.Add("onkeyup", "return tbCount(" + lblJustification.ClientID + ");")
            txtJustification.Attributes.Add("maxLength", "2000")

            txtGeneralNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtGeneralNotes.Attributes.Add("onkeyup", "return tbCount(" + lblGeneralNotes.ClientID + ");")
            txtGeneralNotes.Attributes.Add("maxLength", "2000")

            txtVoidReason.Attributes.Add("onkeypress", "return tbLimit();")
            txtVoidReason.Attributes.Add("onkeyup", "return tbCount(" + lblVoidReason.ClientID + ");")
            txtVoidReason.Attributes.Add("maxLength", "300")

            txtReply.Attributes.Add("onkeypress", "return tbLimit();")
            txtReply.Attributes.Add("onkeyup", "return tbCount(" + lblReply.ClientID + ");")
            txtReply.Attributes.Add("maxLength", "300")

            txtFileDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtFileDesc.Attributes.Add("onkeyup", "return tbCount(" + lblFileDesc.ClientID + ");")
            txtFileDesc.Attributes.Add("maxLength", "200")

            Dim strPreviewClientScript As String = "javascript:void(window.open('crViewExpProjDevelopment.aspx?pProjNo=" & ViewState("pProjNo") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
            btnPreview.Attributes.Add("onclick", strPreviewClientScript)


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
    End Sub 'EOF mnuTabs_MenuItemClick

#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        Try
            ''*******
            '' Disable controls by default
            ''*******
            ddProjectStatus.Enabled = False
            btnAdd.Enabled = False
            btnSave1.Enabled = False
            btnSave2.Enabled = True
            btnReset1.Enabled = False
            btnReset2.Enabled = False
            btnReset4.Enabled = True
            btnReset3.Enabled = False
            btnUpload.Enabled = False
            btnDelete.Enabled = False
            btnPreview.Enabled = False
            btnAppend.Enabled = False
            btnFwdApproval.Enabled = False
            btnBuildApproval.Enabled = False
            btnBuildApproval.Visible = False
            btnExpenditure.Enabled = False
            btnCRProjNoReq.Enabled = False
            cbCRProjNoReq.Enabled = False
            uploadFile.Enabled = False
            mnuTabs.Items(0).Enabled = True
            mnuTabs.Items(1).Enabled = False
            mnuTabs.Items(2).Enabled = False
            mnuTabs.Items(3).Enabled = False
            mnuTabs.Items(4).Enabled = False
            gvSupportingDocument.Columns(3).Visible = False
            gvApprovers.Columns(7).Visible = False
            gvApprovers.Columns(8).Visible = False
            gvApprovers.Columns(9).Visible = False
            gvApprovers.ShowFooter = False
            gvQuestion.Columns(0).Visible = True
            txtVoidReason.Visible = False
            txtVoidReason.Enabled = False
            lblVoidRsn.Visible = False
            lblReqVoidRsn.Visible = False
            rfvVoidReason.Enabled = False
            lblProjectID.Visible = True
            txtProjectID.Visible = False

            lblReqYear.Visible = False
            lblYear.Visible = False
            txtYear.Visible = False
            rvYear.Enabled = False
            lblMake.Visible = False
            lblModel.Visible = False
            ddMakes.Visible = False
            ddModel.Visible = False
            lblReqProgram.Visible = False
            lblProgram.Visible = False
            ddProgram.Visible = False
            lblReqPreDev.Visible = False
            ddPreDvp.Visible = False
            ddCClass.Visible = False
            lblReqCommodity.Visible = False
            ddCommodity.Visible = False
            rfvCommodity.Enabled = False

            If lblProjectTitle.Text <> Nothing Then
                rfvProgram.Enabled = False
                rfvPreDvp.Enabled = False
            End If

            ViewState("Admin") = False
            ViewState("ObjectRole") = False

            ''** Project Status
            If txtRoutingStatus.Text = "N" Or txtRoutingStatus.Text = "A" Or txtRoutingStatus.Text = "C" Or txtRoutingStatus.Text = Nothing Then
                ddProjectStatus.Visible = True
                ddProjectStatus2.Visible = False
            Else
                ddProjectStatus.Visible = False
                ddProjectStatus2.Visible = True
            End If

            Dim ProjectStatus As String = Nothing
            If txtRoutingStatus.Text = "N" Or txtRoutingStatus.Text = "A" Or txtRoutingStatus.Text = "C" Or txtRoutingStatus.Text = Nothing Then
                ProjectStatus = ddProjectStatus.SelectedValue
            Else
                ProjectStatus = ddProjectStatus2.SelectedValue
            End If

            If ddProjectStatus.SelectedValue = "Void" Then
                lblReqVoidRsn.Visible = True
                lblVoidRsn.Visible = True
                txtVoidReason.Visible = True
                txtVoidReason.Enabled = True
                rfvVoidReason.Enabled = True
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
            Dim iFormID As Integer = 130 'Development Expense Form ID
            Dim iRoleID As Integer = 0
            Dim i As Integer = 0
            ViewState("DefaultUserFacility") = Nothing

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Nicolas.Leclercq", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    ViewState("iTeamMemberID") = iTeamMemberID

                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member

                        ''Locate the Shipping/EDI Coordinator to grant access Shipping Info
                        Dim dsPM As DataSet = New DataSet
                        dsPM = commonFunctions.GetTeamMemberBySubscription(31)
                        Dim iPMID As Integer = 0
                        Dim b As Integer = 0
                        ViewState("iPMTMID") = 0
                        If (dsPM.Tables.Item(0).Rows.Count > 0) Then
                            For b = 0 To dsPM.Tables(0).Rows.Count - 1
                                If dsPM.Tables(0).Rows(b).Item("TMID") = iTeamMemberID Then
                                    iPMID = dsPM.Tables(0).Rows(b).Item("TMID")
                                    ViewState("iPMTMID") = iPMID

                                End If
                            Next
                        End If

                        If ViewState("iPMTMID") = ViewState("iTeamMemberID") Then
                            ddPreDvp.Enabled = False
                        Else
                            ddPreDvp.Enabled = True
                        End If

                        'Get Team Member's Facility Location based on SubscriptionID
                        dsTMFacility = SecurityModule.GetTMWorkHistory(iTeamMemberID, 92)
                        If dsTMFacility IsNot Nothing Then
                            If dsTMFacility.Tables.Count And dsTMFacility.Tables(0).Rows.Count > 0 Then
                                iTMFacility = dsTMFacility.Tables(0).Rows(0).Item("UGNFacility")
                                ViewState("DefaultUserFacility") = iTMFacility
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
                                            btnSave1.Enabled = True
                                            btnReset1.Enabled = True
                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pProjNo") = Nothing Or ViewState("pProjNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                If lblProjectTitle.Text = Nothing Then
                                                    lblReqYear.Visible = True
                                                    lblYear.Visible = True
                                                    txtYear.Visible = True
                                                    rvYear.Enabled = True
                                                    lblMake.Visible = True
                                                    lblModel.Visible = True
                                                    ddMakes.Visible = True
                                                    ddModel.Visible = True
                                                    lblReqProgram.Visible = True
                                                    lblProgram.Visible = True
                                                    ddProgram.Visible = True
                                                    lblReqPreDev.Visible = True
                                                    ddPreDvp.Visible = True
                                                    ddCClass.Visible = True
                                                    lblReqCommodity.Visible = True
                                                    ddCommodity.Visible = True
                                                    rfvCommodity.Enabled = True
                                                    txtYear.Focus()
                                                    lblProjectID.Visible = True
                                                    txtProjectID.Visible = False
                                                End If
                                            Else
                                                ViewState("Admin") = True
                                                lblProjectID.Visible = False
                                                txtProjectID.Visible = True
                                                Select Case ProjectStatus
                                                    Case "Open"
                                                        If (txtRoutingStatus.Text = "N") Then
                                                            If iTMFacility = "UT" Then
                                                                btnBuildApproval.Enabled = True
                                                                btnBuildApproval.Visible = True
                                                                gvApprovers.Columns(8).Visible = True
                                                                gvApprovers.Columns(9).Visible = True
                                                                gvApprovers.ShowFooter = True
                                                            End If
                                                            btnFwdApproval.Enabled = True
                                                        End If
                                                        If ddCRProjNo.SelectedValue = Nothing And cbCRProjNoReq.Checked = False Then
                                                            btnCRProjNoReq.Enabled = True
                                                        End If
                                                        btnExpenditure.Enabled = True
                                                        btnReset2.Enabled = True
                                                        btnReset3.Enabled = True
                                                        btnUpload.Enabled = True
                                                        uploadFile.Enabled = True
                                                        gvSupportingDocument.Columns(3).Visible = True
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "R") Then
                                                            btnReset2.Enabled = True
                                                            btnFwdApproval.Enabled = True
                                                            btnExpenditure.Enabled = True
                                                            btnReset3.Enabled = True
                                                            btnUpload.Enabled = True
                                                            uploadFile.Enabled = True
                                                            gvSupportingDocument.Columns(3).Visible = True
                                                            btnSave2.Enabled = True
                                                            btnReset4.Enabled = True
                                                            btnCRProjNoReq.Enabled = False
                                                            gvApprovers.Columns(8).Visible = True
                                                            gvApprovers.Columns(9).Visible = True
                                                            gvApprovers.ShowFooter = True
                                                        ElseIf txtRoutingStatus.Text = "T" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            btnExpenditure.Enabled = True
                                                            btnReset2.Enabled = True
                                                            btnSave2.Enabled = True 'QC
                                                            btnReset4.Enabled = True 'QC
                                                            btnReset3.Enabled = True
                                                            btnUpload.Enabled = True
                                                            uploadFile.Enabled = True
                                                            gvApprovers.Columns(7).Visible = True
                                                            gvSupportingDocument.Columns(3).Visible = True
                                                        End If
                                                        ddProjectStatus.Enabled = True
                                                    Case "Approved"
                                                        btnReset3.Enabled = True
                                                        btnUpload.Enabled = True
                                                        uploadFile.Enabled = True
                                                        gvSupportingDocument.Columns(3).Visible = True
                                                        btnSave2.Enabled = True
                                                        btnReset4.Enabled = True
                                                        btnCRProjNoReq.Enabled = False
                                                        ddProjectStatus.Enabled = True
                                                        ddProjectStatus.Items.RemoveAt(0)
                                                    Case "Completed"
                                                        If txtRoutingStatus.Text <> "C" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            ddProjectStatus.Enabled = True
                                                            btnCRProjNoReq.Enabled = False
                                                        Else
                                                            btnSave1.Enabled = False
                                                            btnReset1.Enabled = False
                                                        End If
                                                    Case "Void"
                                                        If txtRoutingStatus.Text <> "V" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                        End If
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidRsn.Visible = True
                                                        lblReqVoidRsn.Visible = True
                                                        btnCRProjNoReq.Enabled = False
                                                End Select
                                                If (txtRoutingStatus.Text <> "N") And (txtRoutingStatus.Text <> "R") And (txtRoutingStatus.Text <> "T") Then
                                                    btnAppend.Enabled = True
                                                End If
                                                If txtRoutingStatus.Text = "N" Then
                                                    btnDelete.Enabled = True
                                                End If
                                                btnAdd.Enabled = True
                                                btnPreview.Enabled = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                mnuTabs.Items(4).Enabled = True
                                                txtEstCmpltDt.Enabled = True
                                                btnDelete.Enabled = True
                                            End If
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Project Leader
                                            ViewState("ObjectRole") = True
                                            btnSave1.Enabled = True
                                            btnReset1.Enabled = True
                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pProjNo") = Nothing Or ViewState("pProjNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                If lblProjectTitle.Text = Nothing Then
                                                    lblReqYear.Visible = True
                                                    lblYear.Visible = True
                                                    txtYear.Visible = True
                                                    rvYear.Enabled = True
                                                    lblMake.Visible = True
                                                    lblModel.Visible = True
                                                    ddMakes.Visible = True
                                                    ddModel.Visible = True
                                                    lblReqProgram.Visible = True
                                                    lblProgram.Visible = True
                                                    ddProgram.Visible = True
                                                    lblReqPreDev.Visible = True
                                                    ddPreDvp.Visible = True
                                                    ddCClass.Visible = True
                                                    lblReqCommodity.Visible = True
                                                    ddCommodity.Visible = True
                                                    rfvCommodity.Enabled = True
                                                    txtYear.Focus()
                                                End If
                                            Else
                                                ViewState("Admin") = True
                                                Select Case ProjectStatus
                                                    Case "Open"
                                                        If (txtRoutingStatus.Text = "N") Then
                                                            'If iTMFacility = "UT" Then
                                                            'btnBuildApproval.Enabled = True
                                                            'btnBuildApproval.Visible = True
                                                            'gvApprovers.Columns(8).Visible = True
                                                            'gvApprovers.ShowFooter = True
                                                            'End If
                                                            btnFwdApproval.Enabled = True
                                                            btnDelete.Enabled = True
                                                        End If
                                                        If ddCRProjNo.SelectedValue = Nothing And cbCRProjNoReq.Checked = False Then
                                                            btnCRProjNoReq.Enabled = True
                                                        End If
                                                        btnExpenditure.Enabled = True
                                                        btnReset2.Enabled = True
                                                        btnReset3.Enabled = True
                                                        btnUpload.Enabled = True
                                                        uploadFile.Enabled = True
                                                        gvSupportingDocument.Columns(3).Visible = True
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "R") Then
                                                            btnReset2.Enabled = True
                                                            btnFwdApproval.Enabled = True
                                                            btnExpenditure.Enabled = True
                                                            btnReset3.Enabled = True
                                                            btnUpload.Enabled = True
                                                            uploadFile.Enabled = True
                                                            gvSupportingDocument.Columns(3).Visible = True
                                                            btnSave2.Enabled = True
                                                            btnReset4.Enabled = True
                                                            btnCRProjNoReq.Enabled = False
                                                        ElseIf txtRoutingStatus.Text = "T" Then
                                                            btnCRProjNoReq.Enabled = True
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            btnExpenditure.Enabled = True
                                                            btnReset2.Enabled = True
                                                            btnSave2.Enabled = True
                                                            btnReset4.Enabled = True
                                                            btnReset3.Enabled = True
                                                            btnUpload.Enabled = True
                                                            uploadFile.Enabled = True
                                                            gvSupportingDocument.Columns(3).Visible = True
                                                        End If
                                                        ddProjectStatus.Enabled = True
                                                    Case "Approved"
                                                        btnReset3.Enabled = True
                                                        btnUpload.Enabled = True
                                                        uploadFile.Enabled = True
                                                        gvSupportingDocument.Columns(3).Visible = True
                                                        btnSave2.Enabled = True
                                                        btnReset4.Enabled = True
                                                        btnCRProjNoReq.Enabled = False
                                                        ddProjectStatus.Enabled = True
                                                        ddProjectStatus.Items.RemoveAt(0)
                                                    Case "Completed"
                                                        If txtRoutingStatus.Text <> "C" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            ddProjectStatus.Enabled = True
                                                            btnCRProjNoReq.Enabled = False
                                                        Else
                                                            btnSave1.Enabled = False
                                                            btnReset1.Enabled = False
                                                        End If
                                                    Case "Void"
                                                        If txtRoutingStatus.Text <> "V" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                        End If
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidRsn.Visible = True
                                                        lblReqVoidRsn.Visible = True
                                                        btnCRProjNoReq.Enabled = False
                                                End Select
                                                If (txtRoutingStatus.Text <> "N") And (txtRoutingStatus.Text <> "R") And (txtRoutingStatus.Text <> "T") Then
                                                    btnAppend.Enabled = True
                                                End If
                                                If txtRoutingStatus.Text = "N" Then
                                                    btnDelete.Enabled = True
                                                End If
                                                btnAdd.Enabled = True
                                                btnPreview.Enabled = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                mnuTabs.Items(4).Enabled = True
                                                txtEstCmpltDt.Enabled = True
                                            End If
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            '** Used by Approvers & Backup persons
                                            If ViewState("pProjNo") = Nothing Or ViewState("pProjNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                btnCRProjNoReq.Enabled = False
                                            Else
                                                ViewState("ObjectRole") = False
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                mnuTabs.Items(4).Enabled = True
                                                btnPreview.Enabled = True
                                                Select Case ProjectStatus
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "T") Then
                                                            gvApprovers.Columns(7).Visible = True
                                                            btnReset3.Enabled = False
                                                            btnUpload.Enabled = False
                                                            uploadFile.Enabled = False
                                                            gvSupportingDocument.Columns(3).Visible = False
                                                            btnCRProjNoReq.Enabled = False
                                                        End If
                                                End Select
                                            End If
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            ViewState("ObjectRole") = False
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                            mnuTabs.Items(3).Enabled = True
                                            mnuTabs.Items(4).Enabled = True
                                            btnPreview.Enabled = True
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
                                            Else
                                                Select Case ProjectStatus
                                                    Case "In Process"
                                                        gvApprovers.Columns(9).Visible = True
                                                    Case "Void"
                                                        If txtRoutingStatus.Text <> "V" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                        End If
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidRsn.Visible = True
                                                        lblReqVoidRsn.Visible = True
                                                    Case "Approved"
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        ddProjectStatus.Enabled = True
                                                    Case "Completed"
                                                        If txtRoutingStatus.Text <> "C" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            ddProjectStatus.Enabled = True
                                                        End If
                                                End Select
                                                If (txtRoutingStatus.Text <> "N") And (txtRoutingStatus.Text <> "R") And (txtRoutingStatus.Text <> "T") Then
                                                    btnAppend.Enabled = True
                                                End If
                                                btnAdd.Enabled = True
                                                gvSupportingDocument.Columns(3).Visible = True
                                                uploadFile.Enabled = True
                                                btnReset3.Enabled = True
                                                btnUpload.Enabled = True
                                                btnPreview.Enabled = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                mnuTabs.Items(4).Enabled = True
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

            ''bind existing data to drop down Requested By control for selection criteria for search
            ds = commonFunctions.GetTeamMember("") '**SubscriptionID # is used foDevelopment Project Leader
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddRequestedBy.DataSource = ds
                ddRequestedBy.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddRequestedBy.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddRequestedBy.DataBind()
                ddRequestedBy.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Project Leader control for selection criteria for search
            ds = commonFunctions.GetTeamMember("") '**SubscriptionID # is used foDevelopment Project Leader
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddProjectLeader.DataSource = ds
                ddProjectLeader.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddProjectLeader.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddProjectLeader.DataBind()
                ddProjectLeader.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Team Member control for selection criteria for search
            'ds = commonFunctions.GetTeamMember("")
            'If (ds.Tables.Item(0).Rows.Count > 0) Then
            '    ddTeamMember.DataSource = ds
            '    ddTeamMember.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
            '    ddTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
            '    ddTeamMember.DataBind()
            '    ddTeamMember.Items.Insert(0, "")
            '    ddTeamMember.Enabled = False
            'End If

            commonFunctions.UserInfo()
            ddRequestedBy.SelectedValue = ViewState("iTeamMemberID") 'HttpContext.Current.Session("UserId")
            ddProjectLeader.SelectedValue = ViewState("iTeamMemberID") 'HttpContext.Current.Session("UserId")
            ' ddTeamMember.SelectedValue = HttpContext.Current.Session("UserId")

            ''bind existing data to drop down Account Manager control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(9) '**SubscriptionID 9 is used for Account Manager
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddAccountManager.DataSource = ds
                ddAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddAccountManager.DataBind()
                ddAccountManager.Items.Insert(0, "")
            End If

            'bind existing data to drop down UGN Location control for selection criteria for search
            ds = commonFunctions.GetUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

            'bind existing data to drop down Department or Cost Center control for selection criteria for search
            ds = commonFunctions.GetDepartmentGLNo(IIf(ddUGNFacility.SelectedValue = "", "", ddUGNFacility.SelectedValue))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddDepartment.DataSource = ds
                ddDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName.ToString()
                ddDepartment.DataValueField = ds.Tables(0).Columns("GLNo").ColumnName.ToString()
                ddDepartment.DataBind()
                ddDepartment.Items.Insert(0, "")
            End If

            ' ''bind existing data to drop down Customer control for selection criteria for search
            'ds = commonFunctions.GetOEMManufacturer("")
            'If (ds.Tables.Item(0).Rows.Count > 0) Then
            '    ddCustomer.DataSource = ds
            '    ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
            '    ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
            '    ddCustomer.DataBind()
            '    ddCustomer.Items.Insert(0, "")
            'End If

            ''bind existing data to drop down Line # control for selection criteria for search
            ds = EXPModule.GetCostReductionList(IIf(ddUGNFacility.SelectedValue = "", "", ddUGNFacility.SelectedValue))
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
                ds = EXPModule.GetExpProjDevelopment(ProjNo, "", "", 0, 0, 0, "", 0, 0, "", 0, "")
                If commonFunctions.CheckDataSet(ds) = True Then
                    If ViewState("pPrntProjNo") = Nothing Then
                        lblProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                        txtProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                        txtOrigProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                        Select Case ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                            Case "N"
                                ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                            Case "A"
                                ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                            Case "C"
                                ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                            Case "T"
                                ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                            Case "R"
                                ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                            Case "V"
                                ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                        End Select
                        txtRoutingStatus.Text = ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                        If ddProjectStatus2.SelectedValue <> "Void" Then
                            lblRoutingStatusDesc.Text = ds.Tables(0).Rows(0).Item("RoutingStatusDesc").ToString()
                        End If
                        lblRoutingStatusDesc.Visible = True
                    Else
                        If ViewState("pProjNo") = Nothing Then
                            lblProjectID.Text = ProjNo & "?"
                            txtProjectID.Text = ""
                            txtOrigProjectID.Text = ""

                            ddProjectStatus.SelectedValue = "Open"
                            lblPrntProjNo.Text = ProjNo

                            lblPrntAppDate.Text = IIf(ds.Tables(0).Rows(0).Item("OriginalApprovedDt").ToString() = "01/01/1900", "", ds.Tables(0).Rows(0).Item("OriginalApprovedDt").ToString())
                        Else
                            lblProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                            txtProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                            txtOrigProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()

                            Select Case ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                                Case "N"
                                    ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                                Case "A"
                                    ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                                Case "C"
                                    ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                                Case "T"
                                    ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                                Case "R"
                                    ddProjectStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                                Case "V"
                                    ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                            End Select
                            txtRoutingStatus.Text = ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                            If ddProjectStatus2.SelectedValue <> "Void" Then
                                lblRoutingStatusDesc.Text = ds.Tables(0).Rows(0).Item("RoutingStatusDesc").ToString()
                            End If

                            lblRoutingStatusDesc.Visible = True
                            lblPrntProjNo.Text = ds.Tables(0).Rows(0).Item("ParentProjectNo").ToString()
                            lblPrntAppDate.Text = ds.Tables(0).Rows(0).Item("OriginalApprovedDt").ToString()
                        End If
                    End If

                    lblProjectTitle.Text = ds.Tables(0).Rows(0).Item("ProjectTitle").ToString()
                    ddRequestedBy.SelectedValue = ds.Tables(0).Rows(0).Item("RequestedByTMID").ToString()
                    ddProjectLeader.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectLeaderTMID").ToString()
                    ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AcctMgrTMID").ToString()
                    ddBudgeted.SelectedValue = ds.Tables(0).Rows(0).Item("Budgeted").ToString()
                    ddUGNFacility.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                    ddDepartment.SelectedValue = ds.Tables(0).Rows(0).Item("DeptOrCostCenter").ToString()

                    hfYear.Value = ds.Tables(0).Rows(0).Item("Year").ToString()
                    hfMake.Value = ds.Tables(0).Rows(0).Item("Make").ToString()
                    hfModel.Value = ds.Tables(0).Rows(0).Item("Model").ToString()
                    hfProgram.Value = ds.Tables(0).Rows(0).Item("ProgramID").ToString()
                    hfPreDev.Value = ds.Tables(0).Rows(0).Item("PreDvp").ToString()
                    hfCommodityID.Value = ds.Tables(0).Rows(0).Item("CommodityID").ToString()
                    txtProjDateNotes.Text = ds.Tables(0).Rows(0).Item("ProjDtNotes").ToString()
                    txtJustification.Text = ds.Tables(0).Rows(0).Item("Justification").ToString()
                    txtDateSubmitted.Text = ds.Tables(0).Rows(0).Item("DateSubmitted").ToString()

                    txtEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDt").ToString()
                    txtEstSpendDt.Text = ds.Tables(0).Rows(0).Item("EstSpendDt").ToString()
                    txtEstEndSpendDt.Text = ds.Tables(0).Rows(0).Item("EstEndSpendDt").ToString()
                    txtVoidReason.Text = ds.Tables(0).Rows(0).Item("VoidReason").ToString()

                    lblCClass.Text = ds.Tables(0).Rows(0).Item("Commodity_Classification").ToString()
                    lblCommodity.Text = ds.Tables(0).Rows(0).Item("CommodityName").ToString()
                    txtSOP.Text = ds.Tables(0).Rows(0).Item("VehicleSOP").ToString()
                    lblPreDev.Text = IIf(ds.Tables(0).Rows(0).Item("PreDvp") = False, "No", "Yes")
                    txtCustomer.Text = ds.Tables(0).Rows(0).Item("ddCustomerValue").ToString()

                    ''**** Expenditure
                    txtMaterials.Text = Format(ds.Tables(0).Rows(0).Item("Materials"), "#,##0.00")
                    txtLaborOH.Text = Format(ds.Tables(0).Rows(0).Item("LaborOH"), "#,##0.00")
                    txtPackaging.Text = Format(ds.Tables(0).Rows(0).Item("Packaging"), "#,##0.00")
                    txtFreight.Text = Format(ds.Tables(0).Rows(0).Item("Freight"), "#,##0.00")
                    txtTravelExpenditures.Text = Format(ds.Tables(0).Rows(0).Item("TravelExpenditures"), "#,##0.00")
                    txtNITUGN.Text = Format(ds.Tables(0).Rows(0).Item("NITUGN"), "#,##0.00")
                    txtReiter.Text = Format(ds.Tables(0).Rows(0).Item("FarmingtonAcousticTestingCharges"), "#,##0.00")
                    txtOtherTesting.Text = Format(ds.Tables(0).Rows(0).Item("OtherTesting"), "#,##0.00")
                    txtCustReimb.Text = Format(ds.Tables(0).Rows(0).Item("CustReimb"), "#,##0.00")
                    lblTotalRequest.Text = Format(ds.Tables(0).Rows(0).Item("TotalRequest"), "#,##0.00")
                    lblTotalInvestment1.Text = Format(ds.Tables(0).Rows(0).Item("TotalInv"), "#,##0.00")
                    txtHDTotalInvestment.Text = Format(ds.Tables(0).Rows(0).Item("OrigTotalInv"), "#,##0.00")
                    txtGeneralNotes.Text = ds.Tables(0).Rows(0).Item("GeneralNotes").ToString()

                    ''**** Justification
                    ds2 = EXPModule.GetCostReductionList(IIf(ddUGNFacility.SelectedValue = "", "", ddUGNFacility.SelectedValue))
                    If commonFunctions.CheckDataSet(ds2) = True Then
                        ddCRProjNo.DataSource = ds2
                        ddCRProjNo.DataTextField = ds2.Tables(0).Columns("ddProjNoDesc").ColumnName.ToString()
                        ddCRProjNo.DataValueField = ds2.Tables(0).Columns("ProjectNo").ColumnName.ToString()
                        ddCRProjNo.DataBind()
                        ddCRProjNo.Items.Insert(0, "")
                    End If

                    cbCRProjNoReq.Checked = ds.Tables(0).Rows(0).Item("CRProjectNoRequested").ToString()
                    If cbCRProjNoReq.Checked = True Then
                        cbCRProjNoReq.Text = "Submitted"
                    End If

                    txtDevSavings.Text = Format(ds.Tables(0).Rows(0).Item("DevSavings"), "#,##0.00")
                    txtScrapSavings.Text = Format(ds.Tables(0).Rows(0).Item("ScrapSavings"), "#,##0.00")
                    txtConsumableSavings.Text = Format(ds.Tables(0).Rows(0).Item("ConsumSavings"), "#,##0.00")
                    txtLaborSavings.Text = Format(ds.Tables(0).Rows(0).Item("LaborSavings"), "#,##0.00")
                    txtOtherSavings.Text = Format(ds.Tables(0).Rows(0).Item("OtherSavings"), "#,##0.00")

                    ''Bind Communication Board
                    If ViewState("pRID") <> 0 Then
                        ds = EXPModule.GetDevelopmentExpProjRSS(ViewState("pProjNo"), ViewState("pRID"))
                        If commonFunctions.CheckDataSet(ds) = True Then
                            txtQC.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                        Else 'no record found reset query string pRptID
                            Response.Redirect("DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pRID=0&pRC=1", False)
                        End If
                    End If
                End If
            Else
                If ViewState("pProjNo") = "" And ViewState("pFC") <> "" Then
                    lblProjectID.Text = Left(ViewState("pFC"), 5) & "??"
                    txtProjectID.Text = ""
                    txtOrigProjectID.Text = ""

                    ds = EXPModule.GetExpProjDevelopment(ViewState("pFC"), "", "", 0, 0, 0, "", 0, 0, "", 0, "")
                    If commonFunctions.CheckDataSet(ds) = True Then
                        txtYear.Text = ds.Tables(0).Rows(0).Item("Year").ToString()
                        cddMakes.SelectedValue = ds.Tables(0).Rows(0).Item("Make").ToString()
                        cddModel.SelectedValue = ds.Tables(0).Rows(0).Item("Model").ToString()
                        cddProgram.SelectedValue = ds.Tables(0).Rows(0).Item("ProgramID").ToString()
                        ddProjectLeader.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectLeaderTMID").ToString()
                        ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AcctMgrTMID").ToString()
                        ddBudgeted.SelectedValue = ds.Tables(0).Rows(0).Item("Budgeted").ToString()
                        ddUGNFacility.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                        ddDepartment.SelectedValue = ds.Tables(0).Rows(0).Item("DeptOrCostCenter").ToString()
                        txtCustomer.Text = ds.Tables(0).Rows(0).Item("ddCustomerValue").ToString()
                        txtEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDt").ToString()
                        txtEstSpendDt.Text = ds.Tables(0).Rows(0).Item("EstSpendDt").ToString()
                        txtEstEndSpendDt.Text = ds.Tables(0).Rows(0).Item("EstEndSpendDt").ToString()
                    End If
                End If 'EOF If ViewState("pProjNo") = "" And ViewState("pFC") <> "" Then
            End If 'EOF  If ProjNo <> Nothing Then
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
        Response.Redirect("DevelopmentExpProj.aspx?pFC=" & ViewState("pProjNo"), False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnAppend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAppend.Click
        Response.Redirect("DevelopmentExpProj.aspx?pProjNo=&pPrntProjNo=" & ViewState("pProjNo"), False)
    End Sub 'EOF btnAppend_Click

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click, btnExpenditure.Click
        Try
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            Dim DefaultDate As Date = Date.Today
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim ProjectStatus As String = Nothing
            If txtRoutingStatus.Text = "N" Or txtRoutingStatus.Text = "A" Or txtRoutingStatus.Text = "C" Then
                ProjectStatus = ddProjectStatus.SelectedValue
            Else
                ProjectStatus = ddProjectStatus2.SelectedValue
            End If


            ''**********************************************
            ''Kick out if there is an Obsolete selection
            ''**********************************************
            If InStr(ddDepartment.SelectedItem.Text, "**") Then
                lblErrors.Text = "Invalid Department Selection. System does not allow obsoleted items."
                lblErrors.Visible = True
                Exit Sub
            End If

            If (ViewState("pProjNo") <> Nothing Or ViewState("pProjNo") <> "") Then
                '***************
                '* Update Data
                '***************
                UpdateRecord(ProjectStatus, IIf(ProjectStatus = "Completed", "C", IIf(ProjectStatus = "Void", "V", IIf(ProjectStatus = "Open", "N", IIf(ProjectStatus = "Approved", "A", txtRoutingStatus.Text)))))


                '**************
                '* Reload the data - may contain calculated information to TotalInv
                '**************
                BindData(ViewState("pProjNo"))

                ''*************
                ''Check for Completed & Void status, send email notfication 
                ''*************
                If ProjectStatus = "Completed" And txtRoutingStatus.Text = "C" Then
                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), lblProjectTitle.Text, DefaultTMID, "Completed", "", "", "", "")
                    If txtRoutingStatus.Text <> "N" Then 'And (txtHDEstCmpltDt.Text = txtNextEstCmpltDt.Text)
                        SendNotifWhenEventChanges("Completed")
                    End If
                ElseIf ProjectStatus = "Void" And txtRoutingStatus.Text = "V" Then
                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), lblProjectTitle.Text, DefaultTMID, "VOID - Reason:" & txtVoidReason.Text, "", "", "", "")
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Void")
                    End If
                End If
            Else 'New Record
                Dim ProjectTitle As String = Nothing
                Dim ProjectCode As String = Nothing
                Dim PreDvp As String = Nothing
                If (ViewState("pPrntProjNo") = Nothing Or ViewState("pPrntProjNo") = "") Then
                    Dim Program As String = Nothing
                    Dim Commodity As String = Nothing
                    Dim ds1 As DataSet = New DataSet
                    ds1 = commonFunctions.GetPlatformProgram(0, ddProgram.SelectedValue, "", "", "")
                    If (commonFunctions.CheckDataSet(ds1) = True) Then
                        Program = ds1.Tables(0).Rows(0).Item("Program").ToString()
                    Else
                        lblErrors.Text = "Error in save routine. Unable to locate Program Code. Please contact IS Applications Group."
                        lblErrors.Visible = True
                        Exit Sub
                    End If
                    Dim ds2 As DataSet = New DataSet
                    ds2 = commonFunctions.GetCommodity(ddCommodity.SelectedValue, "", "", 0)
                    If (commonFunctions.CheckDataSet(ds2) = True) Then
                        Commodity = ds2.Tables(0).Rows(0).Item("ddCommodityName").ToString()
                        ProjectCode = ds2.Tables(0).Rows(0).Item("ProjectCode").ToString()
                        PreDvp = ds2.Tables(0).Rows(0).Item("PreDevCode").ToString()
                    Else
                        lblErrors.Text = "Error in save routine. Unable to locate Project Code. Please contact IS Applications Group."
                        lblErrors.Visible = True
                        Exit Sub
                    End If

                    ProjectTitle = txtYear.Text & " " & ddMakes.SelectedValue & " " & ddModel.SelectedValue & " " & Program & " " & Commodity
                Else
                    ProjectTitle = lblProjectTitle.Text
                End If

                '***************
                '* Locate Next available ProjectNo based on Facility selection
                '***************
                If ViewState("pFC") <> "" Then
                    If ddPreDvp.SelectedValue = True Then
                        ViewState("pProjNo") = Left(ViewState("pFC"), 5) & "" & PreDvp
                    Else
                        ViewState("pProjNo") = Left(ViewState("pFC"), 5) & "" & ProjectCode
                    End If

                    '***************
                    '* Save Data for new Parent Project
                    '***************
                    EXPModule.InsertExpProjDevelopment(ViewState("pProjNo"), ViewState("pPrntProjNo"), txtDateSubmitted.Text, ddRequestedBy.SelectedValue, ddProjectLeader.SelectedValue, ddAccountManager.SelectedValue, ProjectTitle, txtYear.Text, ddProgram.SelectedValue, txtSOP.Text, ddCommodity.SelectedValue, ddBudgeted.SelectedValue, ddUGNFacility.SelectedValue, ddDepartment.SelectedValue, txtProjDateNotes.Text, txtJustification.Text, txtEstCmpltDt.Text, txtEstSpendDt.Text, txtEstEndSpendDt.Text, lblPrntAppDate.Text, "Open", "N", ddPreDvp.SelectedValue, DefaultUser, DefaultDate)
                Else

                    Dim ds As DataSet = Nothing
                    Dim NextAvailProjNo As String = Nothing
                    ds = EXPModule.GetNextExpProjectNo(ViewState("pPrntProjNo"), "", "Development")
                    If commonFunctions.CheckDataSet(ds) = True Then
                        NextAvailProjNo = CType(ds.Tables(0).Rows(0).Item("NextAvailProjNo").ToString, String)
                        If (ViewState("pPrntProjNo") = Nothing Or ViewState("pPrntProjNo") = "") Then
                            If ddPreDvp.SelectedValue = True Then
                                ViewState("pProjNo") = NextAvailProjNo & "" & PreDvp
                            Else
                                ViewState("pProjNo") = NextAvailProjNo & "" & ProjectCode
                            End If

                            '***************
                            '* Save Data for new Parent Project
                            '***************
                            EXPModule.InsertExpProjDevelopment(ViewState("pProjNo"), ViewState("pPrntProjNo"), txtDateSubmitted.Text, ddRequestedBy.SelectedValue, ddProjectLeader.SelectedValue, ddAccountManager.SelectedValue, ProjectTitle, txtYear.Text, ddProgram.SelectedValue, txtSOP.Text, ddCommodity.SelectedValue, ddBudgeted.SelectedValue, ddUGNFacility.SelectedValue, ddDepartment.SelectedValue, txtProjDateNotes.Text, txtJustification.Text, txtEstCmpltDt.Text, txtEstSpendDt.Text, txtEstEndSpendDt.Text, lblPrntAppDate.Text, "Open", "N", ddPreDvp.SelectedValue, DefaultUser, DefaultDate)

                        Else
                            ViewState("pProjNo") = NextAvailProjNo
                            If Len(ViewState("pProjNo")) <= 5 Then
                                lblErrors.Text = "Error locating next sequential project no.  Please contact IS Applications Group."
                                lblErrors.Visible = True
                                Exit Sub
                            End If
                            '***************
                            '* Save Data for new Supplement Project
                            '***************
                            EXPModule.InsertExpProjDevelopment(ViewState("pProjNo"), ViewState("pPrntProjNo"), txtDateSubmitted.Text, ddRequestedBy.SelectedValue, ddProjectLeader.SelectedValue, ddAccountManager.SelectedValue, ProjectTitle, txtYear.Text, hfProgram.Value, txtSOP.Text, hfCommodityID.Value, ddBudgeted.SelectedValue, ddUGNFacility.SelectedValue, ddDepartment.SelectedValue, txtProjDateNotes.Text, txtJustification.Text, txtEstCmpltDt.Text, txtEstSpendDt.Text, txtEstEndSpendDt.Text, lblPrntAppDate.Text, "Open", "N", hfPreDev.Value, DefaultUser, DefaultDate)

                        End If 'EOF If (ViewState("pPrntProjNo") = Nothing Or ViewState("pPrntProjNo") = "") Then

                    Else
                        lblErrors.Text = "Error locating next sequential project no.  Please contact IS Applications Group."
                        lblErrors.Visible = True
                        CheckRights()
                        Exit Sub
                    End If

                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Record created.", "", "", "", "")

                    ''*******************
                    ''Build Approval List
                    ''*******************
                    BuildApprovalList()
                End If 'eOF If ViewState("pFC") = "" Then


                '***************
                '* Redirect user back to the page.
                '***************
                Dim Aprv As String = Nothing
                If ViewState("pAprv") = 1 Then
                    Aprv = "&pAprv=1"
                End If
                Response.Redirect("DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pPrntProjNo=" & ViewState("pPrntProjNo") & Aprv, False)
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

    Protected Sub btnReset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset1.Click

        If ViewState("pProjNo") <> "" Then
            Response.Redirect("DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pPrntProjNo=" & ViewState("pPrntProjNo"), False)
        Else
            Response.Redirect("DevelopmentExpProjList.aspx", False)
        End If
    End Sub 'EOF btnReset1_Click

    Protected Sub btnReset2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset2.Click, btnReset3.Click, btnReset4.Click

        Dim TempViewState As Integer

        If ViewState("pProjNo") <> "" Then
            TempViewState = mvTabs.ActiveViewIndex
            mvTabs.GetActiveView()
            mnuTabs.Items(TempViewState).Selected = True

            BindData(ViewState("pProjNo"))
        Else
            'Response.Redirect("DevelopmentExpProj.aspx" & IIf(ViewState("pFC") <> "", "?pFC=" & ViewState("pFC"), ""), False)
            Response.Redirect("DevelopmentExpProj.aspx", False)
        End If
    End Sub 'EOF btnReset1_Click

    Protected Function CalcTotalRequest() As Decimal
        Dim Materials As Decimal = IIf(txtMaterials.Text = Nothing, 0, txtMaterials.Text)
        Dim LaborOH As Decimal = IIf(txtLaborOH.Text = Nothing, 0, txtLaborOH.Text)
        Dim Packaging As Decimal = IIf(txtPackaging.Text = Nothing, 0, txtPackaging.Text)
        Dim Freight As Decimal = IIf(txtFreight.Text = Nothing, 0, txtFreight.Text)
        Dim TravelExp As Decimal = IIf(txtTravelExpenditures.Text = Nothing, 0, txtTravelExpenditures.Text)
        Dim NitUGN As Decimal = IIf(txtNITUGN.Text = Nothing, 0, txtNITUGN.Text)
        Dim Reiter As Decimal = IIf(txtReiter.Text = Nothing, 0, txtReiter.Text)
        Dim OtherTesting As Decimal = IIf(txtOtherTesting.Text = Nothing, 0, txtOtherTesting.Text)
        Dim TotalRequest As Decimal = 0

        TotalRequest = Materials + LaborOH + Packaging + Freight + TravelExp + NitUGN + Reiter + OtherTesting

        Return TotalRequest
    End Function 'EOF CalcTotalRequest

    Protected Function CalcTotalInv(ByVal TotalRequest As Decimal) As Decimal
        Dim TotalInv As Decimal = 0
        Dim CustReimb As Decimal = IIf(txtCustReimb.Text = Nothing, 0, txtCustReimb.Text)
        TotalInv = TotalRequest - CustReimb

        Return TotalInv
    End Function 'EOF CalcTotalInv

    Public Function UpdateRecord(ByVal RecStatus As String, ByVal RoutingStatus As String) As String
        Try
            Dim DefaultDate As Date = Date.Today
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

            Dim Materials As Decimal = IIf(txtMaterials.Text = Nothing, 0, txtMaterials.Text)
            Dim LaborOH As Decimal = IIf(txtLaborOH.Text = Nothing, 0, txtLaborOH.Text)
            Dim Packaging As Decimal = IIf(txtPackaging.Text = Nothing, 0, txtPackaging.Text)
            Dim Freight As Decimal = IIf(txtFreight.Text = Nothing, 0, txtFreight.Text)
            Dim TravelExp As Decimal = IIf(txtTravelExpenditures.Text = Nothing, 0, txtTravelExpenditures.Text)
            Dim NitUGN As Decimal = IIf(txtNITUGN.Text = Nothing, 0, txtNITUGN.Text)
            Dim Reiter As Decimal = IIf(txtReiter.Text = Nothing, 0, txtReiter.Text)
            Dim OtherTesting As Decimal = IIf(txtOtherTesting.Text = Nothing, 0, txtOtherTesting.Text)
            Dim TotalRequest As Decimal = CalcTotalRequest()
            Dim CustReimb As Decimal = IIf(txtCustReimb.Text = Nothing, 0, txtCustReimb.Text)
            Dim TotalInv As Decimal = CalcTotalInv(TotalRequest)

            Dim DevSavings As Decimal = IIf(txtDevSavings.Text = Nothing, 0, txtDevSavings.Text)
            Dim ScrapSavings As Decimal = IIf(txtScrapSavings.Text = Nothing, 0, txtScrapSavings.Text)
            Dim ConsumSavings As Decimal = IIf(txtConsumableSavings.Text = Nothing, 0, txtConsumableSavings.Text)
            Dim LaborSavings As Decimal = IIf(txtLaborSavings.Text = Nothing, 0, txtLaborSavings.Text)
            Dim OtherSavings As Decimal = IIf(txtOtherSavings.Text = Nothing, 0, txtOtherSavings.Text)

            Dim EstCmpltDt As String = txtEstCmpltDt.Text 'txtHDEstCmpltDt.Text
            Dim SendEmailToDefaultAdmin As Boolean = False

            Dim ProjectNo As String = IIf(ViewState("pProjNo") <> txtProjectID.Text, txtProjectID.Text, ViewState("pProjNo"))


            '***************
            '* Update Data
            '***************
            EXPModule.UpdateExpProjDevelopment(ProjectNo, txtOrigProjectID.Text, ViewState("pPrntProjNo"), txtDateSubmitted.Text, ddRequestedBy.SelectedValue, ddProjectLeader.SelectedValue, ddAccountManager.SelectedValue, lblProjectTitle.Text, txtSOP.Text, ddBudgeted.SelectedValue, ddUGNFacility.SelectedValue, ddDepartment.SelectedValue, txtProjDateNotes.Text, txtJustification.Text, EstCmpltDt, txtEstSpendDt.Text, txtEstEndSpendDt.Text, IIf(ddCRProjNo.SelectedValue = "", 0, ddCRProjNo.SelectedValue), cbCRProjNoReq.Checked, txtGeneralNotes.Text, Materials, LaborOH, Packaging, Freight, TravelExp, NitUGN, Reiter, OtherTesting, TotalRequest, CustReimb, TotalInv, txtVoidReason.Text, RecStatus, RoutingStatus, DevSavings, ScrapSavings, ConsumSavings, LaborSavings, OtherSavings, DefaultUser, DefaultDate)


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

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            '***************
            '* Delete Record
            '***************
            If ViewState("pPrntProjNo") = Nothing Then
                EXPModule.DeleteExpProjDevelopment(ViewState("pProjNo"), ViewState("pPrntProjNo"), False)
            Else
                EXPModule.DeleteExpProjDevelopment(ViewState("pProjNo"), ViewState("pPrntProjNo"), True)
            End If

            '***************
            '* Redirect user back to the search page.
            '***************
            Response.Redirect("DevelopmentExpProjList.aspx", False)

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

    Protected Sub btnBuildApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuildApproval.Click

        BuildApprovalList()
        gvApprovers.DataBind()

        mvTabs.ActiveViewIndex = Int32.Parse(3)
        mvTabs.GetActiveView()
        mnuTabs.Items(3).Selected = True

    End Sub 'EOF btnBuildApproval

    Public Function BuildApprovalList() As String
        Try
            ''********
            ''* This function is used to build the Approval List
            ''********
            Dim DefaultDate As Date = Date.Today
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim CurrentEmpEmail As String = Nothing
            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value

            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False
            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pProjNo") <> Nothing Then

                    ' ''If (txtRoutingStatus.Text <> "R") Then
                    ''***************
                    ''* Delete 1st Level Approval for rebuild
                    ''***************
                    EXPModule.DeleteExpProjDevelopmentApproval(ViewState("pProjNo"), 0)

                    '***************
                    '* Build 1st Level Approval
                    '***************
                    EXPModule.InsertExpProjDevelopmentApproval(ViewState("pProjNo"), ddUGNFacility.SelectedValue, 128, DefaultUser, DefaultDate)

                    '***************
                    '* Build 2nd Level Approval
                    '***************
                    EXPModule.InsertExpProjDevelopmentApproval(ViewState("pProjNo"), ddUGNFacility.SelectedValue, 129, DefaultUser, DefaultDate)

                    '***************
                    '* Build 3rd Level Approval
                    '***************
                    EXPModule.InsertExpProjDevelopmentApproval(ViewState("pProjNo"), ddUGNFacility.SelectedValue, 130, DefaultUser, DefaultDate)
                    ''End If
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
        Return True
    End Function 'EOF BuildApprovalList

    Protected Sub ddProjectStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProjectStatus.SelectedIndexChanged
        Select Case ddProjectStatus.SelectedValue
            Case "Void"
                txtVoidReason.Visible = True
                lblReqVoidRsn.Visible = True
                lblVoidRsn.Visible = True
                rfvVoidReason.Enabled = True
            Case Else
                lblReqVoidRsn.Visible = False
                lblVoidRsn.Visible = False
                rfvVoidReason.Enabled = False
        End Select
    End Sub 'EOF ddProjectStatus_SelectedIndexChanged
#End Region

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
                    Dim price As ExpProjDevelopment.ExpProj_Development_ApprovalRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProjDevelopment.ExpProj_Development_ApprovalRow)

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

    Protected Sub gvApprovers_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Try
            Dim row As GridViewRow = gvApprovers.Rows(e.RowIndex)
            If row IsNot Nothing Then
                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                Dim DefaultDate As Date = Date.Today

                Dim t As DropDownList = TryCast(row.FindControl("ddStatus"), DropDownList)
                Dim c As TextBox = TryCast(row.FindControl("txtAppComments"), TextBox)
                Dim tm As TextBox = TryCast(row.FindControl("txtTeamMemberID"), TextBox)
                Dim otm As TextBox = TryCast(row.FindControl("txtOrigTeamMemberID"), TextBox)
                Dim TeamMemberID As Integer = CType(tm.Text, Integer)
                Dim OrigTeamMemberID As Integer = CType(otm.Text, Integer)
                Dim s As TextBox = TryCast(row.FindControl("hfSeqNo"), TextBox)
                Dim hfSeqNo As Integer = CType(s.Text, Integer)
                Dim ds As DataSet = New DataSet

                lblErrors.Text = Nothing
                lblErrors.Visible = Nothing
                lblReqAppComments.Visible = False
                lblReqAppComments.Text = Nothing


                If (t.Text <> "Pending") Then
                    If (c.Text <> Nothing Or c.Text <> "") Then
                        ds = SecurityModule.GetTeamMember(TeamMemberID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                        Dim ShortName As String = ds.Tables(0).Rows(0).Item("ShortName").ToString()

                        Dim ds1st As DataSet = New DataSet
                        Dim ds2nd As DataSet = New DataSet
                        Dim dsCC As DataSet = New DataSet
                        Dim dsRej As DataSet = New DataSet
                        Dim dsCommodity As DataSet = New DataSet
                        Dim EmailFrom As String = Nothing
                        Dim EmailTO As String = Nothing
                        Dim EmpName As String = Nothing
                        Dim EmailCC As String = Nothing
                        Dim SponsSameAs1stLvlAprvr As Boolean = False
                        Dim i As Integer = 0
                        Dim ProjectStatus As String = Nothing
                        Dim LvlApvlCmplt As Boolean = False

                        Dim CurrentEmpEmail As String = Nothing
                        If HttpContext.Current.Request.Cookies("UGNDB_User_Email") IsNot Nothing Then
                            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                            EmailFrom = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                            EmailCC = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                        Else
                            CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                        End If

                        '********
                        '* Only users with valid email accounts can send an email.
                        '********
                        If CurrentEmpEmail <> Nothing Then
                            If ViewState("pProjNo") <> Nothing Then
                                If t.SelectedValue = "Rejected" And c.Text = Nothing Then
                                    lblErrors.Text = "Your comments is required for Rejection."
                                    lblErrors.Visible = True
                                    lblReqAppComments.Text = "Your comments is required for Rejection."
                                    lblReqAppComments.Visible = True
                                    lblReqAppComments.ForeColor = Color.Red
                                    Exit Sub

                                Else
                                    ''***************
                                    ''Verify that atleast one Supporting Document is entered
                                    ''***************
                                    Dim ReqDocFound As Boolean = VerifySupportingDocument()
                                    If ReqDocFound = False Then
                                        Exit Sub
                                    End If 'ReqDocFound = false

                                    ''*************************************
                                    ''*Verify that the TotalRequest > $0
                                    ''*************************************
                                    Dim bTotalRequest As Boolean = VerifyTotalRequest()
                                    If bTotalRequest = True Then
                                        Exit Sub
                                    End If

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
                                            NextLvl = 129
                                        Case 2
                                            SeqNo = 2
                                            NextSeqNo = 3
                                            NextLvl = 130
                                        Case 3
                                            SeqNo = 3
                                            NextSeqNo = 0
                                            NextLvl = 130
                                    End Select

                                    If t.SelectedValue = "Approved" And SeqNo = 3 Then
                                        ProjectStatus = "Approved"
                                    Else
                                        ProjectStatus = "In Process"
                                    End If


                                    ''*****************
                                    ''Update Record
                                    ''*****************
                                    UpdateRecord(ProjectStatus, IIf(SeqNo = 3, IIf(t.SelectedValue = "Rejected", "R", "A"), IIf(t.SelectedValue = "Rejected", "R", "T")))

                                    ''*****************
                                    ''History Tracking
                                    ''*****************
                                    EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), lblProjectTitle.Text, DefaultTMID, (DefaultUser.ToUpper) & " " & t.Text & " this record on behalf of " & ShortName.ToUpper & " - Comments: " & c.Text, "", "", "", "")


                                    ''***********************************
                                    ''Update Current Level Approver record.
                                    ''***********************************
                                    EXPModule.UpdateExpProjDevelopmentApproval(ViewState("pProjNo"), DefaultTMID, True, t.SelectedValue, c.Text, SeqNo, 0, DefaultUser, DefaultDate)

                                    ''*******************************
                                    ''Locate Next Approver
                                    ''*******************************
                                    ' ''Check at same sequence level
                                    ds1st = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), SeqNo, TeamMemberID, True, False)
                                    If commonFunctions.CheckDataSet(ds1st) = True Then
                                        ' DO NOTHING
                                    Else 'EOF  commonFunctions.CheckDataSet(ds1st) = false
                                        If t.SelectedValue <> "Rejected" And SeqNo <> 3 Then
                                            ds2nd = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), NextSeqNo, 0, True, False)
                                            If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                                                For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                                    If (ds2nd.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) And ((ddAccountManager.SelectedValue <> ds2nd.Tables(0).Rows(i).Item("TeamMemberID")) Or (ddProjectLeader.SelectedValue <> ds2nd.Tables(0).Rows(i).Item("TeamMemberID"))) Then ''change to DefaultTMID   
                                                        If EmailTO = Nothing Then
                                                            EmailTO = ds2nd.Tables(0).Rows(i).Item("Email")
                                                        Else
                                                            EmailTO = EmailTO & ";" & ds2nd.Tables(0).Rows(i).Item("Email")
                                                        End If
                                                        If EmpName = Nothing Then
                                                            EmpName = ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "
                                                        Else
                                                            EmpName = EmpName & ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "
                                                        End If
                                                        ''*****************************************
                                                        ''Update Approvers DateNotified field.
                                                        ''*****************************************
                                                        EXPModule.UpdateExpProjDevelopmentApproval(ViewState("pProjNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", NextSeqNo, 0, DefaultUser, DefaultDate)
                                                    End If
                                                Next
                                            End If 'EOF ds2nd.Tables.Count > 0 
                                        End If
                                    End If 'EOF ds1st.Tables.Count > 0

                                    'Rejected or last approval
                                    If t.SelectedValue = "Rejected" Or (SeqNo = 3 And t.SelectedValue = "Approved") Then
                                        ''********************************************************
                                        ''Notify Project Lead/Requested By
                                        ''********************************************************
                                        dsRej = EXPModule.GetExpProjDevelopmentLead(ViewState("pProjNo"))
                                        ''Check that the recipient(s) is a valid Team Member
                                        If dsRej.Tables.Count > 0 And (dsRej.Tables.Item(0).Rows.Count > 0) Then
                                            For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                                If (dsRej.Tables(0).Rows(i).Item("TMDesc") <> "Account Manager") And (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                                    If EmailTO = Nothing Then
                                                        EmailTO = dsRej.Tables(0).Rows(i).Item("Email")
                                                    Else
                                                        EmailTO = EmailTO & ";" & dsRej.Tables(0).Rows(i).Item("Email")
                                                    End If
                                                    If EmpName = Nothing Then
                                                        EmpName = dsRej.Tables(0).Rows(i).Item("TMName") & ", "
                                                    Else
                                                        EmpName = EmpName & dsRej.Tables(0).Rows(i).Item("TMName") & ", "
                                                    End If
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
                                            EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)

                                            If SeqNo = 3 Then
                                                ''**************************************************************
                                                ''Carbon Copy 1 Level Approvers
                                                ''**************************************************************
                                                EmailCC = CarbonCopyList(MyMessage, (NextLvl - 2), ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)

                                                ''**************************************************************
                                                ''Carbon Copy Last Level Approvers
                                                ''**************************************************************
                                                EmailCC = CarbonCopyList(MyMessage, NextLvl, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)
                                            End If
                                        Else
                                            EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)
                                        End If

                                        If (SeqNo < 3 And t.SelectedValue <> "Rejected") Then
                                            ''**************************************************************
                                            ''*Carbon Copy Account Manager & Project Lead
                                            ''**************************************************************
                                            EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                                        ElseIf (SeqNo = 3 And t.SelectedValue <> "Rejected") Then
                                            ''**************************************************************
                                            ''*Carbon Copy Account Manager & Project Lead
                                            ''**************************************************************
                                            EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                                            ''********************************
                                            ''Carbon Copy Accounting
                                            ''********************************
                                            EmailCC = CarbonCopyList(MyMessage, 87, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)
                                            ''**************************************
                                            ''*Carbon Copy the Cc List
                                            ''**************************************
                                            EmailCC = CarbonCopyList(MyMessage, 131, "", 0, 0, EmailCC, DefaultTMID)

                                            ''*********************************************************
                                            ''*Carbon Copy the Operations Manager based on UGNFacility
                                            ''*********************************************************
                                            EmailCC = CarbonCopyList(MyMessage, 132, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)
                                        End If 'EOF If (SeqNo < 3 And t.SelectedValue <> "Rejected") Then


                                        If t.SelectedValue = "Rejected" Then
                                            MyMessage.Subject = " Development Project: " & ViewState("pProjNo") & " - " & lblProjectTitle.Text & " - REJECTED"
                                            MyMessage.Body = "<font size='2' face='Tahoma'>" & EmpName & "</font>"
                                            MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & ViewState("pProjNo") & " '" & lblProjectTitle.Text & "' was <font color='red'>REJECTED</font>. <br/>Reason for rejection: " & c.Text & "</p> <p><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</font></p>"
                                        Else
                                            MyMessage.Subject = "Development Project: " & ViewState("pProjNo") & " - " & lblProjectTitle.Text
                                            If SeqNo = 3 Then
                                                MyMessage.Subject &= "- APPROVED"

                                                ''Redirect users to Preview Form at final Approval
                                                MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & ViewState("pProjNo") & " '" & lblProjectTitle.Text & "' is Approved. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</font></p>"
                                            Else
                                                MyMessage.Body = "<font size='2' face='Tahoma'>" & EmpName & "</font>"

                                                ''Redirect users to Approval screen if not final approval
                                                MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & ViewState("pProjNo") & " '" & lblProjectTitle.Text & "' is available for your Review/Approval. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/crExpProjDevelopmentApproval.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</font></p>"
                                            End If
                                        End If

                                        ''*******************
                                        ''Build Email Body
                                        ''*******************
                                        EmailBody(MyMessage)

                                        Dim emailList As String() = commonFunctions.CleanEmailList(EmailCC).Split(";")
                                        Dim ccEmail As String = Nothing
                                        For i = 0 To UBound(emailList)
                                            If emailList(i) <> ";" And emailList(i).Trim <> "" And emailList(i) <> EmailTO Then
                                                ccEmail += emailList(i) & ";"
                                            End If
                                        Next i
                                        EmailCC = ccEmail

                                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                                            MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                                            EmailFrom = "Database.Notifications@ugnauto.com"
                                            EmailTO = CurrentEmpEmail
                                            EmailCC = "lynette.rey@ugnauto.com"
                                        End If

                                        ''**********************************
                                        ''Connect & Send email notification
                                        ''**********************************
                                        Try
                                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (D)", ViewState("pProjNo"))
                                            lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                            lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."
                                        Catch ex As Exception
                                            lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                            lblReqAppComments.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                            UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                            'get current event name
                                            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                                            'log and email error
                                            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                                        End Try
                                        lblErrors.Visible = True
                                        lblReqAppComments.Visible = True
                                        lblReqAppComments.ForeColor = Color.Red

                                        ''*****************
                                        ''History Tracking
                                        ''*****************
                                        If t.SelectedValue <> "Rejected" Then
                                            If SeqNo = 3 Then
                                                EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), lblProjectTitle.Text, DefaultTMID, "Notification sent to all involved.", "", "", "", "")
                                            Else
                                                EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), lblProjectTitle.Text, DefaultTMID, "Notification sent to level " & IIf(SeqNo < 3, (SeqNo + 1), SeqNo) & " approver(s): " & EmpName, "", "", "", "")
                                            End If
                                        Else
                                            EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), lblProjectTitle.Text, DefaultTMID, "Notification sent to " & EmpName, "", "", "", "")
                                        End If

                                    End If 'If EmailTO <> Nothing Then
                                End If 'EOF If commonFunciton.CheckDataset(dsExp) = True
                            End If 'EOF If Rejected - Comments Required
                        End If 'EOF If Comments is not nothing
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

                    mvTabs.ActiveViewIndex = Int32.Parse(3)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(3).Selected = True

                Else
                    lblReqAppComments.Text = "Comments is a required field when approving for another team member."
                    lblReqAppComments.Visible = True
                    lblReqAppComments.ForeColor = Color.Red
                End If 'EOF If status is other than Pending
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
    End Sub 'EOF gvApprovers_RowUpdating
#End Region 'EOF Approval Status

#Region "Email Notifications"
    Protected Function VerifyTotalRequest() As Boolean
        Dim TotalRequest As Decimal = CalcTotalRequest()
        If TotalRequest = 0 Then
            mvTabs.ActiveViewIndex = Int32.Parse(2)
            mvTabs.GetActiveView()
            mnuTabs.Items(2).Selected = True

            lblErrors.Text = "Unable to submit a project with Total Request of $0.00, please Review."
            lblErrors.Font.Size = 12
            lblErrors.Visible = True
            lblReqAppComments.Text = "Unable to submit a project with Total Request of $0.00, please Review."
            lblReqAppComments.ForeColor = Color.Red
            lblReqAppComments.Visible = True

            Return True
        Else
            Return False
        End If
    End Function 'EOF VerifyTotalRequest

    Protected Function VerifySupportingDocument() As Boolean
        Dim dsDoc As DataSet = New DataSet
        dsDoc = EXPModule.GetDevelopmentExpDocument(ViewState("pProjNo"), 0)
        If commonFunctions.CheckDataSet(dsDoc) = False Then 'If missing kick user out from submission.
            mvTabs.ActiveViewIndex = Int32.Parse(1)
            mvTabs.GetActiveView()
            mnuTabs.Items(1).Selected = True

            lblErrors.Text = "Atleast one Supporting Document is required for submission."
            lblErrors.Font.Size = 12
            lblErrors.Visible = True
            lblReqAppComments.Text = "Atleast one Supporting Document is required for submission."
            lblReqAppComments.ForeColor = Color.Red
            lblReqAppComments.Visible = True

            Return False
        Else
            Return True
        End If

    End Function 'EOF VerifySupportingDocument

    Protected Sub btnFwdApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFwdApproval.Click
        Try
            ''********
            ''* This function is used to submit email to the Approvers after the Project Sponsor is done.
            ''********
            Dim DefaultDate As Date = Date.Today
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim ds1st As DataSet = New DataSet
            Dim ds2nd As DataSet = New DataSet
            Dim dsCC As DataSet = New DataSet
            Dim dsCommodity As DataSet = New DataSet
            Dim EmpName As String = Nothing
            Dim EmailTO As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing
            Dim SeqNo As Integer = 0
            Dim OrigTMID As Integer = 0
            Dim i As Integer = 0
            Dim SponsSameAs1stLvlAprvr As Boolean = False

            Dim CurrentEmpEmail As String = Nothing
            If HttpContext.Current.Request.Cookies("UGNDB_User_Email") IsNot Nothing Then
                CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailFrom = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailCC = CurrentEmpEmail
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
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pProjNo") <> Nothing Then
                    ''***********************
                    ''*Rebuild Approval Routing in the event the record was created days prior to submission
                    ''*Do this will default a list of Team Members that are currently available based on Workflow
                    ''*************************
                    If txtRoutingStatus.Text = "N" Then
                        BuildApprovalList()
                    Else
                        If txtRoutingStatus.Text = "R" And txtHDTotalInvestment.Text <> 0 And _
                        txtHDTotalInvestment.Text <> lblTotalInvestment1.Text Then
                            BuildApprovalList()
                            EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), lblProjectTitle.Text, DefaultTMID, "Total Investment amount changed from $" & txtHDTotalInvestment.Text & " to $" & lblTotalInvestment1.Text, "Total Investment", txtHDTotalInvestment.Text, lblTotalInvestment1.Text, "")
                        End If
                    End If

                    ''***************
                    ''Verify that atleast one Supporting Document is entered
                    ''***************
                    Dim ReqDocFound As Boolean = VerifySupportingDocument()
                    If ReqDocFound = False Then
                        Exit Sub
                    End If 'ReqDocFound = false

                    ''*************************************
                    ''*Verify that the TotalRequest > $0
                    ''*************************************
                    Dim bTotalRequest As Boolean = VerifyTotalRequest()
                    If bTotalRequest = True Then
                        Exit Sub
                    End If

                    ''**********************
                    ''* Save data prior to submission before approvals
                    ''**********************
                    UpdateRecord("In Process", "T")

                    ''*******************************
                    ''Locate 1st level approver
                    ''*******************************
                    Dim EM As String = Nothing
                    If (txtRoutingStatus.Text <> "R") Then
                        ds1st = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), 1, 0, False, False)
                    Else 'IF Rejected - only notify the TM who Rejected the record

                        If txtHDTotalInvestment.Text = lblTotalInvestment1.Text Then
                            ds1st = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), 0, 0, False, True)
                        Else
                            ds1st = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), 1, 0, False, False)
                        End If
                    End If
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(ds1st) = True Then
                        For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                            If (ds1st.Tables(0).Rows(i).Item("Email").ToString.ToUpper <> CurrentEmpEmail.ToUpper) And (ds1st.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And (ddProjectLeader.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID")) Then
                                If EM <> ds1st.Tables(0).Rows(i).Item("Email") Then
                                    If EmailTO = Nothing Then
                                        EmailTO = ds1st.Tables(0).Rows(i).Item("Email")
                                    Else
                                        EmailTO = EmailTO & ";" & ds1st.Tables(0).Rows(i).Item("Email")
                                    End If
                                    If EmpName = Nothing Then
                                        EmpName = ds1st.Tables(0).Rows(i).Item("EmailTMName") & ", "
                                    Else
                                        EmpName = EmpName & ds1st.Tables(0).Rows(i).Item("EmailTMName") & ", "
                                    End If
                                End If
                                ''************************************************************
                                ''Update 1st level DateNotified field.
                                ''************************************************************
                                If (txtRoutingStatus.Text <> "R") Then
                                    EXPModule.UpdateExpProjDevelopmentApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 1, 0, DefaultUser, DefaultDate)
                                Else 'IF Rejected - only notify the TM who Rejected the record
                                    EXPModule.UpdateExpProjDevelopmentApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), 0, DefaultUser, DefaultDate)
                                    SeqNo = ds1st.Tables(0).Rows(i).Item("SeqNo")
                                    OrigTMID = ds1st.Tables(0).Rows(i).Item("TeamMemberID")
                                End If
                            Else
                                ''************************************************************
                                ''1st Level Approver same as Project Sponsor.  Update record.DefaultTMID
                                ''************************************************************
                                If (txtRoutingStatus.Text <> "R") Then
                                    EXPModule.UpdateExpProjDevelopmentApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), True, "Approved", "", 1, 1, DefaultUser, DefaultDate)
                                Else 'IF Rejected - only notify the TM who Rejected the record
                                    EXPModule.UpdateExpProjDevelopmentApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Approved", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), 1, DefaultUser, DefaultDate)
                                End If

                                SponsSameAs1stLvlAprvr = True
                            End If 'EOF If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail)
                            EM = ds1st.Tables(0).Rows(i).Item("Email")
                        Next 'EOF For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                    End If 'EOF If commonFunctions.CheckDataset(ds1st) = True 

                    ''***************************************************************
                    ''Locate 2nd Level Approver(s)
                    ''***************************************************************
                    EM = Nothing
                    If SponsSameAs1stLvlAprvr = True And EmailTO = Nothing Then
                        ds2nd = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), 2, 0, False, False)
                        ''Check that the recipient(s) is a valid Team Member
                        If commonFunctions.CheckDataSet(ds2nd) = True Then
                            For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                If (ds2nd.Tables(0).Rows(i).Item("Email").ToString.ToUpper <> CurrentEmpEmail.ToUpper) Then
                                    If (ds2nd.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                        If EM <> ds2nd.Tables(0).Rows(i).Item("Email") Then
                                            If EmailTO = Nothing Then
                                                EmailTO = ds2nd.Tables(0).Rows(i).Item("Email")
                                            Else
                                                EmailTO = EmailTO & ";" & ds2nd.Tables(0).Rows(i).Item("Email")
                                            End If
                                            If EmpName = Nothing Then
                                                EmpName = ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "
                                            Else
                                                EmpName = EmpName & ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "
                                            End If
                                        End If
                                        ''************************************************************
                                        ''Update 2nd level DateNotified field.
                                        ''************************************************************
                                        EXPModule.UpdateExpProjDevelopmentApproval(ViewState("pProjNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 2, 0, DefaultUser, DefaultDate)
                                    End If
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

                        If (txtRoutingStatus.Text <> "R") Then 'Carbon Copy at first submission
                            ''********************************
                            ''Carbon Copy Account Manager/Project Leader/Requested By
                            ''********************************
                            EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                            ''********************************
                            ''Carbon Copy Ops Manager
                            ''********************************
                            EmailCC = CarbonCopyList(MyMessage, 132, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)
                        Else 'Rejected
                            ''********************************
                            ''Carbon Copy Same Level
                            ''********************************
                            EmailCC = CarbonCopyList(MyMessage, 0, "", SeqNo, OrigTMID, EmailCC, DefaultTMID)
                        End If

                        ''********************************
                        ''Carbon Copy Accounting
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 87, "", 0, 0, EmailCC, DefaultTMID)

                        ''Test or Production Message display
                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                            'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
                        End If

                        MyMessage.Subject &= "Development Project: " & ViewState("pProjNo") & " - " & lblProjectTitle.Text

                        MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName
                        MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & lblProjectTitle.Text & "' is available for your Review/Approval. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/crExpProjDevelopmentApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"

                        MyMessage.Body &= "</font>"

                        ''*******************
                        ''Build Email Body
                        ''*******************
                        EmailBody(MyMessage)

                        Dim emailList As String() = commonFunctions.CleanEmailList(EmailCC).Split(";")
                        Dim ccEmail As String = Nothing
                        For i = 0 To UBound(emailList)
                            If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                                ccEmail += emailList(i) & ";"
                            End If
                        Next i
                        EmailCC = ccEmail

                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                            MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                            EmailTO = "lynette.rey@ugnauto.com" 'CurrentEmpEmail
                            EmailCC = "lynette.rey@ugnauto.com"
                        End If

                        ''*****************
                        ''History Tracking
                        ''*****************
                        EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), lblProjectTitle.Text, DefaultTMID, "Record completed and forwarded to " & EmpName & " for approval.", "", "", "", "")

                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (D)", ViewState("pProjNo"))
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
                        lblErrors.Visible = True
                        lblReqAppComments.Visible = True
                        lblReqAppComments.ForeColor = Color.Red

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pProjNo"))
                        gvApprovers.DataBind()

                        ''*************************************************
                        '' "Form Level Security using Roles &/or Subscriptions"
                        ''*************************************************
                        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                        mvTabs.ActiveViewIndex = Int32.Parse(3)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(3).Selected = True
                    Else
                        If ViewState("DefaultUserFacility") = "UT" Then
                            lblErrors.Text = "Please Build Approval List prior to Submission."
                            lblErrors.Visible = True
                            lblReqAppComments.Text = "Please Build Approval List prior to Submission."
                            lblReqAppComments.Visible = True
                            lblReqAppComments.ForeColor = Color.Red
                        End If
                    End If 'EOF EmailTo <> Nothing
                    ' ''End If
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
                    dsCC = EXPModule.GetExpProjDevelopmentLead(ViewState("pProjNo"))
                    IncludeOrigAprvlTM = True
                Else '' All others based on facility
                    If UGNLoc <> Nothing Then
                        dsCC = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(SubscriptionID, UGNLoc)
                    Else
                        If SubscriptionID = 87 Or SubscriptionID = 131 Or SubscriptionID = 128 Or SubscriptionID = 129 Then
                            ''Notify Accounting, CC List, or 1st level or 2nd level
                            dsCC = commonFunctions.GetTeamMemberBySubscription(SubscriptionID)
                        End If
                    End If
                End If

                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("TMID") <> DefaultTMID) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                            If EmailCC = Nothing Then
                                EmailCC = dsCC.Tables(0).Rows(i).Item("Email")
                            Else
                                EmailCC = EmailCC & ";" & dsCC.Tables(0).Rows(i).Item("Email")
                            End If
                        End If
                    Next
                End If
            Else 'Notify same level approvers after a rejection has been released 
                dsCC = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), SeqNo, 0, False, False)
                'Carbon Copy pending approvers at same level as who rejected the record.
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And (RejectedTMID <> dsCC.Tables(0).Rows(i).Item("TeamMemberID") And (dsCC.Tables(0).Rows(i).Item("Status") = "Pending")) Then ''change to DefaultTMID   
                            If EmailCC = Nothing Then
                                EmailCC = dsCC.Tables(0).Rows(i).Item("Email")
                            Else
                                EmailCC = EmailCC & ";" & dsCC.Tables(0).Rows(i).Item("Email")
                            End If
                        End If
                    Next
                End If
            End If

            If IncludeOrigAprvlTM = True Then
                dsCC = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), 0, 0, False, False)
                'CC Orgiginal Approver in an event that their a corporate calendar default responsibility
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And (ddProjectLeader.SelectedValue <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) Then ''change to DefaultTMID   
                            If dsCC.Tables(0).Rows(i).Item("OrigEmail") <> dsCC.Tables(0).Rows(i).Item("Email") Then
                                If EmailCC = Nothing Then
                                    EmailCC = dsCC.Tables(0).Rows(i).Item("OrigEmail")
                                Else
                                    EmailCC = EmailCC & ";" & dsCC.Tables(0).Rows(i).Item("OrigEmail")
                                End If
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
        Dim TotalRequest As Decimal = lblTotalRequest.Text
        Dim CustReimb As Decimal = txtCustReimb.Text
        Dim TotalInv As Decimal = lblTotalInvestment1.Text


        MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>PROJECT OVERVIEW</strong></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Project No:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("pProjNo") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Project Title:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & lblProjectTitle.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Vehicle SOP Date:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtSOP.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Project Leader:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddProjectLeader.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Description:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td style='width: 700px;'>" & txtProjDateNotes.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Justification:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td style='width: 700px;'>" & txtJustification.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddUGNFacility.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Department or Cost Center:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddDepartment.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Estimated Completion Date:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtEstCmpltDt.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Estimated Start Spend Date:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtEstSpendDt.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Estimated End Spend Date:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtEstEndSpendDt.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' style='width: 250px;'>Budgeted:&nbsp;&nbsp;  </td>"
        MyMessage.Body &= "<td>" & IIf(ddBudgeted.SelectedValue = "True", "Yes", "No") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>INVESTMENTS</strong></td>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Total Request ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & Format(TotalRequest, "#,##0.00") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Expected Customer&nbsp;&nbsp;&nbsp;&nbsp;<br/>Reimbursement ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & Format(CustReimb, "#,##0.00") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Total Investment ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & Format(TotalInv, "#,##0.00") & "</td>"
        MyMessage.Body &= "</tr>"

        If ddCRProjNo.SelectedValue <> "" Or ddCRProjNo.SelectedValue <> Nothing Then
            MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>JUSTIFICATION</strong></td>"
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right'>Cost Reduction Ref #:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/CostReduction.aspx?pProjNo=" & ddCRProjNo.SelectedValue & "' target='_blank'>" & ddCRProjNo.SelectedItem.Text & "</a></td>"
            MyMessage.Body &= "</tr>"
        End If

        ''***************************************************
        ''Get list of Supporting Documentation
        ''***************************************************
        Dim dsAED As DataSet
        dsAED = EXPModule.GetDevelopmentExpDocument(ViewState("pProjNo"), 0)
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
                MyMessage.Body &= "<td height='25'><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/DevelopmentExpProjDocument.aspx?pProjNo=" & ViewState("pProjNo") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
                MyMessage.Body &= "</tr>"
            Next
            MyMessage.Body &= "</table>"
            MyMessage.Body &= "</tr>"
        End If
        MyMessage.Body &= "</table>"

        Return True

    End Function 'EOF EmailBody

    Public Sub SendNotifWhenEventChanges(ByVal EventDesc As String)
        ''********
        ''* This section will be used in the following methods:
        ''*     1) Email sent to all involved when an Development is CAPITALIZED
        ''*     2) Email sent to all involved when the Estimated Completion Date changes with the Project Status is not Open
        ''*     3) Email sent to all involved with an Development is VOID
        ''*     4) Email sent to Account with an Development is COMPLETED
        ''********188 371 510 569
        Dim DefaultDate As Date = Date.Today
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
        Dim DefaultUserName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value
        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
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
            EmailFrom = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
            EmailCC = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
        Else
            CurrentEmpEmail = "Database.Notifications@ugnauto.com"
            EmailFrom = "Database.Notifications@ugnauto.com"
        End If

        Dim ProjectStatus As String = Nothing
        If txtRoutingStatus.Text = "N" Or txtRoutingStatus.Text = "A" Or txtRoutingStatus.Text = "C" Then
            ProjectStatus = ddProjectStatus.SelectedValue
        Else
            ProjectStatus = ddProjectStatus2.SelectedValue
        End If
        lblErrors.Text = Nothing
        lblErrors.Visible = False

        Try
            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing Then
                ''Is this for a Group Notification or Individual
                Select Case EventDesc
                    Case "Void" 'Sent by Project Leader, notify all
                        GroupNotif = True
                    Case "Completed" 'Sent by Project Leader, notify accounting
                        GroupNotif = False
                End Select

                If ViewState("pProjNo") <> Nothing Then
                    ''*********************************
                    ''Send Notification
                    ''*********************************
                    If GroupNotif = True Then
                        ''*******************************
                        ''Notify Approvers
                        ''*******************************
                        ds1st = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), 0, 0, False, False)
                        ''Check that the recipient(s) is a valid Team Member
                        If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                                If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                    If (ds1st.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And (ddProjectLeader.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID")) And (Not IsDBNull(ds1st.Tables(0).Rows(i).Item("DateNotified"))) Then ''change to DefaultTMID   
                                        If EmailTO = Nothing Then
                                            EmailTO = ds1st.Tables(0).Rows(i).Item("Email")
                                        Else
                                            EmailTO = EmailTO & ";" & ds1st.Tables(0).Rows(i).Item("Email")
                                        End If
                                    End If
                                End If
                            Next
                        End If 'EOF Notify Approvers

                        ''********************************************************
                        ''Notify Project Lead/Requested By/Account Manager
                        ''********************************************************
                        ds1st = EXPModule.GetExpProjDevelopmentLead(ViewState("pProjNo"))
                        ''Check that the recipient(s) is a valid Team Member
                        If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                                If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                    If EmailTO = Nothing Then
                                        EmailTO = ds1st.Tables(0).Rows(i).Item("Email")
                                    Else
                                        EmailTO = EmailTO & ";" & ds1st.Tables(0).Rows(i).Item("Email")
                                    End If
                                End If
                            Next
                        End If 'EOF Notify Project Lead
                    Else
                        ''*******************************************
                        ''Notify Accounting
                        ''*******************************************
                        ds1st = commonFunctions.GetTeamMemberBySubscription(87)
                        ''Check that the recipient(s) is a valid Team Member
                        If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                                If (ds1st.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds1st.Tables(0).Rows(i).Item("WorkStatus") = 1) Or (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                    If EmailTO = Nothing Then
                                        EmailTO = ds1st.Tables(0).Rows(i).Item("Email")
                                    Else
                                        EmailTO = EmailTO & ";" & ds1st.Tables(0).Rows(i).Item("Email")
                                    End If
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

                    ''********************************
                    ''Carbon Copy Ops Manager
                    ''********************************
                    EmailCC = CarbonCopyList(MyMessage, 132, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)

                    ''********************************
                    ''Carbon Copy Accounting
                    ''********************************
                    EmailCC = CarbonCopyList(MyMessage, 87, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)

                    ''********************************
                    ''Carbon Copy Cc List
                    ''********************************
                    EmailCC = CarbonCopyList(MyMessage, 131, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)


                    'Test or Production Message display
                    If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                    Else
                        MyMessage.Subject = ""
                        MyMessage.Body = ""
                        'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
                    End If

                    MyMessage.Subject &= "Development Project: " & ViewState("pProjNo") & " - " & lblProjectTitle.Text & " - " & EventDesc

                    MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"
                    If EventDesc = "Estimated Completion Date Changed" Then
                        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>" & EventDesc.ToUpper & " by " & DefaultUserName & ".</strong></td>"
                    Else
                        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>This CapEx Development Project was '" & EventDesc.ToUpper & "' by " & DefaultUserName & ".</strong></td>"
                    End If

                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td class='p_text' align='right' style='width: 150px;'>Project No:&nbsp;&nbsp; </td>"
                    MyMessage.Body &= "<td> <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>" & ViewState("pProjNo") & "</a></td>"
                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td class='p_text' align='right'>Project Title:&nbsp;&nbsp; </td>"
                    MyMessage.Body &= "<td>" & lblProjectTitle.Text & "</td>"
                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td class='p_text' align='right'>Project Leader:&nbsp;&nbsp; </td>"
                    MyMessage.Body &= "<td>" & ddProjectLeader.SelectedItem.Text & "</td>"
                    MyMessage.Body &= "</tr>"

                    Select Case EventDesc
                        Case "Void" 'Sent by Project Leader, notify all
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Void Reason:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td style='width: 600px;'>" & txtVoidReason.Text & "</td>"
                            MyMessage.Body &= "</tr>"
                        Case "Completed" 'Sent by Project Leader, notify accounting
                            ''no additional info needed all in the subject line
                    End Select
                    MyMessage.Body &= "</table>"

                    If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                        Dim emailList1 As String() = commonFunctions.CleanEmailList(EmailTO).Split(";")
                        Dim toEmail As String = Nothing
                        For i = 0 To UBound(emailList1)
                            If emailList1(i) <> ";" And emailList1(i).Trim <> "" Then
                                toEmail += emailList1(i) & ";"
                            End If
                        Next i
                        EmailTO = toEmail

                        Dim emailList As String() = commonFunctions.CleanEmailList(EmailCC).Split(";")
                        Dim ccEmail As String = Nothing
                        For i = 0 To UBound(emailList)
                            If emailList(i) <> ";" And emailList(i).Trim <> "" And emailList(i) <> EmailTO Then
                                ccEmail += emailList(i) & ";"
                            End If
                        Next i
                        EmailCC = ccEmail

                        MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                        MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                        EmailFrom = "Database.Notifications@ugnauto.com"
                        EmailTO = CurrentEmpEmail '"lynette.rey@ugnauto.com"
                        EmailCC = "lynette.rey@ugnauto.com"
                    End If

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (D)", ViewState("pProjNo"))
                        lblErrors.Text = "Notification sent successfully."
                    Catch ex As Exception
                        lblErrors.Text &= "Email Notification is queued for the next automated release."

                        UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                        'get current event name
                        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                        'log and email error
                        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                    End Try
                    lblErrors.Visible = True

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
#End Region

#Region "Cost Reduction"
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

    End Sub

    Protected Sub btnCRProjNoReq_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCRProjNoReq.Click
        Try
            ''********
            ''* This function is used to submit email to the Approvers after the Project Sponsor is done.
            ''********
            Dim DefaultDate As Date = Date.Today
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
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

            Dim CurrentEmpEmail As String = Nothing
            If HttpContext.Current.Request.Cookies("UGNDB_User_Email") IsNot Nothing Then
                CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailFrom = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailCC = CurrentEmpEmail
            Else
                CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                EmailFrom = "Database.Notifications@ugnauto.com"
            End If

            Dim ProjectStatus As String = Nothing
            If txtRoutingStatus.Text = "N" Or txtRoutingStatus.Text = "A" Or txtRoutingStatus.Text = "C" Then
                ProjectStatus = ddProjectStatus.SelectedValue
            Else
                ProjectStatus = ddProjectStatus2.SelectedValue
            End If

            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pProjNo") <> Nothing Then
                    ''***************
                    ''Verify that atleast one Supporting Document is entered
                    ''***************
                    Dim ReqDocFound As Boolean = VerifySupportingDocument()
                    If ReqDocFound = False Then
                        Exit Sub
                    End If 'ReqDocFound = false

                    ''***************
                    ''Set flag to send notification for Cost Reduction entry
                    ''***************
                    Dim SendCRNotif As Boolean = False
                    If ddCRProjNo.SelectedValue = Nothing Then
                        SendCRNotif = True
                    End If

                    ''**********************
                    ''* Save data prior to submission before approvals
                    ''**********************
                    If SendCRNotif = True And ReqDocFound = False Then
                        ''*********************************
                        ''Send Notification based on facility to Cost Reduction Team Member User based on Facility
                        ''*********************************
                        Dim ds As DataSet = New DataSet
                        ''*********************************
                        ''Verify that Cost Reduction Ref# is valid before proceeding.
                        ''If it does not exist, default notification to a member of CR module
                        ''*********************************
                        ds = CRModule.GetCostReduction(ddCRProjNo.SelectedValue, 0, ddUGNFacility.SelectedValue, 0, 0, "", 0, False, False, "")
                        If commonFunctions.CheckDataSet(ds) = False Then
                            SendCRNotif = False 'skip to approval process
                        Else
                            ''Locate CR Team Leader according to UGN Facility
                            ds1st = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(75, ddUGNFacility.SelectedValue)
                            If commonFunctions.CheckDataSet(ds1st) = True Then
                                For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                                    If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                        If (ds1st.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And (ddProjectLeader.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TMID")) Then ''change to DefaultTMID   
                                            If EmailTO = Nothing Then
                                                EmailTO = ds1st.Tables(0).Rows(i).Item("Email")
                                            Else
                                                EmailTO = EmailTO & ";" & ds1st.Tables(0).Rows(i).Item("Email")
                                            End If
                                            If EmpName = Nothing Then
                                                EmpName = ds1st.Tables(0).Rows(i).Item("TMName") & ", "
                                            Else
                                                EmpName = EmpName & ds1st.Tables(0).Rows(i).Item("TMName") & ", "
                                            End If
                                        End If 'EOF If (ds1st.Tables(0).Rows(i).Item("Email") <> Nothing) 
                                    End If 'EOF If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                Next 'EOF For i = 0 To
                            End If 'EOF commonFunctions.CheckDataset(ds1st) = True 
                        End If 'EOF If commonFunctions.CheckDataset(ds) = True 
                    End If 'If SendCRNotif = True Then

                    ''********************************************************
                    ''Send Notification only if there is a valid Email Address
                    ''********************************************************
                    If EmailTO <> Nothing Then
                        ''************************
                        ''* Update Record
                        ''************************
                        UpdateRecord(ProjectStatus, txtRoutingStatus.Text)

                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                        ''Test or Production Message display
                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br><br>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                            'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br><br>"
                        End If
                        If SendCRNotif = True Then
                            MyMessage.Subject &= " Cost Reduction Project Request for "
                        End If

                        MyMessage.Subject &= "Development Project: " & ViewState("pProjNo") & " - " & lblProjectTitle.Text

                        MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & lblProjectTitle.Text & "' requires a Cost Reduction Project entry. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/CostReduction.aspx?pProjNo=0&pCPNo=" & ViewState("pProjNo") & "'>Click here</a> to create a new record. Below is a summary of the Development Project Request for your review. For additional information about the CapEx Development Project <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>click here</a>.</p>"

                        ''*****************
                        ''Build Email body
                        ''*****************
                        EmailBody(MyMessage)

                        ''*****************
                        ''History Tracking
                        ''*****************
                        EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), lblProjectTitle.Text, DefaultTMID, "Cost Reduction Project request sent to " & EmpName & "..", "", "", "", "")

                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Dim emailList1 As String() = commonFunctions.CleanEmailList(EmailCC).Split(";")
                        Dim ccEmail As String = Nothing
                        For i = 0 To UBound(emailList1)
                            If emailList1(i) <> ";" And emailList1(i).Trim <> "" And emailList1(i) <> EmailTO Then
                                ccEmail += emailList1(i) & ";"
                            End If
                        Next i
                        EmailCC = ccEmail

                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                            MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                            EmailTO = CurrentEmpEmail '"lynette.rey@ugnauto.com"
                            EmailCC = "lynette.rey@ugnauto.com"
                        End If

                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (D)", ViewState("pProjNo"))
                            lblErrors.Text = "Cost Reduction Project Request submitted to " & EmpName & " successfully."
                        Catch ex As Exception
                            lblErrors.Text &= "Cost Reduction Project Request submitted to " & EmpName & " is queued for the next automated release."

                            UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                            'get current event name
                            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                            'log and email error
                            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                        End Try
                        lblErrors.Visible = True

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pProjNo"))

                        ''*************************************************
                        '' "Form Level Security using Roles &/or Subscriptions"
                        ''*************************************************
                        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                        mvTabs.ActiveViewIndex = Int32.Parse(2)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(2).Selected = True
                        ddCRProjNo.Focus()
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
    End Sub 'EOF btnCRProjNoReq_Click

#End Region 'EOF Cost Reduction

#Region "Supporting Document"
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim DefaultDate As Date = Date.Today
            lblMessageView4.Text = Nothing
            lblMessageView4.Visible = False
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            If ViewState("pProjNo") <> "" Then
                If uploadFile.HasFile Then
                    If uploadFile.PostedFile.ContentLength <= 3500000 Then
                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFile.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFile.PostedFile.FileName)

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
                                EXPModule.InsertExpProjDevelopmentDocuments(ViewState("pProjNo"), ViewState("iTeamMemberID"), txtFileDesc.Text, SupportingDocBinaryFile, uploadFile.FileName, SupportingDocEncodeType, SupportingDocFileSize, 0, 0, "", "")
                            End If

                            gvSupportingDocument.DataBind()
                            revUploadFile.Enabled = False
                            txtFileDesc.Text = Nothing
                        End If
                    Else
                        lblMessageView4.Text = "File exceeds size limit.  Please select a file less than 3MB (3000KB)."
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
                Dim db As ImageButton = CType(e.Row.Cells(3).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As ExpProjDevelopment.ExpProj_Development_DocumentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProjDevelopment.ExpProj_Development_DocumentsRow)

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
            Response.Redirect("DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pSD=1" & Aprv, False)
        End If
    End Sub 'EOF gvSupportingDocument_RowCommand

    Protected Sub ddUGNFacility_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUGNFacility.SelectedIndexChanged
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Department or Cost Center control for selection criteria for search
            ds = commonFunctions.GetDepartmentGLNo(ddUGNFacility.SelectedValue)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddDepartment.DataSource = ds
                ddDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName.ToString()
                ddDepartment.DataValueField = ds.Tables(0).Columns("GLNo").ColumnName.ToString()
                ddDepartment.DataBind()
                ddDepartment.Items.Insert(0, "")
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF ddUGNFacility_SelectedIndexChanged

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
#End Region 'EOF Supporting Document

#Region "Communication Board"
    Public Function GoToCommunicationBoard(ByVal ProjectNo As String, ByVal RSSID As String, ByVal ApprovalLevel As Integer, ByVal TeamMemberID As Integer) As String
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If
        Return "DevelopmentExpProj.aspx?pProjNo=" & ProjectNo & "&pAL=" & ApprovalLevel & "&pTMID=" & TeamMemberID & "&pRID=" & RSSID & "&pRC=1" & Aprv
    End Function 'EOF GoToCommunicationBoard

    Protected Sub btnSave2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave2.Click
        Try
            ''************************************
            ''Send response back to requestor
            ''************************************
            Dim DefaultDate As Date = Date.Today
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

            Dim SeqNo As Integer = IIf(HttpContext.Current.Request.QueryString("pAL") = "", 0, HttpContext.Current.Request.QueryString("pAL"))
            Dim TMID As Integer = IIf(HttpContext.Current.Request.QueryString("pTMID") = "", 0, HttpContext.Current.Request.QueryString("pTMID"))
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim NextSeqNo As Integer = 0
            Dim NextLvl As Integer = 0

            Select Case SeqNo
                Case 1
                    NextSeqNo = 2
                    NextLvl = 129
                Case 2
                    NextSeqNo = 3
                    NextLvl = 130
                Case 3
                    NextSeqNo = 0
                    NextLvl = 130
            End Select

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
                EmailFrom = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailCC = CurrentEmpEmail
            Else
                CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                EmailFrom = "Database.Notifications@ugnauto.com"
            End If

            lblErrors.Text = Nothing
            lblErrors.Visible = False

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pProjNo") <> Nothing Then
                    ''***************************************************************
                    ''Send Reply back to requestor
                    ''***************************************************************
                    ds = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), 0, TMID, False, False) '
                    ''Check that the recipient(s) is a valid Team Member
                    If ds.Tables.Count > 0 And (ds.Tables.Item(0).Rows.Count > 0) Then
                        For i = 0 To ds.Tables.Item(0).Rows.Count - 1
                            If (ds.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                If (ds.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                    If EmailTO = Nothing Then
                                        EmailTO = ds.Tables(0).Rows(i).Item("Email")
                                    Else
                                        EmailTO = EmailTO & ";" & ds.Tables(0).Rows(i).Item("Email")
                                    End If
                                    If EmpName = Nothing Then
                                        EmpName = ds.Tables(0).Rows(i).Item("EmailTMName") & ", "
                                    Else
                                        EmpName = EmpName & ds.Tables(0).Rows(i).Item("EmailTMName") & ", "
                                    End If
                                    'SeqNo = ds.Tables(0).Rows(i).Item("SeqNo")
                                End If
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
                        ''Carbon Copy Project Leader/Requested By/Account Manager
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                        ''***************************************************************
                        ''Carbon Copy Same Level Approvers
                        ''***************************************************************
                        EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), "", 0, 0, EmailCC, DefaultTMID)

                        ''********************************
                        ''Carbon Copy Accounting
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 87, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)

                        ''Test or Production Message display
                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                            ' MyMessage.Body = "THIS IS A TEST. DATA IS NOT VALID FOR USE<br/>"
                        End If

                        MyMessage.Subject &= "Development Project: " & ViewState("pProjNo") & " - " & lblProjectTitle.Text & " - MESSAGE RECIEVED"
                        MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
                        MyMessage.Body &= " <tr>"
                        MyMessage.Body &= "     <td valign='top' width='20%'>"
                        MyMessage.Body &= "         <img src='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/images/messanger60.jpg'/>"
                        MyMessage.Body &= "     </td>"
                        MyMessage.Body &= "     <td valign='top'>"
                        MyMessage.Body &= "         <b>Attention:</b> " & EmpName
                        MyMessage.Body &= "         <p><b>" & DefaultUserFullName & "</b> replied to your message regarding "
                        MyMessage.Body &= "         <font color='red'>" & ViewState("pProjNo") & " - " & lblProjectTitle.Text & "</font>."
                        MyMessage.Body &= "         <br/><br/><i>Question:&nbsp;&nbsp;</i><b>" & txtQC.Text & "</b>"
                        MyMessage.Body &= "         <br/><br/><i>Response:&nbsp;&nbsp;</i><b>" & txtReply.Text & "</b><br/><br/>"

                        MyMessage.Body &= "         </p>"
                        MyMessage.Body &= "         <p><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/crExpProjDevelopmentApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to return to the approval page."
                        MyMessage.Body &= "     </td>"
                        MyMessage.Body &= " </tr>"
                        MyMessage.Body &= "</table>"
                        MyMessage.Body &= "<br/><br/>"

                        ''*****************
                        ''History Tracking
                        ''*****************
                        EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), lblProjectTitle.Text, DefaultTMID, "Message Sent", "", "", "", "")

                        ''**********************************
                        ''Save Reponse to child table
                        ''**********************************
                        EXPModule.InsertExpProjDevelopmentRSSReply(ViewState("pProjNo"), ViewState("pRID"), lblProjectTitle.Text, DefaultTMID, txtReply.Text)


                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Dim emailList1 As String() = commonFunctions.CleanEmailList(EmailCC).Split(";")
                        Dim ccEmail As String = Nothing
                        For i = 0 To UBound(emailList1)
                            If emailList1(i) <> ";" And emailList1(i).Trim <> "" And emailList1(i) <> EmailTO Then
                                ccEmail += emailList1(i) & ";"
                            End If
                        Next i
                        EmailCC = ccEmail

                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                            MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                            EmailTO = CurrentEmpEmail '"lynette.rey@ugnauto.com"
                            EmailCC = "lynette.rey@ugnauto.com"
                        End If

                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (D)", ViewState("pProjNo"))
                            lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                        Catch ex As Exception
                            lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                            UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                            'get current event name
                            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                            'log and email error
                            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                        End Try
                        lblErrors.Visible = True


                        ''************************
                        ''Redirect User
                        ''************************
                        Dim Aprv As String = Nothing
                        If ViewState("pAprv") = 1 Then
                            Aprv = "&pAprv=1"
                        End If
                        Response.Redirect("DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pRC=1" & Aprv, False)
                    Else 'EmailTO = ''
                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pProjNo"))

                        lblErrors.Text = "Unable to locate a valid email address to send message to. Please contact the Application Department for assistance."
                        lblErrors.Visible = True
                    End If 'EOF EmailTO <> ''
                End If
            End If
            'End If
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
            Dim drRSSID As ExpProjDevelopment.ExpProj_Development_RSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, ExpProjDevelopment.ExpProj_Development_RSSRow)

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
#End Region 'EOF Communication Board

End Class