' ************************************************************************************************
' Name:	crExpProjDevelopmentApproval.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from ExpProj_Development table
'           and other sources.  Also used as an approval screen to allow users to view current
'           info related to a capex Development project and approve/reject the project in one screen.
' Date		    Author	    
' 05/10/2012    LRey		Created .Net application
' 01/07/2013    LRey        Added a control to hide the Edit button in the approval process to prevent out of sequence approval.
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class EXP_crViewExpProjDevelopment
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")

        Dim m As ASP.crviewmasterpage_master = Master
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            If ViewState("pProjNo") = Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='DevelopmentExpProjList.aspx'><b>Development Project Search</b></a> > Development Project Preview"
            Else
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='DevelopmentExpProjList.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'><b>Development Project Search</b></a> > <a href='DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'><b>Development Project</b></a> > Approval"
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
            dsExp = EXPModule.GetExpProjRepair(ViewState("pProjNo"), "", "", "", 0, 0, "")
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
            Dim iFormID As Integer = 130 'Development Expense Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "David.Passmore", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

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
                                        ViewState("ObjectRole") = False
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
                                        ViewState("ObjectRole") = False
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
                    oRpt.Load(Server.MapPath(".\Forms\") & "crExpProjDevelopment.rpt")

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

            ' the parameter fields to be sent to the report 
            Dim pfProjectNo As ParameterField = New ParameterField
            Dim pfSubProjectNo As ParameterField = New ParameterField
            Dim pfProjectTitle As ParameterField = New ParameterField
            Dim pfRequestedByTMID As ParameterField = New ParameterField
            Dim pfProjectLeaderTMID As ParameterField = New ParameterField
            Dim pfAcctMgrTMID As ParameterField = New ParameterField
            Dim pfUGNFacility As ParameterField = New ParameterField
            Dim pfDeptOrCostCenter As ParameterField = New ParameterField
            Dim pfProgramID As ParameterField = New ParameterField
            Dim pfCustomer As ParameterField = New ParameterField
            Dim pfCommodityID As ParameterField = New ParameterField
            Dim pfRoutingStatus As ParameterField = New ParameterField

            ' setting the name of parameter fields with which they will be recieved in report 
            pfProjectNo.ParameterFieldName = "@ProjectNo"
            pfSubProjectNo.ParameterFieldName = "@SupProjectNo"
            pfProjectTitle.ParameterFieldName = "@ProjectTitle"
            pfRequestedByTMID.ParameterFieldName = "@RequestedByTMID"
            pfProjectLeaderTMID.ParameterFieldName = "@ProjectLeaderTMID"
            pfAcctMgrTMID.ParameterFieldName = "@AcctMgrTMID"
            pfUGNFacility.ParameterFieldName = "@UGNFacility"
            pfDeptOrCostCenter.ParameterFieldName = "@DeptOrCostCenter"
            pfProgramID.ParameterFieldName = "@ProgramID"
            pfCustomer.ParameterFieldName = "@Customer"
            pfCommodityID.ParameterFieldName = "@CommodityID"
            pfRoutingStatus.ParameterFieldName = "@RoutingStatus"

            ' the above declared parameter fields accept values as discrete objects 
            ' so declaring discrete objects 
            Dim dcProjectNo As New ParameterDiscreteValue
            Dim dcSubProjectNo As New ParameterDiscreteValue
            Dim dcProjectTitle As New ParameterDiscreteValue
            Dim dcRequestedByTMID As New ParameterDiscreteValue
            Dim dcProjectLeaderTMID As New ParameterDiscreteValue
            Dim dcAcctMgrTMID As New ParameterDiscreteValue
            Dim dcUGNFacility As New ParameterDiscreteValue
            Dim dcDeptOrCostCenter As New ParameterDiscreteValue
            Dim dcProgramID As New ParameterDiscreteValue
            Dim dcCustomer As New ParameterDiscreteValue
            Dim dcCommodityID As New ParameterDiscreteValue
            Dim dcRoutingStatus As New ParameterDiscreteValue

            ' setting the values of discrete objects 
            dcProjectNo.Value = ViewState("pProjNo")
            dcSubProjectNo.Value = ""
            dcProjectTitle.Value = ""
            dcRequestedByTMID.Value = 0
            dcProjectLeaderTMID.Value = 0
            dcAcctMgrTMID.Value = 0
            dcUGNFacility.Value = ""
            dcDeptOrCostCenter.Value = 0
            dcProgramID.Value = 0
            dcCustomer.Value = ""
            dcCommodityID.Value = 0
            dcRoutingStatus.Value = ""

            ' now adding these discrete values to parameters 
            pfProjectNo.CurrentValues.Add(dcProjectNo)
            pfSubProjectNo.CurrentValues.Add(dcSubProjectNo)
            pfProjectTitle.CurrentValues.Add(dcProjectTitle)
            pfRequestedByTMID.CurrentValues.Add(dcRequestedByTMID)
            pfProjectLeaderTMID.CurrentValues.Add(dcProjectLeaderTMID)
            pfAcctMgrTMID.CurrentValues.Add(dcAcctMgrTMID)
            pfUGNFacility.CurrentValues.Add(dcUGNFacility)
            pfDeptOrCostCenter.CurrentValues.Add(dcDeptOrCostCenter)
            pfProgramID.CurrentValues.Add(dcProgramID)
            pfCustomer.CurrentValues.Add(dcCustomer)
            pfCommodityID.CurrentValues.Add(dcCommodityID)
            pfRoutingStatus.CurrentValues.Add(dcRoutingStatus)

            ' now adding all these parameter fields to the parameter collection 
            paramFields.Add(pfProjectNo)
            paramFields.Add(pfSubProjectNo)
            paramFields.Add(pfProjectTitle)
            paramFields.Add(pfRequestedByTMID)
            paramFields.Add(pfProjectLeaderTMID)
            paramFields.Add(pfAcctMgrTMID)
            paramFields.Add(pfUGNFacility)
            paramFields.Add(pfDeptOrCostCenter)
            paramFields.Add(pfProgramID)
            paramFields.Add(pfCustomer)
            paramFields.Add(pfCommodityID)
            paramFields.Add(pfRoutingStatus)

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
        Dim ds As DataSet = New DataSet
        If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
            commonFunctions.SetUGNDBUser()
        End If

        Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")

        Try
            ds = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), 0, DefaultTMID, False, False)
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
        Response.Redirect("crExpProjDevelopmentApproval.aspx?pProjNo=" & ViewState("pProjNo"), False)

    End Sub 'EOF btnReset_Click

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        ''********
        ''* This function is used to submit email next level Approvers or to originators when rejected.
        ''********188 371 510 569
        Dim DefaultDate As Date = Date.Today
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")
        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
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
            EmailFrom = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
            EmailCC = CurrentEmpEmail
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
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pProjNo") <> Nothing Then
                    If ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
                        lblErrors.Text = "Your comments is required for Rejection."
                        lblErrors.Visible = True
                        ReqComments.Visible = True
                        CheckRights()
                    Else
                        ''*************************************************************************
                        ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                        ''*************************************************************************
                        Dim dsExp As DataSet = New DataSet
                        dsExp = EXPModule.GetExpProjDevelopment(ViewState("pProjNo"), "", "", 0, 0, 0, "", 0, 0, "", 0, "")
                        If (dsExp.Tables.Item(0).Rows.Count > 0) Then
                            ''**********************
                            ''*Initialize Variables
                            ''**********************
                            Dim ProjectLeader As Integer = dsExp.Tables(0).Rows(0).Item("ProjectLeaderTMID").ToString
                            Dim ProjectLeaderName As String = dsExp.Tables(0).Rows(0).Item("ProjectLeaderName").ToString
                            Dim ProjectTitle As String = dsExp.Tables(0).Rows(0).Item("ProjectTitle").ToString
                            Dim ProjDateNotes As String = dsExp.Tables(0).Rows(0).Item("ProjDtNotes").ToString
                            Dim Justification As String = dsExp.Tables(0).Rows(0).Item("Justification").ToString
                            Dim UGNFacilityName As String = dsExp.Tables(0).Rows(0).Item("UGNFacilityName").ToString
                            Dim UGNFacility As String = dsExp.Tables(0).Rows(0).Item("UGNFacility").ToString
                            Dim Department As String = dsExp.Tables(0).Rows(0).Item("ddDepartmentName").ToString
                            Dim VehicleSOP As String = dsExp.Tables(0).Rows(0).Item("VehicleSOP").ToString
                            Dim EstCmpltDt As String = dsExp.Tables(0).Rows(0).Item("EstCmpltDt").ToString
                            Dim EstSpendDt As String = dsExp.Tables(0).Rows(0).Item("EstSpendDt").ToString
                            Dim EstEndSpendDt As String = dsExp.Tables(0).Rows(0).Item("EstEndSpendDt").ToString
                            Dim Budgeted As String = IIf(dsExp.Tables(0).Rows(0).Item("Budgeted") = "True", "Yes", "No")
                            Dim TotalRequest As Decimal = Format(dsExp.Tables(0).Rows(0).Item("TotalRequest"), "#,##0.00")
                            Dim CustReimb As String = Format(dsExp.Tables(0).Rows(0).Item("CustReimb"), "#,##0.00")
                            Dim TotalInv As String = Format(dsExp.Tables(0).Rows(0).Item("TotalInv"), "#,##0.00")
                            Dim CRProjNo As Integer = dsExp.Tables(0).Rows(0).Item("CRProjectNo").ToString
                            Dim CRProjNoDesc As String = dsExp.Tables(0).Rows(0).Item("CRProjectNoDesc").ToString()

                            Dim SeqNo As Integer = 0
                            Dim NextSeqNo As Integer = 0
                            Dim NextLvl As Integer = 0

                            Select Case hfSeqNo.Value
                                Case 1
                                    SeqNo = 1
                                    NextSeqNo = 2
                                    NextLvl = 129
                                Case 2
                                    SeqNo = 2
                                    NextSeqNo = 3
                                    NextLvl = 130
                                Case 3
                                    SeqNo = 3
                                    NextSeqNo = 0
                                    NextLvl = 130
                            End Select

                            ''**********************************************************************
                            ''Check for same level records Rejected. IF so, cancel approval process.
                            ''**********************************************************************
                            If ddStatus.SelectedValue <> "Pending" Then
                                ''***********************************
                                ''Update Current Level Approver record.
                                ''***********************************
                                EXPModule.UpdateExpProjDevelopmentApproval(ViewState("pProjNo"), DefaultTMID, True, ddStatus.SelectedValue, txtComments.Text, SeqNo, 0, DefaultUser, DefaultDate)

                                ''*****************
                                ''Level Completed
                                ''*****************
                                ds1st = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), SeqNo, 0, False, True)
                                'Locate any Rejected
                                If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                    'do nothing
                                Else
                                    ds1st = EXPModule.GetAssetsExpProjApproval(ViewState("pProjNo"), SeqNo, 0, True, False)
                                    'Located any Pending
                                    If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                        'do nothing
                                    Else 'otherwise all are approved
                                        LvlApvlCmplt = True
                                    End If
                                End If
                            End If

                            ''************************
                            ''* Update Assets record
                            '*************************
                            If SeqNo = 3 Then 'Last Team Member
                                EXPModule.UpdateExpProjDevelopmentStatus(ViewState("pProjNo"), IIf(ddStatus.SelectedValue = "Rejected", "In Process", "Approved"), IIf(ddStatus.SelectedValue = "Rejected", "R", "A"), DefaultUser, DefaultDate)
                            Else 'Not the Last Team Member
                                EXPModule.UpdateExpProjDevelopmentStatus(ViewState("pProjNo"), "In Process", IIf(ddStatus.SelectedValue = "Rejected", "R", "T"), DefaultUser, DefaultDate)
                            End If

                            ''**************************************************************
                            ''Locate Next Level Approver(s)
                            ''************************************************************** 
                            If LvlApvlCmplt = True Then
                                ''Check at same sequence level
                                ds1st = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), SeqNo, 0, True, False)
                                If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then ' Then
                                    ''Do not send email at same level twice.
                                Else
                                    If ddStatus.SelectedValue <> "Rejected" Then 'Team Member Approved
                                        ds2nd = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), IIf(SeqNo < 3, (SeqNo + 1), SeqNo), 0, True, False)
                                        If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                                            For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                                If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                                    If (ds2nd.Tables(0).Rows(i).Item("Email") <> Nothing) And (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then ''change to DefaultTMID   
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
                                                        ''Update Next Level Approvers DateNotified field.
                                                        ''*****************************************
                                                        EXPModule.UpdateExpProjDevelopmentApproval(ViewState("pProjNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", IIf(SeqNo < 3, (SeqNo + 1), SeqNo), 0, DefaultUser, DefaultDate)
                                                    End If
                                                End If
                                            Next
                                        End If 'EOF ds2nd.Tables.Count > 0 
                                    End If 'EOF t.SelectedValue <> "Rejected"

                                    'Rejected or last approval
                                    If ddStatus.SelectedValue = "Rejected" Or (SeqNo = 3 And ddStatus.SelectedValue = "Approved") Then
                                        ''********************************************************
                                        ''Notify Project Lead
                                        ''********************************************************
                                        Dim toEmpName As String = Nothing
                                        dsRej = EXPModule.GetExpProjDevelopmentLead(ViewState("pProjNo"))
                                        ''Check that the recipient(s) is a valid Team Member
                                        If dsRej.Tables.Count > 0 And (dsRej.Tables.Item(0).Rows.Count > 0) Then
                                            For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                                If (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True) And (dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
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
                            Else '' If LvlApvlCmplt is false
                                'Rejected or last approval
                                If ddStatus.SelectedValue = "Rejected" Or (SeqNo = 3 And ddStatus.SelectedValue = "Approved") Then
                                    ''********************************************************
                                    ''Notify Project Lead
                                    ''********************************************************
                                    dsRej = EXPModule.GetExpProjDevelopmentLead(ViewState("pProjNo"))
                                    ''Check that the recipient(s) is a valid Team Member
                                    If dsRej.Tables.Count > 0 And (dsRej.Tables.Item(0).Rows.Count > 0) Then
                                        For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                            If (dsRej.Tables(0).Rows(i).Item("TMDesc") = "Requested By") And (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True) And (dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
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
                            End If 'EOF If LvlApvlCmplt = True Then

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
                                    EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), UGNFacility, 0, 0, EmailCC, DefaultTMID)

                                    If SeqNo = 3 Then
                                        ''**************************************************************
                                        ''Carbon Copy 1 Level Approvers
                                        ''**************************************************************
                                        EmailCC = CarbonCopyList(MyMessage, (NextLvl - 2), UGNFacility, 0, 0, EmailCC, DefaultTMID)

                                        ''**************************************************************
                                        ''Carbon Copy Last Level Approvers
                                        ''**************************************************************
                                        EmailCC = CarbonCopyList(MyMessage, NextLvl, UGNFacility, 0, 0, EmailCC, DefaultTMID)
                                    End If
                                Else
                                    EmailCC = CarbonCopyList(MyMessage, (NextLvl - 1), UGNFacility, 0, 0, EmailCC, DefaultTMID)
                                End If

                                If (SeqNo < 3 And ddStatus.SelectedValue <> "Rejected") Then
                                    ''**************************************************************
                                    ''*Carbon Copy Account Manager & Project Lead
                                    ''**************************************************************
                                    EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                                ElseIf (SeqNo = 3 And ddStatus.SelectedValue <> "Rejected") Then
                                    ''**************************************************************
                                    ''*Carbon Copy Account Manager & Project Lead
                                    ''**************************************************************
                                    EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                                    ''********************************
                                    ''Carbon Copy Accounting
                                    ''********************************
                                    EmailCC = CarbonCopyList(MyMessage, 87, UGNFacility, 0, 0, EmailCC, DefaultTMID)
                                    ''**************************************
                                    ''*Carbon Copy the Cc List
                                    ''**************************************
                                    EmailCC = CarbonCopyList(MyMessage, 131, "", 0, 0, EmailCC, DefaultTMID)

                                    ''*********************************************************
                                    ''*Carbon Copy the Operations Manager based on UGNFacility
                                    ''*********************************************************
                                    EmailCC = CarbonCopyList(MyMessage, 132, UGNFacility, 0, 0, EmailCC, DefaultTMID)
                                End If 'EOF If (SeqNo < 3 And ddStatus.SelectedValue <> "Rejected") Then

                                If ddStatus.SelectedValue = "Rejected" Then
                                    ''********************************
                                    ''Carbon Copy Accounting
                                    ''********************************
                                    EmailCC = CarbonCopyList(MyMessage, 87, "", 0, 0, EmailCC, DefaultTMID)

                                End If
                                'Test or Production Message display
                                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                                    MyMessage.Subject = "TEST: "
                                    MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                                Else
                                    MyMessage.Subject = ""
                                    MyMessage.Body = ""
                                End If

                                If ddStatus.SelectedValue = "Rejected" Then
                                    MyMessage.Subject = " Development Project: " & ViewState("pProjNo") & " - " & ProjectTitle & " - REJECTED"
                                    MyMessage.Body = "<font size='2' face='Tahoma'>" & EmpName & "</font>"
                                    MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & ViewState("pProjNo") & " '" & ProjectTitle & "' was <font color='red'>REJECTED</font>. <br/>Reason for rejection: " & txtComments.Text & "</p> <p><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</font></p>"
                                Else
                                    MyMessage.Subject = "Development Project: " & ViewState("pProjNo") & " - " & ProjectTitle
                                    If SeqNo = 3 Then
                                        MyMessage.Subject &= "- APPROVED"

                                        ''Redirect users to Preview Form at final Approval
                                        MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & ViewState("pProjNo") & " '" & ProjectTitle & "' is Approved. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</font></p>"
                                    Else
                                        MyMessage.Body = "<font size='2' face='Tahoma'>" & EmpName & "</font>"

                                        ''Redirect users to Approval screen if not final approval
                                        MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & ViewState("pProjNo") & " '" & ProjectTitle & "' is available for your Review/Approval. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/crExpProjDevelopmentApproval.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</font></p>"
                                    End If
                                End If

                                MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"
                                MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>PROJECT OVERVIEW</strong></td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Project No:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & ViewState("pProjNo") & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Project Title:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & ProjectTitle & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Vehicle SOP Date:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & VehicleSOP & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Project Leader:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & ProjectLeaderName & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Description:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td style='width: 700px;'>" & ProjDateNotes & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Justification:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td style='width: 700px;'>" & Justification & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & UGNFacilityName & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Department or Cost Center:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & Department & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Estimated Completion Date:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & EstCmpltDt & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Estimated Start Spend Date:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & EstSpendDt & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Estimated End Spend Date:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & EstEndSpendDt & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right' style='width: 250px;'>Budgeted:&nbsp;&nbsp;  </td>"
                                MyMessage.Body &= "<td>" & Budgeted & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>INVESTMENTS</strong></td>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Total Request ($):&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & TotalRequest & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Expected Customer&nbsp;&nbsp;&nbsp;&nbsp;<br/>Reimbursement ($):&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & CustReimb & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Total Investment ($):&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & TotalInv & "</td>"
                                MyMessage.Body &= "</tr>"

                                If CRProjNo <> 0 Then
                                    MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>JUSTIFICATION</strong></td>"
                                    MyMessage.Body &= "<tr>"
                                    MyMessage.Body &= "<td class='p_text' align='right'>Cost Reduction Ref #:&nbsp;&nbsp; </td>"
                                    MyMessage.Body &= "<td><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/CostReduction.aspx?pProjNo=" & CRProjNo & "' target='_blank'>" & CRProjNoDesc & "</a></td>"
                                    MyMessage.Body &= "</tr>"
                                End If

                                ''***************************************************
                                ''Get list of Supporting Documentation
                                ''***************************************************
                                Dim dsAED As DataSet
                                dsAED = EXPModule.GetDevelopmentExpDocument(ViewState("pProjNo"), 0)
                                If dsAED.Tables.Count > 0 And (dsAED.Tables.Item(0).Rows.Count > 0) Then
                                    MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
                                    MyMessage.Body &= "<td colspan='2'><strong>SUPPORTING DOCUMENT(S):</strong></td>"
                                    MyMessage.Body &= "</tr>"
                                    MyMessage.Body &= "<tr>"
                                    MyMessage.Body &= "<td colspan='2'>"
                                    MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
                                    MyMessage.Body &= "  <tr>"
                                    MyMessage.Body &= "   <td width='250px'><b>File Description</b></td>"
                                    MyMessage.Body &= "   <td width='250px'>&nbsp;</td>"
                                    MyMessage.Body &= "</tr>"
                                    For i = 0 To dsAED.Tables.Item(0).Rows.Count - 1
                                        MyMessage.Body &= "<tr>"
                                        MyMessage.Body &= "<td height='25'>" & dsAED.Tables(0).Rows(i).Item("Description") & "</td>"
                                        MyMessage.Body &= "<td height='25'><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/DevelopmentExpProjDocument.aspx?pProjNo=" & ViewState("pProjNo") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
                                        MyMessage.Body &= "</tr>"
                                    Next
                                    MyMessage.Body &= "</table>"
                                    MyMessage.Body &= "</tr>"
                                End If
                                MyMessage.Body &= "</table>"

                                ''************************
                                '' The next few lines of logic to remove duplicate email addresses does not work.
                                ''************************
                                ' ''Dim emailList2 As String() = commonFunctions.CleanEmailList(EmailTO).Split(";")
                                ' ''Dim toEmail As String = Nothing
                                ' ''For i = 0 To UBound(emailList2)
                                ' ''    If emailList2(i) <> ";" And emailList2(i).Trim <> "" And emailList2(i) <> EmailTO Then
                                ' ''        toEmail += emailList2(i) & ";"
                                ' ''    End If
                                ' ''Next i
                                ' ''EmailTO = toEmail

                                ' ''Dim emailList As String() = commonFunctions.CleanEmailList(EmailCC).Split(";")
                                ' ''Dim ccEmail As String = Nothing
                                ' ''For i = 0 To UBound(emailList)
                                ' ''    If emailList(i) <> ";" And emailList(i).Trim <> "" And emailList(i) <> EmailTO Then
                                ' ''        ccEmail += emailList(i) & ";"
                                ' ''    End If
                                ' ''Next i
                                ' ''EmailCC = ccEmail

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
                                EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, ddStatus.SelectedValue & " " & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing), "", "", "", "")
                                ''*****************
                                ''History Tracking
                                ''*****************
                                If ddStatus.SelectedValue <> "Rejected" Then
                                    If SeqNo = 3 Then
                                        EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Notification sent to all involved. ", "", "", "", "")
                                    Else
                                        EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Notification sent to level " & IIf(SeqNo < 3, (SeqNo + 1), SeqNo) & " approver(s): " & EmpName, "", "", "", "")
                                    End If
                                Else
                                    EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Notification sent to " & EmpName, "", "", "", "")
                                End If

                                ''**********************************
                                ''Connect & Send email notification
                                ''**********************************
                                Try
                                    commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (D)", ViewState("pProjNo"))
                                    lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                    lblErrors.Visible = True
                                Catch ex As Exception
                                    lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                    lblErrors.Visible = True

                                    UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                    'get current event name
                                    Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                                    'log and email error
                                    UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                                End Try

                                ''**********************************
                                ''Rebind the data to the form
                                ''********************************** 
                                BindData()
                            Else
                                ''*****************
                                ''History Tracking
                                ''*****************
                                EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, ddStatus.SelectedValue & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing), "", "", "", "")

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
            Dim DefaultDate As Date = Date.Today
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")
            Dim DefaultUserFullName As String = Nothing
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
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
                EmailFrom = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailCC = CurrentEmpEmail
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
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pProjNo") <> Nothing Then
                    ''*************************************************************************
                    ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                    ''*************************************************************************
                    Dim dsExp As DataSet = New DataSet
                    dsExp = EXPModule.GetExpProjDevelopment(ViewState("pProjNo"), "", "", 0, 0, 0, "", 0, 0, "", 0, "")
                    If (dsExp.Tables.Item(0).Rows.Count > 0) Then
                        ''**********************
                        ''*Initialize Variables
                        ''**********************
                        Dim ProjectLeader As Integer = dsExp.Tables(0).Rows(0).Item("ProjectLeaderTMID")
                        Dim ProjectTitle As String = dsExp.Tables(0).Rows(0).Item("ProjectTitle")
                        Dim SeqNo As Integer = 0
                        Dim NextSeqNo As Integer = 0
                        Dim NextLvl As Integer = 0

                        Select Case hfSeqNo.Value
                            Case 1
                                SeqNo = 1
                                NextSeqNo = 2
                                NextLvl = 129
                            Case 2
                                SeqNo = 2
                                NextSeqNo = 3
                                NextLvl = 130
                            Case 3
                                SeqNo = 3
                                NextSeqNo = 0
                                NextLvl = 130
                        End Select

                        ''********************************************************
                        ''Notify Project Lead/Requested By
                        ''********************************************************
                        dsRej = EXPModule.GetExpProjDevelopmentLead(ViewState("pProjNo"))
                        ''Check that the recipient(s) is a valid Team Member
                        If commonFunctions.CheckDataSet(dsRej) = True Then
                            For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                If (dsRej.Tables(0).Rows(i).Item("TMDesc") <> "Account Manager") And (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True) Then
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
                            EmailCC = CarbonCopyList(MyMessage, 0, "", SeqNo, 0, EmailCC, DefaultTMID)

                            'Test or Production Message display
                            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                                MyMessage.Subject = "TEST: "
                                MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                            Else
                                MyMessage.Subject = ""
                                MyMessage.Body = ""
                            End If

                            MyMessage.Subject &= "Development Project: " & ViewState("pProjNo") & " - " & ProjectTitle & " - MESSAGE RECIEVED"

                            MyMessage.Body = "<table style='font-size: 13; font-family: Tahoma;'>"
                            MyMessage.Body &= " <tr>"
                            MyMessage.Body &= "     <td valign='top' width='20%'>"
                            MyMessage.Body &= "         <img src='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/images/messanger60.jpg'/>"
                            MyMessage.Body &= "     </td>"
                            MyMessage.Body &= "     <td valign='top'>"
                            MyMessage.Body &= "             <b>Attention All,</b> "
                            MyMessage.Body &= "             <p><b>" & DefaultUserFullName & "</b> sent a message regarding "
                            MyMessage.Body &= "             <font color='red'>" & ViewState("pProjNo") & " - " & ProjectTitle & "</font>."
                            MyMessage.Body &= "             <br/><br/><i>Question:&nbsp;&nbsp;</i><b>" & txtQC.Text & "</b>"
                            MyMessage.Body &= "             </p>"
                            MyMessage.Body &= "             <p><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/DevelopmentExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pRC=1" & "'>Click here</a> to respond."
                            MyMessage.Body &= "     </td>"
                            MyMessage.Body &= " </tr>"
                            MyMessage.Body &= "</table>"
                            MyMessage.Body &= "<br/><br/>"

                            Dim emailList As String() = commonFunctions.CleanEmailList(EmailCC).Split(";")
                            Dim ccEmail As String = Nothing
                            For i = 0 To UBound(emailList)
                                If emailList(i) <> ";" And emailList(i).Trim <> "" And emailList(i) <> EmailCC Then
                                    ccEmail += emailList(i) & ";"
                                End If
                            Next i
                            EmailCC = ccEmail

                            Dim emailList2 As String() = commonFunctions.CleanEmailList(EmailTO).Split(";")
                            Dim toEmail As String = Nothing
                            For i = 0 To UBound(emailList2)
                                If emailList2(i) <> ";" And emailList2(i).Trim <> "" And emailList2(i) <> EmailTO Then
                                    toEmail += emailList2(i) & ";"
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
                            EXPModule.InsertExpProjDevelopmentHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Message Sent", "", "", "", "")

                            ''*****************
                            ''Save Message
                            ''*****************
                            EXPModule.InsertExpProjDevelopmentRSS(ViewState("pProjNo"), ProjectTitle, DefaultTMID, SeqNo, txtQC.Text)

                            ''**********************************
                            ''Connect & Send email notification
                            ''**********************************
                            Try
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (D)", ViewState("pProjNo"))
                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                lblErrors.Visible = True
                            Catch ex As Exception
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                lblErrors.Visible = True

                                UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                                'get current event name
                                Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                                'log and email error
                                UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                            End Try


                            ''**********************************
                            ''Rebind the data to the form
                            ''********************************** 
                            txtQC.Text = Nothing
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

    Public Function CarbonCopyList(ByVal MyMessage As MailMessage, ByVal SubscriptionID As Integer, ByVal UGNLoc As String, ByVal SeqNo As Integer, ByVal RejectedTMID As Integer, ByVal EmailCC As String, ByVal DefaultTMID As Integer) As String
        Try
            Dim dsCC As DataSet = New DataSet
            Dim IncludeOrigAprvlTM As Boolean = False
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            If SeqNo = 0 Then 'No Rejections have been made, Send notification to all who applies
                If SubscriptionID = 0 Then ''Account Mananager
                    dsCC = EXPModule.GetExpProjDevelopmentLead(ViewState("pProjNo"))
                    IncludeOrigAprvlTM = True
                Else '' All others based on facility
                    If UGNLoc <> Nothing Then
                        dsCC = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(SubscriptionID, UGNLoc)
                    Else
                        If SubscriptionID = 87 Or SubscriptionID = 131 Or SubscriptionID = 128 Or SubscriptionID = 129 Then
                            ''Notify Accounting, CC List, or 1st level or 2nd level
                            dsCC = commonFunctions.GetTeamMemberBySubscription(SubscriptionID)
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
                dsCC = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), SeqNo, 0, False, False)
                'Carbon Copy pending approvers at same level as who rejected the record.
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And (RejectedTMID <> dsCC.Tables(0).Rows(i).Item("TeamMemberID") And (dsCC.Tables(0).Rows(i).Item("Status") = "Pending")) Then ''change to DefaultTMID   
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
                dsCC = EXPModule.GetDevelopmentExpProjApproval(ViewState("pProjNo"), 0, 0, False, False)
                'CC Orgiginal Approver in an event that their a corporate calendar default responsibility
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And (DefaultTMID <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) Then ''change to DefaultTMID   
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

    Protected Sub btnReset2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset2.Click
        Response.Redirect("crExpProjDevelopmentApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1", False)
    End Sub 'EOF btnReset2_Click

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim RSSID As Integer
                Dim drRSSID As ExpProjDevelopment.ExpProj_Development_RSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, ExpProjDevelopment.ExpProj_Development_RSSRow)

                If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                    RSSID = drRSSID.RSSID
                    ' Reference the rpCBRC ObjectDataSource
                    Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                    ' Set the CategoryID Parameter value
                    rpCBRC.SelectParameters("ProjectNo").DefaultValue = drRSSID.ProjectNo.ToString()
                    rpCBRC.SelectParameters("RSSID").DefaultValue = drRSSID.RSSID.ToString()
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