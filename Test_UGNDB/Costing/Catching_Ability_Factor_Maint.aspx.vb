' ************************************************************************************************
'
' Name:		CostSheetCatchingAbilityFactorMaint.aspx
' Purpose:	This Code Behind is to maintain the catching ability factor used by the Costing Module
'
' Date		Author	    
' 10/14/2008 RCarlson

' ************************************************************************************************
Partial Class Catching_Ability_Factor_Maint
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

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 60)

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

                gvCatchingAbilityFactor.Columns(gvCatchingAbilityFactor.Columns.Count - 1).Visible = ViewState("isAdmin")
                If gvCatchingAbilityFactor.FooterRow IsNot Nothing Then
                    gvCatchingAbilityFactor.FooterRow.Visible = ViewState("isAdmin")
                End If
            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
                gvCatchingAbilityFactor.Visible = False               
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
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Catching Ability Factor Maintenance"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > Catching Ability Factor Maintenance "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            'clear crystal reports
            CostingModule.CleanCostingCrystalReports()

            CheckRights()

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
    Protected Sub gvCatchingAbilityFactor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCatchingAbilityFactor.DataBound

        'hide header of first column
        If gvCatchingAbilityFactor.Rows.Count > 0 Then
            gvCatchingAbilityFactor.HeaderRow.Cells(0).Visible = False
        End If

    End Sub

    Protected Sub gvCatchingAbilityFactor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCatchingAbilityFactor.RowCommand

        Try

            Dim txtMinimumPartLengthTemp As TextBox
            Dim txtMaximumPartLengthTemp As TextBox
            Dim cbIsSideBySideTemp As CheckBox
            Dim txtCatchingAbilityFactorTemp As TextBox
            Dim cbObsoleteTemp As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                txtMinimumPartLengthTemp = CType(gvCatchingAbilityFactor.FooterRow.FindControl("txtFooterMinimumPartLength"), TextBox)
                txtMaximumPartLengthTemp = CType(gvCatchingAbilityFactor.FooterRow.FindControl("txtFooterMaximumPartLength"), TextBox)
                cbIsSideBySideTemp = CType(gvCatchingAbilityFactor.FooterRow.FindControl("cbFooterIsSideBySide"), CheckBox)
                txtCatchingAbilityFactorTemp = CType(gvCatchingAbilityFactor.FooterRow.FindControl("txtFooterCatchingAbilityFactor"), TextBox)
                cbObsoleteTemp = CType(gvCatchingAbilityFactor.FooterRow.FindControl("cbFooterObsolete"), CheckBox)

                odsCatchingAbilityFactor.InsertParameters("MinimumPartLength").DefaultValue = txtMinimumPartLengthTemp.Text
                odsCatchingAbilityFactor.InsertParameters("MaximumPartLength").DefaultValue = txtMaximumPartLengthTemp.Text
                odsCatchingAbilityFactor.InsertParameters("IsSideBySide").DefaultValue = cbIsSideBySideTemp.Checked
                odsCatchingAbilityFactor.InsertParameters("CatchingAbilityFactor").DefaultValue = txtCatchingAbilityFactorTemp.Text
                odsCatchingAbilityFactor.InsertParameters("Obsolete").DefaultValue = cbObsoleteTemp.Checked

                intRowsAffected = odsCatchingAbilityFactor.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvCatchingAbilityFactor.ShowFooter = False
            Else
                gvCatchingAbilityFactor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtMinimumPartLengthTemp = CType(gvCatchingAbilityFactor.FooterRow.FindControl("txtFooterMinimumPartLength"), TextBox)
                txtMinimumPartLengthTemp.Text = Nothing

                txtMaximumPartLengthTemp = CType(gvCatchingAbilityFactor.FooterRow.FindControl("txtFooterMaximumPartLength"), TextBox)
                txtMaximumPartLengthTemp.Text = Nothing

                cbIsSideBySideTemp = CType(gvCatchingAbilityFactor.FooterRow.FindControl("cbFooterIsSideBySide"), CheckBox)
                cbIsSideBySideTemp.Checked = False

                txtCatchingAbilityFactorTemp = CType(gvCatchingAbilityFactor.FooterRow.FindControl("txtFooterCatchingAbilityFactor"), TextBox)
                txtCatchingAbilityFactorTemp.Text = Nothing

                cbObsoleteTemp = CType(gvCatchingAbilityFactor.FooterRow.FindControl("cbFooterObsolete"), CheckBox)
                cbObsoleteTemp.Checked = False

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
    Private Property LoadDataEmpty_CatchingAbilityFactor() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CatchingAbilityFactor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CatchingAbilityFactor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CatchingAbilityFactor") = value
        End Set

    End Property
    Protected Sub odsCatchingAbilityFactor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCatchingAbilityFactor.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CatchingAbilityFactor_MaintDataTable = CType(e.ReturnValue, Costing.CatchingAbilityFactor_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CatchingAbilityFactor = True
            Else
                LoadDataEmpty_CatchingAbilityFactor = False
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
    Protected Sub gvCatchingAbilityFactor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCatchingAbilityFactor.RowCreated

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

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CatchingAbilityFactor
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
#End Region
End Class
