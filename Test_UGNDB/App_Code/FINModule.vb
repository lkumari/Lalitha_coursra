''************************************************************************************************
''Name:		FINModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or functions for the Purchasing Module
''
''Date		    Author	    
''01/14/2011    LRey			Created .Net application
''07/15/2011    LRey            Added functions for OEM_Model_Conv
''08/29/2012    LRey            Added GetLastForecastExceptionRowID and modified UpdateForecast to use RowID
''                              so that the system does not time out during execution.
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

Public Class FINModule
#Region "Forecast Exception"
    Public Shared Function GetForecastException(ByVal RowID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnDataMart").ToString
        Dim strStoredProcName As String = "sp_Get_Forecast_Exception"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.VarChar)
            myCommand.Parameters("@RowID").Value = RowID


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Row")

            GetForecastException = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", RowID: " & RowID

            HttpContext.Current.Session("BLLerror") = "GetForecastException : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetForecastException") = "~/FIN/ForecastExceptionMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("GetForecastException : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetForecastException = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetForecastException

    Public Shared Function GetLastForecastExceptionRowID() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnDataMart").ToString
        Dim strStoredProcName As String = "sp_Get_Last_Forecast_Exception_RowID"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Row")

            GetLastForecastExceptionRowID = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetLastForecastExceptionRowID : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetLastForecastExceptionRowID") = "~/FIN/ForecastExceptionMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("GetLastForecastExceptionRowID : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLastForecastExceptionRowID = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetLastForecastExceptionRowID

    Public Shared Sub InsertForecastException(ByVal CompnyValidator As String, ByVal COMPNY As String, ByVal OEMValidator As String, ByVal OEM As String, ByVal CabbvValidator As String, ByVal CABBV As String, ByVal SoldToValidator As String, ByVal SOLDTO As String, ByVal PartNoValidator As String, ByVal PARTNO As String, ByVal DabbvValidator As String, ByVal DABBV As String, ByVal TRNTYP As String, ByVal REQTYP As String, ByVal REQFRQ As String, ByVal DayOfWeekID As Integer, ByVal WeekValidator As String, ByVal SWeekID As Integer, ByVal EWeekID As Integer, ByVal MonthValidator As String, ByVal SMonthID As Integer, ByVal EMonthID As Integer, ByRef YearValidator As String, ByVal SYearID As Integer, ByVal EYearID As Int16, ByVal ReplaceQTYRQ As Integer, ByVal Notes As String, ByVal WKNEFWOM As Boolean, ByVal WKEQFWOM As Boolean, ByVal RDTGTFDOM As Boolean, ByVal RDTlTFDOM As Boolean, ByVal CreatedBy As String, ByVal CreatedOn As String)


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnDataMart").ToString
        Dim strStoredProcName As String = "sp_Insert_Forecast_Exception"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CompnyValidator", SqlDbType.VarChar)
            myCommand.Parameters("@CompnyValidator").Value = CompnyValidator

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            myCommand.Parameters.Add("@OEMValidator", SqlDbType.VarChar)
            myCommand.Parameters("@OEMValidator").Value = OEMValidator

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@CabbvValidator", SqlDbType.VarChar)
            myCommand.Parameters("@CabbvValidator").Value = CabbvValidator

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldToValidator", SqlDbType.VarChar)
            myCommand.Parameters("@SoldToValidator").Value = SoldToValidator

            myCommand.Parameters.Add("@SOLDTO", SqlDbType.VarChar)
            myCommand.Parameters("@SOLDTO").Value = SOLDTO

            myCommand.Parameters.Add("@PartNoValidator", SqlDbType.VarChar)
            myCommand.Parameters("@PartNoValidator").Value = PartNoValidator

            myCommand.Parameters.Add("@PARTNO", SqlDbType.VarChar)
            myCommand.Parameters("@PARTNO").Value = PARTNO

            myCommand.Parameters.Add("@DabbvValidator", SqlDbType.VarChar)
            myCommand.Parameters("@DabbvValidator").Value = DabbvValidator

            myCommand.Parameters.Add("@DABBV", SqlDbType.VarChar)
            myCommand.Parameters("@DABBV").Value = DABBV

            myCommand.Parameters.Add("@TRNTYP", SqlDbType.VarChar)
            myCommand.Parameters("@TRNTYP").Value = TRNTYP

            myCommand.Parameters.Add("@REQTYP", SqlDbType.VarChar)
            myCommand.Parameters("@REQTYP").Value = REQTYP

            myCommand.Parameters.Add("@REQFRQ", SqlDbType.VarChar)
            myCommand.Parameters("@REQFRQ").Value = REQFRQ

            myCommand.Parameters.Add("@DayOfWeekID", SqlDbType.Int)
            myCommand.Parameters("@DayOfWeekID").Value = DayOfWeekID

            myCommand.Parameters.Add("@WeekValidator", SqlDbType.VarChar)
            myCommand.Parameters("@WeekValidator").Value = WeekValidator

            myCommand.Parameters.Add("@SWeekID", SqlDbType.Int)
            myCommand.Parameters("@SWeekID").Value = SWeekID

            myCommand.Parameters.Add("@EWeekID", SqlDbType.Int)
            myCommand.Parameters("@EWeekID").Value = EWeekID

            myCommand.Parameters.Add("@MonthValidator", SqlDbType.VarChar)
            myCommand.Parameters("@MonthValidator").Value = MonthValidator

            myCommand.Parameters.Add("@SMonthID", SqlDbType.Int)
            myCommand.Parameters("@SMonthID").Value = SMonthID

            myCommand.Parameters.Add("@EMonthID", SqlDbType.Int)
            myCommand.Parameters("@EMonthID").Value = EMonthID

            myCommand.Parameters.Add("@YearValidator", SqlDbType.VarChar)
            myCommand.Parameters("@YearValidator").Value = YearValidator

            myCommand.Parameters.Add("@SYearID", SqlDbType.Int)
            myCommand.Parameters("@SYearID").Value = SYearID

            myCommand.Parameters.Add("@EYearID", SqlDbType.Int)
            myCommand.Parameters("@EYearID").Value = EYearID

            myCommand.Parameters.Add("@ReplaceQTYRQ", SqlDbType.Int)
            myCommand.Parameters("@ReplaceQTYRQ").Value = ReplaceQTYRQ

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@WKNEFWOM", SqlDbType.Bit)
            myCommand.Parameters("@WKNEFWOM").Value = WKNEFWOM

            myCommand.Parameters.Add("@WKEQFWOM", SqlDbType.Bit)
            myCommand.Parameters("@WKEQFWOM").Value = WKEQFWOM

            myCommand.Parameters.Add("@RDTGTFDOM", SqlDbType.Bit)
            myCommand.Parameters("@RDTGTFDOM").Value = RDTGTFDOM

            myCommand.Parameters.Add("@RDTLTFDOM", SqlDbType.Bit)
            myCommand.Parameters("@RDTLTFDOM").Value = RDTlTFDOM

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", CreatedBy: " & CreatedBy & ", CreatedOn: " & CreatedOn

            HttpContext.Current.Session("BLLerror") = "InsertForecastException : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertForecastException") = "~/FIN/ForecastExceptionMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertForecastException : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertForecastException

    Public Shared Sub UpdateForecastException(ByVal RowID As Integer, ByVal CompnyValidator As String, ByVal COMPNY As String, ByVal OEMValidator As String, ByVal OEM As String, ByVal CabbvValidator As String, ByVal CABBV As String, ByVal SoldToValidator As String, ByVal SOLDTO As String, ByVal PartNoValidator As String, ByVal PARTNO As String, ByVal DabbvValidator As String, ByVal DABBV As String, ByVal TRNTYP As String, ByVal REQTYP As String, ByVal REQFRQ As String, ByVal DayOfWeekID As Integer, ByVal WeekValidator As String, ByVal SWeekID As Integer, ByVal EWeekID As Integer, ByVal MonthValidator As String, ByVal SMonthID As Integer, ByVal EMonthID As Integer, ByRef YearValidator As String, ByVal SYearID As Integer, ByVal EYearID As Int16, ByVal ReplaceQTYRQ As Integer, ByVal Notes As String, ByVal WKNEFWOM As Boolean, ByVal WKEQFWOM As Boolean, ByVal RDTGTFDOM As Boolean, ByVal RDTLTFDOM As Boolean, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnDataMart").ToString
        Dim strStoredProcName As String = "sp_Update_Forecast_Exception"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@CompnyValidator", SqlDbType.VarChar)
            myCommand.Parameters("@CompnyValidator").Value = CompnyValidator

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            myCommand.Parameters.Add("@OEMValidator", SqlDbType.VarChar)
            myCommand.Parameters("@OEMValidator").Value = OEMValidator

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@CabbvValidator", SqlDbType.VarChar)
            myCommand.Parameters("@CabbvValidator").Value = CabbvValidator

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldToValidator", SqlDbType.VarChar)
            myCommand.Parameters("@SoldToValidator").Value = SoldToValidator

            myCommand.Parameters.Add("@SOLDTO", SqlDbType.VarChar)
            myCommand.Parameters("@SOLDTO").Value = SOLDTO

            myCommand.Parameters.Add("@PartNoValidator", SqlDbType.VarChar)
            myCommand.Parameters("@PartNoValidator").Value = PartNoValidator

            myCommand.Parameters.Add("@PARTNO", SqlDbType.VarChar)
            myCommand.Parameters("@PARTNO").Value = PARTNO

            myCommand.Parameters.Add("@DabbvValidator", SqlDbType.VarChar)
            myCommand.Parameters("@DabbvValidator").Value = DabbvValidator

            myCommand.Parameters.Add("@DABBV", SqlDbType.VarChar)
            myCommand.Parameters("@DABBV").Value = DABBV

            myCommand.Parameters.Add("@TRNTYP", SqlDbType.VarChar)
            myCommand.Parameters("@TRNTYP").Value = TRNTYP

            myCommand.Parameters.Add("@REQTYP", SqlDbType.VarChar)
            myCommand.Parameters("@REQTYP").Value = REQTYP

            myCommand.Parameters.Add("@REQFRQ", SqlDbType.VarChar)
            myCommand.Parameters("@REQFRQ").Value = REQFRQ

            myCommand.Parameters.Add("@DayOfWeekID", SqlDbType.Int)
            myCommand.Parameters("@DayOfWeekID").Value = DayOfWeekID

            myCommand.Parameters.Add("@WeekValidator", SqlDbType.VarChar)
            myCommand.Parameters("@WeekValidator").Value = WeekValidator

            myCommand.Parameters.Add("@SWeekID", SqlDbType.Int)
            myCommand.Parameters("@SWeekID").Value = SWeekID

            myCommand.Parameters.Add("@EWeekID", SqlDbType.Int)
            myCommand.Parameters("@EWeekID").Value = EWeekID

            myCommand.Parameters.Add("@MonthValidator", SqlDbType.VarChar)
            myCommand.Parameters("@MonthValidator").Value = MonthValidator

            myCommand.Parameters.Add("@SMonthID", SqlDbType.Int)
            myCommand.Parameters("@SMonthID").Value = SMonthID

            myCommand.Parameters.Add("@EMonthID", SqlDbType.Int)
            myCommand.Parameters("@EMonthID").Value = EMonthID

            myCommand.Parameters.Add("@YearValidator", SqlDbType.VarChar)
            myCommand.Parameters("@YearValidator").Value = YearValidator

            myCommand.Parameters.Add("@SYearID", SqlDbType.Int)
            myCommand.Parameters("@SYearID").Value = SYearID

            myCommand.Parameters.Add("@EYearID", SqlDbType.Int)
            myCommand.Parameters("@EYearID").Value = EYearID

            myCommand.Parameters.Add("@ReplaceQTYRQ", SqlDbType.Int)
            myCommand.Parameters("@ReplaceQTYRQ").Value = ReplaceQTYRQ

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@WKNEFWOM", SqlDbType.Bit)
            myCommand.Parameters("@WKNEFWOM").Value = WKNEFWOM

            myCommand.Parameters.Add("@WKEQFWOM", SqlDbType.Bit)
            myCommand.Parameters("@WKEQFWOM").Value = WKEQFWOM

            myCommand.Parameters.Add("@RDTGTFDOM", SqlDbType.Bit)
            myCommand.Parameters("@RDTGTFDOM").Value = RDTGTFDOM

            myCommand.Parameters.Add("@RDTLTFDOM", SqlDbType.Bit)
            myCommand.Parameters("@RDTLTFDOM").Value = RDTLTFDOM

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", UpdatedBy: " & UpdatedBy & ", UpdatedOn: " & UpdatedOn

            HttpContext.Current.Session("BLLerror") = "UpdateForecastException : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdateForecastException") = "~/FIN/ForecastExceptionMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateForecastException : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateForecastException

    Public Shared Sub UpdateForecast(ByVal rowID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnDataMart").ToString
        Dim strStoredProcName As String = "sp_Update_Forecast_RowID"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = rowID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateForecast : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdateForecast") = "~/FIN/ForecastExceptionMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateForecast : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateForecast
#End Region

#Region "OEM Model Conversion"
    Public Shared Function GetOEMModelConv(ByVal RowID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnDataMart").ToString
        Dim strStoredProcName As String = "sp_Get_OEM_Model_Conv"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Row")

            GetOEMModelConv = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", RowID: " & RowID

            HttpContext.Current.Session("BLLerror") = "GetOEMModelConv : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetOEMModelConv") = "~/FIN/OEMModelConvMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("GetOEMModelConv : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetOEMModelConv = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetOEMModelConv

    Public Shared Function GetLastOEMModelConvRowID() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnDataMart").ToString
        Dim strStoredProcName As String = "sp_Get_Last_OEM_Model_Conv_RowID"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Row")

            GetLastOEMModelConvRowID = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetLastOEMModelConvRowID : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetLastOEMModelConvRowID") = "~/FIN/OEMModelConvMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("GetLastOEMModelConvRowID : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLastOEMModelConvRowID = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function ' EOF GetLastOEMModelConvRowID

    Public Shared Sub InsertOEMModelConv(ByVal OEMValidator As String, ByVal OEM As String, ByVal CabbvValidator As String, ByVal CABBV As String, ByVal SoldToValidator As String, ByVal SOLDTO As String, ByVal DabbvValidator As String, ByVal DABBV As String, ByVal CPART_LOC1 As Integer, ByVal CPART_LOC2 As Integer, ByVal MiscValue As String, ByVal Notes As String, ByVal PartField As String, ByVal SQLQuery As String, ByVal AltOEMMfg As String, ByVal PartField2 As String, ByVal PartSuffix_LOC1 As Integer, ByVal PartSuffix_LOC2 As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnDataMart").ToString
        Dim strStoredProcName As String = "sp_Insert_OEM_Model_Conv"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@OEMValidator", SqlDbType.VarChar)
            myCommand.Parameters("@OEMValidator").Value = OEMValidator

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@CabbvValidator", SqlDbType.VarChar)
            myCommand.Parameters("@CabbvValidator").Value = CabbvValidator

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldToValidator", SqlDbType.VarChar)
            myCommand.Parameters("@SoldToValidator").Value = SoldToValidator

            myCommand.Parameters.Add("@SOLDTO", SqlDbType.VarChar)
            myCommand.Parameters("@SOLDTO").Value = SOLDTO

            myCommand.Parameters.Add("@DabbvValidator", SqlDbType.VarChar)
            myCommand.Parameters("@DabbvValidator").Value = DabbvValidator

            myCommand.Parameters.Add("@DABBV", SqlDbType.VarChar)
            myCommand.Parameters("@DABBV").Value = DABBV

            myCommand.Parameters.Add("@CPART_LOC1", SqlDbType.Int)
            myCommand.Parameters("@CPART_LOC1").Value = CPART_LOC1

            myCommand.Parameters.Add("@CPART_LOC2", SqlDbType.Int)
            myCommand.Parameters("@CPART_LOC2").Value = CPART_LOC2

            myCommand.Parameters.Add("@MiscValue", SqlDbType.VarChar)
            myCommand.Parameters("@MiscValue").Value = MiscValue

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@PartField", SqlDbType.VarChar)
            myCommand.Parameters("@PartField").Value = PartField

            myCommand.Parameters.Add("@SQLQuery", SqlDbType.VarChar)
            myCommand.Parameters("@SQLQuery").Value = commonFunctions.replaceSpecialChar(SQLQuery, False)

            myCommand.Parameters.Add("@AltOEMMfg", SqlDbType.VarChar)
            myCommand.Parameters("@AltOEMMfg").Value = AltOEMMfg

            myCommand.Parameters.Add("@PartField2", SqlDbType.VarChar)
            myCommand.Parameters("@PartField2").Value = PartField2

            myCommand.Parameters.Add("@PartSuffix_LOC1", SqlDbType.Int)
            myCommand.Parameters("@PartSuffix_LOC1").Value = PartSuffix_LOC1

            myCommand.Parameters.Add("@PartSuffix_LOC2", SqlDbType.Int)
            myCommand.Parameters("@PartSuffix_LOC2").Value = PartSuffix_LOC2

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", CreatedBy: " & CreatedBy & ", CreatedOn: " & CreatedOn

            HttpContext.Current.Session("BLLerror") = "InsertOEMModelConv : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertOEMModelConv") = "~/FIN/OEMModelConvMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertOEMModelConv : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertOEMModelConv

    Public Shared Sub UpdateOEMModelConv(ByVal RowID As Integer, ByVal OEMValidator As String, ByVal OEM As String, ByVal CabbvValidator As String, ByVal CABBV As String, ByVal SoldToValidator As String, ByVal SOLDTO As String, ByVal DabbvValidator As String, ByVal DABBV As String, ByVal CPART_LOC1 As Integer, ByVal CPART_LOC2 As Integer, ByVal MiscValue As String, ByVal Notes As String, ByVal PartField As String, ByVal SQLQuery As String, ByVal AltOEMMfg As String, ByVal PartField2 As String, ByVal PartSuffix_LOC1 As Integer, ByVal PartSuffix_LOC2 As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnDataMart").ToString
        Dim strStoredProcName As String = "sp_Update_OEM_Model_Conv"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@OEMValidator", SqlDbType.VarChar)
            myCommand.Parameters("@OEMValidator").Value = OEMValidator

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@CabbvValidator", SqlDbType.VarChar)
            myCommand.Parameters("@CabbvValidator").Value = CabbvValidator

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldToValidator", SqlDbType.VarChar)
            myCommand.Parameters("@SoldToValidator").Value = SoldToValidator

            myCommand.Parameters.Add("@SOLDTO", SqlDbType.VarChar)
            myCommand.Parameters("@SOLDTO").Value = SOLDTO

            myCommand.Parameters.Add("@DabbvValidator", SqlDbType.VarChar)
            myCommand.Parameters("@DabbvValidator").Value = DabbvValidator

            myCommand.Parameters.Add("@DABBV", SqlDbType.VarChar)
            myCommand.Parameters("@DABBV").Value = DABBV

            myCommand.Parameters.Add("@CPART_LOC1", SqlDbType.Int)
            myCommand.Parameters("@CPART_LOC1").Value = CPART_LOC1

            myCommand.Parameters.Add("@CPART_LOC2", SqlDbType.Int)
            myCommand.Parameters("@CPART_LOC2").Value = CPART_LOC2

            myCommand.Parameters.Add("@MiscValue", SqlDbType.VarChar)
            myCommand.Parameters("@MiscValue").Value = MiscValue

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@PartField", SqlDbType.VarChar)
            myCommand.Parameters("@PartField").Value = PartField

            myCommand.Parameters.Add("@SQLQuery", SqlDbType.VarChar)
            myCommand.Parameters("@SQLQuery").Value = commonFunctions.replaceSpecialChar(SQLQuery, False)

            myCommand.Parameters.Add("@AltOEMMfg", SqlDbType.VarChar)
            myCommand.Parameters("@AltOEMMfg").Value = AltOEMMfg

            myCommand.Parameters.Add("@PartField2", SqlDbType.VarChar)
            myCommand.Parameters("@PartField2").Value = PartField2

            myCommand.Parameters.Add("@PartSuffix_LOC1", SqlDbType.Int)
            myCommand.Parameters("@PartSuffix_LOC1").Value = PartSuffix_LOC1

            myCommand.Parameters.Add("@PartSuffix_LOC2", SqlDbType.Int)
            myCommand.Parameters("@PartSuffix_LOC2").Value = PartSuffix_LOC2

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", UpdatedBy: " & UpdatedBy & ", UpdatedOn: " & UpdatedOn

            HttpContext.Current.Session("BLLerror") = "UpdateOEMModelConv : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdateOEMModelConv") = "~/FIN/OEMModelConvMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateOEMModelConv : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateOEMModelConv

    Public Shared Sub InsertPartNoByOEMbyRowID(ByVal RowID As Integer, ByVal CreatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnDataMart").ToString
        Dim strStoredProcName As String = "sp_Insert_PartNo_by_OEM_by_RowID"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertPartNoByOEMbyRowID : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertPartNoByOEMbyRowID") = "~/FIN/OEMModelConvMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertPartNoByOEMbyRowID : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF InsertPartNoByOEMbyRowID

    Public Shared Sub UpdatePartNoByOEMbyRowID(ByVal RowID As Integer, ByVal Updatedby As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnDataMart").ToString
        Dim strStoredProcName As String = "sp_Update_PartNo_by_OEM_by_RowID"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@Updatedby", SqlDbType.VarChar)
            myCommand.Parameters("@Updatedby").Value = Updatedby

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdatePartNoByOEMbyRowID : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdatePartNoByOEMbyRowID") = "~/FIN/OEMModelConvMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdatePartNoByOEMbyRowID : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdatePartNoByOEMbyRowID

    Public Shared Sub DeleteOEMConvCookies()

        Try
            HttpContext.Current.Response.Cookies("OEM_OEM").Value = ""
            HttpContext.Current.Response.Cookies("OEM_OEM").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("OEM_CABBV").Value = ""
            HttpContext.Current.Response.Cookies("OEM_CABBV").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("OEM_SOLDTO").Value = ""
            HttpContext.Current.Response.Cookies("OEM_SOLDTO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("OEM_DABBV").Value = ""
            HttpContext.Current.Response.Cookies("OEM_DABBV").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("OEM_PARTFIELD").Value = ""
            HttpContext.Current.Response.Cookies("OEM_PARTFIELD").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("OEM_OEMMFG").Value = ""
            HttpContext.Current.Response.Cookies("OEM_OEMMFG").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteOEMConvCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FINModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/FIN/OEMModelConvMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteOEMConvCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "FINModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteOEMConvCookies
#End Region
End Class
