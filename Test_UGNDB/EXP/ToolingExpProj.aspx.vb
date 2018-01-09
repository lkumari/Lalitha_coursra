' ************************************************************************************************
' Name:	ToolingExpProj.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 06/10/2009    LRey	Created .Net application
' 04/27/2010    LREy    Added additional validation for PartNo and EstCmpltDate to exit sub when 
'                       the values are nothing. Also added thist  ddUGNFacility.Items.RemoveAt(5) 
'                       condition to the BindCriteria so that TM's do not select UT as a facility 
'                       due to default sequencing by facility. TM's where selecting this as an option
'                       to expense tools for prototype part with unknown facility and/or unknown pno's.
' 07/30/2012    LRey    Replace LeadTime in Customer/Part tab with LeadTimeVal and LeadTimeWM - used for 
'                       automatic email notification under the condition if a T is not approved prior
'                       to an allotted Lead Time and email will go out to all pending TM's and Originator to 
'                       help push these items through quickly in the system.
' 01/07/2013    LRey    Added a control to hide the Edit button in the approval process to prevent out of sequence approval.
' 01/18/2013    LRey    Added logic to build 1st level approval list based on facility
' 05/06/2013    LRey    Modified the Append process so that the "Amount to be recovered", "Total Expense" and 
'                       "Profit/Loss" is not included in the supplement. Original values were incorrect.
' 06/26/2013    LRey    Modified the Reject process to notify the correct group based on the facility.
'                       Modified the approval routing chain for first level according to Sam Lumetta.
' ************************************************************************************************
#Region "Directives"
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Text
Imports System.Net.Mail
Imports System.Threading
Imports System.Web.Configuration
#End Region

Partial Class EXP_ToolingExpProj
    Inherits System.Web.UI.Page
    ''Initialize Variables for Recovery Type "Piece Price: gvYearlyVolume" Footer Row
    Dim _totalVolume As Integer = 0

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

            ''Used for Customer Info binddata and update
            If HttpContext.Current.Request.QueryString("pTCID") <> "" Then
                ViewState("pTCID") = HttpContext.Current.Request.QueryString("pTCID")
            Else
                ViewState("pTCID") = 0
            End If

            ''Used for Tooling Expense binddata and update
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

            ''Used to take user back to Customer Info Tab after reset/save
            If HttpContext.Current.Request.QueryString("pCV") <> "" Then
                ViewState("pCV") = HttpContext.Current.Request.QueryString("pCV")
            Else
                ViewState("pCV") = 0
            End If

            ''Used to take user back to Tooling Expense Tab after reset/save
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

            ''Used to Show/Hide Tabs when Last Primary/Supplement entry is made 
            ''from TE Tracking system
            If HttpContext.Current.Request.QueryString("pLS") <> "" Then
                ViewState("pLS") = CType(HttpContext.Current.Request.QueryString("pLS"), Boolean)
            Else
                ViewState("pLS") = 0
            End If

            ''Used to add supplement for a record that has been carried over
            ''from old TE Tracking system
            If HttpContext.Current.Request.QueryString("pCO") <> "" Then
                ViewState("pCO") = CType(HttpContext.Current.Request.QueryString("pCO"), Boolean)
            Else
                ViewState("pCO") = 0
            End If

            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            If ViewState("pProjNo") = Nothing Then
                m.ContentLabel = "New Customer Owned Tooling Project"
            Else
                m.ContentLabel = "Customer Owned Tooling Project"
            End If

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pProjNo") = Nothing Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='ToolingExpProjList.aspx'><b>Customer Owned Tooling Search</b></a> > New Customer Owned Tooling Project"
                Else
                    If ViewState("pAprv") = 0 Then
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='ToolingExpProjList.aspx'><b>Customer Owned Tooling Search</b></a> > Customer Owned Tooling Project"
                    Else 'Go Back To approval Screen
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='ToolingExpProjList.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1'><b>Customer Owned Tooling Search</b></a> > <a href='crExpProjToolingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1'><b>Approval</b></a> > Customer Owned Tooling Project"
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
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"


            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                BindCriteria()

                ''****************************************
                ''Redirect user to the right tab location
                ''****************************************
                If ViewState("pProjNo") <> "" Then
                    BindData(ViewState("pProjNo"), ViewState("pCO"))
                Else
                    BindData(ViewState("pPrntProjNo"), ViewState("pCO"))
                    txtProjectTitle.Focus()
                    txtDateSubmitted.Text = Date.Today
                End If

                If ViewState("pTCID") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True
                ElseIf ViewState("pCV") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True
                ElseIf ViewState("pEID") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(3)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(3).Selected = True
                ElseIf ViewState("pEV") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(3)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(3).Selected = True
                ElseIf ViewState("pSD") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(5)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(5).Selected = True
                ElseIf ViewState("pRID") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(6)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(6).Selected = True
                ElseIf ViewState("pRC") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(6)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(6).Selected = True
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

            Dim strPreviewClientScript As String = "javascript:void(window.open('crViewExpProjTooling.aspx?pProjNo=" & ViewState("pProjNo") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
            btnPreview.Attributes.Add("onclick", strPreviewClientScript)

            Dim strPreviewClientScript2 As String = "javascript:void(window.open('../DataMaintenance/ProgramDisplay.aspx?pPlatID=0&pPgmID=" & ddProgram.SelectedValue & " '," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=yes,location=yes'));"
            iBtnPreviewDetail.Attributes.Add("Onclick", strPreviewClientScript2)

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message & " 1 "
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
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            ViewState("strProdOrTestEnvironment") = strProdOrTestEnvironment
            ViewState("Admin") = False
            ViewState("ObjectRole") = False

            ''*******
            '' Disable controls by default
            ''*******
            btnAdd.Enabled = False
            btnSave1.Enabled = False
            btnSave2.Enabled = True
            btnSave3.Enabled = False
            btnReset1.Enabled = True
            btnReset2.Enabled = False
            btnReset3.Enabled = False
            btnReset4.Enabled = False
            btnReset5.Enabled = True
            btnReset6.Enabled = False
            btnUpload.Enabled = False
            uploadFile.Enabled = False
            btnDelete.Enabled = False
            btnPreview.Enabled = False
            btnAppend.Enabled = False
            btnBuildApproval.Enabled = False
            btnBuildApproval.Visible = False
            btnFwdToProjLead.Enabled = False
            btnFwdToProjLead.Visible = False
            btnFwdToolEngrMgr.Enabled = False
            btnFwdApproval.Enabled = False
            btnFwdToolEngrMgr.Visible = False
            ddProjectStatus.Enabled = False
            btnAddtoGrid1.Enabled = False
            btnAddtoGrid2.Enabled = False
            mnuTabs.Items(1).Enabled = False
            mnuTabs.Items(2).Enabled = False
            mnuTabs.Items(3).Enabled = False
            mnuTabs.Items(4).Enabled = False
            mnuTabs.Items(5).Enabled = False
            mnuTabs.Items(6).Enabled = False

            gvCustomer.Columns(9).Visible = False
            gvExpense.Columns(6).Visible = False
            gvYearlyVolume.Columns(2).Visible = False
            gvSupportingDocument.Columns(4).Visible = False
            gvApprovers.Columns(8).Visible = False
            gvQuestion.Columns(0).Visible = True
            ckRecoveryType1.Enabled = False
            ckRecoveryType2.Enabled = False
            txtActualCost.Visible = False
            txtCustomerCost.Visible = False
            txtClosingNotes.Visible = False
            txtVoidReason.Visible = False
            txtActualCost.Enabled = False
            txtCustomerCost.Enabled = False
            txtClosingNotes.Enabled = False
            txtVoidReason.Enabled = False
            lblActualCost.Visible = False
            lblCustomerCost.Visible = False
            lblClosingNts.Visible = False
            rfvClosingNotes.Enabled = False
            lblVoidRsn.Visible = False
            lblReqCustomerCost.Visible = False
            lblReqClosingNts.Visible = False
            lblReqActualCost.Visible = False
            lblReqVoidRsn.Visible = False
            lblReqRecType.Visible = False

            txtAmtToBeRecovered.Enabled = True
            txtHDAmtToBeRecovered.Enabled = True
            txtNextAmtToBeRecovered.Enabled = False

            txtEstCmpltDt.Enabled = True
            txtHDEstCmpltDt.Enabled = True
            txtNextEstCmpltDt.Enabled = False

            rfvAmtToBeRecovered.Enabled = False
            rfvNextAmtToBeRecovered.Enabled = False
            rfvEstCmpltDt.Enabled = False
            rfvNextEstCmpltDt.Enabled = False
            rfvAmtToBeRecoveredChngRsn.Enabled = False
            rfvEstCmpltDt.Enabled = False
            rfvNextEstCmpltDt.Enabled = False
            rfvEstCmpltDtChngRsn.Enabled = False

            TCExtender.Collapsed = False
            TEExtender.Collapsed = False

            lblReqReSubmit.Visible = False
            lblReSubmit.Visible = False
            txtReSubmit.Visible = False
            rfvReSubmit.Enabled = False
            vsReSubmit.Enabled = False


            ''** Project Status
            Dim ProjectStatus As String = Nothing
            Select Case txtRoutingStatus.Text
                Case "N"
                    ProjectStatus = ddProjectStatus.SelectedValue
                    ddProjectStatus.Visible = True
                    ddProjectStatus2.Visible = False
                Case "A"
                    ProjectStatus = ddProjectStatus.SelectedValue
                    ddProjectStatus.Visible = True
                    ddProjectStatus2.Visible = False
                Case "C"
                    ProjectStatus = ddProjectStatus.SelectedValue
                    ddProjectStatus.Visible = True
                    ddProjectStatus2.Visible = False
                Case "T"
                    ProjectStatus = ddProjectStatus2.SelectedValue
                    ddProjectStatus.Visible = False
                    ddProjectStatus2.Visible = True
                Case "S"
                    ProjectStatus = ddProjectStatus2.SelectedValue
                    ddProjectStatus.Visible = False
                    ddProjectStatus2.Visible = True
                Case "R"
                    ProjectStatus = ddProjectStatus2.SelectedValue
                    ddProjectStatus.Visible = False
                    ddProjectStatus2.Visible = True
                    lblReqReSubmit.Visible = True
                    lblReSubmit.Visible = True
                    txtReSubmit.Visible = True
                    rfvReSubmit.Enabled = True
                    vsReSubmit.Enabled = True
                Case "V"
                    ProjectStatus = ddProjectStatus.SelectedValue
                    ddProjectStatus.Visible = True
                    ddProjectStatus2.Visible = False
                Case Else
                    ddProjectStatus.Visible = True
                    ddProjectStatus2.Visible = False
            End Select
            ViewState("ProjectStatus") = ProjectStatus

            If ProjectStatus = "Closed" Then
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
                rfvClosingNotes.Enabled = True
            End If

            If ProjectStatus = "Void" Then
                lblReqVoidRsn.Visible = True
                lblVoidRsn.Visible = True
                txtVoidReason.Visible = True
                txtVoidReason.Enabled = True
            End If

            ''* Amt To Be Recovered
            If txtAmtToBeRecovered.Text = "" And ViewState("pProjNo") = Nothing Then
                rfvAmtToBeRecovered.Enabled = True
            Else
                If ProjectStatus <> "Open" Then
                    rfvNextAmtToBeRecovered.Enabled = True
                End If
            End If

            If txtHDAmtToBeRecovered.Text.Trim <> "" And txtAmtToBeRecovered.Text.Trim <> "" Then
                If txtHDAmtToBeRecovered.Text <> txtNextAmtToBeRecovered.Text And ProjectStatus <> "Open" Then
                    rfvAmtToBeRecoveredChngRsn.Enabled = True
                End If
            End If

            ''* Estimated Completion Date
            If txtEstCmpltDt.Text = "" And ViewState("pProjNo") = Nothing Then
                rfvEstCmpltDt.Enabled = True
            Else
                If ProjectStatus <> "Open" Then
                    rfvNextEstCmpltDt.Enabled = True
                End If
            End If

            If txtHDEstCmpltDt.Text.Trim <> "" And txtNextEstCmpltDt.Text.Trim <> "" Then
                If txtHDEstCmpltDt.Text <> txtNextEstCmpltDt.Text And ProjectStatus <> "Open" Then
                    rfvEstCmpltDtChngRsn.Enabled = True
                End If
            End If

            ''******************************************************
            ''Verify required fields in a carry over to enable tabs
            ''******************************************************
            Dim ReqFieldsMissing As Boolean = False
            If txtProjDateNotes.Text = Nothing Then
                ReqFieldsMissing = True
            End If
            If ddPurchasingLead.SelectedValue = Nothing Then
                ReqFieldsMissing = True
            End If
            If ddToolingLead.SelectedValue = Nothing Then
                ReqFieldsMissing = True
            End If
            If ddProgramManager.SelectedValue = Nothing Then
                ReqFieldsMissing = True
            End If
            If ddAccountManager.SelectedValue = Nothing Then
                ReqFieldsMissing = True
            End If
            If txtAmtToBeRecovered.Text = Nothing Then
                ReqFieldsMissing = True
            End If
            'If txtEstCmpltDt.Text = Nothing Then
            '    ReqFieldsMissing = True
            'End If
            If txtExpToolRtnDt.Text = Nothing Then
                ReqFieldsMissing = True
            End If
            If txtEstSpendDt.Text = Nothing Then
                ReqFieldsMissing = True
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 82 'Customer Owned Tooling Form ID
            Dim iRoleID As Integer = 0
            Dim i As Integer = 0


            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Jerry.Good", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    ViewState("iTeamMemberID") = iTeamMemberID
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
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

                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pProjNo") = Nothing Or ViewState("pProjNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                txtProjectTitle.Focus()
                                                btnSave1.Enabled = True
                                                btnReset1.Enabled = True
                                            Else
                                                Select Case ProjectStatus
                                                    Case "Open"
                                                        If (txtRoutingStatus.Text = "N") And ReqFieldsMissing = False Then
                                                            btnFwdToProjLead.Enabled = True
                                                            btnFwdToProjLead.Visible = True
                                                            btnBuildApproval.Enabled = False
                                                            btnBuildApproval.Visible = False
                                                        End If
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True

                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "R" Or txtRoutingStatus.Text = "S") Then
                                                            If (txtRoutingStatus.Text = "S") And _
                                                                ReqFieldsMissing = False And _
                                                                txtHDToolEngrMgrNotified.Text = "" And _
                                                                ddProjectType.SelectedValue = "Internal" Then
                                                                btnFwdToolEngrMgr.Enabled = True
                                                                btnFwdToolEngrMgr.Visible = True
                                                            Else
                                                                btnFwdApproval.Enabled = True
                                                                btnFwdToProjLead.Visible = True
                                                            End If

                                                        End If
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        gvApprovers.Columns(8).Visible = True
                                                        TCExtender.Collapsed = False
                                                        TEExtender.Collapsed = False
                                                    Case "Approved"
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        TCExtender.Collapsed = True
                                                        TEExtender.Collapsed = True
                                                    Case "Closed"
                                                        btnSave1.Enabled = True
                                                        txtActualCost.Enabled = True
                                                        txtCustomerCost.Enabled = True
                                                        txtClosingNotes.Enabled = True
                                                        TCExtender.Collapsed = True
                                                        TEExtender.Collapsed = True
                                                    Case "Tooling Completed"
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                    Case "Void"
                                                        btnSave1.Enabled = True
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidRsn.Visible = True
                                                        lblReqVoidRsn.Visible = True
                                                        TCExtender.Collapsed = True
                                                        TEExtender.Collapsed = True
                                                End Select

                                                If (txtRoutingStatus.Text <> "N") And (txtRoutingStatus.Text <> "R") And (txtRoutingStatus.Text <> "T") Then
                                                    btnAppend.Enabled = True
                                                End If

                                                btnSave2.Enabled = True
                                                btnSave3.Enabled = True
                                                btnReset2.Enabled = True
                                                btnReset3.Enabled = True
                                                btnReset4.Enabled = True
                                                btnReset5.Enabled = True
                                                btnReset6.Enabled = True
                                                btnAddtoGrid1.Enabled = True
                                                btnAddtoGrid2.Enabled = True
                                                ddProjectStatus.Enabled = True
                                                btnDelete.Enabled = True
                                                btnUpload.Enabled = True
                                                uploadFile.Enabled = True
                                                btnPreview.Enabled = True
                                                gvCustomer.Columns(9).Visible = True
                                                gvYearlyVolume.Columns(2).Visible = True
                                                gvExpense.Columns(6).Visible = True
                                                gvSupportingDocument.Columns(4).Visible = True
                                                ckRecoveryType1.Enabled = True
                                                ckRecoveryType2.Enabled = True
                                                'If ReqFieldsMissing = False Then
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                mnuTabs.Items(4).Enabled = True
                                                mnuTabs.Items(5).Enabled = True
                                                mnuTabs.Items(6).Enabled = True
                                                'End If
                                                txtNextEstCmpltDt.Enabled = True
                                                txtNextAmtToBeRecovered.Enabled = True
                                                If (txtRoutingStatus.Text <> "C") And (ProjectStatus = "Closed") Then
                                                    txtActualCost.Enabled = True
                                                    txtCustomerCost.Enabled = True
                                                    txtClosingNotes.Enabled = True
                                                End If
                                            End If
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Sales/Program Mgrs, Tooling/Purchasing Leads
                                            ViewState("ObjectRole") = True
                                            btnAdd.Enabled = True
                                            btnReset6.Enabled = True
                                            btnUpload.Enabled = True
                                            uploadFile.Enabled = True
                                            gvSupportingDocument.Columns(4).Visible = True

                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pProjNo") = Nothing Or ViewState("pProjNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                txtProjectTitle.Focus()
                                                btnSave1.Enabled = True
                                                btnReset1.Enabled = True
                                            Else
                                                Select Case ProjectStatus
                                                    Case "Open"
                                                        ViewState("Admin") = True
                                                        If (txtRoutingStatus.Text = "N") And ReqFieldsMissing = False Then
                                                            btnFwdToProjLead.Enabled = True
                                                            btnFwdToProjLead.Visible = True
                                                            btnDelete.Enabled = True
                                                        End If
                                                        ddProjectStatus.Enabled = False
                                                        txtNextEstCmpltDt.Enabled = True
                                                        txtNextAmtToBeRecovered.Enabled = True
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        btnReset2.Enabled = True
                                                        btnReset4.Enabled = True
                                                        ckRecoveryType1.Enabled = True
                                                        ckRecoveryType2.Enabled = True
                                                        btnSave3.Enabled = True
                                                        btnReset3.Enabled = True
                                                        btnAddtoGrid1.Enabled = True
                                                        btnAddtoGrid2.Enabled = True
                                                        gvExpense.Columns(6).Visible = True
                                                        gvCustomer.Columns(9).Visible = True
                                                        gvYearlyVolume.Columns(2).Visible = True
                                                        TCExtender.Collapsed = False
                                                        TEExtender.Collapsed = False
                                                    Case "In Process"
                                                        ViewState("Admin") = True
                                                        txtNextEstCmpltDt.Enabled = True
                                                        txtNextAmtToBeRecovered.Enabled = True
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        ddProjectStatus2.Enabled = True
                                                        TCExtender.Collapsed = False
                                                        TEExtender.Collapsed = False
                                                        If (txtRoutingStatus.Text = "R") Or (txtRoutingStatus.Text = "S") Then
                                                            ''Only allow Project Leads to Tooling Engr Mgr
                                                            If ((iTeamMemberID = ddToolingLead.SelectedValue And _
                                                                 txtHDToolEngrMgrNotified.Text = "") Or _
                                                                iTeamMemberID = 204) Then
                                                                btnFwdToolEngrMgr.Enabled = True
                                                                btnFwdToolEngrMgr.Visible = True
                                                                gvExpense.Columns(6).Visible = True
                                                                btnAddtoGrid2.Enabled = True
                                                                btnReset4.Enabled = True
                                                            Else
                                                                ''Only allow Tooling Engr Mgr to Frwd for Approval
                                                                If (iTeamMemberID = ddToolingLead.SelectedValue And txtHDToolEngrMgrNotified.Text = "") Or _
                                                                 (iTeamMemberID = ddToolEngrMgr.SelectedValue And ddProjectType.SelectedValue = "Internal" And txtHDToolEngrMgrNotified.Text <> "") Or _
                                                                     (iTeamMemberID = ddAccountManager.SelectedValue And txtRoutingStatus.Text = "R") Or _
                                                                     (iTeamMemberID = ddProgramManager.SelectedValue And txtRoutingStatus.Text = "R") Or _
                                                                     (iTeamMemberID = ddPurchasingLead.SelectedValue And ddProjectType.SelectedValue = "External") Or _
                                                                     (iTeamMemberID = 204) Then
                                                                    gvExpense.Columns(6).Visible = True
                                                                    btnAddtoGrid2.Enabled = True
                                                                    btnReset4.Enabled = True
                                                                    btnFwdApproval.Enabled = True
                                                                End If
                                                            End If
                                                            btnAddtoGrid1.Enabled = True
                                                            btnReset2.Enabled = True
                                                            gvCustomer.Columns(9).Visible = True
                                                            gvYearlyVolume.Columns(2).Visible = True
                                                        End If
                                                        btnSave3.Enabled = True
                                                        btnReset3.Enabled = True
                                                        ckRecoveryType1.Enabled = True
                                                        ckRecoveryType2.Enabled = True
                                                    Case "Approved"
                                                        ddProjectStatus.Enabled = True
                                                        btnAdd.Enabled = True
                                                        txtNextEstCmpltDt.Enabled = True
                                                        txtNextAmtToBeRecovered.Enabled = True
                                                        ckRecoveryType1.Enabled = False
                                                        ckRecoveryType2.Enabled = False
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        TCExtender.Collapsed = True
                                                        TEExtender.Collapsed = True
                                                    Case "Closed"
                                                        btnAdd.Enabled = True
                                                        ckRecoveryType1.Enabled = False
                                                        ckRecoveryType2.Enabled = False
                                                        ddProjectStatus.Enabled = False
                                                        If (txtRoutingStatus.Text <> "C") Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            txtActualCost.Enabled = True
                                                            txtCustomerCost.Enabled = True
                                                            txtClosingNotes.Enabled = True
                                                            rfvClosingNotes.Enabled = True
                                                        End If
                                                        TCExtender.Collapsed = True
                                                        TEExtender.Collapsed = True
                                                    Case "Tooling Completed"
                                                        btnAdd.Enabled = True
                                                        ckRecoveryType1.Enabled = False
                                                        ckRecoveryType2.Enabled = False
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        TCExtender.Collapsed = True
                                                        TEExtender.Collapsed = True
                                                        ddProjectStatus.Enabled = True
                                                    Case "Void"
                                                        btnAdd.Enabled = True
                                                        ckRecoveryType1.Enabled = False
                                                        ckRecoveryType2.Enabled = False
                                                        txtVoidReason.Visible = True
                                                        If (txtRoutingStatus.Text <> "V") Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            txtVoidReason.Enabled = True
                                                            lblVoidRsn.Visible = True
                                                            lblReqVoidRsn.Visible = True
                                                        End If
                                                        TCExtender.Collapsed = True
                                                        TEExtender.Collapsed = True
                                                End Select
                                                If (txtRoutingStatus.Text <> "N") And (txtRoutingStatus.Text <> "R") And (txtRoutingStatus.Text <> "T") Then
                                                    btnAppend.Enabled = True
                                                End If
                                                btnSave2.Enabled = True
                                                btnReset5.Enabled = True
                                                btnPreview.Enabled = True
                                                'If ReqFieldsMissing = False Then
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                mnuTabs.Items(4).Enabled = True
                                                mnuTabs.Items(5).Enabled = True
                                                mnuTabs.Items(6).Enabled = True
                                                'End If
                                            End If
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            '** Used by Approvers & Backup persons
                                            If ViewState("pProjNo") = Nothing Or ViewState("pProjNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                txtProjectTitle.Focus()
                                            Else
                                                ViewState("ObjectRole") = False
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                mnuTabs.Items(4).Enabled = True
                                                mnuTabs.Items(5).Enabled = True
                                                mnuTabs.Items(6).Enabled = True
                                                btnPreview.Enabled = True
                                                TCExtender.Collapsed = True
                                                TEExtender.Collapsed = True
                                                Select Case ProjectStatus
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "T") Then
                                                            gvApprovers.Columns(8).Visible = True
                                                            btnReset6.Enabled = False
                                                            btnUpload.Enabled = False
                                                            uploadFile.Enabled = False
                                                            gvSupportingDocument.Columns(4).Visible = False
                                                            btnSave2.Enabled = True
                                                            btnReset5.Enabled = True
                                                        End If
                                                End Select
                                            End If
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            ViewState("ObjectRole") = False
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                            mnuTabs.Items(3).Enabled = True
                                            mnuTabs.Items(4).Enabled = True
                                            mnuTabs.Items(5).Enabled = True
                                            mnuTabs.Items(6).Enabled = True
                                            ckRecoveryType1.Enabled = False
                                            ckRecoveryType2.Enabled = False
                                            btnPreview.Enabled = True
                                            TCExtender.Collapsed = True
                                            TEExtender.Collapsed = True
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ''Used by the Accounting Group
                                            ViewState("ObjectRole") = False
                                            TCExtender.Collapsed = True
                                            TEExtender.Collapsed = True
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
                                                    Case "Void"
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidRsn.Visible = True
                                                        lblReqVoidRsn.Visible = True
                                                        If (txtRoutingStatus.Text <> "V") Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                        End If
                                                    Case "Approved"
                                                        ddProjectStatus.Enabled = True
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                    Case "Closed"
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        txtActualCost.Visible = True
                                                        txtCustomerCost.Visible = True
                                                        txtClosingNotes.Visible = True
                                                        lblActualCost.Visible = True
                                                        lblCustomerCost.Visible = True
                                                        lblClosingNts.Visible = True
                                                        ddProjectStatus.Enabled = True
                                                        If (txtRoutingStatus.Text <> "C") Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            txtActualCost.Enabled = True
                                                            txtCustomerCost.Enabled = True
                                                            txtClosingNotes.Enabled = True
                                                            lblReqCustomerCost.Visible = True
                                                            lblReqClosingNts.Visible = True
                                                            lblReqActualCost.Visible = True
                                                            rfvClosingNotes.Enabled = True
                                                        End If
                                                    Case "Tooling Completed"
                                                        ddProjectStatus.Enabled = True
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                End Select
                                                btnSave2.Enabled = True
                                                btnReset5.Enabled = True
                                                gvSupportingDocument.Columns(4).Visible = True
                                                uploadFile.Enabled = True
                                                btnReset6.Enabled = True
                                                btnUpload.Enabled = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                mnuTabs.Items(4).Enabled = True
                                                mnuTabs.Items(5).Enabled = True
                                                mnuTabs.Items(6).Enabled = True
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
            lblErrors.Text = ex.Message & " 2 "
            lblErrors.Visible = True
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

            ''bind existing data to drop down Account Manager control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(9) '**SubscriptionID 9 is used for Account Manager
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddAccountManager.DataSource = ds
                ddAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddAccountManager.DataBind()
                ddAccountManager.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Program Manager control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(31) '**SubscriptionID 31 is used for Program Manager
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddProgramManager.DataSource = ds
                ddProgramManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddProgramManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddProgramManager.DataBind()
                ddProgramManager.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Tooling Lead control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(65) '**SubscriptionID 9 is used for Tooling Lead
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddToolingLead.DataSource = ds
                ddToolingLead.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddToolingLead.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddToolingLead.DataBind()
                ddToolingLead.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetTeamMemberBySubscription(145) '**SubscriptionID 9 is used for Tooling Lead
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddToolEngrMgr.DataSource = ds
                ddToolEngrMgr.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddToolEngrMgr.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddToolEngrMgr.DataBind()
                ddToolEngrMgr.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Purchasing Lead control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(7) '**SubscriptionID 7 is used for Purchasing Lead
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddPurchasingLead.DataSource = ds
                ddPurchasingLead.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddPurchasingLead.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddPurchasingLead.DataBind()
                ddPurchasingLead.Items.Insert(0, "")
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

            If ViewState("pProjNo") = Nothing Then
                ddAccountManager.SelectedValue = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                ddProgramManager.SelectedValue = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                ddToolingLead.SelectedValue = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                ddPurchasingLead.SelectedValue = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            End If

            ddTeamMember.SelectedValue = ViewState("iTeamMemberID")

            ''bind existing data to drop down UGN Location control for selection criteria for search
            ds = commonFunctions.GetUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
                'ddUGNFacility.Items.RemoveAt(5)
            End If

            ''bind existing data to drop down Customer control for selection criteria for search
            ' ''ds = commonFunctions.GetOEMManufacturer("")
            ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
            ' ''    ddCustomer.DataSource = ds
            ' ''    ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
            ' ''    ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
            ' ''    ddCustomer.DataBind()
            ' ''    ddCustomer.Items.Insert(0, "")
            ' ''End If


            ''bind existing data to drop down BPCS Part No control for selection criteria for search
            ''ds = commonFunctions.GetBPCSPartNo("", "")
            ' ''ds = commonFunctions.GetPartNo("", "", "", "", "")
            ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
            ' ''    ddPartNo.DataSource = ds
            ' ''    ddPartNo.DataTextField = ds.Tables(0).Columns("ddPartNo").ColumnName.ToString()
            ' ''    ddPartNo.DataValueField = ds.Tables(0).Columns("PartNo").ColumnName.ToString()
            ' ''    ddPartNo.DataBind()
            ' ''    ddPartNo.Items.Insert(0, "")
            ' ''    ddPartNo.SelectedIndex = 0
            ' ''    'ddPartNo.Items.Insert(1, "N/A - Future Part")
            ' ''End If
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

    Public Sub BindData(ByVal ProjNo As String, ByVal CarriedOver As Boolean)
        Dim ds As DataSet = New DataSet

        Try
            If CarriedOver = True Then
                lblErrors.Text = ""
                Dim ParentProjectNo As String = Nothing
                Dim ProjectNo As String = ProjNo
                Dim ExpProject As String = "Tooling"
                Dim UGNFacility As String = Nothing
                Dim ProjectTitle As String = Nothing
                Dim ProjectType As String = Nothing
                Dim OriginalCEAApprovedDt As String = Nothing
                Dim AccountMgrTMID As Integer = Nothing
                Dim PrgmMgrTMID As Integer = Nothing
                Dim ToolLeadTMID As Integer = Nothing
                Dim PurchLeadTMID As Integer = Nothing
                Dim RoutingFlag As String = Nothing
                Dim DefaultDate As Date = Date.Now

                ''***********************************************************************
                ''Validate Primary/Supplement TE exists in UGN_Database CapitalExpenditure table
                ''***********************************************************************
                ds = EXPModule.GetExpProjToolingLastSupplementNo(ProjectNo)

                If (ds.Tables.Item(0).Rows.Count > 0) Then
                    ParentProjectNo = IIf(ds.Tables(0).Rows(0).Item("ParentProjectNumber").ToString() = Nothing, ProjectNo, ds.Tables(0).Rows(0).Item("ParentProjectNumber").ToString())
                    UGNFacility = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                    ProjectTitle = ds.Tables(0).Rows(0).Item("ProjectTitle").ToString()
                    ProjectType = ds.Tables(0).Rows(0).Item("ProjectType").ToString()
                    OriginalCEAApprovedDt = ds.Tables(0).Rows(0).Item("OriginalCEAApprovedDt").ToString()
                    AccountMgrTMID = ds.Tables(0).Rows(0).Item("AccountMgrTMID").ToString()
                    PrgmMgrTMID = ds.Tables(0).Rows(0).Item("PrgmMgrTMID").ToString()
                    ToolLeadTMID = ds.Tables(0).Rows(0).Item("ToolLeadTMID").ToString()
                    PurchLeadTMID = ds.Tables(0).Rows(0).Item("PurchLeadTMID").ToString()
                    RoutingFlag = ds.Tables(0).Rows(0).Item("RoutingFlag").ToString()

                    If RoutingFlag = "A" Or RoutingFlag = "C" Then
                        'allow carryover of records that have been only approved
                        ''***************
                        ''Get next SeqNo
                        ''***************
                        Dim ds2 As DataSet = Nothing
                        ds2 = EXPModule.GetUGNDatabaseNextProjNo(ParentProjectNo.ToUpper, ProjectNo.ToUpper, ExpProject, UGNFacility)
                        ViewState("pProjNo") = CType(ds2.Tables(0).Rows(0).Item("NextAvailProjNo").ToString, String)

                        '***************
                        '* Save Data
                        '***************
                        'Dim DefaultTMID As Integer = IIf(HttpContext.Current.Request.Cookies("UGNDB_TMID").Value = Nothing, ViewState("iTeamMemberID"), HttpContext.Current.Request.Cookies("UGNDB_TMID").Value)

                        ''Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

                        EXPModule.InsertExpProjTooling(ViewState("pProjNo"), ParentProjectNo.ToUpper, ProjectTitle, "Open", ProjectType, UGNFacility, AccountMgrTMID, PrgmMgrTMID, ToolLeadTMID, PurchLeadTMID, "", DefaultDate, "", "", "", "", 0, OriginalCEAApprovedDt, 0, "auto", DefaultDate, False)

                        ''*****************
                        ''History Tracking
                        ''*****************

                        EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), ProjectTitle, ViewState("iTeamMemberID"), "Record created.")

                        ''****************************************
                        ''Redirect to new Project Number
                        ''****************************************
                        Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pPrntProjNo=" & ParentProjectNo & "&pLS=1", False)
                    Else
                        lblErrors.Text = "Unable to process request. " & ProjectNo & " is or has a series of records pending in TE Tracking system. Please review."
                        lblErrors.Visible = True
                    End If
                Else
                    lblErrors.Text = "Unable to process request. " & ProjectNo & " not in TE Tracking system. Please Try Again."
                    lblErrors.Visible = True
                End If
            Else
                If ProjNo <> Nothing Then
                    ds = EXPModule.GetExpProjTooling(ProjNo, "", "", "", "", 0, 0, 0, 0, 0, "", "", "", "")
                    If (ds.Tables.Item(0).Rows.Count > 0) Then
                        Select Case ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                            Case "N"
                                ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                            Case "A"
                                ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                            Case "C"
                                ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
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
                        ddProjectType.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectType").ToString()
                        ddUGNFacility.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                        ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AccountMgrTMID").ToString()
                        ddProgramManager.SelectedValue = ds.Tables(0).Rows(0).Item("PrgmMgrTMID").ToString()
                        ddToolingLead.SelectedValue = ds.Tables(0).Rows(0).Item("ToolLeadTMID").ToString()
                        ddPurchasingLead.SelectedValue = ds.Tables(0).Rows(0).Item("PurchLeadTMID").ToString()
                        ddToolEngrMgr.SelectedValue = ds.Tables(0).Rows(0).Item("ToolEngrMgrTMID").ToString()
                        txtHDToolEngrMgrNotified.Text = ds.Tables(0).Rows(0).Item("ToolEngrMgrNotified").ToString()

                        If ViewState("pProjNo") <> Nothing Then 'ViewState("pPrntProjNo") = Nothing And 
                            lblProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                            txtProjDateNotes.Text = ds.Tables(0).Rows(0).Item("ProjDtNotes").ToString()
                            txtDateSubmitted.Text = ds.Tables(0).Rows(0).Item("DateSubmitted").ToString()
                            txtEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDt").ToString()
                            txtHDEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDt").ToString()
                            txtNextEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDt").ToString()
                            txtExpToolRtnDt.Text = ds.Tables(0).Rows(0).Item("ExpectedToolRtnDt").ToString()
                            txtEstSpendDt.Text = ds.Tables(0).Rows(0).Item("EstSpendDt").ToString()
                            txtEstRecoveryDt.Text = ds.Tables(0).Rows(0).Item("EstRecoveryDt").ToString()

                            txtAmtToBeRecovered.Text = Format(ds.Tables(0).Rows(0).Item("AmtToBeRecovered"), "#,##0.00")
                            txtHDAmtToBeRecovered.Text = Format(ds.Tables(0).Rows(0).Item("AmtToBeRecovered"), "#,##0.00")
                            txtNextAmtToBeRecovered.Text = Format(ds.Tables(0).Rows(0).Item("AmtToBeRecovered"), "#,##0.00")
                            lblAmtRecvrd.Text = Format(ds.Tables(0).Rows(0).Item("AmtToBeRecovered"), "#,##0.00")
                            txtMPAAmtToBeRecovered.Text = Format(ds.Tables(0).Rows(0).Item("MPA_AmtToBeRecovered"), "#,##0.00")

                            txtActualCost.Text = ds.Tables(0).Rows(0).Item("ActualCost").ToString()
                            txtCustomerCost.Text = ds.Tables(0).Rows(0).Item("CustomerCost").ToString()
                            txtClosingNotes.Text = ds.Tables(0).Rows(0).Item("ClosingNotes").ToString()
                            txtVoidReason.Text = ds.Tables(0).Rows(0).Item("VoidReason").ToString()
                            txtHDTotalInvestment.Text = Format(ds.Tables(0).Rows(0).Item("OrigTotalInv"), "#,##0.00")
                            lblTotalInvestment.Text = Format(ds.Tables(0).Rows(0).Item("TotalInv"), "#,##0.00")
                            lblTotalInvestment2.Text = Format(ds.Tables(0).Rows(0).Item("TotalInv"), "#,##0.00")
                            lblMPATotalInvestment.Text = Format(ds.Tables(0).Rows(0).Item("MPA_TotalInv"), "#,##0.00")

                            lblProfitLoss.Text = Format(ds.Tables(0).Rows(0).Item("ProfitLoss"), "#,##0.00")
                            lblProfitLoss2.Text = Format(ds.Tables(0).Rows(0).Item("ProfitLoss"), "#,##0.00")
                            lblMPAProfitLoss.Text = Format(ds.Tables(0).Rows(0).Item("MPA_ProfitLoss"), "#,##0.00")

                            If lblTotalInvestment.Text > 0 Then
                                If lblProfitLoss.Text > 0 Then
                                    lblROI.Text = Format(((lblProfitLoss.Text / lblTotalInvestment.Text) * 100), "##0.0")
                                    lblROI2.Text = Format(((lblProfitLoss.Text / lblTotalInvestment.Text) * 100), "##0.0")
                                End If
                            End If

                            If lblMPATotalInvestment.Text > 0 Then
                                If lblMPAProfitLoss.Text > 0 Then
                                    lblMPAROI.Text = Format(((lblMPAProfitLoss.Text / lblMPATotalInvestment.Text) * 100), "##0.0")
                                End If
                            End If

                            ckRecoveryType1.Checked = ds.Tables(0).Rows(0).Item("LumpSum").ToString
                            ckRecoveryType2.Checked = ds.Tables(0).Rows(0).Item("PiecePrice").ToString
                            txt1stRecoveryAmt.Text = Format(ds.Tables(0).Rows(0).Item("FirstRecoveryAmount"), "#,##0.00")
                            txt1stRecoveryDate.Text = ds.Tables(0).Rows(0).Item("FirstRecoveryDate").ToString
                            txt2ndRecoveryAmt.Text = Format(ds.Tables(0).Rows(0).Item("SecondRecoveryAmount"), "#,##0.00")
                            txt2ndRecoveryDate.Text = ds.Tables(0).Rows(0).Item("SecondRecoveryDate").ToString
                            If ViewState("toolLumpSum") = False And ViewState("toolPiecePrice") = False And ViewState("toolMonthly") = False Then
                                If ckRecoveryType1.Checked = True Then
                                    ViewState("toolLumpSum") = True
                                    ViewState("toolPiecePrice") = False
                                    ViewState("toolMonthly") = False
                                End If
                                If ckRecoveryType2.Checked = True Then
                                    ViewState("toolLumpSum") = False
                                    ViewState("toolPiecePrice") = True
                                    ViewState("toolMonthly") = False
                                End If
                            End If

                            If ViewState("pTCID") <> 0 Then
                                ds = EXPModule.GetExpProjToolingCustomer(ViewState("pProjNo"), ViewState("pTCID"))
                                If (ds.Tables.Item(0).Rows.Count > 0) Then
                                    'ddCustomer.SelectedValue = ds.Tables(0).Rows(0).Item("ddCustomerValue").ToString()
                                    cddMakes.SelectedValue = ds.Tables(0).Rows(0).Item("Make").ToString()
                                    cddModel.SelectedValue = ds.Tables(0).Rows(0).Item("Model").ToString()
                                    cddProgram.SelectedValue = ds.Tables(0).Rows(0).Item("ProgramID").ToString()
                                    'cddDesignationType.SelectedValue = ds.Tables(0).Rows(0).Item("DesignationType").ToString()
                                    'ddPartNo.SelectedValue = ds.Tables(0).Rows(0).Item("PartNo").ToString()
                                    txtPartNo.Text = ds.Tables(0).Rows(0).Item("Partno").ToString()
                                    txtRevisionLvl.Text = ds.Tables(0).Rows(0).Item("RevisionLevel").ToString()
                                    txtLeadTime.Text = ds.Tables(0).Rows(0).Item("LeadTimeVal").ToString()
                                    ddLeadTime.SelectedValue = ds.Tables(0).Rows(0).Item("LeadTimeWM").ToString()
                                    txtLeadTimeComments.Text = ds.Tables(0).Rows(0).Item("LeadTimeComments").ToString()
                                    txtSOP.Text = ds.Tables(0).Rows(0).Item("SOP").ToString()
                                    txtEOP.Text = ds.Tables(0).Rows(0).Item("EOP").ToString()
                                    txtPPAPDt.Text = ds.Tables(0).Rows(0).Item("PPAP").ToString()
                                Else 'no record found reset query string pRptID
                                    Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pTCID=0", False)
                                End If
                            End If

                            If ViewState("pEID") <> 0 Then
                                ds = EXPModule.GetExpProjToolingExpenditure(ViewState("pProjNo"), ViewState("pEID"))
                                If (ds.Tables.Item(0).Rows.Count > 0) Then
                                    txtDescription.Text = ds.Tables(0).Rows(0).Item("Description").ToString()
                                    txtQuantity.Text = ds.Tables(0).Rows(0).Item("Quantity").ToString()
                                    txtAmountPer.Text = ds.Tables(0).Rows(0).Item("Amount").ToString()
                                    txtMPATotalCost.Text = ds.Tables(0).Rows(0).Item("MPA_Amount").ToString()
                                    txtNotes.Text = ds.Tables(0).Rows(0).Item("Notes").ToString()
                                Else 'no record found reset query string pRptID
                                    Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pEID=0", False)
                                End If
                            End If

                            If ViewState("pRID") <> 0 Then
                                ds = EXPModule.GetToolingExpProjRSS(ViewState("pProjNo"), ViewState("pRID"))
                                If (ds.Tables.Item(0).Rows.Count > 0) Then
                                    txtQC.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                                Else 'no record found reset query string pRptID
                                    Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pRID=0&pRC=1", False)
                                End If
                            End If
                            If ViewState("pPrntProjNo") <> Nothing Then
                                lblPrntProjNo.Text = ds.Tables(0).Rows(0).Item("ParentProjectNo").ToString()
                                lblPrntAppDate.Text = ds.Tables(0).Rows(0).Item("OriginalToolApprovedDt").ToString()
                            End If
                        Else
                            If ViewState("pProjNo") = Nothing Then
                                lblProjectID.Text = ProjNo & "?"
                                ddProjectStatus.SelectedValue = "Open"
                                lblPrntProjNo.Text = ProjNo
                                lblPrntAppDate.Text = IIf(ds.Tables(0).Rows(0).Item("OriginalToolApprovedDt").ToString() = "01/01/1900", "", ds.Tables(0).Rows(0).Item("OriginalToolApprovedDt").ToString())
                            Else
                                lblProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                                lblPrntProjNo.Text = ds.Tables(0).Rows(0).Item("ParentProjectNo").ToString()
                                lblPrntAppDate.Text = ds.Tables(0).Rows(0).Item("OriginalToolApprovedDt").ToString()
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message & " 4 "
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindData()

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("ToolingExpProj.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnAppend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAppend.Click

        Response.Redirect("ToolingExpProj.aspx?pProjNo=&pPrntProjNo=" & ViewState("pProjNo"), False)

    End Sub 'EOF btnAppend_Click

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click, btnSave3.Click
        Try
            lblReqRecType.Visible = False
            Dim ProjectStatus As String = Nothing
            ProjectStatus = ViewState("ProjectStatus")

            Dim DefaultDate As Date = Date.Now
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblErrors2.Text = Nothing
            lblErrors2.Visible = False

            Dim EstCmpltDt As String = IIf(txtNextEstCmpltDt.Text = Nothing, IIf(txtHDEstCmpltDt.Text = Nothing, txtEstCmpltDt.Text, txtHDEstCmpltDt.Text), txtNextEstCmpltDt.Text)
            If EstCmpltDt = Nothing Then
                lblErrors.Text = "Estimated Completion Date is blank. Please re-enter."
                lblErrors.Visible = True
                lblErrors2.Text = "Estimated Completion Date is blank. Please re-enter."
                lblErrors2.Visible = True
                lblErrors.Font.Size = 12
                MaintainScrollPositionOnPostBack = False
                Exit Sub
            End If

            Dim SendEmailToDefaultAdmin As Boolean = False

            If (ViewState("pProjNo") <> Nothing Or ViewState("pProjNo") <> "") Then
                '***************
                '* Update Data
                '***************
                UpdateRecord(ProjectStatus, IIf(ProjectStatus = "Closed", "C", IIf(ProjectStatus = "Void", "V", IIf(ProjectStatus = "Open", "N", IIf(ProjectStatus = "Approved", "A", IIf(ProjectStatus = "In Process", txtRoutingStatus.Text, IIf(ProjectStatus = "Rejected", "R", txtRoutingStatus.Text)))))), False, False)

                '**************
                '* Reload the data - may contain calculated information to TotalInv and ProfitLoss
                '**************
                BindData(ViewState("pProjNo"), ViewState("pCO"))

                ''*************
                ''Check for Capitalized, Completed & Void status, send email notfication 
                ''*************
                If ProjectStatus = "Closed" And txtRoutingStatus.Text = "C" Then
                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Closed")
                    SendNotifWhenEventChanges("Closed")
                ElseIf ProjectStatus = "Tooling Completed" Then
                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Tooling Completed")
                    SendNotifWhenEventChanges("Tooling Completed")
                ElseIf ProjectStatus = "Void" And txtRoutingStatus.Text = "V" Then
                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Void")
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Void")
                    End If
                End If

                '**************
                '* Reload the data - may contain calculated information to TotalInv and ProfitLoss
                '**************
                BindData(ViewState("pProjNo"), ViewState("pCO"))

                ViewState("pLS") = 0
            Else
                Dim NewTestReq As Boolean = False
                Dim Consult As Boolean = False
                Dim Current As Boolean = False

                EstCmpltDt = txtEstCmpltDt.Text
                Dim AmtToBeRecovered As Decimal = IIf(txtAmtToBeRecovered.Text = Nothing, 0, txtAmtToBeRecovered.Text)

                '***************
                '* Locate Next available ProjectNo based on Facility selection
                '***************
                Dim ds As DataSet = Nothing
                ds = EXPModule.GetNextExpProjectNo(ViewState("pPrntProjNo"), ddUGNFacility.SelectedValue, "Tooling")

                ViewState("pProjNo") = CType(ds.Tables(0).Rows(0).Item("NextAvailProjNo").ToString, String)

                '***************
                '* Save Data
                '***************
                EXPModule.InsertExpProjTooling(ViewState("pProjNo"), ViewState("pPrntProjNo"), txtProjectTitle.Text, ProjectStatus, ddProjectType.SelectedValue, ddUGNFacility.SelectedValue, ddAccountManager.SelectedValue, ddProgramManager.SelectedValue, ddToolingLead.SelectedValue, ddPurchasingLead.SelectedValue, txtProjDateNotes.Text, txtDateSubmitted.Text, EstCmpltDt, txtExpToolRtnDt.Text, txtEstSpendDt.Text, txtEstRecoveryDt.Text, IIf(txtAmtToBeRecovered.Text = Nothing, 0, txtAmtToBeRecovered.Text), lblPrntAppDate.Text, txtMPAAmtToBeRecovered.Text, DefaultUser, DefaultDate, False)

                ''*****************
                ''History Tracking
                ''*****************
                EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Record created.")

                '***************
                '* Redirect user back to the page.
                '***************
                Dim Aprv As String = Nothing
                If ViewState("pAprv") = 1 Then
                    Aprv = "&pAprv=1"
                End If
                Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pPrntProjNo=" & ViewState("pPrntProjNo") & "&pLS=" & ViewState("pLS") & Aprv, False)
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

    Public Function UpdateRecord(ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal RecSubmitted As Boolean, ByVal ToolEngrMgr As Boolean) As String
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

            Dim AmtToBeRecovered As Decimal = CType(IIf(txtNextAmtToBeRecovered.Text = Nothing, 0, txtNextAmtToBeRecovered.Text), Decimal)
            Dim HDNextATBR As Decimal = txtHDAmtToBeRecovered.Text
            Dim NextATBR As Decimal = txtNextAmtToBeRecovered.Text

            Dim EstCmpltDt As String = IIf(txtNextEstCmpltDt.Text = Nothing, IIf(txtHDEstCmpltDt.Text = Nothing, txtEstCmpltDt.Text, txtHDEstCmpltDt.Text), txtNextEstCmpltDt.Text)

            '************************************
            '* Capture Amt To Be Recovered Change History
            '************************************ 
            If CType(txtHDAmtToBeRecovered.Text, Decimal) <> CType(txtNextAmtToBeRecovered.Text, Decimal) Then
                If txtDateSubmitted.Text <> Nothing Then
                    If txtRoutingStatus.Text <> "N" Then 'And txtRoutingStatus.Text <> "S"
                        SendNotifWhenEventChanges("Amount to be Recovered Changed")
                        EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Amount to be Recovered Changed From '" & Format(HDNextATBR, "#,##0.00") & "' to '" & Format(NextATBR, "#,##0.00") & "' - Reason: " & txtAmtToBeRecoveredChngRsn.Text)
                    End If
                End If
                lblReqAmtToBeRecoveredChange.Visible = False
                lblAmtToBeRecoveredChange.Visible = False
                txtAmtToBeRecoveredChngRsn.Visible = False
            End If

            '************************************
            '* Capture Imp. Date Change History
            '************************************ 
            If txtHDEstCmpltDt.Text <> txtNextEstCmpltDt.Text Then
                If txtDateSubmitted.Text <> Nothing Then
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Estimated Completion Date Changed")
                        EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Estimated Completion Date Changed From '" & txtHDEstCmpltDt.Text & "' to '" & txtNextEstCmpltDt.Text & "' - Reason: " & txtEstCmpltDtChngRsn.Text)
                    End If
                End If
                lblReqEstCmpltDtChange.Visible = False
                lblEstCmpltDtChange.Visible = False
                txtEstCmpltDtChngRsn.Visible = False
            End If

            EXPModule.UpdateExpProjTooling(ViewState("pProjNo"), txtProjectTitle.Text, ProjectStatus, ddProjectType.SelectedValue, ddUGNFacility.SelectedValue, ddAccountManager.SelectedValue, ddProgramManager.SelectedValue, ddToolingLead.SelectedValue, ddPurchasingLead.SelectedValue, txtProjDateNotes.Text, txtDateSubmitted.Text, EstCmpltDt, txtExpToolRtnDt.Text, txtEstSpendDt.Text, txtEstRecoveryDt.Text, AmtToBeRecovered, RoutingStatus, txtActualCost.Text, txtCustomerCost.Text, txtClosingNotes.Text, txtVoidReason.Text, ckRecoveryType1.Checked, IIf(txt1stRecoveryAmt.Text = Nothing, 0, txt1stRecoveryAmt.Text), txt1stRecoveryDate.Text, IIf(txt2ndRecoveryAmt.Text = Nothing, 0, txt2ndRecoveryAmt.Text), txt2ndRecoveryDate.Text, ckRecoveryType2.Checked, txtMPAAmtToBeRecovered.Text, DefaultUser, DefaultDate, ddToolEngrMgr.SelectedValue, ToolEngrMgr)

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

    Protected Sub btnReset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset1.Click, btnReset2.Click, btnReset3.Click, btnReset4.Click, btnReset5.Click, btnReset6.Click
        'Redirect user to the same tab view when Reset button was clicked
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If
        If ViewState("pProjNo") <> "" Then
            If ViewState("pTCID") > 0 Then
                Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pCV=1" & Aprv, False)
            ElseIf ViewState("pEID") > 0 Then
                Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pEV=1" & Aprv, False)
            ElseIf ViewState("pRID") > 0 Then
                Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pRC=1" & Aprv, False)
            ElseIf ViewState("pSD") > 0 Then
                Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pSD=1" & Aprv, False)
            Else
                Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & Aprv, False)
            End If
        Else
            Response.Redirect("ToolingExpProj.aspx", False)
        End If
    End Sub 'EOF btnReset1_Click

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            '***************
            '* Delete Record
            '***************
            If ViewState("pPrntProjNo") = Nothing Then
                EXPModule.DeleteExpProjTooling(ViewState("pProjNo"), ViewState("pPrntProjNo"), False)
            Else
                EXPModule.DeleteExpProjTooling(ViewState("pProjNo"), ViewState("pPrntProjNo"), True)
            End If

            '***************
            '* Redirect user back to the search page.
            '***************
            Response.Redirect("ToolingExpProjList.aspx", False)

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
            Case "Closed"
                txtActualCost.Visible = True
                txtCustomerCost.Visible = True
                txtClosingNotes.Visible = True
                lblReqActualCost.Visible = True
                lblActualCost.Visible = True
                lblCustomerCost.Visible = True
                lblReqCustomerCost.Visible = True
                lblReqClosingNts.Visible = True
                lblClosingNts.Visible = True
                rfvClosingNotes.Enabled = True
                txtActualCost.Enabled = True
                txtCustomerCost.Enabled = True
                txtClosingNotes.Enabled = True
                txtVoidReason.Visible = False
                lblReqVoidRsn.Visible = False
                lblVoidRsn.Visible = False
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
                rfvClosingNotes.Enabled = False
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
        End Select
    End Sub 'EOF ddProjectStatus_SelectedIndexChanged

    Protected Sub txtNextEstCmpltDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNextEstCmpltDt.TextChanged
        Try
            ''If CType(txtHDEstCmpltDt.Text.ToString, Date) <> CType(txtNextEstCmpltDt.Text.ToString, Date) And (ViewState("ProjectStatus") <> "Open") Then
            If txtHDEstCmpltDt.Text <> txtNextEstCmpltDt.Text And (ViewState("ProjectStatus") <> "Open") Then
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

    Protected Sub txtNextAmtToBeRecovered_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNextAmtToBeRecovered.TextChanged
        Try

            If txtHDAmtToBeRecovered.Text <> txtNextAmtToBeRecovered.Text And (ViewState("ProjectStatus") <> "Open") Then
                rfvAmtToBeRecovered.Enabled = False
                rfvAmtToBeRecoveredChngRsn.Enabled = True
                lblReqAmtToBeRecoveredChange.Visible = True
                lblAmtToBeRecoveredChange.Visible = True
                txtAmtToBeRecoveredChngRsn.Visible = True
                txtAmtToBeRecoveredChngRsn.Focus()
            Else
                rfvAmtToBeRecovered.Enabled = True
                rfvAmtToBeRecoveredChngRsn.Enabled = False
                lblReqAmtToBeRecoveredChange.Visible = False
                lblAmtToBeRecoveredChange.Visible = False
                txtAmtToBeRecoveredChngRsn.Visible = False
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
    End Sub 'EOF txtNextAmtToBeRecovered_TextChanged

#End Region 'EOF General - Project Detail

#Region "Customer Info"
    Protected Sub gvCustomer_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCustomer.RowCreated
        ''Do nothing
    End Sub 'EOF gvCustomer_RowCreated

    Protected Sub gvCustomer_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCustomer.RowDataBound
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
                    Dim price As ExpProj.ExpProj_Tooling_CustomerRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProj.ExpProj_Tooling_CustomerRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Customer (" & DataBinder.Eval(e.Row.DataItem, "ddCustomerDesc") & "); Program (" & DataBinder.Eval(e.Row.DataItem, "ProgramName") & "); Part No. (" & DataBinder.Eval(e.Row.DataItem, "PartNo") & ")?');")
                End If
            End If
        End If
    End Sub 'EOF gvCustomer_RowDataBound

    'Protected Sub ddDesignationType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddDesignationType.SelectedIndexChanged
    '    ''If ddPartNo.SelectedValue <> Nothing Then
    '    ''    Exit Sub
    '    ''End If
    '    ''Dim OEMManufacturer As String = Nothing
    '    ''Dim ds As DataSet = Nothing
    '    ''ds = commonFunctions.GetPlatformProgram(0, ddProgram.SelectedValue, "", "", "")
    '    ''If (ds.Tables.Item(0).Rows.Count > 0) Then
    '    ''    OEMManufacturer = ds.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
    '    ''End If
    '    '' ''ddPartNo.ClearSelection()
    '    '' ''ddPartNo.Items.Clear()
    '    '' '' ''bind existing data to drop down Part Number control for selection criteria
    '    '' '' ''ds = commonFunctions.GetBPCSPartNo("", ddDesignationType.SelectedValue)
    '    '' ''ds = commonFunctions.GetPartNo("", ddDesignationType.SelectedValue, "", "", OEMManufacturer)
    '    '' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
    '    '' ''    ddPartNo.DataSource = ds
    '    '' ''    ddPartNo.DataTextField = ds.Tables(0).Columns("ddPartNo").ColumnName.ToString()
    '    '' ''    ddPartNo.DataValueField = ds.Tables(0).Columns("PartNo").ColumnName.ToString()
    '    '' ''    ddPartNo.ClearSelection()
    '    '' ''    ddPartNo.DataBind()
    '    '' ''    ddPartNo.Items.Insert(0, "")
    '    '' ''    ddPartNo.SelectedIndex = 0
    '    '' ''Else
    '    '' ''    ddPartNo.ClearSelection()
    '    '' ''    ddPartNo.DataBind()
    '    '' ''    ddPartNo.Items.Insert(0, "")
    '    '' ''    ddPartNo.SelectedIndex = 0
    '    '' ''End If
    'End Sub 'EOF ddDesignationType_SelectedIndexChanged

    Protected Sub btnAddtoGrid1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddtoGrid1.Click
        ''This function is used to save/update Customer Info.
        Try
            If ViewState("pProjNo") <> Nothing Then
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                Dim PartNo As String = Nothing
                Dim PartDesc As String = Nothing
                Dim ds As DataSet = New DataSet

                lblErrors.Text = Nothing
                lblErrors.Visible = False

                ''**********************************************
                ''Kick out if there is an Obsolete selection
                ''**********************************************
                If InStr(ddProgram.SelectedItem.Text, "**") Then
                    lblErrors.Text = "Invalid Program Selection. System does not allow obsoleted items."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    Exit Sub
                End If

                'If InStr(ddCustomer.SelectedItem.Text, "**") Then
                '    lblErrors.Text = "Invalid Customer Selection. System does not allow obsoleted items."
                '    lblErrors.Visible = True
                '    lblErrors.Font.Size = 12
                '    Exit Sub
                'End If

                'If InStr(ddPartNo.SelectedItem.Text, "**") Then
                '    lblErrors.Text = "Invalid Part Number Selection. System does not allow obsoleted items."
                '    lblErrors.Visible = True
                '    lblErrors.Font.Size = 12
                '    Exit Sub
                'End If

                If txtPartNo.Text = Nothing Then
                    lblErrors.Text = "Part Number was not captured during save. Please re-enter."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    Exit Sub
                End If

                ''************************************************************
                ''Kick out if the Current Year is greater than dates selected
                ' '' ''************************************************************
                If txtEOP.Text <= Today Then
                    lblErrors.Text = "EOP date must be greater than current date."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    Exit Sub
                End If

                PartNo = txtPartNo.Text

                ''***************************************************
                ''Get Part Description
                ''***************************************************
                ds = commonFunctions.GetBPCSPartNo(PartNo, "")
                If commonFunctions.CheckDataSet(ds) = True Then
                    PartDesc = ds.Tables(0).Rows(0).Item("BPCSPartName").ToString()
                End If

                '*************************************************************
                '* Locate the position of the CABBV and SoldTo from ddCustomer
                '*************************************************************
                'Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
                Dim tempCABBV As String = ""
                Dim tempSoldTo As Integer = 0
                'If Not (Pos = 0) Then
                '    tempCABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                '    tempSoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
                'End If
                'If tempCABBV = Nothing Then
                '    tempCABBV = "N/A"
                'End If

                If ViewState("pTCID") = 0 Or ViewState("pTCID") = Nothing Then
                    '********************************************
                    '* Insert Customer Part information to table
                    '********************************************
                    EXPModule.InsertExpProjToolingCustomer(ViewState("pProjNo"), tempCABBV, tempSoldTo, PartNo, IIf(ddProgram.SelectedValue = "", 0, ddProgram.SelectedValue), "", txtRevisionLvl.Text, txtLeadTime.Text, ddLeadTime.SelectedValue, txtLeadTimeComments.Text, txtSOP.Text, txtEOP.Text, txtPPAPDt.Text, PartDesc, "", DefaultUser)

                Else
                    '***************
                    '* Update Customer Part information to table
                    '***************
                    EXPModule.UpdateExpProjToolingCustomer(ViewState("pTCID"), ViewState("pProjNo"), tempCABBV, tempSoldTo, IIf(ddProgram.SelectedValue = "", 0, ddProgram.SelectedValue), PartNo, "", txtRevisionLvl.Text, txtLeadTime.Text, ddLeadTime.SelectedValue, txtLeadTimeComments.Text, txtSOP.Text, txtEOP.Text, txtPPAPDt.Text, PartDesc, "", DefaultUser)

                End If

                Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pCV=1", False)
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
    End Sub 'EOF btnAddtoGrid1_Click
#End Region 'EOF Customer Info

#Region "Recovery - Lump Sum"
    Protected Sub ckRecoveryType1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ckRecoveryType1.CheckedChanged
        Try
            ViewState("toolLumpSum") = False

            If ckRecoveryType1.Checked = True Then
                ckRecoveryType2.Checked = False
                'ckRecoveryType3.Checked = False
                ViewState("toolLumpSum") = True
                ViewState("toolPiecePrice") = False
                'ViewState("toolMonthly") = False

                '***************
                '* Update Data
                '***************
                UpdateRecord(ViewState("ProjectStatus"), txtRoutingStatus.Text, False, False)

                '**************
                '* Reload the data - may contain calculated information to TotalInv and ProfitLoss
                '**************
                BindData(ViewState("pProjNo"), ViewState("pCO"))
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
    End Sub 'EOF ckRecoveryType1_CheckedChanged

    Protected Sub ckRecoveryType2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ckRecoveryType2.CheckedChanged
        Try
            ViewState("toolPiecePrice") = False
            If ckRecoveryType2.Checked = True Then
                ckRecoveryType1.Checked = False
                'ckRecoveryType3.Checked = False

                txt1stRecoveryAmt.Text = 0
                txt1stRecoveryDate.Text = Nothing
                txt2ndRecoveryAmt.Text = 0
                txt2ndRecoveryDate.Text = Nothing

                ViewState("toolLumpSum") = False
                ViewState("toolPiecePrice") = True
                'ViewState("toolMonthly") = False

                '***************
                '* Update Data
                '***************
                Dim DefaultDate As Date = Date.Now
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

                UpdateRecord(ViewState("ProjectStatus"), txtRoutingStatus.Text, False, False)

                '**************
                '* Reload the data - may contain calculated information to TotalInv and ProfitLoss
                '**************
                BindData(ViewState("pProjNo"), ViewState("pCO"))
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
    End Sub 'EOF ckRecoveryType2_CheckedChanged
#End Region 'EOF Recovery - Lump Sum

#Region "Recovery - Yearly Volume"
    Protected Sub gvYearlyVolume_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvYearlyVolume.RowCommand
        Try
            If ckRecoveryType2.Checked = True Then

                ''***
                ''This section allows the inserting of a new row when save button is clicked from the footer.
                ''***
                If e.CommandName = "Insert" Then
                    ''Insert data
                    Dim Year As TextBox
                    Dim Volume As TextBox

                    If gvYearlyVolume.Rows.Count = 0 Then
                        '' We are inserting through the DetailsView in the EmptyDataTemplate
                        Return
                    End If

                    '' Only perform the following logic when inserting through the footer
                    Year = CType(gvYearlyVolume.FooterRow.FindControl("txtYear"), TextBox)
                    odsYearlyVolume.InsertParameters("Year").DefaultValue = Year.Text

                    Volume = CType(gvYearlyVolume.FooterRow.FindControl("txtVolume"), TextBox)
                    odsYearlyVolume.InsertParameters("Volume").DefaultValue = Volume.Text

                    If Year.Text <> Nothing And Volume.Text <> Nothing Then
                        odsYearlyVolume.Insert()
                    End If
                End If

                ''***
                ''This section allows show/hides the footer row when the Edit control is clicked
                ''***
                If e.CommandName = "Edit" Then
                    gvYearlyVolume.ShowFooter = False
                Else
                    If ViewState("ObjectRole") = True Then
                        gvYearlyVolume.ShowFooter = True
                    Else
                        gvYearlyVolume.ShowFooter = False
                    End If
                End If

                ''***
                ''This section clears out the values in the footer row
                ''***
                If e.CommandName = "Undo" Then
                    Dim Year As TextBox
                    Dim Volume As TextBox
                    Year = CType(gvYearlyVolume.FooterRow.FindControl("txtYear"), TextBox)
                    Year.Text = Nothing
                    Volume = CType(gvYearlyVolume.FooterRow.FindControl("txtVolume"), TextBox)
                    Volume.Text = Nothing

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
    End Sub 'EOF gvYearlyVolume_RowCommand

    Protected Sub gvYearlyVolume_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvYearlyVolume.RowDataBound
        Try
            ''***
            ''This section provides the user with the popup for confirming the delete of a record.
            ''Called by the onClientClick event.
            ''***
            If e.Row.RowType = DataControlRowType.DataRow Then
                ''Calculate Footer Totals
                Dim drVolume As ExpProj.ExpProj_Tooling_Yearly_VolumeRow = CType(CType(e.Row.DataItem, DataRowView).Row, ExpProj.ExpProj_Tooling_Yearly_VolumeRow)
                ''Tables.Item(0).Rows.Count > 0) Then
                If DataBinder.Eval(e.Row.DataItem, "Volume") IsNot DBNull.Value Then
                    _totalVolume += drVolume.Volume
                End If
            ElseIf e.Row.RowType = DataControlRowType.Footer Then
                ''Display Totals at footer
                e.Row.Cells(3).Wrap = False
                e.Row.Cells(4).Wrap = False
                e.Row.Cells(5).Wrap = False
                e.Row.Cells(6).Wrap = False
                e.Row.Cells(7).Wrap = False
                e.Row.Cells(8).Wrap = False
                e.Row.Cells(4).ForeColor = Color.Red
                e.Row.Cells(6).ForeColor = Color.Red
                e.Row.Cells(8).ForeColor = Color.Red
                e.Row.Cells(3).Font.Size = 10
                e.Row.Cells(4).Font.Size = 10
                e.Row.Cells(5).Font.Size = 10
                e.Row.Cells(6).Font.Size = 10
                e.Row.Cells(7).Font.Size = 10
                e.Row.Cells(8).Font.Size = 10
                e.Row.Cells(3).Text = "Total Volume: "
                e.Row.Cells(3).ForeColor = Color.Black
                e.Row.Cells(4).Text = String.Format("{0:#,###}", (_totalVolume.ToString / 2)) ''remove duplicate
                e.Row.Cells(5).Text = "Amortization/Part: "
                e.Row.Cells(5).ForeColor = Color.Black
                If _totalVolume.ToString > 0 Then
                    e.Row.Cells(6).Text = Round((lblTotalInvestment.Text / (_totalVolume.ToString / 2)), 2) ''remove duplicate
                Else
                    e.Row.Cells(6).Text = 0
                End If
                e.Row.Cells(7).Text = "Sales Revenue/Part: $"
                e.Row.Cells(7).ForeColor = Color.Black
                If _totalVolume.ToString > 0 Then
                    e.Row.Cells(8).Text = Round((lblAmtRecvrd.Text / (_totalVolume.ToString / 2)), 2) ''remove duplicate
                Else
                    e.Row.Cells(8).Text = 0
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
    End Sub 'EOF gvYearVolume_RowDataBound

    Private Property LoadDataEmpty_YearlyVolume() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_YearlyVolume") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_YearlyVolume"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_YearlyVolume") = value
        End Set
    End Property 'EOF LoadDataEmpty_YearlyVolume

    Protected Sub odsYearlyVolume_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsYearlyVolume.Selected
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)
        'Dim ds As DataSet = CType(e.ReturnValue, DataSet)
        'Dim dt As DataTable = ds.Tables(0)
        Dim dt As ExpProj.ExpProj_Tooling_Yearly_VolumeDataTable = CType(e.ReturnValue, ExpProj.ExpProj_Tooling_Yearly_VolumeDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 And ckRecoveryType2.Checked = True Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty_YearlyVolume = True
        Else
            LoadDataEmpty_YearlyVolume = False
        End If
    End Sub 'EOF odsYearlyVolume_Selected

    Protected Sub gvYearlyVolume_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvYearlyVolume.RowCreated
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
        ' when binding a row, look for a zero row condition based on the flag.
        ' if we have zero data rows (but a dummy row), hide the grid view row
        ' and clear the controls off of that row so they don't cause binding errors

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_YearlyVolume
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If
    End Sub 'EOF gvYearlyVolume_RowCreated
#End Region 'EOF Recovery - Yearly Volume

#Region "Tooling Expense"
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
                    Dim price As ExpProj.ExpProj_Tooling_ExpenditureRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProj.ExpProj_Tooling_ExpenditureRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record """ & """ for " & DataBinder.Eval(e.Row.DataItem, "Description") & "?');")

                End If
            End If
        End If

        ''**************************************************************************************
        ''Reload data - When a delete occurs, it will recalc the TotalInv & Profit/Loss fields.
        ''**************************************************************************************
        BindData(ViewState("pProjNo"), ViewState("pCO"))

    End Sub 'EOF gvExpense_RowDataBound

    Protected Sub btnAddtoGrid2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddtoGrid2.Click
        Try
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            If ViewState("pProjNo") <> Nothing And (ViewState("pEID") = 0 Or ViewState("pEID") = Nothing) Then
                '***************
                '* Insert Expense information to table
                '***************
                EXPModule.InsertExpProjToolingExpenditure(ViewState("pProjNo"), txtDescription.Text, IIf(txtQuantity.Text = "", 0, txtQuantity.Text), IIf(txtAmountPer.Text = "", 0, txtAmountPer.Text), txtNotes.Text, IIf(txtMPATotalCost.Text = Nothing, 0, txtMPATotalCost.Text), DefaultUser)

                gvExpense.DataBind()
                BindData(ViewState("pProjNo"), ViewState("pCO"))
                txtDescription.Text = Nothing
                txtQuantity.Text = Nothing
                txtAmountPer.Text = Nothing
                txtMPATotalCost.Text = Nothing
                txtNotes.Text = Nothing
            Else
                '***************
                '* Update Expense information to table
                '***************
                EXPModule.UpdateExpProjToolingExpenditure(ViewState("pEID"), ViewState("pProjNo"), txtDescription.Text, IIf(txtQuantity.Text = "", 0, txtQuantity.Text), IIf(txtAmountPer.Text = "", 0, txtAmountPer.Text), txtNotes.Text, IIf(txtMPATotalCost.Text = Nothing, 0, txtMPATotalCost.Text), DefaultUser)

                Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pEV=1", False)
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
#End Region 'EOF Tooling Expense

#Region "Communication Board"
    Public Function GoToCommunicationBoard(ByVal ProjectNo As String, ByVal RSSID As Integer, ByVal ApprovalLevel As Integer, ByVal TeamMemberID As Integer) As String

        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If

        Return "ToolingExpProj.aspx?pProjNo=" & ProjectNo & "&pLS=" & ViewState("pLS") & "&pAL=" & ApprovalLevel & "&pTMID=" & TeamMemberID & "&pRID=" & RSSID & "&pRC=1" & Aprv

    End Function 'EOF GoToCommunicationBoard

    Protected Sub btnSave2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave2.Click
        Try
            ''************************************
            ''Send response back to requestor in Communication Board
            ''************************************
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

            Dim SeqNo As Integer = IIf(HttpContext.Current.Request.QueryString("pAL") = "", 0, HttpContext.Current.Request.QueryString("pAL"))
            Dim TMID As Integer = IIf(HttpContext.Current.Request.QueryString("pTMID") = "", 0, HttpContext.Current.Request.QueryString("pTMID"))

            Dim NextSeqNo As Integer = 0
            Dim NextLvl As Integer = 0

            Select Case SeqNo
                Case 1
                    NextSeqNo = 2
                    NextLvl = 12
                Case 2
                    NextSeqNo = 3
                    NextLvl = 13
                Case 3
                    NextSeqNo = 0
                    NextLvl = 0
            End Select

            If SeqNo = 3 Then
                NextLvl = 13
            End If

            Dim ds As DataSet = New DataSet
            Dim dsCC As DataSet = New DataSet
            Dim ds2CC As DataSet = New DataSet

            Dim EmailFrom As String = Nothing
            Dim EmailTO As String = Nothing
            Dim EmpName As String = Nothing
            Dim EmailCC As String = Nothing

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

                ''**********************
                ''*Initialize Variables
                ''**********************
                Dim ToolingLead As Integer = ddToolingLead.SelectedValue
                Dim PurchasingLead As Integer = ddPurchasingLead.SelectedValue
                Dim ProjectTitle As String = txtProjectTitle.Text

                ''***************************************************************
                ''Send Reply back to requestor
                ''***************************************************************
                ds = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), 0, TMID, False, False)
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

                    ''****************************************************************
                    ''Carbon Copy Account/Program Manager & Tooling/Purchasing Lead
                    ''****************************************************************
                    EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                    ''***************************************************************
                    ''Carbon Copy Same Level Approvers
                    ''***************************************************************
                    EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)

                    ''*****************************************************
                    ''Carbon Copy Default Corporate Engineer 
                    ''*****************************************************
                    EmailCC = CarbonCopyList(MyMessage, 52, "", 0, 0, EmailCC, DefaultTMID)

                    ''*****************************************************
                    ''Carbon Copy Default Program Mgmt 
                    ''*****************************************************
                    EmailCC = CarbonCopyList(MyMessage, 127, "", 0, 0, EmailCC, DefaultTMID)

                    ''*****************************************************
                    ''Carbon Copy Tooling Engr Mgr
                    ''*****************************************************
                    EmailCC = CarbonCopyList(MyMessage, 145, "", 0, 0, EmailCC, DefaultTMID)

                    'Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                    Else
                        MyMessage.Subject = ""
                    End If

                    MyMessage.Subject &= ddProjectType.SelectedValue & "- Customer Tooling Expense: " & ViewState("pProjNo") & " - " & ProjectTitle & " - MESSAGE RECEIVED"

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
                    MyMessage.Body &= "         <p><a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crExpProjToolingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to return to the approval page."
                    MyMessage.Body &= "     </td>"
                    MyMessage.Body &= " </tr>"
                    MyMessage.Body &= "</table>"
                    MyMessage.Body &= "<br/><br/>"

                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Message Sent")

                    ''**********************************
                    ''Save Reponse to child table
                    ''**********************************
                    EXPModule.InsertExpProjToolingRSSReply(ViewState("pProjNo"), ViewState("pRID"), ProjectTitle, DefaultTMID, txtReply.Text)

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                        MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                        EmailFrom = "Database.Notifications@ugnauto.com"
                        EmailTO = "lynette.rey@ugnauto.com"
                        EmailCC = "lynette.rey@ugnauto.com"
                    End If

                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (T)", ViewState("pProjNo"))
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
                    mvTabs.ActiveViewIndex = Int32.Parse(6)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(6).Selected = True

                Else 'EmailTO = ''
                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pProjNo"), ViewState("pCO"))

                    lblErrors.Text = "Unable to locate a valid email address to send message to. Please contact the Application Department for assistance."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False

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
            Dim drRSSID As ExpProj.ExpProj_Tooling_RSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, ExpProj.ExpProj_Tooling_RSSRow)

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
                                EXPModule.InsertExpProjToolingDocuments(ViewState("pProjNo"), ddTeamMember.SelectedValue, txtFileDesc.Text, SupportingDocBinaryFile, uploadFile.FileName, SupportingDocEncodeType, SupportingDocFileSize)

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
                Dim db As ImageButton = CType(e.Row.Cells(4).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As ExpProj.ExpProj_Tooling_DocumentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProj.ExpProj_Tooling_DocumentsRow)

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
            Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pSD=1" & Aprv, False)
        End If
    End Sub 'EOF gvSupportingDocument_RowCommand
#End Region 'EOF Supporting Documents

#Region "Approval Status"
    Protected Sub gvApprovers_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Try
            Dim row As GridViewRow = gvApprovers.Rows(e.RowIndex)
            If row IsNot Nothing Then
                lblErrors.Text = Nothing
                lblErrors.Visible = False
                lblReqAppComments.Visible = False
                lblReqAppComments.Text = Nothing

                Dim DefaultDate As Date = Date.Now
                Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

                Dim t As DropDownList = TryCast(row.FindControl("ddStatus"), DropDownList)
                Dim c As TextBox = TryCast(row.FindControl("txtAppComments"), TextBox)
                Dim tm As TextBox = TryCast(row.FindControl("txtTeamMemberID"), TextBox)
                Dim otm As TextBox = TryCast(row.FindControl("txtOrigTeamMemberID"), TextBox)
                Dim TeamMemberID As Integer = CType(tm.Text, Integer)
                Dim OrigTeamMemberID As Integer = CType(otm.Text, Integer)
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
                        EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, (DefaultUser.ToUpper) & " " & t.Text & " this record on behalf of " & ShortName.ToUpper & " - Comments: " & c.Text)

                        ''********
                        ''* Email sent to the next approvers
                        ''********188 371 510 569
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
                            Else
                                ''*******************************************************************
                                ''*Build Email Notification
                                ''*Verify that atleast one Tooling Expense Info entry is entered
                                ''*******************************************************************
                                Dim dsExp As DataSet = New DataSet
                                dsExp = EXPModule.GetExpProjToolingExpenditure(ViewState("pProjNo"), 0)
                                If commonFunctions.CheckDataSet(dsExp) = False Then 'If missing kick user out from submission.
                                    mvTabs.ActiveViewIndex = Int32.Parse(2)
                                    mvTabs.GetActiveView()
                                    mnuTabs.Items(1).Selected = True

                                    rfvDescription.IsValid = False
                                    rfvQuantity.IsValid = False
                                    rfvAmountPer.IsValid = False
                                    vsToolingExpense.ShowSummary = True

                                    lblErrors.Text = "Atleast one Tooling Expense entry is required for submission."
                                    lblErrors.Visible = True
                                Else 'EOF If commonFunctions.CheckDataset(dsExp) = True
                                    ''*****************
                                    ''Declare Variables
                                    ''*****************
                                    Dim ToolingLead As Integer = ddToolingLead.SelectedValue
                                    Dim PurchasingLead As Integer = ddPurchasingLead.SelectedValue
                                    Dim SeqNo As Integer = 0
                                    Dim NextSeqNo As Integer = 0
                                    Dim NextLvl As Integer = 0

                                    Select Case hfSeqNo
                                        Case 1
                                            SeqNo = 1
                                            NextSeqNo = 2
                                            NextLvl = 12
                                        Case 2
                                            SeqNo = 2
                                            NextSeqNo = 3
                                            NextLvl = 13
                                        Case 3
                                            SeqNo = 3
                                            NextSeqNo = 0
                                            NextLvl = 0
                                    End Select

                                    If SeqNo = 3 Then
                                        NextLvl = 13
                                    End If

                                    ''**********************
                                    ''* Save data prior to submission before approvals
                                    ''**********************
                                    If t.SelectedValue = "Approved" And SeqNo = 3 Then
                                        ProjectStatus = "Approved"
                                    Else
                                        ProjectStatus = "In Process"
                                    End If

                                    UpdateRecord(ProjectStatus, IIf(SeqNo = 3, IIf(t.SelectedValue = "Rejected", "R", "A"), IIf(t.SelectedValue = "Rejected", "R", "T")), False, False)

                                    ''***********************************
                                    ''Update Current Level Approver record.
                                    ''***********************************
                                    EXPModule.UpdateExpProjToolingApproval(ViewState("pProjNo"), DefaultTMID, True, t.SelectedValue, ((DefaultUser.ToUpper) & " " & t.Text & " this record on behalf of " & ShortName.ToUpper & IIf(c.Text <> Nothing, " - Comments: " & c.Text, "")), SeqNo, DefaultUser, DefaultDate)

                                    ''*******************************
                                    ''Locate Next Approver
                                    ''*******************************
                                    ' ''Check at same sequence level
                                    ds1st = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), SeqNo, TeamMemberID, True, False)
                                    If commonFunctions.CheckDataSet(ds1st) = False Then
                                        If t.SelectedValue <> "Rejected" And SeqNo <> 3 Then
                                            ds2nd = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), NextSeqNo, 0, True, False)
                                            If commonFunctions.CheckDataSet(ds2nd) = True Then
                                                For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                                    If (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                                                        ((ddToolingLead.SelectedValue <> ds2nd.Tables(0).Rows(i).Item("TeamMemberID")) Or _
                                                        (ddPurchasingLead.SelectedValue <> ds2nd.Tables(0).Rows(i).Item("TeamMemberID"))) Then

                                                        EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                                        EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                                        ''*****************************************
                                                        ''Update Approvers DateNotified field.
                                                        ''*****************************************
                                                        EXPModule.UpdateExpProjToolingApproval(ViewState("pProjNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", NextSeqNo, DefaultUser, DefaultDate)
                                                    End If
                                                Next
                                            End If 'EOF ds2nd.Tables.Count > 0 
                                        End If
                                    End If 'EOF ds1st.Tables.Count > 0

                                    'Rejected or last approval
                                    If t.SelectedValue = "Rejected" Or (SeqNo = 3 And t.SelectedValue = "Approved") Then
                                        ''********************************************************
                                        ''Notify Project Lead
                                        ''********************************************************
                                        dsRej = EXPModule.GetExpProjToolingLead(ViewState("pProjNo"))
                                        ''Check that the recipient(s) is a valid Team Member
                                        If commonFunctions.CheckDataSet(dsRej) = True Then
                                            For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                                If (((ddProjectType.SelectedValue = "Internal" And _
                                                      (dsRej.Tables(0).Rows(i).Item("TMDesc") = "Tooling Lead" Or _
                                                       dsRej.Tables(0).Rows(i).Item("TMDesc") = "Tooling Engr Mgr")) Or _
                                                        (ddProjectType.SelectedValue = "External" And _
                                                         dsRej.Tables(0).Rows(i).Item("TMDesc") = "Purchasing Lead")) Or _
                                                            (dsRej.Tables(0).Rows(i).Item("TMDesc") = "Account Manager" Or _
                                                            dsRej.Tables(0).Rows(i).Item("TMDesc") = "Program Manager")) And _
                                                        (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True) Or _
                                                        (dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                                        (dsRej.Tables(0).Rows(i).Item("TMID") <> ToolingLead) And _
                                                        (dsRej.Tables(0).Rows(i).Item("TMID") <> PurchasingLead) Then

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
                                            ''Carbon Copy Account/Program Manager & Tooling/Purchasing Lead
                                            ''**************************************************************
                                            EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                                        ElseIf (SeqNo = 3 And t.SelectedValue <> "Rejected") Then
                                            ''**************************************
                                            ''*Carbon Copy the Accounting Department
                                            ''**************************************
                                            EmailCC = CarbonCopyList(MyMessage, 10, "", 0, 0, EmailCC, DefaultTMID)

                                            ''*********************************************************
                                            ''*Carbon Copy the Operations Manager based on UGNFacility
                                            ''*********************************************************
                                            EmailCC = CarbonCopyList(MyMessage, 78, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)
                                        End If 'EOF If (SeqNo < 3 And t.SelectedValue <> "Rejected") Then

                                        ''*****************************************************
                                        ''Carbon Copy Default Corporate Engineer 
                                        ''*****************************************************
                                        EmailCC = CarbonCopyList(MyMessage, 52, "", 0, 0, EmailCC, DefaultTMID)

                                        ''*****************************************************
                                        ''Carbon Copy Default Program Mgmt 
                                        ''*****************************************************
                                        EmailCC = CarbonCopyList(MyMessage, 127, "", 0, 0, EmailCC, DefaultTMID)

                                        MyMessage.Subject = ddProjectType.SelectedValue & " - Customer Tooling Expense: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text

                                        MyMessage.Body = "<font size='2' face='Tahoma'>"
                                        If t.SelectedValue = "Rejected" Then
                                            MyMessage.Subject &= " - REJECTED"
                                            MyMessage.Body &= EmpName
                                            MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' was <font color='red'>REJECTED</font>.  "
                                            MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "'>Click here</a> to access the record.<br/><br/>"
                                            MyMessage.Body &= "<i>Reason for rejection:</i> <b><font color='red'>" & c.Text & "</font></b></p>"
                                        Else
                                            If SeqNo = 3 Then
                                                MyMessage.Subject &= "- APPROVED"
                                                MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' is Approved. "
                                                MyMessage.Body &= " <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "'>Click here</a> to access the record.</p>"
                                            Else

                                                MyMessage.Body &= EmpName
                                                MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' is available for your Review/Approval. "
                                                MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crExpProjToolingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"

                                            End If
                                        End If
                                        MyMessage.Body &= "</font>"

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

                                        ''**********************************
                                        ''Connect & Send email notification
                                        ''**********************************
                                        Try
                                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (T)", ViewState("pProjNo"))
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
                                        lblErrors.Font.Size = 12
                                        MaintainScrollPositionOnPostBack = False

                                        ''*****************
                                        ''History Tracking
                                        ''*****************
                                        If t.SelectedValue <> "Rejected" Then
                                            If SeqNo = 3 Then
                                                EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to all involved.")
                                            Else
                                                EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to level " & IIf(SeqNo < 3, (SeqNo + 1), SeqNo) & " approver(s): " & EmpName)
                                            End If
                                        Else
                                            EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to " & EmpName)
                                        End If

                                    Else
                                        lblErrors.Text = "Your response was submitted successfully."
                                        lblErrors.Visible = True
                                        lblReqAppComments.Text = "Your response was submitted successfully."
                                        lblReqAppComments.Visible = True
                                    End If 'EOF EmailTo <> Nothing
                                End If 'EOF If commonFunciton.CheckDataset(dsExp) = True
                            End If 'EOF If Rejected - Comments Required
                        End If 'EOF If Comments is not nothing
                    End If
                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pProjNo"), ViewState("pCO"))
                    gvApprovers.DataBind()

                    ''*************************************************
                    '' "Form Level Security using Roles &/or Subscriptions"
                    ''*************************************************
                    CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                    mvTabs.ActiveViewIndex = Int32.Parse(4)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(4).Selected = True

                Else
                    lblErrors.Text = "Action Cancelled. Please select a status Approved or Rejected."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    lblReqAppComments.Text = "Comments is a required field when approving for another team member."
                    lblReqAppComments.Visible = True
                    lblReqAppComments.ForeColor = Color.Red
                    Exit Sub
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

    Protected Sub gvApprovers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApprovers.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Edit ImageButton
            Dim imgEdit As ImageButton = CType(e.Row.FindControl("ibtnEdit"), ImageButton)
            If imgEdit IsNot Nothing Then
                Dim db2 As ImageButton = CType(e.Row.Cells(8).Controls(1), ImageButton)
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
        BuildApprovalList()
        gvApprovers.DataBind()

        'mvTabs.ActiveViewIndex = Int32.Parse(2)
        'mvTabs.GetActiveView()
        'mnuTabs.Items(2).Selected = True

    End Sub 'EOF btnBuildApproval

    Public Function BuildApprovalList() As String
        Try
            ''********
            ''* This function is used to build the Approval List
            ''********
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
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
                    If (txtRoutingStatus.Text = "R") Or (txtRoutingStatus.Text = "N") Then
                        ''***************
                        ''* Delete 1st Level Approval for rebuild
                        ''***************
                        EXPModule.DeleteExpProjToolingApproval(ViewState("pProjNo"), 0)

                        '***************
                        '* Build 1st level Approval
                        '***************
                        EXPModule.InsertExpProjToolingApproval(ViewState("pProjNo"), ddUGNFacility.SelectedValue, 11, DefaultUser, DefaultDate)

                        '***************
                        '* Build 2nd Level Approval
                        '***************
                        EXPModule.InsertExpProjToolingApproval(ViewState("pProjNo"), ddUGNFacility.SelectedValue, 12, DefaultUser, DefaultDate)

                        '***************
                        '* Build 3rd Level Approval
                        '***************
                        EXPModule.InsertExpProjToolingApproval(ViewState("pProjNo"), ddUGNFacility.SelectedValue, 13, DefaultUser, DefaultDate)


                        gvApprovers.DataBind()
                        'mvTabs.ActiveViewIndex = Int32.Parse(4)
                        'mvTabs.GetActiveView()
                        'mnuTabs.Items(4).Selected = True

                    End If 'EOF If (txtRoutingStatus.Text <> "R") Then                  
                End If 'EOF  If ViewState("pProjNo") <> Nothing Then
            End If 'EOF If CurrentEmpEmail <> Nothing Then

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

    Protected Sub btnFwdToProjLead_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFwdToProjLead.Click
        ''********
        ''* This function is used to submit email to the Project Sponsor after the Originator is done.
        ''********
        Dim DefaultDate As Date = Date.Now
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

        Dim dsPS As DataSet = New DataSet
        Dim dsCC As DataSet = New DataSet
        Dim dsCommodity As DataSet = New DataSet
        Dim EmailFrom As String = Nothing
        Dim EmpName As String = Nothing
        Dim EmailTO As String = Nothing
        Dim EmailCC As String = Nothing
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
        lblErrors2.Text = Nothing
        lblErrors2.Visible = False

        Try

            Dim EstCmpltDt As String = IIf(txtNextEstCmpltDt.Text = Nothing, IIf(txtHDEstCmpltDt.Text = Nothing, txtEstCmpltDt.Text, txtHDEstCmpltDt.Text), txtNextEstCmpltDt.Text)
            If EstCmpltDt = Nothing Then
                lblErrors.Text = "Estimated Completion Date is blank. Please re-enter."
                lblErrors.Visible = True
                lblErrors2.Text = "Estimated Completion Date is blank. Please re-enter."
                lblErrors2.Visible = True
                lblErrors.Font.Size = 12
                Exit Sub
            End If

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then
                BuildApprovalList()

                ''***************
                ''Verify that atleast one Customer Info entry has been entered before
                ''***************
                Dim dsCust As DataSet = New DataSet
                dsCust = EXPModule.GetExpProjToolingCustomer(ViewState("pProjNo"), 0)
                If (dsCust.Tables.Item(0).Rows.Count = 0) Then 'If missing kick user out from submission.
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True
                    ' ''rfvCustomer.IsValid = False
                    rfvProgram.IsValid = False
                    rfvPartNo.IsValid = False
                    rfvSOP.IsValid = False
                    rfvEOP.IsValid = False
                    rfvPPAPDt.IsValid = False
                    vsCustomer.ShowSummary = True
                    lblErrors.Text = "Atleast one Customer/Part entry is required for submission."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                End If

                If (ckRecoveryType1.Checked = False And ckRecoveryType2.Checked = False) Then
                    ''****************
                    ''Recovery Type is required
                    ''****************
                    mvTabs.ActiveViewIndex = Int32.Parse(2)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(2).Selected = True
                    lblReqRecType.Visible = True
                    lblErrors.Text = "Atleast one Recovery Type is required for submission."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                End If

                If (ckRecoveryType1.Checked = True And txt1stRecoveryAmt.Text = Nothing And txt1stRecoveryDate.Text = Nothing) Then
                    ''****************
                    ''Recovery Type is required
                    ''****************
                    mvTabs.ActiveViewIndex = Int32.Parse(2)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(2).Selected = True
                    lblReqRecType.Visible = True
                    lblErrors.Text = "Recovery Amount and Date is required for submission."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                End If

                ''***********************************************
                ''Save data prior to submission before approvals
                ''***********************************************
                UpdateRecord("In Process", "S", False, False)

                ''*************************************************************************
                ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                ''Locate Project Lead Email Addresses
                ''*************************************************************************
                dsPS = EXPModule.GetExpProjToolingLead(ViewState("pProjNo"))
                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(dsPS) = True Then
                    For i = 0 To dsPS.Tables.Item(0).Rows.Count - 1
                        If (dsPS.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                        ((ddProjectType.SelectedValue = "Internal" And dsPS.Tables(0).Rows(i).Item("TMDesc") = "Tooling Lead") Or _
                         (ddProjectType.SelectedValue = "External" And dsPS.Tables(0).Rows(i).Item("TMDesc") = "Purchasing Lead")) And _
                         (dsPS.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                            EmailTO &= dsPS.Tables(0).Rows(i).Item("Email") & ";"
                            EmpName &= dsPS.Tables(0).Rows(i).Item("TMName") & ", "

                        End If
                    Next
                End If

                ''*********
                ''Send Notification only if there is a valid Email Address
                ''**********
                If EmailTO <> Nothing Then
                    Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                    ''************************************
                    ''Carbon Copy Account/Program Manager
                    ''************************************
                    EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                    ''*******************************
                    ''Carbon Copy Tooling Engineering Manager
                    ''*******************************
                    EmailCC = CarbonCopyList(MyMessage, 145, "", 0, 0, EmailCC, DefaultTMID)

                    ''*****************************************************
                    ''Carbon Copy Default Corporate Engineer 
                    ''*****************************************************
                    EmailCC = CarbonCopyList(MyMessage, 52, "", 0, 0, EmailCC, DefaultTMID)

                    ''*****************************************************
                    ''Carbon Copy Default Program Mgmt 
                    ''*****************************************************
                    EmailCC = CarbonCopyList(MyMessage, 127, "", 0, 0, EmailCC, DefaultTMID)


                    'Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                    Else
                        MyMessage.Subject = ""
                    End If

                    MyMessage.Subject &= ddProjectType.SelectedValue & " - Customer Tooling Expense: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text
                    MyMessage.Body = "<font size='2' face='Tahoma'>"
                    MyMessage.Body &= EmpName
                    MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' is available for your review. This record requires Tooling Expense entries. <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</p>"
                    MyMessage.Body &= "</font>"

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
                    EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Forwarded to the Project Lead(s): " & EmpName)

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (T)", ViewState("pProjNo"))

                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                    Catch ex As Exception
                        lblErrors.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                        UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                    End Try
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False

                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pProjNo"), ViewState("pCO"))
                    gvApprovers.DataBind()

                    ''*************************************************
                    '' "Form Level Security using Roles &/or Subscriptions"
                    ''*************************************************
                    CheckRights() '"Form Level Security using Roles &/or Subscriptions"
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
    End Sub 'EOF btnFwdProjLead_Click

    Protected Sub btnFwdToolEngrMgr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFwdToolEngrMgr.Click
        ''********
        ''* This function is used to submit email to the Project Sponsor after the Originator is done.
        ''********
        Dim DefaultDate As Date = Date.Now
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

        Dim dsPS As DataSet = New DataSet
        Dim dsCC As DataSet = New DataSet
        Dim dsCommodity As DataSet = New DataSet
        Dim EmailFrom As String = Nothing
        Dim EmpName As String = Nothing
        Dim EmailTO As String = Nothing
        Dim EmailCC As String = Nothing
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
        lblErrors2.Text = Nothing
        lblErrors2.Visible = False

        Try

            Dim EstCmpltDt As String = IIf(txtNextEstCmpltDt.Text = Nothing, IIf(txtHDEstCmpltDt.Text = Nothing, txtEstCmpltDt.Text, txtHDEstCmpltDt.Text), txtNextEstCmpltDt.Text)
            If EstCmpltDt = Nothing Then
                lblErrors.Text = "Estimated Completion Date is blank. Please re-enter."
                lblErrors.Visible = True
                lblErrors2.Text = "Estimated Completion Date is blank. Please re-enter."
                lblErrors2.Visible = True
                lblErrors.Font.Size = 12
                Exit Sub
            End If

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then
                BuildApprovalList()

                ''***************
                ''Verify that atleast one Tooling Expense Info entry has been entered before
                ''***************
                Dim dsExp As DataSet = New DataSet
                dsExp = EXPModule.GetExpProjToolingExpenditure(ViewState("pProjNo"), 0)
                If (dsExp.Tables.Item(0).Rows.Count = 0) Then 'If missing kick user out from submission.
                    mvTabs.ActiveViewIndex = Int32.Parse(3)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(3).Selected = True

                    rfvDescription.IsValid = False
                    rfvQuantity.IsValid = False
                    rfvAmountPer.IsValid = False
                    vsToolingExpense.ShowSummary = True

                    lblErrors.Text = "Atleast one Tooling Expense entry is required for submission."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                End If

                ''*************************************************************************
                ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                ''Locate Project Lead Email Addresses
                ''*************************************************************************
                dsPS = EXPModule.GetExpProjToolingLead(ViewState("pProjNo"))
                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(dsPS) = True Then
                    For i = 0 To dsPS.Tables.Item(0).Rows.Count - 1
                        If (dsPS.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                        ((ddProjectType.SelectedValue = "Internal" And dsPS.Tables(0).Rows(i).Item("TMDesc") = "Tooling Engr Mgr")) And _
                         (dsPS.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                            EmailTO &= dsPS.Tables(0).Rows(i).Item("Email") & ";"
                            EmpName &= dsPS.Tables(0).Rows(i).Item("TMName") & ", "

                        End If
                    Next
                End If

                ''*********
                ''Send Notification only if there is a valid Email Address
                ''**********
                If EmailTO <> Nothing Then
                    ''***********************************************
                    ''Save data prior to submission before approvals
                    ''***********************************************
                    UpdateRecord("In Process", "S", False, True)

                    Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                    ''************************************
                    ''Carbon Copy Account/Program Manager
                    ''************************************
                    EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                    ''*******************************
                    ''Carbon Copy 1st level Approver 'SAVE Code should TM's decide on this
                    ''*******************************
                    EmailCC = CarbonCopyList(MyMessage, 11, ddUGNFacility.SelectedValue, 1, 0, EmailCC, DefaultTMID)

                    ''*****************************************************
                    ''Carbon Copy Default Corporate Engineer 
                    ''*****************************************************
                    EmailCC = CarbonCopyList(MyMessage, 52, "", 0, 0, EmailCC, DefaultTMID)

                    ''*****************************************************
                    ''Carbon Copy Default Program Mgmt 
                    ''*****************************************************
                    EmailCC = CarbonCopyList(MyMessage, 127, "", 0, 0, EmailCC, DefaultTMID)

                    'Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                    Else
                        MyMessage.Subject = ""
                    End If

                    MyMessage.Subject &= ddProjectType.SelectedValue & " - Customer Tooling Expense: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text
                    MyMessage.Body = "<font size='2' face='Tahoma'>"
                    MyMessage.Body &= EmpName
                    MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' is available for your review. This record requires Tooling Expense entries. <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</p>"
                    MyMessage.Body &= "</font>"

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
                    EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Forwarded to the Project Lead(s): " & EmpName)

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (T)", ViewState("pProjNo"))

                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                    Catch ex As Exception
                        lblErrors.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                        UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                    End Try
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False

                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pProjNo"), ViewState("pCO"))
                    gvApprovers.DataBind()

                    ''*************************************************
                    '' "Form Level Security using Roles &/or Subscriptions"
                    ''*************************************************
                    CheckRights() '"Form Level Security using Roles &/or Subscriptions"
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
    End Sub 'EOF btnFwdToolEngrMgr_Click


    Protected Sub btnFwdApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFwdApproval.Click
        ''********
        ''* This function is used to submit email to the Approvers after the Project Sponsor is done.
        ''********
        Dim DefaultDate As Date = Date.Now
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

        Dim EmailFrom As String = Nothing
        Dim EmpName As String = Nothing
        Dim EmailTO As String = Nothing
        Dim EmailCC As String = Nothing

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
        lblErrors2.Text = Nothing
        lblErrors2.Visible = False
        lblReqAppComments.Text = Nothing
        lblReqAppComments.Visible = False

        Try

            Dim EstCmpltDt As String = IIf(txtNextEstCmpltDt.Text = Nothing, IIf(txtHDEstCmpltDt.Text = Nothing, txtEstCmpltDt.Text, txtHDEstCmpltDt.Text), txtNextEstCmpltDt.Text)
            If EstCmpltDt = Nothing Then
                lblErrors.Text = "Estimated Completion Date is blank. Please re-enter."
                lblErrors.Visible = True
                lblErrors2.Text = "Estimated Completion Date is blank. Please re-enter."
                lblErrors2.Visible = True
                lblErrors.Font.Size = 12
                MaintainScrollPositionOnPostBack = False
                Exit Sub
            End If

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then
                If (txtRoutingStatus.Text <> "R") Then
                    ''***************
                    ''* Delete 2nd Level Approval for rebuild
                    ''***************
                    EXPModule.DeleteExpProjToolingApproval(ViewState("pProjNo"), 2)

                    '***************
                    '* Build 2nd Level Approval
                    '***************
                    EXPModule.InsertExpProjToolingApproval(ViewState("pProjNo"), ddUGNFacility.SelectedValue, 12, DefaultUser, DefaultDate)
                Else
                    If txtHDTotalInvestment.Text <> 0 And _
                    txtHDTotalInvestment.Text <> lblTotalInvestment.Text Then
                        BuildApprovalList()
                        EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Total Investment amount changed from $" & txtHDTotalInvestment.Text & " to $" & lblTotalInvestment.Text)
                    ElseIf txtHDTotalInvestment.Text = 0 Then
                        BuildApprovalList()
                    End If
                End If



                ''***************
                ''Verify that atleast one Tooling Expense Info entry has been entered before
                ''***************
                Dim dsExp As DataSet = New DataSet
                dsExp = EXPModule.GetExpProjToolingExpenditure(ViewState("pProjNo"), 0)
                If (dsExp.Tables.Item(0).Rows.Count = 0) Then 'If missing kick user out from submission.
                    mvTabs.ActiveViewIndex = Int32.Parse(3)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(3).Selected = True

                    rfvDescription.IsValid = False
                    rfvQuantity.IsValid = False
                    rfvAmountPer.IsValid = False
                    vsToolingExpense.ShowSummary = True

                    lblErrors.Text = "Atleast one Tooling Expense entry is required for submission."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                End If

                ''**********************
                ''* Save data prior to submission before approvals
                ''**********************
                UpdateRecord("In Process", "T", False, False)

                ''*************************************************************************
                ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                ''*************************************************************************
                Dim ds1st As DataSet = New DataSet
                Dim ds2nd As DataSet = New DataSet
                Dim dsCC As DataSet = New DataSet
                Dim dsCommodity As DataSet = New DataSet
                Dim i As Integer = 0
                Dim SponsSameAs1stLvlAprvr As Boolean = False
                Dim SeqNo As Integer = 0
                Dim OrigTMID As Integer = 0

                ''*******************************
                ''Locate 1st level approver
                ''*******************************
                If (txtRoutingStatus.Text <> "R") Then
                    ds1st = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), 1, 0, False, False)
                Else 'IF Rejected - only notify the TM who Rejected the record
                    If txtHDTotalInvestment.Text = lblTotalInvestment.Text Then
                        ds1st = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), 0, 0, False, True)
                    Else
                        ds1st = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), 1, 0, False, False)
                    End If
                End If

                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(ds1st) = True Then
                    For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                        If (ds1st.Tables(0).Rows(i).Item("Email").ToString.ToUpper <> CurrentEmpEmail.ToUpper) And _
                            (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                            ((ddToolingLead.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID")) Or _
                            (ddPurchasingLead.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID"))) Then

                            EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"
                            EmpName &= ds1st.Tables(0).Rows(i).Item("EmailTMName") & ", "

                            ''**********************************************************
                            ''Update 1st level DateNotified field.
                            ''**********************************************************
                            If (txtRoutingStatus.Text <> "R") Then
                                EXPModule.UpdateExpProjToolingApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 1, DefaultUser, DefaultDate)
                            Else 'IF Rejected - only notify the TM who Rejected the record
                                EXPModule.UpdateExpProjToolingApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), DefaultUser, DefaultDate)
                                SeqNo = ds1st.Tables(0).Rows(i).Item("SeqNo")
                                OrigTMID = ds1st.Tables(0).Rows(i).Item("OrigTeamMemberID")
                            End If

                        Else
                            ''**********************************************************
                            ''1st Level Approver same as Project Sponsor.  
                            '' Update(record.DefaultTMID)
                            ''**********************************************************
                            If (txtRoutingStatus.Text <> "R") Then
                                EXPModule.UpdateExpProjToolingApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), True, "Approved", "", 1, DefaultUser, DefaultDate)
                            Else 'IF Rejected - only notify the TM who Rejected the record
                                EXPModule.UpdateExpProjToolingApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Approved", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), DefaultUser, DefaultDate)
                                SeqNo = ds1st.Tables(0).Rows(i).Item("SeqNo")
                                OrigTMID = ds1st.Tables(0).Rows(i).Item("OrigTeamMemberID")
                            End If

                            If (ds1st.Tables(0).Rows(i).Item("SubmitFlag") = True) Then
                                SponsSameAs1stLvlAprvr = True
                            End If
                        End If
                    Next
                End If

                ''***************************************************************
                ''Locate 2nd Level Approver(s)
                ''***************************************************************
                If SponsSameAs1stLvlAprvr = True Then
                    ds2nd = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), 2, 0, False, False)
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(ds2nd) = True Then
                        For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                            If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                            (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                ''******************************************************
                                ''Update 2nd level DateNotified field.
                                ''******************************************************
                                EXPModule.UpdateExpProjToolingApproval(ViewState("pProjNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 2, DefaultUser, DefaultDate)
                            End If
                        Next
                    End If 'EOF If commonFunctions.CheckDataSet(ds2nd) = True Then
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
                        ''Carbon Copy Account/Program Manager & Tooling/Purchasing Lead
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                        ''********************************
                        ''Carbon Copy Ops Manager
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 78, ddUGNFacility.SelectedValue, 0, 0, EmailCC, DefaultTMID)

                    Else 'Rejected
                        ''********************************
                        ''Carbon Copy Account/Program Manager & Tooling/Purchasing Lead
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                        ''********************************
                        ''Carbon Copy Same Level
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 0, ddUGNFacility.SelectedValue, IIf(SeqNo = 3, (SeqNo - 1), SeqNo), OrigTMID, EmailCC, DefaultTMID)
                    End If

                    ''*****************************************************
                    ''Carbon Copy Default Corporate Engineer 
                    ''*****************************************************
                    EmailCC = CarbonCopyList(MyMessage, 52, "", 0, 0, EmailCC, DefaultTMID)

                    ''*****************************************************
                    ''Carbon Copy Default Program Mgmt 
                    ''*****************************************************
                    EmailCC = CarbonCopyList(MyMessage, 127, "", 0, 0, EmailCC, DefaultTMID)

                    If ddProjectType.SelectedValue = "Internal" Then
                        ''*****************************************************
                        ''Carbon Copy Tooling Engineering Manager
                        ''*****************************************************
                        EmailCC = CarbonCopyList(MyMessage, 145, "", 0, 0, EmailCC, DefaultTMID)
                    End If

                    'Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                    Else
                        MyMessage.Subject = ""
                    End If

                    MyMessage.Subject &= ddProjectType.SelectedValue & " - Customer Tooling Expense: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text

                    MyMessage.Body = "<font size='2' face='Tahoma'>"
                    MyMessage.Body &= EmpName
                    MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' is available for your Review/Approval. <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crExpProjToolingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"

                    If txtReSubmit.Text <> Nothing Then
                        MyMessage.Body &= "<p><i>Reason for resubmission:</i> <b><font color='red'>" & txtReSubmit.Text & "</font></b></p>"
                    End If

                    MyMessage.Body &= "</font>"
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
                    EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Record completed and forwarded to " & EmpName & " for approval.")


                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (T)", ViewState("pProjNo"))
                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                    Catch ex As SmtpException
                        lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
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
                    BindData(ViewState("pProjNo"), ViewState("pCO"))
                    gvApprovers.DataBind()

                    ''*************************************************
                    '' "Form Level Security using Roles &/or Subscriptions"
                    ''*************************************************
                    CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                    mvTabs.ActiveViewIndex = Int32.Parse(4)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(4).Selected = True

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
                    dsCC = EXPModule.GetExpProjToolingLead(ViewState("pProjNo"))
                    IncludeOrigAprvlTM = True
                Else '' All others based on facility
                    If (UGNLoc <> Nothing) Then
                        dsCC = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(SubscriptionID, UGNLoc)
                    Else
                        If SubscriptionID = 10 Or SubscriptionID = 11 Or SubscriptionID = 145 Or SubscriptionID = 12 Or SubscriptionID = 13 Or SubscriptionID = 52 Or SubscriptionID = 127 Then
                            ''Notify Accounting or 1st level or 2nd level or 3rd level, Dflt Corp Engineer, Dflt Prgm Mgr
                            dsCC = commonFunctions.GetTeamMemberBySubscription(SubscriptionID)
                        End If
                    End If
                End If

                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    If SubscriptionID = 0 Then
                        For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                            If ((ddProjectType.SelectedValue = "Internal" And dsCC.Tables(0).Rows(i).Item("TMDesc") = "Tooling Lead") Or _
                                (ddProjectType.SelectedValue = "External" And dsCC.Tables(0).Rows(i).Item("TMDesc") = "Purchasing Lead") Or _
                                (dsCC.Tables(0).Rows(i).Item("TMDesc") = "Account Manager" Or _
                                 dsCC.Tables(0).Rows(i).Item("TMDesc") = "Program Manager")) And _
                                 (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"

                            End If
                        Next
                    Else
                        For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                            If (dsCC.Tables(0).Rows(i).Item("TMID") <> DefaultTMID) And _
                                (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"

                            End If
                        Next
                    End If
                End If
            Else 'Notify same level approvers after a rejection has been released 
                dsCC = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), SeqNo, 0, False, False)
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
                dsCC = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), 0, 0, False, False)
                'CC Orgiginal Approver in an event that their a corporate calendar default responsibility
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                            (DefaultTMID <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) And _
                            dsCC.Tables(0).Rows(i).Item("OrigEmail") <> dsCC.Tables(0).Rows(i).Item("Email") Then

                            EmailCC &= dsCC.Tables(0).Rows(i).Item("OrigEmail") & ";"

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

        MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 12; font-family: Tahoma;'>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'>"
        MyMessage.Body &= "     <td colspan='5'><strong>Projected Date Notes</strong></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr style='border-color:white;'>"
        MyMessage.Body &= "     <td colspan='5'>" & txtProjDateNotes.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= " <td colspan='5'>&nbsp;</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:white;'>"
        MyMessage.Body &= "     <td><strong>UGN Location </strong></td>"
        MyMessage.Body &= "     <td><strong>Est. Completion Date</strong></td>"
        MyMessage.Body &= "     <td><strong>Est. Start Spend Date</strong></td>"
        MyMessage.Body &= "     <td><strong>Est. Tool Return Date</strong></td>"
        MyMessage.Body &= "     <td><strong>Est. Customer Recovery Date</strong></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "     <td>" & ddUGNFacility.SelectedItem.Text & "</td>"
        MyMessage.Body &= "     <td>" & txtNextEstCmpltDt.Text & "</td>"
        MyMessage.Body &= "     <td>" & txtEstSpendDt.Text & "</td>"
        MyMessage.Body &= "     <td>" & txtExpToolRtnDt.Text & "</td>"
        MyMessage.Body &= "     <td>" & txtEstRecoveryDt.Text & "</td>"
        MyMessage.Body &= " </tr>"
        MyMessage.Body &= " <tr>"
        MyMessage.Body &= "     <td colspan='5'>"
        MyMessage.Body &= "     <table width='80%' style='font-size: 12; font-family: Tahoma;'>"
        MyMessage.Body &= "         <tr bgcolor='#EBEBEB' style='border-color:#EBEBEB; text-align:center'>"
        MyMessage.Body &= "             <td colspan='2'><strong>Requested Approval</strong></td>"
        MyMessage.Body &= "             <td colspan='2'><strong>Memo at Program Awarded</strong></td>"
        MyMessage.Body &= "         </tr>"
        MyMessage.Body &= "         <tr>"
        MyMessage.Body &= "             <td class='p_text' align='right' width='150px'>Amount to be Recovered ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "             <td>" & lblAmtRecvrd.Text & "</td>"
        MyMessage.Body &= "             <td class='p_text' align='right' width='150px'>Amount to be Recovered ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "             <td>" & txtMPAAmtToBeRecovered.Text & "</td>"
        MyMessage.Body &= "         </tr>"
        MyMessage.Body &= "         <tr>"
        MyMessage.Body &= "             <td class='p_text' align='right'>Total Investment ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "             <td>" & lblTotalInvestment.Text & "</td>"
        MyMessage.Body &= "             <td class='p_text' align='right'>Total Investment ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "             <td>" & lblMPATotalInvestment.Text & "</td>"
        MyMessage.Body &= "         </tr>"

        MyMessage.Body &= "     </table>"
        MyMessage.Body &= "     </td>"
        MyMessage.Body &= " </tr>"

        If ckRecoveryType1.Checked = True Then
            MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:white;'>"
            MyMessage.Body &= "     <td><strong>Recovery Type </strong></td>"
            MyMessage.Body &= "     <td><strong>1st Recovery Amount </strong></td>"
            MyMessage.Body &= "     <td><strong>1st Recovery Date </strong></td>"
            MyMessage.Body &= "     <td colspan='2'></td>"
            MyMessage.Body &= "</tr>"

            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "     <td height='25'>Lump Sum</td>"
            MyMessage.Body &= "     <td height='25'>$ " & txt1stRecoveryAmt.Text & "</td>"
            MyMessage.Body &= "     <td height='25'>" & txt1stRecoveryDate.Text & "</td>"
            MyMessage.Body &= "     <td colspan='2'></td>"
            MyMessage.Body &= "</tr>"
        End If

        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td colspan='5'>&nbsp;</td>"
        MyMessage.Body &= "</tr>"

        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td colspan='5'>"

        ''***************************************************
        ''Get list of Customer/Part information for display
        ''***************************************************
        MyMessage.Body &= "<table style='font-size: 11; font-family: Tahoma;'>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
        MyMessage.Body &= "<td><strong>Customer</strong></td>"
        MyMessage.Body &= "<td><strong>Program / Platform / Assembly Plant</strong></td>"
        MyMessage.Body &= "<td><strong>Part Number</strong></td>"
        MyMessage.Body &= "<td><strong>Lead Time</strong></td>"
        MyMessage.Body &= "<td><strong>SOP Date </strong></td>"
        MyMessage.Body &= "<td><strong>EOP Date </strong></td>"
        MyMessage.Body &= "<td><strong>Est. PPAP Date </strong></td>"
        MyMessage.Body &= "</tr>"

        Dim dsCP As DataSet
        dsCP = EXPModule.GetExpProjToolingCustomer(ViewState("pProjNo"), 0)
        If dsCP.Tables.Count > 0 And (dsCP.Tables.Item(0).Rows.Count > 0) Then
            For i = 0 To dsCP.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= "<tr style='border-color:white'>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("ddCustomerDesc") & "&nbsp;</td>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("ProgramName") & "&nbsp;</td>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("PartNo") & "&nbsp;</td>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("LeadTime") & "&nbsp;</td>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("SOP") & "&nbsp;</td>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("EOP") & "&nbsp;</td>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("PPAP") & "&nbsp;</td>"
                MyMessage.Body &= "</tr>"
            Next
        End If
        MyMessage.Body &= "</Table>"
        MyMessage.Body &= "</td></tr>"

        ''***************************************************
        ''Get list of Supporting Documentation
        ''***************************************************
        Dim dsTED As DataSet
        dsTED = EXPModule.GetToolingExpDocument(ViewState("pProjNo"), 0)
        If dsTED.Tables.Count > 0 And (dsTED.Tables.Item(0).Rows.Count > 0) Then
            MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
            MyMessage.Body &= "<td colspan='5'><strong>SUPPORTING DOCUMENT(S):</strong></td>"
            MyMessage.Body &= "</tr>"
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td colspan='5'>"
            MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
            MyMessage.Body &= "  <tr>"
            MyMessage.Body &= "   <td width='250px'><b>File Description</b></td>"
            MyMessage.Body &= "   <td width='250px'>&nbsp;</td>"
            MyMessage.Body &= "</tr>"

            For i = 0 To dsTED.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td height='25'>" & dsTED.Tables(0).Rows(i).Item("Description") & "</td>"
                MyMessage.Body &= "<td height='25'><a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/ToolingExpProjDocument.aspx?pProjNo=" & ViewState("pProjNo") & "&pDocID=" & dsTED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsTED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
                MyMessage.Body &= "</tr>"
            Next

            MyMessage.Body &= "</table>"
            MyMessage.Body &= "</tr>"
        End If

        MyMessage.Body &= "</Table>"

        Return True

    End Function 'EOF EmailBody()

    Public Sub SendNotifWhenEventChanges(ByVal EventDesc As String)
        ''********
        ''* This section will be used in the following methods:
        ''*     1) Email sent to all involved when an Asset is Tooling Completed
        ''*     2) Email sent to all involved with an Asset is VOID
        ''*     3) Email sent to Account with an Asset is COMPLETED
        ''********188 371 510 569
        Dim DefaultDate As Date = Date.Now
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
        Dim DefaultUserName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value


        Dim ds1st As DataSet = New DataSet
        Dim ds2nd As DataSet = New DataSet
        Dim dsCC As DataSet = New DataSet
        Dim dsCommodity As DataSet = New DataSet
        Dim EmailFrom As String = Nothing
        Dim EmpName As String = Nothing
        Dim EmailTO As String = Nothing
        Dim EmailCC As String = Nothing
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


        Dim HDNextATBR As Decimal = txtHDAmtToBeRecovered.Text
        Dim NextATBR As Decimal = txtNextAmtToBeRecovered.Text
        Dim ActualCost As Decimal = txtActualCost.Text
        Dim CustomerCost As Decimal = txtCustomerCost.Text

        lblErrors.Text = Nothing
        lblErrors.Visible = False

        Try
            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then
                ''Is this for a Group Notification or Individual
                Select Case EventDesc
                    Case "Closed" 'Sent by Accounting, notify all
                        GroupNotif = True
                    Case "Void" 'Sent by Project Leader, notify all
                        GroupNotif = True
                    Case "Tooling Completed" 'Sent by Project Leader, notify accounting
                        GroupNotif = False
                    Case "Estimated Completion Date Changed" 'Sent by either the Initiator or Project Lead, notify all
                        GroupNotif = True
                    Case "Amount to be Recovered Changed" 'Sent by either the Initiator or Project Lead, notify all
                        GroupNotif = True
                End Select

                ''*********************************
                ''Send Notification
                ''*********************************
                If GroupNotif = True Then
                    ''********************************************************
                    ''Notify Account/Program Manager & Tooling/Purchasing Lead
                    ''********************************************************
                    EmailTO = CarbonCopyList(Nothing, 0, "", 0, 0, EmailTO, DefaultTMID)

                    If txtRoutingStatus.Text = "A" Or txtRoutingStatus.Text = "T" Or txtRoutingStatus.Text = "C" Or txtRoutingStatus.Text = "V" Then
                        If txtRoutingStatus.Text <> "T" Then
                            '**************************************
                            '*Notify Accounting Department
                            '**************************************
                            EmailTO = CarbonCopyList(Nothing, 10, "", 0, 0, EmailTO, DefaultTMID)

                            '*********************************************************
                            '*Notify Operations Manager based on UGNFacility
                            '*********************************************************
                            EmailTO = CarbonCopyList(Nothing, 78, ddUGNFacility.SelectedValue, 0, 0, EmailTO, DefaultTMID)
                        End If

                        ''*********************************************************
                        ''*Notify Approvers
                        ''*********************************************************
                        EmailTO = CarbonCopyList(Nothing, 13, ddUGNFacility.SelectedValue, 0, 0, EmailTO, DefaultTMID)
                        EmailTO = CarbonCopyList(Nothing, 12, ddUGNFacility.SelectedValue, 0, 0, EmailTO, DefaultTMID)
                        EmailTO = CarbonCopyList(Nothing, 11, ddUGNFacility.SelectedValue, 0, 0, EmailTO, DefaultTMID)

                    End If

                Else 'GroupNotif = False   
                    '*******************************************
                    'Notify Accounting Department
                    '*******************************************
                    EmailTO = CarbonCopyList(Nothing, 10, "", 0, 0, EmailTO, DefaultTMID)

                End If 'EOF  If GroupNotif = True Then

                If txtRoutingStatus.Text = "A" Or txtRoutingStatus.Text = "T" Or txtRoutingStatus.Text = "C" Or txtRoutingStatus.Text = "V" Then
                    ''*****************************************************
                    ''Carbon Copy Default Corporate Engineer 
                    ''*****************************************************
                    EmailTO = CarbonCopyList(Nothing, 52, "", 0, 0, EmailTO, DefaultTMID)

                    ''*****************************************************
                    ''Carbon Copy Default Program Mgmt 
                    ''*****************************************************
                    EmailTO = CarbonCopyList(Nothing, 127, "", 0, 0, EmailTO, DefaultTMID)

                    ''*****************************************************
                    ''Carbon Copy Tooling Engr Mgr 
                    ''*****************************************************
                    EmailTO = CarbonCopyList(Nothing, 145, "", 0, 0, EmailTO, DefaultTMID)
                End If
            End If

            ''********************************************************
            ''Send Notification only if there is a valid Email Address
            ''********************************************************
            If EmailTO <> Nothing Then
                Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                'Test or Production Message display
                If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                    MyMessage.Subject = "TEST: "
                Else
                    MyMessage.Subject = ""
                End If

                MyMessage.Subject &= ddProjectType.SelectedValue & " - Customer Tooling Expense: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text & " - " & EventDesc

                MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"

                MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>'" & EventDesc.ToUpper & "' by " & DefaultUserName & ".</strong></td>"

                MyMessage.Body &= "</tr>"
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td class='p_text' align='right' style='width: 150px;'>Project No:&nbsp;&nbsp; </td>"
                MyMessage.Body &= "<td> <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>" & ViewState("pProjNo") & "</a></td>"
                MyMessage.Body &= "</tr>"
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td class='p_text' align='right'>Project Title:&nbsp;&nbsp; </td>"
                MyMessage.Body &= "<td>" & txtProjectTitle.Text & "</td>"
                MyMessage.Body &= "</tr>"

                Select Case EventDesc
                    Case "Closed" 'Sent by Accounting, notify all
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>Actual Cost:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>$ " & Format(ActualCost, "#,##0.00") & "</td>"
                        MyMessage.Body &= "</tr>"
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>Customer Cost:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>$ " & Format(CustomerCost, "#,##0.00") & "</td>"
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
                    Case "Tooling Completed" 'Sent by Project Leader, notify accounting
                        ''no additional info needed all in the subject line
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
                    Case "Amount to be Recovered Changed" 'Sent by Project Leader, notify all
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>Previous Value:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>$ " & Format(HDNextATBR, "#,##0.00") & "</td>"
                        MyMessage.Body &= "</tr>"
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>New Value:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>$ " & Format(NextATBR, "#,##0.00") & "</td>"
                        MyMessage.Body &= "</tr>"
                        MyMessage.Body &= "<tr>"
                        MyMessage.Body &= "<td class='p_text' align='right'>Change Reason:&nbsp;&nbsp; </td>"
                        MyMessage.Body &= "<td style='width: 600px;'>" & txtAmtToBeRecoveredChngRsn.Text & "</td>"
                        MyMessage.Body &= "</tr>"
                End Select
                MyMessage.Body &= "</table>"


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
                    commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (T)", ViewState("pProjNo"))
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
                BindData(ViewState("pProjNo"), False)
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
#End Region 'EOF Email Notifications

#Region "Save unused code"
    ' ''Protected Sub ddPartNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddPartNo.SelectedIndexChanged
    ' ''    If ddPartNo.SelectedIndex = 1 Then
    ' ''        ViewState("pFPNo") = True
    ' ''        vsCustomer.ShowSummary = True
    ' ''    Else
    ' ''        ViewState("pFPNo") = False
    ' ''    End If
    ' ''End Sub 'EOF ddPartNo_SelectedIndexChanged
    ' ''Protected Sub ddProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProgram.SelectedIndexChanged
    ' ''    If ddProgram.SelectedValue <> Nothing Then
    ' ''        ''System.Threading.Thread.Sleep(3000)

    ' ''        Dim ds As DataSet = New DataSet
    ' ''        ds = commonFunctions.GetPlatformProgram(0, ddProgram.SelectedValue, "", "", ddMakes.SelectedValue)
    ' ''        If commonFunctions.CheckDataSet(ds) = True Then
    ' ''            Dim NoOfDays As String = Nothing
    ' ''            Select Case ds.Tables(0).Rows(0).Item("EOPMM").ToString()
    ' ''                Case "01"
    ' ''                    NoOfDays = "31"
    ' ''                Case "02"
    ' ''                    NoOfDays = "28"
    ' ''                Case "03"
    ' ''                    NoOfDays = "31"
    ' ''                Case "04"
    ' ''                    NoOfDays = "30"
    ' ''                Case "05"
    ' ''                    NoOfDays = "31"
    ' ''                Case "06"
    ' ''                    NoOfDays = "30"
    ' ''                Case "07"
    ' ''                    NoOfDays = "31"
    ' ''                Case "08"
    ' ''                    NoOfDays = "31"
    ' ''                Case "09"
    ' ''                    NoOfDays = "30"
    ' ''                Case 10
    ' ''                    NoOfDays = "31"
    ' ''                Case 11
    ' ''                    NoOfDays = "30"
    ' ''                Case 12
    ' ''                    NoOfDays = "31"
    ' ''            End Select
    ' ''            If ds.Tables(0).Rows(0).Item("EOPMM").ToString() <> "" Then
    ' ''                txtEOP.Text = ds.Tables(0).Rows(0).Item("EOPMM").ToString() & "/" & NoOfDays & "/" & ds.Tables(0).Rows(0).Item("EOPYY").ToString()
    ' ''            End If
    ' ''            If ds.Tables(0).Rows(0).Item("SOPMM").ToString() <> "" Then
    ' ''                txtSOP.Text = ds.Tables(0).Rows(0).Item("SOPMM").ToString() & "/01/" & ds.Tables(0).Rows(0).Item("SOPYY").ToString()
    ' ''            End If
    ' ''            cddOEMMfg.SelectedValue = ds.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
    ' ''            iBtnPreviewDetail.Visible = True
    ' ''        Else
    ' ''            iBtnPreviewDetail.Visible = False
    ' ''        End If
    ' ''    End If 'EOF ddProgram.SelectedValue

    ' ''End Sub 'EOF ddProgram_SelectedIndexChanged

    'Public Function GoToDetailPP(ByVal ProgramID As Integer) As String

    '    If ProgramID <> Nothing Then
    '        Dim ds As DataSet = New DataSet
    '        ds = commonFunctions.GetPlatformProgram(0, ProgramID, "", "", "")
    '        If commonFunctions.CheckDataSet(ds) = True Then
    '            Dim PlatformID As Integer = ds.Tables(0).Rows(0).Item("PlatformID").ToString()
    '            Return "~/DataMaintenance/ProgramDisplay.aspx?pPlatID=" & PlatformID & "&pPgmID=" & ProgramID
    '        Else
    '            Return ""
    '        End If
    '    Else
    '        Return ""
    '    End If
    'End Function 'EOF GoToDetailPP

    'Protected Sub ckRecoveryType3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ckRecoveryType3.CheckedChanged
    '    Try
    '        ViewState("toolMonthly") = False
    '        If ckRecoveryType3.Checked = True Then
    '            ckRecoveryType1.Checked = False
    '            ckRecoveryType2.Checked = False

    '            txt1stRecoveryAmt.Text = 0
    '            txt1stRecoveryDate.Text = Nothing
    '            txt2ndRecoveryAmt.Text = 0
    '            txt2ndRecoveryDate.Text = Nothing

    '            ViewState("toolLumpSum") = False
    '            ViewState("toolPiecePrice") = False
    '            ViewState("toolMonthly") = True
    '            '**************
    '            '* Reload the data - may contain calculated information to TotalInv and ProfitLoss
    '            '**************
    '            BindData()
    '        End If
    '    Catch ex As Exception
    '        'update error on web page
    '        lblErrors.Text = ex.Message
    '        lblErrors.Visible = True

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub 'EOF ckRecoveryType3_CheckedChanged
#End Region ' Save unused codes"
End Class
