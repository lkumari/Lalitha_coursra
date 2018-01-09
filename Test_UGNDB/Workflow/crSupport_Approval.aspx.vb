' ************************************************************************************************
'
' Name:		crSupport_Approval.aspx
' Purpose:	This Code Behind is for the AR Event Approval and Crystal Report
'
' Date		Author	    
' 02/05/2013    Roderick Carlson
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class crSupport_Approval
    Inherits System.Web.UI.Page

    Protected Sub BindCriteria()

        Try
            Dim ds As DataSet

            ds = SupportModule.GetSupportRequestApprovalStatus()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddApprovalStatus.DataSource = ds
                ddApprovalStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddApprovalStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddApprovalStatus.DataBind()             
            End If

            ds = commonFunctions.GetTeamMember("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddTeamMember.DataSource = ds
                ddTeamMember.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName
                ddTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddTeamMember.DataBind()
                ddTeamMember.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("TeamMemberID") = 0

            ViewState("isAdmin") = False        

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                ''test developer as another team member
                'If iTeamMemberID = 530 Then

                '    'Eileen.Cusack-Marvel 
                '    iTeamMemberID = 366

                '    'randy.khalaf 
                '    'iTeamMemberID = 569

                'End If

                ViewState("TeamMemberID") = iTeamMemberID

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 70)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)                                    
                            ViewState("isAdmin") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True                          
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                            ViewState("isAdmin") = True
                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isAdmin") = True
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.crviewmasterpage_master = Master

            Dim ds As DataSet

            InitializeViewState()

            If HttpContext.Current.Request.QueryString("JobNumber") <> "" Then

                If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                    commonFunctions.SetUGNDBUser()
                End If

                ViewState("JobNumber") = HttpContext.Current.Request.QueryString("JobNumber")

                Dim oRpt As ReportDocument = New ReportDocument()

                ds = SupportModule.GetSupportRequest(ViewState("JobNumber"))

                If commonFunctions.CheckDataSet(ds) = True Then

                    lblJobNumber.Text = ds.Tables(0).Rows(0).Item("JobNumber").ToString.Trim

                    ViewState("jnId") = ds.Tables(0).Rows(0).Item("jnId")
                    lblJnId.Text = ViewState("jnId")

                    Dim strAssignedTo As String = ds.Tables(0).Rows(0).Item("AssignedTo").ToString.Trim

                    Dim dsTeamMember As DataSet = SupportModule.GetTeamMemberByString(strAssignedTo)

                    If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                        If dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                            If dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                ViewState("AssignedTo") = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")                                
                            End If
                        End If
                    End If

                    If Session("SupportPreviewJobNumber") <> ViewState("JobNumber") Then
                        Session("SupportPreview") = Nothing
                        Session("SupportPreviewJobNumber") = Nothing
                    End If

                    If (Session("SupportPreview") Is Nothing) Then

                        Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
                        Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
                        Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

                        ' new report document object 
                        oRpt.Load(Server.MapPath(".\Forms\") & "Support.rpt")

                        'getting the database, the table and the LogOnInfo object which holds login onformation 
                        crDatabase = oRpt.Database

                        'getting the table in an object array of one item 
                        Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
                        crDatabase.Tables.CopyTo(arrTables, 0)
                        ' assigning the first item of array to crTable by downcasting the object to Table 
                        crTable = arrTables(0)

                        Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBSupport").ToString() 'Test_DBRequests or DBRequests
                        Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() 'SQLCLUSTERVS
                        Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                        Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                        oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                        oRpt.SetParameterValue("@JobNumber", ViewState("JobNumber"))

                        ' defining report source 
                        CrystalReportViewer1.DisplayGroupTree = False
                        CrystalReportViewer1.ReportSource = oRpt

                        Session("SupportPreviewJobNumber") = ViewState("JobNumber")
                        Session("SupportPreview") = oRpt

                    Else
                        oRpt = CType(Session("SupportPreview"), ReportDocument)
                        CrystalReportViewer1.ReportSource = oRpt
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Protected Sub EnableControls()

        Try

            btnStatusSubmit.Visible = False
            btnStatusReset.Visible = False

            ddApprovalStatus.Enabled = False
            txtApprovalComment.Enabled = False

            If ddApprovalStatus.SelectedValue = 1 Or ddApprovalStatus.SelectedValue = 2 Then
                btnStatusSubmit.Visible = ViewState("isAdmin")
                btnStatusReset.Visible = ViewState("isAdmin")

                ddApprovalStatus.Enabled = ViewState("isAdmin")
                txtApprovalComment.Enabled = ViewState("isAdmin")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub


    Private Sub InitializeViewState()

        Try
            ViewState("TeamMemberID") = 0
            ViewState("isAdmin") = False

            ViewState("JobNumber") = ""
            ViewState("jnId") = 0
            ViewState("AssignedTo") = 0

            ViewState("ApproverRowID") = 0
            ViewState("ApproverEmail") = ""
            ViewState("ApproverTeamMemberID") = 0
            ViewState("RoutingLevel") = 0

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub BindData(ByVal ApproverTeamMemberID As Integer)

        Try
            Dim ds As DataSet

            Dim dsTeamMember As DataSet
            Dim iApproverTeamMemberID As Integer = ApproverTeamMemberID
            If ApproverTeamMemberID = 0 Then
                ApproverTeamMemberID = ViewState("TeamMemberID")
            End If

            Dim iRowCounter As Integer = 0

            ViewState("ApproverEmail") = ""
            ViewState("ApproverTeamMemberID") = 0
            ViewState("RoutingLevel") = 0
            ViewState("ApproverRowID") = 0

            'if no approval found for existing team member then check if the current team member is admin and allow team member to be selected
            ds = SupportModule.GetSupportRequestApproval(ViewState("jnId"), 0, ApproverTeamMemberID, 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddApprovalStatus.SelectedValue = ds.Tables(0).Rows(0).Item("StatusID").ToString

                If ds.Tables(0).Rows(0).Item("RowID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("RowID") > 0 Then
                        ViewState("ApproverRowID") = ds.Tables(0).Rows(0).Item("RowID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("RoutingLevel") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("RoutingLevel") > 0 Then
                        ViewState("RoutingLevel") = ds.Tables(0).Rows(0).Item("RoutingLevel")
                    End If
                End If

                txtApprovalComment.Text = ds.Tables(0).Rows(0).Item("Comments").ToString
                ddTeamMember.SelectedValue = ds.Tables(0).Rows(0).Item("TeamMemberID").ToString
                lblNotificationDate.Text = ds.Tables(0).Rows(0).Item("NotificationDate").ToString

                If ddTeamMember.SelectedIndex >= 0 Then
                    iApproverTeamMemberID = ddTeamMember.SelectedValue
                    ViewState("ApproverID") = iApproverTeamMemberID

                    dsTeamMember = SecurityModule.GetTeamMember(iApproverTeamMemberID, "", "", "", "", "", True, Nothing)
                    If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                        If dsTeamMember.Tables(0).Rows(iRowCounter).Item("Email").ToString <> "" Then
                            ViewState("ApproverEmail") &= dsTeamMember.Tables(0).Rows(iRowCounter).Item("Email").ToString
                        End If
                    End If
                End If
            Else
                ddTeamMember.Enabled = ViewState("isAdmin")
            End If


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                'If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Or Response.Cookies("UGNDB_UserFullName") Is Nothing Then
                '    ' commonFunctions.SetUGNDBUser()
                '    Dim FullName As String = commonFunctions.getUserName()
                '    Dim UserEmailAddress As String = FullName & "@ugnauto.com"
                '    Response.Cookies("UGNDB_User_Email").Value = UserEmailAddress
                '    If FullName = Nothing Then
                '        FullName = "Demo.Demo"  '* This account has restricted read only rights.
                '    End If
                '    Dim LocationOfDot As Integer = InStr(FullName, ".")
                '    If LocationOfDot > 0 Then
                '        Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                '        Dim FirstInitial As String = Left(FullName, 1)
                '        Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                '        Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
                '        Response.Cookies("UGNDB_UserFullName").Value = Mid(FirstName, 1, 1).ToUpper & Mid(FirstName, 2, Len(FirstName) - 1) & " " & Mid(LastName, 1, 1).ToUpper & Mid(LastName, 2, Len(LastName) - 1)
                '    Else
                '        Response.Cookies("UGNDB_User").Value = FullName
                '        Response.Cookies("UGNDB_UserFullName").Value = FullName

                '    End If
                'End If

                CheckRights()

                BindCriteria()

                BindData(0)

                EnableControls()

                txtApprovalComment.Attributes.Add("onkeypress", "return tbLimit();")
                txtApprovalComment.Attributes.Add("onkeyup", "return tbCount(" + lblApprovalCommentCharCount.ClientID + ");")
                txtApprovalComment.Attributes.Add("maxLength", "400")

            End If

            'normally, this would all be handled in the Init function but since there is a need to check the AR EventID against the deduction table, it needs to be handled here.
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then                
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Work Flow</b> <a href='Support_List.aspx'><b>Support Search</b></a> > Support Detail > <a href='Support_Detail.aspx?JobNumber=" & ViewState("JobNumber") & " '>Support Detail </a> > Support Approval "

                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        Try
            'in order to clear crystal reports
            If HttpContext.Current.Session("SupportPreview") IsNot Nothing Then
                Dim tempRpt As ReportDocument = New ReportDocument()
                tempRpt = CType(HttpContext.Current.Session("SupportPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("SupportPreviewJobNumber") = Nothing
                HttpContext.Current.Session("SupportPreview") = Nothing
                GC.Collect()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BuildEmailToComplete()

        Try

            If ViewState("JobNumber") <> "" Then

                Dim strEmailSubject As String = ""
                Dim strEmailBody As String = ""

                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                Dim strEmailURL As String = strProdOrTestEnvironment & "Workflow/Support_Detail.aspx?JobNumber=" & ViewState("JobNumber")

                Dim dsTeamMember As DataSet
                Dim strTeamMemberEmail As String = ""

                If ViewState("AssignedTo") > 0 Then

                    dsTeamMember = SecurityModule.GetTeamMember(ViewState("AssignedTo"), Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)

                    If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                        If dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim <> "" Then
                            strTeamMemberEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim
                        End If
                    End If
                End If

                'assign email subject
                strEmailSubject = "SUPPORT REQUEST: " & ViewState("JobNumber") & " is has been approved by all and can be developed"

                'build email body
                strEmailBody = "<font size='2' face='Verdana'>The following support request has been approved by all and can be developed:</font><br /><br />"
                strEmailBody &= "<font size='2' face='Verdana'>Support Request ID: <b>" & ViewState("JobNumber") & "</b></font><br /><br />"

                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & "'>Click here to review</a></font><br /><br />"

                'send the actual email
                SendEmail(strTeamMemberEmail, "", strEmailSubject, strEmailBody, ViewState("JobNumber"))
            End If

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Function DisplayImage(ByVal EncodeType As String) As String

        Dim strReturn As String = ""

        Try

            If EncodeType = Nothing Then
                strReturn = ""
            ElseIf EncodeType = "application/vnd.ms-excel" Or EncodeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" Then
                strReturn = "~/images/xls.jpg"
            ElseIf EncodeType = "application/pdf" Then
                strReturn = "~/images/pdf.jpg"
            ElseIf EncodeType = "application/msword" Or EncodeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" Then
                strReturn = "~/images/doc.jpg"
            Else
                strReturn = "~/images/PreviewUp.jpg"
            End If

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        Return strReturn

    End Function 'EOF DisplayImage

    Private Sub BuildEmailToResetApproval()

        Try

            If ViewState("JobNumber") <> "" Then

                Dim strEmailSubject As String = ""
                Dim strEmailBody As String = ""

                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                Dim strEmailURL As String = strProdOrTestEnvironment & "Workflow/Support_Detail.aspx?JobNumber=" & ViewState("JobNumber")

                Dim dsTeamMember As DataSet
                Dim strTeamMemberEmail As String = ""

                If ViewState("AssignedTo") > 0 Then

                    dsTeamMember = SecurityModule.GetTeamMember(ViewState("AssignedTo"), Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)

                    If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                        If dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim <> "" Then
                            strTeamMemberEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim
                        End If
                    End If
                End If

                'assign email subject
                strEmailSubject = "SUPPORT REQUEST: " & ViewState("JobNumber") & " is has been rejected"

                'build email body
                strEmailBody = "<font size='2' face='Verdana'>The following support request has been rejected:</font><br /><br />"
                strEmailBody &= "<font size='2' face='Verdana'>Support Request ID: <b>" & ViewState("JobNumber") & "</b></font><br /><br />"

                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & "'>Click here to review</a></font><br /><br />"

                'send the actual email
                SendEmail(strTeamMemberEmail, "", strEmailSubject, strEmailBody, ViewState("JobNumber"))
            End If

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BuildEmailToForwardApproval(ByVal EmailToAddress As String)

        Try

            If ViewState("JobNumber") <> "" Then

                Dim strEmailSubject As String = ""
                Dim strEmailBody As String = ""

                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                Dim strEmailURL As String = strProdOrTestEnvironment & "Workflow/crSupport_Approval.aspx?JobNumber=" & ViewState("JobNumber")

                Dim dsTeamMember As DataSet
                Dim strTeamMemberEmail As String = ""

                If ViewState("AssignedTo") > 0 Then

                    dsTeamMember = SecurityModule.GetTeamMember(ViewState("AssignedTo"), Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)

                    If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                        If dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim <> "" Then
                            strTeamMemberEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim
                        End If
                    End If
                End If

                'assign email subject
                strEmailSubject = "SUPPORT REQUEST: " & ViewState("JobNumber") & " is ready for approval"

                'build email body
                strEmailBody = "<font size='2' face='Verdana'>The following support request is ready for your approval:</font><br /><br />"
                strEmailBody &= "<font size='2' face='Verdana'>Support Request ID: <b>" & ViewState("JobNumber") & "</b></font><br /><br />"

                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & "'>Click here to review</a></font><br /><br />"

                'send the actual email
                SendEmail(EmailToAddress, strTeamMemberEmail, strEmailSubject, strEmailBody, ViewState("JobNumber"))
            End If

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ForwardApproval()

        Try
            lblMessage.Text = ""

            Dim ds As DataSet
            Dim dsTeamMember As DataSet

            Dim iRowID As Integer = 0
            Dim iNotifyTeamMemberID As Integer = 0
            Dim iRoutingLevel As Integer = 0

            Dim strEmailToAddress As String = ""

            'find next approver with open status
            ds = SupportModule.GetSupportRequestApproval(ViewState("jnid"), 0, 0, 1)
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TeamMemberID") > 0 Then

                        'get next approver details
                        iRowID = ds.Tables(0).Rows(0).Item("RowID")
                        iNotifyTeamMemberID = ds.Tables(0).Rows(0).Item("TeamMemberID")
                        iRoutingLevel = ds.Tables(0).Rows(0).Item("RoutingLevel")

                        'set that approver to in-process
                        SupportModule.UpdateSupportRequestApproval(iNotifyTeamMemberID, iRoutingLevel, "", 2, iRowID)

                        dsTeamMember = SecurityModule.GetTeamMember(iNotifyTeamMemberID, "", "", "", "", "", True, Nothing)
                        strEmailToAddress = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString()

                        'notify
                        BuildEmailToForwardApproval(strEmailToAddress)

                    End If
                End If
            Else
                'if no more approvers, then notify Team Member assigned to work on the request that all is approved
                BuildEmailToComplete()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ResetApproval()

        Try
            lblMessage.Text = ""

            Dim ds As DataSet
            Dim iRowID As Integer = 0
            Dim iTeamMemberID As Integer = 0
            Dim iRoutingLevel As Integer = 0
            Dim strComments As String = ""

            Dim strEmailToAddress As String = ""

            'parse all approvers - reset back to open IF AFTER CURRENT REJECTED TEAM MEMBER
            ds = SupportModule.GetSupportRequestApproval(ViewState("jnid"), 0, 0, 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                    For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                        iRowID = ds.Tables(0).Rows(iRowCounter).Item("RowID")
                        iTeamMemberID = ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID")
                        iRoutingLevel = ds.Tables(0).Rows(iRowCounter).Item("RoutingLevel")
                        strComments = ds.Tables(0).Rows(iRowCounter).Item("Comments")

                        If iRoutingLevel > ViewState("RoutingLevel") Then
                            SupportModule.UpdateSupportRequestApproval(iTeamMemberID, iRoutingLevel, strComments, 1, iRowID)
                        End If
                    Next
                End If           
            End If

            'notify assigned to that request has been rejected
            BuildEmailToResetApproval()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnStatusSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStatusSubmit.Click

        Try
            lblMessage.Text = ""

            Dim iApprovalStatusID As Integer = 0
            If ddApprovalStatus.SelectedIndex > 0 Then
                iApprovalStatusID = ddApprovalStatus.SelectedValue
            End If

            If iApprovalStatusID = 3 Or iApprovalStatusID = 4 Then
                SupportModule.UpdateSupportRequestApproval(ViewState("ApproverID"), ViewState("RoutingLevel"), txtApprovalComment.Text.Trim, iApprovalStatusID, ViewState("ApproverRowID"))

                'only change status is approved or rejected
                Select Case iApprovalStatusID
                    Case 3 'approve                    
                        ForwardApproval()
                    Case 4 'reject
                        ResetApproval()                      
                End Select

                EnableControls()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub SendEmail(ByVal EmailToAddress As String, ByVal EmailCCAddress As String, _
    ByVal EmailSubject As String, ByVal EmailBody As String, ByVal RecordId As String)

        Try

            Dim strSubject As String = ""
            Dim strBody As String = ""
            Dim dsSupportingDocList As DataSet
            Dim strSupportingDocURL As String = ""
            Dim iRowCounter As Integer = 0

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
       
            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            Dim strEmailToAddress As String = commonFunctions.CleanEmailList(EmailToAddress)
            Dim strEmailCCAddress As String = commonFunctions.CleanEmailList(EmailCCAddress & ";" & strEmailFromAddress)

            'handle test environment
            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br /><br />"
            End If

            strSubject &= EmailSubject
            strBody &= EmailBody

            If ViewState("jnId") > 0 Then

                strSupportingDocURL = strProdOrTestEnvironment & "Workflow/Supporting_Doc_Viewer.aspx?jnId=" & ViewState("jnId") & "&RowID="
                dsSupportingDocList = SupportModule.GetSupportingDoc(0, ViewState("jnId"))
                If commonFunctions.CheckDataSet(dsSupportingDocList) = True Then

                    strBody &= "<br /><br /><font size='1' face='Verdana'>Supporting Documents</font>"
                    strBody &= "<table style='border: 1px solid #D0D0BF; width: 100%'>"

                    For iRowCounter = 0 To dsSupportingDocList.Tables(0).Rows.Count - 1
                        strBody &= "<tr>"
                        strBody &= "<td align='left'><font size='1' face='Verdana'><a href=" & strSupportingDocURL & dsSupportingDocList.Tables(0).Rows(iRowCounter).Item("RowID").ToString & ">" & dsSupportingDocList.Tables(0).Rows(iRowCounter).Item("SupportingDocName") & "</a></font></td>"
                        strBody &= "</tr>"
                    Next

                    strBody &= "</table>"
                End If

            End If

            strBody &= "<br /><br /><font size='1' face='Verdana'>"
            strBody &= "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++"
            strBody &= "<br />If you are receiving this by error, please contact the Business Systems Group."
            strBody &= "<br />Please <u>do not</u> reply back to this email because you will not receive a response."
            strBody &= "<br />Please use a separate email or phone call the appropriate UGN Team Member(s) for any questions or concerns you may have.<br />"
            strBody &= "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++</font>"

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody &= "<br /><br />Email To Address List: " & EmailToAddress & "<br />"
                strBody &= "<br /><br />Email CC Address List: " & EmailCCAddress & "<br />"

                strEmailToAddress = "Roderick.Carlson@ugnauto.com"
                strEmailCCAddress = ""
            End If

            ' ''**********************************
            ' ''Connect & Send email notification
            ' ''**********************************
            'Try
            '    commonFunctions.Email.Send("", strEmailFromAddress, strSubject, strBody, strEmailToAddress, strEmailCCAddress, "", "Support", RecordId)
            '    lblMessage.Text = "Notification sent."
            'Catch ex As SmtpException
            '    lblMessage.Text &= "<br />Email Notification queued."
            '    UGNErrorTrapping.InsertEmailQueue("Support", strEmailFromAddress, EmailToAddress, strEmailCCAddress, strSubject, strBody, "")
            'End Try


            'create the mail message using new System.Net.Mail (not CDonts)
            Dim mail As New MailMessage()

            'send the message 
            'set the content
            mail.Subject = strSubject
            mail.Body = strBody

            'set the addresses
            mail.From = New MailAddress(strEmailFromAddress)
            Dim i As Integer

            'build email To list
            Dim emailList As String() = strEmailToAddress.Split(";")

            For i = 0 To UBound(emailList)
                If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                    mail.To.Add(emailList(i))
                End If
            Next i

            If InStr(strProdOrTestEnvironment, "Prod_", CompareMethod.Text) > 0 Then
                'build email CC List
                If strEmailCCAddress IsNot Nothing Then
                    emailList = strEmailCCAddress.Split(";")

                    For i = 0 To UBound(emailList)
                        If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                            mail.CC.Add(emailList(i))
                        End If
                    Next i
                End If

                mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
            End If

            mail.IsBodyHtml = True

            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(Mail)
                lblMessage.Text &= "<br />Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "<br />Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("Support", strEmailFromAddress, EmailToAddress, strEmailCCAddress, strSubject, strBody, "")
            End Try

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub ddTeamMember_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddTeamMember.SelectedIndexChanged

        Try
            Dim iTeamMemberID As Integer = 0

            If ddTeamMember.SelectedIndex >= 0 Then
                iTeamMemberID = ddTeamMember.SelectedValue
                BindData(iTeamMemberID)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnStatusReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStatusReset.Click
        Try

            BindData(0)

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvSupportingDoc_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSupportingDoc.DataBound

        Try

            If gvSupportingDoc.HeaderRow IsNot Nothing Then
                gvSupportingDoc.HeaderRow.Cells(0).Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
