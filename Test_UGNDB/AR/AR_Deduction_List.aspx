<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AR_Deduction_List.aspx.vb" Inherits="AR_Deduction_List" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1100px" DefaultButton="btnSearch">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False"></asp:Label>
        <table>
            <% If ViewState("pAprv") = 1 Then%>
            <tr>
                <td>
                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="c_text" ForeColor="blue">Go Back to Approval</asp:HyperLink>
                </td>
            </tr>
            <% End If%>
            <tr>
                <td class="p_smalltextbold" style="color: #990000">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.
                </td>
            </tr>
        </table>
        <hr />
        <i>Partial Searches can be completed by placing % before or after text.</i>
        <table width="100%" border="0">
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblRecNo" runat="server" Text="Rec No:" />
                </td>
                <td>
                    <asp:TextBox ID="txtARDID" runat="server" MaxLength="15" Width="100px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbARDID" runat="server" TargetControlID="txtARDID"
                        FilterType="Custom" ValidChars="1234567890%" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblReferenceNo" runat="server" Text="Reference No:" />
                </td>
                <td>
                    <asp:TextBox ID="txtReferenceNo" runat="server" MaxLength="15" Width="100px" />
                    <ajax:FilteredTextBoxExtender ID="ftbReferenceNo" runat="server" TargetControlID="txtReferenceNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890%" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblDateSentFrom" runat="server" Text="Date Sent From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDateSubFrom" runat="server" MaxLength="12" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbeDateSubFrom" runat="server" TargetControlID="txtDateSubFrom"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgDateSubFrom" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeDateSubFrom" runat="server" TargetControlID="txtDateSubFrom"
                        PopupButtonID="imgDateSubFrom" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revDateSubFrom" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtDateSubFrom" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsList"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvDateSubFrom" runat="server" ErrorMessage="Date Sent From must be less than Date Sent To."
                        ControlToCompare="txtDateSubTo" ControlToValidate="txtDateSubFrom" Operator="LessThanEqual"
                        Type="Date" ValidationGroup="vsList"><</asp:CompareValidator>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblDateSentTo" runat="server" Text="Date Sent To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDateSubTo" runat="server" MaxLength="12" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbeDateSubTo" runat="server" TargetControlID="txtDateSubTo"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgDateSubTo" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeDateSubTo" runat="server" TargetControlID="txtDateSubTo"
                        PopupButtonID="imgDateSubTo" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revDateSubTo" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtDateSubTo" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsList"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvDateSubTo" runat="server" ControlToCompare="txtDateSubFrom"
                        ControlToValidate="txtDateSubTo" ErrorMessage="Date Sent To must be greater than Date Sent From."
                        Operator="GreaterThanEqual" Type="Date" ValidationGroup="vsList"><</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblSubmittedBy" runat="server" Text="Submitted By:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddSubmittedBy" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblUGNLocation" runat="server" Text="UGN Location:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text" style="width: 160px">
                    <asp:Label ID="lblCustomer" runat="server" Text="Customer:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddCustomer" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblRecordStatus" runat="server" Text="Record Status:" />
                </td>
                <td class="c_textbold" style="color: red;" colspan="3">
                    <asp:DropDownList ID="ddRecStatus" runat="server">
                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="NOpen">New Record</asp:ListItem>
                        <asp:ListItem Value="CClosed">Closed</asp:ListItem>
                        <asp:ListItem Value="6Closed @60 days">Closed @60 days</asp:ListItem>
                        <asp:ListItem Value="TIn Process">In Process</asp:ListItem>
                        <asp:ListItem Value="I6C">Issued</asp:ListItem>
                        <asp:ListItem Value="OIRA">Outstanding</asp:ListItem>
                        <%-- <asp:ListItem Value="RIn Process">Rejected</asp:ListItem>--%>
                        <asp:ListItem Value="AApproved">Review Completed</asp:ListItem>
                        <asp:ListItem Value="VVoid">Void</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReasonDeduction" runat="server" Text="Reason for Deduction:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddReason" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblComments" runat="server" Text="Comments:" />
                </td>
                <td>
                    <asp:TextBox ID="txtComments" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbComments" runat="server" TargetControlID="txtComments"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, %. " />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblClosedDateFrom" runat="server" Text="Closed Date From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtClosedDateFrom" runat="server" MaxLength="12" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbeClosedDateFrom" runat="server" TargetControlID="txtClosedDateFrom"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgClosedDateFrom" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeClosedDateFrom" runat="server" TargetControlID="txtClosedDateFrom"
                        PopupButtonID="imgClosedDateFrom" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revClosedDateFrom" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtClosedDateFrom" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsList"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvClosedDateFrom" runat="server" ErrorMessage="Closed Date From must be less than Closed Date To."
                        ControlToCompare="txtClosedDateTo" ControlToValidate="txtClosedDateFrom" Operator="LessThanEqual"
                        Type="Date" ValidationGroup="vsList"><</asp:CompareValidator>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblClosedDateTo" runat="server" Text="Closed Date To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtClosedDateTo" runat="server" MaxLength="12" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbeClosedDateTo" runat="server" TargetControlID="txtClosedDateTo"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgClosedDateTo" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeClosedDateTo" runat="server" TargetControlID="txtClosedDateTo"
                        PopupButtonID="imgClosedDateTo" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revClosedDateTo" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtClosedDateTo" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsList"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvClosedDateTo" runat="server" ControlToCompare="txtClosedDateFrom"
                        ControlToValidate="txtClosedDateTo" ErrorMessage="Closed Date To must be greater than Closed Date From."
                        Operator="GreaterThanEqual" Type="Date" ValidationGroup="vsList"><</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblPartNo" runat="server" Text="Part Number:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" runat="server" MaxLength="40" Width="150px" />
                    <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                        ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-% "
                        Enabled="True" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblSortBy" runat="server" Text="Sort By:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddSortBy" runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="RecStatus">Record Status</asp:ListItem>
                        <asp:ListItem Value="ARDID">RecNo</asp:ListItem>
                        <asp:ListItem>Reason</asp:ListItem>
                        <asp:ListItem Value="UGNFacility">UGN Location</asp:ListItem>
                        <asp:ListItem Value="ReferenceNo">Reference No</asp:ListItem>
                        <asp:ListItem Value="DeductionAmount">Deduction Amount</asp:ListItem>
                        <asp:ListItem Value="DateSubmitted">Date Sent</asp:ListItem>
                        <asp:ListItem Value="UpdatedOn">Closed Date</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CommandName="search" CausesValidation="true"
                        ValidationGroup="vsList" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CommandName="reset" CausesValidation="False" />
                    <%-- <asp:Button ID="btnReport" runat="server" Text="Create Report" CommandName="reset"
                        CausesValidation="true" ValidationGroup="vsList" />
                    <asp:Button ID="btnCM" runat="server" CausesValidation="true" 
                        CommandName="reset" Text="Counter Measure Report" ValidationGroup="vsList" />--%>
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="summaryList" runat="server" ValidationGroup="vsList" ShowMessageBox="true" />
        <hr />
        <i>Use the parameters above to filter the list below.</i>
        <table width="500px" border="0">
            <tr>
                <td width="80px" align="center" style="white-space: nowrap;">
                    <asp:Label ID="lblClosed" runat="server" Text="Closed" />
                </td>
                <td width="80px" align="center" style="background-color: Fuchsia; white-space: nowrap;">
                    <asp:Label ID="lblNewRecord" runat="server" Text="New Record" />
                </td>
                <td width="80px" align="center" style="background-color: yellow; white-space: nowrap;">
                    <asp:Label ID="lblInProcess" runat="server" Text="In Process" />
                </td>
                <td width="100px" align="center" style="background-color: lime; white-space: nowrap;">
                    <asp:Label ID="lblReviewCompleted" runat="server" Text="Review Completed" />
                </td>
                <%-- <td width="80px" align="center" style="background-color: red; color: white; white-space: nowrap;">
                    Rejected
                </td>--%>
                <td width="80px" align="center" style="background-color: gray; color: white; white-space: nowrap;">
                    Void
                </td>
            </tr>
        </table>
        <table id="TABLE1" width="100%">
            <tbody>
                <tr>
                    <td class="c_text" style="font-style: italic" colspan="2">
                        <asp:Label ID="lblRecListed" runat="server" Text="Records Listed: " />
                        <asp:Label ID="lblFromRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblTo" runat="server" Text=" to " />
                        <asp:Label ID="lblToRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblOf" runat="server" Text=" of " />
                        <asp:Label ID="lblTotalRecords" runat="server" ForeColor="Red" />
                    </td>
                    <td align="right" colspan="9">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgTEP" ErrorMessage="Numeric Value Required." SetFocusOnError="True"
                            ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgTEP" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="11">
                        <asp:Repeater ID="rpARDeduction" runat="server">
                            <SeparatorTemplate>
                                <tr>
                                    <td colspan="11">
                                        <hr style="height: 0.01em" />
                                    </td>
                                </tr>
                            </SeparatorTemplate>
                            <HeaderTemplate>
                                <tr>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="lnkRecStatus" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="RecStatusDesc" Text="Record Status" />
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:LinkButton ID="lnkRecNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="ARDID" Text="Rec No." />
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="lnkRsnDeduct" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="ddReasonDesc" Text="Reason for Deduction" />
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="llnkUGNFac" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="UGNFacility" Text="UGN Location" />
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="lnkRefNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="ReferenceNo" Text="Reference No." />
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:LinkButton ID="lnkDeductAmt" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="DeductionAmount" Text="Deduction Amount" />
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:LinkButton ID="lnkDateSub" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="DateSubmitted" Text="Date Sent" />
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:LinkButton ID="lnkDaysOld" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="DaysOld" Text="Days Old" />
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:LinkButton ID="lnkClosedDate" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="UpdatedOn" Text="Closed Date" />
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="lnkVolume" runat="server">Preview</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label7" runat="server">History</asp:Label>
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="background-color: <%# SetBackGroundColor(Container.DataItem("RoutingStatus")).ToString%>;">
                                        <asp:HyperLink ID="getData" Font-Underline="true" runat="server" NavigateUrl='<%# "AR_Deduction.aspx?pARDID=" & DataBinder.Eval (Container.DataItem,"ARDID").tostring %>'
                                            ForeColor='<%# SetTextColor(DataBinder.Eval(Container, "DataItem.RoutingStatus")) %>'>
                         <%#DataBinder.Eval(Container, "DataItem.RecStatusDesc")%>        
                                        </asp:HyperLink>
                                    </td>
                                    <td align="center">
                                        <%#DataBinder.Eval(Container, "DataItem.ARDID")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ddReasonDesc")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.UGNFacilityName")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ReferenceNo")%>
                                    </td>
                                    <td align="right">
                                        $
                                        <%#Format(DataBinder.Eval(Container, "DataItem.DeductionAmount"), "#,##0.00")%>
                                    </td>
                                    <td align="center">
                                        <%#DataBinder.Eval(Container, "DataItem.DateSubmitted")%>
                                    </td>
                                    <td align="center">
                                        <%#DataBinder.Eval(Container, "DataItem.DaysOld")%>
                                    </td>
                                    <td align="center">
                                        <%#DataBinder.Eval(Container, "DataItem.ClosedDate")%>
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="lnkPreview" runat="server" NavigateUrl='<%# "crViewARDeduction.aspx?pARDID=" & DataBinder.Eval (Container.DataItem,"ARDID").tostring %>'
                                            Target="_blank" ImageUrl="~/images/PreviewUp.jpg"></asp:HyperLink>
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="lnkHistory" runat="server" NavigateUrl='<%# "AR_Deduction_History.aspx?pARDID=" & DataBinder.Eval (Container.DataItem,"ARDID").tostring  %>'
                                            ImageUrl="~/images/PreviewUp.jpg" Visible='<%# ShowHideHistory(DataBinder.Eval(Container, "DataItem.RecStatus")) %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="11">
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
