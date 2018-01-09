<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DrawingLookUp.aspx.vb" MaintainScrollPositionOnPostback="true"
    Inherits="DrawingLookUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UGN, Inc. Drawing Look Up</title>

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
    <form id="form1" runat="server" defaultbutton="btnSearch">
    <ajax:ToolkitScriptManager runat="Server" ID="ScriptManager1" />
    <br />
    <h1 style="text-align: center; background-color: White;">
        Lookup Drawing Numbers
    </h1>
    <hr />
    <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
    <br />
    <table>
        <tr>
            <td class="p_text">
                Drawing No:
            </td>
            <td>
                <asp:TextBox ID="txtDrawingNo" runat="server" Width="200" MaxLength="18"></asp:TextBox>
            </td>
            <td class="p_text" align="right">
                Customer Part No:
            </td>
            <td>
                <asp:TextBox ID="txtCustomerPartNo" runat="server" Width="200" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="p_text">
                Internal Part No:
            </td>
            <td>
                <asp:TextBox ID="txtPartNo" runat="server" Width="200" MaxLength="17"></asp:TextBox>
            </td>
            <td class="p_text">
                Internal Part Name:
            </td>
            <td>
                <asp:TextBox ID="txtPartName" runat="server" Width="200" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="p_text">
                Commodity:
            </td>
            <td>
                <asp:DropDownList ID="ddCommodity" runat="server">
                </asp:DropDownList>
                {Commodity / Classification}
            </td>
            <td class="p_text">
                Designation Type:
            </td>
            <td>
                <asp:DropDownList ID="ddDesignationType" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="p_text">
                Make:
            </td>
            <td>
                <asp:DropDownList ID="ddMake" runat="server" AutoPostBack="true">
                </asp:DropDownList>
            </td>
            <td class="p_text">
                Various Text Fields:
            </td>
            <td>
                <asp:TextBox ID="txtNotes" runat="server" Width="200" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="p_text">
                Drawing Release Type:
            </td>
            <td>
                <asp:DropDownList ID="ddReleaseType" runat="server">
                </asp:DropDownList>
            </td>
            <!--  asp:ListItem Value="A" Text="Approved" / -->
            <!--  asp:ListItem Value="P" Text="Pending" / -->
            <!--  asp:ListItem Value="R" Text="Rejected" / -->
            <!-- asp:ListItem Value="W" Text="Waived" / -->
            <!-- asp:ListItem Value="M" Text="Waiting for My Approval" / -->
            <td class="p_text">
                Drawing Status:
            </td>
            <td>
                <asp:DropDownList ID="ddStatus" runat="server">
                    <asp:ListItem Selected="True" />
                    <asp:ListItem Value="I" Text="Issued" />
                    <asp:ListItem Value="N" Text="New" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="p_text">
                Program:
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ddProgram" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:CheckBox runat="server" ID="cbAdvancedSearch" Text="Advanced Search" AutoPostBack="true" />
    <table runat="Server" id="tblAdvancedSearch" visible="false">
        <tr>
            <td class="p_text">
                Customer :
            </td>
            <td>
                <asp:DropDownList ID="ddCustomer" runat="server">
                </asp:DropDownList>
            </td>
            <td class="p_text">
                Year:
            </td>
            <td>
                <asp:DropDownList ID="ddYear" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="p_text">
                Construction:
            </td>
            <td>
                <asp:TextBox ID="txtConstruction" runat="server" Width="150" MaxLength="50"></asp:TextBox>
            </td>
            <td class="p_text">
                Drawing By Engineer:
            </td>
            <td>
                <asp:DropDownList ID="ddDrawingByEngineer" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="p_text">
                Family-SubFamily:
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ddSubFamily" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="p_text">
                Purchased Good:
            </td>
            <td>
                <asp:DropDownList ID="ddPurchasedGood" runat="server">
                </asp:DropDownList>
            </td>
            <td class="p_text">
                Density Value:
            </td>
            <td>
                <asp:DropDownList ID="ddDensityValue" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="p_text">
                Product Technology:
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ddProductTechnology" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="p_text">
                Last Updated On<br />
                (Begin Range):
            </td>
            <td>
                <asp:TextBox ID="txtLastUpdatedOnStart" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                <asp:ImageButton runat="server" ID="imgLastUpdatedOnStart" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                <ajax:CalendarExtender ID="cbeLastUpdatedOnStart" runat="server" TargetControlID="txtLastUpdatedOnStart"
                    PopupButtonID="imgLastUpdatedOnStart" />
                <asp:RegularExpressionValidator ID="revLastUpdatedOnStart" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                    ControlToValidate="txtLastUpdatedOnStart" Font-Bold="True" ToolTip="MM/DD/YYYY"
                    ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                    Width="8px" ValidationGroup="vgDrawing"><</asp:RegularExpressionValidator>
            </td>
            <td class="p_text">
                Last Updated On
                <br />
                (End Range):
            </td>
            <td>
                <asp:TextBox ID="txtLastUpdatedOnEnd" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                <asp:ImageButton runat="server" ID="imgLastUpdatedOnEnd" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtLastUpdatedOnEnd"
                    PopupButtonID="imgLastUpdatedOnEnd" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                    ControlToValidate="txtLastUpdatedOnEnd" Font-Bold="True" ToolTip="MM/DD/YYYY"
                    ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                    Width="8px" ValidationGroup="vgDrawing"><</asp:RegularExpressionValidator>
            </td>
        </tr>
    </table>
    <table width="98%">
        <tr>
            <td align="center">
                <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgDrawing" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" />
            </td>
        </tr>
    </table>
    <hr />
    <br />
    <asp:GridView ID="gvDrawings" runat="server" DataSourceID="odsDrawings" Width="98%"
        AllowSorting="True" PageSize="15" AutoGenerateColumns="False" AllowPaging="True">
        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <EmptyDataTemplate>
            No records found.</EmptyDataTemplate>
        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#CCCCCC" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:ImageButton ID="ibtnSelectUser" runat="server" CommandName="Select" ImageUrl="~/images/SelectUser.gif"
                        AlternateText="Send Drawing Number data back to previous page" ToolTip="Send Drawing Number data back to parent page" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ddDrawingNo" HeaderStyle-CssClass="none" ItemStyle-CssClass="none">
                <HeaderStyle CssClass="none"></HeaderStyle>
                <ItemStyle CssClass="none"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="Drawing No." SortExpression="DrawingNo">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkViewDrawingNo" runat="server" NavigateUrl='<%# Eval("DrawingNo", "~/PE/DMSDrawingPreview.aspx?DrawingNo={0}") %>'
                        Font-Underline="true" Target="_blank" Text='<%# Eval("DrawingNo") %>'>
                    </asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="center" />
            </asp:TemplateField>
            <asp:BoundField DataField="ddReleaseTypeName" HeaderText="Release Type" SortExpression="ddReleaseTypeName" />
            <asp:BoundField DataField="OldPartName" HeaderText="Old Drawing Name" SortExpression="OldPartName" />
            <asp:BoundField DataField="PartNo" HeaderText="Internal Part No." SortExpression="PartNo" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsDrawings" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetDrawings" TypeName="DrawingsBLL">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtDrawingNo" Name="DrawingNo" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="ddReleaseType" Name="ReleaseTypeID" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="txtPartNo" Name="PartNo" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtPartName" Name="PartName" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtCustomerPartNo" Name="CustomerPartNo" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="ddCustomer" Name="Customer" PropertyName="SelectedValue"
                Type="String" />
            <asp:ControlParameter ControlID="ddDesignationType" Name="DesignationType" PropertyName="SelectedValue"
                Type="String" />
            <asp:ControlParameter ControlID="ddYear" Name="VehicleYear" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="ddProgram" Name="ProgramID" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="ddSubFamily" Name="SubFamilyID" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="ddCommodity" Name="CommodityID" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="ddPurchasedGood" Name="PurchasedGoodID" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="ddDensityValue" Name="DensityValue" PropertyName="SelectedValue"
                Type="Double" />
            <asp:ControlParameter ControlID="txtConstruction" Name="Construction" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="ddStatus" Name="ApprovalStatus" PropertyName="SelectedValue"
                Type="String" />
            <asp:ControlParameter ControlID="txtNotes" Name="Notes" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="ddDrawingByEngineer" Name="DrawingByEngineerID"
                PropertyName="SelectedValue" Type="Int32" />
            <asp:Parameter Name="Obsolete" Type="Boolean" />
            <asp:ControlParameter ControlID="txtLastUpdatedOnStart" Name="DrawingDateStart" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtLastUpdatedOnEnd" Name="DrawingDateEnd" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="ddMake" Name="Make" PropertyName="SelectedValue"
                Type="String" />
            <asp:ControlParameter ControlID="ddProductTechnology" Name="ProductTechnologyID"
                PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    </form>
</body>
</html>
