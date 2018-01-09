<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="CustomerPartMaintenance.aspx.vb"
    Inherits="DataMaintenance_CustomerPartMaintenance" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" Width="818px" SkinID="MessageLabelSkin" />
        <table>
            <tr>
                <td class="p_text">
                    BPCS Part No:
                </td>
                <td>
                    <asp:TextBox ID="txtBPCSPartNoSearch" runat="server" MaxLength="15" Width="200px" />
                </td>
                <td class="p_text">
                    Customer (CABBV):
                </td>
                <td>
                    <asp:DropDownList ID="ddCABBVSearch" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Customer Part No:
                </td>
                <td>
                    <asp:TextBox ID="txtCustomerPartNoSearch" runat="server" MaxLength="30" Width="200px" />
                </td>
                <td style="height: 15px" class="p_text">
                    Customer Part Name:
                </td>
                <td>
                    <asp:TextBox ID="txtCustomerPartNameSearch" runat="server" MaxLength="50" Width="200px" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Bar Code Part No:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtBarCodePartNoSearch" runat="server" MaxLength="30" Width="200px" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3" align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvCustomerPartList" runat="server" AutoGenerateColumns="False"
            DataSourceID="odsCustomerPartList" AllowPaging="True" AllowSorting="True" PageSize="30"
            SkinID="StandardGridWOFooter">
            <Columns>
                <asp:BoundField DataField="CustomerPartNo" HeaderText="Customer Part No." SortExpression="CustomerPartNo"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="BarCodePartNo" HeaderText="Bar Code Part No." SortExpression="BarCodePartNo" />
                <asp:BoundField DataField="BPCSPartNo" HeaderText="F.G. BPCS Part No." SortExpression="BPCSPartNo"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerPartName" HeaderText="Customer Part Name" SortExpression="CustomerPartName" />
                <asp:BoundField DataField="CABBV" HeaderText="Customer" SortExpression="CABBV" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" Width="498px" />
        &nbsp;
        <asp:ObjectDataSource ID="odsCustomerPartList" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetCustomerParts" TypeName="CustomerPartsBLL">
            <SelectParameters>
                <asp:QueryStringParameter Name="BPCSPartNo" QueryStringField="bpcsPartNo" Type="String" />
                <asp:QueryStringParameter Name="CustomerPartNo" QueryStringField="customerPartNo"
                    Type="String" />
                <asp:QueryStringParameter Name="CustomerPartName" QueryStringField="customerPartName"
                    Type="String" />
                <asp:QueryStringParameter Name="CABBV" QueryStringField="cabbv" Type="String" />
                <asp:QueryStringParameter Name="BarCodePartNo" QueryStringField="barCodePartNo" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
