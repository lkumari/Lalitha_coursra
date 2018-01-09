''******************************************************************************************************
''* InvalidUser.vb
''* This error page is shown if the user does not have a proper UGN DB account.
''*  
''*
''* Author  : Roderick Carlson 2011-Nov-17
''* Modified: {Name} {Date} - {Notes}
''*           
''******************************************************************************************************

Partial Class InvalidUser
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Request.QueryString("UserName") IsNot Nothing And Request.QueryString("UserEmail") IsNot Nothing Then
                Dim strUserName As String = Request.QueryString("UserName").ToString
                Dim strUserEmail As String = Request.QueryString("UserEmail").ToString

                lblMessage.Text = "The following user, " & strUserName & ", is attempting to connect to the UGN Database but does not have access."

                If strUserName <> "" And strUserEmail <> "" Then
                    SendEmail(strUserName, strUserEmail)
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br>" & ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub SendEmail(ByVal UserName As String, ByVal EmailToAddress As String)

        Dim bReturnValue As Boolean = False

        Try

            Dim strSubject As String = ""
            Dim strBody As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim strEmailFromAddress As String = EmailToAddress

            Dim strEmailToAddress As String = EmailToAddress

            'handle test environment
            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br><br>"
            End If

            strSubject = strSubject & "Invalid User connecting to the UGN Database. Please obtain permission: " & UserName
            strBody &= "<font color='red' size='3' face='Verdana'><b><br>" & UserName & ",<br>The system has not found your account information in the UGN Database.</b></font>"

            strBody &= "<br><br>The following reasons might cause this error:<br />"
            strBody &= "<li>Your UGN Email address or name has changed</li>"            
            strBody &= "<li>You do not have an account in the UGN Database.</li>"
            strBody &= "<br>Please contact the Applications Group at the corporate office: <a href='mailto:TNPISAppGrp@ugnauto.com'><u>Click Here to send an email for support.</u></a>"
            strBody &= "<br><br><b>Please have the HR department and/or your supervisor complete the following Docushare SOP Documents:</b>"

            strBody &= "<br><a font-size='larger' href='http://tapsd.ugnnet.com:8080/docushare/dsweb/Get/Document-1576/IS107_-_Network_Account_Request_Form.doc' target='_blank'><u>IS-107</u></a>"

            strBody &= "<br><a font-size='larger' href='http://tapsd.ugnnet.com:8080/docushare/dsweb/Get/Document-7802/IS110%20-%20UGN%20Database%20Access%20Sign-in%20Sheet.doc' target='_blank'><u>IS-110</u></a>"

            strBody &= "<br><br>If you had trouble with the direct links to the forms mentioned above, please open Docushare and follow the &quot Document Control&quot folder to the &quot(IS) Information Systems Documents&quot folder, and you will see the forms. Docushare can be opened by <a href='http://tapsd.ugnnet.com:8080/docushare' target='_blank'><u>clicking here</u></a>"

            strBody &= "<br><br><font size='1' face='Verdana'>Thank You.</font>"

            'create the mail message using new System.Net.Mail (not CDonts)
            Dim mail As New MailMessage()

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody &= "<br><br>Email To Address List: " & EmailToAddress & "<br>"                

                strEmailToAddress = "Roderick.Carlson@ugnauto.com"               
            End If

            strBody &= "<br><br><font size='1' face='Verdana'>"
            strBody &= "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++"
            strBody &= "<br>If you are receiving this by error, please submit the problem with the UGN Database Requestor."
            strBody &= "<br>Please <u>do not</u> reply back to this email because you will not receive a response."
            strBody &= "<br>Please use a separate email or phone call the appropriate UGN Team Member(s) for any questions or concerns you may have.<br>"
            strBody &= "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++</font>"

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
                'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
                mail.Bcc.Add("Lynette.Rey@ugnauto.com")
            End If

            mail.IsBodyHtml = True

            'send the message 
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(mail)
                lblMessage.Text &= "<br>Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "<br>Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("Invalid UGNDB User", strEmailFromAddress, EmailToAddress, "", strSubject, strBody, "")
            End Try

            bReturnValue = True

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br>" & ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

End Class
