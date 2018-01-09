<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crSupport_Approval.aspx.vb" MaintainScrollPositionOnPostback="true"
    Inherits="crSupport_Approval" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table width="60%">
            <tr>
                <td class="c_text" colspan="2">
                    <b>Review the information for a Support Request:
                        <asp:Label runat="server" ID="lblJobNumber" Text="Unassigned" ForeColor="Red" Font-Size="Medium"></asp:Label>
                        <asp:Label runat="server" ID="lblJnId" Text="0" CssClass="none"></asp:Label>
                        and submit your response in the section provided.</b>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Team Member:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddTeamMember" Enabled="false" AutoPostBack="true">
                    </asp:DropDownList>
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
                        Width="350px"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rfvApprovalComment" EnableClientScript="true"
                        ControlToValidate="txtApprovalComment" ErrorMessage="Comment is needed for rejection"
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
                    <asp:DropDownList ID="ddApprovalStatus" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvApprovalStatus" EnableClientScript="true"
                        Enabled="false" ControlToValidate="txtApprovalComment" ErrorMessage="Status selection is required"
                        ValidationGroup="vgSave" SetFocusOnError="True" Text="<">
                    </asp:RequiredFieldValidator>
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
            <tr>
                <td colspan="2">
                    <asp:GridView runat="server" ID="gvSupportingDoc" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" DataSourceID="odsSupportingDoc" DataKeyNames="RowID"
                        EmptyDataText="No Supporing Docs Attached" Width="98%">
                        <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Medium" />
                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#CCCCCC" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                        <Columns>
                            <asp:BoundField DataField="RowID">
                                <ItemStyle CssClass="none" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Preview Document">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.SupportingDocEncodeType").tostring)  %>'
                                        NavigateUrl='<%# "Supporting_Doc_Viewer.aspx?jnId=" & DataBinder.Eval (Container.DataItem,"jnId").tostring & "&RowID=" & DataBinder.Eval (Container.DataItem,"RowId").tostring %>'
                                        Target="_blank" ToolTip="Preview Document" />
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
                        SelectMethod="GetSupportingDoc" TypeName="SupportModule">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="lblJnId" DefaultValue="0" Name="jnId" PropertyName="Text"
                                Type="Int32" />
                            <asp:Parameter Name="RowID" Type="Int32" DefaultValue="0" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
        </table>
        <br />
        <asp:Label runat="server" ID="lblMessageBottom" SkinID="MessageLabelSkin"></asp:Label>
        <br />
    </asp:Panel>
    <br />
    <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
        ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="false"
        BackColor="White" Width="1000px" Height="1350px" EnableDatabaseLogonPrompt="False"
        HasCrystalLogo="False" HasPageNavigationButtons="True" DisplayGroupTree="False"
        HasSearchButton="False" HasToggleGroupTreeButton="False" PageZoomFactor="125"
        HyperlinkTarget="_blank" HasDrillUpButton="False" />
    <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="Workflow\Forms\Support.rpt">
        </Report>
    </CrystalRpt:CrystalReportSource>
</asp:Content>
