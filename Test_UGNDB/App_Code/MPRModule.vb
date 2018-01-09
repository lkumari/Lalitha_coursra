''************************************************************************************************
''Name:		MPR.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or getting user-rights for the Production menu
''
''Date		    Author	    
''09/28/2011    LRey			Created .Net application
''05/07/2012    LRey            Added a Region for MFG Capacity Process
''05/16/2012    LRey            Added DeleteChartSpecFrmTmpltCookies
''07/10/2012    LRey            Added DeletePlannerCodeCookies
''07/26/2012    LRey            Adeded StoreMonthEndValuesCCM
''************************************************************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Text
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.Web.UI.WebControls
Imports Microsoft.VisualBasic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class MPRModule
    Inherits System.ComponentModel.Component
#Region "Chart Spec"
    Public Shared Sub DeleteChartSpecCookies()
        ''***
        '' Used to clear out cookies in the Vehicle Volume Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("MPR_FAC").Value = ""
            HttpContext.Current.Response.Cookies("MPR_FAC").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("MPR_OMFG").Value = ""
            HttpContext.Current.Response.Cookies("MPR_OMFG").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("MPR_CUST").Value = ""
            HttpContext.Current.Response.Cookies("MPR_CUST").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("MPR_PNO").Value = ""
            HttpContext.Current.Response.Cookies("MPR_PNO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("MPR_DEPT").Value = ""
            HttpContext.Current.Response.Cookies("MPR_DEPT").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("MPR_WC").Value = ""
            HttpContext.Current.Response.Cookies("MPR_WC").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("MPR_FORMULA").Value = ""
            HttpContext.Current.Response.Cookies("MPR_FORMULA").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("MPR_RECSTATUS").Value = ""
            HttpContext.Current.Response.Cookies("MPR_RECSTATUS").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteChartSpecCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/ChartSpecList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteChartSpecCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF DeletePFCookies_VehicleVolume

    Public Shared Sub DeleteChartSpecFrmTmpltCookies()
        ''***
        '' Used to clear out cookies in the Vehicle Volume Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("MPRT_FORMULA").Value = ""
            HttpContext.Current.Response.Cookies("MPRT_FORMULA").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteChartSpecFrmTmpltCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/ChartSpecFrmTmplt.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteChartSpecFrmTmpltCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF DeleteChartSpecFrmTmpltCookies

    Public Shared Function GetChartSpec(ByVal CSID As Integer, ByVal UGNFacility As String, ByVal OEMManufacturer As String, ByVal CustLoc As String, ByVal DesignationType As String, ByVal PartNo As String, ByVal DeptNo As Integer, ByVal WorkCenter As Integer, ByVal Formula As String, ByVal Obsolete As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Chart_Spec"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CSID", SqlDbType.Int)
            myCommand.Parameters("@CSID").Value = CSID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myCommand.Parameters.Add("@CustLoc", SqlDbType.VarChar)
            myCommand.Parameters("@CustLoc").Value = CustLoc

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@DeptNo", SqlDbType.Int)
            myCommand.Parameters("@DeptNo").Value = DeptNo

            myCommand.Parameters.Add("@WorkCenter", SqlDbType.Int)
            myCommand.Parameters("@WorkCenter").Value = WorkCenter

            myCommand.Parameters.Add("@Formula", SqlDbType.VarChar)
            myCommand.Parameters("@Formula").Value = Formula

            myCommand.Parameters.Add("@Obsolete", SqlDbType.Bit)
            myCommand.Parameters("@Obsolete").Value = Obsolete

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ChartSpec")

            GetChartSpec = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", CSID: " & CSID
            HttpContext.Current.Session("BLLerror") = "GetChartSpec : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetChartSpec") = "~/MfgProd/ChartSpec.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChartSpec : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetChartSpec = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetChartSpec

    Public Shared Function GetChartSpecFrmTmplt(ByVal RowID As Integer, ByVal FormulaID As Integer, ByVal FormulaName As String, ByVal Obsolete As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Chart_Spec_FrmTmplt"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@FormulaID", SqlDbType.Int)
            myCommand.Parameters("@FormulaID").Value = FormulaID

            myCommand.Parameters.Add("@FormulaName", SqlDbType.VarChar)
            myCommand.Parameters("@FormulaName").Value = FormulaName

            myCommand.Parameters.Add("@Obsolete", SqlDbType.Bit)
            myCommand.Parameters("@Obsolete").Value = Obsolete

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ChartSpecFrmTmplt")

            GetChartSpecFrmTmplt = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetChartSpecFrmTmplt : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetChartSpecFrmTmplt") = "~/MfgProd/ChartSpec.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChartSpecFrmTmplt : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetChartSpecFrmTmplt = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetChartSpecFrmTmplt

    Public Shared Function GetChartSpecFormula(ByVal FormulaID As Integer, ByVal FormulaName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Chart_Spec_Formula"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@FormulaID", SqlDbType.Int)
            myCommand.Parameters("@FormulaID").Value = FormulaID

            myCommand.Parameters.Add("@FormulaName", SqlDbType.VarChar)
            myCommand.Parameters("@FormulaName").Value = FormulaName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ChartSpecFormula")

            GetChartSpecFormula = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetChartSpecFormula : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetChartSpecFormula") = "~/MfgProd/ChartSpec.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChartSpecFormula : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetChartSpecFormula = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetChartSpecFormula

    Public Shared Sub InsertChartSpec(ByVal UGNFacility As String, ByVal OEM As String, ByVal OEMManufacturer As String, ByVal CustLoc As String, ByVal DesignationType As String, ByVal CommodityID As Integer, ByVal PartNo As String, ByVal KitPartNo As String, ByVal FamilyPartNo As String, ByVal WorkCenter As Integer, ByVal ThicknessRangeFrom As Decimal, ByVal ThicknessRangeTo As Decimal, ByVal TargetThickness As Decimal, ByVal Width As Decimal, ByVal Formula As String, ByVal ContainerDescription As String, ByVal ContainerDimensions As String, ByVal SPQ As Integer, ByVal PcsPerHour As Integer, ByVal PcsPerCycle As Integer, ByVal SagPanelSize As Integer, ByVal SagPanelUID As Integer, ByVal Travel As Integer, ByVal CallUpNo As Integer, ByVal LineSpeed As Integer, ByVal PressCycles As Integer, ByVal StandardTime As Decimal, ByVal Quantity As Integer, ByVal Notes As String, ByVal Obsolete As Boolean, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Chart_Spec"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myCommand.Parameters.Add("@CustLoc", SqlDbType.VarChar)
            myCommand.Parameters("@CustLoc").Value = CustLoc

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@KitPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@KitPartNo").Value = KitPartNo

            myCommand.Parameters.Add("@FamilyPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@FamilyPartNo").Value = FamilyPartNo

            myCommand.Parameters.Add("@WorkCenter", SqlDbType.Int)
            myCommand.Parameters("@WorkCenter").Value = WorkCenter

            myCommand.Parameters.Add("@ThicknessRangeFrom", SqlDbType.Decimal)
            myCommand.Parameters("@ThicknessRangeFrom").Value = ThicknessRangeFrom

            myCommand.Parameters.Add("@ThicknessRangeTo", SqlDbType.Decimal)
            myCommand.Parameters("@ThicknessRangeTo").Value = ThicknessRangeTo

            myCommand.Parameters.Add("@TargetThickness", SqlDbType.Decimal)
            myCommand.Parameters("@TargetThickness").Value = TargetThickness

            myCommand.Parameters.Add("@Width", SqlDbType.Decimal)
            myCommand.Parameters("@Width").Value = Width

            myCommand.Parameters.Add("@Formula", SqlDbType.VarChar)
            myCommand.Parameters("@Formula").Value = Formula

            myCommand.Parameters.Add("@ContainerDescription", SqlDbType.VarChar)
            myCommand.Parameters("@ContainerDescription").Value = commonFunctions.convertSpecialChar(ContainerDescription, False)

            myCommand.Parameters.Add("@ContainerDimensions", SqlDbType.VarChar)
            myCommand.Parameters("@ContainerDimensions").Value = commonFunctions.convertSpecialChar(ContainerDimensions, False)

            myCommand.Parameters.Add("@SPQ", SqlDbType.Int)
            myCommand.Parameters("@SPQ").Value = SPQ

            myCommand.Parameters.Add("@PcsPerHour", SqlDbType.Int)
            myCommand.Parameters("@PcsPerHour").Value = PcsPerHour

            myCommand.Parameters.Add("@PcsPerCycle", SqlDbType.Int)
            myCommand.Parameters("@PcsPerCycle").Value = PcsPerCycle

            myCommand.Parameters.Add("@SagPanelSize", SqlDbType.Int)
            myCommand.Parameters("@SagPanelSize").Value = SagPanelSize

            myCommand.Parameters.Add("@SagPanelUID", SqlDbType.Int)
            myCommand.Parameters("@SagPanelUID").Value = SagPanelUID

            myCommand.Parameters.Add("@Travel", SqlDbType.Int)
            myCommand.Parameters("@Travel").Value = Travel

            myCommand.Parameters.Add("@CallUpNo", SqlDbType.Int)
            myCommand.Parameters("@CallUpNo").Value = CallUpNo

            myCommand.Parameters.Add("@LineSpeed", SqlDbType.Int)
            myCommand.Parameters("@LineSpeed").Value = LineSpeed

            myCommand.Parameters.Add("@PressCycles", SqlDbType.Int)
            myCommand.Parameters("@PressCycles").Value = PressCycles

            myCommand.Parameters.Add("@StandardTime", SqlDbType.Decimal)
            myCommand.Parameters("@StandardTime").Value = StandardTime

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.convertSpecialChar(Notes, False)

            myCommand.Parameters.Add("@Obsolete", SqlDbType.Bit)
            myCommand.Parameters("@Obsolete").Value = Obsolete

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertChartSpec : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertChartSpec") = "~/MfgProd/ChartSpec.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertChartSpec : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertChartSpec

    Public Shared Sub UpdateChartSpec(ByVal CSID As Integer, ByVal UGNFacility As String, ByVal OEM As String, ByVal OEMManufacturer As String, ByVal CustLoc As String, ByVal DesignationType As String, ByVal CommodityID As String, ByVal PartNo As String, ByVal KitPartNo As String, ByVal FamilyPartNo As String, ByVal WorkCenter As Integer, ByVal ThicknessRangeFrom As Decimal, ByVal ThicknessRangeTo As Decimal, ByVal TargetThickness As Decimal, ByVal Width As Decimal, ByVal Formula As String, ByVal ContainerDescription As String, ByVal ContainerDimensions As String, ByVal SPQ As Integer, ByVal PcsPerHour As Integer, ByVal PcsPerCycle As Integer, ByVal SagPanelSize As Integer, ByVal SagPanelUID As String, ByVal Travel As Integer, ByVal CallUpNo As Integer, ByVal LineSpeed As Integer, ByVal PressCycles As Integer, ByVal StandardTime As Decimal, ByVal Quantity As Integer, ByVal Notes As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Chart_Spec"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CSID", SqlDbType.Int)
            myCommand.Parameters("@CSID").Value = CSID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myCommand.Parameters.Add("@CustLoc", SqlDbType.VarChar)
            myCommand.Parameters("@CustLoc").Value = CustLoc

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@KitPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@KitPartNo").Value = KitPartNo

            myCommand.Parameters.Add("@FamilyPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@FamilyPartNo").Value = FamilyPartNo

            myCommand.Parameters.Add("@WorkCenter", SqlDbType.Int)
            myCommand.Parameters("@WorkCenter").Value = WorkCenter

            myCommand.Parameters.Add("@ThicknessRangeFrom", SqlDbType.Decimal)
            myCommand.Parameters("@ThicknessRangeFrom").Value = ThicknessRangeFrom

            myCommand.Parameters.Add("@ThicknessRangeTo", SqlDbType.Decimal)
            myCommand.Parameters("@ThicknessRangeTo").Value = ThicknessRangeTo

            myCommand.Parameters.Add("@TargetThickness", SqlDbType.Decimal)
            myCommand.Parameters("@TargetThickness").Value = TargetThickness

            myCommand.Parameters.Add("@Width", SqlDbType.Decimal)
            myCommand.Parameters("@Width").Value = Width

            myCommand.Parameters.Add("@Formula", SqlDbType.VarChar)
            myCommand.Parameters("@Formula").Value = Formula

            myCommand.Parameters.Add("@ContainerDescription", SqlDbType.VarChar)
            myCommand.Parameters("@ContainerDescription").Value = commonFunctions.convertSpecialChar(ContainerDescription, False)

            myCommand.Parameters.Add("@ContainerDimensions", SqlDbType.VarChar)
            myCommand.Parameters("@ContainerDimensions").Value = commonFunctions.convertSpecialChar(ContainerDimensions, False)

            myCommand.Parameters.Add("@SPQ", SqlDbType.Int)
            myCommand.Parameters("@SPQ").Value = SPQ

            myCommand.Parameters.Add("@PcsPerHour", SqlDbType.Int)
            myCommand.Parameters("@PcsPerHour").Value = PcsPerHour

            myCommand.Parameters.Add("@PcsPerCycle", SqlDbType.Int)
            myCommand.Parameters("@PcsPerCycle").Value = PcsPerCycle

            myCommand.Parameters.Add("@SagPanelSize", SqlDbType.Int)
            myCommand.Parameters("@SagPanelSize").Value = SagPanelSize

            myCommand.Parameters.Add("@SagPanelUID", SqlDbType.Int)
            myCommand.Parameters("@SagPanelUID").Value = SagPanelUID

            myCommand.Parameters.Add("@Travel", SqlDbType.Int)
            myCommand.Parameters("@Travel").Value = Travel

            myCommand.Parameters.Add("@CallUpNo", SqlDbType.Int)
            myCommand.Parameters("@CallUpNo").Value = CallUpNo

            myCommand.Parameters.Add("@LineSpeed", SqlDbType.Int)
            myCommand.Parameters("@LineSpeed").Value = LineSpeed

            myCommand.Parameters.Add("@PressCycles", SqlDbType.Int)
            myCommand.Parameters("@PressCycles").Value = PressCycles

            myCommand.Parameters.Add("@StandardTime", SqlDbType.Decimal)
            myCommand.Parameters("@StandardTime").Value = StandardTime

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.convertSpecialChar(Notes, False)

            myCommand.Parameters.Add("@Obsolete", SqlDbType.Bit)
            myCommand.Parameters("@Obsolete").Value = Obsolete

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CSID: " & CSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateChartSpec : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateChartSpec") = "~/MfgProd/ChartSpec.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateChartSpec : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateChartSpec
#End Region

#Region "MFG Capacity Process"
    Public Shared Sub DeleteMFGCapacityProcessCookies()

        Try
            HttpContext.Current.Response.Cookies("MFG_CPN").Value = ""
            HttpContext.Current.Response.Cookies("MFG_CPN").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteMFGCapacityProcessCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteMFGCapacityProcessCookies : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Sub  'EOF DeleteMFGCapacityProcessCookies

    Public Shared Function GetMFGCapacityProcess(ByVal PID As Integer, ByVal MFGProcessName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_MFG_Capacity_Process"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PID", SqlDbType.Int)
            myCommand.Parameters("@PID").Value = PID

            myCommand.Parameters.Add("@MFGProcessName", SqlDbType.VarChar)
            myCommand.Parameters("@MFGProcessName").Value = MFGProcessName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "MFGCapacityProcess")

            GetMFGCapacityProcess = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", PID: " & PID & ", MFGProcessName: " & MFGProcessName
            HttpContext.Current.Session("BLLerror") = "GetMFGCapacityProcess : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetMFGCapacityProcess") = "~/MfgProd/MFGCapacityProcessMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMFGCapacityProcess : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetMFGCapacityProcess = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetMFGCapacityProcess

    Public Shared Function GetMFGCapacityProcessWC(ByVal PID As Integer, ByVal WorkCenter As Integer, ByVal UGNFacility As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_MFG_Capacity_Process_WC"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PID", SqlDbType.Int)
            myCommand.Parameters("@PID").Value = PID

            myCommand.Parameters.Add("@WorkCenter", SqlDbType.Int)
            myCommand.Parameters("@WorkCenter").Value = WorkCenter

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "MFGCapacityProcessWC")

            GetMFGCapacityProcessWC = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", PID: " & PID & ", WorkCenter: " & WorkCenter & ", UGNFacility: " & UGNFacility
            HttpContext.Current.Session("BLLerror") = "GetMFGCapacityProcessWC : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetMFGCapacityProcessWC") = "~/MfgProd/MFGCapacityProcessWCMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMFGCapacityProcessWC : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetMFGCapacityProcessWC = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetMFGCapacityProcessWC

#End Region

#Region "Cycle Counter Matrix"
    Public Shared Sub DeletePlannerCodeCookies()
        ''***
        '' Used to clear out cookies in the Vehicle Volume Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("CCMPC_FAC").Value = ""
            HttpContext.Current.Response.Cookies("CCMPC_FAC").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePlannerCodeCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CCM/PlannerCodeMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("DeletePlannerCodeCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF DeletePlannerCodeCookies

    Public Shared Function GetProductionDays(ByVal FromDate As String) As DataSet


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Production_Days"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@Date", SqlDbType.DateTime)
            myCommand.Parameters("@Date").Value = FromDate

            GetProductionDays = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", Date: " & FromDate
            HttpContext.Current.Session("BLLerror") = "GetProductionDays : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetProductionDays") = "~/CCM/CycleCounterMatrix.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProductionDays : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetProductionDays = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetProductionDays

    Public Shared Sub StoreMonthEndValuesCCM(ByVal UGNFacility As String, ByVal FromDate As String, ByVal ToDate As String, ByVal CUBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cycle_Count_Matrix_Store_MEV"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@FromDate", SqlDbType.VarChar)
            myCommand.Parameters("@FromDate").Value = FromDate

            myCommand.Parameters.Add("@ToDate", SqlDbType.VarChar)
            myCommand.Parameters("@ToDate").Value = ToDate

            myCommand.Parameters.Add("@CUBy", SqlDbType.VarChar)
            myCommand.Parameters("@CUBy").Value = CUBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "StoreMonthEndValuesCCM : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MPRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("StoreMonthEndValuesCCM") = "~/CCM/CycleCounterMatrix.aspx"
            UGNErrorTrapping.InsertErrorLog("StoreMonthEndValuesCCM : " & commonFunctions.convertSpecialChar(ex.Message, False), "MPRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF StoreMonthEndValuesCCM
#End Region
End Class
