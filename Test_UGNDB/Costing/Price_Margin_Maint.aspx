<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Price_Margin_Maint.aspx.vb" Inherits="Price_Margin_Maint" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="98%" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server" Visible="false"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblUGNFacilityLabel" Text="UGN Facility:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacilityValue" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" Visible="false"/>
                    <asp:Button ID="btnReset" runat="server" Text="Reset" Visible="false"/>
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <asp:ValidationSummary ID="vsEditPriceMargin" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditPriceMargin" />
        <asp:ValidationSummary ID="vsFooterPriceMargin" runat="server" DisplayMode="List"
            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterPriceMargin" />
        <asp:GridView runat="server" ID="gvPriceMargin" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="100" ShowFooter="True" DataSourceID="odsPriceMargin"
            DataKeyNames="RowID" Width="100%" Visible="false">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" />
                <asp:TemplateField HeaderText="UGNFacility" SortExpression="UGNFacility">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEditUGNFacility" runat="server" DataSource='<%# commonFunctions.GetUGNFacility("")  %>'
                            DataValueField="UGNFacility" DataTextField="ddUGNFacilityName" SelectedValue='<%# Bind("UGNFacility") %>'>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvEditUGNFacility" runat="server" ControlToValidate="ddEditUGNFacility"
                            ErrorMessage="The UGN Facility is required." Font-Bold="True" ValidationGroup="vgEditPriceMargin"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewUGNFacility" runat="server" Text='<%# Bind("ddUGNFacilityName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddFooterUGNFacility" runat="server" DataSource='<%# commonFunctions.GetUGNFacility("") %>'
                            DataValueField="UGNFacility" DataTextField="ddUGNFacilityName">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvFooterUGNFacility" runat="server" ControlToValidate="ddFooterUGNFacility"
                            ErrorMessage="The UGN Facility is required." Font-Bold="True" ValidationGroup="vgFooterPriceMargin"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Minimum Price Margin" SortExpression="MinPriceMargin">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditMinPriceMargin" runat="server" Text='<%# Bind("MinPriceMargin") %>'
                            MaxLength="10" Width="50px"></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvEditMinPriceMargin" Operator="DataTypeCheck"
                            ValidationGroup="vgEditPriceMargin" Type="double" Text="<" ControlToValidate="txtEditMinPriceMargin"
                            ErrorMessage="Minimum Price Margin must be a number." SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewMinPriceMargin" runat="server" Text='<%# Bind("MinPriceMargin") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFooterMinPriceMargin" runat="server" MaxLength="10" Width="50px"></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvFooterMinPriceMargin" Operator="DataTypeCheck"
                            ValidationGroup="vgFooterPriceMargin" Type="double" Text="<" ControlToValidate="txtFooterMinPriceMargin"
                            ErrorMessage="Minimum Price Margin must be a number." SetFocusOnError="True" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Effective Date" SortExpression="EffectiveDate">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditEffectiveDate" runat="server" Text='<%# Bind("EffectiveDate") %>'
                            MaxLength="10" Width="50px"></asp:TextBox>
                        <asp:ImageButton runat="server" ID="imgEditEffectiveDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                        <ajax:CalendarExtender ID="cbeEditEffectiveDate" runat="server" TargetControlID="txtEditEffectiveDate"
                            PopupButtonID="imgEditEffectiveDate" />
                        <asp:RegularExpressionValidator ID="revEditEffectiveDate" runat="server" ErrorMessage='Invalid Effective Date:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                            ControlToValidate="txtEditEffectiveDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            Width="8px" ValidationGroup="vgEditPriceMargin"><</asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvEditEffectiveDate" ControlToValidate="txtEditEffectiveDate"
                            SetFocusOnError="true" ErrorMessage="Effective date is required" ValidationGroup="vgEditPriceMargin"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewEffectiveDate" runat="server" Text='<%# Bind("EffectiveDate") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnPriceMarginUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditPriceMargin" />
                        <asp:ImageButton ID="iBtnPriceMarginCancel" runat="server" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnPriceMarginEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterPriceMargin"
                            runat="server" ID="iBtnFooterPriceMargin" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnPriceMarginUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsPriceMargin" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetCostSheetPriceMarginList" TypeName="CostSheetPriceMarginBLL" UpdateMethod="UpdateCostSheetPriceMargin"
            InsertMethod="InsertCostSheetPriceMargin">
            <SelectParameters>
                <asp:QueryStringParameter Name="UGNFacility" QueryStringField="UGNFacility" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="MinPriceMargin" Type="Double" />
                <asp:Parameter Name="EffectiveDate" Type="String" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="MinPriceMargin" Type="Double" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
