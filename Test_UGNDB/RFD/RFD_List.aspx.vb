' ************************************************************************************************
'
' Name:		RFD_List.aspx
' Purpose:	This Code Behind is for the Request for Development List
'
' Date		 Author	    
' 01/13/2010 Roderick Carlson
' 09/25/2011 Roderick Carlson   Added export to Excel
' 03/28/2012 Roderick Carlson   Added Status Report functionality
' 04/09/2012 Roderick Carlson   Allow File Type Selection for RFD Report
' 04/18/2012 Roderick Carlson   Modifiy Business Process Type and Action events based on new Business Award Rules
' 04/27/2012 Roderick Carlson   Added Program Manager to search
' 05/08/2012 Roderick Carlson   Added Purchasgin for External RFQ role
' 10/17/2012 Roderick Carlson   Added Corporate Engineering Status Report role
' 01/22/2014 LRey               Replaced "BPCSPart" with "PART" and SoldTo|CABBV with Customer wherever used.
' ************************************************************************************************

Partial Class RFD_List
    Inherits System.Web.UI.Page
    Protected WithEvents lnkStatusName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRFDNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPreviousRFDNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewDrawingNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewPartNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewCustomerPartNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewPartName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewDesignLevel As System.Web.UI.WebControls.LinkButton

    Private htControls As New System.Collections.Hashtable

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        'DO NOT DELETE THIS

    End Sub
    Private Sub PrepareGridViewForExport(ByRef gv As Control)

        Dim l As Literal = New Literal()
        Dim i As Integer


        For i = 0 To gv.Controls.Count

            If ((Nothing <> htControls(gv.Controls(i).GetType().Name)) Or (Nothing <> htControls(gv.Controls(i).GetType().BaseType.Name))) Then
                l.Text = GetControlPropertyValue(gv.Controls(i))

                gv.Controls.Remove(gv.Controls(i))

                gv.Controls.AddAt(i, l)

            End If

            If (gv.Controls(i).HasControls()) Then

                PrepareGridViewForExport(gv.Controls(i))

            End If

        Next

    End Sub
    Private Function GetControlPropertyValue(ByVal control As Control) As String
        Dim controlType As Type = control.[GetType]()
        Dim strControlType As String = controlType.Name
        Dim strReturn As String = "Error"
        Dim bReturn As Boolean

        Dim ctrlProps As System.Reflection.PropertyInfo() = controlType.GetProperties()
        Dim ExcelPropertyName As String = DirectCast(htControls(strControlType), String)

        If ExcelPropertyName Is Nothing Then
            ExcelPropertyName = DirectCast(htControls(control.[GetType]().BaseType.Name), String)
            If ExcelPropertyName Is Nothing Then
                Return strReturn
            End If
        End If

        For Each ctrlProp As System.Reflection.PropertyInfo In ctrlProps

            If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(String) Then
                Try
                    strReturn = DirectCast(ctrlProp.GetValue(control, Nothing), String)
                    Exit Try
                Catch
                    strReturn = ""
                End Try
            End If

            If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(Boolean) Then
                Try
                    bReturn = CBool(ctrlProp.GetValue(control, Nothing))
                    strReturn = IIf(bReturn, "True", "False")
                    Exit Try
                Catch
                    strReturn = "Error"
                End Try
            End If

            If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(ListItem) Then
                Try
                    strReturn = DirectCast((ctrlProp.GetValue(control, Nothing)), ListItem).Text
                    Exit Try
                Catch
                    strReturn = ""
                End Try
            End If
        Next
        Return strReturn
    End Function

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Try

            Dim attachment As String = "attachment; filename=RFDList.xls"

            Response.ClearContent()

            Response.AddHeader("content-disposition", attachment)

            Response.ContentType = "application/vnd.ms-excel"

            Dim sw As StringWriter = New StringWriter()

            Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)

            'EnablePartialRendering = False

            Dim ds As DataSet
            ds = RFDModule.GetRFDSearch(ViewState("RFDNo"), ViewState("RFDDesc"), ViewState("StatusID"), ViewState("ApproverStatusID"), _
                         ViewState("DrawingNo"), ViewState("PriorityID"), ViewState("CustomerPartNo"), ViewState("DesignLevel"), ViewState("PartNo"), ViewState("PartName"), ViewState("InitiatorID"), _
                         ViewState("ApproverID"), ViewState("AccountManagerID"), ViewState("ProgramManagerID"), ViewState("BusinessProcessActionID"), ViewState("BusinessProcessTypeID"), _
                         ViewState("DesignationType"), ViewState("CustomerValue"), ViewState("UGNFacility"), _
                         ViewState("ProgramID"), ViewState("CommodityID"), ViewState("ProductTechnologyID"), ViewState("SubFamilyID"), _
                         ViewState("UGNDBVendorID"), ViewState("PurchasedGoodID"), ViewState("CostSheetID"), ViewState("ECINo"), _
                         ViewState("CapExProjectNo"), ViewState("PurchasingPONo"), ViewState("DueDateStart"), _
                         ViewState("DueDateEnd"), ViewState("SubscriptionID"), _
                         ViewState("FilterBusinessAwarded"), ViewState("IsBusinessAwarded"), ViewState("IncludeArchive"))

            If commonFunctions.CheckDataSet(ds) = True Then
                Dim tempDataGridView As New GridView

                tempDataGridView.HeaderStyle.BackColor = Color.White 'System.Drawing.Color.LightGray
                tempDataGridView.HeaderStyle.ForeColor = Color.Black 'System.Drawing.Color.Black
                tempDataGridView.HeaderStyle.Font.Bold = True

                tempDataGridView.AutoGenerateColumns = False

                Dim RFDNoColumn As New BoundField
                RFDNoColumn.HeaderText = "RFDNo"
                RFDNoColumn.DataField = "RFDNo"
                tempDataGridView.Columns.Add(RFDNoColumn)

                Dim RFDDescColumn As New BoundField
                RFDDescColumn.HeaderText = "Desc"
                RFDDescColumn.DataField = "RFDDesc"
                tempDataGridView.Columns.Add(RFDDescColumn)

                Dim OverallStatusNameColumn As New BoundField
                OverallStatusNameColumn.HeaderText = "Overall Status"
                OverallStatusNameColumn.DataField = "ddStatusName"
                tempDataGridView.Columns.Add(OverallStatusNameColumn)

                Dim BusinessProcessTypeNameColumn As New BoundField
                BusinessProcessTypeNameColumn.HeaderText = "Business Process Type"
                BusinessProcessTypeNameColumn.DataField = "ddBusinessProcessTypeName"
                tempDataGridView.Columns.Add(BusinessProcessTypeNameColumn)

                Dim BusinessProcessActionNameColumn As New BoundField
                BusinessProcessActionNameColumn.HeaderText = "Business Process Action"
                BusinessProcessActionNameColumn.DataField = "ddBusinessProcessActionName"
                tempDataGridView.Columns.Add(BusinessProcessActionNameColumn)

                Dim DueDateColumn As New BoundField
                DueDateColumn.HeaderText = "Due Date"
                DueDateColumn.DataField = "DueDate"
                tempDataGridView.Columns.Add(DueDateColumn)

                Dim CompletionDateColumn As New BoundField
                CompletionDateColumn.HeaderText = "Completion Date"
                CompletionDateColumn.DataField = "CompletionDate"
                tempDataGridView.Columns.Add(CompletionDateColumn)

                Dim isBusinessAwardedColumn As New BoundField
                isBusinessAwardedColumn.HeaderText = "isBusinessAwarded"
                isBusinessAwardedColumn.DataField = "isBusinessAwarded"
                tempDataGridView.Columns.Add(isBusinessAwardedColumn)

                Dim NewDrawingNoColumn As New BoundField
                NewDrawingNoColumn.HeaderText = "DrawingNo"
                NewDrawingNoColumn.DataField = "NewDrawingNo"
                tempDataGridView.Columns.Add(NewDrawingNoColumn)

                Dim NewCustomerPartNoColumn As New BoundField
                NewCustomerPartNoColumn.HeaderText = "Customer PartNo"
                NewCustomerPartNoColumn.DataField = "NewCustomerPartNo"
                tempDataGridView.Columns.Add(NewCustomerPartNoColumn)

                Dim NewPartNoColumn As New BoundField
                NewPartNoColumn.HeaderText = "Internal Part No"
                NewPartNoColumn.DataField = "NewPartNo"
                tempDataGridView.Columns.Add(NewPartNoColumn)

                Dim NewPartNameColumn As New BoundField
                NewPartNameColumn.HeaderText = "Part Name"
                NewPartNameColumn.DataField = "NewPartName"
                tempDataGridView.Columns.Add(NewPartNameColumn)

                Dim NewDesignLevelColumn As New BoundField
                NewDesignLevelColumn.HeaderText = "Design Level"
                NewDesignLevelColumn.DataField = "NewDesignLevel"
                tempDataGridView.Columns.Add(NewDesignLevelColumn)

                Dim PackagingStatusNameColumn As New BoundField
                PackagingStatusNameColumn.HeaderText = "Packaging Status"
                PackagingStatusNameColumn.DataField = "ddPackagingStatusName"
                tempDataGridView.Columns.Add(PackagingStatusNameColumn)

                Dim ProcessStatusNameColumn As New BoundField
                ProcessStatusNameColumn.HeaderText = "Process Status"
                ProcessStatusNameColumn.DataField = "ddProcessStatusName"
                tempDataGridView.Columns.Add(ProcessStatusNameColumn)

                Dim ToolingStatusNameColumn As New BoundField
                ToolingStatusNameColumn.HeaderText = "Tooling Status"
                ToolingStatusNameColumn.DataField = "ddToolingStatusName"
                tempDataGridView.Columns.Add(ToolingStatusNameColumn)

                Dim CapitalStatusNameColumn As New BoundField
                CapitalStatusNameColumn.HeaderText = "Capital Status"
                CapitalStatusNameColumn.DataField = "ddCapitalStatusName"
                tempDataGridView.Columns.Add(CapitalStatusNameColumn)

                Dim ProductDevelopmentStatusNameColumn As New BoundField
                ProductDevelopmentStatusNameColumn.HeaderText = "Product Development Status"
                ProductDevelopmentStatusNameColumn.DataField = "ddProductDevelopmentStatusName"
                tempDataGridView.Columns.Add(ProductDevelopmentStatusNameColumn)

                Dim CostingStatusNameColumn As New BoundField
                CostingStatusNameColumn.HeaderText = "Costing Status"
                CostingStatusNameColumn.DataField = "ddCostingStatusName"
                tempDataGridView.Columns.Add(CostingStatusNameColumn)

                Dim QualityEngineerStatusNameColumn As New BoundField
                QualityEngineerStatusNameColumn.HeaderText = "Quality Engineer Status"
                QualityEngineerStatusNameColumn.DataField = "ddQualityEngineeringStatusName"
                tempDataGridView.Columns.Add(QualityEngineerStatusNameColumn)

                Dim PurchasingStatusNameColumn As New BoundField
                PurchasingStatusNameColumn.HeaderText = "Purchasing Status"
                PurchasingStatusNameColumn.DataField = "ddPurchasingStatusName"
                tempDataGridView.Columns.Add(PurchasingStatusNameColumn)

                tempDataGridView.DataSource = ds
                tempDataGridView.DataBind()

                tempDataGridView.RenderControl(htw)

                Response.Write(sw.ToString())

                Response.End()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Function SetHistoryVisible(ByVal ArchiveDate As String) As Boolean

        Dim bReturnValue As Boolean = True

        Try
            If ArchiveDate <> "" Then
                If CType(ArchiveDate, Integer) > 0 Then
                    bReturnValue = False
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetHistoryVisible = bReturnValue

    End Function

    Protected Function SetPreviewVisible(ByVal StatusID As String) As Boolean

        Dim bReturnValue As Boolean = True

        Try
            If StatusID <> "" Then
                If CType(StatusID, Integer) = 4 Then 'voided
                    bReturnValue = False
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewVisible = bReturnValue

    End Function

    Protected Sub CheckIncludeArchives()

        Try
            ddSearchCustomer.Visible = Not cbIncludeArchive.Checked
            ddSearchCommodity.Visible = Not cbIncludeArchive.Checked
            ddSearchDesignationType.Visible = Not cbIncludeArchive.Checked
            ddSearchInitiator.Visible = Not cbIncludeArchive.Checked
            ddSearchPriority.Visible = Not cbIncludeArchive.Checked
            ddSearchProductTechnology.Visible = Not cbIncludeArchive.Checked
            ddSearchProgram.Visible = Not cbIncludeArchive.Checked
            ddSearchPurchasedGood.Visible = Not cbIncludeArchive.Checked
            ddSearchSubFamily.Visible = Not cbIncludeArchive.Checked
            ddSearchUGNFacility.Visible = Not cbIncludeArchive.Checked
            ddSearchVendor.Visible = Not cbIncludeArchive.Checked

            txtSearchCapExProjectNo.Visible = Not cbIncludeArchive.Checked
            txtSearchCostSheetID.Visible = Not cbIncludeArchive.Checked
            txtSearchDueDateStart.Visible = Not cbIncludeArchive.Checked
            txtSearchDueDateEnd.Visible = Not cbIncludeArchive.Checked
            txtSearchECINo.Visible = Not cbIncludeArchive.Checked
            txtSearchPONo.Visible = Not cbIncludeArchive.Checked

            imgSearchDueDateStart.Visible = Not cbIncludeArchive.Checked
            imgSearchDueDateEnd.Visible = Not cbIncludeArchive.Checked

            ShowAdvancedSearch()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = RFDModule.GetRFDSearch(ViewState("RFDNo"), ViewState("RFDDesc"), ViewState("StatusID"), ViewState("ApproverStatusID"), ViewState("DrawingNo"), ViewState("PriorityID"), _
            ViewState("CustomerPartNo"), ViewState("DesignLevel"), ViewState("PartNo"), ViewState("PartName"), ViewState("InitiatorID"), _
            ViewState("ApproverID"), ViewState("AccountManagerID"), ViewState("ProgramManagerID"), ViewState("BusinessProcessActionID"), ViewState("BusinessProcessTypeID"), ViewState("DesignationType"), _
            ViewState("CustomerValue"), ViewState("UGNFacility"), ViewState("ProgramID"), ViewState("CommodityID"), _
            ViewState("ProductTechnologyID"), ViewState("SubFamilyID"), ViewState("UGNDBVendorID"), ViewState("PurchasedGoodID"), _
            ViewState("CostSheetID"), ViewState("ECINo"), ViewState("CapExProjectNo"), ViewState("PurchasingPONo"), ViewState("DueDateStart"), _
            ViewState("DueDateEnd"), ViewState("SubscriptionID"), ViewState("FilterBusinessAwarded"), ViewState("IsBusinessAwarded"), ViewState("IncludeArchive"))

            If commonFunctions.CheckDataSet(ds) = True Then
                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpRFDInfo.DataSource = dv
                rpRFDInfo.DataBind()

                'Dispose Items
                ds.Dispose()
                dv.Dispose()
            Else
                cmdFirst.Enabled = False
                cmdGo.Enabled = False
                cmdPrev.Enabled = False
                cmdNext.Enabled = False
                cmdLast.Enabled = False

                rpRFDInfo.Visible = False

                txtGoToPage.Visible = False
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

    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
       Handles lnkStatusName.Click, lnkRFDNo.Click, lnkPreviousRFDNo.Click, lnkNewDrawingNo.Click, lnkNewPartNo.Click, lnkNewDesignLevel.Click, lnkNewCustomerPartNo.Click, lnkNewPartName.Click

        Try
            Dim lnkButton As New LinkButton
            lnkButton = CType(sender, LinkButton)
            Dim order As String = lnkButton.CommandArgument
            Dim sortType As String = ViewState(lnkButton.ID)

            SortByColumn(order + " " + sortType)
            If sortType = "ASC" Then
                'if column set to ascending sort, change to descending for next click on that column
                lnkButton.CommandName = "DESC"
                ViewState(lnkButton.ID) = "DESC"
            Else
                lnkButton.CommandName = "ASC"
                ViewState(lnkButton.ID) = "ASC"
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

    Protected Function SetRFDHyperlink(ByVal RFDNo As String, ByVal BusinessProcessTypeID As String, ByVal ArchiveData As Integer) As String

        Dim strReturnValue As String = ""

        Try
            If ArchiveData = 0 Then
                strReturnValue = "RFD_Detail.aspx?RFDNo=" & RFDNo
            Else
                strReturnValue = "javascript:void(window.open('crRFD_Preview.aspx?RFDNo=" & RFDNo & "&BusinessProcessTypeID=" & BusinessProcessTypeID & "&ArchiveData=" & ArchiveData & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetRFDHyperlink = strReturnValue

    End Function

    Protected Function SetPreviewRFDHyperLink(ByVal RFDNo As String, ByVal StatusID As String) As String

        Dim strReturnValue As String = ""

        Try
            If RFDNo <> "" And StatusID <> "4" Then
                'strReturnValue = "javascript:void(window.open('crRFD_Preview.aspx?RFDNo=" & RFDNo & "&BusinessProcessTypeID=" & BusinessProcessTypeID & "&ArchiveData=" & ArchiveData & "&SOPNo=" & SOPNo & "&SOPRev=" & SOPRev & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                strReturnValue = "javascript:void(window.open('crRFD_Preview.aspx?RFDNo=" & RFDNo & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewRFDHyperLink = strReturnValue

    End Function

    Protected Function SetBackGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "White" 'N/A or 3-complete 

        Try
            Select Case StatusID
                Case "1" 'open
                    strReturnValue = "Fuchsia"
                Case "2" 'in-process
                    strReturnValue = "Yellow"
                Case "4" 'void
                    strReturnValue = "Gray"
                Case "5" 'rejected
                    strReturnValue = "Red"
                Case "6" 'tasked
                    strReturnValue = "Aqua"
                Case "7", "9" 'on-hold or waiting on cost sheet approval
                    strReturnValue = "Blue"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetBackGroundColor = strReturnValue

    End Function

    Protected Function SetForeGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "Black" 'default

        Try
            Select Case StatusID
                'Case "1" 'open
                '    strReturnValue = "Black"
                'Case "2" 'in-process
                '    strReturnValue = "Black"
                Case "4" 'void
                    strReturnValue = "White"
                Case "5" 'rejected
                    strReturnValue = "White"
                    'Case "6" 'tasked
                    '    strReturnValue = "Black"
                Case "7", "9" 'on-hold / waiting on cost sheet approval
                    strReturnValue = "White"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetForeGroundColor = strReturnValue

    End Function

    Private Sub BindData()

        Try

            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = RFDModule.GetRFDSearch(ViewState("RFDNo"), ViewState("RFDDesc"), ViewState("StatusID"), ViewState("ApproverStatusID"), _
             ViewState("DrawingNo"), ViewState("PriorityID"), ViewState("CustomerPartNo"), ViewState("DesignLevel"), ViewState("PartNo"), ViewState("PartName"), ViewState("InitiatorID"), _
             ViewState("ApproverID"), ViewState("AccountManagerID"), ViewState("ProgramManagerID"), ViewState("BusinessProcessActionID"), ViewState("BusinessProcessTypeID"), _
             ViewState("DesignationType"), ViewState("CustomerValue"), ViewState("UGNFacility"), _
             ViewState("ProgramID"), ViewState("CommodityID"), ViewState("ProductTechnologyID"), ViewState("SubFamilyID"), _
             ViewState("UGNDBVendorID"), ViewState("PurchasedGoodID"), ViewState("CostSheetID"), ViewState("ECINo"), _
             ViewState("CapExProjectNo"), ViewState("PurchasingPONo"), ViewState("DueDateStart"), _
             ViewState("DueDateEnd"), ViewState("SubscriptionID"), _
             ViewState("FilterBusinessAwarded"), ViewState("IsBusinessAwarded"), ViewState("IncludeArchive"))

            If commonFunctions.CheckDataSet(ds) = True Then
                rpRFDInfo.DataSource = ds
                rpRFDInfo.DataBind()

                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 15

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpRFDInfo.DataSource = objPds
                rpRFDInfo.DataBind()

                lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                ViewState("LastPageCount") = objPds.PageCount - 1
                txtGoToPage.Text = CurrentPage + 1

                ' Disable Prev or Next buttons if necessary
                cmdFirst.Enabled = Not objPds.IsFirstPage
                cmdPrev.Enabled = Not objPds.IsFirstPage
                cmdNext.Enabled = Not objPds.IsLastPage
                cmdLast.Enabled = Not objPds.IsLastPage
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Request For Development - List and Search"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Request for Development </b> > List and Search "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("RFDExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Property CurrentPage() As Integer

        Get
            ' look for current page in ViewState
            Dim o As Object = ViewState("_CurrentPage")
            If (o Is Nothing) Then
                Return 0 ' default page index of 0
            Else
                Return o
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("_CurrentPage") = value
        End Set

    End Property

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
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                If iTeamMemberID = 530 Then
                    'iTeamMemberID = 4 'Kenta Shinohara 
                    iTeamMemberID = 105 'Ron Davis
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 37)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isEdit") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete

                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isEdit") = True
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select

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

    Protected Sub EnableControls()

        Try

            btnAdd.Enabled = ViewState("isEdit")

            CheckIncludeArchives()

            lblSearchBusinessProcessAction.Visible = False
            ddSearchBusinessProcessAction.Visible = False

            lblSearchBusinessAwarded.Visible = False
            ddSearchBusinessAwarded.Visible = False

            If ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7 Then
                lblSearchBusinessProcessAction.Visible = True
                ddSearchBusinessProcessAction.Visible = True

                'If ViewState("BusinessProcessActionID") = 1 Then
                If ViewState("BusinessProcessActionID") = 10 Then
                    lblSearchBusinessAwarded.Visible = True
                    ddSearchBusinessAwarded.Visible = True
                Else
                    ddSearchBusinessAwarded.SelectedIndex = -1
                    ViewState("FilterBusinessAwarded") = 0
                    ViewState("IsBusinessAwarded") = 0
                End If

                'If ViewState("BusinessProcessTypeID") = 1 Then
                '    BindBusinessProcessAction(True, False)
                'End If

                'If ViewState("BusinessProcessTypeID") = 7 Then
                '    BindBusinessProcessAction(True, True)
                'End If

                'ddSearchBusinessProcessAction.SelectedValue = ViewState("BusinessProcessActionID")
            Else
                ddSearchBusinessProcessAction.SelectedIndex = -1
                ViewState("BusinessProcessActionID") = 0

                ddSearchBusinessAwarded.SelectedIndex = -1
                ViewState("FilterBusinessAwarded") = 0
                ViewState("IsBusinessAwarded") = 0
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
            CheckRights()

            'clear crystal reports
            RFDModule.CleanRFDCrystalReports()

            If HttpContext.Current.Session("sessionRFDCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionRFDCurrentPage")
            End If

            If Not Page.IsPostBack Then
                ViewState("lnkStatusName") = "DESC"
                ViewState("lnkRFDNo") = "ASC"
                ViewState("lnkPreviousRFDNo") = "ASC"
                ViewState("lnkNewDrawingNo") = "ASC"
                ViewState("lnkNewPartNo") = "ASC"
                ViewState("lnkNewCustomerPartNo") = "ASC"
                ViewState("lnkNewPartName") = "ASC"

                ViewState("RFDNo") = ""
                ViewState("RFDDesc") = ""
                ViewState("StatusID") = 0
                ViewState("ApproverStatusID") = 0
                ViewState("DrawingNo") = ""
                ViewState("PriorityID") = 0
                ViewState("CustomerPartNo") = ""
                ViewState("DesignLevel") = ""
                ViewState("PartNo") = ""
                ViewState("PartName") = ""
                ViewState("InitiatorID") = 0
                ViewState("ApproverID") = 0
                ViewState("AccountManagerID") = 0
                ViewState("ProgramManagerID") = 0
                ViewState("BusinessProcessActionID") = 0
                ViewState("BusinessProcessTypeID") = 0
                ViewState("DesignationType") = ""
                ViewState("CustomerValue") = ""
                ViewState("UGNFacility") = ""
                ViewState("ProgramID") = 0
                ViewState("CommodityID") = 0
                ViewState("ProductTechnologyID") = 0
                ViewState("SubFamilyID") = 0
                ViewState("UGNDBVendorID") = 0
                ViewState("PurchasedGoodID") = 0
                ViewState("CostSheetID") = ""
                ViewState("ECINo") = ""
                ViewState("CapExProjectNo") = ""
                ViewState("PurchasingPONo") = ""
                ViewState("DueDateStart") = ""
                ViewState("DueDateEnd") = ""
                ViewState("SubscriptionID") = 0
                ViewState("FilterBusinessAwarded") = 0
                ViewState("IsBusinessAwarded") = 0
                ViewState("IncludeArchive") = 0

                '' ''******
                '' '' Bind drop down lists
                '' ''******
                BindCriteria()

                '' ''******
                ' ''get saved value of past search criteria or query string, query string takes precedence
                '' ''******

                If HttpContext.Current.Request.QueryString("RFDNo") <> "" Then
                    txtSearchRFDNo.Text = HttpContext.Current.Request.QueryString("RFDNo")
                    ViewState("RFDNo") = HttpContext.Current.Request.QueryString("RFDNo")
                Else
                    If Not Request.Cookies("RFDModule_SaveRFDNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveRFDNoSearch").Value) <> "" Then
                            txtSearchRFDNo.Text = Request.Cookies("RFDModule_SaveRFDNoSearch").Value
                            ViewState("RFDNo") = Request.Cookies("RFDModule_SaveRFDNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("RFDDesc") <> "" Then
                    txtSearchRFDDesc.Text = HttpContext.Current.Request.QueryString("RFDDesc")
                    ViewState("RFDDesc") = HttpContext.Current.Request.QueryString("RFDDesc")
                Else
                    If Not Request.Cookies("RFDModule_SaveRFDDescSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveRFDDescSearch").Value) <> "" Then
                            txtSearchRFDDesc.Text = Request.Cookies("RFDModule_SaveRFDDescSearch").Value
                            ViewState("RFDDesc") = Request.Cookies("RFDModule_SaveRFDDescSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("StatusID") <> "" Then
                    ddSearchStatus.SelectedValue = HttpContext.Current.Request.QueryString("StatusID")
                    ViewState("StatusID") = HttpContext.Current.Request.QueryString("StatusID")
                Else
                    If Not Request.Cookies("RFDModule_SaveStatusIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveStatusIDSearch").Value) <> "" Then
                            ddSearchStatus.SelectedValue = Request.Cookies("RFDModule_SaveStatusIDSearch").Value
                            ViewState("StatusID") = Request.Cookies("RFDModule_SaveStatusIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ApproverStatusID") <> "" Then
                    ddSearchApproverStatus.SelectedValue = HttpContext.Current.Request.QueryString("ApproverStatusID")
                    ViewState("ApproverStatusID") = HttpContext.Current.Request.QueryString("ApproverStatusID")
                Else
                    If Not Request.Cookies("RFDModule_SaveApproverStatusIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveApproverStatusIDSearch").Value) <> "" Then
                            ddSearchApproverStatus.SelectedValue = Request.Cookies("RFDModule_SaveApproverStatusIDSearch").Value
                            ViewState("ApproverStatusID") = Request.Cookies("RFDModule_SaveApproverStatusIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    txtSearchDrawingNo.Text = HttpContext.Current.Request.QueryString("DrawingNo")
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                Else
                    If Not Request.Cookies("RFDModule_SaveDrawingNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveDrawingNoSearch").Value) <> "" Then
                            txtSearchDrawingNo.Text = Request.Cookies("RFDModule_SaveDrawingNoSearch").Value
                            ViewState("DrawingNo") = Request.Cookies("RFDModule_SaveDrawingNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PriorityID") <> "" Then
                    ddSearchPriority.SelectedValue = HttpContext.Current.Request.QueryString("PriorityID")
                    ViewState("PriorityID") = HttpContext.Current.Request.QueryString("PriorityID")
                Else
                    If Not Request.Cookies("RFDModule_SavePrioritySearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SavePrioritySearch").Value) <> "" Then
                            ddSearchPriority.SelectedValue = Request.Cookies("RFDModule_SavePrioritySearch").Value
                            ViewState("PriorityID") = Request.Cookies("RFDModule_SavePrioritySearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CustomerPartNo") <> "" Then
                    txtSearchCustomerPartNo.Text = HttpContext.Current.Request.QueryString("CustomerPartNo")
                    ViewState("CustomerPartNo") = HttpContext.Current.Request.QueryString("CustomerPartNo")
                Else
                    If Not Request.Cookies("RFDModule_SaveCustomerPartNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveCustomerPartNoSearch").Value) <> "" Then
                            txtSearchCustomerPartNo.Text = Request.Cookies("RFDModule_SaveCustomerPartNoSearch").Value
                            ViewState("CustomerPartNo") = Request.Cookies("RFDModule_SaveCustomerPartNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DesignLevel") <> "" Then
                    txtSearchDesignLevel.Text = HttpContext.Current.Request.QueryString("DesignLevel")
                    ViewState("DesignLevel") = HttpContext.Current.Request.QueryString("DesignLevel")
                Else
                    If Not Request.Cookies("RFDModule_SaveDesignLevelSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveDesignLevelSearch").Value) <> "" Then
                            txtSearchDesignLevel.Text = Request.Cookies("RFDModule_SaveDesignLevelSearch").Value
                            ViewState("DesignLevel") = Request.Cookies("RFDModule_SaveDesignLevelSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PartNo") <> "" Then
                    txtSearchPartNo.Text = HttpContext.Current.Request.QueryString("PartNo")
                    ViewState("PartNo") = HttpContext.Current.Request.QueryString("PartNo")
                Else
                    If Not Request.Cookies("RFDModule_SavePartNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SavePartNoSearch").Value) <> "" Then
                            txtSearchPartNo.Text = Request.Cookies("RFDModule_SavePartNoSearch").Value
                            ViewState("PartNo") = Request.Cookies("RFDModule_SavePartNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PartName") <> "" Then
                    txtSearchPartName.Text = HttpContext.Current.Request.QueryString("PartName")
                    ViewState("PartName") = HttpContext.Current.Request.QueryString("PartName")
                Else
                    If Not Request.Cookies("RFDModule_SavePartNameSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SavePartNameSearch").Value) <> "" Then
                            txtSearchPartName.Text = Request.Cookies("RFDModule_SavePartNameSearch").Value
                            ViewState("PartName") = Request.Cookies("RFDModule_SavePartNameSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("InitiatorID") <> "" Then
                    ddSearchInitiator.SelectedValue = HttpContext.Current.Request.QueryString("InitiatorID")
                    ViewState("InitiatorID") = HttpContext.Current.Request.QueryString("InitiatorID")
                Else
                    If Not Request.Cookies("RFDModule_SaveInitiatorIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveInitiatorIDSearch").Value) <> "" Then
                            ddSearchInitiator.SelectedValue = Request.Cookies("RFDModule_SaveInitiatorIDSearch").Value
                            ViewState("InitiatorID") = Request.Cookies("RFDModule_SaveInitiatorIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ApproverID") <> "" Then
                    ddSearchApprover.SelectedValue = HttpContext.Current.Request.QueryString("ApproverID")
                    ViewState("ApproverID") = HttpContext.Current.Request.QueryString("ApproverID")
                Else
                    If Not Request.Cookies("RFDModule_SaveApproverIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveApproverIDSearch").Value) <> "" Then
                            ddSearchApprover.SelectedValue = Request.Cookies("RFDModule_SaveApproverIDSearch").Value
                            ViewState("ApproverID") = Request.Cookies("RFDModule_SaveApproverIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("AccountManagerID") <> "" Then
                    ddSearchAccountManager.SelectedValue = HttpContext.Current.Request.QueryString("AccountManagerID")
                    ViewState("AccountManagerID") = HttpContext.Current.Request.QueryString("AccountManagerID")
                Else
                    If Not Request.Cookies("RFDModule_SaveAccountManagerIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveAccountManagerIDSearch").Value) <> "" Then
                            ddSearchAccountManager.SelectedValue = Request.Cookies("RFDModule_SaveAccountManagerIDSearch").Value
                            ViewState("AccountManagerID") = Request.Cookies("RFDModule_SaveAccountManagerIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ProgramManagerID") <> "" Then
                    ddSearchProgramManager.SelectedValue = HttpContext.Current.Request.QueryString("ProgramManagerID")
                    ViewState("ProgramManagerID") = HttpContext.Current.Request.QueryString("ProgramManagerID")
                Else
                    If Not Request.Cookies("RFDModule_SaveProgramManagerIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveProgramManagerIDSearch").Value) <> "" Then
                            ddSearchProgramManager.SelectedValue = Request.Cookies("RFDModule_SaveProgramManagerIDSearch").Value
                            ViewState("ProgramManagerID") = Request.Cookies("RFDModule_SaveProgramManagerIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("BusinessProcessActionID") <> "" Then
                    ddSearchBusinessProcessType.SelectedValue = HttpContext.Current.Request.QueryString("BusinessProcessActionID")
                    ViewState("BusinessProcessActionID") = HttpContext.Current.Request.QueryString("BusinessProcessActionID")
                Else
                    If Not Request.Cookies("RFDModule_SaveBusinessProcessActionIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveBusinessProcessActionIDSearch").Value) <> "" Then
                            ddSearchBusinessProcessAction.SelectedValue = Request.Cookies("RFDModule_SaveBusinessProcessActionIDSearch").Value
                            ViewState("BusinessProcessActionID") = Request.Cookies("RFDModule_SaveBusinessProcessActionIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("BusinessProcessTypeID") <> "" Then
                    ddSearchBusinessProcessType.SelectedValue = HttpContext.Current.Request.QueryString("BusinessProcessTypeID")
                    ViewState("BusinessProcessTypeID") = HttpContext.Current.Request.QueryString("BusinessProcessTypeID")
                Else
                    If Not Request.Cookies("RFDModule_SaveBusinessProcessTypeIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveBusinessProcessTypeIDSearch").Value) <> "" Then
                            ddSearchBusinessProcessType.SelectedValue = Request.Cookies("RFDModule_SaveBusinessProcessTypeIDSearch").Value
                            ViewState("BusinessProcessTypeID") = Request.Cookies("RFDModule_SaveBusinessProcessTypeIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DesignationType") <> "" Then
                    ddSearchDesignationType.SelectedValue = HttpContext.Current.Request.QueryString("DesignationType")
                    ViewState("DesignationType") = HttpContext.Current.Request.QueryString("DesignationType")
                Else
                    If Not Request.Cookies("RFDModule_SaveDesignationTypeSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveDesignationTypeSearch").Value) <> "" Then
                            ddSearchDesignationType.SelectedValue = Request.Cookies("RFDModule_SaveDesignationTypeSearch").Value
                            ViewState("DesignationType") = Request.Cookies("RFDModule_SaveDesignationTypeSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CustomerValue") <> "" Then
                    ddSearchCustomer.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("CustomerValue"))
                    ViewState("CustomerValue") = Server.UrlDecode(HttpContext.Current.Request.QueryString("CustomerValue"))
                Else
                    If Not Request.Cookies("RFDModule_SaveCustomerSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveCustomerSearch").Value) <> "" Then
                            ddSearchCustomer.SelectedValue = Request.Cookies("RFDModule_SaveCustomerSearch").Value
                            ViewState("CustomerValue") = Request.Cookies("RFDModule_SaveCustomerSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                    ddSearchUGNFacility.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("UGNFacility"))
                    ViewState("UGNFacility") = Server.UrlDecode(HttpContext.Current.Request.QueryString("UGNFacility"))
                Else
                    If Not Request.Cookies("RFDModule_SaveUGNFacilitySearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveUGNFacilitySearch").Value) <> "" Then
                            ddSearchUGNFacility.SelectedValue = Request.Cookies("RFDModule_SaveUGNFacilitySearch").Value
                            ViewState("UGNFacility") = Request.Cookies("RFDModule_SaveUGNFacilitySearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ProgramID") <> "" Then
                    ddSearchProgram.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("ProgramID"))
                    ViewState("ProgramID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("ProgramID"))
                Else
                    If Not Request.Cookies("RFDModule_SaveProgramIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveProgramIDSearch").Value) <> "" Then
                            ddSearchProgram.SelectedValue = Request.Cookies("RFDModule_SaveProgramIDSearch").Value
                            ViewState("ProgramID") = Request.Cookies("RFDModule_SaveProgramIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CommodityID") <> "" Then
                    ddSearchCommodity.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("CommodityID"))
                    ViewState("CommodityID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("CommodityID"))
                Else
                    If Not Request.Cookies("RFDModule_SaveCommodityIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveCommodityIDSearch").Value) <> "" Then
                            ddSearchCommodity.SelectedValue = Request.Cookies("RFDModule_SaveCommodityIDSearch").Value
                            ViewState("CommodityID") = Request.Cookies("RFDModule_SaveCommodityIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ProductTechnologyID") <> "" Then
                    ddSearchProductTechnology.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("ProductTechnologyID"))
                    ViewState("ProductTechnologyID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("ProductTechnologyID"))
                Else
                    If Not Request.Cookies("RFDModule_SaveProductTechnologyIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveProductTechnologyIDSearch").Value) <> "" Then
                            ddSearchProductTechnology.SelectedValue = Request.Cookies("RFDModule_SaveProductTechnologyIDSearch").Value
                            ViewState("ProductTechnologyID") = Request.Cookies("RFDModule_SaveProductTechnologyIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("SubFamilyID") <> "" Then
                    ddSearchSubFamily.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("SubFamilyID"))
                    ViewState("SubFamilyID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("SubFamilyID"))
                Else
                    If Not Request.Cookies("RFDModule_SaveSubFamilyIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveSubFamilyIDSearch").Value) <> "" Then
                            ddSearchSubFamily.SelectedValue = Request.Cookies("RFDModule_SaveSubFamilyIDSearch").Value
                            ViewState("SubFamilyID") = Request.Cookies("RFDModule_SaveSubFamilyIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("UGNDBVendorID") <> "" Then
                    ddSearchVendor.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("UGNDBVendorID"))
                    ViewState("UGNDBVendorID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("UGNDBVendorID"))
                Else
                    If Not Request.Cookies("RFDModule_SaveUGNDBVendorIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveUGNDBVendorIDSearch").Value) <> "" Then
                            ddSearchVendor.SelectedValue = Request.Cookies("RFDModule_SaveUGNDBVendorIDSearch").Value
                            ViewState("UGNDBVendorID") = Request.Cookies("RFDModule_SaveUGNDBVendorIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PurchasedGoodID") <> "" Then
                    ddSearchPurchasedGood.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("PurchasedGoodID"))
                    ViewState("PurchasedGoodID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("PurchasedGoodID"))
                Else
                    If Not Request.Cookies("RFDModule_SavePurchasedGoodIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SavePurchasedGoodIDSearch").Value) <> "" Then
                            ddSearchPurchasedGood.SelectedValue = Request.Cookies("RFDModule_SavePurchasedGoodIDSearch").Value
                            ViewState("PurchasedGoodID") = Request.Cookies("RFDModule_SavePurchasedGoodIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then
                    txtSearchCostSheetID.Text = HttpContext.Current.Request.QueryString("CostSheetID")
                    ViewState("CostSheetID") = HttpContext.Current.Request.QueryString("CostSheetID")
                Else
                    If Not Request.Cookies("RFDModule_SaveCostSheetIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveCostSheetIDSearch").Value) <> "" Then
                            txtSearchCostSheetID.Text = Request.Cookies("RFDModule_SaveCostSheetIDSearch").Value
                            ViewState("CostSheetID") = Request.Cookies("RFDModule_SaveCostSheetIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ECINo") <> "" Then
                    txtSearchECINo.Text = HttpContext.Current.Request.QueryString("ECINo")
                    ViewState("ECINo") = HttpContext.Current.Request.QueryString("ECINo")
                Else
                    If Not Request.Cookies("RFDModule_SaveECINoSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveECINoSearch").Value) <> "" Then
                            txtSearchECINo.Text = Request.Cookies("RFDModule_SaveECINoSearch").Value
                            ViewState("ECINo") = Request.Cookies("RFDModule_SaveECINoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CapExProjectNo") <> "" Then
                    txtSearchCapExProjectNo.Text = HttpContext.Current.Request.QueryString("CapExProjectNo")
                    ViewState("CapExProjectNo") = HttpContext.Current.Request.QueryString("CapExProjectNo")
                Else
                    If Not Request.Cookies("RFDModule_SaveCapExProjectNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveCapExProjectNoSearch").Value) <> "" Then
                            txtSearchCapExProjectNo.Text = Request.Cookies("RFDModule_SaveCapExProjectNoSearch").Value
                            ViewState("CapExProjectNo") = Request.Cookies("RFDModule_SaveCapExProjectNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PurchasingPONo") <> "" Then
                    txtSearchPONo.Text = HttpContext.Current.Request.QueryString("PurchasingPONo")
                    ViewState("PurchasingPONo") = HttpContext.Current.Request.QueryString("PurchasingPONo")
                Else
                    If Not Request.Cookies("RFDModule_SavePurchasingPONoSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SavePurchasingPONoSearch").Value) <> "" Then
                            txtSearchPONo.Text = Request.Cookies("RFDModule_SavePurchasingPONoSearch").Value
                            ViewState("PurchasingPONo") = Request.Cookies("RFDModule_SavePurchasingPONoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DueDateStart") <> "" Then
                    txtSearchDueDateStart.Text = HttpContext.Current.Request.QueryString("DueDateStart")
                    ViewState("DueDateStart") = HttpContext.Current.Request.QueryString("DueDateStart")
                Else
                    If Not Request.Cookies("RFDModule_SaveDueDateStartSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveDueDateStartSearch").Value) <> "" Then
                            txtSearchDueDateStart.Text = Request.Cookies("RFDModule_SaveDueDateStartSearch").Value
                            ViewState("DueDateStart") = Request.Cookies("RFDModule_SaveDueDateStartSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DueDateEnd") <> "" Then
                    txtSearchDueDateEnd.Text = HttpContext.Current.Request.QueryString("DueDateEnd")
                    ViewState("DueDateEnd") = HttpContext.Current.Request.QueryString("DueDateEnd")
                Else
                    If Not Request.Cookies("RFDModule_SaveDueDateEndSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveDueDateEndSearch").Value) <> "" Then
                            txtSearchDueDateEnd.Text = Request.Cookies("RFDModule_SaveDueDateEndSearch").Value
                            ViewState("DueDateEnd") = Request.Cookies("RFDModule_SaveDueDateEndSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("SubscriptionID") <> "" Then
                    ddSearchSubscription.SelectedValue = HttpContext.Current.Request.QueryString("SubscriptionID")
                    ViewState("SubscriptionID") = HttpContext.Current.Request.QueryString("SubscriptionID")
                Else
                    If Not Request.Cookies("RFDModule_SaveSubscriptionIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveSubscriptionIDSearch").Value) <> "" Then
                            ddSearchSubscription.SelectedValue = Request.Cookies("RFDModule_SaveSubscriptionIDSearch").Value
                            ViewState("SubscriptionID") = Request.Cookies("RFDModule_SaveSubscriptionIDSearch").Value
                        End If
                    End If
                End If

                ViewState("FilterBusinessAwarded") = 0
                ViewState("IsBusinessAwarded") = 0
                ddSearchBusinessAwarded.SelectedIndex = -1
                If HttpContext.Current.Request.QueryString("FilterBusinessAwarded") <> "" Then
                    If CType(HttpContext.Current.Request.QueryString("FilterBusinessAwarded"), Integer) = 1 Then
                        ViewState("FilterBusinessAwarded") = 1
                        ViewState("IsBusinessAwarded") = CType(HttpContext.Current.Request.QueryString("IsBusinessAwarded"), Integer)
                        ddSearchBusinessAwarded.SelectedValue = CType(HttpContext.Current.Request.QueryString("IsBusinessAwarded"), Integer)
                    End If
                Else
                    If Request.Cookies("RFDModule_SaveFilterBusinessAwarded") IsNot Nothing Then
                        If Request.Cookies("RFDModule_SaveIsBusinessAwarded") IsNot Nothing Then
                            If Request.Cookies("RFDModule_SaveFilterBusinessAwarded").Value <> "" Then
                                If CType(Request.Cookies("RFDModule_SaveFilterBusinessAwarded").Value, Integer) = 1 Then
                                    ViewState("FilterBusinessAwarded") = 1
                                    ViewState("IsBusinessAwarded") = CType(Request.Cookies("RFDModule_SaveIsBusinessAwarded").Value, Integer)
                                    ddSearchBusinessAwarded.SelectedValue = CType(Request.Cookies("RFDModule_SaveIsBusinessAwarded").Value, Integer)
                                End If
                            End If
                        End If
                    End If
                End If

                ViewState("IncludeArchive") = 0
                If HttpContext.Current.Request.QueryString("IncludeArchive") <> "" Then
                    ViewState("IncludeArchive") = CType(Server.UrlDecode(HttpContext.Current.Request.QueryString("IncludeArchive")), Integer)
                Else
                    If Not Request.Cookies("RFDModule_SaveIncludeArchiveSearch") Is Nothing Then
                        If Trim(Request.Cookies("RFDModule_SaveIncludeArchiveSearch").Value) <> "" Then
                            ViewState("IncludeArchive") = CType(Request.Cookies("RFDModule_SaveIncludeArchiveSearch").Value, Integer)
                        End If
                    End If
                End If

                cbIncludeArchive.Checked = ViewState("IncludeArchive")

                '' ''******
                'load repeater control
                '' ''******
                BindData()

                If cbIncludeArchive.Checked = True _
                    Or ViewState("PriorityID") > 0 _
                    Or ViewState("DesignationType") <> "" _
                    Or ViewState("CustomerValue") <> "" _
                    Or ViewState("UGNFacility") <> "" _
                    Or ViewState("ProgramID") > 0 _
                    Or ViewState("CommodityID") > 0 _
                    Or ViewState("ProductTechnologyID") > 0 _
                    Or ViewState("SubFamilyID") > 0 _
                    Or ViewState("AccountManagerID") > 0 _
                    Or ViewState("UGNDBVendorID") > 0 _
                    Or ViewState("PurchasedGoodID") > 0 _
                    Or ViewState("CostSheetID") <> "" _
                    Or ViewState("ECINo") <> "" _
                    Or ViewState("CapExProjectNo") <> "" _
                    Or ViewState("PurchasingPONo") <> "" _
                    Or ViewState("DueDateStart") <> "" _
                    Or ViewState("DueDateEnd") <> "" Then

                    accAdvancedSearch.SelectedIndex = 0
                    cbShowAdvancedSearch.Checked = True
                Else
                    accAdvancedSearch.SelectedIndex = -1
                    cbShowAdvancedSearch.Checked = False
                End If

            Else
                If txtSearchRFDNo.Text.Trim <> "" Then
                    ViewState("RFDNo") = txtSearchRFDNo.Text.Trim
                End If

                If txtSearchRFDDesc.Text.Trim <> "" Then
                    ViewState("RFDDesc") = txtSearchRFDDesc.Text.Trim
                End If

                If ddSearchStatus.SelectedIndex > 0 Then
                    ViewState("StatusID") = ddSearchStatus.SelectedValue
                End If

                If ddSearchApproverStatus.SelectedIndex > 0 Then
                    ViewState("ApproverStatusID") = ddSearchApproverStatus.SelectedValue
                End If

                If txtSearchDrawingNo.Text.Length > 0 Then
                    ViewState("DrawingNo") = txtSearchDrawingNo.Text.Trim
                End If

                If ddSearchPriority.SelectedIndex > 0 Then
                    ViewState("PriorityID") = ddSearchPriority.SelectedValue
                End If

                If txtSearchCustomerPartNo.Text.Trim <> "" Then
                    ViewState("CustomerPartNo") = txtSearchCustomerPartNo.Text.Trim
                End If

                If txtSearchDesignLevel.Text.Trim <> "" Then
                    ViewState("DesignLevel") = txtSearchDesignLevel.Text.Trim
                End If

                If txtSearchPartNo.Text.Trim <> "" Then
                    ViewState("PartNo") = txtSearchPartNo.Text.Trim
                End If

                If txtSearchPartName.Text.Trim <> "" Then
                    ViewState("PartName") = txtSearchPartName.Text.Trim
                End If

                If ddSearchInitiator.SelectedIndex > 0 Then
                    ViewState("InitiatorID") = ddSearchInitiator.SelectedValue
                End If

                If ddSearchApprover.SelectedIndex > 0 Then
                    ViewState("ApproverID") = ddSearchApprover.SelectedValue
                End If

                If ddSearchAccountManager.SelectedIndex > 0 Then
                    ViewState("AccountManagerID") = ddSearchAccountManager.SelectedValue
                End If

                If ddSearchProgramManager.SelectedIndex > 0 Then
                    ViewState("ProgramManagerID") = ddSearchProgramManager.SelectedValue
                End If

                If ddSearchBusinessProcessAction.SelectedIndex > 0 Then
                    ViewState("BusinessProcessActionID") = ddSearchBusinessProcessAction.SelectedValue
                End If

                If ddSearchBusinessProcessType.SelectedIndex > 0 Then
                    ViewState("BusinessProcessTypeID") = ddSearchBusinessProcessType.SelectedValue
                End If

                If ddSearchDesignationType.SelectedIndex > 0 Then
                    ViewState("DesignationType") = ddSearchDesignationType.SelectedValue
                End If

                If ddSearchCustomer.SelectedIndex > 0 Then
                    ViewState("CustomerValue") = ddSearchCustomer.SelectedValue
                End If

                If ddSearchUGNFacility.SelectedIndex > 0 Then
                    ViewState("UGNFacility") = ddSearchUGNFacility.SelectedValue
                End If

                If ddSearchProgram.SelectedIndex > 0 Then
                    ViewState("ProgramID") = ddSearchProgram.SelectedValue
                End If

                If ddSearchCommodity.SelectedIndex > 0 Then
                    ViewState("CommodityID") = ddSearchCommodity.SelectedValue
                End If

                If ddSearchProductTechnology.SelectedIndex > 0 Then
                    ViewState("ProductTechnologyID") = ddSearchProductTechnology.SelectedValue
                End If

                If ddSearchSubFamily.SelectedIndex > 0 Then
                    ViewState("SubFamilyID") = ddSearchSubFamily.SelectedValue
                End If

                If ddSearchVendor.SelectedIndex > 0 Then
                    ViewState("UGNDBVendorID") = ddSearchVendor.SelectedValue
                End If

                If ddSearchPurchasedGood.SelectedIndex > 0 Then
                    ViewState("PurchasedGoodID") = ddSearchPurchasedGood.SelectedValue
                End If

                If txtSearchCostSheetID.Text.Trim <> "" Then
                    ViewState("CostSheetID") = txtSearchCostSheetID.Text.Trim
                End If

                If txtSearchCostSheetID.Text.Trim <> "" Then
                    ViewState("ECINo") = txtSearchECINo.Text.Trim
                End If

                If txtSearchCapExProjectNo.Text.Trim <> "" Then
                    ViewState("CapExProjectNo") = txtSearchCapExProjectNo.Text.Trim
                End If

                If txtSearchPONo.Text.Trim <> "" Then
                    ViewState("PurchasingPONo") = txtSearchPONo.Text.Trim
                End If

                If txtSearchDueDateStart.Text.Trim <> "" Then
                    ViewState("DueDateStart") = txtSearchDueDateStart.Text.Trim
                End If

                If txtSearchDueDateEnd.Text.Trim <> "" Then
                    ViewState("DueDateEnd") = txtSearchDueDateEnd.Text.Trim
                End If

                If ddSearchSubscription.SelectedIndex > 0 Then
                    ViewState("SubscriptionID") = ddSearchSubscription.SelectedValue
                End If

                If cbIncludeArchive.Checked = True Then
                    ViewState("IncludeArchive") = 1
                Else
                    ViewState("IncludeArchive") = 0
                End If

                ViewState("FilterBusinessAwarded") = 0
                ViewState("IsBusinessAwarded") = 0
                If ddSearchBusinessAwarded.SelectedIndex > 0 Then
                    ViewState("FilterBusinessAwarded") = 1
                    ViewState("IsBusinessAwarded") = ddSearchBusinessAwarded.SelectedValue
                End If

                'focus on RFDNo field
                txtSearchRFDNo.Focus()
            End If

            EnableControls()

            'to trouble shoot get trace information
            'Page.Trace.IsEnabled = False

            'Try
            '    Page.Trace.IsEnabled = Request.QueryString("PAGE_DEBUG").ToUpper() = "TRUE"
            'Catch
            'End Try


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindBusinessProcessAction(ByVal filterQuoteOnly As Boolean, ByVal isQuoteOnly As Boolean)

        Try
            Dim ds As DataSet

            ds = commonFunctions.GetBusinessProcessAction(0, filterQuoteOnly, isQuoteOnly)
            If (commonFunctions.CheckDataSet(ds) = True) Then
                ddSearchBusinessProcessAction.DataSource = ds
                ddSearchBusinessProcessAction.DataTextField = ds.Tables(0).Columns("ddBusinessProcessActionName").ColumnName
                ddSearchBusinessProcessAction.DataValueField = ds.Tables(0).Columns("BusinessProcessActionID").ColumnName
                ddSearchBusinessProcessAction.DataBind()
                ddSearchBusinessProcessAction.Items.Insert(0, "")
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

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''bind existing data to drop downs for selection criteria of search
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Account Manager
            ds = commonFunctions.GetTeamMemberBySubscription(9)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchAccountManager.DataSource = ds
                ddSearchAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddSearchAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddSearchAccountManager.DataBind()
                ddSearchAccountManager.Items.Insert(0, "")
            End If

            ds = RFDModule.GetRFDApproverList()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchApprover.DataSource = ds
                ddSearchApprover.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName
                ddSearchApprover.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddSearchApprover.DataBind()
                ddSearchApprover.Items.Insert(0, "")
            End If

            BindBusinessProcessAction(False, False)

            ds = commonFunctions.GetBusinessProcessType(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchBusinessProcessType.DataSource = ds
                ddSearchBusinessProcessType.DataTextField = ds.Tables(0).Columns("ddBusinessProcessTypeName").ColumnName.ToString()
                ddSearchBusinessProcessType.DataValueField = ds.Tables(0).Columns("BusinessProcessTypeID").ColumnName
                ddSearchBusinessProcessType.DataBind()
                ddSearchBusinessProcessType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchCommodity.DataSource = ds
                ddSearchCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
                ddSearchCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddSearchCommodity.DataBind()
                ddSearchCommodity.Items.Insert(0, "")
                ddSearchCommodity.SelectedIndex = 0
            End If

            ds = commonFunctions.GetOEMManufacturer("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchCustomer.DataSource = ds
                ddSearchCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
                ddSearchCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
                ddSearchCustomer.DataBind()
                ddSearchCustomer.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetDesignationType()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchDesignationType.DataSource = ds
                ddSearchDesignationType.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName.ToString()
                ddSearchDesignationType.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName
                ddSearchDesignationType.DataBind()
                ddSearchDesignationType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchUGNFacility.DataSource = ds
                ddSearchUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddSearchUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddSearchUGNFacility.DataBind()
                ddSearchUGNFacility.Items.Insert(0, "")

                ddStatusFacility.DataSource = ds
                ddStatusFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddStatusFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddStatusFacility.DataBind()
                ddStatusFacility.Items.Insert(0, "")
            End If

            ds = RFDModule.GetRFDInitiatorList()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchInitiator.DataSource = ds
                ddSearchInitiator.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName
                ddSearchInitiator.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddSearchInitiator.DataBind()
                ddSearchInitiator.Items.Insert(0, "")
            End If

            ds = RFDModule.GetRFDPriority(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchPriority.DataSource = ds
                ddSearchPriority.DataTextField = ds.Tables(0).Columns("ddPriorityName").ColumnName
                ddSearchPriority.DataValueField = ds.Tables(0).Columns("PriorityID").ColumnName
                ddSearchPriority.DataBind()
                ddSearchPriority.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProductTechnology("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchProductTechnology.DataSource = ds
                ddSearchProductTechnology.DataTextField = ds.Tables(0).Columns("ddProductTechnologyName").ColumnName.ToString()
                ddSearchProductTechnology.DataValueField = ds.Tables(0).Columns("ProductTechnologyID").ColumnName
                ddSearchProductTechnology.DataBind()
                ddSearchProductTechnology.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProgram("", "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchProgram.DataSource = ds
                ddSearchProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                ddSearchProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName
                ddSearchProgram.DataBind()
                ddSearchProgram.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetPurchasedGood("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchPurchasedGood.DataSource = ds
                ddSearchPurchasedGood.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
                ddSearchPurchasedGood.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddSearchPurchasedGood.DataBind()
                ddSearchPurchasedGood.Items.Insert(0, "")
            End If

            ds = RFDModule.GetRFDStatus(0, False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchStatus.DataSource = ds
                ddSearchStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName
                ddSearchStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddSearchStatus.DataBind()
                ddSearchStatus.Items.Insert(0, "")
            End If

            'approver status
            ds = RFDModule.GetRFDStatus(0, True)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchApproverStatus.DataSource = ds
                ddSearchApproverStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName
                ddSearchApproverStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddSearchApproverStatus.DataBind()
                ddSearchApproverStatus.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetSubFamily(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchSubFamily.DataSource = ds
                ddSearchSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddSearchSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddSearchSubFamily.DataBind()
                ddSearchSubFamily.Items.Insert(0, "")
            End If

            ds = RFDModule.GetRFDSubscriptionByApprover(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchSubscription.DataSource = ds
                ddSearchSubscription.DataTextField = ds.Tables(0).Columns("Subscription").ColumnName
                ddSearchSubscription.DataValueField = ds.Tables(0).Columns("SubscriptionID").ColumnName
                ddSearchSubscription.DataBind()
            End If

            ds = commonFunctions.GetUGNDBVendor(0, "", "", False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchVendor.DataSource = ds
                ddSearchVendor.DataTextField = ds.Tables(0).Columns("ddVendorName").ColumnName.ToString()
                ddSearchVendor.DataValueField = ds.Tables(0).Columns("UGNDBVendorID").ColumnName
                ddSearchVendor.DataBind()
                ddSearchVendor.Items.Insert(0, "")
            End If


            'Program Manager
            ds = commonFunctions.GetTeamMemberBySubscription(31)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchProgramManager.DataSource = ds
                ddSearchProgramManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddSearchProgramManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddSearchProgramManager.DataBind()
                ddSearchProgramManager.Items.Insert(0, "")
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

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Try
            lblMessage.Text = ""

            Response.Redirect("RFD_Creation_Wizard.aspx", False)
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
            lblMessage.Text = ""

            HttpContext.Current.Session("sessionRFDCurrentPage") = Nothing

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'set saved value of what criteria was used to search   
            ''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim iRFDNo As Integer = 0

            If IsNumeric(txtSearchRFDNo.Text.Trim) Then
                iRFDNo = CType(txtSearchRFDNo.Text.Trim, Integer)
            End If

            'if RFD Number to search for is less than 200000, then it is archive data. so the checkbox will be turned on
            If (iRFDNo < 200000 Or txtSearchRFDNo.Text.Trim.Length < 6) And txtSearchRFDNo.Text.Trim <> "" Then
                cbIncludeArchive.Checked = True
                accAdvancedSearch.SelectedIndex = 0
                cbShowAdvancedSearch.Checked = True
                Response.Cookies("UGNDB_ShowRFDAdvancedSearch").Value = 1
            End If

            Response.Cookies("RFDModule_SaveRFDNoSearch").Value = txtSearchRFDNo.Text.Trim

            Response.Cookies("RFDModule_SaveRFDDescSearch").Value = txtSearchRFDDesc.Text.Trim

            If ddSearchStatus.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveStatusIDSearch").Value = ddSearchStatus.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveStatusIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveStatusIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchApproverStatus.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveApproverStatusIDSearch").Value = ddSearchApproverStatus.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveApproverStatusIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveApproverStatusIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("RFDModule_SaveDrawingNoSearch").Value = txtSearchDrawingNo.Text.Trim

            If ddSearchPriority.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SavePrioritySearch").Value = ddSearchPriority.SelectedValue
            Else
                Response.Cookies("RFDModule_SavePrioritySearch").Value = 0
                Response.Cookies("RFDModule_SavePrioritySearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("RFDModule_SaveCustomerPartNoSearch").Value = txtSearchCustomerPartNo.Text.Trim

            Response.Cookies("RFDModule_SaveDesignLevelSearch").Value = txtSearchDesignLevel.Text.Trim

            Response.Cookies("RFDModule_SavePartNoSearch").Value = txtSearchPartNo.Text.Trim

            Response.Cookies("RFDModule_SavePartNameSearch").Value = txtSearchPartName.Text.Trim

            If ddSearchInitiator.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveInitiatorIDSearch").Value = ddSearchInitiator.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveInitiatorIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveInitiatorIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchApprover.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveApproverIDSearch").Value = ddSearchApprover.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveApproverIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveApproverIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchAccountManager.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveAccountManagerIDSearch").Value = ddSearchAccountManager.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveAccountManagerIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveAccountManagerIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchProgramManager.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveProgramManagerIDSearch").Value = ddSearchProgramManager.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveProgramManagerIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveProgramManagerIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchBusinessProcessAction.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveBusinessProcessActionIDSearch").Value = ddSearchBusinessProcessAction.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveBusinessProcessActionIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveBusinessProcessActionIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchBusinessProcessType.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveBusinessProcessTypeIDSearch").Value = ddSearchBusinessProcessType.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveBusinessProcessTypeIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveBusinessProcessTypeIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchDesignationType.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveDesignationTypeSearch").Value = ddSearchDesignationType.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveDesignationTypeSearch").Value = ""
                Response.Cookies("RFDModule_SaveDesignationTypeSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchCustomer.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveCustomerSearch").Value = ddSearchCustomer.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveCustomerSearch").Value = 0
                Response.Cookies("RFDModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchUGNFacility.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveUGNFacilitySearch").Value = ddSearchUGNFacility.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveUGNFacilitySearch").Value = ""
                Response.Cookies("RFDModule_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchProgram.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveProgramIDSearch").Value = ddSearchProgram.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveProgramIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveProgramIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchCommodity.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveCommodityIDSearch").Value = ddSearchCommodity.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveCommodityIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveCommodityIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchProductTechnology.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveProductTechnologyIDSearch").Value = ddSearchProductTechnology.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveProductTechnologyIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveProductTechnologyIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchSubFamily.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveSubFamilyIDSearch").Value = ddSearchSubFamily.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveSubFamilyIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveSubFamilyIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchVendor.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveUGNDBVendorIDSearch").Value = ddSearchVendor.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveUGNDBVendorIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveUGNDBVendorIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchPurchasedGood.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SavePurchasedGoodIDSearch").Value = ddSearchPurchasedGood.SelectedValue
            Else
                Response.Cookies("RFDModule_SavePurchasedGoodIDSearch").Value = 0
                Response.Cookies("RFDModule_SavePurchasedGoodIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("RFDModule_SaveCostSheetIDSearch").Value = txtSearchCostSheetID.Text.Trim

            Response.Cookies("RFDModule_SaveECINoSearch").Value = txtSearchECINo.Text.Trim

            Response.Cookies("RFDModule_SaveCapExProjectNoSearch").Value = txtSearchCapExProjectNo.Text.Trim

            Response.Cookies("RFDModule_SavePurchasingPONoSearch").Value = txtSearchPONo.Text.Trim

            Response.Cookies("RFDModule_SaveDueDateStartSearch").Value = txtSearchDueDateStart.Text.Trim

            Response.Cookies("RFDModule_SaveDueDateEndSearch").Value = txtSearchDueDateEnd.Text.Trim

            If ddSearchSubscription.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveSubscriptionIDSearch").Value = ddSearchSubscription.SelectedValue
            Else
                Response.Cookies("RFDModule_SaveSubscriptionIDSearch").Value = 0
                Response.Cookies("RFDModule_SaveSubscriptionIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("RFDModule_SaveFilterBusinessAwarded").Value = 0
            Response.Cookies("RFDModule_SaveIsBusinessAwarded").Value = 0
            ViewState("FilterBusinessAwarded") = 0
            ViewState("IsBusinessAwarded") = 0
            If ddSearchBusinessAwarded.SelectedIndex > 0 Then
                Response.Cookies("RFDModule_SaveFilterBusinessAwarded").Value = 1
                Response.Cookies("RFDModule_SaveIsBusinessAwarded").Value = ddSearchBusinessAwarded.SelectedValue
                ViewState("FilterBusinessAwarded") = 1
                ViewState("IsBusinessAwarded") = ddSearchBusinessAwarded.SelectedValue
            End If

            If cbIncludeArchive.Checked = True Then
                Response.Cookies("RFDModule_SaveIncludeArchiveSearch").Value = 1
                ViewState("IncludeArchive") = 1
            Else
                Response.Cookies("RFDModule_SaveIncludeArchiveSearch").Value = 0
                ViewState("IncludeArchive") = 0
                Response.Cookies("RFDModule_SaveIncludeArchiveSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Redirect("RFD_List.aspx?RFDNo=" & Server.UrlEncode(txtSearchRFDNo.Text.Trim) _
            & "&StatusID=" & Server.UrlEncode(ddSearchStatus.SelectedValue) _
            & "&ApproverStatusID=" & Server.UrlEncode(ddSearchApproverStatus.SelectedValue) _
            & "&DrawingNo=" & Server.UrlEncode(txtSearchDrawingNo.Text.Trim) _
            & "&PriorityID=" & Server.UrlEncode(ddSearchPriority.SelectedValue) _
            & "&CustomerPartNo=" & Server.UrlEncode(txtSearchCustomerPartNo.Text.Trim) _
            & "&DesignLevel=" & Server.UrlEncode(txtSearchDesignLevel.Text.Trim) _
            & "&PartNo=" & Server.UrlEncode(txtSearchPartNo.Text.Trim) _
            & "&PartName=" & Server.UrlEncode(txtSearchPartName.Text.Trim) _
            & "&InitiatorTeamMemberID=" & Server.UrlEncode(ddSearchInitiator.SelectedValue) _
            & "&ApproverTeamMemberID=" & Server.UrlEncode(ddSearchApprover.SelectedValue) _
            & "&AccountManagerID=" & Server.UrlEncode(ddSearchAccountManager.SelectedValue) _
            & "&ProgramManagerID=" & Server.UrlEncode(ddSearchProgramManager.SelectedValue) _
            & "&BusinessProcessTypeID=" & Server.UrlEncode(ddSearchBusinessProcessType.SelectedValue) _
            & "&DesignationType=" & Server.UrlEncode(ddSearchDesignationType.SelectedValue) _
            & "&CustomerValue=" & Server.UrlEncode(ddSearchCustomer.SelectedValue) _
            & "&UGNFacility=" & Server.UrlEncode(ddSearchUGNFacility.SelectedValue) _
            & "&ProgramID=" & Server.UrlEncode(ddSearchProgram.SelectedValue) _
            & "&CommodityID=" & Server.UrlEncode(ddSearchCommodity.SelectedValue) _
            & "&ProductTechnologyID=" & Server.UrlEncode(ddSearchProductTechnology.SelectedValue) _
            & "&SubFamilyID=" & Server.UrlEncode(ddSearchSubFamily.SelectedValue) _
            & "&UGNDBVendorID=" & Server.UrlEncode(ddSearchVendor.SelectedValue) _
            & "&PurchasedGoodID=" & Server.UrlEncode(ddSearchPurchasedGood.SelectedValue) _
            & "&CostSheetID=" & Server.UrlEncode(txtSearchCostSheetID.Text.Trim) _
            & "&ECINo=" & Server.UrlEncode(txtSearchECINo.Text.Trim) _
            & "&CapExProjectNo=" & Server.UrlEncode(txtSearchCapExProjectNo.Text.Trim) _
            & "&PurchasingPONo=" & Server.UrlEncode(txtSearchPONo.Text.Trim) _
            & "&DueDateStart=" & Server.UrlEncode(txtSearchDueDateStart.Text.Trim) _
            & "&DueDateEnd=" & Server.UrlEncode(txtSearchDueDateEnd.Text.Trim) _
            & "&SubscriptionID=" & Server.UrlEncode(ddSearchSubscription.SelectedValue) _
            & "&FilterBusinessAwarded=" & ViewState("FilterBusinessAwarded") _
            & "&IsBusinessAwarded=" & ViewState("IsBusinessAwarded") _
            & "&IncludeArchive=" & ViewState("IncludeArchive") _
            , False)

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
            lblMessage.Text = ""

            RFDModule.DeleteRFDCookies()

            HttpContext.Current.Session("sessionRFDCurrentPage") = Nothing

            Response.Redirect("RFD_List.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cbIncludeArchive_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbIncludeArchive.CheckedChanged


        Try
            lblMessage.Text = ""

            If cbIncludeArchive.Checked = True Then
                cbShowAdvancedSearch.Checked = True
            End If

            CheckIncludeArchives()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub ShowAdvancedSearch()

        Try

            If cbShowAdvancedSearch.Checked = False Then
                Response.Cookies("UGNDB_ShowRFDAdvancedSearch").Value = 0
                accAdvancedSearch.SelectedIndex = -1
            Else
                Response.Cookies("UGNDB_ShowRFDAdvancedSearch").Value = 1
                accAdvancedSearch.SelectedIndex = 0
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
    Protected Sub cbShowAdvancedSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbShowAdvancedSearch.CheckedChanged

        Try
            lblMessage.Text = ""

            ShowAdvancedSearch()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionRFDCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionRFDCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionRFDCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click

        Try
            If txtGoToPage.Text.Length > 0 Then

                ' Set viewstate variable to the specific page
                If txtGoToPage.Text > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPage.Text - 1
                End If


                HttpContext.Current.Session("sessionRFDCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionRFDCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnStatusReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStatusReport.Click
        Try
            lblMessage.Text = ""

            ' Response.Redirect("RFD_Status_Report.aspx?SubscriptionID=" & ddStatusSubscription.SelectedValue & "&UGNFacility=" & ddStatusFacility.SelectedValue & "&FileTypeExt=" & ddFileType.SelectedValue, False)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "RFD Status Report", "window.open('RFD_Status_Report.aspx?SubscriptionID=" & ddStatusSubscription.SelectedValue & "&UGNFacility=" & ddStatusFacility.SelectedValue & "&FileTypeExt=" & ddFileType.SelectedValue & "'," & Now.Ticks & ",'top=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=yes,location=no');", True)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Protected Sub ddSearchBusinessProcessType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddSearchBusinessProcessType.SelectedIndexChanged

        Try

            If ViewState("BusinessProcessTypeID") = 1 Then
                BindBusinessProcessAction(True, False)
            End If

            If ViewState("BusinessProcessTypeID") = 7 Then
                BindBusinessProcessAction(True, True)
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
End Class
