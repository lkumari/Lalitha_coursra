' ************************************************************************************************
' Name:	crExpProjPackagingApproval.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from ExpProj_Packaging table
'           and other sources.  Also used as an approval screen to allow users to view current
'           info related to a capex asset project and approve/reject the project in one screen.
' Date		    Author	    
' 07/23/2010    LRey		Created .Net application
' 01/07/2013    LRey        Added a control to hide the Edit button in the approval process to prevent out of sequence approval.
' ************************************************************************************************

#Region "Directives"
Imports System.Net.Mail
Imports System.Threading
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
#End Region

Partial Class EXP_crViewExpProjPackaging
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
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='PackagingExpProjList.aspx'><b>Packaging Expenditure Search</b></a> > Packaging Expenditure Preview"
            Else
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='PackagingExpProjList.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'><b>Packaging Expenditure Search</b></a> > <a href='PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'><b>Packaging Expenditure</b></a> > Approval"
            End If
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
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
            dsExp = EXPModule.GetExpProjPackaging(ViewState("pProjNo"), "", "", "", 0, "", 0, "", "", "")
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
            Dim iFormID As Integer = 123 'Packaging Approval Screen
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Peter.Anthony", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

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
        Dim oRpt = New ReportDocument()
        Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
        Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
        Dim dbConn = New TableLogOnInfo()

        If ViewState("pProjNo") <> "" Then
            Try
                If Session("TempCrystalRptFiles") Is Nothing Then
                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "crExpProjPackaging.rpt")

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
            Dim pfUGNFacility As ParameterField = New ParameterField
            Dim pfProjectLeaderTMID As ParameterField = New ParameterField
            Dim pfCustomer As ParameterField = New ParameterField
            Dim pfProgramID As ParameterField = New ParameterField
            Dim pfPartNo As ParameterField = New ParameterField
            Dim pfPartDesc As ParameterField = New ParameterField
            Dim pfProjectStatus As ParameterField = New ParameterField

            ' setting the name of parameter fields with which they will be recieved in report 
            pfProjectNo.ParameterFieldName = "@ProjectNo"
            pfSubProjectNo.ParameterFieldName = "@SupProjectNo"
            pfProjectTitle.ParameterFieldName = "@ProjectTitle"
            pfUGNFacility.ParameterFieldName = "@UGNFacility"
            pfProjectLeaderTMID.ParameterFieldName = "@ProjectLeaderTMID"
            pfCustomer.ParameterFieldName = "@Customer"
            pfProgramID.ParameterFieldName = "@ProgramID"
            pfPartNo.ParameterFieldName = "@PartNo"
            pfPartDesc.ParameterFieldName = "@PartDesc"
            pfProjectStatus.ParameterFieldName = "@ProjectStatus"

            ' the above declared parameter fields accept values as discrete objects 
            ' so declaring discrete objects 
            Dim dcProjectNo As New ParameterDiscreteValue
            Dim dcSubProjectNo As New ParameterDiscreteValue
            Dim dcProjectTitle As New ParameterDiscreteValue
            Dim dcUGNFacility As New ParameterDiscreteValue
            Dim dcProjectLeaderTMID As New ParameterDiscreteValue
            Dim dcCustomer As New ParameterDiscreteValue
            Dim dcProgramID As New ParameterDiscreteValue
            Dim dcPartNo As New ParameterDiscreteValue
            Dim dcPartDesc As New ParameterDiscreteValue
            Dim dcProjectStatus As New ParameterDiscreteValue

            ' setting the values of discrete objects 
            dcProjectNo.Value = ViewState("pProjNo")
            dcSubProjectNo.Value = ""
            dcProjectTitle.Value = ""
            dcUGNFacility.Value = ""
            dcProjectLeaderTMID.Value = 0
            dcCustomer.Value = ""
            dcProgramID.Value = 0
            dcPartNo.Value = ""
            dcPartDesc.Value = ""
            dcProjectStatus.Value = ""

            ' now adding these discrete values to parameters 
            pfProjectNo.CurrentValues.Add(dcProjectNo)
            pfSubProjectNo.CurrentValues.Add(dcSubProjectNo)
            pfProjectTitle.CurrentValues.Add(dcProjectTitle)
            pfUGNFacility.CurrentValues.Add(dcUGNFacility)
            pfProjectLeaderTMID.CurrentValues.Add(dcProjectLeaderTMID)
            pfCustomer.CurrentValues.Add(dcCustomer)
            pfProgramID.CurrentValues.Add(dcProgramID)
            pfPartNo.CurrentValues.Add(dcPartNo)
            pfPartDesc.CurrentValues.Add(dcPartDesc)
            pfProjectStatus.CurrentValues.Add(dcProjectStatus)

            ' now adding all these parameter fields to the parameter collection 
            paramFields.Add(pfProjectNo)
            paramFields.Add(pfSubProjectNo)
            paramFields.Add(pfProjectTitle)
            paramFields.Add(pfUGNFacility)
            paramFields.Add(pfProjectLeaderTMID)
            paramFields.Add(pfCustomer)
            paramFields.Add(pfProgramID)
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
        Dim ds As DataSet = New DataSet
        If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
            commonFunctions.SetUGNDBUser()
        End If

        Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")

        Try
            ds = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), 0, DefaultTMID, False, False)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                lblTeamMbr.Text = ds.Tables(0).Rows(0).Item("TeamMemberName").ToString()
                lblDateNotified.Text = ds.Tables(0).Rows(0).Item("DateNotified").ToString()
                ddStatus.SelectedValue = ds.Tables(0).Rows(0).Item("Status").ToString()
                txtComments.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                hfSeqNo.Value = ds.Tables(0).Rows(0).Item("SeqNo").ToString()

                If ds.Tables(0).Rows(0).Item("DateSigned").ToString() <> Nothing Then
                    btnSubmit.Enabled = False
                    btnReset.Enabled = False
                Else
                    btnSubmit.Enabled = True
                    btnReset.Enabled = True
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
        Response.Redirect("crExpProjPackagingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1", False)

    End Sub 'EOF btnReset_Click

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        ''********
        ''* This function is used to submit email next level Approvers or to originators when rejected
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
                    CheckRights()
                Else 'ELSE If ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
                    ''*************************************************************************
                    ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                    ''*************************************************************************
                    Dim dsExp As DataSet = New DataSet
                    dsExp = EXPModule.GetExpProjPackaging(ViewState("pProjNo"), "", "", "", 0, "", 0, "", "", "")
                    If commonFunctions.CheckDataSet(dsExp) = True Then
                        ''**********************
                        ''*Initialize Variables
                        ''**********************
                        Dim ProjectTitle As String = dsExp.Tables(0).Rows(0).Item("ProjectTitle")
                        Dim ProjDateNotes As String = dsExp.Tables(0).Rows(0).Item("ProjDtNotes")
                        Dim ProjectLeader As String = dsExp.Tables(0).Rows(0).Item("ProjectLeaderName")
                        Dim UT As Boolean = dsExp.Tables(0).Rows(0).Item("UT")
                        Dim UN As Boolean = dsExp.Tables(0).Rows(0).Item("UN")
                        Dim UP As Boolean = dsExp.Tables(0).Rows(0).Item("UP")
                        Dim UR As Boolean = dsExp.Tables(0).Rows(0).Item("UR")
                        Dim US As Boolean = dsExp.Tables(0).Rows(0).Item("US")
                        Dim UW As Boolean = dsExp.Tables(0).Rows(0).Item("UW")
                        Dim OH As Boolean = dsExp.Tables(0).Rows(0).Item("OH")
                        Dim EstCmpltDt As String = dsExp.Tables(0).Rows(0).Item("EstCmpltDt")
                        Dim EstSpendDt As String = dsExp.Tables(0).Rows(0).Item("EstSpendDt")
                        Dim EstEndSpendDt As String = dsExp.Tables(0).Rows(0).Item("EstEndSpendDt")
                        Dim EstRecoveryDt As String = dsExp.Tables(0).Rows(0).Item("EstRecoveryDt")
                        Dim NotRequired As String = IIf(dsExp.Tables(0).Rows(0).Item("NotRequired") = True, "Yes", "No")
                        Dim Payback As Decimal = dsExp.Tables(0).Rows(0).Item("PaybackInYears").ToString()
                        Dim RtnAvgAssets As Decimal = dsExp.Tables(0).Rows(0).Item("ReturnAvgAssets").ToString()
                        Dim DiscountReturned As Decimal = dsExp.Tables(0).Rows(0).Item("DiscountReturned").ToString()
                        Dim UGNTotalCost As Decimal = Format(dsExp.Tables(0).Rows(0).Item("UGNTotalCost"), "#,##0.00")
                        Dim CustTotalCost As Decimal = Format(dsExp.Tables(0).Rows(0).Item("CustTotalCost"), "#,##0.00")
                        Dim VarTotalCost As Decimal = Format(dsExp.Tables(0).Rows(0).Item("VarTotalCost"), "#,##0.00")
                        Dim AmtToBeRecovered As Decimal = Format(dsExp.Tables(0).Rows(0).Item("AmtToBeRecovered"), "#,##0.00")
                        Dim MPAAmtToBeRecovered As Decimal = Format(dsExp.Tables(0).Rows(0).Item("MPA_AmtToBeRecovered"), "#,##0.00")
                        Dim MPATotalCost As Decimal = Format(dsExp.Tables(0).Rows(0).Item("MPA_TotalCost"), "#,##0.00")

                        Dim UGNLocation As String = Nothing
                        If (UT = True) Then
                            UGNLocation = "Tinley Park, "
                        ElseIf (UN = True) Then
                            UGNLocation &= IIf(UGNLocation = Nothing, "Chicago Heights, IL", "; Chicago Heights, IL")
                        ElseIf (UP = True) Then
                            UGNLocation &= IIf(UGNLocation = Nothing, "Jackson, TN", "; Jackson, TN")
                        ElseIf (UR = True) Then
                            UGNLocation &= IIf(UGNLocation = Nothing, "Somerset, KY", "; Somerset, KY")
                        ElseIf (US = True) Then
                            UGNLocation &= IIf(UGNLocation = Nothing, "Valparaiso, IN", "; Valparaiso, IN")
                        ElseIf (UW = True) Then
                            UGNLocation &= IIf(UGNLocation = Nothing, "Silao, MX", "; Silao, MX")
                        ElseIf (OH = True) Then
                            UGNLocation &= IIf(UGNLocation = Nothing, "Monroe, OH", "; Monroe, OH")
                        End If

                        Dim SeqNo As Integer = 0
                        Dim NextSeqNo As Integer = 0
                        Dim NextLvl As Integer = 0

                        Select Case hfSeqNo.Value
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
                        End Select

                        If SeqNo = 3 Then
                            NextLvl = 15
                        End If

                        ''**********************************************************************
                        ''Check for same level records Rejected. IF so, cancel approval process.
                        ''**********************************************************************
                        If ddStatus.SelectedValue <> "Pending" Then
                            ''***********************************
                            ''Update Current Level Approver record.
                            ''***********************************
                            EXPModule.UpdateExpProjPackagingApproval(ViewState("pProjNo"), DefaultTMID, True, ddStatus.SelectedValue, txtComments.Text, SeqNo, DefaultUser, DefaultDate)

                            ''*****************
                            ''Level Completed
                            ''*****************
                            ds1st = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), SeqNo, 0, True, False)
                            If commonFunctions.CheckDataSet(ds1st) = True Then
                                For i = 0 To ds1st.Tables.Item(0).Rows.Count - 1
                                    If (ds1st.Tables(0).Rows(i).Item("SubmitFlag") = True) Then
                                        LvlApvlCmplt = True
                                    End If
                                Next
                            Else
                                LvlApvlCmplt = True
                            End If

                        End If

                        ''************************
                        ''* Update Packaging record
                        '*************************
                        If SeqNo = 3 Then 'Last Team Member
                            EXPModule.UpdateExpProjPackagingStatus(ViewState("pProjNo"), IIf(ddStatus.SelectedValue = "Rejected", "In Process", "Approved"), IIf(ddStatus.SelectedValue = "Rejected", "R", "A"), DefaultUser, DefaultDate)
                        Else 'Not the Last Team Member
                            EXPModule.UpdateExpProjPackagingStatus(ViewState("pProjNo"), "In Process", IIf(ddStatus.SelectedValue = "Rejected", "R", "T"), DefaultUser, DefaultDate)
                        End If

                        ''**************************************************************
                        ''Locate Next Level Approver(s)
                        ''**************************************************************
                        If LvlApvlCmplt = True Then
                            ''Check at same sequence level
                            ds1st = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), SeqNo, 0, True, False)
                            If commonFunctions.CheckDataSet(ds1st) = False Then
                                If ddStatus.SelectedValue <> "Rejected" Then 'Team Member Approved
                                    ds2nd = EXPModule.GetPackagingExpProjApproval(ViewState("pProjNo"), IIf(SeqNo < 3, (SeqNo + 1), SeqNo), 0, True, False)

                                    If commonFunctions.CheckDataSet(ds2nd) = True Then
                                        For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                            If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                            (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                                EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                                EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                                ''*****************************************
                                                ''Update Next Level Approvers DateNotified field.
                                                ''*****************************************
                                                EXPModule.UpdateExpProjPackagingApproval(ViewState("pProjNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", IIf(SeqNo < 3, (SeqNo + 1), SeqNo), DefaultUser, DefaultDate)
                                            End If
                                        Next
                                    End If 'EOF ds2nd.Tables.Count > 0 
                                End If 'EOF t.SelectedValue <> "Rejected"

                                'Rejected or last approval
                                If ((ddStatus.SelectedValue = "Rejected") Or (SeqNo = 3 And ddStatus.SelectedValue = "Approved")) Then
                                    ''********************************************************
                                    ''Notify Project Lead
                                    ''********************************************************
                                    dsRej = EXPModule.GetExpProjPackagingLead(ViewState("pProjNo"))
                                    ''Check that the recipient(s) is a valid Team Member
                                    If commonFunctions.CheckDataSet(dsRej) = True Then
                                        For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                            If (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                                            (dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                            (dsRej.Tables(0).Rows(i).Item("TMDesc") = "Project Leader") Then

                                                EmailTO &= dsRej.Tables(0).Rows(i).Item("Email") & ";"
                                                EmpName &= dsRej.Tables(0).Rows(i).Item("TMName") & ", "

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
                                dsRej = EXPModule.GetExpProjPackagingLead(ViewState("pProjNo"))
                                ''Check that the recipient(s) is a valid Team Member
                                If commonFunctions.CheckDataSet(dsRej) = True Then
                                    For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                        If (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                                        (dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                        (dsRej.Tables(0).Rows(i).Item("TMDesc") = "Project Leader") Then

                                            EmailTO &= dsRej.Tables(0).Rows(i).Item("Email") & ";"
                                            EmpName &= dsRej.Tables(0).Rows(i).Item("TMName") & ", "

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
                            ''**************************************************************
                            ''Carbon Copy Previous Levels
                            ''**************************************************************
                            If (SeqNo > 1 And ddStatus.SelectedValue <> "Rejected") Then
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

                            If SeqNo = 3 And ddStatus.SelectedValue = "Approved" Then
                                ''********************************
                                ''Carbon Copy Account Manager
                                ''********************************
                                EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                                ''**************************************
                                ''*Carbon Copy the Accounting Department
                                ''**************************************
                                EmailTO = CarbonCopyList(Nothing, 87, "", 0, 0, EmailTO, DefaultTMID)
                                EmailTO = CarbonCopyList(Nothing, 121, "", 0, 0, EmailTO, DefaultTMID)

                                ''********************************
                                ''Carbon Copy Materials Managers
                                ''********************************
                                EmailCC = CarbonCopyList(MyMessage, 122, IIf(UN = True, "UN", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 122, IIf(UP = True, "UP", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 122, IIf(UR = True, "UR", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 122, IIf(US = True, "US", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 122, IIf(UT = True, "UT", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 122, IIf(UW = True, "UW", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 122, IIf(OH = True, "OH", ""), 0, 0, EmailCC, DefaultTMID)

                                ''********************************
                                ''Carbon Copy Ops Manager
                                ''********************************
                                EmailCC = CarbonCopyList(MyMessage, 123, IIf(UN = True, "UN", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 123, IIf(UP = True, "UP", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 123, IIf(UR = True, "UR", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 123, IIf(US = True, "US", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 123, IIf(UT = True, "UT", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 123, IIf(UW = True, "UW", ""), 0, 0, EmailCC, DefaultTMID)
                                EmailCC = CarbonCopyList(MyMessage, 123, IIf(OH = True, "OH", ""), 0, 0, EmailCC, DefaultTMID)

                            ElseIf SeqNo <> 3 And ddStatus.SelectedValue = "Approved" Then
                                ''********************************************************
                                ''Notify Project Lead
                                ''********************************************************
                                EmailTO = CarbonCopyList(Nothing, 0, "", 0, 0, EmailTO, DefaultTMID)

                            End If

                            'Test or Production Message display
                            If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                                MyMessage.Subject = "TEST: "
                                MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                            Else
                                MyMessage.Subject = ""
                                MyMessage.Body = ""
                            End If

                            MyMessage.Subject = "Packaging Expenditure: " & ViewState("pProjNo") & " - " & ProjectTitle
                            MyMessage.Body = "<font size='2' face='Tahoma'>"
                            If ddStatus.SelectedValue = "Rejected" Then
                                MyMessage.Subject &= " - REJECTED"
                                MyMessage.Body &= EmpName
                                MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & ProjectTitle & "' was <font color='red'>REJECTED</font>.  "
                                MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.<br/><br/>"
                                MyMessage.Body &= "<i>Reason for rejection:</i> <b>" & txtComments.Text & "</b></p>"

                            Else
                                If SeqNo = 3 Then 'If last approval
                                    MyMessage.Subject &= "- APPROVED"

                                    MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & ProjectTitle & "' is Approved. "
                                    MyMessage.Body &= " <a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pLS=" & ViewState("pLS") & "'>Click here</a> to access the record.</p>"
                                Else
                                    MyMessage.Body &= EmpName
                                    MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & ProjectTitle & "' is available for your Review/Approval. "
                                    MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crExpProjPackagingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"

                                End If
                            End If
                            MyMessage.Body &= "</font>"

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
                            MyMessage.Body &= "<td class='p_text' align='right'>Project Leader:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & ProjectLeader & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Description:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td style='width: 600px;'>" & ProjDateNotes & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>UGN Location(s):&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td style='width: 600px;'>" & UGNLocation & "</td>"
                            MyMessage.Body &= "</tr>"

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
                            MyMessage.Body &= "             <td>" & EstCmpltDt & "</td>"
                            MyMessage.Body &= "             <td>" & EstSpendDt & "</td>"
                            MyMessage.Body &= "             <td>" & EstEndSpendDt & "</td>"
                            MyMessage.Body &= "             <td>" & EstRecoveryDt & "</td>"
                            MyMessage.Body &= "         </tr>"
                            MyMessage.Body &= "     </table>"
                            MyMessage.Body &= "     </td>"
                            MyMessage.Body &= " </tr>"

                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td colspan='2'>"
                            ''***************************************************
                            ''Get list of Customer/Part information for display
                            ''***************************************************
                            MyMessage.Body &= "<table border='0' style='font-size: 13; font-family: Tahoma;'>"
                            MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
                            MyMessage.Body &= "<td><strong>Customer </strong></td>"
                            MyMessage.Body &= "<td><strong>Program </strong></td>"
                            MyMessage.Body &= "<td><strong>Part Number </strong></td>"
                            MyMessage.Body &= "<td><strong>SOP Date </strong></td>"
                            MyMessage.Body &= "<td><strong>EOP Date </strong></td>"
                            MyMessage.Body &= "<td><strong>Est. PPAP Date </strong></td>"
                            MyMessage.Body &= "</tr>"

                            Dim dsCP As DataSet
                            dsCP = EXPModule.GetExpProjPackagingCustomer(ViewState("pProjNo"), 0)
                            If dsCP.Tables.Count > 0 And (dsCP.Tables.Item(0).Rows.Count > 0) Then
                                For i = 0 To dsCP.Tables.Item(0).Rows.Count - 1
                                    MyMessage.Body &= "<tr style='border-color:white'>"
                                    MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("ddCustomerDesc") & "&nbsp;</td>"
                                    MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("ProgramName") & "&nbsp;</td>"
                                    MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("PartNo") & "&nbsp;</td>"
                                    MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("SOP") & "&nbsp;</td>"
                                    MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("EOP") & "&nbsp;</td>"
                                    MyMessage.Body &= "<td height='25'>" & dsCP.Tables(0).Rows(i).Item("PPAP") & "&nbsp;</td>"
                                    MyMessage.Body &= "</tr>"
                                Next
                            End If
                            MyMessage.Body &= "</Table>"
                            MyMessage.Body &= "</td></tr>"

                            MyMessage.Body &= " <tr>"
                            MyMessage.Body &= "     <td colspan='2'>"
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
                            MyMessage.Body &= "             <td class='p_text' align='right'>UGN Total Cost ($):&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "             <td>" & UGNTotalCost & "</td>"
                            MyMessage.Body &= "             <td class='p_text' align='right'>Total Cost ($):&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "             <td>" & MPATotalCost & "</td>"
                            MyMessage.Body &= "         </tr>"
                            MyMessage.Body &= "         <tr>"
                            MyMessage.Body &= "             <td class='p_text' align='right'>Customer Total Cost ($):&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "             <td colspan='3'>" & CustTotalCost & "</td>"
                            MyMessage.Body &= "         </tr>"
                            MyMessage.Body &= "         <tr>"
                            MyMessage.Body &= "             <td class='p_text' align='right'>Variance ($):&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "             <td colspan='3'>" & VarTotalCost & "</td>"
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
                                    MyMessage.Body &= "<td height='25'><a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/PackagingExpProjDocument.aspx?pProjNo=" & ViewState("pProjNo") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
                                    MyMessage.Body &= "</tr>"
                                Next
                                MyMessage.Body &= "</table>"
                                MyMessage.Body &= "</tr>"
                            End If
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
                            EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, ddStatus.SelectedValue & " " & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing), "", "", "", "")

                            ''*****************
                            ''History Tracking
                            ''*****************
                            If ddStatus.SelectedValue <> "Rejected" Then
                                If SeqNo = 3 Then
                                    EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Notification sent to all involved. ", "", "", "", "")
                                Else
                                    EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Notification sent to level " & IIf(SeqNo < 3, (SeqNo + 1), SeqNo) & " approver(s): " & EmpName, "", "", "", "")
                                End If
                            Else
                                EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Notification sent to " & EmpName, "", "", "", "")
                            End If

                            ''**********************************
                            ''Connect & Send email notification
                            ''**********************************
                            Try
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (P)", ViewState("pProjNo"))
                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."

                            Catch ex As SmtpException
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."

                                UGNErrorTrapping.InsertEmailQueue("Project No:" & ViewState("pProjNo"), CurrentEmpEmail, EmailTO, CurrentEmpEmail & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
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
                            ''*****************
                            ''History Tracking
                            ''*****************
                            EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, ddStatus.SelectedValue & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing), "", "", "", "")

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
            End If 'EOF If ViewState("pProjNo") <> Nothing Then
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
        ReqComments.Visible = False

        '*************************
        '* Get Current User Name
        '*************************
        Dim gtm As DataSet = New DataSet
        gtm = SecurityModule.GetTeamMember(DefaultTMID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        If commonFunctions.CheckDataSet(gtm) = True Then
            DefaultUserFullName = gtm.Tables(0).Rows(0).Item("FirstName") & " " & gtm.Tables(0).Rows(0).Item("LastName")
        End If

        Try
            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pProjNo") <> Nothing Then
                ''*************************************************************************
                ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                ''*************************************************************************
                Dim dsExp As DataSet = New DataSet
                dsExp = EXPModule.GetExpProjPackaging(ViewState("pProjNo"), "", "", "", 0, "", 0, "", "", "")
                If commonFunctions.CheckDataSet(dsExp) = True Then
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
                            NextLvl = 14
                        Case 2
                            SeqNo = 2
                            NextSeqNo = 3
                            NextLvl = 15
                        Case 3
                            SeqNo = 3
                            NextSeqNo = 0
                            NextLvl = 0
                    End Select

                    If SeqNo = 3 Then
                        NextLvl = 15
                    End If

                    ''********************************************************
                    ''Notify Project Leader
                    ''********************************************************
                    dsRej = EXPModule.GetExpProjPackagingLead(ViewState("pProjNo"))
                    ''Check that the recipient(s) is a valid Team Member
                    If commonFunctions.CheckDataSet(dsRej) = True Then
                        For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                            If (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True) And _
                            (dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                            (dsRej.Tables(0).Rows(i).Item("TMDesc") = "Project Leader") Then

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
                        EmailCC = CarbonCopyList(MyMessage, 0, "", SeqNo, 0, EmailCC, DefaultTMID)

                        ''***************************************************************
                        ''Carbon Account Manager/Project Lead
                        ''***************************************************************
                        EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)

                        'Test or Production Message display
                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                        End If

                        MyMessage.Subject &= "Packaging Expenditure: " & ViewState("pProjNo") & " - " & ProjectTitle & " - MESSAGE RECIEVED"

                        MyMessage.Body = "<table style='font-size: 13; font-family: Tahoma;'>"
                        MyMessage.Body &= " <tr>"
                        MyMessage.Body &= "     <td valign='top' width='20%'>"
                        MyMessage.Body &= "         <img src='" & ViewState("strProdOrTestEnvironment") & "/images/messanger60.jpg'/>"
                        MyMessage.Body &= "     </td>"
                        MyMessage.Body &= "     <td valign='top'>"
                        MyMessage.Body &= "             <b>" & EmpName & "</b> "
                        MyMessage.Body &= "             <p><b>" & DefaultUserFullName & "</b> sent a message regarding "
                        MyMessage.Body &= "             <font color='red'>" & ViewState("pProjNo") & " - " & ProjectTitle & "</font>."
                        MyMessage.Body &= "             <br/><br/><i>Question:&nbsp;&nbsp;</i><b>" & txtQC.Text & "</b>"
                        MyMessage.Body &= "             </p>"
                        MyMessage.Body &= "             <p><a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pRC=1" & "'>Click here</a> to respond."
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
                        EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Message Sent", "", "", "", "")

                        ''*****************
                        ''Save Message
                        ''*****************
                        EXPModule.InsertExpProjPackagingRSS(ViewState("pProjNo"), ProjectTitle, DefaultTMID, SeqNo, txtQC.Text)

                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Spending Request (P)", ViewState("pProjNo"))
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

    Protected Sub btnReset2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset2.Click
        Response.Redirect("crExpProjPackagingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1", False)
    End Sub 'EOF btnReset2_Click

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
                        (RejectedTMID <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) And _
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

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Try
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