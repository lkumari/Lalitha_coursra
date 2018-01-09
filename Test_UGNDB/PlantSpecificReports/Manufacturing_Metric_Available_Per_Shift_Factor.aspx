<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Manufacturing_Metric_Available_Per_Shift_Factor.aspx.vb" Inherits="Manufacturing_Metric_Available_Per_Shift_Factor"
    MaintainScrollPositionOnPostback="true" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsSearch" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearch" />
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table>
            <tr>
                <td class="p_text" style="white-space: nowrap">
                    UGN Facility:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                        ErrorMessage="UGN Facility is required." Font-Bold="True" ValidationGroup="vgSave"
                        Text="<" SetFocusOnError="true" />
                </td>
                <td class="p_text">
                    Department:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddDepartment">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="98%">
            <tr>
                <td align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="true" ValidationGroup="vgSearch" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:ValidationSummary ID="vsEditAvailablePerShiftFactor" runat="server" DisplayMode="List"
            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditAvailablePerShiftFactor" />
        <asp:ValidationSummary ID="vsFooterAvailablePerShiftFactor" runat="server" DisplayMode="List"
            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterAvailablePerShiftFactor" />
        <asp:GridView ID="gvAvailablePerShiftFactor" runat="server" AutoGenerateColumns="False"
            DataKeyNames="RowID" DataSourceID="odsAvailablePerShiftFactor" Width="98%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:TemplateField HeaderText="UGNFacility" SortExpression="UGNFacility">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEditUGNFacility" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddEditUGNFacility_SelectedIndexChanged"
                            DataSource='<%# commonFunctions.GetUGNFacility("")  %>' DataValueField="UGNFacility"
                            DataTextField="ddUGNFacilityName" SelectedValue='<%# Bind("UGNFacility") %>'>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvEditUGNFacility" runat="server" ControlToValidate="ddEditUGNFacility"
                            ErrorMessage="The UGN Facility is required." Font-Bold="True" ValidationGroup="vgEditAvailablePerShiftFactor"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewUGNFacility" runat="server" Text='<%# Bind("ddUGNFacilityName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddFooterUGNFacility" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddFooterUGNFacility_SelectedIndexChanged"
                            DataSource='<%# commonFunctions.GetUGNFacility("") %>' DataValueField="UGNFacility"
                            DataTextField="ddUGNFacilityName">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvFooterUGNFacility" runat="server" ControlToValidate="ddFooterUGNFacility"
                            ErrorMessage="The UGN Facility is required." Font-Bold="True" ValidationGroup="vgFooterAvailablePerShiftFactor"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Department" SortExpression="ddDepartmentDesc">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEditDepartment" runat="server" DataValueField="CDEPT" AutoPostBack="true"
                            OnSelectedIndexChanged="ddEditDepartment_SelectedIndexChanged" DataTextField="ddDepartmentDesc">
                        </asp:DropDownList>
                        <asp:Label ID="lblEditDeptID" runat="server" Text='<%# Bind("DeptID") %>'
                            CssClass="none"></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvEditDepartment" runat="server" ControlToValidate="ddEditDepartment"
                            ErrorMessage="The Department is required." Font-Bold="True" ValidationGroup="vgEditAvailablePerShiftFactor"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewDepartment" runat="server" Text='<%# Bind("ddDepartmentDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddFooterDepartment" runat="server" DataValueField="CDEPT" DataTextField="ddDepartmentDesc">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvFooterDepartment" runat="server" ControlToValidate="ddFooterDepartment"
                            ErrorMessage="The Department is required." Font-Bold="True" ValidationGroup="vgFooterAvailablePerShiftFactor"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Available<br> Per Shift<br> Factor" SortExpression="AvailablePerShiftFactor">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditAvailablePerShiftFactor" runat="server" Text='<%# Bind("AvailablePerShiftFactor") %>'
                            MaxLength="10" Width="75px"></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvEditAvailablePerShiftFactor" Operator="DataTypeCheck"
                            ValidationGroup="vgEditAvailablePerShiftFactor" Type="double" Text="<" ControlToValidate="txtEditAvailablePerShiftFactor"
                            ErrorMessage="Available Per Shift Factor must be numeric" SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewAvailablePerShiftFactor" runat="server" Text='<%# Bind("AvailablePerShiftFactor") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFooterAvailablePerShiftFactor" runat="server" Text="" MaxLength="10"
                            Width="75px"></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvFooterAvailablePerShiftFactor" Operator="DataTypeCheck"
                            ValidationGroup="vgFooterAvailablePerShiftFactor" Type="double" Text="<" ControlToValidate="txtFooterAvailablePerShiftFactor"
                            ErrorMessage="Available Per Shift Factor must be numeric" SetFocusOnError="True" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Effective Date" SortExpression="EffectiveDate">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditEffectiveDate" runat="server" Text='<%# Bind("EffectiveDate") %>'
                            MaxLength="10" Width="75px"></asp:TextBox>
                        <asp:ImageButton runat="server" ID="imgEditEffectiveDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                        <ajax:CalendarExtender ID="cbeEditEffectiveDate" runat="server" TargetControlID="txtEditEffectiveDate"
                            PopupButtonID="imgEditEffectiveDate" />
                        <asp:RegularExpressionValidator ID="revEditEffectiveDate" runat="server" ErrorMessage='Invalid Effective Date:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                            ControlToValidate="txtEditEffectiveDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            Width="8px" ValidationGroup="vgEditAvailablePerShiftFactor"><</asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvEditEffectiveDate" ControlToValidate="txtEditEffectiveDate"
                            SetFocusOnError="true" ErrorMessage="Effective date is required" ValidationGroup="vgEditAvailablePerShiftFactor"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewEffectiveDate" runat="server" Text='<%# Bind("EffectiveDate") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFooterEffectiveDate" runat="server" MaxLength="10" Width="75px"
                            Text=""></asp:TextBox>
                        <asp:ImageButton runat="server" ID="imgFooterEffectiveDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                        <ajax:CalendarExtender ID="cbeFooterEffectiveDate" runat="server" TargetControlID="txtFooterEffectiveDate"
                            PopupButtonID="imgFooterEffectiveDate" />
                        <asp:RegularExpressionValidator ID="revFooterEffectiveDate" runat="server" ErrorMessage='Invalid Effective Date:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                            ControlToValidate="txtFooterEffectiveDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            Width="8px" ValidationGroup="vgFooterAvailablePerShiftFactor"><</asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvFooterEffectiveDate" ControlToValidate="txtFooterEffectiveDate"
                            SetFocusOnError="true" ErrorMessage="Effective date is required" ValidationGroup="vgFooterAvailablePerShiftFactor"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnPriceAvailablePerShiftFactor" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditAvailablePerShiftFactor" />
                        <asp:ImageButton ID="iBtnAvailablePerShiftFactorCancel" runat="server" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnAvailablePerShiftFactorEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterAvailablePerShiftFactor"
                            runat="server" ID="iBtnFooterAvailablePerShiftFactor" ImageUrl="~/images/save.jpg"
                            AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnAvailablePerShiftFactorUndo" runat="server" CommandName="Undo"
                            CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsAvailablePerShiftFactor" runat="server" DeleteMethod="DeleteManufacturing_MetricCustomerProgram"
            InsertMethod="InsertManufacturingMetricAvailablePerShiftFactor" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetManufacturingMetricAvailablePerShiftFactorList" TypeName="Manufacturing_Metric_Available_Per_Shift_FactorBLL"
            UpdateMethod="UpdateManufacturingMetricAvailablePerShiftFactor">
            <DeleteParameters>
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="DeptID" Type="Int32" />
                <asp:Parameter Name="AvailablePerShiftFactor" Type="Double" />
                <asp:Parameter Name="EffectiveDate" Type="String" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="ddUGNFacility" Name="UGNFacility" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddDepartment" Name="DeptID" PropertyName="SelectedValue"
                    Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="DeptID" Type="Int32" />
                <asp:Parameter Name="AvailablePerShiftFactor" Type="Double" />
                <asp:Parameter Name="EffectiveDate" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
