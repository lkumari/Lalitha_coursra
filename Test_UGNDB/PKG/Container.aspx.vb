
Partial Class Packaging_Container
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to define the primary record
            If HttpContext.Current.Request.QueryString("pCNO") <> "" Then
                ViewState("pCNO") = HttpContext.Current.Request.QueryString("pCNO")
            Else
                ViewState("pCNO") = Nothing
            End If

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Container Entry"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Packaging</b> > <a href='ContainerList.aspx'><b>Container Search</b></a> > Container Entry"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("PKGExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ''****************************************
                ''Redirect user to the right tab location
                ''****************************************
                If ViewState("pCNO") <> Nothing Then
                    BindCriteria()
                    BindData(ViewState("pCNO"))
                Else
                    BindCriteria()
                    txtDescription.Focus()
                End If
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            ''*************************************************
            '' Initialize maxlength
            ''*************************************************
            txtDescription.Attributes.Add("onkeypress", "return tbLimit();")
            txtDescription.Attributes.Add("onkeyup", "return tbCount(" + lblDescChar.ClientID + ");")
            txtDescription.Attributes.Add("maxLength", "240")

            txtType.Attributes.Add("onkeypress", "return tbLimit();")
            txtType.Attributes.Add("onkeyup", "return tbCount(" + lblTypeChar.ClientID + ");")
            txtType.Attributes.Add("maxLength", "50")

            txtNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNotesChar.ClientID + ");")
            txtNotes.Attributes.Add("maxLength", "200")

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load


#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        Try
            ''*******
            '' Disable controls by default
            ''*******
            btnAdd.Enabled = False
            ViewState("Admin") = False
            ViewState("ObjectRole") = False
            ' ''mnuTabs.Items(1).Enabled = False
            ' ''mnuTabs.Items(2).Enabled = False

            If ViewState("pCNO") <> Nothing Then
                ddColor.Enabled = False
                txtInHeight.Enabled = False
                txtInLength.Enabled = False
                txtInWidth.Enabled = False
                txtOutHeight.Enabled = False
                txtOutLength.Enabled = False
                txtOutWidth.Enabled = False
                accSupplier.Visible = True
                'txtTareWeight.Enabled = False
            Else
                accSupplier.Visible = False
            End If

            gvSupplier.Columns(1).Visible = False
            gvSupplier.ShowFooter = False

            gvCustomer.Columns(1).Visible = False
            gvCustomer.ShowFooter = False

            If txtContainerNo.Text = Nothing Then
                txtContainerNo.Visible = False
                lblContainerNo.Visible = True
            Else
                txtContainerNo.Visible = True
                lblContainerNo.Visible = False
            End If
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 64 'Container Form ID
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
                                For i = 0 To dsRoleForm.Tables(0).Rows.Count - 1
                                    iRoleID = dsRoleForm.Tables(0).Rows(i).Item("RoleID")
                                    Select Case iRoleID
                                        Case 11 '*** UGNAdmin: Full Access
                                            ''Used by full admin such as Developers
                                            ViewState("Admin") = True

                                            btnAdd.Enabled = True
                                            If ViewState("pCNO") = Nothing Then
                                            Else
                                                ViewState("ObjectRole") = True
                                                gvSupplier.Columns(1).Visible = True
                                                gvSupplier.ShowFooter = True
                                                gvCustomer.Columns(1).Visible = True
                                                gvCustomer.ShowFooter = True
                                            End If
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Sales/Program Mgrs, Tooling/Purchasing Leads
                                            ViewState("Admin") = True
                                            btnAdd.Enabled = True
                                            If ViewState("pCNO") <> Nothing Then
                                                ViewState("ObjectRole") = True
                                                gvSupplier.Columns(1).Visible = True
                                                gvSupplier.ShowFooter = True
                                                gvCustomer.Columns(1).Visible = True
                                                gvCustomer.ShowFooter = True
                                            End If
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            '** Used by Backup persons
                                            'N/A
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            'N/A
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ''Used by the Accounting Group
                                            'N/A
                                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                            'N/A
                                    End Select 'EOF of "Select Case iRoleID"
                                Next
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
#End Region 'EOF Security

#Region "Detail"
    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Customer control for selection criteria for search
            ds = PKGModule.GetColor("", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddColor.DataSource = ds
                ddColor.DataTextField = ds.Tables(0).Columns("ddColor").ColumnName.ToString()
                ddColor.DataValueField = ds.Tables(0).Columns("CCode").ColumnName.ToString()
                ddColor.DataBind()
                ddColor.Items.Insert(0, "")
            End If

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindCriteria

    Public Sub BindData(ByVal ContainerNo As String)
        Try
            lblMessage.Text = Nothing
            lblMessage.Visible = False

            Dim ds As DataSet = New DataSet
            If ContainerNo <> Nothing Then
                ds = PKGModule.GetPkgContainer(0, ContainerNo, "", "", "", "", 0)
                If commonFunctions.CheckDataSet(ds) = True Then
                    txtCID.Text = ds.Tables(0).Rows(0).Item("CID").ToString()
                    txtContainerNo.Text = ds.Tables(0).Rows(0).Item("ContainerNo").ToString()
                    hfContainerNo.Value = ds.Tables(0).Rows(0).Item("ContainerNo").ToString()
                    lblContainerNo.Text = ds.Tables(0).Rows(0).Item("ContainerNo").ToString()

                    txtDescription.Text = ds.Tables(0).Rows(0).Item("Desc").ToString()
                    txtType.Text = ds.Tables(0).Rows(0).Item("Type").ToString()
                    cddOEM.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("OEM").ToString()))
                    txtOEMMfg.Text = ds.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
                    cddOEMMfg.SelectedValue = ds.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
                    ddColor.SelectedValue = ds.Tables(0).Rows(0).Item("CCode").ToString()
                    txtInLength.Text = ds.Tables(0).Rows(0).Item("InDimL").ToString()
                    txtInWidth.Text = ds.Tables(0).Rows(0).Item("InDimW").ToString()
                    txtInHeight.Text = ds.Tables(0).Rows(0).Item("InDimH").ToString()
                    txtOutLength.Text = ds.Tables(0).Rows(0).Item("OutDimL").ToString()
                    txtOutWidth.Text = ds.Tables(0).Rows(0).Item("OutDimW").ToString()
                    txtOutHeight.Text = ds.Tables(0).Rows(0).Item("OutDimH").ToString()
                    txtTareWeight.Text = ds.Tables(0).Rows(0).Item("TareWeight").ToString()
                    txtNotes.Text = ds.Tables(0).Rows(0).Item("Notes").ToString()
                    ddObsolete.SelectedValue = ds.Tables(0).Rows(0).Item("Obsolete").ToString()
                End If

            End If 'EOF If ContainerNo <> Nothing Then

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindData

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim OEM As String = commonFunctions.GetCCDValue(cddOEM.SelectedValue)
            Dim OEMMfg As String = commonFunctions.GetCCDValue(cddOEMMfg.SelectedValue)
            Dim UpdateValue As Boolean = False

            lblMessage.Text = Nothing
            lblMessage.Visible = False

            If ViewState("pCNO") <> Nothing Then
                If hfContainerNo.Value <> txtContainerNo.Text Then
                    If MsgBox("Container No changed from '" + hfContainerNo.Value + "' to '" + txtContainerNo.Text + "'. Click 'Yes' to accept or 'No' to abort change. **Please Note: If 'Yes', this will change all of the Packaging Layout(s) associated to the previous value.**", MsgBoxStyle.YesNo, "Automated Container No.") = MsgBoxResult.No Then
                        '* return to original state
                        Response.Redirect("Container.aspx?pCNO=" & hfContainerNo.Value, False)
                    Else
                        UpdateValue = True
                    End If 'EOF If MsgBox
                Else
                    UpdateValue = True
                End If 'EOF If hfContainerNo.Value <> txtContainerNo.Text Then

                '**********************
                '* Update Record
                '********************** 
                If UpdateValue = True Then
                    PKGModule.UpdatePkgContainer(txtCID.Text, txtContainerNo.Text, txtDescription.Text, txtType.Text, txtNotes.Text, ddObsolete.SelectedValue, DefaultUser)

                    BindData(ViewState("pCNO"))
                End If

            Else 'EOF  If ViewState("pCNO") <> 0 Then
                '**********************
                '*Build ContainerNo
                '**********************
                Dim ContainerNo As String = ""
                If txtContainerNo.Text = Nothing Then
                    ContainerNo = ddOEM.SelectedValue & ddColor.SelectedValue & "-" & _
                    FormatNumber(txtOutLength.Text, 0) & FormatNumber(txtOutWidth.Text, 0) & FormatNumber(txtOutHeight.Text, 0)
                    If MsgBox("'" + ContainerNo + "' was generated for this entry. Click 'Yes' to Accept or 'No' to correct entries.", MsgBoxStyle.YesNo, "Automated Container No.") = MsgBoxResult.Yes Then
                        '**********************
                        '* Save Record
                        '**********************
                        PKGModule.InsertPkgContainer(ContainerNo, txtDescription.Text, txtType.Text, OEM, OEMMfg, ddColor.SelectedValue, txtInLength.Text, txtInWidth.Text, txtInHeight.Text, txtOutLength.Text, txtOutWidth.Text, txtOutHeight.Text, txtTareWeight.Text, txtNotes.Text, DefaultUser)

                        Response.Redirect("Container.aspx?pCNO=" & ContainerNo, False)
                    End If 'EOF If MsgBox
                End If 'EOF If txtContainerNo.Text = Nothing Then
            End If 'EOF If ViewState("pCNO") <> Nothing Then
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnSave_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        If ViewState("pCNO") <> Nothing Or ViewState("pCNO") <> "" Then
            BindData(ViewState("pCNO"))
        Else
            Response.Redirect("Container.aspx", False)
        End If
    End Sub 'EOF btnReset_Click

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            '***************
            '* Delete Record
            '***************
            If ViewState("pCNO") <> Nothing Then
                PKGModule.DeletePkgContainer(txtCID.Text)

                '***************
                '* Redirect user back to the search page.
                '***************
                Response.Redirect("ContainerList.aspx", False)
            End If

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnDelete_Click

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("Container.aspx", False)
    End Sub 'EOF btnAdd_Click

#End Region 'EOF Detail

#Region "gvCustomer"

    Protected Sub gvCustomer_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustomer.RowCommand

        Try
            Dim Customer As TextBox
            Dim intRowsAffected As Integer = 0

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then

                odsCustomer.InsertParameters("CID").DefaultValue = txtCID.Text

                Customer = CType(gvCustomer.FooterRow.FindControl("txtInsertCustomer"), TextBox)
                odsCustomer.InsertParameters("Customer").DefaultValue = Customer.Text

                odsCustomer.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvCustomer.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvCustomer.ShowFooter = True
                Else
                    gvCustomer.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                Customer = CType(gvCustomer.FooterRow.FindControl("txtInsertCustomer"), TextBox)
                Customer.Text = ""
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF gvCustomer_RowCommand

    Private Property LoadDataEmpty_gvCustomer() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_gvCustomer") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_gvCustomer"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get

        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_gvCustomer") = value
        End Set

    End Property 'EOF LoadDataEmpty_gvCustomer

    Protected Sub odsCustomer_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCustomer.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)


            Dim dt As PKG.PKGContainerCustomerDataTable = CType(e.ReturnValue, PKG.PKGContainerCustomerDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_gvCustomer = True
            Else
                LoadDataEmpty_gvCustomer = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF odsCustomer_Selected

    Protected Sub gvCustomer_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCustomer.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_gvCustomer
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

    End Sub 'EOF gvCustomer_RowCreated

#End Region 'EOF gvCustomer

#Region "gvSupplier"

    Protected Sub gvSupplier_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSupplier.RowCommand

        Try
            Dim Vendor As DropDownList
            Dim intRowsAffected As Integer = 0

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then
                odsSupplier.InsertParameters("CID").DefaultValue = txtCID.Text

                Vendor = CType(gvSupplier.FooterRow.FindControl("ddInsertSupplier"), DropDownList)
                odsSupplier.InsertParameters("VendorNo").DefaultValue = Vendor.SelectedValue

                odsSupplier.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvSupplier.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvSupplier.ShowFooter = True
                Else
                    gvSupplier.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                Vendor = CType(gvSupplier.FooterRow.FindControl("ddInsertSupplier"), DropDownList)
                Vendor.ClearSelection()
                Vendor.Items.Add("")
                Vendor.SelectedValue = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try


    End Sub 'EOF gvSupplier_RowCommand

    Private Property LoadDataEmpty_gvSupplier() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_gvSupplier") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_gvSupplier"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get

        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_gvSupplier") = value
        End Set

    End Property 'EOF LoadDataEmpty_gvSupplier

    Protected Sub odsSupplier_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsSupplier.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)


            Dim dt As PKG.PKGContainerSupplierDataTable = CType(e.ReturnValue, PKG.PKGContainerSupplierDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_gvSupplier = True
            Else
                LoadDataEmpty_gvSupplier = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF odsSupplier

    Protected Sub gvSupplier_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSupplier.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_gvSupplier
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

#End Region 'EOF gvSupplier_RowCreated

End Class
