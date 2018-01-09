' ************************************************************************************************
'
' Name:		Support_Detail_Control.ascs
' Purpose:	This Code Behind is for the Workflow Support Detail page. This is used in both the popup and the page that contains the master page
'
' Date		    Author	    
' 12/13/2011    Roderick Carlson
' 03/07/2012    Roderick Carlson - bold email description
' 05/01/2012    Roderick Carlson - made comment bold and put before description, made all fonts the same size
' 10/16/2012    Roderick Carlson - add ajax to prevent script or sql injection - cleanup br
' 10/24/2012    Roderick Carlson - add warning to DB Module Dropdown 
' 01/31/2013    Roderick Carlson - add approval routing
' ************************************************************************************************

Partial Class Support_Detail_Control
    Inherits System.Web.UI.UserControl

    Private Sub EnableForwardApprovalButton()

        Try

            Dim ds As DataSet

            btnForwardApproval.Visible = False

            'at least 1 row is open
            If ViewState("jnId") > 0 Then
                ds = SupportModule.GetSupportRequestApproval(ViewState("jnId"), 1, 0, 1)
                If commonFunctions.CheckDataSet(ds) = True Then
                    btnForwardApproval.Visible = ViewState("isAdmin")
                Else
                    'at least 1 row is rejected
                    ds = SupportModule.GetSupportRequestApproval(ViewState("jnId"), 0, 0, 4)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        btnForwardApproval.Visible = ViewState("isAdmin")
                    End If
                End If
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

    Private Sub EnableControls()

        Try

            ddStatus.Enabled = ViewState("isAdmin")
            ddAssignedTo.Enabled = ViewState("isAdmin")
            ddRequestBy.Enabled = ViewState("isAdmin")

            lblActualHours.Visible = ViewState("isAdmin")
            lblEstimatedHours.Visible = ViewState("isAdmin")

            txtActualHours.Visible = ViewState("isAdmin")
            txtEstimatedHours.Visible = ViewState("isAdmin")
            txtComments.Enabled = ViewState("isAdmin")

            btnDelete.Visible = False
            btnForwardApproval.Visible = False
            btnNotify.Visible = False
            btnUpdate.Visible = False
            btnPreviewBottom.Visible = False
            btnPreviewTop.Visible = False

            gvApprovals.Columns(0).Visible = False
            gvApprovals.ShowFooter = False

            If ViewState("jnId") > 0 And ViewState("isAdmin") Then
                btnSubmit.Visible = False

                tblHoursRow.Visible = ViewState("isAdmin")
            
                btnDelete.Visible = ViewState("isAdmin")
                btnNotify.Visible = ViewState("isAdmin")
                btnUpdate.Visible = ViewState("isAdmin")

                EnableForwardApprovalButton()

                gvApprovals.Columns(0).Visible = ViewState("isAdmin")
                gvApprovals.ShowFooter = ViewState("isAdmin")

                If ViewState("isAdmin") = True Then
                    hlnkApprovalPage.NavigateUrl = "crSupport_Approval.aspx?JobNumber=" & ViewState("JobNumber")
                    hlnkApprovalPage.Visible = True
                End If

            Else
                btnSubmit.Visible = True
            End If

            If ViewState("jnId") > 0 Then
                btnPreviewBottom.Visible = True
                btnPreviewTop.Visible = True

                Dim strPreviewClientScript As String = "javascript:void(window.open('crSupport_Preview.aspx?JobNumber=" & ViewState("JobNumber") & " '," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=yes,location=yes'));"
                btnPreviewBottom.Attributes.Add("Onclick", strPreviewClientScript)
                btnPreviewTop.Attributes.Add("Onclick", strPreviewClientScript)

                accAdmin.Visible = True

                If ddAssignedTo.SelectedIndex >= 0 Or txtComments.Text.Trim <> "" Then
                    accAdmin.SelectedIndex = 0
                End If
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

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ds = commonFunctions.GetTeamMember("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddRequestBy.DataSource = ds
                ddRequestBy.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName
                ddRequestBy.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddRequestBy.DataBind()
                ddRequestBy.Items.Insert(0, "")
            End If

            ds = SupportModule.GetAssignedTo()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddAssignedTo.DataSource = ds
                ddAssignedTo.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName
                ddAssignedTo.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddAssignedTo.DataBind()
                ddAssignedTo.Items.Insert(0, "")
            End If

            ds = SupportModule.GetModule("", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddModule.DataSource = ds
                ddModule.DataTextField = ds.Tables(0).Columns("Description").ColumnName
                ddModule.DataValueField = ds.Tables(0).Columns("DBMID").ColumnName
                ddModule.DataBind()
                ddModule.Items.Insert(0, "")
            End If

            ds = SupportModule.GetCategory("", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCategory.DataSource = ds
                ddCategory.DataTextField = ds.Tables(0).Columns("Category").ColumnName
                ddCategory.DataValueField = ds.Tables(0).Columns("DBCID").ColumnName
                ddCategory.DataBind()
                'ddCategory.Items.Insert(0, "")
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

    Private Sub BindNewRequestData()

        Try

            'Dim strUser As String = ""
            'Dim strEmail As String = ""

            Dim strSystemDetails As New StringBuilder

            If Session("SupportUrl") IsNot Nothing Then
                strSystemDetails.AppendLine("<br />URL:" & Session("SupportUrl").ToString)
            End If

            If Session("SupportQueryString") IsNot Nothing Then
                strSystemDetails.AppendLine("<br />QueryString:" & Session("SupportQueryString").ToString)
            End If

            lblSystemDetails.Text = strSystemDetails.ToString

            If ViewState("TeamMemberID") > 0 Then
                If ddRequestBy.Items.FindByValue(ViewState("TeamMemberID")) IsNot Nothing Then
                    ddRequestBy.SelectedValue = ViewState("TeamMemberID")
                End If
            Else
                ddRequestBy.Enabled = True
            End If

            'If Response.Cookies("UGNDB_User") IsNot Nothing Then
            '    strUser = Response.Cookies("UGNDB_User").Value
            'End If

            'If Response.Cookies("UGNDB_User_Email") IsNot Nothing Then
            '    strEmail = Response.Cookies("UGNDB_User_Email").Value
            'End If

            Dim strBMID As String = ""

            If Request.QueryString("BMID") IsNot Nothing Then
                strBMID = Request.QueryString("BMID").ToString

                If ddModule.Items.FindByValue(strBMID) IsNot Nothing Then
                    ddModule.SelectedValue = strBMID
                End If
            End If

            If ddCategory.Items.FindByValue(3) IsNot Nothing Then
                ddCategory.SelectedValue = 3
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
    Private Sub CheckRights()

        Try

            ViewState("TeamMemberID") = 0
            ViewState("TeamMemberEmail") = ""

            Dim strFullName As String = commonFunctions.getUserName()
            Dim UserEmailAddress As String = strFullName & "@ugnauto.com"
            ViewState("TeamMemberEmail") = UserEmailAddress

            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Or Response.Cookies("UGNDB_UserFullName") Is Nothing Then

                Response.Cookies("UGNDB_User_Email").Value = UserEmailAddress

                If strFullName = Nothing Then
                    strFullName = "Demo.Demo"  '* This account has restricted read only rights.
                End If

                Dim LocationOfDot As Integer = InStr(strFullName, ".")
                If LocationOfDot > 0 Then
                    Dim FirstName As String = Left(strFullName, LocationOfDot - 1)
                    Dim FirstInitial As String = Left(strFullName, 1)
                    Dim LastName As String = Right(strFullName, Len(strFullName) - LocationOfDot)

                    Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
                    Response.Cookies("UGNDB_UserFullName").Value = Mid(FirstName, 1, 1).ToUpper & Mid(FirstName, 2, Len(FirstName) - 1) & " " & Mid(LastName, 1, 1).ToUpper & Mid(LastName, 2, Len(LastName) - 1)
                Else
                    Response.Cookies("UGNDB_User").Value = strFullName
                    Response.Cookies("UGNDB_UserFullName").Value = strFullName

                End If
            End If

            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("isAdmin") = False
            ' ViewState("isEdit") = True 'everyone else can edit
        
            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                'If iTeamMemberID = 530 Then
                '    iTeamMemberID = 246 ' Mike Echevarria
                'End If

                ViewState("TeamMemberID") = iTeamMemberID

                'get team member with Admin rights to the UGNDB Team Member Maint page
                'dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 3)
                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 70)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True                        
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                        Case 13 '*** UGNAssist: Create/Edit/No Delete

                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete

                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select

                End If
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

    Private Sub InitializeViewState()

        Try

            ViewState("isAdmin") = False
            'ViewState("isEdit") = False

            ViewState("TeamMemberID") = 0
            ViewState("TeamMemberEmail") = ""

            ViewState("JobNumber") = ""
            ViewState("jnId") = 0

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindData()

        Try
            Dim ds As DataSet
            Dim dsTeamMember As DataSet

            Dim strRequestBy As String = ""
            Dim strAssignedTo As String = ""

            'get values of Requestor already exists
            If ViewState("JobNumber") <> "" Then

                ds = SupportModule.GetSupportRequest(ViewState("JobNumber"))

                If commonFunctions.CheckDataSet(ds) = True Then

                    lblRequestDate.Text = ds.Tables(0).Rows(0).Item("RequestDate").ToString
                    lblCompletionDate.Text = ds.Tables(0).Rows(0).Item("DateCompleted").ToString

                    If ds.Tables(0).Rows(0).Item("jnId") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("jnId") > 0 Then
                            ViewState("jnId") = ds.Tables(0).Rows(0).Item("jnId")
                            lblJnId.Text = ds.Tables(0).Rows(0).Item("jnId")
                        End If
                    End If

                    lblJobNumber.Text = ds.Tables(0).Rows(0).Item("JobNumber").ToString.Trim
                    ddStatus.SelectedValue = ds.Tables(0).Rows(0).Item("Status").ToString.Trim

                    strRequestBy = ds.Tables(0).Rows(0).Item("RequestBy").ToString.Trim
                    lblRequestBy.Text = strRequestBy
                    ddRequestBy.Enabled = True
                    If strRequestBy <> "" Then
                        dsTeamMember = SupportModule.GetTeamMemberByString(strRequestBy)

                        If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                            If dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                If dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                    ddRequestBy.SelectedValue = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                                    lblRequestBy.Visible = False
                                End If
                            End If
                        End If

                        If ddRequestBy.SelectedIndex = -1 Then
                            If ViewState("isAdmin") = False Then
                                ddRequestBy.SelectedValue = ViewState("TeamMemberID")
                            End If
                        End If

                        'If ddTeamMember.Items.FindByText(strRequestBy) IsNot Nothing Then
                        '    ddTeamMember.SelectedItem.Text = strRequestBy

                        '    lblRequestBy.Visible = False
                        'Else
                        '    'if current user is NOT Admin, then update requester team member
                        '    If ViewState("isAdmin") = False Then
                        '        ddTeamMember.SelectedValue = ViewState("TeamMemberID")
                        '    End If
                        'End If
                    End If

                    If ds.Tables(0).Rows(0).Item("DBCID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("DBCID") > 0 Then
                            ddCategory.SelectedValue = ds.Tables(0).Rows(0).Item("DBCID")
                        End If
                    End If

                    ddRelatedTo.SelectedValue = ds.Tables(0).Rows(0).Item("RelatedTo").ToString.Trim
                    ddModule.SelectedValue = ds.Tables(0).Rows(0).Item("DBMID").ToString.Trim
                    txtDesc.Text = ds.Tables(0).Rows(0).Item("JobDescription").ToString.Trim

                    strAssignedTo = ds.Tables(0).Rows(0).Item("AssignedTo").ToString.Trim
                    lblAssignedTo.Text = strAssignedTo

                    dsTeamMember = SupportModule.GetTeamMemberByString(strAssignedTo)

                    If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                        If dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                            If dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                ddAssignedTo.SelectedValue = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                                lblAssignedTo.Visible = False
                            End If
                        End If
                    End If

                    'If ddAssignedTo.Items.FindByText(strTeamMember) IsNot Nothing Then
                    '    ddAssignedTo.SelectedItem.Text = strTeamMember
                    '    lblAssignedTo.Visible = False
                    'Else
                    '    ddAssignedTo.SelectedIndex = -1
                    '    'ddAssignedTo.Enabled = True
                    'End If

                    If ds.Tables(0).Rows(0).Item("ActualHours") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualHours") <> 0 Then
                            txtActualHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualHours"), "0.##")
                        End If
                    End If

                    If ds.Tables(0).Rows(0).Item("EstimatedHours") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("EstimatedHours") <> 0 Then
                            txtEstimatedHours.Text = Format(ds.Tables(0).Rows(0).Item("EstimatedHours"), "0.##")
                        End If
                    End If

                    txtComments.Text = ds.Tables(0).Rows(0).Item("Notes").ToString.Trim

                End If
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

    Private Sub HandleMultilineFields()

        Try

            txtDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtDesc.Attributes.Add("onkeyup", "return tbCount(" + lblDescCharCount.ClientID + ");")
            txtDesc.Attributes.Add("maxLength", "2000")

            txtSupportingDocDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtSupportingDocDesc.Attributes.Add("onkeyup", "return tbCount(" + lblSupportingDocDescCharCount.ClientID + ");")
            txtSupportingDocDesc.Attributes.Add("maxLength", "200")

            txtComments.Attributes.Add("onkeypress", "return tbLimit();")
            txtComments.Attributes.Add("onkeyup", "return tbCount(" + lblCommentsCharCount.ClientID + ");")
            txtComments.Attributes.Add("maxLength", "4000")

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then
                InitializeViewState()

                CheckRights()

                BindCriteria()

                If HttpContext.Current.Request.QueryString("JobNumber") <> "" Then
                    ViewState("JobNumber") = HttpContext.Current.Request.QueryString("JobNumber")
                    BindData()
                Else
                    BindNewRequestData()
                End If

                accSupportingDoc.SelectedIndex = -1
                accAdmin.SelectedIndex = -1

                EnableControls()

                HandleMultilineFields()

                ddModule.Attributes.Add("onchange", "alert('WARNING: Please make sure to include the ID of the record in question and steps taken that can reproduce any errors.')")
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

    Private Sub SubmitNotification()

        Try

            If ViewState("JobNumber") <> "" Then

                Dim strEmailToAddress As String = ""
                Dim strEmailCCAddress As String = ""
                Dim strEmailSubject As String = ""
                Dim strEmailBody As String = ""

                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                Dim strEmailURL As String = strProdOrTestEnvironment & "Workflow/Support_Detail.aspx?JobNumber=" & ViewState("JobNumber")

                Dim iTeamMemberID As Integer = 0
                If ddRequestBy.SelectedIndex >= 0 Then
                    iTeamMemberID = ddRequestBy.SelectedValue
                End If

                Dim dsTeamMember As DataSet
                Dim strTeamMemberEmail As String = ""
                dsTeamMember = SecurityModule.GetTeamMember(iTeamMemberID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)

                If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                    If dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim <> "" Then
                        strTeamMemberEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim
                    End If
                End If

                'assign email subject
                strEmailSubject = "SUPPORT REQUEST: " & ViewState("JobNumber") & " is ready for review"

                'build email body
                strEmailBody = "<font size='2' face='Verdana'>The following support request is ready for review by the UGN Business Systems Group:</font><br /><br />"

                strEmailBody &= "<font size='2' face='Verdana' color='red'>Status: <b>" & ddStatus.SelectedValue & "</b></font><br /><br />"
                strEmailBody &= "<font size='2' face='Verdana'>Support Request ID: <b>" & ViewState("JobNumber") & "</b></font><br /><br />"

                If txtComments.Text.Trim <> "" Then
                    strEmailBody &= "<font size='2' face='Verdana'>Comments:<b> " & txtComments.Text.Trim & "</b></font><br /><br />"
                End If

                If txtDesc.Text.Trim <> "" Then
                    strEmailBody &= "<font size='2' face='Verdana'>Description: " & txtDesc.Text.Trim & "</font><br /><br />"
                End If

                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & "'>Click here to review</a></font><br /><br />"

                'send the actual email
                SendEmail("Lynette.Rey@ugnauto.com", strTeamMemberEmail, strEmailSubject, strEmailBody, ViewState("JobNumber"))
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

                Dim iTeamMemberID As Integer = 0
                If ddAssignedTo.SelectedIndex >= 0 Then
                    iTeamMemberID = ddAssignedTo.SelectedValue
                End If

                Dim dsTeamMember As DataSet
                Dim strTeamMemberEmail As String = ""
                dsTeamMember = SecurityModule.GetTeamMember(iTeamMemberID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)

                If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                    If dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim <> "" Then
                        strTeamMemberEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim
                    End If
                End If

                'assign email subject
                strEmailSubject = "SUPPORT REQUEST: " & ViewState("JobNumber") & " is ready for your approval"

                'build email body
                strEmailBody = "<font size='2' face='Verdana'>The following support request is ready for your approval:</font><br /><br />"

                strEmailBody &= "<font size='2' face='Verdana'>Support Request ID: <b>" & ViewState("JobNumber") & "</b></font><br /><br />"

                If txtComments.Text.Trim <> "" Then
                    strEmailBody &= "<font size='2' face='Verdana'>Comments:<b> " & txtComments.Text.Trim & "</b></font><br /><br />"
                End If

                If txtDesc.Text.Trim <> "" Then
                    strEmailBody &= "<font size='2' face='Verdana'>Description: " & txtDesc.Text.Trim & "</font><br /><br />"
                End If

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

    Private Sub UpdateAssignedTo()

        Try
            'if current users is admin, then update ddAssigned To
            If ViewState("isAdmin") = True And ddAssignedTo.SelectedIndex <= 0 Then
                If ddAssignedTo.Items.FindByValue(ViewState("TeamMemberID")) IsNot Nothing Then
                    ddAssignedTo.SelectedValue = ViewState("TeamMemberID")
                End If
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

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Try
            lblMessage.Text = ""

            Dim dActualHours As Double = 0
            Dim dEstimatedHours As Double = 0

            Dim iDBCID As Integer = 0
            If ddCategory.SelectedIndex > -1 Then
                iDBCID = ddCategory.SelectedValue
            End If

            If ViewState("jnId") = 0 Then
                ViewState("JobNumber") = SupportModule.InsertSupportDetail(iDBCID, ddModule.SelectedValue, ddRequestBy.SelectedItem.Text.Trim, txtDesc.Text.Trim, ddRelatedTo.SelectedValue).Trim

                If ViewState("isAdmin") = False Then
                    SubmitNotification()
                End If

            Else
                UpdateAssignedTo

                If txtActualHours.Text.Trim <> "" Then
                    dActualHours = CType(txtActualHours.Text.Trim, Double)
                End If

                If txtEstimatedHours.Text.Trim <> "" Then
                    dEstimatedHours = CType(txtEstimatedHours.Text.Trim, Double)
                End If

                ViewState("JobNumber") = SupportModule.UpdateSupportDetail(ViewState("jnId"), iDBCID, ddModule.SelectedValue, ddRequestBy.SelectedItem.Text.Trim, ddAssignedTo.SelectedItem.Text.Trim, txtDesc.Text.Trim, txtComments.Text.Trim, ddStatus.SelectedValue, ddRelatedTo.SelectedValue, dActualHours, dEstimatedHours).Trim

                If ViewState("isAdmin") = False Then
                    NotifyUpdate()
                End If

            End If

            BindData()

            EnableControls()

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        Try
            lblMessage.Text = ""

            Dim dActualHours As Double = 0
            Dim dEstimatedHours As Double = 0

            Dim iDBCID As Integer = 0
            If ddCategory.SelectedIndex > -1 Then
                iDBCID = ddCategory.SelectedValue
            End If

            If ViewState("jnId") > 0 Then
                UpdateAssignedTo()

                If txtActualHours.Text.Trim <> "" Then
                    dActualHours = CType(txtActualHours.Text.Trim, Double)
                End If

                If txtEstimatedHours.Text.Trim <> "" Then
                    dEstimatedHours = CType(txtEstimatedHours.Text.Trim, Double)
                End If

                ViewState("JobNumber") = SupportModule.UpdateSupportDetail(ViewState("jnId"), iDBCID, ddModule.SelectedValue, ddRequestBy.SelectedItem.Text.Trim, ddAssignedTo.SelectedItem.Text.Trim, txtDesc.Text.Trim, txtComments.Text.Trim, ddStatus.SelectedValue, ddRelatedTo.SelectedValue, dActualHours, dEstimatedHours).Trim

                BindData()

                EnableControls()

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

    Private Sub NotifyUpdate()

        Try

            If ViewState("JobNumber") <> "" Then

                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                Dim strEmailURL As String = strProdOrTestEnvironment & "Workflow/Support_Detail.aspx?JobNumber=" & ViewState("JobNumber")

                Dim strEmailSubject As String = ""
                Dim strEmailBody As String = ""

                Dim iTeamMemberID As Integer = 0
                If ddRequestBy.SelectedIndex >= 0 Then
                    iTeamMemberID = ddRequestBy.SelectedValue
                End If

                Dim dsTeamMember As DataSet
                Dim strTeamMemberEmail As String = ""
                dsTeamMember = SecurityModule.GetTeamMember(iTeamMemberID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)

                If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                    If dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim <> "" Then
                        strTeamMemberEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString.Trim
                    End If
                End If

                'assign email subject
                strEmailSubject = "SUPPORT REQUEST: " & ViewState("JobNumber") & " is has been updated."

                'build email body
                strEmailBody = "<font size='2' face='Verdana'>The following support request has been updated.</font><br /><br />"

                strEmailBody &= "<font size='2' face='Verdana' color='red'>Status: <b>" & ddStatus.SelectedValue & "</b></font><br /><br />"
                strEmailBody &= "<font size='2' face='Verdana'>Support Request ID: <b>" & ViewState("JobNumber") & "</b></font><br /><br />"

                If txtComments.Text.Trim <> "" Then
                    strEmailBody &= "<font size='2' face='Verdana'>Comments:<b> " & txtComments.Text.Trim & "</b></font><br /><br />"
                End If

                If txtDesc.Text.Trim <> "" Then
                    strEmailBody &= "<font size='2' face='Verdana'>Description: " & txtDesc.Text.Trim & "</font><br /><br />"
                End If

                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & "'>Click here to review</a></font><br /><br />"

                'send the actual email
                SendEmail(strTeamMemberEmail, "Lynette.Rey@ugnauto.com", strEmailSubject, strEmailBody, ViewState("JobNumber"))
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


    Protected Sub btnNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNotify.Click

        Try
            lblMessage.Text = ""

            NotifyUpdate()

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
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
            'Dim strEmailURL As String = strProdOrTestEnvironment & "Workflow/Support_Detail.aspx?JobNumber=" & ViewState("JobNumber")

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            Dim strEmailToAddress As String = EmailToAddress
            Dim strEmailCCAddress As String = EmailCCAddress

            'handle test environment
            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br /><br />"
            End If

            strSubject &= EmailSubject
            strBody &= EmailBody

            'strBody &= "<font size='2' face='Verdana' color='red'>Status: <b>" & ddStatus.SelectedValue & "</b></font><br /><br />"
            'strBody &= "<font size='2' face='Verdana'>Support Request ID: <b>" & ViewState("JobNumber") & "</b></font><br /><br />"

            'If txtComments.Text.Trim <> "" Then
            '    strBody &= "<font size='2' face='Verdana'>Comments:<b> " & txtComments.Text.Trim & "</b></font><br /><br />"
            'End If

            'If txtDesc.Text.Trim <> "" Then
            '    strBody &= "<font size='2' face='Verdana'>Description: " & txtDesc.Text.Trim & "</font><br /><br />"
            'End If

            'strBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
            'strBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & "'>Click here to review</a></font><br /><br />"

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

                strEmailToAddress = "Lynette.Rey@ugnauto.com"
                strEmailCCAddress = ""
            End If

            ''**********************************
            ''Connect & Send email notification
            ''**********************************
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
            Mail.Subject = strSubject
            Mail.Body = strBody

            'set the addresses
            Mail.From = New MailAddress(strEmailFromAddress)
            Dim i As Integer

            'build email To list
            Dim emailList As String() = strEmailToAddress.Split(";")

            For i = 0 To UBound(emailList)
                If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                    Mail.To.Add(emailList(i))
                End If
            Next i

            If InStr(strProdOrTestEnvironment, "Prod_", CompareMethod.Text) > 0 Then
                'build email CC List
                If strEmailCCAddress IsNot Nothing Then
                    emailList = strEmailCCAddress.Split(";")

                    For i = 0 To UBound(emailList)
                        If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                            Mail.CC.Add(emailList(i))
                        End If
                    Next i
                End If

                mail.Bcc.Add("Lynette.Rey@ugnauto.com")
            End If

            Mail.IsBodyHtml = True

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

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        Try
            lblMessage.Text = ""

            If ViewState("isAdmin") = True And ViewState("jnId") > 0 Then
                SupportModule.DeleteSupportRequest(ViewState("jnId"))

                Response.Redirect("Support_List.aspx", False)
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

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click

        Try
            lblMessage.Text = ""

            'create the ID if none exists
            If ViewState("jnId") = 0 Then
                btnSubmit_Click(sender, e)
            End If

            If ViewState("jnId") > 0 Then
                If fileUploadSupportingDoc.HasFile Then
                    If fileUploadSupportingDoc.PostedFile.ContentLength <= 3500000 Then
                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(fileUploadSupportingDoc.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(fileUploadSupportingDoc.PostedFile.FileName)

                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(fileUploadSupportingDoc.PostedFile.InputStream.Length)

                        Dim SupportingDocEncodeType As String = fileUploadSupportingDoc.PostedFile.ContentType

                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        fileUploadSupportingDoc.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".xlsx") Or (FileExt = ".doc") Or (FileExt = ".docx") Or (FileExt = ".jpeg") Or (FileExt = ".jpg") Or (FileExt = ".tif") Or (FileExt = ".msg") Or (FileExt = ".ppt") Then

                            ''***************
                            '' Insert Record
                            ''***************
                            SupportModule.InsertSupportingDoc(ViewState("jnId"), fileUploadSupportingDoc.FileName, txtSupportingDocDesc.Text.Trim, SupportingDocBinaryFile, SupportingDocFileSize, SupportingDocEncodeType, cbSignatureReq.Checked)

                            revUploadFile.Enabled = False

                            lblMessage.Text &= "<br />File Uploaded Successfully<br />"

                            gvSupportingDoc.DataBind()
                            gvSupportingDoc.Visible = True

                            revUploadFile.Enabled = True

                            txtSupportingDocDesc.Text = ""
                        End If

                    Else
                        lblMessage.Text = "File exceeds size limit.  Please select a file less than 3MB (3000KB)."
                    End If
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

    Protected Sub gvSupportingDoc_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvSupportingDoc.RowDeleted

        Try
            lblMessage.Text = ""

            gvSupportingDoc.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ftbeDesc_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles ftbeDesc.Init
        ftbeDesc.ValidChars = ftbeDesc.ValidChars & vbCrLf
    End Sub

    Protected Sub ftbeSupportingDocDesc_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles ftbeSupportingDocDesc.Init
        ftbeSupportingDocDesc.ValidChars = ftbeSupportingDocDesc.ValidChars & vbCrLf
    End Sub

    Protected Sub ftbeComments_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles ftbeComments.Init
        ftbeComments.ValidChars = ftbeComments.ValidChars & vbCrLf
    End Sub

    Private Property LoadDataEmpty_Approvals() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Approvals") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Approvals"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Approvals") = value
        End Set

    End Property
    Protected Sub gvApprovals_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvApprovals.RowCommand

        Try

            Dim ddTeamMemberTemp As DropDownList
            
            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("jnId") <> 0) Then

                ddTeamMemberTemp = CType(gvApprovals.FooterRow.FindControl("ddInsertTeamMember"), DropDownList)

                If ddTeamMemberTemp.SelectedIndex > 0 Then
                    odsApprovals.InsertParameters("jnId").DefaultValue = ViewState("jnId")
                    odsApprovals.InsertParameters("TeamMemberID").DefaultValue = ddTeamMemberTemp.SelectedValue

                    intRowsAffected = odsApprovals.Insert()

                    lblMessage.Text = "Record Saved Successfully.<br />"
                    gvApprovals.DataBind()

                    EnableForwardApprovalButton()
                Else
                    lblMessage.Text = "Error: A team member must be selected. The record was NOT saved.<br />"
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvApprovals.ShowFooter = False
            Else
                gvApprovals.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddTeamMemberTemp = CType(gvApprovals.FooterRow.FindControl("ddInsertTeamMember"), DropDownList)
                ddTeamMemberTemp.SelectedIndex = -1
            End If


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub odsApprovals_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsApprovals.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            Dim dsLocal As DataSet = CType(e.ReturnValue, DataSet)
            Dim ds As DataSet = SupportModule.GetSupportRequestApproval(ViewState("jnId"), 0, 0, 0)
            If commonFunctions.CheckDataSet(ds) = False Then         
                dsLocal.Tables(0).Rows.Add(dsLocal.Tables(0).NewRow)
                LoadDataEmpty_Approvals = True                
            Else
                LoadDataEmpty_Approvals = False                
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

    Protected Sub gvApprovals_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApprovals.RowCreated

        Try

            'If e.Row.RowType = DataControlRowType.Footer Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            '    e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            'End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Approvals
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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

    Protected Sub gvApprovals_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApprovals.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim txtEditComments As TextBox = TryCast(e.Row.FindControl("txtEditComments"), TextBox)
                Dim lblEditCommentsCharCount As Label = TryCast(e.Row.FindControl("lblEditCommentsCharCount"), Label)

                If txtEditComments IsNot Nothing Then
                    txtEditComments.Attributes.Add("onkeypress", "return tbLimit();")
                    txtEditComments.Attributes.Add("onkeyup", "return tbCount(" + lblEditCommentsCharCount.ClientID + ");")
                    txtEditComments.Attributes.Add("maxLength", "100")
                End If

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
            ds = SupportModule.GetSupportRequestApproval(ViewState("jnId"), 0, 0, 1)
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TeamMemberID") > 0 Then

                        'get next approver detaisl
                        iRowID = ds.Tables(0).Rows(0).Item("RowID")
                        iNotifyTeamMemberID = ds.Tables(0).Rows(0).Item("TeamMemberID")
                        iRoutingLevel = ds.Tables(0).Rows(0).Item("RoutingLevel")

                        'set that approver to in-process
                        SupportModule.UpdateSupportRequestApproval(iNotifyTeamMemberID, iRoutingLevel, "", 2, iRowID)

                        dsTeamMember = SecurityModule.GetTeamMember(iNotifyTeamMemberID, "", "", "", "", "", True, Nothing)
                        strEmailToAddress = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString()

                        'notify
                        BuildEmailToForwardApproval(strEmailToAddress)

                        'refresh grid
                        gvApprovals.DataBind()

                        'hide forward button
                        EnableForwardApprovalButton()
                    End If
                End If
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

    Protected Sub btnForwardApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnForwardApproval.Click

        Try

            'reset all to open
            UpdateHigherLevelApprover(0, 1)

            ForwardApproval()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvApprovals_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvApprovals.RowDeleted

        EnableForwardApprovalButton()

    End Sub

    Protected Sub gvApprovals_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvApprovals.RowUpdated

        Try


            'if changed to  open, in-process or reject, then others above reset to open
            If e.NewValues("StatusID") = 1 Or e.NewValues("StatusID") = 2 Or e.NewValues("StatusID") = 4 Then
                UpdateHigherLevelApprover(e.NewValues("RoutingLevel"), 1)
            End If

            If e.NewValues("StatusID") = 3 Then
                'if changed to approve, then next higher routing level set to in-process and all others above to open
                ForwardApproval()
            End If

            EnableForwardApprovalButton()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub UpdateHigherLevelApprover(ByVal CurrentRoutingLevel As Integer, ByVal NewStatusID As Integer)

        Try
            Dim ds As DataSet

            Dim iRowCounter As Integer = 0

            Dim iRowID As Integer = 0
            Dim iTeamMemberID As Integer = 0
            Dim iRoutingLevel As Integer = 0
            Dim strComments As String = ""

            If ViewState("jnId") > 0 Then
                ds = SupportModule.GetSupportRequestApproval(ViewState("jnId"), 0, 0, 0)
                If commonFunctions.CheckDataSet(ds) = True Then
                    For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                        iRowID = ds.Tables(0).Rows(iRowCounter).Item("RowID")
                        iTeamMemberID = ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID")
                        iRoutingLevel = ds.Tables(0).Rows(iRowCounter).Item("RoutingLevel")
                        strComments = ds.Tables(0).Rows(iRowCounter).Item("Comments")

                        If iRoutingLevel > CurrentRoutingLevel Then
                            SupportModule.UpdateSupportRequestApproval(iTeamMemberID, iRoutingLevel, strComments, NewStatusID, iRowID)
                        End If

                    Next
                End If
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
End Class
