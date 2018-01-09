' ************************************************************************************************
'
' Name:		CostSheetTemplateMaint.aspx
' Purpose:	This Code Behind is to maintain the template used by the Costing Module
'
' Date		Author	    
' 10/14/2008 RCarlson
'   NEED TO ADD FUNCTIONALITY - IF A NEW TEMPLATE IS CREATED, THE APP GROUP NEEDS TO BE NOTIFIED TO CHECK CRYSTAL REPORTS AND OTHER CALCULATIONS
' 01/11/2011 Roderick Carlson - Modified - Added Email Queue
' ************************************************************************************************
Partial Class Template_Maint
    Inherits System.Web.UI.Page
    Protected Function SendEmail(ByVal TemplateName As String) As Boolean

        'Many of the crystal reports and calculations depend on the ID's of the formulas. So it would behoove us to track new formulas and templates
        Dim bReturnValue As Boolean = False

        Dim strEmailToAddress As String = "Roderick.Carlson@ugnauto.com"

        Try

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim strBody As String = ""
            Dim strSubject As String = ""

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!" & Chr(13) & Chr(13)
            End If

            Dim mail As New MailMessage()

            strSubject += "A new template was created in the UGNDB Costing Module: " & TemplateName

            strBody += "A new template was created in the UGNDB Costing Module: " & TemplateName & Chr(13) & Chr(13)

            strBody += "The user who created the new template was " & strCurrentUser & Chr(13) & Chr(13)

            strBody += "Please check to see if any previews need to be updated." & Chr(13) & Chr(13)

            strBody = strBody & "Thank you." & Chr(13) & Chr(13)

            'When in testing mode, just use developer email address.           
            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody += "Email To Address List: " & strEmailToAddress & Chr(13) & Chr(13)

                strEmailToAddress = "Roderick.Carlson@ugnauto.com"
            End If

            'set the content           
            mail.Subject = strSubject
            mail.Body = strBody

            'set the addresses
            mail.From = New MailAddress(strEmailFromAddress)
            Dim i As Integer

            strEmailToAddress = Replace(strEmailToAddress, ";;", ";")

            'to list
            Dim emailList As String() = strEmailToAddress.Split(";")

            For i = 0 To UBound(emailList)
                If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                    mail.To.Add(emailList(i))
                End If
            Next i

            'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")

            ' ''send the message 
            ''Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            ''smtp.Send(mail)
            'Try
            '    smtp.Send(mail)
            '    lblMessage.Text &= "Email Notification sent."
            'Catch ex As Exception
            '    lblMessage.Text &= "Email Notification queued."
            '    UGNErrorTrapping.InsertEmailQueue("Costing Pre Approval Notification", strEmailFromAddress, EmailToAddress, strEmailCCAddress, strSubject, strBody, "")
            'End Try

            bReturnValue = True

        Catch ex As Exception
            
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & ", To Email Addresses: " & strEmailToAddress & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        Return bReturnValue

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
            Dim iRoleID As Integer = 0

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 63)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
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
                End If
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub EnableControls()

        Try
            If ViewState("isRestricted") = False Then

                gvTemplate.Columns(gvTemplate.Columns.Count - 1).Visible = ViewState("isAdmin")
                If gvTemplate.FooterRow IsNot Nothing Then
                    gvTemplate.FooterRow.Visible = ViewState("isAdmin")
                End If
            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
                gvTemplate.Visible = False
                lblSearchTip.Visible = False
                lblTemplateName.Visible = False
                txtSearchTemplateName.Visible = False
                btnReset.Visible = False
                btnSearch.Visible = False
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
        
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Template Maintenance"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > Template Maintenance "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            CheckRights()

            If Not Page.IsPostBack Then
                'clear crystal reports
                CostingModule.CleanCostingCrystalReports()

                ' ''******
                ''get saved value of past search criteria or query string, query string takes precedence
                ' ''******

                If HttpContext.Current.Request.QueryString("TemplateName") <> "" Then
                    txtSearchTemplateName.Text = HttpContext.Current.Request.QueryString("TemplateName")
                End If

            End If

            EnableControls()

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            lblMessage.Text = ""

            Response.Redirect("Template_Maint.aspx?TemplateName=" & Server.UrlEncode(txtSearchTemplateName.Text.Trim), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            lblMessage.Text = ""

            Response.Redirect("Template_Maint.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvTemplate_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTemplate.DataBound

        'hide header of first column
        If gvTemplate.Rows.Count > 0 Then
            gvTemplate.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvTemplate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTemplate.RowCommand

        Try

            Dim txtTemplateNameTemp As TextBox
            Dim cbObsoleteTemp As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                txtTemplateNameTemp = CType(gvTemplate.FooterRow.FindControl("txtFooterTemplateName"), TextBox)
                cbObsoleteTemp = CType(gvTemplate.FooterRow.FindControl("cbFooterObsolete"), CheckBox)

                odsTemplate.InsertParameters("TemplateName").DefaultValue = txtTemplateNameTemp.Text.Trim
                odsTemplate.InsertParameters("Obsolete").DefaultValue = cbObsoleteTemp.Checked

                intRowsAffected = odsTemplate.Insert()

                If txtTemplateNameTemp.Text.Trim <> "" Then
                    SendEmail(txtTemplateNameTemp.Text)
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvTemplate.ShowFooter = False
            Else
                gvTemplate.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtTemplateNameTemp = CType(gvTemplate.FooterRow.FindControl("txtFooterTemplateName"), TextBox)
                txtTemplateNameTemp.Text = Nothing

                cbObsoleteTemp = CType(gvTemplate.FooterRow.FindControl("cbFooterObsolete"), CheckBox)
                cbObsoleteTemp.Checked = False

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

#Region "Insert Empty GridView Work-Around"
    Private Property LoadDataEmpty_Template() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Template") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Template"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Template") = value
        End Set

    End Property
    Protected Sub odsTemplate_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsTemplate.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.Template_MaintDataTable = CType(e.ReturnValue, Costing.Template_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Template = True
            Else
                LoadDataEmpty_Template = False
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
    Protected Sub gvTemplate_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTemplate.RowCreated

        Try
            'hide first column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Template
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
#End Region
End Class
