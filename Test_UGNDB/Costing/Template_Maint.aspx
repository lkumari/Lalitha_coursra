<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="Template_Maint.aspx.vb" Inherits="Template_Maint"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="98%" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblTemplateName" Text="Template Description:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchTemplateName" MaxLength="50"></asp:TextBox>
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
        <asp:ValidationSummary ID="vsEditTemplate" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditTemplate" />
        <asp:ValidationSummary ID="vsFooterTemplate" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterTemplate" />
        <asp:GridView runat="server" ID="gvTemplate" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="100" ShowFooter="True" DataSourceID="odsTemplate"
            DataKeyNames="TemplateID" Width="100%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:BoundField DataField="TemplateID" HeaderText="TemplateID" SortExpression="TemplateID" />
                <asp:TemplateField HeaderText="Name" SortExpression="TemplateName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditTemplateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtEditTemplateName" runat="server" MaxLength="50" Width="300px"
                            Text='<%# Bind("TemplateName") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEditTemplate" runat="server" ControlToValidate="txtEditTemplateName"
                            ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgEditTemplate"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewTemplateName" runat="server" Text='<%# Bind("ddTemplateName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterTemplateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtFooterTemplateName" runat="server" MaxLength="50" Width="300px"
                            Text='<%# Bind("TemplateName") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFooterTemplate" runat="server" ControlToValidate="txtFooterTemplateName"
                            ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgFooterTemplate"
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
                        <asp:ImageButton ID="iBtnTemplateUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditTemplate" />
                        <asp:ImageButton ID="iBtnTemplateCancel" runat="server" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnTemplateEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterTemplate"
                            runat="server" ID="iBtnFooterTemplate" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnTemplateUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsTemplate" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetTemplate" TypeName="TemplateBLL" UpdateMethod="UpdateTemplate"
            InsertMethod="InsertTemplate">
            <SelectParameters>
                <asp:QueryStringParameter Name="TemplateID" QueryStringField="TemplateID" Type="Int32" />
                <asp:QueryStringParameter Name="TemplateName" QueryStringField="TemplateName" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="TemplateName" Type="String" />
                <asp:Parameter Name="original_TemplateID" Type="Int32" />
                <asp:Parameter Name="TemplateID" Type="Int32" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="TemplateName" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
