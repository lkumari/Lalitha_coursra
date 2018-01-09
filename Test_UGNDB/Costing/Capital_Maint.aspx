<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="Capital_Maint.aspx.vb" Inherits="Capital_Maint"
    Title="Cost Sheet Capital Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="98%" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblCapitalDesc" Text="Capital Description:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchCapitalDesc" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <asp:ValidationSummary ID="vsEditCapital" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditCapital" />
        <asp:ValidationSummary ID="vsFooterCapital" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterCapital" />
        <asp:GridView runat="server" ID="gvCapital" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="100" ShowFooter="True" DataSourceID="odsCapital"
            DataKeyNames="CapitalID" Width="100%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:BoundField DataField="CapitalID" HeaderText="CapitalID" SortExpression="CapitalID" />
                <asp:TemplateField HeaderText="Description" SortExpression="CapitalDesc">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditCapitalDescMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtEditCapitalDesc" runat="server" MaxLength="50" Width="300px"
                            Text='<%# Bind("CapitalDesc") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEditCapital" runat="server" ControlToValidate="txtEditCapitalDesc"
                            ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgEditCapital"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewCaptialDesc" runat="server" Text='<%# Bind("ddCapitalDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterCapitalDescMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtFooterCapitalDesc" runat="server" MaxLength="50" Text='<%# Bind("CapitalDesc") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFooterCapital" runat="server" ControlToValidate="txtFooterCapitalDesc"
                            ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgFooterCapital"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditCapitalObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbViewCapitalObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:CheckBox ID="cbFooterCapitalObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnCapitalUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditCapital" />
                        <asp:ImageButton ID="iBtnCapitalCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnCapitalEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterCapital"
                            runat="server" ID="iBtnFooterCapital" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnCapitalUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsCapital" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetCapital" TypeName="CapitalBLL" UpdateMethod="UpdateCapital"
            InsertMethod="InsertCapital">
            <SelectParameters>
                <asp:QueryStringParameter Name="CapitalID" QueryStringField="CapitalID" Type="Int32" />
                <asp:QueryStringParameter Name="CapitalDesc" QueryStringField="CapitalDesc" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="CapitalDesc" Type="String" />
                <asp:Parameter Name="original_CapitalID" Type="Int32" />
                <asp:Parameter Name="CapitalID" Type="Int32" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="CapitalDesc" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
