' ************************************************************************************************
' Name:	crSupplierRequestApproval.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from Supplier_Request table
'           and other sources.  Also used as an approval screen to allow users to view current
'           info related to a supplier request and approve/reject the project in one screen.
' Date		    Author	    
' 09/24/2010    LRey			Created .Net application
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class SUP_crViewSupplierRequest
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        ViewState("pSUPNo") = HttpContext.Current.Request.QueryString("pSUPNo")
        Master.Page.Header.Title = "UGN, Inc.: Supplier Request #" & ViewState("pSUPNo") & " - Approval"

        Dim m As ASP.crviewmasterpage_master = Master
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            If ViewState("pSUPNo") = Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='SupplierRequestList.aspx'><b>Supplier Request Search</b></a> > Supplier Request Preview"
            Else
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='SupplierRequestList.aspx?pSUPNo=" & ViewState("pSUPNo") & "&pAprv=1" & "'><b>Supplier Request Search</b></a> > <a href='SupplierRequest.aspx?pSUPNo=" & ViewState("pSUPNo") & "&pAprv=1" & "'><b>Supplier Request</b></a> > Approval"
            End If
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

        ''*************************************************
        '' "Form Level Security using Roles &/or Subscriptions"
        ''*************************************************
        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

        ''************************************************************
        ''Code Below counts the number of chars used in comments area
        ''************************************************************
        txtComments.Attributes.Add("onkeypress", "return tbLimit();")
        txtComments.Attributes.Add("onkeyup", "return tbCount(" + lblComments.ClientID + ");")
        txtComments.Attributes.Add("maxLength", "200")

        ''*********
        ''Get Data
        ''*********
        If Not Page.IsPostBack Then
            If ViewState("pSUPNo") <> "" Then
                BindData()
            End If
        End If
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

            ''*********************************************************
            ''If Record is Void, do not allow Team Memember submission
            ''*********************************************************
            Dim dsExp As DataSet = New DataSet
            Dim ProjectStatus As String = Nothing
            ViewState("pProjStat") = Nothing
            dsExp = SUPModule.GetSupplierRequest(ViewState("pSUPNo"))
            If commonFunctions.CheckDataSet(dsExp) = True Then '(dsExp.Tables.Item(0).Rows.Count > 0) Then
                ProjectStatus = dsExp.Tables(0).Rows(0).Item("RecStatus").ToString()
                ViewState("pProjStat") = ProjectStatus
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim dsCorpAcct As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 110 'Supplier Request Form ID
            Dim iRoleID As Integer = 0
            Dim iCorpAcctTMID As Integer = 0 'Used to locate Corporate Accounting 

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Janie.Thompson", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    ViewState("UGNDB_TMID") = iTeamMemberID
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

                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                For i = 0 To dsRoleForm.Tables(0).Rows.Count - 1
                                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                                    Select Case iRoleID
                                        Case 11 '*** UGNAdmin: Full Access
                                            ViewState("ObjectRole") = True
                                            ViewState("Admin") = True
                                            lblInBPCS.Visible = True
                                            lblVendorNo.Visible = True
                                            rfvVendorNo.Enabled = True
                                            rfvInBPCS.Enabled = True
                                            If ddInBPCS.SelectedValue = Nothing Or txtVendorNo.Text = Nothing Then
                                                btnSubmit.Enabled = True
                                                btnReset.Enabled = True
                                            End If
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
                                            ElseIf lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing And (ProjectStatus <> "Void") Then
                                                btnSubmit.Enabled = True
                                                btnReset.Enabled = True
                                            End If
                                            If iCorpAcctTMID = iTeamMemberID Then 'Then ' 'Only the current team member that matches will have access to edit fields
                                                ViewState("Admin") = True
                                                lblInBPCS.Visible = True
                                                lblVendorNo.Visible = True
                                                rfvVendorNo.Enabled = True
                                                rfvInBPCS.Enabled = True
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
                                Next 'EOF For next
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
        ViewState("pSUPNo") = HttpContext.Current.Request.QueryString("pSUPNo")
        Dim oRpt As New ReportDocument()
        Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
        Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
        Dim dbConn As New TableLogOnInfo()

        If ViewState("pSUPNo") <> "" Then
            Try

                If Session("TempCrystalRptFiles") Is Nothing Then
                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "crSupplierRequest.rpt")

                    'getting the database, the table and the LogOnInfo object which holds login onformation
                    crDatabase = oRpt.Database

                    'getting the table in an object array of one item 
                    Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
                    crDatabase.Tables.CopyTo(arrTables, 0)
                    ' assigning the first item of array to crTable by downcasting the object to Table 
                    crTable = arrTables(0)

                    ' setting values 
                    Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                    Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                    Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString
                    Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

                    ' defining report source 
                    CrystalReportViewer1.DisplayGroupTree = False
                    CrystalReportViewer1.ReportSource = oRpt
                    Session("TempCrystalRptFiles") = oRpt

                    'Check if there are parameters or not in report.
                    Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count
                    'setReportParameters()
                    oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                    oRpt.SetParameterValue("@SUPNo", ViewState("pSUPNo"))
                    'oRpt.SetParameterValue("@RequestedByTMID", Nothing)
                    'oRpt.SetParameterValue("@VendorName", Nothing)
                    'oRpt.SetParameterValue("@ProdDesc", Nothing)
                    'oRpt.SetParameterValue("@VendorType", Nothing)
                    'oRpt.SetParameterValue("@VTypeDesc", Nothing)
                    'oRpt.SetParameterValue("@UGNFacility", Nothing)
                    'oRpt.SetParameterValue("@RecStatus", Nothing)
                    'oRpt.SetParameterValue("@RoutingStatus", Nothing)
                    'oRpt.SetParameterValue("@VendorNo", Nothing)
                    oRpt.SetParameterValue("@URLLocation", strProdOrTestEnvironment)
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
    End Sub 'EOF Page_Unload

    Public Sub BindData()
        Try
            btnSubmit.Enabled = False
            btnReset.Enabled = False
            Dim i As Integer = 0
            Dim ds As DataSet = New DataSet
            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                commonFunctions.SetUGNDBUser()
            End If

            Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")

            ds = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), 0, DefaultTMID, False, False)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                For i = 0 To ds.Tables(0).Rows.Count - 1

                    If (ds.Tables(0).Rows(i).Item("DateSigned").ToString() = Nothing _
                        Or ds.Tables(0).Rows(i).Item("DateSigned").ToString() = "") _
                        And ds.Tables(0).Rows(i).Item("DateNotified").ToString() <> Nothing _
                        And ViewState("ObjectRole") = True Then

                        lblTeamMbr.Text = ds.Tables(0).Rows(i).Item("TeamMemberName").ToString()
                        lblDateNotified.Text = ds.Tables(0).Rows(i).Item("DateNotified").ToString()
                        ddStatus.SelectedValue = ds.Tables(0).Rows(i).Item("Status").ToString()
                        txtComments.Text = ds.Tables(0).Rows(i).Item("Comments").ToString()
                        hfSeqNo.Value = ds.Tables(0).Rows(i).Item("SeqNo").ToString()

                        If ViewState("pProjStat") <> "Void" Then
                            btnSubmit.Enabled = True
                            btnReset.Enabled = True
                        End If
                    End If
                Next
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
        Response.Redirect("crSupplierRequestApproval.aspx?pSUPNo=" & ViewState("pSUPNo"), False)
    End Sub 'EOF btnReset_Click

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        ''********
        ''* This function is used to submit email next level Approvers or to originators when rejected
        ''********
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
        Dim DefaultDate As Date = Date.Now
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")


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
            ''*********
            ''* Make sure Required Fields are entered
            ''*********
            Dim ReqFieldsAval As Boolean = False
            If ViewState("Admin") = True And ddStatus.SelectedValue = "Approved" Then
                If ddInBPCS.SelectedValue <> "" And txtVendorNo.Text <> "" Then
                    ReqFieldsAval = True
                Else
                    ReqFieldsAval = False
                    sDetail.Visible = True
                    btnReset.Enabled = True
                End If
            Else
                ReqFieldsAval = True
                sDetail.Visible = False
            End If

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And (ReqFieldsAval = True) And ViewState("pSUPNo") <> Nothing Then
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
                    dsExp = SUPModule.GetSupplierRequest(ViewState("pSUPNo"))
                    If (dsExp.Tables.Item(0).Rows.Count > 0) Then
                        ''**********************
                        ''*Initialize Variables
                        ''**********************
                        Dim NewVendor As Boolean = dsExp.Tables(0).Rows(0).Item("NewVendor")
                        Dim ChangeVendor As Boolean = dsExp.Tables(0).Rows(0).Item("ChangeToCurrentVendor")
                        Dim VendorName As String = dsExp.Tables(0).Rows(0).Item("VendorName")
                        Dim ProductDescription As String = dsExp.Tables(0).Rows(0).Item("ProductDescription")
                        Dim RequestedByTMID As Integer = dsExp.Tables(0).Rows(0).Item("RequestedByTMID")
                        Dim RequestedBy As String = dsExp.Tables(0).Rows(0).Item("RequestedByName")
                        Dim VendorType As String = dsExp.Tables(0).Rows(0).Item("VendorType")
                        Dim VTypeName As String = dsExp.Tables(0).Rows(0).Item("VTypeName")
                        Dim ReasonForAddition As String = dsExp.Tables(0).Rows(0).Item("ReasonForAddition")
                        Dim VendorNumber As Integer = 0
                        If Not IsDBNull(dsExp.Tables(0).Rows(0).Item("VendorNo")) Then
                            VendorNumber = dsExp.Tables(0).Rows(0).Item("VendorNo")
                        End If
                        Dim Ten99 As Boolean = dsExp.Tables(0).Rows(0).Item("Ten99")
                        Dim InBPCS As Boolean = dsExp.Tables(0).Rows(0).Item("InBPCS")
                        Dim UT As Boolean = dsExp.Tables(0).Rows(0).Item("UT")
                        Dim UN As Boolean = dsExp.Tables(0).Rows(0).Item("UN")
                        Dim UP As Boolean = dsExp.Tables(0).Rows(0).Item("UP")
                        Dim UR As Boolean = dsExp.Tables(0).Rows(0).Item("UR")
                        Dim US As Boolean = dsExp.Tables(0).Rows(0).Item("US")
                        Dim UW As Boolean = dsExp.Tables(0).Rows(0).Item("UW")
                        Dim OH As Boolean = dsExp.Tables(0).Rows(0).Item("OH")

                        Dim SeqNo As Integer = hfSeqNo.Value

                        ''**********************************************************************
                        ''Check for same level records Rejected. IF so, cancel approval process.
                        ''**********************************************************************
                        If ddStatus.SelectedValue <> "Pending" Then
                            ''***********************************
                            ''Update Current Level Approver record.
                            ''***********************************
                            SUPModule.UpdateSupplierRequestApproval(ViewState("pSUPNo"), DefaultTMID, True, ddStatus.SelectedValue, txtComments.Text, SeqNo, 0, DefaultUser, DefaultDate)

                            ''*****************
                            ''Level Completed
                            ''*****************
                            ds1st = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), SeqNo, 0, False, True)
                            'Locate any Rejected
                            If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                'do nothing
                            Else
                                ds1st = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), SeqNo, 0, True, False)
                                'Located any Pending
                                If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                    'do nothing
                                Else 'otherwise all are approved
                                    LvlApvlCmplt = True
                                End If
                            End If
                        End If

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
                                If totalApprovers <= hfSeqNo.Value Then
                                    LastSeqNo = True
                                Else
                                    LastSeqNo = False
                                End If
                            Next
                        End If

                        ''************************
                        ''* Update Supplier_Request record
                        '*************************
                        SUPModule.UpdateSupplierRequestStatus(ViewState("pSUPNo"), IIf(ddStatus.SelectedValue = "Rejected", "In Process", IIf(LastSeqNo = True, "Approved", "In Process")), IIf(ddStatus.SelectedValue = "Rejected", "R", IIf(LastSeqNo = True, "A", "T")), IIf(ddInBPCS.SelectedValue = Nothing, False, ddInBPCS.SelectedValue), IIf(cbTen99.Checked = False, False, True), IIf(txtVendorNo.Text = Nothing, 0, txtVendorNo.Text), DefaultUser, DefaultDate)

                        ''**************************************************************
                        ''Locate Next Level Approver(s)
                        ''**************************************************************
                        If LvlApvlCmplt = True Then
                            ''Check at same sequence level
                            ds1st = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), SeqNo, 0, True, False)
                            If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                ''Do not send email at same level twice.
                            Else
                                If ddStatus.SelectedValue <> "Rejected" Then 'Team Member Approved
                                    ds2nd = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), IIf(LastSeqNo = False, (SeqNo + 1), SeqNo), 0, True, False)
                                    If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                                        For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                            '  If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                            '(ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                            If (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                                EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                                EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                                ''*****************************************
                                                ''Update Next Level Approvers DateNotified field.
                                                ''*****************************************
                                                SUPModule.UpdateSupplierRequestApproval(ViewState("pSUPNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", IIf(LastSeqNo = False, (SeqNo + 1), SeqNo), 0, DefaultUser, DefaultDate)

                                            End If
                                        Next
                                    End If 'EOF ddStatus.SelectedValue <> "Rejected"
                                End If 'EOF If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0)
                            End If 'EOF ds1st.Tables.Count > 0
                        End If '' EOF If LvlApvlCmplt is false

                        'Rejected or last approval
                        If ddStatus.SelectedValue = "Rejected" Or (LastSeqNo = True And ddStatus.SelectedValue = "Approved") Then
                            ''********************************************************
                            ''Notify Requestor if Rejected or last approval
                            ''********************************************************
                            dsRej = SecurityModule.GetTeamMember(RequestedByTMID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
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

                        ''********************************************************
                        ''Send Notification only if there is a valid Email Address
                        ''********************************************************
                        If EmailTO <> Nothing Then
                            Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                            Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                            Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                            ''******************************************************
                            ''Carbon Copy Previous approvers
                            ''*******************************************************
                            If LastSeqNo = True And ddStatus.SelectedValue = "Approved" Then
                                ds2nd = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), 0, 0, False, False)
                            Else
                                ds2nd = SUPModule.GetSupplierRequestApproval(ViewState("pSUPNo"), SeqNo, 0, False, False)
                            End If
                            If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                                For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                    If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                    (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                        EmailCC &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                    End If
                                Next
                            End If 'EOF ds2nd.Tab

                            If LastSeqNo = True And ddStatus.SelectedValue = "Approved" Then
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
                                        End If 'EOF If (dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                    Next
                                End If 'EOF CC All Involved

                                If VendorType = "INVU" Then
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

                                If VendorNumber <> Nothing Or txtVendorNo.Text <> Nothing Then
                                    EmailCC &= "Vendorap@ugnauto.com" & ";"
                                End If

                            End If 'EOF  If LastSeqNo = True And ddStatus.SelectedValue = "Approved" Then

                            ''Test or Production Message display
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                            If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                                MyMessage.Subject = "TEST: "
                                MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                            End If

                            MyMessage.Subject &= "Supplier Request - " & VendorName

                            If ddStatus.SelectedValue = "Rejected" Then
                                MyMessage.Subject &= " - REJECTED"
                                MyMessage.Body = EmpName
                                MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & VendorName & "' was <font color='red'>REJECTED</font> back to the initiator. <br/><br/>Reason for rejection: " & txtComments.Text & "<br/><br/>"
                                MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/SUP/SupplierRequest.aspx?pSUPNo=" & ViewState("pSUPNo") & "'>Click here</a> to access the record.</p>"
                            Else
                                If LastSeqNo = True Then
                                    MyMessage.Subject &= " - COMPLETED"
                                    MyMessage.Body &= "<p>" & ViewState("pProjNo") & " '" & VendorName & "' is Completed by all. "
                                    MyMessage.Body &= " <a href='" & ViewState("strProdOrTestEnvironment") & "/SUP/SupplierRequest.aspx?pSUPNo=" & ViewState("pSUPNo") & "'>Click here</a> to access the record.</p>"
                                Else
                                    MyMessage.Body &= EmpName
                                    MyMessage.Body &= "<p>'" & VendorName & "' is available for your Review/Approval. "
                                    MyMessage.Body &= "<a href='" & ViewState("strProdOrTestEnvironment") & "/SUP/crSupplierRequestApproval.aspx?pSUPNo=" & ViewState("pSUPNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"
                                End If
                            End If

                            MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 850px'>"
                            MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>SUPPLIER OVERVIEW</strong></td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right' valign='top'>&nbsp;</td>"
                            MyMessage.Body &= "<td style='width: 600px;'>" & IIf(NewVendor = True, "[X] New Vendor", "[ ] New Vendor") & " " & IIf(ChangeVendor = True, "[X] Change to current vendor", "[  ] Change to current vendor") & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Requestor:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & RequestedBy & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Reference No:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & ViewState("pSUPNo") & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Supplier Name:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & VendorName & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Product Description:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & ProductDescription & "</td>"
                            MyMessage.Body &= "</tr>"

                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right' valign='top'>UGN Location:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td style='width: 600px;'>" & IIf(UT = True, "[X] Tinley Park, IL", "[  ] Tinley Park, IL") & " " & IIf(UN = True, "[X] Chicago Heights, IL", "[  ] Chicago Heights, IL") & " " & IIf(UP = True, "[X] Jackson, TN", "[  ] Jackson, TN") & " " & IIf(UR = True, "[X] Somerset, KY", "[  ] Somerset, KY") & " " & IIf(US = True, "[X] Valparaiso, IN", "[  ] Valparaiso, IN") & " " & IIf(OH = True, "[X] Monroe, OH", "[  ] Monroe, OH") & " " & IIf(UW = True, "[X] Silao, MX", "[  ] Silao, MX") & "</td>"
                            MyMessage.Body &= "</tr>"

                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Vendor Type:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & VTypeName & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Reason for New Supplier Addition:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & ReasonForAddition & "</td>"
                            MyMessage.Body &= "</tr>"

                            If (((ViewState("Admin") = True) And (ReqFieldsAval = True) And ddStatus.SelectedValue = "Approved")) Or (LastSeqNo = True) Then
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Supplier Created in Oracle:&nbsp;&nbsp; </td>"
                                If InBPCS = False Then
                                    MyMessage.Body &= "<td>" & IIf(ddInBPCS.SelectedValue = False, "[  ] Yes   [ X ] No", "[ X ] Yes   [   ] No") & "</td>"
                                Else
                                    MyMessage.Body &= "<td>" & IIf(InBPCS = False, "[  ] Yes   [ X ] No", "[ X ] Yes   [   ] No") & "</td>"
                                End If

                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>1099?:&nbsp;&nbsp; </td>"
                                If Ten99 = False Then
                                    MyMessage.Body &= "<td>" & IIf(cbTen99.Checked = False, "[  ] Yes   [ X ] No", "[ X ] Yes   [   ] No") & "</td>"
                                Else
                                    MyMessage.Body &= "<td>" & IIf(Ten99 = False, "[  ] Yes   [ X ] No", "[ X ] Yes   [   ] No") & "</td>"
                                End If
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Supplier No Assigned:&nbsp;&nbsp; </td>"
                                If VendorNumber = Nothing Then
                                    MyMessage.Body &= "<td>" & txtVendorNo.Text & "</td>"
                                Else
                                    MyMessage.Body &= "<td>" & VendorNumber & "</td>"
                                End If

                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                            End If

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
                                MyMessage.Body &= "<table>"
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
                            SUPModule.InsertSupplierRequestHistory(ViewState("pSUPNo"), VendorName, DefaultTMID, ddStatus.SelectedValue & " " & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing))

                            ''*****************
                            ''History Tracking
                            ''*****************
                            If ddStatus.SelectedValue <> "Rejected" Then
                                If LastSeqNo = True Then
                                    SUPModule.InsertSupplierRequestHistory(ViewState("pSUPNo"), VendorName, DefaultTMID, "Notification sent to all involved. ")
                                Else
                                    SUPModule.InsertSupplierRequestHistory(ViewState("pSUPNo"), VendorName, DefaultTMID, "Notification sent to level " & (SeqNo + 1) & " approver(s): " & EmpName)
                                End If
                            Else
                                SUPModule.InsertSupplierRequestHistory(ViewState("pSUPNo"), VendorName, DefaultTMID, "Notification sent to " & EmpName)
                            End If

                            ''**********************************
                            ''Connect & Send email notification
                            ''**********************************
                            Try
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Supplier Request", ViewState("pSUPNo"))
                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                            Catch ex As Exception
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                UGNErrorTrapping.InsertEmailQueue("Supplier Request No:" & ViewState("pSUPNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
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
                            If ddStatus.SelectedValue <> "Pending" Then
                                ''*****************
                                ''History Tracking
                                ''*****************
                                SUPModule.InsertSupplierRequestHistory(ViewState("pSUPNo"), VendorName, DefaultTMID, ddStatus.SelectedValue & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing))

                                ''**********************************
                                ''Rebind the data to the form
                                ''********************************** 
                                BindData()
                                lblErrors.Text = "Your response was submitted successfully."
                                lblErrors.Visible = True
                            Else
                                lblErrors.Text = "Action Cancelled. Please select a status Approved or Rejected."
                                lblErrors.Visible = True
                            End If
                        End If
                    End If
                End If
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
    End Sub 'EOF btnSubmit_Click

End Class