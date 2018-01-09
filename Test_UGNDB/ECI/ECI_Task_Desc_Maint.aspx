<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" Title="ECI Task Description Maintenance"
    CodeFile="ECI_Task_Desc_Maint.aspx.vb" Inherits="ECI_Task_Desc_Maint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="98%" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage"></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblSearchTaskName" Text="Task Name:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchTaskName" MaxLength="50"></asp:TextBox>
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
        <asp:ValidationSummary runat="server" ID="vsEditTask" ValidationGroup="EditECITaskDesc"
            ShowMessageBox="true" ShowSummary="true" />
        <asp:ValidationSummary runat="server" ID="vsInsertTask" ValidationGroup="InsertECITaskDesc"
            ShowMessageBox="true" ShowSummary="true" />
        <br />
        <asp:GridView ID="gvECITaskDesc" runat="server" AutoGenerateColumns="False" AllowSorting="True"
            AllowPaging="True" PageSize="15" DataKeyNames="TaskID" ShowFooter="True" DataSourceID="odsECITaskDesc"
            EmptyDataText="No records found" Width="100%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Task Name" SortExpression="TaskName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditTaskNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtEditTaskName" runat="server" Text='<%# Bind("TaskName") %>' MaxLength="50"
                            Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvECITaskNameEdit" runat="server" ControlToValidate="txtEditTaskName"
                            ErrorMessage="Task Name is required." Font-Bold="True" ValidationGroup="EditECITaskDesc">
				     <
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewTaskName" runat="server" Text='<%# Bind("ddTaskName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblInsertTaskNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtInsertTaskName" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvECITaskNameInsert" runat="server" ControlToValidate="txtInsertTaskName"
                            ErrorMessage="Task Name is required." Font-Bold="True" ValidationGroup="InsertECITaskDesc">
				    <
                        </asp:RequiredFieldValidator>
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
                        <asp:ImageButton ID="iBtnECITaskDescUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="EditECITaskDesc" />
                        <asp:ImageButton ID="iBtnECITaskDescCancel" runat="server" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnECITaskDescEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="InsertECITaskDesc"
                            runat="server" ID="iBtnECITaskDescSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnECITaskDescUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsECITaskDesc" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetECITaskDesc" TypeName="ECITaskDescBLL" InsertMethod="InsertECITaskDesc"
            UpdateMethod="UpdateECITaskDesc">
            <SelectParameters>
                <asp:Parameter Name="TaskID" Type="Int32" />
                <asp:QueryStringParameter Name="TaskName" QueryStringField="TaskName" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="TaskName" Type="String" />
                <asp:Parameter Name="original_TaskID" Type="Int32" />
                <asp:Parameter Name="TaskID" Type="Int32" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="TaskName" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
