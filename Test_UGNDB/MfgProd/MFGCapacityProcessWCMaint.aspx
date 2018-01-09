<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="MFGCapacityProcessWCMaint.aspx.vb" Inherits="MFGCapacityProcesWCMaint"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <table style="width: 60%; border-bottom-style: groove;" class="sampleStyleC">
            <tr>
                <td class="p_text">
                    Process:&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblProcess" runat="server" Text="" />
                </td>
                <td class="p_text">
                    Status:&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblStatus" runat="server" Text="" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblRaiseError" runat="server" Text="" Visible="false" SkinID="MessageLabelSkin" />
        <br />
        <hr />
        <asp:GridView ID="gvProcessList" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="PID,WorkCenter" DataSourceID="odsProcessList"
            EmptyDataText="No records found." PageSize="50" ShowFooter="True" Width="850px"
            OnRowDataBound="gvProcessList_RowDataBound" OnRowCommand="gvProcessList_RowCommand"
            OnRowUpdating="gvProcessList_RowUpdating" OnRowDeleted="gvProcessList_RowDeleted">
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
                <asp:TemplateField ShowHeader="False" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                    </ItemTemplate>
                    <HeaderStyle Width="30px" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="PID" HeaderText="PID" Visible="false" SortExpression="PID" />
                <asp:TemplateField HeaderText="UGN Facility" SortExpression="UGNFacilityName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddUGNLocation" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvUGNLocation" runat="server" ControlToValidate="ddUGNLocation"
                            Display="Dynamic" ErrorMessage="UGN Facility is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddUGNLocation" runat="server" TargetControlID="ddUGNLocation"
                            Category="UGNLocation" PromptText="Select a UGN Facility" LoadingText="[Loading UGN Facility...]"
                            SelectedValue='<%# Bind("UGNFacility") %>' ServicePath="~/WS/GeneralCDDService.asmx"
                            ServiceMethod="GetUGNLocation" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUGNFacility" runat="server" Text='<%# Bind("UGNFacilityName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddUGNLocationGVF" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvUGNLocationGVF" runat="server" ControlToValidate="ddUGNLocationGVF"
                            Display="Dynamic" ErrorMessage="UGN Facility is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddUGNLocationGVF" runat="server" TargetControlID="ddUGNLocationGVF"
                            Category="UGNLocation" PromptText="Select a UGN Facility" LoadingText="[Loading UGN Facility...]"
                            ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetUGNLocation" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Work Center Name" SortExpression="ddWorkCenterName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddWorkCenter" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvWorkCenter" runat="server" ControlToValidate="ddWorkCenter"
                            Display="Dynamic" ErrorMessage="Work Center is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddWorkCenter" runat="server" TargetControlID="ddWorkCenter"
                            ParentControlID="ddUGNLocation" Category="WorkCenter" SelectedValue='<%# Bind("WorkCenter") %>'
                            PromptText="Select a Work Center" LoadingText="[Loading Work Centers...]" ServicePath="~/WS/GeneralCDDService.asmx"
                            ServiceMethod="GetWorkCenter" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("ddWorkCenterName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddWorkCenterGVF" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvWorkCenterGVF" runat="server" ControlToValidate="ddWorkCenterGVF"
                            Display="Dynamic" ErrorMessage="Work Center is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddWorkCenterGVF" runat="server" TargetControlID="ddWorkCenterGVF"
                            ParentControlID="ddUGNLocationGVF" Category="WorkCenter" PromptText="Select a Work Center"
                            LoadingText="[Loading Work Centers...]" ServicePath="~/WS/GeneralCDDService.asmx"
                            ServiceMethod="GetWorkCenter" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="No of Shifts" SortExpression="NoOfShifts">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNoOfShifts" runat="server" Text='<%# Bind("NoOfShifts") %>' MaxLength="6"
                            Width="50px" />
                        <ajax:FilteredTextBoxExtender ID="ftbNoOfShifts" runat="server" TargetControlID="txtNoOfShifts"
                            FilterType="Custom, Numbers" ValidChars="." />
                        <asp:RequiredFieldValidator ID="rfvNoOfShifts" runat="server" ControlToValidate="txtNoOfShifts"
                            Display="Dynamic" ErrorMessage="No of Shifts is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvNoOfShifts" runat="server" ErrorMessage="No Of Shifts values must be between 0.25 to 999.99"
                            ControlToValidate="txtNoOfShifts" MinimumValue="0.25" MaximumValue="999.99" Type="double"
                            ValidationGroup="EditInfo"><</asp:RangeValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("NoOfShifts") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtNoOfShiftsGVF" runat="server" MaxLength="6" Width="50px" />
                        <ajax:FilteredTextBoxExtender ID="ftbNoOfShiftsGVF" runat="server" TargetControlID="txtNoOfShiftsGVF"
                            FilterType="Custom, Numbers" ValidChars="." />
                        <asp:RequiredFieldValidator ID="rfvNoOfShiftsGVF" runat="server" ControlToValidate="txtNoOfShiftsGVF"
                            Display="Dynamic" ErrorMessage="No of Shifts is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvNoOfShiftsGVF" runat="server" ErrorMessage="No Of Shifts values must be between 0.25 to 999.99"
                            ControlToValidate="txtNoOfShiftsGVF" MinimumValue="0.25" MaximumValue="999.99"
                            Type="double" ValidationGroup="InsertInfo"><</asp:RangeValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hrs/Shift" SortExpression="HrsPerShift">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtHrsPerShift" runat="server" Text='<%# Bind("HrsPerShift") %>'
                            MaxLength="6" Width="50px" />
                        <ajax:FilteredTextBoxExtender ID="ftbHrsPerShift" runat="server" TargetControlID="txtHrsPerShift"
                            FilterType="Custom, Numbers" ValidChars="." />
                        <asp:RequiredFieldValidator ID="rfvHrsPerShift" runat="server" ControlToValidate="txtHrsPerShift"
                            Display="Dynamic" ErrorMessage="Hrs/Shift is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvHrsPerShift" runat="server" ErrorMessage="Hrs/Shift values must be between 000.25 to 999.99"
                            ControlToValidate="txtHrsPerShift" MinimumValue="000.25" MaximumValue="999.99"
                            Type="Double" ValidationGroup="EditInfo"><</asp:RangeValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("HrsPerShift") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtHrsPerShiftGVF" runat="server" MaxLength="6" Width="50px" />
                        <ajax:FilteredTextBoxExtender ID="ftbHrsPerShiftGVF" runat="server" TargetControlID="txtHrsPerShiftGVF"
                            FilterType="Custom, Numbers" ValidChars="." />
                        <asp:RequiredFieldValidator ID="rfvHrsPerShiftGVF" runat="server" ControlToValidate="txtHrsPerShiftGVF"
                            Display="Dynamic" ErrorMessage="Hrs/Shift is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvHrsPerShiftGVF" runat="server" ErrorMessage="Hrs/Shift values must be between 000.25 to 999.99"
                            ControlToValidate="txtHrsPerShiftGVF" MinimumValue="000.25" MaximumValue="999.99"
                            Type="Double" ValidationGroup="InsertInfo"><</asp:RangeValidator>
                    </FooterTemplate>
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
        <asp:ObjectDataSource ID="odsProcessList" runat="server" SelectMethod="GetMFGCapacityProcessWC"
            TypeName="MfgProdBLL" UpdateMethod="UpdateMFGCapacityProcessWC" InsertMethod="InsertMFGCapacityProcessWC"
            OldValuesParameterFormatString="original_{0}" DeleteMethod="DeleteMFGCapacityProcessWC">
            <SelectParameters>
                <asp:QueryStringParameter Name="PID" QueryStringField="pPID" Type="Int32" />
                <asp:Parameter Name="WorkCenter" Type="Int32" />
                <asp:Parameter Name="UGNFacility" Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="WorkCenter" Type="Int32" />
                <asp:Parameter Name="original_PID" Type="Int32" />
                <asp:Parameter Name="original_WorkCenter" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="WorkCenter" Type="Int32" />
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="NoOfShifts" Type="Decimal" />
                <asp:Parameter Name="HrsPerShift" Type="Decimal" />
                <asp:Parameter Name="original_PID" Type="Int32" />
                <asp:Parameter Name="original_WorkCenter" Type="Int32" />
                <asp:Parameter Name="original_UGNFacility" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="PID" Type="Int32" />
                <asp:Parameter Name="WorkCenter" Type="Int32" />
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="NoOfShifts" Type="Decimal" />
                <asp:Parameter Name="HrsPerShift" Type="Decimal" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
