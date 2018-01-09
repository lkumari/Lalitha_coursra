<%@ Page Language="VB" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="Manufacturing_Metric_Calculation_Sources.aspx.vb" Inherits="PlantSpecificReports_Manufacturing_Metric_Calculation_Sources" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manufacturing Metric Calculation Sources</title>
</head>
<body>
    <asp:Panel ID="localPanel" runat="server">
        <form id="form1" runat="server">
        <div>
            <br />
            <br />
            <br />
            <a href="javascript:window.print()">
                <img src="../images/printer.jpg" alt="Print" style="border: 0" />Click to Print This Page</a><br />
            <br />
            <br />
            <br />
            <h1>
                Calculations for Department
                <asp:Label runat="server" ID="lblDeptID"></asp:Label></h1>
            <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
            <table border="0" width="98%">
                <tr>
                    <td class="c_textbold">
                        Actual OEE (Based on Available Hours):
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblActualOEE" CssClass="c_textbold"></asp:Label>%
                        = (<asp:Label runat="server" ID="lblOEEActualGoodPartCount1"></asp:Label>
                        /
                        <asp:Label runat="server" ID="lblOEEActualTotalPartCount1"></asp:Label>) *
                        <asp:Label runat="server" ID="lblOEEActualUtilization1"></asp:Label> * 
                        (<asp:Label runat="server" ID="lblActualMachineStandardHours1"></asp:Label> /
                        <asp:Label runat="server" ID="lblActualMachineHours5"></asp:Label>) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Actual OEE = (OEEActualGoodPartCount / OEEActualTotalPartCount) * OEEActualUtilization
                        * (Actual Machine Earned Hours / Actual Machine Hours Worked)
                        * 100
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Budget OEE (Based on Available Hours):
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblBudgetOEE" CssClass="c_textbold"></asp:Label>%
                        = (<asp:Label runat="server" ID="lblOEEBudgetGoodPartCount1"></asp:Label>
                        /
                        <asp:Label runat="server" ID="lblOEEBudgetTotalPartCount1"></asp:Label>) *
                        <asp:Label runat="server" ID="lblOEEBudgetUtilization1"></asp:Label> * 
                        (<asp:Label runat="server" ID="lblBudgetMachineStandardHours1"></asp:Label>/
                        <asp:Label runat="server" ID="lblBudgetMachineHours4"></asp:Label>) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Budget OEE = (OEEBudgetGoodPartCount / OEEBudgetTotalPartCount) * OEEBudgetUtilization
                        * (Budget Machine Earned Hours / Budget Machine Hours Worked)
                        * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        OEE Actual Good Part Count:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOEEActualGoodPartCount2" CssClass="c_textbold"></asp:Label>
                        =
                        <asp:Label runat="server" ID="lblOEEActualTotalPartCount2"></asp:Label>
                        -
                        <asp:Label runat="server" ID="lblOEEActualScrapPartCount1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        OEE Actual Good Part Count = OEE Actual Total Part Count - OEE Actual Scrap Part
                        Count
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        OEE Actual Scrap Part Count
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOEEActualScrapPartCount2" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        (See Scrap Dollare By Department, RIEM203B - Column: Scrap Quantity)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        OEE Actual Total Part Count
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOEEActualTotalPartCount3" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        (See Scrap Dollar By Department, RIEM203B - Column: Production Dollars)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        OEE Actual Available Hours
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOEEActualAvailableHours3" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        (OEEActualAvailableHours=Machine Hours Available)<br />
                        (See Below)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        OEE Actual Unscheduled Down Hours
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOEEActualDownHours2" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        (OEE Actual Unscheduled Down Hours = UNscheduled Machine Down Time)<br />
                        (See Below)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Actual Machine Hours
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblActualMachineHours1" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        (See Daily Efficiency Report, RIEM214B - Column: Machine Hours Actual)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        OEE Actual Utilization
                    </td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblOEEActualUtilization2" CssClass="c_textbold"></asp:Label>%
                        = (<asp:Label runat="server" ID="lblActualMachineHours2"></asp:Label>
                        /
                        <asp:Label runat="server" ID="lblOEEActualAvailableHours4"></asp:Label>) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        OEE Actual Utilization = (Actual Machine Hours / OEE Actual Available Hours) * 100
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        OEE Budget Utilization
                    </td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblOEEBudgetUtilization2" CssClass="c_textbold"></asp:Label>%
                        = (<asp:Label runat="server" ID="lblBudgetMachineHours1"></asp:Label>
                        /
                        <asp:Label runat="server" ID="lblOEEBudgetAvailableHours4"></asp:Label>) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        OEE Budget Utilization = (Budget Machine Hours / OEE Budget Available Hours) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Earned Direct Labor Hours:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblActualEarnedDLHours1" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        (See Daily Efficiency Report, RIEM214B - Column: Man Hours Standard)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Actual Direct Labor Hours:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblActualDLHours1" CssClass="c_textbold"></asp:Label>
                        =
                        <asp:Label runat="server" ID="lblActualManHoursWorked1"></asp:Label>
                        +
                        <asp:Label runat="server" ID="lblTotalActualManHourDowntime1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Actual DL Hours = Actual Man Hours Worked + Total Actual Man Hours Downtime
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Budget Direct Labor Hours:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblBudgetDLHours1" CssClass="c_textbold"></asp:Label>
                        =
                        <asp:Label runat="server" ID="lblBudgetManHoursWorked1"></asp:Label>
                        +
                        <asp:Label runat="server" ID="lblTotalBudgetManHourDowntime1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Budget DL Hours = Budget Man Hours Worked + Total Budget Man Hours Downtime
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Actual Man Hours Worked
                    </td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblActualManHoursWorked2" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        (See Daily Efficiency Report, RIEM214B - Column: Man Hours Actual)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Total Actual Man Hours Downtime
                    </td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblTotalActualManHourDowntime2" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        (See Below)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Actual Downtime Hours
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblActualDowntimeHours1" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        (See Downtime Hours Report, RIEM205B - Column: MDT Downtime)<br />
                        or<br />
                        (See Daily Efficiency Report, RIEM214B - Column: Downtime Hours)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Budget Machine Earned Hours
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblBudgetMachineStandardHours2" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Actual Machine Earned Hours
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblActualMachineStandardHours2" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        (See Daily Efficiency Report, RIEM214B - Column: Machine Hours Standard)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Actual Net Variance:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblActualDLHoursNetVariance" CssClass="c_textbold"></asp:Label>
                        =
                        <asp:Label runat="server" ID="lblActualEarnedDLHours2"></asp:Label>
                        -
                        <asp:Label runat="server" ID="lblActualDLHours2"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Actual Net Variance = Earned DL Hours - Actual DL Hours
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Budget Net Variance:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblBudgetDLHoursNetVariance" CssClass="c_textbold"></asp:Label>
                        =
                        <asp:Label runat="server" ID="lblBudgetEarnedDLHours1"></asp:Label>
                        -
                        <asp:Label runat="server" ID="lblBudgetDLHours2"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Budget Net Variance = Earned DL Hours - Budget DL Hours
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Actual Labor Productivty:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblActualLaborProductivity" CssClass="c_textbold"></asp:Label>%
                        = (<asp:Label runat="server" ID="lblActualEarnedDLHours3"></asp:Label>
                        /
                        <asp:Label runat="server" ID="lblActualDLHours3"></asp:Label>) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Actual Labor Productivity = (Actual Earned DL Hours / Actual DL Hours) * 100
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Budget Labor Productivty:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblBudgetLaborProductivity" CssClass="c_textbold"></asp:Label>%
                        = (<asp:Label runat="server" ID="lblBudgetEarnedDLHours2"></asp:Label>
                        /
                        <asp:Label runat="server" ID="lblBudgetDLHours3"></asp:Label>) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Budget Labor Productivity = (Budget Earned DL Hours / Budget DL Hours) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Actual Machine Utilization:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblActualMachineUtilization" CssClass="c_textbold"></asp:Label>%
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        (Actual Machine Utilization = OEE Actual Utilization)<br />
                        (See above)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Actual F.G.Scrap as a percentage of Cost of Production:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblActualScrapPercent" CssClass="c_textbold"></asp:Label>%
                        = ((<asp:Label runat="server" ID="lblTotalActualSpecificScrapDollar"></asp:Label>
                        +
                        <asp:Label runat="server" ID="lblTotalActualMiscScrapDollar"></asp:Label>
                        +
                        <asp:Label runat="server" ID="lblTotalActualIndirectScrapDollar"></asp:Label>) /
                        <asp:Label runat="server" ID="lblTotalActualProductionDollar1"></asp:Label>) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Actual F.G. Scrap = ((Total Actual (S) Finished Scrap Dollars + Total Actual (SM) Misc Scrap Dollars
                        + Total Actual Indirect Scrap Dollars ) / Total Actual Production Dollars )* 100<br />
                        (Rounded to 1 decimal)
                    </td>
                </tr>
                <tr>
                    <td class="c_textbold">
                        Budget F.G. Scrap as a percentage of Cost of Production:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblBudgetScrapPercent" CssClass="c_textbold"></asp:Label>%
                        = ((<asp:Label runat="server" ID="lblTotalBudgetSpecificScrapDollar"></asp:Label>
                        +
                        <asp:Label runat="server" ID="lblTotalBudgetMiscScrapDollar"></asp:Label>) /
                        <asp:Label runat="server" ID="lblTotalBudgetProductionDollar1"></asp:Label>) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Budget F.G. Scrap = ((Total Budget (S) Finished Scrap Dollars + Total Budget (SM) Misc Scrap Dollars
                        ) / Total Budget Production Dollars )* 100<br />
                        (Rounded to 1 decimal)
                    </td>
                </tr>
                 <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                 <tr>
                    <td class="c_textbold">
                        Actual In-Process Scrap as a percentage of Cost of Production:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblActualRawWipScrapPercent1" CssClass="c_textbold"></asp:Label>%
                        = 
                        (<asp:Label runat="server" ID="lblTotalActualRawWipScrapDollar1"></asp:Label>
                        /
                        <asp:Label runat="server" ID="lblTotalActualProductionDollar2"></asp:Label>) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Actual In-Process Scrap = (Total Actual (I) In-Process Scrap Dollars / Total Actual Production Dollars )* 100<br />
                        (Rounded to 1 decimal)
                    </td>
                </tr>
                 <tr>
                    <td class="c_textbold">
                        Budget In-Process Scrap as a percentage of Cost of Production:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblBudgetRawWipScrapPercent1" CssClass="c_textbold"></asp:Label>%
                        = 
                        (<asp:Label runat="server" ID="lblTotalBudgetRawWipScrapDollar1"></asp:Label>
                        /
                        <asp:Label runat="server" ID="lblTotalBudgetProductionDollar2"></asp:Label>) * 100
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p_smalltextbold">
                        Budget In-Process Scrap = (Total Budget (I) In-Process Scrap Dollars / Total Budget Production Dollars )* 100<br />
                        (Rounded to 1 decimal)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="c_textbold">
                        OEE Actual Available Hours
                    </td>
                </tr>
                <tr runat="server" id="rowOEEActualAvailableHoursTotal">
                    <td class="c_textbold">
                        Total OEE Actual Available Hours for all Departments
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOEEActualAvailableHours6" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table runat="server" id="tblOEEActualAvailableHoursByDept">
                            <tr>
                                <td>
                                    Actual Machine Hours Worked
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblActualMachineHours3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Actual Downtime Hours
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblActualDowntimeHours2"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Hours Per Shift = (Number of Working Days * 8)
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblHoursPerShift1"></asp:Label>
                                    =
                                    <asp:Label runat="server" ID="lblMonthlyShippingDays1"></asp:Label>
                                    * 8
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Rounded Actual Shift Count = (Actual Machine Hours Worked + Actual Downtime Hours)
                                    / Hours Per Shift
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblActualShiftCount1"></asp:Label>
                                    = (<asp:Label runat="server" ID="lblActualMachineHours4"></asp:Label>
                                    +
                                    <asp:Label runat="server" ID="lblActualDowntimeHours3"></asp:Label>) /
                                    <asp:Label runat="server" ID="lblHoursPerShift2"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Number of Working Days
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMonthlyShippingDays2"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Available per shift Factor
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblAvailablePerShiftFactor1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    OEE Actual Available Hours = Shift Count * Monthly Shipping Days * Available Per
                                    Shift Factor
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOEEActualAvailableHours5" CssClass="c_textbold"></asp:Label>
                                    =
                                    <asp:Label runat="server" ID="lblMonthlyShippingDays3"></asp:Label>
                                    *
                                    <asp:Label runat="server" ID="lblAvailablePerShiftFactor2"></asp:Label>
                                    *
                                    <asp:Label runat="server" ID="lblActualShiftCount2"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="c_textbold">
                        OEE Budget Available Hours
                    </td>
                </tr>
                <tr runat="server" id="rowOEEBudgetAvailableHoursTotal">
                    <td class="c_textbold">
                        Total OEE Budget Available Hours for all Departments
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOEEBudgetAvailableHours6" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table runat="server" id="tblOEEBudgetAvailableHoursByDept">
                            <tr>
                                <td>
                                    Budget Machine Hours Worked
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblBudgetMachineHours2"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Budget Downtime Hours
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblBudgetDowntimeHours1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Hours Per Shift = (Number of Working Days * 8)
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblHoursPerShift3"></asp:Label>
                                    =
                                    <asp:Label runat="server" ID="lblMonthlyShippingDays4"></asp:Label>
                                    * 8
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Rounded Budget Shift Count = (Budget Machine Hours Worked + Budget Downtime Hours)
                                    / Hours Per Shift
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblBudgetShiftCount1"></asp:Label>
                                    = (<asp:Label runat="server" ID="lblBudgetMachineHours3"></asp:Label>
                                    +
                                    <asp:Label runat="server" ID="lblBudgetDowntimeHours2"></asp:Label>) /
                                    <asp:Label runat="server" ID="lblHoursPerShift4"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Number of Working Days
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMonthlyShippingDays5"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Available per shift Factor
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblAvailablePerShiftFactor3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    OEE Budget Available Hours = Shift Count * Monthly Shipping Days * Available Per
                                    Shift Factor
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOEEBudgetAvailableHours5" CssClass="c_textbold"></asp:Label>
                                    =
                                    <asp:Label runat="server" ID="lblMonthlyShippingDays6"></asp:Label>
                                    *
                                    <asp:Label runat="server" ID="lblAvailablePerShiftFactor4"></asp:Label>
                                    *
                                    <asp:Label runat="server" ID="lblBudgetShiftCount2"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="c_textbold">
                        UNscheduled Machine Down Time (OEE Actual Down Hours)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView runat="server" ID="gvOEEActualDownHours" DataSourceID="odsMachineDowntime"
                            AutoGenerateColumns="False" PageSize="10000" AllowPaging="true" Width="98%" ShowFooter="false"
                            EmptyDataText="No Machine Downtime Hours Found">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="TTDTE" ReadOnly="True" HeaderText="Date" SortExpression="TTDTE">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TSHFT" ReadOnly="True" HeaderText="Shift" SortExpression="TSHFT">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TFRES" ReadOnly="True" HeaderText="Reason Code" SortExpression="TFRES">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DATA" ReadOnly="True" HeaderText="Reason Desc" SortExpression="DATA">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="THRS" ReadOnly="True" HeaderText="Hours" SortExpression="THRS">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsMachineDowntime" runat="server" SelectMethod="GetManufacturingMetricMachineHourDowntimeDetailByDept"
                            TypeName="PSRModule">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="DeptID" QueryStringField="DeptID" Type="Int32" />
                                <asp:QueryStringParameter Name="UGNFacility" QueryStringField="UGNFacility" Type="String" />
                                <asp:QueryStringParameter Name="StartDate" QueryStringField="StartDate" Type="String" />
                                <asp:QueryStringParameter Name="EndDate" QueryStringField="EndDate" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                        Total Unscheduled Machine Down Time (OEE Actual Unscheduled Down Hours)
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOEEActualDownHours4" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="c_textbold">
                        Man Hours Downtime (Scheduled and Unscheduled)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView runat="server" ID="gvManHourDowntime" DataSourceID="odsManHourDowntime"
                            AutoGenerateColumns="False" PageSize="10000" AllowPaging="true" Width="98%" ShowFooter="false"
                            EmptyDataText="No Scheduled or Unscheduled Man Hour Downtime Found">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="DateWorked" ReadOnly="True" HeaderText="Date" SortExpression="DateWorked">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="isScheduled" ReadOnly="True" HeaderText="Is Scheduled"
                                    SortExpression="isScheduled">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Shift" ReadOnly="True" HeaderText="Shift" SortExpression="Shift">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ActualManHours" ReadOnly="True" HeaderText="Actual Man Hours"
                                    SortExpression="ActualManHours">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ActualMachineHours" ReadOnly="True" HeaderText="Actual Machine Hours"
                                    SortExpression="ActualMachineHours">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MachineHoursDowntime" ReadOnly="True" HeaderText="Machine Hours Downtime"
                                    SortExpression="MachineHoursDowntime  ">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CrewSize" ReadOnly="True" HeaderText="Crew Size = Actual Man Hours / Actual Machine Hours"
                                    SortExpression="CrewSize">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ManHourDowntime" ReadOnly="True" HeaderText="Man Hour Down Time = Crew Size * Machine Hours Down Teme"
                                    SortExpression="ManHourDowntime">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsManHourDowntime" runat="server" SelectMethod="GetManufacturingMetricManHourDowntimeAllShiftAllScheduleDetailByDept"
                            TypeName="PSRModule">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="DeptID" QueryStringField="DeptID" Type="Int32" />
                                <asp:QueryStringParameter Name="UGNFacility" QueryStringField="UGNFacility" Type="String" />
                                <asp:QueryStringParameter Name="StartDate" QueryStringField="StartDate" Type="String" />
                                <asp:QueryStringParameter Name="EndDate" QueryStringField="EndDate" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                        Total Actual Man Hours Downtime (Scheduled and Unscheduled)
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblTotalActualManHourDowntime3" CssClass="c_textbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="c_textbold">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="c_textbold">
                        Total Actual Indirect Misc Scrap Dollars (SM Transactions that do not tie to a Department)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView runat="server" ID="gvMiscScrapDollarNoDepartment" DataSourceID="odsMiscScrapDollarNoDepartment"
                            AutoGenerateColumns="False" PageSize="10000" AllowPaging="true" Width="98%" ShowFooter="false"
                            EmptyDataText="No Misc Scrap Dollar Without Department Found. All SM Transactions relate to Departments">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="PartNo" ReadOnly="True" HeaderText="PartNo" SortExpression="PartNo">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TotalQuantity" ReadOnly="True" HeaderText="Total Quantity"
                                    SortExpression="TotalQuantity">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TotalDollar" ReadOnly="True" HeaderText="Total Dollar"
                                    SortExpression="TotalDollar">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsMiscScrapDollarNoDepartment" runat="server" SelectMethod="GetManufacturingMetricMiscScrapDollarNoDept"
                            TypeName="PSRModule">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="UGNFacility" QueryStringField="UGNFacility" Type="String" />
                                <asp:QueryStringParameter Name="StartDate" QueryStringField="StartDate" Type="String" />
                                <asp:QueryStringParameter Name="EndDate" QueryStringField="EndDate" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </div>
        </form>
    </asp:Panel>
</body>
</html>
