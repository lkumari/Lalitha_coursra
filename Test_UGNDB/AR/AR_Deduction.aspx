<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AR_Deduction.aspx.vb" Inherits="AR_Deduction" Title="Untitled Page"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" CssClass="c_textbold" />
        <% If ViewState("pProjNo") <> Nothing Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="width: 532px; color: #990000">
                    <asp:Label ID="lblEdit" runat="server" Text="Edit data below or press " />
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="False" />
                    <asp:Label ID="lblToEnter" runat="server" Text=" to enter new data." />
                </td>
            </tr>
        </table>
        <%  End If%>
        <hr />
        <br />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblRecNo" runat="server" Text="Rec No:" />
                </td>
                <td style="color: #990000;">
                    <asp:Label ID="lblARDID" runat="server" Text="?" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Size="Larger" Font-Underline="False" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblDateSubmitted" runat="server" Text="Date Sent:" />
                </td>
                <td style="color: #990000;">
                    <asp:Label ID="txtDateSubmitted" Font-Size="Larger" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label16" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    <asp:Label ID="lblReasonDeduction" runat="server" Text="Reason for Deduction:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddReason" runat="server" AutoPostBack="true" />
                    <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="ddReason"
                        ErrorMessage="Reason is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                    <asp:TextBox ID="hdDefaultNotify" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblRecordStatus" runat="server" Text="Record Status:" />
                </td>
                <td class="c_textbold" style="color: red;" colspan="3">
                    <asp:DropDownList ID="ddRecStatus" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="Open">New Record</asp:ListItem>
                        <asp:ListItem>Closed</asp:ListItem>
                        <asp:ListItem Value="Approved">Review Completed</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddRecStatus2" runat="server" AutoPostBack="True">
                        <asp:ListItem>In Process</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                        <asp:ListItem>Closed @60 days</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lblRoutingStatusDesc" runat="server" Visible="False" Width="312px" />
                    <asp:TextBox ID="txtRoutingStatus" Visible="false" runat="server" Width="1px" />
                </td>
            </tr>
        </table>
        <table width="100%" border="0">
            <tr>
                <td colspan="3" style="width: 30px">
                    <asp:Menu ID="mnuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Detail" Value="0" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Supporting Documents" Value="1" ImageUrl="" />
                            <asp:MenuItem Text="Approval Status" Value="2" ImageUrl="" />
                            <asp:MenuItem Text="Communication Board" Value="3" ImageUrl="" />
                        </Items>
                    </asp:Menu>
                </td>
            </tr>
        </table>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwRequestInfoTab" runat="server">
                <table>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblSubmittedBy" runat="server" Text="Submitted By:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddSubmittedBy" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvSubmittedBy" runat="server" ControlToValidate="ddSubmittedBy"
                                ErrorMessage="Submitted By is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblSampleProdDesc" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            <asp:Label ID="lblUGNLocation" runat="server" Text="UGN Location:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddUGNFacility" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                                ErrorMessage="UGN Location is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label15" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblDeductionAmount" runat="server" Text="Deduction Amount ($):" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDeductionAmount" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbeDeductionAmount" runat="server" TargetControlID="txtDeductionAmount"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RequiredFieldValidator ID="rfvDeductionAmount" runat="server" ControlToValidate="txtDeductionAmount"
                                ErrorMessage="Deduction Amount ($) is a required field." ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvDeductionAmount" runat="server" ControlToValidate="txtDeductionAmount"
                                ErrorMessage="Deduction Amount must be greater than 0." MinimumValue="1" MaximumValue="999999"
                                ValidationGroup="vsDetail"><</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblCustomer" runat="server" Text="Customer:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCustomer" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddCustomer"
                                ErrorMessage="Customer is a required field." ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblReferenceNo" runat="server" Text="Reference No.:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferenceNo" runat="server" MaxLength="25" Width="150px" />
                            <ajax:FilteredTextBoxExtender ID="ftbReferenceNo" runat="server" TargetControlID="txtReferenceNo"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                            <asp:RequiredFieldValidator ID="rfvReferenceNo" runat="server" ControlToValidate="txtReferenceNo"
                                ErrorMessage="Refernce No. is a required field." ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblReqIncidentDt" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblIncidentDate" runat="server" Text="Incident Date:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtIncidentDate" runat="server" MaxLength="12" Width="80px" />
                            <asp:ImageButton runat="server" ID="imgIncidentDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <ajax:CalendarExtender ID="cbeIncidentDate" runat="server" TargetControlID="txtIncidentDate"
                                PopupButtonID="imgIncidentDate" Format="MM/dd/yyyy" />
                            <asp:RegularExpressionValidator ID="revIncidentDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtIncidentDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px" ValidationGroup="vsDetail"><</asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvIncidentDate" runat="server" ControlToValidate="txtIncidentDate"
                                ErrorMessage="Incident Date is a required field." ValidationGroup="vsDetail"
                                Enabled="false"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblPartNo" runat="server" Text="Part Number:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPartNo" runat="server" MaxLength="40" Width="150px" AutoPostBack="true" />
                            <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                                ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- "
                                Enabled="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblReqComments" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                                Visible="false" />
                            <asp:Label ID="lblComments" runat="server" Text="Comments:" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtComments" runat="server" MaxLength="2000" Rows="8" TextMode="MultiLine"
                                Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvComments" runat="server" Enabled="false" ControlToValidate="txtComments"
                                ErrorMessage="Comments is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblCommentsChar" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblReqCreditDebitDate" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            <asp:Label ID="lblCreditDebitDate" runat="server" Text="Credit/Debit Date:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreditDebitDate" runat="server" MaxLength="12" Width="80px" />
                            <asp:ImageButton runat="server" ID="imgCDD" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <ajax:CalendarExtender ID="ceCDD" runat="server" TargetControlID="txtCreditDebitDate"
                                PopupButtonID="imgCDD" Format="MM/dd/yyyy" />
                            <asp:RegularExpressionValidator ID="revCDD" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtCreditDebitDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px" ValidationGroup="vsDetail"><</asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvCDD" runat="server" ControlToValidate="txtCreditDebitDate"
                                ErrorMessage="Credit/Debit Date is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblReqCreditDebitMemo" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            <asp:Label ID="lblCreditDebitMemo" runat="server" Text="Credit/Debit Memo:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreditDebitMemo" runat="server" MaxLength="25" Width="100px" />
                            <ajax:FilteredTextBoxExtender ID="ftbCDM" runat="server" TargetControlID="txtCreditDebitMemo"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                            <asp:RequiredFieldValidator ID="rfvCDM" runat="server" ControlToValidate="txtCreditDebitMemo"
                                ErrorMessage="Credit/Debit Memo is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <%--Display the following rows after record is voided.--%>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblReqVoidRsn" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                                Visible="false" />
                            <asp:Label ID="lblVoidRsn" runat="server" Text="Void Reason:" />
                        </td>
                        <td class="c_text" colspan="3">
                            <asp:TextBox ID="txtVoidReason" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                Width="600px" /><asp:RequiredFieldValidator ID="rfvVoidReason" runat="server" ErrorMessage="Void Reason is a required field."
                                    ControlToValidate="txtVoidReason" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblVoidReasonChar" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                        </td>
                        <td colspan="3">
                            <asp:Button ID="btnSaveDetail" runat="server" Text="Save" CausesValidation="True"
                                ValidationGroup="vsDetail" />
                            <asp:Button ID="btnResetDetail" runat="server" Text="Reset" CausesValidation="False" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CausesValidation="False" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:ValidationSummary ID="sDetail" ValidationGroup="vsDetail" runat="server" ShowMessageBox="True" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwSupportingDoc" runat="server">
                <br />
                <asp:Panel ID="SDPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
                    <asp:Image ID="imgSD" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblSD" runat="server" Text="SUPPORTING DOCUMENT(S):" CssClass="c_textbold" />
                </asp:Panel>
                <asp:Panel ID="SDContentPanel" runat="server" CssClass="collapsePanel" Width="800px">
                    <asp:Label ID="lblSupDoc" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                        color: #990000" Text="This section is available as an option to include additional information. *.PDF, *.DOC, *.DOCX, *.XLS and *.XLSX files are allowed for upload up to 4MB each." /><br />
                    <asp:Label ID="lblSupDoc2" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                        color: #990000" Text="NOTE: Please be sure to upload the latest copy of any document. Any changes you make will not be saved to the upload files. Please be sure to make a copy of the file locally and upload a new version. You have the option to delete or keep previous version of the file for reference. Please use the 'File Description' area to comment on the changes you make." />
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblUploadBy" runat="server" Text="Upload By:" />
                            </td>
                            <td class="c_text">
                                <asp:DropDownList ID="ddTeamMember" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                                    ErrorMessage="Team Member is a required field." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="lblFileDescription" runat="server" Text="File Description:" />
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtFileDesc" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                                    Width="600px" />
                                <asp:RequiredFieldValidator ID="rfvFileDesc" runat="server" ControlToValidate="txtFileDesc"
                                    ErrorMessage="File Description is a required field." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblFileDescChar" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="lblSupportingDocument" runat="server" Text="Supporting Document:" />
                            </td>
                            <td class="c_text">
                                <asp:FileUpload ID="uploadFile" runat="server" Height="22px" Width="600px" />
                                <asp:RequiredFieldValidator ID="rfvUpload" runat="server" ControlToValidate="uploadFile"
                                    ErrorMessage="Supporting Document is required." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Only *.PDF, *.DOC, *.DOCX, *.XLS, *.XLSX files are allowed!"
                                    ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.xlsx|.docx|.PDF|.XLS|.DOC|.XLSX|.DOCX)$"
                                    ControlToValidate="uploadFile" ValidationGroup="vsSupportingDocuments" Font-Bold="True"
                                    Font-Size="Small" /><br />
                                <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Text="Label" Visible="False" Width="541px" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px">
                            </td>
                            <td style="height: 27px">
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="true" ValidationGroup="vsSupportingDocuments" />
                                <asp:Button ID="btnResetUpload" runat="server" CausesValidation="False" Text="Reset" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsSupDoc" runat="server" ValidationGroup="vsSupportingDocuments"
                        ShowMessageBox="true" ShowSummary="true" />
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="SDExtender" runat="server" TargetControlID="SDContentPanel"
                    ExpandControlID="SDPanel" CollapseControlID="SDPanel" Collapsed="FALSE" TextLabelID="lblSD"
                    ExpandedText="SUPPORTING DOCUMENT(S):" CollapsedText="SUPPORTING DOCUMENT(S):"
                    ImageControlID="imgSD" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
                <br />
                <asp:GridView ID="gvSupportingDocument" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="ARDID,DocID" DataSourceID="odsSupportingDocument" Width="900px"
                    AllowSorting="True">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#E2DED6" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EmptyDataRowStyle Wrap="False" />
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description">
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Width="400px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="comboUploadBy" HeaderText="Uploaded By" SortExpression="comboUploadBy">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                    NavigateUrl='<%# "AR_Deduction_Document.aspx?pARDID=" & DataBinder.Eval (Container.DataItem,"ARDID").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                    Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Test Report" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Right" Width="30px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsSupportingDocument" runat="server" DeleteMethod="DeleteARDeductionDocuments"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetARDeductionDocuments"
                    TypeName="ARDeductionBLL">
                    <DeleteParameters>
                        <asp:Parameter Name="Original_ARDID" Type="Int32" />
                        <asp:Parameter Name="Original_DocID" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ARDID" QueryStringField="pARDID" Type="Int32" />
                        <asp:Parameter Name="DocID" Type="Int32" />
                        <asp:Parameter Name="MaxDateOfUpload" Type="Boolean" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vwApprovalStatus" runat="server">
                <br />
                <asp:Label ID="lblReqAppComments" runat="server" Visible="false" CssClass="c_text"
                    Font-Bold="true" />
                <asp:GridView ID="gvApprovers" runat="server" AutoGenerateColumns="False" DataKeyNames="ARDID,SeqNo,OrigTeamMemberID,TeamMemberID"
                    OnRowUpdating="gvApprovers_RowUpdating" OnRowDataBound="gvApprovers_RowDataBound"
                    DataSourceID="odsApprovers" Width="1000px" RowStyle-Height="20px" RowStyle-CssClass="c_text"
                    HeaderStyle-CssClass="c_text" ShowFooter="True">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="c_text" Height="20px" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" CssClass="c_text" />
                    <EditRowStyle BackColor="#E2DED6" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EmptyDataRowStyle Wrap="False" />
                    <Columns>
                        <asp:TemplateField HeaderText="Approval Level" SortExpression="SeqNo">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SeqNo") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SeqNo") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblMsg1" runat="server" Text="1" Font-Italic="true" ForeColor="Black" />
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OrigTeamMemberName" HeaderText="Original Team Member"
                            SortExpression="OrigTeamMemberName" Visible="False">
                            <HeaderStyle HorizontalAlign="Left" Width="140px" Wrap="True" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Assigned Team Member" SortExpression="TeamMemberName">
                            <EditItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddResponsibleTM" runat="server" DataSource='<%# commonFunctions.GetTeamMemberbySubscription(92) %>'
                                    DataValueField="TMID" DataTextField="TMName" SelectedValue='<%# Bind("TMID") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Selected="True">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvResposibleTM" runat="server" ControlToValidate="ddResponsibleTM"
                                    ErrorMessage="Assigned Team Member is a required field." Font-Bold="True" ValidationGroup="InsertApprovalInfo"><</asp:RequiredFieldValidator>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px" Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Notified" SortExpression="DateNotified">
                            <EditItemTemplate>
                                <asp:Label ID="txtDateNotified" runat="server" Text='<%# Bind("DateNotified") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDateNotified" runat="server" Text='<%# Bind("DateNotified") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" SortExpression="Status">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddStatus" runat="server" SelectedValue='<%# Bind("ddStatus") %>'>
                                    <asp:ListItem>Pending</asp:ListItem>
                                    <asp:ListItem Value="Approved">Agree</asp:ListItem>
                                    <asp:ListItem Value="Rejected">Disagree</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="70px" />
                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="DateSigned" HeaderText="Date Signed" SortExpression="DateSigned"
                            ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Comments" SortExpression="Comments">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAppComments" runat="server" MaxLength="200" Rows="2" TextMode="MultiLine"
                                    Text='<%# Bind("Comments") %>' Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvComments" runat="server" ControlToValidate="txtAppComments"
                                    ErrorMessage="Comments is a required field when approving for another team member."
                                    Font-Bold="True" ValidationGroup="EditApprovalInfo"><</asp:RequiredFieldValidator><asp:TextBox
                                        ID="txtTeamMemberID" runat="server" Text='<%# Eval("TeamMemberID") %>' ReadOnly="true"
                                        Width="0px" Visible="false" /><asp:TextBox ID="hfSeqNo" runat="server" Text='<%# Eval("SeqNo") %>'
                                            ReadOnly="true" Width="0px" Visible="false" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Comments") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Wrap="True" />
                            <FooterTemplate>
                                <asp:Label ID="lblMsg2" runat="server" Text="<< Use this row to add another TM for approval, when required. >>"
                                    Font-Italic="true" ForeColor="Black" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    ImageUrl="~/images/save.jpg" ToolTip="Save" ValidationGroup="EditApprovalInfo" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" ValidationGroup="EditApprovalInfo" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    ToolTip="Edit" ImageUrl="~/images/edit.jpg" ValidationGroup="EditApprovalInfo" />&nbsp;&nbsp;&nbsp;
                            </ItemTemplate>
                            <ItemStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <FooterTemplate>
                                <asp:ImageButton ID="btnInsert" runat="server" CausesValidation="true" ValidationGroup="InsertApprovalInfo"
                                    CommandName="Insert" ToolTip="Insert" ImageUrl="~/images/save.jpg" />
                                <asp:ImageButton ID="ibtnUndo" runat="server" CausesValidation="False" CommandName="Undo"
                                    ImageUrl="~/images/undo-gray.jpg" ToolTip="Cancel" ValidationGroup="InsertApprovalInfo" />
                            </FooterTemplate>
                            <ItemStyle Width="60px" HorizontalAlign="Center" />
                            <FooterStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ToolTip="Delete" ImageUrl="~/images/delete.jpg" />
                            </ItemTemplate>
                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ValidationSummary ID="vsEditApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="EditApprovalInfo" />
                <asp:ValidationSummary ID="vsInsertApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="InsertApprovalInfo" />
                <asp:ObjectDataSource ID="odsApprovers" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetARDeductionApproval" TypeName="ARDeductionBLL" UpdateMethod="UpdateARDeductionApproval">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ARDID" QueryStringField="pARDID" Type="Int32" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Status" Type="String" />
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="SameTMID" Type="Boolean" />
                        <asp:Parameter Name="original_ARDID" Type="String" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                        <asp:Parameter Name="TeamMemberName" Type="String" />
                        <asp:Parameter Name="DateNotified" Type="String" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                        <asp:Parameter Name="ddStatus" Type="String" />
                    </UpdateParameters>
                </asp:ObjectDataSource>
                <br />
                <table>
                    <tr>
                        <td>
                        </td>
                        <td>
                            &nbsp;<asp:Button ID="btnBuildApproval" runat="server" CausesValidation="False" Text="Build Approval List" />
                            <asp:Button ID="btnFwdApproval" runat="server" Text="Submit" Width="130px" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwCommunicationBoard" runat="server">
                <table>
                    <tr>
                        <td class="p_text" style="vertical-align: top">
                            <asp:Label ID="lblCM" runat="server" Text="Counter Measure:" />
                        </td>
                        <td style="vertical-align: top">
                            <asp:TextBox ID="txtCM" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                Width="350px" ReadOnly="true" />
                        </td>
                        <td class="p_text" style="vertical-align: top">
                            <asp:Label ID="lblPostDate" Font-Bold="true" runat="server" Text="Post Date:" />
                        </td>
                        <td class="c_text" style="vertical-align: top">
                            <asp:Label ID="txtPostDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="vertical-align: top">
                            <asp:Label ID="lblResolution" runat="server" Text="Resolution:" />
                        </td>
                        <td style="vertical-align: top">
                            <asp:TextBox ID="txtResolution" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                Width="350px" ReadOnly="true" />
                        </td>
                        <td class="p_text" style="vertical-align: top">
                            <asp:Label ID="lblClosedDate" Font-Bold="true" runat="server" Text="Closed Date:" />
                        </td>
                        <td class="c_text" style="vertical-align: top">
                            <asp:Label ID="txtClosedDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnCloseCM" runat="server" Text="Close Counter Measure" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblSQC" runat="server" CssClass="p_smalltextbold" Style="width: 532px;
                    color: #990000" Text="Select a 'Question / Comment' from discussion thread below to respond." />
                <table>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblQuestionComment" runat="server" Text="Question / Comment:" />
                        </td>
                        <td class="c_text">
                            <asp:TextBox ID="txtQC" runat="server" Font-Bold="True" Rows="3" TextMode="MultiLine"
                                Width="550px" ReadOnly="true" />
                            <asp:RequiredFieldValidator ID="rfvQC" runat="server" ErrorMessage="Select a Question / Comment from table below for response."
                                ValidationGroup="ReplyComments" ControlToValidate="txtQC"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblReqReply" runat="server" Text="* " ForeColor="Red" />
                            <asp:Label ID="lblRC" runat="server" Text="Reply / Comments:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReply" runat="server" Rows="3" TextMode="MultiLine" Width="550px" />
                            <asp:RequiredFieldValidator ID="rfvReply" runat="server" ErrorMessage="Reply / Comments is a required field."
                                ValidationGroup="ReplyComments" ControlToValidate="txtReply"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblReplyChar" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px">
                        </td>
                        <td style="height: 26px">
                            <asp:Button ID="btnSaveCB" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="ReplyComments" />
                            <asp:Button ID="btnResetCB" runat="server" Text="Reset" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsReplyComments" runat="server" ValidationGroup="ReplyComments"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataSourceID="odsQuestion"
                    OnRowDataBound="gvQuestion_RowDataBound" Width="900px" RowStyle-BorderStyle="None">
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="20px" BorderStyle="None" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <Columns>
                        <asp:TemplateField HeaderText="Reply">
                            <ItemTemplate>
                                <%--  <% If ViewState("Admin") = "true" Then%>--%>
                                <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/messanger30.jpg"
                                    ToolTip="Reply" NavigateUrl='<%# GoToCommunicationBoard(DataBinder.Eval(Container, "DataItem.ARDID"),DataBinder.Eval(Container, "DataItem.RSSID"),DataBinder.Eval(Container, "DataItem.ApprovalLevel"),DataBinder.Eval(Container, "DataItem.TeamMemberID")) %>' />
                                <%-- <%Else%>
                                <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/images/messanger30.jpg" />
                                <% End If%>--%>
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
                                            DataKeyNames="ARDID,RSSID" Width="100%">
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
                                            SelectMethod="GetARDeductionRSSReply" TypeName="ARDeductionBLL">
                                            <SelectParameters>
                                                <asp:QueryStringParameter Name="ARDID" QueryStringField="pARDID" Type="String" />
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
                    SelectMethod="GetARDeductionRSS" TypeName="ARDeductionBLL">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ARDID" QueryStringField="pARDID" Type="String" />
                        <asp:Parameter Name="RSSID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Content>
