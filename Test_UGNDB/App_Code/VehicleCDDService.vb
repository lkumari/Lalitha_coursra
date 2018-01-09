' ************************************************************************************************
' Name:		VehicleCDDService.vb
' Purpose:	This code is called by the VehicleCDDService.asmx used with AJAX CascadingDropDown controls. 
'           This control is used in the following modules:
'           * PackagingExpProj.aspx {Customer Info tab}
' Author:   LRey
' Date:     08/17/2011
' ************************************************************************************************
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Collections.Generic
Imports System.Collections
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
Public Class VehicleCDDService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetMakes(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        'Get Makes from Make_Maint Table
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim Make As String = (IIf(kv("Make") = Nothing, "", kv("Make"))).ToString.ToUpper

        Dim vAdapter As New MakesTableAdapters.MakeTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetMakes(Make)
            dValues.Add(New CascadingDropDownNameValue(row("ddMakeName").ToString(), row("MakeName").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetMakes

    <WebMethod()> _
Public Function GetModelMaint(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        'Get Model from Model_Maint Table
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim Make As String = (IIf(kv("Make") = Nothing, "", kv("Make"))).ToString.ToUpper

        Dim vAdapter As New ModelsTableAdapters.ModelTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetModels("", Make)
            dValues.Add(New CascadingDropDownNameValue(row("ddModelName").ToString(), row("ModelName").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetModelMaint

    <WebMethod()> _
Public Function GetMakesSearch(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        'Search for Make by using vProgram filters
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim Make As String = (IIf(kv("Make") = Nothing, "", kv("Make"))).ToString.ToUpper
        Dim Model As String = (IIf(kv("Model") = Nothing, "", kv("Model"))).ToString.ToUpper
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper
        Dim Platform As String = (IIf(kv("Platform") = Nothing, 0, kv("Platform"))).ToString.ToUpper
        Dim Program As String = (IIf(kv("Program") = Nothing, 0, kv("Program"))).ToString.ToUpper
        Dim APID As String = (IIf(kv("APID") = Nothing, 0, kv("APID"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.Make_SearchTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetMakeSearch(Make, Model, OEMMfg, Platform, Program, APID)
            dValues.Add(New CascadingDropDownNameValue(row("ddMake").ToString(), row("Make").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetMakesSearch

    <WebMethod()> _
Public Function GetModelSearch(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        'Search for Model by using vProgram filters
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim Make As String = (IIf(kv("Make") = Nothing, "", kv("Make"))).ToString.ToUpper
        Dim Model As String = (IIf(kv("Model") = Nothing, "", kv("Model"))).ToString.ToUpper
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper
        Dim Platform As String = (IIf(kv("Platform") = Nothing, 0, kv("Platform"))).ToString.ToUpper
        Dim Program As String = (IIf(kv("Program") = Nothing, 0, kv("Program"))).ToString.ToUpper
        Dim APID As String = (IIf(kv("APID") = Nothing, 0, kv("APID"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.Model_SearchTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetModelSearch(Make, Model, OEMMfg, Platform, Program, APID)
            dValues.Add(New CascadingDropDownNameValue(row("ddModel").ToString(), row("Model").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetModelSearch


    <WebMethod()> _
      Public Function GetOEMMfg(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim Make As String = (IIf(kv("Make") = Nothing, "", kv("Make"))).ToString.ToUpper
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.Platform_OEM_Mfg_by_MakeTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetPlatformOEMMfgByMake(Make)
            dValues.Add(New CascadingDropDownNameValue(row("ddOEMManufacturer").ToString(), row("OEMManufacturer").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetOEMMfg

    <WebMethod()> _
    Public Function GetOEM(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.OEM_by_OEM_MfgTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetOEMbyOEMMfg(OEMMfg)
            dValues.Add(New CascadingDropDownNameValue(row("ddOEMDesc").ToString(), row("OEM").ToString()))
        Next
        Return dValues.ToArray()
    End Function 'EOF GetOEM

    <WebMethod()> _
Public Function GetPrograms(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim Make As String = (IIf(kv("Make") = Nothing, "", kv("Make"))).ToString.ToUpper
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper
        Dim Platform As String = (IIf(kv("Platform") = Nothing, 0, kv("Platform"))).ToString.ToUpper
        Dim Model As String = (IIf(kv("Model") = Nothing, "", kv("Model"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.Program_by_MakeTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetProgramByMakes(Make, OEMMfg, Platform, Model)
            dValues.Add(New CascadingDropDownNameValue(row("ddProgramPlatformAssembly").ToString(), row("ProgramID").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetPrograms

    <WebMethod()> _
Public Function GetProgramsPlatformAssembly(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim Make As String = (IIf(kv("Make") = Nothing, "", kv("Make"))).ToString.ToUpper
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper
        Dim Platform As String = (IIf(kv("Platform") = Nothing, 0, kv("Platform"))).ToString.ToUpper
        Dim Model As String = (IIf(kv("Model") = Nothing, "", kv("Model"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.Program_by_MakeTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetProgramByMakes(Make, OEMMfg, Platform, Model)
            dValues.Add(New CascadingDropDownNameValue(row("ddProgramPlatformOEMMfgAssembly").ToString(), row("ProgramID").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetProgramsPlatformAssembly

    <WebMethod()> _
Public Function GetProgramsModelPlatformAssembly(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim Make As String = (IIf(kv("Make") = Nothing, "", kv("Make"))).ToString.ToUpper
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper
        Dim Platform As String = (IIf(kv("Platform") = Nothing, 0, kv("Platform"))).ToString.ToUpper
        Dim Model As String = (IIf(kv("Model") = Nothing, "", kv("Model"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.Program_by_MakeTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetProgramByMakes(Make, OEMMfg, Platform, Model)
            dValues.Add(New CascadingDropDownNameValue(row("ddProgramPlatformModelAssembly").ToString(), row("ProgramID").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetProgramsModelPlatformAssembly

    <WebMethod()> _
Public Function GetProgramsAssembly(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim Make As String = (IIf(kv("Make") = Nothing, "", kv("Make"))).ToString.ToUpper
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper
        Dim Platform As String = (IIf(kv("Platform") = Nothing, 0, kv("Platform"))).ToString.ToUpper
        Dim Model As String = (IIf(kv("Model") = Nothing, "", kv("Model"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.Program_by_MakeTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetProgramByMakes(Make, OEMMfg, Platform, Model)
            dValues.Add(New CascadingDropDownNameValue(row("ddProgramAssembly").ToString(), row("ProgramID").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetProgramsAssembly

    <WebMethod()> _
  Public Function GetCustomer(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        ' Dim OEM As String = (IIf(kv("OEM") = Nothing, "", kv("OEM"))).ToString.ToUpper
        Dim OEM As String = (IIf(kv("OEM") = Nothing, "", IIf(kv("OEM") = "99", "", kv("OEM")))).ToString.ToUpper
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.SoldTo_CABBV_by_OEM_MfgTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.GetOEMSoldToCABBVbyOEMMfg(OEM, OEMMfg)
            dValues.Add(New CascadingDropDownNameValue(row("ddCustomerDesc").ToString(), row("ddCustomerValue").ToString()))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetCustomer

    <WebMethod()> _
Public Function GetPartNos(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim OEM As String = (IIf(kv("OEM") = Nothing, "", IIf(kv("OEM") = "99", "", kv("OEM")))).ToString.ToUpper
        Dim OEMMfg As String = (IIf(kv("OEMMfg") = Nothing, "", kv("OEMMfg"))).ToString.ToUpper
        Dim UGNLocation As String = (IIf(kv("UGNLocation") = Nothing, "", kv("UGNLocation"))).ToString.ToUpper


        Dim vAdapter As New VehicleTableAdapters.PartNo_by_OEMTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        'dValues.Add(New CascadingDropDownNameValue("ALL PARTS", "ALL PARTS"))

        For Each row As DataRow In vAdapter.GetPartNoByOEMMfg(OEMMfg, UGNLocation)
            Dim ddPartNo As String = row("ddPartNo").ToString()
            Dim PartNo As String = row("PartNo").ToString()
            dValues.Add(New CascadingDropDownNameValue(row("ddPartNo").ToString(), row("PartNo").ToString()))
        Next
        Return dValues.ToArray()
    End Function 'EOF GetPartNos

    <WebMethod()> _
    Public Function GetVehicleType(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim VTID As String = (IIf(kv("VTID") = Nothing, 0, kv("VTID"))).ToString.ToUpper
        Dim VehicleType As String = (IIf(kv("VehicleType") = Nothing, "", kv("VehicleType"))).ToString.ToUpper

        Dim vAdapter As New VehicleTableAdapters.Vehicle_TypeTableAdapter
        Dim dValues As New List(Of CascadingDropDownNameValue)()

        For Each row As DataRow In vAdapter.Get_Vehicle_Type(VTID, VehicleType)
            dValues.Add(New CascadingDropDownNameValue(row("Vehicle_Type").ToString(), row("VTID")))
        Next

        Return dValues.ToArray()
    End Function 'EOF GetVehicleType


End Class
