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

' ************************************************************************************************
' Name:	PlatformMaintenance.aspx.vb
' Purpose:	This Platform is used to view, insert, update Platform information
'
' Date		    Author	    
' 04/19/2011    LRey            Created .Net application
' ************************************************************************************************

Partial Class DataMaintenance_PlatformMaintenance
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False

    '#Region "Properties & Fields"

    '    ''' <summary> 
    '    ''' List of column names 
    '    ''' </summary> 
    '    Private columnNames As List(Of String) = New List(Of String)(New String() _
    '        {"", "", "", "", "CSM Platform", "WAF Platform", "", "", "", "", "", "Notes", "Last Update"})

    '    Private Property hiddenColumnIndexes() As List(Of Integer)
    '        Get
    '            Return If(ViewState("hiddenColumnIndexes") Is Nothing, New List(Of Integer)(), DirectCast(ViewState("hiddenColumnIndexes"), List(Of Integer)))
    '        End Get
    '        Set(ByVal value As List(Of Integer))
    '            ViewState("hiddenColumnIndexes") = value
    '        End Set
    '    End Property

    '#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Platform"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Platform"
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
                ViewState("sPName") = ""
                ViewState("sOEMMF") = ""
                ViewState("sDUB") = ""
                ViewState("sDCP") = ""

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("P_PNAME") Is Nothing Then
                    txtPlatformName.Text = Server.HtmlEncode(Request.Cookies("P_PNAME").Value)
                    ViewState("sPName") = Server.HtmlEncode(Request.Cookies("P_PNAME").Value)
                End If

                If Not Request.Cookies("P_OEMMF") Is Nothing Then
                    txtOEMManufacturer.Text = Server.HtmlEncode(Request.Cookies("P_OEMMF").Value)
                    ViewState("sOEMMF") = Server.HtmlEncode(Request.Cookies("P_OEMMF").Value)
                End If

                If Not Request.Cookies("P_DUB") Is Nothing Then
                    ddDispUGNBusiness.SelectedValue = Server.HtmlEncode(Request.Cookies("P_DUB").Value)
                    ViewState("sDUB") = Server.HtmlEncode(Request.Cookies("P_DUB").Value)
                End If

                If Not Request.Cookies("P_DCP") Is Nothing Then
                    ddDispCurrentPlatform.SelectedValue = Server.HtmlEncode(Request.Cookies("P_DCP").Value)
                    ViewState("sDCP") = Server.HtmlEncode(Request.Cookies("P_DCP").Value)
                End If

                ''Enable the show hide columns
                ' ''hiddenColumnIndexes = New List(Of Integer)()
            Else
                ViewState("sPName") = txtPlatformName.Text
                ViewState("sOEMMF") = txtOEMManufacturer.Text
                ViewState("sDUB") = ddDispUGNBusiness.SelectedValue
                ViewState("sDCP") = ddDispCurrentPlatform.SelectedValue
            End If

            CheckRights()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF Page_Load

    Protected Sub CheckRights()

        Try
            'default to hide edit column
            ' gvPlatformList.Columns(gvPlatformList.Columns.Count - 1).Visible = False
            If gvPlatformList.FooterRow IsNot Nothing Then
                gvPlatformList.FooterRow.Visible = False
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            ViewState("ObjectRole") = False

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 122)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("ObjectRole") = True
                                    gvPlatformList.Columns(1).Visible = True
                                    'gvPlatformList.Columns(gvPlatformList.Columns.Count - 1).Visible = True
                                    If gvPlatformList.FooterRow IsNot Nothing Then
                                        gvPlatformList.FooterRow.Visible = True
                                    End If
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("ObjectRole") = True
                                    gvPlatformList.Columns(1).Visible = True
                                    'gvPlatformList.Columns(gvPlatformList.Columns.Count - 1).Visible = True
                                    If gvPlatformList.FooterRow IsNot Nothing Then
                                        gvPlatformList.FooterRow.Visible = True
                                    End If
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("ObjectRole") = True
                                    gvPlatformList.Columns(1).Visible = False
                                    'gvPlatformList.Columns(gvPlatformList.Columns.Count - 1).Visible = False
                                    If gvPlatformList.FooterRow IsNot Nothing Then
                                        gvPlatformList.FooterRow.Visible = True
                                    End If
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    gvPlatformList.Columns(0).Visible = False
                                    'gvPlatformList.Columns(gvPlatformList.Columns.Count - 1).Visible = False
                                    If gvPlatformList.FooterRow IsNot Nothing Then
                                        gvPlatformList.FooterRow.Visible = False
                                    End If
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    gvPlatformList.Columns(1).Visible = False
                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                    gvPlatformList.Columns(0).Visible = False

                            End Select
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

    End Sub 'EOF CheckRights

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Response.Cookies("P_PNAME").Value = txtPlatformName.Text
            Response.Cookies("P_OEMMF").Value = txtOEMManufacturer.Text
            Response.Cookies("P_DUB").Value = ddDispUGNBusiness.SelectedValue
            Response.Cookies("P_DCP").Value = ddDispCurrentPlatform.SelectedValue

            Response.Redirect("PlatformMaintenance.aspx?sPName=" & ViewState("sPName") & "&sOEMMF=" & ViewState("sOEMMF") & "&sDUB=" & ViewState("sDUB") & "&sDCP=" & ViewState("sDCP"), False)

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            commonFunctions.DeletePlatformCookies()

            Response.Redirect("PlatformMaintenance.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click

    ' ''#Region "Control Events"

    ' ''    Protected Sub gvPlatformListShowHideColumns_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPlatformListShowHideColumns.SelectedIndexChanged

    ' ''        If Me.gvPlatformListShowHideColumns.SelectedIndex > 0 Then
    ' ''            Dim columnIndex2 As Integer = Integer.Parse(Me.gvPlatformListShowHideColumns.SelectedValue)
    ' ''            hiddenColumnIndexes.Remove(columnIndex2)

    ' ''            SetupShowHideColumns()
    ' ''        End If
    ' ''    End Sub 'EOF gvPlatformListShowHideColumns_SelectedIndexChanged

    ' ''#End Region 'Control Events

#Region "GridView Events"
    Protected Sub gvPlatformList_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            lblMessage.Text = Nothing
            lblMessage.Visible = False
            Dim PlatformName As TextBox
            ' Dim CSMPlatform As TextBox
            ' Dim WAFPlatform As TextBox
            Dim OEMMfg As DropDownList
            Dim BegYear As TextBox
            Dim EndYear As TextBox
            Dim UGNBusiness As DropDownList
            Dim CurrentPlatform As DropDownList
            Dim ServiceYears As TextBox
            Dim Notes As TextBox


            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then

                PlatformName = CType(gvPlatformList.FooterRow.FindControl("txtPlatformNameGV"), TextBox)
                odsPlatformList.InsertParameters("PlatformName").DefaultValue = PlatformName.Text

                ' CSMPlatform = Nothing ''CType(gvPlatformList.FooterRow.FindControl("txtCSMPlatformGV"), TextBox)
                'odsPlatformList.InsertParameters("CSMPlatform").DefaultValue = "" 'CSMPlatform.Text

                ' WAFPlatform = Nothing ''CType(gvPlatformList.FooterRow.FindControl("txtWAFPlatformGV"), TextBox)
                'odsPlatformList.InsertParameters("WAFPlatform").DefaultValue = "" 'WAFPlatform.Text

                OEMMfg = CType(gvPlatformList.FooterRow.FindControl("ddOEMMfg"), DropDownList)
                odsPlatformList.InsertParameters("OEMManufacturer").DefaultValue = OEMMfg.Text

                BegYear = CType(gvPlatformList.FooterRow.FindControl("txtBegYearGV"), TextBox)
                odsPlatformList.InsertParameters("BegYear").DefaultValue = BegYear.Text

                EndYear = CType(gvPlatformList.FooterRow.FindControl("txtEndYearGV"), TextBox)
                odsPlatformList.InsertParameters("EndYear").DefaultValue = EndYear.Text

                UGNBusiness = CType(gvPlatformList.FooterRow.FindControl("ddUGNBusinessGV"), DropDownList)
                odsPlatformList.InsertParameters("UGNBusiness").DefaultValue = UGNBusiness.SelectedValue

                CurrentPlatform = CType(gvPlatformList.FooterRow.FindControl("ddCurrentPlatformGV"), DropDownList)
                odsPlatformList.InsertParameters("UGNBusiness").DefaultValue = CurrentPlatform.SelectedValue

                ServiceYears = CType(gvPlatformList.FooterRow.FindControl("txtSrvYrsGV"), TextBox)
                odsPlatformList.InsertParameters("ServiceYears").DefaultValue = ServiceYears.Text

                Notes = CType(gvPlatformList.FooterRow.FindControl("txtNotesGV"), TextBox)
                odsPlatformList.InsertParameters("Notes").DefaultValue = Notes.Text

                odsPlatformList.Insert()

                '' Indicate that the user needs to be sent to the last page
                SendUserToLastPage = True
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvPlatformList.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvPlatformList.ShowFooter = True
                Else
                    gvPlatformList.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then

                PlatformName = CType(gvPlatformList.FooterRow.FindControl("txtPlatformNameGV"), TextBox)
                PlatformName.Text = Nothing

                'CSMPlatform = CType(gvPlatformList.FooterRow.FindControl("txtCSMPlatformGV"), TextBox)
                'CSMPlatform.Text = Nothing

                'WAFPlatform = CType(gvPlatformList.FooterRow.FindControl("txtWAFPlatformGV"), TextBox)
                'WAFPlatform.Text = Nothing

                OEMMfg = CType(gvPlatformList.FooterRow.FindControl("ddOEMMfg"), DropDownList)
                OEMMfg.Text = Nothing

                BegYear = CType(gvPlatformList.FooterRow.FindControl("txtBegYearGV"), TextBox)
                BegYear.Text = Nothing

                EndYear = CType(gvPlatformList.FooterRow.FindControl("txtEndYearGV"), TextBox)
                EndYear.Text = Nothing

                UGNBusiness = CType(gvPlatformList.FooterRow.FindControl("ddUGNBusinessGV"), DropDownList)
                UGNBusiness.SelectedValue = False

                CurrentPlatform = CType(gvPlatformList.FooterRow.FindControl("ddCurrentPlatformGV"), DropDownList)
                CurrentPlatform.SelectedValue = False

                ServiceYears = CType(gvPlatformList.FooterRow.FindControl("txtSrvYrsGV"), TextBox)
                odsPlatformList.InsertParameters("ServiceYears").DefaultValue = ServiceYears.Text

                Notes = CType(gvPlatformList.FooterRow.FindControl("txtNotesGV"), TextBox)
                Notes.Text = Nothing
            End If


            ' ''If e.CommandName = "imghideCol" Then
            ' ''    ' Add the column index to hide to the hiddenColumnIndexes list 
            ' ''    hiddenColumnIndexes.Add(Integer.Parse(e.CommandArgument.ToString()))
            ' ''End If

            ' ''SetupShowHideColumns()
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF gvPlatformList_RowCommand

#End Region 'GridView Events

#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_Platform() As Boolean
        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.
        Get
            If ViewState("LoadDataEmpty_Platform") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Platform"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Platform") = value
        End Set
    End Property 'EOF LoadDataEmpty_Platform

    Protected Sub odsPlatformList_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsPlatformList.Selected
        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)
        Dim dt As Platform.PlatformDataTable = CType(e.ReturnValue, Platform.PlatformDataTable)

        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty_Platform = True
        Else
            LoadDataEmpty_Platform = False
        End If
    End Sub 'EOF odsPlatformList_Selected

    Protected Sub gvPlatformList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPlatformList.RowCreated
        ' when binding a row, look for a zero row condition based on the flag.
        ' if we have zero data rows (but a dummy row), hide the grid view row
        ' and clear the controls off of that row so they don't cause binding errors
        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Platform
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If

        ''*******************************************************************
        ' For the header row add a link button to each header 
        ' cell which can execute a row command 
        ' ''If e.Row.RowType = DataControlRowType.Header Then
        ' ''    ' Loop through each cell of the header row 
        ' ''    For columnIndex2 As Integer = 4 To 5
        ' ''        Dim hideLinkImg As New ImageButton
        ' ''        hideLinkImg.CommandName = "imghideCol"
        ' ''        hideLinkImg.CommandArgument = columnIndex2.ToString()
        ' ''        hideLinkImg.ImageUrl = "~\images\collapseLeft.jpg"
        ' ''        hideLinkImg.CssClass = "gvHideColLink"
        ' ''        hideLinkImg.Attributes.Add("title", "Hide Column")

        ' ''        ' Add the "Hide Column" ImageButton to the header cell 
        ' ''        e.Row.Cells(columnIndex2).Controls.AddAt(0, hideLinkImg)

        ' ''        ' If there is column header text then 
        ' ''        ' add it back to the header cell as a label 
        ' ''        If e.Row.Cells(columnIndex2).Text.Length > 0 Then
        ' ''            Dim columnTextLabel As New Label()
        ' ''            columnTextLabel.Text = e.Row.Cells(columnIndex2).Text
        ' ''            e.Row.Cells(columnIndex2).Controls.Add(columnTextLabel)
        ' ''        End If
        ' ''    Next

        ' ''    For columnIndex2 As Integer = 11 To 12
        ' ''        Dim hideLinkImg As New ImageButton
        ' ''        hideLinkImg.CommandName = "imghideCol"
        ' ''        hideLinkImg.CommandArgument = columnIndex2.ToString()
        ' ''        hideLinkImg.ImageUrl = "~\images\collapseLeft.jpg"
        ' ''        hideLinkImg.CssClass = "gvHideColLink"
        ' ''        hideLinkImg.Attributes.Add("title", "Hide Column")

        ' ''        'Add the "Hide Column" ImageButton to the header cell 
        ' ''        e.Row.Cells(columnIndex2).Controls.AddAt(0, hideLinkImg)

        ' ''        ' If there is column header text then 
        ' ''        ' add it back to the header cell as a label 
        ' ''        If e.Row.Cells(columnIndex2).Text.Length > 0 Then
        ' ''            Dim columnTextLabel As New Label()
        ' ''            columnTextLabel.Text = e.Row.Cells(columnIndex2).Text
        ' ''            e.Row.Cells(columnIndex2).Controls.Add(columnTextLabel)
        ' ''        End If
        ' ''    Next
        ' ''End If

        '' '' Hide the column indexes which have been stored in hiddenColumnIndexes 
        ' ''For Each columnIndex2 As Integer In hiddenColumnIndexes
        ' ''    If columnIndex2 < e.Row.Cells.Count Then
        ' ''        e.Row.Cells(columnIndex2).Visible = False
        ' ''    End If
        ' ''Next
    End Sub 'EOF gvPlatformList_RowCreated
#End Region ' Insert Empty GridView Work-Around

    ' ''#Region "Private Methods"

    ' ''    ''' <summary> 
    ' ''    ''' Setup the drop down list, adding options based on the hiddenColumnIndexes list 
    ' ''    ''' </summary> 
    ' ''    Private Sub SetupShowHideColumns()
    ' ''        Me.gvPlatformListShowHideColumns.Items.Clear()

    ' ''        If hiddenColumnIndexes.Count > 0 Then
    ' ''            Me.gvPlatformListShowHideColumns.Visible = True
    ' ''            Me.gvPlatformListShowHideColumns.Items.Add(New ListItem("-Show Column-", "-1"))

    ' ''            For Each i As Integer In hiddenColumnIndexes
    ' ''                Me.gvPlatformListShowHideColumns.Items.Add(New ListItem(columnNames(i), i.ToString()))
    ' ''            Next
    ' ''        Else
    ' ''            Me.gvPlatformListShowHideColumns.Visible = False
    ' ''        End If

    ' ''    End Sub 'EOF SetupShowHideColumns
    ' ''#End Region 'Private Methods

    Protected Function DisplayImage(ByVal OEMManufacturer As String) As String
        Dim strReturnValue = "~\images\previewup.jpg"
        Select Case OEMManufacturer
            Case "TOYOTA"
                strReturnValue = "~\images\TOYOTA.jpg"
            Case "SUBARU"
                strReturnValue = "~\images\SUBARU.jpg"
            Case "CHRYSLER"
                strReturnValue = "~\images\CHRYSLER.jpg"
            Case "FORD"
                strReturnValue = "~\images\FORD.jpg"
            Case "MAZDA"
                strReturnValue = "~\images\MAZDA.jpg"
            Case "HYUNDAI"
                strReturnValue = "~\images\HYUNDAI.jpg"
            Case "KIA"
                strReturnValue = "~\images\KIA.jpg"
            Case "NISSAN"
                strReturnValue = "~\images\NISSAN.jpg"
            Case "HONDA"
                strReturnValue = "~\images\HONDA.jpg"
            Case "MITSUBISHI"
                strReturnValue = "~\images\MITSU.jpg"
            Case "BMW"
                strReturnValue = "~\images\BMW.jpg"
            Case "VOLKSWAGEN"
                strReturnValue = "~\images\VW.jpg"
            Case "GENERAL MOTORS"
                strReturnValue = "~\images\GM.jpg"
            Case "NUMMI"
                strReturnValue = "~\images\NUMMI.jpg"
            Case "DAIMLER"
                strReturnValue = "~\images\BENZ.jpg"
            Case Else
                strReturnValue = "~\images\car1.jpg"

        End Select

        DisplayImage = strReturnValue

    End Function

End Class
