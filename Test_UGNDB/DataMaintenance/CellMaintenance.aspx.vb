' ************************************************************************************************
' Name:	CellMaintenance.aspx.vb
' Purpose:	This program is used to view Cells
'
' Date		    Author	    
' 04/2008       Roderick Carlson			Created .Net application
' 07/22/2008    Roderick Carlson            Cleaned Up Error Trapping
' 10/03/2008    Roderick Carlson            Added Security Role Select Statement
' 03/11/2009    Roderick Carlson            Added Filter to get Department

Partial Class DataMaintenance_CellMaintenance
    Inherits System.Web.UI.Page
    Protected Sub CheckRights()

        Try
            'default to hide edit column
            gvCellList.Columns(7).Visible = False
            If gvCellList.FooterRow IsNot Nothing Then
                gvCellList.FooterRow.Visible = False
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 18)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            'If iRoleID = 11 Then ' ADMIN RIGHTS                                
                            '    gvCellList.Columns(7).Visible = True
                            '    gvCellList.FooterRow.Visible = True
                            'End If
                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    gvCellList.Columns(7).Visible = True
                                    If gvCellList.FooterRow IsNot Nothing Then
                                        gvCellList.FooterRow.Visible = True
                                    End If
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    gvCellList.Columns(7).Visible = True
                                    If gvCellList.FooterRow IsNot Nothing Then
                                        gvCellList.FooterRow.Visible = True
                                    End If
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    gvCellList.Columns(7).Visible = False
                                    If gvCellList.FooterRow IsNot Nothing Then
                                        gvCellList.FooterRow.Visible = False
                                    End If
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete

                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                            End Select
                        End If
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Cell"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Cell"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("DMExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then

                BindCriteria()

                If Request.QueryString("CellName") IsNot Nothing Then
                    txtCellNameSearch.Text = Server.UrlDecode(Request.QueryString("CellName").ToString)
                End If

                If Request.QueryString("DepartmentID") IsNot Nothing Then
                    ddDepartmentSearch.SelectedValue = Server.UrlDecode(Request.QueryString("DepartmentID").ToString)
                End If

                If Request.QueryString("UGNFacility") IsNot Nothing Then
                    ddUGNFacilitySearch.SelectedValue = Server.UrlDecode(Request.QueryString("UGNFacility").ToString)
                End If

                If Request.QueryString("PlannerCode") IsNot Nothing Then
                    txtPlannerCodeSearch.Text = Server.UrlDecode(Request.QueryString("PlannerCode").ToString)
                End If

            End If

            CheckRights()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub BindCriteria()

        Try
            Dim ds As DataSet

            ''bind existing data to drop down OEM control for selection criteria 
            ds = commonFunctions.GetUGNFacility("")

            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddUGNFacilitySearch.DataSource = ds
                    ddUGNFacilitySearch.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                    ddUGNFacilitySearch.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                    ddUGNFacilitySearch.DataBind()
                    ddUGNFacilitySearch.Items.Insert(0, "")
                    ddUGNFacilitySearch.SelectedIndex = 0
                End If
            End If

            ''bind existing data to drop down OEM control for selection criteria 
            ds = commonFunctions.GetDepartment("", "", False)

            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddDepartmentSearch.DataSource = ds
                    ddDepartmentSearch.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName.ToString()
                    ddDepartmentSearch.DataValueField = ds.Tables(0).Columns("DepartmentID").ColumnName
                    ddDepartmentSearch.DataBind()
                    ddDepartmentSearch.Items.Insert(0, "")
                    ddDepartmentSearch.SelectedIndex = 0
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
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Response.Redirect("CellMaintenance.aspx?CellName=" & Server.UrlEncode(txtCellNameSearch.Text.Trim) & "&DepartmentID=" & Server.UrlEncode(ddDepartmentSearch.SelectedValue) & "&UGNFacility=" & Server.UrlEncode(ddUGNFacilitySearch.SelectedValue) & "&PlannerCode=" & Server.UrlEncode(txtPlannerCodeSearch.Text.Trim), False)
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
            Response.Redirect("CellMaintenance.aspx", False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub gvCellList_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            Dim txtCellNameTemp As TextBox
            Dim ddDepartmentTemp As DropDownList
            Dim ddUGNFacilityTemp As DropDownList
            Dim txtPlannerCodeTemp As TextBox
            'Dim chkObsoleteTemp As CheckBox
            Dim intRowsAffected As Integer = 0

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then

                txtCellNameTemp = CType(gvCellList.FooterRow.FindControl("txtCellNameInsert"), TextBox)
                ddDepartmentTemp = CType(gvCellList.FooterRow.FindControl("ddDepartmentInsert"), DropDownList)
                ddUGNFacilityTemp = CType(gvCellList.FooterRow.FindControl("ddUGNFacilityInsert"), DropDownList)
                txtPlannerCodeTemp = CType(gvCellList.FooterRow.FindControl("txtPlannerCodeInsert"), TextBox)

                odsCellList.InsertParameters("CellName").DefaultValue = txtCellNameTemp.Text
                odsCellList.InsertParameters("DepartmentID").DefaultValue = ddDepartmentTemp.SelectedValue
                odsCellList.InsertParameters("UGNFacility").DefaultValue = ddUGNFacilityTemp.SelectedValue
                odsCellList.InsertParameters("PlannerCode").DefaultValue = txtPlannerCodeTemp.Text
                intRowsAffected = odsCellList.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvCellList.ShowFooter = False
            Else
                gvCellList.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtCellNameTemp = CType(gvCellList.FooterRow.FindControl("txtCellNameInsert"), TextBox)
                txtCellNameTemp.Text = Nothing

                ddDepartmentTemp = CType(gvCellList.FooterRow.FindControl("ddDepartmentInsert"), DropDownList)
                ddDepartmentTemp.SelectedValue = Nothing

                ddUGNFacilityTemp = CType(gvCellList.FooterRow.FindControl("ddUGNFacilityInsert"), DropDownList)
                ddUGNFacilityTemp.SelectedValue = Nothing

                txtPlannerCodeTemp = CType(gvCellList.FooterRow.FindControl("txtPlannerCodeInsert"), TextBox)
                txtPlannerCodeTemp.Text = Nothing
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
#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_CellList() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CellList") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CellList"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CellList") = value
        End Set

    End Property

    Protected Sub odsCellList_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCellList.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Cells.Cell_MaintDataTable = CType(e.ReturnValue, Cells.Cell_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CellList = True
            Else
                LoadDataEmpty_CellList = False
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

    Protected Sub gvCellList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCellList.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CellList
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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
#End Region ' Insert Empty GridView Work-Around
End Class
