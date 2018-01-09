' ************************************************************************************************
' Name:	AcousticProjectDetail.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 05/04/2009    LRey			Created .Net application
' 07/16/2009    LRey            Modified DataBind for ddCustomer to display SoldTo/Cabbv not found in the list built by vCustomer.
' 01/08/2014    LREY            Replaced GetCustomer with GetOEMManufacturer. SOLDTO|CABBV is not used in the new ERP.
' ************************************************************************************************
Imports System.Net.Mail.SmtpClient

Partial Class Acoustic_Project_Detail
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            If HttpContext.Current.Request.QueryString("pProjID") <> "" Then
                ViewState("pProjID") = CType(HttpContext.Current.Request.QueryString("pProjID"), Integer)
            Else
                ViewState("pProjID") = 0
            End If

            If HttpContext.Current.Request.QueryString("pRptID") <> "" Then
                ViewState("pRptID") = HttpContext.Current.Request.QueryString("pRptID")
            Else
                ViewState("pRptID") = -1
            End If

            'ViewState("DefaultUser") = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            If ViewState("pProjID") = 0 Then
                m.ContentLabel = "New Acoustic Lab Request"
            Else
                m.ContentLabel = "Acoustic Lab Request"
            End If


            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pProjID") = 0 Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > <a href='Acoustic_Lab_Testing_List.aspx'><b>Acoustic Lab Request Search</b></a> > New Acoustic Lab Request"
                Else
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > <a href='Acoustic_Lab_Testing_List.aspx'><b>Acoustic Lab Request Search</b></a> > Acoustic Lab Request"
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
            ctl = m.FindControl("RnDExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If


            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                BindCriteria()
                If Session("viewOnly") Then
                    Session("isEnabled") = "False"
                Else
                    Session("isEnabled") = "True"
                End If

                ''****************************************
                ''Redirect user to the right tab location
                ''****************************************
                If ViewState("pRptID") = -1 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(0)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(0).Selected = True
                ElseIf ViewState("pRptID") >= 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(2)
                    mvTabs.GetActiveView()
                    mnuTabs.Items(2).Selected = True
                    lblMessageView4.Text = Nothing
                    lblMessageView4.Visible = False
                End If

                If ViewState("pProjID") <> 0 Then
                    BindData()
                Else
                    txtTestDescription.Focus()
                    txtDateRequested.Text = Date.Today
                    gvCommodity.Visible = False
                End If

                ''*************************************************
                '' "Form Level Security using Roles &/or Subscriptions"
                ''*************************************************
                CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            End If


            txtProjectGoals.Attributes.Add("onkeypress", "return tbLimit();")
            txtProjectGoals.Attributes.Add("onkeyup", "return tbCount(" + lblProjectGoals.ClientID + ");")
            txtProjectGoals.Attributes.Add("maxLength", "400")

            txtBackground.Attributes.Add("onkeypress", "return tbLimit();")
            txtBackground.Attributes.Add("onkeyup", "return tbCount(" + lblBackground.ClientID + ");")
            txtBackground.Attributes.Add("maxLength", "400")

            txtSpecialInst.Attributes.Add("onkeypress", "return tbLimit();")
            txtSpecialInst.Attributes.Add("onkeyup", "return tbCount(" + lblSpecial.ClientID + ");")
            txtSpecialInst.Attributes.Add("maxLength", "400")

            txtSampleDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtSampleDesc.Attributes.Add("onkeyup", "return tbCount(" + lblSampleDesc.ClientID + ");")
            txtSampleDesc.Attributes.Add("maxLength", "800")

            txtAddInstructions.Attributes.Add("onkeypress", "return tbLimit();")
            txtAddInstructions.Attributes.Add("onkeyup", "return tbCount(" + lblInstructions.ClientID + ");")
            txtAddInstructions.Attributes.Add("maxLength", "400")

            txtRptDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtRptDesc.Attributes.Add("onkeyup", "return tbCount(" + lblRptDesc.ClientID + ");")
            txtRptDesc.Attributes.Add("maxLength", "400")

            txtComments.Attributes.Add("onkeypress", "return tbLimit();")
            txtComments.Attributes.Add("onkeyup", "return tbCount(" + lblComments.ClientID + ");")
            txtComments.Attributes.Add("maxLength", "500")

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
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
            btnAdd.Enabled = False
            btnSave1.Enabled = False
            btnSave2.Enabled = False
            btnReset1.Enabled = False
            btnReset2.Enabled = False
            btnDelete.Enabled = False
            btnSubmit1.Enabled = False
            btnNotify.Enabled = False
            txtComments.Enabled = False
            ddProjectStatus.Enabled = False
            mnuTabs.Items(1).Enabled = False
            mnuTabs.Items(2).Enabled = False
            ViewState("Admin") = False
            gvCommodity.Columns(1).Visible = False
            gvCommodity.FooterRow.Visible = False
            'gvProjectReport.Columns(5).Visible = False

            ViewState("ObjectRole") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 62 'Acoustic Lab Request Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Bryan.Hall", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

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
                                        btnAdd.Enabled = True
                                        btnSave1.Enabled = True
                                        btnReset1.Enabled = True

                                        If ViewState("SubmittedToLab") = Nothing Then
                                            btnSubmit1.Enabled = True
                                        End If
                                        ddProjectStatus.Enabled = True
                                        ViewState("ObjectRole") = True
                                        gvProjectReport.Columns(5).Visible = True
                                        ''*************************************************
                                        ''for new requests, disable all but the first tab
                                        ''*************************************************
                                        If (ViewState("pProjID") <> 0) Then
                                            btnDelete.Enabled = True
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                            btnSave2.Enabled = True
                                            btnReset2.Enabled = True
                                            lblComReq.Visible = True
                                            gvCommodity.Columns(1).Visible = True
                                            gvCommodity.FooterRow.Visible = True
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                            If ddProjectStatus.SelectedValue = "O" Then
                                                btnNotify.Enabled = True
                                                txtComments.Enabled = True
                                            End If
                                        End If
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        btnAdd.Enabled = True
                                        btnDelete.Enabled = True
                                        If txtSubmittedToLab.Text = Nothing Or ddProjectStatus.SelectedValue = "R" Then
                                            btnSave1.Enabled = True
                                            btnReset1.Enabled = True
                                            btnSubmit1.Enabled = True
                                            lblComReq.Visible = True
                                            gvCommodity.Columns(1).Visible = True
                                            gvCommodity.FooterRow.Visible = True
                                        Else
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                        End If
                                        ViewState("ObjectRole") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        btnAdd.Enabled = True
                                        If txtSubmittedToLab.Text = Nothing Then
                                            btnSave1.Enabled = True
                                            btnReset1.Enabled = True
                                            btnSubmit1.Enabled = True
                                            lblComReq.Visible = True
                                            gvCommodity.Columns(1).Visible = True
                                            gvCommodity.FooterRow.Visible = True
                                        Else
                                            mnuTabs.Items(1).Enabled = True
                                            mnuTabs.Items(2).Enabled = True
                                        End If
                                        ViewState("ObjectRole") = True
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        mnuTabs.Items(1).Enabled = True
                                        mnuTabs.Items(2).Enabled = True
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        btnSave1.Enabled = True
                                        btnReset1.Enabled = True
                                        mnuTabs.Items(1).Enabled = True
                                        mnuTabs.Items(2).Enabled = True
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        mnuTabs.Items(1).Enabled = True
                                        mnuTabs.Items(2).Enabled = True
                                End Select 'EOF of "Select Case iRoleID"
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
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

    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        'bind existing data to drop down level control for selection criteria for search
        ds = AcousticModule.GetAcousticStatus("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddProjectStatus.DataSource = ds
            ddProjectStatus.DataTextField = ds.Tables(0).Columns("status").ColumnName.ToString()
            ddProjectStatus.DataValueField = ds.Tables(0).Columns("statusCode").ColumnName.ToString()
            ddProjectStatus.DataBind()
            ddProjectStatus.Items.Insert(0, "")
            ddProjectStatus.SelectedIndex = 4
        End If

        ''bind existing data to drop down Sample Issuer control for selection criteria for search
        ds = commonFunctions.GetTeamMember("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddRequester.DataSource = ds
            ddRequester.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
            ddRequester.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
            ddRequester.DataBind()
            ddRequester.Items.Insert(0, "")

            ddReportIssuer.DataSource = ds
            ddReportIssuer.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
            ddReportIssuer.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
            ddReportIssuer.DataBind()
            ddReportIssuer.Items.Insert(0, "")
        End If

        commonFunctions.UserInfo()
        ddRequester.SelectedValue = HttpContext.Current.Session("UserId")
        ddReportIssuer.SelectedValue = HttpContext.Current.Session("UserId")

        ' '' ''bind existing data to drop down Customer control for selection criteria for search
        ' ''ds = commonFunctions.GetCABBV()
        ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
        ' ''    ddCustomer.DataSource = ds
        ' ''    ddCustomer.DataTextField = ds.Tables(0).Columns("CustomerNameCombo").ColumnName.ToString()
        ' ''    ddCustomer.DataValueField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()
        ' ''    ddCustomer.DataBind()
        ' ''    ddCustomer.Items.Insert(0, "")
        ' ''End If

        ''bind existing data to drop down Customer control for selection criteria for search
        ' ''ds = commonFunctions.GetOEMManufacturer("")
        ' ''If commonFunctions.CheckDataSet(ds) = True Then
        ' ''    ddCustomer.DataSource = ds
        ' ''    ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
        ' ''    ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
        ' ''    ddCustomer.DataBind()
        ' ''    ddCustomer.Items.Insert(0, "")
        ' ''End If

        ' '' ''bind existing data to drop down Customer control for selection criteria for search
        ' ''ds = commonFunctions.GetProgram("", "", "")
        ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
        ' ''    ddProgram.DataSource = ds
        ' ''    ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
        ' ''    ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName.ToString()
        ' ''    ddProgram.DataBind()
        ' ''    ddProgram.Items.Insert(0, "")
        ' ''End If

        ''bind existing data to drop down Engineer/Technician control for selection criteria for search
        ds = AcousticModule.GetAcousticPeople("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddEngineer.DataSource = ds
            ddEngineer.DataTextField = ds.Tables(0).Columns("Name").ColumnName.ToString()
            ddEngineer.DataValueField = ds.Tables(0).Columns("PeopleID").ColumnName.ToString()
            ddEngineer.DataBind()
            ddEngineer.Items.Insert(0, "")

            ddTechnician.DataSource = ds
            ddTechnician.DataTextField = ds.Tables(0).Columns("Name").ColumnName.ToString()
            ddTechnician.DataValueField = ds.Tables(0).Columns("PeopleID").ColumnName.ToString()
            ddTechnician.DataBind()
            ddTechnician.Items.Insert(0, "")
        End If


    End Sub 'EOF of BindCriteria

    Protected Sub BindData()
        Dim ds As DataSet = New DataSet
        Try
            ds = AcousticModule.GetProjectData(ViewState("pProjID"), "", "", 0, 0, "", "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then

                lblProjectNo.Text = ds.Tables(0).Rows(0).Item("ProjectID").ToString()
                txtTestDescription.Text = ds.Tables(0).Rows(0).Item("TestDescription").ToString()
                ddProjectStatus.SelectedValue = ds.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                ddRequester.SelectedValue = ds.Tables(0).Rows(0).Item("SubmittedBy").ToString()
                txtDateRequested.Text = ds.Tables(0).Rows(0).Item("DateRequested").ToString()
                txtTestCmpltDt.Text = ds.Tables(0).Rows(0).Item("TestCmpltDate").ToString()
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("RequestID")) Then
                    hlnkRDReqNo.NavigateUrl = "~/RnD/crViewTestIssuanceRequestform.aspx?pReqID=" & ds.Tables(0).Rows(0).Item("RequestID").ToString()
                    hlnkRDReqNo.Text = ds.Tables(0).Rows(0).Item("RequestID").ToString() & " - " & ds.Tables(0).Rows(0).Item("ReqCategoryDesc").ToString()
                    hlnkRDReqNo.Font.Underline = True
                    hlnkRDReqNo.ForeColor = Color.Blue
                Else
                    hlnkRDReqNo.Visible = False
                End If
                Dim checkCustSoldTo As Boolean = False
                Dim i As Integer
                For i = 0 To ddCustomer.Items.Count - 1
                    If ds.Tables(0).Rows(0).Item("ddCustomerValue").ToString() = ddCustomer.Items(i).Value Then
                        checkCustSoldTo = True
                    End If
                Next

                'If checkCustSoldTo = False Then
                '    ' ddCustomer.Items.Insert(ddCustomer.Items.Count, ds.Tables(0).Rows(0).Item("ddCustomerValue").ToString())
                '    Dim liEventTypeListItem As New System.Web.UI.WebControls.ListItem
                '    liEventTypeListItem.Text = ds.Tables(0).Rows(0).Item("ddCustomerDesc").ToString()
                '    liEventTypeListItem.Value = ds.Tables(0).Rows(0).Item("ddCustomerValue").ToString()
                '    ddCustomer.Items.Add(liEventTypeListItem)
                'End If

                cddOEMMfg.SelectedValue = ds.Tables(0).Rows(0).Item("ddCustomerValue").ToString()
                cddProgram.SelectedValue = ds.Tables(0).Rows(0).Item("ProgramID").ToString()
                txtNoOfTestSamples.Text = ds.Tables(0).Rows(0).Item("NoOfTestSamples").ToString()
                txtSampleDesc.Text = ds.Tables(0).Rows(0).Item("SampleDescription").ToString()
                txtProjectGoals.Text = ds.Tables(0).Rows(0).Item("ProjectGoals").ToString()
                txtBackground.Text = ds.Tables(0).Rows(0).Item("Background").ToString()
                ddRptReq.SelectedValue = ds.Tables(0).Rows(0).Item("RptRequirements").ToString()
                txtSpecialInst.Text = ds.Tables(0).Rows(0).Item("SpecialInstructions").ToString()
                txtSubmittedToLab.Text = ds.Tables(0).Rows(0).Item("SubmittedToLab").ToString()

                If Not IsNothing(ds.Tables(0).Rows(0).Item("EngineerID").ToString()) Then
                    ddEngineer.SelectedValue = ds.Tables(0).Rows(0).Item("EngineerID").ToString()
                End If

                If Not IsNothing(ds.Tables(0).Rows(0).Item("TechnicianID").ToString()) Then
                    ddTechnician.SelectedValue = ds.Tables(0).Rows(0).Item("TechnicianID").ToString()
                End If

                txtReiterRefNo.Text = ds.Tables(0).Rows(0).Item("ReiterRefNo").ToString()

                If Not IsNothing(ds.Tables(0).Rows(0).Item("EstimatedCost").ToString()) Then
                    txtEstCost.Text = ds.Tables(0).Rows(0).Item("EstimatedCost").ToString()
                Else
                    txtEstCost.Text = "0.00"
                End If

                If Not IsNothing(ds.Tables(0).Rows(0).Item("ActualCost").ToString()) Then
                    txtActualCost.Text = ds.Tables(0).Rows(0).Item("ActualCost").ToString()
                Else
                    txtActualCost.Text = "0.00"
                End If

                txtDevExp.Text = ds.Tables(0).Rows(0).Item("DevelopmentExpense").ToString()
                txtProjIntDt.Text = ds.Tables(0).Rows(0).Item("ProjInitiationDate").ToString()
                txtEstCmpltDt.Text = ds.Tables(0).Rows(0).Item("EstCmpltDate").ToString()
                txtActualCmplDt.Text = ds.Tables(0).Rows(0).Item("ActualCmpltDate").ToString()
                txtStatusNotes.Text = ds.Tables(0).Rows(0).Item("StatusNotes").ToString()
                txtAddInstructions.Text = ds.Tables(0).Rows(0).Item("AddInstructions").ToString()

                If ds.Tables(0).Rows(0).Item("SubmittedToLab").ToString() <> Nothing Or ds.Tables(0).Rows(0).Item("SubmittedToLab").ToString() <> "" Then
                    ViewState("SubmittedToLab") = ds.Tables(0).Rows(0).Item("SubmittedToLab")
                    If ViewState("Admin") = False Then
                        btnSave1.Enabled = False
                        btnReset1.Enabled = False
                        btnSubmit1.Enabled = False
                    End If
                End If

                'If ViewState("pRptID") > 0 Then
                '    ds = AcousticModule.GetAcousticProjectReport(ViewState("pProjID"), ViewState("pRptID"))
                '    If (ds.Tables.Item(0).Rows.Count > 0) Then
                '        lblProjectReportNo.Text = ds.Tables(0).Rows(0).Item("ReportID").ToString()
                '        lblProjectReportNo.Visible = True
                '        ddReportIssuer.SelectedValue = ds.Tables(0).Rows(0).Item("TeamMemberID").ToString()
                '        txtRptDesc.Text = ds.Tables(0).Rows(0).Item("ReportDescription").ToString()
                '    Else 'no record found reset query string pRptID
                '        Response.Redirect("Acoustic_Project_Detail.aspx?pProjID=" & ViewState("pProjID") & "&pRptID=0", False)
                '    End If
                'End If

            End If

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF of BindData

    Protected Sub mnuTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuTabs.MenuItemClick
        mvTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub ' EOF mnuTabs_MenuItemClick

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("Acoustic_Project_Detail.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click, btnSave2.Click
        Try
            lblErrors.Text = Nothing
            lblErrors.Visible = False

            Dim DefaultDate As Date = Date.Today
            'Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim ProjectStatus As String = Nothing
            If ddProjectStatus.Enabled = False Then
                ProjectStatus = "O"
            Else
                ProjectStatus = ddProjectStatus.SelectedValue
            End If

            '*****
            '* Locate the position of the CABBV and SoldTo from ddCustomer
            '*****
            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            Dim tempCABBV As String = Nothing
            Dim tempSoldTo As Integer = Nothing
            'If Not (Pos = 0) Then
            '    tempCABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
            '    tempSoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            'End If

            If ViewState("pProjID") <> 0 Then
                '***************
                '* Requestor will Update Data and submit to R&D
                '***************
                UpdateRecord(ProjectStatus, False)

                '**************
                '* Reload the data
                '**************
                BindData()

            Else

                '***************
                '* Save Data
                '***************
                AcousticModule.InsertAcousticLabRequest(txtTestDescription.Text, ProjectStatus, ddRequester.SelectedValue, txtDateRequested.Text, ddProgram.SelectedValue, txtNoOfTestSamples.Text, txtSampleDesc.Text, txtProjectGoals.Text, txtBackground.Text, ddRptReq.SelectedValue, txtSpecialInst.Text, txtDevExp.Text, txtTestCmpltDt.Text, ViewState("DefaultUser"), DefaultDate)

                '***************
                '* Locate Max RequestID
                '***************
                Dim ds As DataSet = Nothing
                ds = AcousticModule.GetLastProjectID(ProjectStatus, txtTestDescription.Text, ddRequester.SelectedValue, txtDateRequested.Text, ddProgram.SelectedValue, txtSampleDesc.Text, ViewState("DefaultUser"), DefaultDate)

                ViewState("pProjID") = ds.Tables(0).Rows(0).Item("LastProjectID").ToString

                '***************
                '* Redirect user back to the page.
                '***************
                Response.Redirect("Acoustic_Project_Detail.aspx?pProjID=" & ViewState("pProjID"), False)
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnSave1_Click

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            '*****
            '* Delete Record
            '*****
            AcousticModule.DeleteAcousticLabRequest(ViewState("pProjID"))

            Response.Redirect("Acoustic_Lab_Testing_List.aspx", False)
        Catch ex As Exception
            lblErrors.Text = "Error occurred during delete record.  Please contact the IS Application Group." & ex.Message
            lblErrors.Visible = "True"
        End Try
    End Sub 'EOF btnDelete_Click

    Protected Sub btnSubmit1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit1.Click
        Try
            '*****
            '* Locate the position of the CABBV and SoldTo from ddCustomer
            '*****
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            Dim tempCABBV As String = Nothing
            Dim tempSoldTo As Integer = Nothing
            If Not (Pos = 0) Then
                tempCABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                tempSoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            End If

            Dim EmpName As String = Nothing
            Dim EmailTO As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing

            Dim CurrentEmpEmail As String = Nothing
            If HttpContext.Current.Request.Cookies("UGNDB_User_Email") IsNot Nothing Then
                CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailFrom = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailCC = CurrentEmpEmail
            Else
                CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                EmailFrom = "Database.Notifications@ugnauto.com"
            End If

            '********
            '* Only users with valid email accounts can submit a test request.
            '********
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pProjID") <> 0 Then
                    'Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                    Dim DefaultDate As Date = Date.Today

                    ''***************
                    ''Verify that atleast one Commodity Info entry has been entered before submitting to LAB
                    ''***************
                    Dim ds As DataSet
                    ds = AcousticModule.GetAcousticProjectCommodity(ViewState("pProjID"))
                    If (ds.Tables.Item(0).Rows.Count = 0) Then 'If missing kick user out from submission.
                        mvTabs.ActiveViewIndex = Int32.Parse(0)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(0).Selected = True

                        lblErrors.Text = "Atleast one Commodity entry is required for submission."
                        lblErrors.Visible = True
                        lblErrors.Font.Size = 12
                        MaintainScrollPositionOnPostBack = False
                        Exit Sub
                    Else

                        ''**************************************************************************
                        ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                        ''**************************************************************************
                        Dim LabDS As DataSet
                        Dim i As Integer = 0

                        ''*******************************
                        ''Locate Default R&D recepient(s)
                        ''*******************************
                        LabDS = commonFunctions.GetTeamMemberBySubscription(62)
                        ''Check that the recipient(s) is a valid Team Member
                        If LabDS.Tables.Count > 0 And (LabDS.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To LabDS.Tables.Item(0).Rows.Count - 1
                                If (LabDS.Tables(0).Rows(i).Item("Email") <> Nothing) And (LabDS.Tables(0).Rows(i).Item("WorkStatus") = 1) Or (LabDS.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                    If EmailTO = Nothing Then
                                        EmailTO = LabDS.Tables(0).Rows(i).Item("Email")
                                    Else
                                        EmailTO = EmailTO & ";" & LabDS.Tables(0).Rows(i).Item("Email")
                                    End If
                                    If EmpName = Nothing Then
                                        EmpName = LabDS.Tables(0).Rows(i).Item("DisplayTMName") & ", "
                                    Else
                                        EmpName = EmpName & LabDS.Tables(0).Rows(i).Item("DisplayTMName") & ", "
                                    End If

                                End If
                            Next
                        End If

                        If EmailTO <> Nothing Then
                            ''***************
                            ''Save any changed data prior to submitting to R&D
                            ''**************
                            UpdateRecord(ddProjectStatus.SelectedValue, True)


                            ''**************************
                            ''Build email
                            ''**************************
                            Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                            Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                            Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                            MyMessage.Subject = "Acoustic Lab Request: Project No. " & ViewState("pProjID") & " for " & txtTestDescription.Text & " due by " & txtTestCmpltDt.Text
                            MyMessage.Body = "<font size='2' face='Verdana'>" & EmpName & "</font>"
                            MyMessage.Body &= "<p>Please review Project No. " & ViewState("pProjID") & " for " & txtTestDescription.Text & ". <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/Acoustic/Acoustic_Project_Detail.aspx?pProjID=" & ViewState("pProjID") & "'>Click here</a> to access the record.</font></p>"

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
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Acoustic Testing", ViewState("pProjID"))
                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                            Catch ex As Exception
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."

                                UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjID"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
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
                            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                        Else
                            lblErrors.Text = "Email Submission Cancelled: Invalid email address found. Please submit a Database Requestor."
                            lblErrors.Visible = True
                        End If
                    End If 'EOF Commodity Verification
                Else
                    lblErrors.Text = "Error found with submission. Please submit a Database Requestor."
                    lblErrors.Visible = True
                End If
            Else
                lblErrors.Text = "You do not have a valid email account. Request Cancelled."
                lblErrors.Visible = True
            End If

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
    End Sub 'EOF btnSubmit1_Click

    Public Function EmailBody(ByVal MyMessage As MailMessage) As String
        MyMessage.Body &= "<table width='80%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
        MyMessage.Body &= "<td width='388'><font size='2' face='Verdana'><strong>Project No.</strong></font></td>"
        MyMessage.Body &= "<td width='423><font size='2' face='Verdana'><strong>Test Description</strong></font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr style='border-color:white;'>"
        MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ViewState("pProjID") & "</font></td>"
        MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & txtTestDescription.Text & "</font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
        MyMessage.Body &= "<td width='423'><font size='2' face='Verdana'><strong>Project Requester </strong></font></td>"
        MyMessage.Body &= "<td width='388'><font size='2' face='Verdana'><strong>Date Requested </strong></font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr style='border-color:white;'>"
        MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ddRequester.SelectedItem.Text & "</font></td>"
        MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & txtDateRequested.Text & "</font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:white;'>"
        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Customer </strong></font></td>"
        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Program </strong></font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ddCustomer.SelectedItem.Text & "</font></td>"
        MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & ddProgram.SelectedItem.Text & "</font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td colspan='2'>"

        ''***************************************************
        ''Get list of Commodity information for display
        ''***************************************************
        MyMessage.Body &= "<table width='100%' border='0'>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Commodity</strong></font></td>"
        MyMessage.Body &= "</tr>"
        Dim dsCP As DataSet
        dsCP = AcousticModule.GetAcousticProjectCommodity(ViewState("pProjID"))
        If dsCP.Tables.Count > 0 And (dsCP.Tables.Item(0).Rows.Count > 0) Then
            For i = 0 To dsCP.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= "<tr style='border-color:white'><font size='2' face='Verdana'>"
                MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("CommodityName") & "&nbsp;</td>"
                MyMessage.Body &= "</font></tr>"
            Next
        End If
        MyMessage.Body &= "</Table>"
        MyMessage.Body &= "</td></tr>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'>"
        MyMessage.Body &= "<td colspan='2'><font size='2' face='Verdana'><strong>Sample Description </strong></font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr style='border-color:white;'>"
        MyMessage.Body &= "<td colspan='2'><font size='2' face='Verdana'>" & txtSampleDesc.Text & "</font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr bgcolor='#EBEBEB'>"
        MyMessage.Body &= "<td colspan='2'><font size='2' face='Verdana'><strong>Project Goals/Description </strong></font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr style='border-color:white;'>"
        MyMessage.Body &= "<td colspan='2'><font size='2' face='Verdana'>" & txtProjectGoals.Text & "</font></td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "</Table>"

        Return True
    End Function 'eof EmailBody

    Public Function UpdateRecord(ByVal RecStatus As String, ByVal RecSubmitted As Boolean) As String
        Try
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")

            AcousticModule.UpdateAcousticLabRequest(ViewState("pProjID"), txtTestDescription.Text, RecStatus, ddRequester.SelectedValue, txtDateRequested.Text, ddProgram.SelectedValue, txtNoOfTestSamples.Text, txtSampleDesc.Text, txtProjectGoals.Text, txtBackground.Text, IIf(ddRptReq.SelectedValue = "", 0, ddRptReq.SelectedValue), txtSpecialInst.Text, IIf(ddEngineer.SelectedValue = "", 0, ddEngineer.SelectedValue), IIf(ddTechnician.SelectedValue = "", 0, ddTechnician.SelectedValue), txtReiterRefNo.Text, IIf(txtEstCost.Text = "", 0, txtEstCost.Text), IIf(txtActualCost.Text = "", 0, txtActualCost.Text), txtDevExp.Text, txtProjIntDt.Text, txtEstCmpltDt.Text, txtActualCmplDt.Text, txtAddInstructions.Text, txtStatusNotes.Text, txtTestCmpltDt.Text, ViewState("DefaultUser"), IIf(RecSubmitted = False, txtSubmittedToLab.Text, DefaultDate))

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

    Protected Sub gvCommodity_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCommodity.RowCommand
        ''***
        ''This section allows the inserting of a new row when save button is clicked from the footer.
        ''***
        If e.CommandName = "Insert" Then
            ''Insert data
            Dim ddCommodity As DropDownList
            Dim ProjectID As Integer

            If gvCommodity.Rows.Count = 0 Then
                '' We are inserting through the DetailsView in the EmptyDataTemplate
                Return
            End If

            '' Only perform the following logic when inserting through the footer
            ProjectID = ViewState("pProjID")
            odsCommodity.InsertParameters("ProjectID").DefaultValue = ProjectID

            '' Only perform the following logic when inserting through the footer
            ddCommodity = CType(gvCommodity.FooterRow.FindControl("ddCommodity"), DropDownList)
            odsCommodity.InsertParameters("CommodityID").DefaultValue = ddCommodity.SelectedValue

            odsCommodity.Insert()
        End If

        ''***
        ''This section clears out the values in the footer row
        ''***
        If e.CommandName = "Undo" Then
            Dim ddCommodity As DropDownList
            ddCommodity = CType(gvCommodity.FooterRow.FindControl("ddCommodity"), DropDownList)
            ddCommodity.SelectedValue = Nothing
        End If

    End Sub 'EOF gvCommodity_RowCommand

    Protected Sub gvCommodity_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCommodity.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(1).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim commodity As Acoustic.Acoustic_Project_CommoditiesRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Acoustic.Acoustic_Project_CommoditiesRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Commodity (" & DataBinder.Eval(e.Row.DataItem, "CommodityName") & ")?');")
                End If
            End If
        End If
    End Sub 'EOF gvCommodity_RowDataBound

#Region "Insert Empty GridView Work-Around for gvCommodity"
    Private Property LoadDataEmpty_Commodity() As Boolean

        Get
            If ViewState("LoadDataEmpty_Commodity") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Commodity"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Commodity") = value
        End Set
    End Property

    Protected Sub odsCommodity_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCommodity.Selected

        Dim ProjectID As String = HttpContext.Current.Request.QueryString("pProjID")

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        'Dim ds As DataSet = CType(e.ReturnValue, DataSet)
        'Dim dt As DataTable = ds.Tables(0)
        Dim dt As Acoustic.Acoustic_Project_CommoditiesDataTable = CType(e.ReturnValue, Acoustic.Acoustic_Project_CommoditiesDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty_Commodity = True
        Else
            LoadDataEmpty_Commodity = False
        End If
    End Sub

    Protected Sub gvCommodity_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCommodity.RowCreated
        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Commodity
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If
    End Sub
#End Region 'EOF Inert Empty GridView Work-Around for gvCommodity

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim DefaultDate As Date = Date.Today
            lblMessageView4.Text = Nothing
            lblMessageView4.Visible = False

            'If ViewState("pRptID") > 0 And ViewState("pRptID") <> Nothing Then
            If uploadFile.HasFile Then
                If uploadFile.PostedFile.ContentLength <= 3500000 Then
                    Dim FileExt As String
                    FileExt = System.IO.Path.GetExtension(uploadFile.FileName)
                    Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                    Dim r As Regex = New Regex(pat)
                    Dim m As Match = r.Match(uploadFile.PostedFile.FileName)

                    '** Original code with use of MS Office 2003 or older **/
                    ' ''Dim BinaryFile(uploadFile.PostedFile.InputStream.Length) As Byte
                    ' ''Dim EncodeType As String = uploadFile.PostedFile.ContentType
                    ' ''uploadFile.PostedFile.InputStream.Read(BinaryFile, 0, BinaryFile.Length)
                    ' ''Dim FileSize As Integer = uploadFile.PostedFile.ContentLength

                    '** With use of MS Office 2007 **/
                    Dim FileSize As Integer = Convert.ToInt32(uploadFile.PostedFile.InputStream.Length)
                    Dim EncodeType As String = uploadFile.PostedFile.ContentType
                    Dim BinaryFile As [Byte]() = New [Byte](FileSize) {}
                    uploadFile.PostedFile.InputStream.Read(BinaryFile, 0, FileSize)

                    If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".doc") Or (FileExt = ".xlsx") Or (FileExt = ".docx") Then
                        ''*************
                        '' Display File Info
                        ''*************
                        lblMessageView4.Text = "File name: " & uploadFile.FileName & "<br/>" & _
                        "File Size: " & CType((FileSize / 1024), Integer) & " KB<br/>"
                        lblMessageView4.Visible = True
                        lblMessageView4.Width = 500
                        lblMessageView4.Height = 30

                        ''*************
                        '' Save Record
                        ''*************
                        AcousticModule.InsertAcousticProjectReport(ViewState("pProjID"), ddReportIssuer.SelectedValue, txtRptDesc.Text, uploadFile.FileName, EncodeType, BinaryFile, FileSize, ViewState("DefaultUser"))

                        gvProjectReport.DataBind()
                        revUploadFile.Enabled = False
                        txtRptDesc.Text = Nothing
                    End If
                Else
                    lblMessageView4.Text = "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
                    lblMessageView4.Visible = True
                    btnUpload.Enabled = False

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
    End Sub 'EOF btnSaveRpt

    Protected Sub btnReset3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset3.Click
        Response.Redirect("Acoustic_Project_Detail.aspx?pProjID=" & ViewState("pProjID") & "&pRptID=" & ViewState("pRptID"), False)

    End Sub 'EOF btnReset3

    Protected Sub gvProjectReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvProjectReport.RowDataBound
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
                    Dim vVar As Acoustic.Acoustic_Project_ReportRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Acoustic.Acoustic_Project_ReportRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Report No: (" & DataBinder.Eval(e.Row.DataItem, "ReportID") & ")?');")

                End If
            End If
        End If
    End Sub 'EOF gvProjectReport_RowDataBound

    Protected Sub gvProjectReport_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvProjectReport.RowCommand
        ''***
        ''This section allows the inserting of a new row when save button is clicked from the footer.
        ''***
        If e.CommandName = "Delete" Then
            ''Reprompt current page
            Response.Redirect("Acoustic_Project_Detail.aspx?pProjID=" & ViewState("pProjID") & "&pRptID=0", False)
        End If
    End Sub 'EOF gvProjectReport_RowCommand

    Protected Sub ddProjectStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProjectStatus.SelectedIndexChanged
        ''Used to Default Actual Completion Date with system date
        If ddProjectStatus.SelectedValue = "C" Then
            txtActualCmplDt.Text = Date.Today
            btnNotify.Enabled = True
            txtComments.Enabled = True
        ElseIf ddProjectStatus.SelectedValue = "R" Then
            btnNotify.Enabled = True
            txtComments.Enabled = True
        ElseIf ddProjectStatus.SelectedValue = "O" Then
            btnNotify.Enabled = True
            txtComments.Enabled = True
        Else
            btnNotify.Enabled = False
            txtComments.Enabled = False
        End If
    End Sub

    Protected Sub btnNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNotify.Click
        Try
            '*****
            '* Locate the position of the CABBV and SoldTo from ddCustomer
            '*****
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            Dim tempCABBV As String = Nothing
            Dim tempSoldTo As Integer = Nothing
            If Not (Pos = 0) Then
                tempCABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                tempSoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            End If

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
                If ViewState("pProjID") <> 0 Then
                    'Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                    Dim DefaultDate As Date = Date.Today

                    ''***************
                    ''Verify that atleast one Commodity Info entry has been entered before submitting to LAB
                    ''***************
                    Dim ds As DataSet
                    ds = AcousticModule.GetAcousticProjectCommodity(ViewState("pProjID"))
                    If (ds.Tables.Item(0).Rows.Count = 0) Then 'If missing kick user out from submission.
                        mvTabs.ActiveViewIndex = Int32.Parse(1)
                        mvTabs.GetActiveView()
                        mnuTabs.Items(0).Selected = True

                        lblErrors.Text = "Atleast one Commodity entry is required for submission."
                        lblErrors.Visible = True
                        lblerrors.font.size = 12
                        MaintainScrollPositionOnPostBack = False
                        Exit Sub
                    Else
                        ''**************************************************************************
                        ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                        ''**************************************************************************
                        Dim LabDS As DataSet
                        Dim i As Integer = 0

                        ''*******************************
                        ''Locate Default R&D recepient(s)
                        ''*******************************
                        LabDS = SecurityModule.GetTeamMember(ddRequester.SelectedValue, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                        ''Check that the recipient(s) is a valid Team Member
                        If LabDS.Tables.Count > 0 And (LabDS.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To LabDS.Tables.Item(0).Rows.Count - 1
                                If (LabDS.Tables(0).Rows(i).Item("Email") <> Nothing) Or (LabDS.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                    If EmailTO = Nothing Then
                                        EmailTO = LabDS.Tables(0).Rows(i).Item("Email")
                                    Else
                                        EmailTO = EmailTO & ";" & LabDS.Tables(0).Rows(i).Item("Email")
                                    End If
                                    If EmpName = Nothing Then
                                        EmpName = LabDS.Tables(0).Rows(i).Item("FirstName") & ", "
                                    Else
                                        EmpName = EmpName & LabDS.Tables(0).Rows(i).Item("FirstName") & ", "
                                    End If

                                End If
                            Next
                        End If

                        If EmailTO <> Nothing Then
                            ''***************
                            ''Save any changed data prior to submitting to R&D
                            ''**************
                            UpdateRecord(ddProjectStatus.SelectedValue, False)


                            ''**************************
                            ''Build email
                            ''**************************
                            Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                            Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                            Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                            MyMessage.Subject = "Acoustic Lab Request: Project No. " & ViewState("pProjID") & " for " & txtTestDescription.Text & " - " & ddProjectStatus.SelectedItem.Text.ToUpper & " as of " & txtActualCmplDt.Text
                            MyMessage.Body = "<font size='2' face='Verdana'>" & EmpName & "</font>"
                            MyMessage.Body &= "<p> Project No. " & ViewState("pProjID") & " for " & txtTestDescription.Text & " was completed on " & txtActualCmplDt.Text & ". <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/Acoustic/Acoustic_Project_Detail.aspx?pProjID=" & ViewState("pProjID") & "'>Click here</a> to access the record.</font></p>"
                            If txtComments.Text <> Nothing Then
                                MyMessage.Body &= "<p> <b>COMMENTS:</b> " & txtComments.Text & "</p>"
                            End If

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
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Acoustic Testing", ViewState("pProjID"))
                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                            Catch ex As Exception
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjID"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
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
                            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                        Else
                            lblErrors.Text = "Email Submission Cancelled: Invalid email address found. Please submit a Database Requestor."
                            lblErrors.Visible = True
                        End If
                    End If 'EOF Commodity Verification
                Else
                    lblErrors.Text = "Error found with submission. Please submit a Database Requestor."
                    lblErrors.Visible = True
                End If
            Else
                lblErrors.Text = "You do not have a valid email account. Request Cancelled."
                lblErrors.Visible = True
            End If

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
    End Sub 'EOF btnNotify_Click

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
End Class