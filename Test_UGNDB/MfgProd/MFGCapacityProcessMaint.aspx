<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="MFGCapacityProcessMaint.aspx.vb" Inherits="MFGCapacityProcessMaint"
    MaintainScrollPositionOnPostback="True" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table width="80%">
            <tr>
                <td class="p_text" style="width: 75px">
                    Process:
                </td>
                <td style="width: 206px">
                    <asp:TextBox ID="txtProcess" runat="server" Width="200px" MaxLength="25" />
                    <ajax:FilteredTextBoxExtender ID="ftbMFGProcessName" runat="server" TargetControlID="txtProcess"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, %. " />
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvProcessList" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="PID" DataSourceID="odsProcessList"
            EmptyDataText="No records found." OnRowCommand="gvProcessList_RowCommand" PageSize="50"
            ShowFooter="True" Width="850px">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" AlternateText="Update"
                            CausesValidation="True" CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update"
                            ValidationGroup="EditInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="Cancel" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" Text="Cancel" ValidationGroup="EditInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" />
                        &nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnInsert" runat="server" AlternateText="Insert"
                            CausesValidation="true" CommandName="Insert" ImageUrl="~/images/save.jpg" Text="Insert"
                            ValidationGroup="InsertInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" AlternateText="Undo"
                            CommandName="Undo" ImageUrl="~/images/undo-gray.jpg" Text="Undo" />
                    </FooterTemplate>
                    <HeaderStyle Width="70px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Work Centers" HeaderStyle-Width="50px">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl="~/images/PreviewUp.jpg"
                            ToolTip="Preview Work Centers" NavigateUrl='<%# "MFGCapacityProcessWCMaint.aspx?pPID=" & DataBinder.Eval (Container.DataItem,"PID").tostring & "&sPrcNme=" & txtProcess.Text %>' />
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
                <asp:BoundField DataField="PID" HeaderText="PID" Visible="false" SortExpression="PID" />
                <asp:TemplateField HeaderText="Process" SortExpression="MFGProcessName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtMFGProcessNameEdit" runat="server" MaxLength="50" Text='<%# Bind("MFGProcessName") %>'
                            Width="250px" />
                        <ajax:FilteredTextBoxExtender ID="ftbMFGProcessNameEdit" runat="server" TargetControlID="txtMFGProcessNameEdit"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, " />
                        <asp:RequiredFieldValidator ID="rfvMFGProcessName" runat="server" ControlToValidate="txtMFGProcessNameEdit"
                            Display="Dynamic" ErrorMessage="Process is a required field." Font-Bold="True"
                            Font-Size="Medium" ValidationGroup="EditInfo"> &lt;
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblMFGProcessNamePreEdit" runat="server" Text='<%# Bind("MFGProcessName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtMFGProcessNameGVF" runat="server" MaxLength="50" Text="" ValidationGroup="InsertInfo"
                            Width="200px" />
                        <ajax:FilteredTextBoxExtender ID="ftbMFGProcessNameGVF" runat="server" TargetControlID="txtMFGProcessNameGVF"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, " />
                        &nbsp;
                        <asp:RequiredFieldValidator ID="rfvMFGProcessNameGVF" runat="server" ControlToValidate="txtMFGProcessNameGVF"
                            ErrorMessage="Process is a required field." ValidationGroup="InsertInfo"> &lt;
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Divide WCs" SortExpression="HalfSplit">
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbHalfSplit" runat="server" Checked='<%# Bind("HalfSplit") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkHalfSplit" runat="server" Checked='<%# Bind("HalfSplit") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <FooterStyle HorizontalAlign="Center" />
                    <FooterTemplate>
                        <asp:CheckBox ID="cbHalfSplitGVF" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="QtyCells" HeaderText="Qty of Cells" SortExpression="QtyCells"
                    ReadOnly="True">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="QtyShftsPerDayPerCell" HeaderText="Qty of Shifts Utilized/Day/Cell"
                    SortExpression="QtyShftsPerDayPerCell" ReadOnly="True">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="AvailShftsPerDay" HeaderText="Available Shifts/Day" SortExpression="AvailShftsPerDay"
                    ReadOnly="True">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Current Qty Shifts/Day" SortExpression="CurrentQtyShftsPerDay">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCurrentQtyShftsPerDay" runat="server" Text='<%# Bind("CurrentQtyShftsPerDay") %>'
                            MaxLength="6" Width="50px" />
                        <ajax:FilteredTextBoxExtender ID="ftbCurrentQtyShftsPerDay" runat="server" TargetControlID="txtCurrentQtyShftsPerDay"
                            FilterType="Custom, Numbers" ValidChars="." />
                        <asp:RequiredFieldValidator ID="rfvCurrentQtyShftsPerDay" runat="server" ControlToValidate="txtCurrentQtyShftsPerDay"
                            Display="Dynamic" ErrorMessage="Current Qty Shifts/Day is a required field."
                            ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvCurrentQtyShftsPerDay" runat="server" ErrorMessage="Current Qty Shifts/Day  values must be between 0.25 to 999.99"
                            ControlToValidate="txtCurrentQtyShftsPerDay" MinimumValue="0.25" MaximumValue="999.99"
                            Type="double" ValidationGroup="EditInfo"><</asp:RangeValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("CurrentQtyShftsPerDay") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtCurrentQtyShftsPerDayGVF" runat="server" MaxLength="6" Width="50px" />
                        <ajax:FilteredTextBoxExtender ID="ftbCurrentQtyShftsPerDayGVF" runat="server" TargetControlID="txtCurrentQtyShftsPerDayGVF"
                            FilterType="Custom, Numbers" ValidChars="." />
                        <asp:RequiredFieldValidator ID="rfvCurrentQtyShftsPerDayGVF" runat="server" ControlToValidate="txtCurrentQtyShftsPerDayGVF"
                            Display="Dynamic" ErrorMessage="Current Qty Shifts/Day is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvCurrentQtyShftsPerDayGVF" runat="server" ErrorMessage="Current Qty Shifts/Day values must be between 0.25 to 999.99"
                            ControlToValidate="txtCurrentQtyShftsPerDayGVF" MinimumValue="0.25" MaximumValue="999.99"
                            Type="double" ValidationGroup="InsertInfo"><</asp:RangeValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CurrentPrcntCrewCap" HeaderText="Current % Crew Capacity"
                    SortExpression="CurrentPrcntCrewCap" ReadOnly="True" DataFormatString="{0:p}">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
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
                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EditInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="InsertInfo" />
        <br />
        <asp:ValidationSummary ID="vsEmptyReasonInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EmptyReasonInfo" />
        <asp:ObjectDataSource ID="odsProcessList" runat="server" SelectMethod="GetMFGCapacityProcess"
            TypeName="MfgProdBLL" UpdateMethod="UpdateMFGCapacityProcess" InsertMethod="InsertMFGCapacityProcess"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter Name="PID" Type="Int32" />
                <asp:QueryStringParameter Name="MFGProcessName" QueryStringField="sMfgCPN" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="MFGProcessName" Type="String" />
                <asp:Parameter Name="HalfSplit" Type="Boolean" />
                <asp:Parameter Name="CurrentQtyShftsPerDay" Type="Decimal" />
                <asp:Parameter Name="original_MFGProcessName" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_PID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="MFGProcessName" Type="String" />
                <asp:Parameter Name="HalfSplit" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
