<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Absence_Calendar.aspx.vb" Inherits="Workflow_Absence_Calendar" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Text="Label" Font-Bold="True" ForeColor="Red"
        Visible="False"></asp:Label>
    <hr />
    <table>
        <tr>
            <td style="width: 200px" valign="top">
                <asp:Calendar ID="Calendar1" runat="server" Font-Bold="False" NextPrevFormat="ShortMonth"
                    Font-Size="Small" Height="200px" ShowGridLines="True" Width="256px">
                    <TitleStyle Font-Bold="True" BorderStyle="Dashed" BackColor="#E0E0E0" BorderColor="Gray"
                        BorderWidth="2px" />
                    <SelectedDayStyle BackColor="#E0E0E0" ForeColor="Black" />
                    <TodayDayStyle Font-Bold="True" ForeColor="#C00000" />
                    <OtherMonthDayStyle ForeColor="DarkGray" />
                    <NextPrevStyle Font-Bold="True" Font-Size="Smaller" ForeColor="Blue" />
                </asp:Calendar>
            </td>
            <td width="1px">
            </td>
            <td style="height: 105px" valign="top">
                <table>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                            <asp:Label ID="lblTeamMember" runat="server" Text="Team Member:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddTeamMember" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                                ErrorMessage="Team Member is a required field."><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                            <asp:Label ID="lblEvent" runat="server" Text="Event:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtEvent" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEvent" runat="server" ControlToValidate="txtEvent"
                                ErrorMessage="Event is a required field."><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                            <asp:Label ID="lblStartDate" runat="server" Text="Start Date:" />
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
                                Operator="LessThan" Type="Date"><</asp:CompareValidator>
                        </td>
                    </tr>
                    <%  If HttpContext.Current.Request.QueryString("sCID") = "" And HttpContext.Current.Request.QueryString("sCID") = Nothing Then%>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblEndDate" runat="server" Text="End Date:" />
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
                            <asp:CheckBox ID="chkBackup" runat="server" />
                        </td>
                        <td class="p_text">
                            <asp:Label ID="lblChkBox" runat="server" Text="Check this box to redirect your UGNDB responsibilities to Backup Person(s) when
                            not at UGN." />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" />&nbsp;<asp:Button ID="btnReset"
                                runat="server" Text="Reset" CausesValidation="False" />&nbsp;<asp:Button ID="btnDelete"
                                    runat="server" Text="Delete" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" />
            </td>
        </tr>
    </table>
    <hr />
    <table width="100%">
        <tr>
            <td valign="top">
                <asp:Label ID="lblWeekDay1" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay1" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="odsTMCalendar"
                    SkinID="StandardGrid" ShowFooter="false" Width="195px" DataKeyNames="CID" PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="TeamMemberEvent" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString="Absence_Calendar.aspx?sCID={0}" />
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:TemplateField HeaderText="Backup" SortExpression="AlertBackup">
                            <EditItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("AlertBackup") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("AlertBackup") %>'
                                    Enabled="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events available.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsTMCalendar" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetTeamMemberCalendar" TypeName="Team_Member_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date1" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td style="width: 1px; background-color: gray">
            </td>
            <td valign="top">
                <asp:Label ID="lblWeekDay2" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay2" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataSourceID="odsTMCalendar2"
                    SkinID="StandardGrid" ShowFooter="false" Width="195px" PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="TeamMemberEvent" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString="Absence_Calendar.aspx?sCID={0}" />
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:TemplateField HeaderText="Backup" SortExpression="AlertBackup">
                            <EditItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("AlertBackup") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("AlertBackup") %>'
                                    Enabled="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events available.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsTMCalendar2" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetTeamMemberCalendar" TypeName="Team_Member_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date2" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td style="width: 1px; background-color: gray">
            </td>
            <td valign="top">
                <asp:Label ID="lblWeekDay3" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay3" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" DataSourceID="odsTMCalendar3"
                    SkinID="StandardGrid" ShowFooter="false" Width="195px" DataKeyNames="CID" PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="TeamMemberEvent" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString="Absence_Calendar.aspx?sCID={0}" />
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:TemplateField HeaderText="Backup" SortExpression="AlertBackup">
                            <EditItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("AlertBackup") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("AlertBackup") %>'
                                    Enabled="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events available.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsTMCalendar3" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetTeamMemberCalendar" TypeName="Team_Member_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date3" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td style="width: 1px; background-color: gray">
            </td>
            <td valign="top">
                <asp:Label ID="lblWeekDay4" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay4" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" DataSourceID="odsTMCalendar4"
                    SkinID="StandardGrid" ShowFooter="false" Width="195px" DataKeyNames="CID" PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="TeamMemberEvent" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString="Absence_Calendar.aspx?sCID={0}" />
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:CheckBoxField DataField="AlertBackup" HeaderText="Backup" SortExpression="AlertBackup">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events available.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsTMCalendar4" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetTeamMemberCalendar" TypeName="Team_Member_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date4" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td style="width: 1px; background-color: gray">
            </td>
            <td valign="top">
                <asp:Label ID="lblWeekDay5" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000" />
                <asp:Label ID="lblDay5" runat="server" Text="Label" CssClass="c_text" Font-Bold="True"
                    Font-Size="Small" ForeColor="#C00000"></asp:Label>
                <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="False" DataSourceID="odsTMCalendar5"
                    SkinID="StandardGrid" ShowFooter="false" Width="195px" DataKeyNames="CID" PageSize="100">
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="CID" SortExpression="CID" Visible="False" />
                        <asp:HyperLinkField HeaderText="Event" DataTextField="TeamMemberEvent" DataNavigateUrlFields="CID"
                            DataNavigateUrlFormatString="Absence_Calendar.aspx?sCID={0}" />
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate"
                            Visible="False" />
                        <asp:CheckBoxField DataField="AlertBackup" HeaderText="Backup" SortExpression="AlertBackup">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                    </Columns>
                    <EmptyDataTemplate>
                        No events available.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsTMCalendar5" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetTeamMemberCalendar" TypeName="Team_Member_CalendarBLL">
                    <SelectParameters>
                        <asp:SessionParameter Name="StartDate" SessionField="Date5" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
