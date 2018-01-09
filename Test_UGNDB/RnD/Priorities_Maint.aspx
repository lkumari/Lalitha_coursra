<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Priorities_Maint.aspx.vb" Inherits="RnD_Priorities_Maint" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="568px" Visible="False"></asp:Label><br />
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    Priority Description:
                </td>
                <td>
                    <asp:TextBox ID="txtPriority" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <asp:GridView ID="gvPriority" runat="server" AutoGenerateColumns="False" DataSourceID="odsPriority"
            AllowSorting="True" AllowPaging="True" Width="70%" OnRowCommand="gvPriority_RowCommand"
            OnRowDataBound="gvPriority_RowDataBound" DataKeyNames="PID" OnDataBound="gvPriority_DataBound"
            PageSize="30" SkinID="StandardGrid">
            <Columns>
                <asp:BoundField DataField="PID" HeaderText="PID" SortExpression="PID" ReadOnly="True"
                    Visible="False" />
                <asp:TemplateField HeaderText="Priority Description" SortExpression="PriorityDescription">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPriorityDesc" runat="server" Text='<%# Bind("PriorityDescription") %>'
                            MaxLength="50" Width="300px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPriorityDesc" runat="server" Text='<%# Bind("PriorityDescription") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtPriorityDesc" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Color Code" SortExpression="ColorCode">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddColorCode" runat="server" SelectedValue='<%# Bind("ColorCode") %>'>
                            <asp:ListItem Value="crNoColor">No Color</asp:ListItem>
                            <asp:ListItem Value="16776960">Aqua</asp:ListItem>
                            <asp:ListItem Value="55295">Gold</asp:ListItem>
                            <asp:ListItem Value="11119017">Gray</asp:ListItem>
                            <asp:ListItem Value="16443110">Lavender</asp:ListItem>
                            <asp:ListItem Value="64636">Lawn Green</asp:ListItem>
                            <asp:ListItem Value="15128749">Light Blue</asp:ListItem>
                            <asp:ListItem Value="9498256">Light Green</asp:ListItem>
                            <asp:ListItem Value="14745599">Light Yellow</asp:ListItem>
                            <asp:ListItem Value="65280">Lime</asp:ListItem>
                            <asp:ListItem Value="33023">Orange</asp:ListItem>
                            <asp:ListItem Value="14053594">Orchid</asp:ListItem>
                            <asp:ListItem Value="16777147">Pale Turquoise</asp:ListItem>
                            <asp:ListItem Value="13353215">Pink</asp:ListItem>
                            <asp:ListItem Value="255">Red</asp:ListItem>
                            <asp:ListItem Value="16436871">Sky Blue</asp:ListItem>
                            <asp:ListItem Value="9221330">Tan</asp:ListItem>
                            <asp:ListItem Value="13688896">Turquoise</asp:ListItem>
                            <asp:ListItem Value="65535">Yellow</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblEditColor" runat="server" Text="COLOR"></asp:Label>
                        <div id="msgEditColor" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="ddColorCode" runat="server" Enabled="false" SelectedValue='<%# Bind("ColorCode") %>'>
                            <asp:ListItem Value="crNoColor">No Color</asp:ListItem>
                            <asp:ListItem Value="16776960">Aqua</asp:ListItem>
                            <asp:ListItem Value="55295">Gold</asp:ListItem>
                            <asp:ListItem Value="11119017">Gray</asp:ListItem>
                            <asp:ListItem Value="16443110">Lavender</asp:ListItem>
                            <asp:ListItem Value="64636">Lawn Green</asp:ListItem>
                            <asp:ListItem Value="15128749">Light Blue</asp:ListItem>
                            <asp:ListItem Value="9498256">Light Green</asp:ListItem>
                            <asp:ListItem Value="14745599">Light Yellow</asp:ListItem>
                            <asp:ListItem Value="65280">Lime</asp:ListItem>
                            <asp:ListItem Value="33023">Orange</asp:ListItem>
                            <asp:ListItem Value="14053594">Orchid</asp:ListItem>
                            <asp:ListItem Value="16777147">Pale Turquoise</asp:ListItem>
                            <asp:ListItem Value="13353215">Pink</asp:ListItem>
                            <asp:ListItem Value="255">Red</asp:ListItem>
                            <asp:ListItem Value="16436871">Sky Blue</asp:ListItem>
                            <asp:ListItem Value="9221330">Tan</asp:ListItem>
                            <asp:ListItem Value="13688896">Turquoise</asp:ListItem>
                            <asp:ListItem Value="65535">Yellow</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblViewColor" runat="server" Text="COLOR"></asp:Label>
                        <div id="msgViewColor" runat="server" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddColorCode" runat="server">
                            <asp:ListItem Value="crNoColor">No Color</asp:ListItem>
                            <asp:ListItem Value="16776960">Aqua</asp:ListItem>
                            <asp:ListItem Value="55295">Gold</asp:ListItem>
                            <asp:ListItem Value="11119017">Gray</asp:ListItem>
                            <asp:ListItem Value="16443110">Lavender</asp:ListItem>
                            <asp:ListItem Value="64636">Lawn Green</asp:ListItem>
                            <asp:ListItem Value="15128749">Light Blue</asp:ListItem>
                            <asp:ListItem Value="9498256">Light Green</asp:ListItem>
                            <asp:ListItem Value="14745599">Light Yellow</asp:ListItem>
                            <asp:ListItem Value="65280">Lime</asp:ListItem>
                            <asp:ListItem Value="33023">Orange</asp:ListItem>
                            <asp:ListItem Value="14053594">Orchid</asp:ListItem>
                            <asp:ListItem Value="16777147">Pale Turquoise</asp:ListItem>
                            <asp:ListItem Value="13353215">Pink</asp:ListItem>
                            <asp:ListItem Value="255">Red</asp:ListItem>
                            <asp:ListItem Value="16436871">Sky Blue</asp:ListItem>
                            <asp:ListItem Value="9221330">Tan</asp:ListItem>
                            <asp:ListItem Value="13688896">Turquoise</asp:ListItem>
                            <asp:ListItem Value="65535">Yellow</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:CheckBoxField DataField="Obsolete" HeaderText="Obsolete" SortExpression="Obsolete">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:CheckBoxField>
                <asp:TemplateField HeaderText="Last Update" SortExpression="comboUpdateInfo">
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("comboUpdateInfo") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("comboUpdateInfo") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Left" />
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="EditPriorityInfo" />&nbsp;&nbsp;&nbsp;<asp:ImageButton
                                ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton CommandName="Insert" CausesValidation="true" runat="server"
                            ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="InsertPriorityInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsPriority" runat="server" InsertMethod="InsertPriorities"
            SelectMethod="GetPriorities" TypeName="RDPrioritiesBLL" UpdateMethod="UpdatePriorities"
            OldValuesParameterFormatString="original_{0}">
            <UpdateParameters>
                <asp:Parameter Name="PriorityDescription" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_PID" Type="Int32" />
                <asp:Parameter Name="ColorCode" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="PriorityDescription" QueryStringField="sPdesc" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="ColorCode" Type="String" />
                <asp:Parameter Name="PriorityDescription" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ValidationSummary ID="EditPriorityInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="EditPriorityInfo" />
        &nbsp;<asp:ValidationSummary ID="InsertPriorityInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertPriorityInfo" />
    </asp:Panel>
</asp:Content>
