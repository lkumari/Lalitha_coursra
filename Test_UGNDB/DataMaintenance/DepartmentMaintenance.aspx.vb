' ************************************************************************************************
' Name:	DepartmentMaintenance.aspx.vb
' Purpose:	This program is used to view, insert, update Department
'
' Date		    Author	    
' 04/2008       RCarlson			Created .Net application
' 07/22/2008    RCarlson            Cleaned Up Error Trapping
' 10/03/2008    RCarlson            Added Security Role Select Statement
' 03/11/2009    RCarlson            Added Filter 
' 01/14/2014    LRey                Modified to use the new ERP COA listing

Partial Class DataMaintenance_DepartmentMaintenance
    Inherits System.Web.UI.Page
    Protected Sub CheckRights()

        Try
            'default to hide edit column
            gvDepartmentList.Columns(6).Visible = False
            If gvDepartmentList.FooterRow IsNot Nothing Then
                gvDepartmentList.FooterRow.Visible = False
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

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 21)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            'If iRoleID = 11 Then ' ADMIN RIGHTS                                
                            '    gvDepartmentList.Columns(5).Visible = True
                            '    If gvDepartmentList.FooterRow IsNot Nothing Then
                            '        gvDepartmentList.FooterRow.Visible = True
                            '    End If
                            'End If

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    gvDepartmentList.Columns(6).Visible = True
                                    If gvDepartmentList.FooterRow IsNot Nothing Then
                                        gvDepartmentList.FooterRow.Visible = True
                                    End If
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    gvDepartmentList.Columns(6).Visible = True
                                    If gvDepartmentList.FooterRow IsNot Nothing Then
                                        gvDepartmentList.FooterRow.Visible = True
                                    End If
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    gvDepartmentList.Columns(5).Visible = False
                                    If gvDepartmentList.FooterRow IsNot Nothing Then
                                        gvDepartmentList.FooterRow.Visible = False
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
            m.ContentLabel = "Charge of Accounts"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> <b>Data Maintenance</b> > Charge of Accounts"
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

                If Request.QueryString("DepartmentName") IsNot Nothing Then
                    txtDepartmentNameSearch.Text = Server.UrlDecode(Request.QueryString("DepartmentName").ToString)
                End If

                If Request.QueryString("UGNFacility") IsNot Nothing Then
                    ddUGNFacilitySearch.SelectedValue = Server.UrlDecode(Request.QueryString("UGNFacility").ToString)
                End If

                If Request.QueryString("Filter") IsNot Nothing Then
                    cbFilter.Checked = Server.UrlDecode(Request.QueryString("Filter").ToString)
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
            Response.Redirect("DepartmentMaintenance.aspx?departmentName=" & Server.UrlEncode(txtDepartmentNameSearch.Text.Trim) & "&UGNFacility=" & Server.UrlEncode(ddUGNFacilitySearch.SelectedValue) & "&Filter=" & cbFilter.Checked, False)
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
            Response.Redirect("DepartmentMaintenance.aspx", False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvDepartmentList_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            Dim txtDepartmentNameTemp As TextBox
            Dim txtGLNoTemp As TextBox
            Dim ddUGNFacilityTemp As DropDownList
            Dim cbFilterTemp As CheckBox

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then
                txtDepartmentNameTemp = CType(gvDepartmentList.FooterRow.FindControl("txtDepartmentNameInsert"), TextBox)
                txtGLNoTemp = CType(gvDepartmentList.FooterRow.FindControl("txtGLNoInsert"), TextBox)
                ddUGNFacilityTemp = CType(gvDepartmentList.FooterRow.FindControl("ddUGNFacilityInsert"), DropDownList)
                cbFilterTemp = CType(gvDepartmentList.FooterRow.FindControl("chkFilterInsert"), CheckBox)

                odsDepartmentList.InsertParameters("DepartmentName").DefaultValue = txtDepartmentNameTemp.Text
                odsDepartmentList.InsertParameters("UGNFacility").DefaultValue = ddUGNFacilityTemp.SelectedValue
                odsDepartmentList.InsertParameters("Filter").DefaultValue = cbFilterTemp.Checked
                odsDepartmentList.InsertParameters("GLNo").DefaultValue = txtGLNoTemp.Text
                odsDepartmentList.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvDepartmentList.ShowFooter = False
            Else
                gvDepartmentList.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtDepartmentNameTemp = CType(gvDepartmentList.FooterRow.FindControl("txtDepartmentNameInsert"), TextBox)
                txtDepartmentNameTemp.Text = Nothing

                txtGLNoTemp = CType(gvDepartmentList.FooterRow.FindControl("txtGLNoInsert"), TextBox)
                txtGLNoTemp.Text = Nothing

                ddUGNFacilityTemp = CType(gvDepartmentList.FooterRow.FindControl("ddUGNFacilityInsert"), DropDownList)
                ddUGNFacilityTemp.SelectedValue = Nothing

                cbFilterTemp = CType(gvDepartmentList.FooterRow.FindControl("chkFilterInsert"), CheckBox)
                cbFilterTemp.Checked = False
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

    Private Property LoadDataEmpty_DepartmentList() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_DepartmentList") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_DepartmentList"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_DepartmentList") = value
        End Set

    End Property

    Protected Sub odsDepartmentList_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsDepartmentList.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Departments.Department_MaintDataTable = CType(e.ReturnValue, Departments.Department_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_DepartmentList = True
            Else
                LoadDataEmpty_DepartmentList = False
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

    Protected Sub gvDepartmentList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDepartmentList.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_DepartmentList
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
