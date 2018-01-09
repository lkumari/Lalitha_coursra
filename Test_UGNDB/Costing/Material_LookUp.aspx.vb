' ************************************************************************************************
' Name:		Costing_Material_LookUp.aspx
' Purpose:	This Code Behind allow users to search for materials in a popup and then select an item to populate in a parent page
'
' Date		    Author	    
' 06/18/2009    Roderick Carlson
' 01/18/2010    Roderick Carlson     added freight cost
' 08/26/2010    Roderick Carlson     added isActiveBPCSOnly Parameter to GetUGNDBVendor
' 01/06/2014    LRey                 Replaced "BPCSPart " to "Part" wherever used.
' ************************************************************************************************

Partial Class Costing_Material_LookUp
    Inherits System.Web.UI.Page

    Private Sub SendDataBackToParentForm(ByVal MaterialID As String, ByVal QuoteCost As String, ByVal FreightCost As String)

        Try
            ' If the control id's of the parent form are available for receiving data, continue.
            ' Otherwise, exit this procedure.
            If ViewState("ddMaterialControlID") Is Nothing Then
                Exit Sub
            End If

            'If ViewState("txtQuoteCostControlID") Is Nothing Then
            '    Exit Sub
            'End If

            ' Build client JavaScript code:
            '  1. populate textboxes on parent form
            '  2. re-validate the parent form
            Dim strScript As String = "<script>window.opener.document.forms[0]." & ViewState("ddMaterialControlID").ToString() & ".value = '" & MaterialID & "';"

            If ViewState("txtQuoteCostControlID") IsNot Nothing And ViewState("txtQuoteCostControlID") <> "" Then
                strScript += "window.opener.document.forms[0]." & ViewState("txtQuoteCostControlID").ToString() & ".value = '" & QuoteCost & "';"
            End If

            If ViewState("txtFreightCostControlID") IsNot Nothing And ViewState("txtFreightCostControlID") <> "" Then
                strScript += "window.opener.document.forms[0]." & ViewState("txtFreightCostControlID").ToString() & ".value = '" & FreightCost & "';"
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
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            'bind existing data to drop down PurchasedGood 
            ds = commonFunctions.GetPurchasedGood("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddSearchPurchasedGoodValue.DataSource = ds
                ddSearchPurchasedGoodValue.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
                ddSearchPurchasedGoodValue.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddSearchPurchasedGoodValue.DataBind()
                ddSearchPurchasedGoodValue.Items.Insert(0, "")
            End If

            'bind existing data to drop down Vendor 
            ds = commonFunctions.GetUGNDBVendor(0, "", "", False)
            If commonFunctions.CheckDataset(ds) = True Then
                ddSearchVendorValue.DataSource = ds
                ddSearchVendorValue.DataTextField = ds.Tables(0).Columns("ddVendorName").ColumnName.ToString()
                ddSearchVendorValue.DataValueField = ds.Tables(0).Columns("UGNDBVendorID").ColumnName
                ddSearchVendorValue.DataBind()
                ddSearchVendorValue.Items.Insert(0, "")
            End If

            ''bind existing data to drop down UGN Facility Code
            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchUGNFacilityCode.DataSource = ds
                ddSearchUGNFacilityCode.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddSearchUGNFacilityCode.DataValueField = ds.Tables(0).Columns("UGNFacilityCode").ColumnName
                ddSearchUGNFacilityCode.DataBind()
                ddSearchUGNFacilityCode.Items.Insert(0, "")

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
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try

            Response.Redirect("Material_LookUp.aspx?ddMaterialControlID=" & ViewState("ddMaterialControlID") & "&txtQuoteCostControlID=" & ViewState("txtQuoteCostControlID") & "&txtFreightCostControlID=" & ViewState("txtFreightCostControlID"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try

            ViewState("MaterialID") = txtSearchMaterialIDValue.Text.Trim

            ViewState("PartName") = txtSearchPartNameValue.Text.Trim

            ViewState("DrawingNo") = txtSearchDrawingNoValue.Text.Trim

            ViewState("PartNo") = txtSearchPartNoValue.Text.Trim

            If ddSearchVendorValue.SelectedIndex > 0 Then
                ViewState("UGNDBVendorID") = ddSearchVendorValue.SelectedValue
            Else
                ViewState("UGNDBVendorID") = 0
            End If

            If ddSearchPurchasedGoodValue.SelectedIndex > 0 Then
                ViewState("PurchasedGoodID") = ddSearchPurchasedGoodValue.SelectedValue
            Else
                ViewState("PurchasedGoodID") = 0
            End If

            If ddSearchUGNFacilityCode.SelectedIndex > 0 Then
                ViewState("UGNFacility") = ddSearchUGNFacilityCode.SelectedValue
            Else
                ViewState("UGNFacility") = 0
            End If

            ViewState("OldMaterialGroup") = txtSearchOldMaterialGroupValue.Text.Trim

            ViewState("isCoating") = 0
            ViewState("filterCoating") = 0

            If ddSearchCoating.SelectedIndex > 0 Then
                If ddSearchCoating.SelectedValue = "Only" Then
                    ViewState("isCoating") = 1
                    ViewState("filterCoating") = 1
                End If

                If ddSearchCoating.SelectedValue = "None" Then
                    ViewState("isCoating") = 0
                    ViewState("filterCoating") = 1
                End If
            End If

            ViewState("isPackaging") = 0
            ViewState("filterPackaging") = 0

            If ddSearchPackaging.SelectedIndex > 0 Then
                If ddSearchPackaging.SelectedValue = "Only" Then
                    ViewState("isPackaging") = 1
                    ViewState("filterPackaging") = 1
                End If

                If ddSearchPackaging.SelectedValue = "None" Then
                    ViewState("isPackaging") = 0
                    ViewState("filterPackaging") = 1
                End If
            End If

            ViewState("Obsolete") = 0
            ViewState("filterObsolete") = 0

            If ddSearchObsolete.SelectedIndex > 0 Then
                If ddSearchObsolete.SelectedValue = "Only" Then
                    ViewState("Obsolete") = 1
                    ViewState("filterObsolete") = 1
                End If

                If ddSearchObsolete.SelectedValue = "None" Then
                    ViewState("Obsolete") = 0
                    ViewState("filterObsolete") = 1
                End If
            End If

            Response.Redirect("Material_LookUp.aspx?ddMaterialControlID=" & ViewState("ddMaterialControlID") & _
            "&txtQuoteCostControlID=" & ViewState("txtQuoteCostControlID") & _
            "&txtFreightCostControlID=" & ViewState("txtFreightCostControlID") & _
            "&MaterialID=" & Server.UrlEncode(ViewState("MaterialID")) & _
            "&PartName=" & Server.UrlEncode(ViewState("PartName")) & _
            "&DrawingNo=" & Server.UrlEncode(ViewState("DrawingNo")) & _
            "&PartNo=" & Server.UrlEncode(ViewState("PartNo")) & _
            "&UGNDBVendorID=" & ViewState("UGNDBVendorID") & _
            "&PurchasedGoodID=" & ViewState("PurchasedGoodID") & _
            "&UGNFacilityCode=" & ViewState("UGNFacilityCode") & _
            "&OldMaterialGroup=" & ViewState("OldMaterialGroup") & _
            "&isCoating= " & ViewState("isCoating") & _
            "&filterCoating= " & ViewState("filterCoating") & _
            "&isPackaging= " & ViewState("isPackaging") & _
            "&filterPackaging= " & ViewState("filterPackaging") & _
            "&Obsolete= " & ViewState("Obsolete") & _
            "&filterObsolete= " & ViewState("filterObsolete"), False)

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
                If Request.QueryString("ddMaterialControlID") IsNot Nothing Then
                    If Request.QueryString("ddMaterialControlID") <> "" Then
                        ViewState("ddMaterialControlID") = Request.QueryString("ddMaterialControlID").ToString()
                    End If
                End If

                If Request.QueryString("txtQuoteCostControlID") IsNot Nothing Then
                    If Request.QueryString("txtQuoteCostControlID") <> "" Then
                        ViewState("txtQuoteCostControlID") = Request.QueryString("txtQuoteCostControlID").ToString()
                    End If
                End If

                If Request.QueryString("txtFreightCostControlID") IsNot Nothing Then
                    If Request.QueryString("txtFreightCostControlID") <> "" Then
                        ViewState("txtFreightCostControlID") = Request.QueryString("txtFreightCostControlID").ToString()
                    End If
                End If

                ViewState("MaterialID") = ""
                ViewState("PartName") = ""
                ViewState("DrawingNo") = ""
                ViewState("PartNo") = ""

                ViewState("UGNDBVendorID") = 0
                ViewState("PurchasedGoodID") = 0

                ViewState("OldMaterialGroup") = ""

                ViewState("isPackaging") = 0
                ViewState("filterPackaging") = 0
                ViewState("isCoating") = 0
                ViewState("filterCoating") = 0
                ViewState("Obsolete") = 0
                ViewState("filterObsolete") = 0

                BindCriteria()

                ' ''******
                ''get saved value of past search criteria or query string
                ' ''******
                If HttpContext.Current.Request.QueryString("MaterialID") <> "" Then
                    txtSearchMaterialIDValue.Text = HttpContext.Current.Request.QueryString("MaterialID")
                    ViewState("MaterialID") = HttpContext.Current.Request.QueryString("MaterialID")
                End If

                If HttpContext.Current.Request.QueryString("PartName") <> "" Then
                    txtSearchPartNameValue.Text = HttpContext.Current.Request.QueryString("PartName")
                    ViewState("PartName") = HttpContext.Current.Request.QueryString("PartName")
                End If

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    txtSearchDrawingNoValue.Text = HttpContext.Current.Request.QueryString("DrawingNo")
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                End If

                If HttpContext.Current.Request.QueryString("PartNo") <> "" Then
                    txtSearchPartNoValue.Text = HttpContext.Current.Request.QueryString("PartNo")
                    ViewState("PartNo") = HttpContext.Current.Request.QueryString("PartNo")
                End If

                If HttpContext.Current.Request.QueryString("UGNDBVendorID") <> "" Then
                    If HttpContext.Current.Request.QueryString("UGNDBVendorID") > 0 Then
                        ddSearchVendorValue.SelectedValue = HttpContext.Current.Request.QueryString("UGNDBVendorID")
                        ViewState("UGNDBVendorID") = HttpContext.Current.Request.QueryString("UGNDBVendorID")
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PurchasedGoodID") <> "" Then
                    If HttpContext.Current.Request.QueryString("PurchasedGoodID") > 0 Then
                        ddSearchPurchasedGoodValue.SelectedValue = HttpContext.Current.Request.QueryString("PurchasedGoodID")
                        ViewState("PurchasedGoodID") = HttpContext.Current.Request.QueryString("PurchasedGoodID")
                    End If
                End If

                If HttpContext.Current.Request.QueryString("OldMaterialGroup") <> "" Then
                    txtSearchOldMaterialGroupValue.Text = HttpContext.Current.Request.QueryString("OldMaterialGroup")
                    ViewState("OldMaterialGroup") = HttpContext.Current.Request.QueryString("OldMaterialGroup")
                End If

                If HttpContext.Current.Request.QueryString("isPackaging") <> "" Then
                    ViewState("isPackaging") = CType(HttpContext.Current.Request.QueryString("isPackaging"), Integer)
                End If

                If HttpContext.Current.Request.QueryString("filterPackaging") <> "" Then
                    ViewState("filterPackaging") = CType(HttpContext.Current.Request.QueryString("filterPackaging"), Integer)
                End If

                If ViewState("filterPackaging") > 0 And ViewState("isPackaging") > 0 Then
                    ddSearchPackaging.SelectedValue = "Only"
                End If

                If ViewState("filterPackaging") > 0 And ViewState("isPackaging") = 0 Then
                    ddSearchPackaging.SelectedValue = "None"
                End If

                If HttpContext.Current.Request.QueryString("isCoating") <> "" Then
                    ViewState("isCoating") = CType(HttpContext.Current.Request.QueryString("isCoating"), Integer)
                End If

                If HttpContext.Current.Request.QueryString("filterCoating") <> "" Then
                    ViewState("filterCoating") = CType(HttpContext.Current.Request.QueryString("filterCoating"), Integer)
                End If

                If ViewState("filterCoating") > 0 And ViewState("isCoating") > 0 Then
                    ddSearchCoating.SelectedValue = "Only"
                End If

                If ViewState("filterCoating") > 0 And ViewState("isCoating") = 0 Then
                    ddSearchCoating.SelectedValue = "None"
                End If

                ddSearchObsolete.SelectedValue = "Only"
                ViewState("filterObsolete") = 1
                ViewState("Obsolete") = 0

                If HttpContext.Current.Request.QueryString("Obsolete") <> "" Then
                    ViewState("Obsolete") = CType(HttpContext.Current.Request.QueryString("Obsolete"), Integer)
                End If

                If HttpContext.Current.Request.QueryString("filterObsolete") <> "" Then
                    ViewState("filterObsolete") = CType(HttpContext.Current.Request.QueryString("filterObsolete"), Integer)
                End If

                If ViewState("filterObsolete") > 0 And ViewState("Obsolete") > 0 Then
                    ddSearchObsolete.SelectedValue = "Only"
                End If

                If ViewState("filterObsolete") > 0 And ViewState("Obsolete") = 0 Then
                    ddSearchObsolete.SelectedValue = "None"
                End If

                If ViewState("filterObsolete") = 0 And ViewState("Obsolete") = 0 Then
                    ddSearchObsolete.SelectedValue = "All"
                End If

            Else
                ViewState("MaterialID") = txtSearchMaterialIDValue.Text.Trim
                ViewState("PartName") = txtSearchPartNameValue.Text.Trim
                ViewState("DrawingNo") = txtSearchDrawingNoValue.Text.Trim
                ViewState("PartNo") = txtSearchPartNoValue.Text.Trim

                If ddSearchVendorValue.SelectedIndex > 0 Then
                    ViewState("UGNDBVendorID") = ddSearchVendorValue.SelectedValue
                Else
                    ViewState("UGNDBVendorID") = 0
                End If

                If ddSearchPurchasedGoodValue.SelectedIndex > 0 Then
                    ViewState("PurchasedGoodID") = ddSearchPurchasedGoodValue.SelectedValue
                Else
                    ViewState("PurchasedGoodID") = 0
                End If

                ViewState("OldMaterialGroup") = txtSearchOldMaterialGroupValue.Text.Trim

                ViewState("isCoating") = 0
                ViewState("filterCoating") = 0

                If ddSearchCoating.SelectedIndex > 0 Then
                    If ddSearchCoating.SelectedValue = "Only" Then
                        ViewState("isCoating") = 1
                        ViewState("filterCoating") = 1
                    End If

                    If ddSearchCoating.SelectedValue = "None" Then
                        ViewState("isCoating") = 0
                        ViewState("filterCoating") = 1
                    End If
                End If

                ViewState("isPackaging") = 0
                ViewState("filterPackaging") = 0

                If ddSearchPackaging.SelectedIndex > 0 Then
                    If ddSearchPackaging.SelectedValue = "Only" Then
                        ViewState("isPackaging") = 1
                        ViewState("filterPackaging") = 1
                    End If

                    If ddSearchPackaging.SelectedValue = "None" Then
                        ViewState("isPackaging") = 0
                        ViewState("filterPackaging") = 1
                    End If
                End If

                ViewState("Obsolete") = 0
                ViewState("filterObsolete") = 0

                If ddSearchObsolete.SelectedIndex > 0 Then
                    If ddSearchObsolete.SelectedValue = "Only" Then
                        ViewState("Obsolete") = 1
                        ViewState("filterObsolete") = 1
                    End If

                    If ddSearchObsolete.SelectedValue = "None" Then
                        ViewState("Obsolete") = 0
                        ViewState("filterObsolete") = 1
                    End If
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

    Protected Sub gvMaterial_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMaterial.SelectedIndexChanged

        Try
            ' The user has selected an item from the GridView.
            ' Send the data back to the parent form.
            Dim row As GridViewRow = gvMaterial.SelectedRow
            Dim strMaterialID As String = row.Cells(1).Text
            Dim strQuoteCost As String = row.Cells(9).Text
            Dim strFreightCost As String = row.Cells(10).Text

            SendDataBackToParentForm(strMaterialID, strQuoteCost, strFreightCost)
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
