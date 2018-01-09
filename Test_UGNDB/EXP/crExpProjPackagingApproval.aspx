<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crExpProjPackagingApproval.aspx.vb" Inherits="EXP_crViewExpProjPackaging"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <table width="60%">
            <tr>
                <td class="c_text" colspan="4">
                    <b>Review the information for <font color="red">
                        <%=ViewState("pProjNo")%>
                    </font>and submit your response in the section provided.</b>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 17px">
                    Team Member:
                </td>
                <td style="height: 17px">
                    <asp:Label ID="lblTeamMbr" runat="server" Text="" CssClass="c_text" Style="width: 243px;
                        color: #990000;"></asp:Label>
                    <asp:HiddenField ID="hfSeqNo" runat="server" />
                </td>
                <td class="p_text" rowspan="3" style="vertical-align: top">
                    <asp:Label ID="ReqComments" runat="server" Font-Bold="True" ForeColor="Red" Text="*"
                        Visible="false" />
                    Comments:
                </td>
                <td rowspan="3">
                    &nbsp;<asp:TextBox ID="txtComments" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                        Width="350px"></asp:TextBox><br />
                    <asp:Label ID="lblComments" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_text" style="vertical-align: top">
                    Date Notified:
                </td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblDateNotified" runat="server" Text="" CssClass="c_text" Style="width: 243px;
                        color: #990000;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Status:
                </td>
                <td style="vertical-align: top">
                    <asp:DropDownList ID="ddStatus" runat="server">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                        <asp:ListItem>Rejected</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="height: 28px">
                </td>
                <td colspan="3" style="height: 28px">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return confirm('Are you sure you want to submit your response?');" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />&nbsp;
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="800px"
            CssClass="c_text" Font-Bold="True" ForeColor="Red"></asp:Label>
        <br />
        <ajax:Accordion ID="accCommBoard" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Width="1150px" Height="30%">
            <Panes>
                <ajax:AccordionPane ID="apCommBoard" runat="server">
                    <Header>
                        <asp:Panel ID="CBPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
                            <asp:Image ID="imgCB" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                                Height="12px" />&nbsp;<asp:Label ID="lblCB" runat="server" Text="Label" CssClass="c_textbold">COMMUNICATION BOARD: Submit your question/comments prior to approval in this
                            section.</asp:Label></asp:Panel>
                        <asp:Panel ID="CBContentPanel" runat="server" CssClass="collapsePanel">
                        </asp:Panel>
                        <ajax:CollapsiblePanelExtender ID="CBExtender" runat="server" TargetControlID="CBContentPanel"
                            ExpandControlID="CBPanel" CollapseControlID="CBPanel" Collapsed="FALSE" TextLabelID="lblCB"
                            ExpandedText="COMMUNICATION BOARD: Submit your question/comments prior to approval in this
                            section." CollapsedText="COMMUNICATION BOARD: Submit your question/comments prior to approval in this
                            section." ImageControlID="imgCB" CollapsedImage="~/images/collapse_blue.jpg"
                            ExpandedImage="~/images/expand_blue.jpg" SuppressPostBack="true">
                        </ajax:CollapsiblePanelExtender>
                    </Header>
                    <Content>
                        <table>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                    Question / Comments:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQC" runat="server" Width="550px" TextMode="MultiLine" Rows="3" />
                                    <asp:RequiredFieldValidator ID="rfvQC" runat="server" ControlToValidate="txtQC" ErrorMessage="Question / Comments is a required field."
                                        ValidationGroup="CommBoard"><</asp:RequiredFieldValidator><br />
                                    <asp:Label ID="lblQC" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSubmit2" runat="server" Text="Submit" CausesValidation="true"
                                        ValidationGroup="CommBoard" /><asp:Button ID="btnReset2" runat="server" Text="Reset"
                                            CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="vsReply" ValidationGroup="CommBoard" runat="server" ShowMessageBox="True" />
                        <br />
                        <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataKeyNames="ProjectNo,RSSID"
                            DataSourceID="odsQuestion" OnRowDataBound="gvQuestion_RowDataBound" Width="900px"
                            RowStyle-BorderStyle="None" HorizontalAlign="Center">
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" BorderColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/messanger30.jpg" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RSSID" HeaderText="RSSID" SortExpression="RSSID" Visible="False" />
                                <asp:BoundField DataField="Comments" HeaderText="Question / Comment" SortExpression="Comments">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle CssClass="c_text" Font-Bold="True" Width="500px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TeamMemberName" HeaderText="Submitted By" SortExpression="TeamMemberName">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Font-Bold="True" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PostDate" HeaderText="Post Date" SortExpression="PostDate">
                                    <ItemStyle Font-Bold="True" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <asp:GridView ID="gvReply" runat="server" AutoGenerateColumns="False" DataSourceID="odsReply"
                                                    DataKeyNames="ProjectNo,RSSID" Width="100%" SkinID="CommBoardResponse">
                                                    <Columns>
                                                        <asp:BoundField DataField="Comments" HeaderText="Response" SortExpression="Comments">
                                                            <HeaderStyle Width="500px" />
                                                            <ItemStyle Font-Bold="True" Width="500px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TeamMemberName" SortExpression="TeamMemberName">
                                                            <HeaderStyle Width="100px" />
                                                            <ItemStyle Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PostDate" SortExpression="PostDate" />
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                                    SelectMethod="GetExpProjPackagingRSSReply" TypeName="ExpProjPackagingBLL">
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                                                        <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle BorderStyle="None" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsQuestion" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetExpProjPackagingRSS" TypeName="ExpProjPackagingBLL">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                                <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <br />
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accSupportingDocument" runat="server" AutoSize="None" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" Height="30%" RequireOpenedPane="true"
            SelectedIndex="-1" SuppressHeaderPostbacks="true" TransitionDuration="250" Width="1150px">
            <Panes>
                <ajax:AccordionPane ID="apSupportingDocument" runat="server">
                    <Header>
                        <asp:Panel ID="SDPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
                            <asp:Image ID="imgSD" runat="server" AlternateText="expand" Height="12px" ImageUrl="~/images/expand_blue.jpg" />
                            &nbsp;<asp:Label ID="lblSD" runat="server" CssClass="c_textbold" Text="Label">SUPPORTING 
                            DOCUMENT(S): Expand this view to see attachments.</asp:Label></asp:Panel>
                        <asp:Panel ID="SDContentPanel" runat="server" CssClass="collapsePanel" />
                        <ajax:CollapsiblePanelExtender ID="SDExtender" runat="server" CollapseControlID="SDPanel"
                            Collapsed="FALSE" CollapsedImage="~/images/collapse_blue.jpg" CollapsedText="SUPPORTING DOCUMENT(S): "
                            ExpandControlID="SDPanel" ExpandedImage="~/images/expand_blue.jpg" ExpandedText="SUPPORTING DOCUMENT(S): Expand this section to preview attachments."
                            ImageControlID="imgSD" SuppressPostBack="true" TargetControlID="SDContentPanel"
                            TextLabelID="lblSD">
                        </ajax:CollapsiblePanelExtender>
                    </Header>
                    <Content>
                        <asp:GridView ID="gvSupportingDocument" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                            DataKeyNames="ProjectNo,DocID" DataSourceID="odsSupportingDocument" Width="800px">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#E2DED6" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EmptyDataRowStyle Wrap="False" />
                            <EmptyDataTemplate>
                                <label class="c_text" style="font-style: italic; color: red;">
                                    There are NO Documents to display.</label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Preview" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlnkPreview" runat="server" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                            NavigateUrl='<%# "PackagingExpProjDocument.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                            Target="_blank" ToolTip="Preview Document" Visible='<%# Bind("BinaryFound") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description">
                                    <HeaderStyle HorizontalAlign="Left" Width="700px" />
                                    <ItemStyle Width="700px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsSupportingDocument" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetExpProjPackagingDocuments" TypeName="ExpProjPackagingBLL">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                                <asp:Parameter Name="DocID" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <br />
        <br />
        <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
            ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="false"
            BackColor="White" Width="1000px" Height="1350px" EnableDatabaseLogonPrompt="False"
            HasCrystalLogo="False" HasPageNavigationButtons="True" DisplayGroupTree="False"
            HasSearchButton="False" HasToggleGroupTreeButton="False" PageZoomFactor="125"
            HyperlinkTarget="_blank" HasDrillUpButton="False" />
        <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="EXP\Forms\crExpProjPackaging.rpt">
            </Report>
        </CrystalRpt:CrystalReportSource>
    </asp:Panel>
</asp:Content>
