<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="VendorMaintenance.aspx.vb" Inherits="DataMaintenance_VendorMaintenance"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table>
            <tr>
                <td class="p_text">
                    Vendor ID:
                </td>
                <td style="width: 123px">
                    <asp:DropDownList ID="ddVendorIDSearch" runat="server" AppendDataBoundItems="True" />
                </td>
                <td class="p_text">
                    Vendor Name:
                </td>
                <td>
                    <asp:TextBox ID="txtVendorNameSearch" runat="server" Width="250px" MaxLength="30" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvVendorList" runat="server" AutoGenerateColumns="False" DataKeyNames="Vendor"
            DataSourceID="odsVendorList" AllowPaging="True" Width="600px" AllowSorting="True"
            PageSize="30" SkinID="StandardGridWOFooter">
            <Columns>
                <asp:BoundField DataField="VENDOR" HeaderText="Vendor ID" ReadOnly="True" SortExpression="VENDOR">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="ddVNDNAM" HeaderText="Vendor Name" SortExpression="VNDNAM"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="VNSTAT" HeaderText="BPCS Status" SortExpression="VNSTAT"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="VMID" HeaderText="VMID" SortExpression="VMID" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="VTYPE" HeaderText="Type" SortExpression="VTYPE" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ObjectDataSource ID="odsVendorList" runat="server" SelectMethod="GetVendors"
            TypeName="VendorsBLL" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter Name="Vendor" QueryStringField="VendorID" Type="Int32"
                    DefaultValue="0" />
                <asp:QueryStringParameter Name="VendorName" QueryStringField="VendorName" Type="String"
                    DefaultValue="" />
                <asp:QueryStringParameter Name="VendorAddress" QueryStringField="VendorAddress" Type="String"
                    DefaultValue="" />
                <asp:QueryStringParameter Name="VendorState" QueryStringField="VendorState" Type="String"
                    DefaultValue="" />
                <asp:QueryStringParameter Name="VendorZipCode" QueryStringField="VendorZipCode" Type="String"
                    DefaultValue="" />
                <asp:QueryStringParameter Name="VendorCountry" QueryStringField="VendorCountry" Type="String"
                    DefaultValue="" />
                <asp:QueryStringParameter Name="VendorPhone" QueryStringField="VendorPhone" Type="String"
                    DefaultValue="" />
                <asp:QueryStringParameter Name="VendorFAX" QueryStringField="VendorFAX" Type="String"
                    DefaultValue="" />
                <asp:Parameter Name="VendorType" Type="String" DefaultValue=""/>
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
