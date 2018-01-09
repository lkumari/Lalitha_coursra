<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AR_Deduction_Reason_Maint.aspx.vb" Inherits="AR_Deduction_Reason_Maint"
    MaintainScrollPositionOnPostback="True" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table width="70%">
            <tr>
                <td class="p_text" style="width: 75px">
                    Reason:
                </td>
                <td style="width: 206px">
                    <asp:TextBox ID="txtReason" runat="server" Width="200px" MaxLength="25" />
                    <ajax:FilteredTextBoxExtender ID="ftbReasonDesc" runat="server" TargetControlID="txtReason"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, %. " />
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvReasonList" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="RID" DataSourceID="odsReasonList" EmptyDataText="No records found."
            OnRowCommand="gvReasonList_RowCommand" PageSize="50" ShowFooter="True" Width="600px">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="RID" HeaderText="Reason ID" Visible="false" SortExpression="RID" />
                <asp:TemplateField HeaderText="Reason" SortExpression="ReasonDesc">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtReasonDescEdit" runat="server" MaxLength="50" Text='<%# Bind("ReasonDesc") %>'
                            Width="200px" />
                        <ajax:FilteredTextBoxExtender ID="ftbReasonDescEdit" runat="server" TargetControlID="txtReasonDescEdit"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, " />
                        <asp:RequiredFieldValidator ID="rfvReasonDesc" runat="server" ControlToValidate="txtReasonDescEdit"
                            Display="Dynamic" ErrorMessage="Reason is a required field" Font-Bold="True"
                            Font-Size="Medium" ValidationGroup="EditReasonInfo"> &lt;
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblReasonDescPreEdit" runat="server" Text='<%# Bind("ReasonDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtReasonDescInsert" runat="server" MaxLength="50" Text="" ValidationGroup="InsertReasonInfo"
                            Width="200px" />
                        <ajax:FilteredTextBoxExtender ID="ftbReasonDescInsert" runat="server" TargetControlID="txtReasonDescInsert"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, " />
                        &nbsp;
                        <asp:RequiredFieldValidator ID="rfvReasonDescInsert" runat="server" ControlToValidate="txtReasonDescInsert"
                            ErrorMessage="Reason is a required field" ValidationGroup="InsertReasonInfo"> &lt;
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Default Notice To" SortExpression="DefaultNotify" HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEDefaultNotify" runat="server" SelectedValue='<%# Bind("DefaultNotify") %>'>
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="B">Both</asp:ListItem>
                            <asp:ListItem Value="M">Materials</asp:ListItem>
                            <asp:ListItem Value="Q">Quality</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                        <asp:RequiredFieldValidator ID="rfvDefaultNotifyEdit" runat="server" ControlToValidate="ddEDefaultNotify"
                            ErrorMessage="Default Notice To is a required field" ValidationGroup="EditReasonInfo"> &lt;
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label8" runat="server" Text='<%# Bind("DefaultNotifyDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddIDefaultNotify" runat="server">
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="B">Both</asp:ListItem>
                            <asp:ListItem Value="M">Materials</asp:ListItem>
                            <asp:ListItem Value="Q">Quality</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                        <asp:RequiredFieldValidator ID="rfvDefaultNotifyInsert" runat="server" ControlToValidate="ddIDefaultNotify"
                            ErrorMessage="Default Notice To is a required field" ValidationGroup="InsertReasonInfo"> &lt;
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkObsoleteEdit" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkObsoletePreEdit" runat="server" Checked='<%# Bind("Obsolete") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="comboUpdateInfo" HeaderStyle-HorizontalAlign="left" HeaderText="Last Update"
                    ReadOnly="True" SortExpression="comboUpdateInfo">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" AlternateText="Update"
                            CausesValidation="True" CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update"
                            ValidationGroup="EditReasonInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="Cancel" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" Text="Cancel" ValidationGroup="EditReasonInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" />
                        &nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnInsert" runat="server" AlternateText="Insert"
                            CausesValidation="true" CommandName="Insert" ImageUrl="~/images/save.jpg" Text="Insert"
                            ValidationGroup="InsertReasonInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" AlternateText="Undo"
                            CommandName="Undo" ImageUrl="~/images/undo-gray.jpg" Text="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditReasonInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EditReasonInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertReasonInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="InsertReasonInfo" />
        <br />
        <asp:ValidationSummary ID="vsEmptyReasonInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EmptyReasonInfo" />
        <asp:ObjectDataSource ID="odsReasonList" runat="server" SelectMethod="GetARDeductionReason"
            TypeName="ARDeductionBLL" UpdateMethod="UpdateARDeductionReason" InsertMethod="InsertARDeductionReason"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter Name="ReasonDesc" QueryStringField="sRsnDesc" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="ReasonDesc" Type="String" />
                <asp:Parameter Name="DefaultNotify" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_RID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="ReasonDesc" Type="String" />
                <asp:Parameter Name="DefaultNotify" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
