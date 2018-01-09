<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="TMWorkHistoryMaintenance.aspx.vb" Inherits="Security_TMWorkHistoryMaintenance"
    Title="Team Member Work History" Debug="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <br />
    &nbsp;&nbsp;&nbsp;
    <asp:LinkButton ID="lbTMGeneralTab" runat="server" Font-Overline="False" Font-Underline="False"
        CssClass="tab">General</asp:LinkButton>
    &nbsp;
    <asp:Label ID="lblTMWorkHistoryTab" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;Work History&nbsp;&nbsp;&nbsp;&nbsp;"
        Font-Bold="True" CssClass="selectedTab" Height="15px"></asp:Label>
    &nbsp;
    <asp:LinkButton ID="lbTMRolesTab" runat="server" CssClass="tab">Roles and Forms</asp:LinkButton>
    <!-- Search Field -->
    <hr />
    <br />
    <table width="100%">
        <tr>
            <td align="right">
                Team Member:&nbsp;
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ddlLookupUser" runat="server" AutoPostBack="True" Width="300px"
                    ToolTip="Select a Team Member">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:Label ID="lblTeamMemberId" runat="server" ForeColor="DarkGreen"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <!-- Search Results -->
    <hr />
    <asp:Label ID="lblCurrentPage" runat="server" />
    <asp:GridView ID="gvWorkHistory" runat="server" AllowPaging="True" Width="700px"
        AutoGenerateColumns="False" DataSourceID="odsWorkHistory" AllowSorting="True"
        ShowFooter="True">
        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" Wrap="True" />
        <EditRowStyle BackColor="#CCCCCC" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EmptyDataTemplate>
            No records found from the database.</EmptyDataTemplate>
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <FooterStyle HorizontalAlign="Center" Wrap="False" />
                <FooterTemplate>
                    <asp:Image ID="imgAsterisk" runat="server" ImageUrl="~/images/asterick_blue.gif"
                        AlternateText="New row" />
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Facility" SortExpression="ddUGNFacilityName">
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlFacilityEdit" runat="server" DataSourceID="odsFacilities"
                        DataValueField="UGNFacility" DataTextField="ddUGNFacilityName" SelectedValue='<%# Bind("UGNFacility") %>'
                        AppendDataBoundItems="True" ToolTip="Select a Facility">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvFacilityEdit" runat="server" ControlToValidate="ddlFacilityEdit"
                        Display="Dynamic" ErrorMessage="Please select a Facility" ValidationGroup="vgEditWHInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:DropDownList ID="ddlFacilityInsert" runat="server" DataSourceID="odsFacilities"
                        DataValueField="UGNFacility" DataTextField="ddUGNFacilityName" AppendDataBoundItems="True"
                        ToolTip="Select a Facility">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvFacilityInsert" runat="server" ControlToValidate="ddlFacilityInsert"
                        Display="Dynamic" ErrorMessage="Please select a Facility" ValidationGroup="vgInsertWHInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                </FooterTemplate>
                <FooterStyle HorizontalAlign="Left" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblUGNFacilityNamePreEdit" runat="server" Text='<%# Bind("UGNFacilityName") %>' />
                    <asp:HiddenField ID="hfUGNFacilityPreEdit" runat="server" Value='<%# Bind("UGNFacility") %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Group (AKA Subscription)" SortExpression="Subscription">
                <FooterTemplate>
                    <asp:DropDownList ID="ddlSubscriptionInsert" runat="server" DataSourceID="odsSubscriptions"
                        DataValueField="SubscriptionID" DataTextField="Subscription" AppendDataBoundItems="true"
                        ToolTip="Select a Group">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvSubscriptionInsert" runat="server" ControlToValidate="ddlSubscriptionInsert"
                        ErrorMessage="Group is required" ValidationGroup="vgInsertWHInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                </FooterTemplate>
                <FooterStyle HorizontalAlign="Left" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblSubscriptionPreEdit" runat="server" Text='<%# Bind("Subscription") %>' />
                    <asp:HiddenField ID="hfSubscriptionIDPreEdit" runat="server" Value='<%# Bind("SubscriptionID") %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Start Date" SortExpression="StartDate">
                <EditItemTemplate>
                    <asp:TextBox ID="txtStartDateEdit" runat="server" Width="90px" Text='<% # Bind("StartDate","{0:MM/dd/yyyy}") %>' />
                    <asp:ImageButton ID="imgStartDateEdit" runat="server" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="ceStartDateEdit" runat="server" TargetControlID="txtStartDateEdit"
                        PopupButtonID="imgStartDateEdit" />
                    <asp:RequiredFieldValidator ID="revStartDateEdit" runat="server" ControlToValidate="txtStartDateEdit"
                        ErrorMessage="Start Date is required" ValidationGroup="vgInsertWHInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvStartDateEdit" runat="server" ControlToValidate="txtStartDateEdit"
                        ErrorMessage="Start Date must be in date format" Operator="DataTypeCheck" Type="Date"
                        ValidationGroup="vgInsertWHInfo">
                        &lt;
                    </asp:CompareValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtStartDateInsert" runat="server" Width="90px" Text='<% # Bind("StartDate","{0:MM/dd/yyyy}") %>' />
                    <asp:ImageButton ID="imgStartDateInsert" runat="server" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="ceStartDateInsert" runat="server" TargetControlID="txtStartDateInsert"
                        PopupButtonID="imgStartDateInsert" />
                    <asp:RequiredFieldValidator ID="revStartDateInsert" runat="server" ControlToValidate="txtStartDateInsert"
                        ErrorMessage="Start Date is required" ValidationGroup="vgInsertWHInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvStartDateInsert" runat="server" ControlToValidate="txtStartDateInsert"
                        ErrorMessage="Start Date must be in date format" Operator="DataTypeCheck" Type="Date"
                        ValidationGroup="vgInsertWHInfo">
                        &lt;
                    </asp:CompareValidator>
                </FooterTemplate>
                <FooterStyle HorizontalAlign="Left" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblStartDatePreEdit" runat="server" Text='<%# Eval("StartDate","{0:MM/dd/yyyy}") %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="End Date" SortExpression="EndDate">
                <EditItemTemplate>
                    <asp:TextBox ID="txtEndDateEdit" runat="server" Width="90px" Text='<% # Bind("EndDate","{0:MM/dd/yyyy}") %>' />
                    <asp:ImageButton ID="imgEndDateEdit" runat="server" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="ceEndDateEdit" runat="server" TargetControlID="txtEndDateEdit"
                        PopupButtonID="imgEndDateEdit" />
                    <asp:CompareValidator ID="cvEndDateEdit" runat="server" ControlToValidate="txtEndDateEdit"
                        ErrorMessage="End date must be in date format" Operator="DataTypeCheck" Type="Date"
                        ValidationGroup="vgEditWHInfo">
                        &lt;
                    </asp:CompareValidator>
                </EditItemTemplate>
                <FooterStyle HorizontalAlign="Left" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblEndDatePreEdit" runat="server" Text='<%# Eval("EndDate","{0:MM/dd/yyyy}") %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Update" SortExpression="comboUpdateInfo">
                <ItemTemplate>
                    <asp:Label ID="lblComboUpdateInfoPreEdit" runat="server" Text='<%# Eval("comboUpdateInfo") %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False" AccessibleHeaderText="Button Column">
                <EditItemTemplate>
                    <asp:ImageButton ID="ibtnUpdate" runat="server" CommandName="UpdateCustom" CausesValidation="True"
                        ValidationGroup="vgEditWHInfo" ImageUrl="~/images/save.jpg" ToolTip="Save changes"
                        AlternateText="Save changes" OnClick="ibtnUpdate_Click" />
                    <asp:ImageButton ID="ibtnCancelEdit" runat="server" CommandName="Cancel" CausesValidation="False"
                        ImageUrl="~/images/undo-transparent.gif" ToolTip="Undo changes" AlternateText="Undo changes" />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:ImageButton ID="ibtnInsert" runat="server" CommandName="InsertCustom" CausesValidation="True"
                        ValidationGroup="vgInsertWHInfo" ImageUrl="~/images/save.jpg" ToolTip="Save new row"
                        AlternateText="Save new row" />
                    <asp:ImageButton ID="ibtnCancelInsert" runat="server" CommandName="Cancel" CausesValidation="False"
                        ImageUrl="~/images/undo-transparent.gif" ToolTip="Undo changes" AlternateText="Undo changes" />
                </FooterTemplate>
                <FooterStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:ImageButton ID="ibtnEdit" runat="server" CommandName="Edit" CausesValidation="False"
                        ImageUrl="~/images/edit.jpg" ToolTip="Edit row" AlternateText="Edit row" />
                    <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="DeleteCustom" CausesValidation="False"
                        ImageUrl="~/images/delete.jpg" ToolTip="Delete row" AlternateText="Delete row"
                        OnClick="ibtnDelete_Click" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    <asp:Label ID="lblStatus" runat="server" /><br />
    <asp:ValidationSummary ID="vsEditWHInfo" runat="server" ValidationGroup="vgEditWHInfo"
        ShowMessageBox="True" />
    <br />
    <asp:ValidationSummary ID="vsInsertWHInfo" runat="server" ValidationGroup="vgInsertWHInfo"
        ShowMessageBox="True" />
    <br />
    <asp:HiddenField ID="hfTeamMemberId" runat="server" />
    <br />
    <asp:ObjectDataSource ID="odsWorkHistory" runat="server" SelectMethod="GetTMWorkHistory"
        TypeName="SecurityModule" OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlLookupUser" Name="TeamMemberID" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:Parameter Name="SubscriptionID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsFacilities" runat="server" SelectMethod="GetFacilities"
        TypeName="SecurityModule" />
    <asp:ObjectDataSource ID="odsSubscriptions" runat="server" SelectMethod="GetSubscriptions"
        TypeName="SecurityModule" />
</asp:Content>
