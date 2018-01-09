' ************************************************************************************************
' Name:		RequestSupplierActivation.aspx
' Purpose:	This Code Behind is for the Supplier Request Look Up page. This page will be called from
'           various modules to allow team members to search or request new suppliers and include unapproved
'           suppliers as (f) future vendors in the drop down lists.
'
' Date		    Author	    
' 05/26/2011    LRey			Created .Net application
' ************************************************************************************************

Partial Class DataMaintenance_RequestSupplierActivation
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.lookupmasterpage_master = Master
            ' ''check test or production environments
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "Requesting Supplier Activation"
                mpTextBox.Font.Size = 18
                mpTextBox.Visible = True
                mpTextBox.Font.Bold = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*******
            '' Initialize ViewState
            ''*******
            If HttpContext.Current.Request.QueryString("pVType") <> "" Then
                ViewState("pVType") = HttpContext.Current.Request.QueryString("pVType")
            Else
                ViewState("pVType") = ""
            End If

            If HttpContext.Current.Request.QueryString("pVNO") <> "" Then
                ViewState("pVNO") = HttpContext.Current.Request.QueryString("pVNO")
            Else
                ViewState("pVNO") = ""
            End If

            If HttpContext.Current.Request.QueryString("pVName") <> "" Then
                ViewState("pVName") = HttpContext.Current.Request.QueryString("pVName")
            Else
                ViewState("pVName") = ""
            End If


            If HttpContext.Current.Request.QueryString("pForm") <> "" Then
                ViewState("pForm") = HttpContext.Current.Request.QueryString("pForm")
            Else
                ViewState("pForm") = ""
            End If

            If HttpContext.Current.Request.QueryString("pProjNo") <> "" Then
                ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
            Else
                ViewState("pProjNo") = ""
            End If



            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                BindData()
            End If
            'javascript:window.close();

            Dim strCloseWindow As String = "javascript:window.close();"
            btnClose.Attributes.Add("onclick", strCloseWindow)


        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

    Private Sub BindData()
        Try
            Dim ds As DataSet = New DataSet
            Dim dsCorpAcct As DataSet
            Dim iCorpAcctTMID As Integer = 0 'Used to locate Corporate Accounting 
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultTMName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value
            Dim EmailTo As String = Nothing
            Dim EmpName As String = Nothing
            Dim CurrentEmpEmail As String = Nothing
            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            lblErrors.Text = Nothing
            lblErrors.Visible = False
            lblMessage.Text = Nothing
            lblMessage.Visible = False

            If ViewState("pVNO") <> Nothing Then
                ''***********
                ''* Locate Corporate Accounting
                ''***********
                dsCorpAcct = commonFunctions.GetTeamMemberBySubscription(95)
                If dsCorpAcct IsNot Nothing Then
                    If dsCorpAcct.Tables.Count And dsCorpAcct.Tables(0).Rows.Count > 0 Then
                        iCorpAcctTMID = dsCorpAcct.Tables(0).Rows(0).Item("TMID")
                    End If
                End If

                ''***********************************************************************
                ''Notify Corporate Accounting for Requesting an Activation of Supplier
                ''***********************************************************************
                ds = SecurityModule.GetTeamMember(iCorpAcctTMID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
                ''Check that the recipient(s) is a valid Team Member
                If ds.Tables.Count > 0 And (ds.Tables.Item(0).Rows.Count > 0) Then
                    For i = 0 To ds.Tables.Item(0).Rows.Count - 1
                        If (ds.Tables(0).Rows(i).Item("Email") <> Nothing) Or (ds.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                            If EmailTo = Nothing Then
                                EmailTo = ds.Tables(0).Rows(i).Item("Email")
                            Else
                                EmailTo = EmailTo & ";" & ds.Tables(0).Rows(i).Item("Email")
                            End If
                            If EmpName = Nothing Then
                                EmpName = ds.Tables(0).Rows(i).Item("FirstName") & " " & ds.Tables(0).Rows(i).Item("LastName") & ", "
                            Else
                                EmpName = EmpName & ds.Tables(0).Rows(i).Item("FirstName") & " " & ds.Tables(0).Rows(i).Item("LastName") & ", "
                            End If
                        End If
                    Next


                    lblTo.Text = EmailTo
                    lblFrom.Text = CurrentEmpEmail
                    lblSubject.Text = "Requesting Supplier Activation"
                    lblBody.Text = "<font size='2' face='Tahoma'>" & EmpName
                    lblBody.Text &= "<p>Supplier #<b>" & ViewState("pVNO") & "</b> for <b>" & ViewState("pVName") & "</b> was in the BPCS Vendor Master as 'INACTIVE'. The Vendor Type is <b>" & ViewState("pVType") & "</b>.<br/><br/>Please confirm if this supplier exists in Oracle. If it does not, please add a new Supplier entry in Oracle and be sure to reference the original BPCS Vendor number. Notify Lynette Rey with the correct Oracle Supplier number for correction in the UGN Database.</p>"
                    lblBody.Text &= "<p>Thank you,<br/>" & DefaultTMName & "</p></font>"

                    SubmitRequestActivation(ViewState("pVType"), ViewState("pVNO"), ViewState("pVName"))

                End If 'EOF If ds.Tables.Count > 0.....
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindData
    Public Function SubmitRequestActivation(ByVal VendorType As String, ByVal VendorNo As String, ByVal VendorName As String) As String
        Try
            Dim ds As DataSet = New DataSet
            Dim dsCorpAcct As DataSet
            Dim iCorpAcctTMID As Integer = 0 'Used to locate Corporate Accounting 
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultTMName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value
            Dim EmailTo As String = Nothing
            Dim EmpName As String = Nothing
            Dim CurrentEmpEmail As String = Nothing
            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            lblErrors.Text = Nothing
            lblErrors.Visible = False

            ''***********
            ''* Locate Corporate Accounting
            ''***********
            dsCorpAcct = commonFunctions.GetTeamMemberBySubscription(95)
            If dsCorpAcct IsNot Nothing Then
                If dsCorpAcct.Tables.Count And dsCorpAcct.Tables(0).Rows.Count > 0 Then
                    iCorpAcctTMID = dsCorpAcct.Tables(0).Rows(0).Item("TMID")
                End If
            End If

            ''***********************************************************************
            ''Notify Corporate Accounting for Requesting an Activation of Supplier
            ''***********************************************************************
            ds = SecurityModule.GetTeamMember(iCorpAcctTMID, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''Check that the recipient(s) is a valid Team Member
            If ds.Tables.Count > 0 And (ds.Tables.Item(0).Rows.Count > 0) Then
                For i = 0 To ds.Tables.Item(0).Rows.Count - 1
                    If (ds.Tables(0).Rows(i).Item("Email") <> Nothing) Or (ds.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) Then
                        If EmailTo = Nothing Then
                            EmailTo = ds.Tables(0).Rows(i).Item("Email")
                        Else
                            EmailTo = EmailTo & ";" & ds.Tables(0).Rows(i).Item("Email")
                        End If
                        If EmpName = Nothing Then
                            EmpName = ds.Tables(0).Rows(i).Item("FirstName") & " " & ds.Tables(0).Rows(i).Item("LastName") & ", "
                        Else
                            EmpName = EmpName & ds.Tables(0).Rows(i).Item("FirstName") & " " & ds.Tables(0).Rows(i).Item("LastName") & ", "
                        End If

                    End If
                Next
            End If 'EOF If ds.Tables.Count > 0.....

            If EmailTo <> Nothing Then
                Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                Dim SendTo As MailAddress = Nothing
                Dim MyMessage As MailMessage
                'send to Test or Production
                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    SendTo = New MailAddress(CurrentEmpEmail) 'use for testing only
                    MyMessage = New MailMessage(SendFrom, SendTo)
                    MyMessage.Bcc.Add("lynette.rey@ugnauto.com")
                Else
                    MyMessage = New MailMessage
                    'build email To list
                    Dim emailList As String() = EmailTo.Split(";")

                    For i = 0 To UBound(emailList)
                        If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                            MyMessage.To.Add(emailList(i))
                        End If
                    Next i
                    MyMessage.From = New MailAddress(CurrentEmpEmail)
                    MyMessage.CC.Add(CurrentEmpEmail)
                    MyMessage.Bcc.Add("lynette.rey@ugnauto.com")
                End If


                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    MyMessage.Subject = "TEST: "
                    MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br><br>"
                Else
                    MyMessage.Subject = ""
                    MyMessage.Body = ""
                    'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br><br>"
                End If

                MyMessage.Subject &= "Requesting Supplier Activation"
                MyMessage.Body = "<font size='2' face='Tahoma'>" & EmpName
                'MyMessage.Body &= "<p>Supplier #<b>" & ViewState("pVNO") & "</b> for <b>" & ViewState("pVName") & "</b> is in the BPCS Vendor Master as 'INACTIVE'. The Vendor Type is <b>" & ViewState("pVType") & "</b>.<br/><br/>Please reactivate this supplier so that it appears to be a valid selection in the UGN Database.</p>"
                MyMessage.Body &= "<p>Supplier #<b>" & ViewState("pVNO") & "</b> for <b>" & ViewState("pVName") & "</b> was in the BPCS Vendor Master as 'INACTIVE'. The Vendor Type is <b>" & ViewState("pVType") & "</b>.<br/><br/>Please confirm if this supplier exists in Oracle. If it does not, please add a new Supplier entry in Oracle and be sure to reference the original BPCS Vendor number. Notify Lynette Rey with the correct Oracle Supplier number for correction in the UGN Database.</p>"
                MyMessage.Body &= "<p>Thank you,<br/>" & DefaultTMName & "</p></font>"

                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    MyMessage.Body &= "<p>EmailTO: " & EmailTo & "</p>"
                End If

                ''**********************************
                ''Connect & Send email notification
                ''**********************************
                MyMessage.IsBodyHtml = True
                Dim emailClient As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

                emailClient.Send(MyMessage)

                Dim pForm As String = ViewState("pForm")
                Select Case pForm
                    Case "EXPKG"
                        ds = EXPModule.GetExpProjPackaging(ViewState("pProjNo"), "", "", "", 0, "", 0, "", "", "")
                        If (ds.Tables.Item(0).Rows.Count > 0) Then
                            Dim ProjectTitle As String = ds.Tables(0).Rows(0).Item("ProjectTitle").ToString()
                            EXPModule.InsertExpProjPackagingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, " Requested Activation of Supplier " & VendorNo & " - " & VendorName, "", "", "", "")

                        End If
                End Select

                lblMessage.Text = "Email notification sent successfully."
                lblMessage.Visible = True
            Else
                lblErrors.Text = "Unable to locate a valid email address to send message to. Please contact the Application Department for assistance."
                lblErrors.Visible = True
            End If 'EOF IF EmailTo <> Nothing



            Return True

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            Return False
        End Try

    End Function 'EOF SubmitRequestActivation
End Class
