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
' 04/2008       RCarlson			Created .Net application
' 07/22/2008    RCarlson            Cleaned Up Error Trapping
' 10/03/2008    RCarlson            Added Security Role Select Statement
' 05/06/2009    LREY                Added BPCSProgramRef, ProgramSuffix and Make

Partial Class DataMaintenance_ProgramMaintenance
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Request.QueryString("pPlatID") IsNot Nothing Then
                ViewState("pPlatID") = Server.UrlDecode(Request.QueryString("pPlatID").ToString)
            End If

            If Request.QueryString("pPgmID") IsNot Nothing Then
                ViewState("pPgmID") = Server.UrlDecode(Request.QueryString("pPgmID").ToString)
            End If


            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Volumes by Program"

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


                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > <a href='PlatformMaintenance.aspx?sPName=" & ViewState("sPName") & "&sOEMMF=" & ViewState("sOEMMF") & "&sDUB=" & ViewState("sDUB") & "&sDCP=" & ViewState("sDCP") & "'><b>Platform</b></a> > <a href='ProgramMaintenance.aspx?pPlatID=" & ViewState("pPlatID") & "&pPgmID=" & ViewState("pPgmID") & "&sPName=" & ViewState("sPName") & "&sCSMPN=" & ViewState("sCSMPN") & "&sWAFPN=" & ViewState("sWAFPN") & "&sOEMMF=" & ViewState("sOEMMF") & "&sDUB=" & ViewState("sDUB") & "&sDCP=" & ViewState("sDCP") & "'><b>Programs by Platform</b></a> > Volumes by Program"

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
                BindData()
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
                End If
            End If

            If ViewState("pPgmID") <> Nothing Then
                'bind data
                ds = commonFunctions.GetPlatformProgram(ViewState("pPlatID"), ViewState("pPgmID"), "", "", "")
                If (ds.Tables.Item(0).Rows.Count > 0) Then
                    lblMakeVal.Text = ds.Tables(0).Rows(0).Item("Make").ToString()
                    lblPgmCode.Text = ds.Tables(0).Rows(0).Item("BPCSProgramRef").ToString()
                    If ds.Tables(0).Rows(0).Item("ProgramSuffix").ToString() <> Nothing Then
                        lblPgmCode.Text = lblPgmCode.Text & "(" & ds.Tables(0).Rows(0).Item("ProgramSuffix").ToString() & ")"
                    End If
                    lblModelName.Text = ds.Tables(0).Rows(0).Item("ProgramName").ToString()
                    lblAPL.Text = ds.Tables(0).Rows(0).Item("Assembly_Plant_Location").ToString()
                    lblStateVal.Text = ds.Tables(0).Rows(0).Item("AssemblyState").ToString()
                    lblCountryVal.Text = ds.Tables(0).Rows(0).Item("AssemblyCountry").ToString()
                    lblSOPVal.Text = ds.Tables(0).Rows(0).Item("SOP").ToString()
                    lblSOPMM.Text = ds.Tables(0).Rows(0).Item("SOPMM").ToString()
                    lblSOPYY.Text = ds.Tables(0).Rows(0).Item("SOPYY").ToString()
                    lblEOPVal.Text = ds.Tables(0).Rows(0).Item("EOP").ToString()
                    lblEOPMM.Text = ds.Tables(0).Rows(0).Item("EOPMM").ToString()
                    lblEOPYY.Text = ds.Tables(0).Rows(0).Item("EOPYY").ToString()
                    lblUGNBiz2.Text = IIf(ds.Tables(0).Rows(0).Item("UGNBusiness") = True, "Yes", "No")
                    lblVehicleTypeVal.Text = ds.Tables(0).Rows(0).Item("ddVehicleType").ToString()
                    lblRecStatus.Text = ds.Tables(0).Rows(0).Item("RecStatus").ToString()

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
            gvProgramVolumeList.Columns(0).Visible = False
            gvProgramVolumeList.Columns(1).Visible = False
            'If gvProgramVolumeList.FooterRow IsNot Nothing Then
            gvProgramVolumeList.FooterRow.Visible = False
            ' End If

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
                                    'gvProgramVolumeList.Columns(1).Visible = True
                                    'If gvProgramVolumeList.FooterRow IsNot Nothing Then
                                    '    gvProgramVolumeList.FooterRow.Visible = True
                                    'End If
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("ObjectRole") = True
                                    'gvProgramVolumeList.Columns(1).Visible = True
                                    'If gvProgramVolumeList.FooterRow IsNot Nothing Then
                                    '    gvProgramVolumeList.FooterRow.Visible = True
                                    'End If
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("ObjectRole") = True
                                    'gvProgramVolumeList.Columns(1).Visible = False
                                    'If gvProgramVolumeList.FooterRow IsNot Nothing Then
                                    '    gvProgramVolumeList.FooterRow.Visible = True
                                    'End If
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    'gvProgramVolumeList.Columns(0).Visible = False
                                    'If gvProgramVolumeList.FooterRow IsNot Nothing Then
                                    '    gvProgramVolumeList.FooterRow.Visible = False
                                    'End If
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    'If gvProgramVolumeList.FooterRow IsNot Nothing Then
                                    '    gvProgramVolumeList.FooterRow.Visible = False
                                    'End If
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

#Region "GridView Events"

    Protected Sub gvProgramVolumeList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvProgramVolumeList.RowDataBound
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
                    Dim price As Platform.Program_VolumeRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Platform.Program_VolumeRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete volume " & " for """ & DataBinder.Eval(e.Row.DataItem, "YearID") & """?');")
                End If
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim YearID As Platform.Program_VolumeRow = _
                    CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Platform.Program_VolumeRow)
                If YearID.YearID = Year(Date.Today) Then
                    e.Row.BackColor = Drawing.Color.Yellow
                End If
            End If

        End If
    End Sub 'EOF gvProgramVolumeList_RowDataBound

    Protected Sub gvProgramVolumeList_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try
            Dim YearID As TextBox
            Dim JanVol As TextBox
            Dim FebVol As TextBox
            Dim MarVol As TextBox
            Dim AprVol As TextBox
            Dim MayVol As TextBox
            Dim JunVol As TextBox
            Dim JulVol As TextBox
            Dim AugVol As TextBox
            Dim SepVol As TextBox
            Dim OctVol As TextBox
            Dim NovVol As TextBox
            Dim DecVol As TextBox

            lblMessage.Text = Nothing
            lblMessage.Visible = False
            lblRaiseError.Text = Nothing
            lblRaiseError.Visible = False

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then

                odsProgramVolume.InsertParameters("ProgramID").DefaultValue = ViewState("pPgmID")

                YearID = CType(gvProgramVolumeList.FooterRow.FindControl("txtYearID"), TextBox)
                odsProgramVolume.InsertParameters("YearID").DefaultValue = YearID.Text

                If ((YearID.Text = lblSOPYY.Text) And (1 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (1 > lblEOPMM.Text)) Then
                    JanVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtJanVol"), TextBox)
                    odsProgramVolume.InsertParameters("JanVolume").DefaultValue = 0
                Else
                    JanVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtJanVol"), TextBox)
                    odsProgramVolume.InsertParameters("JanVolume").DefaultValue = JanVol.Text
                End If

                If ((YearID.Text = lblSOPYY.Text) And (2 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (2 > lblEOPMM.Text)) Then
                    FebVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtFebVol"), TextBox)
                    odsProgramVolume.InsertParameters("FebVolume").DefaultValue = 0
                Else
                    FebVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtFebVol"), TextBox)
                    odsProgramVolume.InsertParameters("FebVolume").DefaultValue = FebVol.Text
                End If

                If ((YearID.Text = lblSOPYY.Text) And (3 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (3 > lblEOPMM.Text)) Then
                    MarVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtMarVol"), TextBox)
                    odsProgramVolume.InsertParameters("MarVolume").DefaultValue = 0
                Else
                    MarVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtMarVol"), TextBox)
                    odsProgramVolume.InsertParameters("MarVolume").DefaultValue = MarVol.Text
                End If

                If ((YearID.Text = lblSOPYY.Text) And (4 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (4 > lblEOPMM.Text)) Then
                    AprVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtAprVol"), TextBox)
                    odsProgramVolume.InsertParameters("AprVolume").DefaultValue = 0
                Else
                    AprVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtAprVol"), TextBox)
                    odsProgramVolume.InsertParameters("AprVolume").DefaultValue = AprVol.Text
                End If

                If ((YearID.Text = lblSOPYY.Text) And (5 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (5 > lblEOPMM.Text)) Then
                    MayVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtMayVol"), TextBox)
                    odsProgramVolume.InsertParameters("MayVolume").DefaultValue = 0
                Else
                    MayVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtMayVol"), TextBox)
                    odsProgramVolume.InsertParameters("MayVolume").DefaultValue = MayVol.Text
                End If

                If ((YearID.Text = lblSOPYY.Text) And (6 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (6 > lblEOPMM.Text)) Then
                    JunVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtJunVol"), TextBox)
                    odsProgramVolume.InsertParameters("JunVolume").DefaultValue = 0
                Else
                    JunVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtJunVol"), TextBox)
                    odsProgramVolume.InsertParameters("JunVolume").DefaultValue = JunVol.Text
                End If

                If ((YearID.Text = lblSOPYY.Text) And (7 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (7 > lblEOPMM.Text)) Then
                    JulVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtJulVol"), TextBox)
                    odsProgramVolume.InsertParameters("JulVolume").DefaultValue = 0
                Else
                    JulVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtJulVol"), TextBox)
                    odsProgramVolume.InsertParameters("JulVolume").DefaultValue = JulVol.Text
                End If

                If ((YearID.Text = lblSOPYY.Text) And (8 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (8 > lblEOPMM.Text)) Then
                    AugVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtAugVol"), TextBox)
                    odsProgramVolume.InsertParameters("AugVolume").DefaultValue = 0
                Else
                    AugVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtAugVol"), TextBox)
                    odsProgramVolume.InsertParameters("AugVolume").DefaultValue = AugVol.Text
                End If


                If ((YearID.Text = lblSOPYY.Text) And (9 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (9 > lblEOPMM.Text)) Then
                    SepVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtSepVol"), TextBox)
                    odsProgramVolume.InsertParameters("SepVolume").DefaultValue = 0
                Else
                    SepVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtSepVol"), TextBox)
                    odsProgramVolume.InsertParameters("SepVolume").DefaultValue = SepVol.Text
                End If

                If ((YearID.Text = lblSOPYY.Text) And (10 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (10 > lblEOPMM.Text)) Then
                    OctVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtOctVol"), TextBox)
                    odsProgramVolume.InsertParameters("OctVolume").DefaultValue = 0
                Else
                    OctVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtOctVol"), TextBox)
                    odsProgramVolume.InsertParameters("OctVolume").DefaultValue = OctVol.Text
                End If

                If ((YearID.Text = lblSOPYY.Text) And (11 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (11 > lblEOPMM.Text)) Then
                    NovVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtNovVol"), TextBox)
                    odsProgramVolume.InsertParameters("NovVolume").DefaultValue = 0
                Else
                    NovVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtNovVol"), TextBox)
                    odsProgramVolume.InsertParameters("NovVolume").DefaultValue = NovVol.Text
                End If

                If ((YearID.Text = lblSOPYY.Text) And (12 < lblSOPMM.Text)) Or ((YearID.Text = lblEOPYY.Text) And (12 > lblEOPMM.Text)) Then
                    DecVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtDecVol"), TextBox)
                    odsProgramVolume.InsertParameters("DecVolume").DefaultValue = 0
                Else
                    DecVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtDecVol"), TextBox)
                    odsProgramVolume.InsertParameters("DecVolume").DefaultValue = DecVol.Text
                End If

                odsProgramVolume.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvProgramVolumeList.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvProgramVolumeList.ShowFooter = True
                Else
                    gvProgramVolumeList.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                YearID = CType(gvProgramVolumeList.FooterRow.FindControl("txtYearID"), TextBox)
                YearID.Text = Nothing

                JanVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtJanVol"), TextBox)
                JanVol.Text = Nothing

                FebVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtFebVol"), TextBox)
                FebVol.Text = Nothing

                MarVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtMarVol"), TextBox)
                MarVol.Text = Nothing

                AprVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtAprVol"), TextBox)
                AprVol.Text = Nothing

                MayVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtMayVol"), TextBox)
                MayVol.Text = Nothing

                JunVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtJunVol"), TextBox)
                JunVol.Text = Nothing

                JulVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtJulVol"), TextBox)
                JulVol.Text = Nothing

                AugVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtAugVol"), TextBox)
                AugVol.Text = Nothing

                SepVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtSepVol"), TextBox)
                SepVol.Text = Nothing

                OctVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtOctVol"), TextBox)
                OctVol.Text = Nothing

                NovVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtNovVol"), TextBox)
                NovVol.Text = Nothing

                DecVol = CType(gvProgramVolumeList.FooterRow.FindControl("txtDecVol"), TextBox)
                DecVol.Text = Nothing
            End If

            If e.CommandName = "Delete" Then
                Response.Redirect("ProgramVolume.aspx?pPlatID=" & ViewState("pPlatID") & "&pPgmID=" & ViewState("pPgmID") & "&sPgmName=" & ViewState("sPgmName") & "&sPgmCode=" & ViewState("sPgmCode") & "&sMake=" & ViewState("sMake") & "&sPName=" & ViewState("sPName") & "&sCSMPN=" & ViewState("sCSMPN") & "&sWAFPN=" & ViewState("sWAFPN") & "&sOEMMF=" & ViewState("sOEMMF") & "&sDUB=" & ViewState("sDUB") & "&sDCP=" & ViewState("sDCP"), False)
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

    End Sub 'gvProgramVolumeList_RowCommand

    Protected Sub gvProgramVolumeList_RowDeleted(ByVal sender As Object, ByVal e As GridViewDeletedEventArgs)
        lblRaiseError.Text = Nothing
        lblRaiseError.Visible = False

        If e.Exception Is Nothing Then
            If e.AffectedRows > 0 Then
                lblRaiseError.Text = "Row deleted successfully."
                lblRaiseError.Visible = True
            Else
                lblRaiseError.Text = "Row deleted successfully."
                lblRaiseError.Visible = True
            End If
        Else
            lblRaiseError.Text = "An error occured while attempting to delete a row."
            lblRaiseError.Visible = True
        End If

    End Sub 'EOF gvProgramVolumeList_RowDeleted

    Protected Function EnableFields(ByVal YearID As Integer, ByVal MonthID As Integer) As Boolean

        Dim strReturnValue As Boolean = True
        ''Disable fields less than start of month when YearID equals SOP Year
        If YearID = lblSOPYY.Text Then
            If MonthID < lblSOPMM.Text Then
                strReturnValue = False
            End If
        ElseIf YearID = lblEOPYY.Text Then
            ''Disable fields greater than end of month when YearID equals EOP Year
            If MonthID > lblEOPMM.Text Then
                strReturnValue = False
            End If
        End If

        EnableFields = strReturnValue
    End Function

#End Region 'GridView Events

#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_Volume() As Boolean
        Get
            If ViewState("LoadDataEmpty_Volume") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Volume"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Volume") = value
        End Set
    End Property 'EOF LoadDataEmpty_Volume

    Protected Sub odsProgramVolume_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsProgramVolume.Selected
        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)
        'Dim ds As DataSet = CType(e.ReturnValue, DataSet)
        'Dim dt As DataTable = ds.Tables(0)
        Dim dt As Platform.Program_VolumeDataTable = CType(e.ReturnValue, Platform.Program_VolumeDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty_Volume = True
        Else
            LoadDataEmpty_Volume = False
        End If
    End Sub 'EOF odsProgramVolume_Selected

    Protected Sub gvProgramVolumeList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvProgramVolumeList.RowCreated

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Volume
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If

    End Sub 'EOF gvProgramVolumeList_RowCreated

#End Region ' Insert Empty GridView Work-Around


End Class
