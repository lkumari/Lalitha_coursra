' ************************************************************************************************
'
' Name:		CustomerPartNoLookUp.aspx.vb

' Purpose:	This Code Behind to search for Customer Part Numbers in the Future 3 PXREF, cross reference
'		It can be used by any UGN DB Applicaiton and should replace all isolated versions used by various modules
'
' Date		Author	  Roderick Carlson 
' 05/04/2009 	Created
' 09/17/2010    Modified - Allow Customer Part Name to be returned
' 01/08/2014    LRey                Disabled GetCABBV. CABBV is not used in new ERP.

Partial Class CustomerPartNoLookUp
    Inherits System.Web.UI.Page
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ''bind existing data to drop down Customer control for selection criteria for search
            '(LREY) 01/08/2014
            ds = commonFunctions.GetCABBV()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCABBV.DataSource = ds
                ddCABBV.DataTextField = ds.Tables(0).Columns("CustomerNameCombo").ColumnName.ToString()
                ddCABBV.DataValueField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()
                ddCABBV.DataBind()
                ddCABBV.Items.Insert(0, "")
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
    Private Sub SendDataBackToParentForm(ByVal CustomerPartNo As String, ByVal BarCodePartNo As String, _
        ByVal BPCSPartNo As String, ByVal CustomerPartName As String, ByVal CABBV As String)

        Try
            ' If the control id's of the parent form are available for receiving data, continue.
            ' Otherwise, exit this procedure.
            If ViewState("CustomerPartNoValueControlID") Is Nothing Then
                Exit Sub
            End If

            ' Build client JavaScript code:
            '  1. populate textboxes on parent form
            '  2. re-validate the parent form
            'Dim strScript As String = _
            '    "<script>window.opener.document.forms[0]." & ViewState("CustomerPartNoValueControlID").ToString() & ".value = '" & CustomerPartNo & "';" & _
            '    "window.opener.document.forms[0]." & ViewState("CABBVValueControlID").ToString() & ".value = '" & CABBV & "';" & _
            '    "self.close();</script>"

            Dim strScript As String = "<script>window.opener.document.forms[0]." & ViewState("CustomerPartNoValueControlID").ToString() & ".value = '" & CustomerPartNo & "';"

            If ViewState("BarCodePartNoValueControlID") IsNot Nothing Then
                strScript += "window.opener.document.forms[0]." & ViewState("BarCodePartNoValueControlID").ToString() & ".value = '" & BarCodePartNo & "';"
            End If

            If ViewState("BPCSPartNoValueControlID") IsNot Nothing Then
                strScript += "window.opener.document.forms[0]." & ViewState("BPCSPartNoValueControlID").ToString() & ".value = '" & BPCSPartNo & "';"
            End If

            If ViewState("CustomerPartNameValueControlID") IsNot Nothing Then
                strScript += "window.opener.document.forms[0]." & ViewState("CustomerPartNameValueControlID").ToString() & ".value = '" & CustomerPartName & "';"
            End If

            If ViewState("CABBVValueControlID") IsNot Nothing Then
                strScript += "window.opener.document.forms[0]." & ViewState("CABBVValueControlID").ToString() & ".value = '" & CABBV & "';"
            End If

            strScript += "self.close();</script>"

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
                If Request.QueryString("CustomerPartNoValueControlID") IsNot Nothing Then
                    ViewState("CustomerPartNoValueControlID") = Request.QueryString("CustomerPartNoValueControlID").ToString()
                End If

                'If Request.QueryString("BarCodePartNoValueControlID") IsNot Nothing Then
                '    ViewState("BarCodePartNoValueControlID") = Request.QueryString("BarCodePartNoValueControlID").ToString()
                'End If

                If Request.QueryString("BPCSPartNoValueControlID") IsNot Nothing Then
                    ViewState("BPCSPartNoValueControlID") = Request.QueryString("BPCSPartNoValueControlID").ToString()
                End If

                If Request.QueryString("CustomerPartNameValueControlID") IsNot Nothing Then
                    ViewState("CustomerPartNameValueControlID") = Request.QueryString("CustomerPartNameValueControlID").ToString()
                End If

                If Request.QueryString("CABBVValueControlID") IsNot Nothing Then
                    ViewState("CABBVValueControlID") = Request.QueryString("CABBVValueControlID").ToString()
                End If

                ViewState("BPCSPartNo") = ""
                ViewState("CustomerPartNo") = ""
                ViewState("CustomerPartName") = ""
                ViewState("CABBV") = ""
                ViewState("BarCodePartNo") = ""

                ''******
                '' Bind drop down lists
                ''******
                BindCriteria()

                ''******
                'get saved value of past search criteria or query string, query string takes precedence
                ''******

                If HttpContext.Current.Request.QueryString("BPCSPartNo") <> "" Then
                    txtBPCSPartNo.Text = HttpContext.Current.Request.QueryString("BPCSPartNo")
                    ViewState("BPCSPartNo") = HttpContext.Current.Request.QueryString("BPCSPartNo")
                End If

                If HttpContext.Current.Request.QueryString("CustomerPartNo") <> "" Then
                    txtCustomerPartNo.Text = HttpContext.Current.Request.QueryString("CustomerPartNo")
                    ViewState("CustomerPartNo") = HttpContext.Current.Request.QueryString("CustomerPartNo")
                End If

                If Request.QueryString("CustomerPartName") IsNot Nothing Then
                    txtCustomerPartName.Text = Server.UrlDecode(Request.QueryString("CustomerPartName"))
                End If

                If HttpContext.Current.Request.QueryString("CABBV") <> "" Then
                    ddCABBV.SelectedValue = HttpContext.Current.Request.QueryString("CABBV")
                    ViewState("CABBV") = HttpContext.Current.Request.QueryString("CABBV")
                End If

                'If Request.QueryString("barCodePartNo") IsNot Nothing Then
                '    txtBarCodePartNo.Text = Server.UrlDecode(Request.QueryString("barCodePartNo"))
                'End If

            Else

                ViewState("CustomerPartNo") = txtCustomerPartNo.Text
                ViewState("CustomerPartName") = txtCustomerPartName.Text

                ViewState("BPCSPartNo") = txtBPCSPartNo.Text
                'ViewState("BarCodePartNo") = txtBarCodePartNo.Text

                If ddCABBV.SelectedIndex > 0 Then
                    ViewState("CABBV") = ddCABBV.SelectedValue
                Else
                    ViewState("CABBV") = ""
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
    Protected Sub gvCustomerPartNoList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCustomerPartNoList.SelectedIndexChanged

        Try
            ' The user has selected an item from the GridView.
            ' Send the data back to the parent form.
            Dim row As GridViewRow = gvCustomerPartNoList.SelectedRow
            Dim strCustomerPartNo As String = row.Cells(1).Text
            'Dim strBarCodePartNo As String = row.Cells(2).Text
            Dim strBPCSPartNo As String = row.Cells(2).Text
            Dim strCustomerPartName As String = row.Cells(3).Text
            Dim strCABBV As String = row.Cells(4).Text

            'SendDataBackToParentForm(strCustomerPartNo, strBarCodePartNo, strBPCSPartNo, strCustomerPartName, strCABBV)
            SendDataBackToParentForm(strCustomerPartNo, "", strBPCSPartNo, strCustomerPartName, strCABBV)
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
            ViewState("BPCSPartNo") = ""
            ViewState("CABBV") = ""
            ViewState("CustomerPartNo") = ""
            ViewState("CustomerPartName") = ""
            ViewState("BarCodePartNo") = ""

            txtBPCSPartNo.Text = ""
            ddCABBV.SelectedIndex = -1
            txtCustomerPartNo.Text = ""
            txtCustomerPartName.Text = ""
            'txtBarCodePartNo.Text = ""

            odsCustomerPartNoList.SelectParameters("BPCSPartNo").DefaultValue = ""
            odsCustomerPartNoList.SelectParameters("CABBV").DefaultValue = ""
            odsCustomerPartNoList.SelectParameters("CustomerPartNo").DefaultValue = ""
            odsCustomerPartNoList.SelectParameters("CustomerPartName").DefaultValue = ""
            'odsCustomerPartNoList.SelectParameters("BarCodePartNo").DefaultValue = ""

            gvCustomerPartNoList.DataBind()
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            ViewState("BPCSPartNo") = txtBPCSPartNo.Text
            ViewState("CABBV") = ddCABBV.SelectedValue
            ViewState("CustomerPartNo") = txtCustomerPartNo.Text
            ViewState("CustomerPartName") = txtCustomerPartName.Text
            ViewState("BarCodePartNo") = "" ''txtBarCodePartNo.Text

            odsCustomerPartNoList.SelectParameters("BPCSPartNo").DefaultValue = ""
            odsCustomerPartNoList.SelectParameters("CABBV").DefaultValue = ""
            odsCustomerPartNoList.SelectParameters("CustomerPartNo").DefaultValue = ""
            odsCustomerPartNoList.SelectParameters("CustomerPartName").DefaultValue = ""
            'odsCustomerPartNoList.SelectParameters("BarCodePartNo").DefaultValue = ""

            gvCustomerPartNoList.DataBind()
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
