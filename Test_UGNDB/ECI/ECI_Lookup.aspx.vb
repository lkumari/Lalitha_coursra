' ************************************************************************************************
'
' Name:		ECI_Lookup.aspx.vb

' Purpose:	This Code Behind to search for ECIs.  
'
' Date		Author	  Roderick Carlson 
' 04/28/2011 	Created
' 05/10/2011    Modified - Prevent Voided ECIs or Obsolete Drawings from being previewed
Partial Class ECI_Lookup
    Inherits System.Web.UI.Page

    Protected Function SetECIHyperlink(ByVal ECINo As String, ByVal StatusID As String) As String

        Dim strReturnValue As String = ""

        Try
            If ECINo <> "" And StatusID <> "4" Then
                strReturnValue = "~/ECI/ECI_Preview.aspx?ECINo=" & ECINo
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetECIHyperlink = strReturnValue

    End Function

    Protected Function SetECIClickable(ByVal ECINo As String, ByVal StatusID As String) As String

        Dim strReturnValue As String = "False"

        Try
            If ECINo <> "" And StatusID <> "4" Then
                strReturnValue = "True"
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetECIClickable = strReturnValue

    End Function
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ds = ECIModule.GetECIStatus(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchStatus.DataSource = ds
                ddSearchStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddSearchStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddSearchStatus.DataBind()
                ddSearchStatus.Items.Insert(0, "")
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

    Private Sub SendDataBackToParentForm(ByVal ECINo As String, ByVal DrawingNo As String)

        Try
            ' If the control id's of the parent form are available for receiving data, continue.
            ' Otherwise, exit this procedure.
            If ViewState("ECINoControlID") Is Nothing Then
                Exit Sub
            End If

            ' Build client JavaScript code:
            '  1. populate textboxes on parent form
            '  2. re-validate the parent form

            Dim strScript As String = "<script>window.opener.document.forms[0]." & ViewState("ECINoControlID").ToString() & ".value = '" & ECINo & "';"

            If ViewState("DrawingNoControlID") IsNot Nothing Then
                strScript += "window.opener.document.forms[0]." & ViewState("DrawingNoControlID").ToString() & ".value = '" & DrawingNo & "';"
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
                BindCriteria()

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
                If Request.QueryString("ECINoControlID") IsNot Nothing Then
                    ViewState("ECINoControlID") = Request.QueryString("ECINoControlID").ToString()
                End If

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                End If


                ViewState("ECINo") = ""
                ViewState("ECIType") = ""
                ViewState("StatusID") = 0
                ViewState("ECIDesc") = ""
                ViewState("RFDNo") = ""
                ViewState("CostSheetID") = ""
                ViewState("DrawingNo") = ""
                ViewState("PartNo") = ""
                ViewState("CustomerPartNo") = ""
                ViewState("DesignLevel") = ""
                ViewState("PartName") = ""

                ''******
                'get saved value of past search criteria or query string, query string takes precedence
                ''******

                If HttpContext.Current.Request.QueryString("ECINo") <> "" Then
                    txtSearchECINo.Text = HttpContext.Current.Request.QueryString("ECINo")
                    ViewState("ECINo") = HttpContext.Current.Request.QueryString("ECINo")
                End If

                If HttpContext.Current.Request.QueryString("ECIType") <> "" Then
                    ddSearchECIType.SelectedValue = HttpContext.Current.Request.QueryString("ECIType")
                    ViewState("ECIType") = HttpContext.Current.Request.QueryString("ECIType")
                End If

                If HttpContext.Current.Request.QueryString("StatusID") <> "" Then
                    ddSearchStatus.SelectedValue = HttpContext.Current.Request.QueryString("StatusID")
                    ViewState("StatusID") = HttpContext.Current.Request.QueryString("StatusID")
                End If

                If HttpContext.Current.Request.QueryString("ECIDesc") <> "" Then
                    txtSearchECIDesc.Text = HttpContext.Current.Request.QueryString("ECIDesc")
                    ViewState("ECIDesc") = HttpContext.Current.Request.QueryString("ECIDesc")
                End If

                If HttpContext.Current.Request.QueryString("RFDNo") <> "" Then
                    txtSearchRFDNo.Text = HttpContext.Current.Request.QueryString("RFDNo")
                    ViewState("RFDNo") = HttpContext.Current.Request.QueryString("RFDNo")
                End If

                If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then
                    txtSearchCostSheetID.Text = HttpContext.Current.Request.QueryString("CostSheetID")
                    ViewState("CostSheetID") = HttpContext.Current.Request.QueryString("CostSheetID")
                End If

                If HttpContext.Current.Request.QueryString("ECINo") <> "" Then
                    txtSearchDrawingNo.Text = HttpContext.Current.Request.QueryString("DrawingNo")
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                End If

                If HttpContext.Current.Request.QueryString("PartNo") <> "" Then
                    txtSearchPartNo.Text = HttpContext.Current.Request.QueryString("PartNo")
                    ViewState("PartNo") = HttpContext.Current.Request.QueryString("PartNo")
                End If

                If HttpContext.Current.Request.QueryString("CustomerPartNo") <> "" Then
                    txtSearchCustomerPartNo.Text = HttpContext.Current.Request.QueryString("CustomerPartNo")
                    ViewState("CustomerPartNo") = HttpContext.Current.Request.QueryString("CustomerPartNo")
                End If

                If HttpContext.Current.Request.QueryString("DesignLevel") <> "" Then
                    txtSearchDesignLevel.Text = HttpContext.Current.Request.QueryString("DesignLevel")
                    ViewState("DesignLevel") = HttpContext.Current.Request.QueryString("DesignLevel")
                End If

                If HttpContext.Current.Request.QueryString("PartName") <> "" Then
                    txtSearchPartName.Text = HttpContext.Current.Request.QueryString("PartName")
                    ViewState("PartName") = HttpContext.Current.Request.QueryString("PartName")
                End If
            Else

                ViewState("ECINo") = txtSearchECINo.Text.Trim

                If ddSearchECIType.SelectedIndex > 0 Then
                    ViewState("ECIType") = ddSearchECIType.SelectedValue
                Else
                    ViewState("ECIType") = ""
                End If

                If ddSearchStatus.SelectedIndex > 0 Then
                    ViewState("StatusID") = ddSearchStatus.SelectedValue
                Else
                    ViewState("StatusID") = 0
                End If

                ViewState("ECIDesc") = txtSearchECIDesc.Text.Trim

                ViewState("RFDNo") = txtSearchRFDNo.Text.Trim

                ViewState("CostSheetID") = txtSearchCostSheetID.Text.Trim

                ViewState("DrawingNo") = txtSearchDrawingNo.Text.Trim

                ViewState("PartNo") = txtSearchPartNo.Text.Trim

                ViewState("CustomerPartNo") = txtSearchCustomerPartNo.Text.Trim

                ViewState("DesignLevel") = txtSearchDesignLevel.Text.Trim

                ViewState("PartName") = txtSearchPartName.Text.Trim
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

    Protected Sub gvECIList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvECIList.SelectedIndexChanged

        Try
            ' The user has selected an item from the GridView.
            ' Send the data back to the parent form.
            Dim row As GridViewRow = gvECIList.SelectedRow
            Dim strECINo As String = row.Cells(1).Text
            Dim strNewDrawingNo As String = row.Cells(6).Text

            SendDataBackToParentForm(strECINo, strNewDrawingNo)
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
            ViewState("ECINo") = ""
            ViewState("ECIType") = ""
            ViewState("StatusID") = 0
            ViewState("ECIDesc") = ""
            ViewState("RFDNo") = ""
            ViewState("CostSheetID") = ""
            ViewState("DrawingNo") = ""
            ViewState("PartNo") = ""
            ViewState("CustomerPartNo") = ""
            ViewState("DesignLevel") = ""
            ViewState("PartName") = ""

            txtSearchECINo.Text = ""
            ddSearchECIType.SelectedIndex = -1
            ddSearchStatus.SelectedIndex = -1
            txtSearchECIDesc.Text = ""
            txtSearchRFDNo.Text = ""
            txtSearchCostSheetID.Text = ""
            txtSearchDrawingNo.Text = ""
            txtSearchPartNo.Text = ""
            txtSearchCustomerPartNo.Text = ""
            txtSearchDesignLevel.Text = ""
            txtSearchPartName.Text = ""

            odsECIList.SelectParameters("ECINo").DefaultValue = ""
            odsECIList.SelectParameters("ECIType").DefaultValue = ""
            odsECIList.SelectParameters("StatusID").DefaultValue = 0
            odsECIList.SelectParameters("ECIDesc").DefaultValue = ""
            odsECIList.SelectParameters("RFDNo").DefaultValue = ""
            odsECIList.SelectParameters("CostSheetID").DefaultValue = ""
            odsECIList.SelectParameters("DrawingNo").DefaultValue = ""
            odsECIList.SelectParameters("PartNo").DefaultValue = ""
            odsECIList.SelectParameters("CustomerPartNo").DefaultValue = ""
            odsECIList.SelectParameters("DesignLevel").DefaultValue = ""
            odsECIList.SelectParameters("PartName").DefaultValue = ""

            gvECIList.DataBind()

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
            ViewState("ECINo") = txtSearchECINo.Text.Trim

            If ddSearchECIType.SelectedIndex > 0 Then
                ViewState("ECIType") = ddSearchECIType.SelectedValue
            Else
                ViewState("ECIType") = ""
            End If

            If ddSearchStatus.SelectedIndex > 0 Then
                ViewState("StatusID") = ddSearchStatus.SelectedIndex = -1
            Else
                ViewState("StatusID") = 0
            End If

            ViewState("ECIDesc") = txtSearchECIDesc.Text.Trim
            ViewState("RFDNo") = txtSearchRFDNo.Text.Trim
            ViewState("CostSheetID") = txtSearchCostSheetID.Text.Trim
            ViewState("DrawingNo") = txtSearchDrawingNo.Text.Trim
            ViewState("PartNo") = txtSearchPartNo.Text.Trim
            ViewState("CustomerPartNo") = txtSearchCustomerPartNo.Text.Trim
            ViewState("DesignLevek") = txtSearchDesignLevel.Text.Trim
            ViewState("PartName") = txtSearchPartName.Text.Trim

            odsECIList.SelectParameters("ECINo").DefaultValue = ViewState("ECINo")
            odsECIList.SelectParameters("ECIType").DefaultValue = ViewState("ECIType")
            odsECIList.SelectParameters("StatusID").DefaultValue = ViewState("StatusID")
            odsECIList.SelectParameters("ECIDesc").DefaultValue = ViewState("ECIDesc")
            odsECIList.SelectParameters("RFDNo").DefaultValue = ViewState("RFDNo")
            odsECIList.SelectParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
            odsECIList.SelectParameters("DrawingNo").DefaultValue = ViewState("DrawingNo")
            odsECIList.SelectParameters("PartNo").DefaultValue = ViewState("PartNo")
            odsECIList.SelectParameters("CustomerPartNo").DefaultValue = ViewState("CustomerPartNo")
            odsECIList.SelectParameters("DesignLevel").DefaultValue = ViewState("DesignLevel")
            odsECIList.SelectParameters("PartName").DefaultValue = ViewState("PartName")

            gvECIList.DataBind()
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
