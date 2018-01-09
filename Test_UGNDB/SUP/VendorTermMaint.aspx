<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="VendorTermMaint.aspx.vb" Inherits="Vendor_Term_Maint" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="568px" Visible="False"></asp:Label><br />
    <asp:Panel ID="localPanel" runat="server" Width="1000px">
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    Vendor Term:
                </td>
                <td>
                    <asp:TextBox ID="txtTerm" runat="server" MaxLength="200" Width="300px"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" /></td>
            </tr>
        </table>
        <hr />
        <br />
        <%--OnDataBound="gvTerm_DataBound" --%>
        <asp:GridView ID="gvTerm" runat="server" AutoGenerateColumns="False" DataSourceID="odsVendorTerm"
            AllowSorting="True" AllowPaging="True" OnRowCommand="gvTerm_RowCommand" DataKeyNames="TID"
            PageSize="20" ShowFooter="True" Width="600px">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Wrap="False" BorderColor="White" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" Wrap="False" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" Wrap="False" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#E2DED6" Wrap="False" BorderStyle="None" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Wrap="False" />
            <EmptyDataRowStyle BackColor="White" Wrap="False" />
            <Columns>
                <asp:BoundField DataField="TID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                    SortExpression="TID" Visible="False" />
                <asp:TemplateField HeaderText="Vendor Term" SortExpression="Term" HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTerm" runat="server" Text='<%# Bind("Term") %>' MaxLength="50"
                            Width="350px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Term") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtTerm" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTerm" runat="server" ControlToValidate="txtTerm"
                            Display="Dynamic" ErrorMessage="Vendor Term is a required field." ValidationGroup="InsertTermInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete" ItemStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:CheckBox ID="CheckBox1" Checked='<%# Bind("Obsolete") %>' runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ckObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Update" SortExpression="comboUpdateInfo" />
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Left" />
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="EditTermInfo" />&nbsp;&nbsp;&nbsp;<asp:ImageButton
                                ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton CommandName="Insert" CausesValidation="true" runat="server"
                            ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="InsertTermInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsVendorTerm" runat="server" InsertMethod="InsertVendorTerm"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetVendorTerm" TypeName="VendorTermBLL"
            UpdateMethod="UpdateVendorTerm">
            <UpdateParameters>
                <asp:Parameter Name="Term" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_Term" Type="String" />
                <asp:Parameter Name="original_TID" Type="Int32" />
                <asp:Parameter Name="comboUpdateInfo" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="Term" QueryStringField="pTerm" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="Term" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ValidationSummary ID="EditTermInfo" runat="server" ShowMessageBox="True" ValidationGroup="EditTermInfo" />
        &nbsp;<asp:ValidationSummary ID="InsertTermInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertTermInfo" />
    </asp:Panel>
</asp:Content>
