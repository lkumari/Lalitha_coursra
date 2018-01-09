Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic

''' ==============================================================
'''  File:       ActiveDirectoryLookup.aspx
''' 
'''  Purpose:    This page displays the Active Directory user list.
'''              It is designed to be opened as a popup-window from
'''              a parent form.
''' 
'''  Language:   VB.NET 2005
''' 
'''  Written by: M. Weyker 2/27/2008
''' 
'''  ADPopUp.ASPX is a page that gets displayed in a popup window
'''  from the GetADUserName.aspx (parent) page.
''' 
'''  This page displays the Active Directory users in a GridView,
'''  The controls (2 TextBoxes and a DropDownList) at the top of the page are used for
'''  filtering the GridView data.
''' 
'''  This page initially receives the IDs of four TextBox controls from the parent form
'''  via a query string, which are then stored in ViewState. When a user selects a row
'''  from the GridView, the four controls on the parent form are populated with the selected
'''  user's data, and this form is closed.
''' 
'''  Modification History
'''  --------------------
''' 
'''  2/27/2008 mw: Added JavaScript code "window.opener.Page_ClientValidate();"
'''    to SendDataBackToParentForm procedure to force validation on the 
'''    parent form.
''' 
'''  3/29/2008 mw: Added onblur="window.focus()" to keep this window
'''    in focus, until it is closed.
''' 
'''  04/15/2008 mw: 
'''     1. Added #Regions to code.
'''     2. Provide AccountName from username portion of email address.
'''        This was a work-around for some usernames that were all
'''        lower-case.
''' 
'''  08/25/2008  MWeyker    Added standard exception reporting,
'''                         using UGNErrorTrapping class.	
''' 
''' ==============================================================

Partial Class Security_ActiveDirectoryLookup
    Inherits System.Web.UI.Page


#Region "Loading and Initialization"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                ' Save the QueryString controls in ViewState
                If Request.QueryString("textbox1") IsNot Nothing Then
                    ViewState("textbox1") = Request.QueryString("textbox1").ToString()
                End If
                If Request.QueryString("textbox2") IsNot Nothing Then
                    ViewState("textbox2") = Request.QueryString("textbox2").ToString()
                End If
                If Request.QueryString("textbox3") IsNot Nothing Then
                    ViewState("textbox3") = Request.QueryString("textbox3").ToString()
                End If
                If Request.QueryString("textbox4") IsNot Nothing Then
                    ViewState("textbox4") = Request.QueryString("textbox4").ToString()
                End If
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' Page_Load

#End Region ' Loading and Initialization


#Region "Event Handlers"

    Protected Sub gvUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvUsers.SelectedIndexChanged
        Try
            ' The user has selected an item from the GridView.
            ' Send the data back to the parent form.
            Dim row As GridViewRow = gvUsers.SelectedRow
            Dim strAccountName As String = row.Cells(3).Text
            Dim strEmail As String = row.Cells(4).Text
            Dim strAccountCorrectCase As String = UsernameFromEmail(strAccountName, strEmail)
            SendDataBackToParentForm(row.Cells(1).Text, row.Cells(2).Text, strAccountCorrectCase, strEmail)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' gvUsers_SelectedIndexChanged

    ' Clear the search fields.
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            txtLname.Text = ""
            txtFname.Text = ""
            ddlLocation.SelectedIndex = 0
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' btnReset_Click

#End Region ' Event Handlers


#Region "Private Methods"

    ''' <summary>
    ''' Extract the username from the e-mail address
    ''' </summary>
    ''' <param name="AccountName">AccountName to validate against</param>
    ''' <param name="Email">e-mail address</param>
    ''' <returns>username portion of Email</returns>
    ''' <remarks>The purpose of this method is to return the username with correct case.
    ''' The username from Email is returned, if it matches AccountName. 
    ''' Otherwise, AccountName is returned.</remarks>
    Private Function UsernameFromEmail(ByVal AccountName As String, ByVal Email As String) As String
        ' Use AccountName as the default return value
        Dim strResult As String = AccountName
        Try
            ' Split Email before and after the "@"
            ' The first string will be the username.
            Dim split As String() = Email.Split(New [Char]() {"@"c})

            For Each s As String In split
                ' do a case-insensitive compare of username with AccountName
                If s.ToLower().Equals(AccountName.ToLower()) Then
                    ' They are equal.
                    ' OK to return the username
                    strResult = s
                End If
                Exit For  ' one time only
            Next
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
        Return strResult
    End Function ' UsernameFromEmail

    Private Sub SendDataBackToParentForm(ByVal s1 As String, ByVal s2 As String, ByVal s3 As String, ByVal s4 As String)
        Try
            ' If the control id's of the parent form are available for receiving data, continue.
            ' Otherwise, exit this procedure.
            If ViewState("textbox1") Is Nothing Then
                Exit Sub
            End If
            If ViewState("textbox2") Is Nothing Then
                Exit Sub
            End If
            If ViewState("textbox3") Is Nothing Then
                Exit Sub
            End If
            If ViewState("textbox4") Is Nothing Then
                Exit Sub
            End If

            ' Build client JavaScript code:
            '  1. populate textboxes on parent form
            '  2. re-validate the parent form
            Dim strScript As String = _
                "<script>window.opener.document.forms[0]." & ViewState("textbox1").ToString() & ".value = '" & s1 & "';" & _
                "window.opener.document.forms[0]." & ViewState("textbox2").ToString() & ".value = '" & s2 & "';" & _
                "window.opener.document.forms[0]." & ViewState("textbox3").ToString() & ".value = '" & s3 & "';" & _
                "window.opener.document.forms[0]." & ViewState("textbox4").ToString() & ".value = '" & s4 & "';" & _
                "self.close();</script>"
            '"window.opener.Page_ClientValidate();" & _   DISABLE
            Dim cstype As Type = Me.GetType()
            ClientScript.RegisterClientScriptBlock(cstype, "test", strScript)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' SendDataBackToParentForm

#End Region ' Private Methods

End Class ' Security_ActiveDirectoryLookup
