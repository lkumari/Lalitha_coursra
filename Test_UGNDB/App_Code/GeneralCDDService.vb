' ************************************************************************************************
' Name:		GeneralCDDService.vb
' Purpose:	This code is called by the GeneralCDDService.asmx used with AJAX CascadingDropDown controls. 
'           This control is used in the following modules:
'           * 
' Author:   LRey
' Date:     08/17/2011
' Modified: 09/24/2012  LRey    Added GetPKGContainerByOEMMfg
' Modified: 04/01/2013  LRey    Added GetSampleTrialEvent
' ************************************************************************************************
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Collections.Generic
Imports AjaxControlToolkit
Imports System.Data
Imports System.Data.DataSet
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Collections.Specialized
Imports System.Web.Services.WebService
Imports System.Xml
Imports System.Web.Script.Services
Imports System.Configuration

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<System.Web.Script.Services.ScriptService()> _
Public Class GeneralCDDService
    Inherits System.Web.Services.WebService
    <WebMethod()> _
Public Function GetUGNLocationByTMFac(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim UGNDB_TMLoc As String = HttpContext.Current.Request.Cookies("UGNDB_TMLoc").Value
        If UGNLocation = "" And UGNDB_TMLoc <> "UT" Then
            UGNLocation = UGNDB_TMLoc
        ElseIf UGNLocation = "UT" And UGNDB_TMLoc = "UT" Then
            UGNLocation = ""
        End If

        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetUGNFacility(UGNLocation)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("UGNFacilityName").ToString(), row("UGNFacility").ToString()))
            Next
        End If

        Return dValues.ToArray()
    End Function 'EOF GetUGNLocationByTMFac

    <WebMethod()> _
Public Function GetUGNLocation(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetUGNFacility(UGNLocation)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("UGNFacilityName").ToString(), row("UGNFacility").ToString()))
            Next
        End If

        Return dValues.ToArray()
    End Function 'EOF GetUGNLocation

    <WebMethod()> _
Public Function GetDepartment(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper

        Dim UGNDB_TMLoc As String = HttpContext.Current.Request.Cookies("UGNDB_TMLoc").Value
        If UGNLocation = "" And UGNDB_TMLoc <> "UT" Then
            UGNLocation = UGNDB_TMLoc
        ElseIf UGNLocation = "UT" And UGNDB_TMLoc = "UT" Then
            UGNLocation = ""
        End If

        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetDepartmentLWK(UGNLocation)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("ddDepartmentName").ToString(), row("DeptNo").ToString()))
            Next
        End If

        Return dValues.ToArray()
    End Function 'EOF GetDepartment

    <WebMethod()> _
Public Function GetWorkCenter(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper
        Dim Department As String = (IIf(kv("Department") = Nothing, 0, kv("Department"))).ToString.ToUpper

        Dim UGNDB_TMLoc As String = HttpContext.Current.Request.Cookies("UGNDB_TMLoc").Value
        If UGNLocation = "" And UGNDB_TMLoc <> "UT" Then
            UGNLocation = UGNDB_TMLoc
        ElseIf UGNLocation = "UT" And UGNDB_TMLoc = "UT" Then
            UGNLocation = ""
        End If

        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetDepartmentWorkCenter(UGNLocation, Department)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("ddWorkCenterName").ToString(), row("WorkCenterNo").ToString()))
            Next
        End If

        Return dValues.ToArray()
    End Function 'EOF GetWorkCenter

    <WebMethod()> _
Public Function GetDepartmentGLNO(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper

        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetDepartmentGLNo(UGNLocation)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("ddDepartmentName").ToString(), row("GLNO").ToString()))
            Next
        End If

        Return dValues.ToArray()
    End Function 'EOF GetDepartmentGLNO

    '(LREY) 01/07/2014
    <WebMethod()> _
Public Function GetOEMbyCOMPNY(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper

        Dim UGNDB_TMLoc As String = HttpContext.Current.Request.Cookies("UGNDB_TMLoc").Value
        If UGNLocation = "" And UGNDB_TMLoc <> "UT" Then
            UGNLocation = UGNDB_TMLoc
        ElseIf UGNLocation = "UT" And UGNDB_TMLoc = "UT" Then
            UGNLocation = ""
        End If

        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetOEMbyCOMPNY(UGNLocation)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("ddOEM").ToString(), row("OEM").ToString()))
            Next
        End If

        Return dValues.ToArray()
    End Function 'EOF GetOEMbyCOMPNY

    <WebMethod()> _
Public Function GetOEMMfgByOEM(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        'Dim OEM As String = (IIf(kv("OEM") = Nothing, "", kv("OEM"))).ToString.ToUpper
        Dim OEM As String = (IIf(kv("OEM") = Nothing, "", IIf(kv("OEM") = "99", "", kv("OEM")))).ToString.ToUpper

        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetOEMMfgByOEM(OEM)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("ddOEMDesc").ToString(), row("OEMManufacturer").ToString()))
            Next
        End If

        Return dValues.ToArray()
    End Function 'EOF GetOEMMfgByOEM

    '(LREY) 01/07/2014
    <WebMethod()> _
Public Function GetCABBVbyOEM(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper
        Dim OEM As String = (IIf(kv("OEM") = Nothing, "", IIf(kv("OEM") = "99", "", kv("OEM")))).ToString.ToUpper

        Dim UGNDB_TMLoc As String = HttpContext.Current.Request.Cookies("UGNDB_TMLoc").Value
        If UGNLocation = "" And UGNDB_TMLoc <> "UT" Then
            UGNLocation = UGNDB_TMLoc
        ElseIf UGNLocation = "UT" And UGNDB_TMLoc = "UT" Then
            UGNLocation = ""
        End If

        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetCABBVbyOEM(UGNLocation, OEM)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("CABBV_OEM").ToString(), row("CABBV").ToString()))
            Next
        End If

        Return dValues.ToArray()
    End Function 'EOF GetCABBVbyOEM

    '(LREY) 01/07/2014
    <WebMethod()> _
Public Function GetCABBVbyOEMMfg(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper

        Dim UGNDB_TMLoc As String = HttpContext.Current.Request.Cookies("UGNDB_TMLoc").Value
        If UGNLocation = "" And UGNDB_TMLoc <> "UT" Then
            UGNLocation = UGNDB_TMLoc
        ElseIf UGNLocation = "UT" And UGNDB_TMLoc = "UT" Then
            UGNLocation = ""
        End If

        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetCABBVbyOEMMfg(UGNLocation, OEMMfg)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("ddCustomerDesc").ToString(), row("ddCustomerValue").ToString()))
            Next
        End If

        Return dValues.ToArray()
    End Function 'EOF GetCABBVbyOEMMfg

    <WebMethod()> _
Public Function GetOEMMfgCABBV(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper

        'Dim UGNDB_TMLoc As String = HttpContext.Current.Request.Cookies("UGNDB_TMLoc").Value
        'If UGNLocation = "" And UGNDB_TMLoc <> "UT" Then
        '    UGNLocation = UGNDB_TMLoc
        'ElseIf UGNLocation = "UT" And UGNDB_TMLoc = "UT" Then
        '    UGNLocation = ""
        'End If

        Dim vAdapter As New VehicleTableAdapters.OEMMfg_CABBVTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetOEMMfgCABBV(UGNLocation)
            Dim ddCustomerDesc As String = row("ddOEMMfg_CABBV").ToString()
            Dim ddCustomerValue As String = row("OEMMfg_CABBV").ToString()
            dValues.Add(New CascadingDropDownNameValue(row("ddOEMMfg_CABBV").ToString(), row("OEMMfg_CABBV").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetOEMMfgCABBV

    '(LREY) 01/07/2014
    <WebMethod()> _
Public Function GetDABBVbyCABBV(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim CABBV As String = (IIf(kv("CABBV") = Nothing, "", kv("CABBV"))).ToString.ToUpper

        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetCustomerDestination(CABBV)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("DABBV").ToString(), row("DABBV").ToString()))
            Next
        End If

        Return dValues.ToArray()
    End Function 'EOF GetDABBVbyCABBV

    '(LREY) 01/07/2014
    <WebMethod()> _
Public Function GetSOLDTObyCOMPNYbyCABBVbyOEM(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper
        Dim OEM As String = (IIf(kv("OEM") = Nothing, "", IIf(kv("OEM") = "99", "", kv("OEM")))).ToString.ToUpper
        Dim CABBV As String = (IIf(kv("CABBV") = Nothing, "", kv("CABBV"))).ToString.ToUpper
        Dim SoldTo As String = (IIf(kv("SoldTo") = Nothing, "", kv("SoldTo"))).ToString.ToUpper

        Dim UGNDB_TMLoc As String = HttpContext.Current.Request.Cookies("UGNDB_TMLoc").Value
        If UGNLocation = "" And UGNDB_TMLoc <> "UT" Then
            UGNLocation = UGNDB_TMLoc
        ElseIf UGNLocation = "UT" And UGNDB_TMLoc = "UT" Then
            UGNLocation = ""
        End If

        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim ds As DataSet = New DataSet
        ds = commonFunctions.GetSOLDTObyCOMPNYbyCABBVbyOEM(UGNLocation, OEM, CABBV)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("ddSoldTo").ToString(), row("SoldTo").ToString()))
            Next
        End If

        Return dValues.ToArray()
    End Function 'EOF GetSOLDTObyCOMPNYbyCABBVbyOEM

    <WebMethod()> _
Public Function GetOEMSoldToCABBVbyOEMMfg(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim OEM As String = (IIf(kv("OEM") = Nothing, "", IIf(kv("OEM") = "99", "", kv("OEM")))).ToString.ToUpper
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.SoldTo_CABBV_by_OEM_MfgTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetOEMSoldToCABBVbyOEMMfg(OEM, OEMMfg)
            Dim ddCustomerDesc As String = row("ddCustomerDesc").ToString()
            Dim ddCustomerValue As String = row("ddCustomerValue").ToString()
            dValues.Add(New CascadingDropDownNameValue(row("ddCustomerDesc").ToString(), row("ddCustomerValue").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetOEMSoldToCABBVbyOEMMfg

    <WebMethod()> _
Public Function GetDesignationTypes(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.Designation_TypeTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetDesignationType
            dValues.Add(New CascadingDropDownNameValue(row("ddDesignationTypeName").ToString(), row("DesignationType").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetDesignationTypes

    <WebMethod()> _
Public Function GetCommodityClass(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim CommodityClassID As String = (IIf(kv("CommodityClassID") = Nothing, 0, kv("CommodityClassID"))).ToString.ToUpper

        Dim vAdapter As New CommoditiesTableAdapters.Commodity_ClassTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.Get_Commodity_Class(CommodityClassID, "")
            dValues.Add(New CascadingDropDownNameValue(row("ddCommodityClassification").ToString(), row("CCID").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetCommodityClass

    <WebMethod()> _
Public Function GetCommodityByClass(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim CommodityClassID As String = (IIf(kv("CommodityClassID") = Nothing, 0, kv("CommodityClassID"))).ToString.ToUpper
        Dim CommodityID As String = (IIf(kv("CommodityID") = Nothing, 0, kv("CommodityID"))).ToString.ToUpper

        Dim vAdapter As New CommoditiesTableAdapters.CommodityTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetCommodities(CommodityID, "", "", CommodityClassID)
            dValues.Add(New CascadingDropDownNameValue(row("ddCommodityName").ToString(), row("CommodityID").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetCommodityByClass

    <WebMethod()> _
Public Function GetCommodityByClassWithClassName(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim CommodityClassID As String = (IIf(kv("CommodityClassID") = Nothing, 0, kv("CommodityClassID"))).ToString.ToUpper
        Dim CommodityID As String = (IIf(kv("CommodityID") = Nothing, 0, kv("CommodityID"))).ToString.ToUpper

        Dim vAdapter As New CommoditiesTableAdapters.CommodityTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetCommodities(CommodityID, "", "", CommodityClassID)
            dValues.Add(New CascadingDropDownNameValue(row("ddCommodityByClassification").ToString(), row("CommodityID").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetCommodityByClassWithClassName


    <WebMethod()> _
Public Function GetPKGContainerByOEMMfg(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        'Search for Make by using vProgram filters
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.PKG_Container_by_OEMMfgTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GETPKGContainerByOEMMfg(OEMMfg)
            dValues.Add(New CascadingDropDownNameValue(row("ContainerNo").ToString(), row("CID").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetPKGContainerByOEMMfg

    <WebMethod()> _
Public Function GetSampleTrialEvent(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim Customer As String = (IIf(kv("Customer") = Nothing, "", kv("Customer"))).ToString.ToUpper
        Dim Pos As Integer = InStr(Customer, " / ")
        Dim OEMMfg As String = ""
        If Not (Pos = 0) Then
            OEMMfg = Microsoft.VisualBasic.Left(Customer, Pos - 1)
        End If

        Dim dValues As New List(Of CascadingDropDownNameValue)()

        Dim ds As DataSet = New DataSet
        ds = PGMModule.GetSampleTrialEvent("", OEMMfg)

        If (ds.Tables.Item(0).Rows.Count > 0) Then
            For Each row As DataRow In ds.Tables.Item(0).Rows
                dValues.Add(New CascadingDropDownNameValue(row("ddTrialEvent").ToString(), row("TEID").ToString()))
            Next
        Else
            dValues.Add(New CascadingDropDownNameValue("Other", 1))
        End If

        Return dValues.ToArray()
    End Function 'EOF GetSampleTrialEvent
End Class
