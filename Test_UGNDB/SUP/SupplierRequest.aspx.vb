' ************************************************************************************************
' Name:	SupplierRequest.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 09/08/2010    LRey			Created .Net application
' 05/16/2012    LRey            Added link back to InternalOrderRequest.aspx page
' 05/22/2012    LRey            Modified email notification to use .NET mail method
' 02/24/2014    LRey            Modified to adhere to the new ERP Supplier codes
' ************************************************************************************************
Partial Class SUP_SupplierRequest
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            '*******
            ' Initialize ViewState
            '*******
            'Used to define the primary record
            If HttpContext.Current.Request.QueryString("pSUPNo") <> "" Then
                ViewState("pSUPNo") = HttpContext.Current.Request.QueryString("pSUPNo")
            Else
                ViewState("pSUPNo") = ""
            End If

            'Used to define the vendor type
            If HttpContext.Current.Request.QueryString("pVT") <> "" Then
                ViewState("pVT") = HttpContext.Current.Request.QueryString("pVT")
            Else
                ViewState("pVT") = ""
            End If

            'Used to take user back to Supporting Documents Tab after save.
            If HttpContext.Current.Request.QueryString("pSD") <> "" Then
                ViewState("pSD") = HttpContext.Current.Request.QueryString("pSD")
            Else
                ViewState("pSD") = 0
            End If

            'Used for Document binddata and update
            If HttpContext.Current.Request.QueryString("pDocID") <> "" Then
                ViewState("pDocID") = HttpContext.Current.Request.QueryString("pDocID")
            Else
                ViewState("pDocID") = 0
            End If

            'Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pAprv") <> "" Then
                ViewState("pAprv") = HttpContext.Current.Request.QueryString("pAprv")
            Else
                ViewState("pAprv") = 0
            End If


            'Used to capture the form name where the user entered from
            If HttpContext.Current.Request.QueryString("pForm") <> "" Then
                ViewState("pForm") = HttpContext.Current.Request.QueryString("pForm")
            Else
                ViewState("pForm") = ""
            End If

            ''Used to take user back to CapEx screen after reset/save
            If HttpContext.Current.Request.QueryString("pProjNo") <> "" Then
                ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
            Else
                ViewState("pProjNo") = ""
            End If

            ''Used to take user back to IOR screen after reset/save
            If HttpContext.Current.Request.QueryString("pIORNo") <> "" Then
                ViewState("pIORNo") = HttpContext.Current.Request.QueryString("pIORNo")
            Else
                ViewState("pIORNo") = ""
            End If


            '****************************************************
            ' Update the title and heading on the Master Page
            '****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            If ViewState("pSUPNo") = Nothing Then
                m.ContentLabel = "New Supplier Request"
            Else
                m.ContentLabel = "Supplier Request"
            End If

            '**************************************************
            ' Override the Master Page bread crumb navigation
            '**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                Dim pForm As String = ViewState("pForm")
                Select Case pForm
                    Case "EXPKG"
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Capital Projects</b> > <a href='../EXP/PackagingExpProjList.aspx'><b>Packaging Expense Search</b></a> > <a href='../EXP/PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pEV=1'><b>Packaging Expense</b></a> > New Supplier Request"
                    Case "PURIOR"
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='../PUR/InternalOrderRequestList.aspx'><b>Internal Order Request Search</b></a> > <a href='../PUR/InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "'><b>Internal Order Request</b></a> > New Supplier Request"
                    Case Else
                        If ViewState("pSUPNo") = Nothing Then
                            lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='SupplierRequestList.aspx'><b>Supplier Request Search</b></a> > New Supplier Request"
                        Else
                            If ViewState("pAprv") = 0 Then
                                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='SupplierRequestList.aspx'><b>Supplier Request Search</b></a> > Supplier Request"
                            Else 'Go Back To approval Screen
                                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='SupplierRequestList.aspx'><b>Supplier Request Search</b></a> > <a href='crSupplierRequestApproval.aspx?pSUPNo=" & ViewState("pSUPNo") & "&pAprv=1'><b>Approval</b></a> > Supplier Request"
                            End If
                        End If
                End Select

                lbl.Visible = True
            End If

            ctl = m.FindControl("SiteMapPath1")
            If ctl IsNot Nothing Then
                Dim smp As SiteMapPath = CType(ctl, SiteMapPath)
                smp.Visible = False
            End If

            '******************************************
            ' Expand this Master Page menu item
            '******************************************
            ctl = m.FindControl("PURExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If

            '*************************************************
            'Check if IsPostBack
            '*************************************************
            If Not Page.IsPostBack Then

                '****************************************
                'Redirect user to the right tab location
                '****************************************
                If ViewState("pSUPNo") <> "" Then
                    BindCriteria()
                    BindData(ViewState("pSUPNo"))
                Else
                    BindCriteria()
                    txtVendorName.Focus()
                End If

                If ViewState("pDocID") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(2)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(2).Selected = True
                ElseIf ViewState("pSD") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(2)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(2).Selected = True
                Else
                    mvTabs.ActiveViewIndex = Int32.Parse(0)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(0).Selected = True
                End If
            End If

            '*************************************************
            ' "Form Level Security using Roles &/or Subscriptions"
            '*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            txtReasonForAddition.Attributes.Add("onkeypress", "return tbLimit();")
            txtReasonForAddition.Attributes.Add("onkeyup", "return tbCount(" + lblReasonForAddition.ClientID + ");")
            txtReasonForAddition.Attributes.Add("maxLength", "300")

            txtVoidReason.Attributes.Add("onkeypress", "return tbLimit();")
            txtVoidReason.Attributes.Add("onkeyup", "return tbCount(" + lblVoidRsn.ClientID + ");")
            txtVoidReason.Attributes.Add("maxLength", "400")

            Dim strPreviewClientScript As String = "javascript:void(window.open('crViewSupplierRequest.aspx?pSUPNo=" & ViewState("pSUPNo") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
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
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            ViewState("strProdOrTestEnvironment") = strProdOrTestEnvironment

            ''*******
            '' Disable controls by default
            ''*******
            btnAdd.Enabled = False
            btnSave1.Enabled = False
            btnSave2.Enabled = False
            btnReset1.Enabled = False 'Supplier Info Tab
            btnReset2.Enabled = False 'Contact Info Tab
            btnReset3.Enabled = False 'Supporting Documents Tab
            btnUpload.Enabled = False
            uploadFile.Enabled = False
            btnBuildApproval.Enabled = False
            btnBuildApproval.Visible = False
            btnFwdApproval.Enabled = False
            gvSupportingDocument.Columns(4).Visible = False
            gvApprovers.Columns(7).Visible = False
            btnDelete.Enabled = False
            btnPreview.Enabled = False
            lblReqVendor.Visible = False
            rfvReqVendor.Visible = False
            ddVendor.Enabled = False
            ddStatus.Enabled = False
            ddInBPCS.Enabled = False
            cbTen99.Enabled = False
            txtVendorNo.Enabled = False

            mnuTabs.Items(1).Enabled = False
            mnuTabs.Items(2).Enabled = False
            mnuTabs.Items(3).Enabled = False

            If ddVendorType.SelectedValue = "MROU" Then
                ddMROU.Visible = True
            Else
                ddINVU.Visible = False
            End If

            If ddVendorType.SelectedValue <> "INVU" Then
                lblComPri.Visible = False
                ddFamily.Enabled = False
                rfvFamily.Enabled = False
                ddSubFamily.Enabled = False
                ddINVU.Visible = False
            Else
                lblComPri.Visible = True
                ddFamily.Enabled = True
                rfvFamily.Enabled = True
                ddSubFamily.Enabled = True
                ddINVU.Visible = True
            End If

            If ddVendorType.SelectedValue = "SERU" Then
                ddContractorOnSite.Enabled = True
                lblReqContractorOnSite.Visible = True
                rfvContractorOnSite.Enabled = True
            Else
                ddContractorOnSite.Enabled = False
                lblReqContractorOnSite.Visible = False
                rfvContractorOnSite.Enabled = False
            End If

            If ddReplacesCurrentVendor.SelectedValue = True Then
                ddVendor.Enabled = True
                lblReqVendor.Visible = True
                rfvReqVendor.Enabled = True
            Else
                ddVendor.Enabled = False
                lblReqVendor.Visible = False
                rfvReqVendor.Enabled = False
                ddVendor.SelectedValue = Nothing
            End If

            txtVoidReason.Enabled = False
            txtVoidReason.Visible = False
            lblVoidReason.Visible = False
            rfvVoidReason.Enabled = False
            lblReqVoidReason.Visible = False

            If ddReplacesCurrentVendor.SelectedValue = False Then
                ddVendor.Enabled = False
            Else
                ddVendor.Enabled = True
            End If

            If ViewState("pSUPNo") <> "" Then
                If ddFutureVendor.SelectedItem.Value = True And _
           ddFutureVendor.SelectedValue IsNot System.DBNull.Value Then
                    lblReqAppComments.Text = "This Supplier is used for Quoting a Cost Sheet only and it does not require approval at this time. If and when this Supplier becomes an active vendor, the value for Future Vendor must be changed to 'No'. Make sure all contact information and documentation is completed prior to submission for approval."
                    lblReqAppComments.Visible = True
                    lblReqAppComments.ForeColor = Color.Red
                End If
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim dsCorpAcct As DataSet
            Dim dsCorpAcctMgr As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 110 'Supplier Request Form ID
            Dim iRoleID As Integer = 0
            Dim i As Integer = 0
            Dim iCorpAcctTMID As Integer = 0 'Used to locate Corporate Accounting 
            Dim iCorpAcctMgrTMID As Integer = 0 'Used to locate Corporate Accounting Mgr 

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Gina.Lacny", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)
                        ''***********
                        ''* Locate Corporate Accounting
                        ''***********
                        dsCorpAcct = commonFunctions.GetTeamMemberBySubscription(95)
                        If dsCorpAcct IsNot Nothing Then
                            If dsCorpAcct.Tables.Count And dsCorpAcct.Tables(0).Rows.Count > 0 Then
                                iCorpAcctTMID = dsCorpAcct.Tables(0).Rows(0).Item("TMID")
                                ViewState("iCorpAcctTMID") = iCorpAcctTMID
                            End If
                        End If

                        ''***********
                        ''* Locate Corporate Accounting Mgr
                        ''***********
                        dsCorpAcctMgr = commonFunctions.GetTeamMemberBySubscription(118)
                        If dsCorpAcct IsNot Nothing Then
                            If dsCorpAcctMgr.Tables.Count And dsCorpAcctMgr.Tables(0).Rows.Count > 0 Then
                                iCorpAcctMgrTMID = dsCorpAcctMgr.Tables(0).Rows(0).Item("TMID")
                                ViewState("iCorpAcctMgrTMID") = iCorpAcctMgrTMID
                            End If
                        End If

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
                                            If ViewState("pSUPNo") = Nothing Or ViewState("pSUPNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                txtVendorName.Focus()
                                                btnSave1.Enabled = True
                                                btnReset1.Enabled = True
                                            Else
                                                Select Case ddStatus.SelectedValue
                                                    Case "New Entry"
                                                        If (txtRoutingStatus.Text = "N") Then
                                                            If ddFutureVendor.SelectedValue = False Then
                                                                btnFwdApproval.Enabled = True
                                                            End If
                                                        End If
                                                        btnDelete.Enabled = True
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "R" Or txtRoutingStatus.Text = "S") Then
                                                            btnFwdApproval.Enabled = True
                                                        End If
                                                        If txtRoutingStatus.Text = "T" Then
                                                            gvApprovers.Columns(7).Visible = True
                                                        End If
                                                        ddStatus.Enabled = True

                                                    Case "Void"
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidReason.Visible = True
                                                        rfvVoidReason.Enabled = True
                                                        lblReqVoidReason.Visible = True
                                                        ddStatus.Enabled = True
                                                End Select
                                                SDExtender.Collapsed = False
                                                btnSave1.Enabled = True
                                                btnReset1.Enabled = True
                                                btnSave2.Enabled = True
                                                btnReset2.Enabled = True
                                                btnReset3.Enabled = True
                                                btnUpload.Enabled = True
                                                btnDelete.Enabled = True
                                                uploadFile.Enabled = True
                                                btnPreview.Enabled = True
                                                gvSupportingDocument.Columns(4).Visible = True
                                                ddInBPCS.Enabled = True
                                                cbTen99.Enabled = True
                                                txtVendorNo.Enabled = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                            End If
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Sales/Program Mgrs, Tooling/Purchasing Leads
                                            ViewState("ObjectRole") = True
                                            btnAdd.Enabled = True
                                            ''*************************************************
                                            ''for new entries, enable only the first tab
                                            ''*************************************************
                                            If ViewState("pSUPNo") = Nothing Or ViewState("pSUPNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                txtVendorName.Focus()
                                                btnSave1.Enabled = True
                                                btnReset1.Enabled = True
                                            Else
                                                btnReset3.Enabled = True
                                                btnUpload.Enabled = True
                                                uploadFile.Enabled = True
                                                Select Case ddStatus.SelectedValue
                                                    Case "New Entry"
                                                        ViewState("Admin") = True
                                                        If (txtRoutingStatus.Text = "N") Then
                                                            If ddFutureVendor.SelectedValue = False Then
                                                                btnFwdApproval.Enabled = True
                                                            End If
                                                        End If
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        btnReset2.Enabled = True
                                                        btnSave2.Enabled = True
                                                        gvSupportingDocument.Columns(4).Visible = True
                                                        SDExtender.Collapsed = False
                                                    Case "In Process"
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        ddStatus.Enabled = True
                                                        If (txtRoutingStatus.Text = "R") Then
                                                            ViewState("Admin") = True
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            btnReset2.Enabled = True
                                                            btnSave2.Enabled = True
                                                            SDExtender.Collapsed = False
                                                            If ddFutureVendor.SelectedValue = False Then
                                                                btnFwdApproval.Enabled = True
                                                            End If
                                                        End If
                                                    Case "Approved"
                                                        btnSave1.Enabled = True
                                                        btnReset1.Enabled = True
                                                        btnSave2.Enabled = True
                                                        btnReset2.Enabled = True
                                                        ddStatus.Enabled = True
                                                        ddStatus.Items.RemoveAt(0)
                                                    Case "Closed"
                                                        If (txtRoutingStatus.Text <> "C") Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            btnSave2.Enabled = True
                                                            btnReset2.Enabled = True
                                                            ddStatus.Enabled = True
                                                        Else
                                                            ddStatus.Visible = False
                                                        End If
                                                    Case "Void"
                                                        txtVoidReason.Enabled = True
                                                        txtVoidReason.Visible = True
                                                        lblVoidReason.Visible = True
                                                        lblReqVoidReason.Visible = True
                                                        If (txtRoutingStatus.Text <> "V") Then
                                                            btnSave1.Enabled = True
                                                            btnReset1.Enabled = True
                                                            btnSave2.Enabled = True
                                                            btnReset2.Enabled = True
                                                        Else
                                                            ddStatus.Visible = False
                                                        End If
                                                End Select
                                                If txtRoutingStatus.Text = "N" Then
                                                    btnDelete.Enabled = True
                                                End If
                                                btnPreview.Enabled = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                            End If
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            '** Used by Backup persons
                                            If ViewState("pSUPNo") = Nothing Or ViewState("pSUPNo") = "" Then
                                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                                mvTabs.GetActiveView()
                                                mnuTabs.Items(0).Selected = True
                                                txtVendorName.Focus()
                                                btnSave1.Enabled = True
                                                btnReset1.Enabled = True
                                            Else
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                mnuTabs.Items(3).Enabled = True
                                                btnPreview.Enabled = True
                                                Select Case ddStatus.SelectedValue
                                                    Case "In Process"
                                                        If (txtRoutingStatus.Text = "T") Then
                                                            If (iCorpAcctTMID = iTeamMemberID) Or (iCorpAcctMgrTMID = iTeamMemberID) Then
                                                                'Only current TM that matches will have access to edit fields
                                                                ViewState("ObjectRole") = True
                                                                ViewState("Admin") = True
                                                                btnSave1.Enabled = True
                                                                btnReset1.Enabled = True
                                                                lblInBPCS.Visible = True
                                                                lblVendorNo.Visible = True
                                                                rfvVendorNo.Enabled = True
                                                                rfvInBPCS.Enabled = True
                                                                ddInBPCS.Enabled = True
                                                                cbTen99.Enabled = True
                                                                txtVendorNo.Enabled = True
                                                            End If
                                                            gvApprovers.Columns(7).Visible = True
                                                        End If
                                                End Select
                                            End If
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            ViewState("ObjectRole") = False
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                            mnuTabs.Items(3).Enabled = True
                                            btnPreview.Enabled = True
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ''Used by the Accounting Group
                                            ViewState("ObjectRole") = False
                                            btnUpload.Enabled = True
                                            uploadFile.Enabled = True
                                            btnReset3.Enabled = True
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
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
#End Region 'EOF Form Level Security

#Region "General"
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
            ddRequestedBy.SelectedValue = HttpContext.Current.Session("UserId")
            ddTeamMember.SelectedValue = HttpContext.Current.Session("UserId")

            ''bind existing data to drop down GLAccounts or Cost Center control for selection criteria for search
            ds = SUPModule.GetVendorTerm("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddTerms.DataSource = ds
                ddTerms.DataTextField = ds.Tables(0).Columns("ddTerm").ColumnName.ToString()
                ddTerms.DataValueField = ds.Tables(0).Columns("TID").ColumnName.ToString()
                ddTerms.DataBind()
                ddTerms.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Vendor Type control for selection criteria for search
            ds = commonFunctions.GetVendorType(True)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddVendorType.DataSource = ds
                ddVendorType.DataTextField = ds.Tables(0).Columns("ddVType").ColumnName.ToString()
                ddVendorType.DataValueField = ds.Tables(0).Columns("VType").ColumnName.ToString()
                ddVendorType.DataBind()
                ddVendorType.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Vendor control for selection criteria for search
            ds = SUPModule.GetSupplierLookUp("", "", IIf(ddVendorType.SelectedValue = Nothing, "", ddVendorType.SelectedValue), "", "", 1)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddVendor.DataSource = ds
                ddVendor.DataTextField = ds.Tables(0).Columns("ddVendorName").ColumnName.ToString()
                ddVendor.DataValueField = ds.Tables(0).Columns("VendorNo").ColumnName.ToString()
                ddVendor.DataBind()
                ddVendor.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Commodity Primary control for selection criteria for search
            ds = commonFunctions.GetFamily()
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddFamily.DataSource = ds
                ddFamily.DataTextField = ds.Tables(0).Columns("ddFamilyName").ColumnName.ToString()
                ddFamily.DataValueField = ds.Tables(0).Columns("FamilyID").ColumnName.ToString()
                ddFamily.DataBind()
                ddFamily.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Commodity Primary control for selection criteria for search
            ds = commonFunctions.GetSubFamily(IIf(ddFamily.SelectedValue = Nothing, 0, ddFamily.SelectedValue))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSubFamily.DataSource = ds
                ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName.ToString()
                ddSubFamily.DataBind()
                ddSubFamily.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Remit Country control for selection criteria for search
            ds = commonFunctions.GetCountry("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddRemitToCountry.DataSource = ds
                ddRemitToCountry.DataTextField = ds.Tables(0).Columns("CountryDesc").ColumnName.ToString()
                ddRemitToCountry.DataValueField = ds.Tables(0).Columns("Country").ColumnName.ToString()
                ddRemitToCountry.DataBind()
                ddRemitToCountry.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Ship From Country control for selection criteria for search
            ds = commonFunctions.GetCountry("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddShipFromCountry.DataSource = ds
                ddShipFromCountry.DataTextField = ds.Tables(0).Columns("CountryDesc").ColumnName.ToString()
                ddShipFromCountry.DataValueField = ds.Tables(0).Columns("Country").ColumnName.ToString()
                ddShipFromCountry.DataBind()
                ddShipFromCountry.Items.Insert(0, "")
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

    Public Sub BindData(ByVal SUPNO As String)
        Try
            Dim ds As DataSet = New DataSet
            If SUPNO <> Nothing Then
                ds = SUPModule.GetSupplierRequest(SUPNO)
                If commonFunctions.CheckDataSet(ds) = True Then
                    lblSUPNO.Text = ds.Tables(0).Rows(0).Item("SUPNo").ToString()
                    ddVendorType.SelectedValue = ds.Tables(0).Rows(0).Item("Vendortype").ToString()
                    lblVendorType.Text = ds.Tables(0).Rows(0).Item("Vendortype").ToString()
                    If ds.Tables(0).Rows(0).Item("Vendortype").ToString() = "INVU" Then
                        ddINVU.SelectedValue = ds.Tables(0).Rows(0).Item("VTypeDesc").ToString()
                    ElseIf ds.Tables(0).Rows(0).Item("Vendortype").ToString() = "MROU" Then
                        ddMROU.SelectedValue = ds.Tables(0).Rows(0).Item("VTypeDesc").ToString()
                    End If

                    txtVendorName.Text = ds.Tables(0).Rows(0).Item("VendorName").ToString()
                    txtVendorNo.Text = ds.Tables(0).Rows(0).Item("VendorNo").ToString()
                    ddInBPCS.SelectedValue = ds.Tables(0).Rows(0).Item("InBPCS").ToString()
                    cbTen99.Checked = ds.Tables(0).Rows(0).Item("Ten99").ToString()
                    txtPhone.Text = ds.Tables(0).Rows(0).Item("Phone").ToString()
                    txtSalesFax.Text = ds.Tables(0).Rows(0).Item("SalesFax").ToString()
                    txtProdDesc.Text = ds.Tables(0).Rows(0).Item("ProductDescription").ToString()
                    ddRequestedBy.SelectedValue = ds.Tables(0).Rows(0).Item("RequestedByTMID").ToString()
                    lblDateSubmitted.Text = ds.Tables(0).Rows(0).Item("DateSubmitted").ToString()
                    cbUT.Checked = ds.Tables(0).Rows(0).Item("UT").ToString()
                    cbUN.Checked = ds.Tables(0).Rows(0).Item("UN").ToString()
                    cbUP.Checked = ds.Tables(0).Rows(0).Item("UP").ToString()
                    cbUR.Checked = ds.Tables(0).Rows(0).Item("UR").ToString()
                    cbUS.Checked = ds.Tables(0).Rows(0).Item("US").ToString()
                    cbUW.Checked = ds.Tables(0).Rows(0).Item("UW").ToString()
                    cbOH.Checked = ds.Tables(0).Rows(0).Item("OH").ToString()
                    cbNew.Checked = ds.Tables(0).Rows(0).Item("NewVendor").ToString()
                    cbChange.Checked = ds.Tables(0).Rows(0).Item("ChangeToCurrentVendor").ToString()
                    ddContractorOnSite.SelectedValue = ds.Tables(0).Rows(0).Item("ContractorOnSite").ToString()
                    If ds.Tables(0).Rows(0).Item("FutureVendor") IsNot System.DBNull.Value Then
                        ddFutureVendor.SelectedValue = ds.Tables(0).Rows(0).Item("FutureVendor").ToString()
                    End If
                    txtSalesContactName.Text = ds.Tables(0).Rows(0).Item("SalesContactName").ToString()
                    txtAcctContact.Text = ds.Tables(0).Rows(0).Item("AcctContact").ToString()
                    txtAcctPhone.Text = ds.Tables(0).Rows(0).Item("AcctPhone").ToString()
                    txtAcctFax.Text = ds.Tables(0).Rows(0).Item("AcctFax").ToString()
                    txtRemitToAddr1.Text = ds.Tables(0).Rows(0).Item("RemitToAddr1").ToString()
                    txtRemitToAddr2.Text = ds.Tables(0).Rows(0).Item("RemitToAddr2").ToString()
                    txtRemitToAddr3.Text = ds.Tables(0).Rows(0).Item("RemitToAddr3").ToString()
                    txtRemitToAddr4.Text = ds.Tables(0).Rows(0).Item("RemitToAddr4").ToString()
                    txtRemitCity.Text = ds.Tables(0).Rows(0).Item("RemitToCity").ToString()
                    txtRemitState.Text = ds.Tables(0).Rows(0).Item("RemitToState").ToString()
                    txtRemitZip.Text = ds.Tables(0).Rows(0).Item("RemitToZip").ToString()
                    ddRemitToCountry.SelectedValue = ds.Tables(0).Rows(0).Item("RemitToCountry").ToString()
                    txtCustServContact.Text = ds.Tables(0).Rows(0).Item("CustServContact").ToString()
                    txtCustServPhone.Text = ds.Tables(0).Rows(0).Item("CustServPhone").ToString()
                    txtCustServFax.Text = ds.Tables(0).Rows(0).Item("CustServFax").ToString()
                    txtCustServEmail.Text = ds.Tables(0).Rows(0).Item("CustServEmail").ToString()
                    txtShipFromAddr1.Text = ds.Tables(0).Rows(0).Item("ShipFromAddr1").ToString()
                    txtShipFromAddr2.Text = ds.Tables(0).Rows(0).Item("ShipFromAddr2").ToString()
                    txtShipFromAddr3.Text = ds.Tables(0).Rows(0).Item("ShipFromAddr3").ToString()
                    txtShipFromAddr4.Text = ds.Tables(0).Rows(0).Item("ShipFromAddr4").ToString()
                    txtShipFromCity.Text = ds.Tables(0).Rows(0).Item("ShipFromCity").ToString()
                    txtShipFromState.Text = ds.Tables(0).Rows(0).Item("ShipFromState").ToString()
                    txtShipFromZip.Text = ds.Tables(0).Rows(0).Item("ShipFromZip").ToString()
                    ddShipFromCountry.SelectedValue = ds.Tables(0).Rows(0).Item("ShipFromCountry").ToString()
                    ddTerms.SelectedValue = ds.Tables(0).Rows(0).Item("Terms").ToString()
                    ddPayType.SelectedValue = ds.Tables(0).Rows(0).Item("PaymentType").ToString()
                    txtInitialPurchaseAmt.Text = ds.Tables(0).Rows(0).Item("InitialPurchaseAmt").ToString()
                    txtEstAmtAnnualPurchase.Text = ds.Tables(0).Rows(0).Item("EstAmtAnnualPurchase").ToString()
                    ddReplacesCurrentVendor.SelectedValue = ds.Tables(0).Rows(0).Item("ReplacesCurrentVendor").ToString()
                    txtReasonForAddition.Text = ds.Tables(0).Rows(0).Item("ReasonForAddition").ToString()
                    ddStatus.SelectedValue = ds.Tables(0).Rows(0).Item("RecStatus").ToString()
                    lblRoutingStatusDesc.Text = ds.Tables(0).Rows(0).Item("RoutingStatusDesc").ToString()
                    txtRoutingStatus.Text = ds.Tables(0).Rows(0).Item("RoutingStatus").ToString()
                    txtVoidReason.Text = ds.Tables(0).Rows(0).Item("VoidReason").ToString()

                    If ds.Tables(0).Rows(0).Item("ReplacesVendorNo") IsNot System.DBNull.Value Then
                        ddVendor.SelectedValue = ds.Tables(0).Rows(0).Item("ReplacesVendorNo").ToString()
                    End If

                    If ds.Tables(0).Rows(0).Item("FamilyID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("FamilyID") > 0 Then
                            ddFamily.SelectedValue = ds.Tables(0).Rows(0).Item("FamilyID")
                            lblFamily.Text = ds.Tables(0).Rows(0).Item("FamilyID")
                            'filter subfamily dropdown choices if a family exists
                            Dim dsFamily As DataSet
                            Dim iFamilyID As Integer = 0

                            If ddFamily.SelectedIndex > 0 Then
                                iFamilyID = ddFamily.SelectedValue
                            End If

                            dsFamily = commonFunctions.GetSubFamily(iFamilyID)
                            If commonFunctions.CheckDataSet(dsFamily) = True Then
                                ddSubFamily.DataSource = dsFamily
                                ddSubFamily.DataTextField = dsFamily.Tables(0).Columns("subFamilyName").ColumnName
                                ddSubFamily.DataValueField = dsFamily.Tables(0).Columns("subFamilyID").ColumnName
                                ddSubFamily.DataBind()
                                ddSubFamily.Items.Insert(0, "")
                            End If

                        End If
                    End If

                    If ds.Tables(0).Rows(0).Item("SubFamilyID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("SubFamilyID") > 0 Then
                            ddSubFamily.SelectedValue = ds.Tables(0).Rows(0).Item("SubFamilyID")
                        End If
                    End If

                    ''Default values on Documents tab
                    If ViewState("pDocID") <> 0 Then
                        ds = SUPModule.GetSupplierRequestDocuments(ViewState("pSUPNo"), ViewState("pDocID"))
                        If (ds.Tables.Item(0).Rows.Count > 0) Then
                            ddTeamMember.SelectedValue = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                            ddTeamMember.Enabled = False
                            txtFormName.Text = ds.Tables(0).Rows(0).Item("FormName").ToString()
                            txtSRFID.Text = ds.Tables(0).Rows(0).Item("SRFID").ToString()
                        Else 'no record found reset query string pRptID
                            Response.Redirect("SupplierRequest.aspx?pSUPNo=" & ViewState("pSUPNo") & "&pDocID=0", False)
                        End If
                    End If 'EOF If ViewState("pDocID") <> 0 Then
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

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click, btnSave2.Click
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim VTypeDesc As String = Nothing

            lblErrors.Text = Nothing
            lblErrors.Visible = False

            If cbNew.Checked = False And cbChange.Checked = False Then
                lblReqNewOrChange.Text = "< Select an option."
                lblReqNewOrChange.Visible = True
            Else
                lblReqNewOrChange.Text = ""
                lblReqNewOrChange.Visible = False

                If (ViewState("pSUPNo") <> Nothing Or ViewState("pSUPNo") <> "") Then
                    '***************
                    '* Update Data
                    '***************
                    UpdateRecord(ddStatus.SelectedValue, IIf(ddStatus.SelectedValue = "Void", "V", IIf(ddStatus.SelectedValue = "Closed", "C", txtRoutingStatus.Text)))

                    '**************
                    '* Reload the data
                    '**************
                    BindData(ViewState("pSUPNo"))

                    ''*************
                    ''Check Void status send email notfication 
                    ''*************
                    If ddStatus.SelectedValue = "Void" And txtRoutingStatus.Text = "V" Then
                        ''*****************
                        ''History Tracking
                        ''*****************
                        SUPModule.InsertSupplierRequestHistory(ViewState("pSUPNo"), txtVendorName.Text, DefaultTMID, "Void - Reason:" & txtVoidReason.Text)
                        If txtRoutingStatus.Text <> "N" Then
                            SendNotifWhenEventChanges("Void")
                        End If
                    End If

                    If txtRoutingStatus.Text = "N" Then
                        GoToBuildApproval(True)
                        If ddVendorType.SelectedValue <> lblVendorType.Text Then
                            GoToBuildSD(True)
                        End If
                    End If

                    '**************
                    '* Reload the data
                    '**************
                    BindData(ViewState("pSUPNo"))
                    CheckRights()

                Else
                    If ddVendorType.SelectedValue = "INVU" Then
                        VTypeDesc = ddINVU.SelectedValue
                    End If

                    If ddVendorType.SelectedValue = "MROU" Then
                        VTypeDesc = ddMROU.SelectedValue
                    End If

                    '***************
                    '* Check that the Supplier entered is not a duplicate entry, if so, kick user out with warning
                    '***************
                    'Check against the Supplier module
                    Dim dsSup As DataSet = New DataSet
                    dsSup = SUPModule.GetSupplierRequestSearch("", 0, "", txtVendorName.Text, "", "", "", "", "", "", "")
                    If commonFunctions.CheckDataSet(dsSup) = True Then
                        lblErrors.Text = "Save Cancelled - Supplier Name is a Duplicate Entry see Ref# " & dsSup.Tables(0).Rows(0).Item("SUPNo").ToString
                        lblErrors.Visible = True
                        MaintainScrollPositionOnPostBack = False
                        Exit Sub
                    End If

                    '***************
                    '* Save Data
                    '***************
                    SUPModule.InsertSupplierRequest(ddVendorType.SelectedValue, VTypeDesc, txtVendorName.Text, IIf(txtVendorNo.Text = Nothing, 0, txtVendorNo.Text), IIf(ddInBPCS.SelectedValue = Nothing, 0, ddInBPCS.SelectedValue), cbTen99.Checked, txtPhone.Text, txtProdDesc.Text, ddRequestedBy.SelectedValue, "", cbUT.Checked, cbUN.Checked, cbUP.Checked, cbUR.Checked, cbUS.Checked, cbUW.Checked, cbOH.Checked, cbNew.Checked, cbChange.Checked, txtSalesContactName.Text, txtSalesFax.Text, txtAcctContact.Text, txtAcctPhone.Text, txtAcctFax.Text, txtRemitToAddr1.Text, txtRemitToAddr2.Text, txtRemitToAddr3.Text, txtRemitToAddr4.Text, txtRemitCity.Text, txtRemitState.Text, txtRemitZip.Text, ddRemitToCountry.SelectedValue, txtCustServContact.Text, txtCustServPhone.Text, txtCustServFax.Text, txtCustServEmail.Text, txtShipFromAddr1.Text, txtShipFromAddr2.Text, txtShipFromAddr3.Text, txtShipFromAddr4.Text, txtShipFromCity.Text, txtShipFromState.Text, txtShipFromZip.Text, ddShipFromCountry.SelectedValue, IIf(ddTerms.SelectedValue = Nothing, 0, ddTerms.SelectedValue), ddPayType.SelectedValue, IIf(txtInitialPurchaseAmt.Text = Nothing, 0, txtInitialPurchaseAmt.Text), IIf(txtEstAmtAnnualPurchase.Text = Nothing, 0, txtEstAmtAnnualPurchase.Text), IIf(ddReplacesCurrentVendor.SelectedValue = Nothing, 0, ddReplacesCurrentVendor.SelectedValue), IIf(ddVendor.SelectedValue = Nothing, 0, ddVendor.SelectedValue), txtReasonForAddition.Text, "N", "New Entry", IIf(ddFamily.SelectedValue = Nothing, 0, ddFamily.SelectedValue), IIf(ddSubFamily.SelectedValue = Nothing, 0, ddSubFamily.SelectedValue), IIf(ddContractorOnSite.SelectedValue = "", 0, ddContractorOnSite.SelectedValue), IIf(ddFutureVendor.SelectedValue = "", 0, ddFutureVendor.SelectedValue), DefaultUser, DefaultDate)

                    '***************
                    '* Locate Next available ProjectNo based on Facility selection
                    '***************
                    Dim ds As DataSet = Nothing
                    ds = SUPModule.GetLastSupplierRequestNo(ddRequestedBy.SelectedValue, txtVendorName.Text, txtProdDesc.Text, "N", DefaultUser, DefaultDate)

                    ViewState("pSUPNo") = CType(ds.Tables(0).Rows(0).Item("LastSUPNO").ToString, Integer)

                    ''*****************
                    ''History Tracking
                    ''*****************
                    SUPModule.InsertSupplierRequestHistory(ViewState("pSUPNo"), txtVendorName.Text, DefaultTMID, "Record created.")
                    Dim pForm As String = ViewState("pForm")
                    Select Case pForm
                        Case "EXPKG"
                            SUPModule.InsertSupplierRequestHistory(ViewState("pSUPNo"), txtVendorName.Text, DefaultTMID, "Requested from Packaging Expenditure " & ViewState("pProjNo"))
                        Case "PURIOR"
                            SUPModule.InsertSupplierRequestHistory(ViewState("pSUPNo"), txtVendorName.Text, DefaultTMID, "Requested from Internal Order Request " & ViewState("pIORNo"))
                    End Select

                    ''*****
                    ''GO TO BUILD: SUPPORTING DOCS AND APPROVAL CHAIN
                    ''*******
                    GoToBuildApproval(True)
                    GoToBuildSD(True)

                    '***************
                    '* Redirect user back to the page.
                    '***************
                    Dim goBackForm As String = Nothing
                    Select Case pForm
                        Case "EXPKG"
                            goBackForm = "&pForm=" & ViewState("pForm") & "&pProjNo=" & ViewState("pProjNo")
                        Case "PURIOR"
                            goBackForm = "&pForm=" & ViewState("pForm") & "&pIORNo=" & ViewState("pIORNo")
                    End Select

                    Dim Aprv As String = Nothing
                    If ViewState("pAprv") = 1 Then
                        Aprv = "&pAprv=1"
                    End If

                    Response.Redirect("SupplierRequest.aspx?pSUPNo=" & ViewState("pSUPNo") & goBackForm & Aprv, False)
                End If 'EOF IF (ViewState("pSUPNo") <> Nothing Or ViewState("pSUPNo") <> "") Then
            End If 'EOF  IF cbNew.Checked = False And cbChange.Checked = False then

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

    Protected Sub btnReset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset1.Click, btnReset2.Click, btnReset3.Click
        ''Reprompt current page
        Dim Aprv As String = Nothing
        If ViewState("pAprv") = 1 Then
            Aprv = "&pAprv=1"
        End If

        Dim goBackForm As String = Nothing
        Select Case ViewState("pForm")
            Case "EXPKG"
                goBackForm = "&pForm=" & ViewState("pForm") & "&pProjNo=" & ViewState("pProjNo")
            Case "PURIOR"
                goBackForm = "&pForm=" & ViewState("pForm") & "&pIORNo=" & ViewState("pIORNo")
        End Select

        Dim TempViewState As Integer
        If ViewState("pSUPNo") <> Nothing Then
            TempViewState = mvTabs.ActiveViewIndex
            mvTabs.GetActiveView()
            mnuTabs.Items(TempViewState).Selected = True

            BindData(ViewState("pSUPNo"))
        Else
            Response.Redirect("SupplierRequest.aspx?pSUPNo=" & ViewState("pSUPNo") & goBackForm & Aprv, False)
        End If

    End Sub 'EOF btnReset1_Click

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("SupplierLookUp.aspx?sBtnSrch=False&pForm=SUPPLIER", False)
    End Sub 'EOF btnAdd_Click

    Public Function UpdateRecord(ByVal RecStatus As String, ByVal RoutingStatus As String) As String
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

            Dim VTypeDesc As String = Nothing
            If ddVendorType.SelectedValue = "INVU" Then
                VTypeDesc = ddINVU.SelectedValue
            ElseIf ddVendorType.SelectedValue = "MROU" Then
                VTypeDesc = ddMROU.SelectedValue
            End If

            SUPModule.UpdateSupplierRequest(ViewState("pSUPNo"), ddVendorType.SelectedValue, VTypeDesc, txtVendorName.Text, IIf(txtVendorNo.Text = Nothing, 0, txtVendorNo.Text), IIf(ddInBPCS.SelectedValue = Nothing, 0, ddInBPCS.SelectedValue), cbTen99.Checked, txtPhone.Text, txtProdDesc.Text, ddRequestedBy.SelectedValue, IIf(txtRoutingStatus.Text = "N", DefaultDate, lblDateSubmitted.Text), cbUT.Checked, cbUN.Checked, cbUP.Checked, cbUR.Checked, cbUS.Checked, cbUW.Checked, cbOH.Checked, cbNew.Checked, cbChange.Checked, txtSalesContactName.Text, txtSalesFax.Text, txtAcctContact.Text, txtAcctPhone.Text, txtAcctFax.Text, txtRemitToAddr1.Text, txtRemitToAddr2.Text, txtRemitToAddr3.Text, txtRemitToAddr4.Text, txtRemitCity.Text, txtRemitState.Text, txtRemitZip.Text, ddRemitToCountry.SelectedValue, txtCustServContact.Text, txtCustServPhone.Text, txtCustServFax.Text, txtCustServEmail.Text, txtShipFromAddr1.Text, txtShipFromAddr2.Text, txtShipFromAddr3.Text, txtShipFromAddr4.Text, txtShipFromCity.Text, txtShipFromState.Text, txtShipFromZip.Text, ddShipFromCountry.SelectedValue, IIf(ddTerms.SelectedValue = Nothing, 0, ddTerms.SelectedValue), ddPayType.SelectedValue, IIf(txtInitialPurchaseAmt.Text = Nothing, 0, txtInitialPurchaseAmt.Text), IIf(txtEstAmtAnnualPurchase.Text = Nothing, 0, txtEstAmtAnnualPurchase.Text), IIf(ddReplacesCurrentVendor.SelectedValue = Nothing, 0, ddReplacesCurrentVendor.SelectedValue), IIf(ddVendor.SelectedValue = Nothing, 0, ddVendor.SelectedValue), txtReasonForAddition.Text, RoutingStatus, RecStatus, IIf(ddFamily.SelectedValue = Nothing, 0, ddFamily.SelectedValue), IIf(ddSubFamily.SelectedValue = Nothing, 0, ddSubFamily.SelectedValue), IIf(ddContractorOnSite.SelectedValue = "", 0, ddContractorOnSite.SelectedValue), txtVoidReason.Text, IIf(ddFutureVendor.SelectedValue = "", 0, ddFutureVendor.SelectedValue), DefaultUser, DefaultDate)
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
            If ViewState("pSUPNo") <> Nothing Then
                SUPModule.DeleteSupplierRequest(ViewState("pSUPNo"))

                '***************
                '* Redirect user back to the search page.
                '***************
                Dim pForm As String = ViewState("pForm")
                Select Case pForm
                    Case "EXPKG"
                        Response.Redirect("~/EXP/PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pEV=1", False)
                    Case "PURIOR"
                        Response.Redirect("~/PUR/InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo"), False)
                    Case Else
                        Response.Redirect("SupplierRequestList.aspx", False)
                End Select
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

    Protected Sub ddVendorType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddVendorType.SelectedIndexChanged

        ddINVU.Visible = False
        ddMROU.Visible = False
        ddINVU.SelectedValue = Nothing
        ddMROU.SelectedValue = Nothing

        If ddVendorType.SelectedValue = "INVU" Then
            ddINVU.Visible = True
            ddMROU.SelectedValue = Nothing
        ElseIf ddVendorType.SelectedValue = "MROU" Then
            ddMROU.Visible = True
            ddINVU.SelectedValue = Nothing
        End If

        If ddVendorType.SelectedValue <> "INVU" Then
            lblComPri.Visible = False
            lblFamily.Text = Nothing
            ddFamily.Enabled = False
            ddFamily.SelectedValue = Nothing
            rfvFamily.Enabled = False
            ddSubFamily.Enabled = False
            ddSubFamily.SelectedValue = Nothing
        Else
            lblComPri.Visible = True
            ddFamily.Enabled = True
            rfvFamily.Enabled = True
            ddSubFamily.Enabled = True
        End If

        If ddVendorType.SelectedValue = "SERU" Then
            ddContractorOnSite.Enabled = True
            lblReqContractorOnSite.Visible = True
            rfvContractorOnSite.Enabled = True
        Else
            ddContractorOnSite.Enabled = False
            lblReqContractorOnSite.Visible = False
            rfvContractorOnSite.Enabled = False
        End If

        ''bind existing data to drop down Department or Cost Center control for selection criteria for search
        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetVendor(0, "", "", "", "", "", "", "", IIf(ddVendorType.SelectedValue = Nothing, 0, ddVendorType.SelectedValue))
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddVendor.DataSource = ds
            ddVendor.DataTextField = ds.Tables(0).Columns("ddVNDNAMcombo").ColumnName.ToString()
            ddVendor.DataValueField = ds.Tables(0).Columns("Vendor").ColumnName.ToString()
            ddVendor.DataBind()
            ddVendor.Items.Insert(0, "")
        End If

        If ViewState("pSUPNo") <> Nothing Then
            GoToBuildSD(True)
            GoToBuildApproval(True)
        End If
    End Sub 'EOF ddVendorType_SelectedIndexChanged

    Protected Sub ddFamily_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFamily.SelectedIndexChanged
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Department or Cost Center control for selection criteria for search
            ds = commonFunctions.GetSubFamily(IIf(ddFamily.SelectedValue = Nothing, 0, ddFamily.SelectedValue))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSubFamily.DataSource = ds
                ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName.ToString()
                ddSubFamily.DataBind()
                ddSubFamily.Items.Insert(0, "")
            End If

            If ViewState("pSUPNo") <> Nothing Then
                GoToBuildApproval(True)
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
    End Sub 'EOF ddFamily_SelectedIndexChanged

    Protected Sub ddReplacesCurrentVendor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddReplacesCurrentVendor.SelectedIndexChanged

        If ddReplacesCurrentVendor.SelectedValue = True Then
            ddVendor.Enabled = True
            lblReqVendor.Visible = True
            rfvReqVendor.Enabled = True
        Else
            ddVendor.Enabled = False
            lblReqVendor.Visible = False
            rfvReqVendor.Enabled = False
            ddVendor.SelectedValue = Nothing
        End If
    End Sub 'EOF ddReplacesCurrentVendor_SelectedIndexChanged

    Protected Sub ddStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddStatus.SelectedIndexChanged
        Try
            If ddStatus.SelectedValue = "Void" Then
                txtVoidReason.Enabled = True
                txtVoidReason.Visible = True
                lblVoidReason.Visible = True
                rfvVoidReason.Enabled = True
                lblReqVoidReason.Visible = True
            Else
                txtVoidReason.Enabled = False
                txtVoidReason.Visible = False
                lblVoidReason.Visible = False
                rfvVoidReason.Enabled = False
                lblReqVoidReason.Visible = False
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
    End Sub 'EOF ddStatus_SelectedIndexChanged

    Protected Sub cbSameAsRemitToAddr_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbSameAsRemitToAddr.CheckedChanged

        If cbSameAsRemitToAddr.Checked = True And txtRemitToAddr1.Text <> Nothing Then
            txtShipFromAddr1.Text = txtRemitToAddr1.Text
            txtShipFromAddr2.Text = txtRemitToAddr2.Text
            txtShipFromCity.Text = txtRemitCity.Text
            txtShipFromState.Text = txtRemitState.Text
            txtShipFromZip.Text = txtRemitZip.Text
            ddShipFromCountry.SelectedValue = ddRemitToCountry.SelectedValue
        Else
            txtShipFromAddr1.Text = Nothing
            txtShipFromAddr2.Text = Nothing
            txtShipFromCity.Text = Nothing
            txtShipFromState.Text = Nothing
            txtShipFromZip.Text = Nothing
            ddShipFromCountry.SelectedValue = Nothing
        End If
    End Sub 'EOF cbSameAsRemitToAddr_CheckedChanged
#End Region 'EOF General

#Region "Return to Form Page used by Supplier Look Up"
    Protected Function GoBackToForm(ByVal DocID As Integer) As String

        Dim strReturnValue As String = "White"
        Dim goBackForm As String = Nothing
        Select Case ViewState("pForm")
            Case "EXPKG"
                goBackForm = "&pForm=" & ViewState("pForm") & "&pProjNo=" & ViewState("pProjNo")
            Case "PURIOR"
                goBackForm = "&pForm=" & ViewState("pForm") & "&pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo")
        End Select

        strReturnValue = "SupplierRequest.aspx?pDocID=" & DocID & "&pSUPNo=" & ViewState("pSUPNo") & goBackForm

        GoBackToForm = strReturnValue

    End Function 'EOF GoBackToForm
#End Region 'EOF Return to Form Page used by Supplier Look Up

#Region "Supporting Document"
    Protected Sub gvSupportingDocument_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSupportingDocument.RowDataBound
        '***
        'This section provides the user with the popup for confirming the delete of a record.
        'Called by the onClientClick event.
        '***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(5).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As Supplier.Supplier_Request_DocumentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Supplier.Supplier_Request_DocumentsRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record (" & DataBinder.Eval(e.Row.DataItem, "FormName") & ")?');")
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

            Dim goBackForm As String = Nothing
            Select Case ViewState("pForm")
                Case "EXPKG"
                    goBackForm = "&pForm=" & ViewState("pForm") & "&pProjNo=" & ViewState("pProjNo")
                Case "PURIOR"
                    goBackForm = "&pForm=" & ViewState("pForm") & "&pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo")
            End Select

            Response.Redirect("SupplierRequest.aspx?pSUPNo=" & ViewState("pSUPNo") & "&pSD=1" & goBackForm & Aprv, False)
        End If
    End Sub 'EOF gvSupportingDocument_RowCommand

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim DefaultDate As Date = Date.Now
            lblMessageView4.Text = Nothing
            lblMessageView4.Visible = False
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblReqAppComments.Text = Nothing
            lblReqAppComments.Visible = False

            If ViewState("pSUPNo") <> "" Then
                If uploadFile.HasFile Then
                    If uploadFile.PostedFile.ContentLength <= 3500000 Then
                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFile.FileName)
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

                                If ViewState("pDocID") = 0 Then
                                    ''***************
                                    '' Insert Record
                                    ''***************
                                    SUPModule.InsertSupplierRequestDocuments(ViewState("pSUPNo"), IIf(txtSRFID.Text = Nothing, 0, txtSRFID.Text), ddTeamMember.SelectedValue, txtFormName.Text, SupportingDocBinaryFile, uploadFile.FileName, SupportingDocEncodeType, SupportingDocFileSize)
                                Else
                                    ''***************
                                    '' Update Record
                                    ''***************
                                    SUPModule.UpdateSupplierRequestDocuments(ViewState("pSUPNo"), ViewState("pDocID"), IIf(txtSRFID.Text = Nothing, 0, txtSRFID.Text), ddTeamMember.SelectedValue, txtFormName.Text, SupportingDocBinaryFile, uploadFile.FileName, SupportingDocEncodeType, SupportingDocFileSize)
                                End If
                            End If
                            gvSupportingDocument.DataBind()
                            revUploadFile.Enabled = False
                            txtFormName.Text = Nothing
                            txtSRFID.Text = Nothing
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
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnUpload_Click

    Protected Function DisplayDeleteBtn(ByVal SRFID As Integer) As String
        If SRFID = 0 Then
            Return True
        Else
            Return False
        End If

    End Function 'EOF DisplayDeleteBtn

    Private Sub GoToBuildSD(ByVal Build As Boolean)
        Try
            If Build = True Then

                ''***************
                ''* Locate Supplier Required Forms based on Vendor Type selection
                ''***************
                Dim ds As DataSet = Nothing
                Dim SRFID As Integer = Nothing
                Dim FormName As String = Nothing
                Dim i As Integer = 0

                If (txtRoutingStatus.Text = "N") Or (ddStatus.SelectedValue = "New Entry") Then
                    ''********************
                    ''Delete Existing List
                    ''********************
                    SUPModule.DeleteSupplierRequestDocuments(0, ViewState("pSUPNo"))

                    '***************
                    '* Default Supplier Required Forms based on Vendor Type selection
                    '***************
                    ds = SUPModule.GetSupplierRequiredForms("", ddVendorType.SelectedValue, False)
                    If ds.Tables.Count > 0 And (ds.Tables.Item(0).Rows.Count > 0) Then
                        For i = 0 To ds.Tables.Item(0).Rows.Count - 1
                            SRFID = ds.Tables(0).Rows(i).Item("SRFID").ToString
                            FormName = ds.Tables(0).Rows(i).Item("FormName").ToString
                            SUPModule.InsertSupplierRequestDocuments(ViewState("pSUPNo"), SRFID, ddTeamMember.SelectedValue, FormName, Nothing, "", "", 0)
                        Next
                    End If

                    gvSupportingDocument.DataBind()
                End If

            End If 'EOF If Build = True then

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF GoToBuildSD

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

#Region "Approval Status"
    Private Sub GoToBuildApproval(ByVal Build As Boolean)
        Try
            If Build = True Then
                ''********
                ''* This function is used to build the Approval List
                ''********
                Dim DefaultDate As Date = Date.Now
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

                lblErrors.Text = Nothing
                lblErrors.Visible = False
                lblReqAppComments.Text = Nothing
                lblReqAppComments.Visible = False

                If (txtRoutingStatus.Text = "N") Or (ddStatus.SelectedValue = "New Entry") Then
                    ''***************
                    ''* Delete 1st Level Approval for rebuild
                    ''***************
                    SUPModule.DeleteSupplierRequestApproval(ViewState("pSUPNo"))

                    '***************
                    '* Build Approval List
                    '***************
                    SUPModule.InsertSupplierRequestApproval(ViewState("pSUPNo"), ddRequestedBy.SelectedValue, IIf(ddFamily.SelectedValue = Nothing, 0, ddFamily.SelectedValue), DefaultUser, DefaultDate)

                End If
            End If 'EOF If Build = True then

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF GoToBuildApproval

    Protected Sub gvApprovers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApprovers.RowDataBound
        Try
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
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF gvApprovers_RowDataBound

    Protected Sub gvApprovers_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Try
            lblErrors.Text = ""
            lblErrors.Visible = False
            lblReqAppComments.Text = "'"
            lblReqAppComments.Visible = False
            lblInBPCS.Visible = False
            rfvInBPCS.Visible = False
            lblVendorNo.Visible = False
            rfvVendorNo.Visible = False

            Dim row As GridViewRow = gvApprovers.Rows(e.RowIndex)
            If row IsNot Nothing Then
                Dim DefaultDate As Date = Date.Now
                Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

                Dim t As DropDownList = TryCast(row.FindControl("ddStatus"), DropDownList)
                Dim c As TextBox = TryCast(row.FindControl("txtAppComments"), TextBox)
                Dim tm As TextBox = TryCast(row.FindControl("txtTeamMemberID"), TextBox)
                Dim TeamMemberID As Integer = CType(tm.Text, Integer)
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
                        'Dim VTypeDesc As String = Nothing
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

                        ''********
                        ''* Only users with valid email accounts can send an email.
                        ''********
                        If CurrentEmpEmail <> Nothing And ViewState("pSUPNo") <> Nothing Then
                            ''***************
                            ''Verify that all required documents has been uploaded prior to submission
                            ''***************
                            Dim dsDoc As DataSet = New DataSet
                            Dim a As Integer = 0
                            Dim ReqFormFound As Boolean = False
                            dsDoc = SUPModule.GetSupplierRequestDocuments(ViewState("pSUPNo"), 0)
                            If commonFunctions.CheckDataSet(dsDoc) = False Then 'If missing kick user out from submission.
                                ReqFormFound = False
                            Else 'Value is true
                                For a = 0 To dsDoc.Tables.Item(0).Rows.Count - 1
                                    If dsDoc.Tables(0).Rows(a).Item("RequiredForm") = True And dsDoc.Tables(0).Rows(a).Item("BinaryFound") = False Then
                                        ReqFormFound = False
                                    Else
                                        ReqFormFound = True
                                    End If
                                Next
                            End If 'EOF If commonFunctions.CheckDataSet(dsDoc) = False Then

                            If ReqFormFound = True Then
                                ''***********
                                ''* Verify that Row selected Team Member Sequence No is Last to Approve
                                ''***********
                                Dim dsLast As DataSet = New DataSet
                                Dim r As Integer = 0
                                Dim LastSeqNo As Boolean = False
                                Dim totalApprovers As Integer = 0
                                dsLast = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), 0, 0, False, False)
                                If commonFunctions.CheckDataSet(dsLast) = True Then
                                    For r = 0 To dsLast.Tables.Item(0).Rows.Count - 1
                                        totalApprovers = totalApprovers + 1
                                        If totalApprovers <= hfSeqNo Then
                                            LastSeqNo = True
                                        Else
                                            LastSeqNo = False
                                        End If
                                    Next
                                End If

                                If (ViewState("iCorpAcctTMID") = TeamMemberID Or ViewState("iCorpAcctMgrTMID") = TeamMemberID) And (LastSeqNo = True) Then
                                    If txtVendorNo.Text = Nothing And ddInBPCS.SelectedValue <> "Yes" Then
                                        lblInBPCS.Visible = True
                                        rfvInBPCS.Visible = True
                                        lblVendorNo.Visible = True
                                        rfvVendorNo.Visible = True

                                        lblErrors.Text = "Supplier No. Assigned and Supplier Created On BPCS fields are required."
                                        lblErrors.Visible = True
                                        lblErrors.Font.Size = 12
                                        lblReqAppComments.Text = "Supplier No. Assigned and Supplier Created On BPCS fields are required."
                                        lblReqAppComments.Visible = True
                                        MaintainScrollPositionOnPostBack = False
                                        Exit Sub
                                    End If
                                End If

                                ''**********************
                                ''* Save data prior to submission
                                ''**********************
                                UpdateRecord(IIf(LastSeqNo = True, IIf(t.SelectedValue = "Rejected", "In Process", "Approved"), "In Process"), IIf(LastSeqNo = True, IIf(t.SelectedValue = "Rejected", "R", "A"), IIf(t.SelectedValue = "Rejected", "R", "T")))

                                ''***********************************
                                ''Update Current Level Approver record.
                                ''***********************************
                                SUPModule.UpdateSupplierRequestApproval(ViewState("pSUPNo"), TeamMemberID, True, t.SelectedValue, c.Text, hfSeqNo, 0, DefaultUser, DefaultDate)

                                ''*******************************
                                ''Locate Next Approver
                                ''*******************************
                                ''Check at same sequence level
                                ds1st = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), hfSeqNo, 0, True, False)
                                If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                    ''Do not send email at same level twice.
                                Else
                                    If t.SelectedValue = "Approved" Then
                                        ds2nd = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), IIf(LastSeqNo = False, (hfSeqNo + 1), hfSeqNo), 0, True, False)
                                        If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                                            For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                                If (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                                                (ddRequestedBy.SelectedValue <> ds2nd.Tables(0).Rows(i).Item("TeamMemberID")) Then
                                                    '(ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                                    EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                                    EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                                    ''*****************************************
                                                    ''Update Approvers DateNotified field.
                                                    ''*****************************************
                                                    SUPModule.UpdateSupplierRequestApproval(ViewState("pSUPNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", IIf(LastSeqNo = False, (hfSeqNo + 1), hfSeqNo), 0, DefaultUser, DefaultDate)
                                                End If
                                            Next
                                        End If 'EOF ds2nd.Tables.Count > 0 
                                    End If 'EOF t.SelectedValue <> "Rejected"

                                    ''********************************************************
                                    ''Notify Requestor if Rejected or last approval
                                    ''********************************************************
                                    If t.SelectedValue = "Rejected" Or (LastSeqNo = True And t.SelectedValue = "Approved") Then
                                        dsRej = SecurityModule.GetTeamMember(ddRequestedBy.SelectedValue, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                                        ''Check that the recipient(s) is a valid Team Member
                                        If dsRej.Tables.Count > 0 And (dsRej.Tables.Item(0).Rows.Count > 0) Then
                                            For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                                If (dsRej.Tables(0).Rows(i).Item("Working") = True) And _
                                                (dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then

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

                                    If (LastSeqNo = True And t.SelectedValue = "Approved") Then
                                        ''**************************************
                                        ''* Notify all involved
                                        ''**************************************
                                        dsCC = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), 0, 0, False, False)
                                        ''Check that the recipient(s) is a valid Team Member
                                        If commonFunctions.CheckDataSet(dsCC) = True Then
                                            For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                                                If (dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                                (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                                    EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"
                                                End If 'EOF If (dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) 
                                            Next
                                        End If 'EOF CC All Involved

                                        If ddVendorType.SelectedValue = "INVU" Then
                                            ''**************************************
                                            ''*Carbon Copy the Supplier Dev. Mgr
                                            ''**************************************
                                            dsCC = commonFunctions.GetTeamMemberBySubscription(96)
                                            ''Check that the recipient(s) is a valid Team Member
                                            If commonFunctions.CheckDataSet(dsCC) = True Then
                                                For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                                                    If ((dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Or _
                                                        (dsCC.Tables(0).Rows(i).Item("Email") <> EmailCC)) And _
                                                        (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                                        EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"

                                                    End If
                                                Next
                                            End If 'EOF CC Supplier Dev. Mgr
                                        End If 'EOF  If ddVendorType.SelectedValue = "INVU" Then

                                        If txtVendorNo.Text <> Nothing Then
                                            EmailCC &= "Vendorap@ugnauto.com" & ";"
                                        End If
                                    End If 'EOF (LastSeqNo = True And t.SelectedValue = "Approved") Then

                                    ''Test or Production Message display
                                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                                        MyMessage.Subject = "TEST: "
                                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                                    Else
                                        MyMessage.Subject = ""
                                        MyMessage.Body = ""
                                        'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
                                    End If

                                    MyMessage.Subject &= "New Supplier Request - " & txtVendorName.Text

                                    MyMessage.Body &= EmpName

                                    If LastSeqNo = True Then
                                        MyMessage.Subject &= " - COMPLETED"
                                        MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & txtVendorName.Text & "' is Completed by all. "
                                        MyMessage.Body &= " <a href='" & ViewState("strProdOrTestEnvironment") & "/SUP/SupplierRequest.aspx?pSUPNo=" & ViewState("pSUPNo") & "'>Click here</a> to access the record.</p>"
                                    Else
                                        MyMessage.Body &= "<p>'" & txtVendorName.Text & "' is available for your Review/Approval. "
                                        MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/SUP/crSupplierRequestApproval.aspx?pSUPNo=" & ViewState("pSUPNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"
                                    End If

                                    EmailBody(MyMessage)


                                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                                        MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                                        MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                                        EmailFrom = "Database.Notifications@ugnauto.com"
                                        EmailTO = "lynette.rey@ugnauto.com" 'CurrentEmpEmail 
                                        EmailCC = "lynette.rey@ugnauto.com"
                                    End If

                                    ''*****************
                                    ''History Tracking
                                    ''*****************
                                    SUPModule.InsertSupplierRequestHistory(ViewState("pSUPNo"), txtVendorName.Text, DefaultTMID, (DefaultUser.ToUpper) & " " & t.Text & " this record on behalf of " & ShortName.ToUpper & " - Comments: " & c.Text)

                                    ''**********************************
                                    ''Connect & Send email notification
                                    ''**********************************
                                    Try
                                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Supplier Request", ViewState("pSUPNo"))
                                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."

                                        lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."
                                    Catch ex As SmtpException
                                        lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                        lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                                        UGNErrorTrapping.InsertEmailQueue("Supplier Request Ref#: " & ViewState("pSUPNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                    End Try
                                    lblReqAppComments.Visible = True
                                    lblReqAppComments.ForeColor = Color.Red
                                    lblErrors.Visible = True
                                    lblErrors.Font.Size = 12
                                    MaintainScrollPositionOnPostBack = False

                                End If 'EOF IF EmailTO <> Nothing Then
                            End If 'EOF If ReqFormFound = True Then
                        End If 'EOF If CurrentEmpEmail <> Nothing Then

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData(ViewState("pSUPNo"))
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
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

            Dim ds1st As DataSet = New DataSet
            Dim ds2nd As DataSet = New DataSet
            Dim dsCC As DataSet = New DataSet
            Dim EmpName As String = Nothing
            Dim EmailTO As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing
            Dim i As Integer = 0

            Dim CurrentEmpEmail As String = Nothing
            If HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value IsNot Nothing Then
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
            lblMessageView4.Text = Nothing
            lblMessageView4.Visible = False

            ''*************************
            ''*Rebuild Approval Chain
            ''*************************
            If txtRoutingStatus.Text = "N" Then
                GoToBuildApproval(True)
            End If

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pSUPNo") <> Nothing Then
                ''***************
                ''Verify that all required documents has been uploaded prior to submission
                ''***************
                Dim dsDoc As DataSet = New DataSet
                Dim a As Integer = 0
                Dim ReqFormFound As Boolean = False
                dsDoc = SUPModule.GetSupplierRequestDocuments(ViewState("pSUPNo"), 0)
                If commonFunctions.CheckDataSet(dsDoc) = False Then 'If missing kick user out from submission.
                    ReqFormFound = False
                Else 'Value is true
                    For a = 0 To dsDoc.Tables.Item(0).Rows.Count - 1
                        If dsDoc.Tables(0).Rows(a).Item("RequiredForm") = True And dsDoc.Tables(0).Rows(a).Item("BinaryFound") = 1 Then
                            ReqFormFound = True
                        ElseIf dsDoc.Tables(0).Rows(a).Item("RequiredForm") = True And dsDoc.Tables(0).Rows(a).Item("BinaryFound") = 0 Then
                            ReqFormFound = False
                            lblErrors.Text = "Supplier Forms Required prior to submission."
                            lblErrors.Visible = True
                            lblErrors.Font.Size = 12
                            lblReqAppComments.Text = "Supplier Forms Required prior to submission."
                            lblReqAppComments.Visible = True
                            lblReqAppComments.ForeColor = Color.Red
                            lblMessageView4.Text = "Select the Form in the table below with a Required check box set to true and upload the document."
                            lblMessageView4.Visible = True
                            mvTabs.ActiveViewIndex = Int32.Parse(2)
                            mvTabs.GetActiveView()
                            mnuTabs.Items(2).Selected = True
                            MaintainScrollPositionOnPostBack = False
                            Exit Sub
                        End If
                    Next a
                End If 'EOF If commonFunctions.CheckDataSet(dsDoc) = False Then

                '***************
                '* Verify Supplier Contact Info is entered.
                '****************
                Dim ReqContactInfo As Boolean = False
                If (txtSalesContactName.Text = Nothing) Or (txtSalesFax.Text = Nothing) Or (txtPhone.Text = Nothing) Or (txtAcctContact.Text = Nothing) Or (txtAcctPhone.Text = Nothing) Or (txtRemitToAddr1.Text = Nothing) Or (txtRemitCity.Text = Nothing) Or (txtRemitState.Text = Nothing) Or (txtRemitZip.Text = Nothing) Or (txtShipFromAddr1.Text = Nothing) Or (txtShipFromCity.Text = Nothing) Or (txtShipFromState.Text = Nothing) Or (txtShipFromZip.Text = Nothing) Then
                    ReqContactInfo = True
                End If

                If ReqContactInfo = False Then 'All required fields entered
                    If ReqFormFound = True Then
                        ''**********************
                        ''* Save data prior to submission
                        ''**********************
                        UpdateRecord("In Process", "T")

                        ''*******************************
                        ''Notify 1st level approver
                        ''*******************************
                        If (txtRoutingStatus.Text <> "R") Then
                            ds1st = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), 1, 0, False, False)
                        Else 'IF Rejected - only notify the TM who Rejected the record
                            ds1st = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), 0, 0, False, True)
                        End If

                        ''Check that the recipient(s) is a valid Team Member
                        If commonFunctions.CheckDataSet(ds1st) = True Then
                            For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                                If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                (ds1st.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                                (ddRequestedBy.SelectedValue <> ds1st.Tables(0).Rows(i).Item("TeamMemberID")) Then

                                    EmailTO &= ds1st.Tables(0).Rows(i).Item("Email") & ";"
                                    EmpName &= ds1st.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                    ''************************************************************
                                    ''Update 1st level DateNotified field.
                                    ''************************************************************
                                    If (txtRoutingStatus.Text <> "R") Then
                                        SUPModule.UpdateSupplierRequestApproval(ViewState("pSUPNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", 1, 0, DefaultUser, DefaultDate)
                                    Else 'IF Rejected - only notify the TM who Rejected the record
                                        SUPModule.UpdateSupplierRequestApproval(ViewState("pSUPNo"), ds1st.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", ds1st.Tables(0).Rows(i).Item("SeqNo"), 0, DefaultUser, DefaultDate)
                                    End If
                                End If 'EOF If (ds1st.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail)
                            Next 'EOF For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                        End If 'EOF If commonFunctions.CheckDataset(ds1st) = True 

                        ''********************************************************
                        ''Send Notification only if there is a valid Email Address
                        ''********************************************************
                        If EmailTO <> Nothing Then
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

                            MyMessage.Subject &= "New Supplier Request - " & txtVendorName.Text

                            MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName
                            MyMessage.Body &= "<p>'" & txtVendorName.Text & "' is available for your Review/Approval. <a href='" & ViewState("strProdOrTestEnvironment") & "/SUP/crSupplierRequestApproval.aspx?pSUPNo=" & ViewState("pSUPNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"
                            MyMessage.Body &= "</font>"

                            EmailBody(MyMessage)

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
                            SUPModule.InsertSupplierRequestHistory(ViewState("pSUPNo"), txtVendorName.Text, DefaultTMID, "Record completed and forwarded to " & EmpName & " for approval.")

                            ''**********************************
                            ''Connect & Send email notification
                            ''**********************************
                            Try
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Supplier Request", ViewState("pSUPNo"))
                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                lblReqAppComments.Text = "Notification sent to " & EmpName & " successfully."
                            Catch ex As SmtpException
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                lblReqAppComments.Text = "Email Notification to " & EmpName & " is queued for the next automated release."
                                UGNErrorTrapping.InsertEmailQueue("Supplier Request Ref#: " & ViewState("pSUPNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                            End Try

                            lblErrors.Visible = True
                            lblErrors.Font.Size = 12
                            lblReqAppComments.Visible = True
                            lblReqAppComments.ForeColor = Color.Red
                            MaintainScrollPositionOnPostBack = False
                        End If 'EOF  If EmailTO <> Nothing Then

                        '***************
                        '* Redirect user back to the search page.
                        '***************
                        Dim pForm As String = ViewState("pForm")
                        Select Case pForm
                            Case "EXPKG"
                                Response.Redirect("~/EXP/PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pEV=1&pVTp=" & ddVendorType.SelectedValue & "&pVNo=" & ViewState("pSUPNo") & "&pNF=1", False)
                            Case "PURIOR"
                                Response.Redirect("~/PUR/InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pVTp=" & ddVendorType.SelectedValue & "&pVNo=" & ViewState("pSUPNo") & "&pProjNo=" & ViewState("pProjNo") & "&pNF=1", False)
                            Case Else
                                ''**********************************
                                ''Rebind the data to the form
                                ''********************************** 
                                BindData(ViewState("pSUPNo"))
                                gvApprovers.DataBind()

                                ''*************************************************
                                '' "Form Level Security using Roles &/or Subscriptions"
                                ''*************************************************
                                CheckRights() '"Form Level Security using Roles &/or Subscriptions"
                        End Select

                    Else
                        lblErrors.Text = "Supplier Forms Required prior to submission."
                        lblErrors.Visible = True
                        lblReqAppComments.Text = "Supplier Forms Required prior to submission."
                        lblReqAppComments.Visible = True
                        lblReqAppComments.ForeColor = Color.Red

                        mvTabs.ActiveViewIndex = Int32.Parse(2)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(2).Selected = True
                    End If 'EOF IF ReqFormFound = True Then
                Else
                    lblErrors.Text = "Supplier Contact Info Required prior to submission."
                    lblErrors.Visible = True
                    lblReqAppComments.Text = "Supplier Contact Info Required prior to submission."
                    lblReqAppComments.Visible = True
                    lblReqAppComments.ForeColor = Color.Red

                    mvTabs.ActiveViewIndex = Int32.Parse(1)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(1).Selected = True
                End If 'EOF IF ReqContactInfo = False Then

            End If 'EOF If HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value <> Nothing Then

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

    Public Sub SendNotifWhenEventChanges(ByVal EventDesc As String)
        ''********
        ''* This section will be used in the following methods:
        ''*     1) Email sent to all involved when an Asset is Tooling Completed
        ''*     2) Email sent to all involved with an Asset is VOID
        ''*     3) Email sent to Account with an Asset is COMPLETED
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
        Dim x As Integer = 0
        Dim z As Integer = 0
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
            If CurrentEmpEmail <> Nothing And ViewState("pSUPNo") <> Nothing Then
                ''********************************************************
                ''Notify Everyone
                ''********************************************************
                ds2nd = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), 0, 0, False, False)
                If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                    For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                        If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                        (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then ''change to DefaultTMID   

                            EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                            EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                        End If
                    Next
                End If 'EOF   If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) 

                ''********************************************************
                ''Send Notification only if there is a valid Email Address
                ''********************************************************
                If EmailTO <> Nothing Then
                    Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                    ''**************************************
                    ''*Carbon Copy the Requestor
                    ''**************************************
                    dsCC = SecurityModule.GetTeamMember(ddRequestedBy.SelectedValue, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(dsCC) = True Then
                        For z = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                            If (dsCC.Tables(0).Rows(z).Item("Working") = True) And _
                            (dsCC.Tables(0).Rows(z).Item("Email") <> CurrentEmpEmail) Then

                                EmailCC &= dsCC.Tables(0).Rows(z).Item("Email") & ";"

                            End If
                        Next
                    End If 'EOF If commonFunctions.CheckDataSet(dsCC) = True Then

                    ''**************************************
                    ''*Carbon Copy the Supplier Dev. Mgr
                    ''**************************************
                    dsCC = commonFunctions.GetTeamMemberBySubscription(96)
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(dsCC) = True Then
                        For x = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                            If ((dsCC.Tables(0).Rows(x).Item("Email") <> CurrentEmpEmail) Or _
                                (dsCC.Tables(0).Rows(x).Item("Email") <> EmailCC)) And _
                                (dsCC.Tables(0).Rows(x).Item("WorkStatus") = True) Then

                                EmailCC &= dsCC.Tables(0).Rows(x).Item("Email") & ";"

                            End If
                        Next
                    End If 'EOF CC Supplier Dev. Mgr

                    ''Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                    Else
                        MyMessage.Subject = ""
                        MyMessage.Body = ""
                        'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
                    End If

                    MyMessage.Subject &= "Supplier Request - " & txtVendorName.Text & " - VOIDED"

                    MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"

                    MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>'" & EventDesc.ToUpper & "' by " & DefaultUserName & ".</strong></td>"

                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td class='p_text' align='right' style='width: 150px;'>Reference No:&nbsp;&nbsp; </td>"
                    MyMessage.Body &= "<td> <a href='" & ViewState("strProdOrTestEnvironment") & "/SUP/SupplierRequest.aspx?pProjNo=" & ViewState("pSUPNo") & "'>" & ViewState("pSUPNo") & "</a></td>"
                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td class='p_text' align='right'>Supplier Name:&nbsp;&nbsp; </td>"
                    MyMessage.Body &= "<td>" & txtVendorName.Text & "</td>"
                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "</tr>"
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td class='p_text' align='right'>Vendor Type:&nbsp;&nbsp; </td>"
                    MyMessage.Body &= "<td>" & ddVendorType.SelectedItem.Text & "</td>"
                    MyMessage.Body &= "</tr>"

                    Select Case EventDesc
                        Case "Void" 'Sent by Project Leader, notify all
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Void Reason:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td style='width: 600px;'>" & txtVoidReason.Text & "</td>"
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
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Supplier Request", ViewState("pSUPNo"))
                        lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                    Catch ex As SmtpException
                        lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                        UGNErrorTrapping.InsertEmailQueue("Supplier Request Ref#: " & ViewState("pSUPNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                    End Try

                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    MaintainScrollPositionOnPostBack = False

                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    BindData(ViewState("pSUPNo"))
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

    Public Function EmailBody(ByVal MyMessage As MailMessage) As String

        MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>SUPPLIER OVERVIEW</strong></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' valign='top'>&nbsp;</td>"
        MyMessage.Body &= "<td style='width: 600px;'>" & IIf(cbNew.Checked = True, "[X] New Vendor", "[ ] New Vendor") & " " & IIf(cbChange.Checked = True, "[X] Change to current vendor", "[  ] Change to current vendor") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Requestor:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddRequestedBy.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Reference No:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("pSUPNo") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Supplier Name:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtVendorName.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Product Description:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtProdDesc.Text & "</td>"
        MyMessage.Body &= "</tr>"

        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' valign='top'>UGN Location:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td style='width: 600px;'>" & IIf(cbUT.Checked = True, "[X] Tinley Park, IL", "[  ] Tinley Park, IL") & " " & IIf(cbUN.Checked = True, "[X] Chicago Heights, IL", "[  ] Chicago Heights, IL") & " " & IIf(cbUP.Checked = True, "[X] Jackson, TN", "[  ] Jackson, TN") & " " & IIf(cbUR.Checked = True, "[X] Somerset, KY", "[ ] Somerset, KY") & " " & IIf(cbUS.Checked = True, "[X] Valparaiso, IN", "[  ] Valparaiso, IN") & " " & IIf(cbOH.Checked = True, "[X] Monroe, OH", "[  ] Monroe, OH") & " " & IIf(cbUW.Checked = True, "[X] Silao, MX", "[ ] Silao, MX") & "</td>"
        MyMessage.Body &= "</tr>"

        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Vendor Type:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ddVendorType.SelectedItem.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Reason for New Supplier Addition:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtReasonForAddition.Text & "</td>"
        MyMessage.Body &= "</tr>"

        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Supplier Created in Oracle:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & IIf(ddInBPCS.SelectedValue = False, "[  ] Yes   [ X ] No", "[ X ] Yes   [   ] No") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>1099?:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & IIf(cbTen99.Checked = False, "[  ] Yes   [ X ] No", "[ X ] Yes   [   ] No") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Supplier No Assigned:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & txtVendorNo.Text & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"


        ''***************************************************
        ''Get list of Supporting Documentation
        ''***************************************************
        Dim dsAED As DataSet
        dsAED = SUPModule.GetSupplierRequestDocuments(ViewState("pSUPNo"), 0)
        If dsAED.Tables.Count > 0 And (dsAED.Tables.Item(0).Rows.Count > 0) Then
            MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
            MyMessage.Body &= "<td colspan='2'><strong>REQUIRED FORMS / DOCUMENTS:</strong></td>"
            MyMessage.Body &= "</tr>"
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td colspan='2'>"
            MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
            MyMessage.Body &= "  <tr>"
            MyMessage.Body &= "   <td width='250px'><b>Form Description</b></td>"
            MyMessage.Body &= "   <td width='250px'>&nbsp;</td>"
            MyMessage.Body &= "</tr>"
            For i = 0 To dsAED.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td height='25'>" & dsAED.Tables(0).Rows(i).Item("FormName") & "</td>"
                MyMessage.Body &= "<td height='25'><a href='" & ViewState("strProdOrTestEnvironment") & "/SUP/SupplierRequestDocument.aspx?pSUPNo=" & ViewState("pSUPNo") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
                MyMessage.Body &= "</tr>"
            Next
            MyMessage.Body &= "</table>"
            MyMessage.Body &= "</tr>"
        End If
        MyMessage.Body &= "</table>"

        Return True

    End Function 'EOF EmailBody

#End Region 'EOF Email Notification

    Protected Sub txtVendorName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVendorName.TextChanged
        lblErrors.Text = Nothing
        lblErrors.Visible = False

        '***************
        '* Check that the Supplier entered is not a duplicate entry, if so, kick user out with warning
        '***************
        'Check against the Supplier module
        Dim dsSup As DataSet = New DataSet
        dsSup = SUPModule.GetSupplierRequestSearch("", 0, "", txtVendorName.Text, "", "", "", "", "", "", "")
        If commonFunctions.CheckDataSet(dsSup) = True Then
            lblErrors.Text = "Save Cancelled - Supplier Name is a Duplicate Entry see Ref# " & dsSup.Tables(0).Rows(0).Item("SUPNo").ToString
            lblErrors.Visible = True
            MaintainScrollPositionOnPostBack = False
            Exit Sub
        End If

    End Sub 'EOF txtVendorName_TextChanged
End Class
