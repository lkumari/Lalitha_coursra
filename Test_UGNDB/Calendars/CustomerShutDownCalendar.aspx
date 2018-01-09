<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CustomerShutDownCalendar.aspx.vb" Inherits="CustomerShutDownCalendar"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Text="Label" Font-Bold="True" ForeColor="Red"
        Visible="False"></asp:Label>
    <br />
   
        <table width="100%" class="displayTable">
            <tr>
                <td style="width: 300px" valign="bottom" class="c_text" colspan="3">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/week.jpg" />
                    <asp:HyperLink ID="lnkWeek" runat="server" Text="Weekly View" NavigateUrl="~/Calendars/CustomerShutDownCalendar.aspx?sView=Week" />
                    &nbsp;&nbsp;
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/month.jpg" />
                    <asp:HyperLink ID="lnkMonth" runat="server" Text="Monthly View" NavigateUrl="~/Calendars/CustomerShutDownCalendar.aspx?sView=Month" />
                </td>
            </tr>
            <tr>
                <%  If HttpContext.Current.Request.QueryString("sView") <> "Month" Then%>
                <td style="width: 200px" valign="top">
                    <asp:Calendar ID="Calendar1" runat="server" Font-Bold="False" NextPrevFormat="ShortMonth"
                        Font-Size="Small" Height="200px" ShowGridLines="True" Width="256px" WeekendDayStyle-BackColor="gray">
                        <TitleStyle Font-Bold="True" BorderStyle="Dashed" BackColor="#E0E0E0" BorderColor="Gray"
                            BorderWidth="2px" />
                        <SelectedDayStyle BackColor="#E0E0E0" ForeColor="Black" />
                        <TodayDayStyle Font-Bold="True" ForeColor="#C00000" />
                        <OtherMonthDayStyle ForeColor="DarkGray" />
                        <NextPrevStyle Font-Bold="True" Font-Size="Smaller" ForeColor="Blue" />
                        <WeekendDayStyle BackColor="WhiteSmoke" BorderColor="Gainsboro" ForeColor="Silver" />
                    </asp:Calendar>
                </td>
                <%End If%>
                <td style="width: 10px">
                </td>
                <td style="height: 105px" valign="top">
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                UGN Facility:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddUGNFacility" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                                    ErrorMessage="UGN Facility is a required field."><</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                OEM:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddOEM" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvOEM" runat="server" ControlToValidate="ddOEM"
                                    ErrorMessage="OEM is a required field."><</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                Customer:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddCustomer" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddCustomer"
                                    ErrorMessage="Customer is a required field."><</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                Start Date:
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartDate" runat="server" MaxLength="12" Width="80px"></asp:TextBox>
                                <asp:ImageButton runat="server" ID="imgStartDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                    CausesValidation="False" />
                                <ajax:CalendarExtender ID="calendarButtonExtender" runat="server" TargetControlID="txtStartDate"
                                    PopupButtonID="imgStartDate" />
                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate"
                                    ErrorMessage="Start Date is a required field."><</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revStartDate" runat="server" ControlToValidate="txtStartDate"
                                    ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                    ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                    Width="8px"><</asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="cvStartDate" runat="server" ControlToCompare="txtEndDate"
                                    ControlToValidate="txtStartDate" ErrorMessage="Start Date must be less than End of Production."
                                    Operator="LessThan" Type="Date"><</asp:CompareValidator></td>
                        </tr>
                        <%  If HttpContext.Current.Request.QueryString("sCID") = "" And HttpContext.Current.Request.QueryString("sCID") = Nothing Then%>
                        <tr>
                            <td class="p_text">
                                End Date:
                            </td>
                            <td>
                                <asp:TextBox ID="txtEndDate" runat="server" MaxLength="12" Width="80px"></asp:TextBox>
                                <asp:ImageButton runat="server" ID="imgEndDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                    CausesValidation="False" />
                                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate"
                                    PopupButtonID="imgEndDate" />
                                <asp:RegularExpressionValidator ID="revEndDate" runat="server" ControlToValidate="txtEndDate"
                                    ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                    ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                    Width="8px"><</asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="cvEndDate" runat="server" ControlToCompare="txtStartDate"
                                    ControlToValidate="txtEndDate" ErrorMessage="End Date must be greater than Start of Production."
                                    Operator="GreaterThan" Type="Date"><</asp:CompareValidator>
                            </td>
                        </tr>
                        <% End If%>
                        <tr>
                            <td align="right">
                                <asp:CheckBox ID="chkWkEndWorkDay" runat="server" />
                            </td>
                            <td class="c_text">
                                Use this check box to indicate Weekend work day only.
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:CheckBox ID="chkHoliday" runat="server" AutoPostBack="true" />
                            </td>
                            <td class="c_text">
                                Use this check box to indicate Holiday shut down for "ALL" Customers.
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False"
                                    Visible="false" /></td>
                            <td>
                                &nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" />
                                &nbsp;<asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                                &nbsp;<asp:Button ID="btnDelete" runat="server" Text="Delete" CausesValidation="False"
                                    OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" />
                </td>
            </tr>
        </table>
        <hr />


    <br />
    <br />
    <!--  WEEKLY VIEW  -->
    <%  If HttpContext.Current.Request.QueryString("sView") = "Week" Then%>
    <table width="100%">
        <tr>
            <td valign="top">
                <asp:Label ID="lblWeekDay1" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay1" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="odsCalendar"
                    ShowHeader="False" SkinID="StandardGrid" Width="170px" DataKeyNames="CID" ShowFooter="false"
                    PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="DisplayInfo" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString='CustomerShutDownCalendar.aspx?sView=Week&amp;sCID={0}'>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:CheckBoxField DataField="WkEndWorkDay" SortExpression="WkEndWorkDay" Visible="False">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events scheduled.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCalendar" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetCustomerShutDownCalendar" TypeName="Customer_Shut_Down_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date1" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td style="width: 1px; background-color: #E0E0E0">
            </td>
            <td valign="top">
                <asp:Label ID="lblWeekDay2" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay2" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataSourceID="odsCalendar2"
                    ShowHeader="False" SkinID="StandardGrid" Width="170px" ShowFooter="false" PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="DisplayInfo" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString="CustomerShutDownCalendar.aspx?sView=Week&amp;sCID={0}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:CheckBoxField DataField="WkEndWorkDay" SortExpression="WkEndWorkDay" Visible="False">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events scheduled.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCalendar2" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetCustomerShutDownCalendar" TypeName="Customer_Shut_Down_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date2" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td style="width: 1px; background-color: #E0E0E0">
            </td>
            <td valign="top">
                <asp:Label ID="lblWeekDay3" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay3" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" DataSourceID="odsCalendar3"
                    ShowHeader="False" SkinID="StandardGrid" Width="170px" DataKeyNames="CID" ShowFooter="false"
                    PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="DisplayInfo" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString="CustomerShutDownCalendar.aspx?sView=Week&amp;sCID={0}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:CheckBoxField DataField="WkEndWorkDay" SortExpression="WkEndWorkDay" Visible="False">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events scheduled.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCalendar3" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetCustomerShutDownCalendar" TypeName="Customer_Shut_Down_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date3" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td style="width: 1px; background-color: #E0E0E0">
            </td>
            <td valign="top">
                <asp:Label ID="lblWeekDay4" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay4" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" DataSourceID="odsCalendar4"
                    ShowHeader="False" SkinID="StandardGrid" Width="170px" DataKeyNames="CID" ShowFooter="false"
                    PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="DisplayInfo" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString="CustomerShutDownCalendar.aspx?sView=Week&amp;sCID={0}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:CheckBoxField DataField="WkEndWorkDay" SortExpression="WkEndWorkDay" Visible="False">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events scheduled.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCalendar4" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetCustomerShutDownCalendar" TypeName="Customer_Shut_Down_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date4" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td style="width: 1px; background-color: #E0E0E0">
            </td>
            <td valign="top">
                <asp:Label ID="lblWeekDay5" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay5" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="False" DataSourceID="odsCalendar5"
                    ShowHeader="False" SkinID="StandardGrid" Width="170px" DataKeyNames="CID" ShowFooter="false"
                    PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="DisplayInfo" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString="CustomerShutDownCalendar.aspx?sView=Week&amp;sCID={0}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:CheckBoxField DataField="WkEndWorkDay" SortExpression="WkEndWorkDay" Visible="False">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events scheduled.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCalendar5" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetCustomerShutDownCalendar" TypeName="Customer_Shut_Down_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date5" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td style="width: 1px; background-color: #E0E0E0">
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <br />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblWeekDay6" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay6" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="False" DataSourceID="odsCalendar6"
                    ShowHeader="False" SkinID="StandardGrid" Width="170px" DataKeyNames="CID" ShowFooter="false"
                    PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="DisplayInfo" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString="CustomerShutDownCalendar.aspx?sView=Week&amp;sCID={0}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:CheckBoxField DataField="WkEndWorkDay" SortExpression="WkEndWorkDay" Visible="False">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events scheduled.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCalendar6" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetCustomerShutDownCalendar" TypeName="Customer_Shut_Down_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date6" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td style="width: 1px; background-color: #E0E0E0">
            </td>
            <td valign="top">
                <asp:Label ID="lblWeekDay7" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay7" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView7" runat="server" AutoGenerateColumns="False" DataSourceID="odsCalendar7"
                    ShowHeader="False" SkinID="StandardGrid" Width="170px" DataKeyNames="CID" ShowFooter="false"
                    PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="DisplayInfo" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString="CustomerShutDownCalendar.aspx?sView=Week&amp;sCID={0}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:CheckBoxField DataField="WkEndWorkDay" SortExpression="WkEndWorkDay" Visible="False">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events scheduled.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCalendar7" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetCustomerShutDownCalendar" TypeName="Customer_Shut_Down_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date7" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
    </table>
    <br />
    <% End If%>
    <!--  MONTHLY VIEW   ondayrender="Calendar1_DayRender" onselectionchanged="Date_Selected"-->
    <%  If HttpContext.Current.Request.QueryString("sView") = "Month" Then%>
    <asp:Calendar ID="Calendar2" runat="server" Font-Bold="False" Font-Size="small" NextPrevFormat="ShortMonth"
        ShowGridLines="true" BorderWidth="1" Font-Names="Verdana" Width="100%" DayStyle-VerticalAlign="Top"
        DayStyle-Height="50px" DayStyle-Width="14%" SelectedDayStyle-BackColor="Navy"
        OnDayRender="Calendar2_DayRender">
        <TitleStyle Font-Bold="True" BorderStyle="Dashed" BackColor="#E0E0E0" BorderColor="Gray"
            BorderWidth="2px" Height="25px" />
        <SelectedDayStyle BackColor="#E0E0E0" ForeColor="Black" />
        <TodayDayStyle Font-Bold="True" ForeColor="#C00000" BorderColor="#C00000" BorderStyle="Solid"
            BorderWidth="2px" />
        <OtherMonthDayStyle ForeColor="DarkGray" />
        <NextPrevStyle Font-Bold="True" Font-Size="Smaller" ForeColor="Blue" />
        <DayStyle Height="50px" VerticalAlign="Top" Width="14%" />
        <WeekendDayStyle BackColor="WhiteSmoke" BorderColor="Gainsboro" ForeColor="LightGray" />
    </asp:Calendar>
    <% End If%>
</asp:Content>
