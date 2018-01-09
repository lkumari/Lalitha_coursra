' ************************************************************************************************
'
' Name:		MaterialSpecLookUp.aspx.vb

' Purpose:	This Code Behind to search drawings for any module. It is a popup
'
' Date  		Author	    
' 08/26/2011    Roderick Carlson
' ************************************************************************************************
Partial Class MaterialSpecLookUp
    Inherits System.Web.UI.Page

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ''bind existing data to drop down PartFamily control for selection criteria for search
            ds = commonFunctions.GetSubFamily(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSubFamily.DataSource = ds
                ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddSubFamily.DataBind()
                ddSubFamily.Items.Insert(0, "")
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
                If Request.QueryString("MaterialSpecNoControlID") IsNot Nothing Then
                    ViewState("MaterialSpecNoControlID") = Request.QueryString("MaterialSpecNoControlID").ToString()
                End If

                BindCriteria()
            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text += ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            lblMessage.Text = ""

            txtSearchDrawingNo.Text = ""
            txtSearchMaterialAreaWeight.Text = ""
            txtSearchMaterialSpecDesc.Text = ""
            txtSearchMaterialSpecNo.Text = ""

            ddSubFamily.SelectedIndex = -1

        Catch ex As Exception

            'update error on web page
            lblMessage.Text += ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub SendDataBackToParentForm(ByVal MaterialSpecNo As String)

        Try
            ' If the control id's of the parent form are available for receiving data, continue.
            ' Otherwise, exit this procedure.
            If ViewState("MaterialSpecNoControlID") Is Nothing Then
                Exit Sub
            End If

            ' Build client JavaScript code:
            '  1. populate textboxes on parent form
            '  2. re-validate the parent form
            Dim strScript As String = _
                "<script>window.opener.document.forms[0]." & ViewState("MaterialSpecNoControlID").ToString() & ".value = '" & MaterialSpecNo & "';" & _
                "self.close();</script>"
            '"window.opener.Page_ClientValidate();" & _   DISABLE
            Dim cstype As Type = Me.GetType()
            ClientScript.RegisterClientScriptBlock(cstype, "test", strScript)
        Catch ex As Exception
            'update error on web page
            lblMessage.Text += ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub ' SendDataBackToParentForm

    Protected Sub gvDrawingMaterialSpec_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDrawingMaterialSpec.SelectedIndexChanged

        Try
            ' The user has selected an item from the GridView.
            ' Send the data back to the parent form.
            Dim row As GridViewRow = gvDrawingMaterialSpec.SelectedRow
            Dim strMaterialSpecNo As String = row.Cells(1).Text

            SendDataBackToParentForm(strMaterialSpecNo)
        Catch ex As Exception
            'update error on web page
            lblMessage.Text += ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

End Class
