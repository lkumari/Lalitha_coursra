<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="AR_Price_Change_No_Accrual_Wizard_Future.aspx.vb"
    Inherits="AR_Price_Change_No_Accrual_Wizard_Future" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" />
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <table>
            <tr>
                <td class="p_text" style="white-space: nowrap;">
                    AR Event ID:
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblAREID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    UGN Facility:&nbsp;<span style="color: red">*</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvFacility" runat="server" ControlToValidate="ddUGNFacility"
                        Text="<" ErrorMessage="UGN Facility is Required." SetFocusOnError="True" ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <span style="color: red">(UGN Facility is required)</span>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Price Code:&nbsp;<span style="color: red">*</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddPriceCode" runat="server">
                        <asp:ListItem Value="A" Text="Mass Production" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="S" Text="Service"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvPriceCode" runat="server" ControlToValidate="ddPriceCode"
                        Text="<" ErrorMessage="Price Code is Required." SetFocusOnError="True" ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <span style="color: red">(Price Code is required)</span>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <hr />
        <table width="98%">
            <tr>
                <td class="p_text">
                    Future Part Number:
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                </td>
                <td class="p_text">
                    Part Description:
                </td>
                <td>
                    <asp:TextBox ID="txtPartDesc" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Team Member:
                </td>
                <td>
                    <asp:DropDownList ID="ddTeamMember" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button runat="server" ID="btnFilterPartNo" Text="Search Part List" ValidationGroup="vgSave"
                        CausesValidation="true" />
                    <asp:Button runat="server" ID="btnClearFilterPartNo" Text="Clear Part Filters" />
                    <asp:Button runat="server" ID="btnCurrentPriceChangeNoAccrualWizard" Text="Switch to Wizard for Currently Shipped Parts." CausesValidation="false" />
                </td>
            </tr>
        </table>       
        <br />
        <a href="../PF/Future_Part_Maint.aspx" target="_blank"><b><font color="blue">Click here to view the Planning and Forecasting Future part number list</font></b></a>
        <br />
        <h2 style="color:Red">
            Part selection is required.</h2>
            <br />
            <asp:label runat="server" id="lblPFTitle" text="Planning & Forecasting Future Part List"></asp:label>
        <asp:GridView runat="server" ID="gvPartNo" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="50" DataSourceID="odsPARTNO" DataKeyNames="PARTNO"
            EmptyDataText="No Planning and Forecasting Future Parts Found" CellPadding="3" Width="98%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Select PARTNO">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelect" runat="server" Enabled="true" AutoPostBack="true" ToolTip='<%# Bind("PARTNO") %>'
                            OnCheckedChanged="cbSelectFuturePartNo_OnCheckedChanged" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="PARTNO" ReadOnly="True" HeaderText="Planning and Forecasting Future Part No.">
                    <ItemStyle HorizontalAlign="center" Font-Bold="true" />
                </asp:BoundField>
                <asp:BoundField DataField="PartDesc" ReadOnly="True" HeaderText="Desc">
                    <ItemStyle HorizontalAlign="left" />
                </asp:BoundField>
                <asp:BoundField DataField="comboUpdateInfo" ReadOnly="True" HeaderText="Updated By">
                    <ItemStyle HorizontalAlign="left" />
                </asp:BoundField>
                <asp:CheckBoxField DataField="Obsolete" HeaderText="Obsolete" SortExpression="Obsolete">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:CheckBoxField>
                <asp:TemplateField HeaderText="Matches AR Module">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbMatch" runat="server" Checked='<%# Bind("isMatch") %>' Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsPARTNO" runat="server" SelectMethod="GetARFuturePartNo"
            TypeName="ARGroupModule">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                <asp:ControlParameter ControlID="txtPartNo" Name="PartNo" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtPartDesc" Name="PartDesc" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddTeamMember" Name="CreatedBy" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
            <asp:label runat="server" id="lblPendingTitle" text="Future-3 Pending to Ship Part List"></asp:label>
        <br />
           <asp:GridView runat="server" ID="gvPendingPartNo" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="50" DataSourceID="odsPendingPARTNO" DataKeyNames="PARTNO"
            EmptyDataText="No Pending to Ship Parts Found" CellPadding="3" Width="98%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Select PARTNO">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelect" runat="server" Enabled="true" AutoPostBack="true" ToolTip='<%# Bind("PARTNO") %>'
                            OnCheckedChanged="cbSelectPendingPartNo_OnCheckedChanged" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="PARTNO" ReadOnly="True" HeaderText="Pending Part No.">
                    <ItemStyle HorizontalAlign="center" Font-Bold="true" />
                </asp:BoundField>                
                <asp:TemplateField HeaderText="Matches AR Module">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbMatch" runat="server" Checked='<%# Bind("isMatch") %>' Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsPendingPARTNO" runat="server" SelectMethod="GetARPendingPartNo"
            TypeName="ARGroupModule">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                <asp:ControlParameter ControlID="txtPartNo" Name="PartNo" PropertyName="Text" Type="String" />               
                <asp:ControlParameter ControlID="ddPriceCode" Name="PRCCDE" PropertyName="SelectedValue" Type="String" />               
                <asp:ControlParameter ControlID="ddUGNFacility" Name="COMPNY" PropertyName="SelectedValue" Type="String" />               
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:RadioButtonList runat="server" ID="rbUpdateType">
            <asp:ListItem Text="Append to selected items in the AR Event" Value="A" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Replace all selected items in the AR Event" Value="R"></asp:ListItem>
        </asp:RadioButtonList>
        <asp:Button runat="server" ID="btnUpdate" Text="Update AR Event" />
        <asp:Button runat="server" ID="btnBackToAREvent" Text="Cancel" />
        <asp:Label ID="lblMessageBottom" runat="server" SkinID="MessageLabelSkin" />
    </asp:Panel>
</asp:Content>
