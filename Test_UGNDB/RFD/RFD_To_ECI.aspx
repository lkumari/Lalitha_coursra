<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RFD_To_ECI.aspx.vb" Inherits="RFD_To_ECI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UGN, Inc. RFD to ECI</title>

    <script language="JavaScript" type="text/javascript" src="../javascripts/calendar.js"></script>

    <script language="javascript" type="text/javascript">
       // Keep the popup in focus until it gets closed.
       // This method works when the document loses focus.
       // It does not work if a form field loses focus.
       function restoreFocus()
       {
          if (!document.hasFocus())
          {
             window.focus();
          }
       }
       onblur=restoreFocus;
    </script>

</head>
<body>
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <form runat="server" id="frmSearchECI">
            <br />
            <h1 style="text-align: center; background-color: White;">
                Search for an ECI or Create a New ECI&nbsp;</h1>
            <hr />
            <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
            <br />
            <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
            <br />
            <table width="68%" style="background-color: White;">
                <tr>
                    <td class="p_smalltextbold" style="color: #990000" colspan="3" align="left">
                        <asp:RadioButtonList runat="server" ID="rbECIType" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Create External" Value="External" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Create Internal" Value="Internal"></asp:ListItem>
                        </asp:RadioButtonList>
                        Review existing data or press
                        <asp:Button ID="btnAdd" runat="server" Text="Add" />
                        to create a new ECI.
                        
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="p_text" style="white-space: nowrap;" align="right">
                        ECI No.:
                    </td>
                    <td>
                        <asp:TextBox ID="txtECINo" runat="server" MaxLength="30"></asp:TextBox>
                    </td>
                    <td class="p_text" style="white-space: nowrap;" align="right">
                        Description:
                    </td>
                    <td>
                        <asp:TextBox ID="txtECIDesc" runat="server" MaxLength="50"></asp:TextBox>
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
                                AlternateText="Send ECI to previous page" ToolTip="Send ECI back to parent page" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ECINo" HeaderText="ECI No." ReadOnly="True" SortExpression="ECINo">
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DesignDesc" HeaderText="Description" ReadOnly="True" SortExpression="DesignDesc">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsECIList" runat="server" SelectMethod="GetECISearch"
                TypeName="ECIModule">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtECINo" Name="ECINo" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtECIDesc" Name="ECIDesc" PropertyName="Text" Type="String" />
                    <asp:Parameter Name="ECIType" Type="String" />
                    <asp:Parameter Name="StatusID" Type="Int32" />
                    <asp:Parameter Name="IssueDate" Type="String" />
                    <asp:Parameter Name="ImplementationDate" Type="String" />
                    <asp:Parameter Name="RFDNo" Type="String" />
                    <asp:Parameter Name="CostSheetID" Type="String" />
                    <asp:Parameter Name="InitiatorTeamMemberID" Type="Int32" />
                    <asp:Parameter Name="DrawingNo" Type="String" />
                    <asp:Parameter Name="PartNo" Type="String" />
                    <asp:Parameter Name="PartName" Type="String" />
                    <asp:Parameter Name="CABBV" Type="String" />
                    <asp:Parameter Name="SoldTo" Type="Int32" />
                    <asp:Parameter Name="CustomerPartNo" Type="String" />
                    <asp:Parameter Name="DesignLevel" Type="String" />
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
