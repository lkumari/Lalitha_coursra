<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="FormsMaintenance.aspx.vb" Inherits="Security_FormsMaintenance" Title="Forms Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <hr />
    <asp:Label ID="lblErrors" runat="server" SkinID="MessageLabelSkin"></asp:Label>
    <!-- Display Search fields -->
    <table>
        <tr>
            <td class="p_text">
                Form Name:
            </td>
            <td>
                <asp:TextBox ID="txtFormName" runat="server" ToolTip="Search for Form Name (May use % wildcard characters)"
                    Width="216px">
                </asp:TextBox>
                &nbsp;
                <asp:RegularExpressionValidator ID="revFormName" runat="server" ControlToValidate="txtFormName"
                    ErrorMessage="Form Name  must contain letters or numbers. Wildcard characters may only be used at the beginning or end."
                    ValidationExpression="^[%]?[\d\w-'\s]{1,30}[%]?$" ValidationGroup="vgSearch">
                    *
                </asp:RegularExpressionValidator>-
            </td>
            <td>
            </td>
            <td class="p_text">
                Hyperlink Id:
            </td>
            <td>
                <asp:TextBox ID="txtHyperlinkID" runat="server" ToolTip="Search for Hyperlink Id (May use % wildcard characters)"
                    Width="216px">
                </asp:TextBox>
                &nbsp;
                <asp:RegularExpressionValidator ID="revHyperlinkID" runat="server" ErrorMessage="HyperlinkID is limited to letters, numbers, and the .',()/~ special characters. Wildcard characters may only be used at the beginning or end."
                    ValidationExpression="^[%]?[\d\w-'\s\.\,\(\)/~]{1,70}[%]?$" ValidationGroup="vgSearch"
                    ControlToValidate="txtHyperlinkID">
                    *
                </asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td colspan="4">
                <%-- Validation Summary for search fields --%>
                <asp:ValidationSummary ID="vsSearch" runat="server" ValidationGroup="vgSearch" Width="400px"
                    ShowMessageBox="True" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td colspan="4">
                <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgSearch" />
                <asp:Button ID="btnResetSearch" runat="server" Text="Reset" CausesValidation="False" />
            </td>
        </tr>
    </table>
    <hr />
    <asp:GridView ID="gvForms" runat="server" Width="69%" DataKeyNames="FormID" AllowPaging="True"
        AllowSorting="True" ShowFooter="True" AutoGenerateColumns="False" DataSourceID="odsForms"
        PageSize="15">
        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#CCCCCC" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EmptyDataTemplate>
            No data to display
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="Form Id" SortExpression="FormID">
                <FooterStyle HorizontalAlign="Right" Wrap="False" />
                <FooterTemplate>
                    <asp:Image ID="imgAsterisk" runat="server" ImageUrl="~/images/asterick_blue.gif"
                        AlternateText="New row" />
                    &nbsp;
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblFormIdPreEdit" runat="server" Text='<%# Eval("FormID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Form Name" SortExpression="FormName">
                <EditItemTemplate>
                    <asp:TextBox ID="txtFormNameEdit" runat="server" Text='<%# Bind("FormName") %>' />
                    <asp:RequiredFieldValidator ID="rfvFormNameEdit" runat="server" ControlToValidate="txtFormNameEdit"
                        ErrorMessage="Form Name is required" ValidationGroup="vgEditFormInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <%-- <asp:RegularExpressionValidator ID="revFormNameEdit" runat="server" ControlToValidate="txtFormNameEdit"
                        ErrorMessage="Form Name must contain only letters or numbers" ValidationGroup="vgEditFormInfo"
                        ValidationExpression="^[\d\w-'\s]{1,30}$">
                        &lt;
                    </asp:RegularExpressionValidator>--%>
                    <ajax:FilteredTextBoxExtender ID="ftbFormNameEdit" runat="server" TargetControlID="txtFormNameEdit"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,/-&()[] " />
                </EditItemTemplate>
                <FooterStyle HorizontalAlign="Left" Wrap="False" />
                <FooterTemplate>
                    <asp:TextBox ID="txtFormNameInsert" runat="server" Text='<%# Bind("FormName") %>' />
                    <asp:RequiredFieldValidator ID="rfvFormNameInsert" runat="server" ControlToValidate="txtFormNameInsert"
                        ErrorMessage="Form Name is required" ValidationGroup="vgInsertFormInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <%-- <asp:RegularExpressionValidator ID="revFormNameInsert" runat="server" ControlToValidate="txtFormNameInsert"
                        ErrorMessage="Form Name must contain only letters or numbers" ValidationGroup="vgINsertFormInfo"
                        ValidationExpression="^[\d\w-'\s]{1,30}$">
                        &lt;
                    </asp:RegularExpressionValidator>--%>
                    <ajax:FilteredTextBoxExtender ID="ftbFormNameInsert" runat="server" TargetControlID="txtFormNameInsert"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,/-&()[] " />
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblFormNamePreEdit" runat="server" Text='<%# Eval("FormName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Hyperlink Id" SortExpression="HyperlinkID">
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                <FooterStyle HorizontalAlign="Left" Wrap="False" />
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlHyperlinkIDEdit" runat="server" DataSourceID="odsHyperlinkID"
                        DataTextField="ddHyperlinkID" DataValueField="ddHyperlinkID" SelectedValue='<%# Eval("ddHyperlinkID") %>'
                        AppendDataBoundItems="True" ToolTip="Select a Hyperlink" Width="350px">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvHyperlinkIDEdit" runat="server" ErrorMessage="HyperlinkID is required"
                        Display="Dynamic" ControlToValidate="ddlHyperlinkIDEdit" ValidationGroup="vgEditFormInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:DropDownList ID="ddlHyperlinkIDInsert" runat="server" DataSourceID="odsHyperlinkID"
                        DataTextField="ddHyperlinkID" DataValueField="ddHyperlinkID" AppendDataBoundItems="True"
                        ToolTip="Select a Hyperlink" Width="350px">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvHyperlinkIDInsert" runat="server" ErrorMessage="HyperlinkID is required"
                        Display="Dynamic" ControlToValidate="ddlHyperlinkIDInsert" ValidationGroup="vgInsertFormInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblHyperlinkIDPreEdit" runat="server" Text='<%# Eval("HyperlinkID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Menu ID" SortExpression="MenuID">
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblMenuID" runat="server" Text='<%# Eval("MenuID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Menu Name" SortExpression="MenuName">
                <EditItemTemplate>
                    <asp:DropDownList ID="ddMenu" runat="server" DataSourceID="odsMenu" DataTextField="MenuName"
                        DataValueField="MenuID" AppendDataBoundItems="True" ToolTip="Select a Menu" SelectedValue='<%# Eval("MenuID") %>'>
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvMenu" runat="server" ErrorMessage="Menu Name is required"
                        Display="Dynamic" ControlToValidate="ddMenu" ValidationGroup="vgEditFormInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:DropDownList ID="ddMenu" runat="server" DataSourceID="odsMenu" DataTextField="MenuName"
                        DataValueField="MenuID" AppendDataBoundItems="True" ToolTip="Select a Menu">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvMenu" runat="server" ErrorMessage="Menu Name is required"
                        Display="Dynamic" ControlToValidate="ddMenu" ValidationGroup="vgInsertFormInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblMenu" runat="server" Text='<%# Eval("MenuName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                <EditItemTemplate>
                    <asp:CheckBox ID="chkObsoleteEdit" runat="server" Checked='<%# Bind("Obsolete") %>' />
                </EditItemTemplate>
                <FooterStyle HorizontalAlign="Center" Wrap="False" />
                <FooterTemplate>
                    <asp:CheckBox ID="chkObsoleteInsert" runat="server" Checked='<%# Bind("Obsolete") %>' />
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:CheckBox ID="chkObsoletePreEdit" runat="server" Checked='<%# Eval("Obsolete") %>'
                        Enabled="false" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Update" SortExpression="comboUpdateInfo">
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblComboUpdateInfo" runat="server" Text='<%# Eval("comboUpdateInfo") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False" AccessibleHeaderText="Buttons Column">
                <EditItemTemplate>
                    <asp:ImageButton ID="ibtnUpdate" runat="server" CommandName="UpdateCustom" CausesValidation="True"
                        ValidationGroup="vgEditFormInfo" ImageUrl="~/images/save.jpg" ToolTip="Save changes"
                        AlternateText="Save changes" OnClick="ibtnUpdate_Click" />
                    <asp:ImageButton ID="ibtnCancelEdit" runat="server" CommandName="Cancel" CausesValidation="False"
                        ImageUrl="~/images/undo-transparent.gif" ToolTip="Undo changes" AlternateText="Undo changes" />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:ImageButton ID="ibtnInsert" runat="server" CommandName="InsertCustom" CausesValidation="true"
                        ValidationGroup="vgInsertFormInfo" ImageUrl="~/images/save.jpg" ToolTip="Save new row"
                        AlternateText="Save new row" />
                    <asp:ImageButton ID="ibtnCancelInsert" runat="server" CommandName="Cancel" CausesValidation="False"
                        ImageUrl="~/images/undo-transparent.gif" ToolTip="Undo changes" AlternateText="Undo changes" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:ImageButton ID="ibtnEdit" runat="server" CommandName="Edit" CausesValidation="False"
                        ImageUrl="~/images/edit.jpg" ToolTip="Edit Row" AlternateText="Edit Row" />
                </ItemTemplate>
                <FooterStyle HorizontalAlign="Center" Wrap="False" />
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    &nbsp;&nbsp;
    <%-- Validation Summary for GridView controls --%>
    <asp:ValidationSummary ID="vsEditFormInfo" runat="server" ShowMessageBox="True" Width="599px"
        ValidationGroup="vgEditFormInfo" />
    <asp:ValidationSummary ID="vsInsertFormInfo" runat="server" ShowMessageBox="True"
        Width="599px" ValidationGroup="vgInsertFormInfo" />
    <asp:Label ID="lblStatus" runat="server" />
    <br />
    <%-- Data Source for GridView --%>
    <asp:ObjectDataSource ID="odsForms" runat="server" SelectMethod="GetForm" TypeName="SecurityModule">
        <SelectParameters>
            <asp:Parameter Name="FormID" Type="Int32" />
            <asp:ControlParameter ControlID="txtFormName" Name="FormName" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtHyperlinkID" Name="HyperlinkID" PropertyName="Text"
                Type="String" />
            <asp:Parameter Name="Obsolete" Type="Boolean" />
            <asp:Parameter Name="SortBy" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <%-- Data Source for GridView HyperlinkID DropDownList --%>
    <asp:ObjectDataSource ID="odsHyperlinkID" runat="server" TypeName="SecurityModule"
        SelectMethod="GetPageUrlsAsDataSet" OldValuesParameterFormatString="original_{0}">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsMenu" runat="server" TypeName="SecurityModule" SelectMethod="GetMenu"
        OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:Parameter Name="MenuID" Type="Int32" />
            <asp:Parameter Name="MenuName" Type="String" />
            <asp:Parameter Name="Obsolete" Type="Boolean" />
            <asp:Parameter Name="SortBy" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
