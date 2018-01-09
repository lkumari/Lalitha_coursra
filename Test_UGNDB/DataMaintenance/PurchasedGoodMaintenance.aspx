<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="PurchasedGoodMaintenance.aspx.vb" Inherits="DataMaintenance_PurchasedGoodMaint"
    Title="Untitled Page" %>

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
                    <asp:Label ID="lblPurchasedGood" runat="server" Text=" Purchased Good Name:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPurchasedGoodNameSearch" runat="server" Width="200px" MaxLength="50"/>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvPurchasedGoodList" runat="server" AutoGenerateColumns="False"
            DataKeyNames="PurchasedGoodID" DataSourceID="odsPurchasedGoodList" Width="600px"
            OnRowCommand="gvPurchasedGoodList_RowCommand" SkinID="StandardGrid" PageSize="30">
            <Columns>
                <asp:BoundField DataField="PurchasedGoodID" HeaderText="Purchased Good ID" ReadOnly="True"
                    SortExpression="PurchasedGoodID" Visible="False" />
                <asp:TemplateField HeaderText="Purchased Good Name" SortExpression="PurchasedGoodName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPurchasedGoodName" runat="server" MaxLength="30" Text='<%# Bind("PurchasedGoodName") %>'
                            Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPurchasedGoodName" runat="server" ControlToValidate="txtPurchasedGoodName"
                            Display="Dynamic" ErrorMessage="PurchasedGood Name is Required for Update." Font-Bold="True"
                            Font-Size="Medium" ValidationGroup="EditPurchasedGoodInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPurchasedGoodNamePreEdit" runat="server" Text='<%# Bind("ddPurchasedGoodName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtPurchasedGoodNameInsert" runat="server" MaxLength="30" ValidationGroup="InsertPurchasedGoodInfo"
                            Width="200px"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="rfvPurchasedGoodNameInsert" runat="server" ControlToValidate="txtPurchasedGoodNameInsert"
                            ErrorMessage="PurchasedGood Name is Required for Insert" ValidationGroup="InsertPurchasedGoodInfo">
                        <
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkObsoleteEdit" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkObsoletePreEdit" runat="server" Checked='<%# Bind("Obsolete") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Update" ReadOnly="True"
                    SortExpression="comboUpdateInfo">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update"
                            ValidationGroup="EditPurchasedGoodInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" ValidationGroup="EditPurchasedGoodInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            ValidationGroup="InsertPurchasedGoodInfo" runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg"
                            AlternateText="Insert" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditPurchasedGoodInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EditPurchasedGoodInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertPurchasedGoodInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="InsertPurchasedGoodInfo" />
        <br />
        <asp:ValidationSummary ID="vsEmptyPurchasedGoodInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EmptyPurchasedGoodInfo" />
        <asp:ObjectDataSource ID="odsPurchasedGoodList" runat="server" SelectMethod="GetPurchasedGoods"
            TypeName="PurchasedGoodsBLL" UpdateMethod="UpdatePurchasedGood" InsertMethod="InsertPurchasedGood"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter Name="PurchasedGoodName" QueryStringField="PurchasedGoodName"
                    Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="PurchasedGoodName" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="UpdatedBy" Type="String" />
                <asp:Parameter Name="original_PurchasedGoodID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="PurchasedGoodName" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
