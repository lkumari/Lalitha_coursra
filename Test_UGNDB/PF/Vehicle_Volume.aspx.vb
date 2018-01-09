' *******************************************************************************************
' Name:	Vehicle_Volume.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'
' Date		    Author	    
' 03/19/2008    LRey			Created .Net application
' 04/22/2008    LRey            commented out all references to DABBV per Mike E.
' 08/11/2008    LRey            Added SoldTo to update/insert/delete/reset events
' *******************************************************************************************
Imports System.Collections.Specialized
Imports System.Collections.Generic
Partial Class PMT_Vehicle
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to define the primary record
            If HttpContext.Current.Request.QueryString("sPGMID") <> "" Then
                ViewState("sPGMID") = HttpContext.Current.Request.QueryString("sPGMID")
            Else
                ViewState("sPGMID") = 0
            End If

            If HttpContext.Current.Request.QueryString("sPlatID") <> "" Then
                ViewState("sPlatID") = HttpContext.Current.Request.QueryString("sPlatID")
            Else
                ViewState("sPlatID") = 0
            End If


            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Vehicle Volume"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Planning and Forecasting </b> > <a href='Vehicle_List.aspx'><b>Vehicle Volume Search</b></a> > Vehicle Volume"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("PFExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                btnDelete.Attributes.Add("onclientclick", "return confirm('Are you sure you want to delete this record?');")
                BindCriteria()
                If ViewState("sPGMID") <> 0 Then
                    BindDataPerRecord() 'used to bind data at the record level
                Else
                    btnDelete.Visible = False
                End If
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"


            txtNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNotes.ClientID + ");")
            txtNotes.Attributes.Add("maxLength", "400")


        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

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
            btnAdd.Enabled = False
            btnInsert.Enabled = False
            btnSave.Enabled = False
            btnReset.Enabled = False
            btnDelete.Enabled = False
            btnCopy.Enabled = False
            ViewState("Admin") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 10 'Vehicle form id
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

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
                                        btnAdd.Enabled = True
                                        btnSave.Enabled = True
                                        btnReset.Enabled = True
                                        btnDelete.Enabled = True
                                        btnCopy.Enabled = True
                                        If ViewState("sPGMID") <> Nothing Then
                                            ViewState("Admin") = True
                                            AEExtender.Collapsed = False
                                            btnInsert.Enabled = True
                                        Else
                                            AEExtender.Collapsed = True
                                        End If
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        btnAdd.Enabled = True
                                        btnSave.Enabled = True
                                        btnReset.Enabled = True
                                        btnDelete.Enabled = True
                                        btnCopy.Enabled = True
                                        If ViewState("sPGMID") <> Nothing Then
                                            ViewState("Admin") = True
                                            AEExtender.Collapsed = False
                                            btnInsert.Enabled = True
                                        Else
                                            AEExtender.Collapsed = True
                                        End If
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        btnAdd.Enabled = True
                                        btnSave.Enabled = True
                                        btnReset.Enabled = True
                                        btnDelete.Enabled = False
                                        btnCopy.Enabled = True
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        btnAdd.Enabled = False
                                        btnSave.Enabled = False
                                        btnReset.Enabled = False
                                        btnDelete.Enabled = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        btnAdd.Enabled = False
                                        btnSave.Enabled = True
                                        btnReset.Enabled = True
                                        btnDelete.Enabled = False
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        btnAdd.Enabled = False
                                        btnSave.Enabled = False
                                        btnReset.Enabled = False
                                        btnDelete.Enabled = False
                                End Select 'EOF of "Select Case iRoleID"
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
#End Region 'EOF Form Level Security

    Protected Sub BindCriteria()
        Try

            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Customer control for selection criteria for search
            ds = commonFunctions.GetCustomer(False)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("ddCustomerDesc").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("ddCustomerValue").ColumnName.ToString()
                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Customer Plant control for selection criteria for search
            ' ''ds = commonFunctions.GetProgram("", "", "")
            ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
            ' ''    ddProgram.DataSource = ds
            ' ''    ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
            ' ''    ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName.ToString()
            ' ''    ddProgram.DataBind()
            ' ''    ddProgram.Items.Insert(0, "")
            ' ''End If
            ds = commonFunctions.GetPlatformProgram(0, 0, "", "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddProgram.DataSource = ds
                ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramModelPlatformAssembly").ColumnName.ToString()
                ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName.ToString()
                ddProgram.DataBind()
                ddProgram.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Planning Year control for selection criteria for search
            ds = commonFunctions.GetYear("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Account Manager control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(18) '**SubscriptionID 18 is used for Account Manager
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddAccountManager.DataSource = ds
                ddAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddAccountManager.DataBind()
                ddAccountManager.Items.Insert(0, "")
            End If
            If ViewState("sPGMID") = 0 Then
                ddAccountManager.SelectedValue = HttpContext.Current.Session("UserId")
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub 'EOF BindCriteria

    Protected Sub BindDataPerRecord()
        ''*************************************************
        ''following code used to bind data at the record level
        ''*************************************************
        Dim ds As DataSet = New DataSet
        Dim PGMID As Integer = HttpContext.Current.Request.QueryString("sPGMID")
        Dim PlatformID As Integer = HttpContext.Current.Request.QueryString("sPlatID")
        Dim Year As Integer = HttpContext.Current.Request.QueryString("sYear")
        Dim CABBV As String = HttpContext.Current.Request.QueryString("sCABBV")
        Dim SoldTo As Integer = HttpContext.Current.Request.QueryString("sSoldTo")
        ' ''Dim DABBV As String = HttpContext.Current.Request.QueryString("sDABBV")

        Try
            If ViewState("sPGMID") <> 0 Then
                ds = PFModule.GetVehicle(PGMID, Year, CABBV, SoldTo, 0, "")

                If (ds.Tables.Item(0).Rows.Count > 0) Then
                    Dim ds2 As DataSet = New DataSet
                    If PlatformID <> Nothing Or PlatformID <> 0 Then
                        'bind data from Platform_Maint
                        ds2 = commonFunctions.GetPlatform(PlatformID, "", "", "", "", "")
                        If (ds2.Tables.Item(0).Rows.Count > 0) Then
                            lblPlatformName.Text = ds2.Tables(0).Rows(0).Item("PlatformName").ToString()
                            lblOEM.Text = ds2.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
                            lblUGNBusiness.Text = IIf(ds2.Tables(0).Rows(0).Item("UGNBusiness") = True, "Yes", "No")
                            lblCurrentPlatform.Text = IIf(ds2.Tables(0).Rows(0).Item("CurrentPlatform") = True, "Yes", "No")
                            lblBegYear.Text = ds2.Tables(0).Rows(0).Item("BegYear").ToString()
                            lblEndYear.Text = ds2.Tables(0).Rows(0).Item("EndYear").ToString()
                        End If
                    End If

                    'bind data from Program_Main
                    Dim ds3 As DataSet = New DataSet
                    ds3 = commonFunctions.GetPlatformProgram(PlatformID, PGMID, "", "", "")
                    If (ds3.Tables.Item(0).Rows.Count > 0) Then
                        lblMake.Text = ds3.Tables(0).Rows(0).Item("Make").ToString()
                        lblPgmCode.Text = ds3.Tables(0).Rows(0).Item("BPCSProgramRef").ToString()
                        lblPgmGen.Text = ds3.Tables(0).Rows(0).Item("ProgramSuffix").ToString()
                        lblModelName.Text = ds3.Tables(0).Rows(0).Item("ProgramName").ToString()
                        lblAPL.Text = ds3.Tables(0).Rows(0).Item("Assembly_Plant_Location").ToString()
                        lblState.Text = ds3.Tables(0).Rows(0).Item("AssemblyState").ToString()
                        lblCountry.Text = ds3.Tables(0).Rows(0).Item("AssemblyCountry").ToString()
                        lblSOP.Text = ds3.Tables(0).Rows(0).Item("SOP").ToString()
                        lblSOPMM.Text = ds3.Tables(0).Rows(0).Item("SOPMM").ToString()
                        lblSOPYY.Text = ds3.Tables(0).Rows(0).Item("SOPYY").ToString()
                        lblEOP.Text = ds3.Tables(0).Rows(0).Item("EOP").ToString()
                        lblEOPMM.Text = ds3.Tables(0).Rows(0).Item("EOPMM").ToString()
                        lblEOPYY.Text = ds3.Tables(0).Rows(0).Item("EOPYY").ToString()
                        lblUGNBiz2.Text = IIf(ds3.Tables(0).Rows(0).Item("UGNBusiness") = True, "Yes", "No")
                        lblVehicleType.Text = ds3.Tables(0).Rows(0).Item("ddVehicleType").ToString()
                        'lblBodyStyle.Text = ds3.Tables(0).Rows(0).Item("ddBodyStyle").ToString()
                        lblRecStatus.text = ds3.Tables(0).Rows(0).Item("RecStatus").ToString()
                        If lblRecStatus.text = "INACTIVE" Then
                            btnInsert.Enabled = False
                        Else
                            btnInsert.Enabled = True
                        End If
                    End If

                    'bind data from Program_Volume
                    Dim ds4 As DataSet = New DataSet
                    ds4 = commonFunctions.GetProgramVolume(PGMID, Year)
                    If (ds4.Tables.Item(0).Rows.Count > 0) Then
                        lblYearID.Text = ds4.Tables(0).Rows(0).Item("YearID").ToString()
                        lblAnnualVolume.Text = ds4.Tables(0).Rows(0).Item("AnnualVolume").ToString()
                        lblQtr1.Text = ds4.Tables(0).Rows(0).Item("Q1Volume").ToString()
                        lblQtr2.Text = ds4.Tables(0).Rows(0).Item("Q2Volume").ToString()
                        lblQtr3.Text = ds4.Tables(0).Rows(0).Item("Q3Volume").ToString()
                        lblQtr4.Text = ds4.Tables(0).Rows(0).Item("Q4Volume").ToString()
                        lblJan.Text = ds4.Tables(0).Rows(0).Item("JanVolume").ToString()
                        lblFeb.Text = ds4.Tables(0).Rows(0).Item("FebVolume").ToString()
                        lblMar.Text = ds4.Tables(0).Rows(0).Item("MarVolume").ToString()
                        lblApr.Text = ds4.Tables(0).Rows(0).Item("AprVolume").ToString()
                        lblMay.Text = ds4.Tables(0).Rows(0).Item("MayVolume").ToString()
                        lblJun.Text = ds4.Tables(0).Rows(0).Item("JunVolume").ToString()
                        lblJul.Text = ds4.Tables(0).Rows(0).Item("JulVolume").ToString()
                        lblAug.Text = ds4.Tables(0).Rows(0).Item("AugVolume").ToString()
                        lblSep.Text = ds4.Tables(0).Rows(0).Item("SepVolume").ToString()
                        lblOct.Text = ds4.Tables(0).Rows(0).Item("OctVolume").ToString()
                        lblNov.Text = ds4.Tables(0).Rows(0).Item("NovVolume").ToString()
                        lblDec.Text = ds4.Tables(0).Rows(0).Item("DecVolume").ToString()
                    End If

                    ddProgram.SelectedValue = ds.Tables(0).Rows(0).Item("ProgramID").ToString()
                    ddYear.SelectedValue = ds.Tables(0).Rows(0).Item("PlanningYear").ToString()
                    ddCustomer.SelectedValue = ds.Tables(0).Rows(0).Item("ddCustomerValue").ToString()
                    txtVolume.Text = ds.Tables(0).Rows(0).Item("AnnualVolume").ToString()
                    ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AcctMgrID").ToString()
                    txtSOP.Text = ds.Tables(0).Rows(0).Item("SOP").ToString()
                    txtEOP.Text = ds.Tables(0).Rows(0).Item("EOP").ToString()
                    txtJan.Text = ds.Tables(0).Rows(0).Item("JanVolume").ToString()
                    txtFeb.Text = ds.Tables(0).Rows(0).Item("FebVolume").ToString()
                    txtMar.Text = ds.Tables(0).Rows(0).Item("MarVolume").ToString()
                    txtApr.Text = ds.Tables(0).Rows(0).Item("AprVolume").ToString()
                    txtMay.Text = ds.Tables(0).Rows(0).Item("MayVolume").ToString()
                    txtJun.Text = ds.Tables(0).Rows(0).Item("JunVolume").ToString()
                    txtJul.Text = ds.Tables(0).Rows(0).Item("JulVolume").ToString()
                    txtAug.Text = ds.Tables(0).Rows(0).Item("AugVolume").ToString()
                    txtSep.Text = ds.Tables(0).Rows(0).Item("SepVolume").ToString()
                    txtOct.Text = ds.Tables(0).Rows(0).Item("OctVolume").ToString()
                    txtNov.Text = ds.Tables(0).Rows(0).Item("NovVolume").ToString()
                    txtDec.Text = ds.Tables(0).Rows(0).Item("DecVolume").ToString()

                    lblPrevAnnualVolume.Text = ds.Tables(0).Rows(0).Item("AnnualVolume").ToString()
                    lblPrevAcctMgr.Text = ds.Tables(0).Rows(0).Item("AcctMgrID").ToString()
                    lblPrevSOP.Text = ds.Tables(0).Rows(0).Item("SOP").ToString()
                    lblPrevEop.Text = ds.Tables(0).Rows(0).Item("EOP").ToString()
                    lblPrevJan.Text = ds.Tables(0).Rows(0).Item("JanVolume").ToString()
                    lblPrevFeb.Text = ds.Tables(0).Rows(0).Item("FebVolume").ToString()
                    lblPrevMar.Text = ds.Tables(0).Rows(0).Item("MarVolume").ToString()
                    lblPrevApr.Text = ds.Tables(0).Rows(0).Item("AprVolume").ToString()
                    lblPrevMay.Text = ds.Tables(0).Rows(0).Item("MayVolume").ToString()
                    lblPrevJun.Text = ds.Tables(0).Rows(0).Item("JunVolume").ToString()
                    lblPrevJul.Text = ds.Tables(0).Rows(0).Item("JulVolume").ToString()
                    lblPrevAug.Text = ds.Tables(0).Rows(0).Item("AugVolume").ToString()
                    lblPrevSep.Text = ds.Tables(0).Rows(0).Item("SepVolume").ToString()
                    lblPrevOct.Text = ds.Tables(0).Rows(0).Item("OctVolume").ToString()
                    lblPrevNov.Text = ds.Tables(0).Rows(0).Item("NovVolume").ToString()
                    lblPrevDec.Text = ds.Tables(0).Rows(0).Item("DecVolume").ToString()

                    ''***********************
                    ''* Disable primary keys
                    ''***********************
                    ddProgram.Enabled = False
                    ddYear.Enabled = False
                    ddCustomer.Enabled = False

                    ''***********************
                    ''* Disable monthly volumes should the Planning Year be greater 
                    ''* than two years from current year.
                    ''***********************
                    If ds.Tables(0).Rows(0).Item("PlanningYear").ToString() > (Date.Today.Year + 1) Then
                        txtJan.Enabled = False
                        txtFeb.Enabled = False
                        txtMar.Enabled = False
                        txtApr.Enabled = False
                        txtMay.Enabled = False
                        txtJun.Enabled = False
                        txtJul.Enabled = False
                        txtAug.Enabled = False
                        txtSep.Enabled = False
                        txtOct.Enabled = False
                        txtNov.Enabled = False
                        txtDec.Enabled = False
                    End If

                    DisableMonthlyTextBoxes()
                End If
            End If 'EOF PGMID
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred with data bind.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = "True"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF BindDataPerRecord
    Protected Sub DisableMonthlyTextBoxes()
        Try

            Dim SOPYY As Integer = 0
            Dim SOPMM As Integer = 0
            Select Case Len(txtSOP.Text)
                Case 8
                    SOPYY = txtSOP.Text.Substring(4, 4)
                    SOPMM = txtSOP.Text.Substring(0, 1)
                Case 10
                    SOPYY = txtSOP.Text.Substring(6, 4)
                    SOPMM = txtSOP.Text.Substring(0, 2)
            End Select

            Dim EOPYY As Integer = 0
            Dim EOPMM As Integer = 0
            Select Case Len(txtEOP.Text)
                Case 8
                    EOPYY = txtEOP.Text.Substring(4, 4)
                    EOPMM = txtEOP.Text.Substring(0, 1)
                Case 10
                    EOPYY = txtEOP.Text.Substring(6, 4)
                    EOPMM = txtEOP.Text.Substring(0, 2)
            End Select

            If ((ddYear.SelectedValue = SOPYY) And (1 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (1 > EOPMM)) Then
                txtJan.Text = 0
                txtJan.Enabled = False
            Else
                txtJan.Enabled = True
            End If

            If ((ddYear.SelectedValue = SOPYY) And (2 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (2 > EOPMM)) Then
                txtFeb.Text = 0
                txtFeb.Enabled = False
            Else
                txtFeb.Enabled = True
            End If

            If ((ddYear.SelectedValue = SOPYY) And (3 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (3 > EOPMM)) Then
                txtMar.Text = 0
                txtMar.Enabled = False
            Else
                txtMar.Enabled = True
            End If

            If ((ddYear.SelectedValue = SOPYY) And (4 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (4 > EOPMM)) Then
                txtApr.Text = 0
                txtApr.Enabled = False
            Else
                txtApr.Enabled = True
            End If

            If ((ddYear.SelectedValue = SOPYY) And (5 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (5 > EOPMM)) Then
                txtMay.Text = 0
                txtMay.Enabled = False
            Else
                txtMay.Enabled = True
            End If

            If ((ddYear.SelectedValue = SOPYY) And (6 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (6 > EOPMM)) Then
                txtJun.Text = 0
                txtJun.Enabled = False
            Else
                txtJun.Enabled = True
            End If

            If ((ddYear.SelectedValue = SOPYY) And (7 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (7 > EOPMM)) Then
                txtJul.Text = 0
                txtJul.Enabled = False
            Else
                txtJul.Enabled = True
            End If

            If ((ddYear.SelectedValue = SOPYY) And (8 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (8 > EOPMM)) Then
                txtAug.Text = 0
                txtAug.Enabled = False
            Else
                txtAug.Enabled = True
            End If

            If ((ddYear.SelectedValue = SOPYY) And (9 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (9 > EOPMM)) Then
                txtSep.Text = 0
                txtSep.Enabled = False
            Else
                txtSep.Enabled = True
            End If

            If ((ddYear.SelectedValue = SOPYY) And (10 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (10 > EOPMM)) Then
                txtOct.Text = 0
                txtOct.Enabled = False
            Else
                txtOct.Enabled = True
            End If

            If ((ddYear.SelectedValue = SOPYY) And (11 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (11 > EOPMM)) Then
                txtNov.Text = 0
                txtNov.Enabled = False
            Else
                txtNov.Enabled = True
            End If

            If ((ddYear.SelectedValue = SOPYY) And (12 < SOPMM)) Or ((ddYear.SelectedValue = EOPYY) And (12 > EOPMM)) Then
                txtDec.Text = 0
                txtDec.Enabled = False
            Else
                txtDec.Enabled = True
            End If


        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred with data bind.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = "True"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            Dim PGMID As Integer = HttpContext.Current.Request.QueryString("sPGMID")
            Dim PlatformID As Integer = HttpContext.Current.Request.QueryString("sPlatID")
            Dim Year As Integer = HttpContext.Current.Request.QueryString("sYear")
            Dim CABBV As String = HttpContext.Current.Request.QueryString("sCABBV")
            Dim SoldTo As Integer = HttpContext.Current.Request.QueryString("sSoldTo")

            If (PGMID = Nothing) And (Year = Nothing) And (CABBV = Nothing) And (SoldTo = Nothing) Then
                Response.Redirect("Vehicle_Volume.aspx", False)
            Else
                Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
                Dim tempCABBV As String = Nothing
                Dim tempSoldTo As Integer = Nothing
                If Not (Pos = 0) Then
                    tempCABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                    tempSoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
                End If

                Response.Redirect("Vehicle_Volume.aspx?sPGMID=" & ddProgram.SelectedValue & "&sPlatID=" & PlatformID & "&sYear=" & ddYear.SelectedValue & "&sCABBV=" & tempCABBV & "&sSoldTo=" & tempSoldTo, False)
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Protected Sub ddCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCustomer.SelectedIndexChanged
        Try
            Dim ds As DataSet = New DataSet
            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            Dim tempCABBV As String = Nothing
            Dim tempSoldTo As Integer = Nothing
            If Not (Pos = 0) Then
                tempCABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                tempSoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            End If

            ''search for SOP in prior Vehicle entry based on ProgramID and CABBV '
            ''NOT USED ACCORDING TO NEW PLATFORM/PROGRAM SOLUTION
            ' ''ds = PFModule.GetVehicleSOP(ddProgram.SelectedValue, tempCABBV, tempSoldTo)
            ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
            ' ''    txtSOP.Text = ds.Tables(0).Rows(0).Item("SOP").ToString()
            ' ''    ddAccountManager.Focus()
            ' ''End If

            ' ''    ''bind existing data to drop down Customer Plant control for selection criteria for search
            ' ''    ds = commonFunctions.GetCustomerDestination(ddCustomer.SelectedValue)
            ' ''    If (ds.Tables.Item(0).Rows.Count > 0) Then
            ' ''        ddCustomerPlant.DataSource = ds
            ' ''        ddCustomerPlant.DataTextField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
            ' ''        ddCustomerPlant.DataValueField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
            ' ''        ddCustomerPlant.DataBind()
            ' ''        ddCustomerPlant.Items.Insert(0, "")
            ' ''        ddCustomerPlant.SelectedIndex = 0
            ' ''    End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim PGMID As Integer = HttpContext.Current.Request.QueryString("sPGMID")
        Dim PlatformID As Integer = ViewState("sPlatID")
        If ViewState("sPGMID") = 0 And ViewState("sPlatID") = 0 Then
            'bind data from Program_Main
            Dim ds As DataSet = New DataSet
            ds = commonFunctions.GetPlatformProgram(0, ddProgram.SelectedValue, "", "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                PlatformID = ds.Tables(0).Rows(0).Item("PlatformID").ToString()
            End If
        End If
        Dim Year As Integer = HttpContext.Current.Request.QueryString("sYear")
        Dim CABBV As String = HttpContext.Current.Request.QueryString("sCABBV")
        Dim SoldTo As Integer = HttpContext.Current.Request.QueryString("sSoldTo")
        Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
        Dim tempCABBV As String = Nothing
        Dim tempSoldTo As Integer = Nothing
        If Not (Pos = 0) Then
            tempCABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
            tempSoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
        End If

        Try
            ' ''If PGMID <> Nothing And Year <> Nothing And CABBV <> Nothing And DABBV <> Nothing Then
            If PGMID <> Nothing And Year <> Nothing And CABBV <> Nothing Then
                ''************************
                ''* Insert into History
                ''************************
                InsertIntoVehicleHistory(Nothing)

                If ViewState("NotesReq") = False Then
                    '*****
                    '* Update Record
                    '*****
                    PFModule.UpdateVehicle(ddProgram.SelectedValue, ddYear.SelectedValue, tempCABBV, tempSoldTo, txtVolume.Text, ddAccountManager.SelectedValue, txtSOP.Text, txtEOP.Text, txtJan.Text, txtFeb.Text, txtMar.Text, txtApr.Text, txtMay.Text, txtJun.Text, txtJul.Text, txtAug.Text, txtSep.Text, txtOct.Text, txtNov.Text, txtDec.Text)

                    ''*********************************************
                    ''Load all values into their designated fields
                    ''*********************************************
                    BindDataPerRecord()

                    lblErrors.Text = Nothing
                    lblErrors.Visible = False
                    txtNotes.Text = Nothing
                    lblReqNotes.Visible = False
                    lblReqNotesText.Visible = False
                    lblMessage.Visible = False
                End If
            Else 'EOF of Update
                '*****
                '* Insert Record
                '*****
                PFModule.InsertVehicle(ddProgram.SelectedValue, ddYear.SelectedValue, tempCABBV, tempSoldTo, txtVolume.Text, ddAccountManager.SelectedValue, txtSOP.Text, txtEOP.Text, txtJan.Text, txtFeb.Text, txtMar.Text, txtApr.Text, txtMay.Text, txtJun.Text, txtJul.Text, txtAug.Text, txtSep.Text, txtOct.Text, txtNov.Text, txtDec.Text)

                Response.Redirect("Vehicle_Volume.aspx?sPGMID=" & ddProgram.SelectedValue & "&sPlatID=" & PlatformID & "&sYear=" & ddYear.SelectedValue & "&sCABBV=" & tempCABBV & "&sSoldTo=" & tempSoldTo, False)


            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during save record.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub
    Protected Sub InsertIntoVehicleHistory(ByVal myMessage As String)
        Try
            Dim PGMID As Integer = HttpContext.Current.Request.QueryString("sPGMID")
            Dim PlatformID As Integer = HttpContext.Current.Request.QueryString("sPlatID")
            Dim Year As Integer = HttpContext.Current.Request.QueryString("sYear")
            Dim CABBV As String = HttpContext.Current.Request.QueryString("sCABBV")
            Dim SoldTo As Integer = HttpContext.Current.Request.QueryString("sSoldTo")
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            ViewState("NotesReq") = False

            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            Dim tempCABBV As String = Nothing
            Dim tempSoldTo As Integer = Nothing
            If Not (Pos = 0) Then
                tempCABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                tempSoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            End If


            Dim SOPYY As Integer = 0
            Dim SOPMM As Integer = 0
            Dim PrevSOPYY As Integer = 0
            Dim PrevSOPMM As Integer = 0
            SOPYY = txtSOP.Text.Substring(6, 4)
            SOPMM = txtSOP.Text.Substring(0, 2)
            PrevSOPYY = lblPrevSOP.Text.Substring(6, 4)
            PrevSOPMM = lblPrevSOP.Text.Substring(0, 2)

            Dim EOPYY As Integer = 0
            Dim EOPMM As Integer = 0
            Dim PrevEOPYY As Integer = 0
            Dim PrevEOPMM As Integer = 0
            EOPYY = txtEOP.Text.Substring(6, 4)
            EOPMM = txtEOP.Text.Substring(0, 2)
            PrevEOPYY = lblPrevEop.Text.Substring(6, 4)
            PrevEOPMM = lblPrevEop.Text.Substring(0, 2)

            Dim NotesReq As Boolean = False
            Dim ProdDateChanged As Boolean = False
            Dim MonthlyVolumeChanged As Boolean = False
            Dim AcctMgrChanged As Boolean = False

            If ddAccountManager.SelectedValue <> lblPrevAcctMgr.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                AcctMgrChanged = True
            End If
            If ((PrevSOPYY <> SOPYY) And (PrevSOPMM <> SOPMM)) Or ((PrevEOPYY <> EOPYY) And (PrevEOPMM <> EOPMM)) Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                ProdDateChanged = True
            End If
            If txtVolume.Text <> lblPrevAnnualVolume.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtJan.Text <> lblPrevJan.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtFeb.Text <> lblPrevFeb.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtMar.Text <> lblPrevMar.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtApr.Text <> lblPrevApr.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtMay.Text <> lblPrevMay.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtJun.Text <> lblPrevJun.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtJul.Text <> lblPrevJul.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtAug.Text <> lblPrevAug.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtSep.Text <> lblPrevSep.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtOct.Text <> lblPrevOct.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtNov.Text <> lblPrevNov.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If
            If txtDec.Text <> lblPrevDec.Text Then
                If txtNotes.Text = Nothing Then
                    NotesReq = True
                End If
                MonthlyVolumeChanged = True
            End If

            If myMessage <> Nothing Then
                NotesReq = False
            End If

            If NotesReq = True Then
                lblReqNotes.Visible = True
                lblReqNotesText.Visible = True
                lblMessage.Text = "Notes is a required field."
                lblMessage.Visible = True
                ViewState("NotesReq") = True
                Exit Sub
            Else
                PFModule.InsertVehicleHistory(ddProgram.SelectedValue, ddYear.SelectedValue, tempCABBV, tempSoldTo, "", ddAccountManager.SelectedValue, lblPrevAcctMgr.Text, txtSOP.Text, lblPrevSOP.Text, txtEOP.Text, lblPrevEop.Text, txtVolume.Text, txtJan.Text, txtFeb.Text, txtMar.Text, txtApr.Text, txtMay.Text, txtJun.Text, txtJul.Text, txtAug.Text, txtSep.Text, txtOct.Text, txtNov.Text, txtDec.Text, lblPrevAnnualVolume.Text, lblPrevJan.Text, lblPrevFeb.Text, lblPrevMar.Text, lblPrevApr.Text, lblPrevMay.Text, lblPrevJun.Text, lblPrevJul.Text, lblPrevAug.Text, lblPrevSep.Text, lblPrevOct.Text, lblPrevNov.Text, lblPrevDec.Text, IIf(myMessage <> Nothing, myMessage, txtNotes.Text), IIf(lblIHSDataUsed.Text = Nothing, 0, 1), ProdDateChanged, MonthlyVolumeChanged, AcctMgrChanged, DefaultTMID, DefaultUser)
            End If


        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during saving to history file.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF InsertIntoVehicleHistory

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        Try
            lblMessage2.Text = Nothing
            lblMessage2.Visible = False
            lblMessage3.Text = Nothing
            lblMessage3.Visible = False

            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            Dim tempCABBV As String = Nothing
            Dim tempSoldTo As Integer = Nothing
            If Not (Pos = 0) Then
                tempCABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                tempSoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            End If
            ''************************
            ''* Verify that there are no parts tied to the Vehicle/Program, prior to deletion.
            ''* If any are found, kick back message to user.
            ''************************
            Dim ds As DataSet = New DataSet
            ds = PFModule.GetProjectedSalesListing("", ViewState("sPGMID"), "", 0, tempCABBV, tempSoldTo, 0, 0, "", 0, ddYear.SelectedValue)
            If (ds.Tables.Item(0).Rows.Count > 0) Then

                lblMessage2.Text = "Delete Canceled.  This vehicle program has parts associated within the planning year " & ddYear.SelectedValue & "."
                lblMessage3.Text = "If this vehicle is a permanent deleteion, you must remove it at the part level. If it will be replaced by another vehicle program, you must create another vehicle entry. Update the parts with new vehicle program. Then after, return to this vehicle program for deletion."
                lblMessage2.Visible = True
                lblMessage3.Visible = True
                Exit Sub

            Else
                ''************************
                ''* Insert into History
                ''************************
                InsertIntoVehicleHistory("Record Deleted")

                '*****
                '* Delete Record
                '*****
                ' ''PFModule.DeleteVehicle(ddProgram.SelectedValue, ddYear.SelectedValue, ddCustomer.SelectedValue, ddCustomerPlant.SelectedValue)
                PFModule.DeleteVehicle(ddProgram.SelectedValue, ddYear.SelectedValue, tempCABBV, tempSoldTo)

                Response.Redirect("Vehicle_List.aspx", False)
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during delete record.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("Vehicle_Volume.aspx", False)
    End Sub

    Protected Sub ddYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddYear.SelectedIndexChanged
        NewRecordInsertDataPullbyPlanningYearSelection()
    End Sub
    Protected Sub NewRecordInsertDataPullbyPlanningYearSelection()
        Try
            lblMessage0.Text = Nothing
            lblMessage0.Visible = False
            lblMessage1.Text = Nothing
            lblMessage1.Visible = False
            btnSave.Enabled = True

            Dim PlatformID As Integer = 0
            Dim SOPYY As Integer = 0
            Dim SOPMM As Integer = 0
            Dim EOPYY As Integer = 0
            Dim EOPMM As Integer = 0

            'bind data from Program_Main
            Dim ds As DataSet = New DataSet
            ds = commonFunctions.GetPlatformProgram(0, ddProgram.SelectedValue, "", "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                PlatformID = ds.Tables(0).Rows(0).Item("PlatformID").ToString()
            End If


            'bind data from Program_Volume
            Dim ds4 As DataSet = New DataSet
            ds4 = commonFunctions.GetProgramVolume(ddProgram.SelectedValue, ddYear.SelectedValue)
            If (ds4.Tables.Item(0).Rows.Count > 0) Then
                txtVolume.Text = ds4.Tables(0).Rows(0).Item("AnnualVolume").ToString()
                lblQtr1.Text = ds4.Tables(0).Rows(0).Item("Q1Volume").ToString()
                lblQtr2.Text = ds4.Tables(0).Rows(0).Item("Q2Volume").ToString()
                lblQtr3.Text = ds4.Tables(0).Rows(0).Item("Q3Volume").ToString()
                lblQtr4.Text = ds4.Tables(0).Rows(0).Item("Q4Volume").ToString()
                txtJan.Text = ds4.Tables(0).Rows(0).Item("JanVolume").ToString()
                txtFeb.Text = ds4.Tables(0).Rows(0).Item("FebVolume").ToString()
                txtMar.Text = ds4.Tables(0).Rows(0).Item("MarVolume").ToString()
                txtApr.Text = ds4.Tables(0).Rows(0).Item("AprVolume").ToString()
                txtMay.Text = ds4.Tables(0).Rows(0).Item("MayVolume").ToString()
                txtJun.Text = ds4.Tables(0).Rows(0).Item("JunVolume").ToString()
                txtJul.Text = ds4.Tables(0).Rows(0).Item("JulVolume").ToString()
                txtAug.Text = ds4.Tables(0).Rows(0).Item("AugVolume").ToString()
                txtSep.Text = ds4.Tables(0).Rows(0).Item("SepVolume").ToString()
                txtOct.Text = ds4.Tables(0).Rows(0).Item("OctVolume").ToString()
                txtNov.Text = ds4.Tables(0).Rows(0).Item("NovVolume").ToString()
                txtDec.Text = ds4.Tables(0).Rows(0).Item("DecVolume").ToString()
            End If

            'bind data from Program_Main
            Dim ds3 As DataSet = New DataSet
            ds3 = commonFunctions.GetPlatformProgram(PlatformID, ddProgram.SelectedValue, "", "", "")
            If (ds3.Tables.Item(0).Rows.Count > 0) Then
                Dim NoOfDays As String = Nothing
                Select Case ds3.Tables(0).Rows(0).Item("EOPMM").ToString()
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


                SOPYY = ds3.Tables(0).Rows(0).Item("SOPYY").ToString()
                SOPMM = ds3.Tables(0).Rows(0).Item("SOPMM").ToString()
                EOPYY = ds3.Tables(0).Rows(0).Item("EOPYY").ToString()
                EOPMM = ds3.Tables(0).Rows(0).Item("EOPMM").ToString()

                txtSOP.Text = SOPMM & "/01/" & SOPYY
                txtEOP.Text = EOPMM & "/" & NoOfDays & "/" & EOPYY
                txtSOP.Enabled = False
                txtEOP.Enabled = False
                imgSOP.Enabled = False
                imgEOP.Enabled = False

                lblPrevSOP.Text = SOPMM & "/01/" & SOPYY
                lblPrevEop.Text = EOPMM & "/" & NoOfDays & "/" & EOPYY

                If SOPYY > ddYear.SelectedValue Then
                    lblMessage0.Text = "The selected Program has an SOP Year Greater Than the Planning Year. Please make another selection"
                    lblMessage0.Visible = True
                    btnSave.Enabled = False
                End If

                If EOPYY < ddYear.SelectedValue Then
                    lblMessage1.Text = "The selected Program has an EOP Year Less Than the Planning Year. Please make another selection."
                    lblMessage1.Visible = True
                    btnSave.Enabled = False
                End If

                DisableMonthlyTextBoxes()

            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during delete record.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF NewRecordInsertDataPullbyPlanningYearSelection

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim PGMID As Integer = HttpContext.Current.Request.QueryString("sPGMID")
        Dim PlatformID As Integer = HttpContext.Current.Request.QueryString("sPlatID")
        Dim CABBV As String = HttpContext.Current.Request.QueryString("sCABBV")
        Dim SoldTo As Integer = HttpContext.Current.Request.QueryString("sSoldTo")
        Dim PYear As Integer = HttpContext.Current.Request.QueryString("sYear")

        Response.Redirect("Copy_Vehicle.aspx?sPGMID=" & PGMID & "&sPlatID=" & PlatformID & "&sYear=" & PYear & "&sCABBV=" & CABBV & _
            "&sSoldTo=" & SoldTo & "&DisplayName=" & ddProgram.SelectedItem.Text, False)
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInsert.Click
        Try
            lblMessage.Text = ""
            lblMessage.Visible = False
            txtJan.Text = lblJan.Text
            txtFeb.Text = lblFeb.Text
            txtMar.Text = lblMar.Text
            txtApr.Text = lblApr.Text
            txtMay.Text = lblMay.Text
            txtJun.Text = lblJun.Text
            txtJul.Text = lblJul.Text
            txtAug.Text = lblAug.Text
            txtSep.Text = lblSep.Text
            txtOct.Text = lblOct.Text
            txtNov.Text = lblNov.Text
            txtDec.Text = lblDec.Text
            txtVolume.Text = lblAnnualVolume.Text

            Dim NoOfDays As String = Nothing
            Select Case lblEOPMM.Text
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

            txtSOP.Text = lblSOPMM.Text & "/01/" & lblSOPYY.Text
            txtEOP.Text = lblEOPMM.Text & "/" & NoOfDays & "/" & lblEOPYY.Text
            lblIHSDataUsed.Text = True
            lblReqNotes.Visible = True
            lblReqNotesText.Visible = True
            lblMessage.Text = "IHS data downloaded for this Vehicle Program. Please SAVE before proceeding, otherwise RESET to retrieve previous data."
            lblMessage.Visible = True

            DisableMonthlyTextBoxes()

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during IHS download.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF btnInsert_Click

    Protected Sub txtSOP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSOP.TextChanged
        Try

            If ViewState("sPGMID") <> 0 Then
                lblMessage0.Text = ""
                lblMessage0.Visible = False
                btnSave.Enabled = True

                Dim SOPYY As Integer = 0
                Select Case Len(txtSOP.Text)
                    Case 8
                        SOPYY = txtSOP.Text.Substring(4, 4)
                    Case 10
                        SOPYY = txtSOP.Text.Substring(6, 4)
                End Select


                If SOPYY < lblSOPYY.Text Then 'lblBegYear.Text
                    'lblMessage0.Text = "Start of Production Year must be Greater Than or Equal to Platform's Beginning Year."
                    lblMessage0.Text = "Start of Production Year must be Greater Than or Equal to IHS SOP Year."
                    lblMessage0.Visible = True
                    btnSave.Enabled = False
                Else
                    If txtSOP.Text <> lblPrevSOP.Text Then
                        lblReqNotes.Visible = True
                        lblReqNotesText.Visible = True
                    End If
                    DisableMonthlyTextBoxes()
                End If
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during IHS download.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF txtSOP_TextChanged

    Protected Sub txtEOP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEOP.TextChanged
        Try

            If ViewState("sPGMID") <> 0 Then

                lblMessage1.Text = ""
                lblMessage1.Visible = False
                btnSave.Enabled = True

                Dim EOPYY As Integer = 0
                Select Case Len(txtEOP.Text)
                    Case 8
                        EOPYY = txtEOP.Text.Substring(4, 4)
                    Case 10
                        EOPYY = txtEOP.Text.Substring(6, 4)
                End Select

                If EOPYY > lblEOPYY.Text Then 'lblEndYear.Text Then
                    'lblMessage1.Text = "End of Production Year must be Less Than or Equal to Platform's End Year."
                    lblMessage1.Text = "End of Production Year must be Less Than or Equal to IHS EOP Year."
                    lblMessage1.Visible = True
                    btnSave.Enabled = False
                Else
                    If txtEOP.Text <> lblPrevEop.Text Then
                        lblReqNotes.Visible = True
                        lblReqNotesText.Visible = True
                    End If
                    DisableMonthlyTextBoxes()
                End If
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during IHS download.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF txtEOP_TextChanged

    Protected Sub ddProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProgram.SelectedIndexChanged
        lblMessage0.Text = Nothing
        lblMessage0.Visible = False
        btnSave.Enabled = True
        ddYear.Enabled = True

        If ddProgram.SelectedItem.Text.Substring(0, 2) = "**" Then
            ddYear.Enabled = False
            lblMessage0.Text = "This Program is not active. Please make another selection."
            lblMessage0.Visible = True
            btnSave.Enabled = False
        Else
            If ddYear.SelectedValue <> Nothing Then
                NewRecordInsertDataPullbyPlanningYearSelection()
            End If
        End If
    End Sub
End Class
