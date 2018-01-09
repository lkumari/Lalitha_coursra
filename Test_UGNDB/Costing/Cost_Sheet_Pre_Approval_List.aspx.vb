' ***********************************************************************************************
'
' Name:		Cost_Sheet_Pre_Approval_List.aspx
' Purpose:	This Code Behind is build a notification list of cost sheet pre-approvers
'
' Date		Author	    
' 10/27/2008 Roderick Carlson - Created 
' 11/17/2009 Roderick Carlson - Modified - Cleaned up formatting a bit
' 12/01/2009 Roderick Carlson - Modified - allow refresh of subscription dropdowns when row is cleared - look for most recent subscription used for team member
' 08/03/2010 Roderick Carlson - Modified - (CO-2947) adjusted case statement for GetCostSheetTeamMemberRecentSubscription to include 43, not 32
' 01/11/2011 Roderick Carlson - Modified - Added Email Queue
' 12/07/2011 Roderick Carlson - Modified - Prevent Duplicate Emails in each line
' 05/07/2014 LREY             - Added isCostReduction to the body of the email
' ************************************************************************************************

Partial Class Cost_Sheet_Pre_Approval_List
    Inherits System.Web.UI.Page
    Protected Function GetApproverList(ByVal RoutingLevel As Integer, ByVal filterNotified As Boolean, ByVal isNotified As Boolean) As String

        Dim strEmailToAddress As String = ""

        Try
            Dim dsRoutingLevelTeamMembers As DataSet
            Dim dsTeamMember As DataSet
            Dim dsBackup As DataSet

            Dim iRowCounter As Integer = 0
            Dim iRoutingLevelTeamMemberID As Integer
            Dim iRoutingLevelSubscriptionID As Integer

            ViewState("isCostReduction") = ""
            
            'get first level approvers
            'if cbFirstLevelRoutingNotifyOnlyNew is checked then  get only people who have NOT been notified, else get all 
            dsRoutingLevelTeamMembers = CostingModule.GetCostSheetPreApprovalList(ViewState("CostSheetID"), 0, RoutingLevel, "", 0, filterNotified, isNotified, False)
            If commonFunctions.CheckDataSet(dsRoutingLevelTeamMembers) = True Then                
                For iRowCounter = 0 To dsRoutingLevelTeamMembers.Tables(0).Rows.Count - 1

                    iRoutingLevelTeamMemberID = dsRoutingLevelTeamMembers.Tables(0).Rows(iRowCounter).Item("TeamMemberID")
                    iRoutingLevelSubscriptionID = dsRoutingLevelTeamMembers.Tables(0).Rows(iRowCounter).Item("SubscriptionID")
                    ViewState("isCostReduction") = dsRoutingLevelTeamMembers.Tables(0).Rows(iRowCounter).Item("isCostReduction")

                    If iRoutingLevelTeamMemberID > 0 Then
                        'get backup TeamMemberID and email, based on Corporate Calendar too

                        dsTeamMember = SecurityModule.GetTeamMember(iRoutingLevelTeamMemberID, "", "", "", "", "", True, Nothing)
                        If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                            'If strEmailToAddress <> "" Then
                            '    strEmailToAddress += ";"
                            'End If

                            If dsTeamMember.Tables(0).Rows(0).Item("Email").ToString <> "" Then

                                If InStr(strEmailToAddress, dsTeamMember.Tables(0).Rows(0).Item("Email").ToString, CompareMethod.Binary) <= 0 Then
                                    If strEmailToAddress <> "" Then
                                        strEmailToAddress &= ";"
                                    End If

                                    strEmailToAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                                End If

                                'update notification date of row
                                CostingModule.UpdateCostSheetPreApprovalNotificationDate(ViewState("CostSheetID"), iRoutingLevelTeamMemberID)

                                dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iRoutingLevelTeamMemberID, iRoutingLevelSubscriptionID)
                                If commonFunctions.CheckDataSet(dsBackup) = True Then
                                    If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 Then

                                        If InStr(strEmailToAddress, dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString, CompareMethod.Binary) <= 0 Then
                                            If strEmailToAddress <> "" Then
                                                strEmailToAddress &= ";"
                                            End If

                                            strEmailToAddress &= dsBackup.Tables(0).Rows(0).Item("BackupEmail")

                                            ViewState("BackupTeamMembers") &= "<br>" & dsBackup.Tables(0).Rows(0).Item("BackupFullName").ToString & ": assigned to approve as backup for: " & dsTeamMember.Tables(0).Rows(0).Item("LastName").ToString & ", " & dsTeamMember.Tables(0).Rows(0).Item("FirstName").ToString & " until " & dsBackup.Tables(0).Rows(0).Item("EndDate").ToString & ".<br>"
                                        End If
                                    End If
                                End If

                            End If

                        End If ' If dsTeamMember IsNot Nothing
                    End If ' If iRoutingLevelTeamMemberID > 0
                Next
            End If ' If dsRoutingLevelTeamMembers IsNot Nothing 

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        GetApproverList = strEmailToAddress

    End Function
    Protected Function SendEmail(ByVal EmailToAddress As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try

            Dim strEmailCCAddress As String = ""
            Dim iRowCounter As Integer = 0
            Dim strSubject As String = ""
            Dim strBody As String = ""
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strApproveURL As String = strProdOrTestEnvironment & "Costing/Cost_Sheet_Approve.aspx?CostSheetID=" & ViewState("CostSheetID")
            Dim strPreviewCostFormURL As String = strProdOrTestEnvironment & "Costing/Cost_Sheet_Preview.aspx?CostSheetID=" & ViewState("CostSheetID")
            Dim strPreviewDieLayoutURL As String = strProdOrTestEnvironment & "Costing/Die_Layout_Preview.aspx?CostSheetID=" & ViewState("CostSheetID")

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"
            strEmailCCAddress = strEmailFromAddress

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br><br>"
                strBody += "<h1>This information is purely for testing and is NOT valid!!!</h1><br><br>"
            End If

            strSubject += "Quote Approval Notification for Cost Sheet: " & ViewState("CostSheetID")

            'create the mail message using new System.Net.Mail (not CDonts)
            Dim mail As New MailMessage()

            strBody += "<font size='3' face='Verdana'>The following Cost Sheet Quote is ready for your review: </font><br><br>"

            If ViewState("BackupTeamMembers") <> "" Then
                strBody += ViewState("BackupTeamMembers") & "<br>"
            End If

            strBody += "<font size='2' face='Verdana'>Cost Sheet: <b>" & ViewState("CostSheetID") & "</b></font><br><br>"

            strBody += "<a href='" & strApproveURL & "'><b><u>Click here to go to the Cost Sheet Approval page</b></u></a><br><br>"

            strBody += "<a href='" & strPreviewCostFormURL & "'><b><u>Click here to Preview the Cost Sheet</u></b></a><br><br>"

            If ViewState("isDiecut") = True Then
                strBody += "<a href='" & strPreviewDieLayoutURL & "'><b><u>Click here to Preview the Die Layout</u></b></a><br><br>"
            End If

            If ViewState("isCostReduction") = "Yes" Then
                strBody += "<font size='3' face='Verdana' color='red'><b>THIS IS A COST REDUCTION.</b></font><br><br>"
            End If

            If ViewState("RFDNo") <> "" Then
                strBody += "<font size='2' face='Verdana'>RFQ/RFC No. : " & ViewState("RFDNo") & "</font><br>"
            End If

            If ViewState("NewCustomerPartNo") <> "" Then
                strBody += "<font size='2' face='Verdana'>New Customer PartNo : " & ViewState("NewCustomerPartNo") & "</font><br>"
            End If

            If ViewState("NewDesignLevel") <> "" Then
                strBody += "<font size='2' face='Verdana'>New Design Level : " & ViewState("NewDesignLevel") & "</font><br>"
            End If

            If ViewState("NewPartName") <> "" Then
                strBody += "<font size='2' face='Verdana'>Part Name : " & ViewState("NewPartName") & "</font><br>"
            End If

            If ViewState("NewDrawingNo") <> "" Then
                strBody += "<font size='2' face='Verdana'>New DrawingNo: " & ViewState("NewDrawingNo") & "</font><br>"
            End If

            If ViewState("OldPartNo") <> "" Then
                strBody += "<font size='2' face='Verdana'>PartNo: " & ViewState("OldPartNo") & "</font><br>"
            End If

            If ViewState("OldFinishedGoodPartNo") <> "" Then
                strBody += "<font size='2' face='Verdana'>PartNo: " & ViewState("OldFinishedGoodPartNo") & "</font><br>"
            End If

            If ViewState("NewPartNo") <> "" Then
                strBody += "<font size='2' face='Verdana'>BPCS PartNo: " & ViewState("NewPartNo") & "</font> "

                If ViewState("NewPartRevision") <> "" Then
                    strBody += "<font size='2' face='Verdana'> Revision: " & ViewState("NewPartRevision") & "</font><br>"
                End If
            End If

            If ViewState("FinishedGoodPart") <> "" Then
                strBody += "<font size='2' face='Verdana'>Finished Good BPCS Part(s):<br> " & ViewState("FinishedGoodPart") & "</font><br>"
            End If

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody += "<br><br>Email To Address List: " & EmailToAddress & "<br>"
                strBody += "<br>Email CC Address List: " & strEmailCCAddress & "<br>"

                EmailToAddress = "Lynette.Rey@ugnauto.com"
                strEmailCCAddress = ""
            End If

            'set the content
            mail.Subject = strSubject
            mail.Body = strBody

            'set the addresses
            mail.From = New MailAddress(strEmailFromAddress)
            Dim i As Integer

            'build email To list
            Dim emailList As String() = EmailToAddress.Split(";")

            For i = 0 To UBound(emailList)
                If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                    mail.To.Add(emailList(i))
                End If
            Next i

            'build email CC List
            If strEmailCCAddress IsNot Nothing Then
                emailList = strEmailCCAddress.Split(";")

                For i = 0 To UBound(emailList)
                    If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                        mail.CC.Add(emailList(i))
                    End If
                Next i
            End If

            'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
            mail.IsBodyHtml = True

            'send the message 
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(mail)
                lblMessage.Text &= "Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("Costing Pre Approval Notification", strEmailFromAddress, EmailToAddress, strEmailCCAddress, strSubject, strBody, "")
            End Try

            bReturnValue = True
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        SendEmail = bReturnValue

    End Function
    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet

            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            ViewState("TeamMemberID") = 0

            Dim iRoleID As Integer = 0

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                ViewState("TeamMemberID") = iTeamMemberID

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 57)
                If commonFunctions.CheckDataSet(dsRoleForm) = True Then
                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isRestricted") = False
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                            ViewState("isRestricted") = False
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
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

            'If ViewState("TeamMemberID") = 530 Then
            '    ''''' ROD TESTING AS ANOTHER USER
            '    'ViewState("SubscriptionID") = 46 'sales
            '    'ViewState("TeamMemberID") = 2 'Bret Barta

            '    ViewState("SubscriptionID") = 46 'sales
            '    ViewState("TeamMemberID") = 391 'Grant Messek

            '    'ViewState("SubscriptionID") = 46 'sales
            '    'ViewState("TeamMemberID") = 246 'Mike Echevarria

            '    'ViewState("SubscriptionID") = 46 'sales
            '    'ViewState("TeamMemberID") = 222 'Jim Meade

            '    'ViewState("SubscriptionID") = 46 'sales
            '    'ViewState("TeamMemberID") = 510 'Paul Papke

            '    'ViewState("SubscriptionID") = 45 'product development
            '    'ViewState("TeamMemberID") = 433 'Derek Ames

            '    'ViewState("SubscriptionID") = 43 'plant manager                    
            '    'ViewState("TeamMemberID") = 36 ' Mike Pulley

            '    'ViewState("SubscriptionID") = 44 'purchasing                    
            '    'ViewState("TeamMemberID") = 371 ' Ron Myotte

            '    'ViewState("SubscriptionID") = 42 'purchasing                    
            '    'ViewState("TeamMemberID") = 140 ' Bryan Hall

            '    lblMessage.Text = "You are using the same rights as Team Member ID: " & ViewState("TeamMemberID")
            '    ViewState("isAdmin") = False
            '    ViewState("isEdit") = False
            'End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Protected Sub EnableControls()

        Try
            If ViewState("isRestricted") = False Then
                If ViewState("CostSheetID") > 0 Then
                    gvPreApprovalListFirstRoutingLevel.Visible = Not ViewState("isRestricted")
                    gvPreApprovalListSecondRoutingLevel.Visible = Not ViewState("isRestricted")
                    lblCostSheetLabel.Visible = Not ViewState("isRestricted")
                    lblCostSheetValue.Visible = Not ViewState("isRestricted")
                    gvPreApprovalHistory.Visible = Not ViewState("isRestricted")

                    If ViewState("isApproved") = False Then
                        lblChooseNotificationGroupLabel.Visible = ViewState("isAdmin")
                        ddChooseNotificationGroupValue.Visible = ViewState("isAdmin")
                        iBtnGetPreApprovalListFromGroup.Visible = ViewState("isAdmin")

                        lblChooseCostSheetLabel.Visible = ViewState("isAdmin")
                        txtChooseCostSheetValue.Visible = ViewState("isAdmin")
                        iBtnGetAnotherCostSheetPreApprovalList.Visible = ViewState("isAdmin")

                        btnFirstLevelRoutingNotify.Visible = ViewState("isAdmin")
                        btnNotifyAll.Visible = ViewState("isAdmin")
                        btnSecondLevelRoutingNotify.Visible = ViewState("isAdmin")

                        cbAllLevelRoutingNotifyOnlyNew.Visible = ViewState("isAdmin")
                        cbFirstLevelRoutingNotifyOnlyNew.Visible = ViewState("isAdmin")
                        cbSecondLevelRoutingNotifyOnlyNew.Visible = ViewState("isAdmin")

                        gvPreApprovalListFirstRoutingLevel.Columns(gvPreApprovalListFirstRoutingLevel.Columns.Count - 1).Visible = ViewState("isAdmin")
                        If gvPreApprovalListFirstRoutingLevel.FooterRow IsNot Nothing Then
                            gvPreApprovalListFirstRoutingLevel.FooterRow.Visible = ViewState("isAdmin")
                        End If

                        gvPreApprovalListSecondRoutingLevel.Columns(gvPreApprovalListSecondRoutingLevel.Columns.Count - 1).Visible = ViewState("isAdmin")
                        If gvPreApprovalListSecondRoutingLevel.FooterRow IsNot Nothing Then
                            gvPreApprovalListSecondRoutingLevel.FooterRow.Visible = ViewState("isAdmin")
                        End If
                    Else
                        btnEdit.Visible = ViewState("isAdmin")
                        gvPreApprovalListFirstRoutingLevel.Columns(gvPreApprovalListFirstRoutingLevel.Columns.Count - 1).Visible = False
                        If gvPreApprovalListFirstRoutingLevel.FooterRow IsNot Nothing Then
                            gvPreApprovalListFirstRoutingLevel.FooterRow.Visible = False
                        End If

                        gvPreApprovalListSecondRoutingLevel.Columns(gvPreApprovalListSecondRoutingLevel.Columns.Count - 1).Visible = False
                        If gvPreApprovalListSecondRoutingLevel.FooterRow IsNot Nothing Then
                            gvPreApprovalListSecondRoutingLevel.FooterRow.Visible = False
                        End If
                    End If

                End If
            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
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

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ds = CostingModule.GetCostSheetGroup(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddChooseNotificationGroupValue.DataSource = ds
                ddChooseNotificationGroupValue.DataTextField = ds.Tables(0).Columns("ddGroupName").ColumnName.ToString()
                ddChooseNotificationGroupValue.DataValueField = ds.Tables(0).Columns("GroupID").ColumnName
                ddChooseNotificationGroupValue.DataBind()
                ddChooseNotificationGroupValue.Items.Insert(0, "")
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
    Private Sub BindData()

        Try
            Dim ds As DataSet
            Dim dsPostApproval As DataSet
            Dim dsTopLevelPartInfo As DataSet
            Dim iRowCounter As Integer = 1

            ViewState("isDiecut") = False
            ViewState("FinishedGoodPart") = ""

            If ViewState("CostSheetID") > 0 Then
                'bind existing CostSheet data to for top level cost sheet info                     
                'ds = CostingModule.GetCostSheet(ViewState("CostSheetID"), "", 0, 0, 0, "", "", "", "", "", "", 0, 0, 0, "", 0, "", 0, 0, True, 41, False, False)
                ds = CostingModule.GetCostSheet(ViewState("CostSheetID"))

                If ViewState("isRestricted") = False Then
                    If commonFunctions.CheckDataSet(ds) = True Then

                        If ds.Tables(0).Rows(0).Item("Obsolete") = False Then
                            lblCostSheetValue.Text = ViewState("CostSheetID")

                            If ds.Tables(0).Rows(0).Item("PreviousCostSheetID") IsNot System.DBNull.Value Then
                                If ds.Tables(0).Rows(0).Item("PreviousCostSheetID") > 0 Then
                                    txtChooseCostSheetValue.Text = ds.Tables(0).Rows(0).Item("PreviousCostSheetID")
                                End If
                            End If

                            If ds.Tables(0).Rows(0).Item("ApprovedDate").ToString <> "" Then
                                'ViewState("isApproved") = True

                                'check if team members have been notified of approved cost sheet
                                dsPostApproval = CostingModule.GetCostSheetPostApprovalList(ViewState("CostSheetID"), True, True)
                                If commonFunctions.CheckDataSet(dsPostApproval) = True Then
                                    ViewState("isApproved") = True
                                End If
                            End If

                            'If ds.Tables(0).Rows(0).Item("RFDNo") IsNot System.DBNull.Value Then
                            '    If ds.Tables(0).Rows(0).Item("RFDNo") > 0 Then
                            '        ViewState("RFDNo") = ds.Tables(0).Rows(0).Item("RFDNo")
                            '    End If
                            'End If

                            ViewState("NewCustomerPartNo") = ds.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString
                            ViewState("NewDesignLevel") = ds.Tables(0).Rows(0).Item("NewDesignLevel").ToString
                            ViewState("NewPartName") = ds.Tables(0).Rows(0).Item("NewPartName").ToString
                            ViewState("NewDrawingNo") = ds.Tables(0).Rows(0).Item("NewDrawingNo").ToString

                            ViewState("isDiecut") = ds.Tables(0).Rows(0).Item("isDiecut")

                            ViewState("OldPartNo") = ds.Tables(0).Rows(0).Item("OldPartNo").ToString
                            ViewState("OldFinishedGoodPartNo") = ds.Tables(0).Rows(0).Item("OldFinishedGoodPartNo").ToString

                            ViewState("NewPartNo") = ds.Tables(0).Rows(0).Item("NewBPCSPartNo").ToString
                            ViewState("NewPartRevision") = ds.Tables(0).Rows(0).Item("NewBPCSPartRevision").ToString

                            ViewState("RFDNo") = ds.Tables(0).Rows(0).Item("RFDNo").ToString

                            dsTopLevelPartInfo = CostingModule.GetCostSheetTopLevelPartInfo(ViewState("CostSheetID"))
                            If commonFunctions.CheckDataSet(dsTopLevelPartInfo) = True Then
                                For iRowCounter = 0 To dsTopLevelPartInfo.Tables(0).Rows.Count - 1
                                    ViewState("FinishedGoodPart") &= dsTopLevelPartInfo.Tables(0).Rows(iRowCounter).Item("PartNo").ToString.PadRight(15, " ") & "   REVISION: " & dsTopLevelPartInfo.Tables(0).Rows(iRowCounter).Item("PartRevision").ToString.PadRight(2, " ") & "    NAME: " & dsTopLevelPartInfo.Tables(0).Rows(iRowCounter).Item("PartName").ToString & "<br />"
                                Next
                            End If
                        End If
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

    Protected Sub ddFooterPreApprovalListFirstRoutingLevelTeamMember_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data to the Footer PreApproval SubScription drop down list based on TeamMember Selection
        '' from the Edittemplate in the grid view.
        ''*******

        lblMessage.Text = ""

        Try
            Dim ddTeamMember As DropDownList
            Dim ddSubscription As DropDownList
            Dim ds As DataSet
            Dim iRowCounter As Integer = 0
            Dim liSubscriptionItem As ListItem

            ddTeamMember = CType(sender, DropDownList)
            ddSubscription = CType(gvPreApprovalListFirstRoutingLevel.FooterRow.FindControl("ddFooterPreApprovalListFirstRoutingLevelSubscription"), DropDownList)

            If ddTeamMember.SelectedIndex > 0 Then
                ds = CostingModule.GetCostSheetSubscriptionByApprover(ddTeamMember.SelectedValue, 1)

                If commonFunctions.CheckDataset(ds) = True Then

                    'clear all rows
                    ddSubscription.Items.Clear()
                    For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                        liSubscriptionItem = New ListItem
                        liSubscriptionItem.Text = ds.Tables(0).Rows(iRowCounter).Item("Subscription").ToString
                        liSubscriptionItem.Value = ds.Tables(0).Rows(iRowCounter).Item("SubscriptionID")
                        ddSubscription.Items.Add(liSubscriptionItem)
                    Next

                    ds = CostingModule.GetCostSheetTeamMemberRecentSubscription(ddTeamMember.SelectedValue)
                    If commonFunctions.CheckDataset(ds) = True Then
                        If ds.Tables(0).Rows(0).Item("SubscriptionID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("SubscriptionID") > 0 Then                               
                                Select Case CType(ds.Tables(0).Rows(0).Item("SubscriptionID"), Integer)
                                    Case 42, 43, 44, 45
                                        ddSubscription.SelectedValue = ds.Tables(0).Rows(0).Item("SubscriptionID")
                                End Select
                            End If
                        End If

                    End If
                End If
            Else 'reset to all
                ddSubscription.Items.Clear()
                ds = CostingModule.GetCostSheetSubscriptionByApprover(0, 1)
                If commonFunctions.CheckDataset(ds) = True Then
                    ddSubscription.DataSource = ds
                    ddSubscription.DataTextField = ds.Tables(0).Columns("Subscription").ColumnName.ToString()
                    ddSubscription.DataValueField = ds.Tables(0).Columns("SubscriptionID").ColumnName
                    ddSubscription.DataBind()
                    ddSubscription.Items.Insert(0, "")
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

    Protected Sub ddFooterPreApprovalListSecondRoutingLevelTeamMember_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data to the Footer PreApproval SubScription drop down list based on TeamMember Selection
        '' from the Edittemplate in the grid view.
        ''*******

        lblMessage.Text = ""

        Try
            Dim ddTeamMember As DropDownList
            Dim ddSubscription As DropDownList
            Dim ds As DataSet
            Dim iRowCounter As Integer = 0
            Dim liSubscriptionItem As ListItem

            ddTeamMember = CType(sender, DropDownList)
            ddSubscription = CType(gvPreApprovalListSecondRoutingLevel.FooterRow.FindControl("ddFooterPreApprovalListSecondRoutingLevelSubscription"), DropDownList)

            ds = CostingModule.GetCostSheetSubscriptionByApprover(ddTeamMember.SelectedValue, 2)
            If commonFunctions.CheckDataSet(ds) = True Then

                'clear all rows
                ddSubscription.Items.Clear()
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    liSubscriptionItem = New ListItem
                    liSubscriptionItem.Text = ds.Tables(0).Rows(iRowCounter).Item("Subscription").ToString
                    liSubscriptionItem.Value = ds.Tables(0).Rows(iRowCounter).Item("SubscriptionID")
                    ddSubscription.Items.Add(liSubscriptionItem)
                Next
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            CheckRights()

            If Not Page.IsPostBack Then

                If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then
                    BindCriteria()
                    ViewState("isApproved") = False
                    ViewState("CostSheetID") = CType(HttpContext.Current.Request.QueryString("CostSheetID"), Integer)
                    BindData()
                End If

            End If

            EnableControls()

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Cost Sheet Pre-Approval List"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > <b> <a href='Cost_Sheet_Detail.aspx?CostSheetID=" & ViewState("CostSheetID") & "'>Cost Sheet Detail </a> </b> > Cost Sheet Pre-Approval List  "
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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub gvPreApprovalListFirstRoutingLevel_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPreApprovalListFirstRoutingLevel.DataBound

        'hide header of first column
        If gvPreApprovalListFirstRoutingLevel.Rows.Count > 0 Then
            gvPreApprovalListFirstRoutingLevel.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvPreApprovalListFirstRoutingLevel_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPreApprovalListFirstRoutingLevel.RowCommand

        Try
            lblMessage.Text = ""
            lblFirstLevelRoutingMessage.Text = ""

            Dim ds As DataSet
            Dim bFoundIt As Boolean = False

            Dim ddTeamMemberNameTemp As DropDownList
            Dim ddSubScriptionTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddTeamMemberNameTemp = CType(gvPreApprovalListFirstRoutingLevel.FooterRow.FindControl("ddFooterPreApprovalListFirstRoutingLevelTeamMember"), DropDownList)
                ddSubScriptionTemp = CType(gvPreApprovalListFirstRoutingLevel.FooterRow.FindControl("ddFooterPreApprovalListFirstRoutingLevelSubscription"), DropDownList)

                ds = CostingModule.GetCostSheetPreApprovalList(ViewState("CostSheetID"), ddTeamMemberNameTemp.SelectedValue, 1, "", 0, False, False, False)

                If commonFunctions.CheckDataSet(ds) = True Then
                    bFoundIt = True
                End If


                If bFoundIt = False Then
                    ds = CostingModule.GetCostSheetPreApprovalList(ViewState("CostSheetID"), 0, 1, "", ddSubScriptionTemp.SelectedValue, False, False, False)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        bFoundIt = True
                    End If
                End If

                ds = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(ddTeamMemberNameTemp.SelectedValue, ddSubScriptionTemp.SelectedValue)

                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("BackupID") > 0 Then
                        lblMessage.Text &= "<br />This user is out of the office until " & ds.Tables(0).Rows(0).Item("EndDate").ToString & "."
                        lblMessage.Text &= "<br />The available backup team member is: " & ds.Tables(0).Rows(0).Item("BackupFullName").ToString & "."
                        lblMessage.Text &= "<br />If you still wish to proceed with this team member, the back up person will be asked to approve. However, both team members will be notified."

                        lblFirstLevelRoutingMessage.Text &= "<br />This user is out of the office until " & ds.Tables(0).Rows(0).Item("EndDate").ToString & "."
                        lblFirstLevelRoutingMessage.Text &= "<br />The available backup team member is: " & ds.Tables(0).Rows(0).Item("BackupFullName").ToString & "."
                        lblFirstLevelRoutingMessage.Text &= "<br />If you still wish to proceed with this team member, the back up person will be asked to approve. However, both team members will be notified."
                    End If
                End If

                'check if selected team member is out of the office
                If bFoundIt = False Then
                    odsPreApprovalListFirstRoutingLevel.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                    odsPreApprovalListFirstRoutingLevel.InsertParameters("RoutingLevel").DefaultValue = 1
                    odsPreApprovalListSecondRoutingLevel.InsertParameters("SignedStatus").DefaultValue = "P"
                    odsPreApprovalListFirstRoutingLevel.InsertParameters("TeamMemberID").DefaultValue = ddTeamMemberNameTemp.SelectedValue
                    odsPreApprovalListFirstRoutingLevel.InsertParameters("SubscriptionID").DefaultValue = ddSubScriptionTemp.SelectedValue

                    intRowsAffected = odsPreApprovalListFirstRoutingLevel.Insert()
                Else
                    lblMessage.Text &= "Error: Either a team member or a subscription has been selected twice.<br />"
                    lblFirstLevelRoutingMessage.Text &= "Error: Either a team member or a subscription has been selected twice.<br />"
                End If
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvPreApprovalListFirstRoutingLevel.ShowFooter = False
            Else
                gvPreApprovalListFirstRoutingLevel.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddTeamMemberNameTemp = CType(gvPreApprovalListFirstRoutingLevel.FooterRow.FindControl("ddFooterPreApprovalListFirstRoutingLevelTeamMember"), DropDownList)
                ddTeamMemberNameTemp.SelectedIndex = -1

                ddSubScriptionTemp = CType(gvPreApprovalListFirstRoutingLevel.FooterRow.FindControl("ddFooterPreApprovalListFirstRoutingLevelSubscription"), DropDownList)
                ddSubScriptionTemp.SelectedIndex = -1

                ddSubScriptionTemp.Items.Clear()
                ds = CostingModule.GetCostSheetSubscriptionByApprover(0, 1)
                If commonFunctions.CheckDataSet(ds) = True Then
                    ddSubScriptionTemp.DataSource = ds
                    ddSubScriptionTemp.DataTextField = ds.Tables(0).Columns("Subscription").ColumnName.ToString()
                    ddSubScriptionTemp.DataValueField = ds.Tables(0).Columns("SubscriptionID").ColumnName
                    ddSubScriptionTemp.DataBind()
                    ddSubScriptionTemp.Items.Insert(0, "")
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

    Protected Sub gvPreApprovalListSecondRoutingLevel_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPreApprovalListSecondRoutingLevel.DataBound

        'hide header of Second column
        If gvPreApprovalListSecondRoutingLevel.Rows.Count > 0 Then
            gvPreApprovalListSecondRoutingLevel.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvPreApprovalListSecondRoutingLevel_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPreApprovalListSecondRoutingLevel.RowCommand

        Try

            lblMessage.Text = ""
            lblSecondLevelRoutingMessage.Text = ""

            Dim ds As DataSet
            Dim bFoundIt As Boolean = False

            Dim ddTeamMemberNameTemp As DropDownList
            Dim ddSubScriptionTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then


                ddTeamMemberNameTemp = CType(gvPreApprovalListSecondRoutingLevel.FooterRow.FindControl("ddFooterPreApprovalListSecondRoutingLevelTeamMember"), DropDownList)
                ddSubScriptionTemp = CType(gvPreApprovalListSecondRoutingLevel.FooterRow.FindControl("ddFooterPreApprovalListSecondRoutingLevelSubscription"), DropDownList)

                ds = CostingModule.GetCostSheetPreApprovalList(ViewState("CostSheetID"), ddTeamMemberNameTemp.SelectedValue, 2, "", 0, False, False, False)

                If commonFunctions.CheckDataSet(ds) = True Then                 
                    bFoundIt = True                
                End If


                If bFoundIt = False Then
                    ds = CostingModule.GetCostSheetPreApprovalList(ViewState("CostSheetID"), 0, 2, "", ddSubScriptionTemp.SelectedValue, False, False, False)

                    If commonFunctions.CheckDataSet(ds) = True Then                       
                        bFoundIt = True                    
                    End If
                End If

                ds = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(ddTeamMemberNameTemp.SelectedValue, ddSubScriptionTemp.SelectedValue)

                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("BackupID") > 0 Then
                        lblMessage.Text &= "<br />This user is unavailable until " & ds.Tables(0).Rows(0).Item("EndDate").ToString & "."
                        lblMessage.Text &= "<br />The available backup team member is: " & ds.Tables(0).Rows(0).Item("BackupFullName").ToString & "."
                        lblMessage.Text &= "<br />If you still wish to proceed with this team member, the back up person will be asked to approve. However, both team members will be notified."

                        lblSecondLevelRoutingMessage.Text &= "<br />This user is unavailable until " & ds.Tables(0).Rows(0).Item("EndDate").ToString & "."
                        lblSecondLevelRoutingMessage.Text &= "<br />The available backup team member is: " & ds.Tables(0).Rows(0).Item("BackupFullName").ToString & "."
                        lblSecondLevelRoutingMessage.Text &= "<br />If you still wish to proceed with this team member, the back up person will be asked to approve. However, both team members will be notified."
                    End If
                End If

                If bFoundIt = False Then
                    odsPreApprovalListSecondRoutingLevel.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                    odsPreApprovalListSecondRoutingLevel.InsertParameters("RoutingLevel").DefaultValue = 2
                    odsPreApprovalListSecondRoutingLevel.InsertParameters("SignedStatus").DefaultValue = "P"
                    odsPreApprovalListSecondRoutingLevel.InsertParameters("TeamMemberID").DefaultValue = ddTeamMemberNameTemp.SelectedValue
                    odsPreApprovalListSecondRoutingLevel.InsertParameters("SubscriptionID").DefaultValue = ddSubScriptionTemp.SelectedValue

                    intRowsAffected = odsPreApprovalListSecondRoutingLevel.Insert()
                Else
                    lblMessage.Text &= "Error: Either a team member or a subscription has been selected twice.<br />"
                    lblSecondLevelRoutingMessage.Text &= "Error: Either a team member or a subscription has been selected twice.<br />"
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvPreApprovalListSecondRoutingLevel.ShowFooter = False
            Else
                gvPreApprovalListSecondRoutingLevel.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddTeamMemberNameTemp = CType(gvPreApprovalListSecondRoutingLevel.FooterRow.FindControl("ddFooterPreApprovalListSecondRoutingLevelTeamMember"), DropDownList)
                ddTeamMemberNameTemp.SelectedIndex = -1

                ddSubScriptionTemp = CType(gvPreApprovalListSecondRoutingLevel.FooterRow.FindControl("ddFooterPreApprovalListSecondRoutingLevelSubscription"), DropDownList)
                ddSubScriptionTemp.SelectedIndex = -1

                ddSubScriptionTemp.Items.Clear()
                ds = CostingModule.GetCostSheetSubscriptionByApprover(0, 2)
                If commonFunctions.CheckDataSet(ds) = True Then
                    ddSubScriptionTemp.DataSource = ds
                    ddSubScriptionTemp.DataTextField = ds.Tables(0).Columns("Subscription").ColumnName.ToString()
                    ddSubScriptionTemp.DataValueField = ds.Tables(0).Columns("SubscriptionID").ColumnName
                    ddSubScriptionTemp.DataBind()
                    ddSubScriptionTemp.Items.Insert(0, "")
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
#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_PreApprovalListFirstRoutingLevel() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_PreApprovalListFirstRoutingLevel") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_PreApprovalListFirstRoutingLevel"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_PreApprovalListFirstRoutingLevel") = value
        End Set

    End Property
    Protected Sub odsPreApprovalListFirstRoutingLevel_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsPreApprovalListFirstRoutingLevel.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetPreApproval_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetPreApproval_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_PreApprovalListFirstRoutingLevel = True
            Else
                LoadDataEmpty_PreApprovalListFirstRoutingLevel = False
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
    Protected Sub gvPreApprovalListFirstRoutingLevel_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPreApprovalListFirstRoutingLevel.RowCreated

        Try
            ''hide first column
            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            'End If

            'If e.Row.RowType = DataControlRowType.Footer Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            'End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_PreApprovalListFirstRoutingLevel
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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

    Private Property LoadDataEmpty_PreApprovalListSecondRoutingLevel() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_PreApprovalListSecondRoutingLevel") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_PreApprovalListSecondRoutingLevel"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_PreApprovalListSecondRoutingLevel") = value
        End Set

    End Property
    Protected Sub odsPreApprovalListSecondRoutingLevel_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsPreApprovalListSecondRoutingLevel.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetPreApproval_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetPreApproval_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_PreApprovalListSecondRoutingLevel = True
            Else
                LoadDataEmpty_PreApprovalListSecondRoutingLevel = False
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
    Protected Sub gvPreApprovalListSecondRoutingLevel_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPreApprovalListSecondRoutingLevel.RowCreated

        Try
            ''hide Second column
            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            'End If

            'If e.Row.RowType = DataControlRowType.Footer Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            'End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_PreApprovalListSecondRoutingLevel
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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
#End Region ' Insert Empty GridView Work-Around

    Protected Sub InsertCostingCoordinatorApproval()

        Try
            'when the costing coordinator notifies users that a Cost Sheet is ready, then the approval of the costing coordinator is complete      
            CostingModule.InsertCostSheetPreApproval(ViewState("CostSheetID"), ViewState("TeamMemberID"), 0, "A", 41)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnFirstLevelRoutingNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFirstLevelRoutingNotify.Click

        Try
            lblMessage.Text = ""

            Dim strEmailToAddress As String = ""

            'send costing coordinator aproval
            InsertCostingCoordinatorApproval()

            'get approvers
            strEmailToAddress = GetApproverList(1, cbFirstLevelRoutingNotifyOnlyNew.Checked, False)

            'if appovers exist, send email
            If strEmailToAddress <> "" Then
                If SendEmail(strEmailToAddress) = True Then
                    'lblMessage.Text &= "Notification was sent successfully."
                    gvPreApprovalListFirstRoutingLevel.DataBind()
                    gvPreApprovalHistory.DataBind()
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

    Protected Sub btnNotifyAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNotifyAll.Click

        Try
            lblMessage.Text = ""

            Dim strEmailToAddress As String = ""

            'send costing coordinator aproval
            InsertCostingCoordinatorApproval()

            'get approvers
            strEmailToAddress = GetApproverList(1, cbAllLevelRoutingNotifyOnlyNew.Checked, False)

            If strEmailToAddress <> "" Then
                strEmailToAddress &= ";"
            End If

            'get approvers
            strEmailToAddress &= GetApproverList(2, cbAllLevelRoutingNotifyOnlyNew.Checked, False)

            'if appovers exist, send email
            If strEmailToAddress <> "" Then
                If SendEmail(strEmailToAddress) = True Then
                    'lblMessage.Text &= "Notification was sent successfully."
                    gvPreApprovalListFirstRoutingLevel.DataBind()
                    gvPreApprovalListSecondRoutingLevel.DataBind()
                    gvPreApprovalHistory.DataBind()
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

    Protected Sub btnSecondLevelRoutingNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSecondLevelRoutingNotify.Click

        Try
            lblMessage.Text = ""

            Dim strEmailToAddress As String = ""

            'send costing coordinator aproval
            InsertCostingCoordinatorApproval()

            'get approvers
            strEmailToAddress = GetApproverList(2, cbSecondLevelRoutingNotifyOnlyNew.Checked, False)

            'if appovers exist, send email
            If strEmailToAddress <> "" Then
                If SendEmail(strEmailToAddress) = True Then
                    'lblMessage.Text &= "Notification was sent successfully."
                    gvPreApprovalListSecondRoutingLevel.DataBind()
                    gvPreApprovalHistory.DataBind()
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

    Protected Sub iBtnGetAnotherCostSheetPreApprovalList_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnGetAnotherCostSheetPreApprovalList.Click

        lblMessage.Text = ""

        Try
            Dim bFountIt As Boolean = False
            Dim iOldCostSheetID As Integer = 0

            If txtChooseCostSheetValue.Text.Trim <> "" Then
                iOldCostSheetID = CType(txtChooseCostSheetValue.Text.Trim, Integer)

                'check to make sure the other cost sheet actually exists
                Dim ds As DataSet
                'ds = CostingModule.GetCostSheet(iOldCostSheetID, "", 0, 0, 0, "", "", "", "", "", "", 0, 0, 0, "", 0, "", 0, 0, True, 41, False, False)
                ds = CostingModule.GetCostSheet(ViewState("CostSheetID"))

                If ds IsNot Nothing Then
                    If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                        If ds.Tables(0).Rows(0).Item("Obsolete") = False Then
                            bFountIt = True
                        End If
                    End If
                End If

                If bFountIt = True Then
                    CostingModule.CopyCostSheetPreApprovalList(ViewState("CostSheetID"), iOldCostSheetID)
                    lblMessage.Text &= "The list of approvers has been copied from a previous cost sheet."
                    gvPreApprovalListFirstRoutingLevel.DataBind()
                    gvPreApprovalListSecondRoutingLevel.DataBind()
                Else
                    lblMessage.Text &= "Error: The cost sheet to copy the approval list can not be found."
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

    Protected Sub iBtnGetFirstLevelPreApprovalList_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnGetPreApprovalListFromGroup.Click

        Try
            lblMessage.Text = ""

            If ddChooseNotificationGroupValue.SelectedIndex > 0 Then
                CostingModule.CopyNotificationGroupToPreApprovalList(ddChooseNotificationGroupValue.SelectedValue, ViewState("CostSheetID"))
                lblMessage.Text = "The list of approvers has been copied from a notification group."
                gvPreApprovalListFirstRoutingLevel.DataBind()
                gvPreApprovalListSecondRoutingLevel.DataBind()
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

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Try
            ViewState("isApproved") = False
            EnableControls()
            btnEdit.Visible = False

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
