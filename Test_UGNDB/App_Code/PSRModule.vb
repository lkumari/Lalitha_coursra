''************************************************************************************************
''Name:		PSRModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or functions for the Plant Specific Reports Module
''
''Date		    Author	    
''06/01/2010    Roderick Carlson - Created .Net application
' 09/28/2010    Roderick Carlson - Modified - added TotalActualIndirectScrapDollar and Allocated Support
' 03/18/2011    Roderick Carlson - Modified - removed references to old Work Center functions
' 03/30/2011    Roderick Carlson - Modified - Added BudgetMachineHourStandard AND ActualMachineHourStandard to multiple functions and added function GetManufacturingMetricMachineHourStandardByDept
' 03/31/2011    Roderick Carlson - Modified - Added BudgetRawWipScrapDollar AND ActualRawWipScrapDollar columns to multiple function and added function GetManufacturingMetricRawWIPScrapDollarByDept
' 11/01/2012    Roderick Carlson - Modified - Removed ability to manually reload reports because it is now handled in SQL SSIS Packages
''************************************************************************************************

Imports Microsoft.VisualBasic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class PSRModule

    Public Shared Sub CleanPSRMMCrystalReports()

        Try
            Dim tempRpt As ReportDocument = New ReportDocument()
            'in order to clear crystal reports for Costing Preview
            If HttpContext.Current.Session("MMReportPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("MMReportPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("MMReportPreview") = Nothing
                GC.Collect()
            End If


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CleanPSRMMCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanPSRMMCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Sub DeletePSRMMCookies()

        Try
            HttpContext.Current.Response.Cookies("PSR-MM-Module_SaveStatusIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("PSR-MM-Module_SaveStatusIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PSR-MM-Module_SaveMonthIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("PSR-MM-Module_SaveMonthIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PSR-MM-Module_SaveYearIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("PSR-MM-Module_SaveYearIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PSR-MM-Module_SaveUGNFacilitySearch").Value = ""
            HttpContext.Current.Response.Cookies("PSR-MM-Module_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PSR-MM-Module_SaveCreatedByTMIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("PSR-MM-Module_SaveCreatedByTMIDSearch").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeletePSRMMCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePSRMMCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    'Public Shared Sub DeleteManufacturingMetric(ByVal ReportID As Integer, ByVal VoidReason As String)

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Delete_Manufacturing_Metric"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@ReportID", SqlDbType.Int)
    '        myCommand.Parameters("@ReportID").Value = ReportID

    '        If VoidReason Is Nothing Then
    '            VoidReason = ""
    '        End If

    '        myCommand.Parameters.Add("@VoidReason", SqlDbType.VarChar)
    '        myCommand.Parameters("@VoidReason").Value = VoidReason

    '        myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "ReportID: " & ReportID _
    '        & ", VoidReason: " & VoidReason _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "DeleteManufacturingMetric : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("DeleteManufacturingMetric : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    'Public Shared Function GetUGNMonthlyShippingDays(ByVal MonthID As Integer) As DataSet

    '    ''this function could be moved to the commonfunctions at a later time
    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_UGN_Monthly_Shipping_Days"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@MonthID", SqlDbType.Int)
    '        myCommand.Parameters("@MonthID").Value = MonthID

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ManufacturingMetricActualDowntimeHours")
    '        GetUGNMonthlyShippingDays = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "MonthID: " & MonthID _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetUGNMonthlyShippingDays : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetUGNMonthlyShippingDays : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetUGNMonthlyShippingDays = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    '  Public Shared Function GetManufacturingMetricActualDowntimeHoursByDept(ByVal DeptID As Integer, ByVal UGNFacility As String, _
    'ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '      Dim myConnection As SqlConnection = New SqlConnection
    '      Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '      Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Actual_Downtime_Hours_By_Dept"
    '      Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '      Dim GetData As New DataSet
    '      Dim myAdapter As New SqlDataAdapter

    '      Try
    '          myConnection.ConnectionString = strConnectionString
    '          myCommand.CommandType = CommandType.StoredProcedure

    '          myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '          myCommand.Parameters("@DeptID").Value = DeptID

    '          If UGNFacility Is Nothing Then
    '              UGNFacility = ""
    '          End If

    '          myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '          myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '          If StartDate Is Nothing Then
    '              StartDate = ""
    '          End If

    '          myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '          myCommand.Parameters("@StartDate").Value = StartDate

    '          If EndDate Is Nothing Then
    '              EndDate = ""
    '          End If

    '          myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '          myCommand.Parameters("@EndDate").Value = EndDate

    '          myCommand.Parameters.Add("@OutputActualDowntimeHours", SqlDbType.Decimal)
    '          myCommand.Parameters("@OutputActualDowntimeHours").Value = 0

    '          myAdapter = New SqlDataAdapter(myCommand)
    '          myAdapter.Fill(GetData, "ManufacturingMetricActualDowntimeHours")
    '          GetManufacturingMetricActualDowntimeHoursByDept = GetData

    '      Catch ex As Exception

    '          'on error, collect function data, error, and last page, then redirect to error page
    '          Dim strUserEditedData As String = "DeptID: " & DeptID _
    '          & ", UGNFacility: " & UGNFacility _
    '          & ", StartDate: " & StartDate _
    '          & ", EndDate: " & EndDate _
    '          & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '          HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricActualDowntimeHoursByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '          HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '          UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricActualDowntimeHours : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '          HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '          GetManufacturingMetricActualDowntimeHoursByDept = Nothing
    '      Finally
    '          myConnection.Close()
    '          myCommand.Dispose()
    '      End Try

    '  End Function

    'Public Shared Function GetManufacturingMetricActualMachineHoursByDept(ByVal DeptID As Integer, ByVal UGNFacility As String, _
    'ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Actual_Machine_Hours_By_Dept"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '        myCommand.Parameters("@DeptID").Value = DeptID

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        If StartDate Is Nothing Then
    '            StartDate = ""
    '        End If

    '        myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@StartDate").Value = StartDate

    '        If EndDate Is Nothing Then
    '            EndDate = ""
    '        End If

    '        myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@EndDate").Value = EndDate

    '        myCommand.Parameters.Add("@OutputActualMachineHours", SqlDbType.Decimal)
    '        myCommand.Parameters("@OutputActualMachineHours").Value = 0

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ManufacturingMetricActualMachineHoursByDept")
    '        GetManufacturingMetricActualMachineHoursByDept = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "DeptID: " & DeptID & ", UGNFacility: " & UGNFacility _
    '        & ", StartDate: " & StartDate & ", EndDate: " & EndDate _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricActualMachineHoursByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricActualMachineHoursByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetManufacturingMetricActualMachineHoursByDept = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    'Public Shared Function GetManufacturingMetricActualManHoursByDept(ByVal DeptID As Integer, ByVal UGNFacility As String, _
    'ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Actual_Man_Hours_By_Dept"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '        myCommand.Parameters("@DeptID").Value = DeptID

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        If StartDate Is Nothing Then
    '            StartDate = ""
    '        End If

    '        myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@StartDate").Value = StartDate

    '        If EndDate Is Nothing Then
    '            EndDate = ""
    '        End If

    '        myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@EndDate").Value = EndDate

    '        myCommand.Parameters.Add("@OutputActualManHours", SqlDbType.Decimal)
    '        myCommand.Parameters("@OutputActualManHours").Value = 0

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ManufacturingMetricActualManHoursByDept")
    '        GetManufacturingMetricActualManHoursByDept = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "DeptID: " & DeptID _
    '        & ", UGNFacility: " & UGNFacility _
    '        & ", StartDate: " & StartDate _
    '        & ", EndDate: " & EndDate _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricActualManHoursByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricActualManHoursByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetManufacturingMetricActualManHoursByDept = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    Public Shared Function GetManufacturingMetricManHourDowntimeAllShiftAllScheduleDetailByDept(ByVal DeptID As Integer, _
        ByVal UGNFacility As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Man_Hour_Downtime_All_Shift_All_Schedule_Detail_By_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
            myCommand.Parameters("@DeptID").Value = DeptID

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            If StartDate Is Nothing Then
                StartDate = ""
            End If

            myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartDate").Value = StartDate

            If EndDate Is Nothing Then
                EndDate = ""
            End If

            myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
            myCommand.Parameters("@EndDate").Value = EndDate

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricManHourDowntimeAllShiftAllScheduleDetailByDept")
            GetManufacturingMetricManHourDowntimeAllShiftAllScheduleDetailByDept = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DeptID: " & DeptID & ", UGNFacility: " & UGNFacility _
            & ", StartDate: " & StartDate & ", EndDate: " & EndDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricManHourDowntimeAllShiftAllScheduleDetailByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricManHourDowntimeAllShiftAllScheduleDetailByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricManHourDowntimeAllShiftAllScheduleDetailByDept = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    'Public Shared Function GetManufacturingMetricManHourDowntimeAllShiftAllScheduleTotalByDept(ByVal DeptID As Integer, _
    '    ByVal UGNFacility As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Man_Hour_Downtime_All_Shift_All_Schedule_Total_By_Dept"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString

    '        myCommand.CommandType = CommandType.StoredProcedure
    '        myCommand.CommandTimeout = 0

    '        myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '        myCommand.Parameters("@DeptID").Value = DeptID

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        If StartDate Is Nothing Then
    '            StartDate = ""
    '        End If

    '        myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@StartDate").Value = StartDate

    '        If EndDate Is Nothing Then
    '            EndDate = ""
    '        End If

    '        myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@EndDate").Value = EndDate

    '        myCommand.Parameters.Add("@OutputFinalTotalManHourDowntimeTotal", SqlDbType.Decimal)
    '        myCommand.Parameters("@OutputFinalTotalManHourDowntimeTotal").Value = 0

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ManufacturingMetricManHourDowntimeAllShiftAllScheduleTotalByDept")
    '        GetManufacturingMetricManHourDowntimeAllShiftAllScheduleTotalByDept = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "DeptID: " & DeptID & ", UGNFacility: " & UGNFacility _
    '        & ", StartDate: " & StartDate & ", EndDate: " & EndDate _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricManHourDowntimeAllShiftAllScheduleTotalByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricManHourDowntimeAllShiftAllScheduleTotalByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetManufacturingMetricManHourDowntimeAllShiftAllScheduleTotalByDept = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    Public Shared Function GetManufacturingMetricDetailByDept(ByVal ReportID As Integer, ByVal DeptID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Detail_By_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ReportID", SqlDbType.Int)
            myCommand.Parameters("@ReportID").Value = ReportID

            myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
            myCommand.Parameters("@DeptID").Value = DeptID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricDetailByDept")
            GetManufacturingMetricDetailByDept = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReportID: " & ReportID _
            & ", DeptID: " & DeptID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricDetailByDept : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricDetailByDept : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricDetailByDept = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetManufacturingMetricDetailTotalByDept(ByVal ReportID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Detail_Total_By_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ReportID", SqlDbType.Int)
            myCommand.Parameters("@ReportID").Value = ReportID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricDetailTotalByDept")
            GetManufacturingMetricDetailTotalByDept = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReportID: " & ReportID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricDetailTotalByDept : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricDetailTotalByDept : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricDetailTotalByDept = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetManufacturingMetricHeader(ByVal ReportID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Header"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ReportID", SqlDbType.Int)
            myCommand.Parameters("@ReportID").Value = ReportID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricHeader")
            GetManufacturingMetricHeader = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReportID: " & ReportID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricHeader : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricHeader : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricHeader = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    'Public Shared Function GetManufacturingMetricMachineHourDowntimeAllShiftTotalByDept(ByVal DeptID As Integer, _
    'ByVal UGNFacility As String, ByVal StartDate As String, ByVal EndDate As String, ByVal isScheduled As Boolean) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Machine_Hour_Downtime_All_Shift_Total_By_Dept"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '        myCommand.Parameters("@DeptID").Value = DeptID

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        If StartDate Is Nothing Then
    '            StartDate = ""
    '        End If

    '        myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@StartDate").Value = StartDate

    '        If EndDate Is Nothing Then
    '            EndDate = ""
    '        End If

    '        myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@EndDate").Value = EndDate

    '        myCommand.Parameters.Add("@isScheduled", SqlDbType.Bit)
    '        myCommand.Parameters("@isScheduled").Value = isScheduled

    '        myCommand.Parameters.Add("@OutputTotalMachineHoursDowntime", SqlDbType.Decimal)
    '        myCommand.Parameters("@OutputTotalMachineHoursDowntime").Value = 0

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ManufacturingMetricMachineHourDowntimeAllShiftTotalbyDept")
    '        GetManufacturingMetricMachineHourDowntimeAllShiftTotalByDept = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "DeptID: " & DeptID _
    '        & ", UGNFacility: " & UGNFacility _
    '        & ", StartDate: " & StartDate _
    '        & ", EndDate: " & EndDate _
    '        & ", isScheduled: " & isScheduled _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricMachineHourDowntimeAllShiftTotalByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricMachineHourDowntimeAllShiftTotalByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetManufacturingMetricMachineHourDowntimeAllShiftTotalByDept = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    Public Shared Function GetManufacturingMetricMachineHourDowntimeDetailByDept(ByVal DeptID As Integer, _
        ByVal UGNFacility As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Machine_Hour_Downtime_Detail_By_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
            myCommand.Parameters("@DeptID").Value = DeptID

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            If StartDate Is Nothing Then
                StartDate = ""
            End If

            myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartDate").Value = StartDate

            If EndDate Is Nothing Then
                EndDate = ""
            End If

            myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
            myCommand.Parameters("@EndDate").Value = EndDate

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetManufacturingMetricMachineHourDowntimeDetailByDept")
            GetManufacturingMetricMachineHourDowntimeDetailByDept = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DeptID: " & DeptID _
            & ", UGNFacility: " & UGNFacility _
            & ", StartDate: " & StartDate _
            & ", EndDate: " & EndDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricMachineHourDowntimeDetailByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricMachineHourDowntimeDetailByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricMachineHourDowntimeDetailByDept = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    'Public Shared Function GetManufacturingMetricMachineHourStandardByDept(ByVal DeptID As Integer, _
    '    ByVal UGNFacility As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Machine_Hour_Standard_By_Dept"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '        myCommand.Parameters("@DeptID").Value = DeptID

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        If StartDate Is Nothing Then
    '            StartDate = ""
    '        End If

    '        myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@StartDate").Value = StartDate

    '        If EndDate Is Nothing Then
    '            EndDate = ""
    '        End If

    '        myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@EndDate").Value = EndDate

    '        myCommand.Parameters.Add("@OutputMachineHourStandard", SqlDbType.Int)
    '        myCommand.Parameters("@OutputMachineHourStandard").Value = 0

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "GetManufacturingMetricMachineHourStandardByDept")
    '        GetManufacturingMetricMachineHourStandardByDept = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "DeptID: " & DeptID _
    '        & ", UGNFacility: " & UGNFacility _
    '        & ", StartDate: " & StartDate _
    '        & ", EndDate: " & EndDate _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricMachineHourStandardByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricMachineHourStandardByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetManufacturingMetricMachineHourStandardByDept = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function
 
    '  Public Shared Function GetManufacturingMetricProductionQuantityByDept(ByVal DeptID As Integer, ByVal UGNFacility As String, _
    'ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '      Dim myConnection As SqlConnection = New SqlConnection
    '      Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '      Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Production_Quantity_By_Dept"
    '      Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '      Dim GetData As New DataSet
    '      Dim myAdapter As New SqlDataAdapter

    '      Try
    '          myConnection.ConnectionString = strConnectionString
    '          myCommand.CommandType = CommandType.StoredProcedure

    '          myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '          myCommand.Parameters("@DeptID").Value = DeptID

    '          If UGNFacility Is Nothing Then
    '              UGNFacility = ""
    '          End If

    '          myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '          myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '          If StartDate Is Nothing Then
    '              StartDate = ""
    '          End If

    '          myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '          myCommand.Parameters("@StartDate").Value = StartDate

    '          If EndDate Is Nothing Then
    '              EndDate = ""
    '          End If

    '          myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '          myCommand.Parameters("@EndDate").Value = EndDate

    '          myCommand.Parameters.Add("@OutputMTDProductionQTY", SqlDbType.Int)
    '          myCommand.Parameters("@OutputMTDProductionQTY").Value = 0

    '          myAdapter = New SqlDataAdapter(myCommand)
    '          myAdapter.Fill(GetData, "ManufacturingMetricProductionQuantityByDept")
    '          GetManufacturingMetricProductionQuantityByDept = GetData

    '      Catch ex As Exception

    '          'on error, collect function data, error, and last page, then redirect to error page
    '          Dim strUserEditedData As String = "DeptID: " & DeptID & ", UGNFacility: " & UGNFacility _
    '          & ", StartDate: " & StartDate & ", EndDate: " & EndDate _
    '          & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '          HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricProductionQuantityByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '          HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '          UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricProductionQuantityByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '          HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '          GetManufacturingMetricProductionQuantityByDept = Nothing
    '      Finally
    '          myConnection.Close()
    '          myCommand.Dispose()
    '      End Try

    '  End Function

    '    Public Shared Function GetManufacturingMetricProductionDollarByDept(ByVal DeptID As Integer, ByVal UGNFacility As String, _
    'ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '        Dim myConnection As SqlConnection = New SqlConnection
    '        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Production_Dollar_By_Dept"
    '        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '        Dim GetData As New DataSet
    '        Dim myAdapter As New SqlDataAdapter

    '        Try
    '            myConnection.ConnectionString = strConnectionString
    '            myCommand.CommandType = CommandType.StoredProcedure

    '            myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '            myCommand.Parameters("@DeptID").Value = DeptID

    '            If UGNFacility Is Nothing Then
    '                UGNFacility = ""
    '            End If

    '            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '            myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '            If StartDate Is Nothing Then
    '                StartDate = ""
    '            End If

    '            myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '            myCommand.Parameters("@StartDate").Value = StartDate

    '            If EndDate Is Nothing Then
    '                EndDate = ""
    '            End If

    '            myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '            myCommand.Parameters("@EndDate").Value = EndDate

    '            myCommand.Parameters.Add("@OutputTotalProductionDollar", SqlDbType.Decimal)
    '            myCommand.Parameters("@OutputTotalProductionDollar").Value = 0

    '            myAdapter = New SqlDataAdapter(myCommand)
    '            myAdapter.Fill(GetData, "ManufacturingMetricProductionDollarByDept")
    '            GetManufacturingMetricProductionDollarByDept = GetData

    '        Catch ex As Exception

    '            'on error, collect function data, error, and last page, then redirect to error page
    '            Dim strUserEditedData As String = "DeptID: " & DeptID & ", UGNFacility: " & UGNFacility _
    '            & ", StartDate: " & StartDate & ", EndDate: " & EndDate _
    '            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricProductionDollarByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricProductionDollarByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '            GetManufacturingMetricProductionDollarByDept = Nothing
    '        Finally
    '            myConnection.Close()
    '            myCommand.Dispose()
    '        End Try

    '    End Function

    Public Shared Function GetManufacturingMetricSearch(ByVal ReportID As String, ByVal MonthID As Integer, ByVal YearID As Integer, _
    ByVal UGNFacility As String, ByVal StatusID As Integer, ByVal CreatedByTMID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If ReportID Is Nothing Then
                ReportID = ""
            End If

            myCommand.Parameters.Add("@ReportID", SqlDbType.VarChar)
            myCommand.Parameters("@ReportID").Value = ReportID

            myCommand.Parameters.Add("@MonthID", SqlDbType.Int)
            myCommand.Parameters("@MonthID").Value = MonthID

            myCommand.Parameters.Add("@YearID", SqlDbType.Int)
            myCommand.Parameters("@YearID").Value = YearID

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@CreatedByTMID", SqlDbType.Int)
            myCommand.Parameters("@CreatedByTMID").Value = CreatedByTMID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricSearch")
            GetManufacturingMetricSearch = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReportID: " & ReportID & ", MonthID: " & MonthID _
            & ", YearID: " & YearID & ", UGNFacility: " & UGNFacility _
            & ", StatusID: " & StatusID & ", CreatedByTMID: " & CreatedByTMID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricSearch : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricSearch : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricSearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetManufacturingMetricStatusList() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Status_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricStatusList")
            GetManufacturingMetricStatusList = GetData

        Catch ex As Exception

            Dim strUserEditedData As String = ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricStatusList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricStatusList : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricStatusList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetManufacturingMetricDepartment(ByVal UGNFacility As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricDepartment")
            GetManufacturingMetricDepartment = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricDepartment = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
   
    '  Public Shared Function GetManufacturingMetricMiscScrapDollarByDept(ByVal DeptID As Integer, ByVal UGNFacility As String, _
    'ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '      Dim myConnection As SqlConnection = New SqlConnection
    '      Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '      Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Misc_Scrap_Dollar_By_Dept"
    '      Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '      Dim GetData As New DataSet
    '      Dim myAdapter As New SqlDataAdapter

    '      Try
    '          myConnection.ConnectionString = strConnectionString
    '          myCommand.CommandType = CommandType.StoredProcedure

    '          myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '          myCommand.Parameters("@DeptID").Value = DeptID

    '          If UGNFacility Is Nothing Then
    '              UGNFacility = ""
    '          End If

    '          myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '          myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '          If StartDate Is Nothing Then
    '              StartDate = ""
    '          End If

    '          myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '          myCommand.Parameters("@StartDate").Value = StartDate

    '          If EndDate Is Nothing Then
    '              EndDate = ""
    '          End If

    '          myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '          myCommand.Parameters("@EndDate").Value = EndDate

    '          myCommand.Parameters.Add("@OutputMiscScrapDollar", SqlDbType.Decimal)
    '          myCommand.Parameters("@OutputMiscScrapDollar").Value = 0

    '          myAdapter = New SqlDataAdapter(myCommand)
    '          myAdapter.Fill(GetData, "ManufacturingMetricMiscScrapDollarByDept")
    '          GetManufacturingMetricMiscScrapDollarByDept = GetData

    '      Catch ex As Exception

    '          'on error, collect function data, error, and last page, then redirect to error page
    '          Dim strUserEditedData As String = "DeptID: " & DeptID & ", UGNFacility: " & UGNFacility _
    '          & ", StartDate: " & StartDate & ", EndDate: " & EndDate _
    '          & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '          HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricMiscScrapDollarByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '          HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '          UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricMiscScrapDollarByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '          HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '          GetManufacturingMetricMiscScrapDollarByDept = Nothing
    '      Finally
    '          myConnection.Close()
    '          myCommand.Dispose()
    '      End Try

    '  End Function

    'Public Shared Function GetManufacturingMetricMiscScrapQuantityByDept(ByVal DeptID As Integer, ByVal UGNFacility As String, _
    'ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Misc_Scrap_Quantity_By_Dept"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '        myCommand.Parameters("@DeptID").Value = DeptID

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        If StartDate Is Nothing Then
    '            StartDate = ""
    '        End If

    '        myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@StartDate").Value = StartDate

    '        If EndDate Is Nothing Then
    '            EndDate = ""
    '        End If

    '        myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@EndDate").Value = EndDate

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ManufacturingMetricMiscScrapQuantityByDept")
    '        GetManufacturingMetricMiscScrapQuantityByDept = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "DeptID: " & DeptID _
    '        & ", UGNFacility: " & UGNFacility _
    '        & ", StartDate: " & StartDate _
    '        & ", EndDate: " & EndDate _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricMiscScrapQuantityByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricMiscScrapQuantityByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetManufacturingMetricMiscScrapQuantityByDept = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    ' Public Shared Function GetManufacturingMetricScrapDollarByDept(ByVal DeptID As Integer, ByVal UGNFacility As String, _
    'ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '     Dim myConnection As SqlConnection = New SqlConnection
    '     Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '     Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Scrap_Dollar_By_Dept"
    '     Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '     Dim GetData As New DataSet
    '     Dim myAdapter As New SqlDataAdapter

    '     Try
    '         myConnection.ConnectionString = strConnectionString
    '         myCommand.CommandType = CommandType.StoredProcedure

    '         myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '         myCommand.Parameters("@DeptID").Value = DeptID

    '         If UGNFacility Is Nothing Then
    '             UGNFacility = ""
    '         End If

    '         myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '         myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '         If StartDate Is Nothing Then
    '             StartDate = ""
    '         End If

    '         myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '         myCommand.Parameters("@StartDate").Value = StartDate

    '         If EndDate Is Nothing Then
    '             EndDate = ""
    '         End If

    '         myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '         myCommand.Parameters("@EndDate").Value = EndDate

    '         myCommand.Parameters.Add("@OutputTotalScrapDollar", SqlDbType.Decimal)
    '         myCommand.Parameters("@OutputTotalScrapDollar").Value = 0

    '         myAdapter = New SqlDataAdapter(myCommand)
    '         myAdapter.Fill(GetData, "ManufacturingMetricScrapDollarByDept")
    '         GetManufacturingMetricScrapDollarByDept = GetData

    '     Catch ex As Exception

    '         'on error, collect function data, error, and last page, then redirect to error page
    '         Dim strUserEditedData As String = "DeptID: " & DeptID & ", UGNFacility: " & UGNFacility _
    '         & ", StartDate: " & StartDate & ", EndDate: " & EndDate _
    '         & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '         HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricScrapDollarByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '         HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '         UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricScrapDollarByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '         HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '         GetManufacturingMetricScrapDollarByDept = Nothing
    '     Finally
    '         myConnection.Close()
    '         myCommand.Dispose()
    '     End Try

    ' End Function

    'Public Shared Function GetManufacturingMetricScrapQuantityByDept(ByVal DeptID As Integer, ByVal UGNFacility As String, _
    'ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Scrap_Quantity_By_Dept"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '        myCommand.Parameters("@DeptID").Value = DeptID

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        If StartDate Is Nothing Then
    '            StartDate = ""
    '        End If

    '        myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@StartDate").Value = StartDate

    '        If EndDate Is Nothing Then
    '            EndDate = ""
    '        End If

    '        myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@EndDate").Value = EndDate

    '        myCommand.Parameters.Add("@OutputMTDScrapQTY", SqlDbType.Int)
    '        myCommand.Parameters("@OutputMTDScrapQTY").Value = 0

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ManufacturingMetricScrapQuantityByDept")
    '        GetManufacturingMetricScrapQuantityByDept = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "DeptID: " & DeptID _
    '        & ", UGNFacility: " & UGNFacility _
    '        & ", StartDate: " & StartDate _
    '        & ", EndDate: " & EndDate _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricScrapQuantityByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricScrapQuantityByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetManufacturingMetricScrapQuantityByDept = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    'Public Shared Function GetManufacturingMetricStandardManHoursByDept(ByVal DeptID As Integer, _
    'ByVal UGNFacility As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Standard_Man_Hours_By_Dept"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '        myCommand.Parameters("@DeptID").Value = DeptID

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        If StartDate Is Nothing Then
    '            StartDate = ""
    '        End If

    '        myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@StartDate").Value = StartDate

    '        If EndDate Is Nothing Then
    '            EndDate = ""
    '        End If

    '        myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@EndDate").Value = EndDate

    '        myCommand.Parameters.Add("@OutputTotalStandardManHours", SqlDbType.Decimal)
    '        myCommand.Parameters("@OutputTotalStandardManHours").Value = 0

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ManufacturingMetricMachineHourDowntimeAllShiftTotalByDept")
    '        GetManufacturingMetricStandardManHoursByDept = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "DeptID: " & DeptID _
    '        & ", UGNFacility: " & UGNFacility _
    '        & ", StartDate: " & StartDate _
    '        & ", EndDate: " & EndDate _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricStandardManHoursByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricStandardManHoursByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetManufacturingMetricStandardManHoursByDept = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    'Public Shared Function GetManufacturingMetricRawWIPScrapDollarByDept(ByVal DeptID As Integer, _
    '    ByVal UGNFacility As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Raw_WIP_Scrap_Dollar_By_Dept"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '        myCommand.Parameters("@DeptID").Value = DeptID

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        If StartDate Is Nothing Then
    '            StartDate = ""
    '        End If

    '        myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@StartDate").Value = StartDate

    '        If EndDate Is Nothing Then
    '            EndDate = ""
    '        End If

    '        myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@EndDate").Value = EndDate

    '        myCommand.Parameters.Add("@OutputRawWipScrapDollar", SqlDbType.Decimal)
    '        myCommand.Parameters("@OutputRawWipScrapDollar").Value = 0

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "GetManufacturingMetricRawWIPScrapDollarByDept")
    '        GetManufacturingMetricRawWIPScrapDollarByDept = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "DeptID: " & DeptID _
    '        & ", UGNFacility: " & UGNFacility _
    '        & ", StartDate: " & StartDate _
    '        & ", EndDate: " & EndDate _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricRawWIPScrapDollarByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricRawWIPScrapDollarByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetManufacturingMetricRawWIPScrapDollarByDept = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    'Public Shared Function InsertManufacturingMetricHeaderByDept(ByVal MonthID As Integer, ByVal YearID As Integer, _
    '   ByVal UGNFacility As String, ByVal StatusID As Integer, ByVal CreatedByTMID As Integer) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Insert_Manufacturing_Metric_By_Dept"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@MonthID", SqlDbType.Int)
    '        myCommand.Parameters("@MonthID").Value = MonthID

    '        myCommand.Parameters.Add("@YearID", SqlDbType.Int)
    '        myCommand.Parameters("@YearID").Value = YearID

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
    '        myCommand.Parameters("@StatusID").Value = StatusID

    '        myCommand.Parameters.Add("@CreatedByTMID", SqlDbType.Int)
    '        myCommand.Parameters("@CreatedByTMID").Value = CreatedByTMID

    '        myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "NewReportID")
    '        InsertManufacturingMetricHeaderByDept = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "MonthID: " & MonthID _
    '        & ", YearID: " & YearID _
    '        & ", UGNFacility: " & UGNFacility _
    '        & ", StatusID: " & StatusID _
    '        & ", CreatedByTMID: " & CreatedByTMID _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertManufacturingMetricHeaderByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertManufacturingMetricHeaderByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        InsertManufacturingMetricHeaderByDept = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    Public Shared Sub UpdateManufacturingMetricDetailByDept(ByVal ReportID As Integer, ByVal DeptID As Integer, _
        ByVal BudgetOEE As Double, ByVal ActualOEE As Double, _
        ByVal BudgetEarnedDLHours As Double, ByVal ActualEarnedDLHours As Double, _
        ByVal BudgetDLHours As Double, ByVal ActualDLHours As Double, _
        ByVal BudgetDirectOTHours As Double, ByVal ActualDirectOTHours As Double, _
        ByVal BudgetIndirectOTHours As Double, ByVal ActualIndirectOTHours As Double, _
        ByVal BudgetScrap As Double, ByVal ActualScrap As Double, _
        ByVal BudgetTeamMemberContainmentCount As Double, ByVal ActualTeamMemberContainmentCount As Double, _
        ByVal BudgetPartContainmentCount As Double, ByVal ActualPartContainmentCount As Double, _
        ByVal BudgetOffStandardDirectCount As Double, ByVal ActualOffStandardDirectCount As Double, _
        ByVal BudgetOffStandardIndirectCount As Double, ByVal ActualOffStandardIndirectCount As Double, _
        ByVal BudgetIsStandardizedWork As Boolean, ByVal ActualIsStandardizedWork As Boolean, _
        ByVal BudgetTeamMemberFactorCount As Integer, ByVal BudgetTeamLeaderFactorCount As Integer, ByVal BudgetTeamMemberLeaderRatio As String, _
        ByVal ActualTeamMemberFactorCount As Integer, ByVal ActualTeamLeaderFactorCount As Integer, ByVal ActualTeamMemberLeaderRatio As String, _
        ByVal BudgetCapacityUtilization As Double, ByVal ActualCapacityUtilization As Double, _
        ByVal OEEBudgetGoodPartCount As Double, ByVal OEEActualGoodPartCount As Double, _
        ByVal OEEBudgetScrapPartCount As Double, ByVal OEEActualScrapPartCount As Double, _
        ByVal OEEBudgetTotalPartCount As Double, ByVal OEEActualTotalPartCount As Double, _
        ByVal OEEBudgetUtilization As Double, ByVal OEEActualUtilization As Double, _
        ByVal OEEBudgetAvailableHours As Double, ByVal OEEActualAvailableHours As Double, _
        ByVal OEEBudgetDownHours As Double, ByVal OEEActualDownHours As Double, _
        ByVal MonthlyShippingDays As Integer, ByVal HoursPerShift As Double, _
        ByVal BudgetShiftCount As Double, ByVal ActualShiftCount As Double, _
        ByVal AvailablePerShiftFactor As Double, _
        ByVal BudgetDowntimeHours As Double, ByVal ActualDowntimeHours As Double, _
        ByVal BudgetMachineWorkedHours As Double, ByVal ActualMachineWorkedHours As Double, _
        ByVal BudgetMachineAvailableHours As Double, ByVal ActualMachineAvailableHours As Double, _
        ByVal BudgetManWorkedHours As Double, ByVal ActualManWorkedHours As Double, _
        ByVal BudgetDowntimeManHours As Double, ByVal ActualDowntimeManHours As Double, _
        ByVal TotalBudgetProductionDollar As Double, ByVal TotalActualProductionDollar As Double, _
        ByVal TotalBudgetSpecificScrapDollar As Double, ByVal TotalActualSpecificScrapDollar As Double, _
        ByVal TotalBudgetMiscScrapDollar As Double, ByVal TotalActualMiscScrapDollar As Double, _
        ByVal TotalActualIndirectScrapDollar As Double, _
        ByVal BudgetMachineHourStandard As Double, ByVal ActualMachineHourStandard As Double, _
        ByVal BudgetRawWipScrapDollar As Double, ByVal ActualRawWipScrapDollar As Double, _
        ByVal BudgetDirectPerm As Double, ByVal FlexDirectPerm As Double, _
        ByVal ActualDirectPerm As Double, ByVal BudgetDirectTemp As Double, ByVal FlexDirectTemp As Double, _
        ByVal ActualDirectTemp As Double, ByVal BudgetIndirectPerm As Double, ByVal FlexIndirectPerm As Double, _
        ByVal ActualIndirectPerm As Double, ByVal BudgetIndirectTemp As Double, ByVal FlexIndirectTemp As Double, _
        ByVal ActualIndirectTemp As Double, ByVal BudgetOfficeHourlyPerm As Double, ByVal FlexOfficeHourlyPerm As Double, _
        ByVal ActualOfficeHourlyPerm As Double, ByVal BudgetOfficeHourlyTemp As Double, ByVal FlexOfficeHourlyTemp As Double, _
        ByVal ActualOfficeHourlyTemp As Double, ByVal BudgetSalaryPerm As Double, ByVal FlexSalaryPerm As Double, _
        ByVal ActualSalaryPerm As Double, ByVal BudgetSalaryTemp As Double, ByVal FlexSalaryTemp As Double, _
        ByVal ActualSalaryTemp As Double, ByVal Notes As String, ByVal Obsolete As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Manufacturing_Metric_Detail_By_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ReportID", SqlDbType.Int)
            myCommand.Parameters("@ReportID").Value = ReportID

            myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
            myCommand.Parameters("@DeptID").Value = DeptID

            myCommand.Parameters.Add("@BudgetOEE", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetOEE").Value = BudgetOEE

            myCommand.Parameters.Add("@ActualOEE", SqlDbType.Decimal)
            myCommand.Parameters("@ActualOEE").Value = ActualOEE

            myCommand.Parameters.Add("@BudgetEarnedDLHours", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetEarnedDLHours").Value = BudgetEarnedDLHours

            myCommand.Parameters.Add("@ActualEarnedDLHours", SqlDbType.Decimal)
            myCommand.Parameters("@ActualEarnedDLHours").Value = ActualEarnedDLHours

            myCommand.Parameters.Add("@BudgetDLHours", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetDLHours").Value = BudgetDLHours

            myCommand.Parameters.Add("@ActualDLHours", SqlDbType.Decimal)
            myCommand.Parameters("@ActualDLHours").Value = ActualDLHours

            myCommand.Parameters.Add("@BudgetDirectOTHours", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetDirectOTHours").Value = BudgetDirectOTHours

            myCommand.Parameters.Add("@ActualDirectOTHours", SqlDbType.Decimal)
            myCommand.Parameters("@ActualDirectOTHours").Value = ActualDirectOTHours

            myCommand.Parameters.Add("@BudgetIndirectOTHours", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetIndirectOTHours").Value = BudgetIndirectOTHours

            myCommand.Parameters.Add("@ActualIndirectOTHours", SqlDbType.Decimal)
            myCommand.Parameters("@ActualIndirectOTHours").Value = ActualIndirectOTHours

            myCommand.Parameters.Add("@BudgetScrap", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetScrap").Value = BudgetScrap

            myCommand.Parameters.Add("@ActualScrap", SqlDbType.Decimal)
            myCommand.Parameters("@ActualScrap").Value = ActualScrap

            myCommand.Parameters.Add("@BudgetTeamMemberContainmentCount", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetTeamMemberContainmentCount").Value = BudgetTeamMemberContainmentCount

            myCommand.Parameters.Add("@ActualTeamMemberContainmentCount", SqlDbType.Decimal)
            myCommand.Parameters("@ActualTeamMemberContainmentCount").Value = ActualTeamMemberContainmentCount

            myCommand.Parameters.Add("@BudgetPartContainmentCount", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetPartContainmentCount").Value = BudgetPartContainmentCount

            myCommand.Parameters.Add("@ActualPartContainmentCount", SqlDbType.Decimal)
            myCommand.Parameters("@ActualPartContainmentCount").Value = ActualPartContainmentCount

            myCommand.Parameters.Add("@BudgetOffStandardDirectCount", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetOffStandardDirectCount").Value = BudgetOffStandardDirectCount

            myCommand.Parameters.Add("@ActualOffStandardDirectCount", SqlDbType.Decimal)
            myCommand.Parameters("@ActualOffStandardDirectCount").Value = ActualOffStandardDirectCount

            myCommand.Parameters.Add("@BudgetOffStandardIndirectCount", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetOffStandardIndirectCount").Value = BudgetOffStandardIndirectCount

            myCommand.Parameters.Add("@ActualOffStandardIndirectCount", SqlDbType.Decimal)
            myCommand.Parameters("@ActualOffStandardIndirectCount").Value = ActualOffStandardIndirectCount

            myCommand.Parameters.Add("@BudgetIsStandardizedWork", SqlDbType.Bit)
            myCommand.Parameters("@BudgetIsStandardizedWork").Value = BudgetIsStandardizedWork

            myCommand.Parameters.Add("@ActualIsStandardizedWork", SqlDbType.Bit)
            myCommand.Parameters("@ActualIsStandardizedWork").Value = ActualIsStandardizedWork

            myCommand.Parameters.Add("@BudgetTeamMemberFactorCount", SqlDbType.Int)
            myCommand.Parameters("@BudgetTeamMemberFactorCount").Value = BudgetTeamMemberFactorCount

            myCommand.Parameters.Add("@BudgetTeamLeaderFactorCount", SqlDbType.Int)
            myCommand.Parameters("@BudgetTeamLeaderFactorCount").Value = BudgetTeamLeaderFactorCount

            If BudgetTeamMemberLeaderRatio Is Nothing Then
                BudgetTeamMemberLeaderRatio = ""
            End If

            myCommand.Parameters.Add("@BudgetTeamMemberLeaderRatio", SqlDbType.VarChar)
            myCommand.Parameters("@BudgetTeamMemberLeaderRatio").Value = BudgetTeamMemberLeaderRatio

            myCommand.Parameters.Add("@ActualTeamMemberFactorCount", SqlDbType.Int)
            myCommand.Parameters("@ActualTeamMemberFactorCount").Value = ActualTeamMemberFactorCount

            myCommand.Parameters.Add("@ActualTeamLeaderFactorCount", SqlDbType.Int)
            myCommand.Parameters("@ActualTeamLeaderFactorCount").Value = ActualTeamLeaderFactorCount

            If ActualTeamMemberLeaderRatio Is Nothing Then
                ActualTeamMemberLeaderRatio = ""
            End If

            myCommand.Parameters.Add("@ActualTeamMemberLeaderRatio", SqlDbType.VarChar)
            myCommand.Parameters("@ActualTeamMemberLeaderRatio").Value = ActualTeamMemberLeaderRatio

            myCommand.Parameters.Add("@BudgetCapacityUtilization", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetCapacityUtilization").Value = BudgetCapacityUtilization

            myCommand.Parameters.Add("@ActualCapacityUtilization", SqlDbType.Decimal)
            myCommand.Parameters("@ActualCapacityUtilization").Value = ActualCapacityUtilization

            myCommand.Parameters.Add("@OEEBudgetGoodPartCount", SqlDbType.Decimal)
            myCommand.Parameters("@OEEBudgetGoodPartCount").Value = OEEBudgetGoodPartCount

            myCommand.Parameters.Add("@OEEActualGoodPartCount", SqlDbType.Decimal)
            myCommand.Parameters("@OEEActualGoodPartCount").Value = OEEActualGoodPartCount

            myCommand.Parameters.Add("@OEEBudgetScrapPartCount", SqlDbType.Decimal)
            myCommand.Parameters("@OEEBudgetScrapPartCount").Value = OEEBudgetScrapPartCount

            myCommand.Parameters.Add("@OEEActualScrapPartCount", SqlDbType.Decimal)
            myCommand.Parameters("@OEEActualScrapPartCount").Value = OEEActualScrapPartCount

            myCommand.Parameters.Add("@OEEBudgetTotalPartCount", SqlDbType.Decimal)
            myCommand.Parameters("@OEEBudgetTotalPartCount").Value = OEEBudgetTotalPartCount

            myCommand.Parameters.Add("@OEEActualTotalPartCount", SqlDbType.Decimal)
            myCommand.Parameters("@OEEActualTotalPartCount").Value = OEEActualTotalPartCount

            myCommand.Parameters.Add("@OEEBudgetUtilization", SqlDbType.Decimal)
            myCommand.Parameters("@OEEBudgetUtilization").Value = OEEBudgetUtilization

            myCommand.Parameters.Add("@OEEActualUtilization", SqlDbType.Decimal)
            myCommand.Parameters("@OEEActualUtilization").Value = OEEActualUtilization

            myCommand.Parameters.Add("@OEEBudgetAvailableHours", SqlDbType.Decimal)
            myCommand.Parameters("@OEEBudgetAvailableHours").Value = OEEBudgetAvailableHours

            myCommand.Parameters.Add("@OEEActualAvailableHours", SqlDbType.Decimal)
            myCommand.Parameters("@OEEActualAvailableHours").Value = OEEActualAvailableHours

            myCommand.Parameters.Add("@OEEBudgetDownHours", SqlDbType.Decimal)
            myCommand.Parameters("@OEEBudgetDownHours").Value = OEEBudgetDownHours

            myCommand.Parameters.Add("@OEEActualDownHours", SqlDbType.Decimal)
            myCommand.Parameters("@OEEActualDownHours").Value = OEEActualDownHours

            myCommand.Parameters.Add("@MonthlyShippingDays", SqlDbType.Int)
            myCommand.Parameters("@MonthlyShippingDays").Value = MonthlyShippingDays

            myCommand.Parameters.Add("@HoursPerShift", SqlDbType.Decimal)
            myCommand.Parameters("@HoursPerShift").Value = HoursPerShift

            myCommand.Parameters.Add("@BudgetShiftCount", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetShiftCount").Value = BudgetShiftCount

            myCommand.Parameters.Add("@ActualShiftCount", SqlDbType.Decimal)
            myCommand.Parameters("@ActualShiftCount").Value = ActualShiftCount

            myCommand.Parameters.Add("@AvailablePerShiftFactor", SqlDbType.Decimal)
            myCommand.Parameters("@AvailablePerShiftFactor").Value = AvailablePerShiftFactor

            myCommand.Parameters.Add("@BudgetDowntimeHours", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetDowntimeHours").Value = BudgetDowntimeHours

            myCommand.Parameters.Add("@ActualDowntimeHours", SqlDbType.Decimal)
            myCommand.Parameters("@ActualDowntimeHours").Value = ActualDowntimeHours

            myCommand.Parameters.Add("@BudgetMachineWorkedHours", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetMachineWorkedHours").Value = BudgetMachineWorkedHours

            myCommand.Parameters.Add("@ActualMachineWorkedHours", SqlDbType.Decimal)
            myCommand.Parameters("@ActualMachineWorkedHours").Value = ActualMachineWorkedHours

            myCommand.Parameters.Add("@BudgetMachineAvailableHours", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetMachineAvailableHours").Value = BudgetMachineAvailableHours

            myCommand.Parameters.Add("@ActualMachineAvailableHours", SqlDbType.Decimal)
            myCommand.Parameters("@ActualMachineAvailableHours").Value = ActualMachineAvailableHours

            myCommand.Parameters.Add("@BudgetManWorkedHours", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetManWorkedHours").Value = BudgetManWorkedHours

            myCommand.Parameters.Add("@ActualManWorkedHours", SqlDbType.Decimal)
            myCommand.Parameters("@ActualManWorkedHours").Value = ActualManWorkedHours

            myCommand.Parameters.Add("@BudgetDowntimeManHours", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetDowntimeManHours").Value = BudgetDowntimeManHours

            myCommand.Parameters.Add("@ActualDowntimeManHours", SqlDbType.Decimal)
            myCommand.Parameters("@ActualDowntimeManHours").Value = ActualDowntimeManHours

            myCommand.Parameters.Add("@TotalBudgetProductionDollar", SqlDbType.Decimal)
            myCommand.Parameters("@TotalBudgetProductionDollar").Value = TotalBudgetProductionDollar

            myCommand.Parameters.Add("@TotalActualProductionDollar", SqlDbType.Decimal)
            myCommand.Parameters("@TotalActualProductionDollar").Value = TotalActualProductionDollar

            myCommand.Parameters.Add("@TotalBudgetSpecificScrapDollar", SqlDbType.Decimal)
            myCommand.Parameters("@TotalBudgetSpecificScrapDollar").Value = TotalBudgetSpecificScrapDollar

            myCommand.Parameters.Add("@TotalActualSpecificScrapDollar", SqlDbType.Decimal)
            myCommand.Parameters("@TotalActualSpecificScrapDollar").Value = TotalActualSpecificScrapDollar

            myCommand.Parameters.Add("@TotalBudgetMiscScrapDollar", SqlDbType.Decimal)
            myCommand.Parameters("@TotalBudgetMiscScrapDollar").Value = TotalBudgetMiscScrapDollar

            myCommand.Parameters.Add("@TotalActualMiscScrapDollar", SqlDbType.Decimal)
            myCommand.Parameters("@TotalActualMiscScrapDollar").Value = TotalActualMiscScrapDollar

            myCommand.Parameters.Add("@TotalActualIndirectScrapDollar", SqlDbType.Decimal)
            myCommand.Parameters("@TotalActualIndirectScrapDollar").Value = TotalActualIndirectScrapDollar

            myCommand.Parameters.Add("@BudgetMachineHourStandard", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetMachineHourStandard").Value = BudgetMachineHourStandard

            myCommand.Parameters.Add("@ActualMachineHourStandard", SqlDbType.Decimal)
            myCommand.Parameters("@ActualMachineHourStandard").Value = ActualMachineHourStandard

            myCommand.Parameters.Add("@BudgetRawWipScrapDollar", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetRawWipScrapDollar").Value = BudgetRawWipScrapDollar

            myCommand.Parameters.Add("@ActualRawWipScrapDollar", SqlDbType.Decimal)
            myCommand.Parameters("@ActualRawWipScrapDollar").Value = ActualRawWipScrapDollar

            myCommand.Parameters.Add("@BudgetDirectPerm", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetDirectPerm").Value = BudgetDirectPerm

            myCommand.Parameters.Add("@FlexDirectPerm", SqlDbType.Decimal)
            myCommand.Parameters("@FlexDirectPerm").Value = FlexDirectPerm

            myCommand.Parameters.Add("@ActualDirectPerm", SqlDbType.Decimal)
            myCommand.Parameters("@ActualDirectPerm").Value = ActualDirectPerm

            myCommand.Parameters.Add("@BudgetDirectTemp", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetDirectTemp").Value = BudgetDirectTemp

            myCommand.Parameters.Add("@FlexDirectTemp", SqlDbType.Decimal)
            myCommand.Parameters("@FlexDirectTemp").Value = FlexDirectTemp

            myCommand.Parameters.Add("@ActualDirectTemp", SqlDbType.Decimal)
            myCommand.Parameters("@ActualDirectTemp").Value = ActualDirectTemp

            myCommand.Parameters.Add("@BudgetIndirectPerm", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetIndirectPerm").Value = BudgetIndirectPerm

            myCommand.Parameters.Add("@FlexIndirectPerm", SqlDbType.Decimal)
            myCommand.Parameters("@FlexIndirectPerm").Value = FlexIndirectPerm

            myCommand.Parameters.Add("@ActualIndirectPerm", SqlDbType.Decimal)
            myCommand.Parameters("@ActualIndirectPerm").Value = ActualIndirectPerm

            myCommand.Parameters.Add("@BudgetIndirectTemp", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetIndirectTemp").Value = BudgetIndirectTemp

            myCommand.Parameters.Add("@FlexIndirectTemp", SqlDbType.Decimal)
            myCommand.Parameters("@FlexIndirectTemp").Value = FlexIndirectTemp

            myCommand.Parameters.Add("@ActualIndirectTemp", SqlDbType.Decimal)
            myCommand.Parameters("@ActualIndirectTemp").Value = ActualIndirectTemp

            myCommand.Parameters.Add("@BudgetOfficeHourlyPerm", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetOfficeHourlyPerm").Value = BudgetOfficeHourlyPerm

            myCommand.Parameters.Add("@FlexOfficeHourlyPerm", SqlDbType.Decimal)
            myCommand.Parameters("@FlexOfficeHourlyPerm").Value = FlexOfficeHourlyPerm

            myCommand.Parameters.Add("@ActualOfficeHourlyPerm", SqlDbType.Decimal)
            myCommand.Parameters("@ActualOfficeHourlyPerm").Value = ActualOfficeHourlyPerm

            myCommand.Parameters.Add("@BudgetOfficeHourlyTemp", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetOfficeHourlyTemp").Value = BudgetOfficeHourlyTemp

            myCommand.Parameters.Add("@FlexOfficeHourlyTemp", SqlDbType.Decimal)
            myCommand.Parameters("@FlexOfficeHourlyTemp").Value = FlexOfficeHourlyTemp

            myCommand.Parameters.Add("@ActualOfficeHourlyTemp", SqlDbType.Decimal)
            myCommand.Parameters("@ActualOfficeHourlyTemp").Value = ActualOfficeHourlyTemp

            myCommand.Parameters.Add("@BudgetSalaryPerm", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetSalaryPerm").Value = BudgetSalaryPerm

            myCommand.Parameters.Add("@FlexSalaryPerm", SqlDbType.Decimal)
            myCommand.Parameters("@FlexSalaryPerm").Value = FlexSalaryPerm

            myCommand.Parameters.Add("@ActualSalaryPerm", SqlDbType.Decimal)
            myCommand.Parameters("@ActualSalaryPerm").Value = ActualSalaryPerm

            myCommand.Parameters.Add("@BudgetSalaryTemp", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetSalaryTemp").Value = BudgetSalaryTemp

            myCommand.Parameters.Add("@FlexSalaryTemp", SqlDbType.Decimal)
            myCommand.Parameters("@FlexSalaryTemp").Value = FlexSalaryTemp

            myCommand.Parameters.Add("@ActualSalaryTemp", SqlDbType.Decimal)
            myCommand.Parameters("@ActualSalaryTemp").Value = ActualSalaryTemp

            If Notes Is Nothing Then
                Notes = ""
            End If

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = Notes

            myCommand.Parameters.Add("@Obsolete", SqlDbType.Bit)
            myCommand.Parameters("@Obsolete").Value = Obsolete

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReportID: " & ReportID _
            & ", DeptID: " & DeptID _
            & ", BudgetOEE: " & BudgetOEE _
            & ", ActualOEE: " & ActualOEE _
            & ", BudgetEarnedDLHours: " & BudgetEarnedDLHours _
            & ", ActualEarnedDLHours: " & ActualEarnedDLHours _
            & ", BudgetDLHours: " & BudgetDLHours _
            & ", ActualDLHours: " & ActualDLHours _
            & ", BudgetDirectOTHours: " & BudgetDirectOTHours _
            & ", ActualDirectOTHours: " & ActualDirectOTHours _
            & ", BudgetIndirectOTHours: " & BudgetIndirectOTHours _
            & ", ActualIndirectOTHours: " & ActualIndirectOTHours _
            & ", BudgetScrap: " & BudgetScrap _
            & ", ActualScrap: " & ActualScrap _
            & ", BudgetTeamMemberContainmentCount: " & BudgetTeamMemberContainmentCount _
            & ", ActualTeamMemberContainmentCount: " & ActualTeamMemberContainmentCount _
            & ", BudgetPartContainmentCount: " & BudgetPartContainmentCount _
            & ", ActualPartContainmentCount: " & ActualPartContainmentCount _
            & ", BudgetOffStandardDirectCount: " & BudgetOffStandardDirectCount _
            & ", ActualOffStandardDirectCount: " & ActualOffStandardDirectCount _
            & ", BudgetOffStandardIndirectCount: " & BudgetOffStandardIndirectCount _
            & ", ActualOffStandardIndirectCount: " & ActualOffStandardIndirectCount _
            & ", BudgetIsStandardizedWork: " & BudgetIsStandardizedWork _
            & ", ActualIsStandardizedWork: " & ActualIsStandardizedWork _
            & ", BudgetTeamMemberFactorCount: " & BudgetTeamMemberFactorCount _
            & ", BudgetTeamLeaderFactorCount: " & BudgetTeamLeaderFactorCount _
            & ", BudgetTeamMemberLeaderRatio: " & BudgetTeamMemberLeaderRatio _
            & ", ActualTeamMemberFactorCount: " & ActualTeamMemberFactorCount _
            & ", ActualTeamLeaderFactorCount: " & ActualTeamLeaderFactorCount _
            & ", ActualTeamMemberLeaderRatio: " & ActualTeamMemberLeaderRatio _
            & ", BudgetCapacityUtilization: " & BudgetCapacityUtilization _
            & ", ActualCapacityUtilization: " & ActualCapacityUtilization _
            & ", OEEBudgetGoodPartCount: " & OEEBudgetGoodPartCount _
            & ", OEEActualGoodPartCount: " & OEEActualGoodPartCount _
            & ", OEEBudgetScrapPartCount: " & OEEBudgetScrapPartCount _
            & ", OEEActualScrapPartCount: " & OEEActualScrapPartCount _
            & ", OEEBudgetTotalPartCount: " & OEEBudgetTotalPartCount _
            & ", OEEActualTotalPartCount: " & OEEActualTotalPartCount _
            & ", OEEBudgetUtilization: " & OEEBudgetUtilization _
            & ", OEEActualUtilization: " & OEEActualUtilization _
            & ", OEEBudgetAvailableHours : " & OEEBudgetAvailableHours _
            & ", OEEActualAvailableHours : " & OEEActualAvailableHours _
            & ", OEEBudgetDownHours: " & OEEBudgetDownHours _
            & ", OEEActualDownHours: " & OEEActualDownHours _
            & ", MonthlyShippingDays: " & MonthlyShippingDays _
            & ", HoursPerShift: " & HoursPerShift _
            & ", BudgetShiftCount: " & BudgetShiftCount _
            & ", ActualShiftCount: " & ActualShiftCount _
            & ", AvailablePerShiftFactor: " & AvailablePerShiftFactor _
            & ", BudgetDowntimeHours: " & BudgetDowntimeHours _
            & ", ActualDowntimeHours: " & ActualDowntimeHours _
            & ", BudgetMachineWorkedHours: " & BudgetMachineWorkedHours _
            & ", ActualMachineWorkedHours: " & ActualMachineWorkedHours _
            & ", BudgetMachineAvailableHours: " & BudgetMachineAvailableHours _
            & ", ActualMachineAvailableHours: " & ActualMachineAvailableHours _
            & ", BudgetManWorkedHours: " & BudgetManWorkedHours _
            & ", ActualManWorkedHours: " & ActualManWorkedHours _
            & ", BudgetDowntimeManHours: " & BudgetDowntimeManHours _
            & ", ActualDowntimeManHours: " & ActualDowntimeManHours _
            & ", TotalActualProductionDollar: " & TotalActualProductionDollar _
            & ", TotalBudgetProductionDollar: " & TotalBudgetProductionDollar _
            & ", TotalBudgetSpecificScrapDollar: " & TotalBudgetSpecificScrapDollar _
            & ", TotalActualSpecificScrapDollar: " & TotalActualSpecificScrapDollar _
            & ", TotalBudgetMiscScrapDollar: " & TotalBudgetMiscScrapDollar _
            & ", TotalActualMiscScrapDollar: " & TotalActualMiscScrapDollar _
            & ", TotalActualIndirectScrapDollar: " & TotalActualIndirectScrapDollar _
            & ", BudgetMachineHourStandard:" & BudgetMachineHourStandard _
            & ", ActualMachineHourStandard: " & ActualMachineHourStandard _
            & ", BudgetRawWipScrapDollar: " & BudgetRawWipScrapDollar _
            & ", ActualRawWipScrapDollar: " & ActualRawWipScrapDollar _
            & ", BudgetDirectPerm: " & BudgetDirectPerm _
            & ", FlexDirectPerm: " & FlexDirectPerm _
            & ", ActualDirectPerm: " & ActualDirectPerm _
            & ", BudgetDirectTemp: " & BudgetDirectTemp _
            & ", FlexDirectTemp: " & FlexDirectTemp _
            & ", ActualDirectTemp: " & ActualDirectTemp _
            & ", BudgetIndirectPerm: " & BudgetIndirectPerm _
            & ", FlexIndirectPerm: " & FlexIndirectPerm _
            & ", ActualIndirectTemp: " & ActualIndirectTemp _
            & ", BudgetIndirectTemp: " & BudgetIndirectTemp _
            & ", FlexIndirectTemp: " & FlexIndirectTemp _
            & ", ActualIndirectTemp: " & ActualIndirectTemp _
            & ", BudgetOfficeHourlyPerm: " & BudgetOfficeHourlyPerm _
            & ", FlexOfficeHourlyPerm: " & FlexOfficeHourlyPerm _
            & ", ActualOfficeHourlyPerm: " & ActualOfficeHourlyPerm _
            & ", BudgetOfficeHourlyTemp: " & BudgetOfficeHourlyTemp _
            & ", FlexOfficeHourlyTemp: " & FlexOfficeHourlyTemp _
            & ", ActualOfficeHourlyTemp: " & ActualOfficeHourlyTemp _
            & ", BudgetSalaryPerm: " & BudgetSalaryPerm _
            & ", FlexSalaryPerm: " & FlexSalaryPerm _
            & ", ActualSalaryPerm: " & ActualSalaryPerm _
            & ", BudgetSalaryTemp: " & BudgetSalaryTemp _
            & ", FlexSalaryTemp: " & FlexSalaryTemp _
            & ", ActualSalaryTemp: " & ActualSalaryTemp _
            & ", Notes: " & Notes _
            & ", Obsolete: " & Obsolete _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateManufacturingMetricDetailByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateManufacturingMetricDetailByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateManufacturingMetricTotalByDept(ByVal ReportID As Integer, _
       ByVal BudgetOEE As Double, ByVal ActualOEE As Double, _
       ByVal BudgetAllocatedSupportOTHours As Double, ByVal ActualAllocatedSupportOTHours As Double, _
       ByVal BudgetMachineUtilization As Double, ByVal ActualMachineUtilization As Double, _
       ByVal BudgetScrap As Double, ByVal ActualScrap As Double, _
       ByVal BudgetAllocatedSupportTeamMemberContainmentCount As Double, ByVal ActualAllocatedSupportTeamMemberContainmentCount As Double, _
       ByVal BudgetAllocatedSupportPartContainmentCount As Double, ByVal ActualAllocatedSupportPartContainmentCount As Double, _
       ByVal BudgetAllocatedSupportOffStandardIndirectCount As Double, ByVal ActualAllocatedSupportOffStandardIndirectCount As Double, _
       ByVal BudgetIsStandardizedWork As Boolean, ByVal ActualIsStandardizedWork As Boolean, _
       ByVal BudgetTeamMemberLeaderRatio As String, ByVal ActualTeamMemberLeaderRatio As String, _
       ByVal BudgetCapacityUtilization As Double, ByVal ActualCapacityUtilization As Double, _
       ByVal BudgetAllocatedSupportIndirectPerm As Double, ByVal FlexAllocatedSupportIndirectPerm As Double, ByVal ActualAllocatedSupportIndirectPerm As Double, _
       ByVal BudgetAllocatedSupportIndirectTemp As Double, ByVal FlexAllocatedSupportIndirectTemp As Double, ByVal ActualAllocatedSupportIndirectTemp As Double, _
       ByVal BudgetAllocatedSupportOfficeHourlyPerm As Double, ByVal FlexAllocatedSupportOfficeHourlyPerm As Double, ByVal ActualAllocatedSupportOfficeHourlyPerm As Double, _
       ByVal BudgetAllocatedSupportOfficeHourlyTemp As Double, ByVal FlexAllocatedSupportOfficeHourlyTemp As Double, ByVal ActualAllocatedSupportOfficeHourlyTemp As Double, _
       ByVal BudgetAllocatedSupportSalaryPerm As Double, ByVal FlexAllocatedSupportSalaryPerm As Double, ByVal ActualAllocatedSupportSalaryPerm As Double, _
       ByVal BudgetAllocatedSupportSalaryTemp As Double, ByVal FlexAllocatedSupportSalaryTemp As Double, ByVal ActualAllocatedSupportSalaryTemp As Double, _
       ByVal Notes As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Manufacturing_Metric_Total_By_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ReportID", SqlDbType.Int)
            myCommand.Parameters("@ReportID").Value = ReportID

            myCommand.Parameters.Add("@BudgetOEE", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetOEE").Value = BudgetOEE

            myCommand.Parameters.Add("@ActualOEE", SqlDbType.Decimal)
            myCommand.Parameters("@ActualOEE").Value = ActualOEE

            myCommand.Parameters.Add("@BudgetAllocatedSupportOTHours", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetAllocatedSupportOTHours").Value = BudgetAllocatedSupportOTHours

            myCommand.Parameters.Add("@ActualAllocatedSupportOTHours", SqlDbType.Decimal)
            myCommand.Parameters("@ActualAllocatedSupportOTHours").Value = ActualAllocatedSupportOTHours

            myCommand.Parameters.Add("@BudgetMachineUtilization", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetMachineUtilization").Value = BudgetMachineUtilization

            myCommand.Parameters.Add("@ActualMachineUtilization", SqlDbType.Decimal)
            myCommand.Parameters("@ActualMachineUtilization").Value = ActualMachineUtilization

            myCommand.Parameters.Add("@BudgetScrap", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetScrap").Value = BudgetScrap

            myCommand.Parameters.Add("@ActualScrap", SqlDbType.Decimal)
            myCommand.Parameters("@ActualScrap").Value = ActualScrap

            myCommand.Parameters.Add("@BudgetAllocatedSupportTeamMemberContainmentCount", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetAllocatedSupportTeamMemberContainmentCount").Value = BudgetAllocatedSupportTeamMemberContainmentCount

            myCommand.Parameters.Add("@ActualAllocatedSupportTeamMemberContainmentCount", SqlDbType.Decimal)
            myCommand.Parameters("@ActualAllocatedSupportTeamMemberContainmentCount").Value = ActualAllocatedSupportTeamMemberContainmentCount

            myCommand.Parameters.Add("@BudgetAllocatedSupportPartContainmentCount", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetAllocatedSupportPartContainmentCount").Value = BudgetAllocatedSupportPartContainmentCount

            myCommand.Parameters.Add("@ActualAllocatedSupportPartContainmentCount", SqlDbType.Decimal)
            myCommand.Parameters("@ActualAllocatedSupportPartContainmentCount").Value = ActualAllocatedSupportPartContainmentCount

            myCommand.Parameters.Add("@BudgetAllocatedSupportOffStandardIndirectCount", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetAllocatedSupportOffStandardIndirectCount").Value = BudgetAllocatedSupportOffStandardIndirectCount

            myCommand.Parameters.Add("@ActualAllocatedSupportOffStandardIndirectCount", SqlDbType.Decimal)
            myCommand.Parameters("@ActualAllocatedSupportOffStandardIndirectCount").Value = ActualAllocatedSupportOffStandardIndirectCount

            myCommand.Parameters.Add("@BudgetIsStandardizedWork", SqlDbType.Bit)
            myCommand.Parameters("@BudgetIsStandardizedWork").Value = BudgetIsStandardizedWork

            myCommand.Parameters.Add("@ActualIsStandardizedWork", SqlDbType.Bit)
            myCommand.Parameters("@ActualIsStandardizedWork").Value = ActualIsStandardizedWork

            If BudgetTeamMemberLeaderRatio Is Nothing Then
                BudgetTeamMemberLeaderRatio = ""
            End If

            myCommand.Parameters.Add("@BudgetTeamMemberLeaderRatio", SqlDbType.VarChar)
            myCommand.Parameters("@BudgetTeamMemberLeaderRatio").Value = BudgetTeamMemberLeaderRatio

            If ActualTeamMemberLeaderRatio Is Nothing Then
                ActualTeamMemberLeaderRatio = ""
            End If

            myCommand.Parameters.Add("@ActualTeamMemberLeaderRatio", SqlDbType.VarChar)
            myCommand.Parameters("@ActualTeamMemberLeaderRatio").Value = ActualTeamMemberLeaderRatio

            myCommand.Parameters.Add("@BudgetCapacityUtilization", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetCapacityUtilization").Value = BudgetCapacityUtilization

            myCommand.Parameters.Add("@ActualCapacityUtilization", SqlDbType.Decimal)
            myCommand.Parameters("@ActualCapacityUtilization").Value = ActualCapacityUtilization

            myCommand.Parameters.Add("@BudgetAllocatedSupportIndirectPerm", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetAllocatedSupportIndirectPerm").Value = BudgetAllocatedSupportIndirectPerm

            myCommand.Parameters.Add("@FlexAllocatedSupportIndirectPerm", SqlDbType.Decimal)
            myCommand.Parameters("@FlexAllocatedSupportIndirectPerm").Value = FlexAllocatedSupportIndirectPerm

            myCommand.Parameters.Add("@ActualAllocatedSupportIndirectPerm", SqlDbType.Decimal)
            myCommand.Parameters("@ActualAllocatedSupportIndirectPerm").Value = ActualAllocatedSupportIndirectPerm

            myCommand.Parameters.Add("@BudgetAllocatedSupportIndirectTemp", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetAllocatedSupportIndirectTemp").Value = BudgetAllocatedSupportIndirectTemp

            myCommand.Parameters.Add("@FlexAllocatedSupportIndirectTemp", SqlDbType.Decimal)
            myCommand.Parameters("@FlexAllocatedSupportIndirectTemp").Value = FlexAllocatedSupportIndirectTemp

            myCommand.Parameters.Add("@ActualAllocatedSupportIndirectTemp", SqlDbType.Decimal)
            myCommand.Parameters("@ActualAllocatedSupportIndirectTemp").Value = ActualAllocatedSupportIndirectTemp

            myCommand.Parameters.Add("@BudgetAllocatedSupportOfficeHourlyPerm", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetAllocatedSupportOfficeHourlyPerm").Value = BudgetAllocatedSupportOfficeHourlyPerm

            myCommand.Parameters.Add("@FlexAllocatedSupportOfficeHourlyPerm", SqlDbType.Decimal)
            myCommand.Parameters("@FlexAllocatedSupportOfficeHourlyPerm").Value = FlexAllocatedSupportOfficeHourlyPerm

            myCommand.Parameters.Add("@ActualAllocatedSupportOfficeHourlyPerm", SqlDbType.Decimal)
            myCommand.Parameters("@ActualAllocatedSupportOfficeHourlyPerm").Value = ActualAllocatedSupportOfficeHourlyPerm

            myCommand.Parameters.Add("@BudgetAllocatedSupportOfficeHourlyTemp", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetAllocatedSupportOfficeHourlyTemp").Value = BudgetAllocatedSupportOfficeHourlyTemp

            myCommand.Parameters.Add("@FlexAllocatedSupportOfficeHourlyTemp", SqlDbType.Decimal)
            myCommand.Parameters("@FlexAllocatedSupportOfficeHourlyTemp").Value = FlexAllocatedSupportOfficeHourlyTemp

            myCommand.Parameters.Add("@ActualAllocatedSupportOfficeHourlyTemp", SqlDbType.Decimal)
            myCommand.Parameters("@ActualAllocatedSupportOfficeHourlyTemp").Value = ActualAllocatedSupportOfficeHourlyTemp

            myCommand.Parameters.Add("@BudgetAllocatedSupportSalaryPerm", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetAllocatedSupportSalaryPerm").Value = BudgetAllocatedSupportSalaryPerm

            myCommand.Parameters.Add("@FlexAllocatedSupportSalaryPerm", SqlDbType.Decimal)
            myCommand.Parameters("@FlexAllocatedSupportSalaryPerm").Value = FlexAllocatedSupportSalaryPerm

            myCommand.Parameters.Add("@ActualAllocatedSupportSalaryPerm", SqlDbType.Decimal)
            myCommand.Parameters("@ActualAllocatedSupportSalaryPerm").Value = ActualAllocatedSupportSalaryPerm

            myCommand.Parameters.Add("@BudgetAllocatedSupportSalaryTemp", SqlDbType.Decimal)
            myCommand.Parameters("@BudgetAllocatedSupportSalaryTemp").Value = BudgetAllocatedSupportSalaryTemp

            myCommand.Parameters.Add("@FlexAllocatedSupportSalaryTemp", SqlDbType.Decimal)
            myCommand.Parameters("@FlexAllocatedSupportSalaryTemp").Value = FlexAllocatedSupportSalaryTemp

            myCommand.Parameters.Add("@ActualAllocatedSupportSalaryTemp", SqlDbType.Decimal)
            myCommand.Parameters("@ActualAllocatedSupportSalaryTemp").Value = ActualAllocatedSupportSalaryTemp

            If Notes Is Nothing Then
                Notes = ""
            End If

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = Notes

            If HttpContext.Current.Request.Cookies("UGNDB_User").Value Is Nothing Then
                HttpContext.Current.Request.Cookies("UGNDB_User").Value = "test"
            End If

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReportID: " & ReportID _
            & ", BudgetOEE: " & BudgetOEE _
            & ", ActualOEE: " & ActualOEE _
            & ", BudgetAllocatedSupportOTHours: " & BudgetOEE _
            & ", ActualAllocatedSupportOTHours: " & BudgetOEE _
            & ", BudgetMachineUtilization: " & BudgetMachineUtilization _
            & ", ActualMachineUtilization: " & ActualMachineUtilization _
            & ", BudgetScrap: " & BudgetScrap _
            & ", ActualScrap: " & ActualScrap _
            & ", BudgetAllocatedSupportTeamMemberContainmentCount: " & BudgetAllocatedSupportTeamMemberContainmentCount _
            & ", ActualAllocatedSupportTeamMemberContainmentCount: " & ActualAllocatedSupportTeamMemberContainmentCount _
            & ", BudgetAllocatedSupportPartContainmentCount: " & BudgetAllocatedSupportPartContainmentCount _
            & ", ActualAllocatedSupportPartContainmentCount: " & ActualAllocatedSupportPartContainmentCount _
            & ", BudgetAllocatedSupportOffStandardIndirectCount : " & BudgetAllocatedSupportOffStandardIndirectCount _
            & ", ActualAllocatedSupportOffStandardIndirectCount: " & ActualAllocatedSupportOffStandardIndirectCount _
            & ", BudgetIsStandardizedWork: " & BudgetIsStandardizedWork _
            & ", ActualIsStandardizedWork: " & ActualIsStandardizedWork _
            & ", BudgetTeamMemberLeaderRatio: " & BudgetTeamMemberLeaderRatio _
            & ", ActualTeamMemberLeaderRatio: " & ActualTeamMemberLeaderRatio _
            & ", BudgetCapacityUtilization: " & BudgetCapacityUtilization _
            & ", ActualCapacityUtilization: " & ActualCapacityUtilization _
            & ", BudgetAllocatedSupportIndirectPerm: " & BudgetAllocatedSupportIndirectPerm _
            & ", FlexAllocatedSupportIndirectPerm: " & FlexAllocatedSupportIndirectPerm _
            & ", ActualAllocatedSupportIndirectPerm: " & ActualAllocatedSupportIndirectPerm _
            & ", BudgetAllocatedSupportIndirectTemp: " & BudgetAllocatedSupportIndirectTemp _
            & ", FlexAllocatedSupportIndirectTemp: " & FlexAllocatedSupportIndirectTemp _
            & ", ActualAllocatedSupportIndirectTemp: " & ActualAllocatedSupportIndirectTemp _
            & ", BudgetAllocatedSupportOfficeHourlyPerm: " & BudgetAllocatedSupportOfficeHourlyPerm _
            & ", FlexAllocatedSupportOfficeHourlyPerm: " & FlexAllocatedSupportOfficeHourlyPerm _
            & ", ActualAllocatedSupportOfficeHourlyPerm: " & ActualAllocatedSupportOfficeHourlyPerm _
            & ", BudgetAllocatedSupportOfficeHourlyTemp: " & BudgetAllocatedSupportOfficeHourlyTemp _
            & ", FlexAllocatedSupportOfficeHourlyTemp: " & FlexAllocatedSupportOfficeHourlyTemp _
            & ", ActualAllocatedSupportOfficeHourlyTemp: " & ActualAllocatedSupportOfficeHourlyTemp _
            & ", BudgetAllocatedSupportSalaryPerm: " & BudgetAllocatedSupportSalaryPerm _
            & ", FlexAllocatedSupportSalaryPerm: " & FlexAllocatedSupportSalaryPerm _
            & ", ActualAllocatedSupportSalaryPerm: " & ActualAllocatedSupportSalaryPerm _
            & ", BudgetAllocatedSupportSalaryTemp: " & BudgetAllocatedSupportSalaryTemp _
            & ", FlexAllocatedSupportSalaryTemp: " & FlexAllocatedSupportSalaryTemp _
            & ", ActualAllocatedSupportSalaryTemp: " & ActualAllocatedSupportSalaryTemp _
            & ", Notes: " & Notes _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateManufacturingMetricTotalByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateManufacturingMetricTotalByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateManufacturingMetricByDept(ByVal ReportID As Integer, ByVal CreatedByTMID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Manufacturing_Metric_By_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ReportID", SqlDbType.Int)
            myCommand.Parameters("@ReportID").Value = ReportID

            myCommand.Parameters.Add("@CreatedByTMID", SqlDbType.Int)
            myCommand.Parameters("@CreatedByTMID").Value = CreatedByTMID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReportID: " & ReportID _
            & ", CreatedByTMID: " & CreatedByTMID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateManufacturingMetricByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateManufacturingMetricByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateManufacturingMetricStatusByDept(ByVal ReportID As Integer, ByVal StatusID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Manufacturing_Metric_Status_By_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ReportID", SqlDbType.Int)
            myCommand.Parameters("@ReportID").Value = ReportID

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReportID: " & ReportID _
            & ", StatusID: " & StatusID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateManufacturingMetricStatusByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateManufacturingMetricStatusByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetManufacturingMetricMonthList() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Month_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricMonthList")
            GetManufacturingMetricMonthList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricMonthList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricMonthList : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricMonthList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetManufacturingMetricWeeklyReportDateList() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Weekly_Report_Date_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricWeekList")
            GetManufacturingMetricWeeklyReportDateList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricWeekList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricWeekList : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricWeeklyReportDateList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetManufacturingMetricYearList() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Year_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricYearList")
            GetManufacturingMetricYearList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricYearList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricYearList : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricYearList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetManufacturingMetricDailyReportDateList() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Daily_Report_Date_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricDailyReportDateList")
            GetManufacturingMetricDailyReportDateList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricDailyReportDateList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricDailyReportDateList : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricDailyReportDateList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetManufacturingMetricHistory(ByVal ReportID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ReportID", SqlDbType.Int)
            myCommand.Parameters("@ReportID").Value = ReportID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricHistory")
            GetManufacturingMetricHistory = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReportID: " & ReportID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricHistory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricHistory = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub InsertManufacturingMetricHistory(ByVal ReportID As Integer, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Manufacturing_Metric_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ReportID", SqlDbType.Int)
            myCommand.Parameters("@ReportID").Value = ReportID

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            If ActionDesc Is Nothing Then
                ActionDesc = ""
            End If

            myCommand.Parameters.Add("@ActionDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ActionDesc").Value = commonFunctions.convertSpecialChar(ActionDesc, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ReportID: " & ReportID & ", ActionTakenTMID:" & ActionTakenTMID _
            & ", ActionDesc:" & ActionDesc _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertManufacturingMetricHistory : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertManufacturingMetricHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    'Public Shared Function GetManufacturingMetricAvailablePerShiftFactorByDept(ByVal UGNFacility As String, ByVal DeptID As Integer) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Available_Per_Shift_Factor_By_Dept"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        myCommand.Parameters.Add("@DeptID", SqlDbType.Int)
    '        myCommand.Parameters("@DeptID").Value = DeptID

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ManufacturingMetricAvailablePerShiftFactorByDept")
    '        GetManufacturingMetricAvailablePerShiftFactorByDept = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "UGNFacility: " & UGNFacility _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricAvailablePerShiftFactorByDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricAvailablePerShiftFactorByDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetManufacturingMetricAvailablePerShiftFactorByDept = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    Public Shared Function GetManufacturingMetricMiscScrapDollarNoDept(ByVal UGNFacility As String, _
        ByVal StartDate As String, ByVal EndDate As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Manufacturing_Metric_Misc_Scrap_Dollar_No_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            If StartDate Is Nothing Then
                StartDate = ""
            End If

            myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartDate").Value = StartDate

            If EndDate Is Nothing Then
                EndDate = ""
            End If

            myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
            myCommand.Parameters("@EndDate").Value = EndDate

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ManufacturingMetricMiscScrapDollarNoDept")
            GetManufacturingMetricMiscScrapDollarNoDept = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility _
            & ", StartDate: " & StartDate & ", EndDate: " & EndDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricMiscScrapDollarNoDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PSRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricMiscScrapDollarNoDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "PSRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetManufacturingMetricMiscScrapDollarNoDept = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

End Class
