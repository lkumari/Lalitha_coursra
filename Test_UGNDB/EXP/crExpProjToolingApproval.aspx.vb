' ************************************************************************************************
' Name:	crExpProjToolingApproval.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from ExpProj_Tooling table
'           and other sources.  Also used as an approval screen to allow users to view current
'           info related to a capex tooling project and approve/reject the project in one screen.
' Date		    Author	    
' 12/30/2009    LRey		Created .Net application
' 01/07/2013    LRey        Added a control to hide the Edit button in the approval process to prevent out of sequence approval.
' 06/26/2013    LRey        Modified the Reject process to notify the correct group based on the facility.
' ************************************************************************************************

#Region "Directives"
Imports System.Net.Mail
Imports System.Threading
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
#End Region

Partial Class EXP_crViewExpProjTooling
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")

        ''Used to Show/Hide Tabs when Last Primary/Supplement entry is made 
        ''from TE Tracking system
        If HttpContext.Current.Request.QueryString("pLS") <> "" Then
            ViewState("pLS") = CType(HttpContext.Current.Request.QueryString("pLS"), Boolean)
        Else
            ViewState("pLS") = 0
        End If

        Dim m As ASP.crviewmasterpage_master = Master
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            If ViewState("pProjNo") = Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='ToolingExpProjList.aspx'><b>Customer Owned Tooling Search</b></a> > Customer Owned Tooling Project Preview"
            Else
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='ToolingExpProjList.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'><b>Customer Owned Tooling Search</b></a> > <a href='ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pAprv=1" & "'><b>Customer Owned Tooling Project</b></a> > Approval"
            End If
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
            Master.Page.Header.Title = "UGN, Inc.: " & ViewState("pProjNo") & " - Approval"
        End If

        ''************************************************************
        ''Code Below counts the number of chars used in comments area
        ''************************************************************
        txtComments.Attributes.Add("onkeypress", "return tbLimit();")
        txtComments.Attributes.Add("onkeyup", "return tbCount(" + lblComments.ClientID + ");")
        txtComments.Attributes.Add("maxLength", "200")

        txtQC.Attributes.Add("onkeypress", "return tbLimit();")
        txtQC.Attributes.Add("onkeyup", "return tbCount(" + lblQC.ClientID + ");")
        txtQC.Attributes.Add("maxLength", "200")

        ''*************************************************
        '' "Form Level Security using Roles &/or Subscriptions"
        ''*************************************************
        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

        ''*********
        ''Get Data
        ''*********
        If Not Page.IsPostBack Then
            If ViewState("pProjNo") <> "" Then
                BindData()
            End If
        End If

        ''*************************************************
        '' "Form Level Security using Roles &/or Subscriptions"
        ''*************************************************
        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

    End Sub 'EOF Page_Load

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
            btnSubmit.Enabled = False
            btnReset.Enabled = False
            lblErrors.Text = ""
            lblErrors.Visible = False

            ''*********************************************************
            ''If Record is Void, do not allow Team Memember submission
            ''*********************************************************
            Dim dsExp As DataSet = New DataSet
            Dim ProjectStatus As String = Nothing
            ViewState("pProjStat") = Nothing
            dsExp = EXPModule.GetExpProjTooling(ViewState("pProjNo"), "", "", "", "", 0, 0, 0, 0, 0, "", "", "", "")
            If commonFunctions.CheckDataSet(dsExp) = True Then '(dsExp.Tables.Item(0).Rows.Count > 0) Then
                ProjectStatus = dsExp.Tables(0).Rows(0).Item("ProjectStatus").ToString()
                ViewState("pProjStat") = ProjectStatus
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

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Eduardo.Anaya", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    ViewState("UGNDB_TMID") = iTeamMemberID
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)

                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                                Select Case iRoleID
                                    Case 11 '*** UGNAdmin: Full Access
                                        ViewState("ObjectRole") = True
                                        ViewState("Admin") = True
                                        If lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Pending" And (ProjectStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                        ElseIf lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing And (ProjectStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                        End If
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        ViewState("ObjectRole") = True
                                        If lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Pending" And (ProjectStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                        End If
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        ViewState("ObjectRole") = True
                                        If lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Pending" And (ProjectStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                            btnSubmit.Visible = True
                                            btnReset.Visible = True
                                        ElseIf lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing And (ProjectStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                            btnSubmit.Visible = True
                                            btnReset.Visible = True
                                        Else
                                            If lblDateNotified.Text = Nothing Or lblDateNotified.Text = "" Then
                                                btnSubmit.Visible = False
                                                btnReset.Visible = False
                                                lblErrors.Text = "Project not ready for your review. Pending previous level(s) to approve."
                                                lblErrors.Visible = True
                                                lblErrors.Font.Size = 12
                                            End If
                                        End If
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        ViewState("ObjectRole") = True
                                        If lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Pending" And (ProjectStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                        End If
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        ''** No Entry allowed **''
                                End Select 'EOF of "Select Case iRoleID"
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
        Dim oRpt As New ReportDocument()
        Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
        Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
        Dim dbConn As New TableLogOnInfo()

        If ViewState("pProjNo") <> "" Then
            Try

                If Session("TempCrystalRptFiles") Is Nothing Then
                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "crExpProjTooling.rpt")

                    'getting the database, the table and the LogOnInfo object which holds login onformation
                    crDatabase = oRpt.Database

                    'getting the table in an object array of one item 
                    Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
                    crDatabase.Tables.CopyTo(arrTables, 0)
                    ' assigning the first item of array to crTable by downcasting the object to Table 
                    crTable = arrTables(0)

                    ' setting values 
                    dbConn = crTable.LogOnInfo
                    dbConn.ConnectionInfo.DatabaseName = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()
                    dbConn.ConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString()
                    dbConn.ConnectionInfo.UserID = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    dbConn.ConnectionInfo.Password = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                    ' applying login info to the table object 
                    crTable.ApplyLogOnInfo(dbConn)

                    ' defining report source 
                    CrystalReportViewer1.DisplayGroupTree = False
                    CrystalReportViewer1.ReportSource = oRpt
                    Session("TempCrystalRptFiles") = oRpt

                    'Check if there are parameters or not in report.
                    Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count
                    setReportParameters()
                    Session("TempCrystalRptFiles") = oRpt
                Else
                    oRpt = CType(Session("TempCrystalRptFiles"), ReportDocument)
                    CrystalReportViewer1.ReportSource = oRpt
                End If
            Catch ex As Exception
                lblErrors.Text = "Error found in report view" & ex.Message
                lblErrors.Visible = "True"
            End Try
        End If
    End Sub 'EOF Page_Init

    Private Sub setReportParameters()

        Try
            ' all the parameter fields will be added to this collection 
            Dim paramFields As New ParameterFields
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            ' the parameter fields to be sent to the report 
            Dim pfProjectNo As ParameterField = New ParameterField
            Dim pfSubProjectNo As ParameterField = New ParameterField
            Dim pfProjectTitle As ParameterField = New ParameterField
            Dim pfUGNFacility As ParameterField = New ParameterField
            Dim pfCustomer As ParameterField = New ParameterField
            Dim pfProgramID As ParameterField = New ParameterField
            Dim pfAcctMgrTMID As ParameterField = New ParameterField
            Dim pfPrgmMgrTMID As ParameterField = New ParameterField
            Dim pfToolLeadTMID As ParameterField = New ParameterField
            Dim pfPurchLeadTMID As ParameterField = New ParameterField
            Dim pfProjectType As ParameterField = New ParameterField
            Dim pfURLLocation As ParameterField = New ParameterField
            Dim pfPartNo As ParameterField = New ParameterField
            Dim pfPartDesc As ParameterField = New ParameterField
            Dim pfProjectStatus As ParameterField = New ParameterField

            ' setting the name of parameter fields with which they will be RECEIVED in report 
            pfProjectNo.ParameterFieldName = "@ProjectNo"
            pfSubProjectNo.ParameterFieldName = "@SupProjectNo"
            pfProjectTitle.ParameterFieldName = "@ProjectTitle"
            pfUGNFacility.ParameterFieldName = "@UGNFacility"
            pfCustomer.ParameterFieldName = "@Customer"
            pfProgramID.ParameterFieldName = "@ProgramID"
            pfAcctMgrTMID.ParameterFieldName = "@AcctMgrTMID"
            pfPrgmMgrTMID.ParameterFieldName = "@PrgmMgrTMID"
            pfToolLeadTMID.ParameterFieldName = "@ToolLeadTMID"
            pfPurchLeadTMID.ParameterFieldName = "@PurchLeadTMID"
            pfProjectType.ParameterFieldName = "@ProjectType"
            pfURLLocation.ParameterFieldName = "@URLLocation"
            pfPartNo.ParameterFieldName = "@PartNo"
            pfPartDesc.ParameterFieldName = "@PartDesc"
            pfProjectStatus.ParameterFieldName = "@ProjectStatus"

            ' the above declared parameter fields accept values as discrete objects 
            ' so declaring discrete objects 
            Dim dcProjectNo As New ParameterDiscreteValue
            Dim dcSubProjectNo As New ParameterDiscreteValue
            Dim dcProjectTitle As New ParameterDiscreteValue
            Dim dcUGNFacility As New ParameterDiscreteValue
            Dim dcCustomer As New ParameterDiscreteValue
            Dim dcProgramID As New ParameterDiscreteValue
            Dim dcAcctMgrTMID As New ParameterDiscreteValue
            Dim dcPrgmMgrTMID As New ParameterDiscreteValue
            Dim dcToolLeadTMID As New ParameterDiscreteValue
            Dim dcPurchLeadTMID As New ParameterDiscreteValue
            Dim dcProjectType As New ParameterDiscreteValue
            Dim dcURLLocation As New ParameterDiscreteValue
            Dim dcPartNo As New ParameterDiscreteValue
            Dim dcPartDesc As New ParameterDiscreteValue
            Dim dcProjectStatus As New ParameterDiscreteValue

            ' setting the values of discrete objects 
            dcProjectNo.Value = ViewState("pProjNo")
            dcSubProjectNo.Value = ""
            dcProjectTitle.Value = ""
            dcUGNFacility.Value = ""
            dcCustomer.Value = ""
            dcProgramID.Value = 0
            dcAcctMgrTMID.Value = 0
            dcPrgmMgrTMID.Value = 0
            dcToolLeadTMID.Value = 0
            dcPurchLeadTMID.Value = 0
            dcProjectType.Value = ""
            dcURLLocation.Value = strProdOrTestEnvironment
            dcPartNo.Value = ""
            dcPartDesc.Value = ""
            dcProjectStatus.Value = ""

            ' now adding these discrete values to parameters 
            pfProjectNo.CurrentValues.Add(dcProjectNo)
            pfSubProjectNo.CurrentValues.Add(dcSubProjectNo)
            pfProjectTitle.CurrentValues.Add(dcProjectTitle)
            pfUGNFacility.CurrentValues.Add(dcUGNFacility)
            pfCustomer.CurrentValues.Add(dcCustomer)
            pfProgramID.CurrentValues.Add(dcProgramID)
            pfAcctMgrTMID.CurrentValues.Add(dcAcctMgrTMID)
            pfPrgmMgrTMID.CurrentValues.Add(dcPrgmMgrTMID)
            pfToolLeadTMID.CurrentValues.Add(dcToolLeadTMID)
            pfPurchLeadTMID.CurrentValues.Add(dcPurchLeadTMID)
            pfProjectType.CurrentValues.Add(dcProjectType)
            pfURLLocation.CurrentValues.Add(dcURLLocation)
            pfPartNo.CurrentValues.Add(dcPartNo)
            pfPartDesc.CurrentValues.Add(dcPartDesc)
            pfProjectStatus.CurrentValues.Add(dcProjectStatus)

            ' now adding all these parameter fields to the parameter collection 
            paramFields.Add(pfProjectNo)
            paramFields.Add(pfSubProjectNo)
            paramFields.Add(pfProjectTitle)
            paramFields.Add(pfUGNFacility)
            paramFields.Add(pfCustomer)
            paramFields.Add(pfProgramID)
            paramFields.Add(pfAcctMgrTMID)
            paramFields.Add(pfPrgmMgrTMID)
            paramFields.Add(pfToolLeadTMID)
            paramFields.Add(pfPurchLeadTMID)
            paramFields.Add(pfProjectType)
            paramFields.Add(pfURLLocation)
            paramFields.Add(pfPartNo)
            paramFields.Add(pfPartDesc)
            paramFields.Add(pfProjectStatus)

            ' finally add the parameter collection to the crystal report viewer 
            CrystalReportViewer1.ParameterFieldInfo = paramFields

        Catch ex As Exception
            lblErrors.Text = "Error found in parameter search " & ex.Message
            lblErrors.Visible = True
        End Try
    End Sub 'setReportParameters

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'in order to clear crystal reports
        If HttpContext.Current.Session("TempCrystalRptFiles") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("TempCrystalRptFiles"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("TempCrystalRptFiles") = Nothing
            GC.Collect()
        End If
    End Sub 'EOF Page_Unload 188 371 510 569

    Public Sub BindData()
        Try
            Dim ds As DataSet = New DataSet
            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                commonFunctions.SetUGNDBUser()
            End If

            Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")

            ds = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), 0, DefaultTMID, False, False)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                lblTeamMbr.Text = ds.Tables(0).Rows(0).Item("TeamMemberName").ToString()
                lblDateNotified.Text = ds.Tables(0).Rows(0).Item("DateNotified").ToString()
                ddStatus.SelectedValue = ds.Tables(0).Rows(0).Item("Status").ToString()
                txtComments.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                hfSeqNo.Value = ds.Tables(0).Rows(0).Item("SeqNo").ToString()

                If (ds.Tables(0).Rows(0).Item("DateSigned").ToString() <> Nothing) Then
                    btnSubmit.Enabled = False
                    btnReset.Enabled = False
                Else
                    If ViewState("pProjStat") <> "Void" Then
                        btnSubmit.Enabled = True
                        btnReset.Enabled = True
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
    End Sub 'EOF BindData()

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Response.Redirect("crExpProjToolingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS"), False)
    End Sub 'EOF btnReset_Click

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        ''********
        ''* This function is used to submit email next level Approvers or to originators when rejected.
        ''********188 371 510 569
        Dim DefaultDate As Date = Date.Now
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")

        Dim ds1st As DataSet = New DataSet
        Dim ds2nd As DataSet = New DataSet
        Dim dsCC As DataSet = New DataSet
        Dim dsRej As DataSet = New DataSet
        Dim dsCommodity As DataSet = New DataSet
        Dim EmailTO As String = Nothing
        Dim EmpName As String = Nothing
        Dim EmailCC As String = Nothing
        Dim EmailFrom As String = Nothing
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

        lblErrors.Text = Nothing
        lblErrors.Visible = False
        ReqComments.Visible = False

        Try
            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then
                If ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
                    lblErrors.Text = "Your comments is required for Rejection."
                    lblErrors.Visible = True
                    ReqComments.Visible = True
                    Exit Sub
                    ''CheckRights()
                Else
                    ''*************************************************************************
                    ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                    ''*************************************************************************
                    Dim dsExp As DataSet = New DataSet
                    dsExp = EXPModule.GetExpProjTooling(ViewState("pProjNo"), "", "", "", "", 0, 0, 0, 0, 0, "", "", "", "")
                    If (dsExp.Tables.Item(0).Rows.Count > 0) Then
                        ''**********************
                        ''*Initialize Variables
                        ''**********************
                        Dim ToolingLead As Integer = dsExp.Tables(0).Rows(0).Item("ToolLeadTMID")
                        Dim PurchasingLead As Integer = dsExp.Tables(0).Rows(0).Item("PurchLeadTMID")
                        Dim ProjectTitle As String = dsExp.Tables(0).Rows(0).Item("ProjectTitle")
                        Dim ProjectType As String = dsExp.Tables(0).Rows(0).Item("ProjectType")
                        Dim ProjDateNotes As String = dsExp.Tables(0).Rows(0).Item("ProjDtNotes")
                        Dim UGNFacilityName As String = dsExp.Tables(0).Rows(0).Item("UGNFacilityName")
                        Dim UGNFacility As String = dsExp.Tables(0).Rows(0).Item("UGNFacility")
                        Dim EstCmpltDt As String = dsExp.Tables(0).Rows(0).Item("EstCmpltDt")
                        Dim ExpToolRtnDt As String = dsExp.Tables(0).Rows(0).Item("ExpectedToolRtnDt").ToString()
                        Dim EstSpendDt As String = dsExp.Tables(0).Rows(0).Item("EstSpendDt").ToString()
                        Dim EstRecoveryDt As String = dsExp.Tables(0).Rows(0).Item("EstRecoveryDt").ToString()
                        Dim AmtToBeRecovered As Decimal = Format(dsExp.Tables(0).Rows(0).Item("AmtToBeRecovered"), "#,##0.00")
                        Dim TotalInv As Decimal = Format(dsExp.Tables(0).Rows(0).Item("TotalInv"), "#,##0.00")
                        Dim MPAAmtToBeRecovered As Decimal = Format(dsExp.Tables(0).Rows(0).Item("MPA_AmtToBeRecovered"), "#,##0.00")
                        Dim MPATotalInv As Decimal = Format(dsExp.Tables(0).Rows(0).Item("MPA_TotalInv"), "#,##0.00")
                        Dim LumpSum As Boolean = dsExp.Tables(0).Rows(0).Item("LumpSum")
                        Dim FirstRecoveryAmount As Decimal = 0
                        Dim FirstRecoveryDate As String = Nothing
                        If LumpSum = True Then
                            FirstRecoveryAmount = Format(dsExp.Tables(0).Rows(0).Item("FirstRecoveryAmount"), "#,##0.00")
                            FirstRecoveryDate = dsExp.Tables(0).Rows(0).Item("FirstRecoveryDate")
                        End If
                        Dim SeqNo As Integer = 0
                        Dim NextSeqNo As Integer = 0
                        Dim NextLvl As Integer = 0
                        Select Case hfSeqNo.Value
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

                        If ddStatus.SelectedValue <> "Pending" Then
                            ''***********************************
                            ''Update Current Level Approver record.
                            ''***********************************
                            EXPModule.UpdateExpProjToolingApproval(ViewState("pProjNo"), DefaultTMID, True, ddStatus.SelectedValue, txtComments.Text, SeqNo, DefaultUser, DefaultDate)

                            ''*****************
                            ''Locate Approvers
                            ''*****************
                            If ddStatus.SelectedValue <> "Rejected" Then
                                ds1st = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), SeqNo, 0, True, False)
                                If commonFunctions.CheckDataSet(ds1st) = True Then
                                    For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                                        If (ds1st.Tables(0).Rows(i).Item("SubmitFlag") = True) Then
                                            LvlApvlCmplt = True
                                        End If
                                    Next
                                Else
                                    LvlApvlCmplt = True
                                End If

                                ''***************************************************************
                                ''Locate Next Level Approver(s)
                                ''***************************************************************
                                If LvlApvlCmplt = True Then
                                    If SeqNo <> 3 Then
                                        ''***************
                                        ''* Delete/Rebuild Next Level Approval for rebuild
                                        ''***************
                                        EXPModule.DeleteExpProjToolingApproval(ViewState("pProjNo"), NextSeqNo)
                                        EXPModule.InsertExpProjToolingApproval(ViewState("pProjNo"), UGNFacility, NextLvl, DefaultUser, DefaultDate)
                                    Else 'If last approval
                                        ''************************
                                        ''* Update Tooling record
                                        '*************************
                                        EXPModule.UpdateExpProjToolingStatus(ViewState("pProjNo"), "Approved", "A", DefaultUser, DefaultDate)
                                    End If
                                    If SeqNo <> 3 Then
                                        ds2nd = EXPModule.GetToolingExpProjApproval(ViewState("pProjNo"), NextSeqNo, 0, False, False)
                                        ''Check that the recipient(s) is a valid Team Member
                                        If commonFunctions.CheckDataSet(ds2nd) = True Then
                                            ''***************
                                            ''* Loop through the next level approvals
                                            ''***************
                                            For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                                If ((ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                                    (ds2nd.Tables(0).Rows(i).Item("TeamMemberID") <> DefaultTMID)) And _
                                                    (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                                    EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                                    EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                                    ''****************************************************
                                                    ''Update Next level DateNotified field.
                                                    ''****************************************************
                                                    If SeqNo <> 3 Then
                                                        EXPModule.UpdateExpProjToolingApproval(ViewState("pProjNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", NextSeqNo, DefaultUser, DefaultDate)
                                                    End If
                                                End If
                                            Next
                                        End If
                                    Else
                                        ''********************************************************
                                        ''Notify Account/Program Manager & Tooling/Purchasing Lead
                                        ''********************************************************
                                        dsRej = EXPModule.GetExpProjToolingLead(ViewState("pProjNo"))
                                        ''Check that the recipient(s) is a valid Team Member
                                        If commonFunctions.CheckDataSet(dsRej) = True Then
                                            For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                                If (((ProjectType = "Internal" And dsRej.Tables(0).Rows(i).Item("TMDesc") = "Tooling Lead") Or _
                                                     (ProjectType = "External" And dsRej.Tables(0).Rows(i).Item("TMDesc") = "Purchasing Lead")) Or _
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
                                    End If 'EOF If <> SeqNo 3
                                End If
                            Else 'IF REJECTED
                                ''********************************************************
                                ''Notify Account/Program Manager & Tooling/Purchasing Lead
                                ''********************************************************
                                dsRej = EXPModule.GetExpProjToolingLead(ViewState("pProjNo"))
                                ''Check that the recipient(s) is a valid Team Member
                                If commonFunctions.CheckDataSet(dsRej) = True Then
                                    For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                        If (((ProjectType = "Internal" And _
                                              (dsRej.Tables(0).Rows(i).Item("TMDesc") = "Tooling Lead" Or _
                                               dsRej.Tables(0).Rows(i).Item("TMDesc") = "Tooling Engr Mgr")) Or _
                                             (ProjectType = "External" And _
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

                                ''************************
                                ''* Update Tooling record
                                '*************************
                                EXPModule.UpdateExpProjToolingStatus(ViewState("pProjNo"), "In Process", "R", DefaultUser, DefaultDate)
                            End If 'EOF  If ddStatus.SelectedValue <> "Rejected" Then

                            ''********************************************************
                            ''Send Notification only if there is a valid Email Address
                            ''********************************************************
                            If EmailTO <> Nothing Then
                                Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                                Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                                Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                                If ddStatus.SelectedValue <> "Rejected" Then
                                    If SeqNo <> 3 Then
                                        ''**************************************************************
                                        ''Carbon Copy Account/Program Manager & Tooling/Purchasing Lead
                                        ''**************************************************************
                                        EmailCC = CarbonCopyList(Nothing, 0, "", 0, 0, EmailCC, ProjectType, DefaultTMID)
                                    End If
                                    If (SeqNo = 3 And ddStatus.SelectedValue <> "Rejected") Then
                                        ''**************************************
                                        ''*Carbon Copy the Accounting Department
                                        ''**************************************
                                        EmailCC = CarbonCopyList(MyMessage, 10, "", 0, 0, EmailCC, "", DefaultTMID)

                                        ''*********************************************************
                                        ''*Carbon Copy the Operations Manager based on UGNFacility
                                        ''*********************************************************
                                        EmailCC = CarbonCopyList(MyMessage, 78, UGNFacility, 0, 0, EmailCC, "", DefaultTMID)
                                    End If
                                End If
                                ''**************************************************************
                                ''Carbon Copy Previous Levels
                                ''**************************************************************
                                If SeqNo > 1 Then
                                    EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), UGNFacility, 0, 0, EmailCC, "", DefaultTMID)
                                    If SeqNo = 3 Then
                                        EmailCC = CarbonCopyList(MyMessage, (NextLvl - 2), UGNFacility, 0, 0, EmailCC, "", DefaultTMID)
                                        ''**************************************************************
                                        ''Carbon Copy Last Level Approvers
                                        ''**************************************************************
                                        EmailCC = CarbonCopyList(MyMessage, NextLvl, UGNFacility, 0, 0, EmailCC, "", DefaultTMID)
                                    End If
                                Else
                                    'EmailCC = CarbonCopyList(MyMessage, IIf(UGNFacility <> "UW", (NextLvl - 1), 145), "", 0, 0, EmailCC, "", DefaultTMID)
                                    EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), UGNFacility, 0, 0, EmailCC, "", DefaultTMID)
                                End If

                                ''*****************************************************
                                ''Carbon Copy Default Corporate Engineer 
                                ''*****************************************************
                                EmailCC = CarbonCopyList(MyMessage, 52, "", 0, 0, EmailCC, "", DefaultTMID)

                                ''*****************************************************
                                ''Carbon Copy Default Program Mgmt 
                                ''*****************************************************
                                EmailCC = CarbonCopyList(MyMessage, 127, "", 0, 0, EmailCC, "", DefaultTMID)

                                ''*****************************************************
                                ''Carbon Copy Tooling Engr Mgr
                                ''*****************************************************
                                EmailCC = CarbonCopyList(MyMessage, 145, "", 0, 0, EmailCC, "", DefaultTMID)

                                'Test or Production Message display
                                If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                                    MyMessage.Subject = "TEST: "
                                    MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                                Else
                                    MyMessage.Subject = ""
                                    MyMessage.Body = ""
                                End If

                                MyMessage.Subject &= ProjectType & " - Customer Tooling Expense: " & ViewState("pProjNo") & " - " & ProjectTitle
                                MyMessage.Body = "<font size='2' face='Tahoma'>"
                                If ddStatus.SelectedValue = "Rejected" Then
                                    MyMessage.Subject &= " - REJECTED"
                                    MyMessage.Body &= EmpName
                                    MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & ProjectTitle & "' was <font color='red'>REJECTED</font>.  "
                                    MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.<br/><br/>"
                                    MyMessage.Body &= "<i>Reason for rejection:</i> <b>" & txtComments.Text & "</b></p>"
                                Else
                                    If SeqNo = 3 Then
                                        MyMessage.Subject &= "- APPROVED"

                                        MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & ProjectTitle & "' is Approved. "
                                        MyMessage.Body &= " <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "'>Click here</a> to access the record.</p>"
                                    Else
                                        MyMessage.Body &= EmpName
                                        MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & ProjectTitle & "' is available for your Review/Approval. "
                                        MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crExpProjToolingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"
                                    End If
                                End If
                                MyMessage.Body &= "</font>"

                                MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 12; font-family: Tahoma;'>"

                                MyMessage.Body &= "<tr bgcolor='#EBEBEB'>"
                                MyMessage.Body &= "<td colspan='5'><strong>Projected Date Notes</strong></td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr style='border-color:white;'>"
                                MyMessage.Body &= "<td colspan='5'>" & ProjDateNotes & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td colspan='5'>&nbsp;</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:white;'>"
                                MyMessage.Body &= "     <td><strong>UGN Location </strong></td>"
                                MyMessage.Body &= "     <td><strong>Est. Completion Date</strong></td>"
                                MyMessage.Body &= "     <td><strong>Est. Start Spend Date</strong></td>"
                                MyMessage.Body &= "     <td><strong>Est. Tool Return Date</strong></td>"
                                MyMessage.Body &= "     <td><strong>Est. Customer Recovery Date</strong></td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "     <td height='25'>" & UGNFacilityName & "</td>"
                                MyMessage.Body &= "     <td>" & EstCmpltDt & "</td>"
                                MyMessage.Body &= "     <td>" & EstSpendDt & "</td>"
                                MyMessage.Body &= "     <td>" & ExpToolRtnDt & "</td>"
                                MyMessage.Body &= "     <td>" & EstRecoveryDt & "</td>"
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
                                MyMessage.Body &= "             <td>" & AmtToBeRecovered & "</td>"
                                MyMessage.Body &= "             <td class='p_text' align='right' width='150px'>Amount to be Recovered ($):&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "             <td>" & MPAAmtToBeRecovered & "</td>"
                                MyMessage.Body &= "         </tr>"
                                MyMessage.Body &= "         <tr>"
                                MyMessage.Body &= "             <td class='p_text' align='right'>Total Investment ($):&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "             <td>" & TotalInv & "</td>"
                                MyMessage.Body &= "             <td class='p_text' align='right'>Total Investment ($):&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "             <td>" & MPATotalInv & "</td>"
                                MyMessage.Body &= "         </tr>"

                                MyMessage.Body &= "     </table>"
                                MyMessage.Body &= "     </td>"
                                MyMessage.Body &= " </tr>"

                                If LumpSum = True Then
                                    MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:white;'>"
                                    MyMessage.Body &= "<td><strong>Recovery Type </strong></td>"
                                    MyMessage.Body &= "<td><strong>1st Recovery Amount </strong></td>"
                                    MyMessage.Body &= "<td><strong>1st Recovery Date </strong></td>"
                                    MyMessage.Body &= "</tr>"

                                    MyMessage.Body &= "<tr>"
                                    MyMessage.Body &= "<td height='25'>Lump Sum </td>"
                                    MyMessage.Body &= "<td height='25'>$" & FirstRecoveryAmount & "</td>"
                                    MyMessage.Body &= "<td height='25'>" & FirstRecoveryDate & "</td>"
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
                                MyMessage.Body &= "<table width='100%' style='font-size: 11; font-family: Tahoma;'>"
                                MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
                                MyMessage.Body &= "<td><strong>Customer</strong></td>"
                                MyMessage.Body &= "<td><strong>Program</strong></td>"
                                MyMessage.Body &= "<td><strong>Part Number</strong></td>"
                                MyMessage.Body &= "<td><strong>Lead Time</strong></td>"
                                MyMessage.Body &= "<td><strong>SOP Date </strong></td>"
                                MyMessage.Body &= "<td><strong>EOP Date </strong></td>"
                                MyMessage.Body &= "<td><strong>Est. PPAP Date </strong></td>"
                                MyMessage.Body &= "</tr>"

                                Dim dsCP As DataSet
                                dsCP = EXPModule.GetExpProjToolingCustomer(ViewState("pProjNo"), 0)
                                If commonFunctions.CheckDataSet(dsCP) = True Then
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
                                If commonFunctions.CheckDataSet(dsTED) = True Then
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
                                EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, ddStatus.SelectedValue & " " & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing))

                                ''*****************
                                ''History Tracking
                                ''*****************
                                If ddStatus.SelectedValue <> "Rejected" Then
                                    If SeqNo = 3 Then
                                        EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Notification sent to all involved. ")
                                    Else
                                        EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Notification sent to level " & IIf(SeqNo < 3, (SeqNo + 1), SeqNo) & " approver(s): " & EmpName)
                                    End If
                                Else
                                    EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Notification sent to " & EmpName)
                                End If

                                ''**********************************
                                ''Connect & Send email notification
                                ''**********************************
                                Try
                                    commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (T)", ViewState("pProjNo"))
                                    lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                Catch ex As SmtpException
                                    lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                    UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, CurrentEmpEmail & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                End Try
                                lblErrors.Visible = True

                                ''**********************************
                                ''Rebind the data to the form
                                ''********************************** 
                                BindData()
                            Else
                                ''*****************
                                ''History Tracking
                                ''*****************
                                EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, ddStatus.SelectedValue & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing))

                                ''**********************************
                                ''Rebind the data to the form
                                ''********************************** 
                                BindData()
                                lblErrors.Text = "Your response was submitted successfully."
                                lblErrors.Visible = True
                            End If
                        Else
                            lblErrors.Text = "Action Cancelled. Please select a status Approved or Rejected."
                            lblErrors.Visible = True
                        End If 'EOF If ddStatus.SelectedValue <> "Pending" Then
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

    End Sub 'EOF btnSubmit_Click

    Protected Sub btnSubmit2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit2.Click
        Try
            ''********
            ''* This function is used to submit email next level Approvers or to originators when rejected.
            ''********188 371 510 569
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")
            Dim DefaultUserFullName As String = Nothing

            Dim ds1st As DataSet = New DataSet
            Dim dsCC As DataSet = New DataSet
            Dim dsRej As DataSet = New DataSet
            Dim EmailFrom As String = Nothing
            Dim EmailTO As String = Nothing
            Dim EmpName As String = Nothing
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
            ReqComments.Visible = False

            '*************************
            '* Get Current User Name
            '*************************
            Dim gtm As DataSet = New DataSet
            gtm = SecurityModule.GetTeamMember(DefaultTMID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            If commonFunctions.CheckDataSet(gtm) = True Then
                DefaultUserFullName = gtm.Tables(0).Rows(0).Item("FirstName") & " " & gtm.Tables(0).Rows(0).Item("LastName")
            End If

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then
                ''*************************************************************************
                ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                ''*************************************************************************
                Dim dsExp As DataSet = New DataSet
                dsExp = EXPModule.GetExpProjTooling(ViewState("pProjNo"), "", "", "", "", 0, 0, 0, 0, 0, "", "", "", "")
                If (dsExp.Tables.Item(0).Rows.Count > 0) Then
                    ''**********************
                    ''*Initialize Variables
                    ''**********************
                    Dim ToolingLead As Integer = dsExp.Tables(0).Rows(0).Item("ToolLeadTMID")
                    Dim PurchasingLead As Integer = dsExp.Tables(0).Rows(0).Item("PurchLeadTMID")
                    Dim ProjectTitle As String = dsExp.Tables(0).Rows(0).Item("ProjectTitle")
                    Dim ProjectType As String = dsExp.Tables(0).Rows(0).Item("ProjectType")
                    Dim SeqNo As Integer = 0
                    Dim NextSeqNo As Integer = 0
                    Dim NextLvl As Integer = 0
                    Select Case hfSeqNo.Value
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

                    ''********************************************************
                    ''Notify Account/Program Manager & Tooling/Purchasing Lead
                    ''********************************************************
                    dsRej = EXPModule.GetExpProjToolingLead(ViewState("pProjNo"))
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(dsRej) = True Then
                        For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                            If (((ProjectType = "Internal" And _
                                  (dsRej.Tables(0).Rows(i).Item("TMDesc") = "Tooling Lead" Or _
                                   dsRej.Tables(0).Rows(i).Item("TMDesc") = "Tooling Engr Mgr")) Or _
                                 (ProjectType = "External" And _
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

                    ''********************************************************
                    ''Send Notification only if there is a valid Email Address
                    ''********************************************************
                    If EmailTO <> Nothing Then
                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                        ''***************************************************************
                        ''Carbon Approvers in same level
                        ''***************************************************************
                        EmailCC = CarbonCopyList(MyMessage, 0, "", SeqNo, 0, EmailCC, "", DefaultTMID)


                        'Test or Production Message display
                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                        End If

                        MyMessage.Subject &= ProjectType & " - Customer Tooling Expense: " & ViewState("pProjNo") & " - " & ProjectTitle & " - MESSAGE RECEIVED"
                        MyMessage.Body = "<table style='font-size: 13; font-family: Tahoma;'>"
                        MyMessage.Body &= " <tr>"
                        MyMessage.Body &= "     <td valign='top' width='20%'>"
                        MyMessage.Body &= "         <img src='" & ViewState("strProdOrTestEnvironment") & "/images/messanger60.jpg'/>"
                        MyMessage.Body &= "     </td>"
                        MyMessage.Body &= "     <td valign='top'>"
                        MyMessage.Body &= "             <b>Attention All,</b> "
                        MyMessage.Body &= "             <p><b>" & DefaultUserFullName & "</b> sent a message regarding "
                        MyMessage.Body &= "             <font color='red'>" & ViewState("pProjNo") & " - " & ProjectTitle & "</font>."
                        MyMessage.Body &= "             <br/><br/><i>Question:&nbsp;&nbsp;</i><b>" & txtQC.Text & "</b>"
                        MyMessage.Body &= "             </p>"
                        MyMessage.Body &= "             <p><a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pRC=1" & "'>Click here</a> to respond."
                        MyMessage.Body &= "     </td>"
                        MyMessage.Body &= " </tr>"
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
                        EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Message Sent")

                        ''*****************
                        ''Save Message
                        ''*****************
                        EXPModule.InsertExpProjToolingRSS(ViewState("pProjNo"), ProjectTitle, DefaultTMID, SeqNo, txtQC.Text)

                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (T)", ViewState("pProjNo"))
                            lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                        Catch ex As SmtpException
                            lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                            UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                        End Try
                        lblErrors.Visible = True

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData()
                        gvQuestion.DataBind()

                    Else 'EmailTO = ''
                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData()

                        lblErrors.Text = "Unable to locate a valid email address to send message to. Please contact the Application Department for assistance."
                        lblErrors.Visible = True
                    End If 'EOF EmailTO <> ''
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
    End Sub 'EOF btnSubmit2_Click

    Public Function CarbonCopyList(ByVal MyMessage As MailMessage, ByVal SubscriptionID As Integer, ByVal UGNLoc As String, ByVal SeqNo As Integer, ByVal RejectedTMID As Integer, ByVal EmailCC As String, ByVal ProjectType As String, ByVal DefaultTMID As Integer) As String
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
                        If SubscriptionID = 10 Or SubscriptionID = 11 Or SubscriptionID = 12 Or SubscriptionID = 13 Or _
                            SubscriptionID = 52 Or SubscriptionID = 127 Or SubscriptionID = 145 Then
                            ''Notify Accounting or 1st level or 2nd level or 3rd level, Dflt Corp Engineer, Dflt Prgm Mgr
                            dsCC = commonFunctions.GetTeamMemberBySubscription(SubscriptionID)
                        End If
                    End If
                End If

                ''Check that the recipient(s) is a valid Team Member
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    If SubscriptionID = 0 Then
                        For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                            If ((ProjectType = "Internal" And dsCC.Tables(0).Rows(i).Item("TMDesc") = "Tooling Lead") Or _
                                (ProjectType = "External" And dsCC.Tables(0).Rows(i).Item("TMDesc") = "Purchasing Lead") Or _
                                (dsCC.Tables(0).Rows(i).Item("TMDesc") = "Account Manager" Or _
                                 dsCC.Tables(0).Rows(i).Item("TMDesc") = "Program Manager")) And _
                                 (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"

                            End If
                        Next
                    Else
                        For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                            If (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then

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


    Protected Sub btnReset2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset2.Click
        Response.Redirect("crExpProjToolingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "&pAprv=1", False)
    End Sub 'EOF btnReset2_Click

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