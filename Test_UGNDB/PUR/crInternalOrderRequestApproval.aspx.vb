' ************************************************************************************************
' Name:	crInternalOrderRequestApproval.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from ExpProj_Assets table
'           and other sources.  Also used as an approval screen to allow users to view current
'           info related to a capex asset project and approve/reject the project in one screen.
'
' Date		    Author	    
' 08/23/2010    LRey			Created .Net application
' 02/29/2012    LRey            Added new Net Mail method
' 07/19/2012	LRey	        Changed the data type to PONo from int to varchar to allow
'								Buyer's to type in PCARD when it doesn't required a PONo
' 04/26/2013    LRey            Added POinPesos to the body of the email
' 05/10/2013    LREy            Added Customer Owned Tooling Revision Level and gridview access for update
' 06/27/2013    LRey            Modified to include workflow for Buyer approvals only
' 07/10/2013    LRey            Fixed the update approval parameter for TMID
' ************************************************************************************************
#Region "Directives"
Imports System.Net.Mail
Imports System.Threading
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
#End Region

Partial Class IOR_crInternalOrderRequestApproval
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''Used to identify the email address of current user
        ViewState("UGNDB_User_Email") = ""


        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        ViewState("pIORNo") = HttpContext.Current.Request.QueryString("pIORNo")
        ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")

        Dim m As ASP.crviewmasterpage_master = Master
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            If ViewState("pIORNo") = Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='InternalOrderRequestList.aspx'><b>Internal Order Request Search</b></a> > Internal Order Request Preview"
            Else
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='InternalOrderRequestList.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'><b>Internal Order Request Search</b></a> > <a href='InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'><b>Internal Order Request</b></a> > Approval"
            End If
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
            Master.Page.Header.Title = "IOR # " & ViewState("pIORNo") & " - Approval"
        End If


        ''************************************************************
        ''Code Below counts the number of chars used in comments area
        ''************************************************************
        txtComments.Attributes.Add("onkeypress", "return tbLimit();")
        txtComments.Attributes.Add("onkeyup", "return tbCount(" + lblCommentsChar.ClientID + ");")
        txtComments.Attributes.Add("maxLength", "200")

        txtQC.Attributes.Add("onkeypress", "return tbLimit();")
        txtQC.Attributes.Add("onkeyup", "return tbCount(" + lblQCChar.ClientID + ");")
        txtQC.Attributes.Add("maxLength", "200")

        ''*************************************************
        '' "Form Level Security using Roles &/or Subscriptions"
        ''*************************************************
        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

        ''*********
        ''Get Data
        ''*********
        If Not Page.IsPostBack Then
            If ViewState("pIORNo") <> "" Then
                BindData()
                CheckRights()
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
            ViewState("ObjectRole") = False
            ViewState("COTRole") = False
            btnSubmit.Enabled = False
            btnReset.Enabled = False
            lblReqPONo.Visible = False
            lblPONo.Visible = False
            rfvPONo.Enabled = False
            txtPONo.Visible = False

            ''*********************************************************
            ''If Record is Void, do not allow Team Memember submission
            ''*********************************************************
            Dim dsExp As DataSet = New DataSet
            Dim ProjectStatus As String = Nothing
            ViewState("pProjStat") = Nothing
            dsExp = PURModule.GetInternalOrderRequest(ViewState("pIORNo"))
            If commonFunctions.CheckDataSet(dsExp) = True Then '(dsExp.Tables.Item(0).Rows.Count > 0) Then
                ProjectStatus = dsExp.Tables(0).Rows(0).Item("IORStatus").ToString()
                ViewState("pProjStat") = ProjectStatus
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iTMEmail As String = ""
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 109 'Internal Order Request Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            'dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Tuesday.Powers", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iTMEmail = dsTeamMember.Tables(0).Rows(0).Item("Email")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")

                    ViewState("UGNDB_User_Email") = iTMEmail
                    ViewState("UGNDB_TMID") = iTeamMemberID

                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member

                        ''Locate the Buyer to grant access to Purchase Order # field
                        Dim dsBuyer As DataSet = New DataSet
                        dsBuyer = commonFunctions.GetTeamMemberBySubscription(99)
                        Dim iBuyerID As Integer = 0
                        Dim b As Integer = 0
                        ViewState("iBuyerID") = 0
                        If (dsBuyer.Tables.Item(0).Rows.Count > 0) Then
                            For b = 0 To dsBuyer.Tables(0).Rows.Count - 1
                                If dsBuyer.Tables(0).Rows(b).Item("TMID") = iTeamMemberID Then
                                    iBuyerID = dsBuyer.Tables(0).Rows(b).Item("TMID")
                                    ViewState("iBuyerID") = iBuyerID
                                End If
                            Next
                        End If

                        ''Locate the Program Manager to grant access to change Customer Owned Tooling Revision Level
                        Dim pProjNo As String = Nothing
                        If HttpContext.Current.Request.QueryString("pProjNo") <> Nothing _
                         And HttpContext.Current.Request.QueryString("pProjNo") <> "" Then
                            pProjNo = HttpContext.Current.Request.QueryString("pProjNo")
                        End If
                        If pProjNo <> Nothing Then
                            If pProjNo.Substring(0, 1) = "T" Then
                                ViewState("COTRole") = True
                                Dim ds1 As DataSet = New DataSet
                                Dim i As Integer = 0
                                ViewState("iProjMgrID") = 0
                                ds1 = EXPModule.GetExpProjToolingLead(ViewState("pProjNo"))
                                If commonFunctions.CheckDataSet(ds1) = True Then
                                    For i = 0 To ds1.Tables.Item(0).Rows.Count - 1
                                        If ds1.Tables(0).Rows(i).Item("TMDesc") = "Program Manager" _
                                        And iTeamMemberID = ds1.Tables(0).Rows(i).Item("TMID") Then
                                            ViewState("iProjMgrID") = ds1.Tables(0).Rows(i).Item("TMID")
                                        End If
                                    Next
                                End If
                            End If
                        End If

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
                                            If iBuyerID <> 0 Then
                                                ViewState("ObjectRole") = True
                                                lblReqPONo.Visible = True
                                                lblPONo.Visible = True
                                                rfvPONo.Enabled = True
                                                txtPONo.Visible = True
                                                sDetail.Enabled = True
                                            End If
                                        ElseIf lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing And (ProjectStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                            sDetail.Enabled = False
                                        ElseIf lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Approved" And (ProjectStatus <> "Void") Then
                                            If iBuyerID <> 0 Then
                                                ViewState("ObjectRole") = True
                                                lblPONo.Visible = True
                                                txtPONo.Visible = True
                                            End If
                                        End If
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                                        If lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Pending" And (ProjectStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                            If iBuyerID <> 0 Then
                                                ViewState("ObjectRole") = True
                                                lblReqPONo.Visible = True
                                                lblPONo.Visible = True
                                                rfvPONo.Enabled = True
                                                txtPONo.Visible = True
                                                sDetail.Enabled = True
                                            End If

                                        ElseIf lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Approved" And (ProjectStatus <> "Void") Then
                                            If iBuyerID <> 0 Then
                                                lblPONo.Visible = True
                                                txtPONo.Visible = True
                                            End If
                                        End If
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        If lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Pending" And (ProjectStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                            If iBuyerID <> 0 Then
                                                ViewState("ObjectRole") = True
                                                lblReqPONo.Visible = True
                                                lblPONo.Visible = True
                                                rfvPONo.Enabled = True
                                                txtPONo.Visible = True
                                                sDetail.Enabled = True
                                            End If
                                        ElseIf lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing And (ProjectStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                            sDetail.Enabled = False
                                        ElseIf lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Approved" And (ProjectStatus <> "Void") Then
                                            If iBuyerID <> 0 Then
                                                ViewState("ObjectRole") = True
                                                lblPONo.Visible = True
                                                txtPONo.Visible = True
                                            End If
                                        End If
                                        If ViewState("iProjMgrID") <> 0 Then
                                            gvExpProjToolingCustomer.Columns(3).Visible = True
                                        Else
                                            gvExpProjToolingCustomer.Columns(3).Visible = False
                                        End If

                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
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
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
#End Region 'EOF Form Level Security

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState("pIORNo") = HttpContext.Current.Request.QueryString("pIORNo")
        Dim oRpt As New ReportDocument()
        Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
        Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
        Dim dbConn As New TableLogOnInfo()

        If ViewState("pIORNo") <> "" Then
            Try
                CheckRights()

                If Session("TempCrystalRptFiles") Is Nothing Then
                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "crInternalOrderRequest.rpt")

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
                    oRpt.SetParameterValue("@IORNo", ViewState("pIORNo"))
                    oRpt.SetParameterValue("BuyerTMID", ViewState("iBuyerID"))
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
    End Sub 'EOF Page_Unload 188 371 510 569

    Public Sub BindData()
        Try
            Dim ds As DataSet = New DataSet
            Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")

            ds = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), 0, DefaultTMID, False, False) ''
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                lblTeamMbr.Text = ds.Tables(0).Rows(0).Item("TeamMemberName").ToString()
                lblDateNotified.Text = ds.Tables(0).Rows(0).Item("DateNotified").ToString()
                ddStatus.SelectedValue = ds.Tables(0).Rows(0).Item("Status").ToString()
                txtComments.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                hfSeqNo.Value = ds.Tables(0).Rows(0).Item("SeqNo").ToString()
                txtPONo.Text = ds.Tables(0).Rows(0).Item("PONo").ToString()

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("AppropriationCode").ToString()) Then
                    Dim ProjNo As String = ds.Tables(0).Rows(0).Item("AppropriationCode").ToString()
                    Dim ProjectTitle As String = Nothing
                    Dim ds2 As DataSet = New DataSet
                    ds2 = PURModule.GetInternalOrderRequestCapEx(ViewState("pIORNo"), "")
                    If commonFunctions.CheckDataSet(ds2) = True Then
                        ProjectTitle = ds2.Tables(0).Rows(0).Item("ProjectTitle").ToString()
                    End If

                    If ProjNo <> Nothing Then
                        hplkAppropriation.Text = "Appropriation Code: " & ds.Tables(0).Rows(0).Item("AppropriationCode").ToString()
                        hplkAppropriation.Visible = True

                        Select Case ProjNo.Substring(0, 1)
                            Case "A"
                                If ProjectTitle <> Nothing Then
                                    hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjAssets.aspx?pProjNo=" & ProjNo
                                Else
                                    hplkAppropriation.Visible = False
                                End If
                            Case "D"
                                If ProjectTitle <> Nothing Then
                                    hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjDevelopment.aspx?pProjNo=" & ProjNo
                                Else
                                    hplkAppropriation.Visible = False
                                End If
                            Case "P"
                                If ProjectTitle <> Nothing Then
                                    hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjPackaging.aspx?pProjNo=" & ProjNo
                                Else
                                    hplkAppropriation.Visible = False
                                End If
                            Case "R"
                                If ProjectTitle <> Nothing Then
                                    hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjRepair.aspx?pProjNo=" & ProjNo
                                Else
                                    hplkAppropriation.Visible = False
                                End If
                            Case "T"
                                If ProjectTitle <> Nothing Then
                                    hplkAppropriation.NavigateUrl = "~/EXP/crViewExpProjTooling.aspx?pProjNo=" & ProjNo
                                Else
                                    hplkAppropriation.Visible = False
                                End If
                        End Select
                    Else
                        gvExpProjDocuments.Visible = False
                    End If
                    gvExpProjDocuments.DataBind()
                End If

                If (ds.Tables(0).Rows(0).Item("DateSigned").ToString() = Nothing Or ds.Tables(0).Rows(0).Item("DateSigned").ToString() = "") And ViewState("ObjectRole") = True Then
                    If ViewState("pProjStat") <> "Void" Then
                        btnSubmit.Enabled = True
                        btnReset.Enabled = True
                    End If
                Else
                    btnSubmit.Enabled = False
                    btnReset.Enabled = False
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

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        ''********
        ''* This function is used to submit email next level Approvers or to originators when rejected.
        ''********
        Dim DefaultDate As Date = Date.Now
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")

        Dim ds1st As DataSet = New DataSet
        Dim ds2nd As DataSet = New DataSet
        Dim dsCC As DataSet = New DataSet
        Dim dsRej As DataSet = New DataSet
        Dim dsSD As DataSet = New DataSet
        Dim EmailTO As String = Nothing
        Dim EmpName As String = Nothing
        Dim EmailCC As String = Nothing
        Dim EmailFrom As String = Nothing
        Dim LinkLocation As String = Nothing
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
            If ddStatus.SelectedValue = "Pending" Then
                lblErrors.Text = "Action Cancelled. Please select a status Approved or Rejected."
                lblErrors.Visible = True
                CheckRights()
                Exit Sub
            End If

            If CurrentEmpEmail <> Nothing And ViewState("pIORNo") <> Nothing Then
                If ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
                    lblErrors.Text = "Your comments is required for Rejection."
                    lblErrors.Visible = True
                    ReqComments.Visible = True
                    CheckRights()
                    Exit Sub

                Else 'ELSE If ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
                    If ViewState("iBuyerID") <> 0 And txtPONo.Text = Nothing Then
                        lblErrors.Text = "Purchase Order # is a required field."
                        lblErrors.Visible = True
                        sDetail.Enabled = True
                        ddStatus.SelectedValue = "Pending"
                        CheckRights()
                        Exit Sub
                    End If
                    ''***************
                    ''Calculate Total Extensions
                    ''***************
                    Dim dsExt As DataSet = New DataSet
                    Dim ReqFormFound As Boolean = False
                    Dim a As Integer = 0
                    Dim TotalAmount As Decimal = 0
                    dsExt = PURModule.GetInternalOrderRequestExpenditure(ViewState("pIORNo"), 0)
                    If commonFunctions.CheckDataSet(dsExt) = True Then 'If missing kick user out from submission.
                        ReqFormFound = True
                        For a = 0 To dsExt.Tables.Item(0).Rows.Count - 1
                            TotalAmount = TotalAmount + dsExt.Tables(0).Rows(a).Item("TotalCost")
                        Next
                    End If 'EOF If commonFunctions.CheckDataSet(dsDoc) = False Then

                    ''*************************************************************************
                    ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                    ''*************************************************************************
                    Dim dsExp As DataSet = New DataSet
                    dsExp = PURModule.GetInternalOrderRequest(ViewState("pIORNo"))
                    If (dsExp.Tables.Item(0).Rows.Count > 0) Then
                        ''**********************
                        ''*Initialize Variables
                        ''**********************
                        Dim IORDescription As String = dsExp.Tables(0).Rows(0).Item("IORDescription")
                        Dim RequestedByTMID As Integer = dsExp.Tables(0).Rows(0).Item("RequestedByTMID")
                        Dim RequestedBy As String = dsExp.Tables(0).Rows(0).Item("RequestedByName")
                        Dim SubmittedByTMID As Integer = dsExp.Tables(0).Rows(0).Item("SubmittedByTMID")
                        Dim ShipTo As String = dsExp.Tables(0).Rows(0).Item("ShipTo")
                        Dim UGNLocation As String = dsExp.Tables(0).Rows(0).Item("UGNFacilityName")
                        Dim POinPesos As Boolean = dsExp.Tables(0).Rows(0).Item("POinPesos")
                        Dim DepartmentName As String = dsExp.Tables(0).Rows(0).Item("DepartmentName")
                        Dim GLAccountName As String = dsExp.Tables(0).Rows(0).Item("GLAccountName")
                        Dim ExpectedDeliveryDate As String = dsExp.Tables(0).Rows(0).Item("ExpectedDeliveryDate")
                        Dim VTypeName As String = dsExp.Tables(0).Rows(0).Item("VTypeName")
                        Dim VendorName As String = dsExp.Tables(0).Rows(0).Item("VendorNo") & " - " & dsExp.Tables(0).Rows(0).Item("VendorName")
                        Dim AppropriationCode As String = dsExp.Tables(0).Rows(0).Item("AppropriationCode")
                        Dim AppropriationDesc As String = dsExp.Tables(0).Rows(0).Item("AppropriationCode") & " - " & dsExp.Tables(0).Rows(0).Item("ProjectTitle")
                        Dim ProjectTitle As String = Nothing
                        If Not IsDBNull(dsExp.Tables(0).Rows(0).Item("ProjectTitle")) Then
                            ProjectTitle = dsExp.Tables(0).Rows(0).Item("ProjectTitle")
                        End If

                        Dim DefinedCapex As String = Nothing
                        If AppropriationCode <> "" Or AppropriationCode <> Nothing Then
                            DefinedCapex = AppropriationCode.Substring(0, 1)
                        End If

                        Dim PONo As String = Nothing

                        If ViewState("iBuyerID") <> 0 Then
                            PONo = txtPONo.Text
                        End If

                        Dim SeqNo As Integer = hfSeqNo.Value

                        ''**********************************************************************
                        ''Check for same level records Rejected. IF so, cancel approval process.
                        ''**********************************************************************
                        If ddStatus.SelectedValue <> "Pending" Then
                            ''***********************************
                            ''Update Current Level Approver record.
                            ''***********************************
                            PURModule.UpdateInternalOrderRequestApproval(ViewState("pIORNo"), DefaultTMID, True, ddStatus.SelectedValue, txtComments.Text, SeqNo, 0, 0, DefaultUser, DefaultDate)

                            ''*****************
                            ''Level Completed
                            ''*****************
                            ds1st = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), SeqNo, 0, False, True)
                            'Locate any Rejected
                            If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                'do nothing
                            Else
                                ds1st = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), SeqNo, 0, True, False)
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
                        dsLast = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), 0, 0, False, False)
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

                        'If LastSeqNo = True Then 'Last Team Member
                        ''***************
                        ''* Locate Next available PONO When Corp Buyer Approvers
                        ''***************
                        ''LRey 06/27/2012 - Manual entry required until sequence is simplified in new ERP
                        ' ''If ddStatus.SelectedValue = "Approved" Then
                        ' ''    Dim ds As DataSet = Nothing
                        ' ''    ds = EXPModule.GetNextExpProjectNo("", "", "IORPO")
                        ' ''    PONo = ds.Tables(0).Rows(0).Item("NextAvailProjNo")
                        ' ''End If
                        'End If

                        ''************************
                        ''* Update Internal_Order_Request record
                        '*************************
                        PURModule.UpdateInternalOrderRequestStatus(ViewState("pIORNo"), IIf(ddStatus.SelectedValue = "Rejected", "In Process", IIf(LastSeqNo = True, "Approved", "In Process")), IIf(ddStatus.SelectedValue = "Rejected", "R", IIf(LastSeqNo = True, "A", "T")), PONo, DefaultUser, DefaultDate)

                        ''**************************************************************
                        ''Locate Next Level Approver(s)
                        ''**************************************************************
                        If LvlApvlCmplt = True Then
                            ''Check at same sequence level
                            ds1st = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), SeqNo, 0, True, False)
                            If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                ''Do not send email at same level twice.
                            Else
                                If ddStatus.SelectedValue <> "Rejected" Then 'Team Member Approved
                                    ds2nd = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), IIf(LastSeqNo = False, (SeqNo + 1), SeqNo), 0, True, False)
                                    If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                                        For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                            If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                            (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                                EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                                EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                                ''*****************************************
                                                ''Update Next Level Approvers DateNotified field.
                                                ''*****************************************
                                                PURModule.UpdateInternalOrderRequestApproval(ViewState("pIORNo"), ds2nd.Tables(0).Rows(i).Item("OrigTeamMemberID"), False, "Pending", "", IIf(LastSeqNo = False, (SeqNo + 1), SeqNo), 0, ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), DefaultUser, DefaultDate)

                                            End If
                                        Next
                                    End If 'EOF ds2nd.Tables.Count > 0
                                End If 'EOF t.SelectedValue <> "Rejected"
                            End If 'EOF ds1st.Tables.Count > 0
                        End If 'EOF If LvlApvlCmplt = True Then

                        'Rejected or last approval
                        If ddStatus.SelectedValue = "Rejected" Or (LastSeqNo = True And ddStatus.SelectedValue = "Approved") Then
                            ''********************************************************
                            ''Notify SubmittedBy if Rejected or last approval
                            ''********************************************************
                            dsRej = SecurityModule.GetTeamMember(IIf(SubmittedByTMID = 0, RequestedByTMID, SubmittedByTMID), Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
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
                            If ddStatus.SelectedValue = "Rejected" Or (LastSeqNo = True And ddStatus.SelectedValue = "Approved") Then
                                ds2nd = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), 0, 0, False, False)
                            Else
                                ds2nd = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), SeqNo, 0, False, False)
                            End If

                            If commonFunctions.CheckDataSet(ds2nd) = True Then
                                For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                    If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                    (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                        EmailCC &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"

                                    End If
                                Next
                            End If 'EOF ds2nd.Tab

                            ''Notify CC List
                            dsCC = commonFunctions.GetTeamMemberBySubscription(152)
                            If commonFunctions.CheckDataSet(dsCC) = True Then
                                For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                                    If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And _
                                    (dsCC.Tables(0).Rows(i).Item("TMID") <> DefaultTMID) And _
                                    (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                        EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"

                                    End If
                                Next
                            End If


                            'Rejected or last approval
                            If ddStatus.SelectedValue = "Rejected" Or (LastSeqNo = True And ddStatus.SelectedValue = "Approved") Then
                                ''********************************************************
                                ''Carbon Copy RequestedByTMID if Rejected or last approval
                                ''********************************************************
                                dsRej = SecurityModule.GetTeamMember(RequestedByTMID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                                ''Check that the recipient(s) is a valid Team Member
                                If dsRej.Tables.Count > 0 And (dsRej.Tables.Item(0).Rows.Count > 0) Then
                                    For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                        If (dsRej.Tables(0).Rows(i).Item("Working") = True) And _
                                        (dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then

                                            EmailCC &= dsRej.Tables(0).Rows(i).Item("Email") & ";"

                                        End If
                                    Next
                                End If 'EOF If dsRej.Tables.Count > 0.....
                            End If 'EOF t.SelectedValue = "Rejected"


                            If AppropriationCode <> Nothing Then
                                LinkLocation = AppropriationCode
                                Select Case AppropriationCode.Substring(0, 1)
                                    Case "A"
                                        LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjAssets.aspx?pProjNo=" & AppropriationCode & "' target='_blank'>" & AppropriationDesc & "</a>"
                                    Case "D"
                                        LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjDevelopment.aspx?pProjNo=" & AppropriationCode & "' target='_blank'>" & AppropriationDesc & "</a>"
                                    Case "P"
                                        LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjPackaging.aspx?pProjNo=" & AppropriationCode & "' target='_blank'>" & AppropriationDesc & "</a>"
                                    Case "R"
                                        LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjRepair.aspx?pProjNo=" & AppropriationCode & "' target='_blank'>" & AppropriationDesc & "</a>"
                                    Case "T"
                                        LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjTooling.aspx?pProjNo=" & AppropriationCode & "' target='_blank'>" & AppropriationDesc & "</a>"
                                End Select
                            End If 'EOF  If txtAppropriation.Text <> Nothing Then

                            ''Test or Production Message display
                            If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                                MyMessage.Subject = "TEST: "
                                MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                            Else
                                MyMessage.Subject = ""
                                MyMessage.Body = ""
                            End If

                            MyMessage.Subject &= "Internal Order Request - " & IORDescription
                            MyMessage.Body &= "<font size='2' face='Verdana'>"
                            If ddStatus.SelectedValue = "Rejected" Then
                                MyMessage.Subject &= " - REJECTED"
                                MyMessage.Body &= EmpName
                                MyMessage.Body &= "<br/><br/>'" & IORDescription & "' was <font color='red'>REJECTED</font>. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/PUR/InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.<br/><br/>Reason for rejection: <font color='red'>" & txtComments.Text & "</font><br/><br/>"
                            Else
                                If LastSeqNo = True Then 'If last approval
                                    MyMessage.Subject &= " - APPROVED"
                                    MyMessage.Body &= "<p>'" & IORDescription & "' is Approved. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/PUR/InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "'>Click here</a> to access the record.</p>"
                                Else
                                    MyMessage.Body &= EmpName
                                    MyMessage.Body &= "<p>'" & IORDescription & "' is available for your Review/Approval. <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/PUR/crInternalOrderRequestApproval.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>"
                                End If
                            End If
                            MyMessage.Body &= "</font>"

                            MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width:900px; font-size: 13; font-family: Tahoma;'>"
                            MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>INTERNAL ORDER REQUEST OVERVIEW</strong></td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right' style='width:70px;'>Reference No:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & ViewState("pIORNo") & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Requestor:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & RequestedBy & "</td>"
                            MyMessage.Body &= "</tr>"

                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & UGNLocation & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Department/Cost Center:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & DepartmentName & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>G/L Account #:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & GLAccountName & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Expected Delivery Date:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & ExpectedDeliveryDate & "</td>"
                            MyMessage.Body &= "</tr>"

                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Vendor:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & VendorName & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Ship To:&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & ShipTo & "</td>"
                            MyMessage.Body &= "</tr>"

                            If AppropriationCode <> Nothing Then
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Appropriation No.:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & LinkLocation & "</td>"
                                MyMessage.Body &= "</tr>"

                                If AppropriationCode.Substring(0, 1) = "T" Then
                                    Dim RevAdapter As New ExpProjTableAdapters.ExpProj_Tooling_Customer_EIOR_TableAdapter
                                    MyMessage.Body &= "<tr>"
                                    MyMessage.Body &= "<td class='p_text' align='center'></td>"
                                    MyMessage.Body &= "<td><table style='border: 1px solid #D0D0BF; font-size: 13; font-family: Tahoma;'> "
                                    MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:white;'>"
                                    MyMessage.Body &= "<td><strong>Part Number</strong></td>"
                                    MyMessage.Body &= "<td><strong>Revision Level</strong></td>"
                                    MyMessage.Body &= "</tr>"
                                    For Each row As DataRow In RevAdapter.Get_ExpProj_Tooling_Customer_EIOR(AppropriationCode)
                                        MyMessage.Body &= "<tr>"
                                        MyMessage.Body &= "<td class='p_text' align='left'>" & row("PartNo").ToString() & "</td>"
                                        MyMessage.Body &= "<td align='left'>" & row("RevisionLevel").ToString() & "</td>"
                                        MyMessage.Body &= "</tr>"
                                    Next
                                    MyMessage.Body &= "</table></td>"
                                    MyMessage.Body &= "</tr>"

                                    MyMessage.Body &= "<tr>"
                                    MyMessage.Body &= "<td >&nbsp;&nbsp;</td>"
                                    MyMessage.Body &= "<td class='p_text' align='left'><strong>Revision Level History</strong></td>"
                                    MyMessage.Body &= "</tr>"
                                    MyMessage.Body &= "<tr>"
                                    MyMessage.Body &= "<td >&nbsp;&nbsp;</td>"
                                    MyMessage.Body &= "<td><table style='border: 1px solid #D0D0BF; font-size: 11; font-family: Tahoma;'> "
                                    MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:white;'>"
                                    MyMessage.Body &= "<td><strong>Action Date</strong></td>"
                                    MyMessage.Body &= "<td><strong>Action Taken By</strong></td>"
                                    MyMessage.Body &= "<td><strong>Action Description</strong></td>"
                                    MyMessage.Body &= "</tr>"
                                    Dim ds1 As DataSet = New DataSet
                                    ds1 = PURModule.GetInternalOrderRequestHistory(ViewState("pIORNo"))
                                    If commonFunctions.CheckDataSet(ds1) = True Then
                                        For i = 0 To ds1.Tables.Item(0).Rows.Count - 1
                                            If ds1.Tables(0).Rows(i).Item("IORDescription") = "" _
                                            Or ds1.Tables(0).Rows(i).Item("IORDescription") = Nothing Then
                                                MyMessage.Body &= "<tr><td>" & ds1.Tables(0).Rows(i).Item("ActionDate")
                                                MyMessage.Body &= "</td><td>" & ds1.Tables(0).Rows(i).Item("ActionTakenBy")
                                                MyMessage.Body &= "</td><td>" & ds1.Tables(0).Rows(i).Item("ActionDesc") & "</td></tr>"
                                            End If
                                        Next
                                    End If
                                    MyMessage.Body &= "</table></td>"
                                    MyMessage.Body &= "</tr>"

                                End If 'EOF  If AppropriationCode <> Nothing Then
                            End If 'EOF   If AppropriationCode <> Nothing Then

                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Total Amount Requested ($):&nbsp;&nbsp; </td>"
                            MyMessage.Body &= "<td>" & Format(TotalAmount, "#,##0.00") & "</td>"
                            MyMessage.Body &= "</tr>"
                            MyMessage.Body &= "<tr>"
                            MyMessage.Body &= "<td class='p_text' align='right'>Create PO in:&nbsp;&nbsp; </td>"
                            If POinPesos = True Then
                                MyMessage.Body &= "<td><font color='red'><strong>MXN Pesos</strong></font></td>"
                            Else
                                MyMessage.Body &= "<td>USD</td>"
                            End If
                            MyMessage.Body &= "</tr>"

                            ''***************************************************
                            ''Get list of Supporting Documentation
                            ''***************************************************
                            Dim dsAED As DataSet
                            dsAED = PURModule.GetInternalOrderRequestDocument(ViewState("pIORNo"), 0)
                            If commonFunctions.CheckDataSet(dsAED) = True Then
                                MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
                                MyMessage.Body &= "<td colspan='2'><strong>SUPPORTING DOCUMENTS:</strong></td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td colspan='2'>"
                                MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "   <td width='250px'><b>Form Description</b></td>"
                                MyMessage.Body &= "   <td width='250px'>&nbsp;</td>"
                                MyMessage.Body &= "</tr>"
                                For i = 0 To dsAED.Tables.Item(0).Rows.Count - 1
                                    MyMessage.Body &= "<tr>"
                                    MyMessage.Body &= "<td height='25'>" & dsAED.Tables(0).Rows(i).Item("Description") & "</td>"
                                    MyMessage.Body &= "<td height='25'><a href='" & ViewState("strProdOrTestEnvironment") & "/PUR/InternalOrderRequestDocument.aspx?pIORNo=" & ViewState("pIORNo") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
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
                            PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), VendorName, DefaultTMID, ddStatus.SelectedValue & " " & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing))

                            ''*****************
                            ''History Tracking
                            ''*****************
                            If ddStatus.SelectedValue <> "Rejected" Then
                                If LastSeqNo = True Then
                                    PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), VendorName, DefaultTMID, "Notification sent to all involved. ")
                                Else
                                    PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), VendorName, DefaultTMID, "Notification sent to level " & (SeqNo + 1) & " approver(s): " & EmpName)
                                End If
                            Else
                                PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), VendorName, DefaultTMID, "Notification sent to " & EmpName)
                            End If

                            ''**********************************
                            ''Connect & Send email notification
                            ''**********************************
                            Try
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "IOR", ViewState("pIORNo"))
                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                            Catch ex As SmtpException
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                UGNErrorTrapping.InsertEmailQueue("IOR Ref#: " & ViewState("pIORNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
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
                                PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), VendorName, DefaultTMID, ddStatus.SelectedValue & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing))

                                ''**********************************
                                ''Rebind the data to the form
                                ''********************************** 
                                BindData()
                                lblErrors.Text = "Your response was submitted successfully."
                                lblErrors.Visible = True
                            End If
                        End If
                    End If
                End If
                'End If 'EOF If ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
            End If 'EOF If ViewState("pIORNo") <> Nothing Then
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

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pIORNo") <> Nothing Then
                ''*************************************************************************
                ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                ''*************************************************************************
                Dim dsExp As DataSet = New DataSet
                dsExp = PURModule.GetInternalOrderRequest(ViewState("pIORNo"))
                If (dsExp.Tables.Item(0).Rows.Count > 0) Then
                    ''**********************
                    ''*Initialize Variables
                    ''**********************
                    Dim SubmittedByTMID As Integer = dsExp.Tables(0).Rows(0).Item("SubmittedByTMID")
                    Dim SubmittedByName As String = dsExp.Tables(0).Rows(0).Item("SubmittedByName")
                    Dim RequestedByTMID As Integer = dsExp.Tables(0).Rows(0).Item("RequestedByTMID")
                    Dim RequestedBy As String = dsExp.Tables(0).Rows(0).Item("RequestedByName")
                    Dim IORDescription As String = dsExp.Tables(0).Rows(0).Item("IORDescription")
                    Dim SeqNo As Integer = hfSeqNo.Value
                    Dim Comments As String = txtQC.Text

                    ''********************************************************
                    ''Notify Submitter
                    ''********************************************************
                    dsRej = SecurityModule.GetTeamMember(SubmittedByTMID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                    ''Check that the recipient(s) is a valid Team Member
                    If dsRej.Tables.Count > 0 And (dsRej.Tables.Item(0).Rows.Count > 0) Then
                        For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                            If (dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                            (dsRej.Tables(0).Rows(i).Item("Working") = True) Then

                                EmailTO &= dsRej.Tables(0).Rows(i).Item("Email") & ";"
                                EmpName &= dsRej.Tables(0).Rows(i).Item("FirstName") & " " & dsRej.Tables(0).Rows(i).Item("LastName") & ", "

                            End If
                        Next
                    End If 'EOF If dsRej.Tables.Count > 0.....

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
                        dsCC = PURModule.GetInternalOrderRequestApproval(ViewState("pIORNo"), SeqNo, 0, False, False)
                        ''Check that the recipient(s) is a valid Team Member
                        If dsCC.Tables.Count > 0 And (dsCC.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                                If (dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                    EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"

                                End If
                            Next
                        End If 'EOF  If dsCC.Tables.Count > 0

                        ''********************************************************
                        ''Carbon Copy RequestedByTMID if Rejected or last approval
                        ''********************************************************
                        dsRej = SecurityModule.GetTeamMember(RequestedByTMID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                        ''Check that the recipient(s) is a valid Team Member
                        If dsRej.Tables.Count > 0 And (dsRej.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                If (dsRej.Tables(0).Rows(i).Item("Working") = True) And _
                                (dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then

                                    EmailCC &= dsRej.Tables(0).Rows(i).Item("Email") & ";"

                                End If
                            Next
                        End If 'EOF If dsRej.Tables.Count > 0.....

                        'Test or Production Message display
                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                        End If

                        MyMessage.Subject &= "Internal Order Request: " & ViewState("pIORNo") & " - " & IORDescription & " - MESSAGE RECEIVED"

                        MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
                        MyMessage.Body &= " <tr>"
                        MyMessage.Body &= "     <td valign='top' width='20%'>"
                        MyMessage.Body &= "         <img src='" & ViewState("strProdOrTestEnvironment") & "/images/messanger60.jpg'/>"
                        MyMessage.Body &= "     </td>"
                        MyMessage.Body &= "     <td valign='top'>"
                        MyMessage.Body &= "             <b>Attention All,</b> "
                        MyMessage.Body &= "             <p><b>" & DefaultUserFullName & "</b> sent a message regarding IOR Ref#"
                        MyMessage.Body &= "             <font color='red'>" & ViewState("pIORNo") & " - " & IORDescription & "</font>."
                        MyMessage.Body &= "             <br/><br/><i>Question:&nbsp;&nbsp;</i><b>" & txtQC.Text & "</b>"
                        MyMessage.Body &= "             </p>"
                        MyMessage.Body &= "             <p><a href='" & ViewState("strProdOrTestEnvironment") & "/PUR/InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pRC=1" & "'>Click here</a> to respond."
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
                        PURModule.InsertInternalOrderRequestHistory(ViewState("pIORNo"), IORDescription, DefaultTMID, "Message Sent")

                        ''*****************
                        ''Save Message
                        ''*****************
                        PURModule.InsertInternalOrderRequestRSS(ViewState("pIORNo"), IORDescription, DefaultTMID, SeqNo, txtQC.Text)

                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "IOR", ViewState("pIORNo"))
                            lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                        Catch ex As SmtpException
                            lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                            UGNErrorTrapping.InsertEmailQueue("IOR Ref#: " & ViewState("pIORNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
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

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click, btnReset2.Click
        Response.Redirect("crInternalOrderRequestApproval.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pAprv=1", False)
    End Sub 'EOF btnReset1_Click

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim RSSID As Integer
            Dim drRSSID As IOR.Internal_Order_Request_RSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, IOR.Internal_Order_Request_RSSRow)

            If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                RSSID = drRSSID.RSSID
                ' Reference the rpCBRC ObjectDataSource
                Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                ' Set the CategoryID Parameter value
                rpCBRC.SelectParameters("IORNo").DefaultValue = drRSSID.IORNO.ToString()
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

    Public Function GoToCapEx(ByVal ProjectNo As String, ByVal DocID As String) As String
        If ProjectNo <> Nothing Then
            Select Case ProjectNo.Substring(0, 1)
                Case "A"
                    Return "~/EXP/AssetsExpProjDocument.aspx?pProjNo=" & ProjectNo & "&pDocID=" & DocID
                Case "D"
                    Return "~/EXP/DevelopmentExpProjDocument.aspx?pProjNo=" & ProjectNo & "&pDocID=" & DocID
                Case "P"
                    Return "~/EXP/PackagingExpProjDocument.aspx?pProjNo=" & ProjectNo & "&pDocID=" & DocID
                Case "R"
                    Return "~/EXP/RepairExpProjDocument.aspx?pProjNo=" & ProjectNo & "&pDocID=" & DocID
                Case "T"
                    Return "~/EXP/ToolingExpProjDocument.aspx?pProjNo=" & ProjectNo & "&pDocID=" & DocID
                Case Else
                    Return Nothing
            End Select
        Else
            Return Nothing
        End If
    End Function 'EOF GoToCapEx
End Class
