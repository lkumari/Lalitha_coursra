<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crSampleMtrlReqApproval.aspx.vb" Inherits="PGM_crSampleMtrlReqApproval"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px">
        <table>
            <tr>
                <td class="c_text" colspan="4">
                    <b>Review the information for Request #<font color="red">
                        <%=ViewState("pSMRNo")%>
                    </font>and submit your response in the section provided.</b><asp:TextBox ID="txtProjNo"
                        runat="server" Width="1px" Visible="false" /><asp:TextBox ID="txtProjectTitle" runat="server"
                            Width="1px" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblTeamMember" runat="server" Text="Team Member:" />
                </td>
                <td>
                    <asp:Label ID="lblTeamMbr" runat="server" Text="" CssClass="c_text" Style="width: 243px;
                        color: #990000;" />
                    <asp:HiddenField ID="hfSeqNo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblDateNotif" runat="server" Text="Date Notified:" />
                </td>
                <td>
                    <asp:Label ID="lblDateNotified" runat="server" Text="" CssClass="c_text" Style="width: 243px;
                        color: #990000;" />
                </td>
            </tr>
            <% If ViewState("iSETMID") = 0 Then%>
            <% If ViewState("iMMTNID") = 0 Then%>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReqShipEDICoord" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " />
                    <asp:Label ID="lblShipEDICoord" runat="server" Text="Assign Request to Shipping / EDI Coordinator:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddShipEDICoord" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvShipEDICoord" runat="server" ControlToValidate="ddShipEDICoord"
                        Display="Dynamic" ErrorMessage="Shipping/EDI Coordinator is a required field."
                        Font-Bold="True" Font-Size="Small" ValidationGroup="vsShipEdi"> <</asp:RequiredFieldValidator>
                    <asp:TextBox ID="hfShipEDICoordEmail" runat="server" Visible="false" Width="1px" />
                    <asp:TextBox ID="hfShipEdiCoordName" runat="server" Visible="false" Width="1px" />
                </td>
            </tr>
            <% End If%>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblStatus" runat="server" Text="Status:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddStatus" runat="server">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                        <asp:ListItem>Rejected</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="vertical-align: top">
                    <asp:Label ID="ReqComments" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                        Visible="false" />
                    <asp:Label ID="lblComments" runat="server" Text="Comments:" />
                </td>
                <td>
                    <asp:TextBox ID="txtComments" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                        Width="350px" /><br />
                    <asp:Label ID="lblCommentsChar" runat="server" Font-Bold="True" ForeColor="Red" CssClass="c_text" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return confirm('Are you sure you want to submit your response?');"
                        CausesValidation="true" ValidationGroup="vsDetail" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
            <% End If%>
        </table>
        <asp:ValidationSummary ID="sDetail" runat="server" ValidationGroup="vsDetail" ShowMessageBox="True" />
        <%-- For Shipping/EDI Coordinator use only --%>
       <% If ViewState("ObjectRole") = True Then%>
        <br />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblShipDocFileDesc" runat="server" Text="Shipping Document File Description:" />
                </td>
                <td class="c_text">
                    <asp:TextBox ID="txtFileDesc6" runat="server" MaxLength="200" Width="400px" />
                    <asp:RequiredFieldValidator ID="rfvShipDocFileDesc" runat="server" ControlToValidate="txtFileDesc6"
                        ErrorMessage="Shipping Document File Description is a required field" Font-Bold="False"
                        ValidationGroup="vsShipDocs"><</asp:RequiredFieldValidator><br />
                    <asp:Label ID="lblFileDescChar6" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td class="p_text" valign="top">
                    <asp:Label ID="Label25" runat="server" Text="Attach Shipping Document:" />
                </td>
                <td class="c_text">
                    <asp:FileUpload ID="uploadFileShipDoc" runat="server" Height="22px" Width="400px" />
                    <asp:RequiredFieldValidator ID="rfvUploadShipDoc" runat="server" ControlToValidate="uploadFileShipDoc"
                        ErrorMessage="Shipping Document is required." Font-Bold="False" ValidationGroup="vsShipDocs"><</asp:RequiredFieldValidator><br />
                    <asp:RegularExpressionValidator ID="revUploadFileShipDoc" runat="server" ErrorMessage="Only *.PDF, *.DOC, *.DOCX, *.XLS, *.XLSX, *.SNP, *.TIF, *.JPG files are allowed!"
                        ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.xlsx|.docx|.snp|.tif|.jpg|.PDF|.XLS|.DOC|.XLSX|.DOCX|.SNP|.TIF|.JPG)$"
                        ControlToValidate="uploadFileShipDoc" ValidationGroup="vsShipDocs" Font-Bold="True"
                        Font-Size="Small" />
                </td>
            </tr>
            <tr>
                <td style="height: 27px">
                </td>
                <td style="height: 27px">
                    <asp:Button ID="btnUploadShipDoc" runat="server" Text="Upload" CausesValidation="true"
                        ValidationGroup="vsShipDocs" />
                    <asp:Button ID="btnResetShipDoc" runat="server" CausesValidation="False" Text="Reset" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Label ID="lblMessageView6" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                        Visible="False" Width="368px" Font-Size="Small" />
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gvShipDocs" runat="server" AutoGenerateColumns="False" DataKeyNames="SMRNo,DocID"
            DataSourceID="odsShipDocs" Width="600px" SkinID="StandardGridWOFooter">
            <Columns>
                <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description"
                    ItemStyle-Width="500px" HeaderStyle-Width="500px" HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" Width="500px" />
                    <ItemStyle Width="500px" />
                </asp:BoundField>
                <asp:BoundField DataField="TeamMemberName" HeaderText="Uploaded By" SortExpression="TeamMemberName"
                    HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                    <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="DateOfUpload" HeaderText="Upload Date" SortExpression="DateOfUpload"
                    HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                            NavigateUrl='<%# "SampleMtrlReqDocument.aspx?pSMRNo=" & DataBinder.Eval (Container.DataItem,"SMRNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                            Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview" />
                    </ItemTemplate>
                    <HeaderStyle Width="30px" />
                    <ItemStyle HorizontalAlign="center" Width="30px" />
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/images/delete.jpg" AlternateText="Delete" /></ItemTemplate>
                    <HeaderStyle Width="30px" />
                    <ItemStyle HorizontalAlign="Right" Width="30px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsShipDocs" runat="server" DeleteMethod="DeleteSampleMtrlReqDocuments"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetSampleMtrlReqDocuments"
            TypeName="PGMBLL">
            <DeleteParameters>
                <asp:Parameter Name="SMRNo" Type="Int32" />
                <asp:Parameter Name="DocID" Type="Int32" />
                <asp:Parameter Name="Section" Type="String" />
                <asp:Parameter Name="original_SMRNo" Type="Int32" />
                <asp:Parameter Name="original_DocID" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="0" Name="SMRNo" QueryStringField="pSMRNo"
                    Type="Int32" />
                <asp:Parameter DefaultValue="0" Name="DocID" Type="Int32" />
                <asp:Parameter DefaultValue="S" Name="Section" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <asp:GridView ID="gvShipping" runat="server" SkinID="StandardGrid" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" DataSourceID="odsShipping" PageSize="30"
                        DataKeyNames="SMRNo,RowID" ShowFooter="True" Width="600px">
                        <Columns>
                            <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True" SortExpression="RowID"
                                Visible="False" />
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="* Shipper Number"
                                SortExpression="ShipperNo">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtShipperNoEdit" runat="server" MaxLength="15" Text='<%# Bind("ShipperNo") %>'
                                        Width="200px" />
                                    <asp:RequiredFieldValidator ID="rfveShipping" runat="server" ControlToValidate="txtShipperNoEdit"
                                        Display="Dynamic" ErrorMessage="Shipper Number is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgEdit2"> <</asp:RequiredFieldValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbShipperNoEdit" runat="server" TargetControlID="txtShipperNoEdit"
                                        FilterType="Numbers" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblShipping" runat="server" Text='<%# Bind("ShipperNo") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtShipperNo" runat="server" MaxLength="15" Width="200px" />
                                    <asp:RequiredFieldValidator ID="rfvShipperNo" runat="server" ControlToValidate="txtShipperNo"
                                        Display="Dynamic" ErrorMessage="Shipper Number is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgInsert2"> <</asp:RequiredFieldValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbShipperNo" runat="server" TargetControlID="txtShipperNo"
                                        FilterType="Numbers" />
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="* Total Shipping Cost (USD)" SortExpression="TotalShippingCost">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTotalShippingCostEdit" runat="server" MaxLength="16" Text='<%# Bind("TotalShippingCost") %>'
                                        Width="100px" />
                                    <ajax:FilteredTextBoxExtender ID="ftTotalShippingCostEdit" runat="server" FilterType="Custom, Numbers"
                                        TargetControlID="txtTotalShippingCostEdit" ValidChars="-,." />
                                    <asp:RequiredFieldValidator ID="rfveTotalShippingCost" runat="server" ControlToValidate="txtTotalShippingCostEdit"
                                        Display="Dynamic" ErrorMessage="Total Shipping Cost is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgEdit2"> < </asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalShippingCost" runat="server" Text='<%# Bind("TotalShippingCost","{0:c}") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTotalShippingCost" runat="server" MaxLength="16" Width="100px" />
                                    <ajax:FilteredTextBoxExtender ID="ftTotalShippingCost" runat="server" FilterType="Custom, Numbers"
                                        TargetControlID="txtTotalShippingCost" ValidChars="-,." />
                                    <asp:RequiredFieldValidator ID="rfvTotalShippingCostEdit" runat="server" ControlToValidate="txtTotalShippingCost"
                                        Display="Dynamic" ErrorMessage="Total Shipping Cost is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgInsert2"> &lt; </asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="* Freight Bill ProNo"
                                SortExpression="FreightBillProNo">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtFreightBillProNoEdit" runat="server" MaxLength="25" Text='<%# Bind("FreightBillProNo") %>'
                                        Width="150px" />
                                    <asp:RequiredFieldValidator ID="rfveFreightBillProNo" runat="server" ControlToValidate="txtFreightBillProNoEdit"
                                        Display="Dynamic" ErrorMessage="Freight Bill ProNo is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgEdit2"> &lt; </asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFreightBillProNo" runat="server" Text='<%# Bind("FreightBillProNo") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFreightBillProNo" runat="server" MaxLength="25" Width="150px" />
                                    <asp:RequiredFieldValidator ID="rfvFreightBillProNo" runat="server" ControlToValidate="txtFreightBillProNo"
                                        Display="Dynamic" ErrorMessage="Freight Bill ProNo is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgInsert2"> &lt; </asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:ImageButton ID="ibtnUpdate" runat="server" AlternateText="Save" CausesValidation="True"
                                        CommandName="Update" ImageUrl="~/images/save.jpg" ValidationGroup="vgEdit2" />&nbsp;<asp:ImageButton
                                            ID="ibtnCancel" runat="server" AlternateText="Cancel" CausesValidation="False"
                                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" ValidationGroup="vgEdit2" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                                        CommandName="Edit" ImageUrl="~/images/edit.jpg" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsert2"
                                        runat="server" ID="iBtnSaveCust" ImageUrl="~/images/save.jpg" AlternateText="Save" />&nbsp;<asp:ImageButton
                                            ID="iBtnUndoCust" runat="server" CommandName="Undo" CausesValidation="false"
                                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsShipping" runat="server" DeleteMethod="DeleteSampleMtrlReqShipping"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetSampleMtrlReqShipping"
                        TypeName="PGMBLL" InsertMethod="InsertSampleMtrlReqShipping" UpdateMethod="UpdateSampleMtrlReqShipping">
                        <DeleteParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="RowID" Type="Int32" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_RowID" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="ShipperNo" Type="Int32" />
                            <asp:Parameter Name="TotalShippingCost" Type="Decimal" />
                            <asp:Parameter Name="FreightBillProNo" Type="String" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_RowID" Type="Int32" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="SMRNo" QueryStringField="pSMRNo"
                                Type="Int32" />
                            <asp:Parameter Name="RowID" Type="Int32" DefaultValue="0" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="ShipperNo" Type="Int32" />
                            <asp:Parameter Name="TotalShippingCost" Type="Decimal" />
                            <asp:Parameter Name="FreightBillProNo" Type="String" />
                        </InsertParameters>
                    </asp:ObjectDataSource>
                    <br />
                    <table>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="lblShippingComments" runat="server" Text="Comments:" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtShippingComments" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                    Width="500px" /><br />
                                <asp:Label ID="lblShippingCommentsChar" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnSubmitCmplt" runat="server" Text="Submit Completion" CausesValidation="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <% End If%>
        <asp:ValidationSummary ID="vsShipDocs" runat="server" ValidationGroup="vsShipDocs"
            ShowMessageBox="true" ShowSummary="true" />
        &nbsp;&nbsp;<asp:ValidationSummary runat="server" ID="vsEdit2" ValidationGroup="vgEdit2"
            ShowMessageBox="true" ShowSummary="true" />
        &nbsp;&nbsp;<asp:ValidationSummary runat="server" ID="vsInsert2" ValidationGroup="vgInsert2"
            ShowMessageBox="true" ShowSummary="true" />
        &nbsp;&nbsp;<asp:ValidationSummary runat="server" ID="vsShipEdi" ValidationGroup="vsShipEdi"
            ShowMessageBox="true" ShowSummary="true" />
        <br />
        <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="800px"
            CssClass="c_text" Font-Bold="True" ForeColor="Red" />
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
                                    Question / Comments:
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
                                        ValidationGroup="CommBoard" /><asp:Button ID="btnReset2" runat="server" Text="Reset"
                                            CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="vsReply" ValidationGroup="CommBoard" runat="server" ShowMessageBox="True" />
                        <br />
                        <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataSourceID="odsQuestion"
                            Width="800px" RowStyle-BorderStyle="None" HorizontalAlign="Center" CssClass="c_text"
                            DataKeyNames="SMRNo,RSSID" SkinID="CommBoardRSS">
                            <RowStyle BorderStyle="None" />
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
                                                    DataKeyNames="SMRNo,RSSID" Width="100%" CssClass="c_text" SkinID="CommBoardResponse">
                                                    <Columns>
                                                        <asp:BoundField DataField="Comments" HeaderText="Response" SortExpression="Comments"
                                                            HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true" />
                                                        <asp:BoundField DataField="TeamMemberName" HeaderText="" SortExpression="TeamMemberName"
                                                            HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                                                        <asp:BoundField DataField="PostDate" HeaderText="" SortExpression="PostDate" />
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                                    SelectMethod="GetSampleMtrlReqRSSReply" TypeName="PGMBLL">
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter Name="SMRNo" QueryStringField="pSMRNo" Type="String" DefaultValue="0" />
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
                            SelectMethod="GetSampleMtrlReqRSS" TypeName="PGMBLL">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="SMRNo" QueryStringField="pSMRNo" Type="String" DefaultValue="0" />
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
            RequireOpenedPane="true" SuppressHeaderPostbacks="true" Width="900px" Height="30%">
            <Panes>
                <ajax:AccordionPane ID="apSupportingDocument" runat="server">
                    <Header>
                        <asp:Panel ID="SDPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
                            <asp:Image ID="imgSD" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                                Height="12px" />&nbsp;<asp:Label ID="lblSD" runat="server" Text="Label" CssClass="c_textbold">SUPPORTING DOCUMENT(S): Expand this view to see attachments.</asp:Label></asp:Panel>
                        <asp:Panel ID="SDContentPanel" runat="server" CssClass="collapsePanel" />
                        <ajax:CollapsiblePanelExtender ID="SDExtender" runat="server" TargetControlID="SDContentPanel"
                            ExpandControlID="SDPanel" CollapseControlID="SDPanel" Collapsed="false" TextLabelID="lblSD"
                            ExpandedText="SUPPORTING DOCUMENT(S): Expand this section to preview attachments."
                            CollapsedText="SUPPORTING DOCUMENT(S): " ImageControlID="imgSD" CollapsedImage="~/images/collapse_blue.jpg"
                            ExpandedImage="~/images/expand_blue.jpg" SuppressPostBack="true">
                        </ajax:CollapsiblePanelExtender>
                    </Header>
                    <Content>
                        <asp:GridView ID="gvSupportingDocument" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="SMRNo,DocID" DataSourceID="odsSupportingDocument" Width="800px"
                            RowStyle-BorderStyle="None" HorizontalAlign="Center" AllowSorting="True" CssClass="c_text"
                            SkinID="StandardGridWOFooter">
                            <Columns>
                                <asp:BoundField DataField="SectionDesc" HeaderText="Section" SortExpression="SectionDesc"
                                    HeaderStyle-Width="200px" />
                                <asp:TemplateField HeaderText="Preview" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                            NavigateUrl='<%# "SampleMtrlReqDocument.aspx?pSMRNo=" & DataBinder.Eval (Container.DataItem,"SMRNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                            Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Document" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description">
                                    <HeaderStyle HorizontalAlign="Left" Width="700px" />
                                    <ItemStyle Width="700px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsSupportingDocument" runat="server" TypeName="PGMBLL"
                            OldValuesParameterFormatString="original_{0}" SelectMethod="GetSampleMtrlReqDocuments">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="SMRNo" QueryStringField="pSMRNo"
                                    Type="Int32" />
                                <asp:Parameter DefaultValue="0" Name="DocID" Type="Int32" />
                                <asp:Parameter DefaultValue="" Name="Section" Type="String" />
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
            <Report FileName="PUR\Forms\crSampleMtrlReq.rpt">
            </Report>
        </CrystalRpt:CrystalReportSource>
    </asp:Panel>
</asp:Content>
