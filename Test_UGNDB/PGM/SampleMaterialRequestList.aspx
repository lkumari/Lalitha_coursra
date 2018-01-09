<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="SampleMaterialRequestList.aspx.vb" Inherits="PGM_SampleMaterialRequestList"
    ValidateRequest="false" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1100px" DefaultButton="btnSearch">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" />
        <table>
            <tr>
                <td class="p_smalltextbold" style="color: #990000">
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
                    <asp:Label ID="lblReqNo" runat="server" Text="Request #:" />
                </td>
                <td>
                    <asp:TextBox ID="txtSMRNo" runat="server" Width="100px" MaxLength="10" />
                    <ajax:FilteredTextBoxExtender ID="ftbSMRNo" runat="server" TargetControlID="txtSMRNo"
                        FilterType="Custom" ValidChars="1234567890%" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblSampleDesc" runat="server" Text="Sample Description:" />
                </td>
                <td>
                    <asp:TextBox ID="txtSampleDesc" runat="server" MaxLength="50" Width="300px" />
                    <ajax:FilteredTextBoxExtender ID="ftbSampleDesc" runat="server" TargetControlID="txtSampleDesc"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-/% " />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblRequestor" runat="server" Text="Requestor:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddRequestor" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblAccountManager" runat="server" Text="Account Manager:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddAccountManager" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblUGNLoc" runat="server" Text="UGN Location:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNLocation" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblPartNo" runat="server" Text="Part Number:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" runat="server" MaxLength="25" Width="200px" />
                    <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-% " />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblCustomer" runat="server" Text="Customer:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddCustomer" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblPONo" runat="server" Text="Purchase Order #:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPONo" runat="server" MaxLength="30" Width="200px" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblIntExt" runat="server" Text="Internal/External:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddIntExt" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Internal</asp:ListItem>
                        <asp:ListItem>External</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblRecStatus" runat="server" Text="Status:" />
                </td>
                <td class="c_textbold" style="color: red;">
                    <asp:DropDownList ID="ddRecStatus" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="NOpen">New Request</asp:ListItem>
                        <asp:ListItem Value="CCompleted">Completed</asp:ListItem>
                        <asp:ListItem Value="TIn Process">In Process</asp:ListItem>
                        <asp:ListItem Value="RIn Process">Rejected</asp:ListItem>
                        <asp:ListItem Value="VVoid">Void</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CommandName="search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CommandName="reset" />
                </td>
            </tr>
        </table>
        <hr />
        <i>Use the parameters above to filter the list below.</i>
        <table width="400px" border="0">
            <tr>
                <td width="80px" align="center" style="white-space: nowrap;">
                    Completed
                </td>
                <td width="80px" align="center" style="background-color: Fuchsia; white-space: nowrap;">
                    New Request
                </td>
                <td width="80px" align="center" style="background-color: yellow; white-space: nowrap;">
                    In-Process
                </td>
                <td width="80px" align="center" style="background-color: red; color: white; white-space: nowrap;">
                    Rejected
                </td>
                <td width="80px" align="center" style="background-color: gray; color: white; white-space: nowrap;">
                    Void
                </td>
            </tr>
        </table>
        <table width="1100px">
            <tr>
                <td class="c_smalltext" style="font-style: italic" width="700px">
                    <asp:Label ID="lblRecListed" runat="server" Text="Records Listed: " />
                    <asp:Label ID="lblFromRec" runat="server" ForeColor="Red" />
                    <asp:Label ID="lblTo" runat="server" Text=" to " />
                    <asp:Label ID="lblToRec" runat="server" ForeColor="Red" />
                    <asp:Label ID="lblOf" runat="server" Text=" of " />
                    <asp:Label ID="lblTotalRecords" runat="server" ForeColor="Red" />
                </td>
                <td width="400px" align="right">
                    <asp:Label ID="PagingInformation" runat="server" Text="" /><asp:DropDownList ID="PageList"
                        runat="server" CssClass="c_smalltext" AutoPostBack="true" OnSelectedIndexChanged="PageList_SelectedIndexChanged" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvSMR" runat="server" AutoGenerateColumns="False" DataSourceID="odsSMR"
            SkinID="StandardGridWOFooter" DataKeyNames="SMRNo" Width="100%" PageSize="30"
            OnRowDataBound="gvSMR_RowDataBound" OnPageIndexChanged="gvSMR_PageIndexChanged"
            OnSorting="gvSMR_Sorting" OnPageIndexChanging="gvSMR_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Status" SortExpression="RecordStatusDesc">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlnkSMRNo" runat="server" Font-Underline="true" NavigateUrl='<%# "SampleMaterialRequest.aspx?pSMRNo=" & DataBinder.Eval (Container.DataItem,"SMRNo").tostring%>'
                            Text='<%# Bind("RecordStatusDesc") %>' ForeColor='<%# SetTextColor(DataBinder.Eval(Container, "DataItem.RoutingStatus")) %>'
                            BackColor='<%# SetBackGroundColor(DataBinder.Eval(Container, "DataItem.RoutingStatus")) %>'
                            Width="80px" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Req #" SortExpression="SMRNo">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("SMRNo") %>' />
                        <itemstyle horizontalalign="center" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sample Description" SortExpression="SampleDesc">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("SampleDesc") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="IntExt" HeaderStyle-HorizontalAlign="Left" HeaderText="Int/Ext"
                    SortExpression="IntExt">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="UGNFacilityName" HeaderStyle-HorizontalAlign="Left" HeaderText="UGN Location"
                    SortExpression="UGNFacilityName">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Customer" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer"
                    SortExpression="Customer">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="ProdLevel" HeaderText="Production Level" SortExpression="ProdLevel"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" SortExpression="IssueDate"
                    HeaderStyle-HorizontalAlign="center">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate"
                    HeaderStyle-HorizontalAlign="center">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Preview">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl="~/images/PreviewUp.jpg"
                            NavigateUrl='<%# "crViewSampleMtrlReq.aspx?pSMRNo=" & DataBinder.Eval (Container.DataItem,"SMRNo").tostring %>'
                            Target="_blank" ToolTip="Preview" />
                        <itemstyle horizontalalign="center" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="History">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlnkHistory" ImageUrl="~/images/History.jpg" Target="_blank"
                            NavigateUrl='<%# "SampleMtrlReqHistory.aspx?pSMRNo=" & DataBinder.Eval (Container.DataItem,"SMRNo").tostring %>'
                            ToolTip="Preview" />
                        <itemstyle horizontalalign="center" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsSMR" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetSampleMtrlReq" TypeName="PGMBLL">
            <SelectParameters>
                <asp:CookieParameter CookieName="SMR_SMRNO" Name="SMRNo" Type="String" />
                <asp:CookieParameter CookieName="SMR_SDESC" Name="SampleDesc" Type="String" />
                <asp:CookieParameter CookieName="SMR_RTMID" Name="RequestorTMID" Type="Int32" DefaultValue="0" />
                <asp:CookieParameter CookieName="SMR_ATMID" Name="AccountMgrTMID" Type="Int32" DefaultValue="0" />
                <asp:CookieParameter CookieName="SMR_UFAC" Name="UGNFacility" Type="String" DefaultValue="" />
                <asp:CookieParameter CookieName="SMR_CUST" Name="Customer" Type="String" />
                <asp:CookieParameter CookieName="SMR_PNO" DefaultValue="" Name="PartNo" Type="String" />
                <asp:CookieParameter CookieName="SMR_IE" Name="IntExt" Type="String" />
                <asp:CookieParameter CookieName="SMR_PONO" Name="PONo" Type="String" />
                <asp:CookieParameter CookieName="SMR_RSTAT" Name="RecStatus" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:Label class="c_smalltext" ID="SortInformationLabel" runat="server" />
        <asp:HiddenField ID="hiddenCatIDs" runat="server" />
    </asp:Panel>
</asp:Content>
