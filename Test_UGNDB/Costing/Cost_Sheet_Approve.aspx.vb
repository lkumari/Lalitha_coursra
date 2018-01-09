' ***********************************************************************************************
'
' Name:		Costing_Cost_Sheet_Approve.aspx
' Purpose:	This Code Behind allows team members to approve cost sheets
'
' Date		Author	    
' 02/17/2009 Roderick Carlson  
' 11/12/2009 Roderick Carlson - do not send email to second level routing on APPROVALS IF ALL first levels have approved - They get a notification from another stored procedure announcing all levels have approved already
' 11/18/2009 Roderick Carlson - use RowID and CostSheetID as keys for approval
' 11/20/2009 Roderick Carlson - put subscription id and routing level as parameters, put all notifications in stored procedures, prevent approval or rejection per row if already done, hide search fields when editing a row in grid
' 12/21/2009 Roderick Carlson - allow user to switch between subscriptions
' 02/22/2012 Roderick Carlson - add part # to list
' ************************************************************************************************

Partial Class Costing_Cost_Sheet_Approve
    Inherits System.Web.UI.Page

    Private Sub ClearMessages()

        lblMessage.Text = ""
        lblMessageBottom.Text = ""

    End Sub

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet            

            ds = CostingModule.GetCostSheetSubscriptionByApprover(ViewState("TeamMemberID"), 0)
            If commonFunctions.CheckDataset(ds) = True Then
                ddSubscription.DataSource = ds
                ddSubscription.DataTextField = ds.Tables(0).Columns("Subscription").ColumnName.ToString()
                ddSubscription.DataValueField = ds.Tables(0).Columns("SubscriptionID").ColumnName
                ddSubscription.DataBind()

                'get most recent subscription usage
                ds = CostingModule.GetCostSheetTeamMemberRecentSubscription(ViewState("TeamMemberID"))
                If commonFunctions.CheckDataset(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("SubscriptionID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("SubscriptionID") > 0 Then
                            ddSubscription.SelectedValue = ds.Tables(0).Rows(0).Item("SubscriptionID")
                        End If
                    End If
                End If
            End If

            If ddSubscription.SelectedIndex > 0 Then
                ViewState("SubscriptionID") = ddSubscription.SelectedValue
            End If

            ''bind existing data to drop down Group List          
            ds = CostingModule.GetCostSheetApproverBySubscription(ViewState("SubscriptionID"), 0)
            If commonFunctions.CheckDataset(ds) = True Then
                ddSearchTeamMember.DataSource = ds
                ddSearchTeamMember.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName.ToString()
                ddSearchTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddSearchTeamMember.DataBind()          
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    'Protected Sub SendEmail(ByVal CostSheetID As String, ByVal SignedStatus As String, ByVal SignedStatusDesc As String, ByVal Comments As String)

    '    Try

    '        Dim dsTeamMember As DataSet
    '        Dim iCostingCoordinatorID As Integer = 0
    '        Dim dsCostingCoordinator As DataSet

    '        Dim strEmailToAddress As String = ""
    '        Dim strEmailCCAddress As String = ""
    '        Dim iRowCounter As Integer = 0
    '        Dim strSubject As String = ""
    '        Dim strBody As String = ""
    '        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

    '        'get current user name
    '        Dim strCurrentUser As String = commonFunctions.getUserName()

    '        Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"
    '        strEmailToAddress = strEmailFromAddress

    '        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
    '            strSubject = "TEST PLEASE DISREGARD: "
    '            strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br><br>"
    '            strBody += "<h1>This information is purely for testing and is NOT valid!!!</h1><br><br>"
    '        End If

    '        strSubject += "Cost Sheet: " & CostSheetID & " has been " & SignedStatusDesc & " by " & strCurrentUser

    '        ''create the mail message using new System.Net.Mail (not CDonts)
    '        Dim mail As New MailMessage()

    '        'get costing coordinator for this costsheet if rejected
    '        If SignedStatus = "R" Then
    '            dsCostingCoordinator = CostingModule.GetCostSheetPreApprovalList(CostSheetID, 0, 0, "", 41, False, False, False)
    '            If dsCostingCoordinator IsNot Nothing Then
    '                If dsCostingCoordinator.Tables.Count > 0 And dsCostingCoordinator.Tables.Item(0).Rows.Count > 0 Then
    '                    iCostingCoordinatorID = dsCostingCoordinator.Tables(0).Rows(0).Item("TeamMemberID")

    '                    If iCostingCoordinatorID > 0 Then
    '                        'get email of Team Member
    '                        dsTeamMember = SecurityModule.GetTeamMember(iCostingCoordinatorID, "", "", "", "", "", True, Nothing)
    '                        If dsTeamMember IsNot Nothing Then
    '                            If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
    '                                If dsTeamMember.Tables(0).Rows(0).Item("Email").ToString <> "" Then

    '                                    If strEmailToAddress <> "" Then
    '                                        strEmailToAddress += ";"
    '                                    End If

    '                                    strEmailToAddress += dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
    '                                End If

    '                            End If ' If dsTeamMember.Tables.Count > 0
    '                        End If ' If dsTeamMember IsNot Nothing 
    '                    End If
    '                End If
    '            End If
    '        End If

    '        strBody += "<font size='3' face='Verdana'>Cost Sheet: <b>" & CostSheetID & "</b> has been " & SignedStatusDesc & " by " & strCurrentUser & "</font><br><br>"

    '        If Comments.Trim <> "" Then
    '            strBody += "<font size='2' face='Verdana'>Comments: " & Comments & "</font><br>"
    '        End If


    '        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
    '            strBody += "<br><br>Email To Address List: " & strEmailToAddress & "<br>"
    '            'strBody += "<br>Email CC Address List: " & strEmailCCAddress & "<br>"

    '            strEmailToAddress = "Roderick.Carlson@ugnauto.com"
    '            'strEmailCCAddress = ""
    '        End If

    '        'set the content
    '        mail.Subject = strSubject
    '        mail.Body = strBody

    '        'set the addresses
    '        mail.From = New MailAddress(strEmailFromAddress)
    '        Dim i As Integer

    '        'build email To list
    '        Dim emailList As String() = strEmailToAddress.Split(";")

    '        For i = 0 To UBound(emailList)
    '            If emailList(i) <> ";" And emailList(i).Trim <> "" Then
    '                mail.To.Add(emailList(i))
    '            End If
    '        Next i

    '        ''build email CC List
    '        'If strEmailCCAddress IsNot Nothing Then
    '        '    emailList = strEmailCCAddress.Split(";")

    '        '    For i = 0 To UBound(emailList)
    '        '        If emailList(i) <> ";" And emailList(i).Trim <> "" Then
    '        '            mail.CC.Add(emailList(i))
    '        '        End If
    '        '    Next i
    '        'End If

    '        'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
    '        mail.IsBodyHtml = True

    '        'send the message 
    '        Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

    '        Try
    '            smtp.Send(mail)
    '        Catch ex As Exception
    '            lblMessage.Text &= "Email Notification queued."
    '            UGNErrorTrapping.InsertEmailQueue("Costing Approval Notification", strEmailFromAddress, strEmailToAddress, strEmailCCAddress, strSubject, strBody, "")
    '        End Try

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub
    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim dsSubscription As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("TeamMemberID") = 0
            ViewState("SubscriptionID") = 0
            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                If iTeamMemberID = 530 Then
                    iTeamMemberID = 246 ' Mike Echevarria
                    'iTeamMemberID = 687 ' Joseph Lentini 
                    'iTeamMemberID = 188 'Duane Rushing 
                    'iTeamMemberID = 433 'Derek Ames 
                    'iTeamMemberID = 391 'Grant Meseck 
                End If

                ViewState("TeamMemberID") = iTeamMemberID

                'CST Corporate Engineering
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 42)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 42
                End If

                'CST Plant Manager
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 43)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 43
                End If

                'CST(Purchasing)
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 44)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then                    
                    ViewState("SubscriptionID") = 44                
                End If

                'CST Product Development
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 45)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then                    
                    ViewState("SubscriptionID") = 45                
                End If

                'CST Sales
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 46)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then                    
                    ViewState("SubscriptionID") = 46                
                End If

                'CST VP of Operations
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 47)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then                    
                    ViewState("SubscriptionID") = 47                
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 71)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then                   
                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                            ViewState("isRestricted") = False
                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                            ViewState("isRestricted") = True
                    End Select
                End If
            End If

            If ViewState("TeamMemberID") = 530 Then
                ''''' ROD TESTING AS ANOTHER USER
                'ViewState("SubscriptionID") = 46 'sales
                'ViewState("TeamMemberID") = 2 'Bret Barta

                'ViewState("SubscriptionID") = 46 'sales
                'ViewState("TeamMemberID") = 391 'Grant Messek

                ViewState("SubscriptionID") = 46 'sales
                ViewState("TeamMemberID") = 246 'Mike Echevarria

                'ViewState("SubscriptionID") = 46 'sales
                'ViewState("TeamMemberID") = 222 'Jim Meade

                'ViewState("SubscriptionID") = 45 'Prod Dev Backup
                'ViewState("TeamMemberID") = 510 'Paul Papke

                'ViewState("SubscriptionID") = 46 'sales
                'ViewState("TeamMemberID") = 510 'Paul Papke

                'ViewState("SubscriptionID") = 45 'product development
                'ViewState("TeamMemberID") = 433 'Derek Ames

                'ViewState("SubscriptionID") = 45 'product development
                'ViewState("TeamMemberID") = 643 'Wallee Keating

                'ViewState("SubscriptionID") = 43 'plant manager                    
                'ViewState("TeamMemberID") = 164 ' Mike Joyner
                'ViewState("TeamMemberID") = 36 ' Mike Pulley

                'ViewState("SubscriptionID") = 44 'purchasing                    
                'ViewState("TeamMemberID") = 371 ' Ron Myotte

                'ViewState("SubscriptionID") = 42 'corporate engineering                
                'ViewState("TeamMemberID") = 140 ' Bryan Hall

                'ViewState("SubscriptionID") = 42 'purchasing                    
                'ViewState("TeamMemberID") = 48 ' Barry Bowhall

                'ViewState("SubscriptionID") = 42 'corporate engineering                    
                'ViewState("TeamMemberID") = 188 ' Duane Rushing

                ''lblMessage.Text = "You are using the same rights as Team Member ID: " & ViewState("TeamMemberID")
                'ViewState("isAdmin") = False
                'ViewState("isEdit") = True
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub EnableControls()

        Try

            lblRole.Visible = Not ViewState("isRestricted")
            ddSubscription.Visible = Not ViewState("isRestricted")
            gvApprovalList.Visible = Not ViewState("isRestricted")
            lblSearchCostSheetLabel.Visible = Not ViewState("isRestricted")
            txtSearchCostSheetID.Visible = Not ViewState("isRestricted")
            lblSearchSignedStatusLabel.Visible = Not ViewState("isRestricted")
            ddSearchSignedStatus.Visible = Not ViewState("isRestricted")
            lblSearchTeamMember.Visible = Not ViewState("isRestricted")
            ddSearchTeamMember.Visible = Not ViewState("isRestricted")
            lblInstructions.Visible = Not ViewState("isRestricted")
            btnReset.Visible = Not ViewState("isRestricted")
            btnSearch.Visible = Not ViewState("isRestricted")

            If ViewState("isRestricted") = False Then

                '        gvApprovalList.Columns(gvApprovalList.Columns.Count - 1).Visible = ViewState("isEdit")

            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Cost Sheet Approval List"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > Cost Sheet Approval List "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                CheckRights()

                BindCriteria()

                If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then
                    txtSearchCostSheetID.Text = HttpContext.Current.Request.QueryString("CostSheetID")
                End If

                If HttpContext.Current.Request.QueryString("SignedStatus") <> "" Then
                    ddSearchSignedStatus.SelectedValue = HttpContext.Current.Request.QueryString("SignedStatus")
                End If

                ddSearchTeamMember.SelectedValue = ViewState("TeamMemberID")
                If HttpContext.Current.Request.QueryString("TeamMemberID") <> "" Then
                    If HttpContext.Current.Request.QueryString("TeamMemberID") > 0 Then
                        ddSearchTeamMember.SelectedValue = HttpContext.Current.Request.QueryString("TeamMemberID")
                    End If
                End If

                txtCurrentTeamMemberID.Text = ViewState("TeamMemberID")

                EnableControls()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvApprovalList_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvApprovalList.DataBound

        'hide header of first column
        If gvApprovalList.Rows.Count > 0 Then
            gvApprovalList.HeaderRow.Cells(0).Visible = False
            'gvApprovalList.HeaderRow.Cells(1).Visible = False
        End If

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try

            txtSearchCostSheetID.Text = ""
            ddSearchSignedStatus.SelectedValue = "P"
            ddSearchTeamMember.SelectedValue = ViewState("TeamMemberID")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvApprovalList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvApprovalList.RowCommand

        Try
            ClearMessages()

            ''***
            ''When editing a row, prevent user from doing a search
            ''***
            If (e.CommandName = "Edit") Then
                lblRole.Visible = False
                ddSubscription.Visible = False
                lblSearchCostSheetLabel.Visible = False
                txtSearchCostSheetID.Visible = False
                lblSearchSignedStatusLabel.Visible = False
                ddSearchSignedStatus.Visible = False
                lblSearchTeamMember.Visible = False
                ddSearchTeamMember.Visible = False
                btnSearch.Visible = False
                btnReset.Visible = False
                lnkGoToCostSheetSearch.Visible = False
            End If

            ''***
            ''When undoing an edit of a row, allow search again
            ''***
            If e.CommandName = "Undo" Or e.CommandName = "Cancel" Then
                lblRole.Visible = True
                ddSubscription.Visible = True
                lblSearchCostSheetLabel.Visible = True
                txtSearchCostSheetID.Visible = True
                lblSearchSignedStatusLabel.Visible = True
                ddSearchSignedStatus.Visible = True
                lblSearchTeamMember.Visible = True
                ddSearchTeamMember.Visible = True
                btnSearch.Visible = True
                btnReset.Visible = True
                lnkGoToCostSheetSearch.Visible = True
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvApprovalList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApprovalList.RowDataBound

        Try

            Dim strBackColor As String = String.Empty
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim aCostFormAnchor As HtmlAnchor = TryCast(e.Row.FindControl("aCostForm"), HtmlAnchor)
                Dim aDieLayoutAnchor As HtmlAnchor = TryCast(e.Row.FindControl("aCostSheetDieLayout"), HtmlAnchor)
                Dim aPreApprovalInfoAnchor As HtmlAnchor = TryCast(e.Row.FindControl("aCostSheetPreApprovalInfo"), HtmlAnchor)
                Dim lbCostSheetID As Label = TryCast(e.Row.FindControl("lblViewCostSheetID"), Label)

                Dim redirstr As String = ""

                If aCostFormAnchor IsNot Nothing And lbCostSheetID IsNot Nothing Then
                    'redirstr = "javascript:void(window.open('Cost_Sheet_Preview.aspx?CostSheetID=" & lbCostSheetID.Text & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    redirstr = "javascript:void(window.open('Cost_Sheet_Preview.aspx?CostSheetID=" & lbCostSheetID.Text & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    aCostFormAnchor.Attributes.Add("onclick", redirstr)
                End If

                If aDieLayoutAnchor IsNot Nothing And lbCostSheetID IsNot Nothing Then
                    'redirstr = "javascript:void(window.open('Die_Layout_Preview.aspx?CostSheetID=" & lbCostSheetID.Text & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=810,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    redirstr = "javascript:void(window.open('Die_Layout_Preview.aspx?CostSheetID=" & lbCostSheetID.Text & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    aDieLayoutAnchor.Attributes.Add("onclick", redirstr)
                End If

                If aPreApprovalInfoAnchor IsNot Nothing And lbCostSheetID IsNot Nothing Then
                    redirstr = "javascript:void(window.open('Cost_Sheet_Pre_Approval_PopUp.aspx?CostSheetID=" & lbCostSheetID.Text & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=300,width=500,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    aPreApprovalInfoAnchor.Attributes.Add("onclick", redirstr)
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub
    Protected Function CheckIfAllFirstLevelsApproved(ByVal CostSheetID As Integer) As Boolean

        Dim bResult As Boolean = True

        Try
            Dim ds As DataSet

            ds = CostingModule.GetCostSheetPreApprovalList(CostSheetID, 0, 1, "P", 0, False, False, False)
            If commonFunctions.CheckDataSet(ds) = True Then
                bResult = False
            End If

            ds = CostingModule.GetCostSheetPreApprovalList(CostSheetID, 0, 1, "R", 0, False, False, False)
            If commonFunctions.CheckDataSet(ds) = True Then
                bResult = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        Return bResult
    End Function

    Protected Sub gvApprovalList_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvApprovalList.RowUpdated

        Try
            ClearMessages()
            'Dim ds As DataSet

            'Dim iCostSheetID As Integer = 0
            'Dim iRoutingLevel As Integer = 0
            'Dim iSubscriptionID As Integer = 0

            Dim lblCostSheetID As Label
            Dim txtCommentsTemp As TextBox
            Dim ddSignedStatusTemp As DropDownList
            'Dim txtRoutingLevelTemp As TextBox
            'Dim txtSubscriptionIDTemp As TextBox

            Dim currentRowInEdit As Integer = gvApprovalList.EditIndex
            'Dim strCostSheetTemp As String = gvApprovalList.Rows(currentRowInEdit).Cells(1).Text

            lblCostSheetID = CType(gvApprovalList.Rows(currentRowInEdit).FindControl("lblEditCostSheetID"), Label)
            txtCommentsTemp = CType(gvApprovalList.Rows(currentRowInEdit).FindControl("txtEditApprovalListComments"), TextBox)
            ddSignedStatusTemp = CType(gvApprovalList.Rows(currentRowInEdit).FindControl("ddEditApprovalListSignedStatusDesc"), DropDownList)
            'txtRoutingLevelTemp = CType(gvApprovalList.Rows(currentRowInEdit).FindControl("txtEditApprovalListRoutingLevel"), TextBox)
            'txtSubscriptionIDTemp = CType(gvApprovalList.Rows(currentRowInEdit).FindControl("txtEditApprovalListSubscriptionID"), TextBox)

            'iCostSheetID = CType(lblCostSheetID.Text, Integer)
            'iRoutingLevel = CType(txtRoutingLevelTemp.Text, Integer)
            'iSubscriptionID = CType(txtSubscriptionIDTemp.Text, Integer)

            'check if approved already by same subscription
            'ds = CostingModule.GetCostSheetPreApprovalList(iCostSheetID, 0, iRoutingLevel, "A", iSubscriptionID, False, False, False)
            'If commonFunctions.CheckDataset(ds) = False Then
            If ddSignedStatusTemp.SelectedValue = "A" Or ddSignedStatusTemp.SelectedValue = "R" Then
                If ddSignedStatusTemp.SelectedValue = "R" And txtCommentsTemp.Text.Trim = "" Then
                    lblMessage.Text += "<br>Error: comments are required for rejected cost sheets."

                Else
                    lblMessage.Text += lblCostSheetID.Text & " was " & ddSignedStatusTemp.SelectedItem.Text & "<br>"

                    'iCostSheetID = CType(lblCostSheetID.Text, Integer)
                    '2009-Nov-20 - put notification in stored procedure
                    'If ddSignedStatusTemp.SelectedValue = "R" Or txtRoutingLevelTemp.Text = "1" Or (txtRoutingLevelTemp.Text = "2" And CheckIfAllFirstLevelsApproved(iCostSheetID) = False) Then
                    '    SendEmail(lblCostSheetID.Text, ddSignedStatusTemp.SelectedValue, ddSignedStatusTemp.SelectedItem.Text, txtCommentsTemp.Text.Trim)
                    'End If

                End If
            Else
                lblMessage.Text += "<br>Error: The cost sheet must either be approved or rejected. Please select a status."
            End If
            'Else
            'lblMessage.Text += "<br>Error: The cost sheet " & lblCostSheetID.Text & "was already approved."
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text
    End Sub

    Protected Sub ddSubscription_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddSubscription.SelectedIndexChanged

        Try
            ClearMessages()

            Dim ds As DataSet

            If ddSubscription.SelectedIndex > 0 Then
                ViewState("SubscriptionID") = ddSubscription.SelectedValue
            End If

            ''bind existing data to drop down Group List          
            ds = CostingModule.GetCostSheetApproverBySubscription(ViewState("SubscriptionID"), 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchTeamMember.Items.Clear()
                ddSearchTeamMember.DataSource = ds
                ddSearchTeamMember.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName.ToString()
                ddSearchTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddSearchTeamMember.DataBind()
            End If

            ddSearchTeamMember.SelectedValue = ViewState("TeamMemberID")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub
End Class
