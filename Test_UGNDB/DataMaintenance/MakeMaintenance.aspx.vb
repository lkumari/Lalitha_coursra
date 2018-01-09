' ************************************************************************************************
' Name:	MakeMaintenance.aspx.vb
' Purpose:	This program is used to view, insert, update Make
'
' Date		    Author	    
' 04/2008       RCarlson			Created .Net application
' 07/22/2008    RCarlson            Cleaned Up Error Trapping
' 10/03/2008    RCarlson            Added Security Role Select Statement

Partial Class DataMaintenance_MakeMaintenance
    Inherits System.Web.UI.Page
    Protected Sub CheckRights()

        Try
            'default to hide edit column
            gvMakeList.Columns(4).Visible = False
            If gvMakeList.FooterRow IsNot Nothing Then
                gvMakeList.FooterRow.Visible = False
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

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 23)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            'If iRoleID = 11 Then ' ADMIN RIGHTS                                
                            'gvMakeList.Columns(4).Visible = True
                            'If gvMakeList.FooterRow IsNot Nothing Then
                            '    gvMakeList.FooterRow.Visible = True
                            'End If
                            'End If
                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    gvMakeList.Columns(5).Visible = True
                                    If gvMakeList.FooterRow IsNot Nothing Then
                                        gvMakeList.FooterRow.Visible = True
                                    End If
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    gvMakeList.Columns(5).Visible = True
                                    If gvMakeList.FooterRow IsNot Nothing Then
                                        gvMakeList.FooterRow.Visible = True
                                    End If
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    gvMakeList.Columns(5).Visible = False
                                    If gvMakeList.FooterRow IsNot Nothing Then
                                        gvMakeList.FooterRow.Visible = False
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
            m.ContentLabel = "Make"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Make"
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
                If Request.QueryString("MakeName") IsNot Nothing Then
                    txtMakeNameSearch.Text = Server.UrlDecode(Request.QueryString("MakeName").ToString)
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
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Response.Redirect("MakeMaintenance.aspx?MakeName=" & Server.UrlEncode(txtMakeNameSearch.Text.Trim), False)
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
            Response.Redirect("MakeMaintenance.aspx", False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub gvMakeList_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            Dim txtMakeNameTemp As TextBox

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then
                txtMakeNameTemp = CType(gvMakeList.FooterRow.FindControl("txtMakeNameInsert"), TextBox)
                odsMakeList.InsertParameters("MakeName").DefaultValue = txtMakeNameTemp.Text
                odsMakeList.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvMakeList.ShowFooter = False
            Else
                gvMakeList.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtMakeNameTemp = CType(gvMakeList.FooterRow.FindControl("txtMakeNameInsert"), TextBox)
                txtMakeNameTemp.Text = Nothing
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

    Private Property LoadDataEmpty_MakeList() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_MakeList") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_MakeList"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_MakeList") = value
        End Set

    End Property

    Protected Sub odsMakeList_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsMakeList.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Makes.Make_MaintDataTable = CType(e.ReturnValue, Makes.Make_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_MakeList = True
            Else
                LoadDataEmpty_MakeList = False
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

    Protected Sub gvMakeList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMakeList.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_MakeList
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
