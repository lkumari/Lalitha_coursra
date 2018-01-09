''************************************************************************************************
''Name:		PFModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or getting user-rights for the Planning Forecasting Module
''
''Date		    Author	    
''03/19/2008    LRey			Created .Net application
''04/22/2008    LRey            commented out all references to DABBV per Mike E.
''05/14/2009    LRey            Added new function(s) for Copy_Vehicle.aspx
''08/06/2010    LRey            Added Royalty to Get, Insert & Update functions.
''05/04/2012    LRey            Added Comments to the Insert & Update function for Projected_Sales
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

Public Class PFModule
    Inherits System.ComponentModel.Component
    ' ''Public Shared Function GetVehicle(ByVal ProgramID As Integer, ByVal PlanningYear As Integer, ByVal CABBV As String, ByVal DABBV As String, ByVal AcctMgrID As Integer) As DataSet
    Public Shared Function GetVehicle(ByVal ProgramID As Integer, ByVal PlanningYear As Integer, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal AcctMgrID As Integer, ByVal Make As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = Nothing
        strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Vehicle"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@PlanningYear", SqlDbType.Int)
            myCommand.Parameters("@PlanningYear").Value = PlanningYear

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            ' ''myCommand.Parameters.Add("@DABBV", SqlDbType.VarChar)
            ' ''myCommand.Parameters("@DABBV").Value = DABBV

            myCommand.Parameters.Add("@AcctMgrID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrID").Value = AcctMgrID

            myCommand.Parameters.Add("@Make", SqlDbType.VarChar)
            myCommand.Parameters("@Make").Value = Make

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Vehicle")

            GetVehicle = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & ProgramID & ", PlanningYear: " & PlanningYear & ", CABBV:" & CABBV & ", SoldTo: " & SoldTo & ", AcctMgrID: " & AcctMgrID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetVehicle : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Vehicle_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetVehicle : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetVehicle = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try


    End Function 'EOF GetVehicle

    Public Shared Function GetVehicleSOP(ByVal ProgramID As Integer, ByVal CABBV As String, ByVal SoldTo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = Nothing
        strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Vehicle_SOP"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            ' ''myCommand.Parameters.Add("@DABBV", SqlDbType.VarChar)
            ' ''myCommand.Parameters("@DABBV").Value = DABBV

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Vehicle")
            GetVehicleSOP = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & ProgramID & ", CABBV:" & CABBV & ", SoldTo: " & SoldTo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetVehicleSOP : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Vehicle_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetVehicleSOP : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetVehicleSOP = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetVehicleSOP

    Public Shared Sub InsertVehicle(ByVal ProgramID As Integer, ByVal PlanningYear As Integer, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal AnnualVolume As Decimal, ByVal AcctMgrID As Integer, ByVal SOP As String, ByVal EOP As String, ByVal Jan As Decimal, ByVal Feb As Decimal, ByVal Mar As Decimal, ByVal Apr As Decimal, ByVal May As Decimal, ByVal Jun As Decimal, ByVal Jul As Decimal, ByVal Aug As Decimal, ByVal Sep As Decimal, ByVal Oct As Decimal, ByVal Nov As Decimal, ByVal Dec As Decimal)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Vehicle"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@PlanningYear", SqlDbType.Int)
            myCommand.Parameters("@PlanningYear").Value = PlanningYear

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            myCommand.Parameters.Add("@AnnualVolume", SqlDbType.Decimal)
            myCommand.Parameters("@AnnualVolume").Value = AnnualVolume

            myCommand.Parameters.Add("@AcctMgrID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrID").Value = AcctMgrID

            myCommand.Parameters.Add("@SOP", SqlDbType.VarChar)
            myCommand.Parameters("@SOP").Value = SOP

            myCommand.Parameters.Add("@EOP", SqlDbType.VarChar)
            myCommand.Parameters("@EOP").Value = EOP

            myCommand.Parameters.Add("@JAN", SqlDbType.Decimal)
            myCommand.Parameters("@JAN").Value = Jan

            myCommand.Parameters.Add("@FEB", SqlDbType.Decimal)
            myCommand.Parameters("@FEB").Value = Feb

            myCommand.Parameters.Add("@MAR", SqlDbType.Decimal)
            myCommand.Parameters("@MAR").Value = Mar

            myCommand.Parameters.Add("@APR", SqlDbType.Decimal)
            myCommand.Parameters("@APR").Value = Apr

            myCommand.Parameters.Add("@MAY", SqlDbType.Decimal)
            myCommand.Parameters("@MAY").Value = May

            myCommand.Parameters.Add("@JUN", SqlDbType.Decimal)
            myCommand.Parameters("@JUN").Value = Jun

            myCommand.Parameters.Add("@JUL", SqlDbType.Decimal)
            myCommand.Parameters("@JUL").Value = Jul

            myCommand.Parameters.Add("@AUG", SqlDbType.Decimal)
            myCommand.Parameters("@AUG").Value = Aug

            myCommand.Parameters.Add("@SEP", SqlDbType.Decimal)
            myCommand.Parameters("@SEP").Value = Sep

            myCommand.Parameters.Add("@OCT", SqlDbType.Decimal)
            myCommand.Parameters("@OCT").Value = Oct

            myCommand.Parameters.Add("@NOV", SqlDbType.Decimal)
            myCommand.Parameters("@NOV").Value = Nov

            myCommand.Parameters.Add("@DEC", SqlDbType.Decimal)
            myCommand.Parameters("@DEC").Value = Dec

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & ProgramID & ", PlanningYear: " & PlanningYear & ", CABBV:" & CABBV & ", SoldTo: " & SoldTo & ", AcctMgrID: " & AcctMgrID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertVehicle : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Vehicle_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertVehicle : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertVehicle

    Public Shared Sub InsertVehicleHistory(ByVal ProgramID As Integer, ByVal PlanningYear As Integer, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal DABBV As String, ByVal Prev_AcctMgrID As Integer, ByVal New_AcctMgrID As Integer, ByVal Prev_SOP As String, ByVal New_SOP As String, ByVal Prev_EOP As String, ByVal New_EOP As String, ByVal Prev_AnnualVolume As Decimal, ByVal Prev_JanVolume As Decimal, ByVal Prev_FebVolume As Decimal, ByVal Prev_MarVolume As Decimal, ByVal Prev_AprVolume As Decimal, ByVal Prev_MayVolume As Decimal, ByVal Prev_JunVolume As Decimal, ByVal Prev_JulVolume As Decimal, ByVal Prev_AugVolume As Decimal, ByVal Prev_SepVolume As Decimal, ByVal Prev_OctVolume As Decimal, ByVal Prev_NovVolume As Decimal, ByVal Prev_DecVolume As Decimal, ByVal New_AnnualVolume As Decimal, ByVal New_JanVolume As Decimal, ByVal New_FebVolume As Decimal, ByVal New_MarVolume As Decimal, ByVal New_AprVolume As Decimal, ByVal New_MayVolume As Decimal, ByVal New_JunVolume As Decimal, ByVal New_JulVolume As Decimal, ByVal New_AugVolume As Decimal, ByVal New_SepVolume As Decimal, ByVal New_OctVolume As Decimal, ByVal New_NovVolume As Decimal, ByVal New_DecVolume As Decimal, ByVal Notes As String, ByVal IHSDataUsed As Boolean, ByVal ProdDateChanged As Boolean, ByVal MonthlyVolumeChanged As Boolean, ByVal AcctMgrChanged As Boolean, ByVal ActionTakenTMID As Integer, ByVal ActionTakenBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Vehicle_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@PlanningYear", SqlDbType.Int)
            myCommand.Parameters("@PlanningYear").Value = PlanningYear

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            myCommand.Parameters.Add("@Prev_AcctMgrID", SqlDbType.Int)
            myCommand.Parameters("@Prev_AcctMgrID").Value = Prev_AcctMgrID

            myCommand.Parameters.Add("@Prev_SOP", SqlDbType.VarChar)
            myCommand.Parameters("@Prev_SOP").Value = Prev_SOP

            myCommand.Parameters.Add("@Prev_EOP", SqlDbType.VarChar)
            myCommand.Parameters("@Prev_EOP").Value = Prev_EOP

            myCommand.Parameters.Add("@Prev_AnnualVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_AnnualVolume").Value = Prev_AnnualVolume

            myCommand.Parameters.Add("@Prev_JanVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_JanVolume").Value = Prev_JanVolume

            myCommand.Parameters.Add("@Prev_FebVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_FebVolume").Value = Prev_FebVolume

            myCommand.Parameters.Add("@Prev_MarVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_MarVolume").Value = Prev_MarVolume

            myCommand.Parameters.Add("@Prev_AprVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_AprVolume").Value = Prev_AprVolume

            myCommand.Parameters.Add("@Prev_MayVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_MayVolume").Value = Prev_MayVolume

            myCommand.Parameters.Add("@Prev_JunVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_JunVolume").Value = Prev_JunVolume

            myCommand.Parameters.Add("@Prev_JulVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_JulVolume").Value = Prev_JulVolume

            myCommand.Parameters.Add("@Prev_AugVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_AugVolume").Value = Prev_AugVolume

            myCommand.Parameters.Add("@Prev_SepVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_SepVolume").Value = Prev_SepVolume

            myCommand.Parameters.Add("@Prev_OctVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_OctVolume").Value = Prev_OctVolume

            myCommand.Parameters.Add("@Prev_NovVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_NovVolume").Value = Prev_NovVolume

            myCommand.Parameters.Add("@Prev_DecVolume", SqlDbType.Decimal)
            myCommand.Parameters("@Prev_DecVolume").Value = Prev_DecVolume

            myCommand.Parameters.Add("@New_AcctMgrID", SqlDbType.Int)
            myCommand.Parameters("@New_AcctMgrID").Value = New_AcctMgrID

            myCommand.Parameters.Add("@New_SOP", SqlDbType.VarChar)
            myCommand.Parameters("@New_SOP").Value = New_SOP

            myCommand.Parameters.Add("@New_EOP", SqlDbType.VarChar)
            myCommand.Parameters("@New_EOP").Value = New_EOP

            myCommand.Parameters.Add("@New_AnnualVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_AnnualVolume").Value = New_AnnualVolume

            myCommand.Parameters.Add("@New_JanVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_JanVolume").Value = New_JanVolume

            myCommand.Parameters.Add("@New_FebVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_FebVolume").Value = New_FebVolume

            myCommand.Parameters.Add("@New_MarVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_MarVolume").Value = New_MarVolume

            myCommand.Parameters.Add("@New_AprVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_AprVolume").Value = New_AprVolume

            myCommand.Parameters.Add("@New_MayVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_MayVolume").Value = New_MayVolume

            myCommand.Parameters.Add("@New_JunVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_JunVolume").Value = New_JunVolume

            myCommand.Parameters.Add("@New_JulVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_JulVolume").Value = New_JulVolume

            myCommand.Parameters.Add("@New_AugVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_AugVolume").Value = New_AugVolume

            myCommand.Parameters.Add("@New_SepVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_SepVolume").Value = New_SepVolume

            myCommand.Parameters.Add("@New_OctVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_OctVolume").Value = New_OctVolume

            myCommand.Parameters.Add("@New_NovVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_NovVolume").Value = New_NovVolume

            myCommand.Parameters.Add("@New_DecVolume", SqlDbType.Decimal)
            myCommand.Parameters("@New_DecVolume").Value = New_DecVolume

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.convertSpecialChar(Notes, False)

            myCommand.Parameters.Add("@IHSDataUsed", SqlDbType.Bit)
            myCommand.Parameters("@IHSDataUsed").Value = IHSDataUsed

            myCommand.Parameters.Add("@ProdDateChanged", SqlDbType.Bit)
            myCommand.Parameters("@ProdDateChanged").Value = ProdDateChanged

            myCommand.Parameters.Add("@MonthlyVolumeChanged", SqlDbType.Bit)
            myCommand.Parameters("@MonthlyVolumeChanged").Value = MonthlyVolumeChanged

            myCommand.Parameters.Add("@AcctMgrChanged", SqlDbType.Bit)
            myCommand.Parameters("@AcctMgrChanged").Value = AcctMgrChanged

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            myCommand.Parameters.Add("@ActionTakenBy", SqlDbType.VarChar)
            myCommand.Parameters("@ActionTakenBy").Value = ActionTakenBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & ProgramID & ", PlanningYear: " & PlanningYear & ", CABBV:" & CABBV & ", SoldTo: " & SoldTo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertVehicleHistory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Vehicle_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertVehicleHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertVehicleHistory


    Public Shared Sub UpdateVehicle(ByVal ProgramID As Integer, ByVal PlanningYear As Integer, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal AnnualVolume As Decimal, ByVal AcctMgrID As Integer, ByVal SOP As String, ByVal EOP As String, ByVal Jan As Decimal, ByVal Feb As Decimal, ByVal Mar As Decimal, ByVal Apr As Decimal, ByVal May As Decimal, ByVal Jun As Decimal, ByVal Jul As Decimal, ByVal Aug As Decimal, ByVal Sep As Decimal, ByVal Oct As Decimal, ByVal Nov As Decimal, ByVal Dec As Decimal)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Vehicle"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@PlanningYear", SqlDbType.Int)
            myCommand.Parameters("@PlanningYear").Value = PlanningYear

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            ' ''myCommand.Parameters.Add("@DABBV", SqlDbType.VarChar)
            ' ''myCommand.Parameters("@DABBV").Value = DABBV

            myCommand.Parameters.Add("@AnnualVolume", SqlDbType.Decimal)
            myCommand.Parameters("@AnnualVolume").Value = AnnualVolume

            myCommand.Parameters.Add("@AcctMgrID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrID").Value = AcctMgrID

            myCommand.Parameters.Add("@SOP", SqlDbType.VarChar)
            myCommand.Parameters("@SOP").Value = SOP

            myCommand.Parameters.Add("@EOP", SqlDbType.VarChar)
            myCommand.Parameters("@EOP").Value = EOP

            myCommand.Parameters.Add("@JAN", SqlDbType.Decimal)
            myCommand.Parameters("@JAN").Value = Jan

            myCommand.Parameters.Add("@FEB", SqlDbType.Decimal)
            myCommand.Parameters("@FEB").Value = Feb

            myCommand.Parameters.Add("@MAR", SqlDbType.Decimal)
            myCommand.Parameters("@MAR").Value = Mar

            myCommand.Parameters.Add("@APR", SqlDbType.Decimal)
            myCommand.Parameters("@APR").Value = Apr

            myCommand.Parameters.Add("@MAY", SqlDbType.Decimal)
            myCommand.Parameters("@MAY").Value = May

            myCommand.Parameters.Add("@JUN", SqlDbType.Decimal)
            myCommand.Parameters("@JUN").Value = Jun

            myCommand.Parameters.Add("@JUL", SqlDbType.Decimal)
            myCommand.Parameters("@JUL").Value = Jul

            myCommand.Parameters.Add("@AUG", SqlDbType.Decimal)
            myCommand.Parameters("@AUG").Value = Aug

            myCommand.Parameters.Add("@SEP", SqlDbType.Decimal)
            myCommand.Parameters("@SEP").Value = Sep

            myCommand.Parameters.Add("@OCT", SqlDbType.Decimal)
            myCommand.Parameters("@OCT").Value = Oct

            myCommand.Parameters.Add("@NOV", SqlDbType.Decimal)
            myCommand.Parameters("@NOV").Value = Nov

            myCommand.Parameters.Add("@DEC", SqlDbType.Decimal)
            myCommand.Parameters("@DEC").Value = Dec

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & ProgramID & ", PlanningYear: " & PlanningYear & ", CABBV:" & CABBV & ", SoldTo: " & SoldTo & ", AcctMgrID: " & AcctMgrID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateVehicle : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Vehicle_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateVehicle : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOP UpdateVehicle
    ' ''Public Shared Sub DeleteVehicle(ByVal ProgramID As Integer, ByVal PlanningYear As Integer, ByVal CABBV As String, ByVal DABBV As String)

    Public Shared Sub DeleteVehicle(ByVal ProgramID As Integer, ByVal PlanningYear As Integer, ByVal CABBV As String, ByVal SoldTo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Vehicle"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@PlanningYear", SqlDbType.Int)
            myCommand.Parameters("@PlanningYear").Value = PlanningYear

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            ' ''myCommand.Parameters.Add("@DABBV", SqlDbType.VarChar)
            ' ''myCommand.Parameters("@DABBV").Value = DABBV

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & ProgramID & ", PlanningYear: " & PlanningYear & ", CABBV:" & CABBV & ", SoldTo: " & SoldTo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteVehicle : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Vehicle_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteVehicle : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteVehicle
    Public Shared Sub DeletePFCookies_VehicleVolume()
        ''***
        '' Used to clear out cookies in the Vehicle Volume Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("PFV_PlanningYear").Value = ""
            HttpContext.Current.Response.Cookies("PFV_PlanningYear").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PFV_Program").Value = ""
            HttpContext.Current.Response.Cookies("PFV_Program").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PFV_CABBV").Value = ""
            HttpContext.Current.Response.Cookies("PFV_CABBV").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PFV_SoldTo").Value = ""
            HttpContext.Current.Response.Cookies("PFV_SoldTo").Expires = DateTime.Now.AddDays(-1)

            ' ''HttpContext.Current.Response.Cookies("PFV_DABBV").Value = ""
            ' ''HttpContext.Current.Response.Cookies("PFV_DABBV").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PFV_AMGRID").Value = ""
            HttpContext.Current.Response.Cookies("PFV_AMGRID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PFV_Make").Value = ""
            HttpContext.Current.Response.Cookies("PFV_Make").Expires = DateTime.Now.AddDays(-1)


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePFCookies_VehicleVolume : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Vehicle_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeletePFCookies_VehicleVolume : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF DeletePFCookies_VehicleVolume
    Public Shared Sub DeletePFCookies_FuturePartNo()
        ''***
        '' Used to clear out cookies in the Vehicle Volume Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("PF_txtPartNo").Value = ""
            HttpContext.Current.Response.Cookies("PF_txtPartNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PF_txtPartDesc").Value = ""
            HttpContext.Current.Response.Cookies("PF_txtPartDesc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PF_ddTeamMember").Value = ""
            HttpContext.Current.Response.Cookies("PF_ddTeamMember").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePFCookies_FuturePartNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Future_Part_Maint.aspx"

            UGNErrorTrapping.InsertErrorLog("DeletePFCookies_FuturePartNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF DeletePFCookies_FuturePartNo
    Public Shared Function GetProjectedSales(ByVal PartNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = Nothing
        strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Projected_Sales"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PartInfo")
            GetProjectedSales = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProjectedSales : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetProjectedSales : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetProjectedSales = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetProjectedSales
    Public Shared Function GetProjectedSalesCopy(ByVal SourcePartNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = Nothing
        strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Projected_Sales_copy"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SourcePartNo", SqlDbType.VarChar)
            myCommand.Parameters("@SourcePartNo").Value = SourcePartNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SourcePartNo")
            GetProjectedSalesCopy = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProjectedSalesCopy : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetProjectedSalesCopy : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetProjectedSalesCopy = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetProjectedSalesCopy

    Public Shared Function GetVehicleCopy(ByVal SourceProgramID As Integer, ByVal SourceCABBV As String, ByVal SourceSoldTo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = Nothing
        strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Vehicle_copy"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SourceProgramID", SqlDbType.Int)
            myCommand.Parameters("@SourceProgramID").Value = SourceProgramID

            myCommand.Parameters.Add("@SourceCABBV", SqlDbType.VarChar)
            myCommand.Parameters("@SourceCABBV").Value = SourceCABBV

            myCommand.Parameters.Add("@SourceSoldTo", SqlDbType.Int)
            myCommand.Parameters("@SourceSoldTo").Value = SourceSoldTo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SourcePartNo")
            GetVehicleCopy = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetVehicleCopy : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Vehicle_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetVehicleCopy : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetVehicleCopy = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetVehicleCopy

    ' ''Public Shared Function GetProjectedSalesListing(ByVal PartNo As String, ByVal ProgramID As Integer, ByVal ProgramStatus As String, ByVal CommodityID As Integer, ByVal CABBV As String, ByVal DABBV As String, ByVal ProductTechnologyID As Integer, ByVal AcctMgrID As Integer, ByVal UGNFacility As String) As DataSet
    Public Shared Function GetProjectedSalesListing(ByVal PartNo As String, ByVal ProgramID As Integer, ByVal ProgramStatus As String, ByVal CommodityID As Integer, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal ProductTechnologyID As Integer, ByVal AcctMgrID As Integer, ByVal UGNFacility As String, ByVal RoyaltyID As Integer, ByVal PlanningYear As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = Nothing
        strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Projected_Sales_Listing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@ProgramStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProgramStatus").Value = ProgramStatus

            myCommand.Parameters.Add("@CommodityID", SqlDbType.VarChar)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@ProductTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@ProductTechnologyID").Value = ProductTechnologyID

            myCommand.Parameters.Add("@AcctMgrID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrID").Value = AcctMgrID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@RoyaltyID", SqlDbType.Int)
            myCommand.Parameters("@RoyaltyID").Value = RoyaltyID


            myCommand.Parameters.Add("@PlanningYear", SqlDbType.Int)
            myCommand.Parameters("@PlanningYear").Value = PlanningYear

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PartInfo")
            GetProjectedSalesListing = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProjectedSalesListing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetProjectedSalesListing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetProjectedSalesListing = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetProjectedSalesListing
    Public Shared Function GetFuturePartNoByCreatedBy(ByVal PartNo As String) As DataSet
        ''Used to build the Team Member drop down list Future Part Maintenance search.
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = Nothing
        strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Future_PartNo_CreatedBy"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PartInfo")
            GetFuturePartNoByCreatedBy = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFuturePartNoByCreatedBy : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetFuturePartNoByCreatedBy : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetFuturePartNoByCreatedBy = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetFuturePartNoByCreatedBy

    Public Shared Sub InsertProjectedSales(ByVal PartNo As String, ByVal KeyPartIndicator As String, ByVal CommodityID As Integer, ByVal ProductTechnologyID As String, ByVal RoyaltyID As Integer, ByVal CostSheetID As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Projected_Sales"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@KeyPartIndicator", SqlDbType.VarChar)
            myCommand.Parameters("@KeyPartIndicator").Value = KeyPartIndicator

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@ProductTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@ProductTechnologyID").Value = CType(IIf(ProductTechnologyID = "", 0, ProductTechnologyID), Integer)

            myCommand.Parameters.Add("@RoyaltyID", SqlDbType.Int)
            myCommand.Parameters("@RoyaltyID").Value = RoyaltyID

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.convertSpecialChar(Comments, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertProjectedSales : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertProjectedSales : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertProjectedSales
    ' ''Public Shared Sub InsertProjectedSalesCustomerProgram(ByVal PartNo As String, ByVal CABBV As String, ByVal DABBV As String, ByVal ProgramID As Integer, ByVal UGNFacility As String, ByVal ProgramStatus As String, ByVal PiecesPerVehicle As Decimal, ByVal UsageFactorPerVehicle As Decimal)
    Public Shared Sub InsertProjectedSalesCustomerProgram(ByVal PartNo As String, ByVal CABBV As String, ByVal ProgramID As Integer, ByVal UGNFacility As String, ByVal ProgramStatus As String, ByVal PiecesPerVehicle As Decimal, ByVal UsageFactorPerVehicle As Decimal)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Projected_Sales_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            ' ''myCommand.Parameters.Add("@DABBV", SqlDbType.VarChar)
            ' ''myCommand.Parameters("@DABBV").Value = DABBV

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@ProgramStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProgramStatus").Value = ProgramStatus

            myCommand.Parameters.Add("@PiecesPerVehicle", SqlDbType.Decimal)
            myCommand.Parameters("@PiecesPerVehicle").Value = PiecesPerVehicle

            myCommand.Parameters.Add("@UsageFactorPerVehicle", SqlDbType.Decimal)
            myCommand.Parameters("@UsageFactorPerVehicle").Value = UsageFactorPerVehicle

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertProjectedSalesCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertProjectedSalesCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertProjectedSalesCustomerProgram
    Public Shared Sub InsertProjectedSalesPrice(ByVal PartNo As String, ByVal Price As Decimal, ByVal CostDownPercentage As Decimal, ByVal EffDate As String, ByVal EndDate As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Projected_Sales_Price"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@Price", SqlDbType.Decimal)
            myCommand.Parameters("@Price").Value = Price

            myCommand.Parameters.Add("@CostDownPercentage", SqlDbType.Decimal)
            myCommand.Parameters("@CostDownPercentage").Value = CostDownPercentage

            myCommand.Parameters.Add("@EffDate", SqlDbType.VarChar)
            myCommand.Parameters("@EffDate").Value = EffDate

            myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
            myCommand.Parameters("@EndDate").Value = EndDate

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertProjectedSalesPrice : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertProjectedSalesPrice : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertProjectedSalesPrice
    Public Shared Sub CopyProjectedSales(ByVal SourcePartNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_PF_Copy_Projected_Sales"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SourcePartNo", SqlDbType.VarChar)
            myCommand.Parameters("@SourcePartNo").Value = SourcePartNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyProjectedSales : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("CopyProjectedSales : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF CopyProjectedSales

    Public Shared Sub CopyVehicle(ByVal SourceProgramID As Integer, ByVal SourceCABBV As String, ByVal SourceSoldTo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_PF_Copy_Vehicle"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SourceProgramID", SqlDbType.Int)
            myCommand.Parameters("@SourceProgramID").Value = SourceProgramID

            myCommand.Parameters.Add("@SourceCABBV", SqlDbType.VarChar)
            myCommand.Parameters("@SourceCABBV").Value = SourceCABBV

            myCommand.Parameters.Add("@SourceSoldTo", SqlDbType.Int)
            myCommand.Parameters("@SourceSoldTo").Value = SourceSoldTo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyVehicle : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Vehicle_List.aspx"

            UGNErrorTrapping.InsertErrorLog("CopyVehicle : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF CopyVehicle
    ' ''Public Shared Sub UpdateProjectedSales(ByVal PartNo As String, ByVal KeyPartIndicator As String, ByVal CommodityID As Integer, ByVal ProductTechnologyID As Integer)
    Public Shared Sub UpdateProjectedSales(ByVal PartNo As String, ByVal KeyPartIndicator As String, ByVal CommodityID As Integer, ByVal ProductTechnologyID As Integer, ByVal Original_PartNo As String, ByVal RoyaltyID As Integer, ByVal CostSheetID As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Projected_Sales"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@OrigPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@OrigPartNo").Value = Original_PartNo

            myCommand.Parameters.Add("@KeyPartIndicator", SqlDbType.VarChar)
            myCommand.Parameters("@KeyPartIndicator").Value = KeyPartIndicator

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@ProductTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@ProductTechnologyID").Value = ProductTechnologyID

            myCommand.Parameters.Add("@RoyaltyID", SqlDbType.Int)
            myCommand.Parameters("@RoyaltyID").Value = RoyaltyID

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.convertSpecialChar(Comments, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateProjectedSales : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateProjectedSales : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateProjectedSales
    Public Shared Sub DeleteProjectedSales(ByVal PartNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Projected_Sales"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteProjectedSales : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteProjectedSales : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteProjectedSales

    Public Shared Sub RecalcProjectedSalesAfterDelete(ByVal PartNo As String, ByVal User As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Projected_Sales"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "RecalcProjectedSalesAfterDelete : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("RecalcProjectedSalesAfterDelete : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF RecalcProjectedSalesAfterDelete

    Public Shared Sub DeleteProjectedSalesCopy(ByVal SourcePartNo As String, ByVal DestinationPartNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Projected_Sales_Copy"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SourcePartNo", SqlDbType.VarChar)
            myCommand.Parameters("@SourcePartNo").Value = SourcePartNo

            myCommand.Parameters.Add("@DestinationPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@DestinationPartNo").Value = DestinationPartNo

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteProjectedSalesCopy : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteProjectedSalesCopy : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteProjectedSalesCopy

    Public Shared Sub DeleteVehicleCopy(ByVal SourceProgramID As Integer, ByVal SourceCABBV As String, ByVal SourceSoldTo As Integer, ByVal DestinationProgramID As Integer, ByVal DestinationCABBV As String, ByVal DestinationSoldTo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Vehicle_Copy"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SourceProgramID", SqlDbType.Int)
            myCommand.Parameters("@SourceProgramID").Value = SourceProgramID

            myCommand.Parameters.Add("@SourceCABBV", SqlDbType.VarChar)
            myCommand.Parameters("@SourceCABBV").Value = SourceCABBV

            myCommand.Parameters.Add("@SourceSoldTo", SqlDbType.Int)
            myCommand.Parameters("@SourceSoldTo").Value = SourceSoldTo

            myCommand.Parameters.Add("@DestinationProgramID", SqlDbType.Int)
            myCommand.Parameters("@DestinationProgramID").Value = DestinationProgramID

            myCommand.Parameters.Add("@DestinationCABBV", SqlDbType.VarChar)
            myCommand.Parameters("@DestinationCABBV").Value = DestinationCABBV

            myCommand.Parameters.Add("@DestinationSoldTo", SqlDbType.Int)
            myCommand.Parameters("@DestinationSoldTo").Value = DestinationSoldTo

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteVehicleCopy : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Vehicle_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteVehicleCopy : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteProjectedSalesCopy
    Public Shared Sub DeletePFCookies_SalesProjection()
        ''***
        '' Used to clear out cookies in the Sales Projection Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("PF_PartNo").Value = ""
            HttpContext.Current.Response.Cookies("PF_PartNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PF_Program").Value = ""
            HttpContext.Current.Response.Cookies("PF_Program").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PF_ProgramStatus").Value = ""
            HttpContext.Current.Response.Cookies("PF_ProgramStatus").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PF_Commodity").Value = ""
            HttpContext.Current.Response.Cookies("PF_Commodity").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PF_CABBV").Value = ""
            HttpContext.Current.Response.Cookies("PF_CABBV").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PF_SoldTo").Value = ""
            HttpContext.Current.Response.Cookies("PF_SoldTo").Expires = DateTime.Now.AddDays(-1)

            ' ''HttpContext.Current.Response.Cookies("PF_DABBV").Value = ""
            ' ''HttpContext.Current.Response.Cookies("PF_DABBV").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PF_ProductTechnology").Value = ""
            HttpContext.Current.Response.Cookies("PF_ProductTechnology").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PF_AMGRID").Value = ""
            HttpContext.Current.Response.Cookies("PF_AMGRID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PF_UGNFacility").Value = ""
            HttpContext.Current.Response.Cookies("PF_UGNFacility").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PF_RID").Value = ""
            HttpContext.Current.Response.Cookies("PF_RID").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePFCookies_SalesProjection : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeletePFCookies_SalesProjection : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF DeletePFCookies_SalesProjection
    Public Shared Sub DeletePFCCookies_VolumeAdjustment()
        ''***
        '' Used to clear out cookies in the Sales Projection Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("PRV_PYear").Value = ""
            HttpContext.Current.Response.Cookies("PRV_PYear").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_RType").Value = ""
            HttpContext.Current.Response.Cookies("PRV_RType").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_RTypeNo").Value = ""
            HttpContext.Current.Response.Cookies("PRV_RTypeNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Calc").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Calc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_CalcDI").Value = ""
            HttpContext.Current.Response.Cookies("PRV_CalcDI").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Jan").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Jan").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Feb").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Feb").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Mar").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Mar").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Apr").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Apr").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_May").Value = ""
            HttpContext.Current.Response.Cookies("PRV_May").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Jun").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Jun").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Jul").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Jul").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Aug").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Aug").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Sep").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Sep").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Oct").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Oct").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Nov").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Nov").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PRV_Dec").Value = ""
            HttpContext.Current.Response.Cookies("PRV_Dec").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePFCCookies_CalculateCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Cost_Down_Up_Calculator.aspx"

            UGNErrorTrapping.InsertErrorLog("DeletePFCCookies_CalculateCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF DeletePFCCookies_CalculateCost
    Public Shared Function GetArchiveData(ByVal PlanningYear As Integer, ByVal RecordType As String, ByVal RecordTypeNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = Nothing
        strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Archive_PF_Data"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PlanningYear", SqlDbType.Int)
            myCommand.Parameters("@PlanningYear").Value = PlanningYear

            myCommand.Parameters.Add("@RecordType", SqlDbType.VarChar)
            myCommand.Parameters("@RecordType").Value = RecordType

            myCommand.Parameters.Add("@RecordTypeNo", SqlDbType.Int)
            myCommand.Parameters("@RecordTypeNo").Value = RecordTypeNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Archive")
            GetArchiveData = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetArchiveData : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetArchiveData : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetArchiveData = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetArchiveData
    Public Shared Sub LockInSalesProjection(ByVal PlanningYear As Integer, ByVal RecordType As String, ByVal RecordTypeNo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Archive_PF_LockIn_Sales_Projection"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PlanningYear", SqlDbType.Int)
            myCommand.Parameters("@PlanningYear").Value = PlanningYear

            myCommand.Parameters.Add("@RecordType", SqlDbType.VarChar)
            myCommand.Parameters("@RecordType").Value = RecordType

            myCommand.Parameters.Add("@RecordTypeNo", SqlDbType.Int)
            myCommand.Parameters("@RecordTypeNo").Value = RecordTypeNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "LockInSalesProjection : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"

            UGNErrorTrapping.InsertErrorLog("LockInSalesProjection : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF LockInSalesProjection
    Public Shared Sub CleanPFCrystalReports()

        Try
            Dim tempRpt As ReportDocument = New ReportDocument()

            If HttpContext.Current.Session("TempCrystalRptFiles") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("TempCrystalRptFiles"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("TempCrystalRptFiles") = Nothing
                GC.Collect()
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CleanPFCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PFModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanPFCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False), "PFModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF CleanPFCrystalReports

End Class
