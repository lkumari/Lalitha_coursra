' ************************************************************************************************
'
' Name:		DrawingBPCSPartNo.aspx.vb

' Purpose:	This Code Behind to search BPCS Part Numbers for the DMS Drawing Detail. It is a popup called from DrawingDetail.aspx
'
' Date		    Author	    
' 09/22/2008    RCarlson
' 12/18/2013    LRey                       Replaced "BPCSPartNo" to "PartNo" wherever used. 
' ************************************************************************************************
Partial Class PE_PE_Drawings_DrawingBPCSPartNoLookUp
    Inherits System.Web.UI.Page
    Private Sub SendDataBackToParentForm(ByVal PartNo As String, ByVal PartRevision As String)

        Try
            ' If the control id's of the parent form are available for receiving data, continue.
            ' Otherwise, exit this procedure.
            If ViewState("PartNoValueControlID") Is Nothing Then
                Exit Sub
            End If

            If ViewState("PartRevisionValueControlID") Is Nothing Then
                Exit Sub
            End If

            ' Build client JavaScript code:
            '  1. populate textboxes on parent form
            '  2. re-validate the parent form
            Dim strScript As String = _
                "<script>window.opener.document.forms[0]." & ViewState("PartNoValueControlID").ToString() & ".value = '" & PartNo & "';" & _
                "window.opener.document.forms[0]." & ViewState("PartRevisionValueControlID").ToString() & ".value = '" & PartRevision & "';" & _
                "self.close();</script>"
            '"window.opener.Page_ClientValidate();" & _   DISABLE
            Dim cstype As Type = Me.GetType()
            ClientScript.RegisterClientScriptBlock(cstype, "test", strScript)
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub ' SendDataBackToParentForm

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            ViewState("SearchPartNo") = txtSearchPartNo.Text.Trim
            ViewState("SearchPartName") = txtSearchPartName.Text.Trim
            odsBPCSPartList.SelectParameters("PartNo").DefaultValue = ViewState("SearchPartNo")
            odsBPCSPartList.SelectParameters("PartName").DefaultValue = ViewState("SearchPartName")
            gvBPCSPartList.DataBind()
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            ViewState("SearchPartNo") = ""
            ViewState("SearchPartName") = ""
            txtSearchPartNo.Text = ""
            txtSearchPartName.Text = ""
            odsBPCSPartList.SelectParameters("PartNo").DefaultValue = ""
            odsBPCSPartList.SelectParameters("PartName").DefaultValue = ""
            gvBPCSPartList.DataBind()
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
                If Request.QueryString("PartNoValueControlID") IsNot Nothing Then
                    ViewState("PartNoValueControlID") = Request.QueryString("PartNoValueControlID").ToString()
                End If

                If Request.QueryString("PartRevisionValueControlID") IsNot Nothing Then
                    ViewState("PartRevisionValueControlID") = Request.QueryString("PartRevisionValueControlID").ToString()
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

    Protected Sub gvBPCSPartList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBPCSPartList.SelectedIndexChanged

        Try
            ' The user has selected an item from the GridView.
            ' Send the data back to the parent form.
            Dim row As GridViewRow = gvBPCSPartList.SelectedRow
            Dim strPartNo As String = row.Cells(1).Text
            Dim strPartRevision As String = row.Cells(2).Text

            SendDataBackToParentForm(strPartNo, strPartRevision)
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
