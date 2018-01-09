' ************************************************************************************************
'
' Name:		ECI_Notification_Group_Maint.aspx
' Purpose:	This Code Behind is for the ECI Description List of Tasks assigned to Team Members
'
' Date  		Author	    
' 06/11/2009    Roderick Carlson    Created

Partial Class ECI_Notification_Group_Maint
    Inherits System.Web.UI.Page

#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_Group() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Group") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Group"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Group") = value
        End Set

    End Property

    Protected Sub odsGroup_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsGroup.Selected

        'From Andrew Robinson's Insert Empty GridView solution
        'http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        'bubble exceptions before we touch e.ReturnValue

        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As ECI.ECIGroup_MaintDataTable = CType(e.ReturnValue, ECI.ECIGroup_MaintDataTable)

        'if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Group = True
            Else
                LoadDataEmpty_Group = False
            End If
        End If

    End Sub
    Protected Sub gvGroup_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvGroup.RowUpdated

        lblMessage.Text = ""

        Try
            'refresh GroupTeamMember and dropdowns if Group was updated
            gvGroupTeamMember.DataBind()
            BindCriteria()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessage.Text = lblMessage.Text

    End Sub
    Protected Sub gvGroup_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGroup.DataBound

        'hide header of first column
        If gvGroup.Rows.Count > 0 Then
            gvGroup.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvGroup_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGroup.RowCommand

        lblMessage.Text = ""

        Try

            Dim txtGroupTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                txtGroupTemp = CType(gvGroup.FooterRow.FindControl("txtInsertGroupName"), TextBox)

                odsGroup.InsertParameters("GroupName").DefaultValue = txtGroupTemp.Text

                intRowsAffected = odsGroup.Insert()

                lblMessage.Text = "Record Saved Successfully."
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvGroup.ShowFooter = False
            Else
                gvGroup.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtGroupTemp = CType(gvGroup.FooterRow.FindControl("txtInsertGroupName"), TextBox)
                txtGroupTemp.Text = Nothing
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

    Protected Sub gvGroup_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGroup.RowCreated

        Try
            'hide first column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Group
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

    Private Property LoadDataEmpty_GroupTeamMember() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_GroupTeamMember") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_GroupTeamMember"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_GroupTeamMember") = value
        End Set

    End Property

    Protected Sub odsGroupTeamMember_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsGroupTeamMember.Selected

        'From Andrew Robinson's Insert Empty GridView solution
        'http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        'bubble exceptions before we touch e.ReturnValue

        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As ECI.ECIGroupTeamMember_MaintDataTable = CType(e.ReturnValue, ECI.ECIGroupTeamMember_MaintDataTable)

        'if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_GroupTeamMember = True
            Else
                LoadDataEmpty_GroupTeamMember = False
            End If
        End If

    End Sub

    Protected Sub gvGroupTeamMember_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGroupTeamMember.RowCommand

        lblMessage.Text = ""

        Try

            Dim ddGroupTemp As DropDownList
            Dim ddTeamMemberTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddGroupTemp = CType(gvGroupTeamMember.FooterRow.FindControl("ddInsertGroup"), DropDownList)
                ddTeamMemberTemp = CType(gvGroupTeamMember.FooterRow.FindControl("ddInsertGroupTeamMember"), DropDownList)

                odsGroupTeamMember.InsertParameters("GroupID").DefaultValue = ddGroupTemp.SelectedValue
                odsGroupTeamMember.InsertParameters("TeamMemberID").DefaultValue = ddTeamMemberTemp.SelectedValue

                intRowsAffected = odsGroupTeamMember.Insert()

                lblMessage.Text = "Record Saved Successfully"
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvGroupTeamMember.ShowFooter = False
            Else
                gvGroupTeamMember.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddGroupTemp = CType(gvGroupTeamMember.FooterRow.FindControl("ddInsertGroup"), DropDownList)
                ddGroupTemp.SelectedIndex = -1

                ddTeamMemberTemp = CType(gvGroupTeamMember.FooterRow.FindControl("ddInsertGroupTeamMember"), DropDownList)
                ddTeamMemberTemp.SelectedIndex = -1

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

    Protected Sub gvGroupTeamMember_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGroupTeamMember.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_GroupTeamMember
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

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 85)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("isAdmin") = True
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("isAdmin") = True
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("isAdmin") = True
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                                Case 15 '*** UGNEdit: No Create/Edit/No Delete

                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                            End Select
                        End If
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
    Protected Sub EnableControls()

        Try

            gvGroup.Columns(gvGroup.Columns.Count - 1).Visible = ViewState("isAdmin")
            gvGroup.Columns(gvGroup.Columns.Count - 2).Visible = ViewState("isAdmin")
            If gvGroup.FooterRow IsNot Nothing Then
                gvGroup.FooterRow.Visible = ViewState("isAdmin")
            End If


            gvGroupTeamMember.Columns(gvGroupTeamMember.Columns.Count - 1).Visible = ViewState("isAdmin")
            If gvGroupTeamMember.FooterRow IsNot Nothing Then
                gvGroupTeamMember.FooterRow.Visible = ViewState("isAdmin")
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
    Protected Sub BindCriteria()

        Try
            Dim ds As DataSet

            'bind existing Group Names to Dropdown
            ds = ECIModule.GetECIGroup(0, "")
            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddSearchGroupName.DataSource = ds
                    ddSearchGroupName.DataTextField = ds.Tables(0).Columns("ddGroupName").ColumnName
                    ddSearchGroupName.DataValueField = ds.Tables(0).Columns("GroupID").ColumnName
                    ddSearchGroupName.DataBind()
                    ddSearchGroupName.Items.Insert(0, "")
                End If
            End If

            'bind existing team members Dropdown
            ds = commonFunctions.GetTeamMemberBySubscription(64)
            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddSearchTeamMember.DataSource = ds
                    ddSearchTeamMember.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                    ddSearchTeamMember.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                    ddSearchTeamMember.DataBind()
                    ddSearchTeamMember.Items.Insert(0, "")
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim m As ASP.masterpage_master = Master
        m.PageTitle = "UGN, Inc."
        m.ContentLabel = "ECI Notification Group and Team Member Maintenance"
        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Engineering Change Instruction </b> > <a href='ECI_List.aspx'><b> ECI List </b></a> > ECI Notification Group Maintenance"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

        ''*****
        ''Expand menu item
        ''*****
        Dim testMasterPanel As CollapsiblePanelExtender
        testMasterPanel = CType(Master.FindControl("ECIExtender"), CollapsiblePanelExtender)
        testMasterPanel.Collapsed = False

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            CheckRights()

            If Not Page.IsPostBack Then
                ''clear crystal reports
                'ECIModule.CleanCostingCrystalReports()

                BindCriteria()

                ' ''******
                ''get query string
                ' ''******

                If HttpContext.Current.Request.QueryString("GroupID") <> "" Then
                    If HttpContext.Current.Request.QueryString("GroupID") > 0 Then
                        ddSearchGroupName.SelectedValue = HttpContext.Current.Request.QueryString("GroupID")
                    End If
                End If

                If HttpContext.Current.Request.QueryString("TeamMemberID") <> "" Then
                    If HttpContext.Current.Request.QueryString("TeamMemberID") > 0 Then
                        ddSearchTeamMember.SelectedValue = HttpContext.Current.Request.QueryString("TeamMemberID")
                    End If
                End If

            End If

            EnableControls()

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

            Response.Redirect("ECI_Notification_Group_Maint.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub imageBtnCopyGroup_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Try
            lblMessage.Text = ""

            Dim bResult As Boolean = False
            Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)

            'lblMessage.Text = "Row: " & row.RowIndex
            'lblMessage.Text += "<br>Group: " & row.Cells(0).Text

            Dim iGroupID As Integer = 0

            If row.Cells(0).Text <> "" Then
                iGroupID = CType(row.Cells(0).Text, Integer)

                bResult = ECIModule.CopyECIGroup(iGroupID)

                If bResult = False Then
                    lblMessage.Text += "Error: The Group was NOT copied successfully."                  
                End If

                gvGroup.DataBind()
                gvGroupTeamMember.DataBind()
                BindCriteria()

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
            lblMessage.Text = ""

            Response.Redirect("ECI_Notification_Group_Maint.aspx?GroupID=" & ddSearchGroupName.SelectedValue & "&TeamMemberID=" & ddSearchTeamMember.SelectedValue, False)

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
