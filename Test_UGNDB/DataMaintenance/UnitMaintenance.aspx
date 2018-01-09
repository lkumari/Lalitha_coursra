<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="UnitMaintenance.aspx.vb" MaintainScrollPositionOnPostback="true" Inherits="DataMaintenance_UnitMaintenance"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="98%" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage" Text=""></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblUnitName" Text="Unit:" CssClass="p_text" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchUnitName" MaxLength="50" />
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblUnitAbbr" Text="Abbreviation:" CssClass="p_text" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchUnitAbbr" MaxLength="10" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <asp:ValidationSummary ID="vsEditUnit" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditUnit" />
        <asp:ValidationSummary ID="vsFooterUnit" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterUnit" />
        <asp:GridView runat="server" ID="gvUnit" DataKeyNames="UnitID" AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" PageSize="30" DataSourceID="odsUnit"
            Width="600px" SkinID="StandardGrid">
            <Columns>
                <asp:BoundField DataField="UnitID" HeaderText="Unit ID" SortExpression="UnitID" />
                <asp:TemplateField HeaderText="Unit" SortExpression="UnitName" HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditUnitName" runat="server" MaxLength="50" Width="300px" Text='<%# Bind("UnitName") %>' />
                        <asp:RequiredFieldValidator ID="rfvEditUnitName" runat="server" ControlToValidate="txtEditUnitName"
                            ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgEditUnit"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewUnitName" runat="server" Text='<%# Bind("ddUnitName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFooterUnitName" runat="server" MaxLength="50" Width="300px" Text='<%# Bind("UnitName") %>' />
                        <asp:RequiredFieldValidator ID="rfvFooterUnitName" runat="server" ControlToValidate="txtFooterUnitName"
                            ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgFooterUnit"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Abbreviation" SortExpression="UnitAbbr" HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditUnitAbbr" runat="server" MaxLength="10" Width="75px" Text='<%# Bind("UnitAbbr") %>' />
                        <asp:RequiredFieldValidator ID="rfvEditUnitAbbr" runat="server" ControlToValidate="txtEditUnitAbbr"
                            ErrorMessage="The abbreviation is required." Font-Bold="True" ValidationGroup="vgEditUnit"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewUnitAbbr" runat="server" Text='<%# Bind("ddUnitAbbr") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFooterUnitAbbr" runat="server" MaxLength="10" Width="75px" Text='<%# Bind("UnitAbbr") %>' />
                        <asp:RequiredFieldValidator ID="rfvFooterUnitAbbr" runat="server" ControlToValidate="txtFooterUnitAbbr"
                            ErrorMessage="The abbreviation is required." Font-Bold="True" ValidationGroup="vgFooterUnit"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbViewObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:CheckBox ID="cbFooterObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnUnitUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditUnit" />
                        <asp:ImageButton ID="iBtnUnitCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnUnitEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterUnit"
                            runat="server" ID="iBtnFooterUnit" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnUnitUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsUnit" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetUnit" TypeName="UnitBLL" UpdateMethod="UpdateUnit" InsertMethod="InsertUnit">
            <UpdateParameters>
                <asp:Parameter Name="original_UnitID" Type="Int32" />
                <asp:Parameter Name="UnitName" Type="String" />
                <asp:Parameter Name="UnitAbbr" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="ddUnitName" Type="String" />
                <asp:Parameter Name="ddUnitAbbr" Type="String" />
                <asp:Parameter Name="UnitID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="UnitID" Type="Int32" DefaultValue="0" />
                <asp:QueryStringParameter Name="UnitName" QueryStringField="UnitName" Type="String" />
                <asp:QueryStringParameter Name="UnitAbbr" QueryStringField="UnitAbbr" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="UnitName" Type="String" />
                <asp:Parameter Name="UnitAbbr" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
