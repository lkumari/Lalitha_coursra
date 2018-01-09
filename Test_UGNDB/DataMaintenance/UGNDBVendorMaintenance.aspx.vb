' ************************************************************************************************
'
' Name:		UGNDBVendorMaintenance.aspx
' Purpose:	This Code Behind is to maintain the overhead used by the Costing Module
'
' Date		Author	    
' 02/19/2009 Roderick Carlson
' 08/28/2009 Roderick Carlson - adjusted future vendor list
' 08/25/2010 Roderick Carlson - adjusted extra isActiveBPCSOnly parameter
' 01/03/2014 LREY   Replaced SupplierNo with SupplierNo
' ************************************************************************************************

Partial Class UGNDBVendorMaintenance
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "UGNDB Vendor Maintenance"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > UGNDB Vendor Maintenance"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            Dim localServiceReference As New ServiceReference
            localServiceReference.Path = "~/AutoComplete.asmx"

            Dim mpScriptManager As ScriptManager
            mpScriptManager = CType(Master.FindControl("ScriptManager1"), ScriptManager)
            mpScriptManager.Services.Add(localServiceReference)

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("DMExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            CheckRights()

            If Not Page.IsPostBack Then

                BindCriteria()

                ' ''******
                ''get saved value of past search criteria or query string, query string takes precedence
                ' ''******

                If HttpContext.Current.Request.QueryString("UGNDBVendorID") IsNot Nothing Then
                    ddSearchVendorValue.SelectedValue = HttpContext.Current.Request.QueryString("UGNDBVendorID")
                End If

                If HttpContext.Current.Request.QueryString("SupplierNo") IsNot Nothing Then
                    txtSearchSupplierNoValue.Text = HttpContext.Current.Request.QueryString("SupplierNo")
                End If

                If HttpContext.Current.Request.QueryString("SupplierName") IsNot Nothing Then
                    txtSearchSupplierNameValue.Text = HttpContext.Current.Request.QueryString("SupplierName")
                End If

            End If

            EnableControls()

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br>" & mb.Name

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

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 73)

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
            lblMessage.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ''bind existing data to drop down Program 
            ds = commonFunctions.GetUGNDBVendor(0, "", "", False)
            If commonFunctions.CheckDataSet(ds) = True Then

                ddSearchVendorValue.DataSource = ds
                ddSearchVendorValue.DataTextField = ds.Tables(0).Columns("ddSupplierName").ColumnName.ToString()
                ddSearchVendorValue.DataValueField = ds.Tables(0).Columns("UGNDBVendorID").ColumnName
                ddSearchVendorValue.DataBind()
                ddSearchVendorValue.Items.Insert(0, "")

            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub EnableControls()

        Try
            lblReview1.Visible = Not ViewState("isRestricted")
            btnAdd.Visible = Not ViewState("isRestricted")
            lblReview2.Visible = Not ViewState("isRestricted")
            gvUGNDBVendor.Visible = Not ViewState("isRestricted")
            lblSearchTip.Visible = Not ViewState("isRestricted")
            lblSearchVendorLabel.Visible = Not ViewState("isRestricted")
            ddSearchVendorValue.Visible = Not ViewState("isRestricted")
            lblSearchSupplierNoLabel.Visible = Not ViewState("isRestricted")
            txtSearchSupplierNoValue.Visible = Not ViewState("isRestricted")
            lblSearchSupplierNameLabel.Visible = Not ViewState("isRestricted")
            txtSearchSupplierNameValue.Visible = Not ViewState("isRestricted")
            btnReset.Visible = Not ViewState("isRestricted")
            btnSearch.Visible = Not ViewState("isRestricted")

            If ViewState("isRestricted") = False Then
                gvUGNDBVendor.Columns(gvUGNDBVendor.Columns.Count - 1).Visible = ViewState("isAdmin")
                btnAdd.Enabled = ViewState("isAdmin")
            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Dim strSearchName As String = txtSearchSupplierNameValue.Text.Trim
            Dim iFirstPipeLocation As Integer = 0
            Dim iSecondPipeLocation As Integer = 0
            Dim strAfterFirstPipe As String = ""

            If ddSearchVendorValue.SelectedIndex > 0 Then
                strSearchName = ""
                txtSearchSupplierNoValue.Text = ""
            End If

            If strSearchName <> "" Then
                iFirstPipeLocation = InStr(strSearchName, "|")
                If iFirstPipeLocation > 0 Then
                    strAfterFirstPipe = Mid(strSearchName, iFirstPipeLocation + 2)
                    iSecondPipeLocation = InStr(strAfterFirstPipe, "|")
                    If iSecondPipeLocation > 0 Then
                        strSearchName = Mid(strSearchName, iFirstPipeLocation + iSecondPipeLocation + 2).Trim
                    End If
                End If
            End If

            'Response.Redirect("UGNDBVendorMaintenance.aspx?SupplierNo=" & ddSearchBPCSVendorValue.SelectedValue & "&SupplierName=" & Server.UrlEncode(strSearchName), False)
            Response.Redirect("UGNDBVendorMaintenance.aspx?UGNDBVendorID=" & ddSearchVendorValue.SelectedValue & "&SupplierNo=" & txtSearchSupplierNoValue.Text.Trim & "&SupplierName=" & strSearchName, False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try

            Response.Redirect("UGNDBVendorMaintenance.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvUGNDBVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvUGNDBVendor.DataBound

        'hide header of first column
        If gvUGNDBVendor.Rows.Count > 0 Then
            gvUGNDBVendor.HeaderRow.Cells(0).Visible = False
        End If

    End Sub

    Protected Sub gvUGNDBVendor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUGNDBVendor.RowCreated

        'hide first column
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
        End If

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Try
            lblMessage.Text = ""
    
            If txtSearchSupplierNameValue.Text.Trim <> "" Then
                ddSearchVendorValue.SelectedIndex = -1
                txtSearchSupplierNoValue.Text = ""
                commonFunctions.InsertUGNDBVendor(txtSearchSupplierNameValue.Text.Trim)
                Response.Redirect("UGNDBVendorMaintenance.aspx?SupplierName=" & txtSearchSupplierNameValue.Text.Trim, False)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
