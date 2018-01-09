<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="PlannerCodeMaint.aspx.vb" Inherits="MfgProd_PlannerCodeMaint" Title="UGNDB - Planner Codes"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch" Width="1000px">
        <asp:Label ID="lblErrors" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <hr />
        <br />
        <table>
            <tr>
                <td class="p_text">
                    Facility:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="2">
                    <asp:Button ID="btnSearch" runat="server" Text="Submit" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:Label ID="lblRaiseError" runat="server" Text="" Visible="false" SkinID="MessageLabelSkin" /><br />
        <br />
        <asp:Label ID="PagingInformation" runat="server" Text="" /><asp:DropDownList ID="PageList"
            runat="server" CssClass="c_textxsmall" AutoPostBack="true" OnSelectedIndexChanged="PageList_SelectedIndexChanged" />
        <asp:GridView ID="gvPlannerCode" runat="server" AutoGenerateColumns="False" DataSourceID="odsPlannerCode"
            DataKeyNames="PlannerID" AllowPaging="True" AllowSorting="True" CssClass="c_smalltext"
            PageSize="30" Width="800px" EmptyDataRowStyle-Font-Size="Medium" EmptyDataRowStyle-Font-Bold="true"
            EmptyDataRowStyle-ForeColor="Red" OnRowDataBound="PlannerCode_DataBound">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EmptyDataTemplate>
                No records found for the combination above.
            </EmptyDataTemplate>
            <EmptyDataRowStyle Font-Bold="True" Font-Size="Medium" ForeColor="Red" />
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
                    <HeaderStyle Width="70px" />
                </asp:TemplateField>
                <asp:BoundField DataField="UGNFacilityName" HeaderText="Facility" ItemStyle-CssClass="c_textxsmall"
                    ReadOnly="true" SortExpression="UGNFacilityName">
                    <ItemStyle CssClass="c_textxsmall" />
                </asp:BoundField>
                 <asp:BoundField DataField="PlannerCode" HeaderText="Planner Code" SortExpression="PlannerCode" ReadOnly="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                       
                    </asp:BoundField>
              
                <asp:TemplateField HeaderText="Planner Desc" SortExpression="PlannerDesc">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEPlannerDesc" runat="server" CssClass="c_textxsmall" MaxLength="30"
                            Text='<%# Bind("PlannerDesc") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("PlannerDesc") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="In Use?" SortExpression="NotUsed" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddENotUsed" CssClass="c_textxsmall" runat="server" SelectedValue='<%# Bind("NotUsed") %>'>
                            <asp:ListItem Value="True">NO</asp:ListItem>
                            <asp:ListItem Value="False">YES</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label8" runat="server" Text='<%# Bind("NotUsedDisplay") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dept/Cell" SortExpression="DeptCell">
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("DeptCell") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("DeptCell") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle CssClass="c_textxsmall" Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="comboUpdateInfo" HeaderStyle-HorizontalAlign="Center"
                    HeaderStyle-Wrap="true" HeaderText="Last Update" ItemStyle-HorizontalAlign="left"
                    ReadOnly="True" SortExpression="comboUpdateInfo">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsPlannerCode" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetPlannerCode" TypeName="MfgProdBLL" 
            UpdateMethod="UpdatePlannerCode">
            <UpdateParameters>
                <asp:Parameter Name="DeptCell" Type="String" />
                <asp:Parameter Name="PlannerDesc" Type="String" />
                <asp:Parameter Name="NotUsed" Type="Boolean" />
                <asp:Parameter Name="original_PlannerID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="ddUGNFacility" Name="UGNFacility" 
                    PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:ValidationSummary ID="vsEditInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EditInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="InsertInfo" />
        <asp:ValidationSummary ID="vsEmptyInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EmptyInfo" />
    </asp:Panel>
</asp:Content>
