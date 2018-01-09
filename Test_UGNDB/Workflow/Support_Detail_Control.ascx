<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Support_Detail_Control.ascx.vb"
    Inherits="Support_Detail_Control" %>

<asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
<asp:ValidationSummary runat="server" ID="vsSave" ValidationGroup="vgSave" ShowMessageBox="true"
    ShowSummary="true" />
<table width="98%">
    <tr>
        <td class="p_text">
            Requestor ID:
        </td>
        <td class="c_text">
            <asp:Label runat="server" ID="lblJobNumber" Text="Unassigned" ForeColor="Red" Font-Size="Medium"></asp:Label>
            <asp:Label runat="server" ID="lblJnId" Text="0" CssClass="none"></asp:Label>
        </td>
        <td class="p_text">
            Status:
        </td>
        <td class="c_text">
            <asp:DropDownList runat="server" ID="ddStatus">
                <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                <asp:ListItem Text="In Process" Value="In Process"></asp:ListItem>
                <asp:ListItem Text="Hold" Value="Hold"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="p_text">
            Category:
        </td>
        <td class="c_text">
            <asp:DropDownList runat="server" ID="ddCategory">
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ID="rfvCategory" ControlToValidate="ddCategory"
                SetFocusOnError="true" ErrorMessage="Category is required" Text="<" ValidationGroup="vgSave" />
        </td>
        <td class="p_text">
            Issue related To:
        </td>
        <td class="c_text">
            <asp:DropDownList runat="server" ID="ddRelatedTo">
                <asp:ListItem Text="Helpdesk" Value="H"></asp:ListItem>
                <asp:ListItem Text="Programming" Value="P"></asp:ListItem>
                <asp:ListItem Text="Other" Value="O"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ID="rfvRelatedTo" ControlToValidate="ddRelatedTo"
                SetFocusOnError="true" ErrorMessage="Issue related to is required" Text="<" ValidationGroup="vgSave" />
        </td>
    </tr>
    <tr>
        <td class="p_text">
            Requested By:
        </td>
        <td class="c_text">
            <asp:DropDownList runat="server" ID="ddRequestBy" Enabled="false">
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ID="rfvRequestBy" ControlToValidate="ddRequestBy"
                SetFocusOnError="true" ErrorMessage="Request by team member is required" Text="<"
                ValidationGroup="vgSave" />
            &nbsp;
            <asp:Label runat="server" ID="lblRequestBy"></asp:Label>
        </td>
        <td class="p_text" valign="top">
            Request Date:
            <br />
            Completion Date:
        </td>
        <td class="c_text" valign="top">
            <asp:Label runat="server" ID="lblRequestDate"></asp:Label>
            <br />
            <asp:Label runat="server" ID="lblCompletionDate"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="p_text">
            UGN Database Module:
        </td>
        <td class="c_text" colspan="3">
            <asp:DropDownList runat="server" ID="ddModule">
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ID="rfvModule" ControlToValidate="ddModule"
                SetFocusOnError="true" ErrorMessage="Module is required" Text="<" ValidationGroup="vgSave" />
        </td>
    </tr>
    <tr>
        <td class="p_text">
            System Details:
        </td>
        <td class="c_text" colspan="3">
            <asp:Label runat="server" ID="lblSystemDetails"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="p_text">
            Description:
        </td>
        <td class="c_text" colspan="3">
            <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Width="500px" Height="100px"></asp:TextBox>
            <ajax:FilteredTextBoxExtender ID="ftbeDesc" runat="server" Enabled="True" TargetControlID="txtDesc"
                ValidChars="?!-*&| ().@åäöÅÄÖ,/'&quot;" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters">
            </ajax:FilteredTextBoxExtender>
            <asp:RequiredFieldValidator runat="server" ID="rfvDesc" ControlToValidate="txtDesc"
                SetFocusOnError="true" ErrorMessage="Description is required" Text="<" ValidationGroup="vgSave" />
            <br />
            <asp:Label ID="lblDescCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<table width="98%">
    <tr>
        <td colspan="4" align="left">
            <ajax:Accordion ID="accSupportingDoc" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
                RequireOpenedPane="false" SuppressHeaderPostbacks="true" Width="98%">
                <Panes>
                    <ajax:AccordionPane ID="apSupportingDocHeader" runat="server">
                        <Header>
                            <a href="" class="accordionLink"><u>Include Supporting Documents?</u></a></Header>
                        <Content>
                            <asp:ValidationSummary ID="vsSupportingDocs" runat="server" DisplayMode="List" ShowMessageBox="true"
                                ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSupportingDocs" />
                            <br />
                            <table runat="server" id="tblUpload">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="server" ID="lblUploadTitle" SkinID="StandardLabelSkin">Upload a Supporting Document</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" valign="top">
                                        <asp:CheckBox runat="server" ID="cbSignatureReq" Text="Check if signature is required" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" valign="top">
                                        File Description:
                                    </td>
                                    <td class="c_text">
                                        <asp:TextBox ID="txtSupportingDocDesc" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                                            Width="500px" />
                                        <ajax:FilteredTextBoxExtender ID="ftbeSupportingDocDesc" runat="server" Enabled="True"
                                            TargetControlID="txtSupportingDocDesc" ValidChars="*&| ().@åäöÅÄÖ," FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters">
                                        </ajax:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvSupportingDocDesc" runat="server" ControlToValidate="txtSupportingDocDesc"
                                            ErrorMessage="Supporting Document File Description is a required field." Font-Bold="False"
                                            ValidationGroup="vgSupportingDocs" SetFocusOnError="true" Text="<"></asp:RequiredFieldValidator><br />
                                        <br />
                                        <asp:Label ID="lblSupportingDocDescCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="white-space: nowrap;">
                                        <asp:Label runat="server" ID="lblFileUploadLabel" Text="Upload a supporting file under 3 MB:<br>(PDF,DOC,DOCX,XLS,JPEG,TIF,MSG,PPT)"
                                            CssClass="p_text"></asp:Label>
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <asp:FileUpload ID="fileUploadSupportingDoc" runat="server" Width="600px" />
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="true" ValidationGroup="vgSupportingDocs" />
                                        <asp:RequiredFieldValidator ID="rfvFileUploadSupportingDoc" runat="server" ControlToValidate="fileUploadSupportingDoc"
                                            ErrorMessage="PDF File is required." Font-Bold="False" ValidationGroup="vgUpload"><</asp:RequiredFieldValidator><br />
                                        <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Please upload only *.PDF, *.DOC,*.DOCX, *.XLS, *.JPEG, *.JPG, *.TIF files are allowed."
                                            ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.xlsx|.doc|.docx|.jpeg|.jpg|.tif|.msg|.ppt|.PDF|.XLS|.XLSX|.DOC|.DOCX|.JPEG|.JPG|.TIF|.MSG|.PPT)$"
                                            ControlToValidate="fileUploadSupportingDoc" ValidationGroup="vgSupportingDocs"
                                            Font-Bold="True" Font-Size="Small" />
                                    </td>
                                </tr>
                            </table>
                        </Content>
                    </ajax:AccordionPane>
                </Panes>
            </ajax:Accordion>
            <br />
            <asp:GridView runat="server" ID="gvSupportingDoc" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" DataSourceID="odsSupportingDoc" DataKeyNames="RowID"
                EmptyDataText="" Width="98%" SkinID="StandardGrid">
                <Columns>
                    <asp:BoundField DataField="RowID">
                        <ItemStyle CssClass="none" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Preview" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.SupportingDocEncodeType").tostring)  %>'
                                NavigateUrl='<%# "Supporting_Doc_Viewer.aspx?jnId=" & DataBinder.Eval (Container.DataItem,"jnId").tostring & "&RowID=" & DataBinder.Eval (Container.DataItem,"RowId").tostring %>'
                                Target="_blank" ToolTip="Preview Document" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibtnSupportingDocDelete" runat="server" CausesValidation="False"
                                CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Signature Required" SortExpression="isSignatureReq"
                        HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbViewSignatureReq" runat="server" Checked='<%# Bind("isSignatureReq") %>'
                                Enabled="false" />
                        </ItemTemplate>
                        <HeaderStyle Width="30px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Supporting Document Name" SortExpression="SupportingDocName">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkViewFormula" runat="server" NavigateUrl='<%# Eval("RowId", "Supporting_Doc_Viewer.aspx?jnID=" & ViewState("jnId") & "&RowID={0}") %>'
                                Target="_blank" Text='<%# Eval("SupportingDocName") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Description" DataField="SupportingDocDesc">
                        <ControlStyle Font-Size="X-Small" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Uploaded By" DataField="CreatedBy">
                        <ControlStyle Font-Size="X-Small" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsSupportingDoc" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetSupportingDoc" TypeName="SupportModule" DeleteMethod="DeleteSupportingDoc">
                <DeleteParameters>
                    <asp:ControlParameter ControlID="lblJnId" DefaultValue="0" Name="jnId" PropertyName="Text"
                        Type="Int32" />
                    <asp:Parameter Name="RowID" Type="Int32" />
                    <asp:Parameter Name="original_RowID" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblJnId" DefaultValue="0" Name="jnId" PropertyName="Text"
                        Type="Int32" />
                    <asp:Parameter Name="RowID" Type="Int32" DefaultValue="0" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </td>
    </tr>
    <tr>
        <td colspan="4" align="center">
            <asp:Button runat="server" ID="btnSubmit" Text="Submit" ValidationGroup="vgSave"
                CausesValidation="true" />
            <asp:Button runat="server" ID="btnPreviewTop" Text="Preview" CausesValidation="false" />
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <ajax:Accordion ID="accAdmin" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
                RequireOpenedPane="false" SuppressHeaderPostbacks="true" Width="98%" Visible="false">
                <Panes>
                    <ajax:AccordionPane ID="apAdmin" runat="server">
                        <Header>
                            <a href="" class="accordionLink">Business System Info</a></Header>
                        <Content>
                            <table width="98%">
                                <tr>
                                    <td colspan="4">
                                        <i>To be completed by the Business Systems Group</i>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Assigned To:
                                    </td>
                                    <td colspan="3" class="c_text">
                                        <asp:DropDownList runat="server" ID="ddAssignedTo">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="lblAssignedTo"></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="tblHoursRow">
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblEstimatedHours" Text="Estimated Hours:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtEstimatedHours" MaxLength="8"></asp:TextBox>
                                        <ajax:FilteredTextBoxExtender ID="ftActualCost" runat="server" TargetControlID="txtEstimatedHours"
                                            FilterType="Custom, Numbers" ValidChars="-.," />
                                    </td>
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblActualHours" Text="Actual Hours:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtActualHours" MaxLength="8"></asp:TextBox>
                                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtActualHours"
                                            FilterType="Custom, Numbers" ValidChars="-.," />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Comments:
                                    </td>
                                    <td colspan="3" class="c_text">
                                        <asp:TextBox runat="server" ID="txtComments" TextMode="MultiLine" Width="500px" Height="100px"></asp:TextBox>
                                        <ajax:FilteredTextBoxExtender ID="ftbeComments" runat="server" Enabled="True" TargetControlID="txtComments"
                                            ValidChars="?!-*&| ().@åäöÅÄÖ,/'&quot;" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters">
                                        </ajax:FilteredTextBoxExtender>
                                        <br />
                                        <asp:Label ID="lblCommentsCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:Button runat="server" ID="btnUpdate" Text="Update" ValidationGroup="vgSave"
                                            CausesValidation="true" />
                                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CausesValidation="false" />
                                        <asp:Button runat="server" ID="btnNotify" Text="Notify" ValidationGroup="vgSave"
                                            CausesValidation="true" />
                                        <asp:Button runat="server" ID="btnPreviewBottom" Text="Preview" CausesValidation="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left">
                                        <hr />
                                        <h2>
                                            Additional Approval Routing</h2>
                                        <br />
                                        <br />
                                        <asp:HyperLink runat="server" ID="hlnkApprovalPage" Text="Click here to go to the actual approval page"
                                            Visible="false" Font-Bold="true" Font-Underline="true" ForeColor="Blue"></asp:HyperLink>
                                        <br />
                                        <br />
                                        <asp:GridView runat="server" ID="gvApprovals" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" DataSourceID="odsApprovals" DataKeyNames="RowID"
                                            ShowFooter="True" EmptyDataText="no approvals found" Width="98%" SkinID="StandardGrid">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Action">
                                                    <EditItemTemplate>
                                                        <asp:ImageButton ID="ibtnUpdateApproval" runat="server" CausesValidation="True" CommandName="Update"
                                                            ImageUrl="~/images/save.jpg" ToolTip="Save" ValidationGroup="vgEditTaskTeamMember" />&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="ibtnCancelApprovals" runat="server" CausesValidation="False"
                                                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtnEditApproval" runat="server" CausesValidation="False" CommandName="Edit"
                                                            ToolTip="Edit" ImageUrl="~/images/edit.jpg" />&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="ibtnApprovalDelete" runat="server" CausesValidation="False"
                                                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="60px" />
                                                    <FooterTemplate>
                                                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertApproval"
                                                            runat="server" ID="iBtnInserApproval" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                                        <asp:ImageButton ID="iBtnUndoInserApproval" runat="server" CommandName="Undo" CausesValidation="false"
                                                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Level" SortExpression="RoutingLevel">
                                                    <HeaderStyle Wrap="true" />
                                                    <FooterStyle Wrap="false" />
                                                    <ItemStyle Wrap="true" />
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditRoutingLevel" runat="server" Text='<%# Bind("RoutingLevel") %>'
                                                            MaxLength="1" Width="25px"></asp:TextBox><br />
                                                        <asp:RequiredFieldValidator ID="rfvEditRoutingLevel" runat="server" ControlToValidate="txtEditRoutingLevel"
                                                            ErrorMessage="The level is required." Font-Bold="True" ValidationGroup="vgEditApprovals"
                                                            Text="<" SetFocusOnError="true" />
                                                        <ajax:FilteredTextBoxExtender ID="ftEditRoutingLevel" runat="server" TargetControlID="txtEditRoutingLevel"
                                                            FilterType="Numbers" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblViewRoutingLevel" runat="server" Text='<%# Bind("RoutingLevel") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Team Member" SortExpression="FullTeamMemberName">
                                                    <HeaderStyle Wrap="true" />
                                                    <FooterStyle Wrap="false" />
                                                    <ItemStyle Wrap="true" />
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddEditTeamMember" runat="server" DataSource='<%# Commonfunctions.GetTeamMember("") %>'
                                                            DataValueField="TeamMemberID" DataTextField="TeamMemberName" SelectedValue='<%# Bind("TeamMemberID") %>'>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvEditTeamMember" runat="server" ControlToValidate="ddEditTeamMember"
                                                            ErrorMessage="The team member is required." Font-Bold="True" ValidationGroup="vgEditApprovals"
                                                            Text="<" SetFocusOnError="true" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblViewTeamMemberName" runat="server" Text='<%# Bind("FullTeamMemberName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddInsertTeamMember" runat="server" DataSource='<%# Commonfunctions.GetTeamMember("") %>'
                                                            DataValueField="TeamMemberID" DataTextField="TeamMemberName" AppendDataBoundItems="True">
                                                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvInsertTeamMember" runat="server" ControlToValidate="ddInsertTeamMember"
                                                            ErrorMessage="The task is required." Font-Bold="True" ValidationGroup="vgInsertTeamMember"
                                                            Text="<" SetFocusOnError="true" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="StatusID">
                                                    <HeaderStyle Wrap="true" />
                                                    <FooterStyle Wrap="false" />
                                                    <ItemStyle Wrap="true" />
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddEditStatus" runat="server" DataSource='<%# SupportModule.GetSupportRequestApprovalStatus() %>'
                                                            DataValueField="StatusID" DataTextField="ddStatusName" SelectedValue='<%# Bind("StatusID") %>'>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddViewStatus" runat="server" DataSource='<%# SupportModule.GetSupportRequestApprovalStatus() %>'
                                                            DataValueField="StatusID" DataTextField="StatusName" SelectedValue='<%# Bind("StatusID") %>'
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="StatusDate" SortExpression="StatusDate" ShowHeader="true"
                                                    HeaderText="Statue Date" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Comments" SortExpression="Comments">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditComments" runat="server" Text='<%# Bind("Comments") %>' TextMode="MultiLine"
                                                            Rows="3" Width="300px"></asp:TextBox><br />
                                                        <asp:Label runat="server" ID="lblEditCommentsCharCount" SkinID="MessageLabelSkin"></asp:Label>
                                                        <ajax:FilteredTextBoxExtender ID="ftbeEditComments" runat="server" Enabled="True"
                                                            TargetControlID="txtEditComments" ValidChars="?!-*&| ().@åäöÅÄÖ,/'&quot;" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblViewComments" runat="server" Text='<%# Bind("Comments") %>' TextMode="MultiLine"
                                                            Rows="3" Width="300px" Enabled="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="True" />
                                                    <FooterStyle Wrap="True" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsApprovals" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetSupportRequestApproval" TypeName="SupportModule" DeleteMethod="DeleteSupportRequestApproval"
                                            InsertMethod="InsertSupportRequestApproval" UpdateMethod="UpdateSupportRequestApproval">
                                            <DeleteParameters>
                                                <asp:Parameter Name="RowID" Type="Int32" />
                                            </DeleteParameters>
                                            <UpdateParameters>
                                                <asp:Parameter Name="RowID" Type="Int32" />
                                                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                                                <asp:Parameter Name="RoutingLevel" Type="Int32" />
                                                <asp:Parameter Name="Comments" Type="String" />
                                                <asp:Parameter Name="StatusID" Type="String" />
                                            </UpdateParameters>
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblJnId" DefaultValue="0" Name="jnId" PropertyName="Text"
                                                    Type="Int32" />
                                                <asp:Parameter Name="RoutingLevel" Type="Int32" DefaultValue="0" />
                                                <asp:Parameter Name="TeamMemberID" Type="Int32" DefaultValue="0" />
                                                <asp:Parameter Name="StatusID" Type="Int32" DefaultValue="0" />
                                            </SelectParameters>
                                            <InsertParameters>
                                                <asp:ControlParameter ControlID="lblJnId" DefaultValue="0" Name="jnId" PropertyName="Text"
                                                    Type="Int32" />
                                                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                                            </InsertParameters>
                                        </asp:ObjectDataSource>
                                        <br />
                                        <asp:Button runat="server" ID="btnForwardApproval" Text="Forward Approval" />
                                    </td>
                                </tr>
                            </table>
                        </Content>
                    </ajax:AccordionPane>
                </Panes>
            </ajax:Accordion>
        </td>
    </tr>
</table>
