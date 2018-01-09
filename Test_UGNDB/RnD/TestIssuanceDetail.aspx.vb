' ************************************************************************************************
' Name:	TestIssuanceDetail.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 02/24/2009    LRey			Created .Net application
' 07/09/2010    RCarlson        Modified: Ref# (RD-2930) - allow PDF files in which both UPPER case and lower case PDF extentions can be used
' 08/16/2010    RCarlson        Modified: Updated Parameters to GetFormula in BindCriteria
' 09/21/2011    RCarlson        Modified: Adjusted ddDesignationType_SelectedIndexChanged if ds returns nothing
' 12/04/2012    RCarlson        Modified: Added Produect Development Subscrtion (5) to parameter of commonfunctions.GetTeamMemberByWorkFlowAssignments function
' 01/17/2013    RCarlson        Modified: Changed DMS Drawing Dropdown to textbox with validation on save
' 02/26/2013    RCarlson        Modified: Make sure DMS DrawingNo is valid
' 01/30/2014  LRey    Replaced SoldTo|CABBV with a RowID next sequential. 
'                     Added CostSheetID per RD-3267 support request.
' ************************************************************************************************
Partial Class RnD_TID
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            If HttpContext.Current.Request.QueryString("pReqID") <> "" Then
                ViewState("pReqID") = CType(HttpContext.Current.Request.QueryString("pReqID"), Integer)
            Else
                ViewState("pReqID") = 0
            End If

            If HttpContext.Current.Request.QueryString("pReqCategory") <> "" Then
                ViewState("pReqCategory") = CType(HttpContext.Current.Request.QueryString("pReqCategory"), Integer)
            Else
                ViewState("pReqCategory") = 0
            End If

            If HttpContext.Current.Request.QueryString("pRptID") <> "" Then
                ViewState("pRptID") = HttpContext.Current.Request.QueryString("pRptID")
            Else
                ViewState("pRptID") = 0
            End If

            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            Select Case ViewState("pReqCategory")
                Case 1
                    m.ContentLabel = "Product Innovation"
                    ViewState("ReqCategorDesc") = "Product Innovation"
                Case 2
                    m.ContentLabel = "Current Mass Production Part"
                    ViewState("ReqCategorDesc") = "Current Mass Production Part"
                Case 3
                    m.ContentLabel = "Consultation"
                    ViewState("ReqCategorDesc") = "Current Mass Production Part"
                Case 4
                    m.ContentLabel = "New Program Launch"
                    ViewState("ReqCategorDesc") = "New Program Launch"
            End Select

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)

                Select Case ViewState("pReqCategory")
                    Case 1
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > <a href='TestIssuanceList.aspx'><b>Test Issuance Search</b></a> > Product Innovation"
                        lbl.Visible = True
                    Case 2
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > <a href='TestIssuanceList.aspx'><b>Test Issuance Search</b></a> > Current Mass Production Part"
                        lbl.Visible = True
                    Case 3
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > <a href='TestIssuanceList.aspx'><b>Test Issuance Search</b></a> > Consultation"
                        lbl.Visible = True
                    Case 4
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > <a href='TestIssuanceList.aspx'><b>Test Issuance Search</b></a> > New Program Launch"
                        lbl.Visible = True

                End Select
            End If

            ctl = m.FindControl("SiteMapPath1")
            If ctl IsNot Nothing Then
                Dim smp As SiteMapPath = CType(ctl, SiteMapPath)
                smp.Visible = False
            End If

            ''******************************************
            '' Expand this Master Page menu item
            ''******************************************
            ctl = m.FindControl("RnDExtender")
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
                If ViewState("pRptID") = Nothing Then
                    mvTabs.ActiveViewIndex = Int32.Parse(0)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(0).Selected = True
                ElseIf ViewState("pRptID") >= 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(4)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(4).Selected = True
                    lblMessageView4.Text = Nothing
                    lblMessageView4.Visible = False
                End If

                If ViewState("pReqID") <> 0 Then
                    BindData()
                Else
                    txtSampleProdDesc.Focus()
                    txtTodaysDate.Text = Date.Today
                End If

                InitializeAllPopUps()

            End If

            txtDescReqTesting.Attributes.Add("onkeypress", "return tbLimit();")
            txtDescReqTesting.Attributes.Add("onkeyup", "return tbCount(" + lblDescReqTesting.ClientID + ");")
            txtDescReqTesting.Attributes.Add("maxLength", "2000")

            txtPartAppMkt.Attributes.Add("onkeypress", "return tbLimit();")
            txtPartAppMkt.Attributes.Add("onkeyup", "return tbCount(" + lblPartAppMkt.ClientID + ");")
            txtPartAppMkt.Attributes.Add("maxLength", "800")

            txtObjPerfTargets.Attributes.Add("onkeypress", "return tbLimit();")
            txtObjPerfTargets.Attributes.Add("onkeyup", "return tbCount(" + lblObjPerfTargets.ClientID + ");")
            txtObjPerfTargets.Attributes.Add("maxLength", "800")

            txtMiscAgenda.Attributes.Add("onkeypress", "return tbLimit();")
            txtMiscAgenda.Attributes.Add("onkeyup", "return tbCount(" + lblMiscAgenda.ClientID + ");")
            txtMiscAgenda.Attributes.Add("maxLength", "800")

            txtObjective.Attributes.Add("onkeypress", "return tbLimit();")
            txtObjective.Attributes.Add("onkeyup", "return tbCount(" + lblObjective.ClientID + ");")
            txtObjective.Attributes.Add("maxLength", "250")

            txtStatusNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtStatusNotes.Attributes.Add("onkeyup", "return tbCount(" + lblStatusNotes.ClientID + ");")
            txtStatusNotes.Attributes.Add("maxLength", "500")

            txtTestDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtTestDesc.Attributes.Add("onkeyup", "return tbCount(" + lblTestDesc.ClientID + ");")
            txtTestDesc.Attributes.Add("maxLength", "400")

            txtAssessment.Attributes.Add("onkeypress", "return tbLimit();")
            txtAssessment.Attributes.Add("onkeyup", "return tbCount(" + lblAssessment.ClientID + ");")
            txtAssessment.Attributes.Add("maxLength", "800")

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        Try
            ''*******
            '' Disable controls by default
            ''*******
            btnAdd.Enabled = False
            btnSave1.Enabled = False
            btnSave2.Enabled = False
            btnReset1.Enabled = False
            btnReset2.Enabled = False
            btnDelete.Enabled = False
            btnSubmit1.Enabled = False
            btnNotify.Enabled = False
            btnResponse.Enabled = False
            btnAddtoGrid.Enabled = False
            mnuTabs.Items(1).Enabled = False
            mnuTabs.Items(2).Enabled = False
            mnuTabs.Items(3).Enabled = False
            mnuTabs.Items(4).Enabled = False
            ddRequestStatus.Visible = False
            lblRequestStatus.Visible = False
            ViewState("Admin") = False
            gvCustomerPart.Columns(9).Visible = False
            gvTMAssignments.Columns(2).Visible = False
            gvTMAssignments.FooterRow.Visible = False
            gvTestReport.Columns(7).Visible = False

            ViewState("ObjectRole") = False


            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 56 'Test Issuance Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Barry.Barretto", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)

                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                                Select Case iRoleID
                                    Case 11 '*** UGNAdmin: Full Access
                                        ViewState("Admin") = True
                                        btnSave1.Enabled = True
                                        btnSubmit1.Enabled = True
                                        btnAddtoGrid.Enabled = True
                                        gvCustomerPart.Columns(9).Visible = True
                                        btnReset1.Enabled = True
                                        btnAdd.Enabled = True
                                        btnDelete.Enabled = True
                                        If ViewState("SentToRnD") <> Nothing Then
                                            btnSave2.Enabled = True
                                            btnReset2.Enabled = True
                                            btnDelete.Enabled = True
                                            btnNotify.Enabled = True
                                        End If
                                        ddRequestStatus.Visible = True
                                        lblRequestStatus.Visible = False
                                        gvTMAssignments.Columns(2).Visible = True
                                        gvTMAssignments.FooterRow.Visible = True
                                        gvTestReport.Columns(7).Visible = True
                                        ViewState("ObjectRole") = True
                                        ''*************************************************
                                        ''for new test requests, disable all but the first tab
                                        ''*************************************************
                                        If (ViewState("pReqID") = 0) Then
                                            txtSampleProdDesc.Focus()
                                        Else
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True

                                            mnuTabs.Items(3).Enabled = True
                                            mnuTabs.Items(4).Enabled = True
                                        End If
                                        If ddRequestStatus.SelectedValue = "Completed" Then
                                            btnResponse.Enabled = True
                                        End If
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        If ViewState("SentToRnD") = Nothing Then
                                            btnSave1.Enabled = True
                                            btnReset1.Enabled = True
                                            btnSubmit1.Enabled = True
                                            btnAddtoGrid.Enabled = True
                                            gvCustomerPart.Columns(9).Visible = True
                                        End If
                                        btnAdd.Enabled = True
                                        btnDelete.Enabled = True
                                        lblRequestStatus.Visible = True
                                        ViewState("ObjectRole") = True
                                        ''*************************************************
                                        ''for new test requests, disable all but the first tab
                                        ''*************************************************
                                        If (ViewState("pReqID") = 0) Then
                                            txtSampleProdDesc.Focus()
                                        Else
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                            mnuTabs.Items(3).Enabled = True
                                            mnuTabs.Items(4).Enabled = True
                                        End If
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        If ViewState("SentToRnD") = Nothing Then
                                            btnSave1.Enabled = True
                                            btnReset1.Enabled = True
                                            btnSubmit1.Enabled = True
                                            btnAddtoGrid.Enabled = True
                                            gvCustomerPart.Columns(9).Visible = True
                                        End If
                                        btnAdd.Enabled = True
                                        lblRequestStatus.Visible = True
                                        ViewState("ObjectRole") = True
                                        ''*************************************************
                                        ''for new test requests, disable all but the first tab
                                        ''*************************************************
                                        If (ViewState("pReqID") = 0) Then
                                            txtSampleProdDesc.Focus()
                                        Else
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                            mnuTabs.Items(3).Enabled = True
                                            mnuTabs.Items(4).Enabled = True
                                        End If
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        lblRequestStatus.Visible = True
                                        mnuTabs.Items(1).Enabled = True
                                        mnuTabs.Items(2).Enabled = True
                                        mnuTabs.Items(3).Enabled = True
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        lblRequestStatus.Visible = True
                                        mnuTabs.Items(1).Enabled = True
                                        mnuTabs.Items(2).Enabled = True
                                        mnuTabs.Items(3).Enabled = True
                                        mnuTabs.Items(4).Enabled = True
                                        gvTestReport.Columns(7).Visible = True
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        lblRequestStatus.Visible = True
                                        mnuTabs.Items(1).Enabled = True
                                        mnuTabs.Items(2).Enabled = True
                                        mnuTabs.Items(3).Enabled = True
                                End Select 'EOF of "Select Case iRoleID"
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"

            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                Dim FullName As String = commonFunctions.getUserName()
                Dim UserEmailAddress As String = FullName & "@ugnusa.com"
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
#End Region 'EOF "Form Level Security using Roles &/or Subscriptions"

#Region "General"
    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Sample Issuer control for selection criteria for search
            ds = commonFunctions.GetTeamMember("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSampleIssuer.DataSource = ds
                ddSampleIssuer.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddSampleIssuer.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddSampleIssuer.DataBind()
                ddSampleIssuer.Items.Insert(0, "")
            End If

            ''bind existing data to drop down ReportIssuer Issuer control for selection criteria for search
            ds = commonFunctions.GetTeamMember("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddReportIssuer.DataSource = ds
                ddReportIssuer.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddReportIssuer.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddReportIssuer.DataBind()
                ddReportIssuer.Items.Insert(0, "")
            End If

            commonFunctions.UserInfo()
            ddSampleIssuer.SelectedValue = HttpContext.Current.Session("UserId")
            ddReportIssuer.SelectedValue = HttpContext.Current.Session("UserId")

            'bind existing data to drop down Department control for selection criteria for search
            ds = commonFunctions.GetPurchasedGood("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddPurchasedGood.DataSource = ds
                ddPurchasedGood.DataTextField = ds.Tables(0).Columns("PurchasedGoodName").ColumnName.ToString()
                ddPurchasedGood.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName.ToString()
                ddPurchasedGood.DataBind()
                ddPurchasedGood.Items.Insert(0, "")
            End If

            ''bind existing data to drop down UGN Location control for selection criteria for search
            ds = commonFunctions.GetUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Commodity control for selection criteria 
            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddCommodity.DataSource = ds
                ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityName").ColumnName.ToString()
                ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddCommodity.DataBind()
                ddCommodity.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Commodity control for selection criteria 
            ds = CostingModule.GetFormula(0)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddFormula.DataSource = ds
                ddFormula.DataTextField = ds.Tables(0).Columns("ddFormulaName").ColumnName.ToString()
                ddFormula.DataValueField = ds.Tables(0).Columns("FormulaID").ColumnName
                ddFormula.DataBind()
                ddFormula.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Test Classification control for selection criteria for search
            ds = RnDModule.GetTestingClassification("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddTestClass.DataSource = ds
                ddTestClass.DataTextField = ds.Tables(0).Columns("TestClassName").ColumnName.ToString()
                ddTestClass.DataValueField = ds.Tables(0).Columns("TestClassID").ColumnName.ToString()
                ddTestClass.DataBind()
                ddTestClass.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Priority control for selection criteria for search
            ds = RnDModule.GetTestRequestPriority("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddPriority.DataSource = ds
                ddPriority.DataTextField = ds.Tables(0).Columns("ddPriorityDescription").ColumnName.ToString()
                ddPriority.DataValueField = ds.Tables(0).Columns("PID").ColumnName.ToString()
                ddPriority.DataBind()
                ddPriority.Items.Insert(0, "")
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

    Public Sub BindData()
        Dim ds As DataSet = New DataSet

        Try
            ds = RnDModule.GetTestIssuanceRequests(ViewState("pReqID"), "", 0, "", 0, "", "", 0, "", "", 0, 0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                lblTestIssuanceReq.Text = "Cust-TI-Req-" & ds.Tables(0).Rows(0).Item("RequestID").ToString()
                txtSampleProdDesc.Text = ds.Tables(0).Rows(0).Item("SampleProductDescription").ToString()
                lblRequestStatus.Text = ds.Tables(0).Rows(0).Item("RequestStatus").ToString()
                cbACReq.Checked = ds.Tables(0).Rows(0).Item("ReqAcoustic").ToString()
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("ProjectID")) Then
                    hlnkACProjNo.NavigateUrl = "~/Acoustic/Acoustic_Project_Detail.aspx?pProjID=" & ds.Tables(0).Rows(0).Item("ProjectID").ToString()
                    hlnkACProjNo.Text = ds.Tables(0).Rows(0).Item("ProjectID").ToString()
                    hlnkACProjNo.Font.Underline = True
                    hlnkACProjNo.ForeColor = Color.Blue
                Else
                    hlnkACProjNo.Visible = False
                End If
                ddRequestStatus.SelectedValue = ds.Tables(0).Rows(0).Item("RequestStatus").ToString()
                ddCommodity.SelectedValue = ds.Tables(0).Rows(0).Item("CommodityID").ToString()
                If ds.Tables(0).Rows(0).Item("PurchasedGoodID") IsNot System.DBNull.Value Then
                    ddPurchasedGood.SelectedValue = ds.Tables(0).Rows(0).Item("PurchasedGoodID").ToString()
                End If
                txtGeneralThickness.Text = ds.Tables(0).Rows(0).Item("GeneralThickness").ToString()
                ddUGNFacility.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                txtSampleQty.Text = ds.Tables(0).Rows(0).Item("SampleQuantity").ToString()
                ddSampleIssuer.SelectedValue = ds.Tables(0).Rows(0).Item("SampleIssuerTMID").ToString()
                txtDepartment.Text = ds.Tables(0).Rows(0).Item("SampleIssuerTMDept").ToString()
                txtTestCmpltDt.Text = ds.Tables(0).Rows(0).Item("TestCmpltDate").ToString()
                txtPartAppMkt.Text = ds.Tables(0).Rows(0).Item("PartApplicationMrkt").ToString()
                txtObjPerfTargets.Text = ds.Tables(0).Rows(0).Item("ObjectivePerfTarget").ToString()
                txtMiscAgenda.Text = ds.Tables(0).Rows(0).Item("MiscAgendaOutStndItems").ToString()
                txtDescReqTesting.Text = ds.Tables(0).Rows(0).Item("DescRequiredTesting").ToString()
                txtEstAnnCostSav.Text = ds.Tables(0).Rows(0).Item("EstAnnualCostSavings").ToString()
                If ds.Tables(0).Rows(0).Item("FormulaID") IsNot System.DBNull.Value Then
                    ddFormula.SelectedValue = ds.Tables(0).Rows(0).Item("FormulaID").ToString()
                End If
                txtObjective.Text = ds.Tables(0).Rows(0).Item("Objective").ToString()
                txtStatusNotes.Text = ds.Tables(0).Rows(0).Item("Status").ToString()
                txtEstManHrs.Text = ds.Tables(0).Rows(0).Item("EstManHrsRem").ToString()
                ddDrawReview.SelectedValue = ds.Tables(0).Rows(0).Item("DrawRevOccur").ToString()
                If (ds.Tables(0).Rows(0).Item("TestClassID") IsNot System.DBNull.Value) And (ds.Tables(0).Rows(0).Item("TestClassID") <> 0) Then
                    ddTestClass.SelectedValue = ds.Tables(0).Rows(0).Item("TestClassID").ToString()
                End If
                txtLongestAgingCycle.Text = ds.Tables(0).Rows(0).Item("LongAgingCycle").ToString()
                txtTAG.Text = ds.Tables(0).Rows(0).Item("TAG").ToString()
                txtStartDt.Text = ds.Tables(0).Rows(0).Item("StartDate").ToString()
                txtProjCmplDt.Text = ds.Tables(0).Rows(0).Item("ProjectedCmpltDate").ToString()
                txtActCmplDt.Text = ds.Tables(0).Rows(0).Item("CompletionDate").ToString()
                If ds.Tables(0).Rows(0).Item("PriorityID") IsNot System.DBNull.Value Then
                    ddPriority.SelectedValue = ds.Tables(0).Rows(0).Item("PriorityID").ToString()
                End If

                If ds.Tables(0).Rows(0).Item("SentToRnD").ToString() <> Nothing Or ds.Tables(0).Rows(0).Item("SentToRnD").ToString() <> "" Then
                    ViewState("SentToRnD") = ds.Tables(0).Rows(0).Item("SentToRnD")
                    If ViewState("Admin") = False Then
                        btnSave1.Enabled = False
                        btnReset1.Enabled = False
                        btnSubmit1.Enabled = False
                        btnAddtoGrid.Enabled = False
                    End If
                    txtTodaysDate.Text = ds.Tables(0).Rows(0).Item("SentToRnD").ToString()
                Else
                    txtTodaysDate.Text = ds.Tables(0).Rows(0).Item("RequestDate").ToString()
                End If

                If ds.Tables(0).Rows(0).Item("LongAgingCycle").ToString() <> 0 Then
                    lblLACdays.Text = CType((txtLongestAgingCycle.Text / 24), Integer) & " day(s)"
                    lblLACdays.Visible = True
                End If
            End If

           

            '*************
            ''* Check that the Appropriation entered is a valid entry in SQL
            ''*************
            If (ds.Tables(0).Rows(0).Item("ProjectNo").ToString() <> Nothing) Then

                txtAppropriation.Text = ds.Tables(0).Rows(0).Item("ProjectNo").ToString()
                ds = PURModule.GetInternalOrderRequestCapEx(0, txtAppropriation.Text)
                If commonFunctions.CheckDataSet(ds) = True Then
                    txtProjectTitle.Text = ds.Tables(0).Rows(0).Item("ProjectTitle")
                    hplkAppropriation.Text = ds.Tables(0).Rows(0).Item("ProjectTitle")
                    txtProjectStatus.Text = ds.Tables(0).Rows(0).Item("ProjectStatus")
                    txtDefinedCapex.Text = ds.Tables(0).Rows(0).Item("DefinedCapEx")
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
                End If 'EOF  
            End If

            If ViewState("pRptID") <> 0 Then
                ds = RnDModule.GetTestReport(ViewState("pReqID"), ViewState("pRptID"))
                If (ds.Tables.Item(0).Rows.Count > 0) Then
                    lblTestReportNo.Text = ds.Tables(0).Rows(0).Item("TestReportID").ToString()
                    lblTestReportNo.Visible = True
                    ddReportIssuer.SelectedValue = ds.Tables(0).Rows(0).Item("TeamMemberID").ToString()
                    txtTestDesc.Text = ds.Tables(0).Rows(0).Item("TestDescription").ToString()
                    txtAssessment.Text = ds.Tables(0).Rows(0).Item("Assessment").ToString()
                    If ds.Tables(0).Rows(0).Item("FileName").ToString() <> Nothing Then
                        txtFileName.Text = ds.Tables(0).Rows(0).Item("FileName").ToString()
                        rfvUpload.Visible = False
                    End If
                Else 'no record found reset query string pRptID
                    Response.Redirect("TestIssuanceDetail.aspx?" & RnDModule.QueryStringParam.pReqId.ToString & "=" & ViewState("pReqID") & "&" & RnDModule.QueryStringParam.pReqCategory.ToString & "=" & ViewState("pReqCategory") & "&pRptID=0", False)
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
    End Sub 'EOF BindData

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click, btnSave2.Click
        Try

            Dim DefaultDate As Date = Date.Today
            Dim ValidEntry As Boolean = True
            Dim RequestStatus As String = Nothing
            If ddRequestStatus.Visible = False Then
                RequestStatus = lblRequestStatus.Text
            Else
                RequestStatus = ddRequestStatus.SelectedValue
            End If
            lblErrors.Text = Nothing
            lblErrors.Visible = False


            If ViewState("pReqID") <> 0 Then
                '***************
                '* Requestoer will Update Data and submit to R&D
                '***************
                UpdateRecord(RequestStatus, False)

            Else
                If ddCommodity.SelectedItem.Text.Substring(0, 2) = "**" Then
                    lblErrors.Text = "Commodity selection is OBSOLETE. "
                    lblErrors.Visible = True
                    ddCommodity.Focus()
                    ValidEntry = False
                End If
                If ddPurchasedGood.SelectedValue <> Nothing Then
                    If ddPurchasedGood.SelectedItem.Text.Substring(0, 2) = "**" Then
                        lblErrors.Text = lblErrors.Text & "Purchased Good selection is OBSOLETE. "
                        lblErrors.Visible = True
                        ddPurchasedGood.Focus()
                        ValidEntry = False
                    End If
                End If
                If lblErrors.Text <> Nothing Then
                    lblErrors.Text = lblErrors.Text & " Please choose a valid option."
                End If

                If ValidEntry = True Then
                    Dim NewTestReq As Boolean = False
                    Dim Consult As Boolean = False
                    Dim Current As Boolean = False

                    '***************
                    '* Save Data
                    '***************
                    RnDModule.InsertTestIssuanceRequests(ViewState("pReqCategory"), txtSampleProdDesc.Text, IIf(ddCommodity.SelectedValue = "", 0, ddCommodity.SelectedValue), IIf(ddPurchasedGood.SelectedValue = Nothing, 0, ddPurchasedGood.SelectedValue), txtGeneralThickness.Text, ddUGNFacility.SelectedValue, txtSampleQty.Text, ddSampleIssuer.SelectedValue, txtDepartment.Text, txtTodaysDate.Text, txtTestCmpltDt.Text, txtPartAppMkt.Text, txtObjPerfTargets.Text, txtMiscAgenda.Text, txtDescReqTesting.Text, IIf(ddFormula.SelectedValue = "", 0, ddFormula.SelectedValue), ViewState("DefaultUser"), DefaultDate, cbACReq.Checked, txtAppropriation.Text)

                    '***************
                    '* Locate Max RequestID
                    '***************
                    Dim ds As DataSet = Nothing
                    ds = RnDModule.GetLastRequestID(ViewState("pReqCategory"), txtSampleProdDesc.Text, ddSampleIssuer.SelectedValue, ddUGNFacility.SelectedValue, txtTodaysDate.Text, txtTestCmpltDt.Text, txtDescReqTesting.Text, ViewState("DefaultUser"), DefaultDate)

                    ViewState("pReqID") = ds.Tables(0).Rows(0).Item("LastRequestID").ToString

                    '***************
                    '* Redirect user back to the page.
                    '***************
                    Response.Redirect("TestIssuanceDetail.aspx?" & _
                             RnDModule.QueryStringParam.pReqId.ToString & "=" & ViewState("pReqID") & "&" & _
                             RnDModule.QueryStringParam.pReqCategory.ToString & "=" & ViewState("pReqCategory"), False)
                End If 'EOF  If ValidEntry = True Then
            End If 'EOF  If ViewState("pReqID") <> 0 Then

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnSave1_Click

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("TestIssuanceNew.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnReset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset1.Click, btnReset2.Click
        Response.Redirect("TestIssuanceDetail.aspx?" & _
                        RnDModule.QueryStringParam.pReqId.ToString & "=" & ViewState("pReqID") & "&" & _
                        RnDModule.QueryStringParam.pReqCategory.ToString & "=" & ViewState("pReqCategory"), False)
    End Sub 'EOF btnReset1_Click

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            '*****
            '* Delete Record
            '*****
            RnDModule.DeleteTestIssuanceRequests(ViewState("pReqID"))

            Response.Redirect("TestIssuanceList.aspx", False)
        Catch ex As Exception
            lblErrors.Text = "Error occurred during delete record.  Please contact the IS Application Group." & ex.Message
            lblErrors.Visible = "True"
        End Try
    End Sub 'EOF btnDelete_Click

    Public Function UpdateRecord(ByVal RecStatus As String, ByVal RecSubmitted As Boolean) As String
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

            RnDModule.UpdateTestIssuanceRequests(ViewState("pReqID"), ViewState("pReqCategory"), RecStatus, txtSampleProdDesc.Text, ddCommodity.SelectedValue, IIf(ddPurchasedGood.SelectedValue = Nothing, 0, ddPurchasedGood.SelectedValue), txtGeneralThickness.Text, ddUGNFacility.SelectedValue, txtSampleQty.Text, ddSampleIssuer.SelectedValue, txtDepartment.Text, txtTodaysDate.Text, txtTestCmpltDt.Text, txtPartAppMkt.Text, txtObjPerfTargets.Text, txtMiscAgenda.Text, txtDescReqTesting.Text, IIf(ddFormula.SelectedValue = Nothing, 0, ddFormula.SelectedValue), IIf(RecSubmitted = True, DefaultDate, txtTodaysDate.Text), txtObjective.Text, txtStatusNotes.Text, txtEstManHrs.Text, ddDrawReview.SelectedValue, IIf(ddTestClass.SelectedValue = Nothing, 0, ddTestClass.SelectedValue), txtLongestAgingCycle.Text, txtTAG.Text, txtStartDt.Text, txtProjCmplDt.Text, txtActCmplDt.Text, txtEstAnnCostSav.Text, IIf(ddPriority.SelectedValue = Nothing, 0, ddPriority.SelectedValue), ViewState("DefaultUser"), cbACReq.Checked, IIf(hlnkACProjNo.Text = Nothing, 0, hlnkACProjNo.Text), txtAppropriation.Text)

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

    Protected Sub mnuTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuTabs.MenuItemClick
        mvTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub 'EOF mnuTabs_MenuItemClick

    Protected Sub ddRequestStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddRequestStatus.SelectedIndexChanged
        ''Used to Default Actual Completion Date with system date
        If ddRequestStatus.SelectedValue = "Completed" Then
            txtActCmplDt.Text = Date.Today
            btnResponse.Enabled = True
        End If
    End Sub 'EOF ddRequestStatus_SelectedIndexChanged

    Protected Sub txtLongestAgingCycle_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLongestAgingCycle.TextChanged
        If txtLongestAgingCycle.Text <> 0 Then
            lblLACdays.Text = CType((txtLongestAgingCycle.Text / 24), Integer) & " day(s)"
            lblLACdays.Visible = True
        End If
    End Sub 'EOF txtLongestAgingCycle_TextChanged

#End Region 'EOF General 

#Region "Customer/Part"
    Protected Sub btnAddtoGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddtoGrid.Click
        Try

            If ViewState("pReqID") <> Nothing Then

                lblMsgCategory4.Visible = False
                lblReqECI.Visible = False
                lblReqDMS.Visible = False

                Dim dsDrawing As DataSet
                dsDrawing = PEModule.GetDrawing(txtDrawingNo.Text.Trim)

                'If txtECINo.Text = Nothing And ddDrawNo.SelectedValue = Nothing And ViewState("pReqCategory") = 4 Then
                If txtECINo.Text = Nothing And commonFunctions.CheckDataSet(dsDrawing) = False And ViewState("pReqCategory") = 4 Then
                    lblReqECI.Visible = True
                    lblReqDMS.Visible = True
                    lblMsgCategory4.Visible = True
                    Exit Sub
                End If

                '*****
                '* Locate the position of the CABBV and SoldTo from ddCustomer
                '*****
                Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
                Dim ValidEntry As Boolean = True

                lblErrors.Text = Nothing
                lblErrors.Visible = False

                Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

                If ddCustomer.SelectedItem.Text.Substring(0, 2) = "**" Then
                    lblErrors.Text = "Customer selection is OBSOLETE. "
                    lblErrors.Visible = True
                    ddCustomer.Focus()
                    ValidEntry = False
                End If
                If ddProgram.SelectedItem.Text.Substring(0, 2) = "**" Then
                    lblErrors.Text = lblErrors.Text & "Program selection is OBSOLETE. "
                    lblErrors.Visible = True
                    ddProgram.Focus()
                    ValidEntry = False
                End If

                If lblErrors.Text <> Nothing Then
                    lblErrors.Text = lblErrors.Text & " Please choose a valid option."
                End If

                If commonFunctions.CheckDataSet(dsDrawing) = False And txtDrawingNo.Text.Trim <> "" Then
                    lblErrors.Text = lblErrors.Text & "DMS DrawingNo is invalid."
                    lblErrors.Visible = True
                    txtDrawingNo.Focus()
                    ValidEntry = False
                End If

                If ValidEntry = True Then
                    '***************
                    '* Insert Customer Part information to table
                    '***************
                    RnDModule.InsertTestIssuanceCustomerPart(ViewState("pReqID"), IIf(ddProgram.SelectedValue = "", 0, ddProgram.SelectedValue), txtPartNo.text, txtDesignLvl.Text, txtDrawingNo.Text.Trim, txtCustSpecNo.Text, txtLotNo.Text, txtMfgDt.Text, IIf(txtECINo.Text = Nothing, 0, txtECINo.Text), IIf(txtCostSheetID.Text = Nothing, 0, txtCostSheetID.Text), DefaultUser)

                    gvCustomerPart.DataBind()

                    cddOEMMfg.SelectedValue = Nothing
                    cddProgram.SelectedValue = Nothing
                    ddCustomer.SelectedValue = Nothing
                    ddProgram.SelectedValue = Nothing
                    txtPartNo.text = Nothing
                    txtDesignLvl.Text = Nothing
                    'ddDrawNo.SelectedValue = Nothing
                    txtDrawingNo.Text = ""
                    txtCustSpecNo.Text = Nothing
                    txtLotNo.Text = Nothing
                    txtMfgDt.Text = Nothing
                    txtECINo.Text = Nothing
                    txtCostSheetID.text = Nothing
                End If 'EOF  If ValidEntry = True Then

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
    End Sub 'EOF btnAddtoGrid_Click
    Protected Sub txtAppropriation_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAppropriation.TextChanged
        Try
            Dim ds As DataSet = New DataSet

            ''*************
            ''* Check that the Appropriation entered is a valid entry in SQL
            ''*************
            ''If true, default Total Expense minus the Total previously expensed if an IOR was written previously
            ''If false, there is no calculation involved. Ask TM to enter the total amount of the approved expense.
            txtAppropriation.Text = txtAppropriation.Text.ToUpper()
            txtProjectTitle.Text = Nothing
            hplkAppropriation.Text = Nothing
            hplkAppropriation.Visible = False

            If (txtAppropriation.Text <> Nothing) Then
                ds = PURModule.GetInternalOrderRequestCapEx(0, txtAppropriation.Text)
                If commonFunctions.CheckDataSet(ds) = True Then
                    txtProjectTitle.Text = ds.Tables(0).Rows(0).Item("ProjectTitle")
                    hplkAppropriation.Text = ds.Tables(0).Rows(0).Item("ProjectTitle")
                    txtProjectStatus.Text = ds.Tables(0).Rows(0).Item("ProjectStatus")
                    txtDefinedCapex.Text = ds.Tables(0).Rows(0).Item("DefinedCapEx")
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
                    lblErrors.Text = "APPROPRIATION IS NOT FOUND IN THE UGNDB, PLEASE REVIEW OR CONTACT THE APPLICATION GROUP FOR ASSISTANCE."
                    lblErrors.Visible = "True"
                    btnSave1.Enabled = False
                End If 'EOF If commonFunctions.CheckDataSet(ds) = True Then
            End If 'EOF If (txtAppropriation.Text <> Nothing) Then


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

    Protected Sub gvCustomerPart_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCustomerPart.RowDataBound
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
                    Dim price As RDTestIssuance.TestIssuance_CustomerPartRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, RDTestIssuance.TestIssuance_CustomerPartRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Customer (" & DataBinder.Eval(e.Row.DataItem, "ddCustomerDesc") & "); Program (" & DataBinder.Eval(e.Row.DataItem, "ProgramName") & "); Part No. (" & DataBinder.Eval(e.Row.DataItem, "PartNo") & ")?');")
                End If
            End If
        End If
    End Sub 'EOF gvCustomerPart_RowDataBound
#End Region 'EOF "Customer/Part"

#Region "Grid View functions"
    Protected Sub gvTMAssignments_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTMAssignments.RowCreated

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_TMAssignments
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If
    End Sub 'EOF gvTMAssignments_RowCreated

    Protected Sub gvTMAssignments_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTMAssignments.RowCommand
        ''***
        ''This section allows the inserting of a new row when save button is clicked from the footer.
        ''***
        If e.CommandName = "Insert" Then
            ''Insert data
            Dim TeamMember As DropDownList

            If gvTMAssignments.Rows.Count = 0 Then
                '' We are inserting through the DetailsView in the EmptyDataTemplate
                Return
            End If

            '' Only perform the following logic when inserting through the footer
            TeamMember = CType(gvTMAssignments.FooterRow.FindControl("ddTeamMember"), DropDownList)
            odsTMAssignments.InsertParameters("TeamMemberID").DefaultValue = TeamMember.SelectedValue

            odsTMAssignments.Insert()
        End If

        ''***
        ''This section clears out the values in the footer row
        ''***
        If e.CommandName = "Undo" Then
            Dim TeamMember As DropDownList
            TeamMember = CType(gvTMAssignments.FooterRow.FindControl("ddTeamMember"), DropDownList)
            TeamMember.SelectedValue = Nothing
        End If

    End Sub 'EOF gvTMAssignments_RowCommand

    Protected Sub gvTMAssignments_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTMAssignments.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(2).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As RDTestIssuance.TestIssuance_AssignmentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, RDTestIssuance.TestIssuance_AssignmentsRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Team Member (" & DataBinder.Eval(e.Row.DataItem, "TMName") & ")?');")
                End If
            End If
        End If
    End Sub 'EOF gvTMAssignments_RowDataBound

    Protected Sub gvTestReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTestReport.RowDataBound
        '***
        'This section provides the user with the popup for confirming the delete of a record.
        'Called by the onClientClick event.
        '***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(7).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As RDTestIssuance.TestIssuance_TestReportRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, RDTestIssuance.TestIssuance_TestReportRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Test Report (" & DataBinder.Eval(e.Row.DataItem, "TestReportID") & ")?');")
                End If
            End If
        End If
    End Sub 'EOF gvTestReport_RowDataBound
#End Region

#Region "Insert Empty GridView Work-Around for gvTMAssignments"
    Private Property LoadDataEmpty_TMAssignments() As Boolean

        Get
            If ViewState("LoadDataEmpty_TMAssignments") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_TMAssignments"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_TMAssignments") = value
        End Set
    End Property 'EOF LoadDataEmpty_TMAssignments

    Protected Sub odsTMAssignments_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsTMAssignments.Selected

        Dim PartNo As String = HttpContext.Current.Request.QueryString("sPartNo")

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        'Dim ds As DataSet = CType(e.ReturnValue, DataSet)
        'Dim dt As DataTable = ds.Tables(0)
        Dim dt As RDTestIssuance.TestIssuance_AssignmentsDataTable = CType(e.ReturnValue, RDTestIssuance.TestIssuance_AssignmentsDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty_TMAssignments = True
        Else
            LoadDataEmpty_TMAssignments = False
        End If
    End Sub 'EOF odsTMAssignments_Selected
#End Region

#Region "Acoustic/DMS/ECI reference"
    Protected Sub InitializeAllPopUps()
        Try
            'search current drawingno popup
            Dim strDrawingNoClientScript As String = HandleDrawingPopUps(txtDrawingNo.ClientID)
            iBtnDrawingSearch.Attributes.Add("onClick", strDrawingNoClientScript)

            'search current drawingno popup
            Dim strECINoClientScript As String = HandleECIPopUps(txtECINo.ClientID)
            iBtnECISearch.Attributes.Add("onClick", strECINoClientScript)

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF InitializeAllPopUps 

    Protected Function HandleDrawingPopUps(ByVal DrawingControlID As String) As String

        Try
            ' Build the client script to open a popup window containing
            ' SubDrawings. Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
                "../PE/DrawingLookUp.aspx?DrawingControlID=" & DrawingControlID
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','DrawingBPCSPartNos','" & _
                strWindowAttribs & "');return false;"

            HandleDrawingPopUps = strClientScript

        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleDrawingPopUps = ""
        End Try

    End Function 'EOF HandleDrawingPopUps

    Protected Function HandleECIPopUps(ByVal ECIControlID As String) As String

        Try
            ' Build the client script to open a popup window containing
            ' SubDrawings. Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
                "../ECI/ECI_LookUp.aspx?ECINoControlID=" & ECIControlID
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','ECINos','" & _
                strWindowAttribs & "');return false;"

            HandleECIPopUps = strClientScript

        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleECIPopUps = ""
        End Try

    End Function 'EOF HandleECIPopUps

    Public Function GoToDMSDetail(ByVal DrawingNo As String) As String
        Dim strReturnValue As String = ""
        Dim ds As DataSet

        If Not IsDBNull(DrawingNo) Then
            '(LREY) 01/08/2014
            ''ds = PEModule.GetDrawingSearch(DrawingNo, 0, "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, False, "", "", "", 0)
            ds = PEModule.GetDrawingSearch(DrawingNo, 0, "", "", "", "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, False, "", "", "", 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                strReturnValue = "~/PE/DMSDrawingPreview.aspx?DrawingNo=" & DrawingNo
            End If
        End If

        GoToDMSDetail = strReturnValue

    End Function 'EOF GoToDMSDetail

    Public Function GoToECIDetail(ByVal ECINo As String) As String
        Dim strReturnValue As String = ""
        Dim ds As DataSet

        If Not IsDBNull(ECINo) Then
            ds = ECIModule.GetECI(ECINo)
            If commonFunctions.CheckDataSet(ds) = True Then
                strReturnValue = "~/ECI/ECI_Preview.aspx?ECINo=" & ECINo
            End If
        End If

        GoToECIDetail = strReturnValue

    End Function 'EOF GoToECIDetail 

    Public Function GoToCostSheetDetail(ByVal CostSheetID As String) As String
        Dim strReturnValue As String = ""
        Dim ds As DataSet

        If Not IsDBNull(CostSheetID) Then
            ds = CostingModule.GetCostSheet(CostSheetID)
            If commonFunctions.CheckDataSet(ds) = True Then
                strReturnValue = "~/Costing/Cost_Sheet_Preview.aspx?CostSheetID=" & CostSheetID
            End If
        End If

        GoToCostSheetDetail = strReturnValue

    End Function 'EOF GoToCostSheetDetail 

    Public Function RequiredAcousticTesting() As Boolean
        Try
            ''**********************************
            '' This function is used to create a new Acoustic Record with related information 
            '' and redirect TM to Acoustic Page.
            ''**********************************
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultDate As Date = Date.Today
            Dim RequestStatus As String = Nothing
            Dim dsAL As DataSet = Nothing
            Dim dsCP As DataSet
            Dim i As Integer = 0
            ViewState("pProjID") = Nothing

            If ddRequestStatus.Visible = False Then
                RequestStatus = lblRequestStatus.Text 'view only
            Else
                RequestStatus = ddRequestStatus.SelectedValue 'Admin use only
            End If


            RnDModule.InsertTestIssuanceAcousticLabRequest(ViewState("pReqID"), txtSampleProdDesc.Text, ddSampleIssuer.SelectedValue, txtTodaysDate.Text, "O", DefaultUser, DefaultDate)

            '***************
            '* Locate Max RequestID
            '***************
            dsCP = RnDModule.GetTestIssuanceCustomerPart(ViewState("pReqID"))
            If commonFunctions.CheckDataSet(dsCP) = True Then
                For i = 0 To dsCP.Tables(0).Rows.Count - 1
                    dsAL = AcousticModule.GetLastProjectID("O", txtSampleProdDesc.Text, ddSampleIssuer.SelectedValue, txtTodaysDate.Text, dsCP.Tables(0).Rows(i).Item("ProgramID"), "", ViewState("DefaultUser"), DefaultDate)

                    ViewState("pProjID") = dsAL.Tables(0).Rows(0).Item("LastProjectID").ToString
                Next
                If ViewState("pProjID") <> Nothing Then
                    AcousticModule.InsertAcousticLabRequestCommodities(ViewState("pProjID"), ddCommodity.SelectedValue, DefaultUser)
                    UpdateRecord(RequestStatus, False)

                    'RnDModule.UpdateTestIssuanceRequests(ViewState("pReqID"), ViewState("pReqCategory"), RequestStatus, txtSampleProdDesc.Text, ddCommodity.SelectedValue, IIf(ddPurchasedGood.SelectedValue = Nothing, 0, ddPurchasedGood.SelectedValue), txtGeneralThickness.Text, ddUGNFacility.SelectedValue, txtSampleQty.Text, ddSampleIssuer.SelectedValue, txtDepartment.Text, txtTodaysDate.Text, txtTestCmpltDt.Text, txtPartAppMkt.Text, txtObjPerfTargets.Text, txtMiscAgenda.Text, txtDescReqTesting.Text, IIf(ddFormula.SelectedValue = Nothing, 0, ddFormula.SelectedValue), DefaultDate, "Please refer to description of required testing.", "", 0, "N/A", 0, 0, "", "", "", "", txtEstAnnCostSav.Text, 0, ViewState("DefaultUser"), cbACReq.Checked, ViewState("pProjID"))


                End If
            End If

            Response.Redirect("~/Acoustic/Acoustic_Project_Detail.aspx?pProjID=" & ViewState("pProjID"), False)

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Function 'EOF Acoustic/DMS/ECI reference
#End Region 'EOF  "Acoustic Testing"

#Region "Test Report"
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click, btnSaveTestRpt.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim DefaultDate As Date = Date.Today
            lblMessageView4.Text = Nothing
            lblMessageView4.Visible = False

            If ViewState("pRptID") > 0 Then
                If uploadFile.HasFile Then
                    If uploadFile.PostedFile.ContentLength <= 3500000 Then
                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFile.FileName)
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFile.PostedFile.FileName)
                        Dim BinaryFile(uploadFile.PostedFile.InputStream.Length) As Byte
                        Dim EncodeType As String = uploadFile.PostedFile.ContentType
                        uploadFile.PostedFile.InputStream.Read(BinaryFile, 0, BinaryFile.Length)
                        Dim FileSize As Integer = uploadFile.PostedFile.ContentLength

                        'If (FileExt = ".pdf")  Then
                        If (FileExt.ToUpper = ".PDF") Then
                            ''*************
                            '' Display File Info
                            ''*************
                            lblMessageView4.Text = "File name: " & uploadFile.FileName & "<br>" & _
                            "File Size: " & CType((FileSize / 1024), Integer) & " KB<br>"
                            lblMessageView4.Visible = True
                            lblMessageView4.Width = 500
                            lblMessageView4.Height = 30

                            ''*************
                            '' Update Record
                            ''*************
                            RnDModule.UpdateTestReport(ViewState("pRptID"), ViewState("pReqID"), ddReportIssuer.SelectedValue, txtTestDesc.Text, txtAssessment.Text, BinaryFile, uploadFile.FileName, EncodeType, FileSize, ViewState("DefaultUser"))

                            Response.Redirect("TestIssuanceDetail.aspx?" & _
                              RnDModule.QueryStringParam.pReqId.ToString & "=" & ViewState("pReqID") & _
                              "&" & RnDModule.QueryStringParam.pReqCategory.ToString & "=" & _
                            ViewState("pReqCategory") & "&pRptID=0", False)
                        End If
                    Else
                        lblMessageView4.Text = "File exceeds size limit.  Please select a file less than 3MB (3000KB)."
                        lblMessageView4.Visible = True
                        btnUpload.Enabled = False

                    End If
                Else
                    Dim BinaryFile(uploadFile.PostedFile.InputStream.Length) As Byte

                    ''*************
                    '' Update Record
                    ''*************
                    RnDModule.UpdateTestReport(ViewState("pRptID"), ViewState("pReqID"), ddReportIssuer.SelectedValue, txtTestDesc.Text, txtAssessment.Text, BinaryFile, "", "", 0, ViewState("DefaultUser"))

                    Response.Redirect("TestIssuanceDetail.aspx?" & _
                      RnDModule.QueryStringParam.pReqId.ToString & "=" & ViewState("pReqID") & _
                      "&" & RnDModule.QueryStringParam.pReqCategory.ToString & "=" & _
                    ViewState("pReqCategory") & "&pRptID=0", False)
                End If
            Else
                ''*************
                '' Save Record
                ''*************
                RnDModule.InsertTestReport(ViewState("pReqID"), ddReportIssuer.SelectedValue, txtTestDesc.Text, txtAssessment.Text, "", "", Nothing, 0, ViewState("DefaultUser"))

                rfvUpload.Visible = False
                rfvUpload.Enabled = False
                txtAssessment.Text = Nothing
                txtTestDesc.Text = Nothing
                gvTestReport.DataBind()

            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnSaveTestRpt_Click

    Protected Sub btnReset3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset3.Click
        Response.Redirect("TestIssuanceDetail.aspx?" & RnDModule.QueryStringParam.pReqId.ToString & "=" & ViewState("pReqID") & "&" & RnDModule.QueryStringParam.pReqCategory.ToString & "=" & ViewState("pReqCategory") & "&pRptID=0", False) '& ViewState("pRptID")
    End Sub ' btnReset3_Click

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
#End Region 'EOF "Test Report"

#Region "Email Notification"
    Public Function EmailBody(ByVal MyMessage As MailMessage) As String
        MyMessage.Body &= "<table width='60%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
        MyMessage.Body &= "<td width='388'><font size='2' face='Verdana'><strong>Sample Product Description</strong></font></td>"
        MyMessage.Body &= "<td width='423'><font size='2' face='Verdana'><strong>Sample Issuer </strong></font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr style='border-color:white;'>"
        MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & txtSampleProdDesc.Text & "</font></td>"
        MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ddSampleIssuer.SelectedItem.Text & "</font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:white;'>"
        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>UGN Location </strong></font></td>"
        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Commodity</strong></font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ddUGNFacility.SelectedItem.Text & "</font></td>"
        MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ddCommodity.SelectedItem.Text & "</font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td colspan='2'>"

        ''***************************************************
        ''Get list of Customer/Part information for display
        ''***************************************************
        MyMessage.Body &= "<table width='100%' border='0'>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Customer</strong></font></td>"
        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Program</strong></font></td>"
        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Part Number</strong></font></td>"
        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Design Level </strong></font></td>"
        MyMessage.Body &= "</tr>"
        Dim dsCP As DataSet
        dsCP = RnDModule.GetTestIssuanceCustomerPart(ViewState("pReqID"))
        If dsCP.Tables.Count > 0 And (dsCP.Tables.Item(0).Rows.Count > 0) Then
            For i = 0 To dsCP.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= "<tr style='border-color:white'><font size='2' face='Verdana'>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("ddCustomerDesc") & "&nbsp;</td>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("ProgramName") & "&nbsp;</td>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("PartNo") & "&nbsp;</td>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("DesignLevel") & "&nbsp;</td>"
                MyMessage.Body &= "</font></tr>"
            Next
        End If
        MyMessage.Body &= "</Table>"
        MyMessage.Body &= "</td></tr>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'>"
        MyMessage.Body &= "<td colspan='2'><font size='2' face='Verdana'><strong>Description of Required Testing</strong></font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr style='border-color:white;'>"
        MyMessage.Body &= "<td colspan='2'><font size='2' face='Verdana'>" & txtDescReqTesting.Text & "</font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "</Table>"

        Return True
    End Function 'eof EmailBody

    Sub btnSubmit1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit1.Click
        Try
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            'Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultDate As Date = Date.Today
            Dim RequestStatus As String = Nothing
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim EmailTO As String = Nothing
            Dim EmpName As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing

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
            '* Only users with valid email accounts can submit a test request.
            '********
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pReqID") <> 0 Then
                    If ddRequestStatus.Visible = False Then
                        RequestStatus = lblRequestStatus.Text 'view only
                    Else
                        RequestStatus = ddRequestStatus.SelectedValue 'Admin use only
                    End If

                    ''***************
                    ''Verify that atleast one Customer/Part Info entry has been entered before submitting to R&D
                    ''***************
                    Dim ds As DataSet
                    ds = RnDModule.GetTestIssuanceCustomerPart(ViewState("pReqID"))
                    If (ds.Tables.Item(0).Rows.Count = 0) Then 'If missing kick user out from submission.
                        mvTabs.ActiveViewIndex = Int32.Parse(1)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(1).Selected = True

                        rfvCustomer.IsValid = False
                        rfvProgram.IsValid = False
                        rfvPartNo.IsValid = False
                        vsCustomerPart.ShowSummary = True

                        lblErrors.Text = "Atleast one Customer/Part entry is required for submission."
                        lblErrors.Visible = True
                        lblErrors.Font.Size = 12
                        MaintainScrollPositionOnPostBack = False
                        Exit Sub
                    Else
                        ''***************
                        ''Save any changed data prior to submitting to R&D
                        ''**************
                        UpdateRecord(RequestStatus, True)

                        ''**************************************************************************
                        ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                        ''**************************************************************************
                        Dim RnDds As DataSet
                        Dim dsCC As DataSet
                        Dim dsCommodity As DataSet
                        Dim i As Integer = 0

                        ''*******************************
                        ''Locate Default R&D recepient(s)
                        ''*******************************
                        RnDds = commonFunctions.GetTeamMemberBySubscription(48)
                        ''Check that the recipient(s) is a valid Team Member
                        If RnDds.Tables.Count > 0 And (RnDds.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To RnDds.Tables.Item(0).Rows.Count - 1
                                If (RnDds.Tables(0).Rows(i).Item("Email") <> Nothing) And (RnDds.Tables(0).Rows(i).Item("WorkStatus") = 1) Or (RnDds.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                    If EmailTO = Nothing Then
                                        EmailTO = RnDds.Tables(0).Rows(i).Item("Email")
                                    Else
                                        EmailTO = EmailTO & ";" & RnDds.Tables(0).Rows(i).Item("Email")
                                    End If
                                    If EmpName = Nothing Then
                                        EmpName = RnDds.Tables(0).Rows(i).Item("DisplayTMName") & ", "
                                    Else
                                        EmpName = EmpName & RnDds.Tables(0).Rows(i).Item("DisplayTMName") & ", "
                                    End If

                                End If
                            Next
                        End If

                        If EmailTO <> Nothing Then
                            Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                            Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                            Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)


                            ''***************************************
                            ''Locate Cc: recepient(s) by Subscription
                            ''***************************************
                            dsCC = commonFunctions.GetTeamMemberBySubscription(55)
                            ''Check that the recipient(s) is a valid Team Member
                            If dsCC.Tables.Count > 0 And (dsCC.Tables.Item(0).Rows.Count > 0) Then
                                For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                                    If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = 1) Or (dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                        ''************************************
                                        ''Locate Cc: recepient(s) by Commodity
                                        ''************************************
                                        dsCommodity = commonFunctions.GetTeamMemberByWorkFlowAssignments(dsCC.Tables(0).Rows(i).Item("TMID"), 5, ddCommodity.SelectedValue, "", 0)
                                        If dsCommodity.Tables.Count > 0 And (dsCommodity.Tables.Item(0).Rows.Count > 0) Then
                                            MyMessage.CC.Add(dsCC.Tables(0).Rows(i).Item("Email"))
                                            If EmailCC = Nothing Then
                                                EmailCC = dsCC.Tables(0).Rows(i).Item("Email")
                                            Else
                                                EmailCC = EmailCC & ";" & dsCC.Tables(0).Rows(i).Item("Email")
                                            End If
                                        End If
                                    End If
                                Next
                            End If

                            MyMessage.Subject = "Test Issuance Request: Cust-TI-Req-" & ViewState("pReqID") & " for " & ViewState("ReqCategorDesc") & " - Date Requested for Test Completion: " & txtTestCmpltDt.Text

                            MyMessage.Body = "<font size='2' face='Verdana'>" & EmpName & "</font>"
                            MyMessage.Body &= "<p><font size='2' face='Verdana'>Please review Cust-TI-Req-" & ViewState("pReqID") & " for " & ViewState("ReqCategorDesc")
                            MyMessage.Body &= " and provide results by " & txtTestCmpltDt.Text
                            MyMessage.Body &= ". <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/RnD/TestIssuanceDetail.aspx?pReqID=" & ViewState("pReqID") & "&pReqCategory=" & ViewState("pReqCategory") & "'>Click here</a> to access the record.</font></p>"


                            ''*******************
                            ''Build Email Body
                            ''*******************
                            EmailBody(MyMessage)

                            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
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
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "R&D Test Issuance", "Cust-TI-Req-" & ViewState("pReqID"))
                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                            Catch ex As Exception
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                UGNErrorTrapping.InsertEmailQueue("Cust-TI-Req-" & ViewState("pReqID"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                'get current event name
                                Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                                'log and email error
                                UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                            End Try
                            lblErrors.Visible = True

                            ''**********************************
                            ''Rebind the data to the form
                            ''********************************** 
                            BindData()

                            ''***********************************************************
                            ''Copy selective information if Acoustic Test is Required
                            ''Open in a new window
                            ''***********************************************************
                            If cbACReq.Checked = True And hlnkACProjNo.Text = Nothing Then
                                RequiredAcousticTesting()
                            End If

                        Else
                            lblErrors.Text = "Email Submission Cancelled: Invalid email address found. Please submit a Database Requestor."
                            lblErrors.Visible = True
                        End If
                    End If
                Else
                    lblErrors.Text = "Error found with submission. Please submit a Database Requestor."
                    lblErrors.Visible = True
                End If
            Else
                lblErrors.Text = "You do not have a valid email account. Request Cancelled."
                lblErrors.Visible = True
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
    End Sub 'EOF btnSubmit1_Click

    Protected Sub btnNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNotify.Click
        Try
            ''**********
            ''This section is used by R&D Administrator to delegate test requests to designated team members.
            ''**********
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            'Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim DefaultDate As Date = Date.Today
            Dim RequestStatus As String = Nothing
            Dim EmailTO As String = Nothing
            Dim EmpName As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing

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
            '* Only users with valid email accounts can submit a test request.
            '********
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pReqID") <> 0 Then
                    If ddRequestStatus.Visible = False Then
                        RequestStatus = lblRequestStatus.Text 'view only
                    Else
                        RequestStatus = ddRequestStatus.SelectedValue 'Admin use only
                    End If

                    ''***************
                    ''Verify that atleast one Customer/Part Info entry has been entered before submitting to R&D
                    ''***************
                    Dim ds As DataSet
                    ds = RnDModule.GetTestIssuanceCustomerPart(ViewState("pReqID"))
                    If (ds.Tables.Item(0).Rows.Count = 0) Then 'If missing kick user out from submission.
                        mvTabs.ActiveViewIndex = Int32.Parse(1)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(1).Selected = True

                        rfvCustomer.IsValid = False
                        rfvProgram.IsValid = False
                        rfvPartNo.IsValid = False
                        vsCustomerPart.ShowSummary = True

                        lblErrors.Text = "Atleast one Customer/Part entry is required for submission."
                        lblErrors.Visible = True
                        lblErrors.Font.Size = 12
                        MaintainScrollPositionOnPostBack = False
                        Exit Sub
                    Else

                        ''**************************
                        ''Build email
                        ''**************************
                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)


                        ''**************************************************************************
                        ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                        ''**************************************************************************
                        Dim TMds As DataSet
                        Dim i As Integer = 0

                        ''*********************************************
                        ''Locate Assigned Team Members for notification
                        ''*********************************************
                        TMds = RnDModule.GetTestIssuanceAssignments(ViewState("pReqID"))
                        ''Check that the recipient(s) is a valid Team Member
                        If TMds.Tables.Count > 0 And (TMds.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To TMds.Tables.Item(0).Rows.Count - 1
                                If (TMds.Tables(0).Rows(i).Item("Email") <> Nothing) And (TMds.Tables(0).Rows(i).Item("Working") = 1) Or (TMds.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And (IsDBNull(TMds.Tables(0).Rows(i).Item("EmailNotificationDate"))) Then

                                    If EmailTO = Nothing Then
                                        MyMessage.To.Add(TMds.Tables(0).Rows(i).Item("Email"))
                                        EmailTO = TMds.Tables(0).Rows(i).Item("Email")
                                    Else
                                        EmailTO = EmailTO & ";" & TMds.Tables(0).Rows(i).Item("Email")
                                        MyMessage.To.Add(TMds.Tables(0).Rows(i).Item("Email"))
                                    End If
                                    If EmpName = Nothing Then
                                        EmpName = TMds.Tables(0).Rows(i).Item("DisplayTMName") & ", "
                                    Else
                                        EmpName = EmpName & TMds.Tables(0).Rows(i).Item("DisplayTMName") & ", "
                                    End If

                                End If
                            Next
                        End If

                        If EmailTO <> Nothing Then
                            ''***************
                            ''Save any changed data prior to submitting to Assigned Team Members
                            ''**************
                            UpdateRecord(RequestStatus, False)

                            MyMessage.Subject = "Test Issuance Request: Cust-TI-Req-" & ViewState("pReqID") & " for " & ViewState("ReqCategorDesc") & " - Date Requested for Test Completion: " & txtTestCmpltDt.Text

                            MyMessage.Body = "<font size='2' face='Verdana'>" & EmpName & "</font>"
                            MyMessage.Body &= "<p><font size='2' face='Verdana'>Cust-TI-Req-" & ViewState("pReqID") & " for " & ViewState("ReqCategorDesc")
                            MyMessage.Body &= " was assigned to you for review/testing. Please provide results by " & txtTestCmpltDt.Text

                            MyMessage.Body &= ". <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/RnD/TestIssuanceDetail.aspx?pReqID=" & ViewState("pReqID") & "&pReqCategory=" & ViewState("pReqCategory") & "'>Click here</a> to access the record.</font></p>"


                            ''*******************
                            ''Build Email Body
                            ''*******************
                            EmailBody(MyMessage)

                            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
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
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "R&D Test Issuance", "Cust-TI-Req-" & ViewState("pReqID"))
                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                            Catch ex As Exception
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                UGNErrorTrapping.InsertEmailQueue("Cust-TI-Req-" & ViewState("pReqID"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                'get current event name
                                Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                                'log and email error
                                UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                            End Try
                            lblErrors.Visible = True


                            ''**********************************
                            ''Update record's "SentToRnD" column
                            ''**********************************
                            RnDModule.UpdateTestIssuanceAssignments(ViewState("pReqID"), ViewState("DefaultUser"))
                            ''**********************************
                            ''Rebind the data to the form
                            ''********************************** 
                            BindData()

                            gvTMAssignments.DataBind()

                            lblErrors.Text = "Test Issuance Submitted Successfully."
                            lblErrors.Visible = True
                        Else
                            lblErrors.Text = "Email Submission Cancelled: Either there are no assigned team members or notification was previously sent. Please review..."
                            lblErrors.Visible = True
                        End If
                    End If
                Else
                    lblErrors.Text = "Error found with submission. Please submit a Database Requestor."
                    lblErrors.Visible = True
                End If
            Else
                lblErrors.Text = "You do not have a valid email account. Request Cancelled."
                lblErrors.Visible = True
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnNotify_Click

    Protected Sub btnResponse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResponse.Click
        Try
            Dim EmailTO As String = Nothing
            Dim EmpName As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing
            'Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultDate As Date = Date.Today
            Dim RequestStatus As String = Nothing
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            lblErrors.Text = Nothing
            lblErrors.Visible = False

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
            '* Only users with valid email accounts can submit an email notification
            '********
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pReqID") <> 0 Then
                    If ddRequestStatus.Visible = False Then
                        RequestStatus = lblRequestStatus.Text 'view only
                    Else
                        RequestStatus = ddRequestStatus.SelectedValue 'Admin use only
                    End If

                    ''***************
                    ''Verify that atleast one Customer/Part Info entry has been entered before submitting to R&D
                    ''***************
                    Dim ds As DataSet
                    ds = RnDModule.GetTestIssuanceCustomerPart(ViewState("pReqID"))
                    If (ds.Tables.Item(0).Rows.Count = 0) Then 'If missing kick user out from submission.
                        mvTabs.ActiveViewIndex = Int32.Parse(1)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(1).Selected = True

                        rfvCustomer.IsValid = False
                        rfvProgram.IsValid = False
                        rfvPartNo.IsValid = False
                        vsCustomerPart.ShowSummary = True

                        lblErrors.Text = "Atleast one Customer/Part entry is required for submission."
                        lblErrors.Visible = True
                        lblErrors.Font.Size = 12
                        MaintainScrollPositionOnPostBack = False
                        Exit Sub
                    Else
                        ''***************
                        ''Save any changed data prior to submitting to R&D
                        ''**************
                        UpdateRecord(RequestStatus, False)

                        ''**************************************************************************
                        ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                        ''**************************************************************************
                        Dim RnDds As DataSet
                        Dim dsCC As DataSet
                        Dim dsCommodity As DataSet
                        Dim i As Integer = 0

                        ''*******************************
                        ''Locate Sample Issuers Email Address
                        ''*******************************
                        RnDds = SecurityModule.GetTeamMember(ddSampleIssuer.SelectedValue, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                        ''Check that the recipient(s) is a valid Team Member
                        If RnDds.Tables.Count > 0 And (RnDds.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To RnDds.Tables.Item(0).Rows.Count - 1
                                If (RnDds.Tables(0).Rows(i).Item("Email") <> Nothing) Or (RnDds.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                    If EmailTO = Nothing Then
                                        EmailTO = RnDds.Tables(0).Rows(i).Item("Email")
                                    Else
                                        EmailTO = EmailTO & ";" & RnDds.Tables(0).Rows(i).Item("Email")
                                    End If
                                    If EmpName = Nothing Then
                                        EmpName = RnDds.Tables(0).Rows(i).Item("FirstName") & ", "
                                    Else
                                        EmpName = EmpName & RnDds.Tables(0).Rows(i).Item("FirstName") & ", "
                                    End If
                                End If
                            Next
                        End If

                        If EmailTO <> Nothing Then
                            ''**************************
                            ''Build email
                            ''**************************
                            Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                            Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                            Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                            ''***************************************
                            ''Locate Cc: recepient(s) by Subscription
                            ''***************************************
                            dsCC = commonFunctions.GetTeamMemberBySubscription(55)
                            ''Check that the recipient(s) is a valid Team Member
                            If dsCC.Tables.Count > 0 And (dsCC.Tables.Item(0).Rows.Count > 0) Then
                                For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                                    If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = 1) Or (dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                        ''************************************
                                        ''Locate Cc: recepient(s) by Commodity
                                        ''************************************
                                        dsCommodity = commonFunctions.GetTeamMemberByWorkFlowAssignments(dsCC.Tables(0).Rows(i).Item("TMID"), 5, ddCommodity.SelectedValue, "", 0)
                                        If dsCommodity.Tables.Count > 0 And (dsCommodity.Tables.Item(0).Rows.Count > 0) Then
                                            MyMessage.CC.Add(dsCC.Tables(0).Rows(i).Item("Email"))
                                            If EmailCC = Nothing Then
                                                EmailCC = dsCC.Tables(0).Rows(i).Item("Email")
                                            Else
                                                EmailCC = EmailCC & ";" & dsCC.Tables(0).Rows(i).Item("Email")
                                            End If
                                        End If
                                    End If
                                Next
                            End If

                            MyMessage.Subject = "Test Issuance Request: Cust-TI-Req-" & ViewState("pReqID") & " for " & ViewState("ReqCategorDesc") & " - Completed as of " & txtActCmplDt.Text

                            MyMessage.Body = "<font size='2' face='Verdana'>" & EmpName & "</font>"
                            MyMessage.Body &= "<p><font size='2' face='Verdana'>Cust-TI-Req-" & ViewState("pReqID") & " for " & ViewState("ReqCategorDesc")
                            MyMessage.Body &= " was completed on " & txtActCmplDt.Text
                            MyMessage.Body &= " and ready for your review. "
                            MyMessage.Body &= " <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/RnD/TestIssuanceDetail.aspx?pReqID=" & ViewState("pReqID") & "&pReqCategory=" & ViewState("pReqCategory") & "'>Click here</a> to access the record.</font></p>"

                            ''*******************
                            ''Build Email Body
                            ''*******************
                            EmailBody(MyMessage)

                            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
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
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "R&D Test Issuance", "Cust-TI-Req-" & ViewState("pReqID"))
                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                            Catch ex As Exception
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                UGNErrorTrapping.InsertEmailQueue("Cust-TI-Req-" & ViewState("pReqID"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                'get current event name
                                Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                                'log and email error
                                UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                            End Try
                            lblErrors.Visible = True

                            ''**********************************
                            ''Rebind the data to the form
                            ''********************************** 
                            BindData()

                        Else
                            lblErrors.Text = "Email Submission Cancelled: Invalid email address found. Please submit a Database Requestor."
                            lblErrors.Visible = True
                        End If
                    End If
                Else
                    lblErrors.Text = "Error found with submission. Please submit a Database Requestor."
                    lblErrors.Visible = True
                End If
            Else
                lblErrors.Text = "You do not have a valid email account. Request Cancelled."
                lblErrors.Visible = True
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
    End Sub 'EOF btnResponse_Click
#End Region 'EOF Email Notification

End Class




