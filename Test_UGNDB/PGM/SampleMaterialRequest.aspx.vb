' ************************************************************************************************
' Name:	SampleMaterialRequest.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 01/18/2013    LRey			Created .Net application
' ************************************************************************************************
Partial Class PGM_SampleMaterialRequest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim a As String = commonFunctions.UserInfo()
            ViewState("TMLoc") = HttpContext.Current.Session("UserFacility")

            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to define the primary record
            If HttpContext.Current.Request.QueryString("pSMRNo") <> "" _
                And HttpContext.Current.Request.QueryString("pSMRNo") <> Nothing Then
                ViewState("pSMRNo") = HttpContext.Current.Request.QueryString("pSMRNo")
            Else
                ViewState("pSMRNo") = 0
            End If

            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pAprv") <> "" Then
                ViewState("pAprv") = HttpContext.Current.Request.QueryString("pAprv")
            Else
                ViewState("pAprv") = 0
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

            ''Used to allow TM(s) to Communicated with Approvers for Q&A
            If HttpContext.Current.Request.QueryString("pCP") <> "" Then
                ViewState("pCP") = HttpContext.Current.Request.QueryString("pCP")
            Else
                ViewState("pCP") = 0
            End If


            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            If ViewState("pSMRNo") = Nothing Then
                m.ContentLabel = "New Sample Material Request"
            Else
                m.ContentLabel = "Sample Material Request"
            End If

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pSMRNo") = Nothing Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='SampleMaterialRequestList.aspx'><b>Sample Material Request Search</b></a> > New Sample Material Request"
                Else
                    If ViewState("pAprv") = 0 Then
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='SampleMaterialRequestList.aspx'><b>Sample Material Request Search</b></a> > Sample Material Request"
                    Else 'Go Back To approval Screen
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='SampleMaterialRequestList.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pAprv=1'><b>Sample Material Request Search</b></a> > <a href='crSampleMtrlReqApproval.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pAprv=1'><b>Approval</b></a> > Sample Material Request"
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
            txtToday.Text = Date.Today
            If Not Page.IsPostBack Then
                ''****************************************
                ''Redirect user to the right tab location
                ''****************************************
                If ViewState("pSMRNo") <> 0 Then
                    BindCriteria()
                    BindData(ViewState("pSMRNo"), 0)
                Else
                    If ViewState("pCP") <> 0 Then
                        BindData(0, ViewState("pCP"))
                    End If
                    CheckRights() '"Form Level Security using Roles &/or Subscriptions"
                    BindCriteria()
                    txtSampleDesc.Focus()
                End If

                If ViewState("pRID") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(4)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(4).Selected = True
                ElseIf ViewState("pRC") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(4)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(4).Selected = True
                End If
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            ''*************************************************
            '' Initialize maxlength
            ''*************************************************
            txtSampleDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtSampleDesc.Attributes.Add("onkeyup", "return tbCount(" + lblSampleDescrChar.ClientID + ");")
            txtSampleDesc.Attributes.Add("maxLength", "50")

            txtSpecInst.Attributes.Add("onkeypress", "return tbLimit();")
            txtSpecInst.Attributes.Add("onkeyup", "return tbCount(" + lblSpecInstChar.ClientID + ");")
            txtSpecInst.Attributes.Add("maxLength", "300")

            txtFileDesc1.Attributes.Add("onkeypress", "return tbLimit();")
            txtFileDesc1.Attributes.Add("onkeyup", "return tbCount(" + lblFileDescChar1.ClientID + ");")
            txtFileDesc1.Attributes.Add("maxLength", "200")

            txtFileDesc2.Attributes.Add("onkeypress", "return tbLimit();")
            txtFileDesc2.Attributes.Add("onkeyup", "return tbCount(" + lblFileDescChar2.ClientID + ");")
            txtFileDesc2.Attributes.Add("maxLength", "200")

            txtFileDesc3.Attributes.Add("onkeypress", "return tbLimit();")
            txtFileDesc3.Attributes.Add("onkeyup", "return tbCount(" + lblFileDescChar3.ClientID + ");")
            txtFileDesc3.Attributes.Add("maxLength", "200")

            txtFileDesc4.Attributes.Add("onkeypress", "return tbLimit();")
            txtFileDesc4.Attributes.Add("onkeyup", "return tbCount(" + lblFileDescChar4.ClientID + ");")
            txtFileDesc4.Attributes.Add("maxLength", "200")

            txtFileDesc5.Attributes.Add("onkeypress", "return tbLimit();")
            txtFileDesc5.Attributes.Add("onkeyup", "return tbCount(" + lblFileDescChar5.ClientID + ");")
            txtFileDesc5.Attributes.Add("maxLength", "200")

            txtFileDesc6.Attributes.Add("onkeypress", "return tbLimit();")
            txtFileDesc6.Attributes.Add("onkeyup", "return tbCount(" + lblFileDescChar6.ClientID + ");")
            txtFileDesc6.Attributes.Add("maxLength", "200")

            txtPkgReq.Attributes.Add("onkeypress", "return tbLimit();")
            txtPkgReq.Attributes.Add("onkeyup", "return tbCount(" + lblPkgReqChar.ClientID + ");")
            txtPkgReq.Attributes.Add("maxLength", "300")

            txtLblReqComments.Attributes.Add("onkeypress", "return tbLimit();")
            txtLblReqComments.Attributes.Add("onkeyup", "return tbCount(" + lblLblReqCommentsChar.ClientID + ");")
            txtLblReqComments.Attributes.Add("maxLength", "300")

            txtReply.Attributes.Add("onkeypress", "return tbLimit();")
            txtReply.Attributes.Add("onkeyup", "return tbCount(" + lblReply.ClientID + ");")
            txtReply.Attributes.Add("maxLength", "300")

            txtShippingComments.Attributes.Add("onkeypress", "return tbLimit();")
            txtShippingComments.Attributes.Add("onkeyup", "return tbCount(" + lblShippingCommentsChar.ClientID + ");")
            txtShippingComments.Attributes.Add("maxLength", "300")

            txtVoidReason.Attributes.Add("onkeypress", "return tbLimit();")
            txtVoidReason.Attributes.Add("onkeyup", "return tbCount(" + lblVoidReasonChar.ClientID + ");")
            txtVoidReason.Attributes.Add("maxLength", "300")

            Dim strPreviewClientScript As String = "javascript:void(window.open('crViewSampleMtrlReq.aspx?pSMRNo=" & ViewState("pSMRNo") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
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
#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub DisableControls()
        ViewState("Admin") = False
        ViewState("ObjectRole") = False

        mnuTabs.Items(1).Enabled = False
        mnuTabs.Items(2).Enabled = False
        mnuTabs.Items(3).Enabled = False
        mnuTabs.Items(4).Enabled = False

        ddRecStatus.Enabled = False
        ddRecStatus2.Enabled = False
        hplkAppropriation.Visible = False

        btnAdd.Enabled = False
        btnCopy.Enabled = False
        btnSave1.Enabled = False
        btnReset1.Enabled = False
        btnDelete.Enabled = False
        btnPreview.Enabled = False
        btnFwdApproval.Enabled = False
        btnBuildApproval.Enabled = False
        btnSaveCB.Enabled = False
        btnResetCB.Enabled = False
        btnSubmitCmplt.Enabled = False

        txtVoidReason.Visible = False
        lblVoidReason.Visible = False
        rfvVoidReason.Enabled = False
        lblReqVoidRsn.Visible = False

        Panel1.Enabled = False
        gvPartNo.Visible = False
        gvPartNo.ShowFooter = False
        gvPartNo.Columns(4).Visible = False
        gvPartNo.Columns(5).Visible = False
        gvPartNo.Columns(6).Visible = False
        gvPartNo.Columns(8).Visible = False
        gvPartNo.Columns(9).Visible = False

        gvApprovers.ShowFooter = False
        gvApprovers.Columns(7).Visible = False
        gvApprovers.Columns(8).Visible = False
        gvApprovers.Columns(9).Visible = False

        gvShipping.ShowFooter = False
        gvShipping.Columns(2).Visible = False
        gvShipping.Columns(4).Visible = False
        gvShipping.Columns(5).Visible = False

        uploadFileAddtlDocs.Enabled = False
        uploadFileDeliveryInst.Enabled = False
        uploadFileInvInfo.Enabled = False
        uploadFileLblReq.Enabled = False
        uploadFilePkgReq.Enabled = False
        uploadFileShipDoc.Enabled = False

        btnUploadAddtlDocs.Enabled = False
        btnUploadDeliveryInst.Enabled = False
        btnUploadInvInfo.Enabled = False
        btnUploadLblReq.Enabled = False
        btnUploadPkgReq.Enabled = False
        btnUploadShipDoc.Enabled = False

        btnResetAddtlDocs.Enabled = False
        btnResetDeliveryInst.Enabled = False
        btnResetInvInfo.Enabled = False
        btnResetLblReq.Enabled = False
        btnResetPkgReq.Enabled = False
        btnResetShipDoc.Enabled = False

        gvAddtlDocs.Columns(4).Visible = False
        gvDeliveryInst.Columns(4).Visible = False
        gvInvInfo.Columns(4).Visible = False
        gvPkgReq.Columns(4).Visible = False
        gvLblReq.Columns(4).Visible = False
        gvShipDocs.Columns(4).Visible = False
    End Sub 'EOF DisableControls()
    Protected Sub EnableGVControls()
        uploadFileAddtlDocs.Enabled = True
        uploadFileDeliveryInst.Enabled = True
        uploadFileInvInfo.Enabled = True
        uploadFileLblReq.Enabled = True
        uploadFilePkgReq.Enabled = True
        uploadFileShipDoc.Enabled = True

        btnUploadAddtlDocs.Enabled = True
        btnUploadDeliveryInst.Enabled = True
        btnUploadInvInfo.Enabled = True
        btnUploadLblReq.Enabled = True
        btnUploadPkgReq.Enabled = True
        btnUploadShipDoc.Enabled = True

        btnResetAddtlDocs.Enabled = True
        btnResetDeliveryInst.Enabled = True
        btnResetInvInfo.Enabled = True
        btnResetLblReq.Enabled = True
        btnResetPkgReq.Enabled = True
        btnResetShipDoc.Enabled = True

        gvAddtlDocs.Columns(4).Visible = True
        gvDeliveryInst.Columns(4).Visible = True
        gvInvInfo.Columns(4).Visible = True
        gvPkgReq.Columns(4).Visible = True
        gvLblReq.Columns(4).Visible = True
        gvShipDocs.Columns(4).Visible = True

    End Sub 'EOF EnableGVControls
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        Try
            ''*******
            '' Disable controls by default
            ''*******
            DisableControls()

            ''** Define Record Status
            Dim RecStatus As String = Nothing
            Select Case txtRoutingStatus.Text
                Case "N"
                    RecStatus = ddRecStatus.SelectedValue
                    ddRecStatus.Visible = True
                    ddRecStatus2.Visible = False
                Case "C"
                    RecStatus = ddRecStatus.SelectedValue
                    ddRecStatus.Visible = True
                    ddRecStatus2.Visible = False
                Case "R"
                    RecStatus = ddRecStatus2.SelectedValue
                    ddRecStatus.Visible = False
                    ddRecStatus2.Visible = True
                Case "T"
                    RecStatus = ddRecStatus2.SelectedValue
                    ddRecStatus.Visible = False
                    ddRecStatus2.Visible = True
                Case "V"
                    RecStatus = ddRecStatus.SelectedValue
                    ddRecStatus.Visible = True
                    ddRecStatus2.Visible = False
                Case Else
                    ddRecStatus.Visible = True
                    ddRecStatus2.Visible = False
            End Select

            ViewState("RecStatus") = RecStatus


            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iTMEmail As String = ""
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 135 'Sample Material Request Form ID
            Dim iRoleID As Integer = 0
            Dim i As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            'dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Brenda.Baisden", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iTMEmail = dsTeamMember.Tables(0).Rows(0).Item("Email")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")

                    ViewState("iTeamMemberID") = iTeamMemberID
                    ViewState("DefaultUserEmail") = iTMEmail

                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member

                        ''Locate the Shipping/EDI Coordinator to grant access Shipping Info
                        Dim dsSE As DataSet = New DataSet
                        dsSE = commonFunctions.GetTeamMemberBySubscription(147)
                        Dim iSEID As Integer = 0
                        Dim b As Integer = 0
                        ViewState("iSETMID") = 0
                        If (dsSE.Tables.Item(0).Rows.Count > 0) Then
                            For b = 0 To dsSE.Tables(0).Rows.Count - 1
                                If dsSE.Tables(0).Rows(b).Item("TMID") = iTeamMemberID Then
                                    iSEID = dsSE.Tables(0).Rows(b).Item("TMID")
                                    ViewState("iSETMID") = iSEID
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
                                            ViewState("Admin") = True
                                            ViewState("ObjectRole") = True
                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pSMRNo") = 0 Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                            Else
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                mnuTabs.Items(4).Enabled = True
                                                Select Case RecStatus
                                                    Case "Open"
                                                        btnFwdApproval.Enabled = True
                                                    Case "In Process"
                                                        ddRecStatus2.Enabled = True
                                                        gvApprovers.Columns(7).Visible = True
                                                        If ddShipEDICoord.SelectedValue = Nothing Then
                                                            gvApprovers.ShowFooter = True
                                                            gvApprovers.Columns(8).Visible = True
                                                        End If
                                                        If txtRoutingStatus.Text = "R" Then
                                                            btnFwdApproval.Enabled = True
                                                        End If
                                                    Case "Void"
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidReason.Visible = True
                                                        rfvVoidReason.Enabled = True
                                                        lblReqVoidRsn.Visible = True
                                                End Select
                                                btnDelete.Enabled = True
                                                btnPreview.Enabled = True
                                                Panel1.Enabled = True
                                                gvPartNo.Visible = True
                                                btnResetCB.Enabled = True
                                                btnSaveCB.Enabled = True
                                                EnableGVControls()
                                            End If
                                            btnAdd.Enabled = True
                                            btnCopy.Enabled = True
                                            btnSave1.Enabled = True
                                            btnReset1.Enabled = True
                                            btnBuildApproval.Enabled = True
                                            btnFwdApproval.Enabled = True
                                            btnSubmitCmplt.Enabled = True
                                            gvPartNo.ShowFooter = True
                                            gvPartNo.Columns(4).Visible = True
                                            gvPartNo.Columns(5).Visible = True
                                            gvPartNo.Columns(6).Visible = True
                                            gvPartNo.Columns(8).Visible = True
                                            gvPartNo.Columns(9).Visible = True
                                            gvShipping.ShowFooter = True
                                            gvShipping.Columns(2).Visible = True
                                            gvShipping.Columns(4).Visible = True
                                            gvShipping.Columns(5).Visible = True

                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Sales/Program Mgrs, Tooling/Purchasing Leads
                                            ViewState("ObjectRole") = True
                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pSMRNo") = 0 Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                gvPartNo.Enabled = False
                                            Else
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                ViewState("Admin") = True
                                                Select Case RecStatus
                                                    Case "Open"
                                                        btnFwdApproval.Enabled = True
                                                        btnBuildApproval.Enabled = True
                                                    Case "In Process"
                                                        ddRecStatus2.Enabled = True
                                                        mnuTabs.Items(3).Enabled = True
                                                        mnuTabs.Items(4).Enabled = True
                                                        btnResetCB.Enabled = True
                                                        btnSaveCB.Enabled = True
                                                        If txtRoutingStatus.Text = "R" Then
                                                            btnFwdApproval.Enabled = True
                                                        End If
                                                    Case "Completed"
                                                        mnuTabs.Items(3).Enabled = True
                                                        mnuTabs.Items(4).Enabled = True
                                                        btnResetCB.Enabled = True
                                                        btnSaveCB.Enabled = True
                                                    Case "Void"
                                                        mnuTabs.Items(3).Enabled = True
                                                        mnuTabs.Items(4).Enabled = True
                                                        btnResetCB.Enabled = True
                                                        btnSaveCB.Enabled = True
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidReason.Visible = True
                                                        lblReqVoidRsn.Visible = True
                                                End Select
                                                btnDelete.Enabled = True
                                                btnAdd.Enabled = True
                                                btnCopy.Enabled = True
                                                btnPreview.Enabled = True
                                                Panel1.Enabled = True
                                                gvPartNo.Visible = True
                                                gvPartNo.ShowFooter = True
                                                gvPartNo.Columns(4).Visible = True
                                                gvPartNo.Columns(5).Visible = True
                                                gvPartNo.Columns(6).Visible = True
                                                gvPartNo.Columns(8).Visible = True
                                                gvPartNo.Columns(9).Visible = True
                                                EnableGVControls()
                                                gvShipping.ShowFooter = True
                                                gvShipping.Columns(2).Visible = True
                                                gvShipping.Columns(4).Visible = True
                                                gvShipping.Columns(5).Visible = True
                                            End If
                                            btnSave1.Enabled = True
                                            btnReset1.Enabled = True

                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            '** Used by Backup persons
                                            If ViewState("pSMRNo") = 0 Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                            Else
                                                ViewState("Admin") = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                Select Case RecStatus
                                                    Case "In Process"
                                                        gvApprovers.Columns(7).Visible = True
                                                        If ddShipEDICoord.SelectedValue = Nothing Then
                                                            gvApprovers.ShowFooter = True
                                                            gvApprovers.Columns(8).Visible = True
                                                        End If
                                                        mnuTabs.Items(3).Enabled = True
                                                        mnuTabs.Items(4).Enabled = True
                                                        btnResetCB.Enabled = True
                                                        btnSaveCB.Enabled = True
                                                    Case "Completed"
                                                        mnuTabs.Items(3).Enabled = True
                                                        mnuTabs.Items(4).Enabled = True
                                                        btnResetCB.Enabled = True
                                                        btnSaveCB.Enabled = True
                                                    Case "Void"
                                                        mnuTabs.Items(3).Enabled = True
                                                        mnuTabs.Items(4).Enabled = True
                                                        btnResetCB.Enabled = True
                                                        btnSaveCB.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidReason.Visible = True
                                                End Select
                                                btnPreview.Enabled = True
                                                Panel1.Enabled = True
                                                gvPartNo.Visible = True
                                                gvPartNo.Columns(4).Visible = True
                                                gvPartNo.Columns(5).Visible = True
                                                gvPartNo.Columns(6).Visible = True
                                                Extender2.Collapsed = True
                                                Extender3.Collapsed = True
                                                Extender4.Collapsed = True
                                                Extender5.Collapsed = True
                                                uploadFileAddtlDocs.Enabled = True
                                                btnUploadAddtlDocs.Enabled = True
                                                btnResetAddtlDocs.Enabled = True
                                                gvAddtlDocs.Columns(4).Visible = True
                                                gvShipping.ShowFooter = True
                                                gvShipping.Columns(2).Visible = True
                                                gvShipping.Columns(4).Visible = True
                                                gvShipping.Columns(5).Visible = True
                                            End If

                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                            mnuTabs.Items(3).Enabled = True
                                            mnuTabs.Items(4).Enabled = True
                                            btnPreview.Enabled = True
                                            txtVoidReason.Visible = True
                                            lblVoidReason.Visible = True
                                            Panel1.Enabled = True
                                            gvPartNo.Visible = True
                                            gvPartNo.Columns(4).Visible = True
                                            gvPartNo.Columns(5).Visible = True
                                            gvPartNo.Columns(6).Visible = True
                                            gvShipping.Columns(2).Visible = True
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ''Used by the Shipping / EDI Coordinator
                                            ViewState("ObjectRole") = True
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                            mnuTabs.Items(3).Enabled = True
                                            mnuTabs.Items(4).Enabled = True
                                            Select Case RecStatus
                                                Case "In Process"
                                                    If ViewState("iSETMID") <> 0 Then
                                                        uploadFileShipDoc.Enabled = True
                                                        btnUploadShipDoc.Enabled = True
                                                        btnResetShipDoc.Enabled = True
                                                        gvShipDocs.Columns(4).Visible = True

                                                        btnSubmitCmplt.Enabled = True
                                                        gvShipping.ShowFooter = True
                                                        gvShipping.Columns(2).Visible = True
                                                        gvShipping.Columns(4).Visible = True
                                                        gvShipping.Columns(5).Visible = True
                                                    End If
                                                Case "Completed"
                                                    If ViewState("iSETMID") <> 0 Then
                                                        btnSubmitCmplt.Enabled = True
                                                        gvShipping.ShowFooter = True
                                                        gvShipping.Columns(2).Visible = True
                                                        gvShipping.Columns(4).Visible = True
                                                        gvShipping.Columns(5).Visible = True
                                                    End If
                                                Case "Void"
                                                    txtVoidReason.Visible = True
                                                    lblVoidReason.Visible = True
                                            End Select
                                            btnPreview.Enabled = True
                                            btnResetCB.Enabled = True
                                            btnSaveCB.Enabled = True
                                            Panel1.Enabled = True
                                            gvPartNo.Visible = True
                                            gvPartNo.Columns(4).Visible = True
                                            gvPartNo.Columns(5).Visible = True
                                            gvPartNo.Columns(6).Visible = True
                                            Extender2.Collapsed = True
                                            Extender3.Collapsed = True
                                            Extender4.Collapsed = True
                                            Extender5.Collapsed = True
                                            uploadFileAddtlDocs.Enabled = True
                                            btnUploadAddtlDocs.Enabled = True
                                            btnResetAddtlDocs.Enabled = True
                                            gvAddtlDocs.Columns(4).Visible = True
                                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)                                            
                                            ''** limited entry **''
                                            txtVoidReason.Visible = True
                                            lblVoidReason.Visible = True
                                            Panel1.Enabled = True
                                            gvPartNo.Visible = True
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

#Region "General - Sample Information"
    Protected Sub mnuTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuTabs.MenuItemClick
        mvTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub 'EOF mnuTabs_MenuItemClick 

    Public Function ClearErrorMsgFlds() As String
        lblErrors.Text = Nothing
        lblErrors.Visible = False
        lblReqAppComments.Text = Nothing
        lblReqAppComments.Visible = False

        lblMessageView1.Text = Nothing
        lblMessageView1.Visible = False

        lblMessageView2.Text = Nothing
        lblMessageView2.Visible = False

        lblMessageView3.Text = Nothing
        lblMessageView3.Visible = False

        lblMessageView4.Text = Nothing
        lblMessageView4.Visible = False

        lblMessageView5.Text = Nothing
        lblMessageView5.Visible = False

        lblMessageView6.Text = Nothing
        lblMessageView6.Visible = False

        Return True
    End Function 'EOF ClearErrorMsgFlds

    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Team Member control for selection criteria for search
            ds = commonFunctions.GetTeamMember("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddRequestor.DataSource = ds
                ddRequestor.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddRequestor.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddRequestor.DataBind()
                ddRequestor.Items.Insert(0, "")
                'ddRequestor.Enabled = False
            End If
            ddRequestor.SelectedValue = ViewState("iTeamMemberID")

            ''bind existing data to drop down Account Manager control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(9) '**SubscriptionID 9 is used for Account Manager
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddAccountManager.DataSource = ds
                ddAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddAccountManager.DataBind()
                ddAccountManager.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Quality Engr control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(22) '**SubscriptionID 9 is used for Account Manager
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddQualityEngr.DataSource = ds
                ddQualityEngr.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddQualityEngr.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddQualityEngr.DataBind()
                ddQualityEngr.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Packaging control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(108) '**SubscriptionID 9 is used for Account Manager
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddPackaging.DataSource = ds
                ddPackaging.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddPackaging.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddPackaging.DataBind()
                ddPackaging.Items.Insert(0, "")
            End If

            ''bind existing data to drop down UGN Location control for selection criteria for search
            ds = commonFunctions.GetUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNLocation.DataSource = ds
                ddUGNLocation.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddUGNLocation.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNLocation.DataBind()
                ddUGNLocation.Items.Insert(0, "")
            End If

            BindCriteriaAfterBind()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message & " 3 "
            lblErrors.Visible = True
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF BindCriteria

    Protected Sub BindCriteriaAfterBind()
        Try
            Dim ds As DataSet = New DataSet
            ''bind existing data to drop down Account Manager control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(147, ddUGNLocation.SelectedValue) '**SubscriptionID 147 is used for Shipping/EDI Coordinators
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddShipEDICoord.DataSource = ds
                ddShipEDICoord.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddShipEDICoord.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddShipEDICoord.DataBind()
                ddShipEDICoord.Items.Insert(0, "")
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message & " 3 "
            lblErrors.Visible = True
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF BindCriteriaAfterBind

    Public Sub BindData(ByVal SMRNo As Integer, ByVal Copy_SMRNo As Integer)
        Try
            If SMRNo = Nothing Then SMRNo = Copy_SMRNo

            Dim ds As DataSet = New DataSet
            If SMRNo <> Nothing Then
                ds = PGMModule.GetSampleMtrlReqRec(SMRNo)
                If commonFunctions.CheckDataSet(ds) = True Then
                    If Copy_SMRNo = 0 Then
                        lblSMRNo.Text = ds.Tables(0).Rows(0).Item("SMRNo").ToString()
                        txtRoutingStatus.Text = ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                        lblRoutingStatusDesc.Text = ds.Tables(0).Rows(0).Item("RoutingStatusDesc").ToString()
                        txtDueDate.Text = ds.Tables(0).Rows(0).Item("DueDate").ToString()
                        txtFormula.Text = ds.Tables(0).Rows(0).Item("Formula").ToString()
                        txtIssueDate.Text = ds.Tables(0).Rows(0).Item("IssueDate").ToString()
                        Select Case ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                            Case "N"
                                ddRecStatus.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                            Case "C"
                                ddRecStatus.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                            Case "T"
                                ddRecStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                            Case "R"
                                ddRecStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                            Case "V"
                                ddRecStatus.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                        End Select
                    Else
                        ddRecStatus.SelectedValue = "Open"
                        txtRoutingStatus.Text = "N"
                    End If
                    txtSampleDesc.Text = ds.Tables(0).Rows(0).Item("SampleDesc").ToString()
                    ddRequestor.SelectedValue = ds.Tables(0).Rows(0).Item("RequestorTMID").ToString()
                    ddUGNLocation.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                    ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AccountMgrTMID").ToString()
                    cbNotifyActMgr.Checked = ds.Tables(0).Rows(0).Item("NotifyActMgr").ToString()
                    ddIntExt.SelectedValue = ds.Tables(0).Rows(0).Item("IntExt").ToString()
                    ddQualityEngr.SelectedValue = ds.Tables(0).Rows(0).Item("QualityEngrTMID").ToString()
                    cddCustomer.SelectedValue = ds.Tables(0).Rows(0).Item("Customer").ToString()
                    ddPackaging.SelectedValue = ds.Tables(0).Rows(0).Item("PackagingTMID").ToString()
                    cbNotifyPkgCoord.Checked = ds.Tables(0).Rows(0).Item("NotifyPkgCoord").ToString()
                    cddTrialEvent.SelectedValue = ds.Tables(0).Rows(0).Item("TEID").ToString()
                    txtProjNo.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                    ddRecoveryType.SelectedValue = ds.Tables(0).Rows(0).Item("RecoveryType").ToString()
                    ddProdLevel.SelectedValue = ds.Tables(0).Rows(0).Item("ProdLevel").ToString()

                    If Copy_SMRNo = 0 Then
                        txtInvPONO.Text = ds.Tables(0).Rows(0).Item("InvPONo").ToString()
                        txtPkgReq.Text = ds.Tables(0).Rows(0).Item("PackagingReq").ToString()
                        ddShipMethod.SelectedValue = ds.Tables(0).Rows(0).Item("ShipMethod").ToString()
                        txtSpecInst.Text = ds.Tables(0).Rows(0).Item("SpecialInstructions").ToString()
                        txtLblReqComments.Text = ds.Tables(0).Rows(0).Item("LblReqComments").ToString()
                        txtInvPONO.Text = ds.Tables(0).Rows(0).Item("InvPONo").ToString()
                        txtShippingComments.Text = ds.Tables(0).Rows(0).Item("ShipComments").ToString()
                        hfAcctMgrEmail.Text = ds.Tables(0).Rows(0).Item("AcctMgrEmail").ToString()
                        hfQEngrEmail.Text = ds.Tables(0).Rows(0).Item("QualityEngrEmail").ToString()
                        hfPkgEmail.Text = ds.Tables(0).Rows(0).Item("PackagingEmail").ToString()
                        hfRequestorEmail.Text = ds.Tables(0).Rows(0).Item("RequestorEmail").ToString()
                        txtVoidReason.Text = ds.Tables(0).Rows(0).Item("VoidReason").ToString()

                        ddShipEDICoord.SelectedValue = ds.Tables(0).Rows(0).Item("ShipEdiCoordTMID").ToString()
                        hfShipEDICoordEmail.Text = ds.Tables(0).Rows(0).Item("ShipEDICoordEmail").ToString()
                        hfShipEdiCoordName.Text = ds.Tables(0).Rows(0).Item("ShipEdiCoordName").ToString()
                    End If

                    '*************
                    ''* Check that the Appropriation entered is a valid entry in SQL
                    ''*************
                    Dim ds2 As DataSet = New DataSet
                    ds2 = PURModule.GetInternalOrderRequestCapEx(0, txtProjNo.Text)
                    If commonFunctions.CheckDataSet(ds2) = True Then
                        If Not IsDBNull(ds2.Tables(0).Rows(0).Item("ProjectStatus")) And _
                           (Not IsDBNull(ds2.Tables(0).Rows(0).Item("ProjectTitle"))) And _
                            ds2.Tables(0).Rows(0).Item("DefinedCapex") = "D" Then
                            txtProjectStatus.Text = ds2.Tables(0).Rows(0).Item("ProjectStatus")
                            txtDefinedCapex.Text = ds2.Tables(0).Rows(0).Item("DefinedCapEx")

                            Select Case txtProjectStatus.Text
                                Case "Void"
                                    lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION IS VOID."
                                    lblErrors.Visible = "True"
                                    btnSave1.Enabled = False
                                    btnFwdApproval.Enabled = False

                                Case "Open"
                                    lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION IS PENDING SUBMISSION FOR APPROVAL."
                                    lblErrors.Visible = "True"
                                    btnSave1.Enabled = False
                                    btnFwdApproval.Enabled = False

                                Case "In Process"
                                    lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION IS IN PROCESS FOR APPROVAL."
                                    lblErrors.Visible = "True"
                                    btnSave1.Enabled = False
                                    btnFwdApproval.Enabled = False

                                Case "Rejected"
                                    lblErrors.Text = "UNABLE TO PROCESS - APPROPRIATION IS REJECTED."
                                    lblErrors.Visible = "True"
                                    btnSave1.Enabled = False
                                    btnFwdApproval.Enabled = False
                            End Select
                        End If 'EOF if ds2.Tables(0).Rows(0).Item("ProjectStatus")

                        If Not IsDBNull(ds2.Tables(0).Rows(0).Item("ProjectTitle")) Then
                            txtProjectTitle.Text = ds2.Tables(0).Rows(0).Item("ProjectTitle")
                            hplkAppropriation.Text = ds2.Tables(0).Rows(0).Item("ProjectTitle")
                            hplkAppropriation.Visible = True
                            If txtProjNo.Text <> Nothing And (txtProjectTitle.Text <> Nothing) Then
                                Select Case txtProjNo.Text.Substring(0, 1)
                                    Case "A"
                                        hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjAssets.aspx?pProjNo=" & txtProjNo.Text
                                    Case "D"
                                        hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjDevelopment.aspx?pProjNo=" & txtProjNo.Text
                                    Case "P"
                                        hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjPackaging.aspx?pProjNo=" & txtProjNo.Text
                                    Case "R"
                                        hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjRepair.aspx?pProjNo=" & txtProjNo.Text
                                    Case "T"
                                        hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjTooling.aspx?pProjNo=" & txtProjNo.Text
                                End Select
                            End If 'EOF If txtSMRNo.Text <> Nothing Then
                        Else
                            hplkAppropriation.Text = "Not Found in UGNDB"
                            hplkAppropriation.Visible = True
                        End If 'EOF If Not IsDBNull(ds2.Tables(0).Rows(0).Item("ProjectTitle")) Then
                    Else
                        txtProjectTitle.Text = Nothing
                        txtDefinedCapex.Text = Nothing
                        hplkAppropriation.Visible = False
                    End If 'EOF If txtSMRNo.Text <> Nothing Then


                    If ViewState("pRID") <> 0 Then
                        ds = PGMModule.GetSampleMtrlReqRSS(ViewState("pSMRNo"), ViewState("pRID"))
                        If (ds.Tables.Item(0).Rows.Count > 0) Then
                            txtQC.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                        Else 'no record found reset query string pRptID
                            Response.Redirect("SampleMaterialRequest.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pRID=0&pRC=1", False)
                        End If
                    End If
                End If 'EOF If commonFunctions.CheckDataSet(ds) = True Then
            End If 'EOF If SMRNo <> Nothing Then

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

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("SampleMaterialRequest.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnReset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset1.Click
        ClearErrorMsgFlds()

        'Redirect user to the same tab view when Reset button was clicked
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If

        Dim TempViewState As Integer
        If ViewState("pSMRNo") <> 0 Or ViewState("pCP") <> 0 Then
            TempViewState = mvTabs.ActiveViewIndex
            mvTabs.GetActiveView()
            mnuTabs.Items(TempViewState).Selected = True

            BindData(ViewState("pSMRNo"), ViewState("pCP"))
        Else
            Response.Redirect("SampleMaterialRequest.aspx", False)
        End If
    End Sub 'EOF btnReset1_Click

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click
        Try
            ClearErrorMsgFlds()

            Dim DefaultDate As String = Date.Now
            Dim RecStatus As String = Nothing
            RecStatus = ViewState("RecStatus")

            If (ViewState("pSMRNo") <> 0) Then
                '***************
                '* Update Data
                '***************
                UpdateRecord(RecStatus, IIf(RecStatus = "Completed", "C", IIf(RecStatus = "Void", "V", IIf(RecStatus = "Open", "N", txtRoutingStatus.Text))), False)

                ''*************
                ''Check for Closed, Void status send email notfication 
                ''*************
                If RecStatus = "Void" Then 'And txtRoutingStatus.Text = "V"
                    If txtRoutingStatus.Text <> "N" And txtRoutingStatus.Text <> "V" Then
                        ''*****************
                        ''History Tracking
                        ''*****************
                        PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), txtSampleDesc.Text, ViewState("iTeamMemberID"), "VOID -" & txtVoidReason.Text)
                        SendNotifWhenEventChanges("Void")
                    End If
                End If

            Else

                '***************
                '* Save Data
                '***************
                PGMModule.InsertSampleMtrlReq("Open", "N", txtSampleDesc.Text, ddRequestor.SelectedValue, ddAccountManager.SelectedValue, ddQualityEngr.SelectedValue, ddPackaging.SelectedValue, ddUGNLocation.SelectedValue, ddCustomer.SelectedValue, ddTrialEvent.SelectedValue, txtFormula.Text, ddIntExt.SelectedValue, txtProjNo.Text, txtDueDate.Text, ddRecoveryType.SelectedValue, ddProdLevel.SelectedValue, cbNotifyActMgr.Checked, cbNotifyPkgCoord.Checked, ViewState("DefaultUser"), DefaultDate)

                '***************
                '* Locate Next available ProjectNo based on Facility selection
                '***************
                Dim ds As DataSet = Nothing
                ds = PGMModule.GetLastSampleMtrlReq(txtSampleDesc.Text, ddRequestor.SelectedValue, ddUGNLocation.SelectedValue, ddCustomer.SelectedValue, ddTrialEvent.SelectedValue, txtFormula.Text, "Open", ViewState("DefaultUser"), DefaultDate)

                ViewState("pSMRNo") = ds.Tables(0).Rows(0).Item("LastSMRNo").ToString()

                ''*****************
                ''History Tracking
                ''*****************
                PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), txtSampleDesc.Text, ViewState("iTeamMemberID"), "Record created.")

                ''***************
                ''* Binda data then Build Notification list with Mtrl/QA Mgr
                ''***************
                BindData(ViewState("pSMRNo"), 0)
                BuildApprovalList()

                Response.Redirect("SampleMaterialRequest.aspx?pSMRNo=" & ViewState("pSMRNo"), False)
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
            PGMModule.UpdateSampleMtrlReq(ViewState("pSMRNo"), RecStatus, RoutingStatus, txtSampleDesc.Text, ddRequestor.SelectedValue, ddAccountManager.SelectedValue, ddQualityEngr.SelectedValue, ddPackaging.SelectedValue, ddUGNLocation.SelectedValue, ddCustomer.SelectedValue, ddTrialEvent.SelectedValue, txtFormula.Text, ddIntExt.SelectedValue, txtProjNo.Text, txtDueDate.Text, ddRecoveryType.SelectedValue, ddProdLevel.SelectedValue, IIf(RecSubmitted = False, txtIssueDate.Text, Date.Now), txtPkgReq.Text, ddShipMethod.SelectedValue, txtSpecInst.Text, txtLblReqComments.Text, txtInvPONO.Text, txtVoidReason.Text, cbNotifyActMgr.Checked, cbNotifyPkgCoord.Checked, ViewState("DefaultUser"))
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

#End Region 'EOF "General - Sample Information"

#Region "gvPartNo"

    Protected Sub gvPartNo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPartNo.RowCommand

        Try
            ClearErrorMsgFlds()
            Dim PartNo As TextBox
            Dim DesignLevel As TextBox
            Dim SizeThickness As TextBox
            Dim Qty As TextBox
            Dim Price As TextBox
            Dim RecoveryAmt As TextBox
            Dim PONo As TextBox

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then

                odsPartNo.InsertParameters("SMRNo").DefaultValue = ViewState("pSMRNo")

                PartNo = CType(gvPartNo.FooterRow.FindControl("txtPartNo"), TextBox)
                odsPartNo.InsertParameters("PartNo").DefaultValue = PartNo.Text

                DesignLevel = CType(gvPartNo.FooterRow.FindControl("txtDesignLevel"), TextBox)
                odsPartNo.InsertParameters("DesignLevel").DefaultValue = DesignLevel.Text

                SizeThickness = CType(gvPartNo.FooterRow.FindControl("txtSizeThickness"), TextBox)
                odsPartNo.InsertParameters("SizeThickness").DefaultValue = SizeThickness.Text

                Qty = CType(gvPartNo.FooterRow.FindControl("txtQty"), TextBox)
                odsPartNo.InsertParameters("Qty").DefaultValue = Qty.Text

                Price = CType(gvPartNo.FooterRow.FindControl("txtPrice"), TextBox)
                odsPartNo.InsertParameters("Price").DefaultValue = Price.Text

                RecoveryAmt = CType(gvPartNo.FooterRow.FindControl("txtRecoveryAmt"), TextBox)
                odsPartNo.InsertParameters("RecoveryAmt").DefaultValue = RecoveryAmt.Text

                PONo = CType(gvPartNo.FooterRow.FindControl("txtPONo"), TextBox)
                odsPartNo.InsertParameters("PONo").DefaultValue = PONo.Text

                odsPartNo.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvPartNo.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvPartNo.ShowFooter = True
                Else
                    gvPartNo.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                PartNo = CType(gvPartNo.FooterRow.FindControl("txtPartNo"), TextBox)
                PartNo.Text = ""

                DesignLevel = CType(gvPartNo.FooterRow.FindControl("txtDesignLevel"), TextBox)
                DesignLevel.Text = ""

                SizeThickness = CType(gvPartNo.FooterRow.FindControl("txtSizeThickness"), TextBox)
                SizeThickness.Text = ""

                Qty = CType(gvPartNo.FooterRow.FindControl("txtQty"), TextBox)
                Qty.Text = ""

                Price = CType(gvPartNo.FooterRow.FindControl("txtPrice"), TextBox)
                Price.Text = ""

                RecoveryAmt = CType(gvPartNo.FooterRow.FindControl("txtRecoveryAmt"), TextBox)
                RecoveryAmt.Text = ""

                PONo = CType(gvPartNo.FooterRow.FindControl("txtPONo"), TextBox)
                PONo.Text = ""
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF gvPartNo_RowCommand

    Private Property LoadDataEmpty_gvPartNo() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_gvPartNo") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_gvPartNo"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get

        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_gvPartNo") = value
        End Set

    End Property 'EOF LoadDataEmpty_gvPartNo

    Protected Sub odsPartNo_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsPartNo.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)


            Dim dt As PGM.SampleMtrlReq_PartNoDataTable = CType(e.ReturnValue, PGM.SampleMtrlReq_PartNoDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_gvPartNo = True
            Else
                LoadDataEmpty_gvPartNo = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF odsPartNo_Selected

    Protected Sub gvPartNo_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPartNo.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_gvPartNo
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub 'EOF gvPartNo_RowCreated

    Protected Sub gvPartNo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPartNo.RowDataBound

        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(8).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As PGM.SampleMtrlReq_PartNoRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, PGM.SampleMtrlReq_PartNoRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Part Number (" & DataBinder.Eval(e.Row.DataItem, "txtPartNo") & ") entry?');")
                End If
            End If
        End If
    End Sub 'EOF gvPartNo_RowDataBound
#End Region 'EOF gvPartNo

#Region "Upload - Supporting Documents"
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
        ElseIf EncodeType = "application/octet-stream" Then
            strReturn = "~/images/snp.jpg"
        ElseIf EncodeType = "image/pjpeg" Then
            strReturn = "~/images/pjpeg.jpg"
        End If

        Return strReturn
    End Function 'EOF DisplayImage

    Protected Sub gvPkgReq_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPkgReq.RowDataBound, gvDeliveryInst.RowDataBound, gvLblReq.RowDataBound, gvAddtlDocs.RowDataBound, gvShipDocs.RowDataBound

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
                    Dim FileDesc As PGM.SampleMtrlReq_DocumentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, PGM.SampleMtrlReq_DocumentsRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record (" & DataBinder.Eval(e.Row.DataItem, "Description") & ")?');")
                End If
            End If
        End If
    End Sub 'EOF gv...._RowDataBound

    Protected Sub gvPkgReq_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPkgReq.RowCommand, gvDeliveryInst.RowCommand, gvLblReq.RowCommand, gvInvInfo.RowCommand, gvAddtlDocs.RowCommand
        ClearErrorMsgFlds()

        ''***
        ''This section allows the inserting of a new row when save button is clicked from the footer.
        ''***
        If e.CommandName = "Delete" Then
            mvTabs.ActiveViewIndex = Int32.Parse(1)
            mvTabs.GetActiveView()
            mnuTabs.Items(1).Selected = True
        End If
    End Sub 'EOF gvPkgReq_RowCommand

    Protected Sub gvShipDocs_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvShipDocs.RowCommand
        ClearErrorMsgFlds()

        ''***
        ''This section allows the inserting of a new row when save button is clicked from the footer.
        ''***
        If e.CommandName = "Delete" Then
            mvTabs.ActiveViewIndex = Int32.Parse(3)
            mvTabs.GetActiveView()
            mnuTabs.Items(3).Selected = True
        End If
    End Sub 'EOF gvShipDocs_RowCommand
    Protected Sub btnResetPkgReq_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetPkgReq.Click, btnResetDeliveryInst.Click, btnResetInvInfo.Click, btnResetLblReq.Click, btnResetPkgReq.Click, btnResetAddtlDocs.Click, btnResetShipDoc.Click
        ClearErrorMsgFlds()

        txtFileDesc1.Text = Nothing
        txtFileDesc2.Text = Nothing
        txtFileDesc3.Text = Nothing
        txtFileDesc4.Text = Nothing
        txtFileDesc5.Text = Nothing
        txtFileDesc6.Text = Nothing

        'Redirect user to the same tab view when Reset button was clicked
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If

        Dim TempViewState As Integer
        If ViewState("pSMRNo") <> Nothing Or ViewState("pSMRNo") <> "" Then
            TempViewState = mvTabs.ActiveViewIndex
            mvTabs.GetActiveView()
            mnuTabs.Items(TempViewState).Selected = True

            BindData(ViewState("pSMRNo"), 0)
        Else
            Response.Redirect("SampleMaterialRequest.aspx", False)
        End If
    End Sub 'EOF btnReset second tab

    Protected Sub btnUploadPkgReq_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadPkgReq.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim RecStatus As String = Nothing
            RecStatus = ViewState("RecStatus")

            ClearErrorMsgFlds()

            If ViewState("pSMRNo") <> "" Then
                UpdateRecord(RecStatus, IIf(RecStatus = "Completed", "C", IIf(RecStatus = "Void", "V", IIf(RecStatus = "Open", "N", txtRoutingStatus.Text))), False)

                If uploadFilePkgReq.HasFile Then
                    If (uploadFilePkgReq.PostedFile.ContentLength <= 3500000) Then

                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFilePkgReq.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFilePkgReq.PostedFile.FileName)

                        '** With use of MS Office 2007 **/
                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(uploadFilePkgReq.PostedFile.InputStream.Length)
                        Dim SupportingDocEncodeType As String = uploadFilePkgReq.PostedFile.ContentType
                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        uploadFilePkgReq.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".doc") Or (FileExt = ".xlsx") Or (FileExt = ".docx") Or (FileExt = ".snp") Or (FileExt = ".tif") Or (FileExt = ".jpg") Then
                            ''*************
                            '' Display File Info
                            ''*************

                            lblMessageView1.Text = "File name: " & uploadFilePkgReq.FileName & "<br/>" & _
                            "File Size: " & CType((SupportingDocFileSize / 1024), Integer) & " KB<br/>"
                            lblMessageView1.Visible = True
                            lblMessageView1.Width = 500
                            lblMessageView1.Height = 30

                            ''***************
                            '' Insert Record
                            ''***************
                            PGMModule.InsertSampleMtrlReqDocuments(ViewState("pSMRNo"), ViewState("iTeamMemberID"), "P", txtFileDesc1.Text, SupportingDocBinaryFile, uploadFilePkgReq.FileName, SupportingDocEncodeType, SupportingDocFileSize)

                            gvPkgReq.DataBind()
                            revUploadFilePkgReq.Enabled = False
                            txtFileDesc1.Text = Nothing
                        End If
                    Else
                        lblMessageView1.Text = "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
                        lblMessageView1.Visible = True
                        btnUploadPkgReq.Enabled = False
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
    End Sub 'EOF btnUploadPkgReq_Click

    Protected Sub btnUploadDeliveryInst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadDeliveryInst.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim RecStatus As String = Nothing
            RecStatus = ViewState("RecStatus")

            ClearErrorMsgFlds()

            If ViewState("pSMRNo") <> 0 Then
                UpdateRecord(RecStatus, IIf(RecStatus = "Completed", "C", IIf(RecStatus = "Void", "V", IIf(RecStatus = "Open", "N", txtRoutingStatus.Text))), False)

                If uploadFileDeliveryInst.HasFile Then
                    If (uploadFileDeliveryInst.PostedFile.ContentLength <= 3500000) Then

                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFileDeliveryInst.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFileDeliveryInst.PostedFile.FileName)

                        '** With use of MS Office 2007 **/
                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(uploadFileDeliveryInst.PostedFile.InputStream.Length)
                        Dim SupportingDocEncodeType As String = uploadFileDeliveryInst.PostedFile.ContentType
                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        uploadFileDeliveryInst.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".doc") Or (FileExt = ".xlsx") Or (FileExt = ".docx") Or (FileExt = ".snp") Or (FileExt = ".tif") Or (FileExt = ".jpg") Then

                            ''*************
                            '' Display File Info
                            ''*************
                            lblMessageView2.Text = "File name: " & uploadFileDeliveryInst.FileName & "<br/>" & _
                            "File Size: " & CType((SupportingDocFileSize / 1024), Integer) & " KB<br/>"
                            lblMessageView2.Visible = True
                            lblMessageView2.Width = 500
                            lblMessageView2.Height = 30

                            ''***************
                            '' Insert Record
                            ''***************
                            PGMModule.InsertSampleMtrlReqDocuments(ViewState("pSMRNo"), ViewState("iTeamMemberID"), "D", txtFileDesc2.Text, SupportingDocBinaryFile, uploadFileDeliveryInst.FileName, SupportingDocEncodeType, SupportingDocFileSize)
                            gvDeliveryInst.DataBind()
                            revUploadFileDeliveryInst.Enabled = False
                            txtFileDesc2.Text = Nothing
                        End If
                    Else
                        lblMessageView2.Text = "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
                        lblMessageView2.Visible = True
                        btnUploadDeliveryInst.Enabled = False
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
    End Sub 'EOF btnUploadDeliveryInst_Click

    Protected Sub btnUploadLblReq_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadLblReq.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim RecStatus As String = Nothing
            RecStatus = ViewState("RecStatus")

            ClearErrorMsgFlds()

            If ViewState("pSMRNo") <> 0 Then
                UpdateRecord(RecStatus, IIf(RecStatus = "Completed", "C", IIf(RecStatus = "Void", "V", IIf(RecStatus = "Open", "N", txtRoutingStatus.Text))), False)

                If uploadFileLblReq.HasFile Then
                    If (uploadFileLblReq.PostedFile.ContentLength <= 3500000) Then

                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFileLblReq.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFileLblReq.PostedFile.FileName)

                        '** With use of MS Office 2007 **/
                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(uploadFileLblReq.PostedFile.InputStream.Length)
                        Dim SupportingDocEncodeType As String = uploadFileLblReq.PostedFile.ContentType
                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        uploadFileLblReq.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".doc") Or (FileExt = ".xlsx") Or (FileExt = ".docx") Or (FileExt = ".snp") Or (FileExt = ".tif") Or (FileExt = ".jpg") Then

                            ''*************
                            '' Display File Info
                            ''*************
                            lblMessageView3.Text = "File name: " & uploadFileLblReq.FileName & "<br/>" & _
                            "File Size: " & CType((SupportingDocFileSize / 1024), Integer) & " KB<br/>"
                            lblMessageView3.Visible = True
                            lblMessageView3.Width = 500
                            lblMessageView3.Height = 30

                            ''***************
                            '' Insert Record
                            ''***************
                            PGMModule.InsertSampleMtrlReqDocuments(ViewState("pSMRNo"), ViewState("iTeamMemberID"), "L", txtFileDesc3.Text, SupportingDocBinaryFile, uploadFileLblReq.FileName, SupportingDocEncodeType, SupportingDocFileSize)

                            gvLblReq.DataBind()
                            revUploadFileLblReq.Enabled = False
                            txtFileDesc3.Text = Nothing
                        End If
                    Else
                        lblMessageView3.Text = "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
                        lblMessageView3.Visible = True
                        btnUploadLblReq.Enabled = False
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
    End Sub 'EOF btnUploadLblReq_Click

    Protected Sub btnUploadInvInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadInvInfo.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim RecStatus As String = Nothing
            RecStatus = ViewState("RecStatus")

            ClearErrorMsgFlds()

            If ViewState("pSMRNo") <> 0 Then
                UpdateRecord(RecStatus, IIf(RecStatus = "Completed", "C", IIf(RecStatus = "Void", "V", IIf(RecStatus = "Open", "N", txtRoutingStatus.Text))), False)

                If uploadFileInvInfo.HasFile Then
                    If (uploadFileInvInfo.PostedFile.ContentLength <= 3500000) Then

                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFileInvInfo.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFileInvInfo.PostedFile.FileName)

                        '** With use of MS Office 2007 **/
                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(uploadFileInvInfo.PostedFile.InputStream.Length)
                        Dim SupportingDocEncodeType As String = uploadFileInvInfo.PostedFile.ContentType
                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        uploadFileInvInfo.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".doc") Or (FileExt = ".xlsx") Or (FileExt = ".docx") Or (FileExt = ".snp") Or (FileExt = ".tif") Or (FileExt = ".jpg") Then

                            ''*************
                            '' Display File Info
                            ''*************
                            lblMessageView4.Text = "File name: " & uploadFileInvInfo.FileName & "<br/>" & _
                            "File Size: " & CType((SupportingDocFileSize / 1024), Integer) & " KB<br/>"
                            lblMessageView4.Visible = True
                            lblMessageView4.Width = 500
                            lblMessageView4.Height = 30

                            ''***************
                            '' Insert Record
                            ''***************
                            PGMModule.InsertSampleMtrlReqDocuments(ViewState("pSMRNo"), ViewState("iTeamMemberID"), "I", txtFileDesc4.Text, SupportingDocBinaryFile, uploadFileInvInfo.FileName, SupportingDocEncodeType, SupportingDocFileSize)

                            gvInvInfo.DataBind()
                            revUploadFileInvInfo.Enabled = False
                            txtFileDesc4.Text = Nothing
                        End If
                    Else
                        lblMessageView4.Text = "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
                        lblMessageView4.Visible = True
                        btnUploadInvInfo.Enabled = False
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
    End Sub 'EOF btnUploadInvInfo_Click

    Protected Sub btnUploadAddtlDocs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadAddtlDocs.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim RecStatus As String = Nothing
            RecStatus = ViewState("RecStatus")

            ClearErrorMsgFlds()

            If ViewState("pSMRNo") <> 0 Then
                UpdateRecord(RecStatus, IIf(RecStatus = "Completed", "C", IIf(RecStatus = "Void", "V", IIf(RecStatus = "Open", "N", txtRoutingStatus.Text))), False)

                If uploadFileAddtlDocs.HasFile Then
                    If (uploadFileAddtlDocs.PostedFile.ContentLength <= 3500000) Then

                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFileAddtlDocs.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFileAddtlDocs.PostedFile.FileName)

                        '** With use of MS Office 2007 **/
                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(uploadFileAddtlDocs.PostedFile.InputStream.Length)
                        Dim SupportingDocEncodeType As String = uploadFileAddtlDocs.PostedFile.ContentType
                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        uploadFileAddtlDocs.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".doc") Or (FileExt = ".xlsx") Or (FileExt = ".docx") Or (FileExt = ".snp") Or (FileExt = ".tif") Or (FileExt = ".jpg") Then

                            ''*************
                            '' Display File Info
                            ''*************
                            lblMessageView5.Text = "File name: " & uploadFileAddtlDocs.FileName & "<br/>" & _
                            "File Size: " & CType((SupportingDocFileSize / 1024), Integer) & " KB<br/>"
                            lblMessageView5.Visible = True
                            lblMessageView5.Width = 500
                            lblMessageView5.Height = 30

                            ''***************
                            '' Insert Record
                            ''***************
                            PGMModule.InsertSampleMtrlReqDocuments(ViewState("pSMRNo"), ViewState("iTeamMemberID"), "A", txtFileDesc5.Text, SupportingDocBinaryFile, uploadFileAddtlDocs.FileName, SupportingDocEncodeType, SupportingDocFileSize)

                            gvAddtlDocs.DataBind()
                            revUploadFileAddtlDocs.Enabled = False
                            txtFileDesc5.Text = Nothing
                        End If
                    Else
                        lblMessageView5.Text = "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
                        lblMessageView5.Visible = True
                        btnUploadAddtlDocs.Enabled = False
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
    End Sub 'EOF btnUploadAddtlDocs_Click

    Protected Sub btnUploadShipDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadShipDoc.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim RecStatus As String = Nothing
            RecStatus = ViewState("RecStatus")

            ClearErrorMsgFlds()

            If ViewState("pSMRNo") <> 0 Then
                UpdateRecord(RecStatus, IIf(RecStatus = "Completed", "C", IIf(RecStatus = "Void", "V", IIf(RecStatus = "Open", "N", txtRoutingStatus.Text))), False)

                If uploadFileShipDoc.HasFile Then
                    If (uploadFileShipDoc.PostedFile.ContentLength <= 3500000) Then

                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFileShipDoc.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFileShipDoc.PostedFile.FileName)

                        '** With use of MS Office 2007 **/
                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(uploadFileShipDoc.PostedFile.InputStream.Length)
                        Dim SupportingDocEncodeType As String = uploadFileShipDoc.PostedFile.ContentType
                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        uploadFileShipDoc.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".doc") Or (FileExt = ".xlsx") Or (FileExt = ".docx") Or (FileExt = ".snp") Or (FileExt = ".tif") Or (FileExt = ".jpg") Then

                            ''*************
                            '' Display File Info
                            ''*************
                            lblMessageView6.Text = "File name: " & uploadFileShipDoc.FileName & "<br/>" & _
                            "File Size: " & CType((SupportingDocFileSize / 1024), Integer) & " KB<br/>"
                            lblMessageView6.Visible = True
                            lblMessageView6.Width = 500
                            lblMessageView6.Height = 30

                            ''***************
                            '' Insert Record
                            ''***************
                            PGMModule.InsertSampleMtrlReqDocuments(ViewState("pSMRNo"), ViewState("iTeamMemberID"), "S", txtFileDesc6.Text, SupportingDocBinaryFile, uploadFileShipDoc.FileName, SupportingDocEncodeType, SupportingDocFileSize)

                            gvShipDocs.DataBind()
                            revUploadFileShipDoc.Enabled = False
                            txtFileDesc6.Text = Nothing
                        End If
                    Else
                        lblMessageView6.Text = "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
                        lblMessageView6.Visible = True
                        btnUploadShipDoc.Enabled = False
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
    End Sub 'EOF btnUploadShipDoc_Click

#End Region 'EOF "Upload - Supporting Documents"

#Region "gvShipping"
    Protected Sub gvShipping_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvShipping.RowCommand

        Try
            Dim ShipperNo As TextBox
            Dim TotalShippingCost As TextBox
            Dim FreightBillProNo As TextBox

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then
                odsShipping.InsertParameters("SMRNo").DefaultValue = ViewState("pSMRNo")

                ShipperNo = CType(gvShipping.FooterRow.FindControl("txtShipperNo"), TextBox)
                odsShipping.InsertParameters("ShipperNo").DefaultValue = ShipperNo.Text

                TotalShippingCost = CType(gvShipping.FooterRow.FindControl("txtTotalShippingCost"), TextBox)
                odsShipping.InsertParameters("TotalShippingCost").DefaultValue = TotalShippingCost.Text

                FreightBillProNo = CType(gvShipping.FooterRow.FindControl("txtFreightBillProNo"), TextBox)
                odsShipping.InsertParameters("FreightBillProNo").DefaultValue = FreightBillProNo.Text

                odsShipping.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvShipping.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvShipping.ShowFooter = True
                Else
                    gvShipping.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ShipperNo = CType(gvShipping.FooterRow.FindControl("txtShipperNo"), TextBox)
                ShipperNo.Text = ""

                TotalShippingCost = CType(gvShipping.FooterRow.FindControl("txtTotalShippingCost"), TextBox)
                TotalShippingCost.Text = ""

                FreightBillProNo = CType(gvShipping.FooterRow.FindControl("txtFreightBillProNo"), TextBox)
                FreightBillProNo.Text = ""
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF gvShipping_RowCommand

    Private Property LoadDataEmpty_gvShipping() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_gvShipping") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_gvShipping"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get

        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_gvShipping") = value
        End Set

    End Property 'EOF LoadDataEmpty_gvShipping

    Protected Sub odsShipping_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsShipping.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)


            Dim dt As PGM.SampleMtrlReq_ShippingDataTable = CType(e.ReturnValue, PGM.SampleMtrlReq_ShippingDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_gvShipping = True
            Else
                LoadDataEmpty_gvShipping = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF odsShipping_Selected

    Protected Sub gvShipping_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvShipping.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_gvShipping
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub 'EOF gvShipping_RowCreated

    Protected Sub gvShipping_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvShipping.RowDataBound

        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(4).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim TotalShippingCost As PGM.SampleMtrlReq_ShippingRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, PGM.SampleMtrlReq_ShippingRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Shipper Number (" & DataBinder.Eval(e.Row.DataItem, "txtShipperNo") & ") entry?');")
                End If
            End If
        End If

    End Sub
#End Region 'EOF gvShipping

#Region "Communication Board"
    Public Function GoToCommunicationBoard(ByVal SMRNo As String, ByVal RSSID As Integer, ByVal ApprovalLevel As Integer, ByVal TeamMemberID As Integer) As String

        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If

        Return "SampleMaterialRequest.aspx?pSMRNo=" & SMRNo & "&pAL=" & ApprovalLevel & "&pTMID=" & TeamMemberID & "&pRID=" & RSSID & "&pRC=1" & Aprv

    End Function 'EOF GoToCommunicationBoard

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim RSSID As Integer
            Dim drRSSID As PGM.SampleMtrlReq_RSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, PGM.SampleMtrlReq_RSSRow)

            If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                RSSID = drRSSID.RSSID
                ' Reference the rpCBRC ObjectDataSource
                Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                ' Set the CategoryID Parameter value
                rpCBRC.SelectParameters("SMRNo").DefaultValue = drRSSID.SMRNo.ToString()
                rpCBRC.SelectParameters("RSSID").DefaultValue = drRSSID.RSSID.ToString()
            End If
        End If
    End Sub 'EOF gvQuestion_RowDataBound

    Protected Sub btnSaveCB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCB.Click
        Try
            ClearErrorMsgFlds()

            ''************************************
            ''Send response back to requestor in Communication Board
            ''************************************
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
            If CurrentEmpEmail <> Nothing And ViewState("pSMRNo") <> Nothing Then
                ''*************************************************************************
                ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                ''*************************************************************************
                Dim dsExp As DataSet = New DataSet

                ''**********************
                ''*Initialize Variables
                ''**********************
                Dim RequestedBy As Integer = ddRequestor.SelectedValue
                Dim SampleDesc As String = txtSampleDesc.Text

                ''***************************************************************
                ''Send Reply back to requestor
                ''***************************************************************
                ds = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, TMID, False, False)
                ''Check that the recipient(s) is a valid Team Member
                If ds.Tables.Count > 0 And (ds.Tables.Item(0).Rows.Count > 0) Then
                    For i = 0 To ds.Tables.Item(0).Rows.Count - 1
                        If (ds.Tables(0).Rows(i).Item("WorkStatus") = True) Then
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
                    If ViewState("iTeamMemberID") <> RequestedBy Then
                        EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC)
                    End If

                    ''***************************************************************
                    ''Carbon Copy Previous Levels
                    ''***************************************************************
                    EmailCC = CarbonCopyList(MyMessage, 0, "", SeqNo, 0, EmailCC)

                    EmailCC &= hfAcctMgrEmail.Text & ";"
                    EmailCC &= hfQEngrEmail.Text & ";"
                    EmailCC &= hfPkgEmail.Text & ";"

                    'Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                    Else
                        MyMessage.Subject = ""
                    End If

                    MyMessage.Subject &= "Sample Material Request: " & SampleDesc & " - MESSAGE RECEIVED"

                    MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
                    MyMessage.Body &= " <tr>"
                    MyMessage.Body &= "     <td valign='top' width='20%'>"
                    MyMessage.Body &= "         <img src='" & ViewState("strProdOrTestEnvironment") & "/images/messanger60.jpg'/>"
                    MyMessage.Body &= "     </td>"
                    MyMessage.Body &= "     <td valign='top'>"
                    MyMessage.Body &= "         <b>Attention:</b> " & EmpName
                    MyMessage.Body &= "             <p><b>" & ViewState("DefaultUserFullName") & "</b> sent a message regarding IOR Ref#"
                    MyMessage.Body &= "         <font color='red'>" & ViewState("pSMRNo") & " - " & SampleDesc & "</font>."
                    MyMessage.Body &= "         <br/><br/><i>Question:&nbsp;&nbsp;</i><b>" & txtQC.Text & "</b>"
                    MyMessage.Body &= "         <br/><br/><i>Response:&nbsp;&nbsp;</i><b>" & txtReply.Text & "</b><br/><br/>"
                    MyMessage.Body &= "         </p>"
                    MyMessage.Body &= "         <p><a href='" & ViewState("strProdOrTestEnvironment") & "/PGM/crSampleMtrlReqApproval.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pAprv=1" & "'>Click here</a> to return to the approval page."
                    MyMessage.Body &= "     </td>"
                    MyMessage.Body &= " </tr>"
                    MyMessage.Body &= "<table>"
                    MyMessage.Body &= "<br><br>"

                    ' ''If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                    ' ''    MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                    ' ''    MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                    ' ''    EmailFrom = "Database.Notifications@ugnauto.com"
                    ' ''    EmailTO = CurrentEmpEmail '"lynette.rey@ugnauto.com"
                    EmailCC = "lynette.rey@ugnauto.com"
                    ' ''End If

                    ''*****************
                    ''History Tracking
                    ''*****************
                    PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), SampleDesc, ViewState("iTeamMemberID"), "Message Sent")

                    ''**********************************
                    ''Save Reponse to child table
                    ''**********************************
                    PGMModule.InsertSampleMtrlReqRSSReply(ViewState("pSMRNo"), ViewState("pRID"), SampleDesc, ViewState("iTeamMemberID"), txtReply.Text)

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Sample Material Request)", ViewState("pSMRNo"))
                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                    Catch ex As Exception
                        lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                        UGNErrorTrapping.InsertEmailQueue("Req#:" & ViewState("pSMRNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
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
                    Response.Redirect("SampleMaterialRequest.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pRC=1" & Aprv, False)

                Else 'EmailTO = ''
                    ''**********************************
                    ''Rebind the data to the form
                    ''**********************************
                    txtQC.Text = Nothing
                    txtReply.Text = Nothing

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
#End Region 'EOF Communication Board

#Region "Approval Status"
    Protected Sub gvApprovers_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvApprovers.RowCommand
        Try
            ClearErrorMsgFlds()

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
                BindData(ViewState("pSMRNo"), 0)
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvApprovers.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True And _
                    ddShipEDICoord.SelectedValue = Nothing Then
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
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub 'EOF gvApprovers_RowCommand

    Protected Sub gvApprovers_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Try
            ClearErrorMsgFlds()

            Dim row As GridViewRow = gvApprovers.Rows(e.RowIndex)
            If row IsNot Nothing Then
                Dim t As DropDownList = TryCast(row.FindControl("ddStatus"), DropDownList)
                Dim c As TextBox = TryCast(row.FindControl("txtAppComments"), TextBox)
                Dim tm As TextBox = TryCast(row.FindControl("txtTeamMemberID"), TextBox)
                Dim TeamMemberID As Integer = CType(tm.Text, Integer)
                Dim LinkLocation As String = Nothing

                Dim s As TextBox = TryCast(row.FindControl("hfSeqNo"), TextBox)
                Dim hfSeqNo As Integer = CType(s.Text, Integer) 'Row Selected - view Approver Sequence No
                Dim ds As DataSet = New DataSet

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
                        Dim LastSeqNo As Boolean = False

                        Dim CurrentEmpEmail As String = Nothing
                        If ViewState("DefaultUserEmail") IsNot Nothing Then
                            CurrentEmpEmail = ViewState("DefaultUserEmail")
                            EmailFrom = CurrentEmpEmail
                            EmailCC = CurrentEmpEmail & ";"
                        Else
                            CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                        End If

                        ''***********
                        ''* Verify that Row selected Team Member Sequence No is Last to Approve
                        ''***********
                        Dim dsLast As DataSet = New DataSet
                        Dim r As Integer = 0
                        Dim totalApprovers As Integer = 0
                        Dim PendingApprovers As Boolean = False

                        dsLast = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, 0, False, False)
                        If commonFunctions.CheckDataSet(dsLast) = True Then
                            For r = 0 To dsLast.Tables.Item(0).Rows.Count - 1
                                totalApprovers = dsLast.Tables(0).Rows(r).Item("SeqNo")
                                PendingApprovers = IIf(dsLast.Tables(0).Rows(r).Item("Status") <> "Pending", True, False)
                            Next
                        End If
                        If totalApprovers = hfSeqNo And PendingApprovers = True Then
                            LastSeqNo = True
                        Else
                            LastSeqNo = False
                        End If

                        If ddShipEDICoord.SelectedValue = Nothing _
                            And (LastSeqNo = True Or totalApprovers = 2) _
                            And t.SelectedValue <> "Rejected" Then

                            lblErrors.Text = "Select a Shipping or EDI Coordinator to assign this Sample Material Request to."
                            lblErrors.Visible = True
                            lblReqAppComments.Text = "Select a Shipping or EDI Coordinator to assign this Sample Material Request to."
                            lblReqAppComments.Visible = True
                            lblReqAppComments.ForeColor = Color.Red
                            MaintainScrollPositionOnPostBack = False
                            CheckRights()
                            Exit Sub
                        End If

                        ''********
                        ''* Only users with valid email accounts can send an email.
                        ''********
                        If CurrentEmpEmail <> Nothing And ViewState("pSMRNo") <> 0 Then
                            If t.SelectedValue = "Rejected" And c.Text = Nothing Then
                                lblErrors.Text = "Your comments is required for Rejection."
                                lblErrors.Visible = True
                                lblReqAppComments.Text = "Your comments is required for Rejection."
                                lblReqAppComments.Visible = True
                                lblReqAppComments.ForeColor = Color.Red
                                Exit Sub
                            Else
                                ''*****************
                                ''History Tracking
                                ''*****************
                                PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), txtSampleDesc.Text, ViewState("iTeamMemberID"), ViewState("DefaultUser").ToUpper & " " & t.Text & " this record on behalf of " & ShortName.ToUpper & " - Comments: " & c.Text)

                                ''***********************************
                                ''Update Current Level Approver record.
                                ''***********************************
                                PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), TeamMemberID, True, t.SelectedValue, c.Text, hfSeqNo, ViewState("DefaultUser"), Date.Now)

                                ''************************
                                ''* Update Internal_Order_Request record
                                '*************************
                                PGMModule.UpdateSampleMtrlReqStatus(ViewState("pSMRNo"), IIf(ViewState("RecStatus") <> "Completed", "In Process", "Completed"), IIf(ViewState("RecStatus") <> "Completed", IIf(t.SelectedValue = "Rejected", "R", "T"), "C"), IIf(ddShipEDICoord.SelectedValue = Nothing, 0, ddShipEDICoord.SelectedValue), "", ViewState("DefaultUser"))

                                BindCriteriaAfterBind()
                                BindData(ViewState("pSMRNo"), 0)

                                If LastSeqNo = False And hfShipEDICoordEmail.Text <> Nothing _
                                    And ddShipEDICoord.SelectedValue <> Nothing _
                                    And t.SelectedValue <> "Rejected" Then

                                    Dim dsSE As DataSet = New DataSet
                                    dsSE = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, ddShipEDICoord.SelectedValue, False, False)
                                    If commonFunctions.CheckDataSet(dsSE) = True Then
                                        If IsDBNull(dsSE.Tables(0).Rows(0).Item("DateNotified")) Then
                                            ''*****************
                                            ''Update Ship/EDI Coordinator to routing list
                                            ''*****************
                                            PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), ddShipEDICoord.SelectedValue, False, "Pending", "", hfSeqNo, ViewState("DefaultUser"), Date.Now)

                                            ''*****************
                                            ''Notify the Shipping/EDI Coordinator
                                            ''*****************
                                            EmailTO &= hfShipEDICoordEmail.Text & ";"
                                            EmpName &= hfShipEdiCoordName.Text & ", "

                                        End If 'EOF  If IsDBNull(dsSE.Tables(0).Rows(0).Item("DateNotified")) Then
                                    End If 'EOF If commonFunctions.CheckDataSet(dsSE) = True Then
                                Else
                                    ''**************************************************************
                                    ''Locate Next Level Approver(s)
                                    ''**************************************************************
                                    ''Check at same sequence level
                                    ds1st = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), hfSeqNo, 0, True, False)
                                    If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                        ''Do not send email at same level twice.
                                    Else
                                        If t.SelectedValue <> "Rejected" Then 'Team Member Approved
                                            ds2nd = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), IIf(LastSeqNo = False, (hfSeqNo + 1), hfSeqNo), 0, True, False)
                                            If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                                                For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                                    If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                                    (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                                        EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                                        EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                                        ''*****************************************
                                                        ''Update Next Level Approvers DateNotified field.
                                                        ''*****************************************
                                                        PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", IIf(LastSeqNo = False, (hfSeqNo + 1), hfSeqNo), ViewState("DefaultUser"), Date.Now)

                                                    End If
                                                Next
                                            End If 'EOF ds2nd.Tables.Count > 0
                                        End If 'EOF t.SelectedValue <> "Rejected"
                                    End If 'EOF ds1st.Tables.Count > 0

                                End If 'EOF If LastSeqNo = False And hfShipEDICoordEmail.Text <> Nothing  Then

                                'Rejected or last approval
                                If t.SelectedValue = "Rejected" Or (LastSeqNo = True And t.SelectedValue = "Approved") Then
                                    ''********************************************************
                                    ''Notify Submitter if Rejected or last approval
                                    ''********************************************************
                                    dsRej = SecurityModule.GetTeamMember(ddRequestor.SelectedValue, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
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
                                        EmailCC &= hfAcctMgrEmail.Text & ";"
                                        EmailCC &= hfQEngrEmail.Text & ";"
                                        EmailCC &= hfPkgEmail.Text & ";"
                                    End If 'EOF  If LastSeqNo = True And ddStatus.SelectedValue = "Approved" Then

                                    EmailCC = CarbonCopyList(MyMessage, 0, "", 1, 0, EmailCC)
                                    EmailCC &= hfRequestorEmail.Text & "; "

                                    ''Test or Production Message display
                                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                                        MyMessage.Subject = "TEST: "
                                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                                    Else
                                        MyMessage.Subject = ""
                                        MyMessage.Body = ""
                                    End If

                                    MyMessage.Subject &= "Sample Material Request - " & txtSampleDesc.Text

                                    If t.SelectedValue = "Rejected" Then
                                        MyMessage.Subject &= " - REJECTED"
                                        MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName
                                        MyMessage.Body &= "<br/><br/>'" & txtSampleDesc.Text & "' was <font color='red'>REJECTED</font>. <a href='" & ViewState("strProdOrTestEnvironment") & "/PGM/SampleMaterialRequest.aspx?pSMRNo=" & ViewState("pSMRNo") & "'>Click here</a> to access the record.<br/><br/>Reason for rejection: <font color='red'>" & c.Text & "</font><br/><br/>" & "</font>"
                                    Else
                                        If LastSeqNo = True Then 'If last approval
                                            MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName
                                            MyMessage.Body &= "<p>'" & txtSampleDesc.Text & "' is available for your Review/Approval. <a href='" & ViewState("strProdOrTestEnvironment") & "/PGM/crSampleMtrlReqApproval.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>" & "</font>"
                                        End If
                                    End If

                                    EmailBody(MyMessage)

                                    'If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                                    '    MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                                    '    MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                                    '    EmailFrom = "Database.Notifications@ugnauto.com"
                                    '    EmailTO = "lynette.rey@ugnauto.com" 'CurrentEmpEmail
                                    'EmailCC = "lynette.rey@ugnauto.com"
                                    'End If

                                    ''**********************************
                                    ''Connect & Send email notification
                                    ''**********************************
                                    Try
                                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Sample Material Request", ViewState("pSMRNo"))
                                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                        lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."
                                    Catch ex As SmtpException
                                        lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                        lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                                        UGNErrorTrapping.InsertEmailQueue("Req#: " & ViewState("pSMRNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                    End Try
                                    lblErrors.Visible = True
                                    lblReqAppComments.Visible = True
                                    lblReqAppComments.ForeColor = Color.Red

                                    ''*****************
                                    ''History Tracking
                                    ''*****************
                                    If t.SelectedValue <> "Rejected" Then
                                        PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), txtSampleDesc.Text, ViewState("iTeamMemberID"), "Notification sent to " & EmpName)
                                    End If

                                End If 'EOF IF EmailTO <> Nothing Then
                            End If 'EOF If ReqFormFound = True Then
                        End If 'EOF If HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value <> Nothing Then

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pSMRNo"), 0)
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

    Protected Sub gvApprovers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApprovers.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Edit ImageButton
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
#End Region 'EOF Approval Status

#Region "Email Notifications"
    Protected Sub btnBuildApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuildApproval.Click
        btnSave1_Click(sender, e)
        BuildApprovalList()
        gvApprovers.DataBind()

        mvTabs.ActiveViewIndex = Int32.Parse(2)
        mvTabs.GetActiveView()
        mnuTabs.Items(2).Selected = True

    End Sub

    Public Function BuildApprovalList() As String
        Try
            ''********
            ''* This function is used to build the Approval List
            ''********

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If ViewState("pSMRNo") <> 0 And ((txtRoutingStatus.Text = "N") Or (txtRoutingStatus.Text = "R")) Then
                ''***************
                ''* Delete 1st Level Approval for rebuild
                ''***************
                PGMModule.DeleteSampleMtrlReqApproval(ViewState("pSMRNo"), 0)

                '***************
                '* Build Approval List
                '***************
                PGMModule.InsertSampleMtrlReqApproval(ViewState("pSMRNo"), ddUGNLocation.SelectedValue, ViewState("DefaultUser"), Date.Now)
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

    Public Function ConfirmRequiredFields() As Boolean
        Try
            ''***************
            ''Verify that atleast one Asset Expense Info entry has been entered before
            ''***************
            Dim dsExp As DataSet = New DataSet
            dsExp = PGMModule.GetSampleMtrlReqPartNo(ViewState("pSMRNo"), 0)
            If commonFunctions.CheckDataSet(dsExp) = False Then 'If missing kick user out from submission.
                mvTabs.ActiveViewIndex = Int32.Parse(0)
                mvTabs.GetActiveView()
                mnuTabs.Items(0).Selected = True
                lblErrors.Text = "Sample Information - Customer Part information is required."
                lblErrors.Visible = True
                lblErrors.Font.Size = 12
                MaintainScrollPositionOnPostBack = False
                Return False
            End If 'EOF If commonFunctions.CheckDataset(dsExp) = True

            If txtPkgReq.Text = Nothing Then
                mvTabs.ActiveViewIndex = Int32.Parse(1)
                mvTabs.GetActiveView()
                mnuTabs.Items(1).Selected = True
                lblErrors.Text = "Packaging Requirements - Requirements is a required field."
                lblErrors.Visible = True
                lblErrors.Font.Size = 12
                MaintainScrollPositionOnPostBack = False
                rfvPkgReq.Enabled = True
                Return False
            End If

            'dsExp = PGMModule.GetSampleMtrlReqDocuments(ViewState("pSMRNo"), 0, "P")
            'If commonFunctions.CheckDataSet(dsExp) = False Then 'If missing kick user out from submission.
            '    mvTabs.ActiveViewIndex = Int32.Parse(1)
            '    mvTabs.GetActiveView()
            '    mnuTabs.Items(1).Selected = True
            '    lblErrors.Text = "Attach a Packaging Layout."
            '    lblErrors.Visible = True
            '    lblErrors.Font.Size = 12
            '    MaintainScrollPositionOnPostBack = False
            '    Return False
            'End If 'EOF If commonFunctions.CheckDataset(dsExp) = True

            If ddShipMethod.SelectedValue = Nothing Then
                mvTabs.ActiveViewIndex = Int32.Parse(1)
                mvTabs.GetActiveView()
                mnuTabs.Items(1).Selected = True
                lblErrors.Text = "Delivery Instructions - Shipping Method is a required field."
                lblErrors.Visible = True
                lblErrors.Font.Size = 12
                MaintainScrollPositionOnPostBack = False
                rfvShipMethod.Enabled = True
                Return False
            End If

            If txtSpecInst.Text = Nothing Then
                mvTabs.ActiveViewIndex = Int32.Parse(1)
                mvTabs.GetActiveView()
                mnuTabs.Items(1).Selected = True
                lblErrors.Text = "Delivery Instructions - Special Instructions is a required field."
                lblErrors.Visible = True
                lblErrors.Font.Size = 12
                MaintainScrollPositionOnPostBack = False
                rfvSpecInst.Enabled = True
                Return False
            End If

            If txtLblReqComments.Text = Nothing Then
                mvTabs.ActiveViewIndex = Int32.Parse(1)
                mvTabs.GetActiveView()
                mnuTabs.Items(1).Selected = True
                lblErrors.Text = "Label Requirements - Comments is a required field."
                lblErrors.Visible = True
                lblErrors.Font.Size = 12
                MaintainScrollPositionOnPostBack = False
                rfvLblReqComments.Enabled = True
                Return False
            End If

            If txtInvPONO.Text = Nothing Then
                mvTabs.ActiveViewIndex = Int32.Parse(1)
                mvTabs.GetActiveView()
                mnuTabs.Items(1).Selected = True
                lblErrors.Text = "Invoice Information - Purchase Order # is a required field."
                lblErrors.Visible = True
                lblErrors.Font.Size = 12
                MaintainScrollPositionOnPostBack = False
                rfvInvPONO.Enabled = True
                Return False
            End If

            'dsExp = PGMModule.GetSampleMtrlReqDocuments(ViewState("pSMRNo"), 0, "I")
            'If commonFunctions.CheckDataSet(dsExp) = False Then 'If missing kick user out from submission.
            '    mvTabs.ActiveViewIndex = Int32.Parse(1)
            '    mvTabs.GetActiveView()
            '    mnuTabs.Items(1).Selected = True
            '    lblErrors.Text = "Attach the Purchase Order."
            '    lblErrors.Visible = True
            '    lblErrors.Font.Size = 12
            '    MaintainScrollPositionOnPostBack = False
            '    Return False
            'End If 'EOF If commonFunctions.CheckDataset(dsExp) = True

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
    End Function 'EOF ConfirmRequiredFields

    Protected Sub btnFwdApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFwdApproval.Click
        Try

            ''********
            ''* This function is used to submit email to the Approvers after the Project Sponsor is done.
            ''********
            Dim ds As DataSet = New DataSet
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
            If ViewState("DefaultUserEmail") IsNot Nothing Then
                CurrentEmpEmail = ViewState("DefaultUserEmail")
                EmailFrom = CurrentEmpEmail
                EmailCC = CurrentEmpEmail & ";"
            Else
                CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                EmailFrom = "Database.Notifications@ugnauto.com"
            End If

            '***************
            '* Build Approval List in the event the TM is submitting at a later date to capture TM's current status
            '***************
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
            If CurrentEmpEmail <> Nothing And ViewState("pSMRNo") <> 0 _
            And (txtRoutingStatus.Text = "N" Or txtRoutingStatus.Text = "R") Then
                Dim RequiredFieldsEntered As Boolean = ConfirmRequiredFields()
                If RequiredFieldsEntered = False Then
                    Exit Sub
                End If


                '**************
                '* Make sure that there is a level 1 approver before submission otherwise alert user
                '**************
                Dim Level1Found As Boolean = False
                ds = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, 0, False, False)
                If commonFunctions.CheckDataSet(ds) = True Then
                    For a = 0 To ds.Tables.Item(0).Rows.Count - 1
                        If ds.Tables(0).Rows(a).Item("SeqNo") = "1" Then
                            Level1Found = True
                        End If
                    Next
                End If

                If Level1Found = False Then
                    lblErrors.Text = "Level 1 reviewer is required prior to submission."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    lblReqAppComments.Text = "Level 1 reviewer is required prior to submission."
                    lblReqAppComments.Visible = True
                    lblReqAppComments.ForeColor = Color.Red
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                End If

                ''**********************
                ''* Save data prior to submission before approvals
                ''**********************
                UpdateRecord("In Process", "T", True)

                ''*******************************
                ''Locate 1st level Reviewer
                ''*******************************
                If (txtRoutingStatus.Text <> "R") Then
                    ds1st = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 1, 0, False, False)
                Else 'IF Rejected - only notify the TM who Rejected the record
                    ds1st = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, 0, False, True)
                End If

                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(ds1st) = True Then
                    For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                        If (ds1st.Tables(0).Rows(i).Item("Email").ToString.ToUpper <> CurrentEmpEmail.ToUpper) And _
                        (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                        (ddRequestor.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID")) Then

                            EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"
                            EmpName &= ds1st.Tables(0).Rows(i).Item("EmailTMName") & ", "

                            ''************************************************************
                            ''Update 1st level DateNotified field.
                            ''************************************************************
                            If (txtRoutingStatus.Text <> "R") Then
                                PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 1, ViewState("DefaultUser"), Date.Now)
                            Else 'IF Rejected - only notify the TM who Rejected the record
                                PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), ViewState("DefaultUser"), Date.Now)
                                SeqNo = ds1st.Tables(0).Rows(i).Item("SeqNo")
                                OrigTMID = ds1st.Tables(0).Rows(i).Item("OrigTeamMemberID")
                            End If
                        Else
                            ''************************************************************
                            ''1st Level Approver same as Requestor  Update record.ViewState("iTeamMemberID")
                            ''************************************************************
                            If (txtRoutingStatus.Text <> "R") Then
                                PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), True, "Approved", "", 1, ViewState("DefaultUser"), Date.Now)
                            Else 'IF Rejected - only notify the TM who Rejected the record
                                PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Approved", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), ViewState("DefaultUser"), Date.Now)
                                SeqNo = ds1st.Tables(0).Rows(i).Item("SeqNo")
                                OrigTMID = ds1st.Tables(0).Rows(i).Item("OrigTeamMemberID")
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

                    If (txtRoutingStatus.Text = "R") Then
                        ''********************************
                        ''Carbon Copy Same Level
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 0, "", IIf(SeqNo = 4, (SeqNo - 1), SeqNo), OrigTMID, EmailCC)
                    End If

                    If cbNotifyActMgr.Checked = True Then
                        EmailCC &= hfAcctMgrEmail.Text & ";"
                    End If

                    EmailCC &= hfQEngrEmail.Text & ";"

                    If cbNotifyPkgCoord.Checked = True Then
                        EmailCC &= hfPkgEmail.Text & ";"
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

                    MyMessage.Subject &= "Sample Material Request: " & txtSampleDesc.Text

                    MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName & "</font>"
                    MyMessage.Body &= "<p><font size='2' face='Tahoma'>The following request is available for your Review/Approval. <a href='" & ViewState("strProdOrTestEnvironment") & "/PGM/crSampleMtrlReqApproval.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pAprv=1" & "'>Click here</a> to access the record.</font></p>"

                    If (CDate(txtDueDate.Text).ToOADate - CDate(Date.Now).ToOADate) < 7 Then
                        MyMessage.Body &= "<p><font size='2' face='Tahoma'><b>Note:</b> This request is at or less than 1 week from the DUE DATE specified. Please confirm if you can make the due date in your approval, otherwise, reject this request with a reason.</font></p>"
                    End If

                    ''*******************
                    ''Build Email Body
                    ''*******************
                    EmailBody(MyMessage)

                    'If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                    '    MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                    '    MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                    '    EmailFrom = "Database.Notifications@ugnauto.com"
                    '    EmailTO = "lynette.rey@ugnauto.com" 'CurrentEmpEmail
                    '    EmailCC = "lynette.rey@ugnauto.com"
                    'End If

                    ''*****************
                    ''History Tracking
                    ''*****************
                    PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), txtSampleDesc.Text, ViewState("iTeamMemberID"), "Record completed and forwarded to " & EmpName & " for review/approval.")

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Sample Material Request", ViewState("pSMRNo"))
                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                        lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."

                    Catch ex As SmtpException
                        lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                        lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                        UGNErrorTrapping.InsertEmailQueue("SMRNo:" & ViewState("pSMRNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                        'get current event name
                        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                        'log and email error
                        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                    End Try

                End If 'EOF EmailTo <> Nothing
            Else
                lblErrors.Text = "This record is in Review/Approval status. No Further action needed."
                lblReqAppComments.Text = "This record is in Review/Approval status. No Further action needed."
            End If

            lblErrors.Visible = True
            lblReqAppComments.Visible = True
            lblReqAppComments.ForeColor = Color.Red

            ''**********************************
            ''Rebind the data to the form
            ''********************************** 
            BindData(ViewState("pSMRNo"), 0)
            gvApprovers.DataBind()

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

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

    Public Function CarbonCopyList(ByVal MyMessage As MailMessage, ByVal SubscriptionID As Integer, ByVal UGNLoc As String, ByVal SeqNo As Integer, ByVal RejectedTMID As Integer, ByVal EmailCC As String) As String
        Try
            Dim dsCC As DataSet = New DataSet
            Dim IncludeOrigAprvlTM As Boolean = False
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            If SeqNo = 0 Then 'No Rejections have been made, Send notification to all who applies
                If SubscriptionID = 0 Then ''Account Mananager
                    dsCC = PGMModule.GetSampleMtrlReqLead(ViewState("pSMRNo"))
                    IncludeOrigAprvlTM = True
                Else '' All others based on facility
                    If UGNLoc <> Nothing Then
                        dsCC = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(SubscriptionID, UGNLoc)
                        ' ''Else
                        ' ''    If SubscriptionID = 87 Or SubscriptionID = 84 Or SubscriptionID = 124 Or SubscriptionID = 80 Or SubscriptionID = 81 Or SubscriptionID = 82 Then
                        ' ''        ''Notify Accounting, CC List or 1st level IS or 1st level or 2nd level or 3rd level
                        ' ''        dsCC = commonFunctions.GetTeamMemberBySubscription(SubscriptionID)
                        ' ''    End If
                    End If
                End If

                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("TMID") <> ViewState("iTeamMemberID")) And _
                        (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                            EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"

                        End If
                    Next
                End If
            Else 'Notify same level approvers after a rejection has been released 
                dsCC = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), SeqNo, 0, False, False)
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
                dsCC = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, 0, False, False)
                'CC Orgiginal Approver in an event that their a corporate calendar default responsibility
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                        (ddRequestor.SelectedValue <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) Then ''change to ViewState("iTeamMemberID")   
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

    Public Function LinkLocationString() As String
        Dim LinkLocation As String = Nothing
        Select Case txtProjNo.Text.Substring(0, 1)
            Case "A"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjAssets.aspx?pProjNo=" & txtProjNo.Text & "' target='_blank'>" & txtProjNo.Text & " - " & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtProjNo.Text
                End If
            Case "D"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjDevelopment.aspx?pProjNo=" & txtProjNo.Text & "' target='_blank'>" & txtProjNo.Text & " - " & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtProjNo.Text
                End If
            Case "P"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjPackaging.aspx?pProjNo=" & txtProjNo.Text & "' target='_blank'>" & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtProjNo.Text
                End If
            Case "R"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjRepair.aspx?pProjNo=" & txtProjNo.Text & "' target='_blank'>" & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtProjNo.Text
                End If
            Case "T"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjTooling.aspx?pProjNo=" & txtProjNo.Text & "' target='_blank'>" & txtProjNo.Text & " - " & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtProjNo.Text
                End If
        End Select

        Return LinkLocation

    End Function 'EOF LinkLocationString

    Public Function EmailBody(ByVal MyMessage As MailMessage) As String

        MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px;  font-size: 13; font-family: Tahoma;'>"
        If ViewState("RecStatus") <> "Completed" And ViewState("RecStatus") <> "Void" Then
            MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>SAMPLE MATERIAL REQUEST</strong></td></tr>"
        End If
        MyMessage.Body &= "<tr>"

        MyMessage.Body &= "<table  style='font-size: 13; font-family: Tahoma;'>"
        If ViewState("RecStatus") = "Void" Then
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right'><b>Void Reason:</b>&nbsp;&nbsp;</td>"
            MyMessage.Body &= "<td style='width: 600px; '><font color='red'>" & txtVoidReason.Text & "</font></td>"
            MyMessage.Body &= "</tr>"
        End If
        If ViewState("RecStatus") = "Completed" And txtShippingComments.Text <> Nothing Then
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right'><b>Shipper/EDI Crd. Comments:</b>&nbsp;&nbsp;</td>"
            MyMessage.Body &= "<td style='width: 600px; '><font color='red'>" & txtShippingComments.Text & "</font></td>"
            MyMessage.Body &= "</tr>"
        End If
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' >Request #:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("pSMRNo") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Sample Description:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtSampleDesc.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Requested By:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddRequestor.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Due Date:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtDueDate.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddUGNLocation.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Customer:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddCustomer.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Trial Event:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddTrialEvent.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Formula:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtFormula.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Recovery Type:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddRecoveryType.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Production Level:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddProdLevel.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        If txtProjNo.Text <> Nothing Then
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right'>D Project No.:&nbsp;&nbsp; </td>"
            If txtProjectTitle.Text <> "" Then
                MyMessage.Body &= "<td>" & LinkLocationString() & "</td>"
            Else
                MyMessage.Body &= "<td>" & txtProjNo.Text & "</td>"
            End If
            MyMessage.Body &= "</tr>"
        End If
        MyMessage.Body &= "</table>"

        MyMessage.Body &= "</tr>"

        ''***************************************************
        ''Get list of Supporting Documentation
        ''***************************************************
        Dim dsSD As DataSet
        dsSD = PGMModule.GetSampleMtrlReqDocuments(ViewState("pSMRNo"), 0, "")
        If dsSD.Tables.Count > 0 And (dsSD.Tables.Item(0).Rows.Count > 0) Then
            MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB; width: 100%'>"
            MyMessage.Body &= "<td colspan='2'><strong>SUPPORTING DOCUMENT(S):</strong></td>"
            MyMessage.Body &= "</tr>"
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td colspan='2'>"
            MyMessage.Body &= "<table border='0' style='font-size: 13; font-family: Tahoma;'>"
            For i = 0 To dsSD.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= "<tr>"
                Select Case dsSD.Tables(0).Rows(i).Item("Section")
                    Case "P"
                        MyMessage.Body &= "<td height='25' class='p_text'>Packaging Requirement:&nbsp;&nbsp; </td>"
                    Case "D"
                        MyMessage.Body &= "<td height='25' class='p_text'>Delivery Instructions&nbsp;&nbsp; </td>"
                    Case "L"
                        MyMessage.Body &= "<td height='25' class='p_text'>Label Requirement:&nbsp;&nbsp; </td>"
                    Case "I"
                        MyMessage.Body &= "<td height='25' class='p_text'>Invoice Information:&nbsp;&nbsp; </td>"
                    Case "S"
                        MyMessage.Body &= "<td height='25' class='p_text'>Shipping Documents:&nbsp;&nbsp; </td>"
                    Case "A"
                        MyMessage.Body &= "<td height='25' class='p_text'>Addt'l Documentation:&nbsp;&nbsp; </td>"
                End Select
                MyMessage.Body &= "<td height='25'><a href='" & ViewState("strProdOrTestEnvironment") & "/PGM/SampleMtrlReqDocument.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pDocID=" & dsSD.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsSD.Tables(0).Rows(i).Item("Description") & "</a></td>"
                MyMessage.Body &= "</tr>"
            Next
            MyMessage.Body &= "</table>"
            MyMessage.Body &= "</tr>"
        End If

        ''***************************************************
        ''Get Shipping Information 
        ''***************************************************
        If ViewState("RecStatus") = "Completed" Then
            Dim dsSI As DataSet
            dsSI = PGMModule.GetSampleMtrlReqShipping(ViewState("pSMRNo"), 0)
            If dsSD.Tables.Count > 0 And (dsSD.Tables.Item(0).Rows.Count > 0) Then
                MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB; width: 100%'>"
                MyMessage.Body &= "<td colspan='2'><strong>SHIPPING INFORMATION:</strong></td>"
                MyMessage.Body &= "</tr>"
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td colspan='2'>"
                MyMessage.Body &= "<table border='0' style='font-size: 13; font-family: Tahoma; width: 60%' >"
                MyMessage.Body &= "  <tr>"
                MyMessage.Body &= "   <td ><b>Shipper No</b></td>"
                MyMessage.Body &= "   <td ><b>Total Shipping Cost (USD)</b></td>"
                MyMessage.Body &= "   <td ><b>Freight Bill ProNo</b></td>"
                MyMessage.Body &= "</tr>"
                For i = 0 To dsSI.Tables.Item(0).Rows.Count - 1
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td height='25'>" & dsSI.Tables(0).Rows(i).Item("ShipperNo") & "</td>"
                    MyMessage.Body &= "<td height='25'>$ " & Format(dsSI.Tables(0).Rows(i).Item("TotalShippingCost"), "#,##0.0000") & "</td>"
                    MyMessage.Body &= "<td height='25'>" & dsSI.Tables(0).Rows(i).Item("FreightBillProNo") & "</td>"
                    MyMessage.Body &= "</tr>"
                Next
                MyMessage.Body &= "</table>"
                MyMessage.Body &= "</tr>"
            End If
        End If 'EOF  If ViewState("RecStatus") = "Completed" Then

        MyMessage.Body &= "</table>"

        Return True

    End Function 'EOF EmailBody()

    Protected Sub btnSubmitCmplt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmitCmplt.Click
        Try
            ClearErrorMsgFlds()
            If ViewState("iTeamMemberID") = ViewState("iSETMID") Or _
                ViewState("iTeamMemberID") = ddShipEDICoord.SelectedValue Or _
                ViewState("iTeamMemberID") = 204 Then

                Dim dsSI As DataSet = New DataSet
                Dim TotalCost As Decimal = 0
                dsSI = PGMModule.GetSampleMtrlReqShipping(ViewState("pSMRNo"), 0)
                If commonFunctions.CheckDataSet(dsSI) = False Then
                    lblErrors.Text = "Shipping Information is required."
                    lblErrors.Visible = True
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                Else
                    TotalCost += dsSI.Tables(0).Rows(0).Item("TotalShippingCost")
                End If

                If txtShippingComments.Text = Nothing Then
                    lblErrors.Text = "Comments is required."
                    lblErrors.Visible = True
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                End If

                ''*****************
                ''Update Ship/EDI Coordinator to routing list
                ''*****************
                PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), ViewState("iSETMID"), True, "Approved", txtShippingComments.Text, 1, ViewState("DefaultUser"), Date.Now)

                ''************************
                ''* Update  record
                '*************************
                PGMModule.UpdateSampleMtrlReqStatus(ViewState("pSMRNo"), "Completed", "C", ddShipEDICoord.SelectedValue, txtShippingComments.Text, ViewState("DefaultUser"))

                ''*****************
                ''History Tracking
                ''*****************
                PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), txtSampleDesc.Text, ViewState("iTeamMemberID"), "Completed - Shipping Information. " & IIf(txtShippingComments.Text <> Nothing, "Comments: " & txtShippingComments.Text, Nothing))


                BindData(ViewState("pSMRNo"), 0)
                CheckRights()
                SendNotifWhenEventChanges("Completed")

                If ddShipMethod.SelectedValue = "Prepaid" And TotalCost > 0 Then
                    EmailFrieghtCompany(ViewState("DefaultUserEmail"), "UGNAuto@chrobinson.com")
                End If

            Else
                lblErrors.Text = "You do not have authorization to update Shipping Information."
                lblErrors.Visible = True
                MaintainScrollPositionOnPostBack = False
                Exit Sub
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
    End Sub 'EOF btnSubmitCmplt_Click

    Public Function EmailFrieghtCompany(ByVal EmailFrom As String, ByVal EmailTo As String) As String
        Try
            Dim EmailCC As String = "Ron.Sintkowski@ugnauto.com; " & EmailFrom & "; " & hfRequestorEmail.Text
            Dim SendFrom As MailAddress = New MailAddress(EmailFrom)
            Dim SendTo As MailAddress = New MailAddress(EmailFrom)
            Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

            'Test or Production Message display
            If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                MyMessage.Subject = "TEST: "
                MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
            Else
                MyMessage.Subject = ""
                MyMessage.Body = ""
                'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
            End If

            MyMessage.Subject &= "UGN Freight Information for Sample Material - " & txtSampleDesc.Text

            MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px;  font-size: 13; font-family: Tahoma;'>"
            MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>UGN FREIGHT INFORMATION FOR SAMPLE MATERIAL</strong></td></tr>"

            MyMessage.Body &= "<tr>"

            MyMessage.Body &= "<table  style='font-size: 13; font-family: Tahoma;'>"
            If txtShippingComments.Text <> Nothing Then
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td class='p_text' align='right'><b>Shipper/EDI Crd. Comments:</b>&nbsp;&nbsp;</td>"
                MyMessage.Body &= "<td style='width: 600px; '><font color='red'>" & txtShippingComments.Text & "</font></td>"
                MyMessage.Body &= "</tr>"
            End If
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right' >Request #:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td>" & ViewState("pSMRNo") & "</td>"
            MyMessage.Body &= "</tr>"
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right'>Sample Description:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td>" & txtSampleDesc.Text & "</td>"
            MyMessage.Body &= "</tr>"
            MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td>" & ddUGNLocation.SelectedItem.Text & "</td>"
            MyMessage.Body &= "</tr>"
            If txtProjNo.Text <> Nothing Then
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td class='p_text' align='right'>D Project No.:&nbsp;&nbsp; </td>"
                MyMessage.Body &= "<td>" & txtProjNo.Text & "</td>"
                MyMessage.Body &= "</tr>"
            End If
            MyMessage.Body &= "</table>"

            MyMessage.Body &= "</tr>"

            ''***************************************************
            ''Get Shipping Information 
            ''***************************************************
            Dim dsSI As DataSet
            dsSI = PGMModule.GetSampleMtrlReqShipping(ViewState("pSMRNo"), 0)
            If dsSI.Tables.Count > 0 And (dsSI.Tables.Item(0).Rows.Count > 0) Then
                MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB; width: 100%'>"
                MyMessage.Body &= "<td colspan='2'><strong>SHIPPING INFORMATION:</strong></td>"
                MyMessage.Body &= "</tr>"
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td colspan='2'>"
                MyMessage.Body &= "<table border='0' style='font-size: 13; font-family: Tahoma; width: 60%' >"
                MyMessage.Body &= "  <tr>"
                MyMessage.Body &= "   <td ><b>Shipper No</b></td>"
                MyMessage.Body &= "   <td ><b>Total Shipping Cost</b></td>"
                MyMessage.Body &= "   <td ><b>Freight Bill ProNo</b></td>"
                MyMessage.Body &= "</tr>"
                For i = 0 To dsSI.Tables.Item(0).Rows.Count - 1
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td height='25'>" & dsSI.Tables(0).Rows(i).Item("ShipperNo") & "</td>"
                    MyMessage.Body &= "<td height='25'>$ " & Format(dsSI.Tables(0).Rows(i).Item("TotalShippingCost"), "#,##0.0000") & "</td>"
                    MyMessage.Body &= "<td height='25'>" & dsSI.Tables(0).Rows(i).Item("FreightBillProNo") & "</td>"
                    MyMessage.Body &= "</tr>"
                Next
                MyMessage.Body &= "</table>"
                MyMessage.Body &= "</tr>"
            End If

            MyMessage.Body &= "</table>"

            'If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
            '    MyMessage.Body &= "<p>EmailTO: " & EmailTo & "</p>"
            '    MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
            '    EmailFrom = "Database.Notifications@ugnauto.com"
            '    EmailTo = EmailFrom '"lynette.rey@ugnauto.com" 
            '    EmailCC = "lynette.rey@ugnauto.com"
            'End If

            ''**********************************
            ''Connect & Send email notification
            ''**********************************
            Try
                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTo, EmailCC, "", "Sample Material Request", ViewState("pSMRNo"))
                lblErrors.Text = "Notification sent successfully."
            Catch ex As Exception
                lblErrors.Text = "Email Notification is queued for the next automated release."

                UGNErrorTrapping.InsertEmailQueue("Req#:" & ViewState("pSMRNo"), EmailFrom, EmailTo, EmailFrom & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
                'get current event name
                Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                'log and email error
                UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            End Try
            lblErrors.Visible = True
            lblErrors.Font.Size = 12
            MaintainScrollPositionOnPostBack = False

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
    End Function 'EOF EmailFrieghtCompany

    Public Sub SendNotifWhenEventChanges(ByVal EventDesc As String)
        ''********
        ''* This section will be used in the following methods:
        ''*     1) Email sent to all involved when an Asset is CAPITALIZED
        ''*     2) Email sent to all involved when the Estimated Completion Date changes with the Project Status is not Open
        ''*     3) Email sent to all involved with an Asset is VOID
        ''*     4) Email sent to Account with an Asset is COMPLETED
        ''********188 371 510 569
        Dim ds1st As DataSet = New DataSet
        Dim ds2nd As DataSet = New DataSet
        Dim dsCC As DataSet = New DataSet
        Dim EmpName As String = Nothing
        Dim EmailTO As String = Nothing
        Dim EmailCC As String = Nothing
        Dim EmailFrom As String = Nothing
        Dim GroupNotif As Boolean = False
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

        Try
            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pSMRNo") <> 0 Then
                ''Is this for a Group Notification or Individual
                Select Case EventDesc
                    Case "Void" 'Sent by Requestor, notify all
                        GroupNotif = True
                    Case "Completed" 'Sent by Shipping/EDI Coordinator, notify All
                        GroupNotif = False
                End Select


                ''*********************************
                ''Send Notification
                ''*********************************
                If GroupNotif = True Then
                    ''*******************************
                    ''Notify Approvers--include Plant Controllers and Ops Mgrs.
                    ''*******************************
                    ds1st = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, 0, False, False)
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(ds1st) = True Then
                        For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                            If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                            (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                            (ddRequestor.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID")) Then ''change to ViewState("iTeamMemberID")   

                                EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"

                            End If
                        Next
                    End If 'EOF Notify Approvers
                Else
                    ''********************************************************
                    ''Notify Project Lead
                    ''********************************************************
                    EmailTO &= hfRequestorEmail.Text & ";"

                End If
            End If  'EOF If CurrentEmpEmail <> Nothing And ViewState("pSMRNo") <> Nothing Then

            ''********************************************************
            ''Send Notification only if there is a valid Email Address
            ''********************************************************
            If EmailTO <> Nothing Then
                Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                ''*****************************
                ''Carbon Copy List
                ''*****************************
                If cbNotifyActMgr.Checked = True Then
                    EmailCC &= hfAcctMgrEmail.Text & ";"
                End If

                EmailCC &= hfQEngrEmail.Text & ";"

                If cbNotifyPkgCoord.Checked = True Then
                    EmailCC &= hfPkgEmail.Text & ";"
                End If

                If GroupNotif = False Then
                    EmailCC = CarbonCopyList(MyMessage, 0, "", 1, 0, EmailCC)
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

                MyMessage.Subject &= "Sample Material Request - " & txtSampleDesc.Text

                MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"

                MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>This Sample Material Request was '" & EventDesc.ToUpper & "' by " & ViewState("DefaultUserFullName") & ".</strong></td>"

                MyMessage.Body &= "</table>"

                EmailBody(MyMessage)


                'If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                '    MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                '    MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                '    EmailFrom = "Database.Notifications@ugnauto.com"
                '    EmailTO = "lynette.rey@ugnauto.com" 'CurrentEmpEmail
                '    EmailCC = "lynette.rey@ugnauto.com"
                'End If

                ''**********************************
                ''Connect & Send email notification
                ''**********************************
                Try
                    commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Sample Material Request", ViewState("pSMRNo"))
                    lblErrors.Text = "Notification sent successfully to all involved."
                Catch ex As Exception
                    lblErrors.Text &= "Email Notification is queued for the next automated release."

                    UGNErrorTrapping.InsertEmailQueue("Req#:" & ViewState("pSMRNo"), CurrentEmpEmail, EmailTO, CurrentEmpEmail & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
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
                BindData(ViewState("pSMRNo"), 0)

                ''*************************************************
                '' "Form Level Security using Roles &/or Subscriptions"
                ''*************************************************
                CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                If EventDesc = "Void" Then
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

#End Region 'EOF "Email Notifications"

    Protected Sub ddRecStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddRecStatus.SelectedIndexChanged, ddRecStatus2.SelectedIndexChanged
        If ddRecStatus.SelectedValue = "Void" Or ddRecStatus2.SelectedValue = "Void" Then
            txtVoidReason.Visible = True
            lblReqVoidRsn.Visible = True
            lblVoidReason.Visible = True
            rfvVoidReason.Enabled = True
            txtVoidReason.Focus()
        Else
            txtVoidReason.Visible = False
            lblReqVoidRsn.Visible = False
            lblVoidReason.Visible = False
            rfvVoidReason.Enabled = False
        End If
    End Sub 'EOF ddRecStatus_SelectedIndexChanged

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click

        Response.Redirect("SampleMaterialRequest.aspx?pSMRNo=&pCP=" & ViewState("pSMRNo"), False)

    End Sub 'EOF btnCopy_Click

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If txtRoutingStatus.Text <> "N" And txtRoutingStatus.Text <> "V" Then
            'User must void record first before deleting. This will trigger the email notification to all involved.
            lblErrors.Text = "Delete Cancelled... Please VOID the record first, save response and then delete."
            lblErrors.Visible = True
            lblErrors.Font.Size = 12
            MaintainScrollPositionOnPostBack = False
            Exit Sub
        Else
            PGMModule.DeleteSampleMtrlReq(ViewState("pSMRNo"))
            Response.Redirect("SampleMaterialRequestList.aspx", False)
        End If

    End Sub


End Class
