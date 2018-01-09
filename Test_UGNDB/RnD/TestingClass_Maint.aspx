<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="TestingClass_Maint.aspx.vb" Inherits="RnDTestingClass_Maint" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="568px" Visible="False"></asp:Label><br />
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    Testing Classification:
                </td>
                <td>
                    <asp:TextBox ID="txtTestClassName" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <asp:GridView ID="gvTestClass" runat="server" AutoGenerateColumns="False" DataSourceID="odsTestClass"
            AllowSorting="True" AllowPaging="True" Width="550px" OnRowCommand="gvTestClass_RowCommand"
            DataKeyNames="TestClassID" OnDataBound="gvTestClass_DataBound" PageSize="30"
            SkinID="StandardGrid">
            <Columns>
                <asp:BoundField DataField="TestClassID" HeaderText="TestClassID" SortExpression="TestClassID"
                    ReadOnly="True" Visible="False" />
                <asp:TemplateField HeaderText="Testing Classification" SortExpression="TestClassName"
                    HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTestClass" runat="server" Text='<%# Bind("TestClassName") %>'
                            MaxLength="50" Width="300px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblTestClass" runat="server" Text='<%# Bind("TestClassName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtTestClass" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:CheckBoxField DataField="Obsolete" HeaderText="Obsolete" SortExpression="Obsolete">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:CheckBoxField>
                <asp:TemplateField HeaderText="Last Update" SortExpression="comboUpdateInfo" HeaderStyle-HorizontalAlign="Left">
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
                            CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update"
                            ValidationGroup="EditTestClassInfo" />&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnCancel"
                                runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.jpg"
                                Text="Cancel" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert"
                            ValidationGroup="InsertTestClassInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsTestClass" runat="server" InsertMethod="InsertTestingClassification"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetTestingClassification"
            TypeName="RDTestingClassificationBLL" UpdateMethod="UpdateTestingClassification">
            <UpdateParameters>
                <asp:Parameter Name="TestClassName" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="Original_TestClassID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="TestClassName" QueryStringField="TestClassName" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="TestClassName" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ValidationSummary ID="EditTestClassInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="EditTestClassInfo" />
        &nbsp;<asp:ValidationSummary ID="InsertTestClassInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertTestClassInfo" />
    </asp:Panel>
</asp:Content>
