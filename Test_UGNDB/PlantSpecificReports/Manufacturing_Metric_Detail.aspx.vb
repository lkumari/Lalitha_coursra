' ************************************************************************************************
'
' Name:		Manufacturing_Metric_Detail.aspx
' Purpose:	This Code Behind is for the detail page of the Manufacturing Metric Module in Plant Specific Reports
'
' Date		    Author	    
' 01/19/2011    Roderick Carlson - Created
' 01/19/2011    Roderick Carlson - Round Actual Shift Count to 2 decimal places
' 01/19/2011    Roderick Carlson - Set Default Available Shift Factor to 6.8 - requested by John Mercado
' 02/08/2011    Roderick Carlson - Make sure Budget Shift Count is also 2 decimal places and refresh all values from  CalculateProductionPerformance before saving
' 03/30/2011    Roderick Carlson - Modified - Added BudgetMachineHourStandard AND ActualMachineHourStandard
' 03/31/2011    Roderick Carlson - Modified - Added BudgetRawWipScrapDollar AND ActualRawWipScrapDollar
' 11/01/2012    Roderick Carlson - Modified - Removed ability to manually reload reports because it is now handled in SQL SSIS Packages
' ************************************************************************************************
Partial Class PlantSpecificReports_Manufacturing_Metric_Detail
    Inherits System.Web.UI.Page

    'Private Sub BPCSRefresh(ByVal DeptID As Integer)

    '    Try

    '        Dim ds As DataSet

    '        Dim dActualDLHours As Double = 0
    '        Dim dActualDowntimeHours As Double = 0
    '        Dim dActualMachineHours As Double = 0
    '        Dim dActualMachineStandardHours As Double = 0
    '        Dim dActualManHours As Double = 0

    '        Dim dAvailablePerShiftFactor As Double = 6.8 '(determined by CFO)

    '        Dim dActualEarnedDLHours As Double = 0

    '        Dim dActualLaborProductivity As Double = 0

    '        Dim dActualOEE As Double = 0
    '        Dim dOEEActualGoodPartCount As Double = 0
    '        Dim dOEEActualTotalPartCount As Double = 0
    '        Dim dOEEActualUtilization As Double = 0
    '        Dim dOEEActualDownHours As Double = 0

    '        Dim dTotalActualMiscScrapDollar As Double = 0
    '        Dim dTotalActualProductionDollar As Double = 0
    '        Dim dTotalActualSpecificScrapDollar As Double = 0

    '        Dim dTotalBudgetRawWipScrapDollar As Double = 0
    '        Dim dTotalActualRawWipScrapDollar As Double = 0

    '        Dim dTotalManHourDowntime As Double = 0

    '        Dim dActualShiftCount As Double = 0
    '        Dim dHoursPerShift As Double = 0
    '        Dim iMTDScrapQTY As Integer = 0
    '        Dim dOEEActualAvailableHours As Double = 0

    '        Dim iMonthlyShippingDays As Integer = 0

    '        cbIncludeDepartment.Checked = False

    '        lblActualDLHoursNetVariance.Text = ""
    '        lblBudgetDLHoursNetVariance.Text = ""

    '        lblActualLaborProductivity.Text = ""
    '        lblBudgetLaborProductivity.Text = ""

    '        lblActualShiftCount.Text = ""
    '        lblBudgetShiftCount.Text = ""

    '        lblAvailablePerShiftFactor.Text = ""

    '        txtActualDLHours.Text = ""
    '        txtBudgetDLHours.Text = ""

    '        txtActualDowntimeHours.Text = ""
    '        txtBudgetDowntimeHours.Text = ""

    '        txtActualDowntimeManHours.Text = ""
    '        txtBudgetDowntimeManHours.Text = ""

    '        txtActualEarnedDLHours.Text = ""
    '        txtBudgetEarnedDLHours.Text = ""

    '        txtActualMachineAvailableHours.Text = ""
    '        txtBudgetMachineAvailableHours.Text = ""

    '        txtActualMachineStandardHours.Text = ""
    '        txtBudgetMachineStandardHours.Text = ""

    '        txtActualMachineWorkedHours.Text = ""
    '        txtBudgetMachineWorkedHours.Text = ""

    '        txtActualManWorkedHours.Text = ""
    '        txtBudgetManWorkedHours.Text = ""

    '        txtActualOEE.Text = ""
    '        txtBudgetOEE.Text = ""

    '        txtActualScrapPercent.Text = ""
    '        txtBudgetScrapPercent.Text = ""

    '        txtBudgetRawWIpScrapPercent.Text = ""
    '        txtActualRawWIpScrapPercent.Text = ""

    '        txtHoursPerShift.Text = ""
    '        txtMonthlyShippingDays.Text = ""

    '        txtOEEActualAvailableHours.Text = ""
    '        txtOEEBudgetAvailableHours.Text = ""

    '        txtOEEActualDownHours.Text = ""
    '        txtOEEBudgetDownHours.Text = ""

    '        txtOEEActualGoodPartCount.Text = ""
    '        txtOEEBudgetGoodPartCount.Text = ""

    '        txtOEEActualScrapPartCount.Text = ""
    '        txtOEEBudgetScrapPartCount.Text = ""

    '        txtOEEActualTotalPartCount.Text = ""
    '        txtOEEBudgetTotalPartCount.Text = ""

    '        txtOEEActualUtilization.Text = ""
    '        txtOEEBudgetUtilization.Text = ""

    '        txtTotalActualProductionDollar.Text = ""
    '        txtTotalBudgetProductionDollar.Text = ""

    '        txtTotalActualSpecificScrapDollar.Text = ""
    '        txtTotalBudgetSpecificScrapDollar.Text = ""

    '        txtTotalActualMiscScrapDollar.Text = ""
    '        txtTotalBudgetMiscScrapDollar.Text = ""

    '        txtTotalBudgetRawWipScrapDollar.Text = ""
    '        txtTotalActualRawWipScrapDollar.Text = ""

    '        If DeptID > 0 And ViewState("UGNFacility") <> "" And ViewState("StartDate") <> "" And ViewState("EndDate") <> "" Then

    '            'if total production dollar is 0, then skip the rest of the calcs
    '            txtTotalActualProductionDollar.Text = ""
    '            ds = PSRModule.GetManufacturingMetricProductionDollarByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))
    '            If commonFunctions.CheckDataSet(ds) Then
    '                If ds.Tables(0).Rows(0).Item("TotalProductionDollar") IsNot System.DBNull.Value Then
    '                    If ds.Tables(0).Rows(0).Item("TotalProductionDollar") <> 0 Then
    '                        dTotalActualProductionDollar = ds.Tables(0).Rows(0).Item("TotalProductionDollar")
    '                        txtTotalActualProductionDollar.Text = Format(dTotalActualProductionDollar, "####0.00")
    '                    End If
    '                End If
    '            End If

    '            If dTotalActualProductionDollar <> 0 Then

    '                ds = PSRModule.GetManufacturingMetricAvailablePerShiftFactorByDept(ViewState("UGNFacility"), DeptID)
    '                If commonFunctions.CheckDataSet(ds) = True Then
    '                    If ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor") > 0 Then
    '                            dAvailablePerShiftFactor = ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor")
    '                        End If
    '                    End If
    '                End If

    '                ds = PSRModule.GetManufacturingMetricScrapQuantityByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))

    '                txtOEEActualScrapPartCount.Text = ""
    '                If commonFunctions.CheckDataSet(ds) = True Then
    '                    If ds.Tables(0).Rows(0).Item("MTDScrapQTY") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("MTDScrapQTY") <> 0 Then
    '                            iMTDScrapQTY = ds.Tables(0).Rows(0).Item("MTDScrapQTY")
    '                            txtOEEActualScrapPartCount.Text = Format(iMTDScrapQTY * -1, "#####0")
    '                        End If
    '                    End If
    '                End If 'if ds-GetManufacturingMetricScrapQuantity has values

    '                ds = PSRModule.GetManufacturingMetricProductionQuantityByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))

    '                txtOEEActualTotalPartCount.Text = ""
    '                If commonFunctions.CheckDataSet(ds) = True Then
    '                    If ds.Tables(0).Rows(0).Item("MTDProductionQTY") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("MTDProductionQTY") <> 0 Then
    '                            dOEEActualTotalPartCount = ds.Tables(0).Rows(0).Item("MTDProductionQTY")
    '                            txtOEEActualTotalPartCount.Text = Format(dOEEActualTotalPartCount, "#####0")
    '                        End If
    '                    End If

    '                End If 'if ds-GetManufacturingMetricProductionQuantity has values

    '                txtOEEActualGoodPartCount.Text = ""
    '                dOEEActualGoodPartCount = dOEEActualTotalPartCount + iMTDScrapQTY
    '                If dOEEActualGoodPartCount <> 0 Then
    '                    txtOEEActualGoodPartCount.Text = Format(dOEEActualGoodPartCount, "#####0")
    '                End If

    '                ds = PSRModule.GetManufacturingMetricActualMachineHoursByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))

    '                If commonFunctions.CheckDataSet(ds) = True Then

    '                    If ds.Tables(0).Rows(0).Item("ActualMachineHours") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("ActualMachineHours") <> 0 Then
    '                            dActualMachineHours = ds.Tables(0).Rows(0).Item("ActualMachineHours")
    '                        End If
    '                    End If
    '                End If 'if ds-GetManufacturingMetricActualMachineHours has values

    '                ds = PSRModule.GetManufacturingMetricActualDowntimeHoursByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))

    '                txtActualDowntimeHours.Text = ""
    '                If commonFunctions.CheckDataSet(ds) = True Then
    '                    If ds.Tables(0).Rows(0).Item("ActualDowntimeHours") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("ActualDowntimeHours") <> 0 Then
    '                            dActualDowntimeHours = ds.Tables(0).Rows(0).Item("ActualDowntimeHours")
    '                            txtActualDowntimeHours.Text = Format(dActualDowntimeHours, "####0.00")
    '                        End If
    '                    End If
    '                End If 'if ds-GetManufacturingMetricActualDowntimeHours has values

    '                txtMonthlyShippingDays.Text = ""
    '                txtHoursPerShift.Text = ""
    '                If ddMonth.SelectedIndex > 0 Then
    '                    ds = PSRModule.GetUGNMonthlyShippingDays(ddMonth.SelectedValue)

    '                    If commonFunctions.CheckDataSet(ds) = True Then

    '                        If ds.Tables(0).Rows(0).Item("ShippingDays") IsNot System.DBNull.Value Then
    '                            If ds.Tables(0).Rows(0).Item("ShippingDays") <> 0 Then
    '                                iMonthlyShippingDays = ds.Tables(0).Rows(0).Item("ShippingDays")
    '                                txtMonthlyShippingDays.Text = iMonthlyShippingDays
    '                                dHoursPerShift = iMonthlyShippingDays * 8
    '                                txtHoursPerShift.Text = Format(dHoursPerShift, "0#")
    '                            End If
    '                        End If
    '                    End If 'if ds-GetUGNMonthlyShippingDays has values
    '                End If

    '                If dHoursPerShift <> 0 Then                       
    '                    dActualShiftCount = Round(((dActualMachineHours + dActualDowntimeHours) / dHoursPerShift), 2)
    '                End If

    '                lblActualShiftCount.Text = ""
    '                If dActualShiftCount <> 0 Then
    '                    lblActualShiftCount.Text = Format(dActualShiftCount, "#0.##")
    '                End If

    '                txtActualMachineWorkedHours.Text = ""
    '                If dActualMachineHours <> 0 Then
    '                    txtActualMachineWorkedHours.Text = Format(dActualMachineHours, "#0.00")
    '                End If

    '                lblAvailablePerShiftFactor.Text = ""
    '                If dAvailablePerShiftFactor <> 0 Then
    '                    lblAvailablePerShiftFactor.Text = Format(dAvailablePerShiftFactor, "####0.0")
    '                End If

    '                dOEEActualAvailableHours = CType(dActualShiftCount * iMonthlyShippingDays * dAvailablePerShiftFactor, Integer)

    '                txtOEEActualAvailableHours.Text = ""
    '                txtActualMachineAvailableHours.Text = ""
    '                If dOEEActualAvailableHours <> 0 Then
    '                    txtOEEActualAvailableHours.Text = dOEEActualAvailableHours
    '                    txtActualMachineAvailableHours.Text = dOEEActualAvailableHours
    '                End If

    '                txtOEEActualUtilization.Text = ""
    '                If dOEEActualAvailableHours <> 0 Then
    '                    dOEEActualUtilization = dActualMachineHours / dOEEActualAvailableHours
    '                End If

    '                If dOEEActualUtilization <> 0 Then
    '                    txtOEEActualUtilization.Text = Format(dOEEActualUtilization * 100, "##0.0")
    '                    lblActualMachineUtilization.Text = Format(dOEEActualUtilization * 100, "##0.0")
    '                End If

    '                ds = PSRModule.GetManufacturingMetricMachineHourDowntimeAllShiftTotalByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"), False)
    '                If commonFunctions.CheckDataSet(ds) = True Then
    '                    If ds.Tables(0).Rows(0).Item("TotalMachineHoursDowntime") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("TotalMachineHoursDowntime") <> 0 Then
    '                            dOEEActualDownHours = ds.Tables(0).Rows(0).Item("TotalMachineHoursDowntime")
    '                            txtOEEActualDownHours.Text = Format(dOEEActualDownHours, "###0.00")
    '                        End If
    '                    End If
    '                End If 'if ds-GetManufacturingMetricMachineHourDowntimeAllShiftTotal is not empty

    '                dOEEActualUtilization = Round(dOEEActualUtilization, 2)

    '                If dOEEActualGoodPartCount <> 0 And dOEEActualAvailableHours <> 0 And dActualMachineHours <> 0 Then
    '                    'dActualOEE = (((dOEEActualGoodPartCount / dOEEActualTotalPartCount) * dOEEActualUtilization)) * ((dOEEActualAvailableHours - dOEEActualDownHours) / dOEEActualAvailableHours)

    '                    '03/30/2011
    '                    ds = PSRModule.GetManufacturingMetricMachineHourStandardByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))

    '                    txtActualMachineStandardHours.Text = ""
    '                    If commonFunctions.CheckDataSet(ds) = True Then
    '                        If ds.Tables(0).Rows(0).Item("MachineHourStandard") IsNot System.DBNull.Value Then
    '                            If ds.Tables(0).Rows(0).Item("MachineHourStandard") <> 0 Then
    '                                dActualMachineStandardHours = ds.Tables(0).Rows(0).Item("MachineHourStandard")
    '                                txtActualMachineStandardHours.Text = Format(dActualMachineStandardHours, "###0.00")
    '                            End If
    '                        End If
    '                    End If
    '                    dActualOEE = (dOEEActualGoodPartCount / dOEEActualTotalPartCount) * dOEEActualUtilization * (dActualMachineStandardHours / dActualMachineHours)

    '                End If

    '                If dActualOEE <> 0 Then
    '                    txtActualOEE.Text = Format(dActualOEE * 100, "##0.0")
    '                End If

    '                txtActualEarnedDLHours.Text = ""
    '                ds = PSRModule.GetManufacturingMetricStandardManHoursByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))
    '                If commonFunctions.CheckDataSet(ds) Then
    '                    If ds.Tables(0).Rows(0).Item("TotalStandardManHours") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("TotalStandardManHours") <> 0 Then
    '                            dActualEarnedDLHours = ds.Tables(0).Rows(0).Item("TotalStandardManHours")
    '                            txtActualEarnedDLHours.Text = Format(dActualEarnedDLHours, "###0")
    '                        End If
    '                    End If
    '                End If

    '                ds = PSRModule.GetManufacturingMetricActualManHoursByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))
    '                If commonFunctions.CheckDataSet(ds) Then
    '                    If ds.Tables(0).Rows(0).Item("ActualManHours") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("ActualManHours") <> 0 Then
    '                            dActualManHours = ds.Tables(0).Rows(0).Item("ActualManHours")
    '                        End If
    '                    End If
    '                End If

    '                ds = PSRModule.GetManufacturingMetricManHourDowntimeAllShiftAllScheduleTotalByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))
    '                If commonFunctions.CheckDataSet(ds) Then
    '                    If ds.Tables(0).Rows(0).Item("TotalManHourDowntime") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("TotalManHourDowntime") <> 0 Then
    '                            dTotalManHourDowntime = ds.Tables(0).Rows(0).Item("TotalManHourDowntime")
    '                        End If
    '                    End If
    '                End If

    '                dActualDLHours = dActualManHours + dTotalManHourDowntime

    '                txtActualManWorkedHours.Text = ""
    '                If dActualManHours <> 0 Then
    '                    txtActualManWorkedHours.Text = Format(dActualManHours, "###0")
    '                End If

    '                txtActualDowntimeManHours.Text = ""
    '                If dTotalManHourDowntime <> 0 Then
    '                    txtActualDowntimeManHours.Text = Format(dTotalManHourDowntime, "###0")
    '                End If

    '                txtActualDLHours.Text = ""
    '                lblActualDLHoursNetVariance.Text = ""
    '                lblActualLaborProductivity.Text = ""

    '                If dActualDLHours <> 0 Then
    '                    dActualLaborProductivity = dActualEarnedDLHours / dActualDLHours

    '                    txtActualDLHours.Text = Format(dActualDLHours, "###0")
    '                    lblActualDLHoursNetVariance.Text = Format(dActualEarnedDLHours - dActualDLHours, "###0")
    '                    lblActualLaborProductivity.Text = Format(dActualLaborProductivity * 100, "###0.0")
    '                End If

    '                txtTotalActualSpecificScrapDollar.Text = ""
    '                ds = PSRModule.GetManufacturingMetricScrapDollarByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))
    '                If commonFunctions.CheckDataSet(ds) Then
    '                    If ds.Tables(0).Rows(0).Item("TotalScrapDollar") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("TotalScrapDollar") <> 0 Then
    '                            dTotalActualSpecificScrapDollar = ds.Tables(0).Rows(0).Item("TotalScrapDollar") * -1
    '                            txtTotalActualSpecificScrapDollar.Text = Format(dTotalActualSpecificScrapDollar, "####0.00")
    '                        End If
    '                    End If
    '                End If

    '                txtTotalActualMiscScrapDollar.Text = ""
    '                ds = PSRModule.GetManufacturingMetricMiscScrapDollarByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))
    '                If commonFunctions.CheckDataSet(ds) Then
    '                    If ds.Tables(0).Rows(0).Item("TotalScrapDollar") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("TotalScrapDollar") <> 0 Then
    '                            dTotalActualMiscScrapDollar = ds.Tables(0).Rows(0).Item("TotalScrapDollar") * -1
    '                            txtTotalActualMiscScrapDollar.Text = Format(dTotalActualMiscScrapDollar, "####0.00")
    '                        End If
    '                    End If
    '                End If

    '                txtTotalActualRawWipScrapDollar.Text = ""
    '                ds = PSRModule.GetManufacturingMetricRawWIPScrapDollarByDept(DeptID, ViewState("UGNFacility"), ViewState("StartDate"), ViewState("EndDate"))
    '                If commonFunctions.CheckDataSet(ds) Then
    '                    If ds.Tables(0).Rows(0).Item("RawWipScrapDollar") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(0).Item("RawWipScrapDollar") <> 0 Then
    '                            dTotalActualRawWipScrapDollar = ds.Tables(0).Rows(0).Item("RawWipScrapDollar") * -1
    '                            txtTotalActualRawWipScrapDollar.Text = Format(dTotalActualRawWipScrapDollar, "####0.00")
    '                        End If
    '                    End If
    '                End If

    '                txtActualScrapPercent.Text = Format(((dTotalActualSpecificScrapDollar + dTotalActualMiscScrapDollar) / dTotalActualProductionDollar) * 100, "###0.0")
    '                txtActualRawWIpScrapPercent.Text = Format(((dTotalActualRawWipScrapDollar) / dTotalActualProductionDollar) * 100, "###0.0")
    '                cbIncludeDepartment.Checked = True
    '            Else
    '                cbIncludeDepartment.Checked = False
    '            End If

    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Private Sub BindDetailData(ByVal DeptID As Integer)

        'Dim strDebugInfo As String = "Section: Start BindDetailData"

        Try

            'if department = 0, then get totals

            Dim ds As DataSet

            Dim dAvailablePerShiftFactor As Double = 0

            Dim dBudgetDLHours As Double = 0
            Dim dActualDLHours As Double = 0

            Dim dBudgetEarnedDLHours As Double = 0
            Dim dActualEarnedDLHours As Double = 0

            Dim dBudgetMachineWorkedHours As Double = 0
            Dim dActualMachineWorkedHours As Double = 0

            Dim dBudgetMachineStandardHours As Double = 0
            Dim dActualMachineStandardHours As Double = 0

            Dim dBudgetLaborProductivity As Double = 0
            Dim dActualLaborProductivity As Double = 0

            Dim dTotalBudgetMiscScrapDollar As Double = 0
            Dim dTotalActualMiscScrapDollar As Double = 0
            Dim dTotalActualIndirectScrapDollar As Double = 0

            Dim dTotalBudgetRawWipScrapDollar As Double = 0
            Dim dTotalActualRawWipScrapDollar As Double = 0

            Dim dBudgetTeamMemberFactorCount As Integer = 0
            Dim dBudgetTeamLeaderFactorCount As Integer = 0

            Dim dActualTeamMemberFactorCount As Integer = 0
            Dim dActualTeamLeaderFactorCount As Integer = 0

            Dim dBudgetDowntimeHours As Double = 0
            Dim dBudgetDowntimeManHours As Double = 0
            Dim dBudgetManWorkedHours As Double = 0
            Dim dHoursPerShift As Double = 0
            Dim dBudgetShiftCount As Double = 0

            Dim dBudgetOEE As Double = 0
            Dim dActualOEE As Double = 0

            Dim dOEEBudgetDownHours As Double = 0
            Dim dOEEActualDownHours As Double = 0

            Dim dOEEBudgetGoodPartCount As Double = 0
            Dim dOEEActualGoodPartCount As Double = 0

            Dim dOEEBudgetTotalPartCount As Double = 0
            Dim dOEEActualTotalPartCount As Double = 0

            Dim dOEEBudgetUtilization As Double = 0
            Dim dOEEActualUtilization As Double = 0

            Dim dTotalBudgetSpecificScrapDollar As Double = 0
            Dim dTotalActualSpecificScrapDollar As Double = 0

            Dim dTotalBudgetProductionDollar As Double = 0
            Dim dTotalActualProductionDollar As Double = 0

            Dim dOEEBudgetAvailableHours As Double = 0
            Dim dOEEActualAvailableHours As Double = 0

            Dim iMonthlyShippingDays As Integer = 0

            Dim dBudgetMachineUtilization As Double = 0
            Dim dActualMachineUtilization As Double = 0

            Dim dBudgetScrap As Double = 0
            Dim dActualScrap As Double = 0

            Dim dBudgetRawWipScrap As Double = 0
            Dim dActualRawWipScrap As Double = 0

            Dim dBudgetCapacityUtilization As Double = 0
            Dim dActualCapacityUtilization As Double = 0

            Dim dBudgetAllocatedSupportOTHours As Double = 0
            Dim dActualAllocatedSupportOTHours As Double = 0
            Dim dBudgetAllocatedSupportTeamMemberContainmentCount As Double = 0
            Dim dActualAllocatedSupportTeamMemberContainmentCount As Double = 0
            Dim dBudgetAllocatedSupportPartContainmentCount As Double = 0
            Dim dActualAllocatedSupportPartContainmentCount As Double = 0
            Dim dBudgetAllocatedSupportOffStandardIndirectCount As Double = 0
            Dim dActualAllocatedSupportOffStandardIndirectCount As Double = 0
            Dim dBudgetAllocatedSupportIndirectPerm As Double = 0
            Dim dFlexAllocatedSupportIndirectPerm As Double = 0
            Dim dActualAllocatedSupportIndirectPerm As Double = 0
            Dim dBudgetAllocatedSupportIndirectTemp As Double = 0
            Dim dFlexAllocatedSupportIndirectTemp As Double = 0
            Dim dActualAllocatedSupportIndirectTemp As Double = 0
            Dim dBudgetAllocatedSupportOfficeHourlyPerm As Double = 0
            Dim dFlexAllocatedSupportOfficeHourlyPerm As Double = 0
            Dim dActualAllocatedSupportOfficeHourlyPerm As Double = 0
            Dim dBudgetAllocatedSupportOfficeHourlyTemp As Double = 0
            Dim dFlexAllocatedSupportOfficeHourlyTemp As Double = 0
            Dim dActualAllocatedSupportOfficeHourlyTemp As Double = 0
            Dim dBudgetAllocatedSupportSalaryPerm As Double = 0
            Dim dFlexAllocatedSupportSalaryPerm As Double = 0
            Dim dActualAllocatedSupportSalaryPerm As Double = 0
            Dim dBudgetAllocatedSupportSalaryTemp As Double = 0
            Dim dFlexAllocatedSupportSalaryTemp As Double = 0
            Dim dActualAllocatedSupportSalaryTemp As Double = 0

            txtNotes.Text = ""

            txtBudgetOEE.Text = ""
            txtActualOEE.Text = ""

            txtBudgetEarnedDLHours.Text = ""
            txtActualEarnedDLHours.Text = ""

            txtBudgetDLHours.Text = ""
            txtActualDLHours.Text = ""

            txtBudgetDirectOTHours.Text = ""
            txtActualDirectOTHours.Text = ""

            txtBudgetIndirectOTHours.Text = ""
            txtActualIndirectOTHours.Text = ""

            txtBudgetAllocatedSupportOTHours.Text = ""
            txtActualAllocatedSupportOTHours.Text = ""

            txtBudgetScrapPercent.Text = ""
            txtActualScrapPercent.Text = ""

            txtBudgetRawWipScrapPercent.Text = ""
            txtActualRawWipScrapPercent.Text = ""

            txtBudgetTeamMemberContainmentCount.Text = ""
            txtActualTeamMemberContainmentCount.Text = ""

            txtBudgetAllocatedSupportTeamMemberContainmentCount.Text = ""
            txtActualAllocatedSupportTeamMemberContainmentCount.Text = ""

            txtBudgetPartContainmentCount.Text = ""
            txtActualPartContainmentCount.Text = ""

            txtBudgetAllocatedSupportPartContainmentCount.Text = ""
            txtActualAllocatedSupportPartContainmentCount.Text = ""

            txtBudgetOffStandardDirectCount.Text = ""
            txtActualOffStandardDirectCount.Text = ""

            txtBudgetOffStandardIndirectCount.Text = ""
            txtActualOffStandardIndirectCount.Text = ""

            txtBudgetAllocatedSupportOffStandardIndirectCount.Text = ""
            txtActualAllocatedSupportOffStandardIndirectCount.Text = ""

            cbBudgetStandardizedCellWork.Checked = False
            cbActualStandardizedCellWork.Checked = False

            txtBudgetTeamMemberFactorCount.Text = ""
            txtBudgetTeamLeaderFactorCount.Text = ""
            lblBudgetTeamMemberLeaderRatio.Text = ""

            txtActualTeamMemberFactorCount.Text = ""
            txtActualTeamLeaderFactorCount.Text = ""
            lblActualTeamMemberLeaderRatio.Text = ""

            txtBudgetCapacityUtilization.Text = ""
            txtActualCapacityUtilization.Text = ""

            txtOEEBudgetGoodPartCount.Text = ""
            txtOEEActualGoodPartCount.Text = ""

            txtOEEBudgetScrapPartCount.Text = ""
            txtOEEActualScrapPartCount.Text = ""

            txtOEEBudgetTotalPartCount.Text = ""
            txtOEEActualTotalPartCount.Text = ""

            txtOEEBudgetUtilization.Text = ""
            txtOEEActualUtilization.Text = ""

            txtOEEBudgetAvailableHours.Text = ""
            txtOEEActualAvailableHours.Text = ""

            txtOEEBudgetDownHours.Text = ""
            txtOEEActualDownHours.Text = ""

            txtMonthlyShippingDays.Text = ""

            lblAvailablePerShiftFactor.Text = ""

            lblBudgetDLHoursNetVariance.Text = ""
            lblActualDLHoursNetVariance.Text = ""

            lblBudgetLaborProductivity.Text = ""
            lblActualLaborProductivity.Text = ""

            lblBudgetMachineUtilization.Text = ""
            lblActualMachineUtilization.Text = ""

            lblBudgetShiftCount.Text = ""
            lblActualShiftCount.Text = ""

            txtBudgetDowntimeHours.Text = ""
            txtActualDowntimeHours.Text = ""

            txtBudgetMachineWorkedHours.Text = ""
            txtActualMachineWorkedHours.Text = ""

            txtBudgetMachineAvailableHours.Text = ""
            txtActualMachineAvailableHours.Text = ""

            txtBudgetMachineStandardHours.Text = ""
            txtActualMachineStandardHours.Text = ""

            txtBudgetManWorkedHours.Text = ""
            txtActualManWorkedHours.Text = ""

            txtBudgetDowntimeManHours.Text = ""
            txtActualDowntimeManHours.Text = ""

            txtTotalBudgetProductionDollar.Text = ""
            txtTotalActualProductionDollar.Text = ""

            txtTotalBudgetSpecificScrapDollar.Text = ""
            txtTotalActualSpecificScrapDollar.Text = ""

            txtTotalBudgetMiscScrapDollar.Text = ""
            txtTotalActualMiscScrapDollar.Text = ""

            txtTotalActualIndirectScrapDollar.Text = ""

            txtTotalBudgetRawWipScrapDollar.Text = ""
            txtTotalActualRawWipScrapDollar.Text = ""

            txtBudgetDirectPerm.Text = ""
            txtFlexDirectPerm.Text = ""
            txtActualDirectPerm.Text = ""

            txtBudgetDirectTemp.Text = ""
            txtFlexDirectTemp.Text = ""
            txtActualDirectTemp.Text = ""

            txtBudgetIndirectPerm.Text = ""
            txtFlexIndirectPerm.Text = ""
            txtActualIndirectPerm.Text = ""

            txtBudgetAllocatedSupportIndirectPerm.Text = ""
            txtFlexAllocatedSupportIndirectPerm.Text = ""
            txtActualAllocatedSupportIndirectPerm.Text = ""

            txtBudgetIndirectTemp.Text = ""
            txtFlexIndirectTemp.Text = ""
            txtActualIndirectTemp.Text = ""

            txtBudgetAllocatedSupportIndirectTemp.Text = ""
            txtFlexAllocatedSupportIndirectTemp.Text = ""
            txtActualAllocatedSupportIndirectTemp.Text = ""

            txtBudgetOfficeHourlyPerm.Text = ""
            txtFlexOfficeHourlyPerm.Text = ""
            txtActualOfficeHourlyPerm.Text = ""

            txtBudgetAllocatedSupportOfficeHourlyPerm.Text = ""
            txtFlexAllocatedSupportOfficeHourlyPerm.Text = ""
            txtActualAllocatedSupportOfficeHourlyPerm.Text = ""

            txtBudgetOfficeHourlyTemp.Text = ""
            txtFlexOfficeHourlyTemp.Text = ""
            txtActualOfficeHourlyTemp.Text = ""

            txtBudgetAllocatedSupportOfficeHourlyTemp.Text = ""
            txtFlexAllocatedSupportOfficeHourlyTemp.Text = ""
            txtActualAllocatedSupportOfficeHourlyTemp.Text = ""

            txtBudgetSalaryPerm.Text = ""
            txtFlexSalaryPerm.Text = ""
            txtActualSalaryPerm.Text = ""

            txtBudgetAllocatedSupportSalaryPerm.Text = ""
            txtFlexAllocatedSupportSalaryPerm.Text = ""
            txtActualAllocatedSupportSalaryPerm.Text = ""

            txtBudgetSalaryTemp.Text = ""
            txtFlexSalaryTemp.Text = ""
            txtActualSalaryTemp.Text = ""

            txtBudgetAllocatedSupportSalaryTemp.Text = ""
            txtFlexAllocatedSupportSalaryTemp.Text = ""
            txtActualAllocatedSupportSalaryTemp.Text = ""

            lblUpdatedOn.Text = ""

            If DeptID > 0 Then
                'get specific department               
                ds = PSRModule.GetManufacturingMetricDetailByDept(ViewState("ReportID"), DeptID)

                lblNotes.Text = "Department Notes:"
            Else
                'get totals for all departments
                ds = PSRModule.GetManufacturingMetricDetailTotalByDept(ViewState("ReportID"))
                cbIncludeDepartment.Checked = True

                lblNotes.Text = "Total Department(s) Notes:"
            End If

            'check for existing data first
            If commonFunctions.CheckDataSet(ds) = True Then

                'strDebugInfo = "Section: Obsolete"

                If ds.Tables(0).Rows(0).Item("Obsolete") IsNot System.DBNull.Value And DeptID > 0 Then
                    cbIncludeDepartment.Checked = Not ds.Tables(0).Rows(0).Item("Obsolete")
                End If

                'strDebugInfo = "Section: Notes"

                txtNotes.Text = ds.Tables(0).Rows(0).Item("Notes").ToString

                'strDebugInfo = "Section: ActualOEE"

                If ds.Tables(0).Rows(0).Item("ActualOEE") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualOEE") <> 0 Then
                        txtActualOEE.Text = Format(ds.Tables(0).Rows(0).Item("ActualOEE"), "#0.0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetEarnedDLHours"

                If ds.Tables(0).Rows(0).Item("BudgetEarnedDLHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetEarnedDLHours") <> 0 Then
                        txtBudgetEarnedDLHours.Text = Format(ds.Tables(0).Rows(0).Item("BudgetEarnedDLHours"), "#0")
                        dBudgetEarnedDLHours = ds.Tables(0).Rows(0).Item("BudgetEarnedDLHours")
                    End If
                End If

                'strDebugInfo = "Section: ActualEarnedDLHours"

                If ds.Tables(0).Rows(0).Item("ActualEarnedDLHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualEarnedDLHours") <> 0 Then
                        txtActualEarnedDLHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualEarnedDLHours"), "#0")
                        dActualEarnedDLHours = ds.Tables(0).Rows(0).Item("ActualEarnedDLHours")
                    End If
                End If

                'strDebugInfo = "Section: BudgetDLHours"

                If ds.Tables(0).Rows(0).Item("BudgetDLHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetDLHours") <> 0 Then
                        txtBudgetDLHours.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDLHours"), "#0")
                        dBudgetDLHours = ds.Tables(0).Rows(0).Item("BudgetDLHours")
                    End If
                End If

                'strDebugInfo = "Section: ActualDLHours"

                If ds.Tables(0).Rows(0).Item("ActualDLHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualDLHours") <> 0 Then
                        txtActualDLHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualDLHours"), "#0")
                        dActualDLHours = ds.Tables(0).Rows(0).Item("ActualDLHours")
                    End If
                End If

                If dActualDLHours <> 0 Then
                    dActualLaborProductivity = dActualEarnedDLHours / dActualDLHours

                    lblActualDLHoursNetVariance.Text = Format(dActualEarnedDLHours - dActualDLHours, "#,###0")
                    lblActualLaborProductivity.Text = Format(dActualLaborProductivity * 100, "###0.0")
                End If

                'strDebugInfo = "Section: BudgetDirectOTHours"

                If ds.Tables(0).Rows(0).Item("BudgetDirectOTHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetDirectOTHours") <> 0 Then
                        txtBudgetDirectOTHours.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDirectOTHours"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualDirectOTHours"

                If ds.Tables(0).Rows(0).Item("ActualDirectOTHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualDirectOTHours") <> 0 Then
                        txtActualDirectOTHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualDirectOTHours"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetIndirectOTHours"

                If ds.Tables(0).Rows(0).Item("BudgetIndirectOTHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetIndirectOTHours") <> 0 Then
                        txtBudgetIndirectOTHours.Text = Format(ds.Tables(0).Rows(0).Item("BudgetIndirectOTHours"), "#0.00")
                    End If
                End If

                'strDebugInfo = "Section: ActualIndirectOTHours"

                If ds.Tables(0).Rows(0).Item("ActualIndirectOTHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualIndirectOTHours") <> 0 Then
                        txtActualIndirectOTHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualIndirectOTHours"), "#0.00")
                    End If
                End If

                'strDebugInfo = "Section: BudgetAllocatedSupportOTHours"

                If DeptID = 0 Then
                    If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOTHours") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOTHours") <> 0 Then
                            txtBudgetAllocatedSupportOTHours.Text = Format(ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOTHours"), "#0.##")
                            dBudgetAllocatedSupportOTHours = ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOTHours")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualAllocatedSupportOTHours"

                    If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOTHours") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOTHours") <> 0 Then
                            txtActualAllocatedSupportOTHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOTHours"), "#0.##")
                            dActualAllocatedSupportOTHours = ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOTHours")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: ActualScrap"

                If ds.Tables(0).Rows(0).Item("ActualScrap") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualScrap") <> 0 Then
                        txtActualScrapPercent.Text = Format((ds.Tables(0).Rows(0).Item("ActualScrap")), "#0.0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetTeamMemberContainmentCount"

                If ds.Tables(0).Rows(0).Item("BudgetTeamMemberContainmentCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetTeamMemberContainmentCount") <> 0 Then
                        txtBudgetTeamMemberContainmentCount.Text = Format(ds.Tables(0).Rows(0).Item("BudgetTeamMemberContainmentCount"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualTeamMemberContainmentCount"

                If ds.Tables(0).Rows(0).Item("ActualTeamMemberContainmentCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualTeamMemberContainmentCount") <> 0 Then
                        txtActualTeamMemberContainmentCount.Text = Format(ds.Tables(0).Rows(0).Item("ActualTeamMemberContainmentCount"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetAllocatedSupportTeamMemberContainmentCount"

                If DeptID = 0 Then
                    If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportTeamMemberContainmentCount") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportTeamMemberContainmentCount") <> 0 Then
                            txtBudgetAllocatedSupportTeamMemberContainmentCount.Text = Format(ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportTeamMemberContainmentCount"), "#0")
                            dBudgetAllocatedSupportTeamMemberContainmentCount = ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportTeamMemberContainmentCount")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualAllocatedSupportTeamMemberContainmentCount"

                    If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportTeamMemberContainmentCount") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportTeamMemberContainmentCount") <> 0 Then
                            txtActualAllocatedSupportTeamMemberContainmentCount.Text = Format(ds.Tables(0).Rows(0).Item("ActualAllocatedSupportTeamMemberContainmentCount"), "#0")
                            dActualAllocatedSupportTeamMemberContainmentCount = ds.Tables(0).Rows(0).Item("ActualAllocatedSupportTeamMemberContainmentCount")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: BudgetPartContainmentCount"

                If ds.Tables(0).Rows(0).Item("BudgetPartContainmentCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetPartContainmentCount") <> 0 Then
                        txtBudgetPartContainmentCount.Text = Format(ds.Tables(0).Rows(0).Item("BudgetPartContainmentCount"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualPartContainmentCount"

                If ds.Tables(0).Rows(0).Item("ActualPartContainmentCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualPartContainmentCount") <> 0 Then
                        txtActualPartContainmentCount.Text = Format(ds.Tables(0).Rows(0).Item("ActualPartContainmentCount"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetAllocatedSupportPartContainmentCount"

                If DeptID = 0 Then
                    If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportPartContainmentCount") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportPartContainmentCount") <> 0 Then
                            txtBudgetAllocatedSupportPartContainmentCount.Text = Format(ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportPartContainmentCount"), "#0")
                            dBudgetAllocatedSupportPartContainmentCount = ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportPartContainmentCount")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualAllocatedSupportPartContainmentCount"

                    If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportPartContainmentCount") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportPartContainmentCount") <> 0 Then
                            txtActualAllocatedSupportPartContainmentCount.Text = Format(ds.Tables(0).Rows(0).Item("ActualAllocatedSupportPartContainmentCount"), "#0")
                            dActualAllocatedSupportPartContainmentCount = ds.Tables(0).Rows(0).Item("ActualAllocatedSupportPartContainmentCount")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: BudgetOffStandardDirectCount"

                If ds.Tables(0).Rows(0).Item("BudgetOffStandardDirectCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetOffStandardDirectCount") <> 0 Then
                        txtBudgetOffStandardDirectCount.Text = Format(ds.Tables(0).Rows(0).Item("BudgetOffStandardDirectCount"), "#0")
                    End If
                End If

                ' strDebugInfo = "Section: ActualOffStandardDirectCount"

                If ds.Tables(0).Rows(0).Item("ActualOffStandardDirectCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualOffStandardDirectCount") <> 0 Then
                        txtActualOffStandardDirectCount.Text = Format(ds.Tables(0).Rows(0).Item("ActualOffStandardDirectCount"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetOffStandardIndirectCount"

                If ds.Tables(0).Rows(0).Item("BudgetOffStandardIndirectCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetOffStandardIndirectCount") <> 0 Then
                        txtBudgetOffStandardIndirectCount.Text = Format(ds.Tables(0).Rows(0).Item("BudgetOffStandardIndirectCount"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualOffStandardIndirectCount"

                If ds.Tables(0).Rows(0).Item("ActualOffStandardIndirectCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualOffStandardIndirectCount") <> 0 Then
                        txtActualOffStandardIndirectCount.Text = Format(ds.Tables(0).Rows(0).Item("ActualOffStandardIndirectCount"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetAllocatedSupportOffStandardIndirectCount"

                If DeptID = 0 Then
                    If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOffStandardIndirectCount") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOffStandardIndirectCount") <> 0 Then
                            txtBudgetAllocatedSupportOffStandardIndirectCount.Text = Format(ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOffStandardIndirectCount"), "#0")
                            dBudgetAllocatedSupportOffStandardIndirectCount = ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOffStandardIndirectCount")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualAllocatedSupportOffStandardIndirectCount"

                    If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOffStandardIndirectCount") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOffStandardIndirectCount") <> 0 Then
                            txtActualAllocatedSupportOffStandardIndirectCount.Text = Format(ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOffStandardIndirectCount"), "#0")
                            dActualAllocatedSupportOffStandardIndirectCount = ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOffStandardIndirectCount")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: BudgetIsStandardizedWork"

                If ds.Tables(0).Rows(0).Item("BudgetIsStandardizedWork") IsNot System.DBNull.Value Then
                    cbBudgetStandardizedCellWork.Checked = ds.Tables(0).Rows(0).Item("BudgetIsStandardizedWork")
                End If

                'strDebugInfo = "Section: ActualIsStandardizedWork"

                If ds.Tables(0).Rows(0).Item("ActualIsStandardizedWork") IsNot System.DBNull.Value Then
                    cbActualStandardizedCellWork.Checked = ds.Tables(0).Rows(0).Item("ActualIsStandardizedWork")
                End If

                'strDebugInfo = "Section: BudgetTeamMemberFactorCount"

                If ds.Tables(0).Rows(0).Item("BudgetTeamMemberFactorCount") IsNot System.DBNull.Value Then
                    txtBudgetTeamMemberFactorCount.Text = Format(ds.Tables(0).Rows(0).Item("BudgetTeamMemberFactorCount"), "#")
                    dBudgetTeamMemberFactorCount = ds.Tables(0).Rows(0).Item("BudgetTeamMemberFactorCount")
                End If

                'strDebugInfo = "Section: BudgetTeamLeaderFactorCount"

                If ds.Tables(0).Rows(0).Item("BudgetTeamLeaderFactorCount") IsNot System.DBNull.Value Then
                    txtBudgetTeamLeaderFactorCount.Text = Format(ds.Tables(0).Rows(0).Item("BudgetTeamLeaderFactorCount"), "#")
                    dBudgetTeamLeaderFactorCount = ds.Tables(0).Rows(0).Item("BudgetTeamLeaderFactorCount")
                End If

                ' strDebugInfo = "Section: ActualTeamMemberFactorCount"

                If ds.Tables(0).Rows(0).Item("ActualTeamMemberFactorCount") IsNot System.DBNull.Value Then
                    txtActualTeamMemberFactorCount.Text = Format(ds.Tables(0).Rows(0).Item("ActualTeamMemberFactorCount"), "#")
                    dActualTeamMemberFactorCount = ds.Tables(0).Rows(0).Item("ActualTeamMemberFactorCount")
                End If

                ' strDebugInfo = "Section: ActualTeamLeaderFactorCount"

                If ds.Tables(0).Rows(0).Item("ActualTeamLeaderFactorCount") IsNot System.DBNull.Value Then
                    txtActualTeamLeaderFactorCount.Text = Format(ds.Tables(0).Rows(0).Item("ActualTeamLeaderFactorCount"), "#")
                    dActualTeamLeaderFactorCount = ds.Tables(0).Rows(0).Item("ActualTeamLeaderFactorCount")
                End If

                'strDebugInfo = "Section: BudgetCapacityUtilization"

                If ds.Tables(0).Rows(0).Item("BudgetCapacityUtilization") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetCapacityUtilization") <> 0 Then
                        txtBudgetCapacityUtilization.Text = Format(ds.Tables(0).Rows(0).Item("BudgetCapacityUtilization"), "#0.0")
                        dBudgetCapacityUtilization = ds.Tables(0).Rows(0).Item("BudgetCapacityUtilization")
                    End If
                End If

                'strDebugInfo = "Section: ActualCapacityUtilization"

                If ds.Tables(0).Rows(0).Item("ActualCapacityUtilization") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualCapacityUtilization") <> 0 Then
                        txtActualCapacityUtilization.Text = Format(ds.Tables(0).Rows(0).Item("ActualCapacityUtilization"), "#0.0")
                        dActualCapacityUtilization = ds.Tables(0).Rows(0).Item("ActualCapacityUtilization")
                    End If
                End If

                'strDebugInfo = "Section: OEEBudgetGoodPartCount"

                If ds.Tables(0).Rows(0).Item("OEEBudgetGoodPartCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OEEBudgetGoodPartCount") <> 0 Then
                        txtOEEBudgetGoodPartCount.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetGoodPartCount"), "#0")
                        dOEEBudgetGoodPartCount = CType(txtOEEBudgetGoodPartCount.Text.Trim, Double)
                    End If
                End If

                'strDebugInfo = "Section: OEEActualGoodPartCount"

                If ds.Tables(0).Rows(0).Item("OEEActualGoodPartCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OEEActualGoodPartCount") <> 0 Then
                        txtOEEActualGoodPartCount.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualGoodPartCount"), "#0")
                        dOEEActualGoodPartCount = CType(txtOEEActualGoodPartCount.Text.Trim, Double)
                    End If
                End If

                'strDebugInfo = "Section: OEEBudgetScrapPartCount"

                If ds.Tables(0).Rows(0).Item("OEEBudgetScrapPartCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OEEBudgetScrapPartCount") <> 0 Then
                        txtOEEBudgetScrapPartCount.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetScrapPartCount"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: OEEActualScrapPartCount"

                If ds.Tables(0).Rows(0).Item("OEEActualScrapPartCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OEEActualScrapPartCount") <> 0 Then
                        txtOEEActualScrapPartCount.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualScrapPartCount"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: OEEBudgetTotalPartCount"

                If ds.Tables(0).Rows(0).Item("OEEBudgetTotalPartCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OEEBudgetTotalPartCount") <> 0 Then
                        txtOEEBudgetTotalPartCount.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetTotalPartCount"), "#0")
                        dOEEBudgetTotalPartCount = CType(txtOEEBudgetTotalPartCount.Text.Trim, Double)
                    End If
                End If

                'strDebugInfo = "Section: OEEActualTotalPartCount"

                If ds.Tables(0).Rows(0).Item("OEEActualTotalPartCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OEEActualTotalPartCount") <> 0 Then
                        txtOEEActualTotalPartCount.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualTotalPartCount"), "#0")
                        dOEEActualTotalPartCount = CType(txtOEEActualTotalPartCount.Text.Trim, Double)
                    End If
                End If

                'strDebugInfo = "Section: OEEActualUtilization"

                If DeptID > 0 Then
                    If ds.Tables(0).Rows(0).Item("OEEActualUtilization") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("OEEActualUtilization") <> 0 Then
                            txtOEEActualUtilization.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualUtilization"), "#0.0")
                            lblActualMachineUtilization.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualUtilization"), "#0.0")
                        End If
                    End If
                Else

                    'strDebugInfo = "Section: ActualMachineUtilization"

                    If ds.Tables(0).Rows(0).Item("ActualMachineUtilization") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualMachineUtilization") <> 0 Then
                            txtOEEActualUtilization.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineUtilization"), "#0.0")
                            lblActualMachineUtilization.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineUtilization"), "#0.0")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: OEEBudgetAvailableHours"

                If ds.Tables(0).Rows(0).Item("OEEBudgetAvailableHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OEEBudgetAvailableHours") <> 0 Then
                        txtOEEBudgetAvailableHours.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetAvailableHours"), "#0.#0")
                        dOEEBudgetAvailableHours = CType(txtOEEBudgetAvailableHours.Text.Trim, Double)
                    End If
                End If

                'strDebugInfo = "Section: OEEActualAvailableHours"

                If ds.Tables(0).Rows(0).Item("OEEActualAvailableHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OEEActualAvailableHours") <> 0 Then
                        txtOEEActualAvailableHours.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualAvailableHours"), "#0.#0")
                        dOEEActualAvailableHours = CType(txtOEEActualAvailableHours.Text.Trim, Double)
                    End If
                End If

                'strDebugInfo = "Section: OEEBudgetDownHours"

                If ds.Tables(0).Rows(0).Item("OEEBudgetDownHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OEEBudgetDownHours") <> 0 Then
                        txtOEEBudgetDownHours.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetDownHours"), "###0.#0")
                        dOEEBudgetDownHours = CType(txtOEEBudgetDownHours.Text.Trim, Double)
                    End If
                End If

                'strDebugInfo = "Section: OEEActualDownHours"

                If ds.Tables(0).Rows(0).Item("OEEActualDownHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OEEActualDownHours") <> 0 Then
                        txtOEEActualDownHours.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualDownHours"), "###0.#0")
                        dOEEActualDownHours = CType(txtOEEActualDownHours.Text.Trim, Double)
                    End If
                End If

                'strDebugInfo = "Section: MonthlyShippingDays"

                If DeptID > 0 Then

                    If ds.Tables(0).Rows(0).Item("MonthlyShippingDays") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("MonthlyShippingDays") <> 0 Then
                            txtMonthlyShippingDays.Text = Format(ds.Tables(0).Rows(0).Item("MonthlyShippingDays"), "#0")
                            iMonthlyShippingDays = ds.Tables(0).Rows(0).Item("MonthlyShippingDays")
                        End If
                    End If

                    'strDebugInfo = "Section: HoursPerShift"

                    If ds.Tables(0).Rows(0).Item("HoursPerShift") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("HoursPerShift") <> 0 Then
                            txtHoursPerShift.Text = Format(ds.Tables(0).Rows(0).Item("HoursPerShift"), "#0.##")
                            dHoursPerShift = ds.Tables(0).Rows(0).Item("HoursPerShift")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualShiftCount"

                    If ds.Tables(0).Rows(0).Item("ActualShiftCount") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualShiftCount") <> 0 Then
                            lblActualShiftCount.Text = Format(ds.Tables(0).Rows(0).Item("ActualShiftCount"), "#0.#0")
                        End If
                    End If

                    'strDebugInfo = "Section: AvailablePerShiftFactor"

                    If ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor") <> 0 Then
                            lblAvailablePerShiftFactor.Text = Format(ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor"), "#0.#0")
                            dAvailablePerShiftFactor = ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor")
                        End If
                    End If

                End If

                'strDebugInfo = "Section: BudgetDowntimeHours"

                If ds.Tables(0).Rows(0).Item("BudgetDowntimeHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetDowntimeHours") <> 0 Then
                        txtBudgetDowntimeHours.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDowntimeHours"), "#0.#0")
                        dBudgetDowntimeHours = ds.Tables(0).Rows(0).Item("BudgetDowntimeHours")
                    End If
                End If

                'strDebugInfo = "Section: ActualDowntimeHours"

                If ds.Tables(0).Rows(0).Item("ActualDowntimeHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualDowntimeHours") <> 0 Then
                        txtActualDowntimeHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualDowntimeHours"), "#0.#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetMachineWorkedHours"

                If ds.Tables(0).Rows(0).Item("BudgetMachineWorkedHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetMachineWorkedHours") <> 0 Then
                        txtBudgetMachineWorkedHours.Text = Format(ds.Tables(0).Rows(0).Item("BudgetMachineWorkedHours"), "#0.#0")
                        dBudgetMachineWorkedHours = ds.Tables(0).Rows(0).Item("BudgetMachineWorkedHours")
                    End If
                End If

                'strDebugInfo = "Section: ActualMachineWorkedHours"

                If ds.Tables(0).Rows(0).Item("ActualMachineWorkedHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualMachineWorkedHours") <> 0 Then
                        txtActualMachineWorkedHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineWorkedHours"), "#0.#0")
                        dActualMachineWorkedHours = ds.Tables(0).Rows(0).Item("ActualMachineWorkedHours")
                    End If
                End If

                'strDebugInfo = "Section: BudgetMachineAvailableHours"

                If ds.Tables(0).Rows(0).Item("BudgetMachineAvailableHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetMachineAvailableHours") <> 0 Then
                        txtBudgetMachineAvailableHours.Text = Format(ds.Tables(0).Rows(0).Item("BudgetMachineAvailableHours"), "#0.#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualMachineAvailableHours"

                If ds.Tables(0).Rows(0).Item("ActualMachineAvailableHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualMachineAvailableHours") <> 0 Then
                        txtActualMachineAvailableHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineAvailableHours"), "#0.#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetMachineHourStandard"

                If ds.Tables(0).Rows(0).Item("BudgetMachineHourStandard") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetMachineHourStandard") <> 0 Then
                        dBudgetMachineStandardHours = ds.Tables(0).Rows(0).Item("BudgetMachineHourStandard")
                        txtBudgetMachineStandardHours.Text = Format(ds.Tables(0).Rows(0).Item("BudgetMachineHourStandard"), "#0.#0")
                    End If
                End If

                ' strDebugInfo = "Section: ActualMachineHourStandard"

                If ds.Tables(0).Rows(0).Item("ActualMachineHourStandard") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualMachineHourStandard") <> 0 Then
                        dActualMachineStandardHours = ds.Tables(0).Rows(0).Item("ActualMachineHourStandard")
                        txtActualMachineStandardHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineHourStandard"), "#0.#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetManWorkedHours"

                If DeptID > 0 Then

                    If ds.Tables(0).Rows(0).Item("BudgetManWorkedHours") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetManWorkedHours") <> 0 Then
                            txtBudgetManWorkedHours.Text = Format(ds.Tables(0).Rows(0).Item("BudgetManWorkedHours"), "#0")
                            dBudgetManWorkedHours = ds.Tables(0).Rows(0).Item("BudgetManWorkedHours")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualManWorkedHours"

                    If ds.Tables(0).Rows(0).Item("ActualManWorkedHours") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualManWorkedHours") <> 0 Then
                            txtActualManWorkedHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualManWorkedHours"), "#0")
                        End If
                    End If

                    'strDebugInfo = "Section: BudgetDowntimeManHours"

                    If ds.Tables(0).Rows(0).Item("BudgetDowntimeManHours") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetDowntimeManHours") <> 0 Then
                            txtBudgetDowntimeManHours.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDowntimeManHours"), "#0")
                            dBudgetDowntimeManHours = ds.Tables(0).Rows(0).Item("BudgetDowntimeManHours")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualDowntimeManHours"

                    If ds.Tables(0).Rows(0).Item("ActualDowntimeManHours") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualDowntimeManHours") <> 0 Then
                            txtActualDowntimeManHours.Text = Format(ds.Tables(0).Rows(0).Item("ActualDowntimeManHours"), "#0")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: TotalBudgetProductionDollar"

                If ds.Tables(0).Rows(0).Item("TotalBudgetProductionDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TotalBudgetProductionDollar") <> 0 Then
                        txtTotalBudgetProductionDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalBudgetProductionDollar"), "#0.#0")
                        dTotalBudgetProductionDollar = ds.Tables(0).Rows(0).Item("TotalBudgetProductionDollar")
                    End If
                End If

                'strDebugInfo = "Section: TotalActualProductionDollar"

                If ds.Tables(0).Rows(0).Item("TotalActualProductionDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TotalActualProductionDollar") <> 0 Then
                        txtTotalActualProductionDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalActualProductionDollar"), "#0.#0")
                        dTotalActualProductionDollar = ds.Tables(0).Rows(0).Item("TotalActualProductionDollar")
                    End If
                End If

                'strDebugInfo = "Section: TotalBudgetSpecificScrapDollar"

                If ds.Tables(0).Rows(0).Item("TotalBudgetSpecificScrapDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TotalBudgetSpecificScrapDollar") <> 0 Then
                        txtTotalBudgetSpecificScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalBudgetSpecificScrapDollar"), "#0.#0")
                        dTotalBudgetSpecificScrapDollar = ds.Tables(0).Rows(0).Item("TotalBudgetSpecificScrapDollar")
                    End If
                End If

                'strDebugInfo = "Section: TotalActualSpecificScrapDollar"

                If ds.Tables(0).Rows(0).Item("TotalActualSpecificScrapDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TotalActualSpecificScrapDollar") <> 0 Then
                        txtTotalActualSpecificScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalActualSpecificScrapDollar"), "#0.#0")
                        dTotalActualSpecificScrapDollar = ds.Tables(0).Rows(0).Item("TotalActualSpecificScrapDollar")
                    End If
                End If

                'strDebugInfo = "Section: TotalBudgetMiscScrapDollar"

                If ds.Tables(0).Rows(0).Item("TotalBudgetMiscScrapDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TotalBudgetMiscScrapDollar") <> 0 Then
                        txtTotalBudgetMiscScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalBudgetMiscScrapDollar"), "#0.#0")
                        dTotalBudgetMiscScrapDollar = ds.Tables(0).Rows(0).Item("TotalBudgetMiscScrapDollar")
                    End If
                End If

                'strDebugInfo = "Section: TotalActualMiscScrapDollar"

                If ds.Tables(0).Rows(0).Item("TotalActualMiscScrapDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TotalActualMiscScrapDollar") <> 0 Then
                        txtTotalActualMiscScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalActualMiscScrapDollar"), "#0.#0")
                        dTotalActualMiscScrapDollar = ds.Tables(0).Rows(0).Item("TotalActualMiscScrapDollar")
                    End If
                End If

                'strDebugInfo = "Section: TotalActualIndirectScrapDollar"

                If ds.Tables(0).Rows(0).Item("TotalActualIndirectScrapDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TotalActualIndirectScrapDollar") <> 0 Then
                        txtTotalActualIndirectScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalActualIndirectScrapDollar"), "#0.#0")
                        dTotalActualIndirectScrapDollar = ds.Tables(0).Rows(0).Item("TotalActualIndirectScrapDollar")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("BudgetRawWipScrapDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetRawWipScrapDollar") <> 0 Then
                        txtTotalBudgetRawWipScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("BudgetRawWipScrapDollar"), "#0.#0")
                        dTotalBudgetRawWipScrapDollar = ds.Tables(0).Rows(0).Item("BudgetRawWipScrapDollar")
                    End If
                End If

                'strDebugInfo = "Section: TotalBudgetRawWipScrapDollar"

                If ds.Tables(0).Rows(0).Item("ActualRawWipScrapDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualRawWipScrapDollar") <> 0 Then
                        txtTotalActualRawWipScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("ActualRawWipScrapDollar"), "#0.#0")
                        dTotalActualRawWipScrapDollar = ds.Tables(0).Rows(0).Item("ActualRawWipScrapDollar")
                    End If
                End If

                'strDebugInfo = "Section: TotalActualRawWipScrapDollar"

                'strDebugInfo = "Section: BudgetDirectPerm"

                If ds.Tables(0).Rows(0).Item("BudgetDirectPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetDirectPerm") <> 0 Then
                        txtBudgetDirectPerm.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDirectPerm"), "#0")
                    End If
                End If

                ' strDebugInfo = "Section: FlexDirectPerm"

                If ds.Tables(0).Rows(0).Item("FlexDirectPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("FlexDirectPerm") <> 0 Then
                        txtFlexDirectPerm.Text = Format(ds.Tables(0).Rows(0).Item("FlexDirectPerm"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualDirectPerm"

                If ds.Tables(0).Rows(0).Item("ActualDirectPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualDirectPerm") <> 0 Then
                        txtActualDirectPerm.Text = Format(ds.Tables(0).Rows(0).Item("ActualDirectPerm"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetDirectTemp"

                If ds.Tables(0).Rows(0).Item("BudgetDirectTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetDirectTemp") <> 0 Then
                        txtBudgetDirectTemp.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDirectTemp"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: FlexDirectTemp"

                If ds.Tables(0).Rows(0).Item("FlexDirectTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("FlexDirectTemp") <> 0 Then
                        txtFlexDirectTemp.Text = Format(ds.Tables(0).Rows(0).Item("FlexDirectTemp"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualDirectTemp"

                If ds.Tables(0).Rows(0).Item("ActualDirectTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualDirectTemp") <> 0 Then
                        txtActualDirectTemp.Text = Format(ds.Tables(0).Rows(0).Item("ActualDirectTemp"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetIndirectPerm"

                If ds.Tables(0).Rows(0).Item("BudgetIndirectPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetIndirectPerm") <> 0 Then
                        txtBudgetIndirectPerm.Text = Format(ds.Tables(0).Rows(0).Item("BudgetIndirectPerm"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: FlexIndirectPerm"

                If ds.Tables(0).Rows(0).Item("FlexIndirectPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("FlexIndirectPerm") <> 0 Then
                        txtFlexIndirectPerm.Text = Format(ds.Tables(0).Rows(0).Item("FlexIndirectPerm"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualIndirectPerm"

                If ds.Tables(0).Rows(0).Item("ActualIndirectPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualIndirectPerm") <> 0 Then
                        txtActualIndirectPerm.Text = Format(ds.Tables(0).Rows(0).Item("ActualIndirectPerm"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetAllocatedSupportIndirectPerm"

                If DeptID = 0 Then
                    If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportIndirectPerm") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportIndirectPerm") <> 0 Then
                            txtBudgetAllocatedSupportIndirectPerm.Text = Format(ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportIndirectPerm"), "#0")
                            dBudgetAllocatedSupportIndirectPerm = ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportIndirectPerm")
                        End If
                    End If

                    'strDebugInfo = "Section: FlexAllocatedSupportIndirectPerm"

                    If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportIndirectPerm") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportIndirectPerm") <> 0 Then
                            txtFlexAllocatedSupportIndirectPerm.Text = Format(ds.Tables(0).Rows(0).Item("FlexAllocatedSupportIndirectPerm"), "#0")
                            dFlexAllocatedSupportIndirectPerm = ds.Tables(0).Rows(0).Item("FlexAllocatedSupportIndirectPerm")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualAllocatedSupportIndirectPerm"

                    If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportIndirectPerm") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportIndirectPerm") <> 0 Then
                            txtActualAllocatedSupportIndirectPerm.Text = Format(ds.Tables(0).Rows(0).Item("ActualAllocatedSupportIndirectPerm"), "#0")
                            dActualAllocatedSupportIndirectPerm = ds.Tables(0).Rows(0).Item("ActualAllocatedSupportIndirectPerm")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: BudgetIndirectTemp"

                If ds.Tables(0).Rows(0).Item("BudgetIndirectTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetIndirectTemp") <> 0 Then
                        txtBudgetIndirectTemp.Text = Format(ds.Tables(0).Rows(0).Item("BudgetIndirectTemp"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: FlexIndirectTemp"

                If ds.Tables(0).Rows(0).Item("FlexIndirectTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("FlexIndirectTemp") <> 0 Then
                        txtFlexIndirectTemp.Text = Format(ds.Tables(0).Rows(0).Item("FlexIndirectTemp"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualIndirectTemp"

                If ds.Tables(0).Rows(0).Item("ActualIndirectTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualIndirectTemp") <> 0 Then
                        txtActualIndirectTemp.Text = Format(ds.Tables(0).Rows(0).Item("ActualIndirectTemp"), "#0")
                    End If
                End If

                ' strDebugInfo = "Section: BudgetAllocatedSupportIndirectTemp"

                If DeptID = 0 Then
                    If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportIndirectTemp") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportIndirectTemp") <> 0 Then
                            txtBudgetAllocatedSupportIndirectTemp.Text = Format(ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportIndirectTemp"), "#0")
                            dBudgetAllocatedSupportIndirectTemp = ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportIndirectTemp")
                        End If
                    End If

                    'strDebugInfo = "Section: FlexAllocatedSupportIndirectTemp"

                    If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportIndirectTemp") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportIndirectTemp") <> 0 Then
                            txtFlexAllocatedSupportIndirectTemp.Text = Format(ds.Tables(0).Rows(0).Item("FlexAllocatedSupportIndirectTemp"), "#0")
                            dFlexAllocatedSupportIndirectTemp = ds.Tables(0).Rows(0).Item("FlexAllocatedSupportIndirectTemp")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualAllocatedSupportIndirectTemp"

                    If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportIndirectTemp") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportIndirectTemp") <> 0 Then
                            txtActualAllocatedSupportIndirectTemp.Text = Format(ds.Tables(0).Rows(0).Item("ActualAllocatedSupportIndirectTemp"), "#0")
                            dActualAllocatedSupportIndirectTemp = ds.Tables(0).Rows(0).Item("ActualAllocatedSupportIndirectTemp")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: BudgetOfficeHourlyPerm"

                If ds.Tables(0).Rows(0).Item("BudgetOfficeHourlyPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetOfficeHourlyPerm") <> 0 Then
                        txtBudgetOfficeHourlyPerm.Text = Format(ds.Tables(0).Rows(0).Item("BudgetOfficeHourlyPerm"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: FlexOfficeHourlyPerm"

                If ds.Tables(0).Rows(0).Item("FlexOfficeHourlyPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("FlexOfficeHourlyPerm") <> 0 Then
                        txtFlexOfficeHourlyPerm.Text = Format(ds.Tables(0).Rows(0).Item("FlexOfficeHourlyPerm"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualOfficeHourlyPerm"

                If ds.Tables(0).Rows(0).Item("ActualOfficeHourlyPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualOfficeHourlyPerm") <> 0 Then
                        txtActualOfficeHourlyPerm.Text = Format(ds.Tables(0).Rows(0).Item("ActualOfficeHourlyPerm"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetAllocatedSupportOfficeHourlyPerm"

                If DeptID = 0 Then
                    If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOfficeHourlyPerm") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOfficeHourlyPerm") <> 0 Then
                            txtBudgetAllocatedSupportOfficeHourlyPerm.Text = Format(ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOfficeHourlyPerm"), "#0")
                            dBudgetAllocatedSupportOfficeHourlyPerm = ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOfficeHourlyPerm")
                        End If
                    End If

                    'strDebugInfo = "Section: FlexAllocatedSupportOfficeHourlyPerm"

                    If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportOfficeHourlyPerm") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportOfficeHourlyPerm") <> 0 Then
                            txtFlexAllocatedSupportOfficeHourlyPerm.Text = Format(ds.Tables(0).Rows(0).Item("FlexAllocatedSupportOfficeHourlyPerm"), "#0")
                            dFlexAllocatedSupportOfficeHourlyPerm = ds.Tables(0).Rows(0).Item("FlexAllocatedSupportOfficeHourlyPerm")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualAllocatedSupportOfficeHourlyPerm"

                    If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOfficeHourlyPerm") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOfficeHourlyPerm") <> 0 Then
                            txtActualAllocatedSupportOfficeHourlyPerm.Text = Format(ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOfficeHourlyPerm"), "#0")
                            dActualAllocatedSupportOfficeHourlyPerm = ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOfficeHourlyPerm")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: BudgetOfficeHourlyTemp"

                If ds.Tables(0).Rows(0).Item("BudgetOfficeHourlyTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetOfficeHourlyTemp") <> 0 Then
                        txtBudgetOfficeHourlyTemp.Text = Format(ds.Tables(0).Rows(0).Item("BudgetOfficeHourlyTemp"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: FlexOfficeHourlyTemp"

                If ds.Tables(0).Rows(0).Item("FlexOfficeHourlyTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("FlexOfficeHourlyTemp") <> 0 Then
                        txtFlexOfficeHourlyTemp.Text = Format(ds.Tables(0).Rows(0).Item("FlexOfficeHourlyTemp"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualOfficeHourlyTemp"

                If ds.Tables(0).Rows(0).Item("ActualOfficeHourlyTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualOfficeHourlyTemp") <> 0 Then
                        txtActualOfficeHourlyTemp.Text = Format(ds.Tables(0).Rows(0).Item("ActualOfficeHourlyTemp"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetAllocatedSupportOfficeHourlyTemp"

                If DeptID = 0 Then
                    If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOfficeHourlyTemp") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOfficeHourlyTemp") <> 0 Then
                            txtBudgetAllocatedSupportOfficeHourlyTemp.Text = Format(ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOfficeHourlyTemp"), "#0")
                            dBudgetAllocatedSupportOfficeHourlyTemp = ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportOfficeHourlyTemp")
                        End If
                    End If

                    'strDebugInfo = "Section: FlexAllocatedSupportOfficeHourlyTemp"

                    If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportOfficeHourlyTemp") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportOfficeHourlyTemp") <> 0 Then
                            txtFlexAllocatedSupportOfficeHourlyTemp.Text = Format(ds.Tables(0).Rows(0).Item("FlexAllocatedSupportOfficeHourlyTemp"), "#0")
                            dFlexAllocatedSupportOfficeHourlyTemp = ds.Tables(0).Rows(0).Item("FlexAllocatedSupportOfficeHourlyTemp")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualAllocatedSupportOfficeHourlyTemp"

                    If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOfficeHourlyTemp") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOfficeHourlyTemp") <> 0 Then
                            txtActualAllocatedSupportOfficeHourlyTemp.Text = Format(ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOfficeHourlyTemp"), "#0")
                            dActualAllocatedSupportOfficeHourlyTemp = ds.Tables(0).Rows(0).Item("ActualAllocatedSupportOfficeHourlyTemp")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: BudgetSalaryPerm"

                If ds.Tables(0).Rows(0).Item("BudgetSalaryPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetSalaryPerm") <> 0 Then
                        txtBudgetSalaryPerm.Text = Format(ds.Tables(0).Rows(0).Item("BudgetSalaryPerm"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: FlexSalaryPerm"

                If ds.Tables(0).Rows(0).Item("FlexSalaryPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("FlexSalaryPerm") <> 0 Then
                        txtFlexSalaryPerm.Text = Format(ds.Tables(0).Rows(0).Item("FlexSalaryPerm"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualSalaryPerm"

                If ds.Tables(0).Rows(0).Item("ActualSalaryPerm") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualSalaryPerm") <> 0 Then
                        txtActualSalaryPerm.Text = Format(ds.Tables(0).Rows(0).Item("ActualSalaryPerm"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetAllocatedSupportSalaryPerm"

                If DeptID = 0 Then
                    If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportSalaryPerm") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportSalaryPerm") <> 0 Then
                            txtBudgetAllocatedSupportSalaryPerm.Text = Format(ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportSalaryPerm"), "#0")
                            dBudgetAllocatedSupportSalaryPerm = ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportSalaryPerm")
                        End If
                    End If

                    'strDebugInfo = "Section: FlexAllocatedSupportSalaryPerm"

                    If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportSalaryPerm") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportSalaryPerm") <> 0 Then
                            txtFlexAllocatedSupportSalaryPerm.Text = Format(ds.Tables(0).Rows(0).Item("FlexAllocatedSupportSalaryPerm"), "#0")
                            dFlexAllocatedSupportSalaryPerm = ds.Tables(0).Rows(0).Item("FlexAllocatedSupportSalaryPerm")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualAllocatedSupportSalaryPerm"

                    If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportSalaryPerm") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportSalaryPerm") <> 0 Then
                            txtActualAllocatedSupportSalaryPerm.Text = Format(ds.Tables(0).Rows(0).Item("ActualAllocatedSupportSalaryPerm"), "#0")
                            dActualAllocatedSupportSalaryPerm = ds.Tables(0).Rows(0).Item("ActualAllocatedSupportSalaryPerm")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: BudgetSalaryTemp"

                If ds.Tables(0).Rows(0).Item("BudgetSalaryTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetSalaryTemp") <> 0 Then
                        txtBudgetSalaryTemp.Text = Format(ds.Tables(0).Rows(0).Item("BudgetSalaryTemp"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: FlexSalaryTemp"

                If ds.Tables(0).Rows(0).Item("FlexSalaryTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("FlexSalaryTemp") <> 0 Then
                        txtFlexSalaryTemp.Text = Format(ds.Tables(0).Rows(0).Item("FlexSalaryTemp"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: ActualSalaryTemp"

                If ds.Tables(0).Rows(0).Item("ActualSalaryTemp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualSalaryTemp") <> 0 Then
                        txtActualSalaryTemp.Text = Format(ds.Tables(0).Rows(0).Item("ActualSalaryTemp"), "#0")
                    End If
                End If

                'strDebugInfo = "Section: BudgetAllocatedSupportSalaryTemp"

                If DeptID = 0 Then
                    If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportSalaryTemp") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportSalaryTemp") <> 0 Then
                            txtBudgetAllocatedSupportSalaryTemp.Text = Format(ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportSalaryTemp"), "#0")
                            dBudgetAllocatedSupportSalaryTemp = ds.Tables(0).Rows(0).Item("BudgetAllocatedSupportSalaryTemp")
                        End If
                    End If

                    'strDebugInfo = "Section: FlexAllocatedSupportSalaryTemp"

                    If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportSalaryTemp") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("FlexAllocatedSupportSalaryTemp") <> 0 Then
                            txtFlexAllocatedSupportSalaryTemp.Text = Format(ds.Tables(0).Rows(0).Item("FlexAllocatedSupportSalaryTemp"), "#0")
                            dFlexAllocatedSupportSalaryTemp = ds.Tables(0).Rows(0).Item("FlexAllocatedSupportSalaryTemp")
                        End If
                    End If

                    'strDebugInfo = "Section: ActualAllocatedSupportSalaryTemp"

                    If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportSalaryTemp") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("ActualAllocatedSupportSalaryTemp") <> 0 Then
                            txtActualAllocatedSupportSalaryTemp.Text = Format(ds.Tables(0).Rows(0).Item("ActualAllocatedSupportSalaryTemp"), "#0")
                            dActualAllocatedSupportSalaryTemp = ds.Tables(0).Rows(0).Item("ActualAllocatedSupportSalaryTemp")
                        End If
                    End If
                End If

                'strDebugInfo = "Section: UpdatedOn"

                If ds.Tables(0).Rows(0).Item("UpdatedOn").ToString <> "" Then
                    lblUpdatedOn.Text = ds.Tables(0).Rows(0).Item("UpdatedOn").ToString
                End If

                'strDebugInfo = "Section: CalculateProductionPerformance"

                CalculateProductionPerformance(DeptID, dBudgetEarnedDLHours, dBudgetDLHours, dHoursPerShift, dBudgetMachineWorkedHours, _
                                               dBudgetDowntimeHours, iMonthlyShippingDays, dAvailablePerShiftFactor, _
                                               dOEEBudgetTotalPartCount, dOEEBudgetGoodPartCount, dOEEBudgetDownHours, _
                                               dBudgetManWorkedHours, dBudgetDowntimeManHours, dTotalBudgetSpecificScrapDollar, _
                                               dTotalBudgetMiscScrapDollar, dTotalBudgetProductionDollar, dTotalActualSpecificScrapDollar, _
                                               dTotalActualMiscScrapDollar, dTotalActualProductionDollar, dTotalActualIndirectScrapDollar, _
                                               dBudgetTeamMemberFactorCount, dBudgetTeamLeaderFactorCount, dActualTeamMemberFactorCount, _
                                               dActualTeamLeaderFactorCount, dBudgetMachineStandardHours, _
                                               dTotalBudgetRawWipScrapDollar, dTotalActualRawWipScrapDollar)

                'strDebugInfo = "Section: recalculate totals"

                'always recalculate these totals
                If DeptID = 0 Then

                    If dOEEBudgetAvailableHours <> 0 Then
                        dOEEBudgetUtilization = dBudgetMachineWorkedHours / dOEEBudgetAvailableHours
                    End If

                    If dOEEActualAvailableHours <> 0 Then
                        dOEEActualUtilization = dActualMachineWorkedHours / dOEEActualAvailableHours
                    End If

                    If dOEEBudgetUtilization <> 0 Then
                        txtOEEBudgetUtilization.Text = Format(dOEEBudgetUtilization * 100, "#0.0")
                        lblBudgetMachineUtilization.Text = Format(dOEEBudgetUtilization * 100, "#0.0")
                        dBudgetMachineUtilization = dOEEBudgetUtilization * 100
                    End If

                    If dOEEActualUtilization <> 0 Then
                        txtOEEActualUtilization.Text = Format(dOEEActualUtilization * 100, "#0.0")
                        lblActualMachineUtilization.Text = Format(dOEEActualUtilization * 100, "#0.0")
                        dActualMachineUtilization = dOEEActualUtilization * 100
                    End If

                    txtOEEBudgetAvailableHours.Text = Format(dOEEBudgetAvailableHours, "#0")

                    If dOEEBudgetTotalPartCount <> 0 And dOEEBudgetAvailableHours <> 0 Then
                        'dBudgetOEE = (((dOEEBudgetGoodPartCount / dOEEBudgetTotalPartCount) * dOEEBudgetUtilization)) * ((dOEEBudgetAvailableHours - dOEEBudgetDownHours) / dOEEBudgetAvailableHours) * 100
                        dBudgetOEE = (dOEEBudgetGoodPartCount / dOEEBudgetTotalPartCount) * dOEEBudgetUtilization * (dBudgetMachineStandardHours / dBudgetMachineWorkedHours) * 100
                    End If

                    If dOEEActualTotalPartCount <> 0 And dOEEActualAvailableHours <> 0 And dActualMachineWorkedHours <> 0 Then
                        ' dActualOEE = (((dOEEActualGoodPartCount / dOEEActualTotalPartCount) * dOEEActualUtilization)) * ((dOEEActualAvailableHours - dOEEActualDownHours) / dOEEActualAvailableHours) * 100
                        dActualOEE = (dOEEActualGoodPartCount / dOEEActualTotalPartCount) * dOEEActualUtilization * (dActualMachineStandardHours / dActualMachineWorkedHours) * 100
                    End If

                    If dBudgetOEE <> 0 Then
                        txtBudgetOEE.Text = Format(dBudgetOEE, "#0.0")
                    End If

                    If dActualOEE <> 0 Then
                        txtActualOEE.Text = Format(dActualOEE, "#0.0")
                    End If

                End If

            Else

                cbIncludeDepartment.Checked = False

            End If ' if original ds is empty

            'strDebugInfo = "Section:  CalculateTeamMembers"

            CalculateTeamMembers()

            'strDebugInfo = "Section:  HandleDepartmentPopups"

            HandleDepartmentPopups(DeptID)

            'strDebugInfo = "Section:  update totals if user changes department data"

            'need to update totals if user changes department data and then goes back to totals section without pressing save
            If DeptID = 0 And ViewState("ReportID") > 0 Then

                'strDebugInfo = "Section:  update dBudgetScrap totals"

                If txtBudgetScrapPercent.Text.Trim <> "" Then
                    dBudgetScrap = CType(txtBudgetScrapPercent.Text.Trim, Double)
                End If

                'strDebugInfo = "Section:  update dActualScrap totals"

                If txtActualScrapPercent.Text.Trim <> "" Then
                    dActualScrap = CType(txtActualScrapPercent.Text.Trim, Double)
                End If

                'strDebugInfo = "Section:  database update of totals"

                If (ViewState("StatusID") = 1 Or ViewState("StatusID") = 2) And ViewState("isAdmin") Then
                    'if department=0 then save a few details to the totals table
                    PSRModule.UpdateManufacturingMetricTotalByDept(ViewState("ReportID"), _
                    dBudgetOEE, dActualOEE, _
                    dBudgetAllocatedSupportOTHours, dActualAllocatedSupportOTHours, _
                    dBudgetMachineUtilization, dActualMachineUtilization, _
                    dBudgetScrap, dActualScrap, _
                    dBudgetAllocatedSupportTeamMemberContainmentCount, dActualAllocatedSupportTeamMemberContainmentCount, _
                    dBudgetAllocatedSupportPartContainmentCount, dActualAllocatedSupportPartContainmentCount, _
                    dBudgetAllocatedSupportOffStandardIndirectCount, dActualAllocatedSupportOffStandardIndirectCount, _
                    cbBudgetStandardizedCellWork.Checked, cbActualStandardizedCellWork.Checked, "", "", _
                    dBudgetCapacityUtilization, dActualCapacityUtilization, _
                    dBudgetAllocatedSupportIndirectPerm, dFlexAllocatedSupportIndirectPerm, dActualAllocatedSupportIndirectPerm, _
                    dBudgetAllocatedSupportIndirectTemp, dFlexAllocatedSupportIndirectTemp, dActualAllocatedSupportIndirectTemp, _
                    dBudgetAllocatedSupportOfficeHourlyPerm, dFlexAllocatedSupportOfficeHourlyPerm, dActualAllocatedSupportOfficeHourlyPerm, _
                    dBudgetAllocatedSupportOfficeHourlyTemp, dFlexAllocatedSupportOfficeHourlyTemp, dActualAllocatedSupportOfficeHourlyTemp, _
                    dBudgetAllocatedSupportSalaryPerm, dFlexAllocatedSupportSalaryPerm, dActualAllocatedSupportSalaryPerm, _
                    dBudgetAllocatedSupportSalaryTemp, dFlexAllocatedSupportSalaryTemp, dActualAllocatedSupportSalaryTemp, _
                    txtNotes.Text.Trim)
                End If

                'strDebugInfo = "Section:  database finished update of totals"

            End If

            'strDebugInfo = "Section:  Completed BindDetailData"

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            'lblMessage.Text += ex.Message & "<br />Debug Info:" & strDebugInfo & "<br />" & mb.Name
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message & "<br />Debug Info:" & strDebugInfo, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message & "<br />", System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Function BuildPlantControllerNotificationList() As String

        Dim strReturnEmailToAddress As String = ""

        Try
            Dim dsSubscriptionMembers As DataSet
            Dim dsTeamMember As DataSet

            Dim strUGNFacility As String = ""
            Dim iRowCounter As Integer = 0

            If ddUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue
            End If

            If strUGNFacility <> "" Then
                'get AR Plant Controllers Subscription Team Members and their email addresses
                dsSubscriptionMembers = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(20, strUGNFacility)
                If dsSubscriptionMembers IsNot Nothing Then
                    If (dsSubscriptionMembers.Tables.Count > 0 And dsSubscriptionMembers.Tables.Item(0).Rows.Count > 0) Then
                        For iRowCounter = 0 To dsSubscriptionMembers.Tables.Item(0).Rows.Count - 1

                            'do not get self
                            If dsSubscriptionMembers.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value Then
                                If dsSubscriptionMembers.Tables(0).Rows(iRowCounter).Item("TMID") > 0 Then
                                    If dsSubscriptionMembers.Tables(0).Rows(iRowCounter).Item("TMID") <> ViewState("TeamMemberID") Then
                                        'get email of Team Member
                                        dsTeamMember = SecurityModule.GetTeamMember(dsSubscriptionMembers.Tables(0).Rows(iRowCounter).Item("TMID"), "", "", "", "", "", True, Nothing)
                                        If dsTeamMember IsNot Nothing Then
                                            If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                                                If strReturnEmailToAddress <> "" Then
                                                    strReturnEmailToAddress += ";"
                                                End If

                                                strReturnEmailToAddress += dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                                            End If
                                        End If
                                    End If
                                End If
                            End If


                        Next
                    End If
                End If
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        Return strReturnEmailToAddress

    End Function
    Private Function BuildInternalReviewNotificationlist() As String

        'get internal review list based on facility

        Dim strReturnEmailToAddress As String = ""

        Try

            Dim dsSubscription As DataSet

            Dim strUGNFacility As String = ""
            Dim iRowCounter As Integer = 0

            If ddUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue
            End If

            'get emails based on subscription and facility
            If strUGNFacility <> "" Then

                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(89, strUGNFacility)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then

                    For iRowCounter = 0 To dsSubscription.Tables(0).Rows.Count - 1
                        If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") IsNot System.DBNull.Value Then

                            'get working team members only
                            If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then

                                'get real email addresses only
                                If dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString <> "" Then

                                    'do not put duplicate email addresses in list
                                    If InStr(strReturnEmailToAddress, dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString()) <= 0 Then

                                        If strReturnEmailToAddress <> "" Then
                                            strReturnEmailToAddress &= ";"
                                        End If

                                        'append to list
                                        strReturnEmailToAddress &= dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString()
                                    End If

                                End If
                            End If
                        End If
                    Next

                End If
            End If


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        BuildInternalReviewNotificationlist = strReturnEmailToAddress

    End Function

    Private Function BuildFinalReviewNotificationlist() As String

        'get final review list based on facility AND Tinley Park

        Dim strReturnEmailToAddress As String = ""

        Try

            Dim dsSubscription As DataSet

            Dim strUGNFacility As String = ""
            Dim iRowCounter As Integer = 0

            If ddUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue
            End If

            'get emails based on subscription and facility
            If strUGNFacility <> "" Then

                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(90, strUGNFacility)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    For iRowCounter = 0 To dsSubscription.Tables(0).Rows.Count - 1
                        If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") IsNot System.DBNull.Value Then

                            'get working team members only
                            If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then

                                'get real email addresses only
                                If dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString <> "" Then

                                    'do not put duplicate email addresses in list
                                    If InStr(strReturnEmailToAddress, dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString()) <= 0 Then

                                        If strReturnEmailToAddress <> "" Then
                                            strReturnEmailToAddress &= ";"
                                        End If

                                        'append to list
                                        strReturnEmailToAddress &= dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString()
                                    End If

                                End If
                            End If
                        End If
                    Next

                End If
            End If

            'get emails based on subscription and Tinley Park/Corporate
            If strUGNFacility <> "" Then

                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(90, "UT")
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    For iRowCounter = 0 To dsSubscription.Tables(0).Rows.Count - 1
                        If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") IsNot System.DBNull.Value Then

                            'get working team members only
                            If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then

                                'get real email addresses only
                                If dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString <> "" Then

                                    'do not put duplicate email addresses in list
                                    If InStr(strReturnEmailToAddress, dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString()) <= 0 Then

                                        If strReturnEmailToAddress <> "" Then
                                            strReturnEmailToAddress &= ";"
                                        End If

                                        'append to list
                                        strReturnEmailToAddress &= dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString()
                                    End If

                                End If
                            End If
                        End If
                    Next

                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        BuildFinalReviewNotificationlist = strReturnEmailToAddress

    End Function

    Private Sub CalculateProductionPerformance(ByVal DeptID As Integer, ByVal BudgetEarnedDLHours As Double, _
    ByVal BudgetDLHours As Double, ByVal HoursPerShift As Double, ByVal BudgetMachineHours As Double, _
    ByVal BudgetDowntimeHours As Double, ByVal UGNMonthlyShippingDays As Integer, _
    ByVal AvailablePerShiftFactor As Double, ByVal OEEBudgetTotalPartCount As Double, _
    ByVal OEEBudgetGoodPartCount As Double, ByVal OEEBudgetDownHours As Double, _
    ByVal BudgetManWorkedHours As Double, ByVal BudgetDowntimeManHours As Double, _
    ByVal TotalBudgetSpecificScrapDollar As Double, ByVal TotalBudgetMiscScrapDollar As Double, _
    ByVal TotalBudgetProductionDollar As Double, _
    ByVal TotalActualSpecificScrapDollar As Double, ByVal TotalActualMiscScrapDollar As Double, _
    ByVal TotalActualProductionDollar As Double, ByVal TotalActualIndirectScrapDollar As Double, _
    ByVal BudgetTeamMemberFactorCount As Integer, ByVal BudgetTeamLeaderFactorCount As Integer, _
    ByVal ActualTeamMemberFactorCount As Integer, ByVal ActualTeamLeaderFactorCount As Integer, _
    ByVal BudgetMachineHourStandard As Double, ByVal TotalBudgetRawWipScrapDollar As Double, _
    ByVal TotalActualRawWipScrapDollar As Double)

        Try
            Dim dBudgetLaborProductivity As Double = 0
            Dim dBudgetShiftCount As Double = 0
            Dim dOEEBudgetAvailableHours As Double = 0
            Dim dOEEBudgetUtilization As Double = 0
            Dim dBudgetOEE As Double = 0
            Dim dTempBudgetDLHours As Double = 0

            If HoursPerShift <> 0 Then
                dBudgetShiftCount = Round((BudgetMachineHours + BudgetDowntimeHours) / HoursPerShift, 2)
                lblBudgetShiftCount.Text = Format(dBudgetShiftCount, "#0.##")
            End If

            dOEEBudgetAvailableHours = CType(dBudgetShiftCount * UGNMonthlyShippingDays * AvailablePerShiftFactor, Integer)

            txtOEEBudgetAvailableHours.Text = Format(dOEEBudgetAvailableHours, "#0")

            If dOEEBudgetAvailableHours <> 0 Then
                dOEEBudgetUtilization = BudgetMachineHours / dOEEBudgetAvailableHours
            End If

            txtOEEBudgetUtilization.Text = Format(dOEEBudgetUtilization * 100, "#0.0")
            lblBudgetMachineUtilization.Text = Format(dOEEBudgetUtilization * 100, "#0.0")

            If OEEBudgetTotalPartCount <> 0 And dOEEBudgetAvailableHours <> 0 And BudgetMachineHours <> 0 Then
                'dBudgetOEE = (((OEEBudgetGoodPartCount / OEEBudgetTotalPartCount) * dOEEBudgetUtilization)) * ((dOEEBudgetAvailableHours - OEEBudgetDownHours) / dOEEBudgetAvailableHours)
                '03/30/2011
                dBudgetOEE = (OEEBudgetGoodPartCount / OEEBudgetTotalPartCount) * dOEEBudgetUtilization * (BudgetMachineHourStandard / BudgetMachineHours)
            End If

            txtBudgetOEE.Text = Format(dBudgetOEE * 100, "#0.0")

            If DeptID > 0 Then
                dTempBudgetDLHours = BudgetManWorkedHours + BudgetDowntimeManHours
            Else
                dTempBudgetDLHours = BudgetDLHours
            End If

            txtBudgetDLHours.Text = Format(dTempBudgetDLHours, "0.#")

            If TotalBudgetProductionDollar <> 0 Then
                txtBudgetScrapPercent.Text = Format(((TotalBudgetSpecificScrapDollar + TotalBudgetMiscScrapDollar) / TotalBudgetProductionDollar) * 100, "###0.0")
                txtBudgetRawWipScrapPercent.Text = Format(((TotalBudgetRawWipScrapDollar) / TotalBudgetProductionDollar) * 100, "###0.0")
            End If

            If dTempBudgetDLHours <> 0 Then
                dBudgetLaborProductivity = BudgetEarnedDLHours / dTempBudgetDLHours

                lblBudgetDLHoursNetVariance.Text = Format(BudgetEarnedDLHours - dTempBudgetDLHours, "#,###0")
                lblBudgetLaborProductivity.Text = Format(dBudgetLaborProductivity * 100, "###0.0")
            End If

            If BudgetTeamLeaderFactorCount <> 0 Then
                lblBudgetTeamMemberLeaderRatio.Text = Format((BudgetTeamMemberFactorCount / BudgetTeamLeaderFactorCount), "#.#") & " to 1"
            End If

            If ActualTeamLeaderFactorCount <> 0 Then
                lblActualTeamMemberLeaderRatio.Text = Format((ActualTeamMemberFactorCount / ActualTeamLeaderFactorCount), "#.#") & " to 1"
            End If

            If TotalActualProductionDollar <> 0 Then
                txtActualScrapPercent.Text = Format(((TotalActualSpecificScrapDollar + TotalActualIndirectScrapDollar + TotalActualMiscScrapDollar) / TotalActualProductionDollar) * 100, "#0.0")
                txtActualRawWipScrapPercent.Text = Format(((TotalActualRawWipScrapDollar) / TotalActualProductionDollar) * 100, "###0.0")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub CalculateTeamMembers()

        Try
            'txtBudgetDirectPerm.Text = ""
            'txtFlexDirectPerm.Text = ""
            'txtActualDirectPerm.Text = ""

            lblBWDirectPerm.Text = ""

            'txtBudgetDirectTemp.Text = ""
            lblBudgetDirectLaborTotal.Text = ""

            'txtFlexDirectTemp.Text = ""
            lblFlexDirectLaborTotal.Text = ""

            'txtActualDirectTemp.Text = ""
            lblActualDirectLaborTotal.Text = ""

            lblBWDirectTemp.Text = ""
            lblBWDirectLaborTotal.Text = ""

            'txtBudgetIndirectPerm.Text = ""
            'txtFlexIndirectPerm.Text = ""
            'txtActualIndirectPerm.Text = ""

            'txtBudgetAllocatedSupportIndirectPerm.Text = ""
            'txtFlexAllocatedSupportIndirectPerm.Text = ""
            'txtActualAllocatedSupportIndirectPerm.Text = ""

            'txtBudgetIndirectTemp.Text = ""
            'txtFlexIndirectTemp.Text = ""
            'txtActualIndirectTemp.Text = ""

            'txtBudgetAllocatedSupportIndirectTemp.Text = ""
            'txtFlexAllocatedSupportIndirectTemp.Text = ""
            'txtActualAllocatedSupportIndirectTemp.Text = ""

            lblBWIndirectPerm.Text = ""
            lblBWAllocatedSupportIndirectPerm.Text = ""
            lblBWIndirectTemp.Text = ""
            lblBWAllocatedSupportIndirectTemp.Text = ""
            lblBudgetIndirectLaborTotal.Text = ""
            lblFlexIndirectLaborTotal.Text = ""
            lblActualIndirectLaborTotal.Text = ""
            lblBWIndirectLaborTotal.Text = ""

            'txtBudgetOfficeHourlyPerm.Text = ""
            'txtFlexOfficeHourlyPerm.Text = ""
            'txtActualOfficeHourlyPerm.Text = ""

            'txtBudgetAllocatedSupportOfficeHourlyPerm.Text = ""
            'txtFlexAllocatedSupportOfficeHourlyPerm.Text = ""
            'txtActualAllocatedSupportOfficeHourlyPerm.Text = ""

            'txtBudgetAllocatedSupportOfficeHourlyTemp.Text = ""
            'txtFlexAllocatedSupportOfficeHourlyTemp.Text = ""
            'txtActualAllocatedSupportOfficeHourlyTemp.Text = ""

            'txtBudgetOfficeHourlyTemp.Text = ""
            'txtFlexOfficeHourlyTemp.Text = ""
            'txtActualOfficeHourlyTemp.Text = ""

            lblBudgetOfficeHourlyTotal.Text = ""
            lblFlexOfficeHourlyTotal.Text = ""
            lblActualOfficeHourlyTotal.Text = ""
            lblBWOfficeHourlyPerm.Text = ""
            lblBWAllocatedSupportOfficeHourlyPerm.Text = ""
            lblBWOfficeHourlyTemp.Text = ""
            lblBWAllocatedSupportOfficeHourlyTemp.Text = ""
            lblBWOfficeHourlyTotal.Text = ""

            'txtBudgetSalaryPerm.Text = ""
            'txtFlexSalaryPerm.Text = ""
            'txtActualSalaryPerm.Text = ""

            'txtBudgetSalaryTemp.Text = ""
            'txtFlexSalaryTemp.Text = ""
            'txtActualSalaryTemp.Text = ""

            'txtBudgetAllocatedSupportSalaryPerm.Text = ""
            'txtFlexAllocatedSupportSalaryPerm.Text = ""
            'txtActualAllocatedSupportSalaryPerm.Text = ""

            'txtBudgetAllocatedSupportSalaryTemp.Text = ""
            'txtFlexAllocatedSupportSalaryTemp.Text = ""
            'txtActualAllocatedSupportSalaryTemp.Text = ""

            lblBudgetSalaryTotal.Text = ""
            lblFlexSalaryTotal.Text = ""
            lblActualSalaryTotal.Text = ""
            lblBWSalaryPerm.Text = ""
            lblBWAllocatedSupportSalaryPerm.Text = ""
            lblBWSalaryTemp.Text = ""
            lblBWAllocatedSupportSalaryTemp.Text = ""
            lblBWSalaryTotal.Text = ""
            lblBudgetTotalTeamMembers.Text = ""
            lblFlexTotalTeamMembers.Text = ""
            lblActualTotalTeamMembers.Text = ""
            lblBWTotalTeamMembers.Text = ""

            Dim dBudgetDirectPerm As Double = 0
            If txtBudgetDirectPerm.Text.Trim <> "" Then
                dBudgetDirectPerm = CType(txtBudgetDirectPerm.Text.Trim, Double)
            End If

            Dim dFlexDirectPerm As Double = 0
            If txtFlexDirectPerm.Text.Trim <> "" Then
                dFlexDirectPerm = CType(txtFlexDirectPerm.Text.Trim, Double)
            End If

            Dim dActualDirectPerm As Double = 0
            If txtActualDirectPerm.Text.Trim <> "" Then
                dActualDirectPerm = CType(txtActualDirectPerm.Text.Trim, Double)
            End If

            lblBWDirectPerm.Text = dFlexDirectPerm - dActualDirectPerm

            Dim dBudgetDirectTemp As Double = 0
            If txtBudgetDirectTemp.Text.Trim <> "" Then
                dBudgetDirectTemp = CType(txtBudgetDirectTemp.Text.Trim, Double)
            End If

            lblBudgetDirectLaborTotal.Text = dBudgetDirectPerm + dBudgetDirectTemp

            Dim dFlexDirectTemp As Double = 0
            If txtFlexDirectTemp.Text.Trim <> "" Then
                dFlexDirectTemp = CType(txtFlexDirectTemp.Text.Trim, Double)
            End If

            lblFlexDirectLaborTotal.Text = dFlexDirectPerm + dFlexDirectTemp

            Dim dActualDirectTemp As Double = 0
            If txtActualDirectTemp.Text.Trim <> "" Then
                dActualDirectTemp = CType(txtActualDirectTemp.Text.Trim, Double)
            End If

            lblActualDirectLaborTotal.Text = dActualDirectPerm + dActualDirectTemp
            lblBWDirectTemp.Text = dFlexDirectTemp - dActualDirectTemp
            lblBWDirectLaborTotal.Text = (dFlexDirectPerm + dFlexDirectTemp) - (dActualDirectPerm + dActualDirectTemp)

            Dim dBudgetIndirectPerm As Double = 0
            If txtBudgetIndirectPerm.Text.Trim <> "" Then
                dBudgetIndirectPerm = CType(txtBudgetIndirectPerm.Text.Trim, Double)
            End If

            Dim dFlexIndirectPerm As Double = 0
            If txtFlexIndirectPerm.Text.Trim <> "" Then
                dFlexIndirectPerm = CType(txtFlexIndirectPerm.Text.Trim, Double)
            End If

            Dim dActualIndirectPerm As Double = 0
            If txtActualIndirectPerm.Text.Trim <> "" Then
                dActualIndirectPerm = CType(txtActualIndirectPerm.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportIndirectPerm As Double = 0
            If txtBudgetAllocatedSupportIndirectPerm.Text.Trim <> "" Then
                dBudgetAllocatedSupportIndirectPerm = CType(txtBudgetAllocatedSupportIndirectPerm.Text.Trim, Double)
            End If

            Dim dFlexAllocatedSupportIndirectPerm As Double = 0
            If txtFlexAllocatedSupportIndirectPerm.Text.Trim <> "" Then
                dFlexAllocatedSupportIndirectPerm = CType(txtFlexAllocatedSupportIndirectPerm.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportIndirectPerm As Double = 0
            If txtActualAllocatedSupportIndirectPerm.Text.Trim <> "" Then
                dActualAllocatedSupportIndirectPerm = CType(txtActualAllocatedSupportIndirectPerm.Text.Trim, Double)
            End If

            Dim dBudgetIndirectTemp As Double = 0
            If txtBudgetIndirectTemp.Text.Trim <> "" Then
                dBudgetIndirectTemp = CType(txtBudgetIndirectTemp.Text.Trim, Double)
            End If

            Dim dFlexIndirectTemp As Double = 0
            If txtFlexIndirectTemp.Text.Trim <> "" Then
                dFlexIndirectTemp = CType(txtFlexIndirectTemp.Text.Trim, Double)
            End If

            Dim dActualIndirectTemp As Double = 0
            If txtActualIndirectTemp.Text.Trim <> "" Then
                dActualIndirectTemp = CType(txtActualIndirectTemp.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportIndirectTemp As Double = 0
            If txtBudgetAllocatedSupportIndirectTemp.Text.Trim <> "" Then
                dBudgetAllocatedSupportIndirectTemp = CType(txtBudgetAllocatedSupportIndirectTemp.Text.Trim, Double)
            End If

            Dim dFlexAllocatedSupportIndirectTemp As Double = 0
            If txtFlexAllocatedSupportIndirectTemp.Text.Trim <> "" Then
                dFlexAllocatedSupportIndirectTemp = CType(txtFlexAllocatedSupportIndirectTemp.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportIndirectTemp As Double = 0
            If txtActualAllocatedSupportIndirectTemp.Text.Trim <> "" Then
                dActualAllocatedSupportIndirectTemp = CType(txtActualAllocatedSupportIndirectTemp.Text.Trim, Double)
            End If

            lblBWIndirectPerm.Text = dFlexIndirectPerm - dActualIndirectPerm
            lblBWAllocatedSupportIndirectPerm.Text = dFlexAllocatedSupportIndirectPerm - dActualAllocatedSupportIndirectPerm
            lblBWIndirectTemp.Text = dFlexIndirectTemp - dActualIndirectTemp
            lblBWAllocatedSupportIndirectTemp.Text = dFlexAllocatedSupportIndirectTemp - dActualAllocatedSupportIndirectTemp

            lblBudgetIndirectLaborTotal.Text = dBudgetIndirectPerm + dBudgetIndirectTemp + dBudgetAllocatedSupportIndirectPerm + dBudgetAllocatedSupportIndirectTemp
            lblFlexIndirectLaborTotal.Text = dFlexIndirectPerm + dFlexIndirectTemp + dFlexAllocatedSupportIndirectPerm + dFlexAllocatedSupportIndirectTemp
            lblActualIndirectLaborTotal.Text = dActualIndirectPerm + dActualIndirectTemp + dActualAllocatedSupportIndirectPerm + dActualAllocatedSupportIndirectTemp

            lblBWIndirectLaborTotal.Text = (dFlexIndirectPerm + dFlexIndirectTemp + dFlexAllocatedSupportIndirectPerm + dFlexAllocatedSupportIndirectTemp) - (dActualIndirectPerm + dActualIndirectTemp + dActualAllocatedSupportIndirectPerm + dActualAllocatedSupportIndirectTemp)

            Dim dBudgetOfficeHourlyPerm As Double = 0
            If txtBudgetOfficeHourlyPerm.Text.Trim <> "" Then
                dBudgetOfficeHourlyPerm = CType(txtBudgetOfficeHourlyPerm.Text.Trim, Double)
            End If

            Dim dFlexOfficeHourlyPerm As Double = 0
            If txtFlexOfficeHourlyPerm.Text.Trim <> "" Then
                dFlexOfficeHourlyPerm = CType(txtFlexOfficeHourlyPerm.Text.Trim, Double)
            End If

            Dim dActualOfficeHourlyPerm As Double = 0
            If txtActualOfficeHourlyPerm.Text.Trim <> "" Then
                dActualOfficeHourlyPerm = CType(txtActualOfficeHourlyPerm.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportOfficeHourlyPerm As Double = 0
            If txtBudgetAllocatedSupportOfficeHourlyPerm.Text.Trim <> "" Then
                dBudgetAllocatedSupportOfficeHourlyPerm = CType(txtBudgetAllocatedSupportOfficeHourlyPerm.Text.Trim, Integer)
            End If

            Dim dFlexAllocatedSupportOfficeHourlyPerm As Double = 0
            If txtFlexAllocatedSupportOfficeHourlyPerm.Text.Trim <> "" Then
                dFlexAllocatedSupportOfficeHourlyPerm = CType(txtFlexAllocatedSupportOfficeHourlyPerm.Text.Trim, Integer)
            End If

            Dim dActualAllocatedSupportOfficeHourlyPerm As Double = 0
            If txtActualAllocatedSupportOfficeHourlyPerm.Text.Trim <> "" Then
                dActualAllocatedSupportOfficeHourlyPerm = CType(txtActualAllocatedSupportOfficeHourlyPerm.Text.Trim, Integer)
            End If

            Dim dBudgetAllocatedSupportOfficeHourlyTemp As Double = 0
            If txtBudgetAllocatedSupportOfficeHourlyTemp.Text.Trim <> "" Then
                dBudgetAllocatedSupportOfficeHourlyTemp = CType(txtBudgetAllocatedSupportOfficeHourlyTemp.Text.Trim, Integer)
            End If

            Dim dFlexAllocatedSupportOfficeHourlyTemp As Double = 0
            If txtFlexAllocatedSupportOfficeHourlyTemp.Text.Trim <> "" Then
                dFlexAllocatedSupportOfficeHourlyTemp = CType(txtFlexAllocatedSupportOfficeHourlyTemp.Text.Trim, Integer)
            End If

            Dim dActualAllocatedSupportOfficeHourlyTemp As Double = 0
            If txtActualAllocatedSupportOfficeHourlyTemp.Text.Trim <> "" Then
                dActualAllocatedSupportOfficeHourlyTemp = CType(txtActualAllocatedSupportOfficeHourlyTemp.Text.Trim, Integer)
            End If

            Dim dBudgetOfficeHourlyTemp As Double = 0
            If txtBudgetOfficeHourlyTemp.Text.Trim <> "" Then
                dBudgetOfficeHourlyTemp = CType(txtBudgetOfficeHourlyTemp.Text.Trim, Double)
            End If

            Dim dFlexOfficeHourlyTemp As Double = 0
            If txtFlexOfficeHourlyTemp.Text.Trim <> "" Then
                dFlexOfficeHourlyTemp = CType(txtFlexOfficeHourlyTemp.Text.Trim, Double)
            End If

            Dim dActualOfficeHourlyTemp As Double = 0
            If txtActualOfficeHourlyTemp.Text.Trim <> "" Then
                dActualOfficeHourlyTemp = CType(txtActualOfficeHourlyTemp.Text.Trim, Double)
            End If

            lblBudgetOfficeHourlyTotal.Text = dBudgetOfficeHourlyPerm + dBudgetOfficeHourlyTemp + dBudgetAllocatedSupportOfficeHourlyPerm + dBudgetAllocatedSupportOfficeHourlyTemp
            lblFlexOfficeHourlyTotal.Text = dFlexOfficeHourlyPerm + dFlexOfficeHourlyTemp + dFlexAllocatedSupportOfficeHourlyPerm + dFlexAllocatedSupportOfficeHourlyTemp
            lblActualOfficeHourlyTotal.Text = dActualOfficeHourlyPerm + dActualOfficeHourlyTemp + dActualAllocatedSupportOfficeHourlyPerm + dActualAllocatedSupportOfficeHourlyTemp

            lblBWOfficeHourlyPerm.Text = dFlexOfficeHourlyPerm - dActualOfficeHourlyPerm
            lblBWAllocatedSupportOfficeHourlyPerm.Text = dFlexAllocatedSupportOfficeHourlyPerm - dActualAllocatedSupportOfficeHourlyPerm

            lblBWOfficeHourlyTemp.Text = dFlexOfficeHourlyTemp - dActualOfficeHourlyTemp
            lblBWAllocatedSupportOfficeHourlyTemp.Text = dFlexAllocatedSupportOfficeHourlyTemp - dActualAllocatedSupportOfficeHourlyTemp

            lblBWOfficeHourlyTotal.Text = (dFlexOfficeHourlyPerm + dFlexOfficeHourlyTemp + dFlexAllocatedSupportOfficeHourlyPerm + dFlexAllocatedSupportOfficeHourlyTemp) - (dActualOfficeHourlyPerm + dActualOfficeHourlyTemp + dActualAllocatedSupportOfficeHourlyPerm + dActualAllocatedSupportOfficeHourlyTemp)

            Dim dBudgetSalaryPerm As Double = 0
            If txtBudgetSalaryPerm.Text.Trim <> "" Then
                dBudgetSalaryPerm = CType(txtBudgetSalaryPerm.Text.Trim, Double)
            End If

            Dim dFlexSalaryPerm As Double = 0
            If txtFlexSalaryPerm.Text.Trim <> "" Then
                dFlexSalaryPerm = CType(txtFlexSalaryPerm.Text.Trim, Double)
            End If

            Dim dActualSalaryPerm As Double = 0
            If txtActualSalaryPerm.Text.Trim <> "" Then
                dActualSalaryPerm = CType(txtActualSalaryPerm.Text.Trim, Double)
            End If

            Dim dBudgetSalaryTemp As Double = 0
            If txtBudgetSalaryTemp.Text.Trim <> "" Then
                dBudgetSalaryTemp = CType(txtBudgetSalaryTemp.Text.Trim, Double)
            End If

            Dim dFlexSalaryTemp As Double = 0
            If txtFlexSalaryTemp.Text.Trim <> "" Then
                dFlexSalaryTemp = CType(txtFlexSalaryTemp.Text.Trim, Double)
            End If

            Dim dActualSalaryTemp As Double = 0
            If txtActualSalaryTemp.Text.Trim <> "" Then
                dActualSalaryTemp = CType(txtActualSalaryTemp.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportSalaryPerm As Double = 0
            If txtBudgetAllocatedSupportSalaryPerm.Text.Trim <> "" Then
                dBudgetAllocatedSupportSalaryPerm = CType(txtBudgetAllocatedSupportSalaryPerm.Text.Trim, Double)
            End If

            Dim dFlexAllocatedSupportSalaryPerm As Double = 0
            If txtFlexAllocatedSupportSalaryPerm.Text.Trim <> "" Then
                dFlexAllocatedSupportSalaryPerm = CType(txtFlexAllocatedSupportSalaryPerm.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportSalaryPerm As Double = 0
            If txtActualAllocatedSupportSalaryPerm.Text.Trim <> "" Then
                dActualAllocatedSupportSalaryPerm = CType(txtActualAllocatedSupportSalaryPerm.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportSalaryTemp As Double = 0
            If txtBudgetAllocatedSupportSalaryTemp.Text.Trim <> "" Then
                dBudgetAllocatedSupportSalaryTemp = CType(txtBudgetAllocatedSupportSalaryTemp.Text.Trim, Double)
            End If

            Dim dFlexAllocatedSupportSalaryTemp As Double = 0
            If txtFlexAllocatedSupportSalaryTemp.Text.Trim <> "" Then
                dFlexAllocatedSupportSalaryTemp = CType(txtFlexAllocatedSupportSalaryTemp.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportSalaryTemp As Double = 0
            If txtActualAllocatedSupportSalaryTemp.Text.Trim <> "" Then
                dActualAllocatedSupportSalaryTemp = CType(txtActualAllocatedSupportSalaryTemp.Text.Trim, Double)
            End If

            lblBudgetSalaryTotal.Text = dBudgetSalaryPerm + dBudgetSalaryTemp + dBudgetAllocatedSupportSalaryPerm + dBudgetAllocatedSupportSalaryTemp
            lblFlexSalaryTotal.Text = dFlexSalaryPerm + dFlexSalaryTemp + dFlexAllocatedSupportSalaryPerm + dFlexAllocatedSupportSalaryTemp
            lblActualSalaryTotal.Text = dActualSalaryPerm + dActualSalaryTemp + dActualAllocatedSupportSalaryPerm + dActualAllocatedSupportSalaryTemp

            lblBWSalaryPerm.Text = dFlexSalaryPerm - dActualSalaryPerm
            lblBWAllocatedSupportSalaryPerm.Text = dFlexAllocatedSupportSalaryPerm - dActualAllocatedSupportSalaryPerm

            lblBWSalaryTemp.Text = dFlexSalaryTemp - dActualSalaryTemp
            lblBWAllocatedSupportSalaryTemp.Text = dFlexAllocatedSupportSalaryTemp - dActualAllocatedSupportSalaryTemp

            lblBWSalaryTotal.Text = (dFlexSalaryPerm + dFlexSalaryTemp + dFlexAllocatedSupportSalaryPerm + dFlexAllocatedSupportSalaryTemp) - (dActualSalaryPerm + dActualSalaryTemp + dActualAllocatedSupportSalaryPerm + dActualAllocatedSupportSalaryTemp)

            lblBudgetTotalTeamMembers.Text = dBudgetDirectPerm + dBudgetDirectTemp + dBudgetIndirectPerm + dBudgetIndirectTemp + dBudgetAllocatedSupportIndirectPerm + dBudgetAllocatedSupportIndirectTemp + dBudgetOfficeHourlyPerm + dBudgetOfficeHourlyTemp + dBudgetAllocatedSupportOfficeHourlyPerm + dBudgetAllocatedSupportOfficeHourlyTemp + dBudgetSalaryPerm + dBudgetSalaryTemp + dBudgetAllocatedSupportSalaryPerm + dBudgetAllocatedSupportSalaryTemp
            lblFlexTotalTeamMembers.Text = dFlexDirectPerm + dFlexDirectTemp + dFlexIndirectPerm + dFlexIndirectTemp + dFlexAllocatedSupportIndirectPerm + dFlexAllocatedSupportIndirectTemp + dFlexOfficeHourlyPerm + dFlexOfficeHourlyTemp + dFlexAllocatedSupportOfficeHourlyPerm + dFlexAllocatedSupportOfficeHourlyTemp + dFlexSalaryPerm + dFlexSalaryTemp + dFlexAllocatedSupportSalaryPerm + dFlexAllocatedSupportSalaryTemp
            lblActualTotalTeamMembers.Text = dActualDirectPerm + dActualDirectTemp + dActualIndirectPerm + dActualIndirectTemp + dActualAllocatedSupportIndirectPerm + dActualAllocatedSupportIndirectTemp + dActualOfficeHourlyPerm + dActualOfficeHourlyTemp + dActualAllocatedSupportOfficeHourlyPerm + dActualAllocatedSupportOfficeHourlyTemp + dActualSalaryPerm + dActualSalaryTemp + dActualAllocatedSupportSalaryPerm + dActualAllocatedSupportSalaryTemp

            lblBWTotalTeamMembers.Text = (dFlexDirectPerm + dFlexDirectTemp + dFlexIndirectPerm + dFlexIndirectTemp + dFlexAllocatedSupportIndirectPerm + dFlexAllocatedSupportIndirectTemp + dFlexOfficeHourlyPerm + dFlexOfficeHourlyTemp + dFlexAllocatedSupportOfficeHourlyPerm + dFlexAllocatedSupportOfficeHourlyTemp + dFlexSalaryPerm + dFlexSalaryTemp + dFlexAllocatedSupportSalaryPerm + dFlexAllocatedSupportSalaryTemp) - (dActualDirectPerm + dActualDirectTemp + dActualIndirectPerm + dActualIndirectTemp + dActualAllocatedSupportIndirectPerm + dActualAllocatedSupportIndirectTemp + dActualOfficeHourlyPerm + dActualOfficeHourlyTemp + dActualAllocatedSupportOfficeHourlyPerm + dActualAllocatedSupportOfficeHourlyTemp + dActualSalaryPerm + dActualSalaryTemp + dActualAllocatedSupportSalaryPerm + dActualAllocatedSupportSalaryTemp)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub SetDateRange(ByVal MonthID As Integer, ByVal YearID As Integer)

        Try

            ViewState("StartDate") = YearID.ToString & MonthID.ToString.PadLeft(2, "0") & "01"
            lblStartDate.Text = ViewState("StartDate")

            Select Case MonthID
                Case 1, 3, 5, 7, 8, 10, 12
                    ViewState("EndDate") = YearID.ToString & MonthID.ToString.PadLeft(2, "0") & "31"
                Case 2
                    ViewState("EndDate") = YearID.ToString & MonthID.ToString.PadLeft(2, "0") & "28"
                Case 4, 6, 9, 11
                    ViewState("EndDate") = YearID.ToString & MonthID.ToString.PadLeft(2, "0") & "30"
            End Select

            lblEndDate.Text = ViewState("EndDate")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindHeaderData()

        Try

            Dim ds As DataSet
            'Dim dsSubscription As DataSet

            Dim iRowCounter As Integer = 0

            'get header info
            ds = PSRModule.GetManufacturingMetricHeader(ViewState("ReportID"))

            If commonFunctions.CheckDataSet(ds) = True Then

                If ds.Tables(0).Rows(0).Item("MonthID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MonthID") > 0 Then
                        ddMonth.SelectedValue = ds.Tables(0).Rows(0).Item("MonthID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("YearID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("YearID") > 0 Then
                        ddYear.SelectedValue = ds.Tables(0).Rows(0).Item("YearID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("UGNFacility") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("UGNFacility") <> "" Then
                        ddUGNFacility.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility")
                        ViewState("UGNFacility") = ddUGNFacility.SelectedValue
                        BindDepartment(ViewState("UGNFacility"))
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("StatusID") > 0 Then
                        ddStatus.SelectedValue = ds.Tables(0).Rows(0).Item("StatusID")
                        ViewState("StatusID") = ds.Tables(0).Rows(0).Item("StatusID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CreatedByTMID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CreatedByTMID") > 0 Then
                        ddCreatedByTMID.SelectedValue = ds.Tables(0).Rows(0).Item("CreatedByTMID")
                    End If
                End If

                lblUpdatedOn.Text = ds.Tables(0).Rows(0).Item("UpdatedOn").ToString

                If ddMonth.SelectedIndex > 0 And ddYear.SelectedIndex > 0 Then
                    SetDateRange(ddMonth.SelectedValue, ddYear.SelectedValue)
                End If

                EnableControls()

                If ddCreatedByTMID.SelectedIndex = 0 And ViewState("isAdmin") = True Then
                    ddCreatedByTMID.SelectedValue = ViewState("TeamMemberID")
                    ViewState("NewTeamMemberID") = True
                End If

                Dim mpTextBox As Label
                mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
                If Not mpTextBox Is Nothing Then
                    'mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Plant Specific Reports </b> > <a href='Manufacturing_Metric_List.aspx'><b> Monthly Manufacturing Monthly Metric List </b></a> > Manufacturing Metric Monthly Data > <a href='Manufacturing_Metric_History.aspx?ReportID=" & ViewState("ReportID") & "&MonthName=" & ddMonth.SelectedItem.Text & "&YearID=" & ddYear.SelectedValue & "&UGNFacilityName=" & ddUGNFacility.SelectedItem.Text & "  '> Monthly Manufacturing Monthly Data History </a> "
                    mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Plant Specific Reports </b> > <a href='Manufacturing_Metric_List.aspx'><b> Monthly Manufacturing Monthly Metric List </b></a> > Manufacturing Metric Monthly Data > <a href='Manufacturing_Metric_History.aspx?ReportID=" & ViewState("ReportID") & "  '> Monthly Manufacturing Monthly Data History </a> "
                    mpTextBox.Visible = True
                End If

                'Else 'no report record yet

                '    'find default UGNFacility 
                '    dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(20, "")
                '    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                '        For iRowCounter = 0 To dsSubscription.Tables(0).Rows.Count - 1
                '            If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value Then
                '                If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") > 0 Then
                '                    If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") = ViewState("TeamMemberID") Then
                '                        ddUGNFacility.SelectedValue = dsSubscription.Tables(0).Rows(iRowCounter).Item("UGNFacility").ToString
                '                        ViewState("UGNFacility") = ddUGNFacility.SelectedValue
                '                        BindDepartment(ViewState("UGNFacility"))
                '                    End If '= ViewState("TeamMemberID")
                '                End If 'Item("TMID") > 0 
                '            End If 'System.DBNull.Value
                '        Next
                '    End If 'not empty

                '    ddCreatedByTMID.SelectedValue = ViewState("TeamMemberID")

                '    If Today.Month - 1 <= 0 Then
                '        ddMonth.SelectedValue = 12
                '        ddYear.SelectedValue = Today.Year - 1
                '    Else
                '        ddMonth.SelectedValue = Today.Month - 1
                '        ddYear.SelectedValue = Today.Year
                '    End If

                '    ddStatus.SelectedValue = 1
                '    SetDateRange(ddMonth.SelectedValue, ddYear.SelectedValue)

                '    'check if a report already exists for same month, year, and UGN Facility
                '    If ddMonth.SelectedIndex > 0 And ddYear.SelectedIndex > 0 And ViewState("UGNFacility") <> "" Then
                '        ds = PSRModule.GetManufacturingMetricSearch(0, ddMonth.SelectedValue, ddYear.SelectedValue, ViewState("UGNFacility"), 0, 0)

                '        If commonFunctions.CheckDataSet(ds) = True Then
                '            lblMessage.Text = "Error: A report already exists for this month, year, and UGN Facility. A second report cannot be created."
                '            DisableAllControls()
                '            ddMonth.Enabled = ViewState("isAdmin")
                '            ddYear.Enabled = ViewState("isAdmin")
                '        Else
                '            EnableControls()
                '        End If 'not empty
                '    End If 'values selected

            End If 'ds has data

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindDepartment(ByVal UGNFacility As String)

        Try
            Dim dsDepartmentList As DataSet
            Dim dsMM As DataSet

            Dim iRowCounter As Integer = 0
            Dim iDeptID As Integer = 0
            Dim iSelectedDeptID As Integer = 0

            Dim strDepartment As String = ""

            If ddDepartment.SelectedIndex > 0 Then
                iSelectedDeptID = ddDepartment.SelectedValue
            End If

            'Dim liListItem As New System.Web.UI.WebControls.ListItem
            Dim liListItem As System.Web.UI.WebControls.ListItem

            dsDepartmentList = PSRModule.GetManufacturingMetricDepartment(UGNFacility)
            If commonFunctions.CheckDataSet(dsDepartmentList) = True Then

                ddDepartment.Items.Clear()
                For iRowCounter = 0 To dsDepartmentList.Tables(0).Rows.Count - 1

                    If dsDepartmentList.Tables(0).Rows(iRowCounter).Item("CDEPT") IsNot System.DBNull.Value Then
                        iDeptID = dsDepartmentList.Tables(0).Rows(iRowCounter).Item("CDEPT")
                        strDepartment = dsDepartmentList.Tables(0).Rows(iRowCounter).Item("ddDepartmentDesc").ToString

                        dsMM = PSRModule.GetManufacturingMetricDetailByDept(ViewState("ReportID"), iDeptID)

                        liListItem = New System.Web.UI.WebControls.ListItem

                        liListItem.Text = strDepartment
                        liListItem.Value = iDeptID

                        liListItem.Attributes.Add("style", "background-color: Gray") 'Make the back color gray
                        If commonFunctions.CheckDataSet(dsMM) = True Then
                            If dsMM.Tables(0).Rows(0).Item("Obsolete") IsNot System.DBNull.Value Then
                                If dsMM.Tables(0).Rows(0).Item("Obsolete") = False Then
                                    liListItem.Attributes.Add("style", "background-color: White") 'Make the back color White
                                End If ' If dsMM.Tables(0).Rows(0).Item("Obsolete") = False 
                            End If ' If dsMM.Tables(0).Rows(0).Item("Obsolete") IsNot System.DBNull.Value Then
                        End If

                        ddDepartment.Items.Add(liListItem)
                    End If ' dsDepartmentList.Tables(0).Rows(iRowCounter).Item("CDEPT") IsNot System.DBNull.Value
                Next
                ddDepartment.Items.Insert(0, "Totals")

                If iSelectedDeptID > 0 Then
                    ddDepartment.SelectedValue = iSelectedDeptID
                End If
            Else
                ddDepartment.Items.Clear()
                ddDepartment.Items.Insert(0, "N/A")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub DisableAllControls()

        ddMonth.Enabled = False
        ddUGNFacility.Enabled = False
        ddDepartment.Enabled = False
        ddYear.Enabled = False

        'btnBPCSRefresh.Visible = False
        btnCalculate.Visible = False
        'btnCancelVoid.Visible = False
        'btnLoadBPCS.Visible = False
        btnSave.Visible = False
        btnSaveBottom.Visible = False
        btnNofityInternal.Visible = False
        btnNotifyFinal.Visible = False
        btnPreview.Visible = False
        btnPreviewBottom.Visible = False
        'btnVoid.Visible = False

        'lblLoadBPCSWarning.Visible = False

        txtBudgetOEE.Enabled = False
        txtActualOEE.Enabled = False

        txtBudgetEarnedDLHours.Enabled = False
        txtActualEarnedDLHours.Enabled = False

        txtBudgetDLHours.Enabled = False
        txtActualDLHours.Enabled = False

        txtBudgetDirectOTHours.Enabled = False
        txtActualDirectOTHours.Enabled = False

        txtBudgetIndirectOTHours.Enabled = False
        txtActualIndirectOTHours.Enabled = False

        txtBudgetScrapPercent.Enabled = False
        txtActualScrapPercent.Enabled = False

        txtBudgetTeamMemberContainmentCount.Enabled = False
        txtActualTeamMemberContainmentCount.Enabled = False

        txtBudgetPartContainmentCount.Enabled = False
        txtActualPartContainmentCount.Enabled = False

        txtBudgetOffStandardDirectCount.Enabled = False
        txtActualOffStandardDirectCount.Enabled = False

        txtBudgetOffStandardIndirectCount.Enabled = False
        txtActualOffStandardIndirectCount.Enabled = False

        cbBudgetStandardizedCellWork.Enabled = False
        cbActualStandardizedCellWork.Enabled = False

        txtBudgetTeamMemberFactorCount.Enabled = False
        txtBudgetTeamLeaderFactorCount.Enabled = False

        txtActualTeamMemberFactorCount.Enabled = False
        txtActualTeamLeaderFactorCount.Enabled = False

        txtBudgetCapacityUtilization.Enabled = False
        txtActualCapacityUtilization.Enabled = False

        txtOEEBudgetGoodPartCount.Enabled = False
        txtOEEActualGoodPartCount.Enabled = False

        txtOEEBudgetScrapPartCount.Enabled = False
        txtOEEActualScrapPartCount.Enabled = False

        txtOEEBudgetTotalPartCount.Enabled = False
        txtOEEActualTotalPartCount.Enabled = False

        txtOEEBudgetUtilization.Enabled = False
        txtOEEActualUtilization.Enabled = False

        txtOEEBudgetAvailableHours.Enabled = False
        txtOEEActualAvailableHours.Enabled = False

        txtOEEBudgetDownHours.Enabled = False
        txtOEEActualDownHours.Enabled = False

        txtMonthlyShippingDays.Enabled = False
        txtHoursPerShift.Enabled = False

        txtBudgetDowntimeHours.Enabled = False
        txtActualDowntimeHours.Enabled = False

        txtBudgetMachineWorkedHours.Enabled = False
        txtActualMachineWorkedHours.Enabled = False

        txtBudgetMachineAvailableHours.Enabled = False
        txtActualMachineAvailableHours.Enabled = False

        txtBudgetMachineStandardHours.Enabled = False
        txtActualMachineStandardHours.Enabled = False

        txtBudgetManWorkedHours.Enabled = False
        txtActualManWorkedHours.Enabled = False

        txtBudgetDowntimeManHours.Enabled = False
        txtActualDowntimeManHours.Enabled = False

        txtTotalBudgetSpecificScrapDollar.Enabled = False
        txtTotalActualSpecificScrapDollar.Enabled = False

        txtTotalBudgetProductionDollar.Enabled = False
        txtTotalActualProductionDollar.Enabled = False

        txtTotalBudgetMiscScrapDollar.Enabled = False
        txtTotalActualMiscScrapDollar.Enabled = False

        txtTotalBudgetProductionDollar.Enabled = False
        txtTotalActualProductionDollar.Enabled = False

        txtTotalActualIndirectScrapDollar.Enabled = False

        txtTotalBudgetRawWipScrapDollar.Enabled = False
        txtTotalActualRawWipScrapDollar.Enabled = False

        txtBudgetDirectPerm.Enabled = False
        txtFlexDirectPerm.Enabled = False
        txtActualDirectPerm.Enabled = False
        txtBudgetDirectTemp.Enabled = False
        txtFlexDirectTemp.Enabled = False
        txtActualDirectTemp.Enabled = False
        txtBudgetIndirectPerm.Enabled = False
        txtFlexIndirectPerm.Enabled = False
        txtActualIndirectPerm.Enabled = False
        txtBudgetIndirectTemp.Enabled = False
        txtFlexIndirectTemp.Enabled = False
        txtActualIndirectTemp.Enabled = False
        txtBudgetOfficeHourlyPerm.Enabled = False
        txtFlexOfficeHourlyPerm.Enabled = False
        txtActualOfficeHourlyPerm.Enabled = False
        txtBudgetOfficeHourlyTemp.Enabled = False
        txtFlexOfficeHourlyTemp.Enabled = False
        txtActualOfficeHourlyTemp.Enabled = False
        txtBudgetSalaryPerm.Enabled = False
        txtFlexSalaryPerm.Enabled = False
        txtActualSalaryPerm.Enabled = False
        txtBudgetSalaryTemp.Enabled = False
        txtFlexSalaryTemp.Enabled = False
        txtActualSalaryTemp.Enabled = False

        'lblVoidReasonCharCount.Visible = False
        'lblVoidMarker.Visible = False
        'lblVoidLabel.Visible = False
        'txtVoidReason.Visible = False
        'rfvVoidReason.Enabled = False

        trAllocatedSupportOTHours.Visible = False
        trAllocatedSupportTeamMemberContainmentCount.Visible = False
        trAllocatedSupportPartContainmentCount.Visible = False
        trAllocatedSupportOffStandardIndirectCount.Visible = False
        trAllocatedSupportIndirectPerm.Visible = False
        trAllocatedSupportIndirectTemp.Visible = False
        trAllocatedSupportOfficeHourlyPerm.Visible = False
        trAllocatedSupportOfficeHourlyTemp.Visible = False
        trAllocatedSupportSalaryPerm.Visible = False
        trAllocatedSupportSalaryTemp.Visible = False

        txtBudgetAllocatedSupportOTHours.Enabled = False
        txtActualAllocatedSupportOTHours.Enabled = False

        txtBudgetAllocatedSupportTeamMemberContainmentCount.Enabled = False
        txtActualAllocatedSupportTeamMemberContainmentCount.Enabled = False

        txtBudgetAllocatedSupportPartContainmentCount.Enabled = False
        txtActualAllocatedSupportPartContainmentCount.Enabled = False

        txtBudgetAllocatedSupportOffStandardIndirectCount.Enabled = False
        txtActualAllocatedSupportOffStandardIndirectCount.Enabled = False

        txtBudgetAllocatedSupportIndirectPerm.Enabled = False
        txtFlexAllocatedSupportIndirectPerm.Enabled = False
        txtActualAllocatedSupportIndirectPerm.Enabled = False

        txtBudgetAllocatedSupportIndirectTemp.Enabled = False
        txtFlexAllocatedSupportIndirectTemp.Enabled = False
        txtActualAllocatedSupportIndirectTemp.Enabled = False

        txtBudgetAllocatedSupportOfficeHourlyPerm.Enabled = False
        txtFlexAllocatedSupportOfficeHourlyPerm.Enabled = False
        txtActualAllocatedSupportOfficeHourlyPerm.Enabled = False

        txtBudgetAllocatedSupportOfficeHourlyTemp.Enabled = False
        txtFlexAllocatedSupportOfficeHourlyTemp.Enabled = False
        txtActualAllocatedSupportOfficeHourlyTemp.Enabled = False

        txtBudgetAllocatedSupportSalaryPerm.Enabled = False
        txtFlexAllocatedSupportSalaryPerm.Enabled = False
        txtActualAllocatedSupportSalaryPerm.Enabled = False

        txtBudgetAllocatedSupportSalaryTemp.Enabled = False
        txtFlexAllocatedSupportSalaryTemp.Enabled = False
        txtActualAllocatedSupportSalaryTemp.Enabled = False


    End Sub

    Private Sub HandleDepartmentPopups(ByVal DeptID As Integer)

        Try
            Dim strMMSource As String = ""
            btnViewCalculationSources.Visible = False

            'If DeptID > 0 Then
            btnViewCalculationSources.Visible = True
            strMMSource = "javascript:void(window.open('Manufacturing_Metric_Calculation_Sources.aspx?ReportID=" & ViewState("ReportID") & "&DeptID=" & DeptID & "&UGNFacility=" & ViewState("UGNFacility") & "&StartDate= " & ViewState("StartDate") & "&EndDate= " & ViewState("EndDate") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"

            btnViewCalculationSources.Attributes.Add("onclick", strMMSource)
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub EnableControls()

        Try
            Dim iDeptID As Integer = 0

            DisableAllControls()

            If ddDepartment.SelectedIndex > 0 Then
                iDeptID = ddDepartment.SelectedValue
            End If

            If ViewState("ReportID") > 0 Then

                'existing record
                lblUGNFacilityLabel.ForeColor = Color.Black
                lblMonthLabel.ForeColor = Color.Black
                lblYearLabel.ForeColor = Color.Black

                ddDepartment.Enabled = True

                If ViewState("StatusID") = 1 Or ViewState("StatusID") = 2 Then ' open or in-process  
                    'If iDeptID > 0 Then
                    '    'btnCalculate.Visible = ViewState("isAdmin")
                    '    ' i am not sure if the team members are ready for this yet
                    '    If ViewState("TeamMemberID") = 530 Then
                    '        btnBPCSRefresh.Visible = ViewState("isAdmin")
                    '    End If

                    'End If

                    btnSave.Visible = ViewState("isAdmin")
                    btnSaveBottom.Visible = ViewState("isAdmin")
                    btnSaveMiddle.Visible = ViewState("isAdmin")

                    If ViewState("StatusID") = 1 Then
                        btnNofityInternal.Visible = ViewState("isAdmin")
                    End If

                    If ViewState("StatusID") = 2 Then
                        btnNotifyFinal.Visible = ViewState("isAdmin")
                    End If

                    '' i am not sure if the team members are ready for this yet
                    'If ViewState("TeamMemberID") = 530 Then
                    '    btnVoid.Visible = ViewState("isAdmin")
                    '    btnVoid.Attributes.Add("onclick", "if(confirm('Are you sure that you want to void? If so, click ok to see and update the Void Reason field. Then click void again. ')){}else{return false}")
                    'End If
                  
                End If

                If ViewState("StatusID") <> 4 Then 'not voided
                    btnPreview.Visible = True
                    btnPreviewBottom.Visible = True
                End If

                'Else
                '    'new record 
                '    ' i am not sure if the team members are ready for this yet
                '    'If ViewState("TeamMemberID") = 530 Then
                '    '    btnLoadBPCS.Visible = ViewState("isAdmin")
                '    '    lblLoadBPCSWarning.Visible = ViewState("isAdmin")
                '    'End If

                '    btnSave.Visible = ViewState("isAdmin")
                '    btnSaveBottom.Visible = ViewState("isAdmin")

                '    lblUGNFacilityLabel.ForeColor = Color.Blue
                '    ddUGNFacility.Enabled = ViewState("isAdmin")

                '    lblMonthLabel.ForeColor = Color.Blue
                '    ddMonth.Enabled = ViewState("isAdmin")

                '    lblYearLabel.ForeColor = Color.Blue
                '    ddYear.Enabled = ViewState("isAdmin")
            End If 'If ViewState("ReportID") > 0

            If ViewState("StatusID") = 1 Or ViewState("StatusID") = 2 Then ' open or in-process 

                '' i am not sure if the team members are ready for this yet
                'If ViewState("TeamMemberID") = 530 Then
                '    btnLoadBPCS.Visible = ViewState("isAdmin")
                '    lblLoadBPCSWarning.Visible = ViewState("isAdmin")
                'End If

                If iDeptID > 0 Then

                    cbIncludeDepartment.Enabled = ViewState("isAdmin")

                    lblBudgetMachineHours.ForeColor = Color.Blue
                    lblBudgetManHours.ForeColor = Color.Blue
                    lblBudgetMetric.ForeColor = Color.Blue
                    lblBudgetScrapDollars.ForeColor = Color.Blue
                    lblBudgetRawWIPScrapDollars.ForeColor = Color.Blue
                    lblDirectOTHoursLabel.ForeColor = Color.Blue
                    lblDirectPermLabel.ForeColor = Color.Blue
                    lblDirectTempLabel.ForeColor = Color.Blue
                    lblEarnedDLHoursLabel.ForeColor = Color.Blue
                    lblIncludeDepartmentLabel.ForeColor = Color.Blue
                    lblIndirectOTHoursLabel.ForeColor = Color.Blue
                    lblIndirectPermLabel.ForeColor = Color.Blue
                    lblIndirectTempLabel.ForeColor = Color.Blue
                    lblTeamMemberContainmentCountLabel.ForeColor = Color.Blue
                    lblTeamMemberLeaderRatioLabel.ForeColor = Color.Blue
                    lblOEEBudgetPartCount.ForeColor = Color.Blue
                    lblOEEBudgetUsage.ForeColor = Color.Blue
                    lblOfficeHourlyPermLabel.ForeColor = Color.Blue
                    lblOfficeHourlyTempLabel.ForeColor = Color.Blue
                    lblOffStandardDirectCountLabel.ForeColor = Color.Blue
                    lblOffStandardIndirectCountLabel.ForeColor = Color.Blue
                    lblPartContainmentCountLabel.ForeColor = Color.Blue
                    lblSalaryPermLabel.ForeColor = Color.Blue
                    lblSalaryTempLabel.ForeColor = Color.Blue
                    lblTotalActualIndirectScrapDollar.ForeColor = Color.Blue

                    lblProductionPerformance.Text = ddDepartment.SelectedItem.Text

                    txtBudgetEarnedDLHours.Enabled = ViewState("isAdmin")

                    txtBudgetDirectOTHours.Enabled = ViewState("isAdmin")
                    txtActualDirectOTHours.Enabled = ViewState("isAdmin")

                    txtBudgetIndirectOTHours.Enabled = ViewState("isAdmin")
                    txtActualIndirectOTHours.Enabled = ViewState("isAdmin")

                    txtBudgetTeamMemberContainmentCount.Enabled = ViewState("isAdmin")
                    txtActualTeamMemberContainmentCount.Enabled = ViewState("isAdmin")

                    txtBudgetPartContainmentCount.Enabled = ViewState("isAdmin")
                    txtActualPartContainmentCount.Enabled = ViewState("isAdmin")

                    txtBudgetOffStandardDirectCount.Enabled = ViewState("isAdmin")
                    txtActualOffStandardDirectCount.Enabled = ViewState("isAdmin")

                    txtBudgetOffStandardIndirectCount.Enabled = ViewState("isAdmin")
                    txtActualOffStandardIndirectCount.Enabled = ViewState("isAdmin")

                    txtBudgetTeamMemberFactorCount.Enabled = ViewState("isAdmin")
                    txtBudgetTeamLeaderFactorCount.Enabled = ViewState("isAdmin")

                    txtActualTeamMemberFactorCount.Enabled = ViewState("isAdmin")
                    txtActualTeamLeaderFactorCount.Enabled = ViewState("isAdmin")

                    txtOEEBudgetGoodPartCount.Enabled = ViewState("isAdmin")
                    txtOEEBudgetScrapPartCount.Enabled = ViewState("isAdmin")
                    txtOEEBudgetTotalPartCount.Enabled = ViewState("isAdmin")

                    txtOEEBudgetDownHours.Enabled = ViewState("isAdmin")
                    txtBudgetMachineWorkedHours.Enabled = ViewState("isAdmin")
                    txtBudgetMachineStandardHours.Enabled = ViewState("isAdmin")
                    txtBudgetDowntimeHours.Enabled = ViewState("isAdmin")
                    txtBudgetMachineAvailableHours.Enabled = ViewState("isAdmin")
                    txtBudgetManWorkedHours.Enabled = ViewState("isAdmin")
                    txtBudgetDowntimeManHours.Enabled = ViewState("isAdmin")
                    txtTotalBudgetSpecificScrapDollar.Enabled = ViewState("isAdmin")
                    txtTotalBudgetMiscScrapDollar.Enabled = ViewState("isAdmin")
                    txtTotalBudgetProductionDollar.Enabled = ViewState("isAdmin")
                    txtTotalActualIndirectScrapDollar.Enabled = ViewState("isAdmin")
                    txtTotalBudgetRawWipScrapDollar.Enabled = ViewState("isAdmin")

                    trShift1Labels.Visible = True
                    trShift1Values.Visible = True
                    trShift2Labels.Visible = True
                    trShift2Values.Visible = True
                    trManHourLabels.Visible = True
                    trManHourActualValues.Visible = True
                    trManHourBudgetValues.Visible = True

                    txtBudgetDirectPerm.Enabled = ViewState("isAdmin")
                    txtFlexDirectPerm.Enabled = ViewState("isAdmin")
                    txtActualDirectPerm.Enabled = ViewState("isAdmin")
                    txtBudgetDirectTemp.Enabled = ViewState("isAdmin")
                    txtFlexDirectTemp.Enabled = ViewState("isAdmin")
                    txtActualDirectTemp.Enabled = ViewState("isAdmin")
                    txtBudgetIndirectPerm.Enabled = ViewState("isAdmin")
                    txtFlexIndirectPerm.Enabled = ViewState("isAdmin")
                    txtActualIndirectPerm.Enabled = ViewState("isAdmin")
                    txtBudgetIndirectTemp.Enabled = ViewState("isAdmin")
                    txtFlexIndirectTemp.Enabled = ViewState("isAdmin")
                    txtActualIndirectTemp.Enabled = ViewState("isAdmin")
                    txtBudgetOfficeHourlyPerm.Enabled = ViewState("isAdmin")

                    txtFlexOfficeHourlyPerm.Enabled = ViewState("isAdmin")
                    txtActualOfficeHourlyPerm.Enabled = ViewState("isAdmin")
                    txtBudgetOfficeHourlyTemp.Enabled = ViewState("isAdmin")
                    txtFlexOfficeHourlyTemp.Enabled = ViewState("isAdmin")
                    txtActualOfficeHourlyTemp.Enabled = ViewState("isAdmin")
                    txtBudgetSalaryPerm.Enabled = ViewState("isAdmin")
                    txtFlexSalaryPerm.Enabled = ViewState("isAdmin")
                    txtActualSalaryPerm.Enabled = ViewState("isAdmin")
                    txtBudgetSalaryTemp.Enabled = ViewState("isAdmin")
                    txtFlexSalaryTemp.Enabled = ViewState("isAdmin")
                    txtActualSalaryTemp.Enabled = ViewState("isAdmin")

                Else 'showing totals section
                    cbIncludeDepartment.Enabled = False

                    lblBudgetMachineHours.ForeColor = Color.Black
                    lblBudgetManHours.ForeColor = Color.Black
                    lblBudgetMetric.ForeColor = Color.Black
                    lblBudgetScrapDollars.ForeColor = Color.Black
                    lblBudgetRawWIPScrapDollars.ForeColor = Color.Black

                    lblDirectOTHoursLabel.ForeColor = Color.Black
                    lblDirectPermLabel.ForeColor = Color.Black
                    lblDirectTempLabel.ForeColor = Color.Black

                    lblEarnedDLHoursLabel.ForeColor = Color.Black

                    lblIncludeDepartmentLabel.ForeColor = Color.Black
                    lblIndirectOTHoursLabel.ForeColor = Color.Black
                    lblIndirectPermLabel.ForeColor = Color.Black
                    lblIndirectTempLabel.ForeColor = Color.Black

                    lblTeamMemberLeaderRatioLabel.ForeColor = Color.Black

                    lblOEEBudgetPartCount.ForeColor = Color.Black
                    lblOEEBudgetUsage.ForeColor = Color.Black

                    lblOfficeHourlyPermLabel.ForeColor = Color.Black
                    lblOfficeHourlyTempLabel.ForeColor = Color.Black
                    lblOffStandardDirectCountLabel.ForeColor = Color.Black
                    lblOffStandardIndirectCountLabel.ForeColor = Color.Black

                    lblPartContainmentCountLabel.ForeColor = Color.Black

                    lblSalaryPermLabel.ForeColor = Color.Black
                    lblSalaryTempLabel.ForeColor = Color.Black

                    lblTeamMemberContainmentCountLabel.ForeColor = Color.Black
                    lblTotalActualIndirectScrapDollar.ForeColor = Color.Black

                    'If txtActualOEE.Text.Trim = "" Then
                    '    btnLoadBPCS.Visible = ViewState("isAdmin")
                    '    lblLoadBPCSWarning.Visible = ViewState("isAdmin")
                    'End If

                    'btnBPCSRefresh.Visible = False

                    cbIncludeDepartment.Checked = True

                    lblProductionPerformance.Text = "Totals"

                    trShift1Labels.Visible = False
                    trShift1Values.Visible = False
                    trShift2Labels.Visible = False
                    trShift2Values.Visible = False
                    trManHourLabels.Visible = False
                    trManHourActualValues.Visible = False
                    trManHourBudgetValues.Visible = False

                    txtBudgetAllocatedSupportOTHours.Enabled = ViewState("isAdmin")
                    txtActualAllocatedSupportOTHours.Enabled = ViewState("isAdmin")

                    txtBudgetAllocatedSupportTeamMemberContainmentCount.Enabled = ViewState("isAdmin")
                    txtActualAllocatedSupportTeamMemberContainmentCount.Enabled = ViewState("isAdmin")

                    txtBudgetAllocatedSupportPartContainmentCount.Enabled = ViewState("isAdmin")
                    txtActualAllocatedSupportPartContainmentCount.Enabled = ViewState("isAdmin")

                    txtBudgetAllocatedSupportOffStandardIndirectCount.Enabled = ViewState("isAdmin")
                    txtActualAllocatedSupportOffStandardIndirectCount.Enabled = ViewState("isAdmin")

                    txtBudgetAllocatedSupportIndirectPerm.Enabled = ViewState("isAdmin")
                    txtFlexAllocatedSupportIndirectPerm.Enabled = ViewState("isAdmin")
                    txtActualAllocatedSupportIndirectPerm.Enabled = ViewState("isAdmin")

                    txtBudgetAllocatedSupportIndirectTemp.Enabled = ViewState("isAdmin")
                    txtFlexAllocatedSupportIndirectTemp.Enabled = ViewState("isAdmin")
                    txtActualAllocatedSupportIndirectTemp.Enabled = ViewState("isAdmin")

                    txtBudgetAllocatedSupportOfficeHourlyPerm.Enabled = ViewState("isAdmin")
                    txtFlexAllocatedSupportOfficeHourlyPerm.Enabled = ViewState("isAdmin")
                    txtActualAllocatedSupportOfficeHourlyPerm.Enabled = ViewState("isAdmin")

                    txtBudgetAllocatedSupportOfficeHourlyTemp.Enabled = ViewState("isAdmin")
                    txtFlexAllocatedSupportOfficeHourlyTemp.Enabled = ViewState("isAdmin")
                    txtActualAllocatedSupportOfficeHourlyTemp.Enabled = ViewState("isAdmin")

                    txtBudgetAllocatedSupportSalaryPerm.Enabled = ViewState("isAdmin")
                    txtFlexAllocatedSupportSalaryPerm.Enabled = ViewState("isAdmin")
                    txtActualAllocatedSupportSalaryPerm.Enabled = ViewState("isAdmin")

                    txtBudgetAllocatedSupportSalaryTemp.Enabled = ViewState("isAdmin")
                    txtFlexAllocatedSupportSalaryTemp.Enabled = ViewState("isAdmin")
                    txtActualAllocatedSupportSalaryTemp.Enabled = ViewState("isAdmin")

                End If

                cbBudgetStandardizedCellWork.Enabled = ViewState("isAdmin")
                cbActualStandardizedCellWork.Enabled = ViewState("isAdmin")

                txtBudgetCapacityUtilization.Enabled = ViewState("isAdmin")
                txtActualCapacityUtilization.Enabled = ViewState("isAdmin")

            End If

            If ViewState("StatusID") = 3 Then 'completed
                If iDeptID > 0 Then
                    trShift1Labels.Visible = True
                    trShift1Values.Visible = True
                    trShift2Labels.Visible = True
                    trShift2Values.Visible = True
                    trManHourLabels.Visible = True
                    trManHourActualValues.Visible = True
                    trManHourBudgetValues.Visible = True
                Else
                    trShift1Labels.Visible = False
                    trShift1Values.Visible = False
                    trShift2Labels.Visible = False
                    trShift2Values.Visible = False
                    trManHourLabels.Visible = False
                    trManHourActualValues.Visible = False
                    trManHourBudgetValues.Visible = False
                End If
            End If

            'no matter the status, show these rows if department=0
            If iDeptID = 0 Then
                trAllocatedSupportOTHours.Visible = True
                trAllocatedSupportTeamMemberContainmentCount.Visible = True
                trAllocatedSupportPartContainmentCount.Visible = True
                trAllocatedSupportOffStandardIndirectCount.Visible = True
                trAllocatedSupportIndirectPerm.Visible = True
                trAllocatedSupportIndirectTemp.Visible = True
                trAllocatedSupportOfficeHourlyPerm.Visible = True
                trAllocatedSupportOfficeHourlyTemp.Visible = True
                trAllocatedSupportSalaryPerm.Visible = True
                trAllocatedSupportSalaryTemp.Visible = True
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindCriteria()

        Try
            ''bind existing data to drop down controls for selection criteria for search       
            Dim ds As DataSet

            BindDepartment("")

            ds = commonFunctions.GetMonth("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddMonth.DataSource = ds
                ddMonth.DataTextField = ds.Tables(0).Columns("MonthName").ColumnName.ToString()
                ddMonth.DataValueField = ds.Tables(0).Columns("MonthID").ColumnName
                ddMonth.DataBind()
                ddMonth.Items.Insert(0, "")
            End If

            ds = PSRModule.GetManufacturingMetricStatusList()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddStatus.DataSource = ds
                ddStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddStatus.DataBind()
                ddStatus.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetTeamMemberBySubscription(20)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCreatedByTMID.DataSource = ds
                ddCreatedByTMID.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddCreatedByTMID.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddCreatedByTMID.DataBind()
                ddCreatedByTMID.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            ViewState("SubscriptionID") = 0
            ViewState("isAdmin") = False
            ViewState("TeamMemberID") = 0

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                'If iTeamMemberID = 530 Then
                '    iTeamMemberID = 171 ' Greg Hall
                '    'iTeamMemberID = 582 ' Bill Schultz
                '    'iTeamMemberID = 655 ' Roger Depperschmidt 
                '    'iTeamMemberID = 539 'Donna Davis
                '    'iTeamMemberID = 688 ' Tony Ugone
                '    'iTeamMemberID = 390 'Kim Worley
                'End If

                ViewState("TeamMemberID") = iTeamMemberID

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 106)

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
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Function SendEmail(ByVal EmailToAddress As String, ByVal EmailSubject As String, ByVal EmailBody As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try

            Dim strSubject As String = ""
            Dim strBody As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            Dim strEmailToAddress As String = EmailToAddress

            'handle test environment
            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br /><br />"
            End If

            strSubject &= EmailSubject
            strBody &= EmailBody

            'create the mail message using new System.Net.Mail (not CDonts)
            Dim mail As New MailMessage()

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody &= "<br /><br />Email To Address List: " & EmailToAddress & "<br />"

                strEmailToAddress = "Roderick.Carlson@ugnauto.com"
            End If

            strBody &= "<br /><br /><font size='1' face='Verdana'> +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++"
            strBody &= "<br />If you are receiving this by error, please submit the problem with the UGN Database Requestor, concerning the Plant Specific Reports Module"
            strBody &= "<br />Please <u>do not</u> reply back to this email because you will not receive a response.  Please use a separate email or phone call the appropriate UGN Team Member(s) for any questions or concerns you may have."
            strBody &= "<br /><font size='1' face='Verdana'> +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++</font>"

            'set the content
            mail.Subject = strSubject
            mail.Body = strBody

            'set the addresses
            mail.From = New MailAddress(strEmailFromAddress)
            Dim i As Integer

            'build email To list
            Dim emailList As String() = strEmailToAddress.Split(";")

            For i = 0 To UBound(emailList)
                If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                    mail.To.Add(emailList(i))
                End If
            Next i

            ''build email CC List
            'If strEmailCCAddress IsNot Nothing Then
            '    emailList = strEmailCCAddress.Split(";")

            '    For i = 0 To UBound(emailList)
            '        If emailList(i) <> ";" And emailList(i).Trim <> "" Then
            '            mail.CC.Add(emailList(i))
            '        End If
            '    Next i
            'End If

            'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
            mail.IsBodyHtml = True

            'send the message 
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(mail)
                lblMessage.Text &= "Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("Manufacturing Metric Notification", strEmailFromAddress, EmailToAddress, "", strSubject, strBody, "")
            End Try

            bReturnValue = True

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        SendEmail = bReturnValue

    End Function

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Manufacturing Metrics Monthly Data"

            '*****
            'Expand menu item
            '*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("MPRExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

            commonFunctions.SetUGNDBUser()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HandlePopups()

        Try

            Dim strPreviewClientScript As String = "javascript:void(window.open('crPreview_Manufacturing_Metric_Report.aspx?ReportType=M&ReportID=" & ViewState("ReportID") & " '," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            btnPreview.Attributes.Add("onclick", strPreviewClientScript)
            btnPreviewBottom.Attributes.Add("onclick", strPreviewClientScript)

            'btnLoadBPCS.Attributes.Add("onclick", "if(confirm('Are you sure that you want to reload all BPCS information for ALL DEPARTMENTS? Current information will be lost.')){}else{return false}")
            'btnBPCSRefresh.Attributes.Add("onclick", "if(confirm('Are you sure that you want to reload all BPCS information for this particular department? Current information will be lost for this department.')){}else{return false}")

            btnNofityInternal.Attributes.Add("onclick", "if(confirm('Are you sure that you want to notify team members to review this report?.')){}else{return false}")
            btnNotifyFinal.Attributes.Add("onclick", "if(confirm('WARNING: ALL INFORMATION WILL BE LOCKED AFTER THIS. Are you sure that you want to notify team members to review this final report?.')){}else{return false}")
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            PSRModule.CleanPSRMMCrystalReports()

            If Not Page.IsPostBack Then

                CheckRights()

                BindCriteria()

                ViewState("ReportID") = 0

                If HttpContext.Current.Request.QueryString("ReportID") <> "" Then
                    ViewState("ReportID") = HttpContext.Current.Request.QueryString("ReportID")
                End If

                BindHeaderData()

                BindDetailData(0)

                HandlePopups()

                txtNotes.Attributes.Add("onkeypress", "return tbLimit();")
                txtNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNotesCharCount.ClientID + ");")
                txtNotes.Attributes.Add("maxLength", "2000")

                ''***********************************************
                ''Code Below overrides the breadcrumb navigation 
                ''***********************************************
                Dim mpTextBox As Label
                mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)

                If ViewState("ReportID") = 0 Then
                    If Not mpTextBox Is Nothing Then
                        mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Manufacturing - Plant Specific Reports </b> > <a href='Manufacturing_Metric_List.aspx'><b> Monthly Manufacturing Monthly Metric List </b></a> > Manufacturing Metric Monthly Data"
                        mpTextBox.Visible = True
                    End If
                End If

                Master.FindControl("SiteMapPath1").Visible = False
            Else
                If ddUGNFacility.SelectedValue <> "" Then
                    BindDepartment(ddUGNFacility.SelectedValue)
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ddDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddDepartment.SelectedIndexChanged

        Try
            lblMessage.Text = ""

            Dim iDept As Integer = 0

            lblProductionPerformance.Text = ""

            If ddDepartment.SelectedIndex > 0 Then
                iDept = ddDepartment.SelectedValue
            End If

            If ddUGNFacility.SelectedValue <> "" Then
                BindDepartment(ddUGNFacility.SelectedValue)
            End If

            BindDetailData(iDept)

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Protected Sub ddUGNFacility_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUGNFacility.SelectedIndexChanged

    '    Try
    '        lblMessage.Text = ""

    '        Dim strUGNFacility As String = ""
    '        Dim iDeptID As Integer = 0

    '        If ddDepartment.SelectedIndex > 0 Then
    '            iDeptID = ddDepartment.SelectedValue
    '        End If

    '        If ddUGNFacility.SelectedIndex > 0 Then
    '            strUGNFacility = ddUGNFacility.SelectedValue
    '        End If

    '        ViewState("UGNFacility") = strUGNFacility

    '        BindDepartment(ViewState("UGNFacility"))
    '        BindDetailData(iDeptID)

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveMiddle.Click, btnSaveBottom.Click

        Try
            lblMessage.Text = ""

            'Dim ds As DataSet

            Dim iCreatedByTMID As Integer = 0
            Dim iMonthID As Integer = 0
            Dim iDeptID As Integer = 0
            Dim iYearID As Integer = 0

            Dim strUGNFacility As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strEmailURL As String = strProdOrTestEnvironment & "PlantSpecificReports/crPreview_Manufacturing_Metric_Report.aspx?ReportType=M&ReportID="

            Dim strEmailToAddress As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailBody As String = ""

            If ddCreatedByTMID.SelectedIndex > 0 Then
                iCreatedByTMID = ddCreatedByTMID.SelectedValue
            End If

            If ddMonth.SelectedIndex > 0 Then
                iMonthID = ddMonth.SelectedValue
            End If

            If ddDepartment.SelectedIndex > 0 Then
                iDeptID = ddDepartment.SelectedValue
            End If

            If ddYear.SelectedIndex > 0 Then
                iYearID = ddYear.SelectedValue
            End If

            If ddUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue
            End If

            Dim dBudgetOEE As Double = 0
            If txtBudgetOEE.Text.Trim <> "" Then
                dBudgetOEE = CType(txtBudgetOEE.Text.Trim, Double)
            End If

            Dim dActualOEE As Double = 0
            If txtActualOEE.Text.Trim <> "" Then
                dActualOEE = CType(txtActualOEE.Text.Trim, Double)
            End If

            Dim dBudgetEarnedDLHours As Double = 0
            If txtBudgetEarnedDLHours.Text.Trim <> "" Then
                dBudgetEarnedDLHours = CType(txtBudgetEarnedDLHours.Text.Trim, Double)
            End If

            Dim dActualEarnedDLHours As Double = 0
            If txtActualEarnedDLHours.Text.Trim <> "" Then
                dActualEarnedDLHours = CType(txtActualEarnedDLHours.Text.Trim, Double)
            End If

            Dim dBudgetDLHours As Double = 0
            If txtBudgetDLHours.Text.Trim <> "" Then
                dBudgetDLHours = CType(txtBudgetDLHours.Text.Trim, Double)
            End If

            Dim dActualDLHours As Double = 0
            If txtActualDLHours.Text.Trim <> "" Then
                dActualDLHours = CType(txtActualDLHours.Text.Trim, Double)
            End If

            Dim dBudgetMachineUtilization As Double = 0
            If lblBudgetMachineUtilization.Text.Trim <> "" Then
                dBudgetMachineUtilization = CType(lblBudgetMachineUtilization.Text.Trim, Double)
            End If

            Dim dActualMachineUtilization As Double = 0
            If lblActualMachineUtilization.Text.Trim <> "" Then
                dActualMachineUtilization = CType(lblActualMachineUtilization.Text.Trim, Double)
            End If

            Dim dBudgetDirectOTHours As Double = 0
            If txtBudgetDirectOTHours.Text.Trim <> "" Then
                dBudgetDirectOTHours = CType(txtBudgetDirectOTHours.Text.Trim, Double)
            End If

            Dim dActualDirectOTHours As Double = 0
            If txtActualDirectOTHours.Text.Trim <> "" Then
                dActualDirectOTHours = CType(txtActualDirectOTHours.Text.Trim, Double)
            End If

            Dim dBudgetIndirectOTHours As Double = 0
            If txtBudgetIndirectOTHours.Text.Trim <> "" Then
                dBudgetIndirectOTHours = CType(txtBudgetIndirectOTHours.Text.Trim, Double)
            End If

            Dim dActualIndirectOTHours As Double = 0
            If txtActualIndirectOTHours.Text.Trim <> "" Then
                dActualIndirectOTHours = CType(txtActualIndirectOTHours.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportOTHours As Double = 0
            If txtBudgetAllocatedSupportOTHours.Text.Trim <> "" Then
                dBudgetAllocatedSupportOTHours = CType(txtBudgetAllocatedSupportOTHours.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportOTHours As Double = 0
            If txtActualAllocatedSupportOTHours.Text.Trim <> "" Then
                dActualAllocatedSupportOTHours = CType(txtActualAllocatedSupportOTHours.Text.Trim, Double)
            End If

            Dim dBudgetScrap As Double = 0
            If txtBudgetScrapPercent.Text.Trim <> "" Then
                dBudgetScrap = CType(txtBudgetScrapPercent.Text.Trim, Double)
            End If

            Dim dActualScrap As Double = 0
            If txtActualScrapPercent.Text.Trim <> "" Then
                dActualScrap = CType(txtActualScrapPercent.Text.Trim, Double)
            End If

            Dim dBudgetTeamMemberContainmentCount As Double = 0
            If txtBudgetTeamMemberContainmentCount.Text.Trim <> "" Then
                dBudgetTeamMemberContainmentCount = CType(txtBudgetTeamMemberContainmentCount.Text.Trim, Double)
            End If

            Dim dActualTeamMemberContainmentCount As Double = 0
            If txtActualTeamMemberContainmentCount.Text.Trim <> "" Then
                dActualTeamMemberContainmentCount = CType(txtActualTeamMemberContainmentCount.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportTeamMemberContainmentCount As Double = 0
            If txtBudgetAllocatedSupportTeamMemberContainmentCount.Text.Trim <> "" Then
                dBudgetAllocatedSupportTeamMemberContainmentCount = CType(txtBudgetAllocatedSupportTeamMemberContainmentCount.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportTeamMemberContainmentCount As Double = 0
            If txtActualAllocatedSupportTeamMemberContainmentCount.Text.Trim <> "" Then
                dActualAllocatedSupportTeamMemberContainmentCount = CType(txtActualAllocatedSupportTeamMemberContainmentCount.Text.Trim, Double)
            End If

            Dim dBudgetPartContainmentCount As Double = 0
            If txtBudgetPartContainmentCount.Text.Trim <> "" Then
                dBudgetPartContainmentCount = CType(txtBudgetPartContainmentCount.Text.Trim, Double)
            End If

            Dim dActualPartContainmentCount As Double = 0
            If txtActualPartContainmentCount.Text.Trim <> "" Then
                dActualPartContainmentCount = CType(txtActualPartContainmentCount.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportPartContainmentCount As Double = 0
            If txtBudgetAllocatedSupportPartContainmentCount.Text.Trim <> "" Then
                dBudgetAllocatedSupportPartContainmentCount = CType(txtBudgetAllocatedSupportPartContainmentCount.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportPartContainmentCount As Double = 0
            If txtActualAllocatedSupportPartContainmentCount.Text.Trim <> "" Then
                dActualAllocatedSupportPartContainmentCount = CType(txtActualAllocatedSupportPartContainmentCount.Text.Trim, Double)
            End If

            Dim dBudgetOffStandardDirectCount As Double = 0
            If txtBudgetOffStandardDirectCount.Text.Trim <> "" Then
                dBudgetOffStandardDirectCount = CType(txtBudgetOffStandardDirectCount.Text.Trim, Double)
            End If

            Dim dActualOffStandardDirectCount As Double = 0
            If txtActualOffStandardDirectCount.Text.Trim <> "" Then
                dActualOffStandardDirectCount = CType(txtActualOffStandardDirectCount.Text.Trim, Double)
            End If

            Dim dBudgetOffStanardIndirectCount As Double = 0
            If txtBudgetOffStandardIndirectCount.Text.Trim <> "" Then
                dBudgetOffStanardIndirectCount = CType(txtBudgetOffStandardIndirectCount.Text.Trim, Double)
            End If

            Dim dActualOffStanardIndirectCount As Double = 0
            If txtActualOffStandardIndirectCount.Text.Trim <> "" Then
                dActualOffStanardIndirectCount = CType(txtActualOffStandardIndirectCount.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportOffStandardIndirectCount As Double = 0
            If txtBudgetAllocatedSupportOffStandardIndirectCount.Text.Trim <> "" Then
                dBudgetAllocatedSupportOffStandardIndirectCount = CType(txtBudgetAllocatedSupportOffStandardIndirectCount.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportOffStandardIndirectCount As Double = 0
            If txtActualAllocatedSupportOffStandardIndirectCount.Text.Trim <> "" Then
                dActualAllocatedSupportOffStandardIndirectCount = CType(txtActualAllocatedSupportOffStandardIndirectCount.Text.Trim, Double)
            End If

            Dim dBudgetTeamMemberFactorCount As Integer = 0
            If txtBudgetTeamMemberFactorCount.Text.Trim <> "" Then
                dBudgetTeamMemberFactorCount = CType(txtBudgetTeamMemberFactorCount.Text.Trim, Integer)
            End If

            Dim dBudgetTeamLeaderFactorCount As Integer = 0
            If txtBudgetTeamLeaderFactorCount.Text.Trim <> "" Then
                dBudgetTeamLeaderFactorCount = CType(txtBudgetTeamLeaderFactorCount.Text.Trim, Integer)
            End If

            Dim dActualTeamMemberFactorCount As Integer = 0
            If txtActualTeamMemberFactorCount.Text.Trim <> "" Then
                dActualTeamMemberFactorCount = CType(txtActualTeamMemberFactorCount.Text.Trim, Integer)
            End If

            Dim dActualTeamLeaderFactorCount As Integer = 0
            If txtActualTeamLeaderFactorCount.Text.Trim <> "" Then
                dActualTeamLeaderFactorCount = CType(txtActualTeamLeaderFactorCount.Text.Trim, Integer)
            End If

            Dim dBudgetCapacityUtilization As Double = 0
            If txtBudgetCapacityUtilization.Text.Trim <> "" Then
                dBudgetCapacityUtilization = CType(txtBudgetCapacityUtilization.Text.Trim, Double)
            End If

            Dim dActualCapacityUtilization As Double = 0
            If txtActualCapacityUtilization.Text.Trim <> "" Then
                dActualCapacityUtilization = CType(txtActualCapacityUtilization.Text.Trim, Double)
            End If

            Dim dOEEBudgetGoodPartCount As Double = 0
            If txtOEEBudgetGoodPartCount.Text.Trim <> "" Then
                dOEEBudgetGoodPartCount = CType(txtOEEBudgetGoodPartCount.Text.Trim, Double)
            End If

            Dim dOEEActualGoodPartCount As Double = 0
            If txtOEEActualGoodPartCount.Text.Trim <> "" Then
                dOEEActualGoodPartCount = CType(txtOEEActualGoodPartCount.Text.Trim, Double)
            End If

            Dim dOEEBudgetScrapPartCount As Double = 0
            If txtOEEBudgetScrapPartCount.Text.Trim <> "" Then
                dOEEBudgetScrapPartCount = CType(txtOEEBudgetScrapPartCount.Text.Trim, Double)
            End If

            Dim dOEEActualScrapPartCount As Double = 0
            If txtOEEActualScrapPartCount.Text.Trim <> "" Then
                dOEEActualScrapPartCount = CType(txtOEEActualScrapPartCount.Text.Trim, Double)
            End If

            Dim dOEEBudgetTotalPartCount As Double = 0
            If txtOEEBudgetTotalPartCount.Text.Trim <> "" Then
                dOEEBudgetTotalPartCount = CType(txtOEEBudgetTotalPartCount.Text.Trim, Double)
            End If

            Dim dOEEActualTotalPartCount As Double = 0
            If txtOEEActualTotalPartCount.Text.Trim <> "" Then
                dOEEActualTotalPartCount = CType(txtOEEActualTotalPartCount.Text.Trim, Double)
            End If

            Dim dOEEBudgetUtilization As Double = 0
            If txtOEEBudgetUtilization.Text.Trim <> "" Then
                dOEEBudgetUtilization = CType(txtOEEBudgetUtilization.Text.Trim, Double)
            End If

            Dim dOEEActualUtilization As Double = 0
            If txtOEEActualUtilization.Text.Trim <> "" Then
                dOEEActualUtilization = CType(txtOEEActualUtilization.Text.Trim, Double)
            End If

            Dim dOEEBudgetAvailableHours As Double = 0
            If txtOEEBudgetAvailableHours.Text.Trim <> "" Then
                dOEEBudgetAvailableHours = CType(txtOEEBudgetAvailableHours.Text.Trim, Double)
            End If

            Dim dOEEActualAvailableHours As Double = 0
            If txtOEEActualAvailableHours.Text.Trim <> "" Then
                dOEEActualAvailableHours = CType(txtOEEActualAvailableHours.Text.Trim, Double)
            End If

            Dim dOEEBudgetDownHours As Double = 0
            If txtOEEBudgetDownHours.Text.Trim <> "" Then
                dOEEBudgetDownHours = CType(txtOEEBudgetDownHours.Text.Trim, Double)
            End If

            Dim dOEEActualDownHours As Double = 0
            If txtOEEActualDownHours.Text.Trim <> "" Then
                dOEEActualDownHours = CType(txtOEEActualDownHours.Text.Trim, Double)
            End If

            Dim iMonthlyShippingDays As Integer = 0
            If txtMonthlyShippingDays.Text.Trim <> "" Then
                iMonthlyShippingDays = CType(txtMonthlyShippingDays.Text.Trim, Integer)
            End If

            Dim dHoursPerShift As Double = 0
            If txtHoursPerShift.Text.Trim <> "" Then
                dHoursPerShift = CType(txtHoursPerShift.Text.Trim, Double)
            End If

            Dim dBudgetShiftCount As Double = 0
            If lblBudgetShiftCount.Text.Trim <> "" Then
                dBudgetShiftCount = CType(lblBudgetShiftCount.Text.Trim, Double)
            End If

            Dim dActualShiftCount As Double = 0
            If lblActualShiftCount.Text.Trim <> "" Then
                dActualShiftCount = CType(lblActualShiftCount.Text.Trim, Double)
            End If

            Dim dAvailablePerShiftFactor As Double = 0
            If lblAvailablePerShiftFactor.Text.Trim <> "" Then
                dAvailablePerShiftFactor = CType(lblAvailablePerShiftFactor.Text.Trim, Double)
            End If

            Dim dBudgetDowntimeHours As Double = 0
            If txtBudgetDowntimeHours.Text.Trim <> "" Then
                dBudgetDowntimeHours = CType(txtBudgetDowntimeHours.Text.Trim, Double)
            End If

            Dim dActualDowntimeHours As Double = 0
            If txtActualDowntimeHours.Text.Trim <> "" Then
                dActualDowntimeHours = CType(txtActualDowntimeHours.Text.Trim, Double)
            End If

            Dim dBudgetMachineWorkedHours As Double = 0
            If txtBudgetMachineWorkedHours.Text.Trim <> "" Then
                dBudgetMachineWorkedHours = CType(txtBudgetMachineWorkedHours.Text.Trim, Double)
            End If

            Dim dActualMachineWorkedHours As Double = 0
            If txtActualMachineWorkedHours.Text.Trim <> "" Then
                dActualMachineWorkedHours = CType(txtActualMachineWorkedHours.Text.Trim, Double)
            End If

            Dim dBudgetMachineAvailableHours As Double = 0
            If txtBudgetMachineAvailableHours.Text.Trim <> "" Then
                dBudgetMachineAvailableHours = CType(txtBudgetMachineAvailableHours.Text.Trim, Double)
            End If

            Dim dActualMachineAvailableHours As Double = 0
            If txtActualMachineAvailableHours.Text.Trim <> "" Then
                dActualMachineAvailableHours = CType(txtActualMachineAvailableHours.Text.Trim, Double)
            End If

            Dim dBudgetMachineStandardHours As Double = 0
            If txtBudgetMachineStandardHours.Text.Trim <> "" Then
                dBudgetMachineStandardHours = CType(txtBudgetMachineStandardHours.Text.Trim, Double)
            End If

            Dim dActualMachineStandardHours As Double = 0
            If txtActualMachineStandardHours.Text.Trim <> "" Then
                dActualMachineStandardHours = CType(txtActualMachineStandardHours.Text.Trim, Double)
            End If

            Dim dBudgetManWorkedHours As Double = 0
            If txtBudgetManWorkedHours.Text.Trim <> "" Then
                dBudgetManWorkedHours = CType(txtBudgetManWorkedHours.Text.Trim, Double)
            End If

            Dim dActualManWorkedHours As Double = 0
            If txtActualManWorkedHours.Text.Trim <> "" Then
                dActualManWorkedHours = CType(txtActualManWorkedHours.Text.Trim, Double)
            End If

            Dim dBudgetDowntimeManHours As Double = 0
            If txtBudgetDowntimeManHours.Text.Trim <> "" Then
                dBudgetDowntimeManHours = CType(txtBudgetDowntimeManHours.Text.Trim, Double)
            End If

            Dim dActualDowntimeManHours As Double = 0
            If txtActualDowntimeManHours.Text.Trim <> "" Then
                dActualDowntimeManHours = CType(txtActualDowntimeManHours.Text.Trim, Double)
            End If

            Dim dTotalBudgetProductionDollar As Double = 0
            If txtTotalBudgetProductionDollar.Text.Trim <> "" Then
                dTotalBudgetProductionDollar = CType(txtTotalBudgetProductionDollar.Text.Trim, Double)
            End If

            Dim dTotalActualProductionDollar As Double = 0
            If txtTotalActualProductionDollar.Text.Trim <> "" Then
                dTotalActualProductionDollar = CType(txtTotalActualProductionDollar.Text.Trim, Double)
            End If

            Dim dTotalBudgetSpecificScrapDollar As Double = 0
            If txtTotalBudgetSpecificScrapDollar.Text.Trim <> "" Then
                dTotalBudgetSpecificScrapDollar = CType(txtTotalBudgetSpecificScrapDollar.Text.Trim, Double)
            End If

            Dim dTotalActualSpecificScrapDollar As Double = 0
            If txtTotalActualSpecificScrapDollar.Text.Trim <> "" Then
                dTotalActualSpecificScrapDollar = CType(txtTotalActualSpecificScrapDollar.Text.Trim, Double)
            End If

            Dim dTotalBudgetMiscScrapDollar As Double = 0
            If txtTotalBudgetMiscScrapDollar.Text.Trim <> "" Then
                dTotalBudgetMiscScrapDollar = CType(txtTotalBudgetMiscScrapDollar.Text.Trim, Double)
            End If

            Dim dTotalActualMiscScrapDollar As Double = 0
            If txtTotalActualMiscScrapDollar.Text.Trim <> "" Then
                dTotalActualMiscScrapDollar = CType(txtTotalActualMiscScrapDollar.Text.Trim, Double)
            End If

            Dim dTotalActualIndirectScrapDollar As Double = 0
            If txtTotalActualIndirectScrapDollar.Text.Trim <> "" Then
                dTotalActualIndirectScrapDollar = CType(txtTotalActualIndirectScrapDollar.Text.Trim, Double)
            End If

            Dim dTotalBudgetRawWipScrapDollar As Double = 0
            If txtTotalBudgetRawWipScrapDollar.Text.Trim <> "" Then
                dTotalBudgetRawWipScrapDollar = CType(txtTotalBudgetRawWipScrapDollar.Text.Trim, Double)
            End If

            Dim dTotalActualRawWipScrapDollar As Double = 0
            If txtTotalActualRawWipScrapDollar.Text.Trim <> "" Then
                dTotalActualRawWipScrapDollar = CType(txtTotalActualRawWipScrapDollar.Text.Trim, Double)
            End If

            Dim dBudgetDirectPerm As Double = 0
            If txtBudgetDirectPerm.Text.Trim <> "" Then
                dBudgetDirectPerm = CType(txtBudgetDirectPerm.Text.Trim, Double)
            End If

            Dim dFlexDirectPerm As Double = 0
            If txtFlexDirectPerm.Text.Trim <> "" Then
                dFlexDirectPerm = CType(txtFlexDirectPerm.Text.Trim, Double)
            End If

            Dim dActualDirectPerm As Double = 0
            If txtActualDirectPerm.Text.Trim <> "" Then
                dActualDirectPerm = CType(txtActualDirectPerm.Text.Trim, Double)
            End If

            Dim dBudgetDirectTemp As Double = 0
            If txtBudgetDirectTemp.Text.Trim <> "" Then
                dBudgetDirectTemp = CType(txtBudgetDirectTemp.Text.Trim, Double)
            End If

            Dim dFlexDirectTemp As Double = 0
            If txtFlexDirectTemp.Text.Trim <> "" Then
                dFlexDirectTemp = CType(txtFlexDirectTemp.Text.Trim, Double)
            End If

            Dim dActualDirectTemp As Double = 0
            If txtActualDirectTemp.Text.Trim <> "" Then
                dActualDirectTemp = CType(txtActualDirectTemp.Text.Trim, Double)
            End If

            Dim dBudgetIndirectPerm As Double = 0
            If txtBudgetIndirectPerm.Text.Trim <> "" Then
                dBudgetIndirectPerm = CType(txtBudgetIndirectPerm.Text.Trim, Double)
            End If

            Dim dFlexIndirectPerm As Double = 0
            If txtFlexIndirectPerm.Text.Trim <> "" Then
                dFlexIndirectPerm = CType(txtFlexIndirectPerm.Text.Trim, Double)
            End If

            Dim dActualIndirectPerm As Double = 0
            If txtActualIndirectPerm.Text.Trim <> "" Then
                dActualIndirectPerm = CType(txtActualIndirectPerm.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportIndirectPerm As Double = 0
            If txtBudgetAllocatedSupportIndirectPerm.Text.Trim <> "" Then
                dBudgetAllocatedSupportIndirectPerm = CType(txtBudgetAllocatedSupportIndirectPerm.Text.Trim, Double)
            End If

            Dim dFlexAllocatedSupportIndirectPerm As Double = 0
            If txtFlexAllocatedSupportIndirectPerm.Text.Trim <> "" Then
                dFlexAllocatedSupportIndirectPerm = CType(txtFlexAllocatedSupportIndirectPerm.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportIndirectPerm As Double = 0
            If txtActualAllocatedSupportIndirectPerm.Text.Trim <> "" Then
                dActualAllocatedSupportIndirectPerm = CType(txtActualAllocatedSupportIndirectPerm.Text.Trim, Double)
            End If

            Dim dBudgetIndirectTemp As Double = 0
            If txtBudgetIndirectTemp.Text.Trim <> "" Then
                dBudgetIndirectTemp = CType(txtBudgetIndirectTemp.Text.Trim, Double)
            End If

            Dim dFlexIndirectTemp As Double = 0
            If txtFlexIndirectTemp.Text.Trim <> "" Then
                dFlexIndirectTemp = CType(txtFlexIndirectTemp.Text.Trim, Double)
            End If

            Dim dActualIndirectTemp As Double = 0
            If txtActualIndirectTemp.Text.Trim <> "" Then
                dActualIndirectTemp = CType(txtActualIndirectTemp.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportIndirectTemp As Double = 0
            If txtBudgetAllocatedSupportIndirectTemp.Text.Trim <> "" Then
                dBudgetAllocatedSupportIndirectTemp = CType(txtBudgetAllocatedSupportIndirectTemp.Text.Trim, Double)
            End If

            Dim dFlexAllocatedSupportIndirectTemp As Double = 0
            If txtFlexAllocatedSupportIndirectTemp.Text.Trim <> "" Then
                dFlexAllocatedSupportIndirectTemp = CType(txtFlexAllocatedSupportIndirectTemp.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportIndirectTemp As Double = 0
            If txtActualAllocatedSupportIndirectTemp.Text.Trim <> "" Then
                dActualAllocatedSupportIndirectTemp = CType(txtActualAllocatedSupportIndirectTemp.Text.Trim, Double)
            End If

            Dim dBudgetOfficeHourlyPerm As Double = 0
            If txtBudgetOfficeHourlyPerm.Text.Trim <> "" Then
                dBudgetOfficeHourlyPerm = CType(txtBudgetOfficeHourlyPerm.Text.Trim, Double)
            End If

            Dim dFlexOfficeHourlyPerm As Double = 0
            If txtFlexOfficeHourlyPerm.Text.Trim <> "" Then
                dFlexOfficeHourlyPerm = CType(txtFlexOfficeHourlyPerm.Text.Trim, Double)
            End If

            Dim dActualOfficeHourlyPerm As Double = 0
            If txtActualOfficeHourlyPerm.Text.Trim <> "" Then
                dActualOfficeHourlyPerm = CType(txtActualOfficeHourlyPerm.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportOfficeHourlyPerm As Double = 0
            If txtBudgetAllocatedSupportOfficeHourlyPerm.Text.Trim <> "" Then
                dBudgetAllocatedSupportOfficeHourlyPerm = CType(txtBudgetAllocatedSupportOfficeHourlyPerm.Text.Trim, Double)
            End If

            Dim dFlexAllocatedSupportOfficeHourlyPerm As Double = 0
            If txtFlexAllocatedSupportOfficeHourlyPerm.Text.Trim <> "" Then
                dFlexAllocatedSupportOfficeHourlyPerm = CType(txtFlexAllocatedSupportOfficeHourlyPerm.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportOfficeHourlyPerm As Double = 0
            If txtActualAllocatedSupportOfficeHourlyPerm.Text.Trim <> "" Then
                dActualAllocatedSupportOfficeHourlyPerm = CType(txtActualAllocatedSupportOfficeHourlyPerm.Text.Trim, Double)
            End If

            Dim dBudgetOfficeHourlyTemp As Double = 0
            If txtBudgetOfficeHourlyTemp.Text.Trim <> "" Then
                dBudgetOfficeHourlyTemp = CType(txtBudgetOfficeHourlyTemp.Text.Trim, Double)
            End If

            Dim dFlexOfficeHourlyTemp As Double = 0
            If txtFlexOfficeHourlyTemp.Text.Trim <> "" Then
                dFlexOfficeHourlyTemp = CType(txtFlexOfficeHourlyTemp.Text.Trim, Double)
            End If

            Dim dActualOfficeHourlyTemp As Double = 0
            If txtActualOfficeHourlyTemp.Text.Trim <> "" Then
                dActualOfficeHourlyTemp = CType(txtActualOfficeHourlyTemp.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportOfficeHourlyTemp As Double = 0
            If txtBudgetAllocatedSupportOfficeHourlyTemp.Text.Trim <> "" Then
                dBudgetAllocatedSupportOfficeHourlyTemp = CType(txtBudgetAllocatedSupportOfficeHourlyTemp.Text.Trim, Double)
            End If

            Dim dFlexAllocatedSupportOfficeHourlyTemp As Double = 0
            If txtFlexAllocatedSupportOfficeHourlyTemp.Text.Trim <> "" Then
                dFlexAllocatedSupportOfficeHourlyTemp = CType(txtFlexAllocatedSupportOfficeHourlyTemp.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportOfficeHourlyTemp As Double = 0
            If txtActualAllocatedSupportOfficeHourlyTemp.Text.Trim <> "" Then
                dActualAllocatedSupportOfficeHourlyTemp = CType(txtActualAllocatedSupportOfficeHourlyTemp.Text.Trim, Double)
            End If

            Dim dBudgetSalaryPerm As Double = 0
            If txtBudgetSalaryPerm.Text.Trim <> "" Then
                dBudgetSalaryPerm = CType(txtBudgetSalaryPerm.Text.Trim, Double)
            End If

            Dim dFlexSalaryPerm As Double = 0
            If txtFlexSalaryPerm.Text.Trim <> "" Then
                dFlexSalaryPerm = CType(txtFlexSalaryPerm.Text.Trim, Double)
            End If

            Dim dActualSalaryPerm As Double = 0
            If txtActualSalaryPerm.Text.Trim <> "" Then
                dActualSalaryPerm = CType(txtActualSalaryPerm.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportSalaryPerm As Double = 0
            If txtBudgetAllocatedSupportSalaryPerm.Text.Trim <> "" Then
                dBudgetAllocatedSupportSalaryPerm = CType(txtBudgetAllocatedSupportSalaryPerm.Text.Trim, Double)
            End If

            Dim dFlexAllocatedSupportSalaryPerm As Double = 0
            If txtFlexAllocatedSupportSalaryPerm.Text.Trim <> "" Then
                dFlexAllocatedSupportSalaryPerm = CType(txtFlexAllocatedSupportSalaryPerm.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportSalaryPerm As Double = 0
            If txtActualAllocatedSupportSalaryPerm.Text.Trim <> "" Then
                dActualAllocatedSupportSalaryPerm = CType(txtActualAllocatedSupportSalaryPerm.Text.Trim, Double)
            End If

            Dim dBudgetSalaryTemp As Double = 0
            If txtBudgetSalaryTemp.Text.Trim <> "" Then
                dBudgetSalaryTemp = CType(txtBudgetSalaryTemp.Text.Trim, Double)
            End If

            Dim dFlexSalaryTemp As Double = 0
            If txtFlexSalaryTemp.Text.Trim <> "" Then
                dFlexSalaryTemp = CType(txtFlexSalaryTemp.Text.Trim, Double)
            End If

            Dim dActualSalaryTemp As Double = 0
            If txtActualSalaryTemp.Text.Trim <> "" Then
                dActualSalaryTemp = CType(txtActualSalaryTemp.Text.Trim, Double)
            End If

            Dim dBudgetAllocatedSupportSalaryTemp As Double = 0
            If txtBudgetAllocatedSupportSalaryTemp.Text.Trim <> "" Then
                dBudgetAllocatedSupportSalaryTemp = CType(txtBudgetAllocatedSupportSalaryTemp.Text.Trim, Double)
            End If

            Dim dFlexAllocatedSupportSalaryTemp As Double = 0
            If txtFlexAllocatedSupportSalaryTemp.Text.Trim <> "" Then
                dFlexAllocatedSupportSalaryTemp = CType(txtFlexAllocatedSupportSalaryTemp.Text.Trim, Double)
            End If

            Dim dActualAllocatedSupportSalaryTemp As Double = 0
            If txtActualAllocatedSupportSalaryTemp.Text.Trim <> "" Then
                dActualAllocatedSupportSalaryTemp = CType(txtActualAllocatedSupportSalaryTemp.Text.Trim, Double)
            End If


            CalculateProductionPerformance(iDeptID, dBudgetEarnedDLHours, dBudgetDLHours, dHoursPerShift, _
                                           dBudgetMachineWorkedHours, dBudgetDowntimeHours, iMonthlyShippingDays, _
                                           dAvailablePerShiftFactor, dOEEBudgetTotalPartCount, dOEEBudgetGoodPartCount, _
                                           dOEEBudgetDownHours, dBudgetManWorkedHours, dBudgetDowntimeManHours, _
                                           dTotalBudgetSpecificScrapDollar, dTotalBudgetMiscScrapDollar, dTotalBudgetProductionDollar, _
                                           dTotalActualSpecificScrapDollar, dTotalActualMiscScrapDollar, dTotalActualProductionDollar, _
                                           dTotalActualIndirectScrapDollar, dBudgetTeamMemberFactorCount, dBudgetTeamLeaderFactorCount, _
                                           dActualTeamMemberFactorCount, dActualTeamLeaderFactorCount, dBudgetMachineStandardHours, _
                                           dTotalBudgetRawWipScrapDollar, dTotalActualRawWipScrapDollar)

            'refresh values based on new calculations
            If lblBudgetShiftCount.Text.Trim <> "" Then
                dBudgetShiftCount = CType(lblBudgetShiftCount.Text.Trim, Double)
            End If

            If txtOEEBudgetAvailableHours.Text.Trim <> "" Then
                dOEEBudgetAvailableHours = CType(txtOEEBudgetAvailableHours.Text.Trim, Double)
            End If

            If txtOEEBudgetUtilization.Text.Trim <> "" Then
                dOEEBudgetUtilization = CType(txtOEEBudgetUtilization.Text.Trim, Double)
            End If

            If lblBudgetMachineUtilization.Text.Trim <> "" Then
                dBudgetMachineUtilization = CType(lblBudgetMachineUtilization.Text.Trim, Double)
            End If

            If txtBudgetOEE.Text.Trim <> "" Then
                dBudgetOEE = CType(txtBudgetOEE.Text.Trim, Double)
            End If

            If txtBudgetDLHours.Text.Trim <> "" Then
                dBudgetDLHours = CType(txtBudgetDLHours.Text.Trim, Double)
            End If

            If txtBudgetScrapPercent.Text.Trim <> "" Then
                dBudgetScrap = CType(txtBudgetScrapPercent.Text.Trim, Double)
            End If

            If txtActualScrapPercent.Text.Trim <> "" Then
                dActualScrap = CType(txtActualScrapPercent.Text.Trim, Double)
            End If

            CalculateTeamMembers()

            If ViewState("isAdmin") = True Then
                'UGN Facility, Month, and Year should be locked after first save
                If ViewState("ReportID") = 0 Then
                    'ViewState("StatusID") = 1
                    'ds = PSRModule.InsertManufacturingMetricHeaderByDept(iMonthID, iYearID, strUGNFacility, ViewState("StatusID"), iCreatedByTMID)
                    'If commonFunctions.CheckDataSet(ds) = True Then
                    '    If ds.Tables(0).Rows(0).Item("NewReportID") IsNot System.DBNull.Value Then
                    '        If ds.Tables(0).Rows(0).Item("NewReportID") > 0 Then
                    '            ViewState("ReportID") = ds.Tables(0).Rows(0).Item("NewReportID")
                    '            PSRModule.InsertManufacturingMetricHistory(ViewState("ReportID"), ViewState("TeamMemberID"), "Created Monthly Report")
                    '        End If ' IF NewReportID > 0
                    '    End If 'If NewReportID is not empty
                    'End If 'If ds is not empty
                Else ' reportID exists                
                    'calculate and save totals, department=0           
                    If iDeptID > 0 Then
                        PSRModule.UpdateManufacturingMetricDetailByDept(ViewState("ReportID"), iDeptID, dBudgetOEE, dActualOEE, _
                        dBudgetEarnedDLHours, dActualEarnedDLHours, dBudgetDLHours, dActualDLHours, dBudgetDirectOTHours, dActualDirectOTHours, _
                        dBudgetIndirectOTHours, dActualIndirectOTHours, dBudgetScrap, dActualScrap, _
                        dBudgetTeamMemberContainmentCount, dActualTeamMemberContainmentCount, _
                        dBudgetPartContainmentCount, dActualPartContainmentCount, _
                        dBudgetOffStandardDirectCount, dActualOffStandardDirectCount, _
                        dBudgetOffStanardIndirectCount, dActualOffStanardIndirectCount, _
                        cbBudgetStandardizedCellWork.Checked, cbActualStandardizedCellWork.Checked, _
                        dBudgetTeamMemberFactorCount, dBudgetTeamLeaderFactorCount, "", _
                        dActualTeamMemberFactorCount, dActualTeamLeaderFactorCount, "", _
                        dBudgetCapacityUtilization, dActualCapacityUtilization, _
                        dOEEBudgetGoodPartCount, dOEEActualGoodPartCount, _
                        dOEEBudgetScrapPartCount, dOEEActualScrapPartCount, _
                        dOEEBudgetTotalPartCount, dOEEActualTotalPartCount, _
                        dOEEBudgetUtilization, dOEEActualUtilization, _
                        dOEEBudgetAvailableHours, dOEEActualAvailableHours, _
                        dOEEBudgetDownHours, dOEEActualDownHours, _
                        iMonthlyShippingDays, dHoursPerShift, _
                        dBudgetShiftCount, dActualShiftCount, _
                        dAvailablePerShiftFactor, dBudgetDowntimeHours, dActualDowntimeHours, _
                        dBudgetMachineWorkedHours, dActualMachineWorkedHours, _
                        dBudgetMachineAvailableHours, dActualMachineAvailableHours, _
                        dBudgetManWorkedHours, dActualManWorkedHours, _
                        dBudgetDowntimeManHours, dActualDowntimeManHours, _
                        dTotalBudgetProductionDollar, dTotalActualProductionDollar, _
                        dTotalBudgetSpecificScrapDollar, dTotalActualSpecificScrapDollar, _
                        dTotalBudgetMiscScrapDollar, dTotalActualMiscScrapDollar, dTotalActualIndirectScrapDollar, _
                        dBudgetMachineStandardHours, dActualMachineStandardHours, _
                        dTotalBudgetRawWipScrapDollar, dTotalActualRawWipScrapDollar, _
                        dBudgetDirectPerm, dFlexDirectPerm, dActualDirectPerm, dBudgetDirectTemp, dFlexDirectTemp, _
                        dActualDirectTemp, dBudgetIndirectPerm, dFlexIndirectPerm, dActualIndirectPerm, _
                        dBudgetIndirectTemp, dFlexIndirectTemp, dActualIndirectTemp, dBudgetOfficeHourlyPerm, _
                        dFlexOfficeHourlyPerm, dActualOfficeHourlyPerm, dBudgetOfficeHourlyTemp, _
                        dFlexOfficeHourlyTemp, dActualOfficeHourlyTemp, dBudgetSalaryPerm, dFlexSalaryPerm, _
                        dActualSalaryPerm, dBudgetSalaryTemp, dFlexSalaryTemp, dActualSalaryTemp, _
                        txtNotes.Text.Trim, Not cbIncludeDepartment.Checked)
                    Else
                       
                        'if department=0 then save a few details to the totals table
                        PSRModule.UpdateManufacturingMetricTotalByDept(ViewState("ReportID"), _
                        dBudgetOEE, dActualOEE, _
                        dBudgetAllocatedSupportOTHours, dActualAllocatedSupportOTHours, _
                        dBudgetMachineUtilization, dActualMachineUtilization, _
                        dBudgetScrap, dActualScrap, _
                        dBudgetAllocatedSupportTeamMemberContainmentCount, dActualAllocatedSupportTeamMemberContainmentCount, _
                        dBudgetAllocatedSupportPartContainmentCount, dActualAllocatedSupportPartContainmentCount, _
                        dBudgetAllocatedSupportOffStandardIndirectCount, dActualAllocatedSupportOffStandardIndirectCount, _
                        cbBudgetStandardizedCellWork.Checked, cbActualStandardizedCellWork.Checked, "", "", _
                        dBudgetCapacityUtilization, dActualCapacityUtilization, _
                        dBudgetAllocatedSupportIndirectPerm, dFlexAllocatedSupportIndirectPerm, dActualAllocatedSupportIndirectPerm, _
                        dBudgetAllocatedSupportIndirectTemp, dFlexAllocatedSupportIndirectTemp, dActualAllocatedSupportIndirectTemp, _
                        dBudgetAllocatedSupportOfficeHourlyPerm, dFlexAllocatedSupportOfficeHourlyPerm, dActualAllocatedSupportOfficeHourlyPerm, _
                        dBudgetAllocatedSupportOfficeHourlyTemp, dFlexAllocatedSupportOfficeHourlyTemp, dActualAllocatedSupportOfficeHourlyTemp, _
                        dBudgetAllocatedSupportSalaryPerm, dFlexAllocatedSupportSalaryPerm, dActualAllocatedSupportSalaryPerm, _
                        dBudgetAllocatedSupportSalaryTemp, dFlexAllocatedSupportSalaryTemp, dActualAllocatedSupportSalaryTemp, _
                        txtNotes.Text.Trim)
                        'if createdby Team MemberID changed
                        If ViewState("NewTeamMemberID") = True Then
                            PSRModule.UpdateManufacturingMetricByDept(ViewState("ReportID"), ViewState("TeamMemberID"))
                        End If
                    End If ' iDeptID > 0 

                    '''''''''''''''''''''''''''''''''''''''''''
                    'Update Monthly Report History IF ALREADY IN PROCESS (being reviewed by team members)
                    '''''''''''''''''''''''''''''''''''''''''''
                    If ViewState("StatusID") = 2 Then
                        PSRModule.InsertManufacturingMetricHistory(ViewState("ReportID"), ViewState("TeamMemberID"), "Updated Monthly Report")
                    End If

                    lblMessage.Text &= "<br />Saved Successfully"

                End If

            End If 'ViewState("ReportID") > 0

            EnableControls()

            If ViewState("StatusID") = 2 And ViewState("isAdmin") = True Then
                ''Allow to Notify Internal Team Members that the report has been updated
                btnNofityInternal.Visible = ViewState("isAdmin")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Protected Sub ddMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddMonth.SelectedIndexChanged

    '    Try
    '        lblMessage.Text = ""

    '        Dim iDeptID As Integer = 0

    '        If ddDepartment.SelectedIndex > 0 Then
    '            iDeptID = ddDepartment.SelectedValue
    '        End If

    '        If ddMonth.SelectedIndex > 0 And ddYear.SelectedIndex > 0 Then
    '            SetDateRange(ddMonth.SelectedValue, ddYear.SelectedValue)

    '            BindDetailData(iDeptID)

    '            EnableControls()
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    'Protected Sub ddYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddYear.SelectedIndexChanged

    '    Try
    '        lblMessage.Text = ""

    '        Dim iDeptID As Integer = 0

    '        If ddDepartment.SelectedIndex > 0 Then
    '            iDeptID = ddDepartment.SelectedValue
    '        End If

    '        If ddMonth.SelectedIndex > 0 And ddYear.SelectedIndex > 0 Then
    '            SetDateRange(ddMonth.SelectedValue, ddYear.SelectedValue)

    '            BindDetailData(iDeptID)
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    'Protected Sub btnBPCSRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBPCSRefresh.Click

    '    Try
    '        lblMessage.Text = ""

    '        Dim iDeptID As Integer = 0

    '        If ddDepartment.SelectedIndex > 0 Then
    '            iDeptID = ddDepartment.SelectedValue
    '        End If

    '        If iDeptID > 0 Then
    '            BPCSRefresh(iDeptID)
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    'Protected Sub btnLoadBPCS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadBPCS.Click

    '    Try
    '        lblMessage.Text = "Loading all department data from BPCS could take a few minutes...."

    '        Dim iDeptID As Integer = 0
    '        Dim iRowCounter As Integer = 0

    '        Dim ds As DataSet

    '        btnLoadBPCS.Enabled = False

    '        ds = PSRModule.GetManufacturingMetricDepartment(ViewState("UGNFacility"))
    '        If commonFunctions.CheckDataSet(ds) = True Then
    '            For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
    '                If ds.Tables(0).Rows(iRowCounter).Item("CDEPT") IsNot System.DBNull.Value Then
    '                    If ds.Tables(0).Rows(iRowCounter).Item("CDEPT") > 0 Then
    '                        iDeptID = ds.Tables(0).Rows(iRowCounter).Item("CDEPT")
    '                        BPCSRefresh(iDeptID)
    '                        ddDepartment.SelectedValue = iDeptID
    '                        btnSave_Click(sender, e)
    '                    End If ' not 0
    '                End If 'not null
    '            Next

    '            BindDepartment(ViewState("UGNFacility"))
    '        End If 'ds is not empty

    '        lblMessage.Text = "All departments have been saved, if they contain valid information."

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub btnNofityInternal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNofityInternal.Click

        Try

            lblMessage.Text = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strEmailURL As String = strProdOrTestEnvironment & "PlantSpecificReports/crPreview_Manufacturing_Metric_Report.aspx?ReportType=M&ReportID="

            Dim strEmailToAddress As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailBody As String = ""

            If ViewState("StatusID") = 0 Or ViewState("StatusID") = 1 Then
                '''''''''''''''''''''''''''''''''''''''''''
                'Update Monthly Report Status
                '''''''''''''''''''''''''''''''''''''''''''
                ViewState("StatusID") = 2
                ddStatus.SelectedValue = 2
                PSRModule.UpdateManufacturingMetricStatusByDept(ViewState("ReportID"), ViewState("StatusID"))

                '''''''''''''''''''''''''''''''''''''''''''
                'Update Monthly Report History
                '''''''''''''''''''''''''''''''''''''''''''
                PSRModule.InsertManufacturingMetricHistory(ViewState("ReportID"), ViewState("TeamMemberID"), "Submitted Monthly Report for Internal Review")
            End If

            '''''''''''''''''''''''''''''''''''''''''''
            'build list of recipients
            '''''''''''''''''''''''''''''''''''''''''''
            strEmailToAddress = BuildInternalReviewNotificationlist()

            If strEmailToAddress <> "" Then
                strEmailToAddress &= ";"
            End If

            strEmailToAddress &= BuildPlantControllerNotificationList()

            ''''''''''''''''''''''''''''''''''
            ''Build Email
            ''''''''''''''''''''''''''''''''''

            'assign email subject
            strEmailSubject = "Internal Review of " & ddMonth.SelectedItem.Text & " Manufacturing Metric Report for " & ddUGNFacility.SelectedItem.Text

            'build email body
            If ViewState("StatusID") = 0 Or ViewState("StatusID") = 1 Then
                strEmailBody = "<font size='2' face='Verdana'>The following Manufacturing Metric Report is ready for INTERNAL review:</font><br /><br />"
            End If

            If ViewState("StatusID") = 2 Then
                strEmailBody = "<font size='2' face='Verdana'>The following Manufacturing Metric Report is has been updated.</font><br /><br />"
            End If

            strEmailBody &= "<font size='2' face='Verdana'>Month: <b>" & ddMonth.SelectedItem.Text & "</b></font><br /><br />"
            strEmailBody &= "<font size='2' face='Verdana'>UGN Facility: <b>" & ddUGNFacility.SelectedItem.Text & "</b></font><br /><br />"

            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & ViewState("ReportID") & "'>Click here to review</a></font><br /><br />"

            If SendEmail(strEmailToAddress, strEmailSubject, strEmailBody) = True Then
                '    lblMessage.Text &= "Notfication Sent."
                'Else
                '    lblMessage.Text &= "Notfication Failed. Please contact IS."
            End If

            EnableControls()


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnNotifyFinal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNotifyFinal.Click

        Try

            lblMessage.Text = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strEmailURL As String = strProdOrTestEnvironment & "PlantSpecificReports/crPreview_Manufacturing_Metric_Report.aspx?ReportType=M&ReportID="

            Dim strEmailToAddress As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailBody As String = ""

            '''''''''''''''''''''''''''''''''''''''''''
            'Update Monthly Report Status
            '''''''''''''''''''''''''''''''''''''''''''
            ViewState("StatusID") = 3
            ddStatus.SelectedValue = 3
            PSRModule.UpdateManufacturingMetricStatusByDept(ViewState("ReportID"), ViewState("StatusID"))

            '''''''''''''''''''''''''''''''''''''''''''
            'Update Monthly Report History
            '''''''''''''''''''''''''''''''''''''''''''
            PSRModule.InsertManufacturingMetricHistory(ViewState("ReportID"), ViewState("TeamMemberID"), "Submitted Monthly Report for Final Review and Closed the Report")

            '''''''''''''''''''''''''''''''''''''''''''
            'build list of recipients
            '''''''''''''''''''''''''''''''''''''''''''
            strEmailToAddress = BuildFinalReviewNotificationlist()

            If strEmailToAddress <> "" Then
                strEmailToAddress &= ";"
            End If

            strEmailToAddress &= BuildPlantControllerNotificationList()

            ''''''''''''''''''''''''''''''''''
            ''Build Email
            ''''''''''''''''''''''''''''''''''

            'assign email subject
            strEmailSubject = "Final Review of " & ddMonth.SelectedItem.Text & " Manufacturing Metric Report for " & ddUGNFacility.SelectedItem.Text

            'build email body
            strEmailBody = "<font size='2' face='Verdana'>The following Manufacturing Metric Report is ready for FINAL review:</font><br /><br />"

            strEmailBody &= "<font size='2' face='Verdana'>Month: <b>" & ddMonth.SelectedItem.Text & "</b></font><br /><br />"
            strEmailBody &= "<font size='2' face='Verdana'>UGN Facility: <b>" & ddUGNFacility.SelectedItem.Text & "</b></font><br /><br />"

            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & ViewState("ReportID") & "'>Click here to review</a></font><br /><br />"

            If SendEmail(strEmailToAddress, strEmailSubject, strEmailBody) = True Then
                '    lblMessage.Text &= "Notfication Sent."
                'Else
                '    lblMessage.Text &= "Notfication Failed. Please contact IS."
            End If

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Protected Sub btnVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoid.Click

    '    Try

    '        lblMessage.Text = ""

    '        DisableAllControls()

    '        cbIncludeDepartment.Enabled = False

    '        btnCancelVoid.Visible = ViewState("isAdmin")
    '        btnVoid.Attributes.Add("onclick", "")
    '        btnVoid.CausesValidation = True
    '        btnVoid.ValidationGroup = "vgVoid"
    '        btnVoid.Visible = ViewState("isAdmin")

    '        lblVoidReasonCharCount.Visible = True
    '        lblVoidMarker.Visible = True
    '        lblVoidLabel.Visible = True

    '        rfvVoidReason.Enabled = True

    '        txtVoidReason.Visible = True
    '        txtVoidReason.Attributes.Add("onkeypress", "return tbLimit();")
    '        txtVoidReason.Attributes.Add("onkeyup", "return tbCount(" + lblVoidReasonCharCount.ClientID + ");")
    '        txtVoidReason.Attributes.Add("maxLength", "500")

    '        If txtVoidReason.Text <> "" Then
    '            PSRModule.DeleteManufacturingMetric(ViewState("ReportID"), txtVoidReason.Text.Trim)

    '            ViewState("StatusID") = 4
    '            ddStatus.SelectedValue = 4

    '            PSRModule.InsertManufacturingMetricHistory(ViewState("ReportID"), ViewState("TeamMemberID"), "Voided Monthly Report")

    '            EnableControls()
    '        Else
    '            lblMessage.Text += "To void an AR Event, please fill in the Void Reason field."
    '            txtVoidReason.Focus()
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    'Protected Sub btnCancelVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelVoid.Click

    '    Try

    '        lblMessage.Text = ""

    '        DisableAllControls()

    '        EnableControls()

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

End Class
