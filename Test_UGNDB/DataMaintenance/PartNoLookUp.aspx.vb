' ************************************************************************************************
'
' Name:		PartNoLookUp.aspx.vb

' Purpose:	This Code Behind to search BPCS Part Numbers for the any module that needs to return the part from the BPCS Item Master and its revision
'
' Date		    Author	    
' 04/14/2009    Roderick Carlson
' 11/27/2012    Roderick Carlson - do not return extra spaces to parent page
' 12/18/2013    LRey    Replaced "PartNo" to "PartNo" wherever used. 
' ************************************************************************************************
Partial Class PartNoLookUp
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lblMessage.Text = ""

        Try
            If Not Page.IsPostBack Then
                Dim FullName As String = commonFunctions.getUserName()
                If FullName = Nothing Then
                    FullName = "Demo.Demo"  '* This account has restricted read only rights.
                End If

                Dim LocationOfDot As Integer = InStr(FullName, ".")
                If LocationOfDot > 0 Then
                    Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                    Dim FirstInitial As String = Left(FullName, 1)
                    Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                    Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
                Else
                    Response.Cookies("UGNDB_User").Value = FullName
                End If

                ' Save the QueryString controls in ViewState
                If Request.QueryString("vcPartNo") IsNot Nothing Then
                    ViewState("vcPartNo") = Request.QueryString("vcPartNo").ToString()
                End If

                If Request.QueryString("vcPartRevision") IsNot Nothing Then
                    ViewState("vcPartRevision") = "" ''Request.QueryString("vcPartRevision").ToString()
                End If

                If Request.QueryString("vcPartDescr") IsNot Nothing Then
                    ViewState("vcPartDescr") = Request.QueryString("vcPartDescr").ToString()
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


    Private Sub SendDataBackToParentForm(ByVal PartNo As String, ByVal PartRevision As String, ByVal PartName As String)

        Try

            ' Build client JavaScript code:
            '  1. populate textboxes on parent form
            '  2. re-validate the parent form
            'Dim strScript As String = _
            '    "<script>window.opener.document.forms[0]." & ViewState("vcPartNo").ToString() & ".value = '" & PartNo & "';" & _
            '    "window.opener.document.forms[0]." & ViewState("vcPartRevision").ToString() & ".value = '" & PartRevision & "';" & _
            '    "window.opener.document.forms[0]." & ViewState("vcPartDescr").ToString() & ".value = '" & PartName & "';" & _
            '    "self.close();</script>"
            ''"window.opener.Page_ClientValidate();" & _   DISABLE

            Dim strScript As String = "<script>"

            If ViewState("vcPartNo") IsNot Nothing Then
                If ViewState("vcPartNo").ToString() <> "" Then
                    strScript += "window.opener.document.forms[0]." & ViewState("vcPartNo").ToString() & ".value = '" & Replace(PartNo, "&nbsp;", "") & "';"
                End If
            End If

            If ViewState("vcPartRevision") IsNot Nothing Then
                If ViewState("vcPartRevision").ToString() <> "" Then
                    strScript += "window.opener.document.forms[0]." & ViewState("vcPartRevision").ToString() & ".value = '" & Replace(PartRevision, "&nbsp;", "") & "';"
                End If
            End If

            If ViewState("vcPartDescr") IsNot Nothing Then
                If ViewState("vcPartDescr").ToString() <> "" Then
                    strScript += "window.opener.document.forms[0]." & ViewState("vcPartDescr").ToString() & ".value = '" & Replace(PartName, "&nbsp;", "") & "';"
                End If
            End If

            strScript += "self.close();</script>"

            Dim cstype As Type = Me.GetType()
            ClientScript.RegisterClientScriptBlock(cstype, "test", strScript)
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub ' SendDataBackToParentForm


    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            txtSearchPartNo.Text = ""
            txtSearchPartName.Text = ""
            txtSearchDrawingNo.Text = ""
            ddSearchActiveType.SelectedIndex = -1
            ''ddSearchDesignationType.SelectedIndex = -1

            'gvPartList.DataBind()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub


    Protected Sub gvPartList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPartList.SelectedIndexChanged

        Try
            ' The user has selected an item from the GridView.
            ' Send the data back to the parent form.
            Dim row As GridViewRow = gvPartList.SelectedRow
            Dim strPartNo As String = row.Cells(1).Text
            'Dim strPartRevision As String = row.Cells(2).Text
            Dim strPartName As String = row.Cells(2).Text

            'SendDataBackToParentForm(strPartNo, strPartRevision, strPartName)
            SendDataBackToParentForm(strPartNo, "", strPartName)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
