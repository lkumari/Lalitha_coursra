Option Explicit On
Option Strict On
Imports System.Reflection
Imports System.Net
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Data
Imports System.Diagnostics
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Web.HttpContext
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

''' ==============================================================
'''  File:      AcousticModule.vb
''' 
'''  Purpose:   This code is referenced from all vb files, mostly for the purpose 
'''             of calling stored procedures or getting user-rights for the Acoustic Module
'''         
'''  Language:   VB.NET 2005
''' 
'''  Written by: M. Weyker 11/11/2008
''' 
'''  --- Modification History ---
''' 02/05/2009   LRey   Created .Net application
''' 04/13/2011   LRey   Added new function InsertAcousticLabRequestCommodities called from 
'''                     TestIssuanceDetail.aspx.vb RequiredAcouticTesting function
''' 10/17/2012  LRey    Modified to adhere new SP standards
''' 01/31/2014	LRey	Replaced SoldTo|CABBV with a Customer
'''  ==============================================================
''' 

Public Class AcousticModule
    Public Shared Function GetProjectData(ByVal ProjectID As String, ByVal ProjStatus As String, ByVal Customer As String, ByVal ProgramID As Integer, ByVal Requester As Integer, ByVal ReiterRefNo As String, ByVal TestDesc As String, ByVal RequestID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Acoustic_Project_Info"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectID", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectID").Value = ProjectID

            myCommand.Parameters.Add("@ProjStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjStatus").Value = ProjStatus

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@Requester", SqlDbType.Int)
            myCommand.Parameters("@Requester").Value = Requester

            myCommand.Parameters.Add("@ReiterRefNo", SqlDbType.VarChar)
            myCommand.Parameters("@ReiterRefNo").Value = ReiterRefNo

            myCommand.Parameters.Add("@TestDesc", SqlDbType.VarChar)
            myCommand.Parameters("@TestDesc").Value = TestDesc

            myCommand.Parameters.Add("@RequestID", SqlDbType.VarChar)
            myCommand.Parameters("@RequestID").Value = RequestID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ProjectData")

            GetProjectData = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetProjectData : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetProjectData") = "~/Acoustic/Acoustic_Lab_Testing_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProjectData : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetProjectData = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetProjectData

    Public Shared Function GetAcousticPeople(ByVal peopleID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Acoustic_People"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@peopleID", SqlDbType.VarChar)
            myCommand.Parameters("@peopleID").Value = peopleID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "AcousticPeople")

            GetAcousticPeople = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAcousticPeople : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetAcousticPeople") = "~/Acoustic/Acoustic_Project_Detail.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAcousticPeople : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAcousticPeople = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetAcousticPeople

    Public Shared Function GetAcousticStatus(ByVal statusCode As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Acoustic_Project_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add("@statusCode", SqlDbType.VarChar)
            myCommand.Parameters("@statusCode").Value = statusCode
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData)

            GetAcousticStatus = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAcousticStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/ARList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAcousticStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAcousticStatus = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetAcousticStatus

    Public Shared Function GetAcousticTestType(ByVal testID As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Acoustic_Test_Type"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@testID", SqlDbType.VarChar)
            myCommand.Parameters("@testID").Value = testID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData)

            GetAcousticTestType = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAcousticTestType : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/ARList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAcousticTestType : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAcousticTestType = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetAcousticTestType

    Public Shared Function GetLastProjectID(ByVal ProjectStatus As String, ByVal TestDesc As String, ByVal SubmittedBy As Integer, ByVal DateRequested As String, ByVal ProgramID As Integer, ByVal SampleDesc As String, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Last_Acoustic_ProjectID"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@TestDesc", SqlDbType.VarChar)
            myCommand.Parameters("@TestDesc").Value = commonFunctions.replaceSpecialChar(TestDesc, False)

            myCommand.Parameters.Add("@SubmittedBy", SqlDbType.Int)
            myCommand.Parameters("@SubmittedBy").Value = SubmittedBy

            myCommand.Parameters.Add("@DateRequested", SqlDbType.VarChar)
            myCommand.Parameters("@DateRequested").Value = DateRequested

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@SampleDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleDesc").Value = commonFunctions.replaceSpecialChar(SampleDesc, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "AcousticData")

            GetLastProjectID = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetLastProjectID : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetLastProjectID") = "~/Acoustic/Acoustic_Project_detail.aspx"
            UGNErrorTrapping.InsertErrorLog("GetLastProjectID : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLastProjectID = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetLastProjectID

    Public Shared Function GetAcousticProjectCommodity(ByVal ProjectID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Acoustic_Project_Commodities"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectID", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectID").Value = ProjectID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Commodity")

            GetAcousticProjectCommodity = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAcousticProjectCommodity : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetAcousticProjectCommodity") = "~/Acoustic/Acoustic_Lab_Testing_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAcousticProjectCommodity : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAcousticProjectCommodity = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetAcousticProjectCommodity

    Public Shared Function GetAcousticProjectReport(ByVal ProjectID As Integer, ByVal ReportID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Acoustic_Project_Report"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectID", SqlDbType.Int)
            myCommand.Parameters("@ProjectID").Value = ProjectID

            myCommand.Parameters.Add("@ReportID", SqlDbType.Int)
            myCommand.Parameters("@ReportID").Value = ReportID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ProjectReport")

            GetAcousticProjectReport = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAcousticProjectReport : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetAcousticProjectReport") = "~/Acoustic/Acoustic_Project_Detail.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAcousticProjectReport : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAcousticProjectReport = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetAcousticProjectReport

    Public Shared Function InsertAcousticLabRequest(ByVal TestDesc As String, ByVal ProjectStatus As String, ByVal SubmittedBy As Integer, ByVal DateRequested As String, ByVal ProgramID As Integer, ByVal NoOfTestSamples As Integer, ByVal SampleDesc As String, ByVal ProjectGoals As String, ByVal Background As String, ByVal RptReq As Integer, ByVal SpecialInst As String, ByVal DevExp As String, ByVal TestCmpltDt As String, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Acoustic_Lab_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TestDesc", SqlDbType.VarChar)
            myCommand.Parameters("@TestDesc").Value = commonFunctions.replaceSpecialChar(TestDesc, False)

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@SubmittedBy", SqlDbType.Int)
            myCommand.Parameters("@SubmittedBy").Value = SubmittedBy

            myCommand.Parameters.Add("@DateRequested", SqlDbType.VarChar)
            myCommand.Parameters("@DateRequested").Value = DateRequested

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@NoOfTestSamples", SqlDbType.Int)
            myCommand.Parameters("@NoOfTestSamples").Value = NoOfTestSamples

            myCommand.Parameters.Add("@SampleDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleDesc").Value = commonFunctions.replaceSpecialChar(SampleDesc, False)

            myCommand.Parameters.Add("@ProjectGoals", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectGoals").Value = commonFunctions.replaceSpecialChar(ProjectGoals, False)

            myCommand.Parameters.Add("@Background", SqlDbType.VarChar)
            myCommand.Parameters("@Background").Value = commonFunctions.replaceSpecialChar(Background, False)

            myCommand.Parameters.Add("@RptReq", SqlDbType.Int)
            myCommand.Parameters("@RptReq").Value = RptReq

            myCommand.Parameters.Add("@SpecialInst", SqlDbType.VarChar)
            myCommand.Parameters("@SpecialInst").Value = commonFunctions.replaceSpecialChar(SpecialInst, False)

            myCommand.Parameters.Add("@DevExp", SqlDbType.VarChar)
            myCommand.Parameters("@DevExp").Value = commonFunctions.replaceSpecialChar(DevExp, False)

            myCommand.Parameters.Add("@TestCmpltDate", SqlDbType.VarChar)
            myCommand.Parameters("@TestCmpltDate").Value = TestCmpltDt

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewProjData")
            InsertAcousticLabRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertAcousticLabRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertAcousticLabRequest") = "~/Acoustic/Acoustic_Project_Detail.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertAcousticLabRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertAcousticLabRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertAcousticLabRequest

    Public Shared Function InsertAcousticProjectReport(ByVal ProjectID As Integer, ByVal TeamMemberID As Integer, ByVal RptDesc As String, ByVal FileName As String, ByVal EncodeType As String, ByVal BinaryFile As Byte(), ByVal FileSize As Integer, ByVal CreatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Acoustic_Project_Report"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectID", SqlDbType.Int)
            myCommand.Parameters("@ProjectID").Value = ProjectID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@RptDesc", SqlDbType.VarChar)
            myCommand.Parameters("@RptDesc").Value = commonFunctions.replaceSpecialChar(RptDesc, False)

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, False)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ProjectReport")
            InsertAcousticProjectReport = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertAcousticProjectReport : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertAcousticProjectReport") = "~/Acoustic/Acoustic_Project_Detail.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertAcousticProjectReport : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertAcousticProjectReport = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertAcousticProjectReport

    Public Shared Function InsertAcousticLabRequestCommodities(ByVal ProjectID As Integer, ByVal CommodityID As Integer, ByVal CreatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Acoustic_Project_Commodities"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectID", SqlDbType.Int)
            myCommand.Parameters("@ProjectID").Value = ProjectID

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewPCData")

            InsertAcousticLabRequestCommodities = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertAcousticLabRequestCommodities : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertAcousticLabRequestCommodities") = "~/RnD/TestIssuanceList.aspx"""
            UGNErrorTrapping.InsertErrorLog("InsertAcousticLabRequestCommodities : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertAcousticLabRequestCommodities = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertAcousticLabRequestCommodities

    Public Shared Function UpdateAcousticLabRequest(ByVal ProjectID As Integer, ByVal TestDesc As String, ByVal ProjectStatus As String, ByVal SubmittedBy As Integer, ByVal DateRequested As String, ByVal ProgramID As Integer, ByVal NoOfTestSamples As Integer, ByVal SampleDesc As String, ByVal ProjectGoals As String, ByVal Background As String, ByVal RptReq As Integer, ByVal SpecialInst As String, ByVal EngineerID As Integer, ByVal TechnicianID As Integer, ByVal ReiterRefNo As String, ByVal EstCost As Decimal, ByVal ActCost As Decimal, ByVal DevExp As String, ByVal ProjIntDate As String, ByVal EstCmpltDate As String, ByVal ActualCmpltDate As String, ByVal AddInstructions As String, ByVal StatusNotes As String, ByVal TestCmpltDt As String, ByVal UpdatedBy As String, ByVal SubmittedToLab As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Acoustic_Lab_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectID", SqlDbType.Int)
            myCommand.Parameters("@ProjectID").Value = ProjectID

            myCommand.Parameters.Add("@TestDesc", SqlDbType.VarChar)
            myCommand.Parameters("@TestDesc").Value = commonFunctions.replaceSpecialChar(TestDesc, False)

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@SubmittedBy", SqlDbType.Int)
            myCommand.Parameters("@SubmittedBy").Value = SubmittedBy

            myCommand.Parameters.Add("@DateRequested", SqlDbType.VarChar)
            myCommand.Parameters("@DateRequested").Value = DateRequested

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@NoOfTestSamples", SqlDbType.Int)
            myCommand.Parameters("@NoOfTestSamples").Value = NoOfTestSamples

            myCommand.Parameters.Add("@SampleDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleDesc").Value = commonFunctions.replaceSpecialChar(SampleDesc, False)

            myCommand.Parameters.Add("@ProjectGoals", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectGoals").Value = commonFunctions.replaceSpecialChar(ProjectGoals, False)

            myCommand.Parameters.Add("@Background", SqlDbType.VarChar)
            myCommand.Parameters("@Background").Value = commonFunctions.replaceSpecialChar(Background, False)

            myCommand.Parameters.Add("@RptReq", SqlDbType.Int)
            myCommand.Parameters("@RptReq").Value = RptReq

            myCommand.Parameters.Add("@SpecialInst", SqlDbType.VarChar)
            myCommand.Parameters("@SpecialInst").Value = commonFunctions.replaceSpecialChar(SpecialInst, False)

            myCommand.Parameters.Add("@EngineerID", SqlDbType.Int)
            myCommand.Parameters("@EngineerID").Value = EngineerID

            myCommand.Parameters.Add("@TechnicianID", SqlDbType.Int)
            myCommand.Parameters("@TechnicianID").Value = TechnicianID

            myCommand.Parameters.Add("@ReiterRefNo", SqlDbType.VarChar)
            myCommand.Parameters("@ReiterRefNo").Value = commonFunctions.replaceSpecialChar(ReiterRefNo, False)

            myCommand.Parameters.Add("@EstCost", SqlDbType.Decimal)
            myCommand.Parameters("@EstCost").Value = EstCost

            myCommand.Parameters.Add("@ActCost", SqlDbType.Decimal)
            myCommand.Parameters("@ActCost").Value = ActCost

            myCommand.Parameters.Add("@DevExp", SqlDbType.VarChar)
            myCommand.Parameters("@DevExp").Value = commonFunctions.replaceSpecialChar(DevExp, False)

            myCommand.Parameters.Add("@ProjIntDate", SqlDbType.VarChar)
            myCommand.Parameters("@ProjIntDate").Value = ProjIntDate

            myCommand.Parameters.Add("@EstCmpltDate", SqlDbType.VarChar)
            myCommand.Parameters("@EstCmpltDate").Value = EstCmpltDate

            myCommand.Parameters.Add("@ActualCmpltDate", SqlDbType.VarChar)
            myCommand.Parameters("@ActualCmpltDate").Value = ActualCmpltDate

            myCommand.Parameters.Add("@AddInstructions", SqlDbType.VarChar)
            myCommand.Parameters("@AddInstructions").Value = commonFunctions.replaceSpecialChar(AddInstructions, False)

            myCommand.Parameters.Add("@StatusNotes", SqlDbType.VarChar)
            myCommand.Parameters("@StatusNotes").Value = commonFunctions.replaceSpecialChar(StatusNotes, False)

            myCommand.Parameters.Add("@TestCmpltDate", SqlDbType.VarChar)
            myCommand.Parameters("@TestCmpltDate").Value = TestCmpltDt

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@SubmittedToLab", SqlDbType.VarChar)
            myCommand.Parameters("@SubmittedToLab").Value = SubmittedToLab

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateProjData")
            UpdateAcousticLabRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateAcousticLabRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateAcousticLabRequest") = "~/Acoustic/Acoustic_Project_Detail.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAcousticLabRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateAcousticLabRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateAcousticLabRequest

    Public Shared Function DeleteAcousticLabRequest(ByVal ProjectID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Acoustic_Lab_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectID", SqlDbType.Int)
            myCommand.Parameters("@ProjectID").Value = ProjectID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DeleteAcousticData")
            DeleteAcousticLabRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteAcousticLabRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteAcousticLabRequest") = "~/Acoustic/Acoustic_Project_Detail.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAcousticLabRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeleteAcousticLabRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF DeleteAcousticLabRequest

    Public Shared Sub DeleteAcousticCookies()

        Try
            HttpContext.Current.Response.Cookies("AL_ProjNo").Value = ""
            HttpContext.Current.Response.Cookies("AL_ProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AL_ProjStatus").Value = ""
            HttpContext.Current.Response.Cookies("AL_ProjStatus").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AL_CABBV").Value = ""
            HttpContext.Current.Response.Cookies("AL_CABBV").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AL_SoldTo").Value = ""
            HttpContext.Current.Response.Cookies("AL_SoldTo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AL_Program").Value = ""
            HttpContext.Current.Response.Cookies("AL_Program").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AL_Requester").Value = ""
            HttpContext.Current.Response.Cookies("AL_Requester").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AL_ReiterRefNo").Value = ""
            HttpContext.Current.Response.Cookies("AL_ReiterRefNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AL_TestDesc").Value = ""
            HttpContext.Current.Response.Cookies("AL_TestDesc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AL_ReqNo").Value = ""
            HttpContext.Current.Response.Cookies("AL_ReqNo").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteAcousticCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/ARList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAcousticCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF DeleteAcousticCookies

    Public Shared Sub CleanAcousticCrystalReports()

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
            HttpContext.Current.Session("BLLerror") = "CleanAcousticCrystalReports : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> AcousticModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanAcousticCrystalReports : " & commonFunctions.replaceSpecialChar(ex.Message, False), "AcousticModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF CleanAcousticCrystalReports
End Class
