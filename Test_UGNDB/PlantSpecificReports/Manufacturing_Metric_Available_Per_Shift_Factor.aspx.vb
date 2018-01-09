' ************************************************************************************************
'
' Name:		Manufacturing_Metric_Available_Per_Shift_Factor.aspx
' Purpose:	This Code Behind is to maintain the Manufacturing_Metric_Available_Per_Shift_Factor list used by the Plant Specific Report Module
'
' Date		Author	    
' 07/15/2010 	Roderick Carlson
' 01/21/2011    Roderick Carlson - By Department

' ************************************************************************************************
Partial Class Manufacturing_Metric_Available_Per_Shift_Factor
    Inherits System.Web.UI.Page

    Private Sub BindCriteria()

        Try
            ''bind existing data to drop down controls for selection criteria for search       

            Dim ds As DataSet

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")           
            End If

            BindDepartment("")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindDepartment(ByVal UGNFacility As String)

        Try
            Dim ds As DataSet

            ds = PSRModule.GetManufacturingMetricDepartment(UGNFacility)
            If commonFunctions.CheckDataset(ds) = True Then
                ddDepartment.DataSource = ds
                ddDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentDesc").ColumnName.ToString()
                ddDepartment.DataValueField = ds.Tables(0).Columns("CDEPT").ColumnName
                ddDepartment.DataBind()
                ddDepartment.Items.Insert(0, "")
            Else
                ddDepartment.Items.Clear()
                ddDepartment.Items.Insert(0, "")
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Manufacturing Metrics Available Per Shift Factor Maintenance"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Manufacturing - Plant Specific Reports </b> > <a href='Manufacturing_Metric_List.aspx'><b> Monthly Manufacturing Monthly Metric List </b></a> > Manufacturing Metric Available Per Shift Factor Maintenance"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            '*****
            'Expand menu item
            '*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("MPRExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            ViewState("SubscriptionID") = 0
            ViewState("isAdmin") = False
            ViewState("TeamMemberID") = 0

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0


            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataset(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                'If iTeamMemberID = 530 Then
                '    iTeamMemberID = 171 ' Greg Hall
                '    'iTeamMemberID = 582 ' Bill Schultz
                '    'iTeamMemberID = 655 ' Roger Depperschmidt 
                '    'iTeamMemberID = 688 ' Tony Ugone
                'End If

                ViewState("TeamMemberID") = iTeamMemberID

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 108)

                If commonFunctions.CheckDataset(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                        Case 13 '*** UGNAssist: Create/Edit/No Delete

                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete

                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
                End If

            End If

            ' ''test developer as another team member
            'If ViewState("TeamMemberID") = 530 Then                
            '    ViewState("TeamMemberID") = 246
            '    ViewState("SubscriptionID") = 9
            '    ViewState("isAdmin") = True
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub EnableControls()

        Try
            gvAvailablePerShiftFactor.Columns(gvAvailablePerShiftFactor.Columns.Count - 1).Visible = ViewState("isAdmin")
            If gvAvailablePerShiftFactor.FooterRow IsNot Nothing Then
                gvAvailablePerShiftFactor.FooterRow.Visible = ViewState("isAdmin")
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            PSRModule.CleanPSRMMCrystalReports()

            If Not Page.IsPostBack Then

                CheckRights()

                BindCriteria()

                If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                    ddUGNFacility.SelectedValue = HttpContext.Current.Request.QueryString("UGNFacility")
                End If

                If HttpContext.Current.Request.QueryString("DeptID") <> "" Then
                    ddDepartment.SelectedValue = HttpContext.Current.Request.QueryString("DeptID")
                End If

            End If

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try

            PSRModule.DeletePSRMMCookies()

            Response.Redirect("Manufacturing_Metric_Available_Per_Shift_Factor.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvAvailablePerShiftFactor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAvailablePerShiftFactor.RowCommand

        Try

            Dim ddTempUGNFacility As DropDownList
            Dim ddTempDepartment As DropDownList
            Dim txtTempAvailablerPerShiftFactor As TextBox
            Dim txtTempEffectiveDate As TextBox

            Dim strUGNFacility As String = ""
            Dim iDeptID As Integer = 0
            Dim dAvailablePerShiftFactor As Double = 0

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddTempUGNFacility = CType(gvAvailablePerShiftFactor.FooterRow.FindControl("ddFooterUGNFacility"), DropDownList)
                ddTempDepartment = CType(gvAvailablePerShiftFactor.FooterRow.FindControl("ddFooterDepartment"), DropDownList)
                txtTempAvailablerPerShiftFactor = CType(gvAvailablePerShiftFactor.FooterRow.FindControl("txtFooterAvailablePerShiftFactor"), TextBox)
                txtTempEffectiveDate = CType(gvAvailablePerShiftFactor.FooterRow.FindControl("txtFooterEffectiveDate"), TextBox)

                If txtTempAvailablerPerShiftFactor.Text.Trim <> "" Then
                    dAvailablePerShiftFactor = CType(txtTempAvailablerPerShiftFactor.Text.Trim, Double)
                End If

                If ddTempUGNFacility.SelectedIndex > 0 Then
                    strUGNFacility = ddTempUGNFacility.SelectedValue
                Else
                    strUGNFacility = ddTempUGNFacility.Items(0).Value
                End If

                If ddTempDepartment.SelectedIndex > 0 Then
                    iDeptID = ddTempDepartment.SelectedValue
                Else
                    iDeptID = ddTempDepartment.Items(0).Value
                End If

                If dAvailablePerShiftFactor > 0 Then
                    odsAvailablePerShiftFactor.InsertParameters("UGNFacility").DefaultValue = strUGNFacility
                    odsAvailablePerShiftFactor.InsertParameters("DeptID").DefaultValue = iDeptID
                    odsAvailablePerShiftFactor.InsertParameters("AvailablePerShiftFactor").DefaultValue = dAvailablePerShiftFactor
                    odsAvailablePerShiftFactor.InsertParameters("EffectiveDate").DefaultValue = txtTempEffectiveDate.Text.Trim

                    intRowsAffected = odsAvailablePerShiftFactor.Insert()
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvAvailablePerShiftFactor.ShowFooter = False
            Else
                gvAvailablePerShiftFactor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddTempUGNFacility = CType(gvAvailablePerShiftFactor.FooterRow.FindControl("ddFooterUGNFacility"), DropDownList)
                ddTempUGNFacility.SelectedIndex = -1

                ddTempDepartment = CType(gvAvailablePerShiftFactor.FooterRow.FindControl("ddFooterDepartment"), DropDownList)
                ddTempDepartment.SelectedIndex = -1

                txtTempAvailablerPerShiftFactor = CType(gvAvailablePerShiftFactor.FooterRow.FindControl("txtFooterAvailablePerShiftFactor"), TextBox)
                txtTempAvailablerPerShiftFactor.Text = ""

                txtTempEffectiveDate = CType(gvAvailablePerShiftFactor.FooterRow.FindControl("txtFooterEffectiveDate"), TextBox)
                txtTempEffectiveDate.Text = ""

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

    Protected Sub ddEditUGNFacility_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data to the Edit WorkCenter drop down list based on UGNFacility Selection
        '' from the Edittemplate in the grid view.
        ''*******

        lblMessage.Text = ""

        Try
            Dim ddTempUGNFacility As DropDownList
            Dim ddTempDepartment As DropDownList

            Dim ds As DataSet

            Dim iRowCounter As Integer = 0
            Dim strUGNFacility As String = ""

            ddTempUGNFacility = CType(sender, DropDownList)
            Dim currentRowInEdit As Integer = gvAvailablePerShiftFactor.EditIndex

            ddTempDepartment = CType(gvAvailablePerShiftFactor.Rows(currentRowInEdit).FindControl("ddEditDepartment"), DropDownList)

            If ddTempUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddTempUGNFacility.SelectedValue
            Else
                strUGNFacility = ddTempUGNFacility.Items(0).Value
            End If

            ddTempDepartment.Items.Clear()
            'ds = PSRModule.GetManufacturingMetricWorkCenter(strUGNFacility)
            ds = PSRModule.GetManufacturingMetricDepartment(strUGNFacility)
            If commonFunctions.CheckDataset(ds) = True Then
                ddTempDepartment.DataSource = ds
                ddTempDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentDesc").ColumnName.ToString()
                ddTempDepartment.DataValueField = ds.Tables(0).Columns("CDEPT").ColumnName
                ddTempDepartment.DataBind()
            Else
                ddTempDepartment.Items.Insert(0, "")
            End If

            ddTempDepartment.SelectedIndex = -1

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub ddFooterUGNFacility_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data to the Edit WorkCenter drop down list based on UGNFacility Selection
        '' from the Edittemplate in the grid view.
        ''*******

        lblMessage.Text = ""

        Try
            Dim ddTempUGNFacility As DropDownList
            Dim ddTempDepartment As DropDownList

            Dim ds As DataSet

            Dim iRowCounter As Integer = 0           
            Dim strUGNFacility As String = ""

            ddTempUGNFacility = CType(sender, DropDownList)
            ddTempDepartment = CType(gvAvailablePerShiftFactor.FooterRow.FindControl("ddFooterDepartment"), DropDownList)

            If ddTempUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddTempUGNFacility.SelectedValue
            Else
                strUGNFacility = ddTempUGNFacility.Items(0).Value
            End If

            ddTempDepartment.Items.Clear()
            'ds = PSRModule.GetManufacturingMetricWorkCenter(strUGNFacility)
            ds = PSRModule.GetManufacturingMetricDepartment(strUGNFacility)
            If commonFunctions.CheckDataset(ds) = True Then
                ddTempDepartment.DataSource = ds
                ddTempDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentDesc").ColumnName.ToString()
                ddTempDepartment.DataValueField = ds.Tables(0).Columns("CDEPT").ColumnName
                ddTempDepartment.DataBind()
                ddTempDepartment.SelectedIndex = 5
            Else
                ddTempDepartment.Items.Insert(0, "")
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

    Protected Sub ddEditDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data to the Edit WorkCenter drop down list based on UGNFacility Selection
        '' from the Edittemplate in the grid view.
        ''*******

        lblMessage.Text = ""

        Try
            Dim ddTempDepartment As DropDownList
            Dim lblTempDeptID As Label

            Dim currentRowInEdit As Integer = gvAvailablePerShiftFactor.EditIndex

            ddTempDepartment = CType(gvAvailablePerShiftFactor.Rows(currentRowInEdit).FindControl("ddEditDepartment"), DropDownList)
            lblTempDeptID = CType(gvAvailablePerShiftFactor.Rows(currentRowInEdit).FindControl("lblEditDeptID"), Label)

            If ddTempDepartment.SelectedIndex > 0 Then
                lblTempDeptID.Text = ddTempDepartment.SelectedValue
            Else
                lblTempDeptID.Text = ddTempDepartment.Items(0).Value
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
#Region "Insert Empty GridView Work-Around"
    Private Property LoadDataEmpty_AvailablePerShiftFactor() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_AvailablePerShiftFactor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_AvailablePerShiftFactor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_AvailablePerShiftFactor") = value
        End Set

    End Property
    Protected Sub odsAvailablePerShiftFactor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsAvailablePerShiftFactor.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            'Dim dt As Costing.CostSheetPriceMargin_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetPriceMargin_MaintDataTable)
            Dim dt As Manufacturing_Metric.ManufacturingMetricAvailablePerShiftFactor_MaintDataTable = CType(e.ReturnValue, Manufacturing_Metric.ManufacturingMetricAvailablePerShiftFactor_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt IsNot Nothing Then
                If dt.Rows.Count = 0 Then
                    dt.Rows.Add(dt.NewRow())
                    LoadDataEmpty_AvailablePerShiftFactor = True
                Else
                    LoadDataEmpty_AvailablePerShiftFactor = False
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
    Protected Sub gvAvailablePerShiftFactor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAvailablePerShiftFactor.RowCreated

        Try
            ''hide first column
            'If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            'End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_AvailablePerShiftFactor
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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
#End Region ' Insert Empty GridView Work-Around

    Protected Sub ddUGNFacility_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUGNFacility.SelectedIndexChanged

        Try
            lblMessage.Text = ""

            Dim strUGNFacility As String = ""

            If ddUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue
            End If

            ddDepartment.SelectedIndex = -1

            BindDepartment(strUGNFacility)

            EnableControls()

            gvAvailablePerShiftFactor.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvAvailablePerShiftFactor_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAvailablePerShiftFactor.RowDataBound

        Try
            Dim ds As DataSet

            Dim ddTempUGNFacility As DropDownList
            Dim ddTempDepartment As DropDownList
            Dim lblTempDeptID As Label

            Dim strUGNFacility As String = ""
            Dim iDeptID As Integer = 0

            If e.Row.RowType = DataControlRowType.DataRow Then

                ddTempUGNFacility = CType(e.Row.FindControl("ddEditUGNFacility"), DropDownList)
                ddTempDepartment = CType(e.Row.FindControl("ddEditDepartment"), DropDownList)
                lblTempDeptID = CType(e.Row.FindControl("lblEditDeptID"), Label)

                If ddTempUGNFacility IsNot Nothing Then
                    If ddTempUGNFacility.SelectedIndex > 0 Then
                        strUGNFacility = ddTempUGNFacility.SelectedValue                  
                    End If
                End If

                If ddTempDepartment IsNot Nothing Then
                    ddTempDepartment.Items.Clear()
                    ds = PSRModule.GetManufacturingMetricDepartment(strUGNFacility)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        ddTempDepartment.DataSource = ds
                        ddTempDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentDesc").ColumnName.ToString()
                        ddTempDepartment.DataValueField = ds.Tables(0).Columns("CDEPT").ColumnName
                        ddTempDepartment.DataBind()

                        If lblTempDeptID IsNot Nothing Then
                            If lblTempDeptID.Text.Trim <> "" Then
                                iDeptID = CType(lblTempDeptID.Text.Trim, Integer)

                                If iDeptID > 0 Then
                                    ddTempDepartment.SelectedValue = iDeptID
                                End If
                            End If
                        End If
                    End If
                End If

            End If

            If (e.Row.RowType = DataControlRowType.Footer) Then

                ddTempUGNFacility = CType(e.Row.FindControl("ddFooterUGNFacility"), DropDownList)
                ddTempDepartment = CType(e.Row.FindControl("ddFooterDepartment"), DropDownList)

                If ddTempUGNFacility IsNot Nothing Then
                    If ddTempUGNFacility.SelectedIndex > 0 Then
                        strUGNFacility = ddTempUGNFacility.SelectedValue                    
                    End If
                End If

                If strUGNFacility = "" Then
                    If ddUGNFacility.SelectedIndex > 0 Then
                        strUGNFacility = ddUGNFacility.SelectedValue
                    End If
                End If

                If strUGNFacility = "" Then
                    strUGNFacility = "UN"
                End If

                If ddTempDepartment IsNot Nothing Then
                    ds = PSRModule.GetManufacturingMetricDepartment(strUGNFacility)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        ddTempDepartment.DataSource = ds
                        ddTempDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentDesc").ColumnName.ToString()
                        ddTempDepartment.DataValueField = ds.Tables(0).Columns("CDEPT").ColumnName
                        ddTempDepartment.DataBind()
                    End If
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Response.Redirect("Manufacturing_Metric_Available_Per_Shift_Factor.aspx?UGNFacility=" & ddUGNFacility.SelectedValue _
                       & "&DeptID=" & ddDepartment.SelectedValue, False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
