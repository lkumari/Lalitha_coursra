''******************************************************************************************************
''* SupplierRequiredFormsMaint.aspx.vb
''* The purpose of this page is to allow users to maintain and insert new Supplier_Required_Forms_Maint data.
''*
''* Author  : LRey 09/22/2010
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Partial Class Supplier_Required_Forms_Maint
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Supplier Required Forms"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > Supplier Required Forms"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("PURExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then

                BindCriteria()

                If Request.QueryString("pFN") IsNot Nothing Then
                    txtFormName.Text = Server.UrlDecode(Request.QueryString("pFN").ToString)
                End If

                If Request.QueryString("pVT") IsNot Nothing Then
                    ddVendorType.SelectedValue = Server.UrlDecode(Request.QueryString("pVT").ToString)
                End If
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF Page_Load
    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet
            ''bind existing data to drop down Vendor Type control for selection criteria for search
            ds = commonFunctions.GetVendorType(False)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddVendorType.DataSource = ds
                ddVendorType.DataTextField = ds.Tables(0).Columns("ddVType").ColumnName.ToString()
                ddVendorType.DataValueField = ds.Tables(0).Columns("VType").ColumnName.ToString()
                ddVendorType.DataBind()
                ddVendorType.Items.Insert(0, "")
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

    End Sub 'EOF BindCriteria

#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on 
        '' TM() 's Security/Subscription
        ''********

        Try
            ''*******
            '' Disable controls by default
            ''*******
            gvSupplierRequiredForms.Columns(6).Visible = False
            gvSupplierRequiredForms.ShowFooter = False
            ViewState("ObjectRole") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 113 'SupplierRequiredFormsMaint form id
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
                                        gvSupplierRequiredForms.Columns(6).Visible = True
                                        gvSupplierRequiredForms.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        gvSupplierRequiredForms.Columns(6).Visible = True
                                        gvSupplierRequiredForms.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        gvSupplierRequiredForms.Columns(6).Visible = True
                                        gvSupplierRequiredForms.ShowFooter = True
                                        ViewState("ObjectRole") = False
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        gvSupplierRequiredForms.Columns(6).Visible = False
                                        gvSupplierRequiredForms.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        gvSupplierRequiredForms.Columns(6).Visible = True
                                        gvSupplierRequiredForms.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        gvSupplierRequiredForms.Columns(6).Visible = False
                                        gvSupplierRequiredForms.ShowFooter = False
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
#End Region

    Protected Sub gvSupplierRequiredForms_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        ''***
        ''This section allows the inserting of a new row when save button is 
        ''clicked from the footer.
        ''***
        If e.CommandName = "Insert" Then
            ''Insert data
            Dim FormName As TextBox
            Dim VendorType As DropDownList
            Dim RequiredForm As CheckBox

            If gvSupplierRequiredForms.Rows.Count = 0 Then
                '' We are inserting through the DetailsView in the EmptyDataTemplate
                Return
            End If

            '' Only perform the following logic when inserting through the footer
            FormName = CType(gvSupplierRequiredForms.FooterRow.FindControl("txtForm"), TextBox)
            odsSupplierRequiredForms.InsertParameters("FormName").DefaultValue = FormName.Text

            VendorType = CType(gvSupplierRequiredForms.FooterRow.FindControl("ddVTypeInsert"), DropDownList)
            odsSupplierRequiredForms.InsertParameters("VendorType").DefaultValue = VendorType.Text


            RequiredForm = CType(gvSupplierRequiredForms.FooterRow.FindControl("chkReqFormInsert"), CheckBox)
            odsSupplierRequiredForms.InsertParameters("RequiredForm").DefaultValue = RequiredForm.Checked

            odsSupplierRequiredForms.Insert()
        End If

        ''***
        ''This section allows show/hides the footer row when the Edit control is clicked
        ''***
        If e.CommandName = "Edit" Then
            gvSupplierRequiredForms.ShowFooter = False
        Else
            If ViewState("ObjectRole") = True Then
                gvSupplierRequiredForms.ShowFooter = True
            Else
                gvSupplierRequiredForms.ShowFooter = False
            End If
        End If

        ''***
        ''This section clears out the values in the footer row
        ''***
        If e.CommandName = "Undo" Then
            Dim FormName As TextBox
            Dim VendorType As DropDownList
            Dim RequiredForm As CheckBox

            FormName = CType(gvSupplierRequiredForms.FooterRow.FindControl("txtForm"), TextBox)
            FormName.Text = Nothing

            VendorType = CType(gvSupplierRequiredForms.FooterRow.FindControl("ddVTypeInsert"), DropDownList)
            VendorType.Text = Nothing

            RequiredForm = CType(gvSupplierRequiredForms.FooterRow.FindControl("chkReqFormInsert"), CheckBox)
            RequiredForm.Checked = False
        End If
    End Sub 'EOF gvSupplier Required Forms_RowCommand

    Protected Sub gvSupplierRequiredForms_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSupplierRequiredForms.DataBound
        If (SendUserToLastPage) Then
            gvSupplierRequiredForms.PageIndex = gvSupplierRequiredForms.PageCount - 1
        End If
    End Sub 'EOF gvSupplier Required Forms_DataBound

#Region "Insert Empty GridView Work-Around"
    Private Property LoadDataEmpty_Form() As Boolean

        Get
            If ViewState("LoadDataEmpty_Form") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Form"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Form") = value
        End Set
    End Property 'EOF LoadDataEmpty_Form()

    Protected Sub gvSupplierRequiredForms_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSupplierRequiredForms.RowCreated

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Form
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If
    End Sub 'EOF gvSupplierRequiredForms_RowCreated

    Protected Sub odsSupplierRequiredForms_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsSupplierRequiredForms.Selected

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)
        Dim dt As Supplier.Supplier_Required_FormsDataTable = CType(e.ReturnValue, Supplier.Supplier_Required_FormsDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty_Form = True
        Else
            LoadDataEmpty_Form = False
        End If
    End Sub 'EOF odsSupplierRequiredForms_Selected
#End Region 'EOF "Insert Empty GridView Work-Around"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Response.Redirect("SupplierRequiredFormsMaint.aspx?pFN=" & Server.UrlEncode(txtFormName.Text.Trim) & "&pVT=" & Server.UrlEncode(ddVendorType.SelectedValue), False)
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Response.Redirect("SupplierRequiredFormsMaint.aspx")
    End Sub
End Class 'EOF btnReset_Click
