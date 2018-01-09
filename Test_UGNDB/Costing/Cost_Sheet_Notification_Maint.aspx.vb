' ***********************************************************************************************
'
' Name:		Cost_Sheet_Notification_Maint.aspx
' Purpose:	This Code Behind is for the Cost Sheet Detail of the Costing/Quote Forms
'
' Date		Author	    
' 10/27/2008 RCarlson  
' ************************************************************************************************
Partial Class Cost_Sheet_Notification_Maint
    Inherits System.Web.UI.Page
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

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 74)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    ViewState("isRestricted") = False
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                    ViewState("isRestricted") = True
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

            If ViewState("isRestricted") = False Then

                gvGroup.Visible = ViewState("isAdmin")
                gvGroupTeamMember.Visible = ViewState("isAdmin")

                lblGroupName.Visible = ViewState("isAdmin")
                lblTeamMember.Visible = ViewState("isAdmin")
                ddSearchGroupName.Visible = ViewState("isAdmin")
                ddSearchTeamMember.Visible = ViewState("isAdmin")
                btnSearch.Visible = ViewState("isAdmin")
                btnReset.Visible = ViewState("isAdmin")
                
                If gvGroup.FooterRow IsNot Nothing Then
                    gvGroup.FooterRow.Visible = ViewState("isAdmin")
                End If

                If gvGroupTeamMember.FooterRow IsNot Nothing Then
                    gvGroupTeamMember.FooterRow.Visible = ViewState("isAdmin")
                End If

            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
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

            ''bind existing data to drop down Group List
            ds = CostingModule.GetCostSheetGroup(0)
            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddSearchGroupName.DataSource = ds
                    ddSearchGroupName.DataTextField = ds.Tables(0).Columns("ddGroupName").ColumnName.ToString()
                    ddSearchGroupName.DataValueField = ds.Tables(0).Columns("GroupID").ColumnName
                    ddSearchGroupName.DataBind()
                    ddSearchGroupName.Items.Insert(0, "")
                End If
            End If

            ''bind existing data to drop down Team Member
            ds = CostingModule.GetCostSheetApproverBySubscription(0, 0)

            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddSearchTeamMember.DataSource = ds
                    ddSearchTeamMember.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName.ToString()
                    ddSearchTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
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

        lblMessageBottom.Text = lblMessage.Text

    End Sub
    Protected Sub ddFooterTeamMemberName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data to the Footer PreApproval SubScription drop down list based on TeamMember Selection
        '' from the Edittemplate in the grid view.
        ''*******

        lblMessage.Text = ""

        Try
            Dim ddTeamMember As DropDownList
            Dim ddSubscription As DropDownList
            Dim ds As DataSet
            Dim iRowCounter As Integer = 0
            Dim liSubscriptionItem As ListItem

            ddTeamMember = CType(sender, DropDownList)
            ddSubscription = CType(gvGroupTeamMember.FooterRow.FindControl("ddFooterSubscription"), DropDownList)

            ds = CostingModule.GetCostSheetSubscriptionByApprover(ddTeamMember.SelectedValue, 0)
            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                    'clear all rows
                    ddSubscription.Items.Clear()
                    For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                        liSubscriptionItem = New ListItem
                        liSubscriptionItem.Text = ds.Tables(0).Rows(iRowCounter).Item("Subscription").ToString
                        liSubscriptionItem.Value = ds.Tables(0).Rows(iRowCounter).Item("SubscriptionID")
                        ddSubscription.Items.Add(liSubscriptionItem)
                    Next

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

        lblMessageBottom.Text = lblMessage.Text

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Cost Sheet Pre-Approval Notification Group and Team Member Maintenance"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > Cost Sheet Notification Group and Team Member Maintenance "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            CheckRights()

            If Not Page.IsPostBack Then
                'clear crystal reports
                CostingModule.CleanCostingCrystalReports()

                BindCriteria()
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

        lblMessageBottom.Text = lblMessage.Text

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

                bResult = CostingModule.CopyCostSheetGroup(iGroupID)

                If bResult = False Then
                    lblMessage.Text += "Error: The Group was NOT copied successfully."
                    lblMessageBottom.Text += "Error: The Group was NOT copied successfully."
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

        lblMessageBottom.Text = lblMessage.Text

    End Sub   

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            lblMessage.Text = ""            

            ddSearchGroupName.SelectedIndex = -1
            ddSearchTeamMember.SelectedIndex = -1

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

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

        lblMessageBottom.Text = lblMessage.Text

    End Sub
    Protected Sub gvGroup_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGroup.DataBound

        'hide header of first column
        If gvGroup.Rows.Count > 0 Then
            gvGroup.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvGroup_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGroup.RowCommand

        Try
            lblMessage.Text = ""

            Dim txtGroupNameTemp As TextBox
            Dim cbGroupObsoleteTemp As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                txtGroupNameTemp = CType(gvGroup.FooterRow.FindControl("txtFooterGroupName"), TextBox)
                cbGroupObsoleteTemp = CType(gvGroup.FooterRow.FindControl("cbFooterGroupObsolete"), CheckBox)

                odsCostSheetGroup.InsertParameters("GroupName").DefaultValue = txtGroupNameTemp.Text
                odsCostSheetGroup.InsertParameters("Obsolete").DefaultValue = cbGroupObsoleteTemp.Checked

                intRowsAffected = odsCostSheetGroup.Insert()

                gvGroupTeamMember.DataBind()
                BindCriteria()
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
                txtGroupNameTemp = CType(gvGroup.FooterRow.FindControl("txtFooterGroupName"), TextBox)
                txtGroupNameTemp.Text = Nothing

                cbGroupObsoleteTemp = CType(gvGroup.FooterRow.FindControl("cbFooterGroupObsolete"), CheckBox)
                cbGroupObsoleteTemp.Checked = False

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvGroupTeamMember_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGroupTeamMember.DataBound

        'hide header of first column
        If gvGroupTeamMember.Rows.Count > 0 Then
            gvGroupTeamMember.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvGroupTeamMember_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGroupTeamMember.RowCommand

        Try
            lblMessage.Text = ""

            Dim ds As DataSet
            Dim bFoundIt As Boolean = False

            Dim ddGroupNameTemp As DropDownList
            Dim ddTeamMemberTemp As DropDownList
            Dim ddSubscriptionTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddGroupNameTemp = CType(gvGroupTeamMember.FooterRow.FindControl("ddFooterGroupTeamMemberName"), DropDownList)
                ddTeamMemberTemp = CType(gvGroupTeamMember.FooterRow.FindControl("ddFooterTeamMemberName"), DropDownList)
                ddSubscriptionTemp = CType(gvGroupTeamMember.FooterRow.FindControl("ddFooterSubscription"), DropDownList)


                ds = CostingModule.GetCostSheetGroupTeamMember(ddGroupNameTemp.SelectedValue, ddTeamMemberTemp.SelectedValue, 0)

                If ds IsNot Nothing Then
                    If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                        bFoundIt = True
                    End If
                End If


                If bFoundIt = False Then
                    ds = CostingModule.GetCostSheetGroupTeamMember(ddGroupNameTemp.SelectedValue, 0, ddSubscriptionTemp.SelectedValue)

                    If ds IsNot Nothing Then
                        If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                            bFoundIt = True
                        End If
                    End If
                End If

                If bFoundIt = False Then
                    odsCostSheetGroupTeamMember.InsertParameters("GroupID").DefaultValue = ddGroupNameTemp.SelectedValue
                    odsCostSheetGroupTeamMember.InsertParameters("TeamMemberID").DefaultValue = ddTeamMemberTemp.SelectedValue
                    odsCostSheetGroupTeamMember.InsertParameters("SubscriptionID").DefaultValue = ddSubscriptionTemp.SelectedValue

                    intRowsAffected = odsCostSheetGroupTeamMember.Insert()
                Else
                    lblMessage.Text += "Error: Either a team member or a subscription has been selected twice for this group.<br>"
                    lblMessageBottom.Text += "Error: Either a team member or a subscription has been selected twice for this group.<br>"
                End If

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
                ddGroupNameTemp = CType(gvGroupTeamMember.FooterRow.FindControl("ddFooterGroupTeamMemberName"), DropDownList)
                ddGroupNameTemp.SelectedIndex = -1

                ddTeamMemberTemp = CType(gvGroupTeamMember.FooterRow.FindControl("ddFooterTeamMemberName"), DropDownList)
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

        lblMessageBottom.Text = lblMessage.Text

    End Sub

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
    Protected Sub odsGroup_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCostSheetGroup.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetGroup_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetGroup_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Group = True
            Else
                LoadDataEmpty_Group = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

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

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

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
    Protected Sub odsGroupTeamMember_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCostSheetGroupTeamMember.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetGroupTeamMember_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetGroupTeamMember_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_GroupTeamMember = True
            Else
                LoadDataEmpty_GroupTeamMember = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub
    Protected Sub gvGroupTeamMember_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGroupTeamMember.RowCreated

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

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_GroupTeamMember
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

        lblMessageBottom.Text = lblMessage.Text

    End Sub

#End Region

End Class
