<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crAR_Event_Approval.aspx.vb" MaintainScrollPositionOnPostback="true"
    Inherits="crAR_Event_Approval" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table width="60%">
            <tr>
                <td class="c_text" colspan="2">
                    <b>Review the information for AR Event ID:
                        <asp:Label runat="server" ID="lblAREID" SkinID="MessageLabelSkin"></asp:Label>
                        and submit your response in the section provided.</b>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    <asp:Label ID="lblApprovalRole" runat="server" Text="Role" CssClass="c_textbold"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddApprrovalRole" runat="server" Enabled="false" Visible="false"
                        AutoPostBack="true">
                        <asp:ListItem Value="0" Text="CHOOSE A ROLE TO APPROVE AS"></asp:ListItem>
                        <asp:ListItem Value="21" Text="Accounting Manager"></asp:ListItem>
                        <asp:ListItem Value="9" Text="Sales/Account Manager"></asp:ListItem>
                        <asp:ListItem Value="23" Text="VP of Sales"></asp:ListItem>
                        <asp:ListItem Value="33" Text="CFO/VP of Finance"></asp:ListItem>
                        <asp:ListItem Value="24" Text="CEO"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    <asp:Label ID="lblEventStatus" runat="server" Text="Event Status:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddEventStatus" runat="server" Enabled="false">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Team Member:
                </td>
                <td>
                    <asp:Label ID="lblTeamMbr" runat="server" Text="" CssClass="c_textbold" Style="color: #990000;"></asp:Label>
                    <asp:HiddenField ID="hfSeqNo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="vertical-align: top">
                    <asp:Label ID="lblApprovalCommentMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    Comment:
                </td>
                <td>
                    <asp:TextBox ID="txtApprovalComment" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                        Width="350px" Enabled="false"></asp:TextBox><br />
                    <asp:RequiredFieldValidator runat="server" ID="rfvApprovalComment" EnableClientScript="true"
                        Enabled="false" ControlToValidate="txtApprovalComment" ErrorMessage="Comment is needed for rejection"
                        ValidationGroup="vgSave" SetFocusOnError="True" Text="<">
                    </asp:RequiredFieldValidator>
                    <asp:Label ID="lblApprovalCommentCharCount" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="vertical-align: top">
                    Notification Date:
                </td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblNotificationDate" runat="server" CssClass="c_textbold" Style="color: #990000;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">                    
                    <asp:Label runat="server" ID="lblPriceChangeDate" Visible="false" SkinID="MessageLabelSkin"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Approval Status:
                </td>
                <td style="vertical-align: top">
                    <asp:DropDownList ID="ddApprovalStatus" runat="server" AutoPostBack="true" Enabled="false">
                    </asp:DropDownList>
                    <asp:RangeValidator ID="rvApprovalStatus" runat="server" ValidationGroup="vgSave"
                        ErrorMessage="Approval Status must be set to approve or reject only" ControlToValidate="ddApprovalStatus"
                        MaximumValue="4" MinimumValue="3" SetFocusOnError="True" Text="<">
                    </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnStatusSubmit" runat="server" Text="Submit" ValidationGroup="vgSave"
                        CausesValidation="true" Visible="false" />
                    <asp:Button ID="btnStatusReset" runat="server" Text="Reset" />&nbsp;
                </td>
            </tr>
        </table>
        <br />
        <asp:Label runat="server" ID="lblMessageBottom" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <ajax:Accordion ID="accCommunicationBoard" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Width="98%">
            <Panes>
                <ajax:AccordionPane ID="apCommunicationBoard" runat="server">
                    <Header>
                        <a href="" class="accordionLink">COMMUNICATION BOARD: Submit your question/comments
                            prior to approval in this section.</a>
                    </Header>
                    <Content>
                        <asp:ValidationSummary runat="server" ID="vsCommunicationBoard" DisplayMode="List"
                            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgCommunicationBoard" />
                        <table>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblRSSComment" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                                    Question / Comments:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRSSComment" runat="server" Width="550px" TextMode="MultiLine"
                                        Rows="3" />
                                    <br />
                                    <asp:Label ID="lblRSSCommentCharCount" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                                    <asp:RequiredFieldValidator ID="rfvRSSComment" runat="server" ControlToValidate="txtRSSComment"
                                        ErrorMessage="Question / Comment is a required field." ValidationGroup="vgCommunicationBoard"><</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnRSSSubmit" runat="server" Text="Submit" CausesValidation="true"
                                        OnClientClick="return confirm('Are you sure you want to submit the question? It can NOT be changed later.');"
                                        ValidationGroup="vgCommunicationBoard" />
                                    <asp:Button ID="btnRSSReset" runat="server" Text="Reset" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="vsReply" ValidationGroup="CommBoard" runat="server" ShowMessageBox="True" />
                        <br />
                        <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataSourceID="odsQuestion"
                            Width="900px" RowStyle-BorderStyle="None" HorizontalAlign="center">
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" BorderColor="white" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/messanger30.jpg" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reply"></asp:TemplateField>
                                <asp:BoundField DataField="RSSID" HeaderText="RSSID" SortExpression="RSSID" Visible="false" />
                                <asp:BoundField DataField="Comment" HeaderText="Question / Comment" SortExpression="Comment"
                                    HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true"
                                    ItemStyle-CssClass="c_text" />
                                <asp:BoundField DataField="ddTeamMemberName" HeaderText="Submitted By" SortExpression="ddTeamMemberName"
                                    HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-Font-Bold="true" />
                                <asp:BoundField DataField="PostDate" HeaderText="Post Date" SortExpression="PostDate"
                                    ItemStyle-Font-Bold="true" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <asp:GridView ID="gvReply" runat="server" AutoGenerateColumns="False" DataSourceID="odsReply"
                                                    DataKeyNames="AREID,RSSID" Width="100%">
                                                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="20px" />
                                                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="red" HorizontalAlign="Left" />
                                                    <EditRowStyle BackColor="#E2DED6" />
                                                    <EmptyDataRowStyle Wrap="False" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Comment" HeaderText="Response" SortExpression="Comment"
                                                            HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true" />
                                                        <asp:BoundField DataField="ddTeamMemberName" HeaderText="" SortExpression="ddTeamMemberName"
                                                            HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                                                        <asp:BoundField DataField="PostDate" HeaderText="" SortExpression="PostDate" />
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                                    SelectMethod="GetARRSSReply" TypeName="ARRSSReplyBLL">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                                                        <asp:Parameter Name="RSSID" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsQuestion" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetARRSS" TypeName="ARRSSBLL">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                                <asp:Parameter Name="RSSID" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
    </asp:Panel>
    <br />
    <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
        ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="false"
        BackColor="White" Width="1000px" Height="1350px" EnableDatabaseLogonPrompt="False"
        HasCrystalLogo="False" HasPageNavigationButtons="True" DisplayGroupTree="False"
        HasSearchButton="False" HasToggleGroupTreeButton="False" PageZoomFactor="125"
        HyperlinkTarget="_blank" HasDrillUpButton="False" />
    <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="AR\Forms\crAREvent.rpt">
        </Report>
    </CrystalRpt:CrystalReportSource>
</asp:Content>
