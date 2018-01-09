' ************************************************************************************************
' Name:	ChartSpeck.aspx.vb
' Purpose:	This program is used to view, insert, update Make
'
' Date		    Author	    
' 09/28/2011    LREY			Created .Net application
' ************************************************************************************************
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
Partial Class MfgProd_ChartSpec
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Production"
            m.ContentLabel = "Part Specification"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Manufacturing</b> > <a href='ChartSpecList.aspx'><b>Part Specification Search</b></a>  > Part Specification"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("MPRExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False


            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to define the primary record
            If HttpContext.Current.Request.QueryString("pCSID") <> "" Then
                ViewState("pCSID") = HttpContext.Current.Request.QueryString("pCSID")
            Else
                ViewState("pCSID") = ""
            End If

            ''Used to display a list of Part Numbers related to the formula selected
            If HttpContext.Current.Request.QueryString("pFormula") <> "" Then
                ViewState("pFormula") = HttpContext.Current.Request.QueryString("pFormula")
            Else
                ViewState("pFormula") = ""
            End If

            ''Used to Show/Hide Future Part Info text boxes
            ViewState("pFPNo") = False

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                BindCriteria()
                If ViewState("pCSID") <> "" Then
                    BindData()
                End If
            End If

            CheckRights()
            txtNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNotes.ClientID + ");")
            txtNotes.Attributes.Add("maxLength", "200")

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
    Protected Sub ShowHideControls(ByVal FormulaID As Integer, ByVal FormulaName As String)

        ''*************************************
        ''*Get Formula Related Fields
        ''*************************************
        Dim dsFrm As DataSet = New DataSet
        Dim i As Integer = 0
        dsFrm = MPRModule.GetChartSpecFrmTmplt(0, FormulaID, FormulaName, 0)
        If commonFunctions.CheckDataSet(dsFrm) = True Then
            For i = 0 To dsFrm.Tables(0).Rows.Count - 1

            Next

        End If
    End Sub 'EOF ShowHideControls

    Protected Sub CheckRights()
        Try

            btnSave.Enabled = False
            btnReset2.Enabled = False

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

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 129) 'Part Specification Form ID

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("ObjectRole") = True
                                    btnSave.Enabled = True
                                    btnReset2.Enabled = True
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("ObjectRole") = True
                                    btnSave.Enabled = True
                                    btnReset2.Enabled = True
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("ObjectRole") = True
                                    btnSave.Enabled = False
                                    btnReset2.Enabled = False
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    btnSave.Enabled = False
                                    btnReset2.Enabled = False
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    btnSave.Enabled = False
                                    btnReset2.Enabled = False
                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                            End Select
                        End If
                    End If
                End If
            End If

            EnableFields(IIf(txtFormulaID.Text = Nothing, 0, txtFormulaID.Text), ddFormula.SelectedValue)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF CheckRights
    Protected Sub EnableFields(ByVal FormulaID As Integer, ByVal FormulaName As String)
        Dim dsFM As DataSet = New DataSet
        Dim FldObjName As String = Nothing
        Dim FldType As String = Nothing
        Dim i As Integer = 0

        ViewState("KitPartNo") = False
        lblKitPartNo.Visible = False
        txtKitPartNo.Visible = False

        ViewState("FamilyPartNo") = False
        lblFamilyPartNo.Visible = False
        txtFamilyPartNo.Visible = False

        ViewState("BlankPartNo") = False
        lblBlankPartNo.Visible = False
        txtBlankPartNo.Visible = False

        ViewState("BlankSize") = False
        lblBlankSize.Visible = False
        txtBlankSize.Visible = False

        ViewState("SpGravFrom") = False
        lblSpGravFrom.Visible = False
        txtSpGravFrom.Visible = False
        ddSpGravFromUOM.Visible = False

        ViewState("SpGravTo") = False
        lblSpGravTo.Visible = False
        txtSpGravTo.Visible = False
        ddSpGravToUOM.Visible = False

        ViewState("SpecFrequency") = False
        lblSpecFrequency.Visible = False
        txtSpecFrequency.Visible = False

        ViewState("ThicknessFrom") = False
        lblThicknessFrom.Visible = False
        txtThicknessFrom.Visible = False
        ddThicknessFromUOM.Visible = False

        ViewState("ThicknessTo") = False
        lblThicknessTo.Visible = False
        txtThicknessTo.Visible = False
        ddThicknessToUOM.Visible = False

        ViewState("ThicknessFrequency") = False
        lblThicknessFrequency.Visible = False
        txtThicknessFrequency.Visible = False

        ViewState("TargetThickness") = False
        lblTargetThickness.Visible = False
        txtTargetThickness.Visible = False
        ddTargetThicknessUOM.Visible = False

        ViewState("Width") = False
        lblWidth.Visible = False
        txtWidth.Visible = False
        ddWidthUOM.Visible = False

        ViewState("PTAreaFrom") = False
        lblPTAreaFrom.Visible = False
        txtPTAreaFrom.Visible = False
        ddPTAreaFromUOM.Visible = False

        ViewState("PTAreaTo") = False
        lblPTAreaTo.Visible = False
        txtPTAreaTo.Visible = False
        ddPTAreaToUOM.Visible = False

        ViewState("SPQ") = False
        lblSPQRqrd.Visible = False
        lblSPQ.Visible = False
        txtSPQ.Visible = False
        rfvSPQ.Enabled = False

        ViewState("PcsPerHour") = False
        lblPcsPerHourRqrd.Visible = False
        lblPcsPerHour.Visible = False
        txtPcsPerHour.Visible = False
        rfvPcsPerHour.Enabled = False

        ViewState("PcsPerCycle") = False
        lblPcsPerCycleRqrd.Visible = False
        lblPcsPerCycle.Visible = False
        txtPcsPerCycle.Visible = False
        rfvPcsPerCycle.Enabled = False

        ViewState("SagSpecFrom") = False
        lblSagSpecFrom.Visible = False
        txtSagSpecFrom.Visible = False
        ddSagSpecFromUOM.Visible = False

        ViewState("SagSpecTo") = False
        lblSagSpecTo.Visible = False
        txtSagSpecTo.Visible = False
        ddSagSpecToUOM.Visible = False

        ViewState("SagPanelSize") = False
        lblSagPanelSize.Visible = False
        txtSagPanelSize.Visible = False
        ddSagPanelUOM.Visible = False

        ViewState("Travel") = False
        lblTravel.Visible = False
        txtTravel.Visible = False

        ViewState("CallUpNo") = False
        lblCallUpNo.Visible = False
        txtCallUpNo.Visible = False

        ViewState("LineSpeed") = False
        lblLineSpeed.Visible = False
        txtLineSpeed.Visible = False
        ddLineSpeed.Visible = False

        ViewState("PressCycles") = False
        lblPressCycles.Visible = False
        txtPressCycles.Visible = False

        ViewState("StandardTime") = False
        lblStandardTime.Visible = False
        txtStandardTime.Visible = False

        ViewState("Quantity") = False
        lblQuantity.Visible = False
        txtQuantity.Visible = False

        ViewState("OvenCondTemp") = False
        lblOvenCondTemp.Visible = False
        txtOvenCondTemp.Visible = False
        ddOvenCondTempUOM.Visible = False

        ViewState("OvenCondTime") = False
        lblOvenCondTime.Visible = False
        txtOvenCondTime.Visible = False
        ddOvenCondTimeUOM.Visible = False

        ViewState("BondTemp") = False
        lblBondTemp.Visible = False
        txtBondTemp.Visible = False
        ddBondTempUOM.Visible = False

        ViewState("BondTime") = False
        lblBondTime.Visible = False
        txtBondTime.Visible = False
        ddBondTimeUOM.Visible = False

        ViewState("BondPLFrom") = False
        lblBondPLFrom.Visible = False
        txtBondPLFrom.Visible = False
        ddBondPLFromUOM.Visible = False

        ViewState("BondPLTo") = False
        lblBondPLTo.Visible = False
        txtBondPLTo.Visible = False
        ddBondPLToUOM.Visible = False

        ViewState("ExpTemp") = False
        lblExpTemp.Visible = False
        txtExpTemp.Visible = False
        ddExpTempUOM.Visible = False

        ViewState("ExpTime") = False
        lblExpTime.Visible = False
        txtExpTime.Visible = False
        ddExpTimeUOM.Visible = False

        ViewState("ExpSpecFrom") = False
        lblExpSpecFrom.Visible = False
        txtExpSpecFrom.Visible = False
        ddExpSpecFromUOM.Visible = False

        ViewState("ExpSpecTo") = False
        lblExpSpecTo.Visible = False
        txtExpSpecTo.Visible = False
        ddExpSpecToUOM.Visible = False

        ViewState("Configuration") = False
        lblConfiguration.Visible = False
        txtConfiguration.Visible = False

        ViewState("WeightFrom") = False
        lblWeightFrom.Visible = False
        txtWeightFrom.Visible = False
        ddWeightFromUOM.Visible = False

        ViewState("WeightTo") = False
        lblWeightTo.Visible = False
        txtWeightTo.Visible = False
        ddWeightToUOM.Visible = False

        ViewState("WeightFrequency") = False
        lblWeightFrequency.Visible = False
        txtWeightFrequency.Visible = False

        ViewState("Moldability") = False
        lblMoldability.Visible = False
        txtMoldability.Visible = False

        ViewState("MoldOvenCondTemp") = False
        lblMoldOvenCondTemp.Visible = False
        txtMoldOvenCondTemp.Visible = False
        ddMoldOvenCondTempUOM.Visible = False

        ViewState("MoldOvenCondTime") = False
        lblMoldOvenCondTime.Visible = False
        txtMoldOvenCondTime.Visible = False
        ddMoldOvenCondTimeUOM.Visible = False

        ViewState("MoldOvenFrequency") = False
        lblMoldOvenFrequency.Visible = False
        txtMoldOvenFrequency.Visible = False

        ViewState("Coating") = False
        lblCoating.Visible = False
        txtCoating.Visible = False

        ViewState("Shrinkage") = False
        lblShrinkage.Visible = False
        txtShrinkage.Visible = False
        ddShrinkageUOM.Visible = False

        ViewState("ShrinkOvenCondTemp") = False
        lblShrinkOvenCondTemp.Visible = False
        txtShrinkOvenCondTemp.Visible = False
        ddShrinkOvenCondTempUOM.Visible = False

        ViewState("ShrinkOvenCondTime") = False
        lblShrinkOvenCondTime.Visible = False
        txtShrinkOvenCondTime.Visible = False
        ddShrinkOvenCondTimeUOM.Visible = False

        ViewState("ShrinkOvenFrequency") = False
        lblShrinkOvenFrequency.Visible = False
        txtShrinkOvenFrequency.Visible = False

        ViewState("BallTestFrom") = False
        lblBallTestFrom.Visible = False
        txtBallTestFrom.Visible = False
        ddBallTestFromUOM.Visible = False

        ViewState("BallTestTo") = False
        lblBallTestTo.Visible = False
        txtBallTestTo.Visible = False
        ddBallTestToUOM.Visible = False

        ViewState("BallFrequency") = False
        lblBallFrequency.Visible = False
        txtBallFrequency.Visible = False

        ViewState("ReleasePoly") = False
        lblReleasePoly.Visible = False
        txtReleasePoly.Visible = False

        ViewState("GluePumpCapacity") = False
        lblGluePumpCapacity.Visible = False
        txtGluePumpCapacity.Visible = False
        ddGluePumpCapacityUOM.Visible = False

        ViewState("NominalWeight") = False
        lblNominalWeight.Visible = False
        txtNominalWeight.Visible = False
        ddNominalWeightUOM.Visible = False

        ViewState("HangTest") = False
        lblHangTest.Visible = False
        txtHangTest.Visible = False
        ddHangTestUOM.Visible = False

        ViewState("HardnessFrom") = False
        lblHardnessFrom.Visible = False
        txtHardnessFrom.Visible = False
        ddHardnessFromUOM.Visible = False

        ViewState("HardnessTo") = False
        lblHardnessTo.Visible = False
        txtHardnessTo.Visible = False
        ddHardnessToUOM.Visible = False

        ViewState("Elongation") = False
        lblElongation.Visible = False
        ddElongationUOM.Visible = False
        txtElongation.Visible = False

        dsFM = MPRModule.GetChartSpecFrmTmplt(0, FormulaID, FormulaName, False)
        If commonFunctions.CheckDataSet(dsFM) = True Then
            For i = 0 To dsFM.Tables.Item(0).Rows.Count - 1
                FldObjName = dsFM.Tables(0).Rows(i).Item("FldObjName").ToString()
                FldType = dsFM.Tables(0).Rows(i).Item("FldType").ToString()

                If FldObjName = "txtKitPartNo" Then
                    ViewState("KitPartNo") = True
                    lblKitPartNo.Visible = True
                    txtKitPartNo.Visible = True
                End If

                If FldObjName = "txtFamilyPartNo" Then
                    ViewState("FamilyPartNo") = True
                    lblFamilyPartNo.Visible = True
                    txtFamilyPartNo.Visible = True
                End If

                If FldObjName = "txtBlankPartNo" Then
                    ViewState("BlankPartNo") = True
                    lblBlankPartNo.Visible = True
                    txtBlankPartNo.Visible = True
                End If

                If FldObjName = "txtBlankSize" Then
                    ViewState("BlankSize") = True
                    lblBlankSize.Visible = True
                    txtBlankSize.Visible = True
                End If

                If FldObjName = "txtSpGravFrom" Then
                    ViewState("SpGravFrom") = True
                    lblSpGravFrom.Visible = True
                    txtSpGravFrom.Visible = True
                    ddSpGravFromUOM.Visible = True
                End If

                If FldObjName = "txtSpGravTo" Then
                    ViewState("SpGravTo") = True
                    lblSpGravTo.Visible = True
                    txtSpGravTo.Visible = True
                    ddSpGravToUOM.Visible = True
                End If

                If FldObjName = "txtSpecFrequency" Then
                    ViewState("SpecFrequency") = True
                    lblSpecFrequency.Visible = True
                    txtSpecFrequency.Visible = True
                End If

                If FldObjName = "txtThicknessFrom" Then
                    ViewState("ThicknessFrom") = True
                    lblThicknessFrom.Visible = True
                    txtThicknessFrom.Visible = True
                    ddThicknessFromUOM.Visible = True
                End If

                If FldObjName = "txtThicknessTo" Then
                    ViewState("ThicknessTo") = True
                    lblThicknessTo.Visible = True
                    txtThicknessTo.Visible = True
                    ddThicknessToUOM.Visible = True
                End If

                If FldObjName = "txtThicknessFrequency" Then
                    ViewState("ThicknessFrequency") = True
                    lblThicknessFrequency.Visible = True
                    txtThicknessFrequency.Visible = True
                End If

                If FldObjName = "txtTargetThickness" Then
                    ViewState("TargetThickness") = True
                    lblTargetThickness.Visible = True
                    txtTargetThickness.Visible = True
                    ddTargetThicknessUOM.Visible = True
                End If

                If FldObjName = "txtWidth" Then
                    ViewState("Width") = True
                    lblWidth.Visible = True
                    txtWidth.Visible = True
                    ddWidthUOM.Visible = True
                End If

                If FldObjName = "txtContainerDescription" Then
                    ViewState("ContainerDescription") = True
                    lblContainerDescription.Visible = True
                    txtContainerDescription.Visible = True
                End If

                If FldObjName = "txtContainerDimensions" Then
                    ViewState("ContainerDimensions") = True
                    lblContainerDimensions.Visible = True
                    txtContainerDimensions.Visible = True
                End If

                If FldObjName = "txtPTAreaFrom" Then
                    ViewState("PTAreaFrom") = True
                    lblPTAreaFrom.Visible = True
                    txtPTAreaFrom.Visible = True
                    ddPTAreaFromUOM.Visible = True
                End If

                If FldObjName = "txtPTAreaTo" Then
                    ViewState("PTAreaTo") = True
                    lblPTAreaTo.Visible = True
                    txtPTAreaTo.Visible = True
                    ddPTAreaToUOM.Visible = True
                End If

                If FldObjName = "txtSPQ" Then
                    ViewState("SPQ") = True
                    lblSPQRqrd.Visible = True
                    lblSPQ.Visible = True
                    txtSPQ.Visible = True
                    rfvSPQ.Enabled = True
                End If

                If FldObjName = "txtPcsPerHour" Then
                    ViewState("PcsPerHour") = True
                    lblPcsPerHourRqrd.Visible = True
                    lblPcsPerHour.Visible = True
                    txtPcsPerHour.Visible = True
                    rfvPcsPerHour.Enabled = True
                End If

                If FldObjName = "txtPcsPerCycle" Then
                    ViewState("PcsPerCycle") = True
                    lblPcsPerCycleRqrd.Visible = True
                    lblPcsPerCycle.Visible = True
                    txtPcsPerCycle.Visible = True
                    rfvPcsPerCycle.Enabled = True
                End If

                If FldObjName = "txtSagSpecFrom" Then
                    ViewState("SagSpecFrom") = True
                    lblSagSpecFrom.Visible = True
                    txtSagSpecFrom.Visible = True
                    ddSagSpecFromUOM.Visible = True
                End If

                If FldObjName = "txtSagSpecTo" Then
                    ViewState("SagSpecTo") = True
                    lblSagSpecTo.Visible = True
                    txtSagSpecTo.Visible = True
                    ddSagSpecToUOM.Visible = True
                End If

                If FldObjName = "txtSagPanelSize" Then
                    ViewState("SagPanelSize") = True
                    lblSagPanelSize.Visible = True
                    txtSagPanelSize.Visible = True
                    ddSagPanelUOM.Visible = True
                End If

                If FldObjName = "txtTravel" Then
                    ViewState("Travel") = True
                    lblTravel.Visible = True
                    txtTravel.Visible = True
                End If

                If FldObjName = "txtCallUpNo" Then
                    ViewState("CallUpNo") = True
                    lblCallUpNo.Visible = True
                    txtCallUpNo.Visible = True
                End If

                If FldObjName = "txtLineSpeed" Then
                    ViewState("LineSpeed") = True
                    lblLineSpeed.Visible = True
                    txtLineSpeed.Visible = True
                    ddLineSpeed.Visible = True
                End If

                If FldObjName = "txtPressCycles" Then
                    ViewState("PressCycles") = True
                    lblPressCycles.Visible = True
                    txtPressCycles.Visible = True
                End If

                If FldObjName = "txtStandardTime" Then
                    ViewState("StandardTime") = True
                    lblStandardTime.Visible = True
                    txtStandardTime.Visible = True
                End If

                If FldObjName = "txtQuantity" Then
                    ViewState("Quantity") = True
                    lblQuantity.Visible = True
                    txtQuantity.Visible = True
                End If

                If FldObjName = "txtOvenCondTemp" Then
                    ViewState("OvenCondTemp") = True
                    lblOvenCondTemp.Visible = True
                    txtOvenCondTemp.Visible = True
                    ddOvenCondTempUOM.Visible = True
                End If

                If FldObjName = "txtOvenCondTime" Then
                    ViewState("OvenCondTime") = True
                    lblOvenCondTime.Visible = True
                    txtOvenCondTime.Visible = True
                    ddOvenCondTimeUOM.Visible = True
                End If

                If FldObjName = "txtBondTemp" Then
                    ViewState("BondTemp") = True
                    lblBondTemp.Visible = True
                    txtBondTemp.Visible = True
                    ddBondTempUOM.Visible = True
                End If

                If FldObjName = "txtBondTime" Then
                    ViewState("BondTime") = True
                    lblBondTime.Visible = True
                    txtBondTime.Visible = True
                    ddBondTimeUOM.Visible = True
                End If

                If FldObjName = "txtBondPLFrom" Then
                    ViewState("BondPLFrom") = True
                    lblBondPLFrom.Visible = True
                    txtBondPLFrom.Visible = True
                    ddBondPLFromUOM.Visible = True
                End If

                If FldObjName = "txtBondPLTo" Then
                    ViewState("BondPLTo") = True
                    lblBondPLTo.Visible = True
                    txtBondPLTo.Visible = True
                    ddBondPLToUOM.Visible = True
                End If

                If FldObjName = "txtExpTemp" Then
                    ViewState("ExpTemp") = True
                    lblExpTemp.Visible = True
                    txtExpTemp.Visible = True
                    ddExpTempUOM.Visible = True
                End If

                If FldObjName = "txtExpTime" Then
                    ViewState("ExpTime") = True
                    lblExpTime.Visible = True
                    txtExpTime.Visible = True
                    ddExpTimeUOM.Visible = True
                End If

                If FldObjName = "txtExpSpecFrom" Then
                    ViewState("ExpSpecFrom") = True
                    lblExpSpecFrom.Visible = True
                    txtExpSpecFrom.Visible = True
                    ddExpSpecFromUOM.Visible = True
                End If

                If FldObjName = "txtExpSpecTo" Then
                    ViewState("ExpSpecTo") = True
                    lblExpSpecTo.Visible = True
                    txtExpSpecTo.Visible = True
                    ddExpSpecToUOM.Visible = True
                End If

                If FldObjName = "txtConfiguration" Then
                    ViewState("Configuration") = True
                    lblConfiguration.Visible = True
                    txtConfiguration.Visible = True
                End If

                If FldObjName = "txtWeightFrom" Then
                    ViewState("WeightFrom") = True
                    lblWeightFrom.Visible = True
                    txtWeightFrom.Visible = True
                    ddWeightFromUOM.Visible = True
                End If

                If FldObjName = "txtWeightTo" Then
                    ViewState("WeightTo") = True
                    lblWeightTo.Visible = True
                    txtWeightTo.Visible = True
                    ddWeightToUOM.Visible = True
                End If

                If FldObjName = "txtWeightFrequency" Then
                    ViewState("WeightFrequency") = True
                    lblWeightFrequency.Visible = True
                    txtWeightFrequency.Visible = True
                End If

                If FldObjName = "txtMoldability" Then
                    ViewState("Moldability") = True
                    lblMoldability.Visible = True
                    txtMoldability.Visible = True
                End If

                If FldObjName = "txtMoldOvenCondTemp" Then
                    ViewState("MoldOvenCondTemp") = True
                    lblMoldOvenCondTemp.Visible = True
                    txtMoldOvenCondTemp.Visible = True
                    ddMoldOvenCondTempUOM.Visible = True
                End If

                If FldObjName = "txtMoldOvenCondTime" Then
                    ViewState("MoldOvenCondTime") = True
                    lblMoldOvenCondTime.Visible = True
                    txtMoldOvenCondTime.Visible = True
                    ddMoldOvenCondTimeUOM.Visible = True
                End If

                If FldObjName = "txtCoating" Then
                    ViewState("Coating") = True
                    lblCoating.Visible = True
                    txtCoating.Visible = True
                End If

                If FldObjName = "txtShrinkage" Then
                    ViewState("Shrinkage") = True
                    lblShrinkage.Visible = True
                    txtShrinkage.Visible = True
                    ddShrinkageUOM.Visible = True
                End If

                If FldObjName = "txtShrinkOvenCondTemp" Then
                    ViewState("ShrinkOvenCondTemp") = True
                    lblShrinkOvenCondTemp.Visible = True
                    txtShrinkOvenCondTemp.Visible = True
                    ddShrinkOvenCondTempUOM.Visible = True
                End If

                If FldObjName = "txtShrinkOvenCondTime" Then
                    ViewState("ShrinkOvenCondTime") = True
                    lblShrinkOvenCondTime.Visible = True
                    txtShrinkOvenCondTime.Visible = True
                    ddShrinkOvenCondTimeUOM.Visible = True
                End If

                If FldObjName = "txtShrinkOvenFrequency" Then
                    ViewState("ShrinkOvenFrequency") = True
                    lblShrinkOvenFrequency.Visible = True
                    txtShrinkOvenFrequency.Visible = True
                End If

                If FldObjName = "txtBallTestFrom" Then
                    ViewState("BallTestFrom") = True
                    lblBallTestFrom.Visible = True
                    txtBallTestFrom.Visible = True
                    ddBallTestFromUOM.Visible = True
                End If

                If FldObjName = "txtBallTestTo" Then
                    ViewState("BallTestTo") = True
                    lblBallTestTo.Visible = True
                    txtBallTestTo.Visible = True
                    ddBallTestToUOM.Visible = True
                End If

                If FldObjName = "txtBallFrequency" Then
                    ViewState("BallFrequency") = True
                    lblBallFrequency.Visible = True
                    txtBallFrequency.Visible = True
                End If

                If FldObjName = "txtReleasePoly" Then
                    ViewState("ReleasePoly") = True
                    lblReleasePoly.Visible = True
                    txtReleasePoly.Visible = True
                End If

                If FldObjName = "txtGluePumpCapacity" Then
                    ViewState("GluePumpCapacity") = True
                    lblGluePumpCapacity.Visible = True
                    txtGluePumpCapacity.Visible = True
                    ddGluePumpCapacityUOM.Visible = True
                End If

                If FldObjName = "txtNominalWeight" Then
                    ViewState("NominalWeight") = True
                    lblNominalWeight.Visible = True
                    txtNominalWeight.Visible = True
                    ddNominalWeightUOM.Visible = True
                End If

                If FldObjName = "txtHangTest" Then
                    ViewState("HangTest") = True
                    lblHangTest.Visible = True
                    txtHangTest.Visible = True
                    ddHangTestUOM.Visible = True
                End If

                If FldObjName = "txtHardnessFrom" Then
                    ViewState("HardnessFrom") = True
                    lblHardnessFrom.Visible = True
                    txtHardnessFrom.Visible = True
                    ddHardnessFromUOM.Visible = True
                End If

                If FldObjName = "txtHardnessTo" Then
                    ViewState("HardnessTo") = True
                    lblHardnessTo.Visible = True
                    txtHardnessTo.Visible = True
                    ddHardnessToUOM.Visible = True
                End If

                If FldObjName = "txtElongation" Then
                    ViewState("Elongation") = True
                    lblElongation.Visible = True
                    ddElongationUOM.Visible = True
                    txtElongation.Visible = True
                End If

            Next

        End If
    End Sub

    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Chart Spec Formula control for selection criteria for search
            ' ''ds = MPRModule.GetChartSpec(0, "", "", "", "", "", 0, 0, ViewState("pFormula"), False)
            ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
            ' ''    ddGoToPNo.DataSource = ds
            ' ''    ddGoToPNo.DataTextField = ds.Tables(0).Columns("PartNoDisplay").ColumnName.ToString()
            ' ''    ddGoToPNo.DataValueField = ds.Tables(0).Columns("CSID").ColumnName.ToString()
            ' ''    ddGoToPNo.DataBind()
            ' ''    ddGoToPNo.Items.Insert(0, "Part Number...")
            ' ''    ddGoToPNo.SelectedIndex = 0
            ' ''End If

            ''bind existing data to drop down Chart Spec Formula control for selection criteria for search
            ds = MPRModule.GetChartSpecFormula(0, "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddFormula.DataSource = ds
                ddFormula.DataTextField = ds.Tables(0).Columns("ddFormulaName").ColumnName.ToString()
                ddFormula.DataValueField = ds.Tables(0).Columns("ddFormulaName").ColumnName.ToString()
                ddFormula.DataBind()
                ddFormula.Items.Insert(0, "")
                ddFormula.SelectedIndex = 0
            End If

            ''bind existing data to drop down Commodity control for selection criteria 
            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddCommodity.DataSource = ds
                ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
                ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddCommodity.DataBind()
                ddCommodity.Items.Insert(0, "")
            End If

            ''bind existing data to drop down BPCS Part No control for selection criteria for search
            ' ''ds = commonFunctions.GetPartNo("", "", "UN", "", "")
            ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
            ' ''    ddPartNo.DataSource = ds
            ' ''    ddPartNo.DataTextField = ds.Tables(0).Columns("ddPartNo").ColumnName.ToString()
            ' ''    ddPartNo.DataValueField = ds.Tables(0).Columns("PartNo").ColumnName.ToString()
            ' ''    ddPartNo.DataBind()
            ' ''    ddPartNo.Items.Insert(0, "")
            ' ''    ddPartNo.SelectedIndex = 0
            ' ''End If

            ''bind existing data to drop down Unit of Measure control for selection criteria for search
            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSpGravFromUOM.DataSource = ds
                ddSpGravFromUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddSpGravFromUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddSpGravFromUOM.DataBind()
                ddSpGravFromUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSpGravToUOM.DataSource = ds
                ddSpGravToUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddSpGravToUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddSpGravToUOM.DataBind()
                ddSpGravToUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddThicknessFromUOM.DataSource = ds
                ddThicknessFromUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddThicknessFromUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddThicknessFromUOM.DataBind()
                ddThicknessFromUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddThicknessToUOM.DataSource = ds
                ddThicknessToUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddThicknessToUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddThicknessToUOM.DataBind()
                ddThicknessToUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddTargetThicknessUOM.DataSource = ds
                ddTargetThicknessUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddTargetThicknessUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddTargetThicknessUOM.DataBind()
                ddTargetThicknessUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddWidthUOM.DataSource = ds
                ddWidthUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddWidthUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddWidthUOM.DataBind()
                ddWidthUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddPTAreaFromUOM.DataSource = ds
                ddPTAreaFromUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddPTAreaFromUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddPTAreaFromUOM.DataBind()
                ddPTAreaFromUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddPTAreaToUOM.DataSource = ds
                ddPTAreaToUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddPTAreaToUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddPTAreaToUOM.DataBind()
                ddPTAreaToUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSagSpecFromUOM.DataSource = ds
                ddSagSpecFromUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddSagSpecFromUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddSagSpecFromUOM.DataBind()
                ddSagSpecFromUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSagSpecToUOM.DataSource = ds
                ddSagSpecToUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddSagSpecToUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddSagSpecToUOM.DataBind()
                ddSagSpecToUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSagPanelUOM.DataSource = ds
                ddSagPanelUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddSagPanelUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddSagPanelUOM.DataBind()
                ddSagPanelUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddLineSpeed.DataSource = ds
                ddLineSpeed.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddLineSpeed.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddLineSpeed.DataBind()
                ddLineSpeed.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddOvenCondTempUOM.DataSource = ds
                ddOvenCondTempUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddOvenCondTempUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddOvenCondTempUOM.DataBind()
                ddOvenCondTempUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddOvenCondTimeUOM.DataSource = ds
                ddOvenCondTimeUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddOvenCondTimeUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddOvenCondTimeUOM.DataBind()
                ddOvenCondTimeUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddBondTempUOM.DataSource = ds
                ddBondTempUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBondTempUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBondTempUOM.DataBind()
                ddBondTempUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddBondTimeUOM.DataSource = ds
                ddBondTimeUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBondTimeUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBondTimeUOM.DataBind()
                ddBondTimeUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddBondPLFromUOM.DataSource = ds
                ddBondPLFromUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBondPLFromUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBondPLFromUOM.DataBind()
                ddBondPLFromUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddBondPLToUOM.DataSource = ds
                ddBondPLToUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBondPLToUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBondPLToUOM.DataBind()
                ddBondPLToUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddExpTempUOM.DataSource = ds
                ddExpTempUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddExpTempUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddExpTempUOM.DataBind()
                ddExpTempUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddExpTimeUOM.DataSource = ds
                ddExpTimeUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddExpTimeUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddExpTimeUOM.DataBind()
                ddExpTimeUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddExpSpecFromUOM.DataSource = ds
                ddExpSpecFromUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddExpSpecFromUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddExpSpecFromUOM.DataBind()
                ddExpSpecFromUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddExpSpecToUOM.DataSource = ds
                ddExpSpecToUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddExpSpecToUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddExpSpecToUOM.DataBind()
                ddExpSpecToUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddWeightFromUOM.DataSource = ds
                ddWeightFromUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddWeightFromUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddWeightFromUOM.DataBind()
                ddWeightFromUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddWeightToUOM.DataSource = ds
                ddWeightToUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddWeightToUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddWeightToUOM.DataBind()
                ddWeightToUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddMoldOvenCondTempUOM.DataSource = ds
                ddMoldOvenCondTempUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddMoldOvenCondTempUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddMoldOvenCondTempUOM.DataBind()
                ddMoldOvenCondTempUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddMoldOvenCondTimeUOM.DataSource = ds
                ddMoldOvenCondTimeUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddMoldOvenCondTimeUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddMoldOvenCondTimeUOM.DataBind()
                ddMoldOvenCondTimeUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddShrinkageUOM.DataSource = ds
                ddShrinkageUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddShrinkageUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddShrinkageUOM.DataBind()
                ddShrinkageUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddShrinkOvenCondTempUOM.DataSource = ds
                ddShrinkOvenCondTempUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddShrinkOvenCondTempUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddShrinkOvenCondTempUOM.DataBind()
                ddShrinkOvenCondTempUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddShrinkOvenCondTimeUOM.DataSource = ds
                ddShrinkOvenCondTimeUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddShrinkOvenCondTimeUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddShrinkOvenCondTimeUOM.DataBind()
                ddShrinkOvenCondTimeUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddBallTestFromUOM.DataSource = ds
                ddBallTestFromUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBallTestFromUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBallTestFromUOM.DataBind()
                ddBallTestFromUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddBallTestToUOM.DataSource = ds
                ddBallTestToUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBallTestToUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddBallTestToUOM.DataBind()
                ddBallTestToUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddGluePumpCapacityUOM.DataSource = ds
                ddGluePumpCapacityUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddGluePumpCapacityUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddGluePumpCapacityUOM.DataBind()
                ddGluePumpCapacityUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddNominalWeightUOM.DataSource = ds
                ddNominalWeightUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddNominalWeightUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddNominalWeightUOM.DataBind()
                ddNominalWeightUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddHangTestUOM.DataSource = ds
                ddHangTestUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddHangTestUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddHangTestUOM.DataBind()
                ddHangTestUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddHardnessFromUOM.DataSource = ds
                ddHardnessFromUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddHardnessFromUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddHardnessFromUOM.DataBind()
                ddHardnessFromUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddHardnessToUOM.DataSource = ds
                ddHardnessToUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddHardnessToUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddHardnessToUOM.DataBind()
                ddHardnessToUOM.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUnit(0, "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddElongationUOM.DataSource = ds
                ddElongationUOM.DataTextField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddElongationUOM.DataValueField = ds.Tables(0).Columns("UnitAbbr").ColumnName.ToString()
                ddElongationUOM.DataBind()
                ddElongationUOM.Items.Insert(0, "")
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

#Region "Add/Edit Chart Spec"
    Public Sub BindData()
        Dim ds As DataSet = New DataSet
        Dim ds2 As DataSet = New DataSet
        ViewState("pFormName") = Nothing
        Try

            If ViewState("pCSID") <> Nothing Then
                ds = MPRModule.GetChartSpec(ViewState("pCSID"), "", "", "", "", "", 0, 0, "", False)
                If commonFunctions.CheckDataSet(ds) = True Then
                    txtFormulaID.Text = IIf(ds.Tables(0).Rows(0).Item("FormulaID").ToString() = Nothing, 0, ds.Tables(0).Rows(0).Item("FormulaID").ToString())
                    ddFormula.SelectedValue = ds.Tables(0).Rows(0).Item("FormulaName").ToString()

                    cddMakes.SelectedValue = ds.Tables(0).Rows(0).Item("Make").ToString()
                    cddModel.SelectedValue = ds.Tables(0).Rows(0).Item("Model").ToString()
                    cddProgram.SelectedValue = ds.Tables(0).Rows(0).Item("ProgramID").ToString()

                    cddUGNLocation.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                    cddWorkCenter.SelectedValue = ds.Tables(0).Rows(0).Item("WorkCenter").ToString()
                    cddOEMMfg.SelectedValue = ds.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
                    cddCustomer.SelectedValue = ds.Tables(0).Rows(0).Item("CABBV").ToString()

                    ddCommodity.SelectedValue = ds.Tables(0).Rows(0).Item("CommodityID").ToString()
                    ddPartNo.SelectedValue = ds.Tables(0).Rows(0).Item("PartNo").ToString()
                    ' ''lblPno.Text = ds.Tables(0).Rows(0).Item("PartNo").ToString()
                    txtDesignLvl.Text = ds.Tables(0).Rows(0).Item("DesignLvl").ToString()
                    txtKitPartNo.Text = ds.Tables(0).Rows(0).Item("KitPartNo").ToString()
                    txtFamilyPartNo.Text = ds.Tables(0).Rows(0).Item("FamilyPartNo").ToString()
                    txtBlankPartNo.Text = ds.Tables(0).Rows(0).Item("BlankPartNo").ToString()
                    txtBlankSize.Text = ds.Tables(0).Rows(0).Item("BlankSize").ToString()

                    txtSpGravFrom.Text = ds.Tables(0).Rows(0).Item("SPGravityFrom").ToString()
                    ddSpGravFromUOM.SelectedValue = ds.Tables(0).Rows(0).Item("SPGravityFromUOM").ToString()
                    txtSpGravTo.Text = ds.Tables(0).Rows(0).Item("SPGravityTo").ToString()
                    ddSpGravToUOM.SelectedValue = ds.Tables(0).Rows(0).Item("SPGravityToUOM").ToString()
                    txtSpecFrequency.Text = ds.Tables(0).Rows(0).Item("SpecFrequency").ToString()

                    txtThicknessFrom.Text = ds.Tables(0).Rows(0).Item("ThicknessFrom").ToString()
                    ddThicknessFromUOM.SelectedValue = ds.Tables(0).Rows(0).Item("ThicknessFromUOM").ToString()
                    txtThicknessTo.Text = ds.Tables(0).Rows(0).Item("ThicknessTo").ToString()
                    ddThicknessToUOM.SelectedValue = ds.Tables(0).Rows(0).Item("ThicknessFromUOM").ToString()
                    txtThicknessFrequency.Text = ds.Tables(0).Rows(0).Item("ThicknessFrequency").ToString()

                    txtTargetThickness.Text = ds.Tables(0).Rows(0).Item("TargetThickness").ToString()
                    ddTargetThicknessUOM.SelectedValue = ds.Tables(0).Rows(0).Item("TargetThicknessUOM").ToString()

                    txtWidth.Text = ds.Tables(0).Rows(0).Item("Width").ToString()
                    ddWidthUOM.SelectedValue = ds.Tables(0).Rows(0).Item("WidthUOM").ToString()

                    txtContainerDescription.Text = ds.Tables(0).Rows(0).Item("ContainerDescription").ToString()
                    txtContainerDimensions.Text = ds.Tables(0).Rows(0).Item("ContainerDimensions").ToString()

                    txtPTAreaFrom.Text = ds.Tables(0).Rows(0).Item("PTAreaFrom").ToString()
                    ddPTAreaFromUOM.SelectedValue = ds.Tables(0).Rows(0).Item("PTAreaFromUOM").ToString()
                    txtPTAreaFrom.Text = ds.Tables(0).Rows(0).Item("PTAreaTo").ToString()
                    ddPTAreaToUOM.SelectedValue = ds.Tables(0).Rows(0).Item("PTAreaToUOM").ToString()

                    txtSPQ.Text = ds.Tables(0).Rows(0).Item("SPQ").ToString()
                    txtPcsPerHour.Text = ds.Tables(0).Rows(0).Item("PcsPerHour").ToString()
                    txtPcsPerCycle.Text = ds.Tables(0).Rows(0).Item("PcsPerCycle").ToString()

                    txtSagSpecFrom.Text = ds.Tables(0).Rows(0).Item("SagSpecFrom").ToString()
                    ddSagSpecFromUOM.SelectedValue = ds.Tables(0).Rows(0).Item("SagSpecFromUOM").ToString()
                    txtSagSpecTo.Text = ds.Tables(0).Rows(0).Item("SagSpecTo").ToString()
                    ddSagSpecToUOM.SelectedValue = ds.Tables(0).Rows(0).Item("SagSpecToUOM").ToString()
                    txtSagPanelSize.Text = ds.Tables(0).Rows(0).Item("SagPanelSize").ToString()
                    ddSagPanelUOM.SelectedValue = ds.Tables(0).Rows(0).Item("SagPanelUOM").ToString()

                    txtTravel.Text = ds.Tables(0).Rows(0).Item("Travel").ToString()
                    txtCallUpNo.Text = ds.Tables(0).Rows(0).Item("CallUpNo").ToString()
                    txtLineSpeed.Text = ds.Tables(0).Rows(0).Item("LineSpeed").ToString()
                    ddLineSpeed.SelectedValue = ds.Tables(0).Rows(0).Item("LineSpeedUOM").ToString()
                    txtPressCycles.Text = ds.Tables(0).Rows(0).Item("PressCycles").ToString()
                    txtStandardTime.Text = ds.Tables(0).Rows(0).Item("StandardTime").ToString()
                    txtQuantity.Text = ds.Tables(0).Rows(0).Item("Quantity").ToString()

                    txtOvenCondTemp.Text = ds.Tables(0).Rows(0).Item("OvenCondTemp").ToString()
                    ddOvenCondTempUOM.SelectedValue = ds.Tables(0).Rows(0).Item("OvenCondTempUOM").ToString()
                    txtOvenCondTime.Text = ds.Tables(0).Rows(0).Item("OvenCondTime").ToString()
                    ddOvenCondTimeUOM.SelectedValue = ds.Tables(0).Rows(0).Item("OvenCondTimeUOM").ToString()

                    txtBondTemp.Text = ds.Tables(0).Rows(0).Item("BondTemp").ToString()
                    ddBondTempUOM.SelectedValue = ds.Tables(0).Rows(0).Item("BondTempUOM").ToString()
                    txtBondTime.Text = ds.Tables(0).Rows(0).Item("BondTime").ToString()
                    ddBondTimeUOM.SelectedValue = ds.Tables(0).Rows(0).Item("BondTimeUOM").ToString()

                    txtBondPLFrom.Text = ds.Tables(0).Rows(0).Item("BondPLFrom").ToString()
                    ddBondPLFromUOM.SelectedValue = ds.Tables(0).Rows(0).Item("BondPLFromUOM").ToString()
                    txtBondPLTo.Text = ds.Tables(0).Rows(0).Item("BondPLTo").ToString()
                    ddBondPLToUOM.SelectedValue = ds.Tables(0).Rows(0).Item("BondPLToUOM").ToString()

                    txtExpTemp.Text = ds.Tables(0).Rows(0).Item("ExpTemp").ToString()
                    ddExpTempUOM.SelectedValue = ds.Tables(0).Rows(0).Item("ExpTempUOM").ToString()
                    txtExpTime.Text = ds.Tables(0).Rows(0).Item("ExpTime").ToString()
                    ddExpTimeUOM.SelectedValue = ds.Tables(0).Rows(0).Item("ExpTimeUOM").ToString()

                    txtExpSpecFrom.Text = ds.Tables(0).Rows(0).Item("ExpSpecFrom").ToString()
                    ddExpSpecFromUOM.SelectedValue = ds.Tables(0).Rows(0).Item("ExpSpecFromUOM").ToString()
                    txtExpSpecTo.Text = ds.Tables(0).Rows(0).Item("ExpSpecTo").ToString()
                    ddExpSpecToUOM.SelectedValue = ds.Tables(0).Rows(0).Item("ExpSpecToUOM").ToString()

                    txtConfiguration.Text = ds.Tables(0).Rows(0).Item("Configuration").ToString()

                    txtWeightFrom.Text = ds.Tables(0).Rows(0).Item("WeightFrom").ToString()
                    ddWeightFromUOM.SelectedValue = ds.Tables(0).Rows(0).Item("WeightFromUOM").ToString()
                    txtWeightTo.Text = ds.Tables(0).Rows(0).Item("WeightTo").ToString()
                    ddWeightToUOM.SelectedValue = ds.Tables(0).Rows(0).Item("WeightToUOM").ToString()
                    txtWeightFrequency.Text = ds.Tables(0).Rows(0).Item("WeightFrequency").ToString()

                    txtMoldability.Text = ds.Tables(0).Rows(0).Item("Moldability").ToString()
                    txtMoldOvenCondTemp.Text = ds.Tables(0).Rows(0).Item("MoldOvenCondTemp").ToString()
                    ddMoldOvenCondTempUOM.SelectedValue = ds.Tables(0).Rows(0).Item("MoldOvenCondTempUOM").ToString()
                    txtMoldOvenCondTime.Text = ds.Tables(0).Rows(0).Item("MoldOvenCondTime").ToString()
                    ddMoldOvenCondTimeUOM.SelectedValue = ds.Tables(0).Rows(0).Item("MoldOvenCondTimeUOM").ToString()
                    txtMoldOvenFrequency.Text = ds.Tables(0).Rows(0).Item("MoldOvenFrequency").ToString()

                    txtCoating.Text = ds.Tables(0).Rows(0).Item("Coating").ToString()

                    txtShrinkage.Text = ds.Tables(0).Rows(0).Item("Shrinkage").ToString()
                    ddShrinkageUOM.SelectedValue = ds.Tables(0).Rows(0).Item("ShrinkageUOM").ToString()
                    txtShrinkOvenCondTemp.Text = ds.Tables(0).Rows(0).Item("ShrinkOvenCondTemp").ToString()
                    ddShrinkOvenCondTempUOM.SelectedValue = ds.Tables(0).Rows(0).Item("ShrinkOvenCondTempUOM").ToString()
                    txtShrinkOvenCondTime.Text = ds.Tables(0).Rows(0).Item("ShrinkOvenCondTime").ToString()
                    ddShrinkOvenCondTimeUOM.SelectedValue = ds.Tables(0).Rows(0).Item("ShrinkOvenCondTimeUOM").ToString()
                    txtShrinkOvenFrequency.Text = ds.Tables(0).Rows(0).Item("ShrinkOvenFrequency").ToString()

                    txtBallTestFrom.Text = ds.Tables(0).Rows(0).Item("BallTestFrom").ToString()
                    ddBallTestFromUOM.SelectedValue = ds.Tables(0).Rows(0).Item("BallTestFromUOM").ToString()
                    txtBallTestTo.Text = ds.Tables(0).Rows(0).Item("BallTestTo").ToString()
                    ddBallTestToUOM.SelectedValue = ds.Tables(0).Rows(0).Item("BallTestToUOM").ToString()
                    txtBallFrequency.Text = ds.Tables(0).Rows(0).Item("BallFrequency").ToString()

                    txtReleasePoly.Text = ds.Tables(0).Rows(0).Item("ReleasePoly").ToString()
                    txtGluePumpCapacity.Text = ds.Tables(0).Rows(0).Item("GluePumpCapacity").ToString()
                    ddGluePumpCapacityUOM.SelectedValue = ds.Tables(0).Rows(0).Item("GluePumpCapacityUOM").ToString()

                    txtNominalWeight.Text = ds.Tables(0).Rows(0).Item("NominalWeight").ToString()
                    ddNominalWeightUOM.SelectedValue = ds.Tables(0).Rows(0).Item("NominalWeightUOM").ToString()

                    txtHangTest.Text = ds.Tables(0).Rows(0).Item("HangTest").ToString()
                    ddHangTestUOM.SelectedValue = ds.Tables(0).Rows(0).Item("HangTestUOM").ToString()

                    txtHardnessFrom.Text = ds.Tables(0).Rows(0).Item("HardnessFrom").ToString()
                    ddHardnessFromUOM.SelectedValue = ds.Tables(0).Rows(0).Item("HardnessFromUOM").ToString()
                    txtHardnessTo.Text = ds.Tables(0).Rows(0).Item("HardnessTo").ToString()
                    ddHardnessToUOM.SelectedValue = ds.Tables(0).Rows(0).Item("HardnessToUOM").ToString()

                    ddElongationUOM.SelectedValue = ds.Tables(0).Rows(0).Item("ElongationUOM").ToString()
                    txtElongation.Text = ds.Tables(0).Rows(0).Item("Elongation").ToString()

                    ddObsolete.SelectedValue = ds.Tables(0).Rows(0).Item("Obsolete").ToString()
                    txtNotes.Text = ds.Tables(0).Rows(0).Item("Notes").ToString()
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
    End Sub 'EOF BindData()

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim DefaultDate As Date = Date.Today
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim PartNo As String = Nothing
            Dim PartDesc As String = Nothing
            Dim ds As DataSet = New DataSet

            lblMessage.Text = Nothing
            lblMessage.Visible = False

            If ViewState("pCSID") <> Nothing Then
                '***************
                '* Update Data
                '***************
                MPRModule.UpdateChartSpec(ViewState("pCSID"), ddUGNLocation.SelectedValue, "", ddOEMMfg.SelectedValue, ddCustomer.SelectedValue, "", ddCommodity.SelectedValue, ddPartNo.SelectedValue, txtKitPartNo.Text, txtFamilyPartNo.Text, ddWorkCenter.SelectedValue, IIf(txtThicknessFrom.Text = Nothing, 0, txtThicknessFrom.Text), IIf(txtThicknessTo.Text = Nothing, 0, txtThicknessTo.Text), IIf(txtTargetThickness.Text = Nothing, 0, txtTargetThickness.Text), IIf(txtWidth.Text = Nothing, 0, txtWidth.Text), txtFormulaID.Text, txtContainerDescription.Text, txtContainerDimensions.Text, IIf(txtSPQ.Text = Nothing, 0, txtSPQ.Text), IIf(txtPcsPerHour.Text = Nothing, 0, txtPcsPerHour.Text), IIf(txtPcsPerCycle.Text = Nothing, 0, txtPcsPerCycle.Text), IIf(txtSagPanelSize.Text = Nothing, 0, txtSagPanelSize.Text), ddSagPanelUOM.SelectedValue, IIf(txtTravel.Text = Nothing, 0, txtTravel.Text), IIf(txtCallUpNo.Text = Nothing, 0, txtCallUpNo.Text), IIf(txtLineSpeed.Text = Nothing, 0, txtLineSpeed.Text), IIf(txtPressCycles.Text = Nothing, 0, txtPressCycles.Text), IIf(txtStandardTime.Text = Nothing, 0, txtStandardTime.Text), IIf(txtQuantity.Text = Nothing, 0, txtQuantity.Text), txtNotes.Text, ddObsolete.SelectedValue, DefaultUser, DefaultDate)

            Else 'New Record
                '***************
                '* Save Data
                '***************
                MPRModule.InsertChartSpec(ddUGNLocation.SelectedValue, "", ddOEMMfg.SelectedValue, ddCustomer.SelectedValue, "", ddCommodity.SelectedValue, ddPartNo.SelectedValue, txtKitPartNo.Text, txtFamilyPartNo.Text, ddWorkCenter.SelectedValue, IIf(txtThicknessFrom.Text = Nothing, 0, txtThicknessFrom.Text), IIf(txtThicknessTo.Text = Nothing, 0, txtThicknessTo.Text), IIf(txtTargetThickness.Text = Nothing, 0, txtTargetThickness.Text), IIf(txtWidth.Text = Nothing, 0, txtWidth.Text), txtFormulaID.Text, txtContainerDescription.Text, txtContainerDimensions.Text, IIf(txtSPQ.Text = Nothing, 0, txtSPQ.Text), IIf(txtPcsPerHour.Text = Nothing, 0, txtPcsPerHour.Text), IIf(txtPcsPerCycle.Text = Nothing, 0, txtPcsPerCycle.Text), IIf(txtSagPanelSize.Text = Nothing, 0, txtSagPanelSize.Text), ddSagPanelUOM.SelectedValue, IIf(txtTravel.Text = Nothing, 0, txtTravel.Text), IIf(txtCallUpNo.Text = Nothing, 0, txtCallUpNo.Text), IIf(txtLineSpeed.Text = Nothing, 0, txtLineSpeed.Text), IIf(txtPressCycles.Text = Nothing, 0, txtPressCycles.Text), IIf(txtStandardTime.Text = Nothing, 0, txtStandardTime.Text), IIf(txtQuantity.Text = Nothing, 0, txtQuantity.Text), txtNotes.Text, ddObsolete.SelectedValue, DefaultUser, DefaultDate)

            End If

            Response.Redirect("ChartSpecList.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnSave1_Click

    ' ''Protected Sub ddProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProgram.SelectedIndexChanged
    ' ''    If ddProgram.SelectedValue <> Nothing Then
    ' ''        ''System.Threading.Thread.Sleep(3000)

    ' ''        Dim ds As DataSet = New DataSet
    ' ''        ds = commonFunctions.GetPlatformProgram(0, ddProgram.SelectedValue, "", "", ddMakes.SelectedValue)
    ' ''        If commonFunctions.CheckDataSet(ds) = True Then
    ' ''            If ds.Tables(0).Rows(0).Item("EOPMM").ToString() <> "" Then
    ' ''                txtEOP.Text = ds.Tables(0).Rows(0).Item("EOPMM").ToString() & "/" & NoOfDays(ds.Tables(0).Rows(0).Item("EOPMM").ToString()) & "/" & ds.Tables(0).Rows(0).Item("EOPYY").ToString()
    ' ''            End If
    ' ''            If ds.Tables(0).Rows(0).Item("SOPMM").ToString() <> "" Then
    ' ''                txtSOP.Text = ds.Tables(0).Rows(0).Item("SOPMM").ToString() & "/01/" & ds.Tables(0).Rows(0).Item("SOPYY").ToString()
    ' ''            End If
    ' ''            If ds.Tables(0).Rows(0).Item("ServiceEOPMM").ToString() <> "" Then
    ' ''                txtSrvEOP.Text = ds.Tables(0).Rows(0).Item("ServiceEOPMM").ToString() & "/" & NoOfDays(ds.Tables(0).Rows(0).Item("ServiceEOPMM").ToString()) & "/" & ds.Tables(0).Rows(0).Item("ServiceEOPYY").ToString()
    ' ''            End If
    ' ''            cddOEMMfg.SelectedValue = ds.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
    ' ''            'iBtnPreviewDetail.Visible = True
    ' ''        Else
    ' ''            ' iBtnPreviewDetail.Visible = False
    ' ''        End If
    ' ''    End If 'EOF ddProgram.SelectedValue

    ' ''End Sub 'EOF ddProgram_SelectedIndexChanged

    Protected Function NoOfDays(ByVal Month As Integer) As String
        Dim NOD As String = Nothing
        Select Case Month
            Case "01"
                NOD = "31"
            Case "02"
                NOD = "28"
            Case "03"
                NOD = "31"
            Case "04"
                NOD = "30"
            Case "05"
                NOD = "31"
            Case "06"
                NOD = "30"
            Case "07"
                NOD = "31"
            Case "08"
                NOD = "31"
            Case "09"
                NOD = "30"
            Case "10"
                NOD = "31"
            Case "11"
                NOD = "30"
            Case "12"
                NOD = "31"
        End Select

        Return NOD
    End Function

#End Region ' "Add/Edit Chart Spec"

End Class
