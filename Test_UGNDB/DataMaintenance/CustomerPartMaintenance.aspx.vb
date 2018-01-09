' ************************************************************************************************
' Name:	CustomerPartMaintenance.aspx.vb
' Purpose:	This program is used to view Customer Part Numbers
'
' Date		    Author	    
' 04/2008       RCarlson			Created .Net application
' 07/22/2008    RCarlson            Cleaned Up Error Trapping
' 01/08/2014    LRey                Disabled GetCABBV. CABBV is not used in new ERP.

Partial Class DataMaintenance_CustomerPartMaintenance
    Inherits System.Web.UI.Page
    'Protected Sub CheckRights()

    '    Try
    '        'default to hide edit column
    '        gvCustomerPartList.Columns(7).Visible = False

    '        ''*******
    '        '' Get current Team Member's TeamMemberID from Team_Member_Maint table
    '        ''*******
    '        Dim strFullName As String = commonFunctions.getUserName()
    '        Dim dsTeamMember As DataSet
    '        Dim dsRoleForm As DataSet

    '        Dim iTeamMemberID As Integer = 0
    '        Dim iRoleID As Integer = 0

    '        ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
    '        dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

    '        If dsTeamMember IsNot Nothing Then
    '            If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
    '                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

    '                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 21)

    '                If dsRoleForm IsNot Nothing Then
    '                    If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
    '                        iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

    '                        If iRoleID = 11 Then ' ADMIN RIGHTS                                
    '                            gvCustomerPartList.Columns(7).Visible = True
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
            m.ContentLabel = "Customer Part Numbers in Future 3"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Customer Part Numbers in Future 3"
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
                If Request.QueryString("bpcsPartNo") IsNot Nothing Then
                    txtBPCSPartNoSearch.Text = Server.UrlDecode(Request.QueryString("bpcsPartNo"))
                End If

                If Request.QueryString("customerPartNo") IsNot Nothing Then
                    txtCustomerPartNoSearch.Text = Server.UrlDecode(Request.QueryString("customerPartNo"))
                End If

                If Request.QueryString("customerPartName") IsNot Nothing Then
                    txtCustomerPartNameSearch.Text = Server.UrlDecode(Request.QueryString("customerPartName"))
                End If

                If Request.QueryString("cabbv") IsNot Nothing Then
                    ddCABBVSearch.SelectedValue = Server.UrlDecode(Request.QueryString("cabbv"))
                End If

                If Request.QueryString("barCodePartNo") IsNot Nothing Then
                    txtBarCodePartNoSearch.Text = Server.UrlDecode(Request.QueryString("barCodePartNo"))
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
    Protected Sub BindCriteria()

        Try
            Dim ds As DataSet

            ''bind existing data to drop down CABBV control for selection criteria
            '(LREY) 01/08/2014
            ds = commonFunctions.GetCABBV()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCABBVSearch.DataSource = ds
                ddCABBVSearch.DataTextField = ds.Tables(0).Columns("CustomerNameCombo").ColumnName.ToString()
                ddCABBVSearch.DataValueField = ds.Tables(0).Columns("CABBV").ColumnName
                ddCABBVSearch.DataBind()
                ddCABBVSearch.Items.Insert(0, "")
                ddCABBVSearch.SelectedIndex = 0
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
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            Response.Redirect("CustomerPartMaintenance.aspx", False)
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
            'Response.Redirect("CustomerPartMaintenance.aspx?bpcsPartNo=" & Server.UrlEncode(txtBPCSPartNoSearch.Text.Trim) & "&customerPartNo=" & Server.UrlEncode(txtCustomerPartNoSearch.Text.Trim) & "&customerPartName=" & Server.UrlEncode(txtCustomerPartNameSearch.Text.Trim) & "&cabbv=" & Server.UrlEncode(ddCABBVSearch.SelectedValue) & "&dabbv=" & Server.UrlEncode(ddDABBVSearch.SelectedValue) & "&designLevel=" & Server.UrlEncode(txtDesignLevelSearch.Text.Trim), False)
            Response.Redirect("CustomerPartMaintenance.aspx?bpcsPartNo=" & txtBPCSPartNoSearch.Text.Trim & "&customerPartNo=" & txtCustomerPartNoSearch.Text.Trim & "&customerPartName=" & txtCustomerPartNameSearch.Text.Trim & "&cabbv=" & ddCABBVSearch.SelectedValue & "&barCodePartNo=" & txtBarCodePartNoSearch.Text.Trim, False)
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
