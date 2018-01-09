' ************************************************************************************************
' Name:	AR_Deduction.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 04/11/2012    LRey			Created .Net application
' 05/21/2013    LRey            Remove the auto email for any deductions less than $300
' 06/20/2013    LRey            Changed the "Closed" status to send the email when its only over $300
' 12/20/2013    LRey            Replaced Customer DDL to OEMManufacturer.
' ************************************************************************************************
Imports System.Threading
Imports System.Globalization
Partial Class AR_Deduction
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to define the primary record
            If HttpContext.Current.Request.QueryString("pARDID") <> "" Then
                ViewState("pARDID") = HttpContext.Current.Request.QueryString("pARDID")
            Else
                ViewState("pARDID") = 0
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

            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            If ViewState("pARDID") = Nothing Then
                m.ContentLabel = "New Operations Deduction Entry"
            Else
                m.ContentLabel = "Operations Deduction"
            End If

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pARDID") = Nothing Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Account Receivable</b> > <a href='AR_Deduction_List.aspx'><b>Operations Deduction Form Search</b></a> > New Operations Deduction Entry"
                Else
                    If ViewState("pAprv") = 0 Then
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Account Receivable</b> > <a href='AR_Deduction_List.aspx'><b>Operations Deduction Form Search</b></a> > Operations Deduction"
                    Else 'Go Back To approval Screen
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Account Receivable</b> > <a href='AR_Deduction_List.aspx'><b>Operations Deduction Form Search</b></a> > <a href='crARDeductionApproval.aspx?pARDID=" & ViewState("pARDID") & "&pAprv=1'><b>Approval</b></a> > Operations Deduction"
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
            ctl = m.FindControl("ARExtender")
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
                If ViewState("pARDID") <> 0 Then
                    BindData(ViewState("pARDID"))
                Else
                    BindData(0)
                    ddReason.Focus()
                    ' txtDateSubmitted.Text = Date.Now
                End If

                If ViewState("pSD") > 0 Then
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

            txtComments.Attributes.Add("onkeypress", "return tbLimit();")
            txtComments.Attributes.Add("onkeyup", "return tbCount(" + lblCommentsChar.ClientID + ");")
            txtComments.Attributes.Add("maxLength", "500")

            txtVoidReason.Attributes.Add("onkeypress", "return tbLimit();")
            txtVoidReason.Attributes.Add("onkeyup", "return tbCount(" + lblVoidReasonChar.ClientID + ");")
            txtVoidReason.Attributes.Add("maxLength", "300")

            txtReply.Attributes.Add("onkeypress", "return tbLimit();")
            txtReply.Attributes.Add("onkeyup", "return tbCount(" + lblReplyChar.ClientID + ");")
            txtReply.Attributes.Add("maxLength", "300")

            txtFileDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtFileDesc.Attributes.Add("onkeyup", "return tbCount(" + lblFileDescChar.ClientID + ");")
            txtFileDesc.Attributes.Add("maxLength", "200")

            Dim strPreviewClientScript As String = "javascript:void(window.open('crViewARDeduction.aspx?pARDID=" & ViewState("pARDID") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
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
            ViewState("Admin") = False
            ViewState("ObjectRole") = False

            btnAdd.Enabled = False
            btnSaveDetail.Enabled = False
            btnResetDetail.Enabled = False
            btnUpload.Enabled = False
            btnResetUpload.Enabled = False
            btnSaveCB.Enabled = True
            btnResetCB.Enabled = True
            btnDelete.Enabled = False
            btnPreview.Enabled = False
            btnFwdApproval.Enabled = False
            btnBuildApproval.Enabled = False
            btnBuildApproval.Visible = False
            ddRecStatus.Enabled = False
            ddSubmittedBy.Enabled = False
            uploadFile.Enabled = False
            mnuTabs.Items(0).Enabled = True
            mnuTabs.Items(1).Enabled = False
            mnuTabs.Items(2).Enabled = False
            mnuTabs.Items(3).Enabled = False
            gvSupportingDocument.Columns(3).Visible = False
            gvApprovers.Columns(7).Visible = False
            gvApprovers.Columns(8).Visible = False
            gvApprovers.Columns(9).Visible = False
            gvApprovers.ShowFooter = False
            gvQuestion.Columns(0).Visible = True

            SDExtender.Collapsed = False

            If ddReason.SelectedItem.Text = "Other" Then
                lblReqComments.Visible = True
                rfvComments.Enabled = True
            Else
                lblReqComments.Visible = False
                rfvComments.Enabled = False
            End If

            ''** Record Status
            Dim RecStatus As String = Nothing
            If txtRoutingStatus.Text = "N" Or txtRoutingStatus.Text = "A" Or txtRoutingStatus.Text = "C" Or txtRoutingStatus.Text = Nothing Then
                RecStatus = ddRecStatus.SelectedValue
                ddRecStatus.Visible = True
                ddRecStatus2.Visible = False
            Else
                RecStatus = ddRecStatus2.SelectedValue
                ddRecStatus.Visible = False
                ddRecStatus2.Visible = True
            End If
            ViewState("RecStatus") = RecStatus

            If RecStatus = "Void" Then
                lblReqVoidRsn.Visible = True
                lblVoidRsn.Visible = True
                txtVoidReason.Visible = True
                txtVoidReason.Enabled = True
                rfvVoidReason.Enabled = True
            Else
                lblReqVoidRsn.Visible = False
                lblVoidRsn.Visible = False
                txtVoidReason.Visible = False
                txtVoidReason.Enabled = False
                rfvVoidReason.Enabled = False
            End If

            If RecStatus = "Closed" Or RecStatus = "Closed @60 days" Then
                lblReqCreditDebitDate.Visible = True
                lblCreditDebitDate.Visible = True
                txtCreditDebitDate.Visible = True
                txtCreditDebitDate.Enabled = True
                rfvCDD.Enabled = True
                imgCDD.Visible = True

                lblReqCreditDebitMemo.Visible = True
                lblCreditDebitMemo.Visible = True
                txtCreditDebitMemo.Visible = True
                txtCreditDebitMemo.Enabled = True
                rfvCDM.Enabled = True
            Else
                lblReqCreditDebitMemo.Visible = False
                lblCreditDebitMemo.Visible = False
                txtCreditDebitMemo.Visible = False
                txtCreditDebitMemo.Enabled = False
                rfvCDM.Enabled = False

                lblReqCreditDebitDate.Visible = False
                lblCreditDebitDate.Visible = False
                txtCreditDebitDate.Visible = False
                txtCreditDebitDate.Enabled = False
                rfvCDD.Enabled = False
                imgCDD.Visible = False
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
            Dim iFormID As Integer = 132 'Operations Deduction Form ID
            Dim iRoleID As Integer = 0
            Dim i As Integer = 0
            ViewState("DefaultUserFacility") = Nothing

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            'dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
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
                                            btnSaveDetail.Enabled = True
                                            btnResetDetail.Enabled = True
                                            btnBuildApproval.Enabled = True
                                            btnBuildApproval.Visible = True

                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pARDID") = 0 Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                            Else
                                                ViewState("Admin") = True
                                                Select Case RecStatus
                                                    Case "Open"
                                                        btnBuildApproval.Enabled = True
                                                        btnBuildApproval.Visible = True
                                                        btnFwdApproval.Enabled = True
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "R") Then
                                                            btnFwdApproval.Enabled = True
                                                        ElseIf txtRoutingStatus.Text = "T" Then
                                                            gvApprovers.Columns(7).Visible = True
                                                        End If
                                                        ddRecStatus.Enabled = True
                                                    Case "Approved"
                                                        ddRecStatus.Enabled = True
                                                    Case "Closed"
                                                        ddRecStatus.Enabled = True
                                                        lblReqCreditDebitDate.Visible = True
                                                        lblCreditDebitDate.Visible = True
                                                        txtCreditDebitDate.Visible = True
                                                        txtCreditDebitDate.Enabled = True

                                                        lblReqCreditDebitMemo.Visible = True
                                                        lblCreditDebitMemo.Visible = True
                                                        txtCreditDebitMemo.Visible = True
                                                        txtCreditDebitMemo.Enabled = True
                                                    Case "Closed @60 days"
                                                        ddRecStatus2.Enabled = True
                                                        lblReqCreditDebitDate.Visible = True
                                                        lblCreditDebitDate.Visible = True
                                                        txtCreditDebitDate.Visible = True
                                                        txtCreditDebitDate.Enabled = True

                                                        lblReqCreditDebitMemo.Visible = True
                                                        lblCreditDebitMemo.Visible = True
                                                        txtCreditDebitMemo.Visible = True
                                                        txtCreditDebitMemo.Enabled = True
                                                    Case "Void"
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidRsn.Visible = True
                                                        lblReqVoidRsn.Visible = True
                                                End Select
                                                If iTeamMemberID = 204 Then
                                                    btnFwdApproval.Enabled = True
                                                End If
                                                btnAdd.Enabled = True
                                                btnDelete.Enabled = True
                                                btnPreview.Enabled = True
                                                btnSaveCB.Enabled = True
                                                btnResetCB.Enabled = True
                                                uploadFile.Enabled = True
                                                btnUpload.Enabled = True
                                                btnResetUpload.Enabled = True
                                                gvSupportingDocument.Columns(3).Visible = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                            End If
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Project Leader
                                            ViewState("ObjectRole") = True
                                            btnSaveDetail.Enabled = True
                                            btnResetDetail.Enabled = True
                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pARDID") = 0 Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                            Else
                                                ViewState("Admin") = True
                                                Select Case RecStatus
                                                    Case "Open"
                                                        btnBuildApproval.Enabled = True
                                                        btnBuildApproval.Visible = True
                                                        btnFwdApproval.Enabled = True
                                                        btnDelete.Enabled = True
                                                        uploadFile.Enabled = True
                                                        btnUpload.Enabled = True
                                                        btnResetUpload.Enabled = True
                                                        gvSupportingDocument.Columns(3).Visible = True
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "R") Then
                                                            btnFwdApproval.Enabled = True
                                                            uploadFile.Enabled = True
                                                            btnUpload.Enabled = True
                                                            btnResetUpload.Enabled = True
                                                            gvSupportingDocument.Columns(3).Visible = True
                                                            btnSaveCB.Enabled = True
                                                            btnResetCB.Enabled = True
                                                        ElseIf txtRoutingStatus.Text = "T" Then
                                                            btnUpload.Enabled = True
                                                            uploadFile.Enabled = True
                                                            gvSupportingDocument.Columns(3).Visible = True
                                                            btnSaveCB.Enabled = True
                                                            btnResetCB.Enabled = True
                                                        End If
                                                        ddRecStatus.Enabled = True
                                                    Case "Approved"
                                                        ddRecStatus.Enabled = True
                                                        'ddRecStatus.Items.RemoveAt(0)
                                                        btnDelete.Enabled = False
                                                        btnSaveCB.Enabled = True
                                                        btnResetCB.Enabled = True
                                                        btnUpload.Enabled = True
                                                        uploadFile.Enabled = True
                                                        gvSupportingDocument.Columns(3).Visible = True
                                                        btnSaveCB.Enabled = True
                                                    Case "Closed"
                                                        'If txtRoutingStatus.Text <> "C" Then
                                                        btnSaveDetail.Enabled = True
                                                        btnResetDetail.Enabled = True
                                                        ddRecStatus.Enabled = True
                                                        'Else
                                                        'ddRecStatus.Enabled = False
                                                        'btnSaveDetail.Enabled = False
                                                        'btnResetDetail.Enabled = False
                                                        'End If
                                                        uploadFile.Enabled = True
                                                        btnUpload.Enabled = True
                                                        btnResetUpload.Enabled = True
                                                        gvSupportingDocument.Columns(3).Visible = True
                                                        btnDelete.Enabled = False
                                                    Case "Closed @60 days"
                                                        'If txtRoutingStatus.Text <> "6" Then
                                                        btnSaveDetail.Enabled = True
                                                        btnResetDetail.Enabled = True
                                                        ddRecStatus.Enabled = True
                                                        'Else
                                                        'ddRecStatus2.Enabled = False
                                                        'btnSaveDetail.Enabled = False
                                                        'btnResetDetail.Enabled = False
                                                        'End If
                                                        uploadFile.Enabled = True
                                                        btnUpload.Enabled = True
                                                        btnResetUpload.Enabled = True
                                                        gvSupportingDocument.Columns(3).Visible = True
                                                        btnDelete.Enabled = False
                                                    Case "Void"
                                                        If txtRoutingStatus.Text <> "V" Then
                                                            btnSaveDetail.Enabled = True
                                                            btnResetDetail.Enabled = True
                                                            ddRecStatus.Visible = True
                                                        Else
                                                            ddRecStatus.Enabled = False
                                                            ddRecStatus2.Enabled = False
                                                            btnSaveDetail.Enabled = False
                                                            btnResetDetail.Enabled = False
                                                        End If
                                                        btnDelete.Enabled = False
                                                End Select
                                                btnAdd.Enabled = True
                                                btnPreview.Enabled = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                            End If
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            '** Used by Approvers & Backup persons
                                            If ViewState("pARDID") = 0 Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                            Else
                                                ViewState("ObjectRole") = False
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                btnPreview.Enabled = True
                                                Select Case RecStatus
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "T") Then
                                                            gvApprovers.Columns(7).Visible = True
                                                            btnSaveCB.Enabled = True
                                                            btnResetCB.Enabled = True
                                                        End If
                                                End Select
                                            End If
                                            uploadFile.Enabled = True
                                            btnUpload.Enabled = True
                                            btnResetUpload.Enabled = True
                                            gvSupportingDocument.Columns(3).Visible = True
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            ViewState("ObjectRole") = False
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                            mnuTabs.Items(3).Enabled = True
                                            btnPreview.Enabled = True
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ''Used by the Accounting Group
                                            ViewState("ObjectRole") = False
                                            ViewState("Admin") = True

                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pARDID") = 0 Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                            Else
                                                Select Case RecStatus
                                                    Case "In Process"
                                                        gvApprovers.Columns(9).Visible = True
                                                    Case "Void"
                                                        If txtRoutingStatus.Text <> "V" Then
                                                            btnSaveDetail.Enabled = True
                                                            btnResetDetail.Enabled = True
                                                        End If
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidRsn.Visible = True
                                                        lblReqVoidRsn.Visible = True
                                                    Case "Approved"
                                                        btnSaveDetail.Enabled = True
                                                        btnResetDetail.Enabled = True
                                                        ddRecStatus.Enabled = True
                                                    Case "Closed"
                                                        If txtRoutingStatus.Text <> "C" Then
                                                            btnSaveDetail.Enabled = True
                                                            btnResetDetail.Enabled = True
                                                            ddRecStatus.Enabled = True
                                                        Else
                                                            ddRecStatus2.Enabled = False
                                                        End If
                                                        lblReqCreditDebitDate.Visible = True
                                                        lblCreditDebitDate.Visible = True
                                                        txtCreditDebitDate.Visible = True
                                                        txtCreditDebitDate.Enabled = True

                                                        lblReqCreditDebitMemo.Visible = True
                                                        lblCreditDebitMemo.Visible = True
                                                        txtCreditDebitMemo.Visible = True
                                                        txtCreditDebitMemo.Enabled = True
                                                    Case "Closed @60 days"
                                                        If txtRoutingStatus.Text <> "6" Then
                                                            btnSaveDetail.Enabled = True
                                                            btnResetDetail.Enabled = True
                                                            ddRecStatus2.Enabled = True
                                                        Else
                                                            ddRecStatus2.Enabled = False
                                                        End If
                                                        lblReqCreditDebitDate.Visible = True
                                                        lblCreditDebitDate.Visible = True
                                                        txtCreditDebitDate.Visible = True
                                                        txtCreditDebitDate.Enabled = True

                                                        lblReqCreditDebitMemo.Visible = True
                                                        lblCreditDebitMemo.Visible = True
                                                        txtCreditDebitMemo.Visible = True
                                                        txtCreditDebitMemo.Enabled = True
                                                End Select
                                                btnAdd.Enabled = True
                                                btnPreview.Enabled = True
                                                uploadFile.Enabled = True
                                                btnUpload.Enabled = True
                                                btnResetUpload.Enabled = True
                                                gvSupportingDocument.Columns(3).Visible = True
                                                btnSaveCB.Enabled = True
                                                btnResetCB.Enabled = True
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
                ddSubmittedBy.DataSource = ds
                ddSubmittedBy.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddSubmittedBy.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddSubmittedBy.DataBind()
                ddSubmittedBy.Items.Insert(0, "")
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
            ddSubmittedBy.SelectedValue = IIf(ddSubmittedBy.SelectedValue = Nothing, HttpContext.Current.Session("UserId"), ddSubmittedBy.SelectedValue)
            ddTeamMember.SelectedValue = IIf(ddTeamMember.SelectedValue = Nothing, HttpContext.Current.Session("UserId"), ddTeamMember.SelectedValue)

            'bind existing data to drop down UGN Location control for selection criteria for search
            ds = commonFunctions.GetUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Customer control for selection criteria for search
            'ds = commonFunctions.GetCustomer(False)
            ds = commonFunctions.GetOEMManufacturer("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Category control for selection criteria for search
            ds = ARGroupModule.GetARDeductionReason("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddReason.DataSource = ds
                ddReason.DataTextField = ds.Tables(0).Columns("ddReasonDesc").ColumnName.ToString()
                ddReason.DataValueField = ds.Tables(0).Columns("RID").ColumnName.ToString()
                ddReason.DataBind()
                ddReason.Items.Insert(0, "")
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

    Public Sub BindData(ByVal ARDID As String)
        Dim ds As DataSet = New DataSet
        Dim ds2 As DataSet = New DataSet
        Try
            ds = ARGroupModule.GetARDeduction(ARDID, "", 0, "", "", "", "", "", "", 0, "", "", "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                If ViewState("pARDID") = 0 Then
                    lblARDID.Text = "?"
                    ddRecStatus.SelectedValue = "Open"
                Else
                    lblARDID.Text = ds.Tables(0).Rows(0).Item("ARDID").ToString()

                    Select Case ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                        Case "N"
                            ddRecStatus.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                        Case "A"
                            ddRecStatus.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                        Case "C"
                            ddRecStatus.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                        Case "T"
                            ddRecStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                        Case "R"
                            ddRecStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                        Case "V"
                            ddRecStatus.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                        Case "6"
                            ddRecStatus2.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                    End Select

                    txtRoutingStatus.Text = ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                    lblRoutingStatusDesc.Text = ds.Tables(0).Rows(0).Item("RoutingStatusDesc").ToString()
                    lblRoutingStatusDesc.Visible = True

                    ddSubmittedBy.SelectedValue = ds.Tables(0).Rows(0).Item("SubmittedByTMID").ToString()
                    txtDateSubmitted.Text = ds.Tables(0).Rows(0).Item("DateSubmitted").ToString()
                    ddUGNFacility.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                    txtDeductionAmount.Text = ds.Tables(0).Rows(0).Item("DeductionAmount").ToString()
                    ddCustomer.SelectedValue = ds.Tables(0).Rows(0).Item("ddCustomerValue").ToString()
                    txtIncidentDate.Text = ds.Tables(0).Rows(0).Item("IncidentDate").ToString()
                    txtReferenceNo.Text = ds.Tables(0).Rows(0).Item("ReferenceNo").ToString()
                    txtPartNo.Text = ds.Tables(0).Rows(0).Item("PartNo").ToString()
                    ddReason.SelectedValue = ds.Tables(0).Rows(0).Item("Reason").ToString()
                    hdDefaultNotify.Text = ds.Tables(0).Rows(0).Item("defaultnotify").ToString()
                    txtComments.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                    txtVoidReason.Text = ds.Tables(0).Rows(0).Item("VoidReason").ToString()
                    txtCreditDebitDate.Text = ds.Tables(0).Rows(0).Item("CreditDebitDate").ToString()
                    txtCreditDebitMemo.Text = ds.Tables(0).Rows(0).Item("CreditDebitMemo").ToString()
                End If

                ''Bind Communication Board
                If ViewState("pRID") <> 0 Then
                    ds = ARGroupModule.GetARDeductionRSS(ViewState("pARDID"), ViewState("pRID"))
                    If commonFunctions.CheckDataSet(ds) = True Then
                        txtQC.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                    Else 'no record found reset query string pRptID
                        Response.Redirect("AR_Deduction.aspx?pARDID=" & ViewState("pARDID") & "&pRID=0&pRC=1", False)
                    End If
                End If

                ds = ARGroupModule.GetARDeductionCntrMsr(ViewState("pARDID"))
                If commonFunctions.CheckDataSet(ds) = True Then
                    txtCM.Text = ds.Tables(0).Rows(0).Item("CounterMeasure").ToString()
                    txtPostDate.Text = ds.Tables(0).Rows(0).Item("PostDate").ToString()
                    txtResolution.Text = ds.Tables(0).Rows(0).Item("Resolution").ToString()
                    txtClosedDate.Text = ds.Tables(0).Rows(0).Item("ClosedDate").ToString()
                End If
            End If 'EOF If commonFunctions.CheckDataSet(ds) = True Then
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
        Response.Redirect("AR_Deduction.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnSaveDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDetail.Click
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            Dim RecStatus As String = Nothing
            RecStatus = ViewState("RecStatus")

            '*************************************************************
            '* If the RecStatus = "Closed" locate most recent supporting document attachment
            '*************************************************************
            If RecStatus = "Closed" Then
                Dim dsExp As DataSet = New DataSet
                Dim ReqSupDocFound As Boolean = False
                dsExp = ARGroupModule.GetARDeductionDocuments(ViewState("pARDID"), 0, True)
                If commonFunctions.CheckDataSet(dsExp) = False Then 'If missing kick user out from submission.
                    ' If Date.Today() <> dsExp.Tables(0).Rows(0).Item("DateOfUpload").ToString Then
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True

                    ReqSupDocFound = True

                    lblErrors.Text = "Supporting Document(s) is required for Closing this record."
                    lblErrors.Visible = True
                    lblMessageView4.Text = "Supporting Document(s) is required for Closing this record."
                    lblMessageView4.Visible = True

                    Exit Sub
                    'End If 'EOF If commonFunctions.CheckDataset(dsExp) = True
                End If
            End If

            If (ViewState("pARDID") <> 0) Then
                Dim SendEmailToDefaultAdmin As Boolean = False

                '***************
                '* Update Data
                '***************
                UpdateRecord(RecStatus, IIf(RecStatus = "Closed @60 days", "6", IIf(RecStatus = "Closed", "C", IIf(RecStatus = "Void", "V", IIf(RecStatus = "Open", "N", txtRoutingStatus.Text)))), False)

                ''*******************
                ''Build Approval List
                ''*******************
                If txtDateSubmitted.Text = Nothing Then
                    BuildApprovalList()
                End If


                ''*************
                ''Check for Void status, send email notfication 
                ''*************
                If RecStatus = "Void" And txtRoutingStatus.Text <> "V" Then
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Void")
                    End If
                ElseIf RecStatus = "Closed" And txtRoutingStatus.Text <> "C" And txtDeductionAmount.Text >= 300 Then
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Closed")
                    End If
                ElseIf RecStatus = "Closed @60 days" And txtRoutingStatus.Text <> "6" Then
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Closed @60 days")
                        '*******
                        'Auto-approve remaining pending approvals when closed a@60 days to prevent an overwrite
                        ARGroupModule.UpdateARDeductionApproval(ViewState("pARDID"), 0, True, "Approved", "Closed @60 days without approval", 0, 0, DefaultUser, DefaultDate)
                        gvApprovers.DataBind()

                    End If
                End If

                '**************
                '* Reload the data
                '**************
                BindData(ViewState("pARDID"))

            Else 'New Record
               
                '***************
                '* Save Data
                '***************
                ARGroupModule.InsertARDeduction(ddSubmittedBy.SelectedValue, ddUGNFacility.SelectedValue, IIf(txtDeductionAmount.Text = Nothing, 0, txtDeductionAmount.Text), ddCustomer.SelectedValue, txtReferenceNo.Text, txtIncidentDate.Text, ddReason.SelectedValue, txtComments.Text, "Open", txtPartNo.Text, DefaultUser, DefaultDate)

                '***************
                '* Locate Last ARDID entry
                '***************
                Dim ds As DataSet = Nothing
                ds = ARGroupModule.GetLastARDeductionRecNo(ddSubmittedBy.SelectedValue, ddReason.SelectedValue, ddUGNFacility.SelectedValue, ddCustomer.SelectedValue, "Open", DefaultUser, DefaultDate)

                ViewState("pARDID") = ds.Tables(0).Rows(0).Item("LastARDID").ToString

                ''*****************
                ''History Tracking
                ''*****************
                ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, "Record created.", "", "", "", "")

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
                Response.Redirect("AR_Deduction.aspx?pARDID=" & ViewState("pARDID") & Aprv, False)
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
    End Sub 'EOF btnSaveDetail_Click

    Public Function UpdateRecord(ByVal RecStatus As String, ByVal RoutingStatus As String, ByVal RecSubmitted As Boolean) As String
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

            ARGroupModule.UpdateARDeduction(ViewState("pARDID"), ddSubmittedBy.SelectedValue, ddUGNFacility.SelectedValue, IIf(txtDeductionAmount.Text = Nothing, 0, txtDeductionAmount.Text), ddCustomer.SelectedValue, txtReferenceNo.Text, txtIncidentDate.Text, ddReason.SelectedValue, txtComments.Text, RecStatus, RoutingStatus, IIf(RecSubmitted = False, txtDateSubmitted.Text, DefaultDate), txtVoidReason.Text, txtCreditDebitDate.Text, txtCreditDebitMemo.Text, txtPartNo.Text, DefaultUser, DefaultDate)

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

    Protected Sub btnResetDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetDetail.Click, btnResetUpload.Click, btnResetCB.Click

        lblErrors.Text = Nothing
        lblErrors.Visible = False

        'Redirect user to the same tab view when Reset button was clicked
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "pAprv=1"
        End If

        Dim TempViewState As Integer
        If ViewState("pARDID") <> 0 Then
            TempViewState = mvTabs.ActiveViewIndex
            mvTabs.GetActiveView()
            mnuTabs.Items(TempViewState).Selected = True

            BindData(ViewState("pARDID"))
        Else
            Response.Redirect("AR_Deduction.aspx" & Aprv, False)
        End If

    End Sub 'EOF btnResetDetail_Click

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            '***************
            '* Delete Record
            '***************
            ARGroupModule.DeleteARDeduction(ViewState("pARDID"))

            '***************
            '* Redirect user back to the search page.
            '***************
            Response.Redirect("AR_Deduction_List.aspx", False)

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

    Protected Sub ddReason_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddReason.SelectedIndexChanged

        Select Case ddReason.SelectedItem.Text
            Case "Other"
                lblReqComments.Visible = True
                rfvComments.Enabled = True
            Case Else
                lblReqComments.Visible = False
                rfvComments.Enabled = False
        End Select

        ddUGNFacility.Focus()

    End Sub 'EOF ddReason_SelectedIndexChanged

    Protected Sub ddRecStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddRecStatus.SelectedIndexChanged

        Select Case ddRecStatus.SelectedValue
            Case "Void"
                txtVoidReason.Visible = True
                lblReqVoidRsn.Visible = True
                lblVoidRsn.Visible = True
                rfvVoidReason.Enabled = True
            Case "Closed"
                lblReqCreditDebitDate.Visible = True
                lblCreditDebitDate.Visible = True
                txtCreditDebitDate.Visible = True
                txtCreditDebitDate.Enabled = True
                rfvCDD.Enabled = True

                lblReqCreditDebitMemo.Visible = True
                lblCreditDebitMemo.Visible = True
                txtCreditDebitMemo.Visible = True
                txtCreditDebitMemo.Enabled = True
                rfvCDM.Enabled = True
            Case Else
                lblReqVoidRsn.Visible = False
                lblVoidRsn.Visible = False
                rfvVoidReason.Enabled = False

                lblReqCreditDebitDate.Visible = False
                lblCreditDebitDate.Visible = False
                txtCreditDebitDate.Visible = False
                txtCreditDebitDate.Enabled = False
                rfvCDD.Enabled = False

                lblReqCreditDebitMemo.Visible = False
                lblCreditDebitMemo.Visible = False
                txtCreditDebitMemo.Visible = False
                txtCreditDebitMemo.Enabled = False
                rfvCDM.Enabled = False
        End Select
    End Sub 'eof ddRecStatus_SelectedIndexChanged

    Protected Sub ddRecStatus2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddRecStatus2.SelectedIndexChanged
        Select Case ddRecStatus2.SelectedValue
            Case "Void"
                txtVoidReason.Visible = True
                lblReqVoidRsn.Visible = True
                lblVoidRsn.Visible = True
                rfvVoidReason.Enabled = True
            Case "Closed @60 days"
                lblReqCreditDebitDate.Visible = True
                lblCreditDebitDate.Visible = True
                txtCreditDebitDate.Visible = True
                txtCreditDebitDate.Enabled = True
                rfvCDD.Enabled = True

                lblReqCreditDebitMemo.Visible = True
                lblCreditDebitMemo.Visible = True
                txtCreditDebitMemo.Visible = True
                txtCreditDebitMemo.Enabled = True
                rfvCDM.Enabled = True
            Case Else
                lblReqVoidRsn.Visible = False
                lblVoidRsn.Visible = False
                rfvVoidReason.Enabled = False

                lblReqCreditDebitDate.Visible = False
                lblCreditDebitDate.Visible = False
                txtCreditDebitDate.Visible = False
                txtCreditDebitDate.Enabled = False
                rfvCDD.Enabled = False

                lblReqCreditDebitMemo.Visible = False
                lblCreditDebitMemo.Visible = False
                txtCreditDebitMemo.Visible = False
                txtCreditDebitMemo.Enabled = False
                rfvCDM.Enabled = False
        End Select
    End Sub

    Protected Sub txtPartNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPartNo.TextChanged
        'Validate that the part number enter is a true part number found in Partno_by_OEM  and/or Future_PartNo
        lblErrors.Text = Nothing
        lblErrors.Visible = False
        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetPartNo(txtPartNo.Text, "", "", "", "")
        If commonFunctions.CheckDataSet(ds) = False Then 'If missing kick user out from submission.
            lblErrors.Text = txtPartNo.Text & " is not a valid Part Number. Please Try again."
            lblErrors.Visible = True
            MaintainScrollPositionOnPostBack = "false"
            txtPartNo.Text = Nothing
        End If
    End Sub 'EOF txtPartNo_TextChanged
#End Region 'EOF "General - Detail"

#Region "Communication Board"
    Public Function GoToCommunicationBoard(ByVal ARDID As String, ByVal RSSID As String, ByVal ApprovalLevel As Integer, ByVal TeamMemberID As Integer) As String
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If
        Return "AR_Deduction.aspx?pARDID=" & ARDID & "&pAL=" & ApprovalLevel & "&pTMID=" & TeamMemberID & "&pRID=" & RSSID & "&pRC=1" & Aprv
    End Function 'EOF GoToCommunicationBoard

    Protected Sub btnSaveCB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCB.Click
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
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

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
                If ViewState("pARDID") <> Nothing Then
                    ''*************************************************************************
                    ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                    ''*************************************************************************
                    Dim dsExp As DataSet = New DataSet

                    ''***************************************************************
                    ''Send Reply back to requestor
                    ''***************************************************************
                    ds = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), 0, TMID, False, False)
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
                        ''Carbon Copy Project Leader
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)


                        ''***************************************************************
                        ''Carbon Copy Previous Levels
                        ''***************************************************************
                        ds2CC = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), SeqNo, TMID, False, False)
                        ''Check that the recipient(s) is a valid Team Member
                        If ds2CC.Tables.Count > 0 And (ds2CC.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To ds2CC.Tables(0).Rows.Count - 1
                                If (ds2CC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                    If (ds2CC.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds2CC.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                        If EmailCC = Nothing Then
                                            EmailCC = ds2CC.Tables(0).Rows(i).Item("Email")
                                        Else
                                            EmailCC = EmailCC & ";" & ds2CC.Tables(0).Rows(i).Item("Email")
                                        End If
                                    End If
                                End If
                            Next
                        End If 'EOF  If ds.Tables.Count > 0

                        ''Test or Production Message display
                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            'MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                        End If

                        MyMessage.Subject &= "Operations Deduction for " & ddReason.SelectedItem.Text & " (Rec# " & ViewState("pARDID") & ") - MESSAGE RECIEVED"

                        MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
                        MyMessage.Body &= " <tr>"
                        MyMessage.Body &= "     <td valign='top' width='20%'>"
                        MyMessage.Body &= "         <img src='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/images/messanger60.jpg'/>"
                        MyMessage.Body &= "     </td>"
                        MyMessage.Body &= "     <td valign='top'>"
                        MyMessage.Body &= "         <b>Attention:</b> " & EmpName
                        MyMessage.Body &= "             <p><b>" & DefaultUserFullName & "</b> sent a message regarding Operations Deduction "
                        MyMessage.Body &= "             <font color='red'>(Rec#" & ViewState("pARDID") & ") " & ddReason.SelectedItem.Text & "</font>."
                        MyMessage.Body &= "         <br/><br/><i>Question:&nbsp;&nbsp;</i><b>" & txtQC.Text & "</b>"
                        MyMessage.Body &= "         <br/><br/><i>Response:&nbsp;&nbsp;</i><b>" & txtReply.Text & "</b><br/><br/>"
                        MyMessage.Body &= "         </p>"
                        MyMessage.Body &= "         <p><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/crARDeductionApproval.aspx?pARDID=" & ViewState("pARDID") & "&pAprv=1" & "'>Click here</a> to return to the approval page."
                        MyMessage.Body &= "     </td>"
                        MyMessage.Body &= " </tr>"
                        MyMessage.Body &= "</table>"

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
                            EmailTO = CurrentEmpEmail '"lynette.rey@ugnauto.com"
                            EmailCC = "lynette.rey@ugnauto.com"
                        End If

                        ''*****************
                        ''History Tracking
                        ''*****************
                        ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, "Message Sent", "", "", "", "")

                        ''**********************************
                        ''Save Reponse to child table
                        ''**********************************
                        ARGroupModule.InsertARDeductionRSSReply(ViewState("pARDID"), ViewState("pRID"), ddReason.SelectedValue, DefaultTMID, txtReply.Text)

                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "AR_Deduction", ViewState("pARDID"))
                            lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                            lblErrors.Visible = True
                        Catch ex As Exception
                            lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                            lblErrors.Visible = True

                            UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pARDID"), CurrentEmpEmail, EmailTO, CurrentEmpEmail & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
                            'get current event name
                            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                            'log and email error
                            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                        End Try

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pARDID"))
                        gvQuestion.DataBind()

                    Else 'EmailTO = ''
                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pARDID"))

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
    End Sub 'EOF btnSaveCB_Click

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim RSSID As Integer
            Dim drRSSID As AR.AR_Deduction_RSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, AR.AR_Deduction_RSSRow)

            If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                RSSID = drRSSID.RSSID
                ' Reference the rpCBRC ObjectDataSource
                Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                ' Set the RID Parameter value
                rpCBRC.SelectParameters("ARDID").DefaultValue = drRSSID.ARDID.ToString()
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
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblMessageView4.Text = Nothing
            lblMessageView4.Visible = False

            If ViewState("pARDID") <> "" Then
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
                                ARGroupModule.InsertARDeductionDocuments(ViewState("pARDID"), ddTeamMember.SelectedValue, txtFileDesc.Text, SupportingDocBinaryFile, uploadFile.FileName, SupportingDocEncodeType, SupportingDocFileSize)
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
                    Dim price As AR.AR_Deduction_DocumentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, AR.AR_Deduction_DocumentsRow)

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
            Response.Redirect("AR_Deduction.aspx?pARDID=" & ViewState("pARDID") & "&pSD=1" & Aprv, False)
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

    Protected Sub gvApprovers_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Try
            Dim row As GridViewRow = gvApprovers.Rows(e.RowIndex)
            If row IsNot Nothing Then
                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                Dim DefaultDate As Date = Date.Now

                Dim t As DropDownList = TryCast(row.FindControl("ddStatus"), DropDownList)
                Dim c As TextBox = TryCast(row.FindControl("txtAppComments"), TextBox)
                Dim tm As TextBox = TryCast(row.FindControl("txtTeamMemberID"), TextBox)
                Dim TeamMemberID As Integer = CType(tm.Text, Integer)
                Dim s As TextBox = TryCast(row.FindControl("hfSeqNo"), TextBox)
                Dim hfSeqNo As Integer = CType(s.Text, Integer)
                Dim ds As DataSet = New DataSet


                If (t.Text <> "Pending") Then
                    If (c.Text <> Nothing Or c.Text <> "") Then
                        ds = SecurityModule.GetTeamMember(TeamMemberID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                        Dim ShortName As String = ds.Tables(0).Rows(0).Item("ShortName").ToString()

                        ''*****************
                        ''History Tracking
                        ''*****************
                        ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, (DefaultUser.ToUpper) & " " & t.Text & " this record on behalf of " & ShortName.ToUpper & " - Comments: " & c.Text, "", "", "", "")

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
                            EmailFrom = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                            EmailCC = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
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
                            If ViewState("pARDID") <> Nothing Then
                                If t.SelectedValue = "Rejected" And c.Text = Nothing Then
                                    lblErrors.Text = "Your comments is required for Disagreement."
                                    lblErrors.Visible = True
                                Else 'BUILD EMAIL
                                    ''*******************************************************************
                                    ''*Verify that atleast one Supporting Document entry is entered
                                    ''*******************************************************************
                                    Dim dsExp As DataSet = New DataSet
                                    dsExp = ARGroupModule.GetARDeductionDocuments(ViewState("pARDID"), 0, False)
                                    If (dsExp.Tables.Item(0).Rows.Count = 0) Then 'If missing kick user out from submission.
                                        mvTabs.ActiveViewIndex = Int32.Parse(1)
                                        mvTabs.GetActiveView()
                                        mnuTabs.Items(1).Selected = True

                                        lblErrors.Text = "Atleast one Supporting Document is required for submission."
                                        lblErrors.Visible = True
                                        lblReqAppComments.Text = "Atleast one Supporting Document is required for submission."
                                        lblReqAppComments.Visible = True
                                        lblReqAppComments.ForeColor = Color.Red
                                        MaintainScrollPositionOnPostBack = "false"
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
                                                NextLvl = 134
                                            Case 2
                                                If txtDeductionAmount.Text >= 2501 Then
                                                    SeqNo = 2
                                                    NextSeqNo = 3
                                                    NextLvl = 135
                                                End If
                                            Case 3
                                                If txtDeductionAmount.Text >= 2501 Then
                                                    SeqNo = 3
                                                    NextSeqNo = 4
                                                    NextLvl = 136
                                                End If
                                            Case 4
                                                If txtDeductionAmount.Text >= 10001 Then
                                                    SeqNo = 4
                                                    NextSeqNo = 5
                                                    NextLvl = 143
                                                End If
                                            Case 5
                                                If txtDeductionAmount.Text >= 10001 Then
                                                    SeqNo = 5
                                                    NextSeqNo = 0
                                                    NextLvl = 143
                                                End If
                                        End Select

                                        ''***********
                                        ''* Verify that Row selected Team Member Sequence No is Last to Approve
                                        ''***********
                                        Dim dsLast As DataSet = New DataSet
                                        Dim r As Integer = 0
                                        Dim LastSeqNo As Boolean = False
                                        Dim totalPending As Integer = 0

                                        dsLast = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), 0, 0, False, False)
                                        If commonFunctions.CheckDataSet(dsLast) = True Then
                                            For r = 0 To dsLast.Tables.Item(0).Rows.Count - 1
                                                If dsLast.Tables(0).Rows(r).Item("Status") = "Pending" Then
                                                    totalPending = totalPending + 1
                                                End If
                                            Next
                                            If (totalPending - 1) = 0 Then
                                                LastSeqNo = True
                                            Else
                                                LastSeqNo = False
                                            End If
                                        End If

                                        ''**********************
                                        ''* Save data prior to submission before approvals
                                        ''**********************
                                        UpdateRecord(IIf(LastSeqNo = True, IIf(t.SelectedValue = "Rejected", "In Process", "Approved"), "In Process"), IIf(LastSeqNo = True, IIf(t.SelectedValue = "Rejected", "T", "A"), IIf(t.SelectedValue = "Rejected", "T", "T")), False)

                                        ''***********************************
                                        ''Update Current Level Approver record.
                                        ''***********************************
                                        ARGroupModule.UpdateARDeductionApproval(ViewState("pARDID"), TeamMemberID, True, t.SelectedValue, c.Text, SeqNo, 0, DefaultUser, DefaultDate)

                                        ''*******************************
                                        ''Locate Next Approver
                                        ''*******************************
                                        ''Check at same sequence level
                                        ds1st = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), SeqNo, 0, True, False)
                                        If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                            ''Do not send email at same level twice.
                                        Else
                                            ds2nd = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), IIf(SeqNo < 4, (SeqNo + 1), SeqNo), 0, True, False)
                                            If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                                                For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                                    If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                                        If (ds2nd.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) And (ddSubmittedBy.SelectedValue <> ds2nd.Tables(0).Rows(i).Item("TeamMemberID")) Then ''change to DefaultTMID   
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
                                                            ARGroupModule.UpdateARDeductionApproval(ViewState("pARDID"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", IIf(LastSeqNo = False, (SeqNo + 1), SeqNo), 0, DefaultUser, DefaultDate)
                                                        End If
                                                    End If
                                                Next
                                                ''***Logic needs to follow here with an else when all approved notification goes to cc
                                            End If 'EOF ds2nd.Tables.Count > 0 
                                            ''********************************************************
                                            ''Notify Requestor if last approval
                                            ''********************************************************
                                            If (LastSeqNo = True And t.SelectedValue = "Approved") Then
                                                ''********************************************************
                                                ''Notify Project Lead
                                                ''********************************************************
                                                dsRej = ARGroupModule.GetARDeductionLead(ViewState("pARDID"))
                                                ''Check that the recipient(s) is a valid Team Member
                                                If dsRej.Tables.Count > 0 And (dsRej.Tables.Item(0).Rows.Count > 0) Then
                                                    For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                                        If (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True) Or (dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
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
                                        End If 'EOF ds1st.Tables.Count > 0

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
                                                If SeqNo = 2 Then
                                                    ''**************************************************************
                                                    ''Carbon Copy 2nd Level Approvers
                                                    ''**************************************************************
                                                    EmailCC = CarbonCopyList(MyMessage, 0, "", 2, 0, EmailCC, DefaultTMID)
                                                End If

                                                If SeqNo = 3 Then
                                                    ''**************************************************************
                                                    ''Carbon Copy 3rd Level Approvers
                                                    ''**************************************************************
                                                    EmailCC = CarbonCopyList(MyMessage, 0, "", 3, 0, EmailCC, DefaultTMID)
                                                End If

                                                If SeqNo = 4 Then
                                                    ''**************************************************************
                                                    ''Carbon Copy 3rd Level Approvers
                                                    ''**************************************************************
                                                    EmailCC = CarbonCopyList(MyMessage, 0, "", 4, 0, EmailCC, DefaultTMID)
                                                End If

                                                If SeqNo = 5 Then
                                                    ''**************************************************************
                                                    ''Carbon Copy 3rd Level Approvers
                                                    ''**************************************************************
                                                    EmailCC = CarbonCopyList(MyMessage, 0, "", 5, 0, EmailCC, DefaultTMID)
                                                End If

                                                If LastSeqNo = True Then
                                                    ''********************************
                                                    '*Carbon Copy CC List Cost Accountants
                                                    ''********************************
                                                    EmailCC = CarbonCopyList(MyMessage, 138, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)
                                                End If
                                            Else
                                                If SeqNo = 1 Then
                                                    EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), ddUGNFacility.SelectedValue, 1, 0, EmailCC, DefaultTMID)
                                                    If txtDeductionAmount.Text > 2500 And t.SelectedValue = "Rejected" Then
                                                        ''CC 2nd level if rejected
                                                        EmailCC = CarbonCopyList(MyMessage, 0, "", 3, 0, EmailCC, DefaultTMID)
                                                        EmailCC = CarbonCopyList(MyMessage, 0, "", 4, 0, EmailCC, DefaultTMID)
                                                    End If
                                                Else
                                                    EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), "", 0, 0, EmailCC, DefaultTMID)
                                                End If
                                            End If

                                            ''**************************************************************
                                            ''Carbon Copy Originator
                                            ''**************************************************************
                                            EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                                            'Test or Production Message display
                                            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                                                MyMessage.Subject = "TEST: "
                                                'MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE.<br/><br/>"
                                            Else
                                                MyMessage.Subject = ""
                                                MyMessage.Body = ""
                                                'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE.<br/><br/>"
                                            End If

                                            MyMessage.Subject &= "Operations Deduction for " & ddReason.SelectedItem.Text & " (Rec# " & ViewState("pARDID") & ")"

                                            ''If t.SelectedValue = "Rejected" Then
                                            ''    MyMessage.Subject &= " - REJECTED"
                                            ''    MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName & "</font>"
                                            ''    MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & "This Operations Deduction for '" & ddReason.SelectedItem.Text & "' was <font color='red'>REJECTED: " & c.Text & "</font> "
                                            ''Else
                                            If SeqNo = 4 Then
                                                ''MyMessage.Subject &= "- APPROVED"
                                                ''Redirect users to Preview Form at final Approval
                                                MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & "This Operations Deduction for '" & ddReason.SelectedItem.Text & "' was reviewed by all team members. "
                                            Else
                                                MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName & "</font>"
                                                ''Redirect users to Approval screen if not final approval
                                                MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & "This Operations Deduction is for '" & ddReason.SelectedItem.Text & "' is available for your Review/Approval. "
                                            End If
                                            ''End If

                                            ''*****************
                                            ''Build Email body
                                            ''*****************
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
                                                EmailTO = CurrentEmpEmail '"lynette.rey@ugnauto.com"
                                                EmailCC = "lynette.rey@ugnauto.com"
                                            End If

                                            ''**********************************
                                            ''Connect & Send email notification
                                            ''**********************************
                                            Try
                                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "AR_Deduction", ViewState("pARDID"))
                                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                                lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."
                                            Catch ex As Exception
                                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                                lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."

                                                UGNErrorTrapping.InsertEmailQueue("Deduction Rec No:" & ViewState("pARDID"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                                'get current event name
                                                Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                                                'log and email error
                                                'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                                            End Try
                                            lblErrors.Visible = True
                                            lblReqAppComments.Visible = True
                                            lblReqAppComments.ForeColor = Color.Red

                                            ''*****************
                                            ''History Tracking
                                            ''*****************
                                            If t.SelectedValue <> "Rejected" Then
                                                If SeqNo = 4 Then
                                                    ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, "Notification sent to all involved.", "", "", "", "")
                                                Else
                                                    ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, "Notification sent to level " & IIf(SeqNo < 4, (SeqNo + 1), SeqNo) & " TM(s): " & EmpName, "", "", "", "")
                                                End If
                                            Else
                                                ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, "Notification sent to " & EmpName, "", "", "", "")
                                            End If

                                        End If
                                    End If
                                End If
                            End If
                        End If

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pARDID"))
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
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br/>" & mb.Name

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
                    Dim price As AR.AR_Deduction_ApprovalRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, AR.AR_Deduction_ApprovalRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record """ & """ for " & DataBinder.Eval(e.Row.DataItem, "TeamMemberName") & "?');")
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
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim TempViewState As Integer

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
                If ViewState("pARDID") <> Nothing Then

                    If (txtRoutingStatus.Text = "N") Or (txtRoutingStatus.Text = "R") Or (txtRoutingStatus.Text = Nothing) Then
                        ''***************
                        ''* Delete 1st Level Approval for rebuild
                        ''***************
                        ARGroupModule.DeleteARDeductionApproval(ViewState("pARDID"), 0, 0)

                        '***************
                        '* Build Approval List
                        '***************
                        ARGroupModule.InsertARDeductionApproval(ViewState("pARDID"), ddUGNFacility.SelectedValue, DefaultUser, DefaultDate)
                        gvApprovers.DataBind()
                        TempViewState = mvTabs.ActiveViewIndex
                        mvTabs.GetActiveView()
                        mnuTabs.Items(TempViewState).Selected = True

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
        Return True
    End Function

    Protected Sub btnFwdApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFwdApproval.Click
        Try
            ''********
            ''* This function is used to submit email to the Approvers after the Project Sponsor is done.
            ''********
            Dim DefaultDate As Date = Date.Now
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
            Dim SeqNo As Integer = 0
            Dim OrigTMID As Integer = 0

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

            '*********************************************************************************
            '* Rebuild list in case tm decides to submit the record after the date of creation
            '*********************************************************************************
            If txtDateSubmitted.Text = Nothing Then
                BuildApprovalList()
            End If

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pARDID") <> Nothing Then
                    ''***************
                    ''Verify that atleast one Supporting Document entry has been entered before
                    ''***************
                    Dim dsExp As DataSet = New DataSet
                    Dim ReqSupDocFound As Boolean = False
                    dsExp = ARGroupModule.GetARDeductionDocuments(ViewState("pARDID"), 0, False)
                    If commonFunctions.CheckDataSet(dsExp) = False Then 'If missing kick user out from submission.
                        mvTabs.ActiveViewIndex = Int32.Parse(1)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(1).Selected = True

                        ReqSupDocFound = True

                        lblErrors.Text = "Atleast one Supporting Document is required for submission."
                        lblErrors.Visible = True
                        lblReqAppComments.Text = "Atleast one Supporting Document is required for submission."
                        lblReqAppComments.Visible = True
                        lblReqAppComments.ForeColor = Color.Red
                        MaintainScrollPositionOnPostBack = "false"
                        Exit Sub
                    End If 'EOF If commonFunctions.CheckDataset(dsExp) = True

                    If txtIncidentDate.Text = Nothing And txtDeductionAmount.Text <= 300 Then
                        lblReqIncidentDt.Visible = True
                        rfvIncidentDate.Enabled = True

                        lblErrors.Text = "Incident Date is required at less than or equal to $300 deduction amount."
                        lblErrors.Visible = True
                        lblReqAppComments.Text = "Incident Date is required at less than or equal to $300 deduction amount."
                        lblReqAppComments.Visible = True
                        lblReqAppComments.ForeColor = Color.Red
                        MaintainScrollPositionOnPostBack = "false"
                        mvTabs.ActiveViewIndex = Int32.Parse(0)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(0).Selected = True
                        Exit Sub
                    End If

                    ''**********************
                    ''* Save data prior to submission before approvals
                    ''**********************
                    UpdateRecord(IIf(txtDeductionAmount.Text > 300, "In Process", "Approved"), IIf(txtDeductionAmount.Text > 300, "T", "A"), True)

                    ''*********************************
                    ''Send Notification to Approvers
                    ''*********************************
                    If ReqSupDocFound = False Then
                        ''*******************************
                        ''Locate 1st level approver
                        ''*******************************
                        If txtDeductionAmount.Text > 300 Then
                            ds1st = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), 1, 0, False, False)
                        Else
                            ds1st = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), 0, 0, False, False) 
                        End If
                        ''Check that the recipient(s) is a valid Team Member
                        If commonFunctions.CheckDataSet(ds1st) = True Then
                            For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                                If (ds1st.Tables(0).Rows(i).Item("Email").ToString.ToUpper <> CurrentEmpEmail.ToUpper) And (ds1st.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And (ddSubmittedBy.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID")) Then ''change to DefaultTMID   
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

                                    ''************************************************************
                                    ''Update 1st level DateNotified field.
                                    ''************************************************************
                                    If txtDeductionAmount.Text > 300 Then
                                        ARGroupModule.UpdateARDeductionApproval(ViewState("pARDID"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 1, 0, DefaultUser, DefaultDate)
                                    Else
                                        ARGroupModule.UpdateARDeductionApproval(ViewState("pARDID"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), True, "Approved", "", 0, 0, DefaultUser, DefaultDate)
                                    End If 'EOF   If txtDeductionAmount.Text > 300 Then
                                Else
                                    ''************************************************************
                                    ''1st Level Approver same as Project Sponsor.  Update record.DefaultTMID
                                    ''************************************************************
                                    ARGroupModule.UpdateARDeductionApproval(ViewState("pARDID"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), True, "Approved", "", 1, 1, DefaultUser, DefaultDate)

                                    If (ds1st.Tables(0).Rows(i).Item("SubmitFlag") = True) Then
                                        SponsSameAs1stLvlAprvr = True
                                    End If

                                End If 'EOF IF (ds1st.Tables(0).Rows(i).Item("Email") <> Nothing) And 
                            Next 'EOF For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                        End If 'EOF If commonFunctions.CheckDataset(ds1st) = True 

                        ''***************************************************************
                        ''Locate 2nd Level Approver(s)
                        ''***************************************************************
                        If SponsSameAs1stLvlAprvr = True And EmailTO = Nothing Then
                            ds2nd = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), 2, 0, False, False)
                            ''Check that the recipient(s) is a valid Team Member
                            If commonFunctions.CheckDataSet(ds2nd) = True Then
                                For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                    If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                        If (ds2nd.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then
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
                                            ''************************************************************
                                            ''Update 2nd level DateNotified field.
                                            ''************************************************************
                                            If txtDeductionAmount.Text > 300 Then
                                                ARGroupModule.UpdateARDeductionApproval(ViewState("pARDID"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 2, 0, DefaultUser, DefaultDate)
                                            End If 'EOF   If txtDeductionAmount.Text > 100 Then
                                        End If
                                    End If
                                Next
                            End If 'EOF IF commonFunctions.CheckDataset(ds2nd) = True 
                        End If 'EOF If SponsSameAs1stLvlAprvr = True Then
                    End If

                    ''********************************************************
                    ''Send Notification only if there is a valid Email Address
                    ''********************************************************
                    If EmailTO <> Nothing Then
                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                        ''********************************
                        '*Carbon Copy CC List Cost Accountants
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 138, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)


                        ''*********************************
                        ''Carbon Copy 2nd & 3rd level
                        ''*********************************
                        If txtDeductionAmount.Text > 2500 Then
                            EmailCC = CarbonCopyList(MyMessage, 0, "", 3, OrigTMID, EmailCC, DefaultTMID)
                            EmailCC = CarbonCopyList(MyMessage, 0, "", 4, OrigTMID, EmailCC, DefaultTMID)
                        End If

                        ''Test or Production Message display
                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            'MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                            'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
                        End If

                        MyMessage.Subject &= "Operations Deduction Form for " & ddReason.SelectedItem.Text & " (Rec# " & ViewState("pARDID") & ")"
                        MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName & "</font>"
                        If txtDeductionAmount.Text > 300 Then
                            MyMessage.Body &= "<p><font size='2' face='Tahoma'>This Operations Deduction is for '" & ddReason.SelectedItem.Text & "' is available for your Review/Approval. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/crARDeductionApproval.aspx?pARDID=" & ViewState("pARDID") & "&pAprv=1" & "'>Click here</a> to access the record.</font></p>"
                        Else
                            MyMessage.Body &= "<p><font size='2' face='Tahoma'>This Operations Deduction is for '" & ddReason.SelectedItem.Text & "' is available for Review only. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/crViewARDeduction.aspx?pARDID=" & ViewState("pARDID") & "&pAprv=1" & "'>Click here</a> to access the record.</font></p>"
                        End If

                        ''*******************
                        ''Build Email Body
                        ''*******************
                        EmailBody(MyMessage)

                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                            MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                            EmailTO = "lynette.rey@ugnauto.com" ' CurrentEmpEmail
                            EmailCC = "lynette.rey@ugnauto.com"
                        End If

                        ''*****************
                        ''History Tracking
                        ''*****************
                        If txtDeductionAmount.Text > 300 Then
                            ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, "Record completed and forwarded to " & EmpName & " for review.", "", "", "", "")

                            ''**********************************
                            ''Connect & Send email notification
                            ''**********************************
                            Try
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "AR_Deduction", ViewState("pARDID"))

                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."

                                mvTabs.ActiveViewIndex = Int32.Parse(2)
                                mvTabs.GetActiveView()
                                mnuTabs.Items(2).Selected = True

                            Catch ex As SmtpException
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."

                                UGNErrorTrapping.InsertEmailQueue("Deduction Rec No:" & ViewState("pARDID"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                'get current event name
                                Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                                'log and email error
                                'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                            End Try
                        Else
                            'LRey 05/21/2013 - Do not send notification if less than 300
                            'ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, "Record completed and forwarded to " & EmpName & " for review only.", "", "", "", "")
                            'Record only the auto completion
                            ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, "Default Approval by Initiator - Deduction Amount is $" & txtDeductionAmount.Text, "", "", "", "")

                            lblErrors.Text = "No email notification required for less than $300. Your submission has been recorded."
                            lblReqAppComments.Text = "No email notification required for less than $300.Your submission has been recorded."
                        End If
                        lblErrors.Visible = True
                        lblReqAppComments.Visible = True
                        lblReqAppComments.ForeColor = Color.Red

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pARDID"))
                        gvApprovers.DataBind()

                        ''*************************************************
                        '' "Form Level Security using Roles &/or Subscriptions"
                        ''*************************************************
                        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                    Else
                        If ViewState("DefaultUserFacility") = "UT" Then
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
                    dsCC = ARGroupModule.GetARDeductionLead(ViewState("pARDID"))
                    IncludeOrigAprvlTM = True
                Else '' All others based on facility
                    If UGNLoc <> Nothing Then
                        dsCC = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(SubscriptionID, UGNLoc)
                    Else
                        If SubscriptionID = 134 Or SubscriptionID = 135 Or SubscriptionID = 136 Or SubscriptionID = 138 Or SubscriptionID = 143 Or SubscriptionID = 149 Then
                            ''Notify 1st level, 2nd level, 3rd level, 4th Level, 5th level or CC List
                            dsCC = commonFunctions.GetTeamMemberBySubscription(SubscriptionID)
                            IncludeOrigAprvlTM = True
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
                dsCC = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), SeqNo, 0, False, False)
                'Carbon Copy pending approvers at same level as who rejected the record.
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And (RejectedTMID <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) Then
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
                dsCC = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), 0, 0, False, False)
                'CC Orgiginal Approver in an event that their a corporate calendar default responsibility
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And (ddSubmittedBy.SelectedValue <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) Then ''change to DefaultTMID   
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

        MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>PROJECT OVERVIEW</strong></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Rec No:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("pARDID") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Submitted By:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddSubmittedBy.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddUGNFacility.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Deduction Amount ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtDeductionAmount.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Customer:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddCustomer.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Reference No:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtReferenceNo.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        If txtIncidentDate.Text <> Nothing Then
            MyMessage.Body &= "<td class='p_text' align='right'>Incident Date:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td>" & txtIncidentDate.Text & "</td>"
        Else
            MyMessage.Body &= "<td class='p_text' align='right' style='color:Red'>Incident Date:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td style='color:Red'>Requires an entry by one of the team member's listed above.</td>"
        End If
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Reason for Deduction:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td style='width: 700px;'>" & ddReason.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Comments:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td style='width: 700px;'>" & txtComments.Text & "</td>"
        MyMessage.Body &= "</tr>"

        ''***************************************************
        ''Get list of Supporting Documentation
        ''***************************************************
        Dim dsAED As DataSet
        dsAED = ARGroupModule.GetARDeductionDocuments(ViewState("pARDID"), 0, False)
        If commonFunctions.CheckDataSet(dsAED) = True Then
            MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
            MyMessage.Body &= "<td colspan='2'><strong>SUPPORTING DOCUMENT(S):</strong></td>"
            MyMessage.Body &= "</tr>"
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td colspan='2'>"
            MyMessage.Body &= "<table border='0' style='font-size: 13; font-family: Tahoma;'>"
            MyMessage.Body &= "  <tr>"
            MyMessage.Body &= "   <td width='250px'><b>File Description</b></td>"
            MyMessage.Body &= "   <td width='250px'>&nbsp;</td>"
            MyMessage.Body &= "</tr>"
            For i = 0 To dsAED.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td height='25'>" & dsAED.Tables(0).Rows(i).Item("Description") & "</td>"
                MyMessage.Body &= "<td height='25'><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/AR_Deduction_Document.aspx?pARDID=" & ViewState("pARDID") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
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
        ''*     1) Email sent to all involved when an Asset is CAPITALIZED
        ''*     2) Email sent to all involved when the Estimated Completion Date changes with the Project Status is not Open
        ''*     3) Email sent to all involved with an Asset is VOID
        ''*     4) Email sent to Account with an Asset is COMPLETED
        ''********188 371 510 569
        Dim DefaultDate As Date = Date.Now
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

        lblErrors.Text = Nothing
        lblErrors.Visible = False

        Try
            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing Then
                ''Is this for a Group Notification or Individual
                Select Case EventDesc
                    Case "Closed" 'Sent by Initiator, notify Plant Controllers
                        GroupNotif = True
                    Case "Closed @60 days" 'Sent by Initiator, notify Plant Controllers
                        GroupNotif = True
                    Case "Void" 'Sent by Initiator, notify all
                        GroupNotif = True
                End Select

                If ViewState("pARDID") <> Nothing Then
                    ''*********************************
                    ''Send Notification
                    ''*********************************
                    If GroupNotif = True Then
                        If EventDesc = "Void" Then
                            ''*******************************
                            ''Notify Approvers--include Plant Controllers and Ops Mgrs.
                            ''*******************************
                            ds1st = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), 0, 0, False, False)
                            ''Check that the recipient(s) is a valid Team Member
                            If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                                    If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                        If (ds1st.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And (ddSubmittedBy.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID")) Then ''change to DefaultTMID   
                                            If EmailTO = Nothing Then
                                                EmailTO = ds1st.Tables(0).Rows(i).Item("Email")
                                            Else
                                                EmailTO = EmailTO & ";" & ds1st.Tables(0).Rows(i).Item("Email")
                                            End If
                                        End If
                                    End If
                                Next
                            End If 'EOF Notify Approvers
                        ElseIf (EventDesc = "Closed") Or (EventDesc = "Closed @60 days") Then
                            ''*******************************************
                            ''Notify 1st level Approvers
                            ''*******************************************
                            ds1st = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(134, ddUGNFacility.SelectedValue)
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
                        End If
                    End If 'EOF  If GroupNotif = True Then

                    ''********************************************************
                    ''Send Notification only if there is a valid Email Address
                    ''********************************************************
                    If EmailTO <> Nothing Then
                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                        EmailCC = CarbonCopyList(MyMessage, 138, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)

                        EmailCC = CarbonCopyList(MyMessage, IIf(hdDefaultNotify.Text = "Q", 140, IIf(hdDefaultNotify.Text = "M", 17, 134)), ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)

                        'Test or Production Message display
                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            'MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                            'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
                        End If

                        MyMessage.Subject &= "Operations Deduction for " & ddReason.SelectedItem.Text & " (Rec# " & ViewState("pARDID") & ") - " & EventDesc

                        MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"

                        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>This record was '" & EventDesc.ToUpper & "' by " & DefaultUserName & ".</strong></td>"

                        MyMessage.Body &= "</tr>"
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right' style='width: 150px;'>Rec No:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td> <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/AR_Deduction.aspx?pARDID=" & ViewState("pARDID") & "'>" & ViewState("pARDID") & "</a></td>"
                        MyMessage.Body &= "</tr>"
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>Reason for Deduction:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td>" & ddReason.SelectedItem.Text & "</td>"
                        MyMessage.Body &= "</tr>"
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td>" & ddUGNFacility.SelectedItem.Text & "</td>"
                        MyMessage.Body &= "</tr>"

                        Select Case EventDesc
                            Case "Void" 'Sent by Project Leader, notify all
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Void Reason:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td style='width: 600px;'>" & txtVoidReason.Text & "</td>"
                                MyMessage.Body &= "</tr>"
                            Case Else
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Credit/Debit Date:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & txtCreditDebitDate.Text & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Credit/Debit Memo:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & txtCreditDebitMemo.Text & "</td>"
                                MyMessage.Body &= "</tr>"
                                ''***************************************************
                                ''Get list of Supporting Documentation
                                ''***************************************************
                                Dim dsAED As DataSet
                                dsAED = ARGroupModule.GetARDeductionDocuments(ViewState("pARDID"), 0, False)
                                If commonFunctions.CheckDataSet(dsAED) = True Then
                                    MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
                                    MyMessage.Body &= "<td colspan='2'><strong>SUPPORTING DOCUMENT(S):</strong></td>"
                                    MyMessage.Body &= "</tr>"
                                    MyMessage.Body &= "<tr>"
                                    MyMessage.Body &= "<td colspan='2'>"
                                    MyMessage.Body &= "<table border='0' style='font-size: 13; font-family: Tahoma;'>"
                                    MyMessage.Body &= "  <tr>"
                                    MyMessage.Body &= "   <td width='250px'><b>File Description</b></td>"
                                    MyMessage.Body &= "   <td width='250px'>&nbsp;</td>"
                                    MyMessage.Body &= "</tr>"
                                    For i = 0 To dsAED.Tables.Item(0).Rows.Count - 1
                                        MyMessage.Body &= "<tr>"
                                        MyMessage.Body &= "<td height='25'>" & dsAED.Tables(0).Rows(i).Item("Description") & "</td>"
                                        MyMessage.Body &= "<td height='25'><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/AR_Deduction_Document.aspx?pARDID=" & ViewState("pARDID") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
                                        MyMessage.Body &= "</tr>"

                                    Next
                                    MyMessage.Body &= "</table>"
                                    MyMessage.Body &= "</tr>"
                                End If
                        End Select
                        MyMessage.Body &= "</table>"

                        Dim emailList As String() = commonFunctions.CleanEmailList(EmailCC).Split(";")
                        Dim ccEmail As String = Nothing
                        For i = 0 To UBound(emailList)
                            If emailList(i) <> ";" And emailList(i).Trim <> "" And emailList(i) <> EmailTO Then
                                ccEmail += emailList(i) & ";"
                            End If
                        Next i
                        EmailCC = ccEmail

                        Dim emailList1 As String() = commonFunctions.CleanEmailList(EmailTO).Split(";")
                        Dim toEmail As String = Nothing
                        For i = 0 To UBound(emailList1)
                            If emailList1(i) <> ";" And emailList1(i).Trim <> "" Then
                                toEmail += emailList1(i) & ";"
                            End If
                        Next i
                        EmailTO = toEmail

                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                            MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                            EmailTO = CurrentEmpEmail '"lynette.rey@ugnauto.com"
                            EmailCC = "lynette.rey@ugnauto.com"
                        End If

                        ''*****************
                        ''History Tracking
                        ''*****************
                        Select Case EventDesc
                            Case "Closed" 'Sent by Project Leader, notify accounting
                                ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, "Closed", "", "", "", "")
                            Case "Closed @60 days" 'Sent by Project Leader, notify accounting
                                ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, "Closed @60 days", "", "", "", "")
                            Case "Void" 'Sent by Project Leader, notify all
                                ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), ddReason.SelectedValue, DefaultTMID, "VOID - Reason:" & txtVoidReason.Text, "", "", "", "")
                        End Select


                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "AR_Deduction", ViewState("pARDID"))
                            lblErrors.Text = "Notification sent successfully."

                        Catch ex As Exception
                            lblErrors.Text &= "Email Notification is queued for the next automated release."
                            UGNErrorTrapping.InsertEmailQueue("Deduction Ref No:" & ViewState("pARDID"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                            'get current event name
                            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                            'log and email error
                            ' UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                        End Try
                        lblErrors.Visible = True
                        MaintainScrollPositionOnPostBack = "false"

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pARDID"))

                        ' ''*************************************************
                        ' '' "Form Level Security using Roles &/or Subscriptions"
                        ' ''*************************************************
                        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                        mvTabs.ActiveViewIndex = Int32.Parse(0)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(0).Selected = True

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
    End Sub 'EOF SendNotifWhenEventChanges

#End Region 'EOF "Email Notifications"

End Class