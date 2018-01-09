<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ECI_Lookup.aspx.vb" MaintainScrollPositionOnPostback="true"
    Inherits="ECI_Lookup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UGN, Inc. ECI Search</title>

    <script language="JavaScript" type="text/javascript" src="../javascripts/calendar.js"></script>

    <script language="javascript" type="text/javascript">
        // Keep the popup in focus until it gets closed.
        // This method works when the document loses focus.
        // It does not work if a form field loses focus.
        function restoreFocus() {
            if (!document.hasFocus()) {
                window.focus();
            }
        }
        onblur = restoreFocus;
    </script>

</head>
<body>
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <form runat="server" id="frmSearchECI">
        <br />
        <h1 style="text-align: center; background-color: White;">
            Search for an ECI</h1>
        <hr />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <table width="68%" style="background-color: White;">
            <tr>
                <td class="p_text" style="white-space: nowrap;" align="right">
                    ECI No.:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchECINo" runat="server" MaxLength="15"></asp:TextBox>
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    ECI Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchECIType" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Text="External"></asp:ListItem>
                        <asp:ListItem Text="Internal"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    ECI Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchStatus" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="p_text" style="white-space: nowrap;" align="right">
                    Description:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchECIDesc" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    RFD No:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchRFDNo" runat="server" MaxLength="16" Width="194px"></asp:TextBox>
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Cost Sheet ID:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchCostSheetID" runat="server" MaxLength="15" Width="194px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Drawing No.:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchDrawingNo" runat="server" MaxLength="17" Width="194px"></asp:TextBox>
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Customer Part No.:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchCustomerPartNo" runat="server" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Internal Part No.:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchPartNo" runat="server" MaxLength="15" Width="194px"></asp:TextBox>
                </td>
                <td class="p_text">
                    Part Name:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSearchPartName" runat="server" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
         
                <td class="p_text">
                    Design Level:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchDesignLevel" runat="server" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="btnSearch" runat="server" Text="Search"></asp:Button>
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset"></asp:Button>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvECIList" runat="server" DataSourceID="odsECIList" AllowPaging="True"
            Width="98%" PageSize="15" AllowSorting="True" AutoGenerateColumns="False">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EmptyDataTemplate>
                No Records Found.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnSelectECI" runat="server" CommandName="Select" ImageUrl="~/images/SelectUser.gif"
                            AlternateText="Send ECI to previous page" ToolTip="Send ECI back to parent page"
                            Visible='<%# SetECIClickable(Container.DataItem("ECINo"),Container.DataItem("StatusID")).ToString %>'
                            Target="_blank" Text='<%# Eval("ECINo") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ECINo" HeaderStyle-CssClass="none" ItemStyle-CssClass="none">
                    <HeaderStyle CssClass="none" />
                    <ItemStyle CssClass="none" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="ECI No." SortExpression="ECINo">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkViewECINo" runat="server" ToolTip='<%# Eval("ECINo", "~/ECI/ECI_Preview.aspx?ECINo={0}") %>'
                            NavigateUrl='<%# SetECIHyperlink(Container.DataItem("ECINo"),Container.DataItem("StatusID")).ToString %>'
                            Font-Underline='<%# SetECIClickable(Container.DataItem("ECINo"),Container.DataItem("StatusID")).ToString %>'
                            Target="_blank" Text='<%# Eval("ECINo") %>'>
                        </asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
                <asp:BoundField DataField="ECIType" HeaderText="Type" ReadOnly="True" SortExpression="ECIType">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="DesignDesc" HeaderText="Description" ReadOnly="True" SortExpression="DesignDesc">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="StatusName" HeaderText="Status" ReadOnly="True" SortExpression="StatusName">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="NewDrawingNo" HeaderStyle-CssClass="none" 
                    ItemStyle-CssClass="none" >
                    <HeaderStyle CssClass="none" />
                    <ItemStyle CssClass="none" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="New Drawing No." SortExpression="NewDrawingNo">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkViewNewDrawingNo" runat="server" NavigateUrl='<%# Eval("NewDrawingNo", "~/PE/DMSDrawingPreview.aspx?DrawingNo={0}") %>'
                            Font-Underline="true" Target="_blank" Text='<%# Eval("NewDrawingNo") %>'>
                        </asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
                <asp:BoundField DataField="NewPartNo" HeaderText="New Internal Part No" ReadOnly="True"
                    SortExpression="NewPartNo">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" ReadOnly="True" SortExpression="IssueDate">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsECIList" runat="server" SelectMethod="GetECISearch"
            TypeName="ECIModule" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtSearchECINo" Name="ECINo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtSearchECIDesc" Name="ECIDesc" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddSearchECIType" Name="ECIType" 
                    PropertyName="SelectedValue" Type="String" />
                <asp:ControlParameter ControlID="ddSearchStatus" Name="StatusID" PropertyName="SelectedValue"
                    Type="Int32" />
                <asp:Parameter Name="IssueDate" Type="String" />
                <asp:Parameter Name="ImplementationDate" Type="String" />
                <asp:ControlParameter ControlID="txtSearchRFDNo" Name="RFDNo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtSearchCostSheetID" Name="CostSheetID" PropertyName="Text"
                    Type="String" />
                <asp:Parameter Name="InitiatorTeamMemberID" Type="Int32" />
                <asp:ControlParameter ControlID="txtSearchDrawingNo" Name="DrawingNo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtSearchPartNo" Name="PartNo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtSearchPartName" Name="PartName" PropertyName="Text"
                    Type="String" />
                <asp:Parameter Name="Customer" Type="String" />
                <asp:ControlParameter ControlID="txtSearchCustomerPartNo" Name="CustomerPartNo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtSearchDesignLevel" Name="DesignLevel" PropertyName="Text"
                    Type="String" />
                <asp:Parameter Name="DesignationType" Type="String" />
                <asp:Parameter Name="BusinessProcessTypeID" Type="Int32" />
                <asp:Parameter Name="ProgramID" Type="Int32" />
                <asp:Parameter Name="CommodityID" Type="Int32" />
                <asp:Parameter Name="PurchasedGoodID" Type="Int32" />
                <asp:Parameter Name="ProductTechnologyID" Type="Int32" />
                <asp:Parameter Name="SubFamilyID" Type="Int32" />
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="UGNDBVendorID" Type="Int32" />
                <asp:Parameter Name="AccountManagerID" Type="Int32" />
                <asp:Parameter Name="QualityEngineerID" Type="Int32" />
                <asp:Parameter Name="filterPPAP" Type="Boolean" />
                <asp:Parameter Name="isPPAP" Type="Boolean" />
                <asp:Parameter Name="filterUgnIPP" Type="Boolean" />
                <asp:Parameter Name="isUgnIPP" Type="Boolean" />
                <asp:Parameter Name="filterCustomerIPP" Type="Boolean" />
                <asp:Parameter Name="isCustomerIPP" Type="Boolean" />
                <asp:Parameter Name="LastUpdatedOnStartDate" Type="String" />
                <asp:Parameter Name="LastUpdatedOnEndDate" Type="String" />
                <asp:Parameter Name="includeArchive" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
        </form>
    </asp:Panel>
</body>
</html>
