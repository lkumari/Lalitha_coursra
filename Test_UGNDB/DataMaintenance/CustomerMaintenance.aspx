<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CustomerMaintenance.aspx.vb" Inherits="DataMaintenance_CustomerMaintenance"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblMessage" runat="server" Width="818px" SkinID="MessageLabelSkin" />
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblOEM" runat="server" Text="OEM:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddOEM" runat="server" Width="120px" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblUGNFacility" runat="server" Text="UGN Facility:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddCOMPNY" runat="server" Width="120px" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblCustomer" runat="server" Text=" Customer (CABBV):" />
                </td>
                <td>
                    <asp:DropDownList ID="ddCABBV" runat="server" Width="120px" />
                </td>
               <%-- <td class="p_text">
                    <asp:Label ID="lblDestination" runat="server" Text="Destination (DABBV):" />
                </td>
                <td>
                    <asp:DropDownList ID="ddDABBV" runat="server" Width="120px" />
                </td>--%>
            </tr>
           <%-- <tr>
                <td class="p_text">
                    <asp:Label ID="lblShipTo" runat="server" Text="Ship To:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddShipTo" runat="server" Width="120px" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblSoldTo" runat="server" Text="Sold To:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddSoldTo" runat="server" Width="120px" />
                </td>
            </tr>--%>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvCustomerList" runat="server" AutoGenerateColumns="False" DataSourceID="odsCustomerList"
            AllowPaging="True" AllowSorting="True" SkinID="StandardGridWOFooter" PageSize="30">
            <Columns>
                <asp:BoundField DataField="UGNFacilityName" HeaderText="UGN Facility" SortExpression="UGNFacilityName">
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="OEM" HeaderText="OEM" SortExpression="OEM" ReadOnly="True" />
                <asp:BoundField DataField="ddCABBV" HeaderText="Customer" SortExpression="CABBV"
                    ReadOnly="True" />
                <asp:BoundField DataField="SOLDTO" HeaderText="SOLD TO" SortExpression="SOLDTO" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="DABBV" HeaderText="Destination" SortExpression="DABBV"
                    ReadOnly="True" />
                <asp:BoundField DataField="SHIPTO" HeaderText="SHIP TO" SortExpression="SHIPTO" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CNME" HeaderText="Name" SortExpression="CNME" />
                <asp:BoundField DataField="CAD1" HeaderText="Address" SortExpression="CAD1" />
                <asp:BoundField DataField="CAD2" HeaderText="Address Line 2" SortExpression="CAD2">
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CAD3" HeaderText="City" SortExpression="CAD3" />
                <asp:BoundField DataField="CSTE" HeaderText="State" SortExpression="CSTE" />
                <asp:BoundField DataField="CZIP" HeaderText="Zip Code" SortExpression="CZIP" />
                <asp:BoundField DataField="CCOUN" HeaderText="Country" SortExpression="CCOUN" />
                <asp:BoundField DataField="CMIDName" HeaderText="Status" SortExpression="CMIDName">
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" Width="498px" />
        <asp:ObjectDataSource ID="odsCustomerList" runat="server" SelectMethod="GetCustomers"
            TypeName="CustomersBLL" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="" Name="COMPNY" QueryStringField="compny"
                    Type="String" />
                <asp:QueryStringParameter DefaultValue="" Name="OEM" QueryStringField="oem" Type="String" />
                <asp:QueryStringParameter Name="CABBV" QueryStringField="cabbv" Type="String" />
                <asp:QueryStringParameter DefaultValue="" Name="DABBV" QueryStringField="dabbv" Type="String" />
                <asp:QueryStringParameter DefaultValue="0" Name="SHIPTO" QueryStringField="shipto"
                    Type="Int32" />
                <asp:QueryStringParameter DefaultValue="0" Name="SOLDTO" QueryStringField="soldto"
                    Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
