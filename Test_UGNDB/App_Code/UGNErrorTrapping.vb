' ************************************************************************************************
' Name:		UGNErrorTrapping.vb
' Purpose:	This code is referenced from all vb files, mostly for the purpose of handling errors
'
' Date		        Author	    
' 06/05/2008      	Roderick Carlson			Created .Net application
' 07/21/2008        Roderick Carlson            Made collection of fields and their values global, added error trapping to error trapping functions
' 09/26/2008        Roderick Carlson            Checked Special Characters when saving to database
' 02/06/2009        Roderick Carlson            Made sure panel had a value before trying to loop through it.
' 01/11/2011        Roderick Carlson            Add an email queue when UGN Database Email Notifications fail. This may occur when the email systems are not available.
' ************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Text
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.Web.UI.WebControls

Imports Microsoft.VisualBasic

Public Class UGNErrorTrapping
    Public Shared Sub UpdateUGNErrorLog(ByVal ErrorMessage As String, ByVal WebPagePath As String)

        'get current ASPX Field Names and their values
        'Dim strScreenData As String = getFieldNamesAndValues()
        Dim strScreenData As String = UGNErrorTrapping.getWebFormFieldNamesAndValues

        'get current QueryString Names and their values
        Dim strQueryStringData As String = UGNErrorTrapping.getQueryStringNamesAndValues()

        'get edited grid view fields and their values (still needing some research)
        'Dim strGridViewData As String = getEditedGridViewCells()

        'combine all collected values
        Dim strUserEditedData As String = strQueryStringData
        If strScreenData.Trim <> "" Then
            strUserEditedData += vbNewLine & strScreenData
        End If

        'If strGridViewData.Trim <> "" Then
        'strUserEditedData += vbNewLine & strGridViewData
        'End If

        'HttpContext.Current.Response.Write("end of UpdateUGNErrorLog<br>")
        'HttpContext.Current.Response.Write(ErrorMessage & "<br>")
        'HttpContext.Current.Response.Write(WebPagePath & "<br>")
        'HttpContext.Current.Response.Write(strUserEditedData & "<br>")
        'log error in Database - a trigger will email us.
        UGNErrorTrapping.InsertErrorLog(ErrorMessage, WebPagePath, strUserEditedData)

    End Sub
    Public Shared Function getWebFormFieldNamesAndValues() As String

        Try
            Dim objCurrentHandler As Object = HttpContext.Current.CurrentHandler
            Dim pageCurrentPage As System.Web.UI.Page = CType(objCurrentHandler, System.Web.UI.Page)
            Dim ScreenData As String = "Error: No fields were found on the form"

            'get a reference to the masterfile's 
            'Content place holder object.
            Dim content As ContentPlaceHolder
            'content = Page.Master.FindControl("maincontent")
            If pageCurrentPage.Master.FindControl("maincontent") IsNot Nothing Then
                content = pageCurrentPage.Master.FindControl("maincontent")

                'get a reference to panel object that is in 
                'the ASPX page associated with the master page.
                Dim mypanel As Panel
                mypanel = content.FindControl("localPanel")

                Dim ctl As Control

                If mypanel IsNot Nothing Then
                    ScreenData = ""

                    'Searches through each control in the form
                    For Each ctl In mypanel.Controls
                        'ScreenData += ctl.ID & ":" & vbNewLine

                        Dim tb As TextBox
                        If TypeOf ctl Is TextBox Then
                            tb = CType(ctl, TextBox)
                            If tb.Text.Trim <> "" Then
                                ScreenData += ctl.ID & " : " '& vbNewLine
                                ScreenData += tb.Text & vbNewLine
                            End If

                        End If

                        Dim cb As CheckBox
                        If TypeOf ctl Is CheckBox Then
                            cb = CType(ctl, CheckBox)
                            ScreenData += ctl.ID & " : " '& vbNewLine
                            ScreenData += cb.Checked.ToString & vbNewLine
                        End If

                        Dim dd As DropDownList
                        If TypeOf ctl Is DropDownList Then
                            dd = CType(ctl, DropDownList)
                            If dd.SelectedIndex > 0 Then
                                ScreenData += ctl.ID & " : " '& vbNewLine
                                ScreenData += dd.SelectedValue & vbNewLine
                            End If

                        End If

                        Dim rb As RadioButton
                        If TypeOf ctl Is RadioButton Then
                            rb = CType(ctl, RadioButton)
                            ScreenData += ctl.ID & " : " '& vbNewLine
                            ScreenData += rb.Checked.ToString & vbNewLine
                        End If
                    Next
                    'Else
                    '    ScreenData = "Error: No fields were found on the form"
                End If

            End If

            getWebFormFieldNamesAndValues = ScreenData

        Catch ex As Exception
            getWebFormFieldNamesAndValues = "No Fields or their values could be found due to an error in the UGNErrorTrapping - getUGNFieldNamesAndValues Function - Error :" & ex.Message
        End Try

    End Function
    Public Shared Function getQueryStringNamesAndValues() As String

        Try
            Dim loop1, loop2 As Integer
            Dim arr1(), arr2() As String
            Dim coll As Collections.Specialized.NameValueCollection
            Dim QueryStringData As String = ""

            ' Load Form variables into NameValueCollection variable.
            coll = HttpContext.Current.Request.QueryString

            ' Get names of all keys into a string array.
            arr1 = coll.AllKeys
            For loop1 = 0 To arr1.GetUpperBound(0)
                'QueryStringData += HttpContext.Current.Server.HtmlEncode(arr1(loop1)) & ":"

                ' Get all values under this key.
                arr2 = coll.GetValues(loop1)
                For loop2 = 0 To arr2.GetUpperBound(0)

                    If HttpContext.Current.Server.HtmlEncode(arr2(loop2)) IsNot Nothing Then
                        QueryStringData += HttpContext.Current.Server.HtmlEncode(arr1(loop1)) & " : " '& vbNewLine
                        'QueryStringData += CStr(loop2) & ": " & HttpContext.Current.Server.HtmlEncode(arr2(loop2)) & vbNewLine
                        QueryStringData += HttpContext.Current.Server.HtmlEncode(arr2(loop2)) & vbNewLine
                    End If
                Next loop2
            Next loop1

            getQueryStringNamesAndValues = QueryStringData
        Catch ex As Exception
            getQueryStringNamesAndValues = "No Query String Parameters or their values could be found due to an error in the UGNErrorTrapping - getQueryStringNamesAndValues Function - Error :" & ex.Message
        End Try

    End Function
    Public Shared Function GetErrorLog() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_UGN_Error_Log"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ErrorInfo")
            GetErrorLog = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetErrorLog : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> UGNErrorTrapping.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetErrorLog = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Sub InsertErrorLog(ByVal ErrorMessage As String, ByVal FormName As String, ByVal ScreenData As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_UGN_Error_Log"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        'HttpContext.Current.Response.Write("InsertErrorLog: before try<br>")

        Try
            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                Dim FullName As String = commonFunctions.getUserName()
                Dim LocationOfDot As Integer = InStr(FullName, ".")
                Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                Dim FirstInitial As String = Left(FullName, 1)
                Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)
                HttpContext.Current.Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
            End If

            'HttpContext.Current.Response.Write("InsertErrorLog: got user<br>")

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@errorMessage", SqlDbType.VarChar)
            myCommand.Parameters("@errorMessage").Value = commonFunctions.convertSpecialChar(ErrorMessage, False)

            myCommand.Parameters.Add("@formName", SqlDbType.VarChar)
            myCommand.Parameters("@formName").Value = FormName

            myCommand.Parameters.Add("@screenData", SqlDbType.VarChar)
            myCommand.Parameters("@screenData").Value = commonFunctions.convertSpecialChar(ScreenData, False)

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'HttpContext.Current.Response.Write(HttpContext.Current.Request.Cookies("UGNDB_User").Value & "<br>")

            'HttpContext.Current.Response.Write("InsertErrorLog: before open query<br>")

            myConnection.Open()

            'HttpContext.Current.Response.Write("InsertErrorLog: after open query<br>")

            myCommand.ExecuteNonQuery()

            'HttpContext.Current.Response.Write("InsertErrorLog: after ExecuteNonQuery<br>")
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ErrorMessage:" & ErrorMessage & ", FormName: " & FormName & ", ScreenData: " & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertErrorLog : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> UGNErrorTrapping.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            'HttpContext.Current.Response.Write("end of InsertErrorLog: EXCEPTION" & ex.Message & "<br>")

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

        'HttpContext.Current.Response.Write("end of InsertErrorLog")
    End Sub

    Public Shared Sub InsertEmailQueue(ByVal ModuleName As String, _
       ByVal EmailFromAddress As String, ByVal EmailToAddress As String, _
       ByVal EmailCCAddress As String, ByVal EmailSubject As String, ByVal EmailBody As String, _
       ByVal EmailFileAttachmentName As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Email_Queue"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If ModuleName Is Nothing Then
                ModuleName = "unknown"
            End If

            myCommand.Parameters.Add("@ModuleName", SqlDbType.VarChar)
            myCommand.Parameters("@ModuleName").Value = ModuleName

            If EmailFromAddress Is Nothing Then
                EmailFromAddress = "Notifications@ugnauto.com"
            End If

            myCommand.Parameters.Add("@EmailFromAddress", SqlDbType.VarChar)
            myCommand.Parameters("@EmailFromAddress").Value = EmailFromAddress

            If EmailToAddress Is Nothing Then
                EmailToAddress = "Lynette.Rey@ugnauto.com"
            End If

            myCommand.Parameters.Add("@EmailToAddress", SqlDbType.VarChar)
            myCommand.Parameters("@EmailToAddress").Value = EmailToAddress

            If EmailCCAddress Is Nothing Then
                EmailCCAddress = ""
            End If

            myCommand.Parameters.Add("@EmailCCAddress", SqlDbType.VarChar)
            myCommand.Parameters("@EmailCCAddress").Value = EmailCCAddress

            If EmailSubject Is Nothing Then
                EmailSubject = "UGN Database Email Notification Error while sending to UGN DB Mail Queue"
            End If

            myCommand.Parameters.Add("@EmailSubject", SqlDbType.VarChar)
            myCommand.Parameters("@EmailSubject").Value = EmailSubject

            If EmailBody Is Nothing Then
                EmailBody = "There was an error sending the email notification."
            End If

            myCommand.Parameters.Add("@EmailBody", SqlDbType.VarChar)
            myCommand.Parameters("@EmailBody").Value = EmailBody

            If EmailFileAttachmentName Is Nothing Then
                EmailFileAttachmentName = ""
            End If

            myCommand.Parameters.Add("@EmailFileAttachmentName", SqlDbType.VarChar)
            myCommand.Parameters("@EmailFileAttachmentName").Value = EmailFileAttachmentName

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ModuleName: " & ModuleName _
            & ", EmailFromAddress: " & EmailFromAddress _
            & ", EmailToAddress: " & EmailToAddress _
            & ", EmailCCAddress : " & EmailCCAddress _
            & ", EmailSubject: " & EmailSubject _
            & ", EmailBody: " & EmailBody _
            & ", EmailFileAttachmentName: " & EmailFileAttachmentName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertEmailNotifyQueue : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> UGNErrorTrapping.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertEmailNotifyQueue : " & commonFunctions.convertSpecialChar(ex.Message, False), "UGNErrorTrapping.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
End Class
