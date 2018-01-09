<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="SampleTrialEvent.aspx.vb" Inherits="SampleTrialEvent_Maint" MaintainScrollPositionOnPostback="True"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table width="60%">
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblOEMMfg" runat="server" Text="Customer:" />
                </td>
                <td>
                    <asp:TextBox ID="txtOEMMfg" runat="server" Width="200px" MaxLength="50" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblTrialEvent" runat="server" Text="Trial Event:" />
                </td>
                <td>
                    <asp:TextBox ID="txtTrialEvent" runat="server" Width="200px" MaxLength="25" />
                </td>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                    </td>
                </tr>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvTrialEvent" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="TEID,TrialEvent,OEMManufacturer" DataSourceID="odsTrialEvent"
            EmptyDataText="No records found." OnRowCommand="gvTrialEvent_RowCommand" PageSize="50"
            SkinID="StandardGrid" Width="800px">
            <Columns>
                <asp:BoundField DataField="TEID" HeaderText="TEID" ReadOnly="True" SortExpression="TEID"
                    InsertVisible="False" Visible="False" />
                <asp:TemplateField HeaderText="Customer" SortExpression="OEMManufacturer" HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddOEMMgf1" runat="server" DataSource='<%# commonFunctions.GetOEMManufacturer("") %>'
                            DataValueField="OEMManufacturer" DataTextField="ddOEMManufacturer" SelectedValue='<%# Bind("OEMManufacturer") %>'
                            AppendDataBoundItems="True">
                            <asp:ListItem Selected="True" Value="" Text="">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvOEMMfg1" runat="server" ControlToValidate="ddOEMMgf1"
                            Display="Dynamic" ErrorMessage="Customer is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblitOEMMfg" runat="server" Text='<%# Bind("ddOEMManufacturer") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddOEMMfgInsert" runat="server" DataSource='<%# commonFunctions.GetOEMManufacturer("") %>'
                            DataValueField="OEMManufacturer" DataTextField="ddOEMManufacturer" AppendDataBoundItems="True">
                            <asp:ListItem Selected="True" Value="" Text="">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvOMGMfg" runat="server" ControlToValidate="ddOEMMfgInsert"
                            Display="Dynamic" ErrorMessage="Customer is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Trial Event" SortExpression="TrialEvent" HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTrialEventEdit" runat="server" Text='<%# Bind("TrialEvent") %>'
                            MaxLength="50" Width="300px" />
                        <asp:RequiredFieldValidator ID="rfvTrialEvent" runat="server" ControlToValidate="txtTrialEventEdit"
                            Display="Dynamic" ErrorMessage="Trial Event is a required field." Font-Bold="True"
                            Font-Size="Medium" ValidationGroup="EditInfo"> < </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblTrialEventDisplay" runat="server" Text='<%# Bind("TrialEvent") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtTrialEventInsert" runat="server" MaxLength="50" Width="300px" />
                        <asp:RequiredFieldValidator ID="rfvTrialEvent" runat="server" ControlToValidate="txtTrialEventInsert"
                            Display="Dynamic" ErrorMessage="Trial Event is a required field." ValidationGroup="InsertInfo"> < </asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
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
                            CausesValidation="True" CommandName="Update" ImageUrl="~/images/save.jpg" ValidationGroup="EditSampleTrialEventInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="Cancel" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" ValidationGroup="EditInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" />
                        &nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnInsert" runat="server" AlternateText="Insert"
                            CausesValidation="true" CommandName="Insert" ImageUrl="~/images/save.jpg" ValidationGroup="InsertInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" AlternateText="Undo"
                            CommandName="Undo" ImageUrl="~/images/undo-gray.jpg" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ValidationSummary ID="vsEditInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EditInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="InsertInfo" />
        <br />
        <asp:ValidationSummary ID="vsEmptyInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EmptyInfo" />
        <asp:ObjectDataSource ID="odsTrialEvent" runat="server" SelectMethod="GetSampleTrialEvent"
            TypeName="PGMBLL" OldValuesParameterFormatString="original_{0}" InsertMethod="InsertSampleTrialEvent"
            UpdateMethod="UpdateSampleTrialEvent">
            <UpdateParameters>
                <asp:Parameter Name="TrialEvent" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_TEID" Type="Int32" />
                <asp:Parameter Name="original_TrialEvent" Type="String" />
                <asp:Parameter Name="OEMManufacturer" Type="String" />
                <asp:Parameter Name="original_OEMManufacturer" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="TrialEvent" QueryStringField="sTE" Type="String" />
                <asp:QueryStringParameter Name="OEMMfg" QueryStringField="sCBU" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="TrialEvent" Type="String" />
                <asp:Parameter Name="OEMManufacturer" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
        &nbsp;&nbsp;
    </asp:Panel>
</asp:Content>
