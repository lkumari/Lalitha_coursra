' ************************************************************************************************
' Name:	Copy_Sales_Projection.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events. The purpose of this program is to allow users
'           to copy current part info and carry the data into a new part number or a series of part
'           numbers to save time in data entry.
'
' Date		    Author	    
' 04/04/2008    LRey			Created .Net application
' 06/21/2012    LRey            Modified the vb code to adhere to new vb standards
' ************************************************************************************************
Partial Class PF_Copy_Sales_Projection
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

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Copy Sales Projection"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Planning and Forecasting </b> > <a href='Sales_Projection_List.aspx'><b>Sales Projection Search</b></a> > <a href='Sales_Projection.aspx?sPartNo=" + ViewState("sPartNo") + "'><b>Sales Projection</b></a> > Copy Sales Projection "
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

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********

        Try
            ''*******
            '' Disable controls by default
            ''*******
            ViewState("ObjectRole") = False
            btnSubmit.Enabled = False

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
                                        ViewState("ObjectRole") = True
                                        btnSubmit.Enabled = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        ViewState("ObjectRole") = True
                                        btnSubmit.Enabled = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        ViewState("ObjectRole") = False
                                        btnSubmit.Enabled = True
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        ViewState("ObjectRole") = False
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        ViewState("ObjectRole") = False
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

    'Protected Sub BindCriteria()
    '    Dim SourcePartNo As String = HttpContext.Current.Request.QueryString("sPartNo")
    '    Dim ds As DataSet = New DataSet

    '    Try
    '        ''****
    '        ''Confirm there is data found in the Projected_Sales_Copy table
    '        ''****
    '        ds = PFModule.GetProjectedSalesCopy(SourcePartNo)
    '        If (ds.Tables.Item(0).Rows.Count > 0) Then
    '            btnSubmit.Visible = True
    '        Else
    '            btnSubmit.Visible = False
    '        End If
    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblErrors.Text = "Error occurred during save record.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '        lblErrors.Visible = True
    '    End Try

    'End Sub

    Protected Sub gvCopy_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCopy.RowCommand
        Try
            Dim DestinationPartNo As DropDownList
            Dim SourcePartNo As String = HttpContext.Current.Request.QueryString("sPartNo")

            ''***
            ''This section allows the inserting of a new row when save button is clicked from the footer.
            ''***
            If e.CommandName = "Insert" Then
                ''Insert data

                If gvCopy.Rows.Count = 0 Then
                    '' We are inserting through the DetailsView in the EmptyDataTemplate
                    Return
                End If

                '' Only perform the following logic when inserting through the footer
                DestinationPartNo = CType(gvCopy.FooterRow.FindControl("ddDestinationPartNo"), DropDownList)
                odsCopy.InsertParameters("DestinationPartNo").DefaultValue = DestinationPartNo.Text

                odsCopy.Insert()

                ''Response.Redirect("Sales_Projection.aspx?sPartNo=" & SourcePartNo, false)
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvCopy.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvCopy.ShowFooter = True
                Else
                    gvCopy.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                DestinationPartNo = CType(gvCopy.FooterRow.FindControl("ddDestinationPartNo"), DropDownList)
                DestinationPartNo.Text = Nothing
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub gvCopy_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCopy.RowDataBound
        Try
            ''***
            ''This section provides the user with the popup for confirming the delete of a record.
            ''Called by the onClientClick event.
            ''***
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' reference the Delete ImageButton
                Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
                If imgBtn IsNot Nothing Then
                    Dim db As ImageButton = CType(e.Row.Cells(1).Controls(1), ImageButton)

                    ' Get information about the product bound to the row
                    If db.CommandName = "Delete" Then
                        Dim DestinationPartNo As Projected_Sales.Projected_Sales_CopyRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Projected_Sales.Projected_Sales_CopyRow)

                        db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Destination Part Number (" & DataBinder.Eval(e.Row.DataItem, "DestinationPartNo") & ")?');")

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
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim SourcePartNo As String = HttpContext.Current.Request.QueryString("sPartNo")
        Dim ds As DataSet = New DataSet

        Try
            ''****
            ''Confirm there is data found in the Projected_Sales_Copy table
            ''****
            ds = PFModule.GetProjectedSalesCopy(SourcePartNo)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ''****
                ''Process carryover
                ''****
                PFModule.CopyProjectedSales(SourcePartNo)
                PFModule.DeleteProjectedSalesCopy(SourcePartNo, "")

                Response.Redirect("Sales_Projection_List.aspx", False)
            Else
                lblErrors.Text = "Submit Cancelled... Destination is undefined."
                lblErrors.Visible = True
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during save record.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            lblErrors.Visible = True
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim SourcePartNo As String = HttpContext.Current.Request.QueryString("sPartNo")

        ''****
        ''Delete any Part Number associated to the SourcePartNo from Projected_Sales_Copy.  
        ''User Decided not to run the copy process.
        ''****
        PFModule.DeleteProjectedSalesCopy(SourcePartNo, "")

        Response.Redirect("Sales_Projection.aspx?sPartNo=" & SourcePartNo, False)

    End Sub
    'Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    '    BindCriteria()
    'End Sub
#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_Copy() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Copy") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Copy"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Copy") = value
        End Set

    End Property

    Protected Sub odsCopy_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCopy.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Projected_Sales.Projected_Sales_CopyDataTable = CType(e.ReturnValue, Projected_Sales.Projected_Sales_CopyDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Copy = True
            Else
                LoadDataEmpty_Copy = False
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

    Protected Sub gvCopy_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCopy.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Copy
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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

#End Region ' Insert Empty GridView Work-Around
End Class
