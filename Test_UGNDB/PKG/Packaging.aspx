<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Packaging.aspx.vb" Inherits="PKG_Packaging" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    &nbsp;&nbsp;&nbsp;
    <asp:Panel ID="localPanel" runat="server" Width="1100px">
        <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0000" SkinID="MessageLabelSkin"></asp:Label>
        <% If ViewState("pPKG") <> Nothing Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="width: 532px; color: #990000">
                    Edit data below or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="False" />
                    to enter new data.<%--&nbsp; Press
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" />
                    to carry data into a new part number(s).--%>
                </td>
            </tr>
        </table>
        <%  End If%>
        <hr />
        <br />
        <table>
            <tr>
                <td class="c_textbold">
                    <asp:Label ID="txtPKGID" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    <asp:Label ID="lblDescription" runat="server" Text="Layout Description:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="240" Width="400" />
                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                        Enabled="True" ErrorMessage="Layout Description is a required field." Font-Bold="False"
                        ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><br />
                    <asp:Label ID="lblDescChar" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label13" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    <asp:Label ID="Label11" runat="server" EnableViewState="False" Text="Packing Leader:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddPackingLead" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvPackagingLead" runat="server" ControlToValidate="ddPackingLead"
                        Enabled="True" ErrorMessage="Packaging Lead is a required field." Font-Bold="False"
                        ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label6" runat="server" Text="Is Publish:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddIsPublish" runat="server">
                        <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                        <asp:ListItem Value="True">Yes</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <ajax:CascadingDropDown ID="cddUGNLocation" runat="server" TargetControlID="ddUGNLocation"
            Category="UGNLocation" PromptText="Please select a UGN Location." LoadingText="[Loading UGN Location(s)...]"
            ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetUGNLocationByTMFac" />
        <ajax:CascadingDropDown ID="cddDepartment" runat="server" TargetControlID="ddDepartment"
            ParentControlID="ddUGNLocation" Category="Department" PromptText="Please select a Department."
            LoadingText="[Loading Department(s)...]" ServicePath="~/WS/GeneralCDDService.asmx"
            ServiceMethod="GetDepartment" />
        <ajax:CascadingDropDown ID="cddWorkCenter" runat="server" TargetControlID="ddWorkCenter"
            ParentControlID="ddDepartment" Category="WorkCenter" PromptText="Please select a Work Center"
            LoadingText="[Loading Work Center(s)...]" ServicePath="~/WS/GeneralCDDService.asmx"
            ServiceMethod="GetWorkCenter" />
        <ajax:CascadingDropDown ID="cddOEMMfg" runat="server" TargetControlID="ddOEMMfg"
            Category="OEMMfg" PromptText="Please select an OEM Manufacturer." LoadingText="[Loading OEM Manufacturer(s)...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetOEMMfg" />
        <ajax:CascadingDropDown ID="cddContainerNo" runat="server" TargetControlID="ddContainerNo"
            ParentControlID="ddOEMMfg" Category="ContainerNo" PromptText="Please select a Container No."
            LoadingText="[Loading Container No(s)...]" ServicePath="~/WS/GeneralCDDService.asmx"
            ServiceMethod="GetPKGContainerByOEMMfg" />
        <ajax:CascadingDropDown ID="cddMake" runat="server" TargetControlID="ddMake" ParentControlID="ddOEMMfg"
            Category="Make" PromptText="Please select a Make. " LoadingText="[Loading Make(s)...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetMakesSearch" />
        <ajax:CascadingDropDown ID="cddModel" runat="server" TargetControlID="ddModel" ParentControlID="ddMake"
            Category="Model" PromptText="Please select a Model. " LoadingText="[Loading Model(s)...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetModelSearch" />
        <ajax:CascadingDropDown ID="cddProgram" runat="server" TargetControlID="ddProgram"
            ParentControlID="ddModel" Category="Make" PromptText="Please select a Program. "
            LoadingText="[Loading Program(s)...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetPrograms" />
        <br />
        <table width="100%" border="0">
            <tr>
                <td style="width: 30px">
                    <asp:Menu ID="mnuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Detail" Value="0" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Part Detail" Value="1" ImageUrl="" />
                            <asp:MenuItem Text="Instructions" Value="2" ImageUrl="" />
                            <%--   <asp:MenuItem Text="Picture" Value="3" ImageUrl="" />--%>
                        </Items>
                    </asp:Menu>
                </td>
            </tr>
        </table>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwDetail" runat="server">
                <table width="1100px">
                    <tr>
                        <td style="width: 600px">
                            <table>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        <asp:Label ID="lblUGNLocation" runat="server" Text="UGN Location:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddUGNLocation" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvUGNLocation" runat="server" ControlToValidate="ddUGNLocation"
                                            Enabled="True" ErrorMessage="UGN Location is a required field." Font-Bold="False"
                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        <asp:Label ID="lblDepartment" runat="server" Text="Department:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddDepartment" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ControlToValidate="ddDepartment"
                                            Enabled="True" ErrorMessage="Department is a required field." Font-Bold="False"
                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label14" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        <asp:Label ID="lblWorkCenter" runat="server" Text="Work Center:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddWorkCenter" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvWorkCenter" runat="server" ControlToValidate="ddWorkCenter"
                                            Enabled="True" ErrorMessage="Work Center is a required field." Font-Bold="False"
                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 15">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        <asp:Label ID="lblOEMMfg" runat="server" EnableViewState="False" Text="Customer:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddOEMMfg" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvOEMMfg" runat="server" ControlToValidate="ddOEMMfg"
                                            Enabled="True" ErrorMessage="OEM Manufacturer is a required field." Font-Bold="False"
                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        <asp:Label ID="lblContainer" runat="server" EnableViewState="False" Text="Container No:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddContainerNo" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvContainerNo" runat="server" ControlToValidate="ddContainerNo"
                                            Enabled="True" ErrorMessage="Container No is a required field." Font-Bold="False"
                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 15">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        <asp:Label ID="lblModelYr" runat="server" Text="Model Year:" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtModelYr" runat="server" MaxLength="18" Width="80px" />
                                        <ajax:FilteredTextBoxExtender ID="ftbeModelYr" runat="server" FilterType="Custom, Numbers"
                                            TargetControlID="txtModelYr" ValidChars="-." />
                                        <asp:RequiredFieldValidator ID="rfvModelYr" runat="server" ControlToValidate="txtModelYr"
                                            Enabled="True" ErrorMessage="Model Year is a required field." Font-Bold="False"
                                            ValidationGroup="vsDetail">&lt;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        <asp:Label ID="lblMake" runat="server" EnableViewState="False" Text="Make:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddMake" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvMake" runat="server" ControlToValidate="ddMake"
                                            Enabled="True" ErrorMessage="Make is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        <asp:Label ID="lblModel" runat="server" EnableViewState="False" Text="Model:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddModel" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="ddModel"
                                            Enabled="True" ErrorMessage="Model is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        <asp:Label ID="lblProgram" runat="server" EnableViewState="False" Text="Program:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddProgram" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                            Enabled="True" ErrorMessage="Program is a required field." Font-Bold="False"
                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 15">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        <asp:Label ID="lblGrossWeight" runat="server" Text="Gross Weight (lbs):" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGrossWeight" runat="server" MaxLength="18" Width="80px" />
                                        <ajax:FilteredTextBoxExtender ID="ftbeGrossWeight" runat="server" FilterType="Custom, Numbers"
                                            TargetControlID="txtGrossWeight" ValidChars="-." />
                                        <asp:RequiredFieldValidator ID="rfvGrossWeight" runat="server" ControlToValidate="txtGrossWeight"
                                            Enabled="True" ErrorMessage="Gross Weight is a required field." Font-Bold="False"
                                            ValidationGroup="vsDetail">&lt;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" valign="top">
                                        <asp:Label ID="lblNotes" runat="server" Text="Notes:" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNotes" runat="server" MaxLength="200" Rows="6" TextMode="MultiLine"
                                            Width="300px" /><br />
                                        <asp:Label ID="lblNotesChar" runat="server" Font-Bold="True" ForeColor="Red" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="lblObsolete" runat="server" Text="Obsolete:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddObsolete" runat="server">
                                            <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                                            <asp:ListItem Value="True">Yes</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" CausesValidation="True" Text="Save" ValidationGroup="vsDetail"
                                            Width="80px" />
                                        <asp:Button ID="btnReset" runat="server" Text="Reset" Width="80px" />
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:ValidationSummary ID="sDetail" runat="server" ShowMessageBox="True" ValidationGroup="vsDetail" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" style="width: 500px">
                            <table style="border: solid" id="tblImage">
                                <tr>
                                    <td class="p_text" valign="top">
                                        <asp:Label ID="lblSelectFile" runat="server" Text="Select a File for Image Upload:" />
                                    </td>
                                    <td class="c_text">
                                        <asp:FileUpload ID="uploadFile" runat="server" Height="22px" Width="400px" />
                                        <asp:RequiredFieldValidator ID="rfvUpload" runat="server" ControlToValidate="uploadFile"
                                            ErrorMessage="Image is required." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator><br />
                                        <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Only *.JPG files are allowed!"
                                            ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.jpg|.JPG)$"
                                            ControlToValidate="uploadFile" ValidationGroup="vsSupportingDocuments" Font-Bold="True"
                                            Font-Size="Small" /><br />
                                        <asp:Label ID="lblMessage2" runat="server" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 27px">
                                    </td>
                                    <td style="height: 27px">
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="true" ValidationGroup="vsSupportingDocuments" />
                                        <asp:Button ID="btnResetFile" runat="server" CausesValidation="False" Text="Reset" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <img alt="" src="" runat="server" id="imgPicture" visible="false" />
                                        <%--     <ajax:Seadragon ID="sdPicture" runat="server" CssClass="seadragon"/>--%>
                                    </td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="vsSupDoc" runat="server" ValidationGroup="vsSupportingDocuments"
                                ShowMessageBox="true" ShowSummary="true" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwPartDetail" runat="server">
                <%-- <asp:GridView ID="gvPartDetail" runat="server" SkinID="StandardGrid" AllowPaging="True"
                    AllowSorting="True" AutoGenerateColumns="False" DataSourceID="odsPartDetail"
                    PageSize="30" DataKeyNames="PKGID" OnRowCommand="gvPartDetail_RowCommand">
                </asp:GridView>
                <asp:ObjectDataSource ID="odsPartDetail" runat="server" 
                    DeleteMethod="DeletePKGLayoutPartNo" InsertMethod="InsertPKGLayoutPartNo" 
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetPKGLayoutPartNo" 
                    TypeName="PKGBLL" UpdateMethod="UpdatePKGLayoutPartNo">
                    <DeleteParameters>
                        <asp:Parameter Name="PKGID" Type="Int32" />
                        <asp:Parameter Name="PartNo" Type="String" />
                        <asp:Parameter Name="original_PKGID" Type="Int32" />
                        <asp:Parameter Name="original_PartNo" Type="String" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="original_PKGID" Type="Int32" />
                        <asp:Parameter Name="original_PartNo" Type="String" />
                        <asp:Parameter Name="QtyPckd" Type="Int32" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="PKGID" QueryStringField="pPKGID" Type="Int32" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="PKGID" Type="Int32" />
                        <asp:Parameter Name="PartNo" Type="String" />
                        <asp:Parameter Name="QtyPckd" Type="Int32" />
                    </InsertParameters>
                </asp:ObjectDataSource>--%>
            </asp:View>
            <asp:View ID="vwInstructions" runat="server">
                <%--  <asp:GridView ID="gvInstructions" runat="server" SkinID="StandardGrid">
                </asp:GridView>
                <asp:ObjectDataSource ID="odsInstructions" runat="server" 
                    DeleteMethod="DeletePKGLayoutInstruction" 
                    InsertMethod="InsertPKGLayoutInstruction" 
                    OldValuesParameterFormatString="original_{0}" 
                    SelectMethod="GetPKGLayoutInstruction" TypeName="PKGBLL" 
                    UpdateMethod="UpdatePKGLayoutInstruction">
                    <DeleteParameters>
                        <asp:Parameter Name="PKGID" Type="Int32" />
                        <asp:Parameter Name="SeqID" Type="String" />
                        <asp:Parameter Name="original_PKGID" Type="Int32" />
                        <asp:Parameter Name="original_SeqID" Type="String" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="original_PKGID" Type="Int32" />
                        <asp:Parameter Name="original_SeqID" Type="Int32" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                        <asp:Parameter Name="IID" Type="Int32" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="PKGID" QueryStringField="pPKGID" Type="Int32" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="PKGID" Type="Int32" />
                        <asp:Parameter Name="IID" Type="Int32" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                    </InsertParameters>
                </asp:ObjectDataSource>--%>
            </asp:View>
            <asp:View ID="vwPicture" runat="server">
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Content>
