<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ExpProj_Capital_Spending_Maint.aspx.vb" Inherits="ExpProj_Capital_Spending_Maint"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="568px" Visible="False"></asp:Label><br />
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    Capital Classification Name:
                </td>
                <td>
                    <asp:TextBox ID="txtCapitalSpendingName" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" /></td>
            </tr>
        </table>
        <hr />
        <br />
        <asp:GridView ID="gvCapitalSpending" runat="server" AutoGenerateColumns="False" DataSourceID="odsCapitalSpending"
            AllowSorting="True" AllowPaging="True" Width="600px" OnRowCommand="gvCapitalSpending_RowCommand"
            DataKeyNames="CSCode, CapitalSpendingName" OnDataBound="gvCapitalSpending_DataBound"
            PageSize="20" ShowFooter="True" EmptyDataText="No records found in the data source for child table.">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Wrap="False" BorderColor="White" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" Wrap="False" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" Wrap="False" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#E2DED6" Wrap="False" BorderStyle="None" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Wrap="False" />
            <EmptyDataRowStyle BackColor="White" Wrap="False" />
            <Columns>
                <asp:BoundField DataField="CSID" HeaderText="CSID" SortExpression="CSID" ReadOnly="True"
                    Visible="False" />
                <asp:TemplateField HeaderText="Code" SortExpression="CSCode">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCSCode" runat="server" MaxLength="1" Text='<%# Bind("CSCode") %>'
                            Width="20px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCSCode" runat="server" ControlToValidate="txtCSCode"
                            Display="Dynamic" ErrorMessage="Code is a required field." ValidationGroup="EditCapitalSpendingInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtCSCode" runat="server" MaxLength="1" Text='<%# Bind("CSCode") %>'
                            Width="20px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCSCode" runat="server" ControlToValidate="txtCSCode"
                            Display="Dynamic" ErrorMessage="Code is a required field." ValidationGroup="InsertCapitalSpendingInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("CSCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Capital Classification Name" SortExpression="CapitalSpendingName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCapitalSpending" runat="server" Text='<%# Bind("CapitalSpendingName") %>'
                            MaxLength="50" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCapitalSpending" runat="server" ControlToValidate="txtCapitalSpending"
                            Display="Dynamic" ErrorMessage="Capital Spending Name is a required field." ValidationGroup="EditCapitalSpendingInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCapitalSpending" runat="server" Text='<%# Bind("CapitalSpendingName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtCapitalSpending" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCapitalSpending" runat="server" ControlToValidate="txtCapitalSpending"
                            Display="Dynamic" ErrorMessage="Capital Spending Name is a required field." ValidationGroup="InsertCapitalSpendingInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:CheckBoxField DataField="Obsolete" HeaderText="Obsolete" SortExpression="Obsolete">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:CheckBoxField>
                <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Update" ReadOnly="True"
                    SortExpression="comboUpdateInfo" />
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Left" />
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="EditCapitalSpendingInfo" />&nbsp;&nbsp;&nbsp;<asp:ImageButton
                                ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton CommandName="Insert" CausesValidation="true" runat="server"
                            ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="InsertCapitalSpendingInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                No Records Found in the database.
            </EmptyDataTemplate>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsCapitalSpending" runat="server" InsertMethod="InsertExpProjCapitalSpending"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetExpProjCapitalSpending"
            TypeName="ExpProjCapitalSpendingBLL" UpdateMethod="UpdateExpProjCapitalSpending">
            <UpdateParameters>
                <asp:Parameter Name="CapitalSpendingName" Type="String" />
                <asp:Parameter Name="CSCode" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_CapitalSpendingName" Type="String" />
                <asp:Parameter Name="original_CSCode" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="CapitalSpendingName" QueryStringField="CapitalSpendingName"
                    Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="CapitalSpendingName" Type="String" />
                <asp:Parameter Name="CSCode" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ValidationSummary ID="EditCapitalSpendingInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="EditCapitalSpendingInfo" />
        &nbsp;<asp:ValidationSummary ID="InsertCapitalSpendingInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertCapitalSpendingInfo" />
    </asp:Panel>
</asp:Content>
