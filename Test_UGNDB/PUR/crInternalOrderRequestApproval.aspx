<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crInternalOrderRequestApproval.aspx.vb" Inherits="IOR_crInternalOrderRequestApproval"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px">
        <table width="80%">
            <tr>
                <td class="c_text" colspan="4">
                    <b>Review the information for <font color="red">
                        <%=ViewState("pIORNo")%>
                    </font>and submit your response in the section provided.</b>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 17px">
                    <asp:Label ID="txtTeamMember" runat="server" Text="Team Member:" />
                </td>
                <td style="height: 17px">
                    <asp:Label ID="lblTeamMbr" runat="server" Text="" CssClass="c_text" Style="width: 243px;
                        color: #990000;" />
                    <asp:HiddenField ID="hfSeqNo" runat="server" />
                </td>
                <td class="p_text" rowspan="3" style="vertical-align: top">
                    <asp:Label ID="ReqComments" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                        Visible="false" />
                    <asp:Label ID="lblComments" runat="server" Text="Comments:" />
                </td>
                <td rowspan="3">
                    <asp:TextBox ID="txtComments" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                        Width="350px" /><br />
                    <asp:Label ID="lblCommentsChar" runat="server" Font-Bold="True" ForeColor="Red" CssClass="c_text" />
                </td>
            </tr>
            <tr>
                <td class="p_text" style="vertical-align: top">
                    <asp:Label ID="lblNotifiedDate" runat="server" Text="Date Notified:" />
                </td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblDateNotified" runat="server" Text="" CssClass="c_text" Style="width: 243px;
                        color: #990000;" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblStatus" runat="server" Text="Status:" />
                </td>
                <td style="vertical-align: top">
                    <asp:DropDownList ID="ddStatus" runat="server">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                        <asp:ListItem>Rejected</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <% If ViewState("ObjectRole") = True Then%>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReqPONo" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                        Visible="false" />
                    <asp:Label ID="lblPONo" runat="server" Text="Purchase Order #:" Visible="false" />
                </td>
                <td>
                    <asp:TextBox ID="txtPONo" runat="server" Visible="False" MaxLength="10" />
                    <ajax:FilteredTextBoxExtender ID="ftPONo" runat="server" TargetControlID="txtPONo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%" />
                    &nbsp;<asp:RequiredFieldValidator ID="rfvPONo" runat="server" ControlToValidate="txtPONo"
                        ErrorMessage="Purchase Order # is a required field." ValidationGroup="vsDetail"
                        Enabled="false"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <% End If%>
            <tr>
                <td style="height: 28px">
                </td>
                <td colspan="3" style="height: 28px">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return confirm('Are you sure you want to submit your response?');"
                        CausesValidation="true" ValidationGroup="vsDetail" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="sDetail" runat="server" ValidationGroup="vsDetail" ShowMessageBox="True" />
        <br />
        <br />
        <% If ViewState("COTRole") = True Then%>
        <asp:Panel ID="COTPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
            <asp:Image ID="imgCOT" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;
            <asp:Label ID="lblCOT" runat="server" Text="REVISION LEVEL(S) FROM CUSTOMER OWNED TOOLING:"
                CssClass="c_textbold" />
        </asp:Panel>
        <asp:Panel ID="COTContentPanel" runat="server" CssClass="collapsePanel" Width="800px">
            <asp:GridView ID="gvExpProjToolingCustomer" runat="server" SkinID="StandardGridWOFooter"
                AutoGenerateColumns="False" DataKeyNames="ProjectNo,PartNo,RevisionLevel" DataSourceID="odsExpProjToolingCustomer"
                Width="330px">
                <Columns>
                    <asp:BoundField DataField="ProjectNo" HeaderText="ProjectNo" ReadOnly="True" SortExpression="ProjectNo"
                        Visible="False" />
                    <asp:BoundField DataField="PartNo" HeaderText="Part Number" SortExpression="PartNo"
                        ReadOnly="true" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="RevisionLevel" HeaderText="Revision Level" SortExpression="RevisionLevel"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" AlternateText="Update"
                                CausesValidation="True" CommandName="Update" ImageUrl="~/images/save.jpg" ValidationGroup="EditSampleTrialEventInfo" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="Cancel" CausesValidation="False"
                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" ValidationGroup="EditInfo" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                                CommandName="Edit" ImageUrl="~/images/edit.jpg" />
                            &nbsp;&nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsExpProjToolingCustomer" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetExpProjToolingCustomerEIOR" TypeName="ExpProjToolingBLL" UpdateMethod="UpdateExpProjToolingCustomerEIOR">
                <UpdateParameters>
                    <asp:Parameter Name="RevisionLevel" Type="String" />
                    <asp:Parameter Name="original_ProjectNo" Type="String" />
                    <asp:Parameter Name="original_PartNo" Type="String" />
                    <asp:Parameter Name="original_RevisionLevel" Type="String" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </asp:Panel>
        <ajax:CollapsiblePanelExtender ID="COTExtender" runat="server" TargetControlID="COTContentPanel"
            ExpandControlID="COTPanel" CollapseControlID="COTPanel" Collapsed="FALSE" TextLabelID="lblCOT"
            ExpandedText="REVISION LEVEL(S) FROM CUSTOMER OWNED TOOLING:" CollapsedText="REVISION LEVEL(S) FROM CUSTOMER OWNED TOOLING:"
            ImageControlID="imgCOT" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true">
        </ajax:CollapsiblePanelExtender>
        <br />
        <% End If%>
        <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="800px"
            CssClass="c_text" Font-Bold="True" ForeColor="Red"></asp:Label>
        <br />
        <ajax:Accordion ID="accCommBoard" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Width="900px" Height="30%">
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
                                    <asp:Label ID="lblQC" runat="server" Text="Question / Comments:" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQC" runat="server" Width="550px" TextMode="MultiLine" Rows="3" />
                                    <asp:RequiredFieldValidator ID="rfvQC" runat="server" ControlToValidate="txtQC" ErrorMessage="Question / Comments is a required field."
                                        ValidationGroup="CommBoard"><</asp:RequiredFieldValidator><br />
                                    <asp:Label ID="lblQCChar" runat="server" Font-Bold="True" ForeColor="Red" CssClass="c_text" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSubmit2" runat="server" Text="Submit" CausesValidation="true"
                                        ValidationGroup="CommBoard" />
                                    <asp:Button ID="btnReset2" runat="server" Text="Reset" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="vsReply" ValidationGroup="CommBoard" runat="server" ShowMessageBox="True" />
                        <br />
                        <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataSourceID="odsQuestion"
                            OnRowDataBound="gvQuestion_RowDataBound" Width="900px" RowStyle-BorderStyle="None"
                            HorizontalAlign="center" CssClass="c_text" DataKeyNames="IORNo,RSSID">
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" BorderColor="white" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/messanger30.jpg" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RSSID" HeaderText="RSSID" SortExpression="RSSID" Visible="false" />
                                <asp:BoundField DataField="Comments" HeaderText="Question / Comment" SortExpression="Comments"
                                    HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true"
                                    ItemStyle-CssClass="c_text" />
                                <asp:BoundField DataField="TeamMemberName" HeaderText="Submitted By" SortExpression="TeamMemberName"
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
                                                    DataKeyNames="IORNo,RSSID" Width="100%" CssClass="c_text">
                                                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="20px" />
                                                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="red" HorizontalAlign="Left" />
                                                    <EditRowStyle BackColor="#E2DED6" />
                                                    <EmptyDataRowStyle Wrap="False" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Comments" HeaderText="Response" SortExpression="Comments"
                                                            HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true" />
                                                        <asp:BoundField DataField="TeamMemberName" HeaderText="" SortExpression="TeamMemberName"
                                                            HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                                                        <asp:BoundField DataField="PostDate" HeaderText="" SortExpression="PostDate" />
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                                    SelectMethod="GetInternalOrderRequestRSSReply" TypeName="InternalOrderRequestBLL">
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter Name="IORNo" QueryStringField="pIORNo" Type="String" />
                                                        <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsQuestion" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetInternalOrderRequestRSS" TypeName="InternalOrderRequestBLL">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="IORNo" QueryStringField="pIORNo" Type="String" />
                                <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <br />
        <ajax:Accordion ID="accSupportingDocument" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="True" SuppressHeaderPostbacks="true" Width="900px" Height="30%">
            <Panes>
                <ajax:AccordionPane ID="apSupportingDocument" runat="server">
                    <Header>
                        <asp:Panel ID="SDPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
                            <asp:Image ID="imgSD" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                                Height="12px" />&nbsp;<asp:Label ID="lblSD" runat="server" Text="Label" CssClass="c_textbold">SUPPORTING DOCUMENT(S): Expand this view to see attachments.</asp:Label></asp:Panel>
                        <asp:Panel ID="SDContentPanel" runat="server" CssClass="collapsePanel" />
                        <ajax:CollapsiblePanelExtender ID="SDExtender" runat="server" TargetControlID="SDContentPanel"
                            ExpandControlID="SDPanel" CollapseControlID="SDPanel" Collapsed="FALSE" TextLabelID="lblSD"
                            ExpandedText="SUPPORTING DOCUMENT(S): Expand this section to preview attachments."
                            CollapsedText="SUPPORTING DOCUMENT(S): " ImageControlID="imgSD" CollapsedImage="~/images/collapse_blue.jpg"
                            ExpandedImage="~/images/expand_blue.jpg" SuppressPostBack="true">
                        </ajax:CollapsiblePanelExtender>
                    </Header>
                    <Content>
                        <asp:GridView ID="gvSupportingDocument" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="IORNO,DocID" DataSourceID="odsSupportingDocument" Width="800px"
                            AllowSorting="True" CssClass="c_text">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#E2DED6" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EmptyDataRowStyle Wrap="False" />
                            <Columns>
                                <asp:TemplateField HeaderText="Preview" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                            NavigateUrl='<%# "InternalOrderRequestDocument.aspx?pIORNo=" & DataBinder.Eval (Container.DataItem,"IORNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                            Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Document" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Description" HeaderText="File Description for IOR" SortExpression="Description">
                                    <HeaderStyle HorizontalAlign="Left" Width="700px" />
                                    <ItemStyle Width="700px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsSupportingDocument" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetInternalOrderRequestDocuments" TypeName="InternalOrderRequestBLL">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="IORNo" QueryStringField="pIORNo" Type="String" />
                                <asp:Parameter Name="DocID" Type="Int32" DefaultValue="0" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <br />
                        <asp:HyperLink ID="hplkAppropriation" runat="server" Visible="false" ForeColor="Blue"
                            Font-Underline="true" Target="_blank" CssClass="c_text" Font-Bold="true" />
                        <asp:GridView ID="gvExpProjDocuments" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="ProjectNo" DataSourceID="odsExpProjDocuments" Width="800px" AllowSorting="True"
                            CssClass="c_text">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#E2DED6" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EmptyDataRowStyle Wrap="False" />
                            <Columns>
                                <asp:TemplateField HeaderText="Preview" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                            NavigateUrl='<%# GoToCapEx(DataBinder.Eval(Container.DataItem,"ProjectNo"),DataBinder.Eval (Container.DataItem,"DocID"))%>'
                                            Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Document" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Description" HeaderText="File Description for Appropriation"
                                    SortExpression="Description">
                                    <HeaderStyle HorizontalAlign="Left" Width="700px" />
                                    <ItemStyle Width="700px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsExpProjDocuments" runat="server" SelectMethod="GetExpProjDocuments"
                            TypeName="ExpProjDocumentsBLL">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <br />
        <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
            ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="false"
            BackColor="White" Width="1000px" Height="1350px" EnableDatabaseLogonPrompt="False"
            HasCrystalLogo="False" HasPageNavigationButtons="True" DisplayGroupTree="False"
            HasSearchButton="False" HasToggleGroupTreeButton="False" PageZoomFactor="125"
            HyperlinkTarget="_blank" HasDrillUpButton="False" PrintMode="ActiveX" />
        <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="PUR\Forms\crInternalOrderRequest.rpt">
            </Report>
        </CrystalRpt:CrystalReportSource>
    </asp:Panel>
</asp:Content>
