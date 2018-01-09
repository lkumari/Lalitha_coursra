' ************************************************************************************************
' Name:	Instructions.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 09/07/2012    SHoward		Created .Net application
' ************************************************************************************************
Partial Class Packaging_Instructions
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim m As ASP.masterpage_master = Master
        m.PageTitle = "UGN, Inc."
        m.ContentLabel = "Instruction Maintenance"
        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Packaging</b> > Instruction Maintenance"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

        ''*****
        ''Expand menu item
        ''*****
        Dim MasterPanel As CollapsiblePanelExtender
        MasterPanel = CType(Master.FindControl("PKGExtender"), CollapsiblePanelExtender)
        MasterPanel.Collapsed = False

    End Sub 'EOF Page_Init

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            CheckRights()

            If Not Page.IsPostBack Then
                ' ''******
                ''get query string
                ' ''******
                If HttpContext.Current.Request.QueryString("Instruction") <> "" Then
                    txtSearch.Text = HttpContext.Current.Request.QueryString("Instruction")
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

    End Sub 'EOF Page_Load

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

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 47)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

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

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF CheckRights

    Protected Sub EnableControls()

        Try
            gvInstru.Columns(gvInstru.Columns.Count - 1).Visible = ViewState("isAdmin")
            If gvInstru.FooterRow IsNot Nothing Then
                gvInstru.FooterRow.Visible = ViewState("isAdmin")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF EnableControls

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            lblMessage.Text = ""

            Response.Redirect("Instructions.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnReset_Click

#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_Instru() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Instru") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Instru"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Instru") = value
        End Set

    End Property 'EOF LoadDataEmpty

    Protected Sub odsInstruMaint_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsInstruMaint.Selected

        'From Andrew Robinson's Insert Empty GridView solution
        'http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        'bubble exceptions before we touch e.ReturnValue

        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the odsTADSMaterial select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As PKG.InstructionMaintDataTable = CType(e.ReturnValue, PKG.InstructionMaintDataTable)

        'if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Instru = True


            Else
                LoadDataEmpty_Instru = False
            End If
        End If

    End Sub 'EOF odsInstruMaint_Selected

    Protected Sub gvInstru_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvInstru.RowCommand

        Try

            Dim txtTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                txtTemp = CType(gvInstru.FooterRow.FindControl("txtInsertInstruction"), TextBox)

                odsInstruMaint.InsertParameters("Instruction").DefaultValue = txtTemp.Text

                intRowsAffected = odsInstruMaint.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvInstru.ShowFooter = False
            Else
                gvInstru.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtTemp = CType(gvInstru.FooterRow.FindControl("txtInsertInstruction"), TextBox)
                txtTemp.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF gvInstru_RowCommand

    Protected Sub gvInstru_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInstru.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Instru
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

    End Sub 'EOF gvInstru_RowCreated


#End Region 'EOF Insert Empty GridView Work-Around


End Class
