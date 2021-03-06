<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ToolingExpProjList.aspx.vb" Inherits="EXP_ToolingExpProjList" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1150px" DefaultButton="btnSearch">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" />
        <table>
            <tr>
                <td class="p_smalltextbold" style="color: #990000">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.<br />
                    Enter Last Primary or Supplement No from old TE Tracking system to start next sequence.
                    <asp:TextBox ID="txtLastSupplementNo" runat="server" MaxLength="15" Width="100px" />
                    <ajax:FilteredTextBoxExtender ID="ftbLS" runat="server" TargetControlID="txtLastSupplementNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" />
                    <asp:Button ID="btnGo" runat="server" Text="Go" />
                </td>
            </tr>
        </table>
        <hr />
        <i>Partial Searches can be completed by placing % before or after text.</i>
        <table width="100%" border="0">
            <tr>
                <td class="p_text">
                    Project No:
                </td>
                <td>
                    <asp:TextBox ID="txtProjectNo" runat="server" MaxLength="15" Width="100px" />
                    <ajax:FilteredTextBoxExtender ID="ftbProjectNo" runat="server" TargetControlID="txtProjectNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%" />
                </td>
                <td class="p_text">
                    Supplement Project No:
                </td>
                <td>
                    <asp:TextBox ID="txtSupProjectNo" runat="server" MaxLength="15" Width="100px" />
                    <ajax:FilteredTextBoxExtender ID="ftbSupProjectNo" runat="server" TargetControlID="txtSupProjectNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Project Title:
                </td>
                <td>
                    <asp:TextBox ID="txtProjectTitle" runat="server" MaxLength="50" Width="250px" />
                    <ajax:FilteredTextBoxExtender ID="ftbProjectTitle" runat="server" TargetControlID="txtProjectTitle"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, -%." />
                </td>
                <td class="p_text">
                    Project Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddProjectType" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Internal</asp:ListItem>
                        <asp:ListItem>External</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Account Manager:
                </td>
                <td>
                    <asp:DropDownList ID="ddAccountManager" runat="server" />
                </td>
                <td class="p_text">
                    Program Manager:
                </td>
                <td>
                    <asp:DropDownList ID="ddProgramManager" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Tooling Lead:
                </td>
                <td>
                    <asp:DropDownList ID="ddToolingLead" runat="server" />
                </td>
                <td class="p_text">
                    Purchasing Lead:
                </td>
                <td>
                    <asp:DropDownList ID="ddPurchasingLead" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    UGN Location:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                </td>
                <td class="p_text">
                    Customer:
                </td>
                <td>
                    <asp:DropDownList ID="ddCustomer" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Part Number:
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" runat="server" MaxLength="25" Width="200px" />
                    <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-% " />
                </td>
                <td class="p_text">
                    Part Description:
                </td>
                <td>
                    <asp:TextBox ID="txtPartDesc" runat="server" Width="200px" MaxLength="50" />
                    <ajax:FilteredTextBoxExtender ID="ftbPartDesc" runat="server" TargetControlID="txtPartDesc"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-/.% " />
                </td>
            </tr>
            <tr>
                <td class="p_text" valign="top">
                    Program:
                </td>
                <td>
                    <asp:DropDownList ID="ddProgram" runat="server" />
                    <br />
                    {Program / Model / Platform / Assembly Plant}
                </td>
                <td class="p_text" valign="top">
                    Project Status:
                </td>
                <td class="c_textbold" style="color: red;">
                    <asp:DropDownList ID="ddProjectStatus" runat="server">
                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="NOpen">New Project</asp:ListItem>
                        <asp:ListItem Value="AApproved">Approved</asp:ListItem>
                        <asp:ListItem Value="CClosed">Closed</asp:ListItem>
                        <asp:ListItem Value="ATooling Completed">Tooling Completed</asp:ListItem>
                        <asp:ListItem Value="TIn Process">In Process</asp:ListItem>
                        <asp:ListItem Value="RIn Process">Rejected</asp:ListItem>
                        <asp:ListItem Value="VVoid">Void</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CommandName="search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CommandName="reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <i>Use the parameters above to filter the list below.</i>
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
                        <asp:Label ID="lblCurrentPage" runat="server" />
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
                        &nbsp;<asp:Repeater ID="rpToolingExpProj" runat="server">
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
                                        <asp:Label ID="Label3" runat="server">Project No.</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label6" runat="server">Project Title</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label5" runat="server">Project Type</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label1" runat="server">UGN Location</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="lnkProgram" runat="server">Date<br />Submitted</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label4" runat="server">Project<br />Status</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label8" runat="server">Carried<br />Over</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" style="width: 60px" align="center">
                                        <asp:Label ID="Label2" runat="server">Add<br />Supplement</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="lnkVolume" runat="server">Preview</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label7" runat="server">History</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label9" runat="server">IOR's</asp:Label>
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="getData" Font-Underline="true" runat="server" NavigateUrl='<%# "ToolingExpProj.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring & "&pPrntProjNo=" &  DataBinder.Eval (Container.DataItem,"ParentProjectNo").tostring & "&pLS=" & DataBinder.Eval(Container.Dataitem,"OldSysCarryOver").ToSTring %>'>
                                 <%#DataBinder.Eval(Container, "DataItem.ProjectNo")%>
                                        </asp:HyperLink>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ProjectTitle")%>
                                    </td>
                                    <td align="center">
                                        <%#DataBinder.Eval(Container, "DataItem.ProjectType")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.UGNFacilityName")%>
                                    </td>
                                    <td align="center">
                                        <%#DataBinder.Eval(Container, "DataItem.DateSubmitted")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ProjectStatusDesc")%>
                                    </td>
                                    <td align="center">
                                        <%#DataBinder.Eval(Container, "DataItem.CarriedOver")%>
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/copy.jpg" Height="24"
                                            Width="24" NavigateUrl='<%# GoToAppend(DataBinder.Eval(Container, "DataItem.ProjectNo"),DataBinder.Eval(Container, "DataItem.ParentProjectNo"),DataBinder.Eval(Container, "DataItem.OldSysCarryOver"))  %>'
                                            Visible='<%# ShowHideImageAppend(DataBinder.Eval(Container, "DataItem.ParentProjectNo"),DataBinder.Eval(Container, "DataItem.RoutingStatus"),DataBinder.Eval(Container, "DataItem.OldSysCarryOver")) %>' />
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="lnkPreview" runat="server" NavigateUrl='<%# "crViewExpProjTooling.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring & "&pLS=" & DataBinder.Eval(Container.Dataitem,"OldSysCarryOver").ToSTring %>'
                                            Target="_blank" ImageUrl="~/images/PreviewUp.jpg"></asp:HyperLink>
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="lnkHistory" runat="server" NavigateUrl='<%# "ToolingExpProjHistory.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring & "&pAprv=" & ViewState("pAprv") %>'
                                            ImageUrl="~/images/History.jpg" Visible='<%# ShowHideHistory(DataBinder.Eval(Container, "DataItem.ProjectStatus")) %>' />
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# "IORsByAppropriation.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring  %>'
                                            ImageUrl="~/images/VolSales.jpg" Visible='<%# ShowHideIORimg(DataBinder.Eval(Container, "DataItem.ProjectNo")) %>' />
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
