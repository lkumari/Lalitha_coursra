<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="EXP_Default"
    Title="Untitled Page" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">

        <script runat="server">
            <System.Web.Services.WebMethod()> _
            <System.Web.Script.Services.ScriptMethod()> _
            Public Shared Function GetHtml(ByVal contextKey As String) As String
                System.Threading.Thread.Sleep(500)
                Dim value As String = ""
                If (contextKey = "U") Then
                    value = DateTime.UtcNow.ToString()
                Else
                    value = String.Format("{0:" + contextKey + "}", DateTime.Now)
                End If
                Return String.Format("<span style='font-family:courier new;font-weight:bold;'>{0}</span>", value)
            End Function
        </script>

        <script type="text/javascript">  
        function UpdateControl(value) {    
        var behavior = $find('dp1');    
        if (behavior)  {          
        behavior.populate(value);         
         }   
          }    
        </script>

        <table>
            <tr>
                <td colspan="2" class="c_Text" style="font-weight: bold">
                    Program Selection:
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label28" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    OEM Manufacturer:
                </td>
                <td style="font-size: smaller">
                    <asp:DropDownList ID="ddOEMMfg" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvOEMMfg" runat="server" ControlToValidate="ddOEMMfg"
                        ErrorMessage="OEM Manufacturer is a required field." Font-Bold="False" ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label29" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    Make:
                </td>
                <td style="font-size: smaller">
                    <asp:DropDownList ID="ddMakes" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvMake" runat="server" ControlToValidate="ddMakes"
                        ErrorMessage="Make is a required field." Font-Bold="False" ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label25" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    Model:
                </td>
                <td style="font-size: smaller">
                    <asp:DropDownList ID="ddModel" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="ddModel"
                        ErrorMessage="Model is a required field." Font-Bold="False" ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <%--  <tr>
                <td class="p_text">
                    <asp:Label ID="Label27" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    Platform:
                </td>
                <td style="font-size: smaller">
                    <asp:DropDownList ID="ddPlatform" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvPlatform" runat="server" ControlToValidate="ddPlatform"
                        ErrorMessage="Platform is a required field." Font-Bold="False" ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                    {Platform / OEM Mfg.}
                </td>
            </tr>--%>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    Program:
                </td>
                <td style="font-size: smaller">
                    <asp:DropDownList ID="ddProgram" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                        ErrorMessage="Program is a required field." Font-Bold="False" ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                    {Program / Platform / Assembly Plant}
                </td>
            </tr>
                       <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="c_Text" style="font-weight: bold">
                    Customer Selection:
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label32" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    OEM Code:
                </td>
                <td style="font-size: smaller">
                    <asp:DropDownList ID="ddOEM" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvOEM" runat="server" ControlToValidate="ddOEM"
                        ErrorMessage="OEM Code is a required field." Font-Bold="False" ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                    {OEM Code/ OEM Description}
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    &nbsp;Customer:
                </td>
                <td style="font-size: smaller">
                    <asp:DropDownList ID="ddCustomer" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddCustomer"
                        ErrorMessage="Customer is a required field." Font-Bold="False" ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                    {Sold To / CABBV / Customer Name}
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="c_Text" style="font-weight: bold">
                    Part Selection:
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    &nbsp;Internal Part No:
                </td>
                <td>
                    <asp:DropDownList ID="ddPartNo" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ControlToValidate="ddPartNo"
                        ErrorMessage="Part No is a required field." Font-Bold="False" ValidationGroup="vsCustomer">&lt;</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    &nbsp;
                </td>
                <td style="height: 11px">
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vsCustomer" runat="server" Font-Size="X-Small" ShowMessageBox="True"
            ShowSummary="true" ValidationGroup="vsCustomer" />
        <ajax:CascadingDropDown ID="cddOEMMfg" runat="server" TargetControlID="ddOEMMfg"
            Category="OEMMfg" PromptText="Please select an OEM Manufacturer." LoadingText="[Loading OEM Manufacturer...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetOEMMfg" />
        <ajax:CascadingDropDown ID="cddMakes" runat="server" TargetControlID="ddMakes" Category="Make"
            ParentControlID="ddOEMMfg" PromptText="Please select a Make." LoadingText="[Loading Makes...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetMakesSearch" />
        <ajax:CascadingDropDown ID="cddModel" runat="server" TargetControlID="ddModel" ParentControlID="ddMakes"
            Category="Model" PromptText="Please select a Model." LoadingText="[Loading Models...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetModelMaint" />
        <%--        <ajax:CascadingDropDown ID="cddPlatform" runat="server" TargetControlID="ddPlatform"
            ParentControlID="ddModel" Category="Platform" PromptText="Please select a Platform."
            LoadingText="[Loading Platform...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetPlatform" />--%>
        <ajax:CascadingDropDown ID="cddProgram" runat="server" TargetControlID="ddProgram"
            ParentControlID="ddModel" Category="Program" PromptText="Please select a Program."
            LoadingText="[Loading Programs...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetProgramsPlatformAssembly" />
       
        <ajax:CascadingDropDown ID="cddOEM" runat="server" TargetControlID="ddOEM" ParentControlID="ddOEMMfg"
            Category="OEM" PromptText="Please select an OEM Code." LoadingText="[Loading OEM Code...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetOEM" />
        <ajax:CascadingDropDown ID="cddCustomer" runat="server" TargetControlID="ddCustomer"
            ParentControlID="ddOEM" Category="Customer" PromptText="Please select a Customer."
            LoadingText="[Loading Customer...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetCustomer" />
        <ajax:CascadingDropDown ID="cddPartNo" runat="server" TargetControlID="ddPartNo"
            Category="PartNo" ParentControlID="ddOEMMfg" PromptText="Please select a Part Number."
            LoadingText="[Loading Part Numbers...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetPartNos" />
        <asp:Button ID="Button1" runat="server" Text="Reset" />
   <%--     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label33" runat="server" Text="Label"></asp:Label>
                <br />
                <br />
                <asp:Button ID="Button1" runat="server" Text="Button" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <img src="../images/AJAX/loading.gif" />
                Loading, Please Wait...
            </ProgressTemplate>
        </asp:UpdateProgress>--%>
    </asp:Panel>
</asp:Content>
