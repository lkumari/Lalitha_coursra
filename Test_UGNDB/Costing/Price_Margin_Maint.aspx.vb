' ************************************************************************************************
'
' Name:		Price_Margin_Maint.aspx
' Purpose:	This Code Behind is to maintain the PriceMargin list used by the Costing Module
'
' Date		Author	    
' 5/27/2010 	Roderick Carlson

' ************************************************************************************************
Partial Class Price_Margin_Maint
    Inherits System.Web.UI.Page

    Protected Sub BindCriteria()

        Dim ds As DataSet

        'bind UGN Facility
        ds = commonFunctions.GetUGNFacility("")
        If commonFunctions.CheckDataset(ds) = True Then
            ddUGNFacilityValue.DataSource = ds
            ddUGNFacilityValue.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
            ddUGNFacilityValue.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
            ddUGNFacilityValue.DataBind()
            ddUGNFacilityValue.Items.Insert(0, "")
        End If

    End Sub

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

            If commonFunctions.CheckDataset(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 105)

                If commonFunctions.CheckDataset(dsRoleForm) = True Then

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

            gvPriceMargin.Visible = Not ViewState("isRestricted")
            'lblSearchTip.Visible = False
            lblUGNFacilityLabel.Visible = Not ViewState("isRestricted")
            ddUGNFacilityValue.Visible = Not ViewState("isRestricted")

            btnReset.Visible = Not ViewState("isRestricted")
            btnSearch.Visible = Not ViewState("isRestricted")

            If ViewState("isRestricted") = False Then
                gvPriceMargin.Columns(gvPriceMargin.Columns.Count - 1).Visible = ViewState("isAdmin")
                If gvPriceMargin.FooterRow IsNot Nothing Then
                    gvPriceMargin.FooterRow.Visible = ViewState("isAdmin")
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Price Margin Maintenance"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > Price Margin Maintenance "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If


            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            'testMasterPanel = CType(Master.FindControl("CostingExtender"), CollapsiblePanelExtender)
            testMasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            CheckRights()

            If Not Page.IsPostBack Then
                BindCriteria()

                'clear crystal reports
                CostingModule.CleanCostingCrystalReports()

                ' ''******
                ''get saved value of past search criteria or query string, query string takes precedence
                ' ''******

                If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                    ddUGNFacilityValue.SelectedValue = HttpContext.Current.Request.QueryString("UGNFacility")
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try

            Response.Redirect("Price_Margin_Maint.aspx?UGNFacility=" & ddUGNFacilityValue.SelectedValue, False)

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

            Response.Redirect("Price_Margin_Maint.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvPriceMargin_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPriceMargin.DataBound

        'hide header of first column
        If gvPriceMargin.Rows.Count > 0 Then
            gvPriceMargin.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvPriceMargin_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPriceMargin.RowCommand

        Try

            Dim ddTempUGNFacility As DropDownList
            Dim txtTempMinPriceMargin As TextBox

            Dim dTempMinPriceMargin As Double = 0
            Dim strUGNFacility As String = ""

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddTempUGNFacility = CType(gvPriceMargin.FooterRow.FindControl("ddFooterUGNFacility"), DropDownList)
                txtTempMinPriceMargin = CType(gvPriceMargin.FooterRow.FindControl("txtFooterMinPriceMargin"), TextBox)

                If txtTempMinPriceMargin.Text.Trim <> "" Then
                    dTempMinPriceMargin = CType(txtTempMinPriceMargin.Text.Trim, Double)
                End If

                If ddTempUGNFacility.SelectedIndex > 0 Then
                    strUGNFacility = ddTempUGNFacility.SelectedValue
                Else
                    strUGNFacility = "UN"
                End If

                If dTempMinPriceMargin > 0 Then
                    odsPriceMargin.InsertParameters("UGNFacility").DefaultValue = strUGNFacility
                    odsPriceMargin.InsertParameters("MinPriceMargin").DefaultValue = dTempMinPriceMargin

                    intRowsAffected = odsPriceMargin.Insert()
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvPriceMargin.ShowFooter = False
            Else
                gvPriceMargin.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddTempUGNFacility = CType(gvPriceMargin.FooterRow.FindControl("ddFooterUGNFacility"), DropDownList)
                ddTempUGNFacility.SelectedIndex = -1

                txtTempMinPriceMargin = CType(gvPriceMargin.FooterRow.FindControl("txtFooterMinPriceMargin"), TextBox)
                txtTempMinPriceMargin.Text = ""
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
    Private Property LoadDataEmpty_PriceMargin() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_PriceMargin") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_PriceMargin"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_PriceMargin") = value
        End Set

    End Property
    Protected Sub odsPriceMargin_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsPriceMargin.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetPriceMargin_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetPriceMargin_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt IsNot Nothing Then
                If dt.Rows.Count = 0 Then
                    dt.Rows.Add(dt.NewRow())
                    LoadDataEmpty_PriceMargin = True
                Else
                    LoadDataEmpty_PriceMargin = False
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
    Protected Sub gvPriceMargin_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPriceMargin.RowCreated

        Try
            'hide first column
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_PriceMargin
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
End Class
