' ************************************************************************************************
'
' Name:		DrawingLookUp.aspx.vb

' Purpose:	This Code Behind to search drawings for any module. It is a popup
'
' Date  		Author	    
' 04/14/2008    Roderick Carlson
' 02/22/2010    Roderick Carlson - Added Make Dropdown box
' 03/02/2010    Roderick Carlson - Made to work similar to DrawingList page
' 01/15/2012    Roderick Carlson - Set default search to production
' 01/06/2014    LRey             - Replaced "BPCSPart " to "Part" wherever used. Changed drop down list Customer SOLDTO|CABBV to OEMManufactuer.
' ************************************************************************************************
Partial Class DrawingLookUp
    Inherits System.Web.UI.Page
   
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ds = commonFunctions.GetOEMManufacturer("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Commodity control for selection criteria 
            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If commonFunctions.CheckDataset(ds) = True Then
                ddCommodity.DataSource = ds
                ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
                ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddCommodity.DataBind()
                ddCommodity.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetDesignationType()
            If commonFunctions.CheckDataset(ds) = True Then
                ddDesignationType.DataSource = ds
                ddDesignationType.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName.ToString()
                ddDesignationType.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName
                ddDesignationType.DataBind()
                ddDesignationType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProgramMake()
            If commonFunctions.CheckDataset(ds) = True Then
                ddMake.DataSource = ds
                ddMake.DataTextField = ds.Tables(0).Columns("Make").ColumnName.ToString()
                ddMake.DataValueField = ds.Tables(0).Columns("Make").ColumnName
                ddMake.DataBind()
                ddMake.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProductTechnology(0)
            If (commonFunctions.CheckDataset(ds) = True) Then
                ddProductTechnology.DataSource = ds
                ddProductTechnology.DataTextField = ds.Tables(0).Columns("ddProductTechnologyName").ColumnName
                ddProductTechnology.DataValueField = ds.Tables(0).Columns("ProductTechnologyID").ColumnName
                ddProductTechnology.DataBind()
                ddProductTechnology.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Year control for selection criteria for search
            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Customer Plant control for selection criteria for search
            ds = commonFunctions.GetProgram("", "", "")
            If commonFunctions.CheckDataset(ds) = True Then
                ddProgram.DataSource = ds
                ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName
                ddProgram.DataBind()
                ddProgram.Items.Insert(0, "")
            End If

            ''bind existing data to drop down PartFamily control for selection criteria for search
            ds = commonFunctions.GetSubFamily(0)
            If commonFunctions.CheckDataset(ds) = True Then
                ddSubFamily.DataSource = ds
                ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddSubFamily.DataBind()
                ddSubFamily.Items.Insert(0, "")
            End If

            'bind existing data to drop down Density control for selection criteria for search
            ds = PEModule.GetDrawingDensity()
            If commonFunctions.CheckDataset(ds) = True Then
                ddDensityValue.DataSource = ds
                ddDensityValue.DataTextField = ds.Tables(0).Columns("densityValue").ColumnName
                ddDensityValue.DataValueField = ds.Tables(0).Columns("densityValue").ColumnName
                ddDensityValue.DataBind()
                ddDensityValue.Items.Insert(0, "")
            End If

            'bind existing data to drop down Density control for selection criteria for search
            ds = commonFunctions.GetPurchasedGood("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddPurchasedGood.DataSource = ds
                ddPurchasedGood.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
                ddPurchasedGood.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddPurchasedGood.DataBind()
                ddPurchasedGood.Items.Insert(0, "")
            End If

            ds = PEModule.GetDrawingReleaseTypeList()
            If commonFunctions.CheckDataset(ds) = True Then
                ddReleaseType.DataSource = ds
                ddReleaseType.DataTextField = ds.Tables(0).Columns("ddReleaseTypeName").ColumnName
                ddReleaseType.DataValueField = ds.Tables(0).Columns("ReleaseTypeID").ColumnName
                ddReleaseType.DataBind()
                ddReleaseType.Items.Insert(0, "")
                ddReleaseType.SelectedValue = 1 'default search to the production release type
            End If

            'bind existing data to drop down Density control for selection criteria for search
            ds = PEModule.GetDrawingByEngineers
            If commonFunctions.CheckDataset(ds) = True Then
                ddDrawingByEngineer.DataSource = ds
                ddDrawingByEngineer.DataTextField = ds.Tables(0).Columns("DrawingByEngineerFullName").ColumnName.ToString()
                ddDrawingByEngineer.DataValueField = ds.Tables(0).Columns("DrawingByEngineerID").ColumnName
                ddDrawingByEngineer.DataBind()
                ddDrawingByEngineer.Items.Insert(0, "")
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
                If Request.QueryString("DrawingControlID") IsNot Nothing Then
                    ViewState("DrawingControlID") = Request.QueryString("DrawingControlID").ToString()
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
    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

    '    Try
    '        If ddSearchSubFamily.SelectedIndex > 0 Then
    '            ViewState("SearchSubFamily") = ddSearchSubFamily.SelectedValue
    '        Else
    '            ViewState("SearchSubFamily") = 0
    '        End If

    '        If ddSearchCommodity.SelectedIndex > 0 Then
    '            ViewState("SearchCommodity") = ddSearchCommodity.SelectedValue
    '        Else
    '            ViewState("SearchCommodity") = 0
    '        End If

    '        If ddSearchProgram.SelectedIndex > 0 Then
    '            ViewState("SearchProgram") = ddSearchProgram.SelectedValue
    '        Else
    '            ViewState("SearchProgram") = 0
    '        End If

    '        If ddSearchPurchasedGood.SelectedIndex > 0 Then
    '            ViewState("SearchPurchasedGood") = ddSearchPurchasedGood.SelectedValue
    '        Else
    '            ViewState("SearchPurchasedGood") = 0
    '        End If

    '        odsDrawings.SelectParameters("DrawingNo").DefaultValue = ViewState("SearchSubDrawingCombo")
    '        odsDrawings.SelectParameters("SubFamilyID").DefaultValue = ViewState("SearchSubFamily")
    '        odsDrawings.SelectParameters("CommodityID").DefaultValue = ViewState("SearchCommodity")
    '        odsDrawings.SelectParameters("ProgramID").DefaultValue = ViewState("SearchProgram")
    '        odsDrawings.SelectParameters("PurchasedGoodID").DefaultValue = ViewState("SearchPurchasedGood")
    '        gvDrawings.DataBind()
    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text = ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            lblMessage.Text = ""

            txtPartNo.Text = ""
            txtConstruction.Text = ""
            txtCustomerPartNo.Text = ""
            txtDrawingNo.Text = ""
            txtLastUpdatedOnEnd.Text = ""
            txtLastUpdatedOnStart.Text = ""
            txtPartName.Text = ""
            txtNotes.Text = ""

            ddCommodity.SelectedIndex = -1
            ddCustomer.SelectedIndex = -1
            ddDensityValue.SelectedIndex = -1
            ddDesignationType.SelectedIndex = -1
            ddDrawingByEngineer.SelectedIndex = -1
            ddMake.SelectedIndex = -1            
            ddProductTechnology.SelectedIndex = -1
            ddProgram.SelectedIndex = -1
            ddPurchasedGood.SelectedIndex = -1
            'ddReleaseType.SelectedIndex = -1
            ddReleaseType.SelectedValue = 1 'default search to the production release type
            ddStatus.SelectedIndex = -1            
            ddSubFamily.SelectedIndex = -1
            ddYear.SelectedIndex = -1

        Catch ex As Exception

            'update error on web page
            lblMessage.Text += ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub SendDataBackToParentForm(ByVal DrawingNo As String)

        Try
            ' If the control id's of the parent form are available for receiving data, continue.
            ' Otherwise, exit this procedure.
            If ViewState("DrawingControlID") Is Nothing Then
                Exit Sub
            End If

            ' Build client JavaScript code:
            '  1. populate textboxes on parent form
            '  2. re-validate the parent form
            Dim strScript As String = _
                "<script>window.opener.document.forms[0]." & ViewState("DrawingControlID").ToString() & ".value = '" & DrawingNo & "';" & _
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

    Protected Sub gvDrawings_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDrawings.SelectedIndexChanged

        Try
            ' The user has selected an item from the GridView.
            ' Send the data back to the parent form.
            Dim row As GridViewRow = gvDrawings.SelectedRow
            Dim strDrawingNo As String = row.Cells(1).Text

            SendDataBackToParentForm(strDrawingNo)
        Catch ex As Exception
            'update error on web page
            lblMessage.Text += ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Protected Sub ddSearchMake_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddSearchMake.SelectedIndexChanged

    '    lblMessage.Text = ""

    '    Try
    '        Dim dsProgram As DataSet

    '        If ddSearchMake.SelectedIndex > 0 Then
    '            dsProgram = commonFunctions.GetProgram("", "", ddSearchMake.SelectedValue)
    '            If commonFunctions.CheckDataset(dsProgram) = True Then
    '                ddSearchProgram.Items.Clear()
    '                ddSearchProgram.DataSource = dsProgram
    '                ddSearchProgram.DataTextField = dsProgram.Tables(0).Columns("ddProgramName").ColumnName.ToString()
    '                ddSearchProgram.DataValueField = dsProgram.Tables(0).Columns("ProgramID").ColumnName
    '                ddSearchProgram.DataBind()
    '                ddSearchProgram.Items.Insert(0, "")
    '            End If
    '        Else
    '            dsProgram = commonFunctions.GetProgram("", "", "")
    '            If commonFunctions.CheckDataset(dsProgram) = True Then
    '                ddSearchProgram.Items.Clear()
    '                ddSearchProgram.DataSource = dsProgram
    '                ddSearchProgram.DataTextField = dsProgram.Tables(0).Columns("ddProgramName").ColumnName.ToString()
    '                ddSearchProgram.DataValueField = dsProgram.Tables(0).Columns("ProgramID").ColumnName
    '                ddSearchProgram.DataBind()
    '                ddSearchProgram.Items.Insert(0, "")
    '            End If
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    'End Sub

    Protected Sub cbAdvancedSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbAdvancedSearch.CheckedChanged

        tblAdvancedSearch.Visible = cbAdvancedSearch.Checked

    End Sub
End Class
