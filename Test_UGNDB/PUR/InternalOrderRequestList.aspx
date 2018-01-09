<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="InternalOrderRequestList.aspx.vb" Inherits="IOR_InternalOrderRequestList"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1250px" DefaultButton="btnSearch">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False"></asp:Label>
        <table>
            <%--<tr>
                <td class="p_smalltextbold" style="color: #990000">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.
                </td>
            </tr>--%>
            <tr>
                <td style="font-size:small; color: #990000">
                    <b>This system is not set up for 1 time Vendor purchase(s). A manual
                    IOR will be required for submission until the next upgrade release of the E-IOR.</b>
                </td>
            </tr>
        </table>
        <hr />
        <i>Partial Searches can be completed by placing % before or after text.</i>
        <table width="100%" border="0">
            <tr>
                <td class="p_text">
                    Reference #:
                </td>
                <td>
                    <asp:TextBox ID="txtIORNo" runat="server" MaxLength="15" Width="100px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbIORNO" runat="server" TargetControlID="txtIORNo"
                        FilterType="Custom" ValidChars="1234567890,%" />
                </td>
                <td class="p_text">
                    Description / Notes:
                </td>
                <td>
                    <asp:TextBox ID="txtIORDescription" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbIORDescription" runat="server" TargetControlID="txtIORDescription"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-/% " />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Requested By:
                </td>
                <td>
                    <asp:DropDownList ID="ddRequestedBy" runat="server" />
                </td>
                <td class="p_text">
                    Buyer:
                </td>
                <td>
                    <asp:DropDownList ID="ddBuyer" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Appropriation (A, D, P, T, R):
                </td>
                <td>
                    <asp:TextBox ID="txtAppropriationCode" runat="server" MaxLength="15" Width="100px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbAppCode" runat="server" TargetControlID="txtAppropriationCode"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%." />
                </td>
                <td class="p_text">
                    Purchase Order No:
                </td>
                <td>
                    <asp:TextBox ID="txtPONo" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftPONo" runat="server" TargetControlID="txtPONo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Vendor Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddVendorType" runat="server" />
                </td>
                 <td class="p_text">
                    Department or Cost Center:
                </td>
                <td>
                    <asp:TextBox ID="txtDepartment" runat="server" MaxLength="50" Width="250px"/>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Vendor:
                </td>
                <td>
                    <asp:DropDownList ID="ddVendor" runat="server" />
                </td>
                <td class="p_text">
                    G/L Account:
                </td>
                <td>
                    <asp:DropDownList ID="ddGLAccount" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    UGN Location:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                </td>
                <td class="p_text">
                    Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddIORStatus" runat="server">
                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="NOpen">New Record</asp:ListItem>
                        <asp:ListItem Value="AApproved">Approved</asp:ListItem>
                        <asp:ListItem Value="ACompleted">Completed</asp:ListItem>
                        <asp:ListItem Value="CClosed">Closed</asp:ListItem>
                        <asp:ListItem Value="TIn Process">In Process</asp:ListItem>
                        <asp:ListItem Value="RIn Process">Rejected</asp:ListItem>
                        <asp:ListItem Value="VVoid">Void</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Date Submitted From:
                </td>
                <td>
                    <asp:TextBox ID="txtDateSubFrom" runat="server" MaxLength="12" Width="80px" />
                    <asp:ImageButton runat="server" ID="imgDateSubFrom" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeDateSubFrom" runat="server" TargetControlID="txtDateSubFrom"
                        PopupButtonID="imgDateSubFrom" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revDateSubFrom" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtDateSubFrom" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsList"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvDateSubFrom" runat="server" ErrorMessage="Date Submitted From must be less than Date Submitted To."
                        ControlToCompare="txtDateSubTo" ControlToValidate="txtDateSubFrom" Operator="LessThan"
                        Type="Date" ValidationGroup="vsList"><</asp:CompareValidator>
                </td>
                <td class="p_text">
                    Date Submitted To:
                </td>
                <td>
                    <asp:TextBox ID="txtDateSubTo" runat="server" MaxLength="12" Width="80px" />
                    <asp:ImageButton runat="server" ID="imgDateSubTo" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeDateSubTo" runat="server" TargetControlID="txtDateSubTo"
                        PopupButtonID="imgDateSubTo" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revDateSubTo" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtDateSubTo" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsList"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvDateSubTo" runat="server" ControlToCompare="txtDateSubFrom"
                        ControlToValidate="txtDateSubTo" ErrorMessage="Date Submitted To must be greater than Date Submitted From."
                        Operator="GreaterThan" Type="Date" ValidationGroup="vsList"><</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Submitted By:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddSubmittedBy" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CommandName="search" CausesValidation="true"
                        ValidationGroup="vsList" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CommandName="reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="summaryList" runat="server" ValidationGroup="vsList" ShowMessageBox="true" />
        <hr />
        <i>Use the parameters above to filter the list below.</i>
        <table width="480px" border="0">
            <tr>
                <td width="80px" align="center" style="white-space: nowrap;">
                    Closed
                </td>
                <td width="80px" align="center" style="background-color: Fuchsia; white-space: nowrap;">
                    New Record
                </td>
                <td width="80px" align="center" style="background-color: yellow; white-space: nowrap;">
                    In-Process
                </td>
                <td width="80px" align="center" style="background-color: lime; white-space: nowrap;">
                    Approved
                </td>
                <td width="80px" align="center" style="background-color: Aqua; white-space: nowrap;">
                    Completed
                </td>
                <td width="80px" align="center" style="background-color: red; color: white; white-space: nowrap;">
                    Rejected
                </td>
                <td width="80px" align="center" style="background-color: gray; color: white; white-space: nowrap;">
                    Void
                </td>
            </tr>
        </table>
        <table id="TABLE1">
            <tbody>
                <tr>
                    <td class="c_text" style="font-style: italic" colspan="4">
                        <asp:Label ID="lblRecListed" runat="server" Text="Records Listed: " />
                        <asp:Label ID="lblFromRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblTo" runat="server" Text=" to " />
                        <asp:Label ID="lblToRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblOf" runat="server" Text=" of " />
                        <asp:Label ID="lblTotalRecords" runat="server" ForeColor="Red" />
                    </td>
                    <td align="right" colspan="9">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgPUR" ErrorMessage="Numeric Value Required." SetFocusOnError="True"
                            ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgPUR" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="13">
                        &nbsp;<asp:Repeater ID="rpIOR" runat="server">
                            <SeparatorTemplate>
                                <tr>
                                    <td colspan="13">
                                        <hr style="height: 0.01em" />
                                    </td>
                                </tr>
                            </SeparatorTemplate>
                            <HeaderTemplate>
                                <tr>
                                    <td class="p_tablebackcolor" width="80px">
                                        <asp:Label ID="Label4" runat="server">Status</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label3" runat="server">Ref #</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label6" runat="server">Description</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label5" runat="server">Requested By</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label1" runat="server">UGN Location</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label10" runat="server">Date Submitted</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label11" runat="server">Submitted By</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label9" runat="server">Vendor</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="lnkProgram" runat="server">Appropriation</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label8" runat="server">Total Extension</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center" width="90px">
                                        <asp:Label ID="Label2" runat="server">PO #</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center" width="50px">
                                        <asp:Label ID="lnkVolume" runat="server">Preview</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center" width="50px">
                                        <asp:Label ID="Label7" runat="server">History</asp:Label>
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="background-color: <%# SetBackGroundColor(DataBinder.Eval(Container, "DataItem.RoutingStatus"),DataBinder.Eval(Container, "DataItem.IORStatus"))%>;">
                                        <asp:HyperLink ID="HyperLink1" Font-Underline="true" runat="server" NavigateUrl='<%# "InternalOrderRequest.aspx?pIORNo=" & DataBinder.Eval (Container.DataItem,"IORNO").tostring & "&pProjNo=" & DataBinder.Eval (Container.DataItem,"AppropriationCode").tostring %>'
                                            ForeColor='<%# SetTextColor(DataBinder.Eval(Container, "DataItem.RoutingStatus")) %>'>
                         <%#DataBinder.Eval(Container, "DataItem.IORStatusDesc")%>        
                                        </asp:HyperLink>
                                    </td>
                                    <td align="center">
                                        <%#DataBinder.Eval(Container, "DataItem.IORNO")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.IORDescription")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.RequestedByName")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.UGNFacilityName")%>
                                    </td>
                                    <td align="center">
                                        <%#DataBinder.Eval(Container, "DataItem.SubmittedOn")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.SubmittedByName")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.VendorName")%>
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="selectRecIDPending" runat="server" Target="_blank" ForeColor='<%# SetCapExForeColor(DataBinder.Eval(Container, "DataItem.IORNO"),DataBinder.Eval(Container, "DataItem.AppropriationCode")) %>'
                                            Font-Underline='<%# SetCapExFontUnderline(DataBinder.Eval(Container, "DataItem.IORNO"),DataBinder.Eval(Container, "DataItem.AppropriationCode")) %>'
                                            NavigateUrl='<%# GoToCapEx(DataBinder.Eval(Container, "DataItem.IORNO"),DataBinder.Eval(Container, "DataItem.AppropriationCode"))  %>'><%#DataBinder.Eval(Container, "DataItem.AppropriationCode")%></asp:HyperLink>
                                    </td>
                                    <td align="right">
                                        $
                                        <%#format(DataBinder.Eval(Container, "DataItem.TotalExpense"), "#,##0.00")%>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblPONo" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.PONo")%>'
                                            Visible='<%# ShowHidePONo(DataBinder.Eval(Container, "DataItem.IORStatus"),DataBinder.Eval(Container, "DataItem.RequestedByTMID")) %>' />
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="lnkPreview" runat="server" NavigateUrl='<%# "crViewInternalOrderRequest.aspx?pIORNo=" & DataBinder.Eval (Container.DataItem,"IORNO").tostring & "&pBuyer=" &  ViewState("iBuyerID") %>'
                                            Target="_blank" ImageUrl="~/images/PreviewUp.jpg"></asp:HyperLink>
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="lnkHistory" runat="server" NavigateUrl='<%# "InternalOrderRequestHistory.aspx?pIORNo=" & DataBinder.Eval (Container.DataItem,"IORNO").tostring & "&pAprv=" & ViewState("pAprv") %>'
                                            ImageUrl="~/images/History.jpg" Visible='<%# ShowHideHistory(DataBinder.Eval(Container, "DataItem.IORStatus")) %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="13">
                                        <hr style="height: 0.01em" />
                                    </td>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
