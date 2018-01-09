' ************************************************************************************************
' Name:	crARDeductionApproval.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from AR_Deductions table
'           and other sources.  Also used as an approval screen to allow users to view current
'           info related to a capex asset project and approve/reject the project in one screen.
' Date		    Author	    
' 05/01/2012    LRey			Created .Net ugnauto
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class AR_crARDeductionApproval
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        ViewState("pARDID") = HttpContext.Current.Request.QueryString("pARDID")

        Dim m As ASP.crviewmasterpage_master = Master
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            If ViewState("pARDID") = Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Account Receivable</b> > <a href='AR_Deduction_List.aspx'><b>Operations Deduction Form Search</b></a> > Operations Deduction Form Preview"
            Else
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Account Receivable</b> > <a href='AR_Deduction_List.aspx?pARDID=" & ViewState("pARDID") & "&pAprv=1" & "'><b>Operations Deduction Form Search</b></a> > <a href='AR_Deduction.aspx?pARDID=" & ViewState("pARDID") & "&pAprv=1" & "'><b>Operations Deduction</b></a> > Approval"
            End If
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
            Master.Page.Header.Title = "UGN, Inc.: " & ViewState("pARDID") & " - Approval"
        End If

        ''*************************************************
        '' "Form Level Security using Roles &/or Subscriptions"
        ''*************************************************
        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

        ''************************************************************
        ''Code Below counts the number of chars used in comments area
        ''************************************************************
        txtComments.Attributes.Add("onkeypress", "return tbLimit();")
        txtComments.Attributes.Add("onkeyup", "return tbCount(" + lblCommentsChar.ClientID + ");")
        txtComments.Attributes.Add("maxLength", "200")

        txtCM.Attributes.Add("onkeypress", "return tbLimit();")
        txtCM.Attributes.Add("onkeyup", "return tbCount(" + lblCMChar.ClientID + ");")
        txtCM.Attributes.Add("maxLength", "400")

        txtResolution.Attributes.Add("onkeypress", "return tbLimit();")
        txtResolution.Attributes.Add("onkeyup", "return tbCount(" + lblResChar.ClientID + ");")
        txtResolution.Attributes.Add("maxLength", "400")

        txtQC.Attributes.Add("onkeypress", "return tbLimit();")
        txtQC.Attributes.Add("onkeyup", "return tbCount(" + lblQC.ClientID + ");")
        txtQC.Attributes.Add("maxLength", "200")

        ''*********
        ''Get Data
        ''*********
        If Not Page.IsPostBack Then
            If ViewState("pARDID") <> "" Then
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
            ''*******
            '' Disable controls by default
            ''*******
            btnSubmit.Enabled = False
            btnReset.Enabled = False
           
            ''*********************************************************
            ''If Record is Void, do not allow Team Memember submission
            ''*********************************************************
            Dim dsAR As DataSet = New DataSet
            Dim RecStatus As String = Nothing
            Dim IncidentDate As String = Nothing
            ViewState("pRecStatus") = Nothing
            ViewState("pIncidentDate") = Nothing
            dsAR = ARGroupModule.GetARDeduction(ViewState("pARDID"), "", 0, "", "", "", "", "", "", 0, "", "", "", "")
            If commonFunctions.CheckDataSet(dsAR) = True Then
                RecStatus = dsAR.Tables(0).Rows(0).Item("RecStatus").ToString()
                ViewState("pRecStatus") = RecStatus

                IncidentDate = dsAR.Tables(0).Rows(0).Item("IncidentDate").ToString
                ViewState("pIncidentDate") = IncidentDate
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 132 'Operations Deduction Form ID
            Dim iRoleID As Integer = 0


            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Tammy.George", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

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
                                        If lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Pending" And (RecStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                        ElseIf lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                        End If
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        ViewState("ObjectRole") = True
                                        If (lblDateNotified.Text <> Nothing) And (ddStatus.SelectedValue = "Pending") And (RecStatus <> "Void") Then

                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                        End If
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        If ViewState("pIncidentDate") = Nothing And (ddStatus.SelectedValue = "Pending") Then
                                            ViewState("Admin") = True
                                        End If
                                        If (lblDateNotified.Text <> Nothing) And (ddStatus.SelectedValue = "Pending") And (RecStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                        ElseIf (lblDateNotified.Text <> Nothing) And (ddStatus.SelectedValue = "Rejected") And (txtComments.Text = Nothing) And (RecStatus <> "Void") Then
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                        End If
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        ViewState("ObjectRole") = False
                                        If (lblDateNotified.Text <> Nothing) And (ddStatus.SelectedValue = "Pending") And (RecStatus <> "Void") Then
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
        ViewState("pARDID") = HttpContext.Current.Request.QueryString("pARDID")
        Dim oRpt As New ReportDocument()
        Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
        Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
        Dim dbConn As New TableLogOnInfo()

        If ViewState("pARDID") <> "" Then
            Try
                If Session("TempCrystalRptFiles") Is Nothing Then
                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "crARDeduction.rpt")

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
            Dim pfARDID As ParameterField = New ParameterField

            ' setting the name of parameter fields with which they will be recieved in report 
            pfARDID.ParameterFieldName = "@ARDID"

            ' the above declared parameter fields accept values as discrete objects 
            ' so declaring discrete objects 
            Dim dcARDID As New ParameterDiscreteValue

            ' setting the values of discrete objects 
            dcARDID.Value = ViewState("pARDID")

            ' now adding these discrete values to parameters 
            pfARDID.CurrentValues.Add(dcARDID)

            ' now adding all these parameter fields to the parameter collection 
            paramFields.Add(pfARDID)

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
            ds = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), 0, DefaultTMID, False, False)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                lblTeamMbr.Text = ds.Tables(0).Rows(0).Item("TeamMemberName").ToString()
                lblDateNotified.Text = ds.Tables(0).Rows(0).Item("DateNotified").ToString()
                ddStatus.SelectedValue = ds.Tables(0).Rows(0).Item("Status").ToString()
                txtComments.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                hfSeqNo.Value = ds.Tables(0).Rows(0).Item("SeqNo").ToString()

                ''*************************************
                ''Counter Measure displays/entry
                ''*************************************
                lblCM.Visible = False
                txtCM.Visible = False
                lblCMChar.Visible = False
                lblReqCM.Visible = False
                hfCM.Text = 0
                lblReqRes.Visible = False
                lblPostDate.Visible = False
                txtPostDate.Visible = False
                lblResolution.Visible = False
                txtResolution.Visible = False
                lblClosedDate.Visible = False
                txtClosedDate.Visible = False
                btnCloseCM.Visible = False
                Dim ds2 As DataSet = New DataSet
                Dim ds3 As DataSet = New DataSet

                ds2 = ARGroupModule.GetARDeduction(ViewState("pARDID"), "", 0, "", "", "", "", "", "", 0, "", "", "", "")
                If (ds2.Tables.Item(0).Rows.Count > 0) Then
                    If ds2.Tables(0).Rows(0).Item("DeductionAmount").ToString() > 2500 Then
                        ds3 = ARGroupModule.GetARDeductionCntrMsr(ViewState("pARDID"))
                        If (ds3.Tables.Item(0).Rows.Count > 0) Then
                            txtCM.Text = ds3.Tables(0).Rows(0).Item("CounterMeasure").ToString()
                            txtPostDate.Text = ds3.Tables(0).Rows(0).Item("PostDate").ToString()
                            txtResolution.Text = ds3.Tables(0).Rows(0).Item("Resolution").ToString()
                            txtClosedDate.Text = ds3.Tables(0).Rows(0).Item("ClosedDate").ToString()
                        End If

                        If hfSeqNo.Value = 1 Then
                            lblCM.Visible = True
                            txtCM.Visible = True
                            lblCMChar.Visible = True
                            lblReqCM.Visible = True
                            hfCM.Text = 1
                            If txtPostDate.Text <> "" Then
                                lblPostDate.Visible = True
                                txtPostDate.Visible = True
                                lblReqRes.Visible = True
                                lblResolution.Visible = True
                                txtResolution.Visible = True
                                lblClosedDate.Visible = True
                                txtClosedDate.Visible = True
                                If txtClosedDate.Text = "" Then
                                    btnCloseCM.Visible = True
                                End If
                            End If
                        Else
                            lblCM.Visible = True
                            lblCM.Font.Bold = True
                            txtCM.Visible = True
                            txtCM.ReadOnly = True
                            lblReqCM.Visible = True
                            lblCMChar.Visible = False
                            hfCM.Text = 0
                            If txtPostDate.Text <> "" Then
                                lblPostDate.Visible = True
                                txtPostDate.Visible = True
                                If txtResolution.Text <> "" Then
                                    lblReqRes.Visible = True
                                    lblResolution.Visible = True
                                    txtResolution.Visible = True
                                    txtResolution.ReadOnly = True
                                    lblResolution.Font.Bold = True
                                    lblClosedDate.Visible = True
                                    txtClosedDate.Visible = True
                                    btnCloseCM.Visible = False
                                End If
                            End If
                        End If
                    End If
                End If

                If (ds.Tables(0).Rows(0).Item("DateSigned").ToString() <> Nothing) Then
                    btnSubmit.Enabled = False
                    btnReset.Enabled = False
                Else
                    If ViewState("pRecStatus") <> "Void" Then
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
        Response.Redirect("crARDeductionApproval.aspx?pARDID=" & ViewState("pARDID") & "&pAprv=1", False)

    End Sub 'EOF btnReset_Click

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        ''********
        ''* This function is used to submit email next level Approvers or to originators when rejected
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
        Dim i As Integer = 0
        Dim LvlApvlCmplt As Boolean = False
        Dim DefaultDate As Date = Date.Now
        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")
        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

        Dim CurrentEmpEmail As String = Nothing
        If HttpContext.Current.Request.Cookies("UGNDB_User_Email") IsNot Nothing Then
            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
            EmailFrom = CurrentEmpEmail
            EmailCC = CurrentEmpEmail
        Else
            CurrentEmpEmail = "Database.Notifications@ugnauto.com"
            EmailFrom = "Database.Notifications@ugnauto.com"
        End If

        lblErrors.Text = Nothing
        lblErrors.Visible = False
        ReqComments.Visible = False

        ''*********
        ''* Make sure Required Fields are entered
        ''*********
        Dim ReqFieldsAval As Boolean = False
        If ViewState("Admin") = True And ddStatus.SelectedValue <> "Pending" Then
            If txtIncidentDate.Text <> "" Then
                ReqFieldsAval = True
            Else
                ReqFieldsAval = False
                sDetail.Visible = True
                btnReset.Enabled = True
                lblErrors.Text = "   Incident Date is a required field. Reset the page and enter required value(s)."
                lblErrors.Visible = True
                Exit Sub
            End If
        Else
            If ddStatus.SelectedValue = "Pending" Then
                ReqFieldsAval = False
                sDetail.Visible = True
                btnReset.Enabled = True
                lblErrors.Text = "   'Pending' is not a valid Status response. Please try again."
                lblErrors.Visible = True
                Exit Sub
            Else
                ReqFieldsAval = True
                sDetail.Visible = False

            End If
        End If

        If hfSeqNo.Value = 1 And hfCM.Text = 1 Then
            If txtCM.Text <> "" And txtCM.Text <> Nothing Then
                ReqFieldsAval = True
                ARGroupModule.InsertARDeductionCntrMsr(ViewState("pARDID"), DefaultTMID, txtCM.Text)
            Else
                ReqFieldsAval = False
                sDetail.Visible = True
                btnReset.Enabled = True
                lblErrors.Text = "   A Counter Measure is required for deduction amount greater than $2500."
                lblErrors.Visible = True
                Exit Sub
            End If
        End If

        Try
            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And (ReqFieldsAval = True) Then
                If ViewState("pARDID") <> Nothing Then
                    If ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
                        lblErrors.Text = "Your comments is required for Disagreement."
                        lblErrors.Visible = True
                        ReqComments.Visible = True
                        CheckRights()
                    Else 'ELSE If ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
                        ''*************************************************************************
                        ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                        ''*************************************************************************
                        Dim dsAR As DataSet = New DataSet
                        dsAR = ARGroupModule.GetARDeduction(ViewState("pARDID"), "", 0, "", "", "", "", "", "", 0, "", "", "", "")
                        If commonFunctions.CheckDataSet(dsAR) = True Then '(dsAR.Tables.Item(0).Rows.Count > 0) Then
                            ''**********************
                            ''*Initialize Variables
                            ''**********************
                            Dim SubmittedByTMID As String = dsAR.Tables(0).Rows(0).Item("SubmittedByTMID")
                            Dim SubmittedByName As String = dsAR.Tables(0).Rows(0).Item("SubmittedByName")
                            Dim UGNFacilityName As String = dsAR.Tables(0).Rows(0).Item("UGNFacilityName")
                            Dim UGNFacility As String = dsAR.Tables(0).Rows(0).Item("UGNFacility")
                            Dim DeductionAmount As Decimal = dsAR.Tables(0).Rows(0).Item("DeductionAmount")
                            Dim Customer As String = dsAR.Tables(0).Rows(0).Item("ddCustomerDesc")
                            Dim ReferenceNo As String = dsAR.Tables(0).Rows(0).Item("ReferenceNo")

                            Dim IncidentDate As String = Nothing
                            If Not IsDBNull(dsAR.Tables(0).Rows(0).Item("IncidentDate")) Then
                                IncidentDate = dsAR.Tables(0).Rows(0).Item("IncidentDate")
                            Else
                                IncidentDate = txtIncidentDate.Text
                            End If

                            Dim ReasonForDeduction As String = dsAR.Tables(0).Rows(0).Item("ddReasonDesc")
                            Dim Comments As String = dsAR.Tables(0).Rows(0).Item("Comments")

                            Dim SeqNo As Integer = 0
                            Dim NextSeqNo As Integer = 0
                            Dim NextLvl As Integer = 0

                            Select Case hfSeqNo.Value
                                Case 1
                                    SeqNo = 1
                                    NextSeqNo = 2
                                    NextLvl = 134
                                Case 2
                                    SeqNo = 2
                                    NextSeqNo = 3
                                    NextLvl = 135
                                Case 3
                                    SeqNo = 3
                                    NextSeqNo = 4
                                    NextLvl = 136
                                Case 4
                                    SeqNo = 4
                                    NextSeqNo = 5
                                    NextLvl = 143
                                Case 5
                                    SeqNo = 5
                                    NextSeqNo = 0
                                    NextLvl = 143
                            End Select

                            ''**********************************************************************
                            ''Check for same level records Rejected. IF so, cancel approval process.
                            ''**********************************************************************
                            If ddStatus.SelectedValue <> "Pending" Then
                                ''***********************************
                                ''Update Current Level Approver record.
                                ''***********************************
                                ARGroupModule.UpdateARDeductionApproval(ViewState("pARDID"), DefaultTMID, True, ddStatus.SelectedValue, txtComments.Text, SeqNo, 0, DefaultUser, DefaultDate)

                                ''*****************
                                ''Level Completed
                                ''*****************
                                ds1st = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), SeqNo, 0, False, True)
                                'Locate any Rejected
                                If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                    'do nothing
                                Else
                                    ds1st = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), SeqNo, 0, True, False)
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
                            Dim totalPending As Integer = 0

                            dsLast = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), 0, 0, False, False)
                            If commonFunctions.CheckDataSet(dsLast) = True Then
                                For r = 0 To dsLast.Tables.Item(0).Rows.Count - 1
                                    If dsLast.Tables(0).Rows(r).Item("Status") = "Pending" Then
                                        totalPending = totalPending + 1
                                    End If
                                Next
                                If totalPending = 0 Then
                                    LastSeqNo = True
                                Else
                                    LastSeqNo = False
                                End If
                            End If

                            If IncidentDate = Nothing And ViewState("pIncidentDate") <> Nothing Then
                                IncidentDate = ViewState("pIncidentDate")
                            End If

                            ''************************
                            ''* Update Assets record
                            '*************************
                            If LastSeqNo = True Then 'Last Team Member
                                ARGroupModule.UpdateARDeductionStatus(ViewState("pARDID"), IIf(ddStatus.SelectedValue = "Rejected", "In Process", "Approved"), IIf(ddStatus.SelectedValue = "Rejected", "T", "A"), IncidentDate, DefaultUser, DefaultDate)
                            Else 'Not the Last Team Member
                                ARGroupModule.UpdateARDeductionStatus(ViewState("pARDID"), "In Process", IIf(ddStatus.SelectedValue = "Rejected", "T", "T"), IncidentDate, DefaultUser, DefaultDate)
                            End If

                            ''**************************************************************
                            ''Locate Next Level Approver(s)
                            ''************************************************************** 
                            If LvlApvlCmplt = True Then
                                ''Check at same sequence level
                                ds1st = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), SeqNo, 0, True, False)
                                If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then ' Then
                                    ''Do not send email at same level twice.
                                Else
                                    'If ddStatus.SelectedValue <> "Rejected" Then 'Team Member Approved
                                    ds2nd = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), IIf(LastSeqNo = False, (SeqNo + 1), SeqNo), 0, True, False)
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
                                                    ARGroupModule.UpdateARDeductionApproval(ViewState("pARDID"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", IIf(LastSeqNo = False, (SeqNo + 1), SeqNo), 0, DefaultUser, DefaultDate)
                                                End If
                                            End If
                                        Next
                                    End If 'EOF ds2nd.Tables.Count > 0 
                                    'End If 'EOF t.SelectedValue <> "Rejected"

                                    ''********************************************************
                                    ''Notify Requestor if last approval
                                    ''********************************************************
                                    If (LastSeqNo = True And ddStatus.SelectedValue = "Approved") Then
                                        ''********************************************************
                                        ''Notify Project Lead
                                        ''********************************************************
                                        dsRej = ARGroupModule.GetARDeductionLead(ViewState("pARDID"))
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
                                'If ddStatus.SelectedValue = "Rejected" Or (LastSeqNo = True And ddStatus.SelectedValue = "Approved") Then
                                If (LastSeqNo = True) Then
                                    ''And ddStatus.SelectedValue = "Approved"
                                    ''********************************************************
                                    ''Notify Project Lead
                                    ''********************************************************
                                    dsRej = ARGroupModule.GetARDeductionLead(ViewState("pARDID"))
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
                                End If 'EOF (LastSeqNo = True And ddStatus.SelectedValue = "Approved")
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
                                If LastSeqNo = True Then
                                    EmailCC = CarbonCopyList(MyMessage, 0, UGNFacility, 1, 0, EmailCC, DefaultTMID)

                                    If SeqNo = 5 Then
                                        EmailCC = CarbonCopyList(MyMessage, 0, "", (SeqNo - 3), 0, EmailCC, DefaultTMID)
                                        EmailCC = CarbonCopyList(MyMessage, 0, "", (SeqNo - 2), 0, EmailCC, DefaultTMID)
                                    End If

                                    EmailCC = CarbonCopyList(MyMessage, 0, "", (SeqNo - 1), 0, EmailCC, DefaultTMID)

                                    ''********************************
                                    '*Carbon Copy CC List Cost Accountants
                                    ''********************************
                                    EmailCC = CarbonCopyList(MyMessage, 138, UGNFacility, 0, 0, EmailCC, DefaultTMID)

                                Else
                                    EmailCC = CarbonCopyList(MyMessage, 0, "", SeqNo, 0, EmailCC, DefaultTMID)
                                End If

                                'If ddStatus.SelectedValue <> "Rejected" Then
                                ''If LastSeqNo = False Then
                                ''    ''**************************************************************
                                ''    ''Carbon Copy Project Lead
                                ''    ''**************************************************************
                                ''    EmailCC = CarbonCopyList(MyMessage, 0, "", 0, 0, EmailCC, DefaultTMID)
                                ''End If
                                ' End If

                                'Test or Production Message display
                                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                                    MyMessage.Subject = "TEST: "
                                Else
                                    MyMessage.Subject = ""
                                End If

                                MyMessage.Subject &= "Operations Deduction for " & ReasonForDeduction & " (Rec# " & ViewState("pARDID") & ")"

                                ''If ddStatus.SelectedValue = "Rejected" Then
                                ''    ''MyMessage.Subject &= " - REJECTED"
                                ''    MyMessage.Body = "<font size='2' face='Tahoma'>" & EmpName & "</font>"
                                ''    MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & "This Operations Deduction for '" & ReasonForDeduction & "' was <font color='red'>REJECTED</font>. <br/><br/>Reason for rejection: " & txtComments.Text & "<br/><br/>"
                                ''    MyMessage.Body &= "<a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/AR_Deduction.aspx?pARDID=" & ViewState("pARDID") & "'>Click here</a> to access the record.</font></p>"
                                ''Else
                                If LastSeqNo = True Then 'If last approval
                                    ''MyMessage.Subject &= " - APPROVED"
                                    MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & "This Operations Deduction for '" & ReasonForDeduction & "' was reviewed by all team members. "
                                    MyMessage.Body &= " <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/AR_Deduction.aspx?pARDID=" & ViewState("pARDID") & "'>Click here</a> to access the record.</font></p>"
                                Else
                                    MyMessage.Body = "<font size='2' face='Tahoma'>" & EmpName & "</font>"
                                    MyMessage.Body &= "<p><font size='2' face='Tahoma'>" & "This Operations Deduction is for '" & ReasonForDeduction & "' is available for your Review. "
                                    MyMessage.Body &= "<a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/crARDeductionApproval.aspx?pARDID=" & ViewState("pARDID") & "&pAprv=1" & "'>Click here</a> to access the record.</font></p>"
                                End If
                                ''End If

                                MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"
                                MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>PROJECT OVERVIEW</strong></td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Rec No:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & ViewState("pARDID") & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Submitted By:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & SubmittedByName & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & UGNFacilityName & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Deduction Amount ($):&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & DeductionAmount & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Customer:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & Customer & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Reference No:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & ReferenceNo & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right'>Incident Date:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td>" & IncidentDate & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Reason for Deduction:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td style='width: 700px;'>" & ReasonForDeduction & "</td>"
                                MyMessage.Body &= "</tr>"
                                MyMessage.Body &= "<tr>"
                                MyMessage.Body &= "<td class='p_text' align='right' valign='top'>Comments:&nbsp;&nbsp; </td>"
                                MyMessage.Body &= "<td style='width: 700px;'>" & Comments & "</td>"
                                MyMessage.Body &= "</tr>"

                                ' ''If txtCM.Text <> "" Then
                                ' ''    MyMessage.Body &= "<tr>"
                                ' ''    MyMessage.Body &= "<td class='p_text' align='right' valign='top'><b>Counter Measure:</b>&nbsp;&nbsp; </td>"
                                ' ''    MyMessage.Body &= "<td style='width: 700px;'>" & txtCM.Text & "</td>"
                                ' ''    MyMessage.Body &= "</tr>"
                                ' ''End If

                                ''***************************************************
                                ''Get list of Supporting Documentation
                                ''***************************************************
                                Dim dsAED As DataSet
                                dsAED = ARGroupModule.GetARDeductionDocuments(ViewState("pARDID"), 0, False)
                                If dsAED.Tables.Count > 0 And (dsAED.Tables.Item(0).Rows.Count > 0) Then
                                    MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
                                    MyMessage.Body &= "<td colspan='2'><strong>SUPPORTING DOCUMENT(S):</strong></td>"
                                    MyMessage.Body &= "</tr>"
                                    MyMessage.Body &= "<tr>"
                                    MyMessage.Body &= "<td colspan='2'>"
                                    MyMessage.Body &= "<table border='0' style='font-size: 13; font-family: Tahoma;'>"
                                    MyMessage.Body &= "  <tr>"
                                    MyMessage.Body &= "   <td width='250px'><b>File Description</b></td>"
                                    MyMessage.Body &= "   <td width='250px'>&nbsp;</td>"
                                    MyMessage.Body &= "</tr>"
                                    For i = 0 To dsAED.Tables.Item(0).Rows.Count - 1
                                        MyMessage.Body &= "<tr>"
                                        MyMessage.Body &= "<td height='25'>" & dsAED.Tables(0).Rows(i).Item("Description") & "</td>"
                                        MyMessage.Body &= "<td height='25'><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/AR_Deduction_Document.aspx?pARDID=" & ViewState("pARDID") & "&pDocID=" & dsAED.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsAED.Tables(0).Rows(i).Item("FileName") & "</a></td>"
                                        MyMessage.Body &= "</tr>"
                                    Next
                                    MyMessage.Body &= "</table>"
                                    MyMessage.Body &= "</tr>"
                                End If
                                MyMessage.Body &= "</table>"

                                Dim emailList As String() = commonFunctions.CleanEmailList(EmailCC).Split(";")
                                Dim ccEmail As String = Nothing
                                For i = 0 To UBound(emailList)
                                    If emailList(i) <> ";" And emailList(i).Trim <> "" And emailList(i) <> EmailTO Then
                                        ccEmail += emailList(i) & ";"
                                    End If
                                Next i
                                EmailCC = ccEmail

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
                                ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), SubmittedByTMID, DefaultTMID, ddStatus.SelectedItem.Text & " " & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing), "", "", "", "")

                                ''*****************
                                ''History Tracking
                                ''*****************
                                If ddStatus.SelectedValue <> "Rejected" Then
                                    If LastSeqNo = True Then
                                        ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), SubmittedByTMID, DefaultTMID, "Notification sent to all involved. ", "", "", "", "")
                                    Else
                                        ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), SubmittedByTMID, DefaultTMID, "Notification sent to level " & (SeqNo + 1) & " approver(s): " & EmpName, "", "", "", "")
                                    End If
                                Else
                                    ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), SubmittedByTMID, DefaultTMID, "Notification sent to " & EmpName, "", "", "", "")
                                End If

                                ''**********************************
                                ''Connect & Send email notification
                                ''**********************************
                                Try
                                    commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "AR_Deduction)", ViewState("pARDID"))
                                    lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                Catch ex As SmtpException
                                    lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."

                                    UGNErrorTrapping.InsertEmailQueue("Deduction Rec No:" & ViewState("pARDID"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
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
                                    ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), SubmittedByTMID, DefaultTMID, ddStatus.SelectedItem.Text & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing), "", "", "", "")
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
                    'End If 'EOF If ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
                End If 'EOF If ViewState("pARDID") <> Nothing Then
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

    Public Function CarbonCopyList(ByVal MyMessage As MailMessage, ByVal SubscriptionID As Integer, ByVal UGNLoc As String, ByVal SeqNo As Integer, ByVal RejectedTMID As Integer, ByVal EmailCC As String, ByVal DefaultTMID As Integer) As String
        Try
            Dim dsCC As DataSet = New DataSet
            Dim IncludeOrigAprvlTM As Boolean = False
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            If SeqNo = 0 Then 'No Rejections have been made, Send notification to all who applies
                If SubscriptionID = 0 Then ''Account Mananager
                    dsCC = ARGroupModule.GetARDeductionLead(ViewState("pARDID"))
                    IncludeOrigAprvlTM = True
                Else '' All others based on facility
                    If UGNLoc <> Nothing Then
                        dsCC = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(SubscriptionID, UGNLoc)
                    Else
                        If SubscriptionID = 134 Or SubscriptionID = 135 Or SubscriptionID = 136 Or SubscriptionID = 143 Or SubscriptionID = 149 Or SubscriptionID = 138 Then
                            ''Notify 1st level, 2nd level, 3rd level, 4th Level, 5th level or CC List
                            dsCC = commonFunctions.GetTeamMemberBySubscription(SubscriptionID)
                            IncludeOrigAprvlTM = True
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
                dsCC = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), SeqNo, 0, False, False)
                'Carbon Copy pending approvers at same level as who rejected the record.
                If commonFunctions.CheckDataSet(dsCC) = True Then
                    For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                        If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) And (RejectedTMID <> dsCC.Tables(0).Rows(i).Item("TeamMemberID")) Then
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
                dsCC = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), 0, 0, False, False)
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

    Protected Sub btnSubmit2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit2.Click
        Try

            ''********
            ''* This function is used to submit email next level Approvers or to originators when rejected.
            ''********188 371 510 569
            Dim DefaultDate As Date = Date.Now
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = ViewState("UGNDB_TMID")
            Dim DefaultUserFullName As String = Nothing
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
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
                If ViewState("pARDID") <> Nothing Then
                    ''*************************************************************************
                    ''Build Email Notification, Sender, Recipient(s), Subject, Body information
                    ''*************************************************************************
                    Dim dsAR As DataSet = New DataSet
                    'dsAR = ARGroupModule.GetARDeduction(ViewState("pARDID"), "", 0, "", "", 0, "", "", "", "", 0, "", "", "", 0, "")
                    dsAR = ARGroupModule.GetARDeduction(ViewState("pARDID"), "", 0, "", "", "", "", "", "", 0, "", "", "", "")
                    If commonFunctions.CheckDataSet(dsAR) = True Then '(dsAR.Tables.Item(0).Rows.Count > 0)
                        ''**********************
                        ''*Initialize Variables
                        ''**********************
                        Dim SubmittedByTMID As String = dsAR.Tables(0).Rows(0).Item("SubmittedByTMID")
                        Dim ReasonForDeduction As String = dsAR.Tables(0).Rows(0).Item("ddReasonDesc")
                        Dim Reason As String = dsAR.Tables(0).Rows(0).Item("Reason")

                        Dim SeqNo As Integer = 0
                        Dim NextSeqNo As Integer = 0
                        Dim NextLvl As Integer = 0

                        Select Case hfSeqNo.Value
                            Case 1
                                SeqNo = 1
                                NextSeqNo = 2
                                NextLvl = 134
                            Case 2
                                SeqNo = 2
                                NextSeqNo = 3
                                NextLvl = 135
                            Case 3
                                SeqNo = 3
                                NextSeqNo = 4
                                NextLvl = 136
                            Case 4
                                SeqNo = 4
                                NextSeqNo = 0
                                NextLvl = 136
                        End Select

                        ''********************************************************
                        ''Notify Project Leader
                        ''********************************************************
                        dsRej = ARGroupModule.GetARDeductionLead(ViewState("pARDID"))
                        ''Check that the recipient(s) is a valid Team Member
                        If commonFunctions.CheckDataSet(dsRej) = True Then 'dsRej.Tables.Count > 0 And (dsRej.Tables.Item(0).Rows.Count > 0)
                            For i = 0 To dsRej.Tables.Item(0).Rows.Count - 1
                                If (dsRej.Tables(0).Rows(i).Item("WorkStatus") = True) And ((dsRej.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail)) Then
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
                            dsCC = ARGroupModule.GetARDeductionApproval(ViewState("pARDID"), SeqNo, DefaultTMID, False, False)
                            ''Check that the recipient(s) is a valid Team Member
                            If commonFunctions.CheckDataSet(dsCC) = True Then 'dsCC.Tables.Count > 0 And (dsCC.Tables.Item(0).Rows.Count > 0)
                                For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                                    If (dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                                        If (dsCC.Tables(0).Rows(i).Item("Email") <> Nothing) And (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                            If EmailCC = Nothing Then
                                                EmailCC = dsCC.Tables(0).Rows(i).Item("Email")
                                            Else
                                                EmailCC = EmailCC & ";" & dsCC.Tables(0).Rows(i).Item("Email")
                                            End If
                                        End If
                                    End If
                                Next

                            End If 'EOF  If dsCC.Tables.Count > 0

                            'Test or Production Message display
                            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                                MyMessage.Subject = "TEST: "
                            Else
                                MyMessage.Subject = ""
                            End If

                            MyMessage.Subject &= "Operations Deduction for " & ReasonForDeduction & " (Rec# " & ViewState("pARDID") & ") - MESSAGE RECEIVED"

                            MyMessage.Body = "<table style='font-size: 13; font-family: Tahoma;'>"
                            MyMessage.Body &= " <tr>"
                            MyMessage.Body &= "     <td valign='top' width='20%'>"
                            MyMessage.Body &= "         <img src='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/images/messanger60.jpg'/>"
                            MyMessage.Body &= "     </td>"
                            MyMessage.Body &= "     <td valign='top'>"
                            MyMessage.Body &= "             <b>Attention All,</b> "
                            MyMessage.Body &= "             <p><b>" & DefaultUserFullName & "</b> sent a message regarding Operations Deduction "
                            MyMessage.Body &= "             <font color='red'>(Rec#" & ViewState("pARDID") & ") " & ReasonForDeduction & "</font>."
                            MyMessage.Body &= "             <br/><br/><i>Question:&nbsp;&nbsp;</i><b>" & txtQC.Text & "</b>"
                            MyMessage.Body &= "             </p>"
                            MyMessage.Body &= "             <p><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/AR_Deduction.aspx?pARDID=" & ViewState("pARDID") & "&pRC=1" & "'>Click here</a> to respond."
                            MyMessage.Body &= "     </td>"
                            MyMessage.Body &= " </tr>"
                            MyMessage.Body &= "</table>"
                            MyMessage.Body &= "<br/><br/>"


                            Dim emailList As String() = commonFunctions.CleanEmailList(EmailCC).Split(";")
                            Dim ccEmail As String = Nothing
                            For i = 0 To UBound(emailList)
                                If emailList(i) <> ";" And emailList(i).Trim <> "" And emailList(i) <> EmailTO Then
                                    ccEmail += emailList(i) & ";"
                                End If
                            Next i
                            EmailCC = ccEmail

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
                            ARGroupModule.InsertARDeductionHistory(ViewState("pARDID"), Reason, DefaultTMID, "Message Sent", "", "", "", "")

                            ''*****************
                            ''Save Message
                            ''*****************
                            ARGroupModule.InsertARDeductionRSS(ViewState("pARDID"), Reason, DefaultTMID, txtQC.Text, SeqNo)

                            ''**********************************
                            ''Connect & Send email notification
                            ''**********************************
                            Try
                                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "AR_Deduction", ViewState("pARDID"))
                                lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                                lblErrors.Visible = True
                            Catch ex As SmtpException
                                lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                                lblErrors.Visible = True

                                UGNErrorTrapping.InsertEmailQueue("Deduction Rec No:" & ViewState("pARDID"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
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

                            lblErrors.Text = "Unable to locate a valid email address to send message to. Please contact the ugnauto Department for assistance."
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

    Protected Sub btnReset2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset2.Click
        Response.Redirect("crARDeductionApproval.aspx?pARDID=" & ViewState("pARDID") & "&pAprv=1", False)
    End Sub 'EOF btnReset2_Click

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim RSSID As Integer
            Dim drRSSID As AR.AR_Deduction_RSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, AR.AR_Deduction_RSSRow)

            If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                RSSID = drRSSID.RSSID
                ' Reference the rpCBRC ObjectDataSource
                Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                ' Set the RID Parameter value
                rpCBRC.SelectParameters("ARDID").DefaultValue = drRSSID.ARDID.ToString()
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

    Protected Sub btnCloseCM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseCM.Click
        If hfSeqNo.Value = 1 Then
            If hfCM.Text = 1 And txtResolution.Text <> "" Then
                ARGroupModule.UpdateARDeductionCntrMsr(ViewState("pARDID"), txtResolution.Text)
                lblErrors.Text = "   Your response was saved successfully."
                lblErrors.Visible = True
                BindData()
            Else
                sDetail.Visible = True
                btnReset.Enabled = True
                lblErrors.Text = "   Resoluion is a required field."
                lblErrors.Visible = True
                Exit Sub
            End If
        End If
    End Sub
End Class