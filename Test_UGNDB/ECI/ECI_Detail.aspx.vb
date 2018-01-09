' ************************************************************************************************
'
' Name:		ECI_Detail.aspx
' Purpose:	This Code Behind is for the Engineering Change Instruction
'
' Date		Author	    
' 08/19/2008   Roderick Carlson
' 10/26/2009   Roderick Carlson - Modified - cleaned up error trapping on ECI Notification Group and Facility List
' 10/27/2009   Roderick Carlson - Modified - automatically pull DMS info when saving, Prevent External ECI Release WITHOUT a VENDOR SELECTED
' 11/16/2009   Roderick Carlson - Modified - QE-2771 - allow edits after release, notify previous recipients that change occured
' 11/17/2009   Roderick Carlson - Modified - Allow user to pull values from Cost Sheet
' 11/23/2009   Roderick Carlson - Modified - ECI-2776 - Added PPAPLevel
' 11/30/2009   Roderick Carlson - Modified - QE-2779 - Changed isPPAP from checkbox to radio button
' 11/30/2009pm Roderick Carlson - Modified - QE-2780 - Allowed Design Level and Customer Numbers to be shown regardless of the ECI Type
' 12/04/2009   Roderick Carlson - Modified - Readjust how Family-SubFamily Get auto populated on GetNewDrawingInfo, Created BindFamilySubFamily, adjust gvCustomerProgram_SelectedIndexChanged
' 01/07/2010   Roderick Carlson - Modified - ECI-2811 - Added Preview Buttons to bottom of web page
' 07/08/2010   Roderick Carlson - Modified - DMS-2909 - Adjust Release Type based on Dropdown selection of ExistingMaterialAction
' 07/21/2010   Roderick Carlson - Modified - Added Insert ECICAR
' 09/10/2010   Roderick Carlson - Modified - ECI-2975 - When ECI is released, then also Notify Prod Dev team member based on New Drawing No
' 09/10/2010   Roderick Carlson - Modified - ECI-2976 - Allow Released ECIs to be voided
' 09/28/2011   Roderick Carlson - Modified - ECI-3119 - Bryan Hall and Silvia Talavera - do not allow ECI to be released when PPAP is checked and no due date is assigned
' 10/19/2011   Roderick Carlson - Modified - Added Program Make Platform filters
' 12/06/2011   Roderick Carlson - Modified - Allow more types of uploads, readjusted how program dropdowns work
' 02/29/2012   Roderick Carlson - Modified - Do not pull obsolete programs from DMS
' 05/07/2012   Roderick Carlson - Modified - Fix bug when pulling inactive problems also do not import from DMS if year is 0
' 11/05/2012   Roderick Carlson - Modified - Supporting Docs should be updloaded even after release - PPAP documents
' 02/18/2013   Roderick Carlson - Modified - Aded ECI Initiator List
' ************************************************************************************************

Partial Class ECI_Detail
    Inherits System.Web.UI.Page
#Region "Customer/Program Gridview"
    Protected Sub gvCustomerProgram_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCustomerProgram.DataBound

        'hide header columns
        If gvCustomerProgram.Rows.Count > 0 Then
            gvCustomerProgram.HeaderRow.Cells(0).Visible = False
            gvCustomerProgram.HeaderRow.Cells(1).Visible = False
            gvCustomerProgram.HeaderRow.Cells(3).Visible = False
            'gvCustomerProgram.HeaderRow.Cells(8).Visible = False
        End If

    End Sub

    Protected Sub gvCustomerProgram_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCustomerProgram.RowCreated

        'hide columns
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            e.Row.Cells(3).Attributes.CssStyle.Add("display", "none")
            'e.Row.Cells(8).Attributes.CssStyle.Add("display", "none")
        End If

    End Sub
    Protected Sub gvCustomerProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCustomerProgram.SelectedIndexChanged

        Try
            ClearMessages()

            Dim iProgramYear As Integer = 0

            tblMakes.Visible = False

            ViewState("CurrentCustomerProgramRow") = gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(0).Text

            ViewState("CurrentCustomerProgramID") = gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(3).Text

            If Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(4).Text, "&nbsp;", "") <> "" Then
                iProgramYear = CType(Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(5).Text, "&nbsp;", ""), Integer)
                If iProgramYear > 0 Then
                    ddYear.SelectedValue = iProgramYear
                End If
            End If

            txtSOPDate.Text = Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(6).Text.Trim, "&nbsp;", "")
            txtEOPDate.Text = Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(7).Text.Trim, "&nbsp;", "")

            cbCustomerApprovalRequired.Checked = CType(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(8).Controls(0), CheckBox).Checked
            txtCustomerApprovalDate.Text = Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(9).Text.Trim, "&nbsp;", "")
            txtCustomerApprovalNo.Text = Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(10).Text.Trim, "&nbsp;", "")

            btnCancelEditCustomerProgram.Visible = True

            gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageCustomerProgram.Text = lblMessage.Text

    End Sub

#End Region
#Region "UGN Facility Gridview"
    Private Property LoadDataEmpty_FacilityDept() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_FacilityDept") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_FacilityDept"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_FacilityDept") = value
        End Set

    End Property

    Protected Sub odsFacilityDept_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFacilityDept.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As ECI.ECIFacilityDept_MaintDataTable = CType(e.ReturnValue, ECI.ECIFacilityDept_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_FacilityDept = True
            Else
                LoadDataEmpty_FacilityDept = False
            End If
        End If


    End Sub

    Protected Sub gvFacilityDept_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFacilityDept.DataBound

        'hide headers
        If gvFacilityDept.Rows.Count > 0 Then
            gvFacilityDept.HeaderRow.Cells(0).Visible = False
            gvFacilityDept.HeaderRow.Cells(1).Visible = False
        End If


    End Sub

    Protected Sub gvFacilityDept_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFacilityDept.RowCommand

        Try

            ClearMessages()

            Dim ddFacilityTemp As DropDownList
            Dim ddDepartmentTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("ECINo") > 0) Then

                ddFacilityTemp = CType(gvFacilityDept.FooterRow.FindControl("ddInsertFacility"), DropDownList)
                ddDepartmentTemp = CType(gvFacilityDept.FooterRow.FindControl("ddInsertDepartment"), DropDownList)

                If ddFacilityTemp.SelectedIndex > 0 Then
                    odsFacilityDept.InsertParameters("ECINo").DefaultValue = ViewState("ECINo")
                    odsFacilityDept.InsertParameters("UGNFacility").DefaultValue = ddFacilityTemp.SelectedValue
                    odsFacilityDept.InsertParameters("DepartmentID").DefaultValue = ddDepartmentTemp.SelectedValue

                    intRowsAffected = odsFacilityDept.Insert()

                    lblMessage.Text += "Record Saved Successfully.<br>"
                Else
                    lblMessage.Text += "ERROR: the UGN Facility is required.<br>"
                End If


            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddFacilityTemp = CType(gvFacilityDept.FooterRow.FindControl("ddInsertFacility"), DropDownList)
                ddFacilityTemp.SelectedIndex = -1

                ddDepartmentTemp = CType(gvFacilityDept.FooterRow.FindControl("ddInsertDepartment"), DropDownList)
                ddDepartmentTemp.SelectedIndex = -1
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageFacilityDepartment.Text = lblMessage.Text

    End Sub

    Protected Sub gvFacilityDept_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFacilityDept.RowCreated

        Try

            'hide  column
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_FacilityDept
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br>"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
#End Region
#Region "Vendor Gridview"
    Private Property LoadDataEmpty_Vendor() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Vendor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Vendor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Vendor") = value
        End Set

    End Property

    Protected Sub odsVendor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsVendor.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As ECI.ECIVendor_MaintDataTable = CType(e.ReturnValue, ECI.ECIVendor_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Vendor = True
            Else
                LoadDataEmpty_Vendor = False
            End If
        End If

    End Sub

    Protected Sub gvVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvVendor.DataBound

        'hide header of first column
        If gvVendor.Rows.Count > 0 Then
            gvVendor.HeaderRow.Cells(0).Visible = False
        End If

    End Sub

    Protected Sub gvVendor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvVendor.RowCommand

        Try

            ClearMessages()

            Dim ddVendorTemp As DropDownList
            Dim txtPPAPDueDate As TextBox
            Dim txtPPAPCompletionDate As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("ECINo") > 0) Then
                ddVendorTemp = CType(gvVendor.FooterRow.FindControl("ddInsertVendor"), DropDownList)
                txtPPAPDueDate = CType(gvVendor.FooterRow.FindControl("txtInsertPPAPDueDate"), TextBox)
                txtPPAPCompletionDate = CType(gvVendor.FooterRow.FindControl("txtInsertPPAPCompletionDate"), TextBox)

                If rbPPAP.SelectedValue = 1 And txtPPAPDueDate.Text.Trim = "" Then
                    lblMessage.Text = "Error: Vendor PPAP Due Date is required.<br>"
                Else
                    odsVendor.InsertParameters("ECINo").DefaultValue = ViewState("ECINo")
                    odsVendor.InsertParameters("UGNDBVendorID").DefaultValue = ddVendorTemp.SelectedValue
                    odsVendor.InsertParameters("PPAPDueDate").DefaultValue = txtPPAPDueDate.Text
                    odsVendor.InsertParameters("PPAPCompletionDate").DefaultValue = txtPPAPCompletionDate.Text

                    intRowsAffected = odsVendor.Insert()

                    lblMessage.Text = "Record Saved Successfully.<br>"

                    btnRelease.Visible = ViewState("isAdmin")
                End If
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvVendor.ShowFooter = False
            Else
                gvVendor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddVendorTemp = CType(gvVendor.FooterRow.FindControl("ddInsertVendor"), DropDownList)
                ddVendorTemp.SelectedIndex = -1

                txtPPAPDueDate = CType(gvVendor.FooterRow.FindControl("txtInsertPPAPDueDate"), TextBox)
                txtPPAPDueDate.Text = ""

                txtPPAPCompletionDate = CType(gvVendor.FooterRow.FindControl("txtInsertPPAPCompletionDate"), TextBox)
                txtPPAPCompletionDate.Text = ""
            End If

            If e.CommandName = "Delete" Then
                btnRelease.Visible = VendorValidate()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageMaterialVendor.Text = lblMessage.Text

    End Sub

    Protected Sub gvVendor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvVendor.RowCreated

        Try
            'hide column
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Vendor
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br>"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
#End Region

#Region "Kit Gridview"
    Private Property LoadDataEmpty_Kit() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Kit") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Kit"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Kit") = value
        End Set

    End Property

    Protected Sub odsKit_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsKit.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As ECI.ECIKit_MaintDataTable = CType(e.ReturnValue, ECI.ECIKit_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Kit = True
            Else
                LoadDataEmpty_Kit = False
            End If
        End If

    End Sub

    Protected Sub gvKit_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvKit.DataBound

        'hide headers
        If gvKit.Rows.Count > 0 Then
            gvKit.HeaderRow.Cells(0).Visible = False
            gvKit.HeaderRow.Cells(1).Visible = False
        End If

    End Sub

    Protected Sub gvKit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvKit.RowCommand

        Try

            ClearMessages()

            Dim txtKitPartNoTemp As TextBox
            Dim txtKitPartRevisionTemp As TextBox
            Dim txtFinishedGoodPartNoTemp As TextBox
            Dim txtFinishedGoodPartRevisionTemp As TextBox


            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("ECINo") > 0) Then

                txtKitPartNoTemp = CType(gvKit.FooterRow.FindControl("txtInsertKitPartNo"), TextBox)
                txtKitPartRevisionTemp = CType(gvKit.FooterRow.FindControl("txtInsertKitPartRevision"), TextBox)
                txtFinishedGoodPartNoTemp = CType(gvKit.FooterRow.FindControl("txtInsertFinishedGoodPartNo"), TextBox)
                txtFinishedGoodPartRevisionTemp = CType(gvKit.FooterRow.FindControl("txtInsertFinishedGoodPartRevision"), TextBox)

                odsKit.InsertParameters("ECINo").DefaultValue = ViewState("ECINo")
                odsKit.InsertParameters("KitPartNo").DefaultValue = txtKitPartNoTemp.Text
                odsKit.InsertParameters("KitPartRevision").DefaultValue = txtKitPartRevisionTemp.Text
                odsKit.InsertParameters("FinishedGoodPartNo").DefaultValue = txtFinishedGoodPartNoTemp.Text
                odsKit.InsertParameters("FinishedGoodPartRevision").DefaultValue = txtFinishedGoodPartRevisionTemp.Text

                intRowsAffected = odsKit.Insert()

                lblMessage.Text = "Record Saved Successfully.<br>"

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvKit.ShowFooter = False
            Else
                gvKit.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtKitPartNoTemp = CType(gvKit.FooterRow.FindControl("txtInsertKitPartNo"), TextBox)
                txtKitPartNoTemp.Text = ""

                txtKitPartRevisionTemp = CType(gvKit.FooterRow.FindControl("txtInsertKitPartRevision"), TextBox)
                txtKitPartRevisionTemp.Text = ""

                txtFinishedGoodPartNoTemp = CType(gvKit.FooterRow.FindControl("txtInsertFinishedGoodPartNo"), TextBox)
                txtFinishedGoodPartNoTemp.Text = ""

                txtFinishedGoodPartRevisionTemp = CType(gvKit.FooterRow.FindControl("txtInsertFinishedGoodPartRevision"), TextBox)
                txtFinishedGoodPartRevisionTemp.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageKIT.Text = lblMessage.Text

    End Sub

    Protected Sub gvKit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvKit.RowCreated

        Try
            'hide data and footer row columns
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Kit
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br>"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvKit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvKit.RowDataBound

        Try
            ' Build the client script to open a popup window containing
            ' SubDrawings. Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribsKit As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strWindowAttribsFinishedGood As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            If (e.Row.RowType = DataControlRowType.Footer) Then
                Dim ibtnInsertSearchKitPartNo As ImageButton = CType(e.Row.FindControl("ibtnInsertSearchKitPartNo"), ImageButton)
                Dim txtFooterKitPartNo As TextBox = CType(e.Row.FindControl("txtInsertKitPartNo"), TextBox)
                Dim txtFooterKitPartRevision As TextBox = CType(e.Row.FindControl("txtInsertKitPartRevision"), TextBox)

                If ibtnInsertSearchKitPartNo IsNot Nothing Then

                    Dim strPagePath As String = _
                    "../DataMaintenance/PartNoLookUp.aspx?vcPartNo=" & txtFooterKitPartNo.ClientID & "&vcPartRevision=" & txtFooterKitPartRevision.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                        strWindowAttribsKit & "');return false;"
                    ibtnInsertSearchKitPartNo.Attributes.Add("onClick", strClientScript)
                End If

                Dim ibtnInsertSearchFinishedGoodPartNo As ImageButton = CType(e.Row.FindControl("ibtnInsertSearchFinishedGoodPartNo"), ImageButton)
                Dim txtFooterFinishedGoodPartNo As TextBox = CType(e.Row.FindControl("txtInsertFinishedGoodPartNo"), TextBox)
                Dim txtFooterFinishedGoodPartRevision As TextBox = CType(e.Row.FindControl("txtInsertFinishedGoodPartRevision"), TextBox)

                If ibtnInsertSearchFinishedGoodPartNo IsNot Nothing Then

                    Dim strPagePath As String = _
                    "../DataMaintenance/PartNoLookUp.aspx?vcPartNo=" & txtFooterFinishedGoodPartNo.ClientID & "&vcPartRevision=" & txtFooterFinishedGoodPartRevision.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                        strWindowAttribsFinishedGood & "');return false;"
                    ibtnInsertSearchFinishedGoodPartNo.Attributes.Add("onClick", strClientScript)
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
#End Region

#Region "Insert Empty GridView Work-Around"
    Private Property LoadDataEmpty_NotificationGroup() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_NotificationGroup") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_NotificationGroup"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_NotificationGroup") = value
        End Set

    End Property

    Private Property LoadDataEmpty_AssignedTask() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_AssignedTask") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_AssignedTask"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_AssignedTask") = value
        End Set

    End Property

    Protected Sub odsAssignedTask_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsAssignedTask.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As ECI.ECITask_MaintDataTable = CType(e.ReturnValue, ECI.ECITask_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_AssignedTask = True
            Else
                LoadDataEmpty_AssignedTask = False
            End If
        End If

    End Sub

    Protected Sub gvAssignedTask_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAssignedTask.DataBound

        'hide header of first column
        If gvAssignedTask.Rows.Count > 0 Then
            gvAssignedTask.HeaderRow.Cells(0).Visible = False
        End If

    End Sub

    Protected Sub gvAssignedTask_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAssignedTask.RowCommand

        Try

            ClearMessages()

            Dim ddAssignedTaskNameTemp As DropDownList
            Dim ddAssignedTaskTeamMemberTemp As DropDownList
            Dim txtAssignedTaskTargetDateTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("ECINo") > 0) Then

                ddAssignedTaskNameTemp = CType(gvAssignedTask.FooterRow.FindControl("ddInsertAssignedTaskName"), DropDownList)
                ddAssignedTaskTeamMemberTemp = CType(gvAssignedTask.FooterRow.FindControl("ddInsertAssignedTaskTeamMember"), DropDownList)
                txtAssignedTaskTargetDateTemp = CType(gvAssignedTask.FooterRow.FindControl("txtInsertAssignedTaskTargetDate"), TextBox)

                If ddAssignedTaskNameTemp.SelectedIndex > 0 And ddAssignedTaskTeamMemberTemp.SelectedIndex > 0 Then
                    odsAssignedTask.InsertParameters("ECINo").DefaultValue = ViewState("ECINo")
                    odsAssignedTask.InsertParameters("TaskID").DefaultValue = ddAssignedTaskNameTemp.SelectedValue
                    odsAssignedTask.InsertParameters("TaskTeamMemberID").DefaultValue = ddAssignedTaskTeamMemberTemp.SelectedValue
                    odsAssignedTask.InsertParameters("TargetDate").DefaultValue = txtAssignedTaskTargetDateTemp.Text

                    intRowsAffected = odsAssignedTask.Insert()

                    lblMessage.Text += "Record Saved Successfully.<br>"

                Else
                    lblMessage.Text += "ERROR: Both a task and a team member are required.<br>"
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvAssignedTask.ShowFooter = False
            Else
                gvAssignedTask.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddAssignedTaskNameTemp = CType(gvAssignedTask.FooterRow.FindControl("ddInsertAssignedTaskName"), DropDownList)
                ddAssignedTaskNameTemp.SelectedIndex = -1

                ddAssignedTaskTeamMemberTemp = CType(gvAssignedTask.FooterRow.FindControl("ddInsertAssignedTaskTeamMember"), DropDownList)
                ddAssignedTaskTeamMemberTemp.SelectedIndex = -1

                txtAssignedTaskTargetDateTemp = CType(gvAssignedTask.FooterRow.FindControl("txtInsertAssignedTaskTargetDate"), TextBox)
                txtAssignedTaskTargetDateTemp.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageAssignedTask.Text = lblMessage.Text
    End Sub

    Protected Sub gvAssignedTask_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssignedTask.RowCreated

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

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_AssignedTask
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br>"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
#End Region ' Insert Empty GridView Work-Around

    Private Sub UpdateDrawing()

        Try

            Dim ds As DataSet

            Dim iReleaseTypeID As Integer = 0

            Dim iExistingMaterialActionID As Integer = 0
            If ddExistingMaterialAction.SelectedIndex > 0 Then
                iExistingMaterialActionID = ddExistingMaterialAction.SelectedValue
            End If

            Select Case iExistingMaterialActionID
                Case 1 'scrap - > dormant  
                    iReleaseTypeID = 4
                Case 2 'consume until no more -> alternative 
                    iReleaseTypeID = 2
                Case 3 'create holdtag - >alternative
                    iReleaseTypeID = 2
                Case 4 'other- >alternative
                    iReleaseTypeID = 2
            End Select

            'update current drawing
            If txtCurrentDrawingNo.Text.Trim <> "" And iReleaseTypeID > 0 Then
                ds = PEModule.GetDrawing(txtCurrentDrawingNo.Text.Trim)

                If commonFunctions.CheckDataSet(ds) = True Then
                    'update DMS drawing releasetype
                    PEModule.UpdateDrawingReleaseType(txtCurrentDrawingNo.Text.Trim, iReleaseTypeID)
                End If
            End If

            'update new drawing
            If txtNewDrawingNo.Text.Trim <> "" Then
                ds = PEModule.GetDrawing(txtNewDrawingNo.Text.Trim)

                If commonFunctions.CheckDataSet(ds) = True Then
                    'update DMS drawing releasetype
                    PEModule.UpdateDrawingReleaseType(txtNewDrawingNo.Text.Trim, 1) 'production releasetype id

                    'update DMS Drawing with ECINo, if Drawing has an ECI, then lock Release Type field
                    ECIModule.UpdateDrawingECI(txtNewDrawingNo.Text.Trim, ViewState("ECINo"))

                    'update drawing Status if new to issued
                    If ds.Tables(0).Rows(0).Item("approvalstatus").ToString = "N" Then
                        PEModule.UpdateDrawingStatus(txtNewDrawingNo.Text.Trim, "I")
                    End If

                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Function HandleDrawingPopUps(ByVal DrawingControlID As String) As String

        Try
            ' Build the client script to open a popup window 
            ' Pass the ClientID of the TextBox (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
                "../PE/DrawingLookUp.aspx?DrawingControlID=" & DrawingControlID
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','DrawingNoPopupSearch','" & _
                strWindowAttribs & "');return false;"

            HandleDrawingPopUps = strClientScript

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleDrawingPopUps = ""
        End Try

    End Function

    Protected Function HandleBPCSPopUps(ByVal ccPartNo As String, ByVal ccPartRevision As String, ByVal ccPartDescr As String) As String

        Try
            ' Build the client script to open a popup window 
            ' Pass the ClientID of the TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
                "../DataMaintenance/PartNoLookUp.aspx?vcPartNo=" & ccPartNo & "&vcPartRevision=" & ccPartRevision & "&vcPartDescr=" & ccPartDescr
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                strWindowAttribs & "');return false;"

            HandleBPCSPopUps = strClientScript

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleBPCSPopUps = ""
        End Try

    End Function

    Private Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("isAdmin") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                If iTeamMemberID = 530 Then
                    iTeamMemberID = 140 ' Bryan Hall
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 86)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete

                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub GetLinks()

        Try
            Dim ds As DataSet

            hlnkCurrentDrawingNo.Visible = False
            If txtCurrentDrawingNo.Text.Trim <> "" Then
                ds = PEModule.GetDrawing(txtCurrentDrawingNo.Text.Trim)
                If commonFunctions.CheckDataSet(ds) = True Then
                    hlnkCurrentDrawingNo.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & txtCurrentDrawingNo.Text.Trim
                    hlnkCurrentDrawingNo.Visible = True
                End If
            End If

            hlnkNewDrawingNo.Visible = False
            If txtNewDrawingNo.Text.Trim <> "" Then
                ds = PEModule.GetDrawing(txtNewDrawingNo.Text.Trim)
                If commonFunctions.CheckDataSet(ds) = True Then
                    hlnkNewDrawingNo.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & txtNewDrawingNo.Text.Trim
                    hlnkNewDrawingNo.Visible = True
                End If
            End If

            hlnkCurrentCustomerImage.Visible = False
            If txtCurrentDrawingNo.Text.Trim <> "" And ddECIType.SelectedValue = "Internal" Then
                ds = PEModule.GetDrawingCustomerImages(txtCurrentDrawingNo.Text.Trim)
                If commonFunctions.CheckDataSet(ds) = True Then
                    hlnkCurrentCustomerImage.NavigateUrl = "~/PE/DrawingCustomerImageView.aspx?DrawingNo=" & txtCurrentDrawingNo.Text.Trim
                    hlnkCurrentCustomerImage.Visible = True
                    txtCurrentCustomerDrawingNo.Text = ds.Tables(0).Rows(0).Item("CustomerDrawingNo").ToString.Trim
                End If
            End If

            hlnkNewCustomerImage.Visible = False
            If txtNewDrawingNo.Text.Trim <> "" And ddECIType.SelectedValue = "Internal" Then
                ds = PEModule.GetDrawingCustomerImages(txtNewDrawingNo.Text.Trim)
                If commonFunctions.CheckDataSet(ds) = True Then
                    hlnkNewCustomerImage.NavigateUrl = "~/PE/DrawingCustomerImageView.aspx?DrawingNo=" & txtNewDrawingNo.Text.Trim
                    hlnkNewCustomerImage.Visible = True
                    txtNewCustomerDrawingNo.Text = ds.Tables(0).Rows(0).Item("CustomerDrawingNo").ToString.Trim
                End If
            End If

            hlnkCostSheet.Visible = False
            ViewState("isCostSheetDieCut") = False
            If txtCostSheetID.Text.Trim <> "" Then
                ds = CostingModule.GetCostSheet(CType(txtCostSheetID.Text.Trim, Integer))

                If commonFunctions.CheckDataSet(ds) = True Then
                    hlnkCostSheet.NavigateUrl = "~/Costing/Cost_Sheet_Preview.aspx?CostSheetID=" & txtCostSheetID.Text.Trim
                    hlnkCostSheet.Visible = True

                    If ds.Tables(0).Rows(0).Item("isDieCut") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("isDieCut") = True Then
                            ViewState("isCostSheetDieCut") = True
                            hlnkDieLayout.NavigateUrl = "~/Costing/Die_Layout_Preview.aspx?CostSheetID=" & txtCostSheetID.Text.Trim
                            hlnkDieLayout.Visible = True
                        End If
                    End If

                    ' ''iBtnCostSheetCopy.Visible = ViewState("isAdmin")
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Function VendorValidate() As Boolean

        Dim bResult As Boolean = False

        Try
            Dim ds As DataSet
            Dim iRowCounter As Integer = 0

            ds = ECIModule.GetECIVendor(ViewState("ECINo"))
            If commonFunctions.CheckDataSet(ds) = True Then
                bResult = True
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    'check if PPAP is required then Due Date is needed
                    If rbPPAP.SelectedValue = 1 And ds.Tables(0).Rows(iRowCounter).Item("PPAPDueDate").ToString.Trim = "" Then
                        bResult = False
                    End If
                Next
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        VendorValidate = bResult
    End Function

    Private Sub EnableControls()

        Try

            cbCustomerApprovalRequired.Visible = False

            tblMakes.Visible = False
            ddYear.Visible = False

            btnAddToCustomerProgram.Visible = False
            btnCancelEdit.Visible = False
            btnCancelEditCustomerProgram.Visible = False
            btnEdit.Visible = False
            btnRelease.Visible = False
            btnSave.Visible = False
            btnSaveHeader.Visible = False
            btnSaveMaterialVendor.Visible = False
            btnSaveSupplementalPartInformation.Visible = False
            btnUpdate.Visible = False
            btnUpdateFooter.Visible = False

            imgCustomerApprovalDate.Visible = False
            imgEOPDate.Visible = False
            imgImplementationDate.Visible = False
            imgSOPDate.Visible = False

            lblCustomerApprovalDate.Visible = False
            lblCustomerApprovalNo.Visible = False
            lblCustomerApprovalRequired.Visible = False
            lblEOPDate.Visible = False

            lblSOPDate.Visible = False
            lblYear.Visible = False
            lblYearMarker.Visible = False

            txtCustomerApprovalDate.Visible = False
            txtCustomerApprovalNo.Visible = False
            txtEOPDate.Visible = False
            txtSOPDate.Visible = False

            ViewState("isEnabled") = False

            If ViewState("ECIStatusID") = 1 Or ViewState("ECIStatusID") = 2 Or _
            (ViewState("ECIStatusID") = 0 And ViewState("isAdmin") = True) Or _
            (ViewState("isOverride") = True And ViewState("isAdmin") = True) Then

                If ViewState("isOverride") = True And ViewState("isAdmin") = True And ViewState("ECIStatusID") = 3 Then
                    btnUpdate.Visible = True
                    btnUpdateFooter.Visible = True
                    btnCancelEdit.Visible = True
                Else
                    btnSave.Visible = ViewState("isAdmin")
                    btnSaveHeader.Visible = ViewState("isAdmin")
                    btnSaveMaterialVendor.Visible = ViewState("isAdmin")
                    btnSaveSupplementalPartInformation.Visible = ViewState("isAdmin")
                End If

                ViewState("isEnabled") = True
            Else
                If ViewState("ECIStatusID") = 3 Then
                    btnEdit.Visible = ViewState("isAdmin")
                End If
            End If


            cbCustomerIPP.Enabled = ViewState("isEnabled")
            'cbPPAP.Enabled = ViewState("isEnabled")
            rbPPAP.Enabled = ViewState("isEnabled")

            cbUGNIPP.Enabled = ViewState("isEnabled")

            ddAccountManager.Enabled = ViewState("isEnabled")
            ddBusinessProcessType.Enabled = ViewState("isEnabled")
            ddCommodity.Enabled = ViewState("isEnabled")
            ddDesignationType.Enabled = ViewState("isEnabled")
            ddECIType.Enabled = ViewState("isEnabled")
            ddExistingMaterialAction.Enabled = ViewState("isEnabled")
            ddFamily.Enabled = ViewState("isEnabled")
            ddInitiatorTeamMember.Enabled = ViewState("isEnabled")
            ddPPAPLevel.Enabled = ViewState("isEnabled")
            ddPriceCode.Enabled = ViewState("isEnabled")
            ddProductTechnology.Enabled = ViewState("isEnabled")
            ddPurchasedGood.Enabled = ViewState("isEnabled")
            ddQualityEngineer.Enabled = ViewState("isEnabled")
            ddSubFamily.Enabled = ViewState("isEnabled")

            txtCostSheetID.Enabled = ViewState("isEnabled")
            txtCurrentPartNo.Enabled = ViewState("isEnabled")
            txtCurrentCustomerPartNo.Enabled = ViewState("isEnabled")
            txtCurrentDesignLevel.Enabled = ViewState("isEnabled")
            txtCurrentDrawingNo.Enabled = ViewState("isEnabled")
            txtDesignDesc.Enabled = ViewState("isEnabled")
            txtEmailComments.Enabled = ViewState("isEnabled")
            txtImplementationDate.Enabled = ViewState("isEnabled")
            txtInternalRequirement.Enabled = ViewState("isEnabled")
            txtIPPDate.Enabled = ViewState("isEnabled")
            txtIPPDesc.Enabled = ViewState("isEnabled")

            txtNewPartName.Enabled = ViewState("isEnabled")
            txtNewPartNo.Enabled = ViewState("isEnabled")
            txtNewCustomerPartNo.Enabled = ViewState("isEnabled")
            txtNewDesignLevel.Enabled = ViewState("isEnabled")
            txtNewDrawingNo.Enabled = ViewState("isEnabled")
            txtPurchasingComment.Enabled = ViewState("isEnabled")
            txtRFDNo.Enabled = ViewState("isEnabled")
            txtVendorRequirement.Enabled = ViewState("isEnabled")
            txtVoidComment.Enabled = ViewState("isEnabled")

            GetLinks()

            If ddECIType.SelectedValue = "External" Then
                alnkCustomerBreakdown.Visible = False
                alnkQa166.Visible = False

                ddCommodity.Visible = False
                ddProductTechnology.Visible = False

                lblCommodity.Visible = False
                lblCommodityNote.Visible = False
                lblCommodityMarker.Visible = False

                lblProductTechnology.Visible = False

            Else
                alnkCustomerBreakdown.Visible = True
                alnkQa166.Visible = True

                ddCommodity.Visible = True
                ddProductTechnology.Visible = True

                lblCommodity.Visible = True
                lblCommodityNote.Visible = True
                lblCommodityMarker.Visible = True

                lblProductTechnology.Visible = True

            End If

            ' do not show grids until eci has been created
            If ViewState("ECINo") > 0 Then

                If ddECIType.SelectedIndex > 0 Then
                    Dim strPreviewECIClientScript As String = "javascript:void(window.open('ECI_Preview.aspx?ECINo=" & ViewState("ECINo") & "&ECIType=" & ddECIType.SelectedValue & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    btnPreviewECI.Attributes.Add("onclick", strPreviewECIClientScript)
                    btnPreviewECIBottom.Attributes.Add("onclick", strPreviewECIClientScript)
                End If

                Dim strPreviewUgnIPPClientScript As String = "javascript:void(window.open('UGN_IPP_Preview.aspx?ECINo=" & ViewState("ECINo") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                btnPreviewUgnIPP.Attributes.Add("onclick", strPreviewUgnIPPClientScript)
                btnPreviewUgnIPPBottom.Attributes.Add("onclick", strPreviewUgnIPPClientScript)

                btnCopy.Visible = ViewState("isAdmin")

                If ViewState("ECIStatusID") = 4 Then
                    btnPreviewECI.Visible = False
                    btnPreviewECIBottom.Visible = False
                    btnPreviewUgnIPP.Visible = False
                    btnPreviewUgnIPPBottom.Visible = False
                Else
                    btnPreviewECI.Visible = True
                    btnPreviewECIBottom.Visible = True

                    If cbUGNIPP.Checked = True Then
                        btnPreviewUgnIPP.Visible = True
                        btnPreviewUgnIPPBottom.Visible = True
                    End If
                End If

                accAssignedTask.Visible = True
                If ddECIType.SelectedValue = "External" Then
                    accVendor.Visible = True
                    accKit.Visible = False
                    btnSaveSupplementalPartInformation.Visible = False

                    If rbPPAP.SelectedValue = 0 Then
                        ddPPAPLevel.SelectedIndex = 0
                        ddPPAPLevel.Visible = False
                        lblPPAPLevelMarker.Visible = False
                        lblPPAPLevel.Visible = False
                    Else
                        ddPPAPLevel.Visible = True
                        lblPPAPLevelMarker.Visible = True
                        lblPPAPLevel.Visible = True
                        If InStr(txtVendorRequirement.Text, "Please refer to UGN PPAP Checklist for PPAP requirements. Please send the PPAP to the attention of the UGN QE at the Corporate Division.") <= 0 Then
                            txtVendorRequirement.Text = "Please refer to UGN PPAP Checklist for PPAP requirements. Please send the PPAP to the attention of the UGN QE at the Corporate Division. " & txtVendorRequirement.Text
                        End If
                    End If

                Else
                    accVendor.Visible = False
                    accKit.Visible = True
                End If

                If ViewState("ECIStatusID") = 1 Or ViewState("ECIStatusID") = 2 Or _
                    (ViewState("isOverride") = True And ViewState("isAdmin") = True) Then

                    cbCustomerApprovalRequired.Visible = ViewState("isAdmin")

                    btnAddToCustomerProgram.Visible = ViewState("isAdmin")

                    If ddECIType.SelectedValue = "External" Then
                        If VendorValidate() = True Then
                            If ViewState("ECIStatusID") = 1 Or ViewState("ECIStatusID") = 2 Then
                                btnRelease.Visible = ViewState("isAdmin")
                            End If
                        Else
                            btnRelease.Visible = False
                            If InStr(lblMessage.Text.Trim, "A vendor is required before releasing the ECI", CompareMethod.Binary) <= 0 Then
                                lblMessage.Text &= "A vendor is required before releasing the ECI.<br />Please also make sure a PPAP Due Date is assigned if PPAP is required."
                            End If
                        End If
                    Else
                        If ViewState("ECIStatusID") = 1 Or ViewState("ECIStatusID") = 2 Then
                            btnRelease.Visible = ViewState("isAdmin")
                        End If
                    End If

                    If ViewState("ECIStatusID") = 1 Or ViewState("ECIStatusID") = 2 Then
                        btnVoid.Visible = ViewState("isAdmin")
                    End If

                    tblMakes.Visible = ViewState("isAdmin")
                    ddYear.Visible = ViewState("isAdmin")

                    imgCustomerApprovalDate.Visible = ViewState("isAdmin")
                    imgEOPDate.Visible = ViewState("isAdmin")
                    imgImplementationDate.Visible = ViewState("isAdmin")
                    imgIPPDate.Visible = ViewState("isAdmin")
                    imgSOPDate.Visible = ViewState("isAdmin")

                    iBtnPartBOMView.Visible = ViewState("isAdmin")

                    iBtnCurrentPartNoSearch.Visible = ViewState("isAdmin")
                    iBtnCurrentDrawingSearch.Visible = ViewState("isAdmin")

                    iBtnNewPartNoSearch.Visible = ViewState("isAdmin")
                    iBtnNewDrawingSearch.Visible = ViewState("isAdmin")

                    iBtnParentPartCopy.Visible = ViewState("isAdmin")

                    lblCustomerApprovalDate.Visible = ViewState("isAdmin")
                    lblCustomerApprovalNo.Visible = ViewState("isAdmin")
                    lblCustomerApprovalRequired.Visible = ViewState("isAdmin")
                    lblEOPDate.Visible = ViewState("isAdmin")

                    lblSOPDate.Visible = ViewState("isAdmin")
                    lblYear.Visible = ViewState("isAdmin")
                    lblYearMarker.Visible = ViewState("isAdmin")

                    txtCustomerApprovalDate.Visible = ViewState("isAdmin")
                    txtCustomerApprovalNo.Visible = ViewState("isAdmin")
                    txtEOPDate.Visible = ViewState("isAdmin")
                    txtSOPDate.Visible = ViewState("isAdmin")

                    If ViewState("ECINo") > 0 Then 'And ViewState("isAdmin") = True
                        lblMaxNote.Visible = True
                        tblUpload.Visible = Not isSupportingDocCountMaximum()
                    End If

                    gvAssignedTask.Columns(gvAssignedTask.Columns.Count - 1).Visible = ViewState("isAdmin")
                    gvAssignedTask.ShowFooter = ViewState("isAdmin")

                    gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = ViewState("isAdmin")

                    gvFacilityDept.Columns(gvFacilityDept.Columns.Count - 1).Visible = ViewState("isAdmin")
                    gvFacilityDept.ShowFooter = ViewState("isAdmin")

                    gvKit.Columns(gvKit.Columns.Count - 1).Visible = ViewState("isAdmin")
                    gvKit.ShowFooter = ViewState("isAdmin")

                    gvNotificationGroup.Columns(gvNotificationGroup.Columns.Count - 1).Visible = ViewState("isAdmin")
                    gvNotificationGroup.ShowFooter = ViewState("isAdmin")

                    gvSupportingDoc.Columns(gvSupportingDoc.Columns.Count - 1).Visible = ViewState("isAdmin")

                    gvVendor.Columns(gvVendor.Columns.Count - 1).Visible = ViewState("isAdmin")
                    gvVendor.ShowFooter = ViewState("isAdmin")
                Else
                    btnRelease.Visible = False
                    tblUpload.Visible = False

                    If ViewState("ECIStatusID") = 3 Then
                        btnVoid.Visible = ViewState("isAdmin")
                    End If

                    imgImplementationDate.Visible = False
                    imgIPPDate.Visible = False

                    iBtnCostSheetCopy.Visible = False

                    iBtnCurrentPartNoSearch.Visible = False
                    iBtnCurrentDrawingSearch.Visible = False

                    iBtnNewPartNoSearch.Visible = False
                    iBtnNewDrawingSearch.Visible = False

                    gvAssignedTask.Columns(gvAssignedTask.Columns.Count - 1).Visible = False
                    gvAssignedTask.ShowFooter = False

                    gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = False
                    gvCustomerProgram.ShowFooter = False

                    gvFacilityDept.Columns(gvFacilityDept.Columns.Count - 1).Visible = False
                    gvFacilityDept.ShowFooter = False

                    gvKit.Columns(gvKit.Columns.Count - 1).Visible = False
                    gvKit.ShowFooter = False

                    gvNotificationGroup.Columns(gvNotificationGroup.Columns.Count - 1).Visible = False
                    gvNotificationGroup.ShowFooter = False

                    gvSupportingDoc.Columns(gvSupportingDoc.Columns.Count - 1).Visible = False

                    ' if PPAP is due, allow editing after issued - but not void
                    If ViewState("ECIStatusID") = 3 And rbPPAP.SelectedValue = 1 Then
                        gvVendor.Columns(gvVendor.Columns.Count - 1).Visible = True
                        gvVendor.ShowFooter = True
                    Else
                        gvVendor.Columns(gvVendor.Columns.Count - 1).Visible = False
                        gvVendor.ShowFooter = False
                    End If
                End If

                accCustomerProgram.Visible = True
                accNotification.Visible = True
                accFacilityDept.Visible = True
                accSupportingDocs.Visible = True

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ClearMessages()

        Try

            lblMessage.Text = ""
            lblMessageAssignedTask.Text = ""
            lblMessageBPCS.Text = ""
            lblMessagePartBOM.Text = ""
            lblMessageCustomerProgram.Text = ""
            lblMessageECINotification.Text = ""
            lblMessageFacilityDepartment.Text = ""
            lblMessageKIT.Text = ""
            lblMessageMaterialVendor.Text = ""
            lblMessageSupplementalPartInformation.Text = ""
            lblMessageSupportingDocs.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ClearCustomerProgramInputFields()

        Try
            ViewState("CurrentCustomerProgramRow") = 0
            ViewState("CurrentCustomerProgramID") = 0

            gvCustomerProgram.DataBind()
            gvCustomerProgram.SelectedIndex = -1
            gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = True
            tblMakes.Visible = True
            cddMakes.SelectedValue = Nothing
            ddYear.SelectedIndex = -1
            txtSOPDate.Text = ""
            txtEOPDate.Text = ""

            cbCustomerApprovalRequired.Checked = False

            txtCustomerApprovalDate.Text = ""
            txtCustomerApprovalNo.Text = ""

            btnCancelEditCustomerProgram.Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Private Sub BindFamilySubFamily()

        Try
            Dim ds As DataSet

            ds = commonFunctions.GetFamily()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddFamily.DataSource = ds
                ddFamily.DataTextField = ds.Tables(0).Columns("ddFamilyName").ColumnName
                ddFamily.DataValueField = ds.Tables(0).Columns("familyID").ColumnName
                ddFamily.DataBind()
                ddFamily.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetSubFamily(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSubFamily.DataSource = ds
                ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddSubFamily.DataBind()
                ddSubFamily.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''bind existing data to drop downs for selection criteria of search
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ''bind existing data to drop down Year 
            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
            End If

            BindFamilySubFamily()

            ds = commonFunctions.GetDesignationType()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddDesignationType.DataSource = ds
                ddDesignationType.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName.ToString()
                ddDesignationType.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName
                ddDesignationType.DataBind()
                ddDesignationType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetBusinessProcessType(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddBusinessProcessType.DataSource = ds
                ddBusinessProcessType.DataTextField = ds.Tables(0).Columns("ddBusinessProcessTypeName").ColumnName.ToString()
                ddBusinessProcessType.DataValueField = ds.Tables(0).Columns("BusinessProcessTypeID").ColumnName
                ddBusinessProcessType.DataBind()
                ddBusinessProcessType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProductTechnology("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddProductTechnology.DataSource = ds
                ddProductTechnology.DataTextField = ds.Tables(0).Columns("ddProductTechnologyName").ColumnName.ToString()
                ddProductTechnology.DataValueField = ds.Tables(0).Columns("ProductTechnologyID").ColumnName
                ddProductTechnology.DataBind()
                ddProductTechnology.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCommodity.DataSource = ds
                ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
                ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddCommodity.DataBind()
                ddCommodity.Items.Insert(0, "")
                ddCommodity.SelectedIndex = 0
            End If

            ds = commonFunctions.GetPurchasedGood("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddPurchasedGood.DataSource = ds
                ddPurchasedGood.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
                ddPurchasedGood.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddPurchasedGood.DataBind()
                ddPurchasedGood.Items.Insert(0, "")
            End If

            'Iniator (Quality Engineer)
            ds = ECIModule.GetECIInitiator()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddInitiatorTeamMember.DataSource = ds
                ddInitiatorTeamMember.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddInitiatorTeamMember.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddInitiatorTeamMember.DataBind()
                ddInitiatorTeamMember.Items.Insert(0, "")
            End If

            ' Quality Engineer
            ds = ECIModule.GetECIInitiator()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddQualityEngineer.DataSource = ds
                ddQualityEngineer.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddQualityEngineer.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddQualityEngineer.DataBind()
                ddQualityEngineer.Items.Insert(0, "")
            End If

            ' Account Manager
            ds = commonFunctions.GetTeamMemberBySubscription(18)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddAccountManager.DataSource = ds
                ddAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddAccountManager.DataBind()
                ddAccountManager.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetPriceCode("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddPriceCode.DataSource = ds
                ddPriceCode.DataTextField = ds.Tables(0).Columns("ddPriceCodeName").ColumnName
                ddPriceCode.DataValueField = ds.Tables(0).Columns("PriceCode").ColumnName
                ddPriceCode.DataBind()
                ddPriceCode.Items.Insert(0, "")
            End If

            ds = ECIModule.GetECIExistingMaterialAction(0, "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddExistingMaterialAction.DataSource = ds
                ddExistingMaterialAction.DataTextField = ds.Tables(0).Columns("ddActionName").ColumnName.ToString()
                ddExistingMaterialAction.DataValueField = ds.Tables(0).Columns("ActionID").ColumnName
                ddExistingMaterialAction.DataBind()
                ddExistingMaterialAction.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindData()

        Try
            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = ECIModule.GetECI(ViewState("ECINo"))

            If commonFunctions.CheckDataSet(ds) = True Then
                lblECINo.Text = ViewState("ECINo")

                cbCustomerIPP.Checked = ds.Tables(0).Rows(0).Item("isCustomerIPP")

                rbPPAP.SelectedIndex = -1
                If ds.Tables(0).Rows(0).Item("isPPAP") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("isPPAP") = True Then
                        rbPPAP.SelectedValue = 1
                    Else
                        rbPPAP.SelectedValue = 0
                    End If
                End If

                cbUGNIPP.Checked = ds.Tables(0).Rows(0).Item("isUgnIPP")

                If ds.Tables(0).Rows(0).Item("AccountManagerID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("AccountManagerID") > 0 Then
                        ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AccountManagerID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("BusinessProcessTypeID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BusinessProcessTypeID") > 0 Then
                        ddBusinessProcessType.SelectedValue = ds.Tables(0).Rows(0).Item("BusinessProcessTypeID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CommodityID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CommodityID") > 0 Then
                        ddCommodity.SelectedValue = ds.Tables(0).Rows(0).Item("CommodityID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("DesignationType") IsNot System.DBNull.Value Then
                    ddDesignationType.SelectedValue = ds.Tables(0).Rows(0).Item("DesignationType")
                End If

                If ds.Tables(0).Rows(0).Item("ECIType") IsNot System.DBNull.Value Then
                    ddECIType.SelectedValue = ds.Tables(0).Rows(0).Item("ECIType")
                Else
                    ddECIType.SelectedValue = "Internal"
                End If

                If ds.Tables(0).Rows(0).Item("ExistingMaterialActionID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ExistingMaterialActionID") > 0 Then
                        ddExistingMaterialAction.SelectedValue = ds.Tables(0).Rows(0).Item("ExistingMaterialActionID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("InitiatorTeamMemberID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("InitiatorTeamMemberID") > 0 Then
                        ddInitiatorTeamMember.SelectedValue = ds.Tables(0).Rows(0).Item("InitiatorTeamMemberID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("ProductionStatus") IsNot System.DBNull.Value Then
                    ddPriceCode.SelectedValue = ds.Tables(0).Rows(0).Item("ProductionStatus")
                End If

                If ds.Tables(0).Rows(0).Item("PPAPLevel") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PPAPLevel") > 0 Then
                        ddPPAPLevel.SelectedValue = ds.Tables(0).Rows(0).Item("PPAPLevel")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("ProductTechnologyID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ProductTechnologyID") > 0 Then
                        ddProductTechnology.SelectedValue = ds.Tables(0).Rows(0).Item("ProductTechnologyID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("PurchasedGoodID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PurchasedGoodID") > 0 Then
                        ddPurchasedGood.SelectedValue = ds.Tables(0).Rows(0).Item("PurchasedGoodID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("QualityEngineerID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("QualityEngineerID") > 0 Then
                        ddQualityEngineer.SelectedValue = ds.Tables(0).Rows(0).Item("QualityEngineerID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("SubFamilyID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("SubFamilyID") > 0 Then
                        ddSubFamily.SelectedValue = ds.Tables(0).Rows(0).Item("SubFamilyID")

                        'get left 2 digits of subfamily code to know family code
                        Dim strFamilyID As String = Left(CType(ddSubFamily.SelectedValue, String).PadLeft(4, "0"), 2)
                        If strFamilyID <> "" Then
                            ddFamily.SelectedValue = CType(strFamilyID, Integer)
                        End If

                    End If
                End If

                If ds.Tables(0).Rows(0).Item("PreviousECINo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PreviousECINo") > 0 Then
                        hlnkPreviousECINo.Text = ds.Tables(0).Rows(0).Item("PreviousECINo").ToString
                        hlnkPreviousECINo.NavigateUrl = "~/ECI/ECI_Detail.aspx?ECINo=" & ds.Tables(0).Rows(0).Item("PreviousECINo").ToString
                        ViewState("PreviousECINo") = ds.Tables(0).Rows(0).Item("PreviousECINo")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CurrentDrawingNo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CurrentDrawingNo").ToString.Trim <> "" Then
                        txtCurrentDrawingNo.Text = ds.Tables(0).Rows(0).Item("CurrentDrawingNo").ToString.Trim
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("NewDrawingNo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("NewDrawingNo").ToString.Trim <> "" Then
                        txtNewDrawingNo.Text = ds.Tables(0).Rows(0).Item("NewDrawingNo").ToString.Trim
                    End If
                End If

                lblCurrentPartName.Text = ds.Tables(0).Rows(0).Item("CurrentPartName").ToString.Trim

                lblECIStatusValue.Text = ds.Tables(0).Rows(0).Item("StatusName").ToString

                ViewState("ECIStatusID") = 1
                If ds.Tables(0).Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("StatusID") > 0 Then
                        ViewState("ECIStatusID") = ds.Tables(0).Rows(0).Item("StatusID")
                    End If
                End If

                lblIssueDate.Text = ds.Tables(0).Rows(0).Item("IssueDate").ToString

                If ds.Tables(0).Rows(0).Item("CostSheetID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CostSheetID") > 0 Then
                        txtCostSheetID.Text = ds.Tables(0).Rows(0).Item("CostSheetID").ToString
                    End If
                End If

                txtCurrentPartNo.Text = ds.Tables(0).Rows(0).Item("CurrentPartNo").ToString.Trim
                txtCurrentCustomerPartNo.Text = ds.Tables(0).Rows(0).Item("CurrentCustomerPartNo").ToString.Trim
                txtCurrentDesignLevel.Text = ds.Tables(0).Rows(0).Item("CurrentDesignLevel").ToString.Trim
                txtCurrentCustomerDrawingNo.Text = ds.Tables(0).Rows(0).Item("CurrentCustomerDrawingNo").ToString.Trim
                txtDesignDesc.Text = ds.Tables(0).Rows(0).Item("DesignDesc").ToString.Trim
                txtImplementationDate.Text = ds.Tables(0).Rows(0).Item("ImplementationDate").ToString
                txtInternalRequirement.Text = ds.Tables(0).Rows(0).Item("InternalRequirement").ToString
                txtIPPDate.Text = ds.Tables(0).Rows(0).Item("IPPDate").ToString
                txtIPPDesc.Text = ds.Tables(0).Rows(0).Item("IPPDesc").ToString.Trim
                txtNewPartName.Text = ds.Tables(0).Rows(0).Item("NewPartName").ToString.Trim
                txtNewPartNo.Text = ds.Tables(0).Rows(0).Item("NewPartNo").ToString.Trim
                txtNewCustomerPartNo.Text = ds.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString.Trim
                txtNewDesignLevel.Text = ds.Tables(0).Rows(0).Item("NewDesignLevel").ToString.Trim
                txtNewCustomerDrawingNo.Text = ds.Tables(0).Rows(0).Item("NewCustomerDrawingNo").ToString.Trim
                txtPurchasingComment.Text = ds.Tables(0).Rows(0).Item("PurchasingComment").ToString.Trim
                txtVendorRequirement.Text = ds.Tables(0).Rows(0).Item("VendorRequirement").ToString.Trim

                If ds.Tables(0).Rows(0).Item("VoidComment").ToString.Trim <> "" Then
                    txtVoidComment.Text = ds.Tables(0).Rows(0).Item("VoidComment").ToString.Trim
                    txtVoidComment.Visible = True
                    lblVoidComment.Visible = True
                    lblVoidCommentMarker.Visible = True
                Else
                    txtVoidComment.Visible = False
                    lblVoidComment.Visible = False
                    lblVoidCommentMarker.Visible = False
                End If

                If ds.Tables(0).Rows(0).Item("RFDNo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("RFDNo") > 0 Then
                        txtRFDNo.Text = ds.Tables(0).Rows(0).Item("RFDNo").ToString
                    End If
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Function SendEmail(ByVal EmailToAddress As String) As Boolean

        Dim bReturnValue As Boolean = False
        Dim strEmailCCAddress As String = ""
        Dim strEmailFromAddress As String = ""

        Try

            Dim dsCustomerProgram As DataSet
            Dim dsAssignedTask As DataSet
            Dim dsSupportingDocs As DataSet
            Dim dsPartsAffected As DataSet

            Dim iRowCounter As Integer = 0
            Dim strSubject As String = ""
            Dim strBody As String = ""
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim strDetailECIURL As String = strProdOrTestEnvironment & "ECI/ECI_Detail.aspx?ECINo=" & ViewState("ECINo")
            ' ''Dim strBPCSTipsURL As String = strProdOrTestEnvironment & "ECI/BPCS/BPCS_Tips.htm"
            Dim strPreviewECIURL As String = strProdOrTestEnvironment & "ECI/ECI_Preview.aspx?ECINo=" & ViewState("ECINo") & "&ECIType=" & ddECIType.SelectedValue
            Dim strPreviewUgnIppURL As String = strProdOrTestEnvironment & "ECI/Ugn_IPP_Preview.aspx?ECINo=" & ViewState("ECINo")

            Dim strPreviewCostFormURL As String = strProdOrTestEnvironment & "Costing/Cost_Sheet_Preview.aspx?CostSheetID=" & txtCostSheetID.Text.Trim
            Dim strPreviewDieLayoutURL As String = strProdOrTestEnvironment & "Costing/Die_Layout_Preview.aspx?CostSheetID=" & txtCostSheetID.Text.Trim
            Dim strPreviewDrawingURL As String = strProdOrTestEnvironment & "PE/DMSDrawingPreview.aspx?DrawingNo=" & txtNewDrawingNo.Text.Trim
            Dim strPreviewCustomerDrawingURL As String = strProdOrTestEnvironment & "PE/DrawingCustomerImageView.aspx?DrawingNo=" & txtNewDrawingNo.Text.Trim
            Dim strPreviewSupportingDocs As String = strProdOrTestEnvironment & "ECI/ECI_Supporting_Doc_View.aspx?RowID="

            'create the mail message using new System.Net.Mail (not CDonts)
            Dim mail As New MailMessage()

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            strEmailFromAddress = strCurrentUser & "@ugnauto.com"
            strEmailCCAddress = strEmailFromAddress

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br /><br />"
                strBody &= "<h1>ALL OF THE INFORMATION BELOW IS PURELY TEST DATA AND SHOULD NOT BE USED OR UPDATED ANYWHERE.</h1><hr />"
            End If

            If ViewState("isOverride") = True Then
                strSubject &= "UPDATED ECI No: " & ViewState("ECINo") & " - New PartNO: " & txtNewPartNo.Text.Trim & " - Implementation Date: " & txtImplementationDate.Text.Trim
            Else
                strSubject &= "ECI No: " & ViewState("ECINo") & " - New PartNO: " & txtNewPartNo.Text.Trim & " - Implementation Date: " & txtImplementationDate.Text.Trim
            End If

            If ViewState("isOverride") = True Then
                strBody &= "<font size='4' face='Verdana' color='red'>A previously released ECI has been updated.</font> <br />"
            Else
                strBody &= "<font size='3' face='Verdana'>A new ECI is ready for your review.</font> <br />"
            End If

            strBody &= "<p><font size='3' face='Verdana'><b>ECI No: " & ViewState("ECINo") & " - New PartNO: " & txtNewPartNo.Text.Trim & " - Implementation Date: " & txtImplementationDate.Text.Trim & "</b></font></p>"

            If ViewState("isOverride") = True Then
                strBody &= "<p><font size='3' face='Verdana'>Please update the AS400 System or documentation with the appropriate changes.</font> <br />"
            Else
                strBody &= "<p><font size='3' face='Verdana'>The creation of a new part or change of an existing part is now official.</font> <br />"
                strBody &= "<p><font size='3' face='Verdana'>Please begin all assigned tasks and update the AS400 System with the appropriate information.</font> <br />"
            End If

            strBody &= "<p><font size='2' face='Verdana'>Comments:<br /> " & txtEmailComments.Text.Trim & "</font></p>"

            strBody &= "<p><font size='2' face='Verdana'>Description:<br /> " & txtDesignDesc.Text.Trim & "</font></p>"

            dsCustomerProgram = ECIModule.GetECICustomerProgram(ViewState("ECINo"))

            If commonFunctions.CheckDataSet(dsCustomerProgram) Then
                strBody &= "<br />"
                strBody &= "<table width='90%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"

                strBody &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                strBody &= "<td><font size='2' face='Verdana'><strong>Customer</strong></font></td>"
                strBody &= "<td><font size='2' face='Verdana'><strong>Program</strong></font></td>"
                strBody &= "<td><font size='2' face='Verdana'><strong>Program Year</strong></font></td>"
                strBody &= "<td><font size='2' face='Verdana'><strong>SOP Date</strong></font></td>"
                strBody &= "<td><font size='2' face='Verdana'><strong>EOP Date</strong></font></td>"
                strBody &= "</tr>"

                For iRowCounter = 0 To dsCustomerProgram.Tables(0).Rows.Count - 1
                    strBody &= "<tr style='border-color:white'><font size='2' face='Verdana'>"
                    strBody &= "<td>" & dsCustomerProgram.Tables(0).Rows(iRowCounter).Item("ddCustomerDesc") & "</td>"
                    strBody &= "<td>" & dsCustomerProgram.Tables(0).Rows(iRowCounter).Item("ddProgramName") & "</td>"

                    If dsCustomerProgram.Tables(0).Rows(iRowCounter).Item("ProgramYear") IsNot System.DBNull.Value Then
                        If dsCustomerProgram.Tables(0).Rows(iRowCounter).Item("ProgramYear") > 0 Then
                            strBody &= "<td>" & dsCustomerProgram.Tables(0).Rows(iRowCounter).Item("ProgramYear") & "</td>"
                        Else
                            strBody &= "<td>&nbsp;</td>"
                        End If
                    Else
                        strBody &= "<td>&nbsp;</td>"
                    End If

                    strBody &= "<td>" & dsCustomerProgram.Tables(0).Rows(iRowCounter).Item("SOPDate") & "</td>"
                    strBody &= "<td>" & dsCustomerProgram.Tables(0).Rows(iRowCounter).Item("EOPDate") & "</td>"
                    strBody &= "</font></tr>"
                Next

                strBody &= "</table>"
            End If

            If ViewState("BackupTeamMembers") <> "" Then
                strBody &= "<p><font size='1' face='Verdana'>"
                strBody &= ViewState("BackupTeamMembers") & "<br />"
                strBody &= "</font></p>"
            End If

            strBody &= "<br /><br /><b><font size='1' color='red'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the links below.</font><b><br />"

            strBody &= "<br /><table width='90%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"

            strBody &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
            strBody &= "<td><font size='2' face='Verdana'><strong>UGNDB Module</strong></font></td>"
            strBody &= "<td><font size='2' face='Verdana'><strong>Identity</strong></font></td>"
            strBody &= "<td><font size='2' face='Verdana'><strong></strong></font></td>"
            strBody &= "<td><font size='2' face='Verdana'><strong></strong></font></td>"
            strBody &= "<td><font size='2' face='Verdana'><strong></strong></font></td>"
            strBody &= "</tr>"

            strBody &= "<tr style='border-color:white'><font size='2' face='Verdana'>"
            strBody &= "<td>ECI</td>"
            strBody &= "<td>" & ViewState("ECINo") & "</td>"
            strBody &= "<td><a href='" & strPreviewECIURL & "'><u>Preview the ECI </u></a></td>"

            If cbUGNIPP.Checked = True Then
                strBody &= "<td><a href='" & strPreviewUgnIppURL & "'><u>Preview the UGN IPP</u></a></td>"
            End If


            strBody &= "<td>&nbsp;</td>"

            strBody &= "</font></tr>"

            strBody &= "<tr style='border-color:white'><font size='2' face='Verdana'>"
            strBody &= "<td>RFD No.</td>"
            strBody &= "<td>" & txtRFDNo.Text.Trim & "</td>"
            strBody &= "<td>&nbsp;</td>"
            strBody &= "<td>&nbsp;</td>"
            strBody &= "<td>&nbsp;</td>"
            strBody &= "</font></tr>"

            strBody &= "<tr style='border-color:white;'><font size='2' face='Verdana'>"
            strBody &= "<td>Cost Sheet</td>"
            strBody &= "<td>" & txtCostSheetID.Text.Trim & "</td>"
            'uncomment once the new costing is turned over to production
            strBody &= "<td><a href='" & strPreviewCostFormURL & "'><u>Preview the Cost Form</u></a></td>"

            If ViewState("isCostSheetDieCut") = True Then
                strBody &= "<td><a href='" & strPreviewDieLayoutURL & "'><u>Preview the Die Layout</u></a></td>"
            Else
                strBody &= "<td>&nbsp;</td>"
            End If

            strBody &= "<td>&nbsp;</td>"
            strBody &= "</font></tr>"

            strBody &= "<tr style='border-color:white'><font size='2' face='Verdana'>"
            strBody &= "<td>DMS Drawing</td>"
            strBody &= "<td>" & txtNewDrawingNo.Text.Trim & "</td>"
            If txtNewDrawingNo.Text.Trim = "" Then
                strBody &= "<td>&nbsp;</td>"
            Else
                strBody &= "<td><a href='" & strPreviewDrawingURL & "'><u>Preview the DMS Drawing</u></a></td>"
            End If

            If hlnkNewCustomerImage.Visible = True Then
                strBody &= "<td><a href='" & strPreviewCustomerDrawingURL & "'><u>Preview the CAD Drawing " & txtNewCustomerDrawingNo.Text.Trim & "</u></a></td>"
            Else
                strBody &= "<td>&nbsp;</td>"
            End If

            strBody &= "<td>&nbsp;</td>"

            strBody &= "</font></tr>"

            strBody &= "</table>"

            strBody &= "<br /><font size='2' face='Verdana'><a href='" & strDetailECIURL & "'><u>View all ECI details </u></a></font><br /><br />"

            dsPartsAffected = ECIModule.GetECIBPCSParentPartsAffected(ViewState("ECINo"))
            If commonFunctions.CheckDataSet(dsPartsAffected) = True Then
                strBody &= "<br />"
                strBody &= "<font size='3' face='Verdana'>Parts Affected</font>"
                strBody &= "<br /><table width='90%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"
                strBody &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                strBody &= "<td><font size='2' face='Verdana'><strong>Current Parent PartNo</strong></font></td>"
                strBody &= "<td><font size='2' face='Verdana'><strong>Current Child PartNo</strong></font></td>"
                strBody &= "</tr>"

                For iRowCounter = 0 To dsPartsAffected.Tables(0).Rows.Count - 1
                    strBody &= "<tr style='border-color:white'><font size='2' face='Verdana'>"

                    strBody &= "<td>" & dsPartsAffected.Tables(0).Rows(iRowCounter).Item("ParentPartNo") & "</td>"
                    strBody &= "<td>" & dsPartsAffected.Tables(0).Rows(iRowCounter).Item("ChildPartNo") & "</td>"

                    strBody &= "</font></tr>"
                Next

                strBody &= "</table><br />"
            End If

            dsSupportingDocs = ECIModule.GetECISupportingDocList(ViewState("ECINo"))
            If commonFunctions.CheckDataSet(dsSupportingDocs) = True Then
                strBody &= "<br /><table width='90%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"
                strBody &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                strBody &= "<td><font size='2' face='Verdana'><strong>Other ECI Supporting Documents</strong></font></td>"
                strBody &= "</tr>"

                For iRowCounter = 0 To dsSupportingDocs.Tables(0).Rows.Count - 1
                    strBody &= "<tr style='border-color:white'><font size='2' face='Verdana'>"
                    strBody &= "<td><a href='" & strPreviewSupportingDocs & dsSupportingDocs.Tables(0).Rows(iRowCounter).Item("RowID") & "'><u>" & dsSupportingDocs.Tables(0).Rows(iRowCounter).Item("SupportingDocName") & "</u></a></td>"
                    strBody &= "</font></tr>"
                Next

                strBody &= "</table><br />"
            End If

            dsAssignedTask = ECIModule.GetECITask(ViewState("ECINo"))

            If commonFunctions.CheckDataSet(dsAssignedTask) = True Then
                strBody &= "<br />"
                strBody &= "<font size='3' face='Verdana'>Team Member Assigned Tasks</font>"
                strBody &= "<table width='90%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"

                strBody &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                strBody &= "<td><font size='2' face='Verdana'><strong>Task</strong></font></td>"
                strBody &= "<td><font size='2' face='Verdana'><strong>Team Member</strong></font></td>"
                strBody &= "<td><font size='2' face='Verdana'><strong>Target Date</strong></font></td>"
                strBody &= "</tr>"

                For iRowCounter = 0 To dsAssignedTask.Tables(0).Rows.Count - 1
                    strBody &= "<tr style='border-color:white'><font size='2' face='Verdana'>"
                    strBody &= "<td>" & dsAssignedTask.Tables(0).Rows(iRowCounter).Item("TaskName") & "</td>"
                    strBody &= "<td>" & dsAssignedTask.Tables(0).Rows(iRowCounter).Item("ddTeamMemberName") & "</td>"
                    strBody &= "<td>" & dsAssignedTask.Tables(0).Rows(iRowCounter).Item("TargetDate") & "</td>"
                    strBody &= "</font></tr>"
                Next

                strBody &= "</table>"
            End If

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody &= "<hr /><br />Email To Address List: " & EmailToAddress & "<br />"
                strBody &= "<br />Email CC Address List: " & strEmailCCAddress & "<br />"
                'for QA Process, the real people will receive the emails
                EmailToAddress = "Roderick.Carlson@ugnauto.com"
                strEmailCCAddress = ""
            End If

            'set the content
            mail.Subject = strSubject
            mail.Body = strBody

            'set the addresses
            mail.From = New MailAddress(strEmailFromAddress)
            Dim i As Integer

            'build email To list
            Dim emailList As String() = EmailToAddress.Split(";")

            For i = 0 To UBound(emailList)
                If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                    mail.To.Add(emailList(i))
                End If
            Next i

            'build email CC List
            If strEmailCCAddress IsNot Nothing Then
                emailList = strEmailCCAddress.Split(";")

                For i = 0 To UBound(emailList)
                    If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                        mail.CC.Add(emailList(i))
                    End If
                Next i
            End If

            mail.IsBodyHtml = True

            'send the message 
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(mail)
                lblMessage.Text &= "Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("ECI Release", strEmailFromAddress, EmailToAddress, strEmailCCAddress, strSubject, strBody, "")
            End Try

            bReturnValue = True
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />EmailToAddress: " & EmailToAddress & "<br />EmailCCAddress:" & strEmailCCAddress & "<br />EmailFromAddress:" & strEmailFromAddress & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        SendEmail = bReturnValue

    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            'clear crystal reports
            ECIModule.CleanECICrystalReports()

            If Not Page.IsPostBack Then
                InitializeViewState()

                CheckRights()

                BindCriteria()

                If HttpContext.Current.Request.QueryString("ECINo") <> "" Then
                    ViewState("ECINo") = HttpContext.Current.Request.QueryString("ECINo")
                    If ViewState("ECINo") >= 200000 Then
                        BindData()
                    Else
                        'if an archived ECI is attempting to be loaded, redirect back to search page
                        Response.Redirect("ECI_List.aspx", False)
                    End If
                End If

                'search current PartNo
                Dim strCurrentPartNoClientScript As String = HandleBPCSPopUps(txtCurrentPartNo.ClientID, "", "")
                iBtnCurrentPartNoSearch.Attributes.Add("onClick", strCurrentPartNoClientScript)

                'search new PartNo
                Dim strNewPartNoClientScript As String = HandleBPCSPopUps(txtNewPartNo.ClientID, "", "")
                iBtnNewPartNoSearch.Attributes.Add("onClick", strNewPartNoClientScript)


                'search current drawingno popup
                Dim strCurrentDrawingNoClientScript As String = HandleDrawingPopUps(txtCurrentDrawingNo.ClientID)
                iBtnCurrentDrawingSearch.Attributes.Add("onClick", strCurrentDrawingNoClientScript)

                'search new drawingno popup
                Dim strNewDrawingNoClientScript As String = HandleDrawingPopUps(txtNewDrawingNo.ClientID)
                iBtnNewDrawingSearch.Attributes.Add("onClick", strNewDrawingNoClientScript)

                txtDesignDesc.Attributes.Add("onkeypress", "return tbLimit();")
                txtDesignDesc.Attributes.Add("onkeyup", "return tbCount(" + lblDesignDescCharCount.ClientID + ");")
                txtDesignDesc.Attributes.Add("maxLength", "400")

                txtInternalRequirement.Attributes.Add("onkeypress", "return tbLimit();")
                txtInternalRequirement.Attributes.Add("onkeyup", "return tbCount(" + lblInternalRequirementCharCount.ClientID + ");")
                txtInternalRequirement.Attributes.Add("maxLength", "400")

                txtIPPDesc.Attributes.Add("onkeypress", "return tbLimit();")
                txtIPPDesc.Attributes.Add("onkeyup", "return tbCount(" + lblIPPDescCharCount.ClientID + ");")
                txtIPPDesc.Attributes.Add("maxLength", "400")

                txtPurchasingComment.Attributes.Add("onkeypress", "return tbLimit();")
                txtPurchasingComment.Attributes.Add("onkeyup", "return tbCount(" + lblPurchasingCharCount.ClientID + ");")
                txtPurchasingComment.Attributes.Add("maxLength", "400")

                txtVendorRequirement.Attributes.Add("onkeypress", "return tbLimit();")
                txtVendorRequirement.Attributes.Add("onkeyup", "return tbCount(" + lblVendorRequirementCharCount.ClientID + ");")
                txtVendorRequirement.Attributes.Add("maxLength", "400")

                btnVoid.Attributes.Add("onclick", "if(confirm('Are you sure that you want to void this ECI? If so, click ok to see and update the void comment field. THEN CLICK VOID AGAIN. ')){}else{return false}")

                txtVoidComment.Attributes.Add("onkeypress", "return tbLimit();")
                txtVoidComment.Attributes.Add("onkeyup", "return tbCount(" + lblVoidCommentCharCount.ClientID + ");")
                txtVoidComment.Attributes.Add("maxLength", "150")

            End If

            If HttpContext.Current.Session("CopyECI") IsNot Nothing Then
                If HttpContext.Current.Session("CopyECI") <> "" Then
                    lblMessage.Text &= "The ECI was successfully copied and saved."
                    HttpContext.Current.Session("CopyECI") = Nothing
                End If
            End If

            EnableControls()

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Engineering Change Instruction Details"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Quality</b> > <a href='ECI_List.aspx'><b> ECI Search </b></a> > ECI Details "

                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("ECIExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

            ViewState("isOverride") = False
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub InitializeViewState()

        Try

            ViewState("ECIStatusID") = 0
            ViewState("ECINo") = 0
            ViewState("PreviousECINo") = 0

            ViewState("isAdmin") = False

            ViewState("CurrentCustomerProgramRow") = 0
            ViewState("CurrentCustomerProgramID") = 0

            ViewState("isOverride") = False

            ViewState("BackupTeamMembers") = ""
            ViewState("isCostSheetDieCut") = False
            ViewState("isEnabled") = False

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ddFamily_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFamily.SelectedIndexChanged

        Try
            ClearMessages()

            Dim ds As DataSet

            Dim iFamilyID As Integer = 0

            If ddFamily.SelectedIndex > 0 Then
                iFamilyID = ddFamily.SelectedValue
            End If

            ds = commonFunctions.GetSubFamily(iFamilyID)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSubFamily.DataSource = ds
                ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName
                ddSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddSubFamily.DataBind()
                ddSubFamily.Items.Insert(0, "")
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub CheckPartNo()

        Try
            Dim ds As DataSet

            If txtCurrentPartNo.Text.Trim <> "" Then
                ds = commonFunctions.GetBPCSPartNo(txtCurrentPartNo.Text.Trim, "")
                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text &= "<br />WARNING: The Current Internal Part number is not in the Oracle System. Please contact Product Engineering."
                End If
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub CheckCostSheet()

        Try
            Dim ds As DataSet

            'warn if cost sheet is not completely approved yet or is missing
            If txtCostSheetID.Text.Trim <> "" Then
                ds = CostingModule.GetCostSheet(CType(txtCostSheetID.Text.Trim, Integer))

                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("ApprovedDate").ToString = "" Then
                        lblMessage.Text &= "WARNING: The cost sheet has not been approved yet.<br />"
                    End If
                Else
                    lblMessage.Text &= "WARNING: The cost sheet referenced does not exist.<br />"
                End If
            Else
                lblMessage.Text &= "WARNING: No cost sheet has been assigned to the ECI.<br />"
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveSupplementalPartInformation.Click, btnSaveMaterialVendor.Click, btnSaveHeader.Click

        Try

            ClearMessages()

            Dim ds As DataSet

            'do not update completed or voided ECIs, unless being overrided
            If (ViewState("ECIStatusID") = 3 Or ViewState("ECIStatusID") = 4) And ViewState("isOverride") = False Then
                lblMessage.Text = "Error: An ECI that is voided or completed cannot be updated."
            Else

                ParentPartCopy()

                CheckPartNo()

                '(LREY) 01/08/2014
                ' ''CheckCustomerPartNo()

                CheckRFD()

                CheckCostSheet()

                CheckDrawing()

                Dim iRFDNo As Integer = 0
                If txtRFDNo.Text.Trim <> "" Then
                    iRFDNo = CType(txtRFDNo.Text, Integer)
                End If

                Dim iCostSheetID As Integer = 0
                If txtCostSheetID.Text.Trim <> "" Then
                    iCostSheetID = CType(txtCostSheetID.Text, Integer)
                End If

                Dim iAccountManagerID As Integer = 0
                If ddAccountManager.SelectedIndex > 0 Then
                    iAccountManagerID = ddAccountManager.SelectedValue
                End If

                Dim iBusinessProcessTypeID As Integer = 0
                If ddBusinessProcessType.SelectedIndex > 0 Then
                    iBusinessProcessTypeID = ddBusinessProcessType.SelectedValue
                End If

                Dim iCommodityID As Integer = 0
                If ddCommodity.SelectedIndex > 0 Then
                    iCommodityID = ddCommodity.SelectedValue
                End If

                Dim iInitiatorTeamMemberID As Integer = 0
                If ddInitiatorTeamMember.SelectedIndex > 0 Then
                    iInitiatorTeamMemberID = ddInitiatorTeamMember.SelectedValue
                End If

                Dim iProductTechnologyID As Integer = 0
                If ddProductTechnology.SelectedIndex > 0 Then
                    iProductTechnologyID = ddProductTechnology.SelectedValue
                End If

                Dim iPurchasedGoodID As Integer = 0
                If ddPurchasedGood.SelectedIndex > 0 Then
                    iPurchasedGoodID = ddPurchasedGood.SelectedValue
                End If

                Dim iQualityEngineerID As Integer = 0
                If ddQualityEngineer.SelectedIndex > 0 Then
                    iQualityEngineerID = ddQualityEngineer.SelectedValue
                End If

                If iQualityEngineerID = 0 Then
                    iQualityEngineerID = iInitiatorTeamMemberID
                End If

                Dim iSubFamilyID As Integer = 0
                If ddSubFamily.SelectedIndex > 0 Then
                    iSubFamilyID = ddSubFamily.SelectedValue
                End If

                Dim iPPAPLevel As Integer = 0
                If ddPPAPLevel.SelectedIndex > 0 Then
                    iPPAPLevel = ddPPAPLevel.SelectedValue
                End If

                Dim iExistingMaterialActionID As Integer = 0
                If ddExistingMaterialAction.SelectedIndex > 0 Then
                    iExistingMaterialActionID = ddExistingMaterialAction.SelectedValue
                End If

                Dim isPPAP As Integer = 0
                If rbPPAP.SelectedIndex > -1 Then
                    If rbPPAP.SelectedValue = 1 Then
                        isPPAP = 1
                    End If
                End If

                'if ECI Exists then update, else insert
                If ViewState("ECINo") > 0 Then
                    ECIModule.UpdateECI(ViewState("ECINo"), ddECIType.SelectedValue, "", txtImplementationDate.Text.Trim, _
                    iRFDNo, iCostSheetID, iInitiatorTeamMemberID, txtCurrentDrawingNo.Text.Trim, txtNewDrawingNo.Text.Trim, _
                    txtCurrentPartNo.Text.Trim, txtNewPartNo.Text.Trim, "", _
                   "", txtNewPartName.Text.Trim, txtCurrentCustomerPartNo.Text.Trim, _
                    txtNewCustomerPartNo.Text.Trim, txtCurrentDesignLevel.Text.Trim, txtNewDesignLevel.Text.Trim, _
                    txtCurrentCustomerDrawingNo.Text.Trim, txtNewCustomerDrawingNo.Text.Trim, _
                    ddDesignationType.SelectedValue, iBusinessProcessTypeID, iCommodityID, iPurchasedGoodID, _
                    iProductTechnologyID, iSubFamilyID, iAccountManagerID, iQualityEngineerID, isPPAP, iPPAPLevel, ddPriceCode.SelectedValue, cbUGNIPP.Checked, cbCustomerIPP.Checked, _
                    txtIPPDesc.Text.Trim, txtIPPDate.Text.Trim, txtDesignDesc.Text.Trim, txtInternalRequirement.Text.Trim, _
                    txtPurchasingComment.Text.Trim, txtVendorRequirement.Text.Trim, iExistingMaterialActionID, 0)
                Else
                    ds = ECIModule.InsertECI(ViewState("PreviousECINo"), ddECIType.SelectedValue, 1, "", txtImplementationDate.Text.Trim, _
                    iRFDNo, iCostSheetID, iInitiatorTeamMemberID, txtCurrentDrawingNo.Text.Trim, txtNewDrawingNo.Text.Trim, _
                    txtCurrentPartNo.Text.Trim, txtNewPartNo.Text.Trim, "", _
                   "", txtNewPartName.Text.Trim, _
                    txtCurrentCustomerPartNo.Text.Trim, txtNewCustomerPartNo.Text.Trim, _
                    txtCurrentDesignLevel.Text.Trim, txtNewDesignLevel.Text.Trim, _
                    txtCurrentCustomerDrawingNo.Text.Trim, txtNewCustomerDrawingNo.Text.Trim, _
                    ddDesignationType.SelectedValue, iBusinessProcessTypeID, iCommodityID, iPurchasedGoodID, _
                    iProductTechnologyID, iSubFamilyID, iAccountManagerID, iQualityEngineerID, isPPAP, iPPAPLevel, _
                    ddPriceCode.SelectedValue, cbUGNIPP.Checked, cbCustomerIPP.Checked, _
                    txtIPPDesc.Text.Trim, txtIPPDate.Text.Trim, txtDesignDesc.Text.Trim, txtInternalRequirement.Text.Trim, _
                    txtPurchasingComment.Text.Trim, txtVendorRequirement.Text.Trim, iExistingMaterialActionID, 0)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        If ds.Tables(0).Rows(0).Item("NewECINo") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("NewECINo") > 0 Then
                                ViewState("ECINo") = ds.Tables(0).Rows(0).Item("NewECINo")
                                lblECINo.Text = ViewState("ECINo")
                                lblECIStatusValue.Text = ds.Tables(0).Rows(0).Item("NewStatusName")
                                ViewState("ECIStatusID") = 1
                            End If
                        End If

                    End If
                End If

                GetNewDrawingInfo()

                If HttpContext.Current.Session("BLLerror") Is Nothing Then
                    lblMessage.Text &= "Record Saved Successfully.<br />"
                Else
                    lblMessage.Text &= HttpContext.Current.Session("BLLerror") & "<br />"
                End If

                BindData()

                EnableControls()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBPCS.Text = lblMessage.Text
        lblMessageSupplementalPartInformation.Text = lblMessage.Text
        lblMessageAssignedTask.Text = lblMessage.Text
        lblMessageECINotification.Text = lblMessage.Text
        lblMessageMaterialVendor.Text = lblMessage.Text

    End Sub

    Protected Sub BuildECICurrentPartBOMTree(ByVal ParentPartNo As String, ByVal n As TreeNode)

        Try

            Dim iRecursionCounter As Integer = Session("sessionECICurrentPartBOMRecursionCounter")
            Dim iCurrentRecursionLevel As Integer = Session("sessionECICurrentPartBOMRecursionLevel")

            If Session("sessionECICurrentPartBOMRecursionCounter") = Nothing Then
                iRecursionCounter = 0
            End If

            Dim dsBOM As DataSet
            Dim iCounter As Integer = 0
            Dim strChildPartNo As String = ""
            Dim dblBuildRequired As Double = 0

            'preventing an infinite loop
            Session("sessionECICurrentPartBOMRecursionCounter") = iRecursionCounter + 1

            If iRecursionCounter <= 99999 Then
                dsBOM = commonFunctions.GetBillOfMaterials(ParentPartNo, "")

                'if SubComponents Exist.
                If dsBOM.Tables(0).Rows.Count > 0 Then
                    Dim root As New TreeNode(txtCurrentPartNo.Text.Trim)

                    'start by creating a ROOT node
                    If iRecursionCounter = 0 Then
                        tvCurrentPartBOM.Nodes.Add(root)
                    End If

                    For iCounter = 0 To dsBOM.Tables(0).Rows.Count - 1

                        iRecursionCounter += 1
                        Session("sessionECICurrentPartBOMRecursionCounter") = iRecursionCounter + 1

                        strChildPartNo = dsBOM.Tables(0).Rows(iCounter).Item("SubPartNo")
                        dblBuildRequired = dsBOM.Tables(0).Rows(iCounter).Item("Quantity")

                        If strChildPartNo.Trim.Length > 0 Then

                            Dim node As New TreeNode(strChildPartNo & "  :: " & " Build Required " & dblBuildRequired)
                            If n Is Nothing Then
                                'root.Checked = True
                                root.SelectAction = TreeNodeSelectAction.None
                                root.ChildNodes.Add(node)
                            Else
                                'n.Checked = True
                                n.SelectAction = TreeNodeSelectAction.None
                                n.ChildNodes.Add(node)
                            End If

                            'node.Checked = True
                            node.SelectAction = TreeNodeSelectAction.None

                            Session("sessionECICurrentPartBOMRecursionLevel") = iCurrentRecursionLevel + 1
                            BuildECICurrentPartBOMTree(strChildPartNo, node)
                            Session("sessionECICurrentPartBOMRecursionLevel") = iCurrentRecursionLevel - 1

                        End If 'end SubComonent
                    Next 'end iCounter Loop
                Else
                    If iRecursionCounter = 0 Then
                        lblMessage.Text = "There are no children currently defined for this part.<br />"
                    End If
                End If 'end iSize                
            End If 'end check recursion counter
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub iBtnPartBOMView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnPartBOMView.Click

        Try
            ClearMessages()

            PartBOMView()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub PartBOMView()

        Try
            If txtCurrentPartNo.Text.Trim <> "" Then

                'need code to clear tree
                tvCurrentPartBOM.Nodes.Clear()

                'clear session variable
                Session("sessionECICurrentPartBOMRecursionCounter") = 0
                Session("sessionECICurrentPartBOMRecursionLevel") = 1

                BuildECICurrentPartBOMTree(txtCurrentPartNo.Text.Trim, Nothing)

                Session("sessionECICurrentPartBOMRecursionCounter") = Nothing
                Session("sessionECICurrentPartBOMRecursionLevel") = Nothing

                'Expand the Whole Tree
                tvCurrentPartBOM.ExpandAll()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ParentPartCopy()

        Try
            If lblMessage.Text = "" And ViewState("isAdmin") = True Then
                ECIModule.InsertECIBPCSPartsAffected(ViewState("ECINo"), txtCurrentPartNo.Text.Trim)

                gvParentPart.DataBind()

                'refresh BOM if Parent Part List is refreshed
                'Call iBtnPartBOMView_Click(sender, E)
                PartBOMView()

            Else
                tvCurrentPartBOM.Nodes.Clear()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub iBtnParentPartCopy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnParentPartCopy.Click

        Try
            ClearMessages()

            CheckPartNo()

            ParentPartCopy()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Function isSupportingDocCountMaximum() As Boolean

        Dim ds As DataSet

        Dim bMax As Boolean = False

        Try

            'check number of supporing docs
            ds = ECIModule.GetECISupportingDocList(ViewState("ECINo"))
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows.Count >= 3 Then
                    bMax = True
                End If
            End If


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            bMax = True
        End Try

        isSupportingDocCountMaximum = bMax

    End Function

    Protected Sub btnSaveUploadSupportingDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveUploadSupportingDocument.Click

        Try
            ClearMessages()

            If ViewState("ECINo") > 0 Then
                If fileUploadSupportingDoc.HasFile Then
                    If fileUploadSupportingDoc.PostedFile.ContentLength <= 3500000 Then
                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(fileUploadSupportingDoc.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(fileUploadSupportingDoc.PostedFile.FileName)

                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(fileUploadSupportingDoc.PostedFile.InputStream.Length)
                        Dim SupportingDocEncodeType As String = fileUploadSupportingDoc.PostedFile.ContentType
                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        fileUploadSupportingDoc.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".xlsx") Or (FileExt = ".doc") Or (FileExt = ".docx") Or (FileExt = ".jpeg") Or (FileExt = ".jpg") Or (FileExt = ".tif") Or (FileExt = ".msg") Or (FileExt = ".ppt") Then

                            ''***************
                            '' Insert Record
                            ''***************
                            ECIModule.InsertECISupportingDoc(ViewState("ECINo"), fileUploadSupportingDoc.FileName, txtSupportingDocDesc.Text.Trim, SupportingDocBinaryFile, SupportingDocFileSize, SupportingDocEncodeType)

                            tblUpload.Visible = Not isSupportingDocCountMaximum()

                            revUploadFile.Enabled = False

                            lblMessage.Text &= "<br />File Uploaded Successfully<br />"

                            gvSupportingDoc.DataBind()
                            gvSupportingDoc.Visible = True

                            revUploadFile.Enabled = True

                            txtSupportingDocDesc.Text = ""
                        End If
                    Else
                        lblMessage.Text = "File exceeds size limit.  Please select a file less than 3MB (3000KB)."
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageSupportingDocs.Text = lblMessage.Text

    End Sub

    Protected Sub gvSupportingDoc_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSupportingDoc.DataBound

        'hide header of first column
        If gvSupportingDoc.Rows.Count > 0 Then
            gvSupportingDoc.HeaderRow.Cells(0).Visible = False
        End If

        If (ViewState("ECIStatusID") = 1 Or ViewState("ECIStatusID") = 2) And (ViewState("isOverride") = True And ViewState("isAdmin") = True) Then
            tblUpload.Visible = Not isSupportingDocCountMaximum()
        End If

    End Sub

    Protected Sub gvSupportingDoc_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSupportingDoc.RowCreated

        'hide first column
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
        End If

    End Sub

    Protected Sub iBtnCostSheetCopy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnCostSheetCopy.Click

        Try
            ClearMessages()

            Dim ds As DataSet
            ' ''Dim iRowCounter As Integer
            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0

            'the new ECI number needs to be identified in order to update grids
            btnSave_Click(sender, e)

            'existing fields will NOT be overwritten
            If txtCostSheetID.Text.Trim <> "" Then

                ds = CostingModule.GetCostSheet(CType(txtCostSheetID.Text.Trim, Integer))

                If commonFunctions.CheckDataSet(ds) = True Then
                    If txtRFDNo.Text.Trim = "" Then
                        txtRFDNo.Text = ds.Tables(0).Rows(0).Item("RFDNo").ToString
                    End If

                    If txtNewCustomerPartNo.Text.Trim = "" Then
                        txtNewCustomerPartNo.Text = ds.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString
                    End If

                    If txtNewDesignLevel.Text.Trim = "" Then
                        txtNewDesignLevel.Text = ds.Tables(0).Rows(0).Item("NewDesignLevel").ToString
                    End If

                    If txtNewDrawingNo.Text.Trim = "" Then
                        txtNewDrawingNo.Text = ds.Tables(0).Rows(0).Item("NewDrawingNo").ToString
                    End If

                    If txtNewPartName.Text.Trim = "" Then
                        txtNewPartName.Text = ds.Tables(0).Rows(0).Item("NewPartName").ToString
                    End If

                    If txtNewPartNo.Text.Trim = "" Then
                        txtNewPartNo.Text = ds.Tables(0).Rows(0).Item("NewPartNo").ToString
                    End If

                    If ddDesignationType.SelectedIndex <= 0 Then
                        If ds.Tables(0).Rows(0).Item("DesignationType") IsNot System.DBNull.Value Then
                            ddDesignationType.SelectedValue = ds.Tables(0).Rows(0).Item("DesignationType").ToString
                        End If
                    End If

                    If ddCommodity.SelectedIndex <= 0 Then
                        If ds.Tables(0).Rows(0).Item("CommodityID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("CommodityID") > 0 Then
                                ddCommodity.SelectedValue = ds.Tables(0).Rows(0).Item("CommodityID")
                            End If
                        End If
                    End If

                    If ddPurchasedGood.SelectedIndex <= 0 Then
                        If ds.Tables(0).Rows(0).Item("PurchasedGoodID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("PurchasedGoodID") > 0 Then
                                ddPurchasedGood.SelectedValue = ds.Tables(0).Rows(0).Item("PurchasedGoodID")
                            End If
                        End If
                    End If

                    If ddAccountManager.SelectedIndex <= 0 Then
                        If ds.Tables(0).Rows(0).Item("AccountManagerID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("AccountManagerID") > 0 Then
                                ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AccountManagerID")
                            End If
                        End If
                    End If

                    '***********************************
                    'update child tables 
                    '***********************************

                    ' ''Dim strFacility As String = ds.Tables(0).Rows(0).Item("UGNFacility").ToString.Trim

                    ' ''Dim iDeptNo As Integer = 0
                    ' ''If ds.Tables(0).Rows(0).Item("DepartmentID") IsNot System.DBNull.Value Then
                    ' ''    If ds.Tables(0).Rows(0).Item("DepartmentID") > 0 Then
                    ' ''        iDeptNo = ds.Tables(0).Rows(0).Item("DepartmentID")
                    ' ''    End If
                    ' ''End If

                    '' ''UGNFacility-Department
                    ' ''If ViewState("ECINo") > 0 And iDeptNo > 0 And strFacility <> "" Then
                    ' ''    ECIModule.InsertECIFacilityDept(ViewState("ECINo"), strFacility, iDeptNo)
                    ' ''End If

                    ' ''ds = CostingModule.GetCostSheetCustomerProgram(CType(txtCostSheetID.Text.Trim, Integer))
                    ' ''If commonFunctions.CheckDataSet(ds) = True Then
                    ' ''    For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1



                    ' ''        iProgramID = 0
                    ' ''        If ds.Tables(0).Rows(iRowCounter).Item("ProgramID") IsNot System.DBNull.Value Then
                    ' ''            If ds.Tables(0).Rows(iRowCounter).Item("ProgramID") > 0 Then
                    ' ''                iProgramID = ds.Tables(0).Rows(iRowCounter).Item("ProgramID")
                    ' ''            End If
                    ' ''        End If

                    ' ''        iProgramYear = 0
                    ' ''        If ds.Tables(0).Rows(iRowCounter).Item("ProgramYear") IsNot System.DBNull.Value Then
                    ' ''            If ds.Tables(0).Rows(iRowCounter).Item("ProgramYear") > 0 Then
                    ' ''                iProgramYear = ds.Tables(0).Rows(iRowCounter).Item("ProgramYear")
                    ' ''            End If
                    ' ''        End If

                    ' ''        ECIModule.InsertECICustomerProgram(ViewState("ECINo"), False, "", "", iProgramID, iProgramYear, "", "")
                    ' ''        gvCustomerProgram.DataBind()
                    ' ''    Next
                    ' ''End If

                Else
                    lblMessage.Text &= "The cost sheet does not exist.<br />"
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub GetNewDrawingInfo()

        Try
            Dim ds As DataSet
            Dim iRowCounter As Integer = 0
            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0
            Dim isActiveProgram As Boolean = False

            'existing fields will NOT be overwritten
            If txtNewDrawingNo.Text.Trim <> "" Then
                ds = PEModule.GetDrawing(txtNewDrawingNo.Text.Trim)

                If commonFunctions.CheckDataSet(ds) = True Then
                    If txtNewCustomerPartNo.Text.Trim = "" Then
                        txtNewCustomerPartNo.Text = ds.Tables(0).Rows(0).Item("customerpartno").ToString.Trim
                    End If

                    If ddDesignationType.SelectedIndex <= 0 Then
                        If ds.Tables(0).Rows(0).Item("DesignationType") IsNot System.DBNull.Value Then
                            ddDesignationType.SelectedValue = ds.Tables(0).Rows(0).Item("DesignationType").ToString
                        End If
                    End If

                    If ddCommodity.SelectedIndex <= 0 Then
                        If ds.Tables(0).Rows(0).Item("CommodityID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("CommodityID") > 0 Then
                                ddCommodity.SelectedValue = ds.Tables(0).Rows(0).Item("CommodityID")
                            End If
                        End If
                    End If

                    If ddPurchasedGood.SelectedIndex <= 0 Then
                        If ds.Tables(0).Rows(0).Item("PurchasedGoodID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("PurchasedGoodID") > 0 Then
                                ddPurchasedGood.SelectedValue = ds.Tables(0).Rows(0).Item("PurchasedGoodID")
                            End If
                        End If
                    End If

                    If ddSubFamily.SelectedIndex <= 0 Then
                        If ds.Tables(0).Rows(0).Item("SubFamilyID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("SubFamilyID") > 0 Then
                                BindFamilySubFamily()

                                ddSubFamily.SelectedValue = ds.Tables(0).Rows(0).Item("SubFamilyID")

                                'get left 2 digits of subfamily
                                Dim strFamilyID As String = Left(CType(ddSubFamily.SelectedValue, String).PadLeft(4, "0"), 2)
                                If strFamilyID <> "" Then
                                    ddFamily.SelectedValue = CType(strFamilyID, Integer)
                                End If
                            End If
                        End If
                    End If

                    ds = PEModule.GetDrawingCustomerProgram(txtNewDrawingNo.Text.Trim)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                            iProgramID = 0
                            If ds.Tables(0).Rows(iRowCounter).Item("ProgramID") IsNot System.DBNull.Value Then
                                If ds.Tables(0).Rows(iRowCounter).Item("ProgramID") > 0 Then
                                    iProgramID = ds.Tables(0).Rows(iRowCounter).Item("ProgramID")
                                End If
                            End If

                            isActiveProgram = False
                            Dim dsProgram As DataSet
                            dsProgram = commonFunctions.GetPlatformProgram(0, iProgramID, "", "", "")
                            If commonFunctions.CheckDataSet(dsProgram) = True Then
                                If ds.Tables(0).Rows(iRowCounter).Item("Obsolete") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(iRowCounter).Item("Obsolete") = False And ds.Tables(0).Rows(iRowCounter).Item("RECSTATUS").ToString.Trim.ToUpper = "ACTIVE" Then
                                        isActiveProgram = True
                                    End If
                                End If
                            End If

                            iProgramYear = 0
                            If ds.Tables(0).Rows(iRowCounter).Item("ProgramYear") IsNot System.DBNull.Value Then
                                If ds.Tables(0).Rows(iRowCounter).Item("ProgramYear") > 0 Then
                                    iProgramYear = ds.Tables(0).Rows(iRowCounter).Item("ProgramYear")
                                End If
                            End If

                            lblMessage.Text = ""
                            If isActiveProgram = True And iProgramYear > 0 Then
                                ECIModule.InsertECICustomerProgram(ViewState("ECINo"), False, "", "", iProgramID, iProgramYear, "", "")
                            Else
                                lblMessage.Text &= "<br />Error: The program could not be copied from the DMS Drawing because it is not active or has no year assigned.<br />"
                            End If

                        Next
                    End If

                    gvCustomerProgram.DataBind()
                Else
                    lblMessage.Text &= "<br />The new drawing does not exist."
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

   

    Private Sub CheckDrawing()

        Try
            Dim ds As DataSet

            If txtNewDrawingNo.Text.Trim <> "" Then
                ds = PEModule.GetDrawing(txtNewDrawingNo.Text.Trim)

                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("approvalstatus").ToString = "N" Then
                        lblMessage.Text &= "WARNING: The DMS Drawing has not been issued yet.<br />"
                    End If
                Else
                    lblMessage.Text &= "WARNING: The DMS Drawing referenced does not exist.<br />"
                End If
            Else
                lblMessage.Text &= "WARNING: No DMS Drawing has been assigned to the ECI yet.<br />"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Shared Function GetOldRfcRfqInfo(ByVal RFQNo As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFC_RFQ_Info"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rfqNo", SqlDbType.Int)
            myCommand.Parameters("@rfqNo").Value = RFQNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "OldRfcRfqInfo")
            GetOldRfcRfqInfo = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFQNo: " & RFQNo & _
            ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetOldRfcRfqInfo : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetOldRfcRfqInfo : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECI_Detail.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetOldRfcRfqInfo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Protected Sub CheckRFD()

        Try

            Dim ds As DataSet

            If txtRFDNo.Text.Trim <> "" Then
                ds = GetOldRfcRfqInfo(CType(txtRFDNo.Text.Trim, Integer))
                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("RFQRFCFlag").ToString = "Q" And ddBusinessProcessType.SelectedValue = 2 Then
                        lblMessage.Text &= "<br />WARNING: This RFD selected should relate to the Business Process type of &quotCustomer Driven Change&quot<br />"
                    End If
                    If ds.Tables(0).Rows(0).Item("RFQRFCFlag").ToString = "C" And ddBusinessProcessType.SelectedValue = 1 Then
                        lblMessage.Text &= "<br />WARNING: This RFC selected should relate to the Business Process type of &quotUGN Driven Change&quot<br />"
                    End If
                End If
            Else
                lblMessage.Text &= "WARNING: No RFD has been assigned to the ECI yet.<br />"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnRelease_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRelease.Click

        Try

            ClearMessages()

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strTeamMemberEmails As String = ""

            If lblIssueDate.Text = "" Then

                'save new values, NOT on grids
                Call btnSave_Click(sender, e)

                'once the new RFD Module is built, then make this stricter
                If VendorValidate() = True Or ddECIType.SelectedValue = "Internal" Then
                    strTeamMemberEmails = BuildNotificationList()

                    If strTeamMemberEmails <> "" Then
                        SendEmail(strTeamMemberEmails)

                        'update ECI Completion Date and ECI Status
                        ECIModule.UpdateECIRelease(ViewState("ECINo"))

                        ViewState("StatusID") = 3

                        'refresh notification sent gridview
                        gvNotificationSent.DataBind()

                        BindData()

                        EnableControls()

                        UpdateDrawing()

                        lblMessage.Text &= "The ECI has been released.<br />"

                        ECIModule.InsertECIHistory(ViewState("ECINo"), lblMessage.Text)

                        gvHistory.DataBind()

                        If ddECIType.SelectedValue = "External" Then
                            'if production environment, then update UGNDatastore.dbo.ECI_CARS table for Rieter to use
                            If InStr(strProdOrTestEnvironment, "Prod_", CompareMethod.Text) > 0 Then
                                ECIModule.InsertECICAR(ViewState("ECINo"))
                            End If
                        End If

                    End If
                Else
                    lblMessage.Text &= "Error: The ECI was NOT released. At least one vendor is required.<br />"
                End If
            Else
                lblMessage.Text &= "Error: The ECI was already released<br />"
                btnRelease.Visible = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageECINotification.Text = lblMessage.Text

    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click

        Try
            ClearMessages()

            Dim iNewECINo As Integer = 0
            Dim iOldECINo As Integer = 0

            HttpContext.Current.Session("CopyECI") = Nothing

            ViewState("PreviousECINo") = ViewState("ECINo")
            iOldECINo = ViewState("ECINo")

            ViewState("ECINo") = 0
            lblECINo.Text = ""
            ViewState("ECIStatusID") = 1

            'save new values, NOT on grids
            Call btnSave_Click(sender, e)

            iNewECINo = ViewState("ECINo")

            If iNewECINo > 0 Then
                'need to copy and save grids
                ECIModule.CopyECICustomerProgram(iNewECINo, iOldECINo)
                ECIModule.CopyECIFacilityDept(iNewECINo, iOldECINo)
                ECIModule.CopyECINotificationGroup(iNewECINo, iOldECINo)
                ECIModule.CopyECITask(iNewECINo, iOldECINo)
                ECIModule.CopyECIVendor(iNewECINo, iOldECINo)

                If HttpContext.Current.Session("BLLerror") Is Nothing Then
                    lblMessage.Text &= "The information has been copied and saved.<br />"
                    HttpContext.Current.Session("CopyECI") = "Copied"

                    'refresh/redirect page
                    Response.Redirect("ECI_Detail.aspx?ECINo=" & iNewECINo, False)
                Else
                    lblMessage.Text &= HttpContext.Current.Session("BLLerror") & "<br />"
                End If
            Else
                btnCopy.Visible = False
                btnPreviewECI.Visible = False
                btnPreviewECIBottom.Visible = False
                btnPreviewUgnIPP.Visible = False
                btnPreviewUgnIPPBottom.Visible = False

                lblMessage.Text &= "Error: Copy failed.<br />"
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoid.Click

        Try
            ClearMessages()

            ViewState("ECIStatusID") = 4

            EnableControls()

            lblVoidComment.Visible = True
            lblVoidCommentMarker.Visible = True
            txtVoidComment.Visible = True
            txtVoidComment.Enabled = True

            btnVoid.Attributes.Add("onclick", "")

            btnCopy.Visible = False
            btnVoid.Visible = True

            If txtVoidComment.Text.Trim <> "" Then

                lblECIStatusValue.Text = ""

                ECIModule.DeleteECI(ViewState("ECINo"), txtVoidComment.Text.Trim)

                lblMessage.Text = "The ECI has been voided.<br />"
                btnVoid.Visible = False
            Else
                lblMessage.Text &= "To void an ECI, please fill in the reason in the Void Comment field and then CLICK THE VOID BUTTON AGAIN."
                txtVoidComment.Focus()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnAddToCustomerProgram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddToCustomerProgram.Click

        Try

            ClearMessages()

            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0

            Dim dSOP As DateTime
            Dim dEOP As DateTime

            'If ViewState("CurrentCustomerProgramID") = 0 Then 'And ddProgram.SelectedIndex >= 0 Then
            '    ViewState("CurrentCustomerProgramID") = ddProgram.SelectedValue
            'End If

            'iProgramID = ViewState("CurrentCustomerProgramID")
            If ViewState("CurrentCustomerProgramRow") > 0 Then
                iProgramID = ViewState("CurrentCustomerProgramID")
            Else
                'If ViewState("CurrentCustomerProgramID") = 0 Then 'And ddProgram.SelectedIndex >= 0 Then
                ViewState("CurrentCustomerProgramID") = ddProgram.SelectedValue
                'End If

                iProgramID = ViewState("CurrentCustomerProgramID")
            End If

            If InStr(ddProgram.SelectedItem.Text, "**") > 0 And ViewState("CurrentCustomerProgramRow") = 0 Then
                lblMessage.Text &= "Error: An obsolete program cannot be selected. The information was NOT saved."
            Else
                'iProgramID = ddProgram.SelectedValue

                'make sure Year Selected is in range of SOP and EOP
                If ddYear.SelectedIndex > 0 Then
                    iProgramYear = ddYear.SelectedValue

                    If txtSOPDate.Text.Trim <> "" Then
                        dSOP = CType(txtSOPDate.Text.Trim, DateTime)

                        If iProgramYear < dSOP.Year Then
                            iProgramYear = dSOP.Year
                        End If
                    End If

                    If txtEOPDate.Text.Trim <> "" Then
                        dEOP = CType(txtEOPDate.Text.Trim, DateTime)

                        If iProgramYear > dEOP.Year Then
                            iProgramYear = dEOP.Year
                        End If
                    End If
                End If

                If iProgramYear > 0 Then
                    If ViewState("CurrentCustomerProgramRow") > 0 Then
                        '(LREY) 01/08/2014
                        ECIModule.UpdateECICustomerProgram(ViewState("CurrentCustomerProgramRow"), cbCustomerApprovalRequired.Checked, txtCustomerApprovalDate.Text, txtCustomerApprovalNo.Text, iProgramID, iProgramYear, txtSOPDate.Text, txtEOPDate.Text)
                    Else
                        '(LREY) 01/08/2014
                        ECIModule.InsertECICustomerProgram(ViewState("ECINo"), cbCustomerApprovalRequired.Checked, txtCustomerApprovalDate.Text, txtCustomerApprovalNo.Text, iProgramID, iProgramYear, txtSOPDate.Text, txtEOPDate.Text)
                    End If

                    ClearCustomerProgramInputFields()

                    If HttpContext.Current.Session("BLLerror") IsNot Nothing Then
                        lblMessage.Text &= HttpContext.Current.Session("BLLerror")
                    Else
                        HttpContext.Current.Session("BLLerror") = Nothing
                        lblMessage.Text &= "Program and Customer were added or updated."
                    End If
                End If

            End If
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerProgram.Text = lblMessage.Text

    End Sub


    Protected Sub btnCancelEditCustomerProgram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelEditCustomerProgram.Click

        Try
            ClearMessages()

            ClearCustomerProgramInputFields()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageCustomerProgram.Text = lblMessage.Text
    End Sub

    Protected Sub gvNotificationGroup_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvNotificationGroup.DataBound

        'hide header of first column
        If gvNotificationGroup.Rows.Count > 0 Then
            gvNotificationGroup.HeaderRow.Cells(0).Visible = False
        End If

    End Sub

    Protected Sub gvNotificationGroup_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvNotificationGroup.RowCommand

        Try

            ClearMessages()

            Dim ddGroupTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("ECINo") > 0) Then

                ddGroupTemp = CType(gvNotificationGroup.FooterRow.FindControl("ddInsertNotificationGroup"), DropDownList)

                If ddGroupTemp.SelectedIndex > 0 Then
                    odsNotificationGroup.InsertParameters("ECINo").DefaultValue = ViewState("ECINo")
                    odsNotificationGroup.InsertParameters("GroupID").DefaultValue = ddGroupTemp.SelectedValue

                    intRowsAffected = odsNotificationGroup.Insert()

                    lblMessage.Text &= "Record Saved Successfully.<br />"

                    gvNotificationGroup.DataBind()
                Else
                    lblMessage.Text &= "ERROR: A notification group is required.<br />"
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvNotificationGroup.ShowFooter = False
            Else
                gvNotificationGroup.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddGroupTemp = CType(gvNotificationGroup.FooterRow.FindControl("ddInsertNotificationGroup"), DropDownList)
                ddGroupTemp.SelectedIndex = -1
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageECINotification.Text = lblMessage.Text

    End Sub

    Protected Sub gvNotificationGroup_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvNotificationGroup.RowCreated

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

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_NotificationGroup
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br />"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub odsNotificationGroup_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsNotificationGroup.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As ECI.ECINotificationGroup_MaintDataTable = CType(e.ReturnValue, ECI.ECINotificationGroup_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_NotificationGroup = True
            Else
                LoadDataEmpty_NotificationGroup = False
            End If
        End If

    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Try
            ClearMessages()

            ViewState("isOverride") = True

            txtEmailComments.Text = "This notification is being sent to inform you of an UPDATED ECI WHICH HAS ALREADY BEEN RELEASED. Upon receipt of this notification, please take the necessary steps as indicated in the ECI fields."

            If ddECIType.SelectedValue = "External" Then
                If rbPPAP.SelectedValue = 1 Then
                    rfvPPAPLevel.Enabled = True
                    If rbPPAP.SelectedValue = 1 Then
                        rfvPPAPLevel.ValidationGroup = "vgSave"
                    End If
                Else
                    rfvPPAPLevel.Enabled = False
                End If

            Else
                rfvCommodity.ValidationGroup = "vgSave"
                rfvProductTechnology.ValidationGroup = "vgSave"
            End If

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Function BuildNotificationList() As String

        Dim strTeamMemberEmails As String = ""

        Try
            Dim ds As DataSet
            Dim dsNotificationGroup As DataSet
            Dim dsBackup As DataSet
            Dim dsTeamMember As DataSet

            Dim iDrawingByTeamMemberID As Integer = 0

            Dim iNotificationGroup As Integer = 0
            Dim iNotificationGroupRowCounter As Integer = 0

            Dim iTeamMemberID As Integer = 0
            Dim iTeamMemberRowCounter As Integer = 0

            dsNotificationGroup = ECIModule.GetECINotificationGroup(ViewState("ECINo"))

            'get all notification groups for this eci
            If commonFunctions.CheckDataSet(dsNotificationGroup) = True Then

                'loop through all notification groups
                For iNotificationGroupRowCounter = 0 To dsNotificationGroup.Tables(0).Rows.Count - 1
                    iNotificationGroup = dsNotificationGroup.Tables(0).Rows(iNotificationGroupRowCounter).Item("GroupID")
                    'get notification group emails and backups based on corporate calendar                        
                    ds = ECIModule.GetECIGroupTeamMember(iNotificationGroup, 0)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        For iTeamMemberRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(iTeamMemberRowCounter).Item("Email").ToString <> "" Then

                                'do not insert duplicate emails
                                If InStr(strTeamMemberEmails, ds.Tables(0).Rows(iTeamMemberRowCounter).Item("Email").ToString) <= 0 Then
                                    If strTeamMemberEmails <> "" Then
                                        strTeamMemberEmails &= ";"
                                    End If

                                    strTeamMemberEmails &= ds.Tables(0).Rows(iTeamMemberRowCounter).Item("Email").ToString
                                    iTeamMemberID = ds.Tables(0).Rows(iTeamMemberRowCounter).Item("TeamMemberID")

                                    'save notification sent date
                                    ECIModule.InsertECINotification(ViewState("ECINo"), iTeamMemberID)

                                    'get backup users based on corporate calendar
                                    dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTeamMemberID, 64)
                                    If commonFunctions.CheckDataSet(dsBackup) = True Then
                                        If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 Then

                                            If InStr(strTeamMemberEmails, dsBackup.Tables(0).Rows(0).Item("Email")) <= 0 Then
                                                If strTeamMemberEmails <> "" Then
                                                    strTeamMemberEmails &= ";"
                                                End If

                                                strTeamMemberEmails &= dsBackup.Tables(0).Rows(0).Item("BackupEmail")
                                                iTeamMemberID = dsBackup.Tables(0).Rows(iTeamMemberRowCounter).Item("BackupID")

                                                'save notification sent date
                                                ECIModule.InsertECINotification(ViewState("ECINo"), iTeamMemberID)
                                            End If

                                            ViewState("BackupTeamMembers") &= "<br />" & dsBackup.Tables(0).Rows(0).Item("BackupFullName").ToString & ": is assigned as backup for: " & ds.Tables(0).Rows(0).Item("LastName").ToString & ", " & ds.Tables(0).Rows(0).Item("FirstName").ToString & " until " & dsBackup.Tables(0).Rows(0).Item("EndDate").ToString & ".<br />"
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next

                'get email addresses of team members assigned activities
                ds = ECIModule.GetECITask(ViewState("ECINo"))
                If commonFunctions.CheckDataSet(ds) = True Then
                    For iTeamMemberRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                        If ds.Tables(0).Rows(iTeamMemberRowCounter).Item("Email").ToString <> "" Then

                            'do not insert duplicate emails
                            If InStr(strTeamMemberEmails, ds.Tables(0).Rows(iTeamMemberRowCounter).Item("Email").ToString) <= 0 Then
                                If strTeamMemberEmails <> "" Then
                                    strTeamMemberEmails &= ";"
                                End If

                                strTeamMemberEmails &= ds.Tables(0).Rows(iTeamMemberRowCounter).Item("Email").ToString

                                iTeamMemberID = ds.Tables(0).Rows(iTeamMemberRowCounter).Item("TaskTeamMemberID")

                                'save notification sent date
                                ECIModule.InsertECINotification(ViewState("ECINo"), iTeamMemberID)

                                'get backup users based on corporate calendar
                                dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTeamMemberID, 64)
                                If commonFunctions.CheckDataSet(dsBackup) = True Then
                                    If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 Then

                                        If InStr(strTeamMemberEmails, dsBackup.Tables(0).Rows(0).Item("Email")) <= 0 Then
                                            If strTeamMemberEmails <> "" Then
                                                strTeamMemberEmails &= ";"
                                            End If

                                            strTeamMemberEmails &= dsBackup.Tables(0).Rows(0).Item("BackupEmail")
                                            iTeamMemberID = dsBackup.Tables(0).Rows(iTeamMemberRowCounter).Item("BackupID")

                                            'save notification sent date
                                            ECIModule.InsertECINotification(ViewState("ECINo"), iTeamMemberID)
                                        End If

                                        ViewState("BackupTeamMembers") &= "<br />" & dsBackup.Tables(0).Rows(0).Item("BackupFullName").ToString & ": is assigned as backup for: " & ds.Tables(0).Rows(0).Item("LastName").ToString & ", " & ds.Tables(0).Rows(0).Item("FirstName").ToString & " until " & dsBackup.Tables(0).Rows(0).Item("EndDate").ToString & ".<br />"
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If

                'append Product Development team member who created the drawing
                If txtNewDrawingNo.Text.Trim <> "" Then
                    ds = PEModule.GetDrawing(txtNewDrawingNo.Text.Trim)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        If ds.Tables(0).Rows(0).Item("DrawingByEngineerID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("DrawingByEngineerID") > 0 Then
                                iDrawingByTeamMemberID = ds.Tables(0).Rows(0).Item("DrawingByEngineerID")

                                'initiator email
                                dsTeamMember = SecurityModule.GetTeamMember(iDrawingByTeamMemberID, "", "", "", "", "", True, Nothing)
                                If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                    If dsTeamMember.Tables(0).Rows(0).Item("Email").ToString <> "" Then
                                        If InStr(strTeamMemberEmails, dsTeamMember.Tables(0).Rows(0).Item("Email").ToString) <= 0 Then
                                            If strTeamMemberEmails.Trim <> "" Then
                                                strTeamMemberEmails &= ";"
                                            End If

                                            strTeamMemberEmails &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString

                                            'save notification sent date
                                            ECIModule.InsertECINotification(ViewState("ECINo"), iDrawingByTeamMemberID)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If

            Else
                strTeamMemberEmails = ""
                lblMessage.Text = "Error: The ECI was NOT updated. At least one notification group is required.<br />"
            End If

        Catch ex As Exception
            strTeamMemberEmails = ""

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        Return strTeamMemberEmails

    End Function

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click, btnUpdateFooter.Click

        Try
            ClearMessages()

            Dim strTeamMemberEmails As String = ""

            If ViewState("isOverride") = True Then
                btnSave_Click(sender, e)

                're-notify everyone that the ECI was updated
                If VendorValidate() = True Or ddECIType.SelectedValue = "Internal" Then
                    strTeamMemberEmails = BuildNotificationList()

                    If strTeamMemberEmails <> "" Then
                        SendEmail(strTeamMemberEmails)

                        'refresh notification sent gridview
                        gvNotificationSent.DataBind()

                        ViewState("isOverride") = False

                        EnableControls()

                        lblMessage.Text = "The ECI has been updated and the announcement has been sent.<br />"

                        ECIModule.InsertECIHistory(ViewState("ECINo"), lblMessage.Text)

                        gvHistory.DataBind()
                    End If

                Else
                    lblMessage.Text = "Error: The ECI was NOT updated. At least one vendor is required.<br />"
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnCancelEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelEdit.Click

        Try
            ClearMessages()

            ViewState("isOverride") = False

            BindData()

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub rbPPAP_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbPPAP.SelectedIndexChanged

        Try
            If rbPPAP.SelectedValue = 1 Then
                rfvPPAPLevel.Enabled = True
            Else
                rfvPPAPLevel.Enabled = False
                ddPPAPLevel.SelectedIndex = 0
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvVendor_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvVendor.RowDeleted

        Try
            btnRelease.Visible = VendorValidate()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvVendor_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvVendor.RowUpdated

        Try
            btnRelease.Visible = VendorValidate()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ddProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProgram.SelectedIndexChanged

        Try

            ClearMessages()

            'If ddProgram.SelectedIndex >= 0 And ddPlatform.SelectedIndex >= 0 And ddMakes.SelectedIndex >= 0 Then
            If ddProgram.SelectedIndex >= 0 And ddMakes.SelectedIndex >= 0 Then
                'System.Threading.Thread.Sleep(3000)
                ViewState("CurrentCustomerProgramID") = ddProgram.SelectedValue

                Dim ds As DataSet = New DataSet
                'ds = commonFunctions.GetPlatformProgram(ddPlatform.SelectedValue, ddProgram.SelectedValue, "", "", ddMakes.SelectedValue)
                ds = commonFunctions.GetPlatformProgram(0, ddProgram.SelectedValue, "", "", ddMakes.SelectedValue)
                If commonFunctions.CheckDataSet(ds) = True Then
                    Dim NoOfDays As String = ""
                    Select Case ds.Tables(0).Rows(0).Item("EOPMM").ToString.Trim
                        Case "01"
                            NoOfDays = "31"
                        Case "02"
                            NoOfDays = "28"
                        Case "03"
                            NoOfDays = "31"
                        Case "04"
                            NoOfDays = "30"
                        Case "05"
                            NoOfDays = "31"
                        Case "06"
                            NoOfDays = "30"
                        Case "07"
                            NoOfDays = "31"
                        Case "08"
                            NoOfDays = "31"
                        Case "09"
                            NoOfDays = "30"
                        Case 10
                            NoOfDays = "31"
                        Case 11
                            NoOfDays = "30"
                        Case 12
                            NoOfDays = "31"
                    End Select
                    If ds.Tables(0).Rows(0).Item("EOPMM").ToString.Trim <> "" Then
                        txtEOPDate.Text = ds.Tables(0).Rows(0).Item("EOPMM").ToString() & "/" & NoOfDays & "/" & ds.Tables(0).Rows(0).Item("EOPYY").ToString()
                    End If
                    If ds.Tables(0).Rows(0).Item("SOPMM").ToString.Trim <> "" Then
                        txtSOPDate.Text = ds.Tables(0).Rows(0).Item("SOPMM").ToString.Trim & "/01/" & ds.Tables(0).Rows(0).Item("SOPYY").ToString.Trim

                        ''pick current year if inside SOP and EOP range 
                        'If ddYear.SelectedIndex > 0 Then
                        '    If ds.Tables(0).Rows(0).Item("SOPYY") < Today.Year And Today.Year <= ds.Tables(0).Rows(0).Item("EOPYY") Then
                        '        ddYear.SelectedValue = Today.Year
                        '    Else
                        '        ddYear.SelectedValue = ds.Tables(0).Rows(0).Item("SOPYY").ToString.Trim
                        '    End If
                        'End If

                    End If
                    '2012-Mar-03 - temporarily disabled - requested by Lynette
                    'iBtnPreviewDetail.Visible = True
                    'Dim strPreviewClientScript2 As String = "javascript:void(window.open('../DataMaintenance/ProgramDisplay.aspx?pPlatID=0&pPgmID=" & ddProgram.SelectedValue & " '," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=yes,location=yes'));"
                    'iBtnPreviewDetail.Attributes.Add("Onclick", strPreviewClientScript2)
                    'Else
                    '    iBtnPreviewDetail.Visible = False
                End If
            End If 'EOF ddProgram.SelectedValue

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Function DisplayImage(ByVal EncodeType As String) As String
        Dim strReturn As String = ""

        If EncodeType = Nothing Then
            strReturn = ""
        ElseIf EncodeType = "application/vnd.ms-excel" Or EncodeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" Then
            strReturn = "~/images/xls.jpg"
        ElseIf EncodeType = "application/pdf" Then
            strReturn = "~/images/pdf.jpg"
        ElseIf EncodeType = "application/msword" Or EncodeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" Then
            strReturn = "~/images/doc.jpg"
        Else
            strReturn = "~/images/PreviewUp.jpg"
        End If

        Return strReturn
    End Function 'EOF DisplayImage
End Class
