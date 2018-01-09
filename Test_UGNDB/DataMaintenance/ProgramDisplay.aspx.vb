' ************************************************************************************************
' Name:		ProgramDisplay.aspx
' Purpose:	This Code Behind is used for Program Display. This page will be called from
'           various modules to allow team members to view detailed information about a Program 
'           built from IHS data, AS400 and UGNDB data modeling concept.
'
' Date		    Author	    
' 08/22/2011    LRey			Created .Net application
' ************************************************************************************************

Partial Class DataMaintenance_ProgramDisplay
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.lookupmasterpage_master = Master
            ' ''check test or production environments
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "Program Detail"
                mpTextBox.Font.Size = 18
                mpTextBox.Visible = True
                mpTextBox.Font.Bold = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pPlatID") <> "" Then
                ViewState("pPlatID") = HttpContext.Current.Request.QueryString("pPlatID")
            Else
                ViewState("pPlatID") = "0"
            End If

            If HttpContext.Current.Request.QueryString("pPgmID") <> "" Then
                ViewState("pPgmID") = HttpContext.Current.Request.QueryString("pPgmID")
            Else
                ViewState("pPgmID") = ""
            End If


            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                BindData()
            End If

            Dim strCloseWindow As String = "javascript:window.close();"
            btnClose.Attributes.Add("onclick", strCloseWindow)

            Dim strGoToTop As String = "#top;"
            btnTop.Attributes.Add("onclick", strGoToTop)

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

    Private Sub BindData()
        Try
            lblMessage.Text = ""
            lblMessage.Visible = False
            Dim PGMID As Integer = ViewState("pPgmID")
            Dim PlatformID As Integer = ViewState("pPlatID")

            If PGMID <> Nothing Then
                Dim ds1 As DataSet = New DataSet
                Dim ds2 As DataSet = New DataSet
                'bind data from Platform_Maint
                If PlatformID = Nothing Or PlatformID = 0 Then
                    ds1 = commonFunctions.GetPlatformProgram(PlatformID, PGMID, "", "", "")
                    If (ds1.Tables.Item(0).Rows.Count > 0) Then
                        PlatformID = ds1.Tables(0).Rows(0).Item("PlatformID").ToString()
                    End If
                End If
                ds2 = commonFunctions.GetPlatform(PlatformID, "", "", "", "", "")
                If (ds2.Tables.Item(0).Rows.Count > 0) Then
                    lblPlatformName.Text = ds2.Tables(0).Rows(0).Item("PlatformName").ToString.ToUpper()
                    lblOEM.Text = ds2.Tables(0).Rows(0).Item("OEMManufacturer").ToString.ToUpper()
                    lblUGNBiz.Text = IIf(ds2.Tables(0).Rows(0).Item("UGNBusiness") = True, "Yes", "No")
                    lblCurrentPlatformVal.Text = IIf(ds2.Tables(0).Rows(0).Item("CurrentPlatform") = True, "Yes", "No")
                    lblBegYear.Text = ds2.Tables(0).Rows(0).Item("BegYear").ToString()
                    lblEndYear.Text = ds2.Tables(0).Rows(0).Item("EndYear").ToString()
                    lblSrvYrs.Text = ds2.Tables(0).Rows(0).Item("ServiceEOPYY").ToString()
                    lblPlatformNotesVal.Text = ds2.Tables(0).Rows(0).Item("Notes").ToString()

                End If

                'bind data from Program_Main
                Dim APID As Integer = 0
                Dim ds3 As DataSet = New DataSet
                ds3 = commonFunctions.GetPlatformProgram(PlatformID, PGMID, "", "", "")
                If (ds3.Tables.Item(0).Rows.Count > 0) Then
                    lblMakeVal.Text = ds3.Tables(0).Rows(0).Item("Make").ToString()
                    lblPgmCode.Text = ds3.Tables(0).Rows(0).Item("BPCSProgramRef").ToString.ToUpper()
                    If ds3.Tables(0).Rows(0).Item("ProgramSuffix").ToString() <> Nothing Then
                        lblPgmCode.Text &= "(" & ds3.Tables(0).Rows(0).Item("ProgramSuffix").ToString.ToUpper() & ")"
                    End If
                    lblModelName.Text = ds3.Tables(0).Rows(0).Item("ProgramName").ToString.ToUpper()
                    lblAPL.Text = ds3.Tables(0).Rows(0).Item("Assembly_Plant_Location").ToString.ToUpper()
                    lblStateVal.Text = ds3.Tables(0).Rows(0).Item("AssemblyState").ToString.ToUpper()
                    lblCountryVal.Text = ds3.Tables(0).Rows(0).Item("AssemblyCountry").ToString.ToUpper()
                    lblSOP.Text = ds3.Tables(0).Rows(0).Item("SOP").ToString()
                    lblSOPMM.Text = ds3.Tables(0).Rows(0).Item("SOPMM").ToString()
                    lblSOPYY.Text = ds3.Tables(0).Rows(0).Item("SOPYY").ToString()
                    lblEOP.Text = ds3.Tables(0).Rows(0).Item("EOP").ToString()
                    lblEOPMM.Text = ds3.Tables(0).Rows(0).Item("EOPMM").ToString()
                    lblEOPYY.Text = ds3.Tables(0).Rows(0).Item("EOPYY").ToString()
                    lblSrvEOPMM.Text = ds3.Tables(0).Rows(0).Item("ServiceEOPMM").ToString()
                    lblSrvEOPYY.Text = ds3.Tables(0).Rows(0).Item("ServiceEOPYY").ToString()
                    lblUGNBiz2.Text = IIf(ds3.Tables(0).Rows(0).Item("UGNBusiness") = True, "Yes", "No")
                    lblVehicleTypeVal.Text = ds3.Tables(0).Rows(0).Item("ddVehicleType").ToString()
                    lblRecStatus.Text = ds3.Tables(0).Rows(0).Item("RecStatus").ToString()
                    lblProgramNotesVal.Text = ds3.Tables(0).Rows(0).Item("Notes").ToString()

                    APID = ds3.Tables(0).Rows(0).Item("APID").ToString()
                    txtAPID.Text = APID
                    Session("sAPID") = APID
                    Session("sMName") = ds3.Tables(0).Rows(0).Item("ProgramName").ToString()

                    Dim NoOfDays As String = LastDayOfMonth(ds3.Tables(0).Rows(0).Item("EOPMM").ToString())
                    If ds3.Tables(0).Rows(0).Item("EOPMM").ToString() <> "" Then
                        lblEOP.Text = ds3.Tables(0).Rows(0).Item("EOPMM").ToString() & "/" & NoOfDays & "/" & ds3.Tables(0).Rows(0).Item("EOPYY").ToString()
                    End If
                    If ds3.Tables(0).Rows(0).Item("SOPMM").ToString() <> "" Then
                        lblSOP.Text = ds3.Tables(0).Rows(0).Item("SOPMM").ToString() & "/01/" & ds3.Tables(0).Rows(0).Item("SOPYY").ToString()
                    End If

                    Dim SrvNoOfDays As String = LastDayOfMonth(ds3.Tables(0).Rows(0).Item("ServiceEOPMM").ToString())
                    If ds3.Tables(0).Rows(0).Item("ServiceEOPMM").ToString() <> "" Then
                        lblSrvEOP.Text = ds3.Tables(0).Rows(0).Item("ServiceEOPMM").ToString() & "/" & SrvNoOfDays & "/" & ds3.Tables(0).Rows(0).Item("ServiceEOPYY").ToString()
                    End If


                End If

                '' ''bind data
                If APID <> 0 Then
                    Dim ds As DataSet = New DataSet
                    ds = commonFunctions.GetAssemblyPlantLocation(APID, "", "", "", "")
                    If commonFunctions.CheckDataSet(ds) = True Then
                        If (ds.Tables.Item(0).Rows.Count > 0) Then
                            ViewState("sAPID") = APID
                        End If
                    End If
                End If 'EOF APID
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
    Public Function LastDayOfMonth(ByVal NoOfDays As String) As String
        Try
            Select Case NoOfDays
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
            Return NoOfDays

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
        Return True
    End Function 'EOF BuildApprovalList

#Region "GridView Events APL"
    Protected Sub gvAPLOEM_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim OEMModelType As String
            Dim drOEMModelType As AssemblyPlantLocation.Assembly_Plant_OEMRow = CType(CType(e.Row.DataItem, DataRowView).Row, AssemblyPlantLocation.Assembly_Plant_OEMRow)

            If DataBinder.Eval(e.Row.DataItem, "OEMModelType") IsNot DBNull.Value Then
                OEMModelType = drOEMModelType.OEMModelType
                ' Reference the rpCBRC ObjectDataSource
                Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsAPLPartOEM"), ObjectDataSource)

                ' Set the CategoryID Parameter value
                rpCBRC.SelectParameters("APID").DefaultValue = drOEMModelType.APID.ToString()
                rpCBRC.SelectParameters("ModelName").DefaultValue = drOEMModelType.ModelName.ToString()
                rpCBRC.SelectParameters("OEMModelType").DefaultValue = drOEMModelType.OEMModelType.ToString()
                rpCBRC.SelectParameters("PARTNO").DefaultValue = Nothing
                rpCBRC.SelectParameters("CPART").DefaultValue = Nothing
                rpCBRC.SelectParameters("COMPNY").DefaultValue = Nothing
                rpCBRC.SelectParameters("PRCCDE").DefaultValue = Nothing
            End If
        End If
    End Sub 'EOF gvAPLOEM_RowDataBound


#End Region 'GridView Events

#Region "GridView Events Program Volume"

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

End Class
