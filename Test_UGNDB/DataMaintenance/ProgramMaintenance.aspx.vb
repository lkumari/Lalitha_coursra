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
' Name:	ProgramMaintenance.aspx.vb
' Purpose:	This program is used to view, insert, update Program information
'
' Date		    Author	    
' 05/06/2009    LREY                Created .Net application

Partial Class DataMaintenance_ProgramMaintenance
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False
#Region "Properties & Fields"

    ''' <summary> 
    ''' List of column names 
    ''' </summary> 
    Private columnNames As List(Of String) = New List(Of String)(New String() _
       {"", "", "", "", "", "Mnemonic Platform **", "Mnemonic Vehicle **", "Mnemonic Vehicle Plant **", "IHS Program Code", "IHS Model Name", "", "", "", "", "Vehicle Type **", "Assembly Plant Location **", "", "", "Service Assembly Plant Location", "Service EOP", "UGN Business **", "Obsolete", "Notes", "Last Update"})
    ' ''{"", "", "", "", "", "Mnemonic Platform **", "Mnemonic Vehicle **", "Mnemonic Vehicle Plant **", "Mnemonic Vehicle Bodystyle **", "Mnemonic Vehicle Bodystyle Plant **", "IHS Program Code", "IHS Model Name", "", "", "", "", "Vehicle Type **", "Bodystyle **", "Assembly Plant Location **", "", "", "Service Assembly Plant Location", "Service EOP", "UGN Business **", "Obsolete", "Notes", "Last Update"})


    Private Property hiddenColumnIndexes() As List(Of Integer)
        Get
            Return If(ViewState("hiddenColumnIndexes") Is Nothing, New List(Of Integer)(), DirectCast(ViewState("hiddenColumnIndexes"), List(Of Integer)))
        End Get
        Set(ByVal value As List(Of Integer))
            ViewState("hiddenColumnIndexes") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Programs by Platform"


            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                If Request.QueryString("sPName") IsNot Nothing Then
                    ViewState("sPName") = Server.UrlDecode(Request.QueryString("sPName").ToString)
                End If
                If Request.QueryString("sOEMMF") IsNot Nothing Then
                    ViewState("sOEMMF") = Server.UrlDecode(Request.QueryString("sOEMMF").ToString)
                End If
                If Request.QueryString("sDUB") IsNot Nothing Then
                    ViewState("sDUB") = Server.UrlDecode(Request.QueryString("sDUB").ToString)
                End If
                If Request.QueryString("sWAFPN") IsNot Nothing Then
                    ViewState("sDCP") = Server.UrlDecode(Request.QueryString("sDCP").ToString)
                End If

                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > <a href='PlatformMaintenance.aspx?sPName=" & ViewState("sPName") & "&sOEMMF=" & ViewState("sOEMMF") & "&sDUB=" & ViewState("sDUB") & "&sDCP=" & ViewState("sDCP") & "'><b>Platform</b></a> > Programs by Platform"

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

                ViewState("sPgmCode") = ""
                ViewState("sPgmName") = ""
                ViewState("sMake") = ""

                If Request.QueryString("pPlatID") IsNot Nothing Then
                    ViewState("pPlatID") = Server.UrlDecode(Request.QueryString("pPlatID").ToString)
                End If

                If Not Request.Cookies("PP_PgmCode") Is Nothing Then
                    txtProgramCodeSearch.Text = Server.HtmlEncode(Request.Cookies("PP_PgmCode").Value)
                    ViewState("sPgmCode") = Server.HtmlEncode(Request.Cookies("PP_PgmCode").Value)
                End If

                If Not Request.Cookies("PP_PName") Is Nothing Then
                    txtProgramNameSearch.Text = Server.HtmlEncode(Request.Cookies("PP_PName").Value)
                    ViewState("sPgmName") = Server.HtmlEncode(Request.Cookies("PP_PName").Value)
                End If

                If Not Request.Cookies("PP_Make") Is Nothing Then
                    txtMakeSearch.Text = Server.HtmlEncode(Request.Cookies("PP_Make").Value)
                    ViewState("sMake") = Server.HtmlEncode(Request.Cookies("PP_Make").Value)
                End If

                ''Enable the show hide columns
                hiddenColumnIndexes = New List(Of Integer)()
                BindData()

            Else
                ViewState("sPgmCode") = txtProgramCodeSearch.Text
                ViewState("sPgmName") = txtProgramNameSearch.Text
                ViewState("sMake") = txtMakeSearch.Text
            End If

            CheckRights()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF Page_Load

    Private Sub BindData()
        Try
            lblMessage.Text = ""
            lblMessage.Visible = False

            Dim ds As DataSet = New DataSet
            If ViewState("pPlatID") <> Nothing Then
                'bind data
                ds = commonFunctions.GetPlatform(ViewState("pPlatID"), "", "", "", "", "")
                If (ds.Tables.Item(0).Rows.Count > 0) Then
                    lblPlatformName.Text = ds.Tables(0).Rows(0).Item("PlatformName").ToString()
                    lblOEM.Text = ds.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
                    lblUGNBiz.Text = IIf(ds.Tables(0).Rows(0).Item("UGNBusiness") = True, "Yes", "No")
                    lblCurrentPlatformVal.Text = IIf(ds.Tables(0).Rows(0).Item("CurrentPlatform") = True, "Yes", "No")
                    lblBegYear.Text = ds.Tables(0).Rows(0).Item("BegYear").ToString()
                    lblEndYear.Text = ds.Tables(0).Rows(0).Item("EndYear").ToString()
                    lblSrvYrs.Text = ds.Tables(0).Rows(0).Item("ServiceEOPYY").ToString()
                End If
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
    End Sub 'EOF BindData

    Protected Sub CheckRights()

        Try
            'default to hide edit column
            gvPlatformProgramList.Columns(gvPlatformProgramList.Columns.Count - 1).Visible = False
            If gvPlatformProgramList.FooterRow IsNot Nothing Then
                gvPlatformProgramList.FooterRow.Visible = False
            End If

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

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 122)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("ObjectRole") = True
                                    gvPlatformProgramList.Columns(gvPlatformProgramList.Columns.Count - 1).Visible = True
                                    If gvPlatformProgramList.FooterRow IsNot Nothing Then
                                        gvPlatformProgramList.FooterRow.Visible = True
                                    End If
                                    gvPlatformProgramList.Columns(1).Visible = True
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("ObjectRole") = True
                                    gvPlatformProgramList.Columns(gvPlatformProgramList.Columns.Count - 1).Visible = True
                                    If gvPlatformProgramList.FooterRow IsNot Nothing Then
                                        gvPlatformProgramList.FooterRow.Visible = True
                                    End If
                                    gvPlatformProgramList.Columns(1).Visible = False
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    ViewState("ObjectRole") = False
                                    gvPlatformProgramList.Columns(gvPlatformProgramList.Columns.Count - 1).Visible = False
                                    If gvPlatformProgramList.FooterRow IsNot Nothing Then
                                        gvPlatformProgramList.FooterRow.Visible = False
                                    End If
                                    gvPlatformProgramList.Columns(1).Visible = False
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete

                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                            End Select
                        End If
                    End If
                End If
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

    End Sub 'EOF CheckRights

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Response.Cookies("PP_PgmCode").Value = txtProgramCodeSearch.Text
            Response.Cookies("PP_PName").Value = txtProgramNameSearch.Text
            Response.Cookies("PP_Make").Value = txtMakeSearch.Text

            Response.Redirect("ProgramMaintenance.aspx?pPlatID=" & ViewState("pPlatID") & "&sPgmName=" & ViewState("sPgmName") & "&sPgmCode=" & ViewState("sPgmCode") & "&sMake=" & ViewState("sMake") & "&sPName=" & ViewState("sPName") & "&sCSMPN=" & ViewState("sCSMPN") & "&sWAFPN=" & ViewState("sWAFPN") & "&sOEMMF=" & ViewState("sOEMMF") & "&sDUB=" & ViewState("sDUB") & "&sDCP=" & ViewState("sDCP"), False)

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            commonFunctions.DeletePlatformProgramCookies()

            Response.Redirect("ProgramMaintenance.aspx?pPlatID=" & ViewState("pPlatID") & "&sPName=" & ViewState("sPName") & "&sCSMPN=" & ViewState("sCSMPN") & "&sWAFPN=" & ViewState("sWAFPN") & "&sOEMMF=" & ViewState("sOEMMF") & "&sDUB=" & ViewState("sDUB") & "&sDCP=" & ViewState("sDCP"), False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnReset_Click

#Region "Control Events"

    Protected Sub gvPlatformProgramListShowHideColumns_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPlatformProgramListShowHideColumns.SelectedIndexChanged

        If Me.gvPlatformProgramListShowHideColumns.SelectedIndex > 0 Then
            Dim columnIndex2 As Integer = Integer.Parse(Me.gvPlatformProgramListShowHideColumns.SelectedValue)
            hiddenColumnIndexes.Remove(columnIndex2)

            SetupShowHideColumns()
        End If
    End Sub 'EOF gvPlatformListShowHideColumns_SelectedIndexChanged

#End Region 'Control Events

#Region "GridView Events"

    Protected Sub gvPlatformProgramList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPlatformProgramList.RowDataBound
        '***
        'This section provides the user with the popup for confirming the delete of a record.
        'Called by the onClientClick event.
        '***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(1).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As Platform.Platform_ProgramRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Platform.Platform_ProgramRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record?');")
                End If
            End If

            ' reference the PrevAPL ImageButton
            Dim imgBtn2 As ImageButton = CType(e.Row.FindControl("btnPrevAPL"), ImageButton)
            If imgBtn2 IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(15).Controls(1), ImageButton)
                If db.CommandName = "PrevAPL" Then
                    Dim price As Platform.Platform_ProgramRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Platform.Platform_ProgramRow)

                    Dim strPreviewClientScript As String = "javascript:void(window.open('AssemblyPlantDisplay.aspx?pAPID=" & DataBinder.Eval(e.Row.DataItem, "APID") & "&pMName=" & DataBinder.Eval(e.Row.DataItem, "ProgramName") & "'," & Now.Ticks.ToString & ",'width=800px,height=550px,top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    db.Attributes.Add("onclick", strPreviewClientScript)

                End If
            End If
        End If
    End Sub 'EOF gvPlatformProgramList_RowDataBound

    Protected Sub gvPlatformProgramList_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Try
            Dim strTemp As String

            Dim strKey As String

            For Each strKey In e.NewValues.Keys
                If e.NewValues(strKey) IsNot Nothing Then
                    strTemp = e.NewValues(strKey).ToString

                    If strTemp.Contains(":::") Then
                        e.NewValues(strKey) = CleanBindValue(strTemp)

                    End If
                End If
            Next
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF gvPlatformProgramList_RowUpdating

    Private Function CleanBindValue(ByVal DirtyValue As String) As String

        'CascadingDropDown returns BIND values as value:::text 

        'and needs to be cleaned prior to database update

        Dim strSplit() As String

        strSplit = DirtyValue.Split(":::")

        Return strSplit(0).ToString

    End Function 'EOF CleanBindValue

    Protected Sub gvPlatformProgramList_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            Dim PlatformID As DropDownList
            Dim MP As TextBox 'MNEMONIC_PLATFORM
            Dim MV As TextBox 'MNEMONIC_VEHICLE
            Dim MVP As TextBox 'MNEMONIC_VEHICLE_PLANT
            'Dim MVB As TextBox 'MNEMONIC_VEHICLE_BODYSTYLE
            'Dim MVBP As TextBox 'MNEMONIC_VEHICLE_BODYSTLE_PLANT
            Dim Make As DropDownList
            Dim CSMProgram As TextBox
            Dim BPCSProgramRef As TextBox
            Dim ProgramSuffix As TextBox
            Dim CSMModelName As TextBox
            Dim ModelName As DropDownList
            Dim VTID As DropDownList
            Dim Assembly As DropDownList
            Dim SOPMM As TextBox
            Dim SOPYY As TextBox
            Dim EOPMM As TextBox
            Dim EOPYY As TextBox
            Dim UGNBusiness As DropDownList
            Dim Notes As TextBox

            lblMessage.Text = Nothing
            lblMessage.Visible = False

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then

                PlatformID = CType(gvPlatformProgramList.FooterRow.FindControl("ddPlatformIDGV"), DropDownList)
                odsPlatformProgram.InsertParameters("PlatformID").DefaultValue = PlatformID.SelectedValue

                MP = CType(gvPlatformProgramList.FooterRow.FindControl("txtMPGV"), TextBox)
                odsPlatformProgram.InsertParameters("Mnemonic_Platform").DefaultValue = MP.Text

                MV = CType(gvPlatformProgramList.FooterRow.FindControl("txtMVGV"), TextBox)
                odsPlatformProgram.InsertParameters("Mnemonic_Vehicle").DefaultValue = MV.Text

                MVP = CType(gvPlatformProgramList.FooterRow.FindControl("txtMVPGV"), TextBox)
                odsPlatformProgram.InsertParameters("Mnemonic_Vehicle_Plant").DefaultValue = MVP.Text

                Make = CType(gvPlatformProgramList.FooterRow.FindControl("ddMakeGV"), DropDownList)
                odsPlatformProgram.InsertParameters("Make").DefaultValue = Make.SelectedValue

                CSMProgram = CType(gvPlatformProgramList.FooterRow.FindControl("txtCSMProgramGV"), TextBox)
                odsPlatformProgram.InsertParameters("CSM_Program").DefaultValue = CSMProgram.Text

                CSMModelName = CType(gvPlatformProgramList.FooterRow.FindControl("txtCSMModelNameGV"), TextBox)
                odsPlatformProgram.InsertParameters("CSM_Model_Name").DefaultValue = CSMModelName.Text

                BPCSProgramRef = CType(gvPlatformProgramList.FooterRow.FindControl("txtBPCSProgramRefGV"), TextBox)
                odsPlatformProgram.InsertParameters("BPCSProgramRef").DefaultValue = BPCSProgramRef.Text

                ProgramSuffix = CType(gvPlatformProgramList.FooterRow.FindControl("txtProgramSuffixGV"), TextBox)
                odsPlatformProgram.InsertParameters("ProgramSuffix").DefaultValue = ProgramSuffix.Text

                ModelName = CType(gvPlatformProgramList.FooterRow.FindControl("ddModelGV"), DropDownList)
                odsPlatformProgram.InsertParameters("ProgramName").DefaultValue = ModelName.SelectedValue

                VTID = CType(gvPlatformProgramList.FooterRow.FindControl("ddVehicleTypeGV"), DropDownList)
                odsPlatformProgram.InsertParameters("VTID").DefaultValue = VTID.SelectedValue

                Assembly = CType(gvPlatformProgramList.FooterRow.FindControl("ddAssemblyGV"), DropDownList)
                odsPlatformProgram.InsertParameters("APID").DefaultValue = Assembly.SelectedValue

                SOPMM = CType(gvPlatformProgramList.FooterRow.FindControl("txtSOPMMGV"), TextBox)
                odsPlatformProgram.InsertParameters("SOPMM").DefaultValue = SOPMM.Text

                SOPYY = CType(gvPlatformProgramList.FooterRow.FindControl("txtSOPYYGV"), TextBox)
                odsPlatformProgram.InsertParameters("SOPYY").DefaultValue = SOPYY.Text

                EOPMM = CType(gvPlatformProgramList.FooterRow.FindControl("txtEOPMMGV"), TextBox)
                odsPlatformProgram.InsertParameters("EOPMM").DefaultValue = EOPMM.Text

                EOPYY = CType(gvPlatformProgramList.FooterRow.FindControl("txtEOPYYGV"), TextBox)
                odsPlatformProgram.InsertParameters("EOPYY").DefaultValue = EOPYY.Text

                UGNBusiness = CType(gvPlatformProgramList.FooterRow.FindControl("ddUGNBusinessGV"), DropDownList)
                odsPlatformProgram.InsertParameters("UGNBusiness").DefaultValue = UGNBusiness.SelectedValue

                Notes = CType(gvPlatformProgramList.FooterRow.FindControl("txtNotesGV"), TextBox)
                odsPlatformProgram.InsertParameters("Notes").DefaultValue = Notes.Text


                odsPlatformProgram.Insert()
                '' Indicate that the user needs to be sent to the last page
                SendUserToLastPage = True
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvPlatformProgramList.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvPlatformProgramList.ShowFooter = True
                Else
                    gvPlatformProgramList.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then

                PlatformID = CType(gvPlatformProgramList.FooterRow.FindControl("ddPlatformIDGV"), DropDownList)
                PlatformID.SelectedValue = Nothing

                MP = CType(gvPlatformProgramList.FooterRow.FindControl("txtMPGV"), TextBox)
                MP.Text = Nothing

                MV = CType(gvPlatformProgramList.FooterRow.FindControl("txtMVGV"), TextBox)
                MV.Text = Nothing

                MVP = CType(gvPlatformProgramList.FooterRow.FindControl("txtMVPGV"), TextBox)
                MVP.Text = Nothing

                Make = CType(gvPlatformProgramList.FooterRow.FindControl("ddMakeGV"), DropDownList)
                Make.SelectedValue = Nothing

                CSMProgram = CType(gvPlatformProgramList.FooterRow.FindControl("txtCSMProgramGV"), TextBox)
                CSMProgram.Text = Nothing

                CSMModelName = CType(gvPlatformProgramList.FooterRow.FindControl("txtCSMModelNameGV"), TextBox)
                CSMModelName.Text = Nothing

                BPCSProgramRef = CType(gvPlatformProgramList.FooterRow.FindControl("txtBPCSProgramRefGV"), TextBox)
                BPCSProgramRef.Text = Nothing

                ProgramSuffix = CType(gvPlatformProgramList.FooterRow.FindControl("txtProgramSuffixGV"), TextBox)
                ProgramSuffix.Text = Nothing

                ModelName = CType(gvPlatformProgramList.FooterRow.FindControl("ddModelGV"), DropDownList)
                ModelName.SelectedValue = Nothing

                VTID = CType(gvPlatformProgramList.FooterRow.FindControl("ddVehicleTypeGV"), DropDownList)
                VTID.SelectedValue = Nothing

                Assembly = CType(gvPlatformProgramList.FooterRow.FindControl("ddAssemblyGV"), DropDownList)
                Assembly.SelectedValue = Nothing

                SOPMM = CType(gvPlatformProgramList.FooterRow.FindControl("txtSOPMMGV"), TextBox)
                SOPMM.Text = Nothing

                SOPYY = CType(gvPlatformProgramList.FooterRow.FindControl("txtSOPYYGV"), TextBox)
                SOPYY.Text = Nothing

                EOPMM = CType(gvPlatformProgramList.FooterRow.FindControl("txtEOPMMGV"), TextBox)
                EOPMM.Text = Nothing

                EOPYY = CType(gvPlatformProgramList.FooterRow.FindControl("txtEOPYYGV"), TextBox)
                EOPYY.Text = Nothing

                UGNBusiness = CType(gvPlatformProgramList.FooterRow.FindControl("ddUGNBusinessGV"), DropDownList)
                UGNBusiness.SelectedValue = Nothing

                Notes = CType(gvPlatformProgramList.FooterRow.FindControl("txtNotesGV"), TextBox)
                Notes.Text = Nothing

            End If

            If e.CommandName = "Delete" Then
                Response.Redirect("ProgramMaintenance.aspx?pPlatID=" & ViewState("pPlatID") & "&sPgmName=" & ViewState("sPgmName") & "&sPgmCode=" & ViewState("sPgmCode") & "&sMake=" & ViewState("sMake") & "&sPName=" & ViewState("sPName") & "&sCSMPN=" & ViewState("sCSMPN") & "&sWAFPN=" & ViewState("sWAFPN") & "&sOEMMF=" & ViewState("sOEMMF") & "&sDUB=" & ViewState("sDUB") & "&sDCP=" & ViewState("sDCP"), False)
            End If

            If e.CommandName = "imghideCol" Then
                ' Add the column index to hide to the hiddenColumnIndexes list 
                hiddenColumnIndexes.Add(Integer.Parse(e.CommandArgument.ToString()))
            End If

            SetupShowHideColumns()
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'gvPlatformProgramList_RowCommand

#End Region 'GridView Events

#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_Platform() As Boolean
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

    Protected Sub odsPlatformProgram_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsPlatformProgram.Selected
        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)
        'Dim ds As DataSet = CType(e.ReturnValue, DataSet)
        'Dim dt As DataTable = ds.Tables(0)
        Dim dt As Platform.Platform_ProgramDataTable = CType(e.ReturnValue, Platform.Platform_ProgramDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty_Platform = True
        Else
            LoadDataEmpty_Platform = False
        End If
    End Sub 'EOF odsPlatformProgram_Selected

    Protected Sub gvPlatformProgramList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPlatformProgramList.RowCreated
        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Platform
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If

        ''*******************************************************************
        ' For the header row add a link button to each header 
        ' cell which can execute a row command 
        If e.Row.RowType = DataControlRowType.Header Then
            For columnIndex2 As Integer = 5 To 9
                Dim hideLinkImg As New ImageButton
                hideLinkImg.CommandName = "imghideCol"
                hideLinkImg.CommandArgument = columnIndex2.ToString()
                hideLinkImg.ImageUrl = "~\images\collapseLeft.jpg"
                hideLinkImg.CssClass = "gvHideColLink"
                hideLinkImg.Attributes.Add("title", "Hide Column")

                'Add the "Hide Column" ImageButton to the header cell 
                e.Row.Cells(columnIndex2).Controls.AddAt(0, hideLinkImg)

                ' If there is column header text then 
                ' add it back to the header cell as a label 
                If e.Row.Cells(columnIndex2).Text.Length > 0 Then
                    Dim columnTextLabel As New Label()
                    columnTextLabel.Text = e.Row.Cells(columnIndex2).Text
                    e.Row.Cells(columnIndex2).Controls.Add(columnTextLabel)
                End If
            Next

            For columnIndex2 As Integer = 14 To 15
                Dim hideLinkImg As New ImageButton
                hideLinkImg.CommandName = "imghideCol"
                hideLinkImg.CommandArgument = columnIndex2.ToString()
                hideLinkImg.ImageUrl = "~\images\collapseLeft.jpg"
                hideLinkImg.CssClass = "gvHideColLink"
                hideLinkImg.Attributes.Add("title", "Hide Column")

                'Add the "Hide Column" ImageButton to the header cell 
                e.Row.Cells(columnIndex2).Controls.AddAt(0, hideLinkImg)

                ' If there is column header text then 
                ' add it back to the header cell as a label 
                If e.Row.Cells(columnIndex2).Text.Length > 0 Then
                    Dim columnTextLabel As New Label()
                    columnTextLabel.Text = e.Row.Cells(columnIndex2).Text
                    e.Row.Cells(columnIndex2).Controls.Add(columnTextLabel)
                End If
            Next


            For columnIndex2 As Integer = 18 To 23
                Dim hideLinkImg As New ImageButton
                hideLinkImg.CommandName = "imghideCol"
                hideLinkImg.CommandArgument = columnIndex2.ToString()
                hideLinkImg.ImageUrl = "~\images\collapseLeft.jpg"
                hideLinkImg.CssClass = "gvHideColLink"
                hideLinkImg.Attributes.Add("title", "Hide Column")

                'Add the "Hide Column" ImageButton to the header cell 
                e.Row.Cells(columnIndex2).Controls.AddAt(0, hideLinkImg)

                ' If there is column header text then 
                ' add it back to the header cell as a label 
                If e.Row.Cells(columnIndex2).Text.Length > 0 Then
                    Dim columnTextLabel As New Label()
                    columnTextLabel.Text = e.Row.Cells(columnIndex2).Text
                    e.Row.Cells(columnIndex2).Controls.Add(columnTextLabel)
                End If
            Next

        End If

        ' Hide the column indexes which have been stored in hiddenColumnIndexes 
        For Each columnIndex2 As Integer In hiddenColumnIndexes
            If columnIndex2 < e.Row.Cells.Count Then
                e.Row.Cells(columnIndex2).Visible = False
            End If
        Next

    End Sub 'EOF gvPlatformProgramList_RowCreated
#End Region ' Insert Empty GridView Work-Around

#Region "Private Methods"

    ''' <summary> 
    ''' Setup the drop down list, adding options based on the hiddenColumnIndexes list 
    ''' </summary> 
    Private Sub SetupShowHideColumns()
        Me.gvPlatformProgramListShowHideColumns.Items.Clear()

        If hiddenColumnIndexes.Count > 0 Then
            Me.gvPlatformProgramListShowHideColumns.Visible = True
            Me.gvPlatformProgramListShowHideColumns.Items.Add(New ListItem("-Show Column-", "-1"))

            For Each i As Integer In hiddenColumnIndexes
                Me.gvPlatformProgramListShowHideColumns.Items.Add(New ListItem(columnNames(i), i.ToString()))
            Next
        Else
            Me.gvPlatformProgramListShowHideColumns.Visible = False
        End If

    End Sub 'EOF SetupShowHideColumns
#End Region 'Private Methods

#Region "Exporting to Excel"
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Dim Tdate As String = Replace(Date.Today.ToShortDateString, "/", "-")

        Response.Clear()

        Response.AddHeader("content-disposition", "attachment; filename=ProgramByPlatform_" & Tdate & ".xls")

        Response.Charset = ""

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        'Response.ContentType = "application/vnd.xls"
        Response.ContentType = "application/vnd.ms-excel"

        Dim stringWrite As StringWriter = New StringWriter()

        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)

        gvPlatformProgramList.Columns(0).Visible = False
        gvPlatformProgramList.Columns(1).Visible = False
        gvPlatformProgramList.Columns(2).Visible = False
        gvPlatformProgramList.ShowFooter = False
        gvPlatformProgramList.AllowPaging = False
        gvPlatformProgramList.AllowSorting = False
        gvPlatformProgramList.DataBind()
        gvPlatformProgramList.HeaderStyle.BackColor = Color.White
        gvPlatformProgramList.HeaderStyle.ForeColor = Color.Black
        gvPlatformProgramList.HeaderStyle.Font.Bold = True
        gvPlatformProgramList.HeaderRow.ToString.ToUpper()
        gvPlatformProgramList.AlternatingRowStyle.ForeColor = Color.Black
        gvPlatformProgramList.RowStyle.ForeColor = Color.Black

        gvPlatformProgramList.GridLines = GridLines.Both
        gvPlatformProgramList.Style.Clear()

        gvPlatformProgramList.BottomPagerRow.Visible = False
        gvPlatformProgramList.RenderControl(htmlWrite)

        Response.Write(stringWrite.ToString())

        Response.End()

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        'DO NOT DELETE THIS
        'Confirms that an HtmlForm control is rendered for the
        'specified ASP.NET server control at run time.

    End Sub
#End Region 'Exporting to Excel

End Class
