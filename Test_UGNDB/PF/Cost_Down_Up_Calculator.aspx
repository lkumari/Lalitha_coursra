<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Cost_Down_Up_Calculator.aspx.vb" Inherits="PF_Cost_Down_Up_Calculator"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" Font-Size="Medium"></asp:Label><br />
        <hr />
        &nbsp;<br />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="* "></asp:Label>
                    Planning Year:</td>
                <td>
                    <asp:DropDownList ID="ddYear" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvYear" runat="server" ErrorMessage="Planning Year is a required field."
                        ControlToValidate="ddYear" Font-Bold="True" ValidationGroup="vsAdmin"><</asp:RequiredFieldValidator>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 24px">
                    <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="* "></asp:Label>
                    Record Type:</td>
                <td style="height: 24px" colspan="2">
                    <asp:DropDownList ID="ddRecordType" runat="server" AutoPostBack="true">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Budget</asp:ListItem>
                        <asp:ListItem>Current</asp:ListItem>
                        <asp:ListItem>Forecast</asp:ListItem>
                        <asp:ListItem>Forecast (Financial)</asp:ListItem>
                        <asp:ListItem>Preliminary</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvRecordType" runat="server" ControlToValidate="ddRecordType"
                        ErrorMessage="Record Type is a required field." Font-Bold="True" ValidationGroup="vsAdmin"><</asp:RequiredFieldValidator>&nbsp;
                    <asp:DropDownList ID="ddRecordTypeNo" runat="server" AutoPostBack="True">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="p_text">
                    Enter % Decrease or Increase:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtDecInc" runat="server" MaxLength="12" Width="100px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfctxtDecInc" runat="server" ErrorMessage="Enter % Decrease or Increase."
                        ValidationGroup="vsAdmin" ControlToValidate="txtDecInc"><</asp:RequiredFieldValidator><asp:RangeValidator
                            ID="rvDecInc" runat="server" ControlToValidate="txtDecInc" Display="Dynamic"
                            ErrorMessage="Estimate % requires a numeric value" Height="16px" MaximumValue="9999.999999999"
                            MinimumValue="0" Type="Double" ValidationGroup="vsAdmin"><</asp:RangeValidator>
                    <ajax:FilteredTextBoxExtender ID="ftbeDecInc" runat="server" TargetControlID="txtDecInc"
                        FilterType="Custom, Numbers" ValidChars="." />
                    &nbsp;
                    <asp:DropDownList ID="ddDecInc" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Decrease</asp:ListItem>
                        <asp:ListItem>Increase</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvDecInc" runat="server" ErrorMessage="Required selection Decrease or Increase."
                        ValidationGroup="vsAdmin" ControlToValidate="ddDecInc"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Apply Calculation to
                </td>
                <td colspan="6" class="c_text">
                    <asp:RadioButtonList ID="rbCheckMonths" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="True">
                        <asp:ListItem Selected="True" Value="True">Check All</asp:ListItem>
                        <asp:ListItem Value="False">Uncheck All</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td class="c_text" colspan="2" valign="top">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbJan" runat="server" />January
                            </td>
                            <td>
                                <asp:CheckBox ID="cbFeb" runat="server" />February</td>
                            <td>
                                <asp:CheckBox ID="cbMar" runat="server" />March
                            </td>
                            <td>
                                <asp:CheckBox ID="cbApr" runat="server" />April
                            </td>
                            <td>
                                <asp:CheckBox ID="cbMay" runat="server" />May</td>
                            <td>
                                <asp:CheckBox ID="cbJun" runat="server" />June</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbJul" runat="server" />July
                            </td>
                            <td>
                                <asp:CheckBox ID="cbAug" runat="server" />August
                            </td>
                            <td>
                                <asp:CheckBox ID="cbSep" runat="server" />September
                            </td>
                            <td>
                                <asp:CheckBox ID="cbOct" runat="server" />October
                            </td>
                            <td>
                                <asp:CheckBox ID="cbNov" runat="server" />November
                            </td>
                            <td>
                                <asp:CheckBox ID="cbDec" runat="server" />December
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="height: 26px">
                </td>
                <td style="height: 26px" colspan="2">
                    <asp:Button ID="btnCalculate" runat="server" Text="Calculate" CausesValidation="True"
                        ValidationGroup="vsAdmin" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                    <asp:Button ID="btnApply" runat="server" Text="Apply" CausesValidation="True" ValidationGroup="vsAdmin"
                        Visible="False" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vsAdmin" runat="server" ValidationGroup="vsAdmin" ShowMessageBox="true" />
    </asp:Panel>
</asp:Content>
