Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Data
Imports System.Diagnostics
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Net
Imports System.Reflection
Imports System.Web.HttpContext
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

''' ==============================================================
'''  File:       RnDModule.vb
''' 
'''  Purpose:    Supplies database access for the programs in
'''              the RnD (Research and Development) module.
'''         
'''  Language:   VB.NET 2005
''' 
'''  Written by: M. Weyker 11/11/2008
''' 
'''  --- Modification History ---
''' 04/12/2011      LRey    Added ReqAcoustic to the Insert/Update Test Issuance Requests functions
''' 04/13/2011      LRey    1) Added new function InsertTestIssuanceAcousticLabRequest called from RequiredAcousticTesting
'''                         function in TestIssuanceDetail.aspx.vb
'''                         2) Added ProjectID to UpdateTestIssuanceRequests 
''' 10/17/2012  LRey    Modified to adhere new SP standards
''' 01/30/2014  LRey    Replaced SoldTo|CABBV with a RowID next sequential. 
'''                     Added CostSheetID per RD-3267 support request.
''' ==============================================================
Public Class RnDModule
    Inherits System.ComponentModel.Component

#Region "Module Level Variables"

    Public Enum RoleType
        Add
        Admin
        Delete
        InquireFull
        InquireRestricted
        Update
    End Enum

    Public Enum ScreenType
        AcousticTesting
        TestIssuance
    End Enum

    Public Enum QueryStringParam
        pCopyId
        pReqCategory
        pReqId
    End Enum

#End Region ' Module Level Variables

#Region "Test Issuance DB Methods"
    Public Shared Function GetTestIssuanceRequests(ByVal RequestID As String, ByVal SampleProdDesc As String, ByVal SampleIssuer As Integer, ByVal UGNFacility As String, ByVal Commodity As Integer, ByVal RequestStatus As String, ByVal PartNo As String, ByVal ReqCat As Integer, ByVal RptID As String, ByVal TAG As String, ByVal ProgramID As Integer, ByVal TestClassID As Integer, ByVal ProjectID As String, ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TestIssuance_Requests"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestID", SqlDbType.VarChar)
            myCommand.Parameters("@RequestID").Value = RequestID

            myCommand.Parameters.Add("@SampleProdDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleProdDesc").Value = IIf(SampleProdDesc = Nothing, "", SampleProdDesc)

            myCommand.Parameters.Add("@SampleIssuer", SqlDbType.Int)
            myCommand.Parameters("@SampleIssuer").Value = SampleIssuer

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@Commodity", SqlDbType.Int)
            myCommand.Parameters("@Commodity").Value = Commodity

            myCommand.Parameters.Add("@ReqStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ReqStatus").Value = RequestStatus

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@ReqCat", SqlDbType.Int)
            myCommand.Parameters("@ReqCat").Value = ReqCat

            myCommand.Parameters.Add("@RptID", SqlDbType.VarChar)
            myCommand.Parameters("@RptID").Value = RptID

            myCommand.Parameters.Add("@TAG", SqlDbType.VarChar)
            myCommand.Parameters("@TAG").Value = TAG

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@TestClassID", SqlDbType.Int)
            myCommand.Parameters("@TestClassID").Value = TestClassID

            myCommand.Parameters.Add("@ProjectID", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectID").Value = ProjectID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TestIssuanceRequestData")

            GetTestIssuanceRequests = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetTestIssuanceRequests : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetTestIssuanceRequests") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTestIssuanceRequests : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTestIssuanceRequests = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetTestIssuanceRequests

    Public Shared Function GetTestReport(ByVal RequestID As Integer, ByVal TestReportID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TestIssuance_TestReport"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestID", SqlDbType.Int)
            myCommand.Parameters("@RequestID").Value = RequestID

            myCommand.Parameters.Add("@TestReportID", SqlDbType.Int)
            myCommand.Parameters("@TestReportID").Value = TestReportID


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TestReport")

            GetTestReport = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetTestReport : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetTestReport") = "~/RnD/TestIssuanceDetail.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTestReport : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTestReport = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetTestReport

    Public Shared Function GetTestRequestPriority(ByVal PriorityDescription As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TestIssuance_Priorities"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PriorityDescription", SqlDbType.VarChar)
            myCommand.Parameters("@PriorityDescription").Value = PriorityDescription

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TestReport")

            GetTestRequestPriority = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetTestRequestPriority : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetTestRequestPriority") = "~/RnD/TestIssuanceDetail.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTestRequestPriority : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTestRequestPriority = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetTestRequestPriority

    Public Shared Function GetLastRequestID(ByVal RequestCategory As Integer, ByVal SampleProdDesc As String, ByVal SampleIssuer As String, ByVal UGNFacility As String, ByVal RequestDt As String, ByVal TestCmpltDt As String, ByVal DescReqTesting As String, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Last_Test_Issuance_RequestID"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestCategory", SqlDbType.Int)
            myCommand.Parameters("@RequestCategory").Value = RequestCategory

            myCommand.Parameters.Add("@SampleProdDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleProdDesc").Value = commonFunctions.convertSpecialChar(Replace(SampleProdDesc, "'", ""), False)

            myCommand.Parameters.Add("@SampleIssuer", SqlDbType.Int)
            myCommand.Parameters("@SampleIssuer").Value = SampleIssuer

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@RequestDt", SqlDbType.VarChar)
            myCommand.Parameters("@RequestDt").Value = RequestDt

            myCommand.Parameters.Add("@TestCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@TestCmpltDt").Value = TestCmpltDt

            myCommand.Parameters.Add("@DescReqTesting", SqlDbType.VarChar)
            myCommand.Parameters("@DescReqTesting").Value = commonFunctions.convertSpecialChar(Replace(DescReqTesting, "'", ""), False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TestIssuanceRequestData")

            GetLastRequestID = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetLastRequestID : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetLastRequestID") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetLastRequestID : " & commonFunctions.convertSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLastRequestID = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetLastRequestID

    Public Shared Function GetLastReportID(ByVal RequestID As Integer, ByVal TeamMemberID As Integer, ByVal TestDesc As String, ByVal Assessment As String, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Last_Test_ReportID"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestID", SqlDbType.Int)
            myCommand.Parameters("@RequestID").Value = RequestID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@TestDesc", SqlDbType.VarChar)
            myCommand.Parameters("@TestDesc").Value = commonFunctions.replaceSpecialChar(TestDesc, False)

            myCommand.Parameters.Add("@Assessment", SqlDbType.VarChar)
            myCommand.Parameters("@Assessment").Value = commonFunctions.replaceSpecialChar(Assessment, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ReportID")

            GetLastReportID = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetLastReportID : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetLastReportID") = "~/RnD/TestIssuanceDetail.aspx"
            UGNErrorTrapping.InsertErrorLog("GetLastReportID : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLastReportID = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetLastReportID

    Public Shared Function GetTestIssuanceCustomerPart(ByVal RequestID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TestIssuance_CustomerPart"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestID", SqlDbType.Int)
            myCommand.Parameters("@RequestID").Value = RequestID

            'myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            'myCommand.Parameters("@CABBV").Value = Nothing

            'myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            'myCommand.Parameters("@ProgramID").Value = 0

            'myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            'myCommand.Parameters("@PartNo").Value = Nothing

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CustomerPart")

            GetTestIssuanceCustomerPart = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetTestIssuanceCustomerPart : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetTestIssuanceCustomerPart") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTestIssuanceCustomerPart : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTestIssuanceCustomerPart = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetTestIssuanceCustomerPart

    Public Shared Function GetTestIssuanceAssignments(ByVal RequestID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TestIssuance_Assignments"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestID", SqlDbType.Int)
            myCommand.Parameters("@RequestID").Value = RequestID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Assignments")

            GetTestIssuanceAssignments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetTestIssuanceAssignments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetTestIssuanceAssignments") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTestIssuanceAssignments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTestIssuanceAssignments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetTestIssuanceAssignments

    Public Shared Function GetTestingClassification(ByVal TestClassName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Testing_Classification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TestClassName", SqlDbType.VarChar)
            myCommand.Parameters("@TestClassName").Value = TestClassName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TestClass")

            GetTestingClassification = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetTestingClassification : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetTestingClassification") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTestingClassification : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTestingClassification = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetTestingClassification

    Public Shared Function InsertTestIssuanceRequests(ByVal RequestCategory As Integer, ByVal SampleProdDesc As String, ByVal CommodityID As Integer, ByVal PurchasedGoodID As Integer, ByVal GeneralThickness As String, ByVal UGNFacility As String, ByVal SampleQuantity As String, ByVal SampleIssuer As Integer, ByVal Department As String, ByVal RequestDt As String, ByVal TestCmpltDt As String, ByVal PartAppMkt As String, ByVal ObjPerfTargets As String, ByVal MiscAgenda As String, ByVal DescReqTesting As String, ByVal Formula As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String, ByVal ReqAcoustic As Boolean, ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Test_Issuance_Requests"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestCategory", SqlDbType.Int)
            myCommand.Parameters("@RequestCategory").Value = RequestCategory

            myCommand.Parameters.Add("@SampleProdDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleProdDesc").Value = commonFunctions.replaceSpecialChar(SampleProdDesc, False)

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@PurchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@PurchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@GeneralThickness", SqlDbType.VarChar)
            myCommand.Parameters("@GeneralThickness").Value = commonFunctions.replaceSpecialChar(GeneralThickness, False)

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@SampleQuantity", SqlDbType.VarChar)
            myCommand.Parameters("@SampleQuantity").Value = commonFunctions.replaceSpecialChar(SampleQuantity, False)

            myCommand.Parameters.Add("@SampleIssuer", SqlDbType.Int)
            myCommand.Parameters("@SampleIssuer").Value = SampleIssuer

            myCommand.Parameters.Add("@Department", SqlDbType.VarChar)
            myCommand.Parameters("@Department").Value = commonFunctions.replaceSpecialChar(Department, False)

            myCommand.Parameters.Add("@RequestDt", SqlDbType.VarChar)
            myCommand.Parameters("@RequestDt").Value = RequestDt

            myCommand.Parameters.Add("@TestCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@TestCmpltDt").Value = TestCmpltDt

            myCommand.Parameters.Add("@PartAppMkt", SqlDbType.VarChar)
            myCommand.Parameters("@PartAppMkt").Value = commonFunctions.replaceSpecialChar(PartAppMkt, False)

            myCommand.Parameters.Add("@ObjPerfTargets", SqlDbType.VarChar)
            myCommand.Parameters("@ObjPerfTargets").Value = commonFunctions.replaceSpecialChar(ObjPerfTargets, False)

            myCommand.Parameters.Add("@MiscAgenda", SqlDbType.VarChar)
            myCommand.Parameters("@MiscAgenda").Value = commonFunctions.replaceSpecialChar(MiscAgenda, False)

            myCommand.Parameters.Add("@DescReqTesting", SqlDbType.VarChar)
            myCommand.Parameters("@DescReqTesting").Value = commonFunctions.replaceSpecialChar(DescReqTesting, False)

            myCommand.Parameters.Add("@FormulaID", SqlDbType.Int)
            myCommand.Parameters("@FormulaID").Value = Formula

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myCommand.Parameters.Add("@ReqAcoustic", SqlDbType.Bit)
            myCommand.Parameters("@ReqAcoustic").Value = ReqAcoustic

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewTestData")
            InsertTestIssuanceRequests = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertTestIssuanceRequests : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertTestIssuanceRequests") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTestIssuanceRequests : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertTestIssuanceRequests = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertTestIssuanceRequests

    Public Shared Function InsertTestIssuanceCustomerPart(ByVal RequestID As Integer, ByVal ProgramID As Integer, ByVal PartNo As String, ByVal DesignLevel As String, ByVal DrawingNo As String, ByVal CustomerSpecNo As String, ByVal LotNo As String, ByVal MfgDt As String, ByVal ECINo As Integer, ByVal CostSheetID As Integer, ByVal CreatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_TestIssuance_CustomerPart"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestID", SqlDbType.Int)
            myCommand.Parameters("@RequestID").Value = RequestID

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@DesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@DesignLevel").Value = commonFunctions.replaceSpecialChar(DesignLevel, False)

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = commonFunctions.replaceSpecialChar(DrawingNo, False)

            myCommand.Parameters.Add("@CustomerSpecNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerSpecNo").Value = commonFunctions.replaceSpecialChar(CustomerSpecNo, False)

            myCommand.Parameters.Add("@LotNo", SqlDbType.VarChar)
            myCommand.Parameters("@LotNo").Value = commonFunctions.replaceSpecialChar(LotNo, False)

            myCommand.Parameters.Add("@MfgDt", SqlDbType.VarChar)
            myCommand.Parameters("@MfgDt").Value = MfgDt

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewTestData")
            InsertTestIssuanceCustomerPart = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertTestIssuanceCustomerPart : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertTestIssuanceCustomerPart") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTestIssuanceCustomerPart : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertTestIssuanceCustomerPart = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertTestIssuanceCustomerPart

    Public Shared Function InsertTestReport(ByVal RequestID As Integer, ByVal TeamMemberID As Integer, ByVal TestDesc As String, ByVal Assessment As String, ByVal FileName As String, ByVal EncodeType As String, ByVal BinaryFile As Byte(), ByVal FileSize As Integer, ByVal CreatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_TestIssuance_TestReport"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestID", SqlDbType.Int)
            myCommand.Parameters("@RequestID").Value = RequestID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@TestDesc", SqlDbType.VarChar)
            myCommand.Parameters("@TestDesc").Value = commonFunctions.replaceSpecialChar(TestDesc, False)

            myCommand.Parameters.Add("@Assessment", SqlDbType.VarChar)
            myCommand.Parameters("@Assessment").Value = commonFunctions.replaceSpecialChar(Assessment, False)

            'myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            'myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, False)

            'myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            'myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            'myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            'myCommand.Parameters("@BinaryFile").Value = BinaryFile

            'myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            'myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TestReport")
            InsertTestReport = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertTestReport : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertTestReport") = "~/RnD/TestIssuanceDetail.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTestReport : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertTestReport = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertTestReport

    Public Shared Function InsertTestIssuanceAcousticLabRequest(ByVal RequestID As Integer, ByVal TestDescription As String, ByVal SubmittedBy As Integer, ByVal DateRequested As String, ByVal ProjectStatus As String, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Test_Issuance_Acoustic_Lab_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestID", SqlDbType.Int)
            myCommand.Parameters("@RequestID").Value = RequestID

            myCommand.Parameters.Add("@TestDescription", SqlDbType.VarChar)
            myCommand.Parameters("@TestDescription").Value = commonFunctions.replaceSpecialChar(TestDescription, False)

            myCommand.Parameters.Add("@SubmittedBy", SqlDbType.Int)
            myCommand.Parameters("@SubmittedBy").Value = SubmittedBy

            myCommand.Parameters.Add("@DateRequested", SqlDbType.VarChar)
            myCommand.Parameters("@DateRequested").Value = DateRequested

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewAcousticData")

            InsertTestIssuanceAcousticLabRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertTestIssuanceAcousticLabRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertTestIssuanceAcousticLabRequest") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTestIssuanceAcousticLabRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertTestIssuanceAcousticLabRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertTestIssuanceAcousticLabRequest

    Public Shared Function UpdateTestIssuanceRequests(ByVal RequestID As Integer, ByVal RequestCategory As Integer, ByVal RequestStatus As String, ByVal SampleProdDesc As String, ByVal CommodityID As Integer, ByVal PurchasedGoodID As Integer, ByVal GeneralThickness As String, ByVal UGNFacility As String, ByVal SampleQuantity As String, ByVal SampleIssuer As Integer, ByVal Department As String, ByVal RequestDt As String, ByVal TestCmpltDt As String, ByVal PartAppMkt As String, ByVal ObjPerfTargets As String, ByVal MiscAgenda As String, ByVal DescReqTesting As String, ByVal Formula As Integer, ByVal SentToRnD As String, ByVal Objective As String, ByVal StatusNotes As String, ByVal EstManHrs As Decimal, ByVal DrawReview As String, ByVal TestClassID As Integer, ByVal LongAgingCycle As Integer, ByVal TAG As String, ByVal StartDate As String, ByVal ProjCmplDt As String, ByVal ActCmplDt As String, ByVal EstAnnualCostSavings As Decimal, ByVal PriorityID As Integer, ByVal UpdatedBy As String, ByVal ReqAcoustic As Boolean, ByVal ProjectID As Integer, ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Test_Issuance_Requests"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestID", SqlDbType.Int)
            myCommand.Parameters("@RequestID").Value = RequestID

            myCommand.Parameters.Add("@RequestCategory", SqlDbType.Int)
            myCommand.Parameters("@RequestCategory").Value = RequestCategory

            myCommand.Parameters.Add("@RequestStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RequestStatus").Value = RequestStatus

            myCommand.Parameters.Add("@SampleProdDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleProdDesc").Value = commonFunctions.convertSpecialChar(SampleProdDesc, False)

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@PurchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@PurchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@GeneralThickness", SqlDbType.VarChar)
            myCommand.Parameters("@GeneralThickness").Value = commonFunctions.convertSpecialChar(GeneralThickness, False)

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@SampleQuantity", SqlDbType.VarChar)
            myCommand.Parameters("@SampleQuantity").Value = commonFunctions.convertSpecialChar(SampleQuantity, False)

            myCommand.Parameters.Add("@SampleIssuer", SqlDbType.Int)
            myCommand.Parameters("@SampleIssuer").Value = SampleIssuer

            myCommand.Parameters.Add("@Department", SqlDbType.VarChar)
            myCommand.Parameters("@Department").Value = commonFunctions.convertSpecialChar(Department, False)

            myCommand.Parameters.Add("@RequestDt", SqlDbType.VarChar)
            myCommand.Parameters("@RequestDt").Value = RequestDt

            myCommand.Parameters.Add("@TestCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@TestCmpltDt").Value = TestCmpltDt

            myCommand.Parameters.Add("@PartAppMkt", SqlDbType.VarChar)
            myCommand.Parameters("@PartAppMkt").Value = commonFunctions.convertSpecialChar(PartAppMkt, False)

            myCommand.Parameters.Add("@ObjPerfTargets", SqlDbType.VarChar)
            myCommand.Parameters("@ObjPerfTargets").Value = commonFunctions.convertSpecialChar(ObjPerfTargets, False)

            myCommand.Parameters.Add("@MiscAgenda", SqlDbType.VarChar)
            myCommand.Parameters("@MiscAgenda").Value = commonFunctions.convertSpecialChar(MiscAgenda, False)

            myCommand.Parameters.Add("@DescReqTesting", SqlDbType.VarChar)
            myCommand.Parameters("@DescReqTesting").Value = commonFunctions.convertSpecialChar(DescReqTesting, False)

            myCommand.Parameters.Add("@FormulaID", SqlDbType.Int)
            myCommand.Parameters("@FormulaID").Value = Formula

            myCommand.Parameters.Add("@SentToRnD", SqlDbType.VarChar)
            myCommand.Parameters("@SentToRnD").Value = SentToRnD

            myCommand.Parameters.Add("@Objective", SqlDbType.VarChar)
            myCommand.Parameters("@Objective").Value = commonFunctions.convertSpecialChar(Objective, False)

            myCommand.Parameters.Add("@StatusNotes", SqlDbType.VarChar)
            myCommand.Parameters("@StatusNotes").Value = commonFunctions.convertSpecialChar(StatusNotes, False)

            myCommand.Parameters.Add("@EstManHrs", SqlDbType.Decimal)
            myCommand.Parameters("@EstManHrs").Value = EstManHrs

            myCommand.Parameters.Add("@DrawReview", SqlDbType.VarChar)
            myCommand.Parameters("@DrawReview").Value = DrawReview

            myCommand.Parameters.Add("@TestClassID", SqlDbType.Int)
            myCommand.Parameters("@TestClassID").Value = TestClassID

            myCommand.Parameters.Add("@LongAgingCycle", SqlDbType.Int)
            myCommand.Parameters("@LongAgingCycle").Value = LongAgingCycle

            myCommand.Parameters.Add("@TAG", SqlDbType.VarChar)
            myCommand.Parameters("@TAG").Value = commonFunctions.convertSpecialChar(TAG, False)

            myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartDate").Value = StartDate

            myCommand.Parameters.Add("@ProjCmplDt", SqlDbType.VarChar)
            myCommand.Parameters("@ProjCmplDt").Value = ProjCmplDt

            myCommand.Parameters.Add("@ActCmplDt", SqlDbType.VarChar)
            myCommand.Parameters("@ActCmplDt").Value = ActCmplDt

            myCommand.Parameters.Add("@EstAnnualCostSavings", SqlDbType.Decimal)
            myCommand.Parameters("@EstAnnualCostSavings").Value = EstAnnualCostSavings

            myCommand.Parameters.Add("@PriorityID", SqlDbType.Int)
            myCommand.Parameters("@PriorityID").Value = PriorityID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@ReqAcoustic", SqlDbType.Bit)
            myCommand.Parameters("@ReqAcoustic").Value = ReqAcoustic

            myCommand.Parameters.Add("@ProjectID", SqlDbType.Int)
            myCommand.Parameters("@ProjectID").Value = ProjectID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateTestData")
            UpdateTestIssuanceRequests = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateTestIssuanceRequests : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateTestIssuanceRequests") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTestIssuanceRequests : " & commonFunctions.convertSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateTestIssuanceRequests = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateTestIssuanceRequests

    Public Shared Function UpdateTestIssuanceAssignments(ByVal RequestID As Integer, ByVal UpdatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Test_Issuance_Assignments"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestID", SqlDbType.Int)
            myCommand.Parameters("@RequestID").Value = RequestID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateTMAssignments")
            UpdateTestIssuanceAssignments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateTestIssuanceAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateTestIssuanceAssignments") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTestIssuanceAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateTestIssuanceAssignments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateTestIssuanceAssignments

    Public Shared Function UpdateTestReport(ByVal TestReportID As Integer, ByVal RequestID As Integer, ByVal TeamMemberID As Integer, ByVal TestDesc As String, ByVal Assessment As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer, ByVal UpdatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_TestIssuance_TestReport"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TestReportID", SqlDbType.Int)
            myCommand.Parameters("@TestReportID").Value = TestReportID

            myCommand.Parameters.Add("@RequestID", SqlDbType.Int)
            myCommand.Parameters("@RequestID").Value = RequestID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@TestDesc", SqlDbType.VarChar)
            myCommand.Parameters("@TestDesc").Value = commonFunctions.replaceSpecialChar(TestDesc, False)

            myCommand.Parameters.Add("@Assessment", SqlDbType.VarChar)
            myCommand.Parameters("@Assessment").Value = commonFunctions.replaceSpecialChar(Assessment, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, False)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TestReport")
            UpdateTestReport = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateTestReport : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateTestReport") = "~/RnD/TestIssuanceDetail.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTestReport : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateTestReport = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateTestReport


    Public Shared Function DeleteTestIssuanceRequests(ByVal RequestID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Test_Issuance_Requests"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestID", SqlDbType.Int)
            myCommand.Parameters("@RequestID").Value = RequestID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DeleteTestData")
            DeleteTestIssuanceRequests = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteTestIssuanceRequests : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteTestIssuanceRequests") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTestIssuanceRequests : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeleteTestIssuanceRequests = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF DeleteTestIssuanceRequests

    Public Shared Sub DeleteTestIssuanceCookies()

        Try
            HttpContext.Current.Response.Cookies("TI_RequestID").Value = ""
            HttpContext.Current.Response.Cookies("TI_RequestID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_SampleProdDesc").Value = ""
            HttpContext.Current.Response.Cookies("TI_SampleProdDesc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_SampleIssuer").Value = ""
            HttpContext.Current.Response.Cookies("TI_SampleIssuer").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_UGNLocation").Value = ""
            HttpContext.Current.Response.Cookies("TI_UGNLocation").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_Commodity").Value = ""
            HttpContext.Current.Response.Cookies("TI_Commodity").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_RequestStatus").Value = ""
            HttpContext.Current.Response.Cookies("TI_RequestStatus").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_PNO").Value = ""
            HttpContext.Current.Response.Cookies("TI_PNO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_ReqCat").Value = ""
            HttpContext.Current.Response.Cookies("TI_ReqCat").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_RptID").Value = ""
            HttpContext.Current.Response.Cookies("TI_RptID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_TAG").Value = ""
            HttpContext.Current.Response.Cookies("TI_TAG").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_ProgramID").Value = ""
            HttpContext.Current.Response.Cookies("TI_ProgramID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_TestClassID").Value = ""
            HttpContext.Current.Response.Cookies("TI_TestClassID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_ProjectID").Value = ""
            HttpContext.Current.Response.Cookies("TI_ProjectID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("TI_ProjectNo").Value = ""
            HttpContext.Current.Response.Cookies("TI_ProjectNo").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteTestIssuanceCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> TIL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RnD/TestIssuanceList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTestIssuanceCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "TIL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF DeleteTestIssuanceCookies

    Public Shared Sub DeleteLabRequestMatrixCookies()

        Try
            HttpContext.Current.Response.Cookies("LM_RequestID").Value = ""
            HttpContext.Current.Response.Cookies("LM_RequestID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("LM_ReqDtFrom").Value = ""
            HttpContext.Current.Response.Cookies("LM_ReqDtFrom").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("LM_ReqDtTo").Value = ""
            HttpContext.Current.Response.Cookies("LM_ReqDtTo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("LM_UGNFacility").Value = ""
            HttpContext.Current.Response.Cookies("LM_UGNFacility").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("LM_ReqStatus").Value = ""
            HttpContext.Current.Response.Cookies("LM_ReqStatus").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("LM_TestClass").Value = ""
            HttpContext.Current.Response.Cookies("LM_TestClass").Expires = DateTime.Now.AddDays(-1)


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteLabRequestMatrixCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> TIL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RnD/LabRequestMatrix.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteLabRequestMatrixCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "TIL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteLabRequestMatrixCookies
    Public Shared Sub DeleteCostSavingsReportCookies()

        Try
            HttpContext.Current.Response.Cookies("RDCS_ReqDtFrom").Value = ""
            HttpContext.Current.Response.Cookies("RDCS_ReqDtFrom").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RDCS_ReqDtTo").Value = ""
            HttpContext.Current.Response.Cookies("RDCS_ReqDtTo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RDCS_UGNFacility").Value = ""
            HttpContext.Current.Response.Cookies("RDCS_UGNFacility").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RDCS_TestClass").Value = ""
            HttpContext.Current.Response.Cookies("RDCS_TestClass").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RDCS_ReqStatus").Value = ""
            HttpContext.Current.Response.Cookies("RDCS_ReqStatus").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostSavingsReportCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> TIL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RnD/CostSavingsReport.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSavingsReportCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "TIL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteCostSavingsReportCookies
    Public Shared Sub CleanRnDCrystalReports()

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
            HttpContext.Current.Session("BLLerror") = "CleanRnDCrystalReports : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> RnDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanRnDCrystalReports : " & commonFunctions.replaceSpecialChar(ex.Message, False), "RnDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF CleanRnDCrystalReports
#End Region ' Test Issuance DB Methods


#Region "Role Based Security Methods"

    Public Shared Function HasRole( _
        ByVal userScreen As ScreenType, ByVal userRole As RoleType) As Boolean
        ''*******
        '' Determine if the user has rights that are specified by 
        ''  ScreenType and RoleType.
        ''*******
        Dim bln As Boolean = False
        Try
            ''********
            '' First, check the session variable
            ''********
            Dim strNameOfVariable As String = userScreen.ToString & userRole.ToString
            Dim obj As New Object
            obj = Current.Session(strNameOfVariable)
            If (obj IsNot Nothing) Then
                bln = CType(obj, Boolean)
            Else
                ''********
                '' The session variable has not been set.
                '' Lookup the security in the database, and
                ''  set the session variable.
                ''********
                bln = HasRoleInDatabase( _
                    HyperlinkID:="%" & userScreen.ToString & "%", userRole:=userRole)
                Current.Session(strNameOfVariable) = bln.ToString
            End If
        Catch ex As Exception
            '------------------------------------------------------------
            ' Get current method and class name and calling web page
            '------------------------------------------------------------
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod
            Dim strMyMethodInfo As String = myMethod.DeclaringType.Name & "." & myMethod.Name & "()"
            Dim strCallingPage As String = System.Web.HttpContext.Current.Request.Url.AbsolutePath
            '-----------------------------
            ' log and email error
            '-----------------------------
            Dim strMethodParms As String = "userScreen: " & userScreen.ToString & Environment.NewLine
            strMethodParms &= "userRole: " & userRole.ToString & Environment.NewLine
            UGNErrorTrapping.InsertErrorLog(ex.Message, _
                strCallingPage, strMyMethodInfo & " -> " & strMethodParms)
        End Try
        Return bln
    End Function ' HasRole

    Private Shared Function HasRoleInDatabase( _
        ByVal HyperlinkID As String, ByVal userRole As RoleType) As Boolean
        ''********
        '' Check the database to determine if the user has the required role.
        ''********
        Dim bln As Boolean = False
        Try
            ''********
            '' Get user's full name.
            '' Lookup the user in the database.
            ''********
            Dim strUserName As String = commonFunctions.getUserName()
            Dim ds As DataSet = SecurityModule.GetTMSecurity( _
                TeamMemberID:=Nothing, UserName:=strUserName, _
                RoleID:=Nothing, RoleName:=Nothing, _
                FormID:=Nothing, FormName:=Nothing, HyperlinkID:=HyperlinkID)
            If ((ds IsNot Nothing) AndAlso (ds.Tables.Count > 0)) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim strRoleName As String = dr.Item("RoleName").ToString
                    Select Case userRole
                        Case RoleType.Add
                            ''*************
                            '' Role: Add
                            ''*************
                            Select Case strRoleName
                                Case "UGNAdmin", "UGNAssist", "UGNChampion", "UGNDeveloper"
                                    bln = True
                                    Exit For
                                Case Else
                                    ' DO NOTHING
                            End Select
                        Case RoleType.Admin
                            ''*************
                            '' Role: Admin
                            ''*************
                            Select Case strRoleName
                                Case "UGNAdmin", "UGNDeveloper"
                                    bln = True
                                    Exit For
                                Case Else
                                    ' DO NOTHING
                            End Select
                        Case RoleType.Delete
                            ''**************
                            '' Role: Delete
                            ''**************
                            Select Case strRoleName
                                Case "UGNAdmin, UGNChampion", "UGNDeveloper"
                                    bln = True
                                    Exit For
                                Case Else
                                    ' DO NOTHING
                            End Select
                        Case RoleType.InquireFull
                            ''**********************
                            '' Role: Inquire (Full)
                            ''**********************
                            Select Case strRoleName
                                Case "UGNAdmin", "UGNAssist", "UGNChampion", _
                                    "UGNDeveloper", "UGNEdit", "UGNReadOnly"
                                    bln = True
                                    Exit For
                                Case Else
                                    ' DO NOTHING
                            End Select
                        Case RoleType.InquireRestricted
                            ''****************************
                            '' Role: Inquire (Restricted)
                            ''****************************
                            Select Case strRoleName
                                Case "UGNAdmin", "UGNAssist", "UGNChampion", _
                                    "UGNDeveloper", "UGNEdit", "UGNReadOnly", _
                                    "UGNReadOnly_Restriction"
                                    bln = True
                                    Exit For
                                Case Else
                                    ' DO NOTHING
                            End Select
                        Case RoleType.Update
                            ''**************
                            '' Role: Update
                            ''**************
                            Select Case strRoleName
                                Case "UGNAdmin", "UGNAssist", "UGNChampion", "UGNDeveloper", "UGNEdit"
                                    bln = True
                                    Exit For
                                Case Else
                                    ' DO NOTHING
                            End Select
                        Case Else
                            ' DO NOTHING
                    End Select
                Next
            End If
        Catch ex As Exception
            '------------------------------------------------------------
            ' Get current method and class name and calling web page
            '------------------------------------------------------------
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod
            Dim strMyMethodInfo As String = myMethod.DeclaringType.Name & "." & myMethod.Name & "()"
            Dim strCallingPage As String = System.Web.HttpContext.Current.Request.Url.AbsolutePath
            '-----------------------------
            ' log and email error
            '-----------------------------
            Dim strMethodParms As String = ""
            If (HyperlinkID Is Nothing) Then
                strMethodParms &= "HyperlinkID: NOTHING" & Environment.NewLine
            Else
                strMethodParms &= "HyperlinkID: " & HyperlinkID & Environment.NewLine
            End If
            strMethodParms &= "userRole: " & userRole.ToString & Environment.NewLine
            UGNErrorTrapping.InsertErrorLog(ex.Message, _
                strCallingPage, strMyMethodInfo & " -> " & strMethodParms)
        End Try
        Return bln
    End Function ' HasRoleInDatabase

#End Region ' Role Based Security Methods

#Region "Misc Methods"

    Public Shared Function QueryStringValue(ByVal param As QueryStringParam) As Integer
        ''*******
        '' Returns the integer value of the QueryString parameter, or 0 if not found.
        ''*******
        Dim int As Integer = 0
        Try
            Dim str As String = Current.Request.QueryString(param.ToString)
            Dim blnResult As Boolean = Integer.TryParse(str, int)
        Catch ex As Exception
            ' DO NOTHING
        End Try
        Return int
    End Function ' QueryStringValue

#End Region ' Misc Methods

End Class ' RnDModule
