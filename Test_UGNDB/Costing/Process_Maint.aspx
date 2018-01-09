<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="Process_Maint.aspx.vb" Inherits="Process_Maint"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="98%" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblProcessName" Text="Process Name:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchProcessName" MaxLength="50"></asp:TextBox>
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
        <asp:ValidationSummary ID="vsEditProcess" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditProcess" />
        <asp:ValidationSummary ID="vsFooterProcess" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterProcess" />
        <asp:GridView runat="server" ID="gvProcess" AllowPaging="True" AllowSorting="True"
            DataKeyNames="ProcessID" AutoGenerateColumns="False" PageSize="100" ShowFooter="True"
            DataSourceID="odsProcess" Width="100%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:BoundField DataField="ProcessID" HeaderText="ProcessID" SortExpression="ProcessID" />
                <asp:TemplateField HeaderText="Name" SortExpression="ProcessName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditProcessNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtEditProcessName" runat="server" MaxLength="50" Width="300px"
                            Text='<%# Bind("ProcessName") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEditProcess" runat="server" ControlToValidate="txtEditProcessName"
                            ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgEditProcess"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewProcessName" runat="server" Text='<%# Bind("ddProcessName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterProcessNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtFooterProcessName" runat="server" MaxLength="50" Width="300px"
                            Text='<%# Bind("ProcessName") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFooterProcess" runat="server" ControlToValidate="txtFooterProcessName"
                            ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgFooterProcess"
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
                        <asp:ImageButton ID="iBtnProcessUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditProcess" />
                        <asp:ImageButton ID="iBtnProcessCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnProcessEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterProcess"
                            runat="server" ID="iBtnFooterProcesse" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnProcessUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsProcess" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetProcess" TypeName="ProcessBLL" UpdateMethod="UpdateProcess"
            InsertMethod="InsertProcess">
            <SelectParameters>
                <asp:QueryStringParameter Name="ProcessID" QueryStringField="ProcessID" Type="Int32" />
                <asp:QueryStringParameter Name="ProcessName" QueryStringField="ProcessName" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="ProcessName" Type="String" />
                <asp:Parameter Name="original_ProcessID" Type="Int32" />
                <asp:Parameter Name="ProcessID" Type="Int32" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="ProcessName" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
