<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Acoustic_Lab_Testing_List.aspx.vb" Inherits="Acoustic_Acoustic_Lab_Testing_List"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch" Width="100%">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False"></asp:Label>
        <table>
            <tr>
                <td class="p_smalltextbold" style="width: 279px; color: #990000">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.
                </td>
            </tr>
        </table>
        <hr />
        <table width="100%" border="0">
            <tr>
                <td class="p_text" align="right" style="width: 154px">
                    Project No:
                </td>
                <td>
                    <asp:TextBox ID="txtProjectNo" runat="server" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbProjectNo" runat="server" TargetControlID="txtProjectNo"
                        FilterType="Custom, Numbers" ValidChars="%" />
                </td>
                <td class="p_text" align="right">
                    Project Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddProjectStatus" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="width: 154px" >
                    Customer:
                </td>
                <td>
                    <asp:DropDownList ID="ddCustomer" runat="server" />
                   
                </td>
                <td class="p_text" align="right" valign="top">
                    Program:
                </td>
                <td>
                    <asp:DropDownList ID="ddProgram" runat="server" />
                    <br />
                    {Program / Platform / Model / Assembly Plant}
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="width: 154px">
                    Project Requester:
                </td>
                <td>
                    <asp:DropDownList ID="ddSubmittedBy" runat="server" />
                </td>
                <td class="p_text" align="right">
                    Autoneum Reference No:
                </td>
                <td>
                    <asp:TextBox ID="txtReiterRefNo" runat="server" Width="80" />
                    <ajax:FilteredTextBoxExtender ID="ftbReiterRefNo" runat="server" TargetControlID="txtReiterRefNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,% " />
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="height: 24px; width: 154px;">
                    Test Description:
                </td>
                <td>
                    <asp:TextBox ID="txtTestDescription" runat="server" Width="300px" />
                    <ajax:FilteredTextBoxExtender ID="ftbTestDescription" runat="server" TargetControlID="txtTestDescription"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,% " />
                </td>
                <td class="p_text" align="right">
                    R&amp;D Test Request No:
                </td>
                <td>
                    <asp:TextBox ID="txtRequestNo" runat="server" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbeRequestNo" runat="server" FilterType="Custom, Numbers"
                        TargetControlID="txtRequestNo" ValidChars="%" />
                </td>
            </tr>
            <tr>
                <td style="width: 154px">
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CommandName="search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CommandName="reset" />
                </td>
            </tr>
        </table>
        <hr />
        <i>Use the parameters above to filter the list below</i>
        <table id="TABLE1" width="98%">
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
                    <td colspan="9" align="right">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgALList" ErrorMessage="Only numbers can be used for the pages."
                            SetFocusOnError="True" ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgALList" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="11">
                        <asp:Repeater ID="rpProjectInfo" runat="server">
                            <SeparatorTemplate>
                                <tr>
                                    <td colspan="11">
                                        <hr style="height: 0.01em" />
                                    </td>
                                </tr>
                            </SeparatorTemplate>
                            <HeaderTemplate>
                                <table width="100%">
                                    <tr>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkProjNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ProjectID">Project<br />No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor">
                                            <asp:LinkButton ID="lnkTestDesc" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="TestDescription">Test Description</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor">
                                         <asp:LinkButton ID="lnkCustomer" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddCustomerDesc">Customer</asp:LinkButton>
                                            
                                        </td>
                                        <td class="p_tablebackcolor">
                                            <asp:LinkButton ID="lnkPgm" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ProgramName">Program</asp:LinkButton> 
                                        </td>
                                        <td class="p_tablebackcolor">
                                           <asp:LinkButton ID="lnkRRNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ReiterRefNo">Autoneum Ref No</asp:LinkButton>   
                                        </td>
                                        <td class="p_tablebackcolor">
                                               <asp:LinkButton ID="lnkPrjStat" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="Status">Project Status</asp:LinkButton>   
                                        </td>
                                        <td class="p_tablebackcolor">
                                             <asp:LinkButton ID="lnkDtReq" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="DateRequested">Date Requested</asp:LinkButton>    
                                        </td>
                                        <td class="p_tablebackcolor">
                                            <asp:LinkButton ID="lnkTestIssuance" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="RequestID">R&amp;D Test Request No</asp:LinkButton>
                                        </td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td align="center">
                                        <asp:HyperLink ID="selectProject" Font-Underline="True" runat="server" NavigateUrl='<%# "Acoustic_Project_Detail.aspx?pProjID=" & DataBinder.Eval (Container.DataItem,"projectID").tostring & "&pRptID=-1"  %>'><%#DataBinder.Eval(Container, "DataItem.projectID")%></asp:HyperLink>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.TestDescription")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ddCustomerDesc")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ProgramName")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ReiterRefNo")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.Status")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.DateRequested")%>
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="hlReqID" runat="server" Target="_blank" Font-Underline="true"
                                            NavigateUrl='<%# GoToTestRequest(Replace(DataBinder.Eval(Container.DataItem, "RequestID" ) & "", ",", environment.newline),Replace(DataBinder.Eval(Container.DataItem, "RequestCategory")  & "", ",", environment.newline)) %>'>
                                        <%# Replace(DataBinder.Eval(Container.DataItem, "RequestID" ) & "", ",", environment.newline) %>
                                        </asp:HyperLink>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
