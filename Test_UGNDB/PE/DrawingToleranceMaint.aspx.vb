' ************************************************************************************************
'
' Name:		DrawingToleranceMaint.aspx
' Purpose:	This Code Behind is for the admin page of the PE Drawings Management System Tolerance Dropdown boxes
'
' Date		    Author	    
' 2008-09-09	RCarlson			Created .Net application
' ************************************************************************************************
Partial Class DrawingToleranceMaint
    Inherits System.Web.UI.Page
    Private Sub EnableControls()

        Try
            gvTolerance.Columns(14).Visible = ViewState("isAdmin")

            If gvTolerance.FooterRow IsNot Nothing Then
                gvTolerance.FooterRow.Visible = ViewState("isAdmin")
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

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 43)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            If iRoleID = 11 Then ' ADMIN RIGHTS                                                               
                                ViewState("isAdmin") = True
                            End If
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
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Tolerance"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Drawing Management</b> > > Tolerance"

                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("DMGExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            If Not Page.IsPostBack Then
                If HttpContext.Current.Request.QueryString("ToleranceName") <> "" Then
                    txtSearchToleranceName.Text = Server.UrlDecode(HttpContext.Current.Request.QueryString("ToleranceName"))
                End If
            End If

            CheckRights()
            EnableControls()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvTolerance_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTolerance.RowCommand

        Try
            Dim txtToleranceNameTemp As TextBox

            Dim txtDensityValueTemp As TextBox
            Dim txtDensityToleranceTemp As TextBox
            Dim txtDensityUnitsTemp As TextBox

            Dim txtThicknessValueTemp As TextBox
            Dim txtThicknessToleranceTemp As TextBox
            Dim txtThicknessUnitsTemp As TextBox

            Dim txtWMDValueTemp As TextBox
            Dim txtWMDToleranceTemp As TextBox
            Dim ddWMDUnitsTemp As DropDownList

            Dim txtAMDValueTemp As TextBox
            Dim txtAMDToleranceTemp As TextBox
            Dim ddAMDUnitsTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then

                txtToleranceNameTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterToleranceName"), TextBox)

                txtDensityValueTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterDensityValue"), TextBox)
                txtDensityToleranceTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterDensityTolerance"), TextBox)
                txtDensityUnitsTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterDensityUnits"), TextBox)

                txtThicknessValueTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterThicknessValue"), TextBox)
                txtThicknessToleranceTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterThicknessTolerance"), TextBox)
                txtThicknessUnitsTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterThicknessUnits"), TextBox)

                txtWMDValueTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterWMDValue"), TextBox)
                txtWMDToleranceTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterWMDTolerance"), TextBox)
                ddWMDUnitsTemp = CType(gvTolerance.FooterRow.FindControl("ddFooterWMDUnits"), DropDownList)

                txtAMDValueTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterAMDValue"), TextBox)
                txtAMDToleranceTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterAMDTolerance"), TextBox)
                ddAMDUnitsTemp = CType(gvTolerance.FooterRow.FindControl("ddFooterAMDUnits"), DropDownList)

                odsTolerance.InsertParameters("ToleranceName").DefaultValue = txtToleranceNameTemp.Text

                odsTolerance.InsertParameters("DensityValue").DefaultValue = txtDensityValueTemp.Text
                odsTolerance.InsertParameters("DensityTolerance").DefaultValue = txtDensityToleranceTemp.Text
                odsTolerance.InsertParameters("DensityUnits").DefaultValue = txtDensityUnitsTemp.Text

                odsTolerance.InsertParameters("ThicknessValue").DefaultValue = txtThicknessValueTemp.Text
                odsTolerance.InsertParameters("ThicknessTolerance").DefaultValue = txtThicknessToleranceTemp.Text
                odsTolerance.InsertParameters("ThicknessUnits").DefaultValue = txtThicknessUnitsTemp.Text

                odsTolerance.InsertParameters("WMDValue").DefaultValue = txtWMDValueTemp.Text
                odsTolerance.InsertParameters("WMDTolerance").DefaultValue = txtWMDToleranceTemp.Text
                odsTolerance.InsertParameters("WMDUnits").DefaultValue = ddWMDUnitsTemp.SelectedValue

                odsTolerance.InsertParameters("AMDValue").DefaultValue = txtAMDValueTemp.Text
                odsTolerance.InsertParameters("AMDTolerance").DefaultValue = txtAMDToleranceTemp.Text
                odsTolerance.InsertParameters("AMDUnits").DefaultValue = ddAMDUnitsTemp.SelectedValue

                intRowsAffected = odsTolerance.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvTolerance.ShowFooter = False
            Else
                gvTolerance.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtToleranceNameTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterToleranceName"), TextBox)
                txtToleranceNameTemp.Text = Nothing

                txtDensityValueTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterDensityValue"), TextBox)
                txtDensityValueTemp.Text = Nothing

                txtDensityToleranceTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterDensityTolerance"), TextBox)
                txtDensityToleranceTemp.Text = Nothing

                txtThicknessUnitsTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterThicknessUnits"), TextBox)
                txtThicknessUnitsTemp.Text = Nothing

                txtThicknessValueTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterThicknessValue"), TextBox)
                txtThicknessValueTemp.Text = Nothing

                txtThicknessToleranceTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterThicknessTolerance"), TextBox)
                txtThicknessToleranceTemp.Text = Nothing

                txtThicknessUnitsTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterThicknessUnits"), TextBox)
                txtThicknessUnitsTemp.Text = Nothing

                txtWMDValueTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterWMDValue"), TextBox)
                txtWMDValueTemp.Text = Nothing

                txtWMDToleranceTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterWMDTolerance"), TextBox)
                txtWMDToleranceTemp.Text = Nothing

                ddWMDUnitsTemp = CType(gvTolerance.FooterRow.FindControl("ddFooterWMDUnits"), DropDownList)
                ddWMDUnitsTemp.SelectedValue = Nothing

                txtAMDValueTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterAMDValue"), TextBox)
                txtAMDValueTemp.Text = Nothing

                txtAMDToleranceTemp = CType(gvTolerance.FooterRow.FindControl("txtFooterAMDTolerance"), TextBox)
                txtAMDToleranceTemp.Text = Nothing

                ddAMDUnitsTemp = CType(gvTolerance.FooterRow.FindControl("ddFooterAMDUnits"), DropDownList)
                ddAMDUnitsTemp.SelectedValue = Nothing
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
            Response.Redirect("DrawingToleranceMaint.aspx?ToleranceName=" & Server.UrlEncode(txtSearchToleranceName.Text.Trim), False)
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
            Response.Redirect("DrawingToleranceMaint.aspx", False)
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

    Private Property LoadDataEmpty_Tolerance() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Tolerance") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Tolerance"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Tolerance") = value
        End Set

    End Property

    Protected Sub odsTolerance_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsTolerance.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Drawings.DrawingTolerance_MaintDataTable = CType(e.ReturnValue, Drawings.DrawingTolerance_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Tolerance = True
            Else
                LoadDataEmpty_Tolerance = False
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

    Protected Sub gvTolerance_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTolerance.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Tolerance
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
