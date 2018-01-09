<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Instructions.aspx.vb" Inherits="Packaging_Instructions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">

    <asp:Panel ID="localPanel" runat="server" Height="422px">
        <asp:Label runat="server" ID="lblMessage"></asp:Label><asp:Label ID="lblSearchTip"
            runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblInstru" runat="server" CssClass="p_text" Text="Instructions:"> </asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtSearch" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:ValidationSummary runat="server" ID="vsEdit" ValidationGroup="vgEdit" ShowMessageBox="true"
            ShowSummary="true" />
        <asp:ValidationSummary runat="server" ID="vsInsert" ValidationGroup="vgInsert" ShowMessageBox="true"
            ShowSummary="true" />
        <asp:GridView ID="gvInstru" runat="server" SkinID="StandardGrid" Width="750px" DataSourceID="odsInstruMaint"
            DataKeyNames="IID" OnRowCommand="gvInstru_RowCommand" AllowPaging="True" AllowSorting="True"
            on PageSize="30">
            <EmptyDataTemplate>
                No records found"</EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Instruction" SortExpression="Instruction">
                    <EditItemTemplate>
                        <asp:Label ID="maker1" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label>
                        <asp:TextBox ID="txtEditInstruction" runat="server" Text='<%# Bind("Instruction") %>'
                            MaxLength="250" Width="600px" Rows="3" TextMode="MultiLine" />
                        <asp:RequiredFieldValidator ID="rfvEdit" runat="server" ControlToValidate="txtEditInstruction"
                            ErrorMessage="Instruction is a required field." Text=" <" Font-Bold="True" ValidationGroup="vgEdit" />
                        <asp:RegularExpressionValidator ID="revEditInstru" runat="server" ControlToValidate="txtEditInstruction"
                            ErrorMessage="You exceeded the number of 250chars required, please revise." Text=" <"
                            ValidationExpression=".{0,250}" ValidationGroup="vgEdit" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblView" runat="server" Text='<%# Bind("ddInstruction") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="marker2" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                        <asp:TextBox ID="txtInsertInstruction" runat="server" MaxLength="250" Width="600px"
                            Rows="3" TextMode="MultiLine"  />
                        <asp:RequiredFieldValidator ID="rfvInsert" runat="server" ControlToValidate="txtInsertInstruction"
                            ErrorMessage="Instruction is a required field." Text=" <" Font-Bold="True" ValidationGroup="vgInsert" />
                         <asp:RegularExpressionValidator ID="revInsertInstru" runat="server" ControlToValidate="txtInsertInstruction"
                            ErrorMessage="You exceeded the number of 250chars required, please revise." Text=" <"
                            ValidationExpression=".{0,250}" ValidationGroup="vgInsert" />
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditObsolete" runat="Server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbViewObsolete" runat="Server" Checked='<%# Bind("Obsolete") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEdit" />
                        <asp:ImageButton ID="iBtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsert"
                            runat="server" ID="iBtnSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsInstruMaint" runat="server" InsertMethod="InsertInstruMaint"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetInstruMaint" TypeName="PKGBLL"
            UpdateMethod="UpdateInstruMaint">
            <UpdateParameters>
                <asp:Parameter Name="Instruction" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_IID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="IID" Type="Int32" />
                <asp:ControlParameter ControlID="txtSearch" Name="Instruction" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="Instruction" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
