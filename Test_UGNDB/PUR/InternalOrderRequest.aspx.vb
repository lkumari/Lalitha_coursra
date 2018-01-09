' ************************************************************************************************
' Name:	InternalOrderRequest.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 08/23/2010    LRey			Created .Net application
' 05/16/2012    LRey            Modified vb file to adhere to new standards with .NET Mail and 
'                               create various subroutines where reusable code can be simplified.
' 07/19/2012	LRey	        Changed the data type to PONo from int to varchar to allow
'								Buyer's to type in PCARD when it doesn't required a PONo
' 07/20/2012    LRey            Added functionality for IS Infrastructure to issue IOR's for other Requisitioner's
'                               Subscription ID 141 created
' 05/10/2013    LREy            Added Customer Owned Tooling Revision Level and gridview access for update
' 06/27/20123   LRey            Modified to include workflow for Buyer approvals only
' 02/25/2014    LRey            Oracle iProcurement replaces this E-IOR module. Disabled the btnAdd feature. 
'                               Allow only the update/edit to complete the existing E-IOR records.
' ************************************************************************************************
Partial Class IOR_InternalOrderRequest
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            Dim a As String = commonFunctions.UserInfo()
            ViewState("TMLoc") = HttpContext.Current.Session("UserFacility")

            ''Used to define the primary record
            If HttpContext.Current.Request.QueryString("pIORNo") <> "" Then
                ViewState("pIORNo") = HttpContext.Current.Request.QueryString("pIORNo")
            Else
                ViewState("pIORNo") = ""
            End If

            ''Used to define the Appropriation Code
            If HttpContext.Current.Request.QueryString("pProjNo") <> "" Then
                ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
            Else
                ViewState("pProjNo") = ""
            End If

            ''Used for IOR Extenstion binddata and update
            If HttpContext.Current.Request.QueryString("pEID") <> "" Then
                ViewState("pEID") = HttpContext.Current.Request.QueryString("pEID")
            Else
                ViewState("pEID") = 0
            End If

            ''Used to take user back to Extenstion Tab after save.
            If HttpContext.Current.Request.QueryString("pEV") <> "" Then
                ViewState("pEV") = HttpContext.Current.Request.QueryString("pEV")
            Else
                ViewState("pEV") = 0
            End If

            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pAprv") <> "" Then
                ViewState("pAprv") = HttpContext.Current.Request.QueryString("pAprv")
            Else
                ViewState("pAprv") = 0
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

            ''Used to display Vendor Info
            If HttpContext.Current.Request.QueryString("pVTp") <> "" Then
                ViewState("pVTp") = HttpContext.Current.Request.QueryString("pVTp")
            Else
                ViewState("pVTp") = Nothing
            End If
            If HttpContext.Current.Request.QueryString("pVNo") <> "" Then
                ViewState("pVNo") = HttpContext.Current.Request.QueryString("pVNo")
            Else
                ViewState("pVNo") = 0
            End If
            If HttpContext.Current.Request.QueryString("pNF") <> "" Then
                ViewState("pNF") = HttpContext.Current.Request.QueryString("pNF")
            Else
                ViewState("pNF") = 0
            End If

            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            If ViewState("pIORNo") = Nothing Then
                m.ContentLabel = "New Internal Order Request"
            Else
                m.ContentLabel = "Internal Order Request"
            End If

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pIORNo") = Nothing Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='InternalOrderRequestList.aspx'><b>Internal Order Request Search</b></a> > New Internal Order Request"
                Else
                    If ViewState("pAprv") = 0 Then
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='InternalOrderRequestList.aspx'><b>Internal Order Request Search</b></a> > Internal Order Request"
                    Else 'Go Back To approval Screen
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Supplier</b> > <a href='InternalOrderRequestList.aspx'><b>Internal Order Request Search</b></a> > <a href='crInternalOrderRequestApproval.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pAprv=1'><b>Approval</b></a> > Internal Order Request"
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
            ctl = m.FindControl("PURExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If


            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ''****************************************
                ''Redirect user to the right tab location
                ''****************************************
                If ViewState("pIORNo") <> "" Then
                    'CheckRights()
                    BindCriteria()
                    BindData(ViewState("pIORNo"))
                Else
                    BindCriteria()
                    txtIORDescription.Focus()
                    txtPONo.Enabled = False
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

            ''*************************************************
            '' Initialize maxlength
            ''*************************************************
            txtIORDescription.Attributes.Add("onkeypress", "return tbLimit();")
            txtIORDescription.Attributes.Add("onkeyup", "return tbCount(" + lblIORDescription.ClientID + ");")
            txtIORDescription.Attributes.Add("maxLength", "50")

            txtVendorWebsite.Attributes.Add("onkeypress", "return tbLimit();")
            txtVendorWebsite.Attributes.Add("onkeyup", "return tbCount(" + lblVendorWebSite.ClientID + ");")
            txtVendorWebsite.Attributes.Add("maxLength", "60")

            txtVendorEmail.Attributes.Add("onkeypress", "return tbLimit();")
            txtVendorEmail.Attributes.Add("onkeyup", "return tbCount(" + lblVendorEmail.ClientID + ");")
            txtVendorEmail.Attributes.Add("maxLength", "100")

            txtComments.Attributes.Add("onkeypress", "return tbLimit();")
            txtComments.Attributes.Add("onkeyup", "return tbCount(" + lblComments.ClientID + ");")
            txtComments.Attributes.Add("maxLength", "300")

            txtVoidReason.Attributes.Add("onkeypress", "return tbLimit();")
            txtVoidReason.Attributes.Add("onkeyup", "return tbCount(" + lblVoidRsn.ClientID + ");")
            txtVoidReason.Attributes.Add("maxLength", "300")

            txtReply.Attributes.Add("onkeypress", "return tbLimit();")
            txtReply.Attributes.Add("onkeyup", "return tbCount(" + lblReply.ClientID + ");")
            txtReply.Attributes.Add("maxLength", "300")

            txtFileDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtFileDesc.Attributes.Add("onkeyup", "return tbCount(" + lblFileDesc.ClientID + ");")
            txtFileDesc.Attributes.Add("maxLength", "200")

            txtNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNotes.ClientID + ");")
            txtNotes.Attributes.Add("maxLength", "300")

            txtReSubmit.Attributes.Add("onkeypress", "return tbLimit();")
            txtReSubmit.Attributes.Add("onkeyup", "return tbCount(" + lblReSubmitCnt.ClientID + ");")
            txtReSubmit.Attributes.Add("maxLength", "300")

            Dim strPreviewClientScript As String = "javascript:void(window.open('crViewInternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pBuyer=" & ViewState("iBuyerID") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
            btnPreview.Attributes.Add("onclick", strPreviewClientScript)

            Dim strBtnPreviewClientScript As String = "javascript:void(window.open('IORsByAppropriation.aspx?pProjNo=" & ViewState("pProjNo") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
            iBtnPreview.Attributes.Add("onclick", strBtnPreviewClientScript)


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
    Protected Sub DisableControls()
        ViewState("Admin") = False
        ViewState("ObjectRole") = False

        mnuTabs.Items(1).Enabled = False
        mnuTabs.Items(2).Enabled = False
        mnuTabs.Items(3).Enabled = False
        mnuTabs.Items(4).Enabled = False

        ddIORStatus.Enabled = False
        ddIORStatus2.Enabled = False
        hplkAppropriation.Visible = False

        'btnAdd.Enabled = False
        btnSave1.Enabled = False
        btnReset1.Enabled = False 'Detail Tab
        btnExtension.Enabled = False
        btnReset2.Enabled = False 'Extension Tab
        btnReset3.Enabled = True 'Supporting Documents Tab
        btnRSS.Enabled = False
        btnReset4.Enabled = False 'Communication Board - RSS Tab
        btnBuildApprovalList.Enabled = False 'used for troubleshooting by developer
        btnBuildApprovalList.Visible = False 'used for troubleshooting by developer
        btnFwdApproval.Enabled = False
        btnDelete.Enabled = False
        btnPreview.Enabled = False

        txtVoidReason.Enabled = False
        txtVoidReason.Visible = False
        lblVoidReason.Visible = False
        rfvVoidReason.Enabled = False
        lblReqVoidReason.Visible = False

        lblReqReSubmit.Visible = False
        lblReSubmit.Visible = False
        txtReSubmit.Visible = False
        rfvReSubmit.Enabled = False
        vsReSubmit.Enabled = False

        lblReqPONo.Visible = False
        txtPONo.Visible = False
        txtPONo.ReadOnly = True
        rfvPONo.Enabled = False

        gvApprovers.ShowFooter = False
        gvApprovers.Columns(7).Visible = False

        btnUpload.Enabled = True
        uploadFile.Enabled = True
        gvSupportingDocument.Columns(3).Visible = True

        If txtAppropriation.Text <> Nothing Then
            ddCurrency.Enabled = False

            If txtAppropriation.Text.Substring(0, 1) <> "T" Then
                COTPanel.Visible = False
                COTContentPanel.Visible = False
                gvExpProjToolingCustomer.Enabled = False
            Else
                COTPanel.Visible = True
                COTContentPanel.Visible = True
                gvExpProjToolingCustomer.Enabled = True
            End If

        End If

        ddCurrency.Visible = False
        gvExpense.Columns(11).Visible = False

    End Sub 'EOF DisableControls()

    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        Try
            ''*******
            '' Disable controls by default
            ''*******
            DisableControls()

            ''** Project Status
            Dim ProjectStatus As String = Nothing
            Select Case txtRoutingStatus.Text
                Case "N"
                    ProjectStatus = ddIORStatus.SelectedValue
                    ddIORStatus.Visible = True
                    ddIORStatus2.Visible = False
                Case "A"
                    ProjectStatus = ddIORStatus.SelectedValue
                    ddIORStatus.Visible = True
                    ddIORStatus2.Visible = False
                    ddIORStatus.Enabled = True
                    ddIORStatus2.Enabled = False
                Case "C"
                    ProjectStatus = ddIORStatus.SelectedValue
                    ddIORStatus.Visible = True
                    ddIORStatus2.Visible = False
                    ddIORStatus.Enabled = True
                    ddIORStatus2.Enabled = False
                Case "T"
                    ProjectStatus = ddIORStatus2.SelectedValue
                    ddIORStatus.Visible = False
                    ddIORStatus2.Visible = True
                    ddIORStatus.Enabled = False
                    ddIORStatus2.Enabled = True
                Case "R"
                    ProjectStatus = ddIORStatus2.SelectedValue
                    ddIORStatus.Visible = False
                    ddIORStatus2.Visible = True
                    ddIORStatus.Enabled = False
                    ddIORStatus2.Enabled = True
                Case "V"
                    ProjectStatus = ddIORStatus.SelectedValue
                    ddIORStatus.Visible = True
                    ddIORStatus2.Visible = False
                Case Else
                    ddIORStatus.Visible = True
                    ddIORStatus2.Visible = False
            End Select
            ViewState("ProjectStatus") = ProjectStatus

            If ViewState("pIORNo") <> Nothing Or ViewState("pIORNo") <> "" Then
                If txtAppropriation.Text = Nothing And txtProjectTitle.Text = Nothing Then
                    ''*************************************************************
                    ''* Show/hide fields according to Appropriation entry for an 
                    ''* easier distinction of what type of IOR used after it has 
                    ''* been submitted.
                    ''*************************************************************
                    lblAppropriation.Visible = False
                    txtAppropriation.Visible = False
                    lblTotalCapEx.Visible = False
                    txtTotalCapEx.Visible = False
                    lblTotalSpent.Visible = False
                    txtTotalSpent.Visible = False
                    lblRemainingCapEx.Visible = False
                    txtRemainingCapEx.Visible = False
                End If
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iTMEmail As String = ""
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 109 'Internal Order Request Form ID
            Dim iRoleID As Integer = 0
            Dim i As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Pam.Delor", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iTMEmail = dsTeamMember.Tables(0).Rows(0).Item("Email")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")

                    ViewState("iTeamMemberID") = iTeamMemberID
                    ViewState("DefaultUserEmail") = iTMEmail

                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member

                        ''Locate the Buyer to grant access to Purchase Order # field
                        Dim dsBuyer As DataSet = New DataSet
                        dsBuyer = commonFunctions.GetTeamMemberBySubscription(99)
                        Dim iBuyerID As Integer = 0
                        Dim b As Integer = 0
                        ViewState("iBuyerID") = 0
                        If (dsBuyer.Tables.Item(0).Rows.Count > 0) Then
                            For b = 0 To dsBuyer.Tables(0).Rows.Count - 1
                                If dsBuyer.Tables(0).Rows(b).Item("TMID") = iTeamMemberID Then
                                    iBuyerID = dsBuyer.Tables(0).Rows(b).Item("TMID")
                                    ViewState("iBuyerID") = iBuyerID
                                End If
                            Next
                        End If

                        ''Locate the IS Infrastructure
                        Dim dsIS As DataSet = New DataSet
                        dsIS = commonFunctions.GetTeamMemberBySubscription(141)
                        Dim iISINF As Integer = 0
                        Dim t As Integer = 0
                        ViewState("iISINF") = 0
                        If (dsIS.Tables.Item(0).Rows.Count > 0) Then
                            For t = 0 To dsIS.Tables(0).Rows.Count - 1
                                If dsIS.Tables(0).Rows(t).Item("TMID") = iTeamMemberID Then
                                    iISINF = dsIS.Tables(0).Rows(t).Item("TMID")
                                    ViewState("iISINF") = iISINF
                                End If
                            Next
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
                                            ' ''btnAdd.Enabled = True
                                            ViewState("Admin") = True

                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pIORNo") = Nothing Or ViewState("pIORNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                ddIORStatus.Enabled = False
                                                txtVendorName.Focus()
                                                If txtProjectTitle.Text <> Nothing And txtAppropriation.Text <> Nothing And txtProjectStatus.Text = "Approved" Then
                                                    btnSave1.Enabled = True
                                                ElseIf txtProjectTitle.Text = Nothing And txtAppropriation.Text = Nothing And txtProjectStatus.Text <> "Approved" Then
                                                    btnSave1.Enabled = True
                                                End If
                                                btnReset1.Enabled = True
                                                btnPreview.Enabled = False
                                            Else
                                                If ViewState("pVNo") = 0 And ViewState("pNF") = 0 Then
                                                    mnuTabs.Items(1).Enabled = True
                                                    mnuTabs.Items(2).Enabled = True
                                                    mnuTabs.Items(3).Enabled = True
                                                    mnuTabs.Items(4).Enabled = True
                                                End If
                                                Select Case ProjectStatus
                                                    Case "Open"
                                                        ddRequestedBy.Enabled = True
                                                        If (txtRoutingStatus.Text = "N") Then
                                                            btnDelete.Enabled = True
                                                            If txtRemainingCapEx.Text >= 0 Then
                                                                If (txtProjectStatus.Text = "Approved" And txtAppropriation.Text <> Nothing) Or txtAppropriation.Text = Nothing Then
                                                                    btnFwdApproval.Enabled = True
                                                                End If
                                                            End If
                                                            ddIORStatus.Enabled = False
                                                        End If
                                                        If (txtProjectStatus.Text = "Approved" And txtAppropriation.Text <> Nothing) Or txtAppropriation.Text = Nothing Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True 'Detail Tab
                                                            btnExtension.Enabled = True
                                                            btnReset2.Enabled = True 'Extension Tab
                                                            btnRSS.Enabled = True
                                                            btnReset4.Enabled = True 'Communication Board - RSS Tab
                                                            gvExpense.Columns(11).Visible = True
                                                        End If
                                                        If ddVendor.SelectedValue = Nothing Then
                                                            mnuTabs.Items(1).Enabled = False
                                                            mnuTabs.Items(2).Enabled = False
                                                            mnuTabs.Items(3).Enabled = False
                                                            mnuTabs.Items(4).Enabled = False
                                                        End If
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "R") Then
                                                            lblReqReSubmit.Visible = True
                                                            lblReSubmit.Visible = True
                                                            txtReSubmit.Visible = True
                                                            rfvReSubmit.Enabled = True
                                                            vsReSubmit.Enabled = True
                                                            btnFwdApproval.Enabled = True
                                                            'Else
                                                            '    ddIORStatus2.Visible = True
                                                        End If
                                                        gvApprovers.Columns(7).Visible = True
                                                        gvExpense.Columns(11).Visible = True
                                                    Case "Void"
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidReason.Visible = True
                                                        rfvVoidReason.Enabled = True
                                                        lblReqVoidReason.Visible = True
                                                    Case "Completed"
                                                        'NA
                                                    Case "Closed"
                                                        'NA
                                                    Case "Approved"
                                                        'NA
                                                End Select
                                                txtPONo.Visible = True
                                                txtPONo.ReadOnly = False

                                                btnSave1.Enabled = True
                                                btnReset1.Enabled = True
                                                btnDelete.Enabled = True
                                                btnPreview.Enabled = True
                                                btnExtension.Enabled = True
                                                btnReset2.Enabled = True
                                                btnRSS.Enabled = True
                                                btnReset4.Enabled = True
                                                gvApprovers.Columns(7).Visible = True
                                                gvExpense.Columns(11).Visible = True
                                                btnBuildApprovalList.Enabled = True
                                                btnBuildApprovalList.Visible = True
                                            End If
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Sales/Program Mgrs, Tooling/Purchasing Leads
                                            ViewState("ObjectRole") = True
                                            ' ''btnAdd.Enabled = True
                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pIORNo") = Nothing Or ViewState("pIORNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                txtVendorName.Focus()
                                                btnReset1.Enabled = True
                                                btnPreview.Enabled = False
                                                ddIORStatus.Enabled = False
                                                If txtRemainingCapEx.Text = "0.00" And txtAppropriation.Text <> Nothing And txtTotalCapEx.Text <> "0.00" Then
                                                    btnSave1.Enabled = False
                                                    lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION HAS NO REMAINING BALANCE."
                                                    lblErrors.Visible = "True"
                                                    btnSave1.Enabled = False
                                                Else
                                                    If txtProjectTitle.Text <> Nothing And txtAppropriation.Text <> Nothing And txtProjectStatus.Text = "Approved" Then
                                                        btnSave1.Enabled = True
                                                    ElseIf txtProjectTitle.Text = Nothing And txtAppropriation.Text = Nothing And txtProjectStatus.Text <> "Approved" Then
                                                        btnSave1.Enabled = True
                                                    End If
                                                End If
                                            Else
                                                ViewState("Admin") = True
                                                If ViewState("pVNo") = 0 And ViewState("pNF") = 0 Then
                                                    mnuTabs.Items(1).Enabled = True
                                                    mnuTabs.Items(2).Enabled = True
                                                    mnuTabs.Items(3).Enabled = True
                                                    mnuTabs.Items(4).Enabled = True
                                                End If
                                                Select Case ProjectStatus
                                                    Case "Open"
                                                        ddRequestedBy.Enabled = True
                                                        If (txtRoutingStatus.Text = "N") Then
                                                            btnDelete.Enabled = True
                                                            If txtRemainingCapEx.Text >= 0 Then
                                                                If (txtProjectStatus.Text = "Approved" And txtAppropriation.Text <> Nothing) Or txtAppropriation.Text = Nothing Then
                                                                    btnFwdApproval.Enabled = True
                                                                End If
                                                            End If
                                                            ddIORStatus.Enabled = False
                                                        End If
                                                        If (txtProjectStatus.Text = "Approved" And txtAppropriation.Text <> Nothing) Or txtAppropriation.Text = Nothing Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True 'Detail Tab
                                                            btnExtension.Enabled = True
                                                            btnReset2.Enabled = True 'Extension Tab
                                                            btnRSS.Enabled = True
                                                            btnReset4.Enabled = True 'Communication Board - RSS Tab
                                                            gvExpense.Columns(11).Visible = True
                                                        End If
                                                        If ddVendor.SelectedValue = Nothing Then
                                                            mnuTabs.Items(1).Enabled = False
                                                            mnuTabs.Items(2).Enabled = False
                                                            mnuTabs.Items(3).Enabled = False
                                                            mnuTabs.Items(4).Enabled = False
                                                        End If
                                                    Case "In Process"
                                                        ddRequestedBy.Enabled = False
                                                        btnRSS.Enabled = True
                                                        btnReset4.Enabled = True 'Communication Board - RSS Tab 
                                                        If (txtRoutingStatus.Text = "R") Then
                                                            lblReqReSubmit.Visible = True
                                                            lblReSubmit.Visible = True
                                                            txtReSubmit.Visible = True
                                                            rfvReSubmit.Enabled = True
                                                            vsReSubmit.Enabled = True

                                                            If txtRemainingCapEx.Text >= 0 Then
                                                                btnFwdApproval.Enabled = True
                                                            End If
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True 'Detail Tab
                                                            btnExtension.Enabled = True
                                                            btnReset2.Enabled = True 'Extension Tab
                                                            gvExpense.Columns(11).Visible = True
                                                        Else
                                                            If iBuyerID <> 0 Then
                                                                lblReqPONo.Visible = True
                                                                txtPONo.Visible = True
                                                                txtPONo.ReadOnly = False
                                                                rfvPONo.Enabled = True
                                                            End If
                                                            ExtExtender.Collapsed = True
                                                            'ddIORStatus2.Visible = True
                                                        End If
                                                    Case "Approved"
                                                        ddRequestedBy.Enabled = False
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        ExtExtender.Collapsed = True
                                                        ddIORStatus.Enabled = True
                                                        If iBuyerID <> 0 Then
                                                            lblReqPONo.Visible = True
                                                            txtPONo.Visible = True
                                                        End If
                                                    Case "Completed"
                                                        ddRequestedBy.Enabled = False
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        SDExtender.Collapsed = True
                                                        ExtExtender.Collapsed = True
                                                        ddIORStatus.Enabled = True
                                                        txtPONo.Visible = True
                                                    Case "Closed"
                                                        ddRequestedBy.Enabled = False
                                                        If (txtRoutingStatus.Text <> "C") Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                        End If
                                                        SDExtender.Collapsed = True
                                                        ExtExtender.Collapsed = True
                                                        ddIORStatus.Enabled = False
                                                        txtPONo.Visible = True
                                                    Case "Void"
                                                        ddRequestedBy.Enabled = False
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidReason.Visible = True
                                                        lblReqVoidReason.Visible = True
                                                        If (txtRoutingStatus.Text <> "V") Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                        End If
                                                        If iBuyerID <> 0 Then
                                                            lblReqPONo.Visible = True
                                                            txtPONo.Visible = True
                                                        End If
                                                        SDExtender.Collapsed = True
                                                        ExtExtender.Collapsed = True
                                                        ddIORStatus.Enabled = False
                                                End Select
                                                btnPreview.Enabled = True
                                            End If
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            '** Used by Backup persons
                                            ViewState("ObjectRole") = False
                                            If ViewState("pIORNo") <> Nothing Then
                                                If ViewState("pVNo") = 0 And ViewState("pNF") = 0 Then
                                                    mnuTabs.Items(1).Enabled = True
                                                    mnuTabs.Items(2).Enabled = True
                                                    mnuTabs.Items(3).Enabled = True
                                                    mnuTabs.Items(4).Enabled = True
                                                End If
                                                btnPreview.Enabled = True
                                                ddIORStatus.Enabled = False
                                                Select Case ProjectStatus
                                                    Case "Open"
                                                        If ddVendor.SelectedValue = Nothing Then
                                                            mnuTabs.Items(1).Enabled = False
                                                            mnuTabs.Items(2).Enabled = False
                                                            mnuTabs.Items(3).Enabled = False
                                                            mnuTabs.Items(4).Enabled = False
                                                        End If

                                                    Case "In Process"
                                                        ddRequestedBy.Enabled = False
                                                        If (txtRoutingStatus.Text = "T") Then
                                                            If iBuyerID <> 0 Then
                                                                lblReqPONo.Visible = True
                                                                txtPONo.Visible = True
                                                                txtPONo.ReadOnly = False
                                                                rfvPONo.Enabled = True
                                                            End If
                                                            gvApprovers.Columns(7).Visible = True
                                                        End If
                                                    Case "Closed"
                                                        txtPONo.Visible = True
                                                    Case "Completed"
                                                        txtPONo.Visible = True
                                                    Case "Void"
                                                        txtVoidReason.Visible = True
                                                        lblVoidReason.Visible = True
                                                End Select
                                                ''***************************************
                                                ''Allow Buyer & IS to upload docs after approval
                                                ''***************************************
                                                If iISINF <> 0 Or iBuyerID <> 0 Or ViewState("iTeamMemberID") = ddRequestedBy.SelectedValue Or ViewState("iTeamMemberID") = txtSubmittedByTMID.Text Or ViewState("iTeamMemberID") = 204 Then
                                                    btnUpload.Enabled = True
                                                    btnReset3.Enabled = True 'Supporting Documents Tab
                                                    uploadFile.Enabled = True
                                                    gvSupportingDocument.Columns(3).Visible = True
                                                Else
                                                    btnUpload.Enabled = False
                                                    btnReset3.Enabled = False 'Supporting Documents Tab
                                                    uploadFile.Enabled = False
                                                    gvSupportingDocument.Columns(3).Visible = False
                                                End If
                                            End If
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            ViewState("ObjectRole") = False
                                            If ViewState("pVNo") = 0 And ViewState("pNF") = 0 Then
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                mnuTabs.Items(4).Enabled = True
                                            End If
                                            btnPreview.Enabled = True
                                            SDExtender.Collapsed = True
                                            ExtExtender.Collapsed = True
                                            ddIORStatus.Enabled = False
                                            Select Case ProjectStatus
                                                Case "Closed"
                                                    txtPONo.Visible = True
                                                Case "Completed"
                                                    txtPONo.Visible = True
                                                Case "Void"
                                                    txtVoidReason.Visible = True
                                                    lblVoidReason.Visible = True
                                            End Select
                                            btnUpload.Enabled = False
                                            btnReset3.Enabled = False 'Supporting Documents Tab
                                            uploadFile.Enabled = False
                                            gvSupportingDocument.Columns(3).Visible = False
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ''Used by the Accounting Group
                                            ViewState("ObjectRole") = False
                                            btnUpload.Enabled = False
                                            btnReset3.Enabled = False 'Supporting Documents Tab
                                            uploadFile.Enabled = False
                                            gvSupportingDocument.Columns(3).Visible = False
                                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                            btnUpload.Enabled = False
                                            btnReset3.Enabled = False 'Supporting Documents Tab
                                            uploadFile.Enabled = False
                                            gvSupportingDocument.Columns(3).Visible = False
                                            ''** No Entry allowed **''
                                    End Select 'EOF of "Select Case iRoleID"
                                Next
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"

            ''***************
            ''Initiate Default Values as view state
            ''***************
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            ViewState("DefaultUser") = DefaultUser
            ViewState("DefaultUserFullName") = DefaultUserFullName
            ViewState("strProdOrTestEnvironment") = strProdOrTestEnvironment

        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
#End Region 'EOF Form Level Security

#Region "General Project"
    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet
            ''bind existing data to drop down Requested By control for selection criteria for search
            ds = commonFunctions.GetTeamMember("") '**SubscriptionID # is used for Project Leader
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddRequestedBy.DataSource = ds
                ddRequestedBy.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddRequestedBy.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddRequestedBy.DataBind()
                ddRequestedBy.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Ship To Attention control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(99)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddBuyer.DataSource = ds
                ddBuyer.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddBuyer.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddBuyer.DataBind()
                ddBuyer.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Ship To Attention control for selection criteria for search
            ds = commonFunctions.GetTeamMember("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddShipToAttention.DataSource = ds
                ddShipToAttention.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddShipToAttention.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddShipToAttention.DataBind()
                ddShipToAttention.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Team Member control for selection criteria for search
            ds = commonFunctions.GetTeamMember("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddTeamMember.DataSource = ds
                ddTeamMember.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddTeamMember.DataBind()
                ddTeamMember.Items.Insert(0, "")
                'ddTeamMember.Enabled = False
            End If

            commonFunctions.UserInfo()
            If ViewState("iBuyerID") = 0 And ViewState("iISINF") = 0 Then
                ddRequestedBy.SelectedValue = ViewState("iTeamMemberID")
            End If
            If ViewState("iBuyerID") <> 0 Then
                ddBuyer.SelectedValue = ViewState("iTeamMemberID")
            End If
            ' ddTeamMember.SelectedValue = IIf(ViewState("iTeamMemberID") = Nothing, HttpContext.Current.Request.Cookies("UGNDB_TMID").Value, ViewState("iTeamMemberID"))

            ddTeamMember.SelectedValue = ViewState("iTeamMemberID")

            ''bind existing data to drop down Ship To control for selection criteria for search
            ds = PURModule.GetIORUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddShipToLocation.DataSource = ds
                ddShipToLocation.DataTextField = ds.Tables(0).Columns("ddUGNFacilityAddr").ColumnName.ToString()
                ddShipToLocation.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddShipToLocation.DataBind()
                ddShipToLocation.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Department or Cost Center control for selection criteria for search
            ds = commonFunctions.GetDepartmentGLNo("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddDepartment.DataSource = ds
                ddDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName.ToString()
                ddDepartment.DataValueField = ds.Tables(0).Columns("GLNo").ColumnName.ToString()
                ddDepartment.DataBind()
                ddDepartment.Items.Insert(0, "")
            End If

            ''bind existing data to drop down GLAccounts or Cost Center control for selection criteria for search
            ds = commonFunctions.GetGLAccounts("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddGLAccount.DataSource = ds
                ddGLAccount.DataTextField = ds.Tables(0).Columns("ddGLAccountName").ColumnName.ToString()
                ddGLAccount.DataValueField = ds.Tables(0).Columns("GLNo").ColumnName.ToString()
                ddGLAccount.DataBind()
                ddGLAccount.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Vendor control for selection criteria for search
            ds = SUPModule.GetSupplierLookUp("", "", "", "", "", 1)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddVendor.DataSource = ds
                ddVendor.DataTextField = ds.Tables(0).Columns("ddVendorName").ColumnName.ToString()
                ddVendor.DataValueField = ds.Tables(0).Columns("ddVendorNo").ColumnName.ToString()
                ddVendor.DataBind()
                ddVendor.Items.Insert(0, "")
            End If

            If ViewState("pNF") > 0 Then
                DefaultVendorInfo()
            End If

            ''bind existing data to drop down Unit of Measure control for selection criteria for search
            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUOM.DataSource = ds
                ddUOM.DataTextField = ds.Tables(0).Columns("UnitName").ColumnName.ToString()
                ddUOM.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName.ToString()
                ddUOM.DataBind()
                ddUOM.Items.Insert(0, "")
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

    Public Sub BindData(ByVal IORNO As String)
        Try
            Dim ds As DataSet = New DataSet
            If IORNO <> Nothing Then
                ds = PURModule.GetInternalOrderRequest(IORNO)
                If commonFunctions.CheckDataSet(ds) = True Then
                    lblIORNO.Text = ds.Tables(0).Rows(0).Item("IORNO").ToString()
                    txtIORDescription.Text = ds.Tables(0).Rows(0).Item("IORDescription").ToString()
                    txtPONo.Text = ds.Tables(0).Rows(0).Item("PONo").ToString()
                    txtSubmittedOn.Text = ds.Tables(0).Rows(0).Item("SubmittedOn").ToString()
                    Select Case ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                        Case "N"
                            ddIORStatus.SelectedValue = ds.Tables(0).Rows(0).Item("IORStatus").ToString()
                        Case "A"
                            ddIORStatus.SelectedValue = ds.Tables(0).Rows(0).Item("IORStatus").ToString()
                        Case "C"
                            ddIORStatus.SelectedValue = ds.Tables(0).Rows(0).Item("IORStatus").ToString()
                        Case "T"
                            ddIORStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("IORStatus").ToString()
                        Case "R"
                            ddIORStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("IORStatus").ToString()
                        Case "V"
                            ddIORStatus.SelectedValue = ds.Tables(0).Rows(0).Item("IORStatus").ToString()
                    End Select

                    txtRoutingStatus.Text = ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                    lblRoutingStatusDesc.Text = ds.Tables(0).Rows(0).Item("RoutingStatusDesc").ToString()
                    txtVoidReason.Text = ds.Tables(0).Rows(0).Item("VoidReason").ToString()
                    ddRequestedBy.SelectedValue = ds.Tables(0).Rows(0).Item("RequestedByTMID").ToString()
                    txtSubmittedByTMID.Text = ds.Tables(0).Rows(0).Item("SubmittedByTMID").ToString()
                    ddBuyer.SelectedValue = ds.Tables(0).Rows(0).Item("BuyerTMID").ToString()
                    txtAppropriation.Text = ds.Tables(0).Rows(0).Item("AppropriationCode").ToString()
                    ddShipToLocation.SelectedValue = ds.Tables(0).Rows(0).Item("ShiptoLocation").ToString()
                    txtUGNLocation.Text = ds.Tables(0).Rows(0).Item("UGNFacilityName").ToString()
                    ddShipToAttention.SelectedValue = ds.Tables(0).Rows(0).Item("ShipToAttention").ToString()
                    ddPOinPesos.SelectedValue = ds.Tables(0).Rows(0).Item("POinPesos").ToString()
                    If ddShipToLocation.SelectedValue = "UW" Then
                        ddPOinPesos.Enabled = True
                    Else
                        ddPOinPesos.Enabled = False
                    End If
                    ddDepartment.SelectedValue = ds.Tables(0).Rows(0).Item("DepartmentID").ToString()
                    ddGLAccount.SelectedValue = ds.Tables(0).Rows(0).Item("GLNo").ToString()
                    txtExptdDeliveryDate.Text = ds.Tables(0).Rows(0).Item("ExpectedDeliveryDate").ToString()
                    cbShippingPoint.Checked = ds.Tables(0).Rows(0).Item("ShippingPoint").ToString()
                    cbDestination.Checked = ds.Tables(0).Rows(0).Item("Destination").ToString()
                    cbTaxExempt.Checked = ds.Tables(0).Rows(0).Item("TaxExempt").ToString()
                    cbTaxable.Checked = ds.Tables(0).Rows(0).Item("Taxable").ToString()
                    ddShipTo.SelectedValue = ds.Tables(0).Rows(0).Item("ShipTo").ToString()
                    txtNotes.Text = ds.Tables(0).Rows(0).Item("Notes").ToString()
                    txtTotalExtension.Text = Format(ds.Tables(0).Rows(0).Item("TotalExpense"), "#,##0.00")
                    hdTotalExtension.Text = Format(ds.Tables(0).Rows(0).Item("TotalExpense"), "#,##0.00")

                    If ViewState("pNF") = 0 Then
                        txtVTYPE.Text = ds.Tables(0).Rows(0).Item("VendorType").ToString()
                        ddVendor.SelectedValue = ds.Tables(0).Rows(0).Item("VendorNo").ToString()
                        txtVendorName.Text = ds.Tables(0).Rows(0).Item("VendorName").ToString()
                        txtVendorAddr1.Text = ds.Tables(0).Rows(0).Item("VendorAddr1").ToString()
                        txtVendorAddr2.Text = ds.Tables(0).Rows(0).Item("VendorAddr2").ToString()
                        txtVendorCountry.Text = ds.Tables(0).Rows(0).Item("VendorCountry").ToString()
                        txtVendorCity.Text = ds.Tables(0).Rows(0).Item("VendorCity").ToString()
                        txtVendorState.Text = ds.Tables(0).Rows(0).Item("VendorState").ToString()
                        txtVendorZip.Text = ds.Tables(0).Rows(0).Item("VendorZip").ToString()
                        txtVendorContact.Text = ds.Tables(0).Rows(0).Item("VendorContact").ToString()
                        txtVendorWebsite.Text = ds.Tables(0).Rows(0).Item("VendorWebsite").ToString()
                        txtVendorPhone.Text = ds.Tables(0).Rows(0).Item("VendorPhone").ToString()
                        txtVendorFax.Text = ds.Tables(0).Rows(0).Item("VendorFax").ToString()
                        txtVendorEmail.Text = ds.Tables(0).Rows(0).Item("VendorEmail").ToString()
                        txtTerms.Text = ds.Tables(0).Rows(0).Item("Terms").ToString()
                    Else
                        DefaultVendorInfo()
                    End If

                    If ViewState("pEID") <> 0 Then
                        Dim dsE As DataSet = New DataSet
                        dsE = PURModule.GetInternalOrderRequestExpenditure(ViewState("pIORNo"), ViewState("pEID"))
                        If (ds.Tables.Item(0).Rows.Count > 0) Then
                            txtSizePN.Text = dsE.Tables(0).Rows(0).Item("SizePN").ToString()
                            txtDescription.Text = dsE.Tables(0).Rows(0).Item("Description").ToString()
                            txtQuantity.Text = dsE.Tables(0).Rows(0).Item("Quantity").ToString()
                            txtAmountPer.Text = dsE.Tables(0).Rows(0).Item("Amount").ToString()
                            ddCurrency.SelectedValue = dsE.Tables(0).Rows(0).Item("Currency").ToString()
                            If txtAppropriation.Text <> Nothing Then
                                ddCurrency.Enabled = False
                            End If
                            txtComments.Text = dsE.Tables(0).Rows(0).Item("Notes").ToString()
                            ddUOM.SelectedValue = dsE.Tables(0).Rows(0).Item("UnitID").ToString()
                            txtHDExpenseAmount.Text = (dsE.Tables(0).Rows(0).Item("Quantity").ToString() * dsE.Tables(0).Rows(0).Item("Amount").ToString())
                        Else 'no record found reset query string pRptID
                            Response.Redirect("InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pEID=0", False)
                        End If
                    End If 'EOF If ViewState("pEID") <> 0 Then

                    '*************
                    ''* Check that the Appropriation entered is a valid entry in SQL
                    ''*************
                    Dim ds2 As DataSet = New DataSet
                    ds2 = PURModule.GetInternalOrderRequestCapEx(ViewState("pIORNo"), "")
                    If commonFunctions.CheckDataSet(ds2) = False Then
                        If txtAppropriation.Text <> Nothing Then
                            ds2 = PURModule.GetInternalOrderRequestCapEx(0, txtAppropriation.Text)
                        End If
                    End If
                    If commonFunctions.CheckDataSet(ds2) = True Then
                        If txtAppropriation.Text <> Nothing Then
                            If Not IsDBNull(ds2.Tables(0).Rows(0).Item("ProjectStatus")) And (Not IsDBNull(ds2.Tables(0).Rows(0).Item("ProjectTitle"))) Then
                                txtProjectStatus.Text = ds2.Tables(0).Rows(0).Item("ProjectStatus")
                                Select Case txtProjectStatus.Text
                                    Case "Void"
                                        lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION IS VOID."
                                        lblErrors.Visible = "True"
                                        btnSave1.Enabled = False

                                    Case "Open"
                                        lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION IS PENDING SUBMISSION FOR APPROVAL."
                                        lblErrors.Visible = "True"
                                        btnSave1.Enabled = False

                                    Case "In Process"
                                        lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION IS IN PROCESS FOR APPROVAL."
                                        lblErrors.Visible = "True"
                                        btnSave1.Enabled = False

                                    Case "Rejected"
                                        lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION IS REJECTED."
                                        lblErrors.Visible = "True"
                                        btnSave1.Enabled = False

                                    Case "Capitalized"
                                        lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION HAS BEEN CAPITALIZED."
                                        lblErrors.Visible = "True"
                                        btnSave1.Enabled = False

                                    Case "Completed"
                                        lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION HAS BEEN COMPLETED."
                                        lblErrors.Visible = "True"
                                        btnSave1.Enabled = False

                                End Select
                            End If 'EOF if ds2.Tables(0).Rows(0).Item("ProjectStatus")

                            If Not IsDBNull(ds2.Tables(0).Rows(0).Item("ProjectTitle")) Then
                                txtProjectTitle.Text = ds2.Tables(0).Rows(0).Item("ProjectTitle")
                                hplkAppropriation.Text = ds2.Tables(0).Rows(0).Item("ProjectTitle")
                                hplkAppropriation.Visible = True
                                If txtAppropriation.Text <> Nothing And (txtProjectTitle.Text <> Nothing) Then
                                    Select Case txtAppropriation.Text.Substring(0, 1)
                                        Case "A"
                                            hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjAssets.aspx?pProjNo=" & txtAppropriation.Text
                                        Case "D"
                                            hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjDevelopment.aspx?pProjNo=" & txtAppropriation.Text
                                        Case "P"
                                            hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjPackaging.aspx?pProjNo=" & txtAppropriation.Text
                                        Case "R"
                                            hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjRepair.aspx?pProjNo=" & txtAppropriation.Text
                                        Case "T"
                                            hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjTooling.aspx?pProjNo=" & txtAppropriation.Text
                                    End Select

                                    txtTotalCapEx.Text = Format(ds2.Tables(0).Rows(0).Item("AllowedToSpend"), "#,##0.00")
                                    txtTotalSpent.Text = Format(ds2.Tables(0).Rows(0).Item("IORTotalSpent"), "#,##0.00")
                                    txtRemainingCapEx.Text = Format(ds2.Tables(0).Rows(0).Item("RemSpendAmount"), "#,##0.00")
                                Else
                                    txtTotalCapEx.Visible = True
                                    lblReqApprovedSpending.Visible = True
                                    txtRemainingCapEx.Text = Format(ds.Tables(0).Rows(0).Item("ApprovedSpending") - txtTotalCapEx.Text - txtTotalExtension.Text, "#,##0.00")

                                End If 'EOF If txtAppropriation.Text <> Nothing Then
                            Else
                                hplkAppropriation.Text = "Not Found in UGNDB"
                                hplkAppropriation.Visible = True
                                txtTotalCapEx.Visible = True
                                lblReqApprovedSpending.Visible = True
                                txtRemainingCapEx.Text = Format(ds.Tables(0).Rows(0).Item("ApprovedSpending") - txtTotalCapEx.Text - txtTotalExtension.Text, "#,##0.00")

                            End If 'EOF If Not IsDBNull(ds2.Tables(0).Rows(0).Item("ProjectTitle")) Then
                            If Not IsDBNull(ds2.Tables(0).Rows(0).Item("DefinedCapEx")) Then
                                txtDefinedCapex.Text = ds2.Tables(0).Rows(0).Item("DefinedCapEx")
                            End If

                        Else
                            txtTotalCapEx.Text = "0.00"
                            txtTotalSpent.Text = "0.00"
                            txtRemainingCapEx.Text = "0.00"
                            txtProjectTitle.Text = Nothing
                            txtDefinedCapex.Text = Nothing
                            hplkAppropriation.Visible = False
                        End If 'EOF If txtAppropriation.Text <> Nothing Then
                    Else
                        If txtAppropriation.Text = Nothing Then
                            txtProjectTitle.Text = Nothing
                            txtDefinedCapex.Text = Nothing
                            hplkAppropriation.Visible = False
                        Else
                            txtTotalCapEx.Visible = True
                            lblReqApprovedSpending.Visible = True
                            txtRemainingCapEx.Text = Format(ds.Tables(0).Rows(0).Item("ApprovedSpending") - txtTotalCapEx.Text - txtTotalExtension.Text, "#,##0.00")

                        End If 'EOF  If txtAppropriation.Text = Nothing Then
                    End If 'EOF   If commonFunctions.CheckDataSet(ds2) = True Then

                    If ViewState("pRID") <> 0 Then
                        ds = PURModule.GetInternalOrderRequestRSS(ViewState("pIORNo"), ViewState("pRID"))
                        If (ds.Tables.Item(0).Rows.Count > 0) Then
                            txtQC.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                        Else 'no record found reset query string pRptID
                            Response.Redirect("InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pRID=0&pRC=1", False)
                        End If
                    End If

                    If ViewState("pED") = 1 Then 'Rebuild Approval when an expense line item is deleted
                        BuildApprovalList()
                    End If
                End If 'EOF If commonFunctions.CheckDataSet(ds) = True Then
            End If 'EOF If IORNO <> Nothing Then

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindData


    ' ''Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
    ' ''    Response.Redirect("InternalOrderRequest.aspx", False)
    ' ''End Sub 'EOF btnAdd_Click

    Protected Sub txtAppropriation_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAppropriation.TextChanged
        Try
            Dim ds As DataSet = New DataSet

            ''*************
            ''* Check that the Appropriation entered is a valid entry in SQL
            ''*************
            ''If true, default Total Expense minus the Total previously expensed if an IOR was written previously
            ''If false, there is no calculation involved. Ask TM to enter the total amount of the approved expense.
            txtAppropriation.Text = txtAppropriation.Text.ToUpper()
            txtTotalCapEx.Text = "0.00"
            txtTotalExtension.Text = "0.00"
            txtTotalSpent.Text = "0.00"
            txtRemainingCapEx.Text = "0.00"
            txtProjectTitle.Text = Nothing
            hplkAppropriation.Text = Nothing
            hplkAppropriation.Visible = False
            lblReqApprovedSpending.Visible = False

            If (txtAppropriation.Text <> Nothing) Then
                ds = PURModule.GetInternalOrderRequestCapEx(0, txtAppropriation.Text)
                If commonFunctions.CheckDataSet(ds) = True Then
                    txtTotalCapEx.Text = Format(ds.Tables(0).Rows(0).Item("AllowedToSpend"), "#,##0.00")
                    txtTotalSpent.Text = Format(ds.Tables(0).Rows(0).Item("IORTotalSpent"), "#,##0.00")
                    txtRemainingCapEx.Text = Format(ds.Tables(0).Rows(0).Item("RemSpendAmount"), "#,##0.00")
                    txtDefinedCapex.Text = ds.Tables(0).Rows(0).Item("DefinedCapEx")
                    txtProjectTitle.Text = ds.Tables(0).Rows(0).Item("ProjectTitle")
                    hplkAppropriation.Text = ds.Tables(0).Rows(0).Item("ProjectTitle")
                    txtProjectStatus.Text = ds.Tables(0).Rows(0).Item("ProjectStatus")
                    Select Case txtProjectStatus.Text
                        Case "Void"
                            lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION IS VOID."
                            lblErrors.Visible = "True"
                            btnSave1.Enabled = False

                        Case "Open"
                            lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION IS PENDING SUBMISSION FOR APPROVAL."
                            lblErrors.Visible = "True"
                            btnSave1.Enabled = False

                        Case "In Process"
                            lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION IS IN PROCESS FOR APPROVAL."
                            lblErrors.Visible = "True"
                            btnSave1.Enabled = False

                        Case "Rejected"
                            lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION WAS REJECTED."
                            lblErrors.Visible = "True"
                            btnSave1.Enabled = False

                        Case "Capitalized"
                            lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION HAS BEEN CAPITALIZED."
                            lblErrors.Visible = "True"
                            btnSave1.Enabled = False

                        Case "Completed"
                            lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION HAS BEEN COMPLETED."
                            lblErrors.Visible = "True"
                            btnSave1.Enabled = False

                        Case "Approved"
                            If txtTotalExtension.Text = "0.00" And txtRemainingCapEx.Text = "0.00" Then
                                lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION HAS NO REMAINING BALANCE."
                                lblErrors.Visible = "True"
                                btnSave1.Enabled = False
                            End If

                        Case Else
                            lblErrors.Text = Nothing
                            lblErrors.Visible = False
                            btnSave1.Enabled = True
                    End Select
                    If txtProjectTitle.Text <> Nothing Then
                        Select Case txtDefinedCapex.Text
                            Case "A"
                                hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjAssets.aspx?pProjNo=" & txtAppropriation.Text
                                hplkAppropriation.Visible = True
                            Case "D"
                                hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjDevelopment.aspx?pProjNo=" & txtAppropriation.Text
                                hplkAppropriation.Visible = True
                            Case "P"
                                hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjPackaging.aspx?pProjNo=" & txtAppropriation.Text
                                hplkAppropriation.Visible = True
                            Case "R"
                                hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjRepair.aspx?pProjNo=" & txtAppropriation.Text
                                hplkAppropriation.Visible = True
                            Case "T"
                                hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjTooling.aspx?pProjNo=" & txtAppropriation.Text
                                hplkAppropriation.Visible = True
                            Case Else
                                hplkAppropriation.Visible = False

                        End Select

                    End If 'EOF  If txtProjectTitle.Text <> Nothing Then
                Else
                    txtTotalCapEx.Visible = True
                    lblReqApprovedSpending.Visible = True

                    lblErrors.Text = "APPROPRIATION IS NOT FOUND IN THE UGNDB, PLEASE REVIEW OR CONTACT THE APPLICATION GROUP FOR ASSISTANCE."
                    lblErrors.Visible = "True"
                    btnSave1.Enabled = False
                End If 'EOF If commonFunctions.CheckDataSet(ds) = True Then
            End If 'EOF If (txtAppropriation.Text <> Nothing) Then

            ''****************
            ''Default GL Account Number
            ''****************
            Select Case txtAppropriation.Text.Substring(0, 1)
                Case "A"
                    ddDepartment.SelectedValue = 0
                    ddGLAccount.SelectedValue = 214410
                Case "D"
                    ddGLAccount.SelectedValue = 943200
                    ddDepartment.SelectedValue = 510000
                Case "P"
                    ddGLAccount.SelectedValue = 162159
                    ddDepartment.SelectedValue = 0
                Case "R"
                    ddGLAccount.SelectedValue = 752125
                Case "T"
                    ddGLAccount.SelectedValue = 162167
                    ddDepartment.SelectedValue = 0
            End Select

            CheckRights()
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF txtAppropriation_TextChanged

    Public Function GoToCapEx(ByVal ProjectNo As String, ByVal DocID As String) As String
        Dim strReturnValue As String = ""
        If ProjectNo <> Nothing Then
            Select Case ProjectNo.Substring(0, 1)
                Case "A"
                    strReturnValue = "~/EXP/AssetsExpProjDocument.aspx?pProjNo=" & ProjectNo & "&pDocID=" & DocID
                Case "D"
                    Return "~/EXP/DevelopmentExpProjDocument.aspx?pProjNo=" & ProjectNo & "&pDocID=" & DocID
                Case "P"
                    Return "~/EXP/PackagingExpProjDocument.aspx?pProjNo=" & ProjectNo & "&pDocID=" & DocID
                Case "R"
                    Return "~/EXP/RepairExpProjDocument.aspx?pProjNo=" & ProjectNo & "&pDocID=" & DocID
                Case "T"
                    strReturnValue = "~/EXP/ToolingExpProjDocument.aspx?pProjNo=" & ProjectNo & "&pDocID=" & DocID
            End Select
        End If
        GoToCapEx = strReturnValue

    End Function 'EOF GoToCapEx

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultTMID As Integer = ViewState("iTeamMemberID")
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            Dim ProjectStatus As String = Nothing
            ProjectStatus = ViewState("ProjectStatus")

            If (ViewState("pIORNo") <> Nothing Or ViewState("pIORNo") <> "") Then
                '***************
                '* Update Data
                '***************
                If ProjectStatus = "Completed" And ((ViewState("iTeamMemberID") <> ddRequestedBy.SelectedValue) And (ViewState("iTeamMemberID") <> ddBuyer.SelectedValue)) Then
                    lblErrors.Text = "You do not have authorization to 'Complete' this IOR."
                    lblErrors.Visible = True
                    Exit Sub
                End If

                UpdateRecord(ProjectStatus, IIf(ProjectStatus = "Closed", "C", IIf(ProjectStatus = "Void", "V", IIf(ProjectStatus = "Open", "N", txtRoutingStatus.Text))), False)

                ''*************
                ''Check for Closed, Void status send email notfication 
                ''*************
                If ProjectStatus = "Closed" And txtRoutingStatus.Text = "C" Then
                    ''*****************
                    ''History Tracking
                    ''*****************
                    PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), txtIORDescription.Text, DefaultTMID, "Closed")
                    'SendNotifWhenEventChanges("Closed")
                ElseIf ProjectStatus = "Completed" And (ViewState("iTeamMemberID") <> ddRequestedBy.SelectedValue) Then
                    'anyone other than the initiator can submit email for completed.
                    ''*****************
                    ''History Tracking
                    ''*****************
                    PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), txtIORDescription.Text, DefaultTMID, "Completed")
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Completed")
                    End If
                ElseIf ProjectStatus = "Void" Then 'And txtRoutingStatus.Text = "V"
                    ''*****************
                    ''History Tracking
                    ''*****************
                    PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), txtIORDescription.Text, DefaultTMID, "Void" & txtVoidReason.Text)
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Void")
                    End If
                End If

                '**************
                '* Reload the data - may contain calculated information to TotalInv and ProfitLoss
                '**************
                If txtQuantity.Text = Nothing Then
                    BindData(ViewState("pIORNo"))
                End If

                If ViewState("pVNo") <> 0 And ViewState("pNF") <> 0 Then
                    Response.Redirect("InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & txtAppropriation.Text, False)
                End If

            Else
                '***************
                '* Save Data
                '***************
                PURModule.InsertInternalOrderRequest(txtIORDescription.Text, "Open", "N", ddRequestedBy.SelectedValue, txtPONo.Text, txtAppropriation.Text, txtTotalCapEx.Text, txtExptdDeliveryDate.Text, ddShipToAttention.SelectedValue, ddShipTo.SelectedValue, ddShipToLocation.SelectedValue, IIf(ddPOinPesos.SelectedValue = Nothing, 0, ddPOinPesos.SelectedValue), ddDepartment.SelectedValue, ddGLAccount.SelectedValue, txtVTYPE.Text, IIf(ddVendor.SelectedValue = Nothing, 0, ddVendor.SelectedValue), txtVendorName.Text, txtVendorAddr1.Text, txtVendorAddr2.Text, txtVendorCountry.Text, txtVendorCity.Text, txtVendorState.Text, txtVendorZip.Text, txtVendorContact.Text, txtVendorPhone.Text, txtVendorFax.Text, txtVendorWebsite.Text, txtVendorEmail.Text, cbTaxExempt.Checked, cbTaxable.Checked, cbShippingPoint.Checked, cbDestination.Checked, txtTerms.Text, txtNotes.Text, ddBuyer.SelectedValue, DefaultTMID, DefaultUser, DefaultDate)

                '***************
                '* Locate Next available ProjectNo based on Facility selection
                '***************
                Dim ds As DataSet = Nothing
                ds = PURModule.GetLastInternalOrderRequestNo(ddRequestedBy.SelectedValue, txtIORDescription.Text, ddShipToLocation.SelectedValue, "N", ddDepartment.SelectedValue, ddGLAccount.SelectedValue, DefaultUser, DefaultDate)

                ViewState("pIORNo") = ds.Tables(0).Rows(0).Item("LastIORNO").ToString()

                ''*****************
                ''History Tracking
                ''*****************
                PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), txtIORDescription.Text, DefaultTMID, "Record created.")

                ''***************
                ''* Redirect user back to the page.
                ''***************
                Dim Aprv As String = Nothing
                If ViewState("pAprv") = 1 Then
                    Aprv = "&pAprv=1"
                End If

                Response.Redirect("InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & txtAppropriation.Text & Aprv, False)
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

    Public Function UpdateRecord(ByVal RecStatus As String, ByVal RoutingStatus As String, ByVal RecSubmitted As Boolean) As String
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = ViewState("iTeamMemberID") ' HttpContext.Current.Request.Cookies("UGNDB_TMID").Value


            PURModule.UpdateInternalOrderRequest(ViewState("pIORNo"), txtIORDescription.Text, RecStatus, RoutingStatus, ddRequestedBy.SelectedValue, txtPONo.Text, txtAppropriation.Text, txtTotalCapEx.Text, txtExptdDeliveryDate.Text, ddShipToAttention.SelectedValue, ddShipTo.SelectedValue, ddShipToLocation.SelectedValue, IIf(ddPOinPesos.SelectedValue = Nothing, 0, ddPOinPesos.SelectedValue), ddDepartment.SelectedValue, ddGLAccount.SelectedValue, txtVTYPE.Text, IIf(txtFutureVendor.Text = Nothing, 0, txtFutureVendor.Text), ddVendor.SelectedValue, txtVendorName.Text, txtVendorAddr1.Text, txtVendorAddr2.Text, txtVendorCountry.Text, txtVendorCity.Text, txtVendorState.Text, txtVendorZip.Text, txtVendorContact.Text, txtVendorPhone.Text, txtVendorFax.Text, txtVendorWebsite.Text, txtVendorEmail.Text, cbTaxExempt.Checked, cbTaxable.Checked, cbShippingPoint.Checked, cbDestination.Checked, txtTerms.Text, txtVoidReason.Text, txtNotes.Text, ddBuyer.SelectedValue, IIf(RecSubmitted = False, txtSubmittedOn.Text, DefaultDate), IIf(RecSubmitted = False, txtSubmittedByTMID.Text, IIf(txtSubmittedOn.Text <> Nothing, txtSubmittedByTMID.Text, DefaultTMID)), DefaultUser, DefaultDate)

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

    Protected Sub btnReset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset1.Click, btnReset2.Click, btnReset3.Click, btnReset4.Click
        'Redirect user to the same tab view when Reset button was clicked
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If

        Dim TempViewState As Integer
        If ViewState("pIORNo") <> Nothing Or ViewState("pIORNo") <> "" Then
            TempViewState = mvTabs.ActiveViewIndex
            mvTabs.GetActiveView()
            mnuTabs.Items(TempViewState).Selected = True

            BindData(ViewState("pIORNo"))
        Else
            Response.Redirect("InternalOrderRequest.aspx", False)
        End If

    End Sub 'EOF btnReset1_Click  

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            '***************
            '* Delete Record
            '***************
            If ViewState("pIORNo") <> Nothing Then
                PURModule.DeleteInternalOrderRequest(ViewState("pIORNo"))

                '***************
                '* Redirect user back to the search page.
                '***************
                Response.Redirect("InternalOrderRequestList.aspx", False)
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
    End Sub 'EOF btnDelete_Click

    Protected Sub ddIORStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddIORStatus.SelectedIndexChanged, ddIORStatus2.SelectedIndexChanged
        If ViewState("Admin") = False Then

            Select Case txtRoutingStatus.Text
                Case "T"
                    If ddIORStatus2.SelectedValue = "Void" Then
                        ddIORStatus2.SelectedValue = "Void"
                    ElseIf ddIORStatus.SelectedValue = "Closed" Then
                        ddIORStatus.SelectedValue = "Closed"
                    Else
                        ddIORStatus2.SelectedValue = "In Process"
                    End If
                Case "N"
                    ddIORStatus.SelectedValue = "Open"
                Case "C"
                    ddIORStatus.SelectedValue = "Closed"
                Case "V"
                    ddIORStatus2.SelectedValue = "Void"
            End Select
        End If
    End Sub 'EOF ddIORStatus_SelectedIndexChanged

#End Region 'EOF General Project

#Region "Supplier/Vendor Lookup and Default"
    Protected Sub ibtnSupplierLookUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSupplierLookUp.Click

        'Form Name IOR
        ViewState("pFVTNo") = False
        Response.Redirect("~\SUP\SupplierLookUp.aspx?sBtnSrch=False&pForm=PURIOR&pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & txtAppropriation.Text, False)

    End Sub 'EOF ibtnSupplierLookUp_Click

    Protected Sub ddVendor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddVendor.SelectedIndexChanged

        DefaultVendorInfo()

    End Sub 'EOF ddVendor_SelectedIndexChanged

    Public Sub DefaultVendorInfo()
        Try
            Dim ds As DataSet = New DataSet
            lblErrors.Text = Nothing
            lblErrors.Visible = False

            If ViewState("pNF") <> 0 Then
                txtVendorName.Text = Nothing
                txtVendorAddr1.Text = Nothing
                txtVendorAddr2.Text = Nothing
                txtVendorCountry.Text = Nothing
                txtVendorCity.Text = Nothing
                txtVendorState.Text = Nothing
                txtVendorZip.Text = Nothing
                txtVendorContact.Text = Nothing
                txtVendorPhone.Text = Nothing
                txtVendorFax.Text = Nothing
                txtVendorWebsite.Text = Nothing
                txtVendorEmail.Text = Nothing
                txtTerms.Text = Nothing
                txtVTYPE.Text = Nothing
                txtFutureVendor.Text = False
            End If

            ''*****************************************************************
            ''If the Supplier Request (f) record selected in ddVendor did not come from the lookup
            ''page use this workaround to identify the record for Vendor Address default below.
            ''*****************************************************************
            Dim dsSup As DataSet = New DataSet
            Dim SUPNo As Integer = 0
            dsSup = SUPModule.GetSupplierLookUp(ddVendor.SelectedValue, "", "", "", "", 1)
            If (dsSup.Tables.Item(0).Rows.Count > 0) Then
                SUPNo = dsSup.Tables(0).Rows(0).Item("SUPNo").ToString()
                txtFutureVendor.Text = True
            End If

            ''Default Vendor Address
            ds = commonFunctions.GetVendorAddress(IIf(ddVendor.SelectedValue = Nothing, ViewState("pVNo"), ddVendor.SelectedValue), IIf(ViewState("pNF") = 0, IIf(SUPNo = 0, 2, 1), IIf(ViewState("pVNo") <> SUPNo, 2, 1)))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                If ddVendor.SelectedValue = Nothing Then
                    ddVendor.SelectedValue = ds.Tables(0).Rows(0).Item("VendorNo").ToString()
                End If
                txtVendorName.Text = ds.Tables(0).Rows(0).Item("VNDNAM").ToString()
                txtVendorAddr1.Text = ds.Tables(0).Rows(0).Item("VNDAD1").ToString()
                txtVendorAddr2.Text = ds.Tables(0).Rows(0).Item("VNDAD2").ToString()
                txtVendorCountry.Text = ds.Tables(0).Rows(0).Item("VCOUN").ToString()
                txtVendorCity.Text = ds.Tables(0).Rows(0).Item("VCITY").ToString()
                txtVendorState.Text = ds.Tables(0).Rows(0).Item("VSTATE").ToString()
                txtVendorZip.Text = ds.Tables(0).Rows(0).Item("VPOST").ToString()
                txtVendorContact.Text = ds.Tables(0).Rows(0).Item("VCONTACT").ToString()
                txtVendorPhone.Text = ds.Tables(0).Rows(0).Item("VPHONE").ToString()
                txtVendorFax.Text = ds.Tables(0).Rows(0).Item("VMVFAX").ToString()
                txtVendorWebsite.Text = Nothing
                txtVendorEmail.Text = Nothing
                txtTerms.Text = ds.Tables(0).Rows(0).Item("VTERMS").ToString()
                txtVTYPE.Text = ds.Tables(0).Rows(0).Item("VTYPE").ToString()
            End If


            If ViewState("pVNo") = 0 And ViewState("pNF") = 0 Then
                ViewState("pVNo") = 1
                ViewState("pNF") = 1
                CheckRights()
            End If
            lblErrors.Text = "New Vendor assigned, please save record to proceed."
            lblErrors.Visible = True
            MaintainScrollPositionOnPostBack = False


        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF DefaultVendorInfo
#End Region 'EOF Supplier/Vendor Lookup and Default

#Region "Extension"
    Protected Sub btnExtension_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExtension.Click
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = ViewState("iTeamMemberID") 'HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            If ViewState("pIORNo") <> Nothing Then
                Dim NewExpenseAmt As Decimal = (txtQuantity.Text * txtAmountPer.Text)
                Dim HDExpenseAmt As Decimal = IIf(txtHDExpenseAmount.Text = Nothing, 0, txtHDExpenseAmount.Text)

                If ViewState("pEID") = 0 Or ViewState("pEID") = Nothing Then
                    If ((txtRemainingCapEx.Text = 0) And (txtProjectTitle.Text <> Nothing)) Then
                        txtSizePN.Text = Nothing
                        txtDescription.Text = Nothing
                        txtQuantity.Text = Nothing
                        txtAmountPer.Text = Nothing
                        txtComments.Text = Nothing
                        ddUOM.SelectedValue = Nothing
                        lblErrors.Text = "Unable to process entry. Limit for approved amount has been reached."
                        lblErrors.Visible = True
                        Exit Sub
                    End If

                    '***************
                    '* Insert Expense information to table
                    '***************
                    PURModule.InsertInternalOrderRequestExpenditure(ViewState("pIORNo"), txtSizePN.Text, ddUOM.SelectedValue, txtDescription.Text, IIf(txtQuantity.Text = "", 0, txtQuantity.Text), IIf(txtAmountPer.Text = "", 0, txtAmountPer.Text), ddCurrency.SelectedValue, txtComments.Text, 0, 0, DefaultUser)

                    txtSizePN.Text = Nothing
                    txtDescription.Text = Nothing
                    txtQuantity.Text = Nothing
                    txtAmountPer.Text = Nothing
                    txtComments.Text = Nothing
                    ddUOM.SelectedValue = Nothing

                    gvExpense.DataBind()
                Else 'ELSE  If ViewState("pEID") = 0 Or ViewState("pEID") = Nothing Then
                    '***************
                    '* Update Expense information to table
                    '***************
                    PURModule.UpdateInternalOrderRequestExpenditure(ViewState("pEID"), ViewState("pIORNo"), txtSizePN.Text, ddUOM.SelectedValue, txtDescription.Text, IIf(txtQuantity.Text = "", 0, txtQuantity.Text), IIf(txtAmountPer.Text = "", 0, txtAmountPer.Text), ddCurrency.SelectedValue, txtComments.Text, 0, 0, DefaultUser)

                    Response.Redirect("InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pEV=1", False)
                End If

                If (txtRoutingStatus.Text <> "R") Then
                    BuildApprovalList()
                Else
                    ''***** Only rebuild if TotalExtension is greater than original value
                    Dim newVal As Decimal = 0
                    newVal = (hdTotalExtension.Text - HDExpenseAmt) + NewExpenseAmt

                    If (newVal > txtTotalExtension.Text) Or (hdTotalExtension.Text = 0) Then
                        'lblErrors.Text = newVal      'Used for testing
                        'lblErrors.Visible = True     'Used for testing
                        BuildApprovalList()
                    End If

                End If
            End If

            gvApprovers.DataBind()
            BindCriteria()
            BindData(ViewState("pIORNo"))

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnExtension_Click

    Protected Sub gvExpense_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvExpense.RowCommand
        Try

            If e.CommandName = "Delete" Then
                Response.Redirect("InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pEV=1&pED=1", False)
            End If 'EOF   If e.CommandName = "Delete" Then

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub 'EOF gvExpense_RowCommand

    Protected Sub gvExpense_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExpense.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(11).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As IOR.Internal_Order_Request_ExpenditureRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, IOR.Internal_Order_Request_ExpenditureRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record for " & DataBinder.Eval(e.Row.DataItem, "Description") & "?');")
                End If
            End If
        End If
    End Sub 'EOF gvExpense_RowDataBound
#End Region 'EOF Extension

#Region "Supporting Document"
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim DefaultDate As Date = Date.Now
            lblMessageView4.Text = Nothing
            lblMessageView4.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False
            lblErrors.Text = Nothing
            lblErrors.Visible = False

            If ViewState("pIORNo") <> "" Then
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
                                lblMessageView4.Text = "File name: " & uploadFile.FileName & "<br>" & _
                                "File Size: " & CType((SupportingDocFileSize / 1024), Integer) & " KB<br>"
                                lblMessageView4.Visible = True
                                lblMessageView4.Width = 500
                                lblMessageView4.Height = 30

                                ''***************
                                '' Insert Record
                                ''***************
                                PURModule.InsertInternalOrderRequestDocuments(ViewState("pIORNo"), ddTeamMember.SelectedValue, txtFileDesc.Text, SupportingDocBinaryFile, uploadFile.FileName, SupportingDocEncodeType, SupportingDocFileSize)
                            End If
                            gvSupportingDocument.DataBind()
                            revUploadFile.Enabled = False
                            txtFileDesc.Text = Nothing
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
                    Dim price As IOR.Internal_Order_Request_DocumentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, IOR.Internal_Order_Request_DocumentsRow)

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
            Response.Redirect("InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pSD=1" & Aprv, False)
        End If
    End Sub 'EOF gvSupportingDocument_RowCommand

#End Region 'EOF Supporting Document

#Region "Approval Status"
    Protected Sub btnBuildApprovalList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuildApprovalList.Click
        btnSave1_Click(sender, e)
        BuildApprovalList()
        gvApprovers.DataBind()

        mvTabs.ActiveViewIndex = Int32.Parse(3)
        mvTabs.GetActiveView()
        mnuTabs.Items(3).Selected = True
    End Sub 'EOF btnBuildApprovalList_Click

    Public Function BuildApprovalList() As String
        ''******************
        ''* Rebuild approval list. 
        ''******************
        Dim DefaultDate As Date = Date.Now
        Dim DefaultUser As String = ViewState("DefaultUser")
        Dim DefaultTMID As Integer = ViewState("iTeamMemberID")
        Dim TotalExpense As Decimal = 0

        Dim ds As DataSet = New DataSet
        Dim ds2 As DataSet = New DataSet
        Dim Dpt1 As Integer = 0
        Dim Dpt2 As Integer = 0
        Dim Dpt3 As Integer = 0
        Dim Dpt4 As Integer = 0
        Dim Dpt5 As Integer = 0

        lblErrors.Visible = True
        lblErrors.Text = Nothing
        lblReqAppComments.Visible = True
        lblReqAppComments.Text = Nothing

        ds = PURModule.GetInternalOrderRequest(ViewState("pIORNo"))
        If commonFunctions.CheckDataSet(ds) = True Then
            ''***** 
            ''Confirm Requestor's workflow setup prior to creating the approval chain
            ''*****
            ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(ddRequestedBy.SelectedValue, 98)
            If commonFunctions.CheckDataSet(ds2) = True Then
                Dpt1 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
            End If
            If Dpt1 = 0 Then
                lblErrors.Text = "Team Member in Requested By field does not have the proper Work Flow. Please contact Lynette Rey for assistance."
                lblErrors.Visible = True
                Return False
                Exit Function
            End If

            TotalExpense = ds.Tables(0).Rows(0).Item("TotalExpense").ToString()
            ''******************
            ''* Delete current list for rebuild
            ''******************
            PURModule.DeleteInternalOrderRequestApproval(ViewState("pIORNo"), 0, 0)

            If txtAppropriation.Text <> Nothing Then 'Record pre-approved go ahead to notify Corp Buyer
                ''********************************************
                ''Include the Program Manager of the T for 1st level approval
                ''********************************************
                If txtAppropriation.Text.Substring(0, 1) = "T" Then
                    Dim ds1 As DataSet = New DataSet
                    Dim TMID As Integer = 0
                    ds1 = EXPModule.GetExpProjToolingLead(txtAppropriation.Text)
                    If commonFunctions.CheckDataSet(ds1) = True Then
                        For i = 0 To ds1.Tables.Item(0).Rows.Count - 1
                            If ds1.Tables(0).Rows(i).Item("TMDesc") = "Program Manager" Then
                                TMID = ds1.Tables(0).Rows(i).Item("TMID")
                                PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), TMID, 0, False, DefaultUser, DefaultDate)
                            End If
                        Next
                    End If
                End If

                ''********************************************
                ''* Locate Team Member to approve up to 1000
                ''********************************************
                If TotalExpense > 0 Then
                    ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(ddRequestedBy.SelectedValue, 98)
                    If commonFunctions.CheckDataSet(ds2) = True Then
                        Dpt1 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                    End If
                    PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt1, 0, False, DefaultUser, DefaultDate)
                End If

            Else ' If txtAppropriation.Text = Nothing do else
                ''********************************************
                ''* Build Approvals when there are no project numbers assigned            
                ''********************************************
                Select Case TotalExpense
                    Case Is <= 1000
                        PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt1, 0, False, DefaultUser, DefaultDate)

                    Case Is <= 10000
                        PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt1, 0, False, DefaultUser, DefaultDate)
                        ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(Dpt1, 114)
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            Dpt2 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                            PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt2, 0, False, DefaultUser, DefaultDate)
                        End If

                    Case Is <= 20000
                        PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt1, 0, False, DefaultUser, DefaultDate)
                        ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(Dpt1, 114)
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            Dpt2 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                            PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt2, 0, False, DefaultUser, DefaultDate)
                        End If
                        ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(IIf(Dpt2 = 0, Dpt1, Dpt2), 115)
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            Dpt2 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                            PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt2, 0, False, DefaultUser, DefaultDate)
                        End If

                    Case Is <= 50000
                        PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt1, 0, False, DefaultUser, DefaultDate)
                        ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(Dpt1, 114)
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            Dpt2 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                            PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt2, 0, False, DefaultUser, DefaultDate)
                        End If
                        ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(IIf(Dpt2 = 0, Dpt1, Dpt2), 115)
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            Dpt2 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                            PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt2, 0, False, DefaultUser, DefaultDate)
                        End If
                        ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(IIf(Dpt2 = 0, Dpt1, Dpt2), 116)
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            Dpt3 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                            PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt2, 0, False, DefaultUser, DefaultDate)
                        End If
                        If TotalExpense > 20000 Then
                            ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(IIf(Dpt3 = 0, Dpt2, Dpt3), 117)
                            If commonFunctions.CheckDataSet(ds2) = True Then
                                Dpt2 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                                If (Dpt2 <> 569 And Dpt3 = 6) Or (Dpt3 <> 6) Then ' Do Not include CEO
                                    PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), IIf(Dpt3 = 0, Dpt2, Dpt3), 0, False, DefaultUser, DefaultDate)
                                End If
                            End If
                        End If

                    Case Is > 50000
                        PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt1, 0, False, DefaultUser, DefaultDate)
                        ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(Dpt1, 114)
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            Dpt2 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                            PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt2, 0, False, DefaultUser, DefaultDate)
                        End If
                        ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(IIf(Dpt2 = 0, Dpt1, Dpt2), 115)
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            Dpt2 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                            PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt2, 0, False, DefaultUser, DefaultDate)
                        End If
                        ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(IIf(Dpt2 = 0, Dpt1, Dpt2), 116)
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            Dpt2 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                            PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt2, 0, False, DefaultUser, DefaultDate)
                        End If
                        ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(IIf(Dpt2 = 0, Dpt1, Dpt2), 117)
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            Dpt2 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                            PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt2, 0, False, DefaultUser, DefaultDate)
                        End If
                        ds2 = PURModule.GetTeamMemberDeptInChargeBySubscription(IIf(Dpt2 = 0, Dpt1, Dpt2), 120)
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            Dpt2 = ds2.Tables(0).Rows(0).Item("DeptInChargeTMID").ToString()
                            PURModule.InsertInternalOrderRequestApproval(ViewState("pIORNo"), Dpt2, 0, False, DefaultUser, DefaultDate)
                        End If
                End Select
            End If
        End If

        ''******************
        ''* Locate Default Buyer
        ''******************
        PURModule.InsertInternalOrderRequestApprovalDefault(ViewState("pIORNo"), ddBuyer.SelectedValue, 99, DefaultUser, DefaultDate)

        Return True
    End Function 'EOF BuildApprovalList

    Protected Sub gvApprovers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApprovers.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the DateNotified label
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
    End Sub 'EOF gvApprovers_RowDataBound

    Protected Sub gvApprovers_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Try
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            Dim row As GridViewRow = gvApprovers.Rows(e.RowIndex)
            If row IsNot Nothing Then

                Dim DefaultTMID As Integer = ViewState("iTeamMemberID") 'HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                Dim DefaultDate As Date = Date.Now

                Dim t As DropDownList = TryCast(row.FindControl("ddStatus"), DropDownList)
                Dim c As TextBox = TryCast(row.FindControl("txtAppComments"), TextBox)
                Dim tm As TextBox = TryCast(row.FindControl("txtTeamMemberID"), TextBox)
                Dim TeamMemberID As Integer = CType(tm.Text, Integer)
                Dim TotalExpense As Decimal = txtTotalExtension.Text
                Dim LinkLocation As String = Nothing

                Dim s As TextBox = TryCast(row.FindControl("hfSeqNo"), TextBox)
                Dim hfSeqNo As Integer = CType(s.Text, Integer) 'Row Selected - view Approver Sequence No
                Dim ds As DataSet = New DataSet

                If ViewState("iBuyerID") <> 0 Then
                    If t.SelectedValue = "Approved" And txtPONo.Text = Nothing Then
                        lblReqPONo.Visible = True
                        rfvPONo.Visible = True
                        txtPONo.Visible = True

                        lblErrors.Text = "Purchase Order # is a required field."
                        lblErrors.Visible = True
                        lblReqAppComments.Text = "Purchase Order # is a required field."
                        lblReqAppComments.Visible = True
                        lblReqAppComments.ForeColor = Color.Red
                        MaintainScrollPositionOnPostBack = False
                        Exit Sub
                    End If
                End If

                If (t.Text <> "Pending") Then
                    If (c.Text <> Nothing Or c.Text <> "") Then
                        ds = SecurityModule.GetTeamMember(TeamMemberID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                        Dim ShortName As String = ds.Tables(0).Rows(0).Item("ShortName").ToString()

                        ''********
                        ''* Email sent to the next approvers
                        ''********
                        Dim ds1st As DataSet = New DataSet
                        Dim ds2nd As DataSet = New DataSet
                        Dim dsCC As DataSet = New DataSet
                        Dim dsRej As DataSet = New DataSet
                        Dim EmailTO As String = Nothing
                        Dim EmpName As String = Nothing
                        Dim EmailCC As String = Nothing
                        Dim EmailFrom As String = Nothing
                        Dim SponsSameAs1stLvlAprvr As Boolean = False
                        Dim i As Integer = 0
                        Dim LvlApvlCmplt As Boolean = False

                        Dim CurrentEmpEmail As String = Nothing
                        If ViewState("UGNDB_User_Email") IsNot Nothing Then
                            CurrentEmpEmail = ViewState("UGNDB_User_Email")
                            EmailFrom = CurrentEmpEmail
                            EmailCC = CurrentEmpEmail & ";"
                        Else
                            CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                        End If


                        ''********
                        ''* Only users with valid email accounts can send an email.
                        ''********
                        If CurrentEmpEmail <> Nothing And ViewState("pIORNo") <> Nothing Then
                            If t.SelectedValue = "Rejected" And c.Text = Nothing Then
                                lblErrors.Text = "Your comments is required for Rejection."
                                lblErrors.Visible = True
                                lblReqAppComments.Text = "Your comments is required for Rejection."
                                lblReqAppComments.Visible = True
                                lblReqAppComments.ForeColor = Color.Red
                                Exit Sub
                            Else
                                If TotalExpense = 0 Then
                                    lblErrors.Text = "Unable to submit the IOR with Total Request of $0.00, please Review."
                                    lblErrors.Visible = True
                                    lblReqAppComments.Text = "Unable to submit the IOR with Total Request of $0.00, please Review."
                                    lblReqAppComments.Visible = True
                                    lblReqAppComments.ForeColor = Color.Red
                                    MaintainScrollPositionOnPostBack = False
                                    Exit Sub
                                End If

                                ''***********
                                ''* Verify that Row selected Team Member Sequence No is Last to Approve
                                ''***********
                                Dim dsLast As DataSet = New DataSet
                                Dim r As Integer = 0
                                Dim LastSeqNo As Boolean = False
                                Dim totalApprovers As Integer = 0

                                dsLast = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), 0, 0, False, False)
                                If commonFunctions.CheckDataSet(dsLast) = True Then
                                    For r = 0 To dsLast.Tables.Item(0).Rows.Count - 1
                                        totalApprovers = dsLast.Tables(0).Rows(r).Item("SeqNo") ''totalApprovers + 1
                                    Next
                                End If
                                If totalApprovers = hfSeqNo Then
                                    LastSeqNo = True
                                Else
                                    LastSeqNo = False
                                End If

                                ''**********************
                                ''* Update Record
                                ''**********************
                                UpdateRecord(IIf(LastSeqNo = True, IIf(t.SelectedValue = "Rejected", "In Process", "Approved"), "In Process"), IIf(LastSeqNo = True, IIf(t.SelectedValue = "Rejected", "R", "A"), IIf(t.SelectedValue = "Rejected", "R", "T")), False)
                                ''*****************
                                ''History Tracking
                                ''*****************
                                PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), txtVendorName.Text, DefaultTMID, (DefaultUser.ToUpper) & " " & t.Text & " this record on behalf of " & ShortName.ToUpper & " - Comments: " & c.Text)

                                ''***********************************
                                ''Update Current Level Approver record.
                                ''***********************************
                                PURModule.UpdateInternalOrderRequestApproval(ViewState("pIORNo"), TeamMemberID, True, t.SelectedValue, c.Text, hfSeqNo, 0, DefaultTMID, DefaultUser, DefaultDate)

                                If LastSeqNo = True Then 'Last Team Member in the Approval Chain
                                    ''************************
                                    ''* Update Internal_Order_Request record
                                    '*************************
                                    PURModule.UpdateInternalOrderRequestStatus(ViewState("pIORNo"), IIf(t.SelectedValue = "Rejected", "In Process", "Approved"), IIf(t.SelectedValue = "Rejected", "R", "A"), txtPONo.Text, DefaultUser, DefaultDate)
                                End If 'EOF If LastSeqNo = True Then 'Last Team Member

                                ''*******************************
                                ''Locate Next Approver
                                ''*******************************
                                ''Check at same sequence level
                                ds1st = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), hfSeqNo, TeamMemberID, True, False)
                                If commonFunctions.CheckDataSet(ds1st) = False Then
                                    If t.SelectedValue <> "Rejected" And LastSeqNo = False Then
                                        ds2nd = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), IIf(LastSeqNo = False, (hfSeqNo + 1), hfSeqNo), 0, True, False)
                                        If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                                            For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                                If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                                (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                                                (ddRequestedBy.SelectedValue <> ds2nd.Tables(0).Rows(i).Item("TeamMemberID")) Then
                                                    EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                                    EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                                    ''*****************************************
                                                    ''Update Next level Approvers DateNotified field.
                                                    ''*****************************************
                                                    PURModule.UpdateInternalOrderRequestApproval(ViewState("pIORNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", IIf(LastSeqNo = False, (hfSeqNo + 1), hfSeqNo), 0, 0, DefaultUser, DefaultDate)
                                                End If
                                            Next
                                        End If 'EOF ds2nd.Tables.Count > 0 
                                    End If 'EOF  If t.SelectedValue <> "Rejected" And LastSeqNo = False Then

                                    'Rejected or last approval
                                    If t.SelectedValue = "Rejected" Or (LastSeqNo = True And t.SelectedValue = "Approved") Then
                                        ''********************************************************
                                        ''Notify Submitter if Rejected or last approval
                                        ''********************************************************
                                        dsRej = SecurityModule.GetTeamMember(txtSubmittedByTMID.Text, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                                        ''Check that the recipient(s) is a valid Team Member
                                        If dsRej.Tables.Count > 0 And (dsRej.Tables.Item(0).Rows.Count > 0) Then
                                            For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                                If (dsRej.Tables(0).Rows(i).Item("Working") = True) Then

                                                    EmailTO &= dsRej.Tables(0).Rows(i).Item("Email") & ";"
                                                    EmpName &= dsRej.Tables(0).Rows(i).Item("FirstName") & " " & dsRej.Tables(0).Rows(i).Item("LastName") & ", "
                                                End If
                                            Next
                                        End If 'EOF If dsRej.Tables.Count > 0.....
                                    End If 'EOF t.SelectedValue = "Rejected"
                                End If 'EOF ds1st.Tables.Count > 0

                                ''********************************************************
                                ''Send Notification only if there is a valid Email Address
                                ''********************************************************
                                If EmailTO <> Nothing Then
                                    Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                                    Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                                    Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                                    ''******************************************************
                                    '' Notify all involved
                                    ''*******************************************************
                                    If t.SelectedValue = "Rejected" Or (LastSeqNo = True And t.SelectedValue = "Approved") Then
                                        If hfSeqNo = 1 Then
                                            EmailCC = CarbonCopyList(MyMessage, 0, "", 1, 0, EmailCC, DefaultTMID, 0, 0)
                                        ElseIf hfSeqNo = 2 Then
                                            EmailCC = CarbonCopyList(MyMessage, 0, "", (hfSeqNo - 1), 0, EmailCC, DefaultTMID, 0, 0)
                                        Else
                                            EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID, 0, 0)
                                        End If

                                        ''************************
                                        ''Notify Requisitioner
                                        ''************************
                                        If ddRequestedBy.SelectedValue <> txtSubmittedByTMID.Text Then
                                            EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID, ddRequestedBy.SelectedValue, 0)
                                        End If 'EOF If ddRequestedBy.SelectedValue <> txtSubmittedByTMID.Text then
                                    End If 'EOF  If LastSeqNo = True And ddStatus.SelectedValue = "Approved" Then

                                    ''Carbon copy "CC List IOR"
                                    EmailCC = CarbonCopyList(MyMessage, 152, "", 0, TeamMemberID, EmailCC, DefaultTMID, 0, 0)

                                    If txtAppropriation.Text <> Nothing Then
                                        LinkLocation = LinkLocationString()
                                    End If 'EOF  If txtAppropriation.Text <> Nothing Then

                                    ''Test or Production Message display
                                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                                        MyMessage.Subject = "TEST: "
                                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                                    Else
                                        MyMessage.Subject = ""
                                        MyMessage.Body = ""
                                    End If

                                    MyMessage.Subject &= "Internal Order Request - " & txtIORDescription.Text

                                    If t.SelectedValue = "Rejected" Then
                                        MyMessage.Subject &= " - REJECTED"
                                        MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName & "</font>"
                                        MyMessage.Body &= "<br/><br/><font size='2' face='Tahoma'>'" & txtIORDescription.Text & "' was <font color='red'>REJECTED</font>. <a href='" & ViewState("strProdOrTestEnvironment") & "/PUR/InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</font> <br/><br/>Reason for rejection: <font color='red'>" & c.Text & "</font><br/><br/>"
                                    Else
                                        If LastSeqNo = True Then 'If last approval
                                            MyMessage.Subject &= " - APPROVED"
                                            MyMessage.Body &= "<p><font size='2' face='Verdana'>'" & txtIORDescription.Text & "' is Approved. <a href='" & ViewState("strProdOrTestEnvironment") & "/PUR/InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</font></p>"
                                        Else
                                            MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName & "</font>"
                                            MyMessage.Body &= "<p><font size='2' face='Tahoma'>'" & txtIORDescription.Text & "' is available for your Review/Approval. <a href='" & ViewState("strProdOrTestEnvironment") & "/PUR/crInternalOrderRequestApproval.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to access the record.</font></p>"
                                        End If
                                    End If

                                    EmailBody(MyMessage, LinkLocation)


                                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
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
                                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "IOR", ViewState("pIORNo"))
                                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                        lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."
                                    Catch ex As SmtpException
                                        lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                        lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                                        UGNErrorTrapping.InsertEmailQueue("IOR Ref#: " & ViewState("pIORNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                    End Try
                                    lblErrors.Visible = True
                                    lblReqAppComments.Visible = True
                                    lblReqAppComments.ForeColor = Color.Red

                                    ''*****************
                                    ''History Tracking
                                    ''*****************
                                    If t.SelectedValue <> "Rejected" Then
                                        ds2nd = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), IIf(LastSeqNo = False, (hfSeqNo + 1), hfSeqNo), 0, True, False)
                                        If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                                            PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to level " & IIf(LastSeqNo = False, (hfSeqNo + 1), hfSeqNo) & " approver(s): " & EmpName)
                                        Else
                                            PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to all involved.")
                                        End If
                                    Else
                                        PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to " & EmpName)
                                    End If

                                End If 'EOF IF EmailTO <> Nothing Then
                            End If 'EOF If ReqFormFound = True Then
                        End If 'EOF If HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value <> Nothing Then

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pIORNo"))
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
                    End If 'EOF  If (c.Text <> Nothing Or c.Text <> "") Then
                End If 'EOF  If (t.Text <> "Pending") Then
            End If 'EOF  If row IsNot Nothing Then
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
#End Region 'EOF Approval Status

#Region "Email Notification"
    Protected Sub btnFwdApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFwdApproval.Click
        Try
            ''********
            ''* This function is used to submit email to the Approvers after the Project Sponsor is done.
            ''********
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = ViewState("DefaultUser")
            Dim DefaultTMID As Integer = ViewState("iTeamMemberID")

            Dim ds1st As DataSet = New DataSet
            Dim ds2nd As DataSet = New DataSet
            Dim dsCC As DataSet = New DataSet
            Dim dsExt As DataSet = New DataSet
            Dim dsSD As DataSet = New DataSet
            Dim EmpName As String = Nothing
            Dim EmailTO As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing
            Dim TotalExpense As Decimal = txtTotalExtension.Text
            Dim SeqNo As Integer = 0

            Dim i As Integer = 0

            Dim CurrentEmpEmail As String = Nothing
            If ViewState("DefaultUserEmail") IsNot Nothing Then
                CurrentEmpEmail = ViewState("DefaultUserEmail")
                EmailFrom = CurrentEmpEmail
                EmailCC = CurrentEmpEmail & ";"
            Else
                CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                EmailFrom = "Database.Notifications@ugnauto.com"
            End If

            Dim ProjectStatus As String = Nothing
            If txtRoutingStatus.Text = "N" Then
                ProjectStatus = ddIORStatus.SelectedValue
            ElseIf txtRoutingStatus.Text = "T" Or txtRoutingStatus.Text = "R" Then
                ProjectStatus = ddIORStatus2.SelectedValue
            End If

            'Rebuild Approval List
            If (txtRoutingStatus.Text = "N") Then
                BuildApprovalList()
            End If

            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pIORNo") <> Nothing Then
                ''***************
                ''Verify that atleast one Extension entry has been made
                ''***************
                Dim ReqFormFound As Boolean = False
                Dim a As Integer = 0
                Dim TotalAmount As Decimal = 0
                dsExt = PURModule.GetInternalOrderRequestExpenditure(ViewState("pIORNo"), 0)
                If commonFunctions.CheckDataSet(dsExt) = True Then 'If missing kick user out from submission.
                    ReqFormFound = True
                    For a = 0 To dsExt.Tables.Item(0).Rows.Count - 1
                        TotalAmount = TotalAmount + dsExt.Tables(0).Rows(a).Item("TotalCost")
                    Next
                Else
                    ReqFormFound = False
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True

                    rfvDescription.IsValid = False
                    rfvQuantity.IsValid = False
                    rfvAmountPer.IsValid = False
                    rfvUOM.IsValid = False
                    vsExtExpense.ShowSummary = True

                    lblErrors.Text = "Atleast one Extension entry is required for submission."
                    lblErrors.Visible = True
                    lblReqAppComments.Text = "Atleast one Extension entry is required for submission."
                    lblReqAppComments.Visible = True
                    lblReqAppComments.ForeColor = Color.Red
                    Exit Sub
                End If 'EOF If commonFunctions.CheckDataSet(dsDoc) = False Then

                ''***************
                ''Verify that atleast one Supporting Document entry has been made
                ''***************
                If txtTotalExtension.Text > 0 Then
                    dsSD = EXPModule.GetExpProjDocuments(txtAppropriation.Text) 'Check for CapEx Documents.
                    If commonFunctions.CheckDataSet(dsSD) = True Then 'If missing check for IOR Supporting Documents
                        ReqFormFound = True
                    Else
                        dsExt = PURModule.GetInternalOrderRequestDocument(ViewState("pIORNo"), 0)
                        If commonFunctions.CheckDataSet(dsExt) = True Then 'If IOR Docs missing alert TM required
                            ReqFormFound = True
                        Else
                            ReqFormFound = False
                            mvTabs.ActiveViewIndex = Int32.Parse(2)
                            mvTabs.GetActiveView()
                            mnuTabs.Items(2).Selected = True

                            lblErrors.Text = "Atleast one Supporting Document is required for submission."
                            lblErrors.Visible = True
                            lblReqAppComments.Text = "Atleast one Supporting Document is required for submission."
                            lblReqAppComments.Visible = True
                            lblReqAppComments.ForeColor = Color.Red
                            MaintainScrollPositionOnPostBack = "false"
                            Exit Sub
                        End If 'EOF If commonFunctions.CheckDataSet(dsDoc) = False Then
                    End If 'EOF  If commonFunctions.CheckDataSet(dsSD) = True Then
                End If 'EOF   If txtAppropriation.Text <> Nothing Then

                If ReqFormFound = True Then
                    ''**********************
                    ''* Save data prior to submission
                    ''**********************
                    UpdateRecord("In Process", "T", True)

                    ''*******************************
                    ''Notify 1st level approver
                    ''*******************************
                    If (txtRoutingStatus.Text <> "R") Then
                        ds1st = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), 1, 0, False, False)
                    Else 'IF Rejected - only notify the TM who Rejected the record
                        ds1st = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), 0, 0, False, True)
                    End If

                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(ds1st) = False Then
                        ds1st = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), 1, 0, False, False)
                    End If
                    If commonFunctions.CheckDataSet(ds1st) = True Then
                        For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                            If (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                            (ddRequestedBy.SelectedValue <> ds1st.Tables(0).Rows(i).Item("OrigTeamMemberID")) Then
                                If (txtRoutingStatus.Text <> "R") Then
                                    EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"
                                    EmpName &= ds1st.Tables(0).Rows(i).Item("EmailTMName") & ", "
                                Else
                                    EmailTO &= ds1st.Tables(0).Rows(i).Item("OrigEmail") & ";"
                                    EmpName &= ds1st.Tables(0).Rows(i).Item("OrigEmailTMName") & ", "
                                End If

                                ''************************************************************
                                ''Update 1st level DateNotified field.
                                ''************************************************************
                                If (txtRoutingStatus.Text <> "R") Then
                                    PURModule.UpdateInternalOrderRequestApproval(ViewState("pIORNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 1, 0, 0, DefaultUser, DefaultDate)
                                Else 'IF Rejected - only notify the TM who Rejected the record
                                    PURModule.UpdateInternalOrderRequestApproval(ViewState("pIORNo"), ds1st.Tables(0).Rows(i).Item("OrigTeamMemberID"), False, "Pending", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), 0, ds1st.Tables(0).Rows(i).Item("OrigTeamMemberID"), DefaultUser, DefaultDate)
                                    SeqNo = ds1st.Tables(0).Rows(i).Item("SeqNo")
                                End If
                            End If 'EOF IF (ds1st.Tables(0).Rows(i).Item("Email") <> Nothing) And 
                        Next 'EOF For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                    End If 'EOF If commonFunctions.CheckDataset(ds1st) = True 

                    ''********************************************************
                    ''Send Notification only if there is a valid Email Address
                    ''********************************************************
                    If EmailTO <> Nothing Then
                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                        Dim LinkLocation As String = Nothing
                        If txtAppropriation.Text <> Nothing Then
                            LinkLocation = LinkLocationString()
                        End If 'EOF  If txtAppropriation.Text <> Nothing Then

                        ''********************************************************
                        ''Notify Requestor if the TM who is forwarding is not the same as the requested by
                        ''********************************************************
                        If DefaultTMID <> ddRequestedBy.SelectedValue Then
                            EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID, ddRequestedBy.SelectedValue, 0)
                        End If

                        ''********************************************************
                        ''Notify SubmittedBy if the TM who is forwarding is not the same as the Submitted by
                        ''********************************************************
                        If DefaultTMID <> txtSubmittedByTMID.Text Then
                            EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID, 0, txtSubmittedByTMID.Text)
                        End If

                        If (txtRoutingStatus.Text = "R") Then
                            If SeqNo = 1 Then
                                EmailCC = CarbonCopyList(MyMessage, 0, "", 1, 0, EmailCC, DefaultTMID, 0, 0)
                            Else
                                EmailCC = CarbonCopyList(MyMessage, 0, "", (SeqNo - 1), 0, EmailCC, DefaultTMID, 0, 0)
                            End If
                        End If

                        ''Test or Production Message display
                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                        End If

                        MyMessage.Subject &= "New Internal Order Request - " & txtIORDescription.Text

                        MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName
                        MyMessage.Body &= "<p>'" & txtIORDescription.Text & "' is available for your Review/Approval. <a href='" & ViewState("strProdOrTestEnvironment") & "/PUR/crInternalOrderRequestApproval.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"

                        If txtReSubmit.Text <> Nothing Then
                            MyMessage.Body &= "<p>Reason for resubmission: <font color='red'>" & txtReSubmit.Text & "</font></p>"
                        End If
                        MyMessage.Body &= "</font>"

                        EmailBody(MyMessage, LinkLocation)

                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                            MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                            EmailTO = CurrentEmpEmail '"lynette.rey@ugnauto.com"
                            EmailCC = "lynette.rey@ugnauto.com"
                        End If

                        ''*****************
                        ''History Tracking
                        ''*****************
                        If txtReSubmit.Text = Nothing Then
                            PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), txtVendorName.Text, DefaultTMID, "Record completed and forwarded to " & EmpName & " for approval.")
                        Else
                            PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), txtVendorName.Text, DefaultTMID, "Record resubmitted to " & EmpName & " for approval. - Reason: " & txtReSubmit.Text)
                        End If

                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "IOR", ViewState("pIORNo"))
                            lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                            lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."
                        Catch ex As SmtpException
                            lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                            lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                            UGNErrorTrapping.InsertEmailQueue("IOR Ref#: " & ViewState("pIORNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                        End Try
                        lblErrors.Visible = True
                        lblReqAppComments.Visible = True

                        lblReqAppComments.ForeColor = Color.Red
                    End If 'EOF  If EmailTO <> Nothing Then

                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pIORNo"))
                    gvApprovers.DataBind()

                    ''*************************************************
                    '' "Form Level Security using Roles &/or Subscriptions"
                    ''*************************************************
                    CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                End If 'EOF IF ReqFormFound = True Then
            End If 'EOF If ViewState("pIORNo") <> Nothing Then

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF  btnFwdApproval_Click

    Public Function LinkLocationString() As String
        Dim LinkLocation As String = Nothing
        Select Case txtAppropriation.Text.Substring(0, 1)
            Case "A"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjAssets.aspx?pProjNo=" & txtAppropriation.Text & "' target='_blank'>" & txtAppropriation.Text & " - " & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtAppropriation.Text
                End If
            Case "D"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjDevelopment.aspx?pProjNo=" & txtAppropriation.Text & "' target='_blank'>" & txtAppropriation.Text & " - " & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtAppropriation.Text
                End If
            Case "P"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjPackaging.aspx?pProjNo=" & txtAppropriation.Text & "' target='_blank'>" & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtAppropriation.Text
                End If
            Case "R"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjRepair.aspx?pProjNo=" & txtAppropriation.Text & "' target='_blank'>" & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtAppropriation.Text
                End If
            Case "T"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjTooling.aspx?pProjNo=" & txtAppropriation.Text & "' target='_blank'>" & txtAppropriation.Text & " - " & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtAppropriation.Text
                End If
        End Select

        Return LinkLocation

    End Function 'EOF LinkLocationString

    Public Function EmailBody(ByVal MyMessage As MailMessage, ByVal LinkLocation As String) As String

        MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width:800px; font-size: 13; font-family: Tahoma;'>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>INTERNAL ORDER REQUEST OVERVIEW</strong></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' style='width:70px;' >Reference No:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td  >" & ViewState("pIORNo") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Requestor:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddRequestedBy.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtUGNLocation.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Department/Cost Center:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddDepartment.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>G/L Account #:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddGLAccount.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Expected Delivery Date:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtExptdDeliveryDate.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Vendor:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddVendor.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Ship To:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddShipTo.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        If txtAppropriation.Text <> Nothing Then
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right'>Appropriation No.:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td>" & LinkLocation & "</td>"
            MyMessage.Body &= "</tr>"
            If txtAppropriation.Text.Substring(0, 1) = "T" Then
                Dim RevAdapter As New ExpProjTableAdapters.ExpProj_Tooling_Customer_EIOR_TableAdapter
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td class='p_text' align='center'></td>"
                MyMessage.Body &= "<td><table style='border: 1px solid #D0D0BF; font-size: 13; font-family: Tahoma;'> "
                MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:white;'>"
                MyMessage.Body &= "<td><strong>Part Number</strong></td>"
                MyMessage.Body &= "<td><strong>Revision Level</strong></td>"
                MyMessage.Body &= "</tr>"
                For Each row As DataRow In RevAdapter.Get_ExpProj_Tooling_Customer_EIOR(txtAppropriation.Text)
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td class='p_text' align='left'>" & row("PartNo").ToString() & "</td>"
                    MyMessage.Body &= "<td align='left'>" & row("RevisionLevel").ToString() & "</td>"
                    MyMessage.Body &= "</tr>"
                Next
                MyMessage.Body &= "</table></td>"
                MyMessage.Body &= "</tr>"

                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td >&nbsp;&nbsp;</td>"
                MyMessage.Body &= "<td class='p_text' align='left'><strong>Revision Level History</strong></td>"
                MyMessage.Body &= "</tr>"
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td >&nbsp;&nbsp;</td>"
                MyMessage.Body &= "<td><table style='border: 1px solid #D0D0BF; font-size: 11; font-family: Tahoma;'> "
                MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:white;'>"
                MyMessage.Body &= "<td><strong>Action Date</strong></td>"
                MyMessage.Body &= "<td><strong>Action Taken By</strong></td>"
                MyMessage.Body &= "<td><strong>Action Description</strong></td>"
                MyMessage.Body &= "</tr>"
                Dim ds1 As DataSet = New DataSet
                ds1 = PURModule.GetInternalOrderRequestHistory(ViewState("pIORNo"))
                If commonFunctions.CheckDataSet(ds1) = True Then
                    For i = 0 To ds1.Tables.Item(0).Rows.Count - 1
                        If ds1.Tables(0).Rows(i).Item("IORDescription") = "" _
                        Or ds1.Tables(0).Rows(i).Item("IORDescription") = Nothing Then
                            MyMessage.Body &= "<tr><td>" & ds1.Tables(0).Rows(i).Item("ActionDate")
                            MyMessage.Body &= "</td><td>" & ds1.Tables(0).Rows(i).Item("ActionTakenBy")
                            MyMessage.Body &= "</td><td>" & ds1.Tables(0).Rows(i).Item("ActionDesc") & "</td></tr>"
                        End If
                    Next
                End If
                MyMessage.Body &= "</table></td>"
                MyMessage.Body &= "</tr>"

            End If 'EOF  If AppropriationCode <> Nothing Then
        End If
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Total Amount Requested ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtTotalExtension.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Create PO in:&nbsp;&nbsp; </td>"
        If ddPOinPesos.SelectedValue = True Then
            MyMessage.Body &= "<td><font color='red'><strong>MXN Pesos</strong></font></td>"
        Else
            MyMessage.Body &= "<td>USD</td>"
        End If
        MyMessage.Body &= "</tr>"
        ''***************************************************
        ''Get list of Supporting Documentation
        ''***************************************************
        Dim dsAED As DataSet
        dsAED = PURModule.GetInternalOrderRequestDocument(ViewState("pIORNo"), 0)
        If commonFunctions.CheckDataSet(dsAED) = True Then
            MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
            MyMessage.Body &= "<td colspan='2'><strong>SUPPORTING DOCUMENTS:</strong></td>"
            MyMessage.Body &= "</tr>"
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td colspan='2'>"
            MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "   <td width='250px'><b>Form Description</b></td>"
            MyMessage.Body &= "   <td width='250px'>&nbsp;</td>"
            MyMessage.Body &= "</tr>"
            For i = 0 To dsAED.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td height='25'>" & dsAED.Tables(0).Rows(i).Item("Description") & "</td>"
                MyMessage.Body &= "<td height='25'><a href='" & ViewState("strProdOrTestEnvironment") & "/PUR/InternalOrderRequestDocument.aspx?pIORNo=" & ViewState("pIORNo") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
                MyMessage.Body &= "</tr>"
            Next
            MyMessage.Body &= "</table>"
            MyMessage.Body &= "</tr>"
        End If
        MyMessage.Body &= "</table>"

        Return True

    End Function 'EOF EmailBody()


    Public Function CarbonCopyList(ByVal MyMessage As MailMessage, ByVal SubscriptionID As Integer, ByVal UGNLoc As String, ByVal SeqNo As Integer, ByVal RejectedTMID As Integer, ByVal EmailCC As String, ByVal DefaultTMID As Integer, ByVal RequestdByTMID As Integer, ByVal SubmittedByTMID As Integer) As String
        Try
            Dim dsCC As DataSet = New DataSet
            Dim IncludeOrigAprvlTM As Boolean = False
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            If RequestdByTMID <> 0 Then
                dsCC = SecurityModule.GetTeamMember(RequestdByTMID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Working") = True) Then
                            EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"
                        End If
                    Next
                End If
            End If

            If SubmittedByTMID <> 0 Then
                dsCC = SecurityModule.GetTeamMember(SubmittedByTMID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Working") = True) Then
                            EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"
                        End If
                    Next
                End If
            End If

            If SeqNo = 0 And SubmittedByTMID = 0 And RequestdByTMID = 0 Then
                dsCC = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), 0, 0, False, False)
                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("TeamMemberID") <> DefaultTMID) And _
                        (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                            EmailCC &= (dsCC.Tables(0).Rows(i).Item("Email") & ";")
                        End If
                    Next
                End If
            End If

            If SeqNo <> 0 Then  'Notify same level approvers after a rejection has been released 
                dsCC = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), SeqNo, 0, False, False)
                'Carbon Copy pending approvers at same level as who rejected the record.
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                        (RejectedTMID <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) Then
                            EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"
                        End If
                    Next
                End If
                IncludeOrigAprvlTM = True
            End If

            If IncludeOrigAprvlTM = True Then
                dsCC = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), 0, 0, False, False)
                'CC Orgiginal Approver in an event that their a corporate calendar default responsibility
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                        dsCC.Tables(0).Rows(i).Item("OrigEmail") <> dsCC.Tables(0).Rows(i).Item("Email") Then
                            EmailCC &= dsCC.Tables(0).Rows(i).Item("OrigEmail") & ";"
                        End If
                    Next
                End If
            End If

            If SubscriptionID = 152 Then
                ''Notify CC List
                dsCC = commonFunctions.GetTeamMemberBySubscription(SubscriptionID)
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("TMID") <> DefaultTMID) And (dsCC.Tables(0).Rows(i).Item("TMID") <> RejectedTMID) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                            EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"
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


    Public Sub SendNotifWhenEventChanges(ByVal EventDesc As String)
        ''********
        ''* This section will be used in the following methods:
        ''*     1) Email sent to all involved when an Asset is Tooling Completed
        ''*     2) Email sent to all involved with an Asset is VOID
        ''*     3) Email sent to Account with an Asset is COMPLETED
        ''********188 371 510 569
        Dim DefaultDate As Date = Date.Now
        Dim DefaultUser As String = ViewState("DefaultUser")
        Dim DefaultTMID As Integer = ViewState("iTeamMemberID")
        Dim DefaultUserName As String = ViewState("DefaultUserFullName")

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
        If ViewState("DefaultUserEmail") IsNot Nothing Then
            CurrentEmpEmail = ViewState("DefaultUserEmail")
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
            If CurrentEmpEmail <> Nothing Then
                ''Is this for a Group Notification or Individual
                Select Case EventDesc
                    Case "Void" 'Sent by Project Leader, notify all
                        GroupNotif = True
                    Case "Completed" 'Sent by the Buyer, notify initiator
                        GroupNotif = False
                End Select

                If GroupNotif = True Then
                    ''*********************************************************
                    ''*Notify Approvers
                    ''*********************************************************
                    ds1st = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), 0, 0, False, False)
                    ''Check that the recipient(s) is a valid Team Member
                    If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                        For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                            If ((ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Or _
                                (ds1st.Tables(0).Rows(i).Item("Email") <> EmailCC)) And _
                                (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"

                            End If
                        Next
                    End If 'Notify Approvers
                Else 'GroupNotif = False
                    ''********************************************************
                    ''Notify Submitter
                    ''********************************************************
                    ds1st = SecurityModule.GetTeamMember(txtSubmittedByTMID.Text, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                    ''Check that the recipient(s) is a valid Team Member
                    If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                        For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                            If (ds1st.Tables(0).Rows(i).Item("Working") = True) And _
                            (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then

                                EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"

                            End If
                        Next
                    End If 'EOF If dsRej.Tables.Count > 0.....
                End If 'EOF GroupNotif = True

                ''********************************************************
                ''Send Notification only if there is a valid Email Address
                ''********************************************************
                If EmailTO <> Nothing Then
                    Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                    ''************************
                    ''Notify Requisitioner
                    ''************************
                    If ddRequestedBy.SelectedValue <> txtSubmittedByTMID.Text Then
                        EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID, ddRequestedBy.SelectedValue, 0)
                    End If 'EOF If ddRequestedBy.SelectedValue <> txtSubmittedByTMID.Text then

                    'Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                    Else
                        MyMessage.Subject = ""
                    End If

                    MyMessage.Subject &= "Internal Order Request: " & txtIORDescription.Text & " - " & EventDesc

                    MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"

                    MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>Internal Order Request '" & EventDesc.ToUpper & "' by " & DefaultUserName & ".</strong></td>"

                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td class='p_text' align='right' style='width: 150px;'>Reference No:&nbsp;&nbsp; </td>"
                    MyMessage.Body &= "<td> <a href='" & ViewState("strProdOrTestEnvironment") & "/PUR/InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "'>" & ViewState("pIORNo") & "</a></td>"
                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td class='p_text' align='right'>Description:&nbsp;&nbsp; </td>"
                    MyMessage.Body &= "<td>" & txtIORDescription.Text & "</td>"
                    MyMessage.Body &= "</tr>"

                    Select Case EventDesc
                        Case "Void" 'Sent by Project Leader, notify all
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Void Reason:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td style='width: 600px;'>" & txtVoidReason.Text & "</td>"
                            MyMessage.Body &= "</tr>"
                        Case "Completed" 'Sent by the Buyer to Initiator
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Purchase Order #:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td style='width: 600px;'>" & txtPONo.Text & "</td>"
                            MyMessage.Body &= "</tr>"
                    End Select
                    MyMessage.Body &= "</table>"

                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
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
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "IOR", ViewState("pIORNo"))
                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                    Catch ex As SmtpException
                        lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                        UGNErrorTrapping.InsertEmailQueue("IOR Ref#: " & ViewState("pIORNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                    End Try
                    lblErrors.Visible = True

                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pIORNo"))
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
#End Region 'EOF Email Notification

#Region "Communication Board"
    Protected Sub btnRSS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRSS.Click
        Try
            ''************************************
            ''Send response back to requestor in Communication Board
            ''************************************
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = ViewState("DefaultUser")
            Dim DefaultTMID As Integer = ViewState("iTeamMemberID")
            Dim DefaultUserFullName As String = ViewState("DefaultUserFullName")


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
            If ViewState("DefaultUserEmail") IsNot Nothing Then
                CurrentEmpEmail = ViewState("DefaultUserEmail")
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
            If CurrentEmpEmail <> Nothing And ViewState("pIORNo") <> Nothing Then
                ''*************************************************************************
                ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                ''*************************************************************************
                Dim dsExp As DataSet = New DataSet

                ''**********************
                ''*Initialize Variables
                ''**********************
                Dim RequestedBy As Integer = ddRequestedBy.SelectedValue
                Dim IORDescription As String = txtIORDescription.Text

                ''***************************************************************
                ''Send Reply back to requestor
                ''***************************************************************
                ds = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), 0, TMID, False, False)
                ''Check that the recipient(s) is a valid Team Member
                If ds.Tables.Count > 0 And (ds.Tables.Item(0).Rows.Count > 0) Then
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

                    ''********************************************************
                    ''Notify Requestor if the TM who is forwarding is not the same as the requested by
                    ''********************************************************
                    If DefaultTMID <> ddRequestedBy.SelectedValue Then
                        EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID, ddRequestedBy.SelectedValue, 0)
                    End If

                    ''********************************************************
                    ''Notify SubmittedBy if the TM who is forwarding is not the same as the Submitted by
                    ''********************************************************
                    If DefaultTMID <> txtSubmittedByTMID.Text Then
                        EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID, 0, txtSubmittedByTMID.Text)
                    End If

                    ''***************************************************************
                    ''Carbon Copy Previous Levels
                    ''***************************************************************
                    EmailCC = CarbonCopyList(MyMessage, 0, "", SeqNo, 0, EmailCC, DefaultTMID, 0, 0)

                    'Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                    Else
                        MyMessage.Subject = ""
                    End If

                    MyMessage.Subject &= "Internal Order Request: " & ViewState("pIORNo") & " - " & IORDescription & " - MESSAGE RECEIVED"

                    MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
                    MyMessage.Body &= " <tr>"
                    MyMessage.Body &= "     <td valign='top' width='20%'>"
                    MyMessage.Body &= "         <img src='" & ViewState("strProdOrTestEnvironment") & "/images/messanger60.jpg'/>"
                    MyMessage.Body &= "     </td>"
                    MyMessage.Body &= "     <td valign='top'>"
                    MyMessage.Body &= "         <b>Attention:</b> " & EmpName
                    MyMessage.Body &= "             <p><b>" & DefaultUserFullName & "</b> sent a message regarding IOR Ref#"
                    MyMessage.Body &= "         <font color='red'>" & ViewState("pIORNo") & " - " & IORDescription & "</font>."
                    MyMessage.Body &= "         <br/><br/><i>Question:&nbsp;&nbsp;</i><b>" & txtQC.Text & "</b>"
                    MyMessage.Body &= "         <br/><br/><i>Response:&nbsp;&nbsp;</i><b>" & txtReply.Text & "</b><br/><br/>"
                    MyMessage.Body &= "         </p>"
                    MyMessage.Body &= "         <p><a href='" & ViewState("strProdOrTestEnvironment") & "/PUR/crInternalOrderRequestApproval.aspx?pIORNo=" & ViewState("pIORNo") & "&pAprv=1" & "'>Click here</a> to return to the approval page."
                    MyMessage.Body &= "     </td>"
                    MyMessage.Body &= " </tr>"
                    MyMessage.Body &= "<table>"
                    MyMessage.Body &= "<br><br>"


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
                    PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), IORDescription, DefaultTMID, "Message Sent")

                    ''**********************************
                    ''Save Reponse to child table
                    ''**********************************
                    PURModule.InsertInternalOrderRequestRSSReply(ViewState("pIORNo"), ViewState("pRID"), IORDescription, DefaultTMID, txtReply.Text)

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "IOR)", ViewState("pIORNo"))
                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                    Catch ex As Exception
                        lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                        UGNErrorTrapping.InsertEmailQueue("IOR Ref#:" & ViewState("pIORNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
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
                    Response.Redirect("InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pRC=1" & Aprv, False)
                Else 'EmailTO = ''
                    ''**********************************
                    ''Rebind the data to the form
                    ''**********************************
                    txtQC.Text = Nothing
                    txtReply.Text = Nothing
                    BindData(ViewState("pIORNo"))

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
            Dim drRSSID As IOR.Internal_Order_Request_RSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, IOR.Internal_Order_Request_RSSRow)

            If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                RSSID = drRSSID.RSSID
                ' Reference the rpCBRC ObjectDataSource
                Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                ' Set the CategoryID Parameter value
                rpCBRC.SelectParameters("IORNO").DefaultValue = drRSSID.IORNO.ToString()
                rpCBRC.SelectParameters("RSSID").DefaultValue = drRSSID.RSSID.ToString()
            End If
        End If
    End Sub 'EOF gvQuestion_RowDataBound

    Public Function GoToCommunicationBoard(ByVal IORNO As String, ByVal RSSID As String, ByVal ApprovalLevel As Integer, ByVal TeamMemberID As Integer) As String
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If
        Return "InternalOrderRequest.aspx?pIORNo=" & IORNO & "&pProjNo=" & ViewState("pProjNo") & "&pAL=" & ApprovalLevel & "&pTMID=" & TeamMemberID & "&pRID=" & RSSID & "&pRC=1" & Aprv
    End Function 'EOF GoToCommunicationBoard
#End Region 'EOF Communication Board
End Class
