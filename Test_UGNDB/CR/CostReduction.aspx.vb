' *********************************************************************************************
' Name:	CostReduction.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'
' Date		    Author	    
' 01/11/2010    LRey			    Created .Net application
' 02/26/2010    Roderick Carlson    Added buttons for phase2, cleaned up error trapping on date fields, removed supporting document, adjusted security for plant controllers, debugged security for non-working team members, added more dataset checking when loading
' 03/15/2010    Roderick Carlson    CR-2859 - As well as fix email lists to make sure non-working team members are excluded.
' 03/20/1010    Roderick Carlson    CR-2870 - prompt the user to update the project category if completion is 100%
' 04/21/2010    Roderick Carlson    CR-2879 - Notify Facility and Corporate Plant Controllers and Team Leaders when steps are updated by team members
' 05/17/2010    Roderick Carlson    CR-2895 - If project is completed, set completion to 100%
' 05/21/2010    Roderick Carlson    CR-2899 - Do not allow the project to be set as complete until the plant controller reviews it.
' 06/24/2010    Roderick Carlson    CR-2920 - Added Checkbox Offsets Cost Downs
' 06/30/2010    LRey                Added Capital Project text field. This field can be defaulted from a link via an email notification sent from EXP module. Otherwise the user can manually enter the CapEx Project Number. The system will verify that there is only one CapExProjNo reference to avoid duplicate entries.
' 07/13/2010    LRey                Added a new function "SendEmailtoExpProjLeader" when there is a Capital Project referenced in a Cost Reduction Project. An email notification will be sent out automatically to the Project Leader and the system will update the fields in the designated ExpProj_"" table. Added a check in the delete process to avoid deleting Cost Reduction records should there be a Capital Project referenced in the database with a message to the user.
' 09/15/2010    Roderick Carlson    CR-2950 - allow percentage to be 100% by leader, even if plant controllers has not reviewed it. However, the plant controller has reviewed it, and the status is at 100%, then it can be closed. Also added Description to all emails
' 11/10/2010    Roderick Carlson    Allow Plant Controller to check reviewed by box even after project completion
' 03/01/2011    Roderick Carlson    Allow Plant Controller to submit project and allow steps to be updated before submission
' 05/06/2011    Roderick Carlson    Fixed a bug in txtNextSuccessRate_TextChanged and btnSave1 when NextSuccessRate value is empty
' 07/14/2011    LRey                Added logic to send notification when CapEx Repair is included
' 09/01/2011    Roderick Carlson    Added Customer Give Back Field
' 09/16/2011    Roderick Carlson    Added Budget Field
' 09/26/2011    Roderick Carlson    Do not let project close if Offsets Cost Downs is checked but Customer Give Back is 0
' 04/30/2012    Roderick Carlson    Allow Negative Cost Savings to be submitted
' 05/11/2012    LRey                Added logic to send notification when CapEx Development is included
' 02/05/2014	LRey                Replaced DeptOrCostCenter with new ERP values.
' 05/27/2014    LRey                Added Plant Manager to the SendEmailWhenStepsChange function.
' *********************************************************************************************
Partial Class CR_CostReduction
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            If ViewState("pProjNo") = Nothing Then
                m.ContentLabel = "New Cost Reduction Project"
            Else
                m.ContentLabel = "Cost Reduction Project"
            End If

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pProjNo") = Nothing Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Cost Reduction</b> > <a href='CostReductionList.aspx'><b>Cost Reduction Project Search</b></a> > New Cost Reduction Project"
                Else
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Cost Reduction</b> > <a href='CostReductionList.aspx'><b>Cost Reduction Project Search</b></a> > Cost Reduction Project"
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
            ctl = m.FindControl("CRExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then

                InitializeViewState()

                ''*******
                '' Get Query String
                ''*******
                If HttpContext.Current.Request.QueryString("pProjNo") > 0 Then
                    ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
                Else
                    ViewState("pProjNo") = 0
                End If
                If HttpContext.Current.Request.QueryString("pStepID") <> "" Then
                    ViewState("pStepID") = HttpContext.Current.Request.QueryString("pStepID")
                Else
                    ViewState("pStepID") = 0
                End If
                If HttpContext.Current.Request.QueryString("pStatusID") <> "" Then
                    ViewState("pStatusID") = HttpContext.Current.Request.QueryString("pStatusID")
                Else
                    ViewState("pStatusID") = 0
                End If
                If HttpContext.Current.Request.QueryString("pCPNo") <> "" Then
                    txtCapExProjNo.Text = HttpContext.Current.Request.QueryString("pCPNo")
                End If

                BindCriteria()

                If Session("viewOnly") Then
                    Session("isEnabled") = "False"
                Else
                    Session("isEnabled") = "True"
                End If

                ''*********
                ''BindData
                ''*********
                If ViewState("pProjNo") > 0 Then
                    BindData(ViewState("pProjNo"))
                End If

                txtDescription.Attributes.Add("onkeypress", "return tbLimit();")
                txtDescription.Attributes.Add("onkeyup", "return tbCount(" + lblDescription.ClientID + ");")
                txtDescription.Attributes.Add("maxLength", "200")

                txtAnnCostChngRsn.Attributes.Add("onkeypress", "return tbLimit();")
                txtAnnCostChngRsn.Attributes.Add("onkeyup", "return tbCount(" + lblAnnCostSave.ClientID + ");")
                txtAnnCostChngRsn.Attributes.Add("maxLength", "200")

                txtCapExChngRsn.Attributes.Add("onkeypress", "return tbLimit();")
                txtCapExChngRsn.Attributes.Add("onkeyup", "return tbCount(" + lblCapEx.ClientID + ");")
                txtCapExChngRsn.Attributes.Add("maxLength", "200")

                txtSuccessRateChngRsn.Attributes.Add("onkeypress", "return tbLimit();")
                txtSuccessRateChngRsn.Attributes.Add("onkeyup", "return tbCount(" + lblSuccessRate.ClientID + ");")
                txtSuccessRateChngRsn.Attributes.Add("maxLength", "200")

                txtImpDateChngRsn.Attributes.Add("onkeypress", "return tbLimit();")
                txtImpDateChngRsn.Attributes.Add("onkeyup", "return tbCount(" + lblImpDate.ClientID + ");")
                txtImpDateChngRsn.Attributes.Add("maxLength", "200")

                txtStatus.Attributes.Add("onkeypress", "return tbLimit();")
                txtStatus.Attributes.Add("onkeyup", "return tbCount(" + lblStatus.ClientID + ");")
                txtStatus.Attributes.Add("maxLength", "2000")

                txtSteps.Attributes.Add("onkeypress", "return tbLimit();")
                txtSteps.Attributes.Add("onkeyup", "return tbCount(" + lblSteps.ClientID + ");")
                txtSteps.Attributes.Add("maxLength", "2000")

            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            Page.ClientScript.RegisterStartupScript(Me.[GetType](), "jsCompletion", "function CheckCompletion(){" & vbCr & vbLf & " var TmpCompletionPercent = document.getElementById('" & txtCompletion.ClientID & "').value; var TmpCategoryControl = document.getElementById('" & ddProjectCategory.ClientID & "'); /* alert(TmpCompletionPercent); */ if (TmpCompletionPercent == 100) { var bTrue = confirm('Would you like to update the project category to be completed? (IF YOU WANT TO JUST UPDATE THE PERCENTAGE AND NOT THE CATEGORY, THEN CLICK CANCEL. THE NEW PERCENTAGE WILL BE SAVED.)'); if (bTrue == true) { for (i = 0; i < TmpCategoryControl.options.length; i++) { if (TmpCategoryControl.options[i].value == 4) { TmpCategoryControl.options[TmpCategoryControl.selectedIndex].selected = false; TmpCategoryControl.selectedIndex = i; TmpCategoryControl.options[i].selected = true;  TmpCategoryControl.selectedItem = TmpCategoryControl.options[i];  break;  }  }  } var TmpSaveButtonControl = document.getElementById('" & btnSave1.ClientID & "'); if (TmpSaveButtonControl != null) { TmpSaveButtonControl.click(); } }  " & vbCr & vbLf & "}", True)

            txtCompletion.Attributes.Add("onblur", "javascript:CheckCompletion();")

            Dim strPreviewClientScript As String = "javascript:void(window.open('crViewCostReductionDetail.aspx?pProjNo=" & ViewState("pProjNo") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
            btnPreview.Attributes.Add("onclick", strPreviewClientScript)

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

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

            btnSave1.Enabled = False
            btnSaveToGrid1.Enabled = False
            btnSaveToGrid2.Enabled = False
            btnProposedDetails.Enabled = False
            btnReset1.Enabled = False
            btnReset2.Enabled = False
            btnReset3.Enabled = False
            btnDelete.Enabled = False
            btnSubmit.Enabled = False
            btnAdd.Enabled = True
            btnPreview.Enabled = False
            btnCopy.Enabled = False

            cbOffsetsCostDowns.Enabled = False
            cbPlantControllerReviewed.Enabled = False

            ddCommodity.Enabled = False
            ddLeader.Enabled = False
            ddProjectCategory.Enabled = False
            ddTeamMember.Enabled = False
            ddUGNFacility.Enabled = False

            txtCompletion.Enabled = False

            txtDateSubmitted.Enabled = False
            txtDescription.Enabled = False
           
            txtNextSuccessRate.Enabled = False
            txtRFDNo.Enabled = False
            txtStatus.Enabled = False
            txtSteps.Enabled = False
            txtSuccessRate.Enabled = False

            txtEstAnnCostSave.Visible = True
            txtCapEx.Visible = True
            txtSuccessRate.Visible = True

            lblRank.Visible = True
            gvStatus.Columns(3).Visible = False
            gvSteps.Columns(5).Visible = False

            ''* Display Accordians
            If ViewState("pProjNo") = 0 Then
                accSteps.Visible = False
                accStatus.Visible = False
                accHistory.Visible = False
                txtEstImpDate.Enabled = True
                txtNextImpDate.Enabled = False
            Else
                accSteps.Visible = True
                accStatus.Visible = True
                accHistory.Visible = True
                txtEstImpDate.Enabled = False
                txtNextImpDate.Enabled = True

                ''* Implementation Date
                If txtEstImpDate.Text = "" Then
                    rfvEstImpDate.Enabled = False
                End If

                rfvImpDateChngRsn.Enabled = True
                If txtHDEstImpDate.Text.Trim <> "" And txtNextImpDate.Text.Trim <> "" Then
                    If CType(txtHDEstImpDate.Text, Date) = CType(txtNextImpDate.Text, Date) Then
                        rfvImpDateChngRsn.Enabled = False
                    End If
                End If

                ''* Annual Cost Save
                If txtEstAnnCostSave.Text = "" Then
                    rfvEstAnnCostSave.Enabled = False
                End If
                If txtHDEstAnnCostSave.Text = txtNextAnnCostSave.Text Then
                    rfvAnnCostChngRsn.Enabled = False
                Else
                    rfvAnnCostChngRsn.Enabled = True
                End If

                ''* CapEx
                If txtCapEx.Text = "" Then
                    rfvCapEx.Enabled = False
                End If
                If txtHDCapEx.Text = txtNextCapEx.Text Then
                    rfvCapExChngRsn.Enabled = False
                Else
                    rfvCapExChngRsn.Enabled = True
                End If

                ''* Success Rate
                If txtSuccessRate.Text = "" Then
                    rfvSuccessRate.Enabled = False
                End If
                If txtHDSuccessRate.Text = txtNextSuccessRate.Text Then
                    rfvSuccessRateChngRsn.Enabled = False
                Else
                    rfvSuccessRateChngRsn.Enabled = True
                End If

                ''* Capital Project
                If txtCapExProjNo.Text = "" Or ViewState("pProjNo") = 0 Then
                    txtCapExProjNo.Enabled = True
                Else ' do not allow overwrite
                    txtCapExProjNo.Enabled = False
                End If
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim dsSubscription As DataSet
            Dim iTeamMemberID As Integer = 0

            ViewState("isProposedDetailsExist") = False
            ViewState("SubscriptionID") = 0

            Dim ds As DataSet
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 97 'Cost Reduction Project Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                ' developer testing as another team member
                If iTeamMemberID = 530 Then
                    'iTeamMemberID = 612 'dan marcon                                            
                    'iTeamMemberID = 171 'greg hall
                    'iTeamMemberID = 433 'derek ames                    
                    'iTeamMemberID = 45 ' Mike Omery
                    iTeamMemberID = 672 'John.Mercado 
                End If

                'Is Plant Controller?
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 20)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 20
                End If

                iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                    'Get Team Member's Role assignment
                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)
                    If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                        iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                        If ViewState("pProjNo") > 0 Then
                            btnPreview.Enabled = True
                            btnProposedDetails.Enabled = True

                            ds = CRModule.GetCostReductionDetail(ViewState("pProjNo"))
                            If commonFunctions.CheckDataSet(ds) = True Then
                                ViewState("isProposedDetailsExist") = True
                            End If
                        End If

                        Select Case iRoleID
                            Case 11 '*** UGNAdmin: Full Access
                                ViewState("ObjectRole") = True
                                ViewState("Admin") = True

                                btnAdd.Enabled = True
                                btnSave1.Enabled = True
                                btnReset1.Enabled = True

                                ddLeader.Enabled = True

                                ''*************************************************
                                ''for new entries, enable only the first tab
                                ''*************************************************
                                If ViewState("pProjNo") = 0 Then
                                    txtDescription.Focus()
                                Else
                                    btnCopy.Enabled = True
                                    btnDelete.Enabled = True
                                    btnSaveToGrid1.Enabled = True
                                    btnSaveToGrid2.Enabled = True
                                    btnReset2.Enabled = True
                                    btnReset3.Enabled = True

                                    If ViewState("isProposedDetailsExist") = False Then
                                        lblErrors.Text = "Please make sure the Plant Controller updates the Proposed Details Page before the project is submitted."
                                        lblErrorsButtons.Text = lblErrors.Text
                                    End If

                                    If txtDateSubmitted.Text = "" And ViewState("isProposedDetailsExist") = True Then
                                        lblErrors.Text &= "<br>This project has NOT been submitted."
                                        lblErrorsButtons.Text = lblErrors.Text
                                        'btnSubmit.Enabled = True
                                    End If

                                    btnSubmit.Enabled = True
                                    cbOffsetsCostDowns.Enabled = True
                                    cbPlantControllerReviewed.Enabled = True
                                    ddCommodity.Enabled = True
                                    ddLeader.Enabled = True
                                    ddProjectCategory.Enabled = True
                                    ddTeamMember.Enabled = True
                                    ddUGNFacility.Enabled = True
                                    gvStatus.Columns(3).Visible = True
                                    gvSteps.Columns(5).Visible = True
                                    txtCompletion.Enabled = True
                                    txtDescription.Enabled = True
                                    txtNextImpDate.Enabled = True
                                    txtNextSuccessRate.Enabled = True
                                    txtRFDNo.Enabled = True
                                    txtStatus.Enabled = True
                                    txtSteps.Enabled = True
                                End If
                            Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                ViewState("ObjectRole") = True

                                If ViewState("pProjNo") = 0 Then
                                    ddLeader.SelectedValue = iTeamMemberID
                                    txtDescription.Focus()
                                    btnSave1.Enabled = True
                                    btnReset1.Enabled = True
                                Else
                                    cbOffsetsCostDowns.Enabled = True
                                    btnAdd.Enabled = True

                                    If iTeamMemberID = ddLeader.SelectedValue Then
                                        btnCopy.Enabled = True
                                        btnSave1.Enabled = True
                                        btnReset1.Enabled = True
                                        btnDelete.Enabled = True

                                        If ViewState("isProposedDetailsExist") = False Then
                                            lblErrors.Text = "Please make sure the Plant Controller updates the Proposed Details Page before the project is submitted."
                                            'lblErrorsButtons.Text = lblErrors.Text
                                        End If

                                        If txtDateSubmitted.Text = "" And ViewState("isProposedDetailsExist") = True Then
                                            lblErrors.Text &= "<br>This project has NOT been submitted."
                                            'lblErrorsButtons.Text = lblErrors.Text
                                            btnSubmit.Enabled = True
                                        End If

                                        If ddProjectCategory.SelectedItem.Text <> "Completed" Then
                                            txtStatus.Enabled = True
                                            btnSaveToGrid1.Enabled = True
                                            btnReset2.Enabled = True
                                            gvStatus.Columns(3).Visible = True
                                        End If

                                        ddCommodity.Enabled = True
                                        ddLeader.Enabled = True
                                        ddProjectCategory.Enabled = True
                                        ddTeamMember.Enabled = True
                                        ddUGNFacility.Enabled = True
                                        gvStatus.Columns(3).Visible = True
                                        gvSteps.Columns(5).Visible = True
                                        txtCompletion.Enabled = True
                                        txtDescription.Enabled = True
                                        txtNextImpDate.Enabled = True
                                        txtNextSuccessRate.Enabled = True
                                        txtRFDNo.Enabled = True
                                        txtStatus.Enabled = True
                                        txtSteps.Enabled = True
                                    End If

                                    ''03/01/2011 - Nicolas Leclercq and Derek Ames - allow steps to be updated before submission
                                    'If iTeamMemberID <> ddLeader.SelectedValue And ddProjectCategory.SelectedItem.Text <> "Completed" And txtDateSubmitted.Text <> "" Then
                                    If iTeamMemberID <> ddLeader.SelectedValue And ddProjectCategory.SelectedItem.Text <> "Completed" Then
                                        ''only Team Members that did not create record 
                                        ''will have access to add steps/comments.
                                        ddTeamMember.Enabled = True
                                        txtSteps.Enabled = True
                                        btnReset3.Enabled = True
                                        btnSaveToGrid2.Enabled = True
                                        gvSteps.Columns(5).Visible = True
                                    End If
                                End If
                            Case 13 '*** UGNAssist: Create/Edit/No Delete
                                ViewState("ObjectRole") = True

                                ''*************************************************
                                ''for new entries, enable only the first tab
                                ''*************************************************
                                If ViewState("pProjNo") = 0 Then
                                    txtDescription.Focus()
                                    btnSave1.Enabled = True
                                    btnReset1.Enabled = True
                                Else
                                    cbOffsetsCostDowns.Enabled = True

                                    btnAdd.Enabled = True

                                    If iTeamMemberID = ddLeader.SelectedValue Then
                                        btnCopy.Enabled = True
                                        btnSave1.Enabled = True
                                        btnReset1.Enabled = True
                                        btnDelete.Enabled = True

                                        If ViewState("isProposedDetailsExist") = False Then
                                            lblErrors.Text = "Please make sure the Plant Controller updates the Proposed Details Page before the project is submitted."
                                            'lblErrorsButtons.Text = lblErrors.Text
                                        End If

                                        If txtDateSubmitted.Text = "" And ViewState("isProposedDetailsExist") = True Then
                                            lblErrors.Text &= "<br>This project has NOT been submitted."
                                            'lblErrorsButtons.Text = lblErrors.Text
                                            btnSubmit.Enabled = True
                                        End If

                                        If ddProjectCategory.SelectedItem.Text <> "Completed" Then
                                            txtStatus.Enabled = True
                                            btnSaveToGrid1.Enabled = True
                                            btnReset2.Enabled = True
                                            gvStatus.Columns(3).Visible = True
                                        End If

                                        ddCommodity.Enabled = True
                                        ddLeader.Enabled = True
                                        ddProjectCategory.Enabled = True
                                        ddTeamMember.Enabled = True
                                        ddUGNFacility.Enabled = True
                                        gvStatus.Columns(3).Visible = True
                                        gvSteps.Columns(5).Visible = True
                                        txtCompletion.Enabled = True
                                        txtDescription.Enabled = True
                                        txtNextImpDate.Enabled = True
                                        txtNextSuccessRate.Enabled = True
                                        txtRFDNo.Enabled = True
                                        txtStatus.Enabled = True
                                        txtSteps.Enabled = True
                                    End If

                                    ''03/01/2011 - Nicolas Leclercq and Derek Ames - allow steps to be updated before submission
                                    'If iTeamMemberID <> ddLeader.SelectedValue And ddProjectCategory.SelectedItem.Text <> "Completed" And (txtDateSubmitted.Text <> "") Then
                                    If iTeamMemberID <> ddLeader.SelectedValue And ddProjectCategory.SelectedItem.Text <> "Completed" Then
                                        ''only Team Members that did not create record 
                                        ''will have access to add steps/comments.
                                        ddTeamMember.Enabled = True
                                        txtSteps.Enabled = True
                                        btnReset3.Enabled = True
                                        btnSaveToGrid2.Enabled = True
                                        gvSteps.Columns(5).Visible = True
                                    End If
                                End If

                            Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                ViewState("ObjectRole") = False
                            Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                ViewState("ObjectRole") = True
                                If txtDateSubmitted.Text = "" Then
                                    lblErrors.Text = "The project has NOT been submitted yet."
                                    'lblErrorsButtons.Text = lblErrors.Text
                                End If

                                ''03/01/2011 - Nicolas Leclercq and Derek Ames - allow steps to be updated before submission                                
                                If ddProjectCategory.SelectedItem.Text <> "Completed" Then

                                    If ddLeader.SelectedIndex > 0 Then
                                        If iTeamMemberID <> ddLeader.SelectedValue Then
                                            ''only Team Members that did not create record 
                                            ''will have access to add steps/comments.
                                            ddTeamMember.Enabled = True
                                            txtSteps.Enabled = True
                                            btnReset3.Enabled = True
                                            btnSaveToGrid2.Enabled = True
                                            gvSteps.Columns(5).Visible = True
                                        End If
                                    End If

                                End If

                                If ViewState("SubscriptionID") = 20 Then
                                    cbPlantControllerReviewed.Enabled = True
                                    btnSave1.Enabled = True
                                    btnReset1.Enabled = True
                                    ' If iTeamMemberID <> ddLeader.SelectedValue Then
                                    cvImpDt1.Enabled = False
                                    cvImpDt2.Enabled = False
                                    'End If

                                    '03/01/2011 - Nicolas Leclercq and Derek Ames - allow Plant Controllers Subscription to submit project
                                    If txtDateSubmitted.Text = "" And ViewState("isProposedDetailsExist") = True Then
                                        btnSubmit.Enabled = True
                                    End If
                                End If

                            Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                    ''** No Entry allowed **''
                                    ViewState("ObjectRole") = False
                        End Select 'EOF of "Select Case iRoleID"                    
                    End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                End If 'EOF of "If iWorking = True Then"            
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"

            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                Dim FullName As String = commonFunctions.getUserName()
                Dim UserEmailAddress As String = FullName & "@ugnauto.com"
                Response.Cookies("UGNDB_User_Email").Value = UserEmailAddress
                If FullName = Nothing Then
                    FullName = "Demo.Demo"  '* This account has restricted read only rights.
                End If

                Dim LocationOfDot As Integer = InStr(FullName, ".")
                If LocationOfDot > 0 Then
                    Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                    Dim FirstInitial As String = Left(FullName, 1)
                    Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                    Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
                Else
                    Response.Cookies("UGNDB_User").Value = FullName
                End If
            End If

            ViewState("DefaultUser") = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            cbOffsetsCostDowns.Enabled = ViewState("ObjectRole")
            ddCommodity.Enabled = ViewState("ObjectRole")
            ddProjectCategory.Enabled = ViewState("ObjectRole")
            ddUGNFacility.Enabled = ViewState("ObjectRole")

            txtDescription.Enabled = ViewState("ObjectRole")
            txtEstAnnCostSave.Visible = ViewState("ObjectRole")
            txtNextAnnCostSave.Visible = ViewState("ObjectRole")
            txtCapEx.Visible = ViewState("ObjectRole")
            txtNextCapEx.Visible = ViewState("ObjectRole")
            txtSuccessRate.Enabled = ViewState("ObjectRole")
            txtRFDNo.Enabled = ViewState("ObjectRole")

            lblRank.Visible = ViewState("ObjectRole")

            If ViewState("pProjNo") > 0 Then
                accHistory.Visible = ViewState("ObjectRole")
            Else
                accHistory.Visible = False
            End If

            btnPreview.Visible = ViewState("ObjectRole")
            btnProposedDetails.Visible = ViewState("ObjectRole")

        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message & ", TESTING", System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblErrorsButtons.Text = lblErrors.Text

    End Sub
#End Region 'EOF Form Level Security

    Protected Sub BindCriteria()

        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down Project Leader control for selection criteria for search
        ds = CRModule.GetCostReductionProjectLeaders()
        If commonFunctions.CheckDataSet(ds) = True Then
            ddLeader.DataSource = ds
            ddLeader.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName.ToString()
            ddLeader.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
            ddLeader.DataBind()
            ddLeader.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Team Member control for selection criteria for search
        ds = commonFunctions.GetTeamMember("")
        If commonFunctions.CheckDataSet(ds) = True Then
            ddTeamMember.DataSource = ds
            ddTeamMember.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
            ddTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
            ddTeamMember.DataBind()
            ddTeamMember.Items.Insert(0, "")
        End If

        commonFunctions.UserInfo()
        ddLeader.SelectedValue = HttpContext.Current.Session("UserId")
        ddTeamMember.SelectedValue = HttpContext.Current.Session("UserId")

        ''bind existing data to drop down UGN Location control for selection criteria for search
        ds = commonFunctions.GetUGNFacility("")
        If commonFunctions.CheckDataSet(ds) = True Then
            ddUGNFacility.DataSource = ds
            ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
            ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
            ddUGNFacility.DataBind()
            ddUGNFacility.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Commodity control for selection criteria 
        ds = commonFunctions.GetCommodity(0, "", "", 0)
        If commonFunctions.CheckDataSet(ds) = True Then
            ddCommodity.DataSource = ds
            ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
            ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
            ddCommodity.DataBind()
            ddCommodity.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Project Category control for selection criteria for search
        ds = CRModule.GetProjectCategory("")
        If commonFunctions.CheckDataSet(ds) = True Then
            ddProjectCategory.DataSource = ds
            ddProjectCategory.DataTextField = ds.Tables(0).Columns("ddProjectCategoryName").ColumnName.ToString()
            ddProjectCategory.DataValueField = ds.Tables(0).Columns("PCID").ColumnName.ToString()
            ddProjectCategory.DataBind()
            ddProjectCategory.Items.Insert(0, "")
        End If

    End Sub 'EOF BindCriteria

    Private Sub InitializeViewState()

        Try

            ViewState("pProjNo") = 0
            ViewState("pStepID") = 0
            ViewState("pStatusID") = 0

            ViewState("Admin") = False
            ViewState("ObjectRole") = False

            ViewState("isProposedDetailsExist") = False
            ViewState("SubscriptionID") = 0


            ViewState("DefaultUser") = ""

            ViewState("OriginalProjectCategoryID") = 0
            ViewState("OriginalCompletion") = 0
            ViewState("OriginalCapExProjNo") = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Sub BindData(ByVal ProjNo As String)

        Try
            Dim ds As DataSet = New DataSet
            Dim ds2 As DataSet = New DataSet

            Dim dCustomerGiveBackDollar As Double = 0
            Dim dCustomerGiveBackPercent As Double = 0

            Dim dTotalSavings As Double = 0
            Dim dTotalSavingsBudget As Double = 0

            Dim iHeigthBySpecificCharCount As Integer = 0
            Dim iHeightByTextFieldLength As Integer = 0
            Dim iHeightToUse As Integer = 0

            If ViewState("pProjNo") > 0 Then
                ds = CRModule.GetCostReduction(ViewState("pProjNo"), 0, "", 0, 0, "", 0, False, False, "")
                If commonFunctions.CheckDataSet(ds) = True Then
                    txtTodaysDate.Text = Date.Today

                    lblProjectNo.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                    txtDateSubmitted.Text = ds.Tables(0).Rows(0).Item("DateSubmitted").ToString()
                    txtDescription.Text = ds.Tables(0).Rows(0).Item("Description").ToString.Trim

                    iHeigthBySpecificCharCount = 0
                    iHeightByTextFieldLength = 0
                    iHeightToUse = 75

                    'count all characters
                    iHeigthBySpecificCharCount = (txtDescription.Text.Trim.Length / 80) * 20
                    'count the number of carriage return line feeds
                    iHeightByTextFieldLength = (UBound(Split(txtDescription.Text, vbCrLf)) * 40)

                    'if calculated heights are greater than 200 use the greater of the 2
                    If iHeigthBySpecificCharCount > iHeightToUse Or iHeightByTextFieldLength > iHeightToUse Then
                        If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                            iHeightToUse = iHeigthBySpecificCharCount
                        Else
                            iHeightToUse = iHeightByTextFieldLength
                        End If
                    End If
                    txtDescription.Height = iHeightToUse

                    ddProjectCategory.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectCategoryID").ToString()

                    ViewState("OriginalProjectCategoryID") = 0
                    If ds.Tables(0).Rows(0).Item("ProjectCategoryID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ProjectCategoryID") > 0 Then
                            ViewState("OriginalProjectCategoryID") = ds.Tables(0).Rows(0).Item("ProjectCategoryID")
                        End If
                    End If

                    ddLeader.SelectedValue = ds.Tables(0).Rows(0).Item("LeaderTMID").ToString()
                    ddUGNFacility.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                    ddCommodity.SelectedValue = ds.Tables(0).Rows(0).Item("CommodityID").ToString()
                    txtEstImpDate.Text = ds.Tables(0).Rows(0).Item("EstImpDate").ToString()
                    txtHDEstImpDate.Text = ds.Tables(0).Rows(0).Item("EstImpDate").ToString()
                    txtNextImpDate.Text = ds.Tables(0).Rows(0).Item("EstImpDate").ToString()
                    txtCompletion.Text = ds.Tables(0).Rows(0).Item("Completion").ToString()

                    ViewState("OriginalCompletion") = 0
                    If ds.Tables(0).Rows(0).Item("Completion") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("Completion") > 0 Then
                            ViewState("OriginalCompletion") = ds.Tables(0).Rows(0).Item("Completion")
                        End If
                    End If

                    If ds.Tables(0).Rows(0).Item("Completion") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("Completion") > 0 Then
                            If ds.Tables(0).Rows(0).Item("Completion") > 100 Then
                                txtCompletionPercent.Width = 100
                            Else
                                txtCompletionPercent.Width = ds.Tables(0).Rows(0).Item("Completion").ToString
                            End If

                            txtCompletionPercent.BackColor = Color.Maroon
                            txtCompletionPercent.Text = IIf(ds.Tables(0).Rows(0).Item("Completion") > 100, 100, ds.Tables(0).Rows(0).Item("Completion").ToString()) & " %"
                            txtCompletionPercent.ForeColor = Color.White
                            txtCompletionPercent.ToolTip = ds.Tables(0).Rows(0).Item("Completion").ToString() & "% complete"
                        End If
                    End If

                    If ds.Tables(0).Rows(0).Item("PrcntNearImpDate") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("PrcntNearImpDate") > 0 Then
                            If ds.Tables(0).Rows(0).Item("PrcntNearImpDate") > 100 Then
                                txtProjectTimeline.Width = 100
                            Else
                                txtProjectTimeline.Width = ds.Tables(0).Rows(0).Item("PrcntNearImpDate").ToString
                            End If
                            txtProjectTimeline.BackColor = Color.Maroon
                            txtProjectTimeline.Text = IIf(ds.Tables(0).Rows(0).Item("PrcntNearImpDate") > 100, 100, ds.Tables(0).Rows(0).Item("PrcntNearImpDate").ToString()) & " %"
                            txtProjectTimeline.ForeColor = Color.White
                        End If
                    End If

                    txtRFDNo.Text = ds.Tables(0).Rows(0).Item("RFDNo").ToString()
                    txtCapExProjNo.Text = ds.Tables(0).Rows(0).Item("CapExProjNo").ToString.Trim
                    ViewState("OriginalCapExProjNo") = ds.Tables(0).Rows(0).Item("CapExProjNo").ToString.Trim

                    txtSuccessRate.Text = ds.Tables(0).Rows(0).Item("SuccessRate").ToString()
                    txtHDSuccessRate.Text = ds.Tables(0).Rows(0).Item("SuccessRate").ToString()
                    txtNextSuccessRate.Text = ds.Tables(0).Rows(0).Item("SuccessRate").ToString()

                    lblRank.Text = "0"
                    If ds.Tables(0).Rows(0).Item("Rank") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("Rank") <> 0 Then
                            lblRank.Text = String.Format("{0:###,###,###}", ds.Tables(0).Rows(0).Item("Rank"))
                        End If
                    End If

                    txtEstAnnCostSave.Text = "0"
                    txtEstAnnCostSave.Text = ds.Tables(0).Rows(0).Item("EstAnnualCostSave").ToString()

                    txtHDEstAnnCostSave.Text = "0"
                    txtHDEstAnnCostSave.Text = ds.Tables(0).Rows(0).Item("EstAnnualCostSave").ToString()

                    txtNextAnnCostSave.Text = "0"
                    txtNextAnnCostSave.Text = ds.Tables(0).Rows(0).Item("EstAnnualCostSave").ToString()

                    If ds.Tables(0).Rows(0).Item("EstAnnualCostSave") IsNot Nothing Then
                        If ds.Tables(0).Rows(0).Item("EstAnnualCostSave") <> 0 Then
                            dTotalSavings = ds.Tables(0).Rows(0).Item("EstAnnualCostSave")
                        End If
                    End If

                    txtCapEx.Text = "0"
                    txtCapEx.Text = ds.Tables(0).Rows(0).Item("CapEx").ToString()

                    txtHDCapEx.Text = "0"
                    txtHDCapEx.Text = ds.Tables(0).Rows(0).Item("CapEx").ToString()

                    txtNextCapEx.Text = "0"
                    txtNextCapEx.Text = ds.Tables(0).Rows(0).Item("CapEx").ToString()

                    If ds.Tables(0).Rows(0).Item("isOffsetsCostDowns") IsNot System.DBNull.Value Then
                        cbOffsetsCostDowns.Checked = ds.Tables(0).Rows(0).Item("isOffsetsCostDowns")
                    End If

                    If ds.Tables(0).Rows(0).Item("isPlantControllerReviewed") IsNot System.DBNull.Value Then
                        cbPlantControllerReviewed.Checked = ds.Tables(0).Rows(0).Item("isPlantControllerReviewed")
                    End If

                    If ViewState("pStepID") <> 0 Then
                        ds2 = CRModule.GetCostReductionSteps(ViewState("pStepID"), ViewState("pProjNo"))
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            ddTeamMember.SelectedValue = ds2.Tables(0).Rows(0).Item("TeamMemberID").ToString()
                            txtSteps.Text = ds2.Tables(0).Rows(0).Item("StepsComments").ToString()
                        Else 'no record found reset query string pRptID
                            Response.Redirect("CostReduction.aspx?pProjNo=" & ViewState("pProjNo"), False)
                        End If
                    End If

                    If ViewState("pStatusID") <> 0 Then
                        ds2 = CRModule.GetCostReductionStatus(ViewState("pStatusID"), ViewState("pProjNo"))
                        If commonFunctions.CheckDataSet(ds2) = True Then
                            txtStatus.Text = ds2.Tables(0).Rows(0).Item("Status").ToString()
                        Else 'no record found reset query string pRptID
                            Response.Redirect("CostReduction.aspx?pProjNo=" & ViewState("pProjNo"), False)
                        End If
                    End If

                    ds = CRModule.GetCostReductionDetail(ViewState("pProjNo"))
                    If commonFunctions.CheckDataSet(ds) = True Then

                        'customer non-grid info
                        If ds.Tables(0).Rows(0).Item("CustomerGiveBackDollar") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("CustomerGiveBackDollar") <> 0 Then
                                txtCustomerGiveBackDollar.Text = Format(ds.Tables(0).Rows(0).Item("CustomerGiveBackDollar"), "##0.00")
                                dCustomerGiveBackDollar = ds.Tables(0).Rows(0).Item("CustomerGiveBackDollar")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("CustomerGiveBackPercent") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("CustomerGiveBackPercent") <> 0 Then
                                dCustomerGiveBackPercent = ds.Tables(0).Rows(0).Item("CustomerGiveBackPercent")
                            End If
                        End If

                        'dollar takes precedence over percent
                        If dCustomerGiveBackPercent <> 0 And dCustomerGiveBackDollar = 0 Then
                            dCustomerGiveBackDollar = dTotalSavings * (dCustomerGiveBackPercent / 100)
                            txtCustomerGiveBackDollar.Text = Format(dCustomerGiveBackDollar, "##0.00")
                        End If

                        If ds.Tables(0).Rows(0).Item("TotalSavingsBudget") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("TotalSavingsBudget") <> 0 Then
                                dTotalSavingsBudget = ds.Tables(0).Rows(0).Item("TotalSavingsBudget")
                            End If
                        End If

                        txtActualNetAnnualCostSavings.Text = Format(dTotalSavings - dCustomerGiveBackDollar, "##0.00")
                        txtBudgetNetAnnualCostSavings.Text = Format(dTotalSavingsBudget - dCustomerGiveBackDollar, "##0.00")
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
    End Sub 'EOF BindData()

    Private Sub ClearMessages()

        lblErrors.Text = ""
        lblErrorsButtons.Text = ""

    End Sub

    Private Sub ResetCompletion()

        If ViewState("OriginalProjectCategoryID") > 0 Then
            ddProjectCategory.SelectedValue = ViewState("OriginalProjectCategoryID")
        End If

        txtCompletion.Text = ViewState("OriginalCompletion")

    End Sub

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click

        Try
            ClearMessages()

            Dim DefaultDate As Date = Date.Today
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim bContinueSave As Boolean = True

            Dim dEstAnnCostSave As Decimal = 0
            Dim dCAPEX As Decimal = 0
            Dim dCompletion As Double = 0
            Dim dCustomerGiveBack As Double = 0
            Dim dSuccessRate As Decimal = 0

            Dim iCategory As Integer = 0
            Dim iCommodity As Integer
            Dim iLeader As Integer = 0
            Dim iRFDNo As Integer = 0

            If txtHDEstAnnCostSave.Text.Trim <> "" Then
                dEstAnnCostSave = CType(txtHDEstAnnCostSave.Text.Trim, Double)
            End If

            If txtHDCapEx.Text.Trim <> "" Then
                dCAPEX = CType(txtHDCapEx.Text.Trim, Double)
            End If

            If txtHDSuccessRate.Text.Trim <> "" Then
                dSuccessRate = CType(txtHDSuccessRate.Text.Trim, Double)
            End If

            If txtCompletion.Text <> "" Then
                dCompletion = CType(txtCompletion.Text, Double)
            End If

            If ddProjectCategory.SelectedIndex > 0 Then
                iCategory = ddProjectCategory.SelectedValue
            End If

            If ddCommodity.SelectedIndex > 0 Then
                iCommodity = ddCommodity.SelectedValue
            End If

            If ddLeader.SelectedIndex > 0 Then
                iLeader = ddLeader.SelectedValue
            End If

            If txtRFDNo.Text.Trim <> "" Then
                iRFDNo = txtRFDNo.Text.Trim
            End If

            If txtCustomerGiveBackDollar.Text.Trim <> "" Then
                dCustomerGiveBack = CType(txtCustomerGiveBackDollar.Text.Trim, Double)
            End If

            ''*****************************************************************************************************************
            ''Verify that the CapExProjNo does not already exist in another Cost Reduction Project to avoid duplicate entries
            ''*****************************************************************************************************************
            If txtCapExProjNo.Text <> Nothing Then
                Dim ds As DataSet
                ds = CRModule.GetCostReduction("", 0, "", 0, 0, "", 0, False, False, txtCapExProjNo.Text)
                If commonFunctions.CheckDataSet(ds) = True Then
                    'same ProjectNo will be returned
                    If ds.Tables(0).Rows(0).Item("ProjectNo") <> ViewState("pProjNo") Then
                        lblErrors.Text &= "Save Cancelled. Capital Project was assigned to Cost Reduction Project No: " & ds.Tables(0).Rows(0).Item("ProjectNo").ToString()

                        vsProjectDetail.Visible = False
                        bContinueSave = False
                        CheckRights()
                    End If
                End If
            End If  'Run the rest of the control

            If bContinueSave = True Then
                'if project exists
                If (ViewState("pProjNo") > 0) Then

                    'do not allow the project to be closed until the plant controller has reviewed it
                    '09/15/2010 - Ref# CR-2950 - allow percentage to be 100% by leader, even if plant controllers has not reviewed it.
                    'However, if the plant controller has reviewed it, and the status is at 100%, then it can be closed.
                    If (iCategory = 4 Or iCategory = 6) And cbPlantControllerReviewed.Checked = False Then
                        'If ViewState("OriginalProjectCategoryID") > 0 Then
                        '    ddProjectCategory.SelectedValue = ViewState("OriginalProjectCategoryID")
                        'End If

                        'txtCompletion.Text = ViewState("OriginalCompletion")
                        bContinueSave = False
                        ResetCompletion()

                        lblErrors.Text = "Error: INFORMATION WAS NOT UPDATED. THE PROJECT CANNOT BE SET TO COMPLETE UNTIL THE PLANT CONTROLLER REVIEWS IT."
                    End If

                    '09/26/2011 - if the offsets cost down was checked, then customer give back must have a value
                    If (iCategory = 4 Or iCategory = 6) And cbOffsetsCostDowns.Checked = True And dCustomerGiveBack = 0 Then
                        bContinueSave = False

                        ResetCompletion()

                        lblErrors.Text = "Error: INFORMATION WAS NOT UPDATED. SINCE THE OFFSETS COSTDOWN WAS CHECKED, THE PROJECT CANNOT BE SET TO COMPLETE UNTIL THE CUSTOMER GIVE BACK VALUE HAS BEEN UPDATED ON THE PROPOSED DETAILS PAGE. PROJECT LEADERS, SALES, OR PLANT CONTROLLERS CAN ENTER THIS VALUE."
                    End If

                    If bContinueSave = True Then
                        'allow update                        
                        Dim EstImpDate As String = txtHDEstImpDate.Text
                        Dim SendEmailToDefaultAdmin As Boolean = False

                        '************************************
                        '* Capture Imp. Date Change History
                        '************************************ 
                        If CType(txtHDEstImpDate.Text, Date) <> CType(txtNextImpDate.Text, Date) Then
                            If txtDateSubmitted.Text <> "" Then
                                CRModule.InsertCostReductionHistory(ViewState("pProjNo"), DefaultTMID, txtImpDateChngRsn.Text, "Implementation Date", EstImpDate, txtNextImpDate.Text)
                                SendEmailToDefaultAdmin = True
                            End If
                            ''Assign EstImpDate with new value.
                            EstImpDate = txtNextImpDate.Text
                            lblReqImpDateChange.Visible = False
                            lblImpDateChange.Visible = False
                            txtImpDateChngRsn.Visible = False
                        End If

                        '************************************
                        '* Capture Annual Cost Save Change History
                        '************************************
                        If txtHDEstAnnCostSave.Text <> txtNextAnnCostSave.Text Then
                            If txtDateSubmitted.Text <> "" Then
                                CRModule.InsertCostReductionHistory(ViewState("pProjNo"), DefaultTMID, txtAnnCostChngRsn.Text, "Annual Cost Save", dEstAnnCostSave, txtNextAnnCostSave.Text)
                                SendEmailToDefaultAdmin = True
                            End If
                            ''Assign EstAnnCostSave with new value.
                            dEstAnnCostSave = CType(txtNextAnnCostSave.Text.Trim, Double)
                            lblReqAnnCostChngRsn.Visible = False
                            lblAnnCostChngRsn.Visible = False
                            txtAnnCostChngRsn.Visible = False
                        End If

                        '************************************
                        '* Capture CapEx Change History
                        '************************************
                        If txtHDCapEx.Text <> txtNextCapEx.Text Then
                            If txtDateSubmitted.Text <> "" Then
                                CRModule.InsertCostReductionHistory(ViewState("pProjNo"), DefaultTMID, txtCapExChngRsn.Text, "CAPEX", dCAPEX, txtNextCapEx.Text)
                                SendEmailToDefaultAdmin = True
                            End If
                            ''Assign CapEx with new value.
                            dCAPEX = CType(txtNextCapEx.Text.Trim, Double)
                            lblReqCapExChngRsn.Visible = False
                            lblCapExChngRsn.Visible = False
                            txtCapExChngRsn.Visible = False
                        End If

                        '************************************
                        '* Capture Success Rate Change History
                        '************************************
                        If txtHDSuccessRate.Text.Trim <> txtNextSuccessRate.Text.Trim Then
                            If txtDateSubmitted.Text <> "" Then
                                CRModule.InsertCostReductionHistory(ViewState("pProjNo"), DefaultTMID, txtSuccessRateChngRsn.Text, "Success Rate", dSuccessRate, txtNextSuccessRate.Text)
                                SendEmailToDefaultAdmin = True
                            End If
                            ''Assign SuccessRate with new value.
                            If txtNextSuccessRate.Text.Trim <> "" Then
                                dSuccessRate = IIf(CType(txtNextSuccessRate.Text.Trim, Double) > 100, 100, CType(txtNextSuccessRate.Text.Trim, Double))
                            End If

                            lblReqSuccessRateChngRsn.Visible = False
                            lblSuccessRateChngRsn.Visible = False
                            txtSuccessRateChngRsn.Visible = False
                        End If

                        '***************
                        '* Update Data
                        '***************
                        CRModule.UpdateCostReduction(ViewState("pProjNo"), txtDescription.Text, iCategory, iLeader, ddUGNFacility.SelectedValue, iCommodity, EstImpDate, dCompletion, iRFDNo, dSuccessRate, dEstAnnCostSave, dCAPEX, cbOffsetsCostDowns.Checked, cbPlantControllerReviewed.Checked, txtCapExProjNo.Text, DefaultUser, DefaultDate, False)

                        ''***************
                        ''* Send Notification to Default Admin when values change
                        ''***************
                        If SendEmailToDefaultAdmin = True And txtDateSubmitted.Text <> "" Then
                            SendEmailWhenValuesChange(ViewState("pProjNo"), DefaultTMID, txtImpDateChngRsn.Text, "Implementation Date", txtHDEstImpDate.Text, txtNextImpDate.Text, txtAnnCostChngRsn.Text, "Annual Cost Save", txtHDEstAnnCostSave.Text, txtNextAnnCostSave.Text, txtCapExChngRsn.Text, "CAPEX", txtHDCapEx.Text, txtNextCapEx.Text, txtSuccessRateChngRsn.Text, "Success Rate", txtHDSuccessRate.Text, dSuccessRate)
                        End If

                        ''***************
                        ''* Send Notification that CapEx ProjectNo has been added
                        ''***************
                        If ViewState("OriginalCapExProjNo") <> txtCapExProjNo.Text.Trim Then
                            SendEmailtoExpProjLeader(txtCapExProjNo.Text, ViewState("pProjNo"))
                        End If

                        '*******************
                        '* Reload the data
                        '*******************
                        BindData(ViewState("pProjNo"))
                        gvCRHistory.DataBind()

                        ''*************************************************
                        '' "Form Level Security using Roles &/or Subscriptions"
                        ''*************************************************
                        CheckRights() '"Form Level Security using Roles &/or Subscriptions"                
                    End If
                Else ''INSERT NEW RECORD
                    Dim NewTestReq As Boolean = False
                    Dim Consult As Boolean = False
                    Dim Current As Boolean = False

                    '***************
                    '* Save Data
                    '***************
                    CRModule.InsertCostReduction(txtDescription.Text, iCategory, iLeader, ddUGNFacility.SelectedValue, iCommodity, txtEstImpDate.Text, dCompletion, iRFDNo, dSuccessRate, dEstAnnCostSave, dCAPEX, cbOffsetsCostDowns.Checked, 0, txtCapExProjNo.Text, DefaultUser, DefaultDate)

                    '***************
                    '* Locate Next available ProjectNo based on Facility selection
                    '***************
                    Dim ds1 As DataSet = Nothing
                    ds1 = CRModule.GetLastCostReductionProjectNo(iLeader, ddUGNFacility.SelectedValue, iCommodity, iCategory, txtDescription.Text, DefaultUser, DefaultDate)

                    ViewState("pProjNo") = CType(ds1.Tables(0).Rows(0).Item("LastProjectNo").ToString, String)

                    '***************
                    '* Send email notification to CapEx Project Leader
                    '***************
                    If txtCapExProjNo.Text <> Nothing Then
                        SendEmailtoExpProjLeader(txtCapExProjNo.Text, ViewState("pProjNo"))
                    End If

                    '***************
                    '* Redirect user back to the page.
                    '***************
                    Response.Redirect("CostReduction.aspx?pProjNo=" & ViewState("pProjNo"), False)

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

        lblErrorsButtons.Text = lblErrors.Text

    End Sub 'btnSave1_Click

    Public Sub SendEmailtoExpProjLeader(ByVal ExpProjNo As String, ByVal CRProjNo As Integer)

        Try
            ''**************************************************************************
            ''This section is used to notify CapEx Project Leader of assigned Cost Reduction Project number
            ''**************************************************************************
            Dim i As Integer = 0
            Dim ds As DataSet = New DataSet
            Dim dsPLdr As DataSet = New DataSet
            Dim dsCC As DataSet = New DataSet
            Dim EmailTO As String = Nothing
            Dim CurrentEmpEmail As String = Nothing
            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim DefaultDate As Date = Date.Today
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim ExpProjPath As String = Nothing
            Dim ProjectTitle As String = Nothing
            Dim SubjectText As String = Nothing
            Dim EmpName As String = Nothing
            Dim EmailCC As String = Nothing


            '***************
            '* Locate ExpProj_Assets data
            '***************
            Select Case ExpProjNo.Substring(0, 1)
                Case "A" ''Capital Project: Property Plant Equipment (Assets)
                    '***************
                    '* Update ExpProj_Assets table with Cost Reduction Project No  
                    '***************
                    CRModule.UpdateExpProjAssetsCRProjectNo(txtCapExProjNo.Text, CRProjNo, DefaultUser, DefaultDate)

                    ds = EXPModule.GetExpProjAssets(ExpProjNo, "", "", "", 0, "", 0, "", "")
                    If commonFunctions.CheckDataSet(ds) = True Then
                        SubjectText = "Property Plant Equipment (Asset): "
                        ProjectTitle = ds.Tables(0).Rows(0).Item("ProjectTitle")
                        ExpProjPath = "<a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/AssetsExpProj.aspx?pProjNo=" & ExpProjNo & "'>" & ExpProjNo & "</a> '" & ProjectTitle & "' "

                        'Locate Project Leader
                        dsPLdr = EXPModule.GetExpProjAssetsLead(ExpProjNo)
                    End If
                Case "R" ''Capital Project: Repair
                    '***************
                    '* Update ExpProj_Repair table with Cost Reduction Project No  
                    '***************
                    CRModule.UpdateExpProjRepairCRProjectNo(txtCapExProjNo.Text, CRProjNo, DefaultUser, DefaultDate)

                    ds = EXPModule.GetExpProjRepair(ExpProjNo, "", "", "", 0, "", "")
                    If commonFunctions.CheckDataSet(ds) = True Then
                        SubjectText = "Repair Expense: "
                        ProjectTitle = ds.Tables(0).Rows(0).Item("ProjectTitle")
                        ExpProjPath = "<a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/RepairExpProj.aspx?pProjNo=" & ExpProjNo & "'>" & ExpProjNo & "</a> '" & ProjectTitle & "' "

                        'Locate Project Leader
                        dsPLdr = EXPModule.GetExpProjRepairLead(ExpProjNo)
                    End If
                Case "D" ''Capital Project: Development
                    '***************
                    '* Update ExpProj_Development table with Cost Reduction Project No  
                    '***************
                    CRModule.UpdateExpProjDevelopmentCRProjectNo(txtCapExProjNo.Text, CRProjNo, DefaultUser, DefaultDate)

                    ds = EXPModule.GetExpProjDevelopment(ExpProjNo, "", "", 0, 0, 0, "", 0, 0, "", 0, "")
                    If commonFunctions.CheckDataSet(ds) = True Then
                        SubjectText = "Development Project: "
                        ProjectTitle = ds.Tables(0).Rows(0).Item("ProjectTitle")
                        ExpProjPath = "<a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/DevelopmentExpProj.aspx?pProjNo=" & ExpProjNo & "'>" & ExpProjNo & "</a> '" & ProjectTitle & "' "

                        'Locate Project Leader
                        dsPLdr = EXPModule.GetExpProjDevelopmentLead(ExpProjNo)
                    End If
            End Select

            ''Check that the recipient(s) is a valid Team Member
            If commonFunctions.CheckDataSet(dsPLdr) = True Then
                For i = 0 To dsPLdr.Tables.Item(0).Rows.Count - 1
                    If (dsPLdr.Tables(0).Rows(i).Item("WorkStatus") = True) Or (dsPLdr.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                        If EmailTO = Nothing Then
                            EmailTO = dsPLdr.Tables(0).Rows(i).Item("Email")
                        Else
                            EmailTO = EmailTO & ";" & dsPLdr.Tables(0).Rows(i).Item("Email")
                        End If
                        If EmpName = Nothing Then
                            EmpName = dsPLdr.Tables(0).Rows(i).Item("TMName") & ", "
                        Else
                            EmpName = EmpName & dsPLdr.Tables(0).Rows(i).Item("TMName") & ", "
                        End If
                    End If
                Next
            End If 'EOF If commonFunctions.CheckDataset(dsPLdr) = True

            ''********************************************************
            ''Send Notification only if there is a valid Email Address
            ''********************************************************
            If EmailTO <> Nothing Then
                Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                Dim SendTo As MailAddress = Nothing
                Dim MyMessage As MailMessage

                'send to Test or Production
                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    SendTo = New MailAddress(CurrentEmpEmail) 'use for testing only
                    MyMessage = New MailMessage(SendFrom, SendTo)
                Else
                    MyMessage = New MailMessage
                    'build email To list
                    Dim emailList As String() = EmailTO.Split(";")

                    For i = 0 To UBound(emailList)
                        If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                            MyMessage.To.Add(emailList(i))
                        End If
                    Next i
                    MyMessage.From = New MailAddress(CurrentEmpEmail)
                    MyMessage.CC.Add(CurrentEmpEmail)
                    'MyMessage.Bcc.Add("lynette.rey@ugnusa.com")
                    'MyMessage.Bcc.Add("roderick.carlson@ugnauto.com")
                End If

                ''Locate CR Team Leader according to UGN Facility
                dsCC = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(75, ddUGNFacility.SelectedValue)
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                            If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And (DefaultTMID <> dsCC.Tables(0).Rows(i).Item("TMID")) Then ''change to DefaultTMID   
                                If EmailCC = Nothing Then
                                    EmailCC = dsCC.Tables(0).Rows(i).Item("Email")
                                    If InStr(strProdOrTestEnvironment, "Prod_", CompareMethod.Text) > 0 Then
                                        MyMessage.CC.Add(dsCC.Tables(0).Rows(i).Item("Email"))
                                    End If
                                Else
                                    EmailCC = EmailCC & ";" & dsCC.Tables(0).Rows(i).Item("Email")
                                    If InStr(strProdOrTestEnvironment, "Prod_", CompareMethod.Text) > 0 Then
                                        MyMessage.CC.Add(dsCC.Tables(0).Rows(i).Item("Email"))
                                    End If
                                End If
                            End If 'EOF If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) 
                        End If 'EOF If (dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                    Next 'EOF For i = 0 To
                End If 'EOF commonFunctions.CheckDataset(dsCC) = True 

                ''Test or Production Message display
                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    MyMessage.Subject = "TEST: "
                    MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                Else
                    MyMessage.Subject = ""
                    MyMessage.Body = ""
                End If

                MyMessage.Subject &= "Cost Reduction Project Assigned to " & SubjectText & ExpProjNo & " - " & ProjectTitle
                MyMessage.Body &= EmpName
                MyMessage.Body &= "<p>" & ExpProjPath & " was assigned to Cost Reduction Project Number:  <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/CostReduction.aspx?pProjNo=" & CRProjNo & "'>" & CRProjNo & "</a> '" & txtDescription.Text & "' ."

                Select Case ExpProjNo.Substring(0, 1)
                    Case "A" ''Capital Project: Property Plant Equipment (Assets)
                        MyMessage.Body &= " Please review and forward record for approval."
                End Select

                MyMessage.Body &= " </p><br/>Thank you."


                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                    MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                End If

                ''*****************
                ''History Tracking
                ''*****************
                EXPModule.InsertExpProjAssetsHistory(ExpProjNo, ProjectTitle, DefaultTMID, "Cost Reduction Reference Assigned - Project Leader notified.", "", "", "", "")

                ''**********************************
                ''Connect & Send email notification
                ''**********************************
                MyMessage.IsBodyHtml = True
                Dim emailClient As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)
                emailClient.Send(MyMessage)

                Try
                    emailClient.Send(MyMessage)
                    lblErrors.Text &= "Email Notification sent."
                Catch ex As Exception
                    lblErrors.Text &= "Email Notification queued."
                    UGNErrorTrapping.InsertEmailQueue("Cost Reduction Notification", CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                End Try

                ''**********************************
                ''Rebind the data to the form
                ''********************************** 
                BindData(CRProjNo)

            End If 'EOF  If EmailTO <> Nothing
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblErrorsButtons.Text = lblErrors.Text

    End Sub
    Public Sub SendEmailWhenValuesChange(ByVal ProjNo As String, ByVal DefaultTMID As Int16, ByVal ImpDateChngRsn As String, ByVal ImpFieldName As String, ByVal EstImpDate As String, ByVal NextImpDate As String, ByVal AnnCostChngRsn As String, ByVal AnnFieldName As String, ByVal EstAnnCostSave As Decimal, ByVal NextAnnCostSave As Decimal, ByVal CapExChngRsn As String, ByVal CapExField As String, ByVal CapEx As Decimal, ByVal NextCapEx As Decimal, ByVal SuccessRateChngRsn As String, ByVal SuccessRateField As String, ByVal SuccessRate As Decimal, ByVal NextSuccessRate As Decimal)

        Try
            ''**************************************************************************
            ''Build Email Notification, Sender, Recipient(s), Subject, Body information
            ''**************************************************************************
            Dim i As Integer = 0
            Dim ds As DataSet = New DataSet
            Dim EmailTO As String = Nothing
            Dim CurrentEmpEmail As String = Nothing
            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            If (ddProjectCategory.SelectedValue = 5 Or ddProjectCategory.SelectedValue = 6) Then 'for kaizen events only notify kaizen group
                ds = commonFunctions.GetTeamMemberBySubscription(85) ''Darell Cook
            Else
                ds = commonFunctions.GetTeamMemberBySubscription(77)
            End If
            ''Check that the recipient(s) is a valid Team Member
            If commonFunctions.CheckDataSet(ds) = True Then
                For i = 0 To ds.Tables.Item(0).Rows.Count - 1
                    If (ds.Tables(0).Rows(i).Item("WorkStatus") = True) And ((ds.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And (ds.Tables(0).Rows(i).Item("TMID") <> ddLeader.SelectedValue)) Then
                        If EmailTO = Nothing Then
                            EmailTO = ds.Tables(0).Rows(i).Item("Email")
                        Else
                            EmailTO = EmailTO & ";" & ds.Tables(0).Rows(i).Item("Email")
                        End If
                    End If
                Next
            End If

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pProjNo") <> Nothing Then
                    If EmailTO <> Nothing Then
                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim SendTo As MailAddress = Nothing
                        SendTo = New MailAddress(EmailTO)
                        'SendTo = New MailAddress(CurrentEmpEmail) 'use for testing only

                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            SendTo = New MailAddress(CurrentEmpEmail) 'use for testing only
                        Else
                            'SendTo = New MailAddress(EmailTO)
                            SendTo = New MailAddress(CurrentEmpEmail)
                        End If

                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)
                        'MyMessage.CC.Add(CurrentEmpEmail)

                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            'build email To list
                            Dim emailList As String() = EmailTO.Split(";")

                            For i = 0 To UBound(emailList)
                                If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                                    MyMessage.To.Add(emailList(i))
                                End If
                            Next i
                        End If

                        MyMessage.Subject &= "Cost Reduction Project No: " & ProjNo & " - Changed Value(s) Alert."
                        MyMessage.Body &= "<p><font size='2' face='Verdana'>There was a change to the Cost Reduction "
                        MyMessage.Body &= "Project No: <u>" & ProjNo & "</u>. "
                        MyMessage.Body &= "<br/><br/>Description: " & txtDescription.Text
                        MyMessage.Body &= "<br/><br/>Open IE browser, wait a few seconds... then <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/CostReduction.aspx?pEM=1&pProjNo=" & ViewState("pProjNo") & "'>click here</a> to access record.</font></p>"
                        MyMessage.Body &= "<table width='80%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"

                        If ImpDateChngRsn <> "" Or AnnCostChngRsn <> "" Or CapExChngRsn <> "" Or SuccessRateChngRsn <> "" Then
                            MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                            MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Field Change</strong></font></td>"
                            MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Previous Value</strong></font></td>"
                            MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>New Value</strong></font></td>"
                            MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Change Reason</strong></font></td>"
                            MyMessage.Body &= "</tr>"
                        End If

                        ''** Implementation Date Change **
                        If ImpDateChngRsn <> "" Then
                            MyMessage.Body &= "<tr style='border-color:white;'>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ImpFieldName & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & EstImpDate & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & NextImpDate & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ImpDateChngRsn & "</font></td>"
                            MyMessage.Body &= "</tr>"
                        End If

                        ''** Annual Cost Change **
                        If AnnCostChngRsn <> "" Then
                            MyMessage.Body &= "<tr style='border-color:white;'>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & AnnFieldName & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>$ " & EstAnnCostSave & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>$ " & NextAnnCostSave & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & AnnCostChngRsn & "</font></td>"
                            MyMessage.Body &= "</tr>"
                        End If

                        ''** CapEx Change **
                        If CapExChngRsn <> "" Then
                            MyMessage.Body &= "<tr style='border-color:white;'>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & CapExField & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>$ " & CapEx & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>$ " & NextCapEx & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & CapExChngRsn & "</font></td>"
                            MyMessage.Body &= "</tr>"
                        End If

                        ''** Success Rate Change **
                        If SuccessRateChngRsn <> "" Then
                            MyMessage.Body &= "<tr style='border-color:white;'>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & SuccessRateField & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & SuccessRate & " %</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & NextSuccessRate & " %</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & SuccessRateChngRsn & "</font></td>"
                            MyMessage.Body &= "</tr>"
                        End If

                        MyMessage.Body &= "</Table>"

                        MyMessage.IsBodyHtml = True

                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                        End If

                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Dim emailClient As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

                        Try
                            emailClient.Send(MyMessage)
                            lblErrors.Text &= "Email Notification sent."
                        Catch ex As Exception
                            lblErrors.Text &= "Email Notification queued."
                            UGNErrorTrapping.InsertEmailQueue("Cost Reduction Values Change", CurrentEmpEmail, EmailTO, "", MyMessage.Subject, MyMessage.Body, "")
                        End Try
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

    End Sub 'EOF SendEmailWhenValuesChange

    Public Sub SendEmailWhenStepsChange(ByVal ProjNo As String)

        Try
            ''**************************************************************************
            ''Build Email Notification, Sender, Recipient(s), Subject, Body information
            ''**************************************************************************
            Dim i As Integer = 0
            Dim ds As DataSet = New DataSet
            Dim CurrentEmpEmail As String = Nothing
            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim dsSubscription As DataSet
            Dim dsTeamMember As DataSet

            Dim iRowCounter As Integer = 0
            Dim iLeaderID As Integer = 0

            Dim EmailTO As String = ""
            Dim strUGNFacility As String = ""

            If ddLeader.SelectedIndex > 0 Then
                iLeaderID = ddLeader.SelectedValue
            End If

            If ddUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue
            End If

            If (ddProjectCategory.SelectedValue = 5 Or ddProjectCategory.SelectedValue = 6) Then 'for kaizen events only notify kaizen group
                '****** DO NOTHING ********
            Else
                'get Plant Managers by UGN Facility
                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(150, strUGNFacility)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    For iRowCounter = 0 To dsSubscription.Tables(0).Rows.Count - 1
                        If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") IsNot System.DBNull.Value Then
                            If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                                If dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString <> "" Then
                                    If EmailTO <> "" Then
                                        EmailTO &= ";"
                                    End If
                                    EmailTO &= dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString
                                End If

                            End If
                        End If
                    Next
                End If

                'get Plant Controller by UGN Facility
                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(20, strUGNFacility)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    For iRowCounter = 0 To dsSubscription.Tables(0).Rows.Count - 1
                        If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") IsNot System.DBNull.Value Then
                            If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                                If dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString <> "" Then
                                    If EmailTO <> "" Then
                                        EmailTO &= ";"
                                    End If
                                    EmailTO &= dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString
                                End If

                            End If
                        End If
                    Next
                End If

                'get Plant Controller from Tinley
                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(20, "UT")
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    For iRowCounter = 0 To dsSubscription.Tables(0).Rows.Count - 1
                        If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") IsNot System.DBNull.Value Then
                            If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                                If dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString <> "" Then
                                    If EmailTO <> "" Then
                                        EmailTO &= ";"
                                    End If
                                    EmailTO &= dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString
                                End If

                            End If
                        End If
                    Next
                End If
            End If

            'get Project Leader Email
            dsTeamMember = SecurityModule.GetTeamMember(iLeaderID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                    If dsTeamMember.Tables(0).Rows(0).Item("Working") = True Then
                        If EmailTO <> "" Then
                            EmailTO &= ";"
                        End If
                        EmailTO &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                    End If
                End If
            End If

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If EmailTO <> "" And CurrentEmpEmail <> "" Then
                If ViewState("pProjNo") <> "" Then

                    Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim SendTo As MailAddress = Nothing

                    If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                        SendTo = New MailAddress(CurrentEmpEmail) 'use for testing only
                    Else
                        'SendTo = New MailAddress(EmailTO)
                        SendTo = New MailAddress(CurrentEmpEmail)
                    End If

                    Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                    If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                    Else
                        'build email To list
                        Dim emailList As String() = EmailTO.Split(";")

                        For i = 0 To UBound(emailList)
                            If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                                MyMessage.To.Add(emailList(i))
                            End If
                        Next i
                    End If

                    MyMessage.Subject &= "Cost Reduction Project No: " & ProjNo & " - has an updated step."
                    MyMessage.Body &= "<p><font size='2' face='Verdana'>There was an updated step to the Cost Reduction "
                    MyMessage.Body &= "Project No: <u>" & ProjNo & "</u>. "
                    MyMessage.Body &= "<br/><br/>Description: " & txtDescription.Text
                    MyMessage.Body &= "<br/><br/> Open IE browser, wait a few seconds... then <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/CostReduction.aspx?pEM=1&pProjNo=" & ViewState("pProjNo") & "'>click here</a> to access record.</font></p>"
                    MyMessage.Body &= "<table width='60%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"

                    MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                    MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Team Member</strong></font></td>"
                    MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Comments</strong></font></td>"
                    MyMessage.Body &= "</tr>"

                    MyMessage.Body &= "<tr style='border-color:#EBEBEB;'>"
                    MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>" & HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value & "</strong></font></td>"
                    MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>" & txtSteps.Text.Trim & "</strong></font></td>"
                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "</table>"

                    MyMessage.IsBodyHtml = True

                    If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                    End If

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Dim emailClient As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)
                    'emailClient.Send(MyMessage)

                    Try
                        emailClient.Send(MyMessage)
                        lblErrors.Text &= "Email Notification sent."
                    Catch ex As Exception
                        lblErrors.Text &= "Email Notification queued."
                        UGNErrorTrapping.InsertEmailQueue("Cost Reduction Steps Change", CurrentEmpEmail, EmailTO, "", MyMessage.Subject, MyMessage.Body, "")
                    End Try

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

    End Sub 'EOF SendEmailWhenValuesChange
    Protected Sub btnReset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset1.Click, btnReset2.Click, btnReset3.Click

        ClearMessages()

        If ViewState("pProjNo") = Nothing Then
            Response.Redirect("CostReduction.aspx", False)
        Else
            Response.Redirect("CostReduction.aspx?pProjNo=" & ViewState("pProjNo"), False)
        End If

        lblErrorsButtons.Text = lblErrors.Text

    End Sub 'EOF btnReset1_Click

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        Try
            ClearMessages()

            Dim deleteRec As Boolean = False


            '***************
            '* Verify that Capital Project is not assigned to an ExpProj_"" table
            '***************
            Dim ds As DataSet = New DataSet
            If txtCapExProjNo.Text <> Nothing Then
                Select Case txtCapExProjNo.Text.Substring(0, 1)
                    Case "A" ''Capital Project: Property Plant Equipment (Assets)
                        ds = EXPModule.GetExpProjAssets(txtCapExProjNo.Text, "", "", "", 0, "", 0, "", "")
                        If commonFunctions.CheckDataSet(ds) = True Then
                            If ds.Tables(0).Rows(0).Item("CRProjectNo").ToString() <> Nothing Then
                                lblErrors.Text = "Delete Cancelled. This record is referenced in Capital Project: " & txtCapExProjNo.Text

                                deleteRec = False
                            Else
                                deleteRec = True
                            End If
                        End If
                End Select
            Else
                deleteRec = True
            End If

            If deleteRec = True Then
                '***************
                '* Delete Record
                '***************
                CRModule.DeleteCostReduction(ViewState("pProjNo"))

                '***************
                '* Redirect user back to the search page.
                '***************
                Response.Redirect("CostReductionList.aspx", False)
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblErrorsButtons.Text = lblErrors.Text

    End Sub 'EOF btnDelete_Click

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            ClearMessages()

            Dim dEstAnnCostSave As Decimal = 0
            Dim dCAPEX As Decimal = 0
            Dim dSuccessRate As Decimal = 0
            Dim dCompletion As Double = 0

            Dim iCategory As Integer = 0
            Dim iLeader As Integer = 0
            Dim iRFDNo As Integer = 0

            If txtHDEstAnnCostSave.Text.Trim <> "" Then
                dEstAnnCostSave = CType(txtHDEstAnnCostSave.Text.Trim, Double)
            End If

            If txtHDCapEx.Text.Trim <> "" Then
                dCAPEX = CType(txtHDCapEx.Text.Trim, Double)
            End If

            If txtHDSuccessRate.Text.Trim <> "" Then
                dSuccessRate = CType(txtHDSuccessRate.Text.Trim, Double)
            End If

            If txtCompletion.Text <> "" Then
                dCompletion = CType(txtCompletion.Text, Double)
            End If

            If ddProjectCategory.SelectedIndex > 0 Then
                iCategory = ddProjectCategory.SelectedValue
            End If

            If ddLeader.SelectedIndex > 0 Then
                iLeader = ddLeader.SelectedValue
            End If

            If txtRFDNo.Text.Trim <> "" Then
                iRFDNo = txtRFDNo.Text.Trim
            End If

            ''**************************************************************************
            ''Check to see if Proposed Details page is complete before allowing submission
            ''**************************************************************************
            If ViewState("isProposedDetailsExist") = True And (dEstAnnCostSave <> 0 Or dCAPEX <> 0) Then
                ''**************************************************************************
                ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                ''**************************************************************************
                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                Dim DefaultDate As Date = Date.Today
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                Dim dsUT As DataSet = New DataSet
                Dim dsCC As DataSet = New DataSet
                Dim dsCommodity As DataSet = New DataSet
                Dim EmpName As String = Nothing
                Dim EmailTO As String = Nothing
                Dim EmailCC As String = Nothing
                Dim Email2CC As String = Nothing
                Dim CurrentEmpEmail As String = Nothing
                Dim EstImpDate As String = txtHDEstImpDate.Text
                Dim i As Integer = 0

                CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value

                '********
                '* Only users with valid email accounts can send an email.
                '********
                If HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value <> Nothing Then
                    If ViewState("pProjNo") <> Nothing Then
                        ''************************************
                        ''Notify Coporate TM's
                        ''************************************
                        dsUT = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(76, "UT")
                        ''Check that the recipient(s) is a valid Team Member
                        If dsUT.Tables.Count > 0 And (dsUT.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To dsUT.Tables.Item(0).Rows.Count - 1
                                If (dsUT.Tables(0).Rows(i).Item("WorkStatus") = True) And ((dsUT.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And (dsUT.Tables(0).Rows(i).Item("TMID") <> ddLeader.SelectedValue)) Then
                                    If EmailTO = Nothing Then
                                        EmailTO = dsUT.Tables(0).Rows(i).Item("Email")
                                    Else
                                        EmailTO = EmailTO & ";" & dsUT.Tables(0).Rows(i).Item("Email")
                                    End If
                                End If
                            Next
                        End If

                        ''************************************
                        ''Notify Plant Level TM's
                        ''************************************
                        If (ddProjectCategory.SelectedValue = 5 Or ddProjectCategory.SelectedValue = 6) Then 'for kaizen events only notify kaizen group
                            dsUT = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(86, ddUGNFacility.SelectedValue)
                        Else
                            dsUT = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(76, ddUGNFacility.SelectedValue)
                        End If

                        ''Check that the recipient(s) is a valid Team Member
                        If dsUT.Tables.Count > 0 And (dsUT.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To dsUT.Tables.Item(0).Rows.Count - 1
                                If (dsUT.Tables(0).Rows(i).Item("WorkStatus") = True) And ((dsUT.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And (dsUT.Tables(0).Rows(i).Item("TMID") <> ddLeader.SelectedValue)) Then
                                    If EmailTO = Nothing Then
                                        EmailTO = dsUT.Tables(0).Rows(i).Item("Email")
                                    Else
                                        EmailTO = EmailTO & ";" & dsUT.Tables(0).Rows(i).Item("Email")
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If

                If EmailTO <> Nothing Then
                    Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim SendTo As MailAddress = Nothing
                    Dim MyMessage As MailMessage
                    'send to Test or Production
                    If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                        SendTo = New MailAddress(CurrentEmpEmail) 'use for testing only
                        MyMessage = New MailMessage(SendFrom, SendTo)

                    Else
                        MyMessage = New MailMessage
                        'build email To list
                        Dim emailList As String() = EmailTO.Split(";")

                        For i = 0 To UBound(emailList)
                            If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                                MyMessage.To.Add(emailList(i))
                            End If
                        Next i
                        MyMessage.From = New MailAddress(CurrentEmpEmail)
                        MyMessage.CC.Add(CurrentEmpEmail)
                        'MyMessage.Bcc.Add("lynette.rey@ugnauto.com")
                        'MyMessage.Bcc.Add("roderick.carlson@ugnauto.com")
                    End If

                    ''***************
                    ''Save any changed data prior to submitting to Assigned Team Members
                    ''**************
                    CRModule.UpdateCostReduction(ViewState("pProjNo"), txtDescription.Text, iCategory, iLeader, ddUGNFacility.SelectedValue, ddCommodity.SelectedValue, EstImpDate, dCompletion, iRFDNo, dSuccessRate, dEstAnnCostSave, dCAPEX, cbOffsetsCostDowns.Checked, cbPlantControllerReviewed.Checked, txtCapExProjNo.Text, DefaultUser, DefaultDate, True)

                    If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                    Else
                        MyMessage.CC.Add(CurrentEmpEmail)
                        MyMessage.Bcc.Add("Lynette.Rey@ugnauto.com")
                    End If

                    MyMessage.Subject &= "Cost Reduction Project No:" & ViewState("pProjNo") & " - Impl. Date: " & txtNextImpDate.Text
                    MyMessage.Body &= "<p><font size='2' face='Verdana'>A Cost Reduction Project "
                    MyMessage.Body &= "No: <u>" & ViewState("pProjNo") & "</u> was entered in the UGNDB. "
                    MyMessage.Body &= "<br/> Open IE browser, wait a few seconds... then <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/CostReduction.aspx?pProjNo=" & ViewState("pProjNo") & "'>click here</a> to access record.</font></p>"
                    MyMessage.Body &= "<table width='60%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"

                    MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                    MyMessage.Body &= "<td width='388' colspan='3'><font size='2' face='Verdana'><strong>Description</strong></font></td>"
                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "<tr style='border-color:white;'>"
                    MyMessage.Body &= "<td height='25' colspan='3'><font size='2' face='Verdana'>" & txtDescription.Text & "</font></td>"
                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                    MyMessage.Body &= "<td width='100px'><font size='2' face='Verdana'><strong>Project Leader</strong></font></td>"
                    MyMessage.Body &= "<td width='100px><font size='2' face='Verdana'><strong>UGN Location </strong></font></td>"
                    MyMessage.Body &= "<td width='100px><font size='2' face='Verdana'><strong>Commodity</strong></font></td>"
                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "<tr style='border-color:white;'>"
                    MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ddLeader.SelectedItem.Text & "</font></td>"
                    MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ddUGNFacility.SelectedItem.Text & "</font></td>"
                    MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ddCommodity.SelectedItem.Text & "</font></td>"
                    MyMessage.Body &= "</tr>"

                    ''***************************************************
                    ''Get list of Supporting Documentation
                    ''***************************************************
                    Dim dsAED As DataSet
                    dsAED = CRModule.GetCostReductionDocument(ViewState("pProjNo"), 0)
                    If dsAED.Tables.Count > 0 And (dsAED.Tables.Item(0).Rows.Count > 0) Then
                        MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
                        MyMessage.Body &= "<td colspan='2'><font size='2' face='Verdana'><strong>REQUIRED FORMS / DOCUMENTS:</strong></font></td>"
                        MyMessage.Body &= "</tr>"
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td colspan='2'>"
                        MyMessage.Body &= "<font size='2' face='Verdana'><table >"
                        MyMessage.Body &= "  <tr>"
                        MyMessage.Body &= "   <td width='250px'><b>Form Description</b></td>"
                        MyMessage.Body &= "   <td width='250px'>&nbsp;</td>"
                        MyMessage.Body &= "</tr>"
                        For i = 0 To dsAED.Tables.Item(0).Rows.Count - 1
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td height='25'>" & dsAED.Tables(0).Rows(i).Item("Description") & "</td>"
                            MyMessage.Body &= "<td height='25'><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/CostReductionDocument.aspx?pProjNo=" & ViewState("pProjNo") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
                            MyMessage.Body &= "</tr>"
                        Next
                        MyMessage.Body &= "</table></font>"
                        MyMessage.Body &= "</tr>"
                    End If
                    MyMessage.Body &= "</table>"

                    If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                        MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"

                        EmailTO = CurrentEmpEmail 'use for testing only   
                        EmailCC = ""
                    End If

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    MyMessage.IsBodyHtml = True
                    Dim emailClient As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

                    Try
                        emailClient.Send(MyMessage)
                        lblErrors.Text &= "Email Notification sent."
                    Catch ex As Exception
                        lblErrors.Text &= "Email Notification queued."
                        UGNErrorTrapping.InsertEmailQueue("Cost Reduction Submission", CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                    End Try

                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pProjNo"))
                    CheckRights() '"Form Level Security using Roles &/or Subscriptions"
                    lblErrors.Text = "Notification Sent Successfully."
                End If
            Else
                lblErrors.Text += "<br/>Error: The project can only be submitted when the proposed details page is completed and the savings is greater than 0. Please click the proposed details button to complete this."
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblErrorsButtons.Text = lblErrors.Text

    End Sub 'EOF btnSubmit_Click

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        ClearMessages()

        '***************
        '* Redirect user back to the search page.
        '***************
        Response.Redirect("CostReduction.aspx?pProjNo=0", False)

        lblErrorsButtons.Text = lblErrors.Text

    End Sub 'EOF btnAdd_Click

    Protected Sub btnSaveToGrid1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveToGrid1.Click
        Try
            ClearMessages()

            If ViewState("pProjNo") <> Nothing Then
                Dim DefaultDate As Date = Date.Today
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                If ViewState("pStatusID") = 0 And txtStatus.Text <> Nothing Then
                    '***************
                    '* Insert Team Members Status/Updates to table
                    '***************
                    CRModule.InsertCostReductionStatus(ViewState("pProjNo"), DefaultDate, txtStatus.Text, DefaultUser)
                    gvStatus.DataBind()
                    txtStatus.Text = Nothing
                    BindData(ViewState("pProjNo"))
                Else
                    '***************
                    '* Update Team Members Status/Updates to table
                    '***************
                    CRModule.UpdateCostReductionStatus(ViewState("pStatusID"), ViewState("pProjNo"), txtStatus.Text, DefaultUser)
                    BindData(ViewState("pProjNo"))
                    Response.Redirect("CostReduction.aspx?pProjNo=" & ViewState("pProjNo"), False)
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

        lblErrorsButtons.Text = lblErrors.Text

    End Sub 'EOF btnSaveToGrid1_Click

    Protected Sub btnSaveToGrid2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveToGrid2.Click

        Try
            ClearMessages()

            If ViewState("pProjNo") <> Nothing Then

                Dim DefaultDate As Date = Date.Today
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

                Dim iTeamMemberID As Integer = 0

                If ddTeamMember.SelectedIndex > 0 Then
                    iTeamMemberID = ddTeamMember.SelectedValue
                End If

                SendEmailWhenStepsChange(ViewState("pProjNo"))

                If ViewState("pStepID") = 0 Then
                    '***************
                    '* Insert Team Members Steps/Comments to table
                    '***************
                    CRModule.InsertCostReductionSteps(ViewState("pProjNo"), DefaultDate, iTeamMemberID, txtSteps.Text, DefaultUser)
                    gvSteps.DataBind()
                    ddTeamMember.SelectedValue = Nothing
                    txtSteps.Text = Nothing
                    BindData(ViewState("pProjNo"))
                    Response.Redirect("CostReduction.aspx?pProjNo=" & ViewState("pProjNo"), False)
                Else
                    '***************
                    '* Update Team Members Steps/Comments to table
                    '***************
                    CRModule.UpdateCostReductionSteps(ViewState("pStepID"), ViewState("pProjNo"), iTeamMemberID, txtSteps.Text, DefaultUser)
                    BindData(ViewState("pProjNo"))
                    Response.Redirect("CostReduction.aspx?pProjNo=" & ViewState("pProjNo"), False)
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

        lblErrorsButtons.Text = lblErrors.Text

    End Sub 'EOF btnSaveToGrid_Click

    Protected Sub gvStatus_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvStatus.RowCommand
        'do nothing
    End Sub 'EOF gvStatus_RowCommand

    Protected Sub gvStatus_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvStatus.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' reference the Delete ImageButton
                Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
                If imgBtn IsNot Nothing Then
                    Dim db As ImageButton = CType(e.Row.Cells(3).Controls(1), ImageButton)

                    ' Get information about the product bound to the row
                    If db.CommandName = "Delete" Then
                        Dim price As CostReduction.Cost_Reduction_StatusRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, CostReduction.Cost_Reduction_StatusRow)
                        db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record?');")
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
    End Sub 'EOF gvStatus_RowDataBound

    Protected Sub gvSteps_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSteps.RowCommand
        'do nothing
    End Sub 'EOF gvSteps_RowCommand

    Protected Sub gvSteps_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSteps.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' reference the Delete ImageButton
                Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
                If imgBtn IsNot Nothing Then
                    Dim db As ImageButton = CType(e.Row.Cells(5).Controls(1), ImageButton)

                    ' Get information about the product bound to the row
                    If db.CommandName = "Delete" Then
                        Dim price As CostReduction.Cost_Reduction_StepsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, CostReduction.Cost_Reduction_StepsRow)
                        db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record?');")
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
    End Sub 'EOF gvSteps_RowDataBound

    Protected Sub txtNextImpDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNextImpDate.TextChanged

        Try
            txtTodaysDate.Text = Date.Today

            If CType(txtHDEstImpDate.Text, Date) <> CType(txtNextImpDate.Text, Date) And (CType(txtNextImpDate.Text, Date) >= CType(txtTodaysDate.Text, Date)) Then
                rfvEstImpDate.Enabled = False
                lblReqImpDateChange.Visible = True
                lblImpDateChange.Visible = True
                txtImpDateChngRsn.Visible = True
                txtImpDateChngRsn.Focus()
                'rfvUploadFile.Enabled = False
                'revUploadFile.Enabled = False
                'rfvuploadFileNew.Enabled = False
                'revuploadFileNew.Enabled = False
            Else
                rfvEstImpDate.Enabled = True
                lblReqImpDateChange.Visible = False
                lblImpDateChange.Visible = False
                txtImpDateChngRsn.Visible = False
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF txtNextImpDate_TextChanged

    Protected Sub txtNextAnnCostSave_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNextAnnCostSave.TextChanged

        Try
            If txtHDEstAnnCostSave.Text <> txtNextAnnCostSave.Text Then
                rfvEstAnnCostSave.Enabled = False
                lblReqAnnCostChngRsn.Visible = True
                lblAnnCostChngRsn.Visible = True
                txtAnnCostChngRsn.Visible = True
                txtAnnCostChngRsn.Focus()
            Else
                rfvEstAnnCostSave.Enabled = True
                lblReqAnnCostChngRsn.Visible = False
                lblAnnCostChngRsn.Visible = False
                txtAnnCostChngRsn.Visible = False
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF txtNextAnnCostSave_TextChanged

    Protected Sub txtNextCapEx_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNextCapEx.TextChanged

        Try
            If txtHDCapEx.Text <> txtNextCapEx.Text Then
                rfvCapEx.Enabled = False
                lblReqCapExChngRsn.Visible = True
                lblCapExChngRsn.Visible = True
                txtCapExChngRsn.Visible = True
                txtCapExChngRsn.Focus()
            Else
                rfvCapEx.Enabled = True
                lblReqCapExChngRsn.Visible = False
                lblCapExChngRsn.Visible = False
                txtCapExChngRsn.Visible = False
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF txtNextCapEx_TextChanged

    Protected Sub txtNextSuccessRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNextSuccessRate.TextChanged

        Try
            If txtHDSuccessRate.Text.Trim <> txtNextSuccessRate.Text.Trim Then
                rfvSuccessRate.Enabled = False
                lblReqSuccessRateChngRsn.Visible = True
                lblSuccessRateChngRsn.Visible = True
                txtSuccessRateChngRsn.Visible = True
                txtSuccessRateChngRsn.Focus()
                txtNextSuccessRate.Text = IIf(CType(txtNextSuccessRate.Text.Trim, Double) > 100, 100, txtNextSuccessRate.Text.Trim)
            Else
                rfvSuccessRate.Enabled = True
                lblReqSuccessRateChngRsn.Visible = False
                lblSuccessRateChngRsn.Visible = False
                txtSuccessRateChngRsn.Visible = False
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF txtNextSuccessRate_TextChanged


    Protected Sub btnProposedDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProposedDetails.Click

        Try
            ClearMessages()

            If ViewState("pProjNo") > 0 Then
                Response.Redirect("CostReductionProposedDetail.aspx?pProjNo=" & ViewState("pProjNo"), False)
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblErrorsButtons.Text = lblErrors.Text

    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click

        Try
            ClearMessages()

            Dim ds As DataSet

            'copy record and child tables
            ds = CRModule.CopyCostReduction(ViewState("pProjNo"))

            'get new Project No 
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("NewProjectNo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("NewProjectNo") > 0 Then
                        'redirect to new record
                        Response.Redirect("~/CR/CostReduction.aspx?pProjNo=" & ds.Tables(0).Rows(0).Item("NewProjectNo").ToString, False)
                    End If
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblErrorsButtons.Text = lblErrors.Text

    End Sub

    Protected Sub ddProjectCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProjectCategory.SelectedIndexChanged

        Try

            ClearMessages()

            Dim iProjectCategoryID As Integer = 0

            If ddProjectCategory.SelectedIndex > 0 Then
                iProjectCategoryID = ddProjectCategory.SelectedValue
            End If

            If iProjectCategoryID = 4 Or iProjectCategoryID = 6 Then
                txtCompletion.Text = 100
            End If

            If cbPlantControllerReviewed.Checked = False Then
                lblErrors.Text = "NOTE: This project cannot be closed until the Plant Controller reviews it."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblErrorsButtons.Text = lblErrors.Text

    End Sub
End Class
