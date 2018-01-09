<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Lock_in_Sales_Projection.aspx.vb" Inherits="PF_Lock_in_Sales_Projection"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">

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
                    ControlToValidate="ddYear" Font-Bold="True"><</asp:RequiredFieldValidator>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="p_text" style="height: 24px">
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="* "></asp:Label>
                Record Type:</td>
            <td style="height: 24px">
                <asp:DropDownList ID="ddRecordType" runat="server" AutoPostBack="true">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem>Budget</asp:ListItem>
                    <asp:ListItem>Forecast</asp:ListItem>
                    <asp:ListItem>Forecast (Financial)</asp:ListItem>
                    <asp:ListItem>Preliminary</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvRecordType" runat="server" ControlToValidate="ddRecordType"
                    ErrorMessage="Record Type is a required field." Font-Bold="True"><</asp:RequiredFieldValidator></td>
            <td style="height: 24px">
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
            <td>
            </td>
            <td colspan="2">
                <asp:Button ID="btnLockIn" runat="server" Text="Lock In Data"  />
                <asp:Button ID="btnReset" runat="server" Text="Reset" /></td>
        </tr>
    </table>
    <br />
    <br />
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Italic="False" Font-Size="Small"
        ForeColor="Red" Text="** NOTE: Once the data is locked and loaded for a specific planning year and record type there is no overwrite."></asp:Label><br />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" />
    <br />
</asp:Content>
