' ************************************************************************************************
' Name:	VendorMaintenance.aspx.vb
' Purpose:	This program is used to view vendor information
'
' Date		    Author	    
' 04/2008       Roderick Carlson			Created .Net application
' 07/22/2008    Roderick Carlson            Cleaned Up Error Trapping
' 07/30/2008    Roderick Carlson            Vendor List was changed to View. No updates are needed. No CheckRights are needed.
' 03/31/2009    Roderick Carlson            Updated GetVendor Parameters

Partial Class DataMaintenance_VendorMaintenance
    Inherits System.Web.UI.Page
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet = New DataSet

            'bind existing Commodity details to gridview for editing
            ds = commonFunctions.GetVendor(0, "", "", "", "", "", "", "", "")
            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddVendorIDSearch.DataSource = ds
                    ddVendorIDSearch.DataTextField = ds.Tables(0).Columns("ddVNDNAMcombo").ColumnName
                    ddVendorIDSearch.DataValueField = ds.Tables(0).Columns("VENDOR").ColumnName
                    ddVendorIDSearch.DataBind()
                    ddVendorIDSearch.Items.Insert(0, "")
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
    Protected Sub CheckRights()

        'Try
        '    'default to hide edit column
        '    'gvVendorList.Columns(7).Visible = False

        '    ''*******
        '    '' Get current Team Member's TeamMemberID from Team_Member_Maint table
        '    ''*******
        '    Dim strFullName As String = commonFunctions.getUserName()
        '    Dim dsTeamMember As DataSet
        '    Dim dsRoleForm As DataSet

        '    Dim iTeamMemberID As Integer = 0
        '    Dim iRoleID As Integer = 0

        '    dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        '    ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

        '    If dsTeamMember IsNot Nothing Then
        '        If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
        '            iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

        '            dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 29)

        '            If dsRoleForm IsNot Nothing Then
        '                If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
        '                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

        '                    If iRoleID = 11 Then ' ADMIN RIGHTS                                
        '                        gvVendorList.Columns(7).Visible = True

        '                    End If
        '                End If
        '            End If
        '        End If
        '    End If
        'Catch ex As Exception

        '    'update error on web page
        '    lblMessage.Text = ex.Message

        '    'get current event name
        '    Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

        '    'log and email error
        '    UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        'End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "BPCS Vendor"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > BPCS Vendor"
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

                If Request.QueryString("VendorID") IsNot Nothing Then
                    ddVendorIDSearch.SelectedValue = Server.UrlDecode(Request.QueryString("VendorID").ToString)
                End If

                If Request.QueryString("VendorName") IsNot Nothing Then
                    txtVendorNameSearch.Text = Server.UrlDecode(Request.QueryString("VendorName").ToString)
                End If

                ' ''If Request.QueryString("VendorAddress") IsNot Nothing Then
                ' ''    txtVendorAddressSearch.Text = Server.UrlDecode(Request.QueryString("VendorAddress").ToString)
                ' ''End If

                ' ''If Request.QueryString("VendorState") IsNot Nothing Then
                ' ''    ddVendorStateSearch.SelectedValue = Server.UrlDecode(Request.QueryString("VendorState").ToString)
                ' ''End If

                ' ''If Request.QueryString("VendorZipCode") IsNot Nothing Then
                ' ''    txtVendorZipCodeSearch.Text = Server.UrlDecode(Request.QueryString("VendorZipCode").ToString)
                ' ''End If

                ' ''If Request.QueryString("VendorCountry") IsNot Nothing Then
                ' ''    txtVendorCountrySearch.Text = Server.UrlDecode(Request.QueryString("VendorCountry").ToString)
                ' ''End If

                ' ''If Request.QueryString("VendorPhone") IsNot Nothing Then
                ' ''    txtVendorPhoneSearch.Text = Server.UrlDecode(Request.QueryString("VendorPhone").ToString)
                ' ''End If

                ' ''If Request.QueryString("VendorFAX") IsNot Nothing Then
                ' ''    txtVendorFAXSearch.Text = Server.UrlDecode(Request.QueryString("VendorFAX").ToString)
                ' ''End If
            End If

            'CheckRights()

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
            ' ''Response.Redirect("VendorMaintenance.aspx?VendorID=" & Server.UrlEncode(txtVendorIDSearch.Text.Trim) & "&VendorName=" & Server.UrlEncode(txtVendorNameSearch.Text.Trim) & "&VendorAddress=" & Server.UrlEncode(txtVendorAddressSearch.Text.Trim) & "&VendorState=" & Server.UrlEncode(ddVendorStateSearch.SelectedValue) & "&VendorZipCode=" & Server.UrlEncode(txtVendorZipCodeSearch.Text.Trim) & "&VendorCountry=" & Server.UrlEncode(txtVendorCountrySearch.Text.Trim) & "&VendorPhone=" & Server.UrlEncode(txtVendorPhoneSearch.Text.Trim) & "&VendorFAX=" & Server.UrlEncode(txtVendorFAXSearch.Text.Trim))
            Response.Redirect("VendorMaintenance.aspx?VendorID=" & Server.UrlEncode(ddVendorIDSearch.SelectedValue) & "&VendorName=" & Server.UrlEncode(txtVendorNameSearch.Text.Trim), False)
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
            Response.Redirect("VendorMaintenance.aspx", False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
