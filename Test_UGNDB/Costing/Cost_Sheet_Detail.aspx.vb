' ***********************************************************************************************
'
' Name:		CostSheetDetail.aspx
' Purpose:	This Code Behind is for the Cost Sheet Detail of the Costing/Quote Forms
'
' Date		    Author	    
' 10/08/2008    Roderick Carlson  
' 11/16/2009    Roderick Carlson - Get dTempProductionRatesWeightPerArea value sooner, Show Weight Per Are if fleece Type or Specific Gravity = 0, make current cost sheets previous on replicate
' 12/03/2009    Roderick Carlson - added Update Totals button and adjusted changing status
' 12/07/2009    Roderick Carlson - CO-2793 - made sure capital ordinal was inserted
' 12/09/2009    Roderick Carlson - CO-2798 - refresh upper left corner of Production Rates tab based on formula, do not allow max line speed to exceed formula line speed
' 12/10/2009    Roderick Carlson - CO-2802 - updated BindData function
' 01/11/2010    Roderick Carlson - Adjust Dropdown Cost Sheet Status logic
' 01/12/2010    Roderick Carlson - CO-2822 - Freight can be stored per cost sheet
' 05/19/2010    Roderick Carlson - Add logic for Scrap Factor
' 06/17/2010    Roderick Carlson - CO-2910 - In the Button to Calculate logic added to check for part weight. If part weight is zero and foam has a value then use it.
' 06/23/2010    Roderick Carlson - CO - Added ReplicatedTo List
' 08/17/2010    Roderick Carlson - The Summary Percentages needed to be divided by 100 before saving
' 08/24/2010    Roderick Carlson - Do not calculate scrap total if all subtotals are 0 - help prevent old format from previewing wrong
' 09/01/2010    Roderick Carlson - CO-2969 Refresh Department Grid when other grids are refreshed
' 11/23/2010    Roderick Carlson - Added RFD Search Popup and collection
' 06/28/2011    Roderick Carlson - Always pull CoatingFactor from Formula
' 07/13/2011    Roderick Carlson - DCADE - always pull CatchingAbilityFactor value from maintenance table and override text boxalways pull CatchingAbilityFactor value from maintenance table and override text box
' 07/21/2011    Roderick Carlson - Added Barrier Run Rate Calculations, function sp_Get_Formula_Barrier_Run_Rate
' 08/26/2011    Roderick Carlson - allow negative material costs
' 10/27/2011    Roderick Carlson - when replicating always get fresh PartSpecificationsSpecificGravityValue
' 12/13/2011    Roderick Carlson - Add Program Make Cascading Dropdowns
' 02/03/2012    Roderick Carlson - Add Model to Program List
' 05/08/2012    Roderick Carlson - Do not let program be added without year
' 09/10/2012    Roderick Carlson - DB Cleanup - adjust image upload
' 01/06/2014    LREY    - Replaced "BPCS Part No" to "Part No" wherever used. 
' 01/08/2014    LREY    - Replaced GetCustomer with GetOEMManufacturer. SOLDTO|CABBV is not used in the new ERP.
' 04/28/2014    LREY    - Added QuickQuote checkbox
' 04/29/2014    LREY    - Added two new gridviews for Assumptions and Approval used by Costing group only
' ************************************************************************************************

Partial Class Cost_Sheet_Detail
    Inherits System.Web.UI.Page
    Protected Sub ddFooterMaterial_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data based on the Footer Material drop down      
        ''*******

        Try

            ClearMessages()

            Dim ddTempMaterial As DropDownList
            Dim txtTempMaterialCostPerUnit As TextBox
            Dim txtTempMaterialFreightCost As TextBox

            Dim dsMaterial As DataSet

            ddTempMaterial = CType(sender, DropDownList)

            txtTempMaterialCostPerUnit = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialCostPerUnit"), TextBox)
            txtTempMaterialFreightCost = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialFreightCost"), TextBox)

            dsMaterial = CostingModule.GetMaterial(ddTempMaterial.SelectedValue, "", "", "", 0, 0, "", "", False, False, False, False, False, False)
            If commonFunctions.CheckDataSet(dsMaterial) = True Then

                If dsMaterial.Tables(0).Rows(0).Item("QuoteCost") IsNot System.DBNull.Value Then
                    txtTempMaterialCostPerUnit.Text = dsMaterial.Tables(0).Rows(0).Item("QuoteCost")
                    txtTempMaterialFreightCost.Text = dsMaterial.Tables(0).Rows(0).Item("FreightCost")
                End If

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
    Protected Sub ddFooterPackaging_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data based on the Footer Packaging drop down      
        ''*******

        Try
            ClearMessages()

            Dim ddTempPackaging As DropDownList
            Dim txtTempPackagingCostPerUnit As TextBox

            Dim dsMaterial As DataSet

            ddTempPackaging = CType(sender, DropDownList)

            txtTempPackagingCostPerUnit = CType(gvPackaging.FooterRow.FindControl("txtFooterPackagingCostPerUnit"), TextBox)

            dsMaterial = CostingModule.GetMaterial(ddTempPackaging.SelectedValue, "", "", "", 0, 0, "", "", False, False, False, False, False, False)
            If commonFunctions.CheckDataSet(dsMaterial) = True Then

                If dsMaterial.Tables(0).Rows(0).Item("QuoteCost") IsNot System.DBNull.Value Then
                    txtTempPackagingCostPerUnit.Text = dsMaterial.Tables(0).Rows(0).Item("QuoteCost")
                End If

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
    Protected Sub ddFooterLabor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data based on the Footer Packaging drop down      
        ''*******

        Try
            ClearMessages()

            Dim ddTempLabor As DropDownList
            Dim txtTempLaborRate As TextBox
            Dim txtTempLaborCrewSize As TextBox
            Dim cbTempLaborIsOffline As CheckBox

            Dim dsLabor As DataSet

            ddTempLabor = CType(sender, DropDownList)

            txtTempLaborRate = CType(gvLabor.FooterRow.FindControl("txtFooterLaborRate"), TextBox)
            txtTempLaborCrewSize = CType(gvLabor.FooterRow.FindControl("txtFooterLaborCrewSize"), TextBox)
            cbTempLaborIsOffline = CType(gvLabor.FooterRow.FindControl("cbFooterLaborIsOffline"), CheckBox)

            dsLabor = CostingModule.GetLabor(ddTempLabor.SelectedValue, "", False, False)
            If commonFunctions.CheckDataSet(dsLabor) = True Then

                If dsLabor.Tables(0).Rows(0).Item("Rate") IsNot System.DBNull.Value Then
                    If dsLabor.Tables(0).Rows(0).Item("Rate") > 0 Then
                        txtTempLaborRate.Text = dsLabor.Tables(0).Rows(0).Item("Rate")
                    End If
                End If

                If dsLabor.Tables(0).Rows(0).Item("CrewSize") IsNot System.DBNull.Value Then
                    If dsLabor.Tables(0).Rows(0).Item("CrewSize") > 0 Then
                        txtTempLaborCrewSize.Text = dsLabor.Tables(0).Rows(0).Item("CrewSize")
                    End If
                End If

                If dsLabor.Tables(0).Rows(0).Item("isOffline") IsNot System.DBNull.Value Then

                    cbTempLaborIsOffline.Checked = dsLabor.Tables(0).Rows(0).Item("isOffline")

                End If
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
    Protected Sub ddFooterOverhead_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data based on the Footer Packaging drop down      
        ''*******

        Try
            ClearMessages()

            Dim ddTempOverhead As DropDownList
            Dim txtTempOverheadRate As TextBox
            Dim txtTempOverheadVariableRate As TextBox
            Dim txtTempOverheadCrewSize As TextBox
            Dim cbTempOverheadIsOffline As CheckBox

            Dim dsOverhead As DataSet

            ddTempOverhead = CType(sender, DropDownList)

            txtTempOverheadRate = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadRate"), TextBox)
            txtTempOverheadVariableRate = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadVariableRate"), TextBox)
            txtTempOverheadCrewSize = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadCrewSize"), TextBox)
            cbTempOverheadIsOffline = CType(gvOverhead.FooterRow.FindControl("cbFooterOverheadIsOffline"), CheckBox)

            dsOverhead = CostingModule.GetOverhead(ddTempOverhead.SelectedValue, "")
            If commonFunctions.CheckDataSet(dsOverhead) = True Then

                If dsOverhead.Tables(0).Rows(0).Item("Rate") IsNot System.DBNull.Value Then
                    If dsOverhead.Tables(0).Rows(0).Item("Rate") > 0 Then
                        txtTempOverheadRate.Text = dsOverhead.Tables(0).Rows(0).Item("Rate")
                    End If
                End If

                If dsOverhead.Tables(0).Rows(0).Item("VariableRate") IsNot System.DBNull.Value Then
                    If dsOverhead.Tables(0).Rows(0).Item("VariableRate") > 0 Then
                        txtTempOverheadVariableRate.Text = dsOverhead.Tables(0).Rows(0).Item("VariableRate")
                    End If
                End If

                If dsOverhead.Tables(0).Rows(0).Item("CrewSize") IsNot System.DBNull.Value Then
                    If dsOverhead.Tables(0).Rows(0).Item("CrewSize") > 0 Then
                        txtTempOverheadCrewSize.Text = dsOverhead.Tables(0).Rows(0).Item("CrewSize")
                    End If
                End If

                If dsOverhead.Tables(0).Rows(0).Item("isOffline") IsNot System.DBNull.Value Then
                    cbTempOverheadIsOffline.Checked = dsOverhead.Tables(0).Rows(0).Item("isOffline")
                End If
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
    Protected Sub ddFooterMiscCost_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data based on the Footer Packaging drop down      
        ''*******

        Try

            ClearMessages()

            Dim ddTempMiscCost As DropDownList
            Dim txtTempMiscCostRate As TextBox

            Dim dsMiscCost As DataSet

            ddTempMiscCost = CType(sender, DropDownList)

            txtTempMiscCostRate = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCostRate"), TextBox)

            dsMiscCost = CostingModule.GetMiscCost(ddTempMiscCost.SelectedValue, "")
            If commonFunctions.CheckDataSet(dsMiscCost) = True Then

                If dsMiscCost.Tables(0).Rows(0).Item("Rate") IsNot System.DBNull.Value Then
                    If dsMiscCost.Tables(0).Rows(0).Item("Rate") > 0 Then
                        txtTempMiscCostRate.Text = dsMiscCost.Tables(0).Rows(0).Item("Rate")
                    End If
                End If
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
    
    Protected Sub CopyImage(ByVal PreviousCostSheetID As Integer)

        Try
            Dim dsImages As DataSet

            Dim TempImageBytes As Byte()

            dsImages = CostingModule.GetCostSheetSketchInfo(PreviousCostSheetID)
            If commonFunctions.CheckDataset(dsImages) = True Then

                If dsImages.Tables(0).Rows(0).Item("SketchImage") IsNot System.DBNull.Value Then
                    TempImageBytes = dsImages.Tables(0).Rows(0).Item("SketchImage")
                    CostingModule.UpdateCostSheetSketchImage(ViewState("CostSheetID"), TempImageBytes)
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
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
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleBPCSPopUps = ""
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Function
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
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleDrawingPopUps = ""
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Function

    Protected Function HandleRFDPopUps(ByVal RFDNoClientControlID As String, _
        ByVal RFDSelectionTypeClientControlID As String, _
        ByVal RFDChildRowClientControlID As String) As String

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
                "../RFD/RFD_To_Cost_Sheet.aspx?RFDNoControlID=" & RFDNoClientControlID _
                & "&RFDSelectionTypeControlID=" & RFDSelectionTypeClientControlID _
                & "&RFDChildRowControlID=" & RFDChildRowClientControlID

            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','RFDNoPopupSearch','" & _
                strWindowAttribs & "');return false;"

            HandleRFDPopUps = strClientScript

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleRFDPopUps = ""
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Function

    Private Sub RefreshGridViews()

        Try
            'refresh gridviews
            gvAdditionalOfflineRate.DataBind()
            gvDepartment.DataBind()
            gvLabor.DataBind()
            gvOverhead.DataBind()
            gvMaterial.DataBind()
            gvPackaging.DataBind()
            gvMiscCost.DataBind()
            gvCapital.DataBind()
            gvProductionLimit.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Private Sub CopyFormulaFullDetails(ByVal FormulaID As Integer)

        'need to put warning on dropdown - if changd then current information will be overwritten
        Try

            'update Part Specification Tab
            If ViewState("Formula_SpecificGravity") > 0 Then
                txtPartSpecificationsSpecificGravityValue.Text = ViewState("Formula_SpecificGravity")
            Else
                txtPartSpecificationsSpecificGravityValue.Text = ""
            End If

            If ViewState("Formula_WeightPerArea") > 0 Then
                txtPartSpecificationsWeightPerAreaValue.Text = ViewState("Formula_WeightPerArea")
            Else
                txtPartSpecificationsWeightPerAreaValue.Text = ""
            End If

            cbPartSpecificationsIsDiecutValue.Checked = ViewState("Formula_isDiecut")
            ddPartSpecificationsProcessValue.SelectedValue = ViewState("Formula_ProcessID")

            'clear out in order to allow calculation to get fresh value from formula
            txtProductionRatesCoatingFactorValue.Text = ""

            'copy department, labor, overhead, material, packaging, misccost lists from formula to cost sheet
            CostingModule.CopyFormulaToCostSheetDepartment(FormulaID, ViewState("CostSheetID"))
            CostingModule.CopyFormulaToCostSheetLabor(FormulaID, ViewState("CostSheetID"))
            CostingModule.CopyFormulaToCostSheetOverhead(FormulaID, ViewState("CostSheetID"))
            CostingModule.CopyFormulaToCostSheetMaterial(FormulaID, ViewState("CostSheetID"))
            CostingModule.CopyFormulaToCostSheetPackaging(FormulaID, ViewState("CostSheetID"))
            CostingModule.CopyFormulaToCostSheetMiscCost(FormulaID, ViewState("CostSheetID"))

            RefreshGridViews()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Private Sub GetFormulaTopLevelDetails(ByVal FormulaID As Integer)

        Try
            ViewState("Formula_SpecificGravity") = 0.0
            ViewState("Formula_MaxMixCapacity") = 0
            ViewState("Formula_MaxLineSpeed") = 0
            ViewState("Formula_MaxPressCycles") = 0
            ViewState("Formula_CoatingSides") = 0
            ViewState("Formula_WeightPerArea") = 0.0
            ViewState("Formula_MaxFormingRate") = 0
            ViewState("Formula_isDiecut") = False
            ViewState("Formula_ProcessID") = 0
            ViewState("Formula_isRecycleReturn") = False
            ViewState("Formula_TemplateID") = 0
            ViewState("Formula_isFleeceType") = False

            Dim ds As DataSet

            ds = CostingModule.GetFormula(FormulaID)

            If commonFunctions.CheckDataSet(ds) = True Then '*** need linespeed units too

                If ds.Tables(0).Rows(0).Item("SpecificGravity") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("SpecificGravity") > 0 Then
                        ViewState("Formula_SpecificGravity") = ds.Tables(0).Rows(0).Item("SpecificGravity")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaxMixCapacity") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaxMixCapacity") > 0 Then
                        ViewState("Formula_MaxMixCapacity") = ds.Tables(0).Rows(0).Item("MaxMixCapacity")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaxLineSpeed") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaxLineSpeed") > 0 Then
                        ViewState("Formula_MaxLineSpeed") = ds.Tables(0).Rows(0).Item("MaxLineSpeed")
                    End If

                End If

                If ds.Tables(0).Rows(0).Item("MaxPressCycles") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaxPressCycles") > 0 Then
                        ViewState("Formula_MaxPressCycles") = ds.Tables(0).Rows(0).Item("MaxPressCycles")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CoatingSides") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CoatingSides") > 0 Then
                        ViewState("Formula_CoatingSides") = ds.Tables(0).Rows(0).Item("CoatingSides")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("WeightPerArea") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("WeightPerArea") > 0 Then
                        ViewState("Formula_WeightPerArea") = ds.Tables(0).Rows(0).Item("WeightPerArea")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaxFormingRate") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaxFormingRate") > 0 Then
                        ViewState("Formula_MaxFormingRate") = ds.Tables(0).Rows(0).Item("MaxFormingRate")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("isDiecut") IsNot System.DBNull.Value Then
                    ViewState("Formula_isDiecut") = ds.Tables(0).Rows(0).Item("isDiecut")
                End If

                If ds.Tables(0).Rows(0).Item("ProcessID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ProcessID") > 0 Then
                        ViewState("Formula_ProcessID") = ds.Tables(0).Rows(0).Item("ProcessID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("isRecycleReturn") IsNot System.DBNull.Value Then
                    ViewState("Formula_isRecycleReturn") = ds.Tables(0).Rows(0).Item("isRecycleReturn")
                End If

                If ds.Tables(0).Rows(0).Item("TemplateID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TemplateID") > 0 Then
                        ViewState("Formula_TemplateID") = ds.Tables(0).Rows(0).Item("TemplateID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("isFleeceType") IsNot System.DBNull.Value Then
                    ViewState("Formula_isFleeceType") = ds.Tables(0).Rows(0).Item("isFleeceType")
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            'Dim dsSubscription As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                'If iTeamMemberID = 530 Then                   
                '    iTeamMemberID = 303 'Julie.Sinchak 
                'End If

                ''CST Costing Coordinator
                'dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 41)
                'If commonFunctions.CheckDataSet(dsSubscription) = True Then
                '    ViewState("SubscriptionID") = 41
                'End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 57)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    'there should be no read only viewers of this page, unless admin users see an approved quote
                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isRestricted") = False
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                            ViewState("isRestricted") = False
                        Case 13 '*** UGNAssist: Create/Edit/No Delete

                            'If ViewState("SubscriptionID") = 41 Then
                            ViewState("isAdmin") = True
                            ViewState("isRestricted") = False
                            'End If


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
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub CheckDesignationType()

        Try

            If ddDesignationTypeValue.SelectedValue = "B" Or ddDesignationTypeValue.SelectedValue = "R" Then
                'show if Semi-Finished Good or Raw Material
                lblNewPartNoLabel.Visible = True
                txtNewPartNoValue.Visible = True
                lblNewPartRevisionLabel.Visible = True
                txtNewPartRevisionValue.Visible = True
                lblOriginalPartNoLabel.Visible = True
                txtOriginalPartNoValue.Visible = True
                lblOriginalPartRevisionLabel.Visible = True
                txtOriginalPartRevisionValue.Visible = True
                lblPurchasedGoodLabel.Visible = True
                ddPurchasedGoodValue.Visible = True
                'iBtnGetNewPartNo.Visible = True
                'iBtnOriginalPartNo.Visible = True

                iBtnGetNewPartNo.Visible = Not ViewState("isApproved")
                iBtnOriginalPartNo.Visible = Not ViewState("isApproved")
            ElseIf ddDesignationTypeValue.SelectedValue = "C" Then
                'show if finsihed good
                lblCommodityLabel.Visible = True
                ddCommodityValue.Visible = True
                lblNewCustomerPartNoLabel.Visible = True
                txtNewCustomerPartNoValue.Visible = True
                ' ''ibtnGetNewCustomerPartNo.Visible = True
                ' ''ibtnGetOriginalCustomerPartNo.Visible = True
                lblNewDesignLevelLabel.Visible = True
                txtNewDesignLevelValue.Visible = True
                lblOriginalCustomerPartNoLabel.Visible = True
                txtOriginalCustomerPartNoValue.Visible = True
                'lblOriginalDesignLevelLabel.Visible = True
                'txtOriginalDesignLevelValue.Visible = True
                gvTopLevelInfo.Visible = True

                ' ''ibtnGetNewCustomerPartNo.Visible = Not ViewState("isApproved")
                ' ''ibtnGetOriginalCustomerPartNo.Visible = Not ViewState("isApproved")

            Else ' show all if NOT a Finished Good, Semi-Finished Good, nor Raw Material
                lblNewPartNoLabel.Visible = True
                txtNewPartNoValue.Visible = True
                lblNewPartRevisionLabel.Visible = True
                txtNewPartRevisionValue.Visible = True
                lblOriginalPartNoLabel.Visible = True
                txtOriginalPartNoValue.Visible = True
                lblOriginalPartRevisionLabel.Visible = True
                txtOriginalPartRevisionValue.Visible = True
                lblPurchasedGoodLabel.Visible = True
                ddPurchasedGoodValue.Visible = True
                'iBtnGetNewPartNo.Visible = True
                'iBtnOriginalPartNo.Visible = True

                ' ''ibtnGetNewCustomerPartNo.Visible = Not ViewState("isApproved")
                ' ''ibtnGetOriginalCustomerPartNo.Visible = Not ViewState("isApproved")
                iBtnGetNewPartNo.Visible = Not ViewState("isApproved")
                iBtnOriginalPartNo.Visible = Not ViewState("isApproved")
                'ibtnGetOriginalCustomerPartNo.Visible = True

                lblCommodityLabel.Visible = True
                ddCommodityValue.Visible = True
                lblNewCustomerPartNoLabel.Visible = True
                txtNewCustomerPartNoValue.Visible = True
                'ibtnGetNewCustomerPartNo.Visible = True
                lblNewDesignLevelLabel.Visible = True
                txtNewDesignLevelValue.Visible = True
                lblOriginalCustomerPartNoLabel.Visible = True
                txtOriginalCustomerPartNoValue.Visible = True
                'lblOriginalDesignLevelLabel.Visible = True
                'txtOriginalDesignLevelValue.Visible = True
                gvTopLevelInfo.Visible = True

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub EnableControls()

        Try
            Dim dsDrawing As DataSet
            Dim dsRFD As DataSet

            Dim iRFDNo As Integer = 0

            accReplicationActivity.Visible = False
            accCostCustomerProgram.Visible = False
            accCostCalculations.Visible = False
            accCostTotals.Visible = False

            btnPreviewCostSheet.Visible = False
            btnPreviewDieLayout.Visible = False

            btnPreApprovalNotification.Visible = False
            btnPostApprovalNotification.Visible = False
            btnEdit.Visible = False

            lblCostSheetIDLabel.Visible = Not ViewState("isRestricted")
            lblCostSheetIDValue.Visible = Not ViewState("isRestricted")
            lblCostSheetStatusLabel.Visible = Not ViewState("isRestricted")
            lblCostSheetStatusMarker.Visible = Not ViewState("isRestricted")
            ddCostSheetStatusValue.Visible = Not ViewState("isRestricted")
            lblPreviousCostSheetIDLabel.Visible = False

            hlnkPreviousCostSheetIDValue.Visible = False
            lblQuoteDateLabel.Visible = Not ViewState("isRestricted")
            txtQuoteDateValue.Visible = Not ViewState("isRestricted")
            lblApprovedDateLabel.Visible = False
            lblApprovedDateValue.Visible = False

            lblRFDNoLabel.Visible = Not ViewState("isRestricted")
            txtRFDNoValue.Visible = Not ViewState("isRestricted")
            hlnkRFD.Visible = False

            lblECINoLabel.Visible = Not ViewState("isRestricted")
            txtECINoValue.Visible = Not ViewState("isRestricted")
            lblUGNFacilityLabel.Visible = Not ViewState("isRestricted")
            lblUGNFacilityMarker.Visible = Not ViewState("isRestricted")
            ddUGNFacilityValue.Visible = Not ViewState("isRestricted")

            lblOldModelLabel.Visible = False
            lblOldModelValue.Visible = False
            lblOldMakeLabel.Visible = False
            lblOldMakeValue.Visible = False
            lblOldPartNoLabel.Visible = False
            lblOldPartNoValue.Visible = False
            lblOldFinishedGoodPartNoLabel.Visible = False
            lblOldFinishedGoodPartNoValue.Visible = False
            lblOldOriginalPartNoLabel.Visible = False
            lblOldOriginalPartNoValue.Visible = False
            lblDesignmationTypeLabel.Visible = Not ViewState("isRestricted")
            ddDesignationTypeValue.Visible = Not ViewState("isRestricted")
            lblNewPartNoLabel.Visible = False
            txtNewPartNoValue.Visible = False
            lblNewPartRevisionLabel.Visible = False
            txtNewPartRevisionValue.Visible = False
            lblOriginalPartRevisionLabel.Visible = False
            txtOriginalPartRevisionValue.Visible = False
            lblOriginalPartNoLabel.Visible = False
            txtOriginalPartNoValue.Visible = False

            lblCommodityLabel.Visible = False
            ddCommodityValue.Visible = False
            lblNewCustomerPartNoLabel.Visible = False
            txtNewCustomerPartNoValue.Visible = False
            lblNewDesignLevelLabel.Visible = False
            txtNewDesignLevelValue.Visible = False
            lblNewPartNameLabel.Visible = False
            txtNewPartNameValue.Visible = False
            lblNewDrawingNoLabel.Visible = Not ViewState("isRestricted")
            txtNewDrawingNoValue.Visible = Not ViewState("isRestricted")
            lblOriginalCustomerPartNoLabel.Visible = False
            txtOriginalCustomerPartNoValue.Visible = False
            lblOriginalDesignLevelLabel.Visible = False
            txtOriginalDesignLevelValue.Visible = False
            lblPurchasedGoodLabel.Visible = False
            ddPurchasedGoodValue.Visible = False

            gvReplicatedTo.Visible = False
            gvTopLevelInfo.Visible = False
            gvAssumptions.Visible = False
            gvAssumptionsApproval.Visible = False

            'lblMakeLabel.Visible = False
            'ddMakeValue.Visible = False
            'lblProgramLabel.Visible = False
            'lblProgramMarker.Visible = False
            'ddProgramValue.Visible = False
            'lblYearLabel.Visible = False
            'lblYearMarker.Visible = False
            'ddYearValue.Visible = False
            'lblCustomerLabel.Visible = False
            'ddCustomerValue.Visible = False
            tblMakes.Visible = False

            gvCustomerProgram.Visible = Not ViewState("isRestricted")

            lblNotesValue.Visible = Not ViewState("isRestricted")
            txtNotesValue.Visible = Not ViewState("isRestricted")

            menuCostSheetTopTabs.Visible = Not ViewState("isRestricted")
            menuCostSheetBottomTabs.Visible = Not ViewState("isRestricted")
            mvBuildCostSheet.Visible = Not ViewState("isRestricted")
            tblCostSheetTotals.Visible = Not ViewState("isRestricted")

            imgQuoteDateValue.Visible = False
            iBtnGetRFDinfo.Visible = False
            iBtnSearchRFD.Visible = False
            iBtnGetDrawingInfo.Visible = False
            iBtnCopyDrawingInfo.Visible = False

            ' ''ibtnGetNewCustomerPartNo.Visible = False
            iBtnGetNewPartNo.Visible = False
            iBtnOriginalPartNo.Visible = False
            ' ''ibtnGetOriginalCustomerPartNo.Visible = False

            lblPartSpecificationsFormulaLabel.Visible = False
            ddPartSpecificationsFormulaValue.Visible = False

            lblPartSpecificationsProcessLabel.Visible = False
            ddPartSpecificationsProcessValue.Visible = False

            lblPartSpecificationsPiecesPerCycleLabel.Visible = False
            txtPartSpecificationsPiecesPerCycleValue.Visible = False

            lblPartSpecificationsIsDiecutLabel.Visible = False
            cbPartSpecificationsIsDiecutValue.Visible = False

            lblPartSpecificationsPiecesCaughtTogetherLabel.Visible = False
            txtPartSpecificationsPiecesCaughtTogetherValue.Visible = False

            lblPartSpecificationsThicknessLabel.Visible = False
            txtPartSpecificationsThicknessValue.Visible = False
            ddPartSpecificationsThicknessUnits.Visible = False

            lblPartSpecificationsIsSideBySideLabel.Visible = False
            cbPartSpecificationsIsSideBySideValue.Visible = False

            lblPartSpecificationsIsCompletedOfflineLabel.Visible = False
            cbPartSpecificationsIsCompletedOfflineValue.Visible = False

            lblPartSpecificationsCalculatedAreaLabel.Visible = False
            txtPartSpecificationsCalculatedAreaValue.Visible = False
            ddPartSpecificationsCalculatedAreaUnits.Visible = False

            lblPartSpecificationsOffLineRateLabel.Visible = False
            txtPartSpecificationsOffLineRateValue.Visible = False

            lblPartSpecificationsChangedAreaLabel.Visible = False
            txtPartSpecificationsChangedAreaValue.Visible = False
            ddPartSpecificationsChangedAreaUnits.Visible = False

            lblPartSpecificationsNumberOfHolesLabel.Visible = False
            txtPartSpecificationsNumberOfHolesValue.Visible = False

            lblPartSpecificationsDieLayoutWidthLabel.Visible = False
            txtPartSpecificationsDieLayoutWidthValue.Visible = False
            ddPartSpecificationsDieLayoutWidthUnits.Visible = False

            lblPartSpecificationsPartWidthLabel.Visible = False
            txtPartSpecificationsPartWidthValue.Visible = False
            ddPartSpecificationsPartWidthUnits.Visible = False

            lblPartSpecificationsDieLayoutTravelLabel.Visible = False
            txtPartSpecificationsDieLayoutTravelValue.Visible = False
            ddPartSpecificationsDieLayoutTravelUnits.Visible = False

            lblPartSpecificationsSpecificGravityLabel.Visible = False
            txtPartSpecificationsSpecificGravityValue.Visible = False
            ddPartSpecificationsSpecificGravityUnits.Visible = False

            lblPartSpecificationsWeightPerAreaLabel.Visible = False
            txtPartSpecificationsWeightPerAreaValue.Visible = False
            ddPartSpecificationsWeightPerAreaUnits.Visible = False

            lblPartSpecificationsPartLengthLabel.Visible = False
            txtPartSpecificationsPartLengthValue.Visible = False
            ddPartSpecificationsPartLengthUnits.Visible = False

            lblPartSpecificationsConfigurationFactorLabel.Visible = False
            txtPartSpecificationsConfigurationFactorValue.Visible = False
            lblPartSpecificationsConfigurationFactorPercentageValue.Visible = False

            lblPartSpecificationsRepackMaterialLabel.Visible = False
            txtPartSpecificationsRepackMaterialValue.Visible = False
            lblPartSpecificationsApproxWeightLabel.Visible = False
            txtPartSpecificationsApproxWeightValue.Visible = False

            lblPartSpecificationsProductionRateLabel.Visible = False
            txtPartSpecificationsProductionRateValue.Visible = False

            txtPartSpecificationsNumberofCarriersLabel.Visible = False
            txtPartSpecificationsNumberOfCarriersValue.Visible = False

            lblPartSpecificationsFoamLabel.Visible = False
            txtPartSpecificationsFoamValue.Visible = False

            gvAdditionalOfflineRate.Visible = False
            gvCapital.Visible = False
            gvLabor.Visible = False
            gvMaterial.Visible = False
            gvMiscCost.Visible = False
            gvOverhead.Visible = False
            gvPackaging.Visible = False
            gvProductionLimit.Visible = False

            If ViewState("isRestricted") = False And ViewState("isAdmin") = True Then
                If cbQuickQuote.Checked = True Then
                    gvAssumptions.Visible = True
                    gvAssumptionsApproval.Visible = True
                End If

                If txtQuoteDateValue.Text.Trim = "" Then
                    txtQuoteDateValue.Text = Today.Date
                End If

                If lblApprovedDateValue.Text.Trim <> "" Then
                    lblApprovedDateLabel.Visible = True
                    lblApprovedDateValue.Visible = True
                End If

                If txtRFDNoValue.Text.Trim <> "" Then
                    iRFDNo = CType(txtRFDNoValue.Text.Trim, Integer)

                    If iRFDNo > 0 Then
                        dsRFD = RFDModule.GetRFD(iRFDNo)

                        If commonFunctions.CheckDataSet(dsRFD) = True Then
                            hlnkRFD.Visible = True
                            hlnkRFD.NavigateUrl = "~/RFD/RFD_Detail.aspx?RFDNo=" & txtRFDNoValue.Text.Trim
                        End If

                    End If
                End If

                If lblOldModelValue.Text.Trim <> "" Then
                    lblOldModelLabel.Visible = True
                    lblOldModelValue.Visible = True
                End If

                If lblOldMakeValue.Text.Trim <> "" Then
                    lblOldMakeLabel.Visible = True
                    lblOldMakeValue.Visible = True
                End If

                If lblOldPartNoValue.Text.Trim <> "" Then
                    lblOldPartNoLabel.Visible = True
                    lblOldPartNoValue.Visible = True
                End If

                If lblOldFinishedGoodPartNoValue.Text.Trim <> "" Then
                    lblOldFinishedGoodPartNoLabel.Visible = True
                    lblOldFinishedGoodPartNoValue.Visible = True
                End If

                If lblOldOriginalPartNoValue.Text.Trim <> "" Then
                    lblOldOriginalPartNoLabel.Visible = True
                    lblOldOriginalPartNoValue.Visible = True
                End If

                CheckDesignationType()

                'lblMakeLabel.Visible = True
                'ddMakeValue.Visible = True
                'lblProgramLabel.Visible = True
                'lblProgramMarker.Visible = True
                'ddProgramValue.Visible = True
                'lblYearLabel.Visible = True
                'lblYearMarker.Visible = True
                'ddYearValue.Visible = True
                'lblCustomerLabel.Visible = True
                'ddCustomerValue.Visible = True
                'tblMakes.Visible = True

                btnAddToCustomerProgram.Visible = True

                'show for all templates
                lblPartSpecificationsFormulaLabel.Visible = True
                ddPartSpecificationsFormulaValue.Visible = True

                lblPartSpecificationsProcessLabel.Visible = True
                ddPartSpecificationsProcessValue.Visible = True

                If ViewState("Formula_TemplateID") <> 12 And ViewState("Formula_TemplateID") <> 13 Then
                    lblPartSpecificationsPiecesPerCycleLabel.Visible = True
                    txtPartSpecificationsPiecesPerCycleValue.Visible = True

                    lblPartSpecificationsIsDiecutLabel.Visible = True
                    cbPartSpecificationsIsDiecutValue.Visible = True

                    lblPartSpecificationsPiecesCaughtTogetherLabel.Visible = True
                    txtPartSpecificationsPiecesCaughtTogetherValue.Visible = True

                    lblPartSpecificationsThicknessLabel.Visible = True
                    txtPartSpecificationsThicknessValue.Visible = True
                    ddPartSpecificationsThicknessUnits.Visible = True

                    lblPartSpecificationsIsSideBySideLabel.Visible = True
                    cbPartSpecificationsIsSideBySideValue.Visible = True

                    lblPartSpecificationsIsCompletedOfflineLabel.Visible = True
                    cbPartSpecificationsIsCompletedOfflineValue.Visible = True

                    lblPartSpecificationsCalculatedAreaLabel.Visible = True
                    txtPartSpecificationsCalculatedAreaValue.Visible = True
                    ddPartSpecificationsCalculatedAreaUnits.Visible = True

                    lblPartSpecificationsOffLineRateLabel.Visible = True
                    txtPartSpecificationsOffLineRateValue.Visible = True

                    lblPartSpecificationsChangedAreaLabel.Visible = True
                    txtPartSpecificationsChangedAreaValue.Visible = True
                    ddPartSpecificationsChangedAreaUnits.Visible = True

                    lblPartSpecificationsNumberOfHolesLabel.Visible = True
                    txtPartSpecificationsNumberOfHolesValue.Visible = True

                    lblPartSpecificationsDieLayoutWidthLabel.Visible = True
                    txtPartSpecificationsDieLayoutWidthValue.Visible = True
                    ddPartSpecificationsDieLayoutWidthUnits.Visible = True

                    'lblPartSpecificationsSpecificGravityLabel.Visible = True
                    'txtPartSpecificationsSpecificGravityValue.Visible = True
                    'ddPartSpecificationsSpecificGravityUnits.Visible = True

                    'lblPartSpecificationsWeightPerAreaLabel.Visible = True
                    'txtPartSpecificationsWeightPerAreaValue.Visible = True
                    'ddPartSpecificationsWeightPerAreaUnits.Visible = True

                    'lblPartSpecificationsWeightPerAreaLabel.Visible = ViewState("Formula_isFleeceType")
                    'txtPartSpecificationsWeightPerAreaValue.Visible = ViewState("Formula_isFleeceType")
                    'ddPartSpecificationsWeightPerAreaUnits.Visible = ViewState("Formula_isFleeceType")

                    'lblPartSpecificationsSpecificGravityLabel.Visible = Not ViewState("Formula_isFleeceType")
                    'txtPartSpecificationsSpecificGravityValue.Visible = Not ViewState("Formula_isFleeceType")
                    'ddPartSpecificationsSpecificGravityUnits.Visible = Not ViewState("Formula_isFleeceType")

                    If ViewState("Formula_SpecificGravity") > 0 Or ViewState("Formula_isFleeceType") = False Then
                        lblPartSpecificationsSpecificGravityLabel.Visible = True
                        txtPartSpecificationsSpecificGravityValue.Visible = True
                        ddPartSpecificationsSpecificGravityUnits.Visible = True

                        lblPartSpecificationsWeightPerAreaLabel.Visible = False
                        txtPartSpecificationsWeightPerAreaValue.Visible = False
                        ddPartSpecificationsWeightPerAreaUnits.Visible = False
                    Else
                        lblPartSpecificationsSpecificGravityLabel.Visible = False
                        txtPartSpecificationsSpecificGravityValue.Visible = False
                        ddPartSpecificationsSpecificGravityUnits.Visible = False

                        lblPartSpecificationsWeightPerAreaLabel.Visible = True
                        txtPartSpecificationsWeightPerAreaValue.Visible = True
                        ddPartSpecificationsWeightPerAreaUnits.Visible = True
                    End If

                    lblPartSpecificationsPartWidthLabel.Visible = True
                    txtPartSpecificationsPartWidthValue.Visible = True
                    ddPartSpecificationsPartWidthUnits.Visible = True

                    lblPartSpecificationsDieLayoutTravelLabel.Visible = True
                    txtPartSpecificationsDieLayoutTravelValue.Visible = True
                    ddPartSpecificationsDieLayoutTravelUnits.Visible = True

                    lblPartSpecificationsPartLengthLabel.Visible = True
                    txtPartSpecificationsPartLengthValue.Visible = True
                    ddPartSpecificationsPartLengthUnits.Visible = True

                    lblPartSpecificationsConfigurationFactorLabel.Visible = True
                    txtPartSpecificationsConfigurationFactorValue.Visible = True
                    lblPartSpecificationsConfigurationFactorPercentageValue.Visible = True

                    If ViewState("Formula_TemplateID") = 7 Then
                        lblPartSpecificationsRepackMaterialLabel.Visible = True
                        txtPartSpecificationsRepackMaterialValue.Visible = True
                        lblPartSpecificationsApproxWeightLabel.Visible = True
                        txtPartSpecificationsApproxWeightValue.Visible = True
                    End If

                End If

                If ViewState("Formula_TemplateID") = 12 Then
                    lblPartSpecificationsProductionRateLabel.Visible = True
                    txtPartSpecificationsProductionRateValue.Visible = True
                End If

                If ViewState("Formula_TemplateID") = 13 Then
                    txtPartSpecificationsNumberofCarriersLabel.Visible = True
                    txtPartSpecificationsNumberOfCarriersValue.Visible = True

                    lblPartSpecificationsFoamLabel.Visible = True
                    txtPartSpecificationsFoamValue.Visible = True

                    lblPartSpecificationsProductionRateLabel.Visible = True
                    txtPartSpecificationsProductionRateValue.Visible = True
                End If

                'disable controls once the cost sheet is approved
                ddCostSheetStatusValue.Enabled = Not ViewState("isApproved")
                txtQuoteDateValue.Enabled = Not ViewState("isApproved")
                txtRFDNoValue.Enabled = Not ViewState("isApproved")
                txtECINoValue.Enabled = Not ViewState("isApproved")
                ddUGNFacilityValue.Enabled = Not ViewState("isApproved")
                'ddMakeValue.Enabled = Not ViewState("isApproved")
                'ddProgramValue.Enabled = Not ViewState("isApproved")
                'ddYearValue.Enabled = Not ViewState("isApproved")
                'ddCustomerValue.Enabled = Not ViewState("isApproved")
                tblMakes.Visible = Not ViewState("isApproved")

                ddDesignationTypeValue.Enabled = Not ViewState("isApproved")
                ddCommodityValue.Enabled = Not ViewState("isApproved")
                txtNewCustomerPartNoValue.Enabled = Not ViewState("isApproved")
                txtNewDesignLevelValue.Enabled = Not ViewState("isApproved")
                txtNewPartNameValue.Enabled = Not ViewState("isApproved")
                txtNewDrawingNoValue.Enabled = Not ViewState("isApproved")
                lblNewPartNameLabel.Visible = Not ViewState("isApproved")
                txtNewPartNameValue.Visible = Not ViewState("isApproved")
                txtOriginalCustomerPartNoValue.Enabled = Not ViewState("isApproved")
                txtOriginalDesignLevelValue.Enabled = Not ViewState("isApproved")
                ddPurchasedGoodValue.Enabled = Not ViewState("isApproved")
                txtNewPartNoValue.Enabled = Not ViewState("isApproved")
                txtNewPartRevisionValue.Enabled = Not ViewState("isApproved")
                txtOriginalPartNoValue.Enabled = Not ViewState("isApproved")
                txtOriginalPartRevisionValue.Enabled = Not ViewState("isApproved")
                txtNotesValue.Enabled = Not ViewState("isApproved")

                btnAddToCustomerProgram.Visible = Not ViewState("isApproved")
                btnSave.Visible = Not ViewState("isApproved")
                btnSaveLowerPage.Visible = Not ViewState("isApproved")

                imgQuoteDateValue.Visible = Not ViewState("isApproved")
                iBtnGetRFDinfo.Visible = Not ViewState("isApproved")
                iBtnSearchRFD.Visible = Not ViewState("isApproved")
                iBtnGetDrawingInfo.Visible = Not ViewState("isApproved")

                If ViewState("CostSheetID") > 0 Then

                    hlnkNewDrawingNo.Visible = False
                    If txtNewDrawingNoValue.Text.Trim <> "" Then
                        dsDrawing = PEModule.GetDrawing(txtNewDrawingNoValue.Text.Trim)
                        If commonFunctions.CheckDataSet(dsDrawing) = True Then
                            iBtnCopyDrawingInfo.Visible = Not ViewState("isApproved")
                            hlnkNewDrawingNo.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & txtNewDrawingNoValue.Text.Trim
                            hlnkNewDrawingNo.Visible = True
                        End If
                    End If
                End If

                gvTopLevelInfo.Columns(0).Visible = Not ViewState("isApproved")
                gvTopLevelInfo.Columns(gvTopLevelInfo.Columns.Count - 1).Visible = Not ViewState("isApproved")
                If gvTopLevelInfo.FooterRow IsNot Nothing Then
                    gvTopLevelInfo.FooterRow.Visible = Not ViewState("isApproved")
                End If

                If ViewState("CostSheetID") > 0 Then
                    gvReplicatedFrom.Visible = ViewState("isAdmin")
                    gvReplicatedTo.Visible = ViewState("isAdmin")

                    accReplicationActivity.Visible = True
                    accCostCustomerProgram.Visible = True
                    accCostCalculations.Visible = True
                    accCostTotals.Visible = True

                    btnEdit.Visible = ViewState("isApproved")
                    btnEdit.Visible = ViewState("isApproved")

                    gvAdditionalOfflineRate.Visible = True
                    gvCapital.Visible = True
                    gvLabor.Visible = True
                    gvMaterial.Visible = True
                    gvMiscCost.Visible = True
                    gvOverhead.Visible = True
                    gvPackaging.Visible = True
                    gvProductionLimit.Visible = True

                    btnDelete.Visible = Not ViewState("isApproved")

                    If ddPartSpecificationsFormulaValue.SelectedIndex > 0 Then
                        btnPreviewDieLayout.Visible = cbPartSpecificationsIsDiecutValue.Checked

                        btnPreviewCostSheet.Visible = True

                        btnPreApprovalNotification.Visible = True
                        btnPostApprovalNotification.Visible = True
                        btnCalculate.Visible = Not ViewState("isApproved")
                        btnUpdateTotals.Visible = Not ViewState("isApproved")

                        btnCopy.Visible = ViewState("isAdmin")
                        rbCopyInformationType.Visible = ViewState("isAdmin")
                        rbCostStatusType.Visible = ViewState("isAdmin")
                    End If

                    ddPartSpecificationsFormulaValue.Enabled = Not ViewState("isApproved")
                    'ddPartSpecificationsDepartmentValue.Enabled = Not ViewState("isApproved")
                    ddPartSpecificationsProcessValue.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsPiecesPerCycleValue.Enabled = Not ViewState("isApproved")
                    cbPartSpecificationsIsDiecutValue.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsPiecesCaughtTogetherValue.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsThicknessValue.Enabled = Not ViewState("isApproved")
                    'ddPartSpecificationsThicknessUnits.Enabled = Not ViewState("isApproved")
                    cbPartSpecificationsIsSideBySideValue.Enabled = Not ViewState("isApproved")
                    cbPartSpecificationsIsCompletedOfflineValue.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsCalculatedAreaValue.Enabled = Not ViewState("isApproved")
                    'ddPartSpecificationsCalculatedAreaUnits.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsOffLineRateValue.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsChangedAreaValue.Enabled = Not ViewState("isApproved")
                    'ddPartSpecificationsChangedAreaUnits.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsNumberOfHolesValue.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsDieLayoutWidthValue.Enabled = Not ViewState("isApproved")
                    'ddPartSpecificationsDieLayoutWidthUnits.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsPartWidthValue.Enabled = Not ViewState("isApproved")
                    'ddPartSpecificationsPartWidthUnits.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsDieLayoutTravelValue.Enabled = Not ViewState("isApproved")
                    'ddPartSpecificationsDieLayoutTravelUnits.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsSpecificGravityValue.Enabled = Not ViewState("isApproved")
                    'ddPartSpecificationsSpecificGravityUnits.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsWeightPerAreaValue.Enabled = Not ViewState("isApproved")
                    'ddPartSpecificationsWeightPerAreaUnits.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsPartLengthValue.Enabled = Not ViewState("isApproved")
                    'ddPartSpecificationsPartLengthUnits.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsConfigurationFactorValue.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsRepackMaterialValue.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsApproxWeightValue.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsProductionRateValue.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsNumberofCarriersLabel.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsNumberOfCarriersValue.Enabled = Not ViewState("isApproved")
                    txtPartSpecificationsFoamValue.Enabled = Not ViewState("isApproved")

                    txtProductionRatesMaxMixCapacityValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesMaxFormingRateValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesCatchingAbilityValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesLineSpeedLimitationValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesCatchPercentValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesCoatingFactorValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesWeightPerAreaValue.Enabled = Not ViewState("isApproved")

                    txtProductionRatesOfflineSpecificSheetsUpValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesOfflineSpecificBlankCodeValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesOfflineSpecificQuotedPressCyclesValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesOfflineSpecificQuotedOfflineRatesValue.Enabled = Not ViewState("isApproved")
                    'txtProductionRatesOfflineSpecificCrewSizeValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesOfflineSpecificPiecesManHourValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesOfflineSpecificPercentRecycleValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresMaxPiecesQuotedValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresMaxPiecesMaximumValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresPressCyclesQuotedValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresPressCyclesMaximumValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresLineSpeedQuotedValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresLineSpeedMaximumValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresMixCapacityQuotedValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresMixCapacityMaximumValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresNetFormingRateQuotedValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresNetFormingRateMaximumValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresRecycleRateQuotedValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresRecycleRateMaximumValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresPartWeightQuotedValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresPartWeightMaximumValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresCoatingWeightQuotedValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresCoatingWeightMaximumValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresTotalWeightQuotedValue.Enabled = Not ViewState("isApproved")
                    txtProductionRatesFinalFiguresTotalWeightMaximumValue.Enabled = Not ViewState("isApproved")

                    ddQuotedInfoAccountManager.Enabled = Not ViewState("isApproved")
                    txtQuotedInfoStandardCostFactor.Enabled = Not ViewState("isApproved")
                    txtQuotedInfoPiecesPerYear.Enabled = Not ViewState("isApproved")
                    txtQuotedInfoComments.Enabled = Not ViewState("isApproved")

                    uploadImage.Visible = Not ViewState("isApproved")
                    uploadImage.Enabled = Not ViewState("isApproved")

                    btnSaveUploadDrawingPartSketchImage.Visible = Not ViewState("isApproved")
                    btnSaveUploadDrawingPartSketchImage.Enabled = Not ViewState("isApproved")

                    btnDeleteDrawingPartSketchImage.Visible = Not ViewState("isApproved")
                    btnDeleteDrawingPartSketchImage.Enabled = Not ViewState("isApproved")

                    txtDrawingPartSketchMemo.Enabled = Not ViewState("isApproved")
                    lnkShowLargerSketchImage.Enabled = True

                    ddCompositePartSpecFormula.Enabled = Not ViewState("isApproved")
                    txtCompositePartSpecPartThicknessValue.Enabled = Not ViewState("isApproved")
                    txtCompositePartSpecPartSpecificGravityValue.Enabled = Not ViewState("isApproved")
                    txtCompositePartSpecPartAreaValue.Enabled = Not ViewState("isApproved")
                    txtCompositePartSpecRSSWeightValue.Enabled = Not ViewState("isApproved")
                    txtCompositePartSpecAntiBlockCoatingValue.Enabled = Not ViewState("isApproved")
                    txtCompositePartSpecHotMeldAdhesiveValue.Enabled = Not ViewState("isApproved")

                    ddMoldedBarrierFormula.Enabled = Not ViewState("isApproved")
                    txtMoldedBarrierApproximateLengthValue.Enabled = Not ViewState("isApproved")
                    txtMoldedBarrierApproximateWidthValue.Enabled = Not ViewState("isApproved")
                    txtMoldedBarrierApproximateThicknessValue.Enabled = Not ViewState("isApproved")
                    txtMoldedBarrierBlankAreaValue.Enabled = Not ViewState("isApproved")
                    txtMoldedBarrierSpecificGravityValue.Enabled = Not ViewState("isApproved")
                    txtMoldedBarrierWeightPerAreaValue.Enabled = Not ViewState("isApproved")
                    txtMoldedBarrierBlankWeightValue.Enabled = Not ViewState("isApproved")
                    txtMoldedBarrierAntiBlockCoatingValue.Enabled = Not ViewState("isApproved")
                    txtMoldedBarrierTotalWeightValue.Enabled = Not ViewState("isApproved")

                    txtMaterialCostTotalValue.Enabled = Not ViewState("isApproved")
                    txtPackagingCostTotalValue.Enabled = Not ViewState("isApproved")
                    txtLaborCostTotalValue.Enabled = Not ViewState("isApproved")
                    txtOverheadCostTotalValue.Enabled = Not ViewState("isApproved")
                    txtCapitalCostTotalValue.Enabled = Not ViewState("isApproved")
                    txtMiscCostTotalValue.Enabled = Not ViewState("isApproved")
                    txtOverallCostTotalValue.Enabled = Not ViewState("isApproved")

                    gvAdditionalOfflineRate.Columns(gvAdditionalOfflineRate.Columns.Count - 1).Visible = Not ViewState("isApproved")
                    btnRemoveAdditionalOfflineRate.Visible = Not ViewState("isApproved")
                    If gvAdditionalOfflineRate.FooterRow IsNot Nothing Then
                        gvAdditionalOfflineRate.FooterRow.Visible = Not ViewState("isApproved")
                    End If

                    gvCapital.Columns(gvCapital.Columns.Count - 1).Visible = Not ViewState("isApproved")
                    btnRemoveCapital.Visible = Not ViewState("isApproved")
                    If gvCapital.FooterRow IsNot Nothing Then
                        gvCapital.FooterRow.Visible = Not ViewState("isApproved")
                    End If

                    gvLabor.Columns(gvLabor.Columns.Count - 1).Visible = Not ViewState("isApproved")
                    btnRemoveLabor.Visible = Not ViewState("isApproved")
                    If gvLabor.FooterRow IsNot Nothing Then
                        gvLabor.FooterRow.Visible = Not ViewState("isApproved")
                    End If

                    gvMaterial.Columns(gvMaterial.Columns.Count - 1).Visible = Not ViewState("isApproved")
                    btnRemoveMaterials.Visible = Not ViewState("isApproved")
                    If gvMaterial.FooterRow IsNot Nothing Then
                        gvMaterial.FooterRow.Visible = Not ViewState("isApproved")
                    End If

                    gvMiscCost.Columns(gvMiscCost.Columns.Count - 1).Visible = Not ViewState("isApproved")
                    btnRemoveMiscCost.Visible = Not ViewState("isApproved")
                    If gvMiscCost.FooterRow IsNot Nothing Then
                        gvMiscCost.FooterRow.Visible = Not ViewState("isApproved")
                    End If

                    gvOverhead.Columns(gvOverhead.Columns.Count - 1).Visible = Not ViewState("isApproved")
                    btnRemoveOverhead.Visible = Not ViewState("isApproved")
                    If gvOverhead.FooterRow IsNot Nothing Then
                        gvOverhead.FooterRow.Visible = Not ViewState("isApproved")
                    End If

                    gvPackaging.Columns(gvPackaging.Columns.Count - 1).Visible = Not ViewState("isApproved")
                    btnRemovePackaging.Visible = Not ViewState("isApproved")
                    If gvPackaging.FooterRow IsNot Nothing Then
                        gvPackaging.FooterRow.Visible = Not ViewState("isApproved")
                    End If

                    'if values deviate from the formula, put a yellow background
                    If txtPartSpecificationsSpecificGravityValue.Text <> "" Then
                        If CType(txtPartSpecificationsSpecificGravityValue.Text, Double) <> ViewState("Formula_SpecificGravity") Then
                            txtPartSpecificationsSpecificGravityValue.BackColor = Color.Yellow
                        End If
                    End If

                    If txtPartSpecificationsWeightPerAreaValue.Text <> "" Then
                        If CType(txtPartSpecificationsWeightPerAreaValue.Text, Double) <> ViewState("Formula_WeightPerArea") Then
                            txtPartSpecificationsWeightPerAreaValue.BackColor = Color.Yellow
                        End If
                    End If

                    'If ddPartSpecificationsDepartmentValue.SelectedIndex > 0 Then
                    '    If CType(ddPartSpecificationsDepartmentValue.SelectedValue, Integer) <> ViewState("Formula_DepartmentID") Then
                    '        ddPartSpecificationsDepartmentValue.BackColor = Color.Yellow
                    '    End If
                    'End If

                    If cbPartSpecificationsIsDiecutValue.Checked <> ViewState("Formula_isDiecut") Then
                        cbPartSpecificationsIsDiecutValue.BackColor = Color.Yellow
                    End If

                    If ddPartSpecificationsProcessValue.SelectedIndex > 0 Then
                        If CType(ddPartSpecificationsProcessValue.SelectedValue, Integer) <> ViewState("Formula_ProcessID") Then
                            ddPartSpecificationsProcessValue.BackColor = Color.Yellow
                        End If
                    End If
                End If
            Else
                lblMessage.Text += "You do not have access to this information. Please contact the Costing Manager."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ' ''bind existing data to drop down Make
            'ds = commonFunctions.GetProgramMake()
            'If commonFunctions.CheckDataset(ds) = True Then
            '    ddMakeValue.DataSource = ds
            '    ddMakeValue.DataTextField = ds.Tables(0).Columns("Make").ColumnName.ToString()
            '    ddMakeValue.DataValueField = ds.Tables(0).Columns("Make").ColumnName
            '    ddMakeValue.DataBind()
            '    ddMakeValue.Items.Insert(0, "")
            'End If

            ' ''bind existing data to drop down Program 
            'ds = commonFunctions.GetProgram("", "", "")
            'If commonFunctions.CheckDataset(ds) = True Then
            '    ddProgramValue.DataSource = ds
            '    ddProgramValue.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
            '    ddProgramValue.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName
            '    ddProgramValue.DataBind()
            '    ddProgramValue.Items.Insert(0, "")
            'End If

            ''bind existing data to drop down Year 
            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
            End If

            ' ''ds = commonFunctions.GetOEMManufacturer("")
            ' ''If commonFunctions.CheckDataSet(ds) = True Then
            ' ''    ddCustomer.DataSource = ds
            ' ''    ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
            ' ''    ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
            ' ''    ddCustomer.DataBind()
            ' ''    ddCustomer.Items.Insert(0, "")
            ' ''End If

            ds = commonFunctions.GetDesignationType()
            If (commonFunctions.CheckDataSet(ds) = True) Then
                ddDesignationTypeValue.DataSource = ds
                ddDesignationTypeValue.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName
                ddDesignationTypeValue.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName
                ddDesignationTypeValue.DataBind()
                ddDesignationTypeValue.Items.Insert(0, "")
            End If

            'bind existing data to Part Specifications Formula DropDown and Composite Part Specification Formula Dropdown         
            ds = CostingModule.GetFormula(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddPartSpecificationsFormulaValue.DataSource = ds
                ddPartSpecificationsFormulaValue.DataTextField = ds.Tables(0).Columns("ddFormulaName").ColumnName
                ddPartSpecificationsFormulaValue.DataValueField = ds.Tables(0).Columns("FormulaID").ColumnName
                ddPartSpecificationsFormulaValue.DataBind()
                ddPartSpecificationsFormulaValue.Items.Insert(0, "")

                ddCompositePartSpecFormula.DataSource = ds
                ddCompositePartSpecFormula.DataTextField = ds.Tables(0).Columns("ddFormulaName").ColumnName
                ddCompositePartSpecFormula.DataValueField = ds.Tables(0).Columns("FormulaID").ColumnName
                ddCompositePartSpecFormula.DataBind()
                ddCompositePartSpecFormula.Items.Insert(0, "")

                ddMoldedBarrierFormula.DataSource = ds
                ddMoldedBarrierFormula.DataTextField = ds.Tables(0).Columns("ddFormulaName").ColumnName
                ddMoldedBarrierFormula.DataValueField = ds.Tables(0).Columns("FormulaID").ColumnName
                ddMoldedBarrierFormula.DataBind()
                ddMoldedBarrierFormula.Items.Insert(0, "")

            End If

            ''bind existing data to drop down Commodity 
            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCommodityValue.DataSource = ds
                ddCommodityValue.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
                ddCommodityValue.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddCommodityValue.DataBind()
                ddCommodityValue.Items.Insert(0, "")
                ddCommodityValue.SelectedIndex = 0
            End If

            '' ''bind existing data to drop down PartFamily 
            ''ds = commonFunctions.GetSubFamily(0)
            ''If ds IsNot Nothing Then
            ''    If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
            ''        ddSubFamily.DataSource = ds
            ''        ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
            ''        ddSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
            ''        ddSubFamily.DataBind()
            ''        ddSubFamily.Items.Insert(0, "")
            ''    End If
            ''End If


            'bind existing data to drop down Density 
            ds = commonFunctions.GetPurchasedGood("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddPurchasedGoodValue.DataSource = ds
                ddPurchasedGoodValue.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
                ddPurchasedGoodValue.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddPurchasedGoodValue.DataBind()
                ddPurchasedGoodValue.Items.Insert(0, "")
            End If

            'bind existing data to drop down Density 
            ds = CostingModule.GetProcess(0, "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddPartSpecificationsProcessValue.DataSource = ds
                ddPartSpecificationsProcessValue.DataTextField = ds.Tables(0).Columns("ddProcessName").ColumnName.ToString()
                ddPartSpecificationsProcessValue.DataValueField = ds.Tables(0).Columns("ProcessID").ColumnName
                ddPartSpecificationsProcessValue.DataBind()
                ddPartSpecificationsProcessValue.Items.Insert(0, "")
            End If

            'bind existing team member list for Account Managers who created cost sheets
            ds = CostingModule.GetCostSheetAccountManagers()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddQuotedInfoAccountManager.DataSource = ds
                ddQuotedInfoAccountManager.DataTextField = ds.Tables(0).Columns("ddAccountManagerFullName").ColumnName
                ddQuotedInfoAccountManager.DataValueField = ds.Tables(0).Columns("AccountManagerID").ColumnName
                ddQuotedInfoAccountManager.DataBind()
                ddQuotedInfoAccountManager.Items.Insert(0, "")
            End If

            'bind UGN Facility
            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddUGNFacilityValue.DataSource = ds
                ddUGNFacilityValue.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacilityValue.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacilityValue.DataBind()
                ddUGNFacilityValue.Items.Insert(0, "")
            End If

            'bind units to multiple unit dropdown boxes
            ds = commonFunctions.GetUnit(0, "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCompositePartSpecAntiBlockCoatingUnits.DataSource = ds
                ddCompositePartSpecAntiBlockCoatingUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddCompositePartSpecAntiBlockCoatingUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddCompositePartSpecAntiBlockCoatingUnits.DataBind()
                ddCompositePartSpecAntiBlockCoatingUnits.Items.Insert(0, "")

                ddCompositePartSpecHotMeldAdhesiveUnits.DataSource = ds
                ddCompositePartSpecHotMeldAdhesiveUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddCompositePartSpecHotMeldAdhesiveUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddCompositePartSpecHotMeldAdhesiveUnits.DataBind()
                ddCompositePartSpecHotMeldAdhesiveUnits.Items.Insert(0, "")

                ddCompositePartSpecPartAreaUnits.DataSource = ds
                ddCompositePartSpecPartAreaUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddCompositePartSpecPartAreaUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddCompositePartSpecPartAreaUnits.DataBind()
                ddCompositePartSpecPartAreaUnits.Items.Insert(0, "")

                ddCompositePartSpecPartSpecificGravityUnits.DataSource = ds
                ddCompositePartSpecPartSpecificGravityUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddCompositePartSpecPartSpecificGravityUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddCompositePartSpecPartSpecificGravityUnits.DataBind()
                ddCompositePartSpecPartSpecificGravityUnits.Items.Insert(0, "")

                ddCompositePartSpecPartThicknessUnits.DataSource = ds
                ddCompositePartSpecPartThicknessUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddCompositePartSpecPartThicknessUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddCompositePartSpecPartThicknessUnits.DataBind()
                ddCompositePartSpecPartThicknessUnits.Items.Insert(0, "")

                ddCompositePartSpecRSSWeightUnits.DataSource = ds
                ddCompositePartSpecRSSWeightUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddCompositePartSpecRSSWeightUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddCompositePartSpecRSSWeightUnits.DataBind()
                ddCompositePartSpecRSSWeightUnits.Items.Insert(0, "")

                ddMoldedBarrierAntiBlockCoatingUnits.DataSource = ds
                ddMoldedBarrierAntiBlockCoatingUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMoldedBarrierAntiBlockCoatingUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMoldedBarrierAntiBlockCoatingUnits.DataBind()
                ddMoldedBarrierAntiBlockCoatingUnits.Items.Insert(0, "")

                ddMoldedBarrierApproximateLengthUnits.DataSource = ds
                ddMoldedBarrierApproximateLengthUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMoldedBarrierApproximateLengthUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMoldedBarrierApproximateLengthUnits.DataBind()
                ddMoldedBarrierApproximateLengthUnits.Items.Insert(0, "")

                ddMoldedBarrierApproximateThicknessUnits.DataSource = ds
                ddMoldedBarrierApproximateThicknessUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMoldedBarrierApproximateThicknessUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMoldedBarrierApproximateThicknessUnits.DataBind()
                ddMoldedBarrierApproximateThicknessUnits.Items.Insert(0, "")

                ddMoldedBarrierApproximateWidthUnits.DataSource = ds
                ddMoldedBarrierApproximateWidthUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMoldedBarrierApproximateWidthUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMoldedBarrierApproximateWidthUnits.DataBind()
                ddMoldedBarrierApproximateWidthUnits.Items.Insert(0, "")

                ddMoldedBarrierBlankAreaUnits.DataSource = ds
                ddMoldedBarrierBlankAreaUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMoldedBarrierBlankAreaUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMoldedBarrierBlankAreaUnits.DataBind()
                ddMoldedBarrierBlankAreaUnits.Items.Insert(0, "")

                ddMoldedBarrierBlankWeightUnits.DataSource = ds
                ddMoldedBarrierBlankWeightUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMoldedBarrierBlankWeightUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMoldedBarrierBlankWeightUnits.DataBind()
                ddMoldedBarrierBlankWeightUnits.Items.Insert(0, "")

                ddMoldedBarrierSpecificGravityUnits.DataSource = ds
                ddMoldedBarrierSpecificGravityUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMoldedBarrierSpecificGravityUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMoldedBarrierSpecificGravityUnits.DataBind()
                ddMoldedBarrierSpecificGravityUnits.Items.Insert(0, "")

                ddMoldedBarrierTotalWeightUnits.DataSource = ds
                ddMoldedBarrierTotalWeightUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMoldedBarrierTotalWeightUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMoldedBarrierTotalWeightUnits.DataBind()
                ddMoldedBarrierTotalWeightUnits.Items.Insert(0, "")

                ddMoldedBarrierWeightPerAreaUnits.DataSource = ds
                ddMoldedBarrierWeightPerAreaUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMoldedBarrierWeightPerAreaUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMoldedBarrierWeightPerAreaUnits.DataBind()
                ddMoldedBarrierWeightPerAreaUnits.Items.Insert(0, "")

                ddPartSpecificationsApproxWeightUnits.DataSource = ds
                ddPartSpecificationsApproxWeightUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddPartSpecificationsApproxWeightUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddPartSpecificationsApproxWeightUnits.DataBind()
                ddPartSpecificationsApproxWeightUnits.Items.Insert(0, "")

                ddPartSpecificationsCalculatedAreaUnits.DataSource = ds
                ddPartSpecificationsCalculatedAreaUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddPartSpecificationsCalculatedAreaUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddPartSpecificationsCalculatedAreaUnits.DataBind()
                ddPartSpecificationsCalculatedAreaUnits.Items.Insert(0, "")

                ddPartSpecificationsChangedAreaUnits.DataSource = ds
                ddPartSpecificationsChangedAreaUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddPartSpecificationsChangedAreaUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddPartSpecificationsChangedAreaUnits.DataBind()
                ddPartSpecificationsChangedAreaUnits.Items.Insert(0, "")

                ddPartSpecificationsDieLayoutTravelUnits.DataSource = ds
                ddPartSpecificationsDieLayoutTravelUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddPartSpecificationsDieLayoutTravelUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddPartSpecificationsDieLayoutTravelUnits.DataBind()
                ddPartSpecificationsDieLayoutTravelUnits.Items.Insert(0, "")

                ddPartSpecificationsDieLayoutWidthUnits.DataSource = ds
                ddPartSpecificationsDieLayoutWidthUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddPartSpecificationsDieLayoutWidthUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddPartSpecificationsDieLayoutWidthUnits.DataBind()
                ddPartSpecificationsDieLayoutWidthUnits.Items.Insert(0, "")

                ddPartSpecificationsFoamUnits.DataSource = ds
                ddPartSpecificationsFoamUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddPartSpecificationsFoamUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddPartSpecificationsFoamUnits.DataBind()
                ddPartSpecificationsFoamUnits.Items.Insert(0, "")

                ddPartSpecificationsPartLengthUnits.DataSource = ds
                ddPartSpecificationsPartLengthUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddPartSpecificationsPartLengthUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddPartSpecificationsPartLengthUnits.DataBind()
                ddPartSpecificationsPartLengthUnits.Items.Insert(0, "")

                ddPartSpecificationsPartWidthUnits.DataSource = ds
                ddPartSpecificationsPartWidthUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddPartSpecificationsPartWidthUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddPartSpecificationsPartWidthUnits.DataBind()
                ddPartSpecificationsPartWidthUnits.Items.Insert(0, "")

                ddPartSpecificationsSpecificGravityUnits.DataSource = ds
                ddPartSpecificationsSpecificGravityUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddPartSpecificationsSpecificGravityUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddPartSpecificationsSpecificGravityUnits.DataBind()
                ddPartSpecificationsSpecificGravityUnits.Items.Insert(0, "")

                ddPartSpecificationsThicknessUnits.DataSource = ds
                ddPartSpecificationsThicknessUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddPartSpecificationsThicknessUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddPartSpecificationsThicknessUnits.DataBind()
                ddPartSpecificationsThicknessUnits.Items.Insert(0, "")

                ddPartSpecificationsWeightPerAreaUnits.DataSource = ds
                ddPartSpecificationsWeightPerAreaUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddPartSpecificationsWeightPerAreaUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddPartSpecificationsWeightPerAreaUnits.DataBind()
                ddPartSpecificationsWeightPerAreaUnits.Items.Insert(0, "")

                ddProductionRatesMaxMixCapacityUnits.DataSource = ds
                ddProductionRatesMaxMixCapacityUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesMaxMixCapacityUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesMaxMixCapacityUnits.DataBind()
                ddProductionRatesMaxMixCapacityUnits.Items.Insert(0, "")

                ddProductionRatesMaxFormingRateUnits.DataSource = ds
                ddProductionRatesMaxFormingRateUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesMaxFormingRateUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesMaxFormingRateUnits.DataBind()
                ddProductionRatesMaxFormingRateUnits.Items.Insert(0, "")

                ddProductionRatesWeightPerAreaUnits.DataSource = ds
                ddProductionRatesWeightPerAreaUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesWeightPerAreaUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesWeightPerAreaUnits.DataBind()
                ddProductionRatesWeightPerAreaUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresCoatingWeightMaximumUnits.DataSource = ds
                ddProductionRatesFinalFiguresCoatingWeightMaximumUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresCoatingWeightMaximumUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresCoatingWeightMaximumUnits.DataBind()
                ddProductionRatesFinalFiguresCoatingWeightMaximumUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.DataSource = ds
                ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.DataBind()
                ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresLineSpeedMaximumUnits.DataSource = ds
                ddProductionRatesFinalFiguresLineSpeedMaximumUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresLineSpeedMaximumUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresLineSpeedMaximumUnits.DataBind()
                ddProductionRatesFinalFiguresLineSpeedMaximumUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresLineSpeedQuotedUnits.DataSource = ds
                ddProductionRatesFinalFiguresLineSpeedQuotedUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresLineSpeedQuotedUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresLineSpeedQuotedUnits.DataBind()
                ddProductionRatesFinalFiguresLineSpeedQuotedUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresMaxPiecesMaximumUnits.DataSource = ds
                ddProductionRatesFinalFiguresMaxPiecesMaximumUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresMaxPiecesMaximumUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresMaxPiecesMaximumUnits.DataBind()
                ddProductionRatesFinalFiguresMaxPiecesMaximumUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresMaxPiecesQuotedUnits.DataSource = ds
                ddProductionRatesFinalFiguresMaxPiecesQuotedUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresMaxPiecesQuotedUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresMaxPiecesQuotedUnits.DataBind()
                ddProductionRatesFinalFiguresMaxPiecesQuotedUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresMixCapacityMaximumUnits.DataSource = ds
                ddProductionRatesFinalFiguresMixCapacityMaximumUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresMixCapacityMaximumUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresMixCapacityMaximumUnits.DataBind()
                ddProductionRatesFinalFiguresMixCapacityMaximumUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresMixCapacityQuotedUnits.DataSource = ds
                ddProductionRatesFinalFiguresMixCapacityQuotedUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresMixCapacityQuotedUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresMixCapacityQuotedUnits.DataBind()
                ddProductionRatesFinalFiguresMixCapacityQuotedUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.DataSource = ds
                ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.DataBind()
                ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.DataSource = ds
                ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.DataBind()
                ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresPartWeightMaximumUnits.DataSource = ds
                ddProductionRatesFinalFiguresPartWeightMaximumUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresPartWeightMaximumUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresPartWeightMaximumUnits.DataBind()
                ddProductionRatesFinalFiguresPartWeightMaximumUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresPartWeightQuotedUnits.DataSource = ds
                ddProductionRatesFinalFiguresPartWeightQuotedUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresPartWeightQuotedUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresPartWeightQuotedUnits.DataBind()
                ddProductionRatesFinalFiguresPartWeightQuotedUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresPressCyclesMaximumUnits.DataSource = ds
                ddProductionRatesFinalFiguresPressCyclesMaximumUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresPressCyclesMaximumUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresPressCyclesMaximumUnits.DataBind()
                ddProductionRatesFinalFiguresPressCyclesMaximumUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresPressCyclesQuotedUnits.DataSource = ds
                ddProductionRatesFinalFiguresPressCyclesQuotedUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresPressCyclesQuotedUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresPressCyclesQuotedUnits.DataBind()
                ddProductionRatesFinalFiguresPressCyclesQuotedUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresRecycleRateMaximumUnits.DataSource = ds
                ddProductionRatesFinalFiguresRecycleRateMaximumUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresRecycleRateMaximumUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresRecycleRateMaximumUnits.DataBind()
                ddProductionRatesFinalFiguresRecycleRateMaximumUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresRecycleRateQuotedUnits.DataSource = ds
                ddProductionRatesFinalFiguresRecycleRateQuotedUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresRecycleRateQuotedUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresRecycleRateQuotedUnits.DataBind()
                ddProductionRatesFinalFiguresRecycleRateQuotedUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresTotalWeightMaximumUnits.DataSource = ds
                ddProductionRatesFinalFiguresTotalWeightMaximumUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresTotalWeightMaximumUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresTotalWeightMaximumUnits.DataBind()
                ddProductionRatesFinalFiguresTotalWeightMaximumUnits.Items.Insert(0, "")

                ddProductionRatesFinalFiguresTotalWeightQuotedUnits.DataSource = ds
                ddProductionRatesFinalFiguresTotalWeightQuotedUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddProductionRatesFinalFiguresTotalWeightQuotedUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddProductionRatesFinalFiguresTotalWeightQuotedUnits.DataBind()
                ddProductionRatesFinalFiguresTotalWeightQuotedUnits.Items.Insert(0, "")

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Private Sub BindCostSheetTotal()

        Try

            Dim ds As DataSet

            Dim dTempCapitalTotal As Double = 0

            Dim dTempCostSheetSubTotalWOScrap As Double = 0
            Dim dTempCostSheetSubTotal As Double = 0

            Dim dFixedCostTotal As Double = 0

            Dim dMinPriceMargin As Double = 0
            Dim dMinSellingPrice As Double = 0

            Dim dPriceVariableMarginPercent As Double = 0
            Dim dPriceVariableMarginInclDeprPercent As Double = 0

            Dim dPriceVariableMarginDollar As Double = 0
            Dim dPriceVariableMarginInclDeprDollar As Double = 0

            Dim dPriceGrossMarginPercent As Double = 0
            Dim dPriceGrossMarginDollar As Double = 0

            Dim dTempLaborTotalWOScrap As Double = 0
            Dim dTempLaborTotal As Double = 0

            Dim dTempMaterialTotalWOScrap As Double = 0
            Dim dTempMaterialTotal As Double = 0

            Dim dTempMiscCostTotal As Double = 0

            Dim dTempOverallCostTotalWOScrap As Double = 0
            Dim dTempOverallCostTotal As Double = 0

            Dim dTempOverheadTotalWOScrap As Double = 0
            Dim dTempOverheadTotal As Double = 0

            Dim dTempPackagingTotalWOScrap As Double = 0
            Dim dTempPackagingTotal As Double = 0

            Dim dTempScrapTotal As Double = 0

            Dim dTempSGATotal As Double = 0

            Dim dTempOverheadCostFixedRateTotalWOScrap As Double = 0
            Dim dTempOverheadCostVariableRateTotalWOScrap As Double = 0

            Dim dVariableCostTotal As Double = 0

            Dim dTempOverallTotal As Double = 0

            Dim strUGNFacility As String = ""

            ds = CostingModule.GetCostSheetTotal(ViewState("CostSheetID"))

            If commonFunctions.CheckDataSet(ds) = True Then

                txtMaterialCostTotalWOScrapValue.Text = ""
                If ds.Tables(0).Rows(0).Item("MaterialCostTotalWOScrap") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialCostTotalWOScrap") > 0 Then
                        txtMaterialCostTotalWOScrapValue.Text = ds.Tables(0).Rows(0).Item("MaterialCostTotalWOScrap")
                        dTempMaterialTotalWOScrap = ds.Tables(0).Rows(0).Item("MaterialCostTotalWOScrap")
                    End If
                End If

                txtMaterialCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("MaterialCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialCostTotal") > 0 Then
                        txtMaterialCostTotalValue.Text = ds.Tables(0).Rows(0).Item("MaterialCostTotal")
                        dTempMaterialTotal = ds.Tables(0).Rows(0).Item("MaterialCostTotal")
                    End If
                End If

                txtPackagingCostTotalWOScrapValue.Text = ""
                If ds.Tables(0).Rows(0).Item("PackagingCostTotalWOScrap") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PackagingCostTotalWOScrap") > 0 Then
                        txtPackagingCostTotalWOScrapValue.Text = Format(ds.Tables(0).Rows(0).Item("PackagingCostTotalWOScrap"), "###0.0000")
                        dTempPackagingTotalWOScrap = ds.Tables(0).Rows(0).Item("PackagingCostTotalWOScrap")
                    End If
                End If

                txtPackagingCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("PackagingCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PackagingCostTotal") > 0 Then
                        txtPackagingCostTotalValue.Text = ds.Tables(0).Rows(0).Item("PackagingCostTotal")
                        dTempPackagingTotal = ds.Tables(0).Rows(0).Item("PackagingCostTotal")
                    End If
                End If

                lblMaterialAndPackagingCostTotalValue.Text = ""
                If txtMaterialCostTotalValue.Text.Trim <> "" Or txtPackagingCostTotalValue.Text.Trim <> "" Then
                    lblMaterialAndPackagingCostTotalWOScrapValue.Text = Format(dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap, "###0.0000")
                    lblMaterialAndPackagingCostTotalValue.Text = Format(dTempMaterialTotal + dTempPackagingTotal, "###0.0000")
                End If

                txtLaborCostTotalWOScrapValue.Text = ""
                If ds.Tables(0).Rows(0).Item("LaborCostTotalWOScrap") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("LaborCostTotalWOScrap") > 0 Then
                        txtLaborCostTotalWOScrapValue.Text = Format(ds.Tables(0).Rows(0).Item("LaborCostTotalWOScrap"), "###0.0000")
                        dTempLaborTotalWOScrap = ds.Tables(0).Rows(0).Item("LaborCostTotalWOScrap")
                    End If
                End If

                txtLaborCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("LaborCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("LaborCostTotal") > 0 Then
                        txtLaborCostTotalValue.Text = Format(ds.Tables(0).Rows(0).Item("LaborCostTotal"), "###0.0000")
                        dTempLaborTotal = ds.Tables(0).Rows(0).Item("LaborCostTotal")
                    End If
                End If

                txtOverheadCostTotalWOScrapValue.Text = ""
                If ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrap") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrap") > 0 Then
                        txtOverheadCostTotalWOScrapValue.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrap"), "###0.0000")
                        dTempOverheadTotalWOScrap = ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrap")
                    End If
                End If

                txtOverheadCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("OverheadCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCostTotal") > 0 Then
                        txtOverheadCostTotalValue.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCostTotal"), "###0.0000")
                        dTempOverheadTotal = ds.Tables(0).Rows(0).Item("OverheadCostTotal")
                    End If
                End If

                dTempScrapTotal = (dTempMaterialTotal + dTempPackagingTotal + dTempLaborTotal + dTempOverheadTotal) - (dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap + dTempLaborTotalWOScrap + dTempOverheadTotalWOScrap)

                lblScrapCostTotalValue.Text = ""
                If dTempScrapTotal <> 0 Then
                    lblScrapCostTotalValue.Text = Format(dTempScrapTotal, "###0.0000")
                End If

                lblCapitalCostTotalWOScrapValue.Text = ""
                txtCapitalCostTotalValue.Text = ""
                lblCapitalCostTotalValue2.Text = ""
                If ds.Tables(0).Rows(0).Item("CapitalCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CapitalCostTotal") <> 0 Then
                        txtCapitalCostTotalValue.Text = Format(ds.Tables(0).Rows(0).Item("CapitalCostTotal"), "###0.0000")
                        lblCapitalCostTotalValue2.Text = Format(ds.Tables(0).Rows(0).Item("CapitalCostTotal"), "###0.0000")
                        dTempCapitalTotal = ds.Tables(0).Rows(0).Item("CapitalCostTotal")
                    End If
                End If

                lblManufacturingCostTotalValue.Text = ""
                lblManufacturingCostTotalWOScrapValue.Text = ""

                dTempCostSheetSubTotalWOScrap = dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap + dTempLaborTotalWOScrap + dTempOverheadTotalWOScrap + dTempScrapTotal + dTempCapitalTotal
                dTempCostSheetSubTotal = dTempMaterialTotal + dTempPackagingTotal + dTempLaborTotal + dTempOverheadTotal + dTempCapitalTotal

                If dTempCostSheetSubTotalWOScrap <> 0 Then
                    lblManufacturingCostTotalWOScrapValue.Text = Format(dTempCostSheetSubTotalWOScrap, "###0.0000")
                End If

                If dTempCostSheetSubTotal <> 0 Then
                    lblManufacturingCostTotalValue.Text = Format(dTempCostSheetSubTotal, "###0.0000")
                End If

                txtMiscCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("MiscCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MiscCostTotal") <> 0 Then
                        txtMiscCostTotalValue.Text = Format(ds.Tables(0).Rows(0).Item("MiscCostTotal"), "###0.0000")
                        dTempMiscCostTotal = ds.Tables(0).Rows(0).Item("MiscCostTotal")
                    End If
                End If

                lblSGACostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("SGACostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("SGACostTotal") <> 0 Then
                        lblSGACostTotalValue.Text = Format(ds.Tables(0).Rows(0).Item("SGACostTotal"), "###0.0000")
                        dTempSGATotal = ds.Tables(0).Rows(0).Item("SGACostTotal")
                    End If
                End If

                txtOverallCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("OverallCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverallCostTotal") <> 0 Then
                        txtOverallCostTotalValue.Text = Format(ds.Tables(0).Rows(0).Item("OverallCostTotal"), "###0.0000")
                    End If
                End If


                lblFixedCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("FixedCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("FixedCostTotal") <> 0 Then
                        dFixedCostTotal = ds.Tables(0).Rows(0).Item("FixedCostTotal")
                    End If
                End If

                If dFixedCostTotal <> 0 Then
                    lblFixedCostTotalValue.Text = dFixedCostTotal
                End If

                lblVariableCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("VariableCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("VariableCostTotal") <> 0 Then
                        dVariableCostTotal = ds.Tables(0).Rows(0).Item("VariableCostTotal")
                    End If
                End If

                If dVariableCostTotal <> 0 Then
                    lblVariableCostTotalValue.Text = dVariableCostTotal
                End If

                lblMinimumSellingPriceValue.Text = ""
                If ds.Tables(0).Rows(0).Item("MinSellingPrice") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MinSellingPrice") <> 0 Then
                        dMinSellingPrice = ds.Tables(0).Rows(0).Item("MinSellingPrice")
                    End If
                End If

                If dMinSellingPrice <> 0 Then
                    lblMinimumSellingPriceValue.Text = dMinSellingPrice
                End If

                lblPriceVariableMarginPercentValue.Text = ""
                If ds.Tables(0).Rows(0).Item("PriceVariableMarginPercent") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PriceVariableMarginPercent") <> 0 Then
                        dPriceVariableMarginPercent = ds.Tables(0).Rows(0).Item("PriceVariableMarginPercent")
                    End If
                End If

                If dPriceVariableMarginPercent <> 0 Then
                    lblPriceVariableMarginPercentValue.Text = Format(dPriceVariableMarginPercent * 100, "###0.0")
                End If

                lblPriceVariableMarginDollarValue.Text = ""
                If ds.Tables(0).Rows(0).Item("PriceVariableMarginDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PriceVariableMarginDollar") <> 0 Then
                        dPriceVariableMarginDollar = ds.Tables(0).Rows(0).Item("PriceVariableMarginDollar")
                    End If
                End If

                If dPriceVariableMarginDollar <> 0 Then
                    lblPriceVariableMarginDollarValue.Text = Format(dPriceVariableMarginDollar, "###0.0000")
                End If

                lblPriceVariableMarginInclDeprPercentValue.Text = ""
                If ds.Tables(0).Rows(0).Item("PriceVariableMarginInclDeprPercent") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PriceVariableMarginInclDeprPercent") <> 0 Then
                        dPriceVariableMarginInclDeprPercent = ds.Tables(0).Rows(0).Item("PriceVariableMarginInclDeprPercent")
                    End If
                End If

                If dPriceVariableMarginInclDeprPercent <> 0 Then
                    lblPriceVariableMarginInclDeprPercentValue.Text = Format(dPriceVariableMarginInclDeprPercent * 100, "###0.0")
                End If

                lblPriceVariableMarginInclDeprDollarValue.Text = ""
                If ds.Tables(0).Rows(0).Item("PriceVariableMarginInclDeprDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PriceVariableMarginInclDeprDollar") <> 0 Then
                        dPriceVariableMarginInclDeprDollar = ds.Tables(0).Rows(0).Item("PriceVariableMarginInclDeprDollar")
                    End If
                End If

                If dPriceVariableMarginInclDeprDollar <> 0 Then
                    lblPriceVariableMarginInclDeprDollarValue.Text = Format(dPriceVariableMarginInclDeprDollar, "###0.0000")
                End If

                lblPriceGrossMarginPercentValue.Text = ""
                If ds.Tables(0).Rows(0).Item("PriceGrossMarginPercent") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PriceGrossMarginPercent") <> 0 Then
                        dPriceGrossMarginPercent = ds.Tables(0).Rows(0).Item("PriceGrossMarginPercent")
                    End If
                End If

                If dPriceGrossMarginPercent <> 0 Then
                    lblPriceGrossMarginPercentValue.Text = Format(dPriceGrossMarginPercent * 100, "###0.0")
                End If

                lblPriceGrossMarginPercentValue.ForeColor = Color.Black
                If dPriceGrossMarginPercent < 0 Then
                    lblPriceGrossMarginPercentValue.ForeColor = Color.Red
                End If

                lblPriceGrossMarginDollarValue.Text = ""
                If ds.Tables(0).Rows(0).Item("PriceGrossMarginDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PriceGrossMarginDollar") <> 0 Then
                        dPriceGrossMarginDollar = ds.Tables(0).Rows(0).Item("PriceGrossMarginDollar")
                    End If
                End If

                If dPriceGrossMarginDollar <> 0 Then
                    lblPriceGrossMarginDollarValue.Text = Format(dPriceGrossMarginDollar, "###0.0000")
                End If

                lblPriceGrossMarginDollarValue.ForeColor = Color.Black
                If dPriceGrossMarginDollar < 0 Then
                    lblPriceGrossMarginDollarValue.ForeColor = Color.Red
                End If

            End If

            lblPriceVariableMarginPercentTargetValue.Text = ""
            lblPriceVariableMarginPercentTargetValue.Visible = False

            'get Minimum Price Margin Percent
            If ddUGNFacilityValue.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacilityValue.SelectedValue

                ds = CostingModule.GetCostSheetPriceMargin(strUGNFacility)
                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("MinPriceMargin") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("MinPriceMargin") <> 0 Then
                            dMinPriceMargin = ds.Tables(0).Rows(0).Item("MinPriceMargin")
                            lblPriceVariableMarginPercentTargetValue.Text = Format(dMinPriceMargin * 100, "###0.0")
                            lblPriceVariableMarginPercentTargetValue.Visible = ViewState("isAdmin")
                        End If
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text
    End Sub

    Private Sub BindData()

        Try
            Dim ds As DataSet
            Dim dsPostApproval As DataSet

            If ViewState("CostSheetID") > 0 Then
                If ViewState("isRestricted") = False Then

                    'bind existing CostSheet data to for top level cost sheet info                                        
                    ds = CostingModule.GetCostSheet(ViewState("CostSheetID"))

                    If commonFunctions.CheckDataSet(ds) = True Then

                        If ds.Tables(0).Rows(0).Item("Obsolete") = False Then

                            lblCostSheetIDValue.Text = ViewState("CostSheetID")
                            ddCostSheetStatusValue.SelectedValue = ds.Tables(0).Rows(0).Item("CostSheetStatus").ToString

                            lblPreviousCostSheetIDLabel.Visible = False
                            'do not show label if value does not exist
                            If ds.Tables(0).Rows(0).Item("PreviousCostSheetID") IsNot System.DBNull.Value Then
                                If ds.Tables(0).Rows(0).Item("PreviousCostSheetID") > 0 Then
                                    hlnkPreviousCostSheetIDValue.Text = ds.Tables(0).Rows(0).Item("PreviousCostSheetID").ToString
                                    hlnkPreviousCostSheetIDValue.NavigateUrl = "~/Costing/Cost_Sheet_Detail.aspx?CostSheetID=" & ds.Tables(0).Rows(0).Item("PreviousCostSheetID").ToString
                                    'lblPreviousCostSheetIDLabel.Visible = True                                  
                                End If
                            End If

                            txtQuoteDateValue.Text = ds.Tables(0).Rows(0).Item("QuoteDate").ToString

                            If ds.Tables(0).Rows(0).Item("ApprovedDate").ToString <> "" Then
                                lblApprovedDateValue.Text = ds.Tables(0).Rows(0).Item("ApprovedDate").ToString
                                lblApprovedDateLabel.Visible = True

                                'check if team members have been notified of approved cost sheet
                                dsPostApproval = CostingModule.GetCostSheetPostApprovalList(ViewState("CostSheetID"), True, True)
                                If commonFunctions.CheckDataSet(dsPostApproval) = True Then
                                    ViewState("isApproved") = True
                                End If

                            Else
                                lblApprovedDateLabel.Visible = False
                            End If

                            If ds.Tables(0).Rows(0).Item("RFDNo") IsNot System.DBNull.Value Then
                                If ds.Tables(0).Rows(0).Item("RFDNo") > 0 Then
                                    txtRFDNoValue.Text = ds.Tables(0).Rows(0).Item("RFDNo")
                                End If
                            End If

                            If ds.Tables(0).Rows(0).Item("ECINo") IsNot System.DBNull.Value Then
                                If ds.Tables(0).Rows(0).Item("ECINo") > 0 Then
                                    txtECINoValue.Text = ds.Tables(0).Rows(0).Item("ECINo")
                                End If
                            End If

                            If ds.Tables(0).Rows(0).Item("UGNFacility").ToString <> "UT" Then
                                If ds.Tables(0).Rows(0).Item("UGNFacility").ToString = "UQ" Then 'old chicago facility
                                    Dim liListItem As New ListItem
                                    liListItem.Text = "** Chicago"
                                    liListItem.Value = "UQ"
                                    ddUGNFacilityValue.Items.Add(liListItem)
                                End If
                                ddUGNFacilityValue.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString
                            End If


                            'If ds.Tables(0).Rows(0).Item("ProgramID") IsNot System.DBNull.Value Then
                            '    If ds.Tables(0).Rows(0).Item("ProgramID") > 0 Then
                            '        ddProgramValue.SelectedValue = ds.Tables(0).Rows(0).Item("ProgramID")
                            '    End If
                            'End If

                            lblOldModelValue.Text = ds.Tables(0).Rows(0).Item("OldModel").ToString
                            lblOldMakeValue.Text = ds.Tables(0).Rows(0).Item("OldMake").ToString
                            lblOldPartNoValue.Text = ds.Tables(0).Rows(0).Item("OldPartNo").ToString
                            lblOldFinishedGoodPartNoValue.Text = ds.Tables(0).Rows(0).Item("OldFinishedGoodPartNo").ToString
                            lblOldOriginalPartNoValue.Text = ds.Tables(0).Rows(0).Item("OldOriginalPartNo").ToString
                            cbQuickQuote.Checked = ds.Tables(0).Rows(0).Item("QuickQuote").ToString

                            If ds.Tables(0).Rows(0).Item("DesignationType").ToString <> "" Then
                                ddDesignationTypeValue.SelectedValue = ds.Tables(0).Rows(0).Item("DesignationType").ToString
                            End If

                            txtNewPartNoValue.Text = ds.Tables(0).Rows(0).Item("NewBPCSPartNo").ToString
                            txtNewPartRevisionValue.Text = ds.Tables(0).Rows(0).Item("NewBPCSPartRevision").ToString
                            txtOriginalPartNoValue.Text = ds.Tables(0).Rows(0).Item("OriginalBPCSPartNo").ToString
                            txtOriginalPartRevisionValue.Text = ds.Tables(0).Rows(0).Item("OriginalBPCSPartRevision").ToString

                            If ds.Tables(0).Rows(0).Item("VehicleYear") IsNot System.DBNull.Value Then
                                If ds.Tables(0).Rows(0).Item("VehicleYear") > 0 Then
                                    'If ds.Tables(0).Rows(0).Item("VehicleYear") < 2002 Or ds.Tables(0).Rows(0).Item("VehicleYear") > 2020 Then
                                    '    ddYearValue.Items.Insert(ddYearValue.Items.Count, ds.Tables(0).Rows(0).Item("VehicleYear"))
                                    'End If
                                    'ddYearValue.SelectedValue = ds.Tables(0).Rows(0).Item("VehicleYear")
                                    lblOldYearValue.Text = ds.Tables(0).Rows(0).Item("VehicleYear")
                                    lblOldYearValue.Visible = True
                                    lblOldYearLabel.Visible = True
                                End If
                            End If


                            'If ds.Tables(0).Rows(0).Item("ddCustomerValue") IsNot System.DBNull.Value Then
                            '    ddCustomerValue.SelectedValue = ds.Tables(0).Rows(0).Item("ddCustomerValue").ToString()
                            'End If

                            If ds.Tables(0).Rows(0).Item("CommodityID") IsNot System.DBNull.Value Then
                                If ds.Tables(0).Rows(0).Item("CommodityID") > 0 Then
                                    ddCommodityValue.SelectedValue = ds.Tables(0).Rows(0).Item("CommodityID")
                                End If
                            End If

                            txtNewCustomerPartNoValue.Text = ds.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString
                            txtNewDesignLevelValue.Text = ds.Tables(0).Rows(0).Item("NewDesignLevel").ToString
                            txtNewPartNameValue.Text = ds.Tables(0).Rows(0).Item("NewPartName").ToString
                            txtNewDrawingNoValue.Text = ds.Tables(0).Rows(0).Item("NewDrawingNo").ToString

                            txtOriginalCustomerPartNoValue.Text = ds.Tables(0).Rows(0).Item("OriginalCustomerPartNo").ToString
                            'txtOriginalDesignLevelValue.Text = ds.Tables(0).Rows(0).Item("OriginalDesignLevel").ToString

                            If ds.Tables(0).Rows(0).Item("PurchasedGoodID") IsNot System.DBNull.Value Then
                                If ds.Tables(0).Rows(0).Item("PurchasedGoodID") > 0 Then
                                    ddPurchasedGoodValue.SelectedValue = ds.Tables(0).Rows(0).Item("PurchasedGoodID")
                                End If
                            End If

                            txtNotesValue.Text = ds.Tables(0).Rows(0).Item("Notes").ToString

                            BindCostSheetTotal()

                            'bind existing CostSheet data for Part Specification Tab                    
                            ds = CostingModule.GetCostSheetPartSpecification(ViewState("CostSheetID"))
                            If commonFunctions.CheckDataSet(ds) = True Then

                                If ds.Tables(0).Rows(0).Item("FormulaID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("FormulaID") > 0 Then
                                        ddPartSpecificationsFormulaValue.SelectedValue = ds.Tables(0).Rows(0).Item("FormulaID")

                                        GetFormulaTopLevelDetails(ds.Tables(0).Rows(0).Item("FormulaID"))
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("PiecesPerCycle") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PiecesPerCycle") <> 0 Then
                                        txtPartSpecificationsPiecesPerCycleValue.Text = ds.Tables(0).Rows(0).Item("PiecesPerCycle").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("isDiecut") IsNot System.DBNull.Value Then
                                    cbPartSpecificationsIsDiecutValue.Checked = ds.Tables(0).Rows(0).Item("isDiecut")
                                End If

                                If ds.Tables(0).Rows(0).Item("PiecesCaughtTogether") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PiecesCaughtTogether") <> 0 Then
                                        txtPartSpecificationsPiecesCaughtTogetherValue.Text = ds.Tables(0).Rows(0).Item("PiecesCaughtTogether").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("PartThickness") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartThickness") <> 0 Then
                                        txtPartSpecificationsThicknessValue.Text = ds.Tables(0).Rows(0).Item("PartThickness").ToString
                                    End If
                                End If

                                'default mm
                                ddPartSpecificationsThicknessUnits.SelectedValue = 17
                                If ds.Tables(0).Rows(0).Item("PartThicknessUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartThicknessUnitID") <> 0 Then
                                        ddPartSpecificationsThicknessUnits.SelectedValue = ds.Tables(0).Rows(0).Item("PartThicknessUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("isSideBySide") IsNot System.DBNull.Value Then
                                    cbPartSpecificationsIsSideBySideValue.Checked = ds.Tables(0).Rows(0).Item("isSideBySide")
                                End If

                                If ds.Tables(0).Rows(0).Item("isCompletedOffline") IsNot System.DBNull.Value Then
                                    cbPartSpecificationsIsCompletedOfflineValue.Checked = ds.Tables(0).Rows(0).Item("isCompletedOffline")
                                End If

                                If ds.Tables(0).Rows(0).Item("CalculatedArea") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("CalculatedArea") <> 0 Then
                                        txtPartSpecificationsCalculatedAreaValue.Text = ds.Tables(0).Rows(0).Item("CalculatedArea").ToString
                                    End If
                                End If

                                'default m2
                                ddPartSpecificationsCalculatedAreaUnits.SelectedValue = 14
                                If ds.Tables(0).Rows(0).Item("CalculatedAreaUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("CalculatedAreaUnitID") > 0 Then
                                        ddPartSpecificationsCalculatedAreaUnits.SelectedValue = ds.Tables(0).Rows(0).Item("CalculatedAreaUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("OffLineRate") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("OffLineRate") <> 0 Then
                                        txtPartSpecificationsOffLineRateValue.Text = ds.Tables(0).Rows(0).Item("OffLineRate").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("ChangedArea") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("ChangedArea") <> 0 Then
                                        txtPartSpecificationsChangedAreaValue.Text = ds.Tables(0).Rows(0).Item("ChangedArea").ToString
                                    End If
                                End If

                                'default m2
                                ddPartSpecificationsChangedAreaUnits.SelectedValue = 14
                                If ds.Tables(0).Rows(0).Item("ChangedAreaUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("ChangedAreaUnitID") > 0 Then
                                        ddPartSpecificationsChangedAreaUnits.SelectedValue = ds.Tables(0).Rows(0).Item("ChangedAreaUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("NumberOfHoles") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("NumberOfHoles") <> 0 Then
                                        txtPartSpecificationsNumberOfHolesValue.Text = ds.Tables(0).Rows(0).Item("NumberOfHoles").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("DieLayoutWidth") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("DieLayoutWidth") <> 0 Then
                                        txtPartSpecificationsDieLayoutWidthValue.Text = ds.Tables(0).Rows(0).Item("DieLayoutWidth").ToString
                                    End If
                                End If

                                'default m
                                ddPartSpecificationsDieLayoutWidthUnits.SelectedValue = 18
                                If ds.Tables(0).Rows(0).Item("DieLayoutWidthUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("DieLayoutWidthUnitID") > 0 Then
                                        ddPartSpecificationsDieLayoutWidthUnits.SelectedValue = ds.Tables(0).Rows(0).Item("DieLayoutWidthUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("PartWidth") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartWidth") <> 0 Then
                                        txtPartSpecificationsPartWidthValue.Text = ds.Tables(0).Rows(0).Item("PartWidth").ToString
                                    End If
                                End If

                                'default m
                                ddPartSpecificationsPartWidthUnits.SelectedValue = 18
                                If ds.Tables(0).Rows(0).Item("PartWidthUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartWidthUnitID") > 0 Then
                                        ddPartSpecificationsPartWidthUnits.SelectedValue = 18
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("DieLayoutTravel") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("DieLayoutTravel") <> 0 Then
                                        txtPartSpecificationsDieLayoutTravelValue.Text = ds.Tables(0).Rows(0).Item("DieLayoutTravel").ToString
                                    End If
                                End If

                                'default m
                                ddPartSpecificationsDieLayoutTravelUnits.SelectedValue = 18
                                If ds.Tables(0).Rows(0).Item("DieLayoutTravelUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("DieLayoutTravelUnitID") > 0 Then
                                        ddPartSpecificationsDieLayoutTravelUnits.SelectedValue = ds.Tables(0).Rows(0).Item("DieLayoutTravelUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("PartLength") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartLength") <> 0 Then
                                        txtPartSpecificationsPartLengthValue.Text = ds.Tables(0).Rows(0).Item("PartLength").ToString
                                    End If
                                End If

                                'default m
                                ddPartSpecificationsPartLengthUnits.SelectedValue = 18
                                If ds.Tables(0).Rows(0).Item("PartLengthUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartLengthUnitID") > 0 Then
                                        ddPartSpecificationsPartLengthUnits.SelectedValue = ds.Tables(0).Rows(0).Item("PartLengthUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("WeightPerArea") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("WeightPerArea") <> 0 Then
                                        txtPartSpecificationsWeightPerAreaValue.Text = ds.Tables(0).Rows(0).Item("WeightPerArea").ToString
                                    End If
                                End If

                                'default g/m2
                                ddPartSpecificationsWeightPerAreaUnits.SelectedValue = 15
                                If ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID") > 0 Then
                                        ddPartSpecificationsWeightPerAreaUnits.SelectedValue = ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID")
                                    End If
                                End If

                                'default to 90%
                                txtPartSpecificationsConfigurationFactorValue.Text = "0.90"
                                lblPartSpecificationsConfigurationFactorPercentageValue.Text = "90.00%"
                                If ds.Tables(0).Rows(0).Item("ConfigurationFactor") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("ConfigurationFactor") <> 0 Then
                                        txtPartSpecificationsConfigurationFactorValue.Text = ds.Tables(0).Rows(0).Item("ConfigurationFactor").ToString

                                        lblPartSpecificationsConfigurationFactorPercentageValue.Text = Format(ds.Tables(0).Rows(0).Item("ConfigurationFactor") * 100, "####.00") & "%"
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("SpecificGravity") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("SpecificGravity") <> 0 Then
                                        txtPartSpecificationsSpecificGravityValue.Text = ds.Tables(0).Rows(0).Item("SpecificGravity").ToString
                                    End If
                                End If

                                'default g/cm3
                                ddPartSpecificationsSpecificGravityUnits.SelectedValue = 25
                                If ds.Tables(0).Rows(0).Item("SpecificGravityUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("SpecificGravityUnitID") > 0 Then
                                        ddPartSpecificationsSpecificGravityUnits.SelectedValue = ds.Tables(0).Rows(0).Item("SpecificGravityUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("ProcessID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("ProcessID") > 0 Then
                                        ddPartSpecificationsProcessValue.SelectedValue = ds.Tables(0).Rows(0).Item("ProcessID")
                                    End If
                                End If

                                txtPartSpecificationsRepackMaterialValue.Text = ds.Tables(0).Rows(0).Item("RepackMaterial").ToString

                                If ds.Tables(0).Rows(0).Item("ApproxWeight") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("ApproxWeight") <> 0 Then
                                        txtPartSpecificationsApproxWeightValue.Text = ds.Tables(0).Rows(0).Item("ApproxWeight").ToString
                                    End If
                                End If

                                'default 5
                                ddPartSpecificationsApproxWeightUnits.SelectedValue = 5
                                If ds.Tables(0).Rows(0).Item("ApproxWeightUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("ApproxWeightUnitID") > 0 Then
                                        ddPartSpecificationsApproxWeightUnits.SelectedValue = ds.Tables(0).Rows(0).Item("ApproxWeightUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("ProductionRate") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("ProductionRate") <> 0 Then
                                        txtPartSpecificationsProductionRateValue.Text = ds.Tables(0).Rows(0).Item("ProductionRate").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("NumberOfCarriers") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("NumberOfCarriers") <> 0 Then
                                        txtPartSpecificationsNumberOfCarriersValue.Text = ds.Tables(0).Rows(0).Item("NumberOfCarriers").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Foam") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Foam") <> 0 Then
                                        txtPartSpecificationsFoamValue.Text = ds.Tables(0).Rows(0).Item("Foam").ToString
                                    End If
                                End If

                                'default mm
                                ddPartSpecificationsFoamUnits.SelectedValue = 17
                                If ds.Tables(0).Rows(0).Item("FoamUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("FoamUnitID") > 0 Then
                                        ddPartSpecificationsFoamUnits.SelectedValue = ds.Tables(0).Rows(0).Item("FoamUnitID")
                                    End If
                                End If

                            End If 'end part specification load table ds is not empty

                            ''bind existing CostSheet data for Production Rates Tab (some fields are duplicated in this table. It will be sorted out later because some calculations might allow updates here but not in the other tables.                    
                            ds = CostingModule.GetCostSheetProductionRate(ViewState("CostSheetID"))
                            If commonFunctions.CheckDataSet(ds) = True Then

                                lblProductionRatesMaxFormingRateLabel.Visible = ViewState("Formula_isFleeceType")
                                txtProductionRatesMaxFormingRateValue.Visible = ViewState("Formula_isFleeceType")
                                ddProductionRatesMaxFormingRateUnits.Visible = ViewState("Formula_isFleeceType")

                                lblProductionRatesMaxMixCapacityLabel.Visible = Not ViewState("Formula_isFleeceType")
                                txtProductionRatesMaxMixCapacityValue.Visible = Not ViewState("Formula_isFleeceType")
                                ddProductionRatesMaxMixCapacityUnits.Visible = Not ViewState("Formula_isFleeceType")

                                If ds.Tables(0).Rows(0).Item("MaxMixCapacity") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("MaxMixCapacity") <> 0 Then
                                        txtProductionRatesMaxMixCapacityValue.Text = ds.Tables(0).Rows(0).Item("MaxMixCapacity").ToString
                                    End If
                                End If

                                'default kg/hr
                                ddProductionRatesMaxMixCapacityUnits.SelectedValue = 20
                                If ds.Tables(0).Rows(0).Item("MaxMixCapacityUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("MaxMixCapacityUnitID") > 0 Then
                                        ddProductionRatesMaxMixCapacityUnits.SelectedValue = ds.Tables(0).Rows(0).Item("MaxMixCapacityUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("MaxFormingRate") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("MaxFormingRate") <> 0 Then
                                        txtProductionRatesMaxFormingRateValue.Text = ds.Tables(0).Rows(0).Item("MaxFormingRate").ToString
                                    End If
                                End If

                                'default kg/hr
                                ddProductionRatesMaxFormingRateUnits.SelectedValue = 20
                                If ds.Tables(0).Rows(0).Item("MaxFormingRateUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("MaxFormingRateUnitID") > 0 Then
                                        ddProductionRatesMaxFormingRateUnits.SelectedValue = ds.Tables(0).Rows(0).Item("MaxFormingRateUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("CatchingAbility") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("CatchingAbility") <> 0 Then
                                        txtProductionRatesCatchingAbilityValue.Text = ds.Tables(0).Rows(0).Item("CatchingAbility").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("LineSpeedLimitation") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("LineSpeedLimitation") <> 0 Then
                                        txtProductionRatesLineSpeedLimitationValue.Text = ds.Tables(0).Rows(0).Item("LineSpeedLimitation").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("CatchPercent") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("CatchPercent") <> 0 Then
                                        txtProductionRatesCatchPercentValue.Text = ds.Tables(0).Rows(0).Item("CatchPercent").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("CoatingFactor") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("CoatingFactor") <> 0 Then
                                        txtProductionRatesCoatingFactorValue.Text = ds.Tables(0).Rows(0).Item("CoatingFactor").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("WeightPerArea") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("WeightPerArea") <> 0 Then
                                        txtProductionRatesWeightPerAreaValue.Text = ds.Tables(0).Rows(0).Item("WeightPerArea").ToString
                                    End If
                                End If

                                'default g/m2
                                ddProductionRatesWeightPerAreaUnits.SelectedValue = 15
                                If ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID") > 0 Then
                                        ddProductionRatesWeightPerAreaUnits.SelectedValue = ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Offline_SheetsUp") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Offline_SheetsUp") <> 0 Then
                                        txtProductionRatesOfflineSpecificSheetsUpValue.Text = ds.Tables(0).Rows(0).Item("Offline_SheetsUp").ToString
                                    End If
                                End If

                                txtProductionRatesOfflineSpecificBlankCodeValue.Text = ds.Tables(0).Rows(0).Item("Offline_BlankCode").ToString

                                If ds.Tables(0).Rows(0).Item("Offline_QuotedPressCycles") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Offline_QuotedPressCycles") <> 0 Then
                                        txtProductionRatesOfflineSpecificQuotedPressCyclesValue.Text = ds.Tables(0).Rows(0).Item("Offline_QuotedPressCycles").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Offline_QuotedOfflineRates") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Offline_QuotedOfflineRates") <> 0 Then
                                        txtProductionRatesOfflineSpecificQuotedOfflineRatesValue.Text = ds.Tables(0).Rows(0).Item("Offline_QuotedOfflineRates").ToString
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Offline_PiecesPerManHour") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Offline_PiecesPerManHour") <> 0 Then
                                        txtProductionRatesOfflineSpecificPiecesManHourValue.Text = Format(ds.Tables(0).Rows(0).Item("Offline_PiecesPerManHour"), "####.00")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Offline_PercentRecycle") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Offline_PercentRecycle") <> 0 Then
                                        txtProductionRatesOfflineSpecificPercentRecycleValue.Text = CType(ds.Tables(0).Rows(0).Item("Offline_PercentRecycle"), Double)
                                        lblProductionRatesOfflineSpecificPercentRecycleValuePercent.Text = CType(ds.Tables(0).Rows(0).Item("Offline_PercentRecycle") * 100, Double) & "%"
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Quoted_MaxPieces") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_MaxPieces") <> 0 Then
                                        txtProductionRatesFinalFiguresMaxPiecesQuotedValue.Text = Format(ds.Tables(0).Rows(0).Item("Quoted_MaxPieces"), "####")
                                    End If
                                End If

                                'default mm
                                ddProductionRatesFinalFiguresMaxPiecesQuotedUnits.SelectedValue = 17
                                If ds.Tables(0).Rows(0).Item("Quoted_MaxPiecesUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_MaxPiecesUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresMaxPiecesQuotedUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Quoted_MaxPiecesUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Max_MaxPieces") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Max_MaxPieces") <> 0 Then
                                        txtProductionRatesFinalFiguresMaxPiecesMaximumValue.Text = Format(ds.Tables(0).Rows(0).Item("Max_MaxPieces"), "####")
                                    End If
                                End If

                                'default mm
                                ddProductionRatesFinalFiguresMaxPiecesMaximumUnits.SelectedValue = 17
                                If ds.Tables(0).Rows(0).Item("Max_MaxPiecesUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Max_MaxPiecesUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresMaxPiecesMaximumUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Max_MaxPiecesUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Quoted_PressCycles") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_PressCycles") <> 0 Then
                                        txtProductionRatesFinalFiguresPressCyclesQuotedValue.Text = Format(ds.Tables(0).Rows(0).Item("Quoted_PressCycles"), "####")
                                    End If
                                End If

                                'default mm
                                ddProductionRatesFinalFiguresPressCyclesQuotedUnits.SelectedValue = 17
                                If ds.Tables(0).Rows(0).Item("Quoted_PressCyclesUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_PressCyclesUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresPressCyclesQuotedUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Quoted_PressCyclesUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Max_PressCycles") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Max_PressCycles") <> 0 Then
                                        txtProductionRatesFinalFiguresPressCyclesMaximumValue.Text = Format(ds.Tables(0).Rows(0).Item("Max_PressCycles"), "####")
                                    End If
                                End If

                                'default mm
                                ddProductionRatesFinalFiguresPressCyclesMaximumUnits.SelectedValue = 17
                                If ds.Tables(0).Rows(0).Item("Max_PressCyclesUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Max_PressCyclesUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresPressCyclesMaximumUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Max_PressCyclesUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Quoted_LineSpeed") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_LineSpeed") <> 0 Then
                                        txtProductionRatesFinalFiguresLineSpeedQuotedValue.Text = Format(ds.Tables(0).Rows(0).Item("Quoted_LineSpeed"), "####")
                                    End If
                                End If

                                'default m/min
                                ddProductionRatesFinalFiguresLineSpeedQuotedUnits.SelectedValue = 19
                                If ds.Tables(0).Rows(0).Item("Quoted_LineSpeedUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_LineSpeedUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresLineSpeedQuotedUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Quoted_LineSpeedUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Max_LineSpeed") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Max_LineSpeed") <> 0 Then
                                        txtProductionRatesFinalFiguresLineSpeedMaximumValue.Text = Format(ds.Tables(0).Rows(0).Item("Max_LineSpeed"), "####")
                                    End If
                                End If

                                'default m/min
                                ddProductionRatesFinalFiguresLineSpeedMaximumUnits.SelectedValue = 19
                                If ds.Tables(0).Rows(0).Item("Max_LineSpeedUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Max_LineSpeedUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresLineSpeedMaximumUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Max_LineSpeedUnitID")
                                    End If
                                End If

                                lblProductionRatesFinalFiguresMixCapacityQuotedLabel.Visible = Not ViewState("Formula_isFleeceType")
                                txtProductionRatesFinalFiguresMixCapacityQuotedValue.Visible = Not ViewState("Formula_isFleeceType")
                                ddProductionRatesFinalFiguresMixCapacityQuotedUnits.Visible = Not ViewState("Formula_isFleeceType")
                                txtProductionRatesFinalFiguresMixCapacityMaximumValue.Visible = Not ViewState("Formula_isFleeceType")
                                ddProductionRatesFinalFiguresMixCapacityMaximumUnits.Visible = Not ViewState("Formula_isFleeceType")

                                lblProductionRatesFinalFiguresNetFormingRateQuotedLabel.Visible = ViewState("Formula_isFleeceType")
                                txtProductionRatesFinalFiguresNetFormingRateQuotedValue.Visible = ViewState("Formula_isFleeceType")
                                ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.Visible = ViewState("Formula_isFleeceType")
                                txtProductionRatesFinalFiguresNetFormingRateMaximumValue.Visible = ViewState("Formula_isFleeceType")
                                ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.Visible = ViewState("Formula_isFleeceType")

                                If ViewState("Formula_isFleeceType") = True Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_NetFormingRate") IsNot System.DBNull.Value Then
                                        If ds.Tables(0).Rows(0).Item("Quoted_NetFormingRate") <> 0 Then
                                            txtProductionRatesFinalFiguresNetFormingRateQuotedValue.Text = Format(ds.Tables(0).Rows(0).Item("Quoted_NetFormingRate"), "####")
                                        End If
                                    End If

                                    'default kg/hr
                                    ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.SelectedValue = 20
                                    If ds.Tables(0).Rows(0).Item("Quoted_NetFormingRateUnitID") IsNot System.DBNull.Value Then
                                        If ds.Tables(0).Rows(0).Item("Quoted_NetFormingRateUnitID") > 0 Then
                                            ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Quoted_NetFormingRateUnitID")
                                        End If
                                    End If

                                    If ds.Tables(0).Rows(0).Item("Max_NetFormingRate") IsNot System.DBNull.Value Then
                                        If ds.Tables(0).Rows(0).Item("Max_NetFormingRate") <> 0 Then
                                            txtProductionRatesFinalFiguresNetFormingRateMaximumValue.Text = Format(ds.Tables(0).Rows(0).Item("Max_NetFormingRate"), "####")
                                        End If
                                    End If

                                    'default kg/hr
                                    ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.SelectedValue = 20
                                    If ds.Tables(0).Rows(0).Item("Max_NetFormingRateUnitID") IsNot System.DBNull.Value Then
                                        If ds.Tables(0).Rows(0).Item("Max_NetFormingRateUnitID") > 0 Then
                                            ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Max_NetFormingRateUnitID")
                                        End If
                                    End If

                                Else
                                    If ds.Tables(0).Rows(0).Item("Quoted_MixCapacity") IsNot System.DBNull.Value Then
                                        If ds.Tables(0).Rows(0).Item("Quoted_MixCapacity") <> 0 Then
                                            txtProductionRatesFinalFiguresMixCapacityQuotedValue.Text = Format(ds.Tables(0).Rows(0).Item("Quoted_MixCapacity"), "####")
                                        End If
                                    End If

                                    'default kg/hr
                                    ddProductionRatesFinalFiguresMixCapacityQuotedUnits.SelectedValue = 20
                                    If ds.Tables(0).Rows(0).Item("Quoted_MixCapacityUnitID") IsNot System.DBNull.Value Then
                                        If ds.Tables(0).Rows(0).Item("Quoted_MixCapacityUnitID") > 0 Then
                                            ddProductionRatesFinalFiguresMixCapacityQuotedUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Quoted_MixCapacityUnitID")
                                        End If
                                    End If

                                    If ds.Tables(0).Rows(0).Item("Max_MixCapacity") IsNot System.DBNull.Value Then
                                        If ds.Tables(0).Rows(0).Item("Max_MixCapacity") <> 0 Then
                                            txtProductionRatesFinalFiguresMixCapacityMaximumValue.Text = Format(ds.Tables(0).Rows(0).Item("Max_MixCapacity"), "####")
                                        End If
                                    End If

                                    'default kg/hr
                                    ddProductionRatesFinalFiguresMixCapacityMaximumUnits.SelectedValue = 20
                                    If ds.Tables(0).Rows(0).Item("Max_MixCapacityUnitID") IsNot System.DBNull.Value Then
                                        If ds.Tables(0).Rows(0).Item("Max_MixCapacityUnitID") > 0 Then
                                            ddProductionRatesFinalFiguresMixCapacityMaximumUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Max_MixCapacityUnitID")
                                        End If
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Quoted_RecycleRate") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_RecycleRate") <> 0 Then
                                        txtProductionRatesFinalFiguresRecycleRateQuotedValue.Text = Format(ds.Tables(0).Rows(0).Item("Quoted_RecycleRate"), "####")
                                    End If
                                End If

                                'default kg/hr
                                ddProductionRatesFinalFiguresRecycleRateQuotedUnits.SelectedValue = 20
                                If ds.Tables(0).Rows(0).Item("Quoted_RecycleRateUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_RecycleRateUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresRecycleRateQuotedUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Quoted_RecycleRateUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Max_RecycleRate") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Max_RecycleRate") <> 0 Then
                                        txtProductionRatesFinalFiguresRecycleRateMaximumValue.Text = Format(ds.Tables(0).Rows(0).Item("Max_RecycleRate"), "####")
                                    End If
                                End If

                                'default kg/hr
                                ddProductionRatesFinalFiguresRecycleRateMaximumUnits.SelectedValue = 20
                                If ds.Tables(0).Rows(0).Item("Max_RecycleRateUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Max_RecycleRateUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresRecycleRateMaximumUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Max_RecycleRateUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Quoted_PartWeight") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_PartWeight") <> 0 Then
                                        txtProductionRatesFinalFiguresPartWeightQuotedValue.Text = Format(ds.Tables(0).Rows(0).Item("Quoted_PartWeight"), "####.0000")
                                    End If
                                End If

                                'default g
                                ddProductionRatesFinalFiguresPartWeightQuotedUnits.SelectedValue = 5
                                If ds.Tables(0).Rows(0).Item("Quoted_PartWeightUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_PartWeightUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresPartWeightQuotedUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Quoted_PartWeightUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Max_PartWeight") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Max_PartWeight") <> 0 Then
                                        txtProductionRatesFinalFiguresPartWeightMaximumValue.Text = Format(ds.Tables(0).Rows(0).Item("Max_PartWeight"), "####.0000")
                                    End If
                                End If

                                'default g
                                ddProductionRatesFinalFiguresPartWeightMaximumUnits.SelectedValue = 5
                                If ds.Tables(0).Rows(0).Item("Max_PartWeightUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Max_PartWeightUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresPartWeightMaximumUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Max_PartWeightUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Quoted_CoatingWeight") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_CoatingWeight") <> 0 Then
                                        txtProductionRatesFinalFiguresCoatingWeightQuotedValue.Text = Format(ds.Tables(0).Rows(0).Item("Quoted_CoatingWeight"), "####.0000")
                                    End If
                                End If

                                'default g
                                ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.SelectedValue = 5
                                If ds.Tables(0).Rows(0).Item("Quoted_CoatingWeightUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_CoatingWeightUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Quoted_CoatingWeightUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("Quoted_TotalWeight") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_TotalWeight") <> 0 Then
                                        txtProductionRatesFinalFiguresTotalWeightQuotedValue.Text = Format(ds.Tables(0).Rows(0).Item("Quoted_TotalWeight"), "####.0000")
                                    End If
                                End If

                                'default g
                                ddProductionRatesFinalFiguresTotalWeightQuotedUnits.SelectedValue = 5
                                If ds.Tables(0).Rows(0).Item("Quoted_TotalWeightUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("Quoted_TotalWeightUnitID") > 0 Then
                                        ddProductionRatesFinalFiguresTotalWeightQuotedUnits.SelectedValue = ds.Tables(0).Rows(0).Item("Quoted_TotalWeightUnitID")
                                    End If
                                End If

                            End If 'end production rates load table ds is not empty

                            ''bind existing CostSheet data for Quoted Info                  
                            ds = CostingModule.GetCostSheetQuotedInfo(ViewState("CostSheetID"))
                            If commonFunctions.CheckDataSet(ds) = True Then

                                If ds.Tables(0).Rows(0).Item("AccountManagerID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("AccountManagerID") > 0 Then
                                        ddQuotedInfoAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AccountManagerID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("PiecesPerYear") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PiecesPerYear") <> 0 Then
                                        txtQuotedInfoPiecesPerYear.Text = ds.Tables(0).Rows(0).Item("PiecesPerYear").ToString
                                    End If
                                End If

                                txtQuotedInfoStandardCostFactor.Text = "1.02"
                                If ds.Tables(0).Rows(0).Item("StandardCostFactor") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("StandardCostFactor") <> 0 Then
                                        txtQuotedInfoStandardCostFactor.Text = ds.Tables(0).Rows(0).Item("StandardCostFactor").ToString
                                    End If
                                End If

                                txtQuotedInfoComments.Text = ds.Tables(0).Rows(0).Item("Comments").ToString

                            End If 'end Quoted Info tab load ds is not empty

                            ''bind existing CostSheet data for Sketch Info                  
                            ds = CostingModule.GetCostSheetSketchInfo(ViewState("CostSheetID"))
                            If commonFunctions.CheckDataSet(ds) = True Then
                                txtDrawingPartSketchMemo.Text = ds.Tables(0).Rows(0).Item("SketchMemo").ToString
                                imgDrawingPartSketch.Src = "Display_Sketch_Image.aspx?CostSheetID=" & ViewState("CostSheetID")
                            End If 'end Sketch Info tab load ds is not empty

                            ''bind existing CostSheet data for Composite Part Specification                 
                            ds = CostingModule.GetCostSheetCompositePartSpecification(ViewState("CostSheetID"))
                            If commonFunctions.CheckDataSet(ds) = True Then

                                If ds.Tables(0).Rows(0).Item("FormulaID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("FormulaID") > 0 Then
                                        ddCompositePartSpecFormula.SelectedValue = ds.Tables(0).Rows(0).Item("FormulaID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("PartThickness") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartThickness") <> 0 Then
                                        txtCompositePartSpecPartThicknessValue.Text = ds.Tables(0).Rows(0).Item("PartThickness").ToString
                                    End If
                                End If

                                'default mm
                                ddCompositePartSpecPartThicknessUnits.SelectedValue = 17
                                If ds.Tables(0).Rows(0).Item("PartThicknessUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartThicknessUnitID") > 0 Then
                                        ddCompositePartSpecPartThicknessUnits.SelectedValue = ds.Tables(0).Rows(0).Item("PartThicknessUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("PartSpecificGravity") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartSpecificGravity") <> 0 Then
                                        txtCompositePartSpecPartSpecificGravityValue.Text = ds.Tables(0).Rows(0).Item("PartSpecificGravity").ToString
                                    End If
                                End If

                                'default g/m3
                                ddCompositePartSpecPartSpecificGravityUnits.SelectedValue = 16
                                If ds.Tables(0).Rows(0).Item("PartSpecificGravityUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartSpecificGravityUnitID") > 0 Then
                                        ddCompositePartSpecPartSpecificGravityUnits.SelectedValue = ds.Tables(0).Rows(0).Item("PartSpecificGravityUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("PartArea") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartArea") <> 0 Then
                                        txtCompositePartSpecPartAreaValue.Text = ds.Tables(0).Rows(0).Item("PartArea").ToString
                                    End If
                                End If

                                'default m2
                                ddCompositePartSpecPartAreaUnits.SelectedValue = 14
                                If ds.Tables(0).Rows(0).Item("PartAreaUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("PartAreaUnitID") > 0 Then
                                        ddCompositePartSpecPartAreaUnits.SelectedValue = ds.Tables(0).Rows(0).Item("PartAreaUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("RSSWeight") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("RSSWeight") <> 0 Then
                                        txtCompositePartSpecRSSWeightValue.Text = ds.Tables(0).Rows(0).Item("RSSWeight").ToString
                                    End If
                                End If

                                'default 5
                                ddCompositePartSpecRSSWeightUnits.SelectedValue = 5
                                If ds.Tables(0).Rows(0).Item("RSSWeightUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("RSSWeightUnitID") > 0 Then
                                        ddCompositePartSpecRSSWeightUnits.SelectedValue = ds.Tables(0).Rows(0).Item("RSSWeightUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("AntiBlockCoating") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("AntiBlockCoating") <> 0 Then
                                        txtCompositePartSpecAntiBlockCoatingValue.Text = ds.Tables(0).Rows(0).Item("AntiBlockCoating").ToString
                                    End If
                                End If

                                'default 5
                                ddCompositePartSpecAntiBlockCoatingUnits.SelectedValue = 5
                                If ds.Tables(0).Rows(0).Item("AntiBlockCoatingUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("AntiBlockCoatingUnitID") > 0 Then
                                        ddCompositePartSpecAntiBlockCoatingUnits.SelectedValue = ds.Tables(0).Rows(0).Item("AntiBlockCoatingUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("HotMeldAdhesive") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("HotMeldAdhesive") <> 0 Then
                                        txtCompositePartSpecHotMeldAdhesiveValue.Text = ds.Tables(0).Rows(0).Item("HotMeldAdhesive").ToString
                                    End If
                                End If

                                'default g
                                ddCompositePartSpecHotMeldAdhesiveUnits.SelectedValue = 5
                                If ds.Tables(0).Rows(0).Item("HotMeldAdhesiveUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("HotMeldAdhesiveUnitID") > 0 Then
                                        ddCompositePartSpecHotMeldAdhesiveUnits.SelectedValue = ds.Tables(0).Rows(0).Item("HotMeldAdhesiveUnitID")
                                    End If
                                End If

                            End If 'end Composite Part Specification tab load ds is not empty

                            ''bind existing CostSheet data for Molded Barrier                 
                            ds = CostingModule.GetCostSheetMoldedBarrier(ViewState("CostSheetID"))
                            If commonFunctions.CheckDataSet(ds) = True Then

                                If ds.Tables(0).Rows(0).Item("FormulaID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("FormulaID") > 0 Then
                                        ddMoldedBarrierFormula.SelectedValue = ds.Tables(0).Rows(0).Item("FormulaID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("BarrierLength") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("BarrierLength") <> 0 Then
                                        txtMoldedBarrierApproximateLengthValue.Text = ds.Tables(0).Rows(0).Item("BarrierLength").ToString
                                    End If
                                End If

                                'default mm
                                ddMoldedBarrierApproximateLengthUnits.SelectedValue = 17
                                If ds.Tables(0).Rows(0).Item("BarrierLengthUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("BarrierLengthUnitID") > 0 Then
                                        ddMoldedBarrierApproximateLengthUnits.SelectedValue = ds.Tables(0).Rows(0).Item("BarrierLengthUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("BarrierWidth") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("BarrierWidth") <> 0 Then
                                        txtMoldedBarrierApproximateWidthValue.Text = ds.Tables(0).Rows(0).Item("BarrierWidth").ToString
                                    End If
                                End If

                                'default mm
                                ddMoldedBarrierApproximateWidthUnits.SelectedValue = 17
                                If ds.Tables(0).Rows(0).Item("BarrierWidthUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("BarrierWidthUnitID") > 0 Then
                                        ddMoldedBarrierApproximateWidthUnits.SelectedValue = ds.Tables(0).Rows(0).Item("BarrierWidthUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("BarrierThickness") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("BarrierThickness") <> 0 Then
                                        txtMoldedBarrierApproximateThicknessValue.Text = ds.Tables(0).Rows(0).Item("BarrierThickness").ToString
                                    End If
                                End If

                                'default mm
                                ddMoldedBarrierApproximateThicknessUnits.SelectedValue = 17
                                If ds.Tables(0).Rows(0).Item("BarrierThicknessUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("BarrierThicknessUnitID") > 0 Then
                                        ddMoldedBarrierApproximateThicknessUnits.SelectedValue = ds.Tables(0).Rows(0).Item("BarrierThicknessUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("BarrierBlankArea") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("BarrierBlankArea") <> 0 Then
                                        txtMoldedBarrierBlankAreaValue.Text = ds.Tables(0).Rows(0).Item("BarrierBlankArea").ToString
                                    End If
                                End If

                                'default m2
                                ddMoldedBarrierBlankAreaUnits.SelectedValue = 14
                                If ds.Tables(0).Rows(0).Item("BarrierBlankAreaUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("BarrierBlankAreaUnitID") > 0 Then
                                        ddMoldedBarrierBlankAreaUnits.SelectedValue = ds.Tables(0).Rows(0).Item("BarrierBlankAreaUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("SpecificGravity") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("SpecificGravity") <> 0 Then
                                        txtMoldedBarrierSpecificGravityValue.Text = ds.Tables(0).Rows(0).Item("SpecificGravity").ToString
                                    End If
                                End If

                                'default g/m3
                                ddMoldedBarrierSpecificGravityUnits.SelectedValue = 16
                                If ds.Tables(0).Rows(0).Item("SpecificGravityUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("SpecificGravityUnitID") > 0 Then
                                        ddMoldedBarrierSpecificGravityUnits.SelectedValue = ds.Tables(0).Rows(0).Item("SpecificGravityUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("WeightPerArea") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("WeightPerArea") <> 0 Then
                                        txtMoldedBarrierWeightPerAreaValue.Text = ds.Tables(0).Rows(0).Item("WeightPerArea").ToString
                                    End If
                                End If

                                'default kg/m2
                                ddMoldedBarrierWeightPerAreaUnits.SelectedValue = 21
                                If ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID") > 0 Then
                                        ddMoldedBarrierWeightPerAreaUnits.SelectedValue = ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("BlankWeight") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("BlankWeight") <> 0 Then
                                        txtMoldedBarrierBlankWeightValue.Text = ds.Tables(0).Rows(0).Item("BlankWeight").ToString
                                    End If
                                End If

                                'default lb
                                ddMoldedBarrierBlankWeightUnits.SelectedValue = 2
                                If ds.Tables(0).Rows(0).Item("BlankWeightUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("BlankWeightUnitID") > 0 Then
                                        ddMoldedBarrierBlankWeightUnits.SelectedValue = ds.Tables(0).Rows(0).Item("BlankWeightUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("AntiBlockCoating") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("AntiBlockCoating") <> 0 Then
                                        txtMoldedBarrierAntiBlockCoatingValue.Text = ds.Tables(0).Rows(0).Item("AntiBlockCoating").ToString
                                    End If
                                End If

                                'default lb/blank
                                ddMoldedBarrierAntiBlockCoatingUnits.SelectedValue = 26
                                If ds.Tables(0).Rows(0).Item("AntiBlockCoatingUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("AntiBlockCoatingUnitID") > 0 Then
                                        ddMoldedBarrierAntiBlockCoatingUnits.SelectedValue = ds.Tables(0).Rows(0).Item("AntiBlockCoatingUnitID")
                                    End If
                                End If

                                If ds.Tables(0).Rows(0).Item("TotalBarrierWeight") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("TotalBarrierWeight") <> 0 Then
                                        txtMoldedBarrierTotalWeightValue.Text = ds.Tables(0).Rows(0).Item("TotalBarrierWeight").ToString
                                    End If
                                End If

                                'default lb
                                ddMoldedBarrierTotalWeightUnits.SelectedValue = 2
                                If ds.Tables(0).Rows(0).Item("TotalBarrierWeightUnitID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("TotalBarrierWeightUnitID") > 0 Then
                                        ddMoldedBarrierTotalWeightUnits.SelectedValue = ds.Tables(0).Rows(0).Item("TotalBarrierWeightUnitID")
                                    End If
                                End If

                            End If

                        Else
                            ViewState("isRestricted") = True
                            Session("DeletedCostSheet") = ViewState("CostSheetID").ToString

                            Response.Redirect("Cost_Sheet_List.aspx", False)
                        End If 'end if not obsolete
                    Else
                        ViewState("isRestricted") = True
                        Response.Redirect("Cost_Sheet_List.aspx", False)
                    End If  'end cost sheet load ds is not empty
                End If ' end restricted read only
            End If ' if cost sheet id > 0

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Private Sub CalculateCostSheetTotal()

        Try

            Dim ds As DataSet

            Dim dTempCapitalTotal As Double = 0

            Dim dTempCostSheetSubTotalWOScrap As Double = 0
            Dim dTempCostSheetSubTotal As Double = 0

            Dim dFixedCostTotal As Double = 0

            Dim dMinPriceMargin As Double = 0
            Dim dMinSellingPrice As Double = 0

            Dim dPriceVariableMarginPercent As Double = 0
            Dim dPriceVariableMarginDollar As Double = 0
            Dim dPriceVariableMarginInclDeprPercent As Double = 0
            Dim dPriceVariableMarginInclDeprDollar As Double = 0

            Dim dPriceGrossMarginPercent As Double = 0
            Dim dPriceGrossMarginDollar As Double = 0

            Dim dTempLaborTotalWOScrap As Double = 0
            Dim dTempLaborTotal As Double = 0

            Dim dTempMaterialTotalWOScrap As Double = 0
            Dim dTempMaterialTotal As Double = 0

            Dim dTempMiscCostTotal As Double = 0

            Dim dTempOverheadCostFixedRateTotalWOScrap As Double = 0
            Dim dTempOverheadCostVariableRateTotalWOScrap As Double = 0

            Dim dTempOverallCostTotalWOScrap As Double = 0
            Dim dTempOverallCostTotal As Double = 0

            Dim dTempOverheadTotalWOScrap As Double = 0
            Dim dTempOverheadTotal As Double = 0

            Dim dTempPackagingTotalWOScrap As Double = 0
            Dim dTempPackagingTotal As Double = 0

            Dim dTempScrapTotal As Double = 0

            Dim dTempSGATotal As Double = 0

            Dim dVariableCostTotal As Double = 0

            Dim strUGNFacility As String = ""

            ds = CostingModule.GetCostSheetTotal(ViewState("CostSheetID"))

            If commonFunctions.CheckDataSet(ds) = True Then

                txtMaterialCostTotalWOScrapValue.Text = ""
                If ds.Tables(0).Rows(0).Item("MaterialCostTotalWOScrap") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialCostTotalWOScrap") <> 0 Then
                        txtMaterialCostTotalWOScrapValue.Text = ds.Tables(0).Rows(0).Item("MaterialCostTotalWOScrap")
                        dTempMaterialTotalWOScrap = ds.Tables(0).Rows(0).Item("MaterialCostTotalWOScrap")
                    End If
                End If

                txtMaterialCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("MaterialCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialCostTotal") <> 0 Then
                        txtMaterialCostTotalValue.Text = ds.Tables(0).Rows(0).Item("MaterialCostTotal")
                        dTempMaterialTotal = ds.Tables(0).Rows(0).Item("MaterialCostTotal")
                    End If
                End If

                txtPackagingCostTotalWOScrapValue.Text = ""
                If ds.Tables(0).Rows(0).Item("PackagingCostTotalWOScrap") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PackagingCostTotalWOScrap") <> 0 Then
                        txtPackagingCostTotalWOScrapValue.Text = Format(ds.Tables(0).Rows(0).Item("PackagingCostTotalWOScrap"), "###0.0000")
                        dTempPackagingTotalWOScrap = ds.Tables(0).Rows(0).Item("PackagingCostTotalWOScrap")
                    End If
                End If

                txtPackagingCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("PackagingCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PackagingCostTotal") <> 0 Then
                        txtPackagingCostTotalValue.Text = ds.Tables(0).Rows(0).Item("PackagingCostTotal")
                        dTempPackagingTotal = ds.Tables(0).Rows(0).Item("PackagingCostTotal")
                    End If
                End If

                lblMaterialAndPackagingCostTotalValue.Text = ""
                If txtMaterialCostTotalValue.Text.Trim <> "" Or txtPackagingCostTotalValue.Text.Trim <> "" Then
                    lblMaterialAndPackagingCostTotalWOScrapValue.Text = Format(dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap, "###0.0000")
                    lblMaterialAndPackagingCostTotalValue.Text = Format(dTempMaterialTotal + dTempPackagingTotal, "###0.0000")
                End If

                txtLaborCostTotalWOScrapValue.Text = ""
                If ds.Tables(0).Rows(0).Item("LaborCostTotalWOScrap") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("LaborCostTotalWOScrap") <> 0 Then
                        txtLaborCostTotalWOScrapValue.Text = ds.Tables(0).Rows(0).Item("LaborCostTotalWOScrap")
                        dTempLaborTotalWOScrap = ds.Tables(0).Rows(0).Item("LaborCostTotalWOScrap")
                    End If
                End If

                txtLaborCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("LaborCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("LaborCostTotal") <> 0 Then
                        txtLaborCostTotalValue.Text = ds.Tables(0).Rows(0).Item("LaborCostTotal")
                        dTempLaborTotal = ds.Tables(0).Rows(0).Item("LaborCostTotal")
                    End If
                End If

                txtOverheadCostTotalWOScrapValue.Text = ""
                If ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrap") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrap") <> 0 Then
                        txtOverheadCostTotalWOScrapValue.Text = ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrap")
                        dTempOverheadTotalWOScrap = ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrap")
                    End If
                End If

                txtOverheadCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("OverheadCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCostTotal") <> 0 Then
                        txtOverheadCostTotalValue.Text = ds.Tables(0).Rows(0).Item("OverheadCostTotal")
                        dTempOverheadTotal = ds.Tables(0).Rows(0).Item("OverheadCostTotal")
                    End If
                End If

                '08/24/2010
                If dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap + dTempLaborTotalWOScrap + dTempOverheadTotalWOScrap <> 0 Then
                    dTempScrapTotal = Round(((dTempMaterialTotal + dTempPackagingTotal + dTempLaborTotal + dTempOverheadTotal) - (dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap + dTempLaborTotalWOScrap + dTempOverheadTotalWOScrap)), 4)
                End If

                lblScrapCostTotalValue.Text = ""
                If dTempScrapTotal <> 0 Then
                    lblScrapCostTotalValue.Text = Format(dTempScrapTotal, "###0.0000")
                End If

                lblCapitalCostTotalWOScrapValue.Text = ""
                txtCapitalCostTotalValue.Text = ""
                lblCapitalCostTotalValue2.Text = ""
                If ds.Tables(0).Rows(0).Item("CapitalCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CapitalCostTotal") <> 0 Then
                        lblCapitalCostTotalWOScrapValue.Text = ds.Tables(0).Rows(0).Item("CapitalCostTotal")
                        txtCapitalCostTotalValue.Text = ds.Tables(0).Rows(0).Item("CapitalCostTotal")
                        lblCapitalCostTotalValue2.Text = ds.Tables(0).Rows(0).Item("CapitalCostTotal")
                        dTempCapitalTotal = ds.Tables(0).Rows(0).Item("CapitalCostTotal")
                    End If
                End If

                lblManufacturingCostTotalValue.Text = ""
                dTempCostSheetSubTotalWOScrap = dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap + dTempLaborTotalWOScrap + dTempOverheadTotalWOScrap + dTempScrapTotal + dTempCapitalTotal
                dTempCostSheetSubTotal = dTempMaterialTotal + dTempPackagingTotal + dTempOverheadTotal + dTempLaborTotal + dTempCapitalTotal

                If dTempCostSheetSubTotalWOScrap <> 0 Then
                    lblManufacturingCostTotalWOScrapValue.Text = Format(dTempCostSheetSubTotalWOScrap, "###0.0000")
                End If

                If dTempCostSheetSubTotal <> 0 Then
                    lblManufacturingCostTotalValue.Text = Format(dTempCostSheetSubTotal, "###0.0000")
                End If

                txtMiscCostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("MiscCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MiscCostTotal") <> 0 Then
                        txtMiscCostTotalValue.Text = ds.Tables(0).Rows(0).Item("MiscCostTotal")
                        dTempMiscCostTotal = ds.Tables(0).Rows(0).Item("MiscCostTotal")
                    End If
                End If

                lblSGACostTotalValue.Text = ""
                If ds.Tables(0).Rows(0).Item("SGACostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("SGACostTotal") <> 0 Then
                        lblSGACostTotalValue.Text = Format(ds.Tables(0).Rows(0).Item("SGACostTotal"), "###0.0000")
                        dTempSGATotal = ds.Tables(0).Rows(0).Item("SGACostTotal")
                    End If
                End If

                'Overall Cost Sheet Total             
                txtOverallCostTotalValue.Text = ""

                dTempOverallCostTotalWOScrap = Format(dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap + dTempLaborTotalWOScrap + dTempOverheadTotalWOScrap + dTempScrapTotal + dTempMiscCostTotal + dTempSGATotal + dTempCapitalTotal, "###0.0000")
                dTempOverallCostTotal = Format(dTempMaterialTotal + dTempPackagingTotal + dTempOverheadTotal + dTempLaborTotal + dTempMiscCostTotal + dTempSGATotal + dTempCapitalTotal, "###0.0000")

                If dTempOverallCostTotal <> 0 Then
                    txtOverallCostTotalValue.Text = dTempOverallCostTotal
                End If

                If ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrapVariableRate") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrapVariableRate") <> 0 Then
                        dTempOverheadCostVariableRateTotalWOScrap = ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrapVariableRate")
                    End If
                End If

                dVariableCostTotal = dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap + dTempLaborTotalWOScrap + dTempOverheadCostVariableRateTotalWOScrap + dTempScrapTotal

                lblVariableCostTotalValue.Text = ""
                If dVariableCostTotal <> 0 Then
                    lblVariableCostTotalValue.Text = Format(dVariableCostTotal, "###0.0000")
                End If

                If ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrapFixedRate") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrapFixedRate") <> 0 Then
                        dTempOverheadCostFixedRateTotalWOScrap = ds.Tables(0).Rows(0).Item("OverheadCostTotalWOScrapFixedRate")
                    End If
                End If

                dFixedCostTotal = dTempOverheadCostFixedRateTotalWOScrap

                lblFixedCostTotalValue.Text = ""
                If dFixedCostTotal <> 0 Then
                    lblFixedCostTotalValue.Text = Format(dFixedCostTotal, "###0.0000")
                End If

                If ddUGNFacilityValue.SelectedIndex > 0 Then
                    strUGNFacility = ddUGNFacilityValue.SelectedValue
                End If

                ds = CostingModule.GetCostSheetPriceMargin(strUGNFacility)

                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("MinPriceMargin") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("MinPriceMargin") <> 0 Then
                            dMinPriceMargin = ds.Tables(0).Rows(0).Item("MinPriceMargin")
                        End If
                    End If
                End If

                lblPriceVariableMarginPercentTargetValue.Text = ""
                If dMinPriceMargin <> 0 Then
                    lblPriceVariableMarginPercentTargetValue.Text = Format(dMinPriceMargin * 100, "###0.0")
                End If

                If (1 - dMinPriceMargin) <> 0 Then
                    dMinSellingPrice = Round((dVariableCostTotal + dTempMiscCostTotal + dTempCapitalTotal) / (1 - dMinPriceMargin), 4)
                End If

                lblMinimumSellingPriceValue.Text = ""
                If dMinSellingPrice <> 0 Then
                    lblMinimumSellingPriceValue.Text = Format(dMinSellingPrice, "###0.0000")
                End If

                dPriceVariableMarginDollar = dMinSellingPrice - dVariableCostTotal
                dPriceVariableMarginInclDeprDollar = ((dMinSellingPrice - dVariableCostTotal) - dTempCapitalTotal) - dTempMiscCostTotal

                lblPriceVariableMarginDollarValue.Text = ""
                lblPriceVariableMarginInclDeprDollarValue.Text = ""
                If dPriceVariableMarginDollar <> 0 Then
                    lblPriceVariableMarginDollarValue.Text = Format(dPriceVariableMarginDollar, "###0.0000")
                    lblPriceVariableMarginInclDeprDollarValue.Text = Format(dPriceVariableMarginInclDeprDollar, "###0.0000")
                End If

                lblPriceVariableMarginPercentValue.Text = ""
                lblPriceVariableMarginInclDeprPercentValue.Text = ""
                If dMinSellingPrice <> 0 Then
                    dPriceVariableMarginPercent = dPriceVariableMarginDollar / dMinSellingPrice
                    dPriceVariableMarginInclDeprPercent = dPriceVariableMarginInclDeprDollar / dMinSellingPrice
                    lblPriceVariableMarginPercentValue.Text = Format(dPriceVariableMarginPercent * 100, "###0.0")
                    lblPriceVariableMarginInclDeprPercentValue.Text = Format(dPriceVariableMarginInclDeprPercent * 100, "###0.0")
                End If

                dPriceGrossMarginDollar = dMinSellingPrice - dTempOverallCostTotal

                lblPriceGrossMarginDollarValue.Text = ""
                If dPriceGrossMarginDollar <> 0 Then
                    lblPriceGrossMarginDollarValue.Text = Format(dPriceGrossMarginDollar, "###0.0000")
                End If

                lblPriceGrossMarginDollarValue.ForeColor = Color.Black
                If dPriceGrossMarginDollar < 0 Then
                    lblPriceGrossMarginDollarValue.ForeColor = Color.Red
                End If

                If dMinSellingPrice <> 0 Then
                    dPriceGrossMarginPercent = dPriceGrossMarginDollar / dMinSellingPrice
                End If

                lblPriceGrossMarginPercentValue.Text = ""
                If dPriceGrossMarginPercent <> 0 Then
                    lblPriceGrossMarginPercentValue.Text = Format(dPriceGrossMarginPercent * 100, "###0.0")
                End If

                lblPriceGrossMarginPercentValue.ForeColor = Color.Black
                If dPriceGrossMarginPercent < 0 Then
                    lblPriceGrossMarginPercentValue.ForeColor = Color.Red
                End If

                'update totals table
                CostingModule.UpdateCostSheetTotalScrap(ViewState("CostSheetID"), dTempScrapTotal)
                CostingModule.UpdateCostSheetTotalOverall(ViewState("CostSheetID"), dTempOverallCostTotal, dFixedCostTotal, dVariableCostTotal, dMinPriceMargin, dMinSellingPrice, dPriceVariableMarginPercent, dPriceVariableMarginDollar, dPriceVariableMarginInclDeprPercent, dPriceVariableMarginInclDeprDollar, dPriceGrossMarginPercent, dPriceGrossMarginDollar)

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text
    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Cost Sheet Detail"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > Cost Sheet Detail "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            'lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            'clear crystal reports
            CostingModule.CleanCostingCrystalReports()

            If Not Page.IsPostBack Then

                InitializeViewState()

                CheckRights()

                BindCriteria()

                'search new BPCS PartNo
                Dim strNewPartNoClientScript As String = HandleBPCSPopUps(txtNewPartNoValue.ClientID, txtNewPartRevisionValue.ClientID, "")
                iBtnGetNewPartNo.Attributes.Add("onClick", strNewPartNoClientScript)

                'search original BPCS PartNo
                Dim strOriginalPartNoClientScript As String = HandleBPCSPopUps(txtOriginalPartNoValue.ClientID, txtOriginalPartRevisionValue.ClientID, "")
                iBtnOriginalPartNo.Attributes.Add("onClick", strOriginalPartNoClientScript)

                'search new drawingno popup
                Dim strNewDrawingNoClientScript As String = HandleDrawingPopUps(txtNewDrawingNoValue.ClientID)
                iBtnGetDrawingInfo.Attributes.Add("onClick", strNewDrawingNoClientScript)

                'search RFD popup
                Dim strRFDNoClientScript As String = HandleRFDPopUps(txtRFDNoValue.ClientID, txtRFDSelectionType.ClientID, txtRFDChildRow.ClientID)
                iBtnSearchRFD.Attributes.Add("onClick", strRFDNoClientScript)

                If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then
                    ViewState("CostSheetID") = CType(HttpContext.Current.Request.QueryString("CostSheetID"), Integer)
                    If ViewState("CostSheetID") > 0 Then
                        BindData()

                        'Cost Form Preview                        
                        Dim strCostFormPreviewClientScript As String = "javascript:void(window.open('Cost_Sheet_Preview.aspx?CostSheetID=" & ViewState("CostSheetID") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                        btnPreviewCostSheet.Attributes.Add("onclick", strCostFormPreviewClientScript)

                        ' dielayout preview popup
                        If cbPartSpecificationsIsDiecutValue.Checked = True Then
                            Dim strDieLayoutPreviewClientScript As String = "javascript:void(window.open('Die_Layout_Preview.aspx?CostSheetID=" & ViewState("CostSheetID") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                            btnPreviewDieLayout.Attributes.Add("onclick", strDieLayoutPreviewClientScript)
                        End If
                    End If
                End If

                'set some default values
                If ViewState("CostSheetID") = 0 Then
                    ddCostSheetStatusValue.SelectedValue = "Pending"
                    txtQuotedInfoStandardCostFactor.Text = "1.02"

                    accReplicationActivity.SelectedIndex = -1
                    accCostHeader.SelectedIndex = 0
                    accCostCustomerProgram.SelectedIndex = -1
                    accCostCalculations.SelectedIndex = -1
                    accCostTotals.SelectedIndex = -1
                Else
                    accReplicationActivity.SelectedIndex = -1
                    accCostHeader.SelectedIndex = 0
                    accCostCustomerProgram.SelectedIndex = 0
                    accCostCalculations.SelectedIndex = 0
                    accCostTotals.SelectedIndex = 0
                End If

            End If

            If HttpContext.Current.Session("CopyCostSheet") IsNot Nothing Then
                If HttpContext.Current.Session("CopyCostSheet") <> "" Then
                    lblMessage.Text += "The Cost sheet was successfully copied and saved."
                    lblMessageLowerPage.Text += "The Cost sheet was successfully copied and saved."
                    HttpContext.Current.Session("CopyCostSheet") = Nothing
                End If
            End If

            txtNotesValue.Attributes.Add("onkeypress", "return tbLimit();")
            txtNotesValue.Attributes.Add("onkeyup", "return tbCount(" + lblNotesValueCharCount.ClientID + ");")
            txtNotesValue.Attributes.Add("maxLength", "400")

            txtPartSpecificationsRepackMaterialValue.Attributes.Add("onkeypress", "return tbLimit();")
            txtPartSpecificationsRepackMaterialValue.Attributes.Add("onkeyup", "return tbCount(" + lblPartSpecificationsRepackMaterialValueCharCount.ClientID + ");")
            txtPartSpecificationsRepackMaterialValue.Attributes.Add("maxLength", "100")

            txtQuotedInfoComments.Attributes.Add("onkeypress", "return tbLimit();")
            txtQuotedInfoComments.Attributes.Add("onkeyup", "return tbCount(" + lblQuotedInfoCommentsCharCount.ClientID + ");")
            txtQuotedInfoComments.Attributes.Add("maxLength", "5000")

            txtDrawingPartSketchMemo.Attributes.Add("onkeypress", "return tbLimit();")
            txtDrawingPartSketchMemo.Attributes.Add("onkeyup", "return tbCount(" + lblDrawingPartSketchMemoCharCount.ClientID + ");")
            txtDrawingPartSketchMemo.Attributes.Add("maxLength", "400")

            btnRemoveAdditionalOfflineRate.Attributes.Add("onclick", "if(confirm('Are you sure you want to delete all additional offline rates for this cost sheet?.  ')){}else{return false}")
            btnRemoveCapital.Attributes.Add("onclick", "if(confirm('Are you sure you want to delete all capital for this cost sheet?.  ')){}else{return false}")
            btnRemoveLabor.Attributes.Add("onclick", "if(confirm('Are you sure you want to delete all labor for this cost sheet?.  ')){}else{return false}")
            btnRemoveMaterials.Attributes.Add("onclick", "if(confirm('Are you sure you want to delete all materials for this cost sheet?.  ')){}else{return false}")
            btnRemoveMiscCost.Attributes.Add("onclick", "if(confirm('Are you sure you want to delete all misc costs for this cost sheet?.  ')){}else{return false}")
            btnRemoveOverhead.Attributes.Add("onclick", "if(confirm('Are you sure you want to delete all overhead for this cost sheet?.  ')){}else{return false}")
            btnRemovePackaging.Attributes.Add("onclick", "if(confirm('Are you sure you want to delete all packaging for this cost sheet?.  ')){}else{return false}")
            btnDelete.Attributes.Add("onclick", "if(confirm('Are you sure that you want to delete this Cost Sheet?.  ')){}else{return false}")

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub lnkShowLargerSketchImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkShowLargerSketchImage.Click

        Try

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Show Enlarged Sketch", "window.open('Display_Full_Sketch_Image.aspx?CostSheetID=" & ViewState("CostSheetID") & "'," & Now.Ticks & ",'resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=yes,location=no');", True)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text

    End Sub
    Protected Sub btnPreApprovalNotification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreApprovalNotification.Click

        Try
            ClearMessages()

            If ViewState("CostSheetID") > 0 Then
                Response.Redirect("Cost_Sheet_Pre_Approval_List.aspx?CostSheetID=" & ViewState("CostSheetID"), False)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub btnPostApprovalNotification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPostApprovalNotification.Click

        Try
            ClearMessages()

            If ViewState("CostSheetID") > 0 Then
                Response.Redirect("Cost_Sheet_Post_Approval_List.aspx?CostSheetID=" & ViewState("CostSheetID"), False)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub menuCostingTopTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles menuCostSheetTopTabs.MenuItemClick

        Try
            menuCostSheetBottomTabs.StaticMenuItemStyle.CssClass = "tab"
            menuCostSheetBottomTabs.StaticSelectedStyle.CssClass = "tab"
            menuCostSheetTopTabs.StaticSelectedStyle.CssClass = "selectedTab"

            mvBuildCostSheet.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text

    End Sub
    Protected Sub menuCostingBottomTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles menuCostSheetBottomTabs.MenuItemClick

        Try
            menuCostSheetTopTabs.StaticMenuItemStyle.CssClass = "tab"
            menuCostSheetTopTabs.StaticSelectedStyle.CssClass = "tab"
            menuCostSheetBottomTabs.StaticSelectedStyle.CssClass = "selectedTab"

            mvBuildCostSheet.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvTopLevelInfo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTopLevelInfo.RowCommand

        Try

            Dim txtPartNoTemp As TextBox
            Dim txtPartRevisionTemp As TextBox
            Dim txtPartNameTemp As TextBox
            Dim intRowsAffected As Integer = 0

            Dim dsCustomerPartNo As DataSet
            Dim dsFinishedGoodPartNo As DataSet
            Dim iRowCounter As Integer = 0
            Dim strTempFinishedGoodPartNo As String = ""
            Dim strTempFinishedGoodPartName As String = ""
            Dim strTempFinishedGoodPartRevision As String = ""

            ' ''***
            ' ''This section gets all finsihed Good BPCS Part Numbers based on Customer Part Number
            ' ''***
            If e.CommandName = "CopyList" Then
                If txtNewCustomerPartNoValue.Text.Trim <> "" Then

                    '(LREY) 01/08/2014
                    dsCustomerPartNo = commonFunctions.GetCustomerPartBPCSPartRelate("", txtNewCustomerPartNoValue.Text.Trim, "", "", "")

                    If commonFunctions.CheckDataSet(dsCustomerPartNo) = True Then
                        For iRowCounter = 0 To dsCustomerPartNo.Tables(0).Rows.Count - 1
                            strTempFinishedGoodPartNo = dsCustomerPartNo.Tables(0).Rows(iRowCounter).Item("PartNo").ToString.Trim

                            dsFinishedGoodPartNo = commonFunctions.GetBPCSPartNo(strTempFinishedGoodPartNo, "")
                            If commonFunctions.CheckDataSet(dsFinishedGoodPartNo) = True Then
                                strTempFinishedGoodPartName = dsFinishedGoodPartNo.Tables(0).Rows(0).Item("PartName").ToString.Trim
                                strTempFinishedGoodPartRevision = dsFinishedGoodPartNo.Tables(0).Rows(0).Item("PartRevision").ToString.Trim

                                'insert new row to Finished Good List of Cost Sheet
                                odsCostSheetTopLevelInfo.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                                odsCostSheetTopLevelInfo.InsertParameters("PartNo").DefaultValue = strTempFinishedGoodPartNo
                                odsCostSheetTopLevelInfo.InsertParameters("PartRevision").DefaultValue = strTempFinishedGoodPartRevision
                                odsCostSheetTopLevelInfo.InsertParameters("PartName").DefaultValue = strTempFinishedGoodPartName
                                intRowsAffected = odsCostSheetTopLevelInfo.Insert()
                            End If
                        Next

                        gvTopLevelInfo.DataBind()
                    End If

                End If
            End If

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                txtPartNoTemp = CType(gvTopLevelInfo.FooterRow.FindControl("txtFooterTopLevelPartNo"), TextBox)
                txtPartRevisionTemp = CType(gvTopLevelInfo.FooterRow.FindControl("txtFooterPartRevision"), TextBox)
                txtPartNameTemp = CType(gvTopLevelInfo.FooterRow.FindControl("txtFooterPartName"), TextBox)

                odsCostSheetTopLevelInfo.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                odsCostSheetTopLevelInfo.InsertParameters("PartNo").DefaultValue = txtPartNoTemp.Text
                odsCostSheetTopLevelInfo.InsertParameters("PartRevision").DefaultValue = txtPartRevisionTemp.Text
                odsCostSheetTopLevelInfo.InsertParameters("PartName").DefaultValue = txtPartNameTemp.Text
                intRowsAffected = odsCostSheetTopLevelInfo.Insert()

                dsFinishedGoodPartNo = commonFunctions.GetBPCSPartNo(txtPartNoTemp.Text, "")
                If commonFunctions.CheckDataSet(dsFinishedGoodPartNo) = False Then
                    lblMessage.Text += "<br>WARNING: The FINISHED GOOD Internal Part Number is not in the Oracle System. Please contact Product Engineering."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvTopLevelInfo.ShowFooter = False
            Else
                gvTopLevelInfo.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtPartNoTemp = CType(gvTopLevelInfo.FooterRow.FindControl("txtFooterTopLevelPartNo"), TextBox)
                txtPartNoTemp.Text = Nothing

                txtPartRevisionTemp = CType(gvTopLevelInfo.FooterRow.FindControl("txtFooterPartRevision"), TextBox)
                txtPartRevisionTemp.Text = Nothing

                txtPartNameTemp = CType(gvTopLevelInfo.FooterRow.FindControl("txtFooterPartName"), TextBox)
                txtPartNameTemp.Text = Nothing
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvAdditionalOfflineRate_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAdditionalOfflineRate.DataBound

        'hide header of first column
        If gvAdditionalOfflineRate.Rows.Count > 0 Then
            gvAdditionalOfflineRate.HeaderRow.Cells(0).Visible = False
            gvAdditionalOfflineRate.HeaderRow.Cells(1).Visible = False
        End If

    End Sub
    Protected Sub gvAdditionalOfflineRate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAdditionalOfflineRate.RowCommand

        Try

            Dim ddDescriptionTemp As DropDownList
            Dim txtOrdinalTemp As TextBox
            Dim txtPiecesPerHourTemp As TextBox
            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddDescriptionTemp = CType(gvAdditionalOfflineRate.FooterRow.FindControl("ddFooterAdditionalOfflineRateLaborItem"), DropDownList)

                If ddDescriptionTemp.SelectedIndex > 0 Then
                    txtOrdinalTemp = CType(gvAdditionalOfflineRate.FooterRow.FindControl("txtFooterAdditionalOfflineRateOrdinal"), TextBox)
                    txtPiecesPerHourTemp = CType(gvAdditionalOfflineRate.FooterRow.FindControl("txtFooterAdditionalOfflineRatePiecesPerHour"), TextBox)

                    odsCostSheetAdditionalOfflineRate.InsertParameters("costSheetID").DefaultValue = ViewState("CostSheetID")
                    odsCostSheetAdditionalOfflineRate.InsertParameters("laborID").DefaultValue = ddDescriptionTemp.SelectedValue
                    odsCostSheetAdditionalOfflineRate.InsertParameters("piecesPerHour").DefaultValue = txtPiecesPerHourTemp.Text
                    odsCostSheetAdditionalOfflineRate.InsertParameters("ordinal").DefaultValue = txtOrdinalTemp.Text

                    intRowsAffected = odsCostSheetAdditionalOfflineRate.Insert()
                Else
                    lblMessage.Text += "Error: no labor was selected to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvAdditionalOfflineRate.ShowFooter = False
            Else
                gvAdditionalOfflineRate.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddDescriptionTemp = CType(gvAdditionalOfflineRate.FooterRow.FindControl("ddFooterAdditionalOfflineRateLaborItem"), DropDownList)
                ddDescriptionTemp.SelectedIndex = -1

                txtPiecesPerHourTemp = CType(gvAdditionalOfflineRate.FooterRow.FindControl("txtFooterAdditionalOfflineRatePiecesPerHour"), TextBox)
                txtPiecesPerHourTemp.Text = Nothing

                txtOrdinalTemp = CType(gvAdditionalOfflineRate.FooterRow.FindControl("txtFooterAdditionalOfflineRateOrdinal"), TextBox)
                txtOrdinalTemp.Text = Nothing
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvMaterials_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMaterial.DataBound

        'hide header of first column
        If gvMaterial.Rows.Count > 0 Then
            gvMaterial.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvMaterials_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMaterial.RowCommand

        Try

            Dim ddMaterialIDTemp As DropDownList
            Dim txtMaterialQuantityTemp As TextBox
            Dim txtMaterialUsageFactorTemp As TextBox
            Dim txtMaterialCostPerUnitTemp As TextBox
            Dim txtMaterialFreightCostTemp As TextBox
            Dim txtMaterialStandardCostFactorTemp As TextBox

            Dim txtMaterialOrdinalTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddMaterialIDTemp = CType(gvMaterial.FooterRow.FindControl("ddFooterMaterial"), DropDownList)

                If ddMaterialIDTemp.SelectedIndex > 0 Then
                    txtMaterialQuantityTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialQuantity"), TextBox)
                    txtMaterialUsageFactorTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialUsageFactor"), TextBox)
                    txtMaterialCostPerUnitTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialCostPerUnit"), TextBox)
                    txtMaterialFreightCostTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialFreightCost"), TextBox)
                    txtMaterialStandardCostFactorTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialStandardCostFactor"), TextBox)
                    txtMaterialOrdinalTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialOrdinal"), TextBox)

                    odsCostSheetMaterial.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                    odsCostSheetMaterial.InsertParameters("MaterialID").DefaultValue = ddMaterialIDTemp.SelectedValue
                    odsCostSheetMaterial.InsertParameters("Quantity").DefaultValue = txtMaterialQuantityTemp.Text
                    odsCostSheetMaterial.InsertParameters("UsageFactor").DefaultValue = txtMaterialUsageFactorTemp.Text
                    odsCostSheetMaterial.InsertParameters("CostPerUnit").DefaultValue = txtMaterialCostPerUnitTemp.Text
                    odsCostSheetMaterial.InsertParameters("FreightCost").DefaultValue = txtMaterialFreightCostTemp.Text
                    odsCostSheetMaterial.InsertParameters("StandardCostFactor").DefaultValue = txtMaterialStandardCostFactorTemp.Text
                    odsCostSheetMaterial.InsertParameters("Ordinal").DefaultValue = txtMaterialOrdinalTemp.Text

                    intRowsAffected = odsCostSheetMaterial.Insert()
                Else
                    lblMessage.Text += "Error: no material was selected to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvMaterial.ShowFooter = False
            Else
                gvMaterial.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then

                ddMaterialIDTemp = CType(gvMaterial.FooterRow.FindControl("ddFooterMaterial"), DropDownList)
                ddMaterialIDTemp.SelectedIndex = -1

                txtMaterialQuantityTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialQuantity"), TextBox)
                txtMaterialQuantityTemp.Text = ""

                txtMaterialUsageFactorTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialUsageFactor"), TextBox)
                txtMaterialUsageFactorTemp.Text = ""

                txtMaterialCostPerUnitTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialCostPerUnit"), TextBox)
                txtMaterialCostPerUnitTemp.Text = ""

                txtMaterialFreightCostTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialFreightCost"), TextBox)
                txtMaterialFreightCostTemp.Text = ""

                txtMaterialStandardCostFactorTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialStandardCostFactor"), TextBox)
                txtMaterialStandardCostFactorTemp.Text = ""

                txtMaterialOrdinalTemp = CType(gvMaterial.FooterRow.FindControl("txtFooterMaterialOrdinal"), TextBox)
                txtMaterialOrdinalTemp.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvPackaging_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPackaging.DataBound

        'hide header of first column
        If gvPackaging.Rows.Count > 0 Then
            gvPackaging.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvPackaging_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPackaging.RowCommand

        Try

            Dim ddPackagingIDTemp As DropDownList
            Dim txtPackagingCostPerUnitTemp As TextBox
            Dim txtPackagingUnitsNeededTemp As TextBox
            Dim txtPackagingPartsPerContainerTemp As TextBox
            Dim txtPackagingStandardCostFactorTemp As TextBox
            Dim txtPackagingIsUsedTemp As CheckBox
            Dim txtPackagingOrdinalTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddPackagingIDTemp = CType(gvPackaging.FooterRow.FindControl("ddFooterPackaging"), DropDownList)

                If ddPackagingIDTemp.SelectedIndex > 0 Then
                    txtPackagingCostPerUnitTemp = CType(gvPackaging.FooterRow.FindControl("txtFooterPackagingCostPerUnit"), TextBox)
                    txtPackagingUnitsNeededTemp = CType(gvPackaging.FooterRow.FindControl("txtFooterPackagingUnitsNeeded"), TextBox)
                    txtPackagingPartsPerContainerTemp = CType(gvPackaging.FooterRow.FindControl("txtFooterPackagingPartsPerContainer"), TextBox)
                    txtPackagingStandardCostFactorTemp = CType(gvPackaging.FooterRow.FindControl("txtFooterPackagingStandardCostFactor"), TextBox)
                    txtPackagingIsUsedTemp = CType(gvPackaging.FooterRow.FindControl("chkFooterPackagingIsUsed"), CheckBox)
                    txtPackagingOrdinalTemp = CType(gvPackaging.FooterRow.FindControl("txtFooterPackagingOrdinal"), TextBox)

                    odsCostSheetPackaging.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                    odsCostSheetPackaging.InsertParameters("MaterialID").DefaultValue = ddPackagingIDTemp.SelectedValue
                    odsCostSheetPackaging.InsertParameters("CostPerUnit").DefaultValue = txtPackagingCostPerUnitTemp.Text
                    odsCostSheetPackaging.InsertParameters("UnitsNeeded").DefaultValue = txtPackagingUnitsNeededTemp.Text
                    odsCostSheetPackaging.InsertParameters("PartsPerContainer").DefaultValue = txtPackagingPartsPerContainerTemp.Text
                    odsCostSheetPackaging.InsertParameters("StandardCostFactor").DefaultValue = txtPackagingStandardCostFactorTemp.Text
                    odsCostSheetPackaging.InsertParameters("isUsed").DefaultValue = txtPackagingIsUsedTemp.Checked
                    odsCostSheetPackaging.InsertParameters("Ordinal").DefaultValue = txtPackagingOrdinalTemp.Text

                    intRowsAffected = odsCostSheetPackaging.Insert()
                Else
                    lblMessage.Text += "Error: no packaging material was selected to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvPackaging.ShowFooter = False
            Else
                gvPackaging.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddPackagingIDTemp = CType(gvPackaging.FooterRow.FindControl("ddFooterPackaging"), DropDownList)
                ddPackagingIDTemp.SelectedIndex = -1

                txtPackagingCostPerUnitTemp = CType(gvPackaging.FooterRow.FindControl("txtFooterPackagingCostPerUnit"), TextBox)
                txtPackagingCostPerUnitTemp.Text = ""

                txtPackagingUnitsNeededTemp = CType(gvPackaging.FooterRow.FindControl("txtFooterPackagingUnitsNeeded"), TextBox)
                txtPackagingUnitsNeededTemp.Text = ""

                txtPackagingPartsPerContainerTemp = CType(gvPackaging.FooterRow.FindControl("txtFooterPackagingPartsPerContainer"), TextBox)
                txtPackagingPartsPerContainerTemp.Text = ""

                txtPackagingIsUsedTemp = CType(gvPackaging.FooterRow.FindControl("chkFooterPackagingIsUsed"), CheckBox)
                txtPackagingIsUsedTemp.Checked = False

                txtPackagingOrdinalTemp = CType(gvPackaging.FooterRow.FindControl("txtFooterPackagingOrdinal"), TextBox)
                txtPackagingOrdinalTemp.Text = ""
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvLabor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvLabor.DataBound

        'hide header of first column
        If gvLabor.Rows.Count > 0 Then
            gvLabor.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvLabor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLabor.RowCommand

        Try

            Dim ddLaborIDTemp As DropDownList
            Dim txtLaborRateTemp As TextBox
            Dim txtLaborCrewSizeTemp As TextBox
            Dim txtLaborStandardCostFactorTemp As TextBox
            Dim cbLaborIsOfflineTemp As CheckBox
            Dim txtLaborOrdinalTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddLaborIDTemp = CType(gvLabor.FooterRow.FindControl("ddFooterLabor"), DropDownList)

                If ddLaborIDTemp.SelectedIndex > 0 Then
                    txtLaborRateTemp = CType(gvLabor.FooterRow.FindControl("txtFooterLaborRate"), TextBox)
                    txtLaborCrewSizeTemp = CType(gvLabor.FooterRow.FindControl("txtFooterLaborCrewSize"), TextBox)
                    txtLaborStandardCostFactorTemp = CType(gvLabor.FooterRow.FindControl("txtFooterLaborStandardCostFactor"), TextBox)
                    cbLaborIsOfflineTemp = CType(gvLabor.FooterRow.FindControl("cbFooterLaborIsOffline"), CheckBox)
                    txtLaborOrdinalTemp = CType(gvLabor.FooterRow.FindControl("txtFooterLaborOrdinal"), TextBox)

                    odsCostSheetLabor.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                    odsCostSheetLabor.InsertParameters("LaborID").DefaultValue = ddLaborIDTemp.SelectedValue
                    odsCostSheetLabor.InsertParameters("Rate").DefaultValue = txtLaborRateTemp.Text
                    odsCostSheetLabor.InsertParameters("CrewSize").DefaultValue = txtLaborCrewSizeTemp.Text
                    odsCostSheetLabor.InsertParameters("StandardCostFactor").DefaultValue = txtLaborStandardCostFactorTemp.Text
                    odsCostSheetLabor.InsertParameters("IsOffline").DefaultValue = cbLaborIsOfflineTemp.Checked
                    odsCostSheetLabor.InsertParameters("Ordinal").DefaultValue = txtLaborOrdinalTemp.Text

                    intRowsAffected = odsCostSheetLabor.Insert()
                Else
                    lblMessage.Text += "Error: no labor was selected to insert."
                End If
            End If
            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvLabor.ShowFooter = False
            Else
                gvLabor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddLaborIDTemp = CType(gvLabor.FooterRow.FindControl("ddFooterLabor"), DropDownList)
                ddLaborIDTemp.SelectedIndex = -1

                txtLaborRateTemp = CType(gvLabor.FooterRow.FindControl("txtFooterLaborRate"), TextBox)
                txtLaborRateTemp.Text = ""

                txtLaborCrewSizeTemp = CType(gvLabor.FooterRow.FindControl("txtFooterLaborCrewSize"), TextBox)
                txtLaborCrewSizeTemp.Text = ""

                txtLaborStandardCostFactorTemp = CType(gvLabor.FooterRow.FindControl("txtFooterLaborStandardCostFactor"), TextBox)
                txtLaborStandardCostFactorTemp.Text = ""

                cbLaborIsOfflineTemp = CType(gvLabor.FooterRow.FindControl("cbFooterLaborIsOffline"), CheckBox)
                cbLaborIsOfflineTemp.Checked = False

                txtLaborOrdinalTemp = CType(gvLabor.FooterRow.FindControl("txtFooterLaborOrdinal"), TextBox)
                txtLaborOrdinalTemp.Text = ""
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvOverhead_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvOverhead.DataBound

        'hide header of first column
        If gvOverhead.Rows.Count > 0 Then
            gvOverhead.HeaderRow.Cells(0).Visible = False
            gvOverhead.HeaderRow.Cells(8).Visible = False
            gvOverhead.HeaderRow.Cells(9).Visible = False
            gvOverhead.HeaderRow.Cells(12).Visible = False
            gvOverhead.HeaderRow.Cells(13).Visible = False
        End If

    End Sub
    Protected Sub gvOverhead_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvOverhead.RowCommand

        Try

            Dim ddOverheadIDTemp As DropDownList
            Dim txtOverheadRateTemp As TextBox
            Dim txtOverheadVariableRateTemp As TextBox
            Dim txtOverheadCrewSizeTemp As TextBox
            Dim txtOverheadStandardCostFactorTemp As TextBox
            Dim cbOverheadIsOfflineTemp As CheckBox
            Dim cbOverheadIsProportionTemp As CheckBox
            Dim txtOverheadOrdinalTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddOverheadIDTemp = CType(gvOverhead.FooterRow.FindControl("ddFooterOverhead"), DropDownList)

                If ddOverheadIDTemp.SelectedIndex > 0 Then
                    txtOverheadRateTemp = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadRate"), TextBox)
                    txtOverheadVariableRateTemp = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadVariableRate"), TextBox)
                    txtOverheadCrewSizeTemp = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadCrewSize"), TextBox)
                    txtOverheadStandardCostFactorTemp = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadStandardCostFactor"), TextBox)
                    cbOverheadIsOfflineTemp = CType(gvOverhead.FooterRow.FindControl("cbFooterOverheadIsOffline"), CheckBox)
                    cbOverheadIsProportionTemp = CType(gvOverhead.FooterRow.FindControl("cbFooterOverheadIsProportion"), CheckBox)
                    txtOverheadOrdinalTemp = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadOrdinal"), TextBox)

                    odsCostSheetOverhead.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                    odsCostSheetOverhead.InsertParameters("LaborID").DefaultValue = ddOverheadIDTemp.SelectedValue
                    odsCostSheetOverhead.InsertParameters("Rate").DefaultValue = txtOverheadRateTemp.Text
                    odsCostSheetOverhead.InsertParameters("VariableRate").DefaultValue = txtOverheadVariableRateTemp.Text
                    odsCostSheetOverhead.InsertParameters("CrewSize").DefaultValue = txtOverheadCrewSizeTemp.Text
                    odsCostSheetOverhead.InsertParameters("StandardCostFactor").DefaultValue = txtOverheadStandardCostFactorTemp.Text
                    odsCostSheetOverhead.InsertParameters("IsOffline").DefaultValue = cbOverheadIsOfflineTemp.Checked
                    odsCostSheetOverhead.InsertParameters("IsProportion").DefaultValue = cbOverheadIsProportionTemp.Checked
                    odsCostSheetOverhead.InsertParameters("Ordinal").DefaultValue = txtOverheadOrdinalTemp.Text

                    intRowsAffected = odsCostSheetOverhead.Insert()
                Else
                    lblMessage.Text += "Error: no overhead was selected to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvOverhead.ShowFooter = False
            Else
                gvOverhead.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddOverheadIDTemp = CType(gvOverhead.FooterRow.FindControl("ddFooterOverhead"), DropDownList)
                ddOverheadIDTemp.SelectedIndex = -1

                txtOverheadRateTemp = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadRate"), TextBox)
                txtOverheadRateTemp.Text = ""

                txtOverheadVariableRateTemp = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadVariableRate"), TextBox)
                txtOverheadVariableRateTemp.Text = ""

                txtOverheadCrewSizeTemp = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadCrewSize"), TextBox)
                txtOverheadCrewSizeTemp.Text = ""

                txtOverheadStandardCostFactorTemp = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadStandardCostFactor"), TextBox)
                txtOverheadStandardCostFactorTemp.Text = ""

                cbOverheadIsOfflineTemp = CType(gvOverhead.FooterRow.FindControl("cbFooterOverheadIsOffline"), CheckBox)
                cbOverheadIsOfflineTemp.Checked = False

                cbOverheadIsProportionTemp = CType(gvOverhead.FooterRow.FindControl("cbFooterOverheadIsProportion"), CheckBox)
                cbOverheadIsProportionTemp.Checked = False

                txtOverheadOrdinalTemp = CType(gvOverhead.FooterRow.FindControl("txtFooterOverheadOrdinal"), TextBox)
                txtOverheadOrdinalTemp.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvMiscCost_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMiscCost.DataBound

        'hide header of first column
        If gvMiscCost.Rows.Count > 0 Then
            gvMiscCost.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvMiscCost_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMiscCost.RowCommand

        Try

            Dim ddMiscCostIDTemp As DropDownList
            Dim txtMiscCostRateTemp As TextBox
            'Dim txtMiscCostQuoteRateTemp As TextBox
            Dim txtMiscCostTemp As TextBox
            Dim txtMiscCostAmortVolumeTemp As TextBox
            'Dim cbMiscCostIsPiecesPerHourTemp As CheckBox
            'Dim cbMiscCostIsPiecesPerYearTemp As CheckBox
            'Dim cbMiscCostIsPiecesPerContainerTemp As CheckBox
            'Dim txtMiscCostPiecesTemp As TextBox
            Dim txtMiscCostOrdinalTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddMiscCostIDTemp = CType(gvMiscCost.FooterRow.FindControl("ddFooterMiscCostID"), DropDownList)

                If ddMiscCostIDTemp.SelectedIndex > 0 Then
                    txtMiscCostRateTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCostRate"), TextBox)
                    'txtMiscCostQuoteRateTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCostQuoteRate"), TextBox)
                    txtMiscCostTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCost"), TextBox)
                    txtMiscCostAmortVolumeTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCostAmortVolume"), TextBox)
                    'cbMiscCostIsPiecesPerHourTemp = CType(gvMiscCost.FooterRow.FindControl("cbFooterMiscCostIsPiecesPerHour"), CheckBox)
                    'cbMiscCostIsPiecesPerYearTemp = CType(gvMiscCost.FooterRow.FindControl("cbFooterMiscCostIsPiecesPerYear"), CheckBox)
                    'cbMiscCostIsPiecesPerContainerTemp = CType(gvMiscCost.FooterRow.FindControl("cbFooterMiscCostIsPiecesPerContainer"), CheckBox)
                    'txtMiscCostPiecesTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCostPieces"), TextBox)
                    txtMiscCostOrdinalTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCostOrdinal"), TextBox)

                    odsCostSheetMiscCost.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                    odsCostSheetMiscCost.InsertParameters("MiscCostID").DefaultValue = ddMiscCostIDTemp.SelectedValue
                    odsCostSheetMiscCost.InsertParameters("Rate").DefaultValue = txtMiscCostRateTemp.Text
                    'odsCostSheetMiscCost.InsertParameters("QuoteRate").DefaultValue = txtMiscCostQuoteRateTemp.Text
                    odsCostSheetMiscCost.InsertParameters("Cost").DefaultValue = txtMiscCostTemp.Text
                    odsCostSheetMiscCost.InsertParameters("AmortVolume").DefaultValue = txtMiscCostAmortVolumeTemp.Text
                    'odsCostSheetMiscCost.InsertParameters("IsPiecesPerHour").DefaultValue = cbMiscCostIsPiecesPerHourTemp.Checked
                    'odsCostSheetMiscCost.InsertParameters("IsPiecesPerYear").DefaultValue = cbMiscCostIsPiecesPerYearTemp.Checked
                    'odsCostSheetMiscCost.InsertParameters("IsPiecesPerContainer").DefaultValue = cbMiscCostIsPiecesPerContainerTemp.Checked
                    'odsCostSheetMiscCost.InsertParameters("Pieces").DefaultValue = txtMiscCostPiecesTemp.Text
                    odsCostSheetMiscCost.InsertParameters("Ordinal").DefaultValue = txtMiscCostOrdinalTemp.Text

                    intRowsAffected = odsCostSheetMiscCost.Insert()
                Else
                    lblMessage.Text += "Error: no misc cost was selected to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvMiscCost.ShowFooter = False
            Else
                gvMiscCost.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddMiscCostIDTemp = CType(gvMiscCost.FooterRow.FindControl("ddFooterMiscCostID"), DropDownList)
                ddMiscCostIDTemp.SelectedIndex = -1

                txtMiscCostRateTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCostRate"), TextBox)
                txtMiscCostRateTemp.Text = Nothing

                'txtMiscCostQuoteRateTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCostQuoteRate"), TextBox)
                'txtMiscCostQuoteRateTemp.Text = Nothing

                txtMiscCostTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCost"), TextBox)
                txtMiscCostTemp.Text = Nothing

                txtMiscCostAmortVolumeTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCostAmortVolume"), TextBox)
                txtMiscCostAmortVolumeTemp.Text = Nothing

                'cbMiscCostIsPiecesPerHourTemp = CType(gvMiscCost.FooterRow.FindControl("cbFooterMiscCostIsPiecesPerHour"), CheckBox)
                'cbMiscCostIsPiecesPerHourTemp.Checked = False

                'cbMiscCostIsPiecesPerYearTemp = CType(gvMiscCost.FooterRow.FindControl("cbFooterMiscCostIsPiecesPerYear"), CheckBox)
                'cbMiscCostIsPiecesPerYearTemp.Checked = False

                'cbMiscCostIsPiecesPerContainerTemp = CType(gvMiscCost.FooterRow.FindControl("cbFooterMiscCostIsPiecesPerContainer"), CheckBox)
                'cbMiscCostIsPiecesPerContainerTemp.Checked = False

                'txtMiscCostPiecesTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCostPieces"), TextBox)
                'txtMiscCostPiecesTemp.Text = Nothing

                txtMiscCostOrdinalTemp = CType(gvMiscCost.FooterRow.FindControl("txtFooterMiscCostOrdinal"), TextBox)
                txtMiscCostOrdinalTemp.Text = Nothing
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text

    End Sub

    Protected Sub iBtnGetRFDinfo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnGetRFDinfo.Click

        Try
            ClearMessages()

            'bind existing RFD info to Cost Sheet   

            'if this is a brand new Cost Sheet, then save it first
            If ViewState("CostSheetID") = 0 Then
                btnSave_Click(sender, e)
            End If

            Dim ds As DataSet

            Dim dtChildPart As DataTable
            Dim objRFDChildPartBLL As RFDChildPartBLL = New RFDChildPartBLL

            Dim dtCustomerProgram As DataTable
            Dim objRFDCustomerProgramBLL As RFDCustomerProgramBLL = New RFDCustomerProgramBLL

            Dim dtFacilityDept As DataTable
            Dim objRFDFacilityDeptBLL As RFDFacilityDeptBLL = New RFDFacilityDeptBLL

            Dim iRFDNo As Integer = 0
            Dim iChildRowID As Integer = 0

            Dim iRowCounter As Integer = 0
            ' ''Dim iSoldTo As Integer = 0
            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0

            ' ''Dim strCABBV As String = ""

            If txtRFDNoValue.Text.Trim <> "" Then
                iRFDNo = CType(txtRFDNoValue.Text.Trim, Integer)

                If iRFDNo > 0 Then
                    ds = RFDModule.GetRFDCostingSearch(txtRFDNoValue.Text.Trim, "", 0, 0, "", "", "", "")

                    If commonFunctions.CheckDataSet(ds) = True Then
                        If txtRFDSelectionType.Text <> "CP" Then '= "TL" Then 'top level
                            'push CostSheetID back to RFD Top Level
                            RFDModule.UpdateRFDFromCosting(iRFDNo, ViewState("CostSheetID"))

                            ddDesignationTypeValue.SelectedValue = ds.Tables(0).Rows(0).Item("NewTopLevelDesignationType").ToString

                            If ds.Tables(0).Rows(0).Item("NewTopLevelNewCommodityID") IsNot System.DBNull.Value Then
                                If ds.Tables(0).Rows(0).Item("NewTopLevelNewCommodityID") > 0 Then
                                    ddCommodityValue.SelectedValue = ds.Tables(0).Rows(0).Item("NewTopLevelNewCommodityID")
                                End If
                            End If

                            txtNewCustomerPartNoValue.Text = ds.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString
                            txtNewDesignLevelValue.Text = ds.Tables(0).Rows(0).Item("NewDesignLevel").ToString
                            txtNewDrawingNoValue.Text = ds.Tables(0).Rows(0).Item("NewTopLevelDrawingNo").ToString
                            txtNewPartNameValue.Text = ds.Tables(0).Rows(0).Item("NewCustomerPartName").ToString
                        End If

                        If txtRFDSelectionType.Text.Trim = "CP" And txtRFDChildRow.Text.Trim <> "" Then 'childpart

                            iChildRowID = CType(txtRFDChildRow.Text.Trim, Integer)

                            If iChildRowID > 0 Then
                                'push CostSheetID back to RFD Child Part
                                RFDModule.UpdateRFDChildPartFromCosting(iChildRowID, iRFDNo, ViewState("CostSheetID"))

                                dtChildPart = objRFDChildPartBLL.GetRFDChildPart(iChildRowID, iRFDNo)

                                If commonFunctions.CheckDataTable(dtChildPart) = True Then
                                    ddDesignationTypeValue.SelectedValue = dtChildPart.Rows(0).Item("NewDesignationType").ToString
                                    txtNewDrawingNoValue.Text = dtChildPart.Rows(0).Item("NewDrawingNo").ToString
                                    txtNewPartNameValue.Text = dtChildPart.Rows(0).Item("NewPartName").ToString

                                    If dtChildPart.Rows(0).Item("NewPurchasedGoodID") IsNot System.DBNull.Value Then
                                        If dtChildPart.Rows(0).Item("NewPurchasedGoodID") > 0 Then
                                            ddPurchasedGoodValue.SelectedValue = dtChildPart.Rows(0).Item("NewPurchasedGoodID")
                                        End If
                                    End If

                                    txtNewPartNoValue.Text = dtChildPart.Rows(0).Item("NewPartNo").ToString
                                    txtNewPartRevisionValue.Text = dtChildPart.Rows(0).Item("NewPartRevision").ToString

                                End If
                            End If
                        End If

                        'append to customer program list
                        dtCustomerProgram = objRFDCustomerProgramBLL.GetRFDCustomerProgram(iRFDNo)

                        If commonFunctions.CheckDataTable(dtCustomerProgram) = True Then

                            For iRowCounter = 0 To dtCustomerProgram.Rows.Count - 1
                                'strCABBV = ""
                                'If dtCustomerProgram.Rows(iRowCounter).Item("CABBV").ToString <> "" Then
                                '    strCABBV = dtCustomerProgram.Rows(iRowCounter).Item("CABBV").ToString
                                'End If

                                'iSoldTo = 0
                                'If dtCustomerProgram.Rows(iRowCounter).Item("Soldto") IsNot System.DBNull.Value Then
                                '    If dtCustomerProgram.Rows(iRowCounter).Item("SoldTo") > 0 Then
                                '        iSoldTo = dtCustomerProgram.Rows(iRowCounter).Item("SoldTo")
                                '    End If
                                'End If

                                iProgramID = 0
                                If dtCustomerProgram.Rows(iRowCounter).Item("ProgramID") IsNot System.DBNull.Value Then
                                    If dtCustomerProgram.Rows(iRowCounter).Item("ProgramID") > 0 Then
                                        iProgramID = dtCustomerProgram.Rows(iRowCounter).Item("ProgramID")
                                    End If
                                End If

                                iProgramYear = 0
                                If dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear") IsNot System.DBNull.Value Then
                                    If dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear") > 0 Then
                                        iProgramYear = dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear")
                                    End If
                                End If

                                If iProgramID > 0 Then
                                    CostingModule.InsertCostSheetCustomerProgram(ViewState("CostSheetID"), "", 0, iProgramID, iProgramYear)
                                End If

                            Next
                        End If

                        'get the first facility in the list
                        dtFacilityDept = objRFDFacilityDeptBLL.GetRFDFacilityDept(iRFDNo)

                        If commonFunctions.CheckDataTable(dtFacilityDept) = True Then
                            ddUGNFacilityValue.SelectedValue = dtFacilityDept.Rows(0).Item("UGNFacility").ToString
                        End If

                        btnSave_Click(sender, e)

                        'reload page
                        CheckDesignationType()
                        gvCustomerProgram.DataBind()

                        lblMessage.Text &= "Information successfully copied from RFD.<br>The RFD has been updated to reference this Cost Sheet."
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvCapital_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCapital.DataBound

        'hide header of first and second columns
        If gvCapital.Rows.Count > 0 Then
            gvCapital.HeaderRow.Cells(0).Visible = False
        End If

    End Sub
    Protected Sub gvCapital_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCapital.RowCommand

        Try

            Dim ddCapitalIDTemp As DropDownList
            Dim txtCapitalTotalDollarAmountTemp As TextBox
            Dim txtCapitalYearsOfDepreciationTemp As TextBox
            Dim txtCapitalAnnualVolumeTemp As TextBox
            'Dim txtCapitalPerPieceTemp As TextBox
            'Dim txtCapitalHoldMoldAmountTemp As TextBox
            Dim txtCapitalOverheadAmountTemp As TextBox
            'Dim txtCapitalHourlyCapitalRateTemp As TextBox
            'Dim txtCapitalOverheadRateTemp As TextBox
            Dim cbCapitalIsOfflineTemp As CheckBox
            Dim cbCapitalisInlineTemp As CheckBox
            Dim txtCapitalOrdinalTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddCapitalIDTemp = CType(gvCapital.FooterRow.FindControl("ddFooterCapital"), DropDownList)

                If ddCapitalIDTemp.SelectedIndex > 0 Then
                    txtCapitalTotalDollarAmountTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalTotalDollarAmount"), TextBox)
                    txtCapitalYearsOfDepreciationTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalYearsOfDepreciation"), TextBox)
                    txtCapitalAnnualVolumeTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalAnnualVolume"), TextBox)
                    'txtCapitalPerPieceTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalPerPiece"), TextBox)
                    'txtCapitalHoldMoldAmountTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalHoldMoldAmount"), TextBox)
                    txtCapitalOverheadAmountTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalOverheadAmount"), TextBox)
                    'txtCapitalHourlyCapitalRateTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalHourlyCapitalRate"), TextBox)
                    'txtCapitalOverheadRateTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalOverheadRate"), TextBox)
                    cbCapitalIsOfflineTemp = CType(gvCapital.FooterRow.FindControl("cbFooterCapitalIsOffline"), CheckBox)
                    cbCapitalisInlineTemp = CType(gvCapital.FooterRow.FindControl("cbFooterCapitalisInline"), CheckBox)
                    txtCapitalOrdinalTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalOrdinal"), TextBox)

                    odsCostSheetCapital.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                    odsCostSheetCapital.InsertParameters("CapitalID").DefaultValue = ddCapitalIDTemp.SelectedValue
                    odsCostSheetCapital.InsertParameters("TotalDollarAmount").DefaultValue = txtCapitalTotalDollarAmountTemp.Text
                    odsCostSheetCapital.InsertParameters("YearsOfDepreciation").DefaultValue = txtCapitalYearsOfDepreciationTemp.Text
                    odsCostSheetCapital.InsertParameters("CapitalAnnualVolume").DefaultValue = txtCapitalAnnualVolumeTemp.Text
                    'odsCostSheetCapital.InsertParameters("PerPiece").DefaultValue = txtCapitalPerPieceTemp.Text
                    'odsCostSheetCapital.InsertParameters("HoldMoldAmount").DefaultValue = txtCapitalHoldMoldAmountTemp.Text
                    odsCostSheetCapital.InsertParameters("OverheadAmount").DefaultValue = txtCapitalOverheadAmountTemp.Text
                    'odsCostSheetCapital.InsertParameters("HourlyCapitalRate").DefaultValue = txtCapitalHourlyCapitalRateTemp.Text
                    'odsCostSheetCapital.InsertParameters("OverheadRate").DefaultValue = txtCapitalOverheadRateTemp.Text
                    odsCostSheetCapital.InsertParameters("isOffline").DefaultValue = cbCapitalIsOfflineTemp.Checked
                    odsCostSheetCapital.InsertParameters("isInline").DefaultValue = cbCapitalisInlineTemp.Checked
                    odsCostSheetCapital.InsertParameters("Ordinal").DefaultValue = txtCapitalOrdinalTemp.Text

                    intRowsAffected = odsCostSheetCapital.Insert()
                Else
                    lblMessage.Text += "Error: no capital was selected to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvCapital.ShowFooter = False
            Else
                gvCapital.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddCapitalIDTemp = CType(gvCapital.FooterRow.FindControl("ddFooterCapital"), DropDownList)
                ddCapitalIDTemp.SelectedIndex = -1

                txtCapitalTotalDollarAmountTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalTotalDollarAmount"), TextBox)
                txtCapitalTotalDollarAmountTemp.Text = ""

                txtCapitalYearsOfDepreciationTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalYearsOfDepreciation"), TextBox)
                txtCapitalYearsOfDepreciationTemp.Text = ""

                txtCapitalAnnualVolumeTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalAnnualVolume"), TextBox)
                txtCapitalAnnualVolumeTemp.Text = ""

                'txtCapitalPerPieceTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalPerPiece"), TextBox)
                'txtCapitalPerPieceTemp.Text = ""

                'txtCapitalHoldMoldAmountTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalHoldMoldAmount"), TextBox)
                'txtCapitalHoldMoldAmountTemp.Text = ""

                txtCapitalOverheadAmountTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalOverheadAmount"), TextBox)
                txtCapitalOverheadAmountTemp.Text = ""

                'txtCapitalHourlyCapitalRateTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalHourlyCapitalRate"), TextBox)
                'txtCapitalHourlyCapitalRateTemp.Text = ""

                'txtCapitalOverheadRateTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalOverheadRate"), TextBox)
                'txtCapitalOverheadRateTemp.Text = ""

                cbCapitalIsOfflineTemp = CType(gvCapital.FooterRow.FindControl("cbFooterCapitalIsOffline"), CheckBox)
                cbCapitalIsOfflineTemp.Checked = False

                cbCapitalisInlineTemp = CType(gvCapital.FooterRow.FindControl("cbFooterCapitalisInline"), CheckBox)
                cbCapitalisInlineTemp.Checked = False

                txtCapitalOrdinalTemp = CType(gvCapital.FooterRow.FindControl("txtFooterCapitalOrdinal"), TextBox)
                txtCapitalOrdinalTemp.Text = ""
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

#Region "Insert Empty GridView Work-Around"

    'Private Property LoadDataEmpty_CostSheetCustomerProgram() As Boolean
    '    ' From Andrew Robinson's Insert Empty GridView solution
    '    ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

    '    ' some controls that are used within a GridView,
    '    ' such as the calendar control, can cuase post backs.
    '    ' we need to preserve LoadDataEmpty across post backs.

    '    Get
    '        If ViewState("LoadDataEmpty_CostSheetCustomerProgram") IsNot Nothing Then
    '            Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CostSheetCustomerProgram"), Boolean)
    '            Return tmpBoolean
    '        Else
    '            Return False
    '        End If
    '    End Get
    '    Set(ByVal value As Boolean)
    '        ViewState("LoadDataEmpty_CostSheetCustomerProgram") = value
    '    End Set

    'End Property

    Private Property LoadDataEmpty_CostSheetTopLevelInfo() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CostSheetTopLevelInfo") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CostSheetTopLevelInfo"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CostSheetTopLevelInfo") = value
        End Set

    End Property
    Protected Sub odsCostSheetTopLevelInfo_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCostSheetTopLevelInfo.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetTopLevelBPCSPartInfo_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetTopLevelBPCSPartInfo_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CostSheetTopLevelInfo = True
            Else
                LoadDataEmpty_CostSheetTopLevelInfo = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub gvTopLevelInfo_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTopLevelInfo.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CostSheetTopLevelInfo
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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

    Private Property LoadDataEmpty_CostSheetAdditionalOfflineRate() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CostSheetAdditionalOfflineRate") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CostSheetAdditionalOfflineRate"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CostSheetAdditionalOfflineRate") = value
        End Set

    End Property
    Protected Sub odsCostSheetAdditionalOfflineRate_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCostSheetAdditionalOfflineRate.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            'Dim dt As Cells.Cell_MaintDataTable = CType(e.ReturnValue, Cells.Cell_MaintDataTable)
            Dim dt As Costing.CostSheetAdditionalOfflineRate_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetAdditionalOfflineRate_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CostSheetAdditionalOfflineRate = True
            Else
                LoadDataEmpty_CostSheetAdditionalOfflineRate = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub gvAdditionalOfflineRate_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAdditionalOfflineRate.RowCreated

        Try

            'hide first column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CostSheetAdditionalOfflineRate
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

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Private Property LoadDataEmpty_CostSheetMaterial() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CostSheetMaterial") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CostSheetMaterial"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CostSheetMaterial") = value
        End Set

    End Property
    Protected Sub odsCostSheetMaterial_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCostSheetMaterial.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetMaterial_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetMaterial_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CostSheetMaterial = True
            Else
                LoadDataEmpty_CostSheetMaterial = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub gvMaterial_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMaterial.RowCreated

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

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CostSheetMaterial
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

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Private Property LoadDataEmpty_CostSheetPackaging() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CostSheetPackaging") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CostSheetPackaging"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CostSheetPackaging") = value
        End Set

    End Property
    Protected Sub odsCostSheetPackaging_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCostSheetPackaging.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetPackaging_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetPackaging_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CostSheetPackaging = True
            Else
                LoadDataEmpty_CostSheetPackaging = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub gvPackaging_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPackaging.RowCreated

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

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CostSheetPackaging
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

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Private Property LoadDataEmpty_CostSheetLabor() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CostSheetLabor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CostSheetLabor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CostSheetLabor") = value
        End Set

    End Property
    Protected Sub odsCostSheetLabor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCostSheetLabor.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetLabor_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetLabor_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CostSheetLabor = True
            Else
                LoadDataEmpty_CostSheetLabor = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvLabor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLabor.RowCreated

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

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CostSheetLabor
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

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Private Property LoadDataEmpty_CostSheetOverhead() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CostSheetOverhead") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CostSheetOverhead"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CostSheetOverhead") = value
        End Set

    End Property
    Protected Sub odsCostSheetOverhead_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCostSheetOverhead.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetOverhead_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetOverhead_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CostSheetOverhead = True
            Else
                LoadDataEmpty_CostSheetOverhead = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub gvOverhead_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOverhead.RowCreated

        Try
            'hide first column
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(8).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(9).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(12).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(13).Attributes.CssStyle.Add("display", "none")
            End If

            'If e.Row.RowType = DataControlRowType.Footer Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            'End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CostSheetOverhead
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

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Private Property LoadDataEmpty_CostSheetMiscCost() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CostSheetMiscCost") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CostSheetMiscCost"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CostSheetMiscCost") = value
        End Set

    End Property
    Protected Sub odsCostSheetMiscCost_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCostSheetMiscCost.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetMiscCost_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetMiscCost_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CostSheetMiscCost = True
            Else
                LoadDataEmpty_CostSheetMiscCost = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub gvMiscCost_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMiscCost.RowCreated

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

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CostSheetMiscCost
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

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Private Property LoadDataEmpty_CostSheetCapital() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CostSheetCapital") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CostSheetCapital"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CostSheetCapital") = value
        End Set

    End Property
    Protected Sub odsCostSheetCapital_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCostSheetCapital.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetCapital_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetCapital_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CostSheetCapital = True
            Else
                LoadDataEmpty_CostSheetCapital = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub gvCapital_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCapital.RowCreated

        Try
            'hide first and second columns
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

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CostSheetCapital
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

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub
#End Region ' Insert Empty GridView Work-Around

    Private Sub InitializeViewState()

        Try

            ViewState("CostSheetID") = 0

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            ViewState("isApproved") = False
            ViewState("StatusChanged") = ""

            ViewState("Formula_SpecificGravity") = 0.0
            ViewState("Formula_MaxMixCapacity") = 0
            ViewState("Formula_MaxLineSpeed") = 0
            ViewState("Formula_MaxPressCycles") = 0
            ViewState("Formula_CoatingSides") = 0
            ViewState("Formula_WeightPerArea") = 0.0
            ViewState("Formula_MaxFormingRate") = 0
            ViewState("Formula_isDiecut") = False
            ViewState("Formula_ProcessID") = 0
            ViewState("Formula_isRecycleReturn") = False
            ViewState("Formula_TemplateID") = 0
            ViewState("Formula_isFleeceType") = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub


    Protected Sub btnCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalculate.Click

        Try
            ClearMessages()

            Dim dCatchingAbilityFactor As Double = 0
            Dim dProductionRatesCatchPercent As Double = 0
            Dim dCoatingFactor As Double = 0
            Dim iProductionLimitLineSpeedLimit As Integer = 0
            Dim iProductionLimitCycleLimit As Integer = 0
            Dim iProductionLimitMixCapacity As Integer = 0
            Dim iProductionLimitDeplugCapacity As Integer = 0
            Dim iProductionLimitCatchingAbilityCapacity As Integer = 0
            Dim iProductionLimitFormingRate As Integer = 0
            Dim iMinProductionLimit As Integer = 0

            Dim iMaxPiecesQuoted As Integer = 0
            Dim iMaxPiecesMaximum As Integer = 0

            Dim iPressCyclesMaximum As Integer = 0
            Dim iPressCyclesQuoted As Integer = 0

            Dim dLineSpeedMaximum As Double = 0
            Dim dLineSpeedQuoted As Double = 0

            Dim dNetFormingRateMaximum As Double = 0
            Dim dNetFormingRateQuoted As Double = 0

            Dim dMixCapacityMaximum As Double = 0
            Dim dMixCapacityQuoted As Double = 0

            Dim dRecycleRateMaximum As Double = 0
            Dim dRecycleRateQuoted As Double = 0

            Dim dPartWeightQuoted As Double = 0
            Dim dPartWeightMaximum As Double = 0

            Dim dCoatingWeightQuoted As Double = 0
            Dim dTotalWeightQuoted As Double = 0

            Dim dOfflineSpecificPiecesManHour As Double = 0
            Dim dLaborOfflineCrewSize As Double = 0
            Dim dOfflineSpecificPercentRecycle As Double = 0

            Dim iFormulaID As Integer = 0

            If ddPartSpecificationsFormulaValue.SelectedIndex > 0 Then
                iFormulaID = CType(ddPartSpecificationsFormulaValue.SelectedValue, Integer)
            End If

            Dim ds As DataSet
            Dim dTempProductionRatesMaxMixCapacity As Double = 0

            'get formula info
            Dim bFormulaFleeceType As Boolean = False
            Dim bFormulaRecycleReturn As Boolean = False
            Dim iFormulaLineSpeed As Integer = 0
            Dim iFormulaMaxPressCycles As Integer = 0
            Dim iFormulaBarrierPressCycles As Integer = 0
            Dim iFormulaTemplateID As Integer = 0
            Dim iFormulaMaxMixCapacity As Integer = 0
            Dim dFormulaMaxFormingRate As Double = 0
            Dim bForumlaUseBarrierRunRate As Boolean = False

            bFormulaFleeceType = ViewState("Formula_isFleeceType")
            bFormulaRecycleReturn = ViewState("Formula_isRecycleReturn")
            iFormulaLineSpeed = ViewState("Formula_MaxLineSpeed")
            iFormulaMaxPressCycles = ViewState("Formula_MaxPressCycles")
            'default to Formula Max Press Cycles
            iFormulaBarrierPressCycles = iFormulaMaxPressCycles
            iFormulaTemplateID = ViewState("Formula_TemplateID")
            iFormulaMaxMixCapacity = ViewState("Formula_MaxMixCapacity")
            dFormulaMaxFormingRate = ViewState("Formula_MaxFormingRate")

            'check if formula has hold deblug factors
            Dim iDeplugFactorCount As Integer = 0

            ds = CostingModule.GetFormulaDeplugFactorCount(iFormulaID)

            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("DeplugFactorCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("DeplugFactorCount") > 0 Then
                        iDeplugFactorCount = ds.Tables(0).Rows(0).Item("DeplugFactorCount")
                    End If
                End If
            End If

            'Get Fields From Web Page
            Dim iTempProductionRatesOfflineSpecificQuotedPressCycles As Integer = 0
            If txtProductionRatesOfflineSpecificQuotedPressCyclesValue.Text.Trim <> "" Then
                iTempProductionRatesOfflineSpecificQuotedPressCycles = CType(txtProductionRatesOfflineSpecificQuotedPressCyclesValue.Text, Integer)
            End If

            Dim iTempPartSpecificationsOffLineRate As Integer = 0
            If txtPartSpecificationsOffLineRateValue.Text.Trim <> "" Then
                iTempPartSpecificationsOffLineRate = CType(txtPartSpecificationsOffLineRateValue.Text, Integer)
            End If

            Dim dTempPartSpecificationsConfigurationFactor As Double = 0
            If txtPartSpecificationsConfigurationFactorValue.Text.Trim <> "" Then
                dTempPartSpecificationsConfigurationFactor = CType(txtPartSpecificationsConfigurationFactorValue.Text, Double)
            End If

            Dim dTempPartSpecificationsSpecificGravity As Double = 0
            If txtPartSpecificationsSpecificGravityValue.Text.Trim <> "" Then
                dTempPartSpecificationsSpecificGravity = CType(txtPartSpecificationsSpecificGravityValue.Text, Double)
            End If

            Dim dTempPartSpecificationsCalculatedArea As Double = 0
            If txtPartSpecificationsCalculatedAreaValue.Text.Trim <> "" Then
                dTempPartSpecificationsCalculatedArea = CType(txtPartSpecificationsCalculatedAreaValue.Text, Double)
            End If

            Dim dTempPartSpecificationsPiecesPerCycle As Double = 0
            If txtPartSpecificationsPiecesPerCycleValue.Text.Trim <> "" Then
                dTempPartSpecificationsPiecesPerCycle = CType(txtPartSpecificationsPiecesPerCycleValue.Text, Double)
            End If

            Dim dTempPartSpecificationsPartLength As Double = 0
            If txtPartSpecificationsPartLengthValue.Text.Trim <> "" Then
                dTempPartSpecificationsPartLength = CType(txtPartSpecificationsPartLengthValue.Text, Double)
            End If

            Dim dTempPartSpecificationsThickness As Double = 0
            If txtPartSpecificationsThicknessValue.Text.Trim <> "" Then
                dTempPartSpecificationsThickness = CType(txtPartSpecificationsThicknessValue.Text, Double)
            End If

            Dim dTempPartSpecificationsWeightPerArea As Double = 0
            If txtPartSpecificationsWeightPerAreaValue.Text.Trim <> "" Then
                dTempPartSpecificationsWeightPerArea = CType(txtPartSpecificationsWeightPerAreaValue.Text, Double)
            End If

            Dim dTempPartSpecificationsDieLayoutTravel As Double = 0
            If txtPartSpecificationsDieLayoutTravelValue.Text.Trim <> "" Then
                dTempPartSpecificationsDieLayoutTravel = CType(txtPartSpecificationsDieLayoutTravelValue.Text, Double)

                '2011-July-21 - New Barrier Calc logic
                Dim dtFormulaDepartment As DataTable
                Dim objFormulaDepartmentBLL As New FormulaDepartmentBLL
                Dim iFormulaDepartmentRowCounter = 0

                Dim dsFormulaBarierRunRate As DataSet

                dtFormulaDepartment = objFormulaDepartmentBLL.GetFormulaDepartment(iFormulaID)
                If commonFunctions.CheckDataTable(dtFormulaDepartment) = True Then
                    For iFormulaDepartmentRowCounter = 0 To dtFormulaDepartment.Rows.Count - 1
                        'find the HTS Barrier/RSS (114030) 
                        If dtFormulaDepartment.Rows(iFormulaDepartmentRowCounter).Item("DepartmentID") = 3 Then
                            'get new press cycles
                            dsFormulaBarierRunRate = CostingModule.GetFormulaBarrierRunRate(dTempPartSpecificationsDieLayoutTravel)

                            If commonFunctions.CheckDataSet(dsFormulaBarierRunRate) = True Then
                                If dsFormulaBarierRunRate.Tables(0).Rows(0).Item("PressCycles") IsNot System.DBNull.Value Then
                                    If dsFormulaBarierRunRate.Tables(0).Rows(0).Item("PressCycles") > 0 Then
                                        iFormulaBarrierPressCycles = dsFormulaBarierRunRate.Tables(0).Rows(0).Item("PressCycles")
                                        'ViewState("Formula_MaxPressCycles") = dsFormulaBarierRunRate.Tables(0).Rows(0).Item("PressCycles")
                                        bForumlaUseBarrierRunRate = True
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End If

            Dim dTempPartSpecificationsChangedArea As Double = 0
            If txtPartSpecificationsChangedAreaValue.Text.Trim <> "" Then
                dTempPartSpecificationsChangedArea = CType(txtPartSpecificationsChangedAreaValue.Text, Double)
            End If

            Dim dTempPartSpecificationsDieLayoutWidth As Double = 0
            If txtPartSpecificationsDieLayoutWidthValue.Text.Trim <> "" Then
                dTempPartSpecificationsDieLayoutWidth = txtPartSpecificationsDieLayoutWidthValue.Text
            End If

            Dim iTempPartSpecificationsNumberOfHoles As Integer = 0
            If txtPartSpecificationsNumberOfHolesValue.Text.Trim <> "" Then
                iTempPartSpecificationsNumberOfHoles = CType(txtPartSpecificationsNumberOfHolesValue.Text, Integer)
            End If

            Dim iTempPartSpecificationsPiecesCaughtTogether As Integer = 0
            If txtPartSpecificationsPiecesCaughtTogetherValue.Text.Trim <> "" Then
                iTempPartSpecificationsPiecesCaughtTogether = CType(txtPartSpecificationsPiecesCaughtTogetherValue.Text, Integer)
            End If

            Dim dTempPartSpecificationsFoamValue As Double = 0
            If txtPartSpecificationsFoamValue.Text.Trim <> "" Then
                dTempPartSpecificationsFoamValue = CType(txtPartSpecificationsFoamValue.Text, Double)
            End If

            Dim iTempProductionRatesMaxFormingRate As Integer = 0
            ''12/09/2009 DCade - override from Formula
            If ViewState("Formula_MaxFormingRate") > 0 Then
                iTempProductionRatesMaxFormingRate = ViewState("Formula_MaxFormingRate")
                txtProductionRatesMaxFormingRateValue.Text = iTempProductionRatesMaxFormingRate
            Else
                If txtProductionRatesMaxFormingRateValue.Text.Trim <> "" Then
                    iTempProductionRatesMaxFormingRate = CType(txtProductionRatesMaxFormingRateValue.Text, Integer)
                End If
            End If

            Dim iTempProductionRatesMaxFormingRateUnitID As Integer = 0
            If ddProductionRatesMaxFormingRateUnits.SelectedIndex > 0 Then
                iTempProductionRatesMaxFormingRateUnitID = ddProductionRatesMaxFormingRateUnits.SelectedValue
            End If

            Dim dTempQuotedInfoStandardCostFactor As Double = 0
            If txtQuotedInfoStandardCostFactor.Text.Trim <> "" Then
                dTempQuotedInfoStandardCostFactor = CType(txtQuotedInfoStandardCostFactor.Text, Double)
            End If

            Dim dTempPartSpecificationsProductionRateValue As Double = 0
            If txtPartSpecificationsProductionRateValue.Text.Trim <> "" Then
                dTempPartSpecificationsProductionRateValue = CType(txtPartSpecificationsProductionRateValue.Text, Double)
            End If

            Dim dTempPartSpecificationsNumberOfCarriers As Double = 0
            If txtPartSpecificationsNumberOfCarriersValue.Text.Trim <> "" Then
                dTempPartSpecificationsNumberOfCarriers = CType(txtPartSpecificationsNumberOfCarriersValue.Text, Double)
            End If

            Dim iTempQuotedInfoPiecesPerYear As Integer = 0
            If txtQuotedInfoPiecesPerYear.Text.Trim <> "" Then
                iTempQuotedInfoPiecesPerYear = CType(txtQuotedInfoPiecesPerYear.Text, Integer)
            End If

            Dim dTempProductionRatesWeightPerArea As Double = 0
            ''12/09/2009 DCade - override from Formula
            If ViewState("Formula_WeightPerArea") > 0 Then
                dTempProductionRatesWeightPerArea = ViewState("Formula_WeightPerArea")
                txtProductionRatesWeightPerAreaValue.Text = dTempProductionRatesWeightPerArea
            Else
                If txtProductionRatesWeightPerAreaValue.Text.Trim <> "" Then
                    dTempProductionRatesWeightPerArea = CType(txtProductionRatesWeightPerAreaValue.Text, Double)
                End If
            End If

            Dim iTempProductionRatesWeightPerAreaUnitID As Integer = 0
            If ddProductionRatesWeightPerAreaUnits.SelectedIndex > 0 Then
                iTempProductionRatesWeightPerAreaUnitID = ddProductionRatesWeightPerAreaUnits.SelectedValue
            End If

            Dim iTempProductionRatesOfflineSpecificSheetsUp As Integer = 0
            If txtProductionRatesOfflineSpecificSheetsUpValue.Text.Trim <> "" Then
                iTempProductionRatesOfflineSpecificSheetsUp = CType(txtProductionRatesOfflineSpecificSheetsUpValue.Text, Integer)
            End If

            Dim iTempProductionRatesOfflineSpecificQuotedOfflineRates As Integer = 0
            If txtProductionRatesOfflineSpecificQuotedOfflineRatesValue.Text.Trim <> "" Then
                iTempProductionRatesOfflineSpecificQuotedOfflineRates = CType(txtProductionRatesOfflineSpecificQuotedOfflineRatesValue.Text, Integer)
            End If

            Dim iTempProductionRatesMaxMixCapacity As Integer = 0
            ''12/09/2009 DCade - override from Formula
            If ViewState("Formula_MaxMixCapacity") > 0 Then
                iTempProductionRatesMaxMixCapacity = ViewState("Formula_MaxMixCapacity")
                txtProductionRatesMaxMixCapacityValue.Text = iTempProductionRatesMaxMixCapacity
            Else
                If txtProductionRatesMaxMixCapacityValue.Text.Trim <> "" Then
                    iTempProductionRatesMaxMixCapacity = CType(txtProductionRatesMaxMixCapacityValue.Text, Integer)
                End If
            End If

            Dim iTempProductionRatesMaxMixCapacityUnitID As Integer = 0
            If ddProductionRatesMaxMixCapacityUnits.SelectedIndex > 0 Then
                iTempProductionRatesMaxMixCapacityUnitID = ddProductionRatesMaxMixCapacityUnits.SelectedValue
            End If

            Dim dTempProductionRatesCatchingAbility As Double = 0
            If txtProductionRatesCatchingAbilityValue.Text.Trim <> "" Then
                dTempProductionRatesCatchingAbility = CType(txtProductionRatesCatchingAbilityValue.Text, Double)
            End If

            Dim iTempProductionRatesLineSpeedLimitation As Integer = 0
            ''12/09/2009 DCade - override from Formula
            If ViewState("Formula_MaxLineSpeed") > 0 Then
                iTempProductionRatesLineSpeedLimitation = ViewState("Formula_MaxLineSpeed")
                txtProductionRatesLineSpeedLimitationValue.Text = iTempProductionRatesLineSpeedLimitation
            Else
                If txtProductionRatesLineSpeedLimitationValue.Text.Trim <> "" Then
                    iTempProductionRatesLineSpeedLimitation = CType(txtProductionRatesLineSpeedLimitationValue.Text, Integer)
                End If
            End If

            'Dim dTempProductionRatesCoatingFactor As Double = 0
            'If txtProductionRatesCoatingFactorValue.Text.Trim <> "" Then
            '    dTempProductionRatesCoatingFactor = CType(txtProductionRatesCoatingFactorValue.Text, Double)
            'End If

            Dim iTempProductionRatesFinalFiguresMaxPiecesQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresMaxPiecesQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresMaxPiecesQuotedUnitID = ddProductionRatesFinalFiguresMaxPiecesQuotedUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresMaxPiecesMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresMaxPiecesMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresMaxPiecesMaximumUnitID = ddProductionRatesFinalFiguresMaxPiecesMaximumUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresPressCyclesQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresPressCyclesQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresPressCyclesQuotedUnitID = ddProductionRatesFinalFiguresPressCyclesQuotedUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresPressCyclesMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresPressCyclesMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresPressCyclesMaximumUnitID = ddProductionRatesFinalFiguresPressCyclesMaximumUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresLineSpeedQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresLineSpeedQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresLineSpeedQuotedUnitID = ddProductionRatesFinalFiguresLineSpeedQuotedUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresLineSpeedMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresLineSpeedMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresLineSpeedMaximumUnitID = ddProductionRatesFinalFiguresLineSpeedMaximumUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresNetFormingRateQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresNetFormingRateQuotedUnitID = ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresNetFormingRateMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresNetFormingRateMaximumUnitID = ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresMixCapacityQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresMixCapacityQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresMixCapacityQuotedUnitID = ddProductionRatesFinalFiguresMixCapacityQuotedUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresMixCapacityMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresMixCapacityMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresMixCapacityMaximumUnitID = ddProductionRatesFinalFiguresMixCapacityMaximumUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresRecycleRateQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresRecycleRateQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresRecycleRateQuotedUnitID = ddProductionRatesFinalFiguresRecycleRateQuotedUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresRecycleRateMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresRecycleRateMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresRecycleRateMaximumUnitID = ddProductionRatesFinalFiguresRecycleRateMaximumUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresPartWeightQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresPartWeightQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresPartWeightQuotedUnitID = ddProductionRatesFinalFiguresPartWeightQuotedUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresPartWeightMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresPartWeightMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresPartWeightMaximumUnitID = ddProductionRatesFinalFiguresPartWeightMaximumUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresCoatingWeightQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresCoatingWeightQuotedUnitID = ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresTotalWeightQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresTotalWeightQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresTotalWeightQuotedUnitID = ddProductionRatesFinalFiguresTotalWeightQuotedUnits.SelectedValue
            End If

            '********************************
            'begin calculations

            'always refresh value from Part Specifications tab
            txtProductionRatesWeightPerAreaValue.Text = dTempPartSpecificationsWeightPerArea
            dTempProductionRatesWeightPerArea = dTempPartSpecificationsWeightPerArea

            '**************************************************
            '*** BEGIN Bottom Left Corner Offline Specific

            ds = CostingModule.GetCostSheetLaborMinOrdinal(ViewState("CostSheetID"))

            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("CrewSize") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CrewSize") > 0 Then
                        dLaborOfflineCrewSize = ds.Tables(0).Rows(0).Item("CrewSize")
                    End If
                End If
            End If

            'txtProductionRatesOfflineSpecificCrewSizeValue.Text = txtPartSpecificationsOffLineRateValue.Text

            If iTempPartSpecificationsOffLineRate > 0 And dLaborOfflineCrewSize > 0 Then
                dOfflineSpecificPiecesManHour = CType(iTempPartSpecificationsOffLineRate / dLaborOfflineCrewSize, Double)

                If dOfflineSpecificPiecesManHour > 0 Then
                    txtProductionRatesOfflineSpecificPiecesManHourValue.Text = Format(dOfflineSpecificPiecesManHour, "####.00")
                End If

            End If

            If dTempPartSpecificationsDieLayoutTravel > 0 And dTempPartSpecificationsDieLayoutWidth > 0 And dTempPartSpecificationsPiecesPerCycle > 0 And dTempPartSpecificationsCalculatedArea > 0 Then
                dOfflineSpecificPercentRecycle = ((dTempPartSpecificationsDieLayoutTravel * dTempPartSpecificationsDieLayoutWidth) - (dTempPartSpecificationsPiecesPerCycle * dTempPartSpecificationsCalculatedArea)) / (dTempPartSpecificationsDieLayoutTravel * dTempPartSpecificationsDieLayoutWidth)

                If dOfflineSpecificPercentRecycle > 0 Then
                    If dOfflineSpecificPercentRecycle > 0 Then
                        txtProductionRatesOfflineSpecificPercentRecycleValue.Text = Format(dOfflineSpecificPercentRecycle, "####.0000")
                        lblProductionRatesOfflineSpecificPercentRecycleValuePercent.Text = Format(dOfflineSpecificPercentRecycle * 100, "####.00") & "%"
                    End If
                End If
            End If

            '*** END Bottom Left Corner Offline Specific
            '**************************************************

            '*************** Part Weight of Bottom Right Corner of the screen is the same calculation whether or not the Formula Deplug has a factor
            '1 gram = 0.00220462262 pounds
            '***** BE CAREFUL HERE. Some formulas need lbs instead of grams. So the * 454 might not always be right. Perhaps the fleece formula
            If dTempPartSpecificationsSpecificGravity > 0 Then ' show pounds on Quoted Column and grams in Maximum Column
                dPartWeightQuoted = (dTempPartSpecificationsCalculatedArea * dTempPartSpecificationsSpecificGravity * dTempPartSpecificationsThickness * 1000) / 454
                ddProductionRatesFinalFiguresPartWeightQuotedUnits.SelectedValue = 2
                iTempProductionRatesFinalFiguresPartWeightQuotedUnitID = 2

                ddProductionRatesFinalFiguresPartWeightMaximumUnits.SelectedValue = 5
                iTempProductionRatesFinalFiguresPartWeightMaximumUnitID = 5

                ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.SelectedValue = 2
                iTempProductionRatesFinalFiguresCoatingWeightQuotedUnitID = 2

                'ddProductionRatesFinalFiguresCoatingWeightMaximumUnits.SelectedValue = 5
                'iTempProductionRatesFinalFiguresCoatingWeightMaximumUnitID = 5

                ddProductionRatesFinalFiguresTotalWeightQuotedUnits.SelectedValue = 2 'lb
                iTempProductionRatesFinalFiguresTotalWeightQuotedUnitID = 2

                'ddProductionRatesFinalFiguresTotalWeightMaximumUnits.SelectedValue = 5 'g

                If dPartWeightQuoted > 0 Then
                    txtProductionRatesFinalFiguresPartWeightQuotedValue.Text = Format(dPartWeightQuoted, "####.0000")
                End If

                dPartWeightMaximum = dPartWeightQuoted * 454
                If dPartWeightMaximum > 0 Then
                    txtProductionRatesFinalFiguresPartWeightMaximumValue.Text = Format(dPartWeightMaximum, "####.0000")
                End If

            Else ' show grams on Quoted Column and pounds in Maximum Column
                dPartWeightQuoted = dTempPartSpecificationsCalculatedArea * dTempProductionRatesWeightPerArea
                ddProductionRatesFinalFiguresPartWeightQuotedUnits.SelectedValue = 5
                iTempProductionRatesFinalFiguresPartWeightQuotedUnitID = 5

                ddProductionRatesFinalFiguresPartWeightMaximumUnits.SelectedValue = 2
                iTempProductionRatesFinalFiguresPartWeightMaximumUnitID = 2

                ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.SelectedValue = 5
                iTempProductionRatesFinalFiguresCoatingWeightQuotedUnitID = 5

                'ddProductionRatesFinalFiguresCoatingWeightMaximumUnits.SelectedValue = 2

                ddProductionRatesFinalFiguresTotalWeightQuotedUnits.SelectedValue = 5
                iTempProductionRatesFinalFiguresTotalWeightQuotedUnitID = 5

                'ddProductionRatesFinalFiguresTotalWeightMaximumUnits.SelectedValue = 2

                If dPartWeightQuoted > 0 Then
                    txtProductionRatesFinalFiguresPartWeightQuotedValue.Text = Format(dPartWeightQuoted, "####.0000")
                End If

                dPartWeightMaximum = dPartWeightQuoted * 0.00220462262
                If dPartWeightMaximum > 0 Then
                    txtProductionRatesFinalFiguresPartWeightMaximumValue.Text = Format(dPartWeightMaximum, "####.0000")
                End If
            End If

            If iDeplugFactorCount > 0 Then 'Formula has Hole Deplug Factors

                '**************************************************
                '*** BEGIN Upper Left corner

                lblProductionRatesMaxFormingRateLabel.Visible = bFormulaFleeceType
                txtProductionRatesMaxFormingRateValue.Visible = bFormulaFleeceType
                ddProductionRatesMaxFormingRateUnits.Visible = bFormulaFleeceType

                lblProductionRatesMaxMixCapacityLabel.Visible = Not bFormulaFleeceType
                txtProductionRatesMaxMixCapacityValue.Visible = Not bFormulaFleeceType
                ddProductionRatesMaxMixCapacityUnits.Visible = Not bFormulaFleeceType

                If bFormulaFleeceType = True Then

                    If txtProductionRatesMaxFormingRateValue.Text.Trim <> "" Then
                        dFormulaMaxFormingRate = CType(iTempProductionRatesMaxFormingRate, Double)
                    Else
                        If dFormulaMaxFormingRate > 0 Then
                            txtProductionRatesMaxFormingRateValue.Text = dFormulaMaxFormingRate.ToString
                        End If
                    End If

                Else
                    'DCADE 12/9/2009 -  value captured above when always getting formula value
                    'If txtProductionRatesMaxMixCapacityValue.Text.Trim <> "" Then 'override with old cost sheet info
                    '    iFormulaMaxMixCapacity = iTempProductionRatesMaxMixCapacity
                    'Else
                    '    If iFormulaMaxMixCapacity > 0 Then
                    '        txtProductionRatesMaxMixCapacityValue.Text = iFormulaMaxMixCapacity.ToString
                    '    End If
                    'End If

                    dTempProductionRatesMaxMixCapacity = CType(iFormulaMaxMixCapacity, Double)

                End If

                dCatchingAbilityFactor = 1.15

                'DCADE 07/13/2011 - always pull CatchingAbilityFactor value from maintenance table and override text box
                'If txtProductionRatesCatchingAbilityValue.Text.Trim <> "" Then
                '    dCatchingAbilityFactor = dTempProductionRatesCatchingAbility
                'Else
                If dTempPartSpecificationsPiecesPerCycle >= 2 Then
                    ds = CostingModule.GetCostSheetCatchingAbilityFactor(0, dTempPartSpecificationsPartLength, cbPartSpecificationsIsSideBySideValue.Checked, False)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        If ds.Tables(0).Rows(0).Item("CatchingAbilityFactor") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("CatchingAbilityFactor") > 0 Then
                                dCatchingAbilityFactor = ds.Tables(0).Rows(0).Item("CatchingAbilityFactor")
                            End If
                        End If
                    End If
                End If

                If dCatchingAbilityFactor > 0 Then
                    txtProductionRatesCatchingAbilityValue.Text = Format(dCatchingAbilityFactor, "####.0000")
                End If
                'End If

                'DCADE 12/9/2009 - value captured above when always getting formula value
                'If txtProductionRatesLineSpeedLimitationValue.Text.Trim <> "" Then 'use old cost sheet info
                'iFormulaLineSpeed = iTempProductionRatesLineSpeedLimitation
                'Else
                '    If iFormulaLineSpeed > 0 Then
                '        txtProductionRatesLineSpeedLimitationValue.Text = iFormulaLineSpeed.ToString
                '    End If
                'End If

                If txtProductionRatesCatchPercentValue.Text.Trim <> "" Then
                    dProductionRatesCatchPercent = CType(txtProductionRatesCatchPercentValue.Text, Double)
                Else
                    txtProductionRatesCatchPercentValue.Text = "1"
                    dProductionRatesCatchPercent = 1
                End If

                'DCADE 06/28/2011 - value should always be pulled from the formula
                'If txtProductionRatesCoatingFactorValue.Text.Trim = "" Then 'use old cost sheet info
                '    dCoatingFactor = dTempProductionRatesCoatingFactor
                'Else
                'get coating factor from formula and part specification thickness
                ds = CostingModule.GetFormulaCoatingFactor(0, iFormulaID, dTempPartSpecificationsThickness)
                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("CoatingFactor") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("CoatingFactor") > 0 Then
                            dCoatingFactor = ds.Tables(0).Rows(0).Item("CoatingFactor")
                        End If
                    End If
                End If

                If dCoatingFactor <> 0 Then
                    txtProductionRatesCoatingFactorValue.Text = Format(dCoatingFactor, "####.0000")
                End If
                'End If

                If dTempProductionRatesWeightPerArea > 0 Then
                    txtProductionRatesWeightPerAreaValue.Visible = True
                    lblProductionRatesWeightPerAreaLabel.Visible = True
                    ddProductionRatesWeightPerAreaUnits.Visible = True
                Else
                    txtProductionRatesWeightPerAreaValue.Visible = False
                    lblProductionRatesWeightPerAreaLabel.Visible = False
                    ddProductionRatesWeightPerAreaUnits.Visible = False
                End If

                '*** END Upper Left corner
                '**************************************************



                '**************************************************
                '*** BEGIN Production Limits based on 100%
                '**************************************************

                'clear all previous production limits
                CostingModule.DeleteCostSheetProductionLimit(ViewState("CostSheetID"))

                'calculate line speed limit, #4 in Production Limit_Maint                
                If dTempPartSpecificationsPiecesPerCycle > 0 And dTempPartSpecificationsDieLayoutTravel > 0 And iFormulaLineSpeed > 0 Then
                    iProductionLimitLineSpeedLimit = CType((iFormulaLineSpeed * dTempPartSpecificationsPiecesPerCycle * 60) / dTempPartSpecificationsDieLayoutTravel, Integer)

                    'save if greater than 0
                    If iProductionLimitLineSpeedLimit > 0 Then
                        CostingModule.InsertCostSheetProductionLimit(ViewState("CostSheetID"), 4, iProductionLimitLineSpeedLimit, 19, 0)
                    End If
                End If

                'calculate cycle limit, #1 in Production Limit Maint                
                If dTempPartSpecificationsPiecesPerCycle > 0 Then
                    iProductionLimitCycleLimit = iFormulaMaxPressCycles * dTempPartSpecificationsPiecesPerCycle

                    'save if greater than 0
                    If iProductionLimitCycleLimit > 0 Then
                        'somehow handle units too - what if not metric?
                        CostingModule.InsertCostSheetProductionLimit(ViewState("CostSheetID"), 1, iProductionLimitCycleLimit, 0, 0)
                    End If
                End If

                'calculate Mix Capacity, # 3 in Production Limit Maint

                Dim dTempFactor As Double = 0

                If dTempPartSpecificationsChangedArea <> dTempPartSpecificationsCalculatedArea Then
                    If dTempPartSpecificationsThickness > 0 And dTempPartSpecificationsChangedArea > 0 And dTempPartSpecificationsSpecificGravity > 0 And dTempProductionRatesMaxMixCapacity > 0 Then
                        If cbPartSpecificationsIsDiecutValue.Checked = False Then
                            'this converts kgs to lbs. should this be done? / 2.205
                            iProductionLimitMixCapacity = (dTempProductionRatesMaxMixCapacity / (dTempPartSpecificationsChangedArea * dTempPartSpecificationsThickness * dTempPartSpecificationsSpecificGravity)) / 2.205
                        Else
                            If bFormulaRecycleReturn = True Then
                                iProductionLimitMixCapacity = CType(dTempProductionRatesMaxMixCapacity / (dTempPartSpecificationsChangedArea * dTempPartSpecificationsThickness * dTempPartSpecificationsSpecificGravity), Integer)
                            Else
                                dTempFactor = ((dTempPartSpecificationsDieLayoutWidth * dTempPartSpecificationsDieLayoutTravel * dTempPartSpecificationsThickness * dTempPartSpecificationsSpecificGravity) / 454) * 1000
                                iProductionLimitMixCapacity = CType((dTempProductionRatesMaxMixCapacity / dTempFactor) * dTempPartSpecificationsPiecesPerCycle, Integer)
                            End If
                        End If
                    End If
                Else
                    If dTempPartSpecificationsThickness > 0 And dTempPartSpecificationsCalculatedArea > 0 And dTempPartSpecificationsSpecificGravity > 0 And dTempProductionRatesMaxMixCapacity > 0 Then
                        If cbPartSpecificationsIsDiecutValue.Checked = False Then
                            'this converts kgs to lbs. should this be done? / 2.205
                            iProductionLimitMixCapacity = (dTempProductionRatesMaxMixCapacity / (dTempPartSpecificationsCalculatedArea * dTempPartSpecificationsThickness * dTempPartSpecificationsSpecificGravity)) / 2.205
                        Else
                            If bFormulaRecycleReturn = True Then
                                iProductionLimitMixCapacity = dTempProductionRatesMaxMixCapacity / (dTempPartSpecificationsCalculatedArea * dTempPartSpecificationsThickness * dTempPartSpecificationsSpecificGravity)
                            Else
                                dTempFactor = ((dTempPartSpecificationsDieLayoutWidth * dTempPartSpecificationsDieLayoutTravel * dTempPartSpecificationsThickness * dTempPartSpecificationsSpecificGravity) / 454) * 1000
                                iProductionLimitMixCapacity = CType((dTempProductionRatesMaxMixCapacity / dTempFactor) * dTempPartSpecificationsPiecesPerCycle, Integer)
                            End If
                        End If
                    End If
                End If

                If iProductionLimitMixCapacity > 0 Then
                    'kg/hr
                    CostingModule.InsertCostSheetProductionLimit(ViewState("CostSheetID"), 3, iProductionLimitMixCapacity, 20, 0)
                End If


                'calculate Deplug Capacity, # 5 in Production Limit Maint

                Dim dDeplugFactor As Double = 0
                'get deplug factor from formula and part specification thickness
                ds = CostingModule.GetFormulaDeplugFactor(0, iFormulaID, dTempPartSpecificationsThickness)
                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("DeplugFactor") IsNot System.DBNull.Value Then
                        dDeplugFactor = ds.Tables(0).Rows(0).Item("DeplugFactor")

                        If dDeplugFactor > 0 Then
                            If dTempPartSpecificationsThickness > 0 And iTempPartSpecificationsNumberOfHoles > 0 Then
                                iProductionLimitDeplugCapacity = CType(dDeplugFactor / iTempPartSpecificationsNumberOfHoles, Integer)

                                If iProductionLimitDeplugCapacity > 0 Then
                                    CostingModule.InsertCostSheetProductionLimit(ViewState("CostSheetID"), 5, iProductionLimitDeplugCapacity, 0, 0)
                                End If
                            End If
                        End If

                    End If
                End If

                'calculate Catching Ability Capacity, # 6 in Production Limit Maint
                If dCatchingAbilityFactor > 0 And dProductionRatesCatchPercent > 0 And iTempPartSpecificationsPiecesCaughtTogether > 0 Then
                    iProductionLimitCatchingAbilityCapacity = CType((((3600 / dCatchingAbilityFactor) * 2) * dProductionRatesCatchPercent) * iTempPartSpecificationsPiecesCaughtTogether, Integer)

                    If iProductionLimitCatchingAbilityCapacity > 0 Then

                        CostingModule.InsertCostSheetProductionLimit(ViewState("CostSheetID"), 6, iProductionLimitCatchingAbilityCapacity, 0, 0)
                    End If
                End If

                'calculate Forming Rate, # 2 in Production Limit Maint
                If iTempProductionRatesMaxFormingRate > 0 And dTempPartSpecificationsDieLayoutWidth > 0 And dTempPartSpecificationsDieLayoutTravel > 0 And dTempProductionRatesWeightPerArea > 0 And dTempPartSpecificationsPiecesPerCycle > 0 Then
                    iProductionLimitFormingRate = CType((iTempProductionRatesMaxFormingRate / (dTempPartSpecificationsDieLayoutWidth * dTempPartSpecificationsDieLayoutTravel * (dTempProductionRatesWeightPerArea / 1000))) * dTempPartSpecificationsPiecesPerCycle, Integer)

                    If iProductionLimitFormingRate > 0 Then
                        CostingModule.InsertCostSheetProductionLimit(ViewState("CostSheetID"), 2, iProductionLimitFormingRate, 20, 0)
                    End If
                End If


                'calculate Travel/Cycle Limit, # 7 in Production Limit Maint
                If bForumlaUseBarrierRunRate = True And iFormulaBarrierPressCycles > 0 Then

                    'Pieces Per Cycle * iFormulaBarrierPressCycles
                    CostingModule.InsertCostSheetProductionLimit(ViewState("CostSheetID"), 7, iFormulaBarrierPressCycles * dTempPartSpecificationsPiecesPerCycle, 0, 0)
                End If

                '**************************************************
                '*** END Production Limits based on 100%
                '**************************************************




                '**************************************************
                '*** BEGIN Bottom Right Corner Quoted vs Max Values

                '***********           ORDER OF ASSIGNMENTS MATTERS HERE BE WARE!!!!   BUT DO NOT FEAR  ******************


                ds = CostingModule.GetCostSheetMinimumProductionLimit(ViewState("CostSheetID"))
                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("MinProductionLimit") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("MinProductionLimit") > 0 Then
                            iMinProductionLimit = ds.Tables(0).Rows(0).Item("MinProductionLimit")
                        End If
                    End If
                End If

                iMaxPiecesMaximum = iMinProductionLimit
                'IF the Pieces Per Cycle is even, then the iMinProductionLimitValue must also be even, add one to it
                If dTempPartSpecificationsPiecesPerCycle Mod 2 = 0 Then
                    If iMaxPiecesMaximum Mod 2 > 0 Then
                        iMaxPiecesMaximum += 1
                    End If
                End If

                If iMaxPiecesMaximum > 0 And dTempPartSpecificationsPiecesPerCycle > 0 Then
                    'force 0.5 to round up to 1
                    iPressCyclesMaximum = CType(((iMaxPiecesMaximum / dTempPartSpecificationsPiecesPerCycle) + 0.01), Integer)
                End If

                If iPressCyclesMaximum > 0 Then
                    txtProductionRatesFinalFiguresPressCyclesMaximumValue.Text = iPressCyclesMaximum.ToString
                End If

                'force 0.5 to round up to 1
                iPressCyclesQuoted = CType(((dTempPartSpecificationsConfigurationFactor * iPressCyclesMaximum) + 0.01), Integer)

                If iPressCyclesQuoted > 0 Then
                    txtProductionRatesFinalFiguresPressCyclesQuotedValue.Text = iPressCyclesQuoted.ToString
                End If

                'force 0.5 to round up to 1
                iMaxPiecesQuoted = CType(((iPressCyclesQuoted * dTempPartSpecificationsPiecesPerCycle) + 0.01), Integer)

                If iMaxPiecesQuoted > 0 Then
                    txtProductionRatesFinalFiguresMaxPiecesQuotedValue.Text = iMaxPiecesQuoted.ToString
                End If

                're-evaluate  iMaxPiecesMaximum
                iMaxPiecesMaximum = iPressCyclesMaximum * dTempPartSpecificationsPiecesPerCycle

                If iMaxPiecesMaximum > 0 Then
                    txtProductionRatesFinalFiguresMaxPiecesMaximumValue.Text = iMaxPiecesMaximum.ToString
                End If

                dLineSpeedMaximum = (iPressCyclesMaximum * dTempPartSpecificationsDieLayoutTravel) / 60

                'DCADE 12/9/2009 - MaxLineSpeed Calculation should not exceed formula
                If dLineSpeedMaximum > CType(iFormulaLineSpeed, Double) Then
                    dLineSpeedMaximum = CType(iFormulaLineSpeed, Double)
                End If

                If dLineSpeedMaximum > 0 Then
                    txtProductionRatesFinalFiguresLineSpeedMaximumValue.Text = Format(dLineSpeedMaximum, "####")
                End If

                dLineSpeedQuoted = dLineSpeedMaximum * dTempPartSpecificationsConfigurationFactor

                If dLineSpeedQuoted > 0 Then
                    txtProductionRatesFinalFiguresLineSpeedQuotedValue.Text = Format(dLineSpeedQuoted, "####")
                End If

                lblProductionRatesFinalFiguresMixCapacityQuotedLabel.Visible = Not bFormulaFleeceType
                txtProductionRatesFinalFiguresMixCapacityQuotedValue.Visible = Not bFormulaFleeceType
                ddProductionRatesFinalFiguresMixCapacityQuotedUnits.Visible = Not bFormulaFleeceType
                txtProductionRatesFinalFiguresMixCapacityMaximumValue.Visible = Not bFormulaFleeceType
                ddProductionRatesFinalFiguresMixCapacityMaximumUnits.Visible = Not bFormulaFleeceType

                lblProductionRatesFinalFiguresNetFormingRateQuotedLabel.Visible = bFormulaFleeceType
                txtProductionRatesFinalFiguresNetFormingRateQuotedValue.Visible = bFormulaFleeceType
                ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.Visible = bFormulaFleeceType
                txtProductionRatesFinalFiguresNetFormingRateMaximumValue.Visible = bFormulaFleeceType
                ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.Visible = bFormulaFleeceType

                If bFormulaFleeceType = True Then
                    If dFormulaMaxFormingRate > 0 And dTempPartSpecificationsCalculatedArea > 0 And dTempProductionRatesWeightPerArea > 0 And iMinProductionLimit > 0 Then

                        If dTempPartSpecificationsChangedArea <> dTempPartSpecificationsCalculatedArea Then
                            dNetFormingRateMaximum = (dTempPartSpecificationsChangedArea * dTempProductionRatesWeightPerArea * iMinProductionLimit) / 1000
                        Else
                            dNetFormingRateMaximum = (dTempPartSpecificationsCalculatedArea * dTempProductionRatesWeightPerArea * iMinProductionLimit) / 1000
                        End If

                        dNetFormingRateQuoted = dNetFormingRateMaximum * dTempPartSpecificationsConfigurationFactor

                        If dNetFormingRateMaximum > 0 Then
                            txtProductionRatesFinalFiguresNetFormingRateMaximumValue.Text = Format(dNetFormingRateMaximum, "####")
                        End If

                        If dNetFormingRateQuoted > 0 Then
                            txtProductionRatesFinalFiguresNetFormingRateQuotedValue.Text = Format(dNetFormingRateQuoted, "####")
                        End If
                    End If
                Else
                    If dTempPartSpecificationsChangedArea <> dTempPartSpecificationsCalculatedArea Then
                        If dTempPartSpecificationsThickness > 0 And dTempPartSpecificationsChangedArea > 0 And dTempPartSpecificationsSpecificGravity > 0 And iMinProductionLimit > 0 Then
                            dMixCapacityMaximum = dTempPartSpecificationsChangedArea * dTempPartSpecificationsThickness * dTempPartSpecificationsSpecificGravity * iMinProductionLimit
                        End If
                    Else
                        If dTempPartSpecificationsThickness > 0 And dTempPartSpecificationsCalculatedArea > 0 And dTempPartSpecificationsSpecificGravity > 0 And iMinProductionLimit > 0 Then
                            dMixCapacityMaximum = dTempPartSpecificationsThickness * dTempPartSpecificationsCalculatedArea * dTempPartSpecificationsSpecificGravity * iMinProductionLimit
                        End If
                    End If

                    'DCADE 12/9/2009 - MixCapacityMax Calculation should not exceed formula
                    If dMixCapacityMaximum > CType(iFormulaMaxMixCapacity, Double) Then
                        dMixCapacityMaximum = CType(iFormulaMaxMixCapacity, Double)
                    End If

                    If dMixCapacityMaximum > 0 Then
                        txtProductionRatesFinalFiguresMixCapacityMaximumValue.Text = Format(dMixCapacityMaximum, "####")
                    End If

                    dMixCapacityQuoted = dMixCapacityMaximum * dTempPartSpecificationsConfigurationFactor

                    If dMixCapacityQuoted > 0 Then
                        txtProductionRatesFinalFiguresMixCapacityQuotedValue.Text = Format(dMixCapacityQuoted, "####")
                    End If
                End If

                If cbPartSpecificationsIsDiecutValue.Checked = True Then
                    If bFormulaFleeceType = True Then
                        dRecycleRateMaximum = ((dTempPartSpecificationsDieLayoutTravel * dTempPartSpecificationsDieLayoutWidth * iPressCyclesMaximum * dTempProductionRatesWeightPerArea) / 1000) - dNetFormingRateMaximum
                    Else
                        dRecycleRateMaximum = (dTempPartSpecificationsDieLayoutTravel * dTempPartSpecificationsDieLayoutWidth * iPressCyclesMaximum * dTempPartSpecificationsThickness * dTempPartSpecificationsSpecificGravity) - dMixCapacityMaximum
                    End If

                    dRecycleRateQuoted = dTempPartSpecificationsConfigurationFactor * dRecycleRateMaximum
                End If

                If dRecycleRateMaximum > 0 Then
                    txtProductionRatesFinalFiguresRecycleRateMaximumValue.Text = Format(dRecycleRateMaximum, "####")
                End If

                If dRecycleRateQuoted > 0 Then
                    txtProductionRatesFinalFiguresRecycleRateQuotedValue.Text = Format(dRecycleRateQuoted, "####")
                End If

                If iFormulaTemplateID = 1 Or iFormulaTemplateID = 5 Then 'if Melsheet Template/Department or Composite
                    If dTempPartSpecificationsPiecesPerCycle > 0 Then
                        dCoatingWeightQuoted = (10.76 * dTempPartSpecificationsDieLayoutTravel * (dTempPartSpecificationsDieLayoutWidth - 0.1) * dCoatingFactor) / (454 * dTempPartSpecificationsPiecesPerCycle)
                    End If
                Else
                    If dTempPartSpecificationsPiecesPerCycle > 0 Then
                        dCoatingWeightQuoted = (10.76 * dTempPartSpecificationsDieLayoutTravel * dTempPartSpecificationsDieLayoutWidth * dCoatingFactor) / (454 * dTempPartSpecificationsPiecesPerCycle)
                    End If
                End If

                If dCoatingWeightQuoted > 0 Then
                    txtProductionRatesFinalFiguresCoatingWeightQuotedValue.Text = Format(dCoatingWeightQuoted, "####.0000")
                End If

                dTotalWeightQuoted = dPartWeightQuoted + dCoatingWeightQuoted

                If dTotalWeightQuoted > 0 Then
                    txtProductionRatesFinalFiguresTotalWeightQuotedValue.Text = Format(dTotalWeightQuoted, "####.0000")
                End If

                '*** END Bottom Right Corner Quoted vs Max Values
                '**************************************************

            Else 'formula has no deplug factor

                iPressCyclesMaximum = iTempProductionRatesOfflineSpecificQuotedPressCycles

                If iPressCyclesMaximum > 0 Then
                    txtProductionRatesFinalFiguresPressCyclesMaximumValue.Text = iPressCyclesMaximum.ToString
                End If

                'force 0.50 to round to 1
                iPressCyclesQuoted = CType(((dTempPartSpecificationsConfigurationFactor * iPressCyclesMaximum) + 0.01), Integer)

                If iPressCyclesQuoted > 0 Then
                    txtProductionRatesFinalFiguresPressCyclesQuotedValue.Text = iPressCyclesQuoted.ToString
                End If

            End If

            '**************************************************
            '*** BEGIN Updating Production Rates tabs

            CostingModule.UpdateCostSheetProductionRate(ViewState("CostSheetID"), iFormulaMaxMixCapacity, _
                iTempProductionRatesMaxMixCapacityUnitID, dFormulaMaxFormingRate, _
                iTempProductionRatesMaxMixCapacityUnitID, dCatchingAbilityFactor, iFormulaLineSpeed, _
                dProductionRatesCatchPercent, dCoatingFactor, dTempProductionRatesWeightPerArea, _
                iTempProductionRatesWeightPerAreaUnitID, iTempProductionRatesOfflineSpecificSheetsUp, _
                txtProductionRatesOfflineSpecificBlankCodeValue.Text, iTempProductionRatesOfflineSpecificQuotedPressCycles, _
                iTempProductionRatesOfflineSpecificQuotedOfflineRates, dOfflineSpecificPiecesManHour, dOfflineSpecificPercentRecycle, _
                iMaxPiecesQuoted, iTempProductionRatesFinalFiguresMaxPiecesQuotedUnitID, iMaxPiecesMaximum, _
                iTempProductionRatesFinalFiguresMaxPiecesMaximumUnitID, iPressCyclesQuoted, _
                iTempProductionRatesFinalFiguresPressCyclesQuotedUnitID, iPressCyclesMaximum, _
                iTempProductionRatesFinalFiguresPressCyclesMaximumUnitID, dLineSpeedQuoted, _
                iTempProductionRatesFinalFiguresLineSpeedQuotedUnitID, dLineSpeedMaximum, _
                iTempProductionRatesFinalFiguresLineSpeedMaximumUnitID, dNetFormingRateQuoted, _
                iTempProductionRatesFinalFiguresNetFormingRateQuotedUnitID, dNetFormingRateMaximum, _
                iTempProductionRatesFinalFiguresNetFormingRateMaximumUnitID, dMixCapacityQuoted, _
                iTempProductionRatesFinalFiguresMixCapacityQuotedUnitID, dMixCapacityMaximum, _
                iTempProductionRatesFinalFiguresMixCapacityMaximumUnitID, dRecycleRateQuoted, _
                iTempProductionRatesFinalFiguresRecycleRateQuotedUnitID, dRecycleRateMaximum, _
                iTempProductionRatesFinalFiguresRecycleRateMaximumUnitID, dPartWeightQuoted, _
                iTempProductionRatesFinalFiguresPartWeightQuotedUnitID, dPartWeightMaximum, _
                iTempProductionRatesFinalFiguresPartWeightMaximumUnitID, dCoatingWeightQuoted, _
                iTempProductionRatesFinalFiguresCoatingWeightQuotedUnitID, dTotalWeightQuoted, _
                iTempProductionRatesFinalFiguresTotalWeightQuotedUnitID)

            '*** END Calculating rows of gridviews on tabs
            '**************************************************

            '**************************************************
            '*** BEGIN Calculating/Updating rows of gridviews on tabs

            'Materials GridView/Tab

            '06/17/2010 - Nina Butler, Foam value is in pounds and should be used for Material Quantities when normal part weight is not available
            If dTempPartSpecificationsFoamValue > 0 And dPartWeightQuoted = 0 Then
                dPartWeightQuoted = dTempPartSpecificationsFoamValue
            End If

            Dim dTempMaterialCostTotal As Double = CostingModule.CalculateCostSheetMaterial(ViewState("CostSheetID"), bFormulaFleeceType, dPartWeightQuoted, dCoatingWeightQuoted, dTempQuotedInfoStandardCostFactor)

            'Packaging GridView/Tab
            Dim dTempPackagingCostTotal As Double = CostingModule.CalculateCostSheetPackaging(ViewState("CostSheetID"), dTempQuotedInfoStandardCostFactor)

            'Labor GridView/Tab
            Dim dTempLaborCostTotal As Double = CostingModule.CalculateCostSheetLabor(ViewState("CostSheetID"), iFormulaTemplateID, iMaxPiecesQuoted, dTempPartSpecificationsProductionRateValue, dTempQuotedInfoStandardCostFactor, iTempPartSpecificationsOffLineRate)

            'Overhead GridView/Tab
            Dim dTempOverheadCostTotal As Double = CostingModule.CalculateCostSheetOverhead(ViewState("CostSheetID"), iFormulaTemplateID, iMaxPiecesQuoted, dTempPartSpecificationsProductionRateValue, dTempPartSpecificationsNumberOfCarriers, dTempQuotedInfoStandardCostFactor, iTempPartSpecificationsOffLineRate)

            'Capital GridView/Tab
            Dim dTempCapitalCostTotal As Double = CostingModule.CalculateCostSheetCapital(ViewState("CostSheetID"), iFormulaTemplateID, iMaxPiecesQuoted, dTempPartSpecificationsProductionRateValue, iTempPartSpecificationsOffLineRate)

            'Get Total Upto This point
            Dim dTempCostSheetSubTotal As Double = dTempMaterialCostTotal + dTempPackagingCostTotal + dTempLaborCostTotal + dTempOverheadCostTotal + dTempCapitalCostTotal

            'Misc Cost GridView/Tab
            CostingModule.CalculateCostSheetMiscCost(ViewState("CostSheetID"), iFormulaTemplateID, txtQuoteDateValue.Text.Trim, dTempCostSheetSubTotal, iTempQuotedInfoPiecesPerYear)

            CalculateCostSheetTotal()

            '*** END Calculating rows of gridviews on tabs
            '**************************************************

            RefreshGridViews()

            lblMessage.Text += "Calculations are complete and saved."

            accCostTotals.SelectedIndex = 0

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub ValidateIdentificationNumbers()

        Try
            Dim ds As DataSet

            If txtNewCustomerPartNoValue.Text.Trim <> "" Then
                '(LREY) 01/08/2014
                'ds = commonFunctions.GetCustomerPartBPCSPartRelate("", txtNewCustomerPartNoValue.Text.Trim, "", "", "")

                'If commonFunctions.CheckDataSet(ds) = False Then
                '    lblMessage.Text += "<br>WARNING: The NEW customer part number is not in the BPCS System yet."
                'End If
            End If

            'check if design level is in new RFD application for this part
            'txtNewDesignLevelValue.Text.Trim

            If txtOriginalCustomerPartNoValue.Text.Trim <> "" Then
                '(LREY) 01/08/2014
                'ds = commonFunctions.GetCustomerPartBPCSPartRelate("", txtOriginalCustomerPartNoValue.Text.Trim, "", "", "")

                'If commonFunctions.CheckDataSet(ds) = False Then
                '    lblMessage.Text += "<br>WARNING: The ORIGINAL customer part number is not in the BPCS System yet."
                'End If
            End If

            If txtNewDrawingNoValue.Text.Trim <> "" Then
                'ds = PEModule.GetDrawing(txtNewDrawingNoValue.Text.Trim, "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, False, "", "")
                ds = PEModule.GetDrawing(txtNewDrawingNoValue.Text.Trim)

                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text += "<br>WARNING: The NEW DMS drawing number is not in the DMS system. Please contact Product Engineering."
                End If
            End If

            If txtNewPartNoValue.Text.Trim <> "" Then
                ds = commonFunctions.GetBPCSPartNo(txtNewPartNoValue.Text.Trim, "")
                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text += "<br>WARNING: The NEW Internal Part Number is not in the Oracle system. Please contact Product Engineering."
                End If
            End If

            If txtOriginalPartNoValue.Text <> "" Then
                ds = commonFunctions.GetBPCSPartNo(txtOriginalPartNoValue.Text, "")
                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text += "<br>WARNING: The Original Internal Part Number is not in the Oracle system. Please contact Product Engineering."
                End If
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
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveLowerPage.Click

        Try
            ClearMessages()

            'check Customer, BPCS, and DMS Part Numbers before saving
            ValidateIdentificationNumbers()

            Dim ds As DataSet
            Dim dsPostApproval As DataSet
            Dim iPreviousCostSheetID As Integer

            'need to validate all info if it exists: RFDNo, ECINo, BPCS PartNo, Drawing, etc...

            Dim iTempPreviousCostSheetID As Integer = 0
            If hlnkPreviousCostSheetIDValue.Text.Trim <> "" Then
                iTempPreviousCostSheetID = hlnkPreviousCostSheetIDValue.Text
            End If

            'Dim iTempProgramID As Integer = 0
            'If ddProgramValue.SelectedIndex > 0 Then
            '    iTempProgramID = ddProgramValue.SelectedValue
            'End If

            Dim iTempCommodityID As Integer = 0
            If ddCommodityValue.SelectedIndex > 0 Then
                iTempCommodityID = ddCommodityValue.SelectedValue
            End If

            Dim iTempPurchasedGoodID As Integer = 0
            If ddPurchasedGoodValue.SelectedIndex > 0 Then
                iTempPurchasedGoodID = ddPurchasedGoodValue.SelectedValue
            End If

            Dim iTempRFDNo As Integer = 0
            If txtRFDNoValue.Text <> "" Then
                iTempRFDNo = CType(txtRFDNoValue.Text, Integer)
            End If

            Dim iTempECINo As Integer = 0
            If txtECINoValue.Text <> "" Then
                iTempECINo = CType(txtECINoValue.Text, Integer)
            End If

            'Dim iTempYear As Integer = 0
            'If ddYearValue.SelectedIndex > 0 Then
            '    iTempYear = ddYearValue.SelectedValue
            'End If

            Dim iTempPartSpecificationFormulaID As Integer = 0
            If ddPartSpecificationsFormulaValue.SelectedIndex > 0 Then
                iTempPartSpecificationFormulaID = ddPartSpecificationsFormulaValue.SelectedValue
            End If

            Dim dTempPartSpecificationsThickness As Double = 0
            If txtPartSpecificationsThicknessValue.Text.Trim <> "" Then
                dTempPartSpecificationsThickness = CType(txtPartSpecificationsThicknessValue.Text, Double)
            End If

            Dim iTempPartSpecificationsThicknessUnitID As Integer = 0
            If ddPartSpecificationsThicknessUnits.SelectedIndex > 0 Then
                iTempPartSpecificationsThicknessUnitID = ddPartSpecificationsThicknessUnits.SelectedValue
            End If

            Dim iTempPartSpecificationsOffLineRate As Integer = 0
            If txtPartSpecificationsOffLineRateValue.Text.Trim <> "" Then
                iTempPartSpecificationsOffLineRate = CType(txtPartSpecificationsOffLineRateValue.Text, Integer)
            End If

            Dim iTempPartSpecificationsNumberOfHoles As Integer = 0
            If txtPartSpecificationsNumberOfHolesValue.Text.Trim <> "" Then
                iTempPartSpecificationsNumberOfHoles = CType(txtPartSpecificationsNumberOfHolesValue.Text, Integer)
            End If

            Dim dTempPartSpecificationsPartWidthValue As Double = 0
            If txtPartSpecificationsPartWidthValue.Text.Trim <> "" Then
                dTempPartSpecificationsPartWidthValue = CType(txtPartSpecificationsPartWidthValue.Text, Double)
            End If

            Dim iTempPartSpecificationsPartWidthUnitID As Integer = 0
            If ddPartSpecificationsPartWidthUnits.SelectedIndex > 0 Then
                iTempPartSpecificationsPartWidthUnitID = ddPartSpecificationsPartWidthUnits.SelectedValue
            End If

            Dim dTempPartSpecificationsPartLengthValue As Double = 0
            If txtPartSpecificationsPartLengthValue.Text.Trim <> "" Then
                dTempPartSpecificationsPartLengthValue = CType(txtPartSpecificationsPartLengthValue.Text, Double)
            End If

            Dim iTempPartSpecificationsPartLengthUnitID As Integer = 0
            If ddPartSpecificationsPartLengthUnits.SelectedIndex > 0 Then
                iTempPartSpecificationsPartLengthUnitID = ddPartSpecificationsPartLengthUnits.SelectedValue
            End If

            Dim dTempPartSpecificationsConfigurationFactor As Double = 0
            If txtPartSpecificationsConfigurationFactorValue.Text.Trim <> "" Then
                dTempPartSpecificationsConfigurationFactor = CType(txtPartSpecificationsConfigurationFactorValue.Text, Double)
                lblPartSpecificationsConfigurationFactorPercentageValue.Text = Format(dTempPartSpecificationsConfigurationFactor * 100, "####.00") & "%"
            End If

            Dim dTempPartSpecificationsApproxWeightValue As Double = 0
            If txtPartSpecificationsApproxWeightValue.Text.Trim <> "" Then
                dTempPartSpecificationsApproxWeightValue = CType(txtPartSpecificationsApproxWeightValue.Text, Double)
            End If

            Dim iTempPartSpecificationsApproxWeightUnitID As Integer = 0
            If ddPartSpecificationsApproxWeightUnits.SelectedIndex > 0 Then
                iTempPartSpecificationsApproxWeightUnitID = ddPartSpecificationsApproxWeightUnits.SelectedValue
            End If

            Dim dTempPartSpecificationsProductionRateValue As Double = 0
            If txtPartSpecificationsProductionRateValue.Text.Trim <> "" Then
                dTempPartSpecificationsProductionRateValue = CType(txtPartSpecificationsProductionRateValue.Text, Double)
            End If

            'Dim iTempPartSpecificationDepartmentID As Integer = 0
            'If ddPartSpecificationsDepartmentValue.SelectedIndex > 0 Then
            '    iTempPartSpecificationDepartmentID = ddPartSpecificationsDepartmentValue.SelectedValue
            'End If

            Dim dTempPartSpecificationsNumberOfCarriers As Double = 0
            If txtPartSpecificationsNumberOfCarriersValue.Text.Trim <> "" Then
                dTempPartSpecificationsNumberOfCarriers = CType(txtPartSpecificationsNumberOfCarriersValue.Text, Double)
            End If

            Dim dTempPartSpecificationsFoamValue As Double = 0
            If txtPartSpecificationsFoamValue.Text.Trim <> "" Then
                dTempPartSpecificationsFoamValue = CType(txtPartSpecificationsFoamValue.Text, Double)
            End If

            Dim iTempPartSpecificationsFoamUnitID As Integer = 0
            If ddPartSpecificationsFoamUnits.SelectedIndex > 0 Then
                iTempPartSpecificationsFoamUnitID = ddPartSpecificationsFoamUnits.SelectedValue
            End If

            Dim iTempPartSpecificationsPiecesPerCycleValue As Integer = 0
            If txtPartSpecificationsPiecesPerCycleValue.Text.Trim <> "" Then
                iTempPartSpecificationsPiecesPerCycleValue = CType(txtPartSpecificationsPiecesPerCycleValue.Text, Integer)
            End If

            Dim iTempPartSpecificationsPiecesCaughtTogether As Integer = 0
            If txtPartSpecificationsPiecesCaughtTogetherValue.Text.Trim <> "" Then
                iTempPartSpecificationsPiecesCaughtTogether = CType(txtPartSpecificationsPiecesCaughtTogetherValue.Text, Integer)
            End If

            Dim dTempPartSpecificationsCalculatedAreaValue As Double = 0
            If txtPartSpecificationsCalculatedAreaValue.Text.Trim <> "" Then
                dTempPartSpecificationsCalculatedAreaValue = CType(txtPartSpecificationsCalculatedAreaValue.Text, Double)
            End If

            Dim iTempPartSpecificationsCalculatedAreaUnitID As Integer = 0
            If ddPartSpecificationsCalculatedAreaUnits.SelectedIndex > 0 Then
                iTempPartSpecificationsCalculatedAreaUnitID = ddPartSpecificationsCalculatedAreaUnits.SelectedValue
            End If

            Dim dTempPartSpecificationsChangedAreaValue As Double = 0
            If txtPartSpecificationsChangedAreaValue.Text.Trim <> "" Then
                dTempPartSpecificationsChangedAreaValue = CType(txtPartSpecificationsChangedAreaValue.Text, Double)
            End If

            Dim iTempPartSpecificationsChangedAreaUnitID As Integer = 0
            If ddPartSpecificationsChangedAreaUnits.SelectedIndex > 0 Then
                iTempPartSpecificationsChangedAreaUnitID = ddPartSpecificationsChangedAreaUnits.SelectedValue
            End If

            Dim dTempPartSpecificationsDieLayoutWidthValue As Double = 0
            If txtPartSpecificationsDieLayoutWidthValue.Text.Trim <> "" Then
                dTempPartSpecificationsDieLayoutWidthValue = CType(txtPartSpecificationsDieLayoutWidthValue.Text, Double)
            End If

            Dim iTempPartSpecificationsDieLayoutWidthUnitID As Integer = 0
            If ddPartSpecificationsDieLayoutWidthUnits.SelectedIndex > 0 Then
                iTempPartSpecificationsDieLayoutWidthUnitID = ddPartSpecificationsDieLayoutWidthUnits.SelectedValue
            End If

            Dim dTempPartSpecificationsDieLayoutTravelValue As Double = 0
            If txtPartSpecificationsDieLayoutTravelValue.Text.Trim <> "" Then
                dTempPartSpecificationsDieLayoutTravelValue = CType(txtPartSpecificationsDieLayoutTravelValue.Text, Double)
            End If

            Dim iTempPartSpecificationsDieLayoutTravelUnitID As Integer = 0
            If ddPartSpecificationsDieLayoutTravelUnits.SelectedIndex > 0 Then
                iTempPartSpecificationsDieLayoutTravelUnitID = ddPartSpecificationsDieLayoutTravelUnits.SelectedValue
            End If

            Dim dTempPartSpecificationsWeightPerAreaValue As Double = 0
            If txtPartSpecificationsWeightPerAreaValue.Text.Trim <> "" Then
                dTempPartSpecificationsWeightPerAreaValue = CType(txtPartSpecificationsWeightPerAreaValue.Text, Double)
            End If

            Dim iTempPartSpecificationsWeightPerAreaUnitID As Integer = 0
            If ddPartSpecificationsWeightPerAreaUnits.SelectedIndex > 0 Then
                iTempPartSpecificationsWeightPerAreaUnitID = ddPartSpecificationsWeightPerAreaUnits.SelectedValue
            End If

            Dim dTempPartSpecificationsSpecificGravityValue As Double = 0
            If txtPartSpecificationsSpecificGravityValue.Text.Trim <> "" Then
                dTempPartSpecificationsSpecificGravityValue = CType(txtPartSpecificationsSpecificGravityValue.Text, Double)
            End If

            Dim iTempPartSpecificationsSpecificGravityUnitID As Integer = 0
            If ddPartSpecificationsSpecificGravityUnits.SelectedIndex > 0 Then
                iTempPartSpecificationsSpecificGravityUnitID = ddPartSpecificationsSpecificGravityUnits.SelectedValue
            End If

            Dim iTempPartSpecificationProcessID As Integer = 0
            If ddPartSpecificationsProcessValue.SelectedIndex > 0 Then
                iTempPartSpecificationProcessID = ddPartSpecificationsProcessValue.SelectedValue
            End If

            Dim iTempProductionRatesMaxMixCapacityValue As Integer = 0
            If txtProductionRatesMaxMixCapacityValue.Text.Trim <> "" Then
                iTempProductionRatesMaxMixCapacityValue = CType(txtProductionRatesMaxMixCapacityValue.Text, Integer)
            End If

            Dim iTempProductionRatesMaxMixCapacityUnitID As Integer = 0
            If ddProductionRatesMaxMixCapacityUnits.SelectedIndex > 0 Then
                iTempProductionRatesMaxMixCapacityUnitID = ddProductionRatesMaxMixCapacityUnits.SelectedValue
            End If

            Dim iTempProductionRatesMaxFormingRateValue As Integer = 0
            If txtProductionRatesMaxFormingRateValue.Text.Trim <> "" Then
                iTempProductionRatesMaxFormingRateValue = CType(txtProductionRatesMaxFormingRateValue.Text, Integer)
            End If

            Dim iTempProductionRatesMaxFormingRateUnitID As Integer = 0
            If ddProductionRatesMaxFormingRateUnits.SelectedIndex > 0 Then
                iTempProductionRatesMaxFormingRateUnitID = ddProductionRatesMaxFormingRateUnits.SelectedValue
            End If

            Dim dTempProductionRatesCatchingAbilityValue As Double = 0
            If txtProductionRatesCatchingAbilityValue.Text.Trim <> "" Then
                dTempProductionRatesCatchingAbilityValue = CType(txtProductionRatesCatchingAbilityValue.Text, Double)
            End If

            Dim iTempProductionRatesLineSpeedLimitationValue As Integer = 0
            If txtProductionRatesLineSpeedLimitationValue.Text.Trim <> "" Then
                iTempProductionRatesLineSpeedLimitationValue = CType(txtProductionRatesLineSpeedLimitationValue.Text, Integer)
            End If

            Dim dTempProductionRatesCatchPercentValue As Double = 0
            If txtProductionRatesCatchPercentValue.Text.Trim <> "" Then
                dTempProductionRatesCatchPercentValue = CType(txtProductionRatesCatchPercentValue.Text, Double)
            End If

            Dim dTempProductionRatesCoatingFactorValue As Double = 0
            If txtProductionRatesCoatingFactorValue.Text.Trim <> "" Then
                dTempProductionRatesCoatingFactorValue = CType(txtProductionRatesCoatingFactorValue.Text, Double)
            End If

            Dim dTempProductionRatesWeightPerAreaValue As Double = 0
            If txtProductionRatesWeightPerAreaValue.Text.Trim <> "" Then
                dTempProductionRatesWeightPerAreaValue = CType(txtProductionRatesWeightPerAreaValue.Text, Double)
            End If

            Dim iTempProductionRatesWeightPerAreaUnitID As Integer = 0
            If ddProductionRatesWeightPerAreaUnits.SelectedIndex > 0 Then
                iTempProductionRatesWeightPerAreaUnitID = ddProductionRatesWeightPerAreaUnits.SelectedValue
            End If

            Dim iTempProductionRatesOfflineSpecificSheetsUpValue As Integer = 0
            If txtProductionRatesOfflineSpecificSheetsUpValue.Text.Trim <> "" Then
                iTempProductionRatesOfflineSpecificSheetsUpValue = CType(txtProductionRatesOfflineSpecificSheetsUpValue.Text, Integer)
            End If

            Dim iTempProductionRatesOfflineSpecificQuotedPressCyclesValue As Integer = 0
            If txtProductionRatesOfflineSpecificQuotedPressCyclesValue.Text.Trim <> "" Then
                iTempProductionRatesOfflineSpecificQuotedPressCyclesValue = CType(txtProductionRatesOfflineSpecificQuotedPressCyclesValue.Text, Integer)
            End If

            Dim iTempProductionRatesOfflineSpecificQuotedOfflineRatesValue As Integer = 0
            If txtProductionRatesOfflineSpecificQuotedOfflineRatesValue.Text.Trim <> "" Then
                iTempProductionRatesOfflineSpecificQuotedOfflineRatesValue = CType(txtProductionRatesOfflineSpecificQuotedOfflineRatesValue.Text, Integer)
            End If

            Dim iTempProductionRatesOfflineSpecificPiecesManHourValue As Integer = 0
            If txtProductionRatesOfflineSpecificPiecesManHourValue.Text.Trim <> "" Then
                iTempProductionRatesOfflineSpecificPiecesManHourValue = CType(txtProductionRatesOfflineSpecificPiecesManHourValue.Text, Integer)
            End If

            Dim dTempProductionRatesOfflineSpecificPercentRecycleValue As Double = 0
            If txtProductionRatesOfflineSpecificPercentRecycleValue.Text.Trim <> "" Then
                dTempProductionRatesOfflineSpecificPercentRecycleValue = CType(txtProductionRatesOfflineSpecificPercentRecycleValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresMaxPiecesQuotedValue As Integer = 0
            If txtProductionRatesFinalFiguresMaxPiecesQuotedValue.Text.Trim <> "" Then
                iTempProductionRatesFinalFiguresMaxPiecesQuotedValue = CType(txtProductionRatesFinalFiguresMaxPiecesQuotedValue.Text, Integer)
            End If

            Dim iTempProductionRatesFinalFiguresMaxPiecesQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresMaxPiecesQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresMaxPiecesQuotedUnitID = ddProductionRatesFinalFiguresMaxPiecesQuotedUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresMaxPiecesMaximumValue As Integer = 0
            If txtProductionRatesFinalFiguresMaxPiecesMaximumValue.Text.Trim <> "" Then
                iTempProductionRatesFinalFiguresMaxPiecesMaximumValue = CType(txtProductionRatesFinalFiguresMaxPiecesMaximumValue.Text, Integer)
            End If

            Dim iTempProductionRatesFinalFiguresMaxPiecesMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresMixCapacityMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresMaxPiecesMaximumUnitID = ddProductionRatesFinalFiguresMixCapacityMaximumUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresPressCyclesQuotedValue As Integer = 0
            If txtProductionRatesFinalFiguresPressCyclesQuotedValue.Text.Trim <> "" Then
                iTempProductionRatesFinalFiguresPressCyclesQuotedValue = CType(txtProductionRatesFinalFiguresPressCyclesQuotedValue.Text, Integer)
            End If

            Dim iTempProductionRatesFinalFiguresPressCyclesQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresPressCyclesQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresPressCyclesQuotedUnitID = ddProductionRatesFinalFiguresPressCyclesQuotedUnits.SelectedValue
            End If

            Dim iTempProductionRatesFinalFiguresPressCyclesMaximumValue As Integer = 0
            If txtProductionRatesFinalFiguresPressCyclesMaximumValue.Text.Trim <> "" Then
                iTempProductionRatesFinalFiguresPressCyclesMaximumValue = CType(txtProductionRatesFinalFiguresPressCyclesMaximumValue.Text, Integer)
            End If

            Dim iTempProductionRatesFinalFiguresPressCyclesMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresPressCyclesMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresPressCyclesMaximumUnitID = ddProductionRatesFinalFiguresPressCyclesMaximumUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresLineSpeedQuotedValue As Double = 0
            If txtProductionRatesFinalFiguresLineSpeedQuotedValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresLineSpeedQuotedValue = CType(txtProductionRatesFinalFiguresLineSpeedQuotedValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresLineSpeedQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresLineSpeedQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresLineSpeedQuotedUnitID = ddProductionRatesFinalFiguresLineSpeedQuotedUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresLineSpeedMaximumValue As Double = 0
            If txtProductionRatesFinalFiguresLineSpeedMaximumValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresLineSpeedMaximumValue = CType(txtProductionRatesFinalFiguresLineSpeedMaximumValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresLineSpeedMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresLineSpeedMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresLineSpeedMaximumUnitID = ddProductionRatesFinalFiguresLineSpeedMaximumUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresNetFormingRateQuotedValue As Double = 0
            If txtProductionRatesFinalFiguresNetFormingRateQuotedValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresNetFormingRateQuotedValue = CType(txtProductionRatesFinalFiguresNetFormingRateQuotedValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresNetFormingRateQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresNetFormingRateQuotedUnitID = ddProductionRatesFinalFiguresNetFormingRateQuotedUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresNetFormingRateMaximumValue As Double = 0
            If txtProductionRatesFinalFiguresNetFormingRateMaximumValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresNetFormingRateMaximumValue = CType(txtProductionRatesFinalFiguresNetFormingRateMaximumValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresNetFormingRateMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresNetFormingRateMaximumUnitID = ddProductionRatesFinalFiguresNetFormingRateMaximumUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresMixCapacityQuotedValue As Double = 0
            If txtProductionRatesFinalFiguresMixCapacityQuotedValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresMixCapacityQuotedValue = CType(txtProductionRatesFinalFiguresMixCapacityQuotedValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresMixCapacityQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresMixCapacityQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresMixCapacityQuotedUnitID = ddProductionRatesFinalFiguresMixCapacityQuotedUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresMixCapacityMaximumValue As Double = 0
            If txtProductionRatesFinalFiguresMixCapacityMaximumValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresMixCapacityMaximumValue = CType(txtProductionRatesFinalFiguresMixCapacityMaximumValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresMixCapacityMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresMixCapacityMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresMixCapacityMaximumUnitID = ddProductionRatesFinalFiguresMixCapacityMaximumUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresRecycleRateQuotedValue As Double = 0
            If txtProductionRatesFinalFiguresRecycleRateQuotedValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresRecycleRateQuotedValue = CType(txtProductionRatesFinalFiguresRecycleRateQuotedValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresRecycleRateQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresRecycleRateQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresRecycleRateQuotedUnitID = ddProductionRatesFinalFiguresRecycleRateQuotedUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresRecycleRateMaximumValue As Double = 0
            If txtProductionRatesFinalFiguresRecycleRateMaximumValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresRecycleRateMaximumValue = CType(txtProductionRatesFinalFiguresRecycleRateMaximumValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresRecycleRateMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresRecycleRateMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresRecycleRateMaximumUnitID = ddProductionRatesFinalFiguresRecycleRateMaximumUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresPartWeightQuotedValue As Double = 0
            If txtProductionRatesFinalFiguresPartWeightQuotedValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresPartWeightQuotedValue = CType(txtProductionRatesFinalFiguresPartWeightQuotedValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresPartWeightQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresPartWeightQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresPartWeightQuotedUnitID = ddProductionRatesFinalFiguresPartWeightQuotedUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresPartWeightMaximumValue As Double = 0
            If txtProductionRatesFinalFiguresPartWeightMaximumValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresPartWeightMaximumValue = CType(txtProductionRatesFinalFiguresPartWeightMaximumValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresPartWeightMaximumUnitID As Integer = 0
            If ddProductionRatesFinalFiguresPartWeightMaximumUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresPartWeightMaximumUnitID = ddProductionRatesFinalFiguresPartWeightMaximumUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresCoatingWeightQuotedValue As Double = 0
            If txtProductionRatesFinalFiguresCoatingWeightQuotedValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresCoatingWeightQuotedValue = CType(txtProductionRatesFinalFiguresCoatingWeightQuotedValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresCoatingWeightQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresCoatingWeightQuotedUnitID = ddProductionRatesFinalFiguresCoatingWeightQuotedUnits.SelectedValue
            End If

            Dim dTempProductionRatesFinalFiguresTotalWeightQuotedValue As Double = 0
            If txtProductionRatesFinalFiguresTotalWeightQuotedValue.Text.Trim <> "" Then
                dTempProductionRatesFinalFiguresTotalWeightQuotedValue = CType(txtProductionRatesFinalFiguresTotalWeightQuotedValue.Text, Double)
            End If

            Dim iTempProductionRatesFinalFiguresTotalWeightQuotedUnitID As Integer = 0
            If ddProductionRatesFinalFiguresTotalWeightQuotedUnits.SelectedIndex > 0 Then
                iTempProductionRatesFinalFiguresTotalWeightQuotedUnitID = ddProductionRatesFinalFiguresTotalWeightQuotedUnits.SelectedValue
            End If

            Dim iTempQuotedInfoAccountManagerID As Integer = 0
            If ddQuotedInfoAccountManager.SelectedIndex > 0 Then
                iTempQuotedInfoAccountManagerID = ddQuotedInfoAccountManager.SelectedValue
            End If

            Dim dTempQuotedInfoStandardCostFactor As Double = 1.02
            If txtQuotedInfoStandardCostFactor.Text.Trim <> "" Then
                dTempQuotedInfoStandardCostFactor = CType(txtQuotedInfoStandardCostFactor.Text, Double)
            End If

            Dim iTempQuotedInfoPiecesPerYear As Integer = 0
            If txtQuotedInfoPiecesPerYear.Text.Trim <> "" Then
                iTempQuotedInfoPiecesPerYear = CType(txtQuotedInfoPiecesPerYear.Text, Integer)
            End If

            Dim iTempCompositePartSpecFormulaID As Integer = 0
            If ddCompositePartSpecFormula.SelectedIndex > 0 Then
                iTempCompositePartSpecFormulaID = ddCompositePartSpecFormula.SelectedValue
            End If

            Dim dTempCompositePartSpecPartThicknessValue As Double = 0
            If txtCompositePartSpecPartThicknessValue.Text.Trim <> "" Then
                dTempCompositePartSpecPartThicknessValue = CType(txtCompositePartSpecPartThicknessValue.Text, Double)
            End If

            Dim iTempCompositePartSpecPartThicknessUnitID As Integer = 0
            If ddCompositePartSpecPartThicknessUnits.SelectedIndex > 0 Then
                iTempCompositePartSpecPartThicknessUnitID = ddCompositePartSpecPartThicknessUnits.SelectedValue
            End If

            Dim dTempCompositePartSpecPartSpecificGravityValue As Double = 0
            If txtCompositePartSpecPartSpecificGravityValue.Text.Trim <> "" Then
                dTempCompositePartSpecPartSpecificGravityValue = CType(txtCompositePartSpecPartSpecificGravityValue.Text, Double)
            End If

            Dim iTempCompositePartSpecPartSpecificGravityUnitID As Integer = 0
            If ddCompositePartSpecPartSpecificGravityUnits.SelectedIndex > 0 Then
                iTempCompositePartSpecPartSpecificGravityUnitID = ddCompositePartSpecPartSpecificGravityUnits.SelectedValue
            End If

            Dim dTempCompositePartSpecPartAreaValue As Double = 0
            If txtCompositePartSpecPartAreaValue.Text.Trim <> "" Then
                dTempCompositePartSpecPartAreaValue = CType(txtCompositePartSpecPartAreaValue.Text, Double)
            End If

            Dim iTempCompositePartSpecPartAreaUnitID As Integer = 0
            If ddCompositePartSpecPartAreaUnits.SelectedIndex > 0 Then
                iTempCompositePartSpecPartAreaUnitID = ddCompositePartSpecPartAreaUnits.SelectedValue
            End If

            Dim dTempCompositePartSpecRSSWeightValue As Double = 0
            If txtCompositePartSpecRSSWeightValue.Text.Trim <> "" Then
                dTempCompositePartSpecRSSWeightValue = CType(txtCompositePartSpecRSSWeightValue.Text, Double)
            End If

            Dim iTempCompositePartSpecRSSWeightUnitID As Integer = 0
            If ddCompositePartSpecRSSWeightUnits.SelectedIndex > 0 Then
                iTempCompositePartSpecRSSWeightUnitID = ddCompositePartSpecRSSWeightUnits.SelectedValue
            End If

            Dim dTempCompositePartSpecAntiBlockCoatingValue As Double = 0
            If txtCompositePartSpecAntiBlockCoatingValue.Text.Trim <> "" Then
                dTempCompositePartSpecAntiBlockCoatingValue = CType(txtCompositePartSpecAntiBlockCoatingValue.Text, Double)
            End If

            Dim iTempCompositePartSpecAntiBlockCoatingUnitID As Integer = 0
            If ddCompositePartSpecAntiBlockCoatingUnits.SelectedIndex > 0 Then
                iTempCompositePartSpecAntiBlockCoatingUnitID = ddCompositePartSpecAntiBlockCoatingUnits.SelectedValue
            End If

            Dim dTempCompositePartSpecHotMeldAdhesiveValue As Double = 0
            If txtCompositePartSpecHotMeldAdhesiveValue.Text.Trim <> "" Then
                dTempCompositePartSpecHotMeldAdhesiveValue = CType(txtCompositePartSpecHotMeldAdhesiveValue.Text, Double)
            End If

            Dim iTempCompositePartSpecHotMeldAdhesiveUnitID As Integer = 0
            If ddCompositePartSpecHotMeldAdhesiveUnits.SelectedIndex > 0 Then
                iTempCompositePartSpecHotMeldAdhesiveUnitID = ddCompositePartSpecHotMeldAdhesiveUnits.SelectedValue
            End If

            Dim iTempMoldedBarrierFormulaID As Integer = 0
            If ddMoldedBarrierFormula.SelectedIndex > 0 Then
                iTempMoldedBarrierFormulaID = ddMoldedBarrierFormula.SelectedValue
            End If

            Dim dTempMoldedBarrierApproximateLengthValue As Double = 0
            If txtMoldedBarrierApproximateLengthValue.Text.Trim <> "" Then
                dTempMoldedBarrierApproximateLengthValue = CType(txtMoldedBarrierApproximateLengthValue.Text, Double)
            End If

            Dim iTempMoldedBarrierApproximateLengthUnitID As Integer = 0
            If ddMoldedBarrierApproximateLengthUnits.SelectedIndex > 0 Then
                iTempMoldedBarrierApproximateLengthUnitID = ddMoldedBarrierApproximateLengthUnits.SelectedValue
            End If

            Dim dTempMoldedBarrierApproximateWidthValue As Double = 0
            If txtMoldedBarrierApproximateWidthValue.Text.Trim <> "" Then
                dTempMoldedBarrierApproximateWidthValue = CType(txtMoldedBarrierApproximateWidthValue.Text, Double)
            End If

            Dim iTempMoldedBarrierApproximateWidthUnitID As Integer = 0
            If ddMoldedBarrierApproximateWidthUnits.SelectedIndex > 0 Then
                iTempMoldedBarrierApproximateWidthUnitID = ddMoldedBarrierApproximateWidthUnits.SelectedValue
            End If

            Dim dTempMoldedBarrierApproximateThicknessValue As Double = 0
            If txtMoldedBarrierApproximateThicknessValue.Text.Trim <> "" Then
                dTempMoldedBarrierApproximateThicknessValue = CType(txtMoldedBarrierApproximateThicknessValue.Text, Double)
            End If

            Dim iTempMoldedBarrierApproximateThicknessUnitID As Integer = 0
            If ddMoldedBarrierApproximateThicknessUnits.SelectedIndex > 0 Then
                iTempMoldedBarrierApproximateThicknessUnitID = ddMoldedBarrierApproximateThicknessUnits.SelectedValue
            End If

            Dim dTempMoldedBarrierBlankAreaValue As Double = 0
            If txtMoldedBarrierBlankAreaValue.Text.Trim <> "" Then
                dTempMoldedBarrierBlankAreaValue = CType(txtMoldedBarrierBlankAreaValue.Text, Double)
            End If

            Dim iTempMoldedBarrierBlankAreaUnitID As Integer = 0
            If ddMoldedBarrierBlankAreaUnits.SelectedIndex > 0 Then
                iTempMoldedBarrierBlankAreaUnitID = ddMoldedBarrierBlankAreaUnits.SelectedValue
            End If

            Dim dTempMoldedBarrierSpecificGravityValue As Double = 0
            If txtMoldedBarrierSpecificGravityValue.Text.Trim <> "" Then
                dTempMoldedBarrierSpecificGravityValue = CType(txtMoldedBarrierSpecificGravityValue.Text, Double)
            End If

            Dim iTempMoldedBarrierSpecificGravityUnitID As Integer = 0
            If ddMoldedBarrierSpecificGravityUnits.SelectedIndex > 0 Then
                iTempMoldedBarrierSpecificGravityUnitID = ddMoldedBarrierSpecificGravityUnits.SelectedValue
            End If

            Dim dTempMoldedBarrierWeightPerAreaValue As Double = 0
            If txtMoldedBarrierWeightPerAreaValue.Text.Trim <> "" Then
                dTempMoldedBarrierWeightPerAreaValue = CType(txtMoldedBarrierWeightPerAreaValue.Text, Double)
            End If

            Dim iTempMoldedBarrierWeightPerAreaUnitID As Integer = 0
            If ddMoldedBarrierWeightPerAreaUnits.SelectedIndex > 0 Then
                iTempMoldedBarrierWeightPerAreaUnitID = ddMoldedBarrierWeightPerAreaUnits.SelectedValue
            End If

            Dim dTempMoldedBarrierBlankWeightValue As Double = 0
            If txtMoldedBarrierBlankWeightValue.Text.Trim <> "" Then
                dTempMoldedBarrierBlankWeightValue = CType(txtMoldedBarrierBlankWeightValue.Text, Double)
            End If

            Dim iTempMoldedBarrierBlankWeightUnitID As Integer = 0
            If ddMoldedBarrierBlankWeightUnits.SelectedIndex > 0 Then
                iTempMoldedBarrierBlankWeightUnitID = ddMoldedBarrierBlankWeightUnits.SelectedValue
            End If

            Dim dTempMoldedBarrierAntiBlockCoatingValue As Double = 0
            If txtMoldedBarrierAntiBlockCoatingValue.Text.Trim <> "" Then
                dTempMoldedBarrierAntiBlockCoatingValue = CType(txtMoldedBarrierAntiBlockCoatingValue.Text, Double)
            End If

            Dim iTempMoldedBarrierAntiBlockCoatingUnitID As Integer = 0
            If ddMoldedBarrierAntiBlockCoatingUnits.SelectedIndex > 0 Then
                iTempMoldedBarrierAntiBlockCoatingUnitID = ddMoldedBarrierAntiBlockCoatingUnits.SelectedValue
            End If

            Dim dTempMoldedBarrierTotalWeightValue As Double = 0
            If txtMoldedBarrierTotalWeightValue.Text.Trim <> "" Then
                dTempMoldedBarrierTotalWeightValue = CType(txtMoldedBarrierTotalWeightValue.Text, Double)
            End If

            Dim iTempMoldedBarrierTotalWeightUnitID As Integer = 0
            If ddMoldedBarrierTotalWeightUnits.SelectedIndex > 0 Then
                iTempMoldedBarrierTotalWeightUnitID = ddMoldedBarrierTotalWeightUnits.SelectedValue
            End If

            Dim dTempMaterialCostWOScrapTotalValue As Double = 0
            If txtMaterialCostTotalWOScrapValue.Text.Trim <> "" Then
                dTempMaterialCostWOScrapTotalValue = CType(txtMaterialCostTotalWOScrapValue.Text, Double)
            End If

            Dim dTempMaterialCostTotalValue As Double = 0
            If txtMaterialCostTotalValue.Text.Trim <> "" Then
                dTempMaterialCostTotalValue = CType(txtMaterialCostTotalValue.Text, Double)
            End If

            Dim dTempPackagingCostTotalValue As Double = 0
            If txtPackagingCostTotalValue.Text.Trim <> "" Then
                dTempPackagingCostTotalValue = CType(txtPackagingCostTotalValue.Text, Double)
            End If

            Dim dTempLaborCostWOScrapTotalValue As Double = 0
            If txtLaborCostTotalWOScrapValue.Text.Trim <> "" Then
                dTempLaborCostWOScrapTotalValue = CType(txtLaborCostTotalWOScrapValue.Text, Double)
            End If

            Dim dTempLaborCostTotalValue As Double = 0
            If txtLaborCostTotalValue.Text.Trim <> "" Then
                dTempLaborCostTotalValue = CType(txtLaborCostTotalValue.Text, Double)
            End If

            Dim dTempOverheadCostWOScrapTotalValue As Double = 0
            If txtOverheadCostTotalWOScrapValue.Text.Trim <> "" Then
                dTempOverheadCostWOScrapTotalValue = CType(txtOverheadCostTotalWOScrapValue.Text, Double)
            End If

            Dim dTempOverheadCostTotalValue As Double = 0
            If txtOverheadCostTotalValue.Text.Trim <> "" Then
                dTempOverheadCostTotalValue = CType(txtOverheadCostTotalValue.Text, Double)
            End If

            Dim dTempScrapCostTotalValue As Double = 0
            If lblScrapCostTotalValue.Text.Trim <> "" Then
                dTempScrapCostTotalValue = CType(lblScrapCostTotalValue.Text, Double)
            End If

            Dim dTempCapitalCostTotalValue As Double = 0
            If txtCapitalCostTotalValue.Text.Trim <> "" Then
                dTempCapitalCostTotalValue = CType(txtCapitalCostTotalValue.Text, Double)
            End If

            Dim dTempMiscCostTotalValue As Double = 0
            If txtMiscCostTotalValue.Text.Trim <> "" Then
                dTempMiscCostTotalValue = CType(txtMiscCostTotalValue.Text, Double)
            End If

            Dim dTempSGACostTotalValue As Double = 0
            If lblSGACostTotalValue.Text.Trim <> "" Then
                dTempSGACostTotalValue = CType(lblSGACostTotalValue.Text, Double)
            End If

            Dim dTempOverallCostTotalValue As Double = 0
            If txtOverallCostTotalValue.Text.Trim <> "" Then
                dTempOverallCostTotalValue = CType(txtOverallCostTotalValue.Text, Double)
            End If

            Dim dTempFixedCostTotalValue As Double = 0
            If lblFixedCostTotalValue.Text.Trim <> "" Then
                dTempFixedCostTotalValue = CType(lblFixedCostTotalValue.Text, Double)
            End If

            Dim dTempVariableCostTotalValue As Double = 0
            If lblVariableCostTotalValue.Text.Trim <> "" Then
                dTempVariableCostTotalValue = CType(lblVariableCostTotalValue.Text, Double)
            End If

            Dim dTempPriceVariableMarginPercentTarget As Double = 0
            If lblPriceVariableMarginPercentTargetValue.Text.Trim <> "" Then
                dTempPriceVariableMarginPercentTarget = CType(lblPriceVariableMarginPercentTargetValue.Text.Trim / 100, Double)
            End If

            Dim dTempMinimumSellingPriceValue As Double = 0
            If lblMinimumSellingPriceValue.Text.Trim <> "" Then
                dTempMinimumSellingPriceValue = CType(lblMinimumSellingPriceValue.Text, Double)
            End If

            Dim dTempPriceVariableMarginPercentValue As Double = 0
            If lblPriceVariableMarginPercentValue.Text.Trim <> "" Then
                dTempPriceVariableMarginPercentValue = CType(lblPriceVariableMarginPercentValue.Text, Double) / 100
            End If

            Dim dTempPriceVariableMarginDollarValue As Double = 0
            If lblPriceVariableMarginDollarValue.Text.Trim <> "" Then
                dTempPriceVariableMarginDollarValue = CType(lblPriceVariableMarginDollarValue.Text, Double)
            End If

            Dim dTempPriceVariableMarginInclDeprPercentValue As Double = 0
            If lblPriceVariableMarginInclDeprPercentValue.Text.Trim <> "" Then
                dTempPriceVariableMarginInclDeprPercentValue = CType(lblPriceVariableMarginInclDeprPercentValue.Text, Double) / 100
            End If

            Dim dTempPriceVariableMarginInclDeprDollarValue As Double = 0
            If lblPriceVariableMarginInclDeprDollarValue.Text.Trim <> "" Then
                dTempPriceVariableMarginInclDeprDollarValue = CType(lblPriceVariableMarginInclDeprDollarValue.Text, Double)
            End If

            Dim dTempPriceGrossMarginPercentValue As Double = 0
            If lblPriceGrossMarginPercentValue.Text.Trim <> "" Then
                dTempPriceGrossMarginPercentValue = CType(lblPriceGrossMarginPercentValue.Text, Double) / 100
            End If

            Dim dTempPriceGrossMarginDollarValue As Double = 0
            If lblPriceGrossMarginDollarValue.Text.Trim <> "" Then
                dTempPriceGrossMarginDollarValue = CType(lblPriceGrossMarginDollarValue.Text, Double)
            End If

            'if the cost sheet exists, update it
            If ViewState("CostSheetID") > 0 Then

                CostingModule.UpdateCostSheet(ViewState("CostSheetID"), ddCostSheetStatusValue.SelectedValue, txtQuoteDateValue.Text, iTempRFDNo, iTempECINo, ddUGNFacilityValue.SelectedValue, ddDesignationTypeValue.SelectedValue, txtNewCustomerPartNoValue.Text.Trim, txtNewPartNameValue.Text.Trim, txtNewDesignLevelValue.Text.Trim, txtNewDrawingNoValue.Text.Trim, txtOriginalCustomerPartNoValue.Text.Trim, txtOriginalDesignLevelValue.Text.Trim, iTempCommodityID, iTempPurchasedGoodID, txtNewPartNoValue.Text.Trim, txtNewPartRevisionValue.Text.Trim, txtOriginalPartNoValue.Text, txtOriginalPartRevisionValue.Text.Trim, txtNotesValue.Text.Trim, cbQuickQuote.Checked)

                CostingModule.UpdateCostSheetPartSpecification(ViewState("CostSheetID"), iTempPartSpecificationFormulaID, _
                cbPartSpecificationsIsDiecutValue.Checked, dTempPartSpecificationsThickness, iTempPartSpecificationsThicknessUnitID, cbPartSpecificationsIsCompletedOfflineValue.Checked, _
                iTempPartSpecificationsOffLineRate, iTempPartSpecificationsNumberOfHoles, dTempPartSpecificationsPartWidthValue, iTempPartSpecificationsPartWidthUnitID, _
                dTempPartSpecificationsPartLengthValue, iTempPartSpecificationsPartLengthUnitID, dTempPartSpecificationsConfigurationFactor, txtPartSpecificationsRepackMaterialValue.Text, _
                dTempPartSpecificationsApproxWeightValue, iTempPartSpecificationsApproxWeightUnitID, dTempPartSpecificationsProductionRateValue, 0, _
                dTempPartSpecificationsNumberOfCarriers, dTempPartSpecificationsFoamValue, 0, iTempPartSpecificationsPiecesPerCycleValue, _
                iTempPartSpecificationsPiecesCaughtTogether, cbPartSpecificationsIsSideBySideValue.Checked, dTempPartSpecificationsCalculatedAreaValue, iTempPartSpecificationsCalculatedAreaUnitID, _
                dTempPartSpecificationsChangedAreaValue, iTempPartSpecificationsChangedAreaUnitID, dTempPartSpecificationsDieLayoutWidthValue, iTempPartSpecificationsDieLayoutWidthUnitID, dTempPartSpecificationsDieLayoutTravelValue, iTempPartSpecificationsDieLayoutTravelUnitID, _
                dTempPartSpecificationsWeightPerAreaValue, iTempPartSpecificationsWeightPerAreaUnitID, dTempPartSpecificationsSpecificGravityValue, iTempPartSpecificationsSpecificGravityUnitID, iTempPartSpecificationProcessID)

                CostingModule.UpdateCostSheetProductionRate(ViewState("CostSheetID"), iTempProductionRatesMaxMixCapacityValue, iTempProductionRatesMaxMixCapacityUnitID, _
                iTempProductionRatesMaxFormingRateValue, iTempProductionRatesMaxFormingRateUnitID, dTempProductionRatesCatchingAbilityValue, iTempProductionRatesLineSpeedLimitationValue, _
                dTempProductionRatesCatchPercentValue, dTempProductionRatesCoatingFactorValue, dTempProductionRatesWeightPerAreaValue, iTempProductionRatesWeightPerAreaUnitID, _
                iTempProductionRatesOfflineSpecificSheetsUpValue, txtProductionRatesOfflineSpecificBlankCodeValue.Text, _
                iTempProductionRatesOfflineSpecificQuotedPressCyclesValue, iTempProductionRatesOfflineSpecificQuotedOfflineRatesValue, _
                iTempProductionRatesOfflineSpecificPiecesManHourValue, dTempProductionRatesOfflineSpecificPercentRecycleValue, _
                iTempProductionRatesFinalFiguresMaxPiecesQuotedValue, iTempProductionRatesFinalFiguresMaxPiecesQuotedUnitID, iTempProductionRatesFinalFiguresMaxPiecesMaximumValue, iTempProductionRatesFinalFiguresMaxPiecesMaximumUnitID, _
                iTempProductionRatesFinalFiguresPressCyclesQuotedValue, iTempProductionRatesFinalFiguresPressCyclesQuotedUnitID, iTempProductionRatesFinalFiguresPressCyclesMaximumValue, iTempProductionRatesFinalFiguresPressCyclesMaximumUnitID, _
                dTempProductionRatesFinalFiguresLineSpeedQuotedValue, iTempProductionRatesFinalFiguresLineSpeedQuotedUnitID, dTempProductionRatesFinalFiguresLineSpeedMaximumValue, iTempProductionRatesFinalFiguresLineSpeedMaximumUnitID, _
                dTempProductionRatesFinalFiguresNetFormingRateQuotedValue, iTempProductionRatesFinalFiguresNetFormingRateQuotedUnitID, dTempProductionRatesFinalFiguresNetFormingRateMaximumValue, iTempProductionRatesFinalFiguresNetFormingRateMaximumUnitID, _
                dTempProductionRatesFinalFiguresMixCapacityQuotedValue, iTempProductionRatesFinalFiguresMixCapacityQuotedUnitID, dTempProductionRatesFinalFiguresMixCapacityMaximumValue, iTempProductionRatesFinalFiguresMixCapacityMaximumUnitID, _
                dTempProductionRatesFinalFiguresRecycleRateQuotedValue, iTempProductionRatesFinalFiguresRecycleRateQuotedUnitID, dTempProductionRatesFinalFiguresRecycleRateMaximumValue, iTempProductionRatesFinalFiguresRecycleRateMaximumUnitID, _
                dTempProductionRatesFinalFiguresPartWeightQuotedValue, iTempProductionRatesFinalFiguresPartWeightQuotedUnitID, dTempProductionRatesFinalFiguresPartWeightMaximumValue, iTempProductionRatesFinalFiguresPartWeightMaximumUnitID, _
                dTempProductionRatesFinalFiguresCoatingWeightQuotedValue, iTempProductionRatesFinalFiguresCoatingWeightQuotedUnitID, dTempProductionRatesFinalFiguresTotalWeightQuotedValue, iTempProductionRatesFinalFiguresTotalWeightQuotedUnitID)

                CostingModule.UpdateCostSheetQuotedInfo(ViewState("CostSheetID"), iTempQuotedInfoAccountManagerID, dTempQuotedInfoStandardCostFactor, _
                iTempQuotedInfoPiecesPerYear, txtQuotedInfoComments.Text)

                CostingModule.UpdateCostSheetSketchMemo(ViewState("CostSheetID"), txtDrawingPartSketchMemo.Text)

                CostingModule.UpdateCostSheetCompositePartSpecification(ViewState("CostSheetID"), iTempCompositePartSpecFormulaID, _
                dTempCompositePartSpecPartThicknessValue, iTempCompositePartSpecPartThicknessUnitID, dTempCompositePartSpecPartSpecificGravityValue, iTempCompositePartSpecPartSpecificGravityUnitID, _
                dTempCompositePartSpecPartAreaValue, iTempCompositePartSpecPartAreaUnitID, dTempCompositePartSpecRSSWeightValue, iTempCompositePartSpecRSSWeightUnitID, _
                dTempCompositePartSpecAntiBlockCoatingValue, iTempCompositePartSpecAntiBlockCoatingUnitID, dTempCompositePartSpecHotMeldAdhesiveValue, iTempCompositePartSpecHotMeldAdhesiveUnitID)

                CostingModule.UpdateCostSheetMoldedBarrier(ViewState("CostSheetID"), iTempMoldedBarrierFormulaID, _
                dTempMoldedBarrierApproximateLengthValue, iTempMoldedBarrierApproximateLengthUnitID, dTempMoldedBarrierApproximateWidthValue, iTempMoldedBarrierApproximateWidthUnitID, _
                dTempMoldedBarrierApproximateThicknessValue, iTempMoldedBarrierApproximateThicknessUnitID, dTempMoldedBarrierBlankAreaValue, iTempMoldedBarrierBlankAreaUnitID, _
                dTempMoldedBarrierSpecificGravityValue, iTempMoldedBarrierSpecificGravityUnitID, dTempMoldedBarrierWeightPerAreaValue, iTempMoldedBarrierWeightPerAreaUnitID, _
                dTempMoldedBarrierBlankWeightValue, iTempMoldedBarrierBlankWeightUnitID, dTempMoldedBarrierAntiBlockCoatingValue, iTempMoldedBarrierAntiBlockCoatingUnitID, _
                dTempMoldedBarrierTotalWeightValue, iTempMoldedBarrierTotalWeightUnitID)

                CostingModule.UpdateCostSheetTotal(ViewState("CostSheetID"), dTempMaterialCostWOScrapTotalValue, dTempMaterialCostTotalValue, dTempPackagingCostTotalValue, dTempLaborCostWOScrapTotalValue, dTempLaborCostTotalValue, dTempOverheadCostWOScrapTotalValue, dTempOverheadCostTotalValue, dTempScrapCostTotalValue, dTempCapitalCostTotalValue, dTempMiscCostTotalValue, dTempSGACostTotalValue, dTempOverallCostTotalValue, dTempFixedCostTotalValue, dTempVariableCostTotalValue, dTempPriceVariableMarginPercentTarget, dTempMinimumSellingPriceValue, dTempPriceVariableMarginPercentValue, dTempPriceVariableMarginDollarValue, dTempPriceVariableMarginInclDeprPercentValue, dTempPriceVariableMarginInclDeprDollarValue, dTempPriceGrossMarginPercentValue, dTempPriceGrossMarginDollarValue)

                'if the user changed from locked to edit more, then after saving information, if the cost sheet is approved, return to locked mode
                btnEdit.Visible = False

                If hlnkPreviousCostSheetIDValue.Text.Trim <> "" And ViewState("StatusChanged") = "Current" Then
                    iPreviousCostSheetID = CType(hlnkPreviousCostSheetIDValue.Text, Integer)
                    If iPreviousCostSheetID > 0 Then
                        If ddCostSheetStatusValue.SelectedValue = "Current" Then
                            CostingModule.UpdateCostSheetStatus(iPreviousCostSheetID, "Previous")
                            lblMessage.Text += "<br>The status of the previous cost sheet was changed to PREVIOUS."
                        End If
                    End If
                End If

                If lblApprovedDateValue.Text <> "" Then
                    'if team members have been notified of cost sheet approval yet
                    dsPostApproval = CostingModule.GetCostSheetPostApprovalList(ViewState("CostSheetID"), True, True)
                    If commonFunctions.CheckDataSet(dsPostApproval) = False Then
                        'no team members have been notified yet of approved cost sheet
                        ViewState("isApproved") = True
                    End If

                    EnableControls()
                    BindData()
                End If
            Else
                'insert new cost sheet id
                ViewState("CostSheetID") = 0
                ds = CostingModule.InsertCostSheet(iTempPreviousCostSheetID, ddCostSheetStatusValue.SelectedValue, txtQuoteDateValue.Text, iTempRFDNo, ddUGNFacilityValue.SelectedValue, ddDesignationTypeValue.SelectedValue, txtNewCustomerPartNoValue.Text.Trim, txtNewPartNameValue.Text.Trim, txtNewDesignLevelValue.Text.Trim, txtNewDrawingNoValue.Text.Trim, txtOriginalCustomerPartNoValue.Text.Trim, txtOriginalDesignLevelValue.Text.Trim, iTempCommodityID, iTempPurchasedGoodID, txtNewPartNoValue.Text.Trim, txtNewPartRevisionValue.Text.Trim, txtOriginalPartNoValue.Text, txtOriginalPartRevisionValue.Text.Trim, lblOldOriginalPartNoValue.Text, txtNotesValue.Text.Trim, cbQuickQuote.Checked)

                If commonFunctions.CheckDataSet(ds) = True Then

                    ViewState("CostSheetID") = ds.Tables(0).Rows(0).Item("NewCostSheetID")

                    If ViewState("CostSheetID") > 0 Then
                        lblCostSheetIDValue.Text = ViewState("CostSheetID")

                        CostingModule.InsertCostSheetPartSpecification(ViewState("CostSheetID"), iTempPartSpecificationFormulaID, _
                        cbPartSpecificationsIsDiecutValue.Checked, dTempPartSpecificationsThickness, iTempPartSpecificationsThicknessUnitID, cbPartSpecificationsIsCompletedOfflineValue.Checked, _
                        iTempPartSpecificationsOffLineRate, iTempPartSpecificationsNumberOfHoles, dTempPartSpecificationsPartWidthValue, iTempPartSpecificationsPartWidthUnitID, _
                        dTempPartSpecificationsPartLengthValue, iTempPartSpecificationsPartLengthUnitID, dTempPartSpecificationsConfigurationFactor, txtPartSpecificationsRepackMaterialValue.Text, _
                        dTempPartSpecificationsApproxWeightValue, iTempPartSpecificationsApproxWeightUnitID, dTempPartSpecificationsProductionRateValue, 0, _
                        dTempPartSpecificationsNumberOfCarriers, dTempPartSpecificationsFoamValue, 0, iTempPartSpecificationsPiecesPerCycleValue, _
                        iTempPartSpecificationsPiecesCaughtTogether, cbPartSpecificationsIsSideBySideValue.Checked, dTempPartSpecificationsCalculatedAreaValue, iTempPartSpecificationsCalculatedAreaUnitID, _
                        dTempPartSpecificationsChangedAreaValue, iTempPartSpecificationsChangedAreaUnitID, dTempPartSpecificationsDieLayoutWidthValue, iTempPartSpecificationsDieLayoutWidthUnitID, dTempPartSpecificationsDieLayoutTravelValue, iTempPartSpecificationsDieLayoutTravelUnitID, _
                        dTempPartSpecificationsWeightPerAreaValue, iTempPartSpecificationsWeightPerAreaUnitID, dTempPartSpecificationsSpecificGravityValue, iTempPartSpecificationsSpecificGravityUnitID, iTempPartSpecificationProcessID)

                        CostingModule.InsertCostSheetProductionRate(ViewState("CostSheetID"), iTempProductionRatesMaxMixCapacityValue, iTempProductionRatesMaxMixCapacityUnitID, _
                        iTempProductionRatesMaxFormingRateValue, iTempProductionRatesMaxFormingRateUnitID, dTempProductionRatesCatchingAbilityValue, iTempProductionRatesLineSpeedLimitationValue, _
                        dTempProductionRatesCatchPercentValue, dTempProductionRatesCoatingFactorValue, dTempProductionRatesWeightPerAreaValue, iTempProductionRatesWeightPerAreaUnitID, _
                        iTempProductionRatesOfflineSpecificSheetsUpValue, txtProductionRatesOfflineSpecificBlankCodeValue.Text, _
                        iTempProductionRatesOfflineSpecificQuotedPressCyclesValue, iTempProductionRatesOfflineSpecificQuotedOfflineRatesValue, _
                        iTempProductionRatesOfflineSpecificPiecesManHourValue, dTempProductionRatesOfflineSpecificPercentRecycleValue, _
                        iTempProductionRatesFinalFiguresMaxPiecesQuotedValue, iTempProductionRatesFinalFiguresMaxPiecesQuotedUnitID, iTempProductionRatesFinalFiguresMaxPiecesMaximumValue, iTempProductionRatesFinalFiguresMaxPiecesMaximumUnitID, _
                        iTempProductionRatesFinalFiguresPressCyclesQuotedValue, iTempProductionRatesFinalFiguresPressCyclesQuotedUnitID, iTempProductionRatesFinalFiguresPressCyclesMaximumValue, iTempProductionRatesFinalFiguresPressCyclesMaximumUnitID, _
                        dTempProductionRatesFinalFiguresLineSpeedQuotedValue, iTempProductionRatesFinalFiguresLineSpeedQuotedUnitID, dTempProductionRatesFinalFiguresLineSpeedMaximumValue, iTempProductionRatesFinalFiguresLineSpeedMaximumUnitID, _
                        dTempProductionRatesFinalFiguresNetFormingRateQuotedValue, iTempProductionRatesFinalFiguresNetFormingRateQuotedUnitID, dTempProductionRatesFinalFiguresNetFormingRateMaximumValue, iTempProductionRatesFinalFiguresNetFormingRateMaximumUnitID, _
                        dTempProductionRatesFinalFiguresMixCapacityQuotedValue, iTempProductionRatesFinalFiguresMixCapacityQuotedUnitID, dTempProductionRatesFinalFiguresMixCapacityMaximumValue, iTempProductionRatesFinalFiguresMixCapacityMaximumUnitID, _
                        dTempProductionRatesFinalFiguresRecycleRateQuotedValue, iTempProductionRatesFinalFiguresRecycleRateQuotedUnitID, dTempProductionRatesFinalFiguresRecycleRateMaximumValue, iTempProductionRatesFinalFiguresRecycleRateMaximumUnitID, _
                        dTempProductionRatesFinalFiguresPartWeightQuotedValue, iTempProductionRatesFinalFiguresPartWeightQuotedUnitID, dTempProductionRatesFinalFiguresPartWeightMaximumValue, iTempProductionRatesFinalFiguresPartWeightMaximumUnitID, _
                        dTempProductionRatesFinalFiguresCoatingWeightQuotedValue, iTempProductionRatesFinalFiguresCoatingWeightQuotedUnitID, dTempProductionRatesFinalFiguresTotalWeightQuotedValue, iTempProductionRatesFinalFiguresTotalWeightQuotedUnitID)

                        CostingModule.InsertCostSheetQuotedInfo(ViewState("CostSheetID"), iTempQuotedInfoAccountManagerID, dTempQuotedInfoStandardCostFactor, _
                        iTempQuotedInfoPiecesPerYear, txtQuotedInfoComments.Text)

                        CostingModule.InsertCostSheetSketchMemo(ViewState("CostSheetID"), txtDrawingPartSketchMemo.Text)

                        CostingModule.InsertCostSheetCompositePartSpecification(ViewState("CostSheetID"), iTempCompositePartSpecFormulaID, _
                        dTempCompositePartSpecPartThicknessValue, iTempCompositePartSpecPartThicknessUnitID, dTempCompositePartSpecPartSpecificGravityValue, iTempCompositePartSpecPartSpecificGravityUnitID, _
                        dTempCompositePartSpecPartAreaValue, iTempCompositePartSpecPartAreaUnitID, dTempCompositePartSpecRSSWeightValue, iTempCompositePartSpecRSSWeightUnitID, _
                        dTempCompositePartSpecAntiBlockCoatingValue, iTempCompositePartSpecAntiBlockCoatingUnitID, dTempCompositePartSpecHotMeldAdhesiveValue, iTempCompositePartSpecHotMeldAdhesiveUnitID)

                        CostingModule.InsertCostSheetMoldedBarrier(ViewState("CostSheetID"), iTempMoldedBarrierFormulaID, _
                        dTempMoldedBarrierApproximateLengthValue, iTempMoldedBarrierApproximateLengthUnitID, dTempMoldedBarrierApproximateWidthValue, iTempMoldedBarrierApproximateWidthUnitID, _
                        dTempMoldedBarrierApproximateThicknessValue, iTempMoldedBarrierApproximateThicknessUnitID, dTempMoldedBarrierBlankAreaValue, iTempMoldedBarrierBlankAreaUnitID, _
                        dTempMoldedBarrierSpecificGravityValue, iTempMoldedBarrierSpecificGravityUnitID, dTempMoldedBarrierWeightPerAreaValue, iTempMoldedBarrierWeightPerAreaUnitID, _
                        dTempMoldedBarrierBlankWeightValue, iTempMoldedBarrierBlankWeightUnitID, dTempMoldedBarrierAntiBlockCoatingValue, iTempMoldedBarrierAntiBlockCoatingUnitID, _
                        dTempMoldedBarrierTotalWeightValue, iTempMoldedBarrierTotalWeightUnitID)

                        CostingModule.InsertCostSheetTotal(ViewState("CostSheetID"), dTempMaterialCostWOScrapTotalValue, dTempMaterialCostTotalValue, dTempPackagingCostTotalValue, dTempLaborCostWOScrapTotalValue, dTempLaborCostTotalValue, dTempOverheadCostWOScrapTotalValue, dTempOverheadCostTotalValue, dTempScrapCostTotalValue, dTempCapitalCostTotalValue, dTempMiscCostTotalValue, dTempSGACostTotalValue, dTempOverallCostTotalValue, dTempFixedCostTotalValue, dTempVariableCostTotalValue, dTempPriceVariableMarginPercentTarget, dTempMinimumSellingPriceValue, dTempPriceVariableMarginPercentValue, dTempPriceVariableMarginDollarValue, dTempPriceVariableMarginInclDeprPercentValue, dTempPriceVariableMarginInclDeprDollarValue, dTempPriceGrossMarginPercentValue, dTempPriceGrossMarginDollarValue)

                        accCostCalculations.SelectedIndex = 0
                    End If

                End If

                If ViewState("CostSheetID") = 0 Then
                    lblMessage.Text += "There was an error saving the new cost sheet. Please contact IS."
                Else
                    Response.Redirect("Cost_Sheet_Detail.aspx?CostSheetID=" & ViewState("CostSheetID"), False)
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        Try
            ClearMessages()

            CostingModule.DeleteCostSheet(ViewState("CostSheetID"))

            Session("DeletedCostSheet") = ViewState("CostSheetID").ToString

            Response.Redirect("Cost_Sheet_List.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click

        Try
            ClearMessages()

            HttpContext.Current.Session("CopyCostSheet") = Nothing

            '01/11/2010 - Dan Cade - when replicating an old quote, do not update the status of the old quote. Instead
            'The new quote will either be pending or proposal. When the new quote has its status changed to current, then the old quote 
            'will be set to previous
            'if cost sheet is current and new quote will be Pending then change current to previous - dont change proposals
            'If ddCostSheetStatusValue.SelectedValue = "Current" And rbCostStatusType.SelectedValue = "Pending" Then
            '    CostingModule.UpdateCostSheetStatus(ViewState("CostSheetID"), "Previous")
            'End If

            'change versions and IDs           
            hlnkPreviousCostSheetIDValue.Text = ViewState("CostSheetID")
            ViewState("CostSheetID") = 0

            'reset certain values
            'dcade 2009-nov-18 - keep NewCustomerPartNo,NewPartNo, NewDesignLevel,NewDrawingNo
            txtQuoteDateValue.Text = Today.Date
            txtRFDNoValue.Text = ""
            txtECINoValue.Text = ""
            'txtNewCustomerPartNoValue.Text = ""
            'txtNewDesignLevelValue.Text = ""
            'txtNewDrawingNoValue.Text = ""
            'txtNewPartNoValue.Text = ""
            txtNotesValue.Text = ""
            lblApprovedDateValue.Text = ""
            lblOldFinishedGoodPartNoLabel.Visible = False
            lblOldFinishedGoodPartNoValue.Text = ""
            lblOldMakeLabel.Visible = False
            lblOldMakeValue.Text = ""
            lblOldModelLabel.Visible = False
            lblOldModelValue.Text = ""
            lblOldYearLabel.Visible = False
            lblOldYearValue.Text = ""
            'lblOldOriginalPartNoLabel.Visible = False
            'lblOldOriginalPartNoValue.Text = ""
            lblOldPartNoLabel.Visible = False
            lblOldPartNoValue.Text = ""

            'clear out in order to allow calculation to get fresh value from formula
            txtProductionRatesCoatingFactorValue.Text = ""

            'pending or proposal replication
            ddCostSheetStatusValue.SelectedValue = rbCostStatusType.SelectedValue

            '2011-Oct-27 always get formula
            GetFormulaTopLevelDetails(ddPartSpecificationsFormulaValue.SelectedValue)
            'CopyFormulaFullDetails(ddPartSpecificationsFormulaValue.SelectedValue)
            'update Part Specification Tab
            If ViewState("Formula_SpecificGravity") > 0 Then
                txtPartSpecificationsSpecificGravityValue.Text = ViewState("Formula_SpecificGravity")
                'Else
                '    txtPartSpecificationsSpecificGravityValue.Text = ""
            End If

            'If ViewState("Formula_WeightPerArea") > 0 Then
            '    txtPartSpecificationsWeightPerAreaValue.Text = ViewState("Formula_WeightPerArea")
            'Else
            '    txtPartSpecificationsWeightPerAreaValue.Text = ""
            'End If

            'cbPartSpecificationsIsDiecutValue.Checked = ViewState("Formula_isDiecut")
            'ddPartSpecificationsProcessValue.SelectedValue = ViewState("Formula_ProcessID")

            ''clear out in order to allow calculation to get fresh value from formula
            'txtProductionRatesCoatingFactorValue.Text = ""

            'save new values, NOT on grids
            Call btnSave_Click(sender, e)

            CopyImage(hlnkPreviousCostSheetIDValue.Text)

            'need to copy and save grids
            CostingModule.CopyCostSheetCustomerProgram(ViewState("CostSheetID"), hlnkPreviousCostSheetIDValue.Text)
            CostingModule.CopyCostSheetDepartment(ViewState("CostSheetID"), hlnkPreviousCostSheetIDValue.Text, rbCopyInformationType.SelectedValue, ddPartSpecificationsFormulaValue.SelectedValue)
            CostingModule.CopyCostSheetAdditionalOfflineRate(ViewState("CostSheetID"), hlnkPreviousCostSheetIDValue.Text, rbCopyInformationType.SelectedValue)
            CostingModule.CopyCostSheetMaterial(ViewState("CostSheetID"), hlnkPreviousCostSheetIDValue.Text, rbCopyInformationType.SelectedValue, ddPartSpecificationsFormulaValue.SelectedValue)
            CostingModule.CopyCostSheetPackaging(ViewState("CostSheetID"), hlnkPreviousCostSheetIDValue.Text, rbCopyInformationType.SelectedValue, ddPartSpecificationsFormulaValue.SelectedValue)
            CostingModule.CopyCostSheetLabor(ViewState("CostSheetID"), hlnkPreviousCostSheetIDValue.Text, rbCopyInformationType.SelectedValue, ddPartSpecificationsFormulaValue.SelectedValue)
            CostingModule.CopyCostSheetOverhead(ViewState("CostSheetID"), hlnkPreviousCostSheetIDValue.Text, rbCopyInformationType.SelectedValue, ddPartSpecificationsFormulaValue.SelectedValue)
            CostingModule.CopyCostSheetMiscCost(ViewState("CostSheetID"), hlnkPreviousCostSheetIDValue.Text, rbCopyInformationType.SelectedValue, ddPartSpecificationsFormulaValue.SelectedValue)
            CostingModule.CopyCostSheetCapital(ViewState("CostSheetID"), hlnkPreviousCostSheetIDValue.Text, rbCopyInformationType.SelectedValue)

            'check for obsoleted materials and get new materials with same BPCS
            If rbCopyInformationType.SelectedValue = "CostSheet" Then
                CostingModule.CopyCostSheetMaterialReplaceObsolete(ViewState("CostSheetID"), hlnkPreviousCostSheetIDValue.Text)
                CostingModule.CopyCostSheetPackagingReplaceObsolete(ViewState("CostSheetID"), hlnkPreviousCostSheetIDValue.Text)
            End If

            lblMessage.Text += "The information has been copied and saved."

            HttpContext.Current.Session("CopyCostSheet") = "Copied"

            'refresh/redirect page
            Response.Redirect("Cost_Sheet_Detail.aspx?CostSheetID=" & ViewState("CostSheetID"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub ddPartSpecificationsFormulaValue_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddPartSpecificationsFormulaValue.SelectedIndexChanged

        Try
            ClearMessages()

            'need to put a warning to use that changing the formula was wipe out values and be saved.

            If ddPartSpecificationsFormulaValue.SelectedIndex > 0 Then

                GetFormulaTopLevelDetails(ddPartSpecificationsFormulaValue.SelectedValue)
                CopyFormulaFullDetails(ddPartSpecificationsFormulaValue.SelectedValue)

                btnSave_Click(sender, e)
                btnCalculate_Click(sender, e)

                EnableControls()

            End If

            lblMessage.Text += "The formula has been changed. The cost sheet has been saved and re-calculated."

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub cbPartSpecificationsIsDiecutValue_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbPartSpecificationsIsDiecutValue.CheckedChanged

        Try
            ClearMessages()

            btnPreviewDieLayout.Visible = cbPartSpecificationsIsDiecutValue.Checked

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub ddCostSheetStatusValue_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCostSheetStatusValue.SelectedIndexChanged

        Try
            ClearMessages()

            ViewState("StatusChanged") = ""

            Dim iPreviousCostSheetID As Integer

            If hlnkPreviousCostSheetIDValue.Text.Trim <> "" Then
                iPreviousCostSheetID = CType(hlnkPreviousCostSheetIDValue.Text, Integer)
                If iPreviousCostSheetID > 0 Then
                    If ddCostSheetStatusValue.SelectedValue = "Current" Then
                        'CostingModule.UpdateCostSheetStatus(iPreviousCostSheetID, "Previous")
                        'lblMessage.Text += "<br>The status of the previous cost sheet was changed to PREVIOUS."
                        ViewState("StatusChanged") = "Current"
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub ddDesignationTypeValue_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddDesignationTypeValue.SelectedIndexChanged

        Try
            ClearMessages()

            CheckDesignationType()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    'Protected Sub gvTopLevelInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTopLevelInfo.RowDataBound

    '    Try
    '        ' Build the client script to open a popup window containing
    '        ' SubDrawings. Pass the ClientID of 4 the 
    '        ' four TextBoxes (which will receive data from the popup)
    '        ' in a query string.

    '        Dim strWindowAttribs As String = _
    '            "width=950px," & _
    '            "height=550px," & _
    '            "left='+((screen.width-950)/2)+'," & _
    '            "top='+((screen.height-550)/2)+'," & _
    '            "resizable=yes,scrollbars=yes,status=yes"

    '        If (e.Row.RowType = DataControlRowType.Footer) Then
    '            Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnSearchTopLevelInfo"), ImageButton)
    '            Dim txtFooterTopLevelPartNo As TextBox = CType(e.Row.FindControl("txtFooterTopLevelPartNo"), TextBox)
    '            Dim txtFooterPartRevision As TextBox = CType(e.Row.FindControl("txtFooterPartRevision"), TextBox)
    '            Dim txtFooterPartName As TextBox = CType(e.Row.FindControl("txtFooterPartName"), TextBox)
    '            If ibtn IsNot Nothing Then

    '                Dim strPagePath As String = _
    '                "../DataMaintenance/PartNoLookUp.aspx?vcPartNo=" & txtFooterTopLevelPartNo.ClientID & "&vcPartRevision=" & txtFooterPartRevision.ClientID & "&vcPartDescr=" & txtFooterPartName.ClientID
    '                Dim strClientScript As String = _
    '                    "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
    '                    strWindowAttribs & "');return false;"
    '                ibtn.Attributes.Add("onClick", strClientScript)
    '            End If


    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    '    lblMessageLowerPage.Text = lblMessage.Text
    '    lblMessageHeader.Text = lblMessage.Text

    'End Sub

    'Protected Sub ibtnGetNewCustomerPartNo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnGetNewCustomerPartNo.Click

    '    Try

    '        ClearMessages()

    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Customer PartNo Search", "window.open('../DataMaintenance/CustomerPartNoLookUp.aspx?CustomervcPartNo=" & txtNewCustomerPartNoValue.ClientID & "&CustomerPartNo=" & txtNewCustomerPartNoValue.Text.Trim & "&CABBV=" & ddCustomer.SelectedValue & "'," & Now.Ticks & ",'resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=yes,location=no');", True)

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    '    lblMessageLowerPage.Text = lblMessage.Text
    '    lblMessageHeader.Text = lblMessage.Text

    'End Sub

    Protected Sub gvMaterial_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMaterial.RowDataBound

        Try
            Dim strBackColor As String = String.Empty
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim lbCostPerUnit As Label = TryCast(e.Row.FindControl("lblViewMaterialCostPerUnit"), Label)
                Dim lbNewQuoteCost As Label = TryCast(e.Row.FindControl("lblViewMaterialNewQuoteCost"), Label)
                Dim lbMismatchedQuoteAndPurchasedCost As Label = TryCast(e.Row.FindControl("lblViewMaterialMismatchedQuoteAndPurchasedCost"), Label)
                Dim lbMismatchedFreightPlusStandardCost As Label = TryCast(e.Row.FindControl("lblViewMaterialMismatchedFreightPlusStandardCost"), Label)

                If lbNewQuoteCost IsNot Nothing Then
                    If lbNewQuoteCost.Text = 1 Then
                        'e.Row.BackColor = Color.Yellow
                        lbCostPerUnit.BackColor = Color.Yellow
                    End If
                End If

                If lbMismatchedQuoteAndPurchasedCost IsNot Nothing Then
                    If lbMismatchedQuoteAndPurchasedCost.Text = 1 Then
                        lbCostPerUnit.ForeColor = Color.Red
                    End If
                End If

                If lbMismatchedFreightPlusStandardCost IsNot Nothing Then
                    If lbMismatchedFreightPlusStandardCost.Text = 1 Then
                        lbCostPerUnit.Font.Bold = True
                        'e.Row.Cells(4).BorderStyle = BorderStyle.Double
                        e.Row.Cells(4).BorderWidth = 5
                        e.Row.Cells(4).BorderColor = Color.Aqua
                    End If
                End If

            End If

            ' Build the client script to open a popup window containing
            ' Materials. Pass the ClientID of the ddFooterDropdown box and Quote Cost Text Box
            ' (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            If (e.Row.RowType = DataControlRowType.Footer) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnGetMaterial"), ImageButton)
                Dim ddTempMaterial As DropDownList = CType(e.Row.FindControl("ddFooterMaterial"), DropDownList)
                Dim txtTempMaterialCostPerUnit As TextBox = CType(e.Row.FindControl("txtFooterMaterialCostPerUnit"), TextBox)
                Dim txtTempMaterialFreightCost As TextBox = CType(e.Row.FindControl("txtFooterMaterialFreightCost"), TextBox)

                If ibtn IsNot Nothing Then

                    Dim strPagePath As String = _
                        "Material_LookUp.aspx?ddMaterialControlID=" & ddTempMaterial.ClientID & "&txtQuoteCostControlID=" & txtTempMaterialCostPerUnit.ClientID & "&txtFreightCostControlID=" & txtTempMaterialFreightCost.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','Materials','" & _
                        strWindowAttribs & "');return false;"
                    ibtn.Attributes.Add("onClick", strClientScript)
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvPackaging_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPackaging.RowDataBound

        Try
            Dim strBackColor As String = String.Empty
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim lbCostPerUnit As Label = TryCast(e.Row.FindControl("lblViewPackagingCostPerUnit"), Label)

                Dim lbNewQuoteCost As Label = TryCast(e.Row.FindControl("lblViewPackagingNewQuoteCost"), Label)
                Dim lbMismatchedQuoteAndPurchasedCost As Label = TryCast(e.Row.FindControl("lblViewPackagingMismatchedQuoteAndPurchasedCost"), Label)

                If lbNewQuoteCost IsNot Nothing Then
                    If lbNewQuoteCost.Text = 1 Then
                        'e.Row.BackColor = Color.Yellow
                        lbCostPerUnit.BackColor = Color.Yellow
                    End If
                End If

                If lbMismatchedQuoteAndPurchasedCost IsNot Nothing Then
                    If lbMismatchedQuoteAndPurchasedCost.Text = 1 Then
                        lbCostPerUnit.ForeColor = Color.Red
                    End If
                End If

            End If

            ' Build the client script to open a popup window containing
            ' Materials. Pass the ClientID of the ddFooterDropdown box and Quote Cost Text Box
            ' (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            If (e.Row.RowType = DataControlRowType.Footer) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnGetPackaging"), ImageButton)
                Dim ddTempPackaging As DropDownList = CType(e.Row.FindControl("ddFooterPackaging"), DropDownList)
                Dim txtTempPackagingCostPerUnit As TextBox = CType(e.Row.FindControl("txtFooterPackagingCostPerUnit"), TextBox)

                If ibtn IsNot Nothing Then

                    'call material popup but filter only packaging as a default
                    Dim strPagePath As String = _
                        "Material_LookUp.aspx?ddMaterialControlID=" & ddTempPackaging.ClientID & "&txtQuoteCostControlID=" & txtTempPackagingCostPerUnit.ClientID & "&isPackaging=1&filterPackaging=1"
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','Materials','" & _
                        strWindowAttribs & "');return false;"
                    ibtn.Attributes.Add("onClick", strClientScript)
                End If
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvLabor_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLabor.RowDataBound

        Try
            Dim strBackColor As String = String.Empty
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim lbRate As Label = TryCast(e.Row.FindControl("lblViewLaborRate"), Label)
                Dim lbNewRate As Label = TryCast(e.Row.FindControl("lblViewLaborNewRate"), Label)

                If lbNewRate IsNot Nothing Then
                    If lbNewRate.Text = 1 Then
                        'e.Row.BackColor = Color.Yellow
                        lbRate.BackColor = Color.Yellow
                    End If
                End If

                Dim lbCrewSize As Label = TryCast(e.Row.FindControl("lblViewLaborCrewSize"), Label)
                Dim lbNewCrewSize As Label = TryCast(e.Row.FindControl("lblViewLaborNewCrewSize"), Label)

                If lbNewCrewSize IsNot Nothing Then
                    If lbNewCrewSize.Text = 1 Then
                        lbCrewSize.BackColor = Color.Yellow
                    End If
                End If

                Dim cbIsOffline As CheckBox = TryCast(e.Row.FindControl("cbViewLaborIsOffline"), CheckBox)
                Dim lbNewOffline As Label = TryCast(e.Row.FindControl("lblViewLaborNewOffline"), Label)

                If lbNewOffline IsNot Nothing Then
                    If lbNewOffline.Text = 1 Then
                        cbIsOffline.BackColor = Color.Yellow
                    End If
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvOverhead_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOverhead.RowDataBound

        Try
            Dim strBackColor As String = String.Empty
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim lbRate As Label = TryCast(e.Row.FindControl("lblViewOverheadRate"), Label)
                Dim lbNewRate As Label = TryCast(e.Row.FindControl("lblViewOverheadNewRate"), Label)

                If lbNewRate IsNot Nothing Then
                    If lbNewRate.Text = 1 Then
                        'e.Row.BackColor = Color.Yellow
                        lbRate.BackColor = Color.Yellow
                    End If
                End If

                Dim lbCrewSize As Label = TryCast(e.Row.FindControl("lblViewOverheadCrewSize"), Label)
                Dim lbNewCrewSize As Label = TryCast(e.Row.FindControl("lblViewOverheadNewCrewSize"), Label)

                If lbNewCrewSize IsNot Nothing Then
                    If lbNewCrewSize.Text = 1 Then
                        lbCrewSize.BackColor = Color.Yellow
                    End If
                End If

                Dim cbIsOffline As CheckBox = TryCast(e.Row.FindControl("cbViewOverheadIsOffline"), CheckBox)
                Dim lblNewOffline As Label = TryCast(e.Row.FindControl("lblViewOverheadNewOffline"), Label)

                If lblNewOffline IsNot Nothing Then
                    If lblNewOffline.Text = 1 Then
                        cbIsOffline.BackColor = Color.Yellow
                    End If
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvMiscCost_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMiscCost.RowDataBound

        Try
            Dim strBackColor As String = String.Empty
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim lbRate As Label = TryCast(e.Row.FindControl("lblViewMiscCostRate"), Label)
                Dim lbNewRate As Label = TryCast(e.Row.FindControl("lblViewMiscCostNewRate"), Label)

                If lbNewRate IsNot Nothing Then
                    If lbNewRate.Text = 1 Then
                        'e.Row.BackColor = Color.Yellow
                        lbRate.BackColor = Color.Yellow
                    End If
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Try
            ClearMessages()

            ViewState("isApproved") = False

            EnableControls()

            lblMessage.Text = "You are now editing an approved cost sheet. After you save your information, the web page will return back to a locked mode.<BR> If you need to change more information, simply click the edit button again."

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    'Protected Sub ibtnGetOriginalCustomerPartNo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnGetOriginalCustomerPartNo.Click

    '    Try
    '        ClearMessages()
    '        '(LREY) 01/08/2014
    '        'Dim strCABBV As String = commonFunctions.GetCustomerCABBV(ddCustomer.SelectedValue)

    '        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "Customer PartNo Search", "window.open('../DataMaintenance/CustomerPartNoLookUp.aspx?CustomervcPartNo=" & txtOriginalCustomerPartNoValue.ClientID & "&CustomerPartNo=" & txtOriginalCustomerPartNoValue.Text.Trim & "&CABBV=" & strCABBV & "'," & Now.Ticks & ",'resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=yes,location=no');", True)

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    '    lblMessageLowerPage.Text = lblMessage.Text
    '    lblMessageHeader.Text = lblMessage.Text

    'End Sub
    
    Protected Sub btnSaveUploadDrawingPartSketchImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveUploadDrawingPartSketchImage.Click

        Try
            ClearMessages()

            Dim ds As DataSet

            If Not uploadImage.HasFile Then
                ''-- Missing file selection
                lblMessage.Text += "<br>Please choose a file to upload"
            Else
                If InStr(UCase(uploadImage.FileName), ".JPG") = 0 Then
                    '-- Selection of non-JPG file
                    lblMessage.Text += "<br>You can upload only JPG files"
                Else
                    If uploadImage.PostedFile.ContentLength > 250000 Then
                        '-- File too large
                        lblMessage.Text += "<br>Uploaded file size must be less than 250 KB"
                    Else
                        'Load FileUpload's InputStream into Byte array
                        Dim imageBytes(uploadImage.PostedFile.InputStream.Length) As Byte
                        uploadImage.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length)

                        'check if image already exists              
                        ds = CostingModule.GetCostSheetSketchInfo(ViewState("CostSheetID"))
                        'If commonFunctions.CheckDataSet(ds) = True Then
                        'CostingModule.UpdateCostSheetSketchImage(ViewState("CostSheetID"), imageBytes)
                        'Else
                        '    CostingModule.InsertCostSheetSketchImage(ViewState("CostSheetID"), imageBytes)                       
                        'End If

                        'if missing, then create record
                        If commonFunctions.CheckDataSet(ds) = False Then
                            CostingModule.InsertCostSheetSketchMemo(ViewState("CostSheetID"), txtDrawingPartSketchMemo.Text)
                        End If

                        CostingModule.UpdateCostSheetSketchImage(ViewState("CostSheetID"), imageBytes)

                        imgDrawingPartSketch.Src = "Display_Sketch_Image.aspx?CostSheetID=" & ViewState("CostSheetID")

                        btnDeleteDrawingPartSketchImage.Visible = True


                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text


    End Sub

    Protected Sub btnDeleteDrawingPartSketchImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDrawingPartSketchImage.Click

        Try
            ClearMessages()

            CostingModule.DeleteCostSheetSketchImage(ViewState("CostSheetID"))

            imgDrawingPartSketch.Src = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub btnRemoveMaterials_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveMaterials.Click

        Try
            ClearMessages()

            Dim ds As DataSet = CostingModule.GetCostSheetMaterial(ViewState("CostSheetID"))
            Dim iRowCounter As Integer = 0

            If commonFunctions.CheckDataset(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            CostingModule.DeleteCostSheetMaterial(ds.Tables(0).Rows(iRowCounter).Item("RowID"))
                        End If
                    End If
                Next

                gvMaterial.DataBind()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub btnRemovePackaging_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemovePackaging.Click
        Try
            ClearMessages()

            Dim ds As DataSet = CostingModule.GetCostSheetPackaging(ViewState("CostSheetID"))
            Dim iRowCounter As Integer = 0

            If commonFunctions.CheckDataset(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            CostingModule.DeleteCostSheetPackaging(ds.Tables(0).Rows(iRowCounter).Item("RowID"))
                        End If
                    End If
                Next

                gvPackaging.DataBind()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub btnRemoveLabor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveLabor.Click

        Try
            ClearMessages()

            Dim ds As DataSet = CostingModule.GetCostSheetLabor(ViewState("CostSheetID"), 0, False, False)
            Dim iRowCounter As Integer = 0

            If commonFunctions.CheckDataset(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            CostingModule.DeleteCostSheetLabor(ds.Tables(0).Rows(iRowCounter).Item("RowID"))
                        End If
                    End If
                Next

                gvLabor.DataBind()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub btnRemoveOverhead_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveOverhead.Click

        Try
            ClearMessages()

            Dim ds As DataSet = CostingModule.GetCostSheetOverhead(ViewState("CostSheetID"), 0)
            Dim iRowCounter As Integer = 0

            If commonFunctions.CheckDataset(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            CostingModule.DeleteCostSheetOverhead(ds.Tables(0).Rows(iRowCounter).Item("RowID"))
                        End If
                    End If
                Next

                gvOverhead.DataBind()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub btnRemoveMiscCost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveMiscCost.Click

        Try
            ClearMessages()

            Dim ds As DataSet = CostingModule.GetCostSheetMiscCost(ViewState("CostSheetID"))
            Dim iRowCounter As Integer = 0

            If commonFunctions.CheckDataset(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            CostingModule.DeleteCostSheetMiscCost(ds.Tables(0).Rows(iRowCounter).Item("RowID"))
                        End If
                    End If
                Next

                gvMiscCost.DataBind()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub btnRemoveCapital_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveCapital.Click

        Try
            ClearMessages()

            Dim ds As DataSet = CostingModule.GetCostSheetCapital(ViewState("CostSheetID"))
            Dim iRowCounter As Integer = 0

            If commonFunctions.CheckDataset(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            CostingModule.DeleteCostSheetCapital(ds.Tables(0).Rows(iRowCounter).Item("RowID"))
                        End If
                    End If
                Next

                gvCapital.DataBind()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub btnRemoveAdditionalOfflineRate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveAdditionalOfflineRate.Click

        Try
            ClearMessages()

            Dim ds As DataSet = CostingModule.GetCostSheetAdditionalOfflineRate(ViewState("CostSheetID"), 0)
            Dim iRowCounter As Integer = 0

            If commonFunctions.CheckDataset(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            CostingModule.DeleteCostSheetAdditionalOfflineRate(ds.Tables(0).Rows(iRowCounter).Item("RowID"))
                        End If
                    End If
                Next
                gvAdditionalOfflineRate.DataBind()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvCustomerProgram_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCustomerProgram.DataBound

        'hide header of first column
        If gvCustomerProgram.Rows.Count > 0 Then
            gvCustomerProgram.HeaderRow.Cells(0).Visible = False
        End If

        'hide header of first column
        If gvCustomerProgram.Rows.Count > 0 Then
            gvCustomerProgram.HeaderRow.Cells(1).Visible = False
        End If

    End Sub

    Protected Sub gvCustomerProgram_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCustomerProgram.RowCreated

        Try

            'hide first column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            'hide second column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            '' From Andrew Robinson's Insert Empty GridView solution
            '' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            '' when binding a row, look for a zero row condition based on the flag.
            '' if we have zero data rows (but a dummy row), hide the grid view row
            '' and clear the controls off of that row so they don't cause binding errors

            'Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CostSheetCustomerProgram
            'If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            '    e.Row.Visible = False
            '    e.Row.Controls.Clear()
            'End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br>"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ClearMessages()

        Try
            lblMessage.Text = ""
            lblMessageAdditionalOfflineRates.Text = ""
            lblMessageCapital.Text = ""
            lblMessageCompositePartSpec.Text = ""
            lblMessageDrawings.Text = ""
            lblMessageHeader.Text = ""
            lblMessageLowerPage.Text = ""
            lblMessageMaterial.Text = ""
            lblMessageMiscCost.Text = ""
            lblMessageMoldedBarrier.Text = ""
            lblMessageOverhead.Text = ""
            lblMessagePackaging.Text = ""
            lblMessagePartSpecifications.Text = ""
            lblMessageLabor.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnAddToCustomerProgram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddToCustomerProgram.Click

        Try
            ClearMessages()

            If ViewState("CostSheetID") = 0 Then
                Call btnSave_Click(sender, e)
            End If

            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0

            Dim dSOP As DateTime
            Dim dEOP As DateTime

            '(LREY) 01/08/2014
            ' ''Dim strCABBV As String = ""
            ' ''Dim iSoldTo As Integer = 0

            If ddProgram.SelectedValue <> "" Then
                iProgramID = ddProgram.SelectedValue

                If InStr(ddProgram.SelectedItem.Text, "**") > 0 Then
                    lblMessage.Text &= "Error: An obsolete program cannot be selected. The information was NOT saved."
                Else

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
                        CostingModule.InsertCostSheetCustomerProgram(ViewState("CostSheetID"), "", 0, iProgramID, iProgramYear)
                    End If

                    If HttpContext.Current.Session("BLLerror") IsNot Nothing Then
                        lblMessage.Text += HttpContext.Current.Session("BLLerror")
                    Else
                        HttpContext.Current.Session("BLLerror") = Nothing
                        lblMessage.Text += "Program and Customer were added."
                    End If

                    gvCustomerProgram.DataBind()

                    cddMakes.SelectedValue = Nothing

                    ddYear.SelectedIndex = -1
                    'ddCustomer.SelectedIndex = -1

                    txtSOPDate.Text = ""
                    txtEOPDate.Text = ""

                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblCustomerProgram.Text = lblMessage.Text

    End Sub

    'Protected Sub ddMakeValue_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddMakeValue.SelectedIndexChanged

    '    lblMessage.Text = ""

    '    Try
    '        Dim dsProgram As DataSet

    '        If ddMakeValue.SelectedIndex > 0 Then
    '            dsProgram = commonFunctions.GetProgram("", "", ddMakeValue.SelectedValue)
    '            If commonFunctions.CheckDataset(dsProgram) = True Then
    '                ddProgramValue.Items.Clear()
    '                ddProgramValue.DataSource = dsProgram
    '                ddProgramValue.DataTextField = dsProgram.Tables(0).Columns("ddProgramName").ColumnName.ToString()
    '                ddProgramValue.DataValueField = dsProgram.Tables(0).Columns("ProgramID").ColumnName
    '                ddProgramValue.DataBind()
    '                ddProgramValue.Items.Insert(0, "")
    '            End If
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    'End Sub

    Protected Sub iBtnCopyDrawingInfo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnCopyDrawingInfo.Click

        Try
            ClearMessages()

            Dim ds As DataSet
            Dim iRowCounter As Integer = 0
            ' ''Dim iSoldTo As Integer = 0
            ' ''Dim strCABBV As String = ""
            Dim iProgramID As Integer = 0
            Dim iYear As Integer = 0

            'existing fields will NOT be overwritten
            If txtNewDrawingNoValue.Text.Trim <> "" Then                
                ds = PEModule.GetDrawing(txtNewDrawingNoValue.Text.Trim)

                If commonFunctions.CheckDataSet(ds) = True Then

                    If ddDesignationTypeValue.SelectedIndex <= 0 Then
                        If ds.Tables(0).Rows(0).Item("DesignationType") IsNot System.DBNull.Value Then
                            ddDesignationTypeValue.SelectedValue = ds.Tables(0).Rows(0).Item("DesignationType").ToString
                        End If
                    End If

                    If ddCommodityValue.SelectedIndex <= 0 Then
                        If ds.Tables(0).Rows(0).Item("CommodityID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("CommodityID") > 0 Then
                                ddCommodityValue.SelectedValue = ds.Tables(0).Rows(0).Item("CommodityID")
                            End If
                        End If
                    End If

                    If ddPurchasedGoodValue.SelectedIndex <= 0 Then
                        If ds.Tables(0).Rows(0).Item("PurchasedGoodID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("PurchasedGoodID") > 0 Then
                                ddPurchasedGoodValue.SelectedValue = ds.Tables(0).Rows(0).Item("PurchasedGoodID")
                            End If
                        End If
                    End If

                    '***********************************
                    'update child tables 
                    '***********************************                   

                    If ViewState("CostSheetID") > 0 Then
                        ds = PEModule.GetDrawingCustomerProgram(txtNewDrawingNoValue.Text.Trim)
                        If commonFunctions.CheckDataSet(ds) = True Then
                            For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                                ' ''iSoldTo = 0
                                ' ''If ds.Tables(0).Rows(iRowCounter).Item("SoldTo") IsNot System.DBNull.Value Then
                                ' ''    If ds.Tables(0).Rows(iRowCounter).Item("SoldTo") > 0 Then
                                ' ''        iSoldTo = ds.Tables(0).Rows(iRowCounter).Item("SoldTo")
                                ' ''    End If
                                ' ''End If

                                iProgramID = 0
                                If ds.Tables(0).Rows(iRowCounter).Item("ProgramID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(iRowCounter).Item("ProgramID") > 0 Then
                                        iProgramID = ds.Tables(0).Rows(iRowCounter).Item("ProgramID")
                                    End If
                                End If

                                iYear = 0
                                If ds.Tables(0).Rows(iRowCounter).Item("ProgramYear") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(iRowCounter).Item("ProgramYear") > 0 Then
                                        iYear = ds.Tables(0).Rows(iRowCounter).Item("ProgramYear")
                                    End If
                                End If

                                CostingModule.InsertCostSheetCustomerProgram(ViewState("CostSheetID"), "", 0, iProgramID, iYear)
                            Next
                        End If

                        gvCustomerProgram.DataBind()
                    End If

                Else
                    lblMessage.Text += "The drawing does not exist.<br>"
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub btnUpdateTotals_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateTotals.Click

        Try

            ClearMessages()

            Dim ds As DataSet

            Dim dTempCapitalTotal As Double = 0

            Dim dMinPriceMargin As Double = 0
            Dim dMinSellingPrice As Double = 0

            Dim dPriceVariableMarginPercent As Double = 0
            Dim dPriceVariableMarginInclDeprPercent As Double = 0
            Dim dPriceVariableMarginDollar As Double = 0
            Dim dPriceVariableMarginInclDeprDollar As Double = 0

            Dim dPriceGrossMarginDollar As Double = 0
            Dim dPriceGrossMarginPercent As Double = 0

            Dim dFixedCostTotal As Double = 0

            Dim dTempLaborTotalWOScrap As Double = 0
            Dim dTempLaborTotal As Double = 0

            Dim dTempMaterialTotalWOScrap As Double = 0
            Dim dTempMaterialTotal As Double = 0

            Dim dTempMiscCostTotal As Double = 0

            Dim dTempOverheadTotalWOScrap As Double = 0
            Dim dTempOverheadTotal As Double = 0

            Dim dTempPackagingTotalWOScrap As Double = 0
            Dim dTempPackagingTotal As Double = 0

            Dim dTempScrapTotal As Double = 0

            Dim dTempSGATotal As Double = 0

            Dim dTempOverheadCostFixedRateTotalWOScrap As Double = 0
            Dim dTempOverheadCostVariableRateTotalWOScrap As Double = 0

            Dim dVariableCostTotal As Double = 0

            Dim dTempCostSheetSubTotalWOScrap As Double = 0
            Dim dTempCostSheetSubTotal As Double = 0

            Dim dTempOverallCostTotalWOScrap As Double = 0
            Dim dTempOverallCostTotal As Double = 0

            Dim strUGNFacility As String = ""

            Dim iMiscCostID As Integer = 0
            Dim iRowCounter As Integer = 0

            Dim dTempOverallTotal As Double = 0
            ds = CostingModule.GetCostSheetCapital(ViewState("CostSheetID"))
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") <> 0 Then
                            dTempCapitalTotal = dTempCapitalTotal + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit")
                        End If
                    End If
                Next
            End If

            ds = CostingModule.GetCostSheetLabor(ViewState("CostSheetID"), 0, False, False)
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap") <> 0 Then
                            dTempLaborTotalWOScrap = dTempLaborTotalWOScrap + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") <> 0 Then
                            dTempLaborTotal = dTempLaborTotal + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit")
                        End If
                    End If

                Next
            End If

            ds = CostingModule.GetCostSheetMaterial(ViewState("CostSheetID"))
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap") <> 0 Then
                            dTempMaterialTotalWOScrap = dTempMaterialTotalWOScrap + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") <> 0 Then
                            dTempMaterialTotal = dTempMaterialTotal + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit")
                        End If
                    End If

                Next
            End If

            iMiscCostID = 0
            ds = CostingModule.GetCostSheetMiscCost(ViewState("CostSheetID"))
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    If ds.Tables(0).Rows(iRowCounter).Item("MiscCostID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("MiscCostID") > 0 Then
                            iMiscCostID = ds.Tables(0).Rows(iRowCounter).Item("MiscCostID")
                        End If
                    End If

                    'split out SGA
                    If iMiscCostID = 1 Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") <> 0 Then
                                dTempSGATotal = ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit")
                            End If
                        End If
                    Else
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") <> 0 Then
                                dTempMiscCostTotal = dTempMiscCostTotal + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit")
                            End If
                        End If
                    End If

                Next
            End If

            ds = CostingModule.GetCostSheetOverhead(ViewState("CostSheetID"), 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrapFixedRate") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrapFixedRate") <> 0 Then
                            dTempOverheadCostFixedRateTotalWOScrap = dTempOverheadCostFixedRateTotalWOScrap + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrapFixedRate")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrapVariableRate") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrapVariableRate") <> 0 Then
                            dTempOverheadCostVariableRateTotalWOScrap = dTempOverheadCostVariableRateTotalWOScrap + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrapVariableRate")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap") <> 0 Then
                            dTempOverheadTotalWOScrap = dTempOverheadTotalWOScrap + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") <> 0 Then
                            dTempOverheadTotal = dTempOverheadTotal + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit")
                        End If
                    End If

                Next
            End If

            ds = CostingModule.GetCostSheetPackaging(ViewState("CostSheetID"))
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap") <> 0 Then
                            dTempPackagingTotalWOScrap = dTempPackagingTotalWOScrap + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnitWOScrap")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit") <> 0 Then
                            dTempPackagingTotal = dTempPackagingTotal + ds.Tables(0).Rows(iRowCounter).Item("StandardCostPerUnit")
                        End If
                    End If

                Next
            End If

            lblCapitalCostTotalWOScrapValue.Text = ""
            txtCapitalCostTotalValue.Text = ""
            lblCapitalCostTotalValue2.Text = ""
            If dTempCapitalTotal > 0 Then
                lblCapitalCostTotalWOScrapValue.Text = Format(dTempCapitalTotal, "###0.0000")
                txtCapitalCostTotalValue.Text = Format(dTempCapitalTotal, "###0.0000")
                lblCapitalCostTotalValue2.Text = Format(dTempCapitalTotal, "###0.0000")
            End If

            txtLaborCostTotalWOScrapValue.Text = ""
            If dTempLaborTotalWOScrap <> 0 Then
                txtLaborCostTotalWOScrapValue.Text = Format(dTempLaborTotalWOScrap, "###0.0000")
            End If

            txtLaborCostTotalValue.Text = ""
            If dTempLaborTotal <> 0 Then
                txtLaborCostTotalValue.Text = Format(dTempLaborTotal, "###0.0000")
            End If

            txtMaterialCostTotalWOScrapValue.Text = ""
            If dTempMaterialTotalWOScrap <> 0 Then
                txtMaterialCostTotalWOScrapValue.Text = Format(dTempMaterialTotalWOScrap, "###0.0000")
            End If

            txtMaterialCostTotalValue.Text = ""
            If dTempMaterialTotal <> 0 Then
                txtMaterialCostTotalValue.Text = Format(dTempMaterialTotal, "###0.0000")
            End If

            txtOverheadCostTotalWOScrapValue.Text = ""
            If dTempOverheadTotalWOScrap <> 0 Then
                txtOverheadCostTotalWOScrapValue.Text = Format(dTempOverheadTotalWOScrap, "###0.0000")
            End If

            txtOverheadCostTotalValue.Text = ""
            If dTempOverheadTotal <> 0 Then
                txtOverheadCostTotalValue.Text = Format(dTempOverheadTotal, "###0.0000")
            End If

            txtPackagingCostTotalWOScrapValue.Text = ""
            If dTempPackagingTotalWOScrap <> 0 Then
                txtPackagingCostTotalWOScrapValue.Text = Format(dTempPackagingTotalWOScrap, "###0.0000")
            End If

            txtPackagingCostTotalValue.Text = ""
            If dTempPackagingTotal <> 0 Then
                txtPackagingCostTotalValue.Text = Format(dTempPackagingTotal, "###0.0000")
            End If

            lblMaterialAndPackagingCostTotalValue.Text = ""
            If dTempMaterialTotal <> 0 Or dTempPackagingTotal <> 0 Then
                lblMaterialAndPackagingCostTotalWOScrapValue.Text = Format(dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap, "###0.0000")
                lblMaterialAndPackagingCostTotalValue.Text = Format(dTempMaterialTotal + dTempPackagingTotal, "###0.0000")
            End If

            dTempScrapTotal = Round(((dTempMaterialTotal + dTempPackagingTotal + dTempLaborTotal + dTempOverheadTotal) - (dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap + dTempLaborTotalWOScrap + dTempOverheadTotalWOScrap)), 4)

            lblScrapCostTotalValue.Text = ""
            If dTempScrapTotal <> 0 Then
                lblScrapCostTotalValue.Text = Format(dTempScrapTotal, "###0.0000")
            End If

            dTempCostSheetSubTotalWOScrap = dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap + dTempLaborTotalWOScrap + dTempOverheadTotalWOScrap + dTempScrapTotal + dTempCapitalTotal
            dTempCostSheetSubTotal = dTempMaterialTotal + dTempPackagingTotal + dTempLaborTotal + dTempOverheadTotal + dTempCapitalTotal

            lblManufacturingCostTotalWOScrapValue.Text = ""
            lblManufacturingCostTotalValue.Text = ""
            If dTempCostSheetSubTotal <> 0 Then
                lblManufacturingCostTotalWOScrapValue.Text = Format(dTempCostSheetSubTotalWOScrap, "###0.0000")
                lblManufacturingCostTotalValue.Text = Format(dTempCostSheetSubTotal, "###0.0000")
            End If

            txtMiscCostTotalValue.Text = ""
            If dTempMiscCostTotal <> 0 Then
                txtMiscCostTotalValue.Text = Format(dTempMiscCostTotal, "###0.0000")
            End If

            lblSGACostTotalValue.Text = ""
            If dTempSGATotal <> 0 Then
                lblSGACostTotalValue.Text = dTempSGATotal
            End If

            dTempOverallCostTotalWOScrap = Format((dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap + dTempLaborTotalWOScrap + dTempOverheadTotalWOScrap + dTempScrapTotal + dTempMiscCostTotal + dTempSGATotal + dTempCapitalTotal), "###0.0000")
            dTempOverallCostTotal = Format((dTempMaterialTotal + dTempPackagingTotal + dTempLaborTotal + dTempOverheadTotal + dTempMiscCostTotal + dTempSGATotal + dTempCapitalTotal), "###0.0000")

            txtOverallCostTotalValue.Text = ""
            If dTempOverallCostTotal <> 0 Then
                txtOverallCostTotalValue.Text = dTempOverallCostTotal
            End If

            dVariableCostTotal = dTempMaterialTotalWOScrap + dTempPackagingTotalWOScrap + dTempLaborTotalWOScrap + dTempOverheadCostVariableRateTotalWOScrap + dTempScrapTotal

            lblVariableCostTotalValue.Text = ""
            If dVariableCostTotal <> 0 Then
                lblVariableCostTotalValue.Text = dVariableCostTotal
            End If

            dFixedCostTotal = dTempOverheadCostFixedRateTotalWOScrap

            lblFixedCostTotalValue.Text = ""
            If dFixedCostTotal <> 0 Then
                lblFixedCostTotalValue.Text = dFixedCostTotal
            End If

            If ddUGNFacilityValue.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacilityValue.SelectedValue
            End If

            ds = CostingModule.GetCostSheetPriceMargin(strUGNFacility)

            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("MinPriceMargin") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MinPriceMargin") <> 0 Then
                        dMinPriceMargin = ds.Tables(0).Rows(0).Item("MinPriceMargin")
                    End If
                End If
            End If

            If (1 - dMinPriceMargin) <> 0 Then
                dMinSellingPrice = Round((dVariableCostTotal + dTempMiscCostTotal + dTempCapitalTotal) / (1 - dMinPriceMargin), 4)
            End If

            lblMinimumSellingPriceValue.Text = ""
            If dMinSellingPrice <> 0 Then
                lblMinimumSellingPriceValue.Text = Format(dMinSellingPrice, "###0.0000")
            End If

            dPriceVariableMarginDollar = dMinSellingPrice - dVariableCostTotal
            dPriceVariableMarginInclDeprDollar = ((dMinSellingPrice - dVariableCostTotal) - dTempCapitalTotal) - dTempMiscCostTotal

            lblPriceVariableMarginDollarValue.Text = ""
            If dPriceVariableMarginDollar <> 0 Then
                lblPriceVariableMarginDollarValue.Text = Format(dPriceVariableMarginDollar, "###0.0000")
            End If

            lblPriceVariableMarginInclDeprDollarValue.Text = ""
            If dPriceVariableMarginInclDeprDollar <> 0 Then
                lblPriceVariableMarginInclDeprDollarValue.Text = Format(dPriceVariableMarginInclDeprDollar, "###0.0000")
            End If

            lblPriceVariableMarginPercentValue.Text = ""
            lblPriceVariableMarginInclDeprPercentValue.Text = ""
            If dMinSellingPrice <> 0 Then
                dPriceVariableMarginPercent = (dPriceVariableMarginDollar / dMinSellingPrice) * 100
                dPriceVariableMarginInclDeprPercent = (dPriceVariableMarginInclDeprDollar / dMinSellingPrice) * 100
                lblPriceVariableMarginPercentValue.Text = Format(dPriceVariableMarginPercent, "###0.0")
                lblPriceVariableMarginInclDeprPercentValue.Text = Format(dPriceVariableMarginInclDeprPercent, "###0.0")
            End If

            lblPriceVariableMarginPercentTargetValue.Text = ""
            If dMinPriceMargin <> 0 Then
                lblPriceVariableMarginPercentTargetValue.Text = Format(dMinPriceMargin * 100, "###0.0")
            End If

            dPriceGrossMarginDollar = dMinSellingPrice - dTempOverallCostTotal

            lblPriceGrossMarginDollarValue.Text = ""
            If dPriceGrossMarginDollar <> 0 Then
                lblPriceGrossMarginDollarValue.Text = Format(dPriceGrossMarginDollar, "###0.0000")
            End If

            lblPriceGrossMarginDollarValue.ForeColor = Color.Black
            If dPriceGrossMarginDollar < 0 Then
                lblPriceGrossMarginDollarValue.ForeColor = Color.Red
            End If

            If dMinSellingPrice <> 0 Then
                dPriceGrossMarginPercent = dPriceGrossMarginDollar / dMinSellingPrice
            End If

            lblPriceGrossMarginPercentValue.Text = ""
            If dPriceGrossMarginPercent <> 0 Then
                lblPriceGrossMarginPercentValue.Text = Format(dPriceGrossMarginPercent * 100, "###0.0")
            End If

            lblPriceGrossMarginPercentValue.ForeColor = Color.Black
            If dPriceGrossMarginPercent < 0 Then
                lblPriceGrossMarginPercentValue.ForeColor = Color.Red
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Private Property LoadDataEmpty_CostSheetDepartment() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CostSheetDepartment") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CostSheetDepartment"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CostSheetDepartment") = value
        End Set

    End Property

    Protected Sub odsCostSheetDepartment_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCostSheetDepartment.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetDepartment_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetDepartment_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CostSheetDepartment = True
            Else
                LoadDataEmpty_CostSheetDepartment = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvDepartment_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDepartment.DataBound

        Try

            'hide header columns
            If gvDepartment.Rows.Count > 0 Then
                gvDepartment.HeaderRow.Cells(0).Visible = False
                gvDepartment.HeaderRow.Cells(1).Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvDepartment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDepartment.RowCommand

        Try

            Dim ddDepartmentIDTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddDepartmentIDTemp = CType(gvDepartment.FooterRow.FindControl("ddFooterDepartment"), DropDownList)

                If ddDepartmentIDTemp.SelectedIndex > 0 Then

                    odsCostSheetDepartment.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                    odsCostSheetDepartment.InsertParameters("DepartmentID").DefaultValue = ddDepartmentIDTemp.SelectedValue

                    intRowsAffected = odsCostSheetDepartment.Insert()
                Else
                    lblMessage.Text += "Error: No Department was selected to insert."
                End If
            End If
            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvDepartment.ShowFooter = False
            Else
                gvDepartment.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddDepartmentIDTemp = CType(gvDepartment.FooterRow.FindControl("ddFooterDepartment"), DropDownList)
                ddDepartmentIDTemp.SelectedIndex = -1
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvDepartment_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDepartment.RowCreated

        Try
            'hide first column
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CostSheetDepartment
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

        lblMessageLowerPage.Text = lblMessage.Text
        lblMessageHeader.Text = lblMessage.Text

    End Sub

    'Private Sub GetProgramInfo(ByVal ProgramID As Integer)

    '    Try

    '        Dim ds As DataSet
    '        Dim strMake As String = ""

    '        If ddMakes.SelectedIndex >= 0 Then
    '            strMake = ddMakes.SelectedValue
    '        End If

    '        ds = commonFunctions.GetPlatformProgram(0, ProgramID, "", "", strMake)
    '        If commonFunctions.CheckDataSet(ds) = True Then
    '            Dim NoOfDays As String = ""
    '            Select Case ds.Tables(0).Rows(0).Item("EOPMM").ToString.Trim
    '                Case "01"
    '                    NoOfDays = "31"
    '                Case "02"
    '                    NoOfDays = "28"
    '                Case "03"
    '                    NoOfDays = "31"
    '                Case "04"
    '                    NoOfDays = "30"
    '                Case "05"
    '                    NoOfDays = "31"
    '                Case "06"
    '                    NoOfDays = "30"
    '                Case "07"
    '                    NoOfDays = "31"
    '                Case "08"
    '                    NoOfDays = "31"
    '                Case "09"
    '                    NoOfDays = "30"
    '                Case 10
    '                    NoOfDays = "31"
    '                Case 11
    '                    NoOfDays = "30"
    '                Case 12
    '                    NoOfDays = "31"
    '            End Select

    '            If ds.Tables(0).Rows(0).Item("EOPMM").ToString.Trim <> "" Then
    '                txtEOPDate.Text = ds.Tables(0).Rows(0).Item("EOPMM").ToString() & "/" & NoOfDays & "/" & ds.Tables(0).Rows(0).Item("EOPYY").ToString()
    '            End If

    '            If ds.Tables(0).Rows(0).Item("SOPMM").ToString.Trim <> "" Then
    '                txtSOPDate.Text = ds.Tables(0).Rows(0).Item("SOPMM").ToString.Trim & "/01/" & ds.Tables(0).Rows(0).Item("SOPYY").ToString.Trim

    '                'pick current year if inside SOP and EOP range 
    '                If ds.Tables(0).Rows(0).Item("SOPYY") < Today.Year And Today.Year <= ds.Tables(0).Rows(0).Item("EOPYY") Then
    '                    ddYear.SelectedValue = Today.Year
    '                Else
    '                    ddYear.SelectedValue = ds.Tables(0).Rows(0).Item("SOPYY").ToString.Trim
    '                End If

    '            End If

    '            '2012-Mar-03 - temporarily disabled - requested by Lynette
    '            'iBtnPreviewDetail.Visible = True
    '            'Dim strPreviewClientScript2 As String = "javascript:void(window.open('../DataMaintenance/ProgramDisplay.aspx?pPlatID=0&pPgmID=" & ProgramID & " '," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=yes,location=yes'));"
    '            'iBtnPreviewDetail.Attributes.Add("Onclick", strPreviewClientScript2)
    '            'Else
    '            '    iBtnPreviewDetail.Visible = False
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    'End Sub
    'Protected Sub ddProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProgram.SelectedIndexChanged

    '    Try
    '        If ddProgram.SelectedIndex >= 0 And ddMakes.SelectedIndex >= 0 Then

    '            GetProgramInfo(ddProgram.SelectedValue)

    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    'End Sub

#Region "Assumptions - Insert Empty GridView Work-Around"
    Protected Sub gvAssumptions_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            Dim Category As TextBox
            Dim Notes As TextBox

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then
                Category = CType(gvAssumptions.FooterRow.FindControl("txtCategory"), TextBox)
                odsAssumptions.InsertParameters("Category").DefaultValue = Category.Text

                Notes = CType(gvAssumptions.FooterRow.FindControl("txtNotes"), TextBox)
                odsAssumptions.InsertParameters("Notes").DefaultValue = Notes.Text

                odsAssumptions.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvAssumptions.ShowFooter = False
            Else
                gvAssumptions.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                Category = CType(gvAssumptions.FooterRow.FindControl("txtCategory"), TextBox)
                Category.Text = Nothing

                Notes = CType(gvAssumptions.FooterRow.FindControl("txtNotes"), TextBox)
                Notes.Text = Nothing

            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF gvAssumptions_RowCommand

    Protected Sub gvAssumptions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssumptions.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(4).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As Costing.Cost_Sheet_AssumptionsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Costing.Cost_Sheet_AssumptionsRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record for " & DataBinder.Eval(e.Row.DataItem, "Category") & "?');")
                End If
            End If
        End If


    End Sub 'EOF gvAssumptions_RowDataBound

    Private Property LoadDataEmpty_Assumptions() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Assumptions") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Assumptions"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Assumptions") = value
        End Set

    End Property 'EOF LoadDataEmpty_Assumptions

    Protected Sub odsAssumptions_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsAssumptions.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.Cost_Sheet_AssumptionsDataTable = CType(e.ReturnValue, Costing.Cost_Sheet_AssumptionsDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Assumptions = True
            Else
                LoadDataEmpty_Assumptions = False
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF odsAssumptions_Selected

    Protected Sub gvAssumptions_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssumptions.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Assumptions
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF gvAssumptions_RowCreated
#End Region ' Insert Empty GridView Work-Around

#Region "Assumptions Approval - Insert Empty GridView Work-Around"
    ' ''Protected Sub gvAssumptionsApproval_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

    ' ''    Try
    ' ''        Dim TeamMemberID As DropDownList
    ' ''        Dim ApprovalDate As TextBox

    ' ''        ''***
    ' ''        ''This section allows the inserting of a new row when called by the OnInserting event call.
    ' ''        ''***
    ' ''        If (e.CommandName = "Insert") Then
    ' ''            TeamMemberID = CType(gvAssumptionsApproval.FooterRow.FindControl("ddTeamMember"), DropDownList)
    ' ''            odsAssumptionsApproval.InsertParameters("TeamMemberID").DefaultValue = TeamMemberID.SelectedValue

    ' ''            ApprovalDate = CType(gvAssumptionsApproval.FooterRow.FindControl("txtApprovalDate"), TextBox)
    ' ''            odsAssumptionsApproval.InsertParameters("ApprovalDate").DefaultValue = ApprovalDate.Text

    ' ''            odsAssumptionsApproval.Insert()
    ' ''        End If

    ' ''        ''***
    ' ''        ''This section allows show/hides the footer row when the Edit control is clicked
    ' ''        ''***
    ' ''        If e.CommandName = "Edit" Then
    ' ''            gvAssumptionsApproval.ShowFooter = False
    ' ''        Else
    ' ''            gvAssumptionsApproval.ShowFooter = True
    ' ''        End If

    ' ''        ''***
    ' ''        ''This section clears out the values in the footer row
    ' ''        ''***
    ' ''        If e.CommandName = "Undo" Then
    ' ''            TeamMemberID = CType(gvAssumptionsApproval.FooterRow.FindControl("txtTeamMember"), DropDownList)
    ' ''            TeamMemberID.SelectedValue = Nothing

    ' ''            ApprovalDate = CType(gvAssumptionsApproval.FooterRow.FindControl("txtApprovalDate"), TextBox)
    ' ''            ApprovalDate.Text = Nothing

    ' ''        End If
    ' ''    Catch ex As Exception

    ' ''        'update error on web page
    ' ''        lblMessage.Text = ex.Message

    ' ''        'get current event name
    ' ''        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    ' ''        'log and email error
    ' ''        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    ' ''    End Try

    ' ''End Sub 'EOF gvAssumptionsApproval_RowCommand

    Protected Sub gvAssumptionsApproval_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssumptionsApproval.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(4).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As Costing.Cost_Sheet_Assumptions_ApprovalRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Costing.Cost_Sheet_Assumptions_ApprovalRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record for " & DataBinder.Eval(e.Row.DataItem, "Department") & "?');")
                End If
            End If
        End If
    End Sub 'EOF gvAssumptionsApproval_RowDataBound

    'Private Property LoadDataEmpty_AssumptionsApproval() As Boolean

    '    ' From Andrew Robinson's Insert Empty GridView solution
    '    ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

    '    ' some controls that are used within a GridView,
    '    ' such as the calendar control, can cuase post backs.
    '    ' we need to preserve LoadDataEmpty across post backs.

    '    Get
    '        If ViewState("LoadDataEmpty_AssumptionsApproval") IsNot Nothing Then
    '            Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_AssumptionsApproval"), Boolean)
    '            Return tmpBoolean
    '        Else
    '            Return False
    '        End If
    '    End Get
    '    Set(ByVal value As Boolean)
    '        ViewState("LoadDataEmpty_AssumptionsApproval") = value
    '    End Set

    'End Property 'EOF LoadDataEmpty_AssumptionsApproval

    'Protected Sub odsAssumptionsApproval_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsAssumptionsApproval.Selected

    '    Try
    '        ' From Andrew Robinson's Insert Empty GridView solution
    '        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

    '        ' bubble exceptions before we touch e.ReturnValue
    '        If e.Exception IsNot Nothing Then
    '            Throw e.Exception
    '        End If

    '        ' get the DataTable from the ODS select method
    '        Console.WriteLine(e.ReturnValue)

    '        Dim dt As Costing.Cost_Sheet_Assumptions_ApprovalDataTable = CType(e.ReturnValue, Costing.Cost_Sheet_Assumptions_ApprovalDataTable)

    '        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
    '        If dt.Rows.Count = 0 Then
    '            dt.Rows.Add(dt.NewRow())
    '            LoadDataEmpty_AssumptionsApproval = True
    '        Else
    '            LoadDataEmpty_AssumptionsApproval = False
    '        End If
    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text = ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub 'EOF odsAssumptions_Selected

    'Protected Sub gvAssumptionsApproval_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssumptions.RowCreated

    '    Try
    '        ' From Andrew Robinson's Insert Empty GridView solution
    '        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
    '        ' when binding a row, look for a zero row condition based on the flag.
    '        ' if we have zero data rows (but a dummy row), hide the grid view row
    '        ' and clear the controls off of that row so they don't cause binding errors

    '        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_AssumptionsApproval
    '        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
    '            e.Row.Visible = False
    '            e.Row.Controls.Clear()
    '        End If
    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text = ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub 'EOF gvAssumptions_RowCreated
#End Region 'Assumptions Approval - Insert Empty GridView Work-Around

    
    
End Class
