' ************************************************************************************************
' Name:	PackagingExpProj.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 07/23/2010    LRey	Created .Net application
' 08/01/2012    LRey    Add Memo at Program Awarded fields per R.Khalaf enhancement doc 06/11/2012
' 01/07/2013    LRey    Added a control to hide the Edit button in the approval process to prevent out of sequence approval.
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

Partial Class EXP_PackagingExpProj
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

            ''Used for Customer Info binddata and update
            If HttpContext.Current.Request.QueryString("pPCID") <> "" Then
                ViewState("pPCID") = HttpContext.Current.Request.QueryString("pPCID")
            Else
                ViewState("pPCID") = 0
            End If

            ''Used to take user back to Customer Info Tab after reset/save
            If HttpContext.Current.Request.QueryString("pCV") <> "" Then
                ViewState("pCV") = HttpContext.Current.Request.QueryString("pCV")
            Else
                ViewState("pCV") = 0
            End If

            ''Used for Packaging Expenditure binddata and update
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

            ''Used to take user back to Packaging Expenditure Tab after reset/save
            If HttpContext.Current.Request.QueryString("pEV") <> "" Then
                ViewState("pEV") = HttpContext.Current.Request.QueryString("pEV")
            Else
                ViewState("pEV") = 0
            End If

            If HttpContext.Current.Request.QueryString("pNF") <> "" Then
                ViewState("pNF") = HttpContext.Current.Request.QueryString("pNF")
            Else
                ViewState("pNF") = 0
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
            ''from PE Tracking system
            If HttpContext.Current.Request.QueryString("pLS") <> "" Then
                ViewState("pLS") = CType(HttpContext.Current.Request.QueryString("pLS"), Boolean)
            Else
                ViewState("pLS") = 0
            End If

            ''Used to add supplement for a record that has been carried over
            ''from old Packaging Expenditure system
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
                m.ContentLabel = "New Packaging Expenditure"
            Else
                m.ContentLabel = "Packaging Expenditure"
            End If

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pProjNo") = Nothing Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='PackagingExpProjList.aspx'><b>Packaging Expenditure Search</b></a> > New Packaging Expenditure"
                Else
                    If ViewState("pAprv") = 0 Then
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='PackagingExpProjList.aspx'><b>Packaging Expenditure Search</b></a> > Packaging Expenditure"
                    Else 'Go Back To approval Screen
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='PackagingExpProjList.aspx'><b>Packaging Expenditure Search</b></a> > <a href='crExpProjPackagingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1'><b>Approval</b></a> > Packaging Expenditure"
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
                    BindData(ViewState("pProjNo"), ViewState("pCO"))
                Else
                    BindData(ViewState("pPrntProjNo"), ViewState("pCO"))
                    txtProjectTitle.Focus()
                    txtDateSubmitted.Text = Date.Today
                End If

                If ViewState("pPCID") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True
                ElseIf ViewState("pCV") > 0 Then
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

            Dim strPreviewClientScript As String = "javascript:void(window.open('crViewExpProjPackaging.aspx?pProjNo=" & ViewState("pProjNo") & "'," & Now.Ticks.ToString & ",'width=950px,height=550px,top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
            btnPreview.Attributes.Add("onclick", strPreviewClientScript)

            Dim strPreviewClientScript2 As String = "javascript:void(window.open('../DataMaintenance/ProgramDisplay.aspx?pPlatID=0&pPgmID=" & ddProgram.SelectedValue & " '," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=yes,location=yes'));"
            iBtnPreviewDetail.Attributes.Add("Onclick", strPreviewClientScript2)

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
            btnReset1.Enabled = False
            btnReset2.Enabled = False
            btnReset3.Enabled = False
            btnReset4.Enabled = False
            btnReset5.Enabled = False
            btnReset6.Enabled = True
            btnUpload.Enabled = False
            btnDelete.Enabled = False
            btnPreview.Enabled = False
            btnAppend.Enabled = False
            btnFwdApproval.Enabled = False
            btnBuildApproval.Enabled = False
            btnBuildApproval.Visible = False
            ddProjectStatus.Enabled = False
            btnAddtoGrid1.Enabled = False
            btnAddtoGrid2.Enabled = False
            uploadFile.Enabled = False
            mnuTabs.Items(0).Enabled = True
            mnuTabs.Items(1).Enabled = False
            mnuTabs.Items(2).Enabled = False
            mnuTabs.Items(3).Enabled = False
            mnuTabs.Items(4).Enabled = False

            gvCustomer.Columns(9).Visible = False
            gvExpense.Columns(12).Visible = False
            gvSupportingDocument.Columns(4).Visible = False
            gvApprovers.Columns(7).Visible = False
            gvApprovers.Columns(8).Visible = False
            gvQuestion.Columns(0).Visible = False
            txtActualCost.Visible = False
            txtCustomerCost.Visible = False
            txtClosingNotes.Visible = False
            txtVoidReason.Visible = False
            txtActualCost.Enabled = False
            txtCustomerCost.Enabled = False
            txtClosingNotes.Enabled = False
            txtVoidReason.Enabled = False

            txtAmtToBeRecovered.Enabled = True
            txtHDAmtToBeRecovered.Enabled = True
            txtNextAmtToBeRecovered.Enabled = False

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
            TCExtender.Collapsed = False
            EXExtender.Collapsed = False
            SDExtender.Collapsed = False
            rfvClostingNotes.Enabled = False
            rfvActualCost.Enabled = False
            rfvCustomerCost.Enabled = False
            rfvAmtToBeRecovered.Enabled = False
            rfvNextAmtToBeRecovered.Enabled = False
            rfvEstCmpltDt.Enabled = False
            rfvNextEstCmpltDt.Enabled = False
            rfvAmtToBeRecoveredChngRsn.Enabled = False
            rfvEstCmpltDt.Enabled = False
            rfvNextEstCmpltDt.Enabled = False
            rfvEstCmpltDtChngRsn.Enabled = False

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

            If ProjectStatus = "Capitalized" Then
                lblReqActualCost.Visible = True
                lblActualCost.Visible = True
                txtActualCost.Visible = True
                txtActualCost.Enabled = True
                rfvActualCost.Enabled = True

                lblReqCustomerCost.Visible = True
                txtCustomerCost.Visible = True
                txtCustomerCost.Enabled = True
                lblCustomerCost.Visible = True
                rfvCustomerCost.Enabled = True

                lblReqClosingNts.Visible = True
                lblClosingNts.Visible = True
                txtClosingNotes.Enabled = True
                txtClosingNotes.Visible = True
                rfvClostingNotes.Enabled = True
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
                If CType(txtHDEstCmpltDt.Text, Date) <> CType(txtNextEstCmpltDt.Text, Date) And ProjectStatus <> "Open" Then
                    rfvEstCmpltDtChngRsn.Enabled = True
                End If
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 123 'Packaging Equipment Form ID
            Dim iRoleID As Integer = 0
            Dim i As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

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
                                            btnSave1.Enabled = True
                                            btnReset1.Enabled = True
                                            btnPreview.Enabled = True
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
                                                    btnDelete.Enabled = True
                                                ElseIf ProjectStatus = "In Process" And (txtRoutingStatus.Text = "R") Then
                                                    ''Build approval during first initial save.
                                                    btnBuildApproval.Enabled = True
                                                    btnBuildApproval.Visible = True
                                                    btnFwdApproval.Enabled = True
                                                ElseIf (txtRoutingStatus.Text <> "N") And (txtRoutingStatus.Text <> "R") And (txtRoutingStatus.Text <> "T") Then
                                                    btnAppend.Enabled = True
                                                End If
                                                btnSave2.Enabled = True
                                                btnSave3.Enabled = True
                                                btnReset2.Enabled = True
                                                btnReset3.Enabled = True
                                                btnReset4.Enabled = True
                                                btnReset5.Enabled = True
                                                btnReset6.Enabled = True
                                                btnUpload.Enabled = True
                                                btnAddtoGrid1.Enabled = True
                                                btnAddtoGrid2.Enabled = True
                                                gvCustomer.Columns(9).Visible = True
                                                gvExpense.Columns(12).Visible = True
                                                gvSupportingDocument.Columns(4).Visible = True
                                                uploadFile.Enabled = True
                                                If (txtRoutingStatus.Text = "T" Or txtRoutingStatus.Text = "R") Then
                                                    gvApprovers.Columns(7).Visible = True
                                                    gvApprovers.Columns(8).Visible = True
                                                End If
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                mnuTabs.Items(4).Enabled = True
                                                txtNextEstCmpltDt.Enabled = True
                                                txtNextAmtToBeRecovered.Enabled = True
                                                If (txtRoutingStatus.Text <> "C") And (ProjectStatus = "Capitalized") Then
                                                    txtActualCost.Enabled = True
                                                    txtCustomerCost.Enabled = True
                                                    txtClosingNotes.Enabled = True
                                                End If
                                                gvQuestion.Columns(0).Visible = True
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
                                                btnSave1.Enabled = True
                                                btnReset1.Enabled = True
                                            Else
                                                ViewState("Admin") = True
                                                Select Case ProjectStatus
                                                    Case "Open"
                                                        If (txtRoutingStatus.Text = "N") Then
                                                            btnFwdApproval.Enabled = True
                                                            btnDelete.Enabled = True
                                                        End If
                                                        ddProjectStatus.Enabled = False
                                                        txtNextEstCmpltDt.Enabled = True
                                                        txtNextAmtToBeRecovered.Enabled = True
                                                        btnAddtoGrid1.Enabled = True
                                                        gvCustomer.Columns(9).Visible = True
                                                        gvSupportingDocument.Columns(4).Visible = True
                                                        btnAddtoGrid2.Enabled = True
                                                        gvExpense.Columns(12).Visible = True
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        btnSave3.Enabled = True
                                                        btnReset2.Enabled = True
                                                        btnReset3.Enabled = True
                                                        btnReset4.Enabled = True
                                                        btnReset5.Enabled = True
                                                        btnReset6.Enabled = True
                                                        btnUpload.Enabled = True
                                                        uploadFile.Enabled = True
                                                    Case "In Process"
                                                        txtNextEstCmpltDt.Enabled = True
                                                        txtNextAmtToBeRecovered.Enabled = True
                                                        btnAdd.Enabled = True
                                                        gvSupportingDocument.Columns(4).Visible = True
                                                        If (txtRoutingStatus.Text = "R") Then
                                                            btnFwdApproval.Enabled = True
                                                            btnAddtoGrid1.Enabled = True
                                                            btnReset2.Enabled = True
                                                            btnAddtoGrid2.Enabled = True
                                                            gvCustomer.Columns(9).Visible = True
                                                            gvExpense.Columns(12).Visible = True
                                                            btnSave3.Enabled = True
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            btnReset3.Enabled = True
                                                            btnReset6.Enabled = True
                                                            btnReset4.Enabled = True
                                                            btnUpload.Enabled = True
                                                            uploadFile.Enabled = True
                                                            btnSave2.Enabled = True
                                                            btnReset5.Enabled = True
                                                            gvQuestion.Columns(0).Visible = True
                                                        ElseIf txtRoutingStatus.Text = "T" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            btnSave2.Enabled = True
                                                            btnReset4.Enabled = True
                                                            btnReset6.Enabled = True
                                                            btnUpload.Enabled = True
                                                            uploadFile.Enabled = True
                                                            gvQuestion.Columns(0).Visible = True
                                                        End If
                                                        TCExtender.Collapsed = True
                                                        EXExtender.Collapsed = True
                                                    Case "Approved"
                                                        ddProjectStatus.Enabled = True
                                                        btnAdd.Enabled = True
                                                        txtNextEstCmpltDt.Enabled = True
                                                        txtNextAmtToBeRecovered.Enabled = True
                                                        TCExtender.Collapsed = True
                                                        EXExtender.Collapsed = True
                                                        SDExtender.Collapsed = True
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        btnSave2.Enabled = True
                                                        btnReset6.Enabled = True
                                                    Case "Capitalized"
                                                        btnAdd.Enabled = True
                                                        If txtRoutingStatus.Text <> "C" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            txtActualCost.Enabled = True
                                                            txtCustomerCost.Enabled = True
                                                            txtClosingNotes.Enabled = True
                                                        End If
                                                        TCExtender.Collapsed = True
                                                        EXExtender.Collapsed = True
                                                        SDExtender.Collapsed = True
                                                    Case "Void"
                                                        btnAdd.Enabled = True
                                                        If txtRoutingStatus.Text <> "V" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                        End If
                                                        TCExtender.Collapsed = True
                                                        EXExtender.Collapsed = True
                                                        SDExtender.Collapsed = True
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidRsn.Visible = True
                                                        lblReqVoidRsn.Visible = True
                                                End Select
                                                If (txtRoutingStatus.Text <> "N") And (txtRoutingStatus.Text <> "R") And (txtRoutingStatus.Text <> "T") And (txtRoutingStatus.Text <> "H") Then
                                                    btnAppend.Enabled = True
                                                End If
                                                'ddProjectStatus.Enabled = True
                                                If txtRoutingStatus.Text = "N" And txtRoutingStatus.Text = "H" Then
                                                    btnDelete.Enabled = True
                                                End If
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
                                                txtProjectTitle.Focus()
                                            Else
                                                ViewState("ObjectRole") = False
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                btnPreview.Enabled = True
                                                Select Case ProjectStatus
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "T") Then
                                                            TCExtender.Collapsed = True
                                                            EXExtender.Collapsed = True
                                                            gvApprovers.Columns(7).Visible = True
                                                            btnReset6.Enabled = False
                                                            btnUpload.Enabled = False
                                                            uploadFile.Enabled = False
                                                            gvSupportingDocument.Columns(4).Visible = False
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
                                            TCExtender.Collapsed = True
                                            SDExtender.Collapsed = True
                                            EXExtender.Collapsed = True
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ''Used by the Accounting Group
                                            ViewState("ObjectRole") = False
                                            ViewState("Admin") = True
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
                                                        TCExtender.Collapsed = True
                                                        EXExtender.Collapsed = True
                                                        gvApprovers.Columns(8).Visible = True
                                                    Case "Void"
                                                        If txtRoutingStatus.Text <> "V" Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                        End If
                                                        TCExtender.Collapsed = True
                                                        SDExtender.Collapsed = True
                                                        EXExtender.Collapsed = True
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidRsn.Visible = True
                                                        lblReqVoidRsn.Visible = True
                                                    Case "Approved"
                                                        TCExtender.Collapsed = True
                                                        SDExtender.Collapsed = True
                                                        EXExtender.Collapsed = True
                                                        'txtNextEstCmpltDt.Enabled = True
                                                        'txtNextAmtToBeRecovered.Enabled = True
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        ddProjectStatus2.Enabled = True
                                                    Case "Capitalized"
                                                        TCExtender.Collapsed = True
                                                        SDExtender.Collapsed = True
                                                        EXExtender.Collapsed = True
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
                                                        ddProjectStatus2.Enabled = True
                                                End Select
                                                If (txtRoutingStatus.Text <> "N") And (txtRoutingStatus.Text <> "R") And (txtRoutingStatus.Text <> "T") Then
                                                    btnAppend.Enabled = True
                                                End If
                                                btnAdd.Enabled = True
                                                gvSupportingDocument.Columns(4).Visible = True
                                                uploadFile.Enabled = True
                                                btnReset6.Enabled = True
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
    Public Function DisableFields() As Boolean
        ddProjectLeader.Enabled = False
        ddAccountManager.Enabled = False
        txtDateSubmitted.Enabled = False
        cbUN.Enabled = False
        cbUP.Enabled = False
        cbUR.Enabled = False
        cbUS.Enabled = False
        cbUT.Enabled = False
        cbUW.Enabled = False
        cbOH.Enabled = False
        txtProjDateNotes.Enabled = False
        txtAmtToBeRecovered.Enabled = False
        txtNextAmtToBeRecovered.Enabled = False
        txtNextEstCmpltDt.Enabled = False
        txtEstSpendDt.Enabled = False
        txtEstEndSpendDt.Enabled = False
        CheckRights()
        Return True
    End Function 'EOF DisableFields

    Protected Sub BindCriteria()

        Try
            Dim DefaultTMID As Integer = HttpContext.Current.Session("UserId")

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

            ''bind existing data to drop down Customer control for selection criteria for search
            '(LREY) 01/08/2014
            'ds = commonFunctions.GetOEMSoldToCABBVbyOEMMfg(ddOEM.SelectedValue, ddOEMMfg.SelectedValue)
            'If (ds.Tables.Item(0).Rows.Count > 0) Then
            '    ddCustomer.ClearSelection()
            '    ddCustomer.DataSource = ds
            '    ddCustomer.DataTextField = ds.Tables(0).Columns("ddCustomerDesc").ColumnName.ToString()
            '    ddCustomer.DataValueField = ds.Tables(0).Columns("ddCustomerValue").ColumnName.ToString()
            '    ddCustomer.DataBind()
            '    ddCustomer.Items.Insert(0, "")
            'End If

            ''bind existing data to drop down Project Leader control for selection criteria for search
            'ds = commonFunctions.GetTeamMember("") '**SubscriptionID # is used for Project Leader
            ds = commonFunctions.GetTeamMemberBySubscription(7) '**SubscriptionID 9 is used for Account Manager
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddProjectLeader.DataSource = ds
                ddProjectLeader.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddProjectLeader.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
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

            If ViewState("pProjNo") = Nothing Then
                ddProjectLeader.SelectedValue = ViewState("iTeamMemberID")
                ddAccountManager.SelectedValue = ViewState("iTeamMemberID")
            End If
            ddTeamMember.SelectedValue = DefaultTMID

            ''bind existing data to drop down Vendor Type control for selection criteria for search
            ds = commonFunctions.GetVendorType(False)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddVendorType.DataSource = ds
                ddVendorType.DataTextField = ds.Tables(0).Columns("ddVType").ColumnName.ToString()
                ddVendorType.DataValueField = ds.Tables(0).Columns("VType").ColumnName.ToString()
                ddVendorType.Items.Add("Select a vendor type to filter 'Supplier' drop down list.")
                ddVendorType.DataBind()
                ddVendorType.SelectedIndex = 0
            End If

            ''bind existing data to drop down Vendor control for selection criteria for search
            ds = SUPModule.GetSupplierLookUp("", "", IIf(ddVendorType.SelectedIndex = 0, "", ddVendorType.SelectedValue), "", "", 1)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddVendor.DataSource = ds
                ddVendor.DataTextField = ds.Tables(0).Columns("ddVendorName").ColumnName.ToString()
                ddVendor.DataValueField = ds.Tables(0).Columns("ddVendorNo").ColumnName.ToString()
                ddVendor.DataBind()
                ddVendor.Items.Insert(0, "")
            End If

            ddVendorType.SelectedValue = HttpContext.Current.Request.QueryString("pVTp")
            ddVendor.SelectedValue = HttpContext.Current.Request.QueryString("pVNO")

            ''bind existing data to drop down UGN Location control for selection criteria for search
            ds = commonFunctions.GetUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNLocation.DataSource = ds
                ddUGNLocation.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddUGNLocation.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNLocation.DataBind()
                ddUGNLocation.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Pkg Expense Line # control for selection criteria for search
            ds = EXPModule.GetExpProjPackagingExpenditure(ViewState("pProjNo"), 0)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddLineNo.DataSource = ds
                ddLineNo.DataTextField = ds.Tables(0).Columns("EID").ColumnName.ToString()
                ddLineNo.DataValueField = ds.Tables(0).Columns("EID").ColumnName.ToString()
                ddLineNo.DataBind()
                ddLineNo.Items.Insert(0, "")
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

    Public Sub BindData(ByVal ProjNo As String, ByVal CarriedOver As Boolean)
        Dim ds As DataSet = New DataSet
        Dim ds2 As DataSet = New DataSet

        Try

            If ProjNo <> Nothing Then
                ds = EXPModule.GetExpProjPackaging(ProjNo, "", "", "", 0, "", 0, "", "", "")
                If commonFunctions.CheckDataSet(ds) = True Then
                    If ViewState("pPrntProjNo") = Nothing Then
                        lblProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                        txtRoutingStatus.Text = ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                        If ddProjectStatus2.SelectedValue <> "Void" Then
                            lblRoutingStatusDesc.Text = ds.Tables(0).Rows(0).Item("RoutingStatusDesc").ToString()
                        End If
                        lblRoutingStatusDesc.Visible = True
                    Else
                        If ViewState("pProjNo") = Nothing Then
                            lblPrntProjNo.Text = ProjNo
                            lblPrntAppDate.Text = IIf(ds.Tables(0).Rows(0).Item("OriginalApprovedDt").ToString() = "01/01/1900", "", ds.Tables(0).Rows(0).Item("OriginalApprovedDt").ToString())
                        Else
                            lblProjectID.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                            txtRoutingStatus.Text = ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                            If ddProjectStatus.SelectedValue <> "Void" Then
                                lblRoutingStatusDesc.Text = ds.Tables(0).Rows(0).Item("RoutingStatusDesc").ToString()
                            End If

                            lblRoutingStatusDesc.Visible = True
                            lblPrntProjNo.Text = ds.Tables(0).Rows(0).Item("ParentProjectNo").ToString()
                            lblPrntAppDate.Text = ds.Tables(0).Rows(0).Item("OriginalApprovedDt").ToString()

                        End If
                    End If

                    Select Case txtRoutingStatus.Text
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
                        Case Else
                            ddProjectStatus.SelectedValue = "Open"
                    End Select

                    txtProjectTitle.Text = ds.Tables(0).Rows(0).Item("ProjectTitle").ToString()
                    cbUT.Checked = ds.Tables(0).Rows(0).Item("UT").ToString()
                    cbUN.Checked = ds.Tables(0).Rows(0).Item("UN").ToString()
                    cbUP.Checked = ds.Tables(0).Rows(0).Item("UP").ToString()
                    cbUR.Checked = ds.Tables(0).Rows(0).Item("UR").ToString()
                    cbUS.Checked = ds.Tables(0).Rows(0).Item("US").ToString()
                    cbUW.Checked = ds.Tables(0).Rows(0).Item("UW").ToString()
                    cbOH.Checked = ds.Tables(0).Rows(0).Item("OH").ToString()

                    ddProjectLeader.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectLeaderTMID").ToString()
                    ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AcctMgrTMID").ToString()

                    txtDateSubmitted.Text = ds.Tables(0).Rows(0).Item("DateSubmitted").ToString()
                    txtProjDateNotes.Text = ds.Tables(0).Rows(0).Item("ProjDtNotes").ToString()
                    txtEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDt").ToString()
                    txtHDEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDt").ToString()
                    txtNextEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDt").ToString()
                    txtEstSpendDt.Text = ds.Tables(0).Rows(0).Item("EstSpendDt").ToString()
                    txtEstEndSpendDt.Text = ds.Tables(0).Rows(0).Item("EstEndSpendDt").ToString()
                    txtEstRecoveryDt.Text = ds.Tables(0).Rows(0).Item("EstRecoveryDt").ToString()

                    txtAmtToBeRecovered.Text = Format(ds.Tables(0).Rows(0).Item("AmtToBeRecovered"), "#,##0.00")
                    txtHDAmtToBeRecovered.Text = Format(ds.Tables(0).Rows(0).Item("AmtToBeRecovered"), "#,##0.00")
                    txtNextAmtToBeRecovered.Text = Format(ds.Tables(0).Rows(0).Item("AmtToBeRecovered"), "#,##0.00")
                    txtMPAAmtToBeRecovered.Text = Format(ds.Tables(0).Rows(0).Item("MPA_AmtToBeRecovered"), "#,##0.00")
                    lblMPATotalCost.Text = Format(ds.Tables(0).Rows(0).Item("MPA_TotalCost"), "#,##0.00")

                    txtActualCost.Text = Format(ds.Tables(0).Rows(0).Item("ActualCost"), "#,##0.00")
                    txtCustomerCost.Text = Format(ds.Tables(0).Rows(0).Item("CustomerCost"), "#,##0.00")
                    txtClosingNotes.Text = ds.Tables(0).Rows(0).Item("ClosingNotes").ToString()
                    txtVoidReason.Text = ds.Tables(0).Rows(0).Item("VoidReason").ToString()

                    If ViewState("pEV") = 1 And ViewState("pNF") <> 0 Then
                        rblVendorStatus.SelectedValue = ViewState("pNF")
                    End If

                    cbNotRequired.Checked = ds.Tables(0).Rows(0).Item("NotRequired").ToString()
                    txtDiscountReturn.Text = ds.Tables(0).Rows(0).Item("DiscountReturned").ToString()
                    txtPayback.Text = ds.Tables(0).Rows(0).Item("PaybackInYears").ToString()
                    txtRtnAvgAssets.Text = ds.Tables(0).Rows(0).Item("ReturnAvgAssets").ToString()
                    lblUGNTotalCost.Text = Format(ds.Tables(0).Rows(0).Item("UGNTotalCost"), "#,##0.00")
                    txtHDOrigUGNTotalCost.Text = Format(ds.Tables(0).Rows(0).Item("OrigUGNTotalCost"), "#,##0.00")
                    lblCustTotalCost.Text = Format(ds.Tables(0).Rows(0).Item("CustTotalCost"), "#,##0.00")
                    lblVarTotalCost.Text = Format(ds.Tables(0).Rows(0).Item("VarTotalCost"), "#,##0.00")

                    'Bind Customer
                    If ViewState("pPCID") <> 0 Then
                        Dim ds3 As DataSet = New DataSet
                        ds3 = EXPModule.GetExpProjPackagingCustomer(ViewState("pProjNo"), ViewState("pPCID"))
                        If commonFunctions.CheckDataSet(ds3) = True Then
                            ' ''cddOEMMfg.SelectedValue = ds3.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
                            cddMake.SelectedValue = ds3.Tables(0).Rows(0).Item("Make").ToString()
                            cddModel.SelectedValue = ds3.Tables(0).Rows(0).Item("Model").ToString()
                            cddProgram.SelectedValue = ds3.Tables(0).Rows(0).Item("ProgramID").ToString()
                            ' ''cddOEM.SelectedValue = ds3.Tables(0).Rows(0).Item("OEM").ToString()
                            ' ''ddCustomer.SelectedValue = ds3.Tables(0).Rows(0).Item("ddCustomerValue").ToString()
                            ' ''cddPartNo.SelectedValue = ds3.Tables(0).Rows(0).Item("PartNo").ToString()
                            txtPartNo.Text = ds3.Tables(0).Rows(0).Item("PartNo").ToString()

                            txtRevisionLvl.Text = ds3.Tables(0).Rows(0).Item("RevisionLevel").ToString()
                            txtLeadTime.Text = ds3.Tables(0).Rows(0).Item("LeadTime").ToString()
                            txtSOP.Text = ds3.Tables(0).Rows(0).Item("SOP").ToString()
                            txtEOP.Text = ds3.Tables(0).Rows(0).Item("EOP").ToString()
                            txtPPAPDt.Text = ds3.Tables(0).Rows(0).Item("PPAP").ToString()
                        Else 'no record found reset query string pPCID
                            Response.Redirect("PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pPCID=0", False)
                        End If
                    End If

                    ''Bind Expenses
                    If ViewState("pEID") <> 0 Then
                        Dim ds4 As DataSet = New DataSet
                        ds4 = EXPModule.GetExpProjPackagingExpenditure(ViewState("pProjNo"), ViewState("pEID"))
                        If (ds4.Tables.Item(0).Rows.Count > 0) Then
                            rblVendorStatus.SelectedValue = IIf(ds4.Tables(0).Rows(0).Item("FutureVendor").ToString() = True, 1, 2)
                            ddVendorType.SelectedValue = ds4.Tables(0).Rows(0).Item("VendorType").ToString()
                            ddVendor.SelectedValue = ds4.Tables(0).Rows(0).Item("VendorNo").ToString()
                            txtDescription.Text = ds4.Tables(0).Rows(0).Item("Description").ToString()
                            ddUGNLocation.SelectedValue = ds4.Tables(0).Rows(0).Item("UGNFacility").ToString()
                            txtQuantity.Text = ds4.Tables(0).Rows(0).Item("Quantity").ToString()
                            txtUGNUnitCost.Text = ds4.Tables(0).Rows(0).Item("UGNUnitCost").ToString()
                            txtCustUnitCost.Text = ds4.Tables(0).Rows(0).Item("CustUnitCost").ToString()
                            txtMPATotalCost.Text = ds4.Tables(0).Rows(0).Item("MPATotalCost").ToString()

                            txtNotes.Text = ds4.Tables(0).Rows(0).Item("Notes").ToString()
                        Else 'no record found reset query string pRptID
                            Response.Redirect("PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pEID=0", False)
                        End If
                    End If

                    ''Bind Communication Board
                    If ViewState("pRID") <> 0 Then
                        Dim ds5 As DataSet = New DataSet
                        ds5 = EXPModule.GetPackagingExpProjRSS(ViewState("pProjNo"), ViewState("pRID"))
                        If commonFunctions.CheckDataSet(ds5) = True Then
                            txtQC.Text = ds5.Tables(0).Rows(0).Item("Comments").ToString()
                        Else 'no record found reset query string pRptID
                            Response.Redirect("PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pRID=0&pRC=1", False)
                        End If
                    End If

                End If
            End If
            'End If 'EOF Carryover
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
        Response.Redirect("PackagingExpProj.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnAppend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAppend.Click
        Response.Redirect("PackagingExpProj.aspx?pProjNo=&pPrntProjNo=" & ViewState("pProjNo"), False)
    End Sub 'EOF btnAppend_Click

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click, btnSave3.Click
        Try
            Dim DefaultDate As Date = Date.Today
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            Dim ProjectStatus As String = Nothing
            ProjectStatus = ViewState("ProjectStatus")

            Dim SendEmailToDefaultAdmin As Boolean = False

            If (ViewState("pProjNo") <> Nothing Or ViewState("pProjNo") <> "") Then
                '***************
                '* Update Data
                '***************
                UpdateRecord(ProjectStatus, IIf(ProjectStatus = "Closed", "C", IIf(ProjectStatus = "Void", "V", IIf(ProjectStatus = "Open", "N", IIf(ProjectStatus = "Approved", "A", IIf(ProjectStatus = "In Process", txtRoutingStatus.Text, IIf(ProjectStatus = "Rejected", "R", txtRoutingStatus.Text)))))), False)

                '**************
                '* Reload the data - may contain calculated information to TotalInv
                '**************
                BindData(ViewState("pProjNo"), ViewState("pCO"))

                ''*************
                ''Check for Order Received & Void status, send email notfication 
                ''*************
                If ProjectStatus = "Capitalized" And txtRoutingStatus.Text = "C" Then
                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Capitalized", "", "", "", "")
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Capitalized")
                    End If
                ElseIf ProjectStatus = "Void" And txtRoutingStatus.Text = "V" Then
                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "VOID - Reason:" & txtVoidReason.Text, "", "", "", "")
                    If txtRoutingStatus.Text <> "N" Then
                        SendNotifWhenEventChanges("Void")
                    End If
                End If

            Else 'New Record
                '***************
                '* Locate Next available ProjectNo based on Facility selection
                '***************
                Dim ds As DataSet = Nothing
                ds = EXPModule.GetNextExpProjectNo(ViewState("pPrntProjNo"), "", "Packaging")

                ViewState("pProjNo") = CType(ds.Tables(0).Rows(0).Item("NextAvailProjNo").ToString, String)

                '***************
                '* Save Data
                '***************
                EXPModule.InsertExpProjPackaging(ViewState("pProjNo"), ViewState("pPrntProjNo"), txtProjectTitle.Text, "Open", "N", ddProjectLeader.SelectedValue, ddAccountManager.SelectedValue, txtDateSubmitted.Text, cbUT.Checked, cbUN.Checked, cbUP.Checked, cbUR.Checked, cbUS.Checked, cbUW.Checked, cbOH.Checked, txtProjDateNotes.Text, txtEstCmpltDt.Text, txtEstSpendDt.Text, txtEstEndSpendDt.Text, IIf(txtAmtToBeRecovered.Text = "", 0, txtAmtToBeRecovered.Text), lblPrntAppDate.Text, False, 0, txtMPAAmtToBeRecovered.Text, txtEstRecoveryDt.Text, DefaultUser, DefaultDate)

                ''************************
                '' Prebuild Approval List
                ''************************
                BuildApprovalList()

                ''*****************
                ''History Tracking
                ''*****************
                EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Record created.", "", "", "", "")

                '***************
                '* Redirect user back to the page.
                '***************
                Dim Aprv As String = Nothing
                If ViewState("pAprv") = 1 Then
                    Aprv = "&pAprv=1"
                End If

                Response.Redirect("PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pPrntProjNo=" & ViewState("pPrntProjNo") & Aprv, False)

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
            If txtHDEstCmpltDt.Text <> "" Then
                If CType(txtHDEstCmpltDt.Text, Date) <> CType(txtNextEstCmpltDt.Text, Date) Then
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
            End If

            EXPModule.UpdateExpProjPackaging(ViewState("pProjNo"), txtProjectTitle.Text, ProjectStatus, RoutingStatus, ddProjectLeader.SelectedValue, ddAccountManager.SelectedValue, txtDateSubmitted.Text, cbUT.Checked, cbUN.Checked, cbUP.Checked, cbUR.Checked, cbUS.Checked, cbUW.Checked, cbOH.Checked, txtProjDateNotes.Text, EstCmpltDt, txtEstSpendDt.Text, txtEstEndSpendDt.Text, AmtToBeRecovered, IIf(txtActualCost.Text = "", 0, txtActualCost.Text), IIf(txtCustomerCost.Text = "", 0, txtCustomerCost.Text), txtClosingNotes.Text, txtVoidReason.Text, cbNotRequired.Checked, IIf(txtDiscountReturn.Text = "", 0, txtDiscountReturn.Text), IIf(txtPayback.Text = "", 0, txtPayback.Text.Trim), IIf(txtRtnAvgAssets.Text = "", 0, txtRtnAvgAssets.Text), 0, txtMPAAmtToBeRecovered.Text, txtEstRecoveryDt.Text, DefaultUser, DefaultDate)

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
        Dim parmVal As String = Nothing
        If mnuTabs.Items(1).Selected = True Then
            parmVal = "&pCV=1"
        End If
        If mnuTabs.Items(2).Selected = True Then
            parmVal = "&pEV=1"
        End If
        If mnuTabs.Items(4).Selected = True Then
            parmVal = "&&pRC=1"
        End If


        'Redirect user to the same tab view when Reset button was clicked
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If
        If ViewState("pProjNo") <> "" Then
            If ViewState("pPCID") > 0 Then
                Response.Redirect("PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pCV=1" & Aprv, False)
            ElseIf ViewState("pEID") > 0 Then
                Response.Redirect("PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pEV=1" & Aprv, False)
            ElseIf ViewState("pRID") > 0 Then
                Response.Redirect("PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pRC=1" & Aprv, False)
            ElseIf ViewState("pSD") > 0 Then
                Response.Redirect("PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pSD=1" & Aprv, False)
            Else
                Response.Redirect("PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & Aprv & parmVal, False)
            End If
        End If
    End Sub 'EOF btnReset1_Click

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            '***************
            '* Delete Record
            '***************
            If ViewState("pPrntProjNo") = Nothing Then
                EXPModule.DeleteExpProjPackaging(ViewState("pProjNo"), ViewState("pPrntProjNo"), False)
            Else
                EXPModule.DeleteExpProjPackaging(ViewState("pProjNo"), ViewState("pPrntProjNo"), True)
            End If

            '***************
            '* Redirect user back to the search page.
            '***************
            Response.Redirect("PackagingExpProjList.aspx", False)

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

    Protected Sub ddProjectStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProjectStatus.SelectedIndexChanged, ddProjectStatus2.SelectedIndexChanged

        Select Case ViewState("ProjectStatus")
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
                rfvActualCost.Enabled = True
                rfvCustomerCost.Enabled = True
                rfvClostingNotes.Enabled = True
                txtActualCost.Enabled = True
                txtCustomerCost.Enabled = True
                txtClosingNotes.Enabled = True
                txtVoidReason.Visible = False
                lblReqVoidRsn.Visible = False
                lblVoidRsn.Visible = False
                DisableFields()
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
                rfvActualCost.Enabled = False
                rfvCustomerCost.Enabled = False
                rfvClostingNotes.Enabled = False
                txtVoidReason.Visible = True
                txtVoidReason.Enabled = True
                lblReqVoidRsn.Visible = True
                lblVoidRsn.Visible = True
                DisableFields()
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
            Dim NextEstCmpltDt As String = Nothing
            If txtNextEstCmpltDt.Text <> Nothing Then
                NextEstCmpltDt = CType(txtNextEstCmpltDt.Text, Date)
            End If
            Dim HDEstCmpltDt As String = Nothing
            If txtHDEstCmpltDt.Text <> Nothing Then
                HDEstCmpltDt = CType(txtHDEstCmpltDt.Text, Date)
            End If

            If HDEstCmpltDt <> NextEstCmpltDt And (ViewState("ProjectStatus") <> "Open") Then
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
    ' ''Protected Sub ddOEM_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOEM.SelectedIndexChanged
    ' ''    '(LREY) 01/08/2014
    ' ''    'Dim ds As DataSet = New DataSet
    ' ''    'ds = commonFunctions.GetOEMSoldToCABBVbyOEMMfg(ddOEM.SelectedValue, ddOEMMfg.SelectedValue)
    ' ''    'If (ds.Tables.Item(0).Rows.Count > 0) And ddCustomer.SelectedValue = Nothing Then
    ' ''    '    ddCustomer.ClearSelection()
    ' ''    '    ddCustomer.DataSource = ds
    ' ''    '    ddCustomer.DataTextField = ds.Tables(0).Columns("ddCustomerDesc").ColumnName.ToString()
    ' ''    '    ddCustomer.DataValueField = ds.Tables(0).Columns("ddCustomerValue").ColumnName.ToString()
    ' ''    '    ddCustomer.DataBind()
    ' ''    '    ddCustomer.Items.Insert(0, "")
    ' ''    'End If
    ' ''End Sub 'EOF ddOEM_SelectedIndexChanged

    Protected Sub ddProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProgram.SelectedIndexChanged
        If ddProgram.SelectedValue <> Nothing Then
            ''System.Threading.Thread.Sleep(3000)

            Dim ds As DataSet = New DataSet
            ds = commonFunctions.GetPlatformProgram(0, ddProgram.SelectedValue, "", "", ddMake.SelectedValue)
            If commonFunctions.CheckDataSet(ds) = True Then
                Dim NoOfDays As String = Nothing
                Select Case ds.Tables(0).Rows(0).Item("EOPMM").ToString()
                    Case "01"
                        NoOfDays = "31"
                    Case "02"
                        NoOfDays = "28"
                    Case "03"
                        NoOfDays = "31"
                    Case "04"
                        NoOfDays = "30"
                    Case "05"
                        NoOfDays = "31"
                    Case "06"
                        NoOfDays = "30"
                    Case "07"
                        NoOfDays = "31"
                    Case "08"
                        NoOfDays = "31"
                    Case "09"
                        NoOfDays = "30"
                    Case 10
                        NoOfDays = "31"
                    Case 11
                        NoOfDays = "30"
                    Case 12
                        NoOfDays = "31"
                End Select
                If ds.Tables(0).Rows(0).Item("EOPMM").ToString() <> "" Then
                    txtEOP.Text = ds.Tables(0).Rows(0).Item("EOPMM").ToString() & "/" & NoOfDays & "/" & ds.Tables(0).Rows(0).Item("EOPYY").ToString()
                End If
                If ds.Tables(0).Rows(0).Item("SOPMM").ToString() <> "" Then
                    txtSOP.Text = ds.Tables(0).Rows(0).Item("SOPMM").ToString() & "/01/" & ds.Tables(0).Rows(0).Item("SOPYY").ToString()
                End If
                'cddOEMMfg.SelectedValue = ds.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
                iBtnPreviewDetail.Visible = True
            Else
                iBtnPreviewDetail.Visible = False
            End If
        End If 'EOF ddProgram.SelectedValue

    End Sub 'EOF ddProgram_SelectedIndexChanged

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
                    Dim price As ExpProjPackaging.ExpProj_Packaging_CustomerRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProjPackaging.ExpProj_Packaging_CustomerRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Customer (" & DataBinder.Eval(e.Row.DataItem, "ddCustomerDesc") & "); Program (" & DataBinder.Eval(e.Row.DataItem, "ProgramName") & "); Part No. (" & DataBinder.Eval(e.Row.DataItem, "PartNo") & ")?');")
                End If
            End If
        End If
    End Sub 'EOF gvCustomer_RowDataBound

    Public Function GoToDetailPP(ByVal ProgramID As Integer) As String

        If ProgramID <> Nothing Then
            Dim ds As DataSet = New DataSet
            ds = commonFunctions.GetPlatformProgram(0, ProgramID, "", "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                Dim PlatformID As Integer = ds.Tables(0).Rows(0).Item("PlatformID").ToString()
                Return "~/DataMaintenance/ProgramDisplay.aspx?pPlatID=" & PlatformID & "&pPgmID=" & ProgramID
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function 'EOF GoToDetailPP

    Protected Sub btnAddtoGrid1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddtoGrid1.Click
        ''This function is used to save/update Customer Info.
        lblMessageView2.Text = Nothing
        lblMessageView2.Visible = False

        Try
            If ViewState("pProjNo") <> Nothing Then
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

                ''**********************************************
                ''Kick out if there is an Obsolete selection
                ''**********************************************
                If InStr(ddProgram.SelectedItem.Text, "**") Then
                    lblMessageView2.Text = "Invalid Program Selection. System does not allow obsoleted items."
                    lblMessageView2.Visible = True
                    Exit Sub
                End If

                'If InStr(ddCustomer.SelectedItem.Text, "**") Then
                '    lblMessageView2.Text = "Invalid Customer Selection. System does not allow obsoleted items."
                '    lblMessageView2.Visible = True
                '    Exit Sub
                'End If

                'If InStr(ddPartNo.SelectedItem.Text, "**") Then
                '    lblMessageView2.Text = "Invalid Part Number Selection. System does not allow obsoleted items."
                '    lblMessageView2.Visible = True
                '    Exit Sub
                'End If

                If txtPartNo.Text = Nothing Then
                    lblMessageView2.Text = "Part Number was not captured during save. Please re-enter."
                    lblMessageView2.Visible = True
                    Exit Sub
                End If

                ''************************************************************
                ''Kick out if the Current Year is greater than dates selected
                ''************************************************************
                If txtEOP.Text <= Today Then
                    lblMessageView2.Text = "Program EOP date must be greater than current date. Choose a different Program."
                    lblMessageView2.Visible = True
                    Exit Sub
                End If


                ''***************************************************
                ''Get Part Description
                ''***************************************************
                Dim PartDesc As String = Nothing
                Dim DType As String = Nothing
                Dim ds As DataSet = New DataSet
                ds = commonFunctions.GetBPCSPartNo(txtPartNo.Text, "")
                If commonFunctions.CheckDataSet(ds) = True Then
                    PartDesc = ds.Tables(0).Rows(0).Item("BPCSPartName").ToString()
                    DType = ds.Tables(0).Rows(0).Item("DesignationType").ToString()
                End If


                '*************************************************************
                '* Locate the position of the CABBV and SoldTo from ddCustomer
                '*************************************************************
                ''Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
                Dim tempCABBV As String = ""
                Dim tempSoldTo As Integer = 0
                ' ''If Not (Pos = 0) Then
                ' ''    tempCABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                ' ''    tempSoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
                ' ''End If
                ' ''If tempCABBV = Nothing Then
                ' ''    tempCABBV = "N/A"
                ' ''End If

                If ViewState("pPCID") = 0 Or ViewState("pPCID") = Nothing Then
                    '********************************************
                    '* Insert Customer Part information to table
                    '********************************************
                    EXPModule.InsertExpProjPackagingCustomer(ViewState("pProjNo"), tempCABBV, tempSoldTo, txtPartNo.Text, IIf(ddProgram.SelectedValue = "", 0, ddProgram.SelectedValue), "", txtRevisionLvl.Text, txtLeadTime.Text, txtSOP.Text, txtEOP.Text, txtPPAPDt.Text, PartDesc, DType, DefaultUser)

                Else
                    '***************
                    '* Update Customer Part information to table
                    '***************
                    EXPModule.UpdateExpProjPackagingCustomer(ViewState("pPCID"), ViewState("pProjNo"), tempCABBV, tempSoldTo, txtPartNo.Text, IIf(ddProgram.SelectedValue = "", 0, ddProgram.SelectedValue), "", txtRevisionLvl.Text, txtLeadTime.Text, txtSOP.Text, txtEOP.Text, txtPPAPDt.Text, PartDesc, DType, DefaultUser)

                End If

                ' ''cddPartNo.SelectedValue = Nothing
                gvCustomer.DataBind()

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

#Region "Packaging Expense"
    Protected Sub ddVendorType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddVendorType.SelectedIndexChanged
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Department or Cost Center control for selection criteria for search
            ds = commonFunctions.GetVendor(0, "", "", "", "", "", "", "", ddVendorType.SelectedValue)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddVendor.DataSource = ds
                ddVendor.DataTextField = ds.Tables(0).Columns("ddVNDNAMcombo").ColumnName.ToString()
                ddVendor.DataValueField = ds.Tables(0).Columns("Vendor").ColumnName.ToString()
                ddVendor.DataBind()
                ddVendor.Items.Insert(0, "")
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
    End Sub 'EOF ddVendorType_SelectedIndexChanged

    Protected Sub rblVendorStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblVendorStatus.SelectedIndexChanged

        'Form Name EXPKG = CapEx: Packaging Expense
        ViewState("pFVTNo") = False
        Response.Redirect("~\SUP\SupplierLookUp.aspx?sBtnSrch=False&pForm=EXPKG&pProjNo=" & ViewState("pProjNo"), False)

    End Sub 'EOF rblVendorStatus_SelectedIndexChanged

    Protected Sub gvExpense_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExpense.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(13).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As ExpProjPackaging.ExpProj_Packaging_ExpenditureRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProjPackaging.ExpProj_Packaging_ExpenditureRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record """ & DataBinder.Eval(e.Row.DataItem, "Description") & """?');")
                End If
            End If
        End If
    End Sub 'EOF gvExpense_RowDataBound

    Protected Sub btnAddtoGrid2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddtoGrid2.Click
        Try
            If ViewState("pProjNo") <> Nothing Then
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                Dim FutureVendor As Boolean = False

                If rblVendorStatus.SelectedValue = "1" Then
                    FutureVendor = True
                End If

                If ViewState("pEID") = 0 Or ViewState("pEID") = Nothing Then
                    '***************
                    '* Insert Expense information to table
                    '***************
                    EXPModule.InsertExpProjPackagingExpenditure(ViewState("pProjNo"), FutureVendor, ddVendorType.SelectedValue, ddVendor.SelectedValue, txtDescription.Text, ddUGNLocation.SelectedValue, 0, IIf(txtQuantity.Text = "", 0, txtQuantity.Text), IIf(txtUGNUnitCost.Text = "", 0, txtUGNUnitCost.Text), IIf(txtCustUnitCost.Text = "", 0, txtCustUnitCost.Text), txtNotes.Text, IIf(txtMPATotalCost.Text = "", 0, txtMPATotalCost.Text), DefaultUser)

                    rblVendorStatus.SelectedValue = Nothing
                    txtDescription.Text = Nothing
                    ddVendorType.SelectedValue = Nothing
                    ddVendor.SelectedValue = Nothing
                    ddUGNLocation.SelectedValue = Nothing
                    txtQuantity.Text = Nothing
                    txtUGNUnitCost.Text = Nothing
                    txtCustUnitCost.Text = Nothing
                    txtNotes.Text = Nothing
                    ViewState("pNF") = 0
                Else
                    '***************
                    '* Update Expense information to table
                    '***************
                    EXPModule.UpdateExpProjPackagingExpenditure(ViewState("pEID"), ViewState("pProjNo"), txtDescription.Text, ddVendorType.SelectedValue, ddVendor.SelectedValue, FutureVendor, ddUGNLocation.SelectedValue, 0, IIf(txtQuantity.Text = "", 0, txtQuantity.Text), IIf(txtUGNUnitCost.Text = "", 0, txtUGNUnitCost.Text), IIf(txtCustUnitCost.Text = "", 0, txtCustUnitCost.Text), txtNotes.Text, IIf(txtMPATotalCost.Text = "", 0, txtMPATotalCost.Text), DefaultUser)

                End If
                Response.Redirect("PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pEV=1", False)

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

#End Region 'EOF Packaging Expense

#Region "Communication Board"
    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim RSSID As Integer
            Dim drRSSID As ExpProjPackaging.ExpProj_Packaging_RSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, ExpProjPackaging.ExpProj_Packaging_RSSRow)

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

    Public Function GoToCommunicationBoard(ByVal ProjectNo As String, ByVal RSSID As String, ByVal ApprovalLevel As Integer, ByVal TeamMemberID As Integer) As String
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If
        Return "PackagingExpProj.aspx?pProjNo=" & ProjectNo & "&pAL=" & ApprovalLevel & "&pTMID=" & TeamMemberID & "&pRID=" & RSSID & "&pRC=1" & Aprv
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
                ds = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), 0, TMID, False, False)
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
                    ''Carbon Copy Project Lead & Account Manager
                    ''********************************
                    EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)


                    ''***************************************************************
                    ''Carbon Copy Previous Levels
                    ''***************************************************************
                    EmailCC = CarbonCopyList(MyMessage, 0, "", SeqNo, TMID, EmailCC, DefaultTMID)


                    ''Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                    Else
                        MyMessage.Subject = ""
                        MyMessage.Body = ""
                    End If

                    MyMessage.Subject &= "Packaging Expenditure: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text & " - MESSAGE RECIEVED"

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
                    MyMessage.Body &= "         <p><a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crExpProjPackagingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to return to the approval page."
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
                    EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Message Sent", "", "", "", "")

                    ''**********************************
                    ''Save Reponse to child table
                    ''**********************************
                    EXPModule.InsertExpProjPackagingRSSReply(ViewState("pProjNo"), ViewState("pRID"), txtProjectTitle.Text, DefaultTMID, txtReply.Text)

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (P)", ViewState("pProjNo"))
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

                    gvQuestion.DataBind()
                    mvTabs.ActiveViewIndex = Int32.Parse(4)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(4).Selected = True

                Else 'EmailTO = ''
                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pProjNo"), ViewState("pCO"))

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

#Region "Supporting Documents"
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim DefaultDate As Date = Date.Today
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
                                EXPModule.InsertExpProjPackagingDocuments(ViewState("pProjNo"), ddTeamMember.SelectedValue, txtFileDesc.Text, SupportingDocBinaryFile, uploadFile.FileName, SupportingDocEncodeType, SupportingDocFileSize, IIf(ddLineNo.SelectedValue = "", 0, ddLineNo.SelectedValue), 0, "", "")
                            End If

                            gvSupportingDocument.DataBind()
                            revUploadFile.Enabled = False
                            txtFileDesc.Text = Nothing
                            ddLineNo.SelectedValue = Nothing
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
                Dim db As ImageButton = CType(e.Row.Cells(4).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As ExpProjPackaging.ExpProj_Packaging_DocumentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProjPackaging.ExpProj_Packaging_DocumentsRow)

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
            Response.Redirect("PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pSD=1" & Aprv, False)
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

#End Region 'EOF Supporting Documents

#Region "Approval Status"
    Protected Sub gvApprovers_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)

        Try
            Dim row As GridViewRow = gvApprovers.Rows(e.RowIndex)
            If row IsNot Nothing Then
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

                lblReqAppComments.Visible = False
                lblReqAppComments.Text = Nothing
                lblErrors.Text = Nothing
                lblErrors.Visible = False

                If (t.Text <> "Pending") Then
                    If (c.Text <> Nothing Or c.Text <> "") Then
                        ds = SecurityModule.GetTeamMember(OrigTeamMemberID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                        Dim ShortName As String = ds.Tables(0).Rows(0).Item("ShortName").ToString()

                        ''*****************
                        ''History Tracking
                        ''*****************
                        EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, (DefaultUser.ToUpper) & " " & t.Text & " this record on behalf of " & ShortName.ToUpper & " - Comments: " & c.Text, "", "", "", "")

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
                        Dim ProjectStatus As String = Nothing

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
                            Else  'BUILD EMAIL

                                ''*****************
                                ''Declare Variables
                                ''*****************
                                Dim ProjectLead As Integer = ddProjectLeader.SelectedValue
                                Dim SeqNo As Integer = 0
                                Dim NextSeqNo As Integer = 0
                                Dim NextLvl As Integer = 0

                                Select Case hfSeqNo
                                    Case 1
                                        SeqNo = 1
                                        NextSeqNo = 2
                                        NextLvl = 14
                                    Case 2
                                        SeqNo = 2
                                        NextSeqNo = 3
                                        NextLvl = 15
                                    Case 3
                                        SeqNo = 3
                                        NextSeqNo = 0
                                        NextLvl = 0
                                    Case 2
                                        SeqNo = 2
                                        NextSeqNo = 0
                                        NextLvl = 0
                                End Select

                                If SeqNo = 3 Then
                                    NextLvl = 15
                                End If

                                If t.SelectedValue = "Approved" And SeqNo = 3 Then
                                    ProjectStatus = "Approved"
                                Else
                                    ProjectStatus = "In Process"
                                End If

                                ''**********************
                                ''* Save data prior to submission before approvals
                                ''**********************
                                UpdateRecord(ProjectStatus, IIf(SeqNo = 3, IIf(t.SelectedValue = "Rejected", "R", "A"), IIf(t.SelectedValue = "Rejected", "R", "T")), False)

                                ''***********************************
                                ''Update Current Level Approver record.
                                ''***********************************
                                EXPModule.UpdateExpProjPackagingApproval(ViewState("pProjNo"), DefaultTMID, True, t.SelectedValue, c.Text, SeqNo, DefaultUser, DefaultDate)

                                ''*******************************
                                ''Locate Next Approver
                                ''*******************************
                                ''Check at same sequence level
                                ds1st = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), SeqNo, TeamMemberID, True, False)
                                If commonFunctions.CheckDataSet(ds1st) = False Then
                                    If t.SelectedValue <> "Rejected" Then
                                        ds2nd = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), IIf(SeqNo < 3, (SeqNo + 1), SeqNo), 0, True, False)
                                        If commonFunctions.CheckDataSet(ds2nd) = True Then
                                            For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                                If (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                                                (ddProjectLeader.SelectedValue <> ds2nd.Tables(0).Rows(i).Item("TeamMemberID")) Then
                                                    EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                                    EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                                    ''*****************************************
                                                    ''Update Approvers DateNotified field.
                                                    ''*****************************************
                                                    EXPModule.UpdateExpProjPackagingApproval(ViewState("pProjNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", IIf(SeqNo < 3, (SeqNo + 1), SeqNo), DefaultUser, DefaultDate)
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
                                    dsRej = EXPModule.GetExpProjPackagingLead(ViewState("pProjNo"))
                                    ''Check that the recipient(s) is a valid Team Member
                                    If commonFunctions.CheckDataSet(dsRej) = True Then
                                        For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                            If (dsRej.Tables(0).Rows(i).Item("TMDesc") = "Project Leader") And _
                                            (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True)  Then
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
                                    If (SeqNo > 1 And t.SelectedValue <> "Rejected") Then
                                        ''**************************************************************
                                        ''Carbon Copy Previous Level Approvers - 1 down
                                        ''**************************************************************
                                        EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), "", 0, 0, EmailCC, DefaultTMID)

                                        If SeqNo = 3 Then
                                            ''**************************************************************
                                            ''Carbon Copy 1 Level Approvers
                                            ''**************************************************************
                                            EmailCC = CarbonCopyList(MyMessage, (NextLvl - 2), "", 0, 0, EmailCC, DefaultTMID)

                                            ''**************************************************************
                                            ''Carbon Copy Last Level Approvers
                                            ''**************************************************************
                                            EmailCC = CarbonCopyList(MyMessage, NextLvl, "", 0, 0, EmailCC, DefaultTMID)
                                        End If
                                    Else
                                        EmailCC = CarbonCopyList(MyMessage, 0, "", SeqNo, 0, EmailCC, DefaultTMID)
                                    End If

                                    If (SeqNo < 3 And t.SelectedValue <> "Rejected") Then
                                        ''********************************
                                        ''Carbon Copy Project Lead & Account Manager
                                        ''********************************
                                        EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                                    ElseIf (SeqNo = 3 And t.SelectedValue <> "Rejected") Then
                                        ''**************************************
                                        ''*Carbon Copy the Accounting Department
                                        ''**************************************
                                        EmailCC = CarbonCopyList(MyMessage, 121, "", 0, 0, EmailCC, DefaultTMID)

                                        ''********************************
                                        ''Carbon Copy Materials Managers
                                        ''********************************
                                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUN.Checked = True, "UN", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUP.Checked = True, "UP", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUR.Checked = True, "UR", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUS.Checked = True, "US", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUT.Checked = True, "UT", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUW.Checked = True, "UW", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbOH.Checked = True, "OH", ""), 0, 0, EmailCC, DefaultTMID)

                                        ''********************************
                                        ''Carbon Copy Ops Manager
                                        ''********************************
                                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUN.Checked = True, "UN", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUP.Checked = True, "UP", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUR.Checked = True, "UR", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUS.Checked = True, "US", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUT.Checked = True, "UT", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUW.Checked = True, "UW", ""), 0, 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbOH.Checked = True, "OH", ""), 0, 0, EmailCC, DefaultTMID)
                                    End If 'EOF of Carbon Copies

                                    MyMessage.Subject = "Packaging Expenditure: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text

                                    MyMessage.Body = "<font size='2' face='Tahoma'>"
                                    If t.SelectedValue = "Rejected" Then
                                        MyMessage.Subject &= " - REJECTED"
                                        MyMessage.Body &= EmpName
                                        MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' was <font color='red'>REJECTED</font>.  "
                                        MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "'>Click here</a> to access the record.<br/><br/>"
                                        MyMessage.Body &= "<i>Reason for rejection:</i> <b>" & c.Text & "</b></p>"
                                    Else
                                        If SeqNo = 3 Then
                                            MyMessage.Subject &= "- APPROVED"
                                            MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' is Approved. "
                                            MyMessage.Body &= " <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "'>Click here</a> to access the record.</p>"
                                        Else

                                            MyMessage.Body &= EmpName
                                            MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' is available for your Review/Approval. "
                                            MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crExpProjPackagingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"

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
                                        EmailTO = "lynette.rey@ugnauto.com;"
                                        EmailCC = "lynette.rey@ugnauto.com"
                                    End If

                                    ''**********************************
                                    ''Connect & Send email notification
                                    ''**********************************
                                    Try
                                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (P)", ViewState("pProjNo"))
                                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                        lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."
                                    Catch ex As Exception
                                        lblErrors.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                                        lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                                        UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, CurrentEmpEmail & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                        'get current event name
                                        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                                        'log and email error
                                        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                                    End Try
                                    lblErrors.Visible = True
                                    lblErrors.Font.Size = 12
                                    MaintainScrollPositionOnPostBack = False

                                    lblReqAppComments.Visible = True
                                    lblReqAppComments.ForeColor = Color.Red

                                    ''*****************
                                    ''History Tracking
                                    ''*****************
                                    If t.SelectedValue <> "Rejected" Then
                                        If SeqNo = 3 Then
                                            EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to all involved.", "", "", "", "")
                                        Else
                                            EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to level " & IIf(SeqNo < 3, (SeqNo + 1), SeqNo) & " approver(s): " & EmpName, "", "", "", "")
                                        End If
                                    Else
                                        EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Notification sent to " & EmpName, "", "", "", "")
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
                BindData(ViewState("pProjNo"), ViewState("pCO"))
                gvApprovers.DataBind()

                ''*************************************************
                '' "Form Level Security using Roles &/or Subscriptions"
                ''*************************************************
                CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                mvTabs.ActiveViewIndex = Int32.Parse(3)
                mvTabs.GetActiveView()
                mnuTabs.Items(3).Selected = True

            Else
                lblErrors.Text = "Action Cancelled. Please select a status Approved or Rejected."
                lblErrors.Visible = True
                lblErrors.Font.Size = 12
                lblReqAppComments.Text = "Comments is a required field when approving for another team member."
                lblReqAppComments.Visible = True
                lblReqAppComments.ForeColor = Color.Red
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
                Dim db As ImageButton = CType(e.Row.Cells(8).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As ExpProjPackaging.ExpProj_Packaging_ApprovalRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, ExpProjPackaging.ExpProj_Packaging_ApprovalRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record for " & DataBinder.Eval(e.Row.DataItem, "TeamMemberName") & "?');")
                End If
            End If

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
                    ''* Delete list for rebuild
                    ''***************
                    EXPModule.DeleteExpProjPackagingApproval(ViewState("pProjNo"), 0)

                    '***************
                    '* Build 1st level Approval
                    '***************
                    EXPModule.InsertExpProjPackagingApproval(ViewState("pProjNo"), cbUT.Checked, cbUN.Checked, cbUP.Checked, cbUR.Checked, cbUS.Checked, cbUW.Checked, cbOH.Checked, 14, DefaultUser, DefaultDate)

                    '***************
                    '* Build 2nd level Approval
                    '***************
                    EXPModule.InsertExpProjPackagingApproval(ViewState("pProjNo"), cbUT.Checked, cbUN.Checked, cbUP.Checked, cbUR.Checked, cbUS.Checked, cbUW.Checked, cbOH.Checked, 15, DefaultUser, DefaultDate)

                    '***************
                    '* Build 2nd level Approval
                    '***************
                    EXPModule.InsertExpProjPackagingApproval(ViewState("pProjNo"), cbUT.Checked, cbUN.Checked, cbUP.Checked, cbUR.Checked, cbUS.Checked, cbUW.Checked, cbOH.Checked, 16, DefaultUser, DefaultDate)

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

    Public Function EmailBody(ByVal MyMessage As MailMessage) As String

        Dim UGNLocation As String = Nothing

        If (cbUT.Checked = True) Then
            UGNLocation = "Tinley Park, "
        ElseIf (cbUN.Checked = True) Then
            UGNLocation &= IIf(UGNLocation = Nothing, "Chicago Heights, IL", "; Chicago Heights, IL")
        ElseIf (cbUP.Checked = True) Then
            UGNLocation &= IIf(UGNLocation = Nothing, "Jackson, TN", "; Jackson, TN")
        ElseIf (cbUR.Checked = True) Then
            UGNLocation &= IIf(UGNLocation = Nothing, "Somerset, KY", "; Somerset, KY")
        ElseIf (cbUS.Checked = True) Then
            UGNLocation &= IIf(UGNLocation = Nothing, "Valparaiso, IN", "; Valparaiso, IN")
        ElseIf (cbUW.Checked = True) Then
            UGNLocation &= IIf(UGNLocation = Nothing, "Silao, MX", "; Silao, MX")
        ElseIf (cbOH.Checked = True) Then
            UGNLocation &= IIf(UGNLocation = Nothing, "Monroe, OH", "; Monroe, OH")
        End If

        MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"
        MyMessage.Body &= " <tr bgcolor='#EBEBEB'>"
        MyMessage.Body &= "     <td colspan='2'><strong>PROJECT OVERVIEW</strong></td>"
        MyMessage.Body &= " </tr>"
        MyMessage.Body &= " <tr>"
        MyMessage.Body &= "     <td class='p_text' align='right'>Project No:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "     <td>" & ViewState("pProjNo") & "</td>"
        MyMessage.Body &= " </tr>"
        MyMessage.Body &= " <tr>"
        MyMessage.Body &= "     <td class='p_text' align='right'>Project Title:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "     <td>" & txtProjectTitle.Text & "</td>"
        MyMessage.Body &= " </tr>"
        MyMessage.Body &= " <tr>"
        MyMessage.Body &= "     <td class='p_text' align='right'>Project Leader:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "     <td>" & ddProjectLeader.SelectedItem.Text & "</td>"
        MyMessage.Body &= " </tr>"
        MyMessage.Body &= " <tr>"
        MyMessage.Body &= "     <td class='p_text' align='right' valign='top'>Description:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "     <td style='width: 700px;'>" & txtProjDateNotes.Text & "</td>"
        MyMessage.Body &= " </tr>"
        MyMessage.Body &= " <tr>"
        MyMessage.Body &= "     <td class='p_text' align='right'>UGN Location(s):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "     <td style='width: 600px;'>" & UGNLocation & "</td>"
        MyMessage.Body &= " </tr>"
        MyMessage.Body &= " <tr>"
        MyMessage.Body &= "     <td colspan='2'>"
        MyMessage.Body &= "     <table width='100%' border='0' style='font-size: 12; font-family: Tahoma;'>"
        MyMessage.Body &= "         <tr  bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
        MyMessage.Body &= "             <td><strong>Est. Completion Date</strong></td>"
        MyMessage.Body &= "             <td><strong>Est. Start Spend Date</strong></td>"
        MyMessage.Body &= "             <td><strong>Est. End Spend Date</strong></td>"
        MyMessage.Body &= "             <td><strong>Est. Customer Recovery Date</strong></td>"
        MyMessage.Body &= "         </tr>"
        MyMessage.Body &= "         <tr>"
        MyMessage.Body &= "             <td>" & txtNextEstCmpltDt.Text & "</td>"
        MyMessage.Body &= "             <td>" & txtEstSpendDt.Text & "</td>"
        MyMessage.Body &= "             <td>" & txtEstEndSpendDt.Text & "</td>"
        MyMessage.Body &= "             <td>" & txtEstRecoveryDt.Text & "</td>"
        MyMessage.Body &= "         </tr>"
        MyMessage.Body &= "     </table>"
        MyMessage.Body &= "     </td>"
        MyMessage.Body &= " </tr>"
        MyMessage.Body &= " <tr>"
        MyMessage.Body &= "     <td colspan='2'>"

        ''***************************************************
        ''Get list of Customer/Part information for display
        ''***************************************************
        MyMessage.Body &= "     <table width='100%' border='0' style='font-size: 12; font-family: Tahoma;'>"
        MyMessage.Body &= "         <tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
        MyMessage.Body &= "             <td><strong>Customer </strong></td>"
        MyMessage.Body &= "             <td><strong>Program / Platform / Assembly </strong></td>"
        MyMessage.Body &= "             <td><strong>Part No. </strong></td>"
        MyMessage.Body &= "             <td style='text-align:center'><strong>Pgm SOP Date </strong></td>"
        MyMessage.Body &= "             <td style='text-align:center'><strong>Pgm EOP Date </strong></td>"
        MyMessage.Body &= "             <td style='text-align:center'><strong>Pkg SOP Date </strong></td>"
        MyMessage.Body &= "         </tr>"

        Dim dsCP As DataSet
        dsCP = EXPModule.GetExpProjPackagingCustomer(ViewState("pProjNo"), 0)
        If dsCP.Tables.Count > 0 And (dsCP.Tables.Item(0).Rows.Count > 0) Then
            For i = 0 To dsCP.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= " <tr style='border-color:white'>"
                MyMessage.Body &= "     <td height='25'>" & dsCP.Tables(0).Rows(i).Item("ddCustomerDesc") & "&nbsp;</td>"
                MyMessage.Body &= "     <td height='25'>" & dsCP.Tables(0).Rows(i).Item("ProgramName") & "&nbsp;</td>"
                MyMessage.Body &= "     <td height='25'>" & dsCP.Tables(0).Rows(i).Item("PartNo") & "&nbsp;</td>"
                MyMessage.Body &= "     <td height='25'>" & dsCP.Tables(0).Rows(i).Item("SOP") & "&nbsp;</td>"
                MyMessage.Body &= "     <td height='25'>" & dsCP.Tables(0).Rows(i).Item("EOP") & "&nbsp;</td>"
                MyMessage.Body &= "     <td height='25'>" & dsCP.Tables(0).Rows(i).Item("PPAP") & "&nbsp;</td>"
                MyMessage.Body &= " </tr>"
            Next
        End If
        MyMessage.Body &= "     </table>"
        MyMessage.Body &= "     </td>"
        MyMessage.Body &= " </tr>"
        MyMessage.Body &= " <tr>"
        MyMessage.Body &= "     <td colspan='2'>"
        MyMessage.Body &= "     <table width='80%' style='font-size: 12; font-family: Tahoma;'>"
        MyMessage.Body &= "         <tr bgcolor='#EBEBEB' style='border-color:#EBEBEB; text-align:center'>"
        MyMessage.Body &= "             <td colspan='2'><strong>Requested Approval</strong></td>"
        MyMessage.Body &= "             <td colspan='2'><strong>Memo at Program Awarded</strong></td>"
        MyMessage.Body &= "         </tr>"
        MyMessage.Body &= "         <tr>"
        MyMessage.Body &= "             <td class='p_text' align='right' width='150px'>Amount to be Recovered ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "             <td>" & IIf(txtAmtToBeRecovered.Text = Nothing, txtNextAmtToBeRecovered, txtAmtToBeRecovered.Text) & "</td>"
        MyMessage.Body &= "             <td class='p_text' align='right' width='150px'>Amount to be Recovered ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "             <td>" & txtMPAAmtToBeRecovered.Text & "</td>"
        MyMessage.Body &= "         </tr>"
        MyMessage.Body &= "         <tr>"
        MyMessage.Body &= "             <td class='p_text' align='right'>UGN Total Cost ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "             <td>" & lblUGNTotalCost.Text & "</td>"
        MyMessage.Body &= "             <td class='p_text' align='right'>Total Cost ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "             <td>" & lblMPATotalCost.Text & "</td>"
        MyMessage.Body &= "         </tr>"
        MyMessage.Body &= "         <tr>"
        MyMessage.Body &= "             <td class='p_text' align='right'>Customer Total Cost ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "             <td colspan='3'>" & lblCustTotalCost.Text & "</td>"
        MyMessage.Body &= "         </tr>"
        MyMessage.Body &= "         <tr>"
        MyMessage.Body &= "             <td class='p_text' align='right'>Variance ($):&nbsp;&nbsp; </td>"
        MyMessage.Body &= "             <td colspan='3'>" & lblVarTotalCost.Text & "</td>"
        MyMessage.Body &= "         </tr>"
        MyMessage.Body &= "     </table>"
        MyMessage.Body &= "     </td>"
        MyMessage.Body &= " </tr>"

        ''***************************************************
        ''Get list of Supporting Documentation
        ''***************************************************
        Dim dsAED As DataSet
        dsAED = EXPModule.GetPackagingExpDocument(ViewState("pProjNo"), 0)
        If dsAED.Tables.Count > 0 And (dsAED.Tables.Item(0).Rows.Count > 0) Then
            MyMessage.Body &= " <tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
            MyMessage.Body &= "     <td colspan='2'><strong>SUPPORTING DOCUMENT(S):</strong></td>"
            MyMessage.Body &= " </tr>"
            MyMessage.Body &= " <tr>"
            MyMessage.Body &= "     <td colspan='2'>"
            MyMessage.Body &= "     <table style='font-size: 13; font-family: Tahoma;'>"
            MyMessage.Body &= "         <tr>"
            MyMessage.Body &= "             <td width='250px'><b>File Description</b></td>"
            MyMessage.Body &= "             <td width='250px'>&nbsp;</td>"
            MyMessage.Body &= "         </tr>"
            For i = 0 To dsAED.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= "     <tr>"
                MyMessage.Body &= "         <td height='25'>" & dsAED.Tables(0).Rows(i).Item("Description") & "</td>"
                MyMessage.Body &= "         <td height='25'><a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/PackagingExpProjDocument.aspx?pProjNo=" & ViewState("pProjNo") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
                MyMessage.Body &= "     </tr>"
            Next
            MyMessage.Body &= "     </table>"
            MyMessage.Body &= " </tr>"
        End If
        MyMessage.Body &= "</table>"

        Return True

    End Function 'EOF EmailBody()

    Public Sub SendNotifWhenEventChanges(ByVal EventDesc As String)
        ''********
        ''* This section will be used in the following methods:
        ''*     1) Email sent to all involved when an Packaging is CAPITALIZED
        ''*     2) Email sent to all involved when the Est. Completion Date changes with the Project Status is not Open
        ''*     3) Email sent to all involved when a Packaging is VOID
        ''********

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
            ''Is this for a Group Notification or Individual
            Select Case EventDesc
                Case "Capitalized" 'Sent by Accounting, notify all
                    GroupNotif = True
                Case "Void" 'Sent by Project Leader, notify all
                    GroupNotif = True
                Case "Estimated Completion Date Changed" 'Sent by Project Leader, notify all
                    GroupNotif = True
                Case "Amount to be Recovered Changed" 'Sent by Project Leader, notify all
                    GroupNotif = True
            End Select

            If ViewState("pProjNo") <> Nothing Then
                ''*********************************
                ''Send Notification
                ''*********************************
                If GroupNotif = True Then
                    ''*******************************
                    ''Notify TMs assigned for approval, including backups
                    ''*******************************
                    EmailTO = CarbonCopyList(Nothing, 14, "", 0, 0, EmailTO, DefaultTMID)
                    EmailTO = CarbonCopyList(Nothing, 15, "", 0, 0, EmailTO, DefaultTMID)
                    EmailTO = CarbonCopyList(Nothing, 16, "", 0, 0, EmailTO, DefaultTMID)

                    ''********************************************************
                    ''Notify Project Lead & Account Manager
                    ''********************************************************
                    EmailTO = CarbonCopyList(Nothing, 0, "", 0, 0, EmailTO, DefaultTMID)

                Else
                    ''*******************************************
                    ''Notify Accounting
                    ''*******************************************
                    EmailTO = CarbonCopyList(Nothing, 87, "", 0, 0, EmailTO, DefaultTMID)
                End If 'EOF  If GroupNotif = True Then
            End If 'EOF  If ReqAssetFound = False Then

            ''********************************************************
            ''Send Notification only if there is a valid Email Address
            ''********************************************************
            If EmailTO <> Nothing Then
                Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                ''********************************
                ''Carbon Copy CCList
                ''********************************
                If txtRoutingStatus.Text <> "T" And txtRoutingStatus.Text <> "R" Then
                    EmailCC = CarbonCopyList(MyMessage, 121, "", 0, 0, EmailCC, DefaultTMID)
                End If

                ''********************************
                ''Carbon Copy Materials Managers
                ''********************************
                EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUN.Checked = True, "UN", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUP.Checked = True, "UP", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUR.Checked = True, "UR", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUS.Checked = True, "US", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUT.Checked = True, "UT", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUT.Checked = True, "UW", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUT.Checked = True, "OH", ""), 0, 0, EmailCC, DefaultTMID)

                ''********************************
                ''Carbon Copy Ops Manager
                ''********************************
                EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUN.Checked = True, "UN", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUP.Checked = True, "UP", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUR.Checked = True, "UR", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUS.Checked = True, "US", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUT.Checked = True, "UT", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUT.Checked = True, "UW", ""), 0, 0, EmailCC, DefaultTMID)
                EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUT.Checked = True, "OH", ""), 0, 0, EmailCC, DefaultTMID)

                'Test or Production Message display
                If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                    MyMessage.Subject = "TEST: "
                    MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                Else
                    MyMessage.Subject = ""
                End If

                MyMessage.Subject &= "Packaging Expenditure: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text & " - " & EventDesc

                MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"
                If EventDesc = "Estimated Completion Date Changed" Or EventDesc = "Amount to be Recovered Changed" Then
                    MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>" & EventDesc.ToUpper & " by " & DefaultUserName & ".</strong></td>"
                Else
                    MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>This Packaging Expenditure was '" & EventDesc.ToUpper & "' by " & DefaultUserName & ".</strong></td>"
                End If

                MyMessage.Body &= "</tr>"
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td class='p_text' align='right' style='width: 150px;'>Project No:&nbsp;&nbsp; </td>"
                MyMessage.Body &= "<td> <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>" & ViewState("pProjNo") & "</a></td>"
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

                ''*****************
                ''History Tracking
                ''*****************
                Select Case EventDesc
                    Case "Capitalized" 'Sent by Accounting, notify all
                        EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Capitalized", "", "", "", "")
                    Case "Void" 'Sent by Project Leader, notify all
                        EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "VOID - Reason:" & txtVoidReason.Text, "", "", "", "")
                    Case "Estimated Completion Date Changed" 'Sent by Project Leader, notify all
                        EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Estimated Completion Date Changed From '" & txtHDEstCmpltDt.Text & "' to '" & txtNextEstCmpltDt.Text & "' - Reason: " & txtEstCmpltDtChngRsn.Text, "Estimated Completion Date", txtHDEstCmpltDt.Text, txtNextEstCmpltDt.Text, txtEstCmpltDtChngRsn.Text)
                    Case "Amount to be Recovered Changed" 'Sent by Project Leader, notify all

                        EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Amount to be Recovered Changed From '" & Format(HDNextATBR, "#,##0.00") & "' to '" & Format(NextATBR, "#,##0.00") & "' - Reason: " & txtAmtToBeRecoveredChngRsn.Text, "Amount to be Recovered", Format(HDNextATBR, "#,##0.00"), Format(NextATBR, "#,##0.00"), txtAmtToBeRecoveredChngRsn.Text)
                End Select

                ''**********************************
                ''Connect & Send email notification
                ''**********************************
                Try
                    commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (P)", ViewState("pProjNo"))
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

    Protected Sub btnFwdApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFwdApproval.Click
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

        Try
            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then

                If txtHDOrigUGNTotalCost.Text <> 0 And _
                txtHDOrigUGNTotalCost.Text <> lblUGNTotalCost.Text Then
                    BuildApprovalList()
                    EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "UGN Total Cost changed from $" & txtHDOrigUGNTotalCost.Text & " to $" & lblUGNTotalCost.Text, "UGN Total Cost", lblUGNTotalCost.Text, txtHDOrigUGNTotalCost.Text, "")
                ElseIf txtHDOrigUGNTotalCost.Text = 0 Then
                    BuildApprovalList()
                End If

                ''***************
                ''Verify that atleast one Customer Info entry has been entered before
                ''***************
                Dim dsExp As DataSet = New DataSet
                Dim ReqAssetFound As Boolean = False
                dsExp = EXPModule.GetExpProjPackagingCustomer(ViewState("pProjNo"), 0)
                If commonFunctions.CheckDataSet(dsExp) = False Then 'If missing kick user out from submission.
                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True

                    rfvProgram.IsValid = False
                    ' ''rfvCustomer.IsValid = False
                    rfvPartNo.IsValid = False
                    rfvSOP.IsValid = False
                    rfvEOP.IsValid = False
                    rfvPPAPDt.IsValid = False
                    vsCustomer.ShowSummary = True
                    ReqAssetFound = True

                    lblErrors.Text = "Atleast one Customer entry is required for submission."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                End If 'EOF If commonFunctions.CheckDataset(dsExp) = True

                ''***************
                ''Verify that atleast one Packaging Expenditure Info entry has been entered before
                ''***************
                dsExp = EXPModule.GetExpProjPackagingExpenditure(ViewState("pProjNo"), 0)
                If commonFunctions.CheckDataSet(dsExp) = False Then 'If missing kick user out from submission.
                    mvTabs.ActiveViewIndex = Int32.Parse(2)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(2).Selected = True

                    rfvVendorStatus.IsValid = False
                    rfvVendorType.IsValid = False
                    rfvVendor.IsValid = False
                    rfvDescription.IsValid = False
                    rfvUGNLocation.IsValid = False
                    rfvQuantity.IsValid = False
                    rfvUGNUnitCost.IsValid = False
                    rfvCustUnitCost.IsValid = False
                    vsExpense.ShowSummary = True
                    ReqAssetFound = True

                    lblErrors.Text = "Atleast one Packaging Expenditure entry is required for submission."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False
                    Exit Sub
                End If 'EOF If commonFunctions.CheckDataset(dsExp) = True


                ''**********************
                ''* Save data prior to submission before approvals
                ''**********************
                UpdateRecord("In Process", "T", False)


                ''*******************************
                ''Send Notification to Approvers
                ''Locate 1st level approver
                ''*******************************
                If (txtRoutingStatus.Text <> "R") Then
                    ds1st = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), 1, 0, False, False)
                Else 'IF Rejected - only notify the TM who Rejected the record
                    If txtHDOrigUGNTotalCost.Text = lblUGNTotalCost.Text Then
                        ds1st = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), 0, 0, False, True)
                    Else
                        ds1st = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), 1, 0, False, False)
                    End If

                End If
                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(ds1st) = True Then
                    For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                        If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                            (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                            (ddProjectLeader.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID")) Then

                            EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"
                            EmpName &= ds1st.Tables(0).Rows(i).Item("EmailTMName") & ", "

                            ''************************************************************
                            ''Update 1st level DateNotified field.
                            ''************************************************************
                            If (txtRoutingStatus.Text <> "R") Then
                                EXPModule.UpdateExpProjPackagingApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 1, DefaultUser, DefaultDate)
                            Else 'IF Rejected - only notify the TM who Rejected the record
                                EXPModule.UpdateExpProjPackagingApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), DefaultUser, DefaultDate)
                                SeqNo = ds1st.Tables(0).Rows(i).Item("SeqNo")
                                OrigTMID = ds1st.Tables(0).Rows(i).Item("TeamMemberID")
                            End If
                        Else
                            ''************************************************************
                            ''1st Level Approver same as Project Sponsor.  Update record.DefaultTMID
                            ''************************************************************
                            If (txtRoutingStatus.Text <> "R") Then
                                EXPModule.UpdateExpProjPackagingApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), True, "Approved", "", 1, DefaultUser, DefaultDate)
                            Else 'IF Rejected - only notify the TM who Rejected the record
                                EXPModule.UpdateExpProjPackagingApproval(ViewState("pProjNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Approved", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), DefaultUser, DefaultDate)
                            End If

                            SponsSameAs1stLvlAprvr = True
                        End If 'EOF If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail)
                    Next 'EOF For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                End If 'EOF If commonFunctions.CheckDataset(ds1st) = True 

                ''***************************************************************
                ''Locate 2nd Level Approver(s)
                ''***************************************************************
                If SponsSameAs1stLvlAprvr = True Then
                    ds2nd = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), 2, 0, False, False)
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(ds2nd) = True Then
                        For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                            If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                ''************************************************************
                                ''Update 2nd level DateNotified field.
                                ''************************************************************
                                EXPModule.UpdateExpProjPackagingApproval(ViewState("pProjNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 2, DefaultUser, DefaultDate)
                            End If
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
                        ''Carbon Copy Account Manager
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                        ''********************************
                        ''Carbon Copy Materials Managers
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUN.Checked = True, "UN", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUP.Checked = True, "UP", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUR.Checked = True, "UR", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUS.Checked = True, "US", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUT.Checked = True, "UT", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbUW.Checked = True, "UW", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 122, IIf(cbOH.Checked = True, "OH", ""), 0, 0, EmailCC, DefaultTMID)

                        ''********************************
                        ''Carbon Copy Ops Manager
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUN.Checked = True, "UN", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUP.Checked = True, "UP", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUR.Checked = True, "UR", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUS.Checked = True, "US", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUT.Checked = True, "UT", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbUW.Checked = True, "UW", ""), 0, 0, EmailCC, DefaultTMID)
                        EmailCC = CarbonCopyList(MyMessage, 123, IIf(cbOH.Checked = True, "OH", ""), 0, 0, EmailCC, DefaultTMID)
                    Else 'Rejected

                        ''********************************
                        ''Carbon Copy Same Level
                        ''********************************
                        EmailCC = CarbonCopyList(MyMessage, 0, "", SeqNo, OrigTMID, EmailCC, DefaultTMID)

                    End If

                    ''Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                    Else
                        MyMessage.Subject = ""
                        MyMessage.Body = ""
                        'MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                    End If

                    MyMessage.Subject &= "Packaging Expenditure: " & ViewState("pProjNo") & " - " & txtProjectTitle.Text

                    MyMessage.Body &= "<font size='2' face='Tahoma'>"
                    MyMessage.Body &= EmpName
                    MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtProjectTitle.Text & "' is available for your Review/Approval. "
                    MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crExpProjPackagingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"
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
                    EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), txtProjectTitle.Text, DefaultTMID, "Record completed and forwarded to " & EmpName & " for approval.", "", "", "", "")

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (P)", ViewState("pProjNo"))
                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                        lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."
                    Catch ex As SmtpException
                        lblErrors.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                        lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."

                        UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, CurrentEmpEmail & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
                        'get current event name
                        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                        'log and email error
                        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                    End Try
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False
                    lblReqAppComments.Visible = True
                    lblReqAppComments.ForeColor = Color.Red

                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pProjNo"), ViewState("pCO"))
                    gvApprovers.DataBind()

                    ''*************************************************
                    '' "Form Level Security using Roles &/or Subscriptions"
                    ''*************************************************
                    CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                    mvTabs.ActiveViewIndex = Int32.Parse(3)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(3).Selected = True

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

            If SeqNo = 0 Then 'No Rejections have been made, Send notification to all who applies
                If SubscriptionID = 0 Then ''Account Mananager
                    dsCC = EXPModule.GetExpProjPackagingLead(ViewState("pProjNo"))
                    IncludeOrigAprvlTM = True
                Else '' All others based on facility
                    If UGNLoc <> Nothing Then
                        dsCC = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(SubscriptionID, UGNLoc)
                    Else
                        If SubscriptionID = 121 Or SubscriptionID = 14 Or SubscriptionID = 15 Then
                            ''Notify Accounting, or 1st level or 2nd level
                            dsCC = commonFunctions.GetTeamMemberBySubscription(SubscriptionID)
                            IncludeOrigAprvlTM = True
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
                dsCC = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), SeqNo, 0, False, False)
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
                dsCC = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), 0, 0, False, False)
                'CC Orgiginal Approver in an event that their a corporate calendar default responsibility
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                        (ddProjectLeader.SelectedValue <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) And _
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

#End Region 'EOF Email Notifications

End Class