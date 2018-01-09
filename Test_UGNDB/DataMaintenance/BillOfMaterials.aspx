<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="BillOfMaterials.aspx.vb" Inherits="DataMaintenance_BillOfMaterials"
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
                    Internal Part No:
                </td>
                <td>
                    <asp:TextBox ID="txtPartNoSearch" runat="server" MaxLength="15"></asp:TextBox>
                </td>
                <td class="p_text" nowrap>
                    Sub Level Internal Part No:
                </td>
                <td>
                    <asp:TextBox ID="txtSubPartNoSearch" runat="server" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
               
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvBillOfMaterialsList" runat="server" AutoGenerateColumns="False"
            DataKeyNames="PartNo,SubPartNo" DataSourceID="odsBillOfMaterialsList"
            AllowPaging="True" Width="650px" AllowSorting="True" SkinID="StandardGridWOFooter"
            PageSize="30">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="PartNo" DataNavigateUrlFormatString="BillOfMaterialsTree.aspx?PartNo={0}"
                    DataTextField="PartNo" HeaderText="Internal Part No" SortExpression="PartNo" />
                <asp:BoundField DataField="SubPartNo" HeaderText="Sub Level Internal Part No" ReadOnly="True"
                    SortExpression="SubPartNo">
                    <ItemStyle HorizontalAlign="left" />
                </asp:BoundField>
                <asp:BoundField DataField="Quantity" HeaderText="Build Required" ReadOnly="True"
                    SortExpression="Quantity">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditSubFamilyInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EditSubFamilyInfo" Height="35px" />
        <asp:ValidationSummary ID="vsEmptySubFamilyInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EmptySubFamilyInfo" />
        <asp:ObjectDataSource ID="odsBillOfMaterialsList" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetBillOfMaterials" TypeName="BillOfMaterials">
            <SelectParameters>
                <asp:QueryStringParameter Name="PartNo" QueryStringField="PartNo" Type="String" />
                <asp:QueryStringParameter Name="SubPartNo" QueryStringField="SubPartNo" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
