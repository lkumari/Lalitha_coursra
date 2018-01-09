''******************************************************************************************************
''* Future_Part_Maint.aspx.vb
''* The purpose of this page is to allow users to maintain and insert new Future_Part_Maint data.
''*
''* Author  : LRey 03/12/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************
#Region "Directives"

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Text

#End Region
Partial Class PF_Future_PartNo
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Future Customer Part Numbers"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Planning and Forecasting </b> > Future Customer Part Numbers"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("PFExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            If Not Page.IsPostBack Then
                ViewState("sPartNo") = ""
                ViewState("sPartDesc") = ""
                ViewState("sCreatedBy") = ""

                ''Build drop down list values
                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("PF_txtPartNo") Is Nothing Then
                    txtPartNo.Text = Server.HtmlEncode(Request.Cookies("PF_txtPartNo").Value)
                    ViewState("sPartNo") = Server.HtmlEncode(Request.Cookies("PF_txtPartNo").Value)
                End If

                If Not Request.Cookies("PF_txtPartDesc") Is Nothing Then
                    txtPartDesc.Text = Server.HtmlEncode(Request.Cookies("PF_txtPartDesc").Value)
                    ViewState("sPartDesc") = Server.HtmlEncode(Request.Cookies("PF_txtPartDesc").Value)
                End If

                If Not Request.Cookies("PF_ddTeamMember") Is Nothing Then
                    ddTeamMember.SelectedValue = Server.HtmlEncode(Request.Cookies("PF_ddTeamMember").Value)
                    ViewState("sCreatedBy") = Server.HtmlEncode(Request.Cookies("PF_ddTeamMember").Value)
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
#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        ''** To disable gridview ibtnDelete control, initialize Visible='<%# ViewState("ObjectRole")%>' in aspx page
        Try
            ''*******
            '' Disable controls by default
            ''*******
            gvFuturePartNo.Columns(0).Visible = False
            gvFuturePartNo.ShowFooter = False
            ViewState("ObjectRole") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 12 'Future Customer Part Numbers form id
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
                                        gvFuturePartNo.Columns(0).Visible = True
                                        gvFuturePartNo.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        gvFuturePartNo.Columns(0).Visible = True
                                        gvFuturePartNo.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        gvFuturePartNo.Columns(0).Visible = True
                                        gvFuturePartNo.ShowFooter = True
                                        ViewState("ObjectRole") = False
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        gvFuturePartNo.Columns(0).Visible = False
                                        gvFuturePartNo.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        gvFuturePartNo.Columns(0).Visible = True
                                        gvFuturePartNo.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        gvFuturePartNo.Columns(0).Visible = False
                                        gvFuturePartNo.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                End Select 'EOF of "Select Case iRoleID"
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub
#End Region

    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down Customer control for selection criteria for search
        ds = PFModule.GetFuturePartNoByCreatedBy("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddTeamMember.DataSource = ds
            ddTeamMember.DataTextField = ds.Tables(0).Columns("EmpName").ColumnName.ToString()
            ddTeamMember.DataValueField = ds.Tables(0).Columns("CreatedBy").ColumnName.ToString()
            ddTeamMember.DataBind()
            ddTeamMember.Items.Insert(0, "")
        End If

        ''
    End Sub
    Protected Sub gvFuturePartNo_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try
            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            Dim PartNo As TextBox
            Dim PartDesc As TextBox
            Dim DesignationType As DropDownList
            Dim UGNFacility As DropDownList
            'Dim OEM As DropDownList
            Dim OEMManufacturer As DropDownList
            lblErrors.Text = Nothing
            lblErrors.Visible = False

            If (e.CommandName = "Insert") Then
                PartNo = CType(gvFuturePartNo.FooterRow.FindControl("txtPartNo"), TextBox)
                odsFuturePartNo.InsertParameters("PartNo").DefaultValue = PartNo.Text

                PartDesc = CType(gvFuturePartNo.FooterRow.FindControl("txtPartDesc"), TextBox)
                odsFuturePartNo.InsertParameters("PartDesc").DefaultValue = PartDesc.Text

                UGNFacility = CType(gvFuturePartNo.FooterRow.FindControl("ddUGNLocation"), DropDownList)
                odsFuturePartNo.InsertParameters("UGNFacility").DefaultValue = UGNFacility.SelectedValue

                'OEM = CType(gvFuturePartNo.FooterRow.FindControl("ddOEM"), DropDownList)
                'odsFuturePartNo.InsertParameters("OEM").DefaultValue = OEM.SelectedValue

                OEMManufacturer = CType(gvFuturePartNo.FooterRow.FindControl("ddOEMMfg"), DropDownList)
                odsFuturePartNo.InsertParameters("OEMManufacturer").DefaultValue = OEMManufacturer.SelectedValue

                DesignationType = CType(gvFuturePartNo.FooterRow.FindControl("ddDesignationType"), DropDownList)
                odsFuturePartNo.InsertParameters("DesignationType").DefaultValue = DesignationType.SelectedValue


                ''Verify that the part number entered is not already in the system, if so prompt message,
                ''else save the new entry.
                Dim ds As DataSet = New DataSet
                ds = commonFunctions.GetPartNo(PartNo.Text, DesignationType.Text, UGNFacility.SelectedValue, "", OEMManufacturer.SelectedValue)
                If (ds.Tables.Item(0).Rows.Count > 0) Then
                    lblErrors.Text = "'" & PartNo.Text & "' part number found in BPCS."
                    lblErrors.Visible = True
                    PartNo.Text = Nothing
                    PartDesc.Text = Nothing
                Else
                    odsFuturePartNo.Insert()
                    '' Indicate that the user needs to be sent to the last page
                    ''SendUserToLastPage = True
                End If
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvFuturePartNo.ShowFooter = False
            Else
                gvFuturePartNo.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                Dim DesignationType2 As DropDownList
                Dim UGNFacility2 As DropDownList
                'Dim OEM2 As DropDownList
                Dim OEMManufacturer2 As DropDownList

                PartNo = CType(gvFuturePartNo.FooterRow.FindControl("txtPartNo"), TextBox)
                PartNo.Text = Nothing

                PartDesc = CType(gvFuturePartNo.FooterRow.FindControl("txtPartDesc"), TextBox)
                PartDesc.Text = Nothing

                UGNFacility2 = CType(gvFuturePartNo.FooterRow.FindControl("ddUGNLocation"), DropDownList)
                UGNFacility2.SelectedValue = Nothing

                'OEM2 = CType(gvFuturePartNo.FooterRow.FindControl("ddOEM"), DropDownList)
                'OEM2.SelectedValue = Nothing

                OEMManufacturer2 = CType(gvFuturePartNo.FooterRow.FindControl("ddOEMMfg"), DropDownList)
                OEMManufacturer2.SelectedValue = Nothing

                DesignationType2 = CType(gvFuturePartNo.FooterRow.FindControl("ddDesignationType"), DropDownList)
                DesignationType2.SelectedValue = Nothing

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Protected Sub gvFuturePartNo_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFuturePartNo.DataBound
        If (SendUserToLastPage) Then
            gvFuturePartNo.PageIndex = gvFuturePartNo.PageCount - 1
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        ''Store search parameters
        Response.Cookies("PF_txtPartNo").Value = txtPartNo.Text
        Response.Cookies("PF_txtPartDesc").Value = txtPartDesc.Text
        Response.Cookies("PF_ddTeamMember").Value = ddTeamMember.SelectedValue

        Response.Redirect("Future_Part_Maint.aspx?sPartNo=" & txtPartNo.Text & "&sPartDesc=" & txtPartDesc.Text & "&sCreatedBy=" & ddTeamMember.SelectedValue, False)
    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        txtPartNo.Text = ""
        txtPartDesc.Text = ""
        ddTeamMember.SelectedValue = ""

        ''******
        '' Delete cookies in search parameters.
        ''******
        PFModule.DeletePFCookies_FuturePartNo()

        Response.Redirect("Future_Part_Maint.aspx", False)
    End Sub

End Class
