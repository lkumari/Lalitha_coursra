<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="TestIssuanceList.aspx.vb" Inherits="RnD_TIL" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1150px" DefaultButton="btnSearch">
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
        <i>Partial Searches can be completed by placing % before or after text.</i>
        <table width="100%" border="0">
            <tr>
                <td class="p_text">
                    Request No:
                </td>
                <td>
                    <asp:TextBox ID="txtRequestID" runat="server" MaxLength="15" Width="100px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbeRequestID" runat="server" TargetControlID="txtRequestID"
                        FilterType="Custom, Numbers" ValidChars="%" />
                </td>
                <td class="p_text">
                    Sample Issuer:
                </td>
                <td>
                    <asp:DropDownList ID="ddSampleIssuer" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Sample Product Description:
                </td>
                <td>
                    <asp:TextBox ID="txtSampleProdDesc" runat="server" MaxLength="100" Width="300px" />
                    <ajax:FilteredTextBoxExtender ID="ftbSampleProdDesc" runat="server" TargetControlID="txtSampleProdDesc"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,% " />
                </td>
                <td class="p_text">
                    UGN Location:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" valign="top">
                    Commodity:
                </td>
                <td>
                    <asp:DropDownList ID="ddCommodity" runat="server" />
                    <br />
                    {Commodity / Classification}
                </td>
                <td class="p_text" style="height: 26px">
                    Request Status:
                </td>
                <td style="color: #990000; height: 26px" class="c_text">
                    <asp:DropDownList ID="ddRequestStatus" runat="server">
                        <asp:ListItem Selected="True"></asp:ListItem>
                        <asp:ListItem>Unassigned</asp:ListItem>
                        <asp:ListItem>Abandoned</asp:ListItem>
                        <asp:ListItem>Completed</asp:ListItem>
                        <asp:ListItem>Nearly Complete</asp:ListItem>
                        <asp:ListItem>On Hold</asp:ListItem>
                        <asp:ListItem>Outstanding</asp:ListItem>
                        <asp:ListItem>Overdue</asp:ListItem>
                        <asp:ListItem>Testing In Progress</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Part No:
                </td>
                <td>
                    <asp:TextBox ID="txtPNO" runat="server" Width="200px" MaxLength="25"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbPNO" runat="server" TargetControlID="txtPno"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-% " />
                </td>
                <td class="p_text">
                    Request Category:
                </td>
                <td>
                    <asp:DropDownList ID="ddRequestCategory" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="1">Product Innovation</asp:ListItem>
                        <asp:ListItem Value="2">Current Mass Production</asp:ListItem>
                        <asp:ListItem Value="3">Consultation</asp:ListItem>
                        <asp:ListItem Value="4">New Program Launch</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Test Report No:
                </td>
                <td>
                    <asp:TextBox ID="txtTestRptNo" runat="server" MaxLength="15" Width="100px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbeTestRptNo" runat="server" TargetControlID="txtTestRptNo"
                        FilterType="Custom, Numbers" ValidChars="%" />
                </td>
                <td class="p_text">
                    TAG:
                </td>
                <td>
                    <asp:TextBox ID="txtTAG" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbTAG" runat="server" TargetControlID="txtTAG"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Testing Classification:
                </td>
                <td>
                    <asp:DropDownList ID="ddTestClass" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    Acoustic Project No:
                </td>
                <td>
                    <asp:TextBox ID="txtProjectID" runat="server" MaxLength="15" Width="100px" /><ajax:FilteredTextBoxExtender
                        ID="ftbeProjectID" runat="server" TargetControlID="txtProjectID" FilterType="Custom, Numbers"
                        ValidChars="%" />
                </td>
            </tr>
            <tr>
                <td class="p_text" valign="top">
                    Program:
                </td>
                <td>
                    <asp:DropDownList ID="ddProgram" runat="server" AppendDataBoundItems="true" />
                    <br />
                    {Program / Platform / Model / Assembly Plant}
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Appropriation (A, D, P, T, R):
                </td>
                <td>
                    <asp:TextBox ID="txtAppropriation" runat="server" Width="100px" MaxLength="25" />
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
                    <td align="right" colspan="8">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgTIList" ErrorMessage="Numeric Value Required." SetFocusOnError="True"
                            ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgTIList" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                        &nbsp;<asp:Repeater ID="rpTestIssuance" runat="server">
                            <SeparatorTemplate>
                                <tr>
                                    <td colspan="10">
                                        <hr style="height: 0.01em" />
                                    </td>
                                </tr>
                            </SeparatorTemplate>
                            <HeaderTemplate>
                                <tr>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:LinkButton ID="lnkReqNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="RequestID">Request<br />No</asp:LinkButton>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="lnkDesc" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="SampleProductDescription">Sample Product Description</asp:LinkButton>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="lnkClass" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="CommodityName">Commodity / Classification</asp:LinkButton>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="lnkFac" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="UGNFacilityName">UGN Location</asp:LinkButton>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="lnkIssuer" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="SampleIssuerName">Sample Issuer</asp:LinkButton>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="lnkReqDat" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="RequestDate">Request Date</asp:LinkButton>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="lnkReqCat" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="ReqCategoryDesc">Request Category</asp:LinkButton>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="lnkReqStat" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="RequestStatus">Request Status</asp:LinkButton>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:LinkButton ID="lnkProjID" ForeColor="white" runat="server" OnClick="SortCommand"
                                            CommandArgument="ProjectID">Acoustic Project No</asp:LinkButton>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="lnkVolume" runat="server">Preview</asp:Label>
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td align="center">
                                        <asp:HyperLink ID="selectVehicleVolume" Font-Underline="true" runat="server" NavigateUrl='<%# "TestIssuanceDetail.aspx?pReqID=" & DataBinder.Eval (Container.DataItem,"RequestID").tostring & "&pReqCategory=" & DataBinder.Eval (Container.DataItem,"RequestCategory").tostring %>'>
                                 <%#DataBinder.Eval(Container, "DataItem.RequestID")%>
                                        </asp:HyperLink>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.SampleProductDescription")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.CommodityName")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.UGNFacilityName")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.SampleIssuerName")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.RequestDate")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ReqCategoryDesc")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.RequestStatus")%>
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="hlProjID" runat="server" Target="_blank" Font-Underline="true"
                                            NavigateUrl='<%# GoToAcoustic(Replace(DataBinder.Eval(Container.DataItem, "ProjectID" ) & "", ",", environment.newline)) %>'>
                                        <%# Replace(DataBinder.Eval(Container.DataItem, "ProjectID" ) & "", ",", environment.newline) %>
                                        </asp:HyperLink>
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="lnkPreview" runat="server" NavigateUrl='<%# "crViewTestIssuanceRequestForm.aspx?pReqID=" & DataBinder.Eval (Container.DataItem,"RequestID").tostring %>'
                                            Target="_blank" ImageUrl="~/images/PreviewUp.jpg"></asp:HyperLink>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="10">
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
