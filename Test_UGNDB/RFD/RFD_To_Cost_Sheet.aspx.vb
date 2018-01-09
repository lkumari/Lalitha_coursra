' ************************************************************************************************
'
' Name:		RFD_To_ECI.aspx.vb

' Purpose:	This Code Behind to search for RFDs. This is called from the Costing Details page to pull RFD information to an existing Cost Sheet
'
' Date		Author	  Roderick Carlson 
' 11/23/2010 	Created
Partial Class RFD_To_Cost_Sheet
    Inherits System.Web.UI.Page

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''bind existing data to drop downs for selection criteria of search
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ds = RFDModule.GetRFDStatus(0, False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddOverallStatus.DataSource = ds
                ddOverallStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName
                ddOverallStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddOverallStatus.DataBind()
                ddOverallStatus.Items.Insert(0, "")
                ddOverallStatus.SelectedValue = 2
            End If

            'approver status
            ds = RFDModule.GetRFDStatus(0, True)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddApproverStatus.DataSource = ds
                ddApproverStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName
                ddApproverStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddApproverStatus.DataBind()
                ddApproverStatus.Items.Insert(0, "")
                ddApproverStatus.SelectedValue = 2
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
    Private Sub SendDataBackToParentForm(ByVal RFDNo As String, ByVal rbTempSelectionionType As String, ByVal RFDChildRowID As String)

        Try
            ' If the control id's of the parent form are available for receiving data, continue.
            ' Otherwise, exit this procedure.
            If ViewState("RFDNoControlID") Is Nothing Then
                Exit Sub
            End If

            ' Build client JavaScript code:
            '  1. populate textboxes on parent form
            '  2. re-validate the parent form

            Dim strScript As String = "<script>window.opener.document.forms[0]." & ViewState("RFDNoControlID").ToString() & ".value = '" & RFDNo & "';"

            If ViewState("RFDSelectionTypeControlID") IsNot Nothing Then
                strScript += "window.opener.document.forms[0]." & ViewState("RFDSelectionTypeControlID").ToString() & ".value = '" & rbTempSelectionionType & "';"
            End If

            If ViewState("RFDChildRowControlID") IsNot Nothing Then
                strScript += "window.opener.document.forms[0]." & ViewState("RFDChildRowControlID").ToString() & ".value = '" & RFDChildRowID & "';"
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
                If Request.QueryString("RFDNoControlID") IsNot Nothing Then
                    ViewState("RFDNoControlID") = Request.QueryString("RFDNoControlID").ToString()
                End If

                If Request.QueryString("RFDSelectionTypeControlID") IsNot Nothing Then
                    ViewState("RFDSelectionTypeControlID") = Request.QueryString("RFDSelectionTypeControlID").ToString()
                End If

                If Request.QueryString("RFDChildRowControlID") IsNot Nothing Then
                    ViewState("RFDChildRowControlID") = Request.QueryString("RFDChildRowControlID").ToString()
                End If

                'If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then
                '    ViewState("CostSheetID") = HttpContext.Current.Request.QueryString("CostSheetID")
                'End If

                ViewState("RFDNo") = ""
                ViewState("RFDDesc") = ""
                ViewState("PartName") = ""
                ViewState("CustomerPartNo") = ""
                ViewState("PartNo") = ""
                ViewState("DrawingNo") = ""
                ViewState("ApproverStatus") = 0
                ViewState("OverallStatus") = 0

                ''******
                '' Bind drop down lists
                ''******
                BindCriteria()

                ''******
                'get saved value of past search criteria or query string, query string takes precedence
                ''******

                If HttpContext.Current.Request.QueryString("RFDNo") <> "" Then
                    txtRFDNo.Text = HttpContext.Current.Request.QueryString("RFDNo")
                    ViewState("RFDNo") = HttpContext.Current.Request.QueryString("RFDNo")
                End If

                If HttpContext.Current.Request.QueryString("RFDDesc") <> "" Then
                    txtRFDDesc.Text = HttpContext.Current.Request.QueryString("RFDDesc")
                    ViewState("RFDDesc") = HttpContext.Current.Request.QueryString("RFDDesc")
                End If

                If HttpContext.Current.Request.QueryString("PartName") <> "" Then
                    txtPartName.Text = HttpContext.Current.Request.QueryString("PartName")
                    ViewState("PartName") = HttpContext.Current.Request.QueryString("PartName")
                End If

                If HttpContext.Current.Request.QueryString("CustomerPartNo") <> "" Then
                    txtCustomerPartNo.Text = HttpContext.Current.Request.QueryString("CustomerPartNo")
                    ViewState("CustomerPartNo") = HttpContext.Current.Request.QueryString("CustomerPartNo")
                End If

                If HttpContext.Current.Request.QueryString("PartNo") <> "" Then
                    txtPartNo.Text = HttpContext.Current.Request.QueryString("PartNo")
                    ViewState("PartNo") = HttpContext.Current.Request.QueryString("PartNo")
                End If

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    txtDrawingNo.Text = HttpContext.Current.Request.QueryString("DrawingNo")
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                End If

                If HttpContext.Current.Request.QueryString("ApproverStatusID") <> "" Then
                    ddApproverStatus.SelectedValue = HttpContext.Current.Request.QueryString("ApproverStatusID")
                    ViewState("ApproverStatusID") = HttpContext.Current.Request.QueryString("ApproverStatusID")
                End If

                If HttpContext.Current.Request.QueryString("OverallStatusID") <> "" Then
                    ddOverallStatus.SelectedValue = HttpContext.Current.Request.QueryString("OverallStatusID")
                    ViewState("OverallStatusID") = HttpContext.Current.Request.QueryString("OverallStatusID")
                End If

            Else

                ViewState("RFDNo") = txtRFDNo.Text
                ViewState("RFDDesc") = txtRFDDesc.Text
                ViewState("PartName") = txtRFDDesc.Text
                ViewState("CustomerPartNo") = txtPartName.Text
                ViewState("PartNo") = txtPartNo.Text
                ViewState("DrawingNo") = txtDrawingNo.Text

                If ddApproverStatus.SelectedIndex > 0 Then
                    ViewState("ApproverStatusID") = ddApproverStatus.SelectedValue
                Else
                    ViewState("ApproverStatusID") = 0
                End If

                If ddOverallStatus.SelectedIndex > 0 Then
                    ViewState("OverallStatusID") = ddOverallStatus.SelectedValue
                Else
                    ViewState("OverallStatusID") = 0
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

    Protected Sub gvRFDList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvRFDList.SelectedIndexChanged

        Try
            ' The user has selected an item from the GridView.
            ' Send the data back to the parent form.
            Dim row As GridViewRow = gvRFDList.SelectedRow
            Dim strRFDNo As String = row.Cells(2).Text
            Dim strRFDChildRowID As String = row.Cells(3).Text

            Dim rbTempSelectionionType As RadioButtonList
            rbTempSelectionionType = CType(gvRFDList.SelectedRow.FindControl("rbSelectionType"), RadioButtonList)

            If rbTempSelectionionType IsNot Nothing Then
                SendDataBackToParentForm(strRFDNo, rbTempSelectionionType.SelectedValue, strRFDChildRowID)
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
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            lblMessage.Text = ""

            ViewState("RFDNo") = ""
            ViewState("RFDDesc") = ""
            ViewState("PartName") = ""
            ViewState("CustomerPartNo") = ""
            ViewState("PartNo") = ""
            ViewState("DrawingNo") = ""
            ViewState("ApproverStatusID") = 0
            ViewState("OverallStatusID") = 0

            txtRFDNo.Text = ""
            txtRFDDesc.Text = ""
            txtPartName.Text = ""
            txtCustomerPartNo.Text = ""
            txtPartNo.Text = ""
            txtDrawingNo.Text = ""

            ddApproverStatus.SelectedIndex = -1
            ddOverallStatus.SelectedIndex = -1

            odsRFDList.SelectParameters("RFDNo").DefaultValue = ""
            odsRFDList.SelectParameters("RFDDesc").DefaultValue = ""
            odsRFDList.SelectParameters("PartName").DefaultValue = ""
            odsRFDList.SelectParameters("CustomerPartNo").DefaultValue = ""
            odsRFDList.SelectParameters("PartNo").DefaultValue = ""
            odsRFDList.SelectParameters("DrawingNo").DefaultValue = ""
            odsRFDList.SelectParameters("StatusID").DefaultValue = 0
            odsRFDList.SelectParameters("ApproverStatusID").DefaultValue = 0

            gvRFDList.DataBind()

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
            lblMessage.Text = ""

            ViewState("RFDNo") = txtRFDNo.Text
            ViewState("RFDDesc") = txtRFDDesc.Text
            ViewState("PartName") = txtRFDDesc.Text
            ViewState("CustomerPartNo") = txtPartName.Text
            ViewState("PartNo") = txtPartNo.Text
            ViewState("DrawingNo") = txtDrawingNo.Text

            If ddApproverStatus.SelectedIndex > 0 Then
                ViewState("ApproverStatusID") = ddApproverStatus.SelectedValue
            Else
                ViewState("ApproverStatusID") = 0
            End If

            If ddOverallStatus.SelectedIndex > 0 Then
                ViewState("OverallStatusID") = ddOverallStatus.SelectedValue
            Else
                ViewState("OverallStatusID") = 0
            End If

            odsRFDList.SelectParameters("RFDNo").DefaultValue = ""
            odsRFDList.SelectParameters("RFDDesc").DefaultValue = ""
            odsRFDList.SelectParameters("PartName").DefaultValue = ""
            odsRFDList.SelectParameters("CustomerPartNo").DefaultValue = ""
            odsRFDList.SelectParameters("PartNo").DefaultValue = ""
            odsRFDList.SelectParameters("DrawingNo").DefaultValue = ""
            odsRFDList.SelectParameters("StatusID").DefaultValue = 0
            odsRFDList.SelectParameters("ApproverStatusID").DefaultValue = 0

            gvRFDList.DataBind()

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
