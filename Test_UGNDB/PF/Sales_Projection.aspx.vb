' ************************************************************************************************
' Name:	Sales_Projection.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 04/04/2008    LRey			Created .Net application
' 06/15/2011    LRey            Modified to the CustomerProgram gridview controls
' 03/02/2012    LREy            Added CostSheetID reference
' 05/04/2012    LRey            Added a text field for Comments
' 06/21/2012    LRey            Added a delete function when users click on the btnCopy feature.
' 02/26/2014    LRey            Replaced Part Number drop down list to a free form text field.
' ************************************************************************************************
Partial Class PMT_Sales_Projection
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to define the primary record
            If HttpContext.Current.Request.QueryString("sPartNo") <> "" Then
                ViewState("sPartNo") = HttpContext.Current.Request.QueryString("sPartNo")
            Else
                ViewState("sPartNo") = ""
            End If

            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Sales Projection"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Planning and Forecasting </b> > <a href='Sales_Projection_List.aspx'><b>Sales Projection Search</b></a> > Sales Projection"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("PFExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False
            Dim DMPanel As CollapsiblePanelExtender = CType(Master.FindControl("DMExtender"), CollapsiblePanelExtender)
            DMPanel.Collapsed = True

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                btnDelete.Attributes.Add("onclientclick", "return confirm('Are you sure you want to delete this record?');")
                BindCriteria()
                If HttpContext.Current.Request.QueryString("sPartNo") <> "" And HttpContext.Current.Request.QueryString("sPartNo") <> Nothing Then
                    BindDataPerRecord() 'used to bind data at the record level

                Else
                    btnDelete.Visible = False
                End If

            End If

            GetCostSheetLink()

            txtComments.Attributes.Add("onkeypress", "return tbLimit();")
            txtComments.Attributes.Add("onkeyup", "return tbCount(" + lblComments.ClientID + ");")
            txtComments.Attributes.Add("maxLength", "300")


        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name


            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********

        Try

            ''*******
            '' Disable controls by default
            ''*******
            btnAdd.Enabled = False
            btnSave.Enabled = False
            btnReset.Enabled = False
            btnDelete.Enabled = False
            ViewState("ObjectRole") = False
            gvPrice.Enabled = False
            gvCustomerProgram.Enabled = False
            gvPrice.Visible = False
            gvCustomerProgram.Visible = False
            accCustomerProgram.Visible = False
            accPrice.Visible = False

            If ViewState("sPartNo") = "" Then
                txtPartNo.Enabled = True
            Else
                txtPartNo.Enabled = False
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 13 'Vehicle form id
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)

                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                                Select Case iRoleID
                                    Case 11 '*** UGNAdmin: Full Access
                                        btnAdd.Enabled = True
                                        btnSave.Enabled = True
                                        btnReset.Enabled = True
                                        btnDelete.Enabled = True
                                        ViewState("ObjectRole") = True
                                        If ViewState("sPartNo") <> "" Then
                                            gvPrice.Enabled = True
                                            gvCustomerProgram.Enabled = True
                                            gvPrice.Columns(3).Visible = True
                                            gvCustomerProgram.Columns(6).Visible = True
                                            gvPrice.Visible = True
                                            gvCustomerProgram.Visible = True
                                            accCustomerProgram.Visible = True
                                            accPrice.Visible = True
                                        End If
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        btnAdd.Enabled = True
                                        btnSave.Enabled = True
                                        btnReset.Enabled = True
                                        btnDelete.Enabled = True
                                        ViewState("ObjectRole") = True
                                        If ViewState("sPartNo") <> "" Then
                                            gvPrice.Enabled = True
                                            gvCustomerProgram.Enabled = True
                                            gvPrice.Columns(3).Visible = True
                                            gvCustomerProgram.Columns(6).Visible = True
                                            gvPrice.Visible = True
                                            gvCustomerProgram.Visible = True
                                            accCustomerProgram.Visible = True
                                            accPrice.Visible = True
                                        End If
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        btnAdd.Enabled = True
                                        btnSave.Enabled = True
                                        btnReset.Enabled = True
                                        btnDelete.Enabled = False
                                        ViewState("ObjectRole") = False
                                        If ViewState("sPartNo") <> "" Then
                                            gvPrice.Enabled = True
                                            gvCustomerProgram.Enabled = True
                                            gvPrice.Columns(3).Visible = True
                                            gvCustomerProgram.Columns(6).Visible = True
                                            gvPrice.Visible = True
                                            gvCustomerProgram.Visible = True
                                            accCustomerProgram.Visible = True
                                            accPrice.Visible = True
                                        End If
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        btnAdd.Enabled = False
                                        btnSave.Enabled = False
                                        btnReset.Enabled = False
                                        btnDelete.Enabled = False
                                        ViewState("ObjectRole") = False
                                        If ViewState("sPartNo") <> "" Then
                                            gvPrice.Enabled = False
                                            gvCustomerProgram.Enabled = False
                                            gvPrice.Columns(3).Visible = False
                                            gvCustomerProgram.Columns(6).Visible = False
                                            gvPrice.Visible = True
                                            gvCustomerProgram.Visible = True
                                            accCustomerProgram.Visible = True
                                            accPrice.Visible = True
                                        End If
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        btnAdd.Enabled = False
                                        btnSave.Enabled = True
                                        btnReset.Enabled = True
                                        btnDelete.Enabled = False
                                        ViewState("ObjectRole") = False
                                        If ViewState("sPartNo") <> "" Then
                                            gvPrice.Enabled = True
                                            gvCustomerProgram.Enabled = True
                                            gvPrice.Columns(3).Visible = True
                                            gvCustomerProgram.Columns(6).Visible = True
                                            gvPrice.Visible = True
                                            gvCustomerProgram.Visible = True
                                            accCustomerProgram.Visible = True
                                            accPrice.Visible = True
                                        End If
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        btnAdd.Enabled = False
                                        btnSave.Enabled = False
                                        btnReset.Enabled = False
                                        btnDelete.Enabled = False
                                        ViewState("ObjectRole") = False
                                        If ViewState("sPartNo") <> "" Then
                                            gvPrice.Enabled = False
                                            gvCustomerProgram.Enabled = False
                                            gvPrice.Columns(3).Visible = False
                                            gvCustomerProgram.Columns(6).Visible = False
                                            gvPrice.Visible = True
                                            gvCustomerProgram.Visible = True
                                            accCustomerProgram.Visible = True
                                            accPrice.Visible = True
                                        End If
                                End Select 'EOF of "Select Case iRoleID"
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
#End Region 'EOF Form Level Security

    Protected Sub BindCriteria()
        Try

            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Part Number and Key Part Indicator control for selection criteria
            ' ''ds = commonFunctions.GetBPCSPartNo("", "FINISHED GOODS")
            ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
            ' ''    ddPartNo.DataSource = ds
            ' ''    ddPartNo.DataTextField = ds.Tables(0).Columns("PartNo").ColumnName.ToString()
            ' ''    ddPartNo.DataValueField = ds.Tables(0).Columns("BPCSPartNo").ColumnName.ToString()
            ' ''    ddPartNo.DataBind()
            ' ''    ddPartNo.Items.Insert(0, "")

            ' ''    ddKeyPartIndicator.DataSource = ds
            ' ''    ddKeyPartIndicator.DataTextField = ds.Tables(0).Columns("PartNo").ColumnName.ToString()
            ' ''    ddKeyPartIndicator.DataValueField = ds.Tables(0).Columns("BPCSPartNo").ColumnName.ToString()
            ' ''    ddKeyPartIndicator.DataBind()
            ' ''    ddKeyPartIndicator.Items.Insert(0, "")
            ' ''End If

            ''bind existing data to drop down Commodity control for selection criteria 
            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddCommodity.DataSource = ds
                ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
                ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddCommodity.DataBind()
                ddCommodity.Items.Insert(0, "")
                ddCommodity.SelectedIndex = 0
            End If

            ''bind existing data to drop down Product Technology control for selection criteria
            ds = commonFunctions.GetProductTechnology("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddProductTechnology.DataSource = ds
                ddProductTechnology.DataTextField = ds.Tables(0).Columns("ddProductTechnologyName").ColumnName.ToString()
                ddProductTechnology.DataValueField = ds.Tables(0).Columns("ProductTechnologyID").ColumnName
                ddProductTechnology.DataBind()
                ddProductTechnology.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Royalty control for selection criteria
            ds = commonFunctions.GetRoyalty("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddRoyalty.DataSource = ds
                ddRoyalty.DataTextField = ds.Tables(0).Columns("ddRoyaltyName").ColumnName.ToString()
                ddRoyalty.DataValueField = ds.Tables(0).Columns("RoyaltyID").ColumnName
                ddRoyalty.DataBind()
                ddRoyalty.Items.Insert(0, "")
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub 'EOF BindCriteria

    Protected Sub BindDataPerRecord()
        Try
            ''*************************************************
            ''following code used to bind data at the record level
            ''*************************************************
            Dim ds As DataSet = New DataSet
            Dim PartNo As String = HttpContext.Current.Request.QueryString("sPartNo")

            ds = PFModule.GetProjectedSales(PartNo)

            If (ds.Tables.Item(0).Rows.Count > 0) Then
                txtPartNo.Text = ds.Tables(0).Rows(0).Item("PartNo").ToString()
                ''lblPartDescription.Text = ds.Tables(0).Rows(0).Item("BPCSPartName").ToString()
                txtKeyPartIndicator.Text = ds.Tables(0).Rows(0).Item("KeyPartIndicator").ToString()
                ddCommodity.SelectedValue = ds.Tables(0).Rows(0).Item("CommodityID").ToString()
                ddProductTechnology.SelectedValue = ds.Tables(0).Rows(0).Item("ProductTechnologyID").ToString()
                ddRoyalty.SelectedValue = ds.Tables(0).Rows(0).Item("RoyaltyID").ToString()
                txtCostSheetID.Text = ds.Tables(0).Rows(0).Item("CostSheetID").ToString()
                txtComments.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                'lblKeyPartIndProgramName.Text = ds.Tables(0).Rows(0).Item("KeyPartIndProgramName").ToString()
                'lblPartProgramName.Text = ds.Tables(0).Rows(0).Item("BPCSPartProgramName").ToString()
                'lblPartDesigTypeName.Text = ds.Tables(0).Rows(0).Item("BPCSPartDesignationType").ToString()
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred with data bind.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = "True"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF BindDataPerRecord

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try

            Dim PartNo As String = HttpContext.Current.Request.QueryString("sPartNo")

            If PartNo = Nothing Then
                Response.Redirect("Sales_Projection.aspx", False)
            Else
                Response.Redirect("Sales_Projection.aspx?sPartNo=" & txtPartNo.Text, False)
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF btnReset_Click

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim OriginalPartNo As String = HttpContext.Current.Request.QueryString("sPartNo")
            lblErrors.Text = Nothing
            lblErrors.Visible = False

            Dim iCostSheetID As Integer = 0
            If (txtCostSheetID.Text <> Nothing Or txtCostSheetID.Text <> "") Then
                iCostSheetID = CType(txtCostSheetID.Text.Trim, Integer)
                Dim ds As DataSet = New DataSet
                ds = CostingModule.GetCostSheet(iCostSheetID)
                If commonFunctions.CheckDataSet(ds) = False Then
                    lblErrors.Text &= "The Cost Sheet ID " & iCostSheetID & " does not exist in UGNDB."
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12
                    iCostSheetID = Nothing
                    'Exit Sub
                End If
            End If
            If iCostSheetID = Nothing Then
                iCostSheetID = 0
            End If

            If OriginalPartNo <> Nothing Then
                '*****
                '* Update Record
                '*****
                PFModule.UpdateProjectedSales(txtPartNo.Text, txtKeyPartIndicator.Text, ddCommodity.SelectedValue, ddProductTechnology.SelectedValue, OriginalPartNo, ddRoyalty.SelectedValue, iCostSheetID, txtComments.Text)

                ''*********************************************
                ''Load all values into their designated fields
                ''*********************************************
                BindDataPerRecord()
                ' Response.Redirect("Sales_Projection.aspx?sPartNo=" & ddPartNo.SelectedValue, False)

            Else 'EOF of Update

                '*****
                '* Insert Record to Primary Table
                '*****
                PFModule.InsertProjectedSales(txtPartNo.Text, txtKeyPartIndicator.Text, ddCommodity.SelectedValue, ddProductTechnology.SelectedValue, ddRoyalty.SelectedValue, iCostSheetID, txtComments.Text)

                Response.Redirect("Sales_Projection.aspx?sPartNo=" & txtPartNo.Text, False)

            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during save record.  Please contact the IS Application Group. " & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = "True"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
        ''End If
    End Sub 'EOF btnSave_Click

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            '*****
            '* Delete Record
            '*****
            PFModule.DeleteProjectedSales(txtPartNo.Text)

            Response.Redirect("Sales_Projection_List.aspx", False)
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during delete record.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name
            lblErrors.Visible = "True"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnDelete_Click

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("Sales_Projection.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        ''************************
        ''* In the event that the user toggles between links in the UGNDB and decides to return
        ''* to prevent part numbers to be collected for copy, a delete will be executed requiring the user
        ''* select the list of part number(s) for copy
        ''************************
        PFModule.DeleteProjectedSalesCopy("", "")

        Response.Redirect("Copy_Sales_Projection.aspx?sPartNo=" & txtPartNo.Text, False)
    End Sub 'EOF btnCopy_Click

#Region "Price GridView Controls"
    Protected Sub gvPrice_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPrice.RowCommand
        Try
            ''***
            ''This section allows the inserting of a new row when save button is clicked from the footer.
            ''***
            If e.CommandName = "Insert" Then
                ''Insert data
                Dim Price As TextBox
                Dim EffDate As TextBox
                Dim CostDown As TextBox

                If gvPrice.Rows.Count = 0 Then
                    '' We are inserting through the DetailsView in the EmptyDataTemplate
                    Return
                End If

                '' Only perform the following logic when inserting through the footer
                Price = CType(gvPrice.FooterRow.FindControl("txtPrice"), TextBox)
                odsPrice.InsertParameters("Price").DefaultValue = Price.Text

                EffDate = CType(gvPrice.FooterRow.FindControl("txtEffDate"), TextBox)
                odsPrice.InsertParameters("EffDate").DefaultValue = EffDate.Text

                CostDown = CType(gvPrice.FooterRow.FindControl("txtCostDown"), TextBox)
                odsPrice.InsertParameters("CostDown").DefaultValue = CostDown.Text


                odsPrice.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvPrice.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvPrice.ShowFooter = True
                Else
                    gvPrice.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                Dim Price As TextBox
                Dim EffDate As TextBox
                Dim CostDown As TextBox
                Price = CType(gvPrice.FooterRow.FindControl("txtPrice"), TextBox)
                Price.Text = "0.0000"
                EffDate = CType(gvPrice.FooterRow.FindControl("txtEffDate"), TextBox)
                EffDate.Text = Nothing
                CostDown = CType(gvPrice.FooterRow.FindControl("txtCostDown"), TextBox)
                CostDown.Text = "0.0000"
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub 'EOF gvPrice_RowCommand

    Protected Sub gvPrice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPrice.RowDataBound
        Try
            ''***
            ''This section provides the user with the popup for confirming the delete of a record.
            ''Called by the onClientClick event.
            ''***
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' reference the Delete ImageButton
                Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
                If imgBtn IsNot Nothing Then
                    Dim db As ImageButton = CType(e.Row.Cells(2).Controls(3), ImageButton)

                    ' Get information about the product bound to the row
                    If db.CommandName = "Delete" Then
                        Dim price As Projected_Sales.Projected_Sales_PriceRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Projected_Sales.Projected_Sales_PriceRow)

                        db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this PRICE (" & DataBinder.Eval(e.Row.DataItem, "Price") & ")?');")
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF gvPrice_RowDataBound
#End Region 'EOF "Price GridView Controls"

#Region "Insert Empty Price GridView Work-Around"

    Private Property LoadDataEmpty_Price() As Boolean
        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Price") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Price"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Price") = value
        End Set
    End Property

    Protected Sub odsPrice_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsPrice.Selected
        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)
        Dim dt As Projected_Sales.Projected_Sales_PriceDataTable = CType(e.ReturnValue, Projected_Sales.Projected_Sales_PriceDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty_Price = True
        Else
            LoadDataEmpty_Price = False
        End If
    End Sub 'EOF odsPrice_Selected

    Protected Sub gvPrice_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPrice.RowCreated
        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Price
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If

    End Sub 'EOF gvPrice_RowCreated
#End Region ' Insert Empty GridView Work-Around

#Region "CustomerProgram GridView Controls"

    Protected Sub gvCustomerProgram_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try
            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If e.CommandName = "Insert" Then
                ''Insert data
                Dim Customer As DropDownList
                Dim Program As DropDownList
                Dim ProgramStatus As DropDownList
                Dim Facility As DropDownList
                Dim PiecesPerVehicle As TextBox
                Dim UsageFactorPerVehicle As TextBox

                If gvCustomerProgram.Rows.Count = 0 Then
                    '' We are inserting through the DetailsView in the EmptyDataTemplate
                    Return
                End If

                '' Only perform the following logic when inserting through the footer

                Customer = CType(gvCustomerProgram.FooterRow.FindControl("ddCustomer"), DropDownList)
                Dim Pos As Integer = InStr(Customer.SelectedValue, "|")
                Dim tempCABBV As String = Nothing
                Dim tempSoldTo As Integer = Nothing
                If Not (Pos = 0) Then
                    tempCABBV = Microsoft.VisualBasic.Right(Customer.SelectedValue, Len(Customer.SelectedValue) - Pos)
                    tempSoldTo = Microsoft.VisualBasic.Left(Customer.SelectedValue, Pos - 1)
                End If

                ' ''odsProjectedSalesCustomerProgram.InsertParameters("CABBV").DefaultValue = Customer.SelectedValue
                odsProjectedSalesCustomerProgram.InsertParameters("CABBV").DefaultValue = tempCABBV
                odsProjectedSalesCustomerProgram.InsertParameters("SoldTo").DefaultValue = tempSoldTo

                Program = CType(gvCustomerProgram.FooterRow.FindControl("ddProgram"), DropDownList)
                odsProjectedSalesCustomerProgram.InsertParameters("ProgramID").DefaultValue = Program.SelectedValue

                ProgramStatus = CType(gvCustomerProgram.FooterRow.FindControl("ddProgramStatus"), DropDownList)
                odsProjectedSalesCustomerProgram.InsertParameters("ProgramStatus").DefaultValue = ProgramStatus.SelectedValue

                Facility = CType(gvCustomerProgram.FooterRow.FindControl("ddUGNFacility"), DropDownList)
                odsProjectedSalesCustomerProgram.InsertParameters("UGNFacility").DefaultValue = Facility.SelectedValue

                PiecesPerVehicle = CType(gvCustomerProgram.FooterRow.FindControl("txtPiecesPerVehicle"), TextBox)
                odsProjectedSalesCustomerProgram.InsertParameters("PiecesPerVehicle").DefaultValue = PiecesPerVehicle.Text

                UsageFactorPerVehicle = CType(gvCustomerProgram.FooterRow.FindControl("txtUsageFactorPerVehicle"), TextBox)
                odsProjectedSalesCustomerProgram.InsertParameters("UsageFactorPerVehicle").DefaultValue = UsageFactorPerVehicle.Text

                odsProjectedSalesCustomerProgram.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvCustomerProgram.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvCustomerProgram.ShowFooter = True
                Else
                    gvCustomerProgram.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                Dim Customer As DropDownList
                Dim Program As DropDownList
                Dim ProgramStatus As DropDownList
                Dim Facility As DropDownList
                Dim PiecesPerVehicle As TextBox
                Dim UsageFactorPerVehicle As TextBox

                Customer = CType(gvCustomerProgram.FooterRow.FindControl("ddCustomer"), DropDownList)
                Customer.ClearSelection()
                Customer.Items.Add("")
                Customer.SelectedValue = ""

                Program = CType(gvCustomerProgram.FooterRow.FindControl("ddProgram"), DropDownList)
                Program.ClearSelection()
                Program.Items.Add("")
                Program.SelectedValue = ""

                ProgramStatus = CType(gvCustomerProgram.FooterRow.FindControl("ddProgramStatus"), DropDownList)
                ProgramStatus.SelectedValue = Nothing

                Facility = CType(gvCustomerProgram.FooterRow.FindControl("ddUGNFacility"), DropDownList)
                Facility.SelectedValue = Nothing

                PiecesPerVehicle = CType(gvCustomerProgram.FooterRow.FindControl("txtPiecesPerVehicle"), TextBox)
                PiecesPerVehicle.Text = Nothing

                UsageFactorPerVehicle = CType(gvCustomerProgram.FooterRow.FindControl("txtUsageFactorPerVehicle"), TextBox)
                UsageFactorPerVehicle.Text = Nothing

                Session("ddCustomer") = ""
                Session("ddProgram") = 0
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF gvCustomerProgram_RowCommand

    Protected Sub gvCustomerProgram_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvCustomerProgram.RowDataBound
        Try
            ''***
            ''This section provides the user with the popup for confirming the delete of a record.
            ''Called by the onClientClick event.
            ''***
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' ' reference the Delete ImageButtone
                Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
                If imgBtn IsNot Nothing Then
                    Dim db As ImageButton = CType(e.Row.Cells(6).Controls(3), ImageButton)

                    ' Get information about the product bound to the row
                    If db.CommandName = "Delete" Then
                        Dim price As Projected_Sales.Projected_Sales_Customer_ProgramRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Projected_Sales.Projected_Sales_Customer_ProgramRow)

                        db.OnClientClick = String.Format("return confirm('Are you certain you want to delete CUSTOMER (" & DataBinder.Eval(e.Row.DataItem, "ddCustomerValue") & ")  PROGRAM (" & DataBinder.Eval(e.Row.DataItem, "ProgramName") & ")?');")

                    End If
                End If
            End If

            ''******************
            ''Below statement is used for Cascading Drop-Down list in a gridview.
            ''******************
            If (e.Row.RowState And DataControlRowState.Edit) = DataControlRowState.Edit Then
                Dim dv As System.Data.DataRowView = e.Row.DataItem

                'Preselect correct value in Customer list
                Dim listCustomers As DropDownList = e.Row.FindControl("ddCustomer")
                listCustomers.SelectedValue = dv("CABBV")
                Dim Pos As Integer = InStr(listCustomers.SelectedValue, "|")
                Dim tempCABBV As String = Nothing
                Dim tempSoldTo As Integer = Nothing
                If Not (Pos = 0) Then
                    tempCABBV = Microsoft.VisualBasic.Right(listCustomers.SelectedValue, Len(listCustomers.SelectedValue) - Pos)
                    tempSoldTo = Microsoft.VisualBasic.Left(listCustomers.SelectedValue, Pos - 1)
                End If

                'Databind list of Program in dependent drop-down list
                Dim listPrograms As DropDownList = e.Row.FindControl("ddProgram")
                Dim dsp As SqlDataSource = e.Row.FindControl("sdsProgram_by_CABBV")
                Dim value As ParameterCollection
                value = dsp.SelectParameters
                dsp.SelectParameters("ProgramID").DefaultValue = 0
                dsp.SelectParameters("CABBV").DefaultValue = tempCABBV
                dsp.SelectParameters("SoldTo").DefaultValue = tempSoldTo
                dsp.SelectParameters("NewEntry").DefaultValue = False

                listPrograms.ClearSelection()
                listPrograms.DataBind()
                listPrograms.SelectedValue = dv("ProgramID")
                If listPrograms.SelectedItem.Text.Substring(0, 2) = "**" Then

                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF gvCustomerProgram_RowDataBound

    Protected Sub ddCustomer_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ''*******
            '' This event is used to bind data to the Program drop down list based on Customer selection
            '' from the Edittemplate in the grid view.
            ''*******
            Dim listCABBV As DropDownList
            Dim listProgram As DropDownList
            Dim sdsProgram_by_CABBV As SqlDataSource
            Dim currentRowInEdit As Integer = gvCustomerProgram.EditIndex

            listCABBV = CType(sender, DropDownList)
            Dim Pos As Integer = InStr(listCABBV.SelectedValue, "|")
            Dim tempCABBV As String = Nothing
            Dim tempSoldTo As Integer = Nothing
            If Not (Pos = 0) Then
                tempCABBV = Microsoft.VisualBasic.Right(listCABBV.SelectedValue, Len(listCABBV.SelectedValue) - Pos)
                tempSoldTo = Microsoft.VisualBasic.Left(listCABBV.SelectedValue, Pos - 1)
            End If

            listProgram = CType(gvCustomerProgram.Rows(currentRowInEdit).FindControl("ddProgram"), DropDownList)

            ''Bind data to Program drop-down list.
            sdsProgram_by_CABBV = CType(gvCustomerProgram.Rows(currentRowInEdit).FindControl("sdsProgram_by_CABBV"), SqlDataSource)
            sdsProgram_by_CABBV.SelectParameters("ProgramID").DefaultValue = 0
            sdsProgram_by_CABBV.SelectParameters("CABBV").DefaultValue = tempCABBV
            sdsProgram_by_CABBV.SelectParameters("SoldTo").DefaultValue = tempSoldTo
            sdsProgram_by_CABBV.SelectParameters("NewEntry").DefaultValue = False
            listProgram.ClearSelection()
            listProgram.DataBind()
            listProgram.Items.Add("")
            listProgram.SelectedValue = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF ddCustomer_SelectedIndexChanged1

    Protected Sub gvCustomerProgram_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvCustomerProgram.RowUpdating
        Try
            ''*******
            '' This event is used to get the new values from Customer and Program drop down lists
            '' from the EditTemplate in the grid view.
            ''*******

            Dim listCABBV As DropDownList = CType(gvCustomerProgram.Rows(e.RowIndex).FindControl("ddCustomer"), DropDownList)
            Dim listProgram As DropDownList = CType(gvCustomerProgram.Rows(e.RowIndex).FindControl("ddProgram"), DropDownList)

            Dim Pos As Integer = InStr(listCABBV.SelectedValue, "|")
            Dim tempCABBV As String = Nothing
            Dim tempSoldTo As Integer = Nothing
            If Not (Pos = 0) Then
                tempCABBV = Microsoft.VisualBasic.Right(listCABBV.SelectedValue, Len(listCABBV.SelectedValue) - Pos)
                tempSoldTo = Microsoft.VisualBasic.Left(listCABBV.SelectedValue, Pos - 1)
            End If

            e.NewValues("CABBV") = tempCABBV
            e.NewValues("SoldTo") = tempSoldTo
            e.NewValues("ProgramID") = listProgram.SelectedValue

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF gvCustomerProgram_RowUpdating

    Protected Sub ddCustomer_Footer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ''*******
            '' This event is used to bind data to the Program drop down list based on Customer selection
            '' from the FooterTemplate in the grid view.
            ''*******

            Session("ddCustomer") = Nothing
            Session("ddProgram") = Nothing

            Dim listCABBV As DropDownList
            Dim listProgram As DropDownList
            Dim sdsProgram_by_CABBV As SqlDataSource
            Dim currentRowInEdit As Integer = gvCustomerProgram.EditIndex

            listCABBV = CType(sender, DropDownList)
            Dim Pos As Integer = InStr(listCABBV.SelectedValue, "|")
            Dim tempCABBV As String = Nothing
            Dim tempSoldTo As Integer = Nothing
            If Not (Pos = 0) Then
                tempCABBV = Microsoft.VisualBasic.Right(listCABBV.SelectedValue, Len(listCABBV.SelectedValue) - Pos)
                tempSoldTo = Microsoft.VisualBasic.Left(listCABBV.SelectedValue, Pos - 1)
            End If


            listProgram = CType(gvCustomerProgram.FooterRow.FindControl("ddProgram"), DropDownList)

            ''Bind data to Program drop-down list.
            sdsProgram_by_CABBV = CType(gvCustomerProgram.FooterRow.FindControl("sdsProgram_by_CABBV"), SqlDataSource)
            sdsProgram_by_CABBV.SelectParameters("ProgramID").DefaultValue = 0
            sdsProgram_by_CABBV.SelectParameters("CABBV").DefaultValue = tempCABBV
            sdsProgram_by_CABBV.SelectParameters("SoldTo").DefaultValue = tempSoldTo
            sdsProgram_by_CABBV.SelectParameters("NewEntry").DefaultValue = True
            listProgram.ClearSelection()
            listProgram.DataBind()
            listProgram.Items.Add("")
            listProgram.SelectedValue = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF ddCustomer_Footer_SelectedIndexChanged

#End Region 'EOF "CustomerProgram GridView Controls"

#Region "Insert Empty Price GridView Work-Around"
    Private Property LoadDataEmpty_CustomerProgram() As Boolean
        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CustomerProgram") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CustomerProgram"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CustomerProgram") = value
        End Set
    End Property 'EOF LoadDataEmpty_CustomerProgram

    Protected Sub odsProjectedSalesCustomerProgram_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsProjectedSalesCustomerProgram.Selected

        Dim PartNo As String = HttpContext.Current.Request.QueryString("sPartNo")
        '' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        'Dim ds As DataSet = CType(e.ReturnValue, DataSet)
        'Dim dt As DataTable = ds.Tables(0)
        Dim dt As Projected_Sales.Projected_Sales_Customer_ProgramDataTable = CType(e.ReturnValue, Projected_Sales.Projected_Sales_Customer_ProgramDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty_CustomerProgram = True
        Else
            LoadDataEmpty_CustomerProgram = False
        End If

    End Sub

    Protected Sub gvCustomerProgram_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCustomerProgram.RowCreated

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CustomerProgram
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If
    End Sub

#End Region 'EOF "Insert Empty CustomerProgram GridView Work-Around"

    Private Sub GetCostSheetLink()

        Try
            Dim dsCosting As DataSet
            Dim iCostSheetID As Integer = 0

            hlnkNewCostSheetID.NavigateUrl = ""
            hlnkNewCostSheetID.Visible = False

            If txtCostSheetID.Text.Trim <> "" Then
                iCostSheetID = CType(txtCostSheetID.Text.Trim, Integer)

                If iCostSheetID > 0 Then
                    dsCosting = CostingModule.GetCostSheet(iCostSheetID)

                    If commonFunctions.CheckDataSet(dsCosting) = True Then
                        hlnkNewCostSheetID.NavigateUrl = "~/Costing/Cost_Sheet_Preview.aspx?CostSheetID=" & iCostSheetID.ToString
                        hlnkNewCostSheetID.Visible = True
                        hlnkNewCostSheetID.Target = "_blank"
                    End If
                End If
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

End Class
