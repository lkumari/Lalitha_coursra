' ************************************************************************************************
' Name:	CustomerMaintenance.aspx.vb
' Purpose:	This program is used to view Customers. Add future customer via SQL
'
' Date		    Author	    
' 04/2008       RCarlson			Created .Net application
' 07/22/2008    RCarlson            Cleaned Up Error Trapping
' 08/20/2008    RCarlson            Views are used and therefore no security is needed
' 01/08/2014    LRey                GetCABBV, GetCustomerDestination, GetShiptTo, GetSoldTo will not be need used in new ERP system
' ************************************************************************************************

Partial Class DataMaintenance_CustomerMaintenance
    Inherits System.Web.UI.Page
    'Protected Sub CheckRights()

    '    Try
    '        'default to hide edit column
    '        gvCustomerList.Columns(16).Visible = False

    '        ''*******
    '        '' Get current Team Member's TeamMemberID from Team_Member_Maint table
    '        ''*******
    '        Dim strFullName As String = commonFunctions.getUserName()
    '        Dim dsTeamMember As DataSet
    '        Dim dsRoleForm As DataSet

    '        Dim iTeamMemberID As Integer = 0
    '        Dim iRoleID As Integer = 0

    '        dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
    '        ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

    '        If dsTeamMember IsNot Nothing Then
    '            If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
    '                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

    '                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 20)

    '                If dsRoleForm IsNot Nothing Then
    '                    If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
    '                        iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

    '                        If iRoleID = 11 Then ' ADMIN RIGHTS                                
    '                            gvCustomerList.Columns(16).Visible = True
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text = ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Customer"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Customer"
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

                ''*************************************************
                ''Check QueryStrings
                ''*************************************************
                If Request.QueryString("compny") IsNot Nothing Then
                    ddCOMPNY.SelectedValue = Server.UrlDecode(Request.QueryString("compny"))
                End If

                If Request.QueryString("oem") IsNot Nothing Then
                    ddOEM.SelectedValue = Server.UrlDecode(Request.QueryString("oem"))
                End If

                If Request.QueryString("cabbv") IsNot Nothing Then
                    ddCABBV.SelectedValue = Server.UrlDecode(Request.QueryString("cabbv"))
                End If

                'If Request.QueryString("dabbv") IsNot Nothing Then
                '    ddDABBV.SelectedValue = Server.UrlDecode(Request.QueryString("dabbv"))
                'End If

                'If Request.QueryString("shipto") IsNot Nothing Then
                '    ddShipTo.SelectedValue = Server.UrlDecode(Request.QueryString("shipto"))
                'End If

                'If Request.QueryString("soldto") IsNot Nothing Then
                '    ddSoldTo.SelectedValue = Server.UrlDecode(Request.QueryString("soldto"))
                'End If
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
    Protected Sub BindCriteria()

        Try
            Dim ds As DataSet
            ''bind existing data to drop down OEM control for selection criteria 
            ds = commonFunctions.GetUGNFacility("")

            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddCOMPNY.DataSource = ds
                    ddCOMPNY.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                    ddCOMPNY.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                    ddCOMPNY.DataBind()
                    ddCOMPNY.Items.Insert(0, "")
                    ddCOMPNY.SelectedIndex = 0
                End If
            End If

            ''bind existing data to drop down OEM control for selection criteria 
            ds = commonFunctions.GetOEM()

            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddOEM.DataSource = ds
                    ddOEM.DataTextField = ds.Tables(0).Columns("OEM").ColumnName.ToString()
                    ddOEM.DataValueField = ds.Tables(0).Columns("OEM").ColumnName
                    ddOEM.DataBind()
                    ddOEM.Items.Insert(0, "")
                    ddOEM.SelectedIndex = 0
                End If
            End If

            ''bind existing data to drop down CABBV control for selection criteria 
            '(LREY) 01/08/2014
            ds = commonFunctions.GetCABBV

            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddCABBV.DataSource = ds
                    ddCABBV.DataTextField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()
                    ddCABBV.DataValueField = ds.Tables(0).Columns("CABBV").ColumnName
                    ddCABBV.DataBind()
                    ddCABBV.Items.Insert(0, "")
                    ddCABBV.SelectedIndex = 0
                End If
            End If

            ''bind existing data to drop down DABBV control for selection criteria 
            '(LREY) 01/08/2014
            'ds = commonFunctions.GetCustomerDestination("")

            'If ds IsNot Nothing Then
            '    If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
            '        ddDABBV.DataSource = ds
            '        ddDABBV.DataTextField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
            '        ddDABBV.DataValueField = ds.Tables(0).Columns("DABBV").ColumnName
            '        ddDABBV.DataBind()
            '        ddDABBV.Items.Insert(0, "")
            '        ddDABBV.SelectedIndex = 0
            '    End If
            'End If

            ''bind existing data to drop down ShipTo control for selection criteria 
            '(LREY) 01/08/2014
            'ds = commonFunctions.GetShipTo()
            'If ds IsNot Nothing Then
            '    If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
            '        ddShipTo.DataSource = ds
            '        ddShipTo.DataTextField = ds.Tables(0).Columns("ShipTo").ColumnName.ToString()
            '        ddShipTo.DataValueField = ds.Tables(0).Columns("ShipTo").ColumnName
            '        ddShipTo.DataBind()
            '        ddShipTo.Items.Insert(0, "")
            '        ddShipTo.SelectedIndex = 0
            '    End If
            'End If

            ''bind existing data to drop down SoldTo control for selection criteria 
            '(LREY) 01/08/2014
            'ds = commonFunctions.GetSoldTo()
            'If ds IsNot Nothing Then
            '    If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
            '        ddSoldTo.DataSource = ds
            '        ddSoldTo.DataTextField = ds.Tables(0).Columns("SoldTo").ColumnName.ToString()
            '        ddSoldTo.DataValueField = ds.Tables(0).Columns("SoldTo").ColumnName
            '        ddSoldTo.DataBind()
            '        ddSoldTo.Items.Insert(0, "")
            '        ddSoldTo.SelectedIndex = 0
            '    End If
            'End If
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
            Response.Redirect("CustomerMaintenance.aspx", False)
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
            'Response.Redirect("CustomerMaintenance.aspx?compny=" & Server.UrlEncode(ddCOMPNY.SelectedValue) & "&oem=" & Server.UrlEncode(ddOEM.SelectedValue) & "&cabbv=" & Server.UrlEncode(ddCABBV.SelectedValue) & "&dabbv=" & Server.UrlEncode(ddDABBV.SelectedValue) & "&shipto=" & Server.UrlEncode(ddShipTo.SelectedValue) & "&soldto=" & Server.UrlEncode(ddSoldTo.SelectedValue), False)
            Response.Redirect("CustomerMaintenance.aspx?compny=" & Server.UrlEncode(ddCOMPNY.SelectedValue) & "&oem=" & Server.UrlEncode(ddOEM.SelectedValue) & "&cabbv=" & Server.UrlEncode(ddCABBV.SelectedValue) & "&dabbv=" & "" & "&shipto=" & "" & "&soldto=" & "", False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    'Protected Sub ddCABBV_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCABBV.SelectedIndexChanged

    '    Try
    '        Dim ds As DataSet

    '        ''bind existing data to drop down DABBV control for selection criteria 
    '        '(LREY) 01/07/0214
    '        'ds = commonFunctions.GetCustomerDestination(ddCABBV.SelectedValue)

    '        'If ds IsNot Nothing Then
    '        '    If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
    '        '        ddDABBV.DataSource = ds
    '        '        ddDABBV.DataTextField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
    '        '        ddDABBV.DataValueField = ds.Tables(0).Columns("DABBV").ColumnName
    '        '        ddDABBV.DataBind()
    '        '        ddDABBV.Items.Insert(0, "")
    '        '        ddDABBV.SelectedIndex = 0
    '        '    End If
    '        'End If
    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text = ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub
End Class
