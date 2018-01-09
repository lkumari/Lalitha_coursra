' ************************************************************************************************
'
' Name:		Manufacturing_Metric_Calculation_Sources.aspx
' Purpose:	This Code Behind is for the pop up to show sources of date used in the the Manufacturing Metric Module in Plant Specific Reports
'
' Date		    Author	    
' 08/05/2010    Roderick Carlson - Created
' 01/20/2011    Roderick Carlson - Modified - Roll up work center to department
' 03/30/2011    Roderick Carlson - Modified - Added ActualMachineStandardHours and BudgetMachineStandardHours
' 03/31/2011    Roderick Carlson - Modified - Added BudgetRawWipScrapDollar AND ActualRawWipScrapDollar
' ************************************************************************************************

Partial Class PlantSpecificReports_Manufacturing_Metric_Calculation_Sources
    Inherits System.Web.UI.Page

    Private Sub BindDetailData(ByVal DeptID As Integer)

        Try

            Dim ds As DataSet

            Dim dBudgetDLHours As Double = 0
            Dim dActualDLHours As Double = 0

            Dim dBudgetEarnedDLHours As Double = 0
            Dim dActualEarnedDLHours As Double = 0

            Dim dBudgetLaborProductivity As Double = 0
            Dim dActualLaborProductivity As Double = 0

            Dim dTotalBudgetRawWipScrapDollar As Double = 0
            Dim dTotalActualRawWipScrapDollar As Double = 0

            Dim dTotalBudgetProductionDollar As Double = 0
            Dim dTotalActualProductionDollar As Double = 0

            If DeptID > 0 Then
                'get specific department              
                ds = PSRModule.GetManufacturingMetricDetailByDept(ViewState("ReportID"), DeptID)
            Else
                'get totals of all departments
                ds = PSRModule.GetManufacturingMetricDetailTotalByDept(ViewState("ReportID"))
            End If

            'check for existing data first
            If commonFunctions.CheckDataSet(ds) = True Then

                If ds.Tables(0).Rows(0).Item("BudgetOEE") IsNot System.DBNull.Value Then
                    lblBudgetOEE.Text = Format(ds.Tables(0).Rows(0).Item("BudgetOEE"), "#0.0")
                End If

                If ds.Tables(0).Rows(0).Item("ActualOEE") IsNot System.DBNull.Value Then
                    lblActualOEE.Text = Format(ds.Tables(0).Rows(0).Item("ActualOEE"), "#0.0")
                End If

                If ds.Tables(0).Rows(0).Item("BudgetEarnedDLHours") IsNot System.DBNull.Value Then
                    lblBudgetEarnedDLHours1.Text = Format(ds.Tables(0).Rows(0).Item("BudgetEarnedDLHours"), "#0")
                    lblBudgetEarnedDLHours2.Text = Format(ds.Tables(0).Rows(0).Item("BudgetEarnedDLHours"), "#0")
                    dBudgetEarnedDLHours = CType(ds.Tables(0).Rows(0).Item("BudgetEarnedDLHours"), Integer)
                End If

                If ds.Tables(0).Rows(0).Item("ActualEarnedDLHours") IsNot System.DBNull.Value Then
                    lblActualEarnedDLHours1.Text = Format(ds.Tables(0).Rows(0).Item("ActualEarnedDLHours"), "#0")
                    lblActualEarnedDLHours2.Text = Format(ds.Tables(0).Rows(0).Item("ActualEarnedDLHours"), "#0")
                    lblActualEarnedDLHours3.Text = Format(ds.Tables(0).Rows(0).Item("ActualEarnedDLHours"), "#0")
                    dActualEarnedDLHours = CType(ds.Tables(0).Rows(0).Item("ActualEarnedDLHours"), Integer)
                End If

                If ds.Tables(0).Rows(0).Item("BudgetDLHours") IsNot System.DBNull.Value Then
                    lblBudgetDLHours1.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDLHours"), "#0")
                    lblBudgetDLHours2.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDLHours"), "#0")
                    lblBudgetDLHours3.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDLHours"), "#0")
                    dBudgetDLHours = CType(ds.Tables(0).Rows(0).Item("BudgetDLHours"), Integer)
                End If

                If ds.Tables(0).Rows(0).Item("ActualDLHours") IsNot System.DBNull.Value Then
                    lblActualDLHours1.Text = Format(ds.Tables(0).Rows(0).Item("ActualDLHours"), "#0")
                    lblActualDLHours2.Text = Format(ds.Tables(0).Rows(0).Item("ActualDLHours"), "#0")
                    lblActualDLHours3.Text = Format(ds.Tables(0).Rows(0).Item("ActualDLHours"), "#0")
                    dActualDLHours = CType(ds.Tables(0).Rows(0).Item("ActualDLHours"), Integer)
                End If

                If dBudgetDLHours <> 0 Then
                    dBudgetLaborProductivity = dBudgetEarnedDLHours / dBudgetDLHours
                End If

                lblBudgetDLHoursNetVariance.Text = Format(dBudgetEarnedDLHours - dBudgetDLHours, "###0")
                lblBudgetLaborProductivity.Text = Format(dBudgetLaborProductivity * 100, "###0.0")

                If dActualDLHours <> 0 Then
                    dActualLaborProductivity = dActualEarnedDLHours / dActualDLHours
                End If

                lblActualDLHoursNetVariance.Text = Format(dActualEarnedDLHours - dActualDLHours, "###0")
                lblActualLaborProductivity.Text = Format(dActualLaborProductivity * 100, "###0.0")

                If ds.Tables(0).Rows(0).Item("BudgetScrap") IsNot System.DBNull.Value Then
                    lblBudgetScrapPercent.Text = Format((ds.Tables(0).Rows(0).Item("BudgetScrap")), "#0.0")
                End If

                If ds.Tables(0).Rows(0).Item("ActualScrap") IsNot System.DBNull.Value Then
                    lblActualScrapPercent.Text = Format((ds.Tables(0).Rows(0).Item("ActualScrap")), "#0.0")
                End If

                If ds.Tables(0).Rows(0).Item("OEEBudgetGoodPartCount") IsNot System.DBNull.Value Then
                    lblOEEBudgetGoodPartCount1.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetGoodPartCount"), "#0")
                End If

                If ds.Tables(0).Rows(0).Item("OEEActualGoodPartCount") IsNot System.DBNull.Value Then
                    lblOEEActualGoodPartCount1.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualGoodPartCount"), "#0")
                    lblOEEActualGoodPartCount2.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualGoodPartCount"), "#0")
                End If

                If ds.Tables(0).Rows(0).Item("OEEActualScrapPartCount") IsNot System.DBNull.Value Then
                    lblOEEActualScrapPartCount1.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualScrapPartCount"), "#0")
                    lblOEEActualScrapPartCount2.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualScrapPartCount"), "#0")
                End If

                If ds.Tables(0).Rows(0).Item("OEEBudgetTotalPartCount") IsNot System.DBNull.Value Then
                    lblOEEBudgetTotalPartCount1.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetTotalPartCount"), "#0")
                End If

                If ds.Tables(0).Rows(0).Item("OEEActualTotalPartCount") IsNot System.DBNull.Value Then
                    lblOEEActualTotalPartCount1.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualTotalPartCount"), "#0")
                    lblOEEActualTotalPartCount2.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualTotalPartCount"), "#0")
                    lblOEEActualTotalPartCount3.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualTotalPartCount"), "#0")
                End If

                If DeptID > 0 Then
                    If ds.Tables(0).Rows(0).Item("OEEBudgetUtilization") IsNot System.DBNull.Value Then
                        lblOEEBudgetUtilization1.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetUtilization"), "#0.0")
                        lblOEEBudgetUtilization2.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetUtilization"), "#0.0")
                    End If

                    If ds.Tables(0).Rows(0).Item("OEEActualUtilization") IsNot System.DBNull.Value Then
                        lblOEEActualUtilization1.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualUtilization"), "#0.0")
                        lblOEEActualUtilization2.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualUtilization"), "#0.0")
                        lblActualMachineUtilization.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualUtilization"), "#0.0")
                    End If
                Else
                    If ds.Tables(0).Rows(0).Item("BudgetMachineUtilization") IsNot System.DBNull.Value Then
                        lblOEEBudgetUtilization1.Text = Format(ds.Tables(0).Rows(0).Item("BudgetMachineUtilization"), "#0.0")
                        lblOEEBudgetUtilization2.Text = Format(ds.Tables(0).Rows(0).Item("BudgetMachineUtilization"), "#0.0")
                    End If

                    If ds.Tables(0).Rows(0).Item("ActualMachineUtilization") IsNot System.DBNull.Value Then
                        lblOEEActualUtilization1.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineUtilization"), "#0.0")
                        lblOEEActualUtilization2.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineUtilization"), "#0.0")
                        lblActualMachineUtilization.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineUtilization"), "#0.0")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("OEEBudgetAvailableHours") IsNot System.DBNull.Value Then                    
                    lblOEEBudgetAvailableHours4.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetAvailableHours"), "#0.##")
                    lblOEEBudgetAvailableHours5.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetAvailableHours"), "#0.##")
                    lblOEEBudgetAvailableHours6.Text = Format(ds.Tables(0).Rows(0).Item("OEEBudgetAvailableHours"), "#0.##")
                End If

                If ds.Tables(0).Rows(0).Item("OEEActualAvailableHours") IsNot System.DBNull.Value Then                  
                    lblOEEActualAvailableHours3.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualAvailableHours"), "#0.##")
                    lblOEEActualAvailableHours4.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualAvailableHours"), "#0.##")
                    lblOEEActualAvailableHours5.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualAvailableHours"), "#0.##")
                    lblOEEActualAvailableHours6.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualAvailableHours"), "#0.##")
                End If

                If ds.Tables(0).Rows(0).Item("OEEActualDownHours") IsNot System.DBNull.Value Then
                    lblOEEActualDownHours2.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualDownHours"), "#0.##")
                    lblOEEActualDownHours4.Text = Format(ds.Tables(0).Rows(0).Item("OEEActualDownHours"), "#0.##")
                End If

                If DeptID > 0 Then
                    tblOEEBudgetAvailableHoursByDept.Visible = True
                    tblOEEActualAvailableHoursByDept.Visible = True

                    rowOEEBudgetAvailableHoursTotal.Visible = False
                    rowOEEActualAvailableHoursTotal.Visible = False

                    If ds.Tables(0).Rows(0).Item("MonthlyShippingDays") IsNot System.DBNull.Value Then
                        lblMonthlyShippingDays1.Text = Format(ds.Tables(0).Rows(0).Item("MonthlyShippingDays"), "#0")
                        lblMonthlyShippingDays2.Text = Format(ds.Tables(0).Rows(0).Item("MonthlyShippingDays"), "#0")
                        lblMonthlyShippingDays3.Text = Format(ds.Tables(0).Rows(0).Item("MonthlyShippingDays"), "#0")
                        lblMonthlyShippingDays4.Text = Format(ds.Tables(0).Rows(0).Item("MonthlyShippingDays"), "#0")
                        lblMonthlyShippingDays5.Text = Format(ds.Tables(0).Rows(0).Item("MonthlyShippingDays"), "#0")
                        lblMonthlyShippingDays6.Text = Format(ds.Tables(0).Rows(0).Item("MonthlyShippingDays"), "#0")
                    End If

                    If ds.Tables(0).Rows(0).Item("HoursPerShift") IsNot System.DBNull.Value Then
                        lblHoursPerShift1.Text = Format(ds.Tables(0).Rows(0).Item("HoursPerShift"), "#0.##")
                        lblHoursPerShift2.Text = Format(ds.Tables(0).Rows(0).Item("HoursPerShift"), "#0.##")
                        lblHoursPerShift3.Text = Format(ds.Tables(0).Rows(0).Item("HoursPerShift"), "#0.##")
                        lblHoursPerShift4.Text = Format(ds.Tables(0).Rows(0).Item("HoursPerShift"), "#0.##")
                    End If

                    If ds.Tables(0).Rows(0).Item("BudgetShiftCount") IsNot System.DBNull.Value Then
                        lblBudgetShiftCount1.Text = Format(ds.Tables(0).Rows(0).Item("BudgetShiftCount"), "#0.##")
                        lblBudgetShiftCount2.Text = Format(ds.Tables(0).Rows(0).Item("BudgetShiftCount"), "#0.##")
                    End If

                    If ds.Tables(0).Rows(0).Item("ActualShiftCount") IsNot System.DBNull.Value Then
                        lblActualShiftCount1.Text = Format(ds.Tables(0).Rows(0).Item("ActualShiftCount"), "#0.##")
                        lblActualShiftCount2.Text = Format(ds.Tables(0).Rows(0).Item("ActualShiftCount"), "#0.##")
                    End If

                    If ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor") IsNot System.DBNull.Value Then
                        lblAvailablePerShiftFactor1.Text = Format(ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor"), "#0.##")
                        lblAvailablePerShiftFactor2.Text = Format(ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor"), "#0.##")
                        lblAvailablePerShiftFactor3.Text = Format(ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor"), "#0.##")
                        lblAvailablePerShiftFactor4.Text = Format(ds.Tables(0).Rows(0).Item("AvailablePerShiftFactor"), "#0.##")
                    End If
                Else
                    tblOEEBudgetAvailableHoursByDept.Visible = False
                    tblOEEActualAvailableHoursByDept.Visible = False

                    rowOEEBudgetAvailableHoursTotal.Visible = True
                    rowOEEActualAvailableHoursTotal.Visible = True
                End If

                If ds.Tables(0).Rows(0).Item("BudgetDowntimeHours") IsNot System.DBNull.Value Then
                    lblBudgetDowntimeHours1.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDowntimeHours"), "#0.##")
                    lblBudgetDowntimeHours2.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDowntimeHours"), "#0.##")
                End If

                If ds.Tables(0).Rows(0).Item("ActualDowntimeHours") IsNot System.DBNull.Value Then
                    lblActualDowntimeHours1.Text = Format(ds.Tables(0).Rows(0).Item("ActualDowntimeHours"), "#0.##")
                    lblActualDowntimeHours2.Text = Format(ds.Tables(0).Rows(0).Item("ActualDowntimeHours"), "#0.##")
                    lblActualDowntimeHours3.Text = Format(ds.Tables(0).Rows(0).Item("ActualDowntimeHours"), "#0.##")
                End If

                If ds.Tables(0).Rows(0).Item("BudgetMachineWorkedHours") IsNot System.DBNull.Value Then
                    lblBudgetMachineHours1.Text = Format(ds.Tables(0).Rows(0).Item("BudgetMachineWorkedHours"), "#0.##")
                    lblBudgetMachineHours2.Text = Format(ds.Tables(0).Rows(0).Item("BudgetMachineWorkedHours"), "#0.##")
                    lblBudgetMachineHours3.Text = Format(ds.Tables(0).Rows(0).Item("BudgetMachineWorkedHours"), "#0.##")
                    lblBudgetMachineHours4.Text = Format(ds.Tables(0).Rows(0).Item("BudgetMachineWorkedHours"), "#0.##")
                End If

                If ds.Tables(0).Rows(0).Item("ActualMachineWorkedHours") IsNot System.DBNull.Value Then
                    lblActualMachineHours1.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineWorkedHours"), "#0.##")
                    lblActualMachineHours2.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineWorkedHours"), "#0.##")
                    lblActualMachineHours3.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineWorkedHours"), "#0.##")
                    lblActualMachineHours4.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineWorkedHours"), "#0.##")
                    lblActualMachineHours5.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineWorkedHours"), "#0.##")
                End If

                lblBudgetMachineStandardHours1.Text = "0"
                lblBudgetMachineStandardHours2.Text = "0"
                If ds.Tables(0).Rows(0).Item("BudgetMachineHourStandard") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("BudgetMachineHourStandard") <> 0 Then                        
                        lblBudgetMachineStandardHours1.Text = Format(ds.Tables(0).Rows(0).Item("BudgetMachineHourStandard"), "#0.##")
                        lblBudgetMachineStandardHours2.Text = Format(ds.Tables(0).Rows(0).Item("BudgetMachineHourStandard"), "#0.##")
                    End If
                End If

                lblActualMachineStandardHours1.Text = "0"
                lblActualMachineStandardHours2.Text = "0"
                If ds.Tables(0).Rows(0).Item("ActualMachineHourStandard") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ActualMachineHourStandard") <> 0 Then
                        lblActualMachineStandardHours1.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineHourStandard"), "#0.##")
                        lblActualMachineStandardHours2.Text = Format(ds.Tables(0).Rows(0).Item("ActualMachineHourStandard"), "#0.##")
                    End If
                End If

                If DeptID > 0 Then
                    If ds.Tables(0).Rows(0).Item("BudgetManWorkedHours") IsNot System.DBNull.Value Then
                        lblBudgetManHoursWorked1.Text = Format(ds.Tables(0).Rows(0).Item("BudgetManWorkedHours"), "#0")
                    End If

                    If ds.Tables(0).Rows(0).Item("ActualManWorkedHours") IsNot System.DBNull.Value Then
                        lblActualManHoursWorked1.Text = Format(ds.Tables(0).Rows(0).Item("ActualManWorkedHours"), "#0")
                        lblActualManHoursWorked2.Text = Format(ds.Tables(0).Rows(0).Item("ActualManWorkedHours"), "#0")
                    End If

                    If ds.Tables(0).Rows(0).Item("BudgetDowntimeManHours") IsNot System.DBNull.Value Then
                        lblTotalBudgetManHourDowntime1.Text = Format(ds.Tables(0).Rows(0).Item("BudgetDowntimeManHours"), "#0")
                    End If

                    If ds.Tables(0).Rows(0).Item("ActualDowntimeManHours") IsNot System.DBNull.Value Then
                        lblTotalActualManHourDowntime1.Text = Format(ds.Tables(0).Rows(0).Item("ActualDowntimeManHours"), "#0")
                        lblTotalActualManHourDowntime2.Text = Format(ds.Tables(0).Rows(0).Item("ActualDowntimeManHours"), "#0")
                        lblTotalActualManHourDowntime3.Text = Format(ds.Tables(0).Rows(0).Item("ActualDowntimeManHours"), "#0")
                    End If
                Else
                    lblBudgetManHoursWorked1.Text = "Total of all Departments Man hours worked"

                    lblActualManHoursWorked1.Text = "Total of all Departments"
                    lblActualManHoursWorked2.Text = "Total of all Departments"

                    lblTotalBudgetManHourDowntime1.Text = "Total of all Departments Man hour downtime"

                    lblTotalActualManHourDowntime1.Text = "Total of all Departments Man hour downtime"
                    lblTotalActualManHourDowntime2.Text = "Total of all Departments Man hour downtime"
                    lblTotalActualManHourDowntime3.Text = "Total of all Departments Man hour downtime"
                End If

                If ds.Tables(0).Rows(0).Item("TotalBudgetProductionDollar") IsNot System.DBNull.Value Then
                    lblTotalBudgetProductionDollar1.Text = Format(ds.Tables(0).Rows(0).Item("TotalBudgetProductionDollar"), "#0.#0")
                    lblTotalBudgetProductionDollar2.Text = Format(ds.Tables(0).Rows(0).Item("TotalBudgetProductionDollar"), "#0.#0")
                    dTotalBudgetProductionDollar = ds.Tables(0).Rows(0).Item("TotalBudgetProductionDollar")
                End If

                If ds.Tables(0).Rows(0).Item("TotalActualProductionDollar") IsNot System.DBNull.Value Then
                    lblTotalActualProductionDollar1.Text = Format(ds.Tables(0).Rows(0).Item("TotalActualProductionDollar"), "#0.#0")
                    lblTotalActualProductionDollar2.Text = Format(ds.Tables(0).Rows(0).Item("TotalActualProductionDollar"), "#0.#0")
                    dTotalActualProductionDollar = ds.Tables(0).Rows(0).Item("TotalActualProductionDollar")
                End If

                If ds.Tables(0).Rows(0).Item("TotalBudgetSpecificScrapDollar") IsNot System.DBNull.Value Then
                    lblTotalBudgetSpecificScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalBudgetSpecificScrapDollar"), "#0.#0")
                End If

                If ds.Tables(0).Rows(0).Item("TotalActualSpecificScrapDollar") IsNot System.DBNull.Value Then
                    lblTotalActualSpecificScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalActualSpecificScrapDollar"), "#0.#0")
                End If

                If ds.Tables(0).Rows(0).Item("TotalBudgetMiscScrapDollar") IsNot System.DBNull.Value Then
                    lblTotalBudgetMiscScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalBudgetMiscScrapDollar"), "#0.#0")
                End If

                If ds.Tables(0).Rows(0).Item("BudgetRawWipScrapDollar") IsNot System.DBNull.Value Then
                    lblTotalBudgetRawWipScrapDollar1.Text = Format(ds.Tables(0).Rows(0).Item("BudgetRawWipScrapDollar"), "#0.#0")
                    dTotalBudgetRawWipScrapDollar = ds.Tables(0).Rows(0).Item("BudgetRawWipScrapDollar")
                End If

                If ds.Tables(0).Rows(0).Item("TotalActualMiscScrapDollar") IsNot System.DBNull.Value Then
                    lblTotalActualMiscScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalActualMiscScrapDollar"), "#0.#0")
                End If

                If ds.Tables(0).Rows(0).Item("TotalActualIndirectScrapDollar") IsNot System.DBNull.Value Then
                    lblTotalActualIndirectScrapDollar.Text = Format(ds.Tables(0).Rows(0).Item("TotalActualIndirectScrapDollar"), "#0.#0")
                End If

                If ds.Tables(0).Rows(0).Item("ActualRawWipScrapDollar") IsNot System.DBNull.Value Then
                    lblTotalActualRawWipScrapDollar1.Text = Format(ds.Tables(0).Rows(0).Item("ActualRawWipScrapDollar"), "#0.#0")
                    dTotalActualRawWipScrapDollar = ds.Tables(0).Rows(0).Item("ActualRawWipScrapDollar")
                End If

                lblBudgetRawWipScrapPercent1.Text = "0.0"
                If dTotalBudgetProductionDollar <> 0 Then
                    lblBudgetRawWipScrapPercent1.Text = Format((dTotalBudgetRawWipScrapDollar / dTotalBudgetProductionDollar) * 100, "#0.0")
                End If

                lblActualRawWipScrapPercent1.Text = "0.0"
                If dTotalActualProductionDollar <> 0 Then
                    lblActualRawWipScrapPercent1.Text = Format((dTotalActualRawWipScrapDollar / dTotalActualProductionDollar) * 100, "#0.0")
                End If

                If DeptID = 0 Then
                    gvOEEActualDownHours.Visible = False
                    gvManHourDowntime.Visible = False
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'MMFieldRef=AvailableTime
            If HttpContext.Current.Request.QueryString("MMFieldRef") <> "" Then

            End If

            If HttpContext.Current.Request.QueryString("ReportID") <> "" Then
                ViewState("ReportID") = CType(HttpContext.Current.Request.QueryString("ReportID"), Integer)
            End If

            If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                ViewState("UGNFacility") = HttpContext.Current.Request.QueryString("UGNFacility")
            End If

            If HttpContext.Current.Request.QueryString("StartDate") <> "" Then
                ViewState("StartDate") = HttpContext.Current.Request.QueryString("StartDate")
            End If

            If HttpContext.Current.Request.QueryString("EndDate") <> "" Then
                ViewState("EndDate") = HttpContext.Current.Request.QueryString("EndDate")
            End If

            If HttpContext.Current.Request.QueryString("DeptID") <> "" Then
                ViewState("DeptID") = CType(HttpContext.Current.Request.QueryString("DeptID"), Integer)

                If ViewState("DeptID") = 0 Then
                    lblDeptID.Text = "Totals (rolled up sum)"
                Else
                    lblDeptID.Text = ViewState("DeptID")
                End If

                BindDetailData(ViewState("DeptID"))
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
