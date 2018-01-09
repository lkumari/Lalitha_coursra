' ************************************************************************************************
'
' Name:		        AR_Search_Shipping_History.vb
' Purpose:	        This code is used to search all shipping information from Future 3
' Called From:      Tree Navigation in SiteMap
'
' Date		Author	    
' 07/07/2010 Roderick Carlson	Created .Net application
' 02/27/2012 Roderick Carlson   Modified: Make Sales Total 2 decimals
' 10/11/2012 Roderick Carlson   Modified: Add RANNO and PONO 
' 01/08/2014 LRey               Disabled the GetCABBV and GetSoldTo. Not used in the new ERP.
' ************************************************************************************************

Partial Class AR_Search_Shipping_History
    Inherits System.Web.UI.Page
    Private htControls As New System.Collections.Hashtable
    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet           
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0
            ViewState("isAdmin") = False
            ViewState("SubscriptionID") = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataset(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                ViewState("CurrentTeamMemberID") = iTeamMemberID

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 55)

                If commonFunctions.CheckDataset(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                            ViewState("isAdmin") = True
                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isAdmin") = True
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
                End If            
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub EnableControls()

        Try

            gvShippingInfo.Visible = ViewState("isAdmin")
            tblSearch.Visible = ViewState("isAdmin")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            'bind existing data to drop down controls for selection criteria for search

            'bind existing data to drop down Customer control for selection criteria for search
            ds = commonFunctions.GetCABBV()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCABBV.DataSource = ds
                ddCABBV.DataTextField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()
                ddCABBV.DataValueField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()
                ddCABBV.DataBind()
                ddCABBV.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetPriceCode("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddPriceCode.DataSource = ds
                ddPriceCode.DataTextField = ds.Tables(0).Columns("ddPriceCodeName").ColumnName.ToString()
                ddPriceCode.DataValueField = ds.Tables(0).Columns("PriceCode").ColumnName
                ddPriceCode.DataBind()
                ddPriceCode.Items.Insert(0, "")
            End If


            ds = commonFunctions.GetSoldTo()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSoldTo.DataSource = ds
                ddSoldTo.DataTextField = ds.Tables(0).Columns("SoldTo").ColumnName
                ddSoldTo.DataValueField = ds.Tables(0).Columns("SoldTo").ColumnName
                ddSoldTo.DataBind()
                ddSoldTo.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        'DO NOT DELETE THIS

    End Sub
    Private Sub PrepareGridViewForExport(ByRef gv As Control)

        'Dim l As Literal = New Literal()
        'Dim i As Integer


        'For i = 0 To gv.Controls.Count

        '    If ((Nothing <> htControls(gv.Controls(i).GetType().Name)) Or (Nothing <> htControls(gv.Controls(i).GetType().BaseType.Name))) Then
        '        l.Text = GetControlPropertyValue(gv.Controls(i))

        '        gv.Controls.Remove(gv.Controls(i))

        '        gv.Controls.AddAt(i, l)

        '    End If

        '    If (gv.Controls(i).HasControls()) Then

        '        PrepareGridViewForExport(gv.Controls(i))

        '    End If

        'Next

    End Sub
    Private Function GetControlPropertyValue(ByVal control As Control) As String
        'Dim controlType As Type = control.[GetType]()
        'Dim strControlType As String = controlType.Name
        Dim strReturn As String = "Error"
        'Dim bReturn As Boolean

        'Dim ctrlProps As System.Reflection.PropertyInfo() = controlType.GetProperties()
        'Dim ExcelPropertyName As String = DirectCast(htControls(strControlType), String)

        'If ExcelPropertyName Is Nothing Then
        '    ExcelPropertyName = DirectCast(htControls(control.[GetType]().BaseType.Name), String)
        '    If ExcelPropertyName Is Nothing Then
        '        Return strReturn
        '    End If
        'End If

        'For Each ctrlProp As System.Reflection.PropertyInfo In ctrlProps

        '    If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(String) Then
        '        Try
        '            strReturn = DirectCast(ctrlProp.GetValue(control, Nothing), String)
        '            Exit Try
        '        Catch
        '            strReturn = ""
        '        End Try
        '    End If

        '    If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(Boolean) Then
        '        Try
        '            bReturn = CBool(ctrlProp.GetValue(control, Nothing))
        '            strReturn = IIf(bReturn, "True", "False")
        '            Exit Try
        '        Catch
        '            strReturn = "Error"
        '        End Try
        '    End If

        '    If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(ListItem) Then
        '        Try
        '            strReturn = DirectCast((ctrlProp.GetValue(control, Nothing)), ListItem).Text
        '            Exit Try
        '        Catch
        '            strReturn = ""
        '        End Try
        '    End If
        'Next
        Return strReturn
    End Function
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Dim attachment As String = "attachment; filename=Future3ShippingInfo.xls"

        Response.ClearContent()

        Response.AddHeader("content-disposition", attachment)

        Response.ContentType = "application/ms-excel"

        Dim sw As StringWriter = New StringWriter()

        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)

        'EnablePartialRendering = False

        'gvShippingInfo.RenderControl(htw)

        Dim tempDataGridView As New GridView
        tempDataGridView = gvShippingInfo
        tempDataGridView.PageSize = 5000
        tempDataGridView.DataBind()

        'tempDataGridView.AllowPaging = False
        'tempDataGridView.AllowSorting = False

        tempDataGridView.HeaderStyle.BackColor = Color.White 'System.Drawing.Color.LightGray
        tempDataGridView.HeaderStyle.ForeColor = Color.Black 'System.Drawing.Color.Black
        tempDataGridView.HeaderStyle.Font.Bold = True

        tempDataGridView.BottomPagerRow.Visible = False

        tempDataGridView.RenderControl(htw)

        Response.Write(sw.ToString())

        Response.End()

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Search Future 3 Shipping History"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Accounts Receivable Tracking </b> > <a href='AR_Event_List.aspx'>AR Event Search </a> > Search Future 3 Shipping History "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("ARExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                CheckRights()

                ''******
                '' Bind drop down lists
                ''******
                BindCriteria()

                ''******
                'get saved value of past search criteria or query string, query string takes precedence
                ''******

                If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                    ddUGNFacility.SelectedValue = HttpContext.Current.Request.QueryString("UGNFacility")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveFacilityHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveFacilityHistory").Value) <> "" Then
                            ddUGNFacility.SelectedValue = Request.Cookies("ARGroupModule_SaveFacilityHistory").Value
                        End If
                    End If
                End If

                txtStartShipDate.Text = Today.Date.AddDays(-1)
                If HttpContext.Current.Request.QueryString("StartShipDate") <> "" Then
                    txtStartShipDate.Text = HttpContext.Current.Request.QueryString("StartShipDate")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveStartShipDateHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveStartShipDateHistory").Value) <> "" Then
                            txtStartShipDate.Text = Request.Cookies("ARGroupModule_SaveStartShipDateHistory").Value
                        End If
                    End If
                End If

                If txtStartShipDate.Text.Trim.Length < 8 Then
                    txtStartShipDate.Text = Today.Date.AddDays(-1)
                End If

                If HttpContext.Current.Request.QueryString("EndShipDate") <> "" Then
                    txtEndShipDate.Text = HttpContext.Current.Request.QueryString("EndShipDate")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveEndShipDateHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveEndShipDateHistory").Value) <> "" Then
                            txtEndShipDate.Text = Request.Cookies("ARGroupModule_SaveEndShipDateHistory").Value
                        End If
                    End If
                End If

                If txtEndShipDate.Text.Trim.Length < 8 Then
                    txtEndShipDate.Text = ""
                End If

                If HttpContext.Current.Request.QueryString("SoldTo") <> "" Then
                    ddSoldTo.SelectedValue = HttpContext.Current.Request.QueryString("SoldTo")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveSoldToHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveSoldToHistory").Value) <> "" Then
                            ddSoldTo.SelectedValue = Request.Cookies("ARGroupModule_SaveSoldToHistory").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CABBV") <> "" Then
                    ddCABBV.SelectedValue = HttpContext.Current.Request.QueryString("CABBV")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveCABBVHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveCABBVHistory").Value) <> "" Then
                            ddCABBV.SelectedValue = Request.Cookies("ARGroupModule_SaveCABBVHistory").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("BPCSPartNo") <> "" Then
                    txtBPCSPartNo.Text = HttpContext.Current.Request.QueryString("BPCSPartNo")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveBPCSPartNoHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveBPCSPartNoHistory").Value) <> "" Then
                            txtBPCSPartNo.Text = Request.Cookies("ARGroupModule_SaveBPCSPartNoHistory").Value
                        End If
                    End If
                End If

                'If HttpContext.Current.Request.QueryString("PONO") <> "" Then
                '    txtPONO.Text = HttpContext.Current.Request.QueryString("PONO")
                'Else
                '    If Not Request.Cookies("ARGroupModule_SavePONOHistory") Is Nothing Then
                '        If Trim(Request.Cookies("ARGroupModule_SavePONOHistory").Value) <> "" Then
                '            txtPONO.Text = Request.Cookies("ARGroupModule_SavePONOHistory").Value
                '        End If
                '    End If
                'End If

                'If HttpContext.Current.Request.QueryString("UnitOfMeasure") <> "" Then
                '    txtUnitOfMeasure.Text = HttpContext.Current.Request.QueryString("UnitOfMeasure")
                'Else
                '    If Not Request.Cookies("ARGroupModule_SaveUnitOfMeasureHistory") Is Nothing Then
                '        If Trim(Request.Cookies("ARGroupModule_SaveUnitOfMeasureHistory").Value) <> "" Then
                '            txtUnitOfMeasure.Text = Request.Cookies("ARGroupModule_SaveUnitOfMeasureHistory").Value
                '        End If
                '    End If
                'End If

                'If HttpContext.Current.Request.QueryString("QuantityShipped") <> "" Then
                '    txtQuantityShipped.Text = HttpContext.Current.Request.QueryString("QuantityShipped")
                'Else
                '    If Not Request.Cookies("ARGroupModule_SaveQuantityShippedHistory") Is Nothing Then
                '        If Trim(Request.Cookies("ARGroupModule_SaveQuantityShippedHistory").Value) <> "" Then
                '            txtQuantityShipped.Text = Request.Cookies("ARGroupModule_SaveQuantityShippedHistory").Value
                '        End If
                '    End If
                'End If

                If HttpContext.Current.Request.QueryString("INVNO") <> "" Then
                    txtINVNo.Text = HttpContext.Current.Request.QueryString("INVNO")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveINVNOHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveINVNOHistory").Value) <> "" Then
                            txtINVNo.Text = Request.Cookies("ARGroupModule_SaveINVNOHistory").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("StartREQDate") <> "" Then
                    txtStartREQDate.Text = HttpContext.Current.Request.QueryString("StartREQDate")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveStartREQDateHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveStartREQDateHistory").Value) <> "" Then
                            txtStartREQDate.Text = Request.Cookies("ARGroupModule_SaveStartREQDateHistory").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("EndREQDate") <> "" Then
                    txtEndREQDate.Text = HttpContext.Current.Request.QueryString("EndREQDate")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveEndREQDateHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveEndREQDateHistory").Value) <> "" Then
                            txtEndREQDate.Text = Request.Cookies("ARGroupModule_SaveEndREQDateHistory").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PriceCode") <> "" Then
                    ddPriceCode.SelectedValue = HttpContext.Current.Request.QueryString("PriceCode")
                Else
                    If Not Request.Cookies("ARGroupModule_SavePriceCodeHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SavePriceCodeHistory").Value) <> "" Then
                            ddPriceCode.SelectedValue = Request.Cookies("ARGroupModule_SavePriceCodeHistory").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("RANNO") <> "" Then
                    txtRANNo.Text = HttpContext.Current.Request.QueryString("RANNO")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveRANNOHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveRANNOHistory").Value) <> "" Then
                            txtRANNo.Text = Request.Cookies("ARGroupModule_SaveRANNOHistory").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PONO") <> "" Then
                    txtPONO.Text = HttpContext.Current.Request.QueryString("PONO")
                Else
                    If Not Request.Cookies("ARGroupModule_SavePONOHistory") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SavePONOHistory").Value) <> "" Then
                            txtPONO.Text = Request.Cookies("ARGroupModule_SavePONOHistory").Value
                        End If
                    End If
                End If

                EnableControls()

            End If

            UpdateShippingHistoryTotal()

        Catch ex As Exception
            ARGroupModule.DeleteARShippingHistoryCookies()

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub UpdateShippingHistoryTotal()

        Try
            Dim ds As DataSet

            lblShippingQuantityTotal.Text = ""
            lblShippingSalesTotal.Text = ""

            If txtStartShipDate.Text.Trim.Length < 8 Then
                txtStartShipDate.Text = Today.Date.AddDays(-1)
            End If

            If txtEndShipDate.Text.Trim.Length < 8 Then
                txtEndShipDate.Text = ""
            End If

            ds = ARGroupModule.GetARShippingHistoryTotal(ddUGNFacility.SelectedValue, _
            ddCABBV.SelectedValue, ddSoldTo.SelectedValue, txtBPCSPartNo.Text.Trim, _
            ddPriceCode.SelectedValue, txtStartShipDate.Text.Trim, _
            txtEndShipDate.Text.Trim, txtINVNo.Text.Trim, _
            txtRANNo.Text.Trim, txtPONO.Text.Trim)

            If commonFunctions.CheckDataset(ds) = True Then
                If ds.Tables(0).Rows(0).Item("ShippingQuantityTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ShippingQuantityTotal") > 0 Then
                        lblShippingQuantityTotal.Text = Format(ds.Tables(0).Rows(0).Item("ShippingQuantityTotal"), "#,###,###")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("ShippingSalesTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ShippingSalesTotal") > 0 Then
                        lblShippingSalesTotal.Text = "$ " & Format(ds.Tables(0).Rows(0).Item("ShippingSalesTotal"), "#,###.00")
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            lblMessage.Text = ""

            ARGroupModule.DeleteARShippingHistoryCookies()
            Response.Redirect("AR_Search_Shipping_History.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            lblMessage.Text = ""

            Response.Cookies("ARGroupModule_SaveFacilityHistory").Value = ddUGNFacility.SelectedValue

            If txtStartShipDate.Text.Length < 8 Then
                txtStartShipDate.Text = ""
            End If

            Response.Cookies("ARGroupModule_SaveStartShipDateHistory").Value = txtStartShipDate.Text

            If txtEndShipDate.Text.Length < 8 Then
                txtEndShipDate.Text = ""
            End If

            Response.Cookies("ARGroupModule_SaveEndShipDateHistory").Value = txtEndShipDate.Text


            Dim iTempSoldTo As Integer = 0
            If ddSoldTo.SelectedIndex > 0 Then
                Response.Cookies("ARGroupModule_SaveSoldToHistory").Value = ddSoldTo.SelectedValue
                iTempSoldTo = ddSoldTo.SelectedValue
            Else
                Response.Cookies("ARGroupModule_SaveSoldToHistory").Value = 0
                Response.Cookies("ARGroupModule_SaveSoldToHistory").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("ARGroupModule_SaveCABBVHistory").Value = ddCABBV.SelectedValue
            Response.Cookies("ARGroupModule_SaveBPCSPartNoHistory").Value = txtBPCSPartNo.Text
           
            Response.Cookies("ARGroupModule_SaveINVNOHistory").Value = txtINVNo.Text

            If txtStartREQDate.Text.Length < 8 Then
                txtStartREQDate.Text = ""
            End If

            Response.Cookies("ARGroupModule_SaveStartREQDateHistory").Value = txtStartREQDate.Text

            If txtEndREQDate.Text.Length < 8 Then
                txtEndREQDate.Text = ""
            End If

            Response.Cookies("ARGroupModule_SaveEndREQDateHistory").Value = txtEndREQDate.Text
            Response.Cookies("ARGroupModule_SavePriceCodeHistory").Value = ddPriceCode.SelectedValue

            Response.Cookies("ARGroupModule_SaveRANNOHistory").Value = txtRANNo.Text
            Response.Cookies("ARGroupModule_SavePONOHistory").Value = txtPONO.Text

            gvShippingInfo.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

End Class
